using Terraria.UI;

namespace Terraria.DataStructures
{
	public interface IEntryFilter<T>
	{
		bool FitsFilter(T entry);

		string GetDisplayNameKey();

		UIElement GetImage();
	}
}
