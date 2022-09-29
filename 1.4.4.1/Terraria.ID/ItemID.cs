using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Reflection;
using Terraria.DataStructures;

namespace Terraria.ID
{
	public class ItemID
	{
		public struct BannerEffect
		{
			public static readonly BannerEffect None = new BannerEffect(0f);

			public static readonly BannerEffect Reduced = new BannerEffect(0.2f);

			public readonly float NormalDamageDealt;

			public readonly float ExpertDamageDealt;

			public readonly float NormalDamageReceived;

			public readonly float ExpertDamageReceived;

			public readonly bool Enabled;

			public BannerEffect(float strength = 1f)
			{
				NormalDamageDealt = 1f + strength * 0.5f;
				ExpertDamageDealt = 1f + strength;
				ExpertDamageReceived = 1f / (strength + 1f);
				NormalDamageReceived = 1f - (1f - ExpertDamageReceived) * 0.5f;
				Enabled = strength != 0f;
			}

			public BannerEffect(float normalDamageDealt, float expertDamageDealt, float normalDamageReceived, float expertDamageReceived)
			{
				NormalDamageDealt = normalDamageDealt;
				ExpertDamageDealt = expertDamageDealt;
				NormalDamageReceived = normalDamageReceived;
				ExpertDamageReceived = expertDamageReceived;
				Enabled = true;
			}
		}

		public class Sets
		{
			public static SetFactory Factory = new SetFactory(5453);

			public static List<int> ItemsThatAreProcessedAfterNormalContentSample = new List<int> { 1533, 1534, 1535, 1536, 1537 };

			public static bool[] IsLavaImmuneRegardlessOfRarity = Factory.CreateBoolSet(false, 318, 312, 173, 174, 175, 4422, 2701, 205, 206, 207, 1128, 2340, 2739, 2492, 1127, 85);

			public static bool[] ItemsThatAllowRepeatedRightClick = Factory.CreateBoolSet(false, 3384, 3858, 3852);

			public static bool[] ItemsThatCountAsBombsForDemolitionistToSpawn = Factory.CreateBoolSet(false, 168, 2586, 3116, 166, 235, 3115, 167, 2896, 3547, 3196, 4423, 1130, 1168, 4824, 4825, 4826, 4827, 4908, 4909);

			public static bool[] ItemsThatShouldNotBeInInventory = Factory.CreateBoolSet(false, 58, 184, 1734, 1735, 1867, 1868, 3453, 3454, 3455, 5013);

			public static bool[] DrawUnsafeIndicator = Factory.CreateBoolSet(false, 5363, 5365, 5366, 5367, 5368, 5369, 5370, 5371, 5372, 5373, 5376, 5375, 5374, 3988, 5384);

			public static bool[] WorksInVoidBag = Factory.CreateBoolSet(true, 4346, 5095);

			public static int[] ShimmerTransformToItem = Factory.CreateIntSet(-1, 3460, 947, 947, 1106, 1106, 366, 366, 1105, 1105, 365, 365, 1104, 1104, 364, 364, 702, 702, 13, 13, 701, 701, 14, 14, 700, 700, 11, 11, 699, 699, 12, 12, 3, 3, 2, 182, 178, 178, 179, 179, 177, 177, 180, 180, 181, 181, 3, 4843, 182, 4836, 182, 4842, 178, 4835, 178, 4841, 179, 4834, 179, 4840, 177, 4833, 177, 4832, 180, 4839, 180, 4838, 181, 4831, 181, 4844, 999, 4837, 999, 620, 9, 619, 9, 911, 9, 621, 9, 2503, 9, 2504, 9, 2260, 9, 1729, 9, 5215, 9, 9, 2, 3271, 169, 3272, 169, 3276, 370, 3274, 370, 3339, 408, 3338, 408, 3277, 1246, 3275, 1246, 1127, 1124, 1125, 1124, 4503, 5363, 134, 5417, 137, 5419, 139, 5421, 141, 5423, 143, 5425, 145, 5427, 415, 5429, 416, 5431, 192, 5433, 214, 5435, 135, 5365, 1379, 5367, 1378, 5366, 140, 5368, 1381, 5370, 1380, 5369, 138, 5371, 1383, 5373, 1382, 5372, 1102, 5376, 3340, 5375, 3273, 5374, 664, 593, 3982, 3203, 3983, 3204, 3984, 3205, 3985, 3206, 4406, 4405, 3981, 2336, 3986, 3207, 3980, 2335, 3987, 3208, 4878, 4877, 4408, 4407, 5003, 5002, 3979, 2334, 3064, 5381, 3086, 3081, 3081, 3086, 1534, 1529, 1535, 1530, 1536, 1531, 1537, 1532, 4714, 4712, 1533, 1528, 206, 207, 207, 1128, 1128, 206, 832, 4281, 3818, 3824, 3824, 3832, 3832, 3829, 3829, 3818, 3819, 3825, 3825, 3833, 3833, 3830, 3830, 3819, 3820, 3826, 3826, 3834, 3834, 3831, 3831, 3820, 960, 228, 961, 229, 962, 230, 228, 960, 229, 961, 230, 962, 956, 102, 957, 101, 958, 100, 102, 956, 101, 957, 100, 958, 4897, 553, 4898, 558, 4896, 559, 4899, 4873, 4900, 551, 4901, 552, 553, 4897, 558, 4898, 559, 4896, 4873, 4899, 551, 4900, 552, 4901, 959, 151, 151, 959, 955, 92, 92, 955, 954, 90, 90, 954, 3093, 4345, 4345, 3093, 215, 5346, 5356, 5346, 3000, 2999, 2999, 3000, 411, 410, 410, 411, 1725, 276, 276, 1725, 2886, 66, 67, 66, 195, 194, 194, 195, 4389, 5128, 4377, 5128, 4378, 5128, 4354, 5128, 5127, 5128, 8, 5353, 966, 5357, 52, 5347, 280, 277, 1304, 215, 40, 5348, 265, 5348, 931, 5380, 1614, 5380, 848, 857, 866, 934, 532, 5355, 3225, 159, 3120, 3096, 3096, 3037, 3037, 3120, 2373, 2374, 2374, 2375, 2375, 2373, 855, 3033, 854, 855, 3033, 854, 490, 491, 491, 489, 489, 2998, 2998, 490, 892, 886, 886, 892, 885, 887, 887, 885, 891, 890, 890, 891, 893, 889, 889, 893, 888, 3781, 3781, 888, 1322, 906, 531, 5336, 29, 5337, 1291, 5338, 109, 5339, 4414, 5340, 2895, 5341, 2222, 5343, 4009, 5342, 4282, 5342, 4290, 5342, 4291, 5342, 4293, 5342, 4286, 5342, 4295, 5342, 4284, 5342, 4289, 5342, 4285, 5342, 4296, 5342, 4292, 5342, 4294, 5342, 4283, 5342, 4287, 5342, 4288, 5342, 4297, 5342, 5278, 5342, 5277, 5342);

			public static int[] NewItemSpawnPriority = Factory.CreateIntSet(0, 2, 200, 3, 150, 61, 150, 836, 150, 409, 150, 593, 200, 664, 100, 834, 100, 833, 100, 835, 100, 169, 100, 370, 100, 1246, 100, 408, 100, 3271, 150, 3277, 150, 3339, 150, 3276, 150, 3272, 150, 3274, 150, 3275, 150, 3338, 150, 176, 100, 172, 200, 424, 50, 1103, 50, 3087, 100, 3066, 100);

			public static int[] ItemsForStuffCannon = new int[50]
			{
				2, 3, 61, 836, 409, 593, 664, 169, 370, 1246,
				408, 3271, 3277, 3339, 3276, 3272, 3274, 3275, 3338, 176,
				172, 424, 1103, 3087, 3066, 9, 2503, 2504, 619, 911,
				621, 620, 1727, 276, 4564, 751, 1124, 1125, 824, 129,
				131, 607, 594, 883, 414, 413, 609, 4050, 192, 412
			};

			public static bool?[] CanBeQuickusedOnGamepad = Factory.CreateCustomSet<bool?>(null, new object[30]
			{
				(short)50,
				true,
				(short)3199,
				true,
				(short)3124,
				true,
				(short)2350,
				true,
				(short)2351,
				true,
				(short)29,
				true,
				(short)109,
				true,
				(short)1291,
				true,
				(short)4870,
				true,
				(short)5358,
				true,
				(short)5359,
				true,
				(short)5360,
				true,
				(short)5361,
				true,
				(short)4263,
				true,
				(short)4819,
				true
			});

			public static bool?[] ForcesBreaksSleeping = Factory.CreateCustomSet<bool?>(null, new object[6]
			{
				(short)1991,
				true,
				(short)4821,
				true,
				(short)3183,
				true
			});

			public static bool[] SkipsInitialUseSound = Factory.CreateBoolSet(false, 2350, 4870);

			public static bool[] UsesCursedByPlanteraTooltip = Factory.CreateBoolSet(false, 1533, 1534, 1535, 1536, 1537, 4714);

			public static bool[] IsAKite = Factory.CreateBoolSet(false, 4367, 4368, 4369, 4370, 4371, 4379, 4610, 4611, 4612, 4613, 4648, 4649, 4650, 4651, 4669, 4670, 4671, 4674, 4675, 4676, 4677, 4681, 4683, 4684);

			public static bool?[] ForceConsumption = Factory.CreateCustomSet<bool?>(null, new object[12]
			{
				(short)2350,
				false,
				(short)4870,
				false,
				(short)2351,
				false,
				(short)2756,
				false,
				(short)4343,
				true,
				(short)4344,
				true
			});

			public static bool[] HasAProjectileThatHasAUsabilityCheck = Factory.CreateBoolSet(false, 4367, 4368, 4369, 4370, 4371, 4379, 4610, 4611, 4612, 4613, 4648, 4649, 4650, 4651, 4669, 4670, 4671, 4674, 4675, 4676, 4677, 4681, 4683, 4684);

			public static bool[] CanGetPrefixes = Factory.CreateBoolSet(true, 267, 562, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 574, 576, 1307, 1596, 1597, 1598, 1599, 1600, 1601, 1602, 1603, 1604, 1605, 1606, 1607, 1608, 1609, 1610, 3538, 3537, 3536, 3539, 4318, 4054, 5345, 5347);

			public static List<int> NonColorfulDyeItems = new List<int> { 3599, 3530, 3534 };

			public static bool[] ColorfulDyeValues = new bool[0];

			public static FlowerPacketInfo[] flowerPacketInfo = Factory.CreateCustomSet<FlowerPacketInfo>(null, new object[18]
			{
				(short)4041,
				new FlowerPacketInfo
				{
					stylesOnPurity = { 9, 16, 20 }
				},
				(short)4042,
				new FlowerPacketInfo
				{
					stylesOnPurity = { 6, 30, 31, 32 }
				},
				(short)4043,
				new FlowerPacketInfo
				{
					stylesOnPurity = { 7, 17, 33, 34, 35 }
				},
				(short)4044,
				new FlowerPacketInfo
				{
					stylesOnPurity = { 19, 21, 22, 23, 39, 40, 41 }
				},
				(short)4045,
				new FlowerPacketInfo
				{
					stylesOnPurity = { 10, 11, 13, 18, 24, 25, 26 }
				},
				(short)4046,
				new FlowerPacketInfo
				{
					stylesOnPurity = { 12, 42, 43, 44 }
				},
				(short)4047,
				new FlowerPacketInfo
				{
					stylesOnPurity = { 14, 15, 27, 28, 29, 36, 37, 38 }
				},
				(short)4241,
				new FlowerPacketInfo
				{
					stylesOnPurity = 
					{
						6, 7, 9, 10, 11, 12, 13, 14, 15, 16,
						17, 18, 19, 20, 21, 22, 23, 24, 25, 26,
						27, 28, 29, 30, 31, 32, 33, 34, 35, 36,
						37, 38, 39, 40, 41, 42, 43, 44
					}
				},
				(short)4048,
				new FlowerPacketInfo
				{
					stylesOnPurity = { 0, 1, 2, 3, 4, 5 }
				}
			});

			public static bool[] IsAMaterial = Factory.CreateBoolSet();

			public static int[] IsCrafted = Factory.CreateIntSet();

			public static int[] IsCraftedCrimson = Factory.CreateIntSet();

			public static int[] IsCraftedCorruption = Factory.CreateIntSet();

			public static bool[] IgnoresEncumberingStone = Factory.CreateBoolSet(58, 184, 1734, 1735, 1867, 1868, 3453, 3454, 3455, 4143);

			public static float[] ToolTipDamageMultiplier = Factory.CreateFloatSet(1f, 162f, 2f, 801f, 2f, 163f, 2f, 220f, 2f, 389f, 2f, 1259f, 2f, 4272f, 2f, 5011f, 2f, 5012f, 2f);

			public static bool[] IsAPickup = Factory.CreateBoolSet(58, 184, 1734, 1735, 1867, 1868, 3453, 3454, 3455);

			public static bool[] IsDrill = Factory.CreateBoolSet(388, 1231, 385, 386, 2779, 1196, 1189, 2784, 3464, 1203, 2774, 579);

			public static bool[] IsChainsaw = Factory.CreateBoolSet(387, 3098, 1232, 383, 384, 2778, 1197, 1190, 2783, 3463, 1204, 2773, 2342, 579);

			public static bool[] IsPaintScraper = Factory.CreateBoolSet(1100, 1545);

			public static bool[] SummonerWeaponThatScalesWithAttackSpeed = Factory.CreateBoolSet(4672, 4679, 4680, 4678, 4913, 4912, 4911, 4914, 5074);

			public static bool[] IsFood = Factory.CreateBoolSet(353, 357, 1787, 1911, 1912, 1919, 1920, 2266, 2267, 2268, 2425, 2426, 2427, 3195, 3532, 4009, 4010, 4011, 4012, 4013, 4014, 4015, 4016, 4017, 4018, 4019, 4020, 4021, 4022, 4023, 4024, 4025, 4026, 4027, 4028, 4029, 4030, 4031, 4032, 4033, 4034, 4035, 4036, 4037, 967, 969, 4282, 4283, 4284, 4285, 4286, 4287, 4288, 4289, 4290, 4291, 4292, 4293, 4294, 4295, 4296, 4297, 4403, 4411, 4614, 4615, 4616, 4617, 4618, 4619, 4620, 4621, 4622, 4623, 4624, 4625, 5009, 5042, 5041, 5092, 5093, 5275, 5277, 5278);

			public static Color[][] FoodParticleColors = Factory.CreateCustomSet(new Color[0], (short)357, new Color[2]
			{
				new Color(253, 209, 77),
				new Color(253, 178, 78)
			}, (short)1787, new Color[3]
			{
				new Color(215, 146, 96),
				new Color(250, 160, 15),
				new Color(226, 130, 33)
			}, (short)1911, new Color[4]
			{
				new Color(219, 219, 213),
				new Color(255, 228, 133),
				new Color(237, 159, 85),
				new Color(207, 32, 51)
			}, (short)1919, new Color[4]
			{
				new Color(206, 168, 119),
				new Color(73, 182, 126),
				new Color(230, 89, 92),
				new Color(228, 238, 241)
			}, (short)1920, new Color[4]
			{
				new Color(218, 167, 69),
				new Color(204, 209, 219),
				new Color(204, 22, 40),
				new Color(0, 212, 47)
			}, (short)2267, new Color[3]
			{
				new Color(229, 129, 82),
				new Color(255, 223, 126),
				new Color(190, 226, 65)
			}, (short)2268, new Color[3]
			{
				new Color(250, 232, 220),
				new Color(216, 189, 157),
				new Color(190, 226, 65)
			}, (short)2425, new Color[4]
			{
				new Color(199, 166, 129),
				new Color(127, 105, 81),
				new Color(128, 151, 43),
				new Color(193, 14, 7)
			}, (short)2426, new Color[2]
			{
				new Color(246, 187, 165),
				new Color(255, 134, 86)
			}, (short)2427, new Color[3]
			{
				new Color(235, 122, 128),
				new Color(216, 193, 186),
				new Color(252, 108, 40)
			}, (short)3195, new Color[4]
			{
				new Color(139, 86, 218),
				new Color(218, 86, 104),
				new Color(218, 182, 86),
				new Color(36, 203, 185)
			}, (short)3532, new Color[2]
			{
				new Color(218, 113, 90),
				new Color(183, 65, 68)
			}, (short)4009, new Color[2]
			{
				new Color(221, 67, 87),
				new Color(255, 252, 217)
			}, (short)4011, new Color[2]
			{
				new Color(224, 143, 91),
				new Color(214, 170, 105)
			}, (short)4012, new Color[4]
			{
				new Color(255, 236, 184),
				new Color(242, 183, 236),
				new Color(215, 137, 122),
				new Color(242, 70, 88)
			}, (short)4013, new Color[2]
			{
				new Color(216, 93, 61),
				new Color(159, 48, 28)
			}, (short)4014, new Color[3]
			{
				new Color(216, 93, 61),
				new Color(205, 150, 71),
				new Color(123, 72, 27)
			}, (short)4015, new Color[4]
			{
				new Color(197, 136, 85),
				new Color(143, 86, 59),
				new Color(100, 156, 58),
				new Color(216, 93, 61)
			}, (short)4016, new Color[2]
			{
				new Color(241, 167, 70),
				new Color(215, 121, 64)
			}, (short)4017, new Color[3]
			{
				new Color(200, 133, 84),
				new Color(141, 71, 19),
				new Color(103, 54, 18)
			}, (short)4019, new Color[3]
			{
				new Color(248, 234, 196),
				new Color(121, 92, 18),
				new Color(128, 151, 43)
			}, (short)4020, new Color[2]
			{
				new Color(237, 243, 248),
				new Color(255, 200, 82)
			}, (short)4021, new Color[3]
			{
				new Color(255, 221, 119),
				new Color(241, 167, 70),
				new Color(215, 121, 64)
			}, (short)4022, new Color[3]
			{
				new Color(255, 249, 181),
				new Color(203, 179, 73),
				new Color(216, 93, 61)
			}, (short)4023, new Color[2]
			{
				new Color(189, 0, 107) * 0.5f,
				new Color(123, 0, 57) * 0.5f
			}, (short)4024, new Color[2]
			{
				new Color(217, 134, 83),
				new Color(179, 80, 54)
			}, (short)4025, new Color[3]
			{
				new Color(229, 114, 63),
				new Color(255, 184, 51),
				new Color(197, 136, 85)
			}, (short)4026, new Color[4]
			{
				new Color(245, 247, 250),
				new Color(142, 96, 60),
				new Color(204, 209, 219),
				new Color(234, 85, 79)
			}, (short)4028, new Color[3]
			{
				new Color(255, 250, 184),
				new Color(217, 123, 0),
				new Color(209, 146, 33)
			}, (short)4029, new Color[4]
			{
				new Color(255, 250, 184),
				new Color(167, 57, 68),
				new Color(209, 146, 33),
				new Color(220, 185, 152)
			}, (short)4030, new Color[3]
			{
				new Color(247, 237, 127),
				new Color(215, 187, 59),
				new Color(174, 139, 43)
			}, (short)4031, new Color[3]
			{
				new Color(255, 198, 134),
				new Color(219, 109, 68),
				new Color(160, 83, 63)
			}, (short)4032, new Color[3]
			{
				new Color(228, 152, 107),
				new Color(170, 81, 57),
				new Color(128, 49, 49)
			}, (short)4033, new Color[3]
			{
				new Color(190, 226, 65),
				new Color(63, 69, 15),
				new Color(173, 50, 37)
			}, (short)4034, new Color[4]
			{
				new Color(255, 134, 86),
				new Color(193, 57, 37),
				new Color(186, 155, 130),
				new Color(178, 206, 46)
			}, (short)4035, new Color[4]
			{
				new Color(234, 244, 82),
				new Color(255, 182, 121),
				new Color(205, 89, 0),
				new Color(240, 157, 81)
			}, (short)4036, new Color[4]
			{
				new Color(223, 207, 74),
				new Color(189, 158, 36),
				new Color(226, 45, 38),
				new Color(131, 9, 0)
			}, (short)4037, new Color[3]
			{
				new Color(195, 109, 68),
				new Color(162, 69, 59),
				new Color(209, 194, 189)
			}, (short)4282, new Color[2]
			{
				new Color(237, 169, 78),
				new Color(211, 106, 62)
			}, (short)4283, new Color[3]
			{
				new Color(242, 235, 172),
				new Color(254, 247, 177),
				new Color(255, 230, 122)
			}, (short)4284, new Color[3]
			{
				new Color(59, 81, 114),
				new Color(105, 62, 118),
				new Color(35, 22, 57)
			}, (short)4285, new Color[3]
			{
				new Color(231, 115, 68),
				new Color(212, 42, 55),
				new Color(168, 16, 37)
			}, (short)4286, new Color[2]
			{
				new Color(185, 27, 68),
				new Color(124, 17, 53)
			}, (short)4287, new Color[3]
			{
				new Color(199, 163, 121),
				new Color(250, 245, 218),
				new Color(252, 250, 235)
			}, (short)4288, new Color[4]
			{
				new Color(209, 44, 90),
				new Color(83, 83, 83),
				new Color(245, 245, 245),
				new Color(250, 250, 250)
			}, (short)4289, new Color[3]
			{
				new Color(59, 81, 114),
				new Color(105, 62, 118),
				new Color(35, 22, 57)
			}, (short)4290, new Color[3]
			{
				new Color(247, 178, 52),
				new Color(221, 60, 96),
				new Color(225, 83, 115)
			}, (short)4291, new Color[2]
			{
				new Color(254, 231, 67),
				new Color(253, 239, 117)
			}, (short)4292, new Color[3]
			{
				new Color(231, 121, 68),
				new Color(216, 139, 33),
				new Color(251, 220, 77)
			}, (short)4293, new Color[2]
			{
				new Color(242, 153, 80),
				new Color(248, 208, 52)
			}, (short)4294, new Color[2]
			{
				new Color(253, 208, 17),
				new Color(253, 239, 117)
			}, (short)4295, new Color[3]
			{
				new Color(192, 47, 129),
				new Color(247, 178, 52),
				new Color(251, 220, 77)
			}, (short)4296, new Color[3]
			{
				new Color(212, 42, 55),
				new Color(250, 245, 218),
				new Color(252, 250, 235)
			}, (short)4297, new Color[2]
			{
				new Color(253, 239, 117),
				new Color(254, 247, 177)
			}, (short)4403, new Color[4]
			{
				new Color(255, 134, 86),
				new Color(193, 57, 37),
				new Color(242, 202, 174),
				new Color(128, 151, 43)
			}, (short)4411, new Color[2]
			{
				new Color(191, 157, 174),
				new Color(222, 196, 197)
			}, (short)4625, new Color[3]
			{
				new Color(255, 230, 122),
				new Color(216, 69, 33),
				new Color(128, 151, 43)
			}, (short)5092, new Color[3]
			{
				new Color(247, 178, 52),
				new Color(221, 60, 96),
				new Color(225, 83, 115)
			}, (short)5093, new Color[6]
			{
				new Color(197, 136, 85),
				new Color(143, 86, 59),
				new Color(100, 156, 58),
				new Color(216, 93, 61),
				new Color(255, 200, 40),
				new Color(50, 160, 30)
			}, (short)5277, new Color[3]
			{
				new Color(255, 204, 130),
				new Color(238, 102, 70),
				new Color(206, 47, 47)
			}, (short)5278, new Color[3]
			{
				new Color(255, 204, 130),
				new Color(215, 67, 51),
				new Color(150, 30, 84)
			});

			public static Color[][] DrinkParticleColors = Factory.CreateCustomSet(new Color[0], (short)28, new Color[3]
			{
				new Color(164, 16, 47),
				new Color(246, 34, 79),
				new Color(255, 95, 129)
			}, (short)110, new Color[3]
			{
				new Color(16, 45, 152),
				new Color(11, 61, 245),
				new Color(93, 127, 255)
			}, (short)126, new Color[3]
			{
				new Color(9, 61, 191),
				new Color(30, 84, 220),
				new Color(51, 107, 249)
			}, (short)188, new Color[3]
			{
				new Color(164, 16, 47),
				new Color(246, 34, 79),
				new Color(255, 95, 129)
			}, (short)189, new Color[3]
			{
				new Color(16, 45, 152),
				new Color(11, 61, 245),
				new Color(93, 127, 255)
			}, (short)226, new Color[3]
			{
				new Color(200, 25, 116),
				new Color(229, 30, 202),
				new Color(254, 149, 210)
			}, (short)227, new Color[3]
			{
				new Color(200, 25, 116),
				new Color(229, 30, 202),
				new Color(254, 149, 210)
			}, (short)288, new Color[3]
			{
				new Color(58, 48, 102),
				new Color(90, 72, 168),
				new Color(132, 116, 199)
			}, (short)289, new Color[3]
			{
				new Color(174, 13, 97),
				new Color(255, 156, 209),
				new Color(255, 56, 162)
			}, (short)290, new Color[3]
			{
				new Color(83, 137, 13),
				new Color(100, 164, 16),
				new Color(134, 230, 10)
			}, (short)291, new Color[3]
			{
				new Color(13, 74, 137),
				new Color(16, 89, 164),
				new Color(10, 119, 230)
			}, (short)292, new Color[3]
			{
				new Color(164, 159, 16),
				new Color(230, 222, 10),
				new Color(255, 252, 159)
			}, (short)293, new Color[3]
			{
				new Color(137, 13, 86),
				new Color(230, 10, 139),
				new Color(255, 144, 210)
			}, (short)294, new Color[3]
			{
				new Color(66, 13, 137),
				new Color(103, 10, 230),
				new Color(163, 95, 255)
			}, (short)295, new Color[3]
			{
				new Color(13, 106, 137),
				new Color(10, 176, 230),
				new Color(146, 229, 255)
			}, (short)296, new Color[3]
			{
				new Color(146, 102, 14),
				new Color(225, 185, 22),
				new Color(250, 213, 64)
			}, (short)297, new Color[3]
			{
				new Color(9, 101, 110),
				new Color(15, 164, 177),
				new Color(34, 229, 246)
			}, (short)298, new Color[3]
			{
				new Color(133, 137, 13),
				new Color(222, 230, 10),
				new Color(252, 254, 161)
			}, (short)299, new Color[3]
			{
				new Color(92, 137, 13),
				new Color(121, 184, 11),
				new Color(189, 255, 73)
			}, (short)300, new Color[3]
			{
				new Color(81, 60, 120),
				new Color(127, 96, 184),
				new Color(165, 142, 208)
			}, (short)301, new Color[3]
			{
				new Color(112, 137, 13),
				new Color(163, 202, 7),
				new Color(204, 246, 34)
			}, (short)302, new Color[3]
			{
				new Color(12, 63, 131),
				new Color(16, 79, 164),
				new Color(34, 124, 246)
			}, (short)303, new Color[3]
			{
				new Color(164, 96, 16),
				new Color(230, 129, 10),
				new Color(255, 200, 134)
			}, (short)304, new Color[3]
			{
				new Color(137, 63, 13),
				new Color(197, 87, 13),
				new Color(230, 98, 10)
			}, (short)305, new Color[3]
			{
				new Color(69, 13, 131),
				new Color(134, 34, 246),
				new Color(170, 95, 255)
			}, (short)499, new Color[3]
			{
				new Color(164, 16, 47),
				new Color(246, 34, 79),
				new Color(255, 95, 129)
			}, (short)500, new Color[3]
			{
				new Color(16, 45, 152),
				new Color(11, 61, 245),
				new Color(93, 127, 255)
			}, (short)678, new Color[2]
			{
				new Color(254, 0, 38),
				new Color(199, 29, 15)
			}, (short)967, new Color[2]
			{
				new Color(221, 226, 229),
				new Color(180, 189, 194)
			}, (short)969, new Color[3]
			{
				new Color(150, 99, 69),
				new Color(219, 170, 132),
				new Color(251, 244, 240)
			}, (short)1134, new Color[3]
			{
				new Color(235, 144, 10),
				new Color(254, 194, 20),
				new Color(254, 246, 37)
			}, (short)1340, new Color[3]
			{
				new Color(151, 79, 162),
				new Color(185, 128, 193),
				new Color(240, 185, 217)
			}, (short)1353, new Color[3]
			{
				new Color(77, 227, 45),
				new Color(218, 253, 9),
				new Color(96, 248, 2)
			}, (short)1354, new Color[3]
			{
				new Color(235, 36, 1),
				new Color(255, 127, 39),
				new Color(255, 203, 83)
			}, (short)1355, new Color[3]
			{
				new Color(148, 126, 24),
				new Color(233, 207, 137),
				new Color(255, 249, 183)
			}, (short)1356, new Color[3]
			{
				new Color(253, 152, 0),
				new Color(254, 202, 80),
				new Color(255, 251, 166)
			}, (short)1357, new Color[3]
			{
				new Color(106, 107, 134),
				new Color(118, 134, 207),
				new Color(120, 200, 226)
			}, (short)1358, new Color[3]
			{
				new Color(202, 0, 147),
				new Color(255, 66, 222),
				new Color(255, 170, 253)
			}, (short)1359, new Color[3]
			{
				new Color(45, 174, 76),
				new Color(112, 218, 138),
				new Color(182, 236, 195)
			}, (short)1977, new Color[3]
			{
				new Color(221, 0, 0),
				new Color(146, 17, 17),
				new Color(51, 21, 21)
			}, (short)1978, new Color[3]
			{
				new Color(24, 92, 248),
				new Color(97, 112, 169),
				new Color(228, 228, 228)
			}, (short)1979, new Color[4]
			{
				new Color(128, 128, 128),
				new Color(151, 107, 75),
				new Color(13, 101, 36),
				new Color(28, 216, 94)
			}, (short)1980, new Color[3]
			{
				new Color(122, 92, 10),
				new Color(185, 164, 23),
				new Color(241, 227, 75)
			}, (short)1981, new Color[2]
			{
				new Color(255, 186, 0),
				new Color(87, 20, 170)
			}, (short)1982, new Color[4]
			{
				new Color(218, 183, 59),
				new Color(59, 218, 85),
				new Color(59, 149, 218),
				new Color(218, 59, 59)
			}, (short)1983, new Color[4]
			{
				new Color(208, 80, 80),
				new Color(109, 106, 174),
				new Color(143, 215, 29),
				new Color(30, 150, 72)
			}, (short)1984, new Color[4]
			{
				new Color(255, 9, 172),
				new Color(219, 4, 121),
				new Color(111, 218, 171),
				new Color(72, 175, 130)
			}, (short)1985, new Color[4]
			{
				new Color(176, 101, 239),
				new Color(101, 238, 239),
				new Color(221, 0, 0),
				new Color(62, 235, 137)
			}, (short)1986, new Color[3]
			{
				new Color(55, 246, 211),
				new Color(20, 223, 168),
				new Color(0, 181, 128)
			}, (short)1990, new Color[3]
			{
				new Color(254, 254, 254),
				new Color(214, 232, 240),
				new Color(234, 242, 246)
			}, (short)2209, new Color[3]
			{
				new Color(16, 45, 152),
				new Color(11, 61, 245),
				new Color(93, 127, 255)
			}, (short)2322, new Color[3]
			{
				new Color(55, 92, 95),
				new Color(84, 149, 154),
				new Color(149, 196, 200)
			}, (short)2323, new Color[3]
			{
				new Color(91, 8, 106),
				new Color(184, 9, 131),
				new Color(250, 64, 188)
			}, (short)2324, new Color[3]
			{
				new Color(21, 40, 138),
				new Color(102, 101, 201),
				new Color(122, 147, 196)
			}, (short)2325, new Color[3]
			{
				new Color(100, 67, 50),
				new Color(141, 93, 68),
				new Color(182, 126, 97)
			}, (short)2326, new Color[3]
			{
				new Color(159, 224, 124),
				new Color(92, 175, 46),
				new Color(51, 95, 27)
			}, (short)2327, new Color[3]
			{
				new Color(95, 194, 255),
				new Color(12, 109, 167),
				new Color(13, 76, 115)
			}, (short)2328, new Color[3]
			{
				new Color(215, 241, 109),
				new Color(150, 178, 31),
				new Color(105, 124, 25)
			}, (short)2329, new Color[3]
			{
				new Color(251, 105, 29),
				new Color(220, 73, 4),
				new Color(140, 33, 10)
			}, (short)2344, new Color[3]
			{
				new Color(166, 166, 166),
				new Color(255, 186, 0),
				new Color(165, 58, 0)
			}, (short)2345, new Color[3]
			{
				new Color(239, 17, 0),
				new Color(209, 15, 0),
				new Color(136, 9, 0)
			}, (short)2346, new Color[3]
			{
				new Color(156, 157, 153),
				new Color(99, 99, 99),
				new Color(63, 62, 58)
			}, (short)2347, new Color[3]
			{
				new Color(243, 11, 11),
				new Color(255, 188, 55),
				new Color(252, 136, 58)
			}, (short)2348, new Color[3]
			{
				new Color(255, 227, 0),
				new Color(255, 135, 0),
				new Color(226, 56, 0)
			}, (short)2349, new Color[3]
			{
				new Color(120, 36, 30),
				new Color(216, 73, 63),
				new Color(233, 125, 117)
			}, (short)2351, new Color[3]
			{
				new Color(255, 95, 252),
				new Color(147, 0, 240),
				new Color(67, 0, 150)
			}, (short)2354, new Color[3]
			{
				new Color(117, 233, 164),
				new Color(40, 199, 103),
				new Color(30, 120, 66)
			}, (short)2355, new Color[3]
			{
				new Color(217, 254, 161),
				new Color(69, 110, 9),
				new Color(135, 219, 11)
			}, (short)2356, new Color[3]
			{
				new Color(233, 175, 117),
				new Color(199, 120, 40),
				new Color(143, 89, 36)
			}, (short)2359, new Color[3]
			{
				new Color(255, 51, 0),
				new Color(248, 184, 0),
				new Color(255, 215, 0)
			}, (short)2756, new Color[4]
			{
				new Color(178, 236, 255),
				new Color(92, 214, 255),
				new Color(184, 96, 163),
				new Color(255, 78, 178)
			}, (short)2863, new Color[3]
			{
				new Color(97, 199, 224),
				new Color(98, 152, 177),
				new Color(26, 232, 249)
			}, (short)3001, new Color[3]
			{
				new Color(104, 25, 103),
				new Color(155, 32, 154),
				new Color(190, 138, 223)
			}, (short)3259, new Color[4]
			{
				new Color(40, 20, 66),
				new Color(186, 68, 255),
				new Color(26, 8, 49),
				new Color(255, 122, 255)
			}, (short)3544, new Color[3]
			{
				new Color(164, 16, 47),
				new Color(246, 34, 79),
				new Color(255, 95, 129)
			}, (short)353, new Color[3]
			{
				new Color(205, 152, 2) * 0.5f,
				new Color(240, 208, 88) * 0.5f,
				new Color(251, 243, 215) * 0.5f
			}, (short)1912, new Color[3]
			{
				new Color(237, 159, 85),
				new Color(255, 228, 133),
				new Color(149, 97, 45)
			}, (short)2266, new Color[1] { new Color(233, 233, 218) * 0.3f }, (short)4018, new Color[2]
			{
				new Color(214, 170, 105) * 0.5f,
				new Color(180, 132, 73) * 0.5f
			}, (short)4027, new Color[4]
			{
				new Color(242, 183, 236),
				new Color(245, 242, 193),
				new Color(226, 133, 217),
				new Color(242, 70, 88)
			}, (short)4477, new Color[2]
			{
				new Color(161, 192, 220),
				new Color(143, 154, 201)
			}, (short)4478, new Color[2]
			{
				new Color(40, 60, 70),
				new Color(26, 27, 36)
			}, (short)4479, new Color[2]
			{
				new Color(224, 0, 152),
				new Color(137, 13, 126)
			}, (short)4614, new Color[2]
			{
				new Color(153, 62, 2),
				new Color(208, 166, 59)
			}, (short)4615, new Color[2]
			{
				new Color(164, 88, 178),
				new Color(124, 64, 152)
			}, (short)4616, new Color[2]
			{
				new Color(255, 245, 109),
				new Color(235, 210, 89)
			}, (short)4617, new Color[2]
			{
				new Color(245, 247, 250),
				new Color(255, 250, 133)
			}, (short)4618, new Color[2]
			{
				new Color(255, 175, 133),
				new Color(237, 93, 85)
			}, (short)4619, new Color[2]
			{
				new Color(247, 245, 224),
				new Color(232, 214, 179)
			}, (short)4620, new Color[2]
			{
				new Color(181, 215, 0),
				new Color(255, 112, 4)
			}, (short)4621, new Color[2]
			{
				new Color(242, 134, 81),
				new Color(153, 2, 42)
			}, (short)4622, new Color[2]
			{
				new Color(90, 62, 123),
				new Color(59, 49, 104)
			}, (short)4623, new Color[3]
			{
				new Color(255, 175, 152),
				new Color(147, 255, 228),
				new Color(231, 247, 150)
			}, (short)4624, new Color[2]
			{
				new Color(155, 0, 67),
				new Color(208, 124, 59)
			}, (short)5009, new Color[2]
			{
				new Color(210, 130, 10),
				new Color(255, 195, 20)
			}, (short)5041, new Color[2]
			{
				new Color(221, 226, 229),
				new Color(180, 189, 194)
			}, (short)5042, new Color[2]
			{
				new Color(70, 43, 21),
				new Color(142, 96, 60)
			}, (short)5275, new Color[2]
			{
				new Color(70, 43, 21),
				new Color(142, 96, 60)
			});

			private static BannerEffect DD2BannerEffect = BannerEffect.Reduced;

			public static BannerEffect[] BannerStrength = Factory.CreateCustomSet(new BannerEffect(1f), (short)3838, DD2BannerEffect, (short)3845, DD2BannerEffect, (short)3837, DD2BannerEffect, (short)3844, DD2BannerEffect, (short)3843, DD2BannerEffect, (short)3839, DD2BannerEffect, (short)3840, DD2BannerEffect, (short)3842, DD2BannerEffect, (short)3841, DD2BannerEffect, (short)3846, DD2BannerEffect);

			public static int DefaultKillsForBannerNeeded = 50;

			public static int[] KillsToBanner = Factory.CreateIntSet(DefaultKillsForBannerNeeded, 3838, 1000, 3845, 200, 3837, 500, 3844, 200, 3843, 50, 3839, 150, 3840, 100, 3842, 200, 3841, 100, 3846, 50, 2971, 200, 2982, 150, 2931, 100, 2961, 100, 2994, 100, 2985, 10, 4541, 10, 2969, 10, 2986, 10, 2915, 10, 4602, 10, 4542, 25, 4543, 25, 4546, 25, 4545, 25, 2901, 25, 2902, 25, 1631, 25, 2913, 25, 4688, 25, 3390, 25, 4973, 25, 4974, 25, 4975, 25, 2934, 25, 1670, 25, 1694, 25, 2958, 25, 2960, 25, 3441, 25, 3780, 25, 3397, 25, 3403, 25);

			public static bool[] CanFishInLava = Factory.CreateBoolSet(2422);

			public static bool[] IsLavaBait = Factory.CreateBoolSet(4849, 4845, 4847);

			private const int healingItemsDecayRate = 4;

			public static int[] ItemSpawnDecaySpeed = Factory.CreateIntSet(1, 58, 4, 184, 4, 1867, 4, 1868, 4, 1734, 4, 1735, 4);

			public static bool[] IsFishingCrate = Factory.CreateBoolSet(2334, 2335, 2336, 3203, 3204, 3205, 3206, 3207, 3208, 4405, 4407, 4877, 5002, 3979, 3980, 3981, 3982, 3983, 3984, 3985, 3986, 3987, 4406, 4408, 4878, 5003);

