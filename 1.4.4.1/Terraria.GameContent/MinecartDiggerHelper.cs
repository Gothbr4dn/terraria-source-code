using Microsoft.Xna.Framework;
using Terraria.GameContent.Achievements;
using Terraria.ID;

namespace Terraria.GameContent
{
	public class MinecartDiggerHelper
	{
		public static MinecartDiggerHelper Instance = new MinecartDiggerHelper();

		public void TryDigging(Player player, Vector2 trackWorldPosition, int digDirectionX, int digDirectionY)
		{
			digDirectionY = 0;
			Point point = trackWorldPosition.ToTileCoordinates();
			if (Framing.GetTileSafely(point).type != 314 || (double)point.Y < Main.worldSurface)
			{
				return;
			}
			Point point2 = point;
			point2.X += digDirectionX;
			point2.Y += digDirectionY;
			if (AlreadyLeadsIntoWantedTrack(point, point2) || (digDirectionY == 0 && (AlreadyLeadsIntoWantedTrack(point, new Point(point2.X, point2.Y - 1)) || AlreadyLeadsIntoWantedTrack(point, new Point(point2.X, point2.Y + 1)))))
			{
				return;
			}
			int num = 5;
			if (digDirectionY != 0)
			{
				num = 5;
			}
			Point point3 = point2;
			Point point4 = point3;
			point4.Y -= num - 1;
			int x = point4.X;
			for (int i = point4.Y; i <= point3.Y; i++)
			{
				if (!CanGetPastTile(x, i) || !HasPickPower(player, x, i))
				{
					return;
				}
			}
			if (CanConsumeATrackItem(player))
			{
				int x2 = point4.X;
				for (int j = point4.Y; j <= point3.Y; j++)
				{
					MineTheTileIfNecessary(x2, j);
				}
				ConsumeATrackItem(player);
				PlaceATrack(point2.X, point2.Y);
				player.velocity.X = MathHelper.Clamp(player.velocity.X, -1f, 1f);
				if (!DoTheTracksConnectProperly(point, point2))
				{
					CorrectTrackConnections(point, point2);
				}
			}
		}

		private bool CanConsumeATrackItem(Player player)
		{
			return FindMinecartTrackItem(player) != null;
		}

		private void ConsumeATrackItem(Player player)
		{
			Item item = FindMinecartTrackItem(player);
			item.stack--;
			if (item.stack == 0)
			{
				item.TurnToAir();
			}
		}

		private Item FindMinecartTrackItem(Player player)
		{
			Item result = null;
			for (int i = 0; i < 58; i++)
			{
				if (player.selectedItem != i || (player.itemAnimation <= 0 && player.reuseDelay <= 0 && player.itemTime <= 0))
				{
					Item item = player.inventory[i];
					if (item.type == 2340 && item.stack > 0)
					{
						result = item;
						break;
					}
				}
			}
			return result;
		}

		private void PoundTrack(Point spot)
		{
			if (Main.tile[spot.X, spot.Y].type == 314 && Minecart.FrameTrack(spot.X, spot.Y, pound: true) && Main.netMode == 1)
			{
				NetMessage.SendData(17, -1, -1, null, 15, spot.X, spot.Y, 1f);
			}
		}

		private bool AlreadyLeadsIntoWantedTrack(Point tileCoordsOfFrontWheel, Point tileCoordsWeWantToReach)
		{
			Tile tileSafely = Framing.GetTileSafely(tileCoordsOfFrontWheel);
			Tile tileSafely2 = Framing.GetTileSafely(tileCoordsWeWantToReach);
			if (!tileSafely.active() || tileSafely.type != 314)
			{
				return false;
			}
			if (!tileSafely2.active() || tileSafely2.type != 314)
			{
				return false;
			}
			GetExpectedDirections(tileCoordsOfFrontWheel, tileCoordsWeWantToReach, out var expectedStartLeft, out var expectedStartRight, out var expectedEndLeft, out var expectedEndRight);
			if (!Minecart.GetAreExpectationsForSidesMet(tileCoordsOfFrontWheel, expectedStartLeft, expectedStartRight))
			{
				return false;
			}
			if (!Minecart.GetAreExpectationsForSidesMet(tileCoordsWeWantToReach, expectedEndLeft, expectedEndRight))
			{
				return false;
			}
			return true;
		}

		private static void GetExpectedDirections(Point startCoords, Point endCoords, out int? expectedStartLeft, out int? expectedStartRight, out int? expectedEndLeft, out int? expectedEndRight)
		{
			int num = endCoords.Y - startCoords.Y;
			int num2 = endCoords.X - startCoords.X;
			expectedStartLeft = null;
			expectedStartRight = null;
			expectedEndLeft = null;
			expectedEndRight = null;
			if (num2 == -1)
			{
				expectedStartLeft = num;
				expectedEndRight = -num;
			}
			if (num2 == 1)
			{
				expectedStartRight = num;
				expectedEndLeft = -num;
			}
		}

		private bool DoTheTracksConnectProperly(Point tileCoordsOfFrontWheel, Point tileCoordsWeWantToReach)
		{
			return AlreadyLeadsIntoWantedTrack(tileCoordsOfFrontWheel, tileCoordsWeWantToReach);
		}

		private void CorrectTrackConnections(Point startCoords, Point endCoords)
		{
			GetExpectedDirections(startCoords, endCoords, out var expectedStartLeft, out var expectedStartRight, out var expectedEndLeft, out var expectedEndRight);
			Tile tileSafely = Framing.GetTileSafely(startCoords);
			Tile tileSafely2 = Framing.GetTileSafely(endCoords);
			if (tileSafely.active() && tileSafely.type == 314)
			{
				Minecart.TryFittingTileOrientation(startCoords, expectedStartLeft, expectedStartRight);
			}
			if (tileSafely2.active() && tileSafely2.type == 314)
			{
				Minecart.TryFittingTileOrientation(endCoords, expectedEndLeft, expectedEndRight);
			}
		}

		private bool HasPickPower(Player player, int x, int y)
		{
			if (player.HasEnoughPickPowerToHurtTile(x, y))
			{
				return true;
			}
			return false;
		}

		private bool CanGetPastTile(int x, int y)
		{
			if (WorldGen.CheckTileBreakability(x, y) != 0)
			{
				return false;
			}
			Tile tile = Main.tile[x, y];
			if (tile.active() && TileID.Sets.Falling[tile.type])
			{
				return false;
			}
			if (tile.active() && !WorldGen.CanKillTile(x, y))
			{
				return false;
			}
			return true;
		}

		private void PlaceATrack(int x, int y)
		{
			int num = 314;
			int num2 = 0;
			if (WorldGen.PlaceTile(x, y, num, mute: false, forced: false, Main.myPlayer, num2))
			{
				NetMessage.SendData(17, -1, -1, null, 1, x, y, num, num2);
			}
		}

		private void MineTheTileIfNecessary(int x, int y)
		{
			AchievementsHelper.CurrentlyMining = true;
			if (Main.tile[x, y].active())
			{
				WorldGen.KillTile(x, y);
				NetMessage.SendData(17, -1, -1, null, 0, x, y);
			}
			AchievementsHelper.CurrentlyMining = false;
		}
	}
}
