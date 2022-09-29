using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	public class TrackGenerator
	{
		private enum TrackPlacementState
		{
			Available,
			Obstructed,
			Invalid
		}

		private enum TrackSlope : sbyte
		{
			Up = -1,
			Straight,
			Down
		}

		private enum TrackMode : byte
		{
			Normal,
			Tunnel
		}

		[DebuggerDisplay("X = {X}, Y = {Y}, Slope = {Slope}")]
		private struct TrackHistory
		{
			public short X;

			public short Y;

			public TrackSlope Slope;

			public TrackMode Mode;

			public TrackHistory(int x, int y, TrackSlope slope)
			{
				X = (short)x;
				Y = (short)y;
				Slope = slope;
				Mode = TrackMode.Normal;
			}
		}

		private static readonly ushort[] InvalidWalls = new ushort[20]
		{
			7, 94, 95, 8, 98, 99, 9, 96, 97, 3,
			83, 68, 62, 78, 87, 86, 42, 74, 27, 149
		};

		private static readonly ushort[] InvalidTiles = new ushort[24]
		{
			383, 384, 15, 304, 30, 321, 245, 246, 240, 241,
			242, 16, 34, 158, 377, 94, 10, 19, 86, 219,
			484, 190, 664, 665
		};

		private readonly TrackHistory[] _history = new TrackHistory[4096];

		private readonly TrackHistory[] _rewriteHistory = new TrackHistory[25];

		private int _xDirection;

		private int _length;

		private int playerHeight = 6;

		public bool Place(Point origin, int minLength, int maxLength)
		{
			if (!FindSuitableOrigin(ref origin))
			{
				return false;
			}
			CreateTrackStart(origin);
			if (!FindPath(minLength, maxLength))
			{
				return false;
			}
			PlacePath();
			return true;
		}

		private void PlacePath()
		{
			bool[] array = new bool[_length];
			for (int i = 0; i < _length; i++)
			{
				if (WorldGen.genRand.Next(7) == 0)
				{
					playerHeight = WorldGen.genRand.Next(5, 9);
				}
				for (int j = 0; j < playerHeight; j++)
				{
					if (Main.tile[_history[i].X, _history[i].Y - j - 1].wall == 244)
					{
						Main.tile[_history[i].X, _history[i].Y - j - 1].wall = 0;
					}
					if (Main.tile[_history[i].X, _history[i].Y - j].wall == 244)
					{
						Main.tile[_history[i].X, _history[i].Y - j].wall = 0;
					}
					if (Main.tile[_history[i].X, _history[i].Y - j + 1].wall == 244)
					{
						Main.tile[_history[i].X, _history[i].Y - j + 1].wall = 0;
					}
					if (Main.tile[_history[i].X, _history[i].Y - j].type == 135)
					{
						array[i] = true;
					}
					WorldGen.KillTile(_history[i].X, _history[i].Y - j, fail: false, effectOnly: false, noItem: true);
				}
			}
			for (int k = 0; k < _length; k++)
			{
				if (WorldGen.genRand.Next(7) == 0)
				{
					playerHeight = WorldGen.genRand.Next(5, 9);
				}
				TrackHistory trackHistory = _history[k];
				Tile.SmoothSlope(trackHistory.X, trackHistory.Y + 1);
				Tile.SmoothSlope(trackHistory.X, trackHistory.Y - playerHeight);
				bool wire = Main.tile[trackHistory.X, trackHistory.Y].wire();
				if (array[k] && k < _length && k > 0 && _history[k - 1].Y == trackHistory.Y && _history[k + 1].Y == trackHistory.Y)
				{
					Main.tile[trackHistory.X, trackHistory.Y].ClearEverything();
					WorldGen.PlaceTile(trackHistory.X, trackHistory.Y, 314, mute: false, forced: true, -1, 1);
				}
				else
				{
					Main.tile[trackHistory.X, trackHistory.Y].ResetToType(314);
				}
				Main.tile[trackHistory.X, trackHistory.Y].wire(wire);
				if (k == 0)
				{
					continue;
				}
				for (int l = 0; l < 8; l++)
				{
					WorldUtils.TileFrame(_history[k - 1].X, _history[k - 1].Y - l, frameNeighbors: true);
				}
				if (k == _length - 1)
				{
					for (int m = 0; m < playerHeight; m++)
					{
						WorldUtils.TileFrame(trackHistory.X, trackHistory.Y - m, frameNeighbors: true);
					}
				}
			}
		}

		private void CreateTrackStart(Point origin)
		{
			_xDirection = ((origin.X <= Main.maxTilesX / 2) ? 1 : (-1));
			_length = 1;
			for (int i = 0; i < _history.Length; i++)
			{
				_history[i] = new TrackHistory(origin.X + i * _xDirection, origin.Y + i, TrackSlope.Down);
			}
		}

		private bool FindPath(int minLength, int maxLength)
		{
			int length = _length;
			while (_length < _history.Length - 100)
			{
				TrackSlope slope = ((_history[_length - 1].Slope != TrackSlope.Up) ? TrackSlope.Down : TrackSlope.Straight);
				AppendToHistory(slope);
				TrackPlacementState trackPlacementState = TryRewriteHistoryToAvoidTiles();
				if (trackPlacementState == TrackPlacementState.Invalid)
				{
					break;
				}
				length = _length;
				TrackPlacementState trackPlacementState2 = trackPlacementState;
				while (trackPlacementState2 != 0)
				{
					trackPlacementState2 = CreateTunnel();
					if (trackPlacementState2 == TrackPlacementState.Invalid)
					{
						break;
					}
					length = _length;
				}
				if (_length >= maxLength)
				{
					break;
				}
			}
			_length = Math.Min(maxLength, length);
			if (_length < minLength)
			{
				return false;
			}
			SmoothTrack();
			return GetHistorySegmentPlacementState(0, _length) != TrackPlacementState.Invalid;
		}

		private TrackPlacementState CreateTunnel()
		{
			TrackSlope trackSlope = TrackSlope.Straight;
			int num = 10;
			TrackPlacementState trackPlacementState = TrackPlacementState.Invalid;
			int x = _history[_length - 1].X;
			int y = _history[_length - 1].Y;
			for (TrackSlope trackSlope2 = TrackSlope.Up; trackSlope2 <= TrackSlope.Down; trackSlope2++)
			{
				TrackPlacementState trackPlacementState2 = TrackPlacementState.Invalid;
				for (int i = 1; i < num; i++)
				{
					trackPlacementState2 = CalculateStateForLocation(x + i * _xDirection, y + i * (int)trackSlope2);
					switch (trackPlacementState2)
					{
					default:
						trackSlope = trackSlope2;
						num = i;
						trackPlacementState = trackPlacementState2;
						break;
					case TrackPlacementState.Obstructed:
						continue;
					case TrackPlacementState.Invalid:
						break;
					}
					break;
				}
				if (trackPlacementState != 0 && trackPlacementState2 == TrackPlacementState.Obstructed && (trackPlacementState != TrackPlacementState.Obstructed || trackSlope != 0))
				{
					trackSlope = trackSlope2;
					num = 10;
					trackPlacementState = trackPlacementState2;
				}
			}
			if (_length == 0 || !CanSlopesTouch(_history[_length - 1].Slope, trackSlope))
			{
				RewriteSlopeDirection(_length - 1, TrackSlope.Straight);
			}
			_history[_length - 1].Mode = TrackMode.Tunnel;
			for (int j = 1; j < num; j++)
			{
				AppendToHistory(trackSlope, TrackMode.Tunnel);
			}
			return trackPlacementState;
		}

		private void AppendToHistory(TrackSlope slope, TrackMode mode = TrackMode.Normal)
		{
			_history[_length] = new TrackHistory(_history[_length - 1].X + _xDirection, (int)_history[_length - 1].Y + (int)slope, slope);
			_history[_length].Mode = mode;
			_length++;
		}

		private TrackPlacementState TryRewriteHistoryToAvoidTiles()
		{
			int num = _length - 1;
			int num2 = Math.Min(_length, _rewriteHistory.Length);
			for (int i = 0; i < num2; i++)
			{
				_rewriteHistory[i] = _history[num - i];
			}
			while (num >= _length - num2)
			{
				if (_history[num].Slope == TrackSlope.Down)
				{
					TrackPlacementState historySegmentPlacementState = GetHistorySegmentPlacementState(num, _length - num);
					if (historySegmentPlacementState == TrackPlacementState.Available)
					{
						return historySegmentPlacementState;
					}
					RewriteSlopeDirection(num, TrackSlope.Straight);
				}
				num--;
			}
			if (GetHistorySegmentPlacementState(num + 1, _length - (num + 1)) == TrackPlacementState.Available)
			{
				return TrackPlacementState.Available;
			}
			for (num = _length - 1; num >= _length - num2 + 1; num--)
			{
				if (_history[num].Slope == TrackSlope.Straight)
				{
					TrackPlacementState historySegmentPlacementState2 = GetHistorySegmentPlacementState(_length - num2, num2);
					if (historySegmentPlacementState2 == TrackPlacementState.Available)
					{
						return historySegmentPlacementState2;
					}
					RewriteSlopeDirection(num, TrackSlope.Up);
				}
			}
			for (int j = 0; j < num2; j++)
			{
				_history[_length - 1 - j] = _rewriteHistory[j];
			}
			RewriteSlopeDirection(_length - 1, TrackSlope.Straight);
			return GetHistorySegmentPlacementState(num + 1, _length - (num + 1));
		}

		private void RewriteSlopeDirection(int index, TrackSlope slope)
		{
			int num = slope - _history[index].Slope;
			_history[index].Slope = slope;
			for (int i = index; i < _length; i++)
			{
				_history[i].Y += (short)num;
			}
		}

		private TrackPlacementState GetHistorySegmentPlacementState(int startIndex, int length)
		{
			TrackPlacementState result = TrackPlacementState.Available;
			for (int i = startIndex; i < startIndex + length; i++)
			{
				TrackPlacementState trackPlacementState = CalculateStateForLocation(_history[i].X, _history[i].Y);
				switch (trackPlacementState)
				{
				case TrackPlacementState.Invalid:
					return trackPlacementState;
				case TrackPlacementState.Obstructed:
					if (_history[i].Mode != TrackMode.Tunnel)
					{
						result = trackPlacementState;
					}
					break;
				}
			}
			return result;
		}

		private void SmoothTrack()
		{
			int num = _length - 1;
			bool flag = false;
			for (int num2 = _length - 1; num2 >= 0; num2--)
			{
				if (flag)
				{
					num = Math.Min(num2 + 15, num);
					if (_history[num2].Y >= _history[num].Y)
					{
						for (int i = num2 + 1; _history[i].Y > _history[num2].Y; i++)
						{
							_history[i].Y = _history[num2].Y;
							_history[i].Slope = TrackSlope.Straight;
						}
						if (_history[num2].Y == _history[num].Y)
						{
							flag = false;
						}
					}
				}
				else if (_history[num2].Y > _history[num].Y)
				{
					flag = true;
				}
				else
				{
					num = num2;
				}
			}
		}

		private static bool CanSlopesTouch(TrackSlope leftSlope, TrackSlope rightSlope)
		{
			if (leftSlope != rightSlope && leftSlope != 0)
			{
				return rightSlope == TrackSlope.Straight;
			}
			return true;
		}

		private static bool FindSuitableOrigin(ref Point origin)
		{
			TrackPlacementState trackPlacementState;
			while ((trackPlacementState = CalculateStateForLocation(origin.X, origin.Y)) != TrackPlacementState.Obstructed)
			{
				origin.Y++;
				if (trackPlacementState == TrackPlacementState.Invalid)
				{
					return false;
				}
			}
			origin.Y--;
			return CalculateStateForLocation(origin.X, origin.Y) == TrackPlacementState.Available;
		}

		private static TrackPlacementState CalculateStateForLocation(int x, int y)
		{
			for (int i = 0; i < 6; i++)
			{
				if (IsLocationInvalid(x, y - i))
				{
					return TrackPlacementState.Invalid;
				}
			}
			for (int j = 0; j < 6; j++)
			{
				if (IsMinecartTrack(x, y + j))
				{
					return TrackPlacementState.Invalid;
				}
			}
			for (int k = 0; k < 6; k++)
			{
				if (WorldGen.SolidTile(x, y - k))
				{
					return TrackPlacementState.Obstructed;
				}
			}
			if (WorldGen.IsTileNearby(x, y, 314, 30))
			{
				return TrackPlacementState.Invalid;
			}
			return TrackPlacementState.Available;
		}

		private static bool IsMinecartTrack(int x, int y)
		{
			if (Main.tile[x, y].active())
			{
				return Main.tile[x, y].type == 314;
			}
			return false;
		}

		private static bool IsLocationInvalid(int x, int y)
		{
			if (y > Main.UnderworldLayer || x < 5 || y < (int)Main.worldSurface || x > Main.maxTilesX - 5)
			{
				return true;
			}
			if (Math.Abs((double)x - GenVars.shimmerPosition.X) < (double)(WorldGen.shimmerSafetyDistance / 2) && Math.Abs((double)y - GenVars.shimmerPosition.Y) < (double)(WorldGen.shimmerSafetyDistance / 2))
			{
				return true;
			}
			if (WorldGen.oceanDepths(x, y))
			{
				return true;
			}
			ushort wall = Main.tile[x, y].wall;
			for (int i = 0; i < InvalidWalls.Length; i++)
			{
				if (wall == InvalidWalls[i] && (!WorldGen.notTheBees || wall != 108))
				{
					return true;
				}
			}
			ushort type = Main.tile[x, y].type;
			for (int j = 0; j < InvalidTiles.Length; j++)
			{
				if (type == InvalidTiles[j])
				{
					return true;
				}
			}
			for (int k = -1; k <= 1; k++)
			{
				if (Main.tile[x + k, y].active() && (Main.tile[x + k, y].type == 314 || !TileID.Sets.GeneralPlacementTiles[Main.tile[x + k, y].type]) && (!WorldGen.notTheBees || Main.tile[x + k, y].type != 225))
				{
					return true;
				}
			}
			return false;
		}

		[Conditional("DEBUG")]
		private void DrawPause()
		{
		}
	}
}
