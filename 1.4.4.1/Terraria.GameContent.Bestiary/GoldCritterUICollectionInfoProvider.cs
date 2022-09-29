using Terraria.ID;

namespace Terraria.GameContent.Bestiary
{
	public class GoldCritterUICollectionInfoProvider : IBestiaryUICollectionInfoProvider
	{
		private string[] _normalCritterPersistentId;

		private string _goldCritterPersistentId;

		public GoldCritterUICollectionInfoProvider(int[] normalCritterPersistentId, string goldCritterPersistentId)
		{
			_normalCritterPersistentId = new string[normalCritterPersistentId.Length];
			for (int i = 0; i < normalCritterPersistentId.Length; i++)
			{
				_normalCritterPersistentId[i] = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[normalCritterPersistentId[i]];
			}
			_goldCritterPersistentId = goldCritterPersistentId;
		}

		public BestiaryUICollectionInfo GetEntryUICollectionInfo()
		{
			BestiaryEntryUnlockState unlockStateForCritter = GetUnlockStateForCritter(_goldCritterPersistentId);
			BestiaryEntryUnlockState bestiaryEntryUnlockState = BestiaryEntryUnlockState.NotKnownAtAll_0;
			if (unlockStateForCritter > bestiaryEntryUnlockState)
			{
				bestiaryEntryUnlockState = unlockStateForCritter;
			}
			string[] normalCritterPersistentId = _normalCritterPersistentId;
			foreach (string persistentId in normalCritterPersistentId)
			{
				BestiaryEntryUnlockState unlockStateForCritter2 = GetUnlockStateForCritter(persistentId);
				if (unlockStateForCritter2 > bestiaryEntryUnlockState)
				{
					bestiaryEntryUnlockState = unlockStateForCritter2;
				}
			}
			BestiaryUICollectionInfo bestiaryUICollectionInfo = default(BestiaryUICollectionInfo);
			bestiaryUICollectionInfo.UnlockState = bestiaryEntryUnlockState;
			BestiaryUICollectionInfo result = bestiaryUICollectionInfo;
			if (bestiaryEntryUnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return result;
			}
			if (!TryFindingOneGoldCritterThatIsAlreadyUnlocked())
			{
				bestiaryUICollectionInfo = default(BestiaryUICollectionInfo);
				bestiaryUICollectionInfo.UnlockState = BestiaryEntryUnlockState.NotKnownAtAll_0;
				return bestiaryUICollectionInfo;
			}
			return result;
		}

		private bool TryFindingOneGoldCritterThatIsAlreadyUnlocked()
		{
			for (int i = 0; i < NPCID.Sets.GoldCrittersCollection.Count; i++)
			{
				int key = NPCID.Sets.GoldCrittersCollection[i];
				string persistentId = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[key];
				if (GetUnlockStateForCritter(persistentId) > BestiaryEntryUnlockState.NotKnownAtAll_0)
				{
					return true;
				}
			}
			return false;
		}

		private BestiaryEntryUnlockState GetUnlockStateForCritter(string persistentId)
		{
			if (!Main.BestiaryTracker.Sights.GetWasNearbyBefore(persistentId))
			{
				return BestiaryEntryUnlockState.NotKnownAtAll_0;
			}
			return BestiaryEntryUnlockState.CanShowDropsWithDropRates_4;
		}
	}
}
