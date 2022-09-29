using Microsoft.Xna.Framework;

namespace Terraria.UI
{
	public class UIScrollWheelEvent : UIMouseEvent
	{
		public readonly int ScrollWheelValue;

		public UIScrollWheelEvent(UIElement target, Vector2 mousePosition, int scrollWheelValue)
			: base(target, mousePosition)
		{
			ScrollWheelValue = scrollWheelValue;
		}
	}
}
