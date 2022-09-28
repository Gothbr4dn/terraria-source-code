using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	public class TwinsBigProgressBar : IBigProgressBar
	{
		private BigProgressBarCache _cache;

		private int _headIndex;

		public bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info)
		{
			if (info.npcIndexToAimAt < 0 || info.npcIndexToAimAt > 200)
			{
				return false;
			}
			NPC nPC = Main.npc[info.npcIndexToAimAt];
			if (!nPC.active)
			{
				return false;
			}
			int num = ((nPC.type == 126) ? 125 : 126);
			int num2 = nPC.lifeMax;
			int num3 = nPC.life;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC2 = Main.npc[i];
				if (nPC2.active && nPC2.type == num)
				{
					num2 += nPC2.lifeMax;
					num3 += nPC2.life;
					break;
				}
			}
			_cache.SetLife(num3, num2);
			_headIndex = nPC.GetBossHeadTextureIndex();
			return true;
		}

		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			Texture2D value = TextureAssets.NpcHeadBoss[_headIndex].get_Value();
			Rectangle barIconFrame = value.Frame();
			BigProgressBarHelper.DrawFancyBar(spriteBatch, _cache.LifeCurrent, _cache.LifeMax, value, barIconFrame);
		}
	}
}
