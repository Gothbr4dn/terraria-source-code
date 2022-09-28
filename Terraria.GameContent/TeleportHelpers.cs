using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	public class TeleportHelpers
	{
		public static bool RequestMagicConchTeleportPosition(Player player, int crawlOffsetX, int startX, out Point landingPoint)
		{
			landingPoint = default(Point);
			Point point = new Point(startX, 50);
			int num = 1;
			int num2 = -1;
			int num3 = 1;
			int num4 = 0;
			int num5 = 5000;
			Vector2 vector = new Vector2((float)player.width * 0.5f, player.height);
			int num6 = 40;
			bool flag = WorldGen.SolidOrSlopedTile(Main.tile[point.X, point.Y]);
			int num7 = 0;
			int num8 = 400;
			while (num4 < num5 && num7 < num8)
			{
				num4++;
				Tile tile = Main.tile[point.X, point.Y];
				Tile tile2 = Main.tile[point.X, point.Y + num3];
				bool flag2 = WorldGen.SolidOrSlopedTile(tile) || tile.liquid > 0;
				bool flag3 = WorldGen.SolidOrSlopedTile(tile2) || tile2.liquid > 0;
				if (IsInSolidTilesExtended(new Vector2(point.X * 16 + 8, point.Y * 16 + 15) - vector, player.velocity, player.width, player.height, (int)player.gravDir))
				{
					if (flag)
					{
						point.Y += num;
					}
					else
					{
						point.Y += num2;
					}
					continue;
				}
				if (flag2)
				{
					if (flag)
					{
						point.Y += num;
					}
					else
					{
						point.Y += num2;
					}
					continue;
				}
				flag = false;
				if (!IsInSolidTilesExtended(new Vector2(point.X * 16 + 8, point.Y * 16 + 15 + 16) - vector, player.velocity, player.width, player.height, (int)player.gravDir) && !flag3 && (double)point.Y < Main.worldSurface)
				{
					point.Y += num;
					continue;
				}
				if (tile2.liquid > 0)
				{
					point.X += crawlOffsetX;
					num7++;
					continue;
				}
				if (TileIsDangerous(point.X, point.Y))
				{
					point.X += crawlOffsetX;
					num7++;
					continue;
				}
				if (TileIsDangerous(point.X, point.Y + num3))
				{
					point.X += crawlOffsetX;
					num7++;
					continue;
				}
				if (point.Y >= num6)
				{
					break;
				}
				point.Y += num;
			}
			if (num4 == num5 || num7 >= num8)
			{
				return false;
			}
			if (!WorldGen.InWorld(point.X, point.Y, 40))
			{
				return false;
			}
			landingPoint = point;
			return true;
		}

		private static bool TileIsDangerous(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			if (tile.liquid > 0 && tile.lava())
			{
				return true;
			}
			if (tile.wall == 87 && (double)y > Main.worldSurface && !NPC.downedPlantBoss)
			{
				return true;
			}
			if (Main.wallDungeon[tile.wall] && (double)y > Main.worldSurface && !NPC.downedBoss3)
			{
				return true;
			}
			return false;
		}

		private static bool IsInSolidTilesExtended(Vector2 testPosition, Vector2 playerVelocity, int width, int height, int gravDir)
		{
			if (Collision.LavaCollision(testPosition, width, height))
			{
				return true;
			}
			if (Collision.AnyHurtingTiles(testPosition, width, height))
			{
				return true;
			}
			if (Collision.SolidCollision(testPosition, width, height))
			{
				return true;
			}
			Vector2 vector = Vector2.UnitX * 16f;
			if (Collision.TileCollision(testPosition - vector, vector, width, height, fallThrough: false, fall2: false, gravDir) != vector)
			{
				return true;
			}
			vector = -Vector2.UnitX * 16f;
			if (Collision.TileCollision(testPosition - vector, vector, width, height, fallThrough: false, fall2: false, gravDir) != vector)
			{
				return true;
			}
			vector = Vector2.UnitY * 16f;
			if (Collision.TileCollision(testPosition - vector, vector, width, height, fallThrough: false, fall2: false, gravDir) != vector)
			{
				return true;
			}
			vector = -Vector2.UnitY * 16f;
			if (Collision.TileCollision(testPosition - vector, vector, width, height, fallThrough: false, fall2: false, gravDir) != vector)
			{
				return true;
			}
			return false;
		}
	}
}
