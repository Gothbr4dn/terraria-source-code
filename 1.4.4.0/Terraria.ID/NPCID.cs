using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Reflection;
using Terraria.DataStructures;

namespace Terraria.ID
{
	public class NPCID
	{
		public static class Sets
		{
			public struct NPCBestiaryDrawModifiers
			{
				public Vector2 Position;

				public float? PortraitPositionXOverride;

				public float? PortraitPositionYOverride;

				public float Rotation;

				public float Scale;

				public float? PortraitScale;

				public bool Hide;

				public bool IsWet;

				public int? Frame;

				public int? Direction;

				public int? SpriteDirection;

				public float Velocity;

				public string CustomTexturePath;

				public NPCBestiaryDrawModifiers(int seriouslyWhyCantStructsHaveParameterlessConstructors)
				{
					Position = default(Vector2);
					Rotation = 0f;
					Scale = 1f;
					PortraitScale = 1f;
					Hide = false;
					IsWet = false;
					Frame = null;
					Direction = null;
					SpriteDirection = null;
					Velocity = 0f;
					PortraitPositionXOverride = null;
					PortraitPositionYOverride = null;
					CustomTexturePath = null;
				}
			}

			private static class LocalBuffID
			{
				public const int Confused = 31;

				public const int Poisoned = 20;

				public const int OnFire = 24;

				public const int OnFire3 = 323;

				public const int ShadowFlame = 153;

				public const int Daybreak = 189;

				public const int Frostburn = 44;

				public const int Frostburn2 = 324;

				public const int CursedInferno = 39;

				public const int BetsysCurse = 203;

				public const int Ichor = 69;

				public const int Venom = 70;

				public const int Oiled = 204;

				public const int BoneJavelin = 169;

				public const int TentacleSpike = 337;

				public const int BloodButcherer = 344;
			}

			public static SetFactory Factory = new SetFactory(688);

			public static Dictionary<int, int> SpecialSpawningRules = new Dictionary<int, int>
			{
				{ 259, 0 },
				{ 260, 0 },
				{ 175, 0 },
				{ 43, 0 },
				{ 56, 0 },
				{ 101, 0 }
			};

			public static Dictionary<int, NPCBestiaryDrawModifiers> NPCBestiaryDrawOffset = NPCBestiaryDrawOffsetCreation();

