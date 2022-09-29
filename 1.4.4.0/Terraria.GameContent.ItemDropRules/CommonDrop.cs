using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class CommonDrop : IItemDropRule
	{
		public int itemId;

		public int chanceDenominator;

		public int amountDroppedMinimum;

		public int amountDroppedMaximum;

		public int chanceNumerator;

		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		public CommonDrop(int itemId, int chanceDenominator, int amountDroppedMinimum = 1, int amountDroppedMaximum = 1, int chanceNumerator = 1)
		{
			this.itemId = itemId;
			this.chanceDenominator = chanceDenominator;
			this.amountDroppedMinimum = amountDroppedMinimum;
			this.amountDroppedMaximum = amountDroppedMaximum;
			this.chanceNumerator = chanceNumerator;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public virtual bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}

		public virtual ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result;
			if (info.player.RollLuck(chanceDenominator) < chanceNumerator)
			{
				CommonCode.DropItemFromNPC(info.npc, itemId, info.rng.Next(amountDroppedMinimum, amountDroppedMaximum + 1));
				result = default(ItemDropAttemptResult);
				result.State = ItemDropAttemptResultState.Success;
				return result;
			}
			result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.FailedRandomRoll;
			return result;
		}

		public virtual void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = (float)chanceNumerator / (float)chanceDenominator;
			float dropRate = num * ratesInfo.parentDroprateChance;
			drops.Add(new DropRateInfo(itemId, amountDroppedMinimum, amountDroppedMaximum, dropRate, ratesInfo.conditions));
			Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
		}
	}
}
