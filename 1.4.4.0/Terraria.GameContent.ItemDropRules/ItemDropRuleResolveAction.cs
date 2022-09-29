namespace Terraria.GameContent.ItemDropRules
{
	public delegate ItemDropAttemptResult ItemDropRuleResolveAction(IItemDropRule rule, DropAttemptInfo info);
}
