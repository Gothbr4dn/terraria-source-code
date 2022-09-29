using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules
{
	public class DropOneByOne : IItemDropRule
	{
		public struct Parameters
		{
			public int ChanceNumerator;

			public int ChanceDenominator;

			public int MinimumItemDropsCount;

			public int MaximumItemDropsCount;

			public int MinimumStackPerChunkBase;

			public int MaximumStackPerChunkBase;

			public int BonusMinDropsPerChunkPerPlayer;

			public int BonusMaxDropsPerChunkPerPlayer;

			public float GetPersonalDropRate()
			{
				return (float)ChanceNumerator / (float)ChanceDenominator;
			}
		}

		public int itemId;

		public Parameters parameters;

		public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

		public DropOneByOne(int itemId, Parameters parameters)
		{
			ChainedRules = new List<IItemDropRuleChainAttempt>();
			this.parameters = parameters;
			this.itemId = itemId;
		}

		public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
		{
			ItemDropAttemptResult result;
			if (info.player.RollLuck(parameters.ChanceDenominator) < parameters.ChanceNumerator)
			{
				int num = info.rng.Next(parameters.MinimumItemDropsCount, parameters.MaximumItemDropsCount + 1);
				int activePlayersCount = Main.CurrentFrameFlags.ActivePlayersCount;
				int minValue = parameters.MinimumStackPerChunkBase + activePlayersCount * parameters.BonusMinDropsPerChunkPerPlayer;
				int num2 = parameters.MaximumStackPerChunkBase + activePlayersCount * parameters.BonusMaxDropsPerChunkPerPlayer;
				for (int i = 0; i < num; i++)
				{
					CommonCode.DropItemFromNPC(info.npc, itemId, info.rng.Next(minValue, num2 + 1), scattered: true);
				}
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
			float personalDropRate = parameters.GetPersonalDropRate();
			float dropRate = personalDropRate * ratesInfo.parentDroprateChance;
			drops.Add(new DropRateInfo(itemId, parameters.MinimumItemDropsCount * (parameters.MinimumStackPerChunkBase + parameters.BonusMinDropsPerChunkPerPlayer), parameters.MaximumItemDropsCount * (parameters.MaximumStackPerChunkBase + parameters.BonusMaxDropsPerChunkPerPlayer), dropRate, ratesInfo.conditions));
			Chains.ReportDroprates(ChainedRules, personalDropRate, drops, ratesInfo);
		}

		public bool CanDrop(DropAttemptInfo info)
		{
			return true;
		}
	}
}
