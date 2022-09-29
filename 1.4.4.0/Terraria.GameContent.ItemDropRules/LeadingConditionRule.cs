using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class LeadingConditionRule : IItemDropRule
	{
		public IItemDropRuleCondition condition;

		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		public LeadingConditionRule(IItemDropRuleCondition condition)
		{
			this.condition = condition;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			return condition.CanDrop(info);
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			ratesInfo.AddCondition(condition);
			Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.Success;
			return result;
		}
	}
}
