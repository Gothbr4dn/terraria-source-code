using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	public class PotionOfReturnHelper
	{
		public static bool TryGetGateHitbox(Player player, out Rectangle homeHitbox)
		{
			homeHitbox = Rectangle.Empty;
			if (!player.PotionOfReturnHomePosition.HasValue)
			{
				return false;
			}
			Vector2 vector = new Vector2(0f, -player.height / 2);
			Vector2 center = player.PotionOfReturnHomePosition.Value + vector;
			homeHitbox = Utils.CenteredRectangle(center, new Vector2(24f, 40f));
			return true;
		}
	}
}
