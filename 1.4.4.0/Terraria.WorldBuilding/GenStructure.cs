using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	public abstract class GenStructure : GenBase
	{
		public abstract bool Place(Point origin, StructureMap structures);
	}
}
