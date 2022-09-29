using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class FromOptionsWithoutRepeatsDropRule : IItemDropRule
	{
		public int[] dropIds;

		public int dropCount;

		private List<int> _temporaryAvailableItems = new List<int>();

		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		public FromOptionsWithoutRepeatsDropRule(int dropCount, params int[] options)
		{
			this.dropCount = dropCount;
			dropIds = options;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			_temporaryAvailableItems.Clear();
			_temporaryAvailableItems.AddRange(dropIds);
			for (int i = 0; i < dropCount; i++)
			{
				if (_temporaryAvailableItems.Count <= 0)
				{
					break;
				}
				int index = info.rng.Next(_temporaryAvailableItems.Count);
				CommonCode.DropItemFromNPC(info.npc, _temporaryAvailableItems[index], 1);
				_temporaryAvailableItems.RemoveAt(index);
			}
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.Success;
			return result;
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float parentDroprateChance = ratesInfo.parentDroprateChance;
			int num = dropIds.Length;
			float num2 = 1f;
			int num3 = 0;
			while (num3 < dropCount && num > 0)
			{
				num2 *= (float)(num - 1) / (float)num;
				num3++;
				num--;
			}
			float dropRate = (1f - num2) * parentDroprateChance;
			for (int i = 0; i < dropIds.Length; i++)
			{
				drops.Add(new DropRateInfo(dropIds[i], 1, 1, dropRate, ratesInfo.conditions));
			}
			Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
		}
	}
}
