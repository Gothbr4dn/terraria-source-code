namespace Terraria.DataStructures
{
	public struct PlayerMovementAccsCache
	{
		private bool _readyToPaste;

		private bool _mountPreventedFlight;

		private bool _mountPreventedExtraJumps;

		private int rocketTime;

		private float wingTime;

		private int rocketDelay;

		private int rocketDelay2;

		private bool jumpAgainCloud;

		private bool jumpAgainSandstorm;

		private bool jumpAgainBlizzard;

		private bool jumpAgainFart;

		private bool jumpAgainSail;

		private bool jumpAgainUnicorn;

		public void CopyFrom(Player player)
		{
			if (!_readyToPaste)
			{
				_readyToPaste = true;
				_mountPreventedFlight = true;
				_mountPreventedExtraJumps = player.mount.BlockExtraJumps;
				rocketTime = player.rocketTime;
				rocketDelay = player.rocketDelay;
				rocketDelay2 = player.rocketDelay2;
				wingTime = player.wingTime;
				jumpAgainCloud = player.canJumpAgain_Cloud;
				jumpAgainSandstorm = player.canJumpAgain_Sandstorm;
				jumpAgainBlizzard = player.canJumpAgain_Blizzard;
				jumpAgainFart = player.canJumpAgain_Fart;
				jumpAgainSail = player.canJumpAgain_Sail;
				jumpAgainUnicorn = player.canJumpAgain_Unicorn;
			}
		}

		public void PasteInto(Player player)
		{
			if (_readyToPaste)
			{
				_readyToPaste = false;
				if (_mountPreventedFlight)
				{
					player.rocketTime = rocketTime;
					player.rocketDelay = rocketDelay;
					player.rocketDelay2 = rocketDelay2;
					player.wingTime = wingTime;
				}
				if (_mountPreventedExtraJumps)
				{
					player.canJumpAgain_Cloud = jumpAgainCloud;
					player.canJumpAgain_Sandstorm = jumpAgainSandstorm;
					player.canJumpAgain_Blizzard = jumpAgainBlizzard;
					player.canJumpAgain_Fart = jumpAgainFart;
					player.canJumpAgain_Sail = jumpAgainSail;
					player.canJumpAgain_Unicorn = jumpAgainUnicorn;
				}
			}
		}
	}
}
