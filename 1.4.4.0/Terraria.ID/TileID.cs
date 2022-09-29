using System.Collections.Generic;
using ReLogic.Reflection;

namespace Terraria.ID
{
	public class TileID
	{
		public static class Sets
		{
			public static class Conversion
			{
				public static bool[] MergesWithDirtInASpecialWay = Factory.CreateBoolSet(2, 23, 109, 199, 477, 492);

				public static bool[] JungleGrass = Factory.CreateBoolSet(60, 661, 662);

				public static bool[] MushroomGrass = Factory.CreateBoolSet(70);

				public static bool[] Grass = Factory.CreateBoolSet(2, 23, 199, 109, 477, 492);

				public static bool[] GolfGrass = Factory.CreateBoolSet(477, 492);

				public static bool[] Dirt = Factory.CreateBoolSet(default(int));

				public static bool[] Snow = Factory.CreateBoolSet(147);

				public static bool[] Stone = Factory.CreateBoolSet(1, 25, 117, 203);

				public static bool[] Ice = Factory.CreateBoolSet(161, 163, 164, 200);

				public static bool[] Sand = Factory.CreateBoolSet(53, 112, 116, 234);

				public static bool[] HardenedSand = Factory.CreateBoolSet(397, 398, 402, 399);

				public static bool[] Sandstone = Factory.CreateBoolSet(396, 400, 403, 401);

				public static bool[] Thorn = Factory.CreateBoolSet(32, 352, 69, 655);

				public static bool[] Moss = Factory.CreateBoolSet(182, 180, 179, 381, 183, 181, 534, 536, 539, 625, 627);

				public static bool[] MossBrick = Factory.CreateBoolSet(512, 513, 514, 515, 516, 517, 535, 537, 540, 626, 628);
			}

			public static class TileCutIgnore
			{
				public static bool[] None = Factory.CreateBoolSet(false);

				public static bool[] IgnoreDontHurtNature = Factory.CreateBoolSet(true, 654, 444, 485, 231, 484, 32, 352, 655, 69, 51, 481, 482, 483);

				public static bool[] Regrowth = Factory.CreateBoolSet(false, 3, 24, 52, 61, 62, 71, 73, 74, 82, 83, 84, 110, 113, 115, 184, 205, 201, 519, 518, 528, 529, 530, 549, 637, 638, 636);
			}

			public static class ForAdvancedCollision
			{
				public static bool[] ForSandshark = Factory.CreateBoolSet(397, 398, 402, 399, 396, 400, 403, 401, 53, 112, 116, 234, 407, 404);
			}

			public static class RoomNeeds
			{
				public static int[] CountsAsChair = new int[6] { 15, 79, 89, 102, 487, 497 };

				public static int[] CountsAsTable = new int[11]
				{
					14, 18, 87, 88, 90, 101, 354, 355, 464, 469,
					487
				};

				public static int[] CountsAsTorch = new int[26]
				{
					4, 33, 34, 35, 42, 49, 93, 95, 98, 100,
					149, 173, 174, 270, 271, 316, 317, 318, 92, 372,
					646, 405, 592, 572, 581, 660
				};

				public static int[] CountsAsDoor = new int[13]
				{
					10, 11, 19, 387, 386, 388, 389, 436, 435, 438,
					427, 439, 437
				};
			}

			public static SetFactory Factory = new SetFactory(693);

			public static bool[] IceSkateSlippery = Factory.CreateBoolSet(161, 162, 127, 163, 164, 200, 659);

			public static bool[] DontDrawTileSliced = Factory.CreateBoolSet(false, 137, 235, 388, 476, 160, 138, 664, 665, 630, 631);

			public static bool[] AllowsSaveCompressionBatching = Factory.CreateBoolSet(true, 520, 423);

			public static bool[] CountsAsGemTree = Factory.CreateBoolSet(false, 583, 584, 585, 586, 587, 588, 589);

			public static bool[] IsATreeTrunk = Factory.CreateBoolSet(false, 5, 72, 583, 584, 585, 586, 587, 588, 589, 596, 616, 634);

			public static bool[] IsShakeable = Factory.CreateBoolSet(false, 5, 72, 323, 583, 584, 585, 586, 587, 588, 589, 596, 616, 634);

			public static bool[] GetsDestroyedForMeteors = Factory.CreateBoolSet(false, 5, 32, 352, 583, 584, 585, 586, 587, 588, 589, 596, 616, 634);

			public static bool[] GetsCheckedForLeaves = Factory.CreateBoolSet(false, 5, 323, 72, 583, 584, 585, 586, 587, 588, 589, 596, 616, 634);

			public static bool[] PreventsTileRemovalIfOnTopOfIt = Factory.CreateBoolSet(false, 5, 323, 72, 488, 26, 583, 584, 585, 586, 587, 588, 589, 596, 616, 470, 475, 634);

			public static bool[] PreventsTileReplaceIfOnTopOfIt = Factory.CreateBoolSet(false, 5, 323, 72, 583, 584, 585, 586, 587, 588, 589, 596, 616, 634);

			public static bool[] CommonSapling = Factory.CreateBoolSet(false, 20, 590, 595, 615);

			public static bool[] AllBlocksWithSmoothBordersToResolveHalfBlockIssue = Factory.CreateBoolSet(false, 321, 157, 208, 159, 190, 80, 251, 202, 229, 56, 38, 39, 152, 118, 151, 148, 206, 119, 175, 46, 176, 45, 140, 347, 370, 121, 122, 680, 681, 682, 685, 686, 346, 687, 688, 689, 690, 691, 692, 160, 226, 54, 156, 155, 153, 154, 150, 250, 198, 273, 274, 325, 284, 348, 385, 327, 326, 345, 328, 329, 357, 369, 268, 261, 262, 255, 267, 260, 265, 258, 266, 259, 264, 257, 263, 256, 311, 431, 426, 433, 430, 434, 432, 272, 145, 146, 350, 127, 472, 473, 477, 478, 479, 492, 496, 507, 508, 563, 618);

			public static bool[] CanBeDugByShovel = Factory.CreateBoolSet(false, 0, 668, 59, 57, 123, 224, 147, 2, 109, 23, 661, 199, 662, 60, 70, 477, 492, 53, 116, 112, 234, 40, 495, 633);

			public static bool[] NonSolidSaveSlopes = Factory.CreateBoolSet(false, 131, 351);

			public static bool[] ResetsHalfBrickPlacementAttempt = Factory.CreateBoolSet(true, 2, 23, 661, 60, 70, 199, 662, 109, 477, 492, 179, 512, 180, 513, 181, 514, 182, 515, 183, 516, 381, 517, 534, 535, 536, 537, 539, 540, 625, 626, 627, 628, 633);

