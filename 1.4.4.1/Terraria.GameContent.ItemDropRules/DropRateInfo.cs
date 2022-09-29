using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public struct DropRateInfo
	{
		public int itemId;

		public int stackMin;

		public int stackMax;

		public float dropRate;

		public List<IItemDropRuleCondition> conditions;

		public DropRateInfo(int itemId, int stackMin, int stackMax, float dropRate, List<IItemDropRuleCondition> conditions = null)
		{
			this.itemId = itemId;
			this.stackMin = stackMin;
			this.stackMax = stackMax;
			this.dropRate = dropRate;
			this.conditions = null;
			if (conditions != null && conditions.Count > 0)
			{
				this.conditions = new List<IItemDropRuleCondition>(conditions);
			}
		}

		public void AddCondition(IItemDropRuleCondition condition)
		{
			if (conditions == null)
			{
				conditions = new List<IItemDropRuleCondition>();
			}
			conditions.Add(condition);
		}
	}
}
