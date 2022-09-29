namespace Terraria.DataStructures
{
	public class RecipeItemCreationContext : ItemCreationContext
	{
		public readonly Recipe Recipe;

		public RecipeItemCreationContext(Recipe recipe)
		{
			Recipe = recipe;
		}
	}
}
