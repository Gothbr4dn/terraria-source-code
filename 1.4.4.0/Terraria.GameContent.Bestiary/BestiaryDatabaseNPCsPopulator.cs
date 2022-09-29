using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent.Bestiary
{
	public class BestiaryDatabaseNPCsPopulator
	{
		public static class CommonTags
		{
			public static class SpawnConditions
			{
				public static class Invasions
				{
					public static SpawnConditionBestiaryInfoElement Goblins = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.Goblins", 49, "Images/MapBG1");

					public static SpawnConditionBestiaryInfoElement Pirates = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.Pirates", 50, "Images/MapBG11");

					public static SpawnConditionBestiaryInfoElement Martian = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.Martian", 53, "Images/MapBG1", new Color(35, 40, 40));

					public static SpawnConditionBestiaryInfoElement OldOnesArmy = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.OldOnesArmy", 55, "Images/MapBG1");

					public static SpawnConditionBestiaryInfoElement PumpkinMoon = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.PumpkinMoon", 51, "Images/MapBG1", new Color(35, 40, 40));

					public static SpawnConditionBestiaryInfoElement FrostMoon = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.FrostMoon", 52, "Images/MapBG12", new Color(35, 40, 40));

					public static SpawnConditionBestiaryInfoElement FrostLegion = new SpawnConditionBestiaryInfoElement("Bestiary_Invasions.FrostLegion", 54, "Images/MapBG12");
				}

				public static class Events
				{
					public static SpawnConditionBestiaryInfoElement SlimeRain = new SpawnConditionBestiaryInfoElement("Bestiary_Events.SlimeRain", 47, "Images/MapBG1")
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					public static SpawnConditionBestiaryInfoElement WindyDay = new SpawnConditionBestiaryInfoElement("Bestiary_Events.WindyDay", 41, "Images/MapBG1")
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					public static SpawnConditionBestiaryInfoElement BloodMoon = new SpawnConditionBestiaryInfoElement("Bestiary_Events.BloodMoon", 38, "Images/MapBG26", new Color(200, 190, 180))
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					public static SpawnConditionBestiaryInfoElement Halloween = new SpawnConditionBestiaryInfoElement("Bestiary_Events.Halloween", 45, "Images/MapBG1")
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					public static SpawnConditionBestiaryOverlayInfoElement Rain = new SpawnConditionBestiaryOverlayInfoElement("Bestiary_Events.Rain", 40)
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					public static SpawnConditionBestiaryInfoElement Christmas = new SpawnConditionBestiaryInfoElement("Bestiary_Events.Christmas", 46, "Images/MapBG12")
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					public static SpawnConditionBestiaryInfoElement Eclipse = new SpawnConditionBestiaryInfoElement("Bestiary_Events.Eclipse", 39, "Images/MapBG1", new Color(60, 30, 0))
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					public static SpawnConditionBestiaryInfoElement Party = new SpawnConditionBestiaryInfoElement("Bestiary_Events.Party", 48, "Images/MapBG1")
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};

					public static SpawnConditionBestiaryOverlayInfoElement Blizzard = new SpawnConditionBestiaryOverlayInfoElement("Bestiary_Events.Blizzard", 42)
					{
						DisplayTextPriority = 1,
						HideInPortraitInfo = true,
						OrderPriority = -2f
					};

					public static SpawnConditionBestiaryOverlayInfoElement Sandstorm = new SpawnConditionBestiaryOverlayInfoElement("Bestiary_Events.Sandstorm", 43, "Images/MapBGOverlay1", Color.White)
					{
						DisplayTextPriority = 1,
						OrderPriority = -2f
					};
				}

				public static class Biomes
				{
					public static SpawnConditionBestiaryInfoElement TheCorruption = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.TheCorruption", 7, "Images/MapBG6", new Color(200, 200, 200));

					public static SpawnConditionBestiaryInfoElement TheCrimson = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Crimson", 12, "Images/MapBG7", new Color(200, 200, 200));

					public static SpawnConditionBestiaryInfoElement Surface = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Surface", 0, "Images/MapBG1");

					public static SpawnConditionBestiaryInfoElement Graveyard = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Graveyard", 35, "Images/MapBG27");

					public static SpawnConditionBestiaryInfoElement UndergroundJungle = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundJungle", 23, "Images/MapBG13");

					public static SpawnConditionBestiaryInfoElement TheUnderworld = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.TheUnderworld", 33, "Images/MapBG3");

					public static SpawnConditionBestiaryInfoElement TheDungeon = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.TheDungeon", 32, "Images/MapBG5");

					public static SpawnConditionBestiaryInfoElement Underground = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Underground", 1, "Images/MapBG2");

					public static SpawnConditionBestiaryInfoElement TheHallow = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.TheHallow", 17, "Images/MapBG8");

					public static SpawnConditionBestiaryInfoElement UndergroundMushroom = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundMushroom", 25, "Images/MapBG21");

					public static SpawnConditionBestiaryInfoElement Jungle = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Jungle", 22, "Images/MapBG9");

					public static SpawnConditionBestiaryInfoElement Caverns = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Caverns", 2, "Images/MapBG32");

					public static SpawnConditionBestiaryInfoElement UndergroundSnow = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundSnow", 6, "Images/MapBG4");

					public static SpawnConditionBestiaryInfoElement Ocean = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Ocean", 28, "Images/MapBG11");

					public static SpawnConditionBestiaryInfoElement SurfaceMushroom = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.SurfaceMushroom", 24, "Images/MapBG20");

					public static SpawnConditionBestiaryInfoElement UndergroundDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundDesert", 4, "Images/MapBG15");

					public static SpawnConditionBestiaryInfoElement Snow = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Snow", 5, "Images/MapBG12");

					public static SpawnConditionBestiaryInfoElement Desert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Desert", 3, "Images/MapBG10");

					public static SpawnConditionBestiaryInfoElement Meteor = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Meteor", 44, "Images/MapBG1", new Color(35, 40, 40));

					public static SpawnConditionBestiaryInfoElement Oasis = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Oasis", 27, "Images/MapBG10");

					public static SpawnConditionBestiaryInfoElement SpiderNest = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.SpiderNest", 34, "Images/MapBG19");

					public static SpawnConditionBestiaryInfoElement TheTemple = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.TheTemple", 31, "Images/MapBG14");

					public static SpawnConditionBestiaryInfoElement CorruptUndergroundDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CorruptUndergroundDesert", 10, "Images/MapBG40");

					public static SpawnConditionBestiaryInfoElement CrimsonUndergroundDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CrimsonUndergroundDesert", 15, "Images/MapBG41");

					public static SpawnConditionBestiaryInfoElement HallowUndergroundDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.HallowUndergroundDesert", 20, "Images/MapBG42");

					public static SpawnConditionBestiaryInfoElement CorruptDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CorruptDesert", 9, "Images/MapBG37");

					public static SpawnConditionBestiaryInfoElement CrimsonDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CrimsonDesert", 14, "Images/MapBG38");

					public static SpawnConditionBestiaryInfoElement HallowDesert = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.HallowDesert", 19, "Images/MapBG39");

					public static SpawnConditionBestiaryInfoElement Granite = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Granite", 30, "Images/MapBG17", new Color(100, 100, 100));

					public static SpawnConditionBestiaryInfoElement UndergroundCorruption = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundCorruption", 8, "Images/MapBG23");

					public static SpawnConditionBestiaryInfoElement UndergroundCrimson = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundCrimson", 13, "Images/MapBG24");

					public static SpawnConditionBestiaryInfoElement UndergroundHallow = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.UndergroundHallow", 18, "Images/MapBG22");

					public static SpawnConditionBestiaryInfoElement Marble = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Marble", 29, "Images/MapBG18");

					public static SpawnConditionBestiaryInfoElement CorruptIce = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CorruptIce", 11, "Images/MapBG34", new Color(200, 200, 200));

					public static SpawnConditionBestiaryInfoElement HallowIce = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.HallowIce", 21, "Images/MapBG36", new Color(200, 200, 200));

					public static SpawnConditionBestiaryInfoElement CrimsonIce = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.CrimsonIce", 16, "Images/MapBG35", new Color(200, 200, 200));

					public static SpawnConditionBestiaryInfoElement Sky = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.Sky", 26, "Images/MapBG33");

					public static SpawnConditionBestiaryInfoElement NebulaPillar = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.NebulaPillar", 58, "Images/MapBG28");

					public static SpawnConditionBestiaryInfoElement SolarPillar = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.SolarPillar", 56, "Images/MapBG29");

					public static SpawnConditionBestiaryInfoElement VortexPillar = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.VortexPillar", 57, "Images/MapBG30");

					public static SpawnConditionBestiaryInfoElement StardustPillar = new SpawnConditionBestiaryInfoElement("Bestiary_Biomes.StardustPillar", 59, "Images/MapBG31");
				}

				public static class Times
				{
					public static SpawnConditionBestiaryInfoElement DayTime = new SpawnConditionBestiaryInfoElement("Bestiary_Times.DayTime", 36)
					{
						DisplayTextPriority = -1,
						OrderPriority = -1f
					};

					public static SpawnConditionBestiaryInfoElement NightTime = new SpawnConditionBestiaryInfoElement("Bestiary_Times.NightTime", 37, "Images/MapBG1", new Color(35, 40, 40))
					{
						DisplayTextPriority = -1,
						OrderPriority = -1f
					};
				}

				public static class Visuals
				{
					public static SpawnConditionDecorativeOverlayInfoElement Sun = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay3", Color.White)
					{
						DisplayPriority = 1f
					};

					public static SpawnConditionDecorativeOverlayInfoElement Moon = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay4", Color.White)
					{
						DisplayPriority = 1f
					};

					public static SpawnConditionDecorativeOverlayInfoElement EclipseSun = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay5", Color.White)
					{
						DisplayPriority = 1f
					};

					public static SpawnConditionDecorativeOverlayInfoElement PumpkinMoon = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay8", Color.White)
					{
						DisplayPriority = 1f
					};

					public static SpawnConditionDecorativeOverlayInfoElement FrostMoon = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay9", Color.White)
					{
						DisplayPriority = 1f
					};

					public static SpawnConditionDecorativeOverlayInfoElement Meteor = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay7", Color.White)
					{
						DisplayPriority = 1f
					};

					public static SpawnConditionDecorativeOverlayInfoElement Rain = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay2", new Color(200, 200, 200))
					{
						DisplayPriority = 1f
					};

					public static SpawnConditionDecorativeOverlayInfoElement Blizzard = new SpawnConditionDecorativeOverlayInfoElement("Images/MapBGOverlay6", Color.White)
					{
						DisplayPriority = 1f
					};
				}
			}

			public static List<IBestiaryInfoElement> GetCommonInfoElementsForFilters()
			{
				return new List<IBestiaryInfoElement>
				{
					SpawnConditions.Biomes.Surface,
					SpawnConditions.Times.DayTime,
					SpawnConditions.Events.Party,
					SpawnConditions.Events.WindyDay,
					SpawnConditions.Events.Rain,
					SpawnConditions.Times.NightTime,
					SpawnConditions.Events.BloodMoon,
					SpawnConditions.Biomes.Graveyard,
					SpawnConditions.Biomes.Underground,
					SpawnConditions.Biomes.Caverns,
					SpawnConditions.Biomes.Granite,
					SpawnConditions.Biomes.Marble,
					SpawnConditions.Biomes.UndergroundMushroom,
					SpawnConditions.Biomes.SpiderNest,
					SpawnConditions.Biomes.Snow,
					SpawnConditions.Biomes.UndergroundSnow,
					SpawnConditions.Biomes.Desert,
					SpawnConditions.Biomes.UndergroundDesert,
					SpawnConditions.Events.Sandstorm,
					SpawnConditions.Biomes.Ocean,
					SpawnConditions.Biomes.Jungle,
					SpawnConditions.Biomes.UndergroundJungle,
					SpawnConditions.Biomes.Meteor,
					SpawnConditions.Biomes.TheDungeon,
					SpawnConditions.Biomes.TheUnderworld,
					SpawnConditions.Biomes.Sky,
					SpawnConditions.Biomes.TheCorruption,
					SpawnConditions.Biomes.UndergroundCorruption,
					SpawnConditions.Biomes.CorruptIce,
					SpawnConditions.Biomes.CorruptDesert,
					SpawnConditions.Biomes.CorruptUndergroundDesert,
					SpawnConditions.Biomes.TheCrimson,
					SpawnConditions.Biomes.UndergroundCrimson,
					SpawnConditions.Biomes.CrimsonIce,
					SpawnConditions.Biomes.CrimsonDesert,
					SpawnConditions.Biomes.CrimsonUndergroundDesert,
					SpawnConditions.Biomes.TheHallow,
					SpawnConditions.Biomes.UndergroundHallow,
					SpawnConditions.Biomes.HallowIce,
					SpawnConditions.Biomes.HallowDesert,
					SpawnConditions.Biomes.HallowUndergroundDesert,
					SpawnConditions.Biomes.SurfaceMushroom,
					SpawnConditions.Biomes.TheTemple,
					SpawnConditions.Invasions.Goblins,
					SpawnConditions.Invasions.OldOnesArmy,
					SpawnConditions.Invasions.Pirates,
					SpawnConditions.Invasions.Martian,
					SpawnConditions.Events.Eclipse,
					SpawnConditions.Invasions.PumpkinMoon,
					SpawnConditions.Invasions.FrostMoon,
					SpawnConditions.Events.Halloween,
					SpawnConditions.Events.Christmas,
					SpawnConditions.Invasions.FrostLegion,
					SpawnConditions.Biomes.NebulaPillar,
					SpawnConditions.Biomes.SolarPillar,
					SpawnConditions.Biomes.VortexPillar,
					SpawnConditions.Biomes.StardustPillar
				};
			}
		}

		public static class Conditions
		{
			public static bool ReachHardMode()
			{
				return Main.hardMode;
			}
		}

		public static class CrownosIconIndexes
		{
			public const int Surface = 0;

			public const int Underground = 1;

			public const int Cave = 2;

			public const int Desert = 3;

			public const int UndergroundDesert = 4;

			public const int Snow = 5;

			public const int UndergroundIce = 6;

			public const int Corruption = 7;

			public const int CorruptionUnderground = 8;

			public const int CorruptionDesert = 9;

			public const int CorruptionUndergroundDesert = 10;

			public const int CorruptionIce = 11;

			public const int Crimson = 12;

			public const int CrimsonUnderground = 13;

			public const int CrimsonDesert = 14;

			public const int CrimsonUndergroundDesert = 15;

			public const int CrimsonIce = 16;

			public const int Hallow = 17;

			public const int HallowUnderground = 18;

			public const int HallowDesert = 19;

			public const int HallowUndergroundDesert = 20;

			public const int HallowIce = 21;

			public const int Jungle = 22;

			public const int UndergroundJungle = 23;

			public const int SurfaceMushroom = 24;

			public const int UndergroundMushroom = 25;

			public const int Sky = 26;

			public const int Oasis = 27;

			public const int Ocean = 28;

			public const int Marble = 29;

			public const int Granite = 30;

			public const int JungleTemple = 31;

			public const int Dungeon = 32;

			public const int Underworld = 33;

			public const int SpiderNest = 34;

			public const int Graveyard = 35;

			public const int Day = 36;

			public const int Night = 37;

			public const int BloodMoon = 38;

			public const int Eclipse = 39;

			public const int Rain = 40;

			public const int WindyDay = 41;

			public const int Blizzard = 42;

			public const int Sandstorm = 43;

			public const int Meteor = 44;

			public const int Halloween = 45;

			public const int Christmas = 46;

			public const int SlimeRain = 47;

			public const int Party = 48;

			public const int GoblinInvasion = 49;

			public const int PirateInvasion = 50;

			public const int PumpkinMoon = 51;

			public const int FrostMoon = 52;

			public const int AlienInvasion = 53;

			public const int FrostLegion = 54;

			public const int OldOnesArmy = 55;

			public const int SolarTower = 56;

			public const int VortexTower = 57;

			public const int NebulaTower = 58;

			public const int StardustTower = 59;

			public const int Hardmode = 60;

			public const int ItemSpawn = 61;
		}

		private BestiaryDatabase _currentDatabase;

		private BestiaryEntry FindEntryByNPCID(int npcNetId)
		{
			return _currentDatabase.FindEntryByNPCID(npcNetId);
		}

		private BestiaryEntry Register(BestiaryEntry entry)
		{
			return _currentDatabase.Register(entry);
		}

		private IBestiaryEntryFilter Register(IBestiaryEntryFilter filter)
		{
			return _currentDatabase.Register(filter);
		}

		public void Populate(BestiaryDatabase database)
		{
			_currentDatabase = database;
			AddEmptyEntries_CrittersAndEnemies_Automated();
			AddTownNPCs_Manual();
			AddNPCBiomeRelationships_Automated();
			AddNPCBiomeRelationships_Manual();
			AddNPCBiomeRelationships_AddDecorations_Automated();
			ModifyEntriesThatNeedIt();
			RegisterFilters();
			RegisterSortSteps();
		}

		private void RegisterTestEntries()
		{
			Register(BestiaryEntry.Biome("Bestiary_Biomes.Hallow", "Images/UI/Bestiary/Biome_Hallow", Conditions.ReachHardMode));
		}

		private void RegisterSortSteps()
		{
			foreach (IBestiarySortStep item in new List<IBestiarySortStep>
			{
				new SortingSteps.ByUnlockState(),
				new SortingSteps.ByBestiarySortingId(),
				new SortingSteps.Alphabetical(),
				new SortingSteps.ByNetId(),
				new SortingSteps.ByAttack(),
				new SortingSteps.ByDefense(),
				new SortingSteps.ByCoins(),
				new SortingSteps.ByHP(),
				new SortingSteps.ByBestiaryRarity()
			})
			{
				_currentDatabase.Register(item);
			}
		}

		private void RegisterFilters()
		{
			Register(new Filters.ByUnlockState());
			Register(new Filters.ByBoss());
			Register(new Filters.ByRareCreature());
			List<IBestiaryInfoElement> commonInfoElementsForFilters = CommonTags.GetCommonInfoElementsForFilters();
			for (int i = 0; i < commonInfoElementsForFilters.Count; i++)
			{
				Register(new Filters.ByInfoElement(commonInfoElementsForFilters[i]));
			}
		}

		private void ModifyEntriesThatNeedIt_NameOverride(int npcID, string newNameKey)
		{
			BestiaryEntry bestiaryEntry = FindEntryByNPCID(npcID);
			bestiaryEntry.Info.RemoveAll((IBestiaryInfoElement x) => x is NamePlateInfoElement);
			bestiaryEntry.Info.Add(new NamePlateInfoElement(newNameKey, npcID));
			bestiaryEntry.Icon = new UnlockableNPCEntryIcon(npcID, 0f, 0f, 0f, 0f, newNameKey);
		}

		private void ModifyEntriesThatNeedIt()
		{
			FindEntryByNPCID(258).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.SurfaceMushroom));
			FindEntryByNPCID(-1).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCorruption));
			FindEntryByNPCID(81).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCorruption));
			FindEntryByNPCID(121).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCorruption));
			FindEntryByNPCID(7).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCorruption));
			FindEntryByNPCID(98).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCorruption));
			FindEntryByNPCID(6).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCorruption));
			FindEntryByNPCID(94).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCorruption));
			FindEntryByNPCID(173).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCrimson));
			FindEntryByNPCID(181).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCrimson));
			FindEntryByNPCID(183).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCrimson));
			FindEntryByNPCID(242).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCrimson));
			FindEntryByNPCID(241).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCrimson));
			FindEntryByNPCID(174).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCrimson));
			FindEntryByNPCID(240).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.TheCrimson));
			FindEntryByNPCID(175).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.UndergroundJungle));
			FindEntryByNPCID(153).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.Jungle));
			FindEntryByNPCID(52).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.Jungle));
			FindEntryByNPCID(58).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.Jungle));
			FindEntryByNPCID(102).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.Caverns));
			FindEntryByNPCID(157).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.Jungle));
			FindEntryByNPCID(51).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.Jungle));
			FindEntryByNPCID(169).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.UndergroundSnow));
			FindEntryByNPCID(510).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.UndergroundDesert));
			FindEntryByNPCID(69).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.Desert));
			FindEntryByNPCID(580).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.UndergroundDesert));
			FindEntryByNPCID(581).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.UndergroundDesert));
			FindEntryByNPCID(78).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.Desert));
			FindEntryByNPCID(79).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.CorruptDesert));
			FindEntryByNPCID(630).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.CrimsonDesert));
			FindEntryByNPCID(80).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.HallowDesert));
			FindEntryByNPCID(533).AddTags(new BestiaryPortraitBackgroundBasedOnWorldEvilProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert, CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert));
			FindEntryByNPCID(528).AddTags(new BestiaryPortraitBackgroundProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.UndergroundDesert));
			FindEntryByNPCID(529).AddTags(new BestiaryPortraitBackgroundBasedOnWorldEvilProviderPreferenceInfoElement(CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert, CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert));
			_currentDatabase.ApplyPass(TryGivingEntryFlavorTextIfItIsMissing);
			BestiaryEntry bestiaryEntry = FindEntryByNPCID(398);
			bestiaryEntry.Info.Add(new MoonLordPortraitBackgroundProviderBestiaryInfoElement());
			bestiaryEntry.Info.RemoveAll((IBestiaryInfoElement x) => x is NamePlateInfoElement);
			bestiaryEntry.Info.Add(new NamePlateInfoElement("Enemies.MoonLord", 398));
			bestiaryEntry.Icon = new UnlockableNPCEntryIcon(398, 0f, 0f, 0f, 0f, "Enemies.MoonLord");
			FindEntryByNPCID(664).Info.RemoveAll((IBestiaryInfoElement x) => x is NPCKillCounterInfoElement);
			FindEntryByNPCID(687).Info.RemoveAll((IBestiaryInfoElement x) => x is NPCKillCounterInfoElement);
			ModifyEntriesThatNeedIt_NameOverride(637, "Friends.TownCat");
			ModifyEntriesThatNeedIt_NameOverride(638, "Friends.TownDog");
			ModifyEntriesThatNeedIt_NameOverride(656, "Friends.TownBunny");
			for (int i = 494; i <= 506; i++)
			{
				FindEntryByNPCID(i).UIInfoProvider = new SalamanderShellyDadUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[i]);
			}
			FindEntryByNPCID(534).UIInfoProvider = new HighestOfMultipleUICollectionInfoProvider(new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[534], quickUnlock: false), new TownNPCUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[441]));
			foreach (NPCStatsReportInfoElement item in from x in FindEntryByNPCID(13).Info
				select x as NPCStatsReportInfoElement into x
				where x != null
				select x)
			{
				item.OnRefreshStats += AdjustEaterOfWorldStats;
			}
			foreach (NPCStatsReportInfoElement item2 in from x in FindEntryByNPCID(491).Info
				select x as NPCStatsReportInfoElement into x
				where x != null
				select x)
			{
				item2.OnRefreshStats += AdjustPirateShipStats;
			}
			FindEntryByNPCID(68).UIInfoProvider = new HighestOfMultipleUICollectionInfoProvider(new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[68], quickUnlock: true), new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[35], quickUnlock: true), new TownNPCUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[54]));
			FindEntryByNPCID(35).UIInfoProvider = new HighestOfMultipleUICollectionInfoProvider(new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[35], quickUnlock: true), new TownNPCUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[54]));
			FindEntryByNPCID(37).UIInfoProvider = new HighestOfMultipleUICollectionInfoProvider(new TownNPCUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[37]), new TownNPCUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[54]), new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[35], quickUnlock: true));
			FindEntryByNPCID(565).UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[565], quickUnlock: true);
			FindEntryByNPCID(577).UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[577], quickUnlock: true);
			FindEntryByNPCID(551).UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[551], quickUnlock: true);
			FindEntryByNPCID(491).UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[491], quickUnlock: true);
			foreach (KeyValuePair<int, int> item3 in new Dictionary<int, int>
			{
				{ 5, 4 },
				{ 267, 266 },
				{ 115, 113 },
				{ 116, 113 },
				{ 117, 113 },
				{ 139, 134 },
				{ 372, 370 },
				{ 658, 657 },
				{ 659, 657 },
				{ 660, 657 },
				{ 454, 439 },
				{ 521, 439 }
			})
			{
				int key = item3.Key;
				int value = item3.Value;
				FindEntryByNPCID(key).UIInfoProvider = new CommonEnemyUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[value], quickUnlock: true);
			}
			Dictionary<int, int[]> dictionary = new Dictionary<int, int[]>();
			dictionary.Add(443, new int[1] { 46 });
			dictionary.Add(442, new int[1] { 74 });
			dictionary.Add(592, new int[1] { 55 });
			dictionary.Add(444, new int[1] { 356 });
			dictionary.Add(601, new int[1] { 599 });
			dictionary.Add(445, new int[1] { 361 });
			dictionary.Add(446, new int[1] { 377 });
			dictionary.Add(605, new int[1] { 604 });
			dictionary.Add(447, new int[1] { 300 });
			dictionary.Add(627, new int[1] { 626 });
			dictionary.Add(613, new int[1] { 612 });
			dictionary.Add(448, new int[1] { 357 });
			dictionary.Add(539, new int[2] { 299, 538 });
			foreach (KeyValuePair<int, int[]> item4 in dictionary)
			{
				FindEntryByNPCID(item4.Key).UIInfoProvider = new GoldCritterUICollectionInfoProvider(item4.Value, ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[item4.Key]);
			}
			foreach (KeyValuePair<int, int> item5 in new Dictionary<int, int>
			{
				{ 362, 363 },
				{ 364, 365 },
				{ 602, 603 },
				{ 608, 609 }
			})
			{
				FindEntryByNPCID(item5.Key).UIInfoProvider = new HighestOfMultipleUICollectionInfoProvider(new CritterUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[item5.Key]), new CritterUICollectionInfoProvider(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[item5.Value]));
			}
			FindEntryByNPCID(4).AddTags(new SearchAliasInfoElement("eoc"));
			FindEntryByNPCID(13).AddTags(new SearchAliasInfoElement("eow"));
			FindEntryByNPCID(266).AddTags(new SearchAliasInfoElement("boc"));
			FindEntryByNPCID(113).AddTags(new SearchAliasInfoElement("wof"));
			FindEntryByNPCID(50).AddTags(new SearchAliasInfoElement("slime king"));
			FindEntryByNPCID(125).AddTags(new SearchAliasInfoElement("the twins"));
			FindEntryByNPCID(126).AddTags(new SearchAliasInfoElement("the twins"));
			FindEntryByNPCID(222).AddTags(new SearchAliasInfoElement("qb"));
			FindEntryByNPCID(222).AddTags(new SearchAliasInfoElement("bee queen"));
			FindEntryByNPCID(398).AddTags(new SearchAliasInfoElement("moonlord"));
			FindEntryByNPCID(398).AddTags(new SearchAliasInfoElement("cthulhu"));
			FindEntryByNPCID(398).AddTags(new SearchAliasInfoElement("ml"));
			FindEntryByNPCID(125).AddTags(new SearchAliasInfoElement("mech boss"));
			FindEntryByNPCID(126).AddTags(new SearchAliasInfoElement("mech boss"));
			FindEntryByNPCID(127).AddTags(new SearchAliasInfoElement("mech boss"));
			FindEntryByNPCID(134).AddTags(new SearchAliasInfoElement("mech boss"));
			FindEntryByNPCID(657).AddTags(new SearchAliasInfoElement("slime queen"));
			FindEntryByNPCID(636).AddTags(new SearchAliasInfoElement("eol"));
			FindEntryByNPCID(636).AddTags(new SearchAliasInfoElement("fairy"));
		}

		private void AdjustEaterOfWorldStats(NPCStatsReportInfoElement element)
		{
			element.LifeMax *= NPC.GetEaterOfWorldsSegmentsCountByGamemode(Main.GameMode);
		}

		private void AdjustPirateShipStats(NPCStatsReportInfoElement element)
		{
			NPC nPC = new NPC();
			int num = 4;
			nPC.SetDefaults(492, new NPCSpawnParams
			{
				strengthMultiplierOverride = 1f,
				playerCountForMultiplayerDifficultyOverride = 1,
				sizeScaleOverride = null,
				gameModeData = Main.GameModeInfo
			});
			element.LifeMax = num * nPC.lifeMax;
		}

		private void TryGivingEntryFlavorTextIfItIsMissing(BestiaryEntry entry)
		{
			if (entry.Info.Any((IBestiaryInfoElement x) => x is FlavorTextBestiaryInfoElement))
			{
				return;
			}
			SpawnConditionBestiaryInfoElement spawnConditionBestiaryInfoElement = null;
			int? num = null;
			foreach (IBestiaryInfoElement item in entry.Info)
			{
				if (item is BestiaryPortraitBackgroundProviderPreferenceInfoElement bestiaryPortraitBackgroundProviderPreferenceInfoElement && bestiaryPortraitBackgroundProviderPreferenceInfoElement.GetPreferredProvider() is SpawnConditionBestiaryInfoElement spawnConditionBestiaryInfoElement2)
				{
					spawnConditionBestiaryInfoElement = spawnConditionBestiaryInfoElement2;
					break;
				}
				if (item is SpawnConditionBestiaryInfoElement spawnConditionBestiaryInfoElement3)
				{
					int displayTextPriority = spawnConditionBestiaryInfoElement3.DisplayTextPriority;
					if (!num.HasValue || displayTextPriority >= num)
					{
						spawnConditionBestiaryInfoElement = spawnConditionBestiaryInfoElement3;
						num = displayTextPriority;
					}
				}
			}
			if (spawnConditionBestiaryInfoElement != null)
			{
				string displayNameKey = spawnConditionBestiaryInfoElement.GetDisplayNameKey();
				string text = "Bestiary_BiomeText.biome_";
				string text2 = displayNameKey.Substring(displayNameKey.IndexOf('.') + 1);
				text += text2;
				entry.Info.Add(new FlavorTextBestiaryInfoElement(text));
			}
		}

		private void AddTownNPCs_Manual()
		{
			Register(BestiaryEntry.TownNPC(22));
			Register(BestiaryEntry.TownNPC(17));
			Register(BestiaryEntry.TownNPC(18));
			Register(BestiaryEntry.TownNPC(19));
			Register(BestiaryEntry.TownNPC(20));
			Register(BestiaryEntry.TownNPC(37));
			Register(BestiaryEntry.TownNPC(54));
			Register(BestiaryEntry.TownNPC(38));
			Register(BestiaryEntry.TownNPC(107));
			Register(BestiaryEntry.TownNPC(108));
			Register(BestiaryEntry.TownNPC(124));
			Register(BestiaryEntry.TownNPC(142));
			Register(BestiaryEntry.TownNPC(160));
			Register(BestiaryEntry.TownNPC(178));
			Register(BestiaryEntry.TownNPC(207));
			Register(BestiaryEntry.TownNPC(208));
			Register(BestiaryEntry.TownNPC(209));
			Register(BestiaryEntry.TownNPC(227));
			Register(BestiaryEntry.TownNPC(228));
			Register(BestiaryEntry.TownNPC(229));
			Register(BestiaryEntry.TownNPC(353));
			Register(BestiaryEntry.TownNPC(369));
			Register(BestiaryEntry.TownNPC(441));
			Register(BestiaryEntry.TownNPC(550));
			Register(BestiaryEntry.TownNPC(588));
			Register(BestiaryEntry.TownNPC(368));
			Register(BestiaryEntry.TownNPC(453));
			Register(BestiaryEntry.TownNPC(633));
			Register(BestiaryEntry.TownNPC(663));
			Register(BestiaryEntry.TownNPC(638));
			Register(BestiaryEntry.TownNPC(637));
			Register(BestiaryEntry.TownNPC(656));
			Register(BestiaryEntry.TownNPC(670));
			Register(BestiaryEntry.TownNPC(678));
			Register(BestiaryEntry.TownNPC(679));
			Register(BestiaryEntry.TownNPC(680));
			Register(BestiaryEntry.TownNPC(681));
			Register(BestiaryEntry.TownNPC(682));
			Register(BestiaryEntry.TownNPC(683));
			Register(BestiaryEntry.TownNPC(684));
		}

		private void AddMultiEntryNPCS_Manual()
		{
			Register(BestiaryEntry.Enemy(85)).Icon = new UnlockableNPCEntryIcon(85, 0f, 0f, 0f, 3f);
		}

		private void AddEmptyEntries_CrittersAndEnemies_Automated()
		{
			HashSet<int> exclusions = GetExclusions();
			foreach (KeyValuePair<int, NPC> item in ContentSamples.NpcsByNetId)
			{
				if (!exclusions.Contains(item.Key) && !item.Value.isLikeATownNPC)
				{
					if (item.Value.CountsAsACritter)
					{
						Register(BestiaryEntry.Critter(item.Key));
					}
					else
					{
						Register(BestiaryEntry.Enemy(item.Key));
					}
				}
			}
		}

		private static HashSet<int> GetExclusions()
		{
			HashSet<int> hashSet = new HashSet<int>();
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, NPCID.Sets.NPCBestiaryDrawModifiers> item in NPCID.Sets.NPCBestiaryDrawOffset)
			{
				if (item.Value.Hide)
				{
					list.Add(item.Key);
				}
			}
			foreach (int item2 in list)
			{
				hashSet.Add(item2);
			}
			return hashSet;
		}

		private void AddNPCBiomeRelationships_Automated()
		{
			FindEntryByNPCID(357).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Events.Rain
			});
			FindEntryByNPCID(448).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Events.Rain
			});
			FindEntryByNPCID(606).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Graveyard });
			FindEntryByNPCID(211).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(377).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(446).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(595).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(596).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(597).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(598).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(599).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(600).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(601).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(612).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(613).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(25).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(30).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Goblins });
			FindEntryByNPCID(665).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(33).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(112).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(666).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(300).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(355).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(358).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Times.NightTime,
				CommonTags.SpawnConditions.Biomes.TheHallow
			});
			FindEntryByNPCID(447).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(610).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Graveyard });
			FindEntryByNPCID(210).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(261).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundMushroom });
			FindEntryByNPCID(402).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(403).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(485).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(486).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(487).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(359).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(410).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(604).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.WindyDay });
			FindEntryByNPCID(605).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.WindyDay });
			FindEntryByNPCID(218).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundSnow });
			FindEntryByNPCID(361).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(404).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(445).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(626).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(627).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(2).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(74).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(190).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(191).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(192).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(193).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(194).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(217).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(297).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(298).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(671).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Jungle,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(672).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Jungle,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(673).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Jungle,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(674).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Jungle,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(675).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Jungle,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(356).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(360).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			FindEntryByNPCID(655).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(653).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(654).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(442).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(444).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(669).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(677).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(676).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(582).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(583).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(584).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(585).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(1).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(59).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(138).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundHallow });
			FindEntryByNPCID(147).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Snow,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(265).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(367).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Desert,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(616).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(617).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Jungle,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(23).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Meteor });
			FindEntryByNPCID(55).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(57).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(58).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Underground,
				CommonTags.SpawnConditions.Biomes.Jungle
			});
			FindEntryByNPCID(102).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(157).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(219).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(220).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(236).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(302).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Halloween });
			FindEntryByNPCID(366).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Desert,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(465).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(537).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(592).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(607).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Desert });
			FindEntryByNPCID(10).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(11).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(12).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(34).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(117).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(118).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(119).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(163).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SpiderNest });
			FindEntryByNPCID(164).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SpiderNest });
			FindEntryByNPCID(230).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Rain });
			FindEntryByNPCID(241).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCrimson });
			FindEntryByNPCID(406).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(496).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(497).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(519).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(593).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Rain });
			FindEntryByNPCID(625).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(49).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(51).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(60).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(93).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(137).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundHallow });
			FindEntryByNPCID(184).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundSnow });
			FindEntryByNPCID(204).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(224).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Rain });
			FindEntryByNPCID(259).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			FindEntryByNPCID(299).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(317).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(318).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(378).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(393).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(494).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(495).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(513).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(514).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(515).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(538).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(539).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(580).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(587).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(16).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(71).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(81).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(183).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCrimson });
			FindEntryByNPCID(67).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(70).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(75).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(239).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCrimson });
			FindEntryByNPCID(267).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCrimson });
			FindEntryByNPCID(288).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(394).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(408).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(428).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.VortexPillar });
			FindEntryByNPCID(43).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(56).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(72).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(141).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(185).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundSnow });
			FindEntryByNPCID(374).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundMushroom });
			FindEntryByNPCID(375).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundMushroom });
			FindEntryByNPCID(661).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Times.NightTime,
				CommonTags.SpawnConditions.Biomes.TheHallow
			});
			FindEntryByNPCID(388).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(602).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(603).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(115).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(232).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(258).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			FindEntryByNPCID(409).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(462).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(516).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(42).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(46).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(47).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(69).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Desert });
			FindEntryByNPCID(231).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(235).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(247).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheTemple });
			FindEntryByNPCID(248).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheTemple });
			FindEntryByNPCID(303).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Events.Halloween,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(304).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Graveyard });
			FindEntryByNPCID(337).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Events.Christmas,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(354).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SpiderNest });
			FindEntryByNPCID(362).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(363).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(364).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(365).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(395).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(443).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(464).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(508).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(532).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(540).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Events.Party,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(578).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(608).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Desert });
			FindEntryByNPCID(609).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Desert });
			FindEntryByNPCID(611).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(264).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(101).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(121).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(122).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Times.NightTime,
				CommonTags.SpawnConditions.Biomes.TheHallow
			});
			FindEntryByNPCID(132).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(148).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Snow });
			FindEntryByNPCID(149).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Snow });
			FindEntryByNPCID(168).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(234).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(250).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Rain });
			FindEntryByNPCID(257).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			FindEntryByNPCID(421).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.NebulaPillar });
			FindEntryByNPCID(470).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(472).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Goblins });
			FindEntryByNPCID(478).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(546).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Desert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(581).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(615).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(256).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			FindEntryByNPCID(133).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(221).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(252).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Pirates });
			FindEntryByNPCID(329).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(385).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(427).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.VortexPillar });
			FindEntryByNPCID(490).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(548).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(63).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(64).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(85).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(629).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundSnow });
			FindEntryByNPCID(103).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(152).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Times.NightTime,
				CommonTags.SpawnConditions.Biomes.Jungle
			});
			FindEntryByNPCID(174).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCrimson });
			FindEntryByNPCID(195).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(254).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SurfaceMushroom });
			FindEntryByNPCID(260).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.SurfaceMushroom,
				CommonTags.SpawnConditions.Biomes.UndergroundMushroom
			});
			FindEntryByNPCID(382).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(383).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(386).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(389).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(466).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(467).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(489).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(530).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(175).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(176).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(188).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(3).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(7).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(8).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(9).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(95).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(96).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(97).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(98).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(99).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(100).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(120).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundHallow });
			FindEntryByNPCID(150).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundSnow });
			FindEntryByNPCID(151).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(153).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(154).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundSnow });
			FindEntryByNPCID(158).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(161).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Times.NightTime,
				CommonTags.SpawnConditions.Biomes.Snow
			});
			FindEntryByNPCID(186).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(187).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(189).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(223).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Events.Rain,
				CommonTags.SpawnConditions.Times.NightTime
			});
			FindEntryByNPCID(233).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(251).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(319).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(320).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(321).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(331).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(332).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(338).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(339).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(340).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(341).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(342).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(350).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(381).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(492).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Pirates });
			FindEntryByNPCID(510).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(511).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(512).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(552).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(553).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(554).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(590).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(82).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(116).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(166).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(199).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheTemple });
			FindEntryByNPCID(263).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(371).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(461).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(463).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(523).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(52).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Times.NightTime,
				CommonTags.SpawnConditions.Biomes.Jungle
			});
			FindEntryByNPCID(200).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(244).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.TheHallow,
				CommonTags.SpawnConditions.Events.Rain
			});
			FindEntryByNPCID(255).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SurfaceMushroom });
			FindEntryByNPCID(384).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(387).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(390).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(418).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(420).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.NebulaPillar });
			FindEntryByNPCID(460).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(468).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(524).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(525).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert });
			FindEntryByNPCID(526).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert });
			FindEntryByNPCID(527).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.HallowUndergroundDesert });
			FindEntryByNPCID(536).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(566).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(567).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(53).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(169).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundSnow });
			FindEntryByNPCID(301).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Graveyard });
			FindEntryByNPCID(391).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(405).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(423).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.NebulaPillar });
			FindEntryByNPCID(438).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(498).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(499).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(500).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(501).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(502).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(503).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(504).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(505).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(506).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(534).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(568).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(569).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(21).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(24).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(26).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Goblins });
			FindEntryByNPCID(27).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Goblins });
			FindEntryByNPCID(28).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Goblins });
			FindEntryByNPCID(29).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Goblins });
			FindEntryByNPCID(31).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(32).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(44).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(73).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(77).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(78).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Desert });
			FindEntryByNPCID(79).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.CorruptDesert });
			FindEntryByNPCID(630).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.CrimsonDesert });
			FindEntryByNPCID(80).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.HallowDesert });
			FindEntryByNPCID(104).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(111).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Goblins });
			FindEntryByNPCID(140).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(159).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(162).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(196).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(198).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheTemple });
			FindEntryByNPCID(201).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(202).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(203).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(212).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Pirates });
			FindEntryByNPCID(213).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Pirates });
			FindEntryByNPCID(242).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCrimson });
			FindEntryByNPCID(269).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(270).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(272).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(273).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(275).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(276).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(277).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(278).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(279).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(280).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(281).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(282).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(283).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(284).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(285).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(286).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(287).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(294).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(295).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(296).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(310).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(311).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(312).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(313).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(316).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Graveyard });
			FindEntryByNPCID(326).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(415).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(449).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(450).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(451).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(452).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(471).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Goblins });
			FindEntryByNPCID(482).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Granite });
			FindEntryByNPCID(572).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(573).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(143).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostLegion });
			FindEntryByNPCID(144).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostLegion });
			FindEntryByNPCID(145).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostLegion });
			FindEntryByNPCID(155).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Times.NightTime,
				CommonTags.SpawnConditions.Biomes.Snow
			});
			FindEntryByNPCID(271).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(274).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(314).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(352).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(379).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(509).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(555).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(556).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(557).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(61).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Desert });
			FindEntryByNPCID(110).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(206).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundSnow });
			FindEntryByNPCID(214).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Pirates });
			FindEntryByNPCID(215).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Pirates });
			FindEntryByNPCID(216).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Pirates });
			FindEntryByNPCID(225).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Rain });
			FindEntryByNPCID(291).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(292).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(293).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(347).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(412).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(413).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(414).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(469).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(473).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(474).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(475).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundHallow });
			FindEntryByNPCID(476).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(483).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Granite });
			FindEntryByNPCID(586).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(62).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(131).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(165).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SpiderNest });
			FindEntryByNPCID(167).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundSnow });
			FindEntryByNPCID(197).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundSnow });
			FindEntryByNPCID(226).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheTemple });
			FindEntryByNPCID(237).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(238).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SpiderNest });
			FindEntryByNPCID(480).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Marble });
			FindEntryByNPCID(528).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(529).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(289).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(439).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(440).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(533).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(170).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.CorruptIce });
			FindEntryByNPCID(171).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.HallowIce });
			FindEntryByNPCID(179).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(180).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.CrimsonIce });
			FindEntryByNPCID(181).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCrimson });
			FindEntryByNPCID(205).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(411).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(424).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.NebulaPillar });
			FindEntryByNPCID(429).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.VortexPillar });
			FindEntryByNPCID(481).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Marble });
			FindEntryByNPCID(240).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCrimson });
			FindEntryByNPCID(290).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(430).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(431).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Snow,
				CommonTags.SpawnConditions.Times.NightTime
			});
			FindEntryByNPCID(432).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(433).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(434).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(435).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(436).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(479).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(518).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(591).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(45).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(130).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(172).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(305).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(306).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(307).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(308).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(309).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(425).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.VortexPillar });
			FindEntryByNPCID(426).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.VortexPillar });
			FindEntryByNPCID(570).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(571).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(417).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(419).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(65).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(372).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(373).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(407).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(542).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Desert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(543).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.CorruptDesert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(544).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.CrimsonDesert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(545).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.HallowDesert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(619).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(621).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(622).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(623).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(128).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(177).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(561).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(562).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(563).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(594).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.WindyDay });
			FindEntryByNPCID(253).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(129).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(6).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(173).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCrimson });
			FindEntryByNPCID(399).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Sky });
			FindEntryByNPCID(416).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(531).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(83).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(84).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundHallow });
			FindEntryByNPCID(86).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(330).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(620).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(48).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Sky });
			FindEntryByNPCID(268).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(328).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(66).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(182).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(13).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(14).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(15).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(39).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(40).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(41).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(315).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(343).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(94).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(392).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(558).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(559).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(560).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(348).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(349).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(156).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(35).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(68).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(134).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(136).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(135).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(454).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(455).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(456).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(457).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(458).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(459).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(113).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(114).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheUnderworld });
			FindEntryByNPCID(564).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(565).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(327).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(520).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Martian });
			FindEntryByNPCID(574).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(575).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(246).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheTemple });
			FindEntryByNPCID(50).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(477).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Eclipse });
			FindEntryByNPCID(541).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Desert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(109).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(243).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Snow,
				CommonTags.SpawnConditions.Events.Rain,
				CommonTags.SpawnConditions.Events.Blizzard
			});
			FindEntryByNPCID(618).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.BloodMoon });
			FindEntryByNPCID(351).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(249).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheTemple });
			FindEntryByNPCID(222).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(262).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(87).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Sky });
			FindEntryByNPCID(88).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Sky });
			FindEntryByNPCID(89).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Sky });
			FindEntryByNPCID(90).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Sky });
			FindEntryByNPCID(91).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Sky });
			FindEntryByNPCID(92).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Sky });
			FindEntryByNPCID(127).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(346).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(370).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(4).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(551).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(245).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheTemple });
			FindEntryByNPCID(576).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(577).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(266).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCrimson });
			FindEntryByNPCID(325).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.PumpkinMoon });
			FindEntryByNPCID(344).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(125).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(126).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(549).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.OldOnesArmy });
			FindEntryByNPCID(345).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.FrostMoon });
			FindEntryByNPCID(668).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Snow,
				CommonTags.SpawnConditions.Events.Rain,
				CommonTags.SpawnConditions.Events.Blizzard
			});
			FindEntryByNPCID(422).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.VortexPillar });
			FindEntryByNPCID(493).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.StardustPillar });
			FindEntryByNPCID(507).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.NebulaPillar });
			FindEntryByNPCID(517).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SolarPillar });
			FindEntryByNPCID(491).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Invasions.Pirates });
		}

		private void AddNPCBiomeRelationships_Manual()
		{
			FindEntryByNPCID(628).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.WindyDay });
			FindEntryByNPCID(-4).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(-3).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(-7).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(1).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.DayTime });
			FindEntryByNPCID(-10).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Jungle,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(-8).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(-9).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(-6).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(-5).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(-2).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheCorruption });
			FindEntryByNPCID(-1).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.TheCorruption,
				CommonTags.SpawnConditions.Biomes.UndergroundCorruption
			});
			FindEntryByNPCID(81).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(121).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(7).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(8).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(9).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(98).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(99).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(100).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(6).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(94).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCorruption });
			FindEntryByNPCID(173).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(181).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(183).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(242).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(241).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(174).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(240).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundCrimson });
			FindEntryByNPCID(175).Info.Remove(CommonTags.SpawnConditions.Biomes.UndergroundJungle);
			FindEntryByNPCID(175).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Jungle,
				CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			FindEntryByNPCID(153).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(52).Info.Remove(CommonTags.SpawnConditions.Times.NightTime);
			FindEntryByNPCID(52).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.UndergroundJungle,
				CommonTags.SpawnConditions.Times.NightTime
			});
			FindEntryByNPCID(58).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(102).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Jungle,
				CommonTags.SpawnConditions.Biomes.UndergroundJungle
			});
			FindEntryByNPCID(157).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(51).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundJungle });
			FindEntryByNPCID(169).Info.Remove(CommonTags.SpawnConditions.Biomes.UndergroundSnow);
			FindEntryByNPCID(169).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Snow,
				CommonTags.SpawnConditions.Biomes.UndergroundSnow
			});
			FindEntryByNPCID(510).Info.Remove(CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			FindEntryByNPCID(510).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Desert,
				CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(511).Info.Remove(CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			FindEntryByNPCID(511).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Desert,
				CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(512).Info.Remove(CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			FindEntryByNPCID(512).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Desert,
				CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(69).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(580).Info.Remove(CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			FindEntryByNPCID(580).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Desert,
				CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(581).Info.Remove(CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			FindEntryByNPCID(581).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Desert,
				CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				CommonTags.SpawnConditions.Events.Sandstorm
			});
			FindEntryByNPCID(78).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundDesert });
			FindEntryByNPCID(79).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert });
			FindEntryByNPCID(630).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert });
			FindEntryByNPCID(80).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.HallowUndergroundDesert });
			FindEntryByNPCID(533).Info.Remove(CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			FindEntryByNPCID(533).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert,
				CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert
			});
			FindEntryByNPCID(528).Info.Remove(CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			FindEntryByNPCID(528).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				CommonTags.SpawnConditions.Biomes.HallowUndergroundDesert
			});
			FindEntryByNPCID(529).Info.Remove(CommonTags.SpawnConditions.Biomes.UndergroundDesert);
			FindEntryByNPCID(529).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert,
				CommonTags.SpawnConditions.Biomes.CrimsonUndergroundDesert
			});
			FindEntryByNPCID(624).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(5).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(139).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(484).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Times.NightTime });
			FindEntryByNPCID(317).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Halloween });
			FindEntryByNPCID(318).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Halloween });
			FindEntryByNPCID(320).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Halloween });
			FindEntryByNPCID(321).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Halloween });
			FindEntryByNPCID(319).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Halloween });
			FindEntryByNPCID(324).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Events.Halloween,
				CommonTags.SpawnConditions.Biomes.Caverns
			});
			FindEntryByNPCID(322).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Events.Halloween,
				CommonTags.SpawnConditions.Biomes.Caverns
			});
			FindEntryByNPCID(323).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Events.Halloween,
				CommonTags.SpawnConditions.Biomes.Caverns
			});
			FindEntryByNPCID(302).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(521).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(332).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Christmas });
			FindEntryByNPCID(331).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Events.Christmas });
			FindEntryByNPCID(335).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Events.Christmas,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(336).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Events.Christmas,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(333).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Events.Christmas,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(334).Info.AddRange(new IBestiaryInfoElement[3]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Events.Christmas,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(535).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Surface,
				CommonTags.SpawnConditions.Times.DayTime
			});
			FindEntryByNPCID(614).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(225).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(224).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(250).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(632).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Graveyard });
			FindEntryByNPCID(631).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(634).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundMushroom });
			FindEntryByNPCID(635).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.UndergroundMushroom });
			FindEntryByNPCID(636).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Times.NightTime,
				CommonTags.SpawnConditions.Biomes.TheHallow
			});
			FindEntryByNPCID(639).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(640).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(641).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(642).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(643).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(644).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(645).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(646).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(647).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(648).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(649).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(650).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(651).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(652).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Caverns });
			FindEntryByNPCID(657).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(658).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(660).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(659).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(22).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(17).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(588).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(441).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Snow });
			FindEntryByNPCID(124).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Snow });
			FindEntryByNPCID(209).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Snow });
			FindEntryByNPCID(142).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Snow,
				CommonTags.SpawnConditions.Events.Christmas
			});
			FindEntryByNPCID(207).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Desert });
			FindEntryByNPCID(19).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Desert });
			FindEntryByNPCID(178).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Desert });
			FindEntryByNPCID(20).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(228).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(227).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(369).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(229).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(353).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Ocean });
			FindEntryByNPCID(38).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(107).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(54).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(108).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(18).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(208).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(550).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(633).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(663).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheHallow });
			FindEntryByNPCID(160).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.SurfaceMushroom });
			FindEntryByNPCID(637).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(638).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(656).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(670).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(678).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(679).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(680).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(681).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(682).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(683).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(684).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(687).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Jungle });
			FindEntryByNPCID(368).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Surface });
			FindEntryByNPCID(37).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.TheDungeon });
			FindEntryByNPCID(453).Info.AddRange(new IBestiaryInfoElement[1] { CommonTags.SpawnConditions.Biomes.Underground });
			FindEntryByNPCID(664).Info.AddRange(new IBestiaryInfoElement[2]
			{
				CommonTags.SpawnConditions.Biomes.Underground,
				CommonTags.SpawnConditions.Biomes.Caverns
			});
		}

		private void AddNPCBiomeRelationships_AddDecorations_Automated()
		{
			foreach (KeyValuePair<int, NPC> item in ContentSamples.NpcsByNetId)
			{
				BestiaryEntry bestiaryEntry = FindEntryByNPCID(item.Key);
				if (bestiaryEntry.Info.Contains(CommonTags.SpawnConditions.Events.Rain))
				{
					if (bestiaryEntry.Info.Contains(CommonTags.SpawnConditions.Biomes.Snow))
					{
						bestiaryEntry.AddTags(CommonTags.SpawnConditions.Visuals.Blizzard);
					}
					else
					{
						bestiaryEntry.AddTags(CommonTags.SpawnConditions.Visuals.Rain);
					}
					continue;
				}
				if (bestiaryEntry.Info.Contains(CommonTags.SpawnConditions.Events.Eclipse))
				{
					bestiaryEntry.AddTags(CommonTags.SpawnConditions.Visuals.EclipseSun);
				}
				if (bestiaryEntry.Info.Contains(CommonTags.SpawnConditions.Times.NightTime))
				{
					bestiaryEntry.AddTags(CommonTags.SpawnConditions.Visuals.Moon);
				}
				if (bestiaryEntry.Info.Contains(CommonTags.SpawnConditions.Times.DayTime))
				{
					bestiaryEntry.AddTags(CommonTags.SpawnConditions.Visuals.Sun);
				}
				if (bestiaryEntry.Info.Contains(CommonTags.SpawnConditions.Invasions.PumpkinMoon))
				{
					bestiaryEntry.AddTags(CommonTags.SpawnConditions.Visuals.PumpkinMoon);
				}
				if (bestiaryEntry.Info.Contains(CommonTags.SpawnConditions.Invasions.FrostMoon))
				{
					bestiaryEntry.AddTags(CommonTags.SpawnConditions.Visuals.FrostMoon);
				}
				if (bestiaryEntry.Info.Contains(CommonTags.SpawnConditions.Biomes.Meteor))
				{
					bestiaryEntry.AddTags(CommonTags.SpawnConditions.Visuals.Moon);
					bestiaryEntry.AddTags(CommonTags.SpawnConditions.Visuals.Meteor);
				}
			}
		}
	}
}
