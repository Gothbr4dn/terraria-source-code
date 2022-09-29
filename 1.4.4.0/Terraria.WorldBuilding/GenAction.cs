using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	public abstract class GenAction : GenBase
	{
		public GenAction NextAction;

		public ShapeData OutputData;

		private bool _returnFalseOnFailure = true;

		public abstract bool Apply(Point origin, int x, int y, params object[] args);

		protected bool UnitApply(Point origin, int x, int y, params object[] args)
		{
			if (OutputData != null)
			{
				OutputData.Add(x - origin.X, y - origin.Y);
			}
			if (NextAction != null)
			{
				return NextAction.Apply(origin, x, y, args);
			}
			return true;
		}

		public GenAction IgnoreFailures()
		{
			_returnFalseOnFailure = false;
			return this;
		}

		protected bool Fail()
		{
			return !_returnFalseOnFailure;
		}

		public GenAction Output(ShapeData data)
		{
			OutputData = data;
			return this;
		}
	}
}
