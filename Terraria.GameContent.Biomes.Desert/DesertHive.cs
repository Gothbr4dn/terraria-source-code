using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Utilities;

namespace Terraria.GameContent.Biomes.Desert
{
	public static class DesertHive
	{
		private struct Block
		{
			public Vector2D Position;

			public Block(double x, double y)
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				Position = new Vector2D(x, y);
			}
		}

		private class Cluster : List<Block>
		{
		}

		private class ClusterGroup : List<Cluster>
		{
			public readonly int Width;

			public readonly int Height;

			private ClusterGroup(int width, int height)
			{
				Width = width;
				Height = height;
				Generate();
			}

			public static ClusterGroup FromDescription(DesertDescription description)
			{
				return new ClusterGroup(description.BlockColumnCount, description.BlockRowCount);
			}

			private static void SearchForCluster(bool[,] blockMap, List<Point> pointCluster, int x, int y, int level = 2)
			{
				pointCluster.Add(new Point(x, y));
				blockMap[x, y] = false;
				level--;
				if (level != -1)
				{
					if (x > 0 && blockMap[x - 1, y])
					{
						SearchForCluster(blockMap, pointCluster, x - 1, y, level);
					}
					if (x < blockMap.GetLength(0) - 1 && blockMap[x + 1, y])
					{
						SearchForCluster(blockMap, pointCluster, x + 1, y, level);
					}
					if (y > 0 && blockMap[x, y - 1])
					{
						SearchForCluster(blockMap, pointCluster, x, y - 1, level);
					}
					if (y < blockMap.GetLength(1) - 1 && blockMap[x, y + 1])
					{
						SearchForCluster(blockMap, pointCluster, x, y + 1, level);
					}
				}
			}

			private static void AttemptClaim(int x, int y, int[,] clusterIndexMap, List<List<Point>> pointClusters, int index)
			{
				int num = clusterIndexMap[x, y];
				if (num == -1 || num == index)
				{
					return;
				}
				int num2 = ((WorldGen.genRand.Next(2) == 0) ? (-1) : index);
				foreach (Point item in pointClusters[num])
				{
					clusterIndexMap[item.X, item.Y] = num2;
				}
			}

			private void Generate()
			{
				Clear();
				bool[,] array = new bool[Width, Height];
				int num = Width / 2 - 1;
				int num2 = Height / 2 - 1;
				int num3 = (num + 1) * (num + 1);
				Point point = new Point(num, num2);
				for (int i = point.Y - num2; i <= point.Y + num2; i++)
				{
					double num4 = (double)num / (double)num2 * (double)(i - point.Y);
					int num5 = Math.Min(num, (int)Math.Sqrt((double)num3 - num4 * num4));
					for (int j = point.X - num5; j <= point.X + num5; j++)
					{
						array[j, i] = WorldGen.genRand.Next(2) == 0;
					}
				}
				List<List<Point>> list = new List<List<Point>>();
				for (int k = 0; k < array.GetLength(0); k++)
				{
					for (int l = 0; l < array.GetLength(1); l++)
					{
						if (array[k, l] && WorldGen.genRand.Next(2) == 0)
						{
							List<Point> list2 = new List<Point>();
							SearchForCluster(array, list2, k, l);
							if (list2.Count > 2)
							{
								list.Add(list2);
							}
						}
					}
				}
				int[,] array2 = new int[array.GetLength(0), array.GetLength(1)];
				for (int m = 0; m < array2.GetLength(0); m++)
				{
					for (int n = 0; n < array2.GetLength(1); n++)
					{
						array2[m, n] = -1;
					}
				}
				for (int num6 = 0; num6 < list.Count; num6++)
				{
					foreach (Point item in list[num6])
					{
						array2[item.X, item.Y] = num6;
					}
				}
				for (int num7 = 0; num7 < list.Count; num7++)
				{
					foreach (Point item2 in list[num7])
					{
						int x = item2.X;
						int y = item2.Y;
						if (array2[x, y] == -1)
						{
							break;
						}
						int index = array2[x, y];
						if (x > 0)
						{
							AttemptClaim(x - 1, y, array2, list, index);
						}
						if (x < array2.GetLength(0) - 1)
						{
							AttemptClaim(x + 1, y, array2, list, index);
						}
						if (y > 0)
						{
							AttemptClaim(x, y - 1, array2, list, index);
						}
						if (y < array2.GetLength(1) - 1)
						{
							AttemptClaim(x, y + 1, array2, list, index);
						}
					}
				}
				foreach (List<Point> item3 in list)
				{
					item3.Clear();
				}
				for (int num8 = 0; num8 < array2.GetLength(0); num8++)
				{
					for (int num9 = 0; num9 < array2.GetLength(1); num9++)
					{
						if (array2[num8, num9] != -1)
						{
							list[array2[num8, num9]].Add(new Point(num8, num9));
						}
					}
				}
				foreach (List<Point> item4 in list)
				{
					if (item4.Count < 4)
					{
						item4.Clear();
					}
				}
				foreach (List<Point> item5 in list)
				{
					Cluster cluster = new Cluster();
					if (item5.Count <= 0)
					{
						continue;
					}
					foreach (Point item6 in item5)
					{
						cluster.Add(new Block((double)item6.X + (WorldGen.genRand.NextDouble() - 0.5) * 0.5, (double)item6.Y + (WorldGen.genRand.NextDouble() - 0.5) * 0.5));
					}
					Add(cluster);
				}
			}
		}

