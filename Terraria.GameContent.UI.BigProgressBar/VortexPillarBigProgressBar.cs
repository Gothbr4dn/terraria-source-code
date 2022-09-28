namespace Terraria.GameContent.UI.BigProgressBar
{
	public class VortexPillarBigProgressBar : LunarPillarBigProgessBar
	{
		internal override float GetCurrentShieldValue()
		{
			return NPC.ShieldStrengthTowerVortex;
		}

		internal override float GetMaxShieldValue()
		{
			return NPC.ShieldStrengthTowerMax;
		}

		internal override bool IsPlayerInCombatArea()
		{
			return Main.LocalPlayer.ZoneTowerVortex;
		}
	}
}
