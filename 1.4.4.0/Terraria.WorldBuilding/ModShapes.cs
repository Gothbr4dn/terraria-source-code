using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.WorldBuilding
{
	public static class ModShapes
	{
		public class All : GenModShape
		{
			public All(ShapeData data)
				: base(data)
			{
			}

			public override bool Perform(Point origin, GenAction action)
			{
				foreach (Point16 datum in _data.GetData())
				{
					if (!UnitApply(action, origin, datum.X + origin.X, datum.Y + origin.Y) && _quitOnFail)
					{
						return false;
					}
				}
				return true;
			}
		}

		public class OuterOutline : GenModShape
		{
			private static readonly int[] POINT_OFFSETS = new int[16]
			{
				1, 0, -1, 0, 0, 1, 0, -1, 1, 1,
				1, -1, -1, 1, -1, -1
			};

			private bool _useDiagonals;

			private bool _useInterior;

			public OuterOutline(ShapeData data, bool useDiagonals = true, bool useInterior = false)
				: base(data)
			{
				_useDiagonals = useDiagonals;
				_useInterior = useInterior;
			}

			public override bool Perform(Point origin, GenAction action)
			{
				int num = (_useDiagonals ? 16 : 8);
				foreach (Point16 datum in _data.GetData())
				{
					if (_useInterior && !UnitApply(action, origin, datum.X + origin.X, datum.Y + origin.Y) && _quitOnFail)
					{
						return false;
					}
					for (int i = 0; i < num; i += 2)
					{
						if (!_data.Contains(datum.X + POINT_OFFSETS[i], datum.Y + POINT_OFFSETS[i + 1]) && !UnitApply(action, origin, origin.X + datum.X + POINT_OFFSETS[i], origin.Y + datum.Y + POINT_OFFSETS[i + 1]) && _quitOnFail)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		public class InnerOutline : GenModShape
		{
			private static readonly int[] POINT_OFFSETS = new int[16]
			{
				1, 0, -1, 0, 0, 1, 0, -1, 1, 1,
				1, -1, -1, 1, -1, -1
			};

			private bool _useDiagonals;

			public InnerOutline(ShapeData data, bool useDiagonals = true)
				: base(data)
			{
				_useDiagonals = useDiagonals;
			}

			public override bool Perform(Point origin, GenAction action)
			{
				int num = (_useDiagonals ? 16 : 8);
				foreach (Point16 datum in _data.GetData())
				{
					bool flag = false;
					for (int i = 0; i < num; i += 2)
					{
						if (!_data.Contains(datum.X + POINT_OFFSETS[i], datum.Y + POINT_OFFSETS[i + 1]))
						{
							flag = true;
							break;
						}
					}
					if (flag && !UnitApply(action, origin, datum.X + origin.X, datum.Y + origin.Y) && _quitOnFail)
					{
						return false;
					}
				}
				return true;
			}
		}
	}
}
