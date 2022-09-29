using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.Bestiary;
using Terraria.Graphics.Shaders;

namespace Terraria.ID
{
	public static class ContentSamples
	{
		public static class CommonlyUsedContentSamples
		{
			public static int TeamDyeShaderIndex = -1;

			public static int ColorOnlyShaderIndex = -1;

			public static void PrepareAfterEverythingElseLoaded()
			{
				TeamDyeShaderIndex = GameShaders.Hair.GetShaderIdFromItemId(1984);
				ColorOnlyShaderIndex = GameShaders.Armor.GetShaderIdFromItemId(3978);
			}
		}

		public static class CreativeHelper
		{
			public enum ItemGroup
			{
				Coin = 10,
				CraftingObjects = 11,
				Torches = 20,
				Glowsticks = 25,
				Wood = 30,
				Bombs = 40,
				LifePotions = 50,
				ManaPotions = 51,
				BuffPotion = 52,
				Flask = 53,
				Food = 54,
				Crates = 60,
				BossBags = 70,
				GoodieBags = 80,
				AlchemyPlants = 83,
				AlchemySeeds = 84,
				DyeMaterial = 87,
				BossItem = 90,
				EventItem = 91,
				ConsumableThatDoesNotDamage = 94,
				Solutions = 95,
				Ammo = 96,
				ConsumableThatDamages = 97,
				PlacableObjects = 100,
				Blocks = 120,
				Wands = 130,
				Rope = 140,
				Walls = 150,
				Wiring = 200,
				Pickaxe = 500,
				Axe = 510,
				Hammer = 520,
				MeleeWeapon = 530,
				RangedWeapon = 540,
				MagicWeapon = 550,
				SummonWeapon = 560,
				Headgear = 600,
				Torso = 610,
				Pants = 620,
				Accessories = 630,
				Hook = 700,
				Mount = 710,
				Minecart = 720,
				VanityPet = 800,
				LightPet = 810,
				Golf = 900,
				BossSpawners = 901,
				Dye = 910,
				HairDye = 920,
				Paint = 930,
				FishingRods = 1000,
				FishingQuestFish = 1010,
				Fish = 1015,
				FishingBait = 1020,
				Critters = 1030,
				Keys = 2000,
				RemainingUseItems = 5000,
				Material = 10000,
				EverythingElse = 11000
			}

			public struct ItemGroupAndOrderInGroup
			{
				public int ItemType;

				public ItemGroup Group;

				public int OrderInGroup;

				public ItemGroupAndOrderInGroup(Item item)
				{
					ItemType = item.type;
					Group = GetItemGroup(item, out OrderInGroup);
				}
			}

			private static List<int> _manualEventItemsOrder = new List<int> { 361, 1315, 2767, 602, 1844, 1958 };

			private static List<int> _manualBossSpawnItemsOrder = new List<int>
			{
				43, 560, 70, 1331, 1133, 5120, 1307, 267, 3828, 4988,
				5334, 544, 557, 556, 1293, 2673, 4961, 3601
			};

			public static List<int> _manualCraftingStations = new List<int>
			{
				33, 35, 716, 221, 524, 1221, 525, 1220, 3549, 398,
				1120, 1430, 1551, 345, 1791, 5008, 332, 352, 487, 995,
				363, 2172, 2196, 2194, 2198, 2204, 998, 2197, 996, 4142,
				2193, 2192, 2203, 2195
			};

			private static List<int> _manualGolfItemsOrder = new List<int>
			{
				4095, 4596, 4597, 4595, 4598, 4592, 4593, 4591, 4594, 4092,
				4093, 4039, 4094, 4588, 4589, 4587, 4590, 3989, 4242, 4243,
				4244, 4245, 4246, 4247, 4248, 4249, 4250, 4251, 4252, 4253,
				4254, 4255, 4040, 4086, 4085, 4088, 4084, 4083, 4087
			};