			public static bool[] CrackedBricks = Factory.CreateBoolSet(481, 482, 483);

			public static bool[] ForcedDirtMerging = Factory.CreateBoolSet(75, 76, 508, 507, 226, 409, 669, 670, 671, 672, 673, 674, 675, 676, 683, 684, 273, 274, 459, 458, 326, 327, 345, 328, 329, 192, 384, 284, 325, 272, 268, 262, 267, 265, 266, 264, 263, 261, 255, 260, 258, 259, 257, 256, 54, 357);

			public static bool[] Paintings = Factory.CreateBoolSet(245, 246, 240, 241, 242);

			public static bool[] isDesertBiomeSand = Factory.CreateBoolSet(53, 397, 396, 400, 403, 401);

			public static bool[] MergesWithClouds = Factory.CreateBoolSet(196, 460);

			public static bool[] Boulders = Factory.CreateBoolSet(138, 484, 664, 665);

			public static bool[] Clouds = Factory.CreateBoolSet(189, 196, 460);

			public static int[] CritterCageLidStyle = Factory.CreateIntSet(-1, 275, 0, 276, 0, 277, 0, 278, 0, 279, 0, 280, 0, 281, 0, 558, 0, 554, 0, 553, 0, 551, 0, 550, 0, 542, 0, 413, 0, 309, 0, 297, 0, 296, 0, 645, 0, 643, 0, 644, 0, 632, 0, 640, 0, 559, 1, 414, 1, 359, 1, 358, 1, 605, 2, 604, 2, 603, 2, 602, 2, 601, 2, 599, 2, 600, 2, 612, 2, 611, 2, 610, 2, 609, 2, 608, 2, 606, 2, 607, 2, 285, 3, 286, 3, 582, 3, 555, 3, 538, 3, 533, 3, 532, 3, 394, 3, 393, 3, 392, 3, 391, 3, 339, 3, 310, 3, 299, 3, 298, 3, 629, 3, 619, 3, 556, 4, 544, 4, 364, 4, 363, 4, 362, 4, 361, 4);

			public static int[] SmartCursorPickaxePriorityOverride = Factory.CreateIntSet(0, 12, 1, 665, 1, 639, 1);

			public static bool[] IgnoreSmartCursorPriorityAxe = Factory.CreateBoolSet(false, 488);

			public static bool[] CanBeSatOnForNPCs = Factory.CreateBoolSet(false, 15, 497);

			public static bool[] CanBeSatOnForPlayers = Factory.CreateBoolSet(false, 15, 497, 89, 102, 487);

			public static bool[] CanBeSleptIn = Factory.CreateBoolSet(false, 79);

			public static bool[] IgnoresTileReplacementDropCheckWhenBeingPlaced = Factory.CreateBoolSet(false, 158, 30);

			public static bool?[] DrawTileInSolidLayer = Factory.CreateCustomSet<bool?>(null, new object[10]
			{
				(ushort)11,
				true,
				(ushort)470,
				true,
				(ushort)475,
				true,
				(ushort)78,
				true,
				(ushort)579,
				true
			});

			public static bool[] DoesntPlaceWithTileReplacement = Factory.CreateBoolSet(2, 60, 70, 109, 199, 23, 661, 662, 633);

			public static bool[] DoesntGetReplacedWithTileReplacement = Factory.CreateBoolSet(58, 225, 171, 127, 481, 482, 483, 423);

			public static bool[] IsVine = Factory.CreateBoolSet(52, 382, 62, 115, 205, 528, 636, 638);

			public static bool[] IsBeam = Factory.CreateBoolSet(124, 561, 574, 575, 576, 577, 578);

			public static bool[] Platforms = Factory.CreateBoolSet(19, 427, 435, 436, 437, 438, 439);

			public static bool[] ReplaceTileBreakUp = Factory.CreateBoolSet(27, 20, 227, 24, 201, 110, 113, 61, 74, 71, 3, 73, 186, 187, 185, 233, 530, 236, 238, 254, 484, 485, 84, 82, 83, 529, 549, 590, 595, 615, 624, 637);

			public static bool[] ReplaceTileBreakDown = Factory.CreateBoolSet(205, 115, 62, 52, 382, 444, 528, 638, 636);

			public static bool[] SlowlyDiesInWater = Factory.CreateBoolSet(3, 20, 24, 27, 73, 201, 80, 110, 529, 530, 590, 595, 615, 637);

			public static bool[] DrawsWalls = Factory.CreateBoolSet(10, 54, 138, 664, 484, 388, 191, 137, 328, 162, 387, 48, 232, 127, 459, 541, 546);

			public static ushort[] GemsparkFramingTypes = Factory.CreateUshortSet(0, 265, 265, 258, 258, 264, 264, 257, 257, 267, 267, 260, 260, 266, 266, 259, 259, 263, 263, 256, 256, 262, 262, 255, 255, 268, 268, 261, 261, 385, 385, 446, 446, 447, 447, 448, 448);

			public static bool[] TeamTiles = Factory.CreateBoolSet(426, 430, 431, 432, 433, 434, 427, 435, 436, 437, 438, 439);

			public static int[] ConveyorDirection = Factory.CreateIntSet(0, 421, 1, 422, -1);

			public static bool[] VineThreads = Factory.CreateBoolSet(382, 62, 115, 205, 52, 528, 636, 638);

			public static bool[] ReverseVineThreads = Factory.CreateBoolSet(549);

			public static bool[] HasSlopeFrames = Factory.CreateBoolSet(421, 422);

			public static bool[] TileInteractRead = Factory.CreateBoolSet(55, 85, 425, 573);

			public static bool[] IgnoresNearbyHalfbricksWhenDrawn = Factory.CreateBoolSet(380, 476, 235, 138, 664, 137, 484, 421, 422);

			public static bool[] SwaysInWindBasic = Factory.CreateBoolSet(3, 20, 24, 61, 71, 73, 74, 83, 84, 110, 113, 201, 227, 529, 590, 595, 615, 624, 656, 637);

			public static int[] DrawFlipMode = Factory.CreateIntSet(0, 3, 1, 13, 1, 20, 1, 24, 1, 49, 1, 372, 1, 646, 1, 50, 1, 52, 1, 61, 1, 62, 1, 71, 1, 73, 1, 74, 1, 81, 1, 82, 1, 83, 1, 84, 1, 91, 1, 92, 1, 93, 1, 110, 1, 113, 1, 115, 1, 135, 1, 141, 1, 165, 1, 174, 1, 201, 1, 205, 1, 227, 1, 270, 1, 271, 1, 382, 1, 184, 2, 185, 3, 528, 1, 529, 1, 590, 1, 595, 1, 615, 1, 624, 1, 638, 1, 636, 1, 656, 1);

