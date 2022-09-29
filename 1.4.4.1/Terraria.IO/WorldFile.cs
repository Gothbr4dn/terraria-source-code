using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.IO
{
	public class WorldFile
	{
		public static class TilePacker
		{
			public const int Header1_1 = 1;

			public const int Header1_2 = 2;

			public const int Header1_4 = 4;

			public const int Header1_8 = 8;

			public const int Header1_10 = 16;

			public const int Header1_18 = 24;

			public const int Header1_20 = 32;

			public const int Header1_40 = 64;

			public const int Header1_80 = 128;

			public const int Header1_C0 = 192;

			public const int Header2_1 = 1;

			public const int Header2_2 = 2;

			public const int Header2_4 = 4;

			public const int Header2_8 = 8;

			public const int Header2_10 = 16;

			public const int Header2_20 = 32;

			public const int Header2_40 = 64;

			public const int Header2_70 = 112;

			public const int Header2_80 = 128;

			public const int Header3_1 = 1;

			public const int Header3_2 = 2;

			public const int Header3_4 = 4;

			public const int Header3_8 = 8;

			public const int Header3_10 = 16;

			public const int Header3_20 = 32;

			public const int Header3_40 = 64;

			public const int Header3_80 = 128;

			public const int Header4_1 = 1;

			public const int Header4_2 = 2;

			public const int Header4_4 = 4;

			public const int Header4_8 = 8;

			public const int Header4_10 = 16;

			public const int Header4_20 = 32;

			public const int Header4_40 = 64;

			public const int Header4_80 = 128;
		}

		private static readonly object IOLock = new object();

		private static double _tempTime = Main.time;

		private static bool _tempRaining;

		private static float _tempMaxRain;

		private static int _tempRainTime;

		private static bool _tempDayTime = Main.dayTime;

		private static bool _tempBloodMoon = Main.bloodMoon;

		private static bool _tempEclipse = Main.eclipse;

		private static int _tempMoonPhase = Main.moonPhase;

		private static int _tempCultistDelay = CultistRitual.delay;

		private static int _versionNumber;

		private static bool _isWorldOnCloud;

		private static bool _tempPartyGenuine;

		private static bool _tempPartyManual;

		private static int _tempPartyCooldown;

		private static readonly List<int> TempPartyCelebratingNPCs = new List<int>();

		private static bool _hasCache;

		private static bool? _cachedDayTime;

		private static double? _cachedTime;

		private static int? _cachedMoonPhase;

		private static bool? _cachedBloodMoon;

		private static bool? _cachedEclipse;

		private static int? _cachedCultistDelay;

		private static bool? _cachedPartyGenuine;

		private static bool? _cachedPartyManual;

		private static int? _cachedPartyDaysOnCooldown;

		private static readonly List<int> CachedCelebratingNPCs = new List<int>();

		private static bool? _cachedSandstormHappening;

		private static bool _tempSandstormHappening;

		private static int? _cachedSandstormTimeLeft;

		private static int _tempSandstormTimeLeft;

		private static float? _cachedSandstormSeverity;

		private static float _tempSandstormSeverity;

		private static float? _cachedSandstormIntendedSeverity;

		private static float _tempSandstormIntendedSeverity;

		private static bool _tempLanternNightGenuine;

		private static bool _tempLanternNightManual;

		private static bool _tempLanternNightNextNightIsGenuine;

		private static int _tempLanternNightCooldown;

		private static bool? _cachedLanternNightGenuine;

		private static bool? _cachedLanternNightManual;

		private static bool? _cachedLanternNightNextNightIsGenuine;

		private static int? _cachedLanternNightCooldown;

		public static Exception LastThrownLoadException;

		public static event Action OnWorldLoad;

		public static void CacheSaveTime()
		{
			_hasCache = true;
			_cachedDayTime = Main.dayTime;
			_cachedTime = Main.time;
			_cachedMoonPhase = Main.moonPhase;
			_cachedBloodMoon = Main.bloodMoon;
			_cachedEclipse = Main.eclipse;
			_cachedCultistDelay = CultistRitual.delay;
			_cachedPartyGenuine = BirthdayParty.GenuineParty;
			_cachedPartyManual = BirthdayParty.ManualParty;
			_cachedPartyDaysOnCooldown = BirthdayParty.PartyDaysOnCooldown;
			CachedCelebratingNPCs.Clear();
			CachedCelebratingNPCs.AddRange(BirthdayParty.CelebratingNPCs);
			_cachedSandstormHappening = Sandstorm.Happening;
			_cachedSandstormTimeLeft = Sandstorm.TimeLeft;
			_cachedSandstormSeverity = Sandstorm.Severity;
			_cachedSandstormIntendedSeverity = Sandstorm.IntendedSeverity;
			_cachedLanternNightCooldown = LanternNight.LanternNightsOnCooldown;
			_cachedLanternNightGenuine = LanternNight.GenuineLanterns;
			_cachedLanternNightManual = LanternNight.ManualLanterns;
			_cachedLanternNightNextNightIsGenuine = LanternNight.NextNightIsLanternNight;
		}

		public static void SetOngoingToTemps()
		{
			Main.dayTime = _tempDayTime;
			Main.time = _tempTime;
			Main.moonPhase = _tempMoonPhase;
			Main.bloodMoon = _tempBloodMoon;
			Main.eclipse = _tempEclipse;
			Main.raining = _tempRaining;
			Main.rainTime = _tempRainTime;
			Main.maxRaining = _tempMaxRain;
			Main.cloudAlpha = _tempMaxRain;
			CultistRitual.delay = _tempCultistDelay;
			BirthdayParty.ManualParty = _tempPartyManual;
			BirthdayParty.GenuineParty = _tempPartyGenuine;
			BirthdayParty.PartyDaysOnCooldown = _tempPartyCooldown;
			BirthdayParty.CelebratingNPCs.Clear();
			BirthdayParty.CelebratingNPCs.AddRange(TempPartyCelebratingNPCs);
			Sandstorm.Happening = _tempSandstormHappening;
			Sandstorm.TimeLeft = _tempSandstormTimeLeft;
			Sandstorm.Severity = _tempSandstormSeverity;
			Sandstorm.IntendedSeverity = _tempSandstormIntendedSeverity;
			LanternNight.GenuineLanterns = _tempLanternNightGenuine;
			LanternNight.LanternNightsOnCooldown = _tempLanternNightCooldown;
			LanternNight.ManualLanterns = _tempLanternNightManual;
			LanternNight.NextNightIsLanternNight = _tempLanternNightNextNightIsGenuine;
		}

		public static bool IsValidWorld(string file, bool cloudSave)
		{
			return GetFileMetadata(file, cloudSave) != null;
		}

		public static WorldFileData GetAllMetadata(string file, bool cloudSave)
		{
			if (file == null || (cloudSave && SocialAPI.Cloud == null))
			{
				return null;
			}
			WorldFileData worldFileData = new WorldFileData(file, cloudSave);
			if (!FileUtilities.Exists(file, cloudSave))
			{
				return WorldFileData.FromInvalidWorld(file, cloudSave);
			}
			try
			{
				using Stream stream = (cloudSave ? ((Stream)new MemoryStream(SocialAPI.Cloud.Read(file))) : ((Stream)new FileStream(file, FileMode.Open)));
				using BinaryReader binaryReader = new BinaryReader(stream);
				int num = binaryReader.ReadInt32();
				if (num >= 135)
				{
					worldFileData.Metadata = FileMetadata.Read(binaryReader, FileType.World);
				}
				else
				{
					worldFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
				}
				if (num <= 270)
				{
					binaryReader.ReadInt16();
					stream.Position = binaryReader.ReadInt32();
					worldFileData.Name = binaryReader.ReadString();
					if (num >= 179)
					{
						string seed = ((num != 179) ? binaryReader.ReadString() : binaryReader.ReadInt32().ToString());
						worldFileData.SetSeed(seed);
						worldFileData.WorldGeneratorVersion = binaryReader.ReadUInt64();
					}
					else
					{
						worldFileData.SetSeedToEmpty();
						worldFileData.WorldGeneratorVersion = 0uL;
					}
					if (num >= 181)
					{
						worldFileData.UniqueId = new Guid(binaryReader.ReadBytes(16));
					}
					else
					{
						worldFileData.UniqueId = Guid.Empty;
					}
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					int y = binaryReader.ReadInt32();
					int x = binaryReader.ReadInt32();
					worldFileData.SetWorldSize(x, y);
					if (num >= 209)
					{
						worldFileData.GameMode = binaryReader.ReadInt32();
						if (num >= 222)
						{
							worldFileData.DrunkWorld = binaryReader.ReadBoolean();
							if (num >= 227)
							{
								worldFileData.ForTheWorthy = binaryReader.ReadBoolean();
							}
							if (num >= 238)
							{
								worldFileData.Anniversary = binaryReader.ReadBoolean();
							}
							if (num >= 239)
							{
								worldFileData.DontStarve = binaryReader.ReadBoolean();
							}
							if (num >= 241)
							{
								worldFileData.NotTheBees = binaryReader.ReadBoolean();
							}
							if (num >= 249)
							{
								worldFileData.RemixWorld = binaryReader.ReadBoolean();
							}
							if (num >= 266)
							{
								worldFileData.NoTrapsWorld = binaryReader.ReadBoolean();
							}
							if (num >= 267)
							{
								worldFileData.ZenithWorld = binaryReader.ReadBoolean();
							}
							else
							{
								worldFileData.ZenithWorld = worldFileData.DrunkWorld && worldFileData.RemixWorld;
							}
						}
					}
					else if (num >= 112)
					{
						if (binaryReader.ReadBoolean())
						{
							worldFileData.GameMode = 1;
						}
						else
						{
							worldFileData.GameMode = 0;
						}
					}
					if (num >= 141)
					{
						worldFileData.CreationTime = DateTime.FromBinary(binaryReader.ReadInt64());
					}
					else if (!cloudSave)
					{
						worldFileData.CreationTime = File.GetCreationTime(file);
					}
					else
					{
						worldFileData.CreationTime = DateTime.Now;
					}
					binaryReader.ReadByte();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadDouble();
					binaryReader.ReadDouble();
					binaryReader.ReadDouble();
					binaryReader.ReadBoolean();
					binaryReader.ReadInt32();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					worldFileData.HasCrimson = binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					if (num >= 118)
					{
						binaryReader.ReadBoolean();
					}
					else
						_ = 0;
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadByte();
					binaryReader.ReadInt32();
					worldFileData.IsHardMode = binaryReader.ReadBoolean();
					if (num >= 257)
					{
						binaryReader.ReadBoolean();
					}
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadDouble();
					if (num >= 118)
					{
						binaryReader.ReadDouble();
					}
					if (num >= 113)
					{
						binaryReader.ReadByte();
					}
					binaryReader.ReadBoolean();
					binaryReader.ReadInt32();
					binaryReader.ReadSingle();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadByte();
					binaryReader.ReadByte();
					binaryReader.ReadByte();
					binaryReader.ReadByte();
					binaryReader.ReadByte();
					binaryReader.ReadByte();
					binaryReader.ReadByte();
					binaryReader.ReadByte();
					binaryReader.ReadInt32();
					binaryReader.ReadInt16();
					binaryReader.ReadSingle();
					if (num < 95)
					{
						return worldFileData;
					}
					for (int num2 = binaryReader.ReadInt32(); num2 > 0; num2--)
					{
						binaryReader.ReadString();
					}
					if (num < 99)
					{
						return worldFileData;
					}
					binaryReader.ReadBoolean();
					if (num < 101)
					{
						return worldFileData;
					}
					binaryReader.ReadInt32();
					if (num < 104)
					{
						return worldFileData;
					}
					binaryReader.ReadBoolean();
					if (num >= 129)
					{
						binaryReader.ReadBoolean();
					}
					if (num >= 201)
					{
						binaryReader.ReadBoolean();
					}
					if (num >= 107)
					{
						binaryReader.ReadInt32();
					}
					if (num >= 108)
					{
						binaryReader.ReadInt32();
					}
					if (num < 109)
					{
						return worldFileData;
					}
					int num3 = binaryReader.ReadInt16();
					for (int i = 0; i < num3; i++)
					{
						binaryReader.ReadInt32();
					}
					if (num < 128)
					{
						return worldFileData;
					}
					binaryReader.ReadBoolean();
					if (num < 131)
					{
						return worldFileData;
					}
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					binaryReader.ReadBoolean();
					worldFileData.DefeatedMoonlord = binaryReader.ReadBoolean();
					return worldFileData;
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		public static WorldFileData CreateMetadata(string name, bool cloudSave, int GameMode)
		{
			WorldFileData worldFileData = new WorldFileData(Main.GetWorldPathFromName(name, cloudSave), cloudSave);
			if (Main.autoGenFileLocation != null && Main.autoGenFileLocation != "")
			{
				worldFileData = new WorldFileData(Main.autoGenFileLocation, cloudSave);
				Main.autoGenFileLocation = null;
			}
			worldFileData.Name = name;
			worldFileData.GameMode = GameMode;
			worldFileData.CreationTime = DateTime.Now;
			worldFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.World);
			worldFileData.SetFavorite(favorite: false);
			worldFileData.WorldGeneratorVersion = 1159641169921uL;
			worldFileData.UniqueId = Guid.NewGuid();
			if (Main.DefaultSeed == "")
			{
				worldFileData.SetSeedToRandom();
			}
			else
			{
				worldFileData.SetSeed(Main.DefaultSeed);
			}
			return worldFileData;
		}

		public static void ResetTemps()
		{
			_tempRaining = false;
			_tempMaxRain = 0f;
			_tempRainTime = 0;
			_tempDayTime = true;
			_tempBloodMoon = false;
			_tempEclipse = false;
			_tempMoonPhase = 0;
			Main.anglerWhoFinishedToday.Clear();
			Main.anglerQuestFinished = false;
		}

		public static void ClearTempTiles()
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					if (Main.tile[i, j].type == 127 || Main.tile[i, j].type == 504)
					{
						WorldGen.KillTile(i, j);
					}
				}
			}
		}

		public static void LoadWorld(bool loadFromCloud)
		{
			Main.lockMenuBGChange = true;
			_isWorldOnCloud = loadFromCloud;
			Main.checkXMas();
			Main.checkHalloween();
			bool flag = loadFromCloud && SocialAPI.Cloud != null;
			if (!FileUtilities.Exists(Main.worldPathName, flag) && Main.autoGen)
			{
				if (!flag)
				{
					for (int num = Main.worldPathName.Length - 1; num >= 0; num--)
					{
						string text = Main.worldPathName.Substring(num, 1);
						char directorySeparatorChar = Path.DirectorySeparatorChar;
						if (text == (directorySeparatorChar.ToString() ?? ""))
						{
							Utils.TryCreatingDirectory(Main.worldPathName.Substring(0, num));
							break;
						}
					}
				}
				WorldGen.clearWorld();
				Main.ActiveWorldFileData = CreateMetadata((Main.worldName == "") ? "World" : Main.worldName, flag, Main.GameMode);
				string text2 = (Main.AutogenSeedName ?? "").Trim();
				if (text2.Length == 0)
				{
					Main.ActiveWorldFileData.SetSeedToRandom();
				}
				else
				{
					Main.ActiveWorldFileData.SetSeed(text2);
				}
				UIWorldCreation.ProcessSpecialWorldSeeds(text2);
				WorldGen.GenerateWorld(Main.ActiveWorldFileData.Seed, Main.AutogenProgress);
				SaveWorld();
			}
			try
			{
				using MemoryStream memoryStream = new MemoryStream(FileUtilities.ReadAllBytes(Main.worldPathName, flag));
				using BinaryReader binaryReader = new BinaryReader(memoryStream);
				try
				{
					WorldGen.loadFailed = false;
					WorldGen.loadSuccess = false;
					int num2 = (_versionNumber = binaryReader.ReadInt32());
					if (_versionNumber <= 0 || _versionNumber > 270)
					{
						WorldGen.loadFailed = true;
						return;
					}
					int num3 = ((num2 > 87) ? LoadWorld_Version2(binaryReader) : LoadWorld_Version1_Old_BeforeRelease88(binaryReader));
					if (num2 < 141)
					{
						if (!loadFromCloud)
						{
							Main.ActiveWorldFileData.CreationTime = File.GetCreationTime(Main.worldPathName);
						}
						else
						{
							Main.ActiveWorldFileData.CreationTime = DateTime.Now;
						}
					}
					CheckSavedOreTiers();
					binaryReader.Close();
					memoryStream.Close();
					if (num3 != 0)
					{
						WorldGen.loadFailed = true;
					}
					else
					{
						WorldGen.loadSuccess = true;
					}
					if (WorldGen.loadFailed || !WorldGen.loadSuccess)
					{
						return;
					}
					ConvertOldTileEntities();
					ClearTempTiles();
					WorldGen.gen = true;
					GenVars.waterLine = Main.maxTilesY;
					Liquid.QuickWater(2);
					WorldGen.WaterCheck();
					int num4 = 0;
					Liquid.quickSettle = true;
					int num5 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
					float num6 = 0f;
					while (Liquid.numLiquid > 0 && num4 < 100000)
					{
						num4++;
						float num7 = (float)(num5 - (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer)) / (float)num5;
						if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num5)
						{
							num5 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
						}
						if (num7 > num6)
						{
							num6 = num7;
						}
						else
						{
							num7 = num6;
						}
						Main.statusText = Lang.gen[27].Value + " " + (int)(num7 * 100f / 2f + 50f) + "%";
						Liquid.UpdateLiquid();
					}
					Liquid.quickSettle = false;
					Main.weatherCounter = WorldGen.genRand.Next(3600, 18000);
					Cloud.resetClouds();
					WorldGen.WaterCheck();
					WorldGen.gen = false;
					NPC.setFireFlyChance();
					if (Main.slimeRainTime > 0.0)
					{
						Main.StartSlimeRain(announce: false);
					}
					NPC.SetWorldSpecificMonstersByWorldID();
				}
				catch (Exception lastThrownLoadException)
				{
					LastThrownLoadException = lastThrownLoadException;
					WorldGen.loadFailed = true;
					WorldGen.loadSuccess = false;
					try
					{
						binaryReader.Close();
						memoryStream.Close();
						return;
					}
					catch
					{
						return;
					}
				}
			}
			catch (Exception lastThrownLoadException2)
			{
				LastThrownLoadException = lastThrownLoadException2;
				WorldGen.loadFailed = true;
				WorldGen.loadSuccess = false;
				return;
			}
			if (WorldFile.OnWorldLoad != null)
			{
				WorldFile.OnWorldLoad();
			}
		}

		public static void CheckSavedOreTiers()
		{
			if (WorldGen.SavedOreTiers.Copper != -1 && WorldGen.SavedOreTiers.Iron != -1 && WorldGen.SavedOreTiers.Silver != -1 && WorldGen.SavedOreTiers.Gold != -1)
			{
				return;
			}
			int[] array = WorldGen.CountTileTypesInWorld(7, 166, 6, 167, 9, 168, 8, 169);
			for (int i = 0; i < array.Length; i += 2)
			{
				int num = array[i];
				int num2 = array[i + 1];
				switch (i)
				{
				case 0:
					if (num > num2)
					{
						WorldGen.SavedOreTiers.Copper = 7;
					}
					else
					{
						WorldGen.SavedOreTiers.Copper = 166;
					}
					break;
				case 2:
					if (num > num2)
					{
						WorldGen.SavedOreTiers.Iron = 6;
					}
					else
					{
						WorldGen.SavedOreTiers.Iron = 167;
					}
					break;
				case 4:
					if (num > num2)
					{
						WorldGen.SavedOreTiers.Silver = 9;
					}
					else
					{
						WorldGen.SavedOreTiers.Silver = 168;
					}
					break;
				case 6:
					if (num > num2)
					{
						WorldGen.SavedOreTiers.Gold = 8;
					}
					else
					{
						WorldGen.SavedOreTiers.Gold = 169;
					}
					break;
				}
			}
		}

		public static void SaveWorld()
		{
			try
			{
				SaveWorld(_isWorldOnCloud);
			}
			catch (Exception exception)
			{
				FancyErrorPrinter.ShowFileSavingFailError(exception, Main.WorldPath);
				throw;
			}
		}

		public static void SaveWorld(bool useCloudSaving, bool resetTime = false)
		{
			if (useCloudSaving && SocialAPI.Cloud == null)
			{
				return;
			}
			if (Main.worldName == "")
			{
				Main.worldName = "World";
			}
			while (WorldGen.IsGeneratingHardMode)
			{
				Main.statusText = Lang.gen[48].Value;
			}
			if (!Monitor.TryEnter(IOLock))
			{
				return;
			}
			try
			{
				FileUtilities.ProtectedInvoke(delegate
				{
					InternalSaveWorld(useCloudSaving, resetTime);
				});
			}
			finally
			{
				Monitor.Exit(IOLock);
			}
		}

		private static void InternalSaveWorld(bool useCloudSaving, bool resetTime)
		{
			Utils.TryCreatingDirectory(Main.WorldPath);
			if (Main.skipMenu)
			{
				return;
			}
			if (_hasCache)
			{
				SetTempToCache();
			}
			else
			{
				SetTempToOngoing();
			}
			if (resetTime)
			{
				ResetTempsToDayTime();
			}
			if (Main.worldPathName == null)
			{
				return;
			}
			new Stopwatch().Start();
			int num;
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream(7000000))
			{
				using (BinaryWriter writer = new BinaryWriter(memoryStream))
				{
					SaveWorld_Version2(writer);
				}
				array = memoryStream.ToArray();
				num = array.Length;
			}
			byte[] array2 = null;
			if (FileUtilities.Exists(Main.worldPathName, useCloudSaving))
			{
				array2 = FileUtilities.ReadAllBytes(Main.worldPathName, useCloudSaving);
			}
			FileUtilities.Write(Main.worldPathName, array, num, useCloudSaving);
			array = FileUtilities.ReadAllBytes(Main.worldPathName, useCloudSaving);
			string text = null;
			using (MemoryStream input = new MemoryStream(array, 0, num, writable: false))
			{
				using BinaryReader fileIO = new BinaryReader(input);
				if (!Main.validateSaves || ValidateWorld(fileIO))
				{
					if (array2 != null)
					{
						text = Main.worldPathName + ".bak";
						Main.statusText = Lang.gen[50].Value;
					}
					DoRollingBackups(text);
				}
				else
				{
					text = Main.worldPathName;
				}
			}
			if (text != null && array2 != null)
			{
				FileUtilities.WriteAllBytes(text, array2, useCloudSaving);
			}
		}

		private static void DoRollingBackups(string backupWorldWritePath)
		{
			if (Main.WorldRollingBackupsCountToKeep <= 1)
			{
				return;
			}
			int num = Main.WorldRollingBackupsCountToKeep;
			if (num > 9)
			{
				num = 9;
			}
			int num2 = 1;
			for (int i = 1; i < num; i++)
			{
				string path = backupWorldWritePath + i;
				if (i == 1)
				{
					path = backupWorldWritePath;
				}
				if (!FileUtilities.Exists(path, cloud: false))
				{
					break;
				}
				num2 = i + 1;
			}
			for (int num3 = num2 - 1; num3 > 0; num3--)
			{
				string text = backupWorldWritePath + num3;
				if (num3 == 1)
				{
					text = backupWorldWritePath;
				}
				string destination = backupWorldWritePath + (num3 + 1);
				if (FileUtilities.Exists(text, cloud: false))
				{
					FileUtilities.Move(text, destination, cloud: false, overwrite: true, forceDeleteSourceFile: true);
				}
			}
		}

		private static void ResetTempsToDayTime()
		{
			_tempDayTime = true;
			_tempTime = 13500.0;
			_tempMoonPhase = 0;
			_tempBloodMoon = false;
			_tempEclipse = false;
			_tempCultistDelay = 86400;
			_tempPartyManual = false;
			_tempPartyGenuine = false;
			if (Main.tenthAnniversaryWorld)
			{
				_tempPartyGenuine = true;
			}
			_tempPartyCooldown = 0;
			TempPartyCelebratingNPCs.Clear();
			_tempSandstormHappening = false;
			_tempSandstormTimeLeft = 0;
			_tempSandstormSeverity = 0f;
			_tempSandstormIntendedSeverity = 0f;
			_tempLanternNightCooldown = 0;
			_tempLanternNightGenuine = false;
			_tempLanternNightManual = false;
			_tempLanternNightNextNightIsGenuine = false;
		}

		private static void SetTempToOngoing()
		{
			_tempDayTime = Main.dayTime;
			_tempTime = Main.time;
			_tempMoonPhase = Main.moonPhase;
			_tempBloodMoon = Main.bloodMoon;
			_tempEclipse = Main.eclipse;
			_tempCultistDelay = CultistRitual.delay;
			_tempPartyManual = BirthdayParty.ManualParty;
			_tempPartyGenuine = BirthdayParty.GenuineParty;
			_tempPartyCooldown = BirthdayParty.PartyDaysOnCooldown;
			TempPartyCelebratingNPCs.Clear();
			TempPartyCelebratingNPCs.AddRange(BirthdayParty.CelebratingNPCs);
			_tempSandstormHappening = Sandstorm.Happening;
			_tempSandstormTimeLeft = Sandstorm.TimeLeft;
			_tempSandstormSeverity = Sandstorm.Severity;
			_tempSandstormIntendedSeverity = Sandstorm.IntendedSeverity;
			_tempRaining = Main.raining;
			_tempRainTime = Main.rainTime;
			_tempMaxRain = Main.maxRaining;
			_tempLanternNightCooldown = LanternNight.LanternNightsOnCooldown;
			_tempLanternNightGenuine = LanternNight.GenuineLanterns;
			_tempLanternNightManual = LanternNight.ManualLanterns;
			_tempLanternNightNextNightIsGenuine = LanternNight.NextNightIsLanternNight;
		}

		private static void SetTempToCache()
		{
			_hasCache = false;
			_tempDayTime = _cachedDayTime.Value;
			_tempTime = _cachedTime.Value;
			_tempMoonPhase = _cachedMoonPhase.Value;
			_tempBloodMoon = _cachedBloodMoon.Value;
			_tempEclipse = _cachedEclipse.Value;
			_tempCultistDelay = _cachedCultistDelay.Value;
			_tempPartyManual = _cachedPartyManual.Value;
			_tempPartyGenuine = _cachedPartyGenuine.Value;
			_tempPartyCooldown = _cachedPartyDaysOnCooldown.Value;
			TempPartyCelebratingNPCs.Clear();
			TempPartyCelebratingNPCs.AddRange(CachedCelebratingNPCs);
			_tempSandstormHappening = _cachedSandstormHappening.Value;
			_tempSandstormTimeLeft = _cachedSandstormTimeLeft.Value;
			_tempSandstormSeverity = _cachedSandstormSeverity.Value;
			_tempSandstormIntendedSeverity = _cachedSandstormIntendedSeverity.Value;
			_tempRaining = Main.raining;
			_tempRainTime = Main.rainTime;
			_tempMaxRain = Main.maxRaining;
			_tempLanternNightCooldown = _cachedLanternNightCooldown.Value;
			_tempLanternNightGenuine = _cachedLanternNightGenuine.Value;
			_tempLanternNightManual = _cachedLanternNightManual.Value;
			_tempLanternNightNextNightIsGenuine = _cachedLanternNightNextNightIsGenuine.Value;
		}

		private static void ConvertOldTileEntities()
		{
			List<Point> list = new List<Point>();
			List<Point> list2 = new List<Point>();
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Main.tile[i, j];
					if ((tile.type == 128 || tile.type == 269) && tile.frameY == 0 && (tile.frameX % 100 == 0 || tile.frameX % 100 == 36))
					{
						list.Add(new Point(i, j));
					}
					if (tile.type == 334 && tile.frameY == 0 && tile.frameX % 54 == 0)
					{
						list2.Add(new Point(i, j));
					}
					if (tile.type == 49 && (tile.frameX == -1 || tile.frameY == -1))
					{
						tile.frameX = 0;
						tile.frameY = 0;
					}
				}
			}
			foreach (Point item in list)
			{
				if (!WorldGen.InWorld(item.X, item.Y, 5))
				{
					continue;
				}
				int frameX = Main.tile[item.X, item.Y].frameX;
				int frameX2 = Main.tile[item.X, item.Y + 1].frameX;
				int frameX3 = Main.tile[item.X, item.Y + 2].frameX;
				for (int k = 0; k < 2; k++)
				{
					for (int l = 0; l < 3; l++)
					{
						Tile tile2 = Main.tile[item.X + k, item.Y + l];
						tile2.frameX %= 100;
						if (tile2.type == 269)
						{
							tile2.frameX += 72;
						}
						tile2.type = 470;
					}
				}
				int num = TEDisplayDoll.Place(item.X, item.Y);
				if (num != -1)
				{
					(TileEntity.ByID[num] as TEDisplayDoll).SetInventoryFromMannequin(frameX, frameX2, frameX3);
				}
			}
			foreach (Point item2 in list2)
			{
				if (!WorldGen.InWorld(item2.X, item2.Y, 5))
				{
					continue;
				}
				bool flag = Main.tile[item2.X, item2.Y].frameX >= 54;
				short frameX4 = Main.tile[item2.X, item2.Y + 1].frameX;
				int frameX5 = Main.tile[item2.X + 1, item2.Y + 1].frameX;
				bool flag2 = frameX4 >= 5000;
				int num2 = frameX4 % 5000;
				num2 -= 100;
				int prefix = frameX5 - ((frameX5 >= 25000) ? 25000 : 10000);
				for (int m = 0; m < 3; m++)
				{
					for (int n = 0; n < 3; n++)
					{
						Tile tile3 = Main.tile[item2.X + m, item2.Y + n];
						tile3.type = 471;
						tile3.frameX = (short)((flag ? 54 : 0) + m * 18);
						tile3.frameY = (short)(n * 18);
					}
				}
				if (TEWeaponsRack.Place(item2.X, item2.Y) != -1 && flag2)
				{
					TEWeaponsRack.TryPlacing(item2.X, item2.Y, num2, prefix, 1);
				}
			}
		}

		public static void SaveWorld_Version2(BinaryWriter writer)
		{
			int[] pointers = new int[11]
			{
				SaveFileFormatHeader(writer),
				SaveWorldHeader(writer),
				SaveWorldTiles(writer),
				SaveChests(writer),
				SaveSigns(writer),
				SaveNPCs(writer),
				SaveTileEntities(writer),
				SaveWeightedPressurePlates(writer),
				SaveTownManager(writer),
				SaveBestiary(writer),
				SaveCreativePowers(writer)
			};
			SaveFooter(writer);
			SaveHeaderPointers(writer, pointers);
		}

		public static int SaveFileFormatHeader(BinaryWriter writer)
		{
			short num = 693;
			short num2 = 11;
			writer.Write(270);
			Main.WorldFileMetadata.IncrementAndWrite(writer);
			writer.Write(num2);
			for (int i = 0; i < num2; i++)
			{
				writer.Write(0);
			}
			writer.Write(num);
			byte b = 0;
			byte b2 = 1;
			for (int i = 0; i < num; i++)
			{
				if (Main.tileFrameImportant[i])
				{
					b = (byte)(b | b2);
				}
				if (b2 == 128)
				{
					writer.Write(b);
					b = 0;
					b2 = 1;
				}
				else
				{
					b2 = (byte)(b2 << 1);
				}
			}
			if (b2 != 1)
			{
				writer.Write(b);
			}
			return (int)writer.BaseStream.Position;
		}

		public static int SaveHeaderPointers(BinaryWriter writer, int[] pointers)
		{
			writer.BaseStream.Position = 0L;
			writer.Write(270);
			writer.BaseStream.Position += 20L;
			writer.Write((short)pointers.Length);
			for (int i = 0; i < pointers.Length; i++)
			{
				writer.Write(pointers[i]);
			}
			return (int)writer.BaseStream.Position;
		}

		public static int SaveWorldHeader(BinaryWriter writer)
		{
			writer.Write(Main.worldName);
			writer.Write(Main.ActiveWorldFileData.SeedText);
			writer.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
			writer.Write(Main.ActiveWorldFileData.UniqueId.ToByteArray());
			writer.Write(Main.worldID);
			writer.Write((int)Main.leftWorld);
			writer.Write((int)Main.rightWorld);
			writer.Write((int)Main.topWorld);
			writer.Write((int)Main.bottomWorld);
			writer.Write(Main.maxTilesY);
			writer.Write(Main.maxTilesX);
			writer.Write(Main.GameMode);
			writer.Write(Main.drunkWorld);
			writer.Write(Main.getGoodWorld);
			writer.Write(Main.tenthAnniversaryWorld);
			writer.Write(Main.dontStarveWorld);
			writer.Write(Main.notTheBeesWorld);
			writer.Write(Main.remixWorld);
			writer.Write(Main.noTrapsWorld);
			writer.Write(Main.zenithWorld);
			writer.Write(Main.ActiveWorldFileData.CreationTime.ToBinary());
			writer.Write((byte)Main.moonType);
			writer.Write(Main.treeX[0]);
			writer.Write(Main.treeX[1]);
			writer.Write(Main.treeX[2]);
			writer.Write(Main.treeStyle[0]);
			writer.Write(Main.treeStyle[1]);
			writer.Write(Main.treeStyle[2]);
			writer.Write(Main.treeStyle[3]);
			writer.Write(Main.caveBackX[0]);
			writer.Write(Main.caveBackX[1]);
			writer.Write(Main.caveBackX[2]);
			writer.Write(Main.caveBackStyle[0]);
			writer.Write(Main.caveBackStyle[1]);
			writer.Write(Main.caveBackStyle[2]);
			writer.Write(Main.caveBackStyle[3]);
			writer.Write(Main.iceBackStyle);
			writer.Write(Main.jungleBackStyle);
			writer.Write(Main.hellBackStyle);
			writer.Write(Main.spawnTileX);
			writer.Write(Main.spawnTileY);
			writer.Write(Main.worldSurface);
			writer.Write(Main.rockLayer);
			writer.Write(_tempTime);
			writer.Write(_tempDayTime);
			writer.Write(_tempMoonPhase);
			writer.Write(_tempBloodMoon);
			writer.Write(_tempEclipse);
			writer.Write(Main.dungeonX);
			writer.Write(Main.dungeonY);
			writer.Write(WorldGen.crimson);
			writer.Write(NPC.downedBoss1);
			writer.Write(NPC.downedBoss2);
			writer.Write(NPC.downedBoss3);
			writer.Write(NPC.downedQueenBee);
			writer.Write(NPC.downedMechBoss1);
			writer.Write(NPC.downedMechBoss2);
			writer.Write(NPC.downedMechBoss3);
			writer.Write(NPC.downedMechBossAny);
			writer.Write(NPC.downedPlantBoss);
			writer.Write(NPC.downedGolemBoss);
			writer.Write(NPC.downedSlimeKing);
			writer.Write(NPC.savedGoblin);
			writer.Write(NPC.savedWizard);
			writer.Write(NPC.savedMech);
			writer.Write(NPC.downedGoblins);
			writer.Write(NPC.downedClown);
			writer.Write(NPC.downedFrost);
			writer.Write(NPC.downedPirates);
			writer.Write(WorldGen.shadowOrbSmashed);
			writer.Write(WorldGen.spawnMeteor);
			writer.Write((byte)WorldGen.shadowOrbCount);
			writer.Write(WorldGen.altarCount);
			writer.Write(Main.hardMode);
			writer.Write(Main.afterPartyOfDoom);
			writer.Write(Main.invasionDelay);
			writer.Write(Main.invasionSize);
			writer.Write(Main.invasionType);
			writer.Write(Main.invasionX);
			writer.Write(Main.slimeRainTime);
			writer.Write((byte)Main.sundialCooldown);
			writer.Write(_tempRaining);
			writer.Write(_tempRainTime);
			writer.Write(_tempMaxRain);
			writer.Write(WorldGen.SavedOreTiers.Cobalt);
			writer.Write(WorldGen.SavedOreTiers.Mythril);
			writer.Write(WorldGen.SavedOreTiers.Adamantite);
			writer.Write((byte)WorldGen.treeBG1);
			writer.Write((byte)WorldGen.corruptBG);
			writer.Write((byte)WorldGen.jungleBG);
			writer.Write((byte)WorldGen.snowBG);
			writer.Write((byte)WorldGen.hallowBG);
			writer.Write((byte)WorldGen.crimsonBG);
			writer.Write((byte)WorldGen.desertBG);
			writer.Write((byte)WorldGen.oceanBG);
			writer.Write((int)Main.cloudBGActive);
			writer.Write((short)Main.numClouds);
			writer.Write(Main.windSpeedTarget);
			writer.Write(Main.anglerWhoFinishedToday.Count);
			for (int i = 0; i < Main.anglerWhoFinishedToday.Count; i++)
			{
				writer.Write(Main.anglerWhoFinishedToday[i]);
			}
			writer.Write(NPC.savedAngler);
			writer.Write(Main.anglerQuest);
			writer.Write(NPC.savedStylist);
			writer.Write(NPC.savedTaxCollector);
			writer.Write(NPC.savedGolfer);
			writer.Write(Main.invasionSizeStart);
			writer.Write(_tempCultistDelay);
			writer.Write((short)688);
			for (int j = 0; j < 688; j++)
			{
				writer.Write(NPC.killCount[j]);
			}
			writer.Write(Main.fastForwardTimeToDawn);
			writer.Write(NPC.downedFishron);
			writer.Write(NPC.downedMartians);
			writer.Write(NPC.downedAncientCultist);
			writer.Write(NPC.downedMoonlord);
			writer.Write(NPC.downedHalloweenKing);
			writer.Write(NPC.downedHalloweenTree);
			writer.Write(NPC.downedChristmasIceQueen);
			writer.Write(NPC.downedChristmasSantank);
			writer.Write(NPC.downedChristmasTree);
			writer.Write(NPC.downedTowerSolar);
			writer.Write(NPC.downedTowerVortex);
			writer.Write(NPC.downedTowerNebula);
			writer.Write(NPC.downedTowerStardust);
			writer.Write(NPC.TowerActiveSolar);
			writer.Write(NPC.TowerActiveVortex);
			writer.Write(NPC.TowerActiveNebula);
			writer.Write(NPC.TowerActiveStardust);
			writer.Write(NPC.LunarApocalypseIsUp);
			writer.Write(_tempPartyManual);
			writer.Write(_tempPartyGenuine);
			writer.Write(_tempPartyCooldown);
			writer.Write(TempPartyCelebratingNPCs.Count);
			for (int k = 0; k < TempPartyCelebratingNPCs.Count; k++)
			{
				writer.Write(TempPartyCelebratingNPCs[k]);
			}
			writer.Write(_tempSandstormHappening);
			writer.Write(_tempSandstormTimeLeft);
			writer.Write(_tempSandstormSeverity);
			writer.Write(_tempSandstormIntendedSeverity);
			writer.Write(NPC.savedBartender);
			DD2Event.Save(writer);
			writer.Write((byte)WorldGen.mushroomBG);
			writer.Write((byte)WorldGen.underworldBG);
			writer.Write((byte)WorldGen.treeBG2);
			writer.Write((byte)WorldGen.treeBG3);
			writer.Write((byte)WorldGen.treeBG4);
			writer.Write(NPC.combatBookWasUsed);
			writer.Write(_tempLanternNightCooldown);
			writer.Write(_tempLanternNightGenuine);
			writer.Write(_tempLanternNightManual);
			writer.Write(_tempLanternNightNextNightIsGenuine);
			WorldGen.TreeTops.Save(writer);
			writer.Write(Main.forceHalloweenForToday);
			writer.Write(Main.forceXMasForToday);
			writer.Write(WorldGen.SavedOreTiers.Copper);
			writer.Write(WorldGen.SavedOreTiers.Iron);
			writer.Write(WorldGen.SavedOreTiers.Silver);
			writer.Write(WorldGen.SavedOreTiers.Gold);
			writer.Write(NPC.boughtCat);
			writer.Write(NPC.boughtDog);
			writer.Write(NPC.boughtBunny);
			writer.Write(NPC.downedEmpressOfLight);
			writer.Write(NPC.downedQueenSlime);
			writer.Write(NPC.downedDeerclops);
			writer.Write(NPC.unlockedSlimeBlueSpawn);
			writer.Write(NPC.unlockedMerchantSpawn);
			writer.Write(NPC.unlockedDemolitionistSpawn);
			writer.Write(NPC.unlockedPartyGirlSpawn);
			writer.Write(NPC.unlockedDyeTraderSpawn);
			writer.Write(NPC.unlockedTruffleSpawn);
			writer.Write(NPC.unlockedArmsDealerSpawn);
			writer.Write(NPC.unlockedNurseSpawn);
			writer.Write(NPC.unlockedPrincessSpawn);
			writer.Write(NPC.combatBookVolumeTwoWasUsed);
			writer.Write(NPC.peddlersSatchelWasUsed);
			writer.Write(NPC.unlockedSlimeGreenSpawn);
			writer.Write(NPC.unlockedSlimeOldSpawn);
			writer.Write(NPC.unlockedSlimePurpleSpawn);
			writer.Write(NPC.unlockedSlimeRainbowSpawn);
			writer.Write(NPC.unlockedSlimeRedSpawn);
			writer.Write(NPC.unlockedSlimeYellowSpawn);
			writer.Write(NPC.unlockedSlimeCopperSpawn);
			writer.Write(Main.fastForwardTimeToDusk);
			writer.Write((byte)Main.moondialCooldown);
			return (int)writer.BaseStream.Position;
		}

		public static int SaveWorldTiles(BinaryWriter writer)
		{
			byte[] array = new byte[16];
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				float num = (float)i / (float)Main.maxTilesX;
				Main.statusText = Lang.gen[49].Value + " " + (int)(num * 100f + 1f) + "%";
				int num2;
				for (num2 = 0; num2 < Main.maxTilesY; num2++)
				{
					Tile tile = Main.tile[i, num2];
					int num3 = 4;
					byte b2;
					byte b;
					byte b3;
					byte b4 = (b3 = (b2 = (b = 0)));
					bool flag = false;
					if (tile.active())
					{
						flag = true;
					}
					if (flag)
					{
						b4 = (byte)(b4 | 2u);
						array[num3] = (byte)tile.type;
						num3++;
						if (tile.type > 255)
						{
							array[num3] = (byte)(tile.type >> 8);
							num3++;
							b4 = (byte)(b4 | 0x20u);
						}
						if (Main.tileFrameImportant[tile.type])
						{
							array[num3] = (byte)((uint)tile.frameX & 0xFFu);
							num3++;
							array[num3] = (byte)((tile.frameX & 0xFF00) >> 8);
							num3++;
							array[num3] = (byte)((uint)tile.frameY & 0xFFu);
							num3++;
							array[num3] = (byte)((tile.frameY & 0xFF00) >> 8);
							num3++;
						}
						if (tile.color() != 0)
						{
							b2 = (byte)(b2 | 8u);
							array[num3] = tile.color();
							num3++;
						}
					}
					if (tile.wall != 0)
					{
						b4 = (byte)(b4 | 4u);
						array[num3] = (byte)tile.wall;
						num3++;
						if (tile.wallColor() != 0)
						{
							b2 = (byte)(b2 | 0x10u);
							array[num3] = tile.wallColor();
							num3++;
						}
					}
					if (tile.liquid != 0)
					{
						if (!tile.shimmer())
						{
							b4 = (tile.lava() ? ((byte)(b4 | 0x10u)) : ((!tile.honey()) ? ((byte)(b4 | 8u)) : ((byte)(b4 | 0x18u))));
						}
						else
						{
							b2 = (byte)(b2 | 0x80u);
							b4 = (byte)(b4 | 8u);
						}
						array[num3] = tile.liquid;
						num3++;
					}
					if (tile.wire())
					{
						b3 = (byte)(b3 | 2u);
					}
					if (tile.wire2())
					{
						b3 = (byte)(b3 | 4u);
					}
					if (tile.wire3())
					{
						b3 = (byte)(b3 | 8u);
					}
					int num4 = (tile.halfBrick() ? 16 : ((tile.slope() != 0) ? (tile.slope() + 1 << 4) : 0));
					b3 = (byte)(b3 | (byte)num4);
					if (tile.actuator())
					{
						b2 = (byte)(b2 | 2u);
					}
					if (tile.inActive())
					{
						b2 = (byte)(b2 | 4u);
					}
					if (tile.wire4())
					{
						b2 = (byte)(b2 | 0x20u);
					}
					if (tile.wall > 255)
					{
						array[num3] = (byte)(tile.wall >> 8);
						num3++;
						b2 = (byte)(b2 | 0x40u);
					}
					if (tile.invisibleBlock())
					{
						b = (byte)(b | 2u);
					}
					if (tile.invisibleWall())
					{
						b = (byte)(b | 4u);
					}
					if (tile.fullbrightBlock())
					{
						b = (byte)(b | 8u);
					}
					if (tile.fullbrightWall())
					{
						b = (byte)(b | 0x10u);
					}
					int num5 = 3;
					if (b != 0)
					{
						b2 = (byte)(b2 | 1u);
						array[num5] = b;
						num5--;
					}
					if (b2 != 0)
					{
						b3 = (byte)(b3 | 1u);
						array[num5] = b2;
						num5--;
					}
					if (b3 != 0)
					{
						b4 = (byte)(b4 | 1u);
						array[num5] = b3;
						num5--;
					}
					short num6 = 0;
					int num7 = num2 + 1;
					int num8 = Main.maxTilesY - num2 - 1;
					while (num8 > 0 && tile.isTheSameAs(Main.tile[i, num7]) && TileID.Sets.AllowsSaveCompressionBatching[tile.type])
					{
						num6 = (short)(num6 + 1);
						num8--;
						num7++;
					}
					num2 += num6;
					if (num6 > 0)
					{
						array[num3] = (byte)((uint)num6 & 0xFFu);
						num3++;
						if (num6 > 255)
						{
							b4 = (byte)(b4 | 0x80u);
							array[num3] = (byte)((num6 & 0xFF00) >> 8);
							num3++;
						}
						else
						{
							b4 = (byte)(b4 | 0x40u);
						}
					}
					array[num5] = b4;
					writer.Write(array, num5, num3 - num5);
				}
			}
			return (int)writer.BaseStream.Position;
		}

		public static int SaveChests(BinaryWriter writer)
		{
			short num = 0;
			for (int i = 0; i < 8000; i++)
			{
				Chest chest = Main.chest[i];
				if (chest == null)
				{
					continue;
				}
				bool flag = false;
				for (int j = chest.x; j <= chest.x + 1; j++)
				{
					for (int k = chest.y; k <= chest.y + 1; k++)
					{
						if (j < 0 || k < 0 || j >= Main.maxTilesX || k >= Main.maxTilesY)
						{
							flag = true;
							break;
						}
						Tile tile = Main.tile[j, k];
						if (!tile.active() || !Main.tileContainer[tile.type])
						{
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					Main.chest[i] = null;
				}
				else
				{
					num = (short)(num + 1);
				}
			}
			writer.Write(num);
			writer.Write((short)40);
			for (int i = 0; i < 8000; i++)
			{
				Chest chest = Main.chest[i];
				if (chest == null)
				{
					continue;
				}
				writer.Write(chest.x);
				writer.Write(chest.y);
				writer.Write(chest.name);
				for (int l = 0; l < 40; l++)
				{
					Item item = chest.item[l];
					if (item == null || ItemID.Sets.ItemsThatShouldNotBeInInventory[item.type])
					{
						writer.Write((short)0);
						continue;
					}
					if (item.stack < 0)
					{
						item.stack = 1;
					}
					writer.Write((short)item.stack);
					if (item.stack > 0)
					{
						writer.Write(item.netID);
						writer.Write(item.prefix);
					}
				}
			}
			return (int)writer.BaseStream.Position;
		}

		public static int SaveSigns(BinaryWriter writer)
		{
			short num = 0;
			for (int i = 0; i < 1000; i++)
			{
				Sign sign = Main.sign[i];
				if (sign != null && sign.text != null)
				{
					num = (short)(num + 1);
				}
			}
			writer.Write(num);
			for (int j = 0; j < 1000; j++)
			{
				Sign sign = Main.sign[j];
				if (sign != null && sign.text != null)
				{
					writer.Write(sign.text);
					writer.Write(sign.x);
					writer.Write(sign.y);
				}
			}
			return (int)writer.BaseStream.Position;
		}

		public static int SaveNPCs(BinaryWriter writer)
		{
			bool[] array = (bool[])NPC.ShimmeredTownNPCs.Clone();
			writer.Write(array.Count(value: true));
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i])
				{
					writer.Write(i);
				}
			}
			for (int j = 0; j < Main.npc.Length; j++)
			{
				NPC nPC = Main.npc[j];
				if (nPC.active && nPC.townNPC && nPC.type != 368)
				{
					writer.Write(nPC.active);
					writer.Write(nPC.netID);
					writer.Write(nPC.GivenName);
					writer.Write(nPC.position.X);
					writer.Write(nPC.position.Y);
					writer.Write(nPC.homeless);
					writer.Write(nPC.homeTileX);
					writer.Write(nPC.homeTileY);
					BitsByte bitsByte = (byte)0;
					bitsByte[0] = nPC.townNPC;
					writer.Write(bitsByte);
					if (bitsByte[0])
					{
						writer.Write(nPC.townNpcVariationIndex);
					}
				}
			}
			writer.Write(value: false);
			for (int k = 0; k < Main.npc.Length; k++)
			{
				NPC nPC2 = Main.npc[k];
				if (nPC2.active && NPCID.Sets.SavesAndLoads[nPC2.type])
				{
					writer.Write(nPC2.active);
					writer.Write(nPC2.netID);
					writer.WriteVector2(nPC2.position);
				}
			}
			writer.Write(value: false);
			return (int)writer.BaseStream.Position;
		}

		public static int SaveFooter(BinaryWriter writer)
		{
			writer.Write(value: true);
			writer.Write(Main.worldName);
			writer.Write(Main.worldID);
			return (int)writer.BaseStream.Position;
		}

		public static int LoadWorld_Version2(BinaryReader reader)
		{
			reader.BaseStream.Position = 0L;
			if (!LoadFileFormatHeader(reader, out var importance, out var positions))
			{
				return 5;
			}
			if (reader.BaseStream.Position != positions[0])
			{
				return 5;
			}
			LoadHeader(reader);
			if (reader.BaseStream.Position != positions[1])
			{
				return 5;
			}
			LoadWorldTiles(reader, importance);
			if (reader.BaseStream.Position != positions[2])
			{
				return 5;
			}
			LoadChests(reader);
			if (reader.BaseStream.Position != positions[3])
			{
				return 5;
			}
			LoadSigns(reader);
			if (reader.BaseStream.Position != positions[4])
			{
				return 5;
			}
			LoadNPCs(reader);
			if (reader.BaseStream.Position != positions[5])
			{
				return 5;
			}
			if (_versionNumber >= 116)
			{
				if (_versionNumber < 122)
				{
					LoadDummies(reader);
					if (reader.BaseStream.Position != positions[6])
					{
						return 5;
					}
				}
				else
				{
					LoadTileEntities(reader);
					if (reader.BaseStream.Position != positions[6])
					{
						return 5;
					}
				}
			}
			if (_versionNumber >= 170)
			{
				LoadWeightedPressurePlates(reader);
				if (reader.BaseStream.Position != positions[7])
				{
					return 5;
				}
			}
			if (_versionNumber >= 189)
			{
				LoadTownManager(reader);
				if (reader.BaseStream.Position != positions[8])
				{
					return 5;
				}
			}
			if (_versionNumber >= 210)
			{
				LoadBestiary(reader, _versionNumber);
				if (reader.BaseStream.Position != positions[9])
				{
					return 5;
				}
			}
			else
			{
				LoadBestiaryForVersionsBefore210();
			}
			if (_versionNumber >= 220)
			{
				LoadCreativePowers(reader, _versionNumber);
				if (reader.BaseStream.Position != positions[10])
				{
					return 5;
				}
			}
			LoadWorld_LastMinuteFixes();
			return LoadFooter(reader);
		}

		private static void LoadWorld_LastMinuteFixes()
		{
			if (_versionNumber < 258)
			{
				ConvertIlluminantPaintToNewField();
			}
			FixAgainstExploits();
		}

		private static void FixAgainstExploits()
		{
			for (int i = 0; i < 8000; i++)
			{
				Main.chest[i]?.FixLoadedData();
			}
			foreach (TileEntity value in TileEntity.ByID.Values)
			{
				if (value is IFixLoadedData fixLoadedData)
				{
					fixLoadedData.FixLoadedData();
				}
			}
		}

		public static bool LoadFileFormatHeader(BinaryReader reader, out bool[] importance, out int[] positions)
		{
			importance = null;
			positions = null;
			if ((_versionNumber = reader.ReadInt32()) >= 135)
			{
				try
				{
					Main.WorldFileMetadata = FileMetadata.Read(reader, FileType.World);
				}
				catch (FormatException value)
				{
					Console.WriteLine(Language.GetTextValue("Error.UnableToLoadWorld"));
					Console.WriteLine(value);
					return false;
				}
			}
			else
			{
				Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
			}
			short num = reader.ReadInt16();
			positions = new int[num];
			for (int i = 0; i < num; i++)
			{
				positions[i] = reader.ReadInt32();
			}
			short num2 = reader.ReadInt16();
			importance = new bool[num2];
			byte b = 0;
			byte b2 = 128;
			for (int i = 0; i < num2; i++)
			{
				if (b2 == 128)
				{
					b = reader.ReadByte();
					b2 = 1;
				}
				else
				{
					b2 = (byte)(b2 << 1);
				}
				if ((b & b2) == b2)
				{
					importance[i] = true;
				}
			}
			return true;
		}

		public static void LoadHeader(BinaryReader reader)
		{
			int versionNumber = _versionNumber;
			Main.worldName = reader.ReadString();
			if (versionNumber >= 179)
			{
				string seed = ((versionNumber != 179) ? reader.ReadString() : reader.ReadInt32().ToString());
				Main.ActiveWorldFileData.SetSeed(seed);
				Main.ActiveWorldFileData.WorldGeneratorVersion = reader.ReadUInt64();
			}
			if (versionNumber >= 181)
			{
				Main.ActiveWorldFileData.UniqueId = new Guid(reader.ReadBytes(16));
			}
			else
			{
				Main.ActiveWorldFileData.UniqueId = Guid.NewGuid();
			}
			Main.worldID = reader.ReadInt32();
			Main.leftWorld = reader.ReadInt32();
			Main.rightWorld = reader.ReadInt32();
			Main.topWorld = reader.ReadInt32();
			Main.bottomWorld = reader.ReadInt32();
			Main.maxTilesY = reader.ReadInt32();
			Main.maxTilesX = reader.ReadInt32();
			WorldGen.clearWorld();
			if (versionNumber >= 209)
			{
				Main.GameMode = reader.ReadInt32();
				if (versionNumber >= 222)
				{
					Main.drunkWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 227)
				{
					Main.getGoodWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 238)
				{
					Main.tenthAnniversaryWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 239)
				{
					Main.dontStarveWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 241)
				{
					Main.notTheBeesWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 249)
				{
					Main.remixWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 266)
				{
					Main.noTrapsWorld = reader.ReadBoolean();
				}
				if (versionNumber >= 267)
				{
					Main.zenithWorld = reader.ReadBoolean();
				}
				else
				{
					Main.zenithWorld = Main.remixWorld && Main.drunkWorld;
				}
			}
			else
			{
				if (versionNumber >= 112)
				{
					Main.GameMode = (reader.ReadBoolean() ? 1 : 0);
				}
				else
				{
					Main.GameMode = 0;
				}
				if (versionNumber == 208 && reader.ReadBoolean())
				{
					Main.GameMode = 2;
				}
			}
			if (versionNumber >= 141)
			{
				Main.ActiveWorldFileData.CreationTime = DateTime.FromBinary(reader.ReadInt64());
			}
			Main.moonType = reader.ReadByte();
			Main.treeX[0] = reader.ReadInt32();
			Main.treeX[1] = reader.ReadInt32();
			Main.treeX[2] = reader.ReadInt32();
			Main.treeStyle[0] = reader.ReadInt32();
			Main.treeStyle[1] = reader.ReadInt32();
			Main.treeStyle[2] = reader.ReadInt32();
			Main.treeStyle[3] = reader.ReadInt32();
			Main.caveBackX[0] = reader.ReadInt32();
			Main.caveBackX[1] = reader.ReadInt32();
			Main.caveBackX[2] = reader.ReadInt32();
			Main.caveBackStyle[0] = reader.ReadInt32();
			Main.caveBackStyle[1] = reader.ReadInt32();
			Main.caveBackStyle[2] = reader.ReadInt32();
			Main.caveBackStyle[3] = reader.ReadInt32();
			Main.iceBackStyle = reader.ReadInt32();
			Main.jungleBackStyle = reader.ReadInt32();
			Main.hellBackStyle = reader.ReadInt32();
			Main.spawnTileX = reader.ReadInt32();
			Main.spawnTileY = reader.ReadInt32();
			Main.worldSurface = reader.ReadDouble();
			Main.rockLayer = reader.ReadDouble();
			_tempTime = reader.ReadDouble();
			_tempDayTime = reader.ReadBoolean();
			_tempMoonPhase = reader.ReadInt32();
			_tempBloodMoon = reader.ReadBoolean();
			_tempEclipse = reader.ReadBoolean();
			Main.eclipse = _tempEclipse;
			Main.dungeonX = reader.ReadInt32();
			Main.dungeonY = reader.ReadInt32();
			WorldGen.crimson = reader.ReadBoolean();
			NPC.downedBoss1 = reader.ReadBoolean();
			NPC.downedBoss2 = reader.ReadBoolean();
			NPC.downedBoss3 = reader.ReadBoolean();
			NPC.downedQueenBee = reader.ReadBoolean();
			NPC.downedMechBoss1 = reader.ReadBoolean();
			NPC.downedMechBoss2 = reader.ReadBoolean();
			NPC.downedMechBoss3 = reader.ReadBoolean();
			NPC.downedMechBossAny = reader.ReadBoolean();
			NPC.downedPlantBoss = reader.ReadBoolean();
			NPC.downedGolemBoss = reader.ReadBoolean();
			if (versionNumber >= 118)
			{
				NPC.downedSlimeKing = reader.ReadBoolean();
			}
			NPC.savedGoblin = reader.ReadBoolean();
			NPC.savedWizard = reader.ReadBoolean();
			NPC.savedMech = reader.ReadBoolean();
			NPC.downedGoblins = reader.ReadBoolean();
			NPC.downedClown = reader.ReadBoolean();
			NPC.downedFrost = reader.ReadBoolean();
			NPC.downedPirates = reader.ReadBoolean();
			WorldGen.shadowOrbSmashed = reader.ReadBoolean();
			WorldGen.spawnMeteor = reader.ReadBoolean();
			WorldGen.shadowOrbCount = reader.ReadByte();
			WorldGen.altarCount = reader.ReadInt32();
			Main.hardMode = reader.ReadBoolean();
			if (versionNumber >= 257)
			{
				Main.afterPartyOfDoom = reader.ReadBoolean();
			}
			else
			{
				Main.afterPartyOfDoom = false;
			}
			Main.invasionDelay = reader.ReadInt32();
			Main.invasionSize = reader.ReadInt32();
			Main.invasionType = reader.ReadInt32();
			Main.invasionX = reader.ReadDouble();
			if (versionNumber >= 118)
			{
				Main.slimeRainTime = reader.ReadDouble();
			}
			if (versionNumber >= 113)
			{
				Main.sundialCooldown = reader.ReadByte();
			}
			_tempRaining = reader.ReadBoolean();
			_tempRainTime = reader.ReadInt32();
			_tempMaxRain = reader.ReadSingle();
			WorldGen.SavedOreTiers.Cobalt = reader.ReadInt32();
			WorldGen.SavedOreTiers.Mythril = reader.ReadInt32();
			WorldGen.SavedOreTiers.Adamantite = reader.ReadInt32();
			WorldGen.setBG(0, reader.ReadByte());
			WorldGen.setBG(1, reader.ReadByte());
			WorldGen.setBG(2, reader.ReadByte());
			WorldGen.setBG(3, reader.ReadByte());
			WorldGen.setBG(4, reader.ReadByte());
			WorldGen.setBG(5, reader.ReadByte());
			WorldGen.setBG(6, reader.ReadByte());
			WorldGen.setBG(7, reader.ReadByte());
			Main.cloudBGActive = reader.ReadInt32();
			Main.cloudBGAlpha = (((double)Main.cloudBGActive < 1.0) ? 0f : 1f);
			Main.cloudBGActive = -WorldGen.genRand.Next(8640, 86400);
			Main.numClouds = reader.ReadInt16();
			Main.windSpeedTarget = reader.ReadSingle();
			Main.windSpeedCurrent = Main.windSpeedTarget;
			if (versionNumber < 95)
			{
				return;
			}
			Main.anglerWhoFinishedToday.Clear();
			for (int num = reader.ReadInt32(); num > 0; num--)
			{
				Main.anglerWhoFinishedToday.Add(reader.ReadString());
			}
			if (versionNumber < 99)
			{
				return;
			}
			NPC.savedAngler = reader.ReadBoolean();
			if (versionNumber < 101)
			{
				return;
			}
			Main.anglerQuest = reader.ReadInt32();
			if (versionNumber < 104)
			{
				return;
			}
			NPC.savedStylist = reader.ReadBoolean();
			if (versionNumber >= 129)
			{
				NPC.savedTaxCollector = reader.ReadBoolean();
			}
			if (versionNumber >= 201)
			{
				NPC.savedGolfer = reader.ReadBoolean();
			}
			if (versionNumber < 107)
			{
				if (Main.invasionType > 0 && Main.invasionSize > 0)
				{
					Main.FakeLoadInvasionStart();
				}
			}
			else
			{
				Main.invasionSizeStart = reader.ReadInt32();
			}
			if (versionNumber < 108)
			{
				_tempCultistDelay = 86400;
			}
			else
			{
				_tempCultistDelay = reader.ReadInt32();
			}
			if (versionNumber < 109)
			{
				return;
			}
			int num2 = reader.ReadInt16();
			for (int i = 0; i < num2; i++)
			{
				if (i < 688)
				{
					NPC.killCount[i] = reader.ReadInt32();
				}
				else
				{
					reader.ReadInt32();
				}
			}
			if (versionNumber < 128)
			{
				return;
			}
			Main.fastForwardTimeToDawn = reader.ReadBoolean();
			if (versionNumber < 131)
			{
				return;
			}
			NPC.downedFishron = reader.ReadBoolean();
			NPC.downedMartians = reader.ReadBoolean();
			NPC.downedAncientCultist = reader.ReadBoolean();
			NPC.downedMoonlord = reader.ReadBoolean();
			NPC.downedHalloweenKing = reader.ReadBoolean();
			NPC.downedHalloweenTree = reader.ReadBoolean();
			NPC.downedChristmasIceQueen = reader.ReadBoolean();
			NPC.downedChristmasSantank = reader.ReadBoolean();
			NPC.downedChristmasTree = reader.ReadBoolean();
			if (versionNumber < 140)
			{
				return;
			}
			NPC.downedTowerSolar = reader.ReadBoolean();
			NPC.downedTowerVortex = reader.ReadBoolean();
			NPC.downedTowerNebula = reader.ReadBoolean();
			NPC.downedTowerStardust = reader.ReadBoolean();
			NPC.TowerActiveSolar = reader.ReadBoolean();
			NPC.TowerActiveVortex = reader.ReadBoolean();
			NPC.TowerActiveNebula = reader.ReadBoolean();
			NPC.TowerActiveStardust = reader.ReadBoolean();
			NPC.LunarApocalypseIsUp = reader.ReadBoolean();
			if (NPC.TowerActiveSolar)
			{
				NPC.ShieldStrengthTowerSolar = NPC.ShieldStrengthTowerMax;
			}
			if (NPC.TowerActiveVortex)
			{
				NPC.ShieldStrengthTowerVortex = NPC.ShieldStrengthTowerMax;
			}
			if (NPC.TowerActiveNebula)
			{
				NPC.ShieldStrengthTowerNebula = NPC.ShieldStrengthTowerMax;
			}
			if (NPC.TowerActiveStardust)
			{
				NPC.ShieldStrengthTowerStardust = NPC.ShieldStrengthTowerMax;
			}
			if (versionNumber < 170)
			{
				_tempPartyManual = false;
				_tempPartyGenuine = false;
				_tempPartyCooldown = 0;
				TempPartyCelebratingNPCs.Clear();
			}
			else
			{
				_tempPartyManual = reader.ReadBoolean();
				_tempPartyGenuine = reader.ReadBoolean();
				_tempPartyCooldown = reader.ReadInt32();
				int num3 = reader.ReadInt32();
				TempPartyCelebratingNPCs.Clear();
				for (int j = 0; j < num3; j++)
				{
					TempPartyCelebratingNPCs.Add(reader.ReadInt32());
				}
			}
			if (versionNumber < 174)
			{
				_tempSandstormHappening = false;
				_tempSandstormTimeLeft = 0;
				_tempSandstormSeverity = 0f;
				_tempSandstormIntendedSeverity = 0f;
			}
			else
			{
				_tempSandstormHappening = reader.ReadBoolean();
				_tempSandstormTimeLeft = reader.ReadInt32();
				_tempSandstormSeverity = reader.ReadSingle();
				_tempSandstormIntendedSeverity = reader.ReadSingle();
			}
			DD2Event.Load(reader, versionNumber);
			if (versionNumber > 194)
			{
				WorldGen.setBG(8, reader.ReadByte());
			}
			else
			{
				WorldGen.setBG(8, 0);
			}
			if (versionNumber >= 215)
			{
				WorldGen.setBG(9, reader.ReadByte());
			}
			else
			{
				WorldGen.setBG(9, 0);
			}
			if (versionNumber > 195)
			{
				WorldGen.setBG(10, reader.ReadByte());
				WorldGen.setBG(11, reader.ReadByte());
				WorldGen.setBG(12, reader.ReadByte());
			}
			else
			{
				WorldGen.setBG(10, WorldGen.treeBG1);
				WorldGen.setBG(11, WorldGen.treeBG1);
				WorldGen.setBG(12, WorldGen.treeBG1);
			}
			if (versionNumber >= 204)
			{
				NPC.combatBookWasUsed = reader.ReadBoolean();
			}
			if (versionNumber < 207)
			{
				_tempLanternNightCooldown = 0;
				_tempLanternNightGenuine = false;
				_tempLanternNightManual = false;
				_tempLanternNightNextNightIsGenuine = false;
			}
			else
			{
				_tempLanternNightCooldown = reader.ReadInt32();
				_tempLanternNightGenuine = reader.ReadBoolean();
				_tempLanternNightManual = reader.ReadBoolean();
				_tempLanternNightNextNightIsGenuine = reader.ReadBoolean();
			}
			WorldGen.TreeTops.Load(reader, versionNumber);
			if (versionNumber >= 212)
			{
				Main.forceHalloweenForToday = reader.ReadBoolean();
				Main.forceXMasForToday = reader.ReadBoolean();
			}
			else
			{
				Main.forceHalloweenForToday = false;
				Main.forceXMasForToday = false;
			}
			if (versionNumber >= 216)
			{
				WorldGen.SavedOreTiers.Copper = reader.ReadInt32();
				WorldGen.SavedOreTiers.Iron = reader.ReadInt32();
				WorldGen.SavedOreTiers.Silver = reader.ReadInt32();
				WorldGen.SavedOreTiers.Gold = reader.ReadInt32();
			}
			else
			{
				WorldGen.SavedOreTiers.Copper = -1;
				WorldGen.SavedOreTiers.Iron = -1;
				WorldGen.SavedOreTiers.Silver = -1;
				WorldGen.SavedOreTiers.Gold = -1;
			}
			if (versionNumber >= 217)
			{
				NPC.boughtCat = reader.ReadBoolean();
				NPC.boughtDog = reader.ReadBoolean();
				NPC.boughtBunny = reader.ReadBoolean();
			}
			else
			{
				NPC.boughtCat = false;
				NPC.boughtDog = false;
				NPC.boughtBunny = false;
			}
			if (versionNumber >= 223)
			{
				NPC.downedEmpressOfLight = reader.ReadBoolean();
				NPC.downedQueenSlime = reader.ReadBoolean();
			}
			else
			{
				NPC.downedEmpressOfLight = false;
				NPC.downedQueenSlime = false;
			}
			if (versionNumber >= 240)
			{
				NPC.downedDeerclops = reader.ReadBoolean();
			}
			else
			{
				NPC.downedDeerclops = false;
			}
			if (versionNumber >= 250)
			{
				NPC.unlockedSlimeBlueSpawn = reader.ReadBoolean();
			}
			else
			{
				NPC.unlockedSlimeBlueSpawn = false;
			}
			if (versionNumber >= 251)
			{
				NPC.unlockedMerchantSpawn = reader.ReadBoolean();
				NPC.unlockedDemolitionistSpawn = reader.ReadBoolean();
				NPC.unlockedPartyGirlSpawn = reader.ReadBoolean();
				NPC.unlockedDyeTraderSpawn = reader.ReadBoolean();
				NPC.unlockedTruffleSpawn = reader.ReadBoolean();
				NPC.unlockedArmsDealerSpawn = reader.ReadBoolean();
				NPC.unlockedNurseSpawn = reader.ReadBoolean();
				NPC.unlockedPrincessSpawn = reader.ReadBoolean();
			}
			else
			{
				NPC.unlockedMerchantSpawn = false;
				NPC.unlockedDemolitionistSpawn = false;
				NPC.unlockedPartyGirlSpawn = false;
				NPC.unlockedDyeTraderSpawn = false;
				NPC.unlockedTruffleSpawn = false;
				NPC.unlockedArmsDealerSpawn = false;
				NPC.unlockedNurseSpawn = false;
				NPC.unlockedPrincessSpawn = false;
			}
			if (versionNumber >= 259)
			{
				NPC.combatBookVolumeTwoWasUsed = reader.ReadBoolean();
			}
			else
			{
				NPC.combatBookVolumeTwoWasUsed = false;
			}
			if (versionNumber >= 260)
			{
				NPC.peddlersSatchelWasUsed = reader.ReadBoolean();
			}
			else
			{
				NPC.peddlersSatchelWasUsed = false;
			}
			if (versionNumber >= 261)
			{
				NPC.unlockedSlimeGreenSpawn = reader.ReadBoolean();
				NPC.unlockedSlimeOldSpawn = reader.ReadBoolean();
				NPC.unlockedSlimePurpleSpawn = reader.ReadBoolean();
				NPC.unlockedSlimeRainbowSpawn = reader.ReadBoolean();
				NPC.unlockedSlimeRedSpawn = reader.ReadBoolean();
				NPC.unlockedSlimeYellowSpawn = reader.ReadBoolean();
				NPC.unlockedSlimeCopperSpawn = reader.ReadBoolean();
			}
			else
			{
				NPC.unlockedSlimeGreenSpawn = false;
				NPC.unlockedSlimeOldSpawn = false;
				NPC.unlockedSlimePurpleSpawn = false;
				NPC.unlockedSlimeRainbowSpawn = false;
				NPC.unlockedSlimeRedSpawn = false;
				NPC.unlockedSlimeYellowSpawn = false;
				NPC.unlockedSlimeCopperSpawn = false;
			}
			if (versionNumber >= 264)
			{
				Main.fastForwardTimeToDusk = reader.ReadBoolean();
				Main.moondialCooldown = reader.ReadByte();
			}
			else
			{
				Main.fastForwardTimeToDusk = false;
				Main.moondialCooldown = 0;
			}
			Main.UpdateTimeRate();
		}

		public static void LoadWorldTiles(BinaryReader reader, bool[] importance)
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				float num = (float)i / (float)Main.maxTilesX;
				Main.statusText = Lang.gen[51].Value + " " + (int)((double)num * 100.0 + 1.0) + "%";
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					int num2 = -1;
					byte b2;
					byte b;
					byte b3 = (b2 = (b = 0));
					Tile tile = Main.tile[i, j];
					byte b4 = reader.ReadByte();
					bool flag = false;
					if ((b4 & 1) == 1)
					{
						flag = true;
						b3 = reader.ReadByte();
					}
					bool flag2 = false;
					if (flag && (b3 & 1) == 1)
					{
						flag2 = true;
						b2 = reader.ReadByte();
					}
					if (flag2 && (b2 & 1) == 1)
					{
						b = reader.ReadByte();
					}
					byte b5;
					if ((b4 & 2) == 2)
					{
						tile.active(active: true);
						if ((b4 & 0x20) == 32)
						{
							b5 = reader.ReadByte();
							num2 = reader.ReadByte();
							num2 = (num2 << 8) | b5;
						}
						else
						{
							num2 = reader.ReadByte();
						}
						tile.type = (ushort)num2;
						if (importance[num2])
						{
							tile.frameX = reader.ReadInt16();
							tile.frameY = reader.ReadInt16();
							if (tile.type == 144)
							{
								tile.frameY = 0;
							}
						}
						else
						{
							tile.frameX = -1;
							tile.frameY = -1;
						}
						if ((b2 & 8) == 8)
						{
							tile.color(reader.ReadByte());
						}
					}
					if ((b4 & 4) == 4)
					{
						tile.wall = reader.ReadByte();
						if (tile.wall >= 347)
						{
							tile.wall = 0;
						}
						if ((b2 & 0x10) == 16)
						{
							tile.wallColor(reader.ReadByte());
						}
					}
					b5 = (byte)((b4 & 0x18) >> 3);
					if (b5 != 0)
					{
						tile.liquid = reader.ReadByte();
						if ((b2 & 0x80) == 128)
						{
							tile.shimmer(shimmer: true);
						}
						else if (b5 > 1)
						{
							if (b5 == 2)
							{
								tile.lava(lava: true);
							}
							else
							{
								tile.honey(honey: true);
							}
						}
					}
					if (b3 > 1)
					{
						if ((b3 & 2) == 2)
						{
							tile.wire(wire: true);
						}
						if ((b3 & 4) == 4)
						{
							tile.wire2(wire2: true);
						}
						if ((b3 & 8) == 8)
						{
							tile.wire3(wire3: true);
						}
						b5 = (byte)((b3 & 0x70) >> 4);
						if (b5 != 0 && (Main.tileSolid[tile.type] || TileID.Sets.NonSolidSaveSlopes[tile.type]))
						{
							if (b5 == 1)
							{
								tile.halfBrick(halfBrick: true);
							}
							else
							{
								tile.slope((byte)(b5 - 1));
							}
						}
					}
					if (b2 > 1)
					{
						if ((b2 & 2) == 2)
						{
							tile.actuator(actuator: true);
						}
						if ((b2 & 4) == 4)
						{
							tile.inActive(inActive: true);
						}
						if ((b2 & 0x20) == 32)
						{
							tile.wire4(wire4: true);
						}
						if ((b2 & 0x40) == 64)
						{
							b5 = reader.ReadByte();
							tile.wall = (ushort)((b5 << 8) | tile.wall);
							if (tile.wall >= 347)
							{
								tile.wall = 0;
							}
						}
					}
					if (b > 1)
					{
						if ((b & 2) == 2)
						{
							tile.invisibleBlock(invisibleBlock: true);
						}
						if ((b & 4) == 4)
						{
							tile.invisibleWall(invisibleWall: true);
						}
						if ((b & 8) == 8)
						{
							tile.fullbrightBlock(fullbrightBlock: true);
						}
						if ((b & 0x10) == 16)
						{
							tile.fullbrightWall(fullbrightWall: true);
						}
					}
					int num3 = (byte)((b4 & 0xC0) >> 6) switch
					{
						0 => 0, 
						1 => reader.ReadByte(), 
						_ => reader.ReadInt16(), 
					};
					if (num2 != -1)
					{
						if ((double)j <= Main.worldSurface)
						{
							if ((double)(j + num3) <= Main.worldSurface)
							{
								WorldGen.tileCounts[num2] += (num3 + 1) * 5;
							}
							else
							{
								int num4 = (int)(Main.worldSurface - (double)j + 1.0);
								int num5 = num3 + 1 - num4;
								WorldGen.tileCounts[num2] += num4 * 5 + num5;
							}
						}
						else
						{
							WorldGen.tileCounts[num2] += num3 + 1;
						}
					}
					while (num3 > 0)
					{
						j++;
						Main.tile[i, j].CopyFrom(tile);
						num3--;
					}
				}
			}
			WorldGen.AddUpAlignmentCounts(clearCounts: true);
			if (_versionNumber < 105)
			{
				WorldGen.FixHearts();
			}
		}

		public static void LoadChests(BinaryReader reader)
		{
			int num = reader.ReadInt16();
			int num2 = reader.ReadInt16();
			int num3;
			int num4;
			if (num2 < 40)
			{
				num3 = num2;
				num4 = 0;
			}
			else
			{
				num3 = 40;
				num4 = num2 - 40;
			}
			int i;
			for (i = 0; i < num; i++)
			{
				Chest chest = new Chest();
				chest.x = reader.ReadInt32();
				chest.y = reader.ReadInt32();
				chest.name = reader.ReadString();
				for (int j = 0; j < num3; j++)
				{
					short num5 = reader.ReadInt16();
					Item item = new Item();
					if (num5 > 0)
					{
						item.netDefaults(reader.ReadInt32());
						item.stack = num5;
						item.Prefix(reader.ReadByte());
					}
					else if (num5 < 0)
					{
						item.netDefaults(reader.ReadInt32());
						item.Prefix(reader.ReadByte());
						item.stack = 1;
					}
					chest.item[j] = item;
				}
				for (int k = 0; k < num4; k++)
				{
					short num5 = reader.ReadInt16();
					if (num5 > 0)
					{
						reader.ReadInt32();
						reader.ReadByte();
					}
				}
				Main.chest[i] = chest;
			}
			List<Point16> list = new List<Point16>();
			for (int l = 0; l < i; l++)
			{
				if (Main.chest[l] != null)
				{
					Point16 item2 = new Point16(Main.chest[l].x, Main.chest[l].y);
					if (list.Contains(item2))
					{
						Main.chest[l] = null;
					}
					else
					{
						list.Add(item2);
					}
				}
			}
			for (; i < 8000; i++)
			{
				Main.chest[i] = null;
			}
			if (_versionNumber < 115)
			{
				FixDresserChests();
			}
		}

		private static void ConvertIlluminantPaintToNewField()
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && tile.color() == 31)
					{
						tile.color(0);
						tile.fullbrightBlock(fullbrightBlock: true);
					}
					if (tile.wallColor() == 31)
					{
						tile.wallColor(0);
						tile.fullbrightWall(fullbrightWall: true);
					}
				}
			}
		}

		public static void LoadSigns(BinaryReader reader)
		{
			short num = reader.ReadInt16();
			int i;
			for (i = 0; i < num; i++)
			{
				string text = reader.ReadString();
				int num2 = reader.ReadInt32();
				int num3 = reader.ReadInt32();
				Tile tile = Main.tile[num2, num3];
				Sign sign;
				if (tile.active() && Main.tileSign[tile.type])
				{
					sign = new Sign();
					sign.text = text;
					sign.x = num2;
					sign.y = num3;
				}
				else
				{
					sign = null;
				}
				Main.sign[i] = sign;
			}
			List<Point16> list = new List<Point16>();
			for (int j = 0; j < 1000; j++)
			{
				if (Main.sign[j] != null)
				{
					Point16 item = new Point16(Main.sign[j].x, Main.sign[j].y);
					if (list.Contains(item))
					{
						Main.sign[j] = null;
					}
					else
					{
						list.Add(item);
					}
				}
			}
			for (; i < 1000; i++)
			{
				Main.sign[i] = null;
			}
		}

		public static void LoadDummies(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				reader.ReadInt16();
				reader.ReadInt16();
			}
		}

		public static void LoadNPCs(BinaryReader reader)
		{
			if (_versionNumber >= 268)
			{
				int num = reader.ReadInt32();
				while (num-- > 0)
				{
					NPC.ShimmeredTownNPCs[reader.ReadInt32()] = true;
				}
			}
			int num2 = 0;
			bool flag = reader.ReadBoolean();
			while (flag)
			{
				NPC nPC = Main.npc[num2];
				if (_versionNumber >= 190)
				{
					nPC.SetDefaults(reader.ReadInt32());
				}
				else
				{
					nPC.SetDefaults(NPCID.FromLegacyName(reader.ReadString()));
				}
				nPC.GivenName = reader.ReadString();
				nPC.position.X = reader.ReadSingle();
				nPC.position.Y = reader.ReadSingle();
				nPC.homeless = reader.ReadBoolean();
				nPC.homeTileX = reader.ReadInt32();
				nPC.homeTileY = reader.ReadInt32();
				if (_versionNumber >= 213 && ((BitsByte)reader.ReadByte())[0])
				{
					nPC.townNpcVariationIndex = reader.ReadInt32();
				}
				num2++;
				flag = reader.ReadBoolean();
			}
			if (_versionNumber < 140)
			{
				return;
			}
			flag = reader.ReadBoolean();
			while (flag)
			{
				NPC nPC = Main.npc[num2];
				if (_versionNumber >= 190)
				{
					nPC.SetDefaults(reader.ReadInt32());
				}
				else
				{
					nPC.SetDefaults(NPCID.FromLegacyName(reader.ReadString()));
				}
				nPC.position = reader.ReadVector2();
				num2++;
				flag = reader.ReadBoolean();
			}
			if (_versionNumber < 251)
			{
				NPC.unlockedMerchantSpawn = NPC.AnyNPCs(17);
				NPC.unlockedDemolitionistSpawn = NPC.AnyNPCs(38);
				NPC.unlockedPartyGirlSpawn = NPC.AnyNPCs(208);
				NPC.unlockedDyeTraderSpawn = NPC.AnyNPCs(207);
				NPC.unlockedTruffleSpawn = NPC.AnyNPCs(160);
				NPC.unlockedArmsDealerSpawn = NPC.AnyNPCs(19);
				NPC.unlockedNurseSpawn = NPC.AnyNPCs(18);
				NPC.unlockedPrincessSpawn = NPC.AnyNPCs(663);
			}
		}

		public static void ValidateLoadNPCs(BinaryReader fileIO)
		{
			int num = fileIO.ReadInt32();
			while (num-- > 0)
			{
				fileIO.ReadInt32();
			}
			bool flag = fileIO.ReadBoolean();
			while (flag)
			{
				fileIO.ReadInt32();
				fileIO.ReadString();
				fileIO.ReadSingle();
				fileIO.ReadSingle();
				fileIO.ReadBoolean();
				fileIO.ReadInt32();
				fileIO.ReadInt32();
				if (((BitsByte)fileIO.ReadByte())[0])
				{
					fileIO.ReadInt32();
				}
				flag = fileIO.ReadBoolean();
			}
			flag = fileIO.ReadBoolean();
			while (flag)
			{
				fileIO.ReadInt32();
				fileIO.ReadSingle();
				fileIO.ReadSingle();
				flag = fileIO.ReadBoolean();
			}
		}

		public static int LoadFooter(BinaryReader reader)
		{
			if (!reader.ReadBoolean())
			{
				return 6;
			}
			if (reader.ReadString() != Main.worldName)
			{
				return 6;
			}
			if (reader.ReadInt32() != Main.worldID)
			{
				return 6;
			}
			return 0;
		}

		public static bool ValidateWorld(BinaryReader fileIO)
		{
			new Stopwatch().Start();
			try
			{
				Stream baseStream = fileIO.BaseStream;
				int num = fileIO.ReadInt32();
				if (num == 0 || num > 270)
				{
					return false;
				}
				baseStream.Position = 0L;
				if (!LoadFileFormatHeader(fileIO, out var importance, out var positions))
				{
					return false;
				}
				string text = fileIO.ReadString();
				if (num >= 179)
				{
					if (num == 179)
					{
						fileIO.ReadInt32();
					}
					else
					{
						fileIO.ReadString();
					}
					fileIO.ReadUInt64();
				}
				if (num >= 181)
				{
					fileIO.ReadBytes(16);
				}
				int num2 = fileIO.ReadInt32();
				fileIO.ReadInt32();
				fileIO.ReadInt32();
				fileIO.ReadInt32();
				fileIO.ReadInt32();
				int num3 = fileIO.ReadInt32();
				int num4 = fileIO.ReadInt32();
				baseStream.Position = positions[1];
				for (int i = 0; i < num4; i++)
				{
					float num5 = (float)i / (float)Main.maxTilesX;
					Main.statusText = Lang.gen[73].Value + " " + (int)(num5 * 100f + 1f) + "%";
					int num6;
					for (num6 = 0; num6 < num3; num6++)
					{
						byte b;
						byte b2 = (b = 0);
						byte b3 = fileIO.ReadByte();
						bool flag = false;
						if ((b3 & 1) == 1)
						{
							flag = true;
							b2 = fileIO.ReadByte();
						}
						bool flag2 = false;
						if (flag && (b2 & 1) == 1)
						{
							flag2 = true;
							b = fileIO.ReadByte();
						}
						if (flag2 && (b & 1) == 1)
						{
							fileIO.ReadByte();
						}
						if ((b3 & 2) == 2)
						{
							int num7;
							if ((b3 & 0x20) == 32)
							{
								byte b4 = fileIO.ReadByte();
								num7 = fileIO.ReadByte();
								num7 = (num7 << 8) | b4;
							}
							else
							{
								num7 = fileIO.ReadByte();
							}
							if (importance[num7])
							{
								fileIO.ReadInt16();
								fileIO.ReadInt16();
							}
							if ((b & 8) == 8)
							{
								fileIO.ReadByte();
							}
						}
						if ((b3 & 4) == 4)
						{
							fileIO.ReadByte();
							if ((b & 0x10) == 16)
							{
								fileIO.ReadByte();
							}
						}
						if ((b3 & 0x18) >> 3 != 0)
						{
							fileIO.ReadByte();
						}
						if ((b & 0x40) == 64)
						{
							fileIO.ReadByte();
						}
						num6 += (byte)((b3 & 0xC0) >> 6) switch
						{
							0 => 0, 
							1 => fileIO.ReadByte(), 
							_ => fileIO.ReadInt16(), 
						};
					}
				}
				if (baseStream.Position != positions[2])
				{
					return false;
				}
				int num8 = fileIO.ReadInt16();
				int num9 = fileIO.ReadInt16();
				for (int j = 0; j < num8; j++)
				{
					fileIO.ReadInt32();
					fileIO.ReadInt32();
					fileIO.ReadString();
					for (int k = 0; k < num9; k++)
					{
						if (fileIO.ReadInt16() > 0)
						{
							fileIO.ReadInt32();
							fileIO.ReadByte();
						}
					}
				}
				if (baseStream.Position != positions[3])
				{
					return false;
				}
				int num10 = fileIO.ReadInt16();
				for (int l = 0; l < num10; l++)
				{
					fileIO.ReadString();
					fileIO.ReadInt32();
					fileIO.ReadInt32();
				}
				if (baseStream.Position != positions[4])
				{
					return false;
				}
				ValidateLoadNPCs(fileIO);
				if (baseStream.Position != positions[5])
				{
					return false;
				}
				if (_versionNumber >= 116 && _versionNumber <= 121)
				{
					int num11 = fileIO.ReadInt32();
					for (int m = 0; m < num11; m++)
					{
						fileIO.ReadInt16();
						fileIO.ReadInt16();
					}
					if (baseStream.Position != positions[6])
					{
						return false;
					}
				}
				if (_versionNumber >= 122)
				{
					int num12 = fileIO.ReadInt32();
					for (int n = 0; n < num12; n++)
					{
						TileEntity.Read(fileIO);
					}
				}
				if (_versionNumber >= 170)
				{
					int num13 = fileIO.ReadInt32();
					for (int num14 = 0; num14 < num13; num14++)
					{
						fileIO.ReadInt64();
					}
				}
				if (_versionNumber >= 189)
				{
					int num15 = fileIO.ReadInt32();
					fileIO.ReadBytes(12 * num15);
				}
				if (_versionNumber >= 210)
				{
					Main.BestiaryTracker.ValidateWorld(fileIO, _versionNumber);
				}
				if (_versionNumber >= 220)
				{
					CreativePowerManager.Instance.ValidateWorld(fileIO, _versionNumber);
				}
				bool num16 = fileIO.ReadBoolean();
				string text2 = fileIO.ReadString();
				int num17 = fileIO.ReadInt32();
				bool result = false;
				if (num16 && (text2 == text || num17 == num2))
				{
					result = true;
				}
				return result;
			}
			catch (Exception value)
			{
				using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true))
				{
					streamWriter.WriteLine(DateTime.Now);
					streamWriter.WriteLine(value);
					streamWriter.WriteLine("");
				}
				return false;
			}
		}

		public static FileMetadata GetFileMetadata(string file, bool cloudSave)
		{
			if (file == null)
			{
				return null;
			}
			try
			{
				byte[] buffer = null;
				int num;
				if (cloudSave)
				{
					num = ((SocialAPI.Cloud != null) ? 1 : 0);
					if (num != 0)
					{
						int num2 = 24;
						buffer = new byte[num2];
						SocialAPI.Cloud.Read(file, buffer, num2);
					}
				}
				else
				{
					num = 0;
				}
				using Stream input = ((num != 0) ? ((Stream)new MemoryStream(buffer)) : ((Stream)new FileStream(file, FileMode.Open)));
				using BinaryReader binaryReader = new BinaryReader(input);
				if (binaryReader.ReadInt32() >= 135)
				{
					return FileMetadata.Read(binaryReader, FileType.World);
				}
				return FileMetadata.FromCurrentSettings(FileType.World);
			}
			catch
			{
			}
			return null;
		}

		private static void FixDresserChests()
		{
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && tile.type == 88 && tile.frameX % 54 == 0 && tile.frameY % 36 == 0)
					{
						Chest.CreateChest(i, j);
					}
				}
			}
		}

		public static int SaveTileEntities(BinaryWriter writer)
		{
			lock (TileEntity.EntityCreationLock)
			{
				writer.Write(TileEntity.ByID.Count);
				foreach (KeyValuePair<int, TileEntity> item in TileEntity.ByID)
				{
					TileEntity.Write(writer, item.Value);
				}
			}
			return (int)writer.BaseStream.Position;
		}

		public static void LoadTileEntities(BinaryReader reader)
		{
			TileEntity.ByID.Clear();
			TileEntity.ByPosition.Clear();
			int num = reader.ReadInt32();
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				TileEntity tileEntity = TileEntity.Read(reader);
				tileEntity.ID = num2++;
				TileEntity.ByID[tileEntity.ID] = tileEntity;
				if (TileEntity.ByPosition.TryGetValue(tileEntity.Position, out var value))
				{
					TileEntity.ByID.Remove(value.ID);
				}
				TileEntity.ByPosition[tileEntity.Position] = tileEntity;
			}
			TileEntity.TileEntitiesNextID = num;
			List<Point16> list = new List<Point16>();
			foreach (KeyValuePair<Point16, TileEntity> item in TileEntity.ByPosition)
			{
				if (!WorldGen.InWorld(item.Value.Position.X, item.Value.Position.Y, 1))
				{
					list.Add(item.Value.Position);
				}
				else if (!TileEntity.manager.CheckValidTile(item.Value.type, item.Value.Position.X, item.Value.Position.Y))
				{
					list.Add(item.Value.Position);
				}
			}
			try
			{
				foreach (Point16 item2 in list)
				{
					TileEntity tileEntity2 = TileEntity.ByPosition[item2];
					if (TileEntity.ByID.ContainsKey(tileEntity2.ID))
					{
						TileEntity.ByID.Remove(tileEntity2.ID);
					}
					if (TileEntity.ByPosition.ContainsKey(item2))
					{
						TileEntity.ByPosition.Remove(item2);
					}
				}
			}
			catch
			{
			}
		}

		public static int SaveWeightedPressurePlates(BinaryWriter writer)
		{
			lock (PressurePlateHelper.EntityCreationLock)
			{
				writer.Write(PressurePlateHelper.PressurePlatesPressed.Count);
				foreach (KeyValuePair<Point, bool[]> item in PressurePlateHelper.PressurePlatesPressed)
				{
					writer.Write(item.Key.X);
					writer.Write(item.Key.Y);
				}
			}
			return (int)writer.BaseStream.Position;
		}

		public static void LoadWeightedPressurePlates(BinaryReader reader)
		{
			PressurePlateHelper.Reset();
			PressurePlateHelper.NeedsFirstUpdate = true;
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Point key = new Point(reader.ReadInt32(), reader.ReadInt32());
				PressurePlateHelper.PressurePlatesPressed.Add(key, new bool[255]);
			}
		}

		public static int SaveTownManager(BinaryWriter writer)
		{
			WorldGen.TownManager.Save(writer);
			return (int)writer.BaseStream.Position;
		}

		public static void LoadTownManager(BinaryReader reader)
		{
			WorldGen.TownManager.Load(reader);
		}

		public static int SaveBestiary(BinaryWriter writer)
		{
			Main.BestiaryTracker.Save(writer);
			return (int)writer.BaseStream.Position;
		}

		public static void LoadBestiary(BinaryReader reader, int loadVersionNumber)
		{
			Main.BestiaryTracker.Load(reader, loadVersionNumber);
		}

		private static void LoadBestiaryForVersionsBefore210()
		{
			Main.BestiaryTracker.FillBasedOnVersionBefore210();
		}

		public static int SaveCreativePowers(BinaryWriter writer)
		{
			CreativePowerManager.Instance.SaveToWorld(writer);
			return (int)writer.BaseStream.Position;
		}

		public static void LoadCreativePowers(BinaryReader reader, int loadVersionNumber)
		{
			CreativePowerManager.Instance.LoadFromWorld(reader, loadVersionNumber);
		}

		private static int LoadWorld_Version1_Old_BeforeRelease88(BinaryReader fileIO)
		{
			Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
			int versionNumber = _versionNumber;
			if (versionNumber > 270)
			{
				return 1;
			}
			Main.worldName = fileIO.ReadString();
			Main.worldID = fileIO.ReadInt32();
			Main.leftWorld = fileIO.ReadInt32();
			Main.rightWorld = fileIO.ReadInt32();
			Main.topWorld = fileIO.ReadInt32();
			Main.bottomWorld = fileIO.ReadInt32();
			Main.maxTilesY = fileIO.ReadInt32();
			Main.maxTilesX = fileIO.ReadInt32();
			if (versionNumber >= 112)
			{
				Main.GameMode = (fileIO.ReadBoolean() ? 1 : 0);
			}
			else
			{
				Main.GameMode = 0;
			}
			if (versionNumber >= 63)
			{
				Main.moonType = fileIO.ReadByte();
			}
			else
			{
				WorldGen.RandomizeMoonState(WorldGen.genRand);
			}
			WorldGen.clearWorld();
			if (versionNumber >= 44)
			{
				Main.treeX[0] = fileIO.ReadInt32();
				Main.treeX[1] = fileIO.ReadInt32();
				Main.treeX[2] = fileIO.ReadInt32();
				Main.treeStyle[0] = fileIO.ReadInt32();
				Main.treeStyle[1] = fileIO.ReadInt32();
				Main.treeStyle[2] = fileIO.ReadInt32();
				Main.treeStyle[3] = fileIO.ReadInt32();
			}
			if (versionNumber >= 60)
			{
				Main.caveBackX[0] = fileIO.ReadInt32();
				Main.caveBackX[1] = fileIO.ReadInt32();
				Main.caveBackX[2] = fileIO.ReadInt32();
				Main.caveBackStyle[0] = fileIO.ReadInt32();
				Main.caveBackStyle[1] = fileIO.ReadInt32();
				Main.caveBackStyle[2] = fileIO.ReadInt32();
				Main.caveBackStyle[3] = fileIO.ReadInt32();
				Main.iceBackStyle = fileIO.ReadInt32();
				if (versionNumber >= 61)
				{
					Main.jungleBackStyle = fileIO.ReadInt32();
					Main.hellBackStyle = fileIO.ReadInt32();
				}
			}
			else
			{
				WorldGen.RandomizeCaveBackgrounds();
			}
			Main.spawnTileX = fileIO.ReadInt32();
			Main.spawnTileY = fileIO.ReadInt32();
			Main.worldSurface = fileIO.ReadDouble();
			Main.rockLayer = fileIO.ReadDouble();
			_tempTime = fileIO.ReadDouble();
			_tempDayTime = fileIO.ReadBoolean();
			_tempMoonPhase = fileIO.ReadInt32();
			_tempBloodMoon = fileIO.ReadBoolean();
			if (versionNumber >= 70)
			{
				_tempEclipse = fileIO.ReadBoolean();
				Main.eclipse = _tempEclipse;
			}
			Main.dungeonX = fileIO.ReadInt32();
			Main.dungeonY = fileIO.ReadInt32();
			if (versionNumber >= 56)
			{
				WorldGen.crimson = fileIO.ReadBoolean();
			}
			else
			{
				WorldGen.crimson = false;
			}
			NPC.downedBoss1 = fileIO.ReadBoolean();
			NPC.downedBoss2 = fileIO.ReadBoolean();
			NPC.downedBoss3 = fileIO.ReadBoolean();
			if (versionNumber >= 66)
			{
				NPC.downedQueenBee = fileIO.ReadBoolean();
			}
			if (versionNumber >= 44)
			{
				NPC.downedMechBoss1 = fileIO.ReadBoolean();
				NPC.downedMechBoss2 = fileIO.ReadBoolean();
				NPC.downedMechBoss3 = fileIO.ReadBoolean();
				NPC.downedMechBossAny = fileIO.ReadBoolean();
			}
			if (versionNumber >= 64)
			{
				NPC.downedPlantBoss = fileIO.ReadBoolean();
				NPC.downedGolemBoss = fileIO.ReadBoolean();
			}
			if (versionNumber >= 29)
			{
				NPC.savedGoblin = fileIO.ReadBoolean();
				NPC.savedWizard = fileIO.ReadBoolean();
				if (versionNumber >= 34)
				{
					NPC.savedMech = fileIO.ReadBoolean();
					if (versionNumber >= 80)
					{
						NPC.savedStylist = fileIO.ReadBoolean();
					}
				}
				if (versionNumber >= 129)
				{
					NPC.savedTaxCollector = fileIO.ReadBoolean();
				}
				if (versionNumber >= 201)
				{
					NPC.savedGolfer = fileIO.ReadBoolean();
				}
				NPC.downedGoblins = fileIO.ReadBoolean();
			}
			if (versionNumber >= 32)
			{
				NPC.downedClown = fileIO.ReadBoolean();
			}
			if (versionNumber >= 37)
			{
				NPC.downedFrost = fileIO.ReadBoolean();
			}
			if (versionNumber >= 56)
			{
				NPC.downedPirates = fileIO.ReadBoolean();
			}
			WorldGen.shadowOrbSmashed = fileIO.ReadBoolean();
			WorldGen.spawnMeteor = fileIO.ReadBoolean();
			WorldGen.shadowOrbCount = fileIO.ReadByte();
			if (versionNumber >= 23)
			{
				WorldGen.altarCount = fileIO.ReadInt32();
				Main.hardMode = fileIO.ReadBoolean();
			}
			Main.invasionDelay = fileIO.ReadInt32();
			Main.invasionSize = fileIO.ReadInt32();
			Main.invasionType = fileIO.ReadInt32();
			Main.invasionX = fileIO.ReadDouble();
			if (versionNumber >= 113)
			{
				Main.sundialCooldown = fileIO.ReadByte();
			}
			if (versionNumber >= 53)
			{
				_tempRaining = fileIO.ReadBoolean();
				_tempRainTime = fileIO.ReadInt32();
				_tempMaxRain = fileIO.ReadSingle();
			}
			if (versionNumber >= 54)
			{
				WorldGen.SavedOreTiers.Cobalt = fileIO.ReadInt32();
				WorldGen.SavedOreTiers.Mythril = fileIO.ReadInt32();
				WorldGen.SavedOreTiers.Adamantite = fileIO.ReadInt32();
			}
			else if (versionNumber >= 23 && WorldGen.altarCount == 0)
			{
				WorldGen.SavedOreTiers.Cobalt = -1;
				WorldGen.SavedOreTiers.Mythril = -1;
				WorldGen.SavedOreTiers.Adamantite = -1;
			}
			else
			{
				WorldGen.SavedOreTiers.Cobalt = 107;
				WorldGen.SavedOreTiers.Mythril = 108;
				WorldGen.SavedOreTiers.Adamantite = 111;
			}
			int style = 0;
			int style2 = 0;
			int style3 = 0;
			int style4 = 0;
			int style5 = 0;
			int style6 = 0;
			int style7 = 0;
			int style8 = 0;
			int style9 = 0;
			int style10 = 0;
			if (versionNumber >= 55)
			{
				style = fileIO.ReadByte();
				style2 = fileIO.ReadByte();
				style3 = fileIO.ReadByte();
			}
			if (versionNumber >= 60)
			{
				style4 = fileIO.ReadByte();
				style5 = fileIO.ReadByte();
				style6 = fileIO.ReadByte();
				style7 = fileIO.ReadByte();
				style8 = fileIO.ReadByte();
			}
			WorldGen.setBG(0, style);
			WorldGen.setBG(1, style2);
			WorldGen.setBG(2, style3);
			WorldGen.setBG(3, style4);
			WorldGen.setBG(4, style5);
			WorldGen.setBG(5, style6);
			WorldGen.setBG(6, style7);
			WorldGen.setBG(7, style8);
			WorldGen.setBG(8, style9);
			WorldGen.setBG(9, style10);
			WorldGen.setBG(10, style);
			WorldGen.setBG(11, style);
			WorldGen.setBG(12, style);
			if (versionNumber >= 60)
			{
				Main.cloudBGActive = fileIO.ReadInt32();
				if (Main.cloudBGActive >= 1f)
				{
					Main.cloudBGAlpha = 1f;
				}
				else
				{
					Main.cloudBGAlpha = 0f;
				}
			}
			else
			{
				Main.cloudBGActive = -WorldGen.genRand.Next(8640, 86400);
			}
			if (versionNumber >= 62)
			{
				Main.numClouds = fileIO.ReadInt16();
				Main.windSpeedTarget = fileIO.ReadSingle();
				Main.windSpeedCurrent = Main.windSpeedTarget;
			}
			else
			{
				WorldGen.RandomizeWeather();
			}
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				float num = (float)i / (float)Main.maxTilesX;
				Main.statusText = Lang.gen[51].Value + " " + (int)(num * 100f + 1f) + "%";
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					Tile tile = Main.tile[i, j];
					int num2 = -1;
					tile.active(fileIO.ReadBoolean());
					if (tile.active())
					{
						num2 = ((versionNumber <= 77) ? fileIO.ReadByte() : fileIO.ReadUInt16());
						tile.type = (ushort)num2;
						if (tile.type == 127 || tile.type == 504)
						{
							tile.active(active: false);
						}
						if (versionNumber < 72 && (tile.type == 35 || tile.type == 36 || tile.type == 170 || tile.type == 171 || tile.type == 172))
						{
							tile.frameX = fileIO.ReadInt16();
							tile.frameY = fileIO.ReadInt16();
						}
						else if (Main.tileFrameImportant[num2])
						{
							if (versionNumber < 28 && num2 == 4)
							{
								tile.frameX = 0;
								tile.frameY = 0;
							}
							else if (versionNumber < 40 && tile.type == 19)
							{
								tile.frameX = 0;
								tile.frameY = 0;
							}
							else if (versionNumber < 195 && tile.type == 49)
							{
								tile.frameX = 0;
								tile.frameY = 0;
							}
							else
							{
								tile.frameX = fileIO.ReadInt16();
								tile.frameY = fileIO.ReadInt16();
								if (tile.type == 144)
								{
									tile.frameY = 0;
								}
							}
						}
						else
						{
							tile.frameX = -1;
							tile.frameY = -1;
						}
						if (versionNumber >= 48 && fileIO.ReadBoolean())
						{
							tile.color(fileIO.ReadByte());
						}
					}
					if (versionNumber <= 25)
					{
						fileIO.ReadBoolean();
					}
					if (fileIO.ReadBoolean())
					{
						tile.wall = fileIO.ReadByte();
						if (tile.wall >= 347)
						{
							tile.wall = 0;
						}
						if (versionNumber >= 48 && fileIO.ReadBoolean())
						{
							tile.wallColor(fileIO.ReadByte());
						}
					}
					if (fileIO.ReadBoolean())
					{
						tile.liquid = fileIO.ReadByte();
						tile.lava(fileIO.ReadBoolean());
						if (versionNumber >= 51)
						{
							tile.honey(fileIO.ReadBoolean());
						}
					}
					if (versionNumber >= 33)
					{
						tile.wire(fileIO.ReadBoolean());
					}
					if (versionNumber >= 43)
					{
						tile.wire2(fileIO.ReadBoolean());
						tile.wire3(fileIO.ReadBoolean());
					}
					if (versionNumber >= 41)
					{
						tile.halfBrick(fileIO.ReadBoolean());
						if (!Main.tileSolid[tile.type] && !TileID.Sets.NonSolidSaveSlopes[tile.type])
						{
							tile.halfBrick(halfBrick: false);
						}
						if (versionNumber >= 49)
						{
							tile.slope(fileIO.ReadByte());
							if (!Main.tileSolid[tile.type] && !TileID.Sets.NonSolidSaveSlopes[tile.type])
							{
								tile.slope(0);
							}
						}
					}
					if (versionNumber >= 42)
					{
						tile.actuator(fileIO.ReadBoolean());
						tile.inActive(fileIO.ReadBoolean());
					}
					int num3 = 0;
					if (versionNumber >= 25)
					{
						num3 = fileIO.ReadInt16();
					}
					if (num2 != -1)
					{
						if ((double)j <= Main.worldSurface)
						{
							if ((double)(j + num3) <= Main.worldSurface)
							{
								WorldGen.tileCounts[num2] += (num3 + 1) * 5;
							}
							else
							{
								int num4 = (int)(Main.worldSurface - (double)j + 1.0);
								int num5 = num3 + 1 - num4;
								WorldGen.tileCounts[num2] += num4 * 5 + num5;
							}
						}
						else
						{
							WorldGen.tileCounts[num2] += num3 + 1;
						}
					}
					if (num3 > 0)
					{
						for (int k = j + 1; k < j + num3 + 1; k++)
						{
							Main.tile[i, k].CopyFrom(Main.tile[i, j]);
						}
						j += num3;
					}
				}
			}
			WorldGen.AddUpAlignmentCounts(clearCounts: true);
			if (versionNumber < 67)
			{
				WorldGen.FixSunflowers();
			}
			if (versionNumber < 72)
			{
				WorldGen.FixChands();
			}
			int num6 = 40;
			if (versionNumber < 58)
			{
				num6 = 20;
			}
			int num7 = 1000;
			for (int l = 0; l < num7; l++)
			{
				if (!fileIO.ReadBoolean())
				{
					continue;
				}
				Main.chest[l] = new Chest();
				Main.chest[l].x = fileIO.ReadInt32();
				Main.chest[l].y = fileIO.ReadInt32();
				if (versionNumber >= 85)
				{
					string text = fileIO.ReadString();
					if (text.Length > 20)
					{
						text = text.Substring(0, 20);
					}
					Main.chest[l].name = text;
				}
				for (int m = 0; m < 40; m++)
				{
					Main.chest[l].item[m] = new Item();
					if (m >= num6)
					{
						continue;
					}
					int num8 = 0;
					num8 = ((versionNumber < 59) ? fileIO.ReadByte() : fileIO.ReadInt16());
					if (num8 > 0)
					{
						if (versionNumber >= 38)
						{
							Main.chest[l].item[m].netDefaults(fileIO.ReadInt32());
						}
						else
						{
							short defaults = ItemID.FromLegacyName(fileIO.ReadString(), versionNumber);
							Main.chest[l].item[m].SetDefaults(defaults);
						}
						Main.chest[l].item[m].stack = num8;
						if (versionNumber >= 36)
						{
							Main.chest[l].item[m].Prefix(fileIO.ReadByte());
						}
					}
				}
			}
			for (int n = 0; n < 1000; n++)
			{
				if (fileIO.ReadBoolean())
				{
					string text2 = fileIO.ReadString();
					int num9 = fileIO.ReadInt32();
					int num10 = fileIO.ReadInt32();
					if (Main.tile[num9, num10].active() && (Main.tile[num9, num10].type == 55 || Main.tile[num9, num10].type == 85))
					{
						Main.sign[n] = new Sign();
						Main.sign[n].x = num9;
						Main.sign[n].y = num10;
						Main.sign[n].text = text2;
					}
				}
			}
			bool flag = fileIO.ReadBoolean();
			int num11 = 0;
			while (flag)
			{
				if (versionNumber >= 190)
				{
					Main.npc[num11].SetDefaults(fileIO.ReadInt32());
				}
				else
				{
					Main.npc[num11].SetDefaults(NPCID.FromLegacyName(fileIO.ReadString()));
				}
				if (versionNumber >= 83)
				{
					Main.npc[num11].GivenName = fileIO.ReadString();
				}
				Main.npc[num11].position.X = fileIO.ReadSingle();
				Main.npc[num11].position.Y = fileIO.ReadSingle();
				Main.npc[num11].homeless = fileIO.ReadBoolean();
				Main.npc[num11].homeTileX = fileIO.ReadInt32();
				Main.npc[num11].homeTileY = fileIO.ReadInt32();
				flag = fileIO.ReadBoolean();
				num11++;
			}
			if (versionNumber >= 31 && versionNumber <= 83)
			{
				NPC.setNPCName(fileIO.ReadString(), 17, resetExtras: true);
				NPC.setNPCName(fileIO.ReadString(), 18, resetExtras: true);
				NPC.setNPCName(fileIO.ReadString(), 19, resetExtras: true);
				NPC.setNPCName(fileIO.ReadString(), 20, resetExtras: true);
				NPC.setNPCName(fileIO.ReadString(), 22, resetExtras: true);
				NPC.setNPCName(fileIO.ReadString(), 54, resetExtras: true);
				NPC.setNPCName(fileIO.ReadString(), 38, resetExtras: true);
				NPC.setNPCName(fileIO.ReadString(), 107, resetExtras: true);
				NPC.setNPCName(fileIO.ReadString(), 108, resetExtras: true);
				if (versionNumber >= 35)
				{
					NPC.setNPCName(fileIO.ReadString(), 124, resetExtras: true);
					if (versionNumber >= 65)
					{
						NPC.setNPCName(fileIO.ReadString(), 160, resetExtras: true);
						NPC.setNPCName(fileIO.ReadString(), 178, resetExtras: true);
						NPC.setNPCName(fileIO.ReadString(), 207, resetExtras: true);
						NPC.setNPCName(fileIO.ReadString(), 208, resetExtras: true);
						NPC.setNPCName(fileIO.ReadString(), 209, resetExtras: true);
						NPC.setNPCName(fileIO.ReadString(), 227, resetExtras: true);
						NPC.setNPCName(fileIO.ReadString(), 228, resetExtras: true);
						NPC.setNPCName(fileIO.ReadString(), 229, resetExtras: true);
						if (versionNumber >= 79)
						{
							NPC.setNPCName(fileIO.ReadString(), 353, resetExtras: true);
						}
					}
				}
			}
			if (Main.invasionType > 0 && Main.invasionSize > 0)
			{
				Main.FakeLoadInvasionStart();
			}
			if (versionNumber >= 7)
			{
				bool num12 = fileIO.ReadBoolean();
				string text3 = fileIO.ReadString();
				int num13 = fileIO.ReadInt32();
				if (num12 && (text3 == Main.worldName || num13 == Main.worldID))
				{
					return 0;
				}
				return 2;
			}
			return 0;
		}
	}
}
