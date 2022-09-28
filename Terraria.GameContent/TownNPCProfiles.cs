using System.Collections.Generic;

namespace Terraria.GameContent
{
	public class TownNPCProfiles
	{
		private const string DefaultNPCFileFolderPath = "Images/TownNPCs/";

		private const string ShimmeredNPCFileFolderPath = "Images/TownNPCs/Shimmered/";

		private static readonly int[] CatHeadIDs = new int[6] { 27, 28, 29, 30, 31, 32 };

		private static readonly int[] DogHeadIDs = new int[6] { 33, 34, 35, 36, 37, 38 };

		private static readonly int[] BunnyHeadIDs = new int[6] { 39, 40, 41, 42, 43, 44 };

		private static readonly int[] SlimeHeadIDs = new int[8] { 46, 47, 48, 49, 50, 51, 52, 53 };

		private Dictionary<int, ITownNPCProfile> _townNPCProfiles = new Dictionary<int, ITownNPCProfile>
		{
			{
				22,
				LegacyWithSimpleShimmer("Guide", 1, 72, uniquePartyTexture: false, uniquePartyTextureShimmered: false)
			},
			{
				20,
				LegacyWithSimpleShimmer("Dryad", 5, 73, uniquePartyTexture: false, uniquePartyTextureShimmered: false)
			},
			{
				19,
				LegacyWithSimpleShimmer("ArmsDealer", 6, 74, uniquePartyTexture: false, uniquePartyTextureShimmered: false)
			},
			{
				107,
				LegacyWithSimpleShimmer("GoblinTinkerer", 9, 75, uniquePartyTexture: false, uniquePartyTextureShimmered: false)
			},
			{
				160,
				LegacyWithSimpleShimmer("Truffle", 12, 76, uniquePartyTexture: false, uniquePartyTextureShimmered: false)
			},
			{
				208,
				LegacyWithSimpleShimmer("PartyGirl", 15, 77, uniquePartyTexture: false, uniquePartyTextureShimmered: false)
			},
			{
				228,
				LegacyWithSimpleShimmer("WitchDoctor", 18, 78, uniquePartyTexture: false, uniquePartyTextureShimmered: false)
			},
			{
				550,
				LegacyWithSimpleShimmer("Tavernkeep", 24, 79, uniquePartyTexture: false, uniquePartyTextureShimmered: false)
			},
			{
				369,
				LegacyWithSimpleShimmer("Angler", 22, 55, uniquePartyTexture: true, uniquePartyTextureShimmered: false)
			},
			{
				54,
				LegacyWithSimpleShimmer("Clothier", 7, 57, uniquePartyTexture: true, uniquePartyTextureShimmered: false)
			},
			{
				209,
				LegacyWithSimpleShimmer("Cyborg", 16, 58)
			},
			{
				38,
				LegacyWithSimpleShimmer("Demolitionist", 4, 59)
			},
			{
				207,
				LegacyWithSimpleShimmer("DyeTrader", 14, 60)
			},
			{
				588,
				LegacyWithSimpleShimmer("Golfer", 25, 61, uniquePartyTexture: true, uniquePartyTextureShimmered: false)
			},
			{
				124,
				LegacyWithSimpleShimmer("Mechanic", 8, 62)
			},
			{
				17,
				LegacyWithSimpleShimmer("Merchant", 2, 63)
			},
			{
				18,
				LegacyWithSimpleShimmer("Nurse", 3, 64)
			},
			{
				227,
				LegacyWithSimpleShimmer("Painter", 17, 65, uniquePartyTexture: true, uniquePartyTextureShimmered: false)
			},
			{
				229,
				LegacyWithSimpleShimmer("Pirate", 19, 66)
			},
			{
				142,
				LegacyWithSimpleShimmer("Santa", 11, 67)
			},
			{
				178,
				LegacyWithSimpleShimmer("Steampunker", 13, 68, uniquePartyTexture: true, uniquePartyTextureShimmered: false)
			},
			{
				353,
				LegacyWithSimpleShimmer("Stylist", 20, 69)
			},
			{
				441,
				LegacyWithSimpleShimmer("TaxCollector", 23, 70)
			},
			{
				108,
				LegacyWithSimpleShimmer("Wizard", 10, 71)
			},
			{
				663,
				LegacyWithSimpleShimmer("Princess", 45, 54)
			},
			{
				633,
				TransformableWithSimpleShimmer("BestiaryGirl", 26, 56, uniqueCreditTexture: true, uniqueCreditTextureShimmered: false)
			},
			{
				37,
				LegacyWithSimpleShimmer("OldMan", -1, -1, uniquePartyTexture: false, uniquePartyTextureShimmered: false)
			},
			{
				453,
				LegacyWithSimpleShimmer("SkeletonMerchant", -1, -1)
			},
			{
				368,
				LegacyWithSimpleShimmer("TravelingMerchant", 21, 80)
			},
			{
				637,
				new Profiles.VariantNPCProfile("Images/TownNPCs/Cat", "Cat", CatHeadIDs, "Siamese", "Black", "OrangeTabby", "RussianBlue", "Silver", "White")
			},
			{
				638,
				new Profiles.VariantNPCProfile("Images/TownNPCs/Dog", "Dog", DogHeadIDs, "Labrador", "PitBull", "Beagle", "Corgi", "Dalmation", "Husky")
			},
			{
				656,
				new Profiles.VariantNPCProfile("Images/TownNPCs/Bunny", "Bunny", BunnyHeadIDs, "White", "Angora", "Dutch", "Flemish", "Lop", "Silver")
			},
			{
				670,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/SlimeBlue", 46, includeDefault: true, uniquePartyTexture: false)
			},
			{
				678,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/SlimeGreen", 47)
			},
			{
				679,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/SlimeOld", 48)
			},
			{
				680,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/SlimePurple", 49)
			},
			{
				681,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/SlimeRainbow", 50)
			},
			{
				682,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/SlimeRed", 51)
			},
			{
				683,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/SlimeYellow", 52)
			},
			{
				684,
				new Profiles.LegacyNPCProfile("Images/TownNPCs/SlimeCopper", 53)
			}
		};

