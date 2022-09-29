using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.NetModules;
using Terraria.Net;

namespace Terraria.GameContent.Ambience
{
	public class AmbienceServer
	{
		public struct AmbienceSpawnInfo
		{
			public SkyEntityType skyEntityType;

			public int targetPlayer;
		}

		private const int MINIMUM_SECONDS_BETWEEN_SPAWNS = 10;

		private const int MAXIMUM_SECONDS_BETWEEN_SPAWNS = 120;

		private readonly Dictionary<SkyEntityType, Func<bool>> _spawnConditions = new Dictionary<SkyEntityType, Func<bool>>();

		private readonly Dictionary<SkyEntityType, Func<Player, bool>> _secondarySpawnConditionsPerPlayer = new Dictionary<SkyEntityType, Func<Player, bool>>();

		private int _updatesUntilNextAttempt;

		private List<AmbienceSpawnInfo> _forcedSpawns = new List<AmbienceSpawnInfo>();

		private static bool IsSunnyDay()
		{
			if (!Main.IsItRaining && Main.dayTime)
			{
				return !Main.eclipse;
			}
			return false;
		}

		private static bool IsSunset()
		{
			if (Main.dayTime)
			{
				return Main.time > 40500.0;
			}
			return false;
		}

		private static bool IsCalmNight()
		{
			if (!Main.IsItRaining && !Main.dayTime && !Main.bloodMoon && !Main.pumpkinMoon)
			{
				return !Main.snowMoon;
			}
			return false;
		}

		public AmbienceServer()
		{
			ResetSpawnTime();
			_spawnConditions[SkyEntityType.BirdsV] = IsSunnyDay;
			_spawnConditions[SkyEntityType.Wyvern] = () => IsSunnyDay() && Main.hardMode;
			_spawnConditions[SkyEntityType.Airship] = () => IsSunnyDay() && Main.IsItAHappyWindyDay;
			_spawnConditions[SkyEntityType.AirBalloon] = () => IsSunnyDay() && !Main.IsItAHappyWindyDay;
			_spawnConditions[SkyEntityType.Eyeball] = () => !Main.dayTime;
			_spawnConditions[SkyEntityType.Butterflies] = () => IsSunnyDay() && !Main.IsItAHappyWindyDay && !NPC.TooWindyForButterflies && NPC.butterflyChance < 6;
			_spawnConditions[SkyEntityType.LostKite] = () => Main.dayTime && !Main.eclipse && Main.IsItAHappyWindyDay;
			_spawnConditions[SkyEntityType.Vulture] = () => IsSunnyDay();
			_spawnConditions[SkyEntityType.Bats] = () => (IsSunset() && IsSunnyDay()) || IsCalmNight();
			_spawnConditions[SkyEntityType.PixiePosse] = () => IsSunnyDay() || IsCalmNight();
			_spawnConditions[SkyEntityType.Seagulls] = () => IsSunnyDay();
			_spawnConditions[SkyEntityType.SlimeBalloons] = () => IsSunnyDay() && Main.IsItAHappyWindyDay;
			_spawnConditions[SkyEntityType.Gastropods] = () => IsCalmNight();
			_spawnConditions[SkyEntityType.Pegasus] = () => IsSunnyDay();
			_spawnConditions[SkyEntityType.EaterOfSouls] = () => IsSunnyDay() || IsCalmNight();
			_spawnConditions[SkyEntityType.Crimera] = () => IsSunnyDay() || IsCalmNight();
			_spawnConditions[SkyEntityType.Hellbats] = () => true;
			_secondarySpawnConditionsPerPlayer[SkyEntityType.Vulture] = (Player player) => player.ZoneDesert;
			_secondarySpawnConditionsPerPlayer[SkyEntityType.PixiePosse] = (Player player) => player.ZoneHallow;
			_secondarySpawnConditionsPerPlayer[SkyEntityType.Seagulls] = (Player player) => player.ZoneBeach;
			_secondarySpawnConditionsPerPlayer[SkyEntityType.Gastropods] = (Player player) => player.ZoneHallow;
			_secondarySpawnConditionsPerPlayer[SkyEntityType.Pegasus] = (Player player) => player.ZoneHallow;
			_secondarySpawnConditionsPerPlayer[SkyEntityType.EaterOfSouls] = (Player player) => player.ZoneCorrupt;
			_secondarySpawnConditionsPerPlayer[SkyEntityType.Crimera] = (Player player) => player.ZoneCrimson;
			_secondarySpawnConditionsPerPlayer[SkyEntityType.Bats] = (Player player) => player.ZoneJungle;
		}

