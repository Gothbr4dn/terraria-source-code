using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.DataStructures;

namespace Terraria.WorldBuilding
{
	public static class GenVars
	{
		public static WorldGenConfiguration configuration;

		public static StructureMap structures;

		public static int copper;

		public static int iron;

		public static int silver;

		public static int gold;

		public static int copperBar = 20;

		public static int ironBar = 22;

		public static int silverBar = 21;

		public static int goldBar = 19;

		public static ushort mossTile = 179;

		public static ushort mossWall = 54;

		public static int lavaLine;

		public static int waterLine;

		public static double worldSurfaceLow;

		public static double worldSurface;

		public static double worldSurfaceHigh;

		public static double rockLayerLow;

		public static double rockLayer;

		public static double rockLayerHigh;

		public static int snowTop;

		public static int snowBottom;

		public static int snowOriginLeft;

		public static int snowOriginRight;

		public static int[] snowMinX;

		public static int[] snowMaxX;

		public static int leftBeachEnd;

		public static int rightBeachStart;

		public static int beachBordersWidth;

		public static int beachSandRandomCenter;

		public static int beachSandRandomWidthRange;

		public static int beachSandDungeonExtraWidth;

		public static int beachSandJungleExtraWidth;

		public static int shellStartXLeft;

		public static int shellStartYLeft;

		public static int shellStartXRight;

		public static int shellStartYRight;

		public static int oceanWaterStartRandomMin;

		public static int oceanWaterStartRandomMax;

		public static int oceanWaterForcedJungleLength;

		public static int evilBiomeBeachAvoidance;

		public static int evilBiomeAvoidanceMidFixer;

		public static int lakesBeachAvoidance;

		public static int smallHolesBeachAvoidance;

		public static int surfaceCavesBeachAvoidance;

		public static int surfaceCavesBeachAvoidance2;

		public static readonly int maxOceanCaveTreasure = 2;

		public static int numOceanCaveTreasure = 0;

		public static Point[] oceanCaveTreasure = new Point[maxOceanCaveTreasure];

		public static bool skipDesertTileCheck = false;

		public static Rectangle UndergroundDesertLocation = Rectangle.Empty;

		public static Rectangle UndergroundDesertHiveLocation = Rectangle.Empty;

		public static int desertHiveHigh;

		public static int desertHiveLow;

		public static int desertHiveLeft;

		public static int desertHiveRight;

		public static int numLarva;

		public static int[] larvaY = new int[100];

		public static int[] larvaX = new int[100];

		public static int numPyr;

		public static int[] PyrX;

		public static int[] PyrY;

		public static int extraBastStatueCount;

		public static int extraBastStatueCountMax;

		public static int jungleOriginX;

		public static int jungleMinX;

		public static int jungleMaxX;

		public static int JungleX;

		public static ushort jungleHut;

		public static bool mudWall;

		public static int JungleItemCount;

		public static int[] JChestX = new int[100];

		public static int[] JChestY = new int[100];

		public static int numJChests;

		public static int tLeft;

		public static int tRight;

		public static int tTop;

		public static int tBottom;

		public static int tRooms;

		public static int lAltarX;

		public static int lAltarY;

		public static int dungeonSide;

		public static int dungeonLocation;

		public static bool dungeonLake;

		public static ushort crackedType = 481;

		public static int dungeonX;

		public static int dungeonY;

		public static Vector2D lastDungeonHall = Vector2D.get_Zero();

		public static readonly int maxDRooms = 100;

		public static int numDRooms;

		public static int[] dRoomX = new int[maxDRooms];

		public static int[] dRoomY = new int[maxDRooms];

		public static int[] dRoomSize = new int[maxDRooms];

		public static bool[] dRoomTreasure = new bool[maxDRooms];

		public static int[] dRoomL = new int[maxDRooms];

		public static int[] dRoomR = new int[maxDRooms];

		public static int[] dRoomT = new int[maxDRooms];

		public static int[] dRoomB = new int[maxDRooms];

		public static int numDDoors;

		public static int[] DDoorX = new int[500];

		public static int[] DDoorY = new int[500];

		public static int[] DDoorPos = new int[500];

		public static int numDungeonPlatforms;

		public static int[] dungeonPlatformX = new int[500];

		public static int[] dungeonPlatformY = new int[500];

		public static int dEnteranceX;

		public static bool dSurface;

		public static double dxStrength1;

		public static double dyStrength1;

		public static double dxStrength2;

		public static double dyStrength2;

		public static int dMinX;

		public static int dMaxX;

		public static int dMinY;

		public static int dMaxY;

		public static int skyLakes;

		public static bool generatedShadowKey;

		public static int numIslandHouses;

		public static int skyIslandHouseCount;

		public static bool[] skyLake = new bool[30];

		public static int[] floatingIslandHouseX = new int[30];

		public static int[] floatingIslandHouseY = new int[30];

		public static int[] floatingIslandStyle = new int[30];

		public static int numMCaves;

		public static int[] mCaveX = new int[30];

		public static int[] mCaveY = new int[30];

		public static readonly int maxTunnels = 50;

		public static int numTunnels;

		public static int[] tunnelX = new int[maxTunnels];

		public static readonly int maxOrePatch = 50;

		public static int numOrePatch;

		public static int[] orePatchX = new int[maxOrePatch];

		public static readonly int maxMushroomBiomes = 50;

		public static int numMushroomBiomes = 0;

		public static Point[] mushroomBiomesPosition = new Point[maxMushroomBiomes];

		public static int logX;

		public static int logY;

		public static readonly int maxLakes = 50;

		public static int numLakes = 0;

		public static int[] LakeX = new int[maxLakes];

		public static readonly int maxOasis = 20;

		public static int numOasis = 0;

		public static Point[] oasisPosition = new Point[maxOasis];

		public static int[] oasisWidth = new int[maxOasis];

		public static readonly int oasisHeight = 20;

		public static int hellChest;

		public static int[] hellChestItem;

		public static Point16[] statueList;

		public static List<int> StatuesWithTraps = new List<int>(new int[4] { 4, 7, 10, 18 });

		public static bool crimsonLeft = true;

		public static Vector2D shimmerPosition;
	}
}
