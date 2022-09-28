namespace Terraria.GameContent.Personalities
{
	public class DesertBiome : AShoppingBiome
	{
		public DesertBiome()
		{
			base.NameKey = "Desert";
		}

		public override bool IsInBiome(Player player)
		{
			return player.ZoneDesert;
		}
	}
}