			public static bool[] IsFishingCrateHardmode = Factory.CreateBoolSet(3979, 3980, 3981, 3982, 3983, 3984, 3985, 3986, 3987, 4406, 4408, 4878, 5003);

			public static bool[] CanBePlacedOnWeaponRacks = Factory.CreateBoolSet(3196, 166, 235, 3115, 167, 2896, 3547, 580, 937, 4423, 4824, 4825, 4826, 4827, 4908, 4909, 4094, 4039, 4092, 4093, 4587, 4588, 4589, 4590, 4591, 4592, 4593, 4594, 4595, 4596, 4597, 4598, 905, 1326, 5335, 3225, 2303, 2299, 2290, 2317, 2305, 2304, 2313, 2318, 2312, 2306, 2308, 2319, 2314, 2302, 2315, 2307, 2310, 2301, 2298, 2316, 2309, 2321, 2297, 2300, 2311, 2420, 2438, 2437, 2436, 4401, 4402, 2475, 2476, 2450, 2477, 2478, 2451, 2479, 2480, 2452, 2453, 2481, 2454, 2482, 2483, 2455, 2456, 2457, 2458, 2459, 2460, 2484, 2472, 2461, 2462, 2463, 2485, 2464, 2465, 2486, 2466, 2467, 2468, 2487, 2469, 2488, 2470, 2471, 2473, 2474, 4393, 4394);

			public static int[] TextureCopyLoad = Factory.CreateIntSet(-1, 3665, 48, 3666, 306, 3667, 328, 3668, 625, 3669, 626, 3670, 627, 3671, 680, 3672, 681, 3673, 831, 3674, 838, 3675, 914, 3676, 952, 3677, 1142, 3678, 1298, 3679, 1528, 3680, 1529, 3681, 1530, 3682, 1531, 3683, 1532, 3684, 2230, 3685, 2249, 3686, 2250, 3687, 2526, 3688, 2544, 3689, 2559, 3690, 2574, 3691, 2612, 3692, 2613, 3693, 2614, 3694, 2615, 3695, 2616, 3696, 2617, 3697, 2618, 3698, 2619, 3699, 2620, 3700, 2748, 3701, 2814, 3703, 3125, 3702, 3180, 3704, 3181, 3705, 3665, 3706, 3665, 4713, 4712, 5167, 5156, 5188, 5177, 5209, 5198);

			public static bool[] TrapSigned = Factory.CreateBoolSet(false, 3665, 3666, 3667, 3668, 3669, 3670, 3671, 3672, 3673, 3674, 3675, 3676, 3677, 3678, 3679, 3680, 3681, 3682, 3683, 3684, 3685, 3686, 3687, 3688, 3689, 3690, 3691, 3692, 3693, 3694, 3695, 3696, 3697, 3698, 3699, 3700, 3701, 3703, 3702, 3704, 3705, 3706, 3886, 3887, 3950, 3976, 4164, 4185, 4206, 4227, 4266, 4268, 4585, 4713, 5167, 5188, 5209);

			public static bool[] Deprecated = Factory.CreateBoolSet(2783, 2785, 2782, 2773, 2775, 2772, 2778, 2780, 2777, 3463, 3465, 3462, 2881, 3847, 3848, 3849, 3850, 3851, 3850, 3861, 3862, 4010, 4058, 5013, 4722, 3978);

			public static bool[] NeverAppearsAsNewInInventory = Factory.CreateBoolSet(71, 72, 73, 74);

			public static bool[] CommonCoin = Factory.CreateBoolSet(71, 72, 73, 74);

			public static bool[] ItemIconPulse = Factory.CreateBoolSet(520, 521, 575, 549, 548, 547, 3456, 3457, 3458, 3459, 3580, 3581);

			public static bool[] ItemNoGravity = Factory.CreateBoolSet(520, 521, 575, 549, 548, 547, 3453, 3454, 3455, 3456, 3457, 3458, 3459, 3580, 3581, 4143);

			public static int[] ExtractinatorMode = Factory.CreateIntSet(-1, 424, 0, 1103, 0, 3347, 1, 2339, 2, 2338, 2, 2337, 2, 4354, 3, 4389, 3, 4377, 3, 4378, 3, 5127, 3, 5128, 3);

			public static int[] StaffMinionSlotsRequired = Factory.CreateIntSet(1);

			public static bool[] ExoticPlantsForDyeTrade = Factory.CreateBoolSet(3385, 3386, 3387, 3388);

			public static bool[] NebulaPickup = Factory.CreateBoolSet(3453, 3454, 3455);

			public static bool[] AnimatesAsSoul = Factory.CreateBoolSet(575, 547, 520, 548, 521, 549, 3580, 3581);

			public static bool[] gunProj = Factory.CreateBoolSet(3475, 3540, 3854, 3930);

			public static int[] SortingPriorityBossSpawns = Factory.CreateIntSet(-1, 43, 1, 560, 2, 70, 3, 1331, 3, 361, 4, 5120, 5, 1133, 5, 4988, 6, 5334, 7, 544, 8, 556, 9, 557, 10, 2495, 11, 2673, 12, 602, 13, 1844, 14, 1958, 15, 1293, 16, 2767, 17, 4271, 18, 3601, 19, 1291, 20, 109, 21, 29, 22, 50, 23, 3199, 24, 3124, 25, 5437, 26, 5358, 27, 5359, 28, 5360, 29, 5361, 30, 4263, 31, 4819, 32);

			public static int[] SortingPriorityWiring = Factory.CreateIntSet(-1, 510, 103, 3625, 102, 509, 101, 851, 100, 850, 99, 3612, 98, 849, 97, 4485, 96, 4484, 95, 583, 94, 584, 93, 585, 92, 538, 91, 513, 90, 3545, 90, 853, 89, 541, 88, 529, 88, 1151, 87, 852, 87, 543, 87, 542, 87, 3707, 87, 2492, 86, 530, 85, 581, 84, 582, 84, 1263, 83);

			public static int[] SortingPriorityMaterials = Factory.CreateIntSet(-1, 3467, 100, 3460, 99, 3458, 98, 3456, 97, 3457, 96, 3459, 95, 3261, 94, 1508, 93, 1552, 92, 1006, 91, 947, 90, 1225, 89, 1198, 88, 1106, 87, 391, 86, 366, 85, 1191, 84, 1105, 83, 382, 82, 365, 81, 1184, 80, 1104, 79, 381, 78, 364, 77, 548, 76, 547, 75, 549, 74, 575, 73, 521, 72, 520, 71, 175, 70, 174, 69, 3380, 68, 1329, 67, 1257, 66, 880, 65, 86, 64, 57, 63, 56, 62, 117, 61, 116, 60, 706, 59, 702, 58, 19, 57, 13, 56, 705, 55, 701, 54, 21, 53, 14, 52, 704, 51, 700, 50, 22, 49, 11, 48, 703, 47, 699, 46, 20, 45, 12, 44, 999, 43, 182, 42, 178, 41, 179, 40, 177, 39, 180, 38, 181, 37);

			public static int[] SortingPriorityExtractibles = Factory.CreateIntSet(-1, 997, 4, 3347, 3, 1103, 2, 424, 1);

			public static int[] SortingPriorityRopes = Factory.CreateIntSet(-1, 965, 1, 85, 1, 210, 1, 3077, 1, 3078, 1);

			public static int[] SortingPriorityPainting = Factory.CreateIntSet(-1, 1543, 100, 1544, 99, 1545, 98, 1071, 97, 1072, 96, 1100, 95);

			public static int[] SortingPriorityTerraforming = Factory.CreateIntSet(-1, 5134, 100, 779, 99, 780, 98, 783, 97, 781, 96, 782, 95, 784, 94, 5392, 93, 5393, 92, 5394, 91, 422, 90, 423, 89, 3477, 88, 66, 67, 67, 86, 2886, 85);

			public static int[] GamepadExtraRange = Factory.CreateIntSet(0, 2797, 20, 3278, 4, 3285, 6, 3279, 8, 3280, 8, 3281, 9, 3262, 10, 3317, 10, 5294, 10, 3282, 10, 3315, 10, 3316, 11, 3283, 12, 3290, 13, 3289, 11, 3284, 13, 3286, 13, 3287, 18, 3288, 18, 3291, 17, 3292, 18, 3389, 21);

			public static bool[] GamepadWholeScreenUseRange = Factory.CreateBoolSet(1326, 5335, 1256, 1244, 3014, 113, 218, 495, 114, 496, 2796, 494, 3006, 65, 1931, 3570, 2750, 3065, 3029, 3030, 4381, 4956, 5065, 1309, 2364, 2365, 2551, 2535, 2584, 1157, 2749, 1802, 2621, 3249, 3531, 3474, 2366, 1572, 3569, 3571, 4269, 4273, 4281, 5119, 3611, 1299, 1254);

			public static float[] BonusMeleeSpeedMultiplier = Factory.CreateFloatSet(1f, 1827f, 0.5f, 3013f, 0.25f, 3106f, 0.33f);

			public static bool[] GamepadSmartQuickReach = Factory.CreateBoolSet(2798, 2797, 3030, 3262, 3278, 3279, 3280, 3281, 3282, 3283, 3284, 3285, 3286, 3287, 3288, 3289, 3290, 3291, 3292, 3315, 3316, 3317, 5294, 3389, 2798, 65, 1931, 3570, 2750, 3065, 3029, 4956, 5065, 1256, 1244, 3014, 113, 218, 495);

			public static bool[] Yoyo = Factory.CreateBoolSet(3262, 3278, 3279, 3280, 3281, 3282, 3283, 3284, 3285, 3286, 3287, 3288, 3289, 3290, 3291, 3292, 3315, 3316, 3317, 3389, 5294);

			public static bool[] AlsoABuildingItem = Factory.CreateBoolSet(3031, 205, 1128, 207, 206, 3032, 849, 3620, 509, 851, 850, 3625, 510, 1071, 1543, 1072, 1544, 1100, 1545, 4820, 4872, 5303, 5304, 5302, 5364);

			public static bool[] LockOnIgnoresCollision = Factory.CreateBoolSet(64, 3570, 1327, 3006, 1227, 788, 756, 1228, 65, 3065, 3473, 3051, 5065, 1309, 2364, 2365, 2551, 2535, 2584, 1157, 2749, 1802, 2621, 3249, 3531, 3474, 2366, 1572, 4269, 4273, 4281, 4607, 5069, 5114, 5119, 3014, 3569, 3571);

			public static int[] LockOnAimAbove = Factory.CreateIntSet(0, 1256, 15, 1244, 15, 3014, 15, 3569, 15, 3571, 15);

			public static float?[] LockOnAimCompensation = Factory.CreateCustomSet<float?>(null, new object[10]
			{
				(short)1336,
				0.2f,
				(short)157,
				0.29f,
				(short)2590,
				0.4f,
				(short)3821,
				0.4f,
				(short)160,
				0.4f
			});

			public static bool[] SingleUseInGamepad = Factory.CreateBoolSet(8, 427, 3004, 523, 433, 429, 974, 1333, 1245, 3114, 430, 3045, 428, 2274, 431, 432, 4383, 4384, 4385, 4386, 4387, 4388, 5293, 5353);

			public static bool[] Torches = Factory.CreateBoolSet(8, 427, 3004, 523, 433, 429, 974, 1333, 1245, 3114, 430, 3045, 428, 2274, 431, 432, 4383, 4384, 4385, 4386, 4387, 4388, 5293, 5353);

			public static bool[] WaterTorches = Factory.CreateBoolSet(523, 1333, 4384);

			public static short[] Workbenches = new short[41]
			{
				36, 635, 636, 637, 673, 811, 812, 813, 814, 815,
				916, 1145, 1398, 1401, 1404, 1461, 1511, 1795, 1817, 2229,
				2251, 2252, 2253, 2534, 2631, 2632, 2633, 2826, 3156, 3157,
				3158, 3909, 3910, 3949, 3975, 4163, 4184, 4205, 4226, 4315,
				4584
			};

			public static bool[] BossBag = Factory.CreateBoolSet(3318, 3319, 3320, 3321, 3322, 3323, 3324, 3325, 3326, 3327, 3328, 3329, 3330, 3331, 3332, 3860, 3861, 3862, 4782, 4957, 5111);

			public static bool[] OpenableBag = Factory.CreateBoolSet(3318, 3319, 3320, 3321, 3322, 3323, 3324, 3325, 3326, 3327, 3328, 3329, 3330, 3331, 3332, 3860, 3861, 3862, 4782, 4957, 5111, 2334, 2335, 2336, 3203, 3204, 3205, 3206, 3207, 3208, 4405, 4407, 4877, 5002, 3979, 3980, 3981, 3982, 3983, 3984, 3985, 3986, 3987, 4406, 4408, 4878, 5003, 3093, 4345, 4410, 1774, 4879, 1869, 599, 600, 601);
		}

		private static Dictionary<string, short> _legacyItemLookup;

		public static readonly IdDictionary Search = IdDictionary.Create<ItemID, short>();

		public const short YellowPhasesaberOld = -24;

		public const short WhitePhasesaberOld = -23;

		public const short PurplePhasesaberOld = -22;

		public const short GreenPhasesaberOld = -21;

		public const short RedPhasesaberOld = -20;

		public const short BluePhasesaberOld = -19;

		public const short PlatinumBowOld = -48;

		public const short PlatinumHammerOld = -47;

		public const short PlatinumAxeOld = -46;

		public const short PlatinumShortswordOld = -45;

		public const short PlatinumBroadswordOld = -44;

		public const short PlatinumPickaxeOld = -43;

		public const short TungstenBowOld = -42;

		public const short TungstenHammerOld = -41;

		public const short TungstenAxeOld = -40;

		public const short TungstenShortswordOld = -39;

		public const short TungstenBroadswordOld = -38;

		public const short TungstenPickaxeOld = -37;

		public const short LeadBowOld = -36;

		public const short LeadHammerOld = -35;

		public const short LeadAxeOld = -34;

		public const short LeadShortswordOld = -33;

		public const short LeadBroadswordOld = -32;

		public const short LeadPickaxeOld = -31;

		public const short TinBowOld = -30;

		public const short TinHammerOld = -29;

		public const short TinAxeOld = -28;

		public const short TinShortswordOld = -27;

		public const short TinBroadswordOld = -26;

		public const short TinPickaxeOld = -25;

		public const short CopperBowOld = -18;

		public const short CopperHammerOld = -17;

		public const short CopperAxeOld = -16;

		public const short CopperShortswordOld = -15;

		public const short CopperBroadswordOld = -14;

		public const short CopperPickaxeOld = -13;

		public const short SilverBowOld = -12;

		public const short SilverHammerOld = -11;

		public const short SilverAxeOld = -10;

		public const short SilverShortswordOld = -9;

		public const short SilverBroadswordOld = -8;

		public const short SilverPickaxeOld = -7;

		public const short GoldBowOld = -6;

		public const short GoldHammerOld = -5;

		public const short GoldAxeOld = -4;

		public const short GoldShortswordOld = -3;

		public const short GoldBroadswordOld = -2;

		public const short GoldPickaxeOld = -1;

		public const short None = 0;

		public const short IronPickaxe = 1;

		public const short DirtBlock = 2;

		public const short StoneBlock = 3;

		public const short IronBroadsword = 4;

		public const short Mushroom = 5;

		public const short IronShortsword = 6;

		public const short IronHammer = 7;

		public const short Torch = 8;

		public const short Wood = 9;

		public const short IronAxe = 10;

		public const short IronOre = 11;

		public const short CopperOre = 12;

		public const short GoldOre = 13;

		public const short SilverOre = 14;

		public const short CopperWatch = 15;

		public const short SilverWatch = 16;

		public const short GoldWatch = 17;

		public const short DepthMeter = 18;

		public const short GoldBar = 19;

		public const short CopperBar = 20;

		public const short SilverBar = 21;

		public const short IronBar = 22;

		public const short Gel = 23;

		public const short WoodenSword = 24;

		public const short WoodenDoor = 25;

		public const short StoneWall = 26;

		public const short Acorn = 27;

		public const short LesserHealingPotion = 28;

		public const short LifeCrystal = 29;

		public const short DirtWall = 30;

		public const short Bottle = 31;

		public const short WoodenTable = 32;

		public const short Furnace = 33;

		public const short WoodenChair = 34;

		public const short IronAnvil = 35;

		public const short WorkBench = 36;

		public const short Goggles = 37;

		public const short Lens = 38;

		public const short WoodenBow = 39;

		public const short WoodenArrow = 40;

		public const short FlamingArrow = 41;

		public const short Shuriken = 42;

		public const short SuspiciousLookingEye = 43;

		public const short DemonBow = 44;

		public const short WarAxeoftheNight = 45;

		public const short LightsBane = 46;

		public const short UnholyArrow = 47;

		public const short Chest = 48;

		public const short BandofRegeneration = 49;

		public const short MagicMirror = 50;

		public const short JestersArrow = 51;

		public const short AngelStatue = 52;

		public const short CloudinaBottle = 53;

		public const short HermesBoots = 54;

		public const short EnchantedBoomerang = 55;

		public const short DemoniteOre = 56;

		public const short DemoniteBar = 57;

		public const short Heart = 58;

		public const short CorruptSeeds = 59;

		public const short VileMushroom = 60;

		public const short EbonstoneBlock = 61;

		public const short GrassSeeds = 62;

		public const short Sunflower = 63;

		public const short Vilethorn = 64;

		public const short Starfury = 65;

		public const short PurificationPowder = 66;

		public const short VilePowder = 67;

		public const short RottenChunk = 68;

		public const short WormTooth = 69;

		public const short WormFood = 70;

		public const short CopperCoin = 71;

		public const short SilverCoin = 72;

		public const short GoldCoin = 73;

		public const short PlatinumCoin = 74;

		public const short FallenStar = 75;

		public const short CopperGreaves = 76;

		public const short IronGreaves = 77;

		public const short SilverGreaves = 78;

		public const short GoldGreaves = 79;

		public const short CopperChainmail = 80;

		public const short IronChainmail = 81;

		public const short SilverChainmail = 82;

		public const short GoldChainmail = 83;

		public const short GrapplingHook = 84;

		public const short Chain = 85;

		public const short ShadowScale = 86;

		public const short PiggyBank = 87;

		public const short MiningHelmet = 88;

		public const short CopperHelmet = 89;

		public const short IronHelmet = 90;

		public const short SilverHelmet = 91;

		public const short GoldHelmet = 92;

		public const short WoodWall = 93;

		public const short WoodPlatform = 94;

		public const short FlintlockPistol = 95;

		public const short Musket = 96;

		public const short MusketBall = 97;

		public const short Minishark = 98;

		public const short IronBow = 99;

		public const short ShadowGreaves = 100;

		public const short ShadowScalemail = 101;

		public const short ShadowHelmet = 102;

		public const short NightmarePickaxe = 103;

		public const short TheBreaker = 104;

		public const short Candle = 105;

		public const short CopperChandelier = 106;

		public const short SilverChandelier = 107;

		public const short GoldChandelier = 108;

		public const short ManaCrystal = 109;

		public const short LesserManaPotion = 110;

		public const short BandofStarpower = 111;

		public const short FlowerofFire = 112;

		public const short MagicMissile = 113;

		public const short DirtRod = 114;

		public const short ShadowOrb = 115;

		public const short Meteorite = 116;

		public const short MeteoriteBar = 117;

		public const short Hook = 118;

		public const short Flamarang = 119;

		public const short MoltenFury = 120;

		public const short FieryGreatsword = 121;

		public const short MoltenPickaxe = 122;

		public const short MeteorHelmet = 123;

		public const short MeteorSuit = 124;

		public const short MeteorLeggings = 125;

		public const short BottledWater = 126;

		public const short SpaceGun = 127;

		public const short RocketBoots = 128;

		public const short GrayBrick = 129;

		public const short GrayBrickWall = 130;

		public const short RedBrick = 131;

		public const short RedBrickWall = 132;

		public const short ClayBlock = 133;

		public const short BlueBrick = 134;

		public const short BlueBrickWall = 135;

		public const short ChainLantern = 136;

		public const short GreenBrick = 137;

		public const short GreenBrickWall = 138;

		public const short PinkBrick = 139;

		public const short PinkBrickWall = 140;

		public const short GoldBrick = 141;

		public const short GoldBrickWall = 142;

		public const short SilverBrick = 143;

		public const short SilverBrickWall = 144;

		public const short CopperBrick = 145;

		public const short CopperBrickWall = 146;

		public const short Spike = 147;

		public const short WaterCandle = 148;

		public const short Book = 149;

		public const short Cobweb = 150;

		public const short NecroHelmet = 151;

		public const short NecroBreastplate = 152;

		public const short NecroGreaves = 153;

		public const short Bone = 154;

		public const short Muramasa = 155;

		public const short CobaltShield = 156;

		public const short AquaScepter = 157;

		public const short LuckyHorseshoe = 158;

		public const short ShinyRedBalloon = 159;

		public const short Harpoon = 160;

		public const short SpikyBall = 161;

		public const short BallOHurt = 162;

		public const short BlueMoon = 163;

		public const short Handgun = 164;

		public const short WaterBolt = 165;

		public const short Bomb = 166;

		public const short Dynamite = 167;

		public const short Grenade = 168;

		public const short SandBlock = 169;

		public const short Glass = 170;

		public const short Sign = 171;

		public const short AshBlock = 172;

		public const short Obsidian = 173;

		public const short Hellstone = 174;

		public const short HellstoneBar = 175;

		public const short MudBlock = 176;

		public const short Sapphire = 177;

		public const short Ruby = 178;

		public const short Emerald = 179;

		public const short Topaz = 180;

		public const short Amethyst = 181;

		public const short Diamond = 182;

		public const short GlowingMushroom = 183;

		public const short Star = 184;

		public const short IvyWhip = 185;

		public const short BreathingReed = 186;

		public const short Flipper = 187;

		public const short HealingPotion = 188;

		public const short ManaPotion = 189;

		public const short BladeofGrass = 190;

		public const short ThornChakram = 191;

		public const short ObsidianBrick = 192;

		public const short ObsidianSkull = 193;

		public const short MushroomGrassSeeds = 194;

		public const short JungleGrassSeeds = 195;

		public const short WoodenHammer = 196;

		public const short StarCannon = 197;

		public const short BluePhaseblade = 198;

		public const short RedPhaseblade = 199;

		public const short GreenPhaseblade = 200;

		public const short PurplePhaseblade = 201;

		public const short WhitePhaseblade = 202;

		public const short YellowPhaseblade = 203;

		public const short MeteorHamaxe = 204;

		public const short EmptyBucket = 205;

		public const short WaterBucket = 206;

		public const short LavaBucket = 207;

		public const short JungleRose = 208;

		public const short Stinger = 209;

		public const short Vine = 210;

		public const short FeralClaws = 211;

		public const short AnkletoftheWind = 212;

		public const short StaffofRegrowth = 213;

		public const short HellstoneBrick = 214;

		public const short WhoopieCushion = 215;

		public const short Shackle = 216;

		public const short MoltenHamaxe = 217;

		public const short Flamelash = 218;

		public const short PhoenixBlaster = 219;

		public const short Sunfury = 220;

		public const short Hellforge = 221;

		public const short ClayPot = 222;

		public const short NaturesGift = 223;

		public const short Bed = 224;

		public const short Silk = 225;

		public const short LesserRestorationPotion = 226;

		public const short RestorationPotion = 227;

		public const short JungleHat = 228;

		public const short JungleShirt = 229;

		public const short JunglePants = 230;

		public const short MoltenHelmet = 231;

		public const short MoltenBreastplate = 232;

		public const short MoltenGreaves = 233;

		public const short MeteorShot = 234;

		public const short StickyBomb = 235;

		public const short BlackLens = 236;

		public const short Sunglasses = 237;

		public const short WizardHat = 238;

		public const short TopHat = 239;

		public const short TuxedoShirt = 240;

		public const short TuxedoPants = 241;

		public const short SummerHat = 242;

		public const short BunnyHood = 243;

		public const short PlumbersHat = 244;

		public const short PlumbersShirt = 245;

		public const short PlumbersPants = 246;

		public const short HerosHat = 247;

		public const short HerosShirt = 248;

		public const short HerosPants = 249;

		public const short FishBowl = 250;

		public const short ArchaeologistsHat = 251;

		public const short ArchaeologistsJacket = 252;

		public const short ArchaeologistsPants = 253;

		public const short BlackThread = 254;

		public const short GreenThread = 255;

		public const short NinjaHood = 256;

		public const short NinjaShirt = 257;

		public const short NinjaPants = 258;

		public const short Leather = 259;

		public const short RedHat = 260;

		public const short Goldfish = 261;

		public const short Robe = 262;

		public const short RobotHat = 263;

		public const short GoldCrown = 264;

		public const short HellfireArrow = 265;

		public const short Sandgun = 266;

		public const short GuideVoodooDoll = 267;

		public const short DivingHelmet = 268;

		public const short FamiliarShirt = 269;

		public const short FamiliarPants = 270;

		public const short FamiliarWig = 271;

		public const short DemonScythe = 272;

		public const short NightsEdge = 273;

		public const short DarkLance = 274;

		public const short Coral = 275;

		public const short Cactus = 276;

		public const short Trident = 277;

		public const short SilverBullet = 278;

		public const short ThrowingKnife = 279;

		public const short Spear = 280;

		public const short Blowpipe = 281;

		public const short Glowstick = 282;

		public const short Seed = 283;

		public const short WoodenBoomerang = 284;

		public const short Aglet = 285;

		public const short StickyGlowstick = 286;

		public const short PoisonedKnife = 287;

		public const short ObsidianSkinPotion = 288;

		public const short RegenerationPotion = 289;

		public const short SwiftnessPotion = 290;

		public const short GillsPotion = 291;

		public const short IronskinPotion = 292;

		public const short ManaRegenerationPotion = 293;

		public const short MagicPowerPotion = 294;

		public const short FeatherfallPotion = 295;

		public const short SpelunkerPotion = 296;

		public const short InvisibilityPotion = 297;

		public const short ShinePotion = 298;

		public const short NightOwlPotion = 299;

		public const short BattlePotion = 300;

		public const short ThornsPotion = 301;

		public const short WaterWalkingPotion = 302;

		public const short ArcheryPotion = 303;

		public const short HunterPotion = 304;

		public const short GravitationPotion = 305;

		public const short GoldChest = 306;

		public const short DaybloomSeeds = 307;

		public const short MoonglowSeeds = 308;

		public const short BlinkrootSeeds = 309;

		public const short DeathweedSeeds = 310;

		public const short WaterleafSeeds = 311;

		public const short FireblossomSeeds = 312;

		public const short Daybloom = 313;

		public const short Moonglow = 314;

		public const short Blinkroot = 315;

		public const short Deathweed = 316;

		public const short Waterleaf = 317;

		public const short Fireblossom = 318;

		public const short SharkFin = 319;

		public const short Feather = 320;

		public const short Tombstone = 321;

		public const short MimeMask = 322;

		public const short AntlionMandible = 323;

		public const short IllegalGunParts = 324;

		public const short TheDoctorsShirt = 325;

		public const short TheDoctorsPants = 326;

		public const short GoldenKey = 327;

		public const short ShadowChest = 328;

		public const short ShadowKey = 329;

		public const short ObsidianBrickWall = 330;

		public const short JungleSpores = 331;

		public const short Loom = 332;

		public const short Piano = 333;

		public const short Dresser = 334;

		public const short Bench = 335;

		public const short Bathtub = 336;

		public const short RedBanner = 337;

		public const short GreenBanner = 338;

		public const short BlueBanner = 339;

		public const short YellowBanner = 340;

		public const short LampPost = 341;

		public const short TikiTorch = 342;

		public const short Barrel = 343;

		public const short ChineseLantern = 344;

		public const short CookingPot = 345;

		public const short Safe = 346;

		public const short SkullLantern = 347;

		public const short TrashCan = 348;

		public const short Candelabra = 349;

		public const short PinkVase = 350;

		public const short Mug = 351;

		public const short Keg = 352;

		public const short Ale = 353;

		public const short Bookcase = 354;

		public const short Throne = 355;

		public const short Bowl = 356;

		public const short BowlofSoup = 357;

		public const short Toilet = 358;

		public const short GrandfatherClock = 359;

		public const short ArmorStatue = 360;

		public const short GoblinBattleStandard = 361;

		public const short TatteredCloth = 362;

		public const short Sawmill = 363;

		public const short CobaltOre = 364;

		public const short MythrilOre = 365;

		public const short AdamantiteOre = 366;

		public const short Pwnhammer = 367;

		public const short Excalibur = 368;

		public const short HallowedSeeds = 369;

		public const short EbonsandBlock = 370;

		public const short CobaltHat = 371;

		public const short CobaltHelmet = 372;

		public const short CobaltMask = 373;

		public const short CobaltBreastplate = 374;

		public const short CobaltLeggings = 375;

		public const short MythrilHood = 376;

		public const short MythrilHelmet = 377;

		public const short MythrilHat = 378;

		public const short MythrilChainmail = 379;

		public const short MythrilGreaves = 380;

		public const short CobaltBar = 381;

		public const short MythrilBar = 382;

		public const short CobaltChainsaw = 383;

		public const short MythrilChainsaw = 384;

		public const short CobaltDrill = 385;

		public const short MythrilDrill = 386;

		public const short AdamantiteChainsaw = 387;

		public const short AdamantiteDrill = 388;

		public const short DaoofPow = 389;

		public const short MythrilHalberd = 390;

		public const short AdamantiteBar = 391;

		public const short GlassWall = 392;

		public const short Compass = 393;

		public const short DivingGear = 394;

		public const short GPS = 395;

		public const short ObsidianHorseshoe = 396;

		public const short ObsidianShield = 397;

		public const short TinkerersWorkshop = 398;

		public const short CloudinaBalloon = 399;

		public const short AdamantiteHeadgear = 400;

		public const short AdamantiteHelmet = 401;

		public const short AdamantiteMask = 402;

		public const short AdamantiteBreastplate = 403;

		public const short AdamantiteLeggings = 404;

		public const short SpectreBoots = 405;

		public const short AdamantiteGlaive = 406;

		public const short Toolbelt = 407;

		public const short PearlsandBlock = 408;

		public const short PearlstoneBlock = 409;

		public const short MiningShirt = 410;

		public const short MiningPants = 411;

		public const short PearlstoneBrick = 412;

		public const short IridescentBrick = 413;

		public const short MudstoneBlock = 414;

		public const short CobaltBrick = 415;

		public const short MythrilBrick = 416;

		public const short PearlstoneBrickWall = 417;

		public const short IridescentBrickWall = 418;

		public const short MudstoneBrickWall = 419;

		public const short CobaltBrickWall = 420;

		public const short MythrilBrickWall = 421;

		public const short HolyWater = 422;

		public const short UnholyWater = 423;

		public const short SiltBlock = 424;

		public const short FairyBell = 425;

		public const short BreakerBlade = 426;

		public const short BlueTorch = 427;

		public const short RedTorch = 428;

		public const short GreenTorch = 429;

		public const short PurpleTorch = 430;

		public const short WhiteTorch = 431;

		public const short YellowTorch = 432;

		public const short DemonTorch = 433;

		public const short ClockworkAssaultRifle = 434;

		public const short CobaltRepeater = 435;

		public const short MythrilRepeater = 436;

		public const short DualHook = 437;

		public const short StarStatue = 438;

		public const short SwordStatue = 439;

		public const short SlimeStatue = 440;

		public const short GoblinStatue = 441;

		public const short ShieldStatue = 442;

		public const short BatStatue = 443;

		public const short FishStatue = 444;

		public const short BunnyStatue = 445;

		public const short SkeletonStatue = 446;

		public const short ReaperStatue = 447;

		public const short WomanStatue = 448;

		public const short ImpStatue = 449;

		public const short GargoyleStatue = 450;

		public const short GloomStatue = 451;

		public const short HornetStatue = 452;

		public const short BombStatue = 453;

		public const short CrabStatue = 454;

		public const short HammerStatue = 455;

		public const short PotionStatue = 456;

		public const short SpearStatue = 457;

		public const short CrossStatue = 458;

		public const short JellyfishStatue = 459;

		public const short BowStatue = 460;

		public const short BoomerangStatue = 461;

		public const short BootStatue = 462;

		public const short ChestStatue = 463;

		public const short BirdStatue = 464;

		public const short AxeStatue = 465;

		public const short CorruptStatue = 466;

		public const short TreeStatue = 467;

		public const short AnvilStatue = 468;

		public const short PickaxeStatue = 469;

		public const short MushroomStatue = 470;

		public const short EyeballStatue = 471;

		public const short PillarStatue = 472;

		public const short HeartStatue = 473;

		public const short PotStatue = 474;

		public const short SunflowerStatue = 475;

		public const short KingStatue = 476;

		public const short QueenStatue = 477;

		public const short PiranhaStatue = 478;

		public const short PlankedWall = 479;

		public const short WoodenBeam = 480;

		public const short AdamantiteRepeater = 481;

		public const short AdamantiteSword = 482;

		public const short CobaltSword = 483;

		public const short MythrilSword = 484;

		public const short MoonCharm = 485;

		public const short Ruler = 486;

		public const short CrystalBall = 487;

		public const short DiscoBall = 488;

		public const short SorcererEmblem = 489;

		public const short WarriorEmblem = 490;

		public const short RangerEmblem = 491;

		public const short DemonWings = 492;

		public const short AngelWings = 493;

		public const short MagicalHarp = 494;

		public const short RainbowRod = 495;

		public const short IceRod = 496;

		public const short NeptunesShell = 497;

		public const short Mannequin = 498;

		public const short GreaterHealingPotion = 499;

		public const short GreaterManaPotion = 500;

		public const short PixieDust = 501;

		public const short CrystalShard = 502;

		public const short ClownHat = 503;

		public const short ClownShirt = 504;

		public const short ClownPants = 505;

		public const short Flamethrower = 506;

		public const short Bell = 507;

		public const short Harp = 508;

		public const short Wrench = 509;

		public const short WireCutter = 510;

		public const short ActiveStoneBlock = 511;

		public const short InactiveStoneBlock = 512;

		public const short Lever = 513;

		public const short LaserRifle = 514;

		public const short CrystalBullet = 515;

		public const short HolyArrow = 516;

		public const short MagicDagger = 517;

		public const short CrystalStorm = 518;

		public const short CursedFlames = 519;

		public const short SoulofLight = 520;

		public const short SoulofNight = 521;

		public const short CursedFlame = 522;

		public const short CursedTorch = 523;

		public const short AdamantiteForge = 524;

		public const short MythrilAnvil = 525;

		public const short UnicornHorn = 526;

		public const short DarkShard = 527;

		public const short LightShard = 528;

		public const short RedPressurePlate = 529;

		public const short Wire = 530;

		public const short SpellTome = 531;

		public const short StarCloak = 532;

		public const short Megashark = 533;

		public const short Shotgun = 534;

		public const short PhilosophersStone = 535;

		public const short TitanGlove = 536;

		public const short CobaltNaginata = 537;

		public const short Switch = 538;

		public const short DartTrap = 539;

		public const short Boulder = 540;

		public const short GreenPressurePlate = 541;

		public const short GrayPressurePlate = 542;

		public const short BrownPressurePlate = 543;

		public const short MechanicalEye = 544;

		public const short CursedArrow = 545;

		public const short CursedBullet = 546;

		public const short SoulofFright = 547;

		public const short SoulofMight = 548;

		public const short SoulofSight = 549;

		public const short Gungnir = 550;

		public const short HallowedPlateMail = 551;

		public const short HallowedGreaves = 552;

		public const short HallowedHelmet = 553;

		public const short CrossNecklace = 554;

		public const short ManaFlower = 555;

		public const short MechanicalWorm = 556;

		public const short MechanicalSkull = 557;

		public const short HallowedHeadgear = 558;

		public const short HallowedMask = 559;

		public const short SlimeCrown = 560;

		public const short LightDisc = 561;

		public const short MusicBoxOverworldDay = 562;

		public const short MusicBoxEerie = 563;

		public const short MusicBoxNight = 564;

		public const short MusicBoxTitle = 565;

		public const short MusicBoxUnderground = 566;

		public const short MusicBoxBoss1 = 567;

		public const short MusicBoxJungle = 568;

		public const short MusicBoxCorruption = 569;

		public const short MusicBoxUndergroundCorruption = 570;

		public const short MusicBoxTheHallow = 571;

		public const short MusicBoxBoss2 = 572;

		public const short MusicBoxUndergroundHallow = 573;

		public const short MusicBoxBoss3 = 574;

		public const short SoulofFlight = 575;

		public const short MusicBox = 576;

		public const short DemoniteBrick = 577;

		public const short HallowedRepeater = 578;

		public const short Drax = 579;

		public const short Explosives = 580;

		public const short InletPump = 581;

		public const short OutletPump = 582;

		public const short Timer1Second = 583;

		public const short Timer3Second = 584;

		public const short Timer5Second = 585;

		public const short CandyCaneBlock = 586;

		public const short CandyCaneWall = 587;

		public const short SantaHat = 588;

		public const short SantaShirt = 589;

		public const short SantaPants = 590;

		public const short GreenCandyCaneBlock = 591;

		public const short GreenCandyCaneWall = 592;

		public const short SnowBlock = 593;

		public const short SnowBrick = 594;

		public const short SnowBrickWall = 595;

		public const short BlueLight = 596;

		public const short RedLight = 597;

		public const short GreenLight = 598;

		public const short BluePresent = 599;

		public const short GreenPresent = 600;

		public const short YellowPresent = 601;

		public const short SnowGlobe = 602;

		public const short Carrot = 603;

		public const short AdamantiteBeam = 604;

		public const short AdamantiteBeamWall = 605;

		public const short DemoniteBrickWall = 606;

		public const short SandstoneBrick = 607;

		public const short SandstoneBrickWall = 608;

		public const short EbonstoneBrick = 609;

		public const short EbonstoneBrickWall = 610;

		public const short RedStucco = 611;

		public const short YellowStucco = 612;

		public const short GreenStucco = 613;

		public const short GrayStucco = 614;

		public const short RedStuccoWall = 615;

		public const short YellowStuccoWall = 616;

		public const short GreenStuccoWall = 617;

		public const short GrayStuccoWall = 618;

		public const short Ebonwood = 619;

		public const short RichMahogany = 620;

		public const short Pearlwood = 621;

		public const short EbonwoodWall = 622;

		public const short RichMahoganyWall = 623;

		public const short PearlwoodWall = 624;

		public const short EbonwoodChest = 625;

		public const short RichMahoganyChest = 626;

		public const short PearlwoodChest = 627;

		public const short EbonwoodChair = 628;

		public const short RichMahoganyChair = 629;

		public const short PearlwoodChair = 630;

		public const short EbonwoodPlatform = 631;

		public const short RichMahoganyPlatform = 632;

		public const short PearlwoodPlatform = 633;

		public const short BonePlatform = 634;

		public const short EbonwoodWorkBench = 635;

		public const short RichMahoganyWorkBench = 636;

		public const short PearlwoodWorkBench = 637;

		public const short EbonwoodTable = 638;

		public const short RichMahoganyTable = 639;

		public const short PearlwoodTable = 640;

		public const short EbonwoodPiano = 641;

		public const short RichMahoganyPiano = 642;

		public const short PearlwoodPiano = 643;

		public const short EbonwoodBed = 644;

		public const short RichMahoganyBed = 645;

		public const short PearlwoodBed = 646;

		public const short EbonwoodDresser = 647;

		public const short RichMahoganyDresser = 648;

		public const short PearlwoodDresser = 649;

		public const short EbonwoodDoor = 650;

		public const short RichMahoganyDoor = 651;

