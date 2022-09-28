namespace Terraria.GameContent.ItemDropRules
{
	public interface IItemDropRuleCondition : IProvideItemConditionDescription
	{
		bool CanDrop(DropAttemptInfo info);

		bool CanShowItemDropInUI();
	}
}