			public static bool[] HasOutlines = Factory.CreateBoolSet(10, 11, 15, 21, 29, 55, 79, 85, 88, 89, 97, 102, 104, 125, 132, 136, 139, 144, 207, 209, 212, 215, 216, 237, 287, 335, 338, 354, 356, 377, 386, 387, 388, 389, 410, 411, 425, 441, 455, 463, 467, 468, 470, 475, 487, 480, 494, 497, 509, 510, 511, 621, 464, 657, 658, 663);

			public static bool[] AllTiles = Factory.CreateBoolSet(true);

			public static bool[] Dirt = Factory.CreateBoolSet(0, 668);

			public static bool[] Mud = Factory.CreateBoolSet(59);

			public static bool[] Ash = Factory.CreateBoolSet(57);

			public static bool[] Snow = Factory.CreateBoolSet(147);

			public static bool[] Ices = Factory.CreateBoolSet(161, 200, 163, 164);

			public static bool[] IcesSlush = Factory.CreateBoolSet(161, 200, 163, 164, 224);

			public static bool[] IcesSnow = Factory.CreateBoolSet(161, 200, 163, 164, 147);

			public static bool[] GrassSpecial = Factory.CreateBoolSet(70, 60, 661, 662);

			public static bool[] JungleSpecial = Factory.CreateBoolSet(226, 225, 211);

			public static bool[] HellSpecial = Factory.CreateBoolSet(58, 76, 75);

			public static bool[] Leaves = Factory.CreateBoolSet(384, 192);

			public static bool[] tileMossBrick = Factory.CreateBoolSet(false, 512, 513, 514, 515, 516, 517, 535, 537, 540, 626, 628);

			public static bool[] GeneralPlacementTiles = Factory.CreateBoolSet(true, 225, 41, 481, 43, 482, 44, 483, 226, 203, 112, 25, 70, 151, 21, 31, 467, 12, 665, 639);

			public static bool[] BasicChest = Factory.CreateBoolSet(21, 467);

			public static bool[] BasicChestFake = Factory.CreateBoolSet(441, 468);

			public static bool[] BasicDresser = Factory.CreateBoolSet(88);

			public static bool[] CanBeClearedDuringGeneration = Factory.CreateBoolSet(true, 396, 400, 401, 397, 398, 399, 404, 368, 367, 226, 237);

			public static List<int> CorruptCountCollection = new List<int> { 23, 661, 25, 112, 163, 398, 400, 636, 24, 32 };

			public static bool[] CorruptBiomeSight = Factory.CreateBoolSet(23, 661, 25, 112, 163, 398, 400, 636, 24, 32);

			public static bool[] Corrupt = Factory.CreateBoolSet(23, 661, 25, 112, 163, 398, 400, 636);

			public static List<int> HallowCountCollection = new List<int> { 109, 117, 116, 164, 402, 403, 115, 110, 113 };

			public static bool[] HallowBiomeSight = Factory.CreateBoolSet(109, 117, 116, 164, 402, 403, 115, 110, 113);

			public static bool[] Hallow = Factory.CreateBoolSet(109, 117, 116, 164, 402, 403, 115);

			public static bool[] CanGrowCrystalShards = Factory.CreateBoolSet(117, 116, 164, 402, 403);

			public static List<int> CrimsonCountCollection = new List<int> { 199, 662, 203, 234, 200, 399, 401, 205, 201, 352 };

			public static bool[] CrimsonBiomeSight = Factory.CreateBoolSet(199, 662, 203, 234, 200, 399, 401, 205, 201, 352);

			public static bool[] Crimson = Factory.CreateBoolSet(199, 662, 203, 234, 200, 399, 401, 205);

			public static bool[] IsSkippedForNPCSpawningGroundTypeCheck = Factory.CreateBoolSet(false, 421, 422);

			public static bool[] BlocksStairs = Factory.CreateBoolSet(386, 387, 54, 541);

			public static bool[] BlocksStairsAbove = Factory.CreateBoolSet(386, 387);

			public static bool[] NotReallySolid = Factory.CreateBoolSet(387, 388, 10);

			public static bool[] BlocksWaterDrawingBehindSelf = Factory.CreateBoolSet(false, 54, 541, 328, 470);

			public static bool[] AllowLightInWater = Factory.CreateBoolSet(false, 54, 541, 328);

			public static bool[] NeedsGrassFraming = Factory.CreateBoolSet(633);

			public static int[] NeedsGrassFramingDirt = Factory.CreateIntSet(0, 633, 57);

			public static bool[] ChecksForMerge = Factory.CreateBoolSet(0, 668, 2, 661, 60, 70, 199, 662, 109, 477, 492, 633, 57, 58, 75, 76, 684, 147, 161, 164, 163, 200, 162, 189, 196, 460, 224, 191, 383, 211, 225, 59, 226, 396, 397, 398, 399, 402, 400, 401, 403, 404, 234, 112, 407);

			public static bool[] FramesOnKillWall = Factory.CreateBoolSet(440, 240, 241, 242, 245, 246, 4, 136, 334, 132, 55, 395, 425, 440, 471, 510, 511, 573, 630, 631);

			public static bool[] AvoidedByNPCs = Factory.CreateBoolSet(21, 467, 55, 85, 395, 88, 463, 334, 29, 97, 99, 356, 663, 425, 440, 209, 441, 468, 471, 491, 510, 511, 520, 573);

			public static bool[] InteractibleByNPCs = Factory.CreateBoolSet(17, 77, 133, 12, 665, 639, 26, 35, 36, 55, 395, 471, 21, 467, 29, 97, 88, 99, 463, 491, 33, 372, 174, 49, 646, 100, 173, 78, 79, 94, 96, 101, 50, 103, 282, 106, 114, 125, 171, 172, 207, 215, 220, 219, 244, 228, 237, 247, 128, 269, 354, 355, 377, 287, 378, 390, 302, 405, 406, 411, 425, 209, 441, 468, 452, 454, 455, 457, 462, 470, 475, 494, 499, 505, 511, 510, 520, 543, 565, 573, 597, 598, 617, 621, 464, 642);

			public static bool[] HousingWalls = Factory.CreateBoolSet(11, 389, 386);

			public static bool[] BreakableWhenPlacing = Factory.CreateBoolSet(324, 186, 187, 185, 165, 530, 233, 227, 485, 81, 624);

			public static bool[] TouchDamageDestroyTile = Factory.CreateBoolSet(32, 69, 352, 655);

			public static bool[] Suffocate = Factory.CreateBoolSet(53, 112, 116, 123, 224, 234);