		public const short PearlwoodDoor = 652;

		public const short EbonwoodSword = 653;

		public const short EbonwoodHammer = 654;

		public const short EbonwoodBow = 655;

		public const short RichMahoganySword = 656;

		public const short RichMahoganyHammer = 657;

		public const short RichMahoganyBow = 658;

		public const short PearlwoodSword = 659;

		public const short PearlwoodHammer = 660;

		public const short PearlwoodBow = 661;

		public const short RainbowBrick = 662;

		public const short RainbowBrickWall = 663;

		public const short IceBlock = 664;

		public const short RedsWings = 665;

		public const short RedsHelmet = 666;

		public const short RedsBreastplate = 667;

		public const short RedsLeggings = 668;

		public const short Fish = 669;

		public const short IceBoomerang = 670;

		public const short Keybrand = 671;

		public const short Cutlass = 672;

		public const short BorealWoodWorkBench = 673;

		public const short TrueExcalibur = 674;

		public const short TrueNightsEdge = 675;

		public const short Frostbrand = 676;

		public const short BorealWoodTable = 677;

		public const short RedPotion = 678;

		public const short TacticalShotgun = 679;

		public const short IvyChest = 680;

		public const short IceChest = 681;

		public const short Marrow = 682;

		public const short UnholyTrident = 683;

		public const short FrostHelmet = 684;

		public const short FrostBreastplate = 685;

		public const short FrostLeggings = 686;

		public const short TinHelmet = 687;

		public const short TinChainmail = 688;

		public const short TinGreaves = 689;

		public const short LeadHelmet = 690;

		public const short LeadChainmail = 691;

		public const short LeadGreaves = 692;

		public const short TungstenHelmet = 693;

		public const short TungstenChainmail = 694;

		public const short TungstenGreaves = 695;

		public const short PlatinumHelmet = 696;

		public const short PlatinumChainmail = 697;

		public const short PlatinumGreaves = 698;

		public const short TinOre = 699;

		public const short LeadOre = 700;

		public const short TungstenOre = 701;

		public const short PlatinumOre = 702;

		public const short TinBar = 703;

		public const short LeadBar = 704;

		public const short TungstenBar = 705;

		public const short PlatinumBar = 706;

		public const short TinWatch = 707;

		public const short TungstenWatch = 708;

		public const short PlatinumWatch = 709;

		public const short TinChandelier = 710;

		public const short TungstenChandelier = 711;

		public const short PlatinumChandelier = 712;

		public const short PlatinumCandle = 713;

		public const short PlatinumCandelabra = 714;

		public const short PlatinumCrown = 715;

		public const short LeadAnvil = 716;

		public const short TinBrick = 717;

		public const short TungstenBrick = 718;

		public const short PlatinumBrick = 719;

		public const short TinBrickWall = 720;

		public const short TungstenBrickWall = 721;

		public const short PlatinumBrickWall = 722;

		public const short BeamSword = 723;

		public const short IceBlade = 724;

		public const short IceBow = 725;

		public const short FrostStaff = 726;

		public const short WoodHelmet = 727;

		public const short WoodBreastplate = 728;

		public const short WoodGreaves = 729;

		public const short EbonwoodHelmet = 730;

		public const short EbonwoodBreastplate = 731;

		public const short EbonwoodGreaves = 732;

		public const short RichMahoganyHelmet = 733;

		public const short RichMahoganyBreastplate = 734;

		public const short RichMahoganyGreaves = 735;

		public const short PearlwoodHelmet = 736;

		public const short PearlwoodBreastplate = 737;

		public const short PearlwoodGreaves = 738;

		public const short AmethystStaff = 739;

		public const short TopazStaff = 740;

		public const short SapphireStaff = 741;

		public const short EmeraldStaff = 742;

		public const short RubyStaff = 743;

		public const short DiamondStaff = 744;

		public const short GrassWall = 745;

		public const short JungleWall = 746;

		public const short FlowerWall = 747;

		public const short Jetpack = 748;

		public const short ButterflyWings = 749;

		public const short CactusWall = 750;

		public const short Cloud = 751;

		public const short CloudWall = 752;

		public const short Seaweed = 753;

		public const short RuneHat = 754;

		public const short RuneRobe = 755;

		public const short MushroomSpear = 756;

		public const short TerraBlade = 757;

		public const short GrenadeLauncher = 758;

		public const short RocketLauncher = 759;

		public const short ProximityMineLauncher = 760;

		public const short FairyWings = 761;

		public const short SlimeBlock = 762;

		public const short FleshBlock = 763;

		public const short MushroomWall = 764;

		public const short RainCloud = 765;

		public const short BoneBlock = 766;

		public const short FrozenSlimeBlock = 767;

		public const short BoneBlockWall = 768;

		public const short SlimeBlockWall = 769;

		public const short FleshBlockWall = 770;

		public const short RocketI = 771;

		public const short RocketII = 772;

		public const short RocketIII = 773;

		public const short RocketIV = 774;

		public const short AsphaltBlock = 775;

		public const short CobaltPickaxe = 776;

		public const short MythrilPickaxe = 777;

		public const short AdamantitePickaxe = 778;

		public const short Clentaminator = 779;

		public const short GreenSolution = 780;

		public const short BlueSolution = 781;

		public const short PurpleSolution = 782;

		public const short DarkBlueSolution = 783;

		public const short RedSolution = 784;

		public const short HarpyWings = 785;

		public const short BoneWings = 786;

		public const short Hammush = 787;

		public const short NettleBurst = 788;

		public const short AnkhBanner = 789;

		public const short SnakeBanner = 790;

		public const short OmegaBanner = 791;

		public const short CrimsonHelmet = 792;

		public const short CrimsonScalemail = 793;

		public const short CrimsonGreaves = 794;

		public const short BloodButcherer = 795;

		public const short TendonBow = 796;

		public const short FleshGrinder = 797;

		public const short DeathbringerPickaxe = 798;

		public const short BloodLustCluster = 799;

		public const short TheUndertaker = 800;

		public const short TheMeatball = 801;

		public const short TheRottedFork = 802;

		public const short EskimoHood = 803;

		public const short EskimoCoat = 804;

		public const short EskimoPants = 805;

		public const short LivingWoodChair = 806;

		public const short CactusChair = 807;

		public const short BoneChair = 808;

		public const short FleshChair = 809;

		public const short MushroomChair = 810;

		public const short BoneWorkBench = 811;

		public const short CactusWorkBench = 812;

		public const short FleshWorkBench = 813;

		public const short MushroomWorkBench = 814;

		public const short SlimeWorkBench = 815;

		public const short CactusDoor = 816;

		public const short FleshDoor = 817;

		public const short MushroomDoor = 818;

		public const short LivingWoodDoor = 819;

		public const short BoneDoor = 820;

		public const short FlameWings = 821;

		public const short FrozenWings = 822;

		public const short GhostWings = 823;

		public const short SunplateBlock = 824;

		public const short DiscWall = 825;

		public const short SkywareChair = 826;

		public const short BoneTable = 827;

		public const short FleshTable = 828;

		public const short LivingWoodTable = 829;

		public const short SkywareTable = 830;

		public const short LivingWoodChest = 831;

		public const short LivingWoodWand = 832;

		public const short PurpleIceBlock = 833;

		public const short PinkIceBlock = 834;

		public const short RedIceBlock = 835;

		public const short CrimstoneBlock = 836;

		public const short SkywareDoor = 837;

		public const short SkywareChest = 838;

		public const short SteampunkHat = 839;

		public const short SteampunkShirt = 840;

		public const short SteampunkPants = 841;

		public const short BeeHat = 842;

		public const short BeeShirt = 843;

		public const short BeePants = 844;

		public const short WorldBanner = 845;

		public const short SunBanner = 846;

		public const short GravityBanner = 847;

		public const short PharaohsMask = 848;

		public const short Actuator = 849;

		public const short BlueWrench = 850;

		public const short GreenWrench = 851;

		public const short BluePressurePlate = 852;

		public const short YellowPressurePlate = 853;

		public const short DiscountCard = 854;

		public const short LuckyCoin = 855;

		public const short UnicornonaStick = 856;

		public const short SandstorminaBottle = 857;

		public const short BorealWoodSofa = 858;

		public const short BeachBall = 859;

		public const short CharmofMyths = 860;

		public const short MoonShell = 861;

		public const short StarVeil = 862;

		public const short WaterWalkingBoots = 863;

		public const short Tiara = 864;

		public const short PrincessDress = 865;

		public const short PharaohsRobe = 866;

		public const short GreenCap = 867;

		public const short MushroomCap = 868;

		public const short TamOShanter = 869;

		public const short MummyMask = 870;

		public const short MummyShirt = 871;

		public const short MummyPants = 872;

		public const short CowboyHat = 873;

		public const short CowboyJacket = 874;

		public const short CowboyPants = 875;

		public const short PirateHat = 876;

		public const short PirateShirt = 877;

		public const short PiratePants = 878;

		public const short VikingHelmet = 879;

		public const short CrimtaneOre = 880;

		public const short CactusSword = 881;

		public const short CactusPickaxe = 882;

		public const short IceBrick = 883;

		public const short IceBrickWall = 884;

		public const short AdhesiveBandage = 885;

		public const short ArmorPolish = 886;

		public const short Bezoar = 887;

		public const short Blindfold = 888;

		public const short FastClock = 889;

		public const short Megaphone = 890;

		public const short Nazar = 891;

		public const short Vitamins = 892;

		public const short TrifoldMap = 893;

		public const short CactusHelmet = 894;

		public const short CactusBreastplate = 895;

		public const short CactusLeggings = 896;

		public const short PowerGlove = 897;

		public const short LightningBoots = 898;

		public const short SunStone = 899;

		public const short MoonStone = 900;

		public const short ArmorBracing = 901;

		public const short MedicatedBandage = 902;

		public const short ThePlan = 903;

		public const short CountercurseMantra = 904;

		public const short CoinGun = 905;

		public const short LavaCharm = 906;

		public const short ObsidianWaterWalkingBoots = 907;

		public const short LavaWaders = 908;

		public const short PureWaterFountain = 909;

		public const short DesertWaterFountain = 910;

		public const short Shadewood = 911;

		public const short ShadewoodDoor = 912;

		public const short ShadewoodPlatform = 913;

		public const short ShadewoodChest = 914;

		public const short ShadewoodChair = 915;

		public const short ShadewoodWorkBench = 916;

		public const short ShadewoodTable = 917;

		public const short ShadewoodDresser = 918;

		public const short ShadewoodPiano = 919;

		public const short ShadewoodBed = 920;

		public const short ShadewoodSword = 921;

		public const short ShadewoodHammer = 922;

		public const short ShadewoodBow = 923;

		public const short ShadewoodHelmet = 924;

		public const short ShadewoodBreastplate = 925;

		public const short ShadewoodGreaves = 926;

		public const short ShadewoodWall = 927;

		public const short Cannon = 928;

		public const short Cannonball = 929;

		public const short FlareGun = 930;

		public const short Flare = 931;

		public const short BoneWand = 932;

		public const short LeafWand = 933;

		public const short FlyingCarpet = 934;

		public const short AvengerEmblem = 935;

		public const short MechanicalGlove = 936;

		public const short LandMine = 937;

		public const short PaladinsShield = 938;

		public const short WebSlinger = 939;

		public const short JungleWaterFountain = 940;

		public const short IcyWaterFountain = 941;

		public const short CorruptWaterFountain = 942;

		public const short CrimsonWaterFountain = 943;

		public const short HallowedWaterFountain = 944;

		public const short BloodWaterFountain = 945;

		public const short Umbrella = 946;

		public const short ChlorophyteOre = 947;

		public const short SteampunkWings = 948;

		public const short Snowball = 949;

		public const short IceSkates = 950;

		public const short SnowballLauncher = 951;

		public const short WebCoveredChest = 952;

		public const short ClimbingClaws = 953;

		public const short AncientIronHelmet = 954;

		public const short AncientGoldHelmet = 955;

		public const short AncientShadowHelmet = 956;

		public const short AncientShadowScalemail = 957;

		public const short AncientShadowGreaves = 958;

		public const short AncientNecroHelmet = 959;

		public const short AncientCobaltHelmet = 960;

		public const short AncientCobaltBreastplate = 961;

		public const short AncientCobaltLeggings = 962;

		public const short BlackBelt = 963;

		public const short Boomstick = 964;

		public const short Rope = 965;

		public const short Campfire = 966;

		public const short Marshmallow = 967;

		public const short MarshmallowonaStick = 968;

		public const short CookedMarshmallow = 969;

		public const short RedRocket = 970;

		public const short GreenRocket = 971;

		public const short BlueRocket = 972;

		public const short YellowRocket = 973;

		public const short IceTorch = 974;

		public const short ShoeSpikes = 975;

		public const short TigerClimbingGear = 976;

		public const short Tabi = 977;

		public const short PinkEskimoHood = 978;

		public const short PinkEskimoCoat = 979;

		public const short PinkEskimoPants = 980;

		public const short PinkThread = 981;

		public const short ManaRegenerationBand = 982;

		public const short SandstorminaBalloon = 983;

		public const short MasterNinjaGear = 984;

		public const short RopeCoil = 985;

		public const short Blowgun = 986;

		public const short BlizzardinaBottle = 987;

		public const short FrostburnArrow = 988;

		public const short EnchantedSword = 989;

		public const short PickaxeAxe = 990;

		public const short CobaltWaraxe = 991;

		public const short MythrilWaraxe = 992;

		public const short AdamantiteWaraxe = 993;

		public const short EatersBone = 994;

		public const short BlendOMatic = 995;

		public const short MeatGrinder = 996;

		public const short Extractinator = 997;

		public const short Solidifier = 998;

		public const short Amber = 999;

		public const short ConfettiGun = 1000;

		public const short ChlorophyteMask = 1001;

		public const short ChlorophyteHelmet = 1002;

		public const short ChlorophyteHeadgear = 1003;

		public const short ChlorophytePlateMail = 1004;

		public const short ChlorophyteGreaves = 1005;

		public const short ChlorophyteBar = 1006;

		public const short RedDye = 1007;

		public const short OrangeDye = 1008;

		public const short YellowDye = 1009;

		public const short LimeDye = 1010;

		public const short GreenDye = 1011;

		public const short TealDye = 1012;

		public const short CyanDye = 1013;

		public const short SkyBlueDye = 1014;

		public const short BlueDye = 1015;

		public const short PurpleDye = 1016;

		public const short VioletDye = 1017;

		public const short PinkDye = 1018;

		public const short RedandBlackDye = 1019;

		public const short OrangeandBlackDye = 1020;

		public const short YellowandBlackDye = 1021;

		public const short LimeandBlackDye = 1022;

		public const short GreenandBlackDye = 1023;

		public const short TealandBlackDye = 1024;

		public const short CyanandBlackDye = 1025;

		public const short SkyBlueandBlackDye = 1026;

		public const short BlueandBlackDye = 1027;

		public const short PurpleandBlackDye = 1028;

		public const short VioletandBlackDye = 1029;

		public const short PinkandBlackDye = 1030;

		public const short FlameDye = 1031;

		public const short FlameAndBlackDye = 1032;

		public const short GreenFlameDye = 1033;

		public const short GreenFlameAndBlackDye = 1034;

		public const short BlueFlameDye = 1035;

		public const short BlueFlameAndBlackDye = 1036;

		public const short SilverDye = 1037;

		public const short BrightRedDye = 1038;

		public const short BrightOrangeDye = 1039;

		public const short BrightYellowDye = 1040;

		public const short BrightLimeDye = 1041;

		public const short BrightGreenDye = 1042;

		public const short BrightTealDye = 1043;

		public const short BrightCyanDye = 1044;

		public const short BrightSkyBlueDye = 1045;

		public const short BrightBlueDye = 1046;

		public const short BrightPurpleDye = 1047;

		public const short BrightVioletDye = 1048;

		public const short BrightPinkDye = 1049;

		public const short BlackDye = 1050;

		public const short RedandSilverDye = 1051;

		public const short OrangeandSilverDye = 1052;

		public const short YellowandSilverDye = 1053;

		public const short LimeandSilverDye = 1054;

		public const short GreenandSilverDye = 1055;

		public const short TealandSilverDye = 1056;

		public const short CyanandSilverDye = 1057;

		public const short SkyBlueandSilverDye = 1058;

		public const short BlueandSilverDye = 1059;

		public const short PurpleandSilverDye = 1060;

		public const short VioletandSilverDye = 1061;

		public const short PinkandSilverDye = 1062;

		public const short IntenseFlameDye = 1063;

		public const short IntenseGreenFlameDye = 1064;

		public const short IntenseBlueFlameDye = 1065;

		public const short RainbowDye = 1066;

		public const short IntenseRainbowDye = 1067;

		public const short YellowGradientDye = 1068;

		public const short CyanGradientDye = 1069;

		public const short VioletGradientDye = 1070;

		public const short Paintbrush = 1071;

		public const short PaintRoller = 1072;

		public const short RedPaint = 1073;

		public const short OrangePaint = 1074;

		public const short YellowPaint = 1075;

		public const short LimePaint = 1076;

		public const short GreenPaint = 1077;

		public const short TealPaint = 1078;

		public const short CyanPaint = 1079;

		public const short SkyBluePaint = 1080;

		public const short BluePaint = 1081;

		public const short PurplePaint = 1082;

		public const short VioletPaint = 1083;

		public const short PinkPaint = 1084;

		public const short DeepRedPaint = 1085;

		public const short DeepOrangePaint = 1086;

		public const short DeepYellowPaint = 1087;

		public const short DeepLimePaint = 1088;

		public const short DeepGreenPaint = 1089;

		public const short DeepTealPaint = 1090;

		public const short DeepCyanPaint = 1091;

		public const short DeepSkyBluePaint = 1092;

		public const short DeepBluePaint = 1093;

		public const short DeepPurplePaint = 1094;

		public const short DeepVioletPaint = 1095;

		public const short DeepPinkPaint = 1096;

		public const short BlackPaint = 1097;

		public const short WhitePaint = 1098;

		public const short GrayPaint = 1099;

		public const short PaintScraper = 1100;

		public const short LihzahrdBrick = 1101;

		public const short LihzahrdBrickWall = 1102;

		public const short SlushBlock = 1103;

		public const short PalladiumOre = 1104;

		public const short OrichalcumOre = 1105;

		public const short TitaniumOre = 1106;

		public const short TealMushroom = 1107;

		public const short GreenMushroom = 1108;

		public const short SkyBlueFlower = 1109;

		public const short YellowMarigold = 1110;

		public const short BlueBerries = 1111;

		public const short LimeKelp = 1112;

		public const short PinkPricklyPear = 1113;

		public const short OrangeBloodroot = 1114;

		public const short RedHusk = 1115;

		public const short CyanHusk = 1116;

		public const short VioletHusk = 1117;

		public const short PurpleMucos = 1118;

		public const short BlackInk = 1119;

		public const short DyeVat = 1120;

		public const short BeeGun = 1121;

		public const short PossessedHatchet = 1122;

		public const short BeeKeeper = 1123;

		public const short Hive = 1124;

		public const short HoneyBlock = 1125;

		public const short HiveWall = 1126;

		public const short CrispyHoneyBlock = 1127;

		public const short HoneyBucket = 1128;

		public const short HiveWand = 1129;

		public const short Beenade = 1130;

		public const short GravityGlobe = 1131;

		public const short HoneyComb = 1132;

		public const short Abeemination = 1133;

		public const short BottledHoney = 1134;

		public const short RainHat = 1135;

		public const short RainCoat = 1136;

		public const short LihzahrdDoor = 1137;

		public const short DungeonDoor = 1138;

		public const short LeadDoor = 1139;

		public const short IronDoor = 1140;

		public const short TempleKey = 1141;

		public const short LihzahrdChest = 1142;

		public const short LihzahrdChair = 1143;

		public const short LihzahrdTable = 1144;

		public const short LihzahrdWorkBench = 1145;

		public const short SuperDartTrap = 1146;

		public const short FlameTrap = 1147;

		public const short SpikyBallTrap = 1148;

		public const short SpearTrap = 1149;

		public const short WoodenSpike = 1150;

		public const short LihzahrdPressurePlate = 1151;

		public const short LihzahrdStatue = 1152;

		public const short LihzahrdWatcherStatue = 1153;

		public const short LihzahrdGuardianStatue = 1154;

		public const short WaspGun = 1155;

		public const short PiranhaGun = 1156;

		public const short PygmyStaff = 1157;

		public const short PygmyNecklace = 1158;

		public const short TikiMask = 1159;

		public const short TikiShirt = 1160;

		public const short TikiPants = 1161;

		public const short LeafWings = 1162;

		public const short BlizzardinaBalloon = 1163;

		public const short BundleofBalloons = 1164;

		public const short BatWings = 1165;

		public const short BoneSword = 1166;

		public const short HerculesBeetle = 1167;

		public const short SmokeBomb = 1168;

		public const short BoneKey = 1169;

		public const short Nectar = 1170;

		public const short TikiTotem = 1171;

		public const short LizardEgg = 1172;

		public const short GraveMarker = 1173;

		public const short CrossGraveMarker = 1174;

		public const short Headstone = 1175;

		public const short Gravestone = 1176;

		public const short Obelisk = 1177;

		public const short LeafBlower = 1178;

		public const short ChlorophyteBullet = 1179;

		public const short ParrotCracker = 1180;

		public const short StrangeGlowingMushroom = 1181;

		public const short Seedling = 1182;

		public const short WispinaBottle = 1183;

		public const short PalladiumBar = 1184;

		public const short PalladiumSword = 1185;

		public const short PalladiumPike = 1186;

		public const short PalladiumRepeater = 1187;

		public const short PalladiumPickaxe = 1188;

		public const short PalladiumDrill = 1189;

		public const short PalladiumChainsaw = 1190;

		public const short OrichalcumBar = 1191;

		public const short OrichalcumSword = 1192;

		public const short OrichalcumHalberd = 1193;

		public const short OrichalcumRepeater = 1194;

		public const short OrichalcumPickaxe = 1195;

		public const short OrichalcumDrill = 1196;

		public const short OrichalcumChainsaw = 1197;

		public const short TitaniumBar = 1198;

		public const short TitaniumSword = 1199;

		public const short TitaniumTrident = 1200;

		public const short TitaniumRepeater = 1201;

		public const short TitaniumPickaxe = 1202;

		public const short TitaniumDrill = 1203;

		public const short TitaniumChainsaw = 1204;

		public const short PalladiumMask = 1205;

		public const short PalladiumHelmet = 1206;

		public const short PalladiumHeadgear = 1207;

		public const short PalladiumBreastplate = 1208;

		public const short PalladiumLeggings = 1209;

		public const short OrichalcumMask = 1210;

		public const short OrichalcumHelmet = 1211;

		public const short OrichalcumHeadgear = 1212;

		public const short OrichalcumBreastplate = 1213;

		public const short OrichalcumLeggings = 1214;

		public const short TitaniumMask = 1215;

		public const short TitaniumHelmet = 1216;

		public const short TitaniumHeadgear = 1217;

		public const short TitaniumBreastplate = 1218;

		public const short TitaniumLeggings = 1219;

		public const short OrichalcumAnvil = 1220;

		public const short TitaniumForge = 1221;

		public const short PalladiumWaraxe = 1222;

		public const short OrichalcumWaraxe = 1223;

		public const short TitaniumWaraxe = 1224;

		public const short HallowedBar = 1225;

		public const short ChlorophyteClaymore = 1226;

		public const short ChlorophyteSaber = 1227;

		public const short ChlorophytePartisan = 1228;

		public const short ChlorophyteShotbow = 1229;

		public const short ChlorophytePickaxe = 1230;

		public const short ChlorophyteDrill = 1231;

		public const short ChlorophyteChainsaw = 1232;

		public const short ChlorophyteGreataxe = 1233;

		public const short ChlorophyteWarhammer = 1234;

		public const short ChlorophyteArrow = 1235;

		public const short AmethystHook = 1236;

		public const short TopazHook = 1237;

		public const short SapphireHook = 1238;

		public const short EmeraldHook = 1239;

		public const short RubyHook = 1240;

		public const short DiamondHook = 1241;

		public const short AmberMosquito = 1242;

		public const short UmbrellaHat = 1243;

		public const short NimbusRod = 1244;

		public const short OrangeTorch = 1245;

		public const short CrimsandBlock = 1246;

		public const short BeeCloak = 1247;

		public const short EyeoftheGolem = 1248;

		public const short HoneyBalloon = 1249;

		public const short BlueHorseshoeBalloon = 1250;

		public const short WhiteHorseshoeBalloon = 1251;

		public const short YellowHorseshoeBalloon = 1252;

		public const short FrozenTurtleShell = 1253;

		public const short SniperRifle = 1254;

		public const short VenusMagnum = 1255;

		public const short CrimsonRod = 1256;

		public const short CrimtaneBar = 1257;

		public const short Stynger = 1258;

		public const short FlowerPow = 1259;

		public const short RainbowGun = 1260;

		public const short StyngerBolt = 1261;

		public const short ChlorophyteJackhammer = 1262;

		public const short Teleporter = 1263;

		public const short FlowerofFrost = 1264;

		public const short Uzi = 1265;

		public const short MagnetSphere = 1266;

		public const short PurpleStainedGlass = 1267;

		public const short YellowStainedGlass = 1268;

		public const short BlueStainedGlass = 1269;

		public const short GreenStainedGlass = 1270;

		public const short RedStainedGlass = 1271;

		public const short MulticoloredStainedGlass = 1272;

		public const short SkeletronHand = 1273;

		public const short Skull = 1274;

		public const short BallaHat = 1275;

		public const short GangstaHat = 1276;

		public const short SailorHat = 1277;

		public const short EyePatch = 1278;

		public const short SailorShirt = 1279;

		public const short SailorPants = 1280;

		public const short SkeletronMask = 1281;

		public const short AmethystRobe = 1282;

		public const short TopazRobe = 1283;

		public const short SapphireRobe = 1284;

		public const short EmeraldRobe = 1285;

		public const short RubyRobe = 1286;

		public const short DiamondRobe = 1287;

		public const short WhiteTuxedoShirt = 1288;

		public const short WhiteTuxedoPants = 1289;

		public const short PanicNecklace = 1290;

		public const short LifeFruit = 1291;

		public const short LihzahrdAltar = 1292;

		public const short LihzahrdPowerCell = 1293;

		public const short Picksaw = 1294;

		public const short HeatRay = 1295;

		public const short StaffofEarth = 1296;

		public const short GolemFist = 1297;

		public const short WaterChest = 1298;

		public const short Binoculars = 1299;

		public const short RifleScope = 1300;

		public const short DestroyerEmblem = 1301;

		public const short HighVelocityBullet = 1302;

		public const short JellyfishNecklace = 1303;

		public const short ZombieArm = 1304;

		public const short TheAxe = 1305;

		public const short IceSickle = 1306;

		public const short ClothierVoodooDoll = 1307;

		public const short PoisonStaff = 1308;

		public const short SlimeStaff = 1309;

		public const short PoisonDart = 1310;

		public const short EyeSpring = 1311;

		public const short ToySled = 1312;

		public const short BookofSkulls = 1313;

		public const short KOCannon = 1314;

		public const short PirateMap = 1315;

		public const short TurtleHelmet = 1316;

		public const short TurtleScaleMail = 1317;

		public const short TurtleLeggings = 1318;

		public const short SnowballCannon = 1319;

		public const short BonePickaxe = 1320;

		public const short MagicQuiver = 1321;

		public const short MagmaStone = 1322;

		public const short ObsidianRose = 1323;

		public const short Bananarang = 1324;

		public const short ChainKnife = 1325;

		public const short RodofDiscord = 1326;

		public const short DeathSickle = 1327;

		public const short TurtleShell = 1328;

		public const short TissueSample = 1329;

		public const short Vertebrae = 1330;

		public const short BloodySpine = 1331;

		public const short Ichor = 1332;

		public const short IchorTorch = 1333;

		public const short IchorArrow = 1334;

		public const short IchorBullet = 1335;

		public const short GoldenShower = 1336;

		public const short BunnyCannon = 1337;

		public const short ExplosiveBunny = 1338;

		public const short VialofVenom = 1339;

		public const short FlaskofVenom = 1340;

		public const short VenomArrow = 1341;

		public const short VenomBullet = 1342;

		public const short FireGauntlet = 1343;

		public const short Cog = 1344;

		public const short Confetti = 1345;

		public const short Nanites = 1346;

		public const short ExplosivePowder = 1347;

		public const short GoldDust = 1348;

		public const short PartyBullet = 1349;

		public const short NanoBullet = 1350;

		public const short ExplodingBullet = 1351;

		public const short GoldenBullet = 1352;

		public const short FlaskofCursedFlames = 1353;

		public const short FlaskofFire = 1354;

		public const short FlaskofGold = 1355;

		public const short FlaskofIchor = 1356;

		public const short FlaskofNanites = 1357;

		public const short FlaskofParty = 1358;

		public const short FlaskofPoison = 1359;

		public const short EyeofCthulhuTrophy = 1360;

		public const short EaterofWorldsTrophy = 1361;

		public const short BrainofCthulhuTrophy = 1362;

		public const short SkeletronTrophy = 1363;

		public const short QueenBeeTrophy = 1364;

		public const short WallofFleshTrophy = 1365;

		public const short DestroyerTrophy = 1366;

		public const short SkeletronPrimeTrophy = 1367;

		public const short RetinazerTrophy = 1368;

		public const short SpazmatismTrophy = 1369;

		public const short PlanteraTrophy = 1370;

		public const short GolemTrophy = 1371;

		public const short BloodMoonRising = 1372;

		public const short TheHangedMan = 1373;

		public const short GloryoftheFire = 1374;

		public const short BoneWarp = 1375;

		public const short WallSkeleton = 1376;

		public const short HangingSkeleton = 1377;

		public const short BlueSlabWall = 1378;

		public const short BlueTiledWall = 1379;

		public const short PinkSlabWall = 1380;

		public const short PinkTiledWall = 1381;

		public const short GreenSlabWall = 1382;

		public const short GreenTiledWall = 1383;

		public const short BlueBrickPlatform = 1384;

		public const short PinkBrickPlatform = 1385;

		public const short GreenBrickPlatform = 1386;

		public const short MetalShelf = 1387;

		public const short BrassShelf = 1388;

		public const short WoodShelf = 1389;

		public const short BrassLantern = 1390;

		public const short CagedLantern = 1391;

		public const short CarriageLantern = 1392;

		public const short AlchemyLantern = 1393;

		public const short DiablostLamp = 1394;

		public const short OilRagSconse = 1395;

		public const short BlueDungeonChair = 1396;

		public const short BlueDungeonTable = 1397;

		public const short BlueDungeonWorkBench = 1398;

		public const short GreenDungeonChair = 1399;

		public const short GreenDungeonTable = 1400;

		public const short GreenDungeonWorkBench = 1401;

		public const short PinkDungeonChair = 1402;

		public const short PinkDungeonTable = 1403;

		public const short PinkDungeonWorkBench = 1404;

		public const short BlueDungeonCandle = 1405;

		public const short GreenDungeonCandle = 1406;

		public const short PinkDungeonCandle = 1407;

		public const short BlueDungeonVase = 1408;

		public const short GreenDungeonVase = 1409;

		public const short PinkDungeonVase = 1410;

		public const short BlueDungeonDoor = 1411;

		public const short GreenDungeonDoor = 1412;

		public const short PinkDungeonDoor = 1413;

		public const short BlueDungeonBookcase = 1414;

		public const short GreenDungeonBookcase = 1415;

		public const short PinkDungeonBookcase = 1416;

		public const short Catacomb = 1417;

		public const short DungeonShelf = 1418;

		public const short SkellingtonJSkellingsworth = 1419;

		public const short TheCursedMan = 1420;

		public const short TheEyeSeestheEnd = 1421;

		public const short SomethingEvilisWatchingYou = 1422;

		public const short TheTwinsHaveAwoken = 1423;

		public const short TheScreamer = 1424;

		public const short GoblinsPlayingPoker = 1425;

		public const short Dryadisque = 1426;

		public const short Sunflowers = 1427;

		public const short TerrarianGothic = 1428;

		public const short Beanie = 1429;

		public const short ImbuingStation = 1430;

		public const short StarinaBottle = 1431;

		public const short EmptyBullet = 1432;

		public const short Impact = 1433;

		public const short PoweredbyBirds = 1434;

		public const short TheDestroyer = 1435;

		public const short ThePersistencyofEyes = 1436;

		public const short UnicornCrossingtheHallows = 1437;

		public const short GreatWave = 1438;

		public const short StarryNight = 1439;

		public const short GuidePicasso = 1440;

		public const short TheGuardiansGaze = 1441;

		public const short FatherofSomeone = 1442;

		public const short NurseLisa = 1443;

		public const short ShadowbeamStaff = 1444;

		public const short InfernoFork = 1445;

		public const short SpectreStaff = 1446;

		public const short WoodenFence = 1447;

		public const short LeadFence = 1448;

		public const short BubbleMachine = 1449;

		public const short BubbleWand = 1450;

		public const short MarchingBonesBanner = 1451;

		public const short NecromanticSign = 1452;

		public const short RustedCompanyStandard = 1453;

		public const short RaggedBrotherhoodSigil = 1454;

		public const short MoltenLegionFlag = 1455;

		public const short DiabolicSigil = 1456;

		public const short ObsidianPlatform = 1457;

		public const short ObsidianDoor = 1458;

		public const short ObsidianChair = 1459;

		public const short ObsidianTable = 1460;

		public const short ObsidianWorkBench = 1461;

		public const short ObsidianVase = 1462;

		public const short ObsidianBookcase = 1463;

		public const short HellboundBanner = 1464;

		public const short HellHammerBanner = 1465;

		public const short HelltowerBanner = 1466;

		public const short LostHopesofManBanner = 1467;

		public const short ObsidianWatcherBanner = 1468;

		public const short LavaEruptsBanner = 1469;

		public const short BlueDungeonBed = 1470;

		public const short GreenDungeonBed = 1471;

		public const short PinkDungeonBed = 1472;

		public const short ObsidianBed = 1473;

		public const short Waldo = 1474;

		public const short Darkness = 1475;

		public const short DarkSoulReaper = 1476;

		public const short Land = 1477;

		public const short TrappedGhost = 1478;

		public const short DemonsEye = 1479;

		public const short FindingGold = 1480;

		public const short FirstEncounter = 1481;

		public const short GoodMorning = 1482;

		public const short UndergroundReward = 1483;

		public const short ThroughtheWindow = 1484;

		public const short PlaceAbovetheClouds = 1485;

		public const short DoNotStepontheGrass = 1486;

		public const short ColdWatersintheWhiteLand = 1487;

		public const short LightlessChasms = 1488;

		public const short TheLandofDeceivingLooks = 1489;

		public const short Daylight = 1490;

		public const short SecretoftheSands = 1491;

		public const short DeadlandComesAlive = 1492;

		public const short EvilPresence = 1493;

		public const short SkyGuardian = 1494;

		public const short AmericanExplosive = 1495;

		public const short Discover = 1496;

		public const short HandEarth = 1497;

		public const short OldMiner = 1498;

		public const short Skelehead = 1499;

		public const short FacingtheCerebralMastermind = 1500;

		public const short LakeofFire = 1501;

		public const short TrioSuperHeroes = 1502;

		public const short SpectreHood = 1503;

		public const short SpectreRobe = 1504;

		public const short SpectrePants = 1505;

		public const short SpectrePickaxe = 1506;

		public const short SpectreHamaxe = 1507;

		public const short Ectoplasm = 1508;

		public const short GothicChair = 1509;

		public const short GothicTable = 1510;

		public const short GothicWorkBench = 1511;

		public const short GothicBookcase = 1512;

		public const short PaladinsHammer = 1513;

		public const short SWATHelmet = 1514;

		public const short BeeWings = 1515;

		public const short GiantHarpyFeather = 1516;

		public const short BoneFeather = 1517;

		public const short FireFeather = 1518;

		public const short IceFeather = 1519;

		public const short BrokenBatWing = 1520;

		public const short TatteredBeeWing = 1521;

		public const short LargeAmethyst = 1522;

		public const short LargeTopaz = 1523;

		public const short LargeSapphire = 1524;

		public const short LargeEmerald = 1525;

		public const short LargeRuby = 1526;

		public const short LargeDiamond = 1527;

		public const short JungleChest = 1528;

		public const short CorruptionChest = 1529;

		public const short CrimsonChest = 1530;

		public const short HallowedChest = 1531;

		public const short FrozenChest = 1532;

		public const short JungleKey = 1533;

		public const short CorruptionKey = 1534;

		public const short CrimsonKey = 1535;

		public const short HallowedKey = 1536;

		public const short FrozenKey = 1537;

		public const short ImpFace = 1538;

		public const short OminousPresence = 1539;

		public const short ShiningMoon = 1540;

		public const short LivingGore = 1541;

		public const short FlowingMagma = 1542;

		public const short SpectrePaintbrush = 1543;

		public const short SpectrePaintRoller = 1544;

		public const short SpectrePaintScraper = 1545;

		public const short ShroomiteHeadgear = 1546;

		public const short ShroomiteMask = 1547;

		public const short ShroomiteHelmet = 1548;

		public const short ShroomiteBreastplate = 1549;

		public const short ShroomiteLeggings = 1550;

		public const short Autohammer = 1551;

		public const short ShroomiteBar = 1552;

		public const short SDMG = 1553;

		public const short CenxsTiara = 1554;

		public const short CenxsBreastplate = 1555;

		public const short CenxsLeggings = 1556;

		public const short CrownosMask = 1557;

		public const short CrownosBreastplate = 1558;

		public const short CrownosLeggings = 1559;

		public const short WillsHelmet = 1560;

		public const short WillsBreastplate = 1561;

		public const short WillsLeggings = 1562;

		public const short JimsHelmet = 1563;

		public const short JimsBreastplate = 1564;

		public const short JimsLeggings = 1565;

		public const short AaronsHelmet = 1566;

		public const short AaronsBreastplate = 1567;

		public const short AaronsLeggings = 1568;

		public const short VampireKnives = 1569;

		public const short BrokenHeroSword = 1570;

		public const short ScourgeoftheCorruptor = 1571;

		public const short StaffoftheFrostHydra = 1572;

		public const short TheCreationoftheGuide = 1573;

		public const short TheMerchant = 1574;

		public const short CrownoDevoursHisLunch = 1575;

		public const short RareEnchantment = 1576;

		public const short GloriousNight = 1577;

		public const short SweetheartNecklace = 1578;

		public const short FlurryBoots = 1579;

		public const short DTownsHelmet = 1580;

		public const short DTownsBreastplate = 1581;

		public const short DTownsLeggings = 1582;

		public const short DTownsWings = 1583;

		public const short WillsWings = 1584;

		public const short CrownosWings = 1585;

		public const short CenxsWings = 1586;

		public const short CenxsDress = 1587;

		public const short CenxsDressPants = 1588;

		public const short PalladiumColumn = 1589;

		public const short PalladiumColumnWall = 1590;

		public const short BubblegumBlock = 1591;

		public const short BubblegumBlockWall = 1592;

		public const short TitanstoneBlock = 1593;

		public const short TitanstoneBlockWall = 1594;

		public const short MagicCuffs = 1595;

		public const short MusicBoxSnow = 1596;

		public const short MusicBoxSpace = 1597;

		public const short MusicBoxCrimson = 1598;

		public const short MusicBoxBoss4 = 1599;

		public const short MusicBoxAltOverworldDay = 1600;

		public const short MusicBoxRain = 1601;

		public const short MusicBoxIce = 1602;

		public const short MusicBoxDesert = 1603;

		public const short MusicBoxOcean = 1604;

