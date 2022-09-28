using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	public class HiveBiome : MicroBiome
	{
		public override bool Place(Point origin, StructureMap structures)
		{
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			if (!structures.CanPlace(new Rectangle(origin.X - 50, origin.Y - 50, 100, 100)))
			{
				return false;
			}
			if (TooCloseToImportantLocations(origin))
			{
				return false;
			}
			Ref<int> @ref = new Ref<int>(0);
			Ref<int> ref2 = new Ref<int>(0);
			Ref<int> ref3 = new Ref<int>(0);
			WorldUtils.Gen(origin, new Shapes.Circle(15), Actions.Chain(new Modifiers.IsSolid(), new Actions.Scanner(@ref), new Modifiers.OnlyTiles(60, 59), new Actions.Scanner(ref2), new Modifiers.OnlyTiles(60), new Actions.Scanner(ref3)));
			if ((double)ref2.Value / (double)@ref.Value < 0.75 || ref3.Value < 2)
			{
				return false;
			}
			int num = 0;
			int[] array = new int[1000];
			int[] array2 = new int[1000];
			Vector2D val = origin.ToVector2D();
			int num2 = WorldGen.genRand.Next(2, 5);
			if (WorldGen.drunkWorldGen)
			{
				num2 += WorldGen.genRand.Next(7, 10);
			}
			else if (WorldGen.remixWorldGen)
			{
				num2 += WorldGen.genRand.Next(2, 5);
			}
			for (int i = 0; i < num2; i++)
			{
				Vector2D val2 = val;
				int num3 = WorldGen.genRand.Next(2, 5);
				for (int j = 0; j < num3; j++)
				{
					val2 = CreateHiveTunnel((int)val.X, (int)val.Y, WorldGen.genRand);
				}
				val = val2;
				array[num] = (int)val.X;
				array2[num] = (int)val.Y;
				num++;
			}
			FrameOutAllHiveContents(origin, 50);
			for (int k = 0; k < num; k++)
			{
				int num4 = array[k];
				int y = array2[k];
				int num5 = 1;
				if (WorldGen.genRand.Next(2) == 0)
				{
					num5 = -1;
				}
				bool flag = false;
				while (WorldGen.InWorld(num4, y, 10) && BadSpotForHoneyFall(num4, y))
				{
					num4 += num5;
					if (Math.Abs(num4 - array[k]) > 50)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					num4 += num5;
					if (!SpotActuallyNotInHive(num4, y))
					{
						CreateBlockedHoneyCube(num4, y);
						CreateDentForHoneyFall(num4, y, num5);
					}
				}
			}
			CreateStandForLarva(val);
			if (WorldGen.drunkWorldGen)
			{
				for (int l = 0; l < 1000; l++)
				{
					Vector2D val3 = val;
					val3.X += WorldGen.genRand.Next(-50, 51);
					val3.Y += WorldGen.genRand.Next(-50, 51);
					if (WorldGen.InWorld((int)val3.X, (int)val3.Y) && Vector2D.Distance(val, val3) > 10.0 && !Main.tile[(int)val3.X, (int)val3.Y].active() && Main.tile[(int)val3.X, (int)val3.Y].wall == 86)
					{
						CreateStandForLarva(val3);
						break;
					}
				}
			}
			structures.AddProtectedStructure(new Rectangle(origin.X - 50, origin.Y - 50, 100, 100), 5);
			return true;
		}

		private static void FrameOutAllHiveContents(Point origin, int squareHalfWidth)
		{
			int num = Math.Max(10, origin.X - squareHalfWidth);
			int num2 = Math.Min(Main.maxTilesX - 10, origin.X + squareHalfWidth);
			int num3 = Math.Max(10, origin.Y - squareHalfWidth);
			int num4 = Math.Min(Main.maxTilesY - 10, origin.Y + squareHalfWidth);
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && tile.type == 225)
					{
						WorldGen.SquareTileFrame(i, j);
					}
					if (tile.wall == 86)
					{
						WorldGen.SquareWallFrame(i, j);
					}
				}
			}
		}

		private static Vector2D CreateHiveTunnel(int i, int j, UnifiedRandom random)
		{
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_052f: Unknown result type (might be due to invalid IL or missing references)
			double num = random.Next(12, 21);
			double num2 = random.Next(10, 21);
			if (WorldGen.drunkWorldGen)
			{
				num = random.Next(8, 26);
				num2 = random.Next(10, 41);
				double num3 = (double)Main.maxTilesX / 4200.0;
				num3 = (num3 + 1.0) / 2.0;
				num *= num3;
				num2 *= num3;
			}
			else if (WorldGen.remixWorldGen)
			{
				num += (double)random.Next(3);
			}
			double num4 = num;
			Vector2D val = default(Vector2D);
			val.X = i;
			val.Y = j;
			Vector2D val2 = default(Vector2D);
			val2.X = (double)random.Next(-10, 11) * 0.2;
			val2.Y = (double)random.Next(-10, 11) * 0.2;
			while (num > 0.0 && num2 > 0.0)
			{
				if (val.Y > (double)(Main.maxTilesY - 250))
				{
					num2 = 0.0;
				}
				num = num4 * (1.0 + (double)random.Next(-20, 20) * 0.01);
				num2 -= 1.0;
				int num5 = (int)(val.X - num);
				int num6 = (int)(val.X + num);
				int num7 = (int)(val.Y - num);
				int num8 = (int)(val.Y + num);
				if (num5 < 1)
				{
					num5 = 1;
				}
				if (num6 > Main.maxTilesX - 1)
				{
					num6 = Main.maxTilesX - 1;
				}
				if (num7 < 1)
				{
					num7 = 1;
				}
				if (num8 > Main.maxTilesY - 1)
				{
					num8 = Main.maxTilesY - 1;
				}
				for (int k = num5; k < num6; k++)
				{
					for (int l = num7; l < num8; l++)
					{
						if (!WorldGen.InWorld(k, l, 50))
						{
							num2 = 0.0;
						}
						else
						{
							if (Main.tile[k - 10, l].wall == 87)
							{
								num2 = 0.0;
							}
							if (Main.tile[k + 10, l].wall == 87)
							{
								num2 = 0.0;
							}
							if (Main.tile[k, l - 10].wall == 87)
							{
								num2 = 0.0;
							}
							if (Main.tile[k, l + 10].wall == 87)
							{
								num2 = 0.0;
							}
						}
						if ((double)l < Main.worldSurface && Main.tile[k, l - 5].wall == 0)
						{
							num2 = 0.0;
						}
						double num9 = Math.Abs((double)k - val.X);
						double num10 = Math.Abs((double)l - val.Y);
						double num11 = Math.Sqrt(num9 * num9 + num10 * num10);
						if (num11 < num4 * 0.4 * (1.0 + (double)random.Next(-10, 11) * 0.005))
						{
							if (random.Next(3) == 0)
							{
								Main.tile[k, l].liquid = byte.MaxValue;
							}
							if (WorldGen.drunkWorldGen)
							{
								Main.tile[k, l].liquid = byte.MaxValue;
							}
							Main.tile[k, l].honey(honey: true);
							Main.tile[k, l].wall = 86;
							Main.tile[k, l].active(active: false);
							Main.tile[k, l].halfBrick(halfBrick: false);
							Main.tile[k, l].slope(0);
						}
						else if (num11 < num4 * 0.75 * (1.0 + (double)random.Next(-10, 11) * 0.005))
						{
							Main.tile[k, l].liquid = 0;
							if (Main.tile[k, l].wall != 86)
							{
								Main.tile[k, l].active(active: true);
								Main.tile[k, l].halfBrick(halfBrick: false);
								Main.tile[k, l].slope(0);
								Main.tile[k, l].type = 225;
							}
						}
						if (num11 < num4 * 0.6 * (1.0 + (double)random.Next(-10, 11) * 0.005))
						{
							Main.tile[k, l].wall = 86;
							if (WorldGen.drunkWorldGen && random.Next(2) == 0)
							{
								Main.tile[k, l].liquid = byte.MaxValue;
								Main.tile[k, l].honey(honey: true);
							}
						}
					}
				}
				val += val2;
				num2 -= 1.0;
				val2.Y += (double)random.Next(-10, 11) * 0.05;
				val2.X += (double)random.Next(-10, 11) * 0.05;
			}
			return val;
		}

		private static bool TooCloseToImportantLocations(Point origin)
		{
			int x = origin.X;
			int y = origin.Y;
			int num = 150;
			for (int i = x - num; i < x + num; i += 10)
			{
				if (i <= 0 || i > Main.maxTilesX - 1)
				{
					continue;
				}
				for (int j = y - num; j < y + num; j += 10)
				{
					if (j > 0 && j <= Main.maxTilesY - 1)
					{
						if (Main.tile[i, j].active() && Main.tile[i, j].type == 226)
						{
							return true;
						}
						if (Main.tile[i, j].wall == 83 || Main.tile[i, j].wall == 3 || Main.tile[i, j].wall == 87)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		private static void CreateDentForHoneyFall(int x, int y, int dir)
		{
			dir *= -1;
			y++;
			int num = 0;
			while ((num < 4 || WorldGen.SolidTile(x, y)) && x > 10 && x < Main.maxTilesX - 10)
			{
				num++;
				x += dir;
				if (WorldGen.SolidTile(x, y))
				{
					WorldGen.PoundTile(x, y);
					if (!Main.tile[x, y + 1].active())
					{
						Main.tile[x, y + 1].active(active: true);
						Main.tile[x, y + 1].type = 225;
					}
				}
			}
		}

		private static void CreateBlockedHoneyCube(int x, int y)
		{
			for (int i = x - 1; i <= x + 2; i++)
			{
				for (int j = y - 1; j <= y + 2; j++)
				{
					if (i >= x && i <= x + 1 && j >= y && j <= y + 1)
					{
						Main.tile[i, j].active(active: false);
						Main.tile[i, j].liquid = byte.MaxValue;
						Main.tile[i, j].honey(honey: true);
					}
					else
					{
						Main.tile[i, j].active(active: true);
						Main.tile[i, j].type = 225;
					}
				}
			}
		}

		private static bool SpotActuallyNotInHive(int x, int y)
		{
			for (int i = x - 1; i <= x + 2; i++)
			{
				for (int j = y - 1; j <= y + 2; j++)
				{
					if (i < 10 || i > Main.maxTilesX - 10)
					{
						return true;
					}
					if (Main.tile[i, j].active() && Main.tile[i, j].type != 225)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool BadSpotForHoneyFall(int x, int y)
		{
			if (Main.tile[x, y].active() && Main.tile[x, y + 1].active() && Main.tile[x + 1, y].active())
			{
				return !Main.tile[x + 1, y + 1].active();
			}
			return true;
		}

		public static void CreateStandForLarva(Vector2D position)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			GenVars.larvaX[GenVars.numLarva] = Utils.Clamp((int)position.X, 5, Main.maxTilesX - 5);
			GenVars.larvaY[GenVars.numLarva] = Utils.Clamp((int)position.Y, 5, Main.maxTilesY - 5);
			GenVars.numLarva++;
			if (GenVars.numLarva >= GenVars.larvaX.Length)
			{
				GenVars.numLarva = GenVars.larvaX.Length - 1;
			}
			int num = (int)position.X;
			int num2 = (int)position.Y;
			for (int i = num - 1; i <= num + 1 && i > 0 && i < Main.maxTilesX; i++)
			{
				for (int j = num2 - 2; j <= num2 + 1 && j > 0 && j < Main.maxTilesY; j++)
				{
					if (j != num2 + 1)
					{
						Main.tile[i, j].active(active: false);
						continue;
					}
					Main.tile[i, j].active(active: true);
					Main.tile[i, j].type = 225;
					Main.tile[i, j].slope(0);
					Main.tile[i, j].halfBrick(halfBrick: false);
				}
			}
		}
	}
}
