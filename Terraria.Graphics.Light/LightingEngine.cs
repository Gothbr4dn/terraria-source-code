using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using ReLogic.Threading;

namespace Terraria.Graphics.Light
{
	public class LightingEngine : ILightingEngine
	{
		private enum EngineState
		{
			MinimapUpdate,
			ExportMetrics,
			Scan,
			Blur,
			Max
		}

		private struct PerFrameLight
		{
			public readonly Point Position;

			public readonly Vector3 Color;

			public PerFrameLight(Point position, Vector3 color)
			{
				Position = position;
				Color = color;
			}
		}

		public const int AREA_PADDING = 28;

		private const int NON_VISIBLE_PADDING = 18;

		private readonly List<PerFrameLight> _perFrameLights = new List<PerFrameLight>();

		private TileLightScanner _tileScanner = new TileLightScanner();

		private LightMap _activeLightMap = new LightMap();

		private Rectangle _activeProcessedArea;

		private LightMap _workingLightMap = new LightMap();

		private Rectangle _workingProcessedArea;

		private readonly Stopwatch _timer = new Stopwatch();

		private EngineState _state;

		public void AddLight(int x, int y, Vector3 color)
		{
			_perFrameLights.Add(new PerFrameLight(new Point(x, y), color));
		}

		public void Clear()
		{
			_activeLightMap.Clear();
			_workingLightMap.Clear();
			_perFrameLights.Clear();
		}

		public Vector3 GetColor(int x, int y)
		{
			if (!_activeProcessedArea.Contains(x, y))
			{
				return Vector3.Zero;
			}
			x -= _activeProcessedArea.X;
			y -= _activeProcessedArea.Y;
			return _activeLightMap[x, y];
		}

		public void ProcessArea(Rectangle area)
		{
			Main.renderCount = (Main.renderCount + 1) % 4;
			_timer.Start();
			TimeLogger.LightingTime(0, 0.0);
			switch (_state)
			{
			case EngineState.MinimapUpdate:
				if (Main.mapDelay > 0)
				{
					Main.mapDelay--;
				}
				else
				{
					ExportToMiniMap();
				}
				TimeLogger.LightingTime(1, _timer.Elapsed.TotalMilliseconds);
				break;
			case EngineState.ExportMetrics:
				area.Inflate(28, 28);
				Main.SceneMetrics.ScanAndExportToMain(new SceneMetricsScanSettings
				{
					VisualScanArea = area,
					BiomeScanCenterPositionInWorld = Main.LocalPlayer.Center,
					ScanOreFinderData = Main.LocalPlayer.accOreFinder
				});
				TimeLogger.LightingTime(2, _timer.Elapsed.TotalMilliseconds);
				break;
			case EngineState.Scan:
				ProcessScan(area);
				TimeLogger.LightingTime(3, _timer.Elapsed.TotalMilliseconds);
				break;
			case EngineState.Blur:
				ProcessBlur();
				Present();
				TimeLogger.LightingTime(4, _timer.Elapsed.TotalMilliseconds);
				break;
			}
			IncrementState();
			_timer.Reset();
		}

		private void IncrementState()
		{
			_state = (EngineState)((int)(_state + 1) % 4);
		}

		private void ProcessScan(Rectangle area)
		{
			area.Inflate(28, 28);
			_workingProcessedArea = area;
			_workingLightMap.SetSize(area.Width, area.Height);
			_workingLightMap.NonVisiblePadding = 18;
			_tileScanner.Update();
			_tileScanner.ExportTo(area, _workingLightMap, new TileLightScannerOptions
			{
				DrawInvisibleWalls = Main.ShouldShowInvisibleWalls()
			});
		}

		private void ProcessBlur()
		{
			UpdateLightDecay();
			ApplyPerFrameLights();
			_workingLightMap.Blur();
		}

		private void Present()
		{
			Utils.Swap(ref _activeLightMap, ref _workingLightMap);
			Utils.Swap(ref _activeProcessedArea, ref _workingProcessedArea);
		}

