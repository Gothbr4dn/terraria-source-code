using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent
{
	public class DontStarveDarknessDamageDealer
	{
		public const int DARKNESS_DAMAGE_PER_HIT = 250;

		public const int DARKNESS_TIMER_MAX_BEFORE_STARTING_HITS = 120;

		public static int darknessTimer = -1;

		public const int DARKNESS_HIT_TIMER_MAX_BEFORE_HIT = 60;

		public const int DARKNESS_MESSAGE_TIME = 120;

		public static int darknessHitTimer = 0;

		public static bool saidMessage = false;

		public static bool lastFrameWasTooBright = true;

		public static void Reset()
		{
			ResetTimer();
			saidMessage = false;
			lastFrameWasTooBright = true;
		}

		private static void ResetTimer()
		{
			darknessTimer = -1;
			darknessHitTimer = 0;
		}

		public static void Update(Player player)
		{
			if (player.DeadOrGhost || Main.gameInactive || player.shimmering)
			{
				ResetTimer();
				return;
			}
			UpdateDarknessState(player);
			if (darknessTimer >= 120)
			{
				darknessTimer = 120;
				darknessHitTimer++;
				if (darknessHitTimer > 60 && !player.immune)
				{
					SoundEngine.PlaySound(SoundID.Item1, player.Center);
					player.Hurt(PlayerDeathReason.ByOther(17), 250, 0);
					darknessHitTimer = 0;
				}
			}
		}

		private static void UpdateDarknessState(Player player)
		{
			if (lastFrameWasTooBright = IsPlayerSafe(player))
			{
				if (saidMessage)
				{
					if (!Main.getGoodWorld)
					{
						Main.NewText(Language.GetTextValue("Game.DarknessSafe"), 50, 200, 50);
					}
					saidMessage = false;
				}
				ResetTimer();
				return;
			}
			if (darknessTimer >= 120 && !saidMessage)
			{
				if (!Main.getGoodWorld)
				{
					Main.NewText(Language.GetTextValue("Game.DarknessDanger"), 200, 50, 50);
				}
				saidMessage = true;
			}
			darknessTimer++;
		}

		private static bool IsPlayerSafe(Player player)
		{
			Vector3 vector = Lighting.GetColor((int)player.Center.X / 16, (int)player.Center.Y / 16).ToVector3();
			bool flag = true;
			if (Main.LocalGolfState != null && (Main.LocalGolfState.ShouldCameraTrackBallLastKnownLocation || Main.LocalGolfState.IsTrackingBall))
			{
				return lastFrameWasTooBright;
			}
			if (Main.DroneCameraTracker != null && Main.DroneCameraTracker.IsInUse())
			{
				return lastFrameWasTooBright;
			}
			return vector.Length() >= 0.1f;
		}
	}
}
