namespace Terraria.GameContent.Personalities
{
	public class OceanBiome : AShoppingBiome
	{
		public OceanBiome()
		{
			base.NameKey = "Ocean";
		}

		public override bool IsInBiome(Player player)
		{
			return player.ZoneBeach;
		}
	}
}
