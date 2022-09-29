using System;
using Microsoft.Xna.Framework;

namespace Terraria
{
	public class StrayMethods
	{
		public static bool CountSandHorizontally(int i, int j, bool[] fittingTypes, int requiredTotalSpread = 4, int spreadInEachAxis = 5)
		{
			if (!WorldGen.InWorld(i, j, 2))
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			int num3 = i - 1;
			while (num < spreadInEachAxis && num3 > 0)
			{
				Tile tile = Main.tile[num3, j];
				if (tile.active() && fittingTypes[tile.type] && !WorldGen.SolidTileAllowBottomSlope(num3, j - 1))
				{
					num++;
				}
				else if (!tile.active())
				{
					break;
				}
				num3--;
			}
			num3 = i + 1;
			while (num2 < spreadInEachAxis && num3 < Main.maxTilesX - 1)
			{
				Tile tile2 = Main.tile[num3, j];
				if (tile2.active() && fittingTypes[tile2.type] && !WorldGen.SolidTileAllowBottomSlope(num3, j - 1))
				{
					num2++;
				}
				else if (!tile2.active())
				{
					break;
				}
				num3++;
			}
			return num + num2 + 1 >= requiredTotalSpread;
		}

		public static bool CanSpawnSandstormHostile(Vector2 position, int expandUp, int expandDown)
		{
			bool result = true;
			Point point = position.ToTileCoordinates();
			for (int i = -1; i <= 1; i++)
			{
				Collision.ExpandVertically(point.X + i, point.Y, out var topY, out var bottomY, expandUp, expandDown);
				topY++;
				bottomY--;
				if (bottomY - topY < 20)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		public static bool CanSpawnSandstormFriendly(Vector2 position, int expandUp, int expandDown)
		{
			bool result = true;
			Point point = position.ToTileCoordinates();
			for (int i = -1; i <= 1; i++)
			{
				Collision.ExpandVertically(point.X + i, point.Y, out var topY, out var bottomY, expandUp, expandDown);
				topY++;
				bottomY--;
				if (bottomY - topY < 10)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		public static void CheckArenaScore(Vector2 arenaCenter, out Point xLeftEnd, out Point xRightEnd, int walkerWidthInTiles = 5, int walkerHeightInTiles = 10)
		{
			bool flag = false;
			Point point = arenaCenter.ToTileCoordinates();
			xLeftEnd = (xRightEnd = point);
			Collision.ExpandVertically(point.X, point.Y, out var _, out var bottomY, 0, 4);
			point.Y = bottomY;
			if (flag)
			{
				Dust.QuickDust(point, Color.Blue).scale = 5f;
			}
			SendWalker(point, walkerHeightInTiles, -1, out var _, out var lastIteratedFloorSpot, 120, flag);
			SendWalker(point, walkerHeightInTiles, 1, out var _, out var lastIteratedFloorSpot2, 120, flag);
			lastIteratedFloorSpot.X++;
			lastIteratedFloorSpot2.X--;
			if (flag)
			{
				Dust.QuickDustLine(lastIteratedFloorSpot.ToWorldCoordinates(), lastIteratedFloorSpot2.ToWorldCoordinates(), 50f, Color.Pink);
			}
			xLeftEnd = lastIteratedFloorSpot;
			xRightEnd = lastIteratedFloorSpot2;
		}

		public static void SendWalker(Point startFloorPosition, int height, int direction, out int distanceCoveredInTiles, out Point lastIteratedFloorSpot, int maxDistance = 100, bool showDebug = false)
		{
			distanceCoveredInTiles = 0;
			startFloorPosition.Y--;
			lastIteratedFloorSpot = startFloorPosition;
			for (int i = 0; i < maxDistance; i++)
			{
				for (int j = 0; j < 3; j++)
				{
					if (!WorldGen.SolidTile3(startFloorPosition.X, startFloorPosition.Y))
					{
						break;
					}
					startFloorPosition.Y--;
				}
				Collision.ExpandVertically(startFloorPosition.X, startFloorPosition.Y, out var topY, out var bottomY, height, 2);
				topY++;
				bottomY--;
				if (!WorldGen.SolidTile3(startFloorPosition.X, bottomY + 1))
				{
					Collision.ExpandVertically(startFloorPosition.X, bottomY, out var topY2, out var bottomY2, 0, 6);
					if (showDebug)
					{
						Dust.QuickBox(new Vector2(startFloorPosition.X * 16 + 8, topY2 * 16), new Vector2(startFloorPosition.X * 16 + 8, bottomY2 * 16), 1, Color.Blue, null);
					}
					if (!WorldGen.SolidTile3(startFloorPosition.X, bottomY2))
					{
						break;
					}
				}
				if (bottomY - topY < height - 1)
				{
					break;
				}
				if (showDebug)
				{
					Dust.QuickDust(startFloorPosition, Color.Green).scale = 1f;
					Dust.QuickBox(new Vector2(startFloorPosition.X * 16 + 8, topY * 16), new Vector2(startFloorPosition.X * 16 + 8, bottomY * 16 + 16), 1, Color.Red, null);
				}
				distanceCoveredInTiles += direction;
				startFloorPosition.X += direction;
				startFloorPosition.Y = bottomY;
				lastIteratedFloorSpot = startFloorPosition;
				if (Math.Abs(distanceCoveredInTiles) >= maxDistance)
				{
					break;
				}
			}
			distanceCoveredInTiles = Math.Abs(distanceCoveredInTiles);
		}
	}
}
