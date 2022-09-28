using System;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent
{
	public struct PlayerSleepingHelper
	{
		public const int BedSleepingMaxDistance = 96;

		public const int TimeToFullyFallAsleep = 120;

		public bool isSleeping;

		public int sleepingIndex;

		public int timeSleeping;

		public Vector2 visualOffsetOfBedBase;

		public bool FullyFallenAsleep
		{
			get
			{
				if (isSleeping)
				{
					return timeSleeping >= 120;
				}
				return false;
			}
		}

		public void GetSleepingOffsetInfo(Player player, out Vector2 posOffset)
		{
			if (isSleeping)
			{
				posOffset = visualOffsetOfBedBase * player.Directions + new Vector2(0f, (float)sleepingIndex * player.gravDir * -4f);
			}
			else
			{
				posOffset = Vector2.Zero;
			}
		}

		private bool DoesPlayerHaveReasonToActUpInBed(Player player)
		{
			if (NPC.AnyDanger(quickBossNPCCheck: true))
			{
				return true;
			}
			if (Main.bloodMoon && !Main.dayTime)
			{
				return true;
			}
			if (Main.eclipse && Main.dayTime)
			{
				return true;
			}
			if (player.itemAnimation > 0)
			{
				return true;
			}
			return false;
		}

		public void SetIsSleepingAndAdjustPlayerRotation(Player player, bool state)
		{
			if (isSleeping != state)
			{
				isSleeping = state;
				if (state)
				{
					player.fullRotation = MathF.PI / 2f * (float)(-player.direction);
					return;
				}
				player.fullRotation = 0f;
				visualOffsetOfBedBase = default(Vector2);
			}
		}

		public void UpdateState(Player player)
		{
			if (!isSleeping)
			{
				timeSleeping = 0;
				return;
			}
			timeSleeping++;
			if (DoesPlayerHaveReasonToActUpInBed(player))
			{
				timeSleeping = 0;
			}
			Point coords = (player.Bottom + new Vector2(0f, -2f)).ToTileCoordinates();
			if (!GetSleepingTargetInfo(coords.X, coords.Y, out var targetDirection, out var _, out var visualoffset))
			{
				StopSleeping(player);
				return;
			}
			if (player.controlLeft || player.controlRight || player.controlUp || player.controlDown || player.controlJump || player.pulley || player.mount.Active || targetDirection != player.direction)
			{
				StopSleeping(player);
			}
			bool flag = false;
			if (player.itemAnimation > 0)
			{
				Item heldItem = player.HeldItem;
				if (heldItem.damage > 0 && !heldItem.noMelee)
				{
					flag = true;
				}
				if (heldItem.fishingPole > 0)
				{
					flag = true;
				}
				bool? flag2 = ItemID.Sets.ForcesBreaksSleeping[heldItem.type];
				if (flag2.HasValue)
				{
					flag = flag2.Value;
				}
			}
			if (flag)
			{
				StopSleeping(player);
			}
			if (Main.sleepingManager.GetNextPlayerStackIndexInCoords(coords) >= 2)
			{
				StopSleeping(player);
			}
			if (isSleeping)
			{
				visualOffsetOfBedBase = visualoffset;
				Main.sleepingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, coords, out sleepingIndex);
			}
		}

		public void StopSleeping(Player player, bool multiplayerBroadcast = true)
		{
			if (isSleeping)
			{
				SetIsSleepingAndAdjustPlayerRotation(player, state: false);
				timeSleeping = 0;
				sleepingIndex = -1;
				visualOffsetOfBedBase = default(Vector2);
				if (multiplayerBroadcast && Main.myPlayer == player.whoAmI)
				{
					NetMessage.SendData(13, -1, -1, null, player.whoAmI);
				}
			}
		}

		public void StartSleeping(Player player, int x, int y)
		{
			GetSleepingTargetInfo(x, y, out var targetDirection, out var anchorPosition, out var visualoffset);
			Vector2 offset = anchorPosition - player.Bottom;
			bool flag = player.CanSnapToPosition(offset);
			if (flag)
			{
				flag &= Main.sleepingManager.GetNextPlayerStackIndexInCoords((anchorPosition + new Vector2(0f, -2f)).ToTileCoordinates()) < 2;
			}
			if (!flag)
			{
				return;
			}
			if (isSleeping && player.Bottom == anchorPosition)
			{
				StopSleeping(player);
				return;
			}
			player.StopVanityActions();
			player.RemoveAllGrapplingHooks();
			player.RemoveAllFishingBobbers();
			if (player.mount.Active)
			{
				player.mount.Dismount(player);
			}
			player.Bottom = anchorPosition;
			player.ChangeDir(targetDirection);
			Main.sleepingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, new Point(x, y), out sleepingIndex);
			player.velocity = Vector2.Zero;
			player.gravDir = 1f;
			SetIsSleepingAndAdjustPlayerRotation(player, state: true);
			visualOffsetOfBedBase = visualoffset;
			if (Main.myPlayer == player.whoAmI)
			{
				NetMessage.SendData(13, -1, -1, null, player.whoAmI);
			}
		}

		public static bool GetSleepingTargetInfo(int x, int y, out int targetDirection, out Vector2 anchorPosition, out Vector2 visualoffset)
		{
			Tile tileSafely = Framing.GetTileSafely(x, y);
			if (!TileID.Sets.CanBeSleptIn[tileSafely.type] || !tileSafely.active())
			{
				targetDirection = 1;
				anchorPosition = default(Vector2);
				visualoffset = default(Vector2);
				return false;
			}
			int num = y;
			int num2 = x - tileSafely.frameX % 72 / 18;
			if (tileSafely.frameY % 36 != 0)
			{
				num--;
			}
			targetDirection = 1;
			int num3 = tileSafely.frameX / 72;
			int num4 = num2;
			switch (num3)
			{
			case 0:
				targetDirection = -1;
				num4++;
				break;
			case 1:
				num4 += 2;
				break;
			}
			anchorPosition = new Point(num4, num + 1).ToWorldCoordinates(8f, 16f);
			visualoffset = SetOffsetbyBed(tileSafely.frameY / 36);
			return true;
		}

		private static Vector2 SetOffsetbyBed(int bedStyle)
		{
			switch (bedStyle)
			{
			default:
				return new Vector2(-9f, 1f);
			case 8:
				return new Vector2(-11f, 1f);
			case 10:
				return new Vector2(-9f, -1f);
			case 11:
				return new Vector2(-11f, 1f);
			case 13:
				return new Vector2(-11f, -3f);
			case 15:
			case 16:
			case 17:
				return new Vector2(-7f, -3f);
			case 18:
				return new Vector2(-9f, -3f);
			case 19:
				return new Vector2(-3f, -1f);
			case 20:
				return new Vector2(-9f, -5f);
			case 21:
				return new Vector2(-9f, 5f);
			case 22:
				return new Vector2(-7f, 1f);
			case 23:
				return new Vector2(-5f, -1f);
			case 24:
			case 25:
				return new Vector2(-7f, 1f);
			case 27:
				return new Vector2(-9f, 3f);
			case 28:
				return new Vector2(-9f, 5f);
			case 29:
				return new Vector2(-11f, -1f);
			case 30:
				return new Vector2(-9f, 3f);
			case 31:
				return new Vector2(-7f, 5f);
			case 32:
				return new Vector2(-7f, -1f);
			case 34:
			case 35:
			case 36:
			case 37:
				return new Vector2(-13f, 1f);
			case 38:
				return new Vector2(-11f, -3f);
			}
		}
	}
}
