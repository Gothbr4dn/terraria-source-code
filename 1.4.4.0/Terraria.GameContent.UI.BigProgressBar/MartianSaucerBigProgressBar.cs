using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar
{
	public class MartianSaucerBigProgressBar : IBigProgressBar
	{
		private BigProgressBarCache _cache;

		private NPC _referenceDummy;

		private HashSet<int> ValidIds = new HashSet<int> { 395 };

		private HashSet<int> ValidIdsToScanHp = new HashSet<int> { 395, 393, 394 };

		public MartianSaucerBigProgressBar()
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
			if (!nPC.active || nPC.type != 395)
			{
				if (!TryFindingAnotherMartianSaucerPiece(ref info))
				{
					return false;
				}
				nPC = Main.npc[info.npcIndexToAimAt];
			}
			int num = 0;
			if (Main.expertMode)
			{
				_referenceDummy.SetDefaults(395, nPC.GetMatchingSpawnParams());
				num += _referenceDummy.lifeMax;
			}
			_referenceDummy.SetDefaults(394, nPC.GetMatchingSpawnParams());
			num += _referenceDummy.lifeMax * 2;
			_referenceDummy.SetDefaults(393, nPC.GetMatchingSpawnParams());
			num += _referenceDummy.lifeMax * 2;
			float num2 = 0f;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC2 = Main.npc[i];
				if (nPC2.active && ValidIdsToScanHp.Contains(nPC2.type) && (Main.expertMode || nPC2.type != 395))
				{
					num2 += (float)nPC2.life;
				}
			}
			_cache.SetLife(num2, num);
			return true;
		}

		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[395];
			Texture2D value = TextureAssets.NpcHeadBoss[num].get_Value();
			Rectangle barIconFrame = value.Frame();
			BigProgressBarHelper.DrawFancyBar(spriteBatch, _cache.LifeCurrent, _cache.LifeMax, value, barIconFrame);
		}

		private bool TryFindingAnotherMartianSaucerPiece(ref BigProgressBarInfo info)
		{
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				if (nPC.active && ValidIds.Contains(nPC.type))
				{
					info.npcIndexToAimAt = i;
					return true;
				}
			}
			return false;
		}
	}
}
