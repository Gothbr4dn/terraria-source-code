using Terraria.IO;

namespace Terraria.WorldBuilding
{
	public static class Passes
	{
		public class Clear : GenPass
		{
			public Clear()
				: base("clear", 1.0)
			{
			}

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				for (int i = 0; i < GenBase._worldWidth; i++)
				{
					for (int j = 0; j < GenBase._worldHeight; j++)
					{
						if (GenBase._tiles[i, j] == null)
						{
							GenBase._tiles[i, j] = new Tile();
						}
						else
						{
							GenBase._tiles[i, j].ClearEverything();
						}
					}
				}
			}
		}

		public class ScatterCustom : GenPass
		{
			private CustomPerUnitAction _perUnit;

			private int _count;

			public ScatterCustom(string name, double loadWeight, int count, CustomPerUnitAction perUnit = null)
				: base(name, loadWeight)
			{
				_perUnit = perUnit;
				_count = count;
			}

			public void SetCustomAction(CustomPerUnitAction perUnit)
			{
				_perUnit = perUnit;
			}

			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				int num = _count;
				while (num > 0)
				{
					if (_perUnit(GenBase._random.Next(1, GenBase._worldWidth), GenBase._random.Next(1, GenBase._worldHeight)))
					{
						num--;
					}
				}
			}
		}
	}
}
