namespace Terraria.GameContent.Personalities
{
	public class UndergroundBiome : AShoppingBiome
	{
		public UndergroundBiome()
		{
			base.NameKey = "NormalUnderground";
		}

		public override bool IsInBiome(Player player)
		{
			return player.ShoppingZone_BelowSurface;
		}
	}
}