			public static bool[] TouchDamageHot = Factory.CreateBoolSet(37, 58, 76, 684, 230);

			public static bool[] TouchDamageBleeding = Factory.CreateBoolSet(48, 232);

			public static int[] TouchDamageImmediate = Factory.CreateIntSet(0, 32, 10, 69, 17, 80, 6, 352, 10, 655, 100, 48, 60, 232, 80, 484, 25);

			public static bool[] Falling = Factory.CreateBoolSet(53, 234, 112, 116, 224, 123, 330, 331, 332, 333, 495);

			public static bool[] BlockMergesWithMergeAllBlock = Factory.CreateBoolSet();

			public static bool[] OreMergesWithMud = Factory.CreateBoolSet(7, 166, 6, 167, 9, 168, 8, 169, 22, 204, 37, 58, 107, 221, 108, 222, 111, 223);

			public static bool[] Ore = Factory.CreateBoolSet(7, 166, 6, 167, 9, 168, 8, 169, 22, 204, 37, 58, 107, 221, 108, 222, 111, 223, 211);

			public static bool[] IsAContainer = Factory.CreateBoolSet(21, 467, 88);

			public static bool[] IsAMechanism = Factory.CreateBoolSet(137, 443, 105, 349, 141, 142, 143, 42, 34, 130, 131, 506, 546, 557, 593, 594);

			public static bool[] IsATrigger = Factory.CreateBoolSet(135, 136, 132, 144, 411, 441, 468);

			public static bool[] FriendlyFairyCanLureTo = Factory.CreateBoolSet(8, 169, 21, 467, 107, 108, 111, 221, 222, 223, 211, 12, 665, 639, 236, 227);

			public static bool[] IgnoredInHouseScore = Factory.CreateBoolSet(4, 3, 73, 82, 83, 84, 386);

			public static bool[] SpreadOverground = Factory.CreateBoolSet(2, 23, 661, 32, 60, 70, 109, 199, 662, 352, 477, 492, 633);

			public static bool[] SpreadUnderground = Factory.CreateBoolSet(23, 661, 109, 199, 662, 60, 70, 633);
		}

		public static readonly IdDictionary Search = IdDictionary.Create<TileID, ushort>();

		public const ushort Dirt = 0;

		public const ushort Stone = 1;

		public const ushort Grass = 2;

		public const ushort Plants = 3;

		public const ushort Torches = 4;

		public const ushort Trees = 5;

		public const ushort Iron = 6;

		public const ushort Copper = 7;

		public const ushort Gold = 8;

		public const ushort Silver = 9;

		public const ushort ClosedDoor = 10;

		public const ushort OpenDoor = 11;

		public const ushort Heart = 12;

		public const ushort Bottles = 13;

		public const ushort Tables = 14;

		public const ushort Chairs = 15;

		public const ushort Anvils = 16;

		public const ushort Furnaces = 17;

		public const ushort WorkBenches = 18;

		public const ushort Platforms = 19;

		public const ushort Saplings = 20;

		public const ushort Containers = 21;

		public const ushort Demonite = 22;

		public const ushort CorruptGrass = 23;

		public const ushort CorruptPlants = 24;

		public const ushort Ebonstone = 25;

		public const ushort DemonAltar = 26;

		public const ushort Sunflower = 27;

		public const ushort Pots = 28;

		public const ushort PiggyBank = 29;

		public const ushort WoodBlock = 30;

		public const ushort ShadowOrbs = 31;

		public const ushort CorruptThorns = 32;

		public const ushort Candles = 33;

		public const ushort Chandeliers = 34;

		public const ushort Jackolanterns = 35;

		public const ushort Presents = 36;

		public const ushort Meteorite = 37;

		public const ushort GrayBrick = 38;

		public const ushort RedBrick = 39;

		public const ushort ClayBlock = 40;

		public const ushort BlueDungeonBrick = 41;

		public const ushort HangingLanterns = 42;

		public const ushort GreenDungeonBrick = 43;

		public const ushort PinkDungeonBrick = 44;

		public const ushort GoldBrick = 45;

		public const ushort SilverBrick = 46;

		public const ushort CopperBrick = 47;

		public const ushort Spikes = 48;

		public const ushort WaterCandle = 49;

		public const ushort Books = 50;

		public const ushort Cobweb = 51;

		public const ushort Vines = 52;

		public const ushort Sand = 53;

		public const ushort Glass = 54;

		public const ushort Signs = 55;

		public const ushort Obsidian = 56;

		public const ushort Ash = 57;

		public const ushort Hellstone = 58;

		public const ushort Mud = 59;

		public const ushort JungleGrass = 60;

		public const ushort JunglePlants = 61;

		public const ushort JungleVines = 62;

		public const ushort Sapphire = 63;

		public const ushort Ruby = 64;

		public const ushort Emerald = 65;

		public const ushort Topaz = 66;

		public const ushort Amethyst = 67;

		public const ushort Diamond = 68;

		public const ushort JungleThorns = 69;

		public const ushort MushroomGrass = 70;

		public const ushort MushroomPlants = 71;

		public const ushort MushroomTrees = 72;

		public const ushort Plants2 = 73;

		public const ushort JunglePlants2 = 74;

		public const ushort ObsidianBrick = 75;

		public const ushort HellstoneBrick = 76;

		public const ushort Hellforge = 77;

		public const ushort ClayPot = 78;

		public const ushort Beds = 79;

		public const ushort Cactus = 80;

		public const ushort Coral = 81;

		public const ushort ImmatureHerbs = 82;

		public const ushort MatureHerbs = 83;

		public const ushort BloomingHerbs = 84;

		public const ushort Tombstones = 85;

		public const ushort Loom = 86;

		public const ushort Pianos = 87;

		public const ushort Dressers = 88;

		public const ushort Benches = 89;

		public const ushort Bathtubs = 90;

		public const ushort Banners = 91;

		public const ushort Lampposts = 92;

		public const ushort Lamps = 93;

		public const ushort Kegs = 94;

		public const ushort ChineseLanterns = 95;

		public const ushort CookingPots = 96;

		public const ushort Safes = 97;

		public const ushort SkullLanterns = 98;

		public const ushort TrashCan = 99;

		public const ushort Candelabras = 100;

		public const ushort Bookcases = 101;

		public const ushort Thrones = 102;

		public const ushort Bowls = 103;

		public const ushort GrandfatherClocks = 104;

		public const ushort Statues = 105;

		public const ushort Sawmill = 106;

		public const ushort Cobalt = 107;

		public const ushort Mythril = 108;

		public const ushort HallowedGrass = 109;

		public const ushort HallowedPlants = 110;

		public const ushort Adamantite = 111;

		public const ushort Ebonsand = 112;

