using Terraria.GameContent.Skies;
using Terraria.Graphics.Effects;

namespace Terraria.GameContent.Events
{
	public class CreditsRollEvent
	{
		private const int MAX_TIME_FOR_CREDITS_ROLL_IN_FRAMES = 28800;

		private static int _creditsRollRemainingTime;

		public static bool IsEventOngoing => _creditsRollRemainingTime > 0;

		public static void TryStartingCreditsRoll()
		{
			_creditsRollRemainingTime = 28800;
			if (SkyManager.Instance["CreditsRoll"] is CreditsRollSky creditsRollSky)
			{
				_creditsRollRemainingTime = creditsRollSky.AmountOfTimeNeededForFullPlay;
			}
			if (Main.netMode == 2)
			{
				NetMessage.SendData(140, -1, -1, null, 0, _creditsRollRemainingTime);
			}
		}

		public static void SendCreditsRollRemainingTimeToPlayer(int playerIndex)
		{
			if (_creditsRollRemainingTime != 0 && Main.netMode == 2)
			{
				NetMessage.SendData(140, playerIndex, -1, null, 0, _creditsRollRemainingTime);
			}
		}

		public static void UpdateTime()
		{
			_creditsRollRemainingTime = Utils.Clamp(_creditsRollRemainingTime - 1, 0, 28800);
		}

		public static void Reset()
		{
			_creditsRollRemainingTime = 0;
		}

		public static void SetRemainingTimeDirect(int time)
		{
			_creditsRollRemainingTime = Utils.Clamp(time, 0, 28800);
		}
	}
}
