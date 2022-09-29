namespace Terraria.GameContent.UI.BigProgressBar
{
	public class SolarFlarePillarBigProgressBar : LunarPillarBigProgessBar
	{
		internal override float GetCurrentShieldValue()
		{
			return NPC.ShieldStrengthTowerSolar;
		}

		internal override float GetMaxShieldValue()
		{
			return NPC.ShieldStrengthTowerMax;
		}

		internal override bool IsPlayerInCombatArea()
		{
			return Main.LocalPlayer.ZoneTowerSolar;
		}
	}
}
