using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class BestiaryPortraitBackgroundBasedOnWorldEvilProviderPreferenceInfoElement : IPreferenceProviderElement, IBestiaryInfoElement
	{
		private IBestiaryBackgroundImagePathAndColorProvider _preferredProviderCorrupt;

		private IBestiaryBackgroundImagePathAndColorProvider _preferredProviderCrimson;

		public BestiaryPortraitBackgroundBasedOnWorldEvilProviderPreferenceInfoElement(IBestiaryBackgroundImagePathAndColorProvider preferredProviderCorrupt, IBestiaryBackgroundImagePathAndColorProvider preferredProviderCrimson)
		{
			_preferredProviderCorrupt = preferredProviderCorrupt;
			_preferredProviderCrimson = preferredProviderCrimson;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}

		public bool Matches(IBestiaryBackgroundImagePathAndColorProvider provider)
		{
			if (Main.ActiveWorldFileData == null || !WorldGen.crimson)
			{
				return provider == _preferredProviderCorrupt;
			}
			return provider == _preferredProviderCrimson;
		}

		public IBestiaryBackgroundImagePathAndColorProvider GetPreferredProvider()
		{
			if (Main.ActiveWorldFileData == null || !WorldGen.crimson)
			{
				return _preferredProviderCorrupt;
			}
			return _preferredProviderCrimson;
		}
	}
}