		public static TownNPCProfiles Instance = new TownNPCProfiles();

		public bool GetProfile(int npcId, out ITownNPCProfile profile)
		{
			return _townNPCProfiles.TryGetValue(npcId, out profile);
		}

		public static ITownNPCProfile LegacyWithSimpleShimmer(string subPath, int headIdNormal, int headIdShimmered, bool uniquePartyTexture = true, bool uniquePartyTextureShimmered = true)
		{
			return new Profiles.StackedNPCProfile(new Profiles.LegacyNPCProfile("Images/TownNPCs/" + subPath, headIdNormal, includeDefault: true, uniquePartyTexture), new Profiles.LegacyNPCProfile("Images/TownNPCs/Shimmered/" + subPath, headIdShimmered, includeDefault: true, uniquePartyTextureShimmered));
		}

		public static ITownNPCProfile TransformableWithSimpleShimmer(string subPath, int headIdNormal, int headIdShimmered, bool uniqueCreditTexture = true, bool uniqueCreditTextureShimmered = true)
		{
			return new Profiles.StackedNPCProfile(new Profiles.TransformableNPCProfile("Images/TownNPCs/" + subPath, headIdNormal, uniqueCreditTexture), new Profiles.TransformableNPCProfile("Images/TownNPCs/Shimmered/" + subPath, headIdShimmered, uniqueCreditTextureShimmered));
		}

		public static int GetHeadIndexSafe(NPC npc)
		{
			if (Instance.GetProfile(npc.type, out var profile))
			{
				return profile.GetHeadTextureIndex(npc);
			}
			return NPC.TypeToDefaultHeadIndex(npc.type);
		}
	}
}
