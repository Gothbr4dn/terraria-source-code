namespace Terraria.ID
{
	public class WallID
	{
		public static class Sets
		{
			public static class Conversion
			{
				public static bool[] Grass = Factory.CreateBoolSet(63, 64, 65, 66, 67, 68, 69, 70, 81, 264, 265, 268);

				public static bool[] Stone = Factory.CreateBoolSet(1, 61, 185, 3, 28, 83, 262, 274, 246, 248, 269);

				public static bool[] Dirt = Factory.CreateBoolSet(2, 16);

				public static bool[] Snow = Factory.CreateBoolSet(40, 249);

				public static bool[] Ice = Factory.CreateBoolSet(71, 266);

				public static bool[] Sandstone = Factory.CreateBoolSet(187, 220, 222, 221, 275, 308, 310, 309);

				public static bool[] HardenedSand = Factory.CreateBoolSet(216, 217, 219, 218, 304, 305, 307, 306);

				public static bool[] PureSand = Factory.CreateBoolSet(216, 187, 304, 275);

				public static bool[] NewWall1 = Factory.CreateBoolSet(188, 192, 200, 204, 212, 276, 280, 288, 292, 300);

				public static bool[] NewWall2 = Factory.CreateBoolSet(189, 193, 201, 205, 213, 277, 281, 289, 293, 301);

				public static bool[] NewWall3 = Factory.CreateBoolSet(190, 194, 202, 206, 214, 278, 282, 290, 294, 302);

				public static bool[] NewWall4 = Factory.CreateBoolSet(191, 195, 203, 207, 215, 279, 283, 291, 295, 303);
			}

			public static SetFactory Factory = new SetFactory(347);

			public static bool[] CanBeConvertedToGlowingMushroom = Factory.CreateBoolSet(64, 67, 15, 247);

			public static bool[] Transparent = Factory.CreateBoolSet(88, 89, 90, 91, 92, 241);

			public static bool[] Corrupt = Factory.CreateBoolSet(69, 217, 220, 3);

			public static bool[] Crimson = Factory.CreateBoolSet(83, 81, 218, 221);

			public static bool[] Hallow = Factory.CreateBoolSet(70, 219, 222, 28);

			public static bool[] AllowsWind = Factory.CreateBoolSet(0, 150, 138, 145, 107, 152, 140, 139, 141, 106, 245, 315, 317);

			public static bool[] AllowsPlantsToGrow = Factory.CreateBoolSet(0, 150, 138, 145, 107, 152, 140, 139, 141, 106, 245, 315, 317, 63, 64, 65, 66, 67, 68, 69, 81, 70, 264, 268, 265, 74, 80);

			public static int[] BlendType = Factory.CreateIntSet(-1, 66, 63, 68, 63, 65, 63, 16, 2, 59, 2, 261, 2, 284, 196, 285, 197, 286, 198, 287, 199, 256, 54, 257, 55, 258, 56, 259, 57, 260, 58, 262, 61, 274, 185, 300, 212, 301, 213, 302, 214, 303, 215, 296, 208, 297, 209, 298, 210, 299, 211, 48, 1, 49, 1, 50, 1, 51, 1, 52, 1, 53, 1, 250, 1, 251, 1, 252, 1, 253, 1, 254, 1, 255, 1, 69, 264, 3, 246, 217, 305, 220, 308, 188, 276, 189, 277, 190, 278, 191, 279, 81, 77, 268, 77, 83, 269, 218, 306, 221, 309, 192, 280, 193, 281, 194, 282, 195, 283, 70, 265, 28, 248, 219, 307, 222, 310, 200, 288, 201, 289, 202, 290, 203, 291, 15, 247, 64, 67, 204, 292, 205, 293, 206, 294, 207, 295, 86, 108, 87, 112, 40, 249, 71, 266, 216, 304, 187, 275, 62, 263, 80, 74, 180, 184, 178, 183, 79, 267, 20, 14, 7, 17, 94, 17, 95, 17, 8, 18, 98, 18, 99, 18, 9, 19, 96, 19, 97, 19);
		}

		public const ushort None = 0;

		public const ushort Stone = 1;

		public const ushort DirtUnsafe = 2;

		public const ushort EbonstoneUnsafe = 3;

		public const ushort Wood = 4;

		public const ushort GrayBrick = 5;

		public const ushort RedBrick = 6;

		public const ushort BlueDungeonUnsafe = 7;

		public const ushort GreenDungeonUnsafe = 8;

		public const ushort PinkDungeonUnsafe = 9;

		public const ushort GoldBrick = 10;

		public const ushort SilverBrick = 11;

		public const ushort CopperBrick = 12;

		public const ushort HellstoneBrickUnsafe = 13;

		public const ushort ObsidianBrickUnsafe = 14;

		public const ushort MudUnsafe = 15;

		public const ushort Dirt = 16;

		public const ushort BlueDungeon = 17;

		public const ushort GreenDungeon = 18;

		public const ushort PinkDungeon = 19;

		public const ushort ObsidianBrick = 20;

		public const ushort Glass = 21;

		public const ushort PearlstoneBrick = 22;

		public const ushort IridescentBrick = 23;

		public const ushort MudstoneBrick = 24;

		public const ushort CobaltBrick = 25;

		public const ushort MythrilBrick = 26;

		public const ushort Planked = 27;

		public const ushort PearlstoneBrickUnsafe = 28;

		public const ushort CandyCane = 29;

		public const ushort GreenCandyCane = 30;

		public const ushort SnowBrick = 31;

		public const ushort AdamantiteBeam = 32;

		public const ushort DemoniteBrick = 33;

		public const ushort SandstoneBrick = 34;

		public const ushort EbonstoneBrick = 35;

		public const ushort RedStucco = 36;

		public const ushort YellowStucco = 37;

		public const ushort GreenStucco = 38;

		public const ushort Gray = 39;

		public const ushort SnowWallUnsafe = 40;

		public const ushort Ebonwood = 41;

		public const ushort RichMaogany = 42;

		public const ushort Pearlwood = 43;

		public const ushort RainbowBrick = 44;

		public const ushort TinBrick = 45;

		public const ushort TungstenBrick = 46;

		public const ushort PlatinumBrick = 47;

		public const ushort AmethystUnsafe = 48;

		public const ushort TopazUnsafe = 49;

		public const ushort SapphireUnsafe = 50;

		public const ushort EmeraldUnsafe = 51;

		public const ushort RubyUnsafe = 52;

		public const ushort DiamondUnsafe = 53;

		public const ushort CaveUnsafe = 54;

		public const ushort Cave2Unsafe = 55;

		public const ushort Cave3Unsafe = 56;

		public const ushort Cave4Unsafe = 57;

		public const ushort Cave5Unsafe = 58;

		public const ushort Cave6Unsafe = 59;

		public const ushort LivingLeaf = 60;

		public const ushort Cave7Unsafe = 61;

		public const ushort SpiderUnsafe = 62;

		public const ushort GrassUnsafe = 63;

		public const ushort JungleUnsafe = 64;

		public const ushort FlowerUnsafe = 65;

		public const ushort Grass = 66;

		public const ushort Jungle = 67;

		public const ushort Flower = 68;

		public const ushort CorruptGrassUnsafe = 69;

		public const ushort HallowedGrassUnsafe = 70;

		public const ushort IceUnsafe = 71;

		public const ushort Cactus = 72;

		public const ushort Cloud = 73;

		public const ushort Mushroom = 74;

		public const ushort Bone = 75;

		public const ushort Slime = 76;

		public const ushort Flesh = 77;

		public const ushort LivingWood = 78;

		public const ushort ObsidianBackUnsafe = 79;

		public const ushort MushroomUnsafe = 80;

		public const ushort CrimsonGrassUnsafe = 81;

		public const ushort DiscWall = 82;

		public const ushort CrimstoneUnsafe = 83;

		public const ushort IceBrick = 84;

		public const ushort Shadewood = 85;

		public const ushort HiveUnsafe = 86;

		public const ushort LihzahrdBrickUnsafe = 87;

		public const ushort PurpleStainedGlass = 88;

		public const ushort YellowStainedGlass = 89;

		public const ushort BlueStainedGlass = 90;

		public const ushort GreenStainedGlass = 91;

		public const ushort RedStainedGlass = 92;

		public const ushort RainbowStainedGlass = 93;

		public const ushort BlueDungeonSlabUnsafe = 94;

		public const ushort BlueDungeonTileUnsafe = 95;

		public const ushort PinkDungeonSlabUnsafe = 96;

		public const ushort PinkDungeonTileUnsafe = 97;

		public const ushort GreenDungeonSlabUnsafe = 98;

		public const ushort GreenDungeonTileUnsafe = 99;

		public const ushort BlueDungeonSlab = 100;

		public const ushort BlueDungeonTile = 101;

		public const ushort PinkDungeonSlab = 102;

		public const ushort PinkDungeonTile = 103;

		public const ushort GreenDungeonSlab = 104;

		public const ushort GreenDungeonTile = 105;

		public const ushort WoodenFence = 106;

		public const ushort MetalFence = 107;

		public const ushort Hive = 108;

		public const ushort PalladiumColumn = 109;

		public const ushort BubblegumBlock = 110;

		public const ushort TitanstoneBlock = 111;

		public const ushort LihzahrdBrick = 112;

		public const ushort Pumpkin = 113;

		public const ushort Hay = 114;

		public const ushort SpookyWood = 115;

		public const ushort ChristmasTreeWallpaper = 116;

		public const ushort OrnamentWallpaper = 117;

		public const ushort CandyCaneWallpaper = 118;

		public const ushort FestiveWallpaper = 119;

		public const ushort StarsWallpaper = 120;

		public const ushort SquigglesWallpaper = 121;

		public const ushort SnowflakeWallpaper = 122;

		public const ushort KrampusHornWallpaper = 123;

		public const ushort BluegreenWallpaper = 124;

		public const ushort GrinchFingerWallpaper = 125;

		public const ushort FancyGrayWallpaper = 126;

		public const ushort IceFloeWallpaper = 127;

		public const ushort MusicWallpaper = 128;

		public const ushort PurpleRainWallpaper = 129;

		public const ushort RainbowWallpaper = 130;

		public const ushort SparkleStoneWallpaper = 131;

		public const ushort StarlitHeavenWallpaper = 132;

		public const ushort BubbleWallpaper = 133;

		public const ushort CopperPipeWallpaper = 134;

		public const ushort DuckyWallpaper = 135;

		public const ushort Waterfall = 136;

		public const ushort Lavafall = 137;

		public const ushort EbonwoodFence = 138;

		public const ushort RichMahoganyFence = 139;

		public const ushort PearlwoodFence = 140;

		public const ushort ShadewoodFence = 141;

		public const ushort WhiteDynasty = 142;

		public const ushort BlueDynasty = 143;

		public const ushort ArcaneRunes = 144;

		public const ushort IronFence = 145;

		public const ushort CopperPlating = 146;

		public const ushort StoneSlab = 147;

		public const ushort Sail = 148;

		public const ushort BorealWood = 149;

		public const ushort BorealWoodFence = 150;

		public const ushort PalmWood = 151;

		public const ushort PalmWoodFence = 152;

		public const ushort AmberGemspark = 153;

		public const ushort AmethystGemspark = 154;

		public const ushort DiamondGemspark = 155;

		public const ushort EmeraldGemspark = 156;

		public const ushort AmberGemsparkOff = 157;

		public const ushort AmethystGemsparkOff = 158;

		public const ushort DiamondGemsparkOff = 159;

		public const ushort EmeraldGemsparkOff = 160;

		public const ushort RubyGemsparkOff = 161;

		public const ushort SapphireGemsparkOff = 162;

		public const ushort TopazGemsparkOff = 163;

		public const ushort RubyGemspark = 164;

		public const ushort SapphireGemspark = 165;

		public const ushort TopazGemspark = 166;

		public const ushort TinPlating = 167;

		public const ushort Confetti = 168;

		public const ushort ConfettiBlack = 169;

		public const ushort CaveWall = 170;

		public const ushort CaveWall2 = 171;

		public const ushort Honeyfall = 172;

		public const ushort ChlorophyteBrick = 173;

		public const ushort CrimtaneBrick = 174;

		public const ushort ShroomitePlating = 175;

		public const ushort MartianConduit = 176;

		public const ushort HellstoneBrick = 177;

		public const ushort MarbleUnsafe = 178;

		public const ushort MarbleBlock = 179;

		public const ushort GraniteUnsafe = 180;

		public const ushort GraniteBlock = 181;

		public const ushort MeteoriteBrick = 182;

		public const ushort Marble = 183;

		public const ushort Granite = 184;

		public const ushort Cave8Unsafe = 185;

		public const ushort Crystal = 186;

		public const ushort Sandstone = 187;

		public const ushort CorruptionUnsafe1 = 188;

		public const ushort CorruptionUnsafe2 = 189;

		public const ushort CorruptionUnsafe3 = 190;

		public const ushort CorruptionUnsafe4 = 191;

		public const ushort CrimsonUnsafe1 = 192;

		public const ushort CrimsonUnsafe2 = 193;

		public const ushort CrimsonUnsafe3 = 194;

		public const ushort CrimsonUnsafe4 = 195;

		public const ushort DirtUnsafe1 = 196;

		public const ushort DirtUnsafe2 = 197;

		public const ushort DirtUnsafe3 = 198;

		public const ushort DirtUnsafe4 = 199;

		public const ushort HallowUnsafe1 = 200;

		public const ushort HallowUnsafe2 = 201;

		public const ushort HallowUnsafe3 = 202;

		public const ushort HallowUnsafe4 = 203;

		public const ushort JungleUnsafe1 = 204;

		public const ushort JungleUnsafe2 = 205;

		public const ushort JungleUnsafe3 = 206;

		public const ushort JungleUnsafe4 = 207;

		public const ushort LavaUnsafe1 = 208;

		public const ushort LavaUnsafe2 = 209;

		public const ushort LavaUnsafe3 = 210;

		public const ushort LavaUnsafe4 = 211;

		public const ushort RocksUnsafe1 = 212;

		public const ushort RocksUnsafe2 = 213;

		public const ushort RocksUnsafe3 = 214;

		public const ushort RocksUnsafe4 = 215;

		public const ushort HardenedSand = 216;

		public const ushort CorruptHardenedSand = 217;

		public const ushort CrimsonHardenedSand = 218;

		public const ushort HallowHardenedSand = 219;

		public const ushort CorruptSandstone = 220;

		public const ushort CrimsonSandstone = 221;

		public const ushort HallowSandstone = 222;

		public const ushort DesertFossil = 223;

		public const ushort LunarBrickWall = 224;

		public const ushort CogWall = 225;

		public const ushort SandFall = 226;

		public const ushort SnowFall = 227;

		public const ushort SillyBalloonPinkWall = 228;

		public const ushort SillyBalloonPurpleWall = 229;

		public const ushort SillyBalloonGreenWall = 230;

		public const ushort IronBrick = 231;

		public const ushort LeadBrick = 232;

		public const ushort LesionBlock = 233;

		public const ushort CrimstoneBrick = 234;

		public const ushort SmoothSandstone = 235;

		public const ushort Spider = 236;

		public const ushort SolarBrick = 237;

		public const ushort VortexBrick = 238;

		public const ushort NebulaBrick = 239;

		public const ushort StardustBrick = 240;

		public const ushort OrangeStainedGlass = 241;

		public const ushort GoldStarryGlassWall = 242;

		public const ushort BlueStarryGlassWall = 243;

		public const ushort LivingWoodUnsafe = 244;

		public const ushort WroughtIronFence = 245;

		public const ushort EbonstoneEcho = 246;

		public const ushort MudWallEcho = 247;

		public const ushort PearlstoneEcho = 248;

		public const ushort SnowWallEcho = 249;

		public const ushort AmethystEcho = 250;

		public const ushort TopazEcho = 251;

		public const ushort SapphireEcho = 252;

		public const ushort EmeraldEcho = 253;

		public const ushort RubyEcho = 254;

		public const ushort DiamondEcho = 255;

		public const ushort Cave1Echo = 256;

		public const ushort Cave2Echo = 257;

		public const ushort Cave3Echo = 258;

		public const ushort Cave4Echo = 259;

		public const ushort Cave5Echo = 260;

		public const ushort Cave6Echo = 261;

		public const ushort Cave7Echo = 262;

		public const ushort SpiderEcho = 263;

		public const ushort CorruptGrassEcho = 264;

		public const ushort HallowedGrassEcho = 265;

		public const ushort IceEcho = 266;

		public const ushort ObsidianBackEcho = 267;

		public const ushort CrimsonGrassEcho = 268;

		public const ushort CrimstoneEcho = 269;

		public const ushort CaveWall1Echo = 270;

		public const ushort CaveWall2Echo = 271;

		public const ushort MarbleEchoUnused = 272;

		public const ushort GraniteEchoUnused = 273;

		public const ushort Cave8Echo = 274;

		public const ushort SandstoneEcho = 275;

		public const ushort Corruption1Echo = 276;

		public const ushort Corruption2Echo = 277;

		public const ushort Corruption3Echo = 278;

		public const ushort Corruption4Echo = 279;

		public const ushort Crimson1Echo = 280;

		public const ushort Crimson2Echo = 281;

		public const ushort Crimson3Echo = 282;

		public const ushort Crimson4Echo = 283;

		public const ushort Dirt1Echo = 284;

		public const ushort Dirt2Echo = 285;

		public const ushort Dirt3Echo = 286;

		public const ushort Dirt4Echo = 287;

		public const ushort Hallow1Echo = 288;

		public const ushort Hallow2Echo = 289;

		public const ushort Hallow3Echo = 290;

		public const ushort Hallow4Echo = 291;

		public const ushort Jungle1Echo = 292;

		public const ushort Jungle2Echo = 293;

		public const ushort Jungle3Echo = 294;

		public const ushort Jungle4Echo = 295;

		public const ushort Lava1Echo = 296;

		public const ushort Lava2Echo = 297;

		public const ushort Lava3Echo = 298;

		public const ushort Lava4Echo = 299;

		public const ushort Rocks1Echo = 300;

		public const ushort Rocks2Echo = 301;

		public const ushort Rocks3Echo = 302;

		public const ushort Rocks4Echo = 303;

		public const ushort HardenedSandEcho = 304;

		public const ushort CorruptHardenedSandEcho = 305;

		public const ushort CrimsonHardenedSandEcho = 306;

		public const ushort HallowHardenedSandEcho = 307;

		public const ushort CorruptSandstoneEcho = 308;

		public const ushort CrimsonSandstoneEcho = 309;

		public const ushort HallowSandstoneEcho = 310;

		public const ushort DesertFossilEcho = 311;

		public const ushort BambooBlockWall = 312;

		public const ushort LargeBambooBlockWall = 313;

		public const ushort AmberStoneWallEcho = 314;

		public const ushort BambooFence = 315;

		public const ushort AshWood = 316;

		public const ushort AshWoodFence = 317;

		public const ushort EchoWall = 318;

		public const ushort ReefWall = 319;

		public const ushort PoopWall = 320;

		public const ushort ShimmerBlockWall = 321;

		public const ushort ShimmerBrickWall = 322;

		public const ushort LunarRustBrickWall = 323;

		public const ushort DarkCelestialBrickWall = 324;

		public const ushort AstraBrickWall = 325;

		public const ushort CosmicEmberBrickWall = 326;

		public const ushort CryocoreBrickWall = 327;

		public const ushort MercuryBrickWall = 328;

		public const ushort StarRoyaleBrickWall = 329;

		public const ushort HeavenforgeBrickWall = 330;

		public const ushort AncientBlueBrickWall = 331;

		public const ushort AncientGreenBrickWall = 332;

		public const ushort AncientPinkBrickWall = 333;

		public const ushort AncientGoldBrickWall = 334;

		public const ushort AncientSilverBrickWall = 335;

		public const ushort AncientCopperBrickWall = 336;

		public const ushort AncientObsidianBrickWall = 337;

		public const ushort AncientHellstoneBrickWall = 338;

		public const ushort AncientCobaltBrickWall = 339;

		public const ushort AncientMythrilBrickWall = 340;

		public const ushort LavaMossBlockWall = 341;

		public const ushort ArgonMossBlockWall = 342;

		public const ushort KryptonMossBlockWall = 343;

		public const ushort XenonMossBlockWall = 344;

		public const ushort VioletMossBlockWall = 345;

		public const ushort RainbowMossBlockWall = 346;

		public const ushort Count = 347;
	}
}
