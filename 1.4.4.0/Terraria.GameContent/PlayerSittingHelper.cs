using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent
{
	public struct PlayerSittingHelper
	{
		public const int ChairSittingMaxDistance = 40;

		public bool isSitting;

		public ExtraSeatInfo details;

		public Vector2 offsetForSeat;

		public int sittingIndex;

		public void GetSittingOffsetInfo(Player player, out Vector2 posOffset, out float seatAdjustment)
		{
			if (isSitting)
			{
				posOffset = new Vector2(sittingIndex * player.direction * 8, (float)sittingIndex * player.gravDir * -4f);
				seatAdjustment = -4f;
				seatAdjustment += (int)offsetForSeat.Y;
				posOffset += offsetForSeat * player.Directions;
			}
			else
			{
				posOffset = Vector2.Zero;
				seatAdjustment = 0f;
			}
		}

		public bool TryGetSittingBlock(Player player, out Tile tile)
		{
			tile = null;
			if (!isSitting)
			{
				return false;
			}
			Point pt = (player.Bottom + new Vector2(0f, -2f)).ToTileCoordinates();
			if (!GetSittingTargetInfo(player, pt.X, pt.Y, out var _, out var _, out var _, out var _))
			{
				return false;
			}
			tile = Framing.GetTileSafely(pt);
			return true;
		}

		public void UpdateSitting(Player player)
		{
			if (!isSitting)
			{
				return;
			}
			Point coords = (player.Bottom + new Vector2(0f, -2f)).ToTileCoordinates();
			if (!GetSittingTargetInfo(player, coords.X, coords.Y, out var targetDirection, out var _, out var seatDownOffset, out var extraInfo))
			{
				SitUp(player);
				return;
			}
			if (player.controlLeft || player.controlRight || player.controlUp || player.controlDown || player.controlJump || player.pulley || player.mount.Active || targetDirection != player.direction)
			{
				SitUp(player);
			}
			if (Main.sittingManager.GetNextPlayerStackIndexInCoords(coords) >= 2)
			{
				SitUp(player);
			}
			if (isSitting)
			{
				offsetForSeat = seatDownOffset;
				details = extraInfo;
				Main.sittingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, coords, out sittingIndex);
			}
		}

		public void SitUp(Player player, bool multiplayerBroadcast = true)
		{
			if (isSitting)
			{
				isSitting = false;
				offsetForSeat = Vector2.Zero;
				sittingIndex = -1;
				details = default(ExtraSeatInfo);
				if (multiplayerBroadcast && Main.myPlayer == player.whoAmI)
				{
					NetMessage.SendData(13, -1, -1, null, player.whoAmI);
				}
			}
		}

		public void SitDown(Player player, int x, int y)
		{
			if (!GetSittingTargetInfo(player, x, y, out var targetDirection, out var playerSittingPosition, out var seatDownOffset, out var extraInfo))
			{
				return;
			}
			Vector2 offset = playerSittingPosition - player.Bottom;
			bool flag = player.CanSnapToPosition(offset);
			if (flag)
			{
				flag &= Main.sittingManager.GetNextPlayerStackIndexInCoords((playerSittingPosition + new Vector2(0f, -2f)).ToTileCoordinates()) < 2;
			}
			if (!flag)
			{
				return;
			}
			if (isSitting && player.Bottom == playerSittingPosition)
			{
				SitUp(player);
				return;
			}
			player.StopVanityActions();
			player.RemoveAllGrapplingHooks();
			if (player.mount.Active)
			{
				player.mount.Dismount(player);
			}
			player.Bottom = playerSittingPosition;
			player.ChangeDir(targetDirection);
			isSitting = true;
			details = extraInfo;
			offsetForSeat = seatDownOffset;
			Main.sittingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, new Point(x, y), out sittingIndex);
			player.velocity = Vector2.Zero;
			player.gravDir = 1f;
			if (Main.myPlayer == player.whoAmI)
			{
				NetMessage.SendData(13, -1, -1, null, player.whoAmI);
			}
		}

		public static bool GetSittingTargetInfo(Player player, int x, int y, out int targetDirection, out Vector2 playerSittingPosition, out Vector2 seatDownOffset, out ExtraSeatInfo extraInfo)
		{
			extraInfo = default(ExtraSeatInfo);
			Tile tileSafely = Framing.GetTileSafely(x, y);
			if (!TileID.Sets.CanBeSatOnForPlayers[tileSafely.type] || !tileSafely.active())
			{
				targetDirection = 1;
				seatDownOffset = Vector2.Zero;
				playerSittingPosition = default(Vector2);
				return false;
			}
			int num = x;
			int num2 = y;
			targetDirection = 1;
			seatDownOffset = Vector2.Zero;
			int num3 = 6;
			Vector2 zero = Vector2.Zero;
			switch (tileSafely.type)
			{
			case 15:
			case 497:
			{
				bool num6 = tileSafely.type == 15 && tileSafely.frameY / 40 == 1;
				bool value = tileSafely.type == 15 && tileSafely.frameY / 40 == 27;
				seatDownOffset.Y = value.ToInt() * 4;
				if (tileSafely.frameY % 40 != 0)
				{
					num2--;
				}
				targetDirection = -1;
				if (tileSafely.frameX != 0)
				{
					targetDirection = 1;
				}
				if (num6 || tileSafely.type == 497)
				{
					extraInfo.IsAToilet = true;
				}
				break;
			}
			case 102:
			{
				int num4 = tileSafely.frameX / 18;
				if (num4 == 0)
				{
					num++;
				}
				if (num4 == 2)
				{
					num--;
				}
				int num5 = tileSafely.frameY / 18;
				if (num5 == 0)
				{
					num2 += 2;
				}
				if (num5 == 1)
				{
					num2++;
				}
				if (num5 == 3)
				{
					num2--;
				}
				targetDirection = player.direction;
				num3 = 0;
				break;
			}
			case 487:
			{
				int num7 = tileSafely.frameX % 72 / 18;
				if (num7 == 1)
				{
					num--;
				}
				if (num7 == 2)
				{
					num++;
				}
				if (tileSafely.frameY / 18 != 0)
				{
					num2--;
				}
				targetDirection = (num7 <= 1).ToDirectionInt();
				num3 = 0;
				seatDownOffset.Y -= 1f;
				break;
			}
			case 89:
			{
				targetDirection = player.direction;
				num3 = 0;
				Vector2 vector = new Vector2(-4f, 2f);
				Vector2 vector2 = new Vector2(4f, 2f);
				Vector2 vector3 = new Vector2(0f, 2f);
				Vector2 zero2 = Vector2.Zero;
				zero2.X = 1f;
				zero.X = -1f;
				switch (tileSafely.frameX / 54)
				{
				case 0:
					vector3.Y = (vector.Y = (vector2.Y = 1f));
					break;
				case 1:
					vector3.Y = 1f;
					break;
				case 2:
				case 14:
				case 15:
				case 17:
				case 20:
				case 21:
				case 22:
				case 23:
				case 25:
				case 26:
				case 27:
				case 28:
				case 35:
				case 37:
				case 38:
				case 39:
				case 40:
				case 41:
				case 42:
					vector3.Y = (vector.Y = (vector2.Y = 1f));
					break;
				case 3:
				case 4:
				case 5:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 16:
				case 18:
				case 19:
				case 36:
					vector3.Y = (vector.Y = (vector2.Y = 0f));
					break;
				case 6:
					vector3.Y = (vector.Y = (vector2.Y = -1f));
					break;
				case 24:
					vector3.Y = 0f;
					vector.Y = -4f;
					vector.X = 0f;
					vector2.X = 0f;
					vector2.Y = -4f;
					break;
				}
				if (tileSafely.frameY % 40 != 0)
				{
					num2--;
				}
				if ((tileSafely.frameX % 54 == 0 && targetDirection == -1) || (tileSafely.frameX % 54 == 36 && targetDirection == 1))
				{
					seatDownOffset = vector;
				}
				else if ((tileSafely.frameX % 54 == 0 && targetDirection == 1) || (tileSafely.frameX % 54 == 36 && targetDirection == -1))
				{
					seatDownOffset = vector2;
				}
				else
				{
					seatDownOffset = vector3;
				}
				seatDownOffset += zero2;
				break;
			}
			}
			playerSittingPosition = new Point(num, num2 + 1).ToWorldCoordinates(8f, 16f);
			playerSittingPosition.X += targetDirection * num3;
			playerSittingPosition += zero;
			return true;
		}
	}
}
