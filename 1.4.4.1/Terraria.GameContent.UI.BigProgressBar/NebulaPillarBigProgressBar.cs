namespace Terraria.GameContent.UI.BigProgressBar
{
	public class NebulaPillarBigProgressBar : LunarPillarBigProgessBar
	{
		internal override float GetCurrentShieldValue()
		{
			return NPC.ShieldStrengthTowerNebula;
		}

		internal override float GetMaxShieldValue()
		{
			return NPC.ShieldStrengthTowerMax;
		}

		internal override bool IsPlayerInCombatArea()
		{
			return Main.LocalPlayer.ZoneTowerNebula;
		}
	}
}
