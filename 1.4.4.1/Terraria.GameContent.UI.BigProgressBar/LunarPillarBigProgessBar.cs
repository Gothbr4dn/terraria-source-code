using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	public abstract class LunarPillarBigProgessBar : IBigProgressBar
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
			if (!IsPlayerInCombatArea())
			{
				return false;
			}
			if (nPC.ai[2] == 1f)
			{
				return false;
			}
			Utils.Clamp((float)nPC.life / (float)nPC.lifeMax, 0f, 1f);
			_ = (float)(int)MathHelper.Clamp(GetCurrentShieldValue(), 0f, GetMaxShieldValue()) / GetMaxShieldValue();
			_ = 600f * Main.GameModeInfo.EnemyMaxLifeMultiplier * GetMaxShieldValue() / (float)nPC.lifeMax;
			_cache.SetLife(nPC.life, nPC.lifeMax);
			_cache.SetShield(GetCurrentShieldValue(), GetMaxShieldValue());
			_headIndex = bossHeadTextureIndex;
			return true;
		}

		public void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch)
		{
			Texture2D value = TextureAssets.NpcHeadBoss[_headIndex].get_Value();
			Rectangle barIconFrame = value.Frame();
			BigProgressBarHelper.DrawFancyBar(spriteBatch, _cache.LifeCurrent, _cache.LifeMax, value, barIconFrame, _cache.ShieldCurrent, _cache.ShieldMax);
		}

		internal abstract float GetCurrentShieldValue();

		internal abstract float GetMaxShieldValue();

		internal abstract bool IsPlayerInCombatArea();
	}
}