		public const ushort HallowedPlants2 = 113;

		public const ushort TinkerersWorkbench = 114;

		public const ushort HallowedVines = 115;

		public const ushort Pearlsand = 116;

		public const ushort Pearlstone = 117;

		public const ushort PearlstoneBrick = 118;

		public const ushort IridescentBrick = 119;

		public const ushort Mudstone = 120;

		public const ushort CobaltBrick = 121;

		public const ushort MythrilBrick = 122;

		public const ushort Silt = 123;

		public const ushort WoodenBeam = 124;

		public const ushort CrystalBall = 125;

		public const ushort DiscoBall = 126;

		public const ushort MagicalIceBlock = 127;

		public const ushort Mannequin = 128;

		public const ushort Crystals = 129;

		public const ushort ActiveStoneBlock = 130;

		public const ushort InactiveStoneBlock = 131;

		public const ushort Lever = 132;

		public const ushort AdamantiteForge = 133;

		public const ushort MythrilAnvil = 134;

		public const ushort PressurePlates = 135;

		public const ushort Switches = 136;

		public const ushort Traps = 137;

		public const ushort Boulder = 138;

		public const ushort MusicBoxes = 139;

		public const ushort DemoniteBrick = 140;

		public const ushort Explosives = 141;

		public const ushort InletPump = 142;

		public const ushort OutletPump = 143;

		public const ushort Timers = 144;

		public const ushort CandyCaneBlock = 145;

		public const ushort GreenCandyCaneBlock = 146;

		public const ushort SnowBlock = 147;

		public const ushort SnowBrick = 148;

		public const ushort HolidayLights = 149;

		public const ushort AdamantiteBeam = 150;

		public const ushort SandstoneBrick = 151;

		public const ushort EbonstoneBrick = 152;

		public const ushort RedStucco = 153;

		public const ushort YellowStucco = 154;

		public const ushort GreenStucco = 155;

		public const ushort GrayStucco = 156;

		public const ushort Ebonwood = 157;

		public const ushort RichMahogany = 158;

		public const ushort Pearlwood = 159;

		public const ushort RainbowBrick = 160;

		public const ushort IceBlock = 161;

		public const ushort BreakableIce = 162;

		public const ushort CorruptIce = 163;

		public const ushort HallowedIce = 164;

		public const ushort Stalactite = 165;

		public const ushort Tin = 166;

		public const ushort Lead = 167;

		public const ushort Tungsten = 168;

		public const ushort Platinum = 169;

		public const ushort PineTree = 170;

		public const ushort ChristmasTree = 171;

		public const ushort Sinks = 172;

		public const ushort PlatinumCandelabra = 173;

		public const ushort PlatinumCandle = 174;

		public const ushort TinBrick = 175;

		public const ushort TungstenBrick = 176;

		public const ushort PlatinumBrick = 177;

		public const ushort ExposedGems = 178;

		public const ushort GreenMoss = 179;

		public const ushort BrownMoss = 180;

		public const ushort RedMoss = 181;

		public const ushort BlueMoss = 182;

		public const ushort PurpleMoss = 183;

		public const ushort LongMoss = 184;

		public const ushort SmallPiles = 185;

		public const ushort LargePiles = 186;

		public const ushort LargePiles2 = 187;

		public const ushort CactusBlock = 188;

		public const ushort Cloud = 189;

		public const ushort MushroomBlock = 190;

		public const ushort LivingWood = 191;

		public const ushort LeafBlock = 192;

		public const ushort SlimeBlock = 193;

		public const ushort BoneBlock = 194;

		public const ushort FleshBlock = 195;

		public const ushort RainCloud = 196;

		public const ushort FrozenSlimeBlock = 197;

		public const ushort Asphalt = 198;

		public const ushort CrimsonGrass = 199;

		public const ushort FleshIce = 200;

		public const ushort CrimsonPlants = 201;

		public const ushort Sunplate = 202;

		public const ushort Crimstone = 203;

		public const ushort Crimtane = 204;

		public const ushort CrimsonVines = 205;

		public const ushort IceBrick = 206;

		public const ushort WaterFountain = 207;

		public const ushort Shadewood = 208;

		public const ushort Cannon = 209;

		public const ushort LandMine = 210;

		public const ushort Chlorophyte = 211;

		public const ushort SnowballLauncher = 212;

		public const ushort Rope = 213;

		public const ushort Chain = 214;

		public const ushort Campfire = 215;

		public const ushort Firework = 216;

		public const ushort Blendomatic = 217;

		public const ushort MeatGrinder = 218;

		public const ushort Extractinator = 219;

		public const ushort Solidifier = 220;

		public const ushort Palladium = 221;

		public const ushort Orichalcum = 222;

		public const ushort Titanium = 223;

		public const ushort Slush = 224;

		public const ushort Hive = 225;

		public const ushort LihzahrdBrick = 226;

		public const ushort DyePlants = 227;

		public const ushort DyeVat = 228;

		public const ushort HoneyBlock = 229;

		public const ushort CrispyHoneyBlock = 230;

		public const ushort Larva = 231;

		public const ushort WoodenSpikes = 232;

		public const ushort PlantDetritus = 233;

		public const ushort Crimsand = 234;

		public const ushort Teleporter = 235;

		public const ushort LifeFruit = 236;

		public const ushort LihzahrdAltar = 237;

		public const ushort PlanteraBulb = 238;

		public const ushort MetalBars = 239;

		public const ushort Painting3X3 = 240;

		public const ushort Painting4X3 = 241;

		public const ushort Painting6X4 = 242;

		public const ushort ImbuingStation = 243;

		public const ushort BubbleMachine = 244;

		public const ushort Painting2X3 = 245;

		public const ushort Painting3X2 = 246;

		public const ushort Autohammer = 247;

		public const ushort PalladiumColumn = 248;

		public const ushort BubblegumBlock = 249;

		public const ushort Titanstone = 250;

		public const ushort PumpkinBlock = 251;

		public const ushort HayBlock = 252;

		public const ushort SpookyWood = 253;

		public const ushort Pumpkins = 254;

		public const ushort AmethystGemsparkOff = 255;

		public const ushort TopazGemsparkOff = 256;

		public const ushort SapphireGemsparkOff = 257;

		public const ushort EmeraldGemsparkOff = 258;

		public const ushort RubyGemsparkOff = 259;

		public const ushort DiamondGemsparkOff = 260;

		public const ushort AmberGemsparkOff = 261;

		public const ushort AmethystGemspark = 262;

		public const ushort TopazGemspark = 263;

		public const ushort SapphireGemspark = 264;

		public const ushort EmeraldGemspark = 265;

		public const ushort RubyGemspark = 266;

