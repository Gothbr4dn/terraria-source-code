using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class SlimeBodyItemDropRule : IItemDropRule
	{
		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		public SlimeBodyItemDropRule()
		{
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.npc.type == 1 && info.npc.ai[1] > 0f)
			{
				return info.npc.ai[1] < 5453f;
			}
			return false;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			int itemId = (int)info.npc.ai[1];
			GetDropInfo(itemId, out var amountDroppedMinimum, out var amountDroppedMaximum);
			CommonCode.DropItemFromNPC(info.npc, itemId, info.rng.Next(amountDroppedMinimum, amountDroppedMaximum + 1));
			ItemDropAttemptResult result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.Success;
			return result;
		}

		public void GetDropInfo(int itemId, out int amountDroppedMinimum, out int amountDroppedMaximum)
		{
			amountDroppedMinimum = 1;
			amountDroppedMaximum = 1;
			switch (itemId)
			{
			case 8:
				amountDroppedMinimum = 5;
				amountDroppedMaximum = 10;
				break;
			case 166:
				amountDroppedMinimum = 2;
				amountDroppedMaximum = 6;
				break;
			case 965:
				amountDroppedMinimum = 20;
				amountDroppedMaximum = 45;
				break;
			case 11:
			case 12:
			case 13:
			case 14:
			case 699:
			case 700:
			case 701:
			case 702:
				amountDroppedMinimum = 3;
				amountDroppedMaximum = 13;
				break;
			case 71:
				amountDroppedMinimum = 50;
				amountDroppedMaximum = 99;
				break;
			case 72:
				amountDroppedMinimum = 20;
				amountDroppedMaximum = 99;
				break;
			case 73:
				amountDroppedMinimum = 1;
				amountDroppedMaximum = 2;
				break;
			case 4343:
			case 4344:
				amountDroppedMinimum = 2;
				amountDroppedMaximum = 5;
				break;
			}
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
		}
	}
}