		public const short MusicBoxDungeon = 1605;

		public const short MusicBoxPlantera = 1606;

		public const short MusicBoxBoss5 = 1607;

		public const short MusicBoxTemple = 1608;

		public const short MusicBoxEclipse = 1609;

		public const short MusicBoxMushrooms = 1610;

		public const short ButterflyDust = 1611;

		public const short AnkhCharm = 1612;

		public const short AnkhShield = 1613;

		public const short BlueFlare = 1614;

		public const short AnglerFishBanner = 1615;

		public const short AngryNimbusBanner = 1616;

		public const short AnomuraFungusBanner = 1617;

		public const short AntlionBanner = 1618;

		public const short ArapaimaBanner = 1619;

		public const short ArmoredSkeletonBanner = 1620;

		public const short BatBanner = 1621;

		public const short BirdBanner = 1622;

		public const short BlackRecluseBanner = 1623;

		public const short BloodFeederBanner = 1624;

		public const short BloodJellyBanner = 1625;

		public const short BloodCrawlerBanner = 1626;

		public const short BoneSerpentBanner = 1627;

		public const short BunnyBanner = 1628;

		public const short ChaosElementalBanner = 1629;

		public const short MimicBanner = 1630;

		public const short ClownBanner = 1631;

		public const short CorruptBunnyBanner = 1632;

		public const short CorruptGoldfishBanner = 1633;

		public const short CrabBanner = 1634;

		public const short CrimeraBanner = 1635;

		public const short CrimsonAxeBanner = 1636;

		public const short CursedHammerBanner = 1637;

		public const short DemonBanner = 1638;

		public const short DemonEyeBanner = 1639;

		public const short DerplingBanner = 1640;

		public const short EaterofSoulsBanner = 1641;

		public const short EnchantedSwordBanner = 1642;

		public const short ZombieEskimoBanner = 1643;

		public const short FaceMonsterBanner = 1644;

		public const short FloatyGrossBanner = 1645;

		public const short FlyingFishBanner = 1646;

		public const short FlyingSnakeBanner = 1647;

		public const short FrankensteinBanner = 1648;

		public const short FungiBulbBanner = 1649;

		public const short FungoFishBanner = 1650;

		public const short GastropodBanner = 1651;

		public const short GoblinThiefBanner = 1652;

		public const short GoblinSorcererBanner = 1653;

		public const short GoblinPeonBanner = 1654;

		public const short GoblinScoutBanner = 1655;

		public const short GoblinWarriorBanner = 1656;

		public const short GoldfishBanner = 1657;

		public const short HarpyBanner = 1658;

		public const short HellbatBanner = 1659;

		public const short HerplingBanner = 1660;

		public const short HornetBanner = 1661;

		public const short IceElementalBanner = 1662;

		public const short IcyMermanBanner = 1663;

		public const short FireImpBanner = 1664;

		public const short JellyfishBanner = 1665;

		public const short JungleCreeperBanner = 1666;

		public const short LihzahrdBanner = 1667;

		public const short ManEaterBanner = 1668;

		public const short MeteorHeadBanner = 1669;

		public const short MothBanner = 1670;

		public const short MummyBanner = 1671;

		public const short MushiLadybugBanner = 1672;

		public const short ParrotBanner = 1673;

		public const short PigronBanner = 1674;

		public const short PiranhaBanner = 1675;

		public const short PirateBanner = 1676;

		public const short PixieBanner = 1677;

		public const short RaincoatZombieBanner = 1678;

		public const short ReaperBanner = 1679;

		public const short SharkBanner = 1680;

		public const short SkeletonBanner = 1681;

		public const short SkeletonMageBanner = 1682;

		public const short SlimeBanner = 1683;

		public const short SnowFlinxBanner = 1684;

		public const short SpiderBanner = 1685;

		public const short SporeZombieBanner = 1686;

		public const short SwampThingBanner = 1687;

		public const short TortoiseBanner = 1688;

		public const short ToxicSludgeBanner = 1689;

		public const short UmbrellaSlimeBanner = 1690;

		public const short UnicornBanner = 1691;

		public const short VampireBanner = 1692;

		public const short VultureBanner = 1693;

		public const short NypmhBanner = 1694;

		public const short WerewolfBanner = 1695;

		public const short WolfBanner = 1696;

		public const short WorldFeederBanner = 1697;

		public const short WormBanner = 1698;

		public const short WraithBanner = 1699;

		public const short WyvernBanner = 1700;

		public const short ZombieBanner = 1701;

		public const short GlassPlatform = 1702;

		public const short GlassChair = 1703;

		public const short GoldenChair = 1704;

		public const short GoldenToilet = 1705;

		public const short BarStool = 1706;

		public const short HoneyChair = 1707;

		public const short SteampunkChair = 1708;

		public const short GlassDoor = 1709;

		public const short GoldenDoor = 1710;

		public const short HoneyDoor = 1711;

		public const short SteampunkDoor = 1712;

		public const short GlassTable = 1713;

		public const short BanquetTable = 1714;

		public const short Bar = 1715;

		public const short GoldenTable = 1716;

		public const short HoneyTable = 1717;

		public const short SteampunkTable = 1718;

		public const short GlassBed = 1719;

		public const short GoldenBed = 1720;

		public const short HoneyBed = 1721;

		public const short SteampunkBed = 1722;

		public const short LivingWoodWall = 1723;

		public const short FartinaJar = 1724;

		public const short Pumpkin = 1725;

		public const short PumpkinWall = 1726;

		public const short Hay = 1727;

		public const short HayWall = 1728;

		public const short SpookyWood = 1729;

		public const short SpookyWoodWall = 1730;

		public const short PumpkinHelmet = 1731;

		public const short PumpkinBreastplate = 1732;

		public const short PumpkinLeggings = 1733;

		public const short CandyApple = 1734;

		public const short SoulCake = 1735;

		public const short NurseHat = 1736;

		public const short NurseShirt = 1737;

		public const short NursePants = 1738;

		public const short WizardsHat = 1739;

		public const short GuyFawkesMask = 1740;

		public const short DyeTraderRobe = 1741;

		public const short SteampunkGoggles = 1742;

		public const short CyborgHelmet = 1743;

		public const short CyborgShirt = 1744;

		public const short CyborgPants = 1745;

		public const short CreeperMask = 1746;

		public const short CreeperShirt = 1747;

		public const short CreeperPants = 1748;

		public const short CatMask = 1749;

		public const short CatShirt = 1750;

		public const short CatPants = 1751;

		public const short GhostMask = 1752;

		public const short GhostShirt = 1753;

		public const short PumpkinMask = 1754;

		public const short PumpkinShirt = 1755;

		public const short PumpkinPants = 1756;

		public const short RobotMask = 1757;

		public const short RobotShirt = 1758;

		public const short RobotPants = 1759;

		public const short UnicornMask = 1760;

		public const short UnicornShirt = 1761;

		public const short UnicornPants = 1762;

		public const short VampireMask = 1763;

		public const short VampireShirt = 1764;

		public const short VampirePants = 1765;

		public const short WitchHat = 1766;

		public const short LeprechaunHat = 1767;

		public const short LeprechaunShirt = 1768;

		public const short LeprechaunPants = 1769;

		public const short PixieShirt = 1770;

		public const short PixiePants = 1771;

		public const short PrincessHat = 1772;

		public const short PrincessDressNew = 1773;

		public const short GoodieBag = 1774;

		public const short WitchDress = 1775;

		public const short WitchBoots = 1776;

		public const short BrideofFrankensteinMask = 1777;

		public const short BrideofFrankensteinDress = 1778;

		public const short KarateTortoiseMask = 1779;

		public const short KarateTortoiseShirt = 1780;

		public const short KarateTortoisePants = 1781;

		public const short CandyCornRifle = 1782;

		public const short CandyCorn = 1783;

		public const short JackOLanternLauncher = 1784;

		public const short ExplosiveJackOLantern = 1785;

		public const short Sickle = 1786;

		public const short PumpkinPie = 1787;

		public const short ScarecrowHat = 1788;

		public const short ScarecrowShirt = 1789;

		public const short ScarecrowPants = 1790;

		public const short Cauldron = 1791;

		public const short PumpkinChair = 1792;

		public const short PumpkinDoor = 1793;

		public const short PumpkinTable = 1794;

		public const short PumpkinWorkBench = 1795;

		public const short PumpkinPlatform = 1796;

		public const short TatteredFairyWings = 1797;

		public const short SpiderEgg = 1798;

		public const short MagicalPumpkinSeed = 1799;

		public const short BatHook = 1800;

		public const short BatScepter = 1801;

		public const short RavenStaff = 1802;

		public const short JungleKeyMold = 1803;

		public const short CorruptionKeyMold = 1804;

		public const short CrimsonKeyMold = 1805;

		public const short HallowedKeyMold = 1806;

		public const short FrozenKeyMold = 1807;

		public const short HangingJackOLantern = 1808;

		public const short RottenEgg = 1809;

		public const short UnluckyYarn = 1810;

		public const short BlackFairyDust = 1811;

		public const short Jackelier = 1812;

		public const short JackOLantern = 1813;

		public const short SpookyChair = 1814;

		public const short SpookyDoor = 1815;

		public const short SpookyTable = 1816;

		public const short SpookyWorkBench = 1817;

		public const short SpookyPlatform = 1818;

		public const short ReaperHood = 1819;

		public const short ReaperRobe = 1820;

		public const short FoxMask = 1821;

		public const short FoxShirt = 1822;

		public const short FoxPants = 1823;

		public const short CatEars = 1824;

		public const short BloodyMachete = 1825;

		public const short TheHorsemansBlade = 1826;

		public const short BladedGlove = 1827;

		public const short PumpkinSeed = 1828;

		public const short SpookyHook = 1829;

		public const short SpookyWings = 1830;

		public const short SpookyTwig = 1831;

		public const short SpookyHelmet = 1832;

		public const short SpookyBreastplate = 1833;

		public const short SpookyLeggings = 1834;

		public const short StakeLauncher = 1835;

		public const short Stake = 1836;

		public const short CursedSapling = 1837;

		public const short SpaceCreatureMask = 1838;

		public const short SpaceCreatureShirt = 1839;

		public const short SpaceCreaturePants = 1840;

		public const short WolfMask = 1841;

		public const short WolfShirt = 1842;

		public const short WolfPants = 1843;

		public const short PumpkinMoonMedallion = 1844;

		public const short NecromanticScroll = 1845;

		public const short JackingSkeletron = 1846;

		public const short BitterHarvest = 1847;

		public const short BloodMoonCountess = 1848;

		public const short HallowsEve = 1849;

		public const short MorbidCuriosity = 1850;

		public const short TreasureHunterShirt = 1851;

		public const short TreasureHunterPants = 1852;

		public const short DryadCoverings = 1853;

		public const short DryadLoincloth = 1854;

		public const short MourningWoodTrophy = 1855;

		public const short PumpkingTrophy = 1856;

		public const short JackOLanternMask = 1857;

		public const short SniperScope = 1858;

		public const short HeartLantern = 1859;

		public const short JellyfishDivingGear = 1860;

		public const short ArcticDivingGear = 1861;

		public const short FrostsparkBoots = 1862;

		public const short FartInABalloon = 1863;

		public const short PapyrusScarab = 1864;

		public const short CelestialStone = 1865;

		public const short Hoverboard = 1866;

		public const short CandyCane = 1867;

		public const short SugarPlum = 1868;

		public const short Present = 1869;

		public const short RedRyder = 1870;

		public const short FestiveWings = 1871;

		public const short PineTreeBlock = 1872;

		public const short ChristmasTree = 1873;

		public const short StarTopper1 = 1874;

		public const short StarTopper2 = 1875;

		public const short StarTopper3 = 1876;

		public const short BowTopper = 1877;

		public const short WhiteGarland = 1878;

		public const short WhiteAndRedGarland = 1879;

		public const short RedGardland = 1880;

		public const short RedAndGreenGardland = 1881;

		public const short GreenGardland = 1882;

		public const short GreenAndWhiteGarland = 1883;

		public const short MulticoloredBulb = 1884;

		public const short RedBulb = 1885;

		public const short YellowBulb = 1886;

		public const short GreenBulb = 1887;

		public const short RedAndGreenBulb = 1888;

		public const short YellowAndGreenBulb = 1889;

		public const short RedAndYellowBulb = 1890;

		public const short WhiteBulb = 1891;

		public const short WhiteAndRedBulb = 1892;

		public const short WhiteAndYellowBulb = 1893;

		public const short WhiteAndGreenBulb = 1894;

		public const short MulticoloredLights = 1895;

		public const short RedLights = 1896;

		public const short GreenLights = 1897;

		public const short BlueLights = 1898;

		public const short YellowLights = 1899;

		public const short RedAndYellowLights = 1900;

		public const short RedAndGreenLights = 1901;

		public const short YellowAndGreenLights = 1902;

		public const short BlueAndGreenLights = 1903;

		public const short RedAndBlueLights = 1904;

		public const short BlueAndYellowLights = 1905;

		public const short GiantBow = 1906;

		public const short ReindeerAntlers = 1907;

		public const short Holly = 1908;

		public const short CandyCaneSword = 1909;

		public const short ElfMelter = 1910;

		public const short ChristmasPudding = 1911;

		public const short Eggnog = 1912;

		public const short StarAnise = 1913;

		public const short ReindeerBells = 1914;

		public const short CandyCaneHook = 1915;

		public const short ChristmasHook = 1916;

		public const short CnadyCanePickaxe = 1917;

		public const short FruitcakeChakram = 1918;

		public const short SugarCookie = 1919;

		public const short GingerbreadCookie = 1920;

		public const short HandWarmer = 1921;

		public const short Coal = 1922;

		public const short Toolbox = 1923;

		public const short PineDoor = 1924;

		public const short PineChair = 1925;

		public const short PineTable = 1926;

		public const short DogWhistle = 1927;

		public const short ChristmasTreeSword = 1928;

		public const short ChainGun = 1929;

		public const short Razorpine = 1930;

		public const short BlizzardStaff = 1931;

		public const short MrsClauseHat = 1932;

		public const short MrsClauseShirt = 1933;

		public const short MrsClauseHeels = 1934;

		public const short ParkaHood = 1935;

		public const short ParkaCoat = 1936;

		public const short ParkaPants = 1937;

		public const short SnowHat = 1938;

		public const short UglySweater = 1939;

		public const short TreeMask = 1940;

		public const short TreeShirt = 1941;

		public const short TreeTrunks = 1942;

		public const short ElfHat = 1943;

		public const short ElfShirt = 1944;

		public const short ElfPants = 1945;

		public const short SnowmanCannon = 1946;

		public const short NorthPole = 1947;

		public const short ChristmasTreeWallpaper = 1948;

		public const short OrnamentWallpaper = 1949;

		public const short CandyCaneWallpaper = 1950;

		public const short FestiveWallpaper = 1951;

		public const short StarsWallpaper = 1952;

		public const short SquigglesWallpaper = 1953;

		public const short SnowflakeWallpaper = 1954;

		public const short KrampusHornWallpaper = 1955;

		public const short BluegreenWallpaper = 1956;

		public const short GrinchFingerWallpaper = 1957;

		public const short NaughtyPresent = 1958;

		public const short BabyGrinchMischiefWhistle = 1959;

		public const short IceQueenTrophy = 1960;

		public const short SantaNK1Trophy = 1961;

		public const short EverscreamTrophy = 1962;

		public const short MusicBoxPumpkinMoon = 1963;

		public const short MusicBoxAltUnderground = 1964;

		public const short MusicBoxFrostMoon = 1965;

		public const short BrownPaint = 1966;

		public const short ShadowPaint = 1967;

		public const short NegativePaint = 1968;

		public const short TeamDye = 1969;

		public const short AmethystGemsparkBlock = 1970;

		public const short TopazGemsparkBlock = 1971;

		public const short SapphireGemsparkBlock = 1972;

		public const short EmeraldGemsparkBlock = 1973;

		public const short RubyGemsparkBlock = 1974;

		public const short DiamondGemsparkBlock = 1975;

		public const short AmberGemsparkBlock = 1976;

		public const short LifeHairDye = 1977;

		public const short ManaHairDye = 1978;

		public const short DepthHairDye = 1979;

		public const short MoneyHairDye = 1980;

		public const short TimeHairDye = 1981;

		public const short TeamHairDye = 1982;

		public const short BiomeHairDye = 1983;

		public const short PartyHairDye = 1984;

		public const short RainbowHairDye = 1985;

		public const short SpeedHairDye = 1986;

		public const short AngelHalo = 1987;

		public const short Fez = 1988;

		public const short Womannquin = 1989;

		public const short HairDyeRemover = 1990;

		public const short BugNet = 1991;

		public const short Firefly = 1992;

		public const short FireflyinaBottle = 1993;

		public const short MonarchButterfly = 1994;

		public const short PurpleEmperorButterfly = 1995;

		public const short RedAdmiralButterfly = 1996;

		public const short UlyssesButterfly = 1997;

		public const short SulphurButterfly = 1998;

		public const short TreeNymphButterfly = 1999;

		public const short ZebraSwallowtailButterfly = 2000;

		public const short JuliaButterfly = 2001;

		public const short Worm = 2002;

		public const short Mouse = 2003;

		public const short LightningBug = 2004;

		public const short LightningBuginaBottle = 2005;

		public const short Snail = 2006;

		public const short GlowingSnail = 2007;

		public const short FancyGreyWallpaper = 2008;

		public const short IceFloeWallpaper = 2009;

		public const short MusicWallpaper = 2010;

		public const short PurpleRainWallpaper = 2011;

		public const short RainbowWallpaper = 2012;

		public const short SparkleStoneWallpaper = 2013;

		public const short StarlitHeavenWallpaper = 2014;

		public const short Bird = 2015;

		public const short BlueJay = 2016;

		public const short Cardinal = 2017;

		public const short Squirrel = 2018;

		public const short Bunny = 2019;

		public const short CactusBookcase = 2020;

		public const short EbonwoodBookcase = 2021;

		public const short FleshBookcase = 2022;

		public const short HoneyBookcase = 2023;

		public const short SteampunkBookcase = 2024;

		public const short GlassBookcase = 2025;

		public const short RichMahoganyBookcase = 2026;

		public const short PearlwoodBookcase = 2027;

		public const short SpookyBookcase = 2028;

		public const short SkywareBookcase = 2029;

		public const short LihzahrdBookcase = 2030;

		public const short FrozenBookcase = 2031;

		public const short CactusLantern = 2032;

		public const short EbonwoodLantern = 2033;

		public const short FleshLantern = 2034;

		public const short HoneyLantern = 2035;

		public const short SteampunkLantern = 2036;

		public const short GlassLantern = 2037;

		public const short RichMahoganyLantern = 2038;

		public const short PearlwoodLantern = 2039;

		public const short FrozenLantern = 2040;

		public const short LihzahrdLantern = 2041;

		public const short SkywareLantern = 2042;

		public const short SpookyLantern = 2043;

		public const short FrozenDoor = 2044;

		public const short CactusCandle = 2045;

		public const short EbonwoodCandle = 2046;

		public const short FleshCandle = 2047;

		public const short GlassCandle = 2048;

		public const short FrozenCandle = 2049;

		public const short RichMahoganyCandle = 2050;

		public const short PearlwoodCandle = 2051;

		public const short LihzahrdCandle = 2052;

		public const short SkywareCandle = 2053;

		public const short PumpkinCandle = 2054;

		public const short CactusChandelier = 2055;

		public const short EbonwoodChandelier = 2056;

		public const short FleshChandelier = 2057;

		public const short HoneyChandelier = 2058;

		public const short FrozenChandelier = 2059;

		public const short RichMahoganyChandelier = 2060;

		public const short PearlwoodChandelier = 2061;

		public const short LihzahrdChandelier = 2062;

		public const short SkywareChandelier = 2063;

		public const short SpookyChandelier = 2064;

		public const short GlassChandelier = 2065;

		public const short CactusBed = 2066;

		public const short FleshBed = 2067;

		public const short FrozenBed = 2068;

		public const short LihzahrdBed = 2069;

		public const short SkywareBed = 2070;

		public const short SpookyBed = 2071;

		public const short CactusBathtub = 2072;

		public const short EbonwoodBathtub = 2073;

		public const short FleshBathtub = 2074;

		public const short GlassBathtub = 2075;

		public const short FrozenBathtub = 2076;

		public const short RichMahoganyBathtub = 2077;

		public const short PearlwoodBathtub = 2078;

		public const short LihzahrdBathtub = 2079;

		public const short SkywareBathtub = 2080;

		public const short SpookyBathtub = 2081;

		public const short CactusLamp = 2082;

		public const short EbonwoodLamp = 2083;

		public const short FleshLamp = 2084;

		public const short GlassLamp = 2085;

		public const short FrozenLamp = 2086;

		public const short RichMahoganyLamp = 2087;

		public const short PearlwoodLamp = 2088;

		public const short LihzahrdLamp = 2089;

		public const short SkywareLamp = 2090;

		public const short SpookyLamp = 2091;

		public const short CactusCandelabra = 2092;

		public const short EbonwoodCandelabra = 2093;

		public const short FleshCandelabra = 2094;

		public const short HoneyCandelabra = 2095;

		public const short SteampunkCandelabra = 2096;

		public const short GlassCandelabra = 2097;

		public const short RichMahoganyCandelabra = 2098;

		public const short PearlwoodCandelabra = 2099;

		public const short FrozenCandelabra = 2100;

		public const short LihzahrdCandelabra = 2101;

		public const short SkywareCandelabra = 2102;

		public const short SpookyCandelabra = 2103;

		public const short BrainMask = 2104;

		public const short FleshMask = 2105;

		public const short TwinMask = 2106;

		public const short SkeletronPrimeMask = 2107;

		public const short BeeMask = 2108;

		public const short PlanteraMask = 2109;

		public const short GolemMask = 2110;

		public const short EaterMask = 2111;

		public const short EyeMask = 2112;

		public const short DestroyerMask = 2113;

		public const short BlacksmithRack = 2114;

		public const short CarpentryRack = 2115;

		public const short HelmetRack = 2116;

		public const short SpearRack = 2117;

		public const short SwordRack = 2118;

		public const short StoneSlab = 2119;

		public const short SandstoneSlab = 2120;

		public const short Frog = 2121;

		public const short MallardDuck = 2122;

		public const short Duck = 2123;

		public const short HoneyBathtub = 2124;

		public const short SteampunkBathtub = 2125;

		public const short LivingWoodBathtub = 2126;

		public const short ShadewoodBathtub = 2127;

		public const short BoneBathtub = 2128;

		public const short HoneyLamp = 2129;

		public const short SteampunkLamp = 2130;

		public const short LivingWoodLamp = 2131;

		public const short ShadewoodLamp = 2132;

		public const short GoldenLamp = 2133;

		public const short BoneLamp = 2134;

		public const short LivingWoodBookcase = 2135;

		public const short ShadewoodBookcase = 2136;

		public const short GoldenBookcase = 2137;

		public const short BoneBookcase = 2138;

		public const short LivingWoodBed = 2139;

		public const short BoneBed = 2140;

		public const short LivingWoodChandelier = 2141;

		public const short ShadewoodChandelier = 2142;

		public const short GoldenChandelier = 2143;

		public const short BoneChandelier = 2144;

		public const short LivingWoodLantern = 2145;

		public const short ShadewoodLantern = 2146;

		public const short GoldenLantern = 2147;

		public const short BoneLantern = 2148;

		public const short LivingWoodCandelabra = 2149;

		public const short ShadewoodCandelabra = 2150;

		public const short GoldenCandelabra = 2151;

		public const short BoneCandelabra = 2152;

		public const short LivingWoodCandle = 2153;

		public const short ShadewoodCandle = 2154;

		public const short GoldenCandle = 2155;

		public const short BlackScorpion = 2156;

		public const short Scorpion = 2157;

		public const short BubbleWallpaper = 2158;

		public const short CopperPipeWallpaper = 2159;

		public const short DuckyWallpaper = 2160;

		public const short FrostCore = 2161;

		public const short BunnyCage = 2162;

		public const short SquirrelCage = 2163;

		public const short MallardDuckCage = 2164;

		public const short DuckCage = 2165;

		public const short BirdCage = 2166;

		public const short BlueJayCage = 2167;

		public const short CardinalCage = 2168;

		public const short WaterfallWall = 2169;

		public const short LavafallWall = 2170;

		public const short CrimsonSeeds = 2171;

		public const short HeavyWorkBench = 2172;

		public const short CopperPlating = 2173;

		public const short SnailCage = 2174;

		public const short GlowingSnailCage = 2175;

		public const short ShroomiteDiggingClaw = 2176;

		public const short AmmoBox = 2177;

		public const short MonarchButterflyJar = 2178;

		public const short PurpleEmperorButterflyJar = 2179;

		public const short RedAdmiralButterflyJar = 2180;

		public const short UlyssesButterflyJar = 2181;

		public const short SulphurButterflyJar = 2182;

		public const short TreeNymphButterflyJar = 2183;

		public const short ZebraSwallowtailButterflyJar = 2184;

		public const short JuliaButterflyJar = 2185;

		public const short ScorpionCage = 2186;

		public const short BlackScorpionCage = 2187;

		public const short VenomStaff = 2188;

		public const short SpectreMask = 2189;

		public const short FrogCage = 2190;

		public const short MouseCage = 2191;

		public const short BoneWelder = 2192;

		public const short FleshCloningVaat = 2193;

		public const short GlassKiln = 2194;

		public const short LihzahrdFurnace = 2195;

		public const short LivingLoom = 2196;

		public const short SkyMill = 2197;

		public const short IceMachine = 2198;

		public const short BeetleHelmet = 2199;

		public const short BeetleScaleMail = 2200;

		public const short BeetleShell = 2201;

		public const short BeetleLeggings = 2202;

		public const short SteampunkBoiler = 2203;

		public const short HoneyDispenser = 2204;

		public const short Penguin = 2205;

		public const short PenguinCage = 2206;

		public const short WormCage = 2207;

		public const short Terrarium = 2208;

		public const short SuperManaPotion = 2209;

		public const short EbonwoodFence = 2210;

		public const short RichMahoganyFence = 2211;

		public const short PearlwoodFence = 2212;

		public const short ShadewoodFence = 2213;

		public const short BrickLayer = 2214;

		public const short ExtendoGrip = 2215;

		public const short PaintSprayer = 2216;

		public const short PortableCementMixer = 2217;

		public const short BeetleHusk = 2218;

		public const short CelestialMagnet = 2219;

		public const short CelestialEmblem = 2220;

		public const short CelestialCuffs = 2221;

		public const short PeddlersHat = 2222;

		public const short PulseBow = 2223;

		public const short DynastyChandelier = 2224;

		public const short DynastyLamp = 2225;

		public const short DynastyLantern = 2226;

		public const short DynastyCandelabra = 2227;

		public const short DynastyChair = 2228;

		public const short DynastyWorkBench = 2229;

		public const short DynastyChest = 2230;

		public const short DynastyBed = 2231;

		public const short DynastyBathtub = 2232;

		public const short DynastyBookcase = 2233;

		public const short DynastyCup = 2234;

		public const short DynastyBowl = 2235;

		public const short DynastyCandle = 2236;

		public const short DynastyClock = 2237;

		public const short GoldenClock = 2238;

		public const short GlassClock = 2239;

		public const short HoneyClock = 2240;

		public const short SteampunkClock = 2241;

		public const short FancyDishes = 2242;

		public const short GlassBowl = 2243;

		public const short WineGlass = 2244;

		public const short LivingWoodPiano = 2245;

		public const short FleshPiano = 2246;

		public const short FrozenPiano = 2247;

		public const short FrozenTable = 2248;

		public const short HoneyChest = 2249;

		public const short SteampunkChest = 2250;

		public const short HoneyWorkBench = 2251;

		public const short FrozenWorkBench = 2252;

		public const short SteampunkWorkBench = 2253;

		public const short GlassPiano = 2254;

		public const short HoneyPiano = 2255;

		public const short SteampunkPiano = 2256;

		public const short HoneyCup = 2257;

		public const short SteampunkCup = 2258;

		public const short DynastyTable = 2259;

		public const short DynastyWood = 2260;

		public const short RedDynastyShingles = 2261;

		public const short BlueDynastyShingles = 2262;

		public const short WhiteDynastyWall = 2263;

		public const short BlueDynastyWall = 2264;

		public const short DynastyDoor = 2265;

		public const short Sake = 2266;

		public const short PadThai = 2267;

		public const short Pho = 2268;

		public const short Revolver = 2269;

		public const short Gatligator = 2270;

		public const short ArcaneRuneWall = 2271;

		public const short WaterGun = 2272;

		public const short Katana = 2273;

		public const short UltrabrightTorch = 2274;

		public const short MagicHat = 2275;

		public const short DiamondRing = 2276;

		public const short Gi = 2277;

		public const short Kimono = 2278;

		public const short GypsyRobe = 2279;

		public const short BeetleWings = 2280;

		public const short TigerSkin = 2281;

		public const short LeopardSkin = 2282;

		public const short ZebraSkin = 2283;

		public const short CrimsonCloak = 2284;

		public const short MysteriousCape = 2285;

		public const short RedCape = 2286;

		public const short WinterCape = 2287;

		public const short FrozenChair = 2288;

		public const short WoodFishingPole = 2289;

		public const short Bass = 2290;

		public const short ReinforcedFishingPole = 2291;

		public const short FiberglassFishingPole = 2292;

		public const short FisherofSouls = 2293;

		public const short GoldenFishingRod = 2294;

		public const short MechanicsRod = 2295;

		public const short SittingDucksFishingRod = 2296;

		public const short Trout = 2297;

		public const short Salmon = 2298;

		public const short AtlanticCod = 2299;

		public const short Tuna = 2300;

		public const short RedSnapper = 2301;

		public const short NeonTetra = 2302;

		public const short ArmoredCavefish = 2303;

		public const short Damselfish = 2304;

		public const short CrimsonTigerfish = 2305;

		public const short FrostMinnow = 2306;

		public const short PrincessFish = 2307;

		public const short GoldenCarp = 2308;

		public const short SpecularFish = 2309;

		public const short Prismite = 2310;

		public const short VariegatedLardfish = 2311;

		public const short FlarefinKoi = 2312;

		public const short DoubleCod = 2313;

		public const short Honeyfin = 2314;

		public const short Obsidifish = 2315;

		public const short Shrimp = 2316;

		public const short ChaosFish = 2317;

		public const short Ebonkoi = 2318;

		public const short Hemopiranha = 2319;

		public const short Rockfish = 2320;

		public const short Stinkfish = 2321;

		public const short MiningPotion = 2322;

		public const short HeartreachPotion = 2323;

		public const short CalmingPotion = 2324;

		public const short BuilderPotion = 2325;

		public const short TitanPotion = 2326;

		public const short FlipperPotion = 2327;

		public const short SummoningPotion = 2328;

		public const short TrapsightPotion = 2329;

		public const short PurpleClubberfish = 2330;

		public const short ObsidianSwordfish = 2331;

		public const short Swordfish = 2332;

		public const short IronFence = 2333;

		public const short WoodenCrate = 2334;

		public const short IronCrate = 2335;

		public const short GoldenCrate = 2336;

		public const short OldShoe = 2337;

		public const short FishingSeaweed = 2338;

		public const short TinCan = 2339;

		public const short MinecartTrack = 2340;

		public const short ReaverShark = 2341;

		public const short SawtoothShark = 2342;

		public const short Minecart = 2343;

		public const short AmmoReservationPotion = 2344;

		public const short LifeforcePotion = 2345;

		public const short EndurancePotion = 2346;

		public const short RagePotion = 2347;

		public const short InfernoPotion = 2348;

		public const short WrathPotion = 2349;

		public const short RecallPotion = 2350;

		public const short TeleportationPotion = 2351;

		public const short LovePotion = 2352;

		public const short StinkPotion = 2353;

		public const short FishingPotion = 2354;

		public const short SonarPotion = 2355;

		public const short CratePotion = 2356;

		public const short ShiverthornSeeds = 2357;

		public const short Shiverthorn = 2358;

		public const short WarmthPotion = 2359;

		public const short FishHook = 2360;

		public const short BeeHeadgear = 2361;

		public const short BeeBreastplate = 2362;

		public const short BeeGreaves = 2363;

		public const short HornetStaff = 2364;

		public const short ImpStaff = 2365;

		public const short QueenSpiderStaff = 2366;

		public const short AnglerHat = 2367;

		public const short AnglerVest = 2368;

		public const short AnglerPants = 2369;

		public const short SpiderMask = 2370;

		public const short SpiderBreastplate = 2371;

		public const short SpiderGreaves = 2372;

		public const short HighTestFishingLine = 2373;

		public const short AnglerEarring = 2374;

		public const short TackleBox = 2375;

		public const short BlueDungeonPiano = 2376;

		public const short GreenDungeonPiano = 2377;

		public const short PinkDungeonPiano = 2378;

		public const short GoldenPiano = 2379;

		public const short ObsidianPiano = 2380;

		public const short BonePiano = 2381;

		public const short CactusPiano = 2382;

		public const short SpookyPiano = 2383;

		public const short SkywarePiano = 2384;

		public const short LihzahrdPiano = 2385;

		public const short BlueDungeonDresser = 2386;

		public const short GreenDungeonDresser = 2387;

		public const short PinkDungeonDresser = 2388;

		public const short GoldenDresser = 2389;

		public const short ObsidianDresser = 2390;

		public const short BoneDresser = 2391;

		public const short CactusDresser = 2392;

		public const short SpookyDresser = 2393;

		public const short SkywareDresser = 2394;

		public const short HoneyDresser = 2395;

		public const short LihzahrdDresser = 2396;

		public const short Sofa = 2397;

		public const short EbonwoodSofa = 2398;

		public const short RichMahoganySofa = 2399;

		public const short PearlwoodSofa = 2400;

		public const short ShadewoodSofa = 2401;

		public const short BlueDungeonSofa = 2402;

		public const short GreenDungeonSofa = 2403;

		public const short PinkDungeonSofa = 2404;

		public const short GoldenSofa = 2405;

		public const short ObsidianSofa = 2406;

		public const short BoneSofa = 2407;

		public const short CactusSofa = 2408;

		public const short SpookySofa = 2409;

		public const short SkywareSofa = 2410;

		public const short HoneySofa = 2411;

		public const short SteampunkSofa = 2412;

		public const short MushroomSofa = 2413;

		public const short GlassSofa = 2414;

		public const short PumpkinSofa = 2415;

		public const short LihzahrdSofa = 2416;

		public const short SeashellHairpin = 2417;

		public const short MermaidAdornment = 2418;

		public const short MermaidTail = 2419;

		public const short ZephyrFish = 2420;

		public const short Fleshcatcher = 2421;

		public const short HotlineFishingHook = 2422;

		public const short FrogLeg = 2423;

		public const short Anchor = 2424;

		public const short CookedFish = 2425;

		public const short CookedShrimp = 2426;

		public const short Sashimi = 2427;

		public const short FuzzyCarrot = 2428;

		public const short ScalyTruffle = 2429;

		public const short SlimySaddle = 2430;

		public const short BeeWax = 2431;

		public const short CopperPlatingWall = 2432;

		public const short StoneSlabWall = 2433;

		public const short Sail = 2434;

		public const short CoralstoneBlock = 2435;

		public const short BlueJellyfish = 2436;

		public const short GreenJellyfish = 2437;

		public const short PinkJellyfish = 2438;

		public const short BlueJellyfishJar = 2439;

		public const short GreenJellyfishJar = 2440;

		public const short PinkJellyfishJar = 2441;

		public const short LifePreserver = 2442;

		public const short ShipsWheel = 2443;

		public const short CompassRose = 2444;

		public const short WallAnchor = 2445;

		public const short GoldfishTrophy = 2446;

		public const short BunnyfishTrophy = 2447;

		public const short SwordfishTrophy = 2448;

		public const short SharkteethTrophy = 2449;

		public const short Batfish = 2450;

		public const short BumblebeeTuna = 2451;

		public const short Catfish = 2452;

		public const short Cloudfish = 2453;

		public const short Cursedfish = 2454;

		public const short Dirtfish = 2455;

		public const short DynamiteFish = 2456;

		public const short EaterofPlankton = 2457;

		public const short FallenStarfish = 2458;

		public const short TheFishofCthulu = 2459;

		public const short Fishotron = 2460;

		public const short Harpyfish = 2461;

		public const short Hungerfish = 2462;

		public const short Ichorfish = 2463;

		public const short Jewelfish = 2464;

		public const short MirageFish = 2465;

		public const short MutantFlinxfin = 2466;

		public const short Pengfish = 2467;

		public const short Pixiefish = 2468;

		public const short Spiderfish = 2469;

		public const short TundraTrout = 2470;

		public const short UnicornFish = 2471;

		public const short GuideVoodooFish = 2472;

		public const short Wyverntail = 2473;

		public const short ZombieFish = 2474;

		public const short AmanitaFungifin = 2475;

		public const short Angelfish = 2476;

		public const short BloodyManowar = 2477;

		public const short Bonefish = 2478;

		public const short Bunnyfish = 2479;

		public const short CapnTunabeard = 2480;

		public const short Clownfish = 2481;

		public const short DemonicHellfish = 2482;

		public const short Derpfish = 2483;

		public const short Fishron = 2484;

		public const short InfectedScabbardfish = 2485;

		public const short Mudfish = 2486;

		public const short Slimefish = 2487;

		public const short TropicalBarracuda = 2488;

		public const short KingSlimeTrophy = 2489;

		public const short ShipInABottle = 2490;

		public const short HardySaddle = 2491;

		public const short PressureTrack = 2492;

		public const short KingSlimeMask = 2493;

		public const short FinWings = 2494;

		public const short TreasureMap = 2495;

		public const short SeaweedPlanter = 2496;

		public const short PillaginMePixels = 2497;

		public const short FishCostumeMask = 2498;

		public const short FishCostumeShirt = 2499;

		public const short FishCostumeFinskirt = 2500;

		public const short GingerBeard = 2501;

		public const short HoneyedGoggles = 2502;

		public const short BorealWood = 2503;

		public const short PalmWood = 2504;

		public const short BorealWoodWall = 2505;

		public const short PalmWoodWall = 2506;

		public const short BorealWoodFence = 2507;

		public const short PalmWoodFence = 2508;

		public const short BorealWoodHelmet = 2509;

		public const short BorealWoodBreastplate = 2510;

		public const short BorealWoodGreaves = 2511;

		public const short PalmWoodHelmet = 2512;

		public const short PalmWoodBreastplate = 2513;

		public const short PalmWoodGreaves = 2514;

		public const short PalmWoodBow = 2515;

		public const short PalmWoodHammer = 2516;

		public const short PalmWoodSword = 2517;

		public const short PalmWoodPlatform = 2518;

		public const short PalmWoodBathtub = 2519;

		public const short PalmWoodBed = 2520;

		public const short PalmWoodBench = 2521;

		public const short PalmWoodCandelabra = 2522;

		public const short PalmWoodCandle = 2523;

		public const short PalmWoodChair = 2524;

		public const short PalmWoodChandelier = 2525;

		public const short PalmWoodChest = 2526;

		public const short PalmWoodSofa = 2527;

		public const short PalmWoodDoor = 2528;

		public const short PalmWoodDresser = 2529;

		public const short PalmWoodLantern = 2530;

		public const short PalmWoodPiano = 2531;

		public const short PalmWoodTable = 2532;

		public const short PalmWoodLamp = 2533;

		public const short PalmWoodWorkBench = 2534;

		public const short OpticStaff = 2535;

		public const short PalmWoodBookcase = 2536;

		public const short MushroomBathtub = 2537;

		public const short MushroomBed = 2538;

