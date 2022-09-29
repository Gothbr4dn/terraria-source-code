using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	public class GraniteBiome : MicroBiome
	{
		private struct Magma
		{
			public readonly double Pressure;

			public readonly double Resistance;

			public readonly bool IsActive;

			private Magma(double pressure, double resistance, bool active)
			{
				Pressure = pressure;
				Resistance = resistance;
				IsActive = active;
			}

			public Magma ToFlow()
			{
				return new Magma(Pressure, Resistance, active: true);
			}

			public static Magma CreateFlow(double pressure, double resistance = 0.0)
			{
				return new Magma(pressure, resistance, active: true);
			}

			public static Magma CreateEmpty(double resistance = 0.0)
			{
				return new Magma(0.0, resistance, active: false);
			}
		}

		private const int MAX_MAGMA_ITERATIONS = 300;

		private Magma[,] _sourceMagmaMap = new Magma[200, 200];

		private Magma[,] _targetMagmaMap = new Magma[200, 200];

		private static Vector2D[] _normalisedVectors = (Vector2D[])(object)new Vector2D[9]
		{
			Vector2D.Normalize(new Vector2D(-1.0, -1.0)),
			Vector2D.Normalize(new Vector2D(-1.0, 0.0)),
			Vector2D.Normalize(new Vector2D(-1.0, 1.0)),
			Vector2D.Normalize(new Vector2D(0.0, -1.0)),
			new Vector2D(0.0, 0.0),
			Vector2D.Normalize(new Vector2D(0.0, 1.0)),
			Vector2D.Normalize(new Vector2D(1.0, -1.0)),
			Vector2D.Normalize(new Vector2D(1.0, 0.0)),
			Vector2D.Normalize(new Vector2D(1.0, 1.0))
		};

		public static bool CanPlace(Point origin, StructureMap structures)
		{
			if (WorldGen.BiomeTileCheck(origin.X, origin.Y))
			{
				return false;
			}
			return !GenBase._tiles[origin.X, origin.Y].active();
		}

		public override bool Place(Point origin, StructureMap structures)
		{
			if (GenBase._tiles[origin.X, origin.Y].active())
			{
				return false;
			}
			origin.X -= _sourceMagmaMap.GetLength(0) / 2;
			origin.Y -= _sourceMagmaMap.GetLength(1) / 2;
			BuildMagmaMap(origin);
			SimulatePressure(out var effectedMapArea);
			PlaceGranite(origin, effectedMapArea);
			CleanupTiles(origin, effectedMapArea);
			PlaceDecorations(origin, effectedMapArea);
			structures.AddStructure(effectedMapArea, 8);
			return true;
		}

		private void BuildMagmaMap(Point tileOrigin)
		{
			_sourceMagmaMap = new Magma[200, 200];
			_targetMagmaMap = new Magma[200, 200];
			for (int i = 0; i < _sourceMagmaMap.GetLength(0); i++)
			{
				for (int j = 0; j < _sourceMagmaMap.GetLength(1); j++)
				{
					int i2 = i + tileOrigin.X;
					int j2 = j + tileOrigin.Y;
					_sourceMagmaMap[i, j] = Magma.CreateEmpty((!WorldGen.SolidTile(i2, j2)) ? 1 : 4);
					_targetMagmaMap[i, j] = _sourceMagmaMap[i, j];
				}
			}
		}

		private void SimulatePressure(out Rectangle effectedMapArea)
		{
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			int length = _sourceMagmaMap.GetLength(0);
			int length2 = _sourceMagmaMap.GetLength(1);
			int num = length / 2;
			int num2 = length2 / 2;
			int num3 = num;
			int num4 = num3;
			int num5 = num2;
			int num6 = num5;
			for (int i = 0; i < 300; i++)
			{
				for (int j = num3; j <= num4; j++)
				{
					for (int k = num5; k <= num6; k++)
					{
						Magma magma = _sourceMagmaMap[j, k];
						if (!magma.IsActive)
						{
							continue;
						}
						double num7 = 0.0;
						Vector2D val = Vector2D.get_Zero();
						for (int l = -1; l <= 1; l++)
						{
							for (int m = -1; m <= 1; m++)
							{
								if (l == 0 && m == 0)
								{
									continue;
								}
								Vector2D val2 = _normalisedVectors[(l + 1) * 3 + (m + 1)];
								Magma magma2 = _sourceMagmaMap[j + l, k + m];
								if (magma.Pressure > 0.01 && !magma2.IsActive)
								{
									if (l == -1)
									{
										num3 = Utils.Clamp(j + l, 1, num3);
									}
									else
									{
										num4 = Utils.Clamp(j + l, num4, length - 2);
									}
									if (m == -1)
									{
										num5 = Utils.Clamp(k + m, 1, num5);
									}
									else
									{
										num6 = Utils.Clamp(k + m, num6, length2 - 2);
									}
									_targetMagmaMap[j + l, k + m] = magma2.ToFlow();
								}
								double pressure = magma2.Pressure;
								num7 += pressure;
								val += pressure * val2;
							}
						}
						num7 /= 8.0;
						if (num7 > magma.Resistance)
						{
							double num8 = ((Vector2D)(ref val)).Length() / 8.0;
							double val3 = Math.Max(num7 - num8 - magma.Pressure, 0.0) + num8 + magma.Pressure * 0.875 - magma.Resistance;
							val3 = Math.Max(0.0, val3);
							_targetMagmaMap[j, k] = Magma.CreateFlow(val3, Math.Max(0.0, magma.Resistance - val3 * 0.02));
						}
					}
				}
				if (i < 2)
				{
					_targetMagmaMap[num, num2] = Magma.CreateFlow(25.0);
				}
				Utils.Swap(ref _sourceMagmaMap, ref _targetMagmaMap);
			}
			effectedMapArea = new Rectangle(num3, num5, num4 - num3 + 1, num6 - num5 + 1);
		}

		private bool ShouldUseLava(Point tileOrigin)
		{
			int length = _sourceMagmaMap.GetLength(0);
			int length2 = _sourceMagmaMap.GetLength(1);
			int num = length / 2;
			int num2 = length2 / 2;
			if (tileOrigin.Y + num2 <= GenVars.lavaLine - 30)
			{
				return false;
			}
			for (int i = -50; i < 50; i++)
			{
				for (int j = -50; j < 50; j++)
				{
					if (GenBase._tiles[tileOrigin.X + num + i, tileOrigin.Y + num2 + j].active())
					{
						ushort type = GenBase._tiles[tileOrigin.X + num + i, tileOrigin.Y + num2 + j].type;
						if (type == 147 || (uint)(type - 161) <= 2u || type == 200)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		private void PlaceGranite(Point tileOrigin, Rectangle magmaMapArea)
		{
			bool flag = ShouldUseLava(tileOrigin);
			ushort type = 368;
			ushort wall = 180;
			if (WorldGen.drunkWorldGen)
			{
				type = 367;
				wall = 178;
			}
			for (int i = magmaMapArea.Left; i < magmaMapArea.Right; i++)
			{
				for (int j = magmaMapArea.Top; j < magmaMapArea.Bottom; j++)
				{
					Magma magma = _sourceMagmaMap[i, j];
					if (!magma.IsActive)
					{
						continue;
					}
					Tile tile = GenBase._tiles[tileOrigin.X + i, tileOrigin.Y + j];
					double num = Math.Sin((double)(tileOrigin.Y + j) * 0.4) * 0.7 + 1.2;
					double num2 = 0.2 + 0.5 / Math.Sqrt(Math.Max(0.0, magma.Pressure - magma.Resistance));
					if (Math.Max(1.0 - Math.Max(0.0, num * num2), magma.Pressure / 15.0) > 0.35 + (WorldGen.SolidTile(tileOrigin.X + i, tileOrigin.Y + j) ? 0.0 : 0.5))
					{
						if (TileID.Sets.Ore[tile.type])
						{
							tile.ResetToType(tile.type);
						}
						else
						{
							tile.ResetToType(type);
						}
						tile.wall = wall;
					}
					else if (magma.Resistance < 0.01)
					{
						WorldUtils.ClearTile(tileOrigin.X + i, tileOrigin.Y + j);
						tile.wall = wall;
					}
					if (tile.liquid > 0 && flag)
					{
						tile.liquidType(1);
					}
				}
			}
		}

		private void CleanupTiles(Point tileOrigin, Rectangle magmaMapArea)
		{
			ushort wall = 180;
			if (WorldGen.drunkWorldGen)
			{
				wall = 178;
			}
			List<Point16> list = new List<Point16>();
			for (int i = magmaMapArea.Left; i < magmaMapArea.Right; i++)
			{
				for (int j = magmaMapArea.Top; j < magmaMapArea.Bottom; j++)
				{
					if (!_sourceMagmaMap[i, j].IsActive)
					{
						continue;
					}
					int num = 0;
					int num2 = i + tileOrigin.X;
					int num3 = j + tileOrigin.Y;
					if (!WorldGen.SolidTile(num2, num3))
					{
						continue;
					}
					for (int k = -1; k <= 1; k++)
					{
						for (int l = -1; l <= 1; l++)
						{
							if (WorldGen.SolidTile(num2 + k, num3 + l))
							{
								num++;
							}
						}
					}
					if (num < 3)
					{
						list.Add(new Point16(num2, num3));
					}
				}
			}
			foreach (Point16 item in list)
			{
				int x = item.X;
				int y = item.Y;
				WorldUtils.ClearTile(x, y, frameNeighbors: true);
				GenBase._tiles[x, y].wall = wall;
			}
			list.Clear();
		}

		private void PlaceDecorations(Point tileOrigin, Rectangle magmaMapArea)
		{
			FastRandom fastRandom = new FastRandom(Main.ActiveWorldFileData.Seed).WithModifier(65440uL);
			for (int i = magmaMapArea.Left; i < magmaMapArea.Right; i++)
			{
				for (int j = magmaMapArea.Top; j < magmaMapArea.Bottom; j++)
				{
					Magma magma = _sourceMagmaMap[i, j];
					int num = i + tileOrigin.X;
					int num2 = j + tileOrigin.Y;
					if (!magma.IsActive)
					{
						continue;
					}
					WorldUtils.TileFrame(num, num2);
					WorldGen.SquareWallFrame(num, num2);
					FastRandom fastRandom2 = fastRandom.WithModifier(num, num2);
					if (fastRandom2.Next(8) == 0 && GenBase._tiles[num, num2].active())
					{
						if (!GenBase._tiles[num, num2 + 1].active())
						{
							WorldGen.PlaceUncheckedStalactite(num, num2 + 1, fastRandom2.Next(2) == 0, fastRandom2.Next(3), spiders: false);
						}
						if (!GenBase._tiles[num, num2 - 1].active())
						{
							WorldGen.PlaceUncheckedStalactite(num, num2 - 1, fastRandom2.Next(2) == 0, fastRandom2.Next(3), spiders: false);
						}
					}
					if (fastRandom2.Next(2) == 0)
					{
						Tile.SmoothSlope(num, num2);
					}
				}
			}
		}
	}
}
