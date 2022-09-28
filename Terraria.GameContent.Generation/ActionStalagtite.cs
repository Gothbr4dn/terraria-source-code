using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	public class ActionStalagtite : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			WorldGen.PlaceTight(x, y);
			return UnitApply(origin, x, y, args);
		}
	}
}
