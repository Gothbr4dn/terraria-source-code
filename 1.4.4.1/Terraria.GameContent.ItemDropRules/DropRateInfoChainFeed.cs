using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public struct DropRateInfoChainFeed
	{
		public float parentDroprateChance;

		public List<IItemDropRuleCondition> conditions;

		public void AddCondition(IItemDropRuleCondition condition)
		{
			if (conditions == null)
			{
				conditions = new List<IItemDropRuleCondition>();
			}
			conditions.Add(condition);
		}

		public DropRateInfoChainFeed(float droprate)
		{
			parentDroprateChance = droprate;
			conditions = null;
		}

		public DropRateInfoChainFeed With(float multiplier)
		{
			DropRateInfoChainFeed result = new DropRateInfoChainFeed(parentDroprateChance * multiplier);
			if (conditions != null)
			{
				result.conditions = new List<IItemDropRuleCondition>(conditions);
			}
			return result;
		}
	}
}
