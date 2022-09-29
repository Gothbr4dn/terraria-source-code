using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class MechBossSpawnersDropRule : IItemDropRule
	{
		public Conditions.MechanicalBossesDummyCondition dummyCondition = new Conditions.MechanicalBossesDummyCondition();

		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		public MechBossSpawnersDropRule()
		{
			ChainedRules = new List<IItemDropRuleChainAttempt>();
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			if (info.npc.value > 0f && Main.hardMode && (!NPC.downedMechBoss1 || !NPC.downedMechBoss2 || !NPC.downedMechBoss3))
			{
				return !info.IsInSimulation;
			}
			return false;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result;
			if (!NPC.downedMechBoss1 && info.player.RollLuck(2500) == 0)
			{
				CommonCode.DropItemFromNPC(info.npc, 556, 1);
				result = default(ItemDropAttemptResult);
				result.State = ItemDropAttemptResultState.Success;
				return result;
			}
			if (!NPC.downedMechBoss2 && info.player.RollLuck(2500) == 0)
			{
				CommonCode.DropItemFromNPC(info.npc, 544, 1);
				result = default(ItemDropAttemptResult);
				result.State = ItemDropAttemptResultState.Success;
				return result;
			}
			if (!NPC.downedMechBoss3 && info.player.RollLuck(2500) == 0)
			{
				CommonCode.DropItemFromNPC(info.npc, 557, 1);
				result = default(ItemDropAttemptResult);
				result.State = ItemDropAttemptResultState.Success;
				return result;
			}
			result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.FailedRandomRoll;
			return result;
		}

		public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			ratesInfo.AddCondition(dummyCondition);
			float num = 0.0004f;
			float dropRate = num * ratesInfo.parentDroprateChance;
			drops.Add(new DropRateInfo(556, 1, 1, dropRate, ratesInfo.conditions));
			drops.Add(new DropRateInfo(544, 1, 1, dropRate, ratesInfo.conditions));
			drops.Add(new DropRateInfo(557, 1, 1, dropRate, ratesInfo.conditions));
			Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
		}
	}
}
