using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI.BigProgressBar
{
	public class PirateShipBigProgressBar : IBigProgressBar
	{
		private BigProgressBarCache _cache;

		private NPC _referenceDummy;

		private HashSet<int> ValidIds = new HashSet<int> { 491 };

		public PirateShipBigProgressBar()
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
			if (!nPC.active || nPC.type != 491)
			{
				if (!TryFindingAnotherPirateShipPiece(ref info))
				{
					return false;
				}
				nPC = Main.npc[info.npcIndexToAimAt];
			}
			int num = 0;
			_referenceDummy.SetDefaults(492, nPC.GetMatchingSpawnParams());
			num += _referenceDummy.lifeMax * 4;
			float num2 = 0f;
			for (int i = 0; i < 4; i++)
			{
				int num3 = (int)nPC.ai[i];
				if (Main.npc.IndexInRange(num3))
				{
					NPC nPC2 = Main.npc[num3];
					if (nPC2.active && nPC2.type == 492)
					{
						num2 += (float)nPC2.life;
					}
				}
			}
			_cache.SetLife(num2, num);
			return true;
		}

		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			int num = NPCID.Sets.BossHeadTextures[491];
			Texture2D value = TextureAssets.NpcHeadBoss[num].get_Value();
			Rectangle barIconFrame = value.Frame();
			BigProgressBarHelper.DrawFancyBar(spriteBatch, _cache.LifeCurrent, _cache.LifeMax, value, barIconFrame);
		}

		private bool TryFindingAnotherPirateShipPiece(ref BigProgressBarInfo info)
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
