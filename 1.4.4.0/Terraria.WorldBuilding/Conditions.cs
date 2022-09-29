namespace Terraria.WorldBuilding
{
	public static class Conditions
	{
		public class IsTile : GenCondition
		{
			private ushort[] _types;

			public IsTile(params ushort[] types)
			{
				_types = types;
			}

			protected override bool CheckValidity(int x, int y)
			{
				if (GenBase._tiles[x, y].active())
				{
					for (int i = 0; i < _types.Length; i++)
					{
						if (GenBase._tiles[x, y].type == _types[i])
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public class Continue : GenCondition
		{
			protected override bool CheckValidity(int x, int y)
			{
				return false;
			}
		}

		public class MysticSnake : GenCondition
		{
			protected override bool CheckValidity(int x, int y)
			{
				if (GenBase._tiles[x, y].active() && !Main.tileCut[GenBase._tiles[x, y].type])
				{
					return GenBase._tiles[x, y].type != 504;
				}
				return false;
			}
		}

		public class IsSolid : GenCondition
		{
			protected override bool CheckValidity(int x, int y)
			{
				if (!WorldGen.InWorld(x, y, 10))
				{
					return false;
				}
				if (GenBase._tiles[x, y].active())
				{
					return Main.tileSolid[GenBase._tiles[x, y].type];
				}
				return false;
			}
		}

		public class HasLava : GenCondition
		{
			protected override bool CheckValidity(int x, int y)
			{
				if (GenBase._tiles[x, y].liquid > 0)
				{
					return GenBase._tiles[x, y].liquidType() == 1;
				}
				return false;
			}
		}

		public class NotNull : GenCondition
		{
			protected override bool CheckValidity(int x, int y)
			{
				return GenBase._tiles[x, y] != null;
			}
		}
	}
}