		public const short MushroomBench = 2539;

		public const short MushroomBookcase = 2540;

		public const short MushroomCandelabra = 2541;

		public const short MushroomCandle = 2542;

		public const short MushroomChandelier = 2543;

		public const short MushroomChest = 2544;

		public const short MushroomDresser = 2545;

		public const short MushroomLantern = 2546;

		public const short MushroomLamp = 2547;

		public const short MushroomPiano = 2548;

		public const short MushroomPlatform = 2549;

		public const short MushroomTable = 2550;

		public const short SpiderStaff = 2551;

		public const short BorealWoodBathtub = 2552;

		public const short BorealWoodBed = 2553;

		public const short BorealWoodBookcase = 2554;

		public const short BorealWoodCandelabra = 2555;

		public const short BorealWoodCandle = 2556;

		public const short BorealWoodChair = 2557;

		public const short BorealWoodChandelier = 2558;

		public const short BorealWoodChest = 2559;

		public const short BorealWoodClock = 2560;

		public const short BorealWoodDoor = 2561;

		public const short BorealWoodDresser = 2562;

		public const short BorealWoodLamp = 2563;

		public const short BorealWoodLantern = 2564;

		public const short BorealWoodPiano = 2565;

		public const short BorealWoodPlatform = 2566;

		public const short SlimeBathtub = 2567;

		public const short SlimeBed = 2568;

		public const short SlimeBookcase = 2569;

		public const short SlimeCandelabra = 2570;

		public const short SlimeCandle = 2571;

		public const short SlimeChair = 2572;

		public const short SlimeChandelier = 2573;

		public const short SlimeChest = 2574;

		public const short SlimeClock = 2575;

		public const short SlimeDoor = 2576;

		public const short SlimeDresser = 2577;

		public const short SlimeLamp = 2578;

		public const short SlimeLantern = 2579;

		public const short SlimePiano = 2580;

		public const short SlimePlatform = 2581;

		public const short SlimeSofa = 2582;

		public const short SlimeTable = 2583;

		public const short PirateStaff = 2584;

		public const short SlimeHook = 2585;

		public const short StickyGrenade = 2586;

		public const short TartarSauce = 2587;

		public const short DukeFishronMask = 2588;

		public const short DukeFishronTrophy = 2589;

		public const short MolotovCocktail = 2590;

		public const short BoneClock = 2591;

		public const short CactusClock = 2592;

		public const short EbonwoodClock = 2593;

		public const short FrozenClock = 2594;

		public const short LihzahrdClock = 2595;

		public const short LivingWoodClock = 2596;

		public const short RichMahoganyClock = 2597;

		public const short FleshClock = 2598;

		public const short MushroomClock = 2599;

		public const short ObsidianClock = 2600;

		public const short PalmWoodClock = 2601;

		public const short PearlwoodClock = 2602;

		public const short PumpkinClock = 2603;

		public const short ShadewoodClock = 2604;

		public const short SpookyClock = 2605;

		public const short SkywareClock = 2606;

		public const short SpiderFang = 2607;

		public const short FalconBlade = 2608;

		public const short FishronWings = 2609;

		public const short SlimeGun = 2610;

		public const short Flairon = 2611;

		public const short GreenDungeonChest = 2612;

		public const short PinkDungeonChest = 2613;

		public const short BlueDungeonChest = 2614;

		public const short BoneChest = 2615;

		public const short CactusChest = 2616;

		public const short FleshChest = 2617;

		public const short ObsidianChest = 2618;

		public const short PumpkinChest = 2619;

		public const short SpookyChest = 2620;

		public const short TempestStaff = 2621;

		public const short RazorbladeTyphoon = 2622;

		public const short BubbleGun = 2623;

		public const short Tsunami = 2624;

		public const short Seashell = 2625;

		public const short Starfish = 2626;

		public const short SteampunkPlatform = 2627;

		public const short SkywarePlatform = 2628;

		public const short LivingWoodPlatform = 2629;

		public const short HoneyPlatform = 2630;

		public const short SkywareWorkbench = 2631;

		public const short GlassWorkBench = 2632;

		public const short LivingWoodWorkBench = 2633;

		public const short FleshSofa = 2634;

		public const short FrozenSofa = 2635;

		public const short LivingWoodSofa = 2636;

		public const short PumpkinDresser = 2637;

		public const short SteampunkDresser = 2638;

		public const short GlassDresser = 2639;

		public const short FleshDresser = 2640;

		public const short PumpkinLantern = 2641;

		public const short ObsidianLantern = 2642;

		public const short PumpkinLamp = 2643;

		public const short ObsidianLamp = 2644;

		public const short BlueDungeonLamp = 2645;

		public const short GreenDungeonLamp = 2646;

		public const short PinkDungeonLamp = 2647;

		public const short HoneyCandle = 2648;

		public const short SteampunkCandle = 2649;

		public const short SpookyCandle = 2650;

		public const short ObsidianCandle = 2651;

		public const short BlueDungeonChandelier = 2652;

		public const short GreenDungeonChandelier = 2653;

		public const short PinkDungeonChandelier = 2654;

		public const short SteampunkChandelier = 2655;

		public const short PumpkinChandelier = 2656;

		public const short ObsidianChandelier = 2657;

		public const short BlueDungeonBathtub = 2658;

		public const short GreenDungeonBathtub = 2659;

		public const short PinkDungeonBathtub = 2660;

		public const short PumpkinBathtub = 2661;

		public const short ObsidianBathtub = 2662;

		public const short GoldenBathtub = 2663;

		public const short BlueDungeonCandelabra = 2664;

		public const short GreenDungeonCandelabra = 2665;

		public const short PinkDungeonCandelabra = 2666;

		public const short ObsidianCandelabra = 2667;

		public const short PumpkinCandelabra = 2668;

		public const short PumpkinBed = 2669;

		public const short PumpkinBookcase = 2670;

		public const short PumpkinPiano = 2671;

		public const short SharkStatue = 2672;

		public const short TruffleWorm = 2673;

		public const short ApprenticeBait = 2674;

		public const short JourneymanBait = 2675;

		public const short MasterBait = 2676;

		public const short AmberGemsparkWall = 2677;

		public const short AmberGemsparkWallOff = 2678;

		public const short AmethystGemsparkWall = 2679;

		public const short AmethystGemsparkWallOff = 2680;

		public const short DiamondGemsparkWall = 2681;

		public const short DiamondGemsparkWallOff = 2682;

		public const short EmeraldGemsparkWall = 2683;

		public const short EmeraldGemsparkWallOff = 2684;

		public const short RubyGemsparkWall = 2685;

		public const short RubyGemsparkWallOff = 2686;

		public const short SapphireGemsparkWall = 2687;

		public const short SapphireGemsparkWallOff = 2688;

		public const short TopazGemsparkWall = 2689;

		public const short TopazGemsparkWallOff = 2690;

		public const short TinPlatingWall = 2691;

		public const short TinPlating = 2692;

		public const short WaterfallBlock = 2693;

		public const short LavafallBlock = 2694;

		public const short ConfettiBlock = 2695;

		public const short ConfettiWall = 2696;

		public const short ConfettiBlockBlack = 2697;

		public const short ConfettiWallBlack = 2698;

		public const short WeaponRack = 2699;

		public const short FireworksBox = 2700;

		public const short LivingFireBlock = 2701;

		public const short AlphabetStatue0 = 2702;

		public const short AlphabetStatue1 = 2703;

		public const short AlphabetStatue2 = 2704;

		public const short AlphabetStatue3 = 2705;

		public const short AlphabetStatue4 = 2706;

		public const short AlphabetStatue5 = 2707;

		public const short AlphabetStatue6 = 2708;

		public const short AlphabetStatue7 = 2709;

		public const short AlphabetStatue8 = 2710;

		public const short AlphabetStatue9 = 2711;

		public const short AlphabetStatueA = 2712;

		public const short AlphabetStatueB = 2713;

		public const short AlphabetStatueC = 2714;

		public const short AlphabetStatueD = 2715;

		public const short AlphabetStatueE = 2716;

		public const short AlphabetStatueF = 2717;

		public const short AlphabetStatueG = 2718;

		public const short AlphabetStatueH = 2719;

		public const short AlphabetStatueI = 2720;

		public const short AlphabetStatueJ = 2721;

		public const short AlphabetStatueK = 2722;

		public const short AlphabetStatueL = 2723;

		public const short AlphabetStatueM = 2724;

		public const short AlphabetStatueN = 2725;

		public const short AlphabetStatueO = 2726;

		public const short AlphabetStatueP = 2727;

		public const short AlphabetStatueQ = 2728;

		public const short AlphabetStatueR = 2729;

		public const short AlphabetStatueS = 2730;

		public const short AlphabetStatueT = 2731;

		public const short AlphabetStatueU = 2732;

		public const short AlphabetStatueV = 2733;

		public const short AlphabetStatueW = 2734;

		public const short AlphabetStatueX = 2735;

		public const short AlphabetStatueY = 2736;

		public const short AlphabetStatueZ = 2737;

		public const short FireworkFountain = 2738;

		public const short BoosterTrack = 2739;

		public const short Grasshopper = 2740;

		public const short GrasshopperCage = 2741;

		public const short MusicBoxUndergroundCrimson = 2742;

		public const short CactusTable = 2743;

		public const short CactusPlatform = 2744;

		public const short BorealWoodSword = 2745;

		public const short BorealWoodHammer = 2746;

		public const short BorealWoodBow = 2747;

		public const short GlassChest = 2748;

		public const short XenoStaff = 2749;

		public const short MeteorStaff = 2750;

		public const short LivingCursedFireBlock = 2751;

		public const short LivingDemonFireBlock = 2752;

		public const short LivingFrostFireBlock = 2753;

		public const short LivingIchorBlock = 2754;

		public const short LivingUltrabrightFireBlock = 2755;

		public const short GenderChangePotion = 2756;

		public const short VortexHelmet = 2757;

		public const short VortexBreastplate = 2758;

		public const short VortexLeggings = 2759;

		public const short NebulaHelmet = 2760;

		public const short NebulaBreastplate = 2761;

		public const short NebulaLeggings = 2762;

		public const short SolarFlareHelmet = 2763;

		public const short SolarFlareBreastplate = 2764;

		public const short SolarFlareLeggings = 2765;

		public const short LunarTabletFragment = 2766;

		public const short SolarTablet = 2767;

		public const short DrillContainmentUnit = 2768;

		public const short CosmicCarKey = 2769;

		public const short MothronWings = 2770;

		public const short BrainScrambler = 2771;

		public const short VortexAxe = 2772;

		public const short VortexChainsaw = 2773;

		public const short VortexDrill = 2774;

		public const short VortexHammer = 2775;

		public const short VortexPickaxe = 2776;

		public const short NebulaAxe = 2777;

		public const short NebulaChainsaw = 2778;

		public const short NebulaDrill = 2779;

		public const short NebulaHammer = 2780;

		public const short NebulaPickaxe = 2781;

		public const short SolarFlareAxe = 2782;

		public const short SolarFlareChainsaw = 2783;

		public const short SolarFlareDrill = 2784;

		public const short SolarFlareHammer = 2785;

		public const short SolarFlarePickaxe = 2786;

		public const short HoneyfallBlock = 2787;

		public const short HoneyfallWall = 2788;

		public const short ChlorophyteBrickWall = 2789;

		public const short CrimtaneBrickWall = 2790;

		public const short ShroomitePlatingWall = 2791;

		public const short ChlorophyteBrick = 2792;

		public const short CrimtaneBrick = 2793;

		public const short ShroomitePlating = 2794;

		public const short LaserMachinegun = 2795;

		public const short ElectrosphereLauncher = 2796;

		public const short Xenopopper = 2797;

		public const short LaserDrill = 2798;

		public const short LaserRuler = 2799;

		public const short AntiGravityHook = 2800;

		public const short MoonMask = 2801;

		public const short SunMask = 2802;

		public const short MartianCostumeMask = 2803;

		public const short MartianCostumeShirt = 2804;

		public const short MartianCostumePants = 2805;

		public const short MartianUniformHelmet = 2806;

		public const short MartianUniformTorso = 2807;

		public const short MartianUniformPants = 2808;

		public const short MartianAstroClock = 2809;

		public const short MartianBathtub = 2810;

		public const short MartianBed = 2811;

		public const short MartianHoverChair = 2812;

		public const short MartianChandelier = 2813;

		public const short MartianChest = 2814;

		public const short MartianDoor = 2815;

		public const short MartianDresser = 2816;

		public const short MartianHolobookcase = 2817;

		public const short MartianHoverCandle = 2818;

		public const short MartianLamppost = 2819;

		public const short MartianLantern = 2820;

		public const short MartianPiano = 2821;

		public const short MartianPlatform = 2822;

		public const short MartianSofa = 2823;

		public const short MartianTable = 2824;

		public const short MartianTableLamp = 2825;

		public const short MartianWorkBench = 2826;

		public const short WoodenSink = 2827;

		public const short EbonwoodSink = 2828;

		public const short RichMahoganySink = 2829;

		public const short PearlwoodSink = 2830;

		public const short BoneSink = 2831;

		public const short FleshSink = 2832;

		public const short LivingWoodSink = 2833;

		public const short SkywareSink = 2834;

		public const short ShadewoodSink = 2835;

		public const short LihzahrdSink = 2836;

		public const short BlueDungeonSink = 2837;

		public const short GreenDungeonSink = 2838;

		public const short PinkDungeonSink = 2839;

		public const short ObsidianSink = 2840;

		public const short MetalSink = 2841;

		public const short GlassSink = 2842;

		public const short GoldenSink = 2843;

		public const short HoneySink = 2844;

		public const short SteampunkSink = 2845;

		public const short PumpkinSink = 2846;

		public const short SpookySink = 2847;

		public const short FrozenSink = 2848;

		public const short DynastySink = 2849;

		public const short PalmWoodSink = 2850;

		public const short MushroomSink = 2851;

		public const short BorealWoodSink = 2852;

		public const short SlimeSink = 2853;

		public const short CactusSink = 2854;

		public const short MartianSink = 2855;

		public const short WhiteLunaticHood = 2856;

		public const short BlueLunaticHood = 2857;

		public const short WhiteLunaticRobe = 2858;

		public const short BlueLunaticRobe = 2859;

		public const short MartianConduitPlating = 2860;

		public const short MartianConduitWall = 2861;

		public const short HiTekSunglasses = 2862;

		public const short MartianHairDye = 2863;

		public const short MartianArmorDye = 2864;

		public const short PaintingCastleMarsberg = 2865;

		public const short PaintingMartiaLisa = 2866;

		public const short PaintingTheTruthIsUpThere = 2867;

		public const short SmokeBlock = 2868;

		public const short LivingFlameDye = 2869;

		public const short LivingRainbowDye = 2870;

		public const short ShadowDye = 2871;

		public const short NegativeDye = 2872;

		public const short LivingOceanDye = 2873;

		public const short BrownDye = 2874;

		public const short BrownAndBlackDye = 2875;

		public const short BrightBrownDye = 2876;

		public const short BrownAndSilverDye = 2877;

		public const short WispDye = 2878;

		public const short PixieDye = 2879;

		public const short InfluxWaver = 2880;

		public const short PhasicWarpEjector = 2881;

		public const short ChargedBlasterCannon = 2882;

		public const short ChlorophyteDye = 2883;

		public const short UnicornWispDye = 2884;

		public const short InfernalWispDye = 2885;

		public const short ViciousPowder = 2886;

		public const short ViciousMushroom = 2887;

		public const short BeesKnees = 2888;

		public const short GoldBird = 2889;

		public const short GoldBunny = 2890;

		public const short GoldButterfly = 2891;

		public const short GoldFrog = 2892;

		public const short GoldGrasshopper = 2893;

		public const short GoldMouse = 2894;

		public const short GoldWorm = 2895;

		public const short StickyDynamite = 2896;

		public const short AngryTrapperBanner = 2897;

		public const short ArmoredVikingBanner = 2898;

		public const short BlackSlimeBanner = 2899;

		public const short BlueArmoredBonesBanner = 2900;

		public const short BlueCultistArcherBanner = 2901;

		public const short BlueCultistCasterBanner = 2902;

		public const short BlueCultistFighterBanner = 2903;

		public const short BoneLeeBanner = 2904;

		public const short ClingerBanner = 2905;

		public const short CochinealBeetleBanner = 2906;

		public const short CorruptPenguinBanner = 2907;

		public const short CorruptSlimeBanner = 2908;

		public const short CorruptorBanner = 2909;

		public const short CrimslimeBanner = 2910;

		public const short CursedSkullBanner = 2911;

		public const short CyanBeetleBanner = 2912;

		public const short DevourerBanner = 2913;

		public const short DiablolistBanner = 2914;

		public const short DoctorBonesBanner = 2915;

		public const short DungeonSlimeBanner = 2916;

		public const short DungeonSpiritBanner = 2917;

		public const short ElfArcherBanner = 2918;

		public const short ElfCopterBanner = 2919;

		public const short EyezorBanner = 2920;

		public const short FlockoBanner = 2921;

		public const short GhostBanner = 2922;

		public const short GiantBatBanner = 2923;

		public const short GiantCursedSkullBanner = 2924;

		public const short GiantFlyingFoxBanner = 2925;

		public const short GingerbreadManBanner = 2926;

		public const short GoblinArcherBanner = 2927;

		public const short GreenSlimeBanner = 2928;

		public const short HeadlessHorsemanBanner = 2929;

		public const short HellArmoredBonesBanner = 2930;

		public const short HellhoundBanner = 2931;

		public const short HoppinJackBanner = 2932;

		public const short IceBatBanner = 2933;

		public const short IceGolemBanner = 2934;

		public const short IceSlimeBanner = 2935;

		public const short IchorStickerBanner = 2936;

		public const short IlluminantBatBanner = 2937;

		public const short IlluminantSlimeBanner = 2938;

		public const short JungleBatBanner = 2939;

		public const short JungleSlimeBanner = 2940;

		public const short KrampusBanner = 2941;

		public const short LacBeetleBanner = 2942;

		public const short LavaBatBanner = 2943;

		public const short LavaSlimeBanner = 2944;

		public const short MartianBrainscramblerBanner = 2945;

		public const short MartianDroneBanner = 2946;

		public const short MartianEngineerBanner = 2947;

		public const short MartianGigazapperBanner = 2948;

		public const short MartianGreyGruntBanner = 2949;

		public const short MartianOfficerBanner = 2950;

		public const short MartianRaygunnerBanner = 2951;

		public const short MartianScutlixGunnerBanner = 2952;

		public const short MartianTeslaTurretBanner = 2953;

		public const short MisterStabbyBanner = 2954;

		public const short MotherSlimeBanner = 2955;

		public const short NecromancerBanner = 2956;

		public const short NutcrackerBanner = 2957;

		public const short PaladinBanner = 2958;

		public const short PenguinBanner = 2959;

		public const short PinkyBanner = 2960;

		public const short PoltergeistBanner = 2961;

		public const short PossessedArmorBanner = 2962;

		public const short PresentMimicBanner = 2963;

		public const short PurpleSlimeBanner = 2964;

		public const short RaggedCasterBanner = 2965;

		public const short RainbowSlimeBanner = 2966;

		public const short RavenBanner = 2967;

		public const short RedSlimeBanner = 2968;

		public const short RuneWizardBanner = 2969;

		public const short RustyArmoredBonesBanner = 2970;

		public const short ScarecrowBanner = 2971;

		public const short ScutlixBanner = 2972;

		public const short SkeletonArcherBanner = 2973;

		public const short SkeletonCommandoBanner = 2974;

		public const short SkeletonSniperBanner = 2975;

		public const short SlimerBanner = 2976;

		public const short SnatcherBanner = 2977;

		public const short SnowBallaBanner = 2978;

		public const short SnowmanGangstaBanner = 2979;

		public const short SpikedIceSlimeBanner = 2980;

		public const short SpikedJungleSlimeBanner = 2981;

		public const short SplinterlingBanner = 2982;

		public const short SquidBanner = 2983;

		public const short TacticalSkeletonBanner = 2984;

		public const short TheGroomBanner = 2985;

		public const short TimBanner = 2986;

		public const short UndeadMinerBanner = 2987;

		public const short UndeadVikingBanner = 2988;

		public const short WhiteCultistArcherBanner = 2989;

		public const short WhiteCultistCasterBanner = 2990;

		public const short WhiteCultistFighterBanner = 2991;

		public const short YellowSlimeBanner = 2992;

		public const short YetiBanner = 2993;

		public const short ZombieElfBanner = 2994;

		public const short SparkyPainting = 2995;

		public const short VineRope = 2996;

		public const short WormholePotion = 2997;

		public const short SummonerEmblem = 2998;

		public const short BewitchingTable = 2999;

		public const short AlchemyTable = 3000;

		public const short StrangeBrew = 3001;

		public const short SpelunkerGlowstick = 3002;

		public const short BoneArrow = 3003;

		public const short BoneTorch = 3004;

		public const short VineRopeCoil = 3005;

		public const short SoulDrain = 3006;

		public const short DartPistol = 3007;

		public const short DartRifle = 3008;

		public const short CrystalDart = 3009;

		public const short CursedDart = 3010;

		public const short IchorDart = 3011;

		public const short ChainGuillotines = 3012;

		public const short FetidBaghnakhs = 3013;

		public const short ClingerStaff = 3014;

		public const short PutridScent = 3015;

		public const short FleshKnuckles = 3016;

		public const short FlowerBoots = 3017;

		public const short Seedler = 3018;

		public const short HellwingBow = 3019;

		public const short TendonHook = 3020;

		public const short ThornHook = 3021;

		public const short IlluminantHook = 3022;

		public const short WormHook = 3023;

		public const short DevDye = 3024;

		public const short PurpleOozeDye = 3025;

		public const short ReflectiveSilverDye = 3026;

		public const short ReflectiveGoldDye = 3027;

		public const short BlueAcidDye = 3028;

		public const short DaedalusStormbow = 3029;

		public const short FlyingKnife = 3030;

		public const short BottomlessBucket = 3031;

		public const short SuperAbsorbantSponge = 3032;

		public const short GoldRing = 3033;

		public const short CoinRing = 3034;

		public const short GreedyRing = 3035;

		public const short FishFinder = 3036;

		public const short WeatherRadio = 3037;

		public const short HadesDye = 3038;

		public const short TwilightDye = 3039;

		public const short AcidDye = 3040;

		public const short MushroomDye = 3041;

		public const short PhaseDye = 3042;

		public const short MagicLantern = 3043;

		public const short MusicBoxLunarBoss = 3044;

		public const short RainbowTorch = 3045;

		public const short CursedCampfire = 3046;

		public const short DemonCampfire = 3047;

		public const short FrozenCampfire = 3048;

		public const short IchorCampfire = 3049;

		public const short RainbowCampfire = 3050;

		public const short CrystalVileShard = 3051;

		public const short ShadowFlameBow = 3052;

		public const short ShadowFlameHexDoll = 3053;

		public const short ShadowFlameKnife = 3054;

		public const short PaintingAcorns = 3055;

		public const short PaintingColdSnap = 3056;

		public const short PaintingCursedSaint = 3057;

		public const short PaintingSnowfellas = 3058;

		public const short PaintingTheSeason = 3059;

		public const short BoneRattle = 3060;

		public const short ArchitectGizmoPack = 3061;

		public const short CrimsonHeart = 3062;

		public const short Meowmere = 3063;

		public const short Sundial = 3064;

		public const short StarWrath = 3065;

		public const short MarbleBlock = 3066;

		public const short HellstoneBrickWall = 3067;

		public const short CordageGuide = 3068;

		public const short WandofSparking = 3069;

		public const short GoldBirdCage = 3070;

		public const short GoldBunnyCage = 3071;

		public const short GoldButterflyCage = 3072;

		public const short GoldFrogCage = 3073;

		public const short GoldGrasshopperCage = 3074;

		public const short GoldMouseCage = 3075;

		public const short GoldWormCage = 3076;

		public const short SilkRope = 3077;

		public const short WebRope = 3078;

		public const short SilkRopeCoil = 3079;

		public const short WebRopeCoil = 3080;

		public const short Marble = 3081;

		public const short MarbleWall = 3082;

		public const short MarbleBlockWall = 3083;

		public const short Radar = 3084;

		public const short LockBox = 3085;

		public const short Granite = 3086;

		public const short GraniteBlock = 3087;

		public const short GraniteWall = 3088;

		public const short GraniteBlockWall = 3089;

		public const short RoyalGel = 3090;

		public const short NightKey = 3091;

		public const short LightKey = 3092;

		public const short HerbBag = 3093;

		public const short Javelin = 3094;

		public const short TallyCounter = 3095;

		public const short Sextant = 3096;

		public const short EoCShield = 3097;

		public const short ButchersChainsaw = 3098;

		public const short Stopwatch = 3099;

		public const short MeteoriteBrick = 3100;

		public const short MeteoriteBrickWall = 3101;

		public const short MetalDetector = 3102;

		public const short EndlessQuiver = 3103;

		public const short EndlessMusketPouch = 3104;

		public const short ToxicFlask = 3105;

		public const short PsychoKnife = 3106;

		public const short NailGun = 3107;

		public const short Nail = 3108;

		public const short NightVisionHelmet = 3109;

		public const short CelestialShell = 3110;

		public const short PinkGel = 3111;

		public const short BouncyGlowstick = 3112;

		public const short PinkSlimeBlock = 3113;

		public const short PinkTorch = 3114;

		public const short BouncyBomb = 3115;

		public const short BouncyGrenade = 3116;

		public const short PeaceCandle = 3117;

		public const short LifeformAnalyzer = 3118;

		public const short DPSMeter = 3119;

		public const short FishermansGuide = 3120;

		public const short GoblinTech = 3121;

		public const short REK = 3122;

		public const short PDA = 3123;

		public const short CellPhone = 3124;

		public const short GraniteChest = 3125;

		public const short MeteoriteClock = 3126;

		public const short MarbleClock = 3127;

		public const short GraniteClock = 3128;

		public const short MeteoriteDoor = 3129;

		public const short MarbleDoor = 3130;

		public const short GraniteDoor = 3131;

		public const short MeteoriteDresser = 3132;

		public const short MarbleDresser = 3133;

		public const short GraniteDresser = 3134;

		public const short MeteoriteLamp = 3135;

		public const short MarbleLamp = 3136;

		public const short GraniteLamp = 3137;

		public const short MeteoriteLantern = 3138;

		public const short MarbleLantern = 3139;

		public const short GraniteLantern = 3140;

		public const short MeteoritePiano = 3141;

		public const short MarblePiano = 3142;

		public const short GranitePiano = 3143;

		public const short MeteoritePlatform = 3144;

		public const short MarblePlatform = 3145;

		public const short GranitePlatform = 3146;

		public const short MeteoriteSink = 3147;

		public const short MarbleSink = 3148;

		public const short GraniteSink = 3149;

		public const short MeteoriteSofa = 3150;

		public const short MarbleSofa = 3151;

		public const short GraniteSofa = 3152;

		public const short MeteoriteTable = 3153;

		public const short MarbleTable = 3154;

		public const short GraniteTable = 3155;

		public const short MeteoriteWorkBench = 3156;

		public const short MarbleWorkBench = 3157;

		public const short GraniteWorkBench = 3158;

		public const short MeteoriteBathtub = 3159;

		public const short MarbleBathtub = 3160;

		public const short GraniteBathtub = 3161;

		public const short MeteoriteBed = 3162;

		public const short MarbleBed = 3163;

		public const short GraniteBed = 3164;

		public const short MeteoriteBookcase = 3165;

		public const short MarbleBookcase = 3166;

		public const short GraniteBookcase = 3167;

		public const short MeteoriteCandelabra = 3168;

		public const short MarbleCandelabra = 3169;

		public const short GraniteCandelabra = 3170;

		public const short MeteoriteCandle = 3171;

		public const short MarbleCandle = 3172;

		public const short GraniteCandle = 3173;

		public const short MeteoriteChair = 3174;

		public const short MarbleChair = 3175;

		public const short GraniteChair = 3176;

		public const short MeteoriteChandelier = 3177;

		public const short MarbleChandelier = 3178;

		public const short GraniteChandelier = 3179;

		public const short MeteoriteChest = 3180;

		public const short MarbleChest = 3181;

		public const short MagicWaterDropper = 3182;

		public const short GoldenBugNet = 3183;

		public const short MagicLavaDropper = 3184;

		public const short MagicHoneyDropper = 3185;

		public const short EmptyDropper = 3186;

		public const short GladiatorHelmet = 3187;

		public const short GladiatorBreastplate = 3188;

		public const short GladiatorLeggings = 3189;

		public const short ReflectiveDye = 3190;

		public const short EnchantedNightcrawler = 3191;

		public const short Grubby = 3192;

		public const short Sluggy = 3193;

		public const short Buggy = 3194;

		public const short GrubSoup = 3195;

		public const short BombFish = 3196;

		public const short FrostDaggerfish = 3197;

		public const short SharpeningStation = 3198;

		public const short IceMirror = 3199;

		public const short SailfishBoots = 3200;

		public const short TsunamiInABottle = 3201;

		public const short TargetDummy = 3202;

		public const short CorruptFishingCrate = 3203;

		public const short CrimsonFishingCrate = 3204;

		public const short DungeonFishingCrate = 3205;

		public const short FloatingIslandFishingCrate = 3206;

		public const short HallowedFishingCrate = 3207;

		public const short JungleFishingCrate = 3208;

		public const short CrystalSerpent = 3209;

		public const short Toxikarp = 3210;

		public const short Bladetongue = 3211;

		public const short SharkToothNecklace = 3212;

		public const short MoneyTrough = 3213;

		public const short Bubble = 3214;

		public const short DayBloomPlanterBox = 3215;

		public const short MoonglowPlanterBox = 3216;

		public const short CorruptPlanterBox = 3217;

		public const short CrimsonPlanterBox = 3218;

		public const short BlinkrootPlanterBox = 3219;

		public const short WaterleafPlanterBox = 3220;

		public const short ShiverthornPlanterBox = 3221;

		public const short FireBlossomPlanterBox = 3222;

		public const short BrainOfConfusion = 3223;

		public const short WormScarf = 3224;

		public const short BalloonPufferfish = 3225;

		public const short BejeweledValkyrieHead = 3226;

		public const short BejeweledValkyrieBody = 3227;

		public const short BejeweledValkyrieWing = 3228;

		public const short RichGravestone1 = 3229;

		public const short RichGravestone2 = 3230;

		public const short RichGravestone3 = 3231;

		public const short RichGravestone4 = 3232;

		public const short RichGravestone5 = 3233;

		public const short CrystalBlock = 3234;

		public const short MusicBoxMartians = 3235;

		public const short MusicBoxPirates = 3236;

		public const short MusicBoxHell = 3237;

		public const short CrystalBlockWall = 3238;

		public const short Trapdoor = 3239;

		public const short TallGate = 3240;

		public const short SharkronBalloon = 3241;

		public const short TaxCollectorHat = 3242;

		public const short TaxCollectorSuit = 3243;

		public const short TaxCollectorPants = 3244;

		public const short BoneGlove = 3245;

		public const short ClothierJacket = 3246;

		public const short ClothierPants = 3247;

		public const short DyeTraderTurban = 3248;

		public const short DeadlySphereStaff = 3249;

		public const short BalloonHorseshoeFart = 3250;

		public const short BalloonHorseshoeHoney = 3251;

		public const short BalloonHorseshoeSharkron = 3252;

		public const short LavaLamp = 3253;

		public const short CageEnchantedNightcrawler = 3254;

		public const short CageBuggy = 3255;

		public const short CageGrubby = 3256;

		public const short CageSluggy = 3257;

		public const short SlapHand = 3258;

		public const short TwilightHairDye = 3259;

		public const short BlessedApple = 3260;

		public const short SpectreBar = 3261;

		public const short Code1 = 3262;

		public const short BuccaneerBandana = 3263;

		public const short BuccaneerShirt = 3264;

		public const short BuccaneerPants = 3265;

		public const short ObsidianHelm = 3266;

		public const short ObsidianShirt = 3267;

		public const short ObsidianPants = 3268;

		public const short MedusaHead = 3269;

		public const short ItemFrame = 3270;

		public const short Sandstone = 3271;

		public const short HardenedSand = 3272;

		public const short SandstoneWall = 3273;

		public const short CorruptHardenedSand = 3274;

		public const short CrimsonHardenedSand = 3275;

		public const short CorruptSandstone = 3276;

		public const short CrimsonSandstone = 3277;

		public const short WoodYoyo = 3278;

		public const short CorruptYoyo = 3279;

		public const short CrimsonYoyo = 3280;

		public const short JungleYoyo = 3281;

		public const short Cascade = 3282;

		public const short Chik = 3283;

		public const short Code2 = 3284;

		public const short Rally = 3285;

		public const short Yelets = 3286;

		public const short RedsYoyo = 3287;

		public const short ValkyrieYoyo = 3288;

		public const short Amarok = 3289;

		public const short HelFire = 3290;

		public const short Kraken = 3291;

		public const short TheEyeOfCthulhu = 3292;

		public const short RedString = 3293;

		public const short OrangeString = 3294;

		public const short YellowString = 3295;

		public const short LimeString = 3296;

		public const short GreenString = 3297;

		public const short TealString = 3298;

		public const short CyanString = 3299;

		public const short SkyBlueString = 3300;

		public const short BlueString = 3301;

		public const short PurpleString = 3302;

		public const short VioletString = 3303;

		public const short PinkString = 3304;

		public const short BrownString = 3305;

		public const short WhiteString = 3306;

		public const short RainbowString = 3307;

		public const short BlackString = 3308;

		public const short BlackCounterweight = 3309;

		public const short BlueCounterweight = 3310;

		public const short GreenCounterweight = 3311;

		public const short PurpleCounterweight = 3312;

		public const short RedCounterweight = 3313;

		public const short YellowCounterweight = 3314;

		public const short FormatC = 3315;

		public const short Gradient = 3316;

		public const short Valor = 3317;

		public const short KingSlimeBossBag = 3318;

		public const short EyeOfCthulhuBossBag = 3319;

		public const short EaterOfWorldsBossBag = 3320;

		public const short BrainOfCthulhuBossBag = 3321;

		public const short QueenBeeBossBag = 3322;

		public const short SkeletronBossBag = 3323;

		public const short WallOfFleshBossBag = 3324;

		public const short DestroyerBossBag = 3325;

		public const short TwinsBossBag = 3326;

		public const short SkeletronPrimeBossBag = 3327;

		public const short PlanteraBossBag = 3328;

		public const short GolemBossBag = 3329;

		public const short FishronBossBag = 3330;

		public const short CultistBossBag = 3331;

		public const short MoonLordBossBag = 3332;

		public const short HiveBackpack = 3333;

		public const short YoYoGlove = 3334;

		public const short DemonHeart = 3335;

		public const short SporeSac = 3336;

		public const short ShinyStone = 3337;

		public const short HallowHardenedSand = 3338;

		public const short HallowSandstone = 3339;

		public const short HardenedSandWall = 3340;

		public const short CorruptHardenedSandWall = 3341;

		public const short CrimsonHardenedSandWall = 3342;

		public const short HallowHardenedSandWall = 3343;

		public const short CorruptSandstoneWall = 3344;

		public const short CrimsonSandstoneWall = 3345;

		public const short HallowSandstoneWall = 3346;

		public const short DesertFossil = 3347;

		public const short DesertFossilWall = 3348;

		public const short DyeTradersScimitar = 3349;

		public const short PainterPaintballGun = 3350;

		public const short TaxCollectorsStickOfDoom = 3351;

		public const short StylistKilLaKillScissorsIWish = 3352;

		public const short MinecartMech = 3353;

		public const short MechanicalWheelPiece = 3354;

		public const short MechanicalWagonPiece = 3355;

		public const short MechanicalBatteryPiece = 3356;

		public const short AncientCultistTrophy = 3357;

		public const short MartianSaucerTrophy = 3358;

		public const short FlyingDutchmanTrophy = 3359;

		public const short LivingMahoganyWand = 3360;

		public const short LivingMahoganyLeafWand = 3361;

		public const short FallenTuxedoShirt = 3362;

		public const short FallenTuxedoPants = 3363;

		public const short Fireplace = 3364;

		public const short Chimney = 3365;

		public const short YoyoBag = 3366;

		public const short ShrimpyTruffle = 3367;

		public const short Arkhalis = 3368;

		public const short ConfettiCannon = 3369;

		public const short MusicBoxTowers = 3370;

		public const short MusicBoxGoblins = 3371;

		public const short BossMaskCultist = 3372;

		public const short BossMaskMoonlord = 3373;

		public const short FossilHelm = 3374;

		public const short FossilShirt = 3375;

		public const short FossilPants = 3376;

		public const short AmberStaff = 3377;

		public const short BoneJavelin = 3378;

		public const short BoneDagger = 3379;

		public const short FossilOre = 3380;

		public const short StardustHelmet = 3381;

		public const short StardustBreastplate = 3382;

		public const short StardustLeggings = 3383;

		public const short PortalGun = 3384;

		public const short StrangePlant1 = 3385;

		public const short StrangePlant2 = 3386;

		public const short StrangePlant3 = 3387;

		public const short StrangePlant4 = 3388;

		public const short Terrarian = 3389;

		public const short GoblinSummonerBanner = 3390;

		public const short SalamanderBanner = 3391;

		public const short GiantShellyBanner = 3392;

		public const short CrawdadBanner = 3393;

		public const short FritzBanner = 3394;

		public const short CreatureFromTheDeepBanner = 3395;

		public const short DrManFlyBanner = 3396;

		public const short MothronBanner = 3397;

		public const short SeveredHandBanner = 3398;

		public const short ThePossessedBanner = 3399;

		public const short ButcherBanner = 3400;

		public const short PsychoBanner = 3401;

		public const short DeadlySphereBanner = 3402;

		public const short NailheadBanner = 3403;

		public const short PoisonousSporeBanner = 3404;

		public const short MedusaBanner = 3405;

		public const short GreekSkeletonBanner = 3406;

		public const short GraniteFlyerBanner = 3407;

		public const short GraniteGolemBanner = 3408;

		public const short BloodZombieBanner = 3409;

		public const short DripplerBanner = 3410;

		public const short TombCrawlerBanner = 3411;

		public const short DuneSplicerBanner = 3412;

		public const short FlyingAntlionBanner = 3413;

		public const short WalkingAntlionBanner = 3414;

		public const short DesertGhoulBanner = 3415;

		public const short DesertLamiaBanner = 3416;

		public const short DesertDjinnBanner = 3417;

		public const short DesertBasiliskBanner = 3418;

		public const short RavagerScorpionBanner = 3419;

		public const short StardustSoldierBanner = 3420;

		public const short StardustWormBanner = 3421;

		public const short StardustJellyfishBanner = 3422;

		public const short StardustSpiderBanner = 3423;

		public const short StardustSmallCellBanner = 3424;

		public const short StardustLargeCellBanner = 3425;

		public const short SolarCoriteBanner = 3426;

		public const short SolarSrollerBanner = 3427;

		public const short SolarCrawltipedeBanner = 3428;

		public const short SolarDrakomireRiderBanner = 3429;

		public const short SolarDrakomireBanner = 3430;

		public const short SolarSolenianBanner = 3431;

		public const short NebulaSoldierBanner = 3432;

		public const short NebulaHeadcrabBanner = 3433;

		public const short NebulaBrainBanner = 3434;

		public const short NebulaBeastBanner = 3435;

		public const short VortexLarvaBanner = 3436;

		public const short VortexHornetQueenBanner = 3437;

		public const short VortexHornetBanner = 3438;

		public const short VortexSoldierBanner = 3439;

		public const short VortexRiflemanBanner = 3440;