			public static ItemGroup GetItemGroup(Item item, out int orderInGroup)
			{
				orderInGroup = 0;
				int num = _manualBossSpawnItemsOrder.IndexOf(item.type);
				if (num != -1)
				{
					orderInGroup = num;
					return ItemGroup.BossItem;
				}
				int num2 = _manualGolfItemsOrder.IndexOf(item.type);
				if (num2 != -1)
				{
					orderInGroup = num2;
					return ItemGroup.Golf;
				}
				int num3 = ItemID.Sets.SortingPriorityWiring[item.type];
				if (num3 != -1)
				{
					orderInGroup = -num3;
					return ItemGroup.Wiring;
				}
				if (item.type == 3620)
				{
					return ItemGroup.Wiring;
				}
				if (item.type == 327 || item.type == 329 || item.type == 1141 || item.type == 1533 || item.type == 1537 || item.type == 1536 || item.type == 1534 || item.type == 1535 || item.type == 3092 || item.type == 3091 || item.type == 4714)
				{
					orderInGroup = -item.rare;
					return ItemGroup.Keys;
				}
				if (item.type == 985 || item.type == 3079 || item.type == 3005 || item.type == 3080)
				{
					return ItemGroup.Rope;
				}
				if (item.type == 781 || item.type == 783 || item.type == 780 || item.type == 782 || item.type == 784)
				{
					return ItemGroup.Solutions;
				}
				if (item.type == 282 || item.type == 3112 || item.type == 4776 || item.type == 3002 || item.type == 286)
				{
					if (item.type == 282)
					{
						orderInGroup = -1;
					}
					return ItemGroup.Glowsticks;
				}
				if (item.type == 166 || item.type == 3115 || item.type == 235 || item.type == 167 || item.type == 3547 || item.type == 2896 || item.type == 3196 || item.type == 4908 || item.type == 4909 || item.type == 4827 || item.type == 4826 || item.type == 4825 || item.type == 4423 || item.type == 4824)
				{
					return ItemGroup.Bombs;
				}
				if (item.createTile == 376)
				{
					return ItemGroup.Crates;
				}
				if (item.type == 1774 || item.type == 1869 || item.type == 4345 || item.type == 3093 || item.type == 4410)
				{
					return ItemGroup.GoodieBags;
				}
				if (ItemID.Sets.BossBag[item.type])
				{
					return ItemGroup.BossBags;
				}
				if (item.type == 1115 || item.type == 1114 || item.type == 1110 || item.type == 1112 || item.type == 1108 || item.type == 1107 || item.type == 1116 || item.type == 1109 || item.type == 1111 || item.type == 1118 || item.type == 1117 || item.type == 1113 || item.type == 1119)
				{
					return ItemGroup.DyeMaterial;
				}
				if (item.type == 3385 || item.type == 3386 || item.type == 3387 || item.type == 3388)
				{
					orderInGroup = -1;
					return ItemGroup.DyeMaterial;
				}
				if (item.dye != 0)
				{
					return ItemGroup.Dye;
				}
				if (item.hairDye != -1)
				{
					return ItemGroup.HairDye;
				}
				if (item.IsACoin)
				{
					if (item.type == 71)
					{
						orderInGroup = 4;
					}
					else if (item.type == 72)
					{
						orderInGroup = 3;
					}
					else if (item.type == 73)
					{
						orderInGroup = 2;
					}
					else if (item.type == 74)
					{
						orderInGroup = 1;
					}
					return ItemGroup.Coin;
				}
				if (item.createWall > 0)
				{
					return ItemGroup.Walls;
				}
				if (item.createTile == 82)
				{
					return ItemGroup.AlchemySeeds;
				}
				if (item.type == 315 || item.type == 313 || item.type == 316 || item.type == 318 || item.type == 314 || item.type == 2358 || item.type == 317)
				{
					return ItemGroup.AlchemyPlants;
				}
				if (item.createTile == 30 || item.createTile == 321 || item.createTile == 322 || item.createTile == 157 || item.createTile == 158 || item.createTile == 208 || item.createTile == 159 || item.createTile == 253 || item.createTile == 311 || item.createTile == 635)
				{
					if (item.createTile == 30)
					{
						orderInGroup = 0;
					}
					else if (item.createTile == 311)
					{
						orderInGroup = 100;
					}
					else
					{
						orderInGroup = 50;
					}
					return ItemGroup.Wood;
				}
				if (item.createTile >= 0)
				{
					if (item.type == 213)
					{
						orderInGroup = -1;
						return ItemGroup.Pickaxe;
					}
					if (item.tileWand >= 0)
					{
						return ItemGroup.Wands;
					}
					if (item.createTile == 213 || item.createTile == 353 || item.createTile == 365 || item.createTile == 366 || item.createTile == 214)
					{
						return ItemGroup.Rope;
					}
					if (!Main.tileSolid[item.createTile] || Main.tileSolidTop[item.createTile] || item.createTile == 10)
					{
						int num4 = _manualCraftingStations.IndexOf(item.type);
						if (num4 != -1)
						{
							orderInGroup = num4;
							return ItemGroup.CraftingObjects;
						}
						if (item.createTile == 4)
						{
							if (item.placeStyle == 0)
							{
								orderInGroup = 5;
							}
							else
							{
								orderInGroup = 10;
							}
							return ItemGroup.Torches;
						}
						if (item.createTile == 178)
						{
							orderInGroup = 5;
						}
						else if (item.createTile == 239)
						{
							orderInGroup = 7;
						}
						else if (item.type == 27 || item.type == 4857 || item.type == 4852 || item.type == 4856 || item.type == 4854 || item.type == 4855 || item.type == 4853 || item.type == 4851)
						{
							orderInGroup = 8;
						}
						else if (TileID.Sets.Platforms[item.createTile])
						{
							if (item.placeStyle == 0)
							{
								orderInGroup = 19;
							}
							else
							{
								orderInGroup = 20;
							}
						}
						else if (item.createTile == 18)
						{
							if (item.placeStyle == 0)
							{
								orderInGroup = 29;
							}
							else
							{
								orderInGroup = 30;
							}
						}
						else if (item.createTile == 16 || item.createTile == 134)
						{
							if (item.placeStyle == 0)
							{
								orderInGroup = 39;
							}
							else
							{
								orderInGroup = 40;
							}
						}
						else if (item.createTile == 133 || item.createTile == 17)
						{
							if (item.placeStyle == 0)
							{
								orderInGroup = 49;
							}
							else
							{
								orderInGroup = 50;
							}
						}
						else if (item.createTile == 10)
						{
							if (item.placeStyle == 0)
							{
								orderInGroup = 59;
							}
							else
							{
								orderInGroup = 60;
							}
						}
						else if (item.createTile == 15)
						{
							if (item.placeStyle == 0)
							{
								orderInGroup = 69;
							}
							else
							{
								orderInGroup = 70;
							}
						}
						else if (item.createTile == 497)
						{
							orderInGroup = 72;
						}
						else if (item.createTile == 79)
						{
							if (item.placeStyle == 0)
							{
								orderInGroup = 74;
							}
							else
							{
								orderInGroup = 75;
							}
						}
						else if (item.createTile == 14)
						{
							if (item.placeStyle == 0)
							{
								orderInGroup = 79;
							}
							else
							{
								orderInGroup = 80;
							}
						}
						else if (item.createTile == 469)
						{
							orderInGroup = 90;
						}
						else if (item.createTile == 21)
						{
							if (item.placeStyle == 0)
							{
								orderInGroup = 99;
							}
							else
							{
								orderInGroup = 100;
							}
						}
						else if (item.createTile == 467)
						{
							orderInGroup = 110;
						}
						else if (item.createTile == 441)
						{
							orderInGroup = 120;
						}
						else if (item.createTile == 468)
						{
							orderInGroup = 130;
						}
						else
						{
							orderInGroup = item.createTile + 1000;
						}
						return ItemGroup.PlacableObjects;
					}
					if (TileID.Sets.Conversion.Grass[item.createTile] || item.type == 194)
					{
						orderInGroup = 5;
					}
					else
					{
						orderInGroup = 10000;
					}
					if (item.type == 2)
					{
						orderInGroup = 10;
					}
					else if (item.type == 3)
					{
						orderInGroup = 20;
					}
					else if (item.type == 133)
					{
						orderInGroup = 30;
					}
					else if (item.type == 424)
					{
						orderInGroup = 40;
					}
					else if (item.type == 1103)
					{
						orderInGroup = 50;
					}
					else if (item.type == 169)
					{
						orderInGroup = 60;
					}
					else if (item.type == 170)
					{
						orderInGroup = 70;
					}
					else if (item.type == 176)
					{
						orderInGroup = 80;
					}
					else if (item.type == 276)
					{
						orderInGroup = 80;
					}
					return ItemGroup.Blocks;
				}
				if (item.mountType != -1)
				{
					if (MountID.Sets.Cart[item.mountType])
					{
						return ItemGroup.Minecart;
					}
					return ItemGroup.Mount;
				}
				if (item.bait > 0)
				{
					orderInGroup = -item.bait;
					return ItemGroup.FishingBait;
				}
				if (item.makeNPC > 0)
				{
					return ItemGroup.Critters;
				}
				if (item.fishingPole > 1)
				{
					orderInGroup = -item.fishingPole;
					return ItemGroup.FishingRods;
				}
				if (item.questItem)
				{
					return ItemGroup.FishingQuestFish;
				}
				if ((item.type >= 2297 && item.type <= 2321) || item.type == 4402 || item.type == 4401 || item.type == 2290)
				{
					orderInGroup = -item.rare;
					return ItemGroup.FishingQuestFish;
				}
				int num5 = ItemID.Sets.SortingPriorityPainting[item.type];
				if (num5 != -1 || item.PaintOrCoating)
				{
					orderInGroup = -num5;
					return ItemGroup.Paint;
				}
				int num6 = _manualEventItemsOrder.IndexOf(item.type);
				if (num6 != -1)
				{
					orderInGroup = num6;
					return ItemGroup.EventItem;
				}
				if (item.shoot != 0 && Main.projHook[item.shoot])
				{
					return ItemGroup.Hook;
				}
				if (item.type == 2756 || item.type == 2351 || item.type == 4870 || item.type == 2350 || item.type == 2997 || item.type == 2352 || item.type == 2353)
				{
					return ItemGroup.BuffPotion;
				}
				if (item.buffType != 0)
				{
					if (BuffID.Sets.IsWellFed[item.buffType])
					{
						orderInGroup = -item.buffType * 10000000 - item.buffTime;
						return ItemGroup.Food;
					}
					if (BuffID.Sets.IsAFlaskBuff[item.buffType])
					{
						return ItemGroup.Flask;
					}
					if (Main.vanityPet[item.buffType])
					{
						return ItemGroup.VanityPet;
					}
					if (Main.lightPet[item.buffType])
					{
						return ItemGroup.VanityPet;
					}
					if (item.damage == -1)
					{
						return ItemGroup.BuffPotion;
					}
				}
				if (item.headSlot >= 0)
				{
					orderInGroup = -item.defense;
					orderInGroup -= item.rare * 1000;
					if (item.vanity)
					{
						orderInGroup += 100000;
					}
					return ItemGroup.Headgear;
				}
				if (item.bodySlot >= 0)
				{
					orderInGroup = -item.defense;
					orderInGroup -= item.rare * 1000;
					if (item.vanity)
					{
						orderInGroup += 100000;
					}
					return ItemGroup.Torso;
				}
				if (item.legSlot >= 0)
				{
					orderInGroup = -item.defense;
					orderInGroup -= item.rare * 1000;
					if (item.vanity)
					{
						orderInGroup += 100000;
					}
					return ItemGroup.Pants;
				}
				if (item.accessory)
				{
					orderInGroup = item.vanity.ToInt() - item.expert.ToInt();
					if (item.type >= 3293 && item.type <= 3308)
					{
						orderInGroup -= 200000;
					}
					else if (item.type >= 3309 && item.type <= 3314)
					{
						orderInGroup -= 100000;
					}
					orderInGroup -= item.rare * 10000;
					if (item.vanity)
					{
						orderInGroup += 100000;
					}
					return ItemGroup.Accessories;
				}
				if (item.pick > 0)
				{
					orderInGroup = -item.pick;
					return ItemGroup.Pickaxe;
				}
				if (item.axe > 0)
				{
					orderInGroup = -item.axe;
					return ItemGroup.Axe;
				}
				if (item.hammer > 0)
				{
					orderInGroup = -item.hammer;
					return ItemGroup.Hammer;
				}
				if (item.healLife > 0)
				{
					if (item.type == 3544)
					{
						orderInGroup = 0;
					}
					else if (item.type == 499)
					{
						orderInGroup = 1;
					}
					else if (item.type == 188)
					{
						orderInGroup = 2;
					}
					else if (item.type == 28)
					{
						orderInGroup = 3;
					}
					else
					{
						orderInGroup = -item.healLife + 1000;
					}
					return ItemGroup.LifePotions;
				}
				if (item.healMana > 0)
				{
					orderInGroup = -item.healMana;
					return ItemGroup.ManaPotions;
				}
				if (item.ammo != AmmoID.None && !item.notAmmo && item.type != 23 && item.type != 75)
				{
					orderInGroup = -item.ammo * 10000;
					orderInGroup += -item.damage;
					return ItemGroup.Ammo;
				}
				if (item.consumable)
				{
					if (item.damage > 0)
					{
						if (item.type == 422 || item.type == 423 || item.type == 3477)
						{
							orderInGroup = -100000;
						}
						else
						{
							orderInGroup = -item.damage;
						}
						return ItemGroup.ConsumableThatDamages;
					}
					if (item.type == 4910 || item.type == 4829 || item.type == 4830)
					{
						orderInGroup = 10;
					}
					else if (item.type == 66 || item.type == 2886 || item.type == 67)
					{
						orderInGroup = -10;
					}
					else if (item.type >= 1874 && item.type <= 1905)
					{
						orderInGroup = 5;
					}
					return ItemGroup.ConsumableThatDoesNotDamage;
				}
				if (item.damage > 0)
				{
					orderInGroup = -item.damage;
					if (item.melee)
					{
						return ItemGroup.MeleeWeapon;
					}
					if (item.ranged)
					{
						return ItemGroup.RangedWeapon;
					}
					if (item.magic)
					{
						return ItemGroup.MagicWeapon;
					}
					if (item.summon)
					{
						return ItemGroup.SummonWeapon;
					}
				}
				orderInGroup = -item.rare;
				if (item.useStyle > 0)
				{
					return ItemGroup.RemainingUseItems;
				}
				if (item.material)
				{
					return ItemGroup.Material;
				}
				return ItemGroup.EverythingElse;
			}

