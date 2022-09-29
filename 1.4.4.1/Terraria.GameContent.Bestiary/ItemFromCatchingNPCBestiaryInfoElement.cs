using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class ItemFromCatchingNPCBestiaryInfoElement : IItemBestiaryInfoElement, IBestiaryInfoElement, IProvideSearchFilterString
	{
		private int _itemType;

		public ItemFromCatchingNPCBestiaryInfoElement(int itemId)
		{
			_itemType = itemId;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3)
			{
				return null;
			}
			return new UIBestiaryInfoLine<string>(("catch item #" + _itemType) ?? "");
		}

		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3)
			{
				return null;
			}
			return ContentSamples.ItemsByType[_itemType].Name;
		}
	}
}
