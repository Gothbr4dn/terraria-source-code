using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Events
{
	public class CultistRitual
	{
		public const int delayStart = 86400;

		public const int respawnDelay = 43200;

		private const int timePerCultist = 3600;

		private const int recheckStart = 600;

		public static int delay;

		public static int recheck;

		public static void UpdateTime()
		{
			if (Main.netMode == 1)
			{
				return;
			}
			delay -= Main.dayRate;
			if (delay < 0)
			{
				delay = 0;
			}
			recheck -= Main.dayRate;
			if (recheck < 0)
			{
				recheck = 0;
			}
			if (delay == 0 && recheck == 0)
			{
				recheck = 600;
				if (NPC.AnyDanger())
				{
					recheck *= 6;
				}
				else
				{
					TrySpawning(Main.dungeonX, Main.dungeonY);
				}
			}
		}

		public static void CultistSlain()
		{
			delay -= 3600;
		}

		public static void TabletDestroyed()
		{
			delay = 43200;
		}

		public static void TrySpawning(int x, int y)
		{
			if (!WorldGen.PlayerLOS(x - 6, y) && !WorldGen.PlayerLOS(x + 6, y) && CheckRitual(x, y))
			{
				NPC.NewNPC(new EntitySource_WorldEvent(), x * 16 + 8, (y - 4) * 16 - 8, 437);
			}
		}

		private static bool CheckRitual(int x, int y)
		{
			if (delay != 0 || !Main.hardMode || !NPC.downedGolemBoss || !NPC.downedBoss3)
			{
				return false;
			}
			if (y < 7 || WorldGen.SolidTile(Main.tile[x, y - 7]))
			{
				return false;
			}
			if (NPC.AnyNPCs(437))
			{
				return false;
			}
			Vector2 center = new Vector2(x * 16 + 8, y * 16 - 64 - 8 - 27);
			Point[] spawnPoints = null;
			if (!CheckFloor(center, out spawnPoints))
			{
				return false;
			}
			return true;
		}

		public static bool CheckFloor(Vector2 Center, out Point[] spawnPoints)
		{
			Point[] array = new Point[4];
			int num = 0;
			Point point = Center.ToTileCoordinates();
			for (int i = -5; i <= 5; i += 2)
			{
				if (i == -1 || i == 1)
				{
					continue;
				}
				for (int j = -5; j < 12; j++)
				{
					int num2 = point.X + i * 2;
					int num3 = point.Y + j;
					if ((WorldGen.SolidTile(num2, num3) || TileID.Sets.Platforms[Framing.GetTileSafely(num2, num3).type]) && (!Collision.SolidTiles(num2 - 1, num2 + 1, num3 - 3, num3 - 1) || (!Collision.SolidTiles(num2, num2, num3 - 3, num3 - 1) && !Collision.SolidTiles(num2 + 1, num2 + 1, num3 - 3, num3 - 2) && !Collision.SolidTiles(num2 - 1, num2 - 1, num3 - 3, num3 - 2))))
					{
						array[num++] = new Point(num2, num3);
						break;
					}
				}
			}
			if (num != 4)
			{
				spawnPoints = null;
				return false;
			}
			spawnPoints = array;
			return true;
		}
	}
}