			public static void SetCreativeMenuOrder()
			{
				List<Item> list = new List<Item>();
				for (int i = 1; i < 5453; i++)
				{
					Item item = new Item();
					item.SetDefaults(i);
					list.Add(item);
				}
				IOrderedEnumerable<IGrouping<ItemGroup, ItemGroupAndOrderInGroup>> orderedEnumerable = from x in list
					select new ItemGroupAndOrderInGroup(x) into x
					group x by x.Group into @group
					orderby (int)@group.Key
					select @group;
				foreach (IGrouping<ItemGroup, ItemGroupAndOrderInGroup> item2 in orderedEnumerable)
				{
					foreach (ItemGroupAndOrderInGroup item3 in item2)
					{
						ItemCreativeSortingId[item3.ItemType] = item3;
					}
				}
				orderedEnumerable.SelectMany((IGrouping<ItemGroup, ItemGroupAndOrderInGroup> group) => group.ToList()).ToList();
			}

			public static bool ShouldRemoveFromList(Item item)
			{
				return ItemID.Sets.Deprecated[item.type];
			}
		}

		public static class BestiaryHelper
		{
			public static List<KeyValuePair<int, NPC>> GetSortedBestiaryEntriesList(BestiaryDatabase database)
			{
				List<IBestiaryInfoElement> commonFilters = BestiaryDatabaseNPCsPopulator.CommonTags.GetCommonInfoElementsForFilters();
				List<KeyValuePair<int, NPC>> list = (from x in NpcsByNetId.ToList()
					orderby GetBestiaryTownPriority(x.Value), !x.Value.isLikeATownNPC, GetBestiaryNormalGoldCritterPriority(x.Value), !x.Value.CountsAsACritter, GetBestiaryBossPriority(x.Value), x.Value.boss || NPCID.Sets.ShouldBeCountedAsBoss[x.Value.type], GetLowestBiomeGroupIndex(x.Value, database, commonFilters), x.Value.aiStyle, GetBestiaryPowerLevel(x.Value), GetBestiaryStarsPriority(x.Value)
					select x).ToList();
				list.RemoveAll((KeyValuePair<int, NPC> x) => ShouldHideBestiaryEntry(x.Value));
				return list;
			}