		public const short PirateCaptainBanner = 3441;

		public const short PirateDeadeyeBanner = 3442;

		public const short PirateCorsairBanner = 3443;

		public const short PirateCrossbowerBanner = 3444;

		public const short MartianWalkerBanner = 3445;

		public const short RedDevilBanner = 3446;

		public const short PinkJellyfishBanner = 3447;

		public const short GreenJellyfishBanner = 3448;

		public const short DarkMummyBanner = 3449;

		public const short LightMummyBanner = 3450;

		public const short AngryBonesBanner = 3451;

		public const short IceTortoiseBanner = 3452;

		public const short NebulaPickup1 = 3453;

		public const short NebulaPickup2 = 3454;

		public const short NebulaPickup3 = 3455;

		public const short FragmentVortex = 3456;

		public const short FragmentNebula = 3457;

		public const short FragmentSolar = 3458;

		public const short FragmentStardust = 3459;

		public const short LunarOre = 3460;

		public const short LunarBrick = 3461;

		public const short StardustAxe = 3462;

		public const short StardustChainsaw = 3463;

		public const short StardustDrill = 3464;

		public const short StardustHammer = 3465;

		public const short StardustPickaxe = 3466;

		public const short LunarBar = 3467;

		public const short WingsSolar = 3468;

		public const short WingsVortex = 3469;

		public const short WingsNebula = 3470;

		public const short WingsStardust = 3471;

		public const short LunarBrickWall = 3472;

		public const short SolarEruption = 3473;

		public const short StardustCellStaff = 3474;

		public const short VortexBeater = 3475;

		public const short NebulaArcanum = 3476;

		public const short BloodWater = 3477;

		public const short TheBrideHat = 3478;

		public const short TheBrideDress = 3479;

		public const short PlatinumBow = 3480;

		public const short PlatinumHammer = 3481;

		public const short PlatinumAxe = 3482;

		public const short PlatinumShortsword = 3483;

		public const short PlatinumBroadsword = 3484;

		public const short PlatinumPickaxe = 3485;

		public const short TungstenBow = 3486;

		public const short TungstenHammer = 3487;

		public const short TungstenAxe = 3488;

		public const short TungstenShortsword = 3489;

		public const short TungstenBroadsword = 3490;

		public const short TungstenPickaxe = 3491;

		public const short LeadBow = 3492;

		public const short LeadHammer = 3493;

		public const short LeadAxe = 3494;

		public const short LeadShortsword = 3495;

		public const short LeadBroadsword = 3496;

		public const short LeadPickaxe = 3497;

		public const short TinBow = 3498;

		public const short TinHammer = 3499;

		public const short TinAxe = 3500;

		public const short TinShortsword = 3501;

		public const short TinBroadsword = 3502;

		public const short TinPickaxe = 3503;

		public const short CopperBow = 3504;

		public const short CopperHammer = 3505;

		public const short CopperAxe = 3506;

		public const short CopperShortsword = 3507;

		public const short CopperBroadsword = 3508;

		public const short CopperPickaxe = 3509;

		public const short SilverBow = 3510;

		public const short SilverHammer = 3511;

		public const short SilverAxe = 3512;

		public const short SilverShortsword = 3513;

		public const short SilverBroadsword = 3514;

		public const short SilverPickaxe = 3515;

		public const short GoldBow = 3516;

		public const short GoldHammer = 3517;

		public const short GoldAxe = 3518;

		public const short GoldShortsword = 3519;

		public const short GoldBroadsword = 3520;

		public const short GoldPickaxe = 3521;

		public const short LunarHamaxeSolar = 3522;

		public const short LunarHamaxeVortex = 3523;

		public const short LunarHamaxeNebula = 3524;

		public const short LunarHamaxeStardust = 3525;

		public const short SolarDye = 3526;

		public const short NebulaDye = 3527;

		public const short VortexDye = 3528;

		public const short StardustDye = 3529;

		public const short VoidDye = 3530;

		public const short StardustDragonStaff = 3531;

		public const short Bacon = 3532;

		public const short ShiftingSandsDye = 3533;

		public const short MirageDye = 3534;

		public const short ShiftingPearlSandsDye = 3535;

		public const short VortexMonolith = 3536;

		public const short NebulaMonolith = 3537;

		public const short StardustMonolith = 3538;

		public const short SolarMonolith = 3539;

		public const short Phantasm = 3540;

		public const short LastPrism = 3541;

		public const short NebulaBlaze = 3542;

		public const short DayBreak = 3543;

		public const short SuperHealingPotion = 3544;

		public const short Detonator = 3545;

		public const short FireworksLauncher = 3546;

		public const short BouncyDynamite = 3547;

		public const short PartyGirlGrenade = 3548;

		public const short LunarCraftingStation = 3549;

		public const short FlameAndSilverDye = 3550;

		public const short GreenFlameAndSilverDye = 3551;

		public const short BlueFlameAndSilverDye = 3552;

		public const short ReflectiveCopperDye = 3553;

		public const short ReflectiveObsidianDye = 3554;

		public const short ReflectiveMetalDye = 3555;

		public const short MidnightRainbowDye = 3556;

		public const short BlackAndWhiteDye = 3557;

		public const short BrightSilverDye = 3558;

		public const short SilverAndBlackDye = 3559;

		public const short RedAcidDye = 3560;

		public const short GelDye = 3561;

		public const short PinkGelDye = 3562;

		public const short SquirrelRed = 3563;

		public const short SquirrelGold = 3564;

		public const short SquirrelOrangeCage = 3565;

		public const short SquirrelGoldCage = 3566;

		public const short MoonlordBullet = 3567;

		public const short MoonlordArrow = 3568;

		public const short MoonlordTurretStaff = 3569;

		public const short LunarFlareBook = 3570;

		public const short RainbowCrystalStaff = 3571;

		public const short LunarHook = 3572;

		public const short LunarBlockSolar = 3573;

		public const short LunarBlockVortex = 3574;

		public const short LunarBlockNebula = 3575;

		public const short LunarBlockStardust = 3576;

		public const short SuspiciousLookingTentacle = 3577;

		public const short Yoraiz0rShirt = 3578;

		public const short Yoraiz0rPants = 3579;

		public const short Yoraiz0rWings = 3580;

		public const short Yoraiz0rDarkness = 3581;

		public const short JimsWings = 3582;

		public const short Yoraiz0rHead = 3583;

		public const short LivingLeafWall = 3584;

		public const short SkiphsHelm = 3585;

		public const short SkiphsShirt = 3586;

		public const short SkiphsPants = 3587;

		public const short SkiphsWings = 3588;

		public const short LokisHelm = 3589;

		public const short LokisShirt = 3590;

		public const short LokisPants = 3591;

		public const short LokisWings = 3592;

		public const short SandSlimeBanner = 3593;

		public const short SeaSnailBanner = 3594;

		public const short MoonLordTrophy = 3595;

		public const short MoonLordPainting = 3596;

		public const short BurningHadesDye = 3597;

		public const short GrimDye = 3598;

		public const short LokisDye = 3599;

		public const short ShadowflameHadesDye = 3600;

		public const short CelestialSigil = 3601;

		public const short LogicGateLamp_Off = 3602;

		public const short LogicGate_AND = 3603;

		public const short LogicGate_OR = 3604;

		public const short LogicGate_NAND = 3605;

		public const short LogicGate_NOR = 3606;

		public const short LogicGate_XOR = 3607;

		public const short LogicGate_NXOR = 3608;

		public const short ConveyorBeltLeft = 3609;

		public const short ConveyorBeltRight = 3610;

		public const short WireKite = 3611;

		public const short YellowWrench = 3612;

		public const short LogicSensor_Sun = 3613;

		public const short LogicSensor_Moon = 3614;

		public const short LogicSensor_Above = 3615;

		public const short WirePipe = 3616;

		public const short AnnouncementBox = 3617;

		public const short LogicGateLamp_On = 3618;

		public const short MechanicalLens = 3619;

		public const short ActuationRod = 3620;

		public const short TeamBlockRed = 3621;

		public const short TeamBlockRedPlatform = 3622;

		public const short StaticHook = 3623;

		public const short ActuationAccessory = 3624;

		public const short MulticolorWrench = 3625;

		public const short WeightedPressurePlatePink = 3626;

		public const short EngineeringHelmet = 3627;

		public const short CompanionCube = 3628;

		public const short WireBulb = 3629;

		public const short WeightedPressurePlateOrange = 3630;

		public const short WeightedPressurePlatePurple = 3631;

		public const short WeightedPressurePlateCyan = 3632;

		public const short TeamBlockGreen = 3633;

		public const short TeamBlockBlue = 3634;

		public const short TeamBlockYellow = 3635;

		public const short TeamBlockPink = 3636;

		public const short TeamBlockWhite = 3637;

		public const short TeamBlockGreenPlatform = 3638;

		public const short TeamBlockBluePlatform = 3639;

		public const short TeamBlockYellowPlatform = 3640;

		public const short TeamBlockPinkPlatform = 3641;

		public const short TeamBlockWhitePlatform = 3642;

		public const short LargeAmber = 3643;

		public const short GemLockRuby = 3644;

		public const short GemLockSapphire = 3645;

		public const short GemLockEmerald = 3646;

		public const short GemLockTopaz = 3647;

		public const short GemLockAmethyst = 3648;

		public const short GemLockDiamond = 3649;

		public const short GemLockAmber = 3650;

		public const short SquirrelStatue = 3651;

		public const short ButterflyStatue = 3652;

		public const short WormStatue = 3653;

		public const short FireflyStatue = 3654;

		public const short ScorpionStatue = 3655;

		public const short SnailStatue = 3656;

		public const short GrasshopperStatue = 3657;

		public const short MouseStatue = 3658;

		public const short DuckStatue = 3659;

		public const short PenguinStatue = 3660;

		public const short FrogStatue = 3661;

		public const short BuggyStatue = 3662;

		public const short LogicGateLamp_Faulty = 3663;

		public const short PortalGunStation = 3664;

		public const short Fake_Chest = 3665;

		public const short Fake_GoldChest = 3666;

		public const short Fake_ShadowChest = 3667;

		public const short Fake_EbonwoodChest = 3668;

		public const short Fake_RichMahoganyChest = 3669;

		public const short Fake_PearlwoodChest = 3670;

		public const short Fake_IvyChest = 3671;

		public const short Fake_IceChest = 3672;

		public const short Fake_LivingWoodChest = 3673;

		public const short Fake_SkywareChest = 3674;

		public const short Fake_ShadewoodChest = 3675;

		public const short Fake_WebCoveredChest = 3676;

		public const short Fake_LihzahrdChest = 3677;

		public const short Fake_WaterChest = 3678;

		public const short Fake_JungleChest = 3679;

		public const short Fake_CorruptionChest = 3680;

		public const short Fake_CrimsonChest = 3681;

		public const short Fake_HallowedChest = 3682;

		public const short Fake_FrozenChest = 3683;

		public const short Fake_DynastyChest = 3684;

		public const short Fake_HoneyChest = 3685;

		public const short Fake_SteampunkChest = 3686;

		public const short Fake_PalmWoodChest = 3687;

		public const short Fake_MushroomChest = 3688;

		public const short Fake_BorealWoodChest = 3689;

		public const short Fake_SlimeChest = 3690;

		public const short Fake_GreenDungeonChest = 3691;

		public const short Fake_PinkDungeonChest = 3692;

		public const short Fake_BlueDungeonChest = 3693;

		public const short Fake_BoneChest = 3694;

		public const short Fake_CactusChest = 3695;

		public const short Fake_FleshChest = 3696;

		public const short Fake_ObsidianChest = 3697;

		public const short Fake_PumpkinChest = 3698;

		public const short Fake_SpookyChest = 3699;

		public const short Fake_GlassChest = 3700;

		public const short Fake_MartianChest = 3701;

		public const short Fake_MeteoriteChest = 3702;

		public const short Fake_GraniteChest = 3703;

		public const short Fake_MarbleChest = 3704;

		public const short Fake_newchest1 = 3705;

		public const short Fake_newchest2 = 3706;

		public const short ProjectilePressurePad = 3707;

		public const short WallCreeperStatue = 3708;

		public const short UnicornStatue = 3709;

		public const short DripplerStatue = 3710;

		public const short WraithStatue = 3711;

		public const short BoneSkeletonStatue = 3712;

		public const short UndeadVikingStatue = 3713;

		public const short MedusaStatue = 3714;

		public const short HarpyStatue = 3715;

		public const short PigronStatue = 3716;

		public const short HopliteStatue = 3717;

		public const short GraniteGolemStatue = 3718;

		public const short ZombieArmStatue = 3719;

		public const short BloodZombieStatue = 3720;

		public const short AnglerTackleBag = 3721;

		public const short GeyserTrap = 3722;

		public const short UltraBrightCampfire = 3723;

		public const short BoneCampfire = 3724;

		public const short PixelBox = 3725;

		public const short LogicSensor_Water = 3726;

		public const short LogicSensor_Lava = 3727;

		public const short LogicSensor_Honey = 3728;

		public const short LogicSensor_Liquid = 3729;

		public const short PartyBundleOfBalloonsAccessory = 3730;

		public const short PartyBalloonAnimal = 3731;

		public const short PartyHat = 3732;

		public const short FlowerBoyHat = 3733;

		public const short FlowerBoyShirt = 3734;

		public const short FlowerBoyPants = 3735;

		public const short SillyBalloonPink = 3736;

		public const short SillyBalloonPurple = 3737;

		public const short SillyBalloonGreen = 3738;

		public const short SillyStreamerBlue = 3739;

		public const short SillyStreamerGreen = 3740;

		public const short SillyStreamerPink = 3741;

		public const short SillyBalloonMachine = 3742;

		public const short SillyBalloonTiedPink = 3743;

		public const short SillyBalloonTiedPurple = 3744;

		public const short SillyBalloonTiedGreen = 3745;

		public const short Pigronata = 3746;

		public const short PartyMonolith = 3747;

		public const short PartyBundleOfBalloonTile = 3748;

		public const short PartyPresent = 3749;

		public const short SliceOfCake = 3750;

		public const short CogWall = 3751;

		public const short SandFallWall = 3752;

		public const short SnowFallWall = 3753;

		public const short SandFallBlock = 3754;

		public const short SnowFallBlock = 3755;

		public const short SnowCloudBlock = 3756;

		public const short PedguinHat = 3757;

		public const short PedguinShirt = 3758;

		public const short PedguinPants = 3759;

		public const short SillyBalloonPinkWall = 3760;

		public const short SillyBalloonPurpleWall = 3761;

		public const short SillyBalloonGreenWall = 3762;

		public const short AviatorSunglasses = 3763;

		public const short BluePhasesaber = 3764;

		public const short RedPhasesaber = 3765;

		public const short GreenPhasesaber = 3766;

		public const short PurplePhasesaber = 3767;

		public const short WhitePhasesaber = 3768;

		public const short YellowPhasesaber = 3769;

		public const short DjinnsCurse = 3770;

		public const short AncientHorn = 3771;

		public const short AntlionClaw = 3772;

		public const short AncientArmorHat = 3773;

		public const short AncientArmorShirt = 3774;

		public const short AncientArmorPants = 3775;

		public const short AncientBattleArmorHat = 3776;

		public const short AncientBattleArmorShirt = 3777;

		public const short AncientBattleArmorPants = 3778;

		public const short SpiritFlame = 3779;

		public const short SandElementalBanner = 3780;

		public const short PocketMirror = 3781;

		public const short MagicSandDropper = 3782;

		public const short AncientBattleArmorMaterial = 3783;

		public const short LamiaPants = 3784;

		public const short LamiaShirt = 3785;

		public const short LamiaHat = 3786;

		public const short SkyFracture = 3787;

		public const short OnyxBlaster = 3788;

		public const short SandsharkBanner = 3789;

		public const short SandsharkCorruptBanner = 3790;

		public const short SandsharkCrimsonBanner = 3791;

		public const short SandsharkHallowedBanner = 3792;

		public const short TumbleweedBanner = 3793;

		public const short AncientCloth = 3794;

		public const short DjinnLamp = 3795;

		public const short MusicBoxSandstorm = 3796;

		public const short ApprenticeHat = 3797;

		public const short ApprenticeRobe = 3798;

		public const short ApprenticeTrousers = 3799;

		public const short SquireGreatHelm = 3800;

		public const short SquirePlating = 3801;

		public const short SquireGreaves = 3802;

		public const short HuntressWig = 3803;

		public const short HuntressJerkin = 3804;

		public const short HuntressPants = 3805;

		public const short MonkBrows = 3806;

		public const short MonkShirt = 3807;

		public const short MonkPants = 3808;

		public const short ApprenticeScarf = 3809;

		public const short SquireShield = 3810;

		public const short HuntressBuckler = 3811;

		public const short MonkBelt = 3812;

		public const short DefendersForge = 3813;

		public const short WarTable = 3814;

		public const short WarTableBanner = 3815;

		public const short DD2ElderCrystalStand = 3816;

		public const short DefenderMedal = 3817;

		public const short DD2FlameburstTowerT1Popper = 3818;

		public const short DD2FlameburstTowerT2Popper = 3819;

		public const short DD2FlameburstTowerT3Popper = 3820;

		public const short AleThrowingGlove = 3821;

		public const short DD2EnergyCrystal = 3822;

		public const short DD2SquireDemonSword = 3823;

		public const short DD2BallistraTowerT1Popper = 3824;

		public const short DD2BallistraTowerT2Popper = 3825;

		public const short DD2BallistraTowerT3Popper = 3826;

		public const short DD2SquireBetsySword = 3827;

		public const short DD2ElderCrystal = 3828;

		public const short DD2LightningAuraT1Popper = 3829;

		public const short DD2LightningAuraT2Popper = 3830;

		public const short DD2LightningAuraT3Popper = 3831;

		public const short DD2ExplosiveTrapT1Popper = 3832;

		public const short DD2ExplosiveTrapT2Popper = 3833;

		public const short DD2ExplosiveTrapT3Popper = 3834;

		public const short MonkStaffT1 = 3835;

		public const short MonkStaffT2 = 3836;

		public const short DD2GoblinBomberBanner = 3837;

		public const short DD2GoblinBanner = 3838;

		public const short DD2SkeletonBanner = 3839;

		public const short DD2DrakinBanner = 3840;

		public const short DD2KoboldFlyerBanner = 3841;

		public const short DD2KoboldBanner = 3842;

		public const short DD2WitherBeastBanner = 3843;

		public const short DD2WyvernBanner = 3844;

		public const short DD2JavelinThrowerBanner = 3845;

		public const short DD2LightningBugBanner = 3846;

		public const short OgreMask = 3847;

		public const short GoblinMask = 3848;

		public const short GoblinBomberCap = 3849;

		public const short EtherianJavelin = 3850;

		public const short KoboldDynamiteBackpack = 3851;

		public const short BookStaff = 3852;

		public const short BoringBow = 3853;

		public const short DD2PhoenixBow = 3854;

		public const short DD2PetGato = 3855;

		public const short DD2PetGhost = 3856;

		public const short DD2PetDragon = 3857;

		public const short MonkStaffT3 = 3858;

		public const short DD2BetsyBow = 3859;

		public const short BossBagBetsy = 3860;

		public const short BossBagOgre = 3861;

		public const short BossBagDarkMage = 3862;

		public const short BossMaskBetsy = 3863;

		public const short BossMaskDarkMage = 3864;

		public const short BossMaskOgre = 3865;

		public const short BossTrophyBetsy = 3866;

		public const short BossTrophyDarkmage = 3867;

		public const short BossTrophyOgre = 3868;

		public const short MusicBoxDD2 = 3869;

		public const short ApprenticeStaffT3 = 3870;

		public const short SquireAltHead = 3871;

		public const short SquireAltShirt = 3872;

		public const short SquireAltPants = 3873;

		public const short ApprenticeAltHead = 3874;

		public const short ApprenticeAltShirt = 3875;

		public const short ApprenticeAltPants = 3876;

		public const short HuntressAltHead = 3877;

		public const short HuntressAltShirt = 3878;

		public const short HuntressAltPants = 3879;

		public const short MonkAltHead = 3880;

		public const short MonkAltShirt = 3881;

		public const short MonkAltPants = 3882;

		public const short BetsyWings = 3883;

		public const short CrystalChest = 3884;

		public const short GoldenChest = 3885;

		public const short Fake_CrystalChest = 3886;

		public const short Fake_GoldenChest = 3887;

		public const short CrystalDoor = 3888;

		public const short CrystalChair = 3889;

		public const short CrystalCandle = 3890;

		public const short CrystalLantern = 3891;

		public const short CrystalLamp = 3892;

		public const short CrystalCandelabra = 3893;

		public const short CrystalChandelier = 3894;

		public const short CrystalBathtub = 3895;

		public const short CrystalSink = 3896;

		public const short CrystalBed = 3897;

		public const short CrystalClock = 3898;

		public const short SkywareClock2 = 3899;

		public const short DungeonClockBlue = 3900;

		public const short DungeonClockGreen = 3901;

		public const short DungeonClockPink = 3902;

		public const short CrystalPlatform = 3903;

		public const short GoldenPlatform = 3904;

		public const short DynastyPlatform = 3905;

		public const short LihzahrdPlatform = 3906;

		public const short FleshPlatform = 3907;

		public const short FrozenPlatform = 3908;

		public const short CrystalWorkbench = 3909;

		public const short GoldenWorkbench = 3910;

		public const short CrystalDresser = 3911;

		public const short DynastyDresser = 3912;

		public const short FrozenDresser = 3913;

		public const short LivingWoodDresser = 3914;

		public const short CrystalPiano = 3915;

		public const short DynastyPiano = 3916;

		public const short CrystalBookCase = 3917;

		public const short CrystalSofaHowDoesThatEvenWork = 3918;

		public const short DynastySofa = 3919;

		public const short CrystalTable = 3920;

		public const short ArkhalisHat = 3921;

		public const short ArkhalisShirt = 3922;

		public const short ArkhalisPants = 3923;

		public const short ArkhalisWings = 3924;

		public const short LeinforsHat = 3925;

		public const short LeinforsShirt = 3926;

		public const short LeinforsPants = 3927;

		public const short LeinforsWings = 3928;

		public const short LeinforsAccessory = 3929;

		public const short Celeb2 = 3930;

		public const short SpiderBathtub = 3931;

		public const short SpiderBed = 3932;

		public const short SpiderBookcase = 3933;

		public const short SpiderDresser = 3934;

		public const short SpiderCandelabra = 3935;

		public const short SpiderCandle = 3936;

		public const short SpiderChair = 3937;

		public const short SpiderChandelier = 3938;

		public const short SpiderChest = 3939;

		public const short SpiderClock = 3940;

		public const short SpiderDoor = 3941;

		public const short SpiderLamp = 3942;

		public const short SpiderLantern = 3943;

		public const short SpiderPiano = 3944;

		public const short SpiderPlatform = 3945;

		public const short SpiderSinkSpiderSinkDoesWhateverASpiderSinkDoes = 3946;

		public const short SpiderSofa = 3947;

		public const short SpiderTable = 3948;

		public const short SpiderWorkbench = 3949;

		public const short Fake_SpiderChest = 3950;

		public const short IronBrick = 3951;

		public const short IronBrickWall = 3952;

		public const short LeadBrick = 3953;

		public const short LeadBrickWall = 3954;

		public const short LesionBlock = 3955;

		public const short LesionBlockWall = 3956;

		public const short LesionPlatform = 3957;

		public const short LesionBathtub = 3958;

		public const short LesionBed = 3959;

		public const short LesionBookcase = 3960;

		public const short LesionCandelabra = 3961;

		public const short LesionCandle = 3962;

		public const short LesionChair = 3963;

		public const short LesionChandelier = 3964;

		public const short LesionChest = 3965;

		public const short LesionClock = 3966;

		public const short LesionDoor = 3967;

		public const short LesionDresser = 3968;

		public const short LesionLamp = 3969;

		public const short LesionLantern = 3970;

		public const short LesionPiano = 3971;

		public const short LesionSink = 3972;

		public const short LesionSofa = 3973;

		public const short LesionTable = 3974;

		public const short LesionWorkbench = 3975;

		public const short Fake_LesionChest = 3976;

		public const short HatRack = 3977;

		public const short ColorOnlyDye = 3978;

		public const short WoodenCrateHard = 3979;

		public const short IronCrateHard = 3980;

		public const short GoldenCrateHard = 3981;

		public const short CorruptFishingCrateHard = 3982;

		public const short CrimsonFishingCrateHard = 3983;

		public const short DungeonFishingCrateHard = 3984;

		public const short FloatingIslandFishingCrateHard = 3985;

		public const short HallowedFishingCrateHard = 3986;

		public const short JungleFishingCrateHard = 3987;

		public const short DeadMansChest = 3988;

		public const short GolfBall = 3989;

		public const short AmphibianBoots = 3990;

		public const short ArcaneFlower = 3991;

		public const short BerserkerGlove = 3992;

		public const short FairyBoots = 3993;

		public const short FrogFlipper = 3994;

		public const short FrogGear = 3995;

		public const short FrogWebbing = 3996;

		public const short FrozenShield = 3997;

		public const short HeroShield = 3998;

		public const short LavaSkull = 3999;

		public const short MagnetFlower = 4000;

		public const short ManaCloak = 4001;

		public const short MoltenQuiver = 4002;

		public const short MoltenSkullRose = 4003;

		public const short ObsidianSkullRose = 4004;

		public const short ReconScope = 4005;

		public const short StalkersQuiver = 4006;

		public const short StingerNecklace = 4007;

		public const short UltrabrightHelmet = 4008;

		public const short Apple = 4009;

		public const short ApplePieSlice = 4010;

		public const short ApplePie = 4011;

		public const short BananaSplit = 4012;

		public const short BBQRibs = 4013;

		public const short BunnyStew = 4014;

		public const short Burger = 4015;

		public const short ChickenNugget = 4016;

		public const short ChocolateChipCookie = 4017;

		public const short CreamSoda = 4018;

		public const short Escargot = 4019;

		public const short FriedEgg = 4020;

		public const short Fries = 4021;

		public const short GoldenDelight = 4022;

		public const short Grapes = 4023;

		public const short GrilledSquirrel = 4024;

		public const short Hotdog = 4025;

		public const short IceCream = 4026;

		public const short Milkshake = 4027;

		public const short Nachos = 4028;

		public const short Pizza = 4029;

		public const short PotatoChips = 4030;

		public const short RoastedBird = 4031;

		public const short RoastedDuck = 4032;

		public const short SauteedFrogLegs = 4033;

		public const short SeafoodDinner = 4034;

		public const short ShrimpPoBoy = 4035;

		public const short Spaghetti = 4036;

		public const short Steak = 4037;

		public const short MoltenCharm = 4038;

		public const short GolfClubIron = 4039;

		public const short GolfCup = 4040;

		public const short FlowerPacketBlue = 4041;

		public const short FlowerPacketMagenta = 4042;

		public const short FlowerPacketPink = 4043;

		public const short FlowerPacketRed = 4044;

		public const short FlowerPacketYellow = 4045;

		public const short FlowerPacketViolet = 4046;

		public const short FlowerPacketWhite = 4047;

		public const short FlowerPacketTallGrass = 4048;

		public const short LawnMower = 4049;

		public const short CrimstoneBrick = 4050;

		public const short SmoothSandstone = 4051;

		public const short CrimstoneBrickWall = 4052;

		public const short SmoothSandstoneWall = 4053;

		public const short BloodMoonMonolith = 4054;

		public const short SandBoots = 4055;

		public const short AncientChisel = 4056;

		public const short CarbonGuitar = 4057;

		public const short SkeletonBow = 4058;

		public const short FossilPickaxe = 4059;

		public const short SuperStarCannon = 4060;

		public const short ThunderSpear = 4061;

		public const short ThunderStaff = 4062;

		public const short DrumSet = 4063;

		public const short PicnicTable = 4064;

		public const short PicnicTableWithCloth = 4065;

		public const short DesertMinecart = 4066;

		public const short FishMinecart = 4067;

		public const short FairyCritterPink = 4068;

		public const short FairyCritterGreen = 4069;

		public const short FairyCritterBlue = 4070;

		public const short JunoniaShell = 4071;

		public const short LightningWhelkShell = 4072;

		public const short TulipShell = 4073;

		public const short PinWheel = 4074;

		public const short WeatherVane = 4075;

		public const short VoidVault = 4076;

		public const short MusicBoxOceanAlt = 4077;

		public const short MusicBoxSlimeRain = 4078;

		public const short MusicBoxSpaceAlt = 4079;

		public const short MusicBoxTownDay = 4080;

		public const short MusicBoxTownNight = 4081;

		public const short MusicBoxWindyDay = 4082;

		public const short GolfCupFlagWhite = 4083;

		public const short GolfCupFlagRed = 4084;

		public const short GolfCupFlagGreen = 4085;

		public const short GolfCupFlagBlue = 4086;

		public const short GolfCupFlagYellow = 4087;

		public const short GolfCupFlagPurple = 4088;

		public const short GolfTee = 4089;

		public const short ShellPileBlock = 4090;

		public const short AntiPortalBlock = 4091;

		public const short GolfClubPutter = 4092;

		public const short GolfClubWedge = 4093;

		public const short GolfClubDriver = 4094;

		public const short GolfWhistle = 4095;

		public const short ToiletEbonyWood = 4096;

		public const short ToiletRichMahogany = 4097;

		public const short ToiletPearlwood = 4098;

		public const short ToiletLivingWood = 4099;

		public const short ToiletCactus = 4100;

		public const short ToiletBone = 4101;

		public const short ToiletFlesh = 4102;

		public const short ToiletMushroom = 4103;

		public const short ToiletSunplate = 4104;

		public const short ToiletShadewood = 4105;

		public const short ToiletLihzhard = 4106;

		public const short ToiletDungeonBlue = 4107;

		public const short ToiletDungeonGreen = 4108;

		public const short ToiletDungeonPink = 4109;

		public const short ToiletObsidian = 4110;

		public const short ToiletFrozen = 4111;

		public const short ToiletGlass = 4112;

		public const short ToiletHoney = 4113;

		public const short ToiletSteampunk = 4114;

		public const short ToiletPumpkin = 4115;

		public const short ToiletSpooky = 4116;

		public const short ToiletDynasty = 4117;

		public const short ToiletPalm = 4118;

		public const short ToiletBoreal = 4119;

		public const short ToiletSlime = 4120;

		public const short ToiletMartian = 4121;

		public const short ToiletGranite = 4122;

		public const short ToiletMarble = 4123;

		public const short ToiletCrystal = 4124;

		public const short ToiletSpider = 4125;

		public const short ToiletLesion = 4126;

		public const short ToiletDiamond = 4127;

		public const short MaidHead = 4128;

		public const short MaidShirt = 4129;

		public const short MaidPants = 4130;

		public const short VoidLens = 4131;

		public const short MaidHead2 = 4132;

		public const short MaidShirt2 = 4133;

		public const short MaidPants2 = 4134;

		public const short GolfHat = 4135;

		public const short GolfShirt = 4136;

		public const short GolfPants = 4137;

		public const short GolfVisor = 4138;

		public const short SpiderBlock = 4139;

		public const short SpiderWall = 4140;

		public const short ToiletMeteor = 4141;

		public const short LesionStation = 4142;

		public const short ManaCloakStar = 4143;

		public const short Terragrim = 4144;

		public const short SolarBathtub = 4145;

		public const short SolarBed = 4146;

		public const short SolarBookcase = 4147;

		public const short SolarDresser = 4148;

		public const short SolarCandelabra = 4149;

		public const short SolarCandle = 4150;

		public const short SolarChair = 4151;

		public const short SolarChandelier = 4152;

		public const short SolarChest = 4153;

		public const short SolarClock = 4154;

		public const short SolarDoor = 4155;

		public const short SolarLamp = 4156;

		public const short SolarLantern = 4157;

		public const short SolarPiano = 4158;

		public const short SolarPlatform = 4159;

		public const short SolarSink = 4160;

		public const short SolarSofa = 4161;

		public const short SolarTable = 4162;

		public const short SolarWorkbench = 4163;

		public const short Fake_SolarChest = 4164;

		public const short SolarToilet = 4165;

		public const short VortexBathtub = 4166;

		public const short VortexBed = 4167;

		public const short VortexBookcase = 4168;

		public const short VortexDresser = 4169;

		public const short VortexCandelabra = 4170;

		public const short VortexCandle = 4171;

		public const short VortexChair = 4172;

		public const short VortexChandelier = 4173;

		public const short VortexChest = 4174;

		public const short VortexClock = 4175;

		public const short VortexDoor = 4176;

		public const short VortexLamp = 4177;

		public const short VortexLantern = 4178;

		public const short VortexPiano = 4179;

		public const short VortexPlatform = 4180;

		public const short VortexSink = 4181;

		public const short VortexSofa = 4182;

		public const short VortexTable = 4183;

		public const short VortexWorkbench = 4184;

		public const short Fake_VortexChest = 4185;

		public const short VortexToilet = 4186;

		public const short NebulaBathtub = 4187;

		public const short NebulaBed = 4188;

		public const short NebulaBookcase = 4189;

		public const short NebulaDresser = 4190;

		public const short NebulaCandelabra = 4191;

		public const short NebulaCandle = 4192;

		public const short NebulaChair = 4193;

		public const short NebulaChandelier = 4194;

		public const short NebulaChest = 4195;

		public const short NebulaClock = 4196;

		public const short NebulaDoor = 4197;

		public const short NebulaLamp = 4198;

		public const short NebulaLantern = 4199;

		public const short NebulaPiano = 4200;

		public const short NebulaPlatform = 4201;

		public const short NebulaSink = 4202;

		public const short NebulaSofa = 4203;

		public const short NebulaTable = 4204;

		public const short NebulaWorkbench = 4205;

		public const short Fake_NebulaChest = 4206;

		public const short NebulaToilet = 4207;

		public const short StardustBathtub = 4208;

		public const short StardustBed = 4209;

		public const short StardustBookcase = 4210;

		public const short StardustDresser = 4211;

		public const short StardustCandelabra = 4212;

		public const short StardustCandle = 4213;

		public const short StardustChair = 4214;

		public const short StardustChandelier = 4215;

		public const short StardustChest = 4216;

		public const short StardustClock = 4217;

		public const short StardustDoor = 4218;

		public const short StardustLamp = 4219;

		public const short StardustLantern = 4220;

		public const short StardustPiano = 4221;

		public const short StardustPlatform = 4222;

		public const short StardustSink = 4223;

		public const short StardustSofa = 4224;

		public const short StardustTable = 4225;

		public const short StardustWorkbench = 4226;

		public const short Fake_StardustChest = 4227;

		public const short StardustToilet = 4228;

		public const short SolarBrick = 4229;

		public const short VortexBrick = 4230;

		public const short NebulaBrick = 4231;

		public const short StardustBrick = 4232;

		public const short SolarBrickWall = 4233;

		public const short VortexBrickWall = 4234;

		public const short NebulaBrickWall = 4235;

		public const short StardustBrickWall = 4236;

		public const short MusicBoxDayRemix = 4237;

		public const short CrackedBlueBrick = 4238;

		public const short CrackedGreenBrick = 4239;

		public const short CrackedPinkBrick = 4240;

		public const short FlowerPacketWild = 4241;

		public const short GolfBallDyedBlack = 4242;

		public const short GolfBallDyedBlue = 4243;

		public const short GolfBallDyedBrown = 4244;

		public const short GolfBallDyedCyan = 4245;

		public const short GolfBallDyedGreen = 4246;

		public const short GolfBallDyedLimeGreen = 4247;

		public const short GolfBallDyedOrange = 4248;

		public const short GolfBallDyedPink = 4249;

		public const short GolfBallDyedPurple = 4250;

		public const short GolfBallDyedRed = 4251;

		public const short GolfBallDyedSkyBlue = 4252;

		public const short GolfBallDyedTeal = 4253;

		public const short GolfBallDyedViolet = 4254;

		public const short GolfBallDyedYellow = 4255;

		public const short AmberRobe = 4256;

		public const short AmberHook = 4257;

		public const short OrangePhaseblade = 4258;

		public const short OrangePhasesaber = 4259;

		public const short OrangeStainedGlass = 4260;

		public const short OrangePressurePlate = 4261;

		public const short MysticCoilSnake = 4262;

		public const short MagicConch = 4263;

		public const short GolfCart = 4264;

		public const short GolfChest = 4265;

		public const short Fake_GolfChest = 4266;

		public const short DesertChest = 4267;

		public const short Fake_DesertChest = 4268;

		public const short SanguineStaff = 4269;

		public const short SharpTears = 4270;

		public const short BloodMoonStarter = 4271;

		public const short DripplerFlail = 4272;

		public const short VampireFrogStaff = 4273;

		public const short GoldGoldfish = 4274;

		public const short GoldGoldfishBowl = 4275;

		public const short CatBast = 4276;

		public const short GoldStarryGlassBlock = 4277;

		public const short BlueStarryGlassBlock = 4278;

		public const short GoldStarryGlassWall = 4279;

		public const short BlueStarryGlassWall = 4280;

		public const short BabyBirdStaff = 4281;

		public const short Apricot = 4282;

		public const short Banana = 4283;

		public const short BlackCurrant = 4284;

		public const short BloodOrange = 4285;

		public const short Cherry = 4286;

		public const short Coconut = 4287;

		public const short Dragonfruit = 4288;

		public const short Elderberry = 4289;

		public const short Grapefruit = 4290;

		public const short Lemon = 4291;

		public const short Mango = 4292;

		public const short Peach = 4293;

		public const short Pineapple = 4294;

		public const short Plum = 4295;

		public const short Rambutan = 4296;

		public const short Starfruit = 4297;

		public const short SandstoneBathtub = 4298;

		public const short SandstoneBed = 4299;

		public const short SandstoneBookcase = 4300;

		public const short SandstoneDresser = 4301;

		public const short SandstoneCandelabra = 4302;

		public const short SandstoneCandle = 4303;

		public const short SandstoneChair = 4304;

		public const short SandstoneChandelier = 4305;

		public const short SandstoneClock = 4306;

		public const short SandstoneDoor = 4307;

		public const short SandstoneLamp = 4308;

		public const short SandstoneLantern = 4309;

		public const short SandstonePiano = 4310;

		public const short SandstonePlatform = 4311;

		public const short SandstoneSink = 4312;

		public const short SandstoneSofa = 4313;

		public const short SandstoneTable = 4314;

		public const short SandstoneWorkbench = 4315;

		public const short SandstoneToilet = 4316;

		public const short BloodHamaxe = 4317;

		public const short VoidMonolith = 4318;

		public const short ArrowSign = 4319;

		public const short PaintedArrowSign = 4320;

		public const short GameMasterShirt = 4321;

		public const short GameMasterPants = 4322;

		public const short StarPrincessCrown = 4323;

		public const short StarPrincessDress = 4324;

		public const short BloodFishingRod = 4325;

		public const short FoodPlatter = 4326;

		public const short BlackDragonflyJar = 4327;

		public const short BlueDragonflyJar = 4328;

		public const short GreenDragonflyJar = 4329;

		public const short OrangeDragonflyJar = 4330;

		public const short RedDragonflyJar = 4331;

		public const short YellowDragonflyJar = 4332;

		public const short GoldDragonflyJar = 4333;

		public const short BlackDragonfly = 4334;

		public const short BlueDragonfly = 4335;

		public const short GreenDragonfly = 4336;

		public const short OrangeDragonfly = 4337;

		public const short RedDragonfly = 4338;

		public const short YellowDragonfly = 4339;

		public const short GoldDragonfly = 4340;

		public const short PortableStool = 4341;

		public const short DragonflyStatue = 4342;