			public static Dictionary<int, NPCDebuffImmunityData> DebuffImmunitySets = new Dictionary<int, NPCDebuffImmunityData>
			{
				{ 0, null },
				{
					1,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 2, null },
				{ 3, null },
				{
					4,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					5,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					6,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					7,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					8,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					9,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					10,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					11,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					12,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					13,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					14,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					15,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					16,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					17,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					18,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					19,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					20,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					21,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					22,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					23,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					24,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 24, 31, 323 }
					}
				},
				{
					25,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 26, null },
				{ 27, null },
				{ 28, null },
				{
					29,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					30,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					665,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					31,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					32,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					33,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					34,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					35,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[5] { 20, 31, 169, 337, 344 }
					}
				},
				{
					36,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					37,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					38,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					39,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					40,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					41,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					42,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					43,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					44,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					45,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					46,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 47, null },
				{
					48,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 49, null },
				{
					50,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 51, null },
				{ 52, null },
				{ 53, null },
				{
					54,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					55,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					56,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					57,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					58,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					59,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					60,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 24, 323 }
					}
				},
				{
					61,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					62,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 24, 31, 153, 323 }
					}
				},
				{
					63,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					64,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					65,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					66,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 24, 31, 153, 323 }
					}
				},
				{
					67,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					68,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					69,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					70,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[6] { 20, 24, 31, 39, 70, 323 }
					}
				},
				{
					71,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					72,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{ 73, null },
				{
					74,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 75, null },
				{
					76,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					77,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 78, null },
				{ 79, null },
				{ 80, null },
				{
					81,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					82,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					83,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					84,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					85,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 86, null },
				{
					87,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					88,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					89,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					90,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					91,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					92,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 93, null },
				{
					94,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					95,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					96,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					97,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					98,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					99,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					100,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					101,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 39, 31 }
					}
				},
				{
					102,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					103,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 104, null },
				{
					105,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					106,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					107,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					108,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					109,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					110,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 111, null },
				{
					112,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					666,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					113,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 24, 31, 323 }
					}
				},
				{
					114,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 24, 31, 323 }
					}
				},
				{
					115,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					116,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					117,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					118,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					119,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					120,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					121,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					122,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					123,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					124,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					125,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					126,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					127,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[5] { 20, 31, 169, 337, 344 }
					}
				},
				{
					128,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					129,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					130,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					131,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 132, null },
				{ 133, null },
				{
					134,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					135,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					136,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					137,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					138,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					139,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					140,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					141,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 70 }
					}
				},
				{
					142,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					143,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					144,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					145,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					146,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					147,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					148,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					149,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					150,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					151,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 24, 323 }
					}
				},
				{ 152, null },
				{ 153, null },
				{
					154,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{ 155, null },
				{
					156,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 24, 31, 153, 323 }
					}
				},
				{
					157,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 158, null },
				{ 159, null },
				{
					160,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					161,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{ 162, null },
				{
					163,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					164,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					165,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					166,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					167,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{ 168, null },
				{
					169,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{ 170, null },
				{ 171, null },
				{
					172,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					173,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					174,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					175,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					176,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					177,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					178,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					179,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 180, null },
				{
					181,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					182,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					183,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					184,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					185,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{ 186, null },
				{ 187, null },
				{ 188, null },
				{ 189, null },
				{ 190, null },
				{ 191, null },
				{ 192, null },
				{ 193, null },
				{ 194, null },
				{
					195,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 196, null },
				{
					197,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					198,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					199,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 200, null },
				{
					201,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					202,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					203,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					204,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					205,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					206,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					207,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					208,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					209,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					210,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					211,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 212, null },
				{ 213, null },
				{ 214, null },
				{ 215, null },
				{
					216,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					217,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					218,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					219,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					220,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					221,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					222,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 223, null },
				{ 224, null },
				{
					225,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 226, null },
				{
					227,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					228,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					229,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					230,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					231,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					232,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					233,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					234,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					235,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					236,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					237,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					238,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					239,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					240,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					241,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					242,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					243,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{
					244,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					245,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					246,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					247,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					248,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					249,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					250,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 251, null },
				{ 252, null },
				{
					253,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 254, null },
				{ 255, null },
				{
					256,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 257, null },
				{ 258, null },
				{
					259,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					260,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					261,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					262,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					263,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					264,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					265,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					266,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					267,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					268,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 69 }
					}
				},
				{
					269,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					270,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					271,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					272,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					273,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					274,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					275,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					276,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					277,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					278,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					279,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					280,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					281,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					282,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					283,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					284,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					285,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					286,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					287,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					288,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					289,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					290,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 31, 69 }
					}
				},
				{
					291,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					292,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					293,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					294,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					295,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					296,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					297,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					298,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					671,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					672,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					673,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					674,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					675,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					299,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					300,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					301,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					302,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					303,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					304,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 305, null },
				{ 306, null },
				{ 307, null },
				{ 308, null },
				{ 309, null },
				{ 310, null },
				{ 311, null },
				{ 312, null },
				{ 313, null },
				{ 314, null },
				{
					315,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					316,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[13]
						{
							20, 24, 31, 39, 44, 69, 70, 153, 189, 203,
							204, 323, 324
						}
					}
				},
				{ 317, null },
				{ 318, null },
				{ 319, null },
				{ 320, null },
				{ 321, null },
				{
					322,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					323,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					324,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					325,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 326, null },
				{
					327,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					328,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					329,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					330,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 331, null },
				{ 332, null },
				{
					333,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					334,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					335,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					336,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					337,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					338,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					339,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					340,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					341,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 342, null },
				{
					343,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					344,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{
					345,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{
					346,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{
					347,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 348, null },
				{ 349, null },
				{ 350, null },
				{
					351,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					352,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{
					353,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					354,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					355,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					356,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					357,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					358,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					359,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					360,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					361,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					362,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					363,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					364,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					365,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					366,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					367,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					368,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					369,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					370,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					371,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					372,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					373,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					374,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					375,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					376,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					377,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					378,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					379,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					380,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					381,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 382, null },
				{ 383, null },
				{
					384,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 385, null },
				{ 386, null },
				{
					387,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					388,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 389, null },
				{ 390, null },
				{ 391, null },
				{
					392,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					393,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					394,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					395,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					396,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					397,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					398,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					399,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					400,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					401,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					402,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					403,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					404,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					405,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					406,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					407,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					408,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 409, null },
				{ 410, null },
				{ 411, null },
				{
					412,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					413,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					414,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 415, null },
				{ 416, null },
				{ 417, null },
				{
					418,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 419, null },
				{
					420,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					421,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					422,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 423, null },
				{ 424, null },
				{ 425, null },
				{ 426, null },
				{ 427, null },
				{
					428,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 429, null },
				{ 430, null },
				{
					431,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{ 432, null },
				{ 433, null },
				{ 434, null },
				{ 435, null },
				{ 436, null },
				{
					437,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					438,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					439,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					440,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					441,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					442,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					443,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					444,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					445,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					446,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					447,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					448,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					449,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					450,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					451,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					452,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					453,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					454,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					455,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					456,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					457,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					458,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					459,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 460, null },
				{
					461,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 462, null },
				{ 463, null },
				{ 464, null },
				{
					465,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 466, null },
				{
					467,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 468, null },
				{ 469, null },
				{ 470, null },
				{
					471,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 31, 153 }
					}
				},
				{
					472,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 31, 153 }
					}
				},
				{
					473,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					474,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					475,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					476,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					477,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					478,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					479,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					480,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					481,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					482,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					483,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					484,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					485,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					486,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					487,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					488,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 489, null },
				{ 490, null },
				{
					491,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					492,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					493,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					494,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					495,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					496,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					497,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 498, null },
				{ 499, null },
				{ 500, null },
				{ 501, null },
				{ 502, null },
				{ 503, null },
				{ 504, null },
				{ 505, null },
				{ 506, null },
				{
					507,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 508, null },
				{ 509, null },
				{
					510,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					511,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					512,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					513,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					514,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					515,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					516,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					517,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					518,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					519,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					520,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					521,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[13]
						{
							20, 24, 31, 39, 44, 69, 70, 153, 189, 203,
							204, 323, 324
						}
					}
				},
				{
					522,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					523,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 524, null },
				{ 525, null },
				{ 526, null },
				{ 527, null },
				{ 528, null },
				{ 529, null },
				{
					530,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					531,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 532, null },
				{
					533,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[13]
						{
							20, 24, 31, 39, 44, 69, 70, 153, 189, 203,
							204, 323, 324
						}
					}
				},
				{ 534, null },
				{
					535,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 536, null },
				{
					537,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					538,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					539,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					540,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					541,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					542,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					543,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					544,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					545,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					546,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					547,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					548,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					549,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					550,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					551,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 24, 31, 323 }
					}
				},
				{
					552,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					553,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					554,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					555,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					556,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					557,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					558,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					559,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					560,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					561,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					562,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					563,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					564,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					565,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					566,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					567,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					568,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					569,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					570,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					571,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					572,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					573,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					574,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					575,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					576,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					577,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					578,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					579,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 580, null },
				{ 581, null },
				{ 582, null },
				{
					583,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					584,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					585,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{ 586, null },
				{ 587, null },
				{
					588,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					589,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 590, null },
				{ 591, null },
				{
					592,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					593,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					594,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					595,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					596,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					597,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					598,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					599,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					600,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					601,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					602,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					603,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					604,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					605,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					606,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					607,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					608,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					609,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					610,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					611,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					612,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					613,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					614,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					615,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					616,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					617,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					618,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					619,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					620,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					621,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					622,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					623,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					624,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					625,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					626,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					627,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					628,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					629,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[6] { 20, 24, 31, 44, 323, 324 }
					}
				},
				{ 630, null },
				{
					631,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 632, null },
				{
					633,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 634, null },
				{
					635,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					636,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					637,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					638,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					639,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					640,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					641,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					642,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					643,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					644,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					645,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					646,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					647,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					648,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					649,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					650,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					651,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					652,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					653,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					654,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					655,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					656,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					657,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					658,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					659,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					660,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					661,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					662,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					663,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					668,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					669,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					670,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					678,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					679,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					680,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					681,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					682,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					683,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					684,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					677,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					685,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					686,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					687,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				}
			};

			public static List<int> NormalGoldCritterBestiaryPriority = new List<int>
			{
				46, 540, 614, 303, 337, 443, 74, 297, 298, 671,
				672, 673, 674, 675, 442, 55, 230, 592, 593, 299,
				538, 539, 300, 447, 361, 445, 377, 446, 356, 444,
				357, 448, 595, 596, 597, 598, 599, 600, 601, 626,
				627, 612, 613, 604, 605, 669, 677
			};

			public static List<int> BossBestiaryPriority = new List<int>
			{
				664, 4, 5, 50, 535, 13, 14, 15, 266, 267,
				668, 35, 36, 222, 113, 114, 117, 115, 116, 657,
				658, 659, 660, 125, 126, 134, 135, 136, 139, 127,
				128, 131, 129, 130, 262, 263, 264, 636, 245, 246,
				249, 247, 248, 370, 372, 373, 439, 438, 379, 380,
				440, 521, 454, 507, 517, 422, 493, 398, 396, 397,
				400, 401
			};

			public static List<int> TownNPCBestiaryPriority = new List<int>
			{
				22, 17, 18, 38, 369, 20, 19, 207, 227, 353,
				633, 550, 588, 107, 228, 124, 54, 108, 178, 229,
				160, 441, 209, 208, 663, 142, 637, 638, 656, 670,
				678, 679, 680, 681, 682, 683, 684, 368, 453, 37,
				687
			};

			public static bool[] DontDoHardmodeScaling = Factory.CreateBoolSet(5, 13, 14, 15, 267, 113, 114, 115, 116, 117, 118, 119, 658, 659, 660, 400, 522);

			public static bool[] ReflectStarShotsInForTheWorthy = Factory.CreateBoolSet(4, 5, 13, 14, 15, 266, 267, 35, 36, 113, 114, 115, 116, 117, 118, 119, 125, 126, 134, 135, 136, 139, 127, 128, 131, 129, 130, 262, 263, 264, 245, 247, 248, 246, 249, 398, 400, 397, 396, 401);

			public static bool[] IsTownPet = Factory.CreateBoolSet(637, 638, 656, 670, 678, 679, 680, 681, 682, 683, 684);

			public static bool[] IsTownSlime = Factory.CreateBoolSet(670, 678, 679, 680, 681, 682, 683, 684);

			public static List<int> GoldCrittersCollection = new List<int>
			{
				443, 442, 592, 593, 444, 601, 445, 446, 605, 447,
				627, 613, 448, 539
			};

			public static bool[] ZappingJellyfish = Factory.CreateBoolSet(63, 64, 103, 242);

			public static bool[] CantTakeLunchMoney = Factory.CreateBoolSet(394, 393, 392, 492, 491, 662, 384, 478, 535, 658, 659, 660, 128, 131, 129, 130, 139, 267, 247, 248, 246, 249, 245, 409, 410, 397, 396, 401, 400, 440, 68);

			public static Dictionary<int, int> RespawnEnemyID = new Dictionary<int, int>
			{
				{ 492, 0 },
				{ 491, 0 },
				{ 394, 0 },
				{ 393, 0 },
				{ 392, 0 },
				{ 13, 0 },
				{ 14, 0 },
				{ 15, 0 },
				{ 412, 0 },
				{ 413, 0 },
				{ 414, 0 },
				{ 134, 0 },
				{ 135, 0 },
				{ 136, 0 },
				{ 454, 0 },
				{ 455, 0 },
				{ 456, 0 },
				{ 457, 0 },
				{ 458, 0 },
				{ 459, 0 },
				{ 8, 7 },
				{ 9, 7 },
				{ 11, 10 },
				{ 12, 10 },
				{ 40, 39 },
				{ 41, 39 },
				{ 96, 95 },
				{ 97, 95 },
				{ 99, 98 },
				{ 100, 98 },
				{ 88, 87 },
				{ 89, 87 },
				{ 90, 87 },
				{ 91, 87 },
				{ 92, 87 },
				{ 118, 117 },
				{ 119, 117 },
				{ 514, 513 },
				{ 515, 513 },
				{ 511, 510 },
				{ 512, 510 },
				{ 622, 621 },
				{ 623, 621 }
			};

			public static int[] TrailingMode = Factory.CreateIntSet(-1, 439, 0, 440, 0, 370, 1, 372, 1, 373, 1, 396, 1, 400, 1, 401, 1, 473, 2, 474, 2, 475, 2, 476, 2, 4, 3, 471, 3, 477, 3, 479, 3, 120, 4, 137, 4, 138, 4, 94, 5, 125, 6, 126, 6, 127, 6, 128, 6, 129, 6, 130, 6, 131, 6, 139, 6, 140, 6, 407, 6, 420, 6, 425, 6, 427, 6, 426, 6, 581, 6, 516, 6, 542, 6, 543, 6, 544, 6, 545, 6, 402, 7, 417, 7, 419, 7, 418, 7, 574, 7, 575, 7, 519, 7, 521, 7, 522, 7, 546, 7, 558, 7, 559, 7, 560, 7, 551, 7, 620, 7, 657, 6, 636, 7, 677, 7, 685, 7);

			public static bool[] IsDragonfly = Factory.CreateBoolSet(595, 596, 597, 598, 599, 600, 601);

			public static bool[] BelongsToInvasionOldOnesArmy = Factory.CreateBoolSet(552, 553, 554, 561, 562, 563, 555, 556, 557, 558, 559, 560, 576, 577, 568, 569, 566, 567, 570, 571, 572, 573, 548, 549, 564, 565, 574, 575, 551, 578);

			public static bool[] TeleportationImmune = Factory.CreateBoolSet(552, 553, 554, 561, 562, 563, 555, 556, 557, 558, 559, 560, 576, 577, 568, 569, 566, 567, 570, 571, 572, 573, 548, 549, 564, 565, 574, 575, 551, 578);

			public static bool[] UsesNewTargetting = Factory.CreateBoolSet(547, 552, 553, 554, 561, 562, 563, 555, 556, 557, 558, 559, 560, 576, 577, 568, 569, 566, 567, 570, 571, 572, 573, 564, 565, 574, 575, 551, 578, 210, 211, 620, 668);

			public static bool[] TakesDamageFromHostilesWithoutBeingFriendly = Factory.CreateBoolSet(46, 55, 74, 148, 149, 230, 297, 298, 299, 303, 355, 356, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 377, 357, 374, 442, 443, 444, 445, 446, 448, 538, 539, 337, 540, 484, 485, 486, 487, 592, 593, 595, 596, 597, 598, 599, 600, 601, 602, 603, 604, 605, 606, 607, 608, 609, 611, 612, 613, 614, 615, 616, 617, 625, 626, 627, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 583, 584, 585, 669, 671, 672, 673, 674, 675, 677, 687);

			public static bool[] AllNPCs = Factory.CreateBoolSet(true);

			public static bool[] HurtingBees = Factory.CreateBoolSet(210, 211, 222);

			public static bool[] FighterUsesDD2PortalAppearEffect = Factory.CreateBoolSet(552, 553, 554, 561, 562, 563, 555, 556, 557, 576, 577, 568, 569, 570, 571, 572, 573, 564, 565);

			public static float[] StatueSpawnedDropRarity = Factory.CreateCustomSet(-1f, (short)480, 0.05f, (short)82, 0.05f, (short)86, 0.05f, (short)48, 0.05f, (short)490, 0.05f, (short)489, 0.05f, (short)170, 0.05f, (short)180, 0.05f, (short)171, 0.05f, (short)167, 0.25f, (short)73, 0.01f, (short)24, 0.05f, (short)481, 0.05f, (short)42, 0.05f, (short)6, 0.05f, (short)2, 0.05f, (short)49, 0.2f, (short)3, 0.2f, (short)58, 0.2f, (short)21, 0.2f, (short)65, 0.2f, (short)449, 0.2f, (short)482, 0.2f, (short)103, 0.2f, (short)64, 0.2f, (short)63, 0.2f, (short)85, 0f);

			public static bool[] NoEarlymodeLootWhenSpawnedFromStatue = Factory.CreateBoolSet(480, 82, 86, 170, 180, 171);

			public static bool[] NeedsExpertScaling = Factory.CreateBoolSet(25, 30, 665, 33, 112, 666, 261, 265, 371, 516, 519, 397, 396, 398, 491);

			public static bool[] ProjectileNPC = Factory.CreateBoolSet(25, 30, 665, 33, 112, 666, 261, 265, 371, 516, 519);

			public static bool[] SavesAndLoads = Factory.CreateBoolSet(422, 507, 517, 493);

			public static int[] TrailCacheLength = Factory.CreateIntSet(10, 402, 36, 519, 20, 522, 20, 620, 20, 677, 60, 685, 10);

			public static bool[] UsesMultiplayerProximitySyncing = Factory.CreateBoolSet(true, 398, 397, 396);

			public static bool[] NoMultiplayerSmoothingByType = Factory.CreateBoolSet(113, 114, 50, 657, 120, 245, 247, 248, 246, 370, 222, 398, 397, 396, 400, 401, 668, 70);

			public static bool[] NoMultiplayerSmoothingByAI = Factory.CreateBoolSet(6, 8, 37);

			public static bool[] MPAllowedEnemies = Factory.CreateBoolSet(4, 13, 50, 126, 125, 134, 127, 128, 131, 129, 130, 222, 245, 266, 370, 657, 668);

			public static bool[] TownCritter = Factory.CreateBoolSet(46, 148, 149, 230, 299, 300, 303, 337, 361, 362, 364, 366, 367, 443, 445, 447, 538, 539, 540, 583, 584, 585, 592, 593, 602, 607, 608, 610, 616, 617, 625, 626, 627, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 687);

			public static bool[] CountsAsCritter = Factory.CreateBoolSet(46, 303, 337, 540, 443, 74, 297, 298, 442, 611, 377, 446, 612, 613, 356, 444, 595, 596, 597, 598, 599, 600, 601, 604, 605, 357, 448, 374, 484, 355, 358, 606, 359, 360, 485, 486, 487, 148, 149, 55, 230, 592, 593, 299, 538, 539, 300, 447, 361, 445, 362, 363, 364, 365, 367, 366, 583, 584, 585, 602, 603, 607, 608, 609, 610, 616, 617, 625, 626, 627, 615, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 661, 669, 671, 672, 673, 674, 675, 677, 687);

			public static bool[] HasNoPartyText = Factory.CreateBoolSet(441, 453);

			public static int[] HatOffsetY = Factory.CreateIntSet(0, 227, 4, 107, 2, 108, 2, 229, 4, 17, 2, 38, 8, 160, -10, 208, 2, 142, 2, 124, 2, 453, 2, 37, 4, 54, 4, 209, 4, 369, 6, 441, 6, 353, -2, 633, -2, 550, -2, 588, 2, 663, 2, 637, 0, 638, 0, 656, 4, 670, 0, 678, 0, 679, 0, 680, 0, 681, 0, 682, 0, 683, 0, 684, 0);

			public static int[] FaceEmote = Factory.CreateIntSet(0, 17, 101, 18, 102, 19, 103, 20, 104, 22, 105, 37, 106, 38, 107, 54, 108, 107, 109, 108, 110, 124, 111, 142, 112, 160, 113, 178, 114, 207, 115, 208, 116, 209, 117, 227, 118, 228, 119, 229, 120, 353, 121, 368, 122, 369, 123, 453, 124, 441, 125, 588, 140, 633, 141, 663, 145);

			public static int[] ExtraFramesCount = Factory.CreateIntSet(0, 17, 9, 18, 9, 19, 9, 20, 7, 22, 10, 37, 5, 38, 9, 54, 7, 107, 9, 108, 7, 124, 9, 142, 9, 160, 7, 178, 9, 207, 9, 208, 9, 209, 10, 227, 9, 228, 10, 229, 10, 353, 9, 633, 9, 368, 10, 369, 9, 453, 9, 441, 9, 550, 9, 588, 9, 663, 7, 637, 18, 638, 11, 656, 20, 670, 6, 678, 6, 679, 6, 680, 6, 681, 6, 682, 6, 683, 6, 684, 6);

			public static int[] AttackFrameCount = Factory.CreateIntSet(0, 17, 4, 18, 4, 19, 4, 20, 2, 22, 5, 37, 0, 38, 4, 54, 2, 107, 4, 108, 2, 124, 4, 142, 4, 160, 2, 178, 4, 207, 4, 208, 4, 209, 5, 227, 4, 228, 5, 229, 5, 353, 4, 633, 4, 368, 5, 369, 4, 453, 4, 441, 4, 550, 4, 588, 4, 663, 2, 637, 0, 638, 0, 656, 0, 670, 0, 678, 0, 679, 0, 680, 0, 681, 0, 682, 0, 683, 0, 684, 0);

			public static int[] DangerDetectRange = Factory.CreateIntSet(-1, 38, 300, 17, 320, 107, 300, 19, 900, 22, 700, 124, 800, 228, 800, 178, 900, 18, 300, 229, 1000, 209, 1000, 54, 700, 108, 700, 160, 700, 20, 1200, 369, 300, 453, 300, 368, 900, 207, 60, 227, 800, 208, 400, 142, 500, 441, 50, 353, 60, 633, 100, 550, 120, 588, 120, 663, 700, 638, 250, 637, 250, 656, 250, 670, 250, 678, 250, 679, 250, 680, 250, 681, 250, 682, 250, 683, 250, 684, 250);

			public static bool[] ShimmerImmunity = Factory.CreateBoolSet(637, 638, 656, 670, 684, 678, 679, 680, 681, 682, 683, 356, 669, 676, 244, 677, 594, 667, 662, 5, 115, 116, 139, 245, 247, 248, 246, 249, 344, 325, 50, 535, 657, 658, 659, 660, 668, 25, 30, 33, 70, 72, 665, 666, 112, 516, 517, 518, 519, 520, 521, 522, 523, 381, 382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 423, 424, 425, 426, 427, 428, 429, 548, 549, 551, 552, 553, 554, 555, 556, 557, 558, 559, 560, 561, 562, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 574, 575, 576, 577, 578);

			public static int[] ShimmerTransformToItem = Factory.CreateIntSet(-1, 651, 182, 644, 182, 650, 178, 643, 178, 649, 179, 642, 179, 648, 177, 641, 177, 640, 180, 647, 180, 646, 181, 639, 181, 652, 999, 645, 999);

			public static bool[] ShimmerTownTransform = Factory.CreateBoolSet(22, 17, 18, 227, 207, 633, 588, 208, 369, 353, 38, 20, 550, 19, 107, 228, 54, 124, 441, 229, 160, 108, 178, 209, 142, 663, 37, 453, 368);

			public static int[] ShimmerTransformToNPC = Factory.CreateIntSet(-1, 3, 21, 132, 202, 186, 201, 187, 21, 188, 21, 189, 202, 200, 203, 590, 21, 1, 676, 302, 676, 335, 676, 336, 676, 334, 676, 333, 676, 225, 676, 141, 676, 16, 676, 147, 676, 184, 676, 537, 676, 204, 676, 81, 676, 183, 676, 138, 676, 121, 676, 591, 449, 430, 449, 436, 452, 432, 450, 433, 449, 434, 449, 435, 451, 614, 677, 74, 677, 297, 677, 298, 677, 673, 677, 672, 677, 671, 677, 675, 677, 674, 677, 362, 677, 363, 677, 364, 677, 365, 677, 608, 677, 609, 677, 602, 677, 603, 677, 611, 677, 148, 677, 149, 677, 46, 677, 299, 677, 538, 677, 55, 677, 607, 677, 615, 677, 625, 677, 626, 677, 361, 677, 687, 677, 484, 677, 604, 677, 358, 677, 355, 677, 616, 677, 617, 677, 654, 677, 653, 677, 655, 677, 585, 677, 584, 677, 583, 677, 595, 677, 596, 677, 600, 677, 597, 677, 598, 677, 599, 677, 357, 677, 377, 677, 606, 677, 359, 677, 360, 677, 367, 677, 366, 677, 300, 677, 610, 677, 612, 677, 487, 677, 486, 677, 485, 677, 669, 677, 356, 677, 661, 677, 374, 677, 442, 677, 443, 677, 444, 677, 601, 677, 445, 677, 592, 677, 446, 677, 605, 677, 447, 677, 627, 677, 539, 677, 613, 677, 448, 677);

			public static int[] AttackTime = Factory.CreateIntSet(-1, 38, 34, 17, 34, 107, 60, 19, 40, 22, 30, 124, 34, 228, 40, 178, 24, 18, 34, 229, 60, 209, 60, 54, 60, 108, 30, 160, 60, 20, 600, 369, 34, 453, 34, 368, 60, 207, 15, 227, 60, 208, 34, 142, 34, 441, 15, 353, 12, 633, 12, 550, 34, 588, 20, 663, 60, 638, -1, 637, -1, 656, -1, 670, -1, 678, -1, 679, -1, 680, -1, 681, -1, 682, -1, 683, -1, 684, -1);

			public static int[] AttackAverageChance = Factory.CreateIntSet(1, 38, 40, 17, 30, 107, 60, 19, 30, 22, 30, 124, 30, 228, 50, 178, 50, 18, 60, 229, 40, 209, 30, 54, 30, 108, 30, 160, 60, 20, 60, 369, 50, 453, 30, 368, 40, 207, 1, 227, 30, 208, 50, 142, 50, 441, 1, 353, 1, 633, 1, 550, 40, 588, 20, 663, 1, 638, 1, 637, 1, 656, 1, 670, 1, 678, 1, 679, 1, 680, 1, 681, 1, 682, 1, 683, 1, 684, 1);

			public static int[] AttackType = Factory.CreateIntSet(-1, 38, 0, 17, 0, 107, 0, 19, 1, 22, 1, 124, 0, 228, 1, 178, 1, 18, 0, 229, 1, 209, 1, 54, 2, 108, 2, 160, 2, 20, 2, 369, 0, 453, 0, 368, 1, 207, 3, 227, 1, 208, 0, 142, 0, 441, 3, 353, 3, 633, 0, 550, 0, 588, 0, 663, 2, 638, -1, 637, -1, 656, -1, 670, -1, 678, -1, 679, -1, 680, -1, 681, -1, 682, -1, 683, -1, 684, -1);

			public static int[] PrettySafe = Factory.CreateIntSet(-1, 19, 300, 22, 200, 124, 200, 228, 300, 178, 300, 229, 300, 209, 300, 54, 100, 108, 100, 160, 100, 20, 200, 368, 200, 227, 200);

			public static Color[] MagicAuraColor = Factory.CreateCustomSet(Color.White, (short)54, new Color(100, 4, 227, 127), (short)108, new Color(255, 80, 60, 127), (short)160, new Color(40, 80, 255, 127), (short)20, new Color(40, 255, 80, 127), (short)663, Main.hslToRgb(0.92f, 1f, 0.78f, 127));

			public static bool[] DemonEyes = Factory.CreateBoolSet(2, 190, 192, 193, 191, 194, 317, 318);

			public static bool[] Zombies = Factory.CreateBoolSet(3, 132, 186, 187, 188, 189, 200, 223, 161, 254, 255, 52, 53, 536, 319, 320, 321, 332, 436, 431, 432, 433, 434, 435, 331, 430, 590);

			public static bool[] Skeletons = Factory.CreateBoolSet(77, 449, 450, 451, 452, 481, 201, 202, 203, 21, 324, 110, 323, 293, 291, 322, 292, 197, 167, 44, 635);

			public static int[] BossHeadTextures = Factory.CreateIntSet(-1, 4, 0, 13, 2, 344, 3, 370, 4, 246, 5, 249, 5, 345, 6, 50, 7, 396, 8, 395, 9, 325, 10, 262, 11, 327, 13, 222, 14, 125, 15, 126, 20, 346, 17, 127, 18, 35, 19, 68, 19, 113, 22, 266, 23, 439, 24, 440, 24, 134, 25, 491, 26, 517, 27, 422, 28, 507, 29, 493, 30, 549, 35, 564, 32, 565, 32, 576, 33, 577, 33, 551, 34, 548, 36, 636, 37, 657, 38, 668, 39);

			public static bool[] PositiveNPCTypesExcludedFromDeathTally = Factory.CreateBoolSet(121, 384, 478, 479, 410, 472, 378);

			public static bool[] ShouldBeCountedAsBoss = Factory.CreateBoolSet(false, 517, 422, 507, 493, 13, 664);

			public static bool[] DangerThatPreventsOtherDangers = Factory.CreateBoolSet(517, 422, 507, 493, 399);

			public static bool[] MustAlwaysDraw = Factory.CreateBoolSet(113, 114, 115, 116, 126, 125);

			public static int[] ExtraTextureCount = Factory.CreateIntSet(0, 38, 1, 17, 1, 107, 0, 19, 0, 22, 0, 124, 1, 228, 0, 178, 1, 18, 1, 229, 1, 209, 1, 54, 1, 108, 1, 160, 0, 20, 0, 369, 1, 453, 1, 368, 1, 207, 1, 227, 1, 208, 0, 142, 1, 441, 1, 353, 1, 633, 1, 550, 0, 588, 1, 633, 2, 663, 1, 638, 0, 637, 0, 656, 0, 670, 0, 678, 0, 679, 0, 680, 0, 681, 0, 682, 0, 683, 0, 684, 0);

			public static int[] NPCFramingGroup = Factory.CreateIntSet(0, 18, 1, 20, 1, 208, 1, 178, 1, 124, 1, 353, 1, 633, 1, 369, 2, 160, 3, 637, 4, 638, 5, 656, 6, 670, 7, 678, 7, 679, 7, 680, 7, 681, 7, 682, 7, 683, 7, 684, 7);

			public static int[][] TownNPCsFramingGroups = new int[8][]
			{
				new int[26]
				{
					0, 0, 0, -2, -2, -2, 0, 0, 0, 0,
					-2, -2, -2, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0
				},
				new int[25]
				{
					0, 0, 0, -2, -2, -2, 0, 0, 0, -2,
					-2, -2, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0
				},
				new int[25]
				{
					0, 0, 0, -2, -2, -2, 0, 0, -2, -2,
					-2, -2, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0
				},
				new int[22]
				{
					0, 0, -2, 0, 0, 0, 0, -2, -2, -2,
					0, 0, 0, 0, -2, -2, 0, 0, 0, 0,
					0, 0
				},
				new int[28]
				{
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					2, 2, 4, 6, 4, 2, 2, -2, -4, -6,
					-4, -2, -4, -4, -6, -6, -6, -4
				},
				new int[28]
				{
					0, 0, 0, 0, 0, 0, 0, 0, -2, -2,
					-2, 0, 0, -2, -2, 0, 0, 4, 6, 6,
					6, 6, 4, 4, 4, 4, 4, 4
				},
				new int[26]
				{
					0, 0, -2, -4, -4, -2, 0, -2, 0, 0,
					2, 4, 6, 4, 2, 0, -2, -4, -6, -6,
					-6, -6, -6, -6, -4, -2
				},
				new int[14]
				{
					0, -2, 0, -2, -4, -6, -4, -2, 0, 0,
					2, 2, 4, 2
				}
			};

			public static Dictionary<int, NPCBestiaryDrawModifiers> NPCBestiaryDrawOffsetCreation()
			{
				Dictionary<int, NPCBestiaryDrawModifiers> redigitEntries = GetRedigitEntries();
				Dictionary<int, NPCBestiaryDrawModifiers> leinforsEntries = GetLeinforsEntries();
				Dictionary<int, NPCBestiaryDrawModifiers> groxEntries = GetGroxEntries();
				Dictionary<int, NPCBestiaryDrawModifiers> dictionary = new Dictionary<int, NPCBestiaryDrawModifiers>();
				foreach (KeyValuePair<int, NPCBestiaryDrawModifiers> item in groxEntries)
				{
					dictionary[item.Key] = item.Value;
				}
				foreach (KeyValuePair<int, NPCBestiaryDrawModifiers> item2 in leinforsEntries)
				{
					dictionary[item2.Key] = item2.Value;
				}
				foreach (KeyValuePair<int, NPCBestiaryDrawModifiers> item3 in redigitEntries)
				{
					dictionary[item3.Key] = item3.Value;
				}
				return dictionary;
			}

			private static Dictionary<int, NPCBestiaryDrawModifiers> GetRedigitEntries()
			{
				return new Dictionary<int, NPCBestiaryDrawModifiers>
				{
					{
						430,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						431,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						432,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						433,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						434,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						435,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						436,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						591,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						449,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						450,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						451,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						452,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						595,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						596,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						597,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						598,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						600,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						495,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						497,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						498,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						500,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						501,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						502,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						503,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						504,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						505,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						506,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						230,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						593,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						158,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-2,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						440,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						568,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						566,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						576,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						558,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						559,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						552,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						553,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						564,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						570,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						555,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						556,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						574,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						561,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						562,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						572,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						535,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					}
				};
			}

			private static Dictionary<int, NPCBestiaryDrawModifiers> GetGroxEntries()
			{
				return new Dictionary<int, NPCBestiaryDrawModifiers>();
			}

			private static Dictionary<int, NPCBestiaryDrawModifiers> GetLeinforsEntries()
			{
				return new Dictionary<int, NPCBestiaryDrawModifiers>
				{
					{
						-65,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-64,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-63,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-62,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-61,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 4f),
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-60,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 3f),
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-59,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-58,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-57,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-56,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-55,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true,
							Velocity = 1f,
							Scale = 1.1f
						}
					},
					{
						-54,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true,
							Velocity = 1f,
							Scale = 0.9f
						}
					},
					{
						-53,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-52,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-51,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-50,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-49,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-48,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-47,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-46,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-45,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-44,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-43,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -15f),
							Hide = true
						}
					},
					{
						-42,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -15f),
							Hide = true
						}
					},
					{
						-41,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -15f),
							Hide = true
						}
					},
					{
						-40,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -15f),
							Hide = true
						}
					},
					{
						-39,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -15f),
							Hide = true
						}
					},
					{
						-38,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -15f),
							Hide = true
						}
					},
					{
						-37,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-36,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-35,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-34,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-33,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-32,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-31,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-30,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-29,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-28,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-27,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-26,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-23,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -9f),
							Rotation = 0.75f,
							Scale = 1.2f,
							Hide = true
						}
					},
					{
						-22,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -9f),
							Rotation = 0.75f,
							Scale = 0.8f,
							Hide = true
						}
					},
					{
						-25,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-24,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						-21,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 5f),
							Scale = 1.2f,
							Hide = true
						}
					},
					{
						-20,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 4f),
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-19,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 3f),
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-18,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 2f),
							Scale = 0.8f,
							Hide = true
						}
					},
					{
						-17,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 3f),
							Scale = 1.2f,
							Hide = true
						}
					},
					{
						-16,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 3f),
							Scale = 0.8f,
							Hide = true
						}
					},
					{
						-15,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.2f,
							Hide = true
						}
					},
					{
						-14,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 1.1f,
							Hide = true
						}
					},
					{
						-13,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Scale = 0.9f,
							Hide = true
						}
					},
					{
						-12,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -9f),
							Rotation = 0.75f,
							Scale = 1.2f,
							Hide = true
						}
					},
					{
						-11,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -9f),
							Rotation = 0.75f,
							Scale = 0.8f,
							Hide = true
						}
					},
					{
						2,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, -15f),
							PortraitPositionYOverride = -35f
						}
					},
					{
						3,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						4,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(25f, -30f),
							Rotation = 0.7f,
							Frame = 4
						}
					},
					{
						5,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(3f, 4f),
							Rotation = 1.5f
						}
					},
					{
						6,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -9f),
							Rotation = 0.75f
						}
					},
					{
						7,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_7",
							Position = new Vector2(20f, 29f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 10f
						}
					},
					{
						8,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						9,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						10,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_10",
							Position = new Vector2(2f, 24f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 10f
						}
					},
					{
						11,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						12,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						13,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_13",
							Position = new Vector2(40f, 22f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 10f
						}
					},
					{
						14,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						15,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						17,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						18,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						19,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						20,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						22,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						25,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						26,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						27,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						28,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						30,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						665,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						31,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						33,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						34,
						new NPCBestiaryDrawModifiers(0)
						{
							Direction = 1
						}
					},
					{
						21,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						35,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -12f),
							Scale = 0.9f,
							PortraitPositionXOverride = -1f,
							PortraitPositionYOverride = -3f
						}
					},
					{
						36,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						38,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						37,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						39,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_39",
							Position = new Vector2(40f, 23f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 10f
						}
					},
					{
						40,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						41,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						43,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(4f, -6f),
							Rotation = MathF.PI * 3f / 4f
						}
					},
					{
						44,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						46,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						47,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						48,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, -14f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						49,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -13f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						50,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 90f),
							PortraitScale = 1.1f,
							PortraitPositionYOverride = 70f
						}
					},
					{
						51,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -13f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						52,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						53,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						54,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						55,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 6f),
							PortraitPositionYOverride = 7f,
							IsWet = true
						}
					},
					{
						56,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(4f, -6f),
							Rotation = MathF.PI * 3f / 4f
						}
					},
					{
						57,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 6f),
							PortraitPositionYOverride = 6f,
							IsWet = true
						}
					},
					{
						58,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 6f),
							PortraitPositionYOverride = 6f,
							IsWet = true
						}
					},
					{
						60,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -19f),
							PortraitPositionYOverride = -36f
						}
					},
					{
						61,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.05f,
							PortraitPositionYOverride = -15f
						}
					},
					{
						62,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -10f),
							PortraitPositionYOverride = -25f
						}
					},
					{
						65,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(35f, 4f),
							Velocity = 1f,
							PortraitPositionXOverride = 5f,
							IsWet = true
						}
					},
					{
						66,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -6f),
							Scale = 0.9f,
							PortraitPositionYOverride = -15f
						}
					},
					{
						68,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-1f, -12f),
							Scale = 0.9f,
							PortraitPositionYOverride = -3f
						}
					},
					{
						70,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						72,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						73,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						74,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, -14f),
							Velocity = 0.05f,
							PortraitPositionYOverride = -30f
						}
					},
					{
						75,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 6f)
						}
					},
					{
						76,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						77,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						78,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.5f
						}
					},
					{
						79,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.5f
						}
					},
					{
						80,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.5f
						}
					},
					{
						83,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-4f, -4f),
							Scale = 0.9f,
							PortraitPositionYOverride = -25f
						}
					},
					{
						84,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, -11f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = -28f
						}
					},
					{
						86,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(20f, 6f),
							Velocity = 1f,
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 2f
						}
					},
					{
						87,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_87",
							Position = new Vector2(55f, 15f),
							PortraitPositionXOverride = 4f,
							PortraitPositionYOverride = 10f
						}
					},
					{
						88,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						89,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						90,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						91,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						92,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						93,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(4f, -11f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						94,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(8f, 0f),
							Rotation = 0.75f
						}
					},
					{
						95,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_95",
							Position = new Vector2(20f, 28f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 10f
						}
					},
					{
						96,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						97,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						98,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_98",
							Position = new Vector2(40f, 24f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 12f
						}
					},
					{
						99,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						100,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						101,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 6f),
							Rotation = MathF.PI * 3f / 4f
						}
					},
					{
						102,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 6f),
							PortraitPositionYOverride = 6f,
							IsWet = true
						}
					},
					{
						104,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						105,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						106,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						107,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						108,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						109,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 35f),
							Velocity = 1f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						110,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 3f
						}
					},
					{
						111,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 3f
						}
					},
					{
						112,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						666,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						113,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_113",
							Position = new Vector2(56f, 5f),
							PortraitPositionXOverride = 10f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						114,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						115,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_115",
							Position = new Vector2(56f, 3f),
							PortraitPositionXOverride = 55f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						116,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(3f, -5f),
							PortraitPositionXOverride = 4f,
							PortraitPositionYOverride = -26f
						}
					},
					{
						117,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_117",
							Position = new Vector2(10f, 20f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						118,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						119,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						120,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						123,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						124,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						121,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, -4f),
							PortraitPositionYOverride = -20f
						}
					},
					{
						122,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 4f)
						}
					},
					{
						125,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-28f, -23f),
							Rotation = -0.75f
						}
					},
					{
						126,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(28f, 30f),
							Rotation = 2.25f
						}
					},
					{
						127,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_127",
							Position = new Vector2(0f, 0f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 1f
						}
					},
					{
						128,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-6f, -2f),
							Rotation = -0.75f,
							Hide = true
						}
					},
					{
						129,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(4f, 4f),
							Rotation = 0.75f,
							Hide = true
						}
					},
					{
						130,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, 8f),
							Rotation = 2.25f,
							Hide = true
						}
					},
					{
						131,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-8f, 8f),
							Rotation = -2.25f,
							Hide = true
						}
					},
					{
						132,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						133,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -5f),
							PortraitPositionYOverride = -25f
						}
					},
					{
						134,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_134",
							Position = new Vector2(60f, 8f),
							PortraitPositionXOverride = 3f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						135,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						136,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						137,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(4f, -11f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						140,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						142,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 1f
						}
					},
					{
						146,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						148,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						149,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						150,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -11f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						151,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -11f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						152,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, -11f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						153,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(20f, 0f),
							Velocity = 1f,
							PortraitPositionXOverride = 0f
						}
					},
					{
						154,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(20f, 0f),
							Velocity = 1f,
							PortraitPositionXOverride = 0f
						}
					},
					{
						155,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(15f, 0f),
							Velocity = 3f,
							PortraitPositionXOverride = 0f
						}
					},
					{
						156,
						new NPCBestiaryDrawModifiers(0)
						{
							PortraitPositionYOverride = -15f
						}
					},
					{
						157,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(20f, 5f),
							PortraitPositionXOverride = 5f,
							PortraitPositionYOverride = 10f,
							IsWet = true
						}
					},
					{
						160,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						158,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -11f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						159,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						161,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						162,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						163,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						164,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						165,
						new NPCBestiaryDrawModifiers(0)
						{
							Rotation = -1.6f,
							Velocity = 2f
						}
					},
					{
						167,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						168,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						170,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(10f, 5f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = -12f
						}
					},
					{
						171,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(10f, 5f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = -12f
						}
					},
					{
						173,
						new NPCBestiaryDrawModifiers(0)
						{
							Rotation = 0.75f,
							Position = new Vector2(0f, -5f)
						}
					},
					{
						174,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -5f),
							Velocity = 1f
						}
					},
					{
						175,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(4f, -2f),
							Rotation = MathF.PI * 3f / 4f
						}
					},
					{
						176,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 5f)
						}
					},
					{
						177,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(10f, 15f),
							PortraitPositionXOverride = -4f,
							PortraitPositionYOverride = 1f,
							Frame = 0
						}
					},
					{
						178,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						179,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-5f, 12f),
							PortraitPositionYOverride = -7f
						}
					},
					{
						180,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(10f, 5f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = -12f
						}
					},
					{
						181,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						185,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						186,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						187,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						188,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						189,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						190,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, -15f),
							PortraitPositionYOverride = -35f
						}
					},
					{
						191,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, -15f),
							PortraitPositionYOverride = -35f
						}
					},
					{
						192,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, -15f),
							PortraitPositionYOverride = -35f
						}
					},
					{
						193,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -15f),
							PortraitPositionYOverride = -35f
						}
					},
					{
						194,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -15f),
							PortraitPositionYOverride = -35f
						}
					},
					{
						196,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						197,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						198,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						199,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						200,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionYOverride = 2f
						}
					},
					{
						201,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						202,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						203,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						206,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 2f
						}
					},
					{
						207,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						208,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						209,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						212,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						213,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						214,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 3f
						}
					},
					{
						215,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 3f
						}
					},
					{
						216,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 3f
						}
					},
					{
						221,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 5f),
							Velocity = 1f
						}
					},
					{
						222,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(10f, 55f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 40f
						}
					},
					{
						223,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						224,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -10f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						226,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, 3f),
							PortraitPositionYOverride = -15f
						}
					},
					{
						227,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = -3f
						}
					},
					{
						228,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = -5f
						}
					},
					{
						229,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						225,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 0f)
						}
					},
					{
						230,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						231,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 5f)
						}
					},
					{
						232,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 5f)
						}
					},
					{
						233,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 5f)
						}
					},
					{
						234,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 5f)
						}
					},
					{
						235,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 5f)
						}
					},
					{
						236,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						237,
						new NPCBestiaryDrawModifiers(0)
						{
							Rotation = -1.6f,
							Velocity = 2f
						}
					},
					{
						238,
						new NPCBestiaryDrawModifiers(0)
						{
							Rotation = -1.6f,
							Velocity = 2f
						}
					},
					{
						239,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						240,
						new NPCBestiaryDrawModifiers(0)
						{
							Rotation = -1.6f,
							Velocity = 2f
						}
					},
					{
						241,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 6f),
							PortraitPositionYOverride = 6f,
							IsWet = true
						}
					},
					{
						242,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 10f)
						}
					},
					{
						243,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 60f),
							Velocity = 1f,
							PortraitPositionYOverride = 15f
						}
					},
					{
						245,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_245",
							Position = new Vector2(2f, 48f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 24f
						}
					},
					{
						246,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						247,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						248,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						249,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						250,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -6f),
							PortraitPositionYOverride = -26f
						}
					},
					{
						251,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						252,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(3f, 3f),
							Velocity = 0.05f
						}
					},
					{
						253,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 5f)
						}
					},
					{
						254,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						255,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = -2f
						}
					},
					{
						256,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 5f)
						}
					},
					{
						257,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						258,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						259,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_259",
							Position = new Vector2(0f, 25f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 8f
						}
					},
					{
						260,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_260",
							Position = new Vector2(0f, 25f),
							PortraitPositionXOverride = 1f,
							PortraitPositionYOverride = 4f
						}
					},
					{
						261,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						262,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 20f),
							Scale = 0.8f
						}
					},
					{
						264,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						263,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						265,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						266,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, 5f),
							Frame = 4
						}
					},
					{
						268,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, -5f)
						}
					},
					{
						269,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-5f, 0f),
							Velocity = 1f
						}
					},
					{
						270,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-5f, 0f),
							Velocity = 1f
						}
					},
					{
						271,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 0f),
							Velocity = 1f
						}
					},
					{
						272,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-5f, 0f),
							Velocity = 1f
						}
					},
					{
						273,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-5f, 0f),
							Velocity = 1f
						}
					},
					{
						274,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-3f, 0f),
							Velocity = 1f
						}
					},
					{
						275,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-3f, 2f),
							PortraitPositionYOverride = 3f,
							Velocity = 1f
						}
					},
					{
						276,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-5f, 0f),
							Velocity = 1f
						}
					},
					{
						277,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-5f, 0f),
							Velocity = 1f
						}
					},
					{
						278,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 0f),
							Velocity = 1f
						}
					},
					{
						279,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-5f, 0f),
							Velocity = 1f
						}
					},
					{
						280,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-3f, 0f),
							Velocity = 1f
						}
					},
					{
						287,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						289,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 10f),
							Direction = 1
						}
					},
					{
						290,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(4f, 6f),
							Velocity = 1f
						}
					},
					{
						291,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						292,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						293,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						294,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						295,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						296,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						297,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, -14f),
							Velocity = 0.05f,
							PortraitPositionYOverride = -30f
						}
					},
					{
						298,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, -14f),
							Velocity = 0.05f,
							PortraitPositionYOverride = -30f
						}
					},
					{
						299,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						301,
						new NPCBestiaryDrawModifiers(0)
						{
							PortraitPositionYOverride = -20f,
							Direction = -1,
							SpriteDirection = 1,
							Velocity = 0.05f
						}
					},
					{
						303,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						305,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.05f
						}
					},
					{
						306,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.05f
						}
					},
					{
						307,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.05f
						}
					},
					{
						308,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.05f
						}
					},
					{
						309,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.05f
						}
					},
					{
						310,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						311,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						312,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						313,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						314,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						315,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(14f, 26f),
							Velocity = 2f,
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						316,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 4f)
						}
					},
					{
						317,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -15f),
							PortraitPositionYOverride = -35f
						}
					},
					{
						318,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, -13f),
							PortraitPositionYOverride = -31f
						}
					},
					{
						319,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						320,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						321,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						322,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						323,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						324,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						325,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 36f)
						}
					},
					{
						326,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						327,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -8f)
						}
					},
					{
						328,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						329,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 2f
						}
					},
					{
						330,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 14f)
						}
					},
					{
						331,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						332,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						337,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						338,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						339,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						340,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						342,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						343,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, 25f),
							Velocity = 1f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						344,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 90f)
						}
					},
					{
						345,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-1f, 90f)
						}
					},
					{
						346,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(30f, 80f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 60f
						}
					},
					{
						347,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 4f)
						}
					},
					{
						348,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Hide = true
						}
					},
					{
						349,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-3f, 18f),
							PortraitPositionYOverride = 0f
						}
					},
					{
						350,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(3f, 0f),
							Velocity = 2f
						}
					},
					{
						351,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, 60f),
							Velocity = 1f,
							PortraitPositionYOverride = 30f
						}
					},
					{
						353,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						633,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						354,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						355,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 2f)
						}
					},
					{
						356,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 3f),
							PortraitPositionYOverride = 1f
						}
					},
					{
						357,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 2f),
							Velocity = 1f
						}
					},
					{
						358,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 2f)
						}
					},
					{
						359,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 18f),
							PortraitPositionYOverride = 40f
						}
					},
					{
						360,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 17f),
							PortraitPositionYOverride = 39f
						}
					},
					{
						362,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(3f, 0f),
							Velocity = 1f
						}
					},
					{
						363,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Hide = true
						}
					},
					{
						364,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(3f, 0f),
							Velocity = 1f
						}
					},
					{
						365,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Hide = true
						}
					},
					{
						366,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 0f),
							Velocity = 1f
						}
					},
					{
						367,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 0f),
							Velocity = 1f
						}
					},
					{
						368,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						369,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						370,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(56f, -4f),
							Direction = 1,
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						371,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						372,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(35f, 4f),
							Velocity = 1f,
							PortraitPositionXOverride = 10f,
							PortraitPositionYOverride = -3f
						}
					},
					{
						373,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						374,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						375,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						376,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						379,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						380,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						381,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 0f),
							Velocity = 1f
						}
					},
					{
						382,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 0f),
							Velocity = 1f
						}
					},
					{
						383,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 0f),
							Velocity = 1f
						}
					},
					{
						384,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						385,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 0f),
							Velocity = 1f
						}
					},
					{
						386,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 0f),
							Velocity = 1f
						}
					},
					{
						387,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 0f),
							Velocity = 3f
						}
					},
					{
						388,
						new NPCBestiaryDrawModifiers(0)
						{
							Direction = 1
						}
					},
					{
						389,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-6f, 0f),
							Velocity = 1f
						}
					},
					{
						390,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(12f, 0f),
							Direction = -1,
							SpriteDirection = 1,
							Velocity = 2f,
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = -12f
						}
					},
					{
						391,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(16f, 12f),
							Direction = -1,
							SpriteDirection = 1,
							Velocity = 2f,
							PortraitPositionXOverride = 3f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						392,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						395,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_395",
							Position = new Vector2(-1f, 18f),
							PortraitPositionXOverride = 1f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						393,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						394,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						396,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						397,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						398,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_398",
							Position = new Vector2(0f, 5f),
							Scale = 0.4f,
							PortraitScale = 0.7f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						400,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						401,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						402,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_402",
							Position = new Vector2(42f, 15f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						403,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						404,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						408,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						410,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						411,
						new NPCBestiaryDrawModifiers(0)
						{
							Direction = -1,
							SpriteDirection = 1,
							Velocity = 1f
						}
					},
					{
						412,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_412",
							Position = new Vector2(50f, 28f),
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 4f
						}
					},
					{
						413,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						414,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						415,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(26f, 0f),
							Velocity = 3f,
							PortraitPositionXOverride = 5f
						}
					},
					{
						416,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, 20f),
							Velocity = 1f,
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						417,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 8f),
							Velocity = 1f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						418,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 4f)
						}
					},
					{
						419,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 0f),
							Velocity = 1f
						}
					},
					{
						420,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 4f),
							Direction = 1
						}
					},
					{
						421,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, -1f)
						}
					},
					{
						422,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 44f),
							Scale = 0.4f,
							PortraitPositionXOverride = 2f,
							PortraitPositionYOverride = 134f
						}
					},
					{
						423,
						new NPCBestiaryDrawModifiers(0)
						{
							Direction = -1,
							SpriteDirection = 1,
							Velocity = 1f
						}
					},
					{
						424,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(4f, 0f),
							Direction = -1,
							SpriteDirection = 1,
							Velocity = 2f
						}
					},
					{
						425,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(4f, 0f),
							Direction = -1,
							SpriteDirection = 1,
							Velocity = 2f
						}
					},
					{
						426,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 8f),
							Velocity = 2f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						427,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionYOverride = -4f
						}
					},
					{
						428,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						429,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 0f),
							Velocity = 1f
						}
					},
					{
						430,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						431,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						432,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						433,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						434,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						435,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						436,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						437,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						439,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						440,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						441,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						442,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, -14f),
							Velocity = 0.05f,
							PortraitPositionYOverride = -30f
						}
					},
					{
						443,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						444,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 2f),
							PortraitPositionYOverride = 0f
						}
					},
					{
						448,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						449,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						450,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						451,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						452,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						453,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						454,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_454",
							Position = new Vector2(57f, 10f),
							PortraitPositionXOverride = 5f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						455,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						456,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						457,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						458,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						459,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						460,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						461,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 0f),
							Velocity = 1f
						}
					},
					{
						462,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						463,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						464,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						465,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 6f),
							PortraitPositionYOverride = 6f,
							IsWet = true
						}
					},
					{
						466,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						467,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						468,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						469,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						470,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						471,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						472,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 0f),
							PortraitPositionYOverride = -30f,
							SpriteDirection = -1,
							Velocity = 1f
						}
					},
					{
						476,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						477,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(25f, 6f),
							PortraitPositionXOverride = 10f
						}
					},
					{
						478,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						479,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, 4f),
							PortraitPositionYOverride = -15f
						}
					},
					{
						481,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						482,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						483,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -10f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						484,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						487,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.5f
						}
					},
					{
						486,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.5f
						}
					},
					{
						485,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.5f
						}
					},
					{
						489,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						491,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_491",
							Position = new Vector2(30f, -5f),
							Scale = 0.8f,
							PortraitPositionXOverride = 1f,
							PortraitPositionYOverride = -1f
						}
					},
					{
						492,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						493,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 44f),
							Scale = 0.4f,
							PortraitPositionXOverride = 2f,
							PortraitPositionYOverride = 134f
						}
					},
					{
						494,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						495,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-4f, 0f),
							Velocity = 1f
						}
					},
					{
						496,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.5f
						}
					},
					{
						497,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.5f
						}
					},
					{
						498,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						499,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						500,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						501,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						502,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						503,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						504,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						505,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						506,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						507,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 44f),
							Scale = 0.4f,
							PortraitPositionXOverride = 2f,
							PortraitPositionYOverride = 134f
						}
					},
					{
						508,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Position = new Vector2(10f, 0f),
							PortraitPositionXOverride = 0f
						}
					},
					{
						509,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, 0f),
							PortraitPositionXOverride = -10f,
							PortraitPositionYOverride = -20f
						}
					},
					{
						510,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_510",
							Position = new Vector2(55f, 18f),
							PortraitPositionXOverride = 10f,
							PortraitPositionYOverride = 12f
						}
					},
					{
						512,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						511,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						513,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_513",
							Position = new Vector2(37f, 24f),
							PortraitPositionXOverride = 10f,
							PortraitPositionYOverride = 17f
						}
					},
					{
						514,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						515,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						516,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						517,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 44f),
							Scale = 0.4f,
							PortraitPositionXOverride = 2f,
							PortraitPositionYOverride = 135f
						}
					},
					{
						518,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-17f, 0f),
							Velocity = 1f
						}
					},
					{
						519,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						520,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 56f),
							Velocity = 1f,
							PortraitPositionYOverride = 10f
						}
					},
					{
						521,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(5f, 5f),
							PortraitPositionYOverride = -10f,
							SpriteDirection = -1,
							Velocity = 0.05f
						}
					},
					{
						522,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						523,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						524,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						525,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						526,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						527,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						528,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						529,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						530,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						531,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 4f),
							Velocity = 2f,
							PortraitPositionYOverride = 10f
						}
					},
					{
						532,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, 0f),
							Velocity = 1f
						}
					},
					{
						533,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(3f, 5f)
						}
					},
					{
						534,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						536,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, 0f),
							Velocity = 1f
						}
					},
					{
						538,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						539,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						540,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						541,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 30f),
							PortraitPositionYOverride = 0f
						}
					},
					{
						542,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(35f, -3f),
							PortraitPositionXOverride = 0f
						}
					},
					{
						543,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(35f, -3f),
							PortraitPositionXOverride = 0f
						}
					},
					{
						544,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(35f, -3f),
							PortraitPositionXOverride = 0f
						}
					},
					{
						545,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(35f, -3f),
							PortraitPositionXOverride = 0f
						}
					},
					{
						546,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -3f),
							Direction = 1
						}
					},
					{
						547,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						548,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						549,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						550,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = -2f
						}
					},
					{
						551,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(95f, -4f)
						}
					},
					{
						552,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						553,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						554,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						555,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						556,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						557,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						558,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(3f, -2f)
						}
					},
					{
						559,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(3f, -2f)
						}
					},
					{
						560,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(3f, -2f)
						}
					},
					{
						561,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						562,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						563,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						566,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-3f, 0f),
							Velocity = 1f
						}
					},
					{
						567,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-3f, 0f),
							Velocity = 1f
						}
					},
					{
						568,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						569,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						570,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(10f, 5f),
							Velocity = 1f,
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 2f
						}
					},
					{
						571,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(10f, 5f),
							Velocity = 1f,
							PortraitPositionXOverride = 0f,
							PortraitPositionYOverride = 2f
						}
					},
					{
						572,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						573,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						578,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 4f)
						}
					},
					{
						574,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(16f, 6f),
							Velocity = 1f
						}
					},
					{
						575,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(16f, 6f),
							Velocity = 1f
						}
					},
					{
						576,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(20f, 70f),
							Velocity = 1f,
							PortraitPositionXOverride = 10f,
							PortraitPositionYOverride = 0f,
							PortraitScale = 0.75f
						}
					},
					{
						577,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(20f, 70f),
							Velocity = 1f,
							PortraitPositionXOverride = 10f,
							PortraitPositionYOverride = 0f,
							PortraitScale = 0.75f
						}
					},
					{
						580,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 0f),
							Scale = 0.9f,
							Velocity = 1f
						}
					},
					{
						581,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -8f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						582,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						585,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 1f),
							Direction = 1
						}
					},
					{
						584,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 1f),
							Direction = 1
						}
					},
					{
						583,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 1f),
							Direction = 1
						}
					},
					{
						586,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						579,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						588,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 1f
						}
					},
					{
						587,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, -14f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						591,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(9f, 0f)
						}
					},
					{
						590,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 2f
						}
					},
					{
						592,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 6f),
							PortraitPositionYOverride = 7f,
							IsWet = true
						}
					},
					{
						593,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						594,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_594",
							Scale = 0.8f
						}
					},
					{
						589,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						602,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						603,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						604,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 22f),
							PortraitPositionYOverride = 41f
						}
					},
					{
						605,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 22f),
							PortraitPositionYOverride = 41f
						}
					},
					{
						606,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						607,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 6f),
							PortraitPositionYOverride = 7f,
							IsWet = true
						}
					},
					{
						608,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						609,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						611,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 0f),
							Direction = -1,
							SpriteDirection = 1
						}
					},
					{
						612,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						613,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						614,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						615,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 10f),
							Scale = 0.88f,
							PortraitPositionYOverride = 20f,
							IsWet = true
						}
					},
					{
						616,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						617,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						618,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(12f, -5f),
							Scale = 0.9f,
							PortraitPositionYOverride = 0f
						}
					},
					{
						619,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 7f),
							Scale = 0.85f,
							PortraitPositionYOverride = 10f
						}
					},
					{
						620,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(6f, 5f),
							Scale = 0.78f,
							Velocity = 1f
						}
					},
					{
						621,
						new NPCBestiaryDrawModifiers(0)
						{
							CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_621",
							Position = new Vector2(46f, 20f),
							PortraitPositionXOverride = 10f,
							PortraitPositionYOverride = 17f
						}
					},
					{
						622,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						623,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						624,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 2f
						}
					},
					{
						625,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -12f),
							Velocity = 1f
						}
					},
					{
						626,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -16f)
						}
					},
					{
						627,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, -16f)
						}
					},
					{
						628,
						new NPCBestiaryDrawModifiers(0)
						{
							Direction = 1
						}
					},
					{
						630,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.5f
						}
					},
					{
						632,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						631,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.75f
						}
					},
					{
						634,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							Position = new Vector2(0f, -13f),
							PortraitPositionYOverride = -30f
						}
					},
					{
						635,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						636,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 50f),
							PortraitPositionYOverride = 30f
						}
					},
					{
						639,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						640,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						641,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						642,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						643,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						644,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						645,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						646,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						647,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						648,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						649,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						650,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						651,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						652,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						637,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0.25f
						}
					},
					{
						638,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						653,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 3f),
							PortraitPositionYOverride = 1f
						}
					},
					{
						654,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 2f)
						}
					},
					{
						655,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 17f),
							PortraitPositionYOverride = 39f
						}
					},
					{
						656,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f
						}
					},
					{
						657,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(0f, 60f),
							PortraitPositionYOverride = 40f
						}
					},
					{
						660,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(-2f, -4f),
							PortraitPositionYOverride = -20f
						}
					},
					{
						661,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 3f),
							PortraitPositionYOverride = 1f
						}
					},
					{
						662,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						663,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 1f,
							PortraitPositionXOverride = 1f
						}
					},
					{
						664,
						new NPCBestiaryDrawModifiers(0)
					},
					{
						667,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						668,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 2.5f,
							Position = new Vector2(36f, 40f),
							Scale = 0.9f,
							PortraitPositionXOverride = 6f,
							PortraitPositionYOverride = 50f
						}
					},
					{
						669,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(2f, 22f),
							PortraitPositionYOverride = 41f
						}
					},
					{
						670,
						new NPCBestiaryDrawModifiers(0)
						{
							SpriteDirection = 1,
							Velocity = 0.7f
						}
					},
					{
						678,
						new NPCBestiaryDrawModifiers(0)
						{
							SpriteDirection = 1,
							Velocity = 0.7f
						}
					},
					{
						679,
						new NPCBestiaryDrawModifiers(0)
						{
							SpriteDirection = 1,
							Velocity = 0.7f
						}
					},
					{
						680,
						new NPCBestiaryDrawModifiers(0)
						{
							SpriteDirection = 1,
							Velocity = 0.7f
						}
					},
					{
						681,
						new NPCBestiaryDrawModifiers(0)
						{
							SpriteDirection = 1,
							Velocity = 0.7f
						}
					},
					{
						682,
						new NPCBestiaryDrawModifiers(0)
						{
							SpriteDirection = 1,
							Velocity = 0.7f
						}
					},
					{
						683,
						new NPCBestiaryDrawModifiers(0)
						{
							SpriteDirection = 1,
							Velocity = 0.7f
						}
					},
					{
						684,
						new NPCBestiaryDrawModifiers(0)
						{
							SpriteDirection = 1,
							Velocity = 0.7f
						}
					},
					{
						671,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, -18f),
							Velocity = 0.05f,
							PortraitPositionYOverride = -35f
						}
					},
					{
						672,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, -18f),
							Velocity = 0.05f,
							PortraitPositionYOverride = -35f
						}
					},
					{
						673,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, -16f),
							Velocity = 0.05f,
							PortraitPositionYOverride = -35f
						}
					},
					{
						674,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, -16f),
							Velocity = 0.05f,
							PortraitPositionYOverride = -35f
						}
					},
					{
						675,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, -16f),
							Velocity = 0.05f,
							PortraitPositionYOverride = -35f
						}
					},
					{
						677,
						new NPCBestiaryDrawModifiers(0)
						{
							Position = new Vector2(1f, 2f)
						}
					},
					{
						685,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						686,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						687,
						new NPCBestiaryDrawModifiers(0)
						{
							Velocity = 0f
						}
					},
					{
						0,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					},
					{
						488,
						new NPCBestiaryDrawModifiers(0)
						{
							Hide = true
						}
					}
				};
			}
		}

		private static readonly int[] NetIdMap = new int[65]
		{
			81, 81, 1, 1, 1, 1, 1, 1, 1, 1,
			6, 6, 31, 31, 77, 42, 42, 176, 176, 176,
			176, 173, 173, 183, 183, 3, 3, 132, 132, 186,
			186, 187, 187, 188, 188, 189, 189, 190, 191, 192,
			193, 194, 2, 200, 200, 21, 21, 201, 201, 202,
			202, 203, 203, 223, 223, 231, 231, 232, 232, 233,
			233, 234, 234, 235, 235
		};

		private static readonly Dictionary<string, int> LegacyNameToIdMap = new Dictionary<string, int>
		{
			{ "Slimeling", -1 },
			{ "Slimer2", -2 },
			{ "Green Slime", -3 },
			{ "Pinky", -4 },
			{ "Baby Slime", -5 },
			{ "Black Slime", -6 },
			{ "Purple Slime", -7 },
			{ "Red Slime", -8 },
			{ "Yellow Slime", -9 },
			{ "Jungle Slime", -10 },
			{ "Little Eater", -11 },
			{ "Big Eater", -12 },
			{ "Short Bones", -13 },
			{ "Big Boned", -14 },
			{ "Heavy Skeleton", -15 },
			{ "Little Stinger", -16 },
			{ "Big Stinger", -17 },
			{ "Tiny Moss Hornet", -18 },
			{ "Little Moss Hornet", -19 },
			{ "Big Moss Hornet", -20 },
			{ "Giant Moss Hornet", -21 },
			{ "Little Crimera", -22 },
			{ "Big Crimera", -23 },
			{ "Little Crimslime", -24 },
			{ "Big Crimslime", -25 },
			{ "Small Zombie", -26 },
			{ "Big Zombie", -27 },
			{ "Small Bald Zombie", -28 },
			{ "Big Bald Zombie", -29 },
			{ "Small Pincushion Zombie", -30 },
			{ "Big Pincushion Zombie", -31 },
			{ "Small Slimed Zombie", -32 },
			{ "Big Slimed Zombie", -33 },
			{ "Small Swamp Zombie", -34 },
			{ "Big Swamp Zombie", -35 },
			{ "Small Twiggy Zombie", -36 },
			{ "Big Twiggy Zombie", -37 },
			{ "Cataract Eye 2", -38 },
			{ "Sleepy Eye 2", -39 },
			{ "Dialated Eye 2", -40 },
			{ "Green Eye 2", -41 },
			{ "Purple Eye 2", -42 },
			{ "Demon Eye 2", -43 },
			{ "Small Female Zombie", -44 },
			{ "Big Female Zombie", -45 },
			{ "Small Skeleton", -46 },
			{ "Big Skeleton", -47 },
			{ "Small Headache Skeleton", -48 },
			{ "Big Headache Skeleton", -49 },
			{ "Small Misassembled Skeleton", -50 },
			{ "Big Misassembled Skeleton", -51 },
			{ "Small Pantless Skeleton", -52 },
			{ "Big Pantless Skeleton", -53 },
			{ "Small Rain Zombie", -54 },
			{ "Big Rain Zombie", -55 },
			{ "Little Hornet Fatty", -56 },
			{ "Big Hornet Fatty", -57 },
			{ "Little Hornet Honey", -58 },
			{ "Big Hornet Honey", -59 },
			{ "Little Hornet Leafy", -60 },
			{ "Big Hornet Leafy", -61 },
			{ "Little Hornet Spikey", -62 },
			{ "Big Hornet Spikey", -63 },
			{ "Little Hornet Stingy", -64 },
			{ "Big Hornet Stingy", -65 },
			{ "Blue Slime", 1 },
			{ "Demon Eye", 2 },
			{ "Zombie", 3 },
			{ "Eye of Cthulhu", 4 },
			{ "Servant of Cthulhu", 5 },
			{ "Eater of Souls", 6 },
			{ "Devourer", 7 },
			{ "Giant Worm", 10 },
			{ "Eater of Worlds", 13 },
			{ "Mother Slime", 16 },
			{ "Merchant", 17 },
			{ "Nurse", 18 },
			{ "Arms Dealer", 19 },
			{ "Dryad", 20 },
			{ "Skeleton", 21 },
			{ "Guide", 22 },
			{ "Meteor Head", 23 },
			{ "Fire Imp", 24 },
			{ "Burning Sphere", 25 },
			{ "Goblin Peon", 26 },
			{ "Goblin Thief", 27 },
			{ "Goblin Warrior", 28 },
			{ "Goblin Sorcerer", 29 },
			{ "Chaos Ball", 30 },
			{ "Angry Bones", 31 },
			{ "Dark Caster", 32 },
			{ "Water Sphere", 33 },
			{ "Cursed Skull", 34 },
			{ "Skeletron", 35 },
			{ "Old Man", 37 },
			{ "Demolitionist", 38 },
			{ "Bone Serpent", 39 },
			{ "Hornet", 42 },
			{ "Man Eater", 43 },
			{ "Undead Miner", 44 },
			{ "Tim", 45 },
			{ "Bunny", 46 },
			{ "Corrupt Bunny", 47 },
			{ "Harpy", 48 },
			{ "Cave Bat", 49 },
			{ "King Slime", 50 },
			{ "Jungle Bat", 51 },
			{ "Doctor Bones", 52 },
			{ "The Groom", 53 },
			{ "Clothier", 54 },
			{ "Goldfish", 55 },
			{ "Snatcher", 56 },
			{ "Corrupt Goldfish", 57 },
			{ "Piranha", 58 },
			{ "Lava Slime", 59 },
			{ "Hellbat", 60 },
			{ "Vulture", 61 },
			{ "Demon", 62 },
			{ "Blue Jellyfish", 63 },
			{ "Pink Jellyfish", 64 },
			{ "Shark", 65 },
			{ "Voodoo Demon", 66 },
			{ "Crab", 67 },
			{ "Dungeon Guardian", 68 },
			{ "Antlion", 69 },
			{ "Spike Ball", 70 },
			{ "Dungeon Slime", 71 },
			{ "Blazing Wheel", 72 },
			{ "Goblin Scout", 73 },
			{ "Bird", 74 },
			{ "Pixie", 75 },
			{ "Armored Skeleton", 77 },
			{ "Mummy", 78 },
			{ "Dark Mummy", 79 },
			{ "Light Mummy", 80 },
			{ "Corrupt Slime", 81 },
			{ "Wraith", 82 },
			{ "Cursed Hammer", 83 },
			{ "Enchanted Sword", 84 },
			{ "Mimic", 85 },
			{ "Unicorn", 86 },
			{ "Wyvern", 87 },
			{ "Giant Bat", 93 },
			{ "Corruptor", 94 },
			{ "Digger", 95 },
			{ "World Feeder", 98 },
			{ "Clinger", 101 },
			{ "Angler Fish", 102 },
			{ "Green Jellyfish", 103 },
			{ "Werewolf", 104 },
			{ "Bound Goblin", 105 },
			{ "Bound Wizard", 106 },
			{ "Goblin Tinkerer", 107 },
			{ "Wizard", 108 },
			{ "Clown", 109 },
			{ "Skeleton Archer", 110 },
			{ "Goblin Archer", 111 },
			{ "Vile Spit", 112 },
			{ "Wall of Flesh", 113 },
			{ "The Hungry", 115 },
			{ "Leech", 117 },
			{ "Chaos Elemental", 120 },
			{ "Slimer", 121 },
			{ "Gastropod", 122 },
			{ "Bound Mechanic", 123 },
			{ "Mechanic", 124 },
			{ "Retinazer", 125 },
			{ "Spazmatism", 126 },
			{ "Skeletron Prime", 127 },
			{ "Prime Cannon", 128 },
			{ "Prime Saw", 129 },
			{ "Prime Vice", 130 },
			{ "Prime Laser", 131 },
			{ "Wandering Eye", 133 },
			{ "The Destroyer", 134 },
			{ "Illuminant Bat", 137 },
			{ "Illuminant Slime", 138 },
			{ "Probe", 139 },
			{ "Possessed Armor", 140 },
			{ "Toxic Sludge", 141 },
			{ "Santa Claus", 142 },
			{ "Snowman Gangsta", 143 },
			{ "Mister Stabby", 144 },
			{ "Snow Balla", 145 },
			{ "Ice Slime", 147 },
			{ "Penguin", 148 },
			{ "Ice Bat", 150 },
			{ "Lava Bat", 151 },
			{ "Giant Flying Fox", 152 },
			{ "Giant Tortoise", 153 },
			{ "Ice Tortoise", 154 },
			{ "Wolf", 155 },
			{ "Red Devil", 156 },
			{ "Arapaima", 157 },
			{ "Vampire", 158 },
			{ "Truffle", 160 },
			{ "Zombie Eskimo", 161 },
			{ "Frankenstein", 162 },
			{ "Black Recluse", 163 },
			{ "Wall Creeper", 164 },
			{ "Swamp Thing", 166 },
			{ "Undead Viking", 167 },
			{ "Corrupt Penguin", 168 },
			{ "Ice Elemental", 169 },
			{ "Pigron", 170 },
			{ "Rune Wizard", 172 },
			{ "Crimera", 173 },
			{ "Herpling", 174 },
			{ "Angry Trapper", 175 },
			{ "Moss Hornet", 176 },
			{ "Derpling", 177 },
			{ "Steampunker", 178 },
			{ "Crimson Axe", 179 },
			{ "Face Monster", 181 },
			{ "Floaty Gross", 182 },
			{ "Crimslime", 183 },
			{ "Spiked Ice Slime", 184 },
			{ "Snow Flinx", 185 },
			{ "Lost Girl", 195 },
			{ "Nymph", 196 },
			{ "Armored Viking", 197 },
			{ "Lihzahrd", 198 },
			{ "Spiked Jungle Slime", 204 },
			{ "Moth", 205 },
			{ "Icy Merman", 206 },
			{ "Dye Trader", 207 },
			{ "Party Girl", 208 },
			{ "Cyborg", 209 },
			{ "Bee", 210 },
			{ "Pirate Deckhand", 212 },
			{ "Pirate Corsair", 213 },
			{ "Pirate Deadeye", 214 },
			{ "Pirate Crossbower", 215 },
			{ "Pirate Captain", 216 },
			{ "Cochineal Beetle", 217 },
			{ "Cyan Beetle", 218 },
			{ "Lac Beetle", 219 },
			{ "Sea Snail", 220 },
			{ "Squid", 221 },
			{ "Queen Bee", 222 },
			{ "Raincoat Zombie", 223 },
			{ "Flying Fish", 224 },
			{ "Umbrella Slime", 225 },
			{ "Flying Snake", 226 },
			{ "Painter", 227 },
			{ "Witch Doctor", 228 },
			{ "Pirate", 229 },
			{ "Jungle Creeper", 236 },
			{ "Blood Crawler", 239 },
			{ "Blood Feeder", 241 },
			{ "Blood Jelly", 242 },
			{ "Ice Golem", 243 },
			{ "Rainbow Slime", 244 },
			{ "Golem", 245 },
			{ "Golem Head", 246 },
			{ "Golem Fist", 247 },
			{ "Angry Nimbus", 250 },
			{ "Eyezor", 251 },
			{ "Parrot", 252 },
			{ "Reaper", 253 },
			{ "Spore Zombie", 254 },
			{ "Fungo Fish", 256 },
			{ "Anomura Fungus", 257 },
			{ "Mushi Ladybug", 258 },
			{ "Fungi Bulb", 259 },
			{ "Giant Fungi Bulb", 260 },
			{ "Fungi Spore", 261 },
			{ "Plantera", 262 },
			{ "Plantera's Hook", 263 },
			{ "Plantera's Tentacle", 264 },
			{ "Spore", 265 },
			{ "Brain of Cthulhu", 266 },
			{ "Creeper", 267 },
			{ "Ichor Sticker", 268 },
			{ "Rusty Armored Bones", 269 },
			{ "Blue Armored Bones", 273 },
			{ "Hell Armored Bones", 277 },
			{ "Ragged Caster", 281 },
			{ "Necromancer", 283 },
			{ "Diabolist", 285 },
			{ "Bone Lee", 287 },
			{ "Dungeon Spirit", 288 },
			{ "Giant Cursed Skull", 289 },
			{ "Paladin", 290 },
			{ "Skeleton Sniper", 291 },
			{ "Tactical Skeleton", 292 },
			{ "Skeleton Commando", 293 },
			{ "Blue Jay", 297 },
			{ "Cardinal", 298 },
			{ "Squirrel", 299 },
			{ "Mouse", 300 },
			{ "Raven", 301 },
			{ "Slime", 302 },
			{ "Hoppin' Jack", 304 },
			{ "Scarecrow", 305 },
			{ "Headless Horseman", 315 },
			{ "Ghost", 316 },
			{ "Mourning Wood", 325 },
			{ "Splinterling", 326 },
			{ "Pumpking", 327 },
			{ "Hellhound", 329 },
			{ "Poltergeist", 330 },
			{ "Zombie Elf", 338 },
			{ "Present Mimic", 341 },
			{ "Gingerbread Man", 342 },
			{ "Yeti", 343 },
			{ "Everscream", 344 },
			{ "Ice Queen", 345 },
			{ "Santa", 346 },
			{ "Elf Copter", 347 },
			{ "Nutcracker", 348 },
			{ "Elf Archer", 350 },
			{ "Krampus", 351 },
			{ "Flocko", 352 },
			{ "Stylist", 353 },
			{ "Webbed Stylist", 354 },
			{ "Firefly", 355 },
			{ "Butterfly", 356 },
			{ "Worm", 357 },
			{ "Lightning Bug", 358 },
			{ "Snail", 359 },
			{ "Glowing Snail", 360 },
			{ "Frog", 361 },
			{ "Duck", 362 },
			{ "Scorpion", 366 },
			{ "Traveling Merchant", 368 },
			{ "Angler", 369 },
			{ "Duke Fishron", 370 },
			{ "Detonating Bubble", 371 },
			{ "Sharkron", 372 },
			{ "Truffle Worm", 374 },
			{ "Sleeping Angler", 376 },
			{ "Grasshopper", 377 },
			{ "Chattering Teeth Bomb", 378 },
			{ "Blue Cultist Archer", 379 },
			{ "White Cultist Archer", 380 },
			{ "Brain Scrambler", 381 },
			{ "Ray Gunner", 382 },
			{ "Martian Officer", 383 },
			{ "Bubble Shield", 384 },
			{ "Gray Grunt", 385 },
			{ "Martian Engineer", 386 },
			{ "Tesla Turret", 387 },
			{ "Martian Drone", 388 },
			{ "Gigazapper", 389 },
			{ "Scutlix Gunner", 390 },
			{ "Scutlix", 391 },
			{ "Martian Saucer", 392 },
			{ "Martian Saucer Turret", 393 },
			{ "Martian Saucer Cannon", 394 },
			{ "Moon Lord", 396 },
			{ "Moon Lord's Hand", 397 },
			{ "Moon Lord's Core", 398 },
			{ "Martian Probe", 399 },
			{ "Milkyway Weaver", 402 },
			{ "Star Cell", 405 },
			{ "Flow Invader", 407 },
			{ "Twinkle Popper", 409 },
			{ "Twinkle", 410 },
			{ "Stargazer", 411 },
			{ "Crawltipede", 412 },
			{ "Drakomire", 415 },
			{ "Drakomire Rider", 416 },
			{ "Sroller", 417 },
			{ "Corite", 418 },
			{ "Selenian", 419 },
			{ "Nebula Floater", 420 },
			{ "Brain Suckler", 421 },
			{ "Vortex Pillar", 422 },
			{ "Evolution Beast", 423 },
			{ "Predictor", 424 },
			{ "Storm Diver", 425 },
			{ "Alien Queen", 426 },
			{ "Alien Hornet", 427 },
			{ "Alien Larva", 428 },
			{ "Vortexian", 429 },
			{ "Mysterious Tablet", 437 },
			{ "Lunatic Devote", 438 },
			{ "Lunatic Cultist", 439 },
			{ "Tax Collector", 441 },
			{ "Gold Bird", 442 },
			{ "Gold Bunny", 443 },
			{ "Gold Butterfly", 444 },
			{ "Gold Frog", 445 },
			{ "Gold Grasshopper", 446 },
			{ "Gold Mouse", 447 },
			{ "Gold Worm", 448 },
			{ "Phantasm Dragon", 454 },
			{ "Butcher", 460 },
			{ "Creature from the Deep", 461 },
			{ "Fritz", 462 },
			{ "Nailhead", 463 },
			{ "Crimtane Bunny", 464 },
			{ "Crimtane Goldfish", 465 },
			{ "Psycho", 466 },
			{ "Deadly Sphere", 467 },
			{ "Dr. Man Fly", 468 },
			{ "The Possessed", 469 },
			{ "Vicious Penguin", 470 },
			{ "Goblin Summoner", 471 },
			{ "Shadowflame Apparation", 472 },
			{ "Corrupt Mimic", 473 },
			{ "Crimson Mimic", 474 },
			{ "Hallowed Mimic", 475 },
			{ "Jungle Mimic", 476 },
			{ "Mothron", 477 },
			{ "Mothron Egg", 478 },
			{ "Baby Mothron", 479 },
			{ "Medusa", 480 },
			{ "Hoplite", 481 },
			{ "Granite Golem", 482 },
			{ "Granite Elemental", 483 },
			{ "Enchanted Nightcrawler", 484 },
			{ "Grubby", 485 },
			{ "Sluggy", 486 },
			{ "Buggy", 487 },
			{ "Target Dummy", 488 },
			{ "Blood Zombie", 489 },
			{ "Drippler", 490 },
			{ "Stardust Pillar", 493 },
			{ "Crawdad", 494 },
			{ "Giant Shelly", 496 },
			{ "Salamander", 498 },
			{ "Nebula Pillar", 507 },
			{ "Antlion Charger", 508 },
			{ "Antlion Swarmer", 509 },
			{ "Dune Splicer", 510 },
			{ "Tomb Crawler", 513 },
			{ "Solar Flare", 516 },
			{ "Solar Pillar", 517 },
			{ "Drakanian", 518 },
			{ "Solar Fragment", 519 },
			{ "Martian Walker", 520 },
			{ "Ancient Vision", 521 },
			{ "Ancient Light", 522 },
			{ "Ancient Doom", 523 },
			{ "Ghoul", 524 },
			{ "Vile Ghoul", 525 },
			{ "Tainted Ghoul", 526 },
			{ "Dreamer Ghoul", 527 },
			{ "Lamia", 528 },
			{ "Sand Poacher", 530 },
			{ "Basilisk", 532 },
			{ "Desert Spirit", 533 },
			{ "Tortured Soul", 534 },
			{ "Spiked Slime", 535 },
			{ "The Bride", 536 },
			{ "Sand Slime", 537 },
			{ "Red Squirrel", 538 },
			{ "Gold Squirrel", 539 },
			{ "Sand Elemental", 541 },
			{ "Sand Shark", 542 },
			{ "Bone Biter", 543 },
			{ "Flesh Reaver", 544 },
			{ "Crystal Thresher", 545 },
			{ "Angry Tumbler", 546 },
			{ "???", 547 },
			{ "Eternia Crystal", 548 },
			{ "Mysterious Portal", 549 },
			{ "Tavernkeep", 550 },
			{ "Betsy", 551 },
			{ "Etherian Goblin", 552 },
			{ "Etherian Goblin Bomber", 555 },
			{ "Etherian Wyvern", 558 },
			{ "Etherian Javelin Thrower", 561 },
			{ "Dark Mage", 564 },
			{ "Old One's Skeleton", 566 },
			{ "Wither Beast", 568 },
			{ "Drakin", 570 },
			{ "Kobold", 572 },
			{ "Kobold Glider", 574 },
			{ "Ogre", 576 },
			{ "Etherian Lightning Bug", 578 }
		};

		public static readonly IdDictionary Search = IdDictionary.Create<NPCID, short>();

		public const short NegativeIDCount = -66;

		public const short BigHornetStingy = -65;

		public const short LittleHornetStingy = -64;

		public const short BigHornetSpikey = -63;

		public const short LittleHornetSpikey = -62;

		public const short BigHornetLeafy = -61;

		public const short LittleHornetLeafy = -60;

		public const short BigHornetHoney = -59;

		public const short LittleHornetHoney = -58;

		public const short BigHornetFatty = -57;

		public const short LittleHornetFatty = -56;

		public const short BigRainZombie = -55;

		public const short SmallRainZombie = -54;

		public const short BigPantlessSkeleton = -53;

		public const short SmallPantlessSkeleton = -52;

		public const short BigMisassembledSkeleton = -51;

		public const short SmallMisassembledSkeleton = -50;

		public const short BigHeadacheSkeleton = -49;

		public const short SmallHeadacheSkeleton = -48;

		public const short BigSkeleton = -47;

		public const short SmallSkeleton = -46;

		public const short BigFemaleZombie = -45;

		public const short SmallFemaleZombie = -44;

		public const short DemonEye2 = -43;

		public const short PurpleEye2 = -42;

		public const short GreenEye2 = -41;

		public const short DialatedEye2 = -40;

		public const short SleepyEye2 = -39;

		public const short CataractEye2 = -38;

		public const short BigTwiggyZombie = -37;

		public const short SmallTwiggyZombie = -36;

		public const short BigSwampZombie = -35;

		public const short SmallSwampZombie = -34;

		public const short BigSlimedZombie = -33;

		public const short SmallSlimedZombie = -32;

		public const short BigPincushionZombie = -31;

		public const short SmallPincushionZombie = -30;

		public const short BigBaldZombie = -29;

		public const short SmallBaldZombie = -28;

		public const short BigZombie = -27;

		public const short SmallZombie = -26;

		public const short BigCrimslime = -25;

		public const short LittleCrimslime = -24;

		public const short BigCrimera = -23;

		public const short LittleCrimera = -22;

		public const short GiantMossHornet = -21;

		public const short BigMossHornet = -20;

		public const short LittleMossHornet = -19;

		public const short TinyMossHornet = -18;

		public const short BigStinger = -17;

		public const short LittleStinger = -16;

		public const short HeavySkeleton = -15;

		public const short BigBoned = -14;

		public const short ShortBones = -13;

		public const short BigEater = -12;

		public const short LittleEater = -11;

		public const short JungleSlime = -10;

		public const short YellowSlime = -9;

		public const short RedSlime = -8;

		public const short PurpleSlime = -7;

		public const short BlackSlime = -6;

		public const short BabySlime = -5;

		public const short Pinky = -4;

		public const short GreenSlime = -3;

		public const short Slimer2 = -2;

		public const short Slimeling = -1;

		public const short None = 0;

		public const short BlueSlime = 1;

		public const short DemonEye = 2;

		public const short Zombie = 3;

		public const short EyeofCthulhu = 4;

		public const short ServantofCthulhu = 5;

		public const short EaterofSouls = 6;

		public const short DevourerHead = 7;

		public const short DevourerBody = 8;

		public const short DevourerTail = 9;

		public const short GiantWormHead = 10;

		public const short GiantWormBody = 11;

		public const short GiantWormTail = 12;

		public const short EaterofWorldsHead = 13;

		public const short EaterofWorldsBody = 14;

		public const short EaterofWorldsTail = 15;

		public const short MotherSlime = 16;

		public const short Merchant = 17;

		public const short Nurse = 18;

		public const short ArmsDealer = 19;

		public const short Dryad = 20;

		public const short Skeleton = 21;

		public const short Guide = 22;

		public const short MeteorHead = 23;

		public const short FireImp = 24;

		public const short BurningSphere = 25;

		public const short GoblinPeon = 26;

		public const short GoblinThief = 27;

		public const short GoblinWarrior = 28;

		public const short GoblinSorcerer = 29;

		public const short ChaosBall = 30;

		public const short AngryBones = 31;

		public const short DarkCaster = 32;

		public const short WaterSphere = 33;

		public const short CursedSkull = 34;

		public const short SkeletronHead = 35;

		public const short SkeletronHand = 36;

		public const short OldMan = 37;

		public const short Demolitionist = 38;

		public const short BoneSerpentHead = 39;

		public const short BoneSerpentBody = 40;

		public const short BoneSerpentTail = 41;

		public const short Hornet = 42;

		public const short ManEater = 43;

		public const short UndeadMiner = 44;

		public const short Tim = 45;

		public const short Bunny = 46;

		public const short CorruptBunny = 47;

		public const short Harpy = 48;

		public const short CaveBat = 49;

		public const short KingSlime = 50;

		public const short JungleBat = 51;

		public const short DoctorBones = 52;

		public const short TheGroom = 53;

		public const short Clothier = 54;

		public const short Goldfish = 55;

		public const short Snatcher = 56;

		public const short CorruptGoldfish = 57;

		public const short Piranha = 58;

		public const short LavaSlime = 59;

		public const short Hellbat = 60;

		public const short Vulture = 61;

		public const short Demon = 62;

		public const short BlueJellyfish = 63;

		public const short PinkJellyfish = 64;

		public const short Shark = 65;

		public const short VoodooDemon = 66;

		public const short Crab = 67;

		public const short DungeonGuardian = 68;

		public const short Antlion = 69;

		public const short SpikeBall = 70;

		public const short DungeonSlime = 71;

		public const short BlazingWheel = 72;

		public const short GoblinScout = 73;

		public const short Bird = 74;

		public const short Pixie = 75;

		public const short None2 = 76;

		public const short ArmoredSkeleton = 77;

		public const short Mummy = 78;

		public const short DarkMummy = 79;

		public const short LightMummy = 80;

		public const short CorruptSlime = 81;

		public const short Wraith = 82;

		public const short CursedHammer = 83;

		public const short EnchantedSword = 84;

		public const short Mimic = 85;

		public const short Unicorn = 86;

		public const short WyvernHead = 87;

		public const short WyvernLegs = 88;

		public const short WyvernBody = 89;

		public const short WyvernBody2 = 90;

		public const short WyvernBody3 = 91;

		public const short WyvernTail = 92;

		public const short GiantBat = 93;

		public const short Corruptor = 94;

		public const short DiggerHead = 95;

		public const short DiggerBody = 96;

		public const short DiggerTail = 97;

		public const short SeekerHead = 98;

		public const short SeekerBody = 99;

		public const short SeekerTail = 100;

		public const short Clinger = 101;

		public const short AnglerFish = 102;

		public const short GreenJellyfish = 103;

		public const short Werewolf = 104;

		public const short BoundGoblin = 105;

		public const short BoundWizard = 106;

		public const short GoblinTinkerer = 107;

		public const short Wizard = 108;

		public const short Clown = 109;

		public const short SkeletonArcher = 110;

		public const short GoblinArcher = 111;

		public const short VileSpit = 112;

		public const short WallofFlesh = 113;

		public const short WallofFleshEye = 114;

		public const short TheHungry = 115;

		public const short TheHungryII = 116;

		public const short LeechHead = 117;

		public const short LeechBody = 118;

		public const short LeechTail = 119;

		public const short ChaosElemental = 120;

		public const short Slimer = 121;

		public const short Gastropod = 122;

		public const short BoundMechanic = 123;

		public const short Mechanic = 124;

		public const short Retinazer = 125;

		public const short Spazmatism = 126;

		public const short SkeletronPrime = 127;

		public const short PrimeCannon = 128;

		public const short PrimeSaw = 129;

		public const short PrimeVice = 130;

		public const short PrimeLaser = 131;

		public const short BaldZombie = 132;

		public const short WanderingEye = 133;

		public const short TheDestroyer = 134;

		public const short TheDestroyerBody = 135;

		public const short TheDestroyerTail = 136;

		public const short IlluminantBat = 137;

		public const short IlluminantSlime = 138;

		public const short Probe = 139;

		public const short PossessedArmor = 140;

		public const short ToxicSludge = 141;

		public const short SantaClaus = 142;

		public const short SnowmanGangsta = 143;

		public const short MisterStabby = 144;

		public const short SnowBalla = 145;

		public const short None3 = 146;

		public const short IceSlime = 147;

		public const short Penguin = 148;

		public const short PenguinBlack = 149;

		public const short IceBat = 150;

		public const short Lavabat = 151;

		public const short GiantFlyingFox = 152;

		public const short GiantTortoise = 153;

		public const short IceTortoise = 154;

		public const short Wolf = 155;

		public const short RedDevil = 156;

		public const short Arapaima = 157;

		public const short VampireBat = 158;

		public const short Vampire = 159;

		public const short Truffle = 160;

		public const short ZombieEskimo = 161;

		public const short Frankenstein = 162;

		public const short BlackRecluse = 163;

		public const short WallCreeper = 164;

		public const short WallCreeperWall = 165;

		public const short SwampThing = 166;

		public const short UndeadViking = 167;

		public const short CorruptPenguin = 168;

		public const short IceElemental = 169;

		public const short PigronCorruption = 170;

		public const short PigronHallow = 171;

		public const short RuneWizard = 172;

		public const short Crimera = 173;

		public const short Herpling = 174;

		public const short AngryTrapper = 175;

		public const short MossHornet = 176;

		public const short Derpling = 177;

		public const short Steampunker = 178;

		public const short CrimsonAxe = 179;

		public const short PigronCrimson = 180;

		public const short FaceMonster = 181;

		public const short FloatyGross = 182;

		public const short Crimslime = 183;

		public const short SpikedIceSlime = 184;

		public const short SnowFlinx = 185;

		public const short PincushionZombie = 186;

		public const short SlimedZombie = 187;

		public const short SwampZombie = 188;

		public const short TwiggyZombie = 189;

		public const short CataractEye = 190;

		public const short SleepyEye = 191;

		public const short DialatedEye = 192;

		public const short GreenEye = 193;

		public const short PurpleEye = 194;

		public const short LostGirl = 195;

		public const short Nymph = 196;

		public const short ArmoredViking = 197;

		public const short Lihzahrd = 198;

		public const short LihzahrdCrawler = 199;

		public const short FemaleZombie = 200;

		public const short HeadacheSkeleton = 201;

		public const short MisassembledSkeleton = 202;

		public const short PantlessSkeleton = 203;

		public const short SpikedJungleSlime = 204;

		public const short Moth = 205;

		public const short IcyMerman = 206;

		public const short DyeTrader = 207;

		public const short PartyGirl = 208;

		public const short Cyborg = 209;

		public const short Bee = 210;

		public const short BeeSmall = 211;

		public const short PirateDeckhand = 212;

		public const short PirateCorsair = 213;

		public const short PirateDeadeye = 214;

		public const short PirateCrossbower = 215;

		public const short PirateCaptain = 216;

		public const short CochinealBeetle = 217;

		public const short CyanBeetle = 218;

		public const short LacBeetle = 219;

		public const short SeaSnail = 220;

		public const short Squid = 221;

		public const short QueenBee = 222;

		public const short ZombieRaincoat = 223;

		public const short FlyingFish = 224;

		public const short UmbrellaSlime = 225;

		public const short FlyingSnake = 226;

		public const short Painter = 227;

		public const short WitchDoctor = 228;

		public const short Pirate = 229;

		public const short GoldfishWalker = 230;

		public const short HornetFatty = 231;

		public const short HornetHoney = 232;

		public const short HornetLeafy = 233;

		public const short HornetSpikey = 234;

		public const short HornetStingy = 235;

		public const short JungleCreeper = 236;

		public const short JungleCreeperWall = 237;

		public const short BlackRecluseWall = 238;

		public const short BloodCrawler = 239;

		public const short BloodCrawlerWall = 240;

		public const short BloodFeeder = 241;

		public const short BloodJelly = 242;

		public const short IceGolem = 243;

		public const short RainbowSlime = 244;

		public const short Golem = 245;

		public const short GolemHead = 246;

		public const short GolemFistLeft = 247;

		public const short GolemFistRight = 248;

		public const short GolemHeadFree = 249;

		public const short AngryNimbus = 250;

		public const short Eyezor = 251;

		public const short Parrot = 252;

		public const short Reaper = 253;

		public const short ZombieMushroom = 254;

		public const short ZombieMushroomHat = 255;

		public const short FungoFish = 256;

		public const short AnomuraFungus = 257;

		public const short MushiLadybug = 258;

		public const short FungiBulb = 259;

		public const short GiantFungiBulb = 260;

		public const short FungiSpore = 261;

		public const short Plantera = 262;

		public const short PlanterasHook = 263;

		public const short PlanterasTentacle = 264;

		public const short Spore = 265;

		public const short BrainofCthulhu = 266;

		public const short Creeper = 267;

		public const short IchorSticker = 268;

		public const short RustyArmoredBonesAxe = 269;

		public const short RustyArmoredBonesFlail = 270;

		public const short RustyArmoredBonesSword = 271;

		public const short RustyArmoredBonesSwordNoArmor = 272;

		public const short BlueArmoredBones = 273;

		public const short BlueArmoredBonesMace = 274;

		public const short BlueArmoredBonesNoPants = 275;

		public const short BlueArmoredBonesSword = 276;

		public const short HellArmoredBones = 277;

		public const short HellArmoredBonesSpikeShield = 278;

		public const short HellArmoredBonesMace = 279;

		public const short HellArmoredBonesSword = 280;

		public const short RaggedCaster = 281;

		public const short RaggedCasterOpenCoat = 282;

		public const short Necromancer = 283;

		public const short NecromancerArmored = 284;

		public const short DiabolistRed = 285;

		public const short DiabolistWhite = 286;

		public const short BoneLee = 287;

		public const short DungeonSpirit = 288;

		public const short GiantCursedSkull = 289;

		public const short Paladin = 290;

		public const short SkeletonSniper = 291;

		public const short TacticalSkeleton = 292;

		public const short SkeletonCommando = 293;

		public const short AngryBonesBig = 294;

		public const short AngryBonesBigMuscle = 295;

		public const short AngryBonesBigHelmet = 296;

		public const short BirdBlue = 297;

		public const short BirdRed = 298;

		public const short Squirrel = 299;

		public const short Mouse = 300;

		public const short Raven = 301;

		public const short SlimeMasked = 302;

		public const short BunnySlimed = 303;

		public const short HoppinJack = 304;

		public const short Scarecrow1 = 305;

		public const short Scarecrow2 = 306;

		public const short Scarecrow3 = 307;

		public const short Scarecrow4 = 308;

		public const short Scarecrow5 = 309;

		public const short Scarecrow6 = 310;

		public const short Scarecrow7 = 311;

		public const short Scarecrow8 = 312;

		public const short Scarecrow9 = 313;

		public const short Scarecrow10 = 314;

		public const short HeadlessHorseman = 315;

		public const short Ghost = 316;

		public const short DemonEyeOwl = 317;

		public const short DemonEyeSpaceship = 318;

		public const short ZombieDoctor = 319;

		public const short ZombieSuperman = 320;

		public const short ZombiePixie = 321;

		public const short SkeletonTopHat = 322;

		public const short SkeletonAstonaut = 323;

		public const short SkeletonAlien = 324;

		public const short MourningWood = 325;

		public const short Splinterling = 326;

		public const short Pumpking = 327;

		public const short PumpkingBlade = 328;

		public const short Hellhound = 329;

		public const short Poltergeist = 330;

		public const short ZombieXmas = 331;

		public const short ZombieSweater = 332;

		public const short SlimeRibbonWhite = 333;

		public const short SlimeRibbonYellow = 334;

		public const short SlimeRibbonGreen = 335;

		public const short SlimeRibbonRed = 336;

		public const short BunnyXmas = 337;

		public const short ZombieElf = 338;

		public const short ZombieElfBeard = 339;

		public const short ZombieElfGirl = 340;

		public const short PresentMimic = 341;

		public const short GingerbreadMan = 342;

		public const short Yeti = 343;

		public const short Everscream = 344;

		public const short IceQueen = 345;

		public const short SantaNK1 = 346;

		public const short ElfCopter = 347;

		public const short Nutcracker = 348;

		public const short NutcrackerSpinning = 349;

		public const short ElfArcher = 350;

		public const short Krampus = 351;

		public const short Flocko = 352;

		public const short Stylist = 353;

		public const short WebbedStylist = 354;

		public const short Firefly = 355;

		public const short Butterfly = 356;

		public const short Worm = 357;

		public const short LightningBug = 358;

		public const short Snail = 359;

		public const short GlowingSnail = 360;

		public const short Frog = 361;

		public const short Duck = 362;

		public const short Duck2 = 363;

		public const short DuckWhite = 364;

		public const short DuckWhite2 = 365;

		public const short ScorpionBlack = 366;

		public const short Scorpion = 367;

		public const short TravellingMerchant = 368;

		public const short Angler = 369;

		public const short DukeFishron = 370;

		public const short DetonatingBubble = 371;

		public const short Sharkron = 372;

		public const short Sharkron2 = 373;

		public const short TruffleWorm = 374;

		public const short TruffleWormDigger = 375;

		public const short SleepingAngler = 376;

		public const short Grasshopper = 377;

		public const short ChatteringTeethBomb = 378;

		public const short CultistArcherBlue = 379;

		public const short CultistArcherWhite = 380;

		public const short BrainScrambler = 381;

		public const short RayGunner = 382;

		public const short MartianOfficer = 383;

		public const short ForceBubble = 384;

		public const short GrayGrunt = 385;

		public const short MartianEngineer = 386;

		public const short MartianTurret = 387;

		public const short MartianDrone = 388;

		public const short GigaZapper = 389;

		public const short ScutlixRider = 390;

		public const short Scutlix = 391;

		public const short MartianSaucer = 392;

		public const short MartianSaucerTurret = 393;

		public const short MartianSaucerCannon = 394;

		public const short MartianSaucerCore = 395;

		public const short MoonLordHead = 396;

		public const short MoonLordHand = 397;

		public const short MoonLordCore = 398;

		public const short MartianProbe = 399;

		public const short MoonLordFreeEye = 400;

		public const short MoonLordLeechBlob = 401;

		public const short StardustWormHead = 402;

		public const short StardustWormBody = 403;

		public const short StardustWormTail = 404;

		public const short StardustCellBig = 405;

		public const short StardustCellSmall = 406;

		public const short StardustJellyfishBig = 407;

		public const short StardustJellyfishSmall = 408;

		public const short StardustSpiderBig = 409;

		public const short StardustSpiderSmall = 410;

		public const short StardustSoldier = 411;

		public const short SolarCrawltipedeHead = 412;

		public const short SolarCrawltipedeBody = 413;

		public const short SolarCrawltipedeTail = 414;

		public const short SolarDrakomire = 415;

		public const short SolarDrakomireRider = 416;

		public const short SolarSroller = 417;

		public const short SolarCorite = 418;

		public const short SolarSolenian = 419;

		public const short NebulaBrain = 420;

		public const short NebulaHeadcrab = 421;

		public const short NebulaBeast = 423;

		public const short NebulaSoldier = 424;

		public const short VortexRifleman = 425;

		public const short VortexHornetQueen = 426;

		public const short VortexHornet = 427;

		public const short VortexLarva = 428;

		public const short VortexSoldier = 429;

		public const short ArmedZombie = 430;

		public const short ArmedZombieEskimo = 431;

		public const short ArmedZombiePincussion = 432;

		public const short ArmedZombieSlimed = 433;

		public const short ArmedZombieSwamp = 434;

		public const short ArmedZombieTwiggy = 435;

		public const short ArmedZombieCenx = 436;

		public const short CultistTablet = 437;

		public const short CultistDevote = 438;

		public const short CultistBoss = 439;

		public const short CultistBossClone = 440;

		public const short GoldBird = 442;

		public const short GoldBunny = 443;

		public const short GoldButterfly = 444;

		public const short GoldFrog = 445;

		public const short GoldGrasshopper = 446;

		public const short GoldMouse = 447;

		public const short GoldWorm = 448;

		public const short BoneThrowingSkeleton = 449;

		public const short BoneThrowingSkeleton2 = 450;

		public const short BoneThrowingSkeleton3 = 451;

		public const short BoneThrowingSkeleton4 = 452;

		public const short SkeletonMerchant = 453;

		public const short CultistDragonHead = 454;

		public const short CultistDragonBody1 = 455;

		public const short CultistDragonBody2 = 456;

		public const short CultistDragonBody3 = 457;

		public const short CultistDragonBody4 = 458;

		public const short CultistDragonTail = 459;

		public const short Butcher = 460;

		public const short CreatureFromTheDeep = 461;

		public const short Fritz = 462;

		public const short Nailhead = 463;

		public const short CrimsonBunny = 464;

		public const short CrimsonGoldfish = 465;

		public const short Psycho = 466;

		public const short DeadlySphere = 467;

		public const short DrManFly = 468;

		public const short ThePossessed = 469;

		public const short CrimsonPenguin = 470;

		public const short GoblinSummoner = 471;

		public const short ShadowFlameApparition = 472;

		public const short BigMimicCorruption = 473;

		public const short BigMimicCrimson = 474;

		public const short BigMimicHallow = 475;

		public const short BigMimicJungle = 476;

		public const short Mothron = 477;

		public const short MothronEgg = 478;

		public const short MothronSpawn = 479;

		public const short Medusa = 480;

		public const short GreekSkeleton = 481;

		public const short GraniteGolem = 482;

		public const short GraniteFlyer = 483;

		public const short EnchantedNightcrawler = 484;

		public const short Grubby = 485;

		public const short Sluggy = 486;

		public const short Buggy = 487;

		public const short TargetDummy = 488;

		public const short BloodZombie = 489;

		public const short Drippler = 490;

		public const short PirateShip = 491;

		public const short PirateShipCannon = 492;

		public const short LunarTowerStardust = 493;

		public const short Crawdad = 494;

		public const short Crawdad2 = 495;

		public const short GiantShelly = 496;

		public const short GiantShelly2 = 497;

		public const short Salamander = 498;

		public const short Salamander2 = 499;

		public const short Salamander3 = 500;

		public const short Salamander4 = 501;

		public const short Salamander5 = 502;

		public const short Salamander6 = 503;

		public const short Salamander7 = 504;

		public const short Salamander8 = 505;

		public const short Salamander9 = 506;

		public const short LunarTowerNebula = 507;

		public const short LunarTowerVortex = 422;

		public const short TaxCollector = 441;

		public const short GiantWalkingAntlion = 508;

		public const short GiantFlyingAntlion = 509;

		public const short DuneSplicerHead = 510;

		public const short DuneSplicerBody = 511;

		public const short DuneSplicerTail = 512;

		public const short TombCrawlerHead = 513;

		public const short TombCrawlerBody = 514;

		public const short TombCrawlerTail = 515;

		public const short SolarFlare = 516;

		public const short LunarTowerSolar = 517;

		public const short SolarSpearman = 518;

		public const short SolarGoop = 519;

		public const short MartianWalker = 520;

		public const short AncientCultistSquidhead = 521;

		public const short AncientLight = 522;

		public const short AncientDoom = 523;

		public const short DesertGhoul = 524;

		public const short DesertGhoulCorruption = 525;

		public const short DesertGhoulCrimson = 526;

		public const short DesertGhoulHallow = 527;

		public const short DesertLamiaLight = 528;

		public const short DesertLamiaDark = 529;

		public const short DesertScorpionWalk = 530;

		public const short DesertScorpionWall = 531;

		public const short DesertBeast = 532;

		public const short DesertDjinn = 533;

		public const short DemonTaxCollector = 534;

		public const short SlimeSpiked = 535;

		public const short TheBride = 536;

		public const short SandSlime = 537;

		public const short SquirrelRed = 538;

		public const short SquirrelGold = 539;

		public const short PartyBunny = 540;

		public const short SandElemental = 541;

		public const short SandShark = 542;

		public const short SandsharkCorrupt = 543;

		public const short SandsharkCrimson = 544;

		public const short SandsharkHallow = 545;

		public const short Tumbleweed = 546;

		public const short DD2AttackerTest = 547;

		public const short DD2EterniaCrystal = 548;

		public const short DD2LanePortal = 549;

		public const short DD2Bartender = 550;

		public const short DD2Betsy = 551;

		public const short DD2GoblinT1 = 552;

		public const short DD2GoblinT2 = 553;

		public const short DD2GoblinT3 = 554;

		public const short DD2GoblinBomberT1 = 555;

		public const short DD2GoblinBomberT2 = 556;

		public const short DD2GoblinBomberT3 = 557;

		public const short DD2WyvernT1 = 558;

		public const short DD2WyvernT2 = 559;

		public const short DD2WyvernT3 = 560;

		public const short DD2JavelinstT1 = 561;

		public const short DD2JavelinstT2 = 562;

		public const short DD2JavelinstT3 = 563;

		public const short DD2DarkMageT1 = 564;

		public const short DD2DarkMageT3 = 565;

		public const short DD2SkeletonT1 = 566;

		public const short DD2SkeletonT3 = 567;

		public const short DD2WitherBeastT2 = 568;

		public const short DD2WitherBeastT3 = 569;

		public const short DD2DrakinT2 = 570;

		public const short DD2DrakinT3 = 571;

		public const short DD2KoboldWalkerT2 = 572;

		public const short DD2KoboldWalkerT3 = 573;

		public const short DD2KoboldFlyerT2 = 574;

		public const short DD2KoboldFlyerT3 = 575;

		public const short DD2OgreT2 = 576;

		public const short DD2OgreT3 = 577;

		public const short DD2LightningBugT3 = 578;

		public const short BartenderUnconscious = 579;

		public const short WalkingAntlion = 580;

		public const short FlyingAntlion = 581;

		public const short LarvaeAntlion = 582;

		public const short FairyCritterPink = 583;

		public const short FairyCritterGreen = 584;

		public const short FairyCritterBlue = 585;

		public const short ZombieMerman = 586;

		public const short EyeballFlyingFish = 587;

		public const short Golfer = 588;

		public const short GolferRescue = 589;

		public const short TorchZombie = 590;

		public const short ArmedTorchZombie = 591;

		public const short GoldGoldfish = 592;

		public const short GoldGoldfishWalker = 593;

		public const short WindyBalloon = 594;

		public const short BlackDragonfly = 595;

		public const short BlueDragonfly = 596;

		public const short GreenDragonfly = 597;

		public const short OrangeDragonfly = 598;

		public const short RedDragonfly = 599;

		public const short YellowDragonfly = 600;

		public const short GoldDragonfly = 601;

		public const short Seagull = 602;

		public const short Seagull2 = 603;

		public const short LadyBug = 604;

		public const short GoldLadyBug = 605;

		public const short Maggot = 606;

		public const short Pupfish = 607;

		public const short Grebe = 608;

		public const short Grebe2 = 609;

		public const short Rat = 610;

		public const short Owl = 611;

		public const short WaterStrider = 612;

		public const short GoldWaterStrider = 613;

		public const short ExplosiveBunny = 614;

		public const short Dolphin = 615;

		public const short Turtle = 616;

		public const short TurtleJungle = 617;

		public const short BloodNautilus = 618;

		public const short BloodSquid = 619;

		public const short GoblinShark = 620;

		public const short BloodEelHead = 621;

		public const short BloodEelBody = 622;

		public const short BloodEelTail = 623;

		public const short Gnome = 624;

		public const short SeaTurtle = 625;

		public const short Seahorse = 626;

		public const short GoldSeahorse = 627;

		public const short Dandelion = 628;

		public const short IceMimic = 629;

		public const short BloodMummy = 630;

		public const short RockGolem = 631;

		public const short MaggotZombie = 632;

		public const short BestiaryGirl = 633;

		public const short SporeBat = 634;

		public const short SporeSkeleton = 635;

		public const short HallowBoss = 636;

		public const short TownCat = 637;

		public const short TownDog = 638;

		public const short GemSquirrelAmethyst = 639;

		public const short GemSquirrelTopaz = 640;

		public const short GemSquirrelSapphire = 641;

		public const short GemSquirrelEmerald = 642;

		public const short GemSquirrelRuby = 643;

		public const short GemSquirrelDiamond = 644;

		public const short GemSquirrelAmber = 645;

		public const short GemBunnyAmethyst = 646;

		public const short GemBunnyTopaz = 647;

		public const short GemBunnySapphire = 648;

		public const short GemBunnyEmerald = 649;

		public const short GemBunnyRuby = 650;

		public const short GemBunnyDiamond = 651;

		public const short GemBunnyAmber = 652;

		public const short HellButterfly = 653;

		public const short Lavafly = 654;

		public const short MagmaSnail = 655;

		public const short TownBunny = 656;

		public const short QueenSlimeBoss = 657;

		public const short QueenSlimeMinionBlue = 658;

		public const short QueenSlimeMinionPink = 659;

		public const short QueenSlimeMinionPurple = 660;

		public const short EmpressButterfly = 661;

		public const short PirateGhost = 662;

		public const short Princess = 663;

		public const short TorchGod = 664;

		public const short ChaosBallTim = 665;

		public const short VileSpitEaterOfWorlds = 666;

		public const short GoldenSlime = 667;

		public const short Deerclops = 668;

		public const short Stinkbug = 669;

		public const short TownSlimeBlue = 670;

		public const short ScarletMacaw = 671;

		public const short BlueMacaw = 672;

		public const short Toucan = 673;

		public const short YellowCockatiel = 674;

		public const short GrayCockatiel = 675;

		public const short ShimmerSlime = 676;

		public const short Shimmerfly = 677;

		public const short TownSlimeGreen = 678;

		public const short TownSlimeOld = 679;

		public const short TownSlimePurple = 680;

		public const short TownSlimeRainbow = 681;

		public const short TownSlimeRed = 682;

		public const short TownSlimeYellow = 683;

		public const short TownSlimeCopper = 684;

		public const short BoundTownSlimeOld = 685;

		public const short BoundTownSlimePurple = 686;

		public const short BoundTownSlimeYellow = 687;

		public const short Count = 688;

		public static int FromLegacyName(string name)
		{
			if (LegacyNameToIdMap.TryGetValue(name, out var value))
			{
				return value;
			}
			return 0;
		}

		public static int FromNetId(int id)
		{
			if (id < 0)
			{
				return NetIdMap[-id - 1];
			}
			return id;
		}
	}
}
