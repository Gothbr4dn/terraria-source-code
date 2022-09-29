using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class NamePlateInfoElement : IBestiaryInfoElement, IProvideSearchFilterString
	{
		private string _key;

		private int _npcNetId;

		public NamePlateInfoElement(string languageKey, int npcNetId)
		{
			_key = languageKey;
			_npcNetId = npcNetId;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			UIElement uIElement = ((info.UnlockState != 0) ? new UIText(Language.GetText(_key)) : new UIText("???"));
			uIElement.HAlign = 0.5f;
			uIElement.VAlign = 0.5f;
			uIElement.Top = new StyleDimension(2f, 0f);
			uIElement.IgnoresMouseInteraction = true;
			UIElement uIElement2 = new UIElement();
			uIElement2.Width = new StyleDimension(0f, 1f);
			uIElement2.Height = new StyleDimension(24f, 0f);
			uIElement2.Append(uIElement);
			return uIElement2;
		}

		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			return Language.GetText(_key).Value;
		}
	}
}