		public const short PaperAirplaneA = 4343;

		public const short PaperAirplaneB = 4344;

		public const short CanOfWorms = 4345;

		public const short EncumberingStone = 4346;

		public const short ZapinatorGray = 4347;

		public const short ZapinatorOrange = 4348;

		public const short GreenMoss = 4349;

		public const short BrownMoss = 4350;

		public const short RedMoss = 4351;

		public const short BlueMoss = 4352;

		public const short PurpleMoss = 4353;

		public const short LavaMoss = 4354;

		public const short BoulderStatue = 4355;

		public const short MusicBoxTitleAlt = 4356;

		public const short MusicBoxStorm = 4357;

		public const short MusicBoxGraveyard = 4358;

		public const short Seagull = 4359;

		public const short SeagullStatue = 4360;

		public const short LadyBug = 4361;

		public const short GoldLadyBug = 4362;

		public const short Maggot = 4363;

		public const short MaggotCage = 4364;

		public const short CelestialWand = 4365;

		public const short EucaluptusSap = 4366;

		public const short KiteBlue = 4367;

		public const short KiteBlueAndYellow = 4368;

		public const short KiteRed = 4369;

		public const short KiteRedAndYellow = 4370;

		public const short KiteYellow = 4371;

		public const short IvyGuitar = 4372;

		public const short Pupfish = 4373;

		public const short Grebe = 4374;

		public const short Rat = 4375;

		public const short RatCage = 4376;

		public const short KryptonMoss = 4377;

		public const short XenonMoss = 4378;

		public const short KiteWyvern = 4379;

		public const short LadybugCage = 4380;

		public const short BloodRainBow = 4381;

		public const short CombatBook = 4382;

		public const short DesertTorch = 4383;

		public const short CoralTorch = 4384;

		public const short CorruptTorch = 4385;

		public const short CrimsonTorch = 4386;

		public const short HallowedTorch = 4387;

		public const short JungleTorch = 4388;

		public const short ArgonMoss = 4389;

		public const short RollingCactus = 4390;

		public const short ThinIce = 4391;

		public const short EchoBlock = 4392;

		public const short ScarabFish = 4393;

		public const short ScorpioFish = 4394;

		public const short Owl = 4395;

		public const short OwlCage = 4396;

		public const short OwlStatue = 4397;

		public const short PupfishBowl = 4398;

		public const short GoldLadybugCage = 4399;

		public const short Geode = 4400;

		public const short Flounder = 4401;

		public const short RockLobster = 4402;

		public const short LobsterTail = 4403;

		public const short FloatingTube = 4404;

		public const short FrozenCrate = 4405;

		public const short FrozenCrateHard = 4406;

		public const short OasisCrate = 4407;

		public const short OasisCrateHard = 4408;

		public const short SpectreGoggles = 4409;

		public const short Oyster = 4410;

		public const short ShuckedOyster = 4411;

		public const short WhitePearl = 4412;

		public const short BlackPearl = 4413;

		public const short PinkPearl = 4414;

		public const short StoneDoor = 4415;

		public const short StonePlatform = 4416;

		public const short OasisFountain = 4417;

		public const short WaterStrider = 4418;

		public const short GoldWaterStrider = 4419;

		public const short LawnFlamingo = 4420;

		public const short MusicBoxUndergroundJungle = 4421;

		public const short Grate = 4422;

		public const short ScarabBomb = 4423;

		public const short WroughtIronFence = 4424;

		public const short SharkBait = 4425;

		public const short BeeMinecart = 4426;

		public const short LadybugMinecart = 4427;

		public const short PigronMinecart = 4428;

		public const short SunflowerMinecart = 4429;

		public const short PottedForestCedar = 4430;

		public const short PottedJungleCedar = 4431;

		public const short PottedHallowCedar = 4432;

		public const short PottedForestTree = 4433;

		public const short PottedJungleTree = 4434;

		public const short PottedHallowTree = 4435;

		public const short PottedForestPalm = 4436;

		public const short PottedJunglePalm = 4437;

		public const short PottedHallowPalm = 4438;

		public const short PottedForestBamboo = 4439;

		public const short PottedJungleBamboo = 4440;

		public const short PottedHallowBamboo = 4441;

		public const short ScarabFishingRod = 4442;

		public const short HellMinecart = 4443;

		public const short WitchBroom = 4444;

		public const short ClusterRocketI = 4445;

		public const short ClusterRocketII = 4446;

		public const short WetRocket = 4447;

		public const short LavaRocket = 4448;

		public const short HoneyRocket = 4449;

		public const short ShroomMinecart = 4450;

		public const short AmethystMinecart = 4451;

		public const short TopazMinecart = 4452;

		public const short SapphireMinecart = 4453;

		public const short EmeraldMinecart = 4454;

		public const short RubyMinecart = 4455;

		public const short DiamondMinecart = 4456;

		public const short MiniNukeI = 4457;

		public const short MiniNukeII = 4458;

		public const short DryRocket = 4459;

		public const short SandcastleBucket = 4460;

		public const short TurtleCage = 4461;

		public const short TurtleJungleCage = 4462;

		public const short Gladius = 4463;

		public const short Turtle = 4464;

		public const short TurtleJungle = 4465;

		public const short TurtleStatue = 4466;

		public const short AmberMinecart = 4467;

		public const short BeetleMinecart = 4468;

		public const short MeowmereMinecart = 4469;

		public const short PartyMinecart = 4470;

		public const short PirateMinecart = 4471;

		public const short SteampunkMinecart = 4472;

		public const short GrebeCage = 4473;

		public const short SeagullCage = 4474;

		public const short WaterStriderCage = 4475;

		public const short GoldWaterStriderCage = 4476;

		public const short LuckPotionLesser = 4477;

		public const short LuckPotion = 4478;

		public const short LuckPotionGreater = 4479;

		public const short Seahorse = 4480;

		public const short SeahorseCage = 4481;

		public const short GoldSeahorse = 4482;

		public const short GoldSeahorseCage = 4483;

		public const short TimerOneHalfSecond = 4484;

		public const short TimerOneFourthSecond = 4485;

		public const short EbonstoneEcho = 4486;

		public const short MudWallEcho = 4487;

		public const short PearlstoneEcho = 4488;

		public const short SnowWallEcho = 4489;

		public const short AmethystEcho = 4490;

		public const short TopazEcho = 4491;

		public const short SapphireEcho = 4492;

		public const short EmeraldEcho = 4493;

		public const short RubyEcho = 4494;

		public const short DiamondEcho = 4495;

		public const short Cave1Echo = 4496;

		public const short Cave2Echo = 4497;

		public const short Cave3Echo = 4498;

		public const short Cave4Echo = 4499;

		public const short Cave5Echo = 4500;

		public const short Cave6Echo = 4501;

		public const short Cave7Echo = 4502;

		public const short SpiderEcho = 4503;

		public const short CorruptGrassEcho = 4504;

		public const short HallowedGrassEcho = 4505;

		public const short IceEcho = 4506;

		public const short ObsidianBackEcho = 4507;

		public const short CrimsonGrassEcho = 4508;

		public const short CrimstoneEcho = 4509;

		public const short CaveWall1Echo = 4510;

		public const short CaveWall2Echo = 4511;

		public const short Cave8Echo = 4512;

		public const short Corruption1Echo = 4513;

		public const short Corruption2Echo = 4514;

		public const short Corruption3Echo = 4515;

		public const short Corruption4Echo = 4516;

		public const short Crimson1Echo = 4517;

		public const short Crimson2Echo = 4518;

		public const short Crimson3Echo = 4519;

		public const short Crimson4Echo = 4520;

		public const short Dirt1Echo = 4521;

		public const short Dirt2Echo = 4522;

		public const short Dirt3Echo = 4523;

		public const short Dirt4Echo = 4524;

		public const short Hallow1Echo = 4525;

		public const short Hallow2Echo = 4526;

		public const short Hallow3Echo = 4527;

		public const short Hallow4Echo = 4528;

		public const short Jungle1Echo = 4529;

		public const short Jungle2Echo = 4530;

		public const short Jungle3Echo = 4531;

		public const short Jungle4Echo = 4532;

		public const short Lava1Echo = 4533;

		public const short Lava2Echo = 4534;

		public const short Lava3Echo = 4535;

		public const short Lava4Echo = 4536;

		public const short Rocks1Echo = 4537;

		public const short Rocks2Echo = 4538;

		public const short Rocks3Echo = 4539;

		public const short Rocks4Echo = 4540;

		public const short TheBrideBanner = 4541;

		public const short ZombieMermanBanner = 4542;

		public const short EyeballFlyingFishBanner = 4543;

		public const short BloodSquidBanner = 4544;

		public const short BloodEelBanner = 4545;

		public const short GoblinSharkBanner = 4546;

		public const short LargeBambooBlock = 4547;

		public const short LargeBambooBlockWall = 4548;

		public const short DemonHorns = 4549;

		public const short BambooLeaf = 4550;

		public const short HellCake = 4551;

		public const short FogMachine = 4552;

		public const short PlasmaLamp = 4553;

		public const short MarbleColumn = 4554;

		public const short ChefHat = 4555;

		public const short ChefShirt = 4556;

		public const short ChefPants = 4557;

		public const short StarHairpin = 4558;

		public const short HeartHairpin = 4559;

		public const short BunnyEars = 4560;

		public const short DevilHorns = 4561;

		public const short Fedora = 4562;

		public const short UnicornHornHat = 4563;

		public const short BambooBlock = 4564;

		public const short BambooBlockWall = 4565;

		public const short BambooBathtub = 4566;

		public const short BambooBed = 4567;

		public const short BambooBookcase = 4568;

		public const short BambooDresser = 4569;

		public const short BambooCandelabra = 4570;

		public const short BambooCandle = 4571;

		public const short BambooChair = 4572;

		public const short BambooChandelier = 4573;

		public const short BambooChest = 4574;

		public const short BambooClock = 4575;

		public const short BambooDoor = 4576;

		public const short BambooLamp = 4577;

		public const short BambooLantern = 4578;

		public const short BambooPiano = 4579;

		public const short BambooPlatform = 4580;

		public const short BambooSink = 4581;

		public const short BambooSofa = 4582;

		public const short BambooTable = 4583;

		public const short BambooWorkbench = 4584;

		public const short Fake_BambooChest = 4585;

		public const short BambooToilet = 4586;

		public const short GolfClubStoneIron = 4587;

		public const short GolfClubRustyPutter = 4588;

		public const short GolfClubBronzeWedge = 4589;

		public const short GolfClubWoodDriver = 4590;

		public const short GolfClubMythrilIron = 4591;

		public const short GolfClubLeadPutter = 4592;

		public const short GolfClubGoldWedge = 4593;

		public const short GolfClubPearlwoodDriver = 4594;

		public const short GolfClubTitaniumIron = 4595;

		public const short GolfClubShroomitePutter = 4596;

		public const short GolfClubDiamondWedge = 4597;

		public const short GolfClubChlorophyteDriver = 4598;

		public const short GolfTrophyBronze = 4599;

		public const short GolfTrophySilver = 4600;

		public const short GolfTrophyGold = 4601;

		public const short BloodNautilusBanner = 4602;

		public const short BirdieRattle = 4603;

		public const short ExoticEasternChewToy = 4604;

		public const short BedazzledNectar = 4605;

		public const short MusicBoxJungleNight = 4606;

		public const short StormTigerStaff = 4607;

		public const short ChumBucket = 4608;

		public const short GardenGnome = 4609;

		public const short KiteBoneSerpent = 4610;

		public const short KiteWorldFeeder = 4611;

		public const short KiteBunny = 4612;

		public const short KitePigron = 4613;

		public const short AppleJuice = 4614;

		public const short GrapeJuice = 4615;

		public const short Lemonade = 4616;

		public const short BananaDaiquiri = 4617;

		public const short PeachSangria = 4618;

		public const short PinaColada = 4619;

		public const short TropicalSmoothie = 4620;

		public const short BloodyMoscato = 4621;

		public const short SmoothieofDarkness = 4622;

		public const short PrismaticPunch = 4623;

		public const short FruitJuice = 4624;

		public const short FruitSalad = 4625;

		public const short AndrewSphinx = 4626;

		public const short WatchfulAntlion = 4627;

		public const short BurningSpirit = 4628;

		public const short JawsOfDeath = 4629;

		public const short TheSandsOfSlime = 4630;

		public const short SnakesIHateSnakes = 4631;

		public const short LifeAboveTheSand = 4632;

		public const short Oasis = 4633;

		public const short PrehistoryPreserved = 4634;

		public const short AncientTablet = 4635;

		public const short Uluru = 4636;

		public const short VisitingThePyramids = 4637;

		public const short BandageBoy = 4638;

		public const short DivineEye = 4639;

		public const short AmethystStoneBlock = 4640;

		public const short TopazStoneBlock = 4641;

		public const short SapphireStoneBlock = 4642;

		public const short EmeraldStoneBlock = 4643;

		public const short RubyStoneBlock = 4644;

		public const short DiamondStoneBlock = 4645;

		public const short AmberStoneBlock = 4646;

		public const short AmberStoneWallEcho = 4647;

		public const short KiteManEater = 4648;

		public const short KiteJellyfishBlue = 4649;

		public const short KiteJellyfishPink = 4650;

		public const short KiteShark = 4651;

		public const short SuperHeroMask = 4652;

		public const short SuperHeroCostume = 4653;

		public const short SuperHeroTights = 4654;

		public const short PinkFairyJar = 4655;

		public const short GreenFairyJar = 4656;

		public const short BlueFairyJar = 4657;

		public const short GolfPainting1 = 4658;

		public const short GolfPainting2 = 4659;

		public const short GolfPainting3 = 4660;

		public const short GolfPainting4 = 4661;

		public const short FogboundDye = 4662;

		public const short BloodbathDye = 4663;

		public const short PrettyPinkDressSkirt = 4664;

		public const short PrettyPinkDressPants = 4665;

		public const short PrettyPinkRibbon = 4666;

		public const short BambooFence = 4667;

		public const short GlowPaint = 4668;

		public const short KiteSandShark = 4669;

		public const short KiteBunnyCorrupt = 4670;

		public const short KiteBunnyCrimson = 4671;

		public const short BlandWhip = 4672;

		public const short DrumStick = 4673;

		public const short KiteGoldfish = 4674;

		public const short KiteAngryTrapper = 4675;

		public const short KiteKoi = 4676;

		public const short KiteCrawltipede = 4677;

		public const short SwordWhip = 4678;

		public const short MaceWhip = 4679;

		public const short ScytheWhip = 4680;

		public const short KiteSpectrum = 4681;

		public const short ReleaseDoves = 4682;

		public const short KiteWanderingEye = 4683;

		public const short KiteUnicorn = 4684;

		public const short UndertakerHat = 4685;

		public const short UndertakerCoat = 4686;

		public const short DandelionBanner = 4687;

		public const short GnomeBanner = 4688;

		public const short DesertCampfire = 4689;

		public const short CoralCampfire = 4690;

		public const short CorruptCampfire = 4691;

		public const short CrimsonCampfire = 4692;

		public const short HallowedCampfire = 4693;

		public const short JungleCampfire = 4694;

		public const short SoulBottleLight = 4695;

		public const short SoulBottleNight = 4696;

		public const short SoulBottleFlight = 4697;

		public const short SoulBottleSight = 4698;

		public const short SoulBottleMight = 4699;

		public const short SoulBottleFright = 4700;

		public const short MudBud = 4701;

		public const short ReleaseLantern = 4702;

		public const short QuadBarrelShotgun = 4703;

		public const short FuneralHat = 4704;

		public const short FuneralCoat = 4705;

		public const short FuneralPants = 4706;

		public const short TragicUmbrella = 4707;

		public const short VictorianGothHat = 4708;

		public const short VictorianGothDress = 4709;

		public const short TatteredWoodSign = 4710;

		public const short GravediggerShovel = 4711;

		public const short DungeonDesertChest = 4712;

		public const short Fake_DungeonDesertChest = 4713;

		public const short DungeonDesertKey = 4714;

		public const short SparkleGuitar = 4715;

		public const short MolluskWhistle = 4716;

		public const short BorealBeam = 4717;

		public const short RichMahoganyBeam = 4718;

		public const short GraniteColumn = 4719;

		public const short SandstoneColumn = 4720;

		public const short MushroomBeam = 4721;

		public const short FirstFractal = 4722;

		public const short Nevermore = 4723;

		public const short Reborn = 4724;

		public const short Graveyard = 4725;

		public const short GhostManifestation = 4726;

		public const short WickedUndead = 4727;

		public const short BloodyGoblet = 4728;

		public const short StillLife = 4729;

		public const short GhostarsWings = 4730;

		public const short TerraToilet = 4731;

		public const short GhostarSkullPin = 4732;

		public const short GhostarShirt = 4733;

		public const short GhostarPants = 4734;

		public const short BallOfFuseWire = 4735;

		public const short FullMoonSqueakyToy = 4736;

		public const short OrnateShadowKey = 4737;

		public const short DrManFlyMask = 4738;

		public const short DrManFlyLabCoat = 4739;

		public const short ButcherMask = 4740;

		public const short ButcherApron = 4741;

		public const short ButcherPants = 4742;

		public const short Football = 4743;

		public const short HunterCloak = 4744;

		public const short CoffinMinecart = 4745;

		public const short SafemanWings = 4746;

		public const short SafemanSunHair = 4747;

		public const short SafemanSunDress = 4748;

		public const short SafemanDressLeggings = 4749;

		public const short FoodBarbarianWings = 4750;

		public const short FoodBarbarianHelm = 4751;

		public const short FoodBarbarianArmor = 4752;

		public const short FoodBarbarianGreaves = 4753;

		public const short GroxTheGreatWings = 4754;

		public const short GroxTheGreatHelm = 4755;

		public const short GroxTheGreatArmor = 4756;

		public const short GroxTheGreatGreaves = 4757;

		public const short Smolstar = 4758;

		public const short SquirrelHook = 4759;

		public const short BouncingShield = 4760;

		public const short RockGolemHead = 4761;

		public const short CritterShampoo = 4762;

		public const short DiggingMoleMinecart = 4763;

		public const short Shroomerang = 4764;

		public const short TreeGlobe = 4765;

		public const short WorldGlobe = 4766;

		public const short DontHurtCrittersBook = 4767;

		public const short DogEars = 4768;

		public const short DogTail = 4769;

		public const short FoxEars = 4770;

		public const short FoxTail = 4771;

		public const short LizardEars = 4772;

		public const short LizardTail = 4773;

		public const short PandaEars = 4774;

		public const short BunnyTail = 4775;

		public const short FairyGlowstick = 4776;

		public const short LightningCarrot = 4777;

		public const short HallowBossDye = 4778;

		public const short MushroomHat = 4779;

		public const short MushroomVest = 4780;

		public const short MushroomPants = 4781;

		public const short FairyQueenBossBag = 4782;

		public const short FairyQueenTrophy = 4783;

		public const short FairyQueenMask = 4784;

		public const short PaintedHorseSaddle = 4785;

		public const short MajesticHorseSaddle = 4786;

		public const short DarkHorseSaddle = 4787;

		public const short JoustingLance = 4788;

		public const short ShadowJoustingLance = 4789;

		public const short HallowJoustingLance = 4790;

		public const short PogoStick = 4791;

		public const short PirateShipMountItem = 4792;

		public const short SpookyWoodMountItem = 4793;

		public const short SantankMountItem = 4794;

		public const short WallOfFleshGoatMountItem = 4795;

		public const short DarkMageBookMountItem = 4796;

		public const short KingSlimePetItem = 4797;

		public const short EyeOfCthulhuPetItem = 4798;

		public const short EaterOfWorldsPetItem = 4799;

		public const short BrainOfCthulhuPetItem = 4800;

		public const short SkeletronPetItem = 4801;

		public const short QueenBeePetItem = 4802;

		public const short DestroyerPetItem = 4803;

		public const short TwinsPetItem = 4804;

		public const short SkeletronPrimePetItem = 4805;

		public const short PlanteraPetItem = 4806;

		public const short GolemPetItem = 4807;

		public const short DukeFishronPetItem = 4808;

		public const short LunaticCultistPetItem = 4809;

		public const short MoonLordPetItem = 4810;

		public const short FairyQueenPetItem = 4811;

		public const short PumpkingPetItem = 4812;

		public const short EverscreamPetItem = 4813;

		public const short IceQueenPetItem = 4814;

		public const short MartianPetItem = 4815;

		public const short DD2OgrePetItem = 4816;

		public const short DD2BetsyPetItem = 4817;

		public const short CombatWrench = 4818;

		public const short DemonConch = 4819;

		public const short BottomlessLavaBucket = 4820;

		public const short FireproofBugNet = 4821;

		public const short FlameWakerBoots = 4822;

		public const short RainbowWings = 4823;

		public const short WetBomb = 4824;

		public const short LavaBomb = 4825;

		public const short HoneyBomb = 4826;

		public const short DryBomb = 4827;

		public const short SuperheatedBlood = 4828;

		public const short LicenseCat = 4829;

		public const short LicenseDog = 4830;

		public const short GemSquirrelAmethyst = 4831;

		public const short GemSquirrelTopaz = 4832;

		public const short GemSquirrelSapphire = 4833;

		public const short GemSquirrelEmerald = 4834;

		public const short GemSquirrelRuby = 4835;

		public const short GemSquirrelDiamond = 4836;

		public const short GemSquirrelAmber = 4837;

		public const short GemBunnyAmethyst = 4838;

		public const short GemBunnyTopaz = 4839;

		public const short GemBunnySapphire = 4840;

		public const short GemBunnyEmerald = 4841;

		public const short GemBunnyRuby = 4842;

		public const short GemBunnyDiamond = 4843;

		public const short GemBunnyAmber = 4844;

		public const short HellButterfly = 4845;

		public const short HellButterflyJar = 4846;

		public const short Lavafly = 4847;

		public const short LavaflyinaBottle = 4848;

		public const short MagmaSnail = 4849;

		public const short MagmaSnailCage = 4850;

		public const short GemTreeTopazSeed = 4851;

		public const short GemTreeAmethystSeed = 4852;

		public const short GemTreeSapphireSeed = 4853;

		public const short GemTreeEmeraldSeed = 4854;

		public const short GemTreeRubySeed = 4855;

		public const short GemTreeDiamondSeed = 4856;

		public const short GemTreeAmberSeed = 4857;

		public const short PotSuspended = 4858;

		public const short PotSuspendedDaybloom = 4859;

		public const short PotSuspendedMoonglow = 4860;

		public const short PotSuspendedWaterleaf = 4861;

		public const short PotSuspendedShiverthorn = 4862;

		public const short PotSuspendedBlinkroot = 4863;

		public const short PotSuspendedDeathweedCorrupt = 4864;

		public const short PotSuspendedDeathweedCrimson = 4865;

		public const short PotSuspendedFireblossom = 4866;

		public const short BrazierSuspended = 4867;

		public const short VolcanoSmall = 4868;

		public const short VolcanoLarge = 4869;

		public const short PotionOfReturn = 4870;

		public const short VanityTreeSakuraSeed = 4871;

		public const short LavaAbsorbantSponge = 4872;

		public const short HallowedHood = 4873;

		public const short HellfireTreads = 4874;

		public const short TeleportationPylonJungle = 4875;

		public const short TeleportationPylonPurity = 4876;

		public const short LavaCrate = 4877;

		public const short LavaCrateHard = 4878;

		public const short ObsidianLockbox = 4879;

		public const short LavaFishbowl = 4880;

		public const short LavaFishingHook = 4881;

		public const short AmethystBunnyCage = 4882;

		public const short TopazBunnyCage = 4883;

		public const short SapphireBunnyCage = 4884;

		public const short EmeraldBunnyCage = 4885;

		public const short RubyBunnyCage = 4886;

		public const short DiamondBunnyCage = 4887;

		public const short AmberBunnyCage = 4888;

		public const short AmethystSquirrelCage = 4889;

		public const short TopazSquirrelCage = 4890;

		public const short SapphireSquirrelCage = 4891;

		public const short EmeraldSquirrelCage = 4892;

		public const short RubySquirrelCage = 4893;

		public const short DiamondSquirrelCage = 4894;

		public const short AmberSquirrelCage = 4895;

		public const short AncientHallowedMask = 4896;

		public const short AncientHallowedHelmet = 4897;

		public const short AncientHallowedHeadgear = 4898;

		public const short AncientHallowedHood = 4899;

		public const short AncientHallowedPlateMail = 4900;

		public const short AncientHallowedGreaves = 4901;

		public const short PottedLavaPlantPalm = 4902;

		public const short PottedLavaPlantBush = 4903;

		public const short PottedLavaPlantBramble = 4904;

		public const short PottedLavaPlantBulb = 4905;

		public const short PottedLavaPlantTendrils = 4906;

		public const short VanityTreeYellowWillowSeed = 4907;

		public const short DirtBomb = 4908;

		public const short DirtStickyBomb = 4909;

		public const short LicenseBunny = 4910;

		public const short CoolWhip = 4911;

		public const short FireWhip = 4912;

		public const short ThornWhip = 4913;

		public const short RainbowWhip = 4914;

		public const short TungstenBullet = 4915;

		public const short TeleportationPylonHallow = 4916;

		public const short TeleportationPylonUnderground = 4917;

		public const short TeleportationPylonOcean = 4918;

		public const short TeleportationPylonDesert = 4919;

		public const short TeleportationPylonSnow = 4920;

		public const short TeleportationPylonMushroom = 4921;

		public const short CavernFountain = 4922;

		public const short PiercingStarlight = 4923;

		public const short EyeofCthulhuMasterTrophy = 4924;

		public const short EaterofWorldsMasterTrophy = 4925;

		public const short BrainofCthulhuMasterTrophy = 4926;

		public const short SkeletronMasterTrophy = 4927;

		public const short QueenBeeMasterTrophy = 4928;

		public const short KingSlimeMasterTrophy = 4929;

		public const short WallofFleshMasterTrophy = 4930;

		public const short TwinsMasterTrophy = 4931;

		public const short DestroyerMasterTrophy = 4932;

		public const short SkeletronPrimeMasterTrophy = 4933;

		public const short PlanteraMasterTrophy = 4934;

		public const short GolemMasterTrophy = 4935;

		public const short DukeFishronMasterTrophy = 4936;

		public const short LunaticCultistMasterTrophy = 4937;

		public const short MoonLordMasterTrophy = 4938;

		public const short UFOMasterTrophy = 4939;

		public const short FlyingDutchmanMasterTrophy = 4940;

		public const short MourningWoodMasterTrophy = 4941;

		public const short PumpkingMasterTrophy = 4942;

		public const short IceQueenMasterTrophy = 4943;

		public const short EverscreamMasterTrophy = 4944;

		public const short SantankMasterTrophy = 4945;

		public const short DarkMageMasterTrophy = 4946;

		public const short OgreMasterTrophy = 4947;

		public const short BetsyMasterTrophy = 4948;

		public const short FairyQueenMasterTrophy = 4949;

		public const short QueenSlimeMasterTrophy = 4950;

		public const short TeleportationPylonVictory = 4951;

		public const short FairyQueenMagicItem = 4952;

		public const short FairyQueenRangedItem = 4953;

		public const short LongRainbowTrailWings = 4954;

		public const short RabbitOrder = 4955;

		public const short Zenith = 4956;

		public const short QueenSlimeBossBag = 4957;

		public const short QueenSlimeTrophy = 4958;

		public const short QueenSlimeMask = 4959;

		public const short QueenSlimePetItem = 4960;

		public const short EmpressButterfly = 4961;

		public const short AccentSlab = 4962;

		public const short TruffleWormCage = 4963;

		public const short EmpressButterflyJar = 4964;

		public const short RockGolemBanner = 4965;

		public const short BloodMummyBanner = 4966;

		public const short SporeSkeletonBanner = 4967;

		public const short SporeBatBanner = 4968;

		public const short LarvaeAntlionBanner = 4969;

		public const short CrimsonBunnyBanner = 4970;

		public const short CrimsonGoldfishBanner = 4971;

		public const short CrimsonPenguinBanner = 4972;

		public const short BigMimicCorruptionBanner = 4973;

		public const short BigMimicCrimsonBanner = 4974;

		public const short BigMimicHallowBanner = 4975;

		public const short MossHornetBanner = 4976;

		public const short WanderingEyeBanner = 4977;

		public const short CreativeWings = 4978;

		public const short MusicBoxQueenSlime = 4979;

		public const short QueenSlimeHook = 4980;

		public const short QueenSlimeMountSaddle = 4981;

		public const short CrystalNinjaHelmet = 4982;

		public const short CrystalNinjaChestplate = 4983;

		public const short CrystalNinjaLeggings = 4984;

		public const short MusicBoxEmpressOfLight = 4985;

		public const short GelBalloon = 4986;

		public const short VolatileGelatin = 4987;

		public const short QueenSlimeCrystal = 4988;

		public const short EmpressFlightBooster = 4989;

		public const short MusicBoxDukeFishron = 4990;

		public const short MusicBoxMorningRain = 4991;

		public const short MusicBoxConsoleTitle = 4992;

		public const short ChippysCouch = 4993;

		public const short GraduationCapBlue = 4994;

		public const short GraduationCapMaroon = 4995;

		public const short GraduationCapBlack = 4996;

		public const short GraduationGownBlue = 4997;

		public const short GraduationGownMaroon = 4998;

		public const short GraduationGownBlack = 4999;

		public const short TerrasparkBoots = 5000;

		public const short MoonLordLegs = 5001;

		public const short OceanCrate = 5002;

		public const short OceanCrateHard = 5003;

		public const short BadgersHat = 5004;

		public const short EmpressBlade = 5005;

		public const short MusicBoxUndergroundDesert = 5006;

		public const short DeadMansSweater = 5007;

		public const short TeaKettle = 5008;

		public const short Teacup = 5009;

		public const short TreasureMagnet = 5010;

		public const short Mace = 5011;

		public const short FlamingMace = 5012;

		public const short SleepingIcon = 5013;

		public const short MusicBoxOWRain = 5014;

		public const short MusicBoxOWDay = 5015;

		public const short MusicBoxOWNight = 5016;

		public const short MusicBoxOWUnderground = 5017;

		public const short MusicBoxOWDesert = 5018;

		public const short MusicBoxOWOcean = 5019;

		public const short MusicBoxOWMushroom = 5020;

		public const short MusicBoxOWDungeon = 5021;

		public const short MusicBoxOWSpace = 5022;

		public const short MusicBoxOWUnderworld = 5023;

		public const short MusicBoxOWSnow = 5024;

		public const short MusicBoxOWCorruption = 5025;

		public const short MusicBoxOWUndergroundCorruption = 5026;

		public const short MusicBoxOWCrimson = 5027;

		public const short MusicBoxOWUndergroundCrimson = 5028;

		public const short MusicBoxOWUndergroundSnow = 5029;

		public const short MusicBoxOWUndergroundHallow = 5030;

		public const short MusicBoxOWBloodMoon = 5031;

		public const short MusicBoxOWBoss2 = 5032;

		public const short MusicBoxOWBoss1 = 5033;

		public const short MusicBoxOWInvasion = 5034;

		public const short MusicBoxOWTowers = 5035;

		public const short MusicBoxOWMoonLord = 5036;

		public const short MusicBoxOWPlantera = 5037;

		public const short MusicBoxOWJungle = 5038;

		public const short MusicBoxOWWallOfFlesh = 5039;

		public const short MusicBoxOWHallow = 5040;

		public const short MilkCarton = 5041;

		public const short CoffeeCup = 5042;

		public const short TorchGodsFavor = 5043;

		public const short MusicBoxCredits = 5044;

		public const short PlaguebringerHelmet = 5045;

		public const short PlaguebringerChestplate = 5046;

		public const short PlaguebringerGreaves = 5047;

		public const short RoninHat = 5048;

		public const short RoninShirt = 5049;

		public const short RoninPants = 5050;

		public const short TimelessTravelerHood = 5051;

		public const short TimelessTravelerRobe = 5052;

		public const short TimelessTravelerBottom = 5053;

		public const short FloretProtectorHelmet = 5054;

		public const short FloretProtectorChestplate = 5055;

		public const short FloretProtectorLegs = 5056;

		public const short CapricornMask = 5057;

		public const short CapricornChestplate = 5058;

		public const short CapricornLegs = 5059;

		public const short CapricornTail = 5060;

		public const short TVHeadMask = 5061;

		public const short TVHeadSuit = 5062;

		public const short TVHeadPants = 5063;

		public const short LavaproofTackleBag = 5064;

		public const short PrincessWeapon = 5065;

		public const short BeeHive = 5066;

		public const short AntlionEggs = 5067;

		public const short FlinxFurCoat = 5068;

		public const short FlinxStaff = 5069;

		public const short FlinxFur = 5070;

		public const short RoyalTiara = 5071;

		public const short RoyalDressTop = 5072;

		public const short RoyalDressBottom = 5073;

		public const short BoneWhip = 5074;

		public const short RainbowCursor = 5075;

		public const short RoyalScepter = 5076;

		public const short GlassSlipper = 5077;

		public const short PrinceUniform = 5078;

		public const short PrincePants = 5079;

		public const short PrinceCape = 5080;

		public const short PottedCrystalPlantFern = 5081;

		public const short PottedCrystalPlantSpiral = 5082;

		public const short PottedCrystalPlantTeardrop = 5083;

		public const short PottedCrystalPlantTree = 5084;

		public const short Princess64 = 5085;

		public const short PaintingOfALass = 5086;

		public const short DarkSideHallow = 5087;

		public const short BerniePetItem = 5088;

		public const short GlommerPetItem = 5089;

		public const short DeerclopsPetItem = 5090;

		public const short PigPetItem = 5091;

		public const short MonsterLasagna = 5092;

		public const short FroggleBunwich = 5093;

		public const short TentacleSpike = 5094;

		public const short LucyTheAxe = 5095;

		public const short HamBat = 5096;

		public const short BatBat = 5097;

		public const short ChesterPetItem = 5098;

		public const short GarlandHat = 5099;

		public const short BoneHelm = 5100;

		public const short Eyebrella = 5101;

		public const short WilsonShirt = 5102;

		public const short WilsonPants = 5103;

		public const short WilsonBeardShort = 5104;

		public const short WilsonBeardLong = 5105;

		public const short WilsonBeardMagnificent = 5106;

		public const short Magiluminescence = 5107;

		public const short DeerclopsTrophy = 5108;

		public const short DeerclopsMask = 5109;

		public const short DeerclopsMasterTrophy = 5110;

		public const short DeerclopsBossBag = 5111;

		public const short MusicBoxDeerclops = 5112;

		public const short DontStarveShaderItem = 5113;

		public const short AbigailsFlower = 5114;

		public const short WillowShirt = 5115;

		public const short WillowSkirt = 5116;

		public const short PewMaticHorn = 5117;

		public const short WeatherPain = 5118;

		public const short HoundiusShootius = 5119;

		public const short DeerThing = 5120;

		public const short PaintingWilson = 5121;

		public const short PaintingWillow = 5122;

		public const short PaintingWendy = 5123;

		public const short PaintingWolfgang = 5124;

		public const short FartMinecart = 5125;

		public const short HandOfCreation = 5126;

		public const short VioletMoss = 5127;

		public const short RainbowMoss = 5128;

		public const short Flymeal = 5129;

		public const short WolfMountItem = 5130;

		public const short ResplendentDessert = 5131;

		public const short Stinkbug = 5132;

		public const short StinkbugCage = 5133;

		public const short Clentaminator2 = 5134;

		public const short VenomDartTrap = 5135;

		public const short VulkelfEar = 5136;

		public const short StinkbugHousingBlocker = 5137;

		public const short StinkbugHousingBlockerEcho = 5138;

		public const short FishingBobber = 5139;

		public const short FishingBobberGlowingStar = 5140;

		public const short FishingBobberGlowingLava = 5141;

		public const short FishingBobberGlowingKrypton = 5142;

		public const short FishingBobberGlowingXenon = 5143;

		public const short FishingBobberGlowingArgon = 5144;

		public const short FishingBobberGlowingViolet = 5145;

		public const short FishingBobberGlowingRainbow = 5146;

		public const short WandofFrosting = 5147;

		public const short CoralBathtub = 5148;

		public const short CoralBed = 5149;

		public const short CoralBookcase = 5150;

		public const short CoralDresser = 5151;

		public const short CoralCandelabra = 5152;

		public const short CoralCandle = 5153;

		public const short CoralChair = 5154;

		public const short CoralChandelier = 5155;

		public const short CoralChest = 5156;

		public const short CoralClock = 5157;

		public const short CoralDoor = 5158;

		public const short CoralLamp = 5159;

		public const short CoralLantern = 5160;

		public const short CoralPiano = 5161;

		public const short CoralPlatform = 5162;

		public const short CoralSink = 5163;

		public const short CoralSofa = 5164;

		public const short CoralTable = 5165;

		public const short CoralWorkbench = 5166;

		public const short Fake_CoralChest = 5167;

		public const short CoralToilet = 5168;

		public const short BalloonBathtub = 5169;

		public const short BalloonBed = 5170;

		public const short BalloonBookcase = 5171;

		public const short BalloonDresser = 5172;

		public const short BalloonCandelabra = 5173;

		public const short BalloonCandle = 5174;

		public const short BalloonChair = 5175;

		public const short BalloonChandelier = 5176;

		public const short BalloonChest = 5177;

		public const short BalloonClock = 5178;

		public const short BalloonDoor = 5179;

		public const short BalloonLamp = 5180;

		public const short BalloonLantern = 5181;

		public const short BalloonPiano = 5182;

		public const short BalloonPlatform = 5183;

		public const short BalloonSink = 5184;

		public const short BalloonSofa = 5185;

		public const short BalloonTable = 5186;

		public const short BalloonWorkbench = 5187;

		public const short Fake_BalloonChest = 5188;

		public const short BalloonToilet = 5189;

		public const short AshWoodBathtub = 5190;

		public const short AshWoodBed = 5191;

		public const short AshWoodBookcase = 5192;

		public const short AshWoodDresser = 5193;

		public const short AshWoodCandelabra = 5194;

		public const short AshWoodCandle = 5195;

		public const short AshWoodChair = 5196;

		public const short AshWoodChandelier = 5197;

		public const short AshWoodChest = 5198;

		public const short AshWoodClock = 5199;

		public const short AshWoodDoor = 5200;

		public const short AshWoodLamp = 5201;

		public const short AshWoodLantern = 5202;

		public const short AshWoodPiano = 5203;

		public const short AshWoodPlatform = 5204;

		public const short AshWoodSink = 5205;

		public const short AshWoodSofa = 5206;

		public const short AshWoodTable = 5207;

		public const short AshWoodWorkbench = 5208;

		public const short Fake_AshWoodChest = 5209;

		public const short AshWoodToilet = 5210;

		public const short BiomeSightPotion = 5211;

		public const short ScarletMacaw = 5212;

		public const short ScarletMacawCage = 5213;

		public const short AshGrassSeeds = 5214;

		public const short AshWood = 5215;

		public const short AshWoodWall = 5216;

		public const short AshWoodFence = 5217;

		public const short Outcast = 5218;

		public const short FairyGuides = 5219;

		public const short AHorribleNightforAlchemy = 5220;

		public const short MorningHunt = 5221;

		public const short SuspiciouslySparkly = 5222;

		public const short Requiem = 5223;

		public const short CatSword = 5224;

		public const short KargohsSummon = 5225;

		public const short HighPitch = 5226;

		public const short AMachineforTerrarians = 5227;

		public const short TerraBladeChronicles = 5228;

		public const short BennyWarhol = 5229;

		public const short LizardKing = 5230;

		public const short MySon = 5231;

		public const short Duality = 5232;

