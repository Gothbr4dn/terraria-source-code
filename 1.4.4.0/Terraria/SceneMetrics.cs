using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria
{
	public class SceneMetrics
	{
		public static int ShimmerTileThreshold = 300;

		public static int CorruptionTileThreshold = 300;

		public static int CorruptionTileMax = 1000;

		public static int CrimsonTileThreshold = 300;

		public static int CrimsonTileMax = 1000;

		public static int HallowTileThreshold = 125;

		public static int HallowTileMax = 600;

		public static int JungleTileThreshold = 140;

		public static int JungleTileMax = 700;

		public static int SnowTileThreshold = 1500;

		public static int SnowTileMax = 6000;

		public static int DesertTileThreshold = 1500;

		public static int MushroomTileThreshold = 100;

		public static int MushroomTileMax = 160;

		public static int MeteorTileThreshold = 75;

		public static int GraveyardTileMax = 36;

		public static int GraveyardTileMin = 16;

		public static int GraveyardTileThreshold = 28;

		public bool CanPlayCreditsRoll;

		public bool[] NPCBannerBuff = new bool[290];

		public bool hasBanner;

		private readonly int[] _tileCounts = new int[693];

		private readonly int[] _liquidCounts = new int[4];

		private readonly List<Point> _oreFinderTileLocations = new List<Point>(512);

		public int bestOre;

		public Point? ClosestOrePosition { get; private set; }

		public int ShimmerTileCount { get; set; }

		public int EvilTileCount { get; set; }

		public int HolyTileCount { get; set; }

		public int HoneyBlockCount { get; set; }

		public int ActiveMusicBox { get; set; }

		public int SandTileCount { get; private set; }

		public int MushroomTileCount { get; private set; }

		public int SnowTileCount { get; private set; }

		public int WaterCandleCount { get; private set; }

		public int PeaceCandleCount { get; private set; }

		public int ShadowCandleCount { get; private set; }

		public int PartyMonolithCount { get; private set; }

		public int MeteorTileCount { get; private set; }

		public int BloodTileCount { get; private set; }

		public int JungleTileCount { get; private set; }

		public int DungeonTileCount { get; private set; }

		public bool HasSunflower { get; private set; }

		public bool HasGardenGnome { get; private set; }

		public bool HasClock { get; private set; }

		public bool HasCampfire { get; private set; }

		public bool HasStarInBottle { get; private set; }

		public bool HasHeartLantern { get; private set; }

		public int ActiveFountainColor { get; private set; }

		public int ActiveMonolithType { get; private set; }

		public bool BloodMoonMonolith { get; private set; }

		public bool MoonLordMonolith { get; private set; }

		public bool EchoMonolith { get; private set; }

		public int ShimmerMonolithState { get; private set; }

		public bool HasCatBast { get; private set; }

		public int GraveyardTileCount { get; private set; }

		public bool EnoughTilesForShimmer => ShimmerTileCount >= ShimmerTileThreshold;

		public bool EnoughTilesForJungle => JungleTileCount >= JungleTileThreshold;

		public bool EnoughTilesForHallow => HolyTileCount >= HallowTileThreshold;

		public bool EnoughTilesForSnow => SnowTileCount >= SnowTileThreshold;

		public bool EnoughTilesForGlowingMushroom => MushroomTileCount >= MushroomTileThreshold;

		public bool EnoughTilesForDesert => SandTileCount >= DesertTileThreshold;

		public bool EnoughTilesForCorruption => EvilTileCount >= CorruptionTileThreshold;

		public bool EnoughTilesForCrimson => BloodTileCount >= CrimsonTileThreshold;

		public bool EnoughTilesForMeteor => MeteorTileCount >= MeteorTileThreshold;

		public bool EnoughTilesForGraveyard => GraveyardTileCount >= GraveyardTileThreshold;

		public SceneMetrics()
		{
			Reset();
		}

		public void ScanAndExportToMain(SceneMetricsScanSettings settings)
		{
			Reset();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (settings.ScanOreFinderData)
			{
				_oreFinderTileLocations.Clear();
			}
			if (settings.BiomeScanCenterPositionInWorld.HasValue)
			{
				Point point = settings.BiomeScanCenterPositionInWorld.Value.ToTileCoordinates();
				Rectangle tileRectangle = new Rectangle(point.X - Main.buffScanAreaWidth / 2, point.Y - Main.buffScanAreaHeight / 2, Main.buffScanAreaWidth, Main.buffScanAreaHeight);
				tileRectangle = WorldUtils.ClampToWorld(tileRectangle);
				for (int i = tileRectangle.Left; i < tileRectangle.Right; i++)
				{
					for (int j = tileRectangle.Top; j < tileRectangle.Bottom; j++)
					{
						if (!tileRectangle.Contains(i, j))
						{
							continue;
						}
						Tile tile = Main.tile[i, j];
						if (tile == null)
						{
							continue;
						}
						if (!tile.active())
						{
							if (tile.liquid > 0)
							{
								_liquidCounts[tile.liquidType()]++;
							}
							continue;
						}
						tileRectangle.Contains(i, j);
						if (!TileID.Sets.isDesertBiomeSand[tile.type] || !WorldGen.oceanDepths(i, j))
						{
							_tileCounts[tile.type]++;
						}
						if (tile.type == 215 && tile.frameY < 36)
						{
							HasCampfire = true;
						}
						if (tile.type == 49 && tile.frameX < 18)
						{
							num++;
						}
						if (tile.type == 372 && tile.frameX < 18)
						{
							num2++;
						}
						if (tile.type == 646 && tile.frameX < 18)
						{
							num3++;
						}
						if (tile.type == 405 && tile.frameX < 54)
						{
							HasCampfire = true;
						}
						if (tile.type == 506 && tile.frameX < 72)
						{
							HasCatBast = true;
						}
						if (tile.type == 42 && tile.frameY >= 324 && tile.frameY <= 358)
						{
							HasHeartLantern = true;
						}
						if (tile.type == 42 && tile.frameY >= 252 && tile.frameY <= 286)
						{
							HasStarInBottle = true;
						}
						if (tile.type == 91 && (tile.frameX >= 396 || tile.frameY >= 54))
						{
							int num4 = tile.frameX / 18 - 21;
							for (int num5 = tile.frameY; num5 >= 54; num5 -= 54)
							{
								num4 += 90;
								num4 += 21;
							}
							int num6 = Item.BannerToItem(num4);
							if (ItemID.Sets.BannerStrength.IndexInRange(num6) && ItemID.Sets.BannerStrength[num6].Enabled)
							{
								NPCBannerBuff[num4] = true;
								hasBanner = true;
							}
						}
						if (settings.ScanOreFinderData && Main.tileOreFinderPriority[tile.type] != 0)
						{
							_oreFinderTileLocations.Add(new Point(i, j));
						}
					}
				}
			}
			if (settings.VisualScanArea.HasValue)
			{
				Rectangle rectangle = WorldUtils.ClampToWorld(settings.VisualScanArea.Value);
				for (int k = rectangle.Left; k < rectangle.Right; k++)
				{
					for (int l = rectangle.Top; l < rectangle.Bottom; l++)
					{
						Tile tile2 = Main.tile[k, l];
						if (tile2 == null || !tile2.active())
						{
							continue;
						}
						if (tile2.type == 104)
						{
							HasClock = true;
						}
						switch (tile2.type)
						{
						case 139:
							if (tile2.frameX >= 36)
							{
								ActiveMusicBox = tile2.frameY / 36;
							}
							break;
						case 207:
							if (tile2.frameY >= 72)
							{
								switch (tile2.frameX / 36)
								{
								case 0:
									ActiveFountainColor = 0;
									break;
								case 1:
									ActiveFountainColor = 12;
									break;
								case 2:
									ActiveFountainColor = 3;
									break;
								case 3:
									ActiveFountainColor = 5;
									break;
								case 4:
									ActiveFountainColor = 2;
									break;
								case 5:
									ActiveFountainColor = 10;
									break;
								case 6:
									ActiveFountainColor = 4;
									break;
								case 7:
									ActiveFountainColor = 9;
									break;
								case 8:
									ActiveFountainColor = 8;
									break;
								case 9:
									ActiveFountainColor = 6;
									break;
								default:
									ActiveFountainColor = -1;
									break;
								}
							}
							break;
						case 410:
							if (tile2.frameY >= 56)
							{
								int num10 = (ActiveMonolithType = tile2.frameX / 36);
							}
							break;
						case 509:
							if (tile2.frameY >= 56)
							{
								ActiveMonolithType = 4;
							}
							break;
						case 480:
							if (tile2.frameY >= 54)
							{
								BloodMoonMonolith = true;
							}
							break;
						case 657:
							if (tile2.frameY >= 54)
							{
								EchoMonolith = true;
							}
							break;
						case 658:
						{
							int num8 = (ShimmerMonolithState = tile2.frameY / 54);
							break;
						}
						}
					}
				}
			}
			WaterCandleCount = num;
			PeaceCandleCount = num2;
			ShadowCandleCount = num3;
			ExportTileCountsToMain();
			CanPlayCreditsRoll = ActiveMusicBox == 85;
			if (settings.ScanOreFinderData)
			{
				UpdateOreFinderData();
			}
		}

		private void ExportTileCountsToMain()
		{
			if (_tileCounts[27] > 0)
			{
				HasSunflower = true;
			}
			if (_tileCounts[567] > 0)
			{
				HasGardenGnome = true;
			}
			ShimmerTileCount = _liquidCounts[3];
			HoneyBlockCount = _tileCounts[229];
			HolyTileCount = _tileCounts[109] + _tileCounts[492] + _tileCounts[110] + _tileCounts[113] + _tileCounts[117] + _tileCounts[116] + _tileCounts[164] + _tileCounts[403] + _tileCounts[402];
			SnowTileCount = _tileCounts[147] + _tileCounts[148] + _tileCounts[161] + _tileCounts[162] + _tileCounts[164] + _tileCounts[163] + _tileCounts[200];
			if (Main.remixWorld)
			{
				JungleTileCount = _tileCounts[60] + _tileCounts[61] + _tileCounts[62] + _tileCounts[74] + _tileCounts[225];
				EvilTileCount = _tileCounts[23] + _tileCounts[661] + _tileCounts[24] + _tileCounts[25] + _tileCounts[32] + _tileCounts[112] + _tileCounts[163] + _tileCounts[400] + _tileCounts[398] + -10 * _tileCounts[27] + _tileCounts[474];
				BloodTileCount = _tileCounts[199] + _tileCounts[662] + _tileCounts[203] + _tileCounts[200] + _tileCounts[401] + _tileCounts[399] + _tileCounts[234] + _tileCounts[352] - 10 * _tileCounts[27] + _tileCounts[195];
			}
			else
			{
				JungleTileCount = _tileCounts[60] + _tileCounts[61] + _tileCounts[62] + _tileCounts[74] + _tileCounts[226] + _tileCounts[225];
				EvilTileCount = _tileCounts[23] + _tileCounts[661] + _tileCounts[24] + _tileCounts[25] + _tileCounts[32] + _tileCounts[112] + _tileCounts[163] + _tileCounts[400] + _tileCounts[398] + -10 * _tileCounts[27];
				BloodTileCount = _tileCounts[199] + _tileCounts[662] + _tileCounts[203] + _tileCounts[200] + _tileCounts[401] + _tileCounts[399] + _tileCounts[234] + _tileCounts[352] - 10 * _tileCounts[27];
			}
			MushroomTileCount = _tileCounts[70] + _tileCounts[71] + _tileCounts[72] + _tileCounts[528];
			MeteorTileCount = _tileCounts[37];
			DungeonTileCount = _tileCounts[41] + _tileCounts[43] + _tileCounts[44] + _tileCounts[481] + _tileCounts[482] + _tileCounts[483];
			SandTileCount = _tileCounts[53] + _tileCounts[112] + _tileCounts[116] + _tileCounts[234] + _tileCounts[397] + _tileCounts[398] + _tileCounts[402] + _tileCounts[399] + _tileCounts[396] + _tileCounts[400] + _tileCounts[403] + _tileCounts[401];
			PartyMonolithCount = _tileCounts[455];
			GraveyardTileCount = _tileCounts[85];
			GraveyardTileCount -= _tileCounts[27] / 2;
			if (_tileCounts[27] > 0)
			{
				HasSunflower = true;
			}
			if (GraveyardTileCount > GraveyardTileMin)
			{
				HasSunflower = false;
			}
			if (GraveyardTileCount < 0)
			{
				GraveyardTileCount = 0;
			}
			if (HolyTileCount < 0)
			{
				HolyTileCount = 0;
			}
			if (EvilTileCount < 0)
			{
				EvilTileCount = 0;
			}
			if (BloodTileCount < 0)
			{
				BloodTileCount = 0;
			}
			int holyTileCount = HolyTileCount;
			HolyTileCount -= EvilTileCount;
			HolyTileCount -= BloodTileCount;
			EvilTileCount -= holyTileCount;
			BloodTileCount -= holyTileCount;
			if (HolyTileCount < 0)
			{
				HolyTileCount = 0;
			}
			if (EvilTileCount < 0)
			{
				EvilTileCount = 0;
			}
			if (BloodTileCount < 0)
			{
				BloodTileCount = 0;
			}
		}

		public int GetTileCount(ushort tileId)
		{
			return _tileCounts[tileId];
		}

		public void Reset()
		{
			Array.Clear(_tileCounts, 0, _tileCounts.Length);
			Array.Clear(_liquidCounts, 0, _liquidCounts.Length);
			SandTileCount = 0;
			EvilTileCount = 0;
			BloodTileCount = 0;
			GraveyardTileCount = 0;
			MushroomTileCount = 0;
			SnowTileCount = 0;
			HolyTileCount = 0;
			MeteorTileCount = 0;
			JungleTileCount = 0;
			DungeonTileCount = 0;
			HasCampfire = false;
			HasSunflower = false;
			HasGardenGnome = false;
			HasStarInBottle = false;
			HasHeartLantern = false;
			HasClock = false;
			HasCatBast = false;
			ActiveMusicBox = -1;
			WaterCandleCount = 0;
			ActiveFountainColor = -1;
			ActiveMonolithType = -1;
			bestOre = -1;
			BloodMoonMonolith = false;
			MoonLordMonolith = false;
			EchoMonolith = false;
			ShimmerMonolithState = 0;
			Array.Clear(NPCBannerBuff, 0, NPCBannerBuff.Length);
			hasBanner = false;
			CanPlayCreditsRoll = false;
		}

		private void UpdateOreFinderData()
		{
			int num = -1;
			foreach (Point oreFinderTileLocation in _oreFinderTileLocations)
			{
				Tile tile = Main.tile[oreFinderTileLocation.X, oreFinderTileLocation.Y];
				if (IsValidForOreFinder(tile) && (num < 0 || Main.tileOreFinderPriority[tile.type] > Main.tileOreFinderPriority[num]))
				{
					num = tile.type;
					ClosestOrePosition = oreFinderTileLocation;
				}
			}
			bestOre = num;
		}

		public static bool IsValidForOreFinder(Tile t)
		{
			if (t.type == 227 && (t.frameX < 272 || t.frameX > 374))
			{
				return false;
			}
			if (t.type == 129 && t.frameX < 324)
			{
				return false;
			}
			return Main.tileOreFinderPriority[t.type] > 0;
		}
	}
}
