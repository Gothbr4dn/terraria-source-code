namespace Terraria.GameContent.Personalities
{
	public abstract class AShoppingBiome
	{
		public string NameKey { get; protected set; }

		public abstract bool IsInBiome(Player player);
	}
}