		public const short ParsecPals = 5233;

		public const short RemnantsofDevotion = 5234;

		public const short NotSoLostInParadise = 5235;

		public const short OcularResonance = 5236;

		public const short WingsofEvil = 5237;

		public const short Constellation = 5238;

		public const short Eyezorhead = 5239;

		public const short DreadoftheRedSea = 5240;

		public const short DoNotEattheVileMushroom = 5241;

		public const short YuumaTheBlueTiger = 5242;

		public const short MoonmanandCompany = 5243;

		public const short SunshineofIsrapony = 5244;

		public const short Purity = 5245;

		public const short SufficientlyAdvanced = 5246;

		public const short StrangeGrowth = 5247;

		public const short HappyLittleTree = 5248;

		public const short StrangeDeadFellows = 5249;

		public const short Secrets = 5250;

		public const short Thunderbolt = 5251;

		public const short Crustography = 5252;

		public const short TheWerewolf = 5253;

		public const short BlessingfromTheHeavens = 5254;

		public const short LoveisintheTrashSlot = 5255;

		public const short Fangs = 5256;

		public const short HailtotheKing = 5257;

		public const short SeeTheWorldForWhatItIs = 5258;

		public const short WhatLurksBelow = 5259;

		public const short ThisIsGettingOutOfHand = 5260;

		public const short Buddies = 5261;

		public const short MidnightSun = 5262;

		public const short CouchGag = 5263;

		public const short SilentFish = 5264;

		public const short TheDuke = 5265;

		public const short RoyalRomance = 5266;

		public const short Bioluminescence = 5267;

		public const short Wildflowers = 5268;

		public const short VikingVoyage = 5269;

		public const short Bifrost = 5270;

		public const short Heartlands = 5271;

		public const short ForestTroll = 5272;

		public const short AuroraBorealis = 5273;

		public const short LadyOfTheLake = 5274;

		public const short JojaCola = 5275;

		public const short JunimoPetItem = 5276;

		public const short SpicyPepper = 5277;

		public const short Pomegranate = 5278;

		public const short AshWoodHelmet = 5279;

		public const short AshWoodBreastplate = 5280;

		public const short AshWoodGreaves = 5281;

		public const short AshWoodBow = 5282;

		public const short AshWoodHammer = 5283;

		public const short AshWoodSword = 5284;

		public const short MoonGlobe = 5285;

		public const short RepairedLifeCrystal = 5286;

		public const short RepairedManaCrystal = 5287;

		public const short TerraFartMinecart = 5288;

		public const short MinecartPowerup = 5289;

		public const short JimsCap = 5290;

		public const short EchoWall = 5291;

		public const short EchoPlatform = 5292;

		public const short MushroomTorch = 5293;

		public const short HiveFive = 5294;

		public const short AcornAxe = 5295;

		public const short ChlorophyteExtractinator = 5296;

		public const short BlueEgg = 5297;

		public const short Trimarang = 5298;

		public const short MushroomCampfire = 5299;

		public const short BlueMacaw = 5300;

		public const short BlueMacawCage = 5301;

		public const short BottomlessHoneyBucket = 5302;

		public const short HoneyAbsorbantSponge = 5303;

		public const short UltraAbsorbantSponge = 5304;

		public const short GoblorcEar = 5305;

		public const short ReefBlock = 5306;

		public const short ReefWall = 5307;

		public const short PlacePainting = 5308;

		public const short DontHurtNatureBook = 5309;

		public const short PrincessStyle = 5310;

		public const short Toucan = 5311;

		public const short YellowCockatiel = 5312;

		public const short GrayCockatiel = 5313;

		public const short ToucanCage = 5314;

		public const short YellowCockatielCage = 5315;

		public const short GrayCockatielCage = 5316;

		public const short MacawStatue = 5317;

		public const short ToucanStatue = 5318;

		public const short CockatielStatue = 5319;

		public const short PlaceableHealingPotion = 5320;

		public const short PlaceableManaPotion = 5321;

		public const short ShadowCandle = 5322;

		public const short DontHurtComboBook = 5323;

		public const short RubblemakerSmall = 5324;

		public const short ClosedVoidBag = 5325;

		public const short ArtisanLoaf = 5326;

		public const short TNTBarrel = 5327;

		public const short ChestLock = 5328;

		public const short RubblemakerMedium = 5329;

		public const short RubblemakerLarge = 5330;

		public const short HorseshoeBundle = 5331;

		public const short SpiffoPlush = 5332;

		public const short GlowTulip = 5333;

		public const short MechdusaSummon = 5334;

		public const short RodOfHarmony = 5335;

		public const short CombatBookVolumeTwo = 5336;

		public const short AegisCrystal = 5337;

		public const short AegisFruit = 5338;

		public const short ArcaneCrystal = 5339;

		public const short GalaxyPearl = 5340;

		public const short GummyWorm = 5341;

		public const short Ambrosia = 5342;

		public const short PeddlersSatchel = 5343;

		public const short EchoCoating = 5344;

		public const short EchoMonolith = 5345;

		public const short GasTrap = 5346;

		public const short ShimmerMonolith = 5347;

		public const short ShimmerArrow = 5348;

		public const short ShimmerBlock = 5349;

		public const short Shimmerfly = 5350;

		public const short ShimmerflyinaBottle = 5351;

		public const short ShimmerSlimeBanner = 5352;

		public const short ShimmerTorch = 5353;

		public const short ReflectiveShades = 5354;

		public const short ShimmerCloak = 5355;

		public const short UsedGasTrap = 5356;

		public const short ShimmerCampfire = 5357;

		public const short Shellphone = 5358;

		public const short ShellphoneSpawn = 5359;

		public const short ShellphoneOcean = 5360;

		public const short ShellphoneHell = 5361;

		public const short MusicBoxShimmer = 5362;

		public const short SpiderWallUnsafe = 5363;

		public const short BottomlessShimmerBucket = 5364;

		public const short BlueBrickWallUnsafe = 5365;

		public const short BlueSlabWallUnsafe = 5366;

		public const short BlueTiledWallUnsafe = 5367;

		public const short PinkBrickWallUnsafe = 5368;

		public const short PinkSlabWallUnsafe = 5369;

		public const short PinkTiledWallUnsafe = 5370;

		public const short GreenBrickWallUnsafe = 5371;

		public const short GreenSlabWallUnsafe = 5372;

		public const short GreenTiledWallUnsafe = 5373;

		public const short SandstoneWallUnsafe = 5374;

		public const short HardenedSandWallUnsafe = 5375;

		public const short LihzahrdWallUnsafe = 5376;

		public const short SpelunkerFlare = 5377;

		public const short CursedFlare = 5378;

		public const short RainbowFlare = 5379;

		public const short ShimmerFlare = 5380;

		public const short Moondial = 5381;

		public const short WaffleIron = 5382;

		public const short BouncyBoulder = 5383;

		public const short LifeCrystalBoulder = 5384;

		public const short DizzyHat = 5385;

		public const short LincolnsHoodie = 5386;

		public const short LincolnsPants = 5387;

		public const short SunOrnament = 5388;

		public const short HoplitePizza = 5389;

		public const short LincolnsHood = 5390;

		public const short UncumberingStone = 5391;

		public const short SandSolution = 5392;

		public const short SnowSolution = 5393;

		public const short DirtSolution = 5394;

		public const short PoopBlock = 5395;

		public const short PoopWall = 5396;

		public const short ShimmerWall = 5397;

		public const short ShimmerBrick = 5398;

		public const short ShimmerBrickWall = 5399;

		public const short DirtiestBlock = 5400;

		public const short LunarRustBrick = 5401;

		public const short DarkCelestialBrick = 5402;

		public const short AstraBrick = 5403;

		public const short CosmicEmberBrick = 5404;

		public const short CryocoreBrick = 5405;

		public const short MercuryBrick = 5406;

		public const short StarRoyaleBrick = 5407;

		public const short HeavenforgeBrick = 5408;

		public const short LunarRustBrickWall = 5409;

		public const short DarkCelestialBrickWall = 5410;

		public const short AstraBrickWall = 5411;

		public const short CosmicEmberBrickWall = 5412;

		public const short CryocoreBrickWall = 5413;

		public const short MercuryBrickWall = 5414;

		public const short StarRoyaleBrickWall = 5415;

		public const short HeavenforgeBrickWall = 5416;

		public const short AncientBlueDungeonBrick = 5417;

		public const short AncientBlueDungeonBrickWall = 5418;

		public const short AncientGreenDungeonBrick = 5419;

		public const short AncientGreenDungeonBrickWall = 5420;

		public const short AncientPinkDungeonBrick = 5421;

		public const short AncientPinkDungeonBrickWall = 5422;

		public const short AncientGoldBrick = 5423;

		public const short AncientGoldBrickWall = 5424;

		public const short AncientSilverBrick = 5425;

		public const short AncientSilverBrickWall = 5426;

		public const short AncientCopperBrick = 5427;

		public const short AncientCopperBrickWall = 5428;

		public const short AncientCobaltBrick = 5429;

		public const short AncientCobaltBrickWall = 5430;

		public const short AncientMythrilBrick = 5431;

		public const short AncientMythrilBrickWall = 5432;

		public const short AncientObsidianBrick = 5433;

		public const short AncientObsidianBrickWall = 5434;

		public const short AncientHellstoneBrick = 5435;

		public const short AncientHellstoneBrickWall = 5436;

		public const short ShellphoneDummy = 5437;

		public const short Fertilizer = 5438;

		public const short LavaMossBlock = 5439;

		public const short ArgonMossBlock = 5440;

		public const short KryptonMossBlock = 5441;

		public const short XenonMossBlock = 5442;

		public const short VioletMossBlock = 5443;

		public const short RainbowMossBlock = 5444;

		public const short LavaMossBlockWall = 5445;

		public const short ArgonMossBlockWall = 5446;

		public const short KryptonMossBlockWall = 5447;

		public const short XenonMossBlockWall = 5448;

		public const short VioletMossBlockWall = 5449;

		public const short RainbowMossBlockWall = 5450;

		public const short JimsDrone = 5451;

		public const short JimsDroneVisor = 5452;

		public const short Count = 5453;

		private static Dictionary<string, short> GenerateLegacyItemDictionary()
		{
			return new Dictionary<string, short>
			{
				{ "Iron Pickaxe", 1 },
				{ "Dirt Block", 2 },
				{ "Stone Block", 3 },
				{ "Iron Broadsword", 4 },
				{ "Mushroom", 5 },
				{ "Iron Shortsword", 6 },
				{ "Iron Hammer", 7 },
				{ "Torch", 8 },
				{ "Wood", 9 },
				{ "Iron Axe", 10 },
				{ "Iron Ore", 11 },
				{ "Copper Ore", 12 },
				{ "Gold Ore", 13 },
				{ "Silver Ore", 14 },
				{ "Copper Watch", 15 },
				{ "Silver Watch", 16 },
				{ "Gold Watch", 17 },
				{ "Depth Meter", 18 },
				{ "Gold Bar", 19 },
				{ "Copper Bar", 20 },
				{ "Silver Bar", 21 },
				{ "Iron Bar", 22 },
				{ "Gel", 23 },
				{ "Wooden Sword", 24 },
				{ "Wooden Door", 25 },
				{ "Stone Wall", 26 },
				{ "Acorn", 27 },
				{ "Lesser Healing Potion", 28 },
				{ "Life Crystal", 29 },
				{ "Dirt Wall", 30 },
				{ "Bottle", 31 },
				{ "Wooden Table", 32 },
				{ "Furnace", 33 },
				{ "Wooden Chair", 34 },
				{ "Iron Anvil", 35 },
				{ "Work Bench", 36 },
				{ "Goggles", 37 },
				{ "Lens", 38 },
				{ "Wooden Bow", 39 },
				{ "Wooden Arrow", 40 },
				{ "Flaming Arrow", 41 },
				{ "Shuriken", 42 },
				{ "Suspicious Looking Eye", 43 },
				{ "Demon Bow", 44 },
				{ "War Axe of the Night", 45 },
				{ "Light's Bane", 46 },
				{ "Unholy Arrow", 47 },
				{ "Chest", 48 },
				{ "Band of Regeneration", 49 },
				{ "Magic Mirror", 50 },
				{ "Jester's Arrow", 51 },
				{ "Angel Statue", 52 },
				{ "Cloud in a Bottle", 53 },
				{ "Hermes Boots", 54 },
				{ "Enchanted Boomerang", 55 },
				{ "Demonite Ore", 56 },
				{ "Demonite Bar", 57 },
				{ "Heart", 58 },
				{ "Corrupt Seeds", 59 },
				{ "Vile Mushroom", 60 },
				{ "Ebonstone Block", 61 },
				{ "Grass Seeds", 62 },
				{ "Sunflower", 63 },
				{ "Vilethorn", 64 },
				{ "Starfury", 65 },
				{ "Purification Powder", 66 },
				{ "Vile Powder", 67 },
				{ "Rotten Chunk", 68 },
				{ "Worm Tooth", 69 },
				{ "Worm Food", 70 },
				{ "Copper Coin", 71 },
				{ "Silver Coin", 72 },
				{ "Gold Coin", 73 },
				{ "Platinum Coin", 74 },
				{ "Fallen Star", 75 },
				{ "Copper Greaves", 76 },
				{ "Iron Greaves", 77 },
				{ "Silver Greaves", 78 },
				{ "Gold Greaves", 79 },
				{ "Copper Chainmail", 80 },
				{ "Iron Chainmail", 81 },
				{ "Silver Chainmail", 82 },
				{ "Gold Chainmail", 83 },
				{ "Grappling Hook", 84 },
				{ "Chain", 85 },
				{ "Shadow Scale", 86 },
				{ "Piggy Bank", 87 },
				{ "Mining Helmet", 88 },
				{ "Copper Helmet", 89 },
				{ "Iron Helmet", 90 },
				{ "Silver Helmet", 91 },
				{ "Gold Helmet", 92 },
				{ "Wood Wall", 93 },
				{ "Wood Platform", 94 },
				{ "Flintlock Pistol", 95 },
				{ "Musket", 96 },
				{ "Musket Ball", 97 },
				{ "Minishark", 98 },
				{ "Iron Bow", 99 },
				{ "Shadow Greaves", 100 },
				{ "Shadow Scalemail", 101 },
				{ "Shadow Helmet", 102 },
				{ "Nightmare Pickaxe", 103 },
				{ "The Breaker", 104 },
				{ "Candle", 105 },
				{ "Copper Chandelier", 106 },
				{ "Silver Chandelier", 107 },
				{ "Gold Chandelier", 108 },
				{ "Mana Crystal", 109 },
				{ "Lesser Mana Potion", 110 },
				{ "Band of Starpower", 111 },
				{ "Flower of Fire", 112 },
				{ "Magic Missile", 113 },
				{ "Dirt Rod", 114 },
				{ "Shadow Orb", 115 },
				{ "Meteorite", 116 },
				{ "Meteorite Bar", 117 },
				{ "Hook", 118 },
				{ "Flamarang", 119 },
				{ "Molten Fury", 120 },
				{ "Fiery Greatsword", 121 },
				{ "Molten Pickaxe", 122 },
				{ "Meteor Helmet", 123 },
				{ "Meteor Suit", 124 },
				{ "Meteor Leggings", 125 },
				{ "Bottled Water", 126 },
				{ "Space Gun", 127 },
				{ "Rocket Boots", 128 },
				{ "Gray Brick", 129 },
				{ "Gray Brick Wall", 130 },
				{ "Red Brick", 131 },
				{ "Red Brick Wall", 132 },
				{ "Clay Block", 133 },
				{ "Blue Brick", 134 },
				{ "Blue Brick Wall", 135 },
				{ "Chain Lantern", 136 },
				{ "Green Brick", 137 },
				{ "Green Brick Wall", 138 },
				{ "Pink Brick", 139 },
				{ "Pink Brick Wall", 140 },
				{ "Gold Brick", 141 },
				{ "Gold Brick Wall", 142 },
				{ "Silver Brick", 143 },
				{ "Silver Brick Wall", 144 },
				{ "Copper Brick", 145 },
				{ "Copper Brick Wall", 146 },
				{ "Spike", 147 },
				{ "Water Candle", 148 },
				{ "Book", 149 },
				{ "Cobweb", 150 },
				{ "Necro Helmet", 151 },
				{ "Necro Breastplate", 152 },
				{ "Necro Greaves", 153 },
				{ "Bone", 154 },
				{ "Muramasa", 155 },
				{ "Cobalt Shield", 156 },
				{ "Aqua Scepter", 157 },
				{ "Lucky Horseshoe", 158 },
				{ "Shiny Red Balloon", 159 },
				{ "Harpoon", 160 },
				{ "Spiky Ball", 161 },
				{ "Ball O' Hurt", 162 },
				{ "Blue Moon", 163 },
				{ "Handgun", 164 },
				{ "Water Bolt", 165 },
				{ "Bomb", 166 },
				{ "Dynamite", 167 },
				{ "Grenade", 168 },
				{ "Sand Block", 169 },
				{ "Glass", 170 },
				{ "Sign", 171 },
				{ "Ash Block", 172 },
				{ "Obsidian", 173 },
				{ "Hellstone", 174 },
				{ "Hellstone Bar", 175 },
				{ "Mud Block", 176 },
				{ "Sapphire", 177 },
				{ "Ruby", 178 },
				{ "Emerald", 179 },
				{ "Topaz", 180 },
				{ "Amethyst", 181 },
				{ "Diamond", 182 },
				{ "Glowing Mushroom", 183 },
				{ "Star", 184 },
				{ "Ivy Whip", 185 },
				{ "Breathing Reed", 186 },
				{ "Flipper", 187 },
				{ "Healing Potion", 188 },
				{ "Mana Potion", 189 },
				{ "Blade of Grass", 190 },
				{ "Thorn Chakram", 191 },
				{ "Obsidian Brick", 192 },
				{ "Obsidian Skull", 193 },
				{ "Mushroom Grass Seeds", 194 },
				{ "Jungle Grass Seeds", 195 },
				{ "Wooden Hammer", 196 },
				{ "Star Cannon", 197 },
				{ "Blue Phaseblade", 198 },
				{ "Red Phaseblade", 199 },
				{ "Green Phaseblade", 200 },
				{ "Purple Phaseblade", 201 },
				{ "White Phaseblade", 202 },
				{ "Yellow Phaseblade", 203 },
				{ "Meteor Hamaxe", 204 },
				{ "Empty Bucket", 205 },
				{ "Water Bucket", 206 },
				{ "Lava Bucket", 207 },
				{ "Jungle Rose", 208 },
				{ "Stinger", 209 },
				{ "Vine", 210 },
				{ "Feral Claws", 211 },
				{ "Anklet of the Wind", 212 },
				{ "Staff of Regrowth", 213 },
				{ "Hellstone Brick", 214 },
				{ "Whoopie Cushion", 215 },
				{ "Shackle", 216 },
				{ "Molten Hamaxe", 217 },
				{ "Flamelash", 218 },
				{ "Phoenix Blaster", 219 },
				{ "Sunfury", 220 },
				{ "Hellforge", 221 },
				{ "Clay Pot", 222 },
				{ "Nature's Gift", 223 },
				{ "Bed", 224 },
				{ "Silk", 225 },
				{ "Lesser Restoration Potion", 226 },
				{ "Restoration Potion", 227 },
				{ "Jungle Hat", 228 },
				{ "Jungle Shirt", 229 },
				{ "Jungle Pants", 230 },
				{ "Molten Helmet", 231 },
				{ "Molten Breastplate", 232 },
				{ "Molten Greaves", 233 },
				{ "Meteor Shot", 234 },
				{ "Sticky Bomb", 235 },
				{ "Black Lens", 236 },
				{ "Sunglasses", 237 },
				{ "Wizard Hat", 238 },
				{ "Top Hat", 239 },
				{ "Tuxedo Shirt", 240 },
				{ "Tuxedo Pants", 241 },
				{ "Summer Hat", 242 },
				{ "Bunny Hood", 243 },
				{ "Plumber's Hat", 244 },
				{ "Plumber's Shirt", 245 },
				{ "Plumber's Pants", 246 },
				{ "Hero's Hat", 247 },
				{ "Hero's Shirt", 248 },
				{ "Hero's Pants", 249 },
				{ "Fish Bowl", 250 },
				{ "Archaeologist's Hat", 251 },
				{ "Archaeologist's Jacket", 252 },
				{ "Archaeologist's Pants", 253 },
				{ "Black Thread", 254 },
				{ "Green Thread", 255 },
				{ "Ninja Hood", 256 },
				{ "Ninja Shirt", 257 },
				{ "Ninja Pants", 258 },
				{ "Leather", 259 },
				{ "Red Hat", 260 },
				{ "Goldfish", 261 },
				{ "Robe", 262 },
				{ "Robot Hat", 263 },
				{ "Gold Crown", 264 },
				{ "Hellfire Arrow", 265 },
				{ "Sandgun", 266 },
				{ "Guide Voodoo Doll", 267 },
				{ "Diving Helmet", 268 },
				{ "Familiar Shirt", 269 },
				{ "Familiar Pants", 270 },
				{ "Familiar Wig", 271 },
				{ "Demon Scythe", 272 },
				{ "Night's Edge", 273 },
				{ "Dark Lance", 274 },
				{ "Coral", 275 },
				{ "Cactus", 276 },
				{ "Trident", 277 },
				{ "Silver Bullet", 278 },
				{ "Throwing Knife", 279 },
				{ "Spear", 280 },
				{ "Blowpipe", 281 },
				{ "Glowstick", 282 },
				{ "Seed", 283 },
				{ "Wooden Boomerang", 284 },
				{ "Aglet", 285 },
				{ "Sticky Glowstick", 286 },
				{ "Poisoned Knife", 287 },
				{ "Obsidian Skin Potion", 288 },
				{ "Regeneration Potion", 289 },
				{ "Swiftness Potion", 290 },
				{ "Gills Potion", 291 },
				{ "Ironskin Potion", 292 },
				{ "Mana Regeneration Potion", 293 },
				{ "Magic Power Potion", 294 },
				{ "Featherfall Potion", 295 },
				{ "Spelunker Potion", 296 },
				{ "Invisibility Potion", 297 },
				{ "Shine Potion", 298 },
				{ "Night Owl Potion", 299 },
				{ "Battle Potion", 300 },
				{ "Thorns Potion", 301 },
				{ "Water Walking Potion", 302 },
				{ "Archery Potion", 303 },
				{ "Hunter Potion", 304 },
				{ "Gravitation Potion", 305 },
				{ "Gold Chest", 306 },
				{ "Daybloom Seeds", 307 },
				{ "Moonglow Seeds", 308 },
				{ "Blinkroot Seeds", 309 },
				{ "Deathweed Seeds", 310 },
				{ "Waterleaf Seeds", 311 },
				{ "Fireblossom Seeds", 312 },
				{ "Daybloom", 313 },
				{ "Moonglow", 314 },
				{ "Blinkroot", 315 },
				{ "Deathweed", 316 },
				{ "Waterleaf", 317 },
				{ "Fireblossom", 318 },
				{ "Shark Fin", 319 },
				{ "Feather", 320 },
				{ "Tombstone", 321 },
				{ "Mime Mask", 322 },
				{ "Antlion Mandible", 323 },
				{ "Illegal Gun Parts", 324 },
				{ "The Doctor's Shirt", 325 },
				{ "The Doctor's Pants", 326 },
				{ "Golden Key", 327 },
				{ "Shadow Chest", 328 },
				{ "Shadow Key", 329 },
				{ "Obsidian Brick Wall", 330 },
				{ "Jungle Spores", 331 },
				{ "Loom", 332 },
				{ "Piano", 333 },
				{ "Dresser", 334 },
				{ "Bench", 335 },
				{ "Bathtub", 336 },
				{ "Red Banner", 337 },
				{ "Green Banner", 338 },
				{ "Blue Banner", 339 },
				{ "Yellow Banner", 340 },
				{ "Lamp Post", 341 },
				{ "Tiki Torch", 342 },
				{ "Barrel", 343 },
				{ "Chinese Lantern", 344 },
				{ "Cooking Pot", 345 },
				{ "Safe", 346 },
				{ "Skull Lantern", 347 },
				{ "Trash Can", 348 },
				{ "Candelabra", 349 },
				{ "Pink Vase", 350 },
				{ "Mug", 351 },
				{ "Keg", 352 },
				{ "Ale", 353 },
				{ "Bookcase", 354 },
				{ "Throne", 355 },
				{ "Bowl", 356 },
				{ "Bowl of Soup", 357 },
				{ "Toilet", 358 },
				{ "Grandfather Clock", 359 },
				{ "Armor Statue", 360 },
				{ "Goblin Battle Standard", 361 },
				{ "Tattered Cloth", 362 },
				{ "Sawmill", 363 },
				{ "Cobalt Ore", 364 },
				{ "Mythril Ore", 365 },
				{ "Adamantite Ore", 366 },
				{ "Pwnhammer", 367 },
				{ "Excalibur", 368 },
				{ "Hallowed Seeds", 369 },
				{ "Ebonsand Block", 370 },
				{ "Cobalt Hat", 371 },
				{ "Cobalt Helmet", 372 },
				{ "Cobalt Mask", 373 },
				{ "Cobalt Breastplate", 374 },
				{ "Cobalt Leggings", 375 },
				{ "Mythril Hood", 376 },
				{ "Mythril Helmet", 377 },
				{ "Mythril Hat", 378 },
				{ "Mythril Chainmail", 379 },
				{ "Mythril Greaves", 380 },
				{ "Cobalt Bar", 381 },
				{ "Mythril Bar", 382 },
				{ "Cobalt Chainsaw", 383 },
				{ "Mythril Chainsaw", 384 },
				{ "Cobalt Drill", 385 },
				{ "Mythril Drill", 386 },
				{ "Adamantite Chainsaw", 387 },
				{ "Adamantite Drill", 388 },
				{ "Dao of Pow", 389 },
				{ "Mythril Halberd", 390 },
				{ "Adamantite Bar", 391 },
				{ "Glass Wall", 392 },
				{ "Compass", 393 },
				{ "Diving Gear", 394 },
				{ "GPS", 395 },
				{ "Obsidian Horseshoe", 396 },
				{ "Obsidian Shield", 397 },
				{ "Tinkerer's Workshop", 398 },
				{ "Cloud in a Balloon", 399 },
				{ "Adamantite Headgear", 400 },
				{ "Adamantite Helmet", 401 },
				{ "Adamantite Mask", 402 },
				{ "Adamantite Breastplate", 403 },
				{ "Adamantite Leggings", 404 },
				{ "Spectre Boots", 405 },
				{ "Adamantite Glaive", 406 },
				{ "Toolbelt", 407 },
				{ "Pearlsand Block", 408 },
				{ "Pearlstone Block", 409 },
				{ "Mining Shirt", 410 },
				{ "Mining Pants", 411 },
				{ "Pearlstone Brick", 412 },
				{ "Iridescent Brick", 413 },
				{ "Mudstone Brick", 414 },
				{ "Cobalt Brick", 415 },
				{ "Mythril Brick", 416 },
				{ "Pearlstone Brick Wall", 417 },
				{ "Iridescent Brick Wall", 418 },
				{ "Mudstone Brick Wall", 419 },
				{ "Cobalt Brick Wall", 420 },
				{ "Mythril Brick Wall", 421 },
				{ "Holy Water", 422 },
				{ "Unholy Water", 423 },
				{ "Silt Block", 424 },
				{ "Fairy Bell", 425 },
				{ "Breaker Blade", 426 },
				{ "Blue Torch", 427 },
				{ "Red Torch", 428 },
				{ "Green Torch", 429 },
				{ "Purple Torch", 430 },
				{ "White Torch", 431 },
				{ "Yellow Torch", 432 },
				{ "Demon Torch", 433 },
				{ "Clockwork Assault Rifle", 434 },
				{ "Cobalt Repeater", 435 },
				{ "Mythril Repeater", 436 },
				{ "Dual Hook", 437 },
				{ "Star Statue", 438 },
				{ "Sword Statue", 439 },
				{ "Slime Statue", 440 },
				{ "Goblin Statue", 441 },
				{ "Shield Statue", 442 },
				{ "Bat Statue", 443 },
				{ "Fish Statue", 444 },
				{ "Bunny Statue", 445 },
				{ "Skeleton Statue", 446 },
				{ "Reaper Statue", 447 },
				{ "Woman Statue", 448 },
				{ "Imp Statue", 449 },
				{ "Gargoyle Statue", 450 },
				{ "Gloom Statue", 451 },
				{ "Hornet Statue", 452 },
				{ "Bomb Statue", 453 },
				{ "Crab Statue", 454 },
				{ "Hammer Statue", 455 },
				{ "Potion Statue", 456 },
				{ "Spear Statue", 457 },
				{ "Cross Statue", 458 },
				{ "Jellyfish Statue", 459 },
				{ "Bow Statue", 460 },
				{ "Boomerang Statue", 461 },
				{ "Boot Statue", 462 },
				{ "Chest Statue", 463 },
				{ "Bird Statue", 464 },
				{ "Axe Statue", 465 },
				{ "Corrupt Statue", 466 },
				{ "Tree Statue", 467 },
				{ "Anvil Statue", 468 },
				{ "Pickaxe Statue", 469 },
				{ "Mushroom Statue", 470 },
				{ "Eyeball Statue", 471 },
				{ "Pillar Statue", 472 },
				{ "Heart Statue", 473 },
				{ "Pot Statue", 474 },
				{ "Sunflower Statue", 475 },
				{ "King Statue", 476 },
				{ "Queen Statue", 477 },
				{ "Piranha Statue", 478 },
				{ "Planked Wall", 479 },
				{ "Wooden Beam", 480 },
				{ "Adamantite Repeater", 481 },
				{ "Adamantite Sword", 482 },
				{ "Cobalt Sword", 483 },
				{ "Mythril Sword", 484 },
				{ "Moon Charm", 485 },
				{ "Ruler", 486 },
				{ "Crystal Ball", 487 },
				{ "Disco Ball", 488 },
				{ "Sorcerer Emblem", 489 },
				{ "Warrior Emblem", 490 },
				{ "Ranger Emblem", 491 },
				{ "Demon Wings", 492 },
				{ "Angel Wings", 493 },
				{ "Magical Harp", 494 },
				{ "Rainbow Rod", 495 },
				{ "Ice Rod", 496 },
				{ "Neptune's Shell", 497 },
				{ "Mannequin", 498 },
				{ "Greater Healing Potion", 499 },
				{ "Greater Mana Potion", 500 },
				{ "Pixie Dust", 501 },
				{ "Crystal Shard", 502 },
				{ "Clown Hat", 503 },
				{ "Clown Shirt", 504 },
				{ "Clown Pants", 505 },
				{ "Flamethrower", 506 },
				{ "Bell", 507 },
				{ "Harp", 508 },
				{ "Red Wrench", 509 },
				{ "Wire Cutter", 510 },
				{ "Active Stone Block", 511 },
				{ "Inactive Stone Block", 512 },
				{ "Lever", 513 },
				{ "Laser Rifle", 514 },
				{ "Crystal Bullet", 515 },
				{ "Holy Arrow", 516 },
				{ "Magic Dagger", 517 },
				{ "Crystal Storm", 518 },
				{ "Cursed Flames", 519 },
				{ "Soul of Light", 520 },
				{ "Soul of Night", 521 },
				{ "Cursed Flame", 522 },
				{ "Cursed Torch", 523 },
				{ "Adamantite Forge", 524 },
				{ "Mythril Anvil", 525 },
				{ "Unicorn Horn", 526 },
				{ "Dark Shard", 527 },
				{ "Light Shard", 528 },
				{ "Red Pressure Plate", 529 },
				{ "Wire", 530 },
				{ "Spell Tome", 531 },
				{ "Star Cloak", 532 },
				{ "Megashark", 533 },
				{ "Shotgun", 534 },
				{ "Philosopher's Stone", 535 },
				{ "Titan Glove", 536 },
				{ "Cobalt Naginata", 537 },
				{ "Switch", 538 },
				{ "Dart Trap", 539 },
				{ "Boulder", 540 },
				{ "Green Pressure Plate", 541 },
				{ "Gray Pressure Plate", 542 },
				{ "Brown Pressure Plate", 543 },
				{ "Mechanical Eye", 544 },
				{ "Cursed Arrow", 545 },
				{ "Cursed Bullet", 546 },
				{ "Soul of Fright", 547 },
				{ "Soul of Might", 548 },
				{ "Soul of Sight", 549 },
				{ "Gungnir", 550 },
				{ "Hallowed Plate Mail", 551 },
				{ "Hallowed Greaves", 552 },
				{ "Hallowed Helmet", 553 },
				{ "Cross Necklace", 554 },
				{ "Mana Flower", 555 },
				{ "Mechanical Worm", 556 },
				{ "Mechanical Skull", 557 },
				{ "Hallowed Headgear", 558 },
				{ "Hallowed Mask", 559 },
				{ "Slime Crown", 560 },
				{ "Light Disc", 561 },
				{ "Music Box (Overworld Day)", 562 },
				{ "Music Box (Eerie)", 563 },
				{ "Music Box (Night)", 564 },
				{ "Music Box (Title)", 565 },
				{ "Music Box (Underground)", 566 },
				{ "Music Box (Boss 1)", 567 },
				{ "Music Box (Jungle)", 568 },
				{ "Music Box (Corruption)", 569 },
				{ "Music Box (Underground Corruption)", 570 },
				{ "Music Box (The Hallow)", 571 },
				{ "Music Box (Boss 2)", 572 },
				{ "Music Box (Underground Hallow)", 573 },
				{ "Music Box (Boss 3)", 574 },
				{ "Soul of Flight", 575 },
				{ "Music Box", 576 },
				{ "Demonite Brick", 577 },
				{ "Hallowed Repeater", 578 },
				{ "Drax", 579 },
				{ "Explosives", 580 },
				{ "Inlet Pump", 581 },
				{ "Outlet Pump", 582 },
				{ "1 Second Timer", 583 },
				{ "3 Second Timer", 584 },
				{ "5 Second Timer", 585 },
				{ "Candy Cane Block", 586 },
				{ "Candy Cane Wall", 587 },
				{ "Santa Hat", 588 },
				{ "Santa Shirt", 589 },
				{ "Santa Pants", 590 },
				{ "Green Candy Cane Block", 591 },
				{ "Green Candy Cane Wall", 592 },
				{ "Snow Block", 593 },
				{ "Snow Brick", 594 },
				{ "Snow Brick Wall", 595 },
				{ "Blue Light", 596 },
				{ "Red Light", 597 },
				{ "Green Light", 598 },
				{ "Blue Present", 599 },
				{ "Green Present", 600 },
				{ "Yellow Present", 601 },
				{ "Snow Globe", 602 },
				{ "Carrot", 603 },
				{ "Yellow Phasesaber", 3769 },
				{ "White Phasesaber", 3768 },
				{ "Purple Phasesaber", 3767 },
				{ "Green Phasesaber", 3766 },
				{ "Red Phasesaber", 3765 },
				{ "Blue Phasesaber", 3764 },
				{ "Platinum Bow", 3480 },
				{ "Platinum Hammer", 3481 },
				{ "Platinum Axe", 3482 },
				{ "Platinum Shortsword", 3483 },
				{ "Platinum Broadsword", 3484 },
				{ "Platinum Pickaxe", 3485 },
				{ "Tungsten Bow", 3486 },
				{ "Tungsten Hammer", 3487 },
				{ "Tungsten Axe", 3488 },
				{ "Tungsten Shortsword", 3489 },
				{ "Tungsten Broadsword", 3490 },
				{ "Tungsten Pickaxe", 3491 },
				{ "Lead Bow", 3492 },
				{ "Lead Hammer", 3493 },
				{ "Lead Axe", 3494 },
				{ "Lead Shortsword", 3495 },
				{ "Lead Broadsword", 3496 },
				{ "Lead Pickaxe", 3497 },
				{ "Tin Bow", 3498 },
				{ "Tin Hammer", 3499 },
				{ "Tin Axe", 3500 },
				{ "Tin Shortsword", 3501 },
				{ "Tin Broadsword", 3502 },
				{ "Tin Pickaxe", 3503 },
				{ "Copper Bow", 3504 },
				{ "Copper Hammer", 3505 },
				{ "Copper Axe", 3506 },
				{ "Copper Shortsword", 3507 },
				{ "Copper Broadsword", 3508 },
				{ "Copper Pickaxe", 3509 },
				{ "Silver Bow", 3510 },
				{ "Silver Hammer", 3511 },
				{ "Silver Axe", 3512 },
				{ "Silver Shortsword", 3513 },
				{ "Silver Broadsword", 3514 },
				{ "Silver Pickaxe", 3515 },
				{ "Gold Bow", 3516 },
				{ "Gold Hammer", 3517 },
				{ "Gold Axe", 3518 },
				{ "Gold Shortsword", 3519 },
				{ "Gold Broadsword", 3520 },
				{ "Gold Pickaxe", 3521 }
			};
		}

		public static short FromNetId(short id)
		{
			return id switch
			{
				-24 => 3769, 
				-23 => 3768, 
				-22 => 3767, 
				-21 => 3766, 
				-20 => 3765, 
				-19 => 3764, 
				-48 => 3480, 
				-47 => 3481, 
				-46 => 3482, 
				-45 => 3483, 
				-44 => 3484, 
				-43 => 3485, 
				-42 => 3486, 
				-41 => 3487, 
				-40 => 3488, 
				-39 => 3489, 
				-38 => 3490, 
				-37 => 3491, 
				-36 => 3492, 
				-35 => 3493, 
				-34 => 3494, 
				-33 => 3495, 
				-32 => 3496, 
				-31 => 3497, 
				-30 => 3498, 
				-29 => 3499, 
				-28 => 3500, 
				-27 => 3501, 
				-26 => 3502, 
				-25 => 3503, 
				-18 => 3504, 
				-17 => 3505, 
				-16 => 3506, 
				-15 => 3507, 
				-14 => 3508, 
				-13 => 3509, 
				-12 => 3510, 
				-11 => 3511, 
				-10 => 3512, 
				-9 => 3513, 
				-8 => 3514, 
				-7 => 3515, 
				-6 => 3516, 
				-5 => 3517, 
				-4 => 3518, 
				-3 => 3519, 
				-2 => 3520, 
				-1 => 3521, 
				_ => id, 
			};
		}

		public static short FromLegacyName(string name, int release)
		{
			if (_legacyItemLookup == null)
			{
				_legacyItemLookup = GenerateLegacyItemDictionary();
			}
			if (release <= 4)
			{
				switch (name)
				{
				case "Cobalt Helmet":
					name = "Jungle Hat";
					break;
				case "Cobalt Breastplate":
					name = "Jungle Shirt";
					break;
				case "Cobalt Greaves":
					name = "Jungle Pants";
					break;
				}
			}
			if (release <= 13 && name == "Jungle Rose")
			{
				name = "Jungle Spores";
			}
			if (release <= 20)
			{
				switch (name)
				{
				case "Gills potion":
					name = "Gills Potion";
					break;
				case "Thorn Chakrum":
					name = "Thorn Chakram";
					break;
				case "Ball 'O Hurt":
					name = "Ball O' Hurt";
					break;
				}
			}
			if (release <= 41 && name == "Iron Chain")
			{
				name = "Chain";
			}
			if (release <= 44 && name == "Orb of Light")
			{
				name = "Shadow Orb";
			}
			if (release <= 46)
			{
				if (name == "Black Dye")
				{
					name = "Black Thread";
				}
				if (name == "Green Dye")
				{
					name = "Green Thread";
				}
			}
			if (_legacyItemLookup.TryGetValue(name, out var value))
			{
				return value;
			}
			return 0;
		}
	}
}
