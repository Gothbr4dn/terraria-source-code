using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.Initializers
{
	public class WingStatsInitializer
	{
		public static void Load()
		{
			WingStats[] array = new WingStats[47];
			float flySpeedOverride = 3f;
			float flySpeedOverride2 = 6f;
			float flySpeedOverride3 = 6.25f;
			float flySpeedOverride4 = 6.5f;
			float flySpeedOverride5 = 6.75f;
			float flySpeedOverride6 = 7f;
			float flySpeedOverride7 = 7.5f;
			float flySpeedOverride8 = 8f;
			float flySpeedOverride9 = 9f;
			int flyTime = 25;
			int flyTime2 = 100;
			int flyTime3 = 130;
			int flyTime4 = 150;
			int flyTime5 = 160;
			int flyTime6 = 170;
			int flyTime7 = 180;
			int flyTime8 = 150;
			array[46] = new WingStats(flyTime, flySpeedOverride);
			array[1] = new WingStats(flyTime2, flySpeedOverride3);
			array[2] = new WingStats(flyTime2, flySpeedOverride3);
			array[25] = new WingStats(flyTime3, flySpeedOverride5);
			array[7] = new WingStats(flyTime3, flySpeedOverride5);
			array[6] = new WingStats(flyTime3, flySpeedOverride5);
			array[10] = new WingStats(flyTime3, flySpeedOverride5);
			array[4] = new WingStats(flyTime4, flySpeedOverride4);
			array[15] = new WingStats(flyTime5, flySpeedOverride7);
			array[5] = new WingStats(flyTime5, flySpeedOverride7);
			array[14] = new WingStats(flyTime5, flySpeedOverride7);
			array[9] = new WingStats(flyTime5, flySpeedOverride7);
			array[13] = new WingStats(flyTime5, flySpeedOverride7);
			array[11] = new WingStats(flyTime6, flySpeedOverride7);
			array[8] = new WingStats(flyTime6, flySpeedOverride7);
			array[27] = new WingStats(flyTime6, flySpeedOverride7);
			array[24] = new WingStats(flyTime6, flySpeedOverride7);
			array[22] = new WingStats(flyTime6, flySpeedOverride4, 1f, hasHoldDownHoverFeatures: true, 10f, 10f);
			array[21] = new WingStats(flyTime7, flySpeedOverride7);
			array[20] = new WingStats(flyTime7, flySpeedOverride7);
			array[12] = new WingStats(flyTime7, flySpeedOverride7);
			array[23] = new WingStats(flyTime7, flySpeedOverride7);
			array[26] = new WingStats(flyTime7, flySpeedOverride8, 2f);
			array[45] = new WingStats(flyTime7, flySpeedOverride8, 4.5f, hasHoldDownHoverFeatures: true, 16f, 16f);
			array[37] = new WingStats(flyTime4, flySpeedOverride6, 2.5f, hasHoldDownHoverFeatures: true, 12f, 12f);
			array[44] = new WingStats(flyTime4, flySpeedOverride8, 2f);
			new WingStats(flyTime4, flySpeedOverride2, 2.5f, hasHoldDownHoverFeatures: true, 12f, 12f);
			array[29] = new WingStats(flyTime7, flySpeedOverride9, 2.5f);
			array[32] = new WingStats(flyTime7, flySpeedOverride9, 2.5f);
			array[30] = new WingStats(flyTime7, flySpeedOverride4, 1.5f, hasHoldDownHoverFeatures: true, 12f, 12f);
			array[31] = new WingStats(flyTime7, flySpeedOverride4, 1.5f, hasHoldDownHoverFeatures: true, 12f, 12f);
			array[43] = (array[41] = (array[42] = (array[40] = (array[39] = (array[38] = (array[36] = (array[35] = (array[34] = (array[33] = (array[28] = (array[19] = (array[18] = (array[17] = (array[16] = (array[3] = new WingStats(flyTime8, flySpeedOverride6))))))))))))))));
			ArmorIDs.Wing.Sets.Stats = array;
		}
	}
}
