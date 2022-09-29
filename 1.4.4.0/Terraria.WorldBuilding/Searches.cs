using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	public static class Searches
	{
		public class Left : GenSearch
		{
			private int _maxDistance;

			public Left(int maxDistance)
			{
				_maxDistance = maxDistance;
			}

			public override Point Find(Point origin)
			{
				for (int i = 0; i < _maxDistance; i++)
				{
					if (Check(origin.X - i, origin.Y))
					{
						return new Point(origin.X - i, origin.Y);
					}
				}
				return GenSearch.NOT_FOUND;
			}
		}

		public class Right : GenSearch
		{
			private int _maxDistance;

			public Right(int maxDistance)
			{
				_maxDistance = maxDistance;
			}

			public override Point Find(Point origin)
			{
				for (int i = 0; i < _maxDistance; i++)
				{
					if (Check(origin.X + i, origin.Y))
					{
						return new Point(origin.X + i, origin.Y);
					}
				}
				return GenSearch.NOT_FOUND;
			}
		}

		public class Down : GenSearch
		{
			private int _maxDistance;

			public Down(int maxDistance)
			{
				_maxDistance = maxDistance;
			}

			public override Point Find(Point origin)
			{
				for (int i = 0; i < _maxDistance && origin.Y + i < Main.maxTilesY; i++)
				{
					if (Check(origin.X, origin.Y + i))
					{
						return new Point(origin.X, origin.Y + i);
					}
				}
				return GenSearch.NOT_FOUND;
			}
		}

		public class Up : GenSearch
		{
			private int _maxDistance;

			public Up(int maxDistance)
			{
				_maxDistance = maxDistance;
			}

			public override Point Find(Point origin)
			{
				for (int i = 0; i < _maxDistance; i++)
				{
					if (Check(origin.X, origin.Y - i))
					{
						return new Point(origin.X, origin.Y - i);
					}
				}
				return GenSearch.NOT_FOUND;
			}
		}

		public class Rectangle : GenSearch
		{
			private int _width;

			private int _height;

			public Rectangle(int width, int height)
			{
				_width = width;
				_height = height;
			}

			public override Point Find(Point origin)
			{
				for (int i = 0; i < _width; i++)
				{
					for (int j = 0; j < _height; j++)
					{
						if (Check(origin.X + i, origin.Y + j))
						{
							return new Point(origin.X + i, origin.Y + j);
						}
					}
				}
				return GenSearch.NOT_FOUND;
			}
		}

		public static GenSearch Chain(GenSearch search, params GenCondition[] conditions)
		{
			return search.Conditions(conditions);
		}
	}
}
