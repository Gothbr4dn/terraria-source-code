using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	public class ActionVines : GenAction
	{
		private int _minLength;

		private int _maxLength;

		private int _vineId;

		public ActionVines(int minLength = 6, int maxLength = 10, int vineId = 52)
		{
			_minLength = minLength;
			_maxLength = maxLength;
			_vineId = vineId;
		}

		public override bool Apply(Point origin, int x, int y, params object[] args)
		{
			int num = GenBase._random.Next(_minLength, _maxLength + 1);
			int i;
			for (i = 0; i < num && !GenBase._tiles[x, y + i].active(); i++)
			{
				GenBase._tiles[x, y + i].type = (ushort)_vineId;
				GenBase._tiles[x, y + i].active(active: true);
			}
			if (i > 0)
			{
				return UnitApply(origin, x, y, args);
			}
			return false;
		}
	}
}
