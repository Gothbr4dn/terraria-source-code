namespace Terraria.GameContent.Personalities
{
	public class CrimsonBiome : AShoppingBiome
	{
		public CrimsonBiome()
		{
			base.NameKey = "Crimson";
		}

		public override bool IsInBiome(Player player)
		{
			return player.ZoneCrimson;
		}
	}
}