		public const ushort DiamondGemspark = 267;

		public const ushort AmberGemspark = 268;

		public const ushort Womannequin = 269;

		public const ushort FireflyinaBottle = 270;

		public const ushort LightningBuginaBottle = 271;

		public const ushort Cog = 272;

		public const ushort StoneSlab = 273;

		public const ushort SandStoneSlab = 274;

		public const ushort BunnyCage = 275;

		public const ushort SquirrelCage = 276;

		public const ushort MallardDuckCage = 277;

		public const ushort DuckCage = 278;

		public const ushort BirdCage = 279;

		public const ushort BlueJay = 280;

		public const ushort CardinalCage = 281;

		public const ushort FishBowl = 282;

		public const ushort HeavyWorkBench = 283;

		public const ushort CopperPlating = 284;

		public const ushort SnailCage = 285;

		public const ushort GlowingSnailCage = 286;

		public const ushort AmmoBox = 287;

		public const ushort MonarchButterflyJar = 288;

		public const ushort PurpleEmperorButterflyJar = 289;

		public const ushort RedAdmiralButterflyJar = 290;

		public const ushort UlyssesButterflyJar = 291;

		public const ushort SulphurButterflyJar = 292;

		public const ushort TreeNymphButterflyJar = 293;

		public const ushort ZebraSwallowtailButterflyJar = 294;

		public const ushort JuliaButterflyJar = 295;

		public const ushort ScorpionCage = 296;

		public const ushort BlackScorpionCage = 297;

		public const ushort FrogCage = 298;

		public const ushort MouseCage = 299;

		public const ushort BoneWelder = 300;

		public const ushort FleshCloningVat = 301;

		public const ushort GlassKiln = 302;

		public const ushort LihzahrdFurnace = 303;

		public const ushort LivingLoom = 304;

		public const ushort SkyMill = 305;

		public const ushort IceMachine = 306;

		public const ushort SteampunkBoiler = 307;

		public const ushort HoneyDispenser = 308;

		public const ushort PenguinCage = 309;

		public const ushort WormCage = 310;

		public const ushort DynastyWood = 311;

		public const ushort RedDynastyShingles = 312;

		public const ushort BlueDynastyShingles = 313;

		public const ushort MinecartTrack = 314;

		public const ushort Coralstone = 315;

		public const ushort BlueJellyfishBowl = 316;

		public const ushort GreenJellyfishBowl = 317;

		public const ushort PinkJellyfishBowl = 318;

		public const ushort ShipInABottle = 319;

		public const ushort SeaweedPlanter = 320;

		public const ushort BorealWood = 321;

		public const ushort PalmWood = 322;

		public const ushort PalmTree = 323;

		public const ushort BeachPiles = 324;

		public const ushort TinPlating = 325;

		public const ushort Waterfall = 326;

		public const ushort Lavafall = 327;

		public const ushort Confetti = 328;

		public const ushort ConfettiBlack = 329;

		public const ushort CopperCoinPile = 330;

		public const ushort SilverCoinPile = 331;

		public const ushort GoldCoinPile = 332;

		public const ushort PlatinumCoinPile = 333;

		public const ushort WeaponsRack = 334;

		public const ushort FireworksBox = 335;

		public const ushort LivingFire = 336;

		public const ushort AlphabetStatues = 337;

		public const ushort FireworkFountain = 338;

		public const ushort GrasshopperCage = 339;

		public const ushort LivingCursedFire = 340;

		public const ushort LivingDemonFire = 341;

		public const ushort LivingFrostFire = 342;

		public const ushort LivingIchor = 343;

		public const ushort LivingUltrabrightFire = 344;

		public const ushort Honeyfall = 345;

		public const ushort ChlorophyteBrick = 346;

		public const ushort CrimtaneBrick = 347;

		public const ushort ShroomitePlating = 348;

		public const ushort MushroomStatue = 349;

		public const ushort MartianConduitPlating = 350;

		public const ushort ChimneySmoke = 351;

		public const ushort CrimsonThorns = 352;

		public const ushort VineRope = 353;

		public const ushort BewitchingTable = 354;

		public const ushort AlchemyTable = 355;

		public const ushort Sundial = 356;

		public const ushort MarbleBlock = 357;

		public const ushort GoldBirdCage = 358;

		public const ushort GoldBunnyCage = 359;

		public const ushort GoldButterflyCage = 360;

		public const ushort GoldFrogCage = 361;

		public const ushort GoldGrasshopperCage = 362;

		public const ushort GoldMouseCage = 363;

		public const ushort GoldWormCage = 364;

		public const ushort SilkRope = 365;

		public const ushort WebRope = 366;

		public const ushort Marble = 367;

		public const ushort Granite = 368;

		public const ushort GraniteBlock = 369;

		public const ushort MeteoriteBrick = 370;

		public const ushort PinkSlimeBlock = 371;

		public const ushort PeaceCandle = 372;

		public const ushort WaterDrip = 373;

		public const ushort LavaDrip = 374;

		public const ushort HoneyDrip = 375;

		public const ushort FishingCrate = 376;

		public const ushort SharpeningStation = 377;

		public const ushort TargetDummy = 378;

		public const ushort Bubble = 379;

		public const ushort PlanterBox = 380;

		public const ushort LavaMoss = 381;

		public const ushort VineFlowers = 382;

		public const ushort LivingMahogany = 383;

		public const ushort LivingMahoganyLeaves = 384;

		public const ushort CrystalBlock = 385;

		public const ushort TrapdoorOpen = 386;

		public const ushort TrapdoorClosed = 387;

		public const ushort TallGateClosed = 388;

		public const ushort TallGateOpen = 389;

		public const ushort LavaLamp = 390;

		public const ushort CageEnchantedNightcrawler = 391;

		public const ushort CageBuggy = 392;

		public const ushort CageGrubby = 393;

		public const ushort CageSluggy = 394;

		public const ushort ItemFrame = 395;

		public const ushort Sandstone = 396;

		public const ushort HardenedSand = 397;

		public const ushort CorruptHardenedSand = 398;

		public const ushort CrimsonHardenedSand = 399;

		public const ushort CorruptSandstone = 400;

		public const ushort CrimsonSandstone = 401;

		public const ushort HallowHardenedSand = 402;

		public const ushort HallowSandstone = 403;

		public const ushort DesertFossil = 404;

		public const ushort Fireplace = 405;

		public const ushort Chimney = 406;

		public const ushort FossilOre = 407;

		public const ushort LunarOre = 408;

		public const ushort LunarBrick = 409;

		public const ushort LunarMonolith = 410;

		public const ushort Detonator = 411;

		public const ushort LunarCraftingStation = 412;

