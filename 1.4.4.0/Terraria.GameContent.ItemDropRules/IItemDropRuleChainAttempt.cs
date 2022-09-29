using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public interface IItemDropRuleChainAttempt
	{
		IItemDropRule RuleToChain { get; }

		bool CanChainIntoRule(ItemDropAttemptResult parentResult);

		void ReportDroprates(float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo);
	}
}
