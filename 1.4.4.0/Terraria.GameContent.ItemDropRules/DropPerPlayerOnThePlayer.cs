namespace Terraria.GameContent.ItemDropRules
{
	public class DropPerPlayerOnThePlayer : CommonDrop
	{
		public IItemDropRuleCondition condition;

		public DropPerPlayerOnThePlayer(int itemId, int chanceDenominator, int amountDroppedMinimum, int amountDroppedMaximum, IItemDropRuleCondition optionalCondition)
			: base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum)
		{
			condition = optionalCondition;
		}

		public override bool CanDrop(DropAttemptInfo info)
		{
			if (condition != null)
			{
				return condition.CanDrop(info);
			}
			return true;
		}

		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			CommonCode.DropItemForEachInteractingPlayerOnThePlayer(info.npc, itemId, info.rng, chanceNumerator, chanceDenominator, info.rng.Next(amountDroppedMinimum, amountDroppedMaximum + 1));
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.Success;
			return result;
		}
	}
}
