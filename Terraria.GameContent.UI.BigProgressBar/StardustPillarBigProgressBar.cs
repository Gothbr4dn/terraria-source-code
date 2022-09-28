namespace Terraria.GameContent.UI.BigProgressBar
{
	public class StardustPillarBigProgressBar : LunarPillarBigProgessBar
	{
		internal override float GetCurrentShieldValue()
		{
			return NPC.ShieldStrengthTowerStardust;
		}

		internal override float GetMaxShieldValue()
		{
			return NPC.ShieldStrengthTowerMax;
		}

		internal override bool IsPlayerInCombatArea()
		{
			return Main.LocalPlayer.ZoneTowerStardust;
		}
	}
}
