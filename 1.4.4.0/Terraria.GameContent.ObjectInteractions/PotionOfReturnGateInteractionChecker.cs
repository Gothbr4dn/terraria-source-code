using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	public class PotionOfReturnGateInteractionChecker : AHoverInteractionChecker
	{
		internal override bool? AttemptOverridingHoverStatus(Player player, Rectangle rectangle)
		{
			if (Main.SmartInteractPotionOfReturn)
			{
				return true;
			}
			return null;
		}

		internal override void DoHoverEffect(Player player, Rectangle hitbox)
		{
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = 4870;
		}

		internal override bool ShouldBlockInteraction(Player player, Rectangle hitbox)
		{
			return Player.BlockInteractionWithProjectiles != 0;
		}

		internal override void PerformInteraction(Player player, Rectangle hitbox)
		{
			player.DoPotionOfReturnReturnToOriginalUsePosition();
		}
	}
}
