using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class CommonDropWithRerolls : CommonDrop
	{
		public int timesToRoll;

		public CommonDropWithRerolls(int itemId, int chanceDenominator, int amountDroppedMinimum, int amountDroppedMaximum, int rerolls)
			: base(itemId, chanceDenominator, amountDroppedMinimum, amountDroppedMaximum)
		{
			timesToRoll = rerolls + 1;
		}

		public override ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			bool flag = false;
			for (int i = 0; i < timesToRoll; i++)
			{
				flag = flag || info.player.RollLuck(chanceDenominator) < chanceNumerator;
			}
			ItemDropAttemptResult result;
			if (flag)
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

		public override void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			float num = (float)chanceNumerator / (float)chanceDenominator;
			float num2 = 1f - num;
			float num3 = 1f;
			for (int i = 0; i < timesToRoll; i++)
			{
				num3 *= num2;
			}
			float num4 = 1f - num3;
			float dropRate = num4 * ratesInfo.parentDroprateChance;
			drops.Add(new DropRateInfo(itemId, amountDroppedMinimum, amountDroppedMaximum, dropRate, ratesInfo.conditions));
			Chains.ReportDroprates(base.ChainedRules, num4, drops, ratesInfo);
		}
	}
}
