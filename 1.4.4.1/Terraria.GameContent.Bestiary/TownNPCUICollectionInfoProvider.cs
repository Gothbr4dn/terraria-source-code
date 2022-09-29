using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class TownNPCUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		private string _persistentIdentifierToCheck;

		public TownNPCUICollectionInfoProvider(string persistentId)
		{
			_persistentIdentifierToCheck = persistentId;
		}

		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			BestiaryUICollectionInfo result = default(BestiaryUICollectionInfo);
			result.UnlockState = (Main.BestiaryTracker.Chats.GetWasChatWith(_persistentIdentifierToCheck) ? BestiaryEntryUnlockState.CanShowDropsWithDropRates_4 : BestiaryEntryUnlockState.NotKnownAtAll_0);
			return result;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}
	}
}
