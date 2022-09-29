using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	public class DeerclopsBigProgressBar : IBigProgressBar
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
			int bossHeadTextureIndex = nPC.GetBossHeadTextureIndex();
			if (bossHeadTextureIndex == -1)
			{
				return false;
			}
			if (!NPC.IsDeerclopsHostile())
			{
				return false;
			}
			_cache.SetLife(nPC.life, nPC.lifeMax);
			_headIndex = bossHeadTextureIndex;
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
