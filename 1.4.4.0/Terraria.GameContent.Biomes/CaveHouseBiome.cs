using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.GameContent.Biomes.CaveHouse;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	public class CaveHouseBiome : MicroBiome
	{
		private readonly HouseBuilderContext _builderContext = new HouseBuilderContext();

		[JsonProperty]
		public double IceChestChance { get; set; }

		[JsonProperty]
		public double JungleChestChance { get; set; }

		[JsonProperty]
		public double GoldChestChance { get; set; }

		[JsonProperty]
		public double GraniteChestChance { get; set; }

		[JsonProperty]
		public double MarbleChestChance { get; set; }

		[JsonProperty]
		public double MushroomChestChance { get; set; }

		[JsonProperty]
		public double DesertChestChance { get; set; }

		public override bool Place(Point origin, StructureMap structures)
		{
			if (!WorldGen.InWorld(origin.X, origin.Y, 10))
			{
				return false;
			}
			int num = 25;
			for (int i = origin.X - num; i <= origin.X + num; i++)
			{
				for (int j = origin.Y - num; j <= origin.Y + num; j++)
				{
					if (Main.tile[i, j].wire())
					{
						return false;
					}
					if (TileID.Sets.BasicChest[Main.tile[i, j].type])
					{
						return false;
					}
				}
			}
			HouseBuilder houseBuilder = HouseUtils.CreateBuilder(origin, structures);
			if (!houseBuilder.IsValid)
			{
				return false;
			}
			ApplyConfigurationToBuilder(houseBuilder);
			houseBuilder.Place(_builderContext, structures);
			return true;
		}

		private void ApplyConfigurationToBuilder(HouseBuilder builder)
		{
			switch (builder.Type)
			{
			case HouseType.Desert:
				builder.ChestChance = DesertChestChance;
				break;
			case HouseType.Granite:
				builder.ChestChance = GraniteChestChance;
				break;
			case HouseType.Ice:
				builder.ChestChance = IceChestChance;
				break;
			case HouseType.Jungle:
				builder.ChestChance = JungleChestChance;
				break;
			case HouseType.Marble:
				builder.ChestChance = MarbleChestChance;
				break;
			case HouseType.Mushroom:
				builder.ChestChance = MushroomChestChance;
				break;
			case HouseType.Wood:
				builder.ChestChance = GoldChestChance;
				break;
			}
		}
	}
}
