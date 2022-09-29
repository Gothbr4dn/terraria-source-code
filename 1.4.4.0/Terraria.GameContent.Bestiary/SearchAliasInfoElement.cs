using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class SearchAliasInfoElement : IBestiaryInfoElement, IProvideSearchFilterString
	{
		private readonly string _alias;

		public SearchAliasInfoElement(string alias)
		{
			_alias = alias;
		}

		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			return _alias;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}
	}
}