		[Flags]
		private enum PostPlacementEffect : byte
		{
			None = 0,
			Smooth = 1
		}

		public static void Place(DesertDescription description)
		{
			ClusterGroup clusters = ClusterGroup.FromDescription(description);
			PlaceClusters(description, clusters);
			AddTileVariance(description);
		}

		private static void PlaceClusters(DesertDescription description, ClusterGroup clusters)
		{
			Rectangle hive = description.Hive;
			hive.Inflate(20, 20);
			PostPlacementEffect[,] array = new PostPlacementEffect[hive.Width, hive.Height];
			PlaceClustersArea(description, clusters, hive, array, Point.Zero);
			for (int i = hive.Left; i < hive.Right; i++)
			{
				for (int j = hive.Top; j < hive.Bottom; j++)
				{
					PostPlacementEffect postPlacementEffect = array[i - hive.Left, j - hive.Top];
					if (postPlacementEffect.HasFlag(PostPlacementEffect.Smooth))
					{
						Tile.SmoothSlope(i, j, applyToNeighbors: false);
					}
				}
			}
		}

		private static void PlaceClustersArea(DesertDescription description, ClusterGroup clusters, Rectangle area, PostPlacementEffect[,] postEffectMap, Point postEffectMapOffset)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0241: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			FastRandom fastRandom = new FastRandom(Main.ActiveWorldFileData.Seed).WithModifier(57005uL);
			Vector2D val = default(Vector2D);
			((Vector2D)(ref val))._002Ector((double)description.Hive.Width, (double)description.Hive.Height);
			Vector2D val2 = default(Vector2D);
			((Vector2D)(ref val2))._002Ector((double)clusters.Width, (double)clusters.Height);
			Vector2D val3 = description.BlockScale / 2.0;
			for (int i = area.Left; i < area.Right; i++)
			{
				for (int j = area.Top; j < area.Bottom; j++)
				{
					byte liquid = Main.tile[i, j].liquid;
					if (!WorldGen.InWorld(i, j, 1))
					{
						continue;
					}
					double num = 0.0;
					int num2 = -1;
					double num3 = 0.0;
					ushort type = 53;
					if (fastRandom.Next(3) == 0)
					{
						type = 397;
					}
					int num4 = i - description.Hive.X;
					int num5 = j - description.Hive.Y;
					Vector2D val4 = (new Vector2D((double)num4, (double)num5) - val3) / val * val2;
					for (int k = 0; k < clusters.Count; k++)
					{
						Cluster cluster = clusters[k];
						if (Math.Abs(cluster[0].Position.X - val4.X) > 10.0 || Math.Abs(cluster[0].Position.Y - val4.Y) > 10.0)
						{
							continue;
						}
						double num6 = 0.0;
						foreach (Block item in cluster)
						{
							num6 += 1.0 / Vector2D.DistanceSquared(item.Position, val4);
						}
						if (num6 > num)
						{
							if (num > num3)
							{
								num3 = num;
							}
							num = num6;
							num2 = k;
						}
						else if (num6 > num3)
						{
							num3 = num6;
						}
					}
					double num7 = num + num3;
					Tile tile = Main.tile[i, j];
					Vector2D val5 = (new Vector2D((double)num4, (double)num5) - val3) / val * 2.0 - Vector2D.get_One();
					bool flag = ((Vector2D)(ref val5)).Length() >= 0.8;
					PostPlacementEffect postPlacementEffect = PostPlacementEffect.None;
					bool flag2 = true;
					if (num7 > 3.5)
					{
						postPlacementEffect = PostPlacementEffect.Smooth;
						tile.ClearEverything();
						if (!WorldGen.remixWorldGen || !((double)j > Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2)))
						{
							tile.wall = 187;
							if (num2 % 15 == 2)
							{
								tile.ResetToType(404);
							}
						}
					}
					else if (num7 > 1.8)
					{
						if (!WorldGen.remixWorldGen || !((double)j > Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2)))
						{
							tile.wall = 187;
						}
						if ((double)j < Main.worldSurface)
						{
							tile.liquid = 0;
						}
						else if (!WorldGen.remixWorldGen)
						{
							tile.lava(lava: true);
						}
						if (!flag || tile.active())
						{
							tile.ResetToType(396);
							postPlacementEffect = PostPlacementEffect.Smooth;
						}
					}
					else if (num7 > 0.7 || !flag)
					{
						if (!WorldGen.remixWorldGen || !((double)j > Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2)))
						{
							tile.wall = 216;
							tile.liquid = 0;
						}
						if (!flag || tile.active())
						{
							tile.ResetToType(type);
							postPlacementEffect = PostPlacementEffect.Smooth;
						}
					}
					else if (num7 > 0.25)
					{
						FastRandom fastRandom2 = fastRandom.WithModifier(num4, num5);
						double num8 = (num7 - 0.25) / 0.45;
						if (fastRandom2.NextDouble() < num8)
						{
							if (!WorldGen.remixWorldGen || !((double)j > Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2)))
							{
								tile.wall = 187;
							}
							if ((double)j < Main.worldSurface)
							{
								tile.liquid = 0;
							}
							else if (!WorldGen.remixWorldGen)
							{
								tile.lava(lava: true);
							}
							if (tile.active())
							{
								tile.ResetToType(type);
								postPlacementEffect = PostPlacementEffect.Smooth;
							}
						}
					}
					else
					{
						flag2 = false;
					}
					if (flag2)
					{
						WorldGen.UpdateDesertHiveBounds(i, j);
					}
					postEffectMap[i - area.X + postEffectMapOffset.X, j - area.Y + postEffectMapOffset.Y] = postPlacementEffect;
					if (WorldGen.remixWorldGen)
					{
						Main.tile[i, j].liquid = liquid;
					}
				}
			}
		}

		private static void AddTileVariance(DesertDescription description)
		{
			for (int i = -20; i < description.Hive.Width + 20; i++)
			{
				for (int j = -20; j < description.Hive.Height + 20; j++)
				{
					int num = i + description.Hive.X;
					int num2 = j + description.Hive.Y;
					if (WorldGen.InWorld(num, num2, 1))
					{
						Tile tile = Main.tile[num, num2];
						Tile testTile = Main.tile[num, num2 + 1];
						Tile testTile2 = Main.tile[num, num2 + 2];
						if (tile.type == 53 && (!WorldGen.SolidTile(testTile) || !WorldGen.SolidTile(testTile2)))
						{
							tile.type = 397;
						}
					}
				}
			}
			for (int k = -20; k < description.Hive.Width + 20; k++)
			{
				for (int l = -20; l < description.Hive.Height + 20; l++)
				{
					int num3 = k + description.Hive.X;
					int num4 = l + description.Hive.Y;
					if (!WorldGen.InWorld(num3, num4, 1))
					{
						continue;
					}
					Tile tile2 = Main.tile[num3, num4];
					if (!tile2.active() || tile2.type != 396)
					{
						continue;
					}
					bool flag = true;
					for (int num5 = -1; num5 >= -3; num5--)
					{
						if (Main.tile[num3, num4 + num5].active())
						{
							flag = false;
							break;
						}
					}
					bool flag2 = true;
					for (int m = 1; m <= 3; m++)
					{
						if (Main.tile[num3, num4 + m].active())
						{
							flag2 = false;
							break;
						}
					}
					if (!WorldGen.remixWorldGen || !((double)num4 > Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2)))
					{
						if (flag && WorldGen.genRand.Next(20) == 0)
						{
							WorldGen.PlaceTile(num3, num4 - 1, 485, mute: true, forced: true, -1, WorldGen.genRand.Next(4));
						}
						else if (flag && WorldGen.genRand.Next(5) == 0)
						{
							WorldGen.PlaceTile(num3, num4 - 1, 484, mute: true, forced: true);
						}
						else if ((flag ^ flag2) && WorldGen.genRand.Next(5) == 0)
						{
							WorldGen.PlaceTile(num3, num4 + ((!flag) ? 1 : (-1)), 165, mute: true, forced: true);
						}
						else if (flag && WorldGen.genRand.Next(5) == 0)
						{
							WorldGen.PlaceTile(num3, num4 - 1, 187, mute: true, forced: true, -1, 29 + WorldGen.genRand.Next(6));
						}
					}
				}
			}
		}
	}
}