		public const ushort SquirrelOrangeCage = 413;

		public const ushort SquirrelGoldCage = 414;

		public const ushort LunarBlockSolar = 415;

		public const ushort LunarBlockVortex = 416;

		public const ushort LunarBlockNebula = 417;

		public const ushort LunarBlockStardust = 418;

		public const ushort LogicGateLamp = 419;

		public const ushort LogicGate = 420;

		public const ushort ConveyorBeltLeft = 421;

		public const ushort ConveyorBeltRight = 422;

		public const ushort LogicSensor = 423;

		public const ushort WirePipe = 424;

		public const ushort AnnouncementBox = 425;

		public const ushort TeamBlockRed = 426;

		public const ushort TeamBlockRedPlatform = 427;

		public const ushort WeightedPressurePlate = 428;

		public const ushort WireBulb = 429;

		public const ushort TeamBlockGreen = 430;

		public const ushort TeamBlockBlue = 431;

		public const ushort TeamBlockYellow = 432;

		public const ushort TeamBlockPink = 433;

		public const ushort TeamBlockWhite = 434;

		public const ushort TeamBlockGreenPlatform = 435;

		public const ushort TeamBlockBluePlatform = 436;

		public const ushort TeamBlockYellowPlatform = 437;

		public const ushort TeamBlockPinkPlatform = 438;

		public const ushort TeamBlockWhitePlatform = 439;

		public const ushort GemLocks = 440;

		public const ushort FakeContainers = 441;

		public const ushort ProjectilePressurePad = 442;

		public const ushort GeyserTrap = 443;

		public const ushort BeeHive = 444;

		public const ushort PixelBox = 445;

		public const ushort SillyBalloonPink = 446;

		public const ushort SillyBalloonPurple = 447;

		public const ushort SillyBalloonGreen = 448;

		public const ushort SillyStreamerBlue = 449;

		public const ushort SillyStreamerGreen = 450;

		public const ushort SillyStreamerPink = 451;

		public const ushort SillyBalloonMachine = 452;

		public const ushort SillyBalloonTile = 453;

		public const ushort Pigronata = 454;

		public const ushort PartyMonolith = 455;

		public const ushort PartyBundleOfBalloonTile = 456;

		public const ushort PartyPresent = 457;

		public const ushort SandFallBlock = 458;

		public const ushort SnowFallBlock = 459;

		public const ushort SnowCloud = 460;

		public const ushort SandDrip = 461;

		public const ushort DjinnLamp = 462;

		public const ushort DefendersForge = 463;

		public const ushort WarTable = 464;

		public const ushort WarTableBanner = 465;

		public const ushort ElderCrystalStand = 466;

		public const ushort Containers2 = 467;

		public const ushort FakeContainers2 = 468;

		public const ushort Tables2 = 469;

		public const ushort DisplayDoll = 470;

		public const ushort WeaponsRack2 = 471;

		public const ushort IronBrick = 472;

		public const ushort LeadBrick = 473;

		public const ushort LesionBlock = 474;

		public const ushort HatRack = 475;

		public const ushort GolfHole = 476;

		public const ushort GolfGrass = 477;

		public const ushort CrimstoneBrick = 478;

		public const ushort SmoothSandstone = 479;

		public const ushort BloodMoonMonolith = 480;

		public const ushort CrackedBlueDungeonBrick = 481;

		public const ushort CrackedGreenDungeonBrick = 482;

		public const ushort CrackedPinkDungeonBrick = 483;

		public const ushort RollingCactus = 484;

		public const ushort AntlionLarva = 485;

		public const ushort DrumSet = 486;

		public const ushort PicnicTable = 487;

		public const ushort FallenLog = 488;

		public const ushort PinWheel = 489;

		public const ushort WeatherVane = 490;

		public const ushort VoidVault = 491;

		public const ushort GolfGrassHallowed = 492;

		public const ushort GolfCupFlag = 493;

		public const ushort GolfTee = 494;

		public const ushort ShellPile = 495;

		public const ushort AntiPortalBlock = 496;

		public const ushort Toilets = 497;

		public const ushort Spider = 498;

		public const ushort LesionStation = 499;

		public const ushort SolarBrick = 500;

		public const ushort VortexBrick = 501;

		public const ushort NebulaBrick = 502;

		public const ushort StardustBrick = 503;

		public const ushort MysticSnakeRope = 504;

		public const ushort GoldGoldfishBowl = 505;

		public const ushort CatBast = 506;

		public const ushort GoldStarryGlassBlock = 507;

		public const ushort BlueStarryGlassBlock = 508;

		public const ushort VoidMonolith = 509;

		public const ushort ArrowSign = 510;

		public const ushort PaintedArrowSign = 511;

		public const ushort GreenMossBrick = 512;

		public const ushort BrownMossBrick = 513;

		public const ushort RedMossBrick = 514;

		public const ushort BlueMossBrick = 515;

		public const ushort PurpleMossBrick = 516;

		public const ushort LavaMossBrick = 517;

		public const ushort LilyPad = 518;

		public const ushort Cattail = 519;

		public const ushort FoodPlatter = 520;

		public const ushort BlackDragonflyJar = 521;

		public const ushort BlueDragonflyJar = 522;

		public const ushort GreenDragonflyJar = 523;

		public const ushort OrangeDragonflyJar = 524;

		public const ushort RedDragonflyJar = 525;

		public const ushort YellowDragonflyJar = 526;

		public const ushort GoldDragonflyJar = 527;

		public const ushort MushroomVines = 528;

		public const ushort SeaOats = 529;

		public const ushort OasisPlants = 530;

		public const ushort BoulderStatue = 531;

		public const ushort MaggotCage = 532;

		public const ushort RatCage = 533;

		public const ushort KryptonMoss = 534;

		public const ushort KryptonMossBrick = 535;

		public const ushort XenonMoss = 536;

		public const ushort XenonMossBrick = 537;

		public const ushort LadybugCage = 538;

		public const ushort ArgonMoss = 539;

		public const ushort ArgonMossBrick = 540;

		public const ushort EchoBlock = 541;

		public const ushort OwlCage = 542;

		public const ushort PupfishBowl = 543;

		public const ushort GoldLadybugCage = 544;

		public const ushort LawnFlamingo = 545;

		public const ushort Grate = 546;

		public const ushort PottedPlants1 = 547;

		public const ushort PottedPlants2 = 548;

		public const ushort Seaweed = 549;

		public const ushort TurtleCage = 550;

		public const ushort TurtleJungleCage = 551;

		public const ushort Sandcastles = 552;

		public const ushort GrebeCage = 553;

		public const ushort SeagullCage = 554;

		public const ushort WaterStriderCage = 555;

