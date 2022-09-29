using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace Terraria.GameContent
{
	public class DoorOpeningHelper
	{
		public enum DoorAutoOpeningPreference
		{
			Disabled,
			EnabledForGamepadOnly,
			EnabledForEverything
		}

		private enum DoorCloseAttemptResult
		{
			StillInDoorArea,
			ClosedDoor,
			FailedToCloseDoor,
			DoorIsInvalidated
		}

		private struct DoorOpenCloseTogglingInfo
		{
			public Point tileCoordsForToggling;

			public DoorAutoHandler handler;
		}

		private struct PlayerInfoForOpeningDoors
		{
			public Rectangle hitboxToOpenDoor;

			public int intendedOpeningDirection;

			public int playerGravityDirection;

			public Rectangle tileCoordSpaceForCheckingForDoors;
		}

		private struct PlayerInfoForClosingDoors
		{
			public Rectangle hitboxToNotCloseDoor;
		}

		private interface DoorAutoHandler
		{
			DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords);

			bool TryOpenDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForOpeningDoors playerInfo);

			DoorCloseAttemptResult TryCloseDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForClosingDoors playerInfo);
		}

		private class CommonDoorOpeningInfoProvider : DoorAutoHandler
		{
			public DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords)
			{
				Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
				Point tileCoordsForToggling = tileCoords;
				tileCoordsForToggling.Y -= tile.frameY % 54 / 18;
				DoorOpenCloseTogglingInfo result = default(DoorOpenCloseTogglingInfo);
				result.handler = this;
				result.tileCoordsForToggling = tileCoordsForToggling;
				return result;
			}

			public bool TryOpenDoor(DoorOpenCloseTogglingInfo doorInfo, PlayerInfoForOpeningDoors playerInfo)
			{
				Point tileCoordsForToggling = doorInfo.tileCoordsForToggling;
				int intendedOpeningDirection = playerInfo.intendedOpeningDirection;
				Rectangle rectangle = new Rectangle(doorInfo.tileCoordsForToggling.X * 16, doorInfo.tileCoordsForToggling.Y * 16, 16, 48);
				switch (playerInfo.playerGravityDirection)
				{
				case 1:
					rectangle.Height += 16;
					break;
				case -1:
					rectangle.Y -= 16;
					rectangle.Height += 16;
					break;
				}
				if (!rectangle.Intersects(playerInfo.hitboxToOpenDoor))
				{
					return false;
				}
				if (playerInfo.hitboxToOpenDoor.Top < rectangle.Top || playerInfo.hitboxToOpenDoor.Bottom > rectangle.Bottom)
				{
					return false;
				}
				WorldGen.OpenDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y, intendedOpeningDirection);
				if (Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y].type != 10)
				{
					NetMessage.SendData(19, -1, -1, null, 0, tileCoordsForToggling.X, tileCoordsForToggling.Y, intendedOpeningDirection);
					return true;
				}
				WorldGen.OpenDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y, -intendedOpeningDirection);
				if (Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y].type != 10)
				{
					NetMessage.SendData(19, -1, -1, null, 0, tileCoordsForToggling.X, tileCoordsForToggling.Y, -intendedOpeningDirection);
					return true;
				}
				return false;
			}

			public DoorCloseAttemptResult TryCloseDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForClosingDoors playerInfo)
			{
				Point tileCoordsForToggling = info.tileCoordsForToggling;
				Tile tile = Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y];
				if (!tile.active() || tile.type != 11)
				{
					return DoorCloseAttemptResult.DoorIsInvalidated;
				}
				int num = tile.frameX % 72 / 18;
				Rectangle value = new Rectangle(tileCoordsForToggling.X * 16, tileCoordsForToggling.Y * 16, 16, 48);
				switch (num)
				{
				case 1:
					value.X -= 16;
					break;
				case 2:
					value.X += 16;
					break;
				}
				value.Inflate(1, 0);
				Rectangle rectangle = Rectangle.Intersect(value, playerInfo.hitboxToNotCloseDoor);
				if (rectangle.Width > 0 || rectangle.Height > 0)
				{
					return DoorCloseAttemptResult.StillInDoorArea;
				}
				if (WorldGen.CloseDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y))
				{
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
					NetMessage.SendData(19, -1, -1, null, 1, tileCoordsForToggling.X, tileCoordsForToggling.Y, 1f);
					return DoorCloseAttemptResult.ClosedDoor;
				}
				return DoorCloseAttemptResult.FailedToCloseDoor;
			}
		}

		private class TallGateOpeningInfoProvider : DoorAutoHandler
		{
			public DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords)
			{
				Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
				Point tileCoordsForToggling = tileCoords;
				tileCoordsForToggling.Y -= tile.frameY % 90 / 18;
				DoorOpenCloseTogglingInfo result = default(DoorOpenCloseTogglingInfo);
				result.handler = this;
				result.tileCoordsForToggling = tileCoordsForToggling;
				return result;
			}

			public bool TryOpenDoor(DoorOpenCloseTogglingInfo doorInfo, PlayerInfoForOpeningDoors playerInfo)
			{
				Point tileCoordsForToggling = doorInfo.tileCoordsForToggling;
				Rectangle rectangle = new Rectangle(doorInfo.tileCoordsForToggling.X * 16, doorInfo.tileCoordsForToggling.Y * 16, 16, 80);
				switch (playerInfo.playerGravityDirection)
				{
				case 1:
					rectangle.Height += 16;
					break;
				case -1:
					rectangle.Y -= 16;
					rectangle.Height += 16;
					break;
				}
				if (!rectangle.Intersects(playerInfo.hitboxToOpenDoor))
				{
					return false;
				}
				if (playerInfo.hitboxToOpenDoor.Top < rectangle.Top || playerInfo.hitboxToOpenDoor.Bottom > rectangle.Bottom)
				{
					return false;
				}
				bool flag = false;
				if (WorldGen.ShiftTallGate(tileCoordsForToggling.X, tileCoordsForToggling.Y, flag))
				{
					NetMessage.SendData(19, -1, -1, null, 4 + flag.ToInt(), tileCoordsForToggling.X, tileCoordsForToggling.Y);
					return true;
				}
				return false;
			}

			public DoorCloseAttemptResult TryCloseDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForClosingDoors playerInfo)
			{
				Point tileCoordsForToggling = info.tileCoordsForToggling;
				Tile tile = Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y];
				if (!tile.active() || tile.type != 389)
				{
					return DoorCloseAttemptResult.DoorIsInvalidated;
				}
				_ = tile.frameY % 90 / 18;
				Rectangle value = new Rectangle(tileCoordsForToggling.X * 16, tileCoordsForToggling.Y * 16, 16, 80);
				value.Inflate(1, 0);
				Rectangle rectangle = Rectangle.Intersect(value, playerInfo.hitboxToNotCloseDoor);
				if (rectangle.Width > 0 || rectangle.Height > 0)
				{
					return DoorCloseAttemptResult.StillInDoorArea;
				}
				bool flag = true;
				if (WorldGen.ShiftTallGate(tileCoordsForToggling.X, tileCoordsForToggling.Y, flag))
				{
					NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
					NetMessage.SendData(19, -1, -1, null, 4 + flag.ToInt(), tileCoordsForToggling.X, tileCoordsForToggling.Y);
					return DoorCloseAttemptResult.ClosedDoor;
				}
				return DoorCloseAttemptResult.FailedToCloseDoor;
			}
		}

		public static DoorAutoOpeningPreference PreferenceSettings = DoorAutoOpeningPreference.EnabledForEverything;

		private Dictionary<int, DoorAutoHandler> _handlerByTileType = new Dictionary<int, DoorAutoHandler>
		{
			{
				10,
				new CommonDoorOpeningInfoProvider()
			},
			{
				388,
				new TallGateOpeningInfoProvider()
			}
		};

		private List<DoorOpenCloseTogglingInfo> _ongoingOpenDoors = new List<DoorOpenCloseTogglingInfo>();

		private int _timeWeCanOpenDoorsUsingVelocityAlone;

		public void AllowOpeningDoorsByVelocityAloneForATime(int timeInFramesToAllow)
		{
			_timeWeCanOpenDoorsUsingVelocityAlone = timeInFramesToAllow;
		}

		public void Update(Player player)
		{
			LookForDoorsToClose(player);
			if (ShouldTryOpeningDoors())
			{
				LookForDoorsToOpen(player);
			}
			if (_timeWeCanOpenDoorsUsingVelocityAlone > 0)
			{
				_timeWeCanOpenDoorsUsingVelocityAlone--;
			}
		}

		private bool ShouldTryOpeningDoors()
		{
			return PreferenceSettings switch
			{
				DoorAutoOpeningPreference.EnabledForEverything => true, 
				DoorAutoOpeningPreference.EnabledForGamepadOnly => PlayerInput.UsingGamepad, 
				_ => false, 
			};
		}

		public static void CyclePreferences()
		{
			switch (PreferenceSettings)
			{
			case DoorAutoOpeningPreference.Disabled:
				PreferenceSettings = DoorAutoOpeningPreference.EnabledForEverything;
				break;
			case DoorAutoOpeningPreference.EnabledForEverything:
				PreferenceSettings = DoorAutoOpeningPreference.EnabledForGamepadOnly;
				break;
			case DoorAutoOpeningPreference.EnabledForGamepadOnly:
				PreferenceSettings = DoorAutoOpeningPreference.Disabled;
				break;
			}
		}

		public void LookForDoorsToClose(Player player)
		{
			PlayerInfoForClosingDoors playerInfoForClosingDoor = GetPlayerInfoForClosingDoor(player);
			for (int num = _ongoingOpenDoors.Count - 1; num >= 0; num--)
			{
				DoorOpenCloseTogglingInfo info = _ongoingOpenDoors[num];
				if (info.handler.TryCloseDoor(info, playerInfoForClosingDoor) != 0)
				{
					_ongoingOpenDoors.RemoveAt(num);
				}
			}
		}

		private PlayerInfoForClosingDoors GetPlayerInfoForClosingDoor(Player player)
		{
			PlayerInfoForClosingDoors result = default(PlayerInfoForClosingDoors);
			result.hitboxToNotCloseDoor = player.Hitbox;
			return result;
		}

		public void LookForDoorsToOpen(Player player)
		{
			PlayerInfoForOpeningDoors playerInfoForOpeningDoor = GetPlayerInfoForOpeningDoor(player);
			if (playerInfoForOpeningDoor.intendedOpeningDirection == 0 && player.velocity.X == 0f)
			{
				return;
			}
			Point tileCoords = default(Point);
			for (int i = playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Left; i <= playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Right; i++)
			{
				for (int j = playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Top; j <= playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors.Bottom; j++)
				{
					tileCoords.X = i;
					tileCoords.Y = j;
					TryAutoOpeningDoor(tileCoords, playerInfoForOpeningDoor);
				}
			}
		}

		private PlayerInfoForOpeningDoors GetPlayerInfoForOpeningDoor(Player player)
		{
			int num = player.controlRight.ToInt() - player.controlLeft.ToInt();
			int playerGravityDirection = (int)player.gravDir;
			Rectangle hitbox = player.Hitbox;
			hitbox.Y -= -1;
			hitbox.Height += -2;
			float num2 = player.velocity.X;
			if (num == 0 && _timeWeCanOpenDoorsUsingVelocityAlone == 0)
			{
				num2 = 0f;
			}
			float value = (float)num + num2;
			int num3 = Math.Sign(value) * (int)Math.Ceiling(Math.Abs(value));
			hitbox.X += num3;
			if (num == 0)
			{
				num = Math.Sign(value);
			}
			Rectangle hitbox2;
			Rectangle value2 = (hitbox2 = player.Hitbox);
			hitbox2.X += num3;
			Rectangle r = Rectangle.Union(value2, hitbox2);
			Point point = r.TopLeft().ToTileCoordinates();
			Point point2 = r.BottomRight().ToTileCoordinates();
			Rectangle tileCoordSpaceForCheckingForDoors = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
			PlayerInfoForOpeningDoors result = default(PlayerInfoForOpeningDoors);
			result.hitboxToOpenDoor = hitbox;
			result.intendedOpeningDirection = num;
			result.playerGravityDirection = playerGravityDirection;
			result.tileCoordSpaceForCheckingForDoors = tileCoordSpaceForCheckingForDoors;
			return result;
		}

		private void TryAutoOpeningDoor(Point tileCoords, PlayerInfoForOpeningDoors playerInfo)
		{
			if (TryGetHandler(tileCoords, out var infoProvider))
			{
				DoorOpenCloseTogglingInfo doorOpenCloseTogglingInfo = infoProvider.ProvideInfo(tileCoords);
				if (infoProvider.TryOpenDoor(doorOpenCloseTogglingInfo, playerInfo))
				{
					_ongoingOpenDoors.Add(doorOpenCloseTogglingInfo);
				}
			}
		}

		private bool TryGetHandler(Point tileCoords, out DoorAutoHandler infoProvider)
		{
			infoProvider = null;
			if (!WorldGen.InWorld(tileCoords.X, tileCoords.Y, 3))
			{
				return false;
			}
			Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
			if (tile == null)
			{
				return false;
			}
			if (!_handlerByTileType.TryGetValue(tile.type, out infoProvider))
			{
				return false;
			}
			return true;
		}
	}
}
