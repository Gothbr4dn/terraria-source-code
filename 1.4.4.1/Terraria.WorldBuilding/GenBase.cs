using Terraria.Utilities;

namespace Terraria.WorldBuilding
{
	public class GenBase
	{
		public delegate bool CustomPerUnitAction(int x, int y, params object[] args);

		protected static UnifiedRandom _random => WorldGen.genRand;

		protected static Tile[,] _tiles => Main.tile;

		protected static int _worldWidth => Main.maxTilesX;

		protected static int _worldHeight => Main.maxTilesY;
	}
}