			public static int GetLowestBiomeGroupIndex(NPC npc, BestiaryDatabase database, List<IBestiaryInfoElement> commonElements)
			{
				List<IBestiaryInfoElement> info = database.FindEntryByNPCID(npc.netID).Info;
				for (int num = commonElements.Count - 1; num >= 0; num--)
				{
					if (info.IndexOf(commonElements[num]) != -1)
					{
						return num;
					}
				}
				return int.MaxValue;
			}

			public static bool ShouldHideBestiaryEntry(NPC npc)
			{
				if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(npc.netID, out var value))
				{
					return value.Hide;
				}
				return false;
			}

			public static float GetBestiaryPowerLevel(NPC npc)
			{
				return npc.damage + npc.defense + npc.lifeMax / 4;
			}

			public static int GetBestiaryTownPriority(NPC npc)
			{
				int num = NPCID.Sets.TownNPCBestiaryPriority.IndexOf(npc.netID);
				if (num == -1)
				{
					num = int.MaxValue;
				}
				return num;
			}

			public static int GetBestiaryNormalGoldCritterPriority(NPC npc)
			{
				int num = NPCID.Sets.NormalGoldCritterBestiaryPriority.IndexOf(npc.netID);
				if (num == -1)
				{
					num = int.MaxValue;
				}
				return num;
			}

			public static int GetBestiaryBossPriority(NPC npc)
			{
				return NPCID.Sets.BossBestiaryPriority.IndexOf(npc.netID);
			}

			public static int GetBestiaryStarsPriority(NPC npc)
			{
				return NpcBestiaryRarityStars[npc.type];
			}
		}

		public static Dictionary<int, NPC> NpcsByNetId = new Dictionary<int, NPC>();

		public static Dictionary<int, Projectile> ProjectilesByType = new Dictionary<int, Projectile>();

		public static Dictionary<int, Item> ItemsByType = new Dictionary<int, Item>();

		public static Dictionary<string, int> ItemNetIdsByPersistentIds = new Dictionary<string, int>();

		public static Dictionary<int, string> ItemPersistentIdsByNetIds = new Dictionary<int, string>();

		public static Dictionary<int, int> CreativeResearchItemPersistentIdOverride = new Dictionary<int, int>();

		public static Dictionary<string, int> NpcNetIdsByPersistentIds = new Dictionary<string, int>();

		public static Dictionary<int, string> NpcPersistentIdsByNetIds = new Dictionary<int, string>();

		public static Dictionary<int, int> NpcBestiarySortingId = new Dictionary<int, int>();

		public static Dictionary<int, int> NpcBestiaryRarityStars = new Dictionary<int, int>();

		public static Dictionary<int, string> NpcBestiaryCreditIdsByNpcNetIds = new Dictionary<int, string>();

		public static Dictionary<int, CreativeHelper.ItemGroupAndOrderInGroup> ItemCreativeSortingId = new Dictionary<int, CreativeHelper.ItemGroupAndOrderInGroup>();

