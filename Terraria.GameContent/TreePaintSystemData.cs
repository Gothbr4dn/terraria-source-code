namespace Terraria.GameContent
{
	public static class TreePaintSystemData
	{
		private static TreePaintingSettings DefaultNoSpecialGroups = new TreePaintingSettings
		{
			UseSpecialGroups = false
		};

		private static TreePaintingSettings DefaultNoSpecialGroups_ForWalls = new TreePaintingSettings
		{
			UseSpecialGroups = false,
			UseWallShaderHacks = true
		};

		private static TreePaintingSettings DefaultDirt = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.03f,
			SpecialGroupMaximumHueValue = 0.08f,
			SpecialGroupMinimumSaturationValue = 0.38f,
			SpecialGroupMaximumSaturationValue = 0.53f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings CullMud = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			HueTestOffset = 0.5f,
			SpecialGroupMinimalHueValue = 0.42f,
			SpecialGroupMaximumHueValue = 0.55f,
			SpecialGroupMinimumSaturationValue = 0.2f,
			SpecialGroupMaximumSaturationValue = 0.27f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings WoodPurity = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 1f / 6f,
			SpecialGroupMaximumHueValue = 5f / 6f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		private static TreePaintingSettings WoodCorruption = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.5f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0.27f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		private static TreePaintingSettings WoodJungle = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 1f / 6f,
			SpecialGroupMaximumHueValue = 5f / 6f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		private static TreePaintingSettings WoodHallow = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.34f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings WoodSnow = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 5f / 72f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		private static TreePaintingSettings WoodCrimson = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 1f / 3f,
			SpecialGroupMaximumHueValue = 2f / 3f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings WoodJungleUnderground = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 1f / 6f,
			SpecialGroupMaximumHueValue = 5f / 6f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		private static TreePaintingSettings WoodGlowingMushroom = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.5f,
			SpecialGroupMaximumHueValue = 5f / 6f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		private static TreePaintingSettings VanityCherry = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 5f / 6f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		private static TreePaintingSettings VanityYellowWillow = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 0.025f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings TreeAsh = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 0.025f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings GemTreeRuby = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings GemTreeAmber = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings GemTreeSapphire = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings GemTreeEmerald = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings GemTreeAmethyst = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings GemTreeTopaz = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings GemTreeDiamond = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 0.0027777778f,
			InvertSpecialGroupResult = true
		};

		private static TreePaintingSettings PalmTreePurity = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 11f / 72f,
			SpecialGroupMaximumHueValue = 0.25f,
			SpecialGroupMinimumSaturationValue = 0.88f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		private static TreePaintingSettings PalmTreeCorruption = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0f,
			SpecialGroupMaximumHueValue = 1f,
			SpecialGroupMinimumSaturationValue = 0.4f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		private static TreePaintingSettings PalmTreeCrimson = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			HueTestOffset = 0.5f,
			SpecialGroupMinimalHueValue = 1f / 3f,
			SpecialGroupMaximumHueValue = 19f / 36f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		private static TreePaintingSettings PalmTreeHallow = new TreePaintingSettings
		{
			UseSpecialGroups = true,
			SpecialGroupMinimalHueValue = 0.5f,
			SpecialGroupMaximumHueValue = 11f / 18f,
			SpecialGroupMinimumSaturationValue = 0f,
			SpecialGroupMaximumSaturationValue = 1f
		};

		public static TreePaintingSettings GetTileSettings(int tileType, int tileStyle)
		{
			switch (tileType)
			{
			default:
				return DefaultNoSpecialGroups;
			case 0:
			case 2:
			case 23:
			case 109:
			case 199:
			case 477:
			case 492:
			case 633:
				return DefaultDirt;
			case 59:
			case 60:
			case 70:
				return CullMud;
			case 5:
				return tileStyle switch
				{
					0 => WoodCorruption, 
					1 => WoodJungle, 
					2 => WoodHallow, 
					3 => WoodSnow, 
					4 => WoodCrimson, 
					5 => WoodJungleUnderground, 
					6 => WoodGlowingMushroom, 
					_ => WoodPurity, 
				};
			case 323:
				switch (tileStyle)
				{
				default:
					return WoodPurity;
				case 0:
				case 4:
					return PalmTreePurity;
				case 1:
				case 5:
					return PalmTreeCrimson;
				case 2:
				case 6:
					return PalmTreeHallow;
				case 3:
				case 7:
					return PalmTreeCorruption;
				}
			case 587:
				return GemTreeRuby;
			case 588:
				return GemTreeDiamond;
			case 584:
				return GemTreeAmethyst;
			case 583:
				return GemTreeTopaz;
			case 585:
				return GemTreeSapphire;
			case 586:
				return GemTreeEmerald;
			case 589:
				return GemTreeAmber;
			case 595:
			case 596:
				return VanityCherry;
			case 615:
			case 616:
				return VanityYellowWillow;
			case 634:
				return TreeAsh;
			}
		}

		public static TreePaintingSettings GetTreeFoliageSettings(int foliageIndex, int foliageStyle)
		{
			switch (foliageIndex)
			{
			default:
				return DefaultDirt;
			case 0:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				return WoodPurity;
			case 15:
			case 21:
				switch (foliageStyle)
				{
				default:
					return WoodPurity;
				case 0:
				case 4:
					return PalmTreePurity;
				case 1:
				case 5:
					return PalmTreeCrimson;
				case 2:
				case 6:
					return PalmTreeHallow;
				case 3:
				case 7:
					return PalmTreeCorruption;
				}
			case 2:
			case 11:
			case 13:
				return WoodJungle;
			case 1:
				return WoodCorruption;
			case 3:
			case 19:
			case 20:
				return WoodHallow;
			case 4:
			case 12:
			case 16:
			case 17:
			case 18:
				return WoodSnow;
			case 5:
				return WoodCrimson;
			case 14:
				return WoodGlowingMushroom;
			case 22:
				return GemTreeTopaz;
			case 23:
				return GemTreeAmethyst;
			case 24:
				return GemTreeSapphire;
			case 25:
				return GemTreeEmerald;
			case 26:
				return GemTreeRuby;
			case 27:
				return GemTreeDiamond;
			case 28:
				return GemTreeAmber;
			case 29:
				return VanityCherry;
			case 30:
				return VanityYellowWillow;
			}
		}

		public static TreePaintingSettings GetWallSettings(int wallType)
		{
			return DefaultNoSpecialGroups_ForWalls;
		}
	}
}
