using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class CommonEnemyUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		private string _persistentIdentifierToCheck;

		private bool _quickUnlock;

		private int _killCountNeededToFullyUnlock;

		public CommonEnemyUICollectionInfoProvider(string persistentId, bool quickUnlock)
		{
			_persistentIdentifierToCheck = persistentId;
			_quickUnlock = quickUnlock;
			_killCountNeededToFullyUnlock = GetKillCountNeeded(persistentId);
		}

		public static int GetKillCountNeeded(string persistentId)
		{
			int defaultKillsForBannerNeeded = ItemID.Sets.DefaultKillsForBannerNeeded;
			if (!ContentSamples.NpcNetIdsByPersistentIds.TryGetValue(persistentId, out var value))
			{
				return defaultKillsForBannerNeeded;
			}
			if (!ContentSamples.NpcsByNetId.TryGetValue(value, out var value2))
			{
				return defaultKillsForBannerNeeded;
			}
			int num = Item.BannerToItem(Item.NPCtoBanner(value2.BannerID()));
			return ItemID.Sets.KillsToBanner[num];
		}

		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			int killCount = Main.BestiaryTracker.Kills.GetKillCount(_persistentIdentifierToCheck);
			BestiaryEntryUnlockState unlockStateByKillCount = GetUnlockStateByKillCount(killCount, _quickUnlock);
			BestiaryUICollectionInfo result = default(BestiaryUICollectionInfo);
			result.UnlockState = unlockStateByKillCount;
			return result;
		}

		public BestiaryEntryUnlockState GetUnlockStateByKillCount(int killCount, bool quickUnlock)
		{
			int killCountNeededToFullyUnlock = _killCountNeededToFullyUnlock;
			return GetUnlockStateByKillCount(killCount, quickUnlock, killCountNeededToFullyUnlock);
		}

		public static BestiaryEntryUnlockState GetUnlockStateByKillCount(int killCount, bool quickUnlock, int fullKillCountNeeded)
		{
			BestiaryEntryUnlockState bestiaryEntryUnlockState = BestiaryEntryUnlockState.NotKnownAtAll_0;
			int num = fullKillCountNeeded / 2;
			int num2 = fullKillCountNeeded / 5;
			if (quickUnlock && killCount > 0)
			{
				return BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
			}
			if (killCount >= fullKillCountNeeded)
			{
				return BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
			}
			if (killCount >= num)
			{
				return BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3;
			}
			if (killCount >= num2)
			{
				return BestiaryEntryUnlockState.CanShowStats_2;
			}
			if (killCount >= 1)
			{
				return BestiaryEntryUnlockState.CanShowPortraitOnly_1;
			}
			return BestiaryEntryUnlockState.NotKnownAtAll_0;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}
	}
}
