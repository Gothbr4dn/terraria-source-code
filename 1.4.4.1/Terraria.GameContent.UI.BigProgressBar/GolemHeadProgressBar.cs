using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar
{
	public class GolemHeadProgressBar : IBigProgressBar
	{
		private BigProgressBarCache _cache;

		private NPC _referenceDummy;

		private HashSet<int> ValidIds = new HashSet<int> { 246, 245 };

		public GolemHeadProgressBar()
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
			if (!nPC.active && !TryFindingAnotherGolemPiece(ref info))
			{
				return false;
			}
			int num = 0;
			_referenceDummy.SetDefaults(245, nPC.GetMatchingSpawnParams());
			num += _referenceDummy.lifeMax;
			_referenceDummy.SetDefaults(246, nPC.GetMatchingSpawnParams());
			num += _referenceDummy.lifeMax;
			float num2 = 0f;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC2 = Main.npc[i];
				if (nPC2.active && ValidIds.Contains(nPC2.type))
				{
					num2 += (float)nPC2.life;
				}
			}
			_cache.SetLife(num2, num);
			return true;
		}

		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[246];
			Texture2D value = TextureAssets.NpcHeadBoss[num].get_Value();
			Rectangle barIconFrame = value.Frame();
			BigProgressBarHelper.DrawFancyBar(spriteBatch, _cache.LifeCurrent, _cache.LifeMax, value, barIconFrame);
		}

		private bool TryFindingAnotherGolemPiece(ref BigProgressBarInfo info)
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
