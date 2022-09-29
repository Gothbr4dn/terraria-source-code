using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	public class PotionOfReturnSmartInteractCandidateProvider : ISmartInteractCandidateProvider
	{
		private class ReusableCandidate : ISmartInteractCandidate
		{
			public float DistanceFromCursor { get; private set; }

			public void WinCandidacy()
			{
				Main.SmartInteractPotionOfReturn = true;
				Main.SmartInteractShowingGenuine = true;
			}

			public void Reuse(float distanceFromCursor)
			{
				DistanceFromCursor = distanceFromCursor;
			}
		}

		private ReusableCandidate _candidate = new ReusableCandidate();

		public void ClearSelfAndPrepareForCheck()
		{
			Main.SmartInteractPotionOfReturn = false;
		}

		public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
		{
			candidate = null;
			if (!PotionOfReturnHelper.TryGetGateHitbox(settings.player, out var homeHitbox))
			{
				return false;
			}
			Vector2 vector = homeHitbox.ClosestPointInRect(settings.mousevec);
			float distanceFromCursor = vector.Distance(settings.mousevec);
			Point point = vector.ToTileCoordinates();
			if (point.X < settings.LX || point.X > settings.HX || point.Y < settings.LY || point.Y > settings.HY)
			{
				return false;
			}
			_candidate.Reuse(distanceFromCursor);
			candidate = _candidate;
			return true;
		}
	}
}
