using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	public class ActionGrass : GenAction
	{
		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			if (GenBase._tiles[x, y].active() || GenBase._tiles[x, y - 1].active())
			{
				return false;
			}
			WorldGen.PlaceTile(x, y, Utils.SelectRandom(GenBase._random, new ushort[2] { 3, 73 }), mute: true);
			return UnitApply(origin, x, y, args);
		}
	}
}
