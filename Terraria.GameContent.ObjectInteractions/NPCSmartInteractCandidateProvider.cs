using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	public class NPCSmartInteractCandidateProvider : ISmartInteractCandidateProvider
	{
		private class ReusableCandidate : ISmartInteractCandidate
		{
			private int _npcIndexToTarget;

			public float DistanceFromCursor { get; private set; }

			public void WinCandidacy()
			{
				Main.SmartInteractNPC = _npcIndexToTarget;
				Main.SmartInteractShowingGenuine = true;
			}

			public void Reuse(int npcIndex, float npcDistanceFromCursor)
			{
				_npcIndexToTarget = npcIndex;
				DistanceFromCursor = npcDistanceFromCursor;
			}
		}

		private ReusableCandidate _candidate = new ReusableCandidate();

		public void ClearSelfAndPrepareForCheck()
		{
			Main.SmartInteractNPC = -1;
		}

		public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
		{
			candidate = null;
			if (!settings.FullInteraction)
			{
				return false;
			}
			Rectangle value = Utils.CenteredRectangle(settings.player.Center, new Vector2(Player.tileRangeX, Player.tileRangeY) * 16f * 2f);
			Vector2 mousevec = settings.mousevec;
			mousevec.ToPoint();
			bool flag = false;
			int num = -1;
			float npcDistanceFromCursor = -1f;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.active && nPC.townNPC && nPC.Hitbox.Intersects(value) && !flag)
				{
					float num2 = nPC.Hitbox.Distance(mousevec);
					if (num == -1 || Main.npc[num].Hitbox.Distance(mousevec) > num2)
					{
						num = i;
						npcDistanceFromCursor = num2;
					}
					if (num2 == 0f)
					{
						flag = true;
						num = i;
						npcDistanceFromCursor = num2;
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
				_candidate.Reuse(num, npcDistanceFromCursor);
				candidate = _candidate;
				return true;
			}
			return false;
		}
	}
}
