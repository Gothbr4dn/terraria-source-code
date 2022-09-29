using Terraria.UI;

namespace Terraria.GameContent.UI.States
{
	public class UISortableElement : UIElement
	{
		public int OrderIndex;

		public UISortableElement(int index)
		{
			OrderIndex = index;
		}

		public override int CompareTo(object obj)
		{
			if (obj is UISortableElement uISortableElement)
			{
				return OrderIndex.CompareTo(uISortableElement.OrderIndex);
			}
			return base.CompareTo(obj);
		}
	}
}
