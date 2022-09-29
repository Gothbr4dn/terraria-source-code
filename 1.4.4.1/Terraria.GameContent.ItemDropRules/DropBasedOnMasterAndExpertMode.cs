using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class DropBasedOnMasterAndExpertMode : IItemDropRule, INestedItemDropRule
	{
		public IItemDropRule ruleForDefault;

		public IItemDropRule ruleForExpertmode;

		public IItemDropRule ruleForMasterMode;

		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		public DropBasedOnMasterAndExpertMode(IItemDropRule ruleForDefault, IItemDropRule ruleForExpertMode, IItemDropRule ruleForMasterMode)
		{
			this.ruleForDefault = ruleForDefault;
			ruleForExpertmode = ruleForExpertMode;
			this.ruleForMasterMode = ruleForMasterMode;
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.IsMasterMode)
			{
				return ruleForMasterMode.CanDrop(info);
			}
			if (info.IsExpertMode)
			{
				return ruleForExpertmode.CanDrop(info);
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
			if (info.IsExpertMode)
			{
				return resolveAction(ruleForExpertmode, info);
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
			ratesInfo3.AddCondition(new Conditions.IsExpert());
			ruleForExpertmode.ReportDroprates(drops, ratesInfo3);
			DropRateInfoChainFeed ratesInfo4 = ratesInfo.With(1f);
			ratesInfo4.AddCondition(new Conditions.NotMasterMode());
			ratesInfo4.AddCondition(new Conditions.NotExpert());
			ruleForDefault.ReportDroprates(drops, ratesInfo4);
			Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
		}
	}
}
