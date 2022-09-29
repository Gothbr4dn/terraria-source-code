using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Zlib;
using Microsoft.Xna.Framework;
using Terraria.IO;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria.Map
{
	public static class MapHelper
	{
		private struct OldMapHelper
		{
			public byte misc;

			public byte misc2;

			public bool active()
			{
				if ((misc & 1) == 1)
				{
					return true;
				}
				return false;
			}

			public bool water()
			{
				if ((misc & 2) == 2)
				{
					return true;
				}
				return false;
			}

			public bool lava()
			{
				if ((misc & 4) == 4)
				{
					return true;
				}
				return false;
			}

			public bool honey()
			{
				if ((misc2 & 0x40) == 64)
				{
					return true;
				}
				return false;
			}

			public bool changed()
			{
				if ((misc & 8) == 8)
				{
					return true;
				}
				return false;
			}

			public bool wall()
			{
				if ((misc & 0x10) == 16)
				{
					return true;
				}
				return false;
			}

			public byte option()
			{
				byte b = 0;
				if ((misc & 0x20) == 32)
				{
					b = (byte)(b + 1);
				}
				if ((misc & 0x40) == 64)
				{
					b = (byte)(b + 2);
				}
				if ((misc & 0x80) == 128)
				{
					b = (byte)(b + 4);
				}
				if ((misc2 & 1) == 1)
				{
					b = (byte)(b + 8);
				}
				return b;
			}

			public byte color()
			{
				return (byte)((misc2 & 0x1E) >> 1);
			}
		}

		public const int drawLoopMilliseconds = 5;

		private const int HeaderEmpty = 0;

		private const int HeaderTile = 1;

		private const int HeaderWall = 2;

		private const int HeaderWater = 3;

		private const int HeaderLava = 4;

		private const int HeaderHoney = 5;

		private const int HeaderHeavenAndHell = 6;

		private const int HeaderBackground = 7;

		private const int Header2_ReadHeader3Bit = 1;

		private const int Header2Color1 = 2;

		private const int Header2Color2 = 4;

		private const int Header2Color3 = 8;

		private const int Header2Color4 = 16;

		private const int Header2Color5 = 32;

		private const int Header2ShimmerBit = 64;

		private const int Header2_UnusedBit8 = 128;

		private const int Header3_ReservedForHeader4Bit = 1;

		private const int Header3_UnusudBit2 = 2;

		private const int Header3_UnusudBit3 = 4;

		private const int Header3_UnusudBit4 = 8;

		private const int Header3_UnusudBit5 = 16;

		private const int Header3_UnusudBit6 = 32;

		private const int Header3_UnusudBit7 = 64;

		private const int Header3_UnusudBit8 = 128;

		private const int maxTileOptions = 12;

		private const int maxWallOptions = 2;

		private const int maxLiquidTypes = 4;

		private const int maxSkyGradients = 256;

		private const int maxDirtGradients = 256;

		private const int maxRockGradients = 256;

		public static int maxUpdateTile = 1000;

		public static int numUpdateTile = 0;

		public static short[] updateTileX = new short[maxUpdateTile];

		public static short[] updateTileY = new short[maxUpdateTile];

		private static object IOLock = new object();

		public static int[] tileOptionCounts;

		public static int[] wallOptionCounts;

		public static ushort[] tileLookup;

		public static ushort[] wallLookup;

		private static ushort tilePosition;

		private static ushort wallPosition;

		private static ushort liquidPosition;

		private static ushort skyPosition;

		private static ushort dirtPosition;

		private static ushort rockPosition;

		private static ushort hellPosition;

		private static Color[] colorLookup;

		private static ushort[] snowTypes;

		private static ushort wallRangeStart;

		private static ushort wallRangeEnd;

		public static bool noStatusText = false;

		public static void Initialize()
		{
			Color[][] array = new Color[693][];
			for (int i = 0; i < 693; i++)
			{
				array[i] = new Color[12];
			}
			array[656][0] = new Color(21, 124, 212);
			array[624][0] = new Color(210, 91, 77);
			array[621][0] = new Color(250, 250, 250);
			array[622][0] = new Color(235, 235, 249);
			array[518][0] = new Color(26, 196, 84);
			array[518][1] = new Color(48, 208, 234);
			array[518][2] = new Color(135, 196, 26);
			array[519][0] = new Color(28, 216, 109);
			array[519][1] = new Color(107, 182, 0);
			array[519][2] = new Color(75, 184, 230);
			array[519][3] = new Color(208, 80, 80);
			array[519][4] = new Color(141, 137, 223);
			array[519][5] = new Color(182, 175, 130);
			array[549][0] = new Color(54, 83, 20);
			array[528][0] = new Color(182, 175, 130);
			array[529][0] = new Color(99, 150, 8);
			array[529][1] = new Color(139, 154, 64);
			array[529][2] = new Color(34, 129, 168);
			array[529][3] = new Color(180, 82, 82);
			array[529][4] = new Color(113, 108, 205);
			Color color = new Color(151, 107, 75);
			array[0][0] = color;
			array[668][0] = color;
			array[5][0] = color;
			array[5][1] = new Color(182, 175, 130);
			Color color2 = new Color(127, 127, 127);
			array[583][0] = color2;
			array[584][0] = color2;
			array[585][0] = color2;
			array[586][0] = color2;
			array[587][0] = color2;
			array[588][0] = color2;
			array[589][0] = color2;
			array[590][0] = color2;
			array[595][0] = color;
			array[596][0] = color;
			array[615][0] = color;
			array[616][0] = color;
			array[634][0] = new Color(145, 120, 120);
			array[633][0] = new Color(210, 140, 100);
			array[637][0] = new Color(200, 120, 75);
			array[638][0] = new Color(200, 120, 75);
			array[30][0] = color;
			array[191][0] = color;
			array[272][0] = new Color(121, 119, 101);
			color = new Color(128, 128, 128);
			array[1][0] = color;
			array[38][0] = color;
			array[48][0] = color;
			array[130][0] = color;
			array[138][0] = color;
			array[664][0] = color;
			array[273][0] = color;
			array[283][0] = color;
			array[618][0] = color;
			array[654][0] = new Color(200, 44, 28);
			array[2][0] = new Color(28, 216, 94);
			array[477][0] = new Color(28, 216, 94);
			array[492][0] = new Color(78, 193, 227);
			color = new Color(26, 196, 84);
			array[3][0] = color;
			array[192][0] = color;
			array[73][0] = new Color(27, 197, 109);
			array[52][0] = new Color(23, 177, 76);
			array[353][0] = new Color(28, 216, 94);
			array[20][0] = new Color(163, 116, 81);
			array[6][0] = new Color(140, 101, 80);
			color = new Color(150, 67, 22);
			array[7][0] = color;
			array[47][0] = color;
			array[284][0] = color;
			array[682][0] = color;
			array[560][0] = color;
			color = new Color(185, 164, 23);
			array[8][0] = color;
			array[45][0] = color;
			array[680][0] = color;
			array[560][2] = color;
			color = new Color(185, 194, 195);
			array[9][0] = color;
			array[46][0] = color;
			array[681][0] = color;
			array[560][1] = color;
			color = new Color(98, 95, 167);
			array[22][0] = color;
			array[140][0] = color;
			array[23][0] = new Color(141, 137, 223);
			array[24][0] = new Color(122, 116, 218);
			array[636][0] = new Color(122, 116, 218);
			array[25][0] = new Color(109, 90, 128);
			array[37][0] = new Color(104, 86, 84);
			array[39][0] = new Color(181, 62, 59);
			array[40][0] = new Color(146, 81, 68);
			array[41][0] = new Color(66, 84, 109);
			array[677][0] = new Color(66, 84, 109);
			array[481][0] = new Color(66, 84, 109);
			array[43][0] = new Color(84, 100, 63);
			array[678][0] = new Color(84, 100, 63);
			array[482][0] = new Color(84, 100, 63);
			array[44][0] = new Color(107, 68, 99);
			array[679][0] = new Color(107, 68, 99);
			array[483][0] = new Color(107, 68, 99);
			array[53][0] = new Color(186, 168, 84);
			color = new Color(190, 171, 94);
			array[151][0] = color;
			array[154][0] = color;
			array[274][0] = color;
			array[328][0] = new Color(200, 246, 254);
			array[329][0] = new Color(15, 15, 15);
			array[54][0] = new Color(200, 246, 254);
			array[56][0] = new Color(43, 40, 84);
			array[75][0] = new Color(26, 26, 26);
			array[683][0] = new Color(100, 90, 190);
			array[57][0] = new Color(68, 68, 76);
			color = new Color(142, 66, 66);
			array[58][0] = color;
			array[76][0] = color;
			array[684][0] = color;
			color = new Color(92, 68, 73);
			array[59][0] = color;
			array[120][0] = color;
			array[60][0] = new Color(143, 215, 29);
			array[61][0] = new Color(135, 196, 26);
			array[74][0] = new Color(96, 197, 27);
			array[62][0] = new Color(121, 176, 24);
			array[233][0] = new Color(107, 182, 29);
			array[652][0] = array[233][0];
			array[651][0] = array[233][0];
			array[63][0] = new Color(110, 140, 182);
			array[64][0] = new Color(196, 96, 114);
			array[65][0] = new Color(56, 150, 97);
			array[66][0] = new Color(160, 118, 58);
			array[67][0] = new Color(140, 58, 166);
			array[68][0] = new Color(125, 191, 197);
			array[566][0] = new Color(233, 180, 90);
			array[70][0] = new Color(93, 127, 255);
			color = new Color(182, 175, 130);
			array[71][0] = color;
			array[72][0] = color;
			array[190][0] = color;
			array[578][0] = new Color(172, 155, 110);
			color = new Color(73, 120, 17);
			array[80][0] = color;
			array[484][0] = color;
			array[188][0] = color;
			array[80][1] = new Color(87, 84, 151);
			array[80][2] = new Color(34, 129, 168);
			array[80][3] = new Color(130, 56, 55);
			color = new Color(11, 80, 143);
			array[107][0] = color;
			array[121][0] = color;
			array[685][0] = color;
			color = new Color(91, 169, 169);
			array[108][0] = color;
			array[122][0] = color;
			array[686][0] = color;
			color = new Color(128, 26, 52);
			array[111][0] = color;
			array[150][0] = color;
			array[109][0] = new Color(78, 193, 227);
			array[110][0] = new Color(48, 186, 135);
			array[113][0] = new Color(48, 208, 234);
			array[115][0] = new Color(33, 171, 207);
			array[112][0] = new Color(103, 98, 122);
			color = new Color(238, 225, 218);
			array[116][0] = color;
			array[118][0] = color;
			array[117][0] = new Color(181, 172, 190);
			array[119][0] = new Color(107, 92, 108);
			array[123][0] = new Color(106, 107, 118);
			array[124][0] = new Color(73, 51, 36);
			array[131][0] = new Color(52, 52, 52);
			array[145][0] = new Color(192, 30, 30);
			array[146][0] = new Color(43, 192, 30);
			color = new Color(211, 236, 241);
			array[147][0] = color;
			array[148][0] = color;
			array[152][0] = new Color(128, 133, 184);
			array[153][0] = new Color(239, 141, 126);
			array[155][0] = new Color(131, 162, 161);
			array[156][0] = new Color(170, 171, 157);
			array[157][0] = new Color(104, 100, 126);
			color = new Color(145, 81, 85);
			array[158][0] = color;
			array[232][0] = color;
			array[575][0] = new Color(125, 61, 65);
			array[159][0] = new Color(148, 133, 98);
			array[161][0] = new Color(144, 195, 232);
			array[162][0] = new Color(184, 219, 240);
			array[163][0] = new Color(174, 145, 214);
			array[164][0] = new Color(218, 182, 204);
			array[170][0] = new Color(27, 109, 69);
			array[171][0] = new Color(33, 135, 85);
			color = new Color(129, 125, 93);
			array[166][0] = color;
			array[175][0] = color;
			array[167][0] = new Color(62, 82, 114);
			color = new Color(132, 157, 127);
			array[168][0] = color;
			array[176][0] = color;
			color = new Color(152, 171, 198);
			array[169][0] = color;
			array[177][0] = color;
			array[179][0] = new Color(49, 134, 114);
			array[180][0] = new Color(126, 134, 49);
			array[181][0] = new Color(134, 59, 49);
			array[182][0] = new Color(43, 86, 140);
			array[183][0] = new Color(121, 49, 134);
			array[381][0] = new Color(254, 121, 2);
			array[687][0] = new Color(254, 121, 2);
			array[534][0] = new Color(114, 254, 2);
			array[689][0] = new Color(114, 254, 2);
			array[536][0] = new Color(0, 197, 208);
			array[690][0] = new Color(0, 197, 208);
			array[539][0] = new Color(208, 0, 126);
			array[688][0] = new Color(208, 0, 126);
			array[625][0] = new Color(220, 12, 237);
			array[691][0] = new Color(220, 12, 237);
			array[627][0] = new Color(255, 76, 76);
			array[627][1] = new Color(255, 195, 76);
			array[627][2] = new Color(195, 255, 76);
			array[627][3] = new Color(76, 255, 76);
			array[627][4] = new Color(76, 255, 195);
			array[627][5] = new Color(76, 195, 255);
			array[627][6] = new Color(77, 76, 255);
			array[627][7] = new Color(196, 76, 255);
			array[627][8] = new Color(255, 76, 195);
			array[512][0] = new Color(49, 134, 114);
			array[513][0] = new Color(126, 134, 49);
			array[514][0] = new Color(134, 59, 49);
			array[515][0] = new Color(43, 86, 140);
			array[516][0] = new Color(121, 49, 134);
			array[517][0] = new Color(254, 121, 2);
			array[535][0] = new Color(114, 254, 2);
			array[537][0] = new Color(0, 197, 208);
			array[540][0] = new Color(208, 0, 126);
			array[626][0] = new Color(220, 12, 237);
			for (int j = 0; j < array[628].Length; j++)
			{
				array[628][j] = array[627][j];
			}
			for (int k = 0; k < array[692].Length; k++)
			{
				array[692][k] = array[627][k];
			}
			for (int l = 0; l < array[160].Length; l++)
			{
				array[160][l] = array[627][l];
			}
			array[184][0] = new Color(29, 106, 88);
			array[184][1] = new Color(94, 100, 36);
			array[184][2] = new Color(96, 44, 40);
			array[184][3] = new Color(34, 63, 102);
			array[184][4] = new Color(79, 35, 95);
			array[184][5] = new Color(253, 62, 3);
			array[184][6] = new Color(22, 123, 62);
			array[184][7] = new Color(0, 106, 148);
			array[184][8] = new Color(148, 0, 132);
			array[184][9] = new Color(122, 24, 168);
			array[184][10] = new Color(220, 20, 20);
			array[189][0] = new Color(223, 255, 255);
			array[193][0] = new Color(56, 121, 255);
			array[194][0] = new Color(157, 157, 107);
			array[195][0] = new Color(134, 22, 34);
			array[196][0] = new Color(147, 144, 178);
			array[197][0] = new Color(97, 200, 225);
			array[198][0] = new Color(62, 61, 52);
			array[199][0] = new Color(208, 80, 80);
			array[201][0] = new Color(203, 61, 64);
			array[205][0] = new Color(186, 50, 52);
			array[200][0] = new Color(216, 152, 144);
			array[202][0] = new Color(213, 178, 28);
			array[203][0] = new Color(128, 44, 45);
			array[204][0] = new Color(125, 55, 65);
			array[206][0] = new Color(124, 175, 201);
			array[208][0] = new Color(88, 105, 118);
			array[211][0] = new Color(191, 233, 115);
			array[213][0] = new Color(137, 120, 67);
			array[214][0] = new Color(103, 103, 103);
			array[221][0] = new Color(239, 90, 50);
			array[222][0] = new Color(231, 96, 228);
			array[223][0] = new Color(57, 85, 101);
			array[224][0] = new Color(107, 132, 139);
			array[225][0] = new Color(227, 125, 22);
			array[226][0] = new Color(141, 56, 0);
			array[229][0] = new Color(255, 156, 12);
			array[659][0] = new Color(247, 228, 254);
			array[230][0] = new Color(131, 79, 13);
			array[234][0] = new Color(53, 44, 41);
			array[235][0] = new Color(214, 184, 46);
			array[236][0] = new Color(149, 232, 87);
			array[237][0] = new Color(255, 241, 51);
			array[238][0] = new Color(225, 128, 206);
			array[655][0] = new Color(225, 128, 206);
			array[243][0] = new Color(198, 196, 170);
			array[248][0] = new Color(219, 71, 38);
			array[249][0] = new Color(235, 38, 231);
			array[250][0] = new Color(86, 85, 92);
			array[251][0] = new Color(235, 150, 23);
			array[252][0] = new Color(153, 131, 44);
			array[253][0] = new Color(57, 48, 97);
			array[254][0] = new Color(248, 158, 92);
			array[255][0] = new Color(107, 49, 154);
			array[256][0] = new Color(154, 148, 49);
			array[257][0] = new Color(49, 49, 154);
			array[258][0] = new Color(49, 154, 68);
			array[259][0] = new Color(154, 49, 77);
			array[260][0] = new Color(85, 89, 118);
			array[261][0] = new Color(154, 83, 49);
			array[262][0] = new Color(221, 79, 255);
			array[263][0] = new Color(250, 255, 79);
			array[264][0] = new Color(79, 102, 255);
			array[265][0] = new Color(79, 255, 89);
			array[266][0] = new Color(255, 79, 79);
			array[267][0] = new Color(240, 240, 247);
			array[268][0] = new Color(255, 145, 79);
			array[287][0] = new Color(79, 128, 17);
			color = new Color(122, 217, 232);
			array[275][0] = color;
			array[276][0] = color;
			array[277][0] = color;
			array[278][0] = color;
			array[279][0] = color;
			array[280][0] = color;
			array[281][0] = color;
			array[282][0] = color;
			array[285][0] = color;
			array[286][0] = color;
			array[288][0] = color;
			array[289][0] = color;
			array[290][0] = color;
			array[291][0] = color;
			array[292][0] = color;
			array[293][0] = color;
			array[294][0] = color;
			array[295][0] = color;
			array[296][0] = color;
			array[297][0] = color;
			array[298][0] = color;
			array[299][0] = color;
			array[309][0] = color;
			array[310][0] = color;
			array[413][0] = color;
			array[339][0] = color;
			array[542][0] = color;
			array[632][0] = color;
			array[640][0] = color;
			array[643][0] = color;
			array[644][0] = color;
			array[645][0] = color;
			array[358][0] = color;
			array[359][0] = color;
			array[360][0] = color;
			array[361][0] = color;
			array[362][0] = color;
			array[363][0] = color;
			array[364][0] = color;
			array[391][0] = color;
			array[392][0] = color;
			array[393][0] = color;
			array[394][0] = color;
			array[414][0] = color;
			array[505][0] = color;
			array[543][0] = color;
			array[598][0] = color;
			array[521][0] = color;
			array[522][0] = color;
			array[523][0] = color;
			array[524][0] = color;
			array[525][0] = color;
			array[526][0] = color;
			array[527][0] = color;
			array[532][0] = color;
			array[533][0] = color;
			array[538][0] = color;
			array[544][0] = color;
			array[629][0] = color;
			array[550][0] = color;
			array[551][0] = color;
			array[553][0] = color;
			array[554][0] = color;
			array[555][0] = color;
			array[556][0] = color;
			array[558][0] = color;
			array[559][0] = color;
			array[580][0] = color;
			array[582][0] = color;
			array[599][0] = color;
			array[600][0] = color;
			array[601][0] = color;
			array[602][0] = color;
			array[603][0] = color;
			array[604][0] = color;
			array[605][0] = color;
			array[606][0] = color;
			array[607][0] = color;
			array[608][0] = color;
			array[609][0] = color;
			array[610][0] = color;
			array[611][0] = color;
			array[612][0] = color;
			array[619][0] = color;
			array[620][0] = color;
			array[630][0] = new Color(117, 145, 73);
			array[631][0] = new Color(122, 234, 225);
			array[552][0] = array[53][0];
			array[564][0] = new Color(87, 127, 220);
			array[408][0] = new Color(85, 83, 82);
			array[409][0] = new Color(85, 83, 82);
			array[669][0] = new Color(83, 46, 57);
			array[670][0] = new Color(91, 87, 167);
			array[671][0] = new Color(23, 33, 81);
			array[672][0] = new Color(53, 133, 103);
			array[673][0] = new Color(11, 67, 80);
			array[674][0] = new Color(40, 49, 60);
			array[675][0] = new Color(21, 13, 77);
			array[676][0] = new Color(195, 201, 215);
			array[415][0] = new Color(249, 75, 7);
			array[416][0] = new Color(0, 160, 170);
			array[417][0] = new Color(160, 87, 234);
			array[418][0] = new Color(22, 173, 254);
			array[489][0] = new Color(255, 29, 136);
			array[490][0] = new Color(211, 211, 211);
			array[311][0] = new Color(117, 61, 25);
			array[312][0] = new Color(204, 93, 73);
			array[313][0] = new Color(87, 150, 154);
			array[4][0] = new Color(253, 221, 3);
			array[4][1] = new Color(253, 221, 3);
			color = new Color(253, 221, 3);
			array[93][0] = color;
			array[33][0] = color;
			array[174][0] = color;
			array[100][0] = color;
			array[98][0] = color;
			array[173][0] = color;
			color = new Color(119, 105, 79);
			array[11][0] = color;
			array[10][0] = color;
			array[593][0] = color;
			array[594][0] = color;
			color = new Color(191, 142, 111);
			array[14][0] = color;
			array[469][0] = color;
			array[486][0] = color;
			array[488][0] = new Color(127, 92, 69);
			array[487][0] = color;
			array[487][1] = color;
			array[15][0] = color;
			array[15][1] = color;
			array[497][0] = color;
			array[18][0] = color;
			array[19][0] = color;
			array[19][1] = Color.Black;
			array[55][0] = color;
			array[79][0] = color;
			array[86][0] = color;
			array[87][0] = color;
			array[88][0] = color;
			array[89][0] = color;
			array[94][0] = color;
			array[101][0] = color;
			array[104][0] = color;
			array[106][0] = color;
			array[114][0] = color;
			array[128][0] = color;
			array[139][0] = color;
			array[172][0] = color;
			array[216][0] = color;
			array[269][0] = color;
			array[334][0] = color;
			array[471][0] = color;
			array[470][0] = color;
			array[475][0] = color;
			array[377][0] = color;
			array[380][0] = color;
			array[395][0] = color;
			array[573][0] = color;
			array[12][0] = new Color(174, 24, 69);
			array[665][0] = new Color(174, 24, 69);
			array[639][0] = new Color(110, 105, 255);
			array[13][0] = new Color(133, 213, 247);
			color = new Color(144, 148, 144);
			array[17][0] = color;
			array[90][0] = color;
			array[96][0] = color;
			array[97][0] = color;
			array[99][0] = color;
			array[132][0] = color;
			array[142][0] = color;
			array[143][0] = color;
			array[144][0] = color;
			array[207][0] = color;
			array[209][0] = color;
			array[212][0] = color;
			array[217][0] = color;
			array[218][0] = color;
			array[219][0] = color;
			array[220][0] = color;
			array[228][0] = color;
			array[300][0] = color;
			array[301][0] = color;
			array[302][0] = color;
			array[303][0] = color;
			array[304][0] = color;
			array[305][0] = color;
			array[306][0] = color;
			array[307][0] = color;
			array[308][0] = color;
			array[567][0] = color;
			array[349][0] = new Color(144, 148, 144);
			array[531][0] = new Color(144, 148, 144);
			array[105][0] = new Color(144, 148, 144);
			array[105][1] = new Color(177, 92, 31);
			array[105][2] = new Color(201, 188, 170);
			array[137][0] = new Color(144, 148, 144);
			array[137][1] = new Color(141, 56, 0);
			array[137][2] = new Color(144, 148, 144);
			array[16][0] = new Color(140, 130, 116);
			array[26][0] = new Color(119, 101, 125);
			array[26][1] = new Color(214, 127, 133);
			array[36][0] = new Color(230, 89, 92);
			array[28][0] = new Color(151, 79, 80);
			array[28][1] = new Color(90, 139, 140);
			array[28][2] = new Color(192, 136, 70);
			array[28][3] = new Color(203, 185, 151);
			array[28][4] = new Color(73, 56, 41);
			array[28][5] = new Color(148, 159, 67);
			array[28][6] = new Color(138, 172, 67);
			array[28][7] = new Color(226, 122, 47);
			array[28][8] = new Color(198, 87, 93);
			for (int m = 0; m < array[653].Length; m++)
			{
				array[653][m] = array[28][m];
			}
			array[29][0] = new Color(175, 105, 128);
			array[51][0] = new Color(192, 202, 203);
			array[31][0] = new Color(141, 120, 168);
			array[31][1] = new Color(212, 105, 105);
			array[32][0] = new Color(151, 135, 183);
			array[42][0] = new Color(251, 235, 127);
			array[50][0] = new Color(170, 48, 114);
			array[85][0] = new Color(192, 192, 192);
			array[69][0] = new Color(190, 150, 92);
			array[77][0] = new Color(238, 85, 70);
			array[81][0] = new Color(245, 133, 191);
			array[78][0] = new Color(121, 110, 97);
			array[141][0] = new Color(192, 59, 59);
			array[129][0] = new Color(255, 117, 224);
			array[129][1] = new Color(255, 117, 224);
			array[126][0] = new Color(159, 209, 229);
			array[125][0] = new Color(141, 175, 255);
			array[103][0] = new Color(141, 98, 77);
			array[95][0] = new Color(255, 162, 31);
			array[92][0] = new Color(213, 229, 237);
			array[91][0] = new Color(13, 88, 130);
			array[215][0] = new Color(254, 121, 2);
			array[592][0] = new Color(254, 121, 2);
			array[316][0] = new Color(157, 176, 226);
			array[317][0] = new Color(118, 227, 129);
			array[318][0] = new Color(227, 118, 215);
			array[319][0] = new Color(96, 68, 48);
			array[320][0] = new Color(203, 185, 151);
			array[321][0] = new Color(96, 77, 64);
			array[574][0] = new Color(76, 57, 44);
			array[322][0] = new Color(198, 170, 104);
			array[635][0] = new Color(145, 120, 120);
			array[149][0] = new Color(220, 50, 50);
			array[149][1] = new Color(0, 220, 50);
			array[149][2] = new Color(50, 50, 220);
			array[133][0] = new Color(231, 53, 56);
			array[133][1] = new Color(192, 189, 221);
			array[134][0] = new Color(166, 187, 153);
			array[134][1] = new Color(241, 129, 249);
			array[102][0] = new Color(229, 212, 73);
			array[35][0] = new Color(226, 145, 30);
			array[34][0] = new Color(235, 166, 135);
			array[136][0] = new Color(213, 203, 204);
			array[231][0] = new Color(224, 194, 101);
			array[239][0] = new Color(224, 194, 101);
			array[240][0] = new Color(120, 85, 60);
			array[240][1] = new Color(99, 50, 30);
			array[240][2] = new Color(153, 153, 117);
			array[240][3] = new Color(112, 84, 56);
			array[240][4] = new Color(234, 231, 226);
			array[241][0] = new Color(77, 74, 72);
			array[244][0] = new Color(200, 245, 253);
			color = new Color(99, 50, 30);
			array[242][0] = color;
			array[245][0] = color;
			array[246][0] = color;
			array[242][1] = new Color(185, 142, 97);
			array[247][0] = new Color(140, 150, 150);
			array[271][0] = new Color(107, 250, 255);
			array[270][0] = new Color(187, 255, 107);
			array[581][0] = new Color(255, 150, 150);
			array[660][0] = new Color(255, 150, 150);
			array[572][0] = new Color(255, 186, 212);
			array[572][1] = new Color(209, 201, 255);
			array[572][2] = new Color(200, 254, 255);
			array[572][3] = new Color(199, 255, 211);
			array[572][4] = new Color(180, 209, 255);
			array[572][5] = new Color(255, 220, 214);
			array[314][0] = new Color(181, 164, 125);
			array[324][0] = new Color(228, 213, 173);
			array[351][0] = new Color(31, 31, 31);
			array[424][0] = new Color(146, 155, 187);
			array[429][0] = new Color(220, 220, 220);
			array[445][0] = new Color(240, 240, 240);
			array[21][0] = new Color(174, 129, 92);
			array[21][1] = new Color(233, 207, 94);
			array[21][2] = new Color(137, 128, 200);
			array[21][3] = new Color(160, 160, 160);
			array[21][4] = new Color(106, 210, 255);
			array[441][0] = array[21][0];
			array[441][1] = array[21][1];
			array[441][2] = array[21][2];
			array[441][3] = array[21][3];
			array[441][4] = array[21][4];
			array[27][0] = new Color(54, 154, 54);
			array[27][1] = new Color(226, 196, 49);
			color = new Color(246, 197, 26);
			array[82][0] = color;
			array[83][0] = color;
			array[84][0] = color;
			color = new Color(76, 150, 216);
			array[82][1] = color;
			array[83][1] = color;
			array[84][1] = color;
			color = new Color(185, 214, 42);
			array[82][2] = color;
			array[83][2] = color;
			array[84][2] = color;
			color = new Color(167, 203, 37);
			array[82][3] = color;
			array[83][3] = color;
			array[84][3] = color;
			array[591][6] = color;
			color = new Color(32, 168, 117);
			array[82][4] = color;
			array[83][4] = color;
			array[84][4] = color;
			color = new Color(177, 69, 49);
			array[82][5] = color;
			array[83][5] = color;
			array[84][5] = color;
			color = new Color(40, 152, 240);
			array[82][6] = color;
			array[83][6] = color;
			array[84][6] = color;
			array[591][1] = new Color(246, 197, 26);
			array[591][2] = new Color(76, 150, 216);
			array[591][3] = new Color(32, 168, 117);
			array[591][4] = new Color(40, 152, 240);
			array[591][5] = new Color(114, 81, 56);
			array[591][6] = new Color(141, 137, 223);
			array[591][7] = new Color(208, 80, 80);
			array[591][8] = new Color(177, 69, 49);
			array[165][0] = new Color(115, 173, 229);
			array[165][1] = new Color(100, 100, 100);
			array[165][2] = new Color(152, 152, 152);
			array[165][3] = new Color(227, 125, 22);
			array[178][0] = new Color(208, 94, 201);
			array[178][1] = new Color(233, 146, 69);
			array[178][2] = new Color(71, 146, 251);
			array[178][3] = new Color(60, 226, 133);
			array[178][4] = new Color(250, 30, 71);
			array[178][5] = new Color(166, 176, 204);
			array[178][6] = new Color(255, 217, 120);
			color = new Color(99, 99, 99);
			array[185][0] = color;
			array[186][0] = color;
			array[187][0] = color;
			array[565][0] = color;
			array[579][0] = color;
			color = new Color(114, 81, 56);
			array[185][1] = color;
			array[186][1] = color;
			array[187][1] = color;
			array[591][0] = color;
			color = new Color(133, 133, 101);
			array[185][2] = color;
			array[186][2] = color;
			array[187][2] = color;
			color = new Color(151, 200, 211);
			array[185][3] = color;
			array[186][3] = color;
			array[187][3] = color;
			color = new Color(177, 183, 161);
			array[185][4] = color;
			array[186][4] = color;
			array[187][4] = color;
			color = new Color(134, 114, 38);
			array[185][5] = color;
			array[186][5] = color;
			array[187][5] = color;
			color = new Color(82, 62, 66);
			array[185][6] = color;
			array[186][6] = color;
			array[187][6] = color;
			color = new Color(143, 117, 121);
			array[185][7] = color;
			array[186][7] = color;
			array[187][7] = color;
			color = new Color(177, 92, 31);
			array[185][8] = color;
			array[186][8] = color;
			array[187][8] = color;
			color = new Color(85, 73, 87);
			array[185][9] = color;
			array[186][9] = color;
			array[187][9] = color;
			color = new Color(26, 196, 84);
			array[185][10] = color;
			array[186][10] = color;
			array[187][10] = color;
			Color[] array2 = array[647];
			for (int n = 0; n < array2.Length; n++)
			{
				array2[n] = array[186][n];
			}
			array2 = array[648];
			for (int num = 0; num < array2.Length; num++)
			{
				array2[num] = array[187][num];
			}
			array2 = array[650];
			for (int num2 = 0; num2 < array2.Length; num2++)
			{
				array2[num2] = array[185][num2];
			}
			array2 = array[649];
			for (int num3 = 0; num3 < array2.Length; num3++)
			{
				array2[num3] = array[185][num3];
			}
			array[227][0] = new Color(74, 197, 155);
			array[227][1] = new Color(54, 153, 88);
			array[227][2] = new Color(63, 126, 207);
			array[227][3] = new Color(240, 180, 4);
			array[227][4] = new Color(45, 68, 168);
			array[227][5] = new Color(61, 92, 0);
			array[227][6] = new Color(216, 112, 152);
			array[227][7] = new Color(200, 40, 24);
			array[227][8] = new Color(113, 45, 133);
			array[227][9] = new Color(235, 137, 2);
			array[227][10] = new Color(41, 152, 135);
			array[227][11] = new Color(198, 19, 78);
			array[373][0] = new Color(9, 61, 191);
			array[374][0] = new Color(253, 32, 3);
			array[375][0] = new Color(255, 156, 12);
			array[461][0] = new Color(212, 192, 100);
			array[461][1] = new Color(137, 132, 156);
			array[461][2] = new Color(148, 122, 112);
			array[461][3] = new Color(221, 201, 206);
			array[323][0] = new Color(182, 141, 86);
			array[325][0] = new Color(129, 125, 93);
			array[326][0] = new Color(9, 61, 191);
			array[327][0] = new Color(253, 32, 3);
			array[507][0] = new Color(5, 5, 5);
			array[508][0] = new Color(5, 5, 5);
			array[330][0] = new Color(226, 118, 76);
			array[331][0] = new Color(161, 172, 173);
			array[332][0] = new Color(204, 181, 72);
			array[333][0] = new Color(190, 190, 178);
			array[335][0] = new Color(217, 174, 137);
			array[336][0] = new Color(253, 62, 3);
			array[337][0] = new Color(144, 148, 144);
			array[338][0] = new Color(85, 255, 160);
			array[315][0] = new Color(235, 114, 80);
			array[641][0] = new Color(235, 125, 150);
			array[340][0] = new Color(96, 248, 2);
			array[341][0] = new Color(105, 74, 202);
			array[342][0] = new Color(29, 240, 255);
			array[343][0] = new Color(254, 202, 80);
			array[344][0] = new Color(131, 252, 245);
			array[345][0] = new Color(255, 156, 12);
			array[346][0] = new Color(149, 212, 89);
			array[642][0] = new Color(149, 212, 89);
			array[347][0] = new Color(236, 74, 79);
			array[348][0] = new Color(44, 26, 233);
			array[350][0] = new Color(55, 97, 155);
			array[352][0] = new Color(238, 97, 94);
			array[354][0] = new Color(141, 107, 89);
			array[355][0] = new Color(141, 107, 89);
			array[463][0] = new Color(155, 214, 240);
			array[491][0] = new Color(60, 20, 160);
			array[464][0] = new Color(233, 183, 128);
			array[465][0] = new Color(51, 84, 195);
			array[466][0] = new Color(205, 153, 73);
			array[356][0] = new Color(233, 203, 24);
			array[663][0] = new Color(24, 203, 233);
			array[357][0] = new Color(168, 178, 204);
			array[367][0] = new Color(168, 178, 204);
			array[561][0] = new Color(148, 158, 184);
			array[365][0] = new Color(146, 136, 205);
			array[366][0] = new Color(223, 232, 233);
			array[368][0] = new Color(50, 46, 104);
			array[369][0] = new Color(50, 46, 104);
			array[576][0] = new Color(30, 26, 84);
			array[370][0] = new Color(127, 116, 194);
			array[49][0] = new Color(89, 201, 255);
			array[372][0] = new Color(252, 128, 201);
			array[646][0] = new Color(108, 133, 140);
			array[371][0] = new Color(249, 101, 189);
			array[376][0] = new Color(160, 120, 92);
			array[378][0] = new Color(160, 120, 100);
			array[379][0] = new Color(251, 209, 240);
			array[382][0] = new Color(28, 216, 94);
			array[383][0] = new Color(221, 136, 144);
			array[384][0] = new Color(131, 206, 12);
			array[385][0] = new Color(87, 21, 144);
			array[386][0] = new Color(127, 92, 69);
			array[387][0] = new Color(127, 92, 69);
			array[388][0] = new Color(127, 92, 69);
			array[389][0] = new Color(127, 92, 69);
			array[390][0] = new Color(253, 32, 3);
			array[397][0] = new Color(212, 192, 100);
			array[396][0] = new Color(198, 124, 78);
			array[577][0] = new Color(178, 104, 58);
			array[398][0] = new Color(100, 82, 126);
			array[399][0] = new Color(77, 76, 66);
			array[400][0] = new Color(96, 68, 117);
			array[401][0] = new Color(68, 60, 51);
			array[402][0] = new Color(174, 168, 186);
			array[403][0] = new Color(205, 152, 186);
			array[404][0] = new Color(212, 148, 88);
			array[405][0] = new Color(140, 140, 140);
			array[406][0] = new Color(120, 120, 120);
			array[407][0] = new Color(255, 227, 132);
			array[411][0] = new Color(227, 46, 46);
			array[494][0] = new Color(227, 227, 227);
			array[421][0] = new Color(65, 75, 90);
			array[422][0] = new Color(65, 75, 90);
			array[425][0] = new Color(146, 155, 187);
			array[426][0] = new Color(168, 38, 47);
			array[430][0] = new Color(39, 168, 96);
			array[431][0] = new Color(39, 94, 168);
			array[432][0] = new Color(242, 221, 100);
			array[433][0] = new Color(224, 100, 242);
			array[434][0] = new Color(197, 193, 216);
			array[427][0] = new Color(183, 53, 62);
			array[435][0] = new Color(54, 183, 111);
			array[436][0] = new Color(54, 109, 183);
			array[437][0] = new Color(255, 236, 115);
			array[438][0] = new Color(239, 115, 255);
			array[439][0] = new Color(212, 208, 231);
			array[440][0] = new Color(238, 51, 53);
			array[440][1] = new Color(13, 107, 216);
			array[440][2] = new Color(33, 184, 115);
			array[440][3] = new Color(255, 221, 62);
			array[440][4] = new Color(165, 0, 236);
			array[440][5] = new Color(223, 230, 238);
			array[440][6] = new Color(207, 101, 0);
			array[419][0] = new Color(88, 95, 114);
			array[419][1] = new Color(214, 225, 236);
			array[419][2] = new Color(25, 131, 205);
			array[423][0] = new Color(245, 197, 1);
			array[423][1] = new Color(185, 0, 224);
			array[423][2] = new Color(58, 240, 111);
			array[423][3] = new Color(50, 107, 197);
			array[423][4] = new Color(253, 91, 3);
			array[423][5] = new Color(254, 194, 20);
			array[423][6] = new Color(174, 195, 215);
			array[420][0] = new Color(99, 255, 107);
			array[420][1] = new Color(99, 255, 107);
			array[420][4] = new Color(99, 255, 107);
			array[420][2] = new Color(218, 2, 5);
			array[420][3] = new Color(218, 2, 5);
			array[420][5] = new Color(218, 2, 5);
			array[476][0] = new Color(160, 160, 160);
			array[410][0] = new Color(75, 139, 166);
			array[480][0] = new Color(120, 50, 50);
			array[509][0] = new Color(50, 50, 60);
			array[657][0] = new Color(35, 205, 215);
			array[658][0] = new Color(200, 105, 230);
			array[412][0] = new Color(75, 139, 166);
			array[443][0] = new Color(144, 148, 144);
			array[442][0] = new Color(3, 144, 201);
			array[444][0] = new Color(191, 176, 124);
			array[446][0] = new Color(255, 66, 152);
			array[447][0] = new Color(179, 132, 255);
			array[448][0] = new Color(0, 206, 180);
			array[449][0] = new Color(91, 186, 240);
			array[450][0] = new Color(92, 240, 91);
			array[451][0] = new Color(240, 91, 147);
			array[452][0] = new Color(255, 150, 181);
			array[453][0] = new Color(179, 132, 255);
			array[453][1] = new Color(0, 206, 180);
			array[453][2] = new Color(255, 66, 152);
			array[454][0] = new Color(174, 16, 176);
			array[455][0] = new Color(48, 225, 110);
			array[456][0] = new Color(179, 132, 255);
			array[457][0] = new Color(150, 164, 206);
			array[457][1] = new Color(255, 132, 184);
			array[457][2] = new Color(74, 255, 232);
			array[457][3] = new Color(215, 159, 255);
			array[457][4] = new Color(229, 219, 234);
			array[458][0] = new Color(211, 198, 111);
			array[459][0] = new Color(190, 223, 232);
			array[460][0] = new Color(141, 163, 181);
			array[462][0] = new Color(231, 178, 28);
			array[467][0] = new Color(129, 56, 121);
			array[467][1] = new Color(255, 249, 59);
			array[467][2] = new Color(161, 67, 24);
			array[467][3] = new Color(89, 70, 72);
			array[467][4] = new Color(233, 207, 94);
			array[467][5] = new Color(254, 158, 35);
			array[467][6] = new Color(34, 221, 151);
			array[467][7] = new Color(249, 170, 236);
			array[467][8] = new Color(35, 200, 254);
			array[467][9] = new Color(190, 200, 200);
			array[467][10] = new Color(230, 170, 100);
			array[467][11] = new Color(165, 168, 26);
			for (int num4 = 0; num4 < 12; num4++)
			{
				array[468][num4] = array[467][num4];
			}
			array[472][0] = new Color(190, 160, 140);
			array[473][0] = new Color(85, 114, 123);
			array[474][0] = new Color(116, 94, 97);
			array[478][0] = new Color(108, 34, 35);
			array[479][0] = new Color(178, 114, 68);
			array[485][0] = new Color(198, 134, 88);
			array[492][0] = new Color(78, 193, 227);
			array[492][0] = new Color(78, 193, 227);
			array[493][0] = new Color(250, 249, 252);
			array[493][1] = new Color(240, 90, 90);
			array[493][2] = new Color(98, 230, 92);
			array[493][3] = new Color(95, 197, 238);
			array[493][4] = new Color(241, 221, 100);
			array[493][5] = new Color(213, 92, 237);
			array[494][0] = new Color(224, 219, 236);
			array[495][0] = new Color(253, 227, 215);
			array[496][0] = new Color(165, 159, 153);
			array[498][0] = new Color(202, 174, 165);
			array[499][0] = new Color(160, 187, 142);
			array[500][0] = new Color(254, 158, 35);
			array[501][0] = new Color(34, 221, 151);
			array[502][0] = new Color(249, 170, 236);
			array[503][0] = new Color(35, 200, 254);
			array[506][0] = new Color(61, 61, 61);
			array[510][0] = new Color(191, 142, 111);
			array[511][0] = new Color(187, 68, 74);
			array[520][0] = new Color(224, 219, 236);
			array[545][0] = new Color(255, 126, 145);
			array[530][0] = new Color(107, 182, 0);
			array[530][1] = new Color(23, 154, 209);
			array[530][2] = new Color(238, 97, 94);
			array[530][3] = new Color(113, 108, 205);
			array[546][0] = new Color(60, 60, 60);
			array[557][0] = new Color(60, 60, 60);
			array[547][0] = new Color(120, 110, 100);
			array[548][0] = new Color(120, 110, 100);
			array[562][0] = new Color(165, 168, 26);
			array[563][0] = new Color(165, 168, 26);
			array[571][0] = new Color(165, 168, 26);
			array[568][0] = new Color(248, 203, 233);
			array[569][0] = new Color(203, 248, 218);
			array[570][0] = new Color(160, 242, 255);
			array[597][0] = new Color(28, 216, 94);
			array[597][1] = new Color(183, 237, 20);
			array[597][2] = new Color(185, 83, 200);
			array[597][3] = new Color(131, 128, 168);
			array[597][4] = new Color(38, 142, 214);
			array[597][5] = new Color(229, 154, 9);
			array[597][6] = new Color(142, 227, 234);
			array[597][7] = new Color(98, 111, 223);
			array[597][8] = new Color(241, 233, 158);
			array[617][0] = new Color(233, 207, 94);
			Color color3 = new Color(250, 100, 50);
			array[548][1] = color3;
			array[613][0] = color3;
			array[614][0] = color3;
			array[623][0] = new Color(220, 210, 245);
			array[661][0] = new Color(141, 137, 223);
			array[662][0] = new Color(208, 80, 80);
			array[666][0] = new Color(115, 60, 40);
			array[667][0] = new Color(247, 228, 254);
			Color[] array3 = new Color[4]
			{
				new Color(9, 61, 191),
				new Color(253, 32, 3),
				new Color(254, 194, 20),
				new Color(161, 127, 255)
			};
			Color[][] array4 = new Color[347][];
			for (int num5 = 0; num5 < 347; num5++)
			{
				array4[num5] = new Color[2];
			}
			array4[158][0] = new Color(107, 49, 154);
			array4[163][0] = new Color(154, 148, 49);
			array4[162][0] = new Color(49, 49, 154);
			array4[160][0] = new Color(49, 154, 68);
			array4[161][0] = new Color(154, 49, 77);
			array4[159][0] = new Color(85, 89, 118);
			array4[157][0] = new Color(154, 83, 49);
			array4[154][0] = new Color(221, 79, 255);
			array4[166][0] = new Color(250, 255, 79);
			array4[165][0] = new Color(79, 102, 255);
			array4[156][0] = new Color(79, 255, 89);
			array4[164][0] = new Color(255, 79, 79);
			array4[155][0] = new Color(240, 240, 247);
			array4[153][0] = new Color(255, 145, 79);
			array4[169][0] = new Color(5, 5, 5);
			array4[224][0] = new Color(57, 55, 52);
			array4[323][0] = new Color(55, 25, 33);
			array4[324][0] = new Color(60, 55, 145);
			array4[325][0] = new Color(10, 5, 50);
			array4[326][0] = new Color(30, 105, 75);
			array4[327][0] = new Color(5, 45, 55);
			array4[328][0] = new Color(20, 25, 35);
			array4[329][0] = new Color(15, 10, 50);
			array4[330][0] = new Color(153, 164, 187);
			array4[225][0] = new Color(68, 68, 68);
			array4[226][0] = new Color(148, 138, 74);
			array4[227][0] = new Color(95, 137, 191);
			array4[170][0] = new Color(59, 39, 22);
			array4[171][0] = new Color(59, 39, 22);
			color = new Color(52, 52, 52);
			array4[1][0] = color;
			array4[53][0] = color;
			array4[52][0] = color;
			array4[51][0] = color;
			array4[50][0] = color;
			array4[49][0] = color;
			array4[48][0] = color;
			array4[44][0] = color;
			array4[346][0] = color;
			array4[5][0] = color;
			color = new Color(88, 61, 46);
			array4[2][0] = color;
			array4[16][0] = color;
			array4[59][0] = color;
			array4[3][0] = new Color(61, 58, 78);
			array4[4][0] = new Color(73, 51, 36);
			array4[6][0] = new Color(91, 30, 30);
			color = new Color(27, 31, 42);
			array4[7][0] = color;
			array4[17][0] = color;
			array4[331][0] = color;
			color = new Color(32, 40, 45);
			array4[94][0] = color;
			array4[100][0] = color;
			color = new Color(44, 41, 50);
			array4[95][0] = color;
			array4[101][0] = color;
			color = new Color(31, 39, 26);
			array4[8][0] = color;
			array4[18][0] = color;
			array4[332][0] = color;
			color = new Color(36, 45, 44);
			array4[98][0] = color;
			array4[104][0] = color;
			color = new Color(38, 49, 50);
			array4[99][0] = color;
			array4[105][0] = color;
			color = new Color(41, 28, 36);
			array4[9][0] = color;
			array4[19][0] = color;
			array4[333][0] = color;
			color = new Color(72, 50, 77);
			array4[96][0] = color;
			array4[102][0] = color;
			color = new Color(78, 50, 69);
			array4[97][0] = color;
			array4[103][0] = color;
			array4[10][0] = new Color(74, 62, 12);
			array4[334][0] = new Color(74, 62, 12);
			array4[11][0] = new Color(46, 56, 59);
			array4[335][0] = new Color(46, 56, 59);
			array4[12][0] = new Color(75, 32, 11);
			array4[336][0] = new Color(75, 32, 11);
			array4[13][0] = new Color(67, 37, 37);
			array4[338][0] = new Color(67, 37, 37);
			color = new Color(15, 15, 15);
			array4[14][0] = color;
			array4[337][0] = color;
			array4[20][0] = color;
			array4[15][0] = new Color(52, 43, 45);
			array4[22][0] = new Color(113, 99, 99);
			array4[23][0] = new Color(38, 38, 43);
			array4[24][0] = new Color(53, 39, 41);
			array4[25][0] = new Color(11, 35, 62);
			array4[339][0] = new Color(11, 35, 62);
			array4[26][0] = new Color(21, 63, 70);
			array4[340][0] = new Color(21, 63, 70);
			array4[27][0] = new Color(88, 61, 46);
			array4[27][1] = new Color(52, 52, 52);
			array4[28][0] = new Color(81, 84, 101);
			array4[29][0] = new Color(88, 23, 23);
			array4[30][0] = new Color(28, 88, 23);
			array4[31][0] = new Color(78, 87, 99);
			color = new Color(69, 67, 41);
			array4[34][0] = color;
			array4[37][0] = color;
			array4[32][0] = new Color(86, 17, 40);
			array4[33][0] = new Color(49, 47, 83);
			array4[35][0] = new Color(51, 51, 70);
			array4[36][0] = new Color(87, 59, 55);
			array4[38][0] = new Color(49, 57, 49);
			array4[39][0] = new Color(78, 79, 73);
			array4[45][0] = new Color(60, 59, 51);
			array4[46][0] = new Color(48, 57, 47);
			array4[47][0] = new Color(71, 77, 85);
			array4[40][0] = new Color(85, 102, 103);
			array4[41][0] = new Color(52, 50, 62);
			array4[42][0] = new Color(71, 42, 44);
			array4[43][0] = new Color(73, 66, 50);
			array4[54][0] = new Color(40, 56, 50);
			array4[55][0] = new Color(49, 48, 36);
			array4[56][0] = new Color(43, 33, 32);
			array4[57][0] = new Color(31, 40, 49);
			array4[58][0] = new Color(48, 35, 52);
			array4[60][0] = new Color(1, 52, 20);
			array4[61][0] = new Color(55, 39, 26);
			array4[62][0] = new Color(39, 33, 26);
			array4[69][0] = new Color(43, 42, 68);
			array4[70][0] = new Color(30, 70, 80);
			array4[341][0] = new Color(100, 40, 1);
			array4[342][0] = new Color(92, 30, 72);
			array4[343][0] = new Color(42, 81, 1);
			array4[344][0] = new Color(1, 81, 109);
			array4[345][0] = new Color(56, 22, 97);
			color = new Color(30, 80, 48);
			array4[63][0] = color;
			array4[65][0] = color;
			array4[66][0] = color;
			array4[68][0] = color;
			color = new Color(53, 80, 30);
			array4[64][0] = color;
			array4[67][0] = color;
			array4[78][0] = new Color(63, 39, 26);
			array4[244][0] = new Color(63, 39, 26);
			array4[71][0] = new Color(78, 105, 135);
			array4[72][0] = new Color(52, 84, 12);
			array4[73][0] = new Color(190, 204, 223);
			color = new Color(64, 62, 80);
			array4[74][0] = color;
			array4[80][0] = color;
			array4[75][0] = new Color(65, 65, 35);
			array4[76][0] = new Color(20, 46, 104);
			array4[77][0] = new Color(61, 13, 16);
			array4[79][0] = new Color(51, 47, 96);
			array4[81][0] = new Color(101, 51, 51);
			array4[82][0] = new Color(77, 64, 34);
			array4[83][0] = new Color(62, 38, 41);
			array4[234][0] = new Color(60, 36, 39);
			array4[84][0] = new Color(48, 78, 93);
			array4[85][0] = new Color(54, 63, 69);
			color = new Color(138, 73, 38);
			array4[86][0] = color;
			array4[108][0] = color;
			color = new Color(50, 15, 8);
			array4[87][0] = color;
			array4[112][0] = color;
			array4[109][0] = new Color(94, 25, 17);
			array4[110][0] = new Color(125, 36, 122);
			array4[111][0] = new Color(51, 35, 27);
			array4[113][0] = new Color(135, 58, 0);
			array4[114][0] = new Color(65, 52, 15);
			array4[115][0] = new Color(39, 42, 51);
			array4[116][0] = new Color(89, 26, 27);
			array4[117][0] = new Color(126, 123, 115);
			array4[118][0] = new Color(8, 50, 19);
			array4[119][0] = new Color(95, 21, 24);
			array4[120][0] = new Color(17, 31, 65);
			array4[121][0] = new Color(192, 173, 143);
			array4[122][0] = new Color(114, 114, 131);
			array4[123][0] = new Color(136, 119, 7);
			array4[124][0] = new Color(8, 72, 3);
			array4[125][0] = new Color(117, 132, 82);
			array4[126][0] = new Color(100, 102, 114);
			array4[127][0] = new Color(30, 118, 226);
			array4[128][0] = new Color(93, 6, 102);
			array4[129][0] = new Color(64, 40, 169);
			array4[130][0] = new Color(39, 34, 180);
			array4[131][0] = new Color(87, 94, 125);
			array4[132][0] = new Color(6, 6, 6);
			array4[133][0] = new Color(69, 72, 186);
			array4[134][0] = new Color(130, 62, 16);
			array4[135][0] = new Color(22, 123, 163);
			array4[136][0] = new Color(40, 86, 151);
			array4[137][0] = new Color(183, 75, 15);
			array4[138][0] = new Color(83, 80, 100);
			array4[139][0] = new Color(115, 65, 68);
			array4[140][0] = new Color(119, 108, 81);
			array4[141][0] = new Color(59, 67, 71);
			array4[142][0] = new Color(222, 216, 202);
			array4[143][0] = new Color(90, 112, 105);
			array4[144][0] = new Color(62, 28, 87);
			array4[146][0] = new Color(120, 59, 19);
			array4[147][0] = new Color(59, 59, 59);
			array4[148][0] = new Color(229, 218, 161);
			array4[149][0] = new Color(73, 59, 50);
			array4[151][0] = new Color(102, 75, 34);
			array4[167][0] = new Color(70, 68, 51);
			Color color4 = new Color(125, 100, 100);
			array4[316][0] = color4;
			array4[317][0] = color4;
			array4[172][0] = new Color(163, 96, 0);
			array4[242][0] = new Color(5, 5, 5);
			array4[243][0] = new Color(5, 5, 5);
			array4[173][0] = new Color(94, 163, 46);
			array4[174][0] = new Color(117, 32, 59);
			array4[175][0] = new Color(20, 11, 203);
			array4[176][0] = new Color(74, 69, 88);
			array4[177][0] = new Color(60, 30, 30);
			array4[183][0] = new Color(111, 117, 135);
			array4[179][0] = new Color(111, 117, 135);
			array4[178][0] = new Color(111, 117, 135);
			array4[184][0] = new Color(25, 23, 54);
			array4[181][0] = new Color(25, 23, 54);
			array4[180][0] = new Color(25, 23, 54);
			array4[182][0] = new Color(74, 71, 129);
			array4[185][0] = new Color(52, 52, 52);
			array4[186][0] = new Color(38, 9, 66);
			array4[216][0] = new Color(158, 100, 64);
			array4[217][0] = new Color(62, 45, 75);
			array4[218][0] = new Color(57, 14, 12);
			array4[219][0] = new Color(96, 72, 133);
			array4[187][0] = new Color(149, 80, 51);
			array4[235][0] = new Color(140, 75, 48);
			array4[220][0] = new Color(67, 55, 80);
			array4[221][0] = new Color(64, 37, 29);
			array4[222][0] = new Color(70, 51, 91);
			array4[188][0] = new Color(82, 63, 80);
			array4[189][0] = new Color(65, 61, 77);
			array4[190][0] = new Color(64, 65, 92);
			array4[191][0] = new Color(76, 53, 84);
			array4[192][0] = new Color(144, 67, 52);
			array4[193][0] = new Color(149, 48, 48);
			array4[194][0] = new Color(111, 32, 36);
			array4[195][0] = new Color(147, 48, 55);
			array4[196][0] = new Color(97, 67, 51);
			array4[197][0] = new Color(112, 80, 62);
			array4[198][0] = new Color(88, 61, 46);
			array4[199][0] = new Color(127, 94, 76);
			array4[200][0] = new Color(143, 50, 123);
			array4[201][0] = new Color(136, 120, 131);
			array4[202][0] = new Color(219, 92, 143);
			array4[203][0] = new Color(113, 64, 150);
			array4[204][0] = new Color(74, 67, 60);
			array4[205][0] = new Color(60, 78, 59);
			array4[206][0] = new Color(0, 54, 21);
			array4[207][0] = new Color(74, 97, 72);
			array4[208][0] = new Color(40, 37, 35);
			array4[209][0] = new Color(77, 63, 66);
			array4[210][0] = new Color(111, 6, 6);
			array4[211][0] = new Color(88, 67, 59);
			array4[212][0] = new Color(88, 87, 80);
			array4[213][0] = new Color(71, 71, 67);
			array4[214][0] = new Color(76, 52, 60);
			array4[215][0] = new Color(89, 48, 59);
			array4[223][0] = new Color(51, 18, 4);
			array4[228][0] = new Color(160, 2, 75);
			array4[229][0] = new Color(100, 55, 164);
			array4[230][0] = new Color(0, 117, 101);
			array4[236][0] = new Color(127, 49, 44);
			array4[231][0] = new Color(110, 90, 78);
			array4[232][0] = new Color(47, 69, 75);
			array4[233][0] = new Color(91, 67, 70);
			array4[237][0] = new Color(200, 44, 18);
			array4[238][0] = new Color(24, 93, 66);
			array4[239][0] = new Color(160, 87, 234);
			array4[240][0] = new Color(6, 106, 255);
			array4[245][0] = new Color(102, 102, 102);
			array4[315][0] = new Color(181, 230, 29);
			array4[246][0] = new Color(61, 58, 78);
			array4[247][0] = new Color(52, 43, 45);
			array4[248][0] = new Color(81, 84, 101);
			array4[249][0] = new Color(85, 102, 103);
			array4[250][0] = new Color(52, 52, 52);
			array4[251][0] = new Color(52, 52, 52);
			array4[252][0] = new Color(52, 52, 52);
			array4[253][0] = new Color(52, 52, 52);
			array4[254][0] = new Color(52, 52, 52);
			array4[255][0] = new Color(52, 52, 52);
			array4[314][0] = new Color(52, 52, 52);
			array4[256][0] = new Color(40, 56, 50);
			array4[257][0] = new Color(49, 48, 36);
			array4[258][0] = new Color(43, 33, 32);
			array4[259][0] = new Color(31, 40, 49);
			array4[260][0] = new Color(48, 35, 52);
			array4[261][0] = new Color(88, 61, 46);
			array4[262][0] = new Color(55, 39, 26);
			array4[263][0] = new Color(39, 33, 26);
			array4[264][0] = new Color(43, 42, 68);
			array4[265][0] = new Color(30, 70, 80);
			array4[266][0] = new Color(78, 105, 135);
			array4[267][0] = new Color(51, 47, 96);
			array4[268][0] = new Color(101, 51, 51);
			array4[269][0] = new Color(62, 38, 41);
			array4[270][0] = new Color(59, 39, 22);
			array4[271][0] = new Color(59, 39, 22);
			array4[272][0] = new Color(111, 117, 135);
			array4[273][0] = new Color(25, 23, 54);
			array4[274][0] = new Color(52, 52, 52);
			array4[275][0] = new Color(149, 80, 51);
			array4[276][0] = new Color(82, 63, 80);
			array4[277][0] = new Color(65, 61, 77);
			array4[278][0] = new Color(64, 65, 92);
			array4[279][0] = new Color(76, 53, 84);
			array4[280][0] = new Color(144, 67, 52);
			array4[281][0] = new Color(149, 48, 48);
			array4[282][0] = new Color(111, 32, 36);
			array4[283][0] = new Color(147, 48, 55);
			array4[284][0] = new Color(97, 67, 51);
			array4[285][0] = new Color(112, 80, 62);
			array4[286][0] = new Color(88, 61, 46);
			array4[287][0] = new Color(127, 94, 76);
			array4[288][0] = new Color(143, 50, 123);
			array4[289][0] = new Color(136, 120, 131);
			array4[290][0] = new Color(219, 92, 143);
			array4[291][0] = new Color(113, 64, 150);
			array4[292][0] = new Color(74, 67, 60);
			array4[293][0] = new Color(60, 78, 59);
			array4[294][0] = new Color(0, 54, 21);
			array4[295][0] = new Color(74, 97, 72);
			array4[296][0] = new Color(40, 37, 35);
			array4[297][0] = new Color(77, 63, 66);
			array4[298][0] = new Color(111, 6, 6);
			array4[299][0] = new Color(88, 67, 59);
			array4[300][0] = new Color(88, 87, 80);
			array4[301][0] = new Color(71, 71, 67);
			array4[302][0] = new Color(76, 52, 60);
			array4[303][0] = new Color(89, 48, 59);
			array4[304][0] = new Color(158, 100, 64);
			array4[305][0] = new Color(62, 45, 75);
			array4[306][0] = new Color(57, 14, 12);
			array4[307][0] = new Color(96, 72, 133);
			array4[308][0] = new Color(67, 55, 80);
			array4[309][0] = new Color(64, 37, 29);
			array4[310][0] = new Color(70, 51, 91);
			array4[311][0] = new Color(51, 18, 4);
			array4[312][0] = new Color(78, 110, 51);
			array4[313][0] = new Color(78, 110, 51);
			array4[319][0] = new Color(105, 51, 108);
			array4[320][0] = new Color(75, 30, 15);
			array4[321][0] = new Color(91, 108, 130);
			array4[322][0] = new Color(91, 108, 130);
			Color[] array5 = new Color[256];
			Color color5 = new Color(50, 40, 255);
			Color color6 = new Color(145, 185, 255);
			for (int num6 = 0; num6 < array5.Length; num6++)
			{
				float num7 = (float)num6 / (float)array5.Length;
				float num8 = 1f - num7;
				array5[num6] = new Color((byte)((float)(int)color5.R * num8 + (float)(int)color6.R * num7), (byte)((float)(int)color5.G * num8 + (float)(int)color6.G * num7), (byte)((float)(int)color5.B * num8 + (float)(int)color6.B * num7));
			}
			Color[] array6 = new Color[256];
			Color color7 = new Color(88, 61, 46);
			Color color8 = new Color(37, 78, 123);
			for (int num9 = 0; num9 < array6.Length; num9++)
			{
				float num10 = (float)num9 / 255f;
				float num11 = 1f - num10;
				array6[num9] = new Color((byte)((float)(int)color7.R * num11 + (float)(int)color8.R * num10), (byte)((float)(int)color7.G * num11 + (float)(int)color8.G * num10), (byte)((float)(int)color7.B * num11 + (float)(int)color8.B * num10));
			}
			Color[] array7 = new Color[256];
			Color color9 = new Color(74, 67, 60);
			color8 = new Color(53, 70, 97);
			for (int num12 = 0; num12 < array7.Length; num12++)
			{
				float num13 = (float)num12 / 255f;
				float num14 = 1f - num13;
				array7[num12] = new Color((byte)((float)(int)color9.R * num14 + (float)(int)color8.R * num13), (byte)((float)(int)color9.G * num14 + (float)(int)color8.G * num13), (byte)((float)(int)color9.B * num14 + (float)(int)color8.B * num13));
			}
			Color color10 = new Color(50, 44, 38);
			int num15 = 0;
			tileOptionCounts = new int[693];
			for (int num16 = 0; num16 < 693; num16++)
			{
				Color[] array8 = array[num16];
				int num17;
				for (num17 = 0; num17 < 12 && !(array8[num17] == Color.Transparent); num17++)
				{
				}
				tileOptionCounts[num16] = num17;
				num15 += num17;
			}
			wallOptionCounts = new int[347];
			for (int num18 = 0; num18 < 347; num18++)
			{
				Color[] array9 = array4[num18];
				int num19;
				for (num19 = 0; num19 < 2 && !(array9[num19] == Color.Transparent); num19++)
				{
				}
				wallOptionCounts[num18] = num19;
				num15 += num19;
			}
			num15 += 774;
			colorLookup = new Color[num15];
			colorLookup[0] = Color.Transparent;
			ushort num20 = (tilePosition = 1);
			tileLookup = new ushort[693];
			for (int num21 = 0; num21 < 693; num21++)
			{
				if (tileOptionCounts[num21] > 0)
				{
					_ = array[num21];
					tileLookup[num21] = num20;
					for (int num22 = 0; num22 < tileOptionCounts[num21]; num22++)
					{
						colorLookup[num20] = array[num21][num22];
						num20 = (ushort)(num20 + 1);
					}
				}
				else
				{
					tileLookup[num21] = 0;
				}
			}
			wallPosition = num20;
			wallLookup = new ushort[347];
			wallRangeStart = num20;
			for (int num23 = 0; num23 < 347; num23++)
			{
				if (wallOptionCounts[num23] > 0)
				{
					_ = array4[num23];
					wallLookup[num23] = num20;
					for (int num24 = 0; num24 < wallOptionCounts[num23]; num24++)
					{
						colorLookup[num20] = array4[num23][num24];
						num20 = (ushort)(num20 + 1);
					}
				}
				else
				{
					wallLookup[num23] = 0;
				}
			}
			wallRangeEnd = num20;
			liquidPosition = num20;
			for (int num25 = 0; num25 < 4; num25++)
			{
				colorLookup[num20] = array3[num25];
				num20 = (ushort)(num20 + 1);
			}
			skyPosition = num20;
			for (int num26 = 0; num26 < 256; num26++)
			{
				colorLookup[num20] = array5[num26];
				num20 = (ushort)(num20 + 1);
			}
			dirtPosition = num20;
			for (int num27 = 0; num27 < 256; num27++)
			{
				colorLookup[num20] = array6[num27];
				num20 = (ushort)(num20 + 1);
			}
			rockPosition = num20;
			for (int num28 = 0; num28 < 256; num28++)
			{
				colorLookup[num20] = array7[num28];
				num20 = (ushort)(num20 + 1);
			}
			hellPosition = num20;
			colorLookup[num20] = color10;
			snowTypes = new ushort[6];
			snowTypes[0] = tileLookup[147];
			snowTypes[1] = tileLookup[161];
			snowTypes[2] = tileLookup[162];
			snowTypes[3] = tileLookup[163];
			snowTypes[4] = tileLookup[164];
			snowTypes[5] = tileLookup[200];
			Lang.BuildMapAtlas();
		}

		public static void ResetMapData()
		{
			numUpdateTile = 0;
		}

		public static bool HasOption(int tileType, int option)
		{
			return option < tileOptionCounts[tileType];
		}

		public static int TileToLookup(int tileType, int option)
		{
			return tileLookup[tileType] + option;
		}

		public static int LookupCount()
		{
			return colorLookup.Length;
		}

		private static void MapColor(ushort type, ref Color oldColor, byte colorType)
		{
			Color color = WorldGen.paintColor(colorType);
			float num = (float)(int)oldColor.R / 255f;
			float num2 = (float)(int)oldColor.G / 255f;
			float num3 = (float)(int)oldColor.B / 255f;
			if (num2 > num)
			{
				float num4 = num;
				num = num2;
				num2 = num4;
			}
			if (num3 > num)
			{
				float num5 = num;
				num = num3;
				num3 = num5;
			}
			switch (colorType)
			{
			case 29:
			{
				float num7 = num3 * 0.3f;
				oldColor.R = (byte)((float)(int)color.R * num7);
				oldColor.G = (byte)((float)(int)color.G * num7);
				oldColor.B = (byte)((float)(int)color.B * num7);
				break;
			}
			case 30:
				if (type >= wallRangeStart && type <= wallRangeEnd)
				{
					oldColor.R = (byte)((float)(255 - oldColor.R) * 0.5f);
					oldColor.G = (byte)((float)(255 - oldColor.G) * 0.5f);
					oldColor.B = (byte)((float)(255 - oldColor.B) * 0.5f);
				}
				else
				{
					oldColor.R = (byte)(255 - oldColor.R);
					oldColor.G = (byte)(255 - oldColor.G);
					oldColor.B = (byte)(255 - oldColor.B);
				}
				break;
			default:
			{
				float num6 = num;
				oldColor.R = (byte)((float)(int)color.R * num6);
				oldColor.G = (byte)((float)(int)color.G * num6);
				oldColor.B = (byte)((float)(int)color.B * num6);
				break;
			}
			}
		}

		public static Color GetMapTileXnaColor(ref MapTile tile)
		{
			Color oldColor = colorLookup[tile.Type];
			byte color = tile.Color;
			if (color > 0)
			{
				MapColor(tile.Type, ref oldColor, color);
			}
			if (tile.Light == byte.MaxValue)
			{
				return oldColor;
			}
			float num = (float)(int)tile.Light / 255f;
			oldColor.R = (byte)((float)(int)oldColor.R * num);
			oldColor.G = (byte)((float)(int)oldColor.G * num);
			oldColor.B = (byte)((float)(int)oldColor.B * num);
			return oldColor;
		}

		public static MapTile CreateMapTile(int i, int j, byte Light)
		{
			Tile tile = Main.tile[i, j];
			if (tile == null)
			{
				return default(MapTile);
			}
			int num = 0;
			int num2 = Light;
			_ = Main.Map[i, j];
			int num3 = 0;
			int baseOption = 0;
			if (tile.active())
			{
				int num4 = tile.type;
				num3 = tileLookup[num4];
				bool flag = tile.invisibleBlock();
				if (tile.fullbrightBlock() && !flag)
				{
					num2 = 255;
				}
				if (flag)
				{
					num3 = 0;
				}
				else if (num4 == 5)
				{
					if (WorldGen.IsThisAMushroomTree(i, j))
					{
						baseOption = 1;
					}
					num = tile.color();
				}
				else
				{
					switch (num4)
					{
					case 51:
						if ((i + j) % 2 == 0)
						{
							num3 = 0;
						}
						break;
					case 19:
						if (tile.frameY == 864)
						{
							num3 = 0;
						}
						break;
					case 184:
						if (tile.frameX / 22 == 10)
						{
							num4 = 627;
							num3 = tileLookup[num4];
						}
						break;
					}
					if (num3 != 0)
					{
						GetTileBaseOption(i, j, num4, tile, ref baseOption);
						num = ((num4 != 160) ? tile.color() : 0);
					}
				}
			}
			if (num3 == 0)
			{
				bool flag2 = tile.invisibleWall();
				if (tile.wall > 0 && tile.fullbrightWall() && !flag2)
				{
					num2 = 255;
				}
				if (tile.liquid > 32)
				{
					int num5 = tile.liquidType();
					num3 = liquidPosition + num5;
				}
				else if (!tile.invisibleWall() && tile.wall > 0 && tile.wall < 347)
				{
					int wall = tile.wall;
					num3 = wallLookup[wall];
					num = tile.wallColor();
					switch (wall)
					{
					case 21:
					case 88:
					case 89:
					case 90:
					case 91:
					case 92:
					case 93:
					case 168:
					case 241:
						num = 0;
						break;
					case 27:
						baseOption = i % 2;
						break;
					default:
						baseOption = 0;
						break;
					}
				}
			}
			if (num3 == 0)
			{
				if ((double)j < Main.worldSurface)
				{
					if (Main.remixWorld)
					{
						num2 = 5;
						num3 = 100;
					}
					else
					{
						int num6 = (byte)(255.0 * ((double)j / Main.worldSurface));
						num3 = skyPosition + num6;
						num2 = 255;
						num = 0;
					}
				}
				else if (j < Main.UnderworldLayer)
				{
					num = 0;
					byte b = 0;
					float num7 = Main.screenPosition.X / 16f - 5f;
					float num8 = (Main.screenPosition.X + (float)Main.screenWidth) / 16f + 5f;
					float num9 = Main.screenPosition.Y / 16f - 5f;
					float num10 = (Main.screenPosition.Y + (float)Main.screenHeight) / 16f + 5f;
					if (((float)i < num7 || (float)i > num8 || (float)j < num9 || (float)j > num10) && i > 40 && i < Main.maxTilesX - 40 && j > 40 && j < Main.maxTilesY - 40)
					{
						for (int k = i - 36; k <= i + 30; k += 10)
						{
							for (int l = j - 36; l <= j + 30; l += 10)
							{
								int type = Main.Map[k, l].Type;
								for (int m = 0; m < snowTypes.Length; m++)
								{
									if (snowTypes[m] == type)
									{
										b = byte.MaxValue;
										k = i + 31;
										l = j + 31;
										break;
									}
								}
							}
						}
					}
					else
					{
						float num11 = (float)Main.SceneMetrics.SnowTileCount / (float)SceneMetrics.SnowTileMax;
						num11 *= 255f;
						if (num11 > 255f)
						{
							num11 = 255f;
						}
						b = (byte)num11;
					}
					num3 = ((!((double)j < Main.rockLayer)) ? (rockPosition + b) : (dirtPosition + b));
				}
				else
				{
					num3 = hellPosition;
				}
			}
			return MapTile.Create((ushort)(num3 + baseOption), (byte)num2, (byte)num);
		}

		public static void GetTileBaseOption(int x, int y, int tileType, Tile tileCache, ref int baseOption)
		{
			switch (tileType)
			{
			case 160:
			case 627:
			case 628:
			case 692:
				baseOption = (x + y) % 9;
				break;
			case 461:
				if (Main.player[Main.myPlayer].ZoneCorrupt)
				{
					baseOption = 1;
				}
				else if (Main.player[Main.myPlayer].ZoneCrimson)
				{
					baseOption = 2;
				}
				else if (Main.player[Main.myPlayer].ZoneHallow)
				{
					baseOption = 3;
				}
				break;
			case 80:
			{
				WorldGen.GetCactusType(x, y, tileCache.frameX, tileCache.frameY, out var evil, out var good, out var crimson);
				if (evil)
				{
					baseOption = 1;
				}
				else if (good)
				{
					baseOption = 2;
				}
				else if (crimson)
				{
					baseOption = 3;
				}
				else
				{
					baseOption = 0;
				}
				break;
			}
			case 529:
			{
				int num11 = y + 1;
				WorldGen.GetBiomeInfluence(x, x, num11, num11, out var corruptCount2, out var crimsonCount2, out var hallowedCount2);
				int num12 = corruptCount2;
				if (num12 < crimsonCount2)
				{
					num12 = crimsonCount2;
				}
				if (num12 < hallowedCount2)
				{
					num12 = hallowedCount2;
				}
				int num13 = 0;
				num13 = (baseOption = ((corruptCount2 == 0 && crimsonCount2 == 0 && hallowedCount2 == 0) ? ((x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance) ? 1 : 0) : ((hallowedCount2 == num12) ? 2 : ((crimsonCount2 != num12) ? 4 : 3))));
				break;
			}
			case 530:
			{
				int num4 = y - tileCache.frameY % 36 / 18 + 2;
				int num5 = x - tileCache.frameX % 54 / 18;
				WorldGen.GetBiomeInfluence(num5, num5 + 3, num4, num4, out var corruptCount, out var crimsonCount, out var hallowedCount);
				int num6 = corruptCount;
				if (num6 < crimsonCount)
				{
					num6 = crimsonCount;
				}
				if (num6 < hallowedCount)
				{
					num6 = hallowedCount;
				}
				int num7 = 0;
				num7 = (baseOption = ((corruptCount != 0 || crimsonCount != 0 || hallowedCount != 0) ? ((hallowedCount == num6) ? 1 : ((crimsonCount != num6) ? 3 : 2)) : 0));
				break;
			}
			case 19:
			{
				int num9 = tileCache.frameY / 18;
				baseOption = 0;
				if (num9 == 48)
				{
					baseOption = 1;
				}
				break;
			}
			case 15:
			{
				int num2 = tileCache.frameY / 40;
				baseOption = 0;
				if (num2 == 1 || num2 == 20)
				{
					baseOption = 1;
				}
				break;
			}
			case 518:
			case 519:
				baseOption = tileCache.frameY / 18;
				break;
			case 4:
				if (tileCache.frameX < 66)
				{
					baseOption = 1;
				}
				baseOption = 0;
				break;
			case 572:
				baseOption = tileCache.frameY / 36;
				break;
			case 21:
			case 441:
				switch (tileCache.frameX / 36)
				{
				case 1:
				case 2:
				case 10:
				case 13:
				case 15:
					baseOption = 1;
					break;
				case 3:
				case 4:
					baseOption = 2;
					break;
				case 6:
					baseOption = 3;
					break;
				case 11:
				case 17:
					baseOption = 4;
					break;
				default:
					baseOption = 0;
					break;
				}
				break;
			case 467:
			case 468:
			{
				int num = tileCache.frameX / 36;
				switch (num)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
					baseOption = num;
					break;
				case 12:
				case 13:
					baseOption = 10;
					break;
				case 14:
					baseOption = 0;
					break;
				case 15:
					baseOption = 10;
					break;
				case 16:
					baseOption = 3;
					break;
				default:
					baseOption = 0;
					break;
				}
				break;
			}
			case 560:
			{
				int num = tileCache.frameX / 36;
				if ((uint)num <= 2u)
				{
					baseOption = num;
				}
				else
				{
					baseOption = 0;
				}
				break;
			}
			case 28:
			case 653:
				if (tileCache.frameY < 144)
				{
					baseOption = 0;
				}
				else if (tileCache.frameY < 252)
				{
					baseOption = 1;
				}
				else if (tileCache.frameY < 360 || (tileCache.frameY > 900 && tileCache.frameY < 1008))
				{
					baseOption = 2;
				}
				else if (tileCache.frameY < 468)
				{
					baseOption = 3;
				}
				else if (tileCache.frameY < 576)
				{
					baseOption = 4;
				}
				else if (tileCache.frameY < 684)
				{
					baseOption = 5;
				}
				else if (tileCache.frameY < 792)
				{
					baseOption = 6;
				}
				else if (tileCache.frameY < 898)
				{
					baseOption = 8;
				}
				else if (tileCache.frameY < 1006)
				{
					baseOption = 7;
				}
				else if (tileCache.frameY < 1114)
				{
					baseOption = 0;
				}
				else if (tileCache.frameY < 1222)
				{
					baseOption = 3;
				}
				else
				{
					baseOption = 7;
				}
				break;
			case 27:
				if (tileCache.frameY < 34)
				{
					baseOption = 1;
				}
				else
				{
					baseOption = 0;
				}
				break;
			case 31:
				if (tileCache.frameX >= 36)
				{
					baseOption = 1;
				}
				else
				{
					baseOption = 0;
				}
				break;
			case 26:
				if (tileCache.frameX >= 54)
				{
					baseOption = 1;
				}
				else
				{
					baseOption = 0;
				}
				break;
			case 137:
				switch (tileCache.frameY / 18)
				{
				default:
					baseOption = 0;
					break;
				case 1:
				case 2:
				case 3:
				case 4:
					baseOption = 1;
					break;
				case 5:
					baseOption = 2;
					break;
				}
				break;
			case 82:
			case 83:
			case 84:
				if (tileCache.frameX < 18)
				{
					baseOption = 0;
				}
				else if (tileCache.frameX < 36)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX < 54)
				{
					baseOption = 2;
				}
				else if (tileCache.frameX < 72)
				{
					baseOption = 3;
				}
				else if (tileCache.frameX < 90)
				{
					baseOption = 4;
				}
				else if (tileCache.frameX < 108)
				{
					baseOption = 5;
				}
				else
				{
					baseOption = 6;
				}
				break;
			case 591:
				baseOption = tileCache.frameX / 36;
				break;
			case 105:
				if (tileCache.frameX >= 1548 && tileCache.frameX <= 1654)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX >= 1656 && tileCache.frameX <= 1798)
				{
					baseOption = 2;
				}
				else
				{
					baseOption = 0;
				}
				break;
			case 133:
				if (tileCache.frameX < 52)
				{
					baseOption = 0;
				}
				else
				{
					baseOption = 1;
				}
				break;
			case 134:
				if (tileCache.frameX < 28)
				{
					baseOption = 0;
				}
				else
				{
					baseOption = 1;
				}
				break;
			case 149:
				baseOption = y % 3;
				break;
			case 165:
				if (tileCache.frameX < 54)
				{
					baseOption = 0;
				}
				else if (tileCache.frameX < 106)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX >= 216)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX < 162)
				{
					baseOption = 2;
				}
				else
				{
					baseOption = 3;
				}
				break;
			case 178:
				if (tileCache.frameX < 18)
				{
					baseOption = 0;
				}
				else if (tileCache.frameX < 36)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX < 54)
				{
					baseOption = 2;
				}
				else if (tileCache.frameX < 72)
				{
					baseOption = 3;
				}
				else if (tileCache.frameX < 90)
				{
					baseOption = 4;
				}
				else if (tileCache.frameX < 108)
				{
					baseOption = 5;
				}
				else
				{
					baseOption = 6;
				}
				break;
			case 184:
				if (tileCache.frameX < 22)
				{
					baseOption = 0;
				}
				else if (tileCache.frameX < 44)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX < 66)
				{
					baseOption = 2;
				}
				else if (tileCache.frameX < 88)
				{
					baseOption = 3;
				}
				else if (tileCache.frameX < 110)
				{
					baseOption = 4;
				}
				else if (tileCache.frameX < 132)
				{
					baseOption = 5;
				}
				else if (tileCache.frameX < 154)
				{
					baseOption = 6;
				}
				else if (tileCache.frameX < 176)
				{
					baseOption = 7;
				}
				else if (tileCache.frameX < 198)
				{
					baseOption = 8;
				}
				else if (tileCache.frameX < 220)
				{
					baseOption = 9;
				}
				else if (tileCache.frameX < 242)
				{
					baseOption = 10;
				}
				break;
			case 650:
			{
				int num = tileCache.frameX / 36;
				int num10 = tileCache.frameY / 18 - 1;
				num += num10 * 18;
				if (num < 6 || num == 19 || num == 20 || num == 21 || num == 22 || num == 23 || num == 24 || num == 33 || num == 38 || num == 39 || num == 40)
				{
					baseOption = 0;
				}
				else if (num < 16)
				{
					baseOption = 2;
				}
				else if (num < 19 || num == 31 || num == 32)
				{
					baseOption = 1;
				}
				else if (num < 31)
				{
					baseOption = 3;
				}
				else if (num < 38)
				{
					baseOption = 4;
				}
				else if (num < 59)
				{
					baseOption = 0;
				}
				else if (num < 62)
				{
					baseOption = 1;
				}
				break;
			}
			case 649:
			{
				int num = tileCache.frameX / 18;
				if (num < 6 || num == 28 || num == 29 || num == 30 || num == 31 || num == 32)
				{
					baseOption = 0;
				}
				else if (num < 12 || num == 33 || num == 34 || num == 35)
				{
					baseOption = 1;
				}
				else if (num < 28)
				{
					baseOption = 2;
				}
				else if (num < 48)
				{
					baseOption = 3;
				}
				else if (num < 54)
				{
					baseOption = 4;
				}
				else if (num < 72)
				{
					baseOption = 0;
				}
				else if (num == 72)
				{
					baseOption = 1;
				}
				break;
			}
			case 185:
			{
				int num;
				if (tileCache.frameY < 18)
				{
					num = tileCache.frameX / 18;
					if (num < 6 || num == 28 || num == 29 || num == 30 || num == 31 || num == 32)
					{
						baseOption = 0;
					}
					else if (num < 12 || num == 33 || num == 34 || num == 35)
					{
						baseOption = 1;
					}
					else if (num < 28)
					{
						baseOption = 2;
					}
					else if (num < 48)
					{
						baseOption = 3;
					}
					else if (num < 54)
					{
						baseOption = 4;
					}
					else if (num < 72)
					{
						baseOption = 0;
					}
					else if (num == 72)
					{
						baseOption = 1;
					}
					break;
				}
				num = tileCache.frameX / 36;
				int num8 = tileCache.frameY / 18 - 1;
				num += num8 * 18;
				if (num < 6 || num == 19 || num == 20 || num == 21 || num == 22 || num == 23 || num == 24 || num == 33 || num == 38 || num == 39 || num == 40)
				{
					baseOption = 0;
				}
				else if (num < 16)
				{
					baseOption = 2;
				}
				else if (num < 19 || num == 31 || num == 32)
				{
					baseOption = 1;
				}
				else if (num < 31)
				{
					baseOption = 3;
				}
				else if (num < 38)
				{
					baseOption = 4;
				}
				else if (num < 59)
				{
					baseOption = 0;
				}
				else if (num < 62)
				{
					baseOption = 1;
				}
				break;
			}
			case 186:
			case 647:
			{
				int num = tileCache.frameX / 54;
				if (num < 7)
				{
					baseOption = 2;
				}
				else if (num < 22 || num == 33 || num == 34 || num == 35)
				{
					baseOption = 0;
				}
				else if (num < 25)
				{
					baseOption = 1;
				}
				else if (num == 25)
				{
					baseOption = 5;
				}
				else if (num < 32)
				{
					baseOption = 3;
				}
				break;
			}
			case 187:
			case 648:
			{
				int num = tileCache.frameX / 54;
				int num3 = tileCache.frameY / 36;
				num += num3 * 36;
				if (num < 3 || num == 14 || num == 15 || num == 16)
				{
					baseOption = 0;
				}
				else if (num < 6)
				{
					baseOption = 6;
				}
				else if (num < 9)
				{
					baseOption = 7;
				}
				else if (num < 14)
				{
					baseOption = 4;
				}
				else if (num < 18)
				{
					baseOption = 4;
				}
				else if (num < 23)
				{
					baseOption = 8;
				}
				else if (num < 25)
				{
					baseOption = 0;
				}
				else if (num < 29)
				{
					baseOption = 1;
				}
				else if (num < 47)
				{
					baseOption = 0;
				}
				else if (num < 50)
				{
					baseOption = 1;
				}
				else if (num < 52)
				{
					baseOption = 10;
				}
				else if (num < 55)
				{
					baseOption = 2;
				}
				break;
			}
			case 227:
				baseOption = tileCache.frameX / 34;
				break;
			case 129:
				if (tileCache.frameX >= 324)
				{
					baseOption = 1;
				}
				else
				{
					baseOption = 0;
				}
				break;
			case 240:
			{
				int num = tileCache.frameX / 54;
				int num14 = tileCache.frameY / 54;
				num += num14 * 36;
				if ((num < 0 || num > 11) && (num < 47 || num > 53))
				{
					switch (num)
					{
					case 72:
					case 73:
					case 75:
						break;
					case 12:
					case 13:
					case 14:
					case 15:
						baseOption = 1;
						return;
					default:
						switch (num)
						{
						case 16:
						case 17:
							baseOption = 2;
							break;
						default:
							if (num < 63 || num > 71)
							{
								switch (num)
								{
								case 74:
								case 76:
								case 77:
								case 78:
								case 79:
								case 80:
								case 81:
								case 82:
								case 83:
								case 84:
								case 85:
								case 86:
								case 87:
								case 88:
								case 89:
								case 90:
								case 91:
								case 92:
									break;
								default:
									if (num >= 41 && num <= 45)
									{
										baseOption = 3;
									}
									else if (num == 46)
									{
										baseOption = 4;
									}
									return;
								}
							}
							goto case 18;
						case 18:
						case 19:
						case 20:
						case 21:
						case 22:
						case 23:
						case 24:
						case 25:
						case 26:
						case 27:
						case 28:
						case 29:
						case 30:
						case 31:
						case 32:
						case 33:
						case 34:
						case 35:
							baseOption = 1;
							break;
						}
						return;
					}
				}
				baseOption = 0;
				break;
			}
			case 242:
			{
				int num = tileCache.frameY / 72;
				if (tileCache.frameX / 106 == 0 && num >= 22 && num <= 24)
				{
					baseOption = 1;
				}
				else
				{
					baseOption = 0;
				}
				break;
			}
			case 440:
			{
				int num = tileCache.frameX / 54;
				if (num > 6)
				{
					num = 6;
				}
				baseOption = num;
				break;
			}
			case 457:
			{
				int num = tileCache.frameX / 36;
				if (num > 4)
				{
					num = 4;
				}
				baseOption = num;
				break;
			}
			case 453:
			{
				int num = tileCache.frameX / 36;
				if (num > 2)
				{
					num = 2;
				}
				baseOption = num;
				break;
			}
			case 419:
			{
				int num = tileCache.frameX / 18;
				if (num > 2)
				{
					num = 2;
				}
				baseOption = num;
				break;
			}
			case 428:
			{
				int num = tileCache.frameY / 18;
				if (num > 3)
				{
					num = 3;
				}
				baseOption = num;
				break;
			}
			case 420:
			{
				int num = tileCache.frameY / 18;
				if (num > 5)
				{
					num = 5;
				}
				baseOption = num;
				break;
			}
			case 423:
			{
				int num = tileCache.frameY / 18;
				if (num > 6)
				{
					num = 6;
				}
				baseOption = num;
				break;
			}
			case 493:
				if (tileCache.frameX < 18)
				{
					baseOption = 0;
				}
				else if (tileCache.frameX < 36)
				{
					baseOption = 1;
				}
				else if (tileCache.frameX < 54)
				{
					baseOption = 2;
				}
				else if (tileCache.frameX < 72)
				{
					baseOption = 3;
				}
				else if (tileCache.frameX < 90)
				{
					baseOption = 4;
				}
				else
				{
					baseOption = 5;
				}
				break;
			case 548:
				if (tileCache.frameX / 54 < 7)
				{
					baseOption = 0;
				}
				else
				{
					baseOption = 1;
				}
				break;
			case 597:
			{
				int num = tileCache.frameX / 54;
				if ((uint)num <= 8u)
				{
					baseOption = num;
				}
				else
				{
					baseOption = 0;
				}
				break;
			}
			default:
				baseOption = 0;
				break;
			}
		}

		public static void SaveMap()
		{
			if ((Main.ActivePlayerFileData.IsCloudSave && SocialAPI.Cloud == null) || !Main.mapEnabled || !Monitor.TryEnter(IOLock))
			{
				return;
			}
			try
			{
				FileUtilities.ProtectedInvoke(InternalSaveMap);
			}
			catch (Exception value)
			{
				using StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true);
				streamWriter.WriteLine(DateTime.Now);
				streamWriter.WriteLine(value);
				streamWriter.WriteLine("");
			}
			finally
			{
				Monitor.Exit(IOLock);
			}
		}

		private static void InternalSaveMap()
		{
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			bool isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
			string text = Main.playerPathName.Substring(0, Main.playerPathName.Length - 4);
			if (!isCloudSave)
			{
				Utils.TryCreatingDirectory(text);
			}
			string text2 = text;
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			text = text2 + directorySeparatorChar;
			text = ((!Main.ActiveWorldFileData.UseGuidAsMapName) ? (text + Main.worldID + ".map") : string.Concat(text, Main.ActiveWorldFileData.UniqueId, ".map"));
			new Stopwatch().Start();
			if (!Main.gameMenu)
			{
				noStatusText = true;
			}
			using (MemoryStream memoryStream = new MemoryStream(4000))
			{
				using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				DeflateStream val = new DeflateStream((Stream)memoryStream, (CompressionMode)0);
				try
				{
					int num = 0;
					byte[] array = new byte[16384];
					binaryWriter.Write(269);
					Main.MapFileMetadata.IncrementAndWrite(binaryWriter);
					binaryWriter.Write(Main.worldName);
					binaryWriter.Write(Main.worldID);
					binaryWriter.Write(Main.maxTilesY);
					binaryWriter.Write(Main.maxTilesX);
					binaryWriter.Write((short)693);
					binaryWriter.Write((short)347);
					binaryWriter.Write((short)4);
					binaryWriter.Write((short)256);
					binaryWriter.Write((short)256);
					binaryWriter.Write((short)256);
					byte b = 1;
					byte b2 = 0;
					int i;
					for (i = 0; i < 693; i++)
					{
						if (tileOptionCounts[i] != 1)
						{
							b2 = (byte)(b2 | b);
						}
						if (b == 128)
						{
							binaryWriter.Write(b2);
							b2 = 0;
							b = 1;
						}
						else
						{
							b = (byte)(b << 1);
						}
					}
					if (b != 1)
					{
						binaryWriter.Write(b2);
					}
					i = 0;
					b = 1;
					b2 = 0;
					for (; i < 347; i++)
					{
						if (wallOptionCounts[i] != 1)
						{
							b2 = (byte)(b2 | b);
						}
						if (b == 128)
						{
							binaryWriter.Write(b2);
							b2 = 0;
							b = 1;
						}
						else
						{
							b = (byte)(b << 1);
						}
					}
					if (b != 1)
					{
						binaryWriter.Write(b2);
					}
					for (i = 0; i < 693; i++)
					{
						if (tileOptionCounts[i] != 1)
						{
							binaryWriter.Write((byte)tileOptionCounts[i]);
						}
					}
					for (i = 0; i < 347; i++)
					{
						if (wallOptionCounts[i] != 1)
						{
							binaryWriter.Write((byte)wallOptionCounts[i]);
						}
					}
					binaryWriter.Flush();
					for (int j = 0; j < Main.maxTilesY; j++)
					{
						if (!noStatusText)
						{
							float num2 = (float)j / (float)Main.maxTilesY;
							Main.statusText = Lang.gen[66].Value + " " + (int)(num2 * 100f + 1f) + "%";
						}
						int num3;
						for (num3 = 0; num3 < Main.maxTilesX; num3++)
						{
							MapTile mapTile = Main.Map[num3, j];
							byte b4;
							byte b3;
							byte b5 = (b4 = (b3 = 0));
							int num4 = 0;
							bool flag = true;
							bool flag2 = true;
							int num5 = 0;
							int num6 = 0;
							byte b6 = 0;
							int num7;
							ushort num8;
							if (mapTile.Light <= 18)
							{
								flag2 = false;
								flag = false;
								num7 = 0;
								num8 = 0;
								num4 = 0;
								int num9 = num3 + 1;
								int num10 = Main.maxTilesX - num3 - 1;
								while (num10 > 0 && Main.Map[num9, j].Light <= 18)
								{
									num4++;
									num10--;
									num9++;
								}
							}
							else
							{
								b6 = mapTile.Color;
								num8 = mapTile.Type;
								if (num8 < wallPosition)
								{
									num7 = 1;
									num8 = (ushort)(num8 - tilePosition);
								}
								else if (num8 < liquidPosition)
								{
									num7 = 2;
									num8 = (ushort)(num8 - wallPosition);
								}
								else if (num8 < skyPosition)
								{
									int num11 = num8 - liquidPosition;
									if (num11 == 3)
									{
										b4 = (byte)(b4 | 0x40u);
										num11 = 0;
									}
									num7 = 3 + num11;
									flag = false;
								}
								else if (num8 < dirtPosition)
								{
									num7 = 6;
									flag2 = false;
									flag = false;
								}
								else if (num8 < hellPosition)
								{
									num7 = 7;
									num8 = ((num8 >= rockPosition) ? ((ushort)(num8 - rockPosition)) : ((ushort)(num8 - dirtPosition)));
								}
								else
								{
									num7 = 6;
									flag = false;
								}
								if (mapTile.Light == byte.MaxValue)
								{
									flag2 = false;
								}
								if (flag2)
								{
									num4 = 0;
									int num9 = num3 + 1;
									int num10 = Main.maxTilesX - num3 - 1;
									num5 = num9;
									while (num10 > 0)
									{
										MapTile other = Main.Map[num9, j];
										if (mapTile.EqualsWithoutLight(ref other))
										{
											num10--;
											num4++;
											num9++;
											continue;
										}
										num6 = num9;
										break;
									}
								}
								else
								{
									num4 = 0;
									int num9 = num3 + 1;
									int num10 = Main.maxTilesX - num3 - 1;
									while (num10 > 0)
									{
										MapTile other2 = Main.Map[num9, j];
										if (!mapTile.Equals(ref other2))
										{
											break;
										}
										num10--;
										num4++;
										num9++;
									}
								}
							}
							if (b6 > 0)
							{
								b4 = (byte)(b4 | (byte)(b6 << 1));
							}
							if (b3 != 0)
							{
								b4 = (byte)(b4 | 1u);
							}
							if (b4 != 0)
							{
								b5 = (byte)(b5 | 1u);
							}
							b5 = (byte)(b5 | (byte)(num7 << 1));
							if (flag && num8 > 255)
							{
								b5 = (byte)(b5 | 0x10u);
							}
							if (flag2)
							{
								b5 = (byte)(b5 | 0x20u);
							}
							if (num4 > 0)
							{
								b5 = ((num4 <= 255) ? ((byte)(b5 | 0x40u)) : ((byte)(b5 | 0x80u)));
							}
							array[num] = b5;
							num++;
							if (b4 != 0)
							{
								array[num] = b4;
								num++;
							}
							if (b3 != 0)
							{
								array[num] = b3;
								num++;
							}
							if (flag)
							{
								array[num] = (byte)num8;
								num++;
								if (num8 > 255)
								{
									array[num] = (byte)(num8 >> 8);
									num++;
								}
							}
							if (flag2)
							{
								array[num] = mapTile.Light;
								num++;
							}
							if (num4 > 0)
							{
								array[num] = (byte)num4;
								num++;
								if (num4 > 255)
								{
									array[num] = (byte)(num4 >> 8);
									num++;
								}
							}
							for (int k = num5; k < num6; k++)
							{
								array[num] = Main.Map[k, j].Light;
								num++;
							}
							num3 += num4;
							if (num >= 4096)
							{
								((Stream)(object)val).Write(array, 0, num);
								num = 0;
							}
						}
					}
					if (num > 0)
					{
						((Stream)(object)val).Write(array, 0, num);
					}
					((Stream)(object)val).Dispose();
					FileUtilities.WriteAllBytes(text, memoryStream.ToArray(), isCloudSave);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			noStatusText = false;
		}

		public static void LoadMapVersion1(BinaryReader fileIO, int release)
		{
			string text = fileIO.ReadString();
			int num = fileIO.ReadInt32();
			int num2 = fileIO.ReadInt32();
			int num3 = fileIO.ReadInt32();
			if (text != Main.worldName || num != Main.worldID || num3 != Main.maxTilesX || num2 != Main.maxTilesY)
			{
				throw new Exception("Map meta-data is invalid.");
			}
			OldMapHelper oldMapHelper = default(OldMapHelper);
			for (int i = 0; i < Main.maxTilesX; i++)
			{
				float num4 = (float)i / (float)Main.maxTilesX;
				Main.statusText = Lang.gen[67].Value + " " + (int)(num4 * 100f + 1f) + "%";
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					if (fileIO.ReadBoolean())
					{
						int num5 = ((release <= 77) ? fileIO.ReadByte() : fileIO.ReadUInt16());
						byte b = fileIO.ReadByte();
						oldMapHelper.misc = fileIO.ReadByte();
						if (release >= 50)
						{
							oldMapHelper.misc2 = fileIO.ReadByte();
						}
						else
						{
							oldMapHelper.misc2 = 0;
						}
						bool flag = false;
						int num6 = oldMapHelper.option();
						int num7;
						if (oldMapHelper.active())
						{
							num7 = num6 + tileLookup[num5];
						}
						else if (oldMapHelper.water())
						{
							num7 = liquidPosition;
						}
						else if (oldMapHelper.lava())
						{
							num7 = liquidPosition + 1;
						}
						else if (oldMapHelper.honey())
						{
							num7 = liquidPosition + 2;
						}
						else if (oldMapHelper.wall())
						{
							num7 = num6 + wallLookup[num5];
						}
						else if ((double)j < Main.worldSurface)
						{
							flag = true;
							int num8 = (byte)(256.0 * ((double)j / Main.worldSurface));
							num7 = skyPosition + num8;
						}
						else if ((double)j < Main.rockLayer)
						{
							flag = true;
							if (num5 > 255)
							{
								num5 = 255;
							}
							num7 = num5 + dirtPosition;
						}
						else if (j < Main.UnderworldLayer)
						{
							flag = true;
							if (num5 > 255)
							{
								num5 = 255;
							}
							num7 = num5 + rockPosition;
						}
						else
						{
							num7 = hellPosition;
						}
						MapTile tile = MapTile.Create((ushort)num7, b, 0);
						Main.Map.SetTile(i, j, ref tile);
						int num9 = fileIO.ReadInt16();
						if (b == byte.MaxValue)
						{
							while (num9 > 0)
							{
								num9--;
								j++;
								if (flag)
								{
									if ((double)j < Main.worldSurface)
									{
										flag = true;
										int num10 = (byte)(256.0 * ((double)j / Main.worldSurface));
										num7 = skyPosition + num10;
									}
									else if ((double)j < Main.rockLayer)
									{
										flag = true;
										num7 = num5 + dirtPosition;
									}
									else if (j < Main.UnderworldLayer)
									{
										flag = true;
										num7 = num5 + rockPosition;
									}
									else
									{
										flag = true;
										num7 = hellPosition;
									}
									tile.Type = (ushort)num7;
								}
								Main.Map.SetTile(i, j, ref tile);
							}
							continue;
						}
						while (num9 > 0)
						{
							j++;
							num9--;
							b = fileIO.ReadByte();
							if (b <= 18)
							{
								continue;
							}
							tile.Light = b;
							if (flag)
							{
								if ((double)j < Main.worldSurface)
								{
									flag = true;
									int num11 = (byte)(256.0 * ((double)j / Main.worldSurface));
									num7 = skyPosition + num11;
								}
								else if ((double)j < Main.rockLayer)
								{
									flag = true;
									num7 = num5 + dirtPosition;
								}
								else if (j < Main.UnderworldLayer)
								{
									flag = true;
									num7 = num5 + rockPosition;
								}
								else
								{
									flag = true;
									num7 = hellPosition;
								}
								tile.Type = (ushort)num7;
							}
							Main.Map.SetTile(i, j, ref tile);
						}
					}
					else
					{
						int num12 = fileIO.ReadInt16();
						j += num12;
					}
				}
			}
		}

		public static void LoadMapVersion2(BinaryReader fileIO, int release)
		{
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Expected O, but got Unknown
			if (release >= 135)
			{
				Main.MapFileMetadata = FileMetadata.Read(fileIO, FileType.Map);
			}
			else
			{
				Main.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
			}
			fileIO.ReadString();
			int num = fileIO.ReadInt32();
			int num2 = fileIO.ReadInt32();
			int num3 = fileIO.ReadInt32();
			if (num != Main.worldID || num3 != Main.maxTilesX || num2 != Main.maxTilesY)
			{
				throw new Exception("Map meta-data is invalid.");
			}
			short num4 = fileIO.ReadInt16();
			short num5 = fileIO.ReadInt16();
			short num6 = fileIO.ReadInt16();
			short num7 = fileIO.ReadInt16();
			short num8 = fileIO.ReadInt16();
			short num9 = fileIO.ReadInt16();
			bool[] array = new bool[num4];
			byte b = 0;
			byte b2 = 128;
			for (int i = 0; i < num4; i++)
			{
				if (b2 == 128)
				{
					b = fileIO.ReadByte();
					b2 = 1;
				}
				else
				{
					b2 = (byte)(b2 << 1);
				}
				if ((b & b2) == b2)
				{
					array[i] = true;
				}
			}
			bool[] array2 = new bool[num5];
			b = 0;
			b2 = 128;
			for (int i = 0; i < num5; i++)
			{
				if (b2 == 128)
				{
					b = fileIO.ReadByte();
					b2 = 1;
				}
				else
				{
					b2 = (byte)(b2 << 1);
				}
				if ((b & b2) == b2)
				{
					array2[i] = true;
				}
			}
			byte[] array3 = new byte[num4];
			ushort num10 = 0;
			for (int i = 0; i < num4; i++)
			{
				if (array[i])
				{
					array3[i] = fileIO.ReadByte();
				}
				else
				{
					array3[i] = 1;
				}
				num10 = (ushort)(num10 + array3[i]);
			}
			byte[] array4 = new byte[num5];
			ushort num11 = 0;
			for (int i = 0; i < num5; i++)
			{
				if (array2[i])
				{
					array4[i] = fileIO.ReadByte();
				}
				else
				{
					array4[i] = 1;
				}
				num11 = (ushort)(num11 + array4[i]);
			}
			ushort[] array5 = new ushort[num10 + num11 + num6 + num7 + num8 + num9 + 2];
			array5[0] = 0;
			ushort num12 = 1;
			ushort num13 = 1;
			ushort num14 = num13;
			for (int i = 0; i < 693; i++)
			{
				if (i < num4)
				{
					int num15 = array3[i];
					int num16 = tileOptionCounts[i];
					for (int j = 0; j < num16; j++)
					{
						if (j < num15)
						{
							array5[num13] = num12;
							num13 = (ushort)(num13 + 1);
						}
						num12 = (ushort)(num12 + 1);
					}
				}
				else
				{
					num12 = (ushort)(num12 + (ushort)tileOptionCounts[i]);
				}
			}
			ushort num17 = num13;
			for (int i = 0; i < 347; i++)
			{
				if (i < num5)
				{
					int num18 = array4[i];
					int num19 = wallOptionCounts[i];
					for (int k = 0; k < num19; k++)
					{
						if (k < num18)
						{
							array5[num13] = num12;
							num13 = (ushort)(num13 + 1);
						}
						num12 = (ushort)(num12 + 1);
					}
				}
				else
				{
					num12 = (ushort)(num12 + (ushort)wallOptionCounts[i]);
				}
			}
			ushort num20 = num13;
			for (int i = 0; i < 4; i++)
			{
				if (i < num6)
				{
					array5[num13] = num12;
					num13 = (ushort)(num13 + 1);
				}
				num12 = (ushort)(num12 + 1);
			}
			ushort num21 = num13;
			for (int i = 0; i < 256; i++)
			{
				if (i < num7)
				{
					array5[num13] = num12;
					num13 = (ushort)(num13 + 1);
				}
				num12 = (ushort)(num12 + 1);
			}
			ushort num22 = num13;
			for (int i = 0; i < 256; i++)
			{
				if (i < num8)
				{
					array5[num13] = num12;
					num13 = (ushort)(num13 + 1);
				}
				num12 = (ushort)(num12 + 1);
			}
			ushort num23 = num13;
			for (int i = 0; i < 256; i++)
			{
				if (i < num9)
				{
					array5[num13] = num12;
					num13 = (ushort)(num13 + 1);
				}
				num12 = (ushort)(num12 + 1);
			}
			ushort num24 = num13;
			array5[num13] = num12;
			BinaryReader binaryReader = ((release < 93) ? new BinaryReader(fileIO.BaseStream) : new BinaryReader((Stream)new DeflateStream(fileIO.BaseStream, (CompressionMode)1)));
			for (int l = 0; l < Main.maxTilesY; l++)
			{
				float num25 = (float)l / (float)Main.maxTilesY;
				Main.statusText = Lang.gen[67].Value + " " + (int)(num25 * 100f + 1f) + "%";
				for (int m = 0; m < Main.maxTilesX; m++)
				{
					byte b3 = binaryReader.ReadByte();
					byte b4 = (byte)(((b3 & 1) == 1) ? binaryReader.ReadByte() : 0);
					if ((b4 & 1) == 1)
					{
						binaryReader.ReadByte();
					}
					byte b5 = (byte)((b3 & 0xE) >> 1);
					bool flag;
					switch (b5)
					{
					case 0:
						flag = false;
						break;
					case 1:
					case 2:
					case 7:
						flag = true;
						break;
					case 3:
					case 4:
					case 5:
						flag = false;
						break;
					case 6:
						flag = false;
						break;
					default:
						flag = false;
						break;
					}
					ushort num26 = (ushort)(flag ? (((b3 & 0x10) != 16) ? binaryReader.ReadByte() : binaryReader.ReadUInt16()) : 0);
					byte b6 = (((b3 & 0x20) != 32) ? byte.MaxValue : binaryReader.ReadByte());
					int num27 = (byte)((b3 & 0xC0) >> 6) switch
					{
						0 => 0, 
						1 => binaryReader.ReadByte(), 
						2 => binaryReader.ReadInt16(), 
						_ => 0, 
					};
					switch (b5)
					{
					case 0:
						m += num27;
						continue;
					case 1:
						num26 = (ushort)(num26 + num14);
						break;
					case 2:
						num26 = (ushort)(num26 + num17);
						break;
					case 3:
					case 4:
					case 5:
					{
						int num28 = b5 - 3;
						if ((b4 & 0x40) == 64)
						{
							num28 = 3;
						}
						num26 = (ushort)(num26 + (ushort)(num20 + num28));
						break;
					}
					case 6:
						if ((double)l < Main.worldSurface)
						{
							ushort num29 = (ushort)((double)num7 * ((double)l / Main.worldSurface));
							num26 = (ushort)(num26 + (ushort)(num21 + num29));
						}
						else
						{
							num26 = num24;
						}
						break;
					case 7:
						num26 = ((!((double)l < Main.rockLayer)) ? ((ushort)(num26 + num23)) : ((ushort)(num26 + num22)));
						break;
					}
					MapTile tile = MapTile.Create(array5[num26], b6, (byte)((uint)(b4 >> 1) & 0x1Fu));
					Main.Map.SetTile(m, l, ref tile);
					if (b6 == byte.MaxValue)
					{
						while (num27 > 0)
						{
							m++;
							Main.Map.SetTile(m, l, ref tile);
							num27--;
						}
						continue;
					}
					while (num27 > 0)
					{
						m++;
						tile = tile.WithLight(binaryReader.ReadByte());
						Main.Map.SetTile(m, l, ref tile);
						num27--;
					}
				}
			}
			binaryReader.Close();
		}
	}
}
