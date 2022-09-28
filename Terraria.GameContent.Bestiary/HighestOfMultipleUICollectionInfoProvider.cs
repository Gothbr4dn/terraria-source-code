using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class HighestOfMultipleUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		private IBestiaryUICollectionInfoProvider[] _providers;

		private int _mainProviderIndex;

		public HighestOfMultipleUICollectionInfoProvider(params IBestiaryUICollectionInfoProvider[] providers)
		{
			_providers = providers;
			_mainProviderIndex = 0;
		}

		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			BestiaryUICollectionInfo entryUICollectionInfo = _providers[_mainProviderIndex].GetEntryUICollectionInfo();
			BestiaryEntryUnlockState unlockState = entryUICollectionInfo.UnlockState;
			for (int i = 0; i < _providers.Length; i++)
			{
				BestiaryUICollectionInfo entryUICollectionInfo2 = _providers[i].GetEntryUICollectionInfo();
				if (unlockState < entryUICollectionInfo2.UnlockState)
				{
					unlockState = entryUICollectionInfo2.UnlockState;
				}
			}
			entryUICollectionInfo.UnlockState = unlockState;
			return entryUICollectionInfo;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}
	}
}
