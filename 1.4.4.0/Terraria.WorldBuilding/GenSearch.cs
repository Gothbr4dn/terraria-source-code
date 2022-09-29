using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	public abstract class GenSearch : GenBase
	{
		public static Point NOT_FOUND = new Point(int.MaxValue, int.MaxValue);

		private bool _requireAll = true;

		private GenCondition[] _conditions;

		public GenSearch Conditions(params GenCondition[] conditions)
		{
			_conditions = conditions;
			return this;
		}

		public abstract Point Find(Point origin);

		protected bool Check(int x, int y)
		{
			for (int i = 0; i < _conditions.Length; i++)
			{
				if (_requireAll ^ _conditions[i].IsValid(x, y))
				{
					return !_requireAll;
				}
			}
			return _requireAll;
		}

		public GenSearch RequireAll(bool mode)
		{
			_requireAll = mode;
			return this;
		}
	}
}
