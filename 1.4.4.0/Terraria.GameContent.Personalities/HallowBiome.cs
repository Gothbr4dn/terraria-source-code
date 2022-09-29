namespace Terraria.GameContent.Personalities
{
	public class HallowBiome : AShoppingBiome
	{
		public HallowBiome()
		{
			base.NameKey = "Hallow";
		}

		public override bool IsInBiome(Player player)
		{
			return player.ZoneHallow;
		}
	}
}
