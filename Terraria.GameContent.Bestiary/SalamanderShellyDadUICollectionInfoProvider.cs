using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class SalamanderShellyDadUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		private string _persistentIdentifierToCheck;

		private int _killCountNeededToFullyUnlock;

		public SalamanderShellyDadUICollectionInfoProvider(string persistentId)
		{
			_persistentIdentifierToCheck = persistentId;
			_killCountNeededToFullyUnlock = CommonEnemyUICollectionInfoProvider.GetKillCountNeeded(persistentId);
		}

		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			BestiaryEntryUnlockState bestiaryEntryUnlockState = CommonEnemyUICollectionInfoProvider.GetUnlockStateByKillCount(Main.BestiaryTracker.Kills.GetKillCount(_persistentIdentifierToCheck), quickUnlock: false, _killCountNeededToFullyUnlock);
			if (!IsIncludedInCurrentWorld())
			{
				bestiaryEntryUnlockState = GetLowestAvailableUnlockStateFromEntriesThatAreInWorld(bestiaryEntryUnlockState);
			}
			BestiaryUICollectionInfo result = default(BestiaryUICollectionInfo);
			result.UnlockState = bestiaryEntryUnlockState;
			return result;
		}

		private BestiaryEntryUnlockState GetLowestAvailableUnlockStateFromEntriesThatAreInWorld(BestiaryEntryUnlockState unlockstatus)
		{
			BestiaryEntryUnlockState bestiaryEntryUnlockState = BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
			int[,] cavernMonsterType = NPC.cavernMonsterType;
			for (int i = 0; i < cavernMonsterType.GetLength(0); i++)
			{
				for (int j = 0; j < cavernMonsterType.GetLength(1); j++)
				{
					string persistentId = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[cavernMonsterType[i, j]];
					BestiaryEntryUnlockState unlockStateByKillCount = CommonEnemyUICollectionInfoProvider.GetUnlockStateByKillCount(Main.BestiaryTracker.Kills.GetKillCount(persistentId), quickUnlock: false, _killCountNeededToFullyUnlock);
					if (bestiaryEntryUnlockState > unlockStateByKillCount)
					{
						bestiaryEntryUnlockState = unlockStateByKillCount;
					}
				}
			}
			unlockstatus = bestiaryEntryUnlockState;
			return unlockstatus;
		}

		private bool IsIncludedInCurrentWorld()
		{
			_ = ContentSamples.NpcNetIdsByPersistentIds[_persistentIdentifierToCheck];
			int[,] cavernMonsterType = NPC.cavernMonsterType;
			for (int i = 0; i < cavernMonsterType.GetLength(0); i++)
			{
				for (int j = 0; j < cavernMonsterType.GetLength(1); j++)
				{
					if (ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[cavernMonsterType[i, j]] == _persistentIdentifierToCheck)
					{
						return true;
					}
				}
			}
			return false;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}
	}
}