		private bool IsPlayerAtRightHeightForType(SkyEntityType type, Player plr)
		{
			if (type == SkyEntityType.Hellbats)
			{
				return IsPlayerInAPlaceWhereTheyCanSeeAmbienceHell(plr);
			}
			return IsPlayerInAPlaceWhereTheyCanSeeAmbienceSky(plr);
		}

		public void Update()
		{
			SpawnForcedEntities();
			if (_updatesUntilNextAttempt > 0)
			{
				_updatesUntilNextAttempt -= Main.dayRate;
				return;
			}
			ResetSpawnTime();
			IEnumerable<SkyEntityType> source = from pair in _spawnConditions
				where pair.Value()
				select pair.Key;
			if (source.Count((SkyEntityType type) => true) == 0)
			{
				return;
			}
			FindPlayerThatCanSeeBackgroundAmbience(out var player);
			if (player == null)
			{
				return;
			}
			IEnumerable<SkyEntityType> source2 = source.Where((SkyEntityType type) => IsPlayerAtRightHeightForType(type, player) && _secondarySpawnConditionsPerPlayer.ContainsKey(type) && _secondarySpawnConditionsPerPlayer[type](player));
			int num = source2.Count((SkyEntityType type) => true);
			if (num == 0 || Main.rand.Next(5) < 3)
			{
				source2 = source.Where((SkyEntityType type) => IsPlayerAtRightHeightForType(type, player) && (!_secondarySpawnConditionsPerPlayer.ContainsKey(type) || _secondarySpawnConditionsPerPlayer[type](player)));
				num = source2.Count((SkyEntityType type) => true);
			}
			if (num != 0)
			{
				SkyEntityType type2 = source2.ElementAt(Main.rand.Next(num));
				SpawnForPlayer(player, type2);
			}
		}

		public void ResetSpawnTime()
		{
			_updatesUntilNextAttempt = Main.rand.Next(600, 7200);
			if (Main.tenthAnniversaryWorld)
			{
				_updatesUntilNextAttempt /= 2;
			}
		}

		public void ForceEntitySpawn(AmbienceSpawnInfo info)
		{
			_forcedSpawns.Add(info);
		}

		private void SpawnForcedEntities()
		{
			if (_forcedSpawns.Count == 0)
			{
				return;
			}
			for (int num = _forcedSpawns.Count - 1; num >= 0; num--)
			{
				AmbienceSpawnInfo ambienceSpawnInfo = _forcedSpawns[num];
				Player player;
				if (ambienceSpawnInfo.targetPlayer == -1)
				{
					FindPlayerThatCanSeeBackgroundAmbience(out player);
				}
				else
				{
					player = Main.player[ambienceSpawnInfo.targetPlayer];
				}
				if (player != null && IsPlayerAtRightHeightForType(ambienceSpawnInfo.skyEntityType, player))
				{
					SpawnForPlayer(player, ambienceSpawnInfo.skyEntityType);
				}
				_forcedSpawns.RemoveAt(num);
			}
		}

		private static void FindPlayerThatCanSeeBackgroundAmbience(out Player player)
		{
			player = null;
			int num = Main.player.Count((Player plr) => plr.active && IsPlayerInAPlaceWhereTheyCanSeeAmbience(plr));
			if (num != 0)
			{
				player = Main.player.Where((Player plr) => plr.active && IsPlayerInAPlaceWhereTheyCanSeeAmbience(plr)).ElementAt(Main.rand.Next(num));
			}
		}

		private static bool IsPlayerInAPlaceWhereTheyCanSeeAmbience(Player plr)
		{
			if (!IsPlayerInAPlaceWhereTheyCanSeeAmbienceSky(plr))
			{
				return IsPlayerInAPlaceWhereTheyCanSeeAmbienceHell(plr);
			}
			return true;
		}

		private static bool IsPlayerInAPlaceWhereTheyCanSeeAmbienceSky(Player plr)
		{
			return (double)plr.position.Y <= Main.worldSurface * 16.0 + 1600.0;
		}

		private static bool IsPlayerInAPlaceWhereTheyCanSeeAmbienceHell(Player plr)
		{
			return plr.position.Y >= (float)((Main.UnderworldLayer - 100) * 16);
		}

		private void SpawnForPlayer(Player player, SkyEntityType type)
		{
			NetManager.Instance.BroadcastOrLoopback(NetAmbienceModule.SerializeSkyEntitySpawn(player, type));
		}
	}
}
