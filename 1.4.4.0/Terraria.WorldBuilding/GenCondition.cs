namespace Terraria.WorldBuilding
{
	public abstract class GenCondition : GenBase
	{
		private enum AreaType
		{
			And,
			Or,
			None
		}

		private bool InvertResults;

		private int _width;

		private int _height;

		private AreaType _areaType = AreaType.None;

		public bool IsValid(int x, int y)
		{
			switch (_areaType)
			{
			case AreaType.None:
				return CheckValidity(x, y) ^ InvertResults;
			case AreaType.And:
			{
				for (int k = x; k < x + _width; k++)
				{
					for (int l = y; l < y + _height; l++)
					{
						if (!CheckValidity(k, l))
						{
							return InvertResults;
						}
					}
				}
				return !InvertResults;
			}
			case AreaType.Or:
			{
				for (int i = x; i < x + _width; i++)
				{
					for (int j = y; j < y + _height; j++)
					{
						if (CheckValidity(i, j))
						{
							return !InvertResults;
						}
					}
				}
				return InvertResults;
			}
			default:
				return true;
			}
		}

		public GenCondition Not()
		{
			InvertResults = !InvertResults;
			return this;
		}

		public GenCondition AreaOr(int width, int height)
		{
			_areaType = AreaType.Or;
			_width = width;
			_height = height;
			return this;
		}

		public GenCondition AreaAnd(int width, int height)
		{
			_areaType = AreaType.And;
			_width = width;
			_height = height;
			return this;
		}

		protected abstract bool CheckValidity(int x, int y);
	}
}
