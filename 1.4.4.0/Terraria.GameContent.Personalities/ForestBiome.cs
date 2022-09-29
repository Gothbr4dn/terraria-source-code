namespace Terraria.GameContent.Personalities
{
	public class ForestBiome : AShoppingBiome
	{
		public ForestBiome()
		{
			base.NameKey = "Forest";
		}

		public override bool IsInBiome(Player player)
		{
			return player.ShoppingZone_Forest;
		}
	}
}
