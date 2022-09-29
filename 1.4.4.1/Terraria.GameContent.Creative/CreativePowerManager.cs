using System.Collections.Generic;
using System.IO;
using Terraria.GameContent.NetModules;
using Terraria.Net;

namespace Terraria.GameContent.Creative
{
	public class CreativePowerManager
	{
		private class PowerTypeStorage<T> where T : ICreativePower
		{
			public static ushort Id;

			public static string Name;

			public static T Power;
		}

		public static readonly CreativePowerManager Instance = new CreativePowerManager();

		private Dictionary<ushort, ICreativePower> _powersById = new Dictionary<ushort, ICreativePower>();

		private Dictionary<string, ICreativePower> _powersByName = new Dictionary<string, ICreativePower>();

		private ushort _powersCount;

		private static bool _initialized = false;

		private const string _powerPermissionsLineHeader = "journeypermission_";

		private CreativePowerManager()
		{
		}

		public void Register<T>(string nameInServerConfig) where T : ICreativePower, new()
		{
			T val = (PowerTypeStorage<T>.Power = new T());
			PowerTypeStorage<T>.Id = _powersCount;
			PowerTypeStorage<T>.Name = nameInServerConfig;
			val.DefaultPermissionLevel = PowerPermissionLevel.CanBeChangedByEveryone;
			val.CurrentPermissionLevel = PowerPermissionLevel.CanBeChangedByEveryone;
			_powersById[_powersCount] = val;
			_powersByName[nameInServerConfig] = val;
			val.PowerId = _powersCount;
			val.ServerConfigName = nameInServerConfig;
			_powersCount++;
		}

		public T GetPower<T>() where T : ICreativePower
		{
			return PowerTypeStorage<T>.Power;
		}

		public ushort GetPowerId<T>() where T : ICreativePower
		{
			return PowerTypeStorage<T>.Id;
		}

		public bool TryGetPower(ushort id, out ICreativePower power)
		{
			return _powersById.TryGetValue(id, out power);
		}

		public static void TryListingPermissionsFrom(string line)
		{
			int length = "journeypermission_".Length;
			if (line.Length < length || !line.ToLower().StartsWith("journeypermission_"))
			{
				return;
			}
			string[] array = line.Substring(length).Split(new char[1] { '=' });
			if (array.Length == 2 && int.TryParse(array[1].Trim(), out var result))
			{
				PowerPermissionLevel powerPermissionLevel = (PowerPermissionLevel)Utils.Clamp(result, 0, 2);
				string key = array[0].Trim().ToLower();
				Initialize();
				if (Instance._powersByName.TryGetValue(key, out var value))
				{
					value.DefaultPermissionLevel = powerPermissionLevel;
					value.CurrentPermissionLevel = powerPermissionLevel;
				}
			}
		}

		public static void Initialize()
		{
			if (!_initialized)
			{
				Instance.Register<CreativePowers.FreezeTime>("time_setfrozen");
				Instance.Register<CreativePowers.StartDayImmediately>("time_setdawn");
				Instance.Register<CreativePowers.StartNoonImmediately>("time_setnoon");
				Instance.Register<CreativePowers.StartNightImmediately>("time_setdusk");
				Instance.Register<CreativePowers.StartMidnightImmediately>("time_setmidnight");
				Instance.Register<CreativePowers.GodmodePower>("godmode");
				Instance.Register<CreativePowers.ModifyWindDirectionAndStrength>("wind_setstrength");
				Instance.Register<CreativePowers.ModifyRainPower>("rain_setstrength");
				Instance.Register<CreativePowers.ModifyTimeRate>("time_setspeed");
				Instance.Register<CreativePowers.FreezeRainPower>("rain_setfrozen");
				Instance.Register<CreativePowers.FreezeWindDirectionAndStrength>("wind_setfrozen");
				Instance.Register<CreativePowers.FarPlacementRangePower>("increaseplacementrange");
				Instance.Register<CreativePowers.DifficultySliderPower>("setdifficulty");
				Instance.Register<CreativePowers.StopBiomeSpreadPower>("biomespread_setfrozen");
				Instance.Register<CreativePowers.SpawnRateSliderPerPlayerPower>("setspawnrate");
				_initialized = true;
			}
		}

