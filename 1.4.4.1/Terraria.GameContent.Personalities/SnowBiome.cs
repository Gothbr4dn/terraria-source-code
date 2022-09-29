namespace Terraria.GameContent.Personalities
{
	public class SnowBiome : AShoppingBiome
	{
		public SnowBiome()
		{
			base.NameKey = "Snow";
		}

		public override bool IsInBiome(Player player)
		{
			return player.ZoneSnow;
		}
	}
}
