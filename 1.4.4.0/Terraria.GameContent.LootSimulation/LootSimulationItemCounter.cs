using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace Terraria.GameContent.LootSimulation
{
	public class LootSimulationItemCounter
	{
		private long[] _itemCountsObtained = new long[5453];

		private long[] _itemCountsObtainedExpert = new long[5453];

		private long _totalTimesAttempted;

		private long _totalTimesAttemptedExpert;

		public void AddItem(int itemId, int amount, bool expert)
		{
			if (expert)
			{
				_itemCountsObtainedExpert[itemId] += amount;
			}
			else
			{
				_itemCountsObtained[itemId] += amount;
			}
		}

		public void Exclude(params int[] itemIds)
		{
			foreach (int num in itemIds)
			{
				_itemCountsObtained[num] = 0L;
				_itemCountsObtainedExpert[num] = 0L;
			}
		}

		public void IncreaseTimesAttempted(int amount, bool expert)
		{
			if (expert)
			{
				_totalTimesAttemptedExpert += amount;
			}
			else
			{
				_totalTimesAttempted += amount;
			}
		}

		public string PrintCollectedItems(bool expert)
		{
			long[] collectionToUse = _itemCountsObtained;
			long totalDropsAttempted = _totalTimesAttempted;
			if (expert)
			{
				collectionToUse = _itemCountsObtainedExpert;
				_totalTimesAttempted = _totalTimesAttemptedExpert;
			}
			IEnumerable<string> values = from entry in collectionToUse.Select((long count, int itemId) => new { itemId, count })
				where entry.count > 0
				select entry.itemId into itemId
				select $"new ItemDropInfo(ItemID.{ItemID.Search.GetName(itemId)}, {collectionToUse[itemId]}, {totalDropsAttempted})";
			return string.Join(",\n", values);
		}
	}
}
