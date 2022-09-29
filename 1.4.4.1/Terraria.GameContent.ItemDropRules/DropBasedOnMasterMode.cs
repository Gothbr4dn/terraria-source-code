using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class DropBasedOnMasterMode : IItemDropRule, INestedItemDropRule
	{
		public IItemDropRule ruleForDefault;

		public IItemDropRule ruleForMasterMode;

		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		public DropBasedOnMasterMode(IItemDropRule ruleForDefault, IItemDropRule ruleForMasterMode)
		{
			this.ruleForDefault = ruleForDefault;
			this.ruleForMasterMode = ruleForMasterMode;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsMasterMode)
			{
				return ruleForMasterMode.CanDrop(info);
			}
			return ruleForDefault.CanDrop(info);
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.DidNotRunCode;
			return result;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
		{
			if (info.IsMasterMode)
			{
				return resolveAction(ruleForMasterMode, info);
			}
			return resolveAction(ruleForDefault, info);
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			DropRateInfoChainFeed ratesInfo2 = ratesInfo.With(1f);
			ratesInfo2.AddCondition(new Conditions.IsMasterMode());
			ruleForMasterMode.ReportDroprates(drops, ratesInfo2);
			DropRateInfoChainFeed ratesInfo3 = ratesInfo.With(1f);
			ratesInfo3.AddCondition(new Conditions.NotMasterMode());
			ruleForDefault.ReportDroprates(drops, ratesInfo3);
			Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
		}
	}
}
