namespace Terraria.GameContent.Personalities
{
	public class CorruptionBiome : AShoppingBiome
	{
		public CorruptionBiome()
		{
			base.NameKey = "Corruption";
		}

		public override bool IsInBiome(Player player)
		{
			return player.ZoneCorrupt;
		}
	}
}
