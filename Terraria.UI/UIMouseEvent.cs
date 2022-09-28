using Microsoft.Xna.Framework;

namespace Terraria.UI
{
	public class UIMouseEvent : UIEvent
	{
		public readonly Vector2 MousePosition;

		public UIMouseEvent(UIElement target, Vector2 mousePosition)
			: base(target)
		{
			MousePosition = mousePosition;
		}
	}
}
