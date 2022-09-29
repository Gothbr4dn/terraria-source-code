using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class ItemDropBestiaryInfoElement : IItemBestiaryInfoElement, IBestiaryInfoElement, IProvideSearchFilterString
	{
		protected DropRateInfo _droprateInfo;

		public ItemDropBestiaryInfoElement(DropRateInfo info)
		{
			_droprateInfo = info;
		}

		public virtual UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			bool flag = ShouldShowItem(ref _droprateInfo);
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowStats_2)
			{
				flag = false;
			}
			if (!flag)
			{
				return null;
			}
			return new UIBestiaryInfoItemLine(_droprateInfo, info);
		}

		private static bool ShouldShowItem(ref DropRateInfo dropRateInfo)
		{
			bool result = true;
			if (dropRateInfo.conditions != null && dropRateInfo.conditions.Count > 0)
			{
				for (int i = 0; i < dropRateInfo.conditions.Count; i++)
				{
					if (!dropRateInfo.conditions[i].CanShowItemDropInUI())
					{
						result = false;
						break;
					}
				}
			}
			return result;
		}

		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			bool flag = ShouldShowItem(ref _droprateInfo);
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowStats_2)
			{
				flag = false;
			}
			if (!flag)
			{
				return null;
			}
			return ContentSamples.ItemsByType[_droprateInfo.itemId].Name;
		}
	}
}