		public static void Initialize()
		{
			NpcsByNetId.Clear();
			NpcNetIdsByPersistentIds.Clear();
			NpcPersistentIdsByNetIds.Clear();
			NpcBestiarySortingId.Clear();
			for (int i = -65; i < 688; i++)
			{
				NPC nPC = new NPC();
				nPC.SetDefaults(i);
				NpcsByNetId[i] = nPC;
				string name = NPCID.Search.GetName(nPC.netID);
				NpcPersistentIdsByNetIds[i] = name;
				NpcBestiaryCreditIdsByNpcNetIds[i] = name;
				NpcNetIdsByPersistentIds[name] = i;
			}
			ModifyNPCIds();
			ProjectilesByType.Clear();
			for (int j = 0; j < 1022; j++)
			{
				Projectile projectile = new Projectile();
				projectile.SetDefaults(j);
				ProjectilesByType[j] = projectile;
			}
			ItemsByType.Clear();
			for (int k = 0; k < 5453; k++)
			{
				Item item = new Item();
				item.SetDefaults(k);
				ItemsByType[k] = item;
				string name2 = ItemID.Search.GetName(item.netID);
				ItemPersistentIdsByNetIds[k] = name2;
				ItemNetIdsByPersistentIds[name2] = k;
			}
			foreach (int item3 in ItemID.Sets.ItemsThatAreProcessedAfterNormalContentSample)
			{
				Item item2 = new Item();
				item2.SetDefaults(item3);
				ItemsByType[item3] = item2;
				string name3 = ItemID.Search.GetName(item2.netID);
				ItemPersistentIdsByNetIds[item3] = name3;
				ItemNetIdsByPersistentIds[name3] = item3;
			}
			FillResearchItemOverrides();
			FillNpcRarities();
		}

		private static void FillResearchItemOverrides()
		{
			AddItemResearchOverride(4131, 5325);
			AddItemResearchOverride(5324, 5329, 5330);
			AddItemResearchOverride(5437, 5358, 5359, 5360, 5361);
			AddItemResearchOverride(4346, 5391);
		}

		private static void AddItemResearchOverride(int itemTypeToUnlock, params int[] itemsThatWillResearchTheItemToUnlock)
		{
			for (int i = 0; i < itemsThatWillResearchTheItemToUnlock.Length; i++)
			{
				AddItemResearchOverride_Inner(itemsThatWillResearchTheItemToUnlock[i], itemTypeToUnlock);
			}
		}

		private static void AddItemResearchOverride_Inner(int itemTypeToSacrifice, int itemTypeToUnlock)
		{
			CreativeResearchItemPersistentIdOverride[itemTypeToSacrifice] = itemTypeToUnlock;
		}

		public static void FixItemsAfterRecipesAreAdded()
		{
			foreach (KeyValuePair<int, Item> item in ItemsByType)
			{
				item.Value.material = ItemID.Sets.IsAMaterial[item.Key];
			}
		}

		public static void RebuildBestiarySortingIDsByBestiaryDatabaseContents(BestiaryDatabase database)
		{
			NpcBestiarySortingId.Clear();
			CreateBestiarySortingIds(database);
		}

		public static void RebuildItemCreativeSortingIDsAfterRecipesAreSetUp()
		{
			ItemCreativeSortingId.Clear();
			CreateCreativeItemSortingIds();
		}

