using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public interface IFilterInfoProvider
	{
		UIElement GetFilterImage();

		string GetDisplayNameKey();
	}
}
