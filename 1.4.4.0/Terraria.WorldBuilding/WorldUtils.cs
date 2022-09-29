using System;
using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	public static class WorldUtils
	{
		public static Rectangle ClampToWorld(Rectangle tileRectangle)
		{
			int num = Math.Max(0, Math.Min(tileRectangle.Left, Main.maxTilesX));
			int num2 = Math.Max(0, Math.Min(tileRectangle.Top, Main.maxTilesY));
			int num3 = Math.Max(0, Math.Min(tileRectangle.Right, Main.maxTilesX));
			int num4 = Math.Max(0, Math.Min(tileRectangle.Bottom, Main.maxTilesY));
			return new Rectangle(num, num2, num3 - num, num4 - num2);
		}

		public static bool Gen(Point origin, GenShape shape, GenAction action)
		{
			return shape.Perform(origin, action);
		}

		public static bool Gen(Point origin, GenShapeActionPair pair)
		{
			return pair.Shape.Perform(origin, pair.Action);
		}

		public static bool Find(Point origin, GenSearch search, out Point result)
		{
			result = search.Find(origin);
			if (result == GenSearch.NOT_FOUND)
			{
				return false;
			}
			return true;
		}

		public static void ClearTile(int x, int y, bool frameNeighbors = false)
		{
			Main.tile[x, y].ClearTile();
			if (frameNeighbors)
			{
				WorldGen.TileFrame(x + 1, y);
				WorldGen.TileFrame(x - 1, y);
				WorldGen.TileFrame(x, y + 1);
				WorldGen.TileFrame(x, y - 1);
			}
		}

		public static void ClearWall(int x, int y, bool frameNeighbors = false)
		{
			Main.tile[x, y].wall = 0;
			if (frameNeighbors)
			{
				WorldGen.SquareWallFrame(x + 1, y);
				WorldGen.SquareWallFrame(x - 1, y);
				WorldGen.SquareWallFrame(x, y + 1);
				WorldGen.SquareWallFrame(x, y - 1);
			}
		}

		public static void TileFrame(int x, int y, bool frameNeighbors = false)
		{
			WorldGen.TileFrame(x, y, resetFrame: true);
			if (frameNeighbors)
			{
				WorldGen.TileFrame(x + 1, y, resetFrame: true);
				WorldGen.TileFrame(x - 1, y, resetFrame: true);
				WorldGen.TileFrame(x, y + 1, resetFrame: true);
				WorldGen.TileFrame(x, y - 1, resetFrame: true);
			}
		}

		public static void ClearChestLocation(int x, int y)
		{
			ClearTile(x, y, frameNeighbors: true);
			ClearTile(x - 1, y, frameNeighbors: true);
			ClearTile(x, y - 1, frameNeighbors: true);
			ClearTile(x - 1, y - 1, frameNeighbors: true);
		}

		public static void WireLine(Point start, Point end)
		{
			Point point = start;
			Point point2 = end;
			if (end.X < start.X)
			{
				Utils.Swap(ref end.X, ref start.X);
			}
			if (end.Y < start.Y)
			{
				Utils.Swap(ref end.Y, ref start.Y);
			}
			for (int i = start.X; i <= end.X; i++)
			{
				WorldGen.PlaceWire(i, point.Y);
			}
			for (int j = start.Y; j <= end.Y; j++)
			{
				WorldGen.PlaceWire(point2.X, j);
			}
		}

		public static void DebugRegen()
		{
			WorldGen.clearWorld();
			WorldGen.GenerateWorld(Main.ActiveWorldFileData.Seed);
			Main.NewText("World Regen Complete.");
		}

		public static void DebugRotate()
		{
			int num = 0;
			int num2 = 0;
			int maxTilesY = Main.maxTilesY;
			for (int i = 0; i < Main.maxTilesX / Main.maxTilesY; i++)
			{
				for (int j = 0; j < maxTilesY / 2; j++)
				{
					for (int k = j; k < maxTilesY - j; k++)
					{
						Tile tile = Main.tile[k + num, j + num2];
						Main.tile[k + num, j + num2] = Main.tile[j + num, maxTilesY - k + num2];
						Main.tile[j + num, maxTilesY - k + num2] = Main.tile[maxTilesY - k + num, maxTilesY - j + num2];
						Main.tile[maxTilesY - k + num, maxTilesY - j + num2] = Main.tile[maxTilesY - j + num, k + num2];
						Main.tile[maxTilesY - j + num, k + num2] = tile;
					}
				}
				num += maxTilesY;
			}
		}
	}
}