		private void UpdateLightDecay()
		{
			LightMap workingLightMap = _workingLightMap;
			workingLightMap.LightDecayThroughAir = 0.91f;
			workingLightMap.LightDecayThroughSolid = 0.56f;
			workingLightMap.LightDecayThroughHoney = new Vector3(0.75f, 0.7f, 0.6f) * 0.91f;
			switch (Main.waterStyle)
			{
			case 0:
			case 1:
			case 7:
			case 8:
				workingLightMap.LightDecayThroughWater = new Vector3(0.88f, 0.96f, 1.015f) * 0.91f;
				break;
			case 2:
				workingLightMap.LightDecayThroughWater = new Vector3(0.94f, 0.85f, 1.01f) * 0.91f;
				break;
			case 3:
				workingLightMap.LightDecayThroughWater = new Vector3(0.84f, 0.95f, 1.015f) * 0.91f;
				break;
			case 4:
				workingLightMap.LightDecayThroughWater = new Vector3(0.9f, 0.86f, 1.01f) * 0.91f;
				break;
			case 5:
				workingLightMap.LightDecayThroughWater = new Vector3(0.84f, 0.99f, 1.01f) * 0.91f;
				break;
			case 6:
				workingLightMap.LightDecayThroughWater = new Vector3(0.83f, 0.93f, 0.98f) * 0.91f;
				break;
			case 9:
				workingLightMap.LightDecayThroughWater = new Vector3(1f, 0.88f, 0.84f) * 0.91f;
				break;
			case 10:
				workingLightMap.LightDecayThroughWater = new Vector3(0.83f, 1f, 1f) * 0.91f;
				break;
			case 12:
				workingLightMap.LightDecayThroughWater = new Vector3(0.95f, 0.98f, 0.85f) * 0.91f;
				break;
			case 13:
				workingLightMap.LightDecayThroughWater = new Vector3(0.9f, 1f, 1.02f) * 0.91f;
				break;
			}
			if (Main.player[Main.myPlayer].nightVision)
			{
				workingLightMap.LightDecayThroughAir *= 1.03f;
				workingLightMap.LightDecayThroughSolid *= 1.03f;
			}
			if (Main.player[Main.myPlayer].blind)
			{
				workingLightMap.LightDecayThroughAir *= 0.95f;
				workingLightMap.LightDecayThroughSolid *= 0.95f;
			}
			if (Main.player[Main.myPlayer].blackout)
			{
				workingLightMap.LightDecayThroughAir *= 0.85f;
				workingLightMap.LightDecayThroughSolid *= 0.85f;
			}
			if (Main.player[Main.myPlayer].headcovered)
			{
				workingLightMap.LightDecayThroughAir *= 0.85f;
				workingLightMap.LightDecayThroughSolid *= 0.85f;
			}
			workingLightMap.LightDecayThroughAir *= Player.airLightDecay;
			workingLightMap.LightDecayThroughSolid *= Player.solidLightDecay;
		}

		private void ApplyPerFrameLights()
		{
			for (int i = 0; i < _perFrameLights.Count; i++)
			{
				Point position = _perFrameLights[i].Position;
				if (_workingProcessedArea.Contains(position))
				{
					Vector3 value = _perFrameLights[i].Color;
					Vector3 value2 = _workingLightMap[position.X - _workingProcessedArea.X, position.Y - _workingProcessedArea.Y];
					Vector3.Max(ref value2, ref value, out value);
					_workingLightMap[position.X - _workingProcessedArea.X, position.Y - _workingProcessedArea.Y] = value;
				}
			}
			_perFrameLights.Clear();
		}

		public void Rebuild()
		{
			_activeProcessedArea = Rectangle.Empty;
			_workingProcessedArea = Rectangle.Empty;
			_state = EngineState.MinimapUpdate;
			_activeLightMap = new LightMap();
			_workingLightMap = new LightMap();
		}

		private void ExportToMiniMap()
		{
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Expected O, but got Unknown
			if (!Main.mapEnabled || _activeProcessedArea.Width <= 0 || _activeProcessedArea.Height <= 0)
			{
				return;
			}
			Rectangle area = new Rectangle(_activeProcessedArea.X + 28, _activeProcessedArea.Y + 28, _activeProcessedArea.Width - 56, _activeProcessedArea.Height - 56);
			Rectangle value = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
			value.Inflate(-40, -40);
			area = Rectangle.Intersect(area, value);
			Main.mapMinX = area.Left;
			Main.mapMinY = area.Top;
			Main.mapMaxX = area.Right;
			Main.mapMaxY = area.Bottom;
			FastParallel.For(area.Left, area.Right, (ParallelForAction)delegate(int start, int end, object context)
			{
				for (int i = start; i < end; i++)
				{
					for (int j = area.Top; j < area.Bottom; j++)
					{
						Vector3 vector = _activeLightMap[i - _activeProcessedArea.X, j - _activeProcessedArea.Y];
						float num = Math.Max(Math.Max(vector.X, vector.Y), vector.Z);
						byte light = (byte)Math.Min(255, (int)(num * 255f));
						Main.Map.UpdateLighting(i, j, light);
					}
				}
			}, (object)null);
			Main.updateMap = true;
		}
	}
}
