using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class BossBestiaryInfoElement : IBestiaryInfoElement, IProvideSearchFilterString
	{
		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState < BestiaryEntryUnlockState.CanShowPortraitOnly_1)
			{
				return null;
			}
			return Language.GetText("BestiaryInfo.IsBoss").Value;
		}
	}
}
