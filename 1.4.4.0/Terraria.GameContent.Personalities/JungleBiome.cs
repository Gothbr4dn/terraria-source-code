namespace Terraria.GameContent.Personalities
{
	public class JungleBiome : AShoppingBiome
	{
		public JungleBiome()
		{
			base.NameKey = "Jungle";
		}

		public override bool IsInBiome(Player player)
		{
			return player.ZoneJungle;
		}
	}
}
