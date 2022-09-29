using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class DropBasedOnExpertMode : IItemDropRule, INestedItemDropRule
	{
		public IItemDropRule ruleForNormalMode;

		public IItemDropRule ruleForExpertMode;

		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		public DropBasedOnExpertMode(IItemDropRule ruleForNormalMode, IItemDropRule ruleForExpertMode)
		{
			this.ruleForNormalMode = ruleForNormalMode;
			this.ruleForExpertMode = ruleForExpertMode;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsExpertMode)
			{
				return ruleForExpertMode.CanDrop(info);
			}
			return ruleForNormalMode.CanDrop(info);
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.DidNotRunCode;
			return result;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
		{
			if (info.IsExpertMode)
			{
				return resolveAction(ruleForExpertMode, info);
			}
			return resolveAction(ruleForNormalMode, info);
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
			ratesInfo2.AddCondition(new Conditions.IsExpert());
			ruleForExpertMode.ReportDroprates(drops, ratesInfo2);
			DropRateInfoChainFeed ratesInfo3 = ratesInfo.With(1f);
			ratesInfo3.AddCondition(new Conditions.NotExpert());
			ruleForNormalMode.ReportDroprates(drops, ratesInfo3);
			Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
		}
	}
}
