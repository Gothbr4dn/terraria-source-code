namespace Terraria.GameContent.ItemDropRules
{
	public class ItemDropRule
	{
		public static IItemDropRule Common(int itemId, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new CommonDrop(itemId, chanceDenominator, minimumDropped, maximumDropped);
		}

		public static IItemDropRule BossBag(int itemId)
		{
			return new DropBasedOnExpertMode(DropNothing(), new DropLocalPerClientAndResetsNPCMoneyTo0(itemId, 1, 1, 1, null));
		}

		public static IItemDropRule BossBagByCondition(IItemDropRuleCondition condition, int itemId)
		{
			return new DropBasedOnExpertMode(DropNothing(), new DropLocalPerClientAndResetsNPCMoneyTo0(itemId, 1, 1, 1, condition));
		}

		public static IItemDropRule ExpertGetsRerolls(int itemId, int chanceDenominator, int expertRerolls)
		{
			return new DropBasedOnExpertMode(WithRerolls(itemId, 0, chanceDenominator), WithRerolls(itemId, expertRerolls, chanceDenominator));
		}

		public static IItemDropRule MasterModeCommonDrop(int itemId)
		{
			return ByCondition(new Conditions.IsMasterMode(), itemId);
		}

		public static IItemDropRule MasterModeDropOnAllPlayers(int itemId, int chanceDenominator = 1)
		{
			return new DropBasedOnMasterMode(DropNothing(), new DropPerPlayerOnThePlayer(itemId, chanceDenominator, 1, 1, new Conditions.IsMasterMode()));
		}

		public static IItemDropRule WithRerolls(int itemId, int rerolls, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new CommonDropWithRerolls(itemId, chanceDenominator, minimumDropped, maximumDropped, rerolls);
		}

		public static IItemDropRule ByCondition(IItemDropRuleCondition condition, int itemId, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1, int chanceNumerator = 1)
		{
			return new ItemDropWithConditionRule(itemId, chanceDenominator, minimumDropped, maximumDropped, condition, chanceNumerator);
		}

		public static IItemDropRule NotScalingWithLuck(int itemId, int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new CommonDropNotScalingWithLuck(itemId, chanceDenominator, minimumDropped, maximumDropped);
		}

		public static IItemDropRule OneFromOptionsNotScalingWithLuck(int chanceDenominator, params int[] options)
		{
			return new OneFromOptionsNotScaledWithLuckDropRule(chanceDenominator, 1, options);
		}

		public static IItemDropRule OneFromOptionsNotScalingWithLuckWithX(int chanceDenominator, int chanceNumerator, params int[] options)
		{
			return new OneFromOptionsNotScaledWithLuckDropRule(chanceDenominator, chanceNumerator, options);
		}

		public static IItemDropRule OneFromOptions(int chanceDenominator, params int[] options)
		{
			return new OneFromOptionsDropRule(chanceDenominator, 1, options);
		}

		public static IItemDropRule OneFromOptionsWithNumerator(int chanceDenominator, int chanceNumerator, params int[] options)
		{
			return new OneFromOptionsDropRule(chanceDenominator, chanceNumerator, options);
		}

		public static IItemDropRule DropNothing()
		{
			return new DropNothing();
		}

		public static IItemDropRule NormalvsExpert(int itemId, int chanceDenominatorInNormal, int chanceDenominatorInExpert)
		{
			return new DropBasedOnExpertMode(Common(itemId, chanceDenominatorInNormal), Common(itemId, chanceDenominatorInExpert));
		}

		public static IItemDropRule NormalvsExpertNotScalingWithLuck(int itemId, int chanceDenominatorInNormal, int chanceDenominatorInExpert)
		{
			return new DropBasedOnExpertMode(NotScalingWithLuck(itemId, chanceDenominatorInNormal), NotScalingWithLuck(itemId, chanceDenominatorInExpert));
		}

		public static IItemDropRule NormalvsExpertOneFromOptionsNotScalingWithLuck(int chanceDenominatorInNormal, int chanceDenominatorInExpert, params int[] options)
		{
			return new DropBasedOnExpertMode(OneFromOptionsNotScalingWithLuck(chanceDenominatorInNormal, options), OneFromOptionsNotScalingWithLuck(chanceDenominatorInExpert, options));
		}

		public static IItemDropRule NormalvsExpertOneFromOptions(int chanceDenominatorInNormal, int chanceDenominatorInExpert, params int[] options)
		{
			return new DropBasedOnExpertMode(OneFromOptions(chanceDenominatorInNormal, options), OneFromOptions(chanceDenominatorInExpert, options));
		}

		public static IItemDropRule Food(int itemId, int chanceDenominator, int minimumDropped = 1, int maximumDropped = 1)
		{
			return new ItemDropWithConditionRule(itemId, chanceDenominator, minimumDropped, maximumDropped, new Conditions.NotFromStatue());
		}

		public static IItemDropRule StatusImmunityItem(int itemId, int dropsOutOfX)
		{
			return ExpertGetsRerolls(itemId, dropsOutOfX, 1);
		}
	}
}
