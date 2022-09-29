namespace Terraria.GameContent.ItemDropRules
{
	public class CommonDropNotScalingWithLuck : CommonDrop
	{
		public CommonDropNotScalingWithLuck(int itemId, int chanceDenominator, int amountDroppedMinimum, int amountDroppedMaximum)
			: base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum)
		{
		}

		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result;
			if (info.rng.Next(chanceDenominator) < chanceNumerator)
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
	}
}
