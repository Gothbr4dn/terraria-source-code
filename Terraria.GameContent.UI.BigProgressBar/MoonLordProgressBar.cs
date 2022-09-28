using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar
{
	public class MoonLordProgressBar : IBigProgressBar
	{
		private BigProgressBarCache _cache;

		private NPC _referenceDummy;

		private HashSet<int> ValidIds = new HashSet<int> { 396, 397, 398 };

		public MoonLordProgressBar()
		{
			_referenceDummy = new NPC();
		}

		public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
		{
			if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > 200)
			{
				return false;
			}
			NPC nPC = Main.npc[info.npcIndexToAimAt];
			if ((!nPC.active || IsInBadAI(nPC)) && !TryFindingAnotherMoonLordPiece(ref info))
			{
				return false;
			}
			int num = 0;
			NPCSpawnParams nPCSpawnParams = default(NPCSpawnParams);
			nPCSpawnParams.strengthMultiplierOverride = nPC.strengthMultiplier;
			nPCSpawnParams.playerCountForMultiplayerDifficultyOverride = nPC.statsAreScaledForThisManyPlayers;
			NPCSpawnParams spawnparams = nPCSpawnParams;
			_referenceDummy.SetDefaults(398, spawnparams);
			num += _referenceDummy.lifeMax;
			_referenceDummy.SetDefaults(396, spawnparams);
			num += _referenceDummy.lifeMax;
			_referenceDummy.SetDefaults(397, spawnparams);
			num += _referenceDummy.lifeMax;
			num += _referenceDummy.lifeMax;
			float num2 = 0f;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC2 = Main.npc[i];
				if (nPC2.active && ValidIds.Contains(nPC2.type) && !IsInBadAI(nPC2))
				{
					num2 += (float)nPC2.life;
				}
			}
			_cache.SetLife(num2, num);
			return true;
		}

		private bool IsInBadAI(NPC npc)
		{
			if (npc.type == 398 && (npc.ai[0] == 2f || npc.ai[0] == -1f))
			{
				return true;
			}
			if (npc.type == 398 && npc.localAI[3] == 0f)
			{
				return true;
			}
			if (npc.ai[0] == -2f || npc.ai[0] == -3f)
			{
				return true;
			}
			return false;
		}

		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[396];
			Texture2D value = TextureAssets.NpcHeadBoss[num].get_Value();
			Rectangle barIconFrame = value.Frame();
			BigProgressBarHelper.DrawFancyBar(spriteBatch, _cache.LifeCurrent, _cache.LifeMax, value, barIconFrame);
		}

		private bool TryFindingAnotherMoonLordPiece(ref BigProgressBarInfo info)
		{
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.active && ValidIds.Contains(nPC.type) && !IsInBadAI(nPC))
				{
					info.npcIndexToAimAt = i;
					return true;
				}
			}
			return false;
		}
	}
}
