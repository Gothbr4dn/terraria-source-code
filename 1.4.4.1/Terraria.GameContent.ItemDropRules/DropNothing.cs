using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class DropNothing : IItemDropRule
	{
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		public DropNothing()
		{
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			return false;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.DoesntFillConditions;
			return result;
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
		}
	}
}
