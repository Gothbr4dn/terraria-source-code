using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.GameContent.Biomes.Desert;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	public class DesertBiome : MicroBiome
	{
		[JsonProperty("ChanceOfEntrance")]
		public double ChanceOfEntrance = 0.3333;

		public override bool Place(Point origin, StructureMap structures)
		{
			DesertDescription desertDescription = DesertDescription.CreateFromPlacement(origin);
			if (!desertDescription.IsValid)
			{
				return false;
			}
			ExportDescriptionToEngine(desertDescription);
			SandMound.Place(desertDescription);
			desertDescription.UpdateSurfaceMap();
			if (!Main.tenthAnniversaryWorld && GenBase._random.NextDouble() <= ChanceOfEntrance)
			{
				switch (GenBase._random.Next(4))
				{
				case 0:
					ChambersEntrance.Place(desertDescription);
					break;
				case 1:
					AnthillEntrance.Place(desertDescription);
					break;
				case 2:
					LarvaHoleEntrance.Place(desertDescription);
					break;
				case 3:
					PitEntrance.Place(desertDescription);
					break;
				}
			}
			DesertHive.Place(desertDescription);
			CleanupArea(desertDescription.Hive);
			Rectangle area = new Rectangle(desertDescription.CombinedArea.X, 50, desertDescription.CombinedArea.Width, desertDescription.CombinedArea.Bottom - 20);
			structures.AddStructure(area, 10);
			return true;
		}

		private static void ExportDescriptionToEngine(DesertDescription description)
		{
			GenVars.UndergroundDesertLocation = description.CombinedArea;
			GenVars.UndergroundDesertLocation.Inflate(10, 10);
			GenVars.UndergroundDesertHiveLocation = description.Hive;
		}

		private static void CleanupArea(Rectangle area)
		{
			for (int i = -20 + area.Left; i < area.Right + 20; i++)
			{
				for (int j = -20 + area.Top; j < area.Bottom + 20; j++)
				{
					if (i > 0 && i < Main.maxTilesX - 1 && j > 0 && j < Main.maxTilesY - 1)
					{
						WorldGen.SquareWallFrame(i, j);
						WorldUtils.TileFrame(i, j, frameNeighbors: true);
					}
				}
			}
		}
	}
}
