using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	public class ProjectileSmartInteractCandidateProvider : ISmartInteractCandidateProvider
	{
		private class ReusableCandidate : ISmartInteractCandidate
		{
			private int _projectileIndexToTarget;

			public float DistanceFromCursor { get; private set; }

			public void WinCandidacy()
			{
				Main.SmartInteractProj = _projectileIndexToTarget;
				Main.SmartInteractShowingGenuine = true;
			}

			public void Reuse(int projectileIndex, float projectileDistanceFromCursor)
			{
				_projectileIndexToTarget = projectileIndex;
				DistanceFromCursor = projectileDistanceFromCursor;
			}
		}

		private ReusableCandidate _candidate = new ReusableCandidate();

		public void ClearSelfAndPrepareForCheck()
		{
			Main.SmartInteractProj = -1;
		}

		public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
		{
			candidate = null;
			if (!settings.FullInteraction)
			{
				return false;
			}
			List<int> listOfProjectilesToInteractWithHack = settings.player.GetListOfProjectilesToInteractWithHack();
			bool flag = false;
			Vector2 mousevec = settings.mousevec;
			mousevec.ToPoint();
			int num = -1;
			float projectileDistanceFromCursor = -1f;
			for (int i = 0; i < listOfProjectilesToInteractWithHack.Count; i++)
			{
				int num2 = listOfProjectilesToInteractWithHack[i];
				Projectile projectile = Main.projectile[num2];
				if (projectile.active)
				{
					float num3 = projectile.Hitbox.Distance(mousevec);
					if (num == -1 || Main.projectile[num].Hitbox.Distance(mousevec) > num3)
					{
						num = num2;
						projectileDistanceFromCursor = num3;
					}
					if (num3 == 0f)
					{
						flag = true;
						num = num2;
						projectileDistanceFromCursor = num3;
						break;
					}
				}
			}
			if (settings.DemandOnlyZeroDistanceTargets && !flag)
			{
				return false;
			}
			if (num != -1)
			{
				_candidate.Reuse(num, projectileDistanceFromCursor);
				candidate = _candidate;
				return true;
			}
			return false;
		}
	}
}
