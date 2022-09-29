namespace Terraria.GameContent.ItemDropRules
{
	public interface INestedItemDropRule
	{
		ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction);
	}
}