		public const ushort GoldWaterStriderCage = 556;

		public const ushort GrateClosed = 557;

		public const ushort SeahorseCage = 558;

		public const ushort GoldSeahorseCage = 559;

		public const ushort GolfTrophies = 560;

		public const ushort MarbleColumn = 561;

		public const ushort BambooBlock = 562;

		public const ushort LargeBambooBlock = 563;

		public const ushort PlasmaLamp = 564;

		public const ushort FogMachine = 565;

		public const ushort AmberStoneBlock = 566;

		public const ushort GardenGnome = 567;

		public const ushort PinkFairyJar = 568;

		public const ushort GreenFairyJar = 569;

		public const ushort BlueFairyJar = 570;

		public const ushort Bamboo = 571;

		public const ushort SoulBottles = 572;

		public const ushort TatteredWoodSign = 573;

		public const ushort BorealBeam = 574;

		public const ushort RichMahoganyBeam = 575;

		public const ushort GraniteColumn = 576;

		public const ushort SandstoneColumn = 577;

		public const ushort MushroomBeam = 578;

		public const ushort RockGolemHead = 579;

		public const ushort HellButterflyJar = 580;

		public const ushort LavaflyinaBottle = 581;

		public const ushort MagmaSnailCage = 582;

		public const ushort TreeTopaz = 583;

		public const ushort TreeAmethyst = 584;

		public const ushort TreeSapphire = 585;

		public const ushort TreeEmerald = 586;

		public const ushort TreeRuby = 587;

		public const ushort TreeDiamond = 588;

		public const ushort TreeAmber = 589;

		public const ushort GemSaplings = 590;

		public const ushort PotsSuspended = 591;

		public const ushort BrazierSuspended = 592;

		public const ushort VolcanoSmall = 593;

		public const ushort VolcanoLarge = 594;

		public const ushort VanityTreeSakuraSaplings = 595;

		public const ushort VanityTreeSakura = 596;

		public const ushort TeleportationPylon = 597;

		public const ushort LavafishBowl = 598;

		public const ushort AmethystBunnyCage = 599;

		public const ushort TopazBunnyCage = 600;

		public const ushort SapphireBunnyCage = 601;

		public const ushort EmeraldBunnyCage = 602;

		public const ushort RubyBunnyCage = 603;

		public const ushort DiamondBunnyCage = 604;

		public const ushort AmberBunnyCage = 605;

		public const ushort AmethystSquirrelCage = 606;

		public const ushort TopazSquirrelCage = 607;

		public const ushort SapphireSquirrelCage = 608;

		public const ushort EmeraldSquirrelCage = 609;

		public const ushort RubySquirrelCage = 610;

		public const ushort DiamondSquirrelCage = 611;

		public const ushort AmberSquirrelCage = 612;

		public const ushort PottedLavaPlants = 613;

		public const ushort PottedLavaPlantTendrils = 614;

		public const ushort VanityTreeWillowSaplings = 615;

		public const ushort VanityTreeYellowWillow = 616;

		public const ushort MasterTrophyBase = 617;

		public const ushort AccentSlab = 618;

		public const ushort TruffleWormCage = 619;

		public const ushort EmpressButterflyJar = 620;

		public const ushort SliceOfCake = 621;

		public const ushort TeaKettle = 622;

		public const ushort PottedCrystalPlants = 623;

		public const ushort AbigailsFlower = 624;

		public const ushort VioletMoss = 625;

		public const ushort VioletMossBrick = 626;

		public const ushort RainbowMoss = 627;

		public const ushort RainbowMossBrick = 628;

		public const ushort StinkbugCage = 629;

		public const ushort StinkbugHousingBlocker = 630;

		public const ushort StinkbugHousingBlockerEcho = 631;

		public const ushort ScarletMacawCage = 632;

		public const ushort AshGrass = 633;

		public const ushort TreeAsh = 634;

		public const ushort AshWood = 635;

		public const ushort CorruptVines = 636;

		public const ushort AshPlants = 637;

		public const ushort AshVines = 638;

		public const ushort ManaCrystal = 639;

		public const ushort BlueMacawCage = 640;

		public const ushort ReefBlock = 641;

		public const ushort ChlorophyteExtractinator = 642;

		public const ushort ToucanCage = 643;

		public const ushort YellowCockatielCage = 644;

		public const ushort GrayCockatielCage = 645;

		public const ushort ShadowCandle = 646;

		public const ushort LargePilesEcho = 647;

		public const ushort LargePiles2Echo = 648;

		public const ushort SmallPiles2x1Echo = 649;

		public const ushort SmallPiles1x1Echo = 650;

		public const ushort PlantDetritus3x2Echo = 651;

		public const ushort PlantDetritus2x2Echo = 652;

		public const ushort PotsEcho = 653;

		public const ushort TNTBarrel = 654;

		public const ushort PlanteraThorns = 655;

		public const ushort GlowTulip = 656;

		public const ushort EchoMonolith = 657;

		public const ushort ShimmerMonolith = 658;

		public const ushort ShimmerBlock = 659;

		public const ushort ShimmerflyinaBottle = 660;

		public const ushort CorruptJungleGrass = 661;

		public const ushort CrimsonJungleGrass = 662;

		public const ushort Moondial = 663;

		public const ushort BouncyBoulder = 664;

		public const ushort LifeCrystalBoulder = 665;

		public const ushort PoopBlock = 666;

		public const ushort ShimmerBrick = 667;

		public const ushort DirtiestBlock = 668;

		public const ushort LunarRustBrick = 669;

		public const ushort DarkCelestialBrick = 670;

		public const ushort AstraBrick = 671;

		public const ushort CosmicEmberBrick = 672;

		public const ushort CryocoreBrick = 673;

		public const ushort MercuryBrick = 674;

		public const ushort StarRoyaleBrick = 675;

		public const ushort HeavenforgeBrick = 676;

		public const ushort AncientBlueBrick = 677;

		public const ushort AncientGreenBrick = 678;

		public const ushort AncientPinkBrick = 679;

		public const ushort AncientGoldBrick = 680;

		public const ushort AncientSilverBrick = 681;

		public const ushort AncientCopperBrick = 682;

		public const ushort AncientObsidianBrick = 683;

		public const ushort AncientHellstoneBrick = 684;

		public const ushort AncientCobaltBrick = 685;

		public const ushort AncientMythrilBrick = 686;

		public const ushort LavaMossBlock = 687;

		public const ushort ArgonMossBlock = 688;

		public const ushort KryptonMossBlock = 689;

		public const ushort XenonMossBlock = 690;

		public const ushort VioletMossBlock = 691;

		public const ushort RainbowMossBlock = 692;

		public const ushort Count = 693;
	}
}