		private static void ModifyNPCIds()
		{
			Dictionary<int, string> npcBestiaryCreditIdsByNpcNetIds = NpcBestiaryCreditIdsByNpcNetIds;
			npcBestiaryCreditIdsByNpcNetIds[-65] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-64] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-63] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-62] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-61] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-60] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-59] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-58] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-57] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-56] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-55] = npcBestiaryCreditIdsByNpcNetIds[223];
			npcBestiaryCreditIdsByNpcNetIds[-54] = npcBestiaryCreditIdsByNpcNetIds[223];
			npcBestiaryCreditIdsByNpcNetIds[-53] = npcBestiaryCreditIdsByNpcNetIds[21];
			npcBestiaryCreditIdsByNpcNetIds[-52] = npcBestiaryCreditIdsByNpcNetIds[21];
			npcBestiaryCreditIdsByNpcNetIds[-51] = npcBestiaryCreditIdsByNpcNetIds[21];
			npcBestiaryCreditIdsByNpcNetIds[-50] = npcBestiaryCreditIdsByNpcNetIds[21];
			npcBestiaryCreditIdsByNpcNetIds[-49] = npcBestiaryCreditIdsByNpcNetIds[21];
			npcBestiaryCreditIdsByNpcNetIds[-48] = npcBestiaryCreditIdsByNpcNetIds[21];
			npcBestiaryCreditIdsByNpcNetIds[-47] = npcBestiaryCreditIdsByNpcNetIds[21];
			npcBestiaryCreditIdsByNpcNetIds[-46] = npcBestiaryCreditIdsByNpcNetIds[21];
			npcBestiaryCreditIdsByNpcNetIds[-45] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[-44] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[-43] = npcBestiaryCreditIdsByNpcNetIds[2];
			npcBestiaryCreditIdsByNpcNetIds[-42] = npcBestiaryCreditIdsByNpcNetIds[2];
			npcBestiaryCreditIdsByNpcNetIds[-41] = npcBestiaryCreditIdsByNpcNetIds[2];
			npcBestiaryCreditIdsByNpcNetIds[-40] = npcBestiaryCreditIdsByNpcNetIds[2];
			npcBestiaryCreditIdsByNpcNetIds[-39] = npcBestiaryCreditIdsByNpcNetIds[2];
			npcBestiaryCreditIdsByNpcNetIds[-38] = npcBestiaryCreditIdsByNpcNetIds[2];
			npcBestiaryCreditIdsByNpcNetIds[-37] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[-36] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[-35] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[-34] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[-33] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[-32] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[-31] = npcBestiaryCreditIdsByNpcNetIds[186];
			npcBestiaryCreditIdsByNpcNetIds[-30] = npcBestiaryCreditIdsByNpcNetIds[186];
			npcBestiaryCreditIdsByNpcNetIds[-27] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[-26] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[-23] = npcBestiaryCreditIdsByNpcNetIds[173];
			npcBestiaryCreditIdsByNpcNetIds[-22] = npcBestiaryCreditIdsByNpcNetIds[173];
			npcBestiaryCreditIdsByNpcNetIds[-25] = npcBestiaryCreditIdsByNpcNetIds[183];
			npcBestiaryCreditIdsByNpcNetIds[-24] = npcBestiaryCreditIdsByNpcNetIds[183];
			npcBestiaryCreditIdsByNpcNetIds[-21] = npcBestiaryCreditIdsByNpcNetIds[176];
			npcBestiaryCreditIdsByNpcNetIds[-20] = npcBestiaryCreditIdsByNpcNetIds[176];
			npcBestiaryCreditIdsByNpcNetIds[-19] = npcBestiaryCreditIdsByNpcNetIds[176];
			npcBestiaryCreditIdsByNpcNetIds[-18] = npcBestiaryCreditIdsByNpcNetIds[176];
			npcBestiaryCreditIdsByNpcNetIds[-17] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-16] = npcBestiaryCreditIdsByNpcNetIds[42];
			npcBestiaryCreditIdsByNpcNetIds[-15] = npcBestiaryCreditIdsByNpcNetIds[77];
			npcBestiaryCreditIdsByNpcNetIds[-14] = npcBestiaryCreditIdsByNpcNetIds[31];
			npcBestiaryCreditIdsByNpcNetIds[-13] = npcBestiaryCreditIdsByNpcNetIds[31];
			npcBestiaryCreditIdsByNpcNetIds[-12] = npcBestiaryCreditIdsByNpcNetIds[6];
			npcBestiaryCreditIdsByNpcNetIds[-11] = npcBestiaryCreditIdsByNpcNetIds[6];
			npcBestiaryCreditIdsByNpcNetIds[497] = npcBestiaryCreditIdsByNpcNetIds[496];
			npcBestiaryCreditIdsByNpcNetIds[495] = npcBestiaryCreditIdsByNpcNetIds[494];
			short key = 499;
			for (int i = 498; i <= 506; i++)
			{
				npcBestiaryCreditIdsByNpcNetIds[i] = npcBestiaryCreditIdsByNpcNetIds[key];
			}
			npcBestiaryCreditIdsByNpcNetIds[591] = npcBestiaryCreditIdsByNpcNetIds[590];
			npcBestiaryCreditIdsByNpcNetIds[430] = npcBestiaryCreditIdsByNpcNetIds[3];
			npcBestiaryCreditIdsByNpcNetIds[436] = npcBestiaryCreditIdsByNpcNetIds[200];
			npcBestiaryCreditIdsByNpcNetIds[431] = npcBestiaryCreditIdsByNpcNetIds[161];
			npcBestiaryCreditIdsByNpcNetIds[432] = npcBestiaryCreditIdsByNpcNetIds[186];
			npcBestiaryCreditIdsByNpcNetIds[433] = npcBestiaryCreditIdsByNpcNetIds[187];
			npcBestiaryCreditIdsByNpcNetIds[434] = npcBestiaryCreditIdsByNpcNetIds[188];
			npcBestiaryCreditIdsByNpcNetIds[435] = npcBestiaryCreditIdsByNpcNetIds[189];
			npcBestiaryCreditIdsByNpcNetIds[164] = npcBestiaryCreditIdsByNpcNetIds[165];
			npcBestiaryCreditIdsByNpcNetIds[236] = npcBestiaryCreditIdsByNpcNetIds[237];
			npcBestiaryCreditIdsByNpcNetIds[163] = npcBestiaryCreditIdsByNpcNetIds[238];
			npcBestiaryCreditIdsByNpcNetIds[239] = npcBestiaryCreditIdsByNpcNetIds[240];
			npcBestiaryCreditIdsByNpcNetIds[530] = npcBestiaryCreditIdsByNpcNetIds[531];
			npcBestiaryCreditIdsByNpcNetIds[449] = npcBestiaryCreditIdsByNpcNetIds[21];
			npcBestiaryCreditIdsByNpcNetIds[450] = npcBestiaryCreditIdsByNpcNetIds[201];
			npcBestiaryCreditIdsByNpcNetIds[451] = npcBestiaryCreditIdsByNpcNetIds[202];
			npcBestiaryCreditIdsByNpcNetIds[452] = npcBestiaryCreditIdsByNpcNetIds[203];
			npcBestiaryCreditIdsByNpcNetIds[595] = npcBestiaryCreditIdsByNpcNetIds[599];
			npcBestiaryCreditIdsByNpcNetIds[596] = npcBestiaryCreditIdsByNpcNetIds[599];
			npcBestiaryCreditIdsByNpcNetIds[597] = npcBestiaryCreditIdsByNpcNetIds[599];
			npcBestiaryCreditIdsByNpcNetIds[598] = npcBestiaryCreditIdsByNpcNetIds[599];
			npcBestiaryCreditIdsByNpcNetIds[600] = npcBestiaryCreditIdsByNpcNetIds[599];
			npcBestiaryCreditIdsByNpcNetIds[230] = npcBestiaryCreditIdsByNpcNetIds[55];
			npcBestiaryCreditIdsByNpcNetIds[593] = npcBestiaryCreditIdsByNpcNetIds[592];
			npcBestiaryCreditIdsByNpcNetIds[-2] = npcBestiaryCreditIdsByNpcNetIds[121];
			npcBestiaryCreditIdsByNpcNetIds[195] = npcBestiaryCreditIdsByNpcNetIds[196];
			npcBestiaryCreditIdsByNpcNetIds[198] = npcBestiaryCreditIdsByNpcNetIds[199];
			npcBestiaryCreditIdsByNpcNetIds[158] = npcBestiaryCreditIdsByNpcNetIds[159];
			npcBestiaryCreditIdsByNpcNetIds[568] = npcBestiaryCreditIdsByNpcNetIds[569];
			npcBestiaryCreditIdsByNpcNetIds[566] = npcBestiaryCreditIdsByNpcNetIds[567];
			npcBestiaryCreditIdsByNpcNetIds[576] = npcBestiaryCreditIdsByNpcNetIds[577];
			npcBestiaryCreditIdsByNpcNetIds[558] = npcBestiaryCreditIdsByNpcNetIds[560];
			npcBestiaryCreditIdsByNpcNetIds[559] = npcBestiaryCreditIdsByNpcNetIds[560];
			npcBestiaryCreditIdsByNpcNetIds[552] = npcBestiaryCreditIdsByNpcNetIds[554];
			npcBestiaryCreditIdsByNpcNetIds[553] = npcBestiaryCreditIdsByNpcNetIds[554];
			npcBestiaryCreditIdsByNpcNetIds[564] = npcBestiaryCreditIdsByNpcNetIds[565];
			npcBestiaryCreditIdsByNpcNetIds[570] = npcBestiaryCreditIdsByNpcNetIds[571];
			npcBestiaryCreditIdsByNpcNetIds[555] = npcBestiaryCreditIdsByNpcNetIds[557];
			npcBestiaryCreditIdsByNpcNetIds[556] = npcBestiaryCreditIdsByNpcNetIds[557];
			npcBestiaryCreditIdsByNpcNetIds[574] = npcBestiaryCreditIdsByNpcNetIds[575];
			npcBestiaryCreditIdsByNpcNetIds[561] = npcBestiaryCreditIdsByNpcNetIds[563];
			npcBestiaryCreditIdsByNpcNetIds[562] = npcBestiaryCreditIdsByNpcNetIds[563];
			npcBestiaryCreditIdsByNpcNetIds[572] = npcBestiaryCreditIdsByNpcNetIds[573];
			npcBestiaryCreditIdsByNpcNetIds[14] = npcBestiaryCreditIdsByNpcNetIds[13];
			npcBestiaryCreditIdsByNpcNetIds[15] = npcBestiaryCreditIdsByNpcNetIds[13];
		}

		private static void CreateBestiarySortingIds(BestiaryDatabase database)
		{
			List<KeyValuePair<int, NPC>> sortedBestiaryEntriesList = BestiaryHelper.GetSortedBestiaryEntriesList(database);
			int num = 1;
			foreach (KeyValuePair<int, NPC> item in sortedBestiaryEntriesList)
			{
				NpcBestiarySortingId[item.Key] = num;
				num++;
			}
		}

		private static void FillNpcRarities()
		{
			NPCSpawnParams nPCSpawnParams = default(NPCSpawnParams);
			nPCSpawnParams.gameModeData = Main.RegisteredGameModes[0];
			NPCSpawnParams spawnparams = nPCSpawnParams;
			for (int i = -65; i < 688; i++)
			{
				NPC nPC = new NPC();
				nPC.SetDefaults(i, spawnparams);
				NpcBestiaryRarityStars[i] = GetNPCBestiaryRarityStarsCount(nPC);
			}
			NpcBestiaryRarityStars[22] = 1;
			NpcBestiaryRarityStars[17] = 1;
			NpcBestiaryRarityStars[18] = 1;
			NpcBestiaryRarityStars[38] = 1;
			NpcBestiaryRarityStars[369] = 2;
			NpcBestiaryRarityStars[20] = 3;
			NpcBestiaryRarityStars[19] = 1;
			NpcBestiaryRarityStars[227] = 2;
			NpcBestiaryRarityStars[353] = 2;
			NpcBestiaryRarityStars[550] = 2;
			NpcBestiaryRarityStars[588] = 2;
			NpcBestiaryRarityStars[107] = 3;
			NpcBestiaryRarityStars[228] = 2;
			NpcBestiaryRarityStars[124] = 2;
			NpcBestiaryRarityStars[54] = 2;
			NpcBestiaryRarityStars[108] = 3;
			NpcBestiaryRarityStars[178] = 3;
			NpcBestiaryRarityStars[216] = 3;
			NpcBestiaryRarityStars[160] = 5;
			NpcBestiaryRarityStars[441] = 5;
			NpcBestiaryRarityStars[209] = 3;
			NpcBestiaryRarityStars[208] = 4;
			NpcBestiaryRarityStars[142] = 5;
			NpcBestiaryRarityStars[368] = 3;
			NpcBestiaryRarityStars[453] = 4;
			NpcBestiaryRarityStars[37] = 2;
			NpcBestiaryRarityStars[633] = 5;
			NpcBestiaryRarityStars[663] = 5;
			NpcBestiaryRarityStars[638] = 3;
			NpcBestiaryRarityStars[637] = 3;
			NpcBestiaryRarityStars[656] = 3;
			NpcBestiaryRarityStars[670] = 3;
			NpcBestiaryRarityStars[678] = 3;
			NpcBestiaryRarityStars[679] = 3;
			NpcBestiaryRarityStars[680] = 3;
			NpcBestiaryRarityStars[681] = 3;
			NpcBestiaryRarityStars[682] = 3;
			NpcBestiaryRarityStars[683] = 3;
			NpcBestiaryRarityStars[684] = 3;
			NpcBestiaryRarityStars[664] = 5;
			NpcBestiaryRarityStars[484] = 5;
			NpcBestiaryRarityStars[614] = 4;
			NpcBestiaryRarityStars[303] = 4;
			NpcBestiaryRarityStars[337] = 4;
			NpcBestiaryRarityStars[360] = 3;
			NpcBestiaryRarityStars[655] = 2;
			NpcBestiaryRarityStars[374] = 3;
			NpcBestiaryRarityStars[661] = 3;
			NpcBestiaryRarityStars[362] = 2;
			NpcBestiaryRarityStars[364] = 2;
			NpcBestiaryRarityStars[616] = 2;
			NpcBestiaryRarityStars[298] = 2;
			NpcBestiaryRarityStars[671] = 3;
			NpcBestiaryRarityStars[672] = 3;
			NpcBestiaryRarityStars[673] = 3;
			NpcBestiaryRarityStars[674] = 3;
			NpcBestiaryRarityStars[675] = 3;
			NpcBestiaryRarityStars[599] = 3;
			NpcBestiaryRarityStars[355] = 2;
			NpcBestiaryRarityStars[358] = 3;
			NpcBestiaryRarityStars[654] = 3;
			NpcBestiaryRarityStars[653] = 2;
			NpcBestiaryRarityStars[540] = 2;
			NpcBestiaryRarityStars[604] = 3;
			NpcBestiaryRarityStars[611] = 3;
			NpcBestiaryRarityStars[612] = 2;
			NpcBestiaryRarityStars[608] = 2;
			NpcBestiaryRarityStars[607] = 2;
			NpcBestiaryRarityStars[615] = 3;
			NpcBestiaryRarityStars[626] = 2;
			NpcBestiaryRarityStars[486] = 2;
			NpcBestiaryRarityStars[487] = 3;
			NpcBestiaryRarityStars[669] = 3;
			NpcBestiaryRarityStars[677] = 5;
			NpcBestiaryRarityStars[676] = 5;
			NpcBestiaryRarityStars[149] = 2;
			NpcBestiaryRarityStars[366] = 2;
			NpcBestiaryRarityStars[47] = 3;
			NpcBestiaryRarityStars[57] = 3;
			NpcBestiaryRarityStars[168] = 3;
			NpcBestiaryRarityStars[464] = 3;
			NpcBestiaryRarityStars[465] = 3;
			NpcBestiaryRarityStars[470] = 3;
			NpcBestiaryRarityStars[301] = 2;
			NpcBestiaryRarityStars[316] = 3;
			NpcBestiaryRarityStars[546] = 2;
			NpcBestiaryRarityStars[170] = 3;
			NpcBestiaryRarityStars[180] = 3;
			NpcBestiaryRarityStars[171] = 3;
			NpcBestiaryRarityStars[29] = 2;
			NpcBestiaryRarityStars[471] = 4;
			NpcBestiaryRarityStars[66] = 3;
			NpcBestiaryRarityStars[223] = 2;
			NpcBestiaryRarityStars[161] = 2;
			NpcBestiaryRarityStars[491] = 4;
			NpcBestiaryRarityStars[-9] = 3;
			NpcBestiaryRarityStars[594] = 2;
			NpcBestiaryRarityStars[628] = 2;
			NpcBestiaryRarityStars[225] = 2;
			NpcBestiaryRarityStars[224] = 2;
			NpcBestiaryRarityStars[250] = 3;
			NpcBestiaryRarityStars[16] = 2;
			NpcBestiaryRarityStars[481] = 2;
			NpcBestiaryRarityStars[483] = 2;
			NpcBestiaryRarityStars[184] = 2;
			NpcBestiaryRarityStars[185] = 3;
			NpcBestiaryRarityStars[206] = 3;
			NpcBestiaryRarityStars[541] = 4;
			NpcBestiaryRarityStars[537] = 2;
			NpcBestiaryRarityStars[205] = 4;
			NpcBestiaryRarityStars[499] = 2;
			NpcBestiaryRarityStars[494] = 2;
			NpcBestiaryRarityStars[496] = 2;
			NpcBestiaryRarityStars[302] = 3;
			NpcBestiaryRarityStars[317] = 3;
			NpcBestiaryRarityStars[318] = 3;
			NpcBestiaryRarityStars[319] = 3;
			NpcBestiaryRarityStars[320] = 3;
			NpcBestiaryRarityStars[321] = 3;
			NpcBestiaryRarityStars[331] = 3;
			NpcBestiaryRarityStars[332] = 3;
			NpcBestiaryRarityStars[322] = 3;
			NpcBestiaryRarityStars[323] = 3;
			NpcBestiaryRarityStars[324] = 3;
			NpcBestiaryRarityStars[335] = 3;
			NpcBestiaryRarityStars[336] = 3;
			NpcBestiaryRarityStars[333] = 3;
			NpcBestiaryRarityStars[334] = 3;
			NpcBestiaryRarityStars[4] = 2;
			NpcBestiaryRarityStars[50] = 2;
			NpcBestiaryRarityStars[35] = 3;
			NpcBestiaryRarityStars[13] = 3;
			NpcBestiaryRarityStars[134] = 4;
			NpcBestiaryRarityStars[262] = 4;
			NpcBestiaryRarityStars[668] = 3;
		}

		private static int GetNPCBestiaryRarityStarsCount(NPC npc)
		{
			float num = 1f;
			num += (float)npc.rarity;
			if (npc.rarity == 1)
			{
				num += 1f;
			}
			else if (npc.rarity == 2)
			{
				num += 1.5f;
			}
			else if (npc.rarity == 3)
			{
				num += 2f;
			}
			else if (npc.rarity == 4)
			{
				num += 2.5f;
			}
			else if (npc.rarity == 5)
			{
				num += 3f;
			}
			else if (npc.rarity > 0)
			{
				num += 3.5f;
			}
			if (npc.boss)
			{
				num += 0.5f;
			}
			int num2 = npc.damage + npc.defense + npc.lifeMax / 4;
			if (num2 > 10000)
			{
				num += 3.5f;
			}
			else if (num2 > 5000)
			{
				num += 3f;
			}
			else if (num2 > 1000)
			{
				num += 2.5f;
			}
			else if (num2 > 500)
			{
				num += 2f;
			}
			else if (num2 > 150)
			{
				num += 1.5f;
			}
			else if (num2 > 50)
			{
				num += 1f;
			}
			if (num > 5f)
			{
				num = 5f;
			}
			return (int)num;
		}

		private static void CreateCreativeItemSortingIds()
		{
			CreativeHelper.SetCreativeMenuOrder();
		}
	}
}