		public void Reset()
		{
			foreach (KeyValuePair<ushort, ICreativePower> item in _powersById)
			{
				item.Value.CurrentPermissionLevel = item.Value.DefaultPermissionLevel;
				if (item.Value is IPersistentPerWorldContent persistentPerWorldContent)
				{
					persistentPerWorldContent.Reset();
				}
				if (item.Value is IPersistentPerPlayerContent persistentPerPlayerContent)
				{
					persistentPerPlayerContent.Reset();
				}
			}
		}

		public void SaveToWorld(BinaryWriter writer)
		{
			foreach (KeyValuePair<ushort, ICreativePower> item in _powersById)
			{
				if (item.Value is IPersistentPerWorldContent persistentPerWorldContent)
				{
					writer.Write(value: true);
					writer.Write(item.Key);
					persistentPerWorldContent.Save(writer);
				}
			}
			writer.Write(value: false);
		}

		public void LoadFromWorld(BinaryReader reader, int versionGameWasLastSavedOn)
		{
			while (reader.ReadBoolean())
			{
				ushort key = reader.ReadUInt16();
				if (_powersById.TryGetValue(key, out var value) && value is IPersistentPerWorldContent persistentPerWorldContent)
				{
					persistentPerWorldContent.Load(reader, versionGameWasLastSavedOn);
					continue;
				}
				break;
			}
		}

		public void ValidateWorld(BinaryReader reader, int versionGameWasLastSavedOn)
		{
			while (reader.ReadBoolean())
			{
				ushort key = reader.ReadUInt16();
				if (_powersById.TryGetValue(key, out var value) && value is IPersistentPerWorldContent persistentPerWorldContent)
				{
					persistentPerWorldContent.ValidateWorld(reader, versionGameWasLastSavedOn);
					continue;
				}
				break;
			}
		}

		public void SyncThingsToJoiningPlayer(int playerIndex)
		{
			foreach (KeyValuePair<ushort, ICreativePower> item in _powersById)
			{
				NetPacket packet = NetCreativePowerPermissionsModule.SerializeCurrentPowerPermissionLevel(item.Key, (int)item.Value.CurrentPermissionLevel);
				NetManager.Instance.SendToClient(packet, playerIndex);
			}
			foreach (KeyValuePair<ushort, ICreativePower> item2 in _powersById)
			{
				if (item2.Value is IOnPlayerJoining onPlayerJoining)
				{
					onPlayerJoining.OnPlayerJoining(playerIndex);
				}
			}
		}

		public void SaveToPlayer(Player player, BinaryWriter writer)
		{
			foreach (KeyValuePair<ushort, ICreativePower> item in _powersById)
			{
				if (item.Value is IPersistentPerPlayerContent persistentPerPlayerContent)
				{
					writer.Write(value: true);
					writer.Write(item.Key);
					persistentPerPlayerContent.Save(player, writer);
				}
			}
			writer.Write(value: false);
		}

		public void LoadToPlayer(Player player, BinaryReader reader, int versionGameWasLastSavedOn)
		{
			while (reader.ReadBoolean())
			{
				ushort key = reader.ReadUInt16();
				if (!_powersById.TryGetValue(key, out var value))
				{
					break;
				}
				if (value is IPersistentPerPlayerContent persistentPerPlayerContent)
				{
					persistentPerPlayerContent.Load(player, reader, versionGameWasLastSavedOn);
				}
			}
			if (player.difficulty != 3)
			{
				ResetPowersForPlayer(player);
			}
		}

		public void ApplyLoadedDataToPlayer(Player player)
		{
			foreach (KeyValuePair<ushort, ICreativePower> item in _powersById)
			{
				if (item.Value is IPersistentPerPlayerContent persistentPerPlayerContent)
				{
					persistentPerPlayerContent.ApplyLoadedDataToOutOfPlayerFields(player);
				}
			}
		}

		public void ResetPowersForPlayer(Player player)
		{
			foreach (KeyValuePair<ushort, ICreativePower> item in _powersById)
			{
				if (item.Value is IPersistentPerPlayerContent persistentPerPlayerContent)
				{
					persistentPerPlayerContent.ResetDataForNewPlayer(player);
				}
			}
		}

		public void ResetDataForNewPlayer(Player player)
		{
			foreach (KeyValuePair<ushort, ICreativePower> item in _powersById)
			{
				if (item.Value is IPersistentPerPlayerContent persistentPerPlayerContent)
				{
					persistentPerPlayerContent.Reset();
					persistentPerPlayerContent.ResetDataForNewPlayer(player);
				}
			}
		}
	}
}
