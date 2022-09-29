namespace Terraria.Graphics.Capture
{
	public class CaptureBiome
	{
		public enum TileColorStyle
		{
			Normal,
			Jungle,
			Crimson,
			Corrupt,
			Mushroom
		}

		public class Sets
		{
			public class WaterStyles
			{
				public const int BloodMoon = 9;
			}
		}

		public class Styles
		{
			public static CaptureBiome Purity = new CaptureBiome(0, 0);

			public static CaptureBiome Purity2 = new CaptureBiome(10, 0);

			public static CaptureBiome Purity3 = new CaptureBiome(11, 0);

			public static CaptureBiome Purity4 = new CaptureBiome(12, 0);

			public static CaptureBiome Corruption = new CaptureBiome(1, 2, TileColorStyle.Corrupt);

			public static CaptureBiome Jungle = new CaptureBiome(3, 3, TileColorStyle.Jungle);

			public static CaptureBiome Hallow = new CaptureBiome(6, 4);

			public static CaptureBiome Snow = new CaptureBiome(7, 5);

			public static CaptureBiome Desert = new CaptureBiome(2, 6);

			public static CaptureBiome DirtLayer = new CaptureBiome(0, 7);

			public static CaptureBiome RockLayer = new CaptureBiome(0, 8);

			public static CaptureBiome BloodMoon = new CaptureBiome(0, 9);

			public static CaptureBiome Crimson = new CaptureBiome(8, 10, TileColorStyle.Crimson);

			public static CaptureBiome UndergroundDesert = new CaptureBiome(2, 12);

			public static CaptureBiome Ocean = new CaptureBiome(4, 13);

			public static CaptureBiome Mushroom = new CaptureBiome(9, 7, TileColorStyle.Mushroom);
		}

		private enum BiomeChoiceIndex
		{
			AutomatedForPlayer = -1,
			Purity = 1,
			Corruption = 2,
			Jungle = 3,
			Hallow = 4,
			Snow = 5,
			Desert = 6,
			DirtLayer = 7,
			RockLayer = 8,
			Crimson = 9,
			UndergroundDesert = 10,
			Ocean = 11,
			Mushroom = 12
		}

		public static readonly CaptureBiome DefaultPurity = new CaptureBiome(0, 0);

		public static CaptureBiome[] BiomesByWaterStyle = new CaptureBiome[15]
		{
			null,
			null,
			Styles.Corruption,
			Styles.Jungle,
			Styles.Hallow,
			Styles.Snow,
			Styles.Desert,
			Styles.DirtLayer,
			Styles.RockLayer,
			Styles.BloodMoon,
			Styles.Crimson,
			null,
			Styles.UndergroundDesert,
			Styles.Ocean,
			Styles.Mushroom
		};

		public readonly int WaterStyle;

		public readonly int BackgroundIndex;

		public readonly TileColorStyle TileColor;

		public CaptureBiome(int backgroundIndex, int waterStyle, TileColorStyle tileColorStyle = TileColorStyle.Normal)
		{
			BackgroundIndex = backgroundIndex;
			WaterStyle = waterStyle;
			TileColor = tileColorStyle;
		}

		public static CaptureBiome GetCaptureBiome(int biomeChoice)
		{
			switch (biomeChoice)
			{
			case 1:
				return GetPurityForPlayer();
			case 2:
				return Styles.Corruption;
			case 3:
				return Styles.Jungle;
			case 4:
				return Styles.Hallow;
			case 5:
				return Styles.Snow;
			case 6:
				return Styles.Desert;
			case 7:
				return Styles.DirtLayer;
			case 8:
				return Styles.RockLayer;
			case 9:
				return Styles.Crimson;
			case 10:
				return Styles.UndergroundDesert;
			case 11:
				return Styles.Ocean;
			case 12:
				return Styles.Mushroom;
			default:
			{
				CaptureBiome biomeByLocation = GetBiomeByLocation();
				if (biomeByLocation != null)
				{
					return biomeByLocation;
				}
				CaptureBiome biomeByWater = GetBiomeByWater();
				if (biomeByWater != null)
				{
					return biomeByWater;
				}
				return GetPurityForPlayer();
			}
			}
		}

		private static CaptureBiome GetBiomeByWater()
		{
			int num = Main.CalculateWaterStyle(ignoreFountains: true);
			for (int i = 0; i < BiomesByWaterStyle.Length; i++)
			{
				CaptureBiome captureBiome = BiomesByWaterStyle[i];
				if (captureBiome != null && captureBiome.WaterStyle == num)
				{
					return captureBiome;
				}
			}
			return null;
		}

		private static CaptureBiome GetBiomeByLocation()
		{
			return Main.GetPreferredBGStyleForPlayer() switch
			{
				0 => Styles.Purity, 
				10 => Styles.Purity2, 
				11 => Styles.Purity3, 
				12 => Styles.Purity4, 
				1 => Styles.Corruption, 
				2 => Styles.Desert, 
				3 => Styles.Jungle, 
				4 => Styles.Ocean, 
				5 => Styles.Desert, 
				6 => Styles.Hallow, 
				7 => Styles.Snow, 
				8 => Styles.Crimson, 
				9 => Styles.Mushroom, 
				_ => null, 
			};
		}

		private static CaptureBiome GetPurityForPlayer()
		{
			int num = (int)Main.LocalPlayer.Center.X / 16;
			if (num < Main.treeX[0])
			{
				return Styles.Purity;
			}
			if (num < Main.treeX[1])
			{
				return Styles.Purity2;
			}
			if (num < Main.treeX[2])
			{
				return Styles.Purity3;
			}
			return Styles.Purity4;
		}
	}
}
