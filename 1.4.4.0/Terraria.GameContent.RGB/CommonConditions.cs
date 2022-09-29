using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public static class CommonConditions
	{
		public abstract class ConditionBase : ChromaCondition
		{
			protected Player CurrentPlayer => Main.player[Main.myPlayer];
		}

		private class SimpleCondition : ConditionBase
		{
			private Func<Player, bool> _condition;

			public SimpleCondition(Func<Player, bool> condition)
			{
				_condition = condition;
			}

			public override bool IsActive()
			{
				return _condition(base.CurrentPlayer);
			}
		}

		public static class SurfaceBiome
		{
			public static readonly ChromaCondition Ocean = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneBeach && player.ZoneOverworldHeight);

			public static readonly ChromaCondition Desert = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneDesert && !player.ZoneBeach && player.ZoneOverworldHeight);

			public static readonly ChromaCondition Jungle = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneJungle && player.ZoneOverworldHeight);

			public static readonly ChromaCondition Snow = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneSnow && player.ZoneOverworldHeight);

			public static readonly ChromaCondition Mushroom = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneGlowshroom && player.ZoneOverworldHeight);

			public static readonly ChromaCondition Corruption = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneCorrupt && player.ZoneOverworldHeight);

			public static readonly ChromaCondition Hallow = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneHallow && player.ZoneOverworldHeight);

			public static readonly ChromaCondition Crimson = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneCrimson && player.ZoneOverworldHeight);
		}

		public static class MiscBiome
		{
			public static readonly ChromaCondition Meteorite = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneMeteor);
		}

		public static class UndergroundBiome
		{
			public static readonly ChromaCondition Hive = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneHive);

			public static readonly ChromaCondition Jungle = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneJungle && !player.ZoneOverworldHeight);

			public static readonly ChromaCondition Mushroom = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneGlowshroom && !player.ZoneOverworldHeight);

			public static readonly ChromaCondition Ice = (ChromaCondition)(object)new SimpleCondition(InIce);

			public static readonly ChromaCondition HallowIce = (ChromaCondition)(object)new SimpleCondition((Player player) => InIce(player) && player.ZoneHallow);

			public static readonly ChromaCondition CrimsonIce = (ChromaCondition)(object)new SimpleCondition((Player player) => InIce(player) && player.ZoneCrimson);

			public static readonly ChromaCondition CorruptIce = (ChromaCondition)(object)new SimpleCondition((Player player) => InIce(player) && player.ZoneCorrupt);

			public static readonly ChromaCondition Hallow = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneHallow && !player.ZoneOverworldHeight);

			public static readonly ChromaCondition Crimson = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneCrimson && !player.ZoneOverworldHeight);

			public static readonly ChromaCondition Corrupt = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneCorrupt && !player.ZoneOverworldHeight);

			public static readonly ChromaCondition Desert = (ChromaCondition)(object)new SimpleCondition(InDesert);

			public static readonly ChromaCondition HallowDesert = (ChromaCondition)(object)new SimpleCondition((Player player) => InDesert(player) && player.ZoneHallow);

			public static readonly ChromaCondition CrimsonDesert = (ChromaCondition)(object)new SimpleCondition((Player player) => InDesert(player) && player.ZoneCrimson);

			public static readonly ChromaCondition CorruptDesert = (ChromaCondition)(object)new SimpleCondition((Player player) => InDesert(player) && player.ZoneCorrupt);

			public static readonly ChromaCondition Temple = (ChromaCondition)(object)new SimpleCondition(InTemple);

			public static readonly ChromaCondition Dungeon = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneDungeon);

			public static readonly ChromaCondition Marble = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneMarble);

			public static readonly ChromaCondition Granite = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneGranite);

			public static readonly ChromaCondition GemCave = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneGemCave);

			public static readonly ChromaCondition Shimmer = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneShimmer);

			private static bool InTemple(Player player)
			{
				int num = (int)(player.position.X + (float)(player.width / 2)) / 16;
				int num2 = (int)(player.position.Y + (float)(player.height / 2)) / 16;
				if (WorldGen.InWorld(num, num2) && Main.tile[num, num2] != null)
				{
					return Main.tile[num, num2].wall == 87;
				}
				return false;
			}

			private static bool InIce(Player player)
			{
				if (player.ZoneSnow)
				{
					return !player.ZoneOverworldHeight;
				}
				return false;
			}

			private static bool InDesert(Player player)
			{
				if (player.ZoneDesert)
				{
					return !player.ZoneOverworldHeight;
				}
				return false;
			}
		}

		public static class Boss
		{
			public static int HighestTierBossOrEvent;

			public static readonly ChromaCondition EaterOfWorlds = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 13);

			public static readonly ChromaCondition Destroyer = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 134);

			public static readonly ChromaCondition KingSlime = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 50);

			public static readonly ChromaCondition QueenSlime = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 657);

			public static readonly ChromaCondition BrainOfCthulhu = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 266);

			public static readonly ChromaCondition DukeFishron = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 370);

			public static readonly ChromaCondition QueenBee = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 222);

			public static readonly ChromaCondition Plantera = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 262);

			public static readonly ChromaCondition Empress = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 636);

			public static readonly ChromaCondition EyeOfCthulhu = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 4);

			public static readonly ChromaCondition TheTwins = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 126);

			public static readonly ChromaCondition MoonLord = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 398);

			public static readonly ChromaCondition WallOfFlesh = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 113);

			public static readonly ChromaCondition Golem = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 245);

			public static readonly ChromaCondition Cultist = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 439);

			public static readonly ChromaCondition Skeletron = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 35);

			public static readonly ChromaCondition SkeletronPrime = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 127);

			public static readonly ChromaCondition Deerclops = (ChromaCondition)(object)new SimpleCondition((Player player) => HighestTierBossOrEvent == 668);
		}

		public static class Weather
		{
			public static readonly ChromaCondition Rain = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneRain && !player.ZoneSnow && !player.ZoneSandstorm);

			public static readonly ChromaCondition Sandstorm = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneSandstorm);

			public static readonly ChromaCondition Blizzard = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneSnow && player.ZoneRain);

			public static readonly ChromaCondition SlimeRain = (ChromaCondition)(object)new SimpleCondition((Player player) => Main.slimeRain && player.ZoneOverworldHeight);
		}

		public static class Depth
		{
			public static readonly ChromaCondition Sky = (ChromaCondition)(object)new SimpleCondition((Player player) => (double)(player.position.Y / 16f) < Main.worldSurface * 0.44999998807907104);

			public static readonly ChromaCondition Surface = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneOverworldHeight && !((double)(player.position.Y / 16f) < Main.worldSurface * 0.44999998807907104) && !IsPlayerInFrontOfDirtWall(player));

			public static readonly ChromaCondition Vines = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneOverworldHeight && !((double)(player.position.Y / 16f) < Main.worldSurface * 0.44999998807907104) && IsPlayerInFrontOfDirtWall(player));

			public static readonly ChromaCondition Underground = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneDirtLayerHeight);

			public static readonly ChromaCondition Caverns = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneRockLayerHeight && player.position.ToTileCoordinates().Y <= Main.maxTilesY - 400);

			public static readonly ChromaCondition Magma = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneRockLayerHeight && player.position.ToTileCoordinates().Y > Main.maxTilesY - 400);

			public static readonly ChromaCondition Underworld = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneUnderworldHeight);

			private static bool IsPlayerInFrontOfDirtWall(Player player)
			{
				Point point = player.Center.ToTileCoordinates();
				if (!WorldGen.InWorld(point.X, point.Y))
				{
					return false;
				}
				if (Main.tile[point.X, point.Y] == null)
				{
					return false;
				}
				switch (Main.tile[point.X, point.Y].wall)
				{
				case 2:
				case 16:
				case 54:
				case 55:
				case 56:
				case 57:
				case 58:
				case 59:
				case 61:
				case 170:
				case 171:
				case 185:
				case 196:
				case 197:
				case 198:
				case 199:
				case 212:
				case 213:
				case 214:
				case 215:
					return true;
				default:
					return false;
				}
			}
		}

		public static class Events
		{
			public static readonly ChromaCondition BloodMoon = (ChromaCondition)(object)new SimpleCondition((Player player) => Main.bloodMoon && !Main.snowMoon && !Main.pumpkinMoon);

			public static readonly ChromaCondition FrostMoon = (ChromaCondition)(object)new SimpleCondition((Player player) => Main.snowMoon);

			public static readonly ChromaCondition PumpkinMoon = (ChromaCondition)(object)new SimpleCondition((Player player) => Main.pumpkinMoon);

			public static readonly ChromaCondition SolarEclipse = (ChromaCondition)(object)new SimpleCondition((Player player) => Main.eclipse);

			public static readonly ChromaCondition SolarPillar = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneTowerSolar);

			public static readonly ChromaCondition NebulaPillar = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneTowerNebula);

			public static readonly ChromaCondition VortexPillar = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneTowerVortex);

			public static readonly ChromaCondition StardustPillar = (ChromaCondition)(object)new SimpleCondition((Player player) => player.ZoneTowerStardust);

			public static readonly ChromaCondition PirateInvasion = (ChromaCondition)(object)new SimpleCondition((Player player) => Boss.HighestTierBossOrEvent == -3);

			public static readonly ChromaCondition DD2Event = (ChromaCondition)(object)new SimpleCondition((Player player) => Boss.HighestTierBossOrEvent == -6);

			public static readonly ChromaCondition FrostLegion = (ChromaCondition)(object)new SimpleCondition((Player player) => Boss.HighestTierBossOrEvent == -2);

			public static readonly ChromaCondition MartianMadness = (ChromaCondition)(object)new SimpleCondition((Player player) => Boss.HighestTierBossOrEvent == -4);

			public static readonly ChromaCondition GoblinArmy = (ChromaCondition)(object)new SimpleCondition((Player player) => Boss.HighestTierBossOrEvent == -1);
		}

		public static class Alert
		{
			public static readonly ChromaCondition MoonlordComing = (ChromaCondition)(object)new SimpleCondition((Player player) => NPC.MoonLordCountdown > 0);

			public static readonly ChromaCondition Drowning = (ChromaCondition)(object)new SimpleCondition((Player player) => player.breath != player.breathMax);

			public static readonly ChromaCondition Keybinds = (ChromaCondition)(object)new SimpleCondition((Player player) => Main.InGameUI.CurrentState == Main.ManageControlsMenu || Main.MenuUI.CurrentState == Main.ManageControlsMenu);

			public static readonly ChromaCondition LavaIndicator = (ChromaCondition)(object)new SimpleCondition((Player player) => player.lavaWet);
		}

		public static class CriticalAlert
		{
			public static readonly ChromaCondition LowLife = (ChromaCondition)(object)new SimpleCondition((Player player) => Main.ChromaPainter.PotionAlert);

			public static readonly ChromaCondition Death = (ChromaCondition)(object)new SimpleCondition((Player player) => player.dead);
		}

		public static readonly ChromaCondition InMenu = (ChromaCondition)(object)new SimpleCondition((Player player) => Main.gameMenu && !Main.drunkWorld);

		public static readonly ChromaCondition DrunkMenu = (ChromaCondition)(object)new SimpleCondition((Player player) => Main.gameMenu && Main.drunkWorld);
	}
}
