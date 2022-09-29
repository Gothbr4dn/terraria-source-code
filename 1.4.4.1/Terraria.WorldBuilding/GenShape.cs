using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	public abstract class GenShape : GenBase
	{
		private ShapeData _outputData;

		protected bool _quitOnFail;

		public abstract bool Perform(Point origin, GenAction action);

		protected bool UnitApply(GenAction action, Point origin, int x, int y, params object[] args)
		{
			if (_outputData != null)
			{
				_outputData.Add(x - origin.X, y - origin.Y);
			}
			return action.Apply(origin, x, y, args);
		}

		public GenShape Output(ShapeData outputData)
		{
			_outputData = outputData;
			return this;
		}

		public GenShape QuitOnFail(bool value = true)
		{
			_quitOnFail = value;
			return this;
		}
	}
}
