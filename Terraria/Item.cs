using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Events;
using Terraria.GameContent.Items;
using Terraria.GameContent.Prefixes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria
{
	public class Item : Entity
	{
		private string _nameOverride;

		public const int luckPotionDuration1 = 18000;

		public const int luckPotionDuration2 = 36000;

		public const int luckPotionDuration3 = 54000;

		public const int flaskTime = 72000;

		public const int copper = 1;

		public const int silver = 100;

		public const int gold = 10000;

		public const int platinum = 1000000;

		public const int goldCritterRarityColor = 3;

		private readonly int shadowOrbPrice = sellPrice(0, 1, 50);

		private readonly int dungeonPrice = sellPrice(0, 1, 75);

		private readonly int queenBeePrice = sellPrice(0, 2);

		private readonly int hellPrice = sellPrice(0, 2, 50);

		private readonly int eclipsePrice = sellPrice(0, 7, 50);

		private readonly int eclipsePostPlanteraPrice = sellPrice(0, 10);

		private readonly int eclipseMothronPrice = sellPrice(0, 12, 50);

		public static int CommonMaxStack = 9999;

		public static int[] cachedItemSpawnsByType = ItemID.Sets.Factory.CreateIntSet(-1);

		public static int potionDelay = 3600;

		public static int restorationDelay = 2700;

		public static int mushroomDelay = 1800;

		public bool questItem;

		public static int[] headType = new int[282];

		public static int[] bodyType = new int[248];

		public static int[] legType = new int[236];

		public static bool[] staff = new bool[5453];

		public static bool[] claw = new bool[5453];

		public bool flame;

		public bool mech;

		public int noGrabDelay;

		public bool beingGrabbed;

		public int timeSinceItemSpawned;

		public int tileWand = -1;

		public bool wornArmor;

		public int tooltipContext = -1;

		public byte dye;

		public int fishingPole = 1;

		public int bait;

		public static int coinGrabRange = 350;

		public static int manaGrabRange = 300;

		public static int lifeGrabRange = 250;

		public static int treasureGrabRange = 150;

		public short makeNPC;

		public bool expertOnly;

		public bool expert;

		public bool isAShopItem;

		public short hairDye = -1;

		public byte paint;

		public byte paintCoating;

		public bool instanced;

		public int ownIgnore = -1;

		public int ownTime;

		public int keepTime;

		public int timeLeftInWhichTheItemCannotBeTakenByEnemies;

		public int type;

		public bool favorited;

		public int holdStyle;

		public int useStyle;

		public bool channel;

		public bool accessory;

		public int useAnimation;

		public int useTime;

		public int stack;

		public int maxStack;

		public int pick;

		public int axe;

		public int hammer;

		public int tileBoost;

		public int createTile = -1;

		public int createWall = -1;

		public int placeStyle;

		public int damage;

		public float knockBack;

		public int healLife;

		public int healMana;

		public bool potion;

		public bool consumable;

		public bool autoReuse;

		public bool useTurn;

		public Color color;

		public int alpha;

		public short glowMask;

		public float scale = 1f;

		public LegacySoundStyle UseSound;

		public int defense;

		public int headSlot = -1;

		public int bodySlot = -1;

		public int legSlot = -1;

		public sbyte handOnSlot = -1;

		public sbyte handOffSlot = -1;

		public sbyte backSlot = -1;

		public sbyte frontSlot = -1;

		public sbyte shoeSlot = -1;

		public sbyte waistSlot = -1;

		public sbyte wingSlot = -1;

		public sbyte shieldSlot = -1;

		public sbyte neckSlot = -1;

		public sbyte faceSlot = -1;

		public sbyte balloonSlot = -1;

		public sbyte beardSlot = -1;

		public int stringColor;

		public ItemTooltip ToolTip;

		public string BestiaryNotes;

		public int playerIndexTheItemIsReservedFor = 255;

		public int rare;

		public int shoot;

		public float shootSpeed;

		public int ammo = AmmoID.None;

		public bool notAmmo;

		public int useAmmo = AmmoID.None;

		public int lifeRegen;

		public int manaIncrease;

		public bool buyOnce;

		public int mana;

		public bool noUseGraphic;

		public bool noMelee;

		public int timeSinceTheItemHasBeenReservedForSomeone;

		public int value;

		public bool buy;

		public bool social;

		public bool vanity;

		public bool material;

		public bool noWet;

		public int buffType;

		public int buffTime;

		public int mountType = -1;

		public bool cartTrack;

		public bool uniqueStack;

		public int shopSpecialCurrency = -1;

		public int? shopCustomPrice;

		public bool shootsEveryUse;

		public bool DD2Summon;

		public int netID;

		public int crit;

		public byte prefix;

		public bool melee;

		public bool magic;

		public bool ranged;

		public bool summon;

		public bool sentry;

		public int reuseDelay;

		public bool newAndShiny;

		[Old("This is used to allow items to be discerned as vanity even if they didn't have visual slots to poll against")]
		public bool hasVanityEffects;

		private const int foodWidth = 22;

		private const int foodHeight = 22;

		public const int WALL_PLACEMENT_USETIME = 7;

		public static int numberOfNewItems = 0;

		public bool shimmered;

		public float shimmerTime;

		public string Name => _nameOverride ?? Lang.GetItemNameValue(type);

		public string HoverName
		{
			get
			{
				string text = AffixName();
				if (stack > 1)
				{
					text = text + " (" + stack + ")";
				}
				return text;
			}
		}

		public bool PaintOrCoating
		{
			get
			{
				if (paint <= 0)
				{
					return paintCoating > 0;
				}
				return true;
			}
		}

		public bool FitsAccessoryVanitySlot => true;

		public int OriginalRarity => ContentSamples.ItemsByType[type].rare;

		public int OriginalDamage => ContentSamples.ItemsByType[type].damage;

		public int OriginalDefense => ContentSamples.ItemsByType[type].defense;

		public ItemVariant Variant { get; private set; }

		public bool IsACoin
		{
			get
			{
				int num = type;
				if ((uint)(num - 71) <= 3u)
				{
					return true;
				}
				return false;
			}
		}

		public bool IsAir
		{
			get
			{
				if (type > 0)
				{
					return stack <= 0;
				}
				return true;
			}
		}

		public bool IsCurrency
		{
			get
			{
				if (type < 71 || type > 74)
				{
					return CustomCurrencyManager.IsCustomCurrency(this);
				}
				return true;
			}
		}

		public bool CanBeQuickUsed
		{
			get
			{
				if (IsAir)
				{
					return false;
				}
				bool? flag = ItemID.Sets.CanBeQuickusedOnGamepad[type];
				if (flag.HasValue)
				{
					return flag.Value;
				}
				if (healLife > 0)
				{
					return true;
				}
				if (healMana > 0)
				{
					return true;
				}
				if (buffType > 0 && buffTime > 0)
				{
					return true;
				}
				return false;
			}
		}

		public static void StartCachingType(int t)
		{
			if (cachedItemSpawnsByType[t] == -1)
			{
				cachedItemSpawnsByType[t] = 0;
			}
		}

		public static void DropCache(IEntitySource reason, Vector2 pos, Vector2 spread, int t, bool stopCaching = true)
		{
			if (cachedItemSpawnsByType[t] == -1)
			{
				return;
			}
			int num = cachedItemSpawnsByType[t];
			cachedItemSpawnsByType[t] = (stopCaching ? (-1) : 0);
			Item item = new Item();
			item.SetDefaults(t);
			while (num > 0)
			{
				int num2 = item.maxStack;
				if (num < num2)
				{
					num2 = num;
				}
				NewItem(reason, (int)pos.X, (int)pos.Y, (int)spread.X, (int)spread.Y, t, num2);
				num -= num2;
			}
		}

		public IEntitySource GetItemSource_Misc(int itemSourceId)
		{
			return new EntitySource_ByItemSourceId(this, itemSourceId);
		}

		public override string ToString()
		{
			return $"{{Name: \"{Name}\" NetID: {netID} Stack: {stack}";
		}

		private bool CanHavePrefixes()
		{
			if (type != 0 && maxStack == 1)
			{
				if (damage <= 0)
				{
					return accessory;
				}
				return true;
			}
			return false;
		}

		public bool Prefix(int prefixWeWant)
		{
			if (!WorldGen.gen && Main.rand == null)
			{
				Main.rand = new UnifiedRandom();
			}
			if (prefixWeWant == 0)
			{
				return false;
			}
			if (!CanHavePrefixes())
			{
				return false;
			}
			UnifiedRandom unifiedRandom = (WorldGen.gen ? WorldGen.genRand : Main.rand);
			int rolledPrefix = prefixWeWant;
			float dmg = 1f;
			float kb = 1f;
			float spd = 1f;
			float size = 1f;
			float shtspd = 1f;
			float mcst = 1f;
			int crt = 0;
			bool flag = true;
			while (flag)
			{
				flag = false;
				if (rolledPrefix == -1 && unifiedRandom.Next(4) == 0)
				{
					rolledPrefix = 0;
				}
				if (prefixWeWant < -1)
				{
					rolledPrefix = -1;
				}
				if ((rolledPrefix == -1 || rolledPrefix == -2 || rolledPrefix == -3) && !RollAPrefix(unifiedRandom, ref rolledPrefix))
				{
					return false;
				}
				switch (prefixWeWant)
				{
				case -3:
					return true;
				case -1:
					if ((rolledPrefix == 7 || rolledPrefix == 8 || rolledPrefix == 9 || rolledPrefix == 10 || rolledPrefix == 11 || rolledPrefix == 22 || rolledPrefix == 23 || rolledPrefix == 24 || rolledPrefix == 29 || rolledPrefix == 30 || rolledPrefix == 31 || rolledPrefix == 39 || rolledPrefix == 40 || rolledPrefix == 56 || rolledPrefix == 41 || rolledPrefix == 47 || rolledPrefix == 48 || rolledPrefix == 49) && unifiedRandom.Next(3) != 0)
					{
						rolledPrefix = 0;
					}
					break;
				}
				if (!TryGetPrefixStatMultipliersForItem(rolledPrefix, out dmg, out kb, out spd, out size, out shtspd, out mcst, out crt))
				{
					flag = true;
					rolledPrefix = -1;
				}
				if (prefixWeWant == -2 && rolledPrefix == 0)
				{
					rolledPrefix = -1;
					flag = true;
				}
			}
			damage = (int)Math.Round((float)damage * dmg);
			useAnimation = (int)Math.Round((float)useAnimation * spd);
			useTime = (int)Math.Round((float)useTime * spd);
			reuseDelay = (int)Math.Round((float)reuseDelay * spd);
			mana = (int)Math.Round((float)mana * mcst);
			knockBack *= kb;
			scale *= size;
			shootSpeed *= shtspd;
			crit += crt;
			float num = 1f * dmg * (2f - spd) * (2f - mcst) * size * kb * shtspd * (1f + (float)crt * 0.02f);
			if (rolledPrefix == 62 || rolledPrefix == 69 || rolledPrefix == 73 || rolledPrefix == 77)
			{
				num *= 1.05f;
			}
			if (rolledPrefix == 63 || rolledPrefix == 70 || rolledPrefix == 74 || rolledPrefix == 78 || rolledPrefix == 67)
			{
				num *= 1.1f;
			}
			if (rolledPrefix == 64 || rolledPrefix == 71 || rolledPrefix == 75 || rolledPrefix == 79 || rolledPrefix == 66)
			{
				num *= 1.15f;
			}
			if (rolledPrefix == 65 || rolledPrefix == 72 || rolledPrefix == 76 || rolledPrefix == 80 || rolledPrefix == 68)
			{
				num *= 1.2f;
			}
			if ((double)num >= 1.2)
			{
				rare += 2;
			}
			else if ((double)num >= 1.05)
			{
				rare++;
			}
			else if ((double)num <= 0.8)
			{
				rare -= 2;
			}
			else if ((double)num <= 0.95)
			{
				rare--;
			}
			if (rare > -11)
			{
				if (rare < -1)
				{
					rare = -1;
				}
				if (rare > 11)
				{
					rare = 11;
				}
			}
			num *= num;
			value = (int)((float)value * num);
			prefix = (byte)rolledPrefix;
			return true;
		}

		public bool CanRollPrefix(int prefix)
		{
			if (!CanHavePrefixes())
			{
				return false;
			}
			int[] rollablePrefixes = GetRollablePrefixes();
			if (rollablePrefixes == null)
			{
				return false;
			}
			for (int i = 0; i < rollablePrefixes.Length; i++)
			{
				if (rollablePrefixes[i] == prefix)
				{
					return true;
				}
			}
			return false;
		}

		public bool CanApplyPrefix(int prefix)
		{
			if (!CanRollPrefix(prefix))
			{
				return false;
			}
			float dmg = 1f;
			float kb = 1f;
			float spd = 1f;
			float size = 1f;
			float shtspd = 1f;
			float mcst = 1f;
			int crt = 0;
			return TryGetPrefixStatMultipliersForItem(prefix, out dmg, out kb, out spd, out size, out shtspd, out mcst, out crt);
		}

		private bool TryGetPrefixStatMultipliersForItem(int rolledPrefix, out float dmg, out float kb, out float spd, out float size, out float shtspd, out float mcst, out int crt)
		{
			dmg = 1f;
			kb = 1f;
			spd = 1f;
			size = 1f;
			shtspd = 1f;
			mcst = 1f;
			crt = 0;
			switch (rolledPrefix)
			{
			case 1:
				size = 1.12f;
				break;
			case 2:
				size = 1.18f;
				break;
			case 3:
				dmg = 1.05f;
				crt = 2;
				size = 1.05f;
				break;
			case 4:
				dmg = 1.1f;
				size = 1.1f;
				kb = 1.1f;
				break;
			case 5:
				dmg = 1.15f;
				break;
			case 6:
				dmg = 1.1f;
				break;
			case 81:
				kb = 1.15f;
				dmg = 1.15f;
				crt = 5;
				spd = 0.9f;
				size = 1.1f;
				break;
			case 7:
				size = 0.82f;
				break;
			case 8:
				kb = 0.85f;
				dmg = 0.85f;
				size = 0.87f;
				break;
			case 9:
				size = 0.9f;
				break;
			case 10:
				dmg = 0.85f;
				break;
			case 11:
				spd = 1.1f;
				kb = 0.9f;
				size = 0.9f;
				break;
			case 12:
				kb = 1.1f;
				dmg = 1.05f;
				size = 1.1f;
				spd = 1.15f;
				break;
			case 13:
				kb = 0.8f;
				dmg = 0.9f;
				size = 1.1f;
				break;
			case 14:
				kb = 1.15f;
				spd = 1.1f;
				break;
			case 15:
				kb = 0.9f;
				spd = 0.85f;
				break;
			case 16:
				dmg = 1.1f;
				crt = 3;
				break;
			case 17:
				spd = 0.85f;
				shtspd = 1.1f;
				break;
			case 18:
				spd = 0.9f;
				shtspd = 1.15f;
				break;
			case 19:
				kb = 1.15f;
				shtspd = 1.05f;
				break;
			case 20:
				kb = 1.05f;
				shtspd = 1.05f;
				dmg = 1.1f;
				spd = 0.95f;
				crt = 2;
				break;
			case 21:
				kb = 1.15f;
				dmg = 1.1f;
				break;
			case 82:
				kb = 1.15f;
				dmg = 1.15f;
				crt = 5;
				spd = 0.9f;
				shtspd = 1.1f;
				break;
			case 22:
				kb = 0.9f;
				shtspd = 0.9f;
				dmg = 0.85f;
				break;
			case 23:
				spd = 1.15f;
				shtspd = 0.9f;
				break;
			case 24:
				spd = 1.1f;
				kb = 0.8f;
				break;
			case 25:
				spd = 1.1f;
				dmg = 1.15f;
				crt = 1;
				break;
			case 58:
				spd = 0.85f;
				dmg = 0.85f;
				break;
			case 26:
				mcst = 0.85f;
				dmg = 1.1f;
				break;
			case 27:
				mcst = 0.85f;
				break;
			case 28:
				mcst = 0.85f;
				dmg = 1.15f;
				kb = 1.05f;
				break;
			case 83:
				kb = 1.15f;
				dmg = 1.15f;
				crt = 5;
				spd = 0.9f;
				mcst = 0.9f;
				break;
			case 29:
				mcst = 1.1f;
				break;
			case 30:
				mcst = 1.2f;
				dmg = 0.9f;
				break;
			case 31:
				kb = 0.9f;
				dmg = 0.9f;
				break;
			case 32:
				mcst = 1.15f;
				dmg = 1.1f;
				break;
			case 33:
				mcst = 1.1f;
				kb = 1.1f;
				spd = 0.9f;
				break;
			case 34:
				mcst = 0.9f;
				kb = 1.1f;
				spd = 1.1f;
				dmg = 1.1f;
				break;
			case 35:
				mcst = 1.2f;
				dmg = 1.15f;
				kb = 1.15f;
				break;
			case 52:
				mcst = 0.9f;
				dmg = 0.9f;
				spd = 0.9f;
				break;
			case 84:
				kb = 1.17f;
				dmg = 1.17f;
				crt = 8;
				break;
			case 36:
				crt = 3;
				break;
			case 37:
				dmg = 1.1f;
				crt = 3;
				kb = 1.1f;
				break;
			case 38:
				kb = 1.15f;
				break;
			case 53:
				dmg = 1.1f;
				break;
			case 54:
				kb = 1.15f;
				break;
			case 55:
				kb = 1.15f;
				dmg = 1.05f;
				break;
			case 59:
				kb = 1.15f;
				dmg = 1.15f;
				crt = 5;
				break;
			case 60:
				dmg = 1.15f;
				crt = 5;
				break;
			case 61:
				crt = 5;
				break;
			case 39:
				dmg = 0.7f;
				kb = 0.8f;
				break;
			case 40:
				dmg = 0.85f;
				break;
			case 56:
				kb = 0.8f;
				break;
			case 41:
				kb = 0.85f;
				dmg = 0.9f;
				break;
			case 57:
				kb = 0.9f;
				dmg = 1.18f;
				break;
			case 42:
				spd = 0.9f;
				break;
			case 43:
				dmg = 1.1f;
				spd = 0.9f;
				break;
			case 44:
				spd = 0.9f;
				crt = 3;
				break;
			case 45:
				spd = 0.95f;
				break;
			case 46:
				crt = 3;
				spd = 0.94f;
				dmg = 1.07f;
				break;
			case 47:
				spd = 1.15f;
				break;
			case 48:
				spd = 1.2f;
				break;
			case 49:
				spd = 1.08f;
				break;
			case 50:
				dmg = 0.8f;
				spd = 1.15f;
				break;
			case 51:
				kb = 0.9f;
				spd = 0.9f;
				dmg = 1.05f;
				crt = 2;
				break;
			}
			if (dmg != 1f && Math.Round((float)damage * dmg) == (double)damage)
			{
				return false;
			}
			if (spd != 1f && Math.Round((float)useAnimation * spd) == (double)useAnimation)
			{
				return false;
			}
			if (mcst != 1f && Math.Round((float)mana * mcst) == (double)mana)
			{
				return false;
			}
			if (kb != 1f && knockBack == 0f)
			{
				return false;
			}
			return true;
		}

		public int[] GetRollablePrefixes()
		{
			_ = type;
			if (PrefixLegacy.ItemSets.SwordsHammersAxesPicks[type])
			{
				return PrefixLegacy.Prefixes.PrefixesForSwords;
			}
			if (PrefixLegacy.ItemSets.SpearsMacesChainsawsDrillsPunchCannon[type])
			{
				return PrefixLegacy.Prefixes.PrefixesForSpears;
			}
			if (PrefixLegacy.ItemSets.GunsBows[type])
			{
				return PrefixLegacy.Prefixes.PrefixesForGunsBows;
			}
			if (PrefixLegacy.ItemSets.MagicAndSummon[type])
			{
				return PrefixLegacy.Prefixes.PrefixesForMagicAndSummons;
			}
			if (PrefixLegacy.ItemSets.BoomerangsChakrams[type])
			{
				return PrefixLegacy.Prefixes.PrefixesForBoomeransAndChakrums;
			}
			if (PrefixLegacy.ItemSets.ItemsThatCanHaveLegendary2[type])
			{
				return PrefixLegacy.Prefixes.PrefixesForBoomeransAndChakrums_TerrarianYoyo;
			}
			if (IsAPrefixableAccessory())
			{
				return PrefixLegacy.Prefixes.PrefixesForAccessories;
			}
			return null;
		}

		private bool RollAPrefix(UnifiedRandom random, ref int rolledPrefix)
		{
			int[] rollablePrefixes = GetRollablePrefixes();
			if (rollablePrefixes == null)
			{
				return false;
			}
			rolledPrefix = rollablePrefixes[random.Next(rollablePrefixes.Length)];
			return true;
		}

		public bool IsAPrefixableAccessory()
		{
			if (accessory && !vanity)
			{
				return ItemID.Sets.CanGetPrefixes[type];
			}
			return false;
		}

		public string AffixName()
		{
			if (prefix < 0 || prefix >= Lang.prefix.Length)
			{
				return Name;
			}
			string text = Lang.prefix[prefix].Value;
			if (text == "")
			{
				return Name;
			}
			if (text.StartsWith("("))
			{
				return Name + " " + text;
			}
			return text + " " + Name;
		}

		public void RebuildTooltip()
		{
			if (type >= 0)
			{
				ToolTip = Lang.GetTooltip(netID);
			}
		}

		public Rectangle getRect()
		{
			return new Rectangle((int)position.X, (int)position.Y, width, height);
		}

		public bool checkMat()
		{
			if (type >= 71 && type <= 74)
			{
				material = false;
				return false;
			}
			switch (type)
			{
			case 529:
			case 541:
			case 542:
			case 543:
			case 852:
			case 853:
			case 1151:
			case 3272:
			case 3274:
			case 3275:
			case 3338:
			case 4261:
			case 4282:
			case 4286:
			case 4290:
			case 4295:
			case 5277:
			case 5278:
				material = true;
				return true;
			case 4076:
			case 4131:
			case 5325:
				material = false;
				return false;
			default:
			{
				for (int i = 0; i < Recipe.numRecipes; i++)
				{
					for (int j = 0; Main.recipe[i].requiredItem[j].type > 0; j++)
					{
						if (netID == Main.recipe[i].requiredItem[j].netID)
						{
							material = true;
							return true;
						}
					}
				}
				material = false;
				return false;
			}
			}
		}

		public void netDefaults(int type)
		{
			if (type < 0)
			{
				switch (type)
				{
				case -1:
					SetDefaults(3521);
					break;
				case -2:
					SetDefaults(3520);
					break;
				case -3:
					SetDefaults(3519);
					break;
				case -4:
					SetDefaults(3518);
					break;
				case -5:
					SetDefaults(3517);
					break;
				case -6:
					SetDefaults(3516);
					break;
				case -7:
					SetDefaults(3515);
					break;
				case -8:
					SetDefaults(3514);
					break;
				case -9:
					SetDefaults(3513);
					break;
				case -10:
					SetDefaults(3512);
					break;
				case -11:
					SetDefaults(3511);
					break;
				case -12:
					SetDefaults(3510);
					break;
				case -13:
					SetDefaults(3509);
					break;
				case -14:
					SetDefaults(3508);
					break;
				case -15:
					SetDefaults(3507);
					break;
				case -16:
					SetDefaults(3506);
					break;
				case -17:
					SetDefaults(3505);
					break;
				case -18:
					SetDefaults(3504);
					break;
				case -19:
					SetDefaults(3764);
					break;
				case -20:
					SetDefaults(3765);
					break;
				case -21:
					SetDefaults(3766);
					break;
				case -22:
					SetDefaults(3767);
					break;
				case -23:
					SetDefaults(3768);
					break;
				case -24:
					SetDefaults(3769);
					break;
				case -25:
					SetDefaults(3503);
					break;
				case -26:
					SetDefaults(3502);
					break;
				case -27:
					SetDefaults(3501);
					break;
				case -28:
					SetDefaults(3500);
					break;
				case -29:
					SetDefaults(3499);
					break;
				case -30:
					SetDefaults(3498);
					break;
				case -31:
					SetDefaults(3497);
					break;
				case -32:
					SetDefaults(3496);
					break;
				case -33:
					SetDefaults(3495);
					break;
				case -34:
					SetDefaults(3494);
					break;
				case -35:
					SetDefaults(3493);
					break;
				case -36:
					SetDefaults(3492);
					break;
				case -37:
					SetDefaults(3491);
					break;
				case -38:
					SetDefaults(3490);
					break;
				case -39:
					SetDefaults(3489);
					break;
				case -40:
					SetDefaults(3488);
					break;
				case -41:
					SetDefaults(3487);
					break;
				case -42:
					SetDefaults(3486);
					break;
				case -43:
					SetDefaults(3485);
					break;
				case -44:
					SetDefaults(3484);
					break;
				case -45:
					SetDefaults(3483);
					break;
				case -46:
					SetDefaults(3482);
					break;
				case -47:
					SetDefaults(3481);
					break;
				case -48:
					SetDefaults(3480);
					break;
				}
			}
			else
			{
				SetDefaults(type);
			}
		}

		public static int BannerToItem(int banner)
		{
			int num = 0;
			if (banner == 289)
			{
				return 5352;
			}
			if (banner >= 276)
			{
				return 4965 + banner - 276;
			}
			if (banner >= 274)
			{
				return 4687 + banner - 274;
			}
			if (banner == 273)
			{
				return 4602;
			}
			if (banner >= 267)
			{
				return 4541 + banner - 267;
			}
			if (banner >= 257)
			{
				return 3837 + banner - 257;
			}
			if (banner >= 252)
			{
				return 3789 + banner - 252;
			}
			if (banner == 251)
			{
				return 3780;
			}
			if (banner >= 249)
			{
				return 3593 + banner - 249;
			}
			if (banner >= 186)
			{
				return 3390 + banner - 186;
			}
			if (banner >= 88)
			{
				return 2897 + banner - 88;
			}
			return 1615 + banner - 1;
		}

		public static int NPCtoBanner(int i)
		{
			switch (i)
			{
			case 102:
				return 1;
			case 250:
				return 2;
			case 257:
				return 3;
			case 69:
				return 4;
			case 157:
				return 5;
			case 77:
				return 6;
			case 49:
				return 7;
			case 74:
				return 8;
			case 163:
			case 238:
				return 9;
			case 241:
				return 10;
			case 242:
				return 11;
			case 239:
			case 240:
				return 12;
			case 39:
			case 40:
			case 41:
				return 13;
			case 46:
			case 303:
			case 337:
			case 540:
				return 14;
			case 120:
				return 15;
			case 85:
			case 629:
				return 16;
			case 109:
			case 378:
				return 17;
			case 47:
				return 18;
			case 57:
				return 19;
			case 67:
				return 20;
			case 173:
				return 21;
			case 179:
				return 22;
			case 83:
				return 23;
			case 62:
			case 66:
				return 24;
			case 2:
			case 190:
			case 191:
			case 192:
			case 193:
			case 194:
			case 317:
			case 318:
				return 25;
			case 177:
				return 26;
			case 6:
				return 27;
			case 84:
				return 28;
			case 161:
			case 431:
				return 29;
			case 181:
				return 30;
			case 182:
				return 31;
			case 224:
				return 32;
			case 226:
				return 33;
			case 162:
				return 34;
			case 259:
			case 260:
			case 261:
				return 35;
			case 256:
				return 36;
			case 122:
				return 37;
			case 27:
				return 38;
			case 29:
			case 30:
				return 39;
			case 26:
				return 40;
			case 73:
				return 41;
			case 28:
				return 42;
			case 55:
			case 230:
				return 43;
			case 48:
				return 44;
			case 60:
				return 45;
			case 174:
				return 46;
			case 42:
			case 231:
			case 232:
			case 233:
			case 234:
			case 235:
				return 47;
			case 169:
				return 48;
			case 206:
				return 49;
			case 24:
			case 25:
				return 50;
			case 63:
				return 51;
			case 236:
			case 237:
				return 52;
			case 198:
			case 199:
				return 53;
			case 43:
				return 54;
			case 23:
				return 55;
			case 205:
				return 56;
			case 78:
				return 57;
			case 258:
				return 58;
			case 252:
				return 59;
			case 170:
			case 171:
			case 180:
				return 60;
			case 58:
				return 61;
			case 212:
				return 62;
			case 75:
				return 63;
			case 223:
				return 64;
			case 253:
				return 65;
			case 65:
				return 66;
			case 21:
			case 201:
			case 202:
			case 203:
			case 449:
			case 450:
			case 451:
			case 452:
				return 67;
			case 32:
			case 33:
				return 68;
			case 1:
			case 302:
			case 333:
			case 334:
			case 335:
			case 336:
				return 69;
			case 185:
				return 70;
			case 164:
			case 165:
				return 71;
			case 254:
			case 255:
				return 72;
			case 166:
				return 73;
			case 153:
				return 74;
			case 141:
				return 75;
			case 225:
				return 76;
			case 86:
				return 77;
			case 158:
			case 159:
				return 78;
			case 61:
				return 79;
			case 195:
			case 196:
				return 80;
			case 104:
				return 81;
			case 155:
				return 82;
			case 98:
			case 99:
			case 100:
				return 83;
			case 10:
			case 11:
			case 12:
			case 95:
			case 96:
			case 97:
				return 84;
			case 82:
				return 85;
			case 87:
			case 88:
			case 89:
			case 90:
			case 91:
			case 92:
				return 86;
			case 3:
			case 132:
			case 186:
			case 187:
			case 188:
			case 189:
			case 200:
			case 319:
			case 320:
			case 321:
			case 331:
			case 332:
			case 430:
			case 432:
			case 433:
			case 434:
			case 435:
			case 436:
			case 590:
			case 591:
			case 632:
				return 87;
			case 175:
				return 88;
			case 197:
				return 89;
			case 273:
			case 274:
			case 275:
			case 276:
				return 91;
			case 379:
				return 92;
			case 438:
				return 93;
			case 287:
				return 95;
			case 101:
				return 96;
			case 217:
				return 97;
			case 168:
				return 98;
			case -1:
			case 81:
				return 99;
			case 94:
			case 112:
				return 100;
			case 183:
				return 101;
			case 34:
				return 102;
			case 218:
				return 103;
			case 7:
			case 8:
			case 9:
				return 104;
			case 285:
			case 286:
				return 105;
			case 52:
				return 106;
			case 71:
				return 107;
			case 288:
				return 108;
			case 350:
				return 109;
			case 347:
				return 110;
			case 251:
				return 111;
			case 352:
				return 112;
			case 316:
				return 113;
			case 93:
				return 114;
			case 289:
				return 115;
			case 152:
				return 116;
			case 342:
				return 117;
			case 111:
				return 118;
			case 315:
				return 120;
			case 277:
			case 278:
			case 279:
			case 280:
				return 121;
			case 329:
				return 122;
			case 304:
				return 123;
			case 150:
				return 124;
			case 243:
				return 125;
			case 147:
				return 126;
			case 268:
				return 127;
			case 137:
				return 128;
			case 138:
				return 129;
			case 51:
				return 130;
			case 351:
				return 132;
			case 219:
				return 133;
			case 151:
				return 134;
			case 59:
				return 135;
			case 381:
				return 136;
			case 388:
				return 137;
			case 386:
				return 138;
			case 389:
				return 139;
			case 385:
				return 140;
			case 383:
			case 384:
				return 141;
			case 382:
				return 142;
			case 390:
				return 143;
			case 387:
				return 144;
			case 144:
				return 145;
			case -5:
			case 16:
				return 146;
			case 283:
			case 284:
				return 147;
			case 348:
			case 349:
				return 148;
			case 290:
				return 149;
			case 148:
			case 149:
				return 150;
			case -4:
				return 151;
			case 330:
				return 152;
			case 140:
				return 153;
			case 341:
				return 154;
			case 281:
			case 282:
				return 156;
			case 244:
				return 157;
			case 301:
				return 158;
			case 172:
				return 160;
			case 269:
			case 270:
			case 271:
			case 272:
				return 161;
			case 305:
			case 306:
			case 307:
			case 308:
			case 309:
			case 310:
			case 311:
			case 312:
			case 313:
			case 314:
				return 162;
			case 391:
				return 163;
			case 110:
				return 164;
			case 293:
				return 165;
			case 291:
				return 166;
			case -2:
			case 121:
				return 167;
			case 56:
				return 168;
			case 145:
				return 169;
			case 143:
				return 170;
			case 184:
				return 171;
			case 204:
				return 172;
			case 326:
				return 173;
			case 221:
				return 174;
			case 292:
				return 175;
			case 53:
				return 176;
			case 45:
			case 665:
				return 177;
			case 44:
				return 178;
			case 167:
				return 179;
			case 380:
				return 180;
			case 343:
				return 184;
			case 338:
			case 339:
			case 340:
				return 185;
			case -6:
				return 90;
			case -3:
				return 119;
			case -10:
				return 131;
			case -7:
				return 155;
			case -8:
				return 159;
			case -9:
				return 183;
			case 471:
			case 472:
				return 186;
			case 498:
			case 499:
			case 500:
			case 501:
			case 502:
			case 503:
			case 504:
			case 505:
			case 506:
				return 187;
			case 496:
			case 497:
				return 188;
			case 494:
			case 495:
				return 189;
			case 462:
				return 190;
			case 461:
				return 191;
			case 468:
				return 192;
			case 477:
			case 478:
			case 479:
				return 193;
			case 469:
				return 195;
			case 460:
				return 196;
			case 466:
				return 197;
			case 467:
				return 198;
			case 463:
				return 199;
			case 480:
				return 201;
			case 481:
				return 202;
			case 483:
				return 203;
			case 482:
				return 204;
			case 489:
				return 205;
			case 490:
				return 206;
			case 513:
			case 514:
			case 515:
				return 207;
			case 510:
			case 511:
			case 512:
				return 208;
			case 509:
			case 581:
				return 209;
			case 508:
			case 580:
				return 210;
			case 524:
			case 525:
			case 526:
			case 527:
				return 211;
			case 528:
			case 529:
				return 212;
			case 533:
				return 213;
			case 532:
				return 214;
			case 530:
			case 531:
				return 215;
			case 411:
				return 216;
			case 402:
			case 403:
			case 404:
				return 217;
			case 407:
			case 408:
				return 218;
			case 409:
			case 410:
				return 219;
			case 406:
				return 220;
			case 405:
				return 221;
			case 418:
				return 222;
			case 417:
				return 223;
			case 412:
			case 413:
			case 414:
				return 224;
			case 416:
			case 518:
				return 225;
			case 415:
			case 516:
				return 226;
			case 419:
				return 227;
			case 424:
				return 228;
			case 421:
				return 229;
			case 420:
				return 230;
			case 423:
				return 231;
			case 428:
				return 232;
			case 426:
				return 233;
			case 427:
				return 234;
			case 429:
				return 235;
			case 425:
				return 236;
			case 216:
				return 237;
			case 214:
				return 238;
			case 213:
				return 239;
			case 215:
				return 240;
			case 520:
				return 241;
			case 156:
				return 242;
			case 64:
				return 243;
			case 103:
				return 244;
			case 79:
				return 245;
			case 80:
				return 246;
			case 31:
			case 294:
			case 295:
			case 296:
				return 247;
			case 154:
				return 248;
			case 537:
				return 249;
			case 220:
				return 250;
			case 541:
				return 251;
			case 542:
				return 252;
			case 543:
				return 253;
			case 544:
				return 254;
			case 545:
				return 255;
			case 546:
				return 256;
			case 555:
			case 556:
			case 557:
				return 257;
			case 552:
			case 553:
			case 554:
				return 258;
			case 566:
			case 567:
				return 259;
			case 570:
			case 571:
				return 260;
			case 574:
			case 575:
				return 261;
			case 572:
			case 573:
				return 262;
			case 568:
			case 569:
				return 263;
			case 558:
			case 559:
			case 560:
				return 264;
			case 561:
			case 562:
			case 563:
				return 265;
			case 578:
				return 266;
			case 536:
				return 267;
			case 586:
				return 268;
			case 587:
				return 269;
			case 619:
				return 270;
			case 621:
			case 622:
			case 623:
				return 271;
			case 620:
				return 272;
			case 618:
				return 273;
			case 628:
				return 274;
			case 624:
				return 275;
			case 631:
				return 276;
			case 630:
				return 277;
			case 635:
				return 278;
			case 634:
				return 279;
			case 582:
				return 280;
			case 464:
				return 281;
			case 465:
				return 282;
			case 470:
				return 283;
			case 473:
				return 284;
			case 474:
				return 285;
			case 475:
				return 286;
			case 176:
				return 287;
			case 133:
				return 288;
			case 676:
				return 289;
			default:
				return 0;
			}
		}

		public static int BannerToNPC(int i)
		{
			return i switch
			{
				1 => 102, 
				2 => 250, 
				3 => 257, 
				4 => 69, 
				5 => 157, 
				6 => 77, 
				7 => 49, 
				8 => 74, 
				9 => 163, 
				10 => 241, 
				11 => 242, 
				12 => 239, 
				13 => 39, 
				14 => 46, 
				15 => 120, 
				16 => 85, 
				17 => 109, 
				18 => 47, 
				19 => 57, 
				20 => 67, 
				21 => 173, 
				22 => 179, 
				23 => 83, 
				24 => 62, 
				25 => 2, 
				26 => 177, 
				27 => 6, 
				28 => 84, 
				29 => 161, 
				30 => 181, 
				31 => 182, 
				32 => 224, 
				33 => 226, 
				34 => 162, 
				35 => 259, 
				36 => 256, 
				37 => 122, 
				38 => 27, 
				39 => 29, 
				40 => 26, 
				41 => 73, 
				42 => 28, 
				43 => 55, 
				44 => 48, 
				45 => 60, 
				46 => 174, 
				47 => 42, 
				48 => 169, 
				49 => 206, 
				50 => 24, 
				51 => 63, 
				52 => 236, 
				53 => 199, 
				54 => 43, 
				55 => 23, 
				56 => 205, 
				57 => 78, 
				58 => 258, 
				59 => 252, 
				60 => 170, 
				61 => 58, 
				62 => 212, 
				63 => 75, 
				64 => 223, 
				65 => 253, 
				66 => 65, 
				67 => 21, 
				68 => 32, 
				69 => 1, 
				70 => 185, 
				71 => 164, 
				72 => 254, 
				73 => 166, 
				74 => 153, 
				75 => 141, 
				76 => 225, 
				77 => 86, 
				78 => 158, 
				79 => 61, 
				80 => 196, 
				81 => 104, 
				82 => 155, 
				83 => 98, 
				84 => 10, 
				85 => 82, 
				86 => 87, 
				87 => 3, 
				88 => 175, 
				89 => 197, 
				91 => 273, 
				92 => 379, 
				93 => 438, 
				95 => 287, 
				96 => 101, 
				97 => 217, 
				98 => 168, 
				99 => 81, 
				100 => 94, 
				101 => 183, 
				102 => 34, 
				103 => 218, 
				104 => 7, 
				105 => 285, 
				106 => 52, 
				107 => 71, 
				108 => 288, 
				109 => 350, 
				110 => 347, 
				111 => 251, 
				112 => 352, 
				113 => 316, 
				114 => 93, 
				115 => 289, 
				116 => 152, 
				117 => 342, 
				118 => 111, 
				120 => 315, 
				121 => 277, 
				122 => 329, 
				123 => 304, 
				124 => 150, 
				125 => 243, 
				126 => 147, 
				127 => 268, 
				128 => 137, 
				129 => 138, 
				130 => 51, 
				132 => 351, 
				133 => 219, 
				134 => 151, 
				135 => 59, 
				136 => 381, 
				137 => 388, 
				138 => 386, 
				139 => 389, 
				140 => 385, 
				141 => 383, 
				142 => 382, 
				143 => 390, 
				144 => 387, 
				145 => 144, 
				146 => 16, 
				147 => 283, 
				148 => 348, 
				149 => 290, 
				150 => 148, 
				151 => -4, 
				152 => 330, 
				153 => 140, 
				154 => 341, 
				156 => 281, 
				157 => 244, 
				158 => 301, 
				160 => 172, 
				161 => 269, 
				162 => 305, 
				163 => 391, 
				164 => 110, 
				165 => 293, 
				166 => 291, 
				167 => 121, 
				168 => 56, 
				169 => 145, 
				170 => 143, 
				171 => 184, 
				172 => 204, 
				173 => 326, 
				174 => 221, 
				175 => 292, 
				176 => 53, 
				177 => 45, 
				178 => 44, 
				179 => 167, 
				180 => 380, 
				184 => 343, 
				185 => 338, 
				90 => -6, 
				119 => -3, 
				131 => -10, 
				155 => -7, 
				159 => -8, 
				183 => -9, 
				186 => 471, 
				187 => 498, 
				188 => 496, 
				189 => 494, 
				190 => 462, 
				191 => 461, 
				192 => 468, 
				193 => 477, 
				195 => 469, 
				196 => 460, 
				197 => 466, 
				198 => 467, 
				199 => 463, 
				201 => 480, 
				202 => 481, 
				203 => 483, 
				204 => 482, 
				205 => 489, 
				206 => 490, 
				207 => 513, 
				208 => 510, 
				209 => 581, 
				210 => 580, 
				211 => 524, 
				212 => 529, 
				213 => 533, 
				214 => 532, 
				215 => 530, 
				216 => 411, 
				217 => 402, 
				218 => 407, 
				219 => 409, 
				220 => 406, 
				221 => 405, 
				222 => 418, 
				223 => 417, 
				224 => 412, 
				225 => 416, 
				226 => 415, 
				227 => 419, 
				228 => 424, 
				229 => 421, 
				230 => 420, 
				231 => 423, 
				232 => 428, 
				233 => 426, 
				234 => 427, 
				235 => 429, 
				236 => 425, 
				237 => 216, 
				238 => 214, 
				239 => 213, 
				240 => 215, 
				241 => 520, 
				242 => 156, 
				243 => 64, 
				244 => 103, 
				245 => 79, 
				246 => 80, 
				247 => 31, 
				248 => 154, 
				249 => 537, 
				250 => 220, 
				251 => 541, 
				252 => 542, 
				253 => 543, 
				254 => 544, 
				255 => 545, 
				256 => 546, 
				257 => 555, 
				258 => 552, 
				259 => 566, 
				260 => 570, 
				261 => 574, 
				262 => 572, 
				263 => 568, 
				264 => 558, 
				265 => 561, 
				266 => 578, 
				267 => 536, 
				268 => 586, 
				269 => 587, 
				270 => 619, 
				271 => 621, 
				272 => 620, 
				273 => 618, 
				274 => 628, 
				275 => 624, 
				276 => 631, 
				277 => 630, 
				278 => 635, 
				279 => 634, 
				280 => 582, 
				281 => 464, 
				282 => 465, 
				283 => 470, 
				284 => 473, 
				285 => 474, 
				286 => 475, 
				287 => 176, 
				288 => 133, 
				289 => 676, 
				_ => 0, 
			};
		}

		public bool FitsAmmoSlot()
		{
			if ((type != 0 && ammo <= 0 && bait <= 0 && type != 530 && type != 849 && !PaintOrCoating) || notAmmo)
			{
				return type == 353;
			}
			return true;
		}

		public bool CanFillEmptyAmmoSlot()
		{
			if (bait <= 0 && !PaintOrCoating && type != 353 && type != 849 && type != 169 && type != 75 && type != 23 && type != 408 && type != 370 && type != 1246)
			{
				return !notAmmo;
			}
			return false;
		}

		public void SetDefaults1(int type)
		{
			switch (type)
			{
			case 1:
				useStyle = 1;
				useTurn = true;
				useAnimation = 20;
				useTime = 13;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 5;
				pick = 40;
				UseSound = SoundID.Item1;
				knockBack = 2f;
				value = 2000;
				melee = true;
				break;
			case 2:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 0;
				width = 12;
				height = 12;
				break;
			case 3:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 1;
				width = 12;
				height = 12;
				break;
			case 4:
				useStyle = 1;
				useTurn = false;
				useAnimation = 20;
				useTime = 20;
				width = 24;
				height = 28;
				damage = 12;
				knockBack = 5.5f;
				UseSound = SoundID.Item1;
				scale = 1.15f;
				value = 1800;
				melee = true;
				break;
			case 5:
				useStyle = 2;
				UseSound = SoundID.Item2;
				useTurn = false;
				useAnimation = 17;
				useTime = 17;
				width = 16;
				height = 18;
				healLife = 15;
				maxStack = CommonMaxStack;
				consumable = true;
				potion = true;
				value = sellPrice(0, 0, 2, 50);
				break;
			case 6:
				autoReuse = false;
				useStyle = 13;
				useAnimation = 12;
				useTime = 12;
				width = 50;
				height = 18;
				shoot = 940;
				UseSound = SoundID.Item1;
				damage = 8;
				knockBack = 4f;
				shootSpeed = 2.1f;
				noMelee = true;
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 2, 80));
				melee = true;
				noUseGraphic = true;
				break;
			case 7:
				autoReuse = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 30;
				useTime = 20;
				hammer = 40;
				width = 24;
				height = 28;
				damage = 7;
				knockBack = 5.5f;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				value = 1600;
				melee = true;
				break;
			case 8:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				width = 10;
				height = 12;
				value = 50;
				break;
			case 9:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 30;
				width = 8;
				height = 10;
				break;
			case 10:
				useStyle = 1;
				useTurn = true;
				useAnimation = 27;
				knockBack = 4.5f;
				useTime = 19;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 5;
				axe = 9;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				value = 1600;
				melee = true;
				break;
			case 11:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 6;
				width = 12;
				height = 12;
				value = 500;
				break;
			case 12:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 7;
				width = 12;
				height = 12;
				value = 250;
				break;
			case 13:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 8;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 3);
				break;
			case 14:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 9;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 1, 50);
				break;
			case 15:
				width = 24;
				height = 28;
				accessory = true;
				value = 1000;
				waistSlot = 2;
				break;
			case 16:
				width = 24;
				height = 28;
				accessory = true;
				value = 5000;
				waistSlot = 7;
				break;
			case 17:
				width = 24;
				height = 28;
				accessory = true;
				rare = 1;
				value = 10000;
				waistSlot = 3;
				break;
			case 18:
				width = 24;
				height = 18;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 0, 25);
				break;
			case 19:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 6000;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 6;
				break;
			case 20:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 750;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 0;
				break;
			case 21:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 3000;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 4;
				break;
			case 22:
				color = new Color(160, 145, 130, 110);
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 1500;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 2;
				break;
			case 23:
				width = 10;
				height = 12;
				maxStack = CommonMaxStack;
				alpha = 175;
				ammo = AmmoID.Gel;
				color = new Color(0, 80, 255, 100);
				value = 5;
				consumable = true;
				break;
			case 24:
				useStyle = 1;
				useTurn = false;
				useTime = 20;
				useAnimation = 20;
				width = 24;
				height = 28;
				damage = 7;
				knockBack = 5f;
				scale = 1f;
				UseSound = SoundID.Item1;
				value = 100;
				melee = true;
				break;
			case 25:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 26:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 1;
				width = 12;
				height = 12;
				break;
			case 27:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				autoReuse = true;
				createTile = 20;
				width = 18;
				height = 18;
				value = 10;
				break;
			case 28:
				UseSound = SoundID.Item3;
				healLife = 50;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				potion = true;
				value = 300;
				break;
			case 29:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 18;
				height = 18;
				useStyle = 4;
				useTime = 30;
				UseSound = SoundID.Item4;
				useAnimation = 30;
				rare = 2;
				value = 75000;
				break;
			case 30:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 16;
				width = 12;
				height = 12;
				break;
			case 31:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 13;
				width = 16;
				height = 24;
				value = 20;
				break;
			case 32:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				width = 26;
				height = 20;
				value = 300;
				break;
			case 33:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 17;
				width = 26;
				height = 24;
				value = 300;
				break;
			case 34:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 35:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 16;
				width = 28;
				height = 14;
				value = 5000;
				break;
			case 36:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 37:
				width = 28;
				height = 12;
				defense = 1;
				headSlot = 10;
				value = 1000;
				break;
			case 38:
				width = 12;
				height = 20;
				maxStack = CommonMaxStack;
				value = 500;
				break;
			case 39:
				useStyle = 5;
				useAnimation = 30;
				useTime = 30;
				width = 12;
				height = 28;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 4;
				shootSpeed = 6.1f;
				noMelee = true;
				value = 100;
				ranged = true;
				break;
			case 40:
				shootSpeed = 3f;
				shoot = 1;
				damage = 5;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 2f;
				value = 5;
				ranged = true;
				break;
			case 41:
				shootSpeed = 3.5f;
				shoot = 2;
				damage = 7;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 2f;
				value = 10;
				ranged = true;
				break;
			case 42:
				useStyle = 1;
				shootSpeed = 9f;
				shoot = 3;
				damage = 10;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 15;
				ranged = true;
				break;
			case 43:
				useStyle = 4;
				width = 22;
				height = 14;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				maxStack = CommonMaxStack;
				rare = 1;
				break;
			case 44:
				useStyle = 5;
				useAnimation = 25;
				useTime = 25;
				width = 12;
				height = 28;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 14;
				shootSpeed = 6.7f;
				knockBack = 1f;
				alpha = 30;
				rare = 1;
				noMelee = true;
				value = 18000;
				ranged = true;
				break;
			case 45:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 30;
				knockBack = 6f;
				useTime = 15;
				width = 24;
				height = 28;
				damage = 20;
				axe = 15;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = 13500;
				melee = true;
				break;
			case 46:
				useStyle = 1;
				useAnimation = 20;
				useTime = 20;
				knockBack = 5f;
				width = 24;
				height = 28;
				damage = 16;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = 13500;
				melee = true;
				shoot = 974;
				glowMask = 328;
				break;
			case 47:
				shootSpeed = 3.4f;
				shoot = 4;
				damage = 12;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 3f;
				alpha = 30;
				rare = 1;
				value = 40;
				ranged = true;
				break;
			case 48:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				width = 26;
				height = 22;
				value = 500;
				break;
			case 49:
				width = 22;
				height = 22;
				accessory = true;
				lifeRegen = 2;
				rare = 1;
				value = 50000;
				handOnSlot = 2;
				break;
			case 50:
				useTurn = true;
				width = 20;
				height = 20;
				useStyle = 4;
				useTime = 90;
				UseSound = SoundID.Item6;
				useAnimation = 90;
				rare = 1;
				value = 50000;
				break;
			case 51:
				shootSpeed = 0.5f;
				shoot = 5;
				damage = 10;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 4f;
				rare = 1;
				value = 100;
				ranged = true;
				break;
			case 52:
				type = 52;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 1;
				break;
			case 53:
				width = 16;
				height = 24;
				accessory = true;
				rare = 1;
				value = 50000;
				waistSlot = 1;
				break;
			case 54:
				width = 28;
				height = 24;
				accessory = true;
				rare = 1;
				value = 50000;
				shoeSlot = 6;
				break;
			case 55:
				noMelee = true;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 6;
				damage = 17;
				knockBack = 8f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				rare = 1;
				value = 50000;
				melee = true;
				break;
			case 56:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 22;
				width = 12;
				height = 12;
				rare = 1;
				value = sellPrice(0, 0, 10);
				break;
			case 57:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 1;
				value = sellPrice(0, 0, 30);
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 8;
				break;
			case 58:
				width = 12;
				height = 12;
				break;
			case 59:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 23;
				width = 14;
				height = 14;
				value = 500;
				autoReuse = true;
				break;
			case 60:
				width = 16;
				height = 18;
				maxStack = CommonMaxStack;
				value = 50;
				break;
			case 61:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 25;
				width = 12;
				height = 12;
				break;
			case 62:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 2;
				width = 14;
				height = 14;
				value = 20;
				autoReuse = true;
				break;
			case 63:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				autoReuse = true;
				createTile = 27;
				width = 26;
				height = 26;
				value = buyPrice(0, 0, 50);
				break;
			case 64:
				mana = 10;
				damage = 10;
				useStyle = 1;
				shootSpeed = 32f;
				shoot = 7;
				width = 26;
				height = 28;
				useAnimation = 28;
				useTime = 28;
				rare = 1;
				noMelee = true;
				knockBack = 1f;
				value = shadowOrbPrice;
				magic = true;
				break;
			case 65:
				knockBack = 5f;
				alpha = 100;
				color = new Color(150, 150, 150, 0);
				damage = 22;
				useStyle = 1;
				scale = 1.25f;
				shootSpeed = 25f;
				shoot = 9;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 40;
				rare = 2;
				value = 50000;
				melee = true;
				break;
			case 66:
				useStyle = 1;
				shootSpeed = 4f;
				shoot = 10;
				width = 16;
				height = 24;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noMelee = true;
				value = 75;
				break;
			case 67:
				damage = 0;
				useStyle = 1;
				shootSpeed = 4f;
				shoot = 11;
				width = 16;
				height = 24;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noMelee = true;
				value = 100;
				break;
			case 68:
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10;
				break;
			case 69:
				width = 8;
				height = 20;
				maxStack = CommonMaxStack;
				value = 100;
				break;
			case 70:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				rare = 1;
				break;
			case 71:
				width = 10;
				height = 10;
				maxStack = 100;
				value = 5;
				ammo = AmmoID.Coin;
				shoot = 158;
				notAmmo = true;
				damage = 25;
				shootSpeed = 1f;
				ranged = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 330;
				noMelee = true;
				break;
			case 72:
				width = 10;
				height = 12;
				maxStack = 100;
				value = 500;
				ammo = AmmoID.Coin;
				notAmmo = true;
				damage = 50;
				shoot = 159;
				shootSpeed = 2f;
				ranged = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 331;
				noMelee = true;
				break;
			case 73:
				width = 10;
				height = 14;
				maxStack = 100;
				value = 50000;
				ammo = AmmoID.Coin;
				notAmmo = true;
				damage = 100;
				shoot = 160;
				shootSpeed = 3f;
				ranged = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 332;
				noMelee = true;
				break;
			case 74:
				width = 12;
				height = 14;
				maxStack = CommonMaxStack;
				value = 5000000;
				ammo = AmmoID.Coin;
				notAmmo = true;
				damage = 200;
				shoot = 161;
				shootSpeed = 4f;
				ranged = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 333;
				noMelee = true;
				break;
			case 75:
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				alpha = 75;
				ammo = AmmoID.FallenStar;
				value = sellPrice(0, 0, 5);
				useStyle = 4;
				UseSound = SoundID.Item4;
				useTurn = false;
				useAnimation = 17;
				useTime = 17;
				consumable = true;
				rare = 1;
				break;
			case 76:
				width = 18;
				height = 18;
				defense = 1;
				legSlot = 1;
				value = 1000;
				break;
			case 77:
				width = 18;
				height = 18;
				defense = 2;
				legSlot = 2;
				value = 4000;
				break;
			case 78:
				width = 18;
				height = 18;
				defense = 3;
				legSlot = 3;
				value = 10000;
				break;
			case 79:
				width = 18;
				height = 18;
				defense = 4;
				legSlot = 4;
				value = 20000;
				break;
			case 80:
				width = 18;
				height = 18;
				defense = 2;
				bodySlot = 1;
				value = 1250;
				break;
			case 81:
				width = 18;
				height = 18;
				defense = 3;
				bodySlot = 2;
				value = 5000;
				break;
			case 82:
				width = 18;
				height = 18;
				defense = 4;
				bodySlot = 3;
				value = 12500;
				break;
			case 83:
				width = 18;
				height = 18;
				defense = 5;
				bodySlot = 4;
				value = 25000;
				break;
			case 84:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 11.5f;
				shoot = 13;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = 20000;
				break;
			case 85:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 8;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 214;
				width = 12;
				height = 12;
				value = 200;
				tileBoost += 3;
				break;
			case 86:
				width = 14;
				height = 18;
				maxStack = CommonMaxStack;
				rare = 1;
				value = 500;
				break;
			case 87:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 29;
				width = 20;
				height = 12;
				value = 10000;
				break;
			case 88:
				width = 22;
				height = 16;
				defense = 2;
				headSlot = 11;
				rare = 1;
				value = buyPrice(0, 4);
				break;
			case 89:
				width = 18;
				height = 18;
				defense = 1;
				headSlot = 1;
				value = 750;
				break;
			case 90:
				width = 18;
				height = 18;
				defense = 2;
				headSlot = 2;
				value = 3000;
				break;
			case 91:
				width = 18;
				height = 18;
				defense = 3;
				headSlot = 3;
				value = 7500;
				break;
			case 92:
				width = 18;
				height = 18;
				defense = 4;
				headSlot = 4;
				value = 15000;
				break;
			case 93:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 4;
				width = 12;
				height = 12;
				break;
			case 94:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				width = 8;
				height = 10;
				break;
			case 95:
				useStyle = 5;
				useAnimation = 16;
				useTime = 16;
				width = 24;
				height = 28;
				shoot = 14;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item11;
				damage = 13;
				shootSpeed = 5f;
				noMelee = true;
				value = 50000;
				knockBack = 1f;
				scale = 0.9f;
				rare = 1;
				ranged = true;
				break;
			case 96:
				useStyle = 5;
				autoReuse = false;
				useAnimation = 32;
				useTime = 32;
				width = 44;
				height = 14;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item11;
				damage = 31;
				shootSpeed = 9f;
				noMelee = true;
				value = shadowOrbPrice;
				knockBack = 5.25f;
				rare = 1;
				ranged = true;
				crit = 7;
				break;
			case 97:
				shootSpeed = 4f;
				shoot = 14;
				damage = 7;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 2f;
				value = 7;
				ranged = true;
				break;
			case 98:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 8;
				useTime = 8;
				width = 50;
				height = 18;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item11;
				damage = 6;
				shootSpeed = 7f;
				noMelee = true;
				value = 350000;
				rare = 2;
				ranged = true;
				break;
			case 99:
				useStyle = 5;
				useAnimation = 28;
				useTime = 28;
				width = 12;
				height = 28;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 8;
				shootSpeed = 6.6f;
				noMelee = true;
				value = 1400;
				ranged = true;
				break;
			case 100:
				width = 18;
				height = 18;
				defense = 6;
				legSlot = 5;
				rare = 1;
				value = 22500;
				break;
			case 101:
				width = 18;
				height = 18;
				defense = 7;
				bodySlot = 5;
				rare = 1;
				value = 30000;
				break;
			case 102:
				width = 18;
				height = 18;
				defense = 6;
				headSlot = 5;
				rare = 1;
				value = 37500;
				break;
			case 103:
				useStyle = 1;
				useTurn = true;
				useAnimation = 20;
				useTime = 15;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 9;
				pick = 65;
				UseSound = SoundID.Item1;
				knockBack = 3f;
				rare = 1;
				value = 18000;
				scale = 1.15f;
				melee = true;
				break;
			case 104:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 45;
				useTime = 19;
				hammer = 55;
				width = 24;
				height = 28;
				damage = 24;
				knockBack = 6f;
				scale = 1.3f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = 15000;
				melee = true;
				break;
			case 105:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 33;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 3);
				holdStyle = 1;
				break;
			case 106:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				width = 26;
				height = 26;
				value = 3000;
				break;
			case 107:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				placeStyle = 1;
				width = 26;
				height = 26;
				value = 12000;
				break;
			case 108:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				placeStyle = 2;
				width = 26;
				height = 26;
				value = 24000;
				break;
			case 109:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 18;
				height = 18;
				useStyle = 4;
				useTime = 30;
				UseSound = SoundID.Item29;
				useAnimation = 30;
				rare = 2;
				value = sellPrice(0, 0, 25);
				break;
			case 110:
				UseSound = SoundID.Item3;
				healMana = 50;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				value = buyPrice(0, 0, 1);
				break;
			case 111:
				width = 22;
				height = 22;
				accessory = true;
				rare = 1;
				value = shadowOrbPrice;
				handOnSlot = 3;
				break;
			case 112:
				mana = 12;
				damage = 48;
				useStyle = 1;
				shootSpeed = 7.5f;
				shoot = 15;
				width = 26;
				height = 28;
				UseSound = SoundID.Item20;
				useAnimation = 16;
				useTime = 16;
				rare = 3;
				noMelee = true;
				knockBack = 5.5f;
				value = hellPrice;
				magic = true;
				if (Variant == ItemVariants.StrongerVariant)
				{
					rare = 6;
					value = 500000;
					damage = 85;
					useAnimation = 12;
					useTime = 12;
				}
				break;
			case 113:
				mana = 14;
				channel = true;
				damage = 35;
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 16;
				width = 26;
				height = 28;
				UseSound = SoundID.Item9;
				useAnimation = 22;
				useTime = 22;
				rare = 2;
				noMelee = true;
				knockBack = 7.5f;
				value = dungeonPrice;
				magic = true;
				break;
			case 114:
				channel = true;
				knockBack = 5f;
				useStyle = 1;
				shoot = 17;
				width = 26;
				height = 28;
				UseSound = SoundID.Item8;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = buyPrice(0, 5);
				break;
			case 115:
				channel = true;
				damage = 0;
				useStyle = 4;
				shoot = 18;
				width = 24;
				height = 24;
				UseSound = SoundID.Item8;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = shadowOrbPrice;
				buffType = 19;
				break;
			case 116:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 37;
				width = 12;
				height = 12;
				value = 1000;
				break;
			case 117:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 1;
				value = 7000;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 9;
				break;
			case 118:
				maxStack = CommonMaxStack;
				width = 18;
				height = 18;
				value = 1000;
				break;
			case 119:
				noMelee = true;
				useStyle = 1;
				shootSpeed = 14f;
				shoot = 19;
				damage = 49;
				knockBack = 8f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				rare = 3;
				value = 100000;
				melee = true;
				break;
			case 120:
				useStyle = 5;
				useAnimation = 22;
				useTime = 22;
				width = 14;
				height = 32;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 31;
				shootSpeed = 8f;
				knockBack = 2f;
				alpha = 30;
				rare = 3;
				noMelee = true;
				scale = 1.1f;
				value = 27000;
				ranged = true;
				break;
			case 121:
				useStyle = 1;
				useTime = 40;
				useAnimation = 40;
				knockBack = 6.5f;
				width = 24;
				height = 28;
				damage = 40;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 3;
				value = 27000;
				melee = true;
				break;
			}
			switch (type)
			{
			case 122:
				useStyle = 1;
				useTurn = true;
				useAnimation = 23;
				useTime = 18;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 12;
				pick = 100;
				scale = 1.15f;
				UseSound = SoundID.Item1;
				knockBack = 2f;
				rare = 3;
				value = 27000;
				melee = true;
				break;
			case 123:
				width = 18;
				height = 18;
				defense = 5;
				headSlot = 6;
				rare = 1;
				value = 45000;
				break;
			case 124:
				width = 18;
				height = 18;
				defense = 6;
				bodySlot = 6;
				rare = 1;
				value = 30000;
				break;
			case 125:
				width = 18;
				height = 18;
				defense = 5;
				legSlot = 6;
				rare = 1;
				value = 30000;
				break;
			case 126:
				UseSound = SoundID.Item3;
				healLife = 20;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				potion = true;
				value = 20;
				break;
			case 127:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 17;
				useTime = 17;
				width = 24;
				height = 28;
				shoot = 20;
				mana = 6;
				UseSound = SoundID.Item157;
				knockBack = 0.75f;
				damage = 17;
				shootSpeed = 10f;
				noMelee = true;
				scale = 0.8f;
				rare = 1;
				magic = true;
				value = 20000;
				break;
			case 128:
				width = 28;
				height = 24;
				accessory = true;
				rare = 3;
				value = 50000;
				shoeSlot = 12;
				break;
			case 129:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 38;
				width = 12;
				height = 12;
				break;
			case 130:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 5;
				width = 12;
				height = 12;
				break;
			case 131:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 39;
				width = 12;
				height = 12;
				break;
			case 132:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 6;
				width = 12;
				height = 12;
				break;
			case 133:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 40;
				width = 12;
				height = 12;
				break;
			case 134:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 41;
				width = 12;
				height = 12;
				break;
			case 135:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 17;
				width = 12;
				height = 12;
				break;
			case 136:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				value = sellPrice(0, 0, 0, 30);
				break;
			case 137:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 43;
				width = 12;
				height = 12;
				break;
			case 138:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 18;
				width = 12;
				height = 12;
				break;
			case 139:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 44;
				width = 12;
				height = 12;
				break;
			case 140:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 19;
				width = 12;
				height = 12;
				break;
			case 141:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 45;
				width = 12;
				height = 12;
				break;
			case 142:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 10;
				width = 12;
				height = 12;
				break;
			case 143:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 46;
				width = 12;
				height = 12;
				break;
			case 144:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 11;
				width = 12;
				height = 12;
				break;
			case 145:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 47;
				width = 12;
				height = 12;
				break;
			case 146:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 12;
				width = 12;
				height = 12;
				break;
			case 147:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 48;
				width = 12;
				height = 12;
				break;
			case 148:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 49;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 1);
				holdStyle = 1;
				rare = 1;
				break;
			case 149:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 50;
				width = 24;
				height = 28;
				value = sellPrice(0, 0, 3);
				break;
			case 150:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 51;
				width = 20;
				height = 24;
				alpha = 100;
				break;
			case 151:
				width = 18;
				height = 18;
				defense = 6;
				headSlot = 7;
				rare = 2;
				value = 45000;
				break;
			case 152:
				width = 18;
				height = 18;
				defense = 7;
				bodySlot = 7;
				rare = 2;
				value = 30000;
				break;
			case 153:
				width = 18;
				height = 18;
				defense = 6;
				legSlot = 7;
				rare = 2;
				value = 30000;
				break;
			case 154:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 14;
				value = 50;
				useAnimation = 12;
				useTime = 12;
				useStyle = 1;
				UseSound = SoundID.Item1;
				shootSpeed = 8f;
				noUseGraphic = true;
				noMelee = true;
				damage = 20;
				knockBack = 2.3f;
				shoot = 21;
				ranged = true;
				break;
			case 155:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useTime = 18;
				useAnimation = 18;
				width = 40;
				height = 40;
				damage = 24;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 2;
				value = dungeonPrice;
				knockBack = 3f;
				melee = true;
				break;
			case 156:
				width = 24;
				height = 28;
				rare = 2;
				value = dungeonPrice;
				accessory = true;
				defense = 1;
				shieldSlot = 1;
				break;
			case 157:
				mana = 7;
				autoReuse = true;
				useStyle = 5;
				useAnimation = 16;
				useTime = 8;
				knockBack = 7f;
				width = 38;
				height = 10;
				damage = 27;
				scale = 1f;
				shoot = 22;
				shootSpeed = 12.5f;
				UseSound = SoundID.Item13;
				noMelee = true;
				rare = 2;
				value = dungeonPrice;
				magic = true;
				if (Variant == ItemVariants.StrongerVariant)
				{
					value = sellPrice(0, 5);
					rare = 8;
					damage = 90;
					useAnimation = 10;
					useTime = 5;
				}
				break;
			case 158:
				width = 20;
				height = 22;
				rare = 1;
				value = 27000;
				accessory = true;
				break;
			case 159:
				width = 14;
				height = 28;
				rare = 1;
				value = sellPrice(0, 1, 50);
				accessory = true;
				balloonSlot = 8;
				break;
			case 160:
				autoReuse = true;
				noMelee = true;
				useStyle = 5;
				useAnimation = 30;
				useTime = 30;
				knockBack = 6f;
				width = 30;
				height = 10;
				damage = 25;
				scale = 1.1f;
				shoot = 23;
				shootSpeed = 11f;
				UseSound = SoundID.Item10;
				rare = 2;
				value = 27000;
				ranged = true;
				break;
			case 161:
				useStyle = 1;
				shootSpeed = 5f;
				shoot = 24;
				knockBack = 1f;
				damage = 16;
				width = 10;
				height = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 80;
				ranged = true;
				break;
			case 162:
				useStyle = 5;
				useAnimation = 45;
				useTime = 45;
				knockBack = 5.5f;
				width = 30;
				height = 10;
				damage = 15;
				scale = 1.1f;
				noUseGraphic = true;
				shoot = 25;
				shootSpeed = 12f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = shadowOrbPrice;
				melee = true;
				channel = true;
				noMelee = true;
				break;
			case 163:
				noMelee = true;
				useStyle = 5;
				useAnimation = 45;
				useTime = 45;
				knockBack = 6f;
				width = 30;
				height = 10;
				damage = 27;
				scale = 1.1f;
				noUseGraphic = true;
				shoot = 26;
				shootSpeed = 12f;
				UseSound = SoundID.Item1;
				rare = 2;
				value = dungeonPrice;
				melee = true;
				channel = true;
				break;
			case 164:
				autoReuse = false;
				useStyle = 5;
				useAnimation = 15;
				useTime = 15;
				width = 24;
				height = 24;
				shoot = 14;
				knockBack = 3f;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item41;
				damage = 26;
				shootSpeed = 10f;
				noMelee = true;
				value = dungeonPrice;
				scale = 0.85f;
				rare = 2;
				ranged = true;
				break;
			case 165:
				autoReuse = true;
				rare = 2;
				mana = 10;
				UseSound = SoundID.Item21;
				noMelee = true;
				useStyle = 5;
				damage = 19;
				useAnimation = 17;
				useTime = 17;
				width = 24;
				height = 28;
				shoot = 27;
				scale = 0.9f;
				shootSpeed = 4.5f;
				knockBack = 5f;
				magic = true;
				value = sellPrice(0, 1, 50);
				break;
			case 166:
				useStyle = 1;
				shootSpeed = 5f;
				shoot = 28;
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 25;
				useTime = 25;
				noUseGraphic = true;
				noMelee = true;
				value = buyPrice(0, 0, 3);
				damage = 0;
				break;
			case 167:
				useStyle = 1;
				shootSpeed = 4f;
				shoot = 29;
				width = 8;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 40;
				useTime = 40;
				noUseGraphic = true;
				noMelee = true;
				value = buyPrice(0, 0, 20);
				rare = 1;
				break;
			case 168:
				useStyle = 5;
				shootSpeed = 5.5f;
				shoot = 30;
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 45;
				useTime = 45;
				noUseGraphic = true;
				noMelee = true;
				value = 75;
				damage = 60;
				knockBack = 8f;
				ranged = true;
				break;
			case 169:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 53;
				width = 12;
				height = 12;
				ammo = AmmoID.Sand;
				notAmmo = true;
				break;
			case 170:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 54;
				width = 12;
				height = 12;
				break;
			case 171:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 55;
				width = 28;
				height = 28;
				break;
			case 172:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 57;
				width = 12;
				height = 12;
				break;
			case 173:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 56;
				width = 12;
				height = 12;
				break;
			case 174:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 58;
				width = 12;
				height = 12;
				rare = 2;
				value = sellPrice(0, 0, 2, 50);
				break;
			case 175:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 2;
				value = 20000;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 10;
				break;
			case 176:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 59;
				width = 12;
				height = 12;
				break;
			case 181:
				createTile = 178;
				placeStyle = 0;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				alpha = 50;
				width = 10;
				height = 14;
				value = 1875;
				break;
			case 180:
				createTile = 178;
				placeStyle = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				alpha = 50;
				width = 10;
				height = 14;
				value = 3750;
				break;
			case 177:
				createTile = 178;
				placeStyle = 2;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				alpha = 50;
				width = 10;
				height = 14;
				value = 5625;
				break;
			case 179:
				createTile = 178;
				placeStyle = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				alpha = 50;
				width = 10;
				height = 14;
				value = 7500;
				break;
			case 178:
				createTile = 178;
				placeStyle = 4;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				alpha = 50;
				width = 10;
				height = 14;
				value = 11250;
				break;
			case 182:
				createTile = 178;
				placeStyle = 5;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				alpha = 50;
				width = 10;
				height = 14;
				value = 15000;
				break;
			case 183:
				width = 16;
				height = 18;
				value = 50;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 190;
				break;
			case 184:
				width = 12;
				height = 12;
				break;
			case 185:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 13f;
				shoot = 32;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = 20000;
				break;
			case 186:
				width = 44;
				height = 44;
				rare = 1;
				value = 10000;
				holdStyle = 2;
				useStyle = 1;
				useAnimation = 27;
				useTime = 19;
				knockBack = 4f;
				damage = 10;
				UseSound = SoundID.Item1;
				melee = true;
				break;
			case 187:
				width = 28;
				height = 28;
				rare = 1;
				value = 10000;
				accessory = true;
				shoeSlot = 1;
				break;
			case 188:
				UseSound = SoundID.Item3;
				healLife = 100;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				rare = 1;
				potion = true;
				value = 1000;
				break;
			case 189:
				UseSound = SoundID.Item3;
				healMana = 100;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				rare = 1;
				value = buyPrice(0, 0, 2, 50);
				break;
			case 190:
				useStyle = 1;
				useAnimation = 20;
				useTime = 20;
				shoot = 976;
				shootSpeed = 20f;
				knockBack = 4.5f;
				width = 40;
				height = 40;
				damage = 18;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 3;
				value = 27000;
				melee = true;
				shootsEveryUse = true;
				break;
			case 191:
				noMelee = true;
				useStyle = 1;
				shootSpeed = 14f;
				shoot = 33;
				damage = 25;
				knockBack = 8f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				rare = 3;
				value = 50000;
				melee = true;
				break;
			case 192:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 75;
				width = 12;
				height = 12;
				break;
			case 193:
				width = 20;
				height = 22;
				rare = 2;
				value = 27000;
				accessory = true;
				faceSlot = 12;
				defense = 1;
				break;
			case 194:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 70;
				width = 14;
				height = 14;
				value = 150;
				break;
			case 195:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 60;
				width = 14;
				height = 14;
				value = 150;
				break;
			case 196:
				autoReuse = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 37;
				useTime = 25;
				hammer = 25;
				width = 24;
				height = 28;
				damage = 2;
				knockBack = 5.5f;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				tileBoost = -1;
				value = 50;
				melee = true;
				break;
			case 197:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 12;
				useTime = 12;
				width = 50;
				height = 18;
				shoot = 955;
				useAmmo = AmmoID.FallenStar;
				UseSound = SoundID.Item9;
				knockBack = 3f;
				damage = 55;
				shootSpeed = 14f;
				noMelee = true;
				value = 500000;
				rare = 2;
				ranged = true;
				if (Variant == ItemVariants.RebalancedVariant)
				{
					damage = (int)((double)damage * 0.9);
					useTime = (int)((double)useTime * 1.1);
				}
				break;
			case 198:
				useStyle = 1;
				useTime = 18;
				useAnimation = 18;
				knockBack = 3f;
				width = 40;
				height = 40;
				damage = 26;
				scale = 1f;
				UseSound = SoundID.Item15;
				rare = 1;
				value = 27000;
				melee = true;
				break;
			case 199:
				useStyle = 1;
				useTime = 18;
				useAnimation = 18;
				knockBack = 3f;
				width = 40;
				height = 40;
				damage = 26;
				scale = 1f;
				UseSound = SoundID.Item15;
				rare = 1;
				value = 27000;
				melee = true;
				break;
			case 200:
				useStyle = 1;
				useTime = 18;
				useAnimation = 18;
				knockBack = 3f;
				width = 40;
				height = 40;
				damage = 26;
				scale = 1f;
				UseSound = SoundID.Item15;
				rare = 1;
				value = 27000;
				melee = true;
				break;
			case 201:
				useStyle = 1;
				useTime = 18;
				useAnimation = 18;
				knockBack = 3f;
				width = 40;
				height = 40;
				damage = 26;
				scale = 1f;
				UseSound = SoundID.Item15;
				rare = 1;
				value = 27000;
				melee = true;
				break;
			case 202:
				useStyle = 1;
				useTime = 18;
				useAnimation = 18;
				knockBack = 3f;
				width = 40;
				height = 40;
				damage = 26;
				scale = 1f;
				UseSound = SoundID.Item15;
				rare = 1;
				value = 27000;
				melee = true;
				break;
			case 203:
				useStyle = 1;
				useTime = 18;
				useAnimation = 18;
				knockBack = 3f;
				width = 40;
				height = 40;
				damage = 26;
				scale = 1f;
				UseSound = SoundID.Item15;
				rare = 1;
				value = 27000;
				melee = true;
				break;
			case 204:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 30;
				useTime = 16;
				hammer = 60;
				axe = 20;
				width = 24;
				height = 28;
				damage = 20;
				knockBack = 7f;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = 15000;
				melee = true;
				break;
			case 205:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				width = 20;
				height = 20;
				headSlot = 13;
				defense = 1;
				maxStack = CommonMaxStack;
				autoReuse = true;
				break;
			case 206:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				autoReuse = true;
				break;
			case 207:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				autoReuse = true;
				break;
			case 208:
				width = 20;
				height = 20;
				value = 100;
				faceSlot = 8;
				accessory = true;
				vanity = true;
				break;
			case 209:
				width = 16;
				height = 18;
				maxStack = CommonMaxStack;
				value = 200;
				break;
			case 210:
				width = 14;
				height = 20;
				maxStack = CommonMaxStack;
				value = 1000;
				break;
			case 211:
				width = 20;
				height = 20;
				accessory = true;
				rare = 3;
				value = 50000;
				handOnSlot = 5;
				handOffSlot = 9;
				break;
			case 212:
				width = 20;
				height = 20;
				accessory = true;
				rare = 3;
				value = 50000;
				break;
			case 213:
				useStyle = 1;
				useTurn = true;
				useAnimation = 25;
				useTime = 13;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 7;
				createTile = 2;
				UseSound = SoundID.Item1;
				knockBack = 3f;
				rare = 3;
				SetShopValues(ItemRarityColor.Orange3, sellPrice(0, 0, 50));
				melee = true;
				break;
			case 214:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 76;
				width = 12;
				height = 12;
				break;
			case 215:
				width = 18;
				height = 18;
				useTurn = true;
				useTime = 30;
				useAnimation = 30;
				noUseGraphic = true;
				useStyle = 10;
				UseSound = SoundID.Item16;
				rare = 2;
				value = 100;
				break;
			case 216:
				width = 20;
				height = 20;
				rare = 1;
				value = sellPrice(0, 0, 20);
				accessory = true;
				defense = 1;
				handOffSlot = 7;
				handOnSlot = 12;
				break;
			case 217:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 27;
				useTime = 14;
				hammer = 70;
				axe = 30;
				width = 24;
				height = 28;
				damage = 20;
				knockBack = 7f;
				scale = 1.4f;
				UseSound = SoundID.Item1;
				rare = 3;
				value = 27000;
				melee = true;
				break;
			case 218:
				mana = 21;
				channel = true;
				damage = 32;
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 34;
				width = 26;
				height = 28;
				UseSound = SoundID.Item20;
				useAnimation = 30;
				useTime = 30;
				rare = 3;
				noMelee = true;
				knockBack = 6.5f;
				value = hellPrice;
				magic = true;
				break;
			case 219:
				autoReuse = false;
				useStyle = 5;
				useAnimation = 17;
				useTime = 17;
				width = 24;
				height = 22;
				shoot = 14;
				knockBack = 2f;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item41;
				damage = 33;
				shootSpeed = 13f;
				noMelee = true;
				value = sellPrice(0, 3, 50);
				scale = 0.85f;
				rare = 3;
				ranged = true;
				break;
			case 220:
				noMelee = true;
				useStyle = 5;
				useAnimation = 45;
				useTime = 45;
				knockBack = 6.75f;
				width = 30;
				height = 10;
				damage = 32;
				crit = 7;
				scale = 1.1f;
				noUseGraphic = true;
				shoot = 35;
				shootSpeed = 12f;
				UseSound = SoundID.Item1;
				rare = 3;
				value = hellPrice;
				melee = true;
				channel = true;
				break;
			case 221:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 77;
				width = 26;
				height = 24;
				value = 3000;
				break;
			case 222:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 78;
				width = 14;
				height = 14;
				value = 100;
				break;
			case 223:
				width = 20;
				height = 22;
				rare = 3;
				value = 27000;
				accessory = true;
				faceSlot = 1;
				break;
			case 224:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 79;
				width = 28;
				height = 20;
				value = 2000;
				break;
			case 225:
				maxStack = CommonMaxStack;
				width = 22;
				height = 22;
				value = 1000;
				break;
			case 226:
			case 227:
				this.type = 227;
				UseSound = SoundID.Item3;
				healLife = 90;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				potion = true;
				value = 1500;
				rare = 1;
				break;
			case 228:
				width = 18;
				height = 18;
				defense = 5;
				headSlot = 8;
				rare = 3;
				value = 45000;
				break;
			case 229:
				width = 18;
				height = 18;
				defense = 6;
				bodySlot = 8;
				rare = 3;
				value = 30000;
				break;
			case 230:
				width = 18;
				height = 18;
				defense = 6;
				legSlot = 8;
				rare = 3;
				value = 30000;
				break;
			case 231:
				width = 18;
				height = 18;
				defense = 8;
				headSlot = 9;
				rare = 3;
				value = 45000;
				break;
			case 232:
				width = 18;
				height = 18;
				defense = 9;
				bodySlot = 9;
				rare = 3;
				value = 30000;
				break;
			case 233:
				width = 18;
				height = 18;
				defense = 8;
				legSlot = 9;
				rare = 3;
				value = 30000;
				break;
			case 234:
				shootSpeed = 3f;
				shoot = 36;
				damage = 8;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 1f;
				value = 8;
				rare = 1;
				ranged = true;
				break;
			case 235:
				useStyle = 1;
				shootSpeed = 5f;
				shoot = 37;
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 25;
				useTime = 25;
				noUseGraphic = true;
				noMelee = true;
				value = 500;
				damage = 0;
				break;
			case 236:
				width = 12;
				height = 20;
				maxStack = CommonMaxStack;
				value = 5000;
				break;
			case 237:
				width = 28;
				height = 12;
				headSlot = 12;
				rare = 2;
				value = 10000;
				vanity = true;
				break;
			case 238:
				width = 28;
				height = 20;
				headSlot = 14;
				rare = 2;
				value = 10000;
				defense = 4;
				break;
			case 239:
				width = 18;
				height = 18;
				headSlot = 15;
				value = 10000;
				vanity = true;
				break;
			case 240:
				width = 18;
				height = 18;
				bodySlot = 10;
				value = 5000;
				vanity = true;
				break;
			case 241:
				width = 18;
				height = 18;
				legSlot = 10;
				value = 5000;
				vanity = true;
				break;
			case 242:
				width = 18;
				height = 18;
				headSlot = 16;
				value = 10000;
				vanity = true;
				break;
			case 243:
				width = 18;
				height = 18;
				headSlot = 17;
				value = 20000;
				vanity = true;
				break;
			case 244:
				width = 18;
				height = 12;
				headSlot = 18;
				value = 10000;
				vanity = true;
				break;
			case 245:
				width = 18;
				height = 18;
				bodySlot = 11;
				value = 250000;
				vanity = true;
				break;
			case 246:
				width = 18;
				height = 18;
				legSlot = 11;
				value = 250000;
				vanity = true;
				break;
			case 247:
				width = 18;
				height = 12;
				headSlot = 19;
				value = 10000;
				vanity = true;
				break;
			case 248:
				width = 18;
				height = 18;
				bodySlot = 12;
				value = 5000;
				vanity = true;
				break;
			case 249:
				width = 18;
				height = 18;
				legSlot = 12;
				value = 5000;
				vanity = true;
				break;
			case 250:
				width = 18;
				height = 18;
				headSlot = 20;
				value = 10000;
				vanity = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 282;
				width = 12;
				height = 12;
				break;
			case 251:
				width = 18;
				height = 12;
				headSlot = 21;
				value = 10000;
				vanity = true;
				break;
			case 252:
				width = 18;
				height = 18;
				bodySlot = 13;
				value = 5000;
				vanity = true;
				break;
			case 253:
				width = 18;
				height = 18;
				legSlot = 13;
				value = 5000;
				vanity = true;
				break;
			case 254:
				maxStack = CommonMaxStack;
				width = 12;
				height = 20;
				value = 10000;
				break;
			case 255:
				maxStack = CommonMaxStack;
				width = 12;
				height = 20;
				value = 2000;
				break;
			case 256:
				width = 18;
				height = 12;
				headSlot = 22;
				value = 10000;
				defense = 2;
				rare = 1;
				break;
			case 257:
				width = 18;
				height = 18;
				bodySlot = 14;
				value = 5000;
				defense = 4;
				rare = 1;
				break;
			case 258:
				width = 18;
				height = 18;
				legSlot = 14;
				value = 5000;
				defense = 3;
				rare = 1;
				break;
			case 259:
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				value = 50;
				break;
			case 260:
				width = 18;
				height = 14;
				headSlot = 24;
				value = 1000;
				vanity = true;
				break;
			case 261:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				makeNPC = 55;
				break;
			case 262:
				width = 18;
				height = 14;
				bodySlot = 15;
				value = 2000;
				vanity = true;
				break;
			case 263:
				width = 18;
				height = 18;
				headSlot = 25;
				value = 10000;
				vanity = true;
				break;
			case 264:
				width = 18;
				height = 18;
				headSlot = 26;
				value = 10000;
				vanity = true;
				break;
			case 265:
				shootSpeed = 6.5f;
				shoot = 41;
				damage = 13;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 8f;
				value = 100;
				rare = 2;
				ranged = true;
				break;
			case 266:
				useStyle = 5;
				useAnimation = 16;
				useTime = 16;
				autoReuse = true;
				width = 40;
				height = 20;
				shoot = 42;
				useAmmo = AmmoID.Sand;
				UseSound = SoundID.Item11;
				damage = 30;
				shootSpeed = 12f;
				noMelee = true;
				knockBack = 5f;
				value = 10000;
				rare = 2;
				ranged = true;
				break;
			case 267:
				accessory = true;
				width = 14;
				height = 26;
				value = 1000;
				maxStack = CommonMaxStack;
				break;
			case 268:
				headSlot = 27;
				defense = 2;
				width = 20;
				height = 20;
				value = 1000;
				rare = 2;
				break;
			case 269:
				bodySlot = 0;
				width = 20;
				height = 20;
				value = 10000;
				color = Main.player[Main.myPlayer].shirtColor;
				vanity = true;
				break;
			case 270:
				legSlot = 0;
				width = 20;
				height = 20;
				value = 10000;
				color = Main.player[Main.myPlayer].pantsColor;
				vanity = true;
				break;
			case 271:
				headSlot = 0;
				width = 20;
				height = 20;
				value = 10000;
				color = Main.player[Main.myPlayer].hairColor;
				vanity = true;
				break;
			case 272:
				mana = 14;
				damage = 35;
				useStyle = 5;
				shootSpeed = 0.2f;
				shoot = 45;
				width = 26;
				height = 28;
				UseSound = SoundID.Item8;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				knockBack = 5f;
				scale = 0.9f;
				value = sellPrice(0, 1, 50);
				magic = true;
				break;
			case 273:
				useStyle = 1;
				useAnimation = 21;
				useTime = 21;
				autoReuse = true;
				knockBack = 4.5f;
				width = 40;
				height = 40;
				damage = 42;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 3;
				value = sellPrice(0, 4);
				melee = true;
				shoot = 972;
				shootSpeed = 5f;
				noMelee = true;
				shootsEveryUse = true;
				break;
			case 274:
				useStyle = 5;
				useAnimation = 22;
				useTime = 22;
				shootSpeed = 6f;
				knockBack = 5f;
				width = 40;
				height = 40;
				damage = 34;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 46;
				rare = 3;
				value = hellPrice;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				break;
			case 275:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 81;
				width = 20;
				height = 22;
				value = sellPrice(0, 0, 2);
				break;
			case 276:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 188;
				width = 12;
				height = 12;
				value = 10;
				break;
			case 277:
				useStyle = 5;
				useAnimation = 31;
				useTime = 31;
				shootSpeed = 4f;
				knockBack = 6f;
				width = 40;
				height = 40;
				damage = 14;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 47;
				rare = 1;
				value = 10000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				break;
			case 278:
				shootSpeed = 4.5f;
				shoot = 981;
				damage = 9;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 3f;
				value = 15;
				ranged = true;
				break;
			case 279:
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 48;
				damage = 12;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 50;
				knockBack = 2f;
				ranged = true;
				break;
			case 280:
				useStyle = 5;
				useAnimation = 31;
				useTime = 31;
				shootSpeed = 3.7f;
				knockBack = 6.5f;
				width = 32;
				height = 32;
				damage = 8;
				scale = 1f;
				UseSound = SoundID.Item1;
				shoot = 49;
				value = 1000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				break;
			case 281:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 25;
				useTime = 25;
				width = 38;
				height = 6;
				shoot = 10;
				useAmmo = AmmoID.Dart;
				UseSound = SoundID.Item63;
				damage = 9;
				shootSpeed = 11f;
				noMelee = true;
				value = 10000;
				knockBack = 3.5f;
				ranged = true;
				break;
			case 282:
				color = new Color(255, 255, 255, 0);
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 50;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noMelee = true;
				value = 10;
				holdStyle = 1;
				break;
			case 283:
				shoot = 51;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				ammo = AmmoID.Dart;
				damage = 4;
				ranged = true;
				consumable = true;
				break;
			case 284:
				noMelee = true;
				useStyle = 1;
				shootSpeed = 6.5f;
				shoot = 52;
				damage = 10;
				knockBack = 5f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				value = 10000;
				rare = 1;
				melee = true;
				break;
			case 285:
				width = 24;
				height = 8;
				accessory = true;
				value = sellPrice(0, 0, 50);
				rare = 1;
				break;
			case 286:
				color = new Color(255, 255, 255, 0);
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 53;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noMelee = true;
				value = 20;
				holdStyle = 1;
				break;
			case 287:
				crit = 4;
				useStyle = 1;
				shootSpeed = 12f;
				shoot = 54;
				damage = 14;
				autoReuse = true;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 60;
				knockBack = 2.4f;
				ranged = true;
				break;
			case 288:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 1;
				buffTime = 21600;
				value = 1000;
				rare = 1;
				break;
			case 289:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 2;
				buffTime = 28800;
				value = 1000;
				rare = 1;
				break;
			case 290:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 3;
				buffTime = 28800;
				value = 1000;
				rare = 1;
				break;
			case 291:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 4;
				buffTime = 14400;
				value = 1000;
				rare = 1;
				break;
			case 292:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 5;
				buffTime = 28800;
				value = 1000;
				rare = 1;
				break;
			case 293:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 6;
				buffTime = 28800;
				value = 1000;
				rare = 1;
				break;
			case 294:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 7;
				buffTime = 14400;
				value = 1000;
				rare = 1;
				break;
			case 295:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 8;
				buffTime = 36000;
				value = 1000;
				rare = 1;
				break;
			case 296:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 9;
				buffTime = 18000;
				value = 1000;
				rare = 1;
				break;
			case 297:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 10;
				buffTime = 10800;
				value = 1000;
				rare = 1;
				break;
			case 298:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 11;
				buffTime = 36000;
				value = 1000;
				rare = 1;
				break;
			case 299:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 12;
				buffTime = 36000;
				value = 1000;
				rare = 1;
				break;
			case 300:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 13;
				buffTime = 25200;
				value = 1000;
				rare = 1;
				break;
			case 301:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 14;
				buffTime = 28800;
				value = 1000;
				rare = 1;
				break;
			case 302:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 15;
				buffTime = 36000;
				value = 1000;
				rare = 1;
				break;
			case 303:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 16;
				buffTime = 28800;
				value = 1000;
				rare = 1;
				break;
			case 304:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 17;
				buffTime = 28800;
				value = 1000;
				rare = 1;
				break;
			case 305:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 18;
				buffTime = 10800;
				value = 1000;
				rare = 1;
				break;
			case 306:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 1;
				width = 26;
				height = 22;
				value = 5000;
				break;
			case 307:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 82;
				placeStyle = 0;
				width = 12;
				height = 14;
				value = 80;
				break;
			case 308:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 82;
				placeStyle = 1;
				width = 12;
				height = 14;
				value = 80;
				break;
			case 309:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 82;
				placeStyle = 2;
				width = 12;
				height = 14;
				value = 80;
				break;
			case 310:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 82;
				placeStyle = 3;
				width = 12;
				height = 14;
				value = 80;
				break;
			case 311:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 82;
				placeStyle = 4;
				width = 12;
				height = 14;
				value = 80;
				break;
			case 312:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 82;
				placeStyle = 5;
				width = 12;
				height = 14;
				value = 80;
				break;
			case 313:
				maxStack = CommonMaxStack;
				width = 12;
				height = 14;
				value = 100;
				break;
			case 314:
				maxStack = CommonMaxStack;
				width = 12;
				height = 14;
				value = 100;
				break;
			case 315:
				maxStack = CommonMaxStack;
				width = 12;
				height = 14;
				value = 100;
				break;
			case 316:
				maxStack = CommonMaxStack;
				width = 12;
				height = 14;
				value = 100;
				break;
			case 317:
				maxStack = CommonMaxStack;
				width = 12;
				height = 14;
				value = 100;
				break;
			case 318:
				maxStack = CommonMaxStack;
				width = 12;
				height = 14;
				value = 100;
				break;
			case 319:
				maxStack = CommonMaxStack;
				width = 16;
				height = 14;
				value = 200;
				color = new Color(123, 167, 163, 255);
				break;
			case 320:
				maxStack = CommonMaxStack;
				width = 16;
				height = 14;
				value = 50;
				break;
			case 321:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 85;
				width = 20;
				height = 20;
				break;
			case 322:
				headSlot = 28;
				width = 20;
				height = 20;
				value = 20000;
				vanity = true;
				break;
			case 323:
				width = 10;
				height = 20;
				maxStack = CommonMaxStack;
				value = 50;
				break;
			case 324:
				width = 10;
				height = 20;
				maxStack = CommonMaxStack;
				value = 200000;
				break;
			case 325:
				width = 18;
				height = 18;
				bodySlot = 16;
				value = 200000;
				vanity = true;
				break;
			case 326:
				width = 18;
				height = 18;
				legSlot = 15;
				value = 200000;
				vanity = true;
				break;
			case 327:
				width = 14;
				height = 20;
				maxStack = CommonMaxStack;
				break;
			case 328:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 3;
				width = 26;
				height = 22;
				value = 5000;
				break;
			case 329:
				width = 14;
				height = 20;
				maxStack = 1;
				value = dungeonPrice;
				break;
			case 330:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 20;
				width = 12;
				height = 12;
				break;
			case 331:
				width = 18;
				height = 16;
				maxStack = CommonMaxStack;
				value = 100;
				break;
			case 332:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 86;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 333:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 87;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 334:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 88;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 335:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 89;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 336:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 90;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 337:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 0;
				width = 10;
				height = 24;
				value = 500;
				break;
			case 338:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 1;
				width = 10;
				height = 24;
				value = 500;
				break;
			case 339:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 2;
				width = 10;
				height = 24;
				value = 500;
				break;
			case 340:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 3;
				width = 10;
				height = 24;
				value = 500;
				break;
			case 341:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 92;
				width = 10;
				height = 24;
				value = 500;
				break;
			case 342:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 93;
				width = 10;
				height = 24;
				value = 500;
				break;
			case 343:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 5;
				width = 20;
				height = 20;
				value = 500;
				break;
			case 344:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 95;
				width = 20;
				height = 20;
				value = 500;
				break;
			case 345:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 96;
				width = 20;
				height = 20;
				value = 500;
				break;
			case 346:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 97;
				width = 20;
				height = 20;
				value = 200000;
				break;
			case 347:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 98;
				width = 20;
				height = 20;
				value = 500;
				break;
			case 348:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 6;
				width = 20;
				height = 20;
				value = 1000;
				break;
			case 349:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 100;
				width = 20;
				height = 20;
				value = 1500;
				break;
			case 350:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 13;
				placeStyle = 3;
				width = 16;
				height = 24;
				value = 70;
				break;
			case 351:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 13;
				placeStyle = 4;
				width = 16;
				height = 24;
				value = 20;
				break;
			case 352:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 94;
				width = 24;
				height = 24;
				value = 600;
				break;
			case 354:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 355:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 102;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 356:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 103;
				width = 16;
				height = 24;
				value = 20;
				break;
			case 358:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 1;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 359:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 104;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 360:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 361:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				break;
			case 362:
				maxStack = CommonMaxStack;
				width = 24;
				height = 24;
				value = 30;
				break;
			case 363:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 106;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 364:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 107;
				width = 12;
				height = 12;
				value = 3500;
				rare = 3;
				break;
			case 365:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 108;
				width = 12;
				height = 12;
				value = 5500;
				rare = 3;
				break;
			case 366:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 111;
				width = 12;
				height = 12;
				value = 7500;
				rare = 3;
				break;
			case 367:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 27;
				useTime = 14;
				hammer = 80;
				width = 24;
				height = 28;
				damage = 26;
				knockBack = 7.5f;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 39000;
				melee = true;
				break;
			case 368:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 20;
				useTime = 20;
				knockBack = 4.5f;
				width = 40;
				height = 40;
				damage = 72;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 5;
				value = 230000;
				melee = true;
				shoot = 982;
				noMelee = true;
				shootsEveryUse = true;
				break;
			case 369:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 109;
				width = 14;
				height = 14;
				value = 2000;
				rare = 3;
				break;
			case 370:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 112;
				width = 12;
				height = 12;
				ammo = AmmoID.Sand;
				notAmmo = true;
				break;
			case 371:
				width = 18;
				height = 18;
				defense = 3;
				headSlot = 29;
				rare = 4;
				value = 75000;
				break;
			case 372:
				width = 18;
				height = 18;
				defense = 14;
				headSlot = 30;
				rare = 4;
				value = 75000;
				break;
			case 373:
				width = 18;
				height = 18;
				defense = 5;
				headSlot = 31;
				rare = 4;
				value = 75000;
				break;
			case 374:
				width = 18;
				height = 18;
				defense = 10;
				bodySlot = 17;
				rare = 4;
				value = 60000;
				break;
			case 375:
				width = 18;
				height = 18;
				defense = 8;
				legSlot = 16;
				rare = 4;
				value = 45000;
				break;
			case 376:
				width = 18;
				height = 18;
				defense = 3;
				headSlot = 32;
				rare = 4;
				value = 112500;
				break;
			case 377:
				width = 18;
				height = 18;
				defense = 16;
				headSlot = 33;
				rare = 4;
				value = 112500;
				break;
			case 378:
				width = 18;
				height = 18;
				defense = 6;
				headSlot = 34;
				rare = 4;
				value = 112500;
				break;
			case 379:
				width = 18;
				height = 18;
				defense = 12;
				bodySlot = 18;
				rare = 4;
				value = 90000;
				break;
			case 380:
				width = 18;
				height = 18;
				defense = 9;
				legSlot = 17;
				rare = 4;
				value = 67500;
				break;
			case 381:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10500;
				rare = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 11;
				break;
			case 382:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 22000;
				rare = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 13;
				break;
			case 383:
				useStyle = 5;
				useAnimation = 25;
				useTime = 13;
				shootSpeed = 40f;
				knockBack = 2.75f;
				width = 20;
				height = 12;
				damage = 23;
				axe = 14;
				UseSound = SoundID.Item23;
				shoot = 57;
				rare = 4;
				value = 54000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				break;
			case 384:
				useStyle = 5;
				useAnimation = 25;
				useTime = 10;
				shootSpeed = 40f;
				knockBack = 3f;
				width = 20;
				height = 12;
				damage = 29;
				axe = 17;
				UseSound = SoundID.Item23;
				shoot = 58;
				rare = 4;
				value = 81000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				break;
			case 385:
				useStyle = 5;
				useAnimation = 25;
				useTime = 13;
				shootSpeed = 32f;
				knockBack = 0.5f;
				width = 20;
				height = 12;
				damage = 10;
				pick = 110;
				UseSound = SoundID.Item23;
				shoot = 59;
				rare = 4;
				value = 54000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				break;
			case 386:
				useStyle = 5;
				useAnimation = 25;
				useTime = 10;
				shootSpeed = 32f;
				knockBack = 0.5f;
				width = 20;
				height = 12;
				damage = 15;
				pick = 150;
				UseSound = SoundID.Item23;
				shoot = 60;
				rare = 4;
				value = 81000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				break;
			case 387:
				useStyle = 5;
				useAnimation = 25;
				useTime = 8;
				shootSpeed = 40f;
				knockBack = 4.5f;
				width = 20;
				height = 12;
				damage = 33;
				axe = 20;
				UseSound = SoundID.Item23;
				shoot = 61;
				rare = 4;
				value = 108000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				break;
			case 388:
				useStyle = 5;
				useAnimation = 25;
				useTime = 8;
				shootSpeed = 32f;
				knockBack = 0.5f;
				width = 20;
				height = 12;
				damage = 20;
				pick = 180;
				UseSound = SoundID.Item23;
				shoot = 62;
				rare = 4;
				value = 108000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				break;
			case 389:
				noMelee = true;
				useStyle = 5;
				useAnimation = 45;
				useTime = 45;
				knockBack = 6f;
				width = 30;
				height = 10;
				damage = 50;
				scale = 1.1f;
				noUseGraphic = true;
				shoot = 63;
				shootSpeed = 15f;
				UseSound = SoundID.Item1;
				rare = 5;
				value = 144000;
				melee = true;
				channel = true;
				break;
			case 390:
				useStyle = 5;
				useAnimation = 26;
				useTime = 26;
				shootSpeed = 4.5f;
				knockBack = 5f;
				width = 40;
				height = 40;
				damage = 45;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 64;
				rare = 4;
				value = 67500;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				break;
			case 391:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 30000;
				rare = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 15;
				break;
			case 392:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 21;
				width = 12;
				height = 12;
				break;
			case 393:
				width = 24;
				height = 28;
				rare = 1;
				value = sellPrice(0, 0, 25);
				accessory = true;
				break;
			case 394:
				width = 24;
				height = 28;
				rare = 4;
				value = 100000;
				accessory = true;
				faceSlot = 4;
				break;
			case 395:
				width = 24;
				height = 28;
				rare = 3;
				value = sellPrice(0, 3);
				accessory = true;
				break;
			case 396:
				width = 24;
				height = 28;
				rare = 4;
				value = buyPrice(0, 6);
				accessory = true;
				break;
			case 397:
				width = 24;
				height = 28;
				rare = 4;
				value = 100000;
				accessory = true;
				defense = 2;
				shieldSlot = 3;
				break;
			case 398:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 114;
				width = 26;
				height = 20;
				value = 100000;
				break;
			case 399:
				width = 14;
				height = 28;
				rare = 4;
				value = 150000;
				accessory = true;
				balloonSlot = 4;
				break;
			case 400:
				width = 18;
				height = 18;
				defense = 4;
				headSlot = 35;
				rare = 4;
				value = 150000;
				break;
			case 401:
				width = 18;
				height = 18;
				defense = 22;
				headSlot = 36;
				rare = 4;
				value = 150000;
				break;
			case 402:
				width = 18;
				height = 18;
				defense = 8;
				headSlot = 37;
				rare = 4;
				value = 150000;
				break;
			case 403:
				width = 18;
				height = 18;
				defense = 16;
				bodySlot = 19;
				rare = 4;
				value = 120000;
				break;
			case 404:
				width = 18;
				height = 18;
				defense = 12;
				legSlot = 18;
				rare = 4;
				value = 90000;
				break;
			case 405:
				width = 28;
				height = 24;
				accessory = true;
				rare = 4;
				value = 100000;
				shoeSlot = 13;
				break;
			case 406:
				useStyle = 5;
				useAnimation = 25;
				useTime = 25;
				shootSpeed = 5f;
				knockBack = 6f;
				width = 40;
				height = 40;
				damage = 49;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 66;
				rare = 4;
				value = 90000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				break;
			case 407:
				width = 28;
				height = 24;
				accessory = true;
				rare = 3;
				value = 100000;
				waistSlot = 5;
				break;
			case 408:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 116;
				width = 12;
				height = 12;
				ammo = AmmoID.Sand;
				notAmmo = true;
				break;
			case 409:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 117;
				width = 12;
				height = 12;
				break;
			case 410:
				width = 18;
				height = 18;
				defense = 1;
				bodySlot = 20;
				value = 5000;
				rare = 1;
				break;
			case 411:
				width = 18;
				height = 18;
				defense = 1;
				legSlot = 19;
				value = 5000;
				rare = 1;
				break;
			case 412:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 118;
				width = 12;
				height = 12;
				break;
			case 413:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 119;
				width = 12;
				height = 12;
				break;
			case 414:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 120;
				width = 12;
				height = 12;
				break;
			case 415:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 121;
				width = 12;
				height = 12;
				break;
			case 416:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 122;
				width = 12;
				height = 12;
				break;
			case 417:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 22;
				width = 12;
				height = 12;
				break;
			case 418:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 23;
				width = 12;
				height = 12;
				break;
			case 419:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 24;
				width = 12;
				height = 12;
				break;
			case 420:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 25;
				width = 12;
				height = 12;
				break;
			case 421:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 26;
				width = 12;
				height = 12;
				break;
			case 422:
				useStyle = 1;
				shootSpeed = 9f;
				rare = 3;
				damage = 20;
				shoot = 69;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				knockBack = 3f;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 200;
				break;
			case 423:
				useStyle = 1;
				shootSpeed = 9f;
				rare = 3;
				damage = 20;
				shoot = 70;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				knockBack = 3f;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 100;
				break;
			case 424:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 123;
				width = 12;
				height = 12;
				break;
			case 425:
				channel = true;
				damage = 0;
				useStyle = 1;
				width = 24;
				height = 24;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 5;
				noMelee = true;
				value = (value = 250000);
				buffType = 27;
				break;
			case 426:
				useStyle = 1;
				useTime = 35;
				useAnimation = 35;
				knockBack = 8f;
				width = 60;
				height = 70;
				damage = 70;
				scale = 1.15f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 150000;
				melee = true;
				break;
			case 427:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 1;
				width = 10;
				height = 12;
				value = 200;
				break;
			case 428:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 2;
				width = 10;
				height = 12;
				value = 200;
				break;
			case 429:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 3;
				width = 10;
				height = 12;
				value = 200;
				break;
			case 430:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 4;
				width = 10;
				height = 12;
				value = 200;
				break;
			case 431:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 5;
				width = 10;
				height = 12;
				value = 500;
				break;
			case 432:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 6;
				width = 10;
				height = 12;
				value = 200;
				break;
			case 433:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 7;
				width = 10;
				height = 12;
				value = 300;
				break;
			case 434:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 12;
				useTime = 4;
				reuseDelay = 14;
				width = 50;
				height = 18;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item31;
				damage = 17;
				shootSpeed = 7.75f;
				noMelee = true;
				value = 150000;
				rare = 4;
				ranged = true;
				break;
			case 435:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 23;
				useTime = 23;
				width = 50;
				height = 18;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 35;
				shootSpeed = 9f;
				noMelee = true;
				value = 60000;
				ranged = true;
				rare = 4;
				knockBack = 1.5f;
				break;
			case 436:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 20;
				useTime = 20;
				width = 50;
				height = 18;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 39;
				shootSpeed = 9.5f;
				noMelee = true;
				value = 90000;
				ranged = true;
				rare = 4;
				knockBack = 2f;
				break;
			case 437:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 14f;
				shoot = 73;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 4;
				noMelee = true;
				value = buyPrice(0, 15);
				break;
			case 438:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 2;
				break;
			case 439:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 3;
				break;
			case 440:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 4;
				break;
			case 441:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 5;
				break;
			case 442:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 6;
				break;
			case 443:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 7;
				break;
			case 444:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 8;
				break;
			case 445:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 9;
				break;
			case 446:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 10;
				break;
			case 447:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 11;
				break;
			case 448:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 12;
				break;
			case 449:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 13;
				break;
			case 450:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 14;
				break;
			case 451:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 15;
				break;
			case 452:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 16;
				break;
			case 453:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 17;
				break;
			case 454:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 18;
				break;
			case 455:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 19;
				break;
			case 456:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 20;
				break;
			case 457:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 21;
				break;
			case 458:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 22;
				break;
			case 459:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 23;
				break;
			case 460:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 24;
				break;
			case 461:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 25;
				break;
			case 462:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 26;
				break;
			case 463:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 27;
				break;
			case 464:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 28;
				break;
			case 465:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 29;
				break;
			case 466:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 30;
				break;
			case 467:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 31;
				break;
			case 468:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 32;
				break;
			case 469:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 33;
				break;
			case 470:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 349;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 471:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 35;
				break;
			case 472:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 36;
				break;
			case 473:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 37;
				break;
			case 474:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 38;
				break;
			case 475:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 39;
				break;
			case 476:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 40;
				break;
			case 477:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 41;
				break;
			case 478:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 42;
				break;
			case 479:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 27;
				width = 12;
				height = 12;
				break;
			case 480:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 124;
				width = 12;
				height = 12;
				break;
			case 481:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 18;
				useTime = 18;
				width = 50;
				height = 18;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 42;
				shootSpeed = 10f;
				noMelee = true;
				value = 120000;
				ranged = true;
				rare = 4;
				knockBack = 2.5f;
				break;
			case 482:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 21;
				useTime = 21;
				knockBack = 6f;
				width = 40;
				height = 40;
				damage = 61;
				scale = 1.25f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 138000;
				melee = true;
				break;
			case 483:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 19;
				useTime = 19;
				knockBack = 5f;
				width = 40;
				height = 40;
				damage = 40;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 69000;
				melee = true;
				break;
			case 484:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 20;
				useTime = 20;
				knockBack = 6f;
				width = 40;
				height = 40;
				damage = 50;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 103500;
				melee = true;
				break;
			case 485:
				rare = 4;
				width = 24;
				height = 28;
				accessory = true;
				value = 150000;
				hasVanityEffects = true;
				break;
			case 486:
				autoReuse = true;
				useStyle = 13;
				useAnimation = 20;
				useTime = 5;
				width = 40;
				height = 18;
				shoot = 842;
				UseSound = SoundID.Item1;
				damage = 12;
				shootSpeed = 2.4f;
				noMelee = true;
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 10));
				melee = true;
				knockBack = 0.5f;
				noUseGraphic = true;
				break;
			case 487:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 125;
				width = 22;
				height = 22;
				value = 100000;
				rare = 3;
				break;
			case 488:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 126;
				width = 22;
				height = 26;
				value = 10000;
				break;
			case 489:
				width = 24;
				height = 24;
				accessory = true;
				value = 100000;
				rare = 4;
				break;
			case 491:
				width = 24;
				height = 24;
				accessory = true;
				value = 100000;
				rare = 4;
				break;
			case 490:
				width = 24;
				height = 24;
				accessory = true;
				value = 100000;
				rare = 4;
				break;
			case 492:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 1;
				break;
			case 493:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 2;
				break;
			case 494:
				rare = 5;
				useStyle = 5;
				useAnimation = 12;
				useTime = 12;
				width = 12;
				height = 28;
				shoot = 76;
				holdStyle = 3;
				autoReuse = true;
				damage = 42;
				shootSpeed = 4.5f;
				noMelee = true;
				knockBack = 2f;
				value = 200000;
				mana = 5;
				magic = true;
				break;
			case 495:
				rare = 5;
				mana = 21;
				channel = true;
				damage = 50;
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 79;
				width = 26;
				height = 28;
				UseSound = SoundID.Item28;
				useAnimation = 25;
				useTime = 25;
				noMelee = true;
				knockBack = 6f;
				value = 200000;
				magic = true;
				break;
			case 496:
				rare = 4;
				mana = 6;
				damage = 28;
				useStyle = 1;
				shootSpeed = 12f;
				shoot = 80;
				width = 26;
				height = 28;
				UseSound = SoundID.Item28;
				useAnimation = 9;
				useTime = 9;
				rare = 4;
				autoReuse = true;
				noMelee = true;
				knockBack = 0f;
				value = buyPrice(0, 50);
				magic = true;
				knockBack = 2f;
				break;
			case 497:
				width = 24;
				height = 28;
				accessory = true;
				value = eclipsePrice;
				rare = 5;
				hasVanityEffects = true;
				break;
			case 498:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 128;
				width = 12;
				height = 12;
				createTile = 470;
				placeStyle = 0;
				break;
			case 499:
				UseSound = SoundID.Item3;
				healLife = 150;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				rare = 3;
				potion = true;
				value = 5000;
				break;
			case 500:
				UseSound = SoundID.Item3;
				healMana = 200;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				rare = 3;
				value = buyPrice(0, 0, 5);
				break;
			case 501:
				width = 16;
				height = 14;
				maxStack = CommonMaxStack;
				value = 500;
				rare = 1;
				break;
			case 502:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 129;
				width = 24;
				height = 24;
				value = 8000;
				rare = 1;
				break;
			case 503:
				width = 18;
				height = 18;
				headSlot = 40;
				value = 20000;
				vanity = true;
				rare = 2;
				break;
			case 504:
				width = 18;
				height = 18;
				bodySlot = 23;
				value = 10000;
				vanity = true;
				rare = 2;
				break;
			case 505:
				width = 18;
				height = 18;
				legSlot = 22;
				value = 10000;
				vanity = true;
				rare = 2;
				break;
			case 506:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 30;
				useTime = 6;
				width = 50;
				height = 18;
				shoot = 85;
				useAmmo = AmmoID.Gel;
				UseSound = SoundID.Item34;
				damage = 35;
				knockBack = 0.3f;
				shootSpeed = 7f;
				noMelee = true;
				value = 500000;
				rare = 5;
				ranged = true;
				break;
			case 507:
				rare = 3;
				useStyle = 1;
				useAnimation = 12;
				useTime = 12;
				width = 12;
				height = 28;
				autoReuse = true;
				noMelee = true;
				value = 10000;
				break;
			case 508:
				rare = 3;
				useStyle = 5;
				useAnimation = 12;
				useTime = 12;
				width = 12;
				height = 28;
				autoReuse = true;
				noMelee = true;
				value = 10000;
				break;
			case 509:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 5;
				autoReuse = true;
				width = 24;
				height = 28;
				rare = 1;
				value = 20000;
				mech = true;
				tileBoost = 20;
				break;
			case 510:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 5;
				autoReuse = true;
				width = 24;
				height = 28;
				rare = 1;
				value = 20000;
				mech = true;
				tileBoost = 20;
				break;
			case 511:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 130;
				width = 12;
				height = 12;
				value = 1000;
				mech = true;
				break;
			case 512:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 131;
				width = 12;
				height = 12;
				value = 1000;
				mech = true;
				break;
			case 513:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 132;
				width = 24;
				height = 24;
				value = 3000;
				mech = true;
				break;
			case 514:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 12;
				useTime = 12;
				width = 36;
				height = 22;
				shoot = 88;
				mana = 8;
				UseSound = SoundID.Item12;
				knockBack = 2.5f;
				damage = 29;
				shootSpeed = 17f;
				noMelee = true;
				rare = 4;
				magic = true;
				value = 150000;
				break;
			case 515:
				shootSpeed = 5f;
				shoot = 89;
				damage = 9;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 1f;
				value = 30;
				ranged = true;
				rare = 3;
				break;
			case 516:
				shootSpeed = 3.5f;
				shoot = 91;
				damage = 13;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 2f;
				value = 80;
				ranged = true;
				rare = 3;
				break;
			case 517:
				useStyle = 1;
				shootSpeed = 12f;
				shoot = 93;
				damage = 35;
				width = 18;
				height = 20;
				mana = 6;
				autoReuse = true;
				UseSound = SoundID.Item1;
				useAnimation = 8;
				useTime = 8;
				noUseGraphic = true;
				noMelee = true;
				value = sellPrice(0, 5);
				knockBack = 3.75f;
				magic = true;
				rare = 4;
				if (Variant == ItemVariants.WeakerVariant)
				{
					value = 5000;
					rare = 1;
					damage = 14;
					useAnimation = 15;
					useTime = 15;
					mana = 5;
					autoReuse = false;
				}
				break;
			case 518:
				autoReuse = true;
				rare = 4;
				mana = 5;
				UseSound = SoundID.Item9;
				noMelee = true;
				useStyle = 5;
				damage = 32;
				useAnimation = 7;
				useTime = 7;
				width = 24;
				height = 28;
				shoot = 94;
				scale = 0.9f;
				shootSpeed = 16f;
				knockBack = 5f;
				magic = true;
				value = sellPrice(0, 4);
				break;
			case 519:
				autoReuse = true;
				rare = 4;
				mana = 9;
				UseSound = SoundID.Item20;
				noMelee = true;
				useStyle = 5;
				damage = 55;
				useAnimation = 15;
				useTime = 15;
				width = 24;
				height = 28;
				shoot = 95;
				scale = 0.9f;
				shootSpeed = 10f;
				knockBack = 6.5f;
				magic = true;
				value = sellPrice(0, 4);
				break;
			case 520:
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				value = 1000;
				rare = 3;
				break;
			case 521:
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				value = 1000;
				rare = 3;
				break;
			case 522:
				width = 12;
				height = 14;
				maxStack = CommonMaxStack;
				value = 4000;
				rare = 3;
				break;
			case 523:
				flame = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 8;
				width = 10;
				height = 12;
				value = 150;
				rare = 1;
				break;
			case 524:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 133;
				width = 44;
				height = 30;
				value = 50000;
				rare = 3;
				break;
			case 525:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 134;
				width = 28;
				height = 14;
				value = 25000;
				rare = 3;
				break;
			case 526:
				width = 14;
				height = 14;
				maxStack = CommonMaxStack;
				value = 15000;
				rare = 1;
				break;
			case 527:
				width = 14;
				height = 14;
				maxStack = CommonMaxStack;
				value = 4500;
				rare = 2;
				break;
			case 528:
				width = 14;
				height = 14;
				maxStack = CommonMaxStack;
				value = 4500;
				rare = 2;
				break;
			case 529:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 135;
				width = 12;
				height = 12;
				placeStyle = 0;
				mech = true;
				value = 5000;
				mech = true;
				break;
			case 530:
				width = 12;
				height = 18;
				maxStack = CommonMaxStack;
				value = 500;
				mech = true;
				break;
			case 531:
				width = 12;
				height = 18;
				maxStack = CommonMaxStack;
				value = 50000;
				rare = 1;
				break;
			case 532:
				width = 20;
				height = 24;
				value = 100000;
				accessory = true;
				rare = 4;
				backSlot = 2;
				break;
			case 533:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 7;
				useTime = 7;
				width = 50;
				height = 18;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item11;
				damage = 25;
				shootSpeed = 10f;
				noMelee = true;
				value = buyPrice(0, 35);
				rare = 5;
				knockBack = 1f;
				ranged = true;
				break;
			case 534:
				knockBack = 6.5f;
				useStyle = 5;
				useAnimation = 45;
				useTime = 45;
				width = 50;
				height = 14;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item36;
				damage = 24;
				shootSpeed = 7f;
				noMelee = true;
				value = 250000;
				rare = 4;
				ranged = true;
				break;
			case 535:
				width = 12;
				height = 18;
				value = 100000;
				accessory = true;
				rare = 4;
				break;
			case 536:
				width = 12;
				height = 18;
				value = 100000;
				rare = 4;
				accessory = true;
				handOnSlot = 15;
				handOffSlot = 8;
				break;
			case 537:
				useStyle = 5;
				useAnimation = 28;
				useTime = 28;
				shootSpeed = 4.3f;
				knockBack = 4f;
				width = 40;
				height = 40;
				damage = 44;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 97;
				rare = 4;
				value = 45000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				break;
			case 538:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 136;
				width = 12;
				height = 12;
				value = 2000;
				mech = true;
				break;
			case 539:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 137;
				width = 12;
				height = 12;
				value = 10000;
				mech = true;
				break;
			case 540:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 138;
				width = 12;
				height = 12;
				mech = true;
				break;
			case 541:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 135;
				width = 12;
				height = 12;
				placeStyle = 1;
				mech = true;
				value = 5000;
				break;
			case 542:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 135;
				width = 12;
				height = 12;
				placeStyle = 2;
				mech = true;
				value = 5000;
				break;
			case 543:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 135;
				width = 12;
				height = 12;
				placeStyle = 3;
				mech = true;
				value = 5000;
				break;
			case 544:
				width = 22;
				height = 14;
				if (Variant != ItemVariants.DisabledBossSummonVariant)
				{
					useStyle = 4;
					consumable = true;
					useAnimation = 45;
					useTime = 45;
				}
				maxStack = CommonMaxStack;
				rare = 3;
				break;
			case 545:
				shootSpeed = 4f;
				shoot = 103;
				damage = 17;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 3f;
				value = 40;
				ranged = true;
				rare = 3;
				break;
			case 546:
				shootSpeed = 5f;
				shoot = 104;
				damage = 12;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 4f;
				value = 30;
				rare = 1;
				ranged = true;
				rare = 3;
				break;
			case 547:
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				value = 40000;
				rare = 5;
				break;
			case 548:
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				value = 40000;
				rare = 5;
				break;
			case 549:
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				value = 40000;
				rare = 5;
				break;
			case 550:
				useStyle = 5;
				useAnimation = 22;
				useTime = 22;
				shootSpeed = 5.6f;
				knockBack = 6.4f;
				width = 40;
				height = 40;
				damage = 61;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 105;
				rare = 5;
				value = 230000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				break;
			case 551:
				width = 18;
				height = 18;
				defense = 15;
				bodySlot = 24;
				rare = 5;
				value = 200000;
				break;
			case 552:
				width = 18;
				height = 18;
				defense = 11;
				legSlot = 23;
				rare = 5;
				value = 150000;
				break;
			case 553:
				width = 18;
				height = 18;
				defense = 9;
				headSlot = 41;
				rare = 5;
				value = 250000;
				break;
			case 558:
				width = 18;
				height = 18;
				defense = 5;
				headSlot = 42;
				rare = 5;
				value = 250000;
				break;
			case 559:
				width = 18;
				height = 18;
				defense = 24;
				headSlot = 43;
				rare = 5;
				value = 250000;
				break;
			case 554:
				width = 20;
				height = 24;
				value = buyPrice(0, 10);
				accessory = true;
				rare = 4;
				neckSlot = 2;
				break;
			case 555:
				width = 20;
				height = 24;
				value = 50000;
				accessory = true;
				rare = 4;
				waistSlot = 6;
				break;
			case 556:
				width = 22;
				height = 14;
				if (Variant != ItemVariants.DisabledBossSummonVariant)
				{
					useStyle = 4;
					consumable = true;
					useAnimation = 45;
					useTime = 45;
				}
				maxStack = CommonMaxStack;
				rare = 3;
				break;
			case 557:
				width = 22;
				height = 14;
				if (Variant != ItemVariants.DisabledBossSummonVariant)
				{
					useStyle = 4;
					consumable = true;
					useAnimation = 45;
					useTime = 45;
				}
				maxStack = CommonMaxStack;
				rare = 3;
				break;
			case 560:
				useStyle = 4;
				width = 22;
				height = 14;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				maxStack = CommonMaxStack;
				rare = 1;
				break;
			case 561:
				melee = true;
				autoReuse = true;
				noMelee = true;
				useStyle = 1;
				shootSpeed = 16f;
				shoot = 106;
				damage = 60;
				knockBack = 8f;
				width = 24;
				height = 24;
				UseSound = SoundID.Item1;
				useAnimation = 14;
				useTime = 14;
				noUseGraphic = true;
				rare = 5;
				value = sellPrice(0, 30);
				break;
			case 562:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 0;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 563:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 1;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 564:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 2;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 565:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 3;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 566:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 4;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 567:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 5;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 568:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 6;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 569:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 7;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 570:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 8;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 571:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 9;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 572:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 10;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 573:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 11;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 574:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 12;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				break;
			case 575:
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				value = 1000;
				rare = 3;
				break;
			case 576:
				width = 24;
				height = 24;
				rare = 3;
				value = 100000;
				accessory = true;
				break;
			case 577:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 140;
				width = 12;
				height = 12;
				break;
			case 578:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 17;
				useTime = 17;
				width = 50;
				height = 18;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 50;
				shootSpeed = 11f;
				noMelee = true;
				value = 200000;
				ranged = true;
				rare = 4;
				knockBack = 2.5f;
				break;
			case 579:
				useStyle = 5;
				useAnimation = 25;
				useTime = 7;
				shootSpeed = 36f;
				knockBack = 4.75f;
				width = 20;
				height = 12;
				damage = 35;
				pick = 200;
				axe = 22;
				UseSound = SoundID.Item23;
				shoot = 107;
				rare = 4;
				value = 220000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				break;
			case 580:
				mech = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 141;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 10);
				break;
			case 581:
				mech = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 142;
				width = 12;
				height = 12;
				break;
			case 582:
				mech = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 143;
				width = 12;
				height = 12;
				break;
			case 583:
				mech = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 144;
				placeStyle = 0;
				width = 10;
				height = 12;
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 1));
				break;
			case 584:
				mech = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 144;
				placeStyle = 1;
				width = 10;
				height = 12;
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 1));
				break;
			case 585:
				mech = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 144;
				placeStyle = 2;
				width = 10;
				height = 12;
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 1));
				break;
			case 586:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 145;
				width = 12;
				height = 12;
				break;
			case 587:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 29;
				width = 12;
				height = 12;
				break;
			case 588:
				width = 18;
				height = 12;
				headSlot = 44;
				value = 150000;
				vanity = true;
				break;
			case 589:
				width = 18;
				height = 18;
				bodySlot = 25;
				value = 150000;
				vanity = true;
				break;
			case 590:
				width = 18;
				height = 18;
				legSlot = 24;
				value = 150000;
				vanity = true;
				break;
			case 591:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 146;
				width = 12;
				height = 12;
				break;
			case 592:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 30;
				width = 12;
				height = 12;
				break;
			case 593:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 147;
				width = 12;
				height = 12;
				break;
			case 594:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 148;
				width = 12;
				height = 12;
				break;
			case 595:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 31;
				width = 12;
				height = 12;
				break;
			case 596:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 149;
				placeStyle = 0;
				width = 12;
				height = 12;
				value = 500;
				break;
			case 597:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 149;
				placeStyle = 1;
				width = 12;
				height = 12;
				value = 500;
				break;
			case 598:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 149;
				placeStyle = 2;
				width = 12;
				height = 12;
				value = 500;
				break;
			case 599:
				width = 12;
				height = 12;
				rare = 1;
				break;
			case 600:
				width = 12;
				height = 12;
				rare = 1;
				break;
			case 601:
				width = 12;
				height = 12;
				rare = 1;
				break;
			case 602:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				width = 28;
				height = 28;
				rare = 2;
				maxStack = CommonMaxStack;
				break;
			case 603:
				damage = 0;
				useStyle = 1;
				shoot = 111;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = 0;
				buffType = 40;
				break;
			case 604:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 150;
				width = 12;
				height = 12;
				break;
			case 605:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 32;
				width = 12;
				height = 12;
				break;
			case 606:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 33;
				width = 12;
				height = 12;
				break;
			case 607:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 151;
				width = 12;
				height = 12;
				break;
			case 608:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 34;
				width = 12;
				height = 12;
				break;
			case 609:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 152;
				width = 12;
				height = 12;
				break;
			case 610:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 35;
				width = 12;
				height = 12;
				break;
			case 611:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 153;
				width = 12;
				height = 12;
				break;
			case 612:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 154;
				width = 12;
				height = 12;
				break;
			case 613:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 155;
				width = 12;
				height = 12;
				break;
			case 614:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 156;
				width = 12;
				height = 12;
				break;
			case 615:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 36;
				width = 12;
				height = 12;
				break;
			case 616:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 37;
				width = 12;
				height = 12;
				break;
			case 617:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 38;
				width = 12;
				height = 12;
				break;
			case 618:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 39;
				width = 12;
				height = 12;
				break;
			case 619:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 157;
				width = 8;
				height = 10;
				break;
			case 620:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 158;
				width = 8;
				height = 10;
				break;
			case 621:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 159;
				width = 8;
				height = 10;
				break;
			case 622:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 41;
				width = 12;
				height = 12;
				break;
			case 623:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 42;
				width = 12;
				height = 12;
				break;
			case 624:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 43;
				width = 12;
				height = 12;
				break;
			case 625:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 7;
				width = 26;
				height = 22;
				value = 500;
				break;
			case 626:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 8;
				width = 26;
				height = 22;
				value = 500;
				break;
			case 627:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 9;
				width = 26;
				height = 22;
				value = 500;
				break;
			case 628:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 2;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 629:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 3;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 630:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 4;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 631:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 1;
				width = 8;
				height = 10;
				break;
			case 632:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 2;
				width = 8;
				height = 10;
				break;
			case 633:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 3;
				width = 8;
				height = 10;
				break;
			case 634:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 4;
				width = 8;
				height = 10;
				break;
			case 635:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 1;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 636:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 2;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 637:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 3;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 638:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 1;
				width = 26;
				height = 20;
				value = 300;
				break;
			case 639:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 2;
				width = 26;
				height = 20;
				value = 300;
				break;
			case 640:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 3;
				width = 26;
				height = 20;
				value = 300;
				break;
			case 641:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 87;
				placeStyle = 1;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 642:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 87;
				placeStyle = 2;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 643:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 87;
				placeStyle = 3;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 644:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 79;
				placeStyle = 1;
				width = 28;
				height = 20;
				value = 2000;
				break;
			case 645:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 79;
				placeStyle = 2;
				width = 28;
				height = 20;
				value = 2000;
				break;
			case 646:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 79;
				placeStyle = 3;
				width = 28;
				height = 20;
				value = 2000;
				break;
			case 647:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 88;
				placeStyle = 1;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 648:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 88;
				placeStyle = 2;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 649:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 88;
				placeStyle = 3;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 650:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 1;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 651:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 2;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 652:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 3;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 653:
				useStyle = 1;
				useTurn = false;
				useAnimation = 19;
				useTime = 19;
				width = 24;
				height = 28;
				damage = 11;
				knockBack = 6f;
				UseSound = SoundID.Item1;
				scale = 1f;
				value = 100;
				melee = true;
				break;
			case 654:
				autoReuse = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 30;
				useTime = 20;
				hammer = 40;
				width = 24;
				height = 28;
				damage = 7;
				knockBack = 5.5f;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				value = 50;
				melee = true;
				break;
			case 655:
				useStyle = 5;
				useAnimation = 28;
				useTime = 28;
				width = 12;
				height = 28;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 8;
				shootSpeed = 6.6f;
				noMelee = true;
				value = 100;
				ranged = true;
				break;
			case 656:
				useStyle = 1;
				useTurn = false;
				useAnimation = 19;
				useTime = 19;
				width = 24;
				height = 28;
				damage = 8;
				knockBack = 6f;
				UseSound = SoundID.Item1;
				scale = 1f;
				value = 100;
				melee = true;
				break;
			case 657:
				autoReuse = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 33;
				useTime = 23;
				hammer = 35;
				width = 24;
				height = 28;
				damage = 4;
				knockBack = 5.5f;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				value = 50;
				melee = true;
				break;
			case 658:
				useStyle = 5;
				useAnimation = 29;
				useTime = 29;
				width = 12;
				height = 28;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 6;
				shootSpeed = 6.6f;
				noMelee = true;
				value = 100;
				ranged = true;
				break;
			case 659:
				useStyle = 1;
				useTurn = false;
				useAnimation = 15;
				useTime = 15;
				width = 24;
				height = 28;
				damage = 30;
				knockBack = 7f;
				UseSound = SoundID.Item1;
				scale = 1f;
				value = 100;
				melee = true;
				autoReuse = true;
				break;
			case 660:
				autoReuse = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 29;
				useTime = 19;
				hammer = 55;
				width = 24;
				height = 28;
				damage = 10;
				knockBack = 5.5f;
				scale = 1.25f;
				UseSound = SoundID.Item1;
				value = 50;
				melee = true;
				break;
			case 661:
				useStyle = 5;
				useAnimation = 20;
				useTime = 20;
				width = 12;
				height = 28;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 12;
				shootSpeed = 7f;
				noMelee = true;
				value = 100;
				ranged = true;
				break;
			case 662:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 160;
				width = 12;
				height = 12;
				rare = 1;
				break;
			case 663:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 44;
				width = 12;
				height = 12;
				rare = 1;
				break;
			case 664:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 161;
				width = 12;
				height = 12;
				break;
			case 665:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 3;
				value = 400000;
				break;
			case 666:
				width = 18;
				height = 18;
				headSlot = 45;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 667:
				width = 18;
				height = 18;
				bodySlot = 26;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 668:
				width = 18;
				height = 18;
				legSlot = 25;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 669:
				damage = 0;
				useStyle = 1;
				shoot = 112;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				buffType = 41;
				value = sellPrice(0, 2);
				break;
			case 670:
				crit = 2;
				noMelee = true;
				useStyle = 1;
				shootSpeed = 11.5f;
				shoot = 113;
				damage = 21;
				knockBack = 8.5f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				rare = 1;
				value = 50000;
				melee = true;
				break;
			case 671:
				crit = 16;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 20;
				useTime = 20;
				knockBack = 6.5f;
				width = 40;
				height = 40;
				damage = 105;
				scale = 1.25f;
				UseSound = SoundID.Item1;
				rare = 8;
				value = buyPrice(0, 20);
				melee = true;
				if (Variant == ItemVariants.WeakerVariant)
				{
					rare = 1;
					value = buyPrice(0, 10);
					damage = 15;
				}
				break;
			case 672:
				useStyle = 1;
				useTime = 16;
				useAnimation = 16;
				knockBack = 4f;
				width = 24;
				height = 28;
				damage = 53;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 180000;
				melee = true;
				autoReuse = true;
				useTurn = true;
				break;
			case 673:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 23;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 674:
				useStyle = 1;
				useAnimation = 18;
				useTime = 18;
				autoReuse = true;
				shoot = 983;
				shootSpeed = 11f;
				knockBack = 4.5f;
				width = 40;
				height = 40;
				damage = 72;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 8;
				value = sellPrice(0, 10);
				melee = true;
				noMelee = true;
				shootsEveryUse = true;
				break;
			case 675:
				useStyle = 1;
				useAnimation = 32;
				useTime = 32;
				autoReuse = true;
				shoot = 973;
				shootSpeed = 14f;
				knockBack = 4.75f;
				width = 40;
				height = 40;
				damage = 70;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 8;
				value = sellPrice(0, 10);
				melee = true;
				noMelee = true;
				shootsEveryUse = true;
				break;
			case 676:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 23;
				useTime = 30;
				knockBack = 4.5f;
				width = 24;
				height = 28;
				damage = 49;
				scale = 1.15f;
				UseSound = SoundID.Item1;
				rare = 5;
				shoot = 119;
				shootSpeed = 12f;
				value = 250000;
				melee = true;
				break;
			case 677:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 28;
				width = 26;
				height = 20;
				value = 300;
				break;
			case 678:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				rare = 4;
				break;
			case 679:
				autoReuse = true;
				knockBack = 7f;
				useStyle = 5;
				useAnimation = 34;
				useTime = 34;
				width = 50;
				height = 14;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item38;
				damage = 29;
				shootSpeed = 6f;
				noMelee = true;
				value = buyPrice(0, 40);
				rare = 8;
				ranged = true;
				break;
			case 680:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 10;
				width = 26;
				height = 22;
				value = 5000;
				break;
			case 681:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 11;
				width = 26;
				height = 22;
				value = 5000;
				break;
			case 682:
				useStyle = 5;
				useAnimation = 19;
				useTime = 19;
				width = 14;
				height = 32;
				shoot = 1;
				autoReuse = true;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 53;
				shootSpeed = 11f;
				knockBack = 4.7f;
				rare = 5;
				crit = 5;
				noMelee = true;
				scale = 1.1f;
				value = 27000;
				ranged = true;
				break;
			case 683:
				autoReuse = true;
				rare = 6;
				mana = 19;
				UseSound = SoundID.Item20;
				noMelee = true;
				useStyle = 5;
				damage = 88;
				useAnimation = 17;
				useTime = 17;
				width = 30;
				height = 30;
				shoot = 114;
				shootSpeed = 13f;
				knockBack = 6.5f;
				magic = true;
				value = 500000;
				if (Variant == ItemVariants.WeakerVariant)
				{
					value = hellPrice;
					rare = 3;
					damage = 42;
					mana = 9;
				}
				break;
			case 684:
				width = 18;
				height = 18;
				defense = 10;
				headSlot = 46;
				rare = 5;
				value = 250000;
				break;
			case 685:
				width = 18;
				height = 18;
				defense = 20;
				bodySlot = 27;
				rare = 5;
				value = 200000;
				break;
			case 686:
				width = 18;
				height = 18;
				defense = 13;
				legSlot = 26;
				rare = 5;
				value = 150000;
				break;
			case 687:
				width = 18;
				height = 18;
				defense = 2;
				headSlot = 47;
				value = 1125;
				break;
			case 688:
				width = 18;
				height = 18;
				defense = 2;
				bodySlot = 28;
				value = 1875;
				break;
			case 689:
				width = 18;
				height = 18;
				defense = 1;
				legSlot = 27;
				value = 1500;
				break;
			case 690:
				width = 18;
				height = 18;
				defense = 3;
				headSlot = 48;
				value = 4500;
				break;
			case 691:
				width = 18;
				height = 18;
				defense = 3;
				bodySlot = 29;
				value = 7500;
				break;
			case 692:
				width = 18;
				height = 18;
				defense = 2;
				legSlot = 28;
				value = 6000;
				break;
			case 693:
				width = 18;
				height = 18;
				defense = 4;
				headSlot = 49;
				value = 11250;
				break;
			case 694:
				width = 18;
				height = 18;
				defense = 5;
				bodySlot = 30;
				value = 18750;
				break;
			case 695:
				width = 18;
				height = 18;
				defense = 3;
				legSlot = 29;
				value = 15000;
				break;
			case 696:
				width = 18;
				height = 18;
				defense = 5;
				headSlot = 50;
				value = 22500;
				break;
			case 697:
				width = 18;
				height = 18;
				defense = 6;
				bodySlot = 31;
				value = 37500;
				break;
			case 698:
				width = 18;
				height = 18;
				defense = 5;
				legSlot = 30;
				value = 30000;
				break;
			case 699:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 166;
				width = 12;
				height = 12;
				value = 375;
				break;
			case 700:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 167;
				width = 12;
				height = 12;
				value = 750;
				break;
			case 701:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 168;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 2, 25);
				break;
			case 702:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 169;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 4, 50);
				break;
			case 703:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 1125;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 1;
				break;
			case 704:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 2250;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 3;
				break;
			case 705:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 4500;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 5;
				break;
			case 706:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 9000;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 7;
				break;
			case 707:
				width = 24;
				height = 28;
				accessory = true;
				value = 1500;
				waistSlot = 8;
				break;
			case 708:
				width = 24;
				height = 28;
				accessory = true;
				value = 7500;
				waistSlot = 9;
				break;
			case 709:
				width = 24;
				height = 28;
				accessory = true;
				rare = 1;
				value = 15000;
				waistSlot = 4;
				break;
			case 710:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				placeStyle = 3;
				width = 26;
				height = 26;
				value = 4500;
				break;
			case 711:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				placeStyle = 4;
				width = 26;
				height = 26;
				value = 18000;
				break;
			case 712:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				placeStyle = 5;
				width = 26;
				height = 26;
				value = 36000;
				break;
			case 713:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 174;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 3);
				holdStyle = 1;
				break;
			case 714:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 173;
				width = 20;
				height = 20;
				value = buyPrice(0, 0, 15);
				break;
			case 715:
				width = 18;
				height = 18;
				headSlot = 51;
				value = 15000;
				vanity = true;
				break;
			case 716:
				placeStyle = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 16;
				width = 28;
				height = 14;
				value = 7500;
				break;
			case 717:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 175;
				width = 12;
				height = 12;
				break;
			case 718:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 176;
				width = 12;
				height = 12;
				break;
			case 719:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 177;
				width = 12;
				height = 12;
				break;
			case 720:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 45;
				width = 12;
				height = 12;
				break;
			case 721:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 46;
				width = 12;
				height = 12;
				break;
			case 722:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 47;
				width = 12;
				height = 12;
				break;
			case 723:
				rare = 4;
				UseSound = SoundID.Item1;
				useStyle = 1;
				damage = 52;
				scale = 1.3f;
				useAnimation = 20;
				useTime = 35;
				autoReuse = true;
				width = 30;
				height = 30;
				shoot = 116;
				shootSpeed = 11f;
				knockBack = 6.5f;
				melee = true;
				value = sellPrice(0, 3);
				break;
			case 724:
				autoReuse = true;
				crit = 2;
				rare = 1;
				UseSound = SoundID.Item1;
				useStyle = 1;
				damage = 17;
				useAnimation = 20;
				useTime = 55;
				width = 30;
				height = 30;
				shoot = 118;
				shootSpeed = 9.5f;
				knockBack = 4.75f;
				melee = true;
				value = 20000;
				break;
			case 725:
				useStyle = 5;
				useAnimation = 14;
				useTime = 14;
				width = 12;
				height = 28;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 39;
				shootSpeed = 10f;
				knockBack = 4.5f;
				alpha = 30;
				rare = 5;
				noMelee = true;
				value = sellPrice(0, 5);
				ranged = true;
				autoReuse = true;
				if (Variant == ItemVariants.WeakerVariant)
				{
					value = 100000;
					rare = 1;
					damage = 8;
					useAnimation = 17;
					useTime = 17;
				}
				break;
			case 726:
				autoReuse = true;
				rare = 5;
				mana = 12;
				UseSound = SoundID.Item20;
				useStyle = 5;
				damage = 46;
				useAnimation = 12;
				useTime = 12;
				width = 30;
				height = 30;
				shoot = 359;
				shootSpeed = 16f;
				knockBack = 5f;
				magic = true;
				value = sellPrice(0, 4);
				noMelee = true;
				break;
			case 727:
				width = 18;
				height = 18;
				defense = 1;
				headSlot = 52;
				break;
			case 728:
				width = 18;
				height = 18;
				defense = 1;
				bodySlot = 32;
				break;
			case 729:
				width = 18;
				height = 18;
				defense = 0;
				legSlot = 31;
				break;
			case 730:
				width = 18;
				height = 18;
				defense = 1;
				headSlot = 53;
				break;
			case 731:
				width = 18;
				height = 18;
				defense = 2;
				bodySlot = 33;
				break;
			case 732:
				width = 18;
				height = 18;
				defense = 1;
				legSlot = 32;
				break;
			case 733:
				width = 18;
				height = 18;
				defense = 1;
				headSlot = 54;
				break;
			case 734:
				width = 18;
				height = 18;
				defense = 1;
				bodySlot = 34;
				break;
			case 735:
				width = 18;
				height = 18;
				defense = 1;
				legSlot = 33;
				break;
			case 736:
				width = 18;
				height = 18;
				defense = 2;
				headSlot = 55;
				break;
			case 737:
				width = 18;
				height = 18;
				defense = 3;
				bodySlot = 35;
				break;
			case 738:
				width = 18;
				height = 18;
				defense = 2;
				legSlot = 34;
				break;
			case 739:
				mana = 5;
				UseSound = SoundID.Item43;
				useStyle = 5;
				damage = 15;
				useAnimation = 37;
				useTime = 37;
				width = 40;
				height = 40;
				shoot = 121;
				shootSpeed = 6f;
				knockBack = 3.25f;
				value = 2000;
				magic = true;
				noMelee = true;
				break;
			case 740:
				mana = 5;
				UseSound = SoundID.Item43;
				useStyle = 5;
				damage = 16;
				useAnimation = 36;
				useTime = 36;
				width = 40;
				height = 40;
				shoot = 122;
				shootSpeed = 6.5f;
				knockBack = 3.5f;
				value = 3000;
				magic = true;
				noMelee = true;
				break;
			case 741:
				mana = 6;
				UseSound = SoundID.Item43;
				useStyle = 5;
				damage = 18;
				useAnimation = 34;
				useTime = 34;
				width = 40;
				height = 40;
				shoot = 123;
				shootSpeed = 7.5f;
				knockBack = 4f;
				value = 10000;
				magic = true;
				autoReuse = true;
				rare = 1;
				noMelee = true;
				break;
			case 742:
				mana = 6;
				UseSound = SoundID.Item43;
				useStyle = 5;
				damage = 19;
				useAnimation = 32;
				useTime = 32;
				width = 40;
				height = 40;
				shoot = 124;
				shootSpeed = 8f;
				knockBack = 4.25f;
				magic = true;
				autoReuse = true;
				value = 15000;
				rare = 1;
				noMelee = true;
				break;
			case 743:
				mana = 7;
				UseSound = SoundID.Item43;
				useStyle = 5;
				damage = 21;
				useAnimation = 28;
				useTime = 28;
				width = 40;
				height = 40;
				shoot = 125;
				shootSpeed = 9f;
				knockBack = 4.75f;
				magic = true;
				autoReuse = true;
				value = 20000;
				rare = 1;
				noMelee = true;
				break;
			case 744:
				mana = 8;
				UseSound = SoundID.Item43;
				useStyle = 5;
				damage = 23;
				useAnimation = 26;
				useTime = 26;
				width = 40;
				height = 40;
				shoot = 126;
				shootSpeed = 9.5f;
				knockBack = 5.5f;
				magic = true;
				autoReuse = true;
				value = 30000;
				rare = 2;
				noMelee = true;
				break;
			case 745:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 66;
				width = 12;
				height = 12;
				value = 10;
				break;
			case 746:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 67;
				width = 12;
				height = 12;
				value = 10;
				break;
			case 747:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 68;
				width = 12;
				height = 12;
				value = 10;
				break;
			case 748:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 4;
				break;
			case 749:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 5;
				break;
			case 750:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 72;
				width = 12;
				height = 12;
				break;
			case 751:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 189;
				width = 12;
				height = 12;
				break;
			case 752:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 73;
				width = 12;
				height = 12;
				break;
			case 753:
				damage = 0;
				useStyle = 1;
				shoot = 127;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 2);
				buffType = 42;
				break;
			case 754:
				width = 28;
				height = 20;
				headSlot = 56;
				rare = 5;
				value = 50000;
				vanity = true;
				break;
			case 755:
				width = 18;
				height = 14;
				bodySlot = 36;
				value = 50000;
				vanity = true;
				rare = 5;
				break;
			case 756:
				rare = 7;
				useStyle = 5;
				useAnimation = 40;
				useTime = 40;
				shootSpeed = 5.5f;
				knockBack = 6.2f;
				width = 32;
				height = 32;
				damage = 60;
				scale = 1f;
				UseSound = SoundID.Item1;
				shoot = 130;
				value = buyPrice(0, 70);
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				break;
			case 757:
				rare = 8;
				UseSound = SoundID.Item1;
				useStyle = 1;
				damage = 85;
				useAnimation = 18;
				useTime = 18;
				width = 30;
				height = 30;
				shoot = 985;
				scale = 1f;
				shootSpeed = 12f;
				knockBack = 6.5f;
				melee = true;
				value = sellPrice(0, 20);
				autoReuse = true;
				noMelee = true;
				shootsEveryUse = true;
				break;
			case 758:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 20;
				useTime = 20;
				useAmmo = AmmoID.Rocket;
				width = 50;
				height = 20;
				shoot = 133;
				UseSound = SoundID.Item61;
				damage = 60;
				shootSpeed = 10f;
				noMelee = true;
				value = buyPrice(0, 35);
				knockBack = 4f;
				rare = 8;
				ranged = true;
				break;
			case 759:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 30;
				useTime = 30;
				useAmmo = AmmoID.Rocket;
				width = 50;
				height = 20;
				shoot = 134;
				UseSound = SoundID.Item11;
				damage = 55;
				shootSpeed = 5f;
				noMelee = true;
				value = buyPrice(0, 40);
				knockBack = 4f;
				rare = 8;
				ranged = true;
				break;
			case 760:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 50;
				useTime = 50;
				useAmmo = AmmoID.Rocket;
				width = 50;
				height = 20;
				shoot = 135;
				UseSound = SoundID.Item11;
				damage = 80;
				shootSpeed = 12f;
				noMelee = true;
				value = buyPrice(0, 35);
				knockBack = 4f;
				rare = 8;
				ranged = true;
				break;
			case 761:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 6;
				break;
			case 762:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 193;
				width = 12;
				height = 12;
				break;
			case 763:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 195;
				width = 12;
				height = 12;
				break;
			case 764:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 74;
				width = 12;
				height = 12;
				break;
			case 765:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 196;
				width = 12;
				height = 12;
				break;
			case 766:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 194;
				width = 12;
				height = 12;
				break;
			case 767:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 197;
				width = 12;
				height = 12;
				break;
			case 768:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 75;
				width = 12;
				height = 12;
				break;
			case 769:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 76;
				width = 12;
				height = 12;
				break;
			case 770:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 77;
				width = 12;
				height = 12;
				break;
			case 771:
				shoot = 0;
				damage = 40;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 4f;
				value = buyPrice(0, 0, 0, 50);
				ranged = true;
				break;
			case 772:
				shoot = 3;
				damage = 40;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 4f;
				value = buyPrice(0, 0, 2, 50);
				ranged = true;
				rare = 1;
				break;
			case 773:
				shoot = 6;
				damage = 65;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 6f;
				value = buyPrice(0, 0, 1);
				ranged = true;
				rare = 1;
				break;
			case 774:
				shoot = 9;
				damage = 65;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 6f;
				value = (value = buyPrice(0, 0, 5));
				ranged = true;
				rare = 2;
				break;
			case 775:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 198;
				width = 12;
				height = 12;
				break;
			case 776:
				useStyle = 1;
				useTurn = true;
				autoReuse = true;
				useAnimation = 25;
				useTime = 13;
				knockBack = 5f;
				width = 20;
				height = 12;
				damage = 10;
				pick = 110;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 54000;
				melee = true;
				scale = 1.15f;
				break;
			case 777:
				useStyle = 1;
				useAnimation = 25;
				useTime = 10;
				knockBack = 5f;
				useTurn = true;
				autoReuse = true;
				width = 20;
				height = 12;
				damage = 15;
				pick = 150;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 81000;
				melee = true;
				scale = 1.15f;
				break;
			case 778:
				useStyle = 1;
				useAnimation = 25;
				useTime = 8;
				knockBack = 5f;
				useTurn = true;
				autoReuse = true;
				width = 20;
				height = 12;
				damage = 20;
				pick = 180;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 108000;
				melee = true;
				scale = 1.15f;
				break;
			case 779:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 30;
				useTime = 5;
				width = 50;
				height = 18;
				shoot = 145;
				useAmmo = AmmoID.Solution;
				UseSound = SoundID.Item34;
				knockBack = 0.3f;
				shootSpeed = 7f;
				noMelee = true;
				value = buyPrice(2);
				rare = 5;
				break;
			case 780:
				shoot = 0;
				ammo = AmmoID.Solution;
				width = 10;
				height = 12;
				value = buyPrice(0, 0, 15);
				rare = 3;
				maxStack = CommonMaxStack;
				consumable = true;
				break;
			case 781:
				shoot = 1;
				ammo = AmmoID.Solution;
				width = 10;
				height = 12;
				value = buyPrice(0, 0, 15);
				rare = 3;
				maxStack = CommonMaxStack;
				consumable = true;
				break;
			case 782:
				shoot = 2;
				ammo = AmmoID.Solution;
				width = 10;
				height = 12;
				value = buyPrice(0, 0, 15);
				rare = 3;
				maxStack = CommonMaxStack;
				consumable = true;
				break;
			case 783:
				shoot = 3;
				ammo = AmmoID.Solution;
				width = 10;
				height = 12;
				value = buyPrice(0, 0, 15);
				rare = 3;
				maxStack = CommonMaxStack;
				consumable = true;
				break;
			case 784:
				shoot = 4;
				ammo = AmmoID.Solution;
				width = 10;
				height = 12;
				value = buyPrice(0, 0, 15);
				rare = 3;
				maxStack = CommonMaxStack;
				consumable = true;
				break;
			case 785:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 7;
				break;
			case 786:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 8;
				break;
			case 787:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 27;
				useTime = 14;
				hammer = 85;
				width = 24;
				height = 28;
				damage = 26;
				knockBack = 7.5f;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				rare = 7;
				value = buyPrice(0, 40);
				melee = true;
				break;
			case 788:
				mana = 12;
				damage = 35;
				useStyle = 5;
				shootSpeed = 32f;
				shoot = 150;
				width = 26;
				height = 28;
				useAnimation = 25;
				useTime = 25;
				autoReuse = true;
				rare = 7;
				noMelee = true;
				knockBack = 1f;
				value = 200000;
				magic = true;
				break;
			case 789:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 4;
				width = 10;
				height = 24;
				value = 5000;
				break;
			case 790:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 5;
				width = 10;
				height = 24;
				value = 5000;
				break;
			case 791:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 6;
				width = 10;
				height = 24;
				value = 5000;
				break;
			case 792:
				width = 18;
				height = 18;
				defense = 6;
				headSlot = 57;
				value = 50000;
				rare = 1;
				break;
			case 793:
				width = 18;
				height = 18;
				defense = 7;
				bodySlot = 37;
				value = 40000;
				rare = 1;
				break;
			case 794:
				width = 18;
				height = 18;
				defense = 6;
				legSlot = 35;
				value = 30000;
				rare = 1;
				break;
			case 795:
				useStyle = 1;
				useTime = 25;
				useAnimation = 25;
				knockBack = 5f;
				width = 24;
				height = 28;
				damage = 22;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = 13500;
				melee = true;
				break;
			case 796:
				useStyle = 5;
				useAnimation = 30;
				useTime = 30;
				width = 12;
				height = 28;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 19;
				shootSpeed = 6.7f;
				knockBack = 1f;
				alpha = 30;
				rare = 1;
				noMelee = true;
				value = 18000;
				ranged = true;
				break;
			case 797:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 40;
				useTime = 19;
				hammer = 55;
				width = 24;
				height = 28;
				damage = 23;
				knockBack = 6f;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = 15000;
				melee = true;
				break;
			case 798:
				useStyle = 1;
				useTurn = true;
				useAnimation = 22;
				useTime = 14;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 12;
				pick = 70;
				UseSound = SoundID.Item1;
				knockBack = 3.5f;
				rare = 1;
				value = 18000;
				scale = 1.15f;
				melee = true;
				break;
			case 799:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 32;
				knockBack = 6f;
				useTime = 15;
				width = 24;
				height = 28;
				damage = 22;
				axe = 15;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = 13500;
				melee = true;
				break;
			case 800:
				useStyle = 5;
				useAnimation = 20;
				useTime = 20;
				width = 24;
				height = 28;
				shoot = 14;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item11;
				damage = 22;
				shootSpeed = 6f;
				noMelee = true;
				knockBack = 2f;
				value = shadowOrbPrice;
				scale = 0.9f;
				rare = 1;
				ranged = true;
				break;
			case 801:
				useStyle = 5;
				useAnimation = 45;
				useTime = 45;
				knockBack = 5.5f;
				width = 30;
				height = 10;
				damage = 17;
				scale = 1.1f;
				noUseGraphic = true;
				shoot = 154;
				shootSpeed = 12f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = 27000;
				melee = true;
				channel = true;
				noMelee = true;
				break;
			case 802:
				useStyle = 5;
				useAnimation = 31;
				useTime = 31;
				shootSpeed = 4f;
				knockBack = 5f;
				width = 40;
				height = 40;
				damage = 17;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 153;
				rare = 1;
				value = shadowOrbPrice;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				break;
			case 803:
				width = 18;
				height = 18;
				headSlot = 58;
				value = 50000;
				defense = 3;
				break;
			case 804:
				width = 18;
				height = 18;
				bodySlot = 38;
				value = 40000;
				defense = 3;
				break;
			case 805:
				width = 18;
				height = 18;
				legSlot = 36;
				value = 30000;
				defense = 3;
				break;
			case 806:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 5;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 807:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 6;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 808:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 7;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 809:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 8;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 810:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 9;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 811:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 4;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 812:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 5;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 813:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 6;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 814:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 7;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 815:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 8;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 816:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 4;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 817:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 5;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 818:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 6;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 819:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 7;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 820:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 8;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 821:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 9;
				break;
			case 822:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 10;
				break;
			case 823:
				color = new Color(255, 255, 255, 0);
				alpha = 255;
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 8;
				wingSlot = 11;
				break;
			case 824:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 202;
				width = 12;
				height = 12;
				break;
			case 825:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 82;
				width = 12;
				height = 12;
				break;
			case 826:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 10;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 827:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 4;
				width = 26;
				height = 20;
				value = 300;
				break;
			case 828:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 5;
				width = 26;
				height = 20;
				value = 300;
				break;
			case 829:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 6;
				width = 26;
				height = 20;
				value = 300;
				break;
			case 830:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 7;
				width = 26;
				height = 20;
				value = 300;
				break;
			case 831:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 12;
				width = 26;
				height = 22;
				value = 5000;
				break;
			case 832:
				tileWand = 9;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				createTile = 191;
				width = 8;
				height = 10;
				rare = 1;
				value = sellPrice(0, 0, 25);
				break;
			case 833:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 163;
				width = 12;
				height = 12;
				break;
			case 834:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 164;
				width = 12;
				height = 12;
				break;
			case 835:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 200;
				width = 12;
				height = 12;
				break;
			case 836:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 203;
				width = 12;
				height = 12;
				break;
			case 837:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 9;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 838:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 13;
				width = 26;
				height = 22;
				value = 5000;
				break;
			case 839:
				width = 28;
				height = 20;
				headSlot = 59;
				rare = 2;
				vanity = true;
				value = buyPrice(0, 1, 50);
				break;
			case 840:
				width = 18;
				height = 14;
				bodySlot = 39;
				rare = 2;
				vanity = true;
				value = buyPrice(0, 1, 50);
				break;
			case 841:
				width = 18;
				height = 14;
				legSlot = 37;
				rare = 2;
				vanity = true;
				value = buyPrice(0, 1, 50);
				break;
			case 842:
				width = 28;
				height = 20;
				headSlot = 60;
				rare = 1;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 843:
				width = 18;
				height = 14;
				bodySlot = 40;
				rare = 1;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 844:
				width = 18;
				height = 14;
				legSlot = 38;
				rare = 1;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 845:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 7;
				width = 10;
				height = 24;
				value = 5000;
				break;
			case 846:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 8;
				width = 10;
				height = 24;
				value = 5000;
				break;
			case 847:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 9;
				width = 10;
				height = 24;
				value = 5000;
				break;
			case 848:
				width = 28;
				height = 20;
				headSlot = 61;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 2);
				break;
			case 849:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				width = 24;
				height = 28;
				maxStack = CommonMaxStack;
				mech = true;
				value = buyPrice(0, 0, 10);
				break;
			case 850:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 5;
				autoReuse = true;
				width = 24;
				height = 28;
				rare = 1;
				value = 20000;
				mech = true;
				tileBoost = 20;
				break;
			case 851:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 5;
				autoReuse = true;
				width = 24;
				height = 28;
				rare = 1;
				value = 20000;
				mech = true;
				tileBoost = 20;
				break;
			case 852:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 135;
				width = 12;
				height = 12;
				placeStyle = 4;
				mech = true;
				value = 5000;
				break;
			case 853:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 135;
				width = 12;
				height = 12;
				placeStyle = 5;
				mech = true;
				value = 5000;
				break;
			case 854:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = 50000;
				break;
			case 855:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = 50000;
				break;
			case 856:
				noWet = true;
				holdStyle = 1;
				width = 30;
				height = 30;
				value = 500;
				rare = 2;
				vanity = true;
				break;
			case 857:
				width = 16;
				height = 24;
				accessory = true;
				rare = 2;
				value = 50000;
				waistSlot = 15;
				break;
			case 858:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 89;
				placeStyle = 24;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 859:
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 155;
				width = 44;
				height = 44;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				noMelee = true;
				value = 20;
				break;
			case 860:
				width = 16;
				height = 24;
				accessory = true;
				rare = 6;
				lifeRegen = 2;
				value = buyPrice(0, 20);
				handOnSlot = 4;
				break;
			case 861:
				width = 16;
				height = 24;
				accessory = true;
				rare = 6;
				value = buyPrice(0, 40);
				hasVanityEffects = true;
				break;
			case 862:
				width = 16;
				height = 24;
				accessory = true;
				rare = 6;
				value = buyPrice(0, 10);
				neckSlot = 5;
				break;
			case 863:
				width = 16;
				height = 24;
				accessory = true;
				rare = 4;
				value = 200000;
				shoeSlot = 2;
				break;
			case 864:
				width = 28;
				height = 20;
				headSlot = 62;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 25);
				break;
			case 865:
				width = 18;
				height = 14;
				bodySlot = 41;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 10);
				break;
			case 866:
				width = 18;
				height = 14;
				bodySlot = 42;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 2);
				break;
			case 867:
				width = 28;
				height = 20;
				headSlot = 63;
				rare = 1;
				defense = 2;
				value = sellPrice(0, 0, 50);
				break;
			case 868:
				width = 28;
				height = 20;
				headSlot = 64;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 2);
				break;
			case 869:
				width = 28;
				height = 20;
				headSlot = 65;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 2, 50);
				break;
			case 870:
				width = 28;
				height = 20;
				headSlot = 66;
				rare = 1;
				value = buyPrice(0, 2);
				vanity = true;
				break;
			case 871:
				width = 28;
				height = 20;
				bodySlot = 43;
				rare = 1;
				value = buyPrice(0, 2);
				vanity = true;
				break;
			case 872:
				width = 28;
				height = 20;
				legSlot = 39;
				rare = 1;
				value = buyPrice(0, 2);
				vanity = true;
				break;
			case 873:
				width = 28;
				height = 20;
				headSlot = 67;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 5);
				break;
			case 874:
				width = 28;
				height = 20;
				bodySlot = 44;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 5);
				break;
			case 875:
				width = 28;
				height = 20;
				legSlot = 40;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 5);
				break;
			case 876:
				width = 28;
				height = 20;
				headSlot = 68;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 5);
				break;
			case 877:
				width = 28;
				height = 20;
				bodySlot = 45;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 5);
				break;
			case 878:
				width = 28;
				height = 20;
				legSlot = 41;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 5);
				break;
			case 879:
				width = 28;
				height = 20;
				headSlot = 69;
				rare = 1;
				defense = 4;
				value = sellPrice(0, 0, 50);
				break;
			case 880:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 204;
				width = 12;
				height = 12;
				rare = 1;
				value = sellPrice(0, 0, 13);
				break;
			case 881:
				useStyle = 1;
				useTurn = false;
				useAnimation = 30;
				useTime = 30;
				width = 24;
				height = 28;
				damage = 10;
				knockBack = 4.5f;
				UseSound = SoundID.Item1;
				scale = 1f;
				value = 1800;
				melee = true;
				break;
			case 882:
				useStyle = 1;
				useTurn = true;
				useAnimation = 25;
				useTime = 16;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 4;
				pick = 35;
				UseSound = SoundID.Item1;
				knockBack = 2f;
				value = 2000;
				melee = true;
				break;
			case 883:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 206;
				width = 12;
				height = 12;
				break;
			case 884:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 84;
				width = 12;
				height = 12;
				break;
			case 885:
				width = 16;
				height = 24;
				accessory = true;
				rare = 4;
				value = 100000;
				break;
			case 886:
				width = 16;
				height = 24;
				accessory = true;
				rare = 4;
				value = 100000;
				break;
			case 887:
				width = 16;
				height = 24;
				accessory = true;
				rare = 4;
				value = 100000;
				break;
			case 888:
				width = 16;
				height = 24;
				accessory = true;
				rare = 4;
				value = 100000;
				faceSlot = 5;
				break;
			case 889:
				width = 16;
				height = 24;
				accessory = true;
				rare = 4;
				value = 100000;
				break;
			case 890:
				width = 16;
				height = 24;
				accessory = true;
				rare = 4;
				value = 100000;
				break;
			case 891:
				width = 16;
				height = 24;
				accessory = true;
				rare = 2;
				value = 100000;
				break;
			case 892:
				width = 16;
				height = 24;
				accessory = true;
				rare = 4;
				value = 100000;
				break;
			case 893:
				width = 16;
				height = 24;
				accessory = true;
				rare = 4;
				value = 100000;
				break;
			case 894:
				width = 18;
				height = 18;
				defense = 1;
				headSlot = 70;
				value = sellPrice(0, 0, 0, 40);
				break;
			case 895:
				width = 18;
				height = 18;
				defense = 1;
				bodySlot = 46;
				value = sellPrice(0, 0, 0, 60);
				break;
			case 896:
				width = 18;
				height = 18;
				defense = 1;
				legSlot = 42;
				value = sellPrice(0, 0, 0, 50);
				break;
			case 897:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = buyPrice(0, 20);
				handOffSlot = 5;
				handOnSlot = 10;
				break;
			case 898:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = 300000;
				shoeSlot = 10;
				break;
			case 899:
				width = 16;
				height = 24;
				accessory = true;
				rare = 7;
				value = 300000;
				handOnSlot = 13;
				break;
			case 900:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = eclipsePrice;
				handOnSlot = 14;
				break;
			case 901:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = 100000;
				break;
			case 902:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = 100000;
				break;
			case 903:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = 100000;
				break;
			case 904:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = 100000;
				break;
			case 905:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 8;
				useTime = 8;
				width = 50;
				height = 18;
				shoot = 158;
				useAmmo = AmmoID.Coin;
				UseSound = SoundID.Item11;
				damage = 0;
				shootSpeed = 10f;
				noMelee = true;
				value = 300000;
				rare = 6;
				knockBack = 2f;
				ranged = true;
				break;
			case 906:
				width = 16;
				height = 24;
				accessory = true;
				rare = 3;
				value = 300000;
				break;
			case 907:
				width = 16;
				height = 24;
				accessory = true;
				rare = 4;
				value = buyPrice(0, 30);
				shoeSlot = 11;
				break;
			case 908:
				width = 16;
				height = 24;
				accessory = true;
				rare = 7;
				value = 500000;
				shoeSlot = 8;
				break;
			case 909:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 207;
				placeStyle = 0;
				width = 26;
				height = 36;
				value = buyPrice(0, 4);
				rare = 1;
				break;
			case 910:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 207;
				placeStyle = 1;
				width = 26;
				height = 36;
				value = buyPrice(0, 4);
				rare = 1;
				break;
			case 911:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 208;
				width = 8;
				height = 10;
				break;
			case 912:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 10;
				width = 14;
				height = 28;
				value = 200;
				break;
			case 913:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 5;
				width = 8;
				height = 10;
				break;
			case 914:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 14;
				width = 26;
				height = 22;
				value = 500;
				break;
			case 915:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 11;
				width = 12;
				height = 30;
				value = 150;
				break;
			case 916:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 9;
				width = 28;
				height = 14;
				value = 150;
				break;
			case 917:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 8;
				width = 26;
				height = 20;
				value = 300;
				break;
			case 918:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 88;
				placeStyle = 4;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 919:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 87;
				placeStyle = 4;
				width = 20;
				height = 20;
				value = 300;
				break;
			case 920:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 79;
				placeStyle = 4;
				width = 28;
				height = 20;
				value = 2000;
				break;
			case 921:
				useStyle = 1;
				useTurn = false;
				useAnimation = 19;
				useTime = 19;
				width = 24;
				height = 28;
				damage = 11;
				knockBack = 6f;
				UseSound = SoundID.Item1;
				scale = 1f;
				value = 100;
				melee = true;
				break;
			case 922:
				autoReuse = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 30;
				useTime = 20;
				hammer = 40;
				width = 24;
				height = 28;
				damage = 7;
				knockBack = 5.5f;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				value = 50;
				melee = true;
				break;
			case 923:
				useStyle = 5;
				useAnimation = 28;
				useTime = 28;
				width = 12;
				height = 28;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 8;
				shootSpeed = 6.6f;
				noMelee = true;
				value = 100;
				ranged = true;
				break;
			case 924:
				width = 18;
				height = 18;
				defense = 1;
				headSlot = 71;
				break;
			case 925:
				width = 18;
				height = 18;
				defense = 2;
				bodySlot = 47;
				break;
			case 926:
				width = 18;
				height = 18;
				defense = 1;
				legSlot = 43;
				break;
			case 927:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 85;
				width = 12;
				height = 12;
				break;
			case 928:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 209;
				width = 12;
				height = 12;
				rare = 3;
				value = buyPrice(0, 25);
				break;
			case 929:
				useStyle = 1;
				useTurn = true;
				useAnimation = 20;
				useTime = 20;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				damage = 300;
				noMelee = true;
				value = buyPrice(0, 0, 15);
				break;
			case 930:
				useStyle = 5;
				useAnimation = 18;
				useTime = 18;
				width = 24;
				height = 28;
				shoot = 163;
				useAmmo = AmmoID.Flare;
				UseSound = SoundID.Item11;
				damage = 2;
				shootSpeed = 6f;
				noMelee = true;
				value = 50000;
				scale = 0.9f;
				rare = 1;
				holdStyle = 1;
				break;
			case 931:
				shootSpeed = 6f;
				shoot = 163;
				damage = 1;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Flare;
				knockBack = 1.5f;
				value = 7;
				ranged = true;
				break;
			case 932:
				tileWand = 154;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				createTile = 194;
				width = 8;
				height = 10;
				rare = 1;
				value = sellPrice(0, 0, 50);
				break;
			case 933:
				tileWand = 9;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				createTile = 192;
				width = 8;
				height = 10;
				rare = 1;
				value = sellPrice(0, 0, 25);
				break;
			case 934:
				width = 34;
				height = 12;
				accessory = true;
				rare = 2;
				value = 50000;
				break;
			case 935:
				width = 24;
				height = 24;
				accessory = true;
				value = 300000;
				rare = 5;
				break;
			case 936:
				width = 24;
				height = 24;
				accessory = true;
				rare = 6;
				value = buyPrice(0, 25);
				handOffSlot = 4;
				handOnSlot = 9;
				break;
			case 937:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 210;
				width = 12;
				height = 12;
				placeStyle = 0;
				mech = true;
				value = 50000;
				mech = true;
				break;
			case 938:
				width = 24;
				height = 24;
				accessory = true;
				rare = 8;
				defense = 6;
				value = 300000;
				shieldSlot = 2;
				break;
			case 939:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 10f;
				shoot = 165;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 2;
				noMelee = true;
				value = 20000;
				break;
			case 940:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 207;
				placeStyle = 2;
				width = 26;
				height = 36;
				value = buyPrice(0, 4);
				rare = 1;
				break;
			case 941:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 207;
				placeStyle = 3;
				width = 26;
				height = 36;
				value = buyPrice(0, 4);
				rare = 1;
				break;
			case 942:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 207;
				placeStyle = 4;
				width = 26;
				height = 36;
				value = buyPrice(0, 4);
				rare = 1;
				break;
			case 943:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 207;
				placeStyle = 5;
				width = 26;
				height = 36;
				value = buyPrice(0, 4);
				rare = 1;
				break;
			case 944:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 207;
				placeStyle = 6;
				width = 26;
				height = 36;
				value = buyPrice(0, 4);
				rare = 1;
				break;
			case 945:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 207;
				placeStyle = 7;
				width = 26;
				height = 36;
				value = buyPrice(0, 4);
				rare = 1;
				break;
			case 946:
				width = 44;
				height = 44;
				rare = 1;
				value = 10000;
				holdStyle = 2;
				useStyle = 3;
				useAnimation = 22;
				useTime = 22;
				damage = 10;
				knockBack = 5f;
				UseSound = SoundID.Item1;
				melee = true;
				break;
			case 947:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 211;
				width = 12;
				height = 12;
				rare = 7;
				value = sellPrice(0, 0, 15);
				break;
			case 948:
				width = 24;
				height = 8;
				accessory = true;
				rare = 8;
				wingSlot = 12;
				value = buyPrice(3);
				break;
			case 949:
				useStyle = 1;
				shootSpeed = 7f;
				shoot = 166;
				ammo = AmmoID.Snowball;
				damage = 8;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 19;
				useTime = 19;
				noUseGraphic = true;
				noMelee = true;
				ranged = true;
				knockBack = 5.75f;
				break;
			case 950:
				width = 16;
				height = 24;
				accessory = true;
				rare = 1;
				value = 50000;
				shoeSlot = 7;
				break;
			case 951:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 212;
				width = 20;
				height = 20;
				value = 50000;
				rare = 2;
				break;
			case 952:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 15;
				width = 26;
				height = 22;
				value = 500;
				break;
			case 953:
				width = 16;
				height = 24;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 0, 50);
				handOnSlot = 11;
				handOffSlot = 6;
				break;
			case 954:
				width = 18;
				height = 18;
				defense = 2;
				headSlot = 72;
				value = 5000;
				break;
			case 955:
				width = 18;
				height = 18;
				defense = 4;
				headSlot = 73;
				value = 25000;
				break;
			case 956:
				width = 18;
				height = 18;
				defense = 6;
				headSlot = 74;
				rare = 1;
				value = 37500;
				break;
			case 957:
				width = 18;
				height = 18;
				defense = 7;
				bodySlot = 48;
				rare = 1;
				value = 30000;
				break;
			case 958:
				width = 18;
				height = 18;
				defense = 6;
				legSlot = 44;
				rare = 1;
				value = 22500;
				break;
			case 959:
				width = 18;
				height = 18;
				defense = 6;
				headSlot = 75;
				rare = 2;
				value = 45000;
				break;
			case 960:
				width = 18;
				height = 18;
				defense = 5;
				headSlot = 76;
				rare = 3;
				value = 45000;
				break;
			case 961:
				width = 18;
				height = 18;
				defense = 6;
				bodySlot = 49;
				rare = 3;
				value = 30000;
				break;
			case 962:
				width = 18;
				height = 18;
				defense = 6;
				legSlot = 45;
				rare = 3;
				value = 30000;
				break;
			case 963:
				width = 16;
				height = 24;
				accessory = true;
				rare = 7;
				value = buyPrice(0, 15);
				waistSlot = 10;
				break;
			case 964:
				knockBack = 5.75f;
				useStyle = 5;
				useAnimation = 40;
				useTime = 40;
				width = 50;
				height = 14;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item36;
				damage = 14;
				shootSpeed = 5.35f;
				noMelee = true;
				value = sellPrice(0, 2);
				rare = 2;
				ranged = true;
				break;
			case 965:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 8;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 213;
				width = 12;
				height = 12;
				value = 10;
				tileBoost += 3;
				break;
			case 966:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 215;
				width = 12;
				height = 12;
				break;
			case 967:
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				value = 100;
				break;
			case 968:
				holdStyle = 1;
				width = 12;
				height = 12;
				value = 200;
				break;
			case 970:
				createTile = 216;
				placeStyle = 0;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 30;
				value = 1500;
				mech = true;
				break;
			case 971:
				createTile = 216;
				placeStyle = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 30;
				value = 1500;
				mech = true;
				break;
			case 972:
				createTile = 216;
				placeStyle = 2;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 30;
				value = 1500;
				mech = true;
				break;
			case 973:
				createTile = 216;
				placeStyle = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 30;
				value = 1500;
				mech = true;
				break;
			case 974:
				flame = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 9;
				width = 10;
				height = 12;
				value = 60;
				noWet = true;
				break;
			case 975:
				width = 16;
				height = 24;
				accessory = true;
				rare = 1;
				value = 50000;
				shoeSlot = 4;
				break;
			case 976:
				width = 16;
				height = 24;
				accessory = true;
				rare = 2;
				value = 50000;
				shoeSlot = 4;
				handOnSlot = 11;
				handOffSlot = 6;
				break;
			case 977:
				width = 16;
				height = 24;
				accessory = true;
				rare = 7;
				value = buyPrice(0, 15);
				shoeSlot = 3;
				break;
			case 978:
				width = 18;
				height = 18;
				headSlot = 77;
				value = 50000;
				defense = 3;
				break;
			case 979:
				width = 18;
				height = 18;
				bodySlot = 50;
				value = 40000;
				defense = 3;
				break;
			case 980:
				width = 18;
				height = 18;
				legSlot = 46;
				value = 30000;
				defense = 3;
				break;
			case 981:
				maxStack = CommonMaxStack;
				width = 12;
				height = 20;
				value = 10000;
				break;
			case 982:
				width = 22;
				height = 22;
				accessory = true;
				rare = 1;
				value = 50000;
				handOnSlot = 1;
				break;
			case 983:
				width = 14;
				height = 28;
				rare = 4;
				value = 150000;
				accessory = true;
				balloonSlot = 6;
				break;
			case 984:
				width = 16;
				height = 24;
				accessory = true;
				rare = 8;
				value = 500000;
				handOnSlot = 11;
				handOffSlot = 6;
				shoeSlot = 14;
				waistSlot = 10;
				break;
			case 985:
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 171;
				damage = 0;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				noMelee = true;
				value = 100;
				break;
			case 986:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 35;
				useTime = 35;
				width = 38;
				height = 6;
				shoot = 10;
				useAmmo = AmmoID.Dart;
				UseSound = SoundID.Item64;
				damage = 27;
				shootSpeed = 13f;
				noMelee = true;
				value = buyPrice(0, 5);
				knockBack = 4f;
				useAmmo = AmmoID.Dart;
				ranged = true;
				rare = 3;
				break;
			case 987:
				width = 16;
				height = 24;
				accessory = true;
				rare = 1;
				value = 50000;
				waistSlot = 13;
				break;
			case 988:
				shootSpeed = 3.75f;
				shoot = 172;
				damage = 7;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 2.2f;
				value = 15;
				ranged = true;
				break;
			case 989:
				autoReuse = true;
				rare = 2;
				UseSound = SoundID.Item1;
				useStyle = 1;
				damage = 23;
				useAnimation = 21;
				useTime = 45;
				scale = 1.1f;
				width = 30;
				height = 30;
				shoot = 173;
				shootSpeed = 9.5f;
				knockBack = 4.25f;
				melee = true;
				value = sellPrice(0, 3);
				break;
			case 990:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 25;
				useTime = 7;
				knockBack = 4.75f;
				width = 20;
				height = 12;
				damage = 35;
				pick = 200;
				axe = 22;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 220000;
				melee = true;
				scale = 1.1f;
				break;
			case 991:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 35;
				useTime = 13;
				knockBack = 5f;
				width = 20;
				height = 12;
				damage = 33;
				axe = 14;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 54000;
				melee = true;
				scale = 1.1f;
				break;
			case 992:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 35;
				useTime = 10;
				knockBack = 6f;
				width = 20;
				height = 12;
				damage = 39;
				axe = 17;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 81000;
				melee = true;
				scale = 1.1f;
				break;
			case 993:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 35;
				useTime = 8;
				knockBack = 7f;
				width = 20;
				height = 12;
				damage = 43;
				axe = 20;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 108000;
				melee = true;
				scale = 1.1f;
				break;
			case 994:
				damage = 0;
				useStyle = 1;
				shoot = 175;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 7, 50);
				buffType = 45;
				break;
			case 995:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 217;
				width = 26;
				height = 20;
				value = 100000;
				break;
			case 996:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 218;
				width = 26;
				height = 20;
				value = 100000;
				break;
			case 997:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 219;
				width = 26;
				height = 20;
				value = 100000;
				break;
			case 998:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 220;
				width = 26;
				height = 20;
				value = 100000;
				break;
			case 999:
				createTile = 178;
				placeStyle = 6;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				alpha = 50;
				width = 10;
				height = 14;
				value = 15000;
				break;
			case 1000:
				useStyle = 5;
				shootSpeed = 10f;
				shoot = 178;
				damage = 0;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item11;
				useAnimation = 15;
				useTime = 15;
				noMelee = true;
				value = 100;
				ranged = true;
				break;
			}
		}

		public void SetDefaults2(int type)
		{
			switch (type)
			{
			case 1001:
				width = 18;
				height = 18;
				defense = 20;
				headSlot = 78;
				rare = 7;
				value = 300000;
				return;
			case 1002:
				width = 18;
				height = 18;
				defense = 13;
				headSlot = 79;
				rare = 7;
				value = 300000;
				return;
			case 1003:
				width = 18;
				height = 18;
				defense = 7;
				headSlot = 80;
				rare = 7;
				value = 300000;
				return;
			case 1004:
				width = 18;
				height = 18;
				defense = 18;
				bodySlot = 51;
				rare = 7;
				value = 240000;
				return;
			case 1005:
				width = 18;
				height = 18;
				defense = 13;
				legSlot = 47;
				rare = 7;
				value = 180000;
				return;
			case 1006:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 90);
				rare = 7;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 17;
				return;
			case 1007:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1008:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1009:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1010:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1011:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1012:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1013:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1014:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1015:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1016:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1017:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1018:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1019:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1020:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1021:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1022:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1023:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1024:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1025:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1026:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1027:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1028:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1029:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1030:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1031:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1032:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1033:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1034:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1035:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1036:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1037:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1038:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1039:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1040:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1041:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1042:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1043:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1044:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1045:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1046:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1047:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1048:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1049:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1050:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1051:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1052:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1053:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1054:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1055:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1056:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1057:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1058:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1059:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1060:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1061:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1062:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1063:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1064:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1065:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1066:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1067:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1068:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1069:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1070:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1071:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				width = 24;
				height = 24;
				value = 10000;
				return;
			case 1072:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				width = 24;
				height = 24;
				value = 10000;
				return;
			case 1073:
				paint = 1;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1074:
				paint = 2;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1075:
				paint = 3;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1076:
				paint = 4;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1077:
				paint = 5;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1078:
				paint = 6;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1079:
				paint = 7;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1080:
				paint = 8;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1081:
				paint = 9;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1082:
				paint = 10;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1083:
				paint = 11;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1084:
				paint = 12;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1085:
				paint = 13;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1086:
				paint = 14;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1087:
				paint = 15;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1088:
				paint = 16;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1089:
				paint = 17;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1090:
				paint = 18;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1091:
				paint = 19;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1092:
				paint = 20;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1093:
				paint = 21;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1094:
				paint = 22;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1095:
				paint = 23;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1096:
				paint = 24;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1097:
				paint = 25;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1098:
				paint = 26;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1099:
				paint = 27;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1100:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				width = 24;
				height = 24;
				value = 10000;
				return;
			case 1101:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 226;
				width = 12;
				height = 12;
				return;
			case 1102:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 112;
				width = 12;
				height = 12;
				return;
			case 1103:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 224;
				width = 12;
				height = 12;
				return;
			case 1104:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 221;
				width = 12;
				height = 12;
				value = 4500;
				rare = 3;
				return;
			case 1105:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 222;
				width = 12;
				height = 12;
				value = 6500;
				rare = 3;
				return;
			case 1106:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 223;
				width = 12;
				height = 12;
				value = 8500;
				rare = 3;
				return;
			case 1107:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				placeStyle = 0;
				createTile = 227;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				return;
			case 1108:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				placeStyle = 1;
				createTile = 227;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				return;
			case 1109:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				placeStyle = 2;
				createTile = 227;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				return;
			case 1110:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				placeStyle = 3;
				createTile = 227;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				return;
			case 1111:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				placeStyle = 4;
				createTile = 227;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				return;
			case 1112:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				placeStyle = 5;
				createTile = 227;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				return;
			case 1113:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1114:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				placeStyle = 7;
				createTile = 227;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				return;
			case 1115:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1116:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1117:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1118:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1119:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1120:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 228;
				width = 26;
				height = 20;
				value = buyPrice(0, 5);
				return;
			case 1121:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 12;
				useTime = 12;
				mana = 5;
				width = 50;
				height = 18;
				shoot = 181;
				UseSound = SoundID.Item11;
				damage = 9;
				shootSpeed = 8f;
				noMelee = true;
				value = queenBeePrice;
				knockBack = 0.25f;
				rare = 2;
				magic = true;
				scale = 0.8f;
				return;
			case 1122:
				autoReuse = true;
				useStyle = 1;
				shootSpeed = 12f;
				shoot = 182;
				damage = 80;
				width = 18;
				height = 20;
				UseSound = SoundID.Item1;
				useAnimation = 14;
				useTime = 14;
				noUseGraphic = true;
				noMelee = true;
				value = buyPrice(0, 35);
				knockBack = 5f;
				melee = true;
				rare = 7;
				return;
			case 1123:
				useStyle = 1;
				useTime = 20;
				useAnimation = 20;
				knockBack = 5.3f;
				width = 40;
				autoReuse = true;
				height = 40;
				damage = 30;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 3;
				value = queenBeePrice;
				melee = true;
				return;
			case 1124:
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				return;
			case 1125:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 229;
				width = 12;
				height = 12;
				return;
			case 1126:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 108;
				width = 12;
				height = 12;
				return;
			case 1127:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 230;
				width = 12;
				height = 12;
				return;
			case 1128:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				autoReuse = true;
				return;
			case 1129:
				tileWand = 1124;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				createTile = 225;
				width = 8;
				height = 10;
				rare = 1;
				value = sellPrice(0, 0, 50);
				return;
			case 1130:
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 183;
				knockBack = 1f;
				damage = 12;
				width = 10;
				height = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 200;
				ranged = true;
				return;
			case 1131:
				width = 22;
				height = 22;
				accessory = true;
				rare = 8;
				value = sellPrice(0, 40);
				expert = true;
				return;
			case 1132:
				width = 22;
				height = 22;
				accessory = true;
				rare = 2;
				value = 100000;
				return;
			case 1133:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				rare = 1;
				return;
			case 1134:
				UseSound = SoundID.Item3;
				healLife = 80;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				potion = true;
				buffType = 48;
				buffTime = 900;
				value = 40;
				return;
			case 1135:
				width = 18;
				height = 18;
				headSlot = 81;
				value = 1000;
				defense = 1;
				return;
			case 1136:
				width = 18;
				height = 18;
				bodySlot = 52;
				value = 1000;
				defense = 2;
				return;
			case 1137:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 12;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1138:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 13;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1139:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 14;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1140:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 15;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1141:
				width = 14;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 7;
				return;
			case 1142:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 16;
				width = 26;
				height = 22;
				value = 500;
				return;
			case 1143:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 12;
				width = 12;
				height = 30;
				value = 150;
				return;
			case 1144:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 9;
				width = 26;
				height = 20;
				value = 300;
				return;
			case 1145:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 10;
				width = 28;
				height = 14;
				value = 150;
				return;
			case 1146:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 137;
				placeStyle = 1;
				width = 12;
				height = 12;
				value = 10000;
				mech = true;
				return;
			case 1147:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 137;
				placeStyle = 2;
				width = 12;
				height = 12;
				value = 10000;
				mech = true;
				return;
			case 1148:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 137;
				placeStyle = 3;
				width = 12;
				height = 12;
				value = 10000;
				mech = true;
				return;
			case 1149:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 137;
				placeStyle = 4;
				width = 12;
				height = 12;
				value = 10000;
				mech = true;
				return;
			case 1150:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 232;
				width = 12;
				height = 12;
				return;
			case 1151:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 135;
				width = 12;
				height = 12;
				placeStyle = 6;
				mech = true;
				value = 5000;
				return;
			case 1152:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 43;
				return;
			case 1153:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 44;
				return;
			case 1154:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 45;
				return;
			case 1155:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 18;
				useTime = 18;
				mana = 10;
				width = 50;
				height = 18;
				shoot = 189;
				UseSound = SoundID.Item11;
				damage = 31;
				shootSpeed = 9f;
				noMelee = true;
				value = 500000;
				knockBack = 0.25f;
				rare = 8;
				magic = true;
				return;
			case 1156:
				channel = true;
				useStyle = 5;
				useAnimation = 30;
				useTime = 30;
				knockBack = 1f;
				width = 30;
				height = 10;
				damage = 38;
				scale = 1.1f;
				shoot = 190;
				shootSpeed = 14f;
				UseSound = SoundID.Item10;
				rare = 8;
				value = sellPrice(0, 20);
				ranged = true;
				noMelee = true;
				return;
			case 1157:
				mana = 10;
				damage = 40;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 191;
				width = 26;
				height = 28;
				UseSound = SoundID.Item44;
				useAnimation = 28;
				useTime = 28;
				rare = 7;
				noMelee = true;
				knockBack = 3f;
				buffType = 49;
				value = buyPrice(0, 35);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				return;
			case 1158:
				rare = 7;
				width = 24;
				height = 28;
				accessory = true;
				value = buyPrice(0, 20);
				neckSlot = 4;
				return;
			case 1159:
				width = 18;
				height = 18;
				defense = 6;
				headSlot = 82;
				rare = 7;
				value = buyPrice(0, 50);
				return;
			case 1160:
				width = 18;
				height = 18;
				defense = 17;
				bodySlot = 53;
				rare = 7;
				value = buyPrice(0, 50);
				return;
			case 1161:
				width = 18;
				height = 18;
				defense = 12;
				legSlot = 48;
				rare = 7;
				value = buyPrice(0, 50);
				return;
			case 1162:
				width = 24;
				height = 8;
				accessory = true;
				value = buyPrice(1, 50);
				wingSlot = 13;
				rare = 5;
				return;
			case 1163:
				width = 14;
				height = 28;
				rare = 4;
				value = 150000;
				accessory = true;
				balloonSlot = 1;
				return;
			case 1164:
				width = 14;
				height = 28;
				rare = 8;
				value = 150000;
				accessory = true;
				balloonSlot = 3;
				return;
			case 1165:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 14;
				return;
			case 1166:
				useStyle = 1;
				useTime = 22;
				useAnimation = 22;
				knockBack = 5.5f;
				width = 24;
				height = 28;
				damage = 19;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				rare = 3;
				value = 9000;
				melee = true;
				return;
			case 1167:
				rare = 7;
				width = 24;
				height = 28;
				accessory = true;
				value = buyPrice(0, 40);
				return;
			case 1168:
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 196;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 20;
				return;
			case 1169:
				damage = 0;
				useStyle = 1;
				shoot = 197;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 5);
				buffType = 50;
				return;
			case 1170:
				damage = 0;
				useStyle = 1;
				shoot = 198;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 3);
				buffType = 51;
				return;
			case 1171:
				damage = 0;
				useStyle = 1;
				shoot = 199;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				buffType = 52;
				value = buyPrice(2);
				return;
			case 1172:
				damage = 0;
				useStyle = 1;
				shoot = 200;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 2);
				buffType = 53;
				return;
			case 1173:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 85;
				placeStyle = 1;
				width = 20;
				height = 20;
				return;
			case 1174:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 85;
				placeStyle = 2;
				width = 20;
				height = 20;
				return;
			case 1175:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 85;
				placeStyle = 3;
				width = 20;
				height = 20;
				return;
			case 1176:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 85;
				placeStyle = 4;
				width = 20;
				height = 20;
				return;
			case 1177:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 85;
				placeStyle = 5;
				width = 20;
				height = 20;
				return;
			case 1178:
				useStyle = 5;
				mana = 5;
				autoReuse = true;
				useAnimation = 7;
				useTime = 7;
				width = 24;
				height = 18;
				shoot = 206;
				UseSound = SoundID.Item7;
				damage = 48;
				shootSpeed = 11f;
				noMelee = true;
				value = 300000;
				knockBack = 4f;
				rare = 7;
				magic = true;
				return;
			case 1179:
				shootSpeed = 5f;
				shoot = 207;
				damage = 9;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 4.5f;
				value = 50;
				ranged = true;
				rare = 7;
				return;
			case 1180:
				damage = 0;
				useStyle = 1;
				shoot = 208;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				buffType = 54;
				value = sellPrice(0, 75);
				return;
			case 1181:
				damage = 0;
				useStyle = 1;
				shoot = 209;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = buyPrice(0, 45);
				buffType = 55;
				return;
			case 1182:
				damage = 0;
				useStyle = 1;
				shoot = 210;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 2);
				buffType = 56;
				return;
			case 1183:
				damage = 0;
				useStyle = 1;
				shoot = 211;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				value = sellPrice(0, 5, 50);
				buffType = 57;
				return;
			case 1184:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 13500;
				rare = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 12;
				return;
			case 1185:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 22;
				useTime = 22;
				knockBack = 5.5f;
				width = 40;
				height = 40;
				damage = 49;
				scale = 1.2f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 92000;
				melee = true;
				return;
			case 1186:
				useStyle = 5;
				useAnimation = 27;
				useTime = 27;
				shootSpeed = 4.4f;
				knockBack = 4.5f;
				width = 40;
				height = 40;
				damage = 44;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 212;
				rare = 4;
				value = 60000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				return;
			case 1187:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 22;
				useTime = 22;
				width = 50;
				height = 18;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 37;
				shootSpeed = 9.25f;
				noMelee = true;
				value = 80000;
				ranged = true;
				rare = 4;
				knockBack = 1.75f;
				return;
			case 1188:
				useStyle = 1;
				useTurn = true;
				autoReuse = true;
				useAnimation = 25;
				useTime = 12;
				knockBack = 5f;
				width = 20;
				height = 12;
				damage = 12;
				pick = 130;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 72000;
				melee = true;
				scale = 1.15f;
				return;
			case 1189:
				useStyle = 5;
				useAnimation = 25;
				useTime = 12;
				shootSpeed = 32f;
				knockBack = 0.5f;
				width = 20;
				height = 12;
				damage = 12;
				pick = 130;
				UseSound = SoundID.Item23;
				shoot = 213;
				rare = 4;
				value = 72000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				return;
			case 1190:
				useStyle = 5;
				useAnimation = 25;
				useTime = 12;
				shootSpeed = 40f;
				knockBack = 2.9f;
				width = 20;
				height = 12;
				damage = 26;
				axe = 15;
				UseSound = SoundID.Item23;
				shoot = 214;
				rare = 4;
				value = 72000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				return;
			case 1191:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 52);
				rare = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 14;
				return;
			case 1192:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 22;
				useTime = 22;
				knockBack = 6f;
				width = 40;
				height = 40;
				damage = 59;
				scale = 1.22f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 126500;
				melee = true;
				return;
			case 1193:
				useStyle = 5;
				useAnimation = 25;
				useTime = 25;
				shootSpeed = 4.5f;
				knockBack = 5.5f;
				width = 40;
				height = 40;
				damage = 46;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 215;
				rare = 4;
				value = 82500;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				return;
			case 1194:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 19;
				useTime = 19;
				width = 50;
				height = 18;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 40;
				shootSpeed = 9.75f;
				noMelee = true;
				value = 110000;
				ranged = true;
				rare = 4;
				knockBack = 2f;
				return;
			case 1195:
				useStyle = 1;
				useAnimation = 25;
				useTime = 9;
				knockBack = 5f;
				useTurn = true;
				autoReuse = true;
				width = 20;
				height = 12;
				damage = 17;
				pick = 165;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 99000;
				melee = true;
				scale = 1.15f;
				return;
			case 1196:
				useStyle = 5;
				useAnimation = 25;
				useTime = 9;
				shootSpeed = 32f;
				knockBack = 0.5f;
				width = 20;
				height = 12;
				damage = 17;
				pick = 165;
				UseSound = SoundID.Item23;
				shoot = 216;
				rare = 4;
				value = 99000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				return;
			case 1197:
				useStyle = 5;
				useAnimation = 25;
				useTime = 9;
				shootSpeed = 40f;
				knockBack = 3.75f;
				width = 20;
				height = 12;
				damage = 31;
				axe = 18;
				UseSound = SoundID.Item23;
				shoot = 217;
				rare = 4;
				value = 99000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				return;
			case 1198:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 68);
				rare = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 16;
				return;
			case 1199:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 20;
				useTime = 20;
				knockBack = 6f;
				width = 40;
				height = 40;
				damage = 61;
				scale = 1.25f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 161000;
				melee = true;
				return;
			case 1200:
				useStyle = 5;
				useAnimation = 23;
				useTime = 23;
				shootSpeed = 5f;
				knockBack = 6.2f;
				width = 40;
				height = 40;
				damage = 48;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 218;
				rare = 4;
				value = 105000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				return;
			case 1201:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 17;
				useTime = 17;
				width = 50;
				height = 18;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 43;
				shootSpeed = 10.5f;
				noMelee = true;
				value = 140000;
				ranged = true;
				rare = 4;
				knockBack = 2.5f;
				return;
			case 1202:
				useStyle = 1;
				useAnimation = 25;
				useTime = 7;
				knockBack = 5f;
				useTurn = true;
				autoReuse = true;
				width = 20;
				height = 12;
				damage = 27;
				pick = 190;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 126000;
				melee = true;
				scale = 1.15f;
				return;
			case 1203:
				useStyle = 5;
				useAnimation = 25;
				useTime = 7;
				shootSpeed = 32f;
				knockBack = 0.5f;
				width = 20;
				height = 12;
				damage = 27;
				pick = 190;
				UseSound = SoundID.Item23;
				shoot = 219;
				rare = 4;
				value = 126000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				return;
			case 1204:
				useStyle = 5;
				useAnimation = 25;
				useTime = 7;
				shootSpeed = 40f;
				knockBack = 4.6f;
				width = 20;
				height = 12;
				damage = 34;
				axe = 21;
				UseSound = SoundID.Item23;
				shoot = 220;
				rare = 4;
				value = 126000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				return;
			case 1205:
				width = 18;
				height = 18;
				defense = 14;
				headSlot = 83;
				rare = 4;
				value = 75000;
				return;
			case 1206:
				width = 18;
				height = 18;
				defense = 5;
				headSlot = 84;
				rare = 4;
				value = 75000;
				return;
			case 1207:
				width = 18;
				height = 18;
				defense = 3;
				headSlot = 85;
				rare = 4;
				value = 75000;
				return;
			case 1208:
				width = 18;
				height = 18;
				defense = 10;
				bodySlot = 54;
				rare = 4;
				value = 60000;
				return;
			case 1209:
				width = 18;
				height = 18;
				defense = 8;
				legSlot = 49;
				rare = 4;
				value = 45000;
				return;
			case 1210:
				width = 18;
				height = 18;
				defense = 19;
				headSlot = 86;
				rare = 4;
				value = 112500;
				return;
			case 1211:
				width = 18;
				height = 18;
				defense = 7;
				headSlot = 87;
				rare = 4;
				value = 112500;
				return;
			case 1212:
				width = 18;
				height = 18;
				defense = 4;
				headSlot = 88;
				rare = 4;
				value = 112500;
				return;
			case 1213:
				width = 18;
				height = 18;
				defense = 13;
				bodySlot = 55;
				rare = 4;
				value = 90000;
				return;
			case 1214:
				width = 18;
				height = 18;
				defense = 10;
				legSlot = 50;
				rare = 4;
				value = 67500;
				return;
			case 1215:
				width = 18;
				height = 18;
				defense = 23;
				headSlot = 89;
				rare = 4;
				value = 150000;
				return;
			case 1216:
				width = 18;
				height = 18;
				defense = 8;
				headSlot = 90;
				rare = 4;
				value = 150000;
				return;
			case 1217:
				width = 18;
				height = 18;
				defense = 4;
				headSlot = 91;
				rare = 4;
				value = 150000;
				return;
			case 1218:
				width = 18;
				height = 18;
				defense = 15;
				bodySlot = 56;
				rare = 4;
				value = 120000;
				return;
			case 1219:
				width = 18;
				height = 18;
				defense = 11;
				legSlot = 51;
				rare = 4;
				value = 90000;
				return;
			case 1220:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 134;
				placeStyle = 1;
				width = 28;
				height = 14;
				value = 25000;
				rare = 3;
				return;
			case 1221:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 133;
				placeStyle = 1;
				width = 44;
				height = 30;
				value = 50000;
				rare = 3;
				return;
			case 1222:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 35;
				useTime = 12;
				knockBack = 5.5f;
				width = 20;
				height = 12;
				damage = 36;
				axe = 15;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 72000;
				melee = true;
				scale = 1.1f;
				return;
			case 1223:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 35;
				useTime = 9;
				knockBack = 6.5f;
				width = 20;
				height = 12;
				damage = 41;
				axe = 18;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 99000;
				melee = true;
				scale = 1.1f;
				return;
			case 1224:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 35;
				useTime = 7;
				knockBack = 7.5f;
				width = 20;
				height = 12;
				damage = 44;
				axe = 21;
				UseSound = SoundID.Item1;
				rare = 4;
				value = 126000;
				melee = true;
				scale = 1.1f;
				return;
			case 1225:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 40);
				rare = 4;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 18;
				return;
			case 1226:
				useStyle = 1;
				useAnimation = 26;
				useTime = 60;
				shoot = 229;
				shootSpeed = 8f;
				knockBack = 6f;
				width = 40;
				height = 40;
				damage = 95;
				UseSound = SoundID.Item1;
				rare = 7;
				value = 276000;
				scale = 1.25f;
				melee = true;
				return;
			case 1227:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 16;
				useTime = 28;
				shoot = 228;
				shootSpeed = 8f;
				knockBack = 4f;
				width = 40;
				height = 40;
				damage = 57;
				UseSound = SoundID.Item1;
				rare = 7;
				value = 276000;
				melee = true;
				return;
			case 1228:
				useStyle = 5;
				useAnimation = 23;
				useTime = 23;
				shootSpeed = 5f;
				knockBack = 6.2f;
				width = 40;
				height = 40;
				damage = 49;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 222;
				rare = 7;
				value = 180000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				return;
			case 1229:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 19;
				useTime = 19;
				width = 50;
				height = 18;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 34;
				shootSpeed = 11.5f;
				noMelee = true;
				value = 240000;
				ranged = true;
				rare = 7;
				knockBack = 2.75f;
				return;
			case 1230:
				useStyle = 1;
				useAnimation = 25;
				useTime = 7;
				knockBack = 5f;
				useTurn = true;
				autoReuse = true;
				width = 20;
				height = 12;
				damage = 40;
				pick = 200;
				UseSound = SoundID.Item1;
				rare = 7;
				value = 216000;
				melee = true;
				scale = 1.15f;
				tileBoost++;
				return;
			case 1231:
				useStyle = 5;
				useAnimation = 25;
				useTime = 7;
				shootSpeed = 40f;
				knockBack = 1f;
				width = 20;
				height = 12;
				damage = 35;
				pick = 200;
				UseSound = SoundID.Item23;
				shoot = 223;
				rare = 7;
				value = 216000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				tileBoost++;
				return;
			case 1232:
				useStyle = 5;
				useAnimation = 25;
				useTime = 7;
				shootSpeed = 46f;
				knockBack = 4.6f;
				width = 20;
				height = 12;
				damage = 50;
				axe = 23;
				UseSound = SoundID.Item23;
				shoot = 224;
				rare = 7;
				value = 216000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				tileBoost++;
				return;
			case 1233:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 30;
				useTime = 7;
				knockBack = 7f;
				width = 20;
				height = 12;
				damage = 70;
				axe = 23;
				UseSound = SoundID.Item1;
				rare = 7;
				value = 216000;
				melee = true;
				scale = 1.15f;
				tileBoost++;
				return;
			case 1234:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 35;
				useTime = 14;
				hammer = 90;
				width = 24;
				height = 28;
				damage = 80;
				knockBack = 8f;
				scale = 1.25f;
				UseSound = SoundID.Item1;
				rare = 7;
				value = 216000;
				melee = true;
				tileBoost++;
				return;
			case 1235:
				shootSpeed = 4.5f;
				shoot = 225;
				damage = 16;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 3.5f;
				value = 100;
				ranged = true;
				rare = 7;
				return;
			case 1236:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 10f;
				shoot = 230;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = 20000;
				return;
			case 1237:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 10.5f;
				shoot = 231;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = 20000;
				return;
			case 1238:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 11f;
				shoot = 232;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = 20000;
				return;
			case 1239:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 11.5f;
				shoot = 233;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = 20000;
				return;
			case 1240:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 12f;
				shoot = 234;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = 20000;
				return;
			case 1241:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 12.5f;
				shoot = 235;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = 20000;
				return;
			case 1242:
				damage = 0;
				useStyle = 1;
				shoot = 236;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 7, 50);
				buffType = 61;
				return;
			case 1243:
				width = 28;
				height = 20;
				headSlot = 92;
				rare = 1;
				vanity = true;
				return;
			case 1244:
				mana = 30;
				damage = 30;
				useStyle = 1;
				shootSpeed = 16f;
				shoot = 237;
				width = 26;
				height = 28;
				UseSound = SoundID.Item66;
				useAnimation = 22;
				useTime = 22;
				rare = 6;
				noMelee = true;
				knockBack = 0f;
				value = sellPrice(0, 3, 50);
				magic = true;
				return;
			case 1245:
				flame = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 10;
				width = 10;
				height = 12;
				value = 60;
				noWet = true;
				return;
			case 1246:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 234;
				width = 12;
				height = 12;
				ammo = AmmoID.Sand;
				notAmmo = true;
				return;
			case 1247:
				width = 20;
				height = 24;
				value = 150000;
				accessory = true;
				rare = 4;
				backSlot = 1;
				return;
			case 1248:
				width = 24;
				height = 24;
				accessory = true;
				value = buyPrice(0, 25);
				rare = 7;
				return;
			case 1249:
				width = 14;
				height = 28;
				rare = 2;
				value = sellPrice(0, 2);
				accessory = true;
				balloonSlot = 7;
				return;
			case 1250:
				width = 20;
				height = 22;
				rare = 4;
				value = buyPrice(0, 15);
				accessory = true;
				balloonSlot = 2;
				return;
			case 1251:
				width = 20;
				height = 22;
				rare = 4;
				value = buyPrice(0, 15);
				accessory = true;
				balloonSlot = 9;
				return;
			case 1252:
				width = 20;
				height = 22;
				rare = 4;
				value = buyPrice(0, 15);
				accessory = true;
				balloonSlot = 10;
				return;
			case 1253:
				width = 20;
				height = 24;
				value = 225000;
				accessory = true;
				rare = 5;
				return;
			case 1254:
				useStyle = 5;
				useAnimation = 36;
				useTime = 36;
				crit += 25;
				width = 44;
				height = 14;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item40;
				damage = 185;
				shootSpeed = 16f;
				noMelee = true;
				value = buyPrice(0, 40);
				knockBack = 8f;
				rare = 8;
				ranged = true;
				return;
			case 1255:
				autoReuse = false;
				useStyle = 5;
				useAnimation = 9;
				useTime = 9;
				width = 24;
				height = 22;
				shoot = 14;
				knockBack = 5.5f;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item41;
				damage = 50;
				shootSpeed = 13.5f;
				noMelee = true;
				value = sellPrice(0, 5);
				scale = 0.85f;
				rare = 7;
				ranged = true;
				autoReuse = true;
				return;
			case 1256:
				mana = 30;
				damage = 12;
				useStyle = 1;
				shootSpeed = 12f;
				shoot = 243;
				width = 26;
				height = 28;
				UseSound = SoundID.Item8;
				useAnimation = 24;
				useTime = 24;
				rare = 1;
				noMelee = true;
				knockBack = 0f;
				value = shadowOrbPrice;
				magic = true;
				return;
			case 1257:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 1;
				value = sellPrice(0, 0, 39);
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 19;
				return;
			case 1258:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 22;
				useTime = 22;
				width = 50;
				height = 18;
				shoot = 246;
				useAmmo = AmmoID.StyngerBolt;
				UseSound = SoundID.Item11;
				damage = 45;
				knockBack = 5f;
				shootSpeed = 9f;
				noMelee = true;
				value = buyPrice(0, 35);
				rare = 7;
				ranged = true;
				return;
			case 1259:
				noMelee = true;
				useStyle = 5;
				useAnimation = 40;
				useTime = 40;
				knockBack = 6.5f;
				width = 30;
				height = 10;
				damage = 65;
				scale = 1.1f;
				noUseGraphic = true;
				shoot = 247;
				shootSpeed = 15.9f;
				UseSound = SoundID.Item1;
				rare = 7;
				value = sellPrice(0, 6);
				melee = true;
				channel = true;
				return;
			case 1260:
				useStyle = 5;
				useAnimation = 40;
				useTime = 40;
				width = 50;
				height = 18;
				shoot = 250;
				UseSound = SoundID.Item67;
				damage = 45;
				knockBack = 2.5f;
				shootSpeed = 16f;
				noMelee = true;
				value = sellPrice(0, 20);
				rare = 8;
				magic = true;
				mana = 20;
				return;
			case 1261:
				shootSpeed = 2f;
				shoot = 246;
				damage = 17;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.StyngerBolt;
				knockBack = 1f;
				value = 75;
				rare = 5;
				ranged = true;
				return;
			case 1262:
				useStyle = 5;
				useAnimation = 25;
				useTime = 7;
				shootSpeed = 46f;
				knockBack = 5.2f;
				width = 20;
				height = 12;
				damage = 45;
				hammer = 90;
				UseSound = SoundID.Item23;
				shoot = 252;
				rare = 7;
				value = 216000;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				tileBoost++;
				return;
			case 1263:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 235;
				width = 12;
				height = 12;
				value = buyPrice(0, 2, 50);
				mech = true;
				return;
			case 1264:
				mana = 11;
				damage = 60;
				useStyle = 1;
				shootSpeed = 9f;
				shoot = 253;
				width = 26;
				height = 28;
				UseSound = SoundID.Item20;
				useAnimation = 12;
				useTime = 12;
				rare = 5;
				noMelee = true;
				knockBack = 6.5f;
				value = sellPrice(0, 5);
				magic = true;
				return;
			case 1265:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 9;
				useTime = 9;
				width = 24;
				height = 22;
				shoot = 14;
				knockBack = 3.5f;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item11;
				damage = 30;
				shootSpeed = 13f;
				noMelee = true;
				value = sellPrice(0, 7);
				scale = 0.75f;
				rare = 7;
				ranged = true;
				return;
			case 1266:
				rare = 8;
				mana = 14;
				UseSound = SoundID.Item20;
				noMelee = true;
				useStyle = 5;
				damage = 48;
				knockBack = 6f;
				useAnimation = 20;
				useTime = 20;
				width = 24;
				height = 28;
				shoot = 254;
				shootSpeed = 1.2f;
				magic = true;
				value = 500000;
				return;
			case 1267:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 88;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 0, 20);
				return;
			case 1268:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 89;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 0, 40);
				return;
			case 1269:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 90;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 0, 60);
				return;
			case 1270:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 91;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 0, 80);
				return;
			case 1271:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 92;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 1, 20);
				return;
			case 1272:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 93;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 1);
				return;
			case 1273:
				useStyle = 5;
				useAnimation = 25;
				useTime = 25;
				width = 30;
				height = 10;
				noUseGraphic = true;
				shoot = 256;
				shootSpeed = 15f;
				UseSound = SoundID.Item1;
				rare = 2;
				value = 45000;
				return;
			case 1274:
				width = 28;
				height = 20;
				headSlot = 93;
				rare = 1;
				vanity = true;
				return;
			case 1275:
				width = 28;
				height = 20;
				headSlot = 94;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 1);
				return;
			case 1276:
				width = 28;
				height = 20;
				headSlot = 95;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 1);
				return;
			case 1277:
				width = 28;
				height = 20;
				headSlot = 96;
				rare = 1;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1278:
				width = 28;
				height = 20;
				headSlot = 97;
				rare = 1;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1279:
				width = 28;
				height = 20;
				bodySlot = 57;
				rare = 1;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1280:
				width = 28;
				height = 20;
				legSlot = 52;
				rare = 1;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1281:
				width = 28;
				height = 20;
				headSlot = 98;
				rare = 1;
				value = sellPrice(0, 0, 75);
				vanity = true;
				return;
			case 1282:
				width = 18;
				height = 14;
				bodySlot = 58;
				value = sellPrice(0, 0, 50);
				return;
			case 1283:
				width = 18;
				height = 14;
				bodySlot = 59;
				defense = 1;
				value = sellPrice(0, 0, 50) * 2;
				return;
			case 1284:
				width = 18;
				height = 14;
				bodySlot = 60;
				defense = 1;
				value = sellPrice(0, 0, 50) * 3;
				rare = 1;
				return;
			case 1285:
				width = 18;
				height = 14;
				bodySlot = 61;
				defense = 2;
				value = sellPrice(0, 0, 50) * 4;
				rare = 1;
				return;
			case 1286:
				width = 18;
				height = 14;
				bodySlot = 62;
				defense = 2;
				value = sellPrice(0, 0, 50) * 5;
				rare = 1;
				return;
			case 1287:
				defense = 3;
				width = 18;
				height = 14;
				bodySlot = 63;
				value = sellPrice(0, 0, 50) * 6;
				rare = 2;
				return;
			case 1288:
				width = 28;
				height = 20;
				bodySlot = 64;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 25);
				return;
			case 1289:
				width = 28;
				height = 20;
				legSlot = 53;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 25);
				return;
			case 1290:
				width = 22;
				height = 22;
				accessory = true;
				rare = 1;
				value = shadowOrbPrice;
				neckSlot = 3;
				return;
			case 1291:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 18;
				height = 18;
				useStyle = 4;
				useTime = 30;
				UseSound = SoundID.Item4;
				useAnimation = 30;
				rare = 7;
				value = sellPrice(0, 2);
				return;
			case 1292:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 237;
				width = 26;
				height = 20;
				value = 300;
				return;
			case 1293:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 22;
				height = 10;
				value = sellPrice(0, 1);
				return;
			case 1294:
				useStyle = 1;
				useAnimation = 16;
				useTime = 6;
				knockBack = 5.5f;
				useTurn = true;
				autoReuse = true;
				width = 20;
				height = 12;
				damage = 34;
				pick = 210;
				axe = 25;
				UseSound = SoundID.Item1;
				rare = 7;
				value = 216000;
				melee = true;
				scale = 1.15f;
				tileBoost++;
				return;
			case 1295:
				mana = 8;
				useStyle = 5;
				autoReuse = true;
				useAnimation = 10;
				useTime = 10;
				width = 24;
				height = 18;
				shoot = 260;
				UseSound = SoundID.Item12;
				damage = 90;
				shootSpeed = 15f;
				noMelee = true;
				value = 350000;
				knockBack = 3f;
				rare = 7;
				magic = true;
				return;
			case 1296:
				mana = 18;
				damage = 125;
				useStyle = 5;
				crit = 20;
				shootSpeed = 12f;
				shoot = 261;
				width = 26;
				height = 28;
				UseSound = SoundID.Item69;
				useAnimation = (useTime = 24);
				rare = 7;
				noMelee = true;
				knockBack = 7.5f;
				value = buyPrice(0, 35);
				magic = true;
				return;
			case 1297:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 24;
				useTime = 24;
				knockBack = 12f;
				width = 30;
				height = 10;
				damage = 90;
				scale = 0.9f;
				shoot = 262;
				shootSpeed = 14f;
				UseSound = SoundID.Item10;
				rare = 7;
				value = buyPrice(0, 35);
				melee = true;
				noMelee = true;
				return;
			case 1298:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 17;
				width = 26;
				height = 22;
				value = 500;
				return;
			case 1299:
				width = 14;
				height = 28;
				rare = 4;
				value = 150000;
				return;
			case 1300:
				width = 14;
				height = 28;
				rare = 4;
				value = 150000;
				accessory = true;
				return;
			case 1301:
				width = 24;
				height = 24;
				accessory = true;
				value = 300000;
				rare = 7;
				return;
			case 1302:
				shootSpeed = 4f;
				shoot = 242;
				damage = 11;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 4f;
				value = 40;
				ranged = true;
				rare = 3;
				return;
			case 1303:
				width = 24;
				height = 24;
				accessory = true;
				value = sellPrice(0, 1);
				rare = 2;
				neckSlot = 1;
				return;
			case 1304:
				useStyle = 1;
				useTurn = false;
				useAnimation = 22;
				useTime = 22;
				width = 24;
				height = 28;
				damage = 15;
				knockBack = 5.5f;
				UseSound = SoundID.Item1;
				scale = 1.2f;
				value = 2000;
				melee = true;
				return;
			case 1305:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 23;
				knockBack = 7.25f;
				useTime = 7;
				width = 24;
				height = 28;
				damage = 72;
				axe = 35;
				hammer = 100;
				tileBoost = 1;
				scale = 1.15f;
				rare = 8;
				value = sellPrice(0, 10);
				melee = true;
				return;
			case 1306:
				useStyle = 1;
				useAnimation = 25;
				useTime = 25;
				knockBack = 5.5f;
				width = 24;
				height = 28;
				damage = 50;
				scale = 1.15f;
				UseSound = SoundID.Item1;
				rare = 5;
				shoot = 263;
				shootSpeed = 12f;
				value = 250000;
				melee = true;
				return;
			case 1307:
				accessory = true;
				width = 14;
				height = 26;
				value = 1000;
				rare = 1;
				return;
			case 1308:
				mana = 22;
				UseSound = SoundID.Item43;
				useStyle = 5;
				damage = 43;
				useAnimation = 36;
				useTime = 36;
				width = 40;
				height = 40;
				shoot = 265;
				shootSpeed = 13.5f;
				knockBack = 5.6f;
				magic = true;
				autoReuse = true;
				rare = 6;
				noMelee = true;
				value = sellPrice(0, 4);
				return;
			case 1309:
				mana = 10;
				damage = 8;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 266;
				width = 26;
				height = 28;
				UseSound = SoundID.Item44;
				useAnimation = 28;
				useTime = 28;
				rare = 4;
				noMelee = true;
				knockBack = 2f;
				buffType = 64;
				value = 100000;
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				return;
			case 1310:
				shoot = 267;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				ammo = AmmoID.Dart;
				damage = 10;
				knockBack = 2f;
				shootSpeed = 2f;
				ranged = true;
				rare = 2;
				consumable = true;
				return;
			case 1311:
				damage = 0;
				useStyle = 1;
				shoot = 268;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 6;
				noMelee = true;
				value = sellPrice(0, 3);
				buffType = 65;
				return;
			case 1312:
				damage = 0;
				useStyle = 1;
				shoot = 269;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 6;
				noMelee = true;
				value = sellPrice(0, 2, 50);
				buffType = 66;
				return;
			case 1313:
				autoReuse = true;
				rare = 2;
				mana = 18;
				UseSound = SoundID.Item8;
				noMelee = true;
				useStyle = 5;
				damage = 29;
				useAnimation = 26;
				useTime = 26;
				width = 24;
				height = 28;
				shoot = 837;
				scale = 0.9f;
				shootSpeed = 3.5f;
				knockBack = 3.5f;
				magic = true;
				value = sellPrice(0, 1, 50);
				return;
			case 1314:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 28;
				useTime = 28;
				knockBack = 6.5f;
				width = 30;
				height = 10;
				damage = 40;
				scale = 0.9f;
				shoot = 271;
				shootSpeed = 15f;
				UseSound = SoundID.Item174;
				rare = 4;
				value = sellPrice(0, 3, 50);
				melee = true;
				noMelee = true;
				if (Variant == ItemVariants.WeakerVariant)
				{
					rare = 4;
					value = sellPrice(0, 3, 50);
					damage = 18;
					useAnimation = 25;
					useTime = 25;
					knockBack = 5f;
					autoReuse = false;
				}
				return;
			case 1315:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				return;
			case 1316:
				width = 18;
				height = 18;
				defense = 21;
				headSlot = 99;
				rare = 8;
				value = 300000;
				return;
			case 1317:
				width = 18;
				height = 18;
				defense = 27;
				bodySlot = 65;
				rare = 8;
				value = 240000;
				return;
			case 1318:
				width = 18;
				height = 18;
				defense = 17;
				legSlot = 54;
				rare = 8;
				value = 180000;
				return;
			case 1319:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 19;
				useTime = 19;
				width = 44;
				height = 14;
				shoot = 166;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item11;
				damage = 10;
				shootSpeed = 11f;
				noMelee = true;
				value = 100000;
				knockBack = 1f;
				rare = 1;
				ranged = true;
				useAmmo = AmmoID.Snowball;
				shoot = 166;
				if (Variant == ItemVariants.StrongerVariant)
				{
					value = sellPrice(0, 5);
					rare = 5;
					damage = 22;
					useAnimation = 6;
					useTime = 6;
				}
				return;
			case 1320:
				useStyle = 1;
				useTurn = true;
				useAnimation = 19;
				useTime = 11;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 8;
				pick = 55;
				UseSound = SoundID.Item1;
				knockBack = 3f;
				rare = 1;
				value = buyPrice(0, 1, 50);
				scale = 1.15f;
				melee = true;
				return;
			case 1321:
				width = 24;
				height = 28;
				accessory = true;
				value = sellPrice(0, 5);
				rare = 4;
				backSlot = 7;
				return;
			case 1322:
				width = 24;
				height = 28;
				accessory = true;
				value = sellPrice(0, 2);
				rare = 3;
				return;
			case 1323:
				width = 24;
				height = 28;
				accessory = true;
				value = sellPrice(0, 2);
				rare = 3;
				faceSlot = 6;
				return;
			case 1324:
				autoReuse = true;
				noMelee = true;
				useStyle = 1;
				shootSpeed = 16f;
				shoot = 272;
				damage = 45;
				knockBack = 6.5f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 11;
				useTime = 11;
				noUseGraphic = true;
				rare = 5;
				value = sellPrice(0, 15);
				melee = true;
				return;
			case 1325:
				autoReuse = false;
				useStyle = 5;
				useAnimation = 20;
				useTime = 20;
				knockBack = 3.5f;
				width = 30;
				height = 10;
				damage = 12;
				shoot = 273;
				shootSpeed = 12f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = sellPrice(0, 0, 25);
				melee = true;
				noMelee = true;
				noUseGraphic = true;
				if (Variant == ItemVariants.StrongerVariant)
				{
					rare = 4;
					value = sellPrice(0, 3, 50);
					damage = 60;
					useAnimation = 15;
					useTime = 15;
					autoReuse = true;
				}
				return;
			case 1326:
				autoReuse = false;
				useStyle = 1;
				useAnimation = 20;
				useTime = 20;
				width = 20;
				height = 20;
				UseSound = SoundID.Item8;
				rare = 7;
				value = sellPrice(0, 10);
				return;
			case 1327:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 25;
				useTime = 25;
				knockBack = 5f;
				width = 24;
				height = 28;
				damage = 57;
				scale = 1.15f;
				UseSound = SoundID.Item71;
				rare = 6;
				shoot = 274;
				shootSpeed = 9f;
				value = eclipsePrice;
				melee = true;
				return;
			case 1328:
				width = 14;
				height = 18;
				maxStack = CommonMaxStack;
				rare = 7;
				value = 5000;
				return;
			case 1329:
				width = 14;
				height = 18;
				maxStack = CommonMaxStack;
				rare = 1;
				value = 750;
				return;
			case 1330:
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				value = 12;
				return;
			case 1331:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				rare = 1;
				return;
			case 1332:
				width = 12;
				height = 14;
				maxStack = CommonMaxStack;
				value = 4500;
				rare = 3;
				return;
			case 1333:
				flame = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 11;
				width = 10;
				height = 12;
				value = 160;
				rare = 1;
				return;
			case 1334:
				shootSpeed = 4.25f;
				shoot = 278;
				damage = 16;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 3f;
				value = 40;
				ranged = true;
				rare = 3;
				return;
			case 1335:
				shootSpeed = 5.25f;
				shoot = 279;
				damage = 13;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 4f;
				value = 30;
				ranged = true;
				rare = 3;
				return;
			case 1336:
				mana = 7;
				autoReuse = true;
				useStyle = 5;
				useAnimation = 18;
				useTime = 6;
				knockBack = 4f;
				width = 38;
				height = 10;
				damage = 30;
				shoot = 280;
				shootSpeed = 10f;
				UseSound = SoundID.Item13;
				rare = 4;
				value = sellPrice(0, 4);
				magic = true;
				noMelee = true;
				return;
			case 1337:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 209;
				placeStyle = 1;
				width = 12;
				height = 12;
				value = buyPrice(0, 50);
				return;
			case 1338:
				noUseGraphic = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 20;
				useTime = 20;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				damage = 350;
				noMelee = true;
				value = buyPrice(0, 0, 35);
				makeNPC = 614;
				return;
			case 1339:
				width = 12;
				height = 20;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 0, 15);
				return;
			case 1340:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 71;
				buffTime = 72000;
				value = sellPrice(0, 0, 5);
				rare = 4;
				return;
			case 1341:
				shootSpeed = 4.3f;
				shoot = 282;
				damage = 19;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 4.2f;
				value = 90;
				ranged = true;
				rare = 3;
				return;
			case 1342:
				shootSpeed = 5.3f;
				shoot = 283;
				damage = 15;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 4.1f;
				value = 40;
				ranged = true;
				rare = 3;
				return;
			case 1343:
				width = 16;
				height = 24;
				accessory = true;
				rare = 7;
				value = 300000;
				handOffSlot = 1;
				handOnSlot = 6;
				return;
			case 1344:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 272;
				width = 12;
				height = 12;
				value = buyPrice(0, 0, 7);
				return;
			case 1345:
				width = 12;
				height = 20;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 0, 2);
				noMelee = true;
				useStyle = 1;
				useAnimation = (useTime = 20);
				autoReuse = true;
				consumable = true;
				return;
			case 1346:
				width = 12;
				height = 20;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 0, 15);
				return;
			case 1347:
				width = 12;
				height = 20;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 0, 12);
				return;
			case 1348:
				width = 12;
				height = 20;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 0, 17);
				return;
			case 1349:
				shootSpeed = 5.1f;
				shoot = 284;
				damage = 10;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 5f;
				value = 10;
				ranged = true;
				rare = 3;
				return;
			case 1350:
				shootSpeed = 4.6f;
				shoot = 285;
				damage = 15;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 3.6f;
				value = 40;
				ranged = true;
				rare = 3;
				return;
			case 1351:
				shootSpeed = 4.7f;
				shoot = 286;
				damage = 10;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 6.6f;
				value = 40;
				ranged = true;
				rare = 3;
				return;
			case 1352:
				shootSpeed = 4.6f;
				shoot = 287;
				damage = 10;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 3.6f;
				value = 40;
				ranged = true;
				rare = 3;
				return;
			case 1353:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 73;
				buffTime = 72000;
				value = sellPrice(0, 0, 5);
				rare = 4;
				return;
			case 1354:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 74;
				buffTime = 72000;
				value = sellPrice(0, 0, 5);
				rare = 4;
				return;
			case 1355:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 75;
				buffTime = 72000;
				value = sellPrice(0, 0, 5);
				rare = 4;
				return;
			case 1356:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 76;
				buffTime = 72000;
				value = sellPrice(0, 0, 5);
				rare = 4;
				return;
			case 1357:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 77;
				buffTime = 72000;
				value = sellPrice(0, 0, 5);
				rare = 4;
				return;
			case 1358:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 78;
				buffTime = 72000;
				value = sellPrice(0, 0, 3);
				rare = 4;
				return;
			case 1359:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 79;
				buffTime = 72000;
				value = sellPrice(0, 0, 5);
				rare = 4;
				return;
			case 1360:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 0;
				rare = 1;
				return;
			case 1361:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 1;
				rare = 1;
				return;
			case 1362:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 2;
				rare = 1;
				return;
			case 1363:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 3;
				rare = 1;
				return;
			case 1364:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 4;
				rare = 1;
				return;
			case 1365:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 5;
				rare = 1;
				return;
			case 1366:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 6;
				rare = 1;
				return;
			case 1367:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 7;
				rare = 1;
				return;
			case 1368:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 8;
				rare = 1;
				return;
			case 1369:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 9;
				rare = 1;
				return;
			case 1370:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 10;
				rare = 1;
				return;
			case 1371:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 11;
				rare = 1;
				return;
			case 1372:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 12;
				return;
			case 1373:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 13;
				return;
			case 1374:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 14;
				return;
			case 1375:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 15;
				return;
			case 1376:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				placeStyle = 16;
				return;
			case 1377:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				placeStyle = 17;
				return;
			case 1378:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 100;
				width = 12;
				height = 12;
				return;
			case 1379:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 101;
				width = 12;
				height = 12;
				return;
			case 1380:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 102;
				width = 12;
				height = 12;
				return;
			case 1381:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 103;
				width = 12;
				height = 12;
				return;
			case 1382:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 104;
				width = 12;
				height = 12;
				return;
			case 1383:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 105;
				width = 12;
				height = 12;
				return;
			case 1384:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 6;
				width = 8;
				height = 10;
				return;
			case 1385:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 7;
				width = 8;
				height = 10;
				return;
			case 1386:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 8;
				width = 8;
				height = 10;
				return;
			case 1387:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 9;
				width = 8;
				height = 10;
				return;
			case 1388:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 10;
				width = 8;
				height = 10;
				return;
			case 1389:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 11;
				width = 8;
				height = 10;
				return;
			case 1390:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				value = sellPrice(0, 0, 0, 30);
				placeStyle = 1;
				return;
			case 1391:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				value = sellPrice(0, 0, 0, 30);
				placeStyle = 2;
				return;
			case 1392:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				value = sellPrice(0, 0, 0, 30);
				placeStyle = 3;
				return;
			case 1393:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				value = sellPrice(0, 0, 0, 30);
				placeStyle = 4;
				return;
			case 1394:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				value = sellPrice(0, 0, 0, 30);
				placeStyle = 5;
				return;
			case 1395:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				value = sellPrice(0, 0, 0, 30);
				placeStyle = 6;
				return;
			case 1396:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 13;
				width = 12;
				height = 30;
				value = 150;
				return;
			case 1397:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 10;
				width = 26;
				height = 20;
				value = 300;
				return;
			case 1398:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 11;
				width = 28;
				height = 14;
				value = 150;
				return;
			case 1399:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 14;
				width = 12;
				height = 30;
				value = 150;
				return;
			case 1400:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 11;
				width = 26;
				height = 20;
				value = 300;
				return;
			case 1401:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 12;
				width = 28;
				height = 14;
				value = 150;
				return;
			case 1402:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 15;
				width = 12;
				height = 30;
				value = 150;
				return;
			case 1403:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 12;
				width = 26;
				height = 20;
				value = 300;
				return;
			case 1404:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 13;
				width = 28;
				height = 14;
				value = 150;
				return;
			case 1405:
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 33;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				placeStyle = 1;
				return;
			case 1406:
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 33;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				placeStyle = 2;
				return;
			case 1407:
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 33;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				placeStyle = 3;
				return;
			case 1408:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 46;
				return;
			case 1409:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 47;
				return;
			case 1410:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 48;
				return;
			case 1411:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 16;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1412:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 17;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1413:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 18;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1414:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 1;
				return;
			case 1415:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 2;
				return;
			case 1416:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 3;
				return;
			case 1417:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 241;
				placeStyle = 0;
				width = 30;
				height = 30;
				return;
			case 1418:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 12;
				width = 8;
				height = 10;
				return;
			case 1419:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 18;
				return;
			case 1420:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 19;
				return;
			case 1421:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 0;
				return;
			case 1422:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 1;
				return;
			case 1423:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 2;
				return;
			case 1424:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 3;
				return;
			case 1425:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 4;
				return;
			case 1426:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 5;
				return;
			case 1427:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 20;
				return;
			case 1428:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 21;
				return;
			case 1429:
				width = 18;
				height = 18;
				headSlot = 100;
				vanity = true;
				value = buyPrice(0, 1);
				return;
			case 1430:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 243;
				width = 26;
				height = 20;
				value = buyPrice(0, 7);
				rare = 2;
				return;
			case 1431:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				value = sellPrice(0, 0, 5);
				placeStyle = 7;
				return;
			case 1432:
				width = 12;
				height = 20;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 0, 0, 5);
				return;
			case 1433:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 6;
				return;
			case 1434:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 7;
				return;
			case 1435:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 8;
				return;
			case 1436:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 9;
				return;
			case 1437:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 10;
				return;
			case 1438:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 11;
				return;
			case 1439:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 12;
				return;
			case 1440:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 22;
				return;
			case 1441:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 23;
				return;
			case 1442:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 24;
				return;
			case 1443:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 25;
				return;
			case 1444:
				mana = 7;
				UseSound = SoundID.Item72;
				useStyle = 5;
				damage = 80;
				useAnimation = 15;
				useTime = 15;
				autoReuse = true;
				width = 40;
				height = 40;
				shoot = 294;
				shootSpeed = 6f;
				knockBack = 3.25f;
				value = sellPrice(0, 6);
				magic = true;
				rare = 8;
				noMelee = true;
				return;
			case 1445:
				mana = 18;
				UseSound = SoundID.Item73;
				useStyle = 5;
				damage = 70;
				useAnimation = 30;
				useTime = 30;
				width = 40;
				height = 40;
				shoot = 295;
				shootSpeed = 8f;
				knockBack = 5f;
				value = sellPrice(0, 6);
				magic = true;
				noMelee = true;
				rare = 8;
				return;
			case 1446:
				mana = 15;
				UseSound = SoundID.Item43;
				useStyle = 5;
				damage = 65;
				autoReuse = true;
				useAnimation = 24;
				useTime = 24;
				width = 40;
				height = 40;
				shoot = 297;
				shootSpeed = 6f;
				knockBack = 6f;
				value = sellPrice(0, 6);
				magic = true;
				noMelee = true;
				rare = 8;
				return;
			case 1447:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 106;
				width = 12;
				height = 12;
				return;
			case 1448:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 107;
				width = 12;
				height = 12;
				return;
			case 1449:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 244;
				width = 26;
				height = 20;
				value = buyPrice(0, 4);
				rare = 1;
				return;
			case 1450:
				useStyle = 1;
				autoReuse = true;
				useTurn = false;
				useAnimation = 25;
				useTime = 25;
				width = 24;
				height = 28;
				scale = 1f;
				value = buyPrice(0, 5);
				noMelee = true;
				rare = 1;
				return;
			case 1451:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 10;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1452:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 11;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1453:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 12;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1454:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 13;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1455:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 14;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1456:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 15;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1457:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 13;
				width = 8;
				height = 10;
				return;
			case 1458:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 19;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1459:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 16;
				width = 12;
				height = 30;
				value = 150;
				return;
			case 1460:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 13;
				width = 26;
				height = 20;
				value = 300;
				return;
			case 1461:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 14;
				width = 28;
				height = 14;
				value = 150;
				return;
			case 1462:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 49;
				return;
			case 1463:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 4;
				return;
			case 1464:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 16;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1465:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 17;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1466:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 18;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1467:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 19;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1468:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 20;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1469:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 21;
				width = 10;
				height = 24;
				value = 1000;
				return;
			case 1470:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 79;
				placeStyle = 5;
				width = 28;
				height = 20;
				value = 2000;
				return;
			case 1471:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 79;
				placeStyle = 6;
				width = 28;
				height = 20;
				value = 2000;
				return;
			case 1472:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 79;
				placeStyle = 7;
				width = 28;
				height = 20;
				value = 2000;
				return;
			case 1473:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 79;
				placeStyle = 8;
				width = 28;
				height = 20;
				value = 2000;
				return;
			case 1474:
			case 1475:
			case 1476:
			case 1477:
			case 1478:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = type - 1474;
				return;
			}
			if (type >= 1479 && type <= 1494)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 246;
				width = 30;
				height = 30;
				if (type >= 1481 && type <= 1494)
				{
					value = buyPrice(0, 1);
				}
				else
				{
					value = sellPrice(0, 0, 10);
				}
				placeStyle = type - 1479;
				return;
			}
			switch (type)
			{
			case 1495:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 5;
				return;
			case 1496:
			case 1497:
			case 1498:
			case 1499:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 26 + type - 1496;
				return;
			}
			if (type >= 1500 && type <= 1502)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 13 + type - 1500;
				return;
			}
			switch (type)
			{
			case 1503:
				width = 18;
				height = 18;
				defense = 6;
				headSlot = 101;
				rare = 8;
				value = 375000;
				return;
			case 1504:
				width = 18;
				height = 18;
				defense = 14;
				bodySlot = 66;
				rare = 8;
				value = 300000;
				return;
			case 1505:
				width = 18;
				height = 18;
				defense = 10;
				legSlot = 55;
				rare = 8;
				value = 225000;
				return;
			case 1506:
				useStyle = 1;
				useAnimation = 24;
				useTime = 8;
				knockBack = 5.25f;
				useTurn = true;
				autoReuse = true;
				width = 20;
				height = 12;
				damage = 32;
				pick = 200;
				UseSound = SoundID.Item1;
				rare = 8;
				value = 216000;
				melee = true;
				scale = 1.15f;
				tileBoost += 3;
				return;
			case 1507:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 28;
				useTime = 8;
				knockBack = 7f;
				width = 20;
				height = 12;
				damage = 60;
				axe = 30;
				hammer = 90;
				UseSound = SoundID.Item1;
				rare = 8;
				value = 216000;
				melee = true;
				scale = 1.05f;
				tileBoost += 3;
				return;
			case 1508:
				maxStack = CommonMaxStack;
				width = 16;
				height = 14;
				value = sellPrice(0, 0, 50);
				rare = 8;
				return;
			case 1509:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 17;
				width = 12;
				height = 30;
				value = 150;
				return;
			case 1510:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 14;
				width = 26;
				height = 20;
				value = 300;
				return;
			case 1511:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 15;
				width = 28;
				height = 14;
				value = 150;
				return;
			case 1512:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 5;
				return;
			case 1513:
				noMelee = true;
				useStyle = 1;
				shootSpeed = 14f;
				shoot = 301;
				damage = 90;
				knockBack = 9f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				rare = 8;
				value = sellPrice(0, 10);
				melee = true;
				autoReuse = true;
				return;
			case 1514:
				width = 18;
				height = 18;
				headSlot = 102;
				rare = 1;
				value = sellPrice(0, 1);
				vanity = true;
				return;
			case 1515:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 15;
				return;
			case 1516:
			case 1517:
			case 1518:
			case 1519:
			case 1520:
			case 1521:
				maxStack = CommonMaxStack;
				width = 16;
				height = 14;
				value = sellPrice(0, 2, 50);
				rare = 5;
				return;
			}
			if (type >= 1522 && type <= 1527)
			{
				width = 20;
				height = 20;
				rare = 1;
				return;
			}
			if (type >= 1528 && type <= 1532)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				placeStyle = 18 + type - 1528;
				width = 26;
				height = 22;
				value = 2500;
				return;
			}
			if (type >= 1533 && type <= 1537)
			{
				width = 14;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 8;
				return;
			}
			if (type >= 1538 && type <= 1540)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 30 + type - 1538;
				return;
			}
			if (type >= 1541 && type <= 1542)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 246;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 16 + type - 1541;
				return;
			}
			if (type >= 1543 && type <= 1545)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				width = 24;
				height = 24;
				value = sellPrice(0, 6);
				tileBoost += 3;
				return;
			}
			switch (type)
			{
			case 1546:
				width = 18;
				height = 18;
				defense = 11;
				headSlot = 103;
				rare = 8;
				value = 375000;
				return;
			case 1547:
				width = 18;
				height = 18;
				defense = 11;
				headSlot = 104;
				rare = 8;
				value = 375000;
				return;
			case 1548:
				width = 18;
				height = 18;
				defense = 11;
				headSlot = 105;
				rare = 8;
				value = 375000;
				return;
			case 1549:
				width = 18;
				height = 18;
				defense = 24;
				bodySlot = 67;
				rare = 8;
				value = 300000;
				return;
			case 1550:
				width = 18;
				height = 18;
				defense = 16;
				legSlot = 56;
				rare = 8;
				value = 225000;
				return;
			case 1551:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 247;
				width = 26;
				height = 24;
				value = buyPrice(1);
				return;
			case 1552:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 7;
				value = sellPrice(0, 1);
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 20;
				return;
			case 1553:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 5;
				useTime = 5;
				crit += 10;
				width = 60;
				height = 26;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item40;
				damage = 85;
				shootSpeed = 12f;
				noMelee = true;
				value = 750000;
				rare = 10;
				knockBack = 2.5f;
				ranged = true;
				return;
			case 1554:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				headSlot = 106;
				value = sellPrice(0, 5);
				return;
			case 1555:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				bodySlot = 68;
				value = sellPrice(0, 5);
				return;
			case 1556:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				legSlot = 57;
				value = sellPrice(0, 5);
				return;
			case 1557:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				headSlot = 107;
				value = sellPrice(0, 5);
				return;
			case 1558:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				bodySlot = 69;
				value = sellPrice(0, 5);
				return;
			case 1559:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				legSlot = 58;
				value = sellPrice(0, 5);
				return;
			case 1560:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				headSlot = 108;
				value = sellPrice(0, 5);
				return;
			case 1561:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				bodySlot = 70;
				value = sellPrice(0, 5);
				return;
			case 1562:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				legSlot = 59;
				value = sellPrice(0, 5);
				return;
			case 1563:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				headSlot = 109;
				value = sellPrice(0, 5);
				return;
			case 1564:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				bodySlot = 71;
				value = sellPrice(0, 5);
				return;
			case 1565:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				legSlot = 60;
				value = sellPrice(0, 5);
				return;
			case 1566:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				headSlot = 110;
				value = sellPrice(0, 5);
				return;
			case 1567:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				bodySlot = 72;
				value = sellPrice(0, 5);
				return;
			case 1568:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				legSlot = 61;
				value = sellPrice(0, 5);
				return;
			case 1569:
				autoReuse = true;
				useStyle = 1;
				shootSpeed = 15f;
				shoot = 304;
				damage = 29;
				width = 18;
				height = 20;
				UseSound = SoundID.Item39;
				useAnimation = 16;
				useTime = 16;
				noUseGraphic = true;
				noMelee = true;
				value = sellPrice(0, 20);
				knockBack = 2.75f;
				melee = true;
				rare = 8;
				return;
			case 1570:
				width = 14;
				height = 18;
				maxStack = CommonMaxStack;
				rare = 8;
				value = eclipsePrice;
				return;
			case 1571:
				autoReuse = true;
				useStyle = 5;
				shootSpeed = 14f;
				shoot = 306;
				damage = 70;
				width = 18;
				height = 20;
				UseSound = SoundID.Item39;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				noMelee = true;
				value = sellPrice(0, 20);
				knockBack = 5f;
				melee = true;
				rare = 8;
				return;
			case 1572:
				useStyle = 1;
				shootSpeed = 14f;
				shoot = 308;
				damage = 100;
				width = 18;
				height = 20;
				UseSound = SoundID.Item1;
				useAnimation = 30;
				useTime = 30;
				noMelee = true;
				value = sellPrice(0, 20);
				knockBack = 7.5f;
				rare = 8;
				summon = true;
				mana = 20;
				sentry = true;
				return;
			case 1573:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 16;
				return;
			case 1574:
			case 1575:
			case 1576:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 33 + type - 1574;
				return;
			}
			switch (type)
			{
			case 1577:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 6;
				return;
			case 1578:
				width = 22;
				height = 22;
				accessory = true;
				rare = 3;
				value = 100000;
				neckSlot = 6;
				return;
			case 1579:
				width = 28;
				height = 24;
				accessory = true;
				rare = 1;
				value = 50000;
				shoeSlot = 5;
				return;
			case 1580:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				headSlot = 111;
				value = sellPrice(0, 5);
				return;
			case 1581:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				bodySlot = 73;
				value = sellPrice(0, 5);
				return;
			case 1582:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				legSlot = 62;
				value = sellPrice(0, 5);
				return;
			case 1583:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 16;
				value = 400000;
				return;
			case 1584:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 17;
				value = 400000;
				return;
			case 1585:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 18;
				value = 400000;
				return;
			case 1586:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 19;
				value = 400000;
				return;
			case 1587:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				bodySlot = 74;
				value = sellPrice(0, 5);
				return;
			case 1588:
				width = 18;
				height = 18;
				rare = 9;
				vanity = true;
				legSlot = 63;
				value = sellPrice(0, 5);
				return;
			case 1589:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 248;
				width = 12;
				height = 12;
				return;
			case 1590:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 109;
				width = 12;
				height = 12;
				return;
			case 1591:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 249;
				width = 12;
				height = 12;
				return;
			case 1592:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 110;
				width = 12;
				height = 12;
				return;
			case 1593:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 250;
				width = 12;
				height = 12;
				return;
			case 1594:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 111;
				width = 12;
				height = 12;
				return;
			case 1595:
				width = 22;
				height = 22;
				accessory = true;
				rare = 2;
				value = 100000;
				handOffSlot = 3;
				handOnSlot = 8;
				return;
			case 1596:
			case 1597:
			case 1598:
			case 1599:
			case 1600:
			case 1601:
			case 1602:
			case 1603:
			case 1604:
			case 1605:
			case 1606:
			case 1607:
			case 1608:
			case 1609:
			case 1610:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = type - 1596 + 13;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				return;
			}
			switch (type)
			{
			case 1611:
				maxStack = CommonMaxStack;
				width = 16;
				height = 14;
				value = sellPrice(0, 2, 50);
				rare = 5;
				return;
			case 1612:
				width = 16;
				height = 24;
				accessory = true;
				rare = 6;
				value = sellPrice(0, 3);
				return;
			case 1613:
				width = 24;
				height = 28;
				rare = 7;
				value = sellPrice(0, 5);
				accessory = true;
				defense = 4;
				shieldSlot = 4;
				return;
			case 1614:
				shootSpeed = 6f;
				shoot = 310;
				damage = 1;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Flare;
				knockBack = 1.5f;
				value = 7;
				ranged = true;
				return;
			case 1615:
			case 1616:
			case 1617:
			case 1618:
			case 1619:
			case 1620:
			case 1621:
			case 1622:
			case 1623:
			case 1624:
			case 1625:
			case 1626:
			case 1627:
			case 1628:
			case 1629:
			case 1630:
			case 1631:
			case 1632:
			case 1633:
			case 1634:
			case 1635:
			case 1636:
			case 1637:
			case 1638:
			case 1639:
			case 1640:
			case 1641:
			case 1642:
			case 1643:
			case 1644:
			case 1645:
			case 1646:
			case 1647:
			case 1648:
			case 1649:
			case 1650:
			case 1651:
			case 1652:
			case 1653:
			case 1654:
			case 1655:
			case 1656:
			case 1657:
			case 1658:
			case 1659:
			case 1660:
			case 1661:
			case 1662:
			case 1663:
			case 1664:
			case 1665:
			case 1666:
			case 1667:
			case 1668:
			case 1669:
			case 1670:
			case 1671:
			case 1672:
			case 1673:
			case 1674:
			case 1675:
			case 1676:
			case 1677:
			case 1678:
			case 1679:
			case 1680:
			case 1681:
			case 1682:
			case 1683:
			case 1684:
			case 1685:
			case 1686:
			case 1687:
			case 1688:
			case 1689:
			case 1690:
			case 1691:
			case 1692:
			case 1693:
			case 1694:
			case 1695:
			case 1696:
			case 1697:
			case 1698:
			case 1699:
			case 1700:
			case 1701:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 22 + type - 1615;
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				return;
			}
			switch (type)
			{
			case 1702:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 14;
				width = 8;
				height = 10;
				return;
			case 1703:
			case 1704:
			case 1705:
			case 1706:
			case 1707:
			case 1708:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 18 + type - 1703;
				width = 12;
				height = 30;
				value = 150;
				return;
			}
			if (type >= 1709 && type <= 1712)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 20 + type - 1709;
				width = 14;
				height = 28;
				value = 200;
				return;
			}
			if (type >= 1713 && type <= 1718)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 15 + type - 1713;
				width = 26;
				height = 20;
				value = 300;
				return;
			}
			if (type >= 1719 && type <= 1722)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 79;
				placeStyle = 9 + type - 1719;
				width = 28;
				height = 20;
				value = 2000;
				return;
			}
			switch (type)
			{
			case 1723:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 78;
				width = 12;
				height = 12;
				return;
			case 1724:
				width = 16;
				height = 24;
				accessory = true;
				rare = 2;
				value = 50000;
				waistSlot = 14;
				return;
			case 1725:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 251;
				width = 8;
				height = 10;
				value = sellPrice(0, 0, 0, 25);
				return;
			case 1726:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 113;
				width = 12;
				height = 12;
				return;
			case 1727:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 252;
				width = 8;
				height = 10;
				return;
			case 1728:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 114;
				width = 12;
				height = 12;
				return;
			case 1729:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 253;
				width = 8;
				height = 10;
				return;
			case 1730:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 115;
				width = 12;
				height = 12;
				return;
			case 1731:
				width = 18;
				height = 18;
				defense = 2;
				headSlot = 112;
				return;
			case 1732:
				width = 18;
				height = 18;
				defense = 3;
				bodySlot = 75;
				return;
			case 1733:
				width = 18;
				height = 18;
				defense = 2;
				legSlot = 64;
				return;
			case 1734:
				width = 12;
				height = 12;
				return;
			case 1735:
				width = 12;
				height = 12;
				return;
			case 1736:
				width = 18;
				height = 18;
				headSlot = 113;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1737:
				width = 18;
				height = 18;
				bodySlot = 76;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1738:
				width = 18;
				height = 18;
				legSlot = 65;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1739:
				width = 18;
				height = 18;
				headSlot = 114;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1740:
				width = 18;
				height = 18;
				headSlot = 115;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1741:
				width = 18;
				height = 18;
				bodySlot = 77;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1742:
				width = 18;
				height = 18;
				headSlot = 116;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1743:
				width = 18;
				height = 18;
				headSlot = 117;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1744:
				width = 18;
				height = 18;
				bodySlot = 78;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1745:
				width = 18;
				height = 18;
				legSlot = 66;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1746:
				width = 18;
				height = 18;
				headSlot = 118;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1747:
				width = 18;
				height = 18;
				bodySlot = 79;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1748:
				width = 18;
				height = 18;
				legSlot = 67;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1749:
				width = 18;
				height = 18;
				headSlot = 119;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1750:
				width = 18;
				height = 18;
				bodySlot = 80;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1751:
				width = 18;
				height = 18;
				legSlot = 68;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1752:
				width = 18;
				height = 18;
				headSlot = 120;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1753:
				width = 18;
				height = 18;
				bodySlot = 81;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1754:
				width = 18;
				height = 18;
				headSlot = 121;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1755:
				width = 18;
				height = 18;
				bodySlot = 82;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1756:
				width = 18;
				height = 18;
				legSlot = 69;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1757:
				width = 18;
				height = 18;
				headSlot = 122;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1758:
				width = 18;
				height = 18;
				bodySlot = 83;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1759:
				width = 18;
				height = 18;
				legSlot = 70;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1760:
				width = 18;
				height = 18;
				headSlot = 123;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1761:
				width = 18;
				height = 18;
				bodySlot = 84;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1762:
				width = 18;
				height = 18;
				legSlot = 71;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1763:
				width = 18;
				height = 18;
				headSlot = 124;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1764:
				width = 18;
				height = 18;
				bodySlot = 85;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1765:
				width = 18;
				height = 18;
				legSlot = 72;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1766:
				width = 18;
				height = 18;
				headSlot = 125;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1767:
				width = 18;
				height = 18;
				headSlot = 126;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1768:
				width = 18;
				height = 18;
				bodySlot = 86;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1769:
				width = 18;
				height = 18;
				legSlot = 73;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1770:
				width = 18;
				height = 18;
				bodySlot = 87;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1771:
				width = 18;
				height = 18;
				legSlot = 74;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1772:
				width = 18;
				height = 18;
				headSlot = 127;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1773:
				width = 18;
				height = 18;
				bodySlot = 88;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1774:
				width = 12;
				height = 12;
				rare = 3;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1);
				return;
			case 1775:
				width = 18;
				height = 18;
				bodySlot = 89;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1776:
				width = 18;
				height = 18;
				legSlot = 75;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1777:
				width = 18;
				height = 18;
				headSlot = 128;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1778:
				width = 18;
				height = 18;
				bodySlot = 90;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1779:
				width = 18;
				height = 18;
				headSlot = 129;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1780:
				width = 18;
				height = 18;
				bodySlot = 91;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1781:
				width = 18;
				height = 18;
				legSlot = 76;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1782:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 9;
				useTime = 9;
				crit += 6;
				width = 60;
				height = 26;
				shoot = 311;
				useAmmo = AmmoID.CandyCorn;
				UseSound = SoundID.Item11;
				damage = 44;
				shootSpeed = 10f;
				noMelee = true;
				value = sellPrice(0, 10);
				rare = 8;
				knockBack = 2f;
				ranged = true;
				return;
			case 1783:
				shootSpeed = 4f;
				shoot = 311;
				damage = 9;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.CandyCorn;
				knockBack = 1.5f;
				value = 5;
				ranged = true;
				return;
			case 1784:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 25;
				useTime = 25;
				crit += 6;
				width = 60;
				height = 26;
				shoot = 312;
				useAmmo = AmmoID.JackOLantern;
				UseSound = SoundID.Item11;
				damage = 65;
				shootSpeed = 7f;
				noMelee = true;
				value = sellPrice(0, 10);
				rare = 8;
				knockBack = 5f;
				ranged = true;
				return;
			case 1785:
				shootSpeed = 4f;
				shoot = 312;
				damage = 60;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.JackOLantern;
				knockBack = 3f;
				value = 15;
				ranged = true;
				return;
			case 1786:
				useStyle = 1;
				useTurn = true;
				useTime = 24;
				useAnimation = 24;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 9;
				UseSound = SoundID.Item1;
				knockBack = 2.25f;
				value = buyPrice(0, 0, 60);
				melee = true;
				return;
			case 1788:
				width = 18;
				height = 18;
				headSlot = 130;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1789:
				width = 18;
				height = 18;
				bodySlot = 92;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1790:
				width = 18;
				height = 18;
				legSlot = 77;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1791:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 96;
				placeStyle = 1;
				width = 20;
				height = 20;
				value = buyPrice(0, 1, 50);
				return;
			case 1792:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 24;
				width = 12;
				height = 30;
				value = 150;
				return;
			case 1793:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 24;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1794:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 21;
				width = 26;
				height = 20;
				value = 300;
				return;
			case 1795:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 16;
				width = 28;
				height = 14;
				value = 150;
				return;
			case 1796:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 15;
				width = 8;
				height = 10;
				return;
			case 1797:
				width = 24;
				height = 8;
				accessory = true;
				rare = 7;
				value = 400000;
				wingSlot = 20;
				return;
			case 1798:
				damage = 0;
				useStyle = 1;
				shoot = 313;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				buffType = 81;
				value = sellPrice(0, 2);
				return;
			case 1799:
				damage = 0;
				useStyle = 1;
				shoot = 314;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				buffType = 82;
				value = sellPrice(0, 2);
				return;
			case 1800:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 13.5f;
				shoot = 315;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 1, 50);
				return;
			case 1801:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 12;
				useTime = 12;
				mana = 6;
				width = 50;
				height = 18;
				shoot = 316;
				UseSound = SoundID.Item32;
				damage = 45;
				shootSpeed = 10f;
				noMelee = true;
				value = sellPrice(0, 10);
				rare = 8;
				magic = true;
				knockBack = 3f;
				return;
			case 1802:
				mana = 10;
				damage = 55;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 317;
				width = 26;
				height = 28;
				UseSound = SoundID.Item44;
				useAnimation = 28;
				useTime = 28;
				rare = 8;
				noMelee = true;
				knockBack = 3f;
				buffType = 83;
				value = sellPrice(0, 10);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				return;
			case 1803:
			case 1804:
			case 1805:
			case 1806:
			case 1807:
				return;
			}
			switch (type)
			{
			case 1808:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 8;
				return;
			case 1809:
				useStyle = 1;
				shootSpeed = 9f;
				shoot = 318;
				damage = 13;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 19;
				useTime = 19;
				noUseGraphic = true;
				noMelee = true;
				ranged = true;
				knockBack = 6.5f;
				return;
			case 1810:
				damage = 0;
				useStyle = 1;
				shoot = 319;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				buffType = 84;
				value = sellPrice(0, 2);
				return;
			case 1811:
				maxStack = CommonMaxStack;
				width = 16;
				height = 14;
				value = sellPrice(0, 2, 50);
				rare = 5;
				return;
			case 1812:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				placeStyle = 6;
				width = 26;
				height = 26;
				return;
			case 1813:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 35;
				width = 26;
				height = 26;
				return;
			case 1814:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 25;
				width = 12;
				height = 30;
				value = 150;
				return;
			case 1815:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 25;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1816:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 22;
				width = 26;
				height = 20;
				value = 300;
				return;
			case 1817:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				placeStyle = 17;
				width = 28;
				height = 14;
				value = 150;
				return;
			case 1818:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				placeStyle = 16;
				width = 8;
				height = 10;
				return;
			case 1819:
				width = 18;
				height = 18;
				headSlot = 131;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1820:
				width = 18;
				height = 18;
				bodySlot = 93;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1821:
				width = 18;
				height = 18;
				headSlot = 132;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1822:
				width = 18;
				height = 18;
				bodySlot = 94;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1823:
				width = 18;
				height = 18;
				legSlot = 78;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1824:
				width = 18;
				height = 18;
				headSlot = 133;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1825:
				noMelee = true;
				useStyle = 1;
				shootSpeed = 15f;
				shoot = 320;
				damage = 20;
				knockBack = 5f;
				width = 34;
				height = 34;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				rare = 2;
				value = 50000;
				melee = true;
				return;
			case 1826:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 26;
				useTime = 26;
				knockBack = 7.5f;
				width = 40;
				height = 40;
				damage = 150;
				scale = 1f;
				UseSound = SoundID.Item1;
				rare = 8;
				value = sellPrice(0, 10);
				melee = true;
				shoot = 997;
				noMelee = true;
				shootsEveryUse = true;
				return;
			case 1827:
				useStyle = 1;
				useTurn = true;
				autoReuse = true;
				useAnimation = 8;
				useTime = 8;
				width = 24;
				height = 28;
				damage = 14;
				knockBack = 4f;
				UseSound = SoundID.Item1;
				scale = 1.35f;
				melee = true;
				rare = 2;
				value = 50000;
				melee = true;
				return;
			case 1828:
				autoReuse = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 254;
				width = 8;
				height = 10;
				value = buyPrice(0, 0, 2, 50);
				return;
			case 1829:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 15.5f;
				shoot = 322;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 7;
				noMelee = true;
				value = sellPrice(0, 4);
				return;
			case 1830:
				width = 24;
				height = 8;
				accessory = true;
				rare = 7;
				value = 400000;
				wingSlot = 21;
				return;
			case 1831:
				maxStack = CommonMaxStack;
				width = 16;
				height = 14;
				value = sellPrice(0, 2, 50);
				rare = 5;
				return;
			case 1832:
				width = 18;
				height = 18;
				headSlot = 134;
				value = sellPrice(0, 1);
				defense = 9;
				rare = 8;
				return;
			case 1833:
				width = 18;
				height = 18;
				bodySlot = 95;
				value = sellPrice(0, 1);
				defense = 11;
				rare = 8;
				return;
			case 1834:
				width = 18;
				height = 18;
				legSlot = 79;
				value = sellPrice(0, 1);
				defense = 10;
				rare = 8;
				return;
			case 1835:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 12;
				useTime = 12;
				crit += 10;
				width = 40;
				height = 26;
				shoot = 323;
				useAmmo = AmmoID.Stake;
				UseSound = SoundID.Item5;
				damage = 75;
				shootSpeed = 9f;
				noMelee = true;
				value = sellPrice(0, 10);
				rare = 8;
				knockBack = 6.5f;
				ranged = true;
				return;
			case 1836:
				shootSpeed = 3f;
				shoot = 323;
				damage = 25;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Stake;
				knockBack = 4.5f;
				value = 15;
				ranged = true;
				return;
			case 1837:
				useStyle = 1;
				shoot = 324;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				buffType = 85;
				value = sellPrice(0, 2);
				return;
			case 1838:
				width = 18;
				height = 18;
				headSlot = 135;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1839:
				width = 18;
				height = 18;
				bodySlot = 96;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1840:
				width = 18;
				height = 18;
				legSlot = 80;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1841:
				width = 18;
				height = 18;
				headSlot = 136;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1842:
				width = 18;
				height = 18;
				bodySlot = 97;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1843:
				width = 18;
				height = 18;
				legSlot = 81;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1844:
				useStyle = 4;
				width = 22;
				height = 14;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				maxStack = CommonMaxStack;
				rare = 8;
				return;
			case 1845:
				rare = 8;
				width = 24;
				height = 28;
				accessory = true;
				value = buyPrice(0, 20);
				return;
			case 1846:
			case 1847:
			case 1848:
			case 1849:
			case 1850:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 17 + type - 1846;
				return;
			}
			switch (type)
			{
			case 1851:
				width = 18;
				height = 18;
				bodySlot = 98;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1852:
				width = 18;
				height = 18;
				legSlot = 82;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1853:
				width = 18;
				height = 18;
				bodySlot = 99;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1854:
				width = 18;
				height = 18;
				legSlot = 83;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 1855:
			case 1856:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				rare = 1;
				placeStyle = 36 + type - 1855;
				return;
			case 1857:
				width = 18;
				height = 18;
				headSlot = 137;
				value = sellPrice(0, 5);
				vanity = true;
				rare = 3;
				return;
			case 1858:
				width = 14;
				height = 28;
				rare = 7;
				value = 300000;
				accessory = true;
				return;
			case 1859:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 9;
				value = 75000;
				rare = 2;
				return;
			case 1860:
				width = 24;
				height = 28;
				rare = 5;
				value = 150000;
				accessory = true;
				faceSlot = 3;
				return;
			case 1861:
				width = 24;
				height = 28;
				rare = 6;
				value = 250000;
				accessory = true;
				faceSlot = 2;
				return;
			case 1862:
				width = 16;
				height = 24;
				accessory = true;
				rare = 7;
				value = 350000;
				shoeSlot = 9;
				return;
			case 1863:
				width = 14;
				height = 28;
				rare = 4;
				value = 150000;
				accessory = true;
				balloonSlot = 5;
				return;
			case 1864:
				rare = 8;
				width = 24;
				height = 28;
				accessory = true;
				value = buyPrice(0, 25);
				return;
			case 1865:
				width = 16;
				height = 24;
				accessory = true;
				rare = 7;
				value = 400000;
				return;
			case 1866:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 22;
				return;
			case 1867:
				width = 12;
				height = 12;
				return;
			case 1868:
				width = 12;
				height = 12;
				return;
			case 1869:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 36;
				width = 12;
				height = 28;
				rare = 1;
				return;
			case 1870:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 38;
				useTime = 38;
				width = 44;
				height = 14;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item11;
				damage = 20;
				shootSpeed = 8f;
				noMelee = true;
				value = 100000;
				knockBack = 3.75f;
				rare = 1;
				ranged = true;
				return;
			case 1871:
				width = 24;
				height = 8;
				accessory = true;
				value = 400000;
				rare = 5;
				wingSlot = 23;
				return;
			case 1872:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 170;
				width = 12;
				height = 12;
				return;
			case 1873:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 171;
				width = 12;
				height = 12;
				value = buyPrice(0, 0, 25);
				return;
			case 1874:
			case 1875:
			case 1876:
			case 1877:
			case 1878:
			case 1879:
			case 1880:
			case 1881:
			case 1882:
			case 1883:
			case 1884:
			case 1885:
			case 1886:
			case 1887:
			case 1888:
			case 1889:
			case 1890:
			case 1891:
			case 1892:
			case 1893:
			case 1894:
			case 1895:
			case 1896:
			case 1897:
			case 1898:
			case 1899:
			case 1900:
			case 1901:
			case 1902:
			case 1903:
			case 1904:
			case 1905:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noMelee = true;
				value = buyPrice(0, 0, 5);
				return;
			}
			switch (type)
			{
			case 1906:
				width = 18;
				height = 18;
				headSlot = 138;
				vanity = true;
				value = buyPrice(0, 1);
				return;
			case 1907:
				width = 18;
				height = 18;
				headSlot = 139;
				vanity = true;
				value = buyPrice(0, 1);
				return;
			case 1908:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 246;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 18;
				return;
			case 1909:
				useStyle = 1;
				useTime = 27;
				useAnimation = 27;
				knockBack = 5.3f;
				width = 24;
				height = 28;
				damage = 19;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = 13500;
				melee = true;
				return;
			case 1910:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 30;
				useTime = 6;
				width = 50;
				height = 18;
				shoot = 85;
				useAmmo = AmmoID.Gel;
				UseSound = SoundID.Item34;
				damage = 53;
				knockBack = 0.425f;
				shootSpeed = 8.5f;
				noMelee = true;
				value = 500000;
				rare = 8;
				ranged = true;
				return;
			case 1912:
				UseSound = SoundID.Item3;
				healLife = 80;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				potion = true;
				value = 40;
				rare = 1;
				return;
			case 1913:
				useStyle = 1;
				shootSpeed = 12f;
				shoot = 330;
				damage = 14;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 25;
				ranged = true;
				return;
			case 1914:
				useStyle = 1;
				width = 16;
				height = 30;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				mountType = 0;
				value = sellPrice(0, 5);
				return;
			case 1915:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 11.5f;
				shoot = 331;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 7;
				noMelee = true;
				value = sellPrice(0, 2);
				return;
			case 1916:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 15.5f;
				shoot = 332;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 7;
				noMelee = true;
				value = sellPrice(0, 4);
				return;
			case 1917:
				useStyle = 1;
				useTurn = true;
				useAnimation = 20;
				useTime = 16;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 7;
				pick = 55;
				UseSound = SoundID.Item1;
				knockBack = 2.5f;
				value = 10000;
				melee = true;
				return;
			case 1918:
				noMelee = true;
				useStyle = 1;
				shootSpeed = 11f;
				shoot = 333;
				damage = 19;
				knockBack = 8f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				rare = 1;
				value = 50000;
				melee = true;
				return;
			case 1921:
				width = 16;
				height = 24;
				accessory = true;
				rare = 2;
				value = 50000;
				handOffSlot = 2;
				handOnSlot = 7;
				return;
			case 1922:
				width = 16;
				height = 24;
				return;
			case 1923:
				width = 16;
				height = 24;
				accessory = true;
				rare = 2;
				value = 50000;
				return;
			case 1924:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 26;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 1925:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				placeStyle = 26;
				width = 12;
				height = 30;
				value = 150;
				return;
			case 1926:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				placeStyle = 23;
				width = 26;
				height = 20;
				value = 300;
				return;
			case 1927:
				useStyle = 1;
				shoot = 334;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = 0;
				buffType = 91;
				return;
			case 1928:
				useStyle = 1;
				autoReuse = true;
				useAnimation = 23;
				useTime = 23;
				knockBack = 7f;
				width = 40;
				height = 40;
				damage = 86;
				scale = 1.1f;
				shoot = 907;
				shootSpeed = 5f;
				UseSound = SoundID.Item1;
				rare = 8;
				value = sellPrice(0, 10);
				melee = true;
				return;
			case 1929:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 4;
				useTime = 4;
				width = 50;
				height = 18;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item41;
				damage = 31;
				shootSpeed = 14f;
				noMelee = true;
				value = buyPrice(0, 45);
				rare = 8;
				knockBack = 1.75f;
				ranged = true;
				return;
			case 1930:
				autoReuse = true;
				mana = 5;
				UseSound = SoundID.Item39;
				useStyle = 5;
				damage = 48;
				useAnimation = 8;
				useTime = 8;
				width = 40;
				height = 40;
				shoot = 336;
				shootSpeed = 12f;
				knockBack = 3.25f;
				value = buyPrice(0, 45);
				magic = true;
				rare = 8;
				noMelee = true;
				return;
			case 1931:
				autoReuse = true;
				mana = 9;
				useStyle = 5;
				damage = 58;
				useAnimation = 10;
				useTime = 5;
				width = 40;
				height = 40;
				shoot = 337;
				shootSpeed = 10f;
				knockBack = 4.5f;
				value = buyPrice(0, 45);
				magic = true;
				rare = 8;
				noMelee = true;
				return;
			case 1932:
				width = 18;
				height = 18;
				headSlot = 140;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1933:
				width = 18;
				height = 18;
				bodySlot = 100;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1934:
				width = 18;
				height = 18;
				legSlot = 84;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1935:
				width = 18;
				height = 18;
				headSlot = 142;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1936:
				width = 18;
				height = 18;
				bodySlot = 102;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1937:
				width = 18;
				height = 18;
				legSlot = 86;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1940:
				width = 18;
				height = 18;
				headSlot = 141;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1941:
				width = 18;
				height = 18;
				bodySlot = 101;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1942:
				width = 18;
				height = 18;
				legSlot = 85;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1938:
				width = 18;
				height = 18;
				headSlot = 143;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1939:
				width = 18;
				height = 18;
				bodySlot = 103;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1943:
				width = 18;
				height = 18;
				headSlot = 144;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1944:
				width = 18;
				height = 18;
				bodySlot = 104;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1945:
				width = 18;
				height = 18;
				legSlot = 87;
				vanity = true;
				value = buyPrice(0, 3);
				return;
			case 1946:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 15;
				useTime = 15;
				useAmmo = AmmoID.Rocket;
				width = 50;
				height = 20;
				shoot = 338;
				UseSound = SoundID.Item11;
				damage = 67;
				shootSpeed = 15f;
				noMelee = true;
				value = buyPrice(0, 45);
				knockBack = 4f;
				rare = 8;
				ranged = true;
				return;
			case 1947:
				useStyle = 5;
				useAnimation = 30;
				useTime = 30;
				shootSpeed = 4.75f;
				knockBack = 6.7f;
				width = 40;
				height = 40;
				damage = 73;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				shoot = 342;
				rare = 7;
				value = buyPrice(0, 45);
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				return;
			case 1948:
			case 1949:
			case 1950:
			case 1951:
			case 1952:
			case 1953:
			case 1954:
			case 1955:
			case 1956:
			case 1957:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 116 + type - 1948;
				width = 12;
				height = 12;
				value = buyPrice(0, 0, 0, 75);
				return;
			}
			switch (type)
			{
			case 1958:
				useStyle = 4;
				width = 22;
				height = 14;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				maxStack = CommonMaxStack;
				rare = 8;
				return;
			case 1959:
				useStyle = 1;
				shoot = 353;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 2);
				buffType = 92;
				return;
			case 1960:
			case 1961:
			case 1962:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				rare = 1;
				placeStyle = 38 + type - 1960;
				return;
			case 1963:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 28;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				return;
			case 1964:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 29;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				return;
			case 1965:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 30;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				return;
			case 1966:
				paint = 28;
				width = 24;
				height = 24;
				value = 25;
				maxStack = CommonMaxStack;
				return;
			case 1967:
				paint = 29;
				width = 24;
				height = 24;
				value = 50;
				maxStack = CommonMaxStack;
				return;
			case 1968:
				paint = 30;
				width = 24;
				height = 24;
				value = 75;
				maxStack = CommonMaxStack;
				return;
			case 1969:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 1970:
			case 1971:
			case 1972:
			case 1973:
			case 1974:
			case 1975:
			case 1976:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 262 + type - 1970;
				width = 12;
				height = 12;
				return;
			}
			if (type >= 1977 && type <= 1986)
			{
				width = 20;
				height = 26;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 5);
				rare = 2;
				if (type == 1980)
				{
					value = buyPrice(0, 10);
				}
				if (type == 1984)
				{
					value = buyPrice(0, 7, 50);
				}
				if (type == 1985)
				{
					value = buyPrice(0, 15);
				}
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				consumable = true;
				return;
			}
			switch (type)
			{
			case 1987:
				width = 18;
				height = 12;
				maxStack = 1;
				value = buyPrice(0, 40);
				rare = 5;
				accessory = true;
				vanity = true;
				hasVanityEffects = true;
				break;
			case 1988:
				width = 20;
				height = 14;
				maxStack = 1;
				value = buyPrice(0, 3, 50);
				vanity = true;
				headSlot = 145;
				break;
			case 1989:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 269;
				width = 22;
				height = 32;
				createTile = 470;
				placeStyle = 2;
				break;
			case 1990:
				width = 20;
				height = 26;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 2);
				rare = 2;
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				hairDye = 0;
				useAnimation = 17;
				useTime = 17;
				consumable = true;
				break;
			case 1991:
				useTurn = true;
				useStyle = 1;
				useTime = 25;
				useAnimation = 25;
				width = 24;
				height = 28;
				UseSound = SoundID.Item1;
				value = buyPrice(0, 0, 25);
				autoReuse = true;
				break;
			case 1992:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 355;
				noUseGraphic = true;
				bait = 20;
				break;
			case 1993:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 270;
				width = 12;
				height = 28;
				break;
			case 1994:
			case 1995:
			case 1996:
			case 1997:
			case 1998:
			case 1999:
			case 2000:
			case 2001:
			{
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 356;
				placeStyle = 1 + type - 1994;
				noUseGraphic = true;
				int num = type - 1994;
				if (num == 0)
				{
					bait = 5;
				}
				if (num == 4)
				{
					bait = 10;
				}
				if (num == 6)
				{
					bait = 15;
				}
				if (num == 3)
				{
					bait = 20;
				}
				if (num == 7)
				{
					bait = 25;
				}
				if (num == 2)
				{
					bait = 30;
				}
				if (num == 1)
				{
					bait = 35;
				}
				if (num == 5)
				{
					bait = 50;
				}
				break;
			}
			}
		}

		public void SetDefaults3(int type)
		{
			switch (type)
			{
			case 2002:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 357;
				noUseGraphic = true;
				bait = 25;
				return;
			case 2003:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 300;
				noUseGraphic = true;
				return;
			case 2004:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 358;
				noUseGraphic = true;
				bait = 35;
				return;
			case 2005:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 271;
				width = 12;
				height = 28;
				return;
			case 2006:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 359;
				noUseGraphic = true;
				bait = 10;
				return;
			case 2007:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 360;
				noUseGraphic = true;
				bait = 15;
				return;
			case 2008:
			case 2009:
			case 2010:
			case 2011:
			case 2012:
			case 2013:
			case 2014:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 126 + type - 2008;
				width = 12;
				height = 12;
				value = buyPrice(0, 0, 0, 75);
				return;
			}
			if (type >= 2015 && type <= 2019)
			{
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				if (type == 2015)
				{
					makeNPC = 74;
				}
				if (type == 2016)
				{
					makeNPC = 297;
				}
				if (type == 2017)
				{
					makeNPC = 298;
				}
				if (type == 2018)
				{
					makeNPC = 299;
				}
				if (type == 2019)
				{
					makeNPC = 46;
				}
				return;
			}
			switch (type)
			{
			case 2020:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 6;
				return;
			case 2021:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 7;
				return;
			case 2022:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 8;
				return;
			case 2023:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 9;
				return;
			case 2024:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 10;
				return;
			case 2025:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 11;
				return;
			case 2026:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 12;
				return;
			case 2027:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 13;
				return;
			case 2028:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 14;
				return;
			case 2029:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 15;
				return;
			case 2030:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 16;
				return;
			case 2031:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 17;
				return;
			case 2032:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 10;
				value = 150;
				return;
			case 2033:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 11;
				value = 150;
				return;
			case 2034:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 12;
				value = 150;
				return;
			case 2035:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 13;
				value = 150;
				return;
			case 2036:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 14;
				value = 150;
				return;
			case 2037:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 15;
				value = 150;
				return;
			case 2038:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 16;
				value = 150;
				return;
			case 2039:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 17;
				value = 150;
				return;
			case 2040:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 18;
				value = 150;
				return;
			case 2041:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 19;
				value = 150;
				return;
			case 2042:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 20;
				value = 150;
				return;
			case 2043:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 21;
				value = 150;
				return;
			case 2044:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				placeStyle = 27;
				width = 14;
				height = 28;
				value = 200;
				return;
			case 2045:
			case 2046:
			case 2047:
			case 2048:
			case 2049:
			case 2050:
			case 2051:
			case 2052:
			case 2053:
			case 2054:
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 33;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				placeStyle = 4 + type - 2045;
				return;
			}
			if (type >= 2055 && type <= 2065)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				placeStyle = 7 + type - 2055;
				width = 26;
				height = 26;
				value = 3000;
				return;
			}
			if (type >= 2066 && type <= 2071)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				autoReuse = true;
				createTile = 79;
				placeStyle = 13 + type - 2066;
				width = 28;
				height = 20;
				value = 2000;
				return;
			}
			if (type >= 2072 && type <= 2081)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 90;
				placeStyle = type + 1 - 2072;
				width = 20;
				height = 20;
				value = 300;
				return;
			}
			if (type >= 2082 && type <= 2091)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 93;
				placeStyle = type + 1 - 2082;
				width = 10;
				height = 24;
				value = 500;
				return;
			}
			if (type >= 2092 && type <= 2103)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 100;
				placeStyle = type + 1 - 2092;
				width = 20;
				height = 20;
				value = 1500;
				return;
			}
			if (type >= 2104 && type <= 2113)
			{
				width = 28;
				height = 20;
				headSlot = type + 146 - 2104;
				rare = 1;
				value = sellPrice(0, 0, 75);
				vanity = true;
				return;
			}
			if (type >= 2114 && type <= 2118)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 5);
				placeStyle = 41 + type - 2114;
				maxStack = CommonMaxStack;
				return;
			}
			switch (type)
			{
			case 2119:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 273;
				width = 12;
				height = 12;
				return;
			case 2120:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 274;
				width = 12;
				height = 12;
				return;
			case 2121:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 361;
				noUseGraphic = true;
				return;
			case 2122:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 362;
				noUseGraphic = true;
				return;
			case 2123:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 364;
				noUseGraphic = true;
				return;
			case 2124:
			case 2125:
			case 2126:
			case 2127:
			case 2128:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 90;
				placeStyle = type + 11 - 2124;
				width = 20;
				height = 20;
				value = 300;
				return;
			}
			if (type >= 2129 && type <= 2134)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 93;
				placeStyle = type + 11 - 2129;
				width = 10;
				height = 24;
				value = 500;
				return;
			}
			if (type >= 2135 && type <= 2138)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 18 + type - 2135;
				return;
			}
			switch (type)
			{
			case 2139:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				autoReuse = true;
				createTile = 79;
				placeStyle = 19;
				width = 28;
				height = 20;
				value = 2000;
				return;
			case 2140:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				autoReuse = true;
				createTile = 79;
				placeStyle = 20;
				width = 28;
				height = 20;
				value = 2000;
				return;
			case 2141:
			case 2142:
			case 2143:
			case 2144:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				placeStyle = 18 + type - 2141;
				width = 26;
				height = 26;
				value = 3000;
				return;
			}
			if (type >= 2145 && type <= 2148)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 22 + type - 2145;
				value = 150;
				return;
			}
			if (type >= 2149 && type <= 2152)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 100;
				placeStyle = type + 13 - 2149;
				width = 20;
				height = 20;
				value = 1500;
				return;
			}
			if (type >= 2153 && type <= 2155)
			{
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 33;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				placeStyle = 14 + type - 2153;
				return;
			}
			switch (type)
			{
			case 2156:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 366;
				noUseGraphic = true;
				bait = 15;
				return;
			case 2157:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 367;
				noUseGraphic = true;
				bait = 10;
				return;
			case 2158:
			case 2159:
			case 2160:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 133 + type - 2158;
				width = 12;
				height = 12;
				value = buyPrice(0, 0, 0, 75);
				return;
			}
			switch (type)
			{
			case 2161:
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				value = 50000;
				rare = 5;
				return;
			case 2162:
			case 2163:
			case 2164:
			case 2165:
			case 2166:
			case 2167:
			case 2168:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 275 + type - 2162;
				width = 12;
				height = 12;
				return;
			}
			switch (type)
			{
			case 2169:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 136;
				width = 12;
				height = 12;
				return;
			case 2170:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 137;
				width = 12;
				height = 12;
				return;
			case 2171:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 199;
				width = 14;
				height = 14;
				value = 500;
				return;
			case 2172:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 283;
				width = 28;
				height = 14;
				value = 500;
				return;
			case 2173:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 284;
				width = 12;
				height = 12;
				return;
			case 2174:
			case 2175:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 285 + type - 2174;
				width = 12;
				height = 12;
				return;
			}
			switch (type)
			{
			case 2176:
				useStyle = 1;
				useAnimation = 12;
				useTime = 4;
				knockBack = 6f;
				useTurn = true;
				autoReuse = true;
				width = 20;
				height = 12;
				damage = 45;
				pick = 200;
				axe = 25;
				UseSound = SoundID.Item1;
				rare = 8;
				value = sellPrice(0, 4);
				melee = true;
				tileBoost--;
				return;
			case 2177:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 287;
				width = 22;
				height = 22;
				value = buyPrice(0, 10);
				rare = 6;
				return;
			case 2178:
			case 2179:
			case 2180:
			case 2181:
			case 2182:
			case 2183:
			case 2184:
			case 2185:
			case 2186:
			case 2187:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 288 + type - 2178;
				width = 12;
				height = 12;
				return;
			}
			switch (type)
			{
			case 2189:
				width = 18;
				height = 18;
				defense = 18;
				headSlot = 156;
				rare = 8;
				value = 375000;
				return;
			case 2188:
				mana = 25;
				UseSound = SoundID.Item43;
				useStyle = 5;
				damage = 44;
				useAnimation = 30;
				useTime = 30;
				width = 40;
				height = 40;
				shoot = 355;
				shootSpeed = 14f;
				knockBack = 7f;
				magic = true;
				autoReuse = true;
				rare = 7;
				noMelee = true;
				value = sellPrice(0, 7);
				return;
			case 2190:
			case 2191:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 298 + type - 2190;
				width = 12;
				height = 12;
				return;
			}
			if (type < 2192 || type > 2198)
			{
				switch (type)
				{
				case 2203:
				case 2204:
					break;
				case 2199:
					width = 18;
					height = 18;
					defense = 23;
					headSlot = 157;
					rare = 8;
					value = 300000;
					return;
				case 2200:
					width = 18;
					height = 18;
					defense = 20;
					bodySlot = 105;
					rare = 8;
					value = 240000;
					return;
				case 2201:
					width = 18;
					height = 18;
					defense = 32;
					bodySlot = 106;
					rare = 8;
					value = 240000;
					return;
				case 2202:
					width = 18;
					height = 18;
					defense = 18;
					legSlot = 98;
					rare = 8;
					value = 180000;
					return;
				case 2205:
					useStyle = 1;
					autoReuse = true;
					useTurn = true;
					useAnimation = 15;
					useTime = 10;
					maxStack = CommonMaxStack;
					consumable = true;
					width = 12;
					height = 12;
					makeNPC = 148;
					noUseGraphic = true;
					return;
				case 2206:
				case 2207:
					useStyle = 1;
					useTurn = true;
					useAnimation = 15;
					useTime = 10;
					autoReuse = true;
					maxStack = CommonMaxStack;
					consumable = true;
					createTile = 309 + type - 2206;
					width = 12;
					height = 12;
					return;
				case 2208:
					width = 18;
					height = 20;
					maxStack = CommonMaxStack;
					return;
				case 2209:
					UseSound = SoundID.Item3;
					healMana = 300;
					useStyle = 9;
					useTurn = true;
					useAnimation = 17;
					useTime = 17;
					maxStack = CommonMaxStack;
					consumable = true;
					width = 14;
					height = 24;
					rare = 4;
					value = 1500;
					return;
				case 2210:
				case 2211:
				case 2212:
				case 2213:
					useStyle = 1;
					useTurn = true;
					useAnimation = 15;
					useTime = 7;
					autoReuse = true;
					maxStack = CommonMaxStack;
					consumable = true;
					createWall = 138 + type - 2210;
					width = 12;
					height = 12;
					return;
				default:
					if (type >= 2214 && type <= 2217)
					{
						width = 30;
						height = 30;
						accessory = true;
						rare = 3;
						value = buyPrice(0, 10);
						return;
					}
					switch (type)
					{
					case 2218:
						width = 14;
						height = 18;
						maxStack = CommonMaxStack;
						rare = 8;
						value = sellPrice(0, 0, 50);
						return;
					case 2219:
						width = 24;
						height = 24;
						accessory = true;
						value = buyPrice(0, 15);
						rare = 4;
						return;
					case 2220:
						width = 24;
						height = 24;
						accessory = true;
						value = buyPrice(0, 16);
						rare = 5;
						return;
					case 2221:
						width = 24;
						height = 24;
						accessory = true;
						rare = 5;
						value = buyPrice(0, 16);
						handOffSlot = 10;
						handOnSlot = 17;
						return;
					case 2222:
						width = 18;
						height = 18;
						headSlot = 158;
						vanity = true;
						value = sellPrice(0, 0, 25);
						return;
					case 2223:
						autoReuse = true;
						useStyle = 5;
						useAnimation = 20;
						useTime = 20;
						width = 50;
						height = 18;
						shoot = 10;
						useAmmo = AmmoID.Arrow;
						UseSound = SoundID.Item75;
						crit = 7;
						damage = 80;
						knockBack = 3f;
						shootSpeed = 7.75f;
						noMelee = true;
						value = buyPrice(0, 45);
						rare = 8;
						ranged = true;
						return;
					case 2224:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 34;
						placeStyle = 22;
						width = 26;
						height = 26;
						value = 160;
						return;
					case 2225:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 93;
						placeStyle = 17;
						width = 10;
						height = 24;
						value = 120;
						return;
					case 2226:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 42;
						width = 12;
						height = 28;
						placeStyle = 26;
						value = 200;
						return;
					case 2227:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 100;
						placeStyle = 17;
						width = 20;
						height = 20;
						value = 120;
						return;
					case 2228:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 15;
						placeStyle = 27;
						width = 12;
						height = 30;
						value = 150;
						return;
					case 2229:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 18;
						placeStyle = 18;
						width = 28;
						height = 14;
						value = 150;
						return;
					case 2230:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 21;
						placeStyle = 28;
						width = 26;
						height = 22;
						value = 320;
						return;
					case 2231:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						autoReuse = true;
						createTile = 79;
						placeStyle = 21;
						width = 28;
						height = 20;
						value = 600;
						return;
					case 2232:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 90;
						placeStyle = 16;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2233:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 101;
						width = 20;
						height = 20;
						value = 300;
						placeStyle = 22;
						return;
					case 2234:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 13;
						placeStyle = 5;
						width = 16;
						height = 24;
						value = 20;
						return;
					case 2235:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 103;
						placeStyle = 1;
						width = 16;
						height = 24;
						value = 20;
						return;
					case 2236:
						noWet = true;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 33;
						width = 8;
						height = 18;
						value = sellPrice(0, 0, 0, 60);
						placeStyle = 17;
						return;
					case 2237:
					case 2238:
					case 2239:
					case 2240:
					case 2241:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 104;
						placeStyle = 1 + type - 2237;
						width = 20;
						height = 20;
						value = 300;
						return;
					}
					switch (type)
					{
					case 2242:
					case 2243:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 103;
						placeStyle = 2 + type - 2242;
						width = 16;
						height = 24;
						value = 20;
						if (type == 2242)
						{
							value = buyPrice(0, 0, 20);
						}
						return;
					case 2244:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 13;
						placeStyle = 6;
						width = 16;
						height = 24;
						value = 20;
						return;
					case 2245:
					case 2246:
					case 2247:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 87;
						placeStyle = 5 + type - 2245;
						width = 20;
						height = 20;
						value = 300;
						return;
					}
					switch (type)
					{
					case 2248:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 14;
						placeStyle = 24;
						width = 26;
						height = 20;
						value = 300;
						return;
					case 2249:
					case 2250:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 21;
						placeStyle = 29 + type - 2249;
						width = 26;
						height = 22;
						value = 2500;
						return;
					case 2251:
					case 2252:
					case 2253:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 18;
						placeStyle = 19 + type - 2251;
						width = 28;
						height = 14;
						value = 150;
						return;
					}
					if (type >= 2254 && type <= 2256)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 87;
						placeStyle = 8 + type - 2254;
						width = 20;
						height = 20;
						value = 300;
						return;
					}
					switch (type)
					{
					case 2257:
					case 2258:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 13;
						placeStyle = 7 + type - 2257;
						width = 16;
						height = 24;
						value = 20;
						if (type == 2258)
						{
							value = buyPrice(0, 0, 50);
						}
						return;
					case 2259:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 14;
						placeStyle = 25;
						width = 26;
						height = 20;
						value = 300;
						return;
					case 2260:
					case 2261:
					case 2262:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 311 + type - 2260;
						width = 12;
						height = 12;
						value = buyPrice(0, 0, 0, 50);
						return;
					}
					if (type >= 2263 && type <= 2264)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 142 + type - 2263;
						width = 12;
						height = 12;
						return;
					}
					switch (type)
					{
					case 2265:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 10;
						placeStyle = 28;
						width = 14;
						height = 28;
						value = 200;
						return;
					case 2266:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 10;
						height = 10;
						buffType = 25;
						buffTime = 14400;
						rare = 1;
						value = buyPrice(0, 0, 5);
						return;
					case 2269:
						autoReuse = false;
						useStyle = 5;
						useAnimation = 22;
						useTime = 22;
						width = 24;
						height = 24;
						shoot = 14;
						knockBack = 4f;
						useAmmo = AmmoID.Bullet;
						UseSound = SoundID.Item41;
						damage = 20;
						shootSpeed = 16f;
						noMelee = true;
						value = buyPrice(0, 10);
						scale = 0.85f;
						rare = 2;
						ranged = true;
						crit = 5;
						return;
					case 2270:
						useStyle = 5;
						autoReuse = true;
						useAnimation = 7;
						useTime = 7;
						width = 50;
						height = 18;
						shoot = 10;
						useAmmo = AmmoID.Bullet;
						UseSound = SoundID.Item41;
						damage = 21;
						shootSpeed = 8f;
						noMelee = true;
						value = buyPrice(0, 35);
						knockBack = 1.5f;
						rare = 4;
						ranged = true;
						return;
					case 2271:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 144;
						width = 12;
						height = 12;
						value = buyPrice(0, 0, 2, 50);
						return;
					case 2272:
						useStyle = 5;
						useAnimation = 20;
						useTime = 20;
						width = 38;
						height = 10;
						damage = 0;
						scale = 0.9f;
						shoot = 358;
						shootSpeed = 11f;
						value = buyPrice(0, 1, 50);
						return;
					case 2273:
						autoReuse = true;
						useTurn = true;
						useStyle = 1;
						useAnimation = 20;
						knockBack = 3.5f;
						width = 34;
						height = 34;
						damage = 18;
						crit = 15;
						scale = 1.1f;
						UseSound = SoundID.Item1;
						rare = 1;
						value = buyPrice(0, 10);
						melee = true;
						if (Variant == ItemVariants.StrongerVariant)
						{
							rare = 8;
							value = buyPrice(0, 20);
							damage = 150;
							useAnimation = 15;
							crit = 29;
						}
						return;
					case 2274:
						flame = true;
						noWet = true;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						holdStyle = 1;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 4;
						placeStyle = 12;
						width = 10;
						height = 12;
						value = buyPrice(0, 0, 3);
						return;
					case 2275:
						width = 18;
						height = 18;
						headSlot = 159;
						value = buyPrice(0, 3);
						defense = 2;
						rare = 2;
						return;
					case 2276:
						width = 24;
						height = 24;
						accessory = true;
						vanity = true;
						rare = 8;
						value = buyPrice(2);
						handOnSlot = 16;
						return;
					case 2277:
						width = 18;
						height = 14;
						bodySlot = 165;
						value = buyPrice(0, 2);
						defense = 4;
						rare = 1;
						return;
					case 2278:
						width = 18;
						height = 14;
						bodySlot = 166;
						vanity = true;
						value = buyPrice(0, 1);
						return;
					case 2279:
						width = 18;
						height = 14;
						bodySlot = 167;
						value = buyPrice(0, 3, 50);
						defense = 2;
						rare = 1;
						return;
					case 2280:
						width = 22;
						height = 20;
						accessory = true;
						value = 400000;
						rare = 7;
						wingSlot = 24;
						return;
					case 2281:
					case 2282:
					case 2283:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 242;
						width = 30;
						height = 30;
						value = buyPrice(0, 1);
						placeStyle = 22 + type - 2281;
						return;
					}
					if (type >= 2284 && type <= 2287)
					{
						width = 26;
						height = 30;
						maxStack = 1;
						value = buyPrice(0, 5);
						rare = 5;
						accessory = true;
						backSlot = (sbyte)(3 + type - 2284);
						frontSlot = (sbyte)(1 + type - 2284);
						vanity = true;
						return;
					}
					switch (type)
					{
					case 2288:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 15;
						placeStyle = 28;
						width = 12;
						height = 30;
						value = 150;
						return;
					case 2289:
					case 2291:
					case 2292:
					case 2293:
					case 2294:
					case 2295:
					case 2296:
						useStyle = 1;
						useAnimation = 8;
						useTime = 8;
						width = 24;
						height = 28;
						UseSound = SoundID.Item1;
						shoot = 361 + type - 2291;
						switch (type)
						{
						case 2289:
							fishingPole = 5;
							shootSpeed = 9f;
							shoot = 360;
							value = sellPrice(0, 0, 0, 60);
							break;
						case 2291:
							fishingPole = 15;
							shootSpeed = 11f;
							value = sellPrice(0, 0, 24);
							break;
						case 2293:
							fishingPole = 20;
							shootSpeed = 13f;
							rare = 1;
							value = sellPrice(0, 2, 40);
							break;
						case 2292:
							fishingPole = 30;
							shootSpeed = 14f;
							rare = 2;
							value = sellPrice(0, 1);
							break;
						case 2295:
							fishingPole = 35;
							shootSpeed = 15f;
							rare = 2;
							value = buyPrice(0, 20);
							break;
						case 2296:
							fishingPole = 40;
							shootSpeed = 16f;
							rare = 2;
							value = buyPrice(0, 35);
							break;
						case 2294:
							fishingPole = 50;
							shootSpeed = 17f;
							rare = 3;
							value = sellPrice(0, 20);
							break;
						}
						return;
					}
					if (type >= 2421 && type <= 2422)
					{
						useStyle = 1;
						useAnimation = 8;
						useTime = 8;
						width = 24;
						height = 28;
						UseSound = SoundID.Item1;
						shoot = 381 + type - 2421;
						if (type == 2421)
						{
							fishingPole = 22;
							shootSpeed = 13.5f;
							rare = 1;
							value = sellPrice(0, 3, 12);
						}
						else
						{
							fishingPole = 45;
							shootSpeed = 16.5f;
							rare = 3;
							value = sellPrice(0, 10);
						}
						return;
					}
					if (type == 2320)
					{
						autoReuse = true;
						width = 26;
						height = 26;
						value = sellPrice(0, 1, 50);
						useStyle = 1;
						useAnimation = 24;
						useTime = 14;
						hammer = 70;
						knockBack = 6f;
						damage = 24;
						scale = 1.05f;
						UseSound = SoundID.Item1;
						rare = 3;
						melee = true;
						return;
					}
					switch (type)
					{
					case 2314:
						maxStack = CommonMaxStack;
						width = 26;
						height = 26;
						value = sellPrice(0, 0, 15);
						rare = 1;
						UseSound = SoundID.Item3;
						healLife = 120;
						useStyle = 2;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						consumable = true;
						potion = true;
						return;
					case 2290:
					case 2291:
					case 2292:
					case 2293:
					case 2294:
					case 2295:
					case 2296:
					case 2297:
					case 2298:
					case 2299:
					case 2300:
					case 2301:
					case 2302:
					case 2303:
					case 2304:
					case 2305:
					case 2306:
					case 2307:
					case 2308:
					case 2309:
					case 2310:
					case 2311:
					case 2312:
					case 2313:
					case 2315:
					case 2316:
					case 2317:
					case 2318:
					case 2319:
					case 2320:
					case 2321:
						maxStack = CommonMaxStack;
						width = 26;
						height = 26;
						value = sellPrice(0, 0, 5);
						if (type == 2308)
						{
							value = sellPrice(0, 10);
							rare = 4;
						}
						if (type == 2312)
						{
							value = sellPrice(0, 0, 50);
							rare = 2;
						}
						if (type == 2317)
						{
							value = sellPrice(0, 3);
							rare = 4;
						}
						if (type == 2310)
						{
							value = sellPrice(0, 1);
							rare = 3;
						}
						if (type == 2321)
						{
							value = sellPrice(0, 0, 25);
							rare = 1;
						}
						if (type == 2315)
						{
							value = sellPrice(0, 0, 15);
							rare = 2;
						}
						if (type == 2303)
						{
							value = sellPrice(0, 0, 15);
							rare = 1;
						}
						if (type == 2304)
						{
							value = sellPrice(0, 0, 30);
							rare = 1;
						}
						if (type == 2316)
						{
							value = sellPrice(0, 0, 15);
						}
						if (type == 2311)
						{
							value = sellPrice(0, 0, 15);
							rare = 1;
						}
						if (type == 2313)
						{
							value = sellPrice(0, 0, 15);
							rare = 1;
						}
						if (type == 2306)
						{
							value = sellPrice(0, 0, 15);
							rare = 1;
						}
						if (type == 2307)
						{
							value = sellPrice(0, 0, 25);
							rare = 2;
						}
						if (type == 2319)
						{
							value = sellPrice(0, 0, 15);
							rare = 1;
						}
						if (type == 2318)
						{
							value = sellPrice(0, 0, 15);
							rare = 1;
						}
						if (type == 2298)
						{
							value = sellPrice(0, 0, 7, 50);
						}
						if (type == 2309)
						{
							value = sellPrice(0, 0, 7, 50);
							rare = 1;
						}
						if (type == 2300)
						{
							value = sellPrice(0, 0, 7, 50);
						}
						if (type == 2301)
						{
							value = sellPrice(0, 0, 7, 50);
						}
						if (type == 2302)
						{
							value = sellPrice(0, 0, 15);
						}
						if (type == 2299)
						{
							value = sellPrice(0, 0, 7, 50);
						}
						if (type == 2305)
						{
							value = sellPrice(0, 0, 7, 50);
							rare = 1;
						}
						return;
					}
					switch (type)
					{
					case 2322:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 104;
						buffTime = 36000;
						value = 1000;
						rare = 1;
						return;
					case 2323:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 105;
						buffTime = 28800;
						value = 1000;
						rare = 1;
						return;
					case 2324:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 106;
						buffTime = 43200;
						value = 1000;
						rare = 1;
						return;
					case 2325:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 107;
						buffTime = 162000;
						value = 1000;
						rare = 1;
						return;
					case 2326:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 108;
						buffTime = 28800;
						value = 1000;
						rare = 1;
						return;
					case 2327:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 109;
						buffTime = 28800;
						value = 1000;
						rare = 1;
						return;
					case 2328:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 110;
						buffTime = 28800;
						value = 1000;
						rare = 1;
						return;
					case 2329:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 111;
						buffTime = 36000;
						value = 1000;
						rare = 1;
						return;
					case 2330:
						autoReuse = true;
						useStyle = 1;
						useTime = 35;
						useAnimation = 35;
						width = 24;
						height = 28;
						damage = 35;
						knockBack = 8f;
						scale = 1.15f;
						UseSound = SoundID.Item1;
						rare = 1;
						value = sellPrice(0, 1);
						melee = true;
						return;
					case 2331:
						useStyle = 5;
						useAnimation = 20;
						useTime = 20;
						shootSpeed = 4f;
						knockBack = 6.5f;
						width = 40;
						height = 40;
						damage = 70;
						crit = 20;
						UseSound = SoundID.Item1;
						shoot = 367;
						rare = 7;
						value = sellPrice(0, 1);
						noMelee = true;
						noUseGraphic = true;
						melee = true;
						return;
					case 2332:
						useStyle = 5;
						useAnimation = 20;
						useTime = 20;
						shootSpeed = 4f;
						knockBack = 4.25f;
						width = 40;
						height = 40;
						damage = 19;
						UseSound = SoundID.Item1;
						shoot = 368;
						rare = 2;
						value = sellPrice(0, 0, 50);
						noMelee = true;
						noUseGraphic = true;
						melee = true;
						return;
					case 2333:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 145;
						width = 12;
						height = 12;
						return;
					case 2334:
						width = 12;
						height = 12;
						rare = 1;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 0, 10);
						createTile = 376;
						placeStyle = 0;
						useAnimation = 15;
						useTime = 15;
						autoReuse = true;
						useStyle = 1;
						consumable = true;
						return;
					case 2335:
						width = 12;
						height = 12;
						rare = 2;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 0, 50);
						createTile = 376;
						placeStyle = 1;
						useAnimation = 15;
						useTime = 15;
						autoReuse = true;
						useStyle = 1;
						consumable = true;
						return;
					case 2336:
						width = 12;
						height = 12;
						rare = 3;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 2);
						createTile = 376;
						placeStyle = 2;
						useAnimation = 15;
						useTime = 15;
						autoReuse = true;
						useStyle = 1;
						consumable = true;
						return;
					case 2337:
					case 2338:
					case 2339:
						useStyle = 1;
						useAnimation = 15;
						useTime = 10;
						width = 12;
						height = 12;
						rare = -1;
						maxStack = CommonMaxStack;
						autoReuse = true;
						consumable = true;
						return;
					}
					switch (type)
					{
					case 2340:
						useStyle = 1;
						useAnimation = 15;
						useTime = 7;
						useTurn = true;
						autoReuse = true;
						width = 16;
						height = 16;
						maxStack = CommonMaxStack;
						createTile = 314;
						placeStyle = 0;
						consumable = true;
						cartTrack = true;
						tileBoost = 5;
						return;
					case 2341:
						useStyle = 1;
						useTurn = true;
						useAnimation = 22;
						useTime = 13;
						autoReuse = true;
						width = 24;
						height = 28;
						damage = 16;
						pick = 59;
						scale = 1.15f;
						UseSound = SoundID.Item1;
						knockBack = 3f;
						rare = 3;
						value = sellPrice(0, 1, 50);
						melee = true;
						return;
					case 2342:
						useStyle = 5;
						useAnimation = 25;
						useTime = 8;
						shootSpeed = 48f;
						knockBack = 2.25f;
						width = 20;
						height = 12;
						damage = 13;
						axe = 14;
						UseSound = SoundID.Item23;
						shoot = 369;
						rare = 3;
						value = sellPrice(0, 1, 50);
						noMelee = true;
						noUseGraphic = true;
						melee = true;
						channel = true;
						return;
					case 2343:
						width = 48;
						height = 28;
						mountType = 6;
						rare = 1;
						value = sellPrice(0, 0, 2);
						return;
					case 2344:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 112;
						buffTime = 28800;
						value = 1000;
						rare = 1;
						return;
					case 2345:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 113;
						buffTime = 28800;
						value = 1000;
						rare = 1;
						return;
					case 2346:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 114;
						buffTime = 14400;
						value = 1000;
						rare = 1;
						return;
					case 2347:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 115;
						buffTime = 14400;
						value = 1000;
						rare = 1;
						return;
					case 2348:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 116;
						buffTime = 14400;
						value = 1000;
						rare = 1;
						return;
					case 2349:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 117;
						buffTime = 14400;
						value = 1000;
						rare = 1;
						return;
					case 2350:
						UseSound = SoundID.Item6;
						useStyle = 6;
						useTurn = true;
						useTime = (useAnimation = 30);
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						value = 1000;
						rare = 1;
						return;
					case 2351:
						UseSound = SoundID.Item6;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						value = 1000;
						rare = 1;
						return;
					case 2352:
						useStyle = 1;
						shootSpeed = 9f;
						shoot = 370;
						width = 18;
						height = 20;
						maxStack = CommonMaxStack;
						consumable = true;
						UseSound = SoundID.Item1;
						useAnimation = 15;
						useTime = 15;
						noUseGraphic = true;
						noMelee = true;
						value = 200;
						return;
					case 2353:
						useStyle = 1;
						shootSpeed = 9f;
						shoot = 371;
						width = 18;
						height = 20;
						maxStack = CommonMaxStack;
						consumable = true;
						UseSound = SoundID.Item1;
						useAnimation = 15;
						useTime = 15;
						noUseGraphic = true;
						noMelee = true;
						value = 200;
						return;
					case 2354:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 121;
						buffTime = 28800;
						rare = 1;
						value = 1000;
						return;
					case 2355:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 122;
						buffTime = 28800;
						value = 1000;
						rare = 1;
						return;
					case 2356:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 123;
						buffTime = 14400;
						value = 1000;
						rare = 1;
						return;
					case 2357:
						autoReuse = true;
						useTurn = true;
						useStyle = 1;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 82;
						placeStyle = 6;
						width = 12;
						height = 14;
						value = 80;
						return;
					case 2358:
						maxStack = CommonMaxStack;
						width = 12;
						height = 14;
						value = 100;
						return;
					case 2359:
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						buffType = 124;
						buffTime = 54000;
						value = 1000;
						rare = 1;
						return;
					case 2360:
						noUseGraphic = true;
						damage = 0;
						useStyle = 5;
						shootSpeed = 13f;
						shoot = 372;
						width = 18;
						height = 28;
						UseSound = SoundID.Item1;
						useAnimation = 20;
						useTime = 20;
						rare = 3;
						noMelee = true;
						value = 20000;
						return;
					case 2361:
						width = 18;
						height = 18;
						defense = 4;
						headSlot = 160;
						rare = 3;
						value = 45000;
						return;
					case 2362:
						width = 18;
						height = 18;
						defense = 5;
						bodySlot = 168;
						rare = 3;
						value = 30000;
						return;
					case 2363:
						width = 18;
						height = 18;
						defense = 4;
						legSlot = 103;
						rare = 3;
						value = 30000;
						return;
					case 2364:
						mana = 10;
						damage = 12;
						useStyle = 1;
						shootSpeed = 10f;
						shoot = 373;
						width = 26;
						height = 28;
						UseSound = SoundID.Item76;
						useAnimation = 22;
						useTime = 22;
						rare = 3;
						noMelee = true;
						knockBack = 2f;
						buffType = 125;
						value = 10000;
						summon = true;
						autoReuse = true;
						reuseDelay = 2;
						return;
					case 2365:
						mana = 10;
						damage = 17;
						useStyle = 1;
						shootSpeed = 10f;
						shoot = 375;
						width = 26;
						height = 28;
						UseSound = SoundID.Item77;
						useAnimation = 36;
						useTime = 36;
						rare = 3;
						noMelee = true;
						knockBack = 2f;
						buffType = 126;
						value = 10000;
						summon = true;
						autoReuse = true;
						reuseDelay = 2;
						return;
					case 2366:
						mana = 10;
						damage = 26;
						useStyle = 1;
						shootSpeed = 14f;
						shoot = 377;
						width = 18;
						height = 20;
						UseSound = SoundID.Item78;
						useAnimation = 30;
						useTime = 30;
						noMelee = true;
						value = sellPrice(0, 5);
						knockBack = 7.5f;
						rare = 4;
						summon = true;
						sentry = true;
						return;
					case 2367:
						width = 18;
						height = 18;
						defense = 1;
						headSlot = 161;
						rare = 1;
						value = sellPrice(0, 1);
						return;
					case 2368:
						width = 18;
						height = 18;
						bodySlot = 169;
						defense = 2;
						rare = 1;
						value = sellPrice(0, 1);
						return;
					case 2369:
						width = 18;
						height = 18;
						legSlot = 104;
						defense = 1;
						rare = 1;
						value = sellPrice(0, 1);
						return;
					case 2370:
						width = 18;
						height = 18;
						headSlot = 162;
						rare = 4;
						value = sellPrice(0, 0, 75);
						defense = 5;
						return;
					case 2371:
						width = 18;
						height = 18;
						bodySlot = 170;
						rare = 4;
						value = sellPrice(0, 0, 75);
						defense = 8;
						return;
					case 2372:
						width = 18;
						height = 18;
						legSlot = 105;
						rare = 4;
						value = sellPrice(0, 0, 75);
						defense = 7;
						return;
					case 2373:
					case 2374:
					case 2375:
						width = 26;
						height = 30;
						maxStack = 1;
						value = sellPrice(0, 1);
						rare = 1;
						accessory = true;
						return;
					}
					if (type >= 2376 && type <= 2385)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 87;
						placeStyle = 11 + type - 2376;
						width = 20;
						height = 20;
						value = 300;
						if (type == 2379)
						{
							value = buyPrice(0, 10);
						}
						return;
					}
					if (type >= 2386 && type <= 2396)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 88;
						placeStyle = 5 + type - 2386;
						width = 20;
						height = 20;
						value = 300;
						if (type == 2389)
						{
							value = buyPrice(0, 10);
						}
						return;
					}
					if (type >= 2397 && type <= 2416)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 89;
						placeStyle = 1 + type - 2397;
						width = 20;
						height = 20;
						value = 300;
						return;
					}
					switch (type)
					{
					case 2417:
						width = 18;
						height = 18;
						headSlot = 163;
						vanity = true;
						value = sellPrice(0, 1);
						return;
					case 2418:
						width = 18;
						height = 18;
						bodySlot = 171;
						vanity = true;
						value = sellPrice(0, 1);
						return;
					case 2419:
						width = 18;
						height = 18;
						legSlot = 106;
						vanity = true;
						value = sellPrice(0, 1);
						return;
					case 2420:
						damage = 0;
						useStyle = 1;
						shoot = 380;
						width = 16;
						height = 30;
						UseSound = SoundID.Item2;
						useAnimation = 20;
						useTime = 20;
						rare = 3;
						noMelee = true;
						value = sellPrice(0, 3);
						buffType = 127;
						return;
					case 2423:
						width = 16;
						height = 24;
						accessory = true;
						rare = 1;
						value = 50000;
						shoeSlot = 15;
						return;
					case 2424:
						noMelee = true;
						useStyle = 1;
						shootSpeed = 20f;
						shoot = 383;
						damage = 70;
						knockBack = 8f;
						width = 34;
						height = 34;
						UseSound = SoundID.Item1;
						useAnimation = 20;
						useTime = 20;
						noUseGraphic = true;
						rare = 3;
						value = 50000;
						melee = true;
						return;
					case 2428:
						useStyle = 1;
						width = 16;
						height = 30;
						UseSound = SoundID.Item79;
						useAnimation = 20;
						useTime = 20;
						rare = 8;
						noMelee = true;
						mountType = 1;
						value = sellPrice(0, 5);
						return;
					case 2429:
						useStyle = 1;
						width = 16;
						height = 30;
						UseSound = SoundID.Item80;
						useAnimation = 20;
						useTime = 20;
						rare = 8;
						noMelee = true;
						mountType = 2;
						value = sellPrice(0, 5);
						return;
					case 2430:
						useStyle = 1;
						width = 16;
						height = 30;
						UseSound = SoundID.Item81;
						useAnimation = 20;
						useTime = 20;
						rare = 8;
						noMelee = true;
						mountType = 3;
						value = sellPrice(0, 5);
						return;
					case 2431:
						width = 18;
						height = 16;
						maxStack = CommonMaxStack;
						value = 100;
						return;
					case 2432:
					case 2433:
					case 2434:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 146 + type - 2432;
						width = 12;
						height = 12;
						if (type == 2434)
						{
							value = buyPrice(0, 0, 0, 50);
						}
						return;
					}
					switch (type)
					{
					case 2435:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 315;
						width = 12;
						height = 12;
						value = buyPrice(0, 0, 0, 50);
						return;
					case 2436:
					case 2437:
					case 2438:
						useStyle = 1;
						autoReuse = true;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 12;
						height = 12;
						noUseGraphic = true;
						bait = 20;
						value = sellPrice(0, 3, 50);
						return;
					}
					if (type >= 2439 && type <= 2441)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 316 + type - 2439;
						width = 12;
						height = 12;
						return;
					}
					if (type >= 2442 && type <= 2449)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 240;
						width = 30;
						height = 30;
						value = sellPrice(0, 0, 50);
						placeStyle = 46 + type - 2442;
						return;
					}
					if (type >= 2450 && type <= 2488)
					{
						DefaultToQuestFish();
						return;
					}
					switch (type)
					{
					case 2489:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 240;
						width = 30;
						height = 30;
						value = sellPrice(0, 1);
						placeStyle = 54;
						rare = 1;
						return;
					case 2490:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 319;
						width = 12;
						height = 12;
						value = sellPrice(0, 3);
						return;
					case 2491:
						useStyle = 1;
						width = 16;
						height = 30;
						UseSound = SoundID.Item25;
						useAnimation = 20;
						useTime = 20;
						rare = 8;
						noMelee = true;
						mountType = 4;
						value = sellPrice(0, 5);
						return;
					case 2492:
						useStyle = 1;
						useAnimation = 15;
						useTime = 7;
						useTurn = true;
						autoReuse = true;
						width = 16;
						height = 16;
						maxStack = CommonMaxStack;
						createTile = 314;
						placeStyle = 1;
						consumable = true;
						cartTrack = true;
						mech = true;
						tileBoost = 2;
						value = sellPrice(0, 0, 10);
						return;
					case 2493:
						width = 28;
						height = 20;
						headSlot = 164;
						rare = 1;
						value = sellPrice(0, 0, 75);
						vanity = true;
						return;
					case 2494:
						width = 22;
						height = 20;
						accessory = true;
						value = buyPrice(0, 40);
						rare = 4;
						wingSlot = 25;
						return;
					case 2495:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 242;
						width = 30;
						height = 30;
						value = sellPrice(0, 1);
						placeStyle = 25;
						return;
					case 2496:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 320;
						placeStyle = 0;
						width = 22;
						height = 30;
						value = sellPrice(0, 1);
						return;
					case 2497:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 242;
						width = 30;
						height = 30;
						value = sellPrice(0, 0, 50);
						placeStyle = 26;
						return;
					case 2498:
						width = 18;
						height = 18;
						headSlot = 165;
						vanity = true;
						value = sellPrice(0, 1);
						return;
					case 2499:
						width = 18;
						height = 18;
						bodySlot = 172;
						vanity = true;
						value = sellPrice(0, 1);
						return;
					case 2500:
						width = 18;
						height = 18;
						legSlot = 107;
						vanity = true;
						value = sellPrice(0, 1);
						return;
					case 2501:
						width = 18;
						height = 12;
						maxStack = 1;
						value = sellPrice(0, 1);
						rare = 5;
						beardSlot = 1;
						accessory = true;
						vanity = true;
						return;
					case 2502:
						useStyle = 1;
						width = 16;
						height = 30;
						UseSound = SoundID.Item25;
						useAnimation = 20;
						useTime = 20;
						rare = 8;
						noMelee = true;
						mountType = 5;
						value = sellPrice(0, 5);
						return;
					case 2503:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 321;
						width = 8;
						height = 10;
						return;
					case 2504:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 322;
						width = 8;
						height = 10;
						return;
					case 2505:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 149;
						width = 12;
						height = 12;
						return;
					case 2506:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 151;
						width = 12;
						height = 12;
						return;
					case 2507:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 150;
						width = 12;
						height = 12;
						return;
					case 2508:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 152;
						width = 12;
						height = 12;
						return;
					case 2509:
						width = 18;
						height = 18;
						defense = 1;
						headSlot = 166;
						return;
					case 2510:
						width = 18;
						height = 18;
						defense = 1;
						bodySlot = 173;
						return;
					case 2511:
						width = 18;
						height = 18;
						defense = 1;
						legSlot = 108;
						return;
					case 2512:
						width = 18;
						height = 18;
						defense = 1;
						headSlot = 167;
						return;
					case 2513:
						width = 18;
						height = 18;
						defense = 1;
						bodySlot = 174;
						return;
					case 2514:
						width = 18;
						height = 18;
						defense = 1;
						legSlot = 109;
						return;
					case 2517:
						useStyle = 1;
						useTurn = false;
						useAnimation = 19;
						useTime = 19;
						width = 24;
						height = 28;
						damage = 8;
						knockBack = 6f;
						UseSound = SoundID.Item1;
						scale = 1f;
						value = 100;
						melee = true;
						return;
					case 2516:
						autoReuse = true;
						useStyle = 1;
						useTurn = true;
						useAnimation = 33;
						useTime = 23;
						hammer = 35;
						width = 24;
						height = 28;
						damage = 4;
						knockBack = 5.5f;
						scale = 1.1f;
						UseSound = SoundID.Item1;
						value = 50;
						melee = true;
						autoReuse = true;
						return;
					case 2515:
						useStyle = 5;
						useAnimation = 29;
						useTime = 29;
						width = 12;
						height = 28;
						shoot = 1;
						useAmmo = AmmoID.Arrow;
						UseSound = SoundID.Item5;
						damage = 6;
						shootSpeed = 6.6f;
						noMelee = true;
						value = 100;
						ranged = true;
						return;
					case 2518:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 19;
						placeStyle = 17;
						width = 8;
						height = 10;
						return;
					case 2519:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 90;
						placeStyle = 17;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2520:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						autoReuse = true;
						createTile = 79;
						placeStyle = 22;
						width = 28;
						height = 20;
						value = 2000;
						return;
					case 2521:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 89;
						placeStyle = 21;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2527:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 89;
						placeStyle = 22;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2522:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 100;
						placeStyle = 18;
						width = 20;
						height = 20;
						value = 1500;
						return;
					case 2523:
						noWet = true;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 33;
						placeStyle = 18;
						width = 8;
						value = sellPrice(0, 0, 0, 60);
						height = 18;
						return;
					case 2524:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 15;
						placeStyle = 29;
						width = 12;
						height = 30;
						value = 150;
						return;
					case 2525:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 34;
						placeStyle = 23;
						width = 26;
						height = 26;
						value = 3000;
						return;
					case 2526:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 21;
						placeStyle = 31;
						width = 26;
						height = 22;
						value = 500;
						return;
					case 2528:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 10;
						placeStyle = 29;
						width = 14;
						height = 28;
						value = 200;
						return;
					case 2529:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 88;
						placeStyle = 16;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2530:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 42;
						width = 12;
						height = 28;
						placeStyle = 27;
						value = 150;
						return;
					case 2531:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 87;
						placeStyle = 21;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2535:
						mana = 10;
						damage = 21;
						useStyle = 1;
						shootSpeed = 10f;
						shoot = 387;
						width = 26;
						height = 28;
						UseSound = SoundID.Item82;
						useAnimation = 36;
						useTime = 36;
						rare = 5;
						noMelee = true;
						knockBack = 2f;
						buffType = 134;
						value = buyPrice(0, 10);
						summon = true;
						autoReuse = true;
						reuseDelay = 2;
						return;
					case 2532:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 14;
						placeStyle = 26;
						width = 26;
						height = 20;
						value = 300;
						return;
					case 2533:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 93;
						placeStyle = 18;
						width = 10;
						height = 24;
						value = 500;
						return;
					case 2534:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 18;
						placeStyle = 22;
						width = 28;
						height = 14;
						value = 150;
						return;
					case 2536:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 101;
						width = 20;
						height = 20;
						value = 300;
						placeStyle = 23;
						return;
					case 2549:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 19;
						placeStyle = 18;
						width = 8;
						height = 10;
						return;
					case 2537:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 90;
						placeStyle = 18;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2538:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						autoReuse = true;
						createTile = 79;
						placeStyle = 23;
						width = 28;
						height = 20;
						value = 2000;
						return;
					case 2539:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 89;
						placeStyle = 23;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2540:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 101;
						width = 20;
						height = 20;
						value = 300;
						placeStyle = 24;
						return;
					case 2541:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 100;
						placeStyle = 19;
						width = 20;
						height = 20;
						value = 1500;
						return;
					case 2542:
						noWet = true;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 33;
						placeStyle = 19;
						width = 8;
						value = sellPrice(0, 0, 0, 60);
						height = 18;
						return;
					case 2543:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 34;
						placeStyle = 24;
						width = 26;
						height = 26;
						value = 3000;
						return;
					case 2544:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 21;
						placeStyle = 32;
						width = 26;
						height = 22;
						value = 500;
						return;
					case 2545:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 88;
						placeStyle = 17;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2547:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 93;
						placeStyle = 19;
						width = 10;
						height = 24;
						value = 500;
						return;
					case 2546:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 42;
						width = 12;
						height = 28;
						placeStyle = 28;
						value = 150;
						return;
					case 2548:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 87;
						placeStyle = 22;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2413:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 89;
						placeStyle = 23;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2550:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 14;
						placeStyle = 27;
						width = 26;
						height = 20;
						value = 300;
						return;
					case 2551:
						mana = 10;
						damage = 26;
						useStyle = 1;
						shootSpeed = 10f;
						shoot = 390;
						width = 26;
						height = 28;
						UseSound = SoundID.Item83;
						useAnimation = 36;
						useTime = 36;
						rare = 4;
						noMelee = true;
						knockBack = 3f;
						buffType = 133;
						value = buyPrice(0, 5);
						summon = true;
						autoReuse = true;
						reuseDelay = 2;
						return;
					case 2552:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 90;
						placeStyle = 19;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2553:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						autoReuse = true;
						createTile = 79;
						placeStyle = 24;
						width = 28;
						height = 20;
						value = 2000;
						return;
					case 2554:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 101;
						width = 20;
						height = 20;
						value = 300;
						placeStyle = 25;
						return;
					case 2555:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 100;
						placeStyle = 20;
						width = 20;
						height = 20;
						value = 1500;
						return;
					case 2556:
						noWet = true;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 33;
						placeStyle = 20;
						width = 8;
						value = sellPrice(0, 0, 0, 60);
						height = 18;
						return;
					case 2557:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 15;
						placeStyle = 30;
						width = 12;
						height = 30;
						value = 150;
						return;
					case 2558:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 34;
						placeStyle = 25;
						width = 26;
						height = 26;
						value = 3000;
						return;
					case 2559:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 21;
						placeStyle = 33;
						width = 26;
						height = 22;
						value = 500;
						return;
					case 2560:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 104;
						placeStyle = 6;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2561:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 10;
						placeStyle = 30;
						width = 14;
						height = 28;
						value = 200;
						return;
					case 2562:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 88;
						placeStyle = 18;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2563:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 93;
						placeStyle = 20;
						width = 10;
						height = 24;
						value = 500;
						return;
					case 2564:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 42;
						placeStyle = 29;
						width = 12;
						height = 28;
						value = 150;
						return;
					case 2565:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 87;
						placeStyle = 23;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2566:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 19;
						placeStyle = 19;
						width = 8;
						height = 10;
						return;
					case 2567:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 90;
						placeStyle = 20;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2568:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						autoReuse = true;
						createTile = 79;
						placeStyle = 25;
						width = 28;
						height = 20;
						value = 2000;
						return;
					case 2569:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 101;
						placeStyle = 26;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2570:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 100;
						placeStyle = 21;
						width = 20;
						height = 20;
						value = 1500;
						return;
					case 2571:
						noWet = true;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 33;
						placeStyle = 21;
						width = 8;
						value = sellPrice(0, 0, 0, 60);
						height = 18;
						return;
					case 2572:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 15;
						placeStyle = 31;
						width = 12;
						height = 30;
						value = 150;
						return;
					case 2573:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 34;
						placeStyle = 26;
						width = 26;
						height = 26;
						value = 3000;
						return;
					case 2574:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 21;
						placeStyle = 34;
						width = 26;
						height = 22;
						value = 500;
						return;
					case 2575:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 104;
						placeStyle = 7;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2576:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 10;
						placeStyle = 31;
						width = 14;
						height = 28;
						value = 200;
						return;
					case 2577:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 88;
						placeStyle = 19;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2578:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 93;
						placeStyle = 21;
						width = 10;
						height = 24;
						value = 500;
						return;
					case 2579:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 42;
						placeStyle = 30;
						width = 12;
						height = 28;
						value = 150;
						return;
					case 2580:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 87;
						placeStyle = 24;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2581:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 19;
						placeStyle = 20;
						width = 8;
						height = 10;
						return;
					case 2582:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 89;
						placeStyle = 25;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2583:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 14;
						placeStyle = 29;
						width = 26;
						height = 20;
						value = 300;
						return;
					case 2584:
						mana = 10;
						damage = 40;
						useStyle = 1;
						shootSpeed = 10f;
						shoot = 393;
						width = 26;
						height = 28;
						UseSound = SoundID.Item44;
						useAnimation = 36;
						useTime = 36;
						rare = 5;
						noMelee = true;
						knockBack = 6f;
						buffType = 135;
						value = buyPrice(0, 5);
						summon = true;
						autoReuse = true;
						reuseDelay = 2;
						return;
					case 2585:
						noUseGraphic = true;
						damage = 0;
						useStyle = 5;
						shootSpeed = 13f;
						shoot = 396;
						width = 18;
						height = 28;
						UseSound = SoundID.Item1;
						useAnimation = 20;
						useTime = 20;
						rare = 3;
						noMelee = true;
						value = 20000;
						return;
					case 2586:
						useStyle = 5;
						shootSpeed = 5.5f;
						shoot = 397;
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						consumable = true;
						UseSound = SoundID.Item1;
						useAnimation = 45;
						useTime = 45;
						noUseGraphic = true;
						noMelee = true;
						value = 75;
						damage = 60;
						knockBack = 8f;
						ranged = true;
						return;
					case 2587:
						damage = 0;
						useStyle = 1;
						shoot = 398;
						width = 16;
						height = 30;
						UseSound = SoundID.Item2;
						useAnimation = 20;
						useTime = 20;
						rare = 3;
						noMelee = true;
						buffType = 136;
						value = sellPrice(0, 2);
						return;
					case 2588:
						width = 28;
						height = 20;
						headSlot = 168;
						rare = 1;
						value = sellPrice(0, 0, 75);
						vanity = true;
						return;
					case 2589:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 240;
						width = 30;
						height = 30;
						value = sellPrice(0, 1);
						placeStyle = 55;
						rare = 1;
						return;
					case 2590:
						useStyle = 5;
						shootSpeed = 6.5f;
						shoot = 399;
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						consumable = true;
						UseSound = SoundID.Item1;
						useAnimation = 40;
						useTime = 40;
						noUseGraphic = true;
						noMelee = true;
						value = sellPrice(0, 0, 1);
						damage = 23;
						knockBack = 7f;
						ranged = true;
						rare = 1;
						return;
					case 2591:
					case 2592:
					case 2593:
					case 2594:
					case 2595:
					case 2596:
					case 2597:
					case 2598:
					case 2599:
					case 2600:
					case 2601:
					case 2602:
					case 2603:
					case 2604:
					case 2605:
					case 2606:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 104;
						placeStyle = 8 + type - 2591;
						width = 20;
						height = 20;
						value = 300;
						return;
					}
					switch (type)
					{
					case 2607:
						maxStack = CommonMaxStack;
						width = 12;
						height = 12;
						rare = 4;
						value = sellPrice(0, 0, 5);
						return;
					case 2608:
						autoReuse = true;
						scale = 1.05f;
						useStyle = 1;
						useTime = 20;
						useAnimation = 20;
						knockBack = 6f;
						width = 24;
						height = 28;
						damage = 25;
						scale = 1.175f;
						UseSound = SoundID.Item1;
						rare = 4;
						value = 10000;
						melee = true;
						return;
					case 2609:
						width = 22;
						height = 20;
						accessory = true;
						value = buyPrice(0, 40);
						rare = 8;
						wingSlot = 26;
						return;
					case 2610:
						useStyle = 5;
						useAnimation = 12;
						useTime = 12;
						width = 38;
						height = 10;
						damage = 0;
						scale = 0.9f;
						shoot = 406;
						shootSpeed = 8f;
						autoReuse = true;
						value = buyPrice(0, 1, 50);
						return;
					case 2611:
						autoReuse = false;
						useStyle = 5;
						useAnimation = 20;
						useTime = 20;
						autoReuse = true;
						knockBack = 4.5f;
						width = 30;
						height = 10;
						damage = 66;
						shoot = 404;
						shootSpeed = 14f;
						UseSound = SoundID.Item1;
						rare = 8;
						value = sellPrice(0, 5);
						melee = true;
						noMelee = true;
						noUseGraphic = true;
						return;
					case 2612:
					case 2613:
					case 2614:
					case 2615:
					case 2616:
					case 2617:
					case 2618:
					case 2619:
					case 2620:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 21;
						if (type <= 2614)
						{
							placeStyle = 35 + (type - 2612) * 2;
						}
						else
						{
							placeStyle = 41 + type - 2615;
						}
						width = 26;
						height = 22;
						value = 500;
						return;
					}
					switch (type)
					{
					case 2621:
						mana = 10;
						damage = 50;
						useStyle = 1;
						shootSpeed = 10f;
						shoot = 407;
						width = 26;
						height = 28;
						UseSound = SoundID.Item44;
						useAnimation = 36;
						useTime = 36;
						rare = 8;
						noMelee = true;
						knockBack = 2f;
						buffType = 139;
						value = sellPrice(0, 5);
						summon = true;
						autoReuse = true;
						reuseDelay = 2;
						return;
					case 2624:
						useStyle = 5;
						autoReuse = true;
						useAnimation = 24;
						useTime = 24;
						width = 50;
						height = 18;
						shoot = 1;
						useAmmo = AmmoID.Arrow;
						UseSound = SoundID.Item5;
						damage = 53;
						shootSpeed = 10f;
						noMelee = true;
						value = sellPrice(0, 5);
						ranged = true;
						rare = 8;
						knockBack = 2f;
						return;
					case 2622:
						mana = 20;
						damage = 85;
						useStyle = 5;
						shootSpeed = 6f;
						shoot = 409;
						width = 26;
						height = 28;
						UseSound = SoundID.Item84;
						useAnimation = 40;
						useTime = 40;
						autoReuse = true;
						rare = 8;
						noMelee = true;
						knockBack = 5f;
						scale = 0.9f;
						value = sellPrice(0, 5);
						magic = true;
						return;
					case 2625:
					case 2626:
						DefaultToSeaShelll();
						return;
					case 2627:
					case 2628:
					case 2629:
					case 2630:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 19;
						placeStyle = 21 + type - 2627;
						width = 8;
						height = 10;
						return;
					}
					if (type >= 2631 && type <= 2633)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 18;
						placeStyle = 24 + type - 2631;
						width = 28;
						height = 14;
						value = 150;
						return;
					}
					if (type >= 2634 && type <= 2636)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 89;
						placeStyle = 26 + type - 2634;
						width = 20;
						height = 20;
						value = 300;
						return;
					}
					switch (type)
					{
					case 2623:
						autoReuse = true;
						mana = 5;
						UseSound = SoundID.Item85;
						useStyle = 5;
						damage = 70;
						useAnimation = 9;
						useTime = 9;
						width = 40;
						height = 40;
						shoot = 410;
						shootSpeed = 15f;
						knockBack = 3f;
						value = sellPrice(0, 5);
						magic = true;
						rare = 8;
						noMelee = true;
						if (Variant == ItemVariants.WeakerVariant)
						{
							rare = 2;
							value = dungeonPrice;
							damage = 5;
						}
						return;
					case 2637:
					case 2638:
					case 2639:
					case 2640:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 88;
						placeStyle = 20 + type - 2637;
						width = 20;
						height = 20;
						value = 300;
						return;
					}
					switch (type)
					{
					case 2641:
					case 2642:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 42;
						if (type == 2641)
						{
							placeStyle = 31;
						}
						else
						{
							placeStyle = 32;
						}
						width = 12;
						height = 28;
						value = 150;
						return;
					case 2643:
					case 2644:
					case 2645:
					case 2646:
					case 2647:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 93;
						placeStyle = 22 + type - 2643;
						width = 10;
						height = 24;
						value = 500;
						return;
					}
					if (type >= 2648 && type <= 2651)
					{
						noWet = true;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 33;
						width = 8;
						height = 18;
						value = sellPrice(0, 0, 0, 60);
						placeStyle = 22 + type - 2648;
						return;
					}
					if (type >= 2652 && type <= 2657)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 34;
						placeStyle = 27 + type - 2652;
						width = 26;
						height = 26;
						value = 3000;
						return;
					}
					if (type >= 2658 && type <= 2663)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 90;
						placeStyle = 21 + type - 2658;
						width = 20;
						height = 20;
						value = 300;
						return;
					}
					if (type >= 2664 && type <= 2668)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 100;
						placeStyle = 22 + type - 2664;
						width = 20;
						height = 20;
						value = 1500;
						return;
					}
					switch (type)
					{
					case 2669:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						autoReuse = true;
						createTile = 79;
						placeStyle = 26;
						width = 28;
						height = 20;
						value = 2000;
						return;
					case 2670:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 101;
						width = 20;
						height = 20;
						value = 300;
						placeStyle = 27;
						return;
					case 2671:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 87;
						placeStyle = 25;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2672:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 105;
						width = 20;
						height = 20;
						value = 300;
						placeStyle = 50;
						return;
					case 2673:
						useStyle = 1;
						autoReuse = true;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 12;
						height = 12;
						makeNPC = 374;
						noUseGraphic = true;
						bait = 666;
						return;
					case 2674:
					case 2675:
					case 2676:
						maxStack = CommonMaxStack;
						consumable = true;
						width = 12;
						height = 12;
						switch (type)
						{
						case 2675:
							bait = 30;
							value = sellPrice(0, 0, 3);
							break;
						case 2676:
							bait = 50;
							value = sellPrice(0, 0, 10);
							break;
						default:
							bait = 15;
							value = sellPrice(0, 0, 1);
							break;
						}
						return;
					}
					if (type >= 2677 && type <= 2690)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						switch (type)
						{
						case 2677:
							createWall = 153;
							break;
						case 2678:
							createWall = 157;
							break;
						case 2679:
							createWall = 154;
							break;
						case 2680:
							createWall = 158;
							break;
						case 2681:
							createWall = 155;
							break;
						case 2682:
							createWall = 159;
							break;
						case 2683:
							createWall = 156;
							break;
						case 2684:
							createWall = 160;
							break;
						case 2685:
							createWall = 164;
							break;
						case 2686:
							createWall = 161;
							break;
						case 2687:
							createWall = 165;
							break;
						case 2688:
							createWall = 162;
							break;
						case 2689:
							createWall = 166;
							break;
						case 2690:
							createWall = 163;
							break;
						}
						width = 12;
						height = 12;
						return;
					}
					switch (type)
					{
					case 2691:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 167;
						width = 12;
						height = 12;
						return;
					case 2692:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 325;
						width = 12;
						height = 12;
						return;
					case 2693:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 326;
						width = 12;
						height = 12;
						return;
					case 2694:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 327;
						width = 12;
						height = 12;
						return;
					case 2695:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 328;
						width = 12;
						height = 12;
						return;
					case 2696:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 168;
						width = 12;
						height = 12;
						return;
					case 2697:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 329;
						width = 12;
						height = 12;
						return;
					case 2698:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 169;
						width = 12;
						height = 12;
						return;
					case 2699:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 471;
						width = 30;
						height = 30;
						value = sellPrice(0, 0, 0, 50);
						return;
					case 2700:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 335;
						width = 26;
						height = 22;
						value = buyPrice(0, 5);
						mech = true;
						return;
					case 2701:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 336;
						width = 12;
						height = 12;
						return;
					case 2702:
					case 2703:
					case 2704:
					case 2705:
					case 2706:
					case 2707:
					case 2708:
					case 2709:
					case 2710:
					case 2711:
					case 2712:
					case 2713:
					case 2714:
					case 2715:
					case 2716:
					case 2717:
					case 2718:
					case 2719:
					case 2720:
					case 2721:
					case 2722:
					case 2723:
					case 2724:
					case 2725:
					case 2726:
					case 2727:
					case 2728:
					case 2729:
					case 2730:
					case 2731:
					case 2732:
					case 2733:
					case 2734:
					case 2735:
					case 2736:
					case 2737:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 337;
						width = 20;
						height = 20;
						value = 300;
						placeStyle = type - 2702;
						return;
					}
					switch (type)
					{
					case 2738:
						createTile = 338;
						placeStyle = 0;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 12;
						height = 30;
						value = buyPrice(0, 3);
						mech = true;
						return;
					case 2739:
						useStyle = 1;
						useAnimation = 15;
						useTime = 7;
						useTurn = true;
						autoReuse = true;
						width = 16;
						height = 16;
						maxStack = CommonMaxStack;
						createTile = 314;
						placeStyle = 2;
						consumable = true;
						cartTrack = true;
						mech = true;
						tileBoost = 2;
						value = buyPrice(0, 0, 50);
						return;
					case 2740:
						useStyle = 1;
						autoReuse = true;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 12;
						height = 12;
						makeNPC = 377;
						noUseGraphic = true;
						bait = 10;
						return;
					case 2741:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 339;
						width = 12;
						height = 12;
						return;
					case 2742:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						consumable = true;
						createTile = 139;
						placeStyle = 31;
						width = 24;
						height = 24;
						rare = 4;
						value = 100000;
						accessory = true;
						hasVanityEffects = true;
						return;
					case 2743:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 14;
						placeStyle = 30;
						width = 26;
						height = 20;
						value = 300;
						return;
					case 2744:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 19;
						placeStyle = 25;
						width = 8;
						height = 10;
						return;
					case 2745:
						useStyle = 1;
						useTurn = false;
						useAnimation = 20;
						useTime = 20;
						width = 24;
						height = 28;
						damage = 8;
						knockBack = 6f;
						UseSound = SoundID.Item1;
						scale = 1f;
						value = 100;
						melee = true;
						return;
					case 2746:
						autoReuse = true;
						useStyle = 1;
						useTurn = true;
						useAnimation = 33;
						useTime = 23;
						hammer = 35;
						width = 24;
						height = 28;
						damage = 4;
						knockBack = 5.5f;
						scale = 1.1f;
						UseSound = SoundID.Item1;
						value = 50;
						melee = true;
						return;
					case 2747:
						useStyle = 5;
						useAnimation = 29;
						useTime = 29;
						width = 12;
						height = 28;
						shoot = 1;
						useAmmo = AmmoID.Arrow;
						UseSound = SoundID.Item5;
						damage = 6;
						shootSpeed = 6.6f;
						noMelee = true;
						value = 100;
						ranged = true;
						return;
					case 2748:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 21;
						placeStyle = 47;
						width = 26;
						height = 22;
						value = 500;
						return;
					case 2749:
						mana = 10;
						damage = 36;
						useStyle = 1;
						shootSpeed = 10f;
						shoot = 423;
						width = 26;
						height = 28;
						UseSound = SoundID.Item44;
						useAnimation = 36;
						useTime = 36;
						rare = 8;
						noMelee = true;
						knockBack = 2f;
						buffType = 140;
						value = sellPrice(0, 10);
						summon = true;
						autoReuse = true;
						reuseDelay = 2;
						return;
					case 2750:
						autoReuse = true;
						mana = 9;
						useStyle = 5;
						damage = 50;
						useAnimation = 10;
						useTime = 10;
						width = 40;
						height = 40;
						shoot = 424;
						shootSpeed = 10f;
						knockBack = 4.5f;
						value = sellPrice(0, 2);
						magic = true;
						rare = 5;
						noMelee = true;
						UseSound = SoundID.Item88;
						return;
					case 2751:
					case 2752:
					case 2753:
					case 2754:
					case 2755:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 340 + type - 2751;
						width = 12;
						height = 12;
						return;
					}
					switch (type)
					{
					case 2756:
						UseSound = SoundID.Item6;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						value = 1000;
						rare = 1;
						return;
					case 2757:
						width = 18;
						height = 18;
						defense = 14;
						headSlot = 169;
						glowMask = 26;
						rare = 10;
						value = sellPrice(0, 7);
						return;
					case 2758:
						width = 18;
						height = 18;
						defense = 28;
						bodySlot = 175;
						glowMask = 27;
						rare = 10;
						value = sellPrice(0, 7) * 2;
						return;
					case 2759:
						width = 18;
						height = 18;
						defense = 20;
						legSlot = 110;
						rare = 10;
						value = (int)((double)sellPrice(0, 7) * 1.5);
						return;
					case 2760:
						width = 18;
						height = 18;
						defense = 14;
						headSlot = 170;
						glowMask = 28;
						rare = 10;
						value = sellPrice(0, 7);
						return;
					case 2761:
						width = 18;
						height = 18;
						defense = 18;
						bodySlot = 176;
						glowMask = 29;
						rare = 10;
						value = sellPrice(0, 7) * 2;
						return;
					case 2762:
						width = 18;
						height = 18;
						defense = 14;
						legSlot = 111;
						glowMask = 30;
						rare = 10;
						value = (int)((double)sellPrice(0, 7) * 1.5);
						return;
					case 2763:
						width = 18;
						height = 18;
						defense = 24;
						headSlot = 171;
						rare = 10;
						value = sellPrice(0, 7);
						return;
					case 2764:
						width = 18;
						height = 18;
						defense = 34;
						bodySlot = 177;
						rare = 10;
						value = sellPrice(0, 7) * 2;
						return;
					case 2765:
						width = 18;
						height = 18;
						defense = 20;
						legSlot = 112;
						rare = 10;
						value = (int)((double)sellPrice(0, 7) * 1.5);
						return;
					case 2767:
						useStyle = 4;
						width = 22;
						height = 14;
						consumable = true;
						useAnimation = 45;
						useTime = 45;
						maxStack = CommonMaxStack;
						rare = 8;
						return;
					case 2766:
						width = 22;
						height = 14;
						maxStack = CommonMaxStack;
						rare = 8;
						return;
					case 2770:
						width = 22;
						height = 20;
						accessory = true;
						value = eclipseMothronPrice;
						rare = 8;
						wingSlot = 27;
						return;
					case 2769:
						useStyle = 1;
						width = 32;
						height = 30;
						UseSound = SoundID.Item25;
						useAnimation = 20;
						useTime = 20;
						rare = 8;
						noMelee = true;
						mountType = 7;
						value = sellPrice(0, 5);
						return;
					case 2768:
						useStyle = 1;
						width = 32;
						height = 30;
						UseSound = SoundID.Item25;
						useAnimation = 20;
						useTime = 20;
						rare = 8;
						noMelee = true;
						mountType = 8;
						value = sellPrice(0, 5);
						return;
					case 2771:
						useStyle = 1;
						channel = true;
						width = 34;
						height = 34;
						UseSound = SoundID.Item90;
						useAnimation = 20;
						useTime = 20;
						rare = 8;
						noMelee = true;
						mountType = 9;
						value = sellPrice(0, 5);
						return;
					case 2772:
						autoReuse = true;
						useStyle = 1;
						useAnimation = 25;
						knockBack = 6f;
						useTime = 7;
						width = 54;
						height = 54;
						damage = 100;
						axe = 27;
						UseSound = SoundID.Item1;
						rare = 10;
						scale = 1.05f;
						value = sellPrice(0, 6);
						melee = true;
						glowMask = 1;
						tileBoost += 4;
						return;
					case 2773:
						useStyle = 5;
						useAnimation = 25;
						useTime = 7;
						shootSpeed = 28f;
						knockBack = 4f;
						width = 56;
						height = 22;
						damage = 80;
						axe = 27;
						UseSound = SoundID.Item23;
						shoot = 427;
						rare = 10;
						value = sellPrice(0, 6);
						noMelee = true;
						noUseGraphic = true;
						melee = true;
						channel = true;
						glowMask = 20;
						tileBoost += 4;
						return;
					case 2774:
						useStyle = 5;
						useAnimation = 25;
						useTime = 4;
						shootSpeed = 32f;
						knockBack = 0.5f;
						width = 54;
						height = 26;
						damage = 50;
						pick = 225;
						UseSound = SoundID.Item23;
						shoot = 428;
						rare = 10;
						value = sellPrice(0, 7);
						noMelee = true;
						noUseGraphic = true;
						melee = true;
						channel = true;
						glowMask = 21;
						tileBoost += 3;
						return;
					case 2776:
						useStyle = 1;
						useAnimation = 12;
						useTime = 6;
						knockBack = 5.5f;
						useTurn = true;
						autoReuse = true;
						width = 36;
						height = 36;
						damage = 80;
						pick = 225;
						UseSound = SoundID.Item1;
						rare = 10;
						value = sellPrice(0, 7);
						melee = true;
						glowMask = 5;
						tileBoost += 4;
						return;
					case 2775:
						useTurn = true;
						autoReuse = true;
						useStyle = 1;
						useAnimation = 30;
						useTime = 7;
						knockBack = 7f;
						width = 44;
						height = 42;
						damage = 110;
						hammer = 100;
						UseSound = SoundID.Item1;
						rare = 10;
						value = sellPrice(0, 8);
						melee = true;
						scale = 1.1f;
						glowMask = 4;
						tileBoost += 4;
						return;
					case 2777:
						SetDefaults3(2772);
						type = 2777;
						glowMask = 6;
						return;
					case 2778:
						SetDefaults3(2773);
						type = 2778;
						shoot = 429;
						glowMask = 22;
						return;
					case 2779:
						SetDefaults3(2774);
						type = 2779;
						shoot = 430;
						glowMask = 23;
						return;
					case 2780:
						SetDefaults3(2775);
						type = 2780;
						glowMask = 9;
						return;
					case 2781:
						SetDefaults3(2776);
						type = 2781;
						glowMask = 10;
						return;
					case 2782:
						SetDefaults3(2772);
						type = 2782;
						glowMask = -1;
						return;
					case 2783:
						SetDefaults3(2773);
						type = 2783;
						shoot = 431;
						glowMask = -1;
						return;
					case 2784:
						SetDefaults3(2774);
						type = 2784;
						shoot = 432;
						glowMask = -1;
						return;
					case 2785:
						SetDefaults3(2775);
						type = 2785;
						glowMask = -1;
						return;
					case 2786:
						SetDefaults3(2776);
						type = 2786;
						glowMask = -1;
						return;
					case 2787:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 345;
						width = 12;
						height = 12;
						return;
					case 2788:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 172;
						width = 12;
						height = 12;
						return;
					case 2789:
					case 2790:
					case 2791:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createWall = 173 + type - 2789;
						width = 12;
						height = 12;
						return;
					}
					if (type >= 2792 && type <= 2794)
					{
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 346 + type - 2792;
						width = 12;
						height = 12;
						return;
					}
					switch (type)
					{
					case 2795:
						useStyle = 5;
						useAnimation = 20;
						useTime = 20;
						shootSpeed = 20f;
						knockBack = 2f;
						width = 20;
						height = 12;
						damage = 60;
						shoot = 439;
						mana = 6;
						rare = 8;
						value = sellPrice(0, 10);
						noMelee = true;
						noUseGraphic = true;
						magic = true;
						channel = true;
						glowMask = 47;
						return;
					case 2796:
						useStyle = 5;
						useAnimation = 12;
						useTime = 12;
						width = 50;
						height = 18;
						shoot = 442;
						useAmmo = 771;
						glowMask = 36;
						UseSound = SoundID.Item92;
						damage = 40;
						shootSpeed = 12f;
						noMelee = true;
						value = sellPrice(0, 10);
						ranged = true;
						rare = 8;
						knockBack = 2f;
						return;
					case 2797:
						useStyle = 5;
						useAnimation = 21;
						useTime = 21;
						autoReuse = true;
						width = 50;
						height = 18;
						shoot = 444;
						useAmmo = AmmoID.Bullet;
						glowMask = 38;
						UseSound = SoundID.Item95;
						damage = 45;
						shootSpeed = 12f;
						noMelee = true;
						value = sellPrice(0, 10);
						ranged = true;
						rare = 8;
						knockBack = 3f;
						return;
					case 2798:
						useStyle = 5;
						useAnimation = 25;
						useTime = 6;
						shootSpeed = 36f;
						knockBack = 4.75f;
						width = 20;
						height = 12;
						damage = 35;
						pick = 230;
						shoot = 445;
						rare = 8;
						value = sellPrice(0, 10);
						tileBoost = 11;
						noMelee = true;
						noUseGraphic = true;
						melee = true;
						channel = true;
						glowMask = 39;
						return;
					case 2799:
						width = 10;
						height = 26;
						accessory = true;
						value = buyPrice(0, 1);
						rare = 1;
						return;
					case 2800:
						noUseGraphic = true;
						damage = 0;
						knockBack = 7f;
						useStyle = 5;
						shootSpeed = 14f;
						shoot = 446;
						width = 18;
						height = 28;
						UseSound = SoundID.Item1;
						useAnimation = 20;
						useTime = 20;
						rare = 7;
						noMelee = true;
						value = sellPrice(0, 2, 50);
						return;
					case 2801:
						width = 28;
						height = 20;
						headSlot = 172;
						rare = 1;
						vanity = true;
						return;
					case 2802:
						width = 28;
						height = 20;
						headSlot = 173;
						rare = 1;
						vanity = true;
						return;
					case 2803:
						width = 18;
						height = 18;
						headSlot = 174;
						vanity = true;
						value = sellPrice(0, 1);
						return;
					case 2804:
						width = 18;
						height = 18;
						bodySlot = 178;
						vanity = true;
						value = sellPrice(0, 1);
						return;
					case 2805:
						width = 18;
						height = 18;
						legSlot = 113;
						vanity = true;
						value = sellPrice(0, 1);
						return;
					case 2806:
						width = 18;
						height = 18;
						headSlot = 175;
						vanity = true;
						value = sellPrice(0, 1);
						glowMask = 46;
						return;
					case 2807:
						width = 18;
						height = 18;
						bodySlot = 179;
						vanity = true;
						value = sellPrice(0, 1);
						glowMask = 45;
						return;
					case 2808:
						width = 18;
						height = 18;
						legSlot = 114;
						vanity = true;
						value = sellPrice(0, 1);
						return;
					case 2822:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 19;
						placeStyle = 26;
						width = 8;
						height = 10;
						return;
					case 2810:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 90;
						placeStyle = 27;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2811:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						autoReuse = true;
						createTile = 79;
						placeStyle = 27;
						width = 28;
						height = 20;
						value = 2000;
						return;
					case 2823:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 89;
						placeStyle = 29;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2825:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 100;
						placeStyle = 27;
						width = 20;
						height = 20;
						value = 1500;
						return;
					case 2818:
						noWet = true;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 33;
						placeStyle = 26;
						width = 8;
						value = sellPrice(0, 0, 0, 60);
						height = 18;
						return;
					case 2812:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 15;
						placeStyle = 32;
						width = 12;
						height = 30;
						value = 150;
						return;
					case 2813:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 34;
						placeStyle = 33;
						width = 26;
						height = 26;
						value = 3000;
						return;
					case 2814:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 21;
						placeStyle = 48;
						width = 26;
						height = 22;
						value = 500;
						return;
					case 2815:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 10;
						placeStyle = 32;
						width = 14;
						height = 28;
						value = 200;
						return;
					case 2816:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 88;
						placeStyle = 24;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2820:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 42;
						width = 12;
						height = 28;
						placeStyle = 33;
						value = 150;
						return;
					case 2821:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 87;
						placeStyle = 26;
						width = 20;
						height = 20;
						value = 300;
						return;
					case 2824:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 14;
						placeStyle = 31;
						width = 26;
						height = 20;
						value = 300;
						return;
					case 2819:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 93;
						placeStyle = 27;
						width = 10;
						height = 24;
						value = 500;
						return;
					case 2826:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 18;
						placeStyle = 27;
						width = 28;
						height = 14;
						value = 150;
						return;
					case 2817:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 101;
						width = 20;
						height = 20;
						value = 300;
						placeStyle = 28;
						return;
					case 2809:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 104;
						width = 20;
						height = 20;
						value = 300;
						placeStyle = 24;
						return;
					case 2827:
					case 2828:
					case 2829:
					case 2830:
					case 2831:
					case 2832:
					case 2833:
					case 2834:
					case 2835:
					case 2836:
					case 2837:
					case 2838:
					case 2839:
					case 2840:
					case 2841:
					case 2842:
					case 2843:
					case 2844:
					case 2845:
					case 2846:
					case 2847:
					case 2848:
					case 2849:
					case 2850:
					case 2851:
					case 2852:
					case 2853:
					case 2854:
					case 2855:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 172;
						placeStyle = type - 2827;
						width = 20;
						height = 20;
						value = 300;
						if (type == 2843)
						{
							value = buyPrice(0, 10);
						}
						return;
					}
					switch (type)
					{
					case 2856:
						width = 28;
						height = 20;
						headSlot = 176;
						rare = 1;
						vanity = true;
						value = buyPrice(0, 10);
						return;
					case 2857:
						width = 28;
						height = 20;
						headSlot = 177;
						rare = 1;
						vanity = true;
						value = buyPrice(0, 10);
						return;
					case 2858:
						width = 18;
						height = 14;
						bodySlot = 180;
						rare = 1;
						vanity = true;
						value = buyPrice(0, 10);
						return;
					case 2859:
						width = 18;
						height = 14;
						bodySlot = 181;
						rare = 1;
						vanity = true;
						value = buyPrice(0, 10);
						return;
					case 2860:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						glowMask = 93;
						createTile = 350;
						width = 12;
						height = 12;
						return;
					case 2861:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 7;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						glowMask = 95;
						createWall = 176;
						width = 12;
						height = 12;
						return;
					case 2862:
						width = 28;
						height = 12;
						headSlot = 178;
						rare = 3;
						value = sellPrice(0, 1);
						vanity = true;
						glowMask = 97;
						return;
					case 2863:
						width = 20;
						height = 26;
						maxStack = CommonMaxStack;
						rare = 3;
						glowMask = 98;
						value = buyPrice(0, 30);
						UseSound = SoundID.Item3;
						useStyle = 9;
						useTurn = true;
						useAnimation = 17;
						useTime = 17;
						consumable = true;
						return;
					case 2864:
						glowMask = 99;
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 1, 50);
						rare = 3;
						return;
					case 2865:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 242;
						width = 30;
						height = 30;
						value = buyPrice(0, 2);
						placeStyle = 27;
						return;
					case 2866:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 242;
						width = 30;
						height = 30;
						value = buyPrice(0, 2);
						placeStyle = 28;
						return;
					case 2867:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 242;
						width = 30;
						height = 30;
						value = buyPrice(0, 2);
						placeStyle = 29;
						return;
					case 2868:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 351;
						width = 12;
						height = 12;
						value = buyPrice(0, 0, 1);
						return;
					case 2869:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 1, 50);
						rare = 3;
						return;
					case 2870:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 1, 50);
						rare = 3;
						return;
					case 2871:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 0, 75);
						rare = 2;
						return;
					case 2872:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 0, 75);
						rare = 2;
						return;
					case 2873:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 1, 50);
						rare = 3;
						return;
					case 2874:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = 10000;
						rare = 1;
						return;
					case 2875:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = 10000;
						rare = 1;
						return;
					case 2876:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = 10000;
						rare = 1;
						return;
					case 2877:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = 10000;
						rare = 1;
						return;
					case 2878:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 1, 50);
						rare = 3;
						glowMask = 105;
						return;
					case 2879:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 1, 50);
						rare = 3;
						glowMask = 104;
						return;
					case 2880:
						useStyle = 1;
						useAnimation = 20;
						useTime = 20;
						autoReuse = true;
						shoot = 451;
						shootSpeed = 11f;
						knockBack = 4.5f;
						width = 40;
						height = 40;
						damage = 100;
						scale = 1.05f;
						UseSound = SoundID.Item1;
						rare = 8;
						value = sellPrice(0, 10);
						melee = true;
						return;
					case 2882:
						useStyle = 5;
						useAnimation = 20;
						useTime = 20;
						shootSpeed = 14f;
						knockBack = 2f;
						width = 16;
						height = 16;
						damage = 100;
						UseSound = SoundID.Item75;
						shoot = 460;
						mana = 14;
						rare = 8;
						value = sellPrice(0, 10);
						noMelee = true;
						noUseGraphic = true;
						magic = true;
						channel = true;
						glowMask = 102;
						return;
					case 2883:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 1, 50);
						rare = 3;
						glowMask = 103;
						return;
					case 2885:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 1, 50);
						rare = 3;
						glowMask = 106;
						return;
					case 2884:
						width = 20;
						height = 20;
						maxStack = CommonMaxStack;
						value = sellPrice(0, 1, 50);
						rare = 3;
						glowMask = 107;
						return;
					case 2887:
						width = 16;
						height = 18;
						maxStack = CommonMaxStack;
						value = 50;
						return;
					case 2886:
						damage = 0;
						useStyle = 1;
						shootSpeed = 4f;
						shoot = 463;
						width = 16;
						height = 24;
						maxStack = CommonMaxStack;
						consumable = true;
						UseSound = SoundID.Item1;
						useAnimation = 15;
						useTime = 15;
						noMelee = true;
						value = 100;
						return;
					case 2888:
						useStyle = 5;
						useAnimation = 23;
						useTime = 23;
						width = 12;
						height = 28;
						shoot = 469;
						useAmmo = AmmoID.Arrow;
						UseSound = SoundID.Item97;
						damage = 23;
						shootSpeed = 8f;
						knockBack = 3f;
						rare = 3;
						noMelee = true;
						value = queenBeePrice;
						ranged = true;
						return;
					case 2889:
					case 2890:
					case 2891:
					case 2892:
					case 2893:
					case 2894:
					case 2895:
						useStyle = 1;
						autoReuse = true;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						maxStack = CommonMaxStack;
						consumable = true;
						width = 12;
						height = 12;
						makeNPC = (short)(442 + type - 2889);
						noUseGraphic = true;
						value = sellPrice(0, 10);
						rare = 3;
						if (type == 2895 || type == 2893 || type == 2891)
						{
							bait = 50;
						}
						return;
					}
					switch (type)
					{
					case 2896:
						useStyle = 1;
						shootSpeed = 4f;
						shoot = 470;
						width = 8;
						height = 28;
						maxStack = CommonMaxStack;
						consumable = true;
						UseSound = SoundID.Item1;
						useAnimation = 40;
						useTime = 40;
						noUseGraphic = true;
						noMelee = true;
						value = buyPrice(0, 0, 20);
						rare = 1;
						return;
					case 2897:
					case 2898:
					case 2899:
					case 2900:
					case 2901:
					case 2902:
					case 2903:
					case 2904:
					case 2905:
					case 2906:
					case 2907:
					case 2908:
					case 2909:
					case 2910:
					case 2911:
					case 2912:
					case 2913:
					case 2914:
					case 2915:
					case 2916:
					case 2917:
					case 2918:
					case 2919:
					case 2920:
					case 2921:
					case 2922:
					case 2923:
					case 2924:
					case 2925:
					case 2926:
					case 2927:
					case 2928:
					case 2929:
					case 2930:
					case 2931:
					case 2932:
					case 2933:
					case 2934:
					case 2935:
					case 2936:
					case 2937:
					case 2938:
					case 2939:
					case 2940:
					case 2941:
					case 2942:
					case 2943:
					case 2944:
					case 2945:
					case 2946:
					case 2947:
					case 2948:
					case 2949:
					case 2950:
					case 2951:
					case 2952:
					case 2953:
					case 2954:
					case 2955:
					case 2956:
					case 2957:
					case 2958:
					case 2959:
					case 2960:
					case 2961:
					case 2962:
					case 2963:
					case 2964:
					case 2965:
					case 2966:
					case 2967:
					case 2968:
					case 2969:
					case 2970:
					case 2971:
					case 2972:
					case 2973:
					case 2974:
					case 2975:
					case 2976:
					case 2977:
					case 2978:
					case 2979:
					case 2980:
					case 2981:
					case 2982:
					case 2983:
					case 2984:
					case 2985:
					case 2986:
					case 2987:
					case 2988:
					case 2989:
					case 2990:
					case 2991:
					case 2992:
					case 2993:
					case 2994:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 91;
						placeStyle = 109 + type - 2897;
						width = 10;
						height = 24;
						value = 1000;
						rare = 1;
						return;
					}
					switch (type)
					{
					case 2995:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 242;
						width = 30;
						height = 30;
						value = sellPrice(0, 0, 10);
						placeStyle = 30;
						break;
					case 2996:
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 8;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 353;
						width = 12;
						height = 12;
						tileBoost += 3;
						break;
					case 2997:
						maxStack = CommonMaxStack;
						consumable = true;
						width = 14;
						height = 24;
						value = 1000;
						rare = 1;
						break;
					case 2998:
						width = 24;
						height = 24;
						accessory = true;
						value = 100000;
						rare = 4;
						break;
					case 2999:
						rare = 1;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 354;
						width = 12;
						height = 12;
						value = 100000;
						break;
					case 3000:
						rare = 1;
						useStyle = 1;
						useTurn = true;
						useAnimation = 15;
						useTime = 10;
						autoReuse = true;
						maxStack = CommonMaxStack;
						consumable = true;
						createTile = 355;
						width = 12;
						height = 12;
						value = 100000;
						break;
					}
					return;
				}
			}
			useStyle = 1;
			useTurn = true;
			useAnimation = 15;
			useTime = 10;
			autoReuse = true;
			maxStack = CommonMaxStack;
			consumable = true;
			switch (type)
			{
			case 2203:
				createTile = 307;
				break;
			case 2204:
				createTile = 308;
				break;
			default:
				createTile = 300 + type - 2192;
				break;
			}
			width = 12;
			height = 12;
			value = buyPrice(0, 10);
		}

		public void DefaultToQuestFish()
		{
			questItem = true;
			maxStack = 1;
			width = 26;
			height = 26;
			uniqueStack = true;
			rare = -11;
		}

		public void SetDefaults4(int type)
		{
			switch (type)
			{
			case 3001:
				rare = 1;
				UseSound = SoundID.Item3;
				healLife = 70;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				potion = true;
				value = buyPrice(0, 0, 5);
				return;
			case 3061:
				width = 30;
				height = 30;
				accessory = true;
				rare = 5;
				value = buyPrice(0, 20);
				backSlot = 8;
				return;
			case 3002:
				alpha = 0;
				color = new Color(255, 255, 255, 0);
				rare = 1;
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 473;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noMelee = true;
				value = buyPrice(0, 0, 1, 50);
				holdStyle = 1;
				return;
			case 3003:
				shootSpeed = 3.5f;
				shoot = 474;
				damage = 8;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 2.5f;
				value = buyPrice(0, 0, 0, 15);
				ranged = true;
				return;
			case 3004:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 13;
				width = 10;
				height = 12;
				value = buyPrice(0, 0, 1);
				return;
			case 3005:
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 475;
				damage = 0;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				noMelee = true;
				return;
			case 3006:
				mana = 10;
				autoReuse = true;
				damage = 35;
				useStyle = 5;
				shootSpeed = 10f;
				shoot = 476;
				width = 26;
				height = 28;
				useAnimation = 12;
				useTime = 12;
				rare = 5;
				noMelee = true;
				knockBack = 2.5f;
				value = sellPrice(0, 8);
				magic = true;
				return;
			case 3007:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 22;
				useTime = 22;
				width = 38;
				height = 6;
				shoot = 10;
				useAmmo = AmmoID.Dart;
				UseSound = SoundID.Item98;
				damage = 28;
				shootSpeed = 13f;
				noMelee = true;
				value = sellPrice(0, 8);
				knockBack = 3.5f;
				useAmmo = AmmoID.Dart;
				ranged = true;
				rare = 5;
				scale = 0.9f;
				return;
			case 3008:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 38;
				useTime = 38;
				width = 38;
				height = 6;
				shoot = 10;
				useAmmo = AmmoID.Dart;
				UseSound = SoundID.Item99;
				damage = 52;
				shootSpeed = 14.5f;
				noMelee = true;
				value = sellPrice(0, 8);
				knockBack = 5.5f;
				useAmmo = AmmoID.Dart;
				ranged = true;
				rare = 5;
				scale = 1f;
				return;
			case 3009:
				shoot = 477;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				ammo = AmmoID.Dart;
				damage = 14;
				knockBack = 3.5f;
				shootSpeed = 1f;
				ranged = true;
				rare = 3;
				value = sellPrice(0, 0, 0, 6);
				consumable = true;
				return;
			case 3010:
				shoot = 478;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				ammo = AmmoID.Dart;
				damage = 9;
				knockBack = 2.2f;
				shootSpeed = 3f;
				ranged = true;
				rare = 3;
				value = sellPrice(0, 0, 0, 6);
				consumable = true;
				return;
			case 3011:
				shoot = 479;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				ammo = AmmoID.Dart;
				damage = 10;
				knockBack = 2.5f;
				shootSpeed = 3f;
				ranged = true;
				rare = 3;
				value = sellPrice(0, 0, 0, 6);
				consumable = true;
				return;
			case 3012:
				autoReuse = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 14;
				useTime = 14;
				knockBack = 3.25f;
				width = 30;
				height = 10;
				damage = 59;
				shoot = 481;
				shootSpeed = 9f;
				UseSound = SoundID.Item1;
				rare = 5;
				value = sellPrice(0, 8);
				melee = true;
				noUseGraphic = true;
				noMelee = true;
				return;
			case 3013:
				useStyle = 1;
				useTurn = true;
				autoReuse = true;
				useAnimation = 8;
				useTime = 8;
				width = 24;
				height = 28;
				damage = 60;
				knockBack = 6f;
				UseSound = SoundID.Item1;
				scale = 1.35f;
				melee = true;
				rare = 5;
				value = sellPrice(0, 8);
				melee = true;
				return;
			case 3014:
				mana = 40;
				autoReuse = true;
				damage = 43;
				useStyle = 1;
				shootSpeed = 15f;
				shoot = 482;
				width = 26;
				height = 28;
				UseSound = SoundID.Item100;
				useAnimation = 24;
				useTime = 24;
				rare = 5;
				noMelee = true;
				knockBack = 8f;
				value = sellPrice(0, 8);
				magic = true;
				return;
			case 3024:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 9;
				value = sellPrice(0, 3);
				return;
			case 3599:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 9;
				value = sellPrice(0, 3);
				return;
			case 3015:
				width = 24;
				height = 24;
				accessory = true;
				value = sellPrice(0, 8);
				rare = 6;
				return;
			case 3016:
				width = 24;
				height = 24;
				accessory = true;
				defense = 8;
				value = sellPrice(0, 8);
				rare = 5;
				return;
			case 3017:
				width = 16;
				height = 24;
				accessory = true;
				rare = 7;
				value = sellPrice(0, 6);
				shoeSlot = 16;
				return;
			case 3018:
				useStyle = 1;
				autoReuse = true;
				useAnimation = 23;
				useTime = 23;
				width = 50;
				height = 20;
				shoot = 483;
				UseSound = SoundID.Item1;
				damage = 50;
				shootSpeed = 12f;
				value = sellPrice(0, 10);
				knockBack = 6f;
				rare = 5;
				melee = true;
				return;
			case 3019:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 13;
				useTime = 13;
				width = 18;
				height = 46;
				shoot = 485;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 22;
				knockBack = 5.5f;
				shootSpeed = 6f;
				noMelee = true;
				value = hellPrice;
				rare = 3;
				ranged = true;
				return;
			case 3020:
			case 3021:
			case 3022:
			case 3023:
				noUseGraphic = true;
				damage = 0;
				useStyle = 5;
				shootSpeed = 15f;
				shoot = 486 + type - 3020;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 6;
				noMelee = true;
				value = sellPrice(0, 6);
				if (type == 3021)
				{
					shootSpeed = 16f;
				}
				return;
			}
			switch (type)
			{
			case 3025:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3026:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3027:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3190:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3038:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3597:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3600:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3598:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3029:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 19;
				useTime = 19;
				width = 28;
				height = 60;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 38;
				shootSpeed = 12.5f;
				noMelee = true;
				value = sellPrice(0, 8);
				ranged = true;
				rare = 6;
				knockBack = 2.25f;
				return;
			case 3030:
				channel = true;
				damage = 40;
				useStyle = 1;
				shootSpeed = 17f;
				shoot = 491;
				width = 26;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				rare = 6;
				noMelee = true;
				knockBack = 4.5f;
				value = sellPrice(0, 8);
				melee = true;
				noUseGraphic = true;
				return;
			case 3031:
			case 3032:
				useStyle = 1;
				useTurn = true;
				useAnimation = 12;
				useTime = 5;
				width = 20;
				height = 20;
				autoReuse = true;
				rare = 7;
				value = sellPrice(0, 10);
				tileBoost += 2;
				return;
			case 3036:
				width = 24;
				height = 28;
				rare = 3;
				value = sellPrice(0, 3);
				accessory = true;
				return;
			case 3037:
				width = 24;
				height = 28;
				rare = 1;
				value = sellPrice(0, 1);
				accessory = true;
				return;
			case 3033:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = 50000;
				return;
			case 3034:
				width = 16;
				height = 24;
				accessory = true;
				rare = 5;
				value = 100000;
				return;
			case 3035:
				width = 16;
				height = 24;
				accessory = true;
				rare = 6;
				value = 150000;
				return;
			case 3039:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3040:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3028:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3041:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3042:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3043:
				damage = 0;
				useStyle = 1;
				shoot = 492;
				width = 16;
				height = 30;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = buyPrice(0, 10);
				buffType = 152;
				return;
			case 3044:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 32;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				return;
			case 3045:
				flame = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 14;
				width = 10;
				height = 12;
				value = 250;
				rare = 1;
				return;
			case 3046:
			case 3047:
			case 3048:
			case 3049:
			case 3050:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 215;
				placeStyle = 1 + type - 3046;
				width = 12;
				height = 12;
				return;
			}
			switch (type)
			{
			case 3051:
				mana = 13;
				damage = 25;
				useStyle = 5;
				shootSpeed = 32f;
				shoot = 494;
				width = 26;
				height = 28;
				useAnimation = 33;
				useTime = 33;
				rare = 5;
				noMelee = true;
				knockBack = 3f;
				value = sellPrice(0, 8);
				magic = true;
				autoReuse = true;
				return;
			case 3052:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 20;
				useTime = 20;
				width = 14;
				height = 32;
				shoot = 495;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item102;
				damage = 47;
				shootSpeed = 11f;
				knockBack = 4.5f;
				rare = 5;
				crit = 3;
				noMelee = true;
				value = sellPrice(0, 2);
				ranged = true;
				return;
			case 3053:
				autoReuse = true;
				rare = 5;
				mana = 6;
				UseSound = SoundID.Item103;
				useStyle = 5;
				damage = 32;
				useAnimation = 21;
				useTime = 7;
				width = 24;
				height = 28;
				shoot = 496;
				shootSpeed = 9f;
				knockBack = 3.75f;
				magic = true;
				value = sellPrice(0, 2);
				noMelee = true;
				noUseGraphic = true;
				crit = 3;
				return;
			case 3054:
				crit = 3;
				autoReuse = true;
				useStyle = 1;
				shootSpeed = 13f;
				shoot = 497;
				damage = 38;
				width = 18;
				height = 20;
				UseSound = SoundID.Item1;
				useAnimation = 12;
				useTime = 12;
				noUseGraphic = true;
				noMelee = true;
				value = sellPrice(0, 2);
				knockBack = 5.75f;
				melee = true;
				rare = 5;
				return;
			case 3055:
			case 3056:
			case 3057:
			case 3058:
			case 3059:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 31 + type - 3055;
				return;
			}
			switch (type)
			{
			case 3060:
				damage = 0;
				useStyle = 1;
				shoot = 499;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 7, 50);
				buffType = 154;
				return;
			case 3062:
				channel = true;
				damage = 0;
				useStyle = 4;
				shoot = 500;
				width = 24;
				height = 24;
				UseSound = SoundID.Item8;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = shadowOrbPrice;
				buffType = 155;
				return;
			case 3063:
				rare = 10;
				UseSound = SoundID.Item1;
				useStyle = 1;
				damage = 200;
				useAnimation = 14;
				useTime = 14;
				width = 30;
				height = 30;
				shoot = 502;
				scale = 1.1f;
				shootSpeed = 12f;
				knockBack = 6.5f;
				melee = true;
				value = sellPrice(0, 20);
				autoReuse = true;
				return;
			case 3064:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 356;
				width = 18;
				height = 34;
				value = sellPrice(0, 3);
				rare = 7;
				return;
			case 3065:
				rare = 10;
				UseSound = SoundID.Item105;
				useStyle = 1;
				damage = 170;
				useAnimation = 16;
				useTime = 16;
				width = 30;
				height = 30;
				shoot = 503;
				scale = 1.1f;
				shootSpeed = 8f;
				knockBack = 6.5f;
				melee = true;
				value = sellPrice(0, 20);
				autoReuse = true;
				return;
			case 3066:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 357;
				width = 12;
				height = 12;
				return;
			case 3067:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 177;
				width = 12;
				height = 12;
				return;
			case 3068:
				width = 16;
				height = 24;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 0, 50);
				return;
			case 3069:
				mana = 2;
				damage = 14;
				useStyle = 1;
				shootSpeed = 7f;
				shoot = 954;
				width = 26;
				height = 28;
				UseSound = SoundID.Item8;
				useAnimation = 26;
				useTime = 26;
				rare = 1;
				noMelee = true;
				value = 10000;
				magic = true;
				crit = 10;
				if (Variant == ItemVariants.StrongerVariant)
				{
					value = sellPrice(0, 5);
					rare = 4;
					damage = 40;
					useAnimation = 10;
					useTime = 10;
					mana = 6;
					shootSpeed = 12f;
					autoReuse = true;
				}
				return;
			case 3070:
			case 3071:
			case 3072:
			case 3073:
			case 3074:
			case 3075:
			case 3076:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 358 + type - 3070;
				width = 12;
				height = 12;
				value = sellPrice(0, 10);
				rare = 3;
				return;
			}
			switch (type)
			{
			case 3077:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 8;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 365;
				width = 12;
				height = 12;
				value = 10;
				tileBoost += 3;
				return;
			case 3078:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 8;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 366;
				width = 12;
				height = 12;
				value = 10;
				tileBoost += 3;
				return;
			case 3081:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 367;
				width = 12;
				height = 12;
				return;
			case 3082:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 183;
				width = 12;
				height = 12;
				return;
			case 3083:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 179;
				width = 12;
				height = 12;
				return;
			case 3084:
				width = 24;
				height = 18;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 0, 50);
				return;
			case 3085:
				width = 12;
				height = 12;
				rare = 2;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 2);
				return;
			case 3086:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 368;
				width = 12;
				height = 12;
				return;
			case 3080:
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 506;
				damage = 0;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				noMelee = true;
				value = 100;
				return;
			case 3079:
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 505;
				damage = 0;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				noMelee = true;
				value = 100;
				return;
			case 3087:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 369;
				width = 12;
				height = 12;
				return;
			case 3088:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 184;
				width = 12;
				height = 12;
				return;
			case 3089:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 181;
				width = 12;
				height = 12;
				return;
			case 3090:
				width = 16;
				height = 24;
				accessory = true;
				rare = 2;
				value = 100000;
				expert = true;
				return;
			case 3091:
			case 3092:
				width = 14;
				height = 20;
				maxStack = CommonMaxStack;
				useAnimation = 20;
				useTime = 20;
				return;
			case 3093:
				width = 12;
				height = 12;
				rare = 1;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 10);
				return;
			case 3094:
				useStyle = 1;
				shootSpeed = 11.5f;
				shoot = 507;
				damage = 17;
				width = 30;
				height = 30;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 24;
				useTime = 24;
				noUseGraphic = true;
				noMelee = true;
				knockBack = 4.75f;
				value = sellPrice(0, 0, 0, 5);
				ranged = true;
				return;
			case 3095:
				width = 24;
				height = 18;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 1);
				return;
			case 3097:
				melee = true;
				damage = 30;
				width = 24;
				height = 28;
				rare = 1;
				value = sellPrice(0, 2);
				accessory = true;
				defense = 2;
				shieldSlot = 5;
				knockBack = 9f;
				expert = true;
				return;
			case 3098:
				useStyle = 5;
				useAnimation = 25;
				useTime = 8;
				shootSpeed = 48f;
				knockBack = 8f;
				width = 54;
				height = 20;
				damage = 120;
				axe = 30;
				UseSound = SoundID.Item23;
				shoot = 509;
				rare = 8;
				value = eclipsePostPlanteraPrice;
				noMelee = true;
				noUseGraphic = true;
				melee = true;
				channel = true;
				return;
			case 3099:
				width = 24;
				height = 18;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 1);
				return;
			case 3100:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 370;
				width = 12;
				height = 12;
				return;
			case 3101:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 182;
				width = 12;
				height = 12;
				return;
			case 3102:
				width = 24;
				height = 18;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 1);
				return;
			case 3103:
				shootSpeed = 3f;
				shoot = 1;
				damage = 5;
				width = 26;
				height = 26;
				ammo = AmmoID.Arrow;
				knockBack = 2f;
				value = sellPrice(0, 1);
				ranged = true;
				rare = 2;
				return;
			case 3104:
				shootSpeed = 4f;
				shoot = 14;
				damage = 7;
				width = 26;
				height = 26;
				ammo = AmmoID.Bullet;
				knockBack = 2f;
				value = sellPrice(0, 1);
				ranged = true;
				rare = 2;
				return;
			case 3105:
				magic = true;
				mana = 30;
				useStyle = 1;
				shootSpeed = 14f;
				rare = 8;
				damage = 52;
				shoot = 510;
				width = 18;
				height = 20;
				knockBack = 4f;
				UseSound = SoundID.Item106;
				useAnimation = 45;
				useTime = 45;
				noUseGraphic = true;
				noMelee = true;
				value = eclipsePostPlanteraPrice;
				return;
			case 3106:
				autoReuse = true;
				useStyle = 1;
				useAnimation = 8;
				useTime = 8;
				knockBack = 3.5f;
				width = 30;
				height = 30;
				damage = 85;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				rare = 8;
				value = eclipsePostPlanteraPrice;
				melee = true;
				return;
			case 3107:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 15;
				useTime = 15;
				width = 50;
				height = 18;
				shoot = 514;
				useAmmo = AmmoID.NailFriendly;
				UseSound = SoundID.Item108;
				damage = 85;
				shootSpeed = 10f;
				noMelee = true;
				value = eclipsePostPlanteraPrice;
				rare = 8;
				ranged = true;
				return;
			case 3108:
				shootSpeed = 6f;
				shoot = 514;
				damage = 30;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.NailFriendly;
				knockBack = 3f;
				value = buyPrice(0, 0, 1);
				ranged = true;
				rare = 8;
				return;
			case 3109:
				width = 22;
				height = 22;
				defense = 4;
				headSlot = 179;
				rare = 2;
				value = sellPrice(0, 1);
				return;
			case 3110:
				width = 16;
				height = 24;
				accessory = true;
				rare = 8;
				value = 700000;
				hasVanityEffects = true;
				return;
			case 3111:
				width = 10;
				height = 12;
				maxStack = CommonMaxStack;
				alpha = 100;
				value = 15;
				return;
			case 3112:
				color = new Color(255, 255, 255, 0);
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 515;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noMelee = true;
				value = 10;
				holdStyle = 1;
				return;
			case 3113:
				createTile = 371;
				width = 12;
				height = 12;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				return;
			case 3114:
				flame = true;
				noWet = true;
				holdStyle = 1;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 4;
				placeStyle = 15;
				width = 10;
				height = 12;
				value = 80;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				return;
			case 3115:
				useStyle = 1;
				shootSpeed = 5f;
				shoot = 516;
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 25;
				useTime = 25;
				noUseGraphic = true;
				noMelee = true;
				value = buyPrice(0, 0, 4);
				damage = 0;
				return;
			case 3116:
				useStyle = 5;
				shootSpeed = 6.5f;
				shoot = 517;
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 40;
				useTime = 40;
				noUseGraphic = true;
				noMelee = true;
				value = 100;
				damage = 65;
				knockBack = 8f;
				ranged = true;
				return;
			case 3117:
				flame = true;
				noWet = true;
				createTile = 372;
				width = 8;
				height = 18;
				holdStyle = 1;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 1);
				consumable = true;
				return;
			case 3203:
			case 3204:
			case 3205:
			case 3206:
			case 3207:
			case 3208:
				width = 12;
				height = 12;
				rare = 2;
				maxStack = CommonMaxStack;
				createTile = 376;
				placeStyle = 3 + type - 3203;
				useAnimation = 15;
				useTime = 15;
				autoReuse = true;
				useStyle = 1;
				consumable = true;
				value = sellPrice(0, 1);
				return;
			}
			switch (type)
			{
			case 3209:
				mana = 9;
				UseSound = SoundID.Item109;
				useStyle = 5;
				damage = 40;
				useAnimation = 29;
				useTime = 29;
				width = 36;
				height = 40;
				shoot = 521;
				shootSpeed = 13f;
				knockBack = 4.4f;
				magic = true;
				autoReuse = true;
				value = sellPrice(0, 4);
				rare = 5;
				noMelee = true;
				return;
			case 3210:
				UseSound = SoundID.Item111;
				useStyle = 5;
				damage = 43;
				useAnimation = 10;
				useTime = 10;
				width = 30;
				height = 28;
				shoot = 523;
				shootSpeed = 8.5f;
				knockBack = 3f;
				ranged = true;
				autoReuse = true;
				value = sellPrice(0, 4);
				rare = 5;
				noMelee = true;
				return;
			case 3211:
				useStyle = 1;
				useAnimation = 28;
				useTime = 28;
				knockBack = 5.75f;
				width = 40;
				height = 40;
				damage = 55;
				scale = 1.125f;
				UseSound = SoundID.Item1;
				rare = 5;
				autoReuse = true;
				value = sellPrice(0, 4);
				melee = true;
				return;
			case 3212:
				width = 22;
				height = 22;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 1);
				neckSlot = 7;
				return;
			case 3213:
				useStyle = 1;
				shootSpeed = 4f;
				shoot = 525;
				width = 26;
				height = 24;
				UseSound = SoundID.Item59;
				useAnimation = 28;
				useTime = 28;
				rare = 3;
				value = sellPrice(0, 2);
				return;
			case 3119:
				width = 24;
				height = 18;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 1);
				return;
			case 3118:
				width = 24;
				height = 18;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 1);
				return;
			case 3096:
				width = 24;
				height = 18;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 1);
				return;
			case 3120:
				width = 24;
				height = 28;
				rare = 1;
				value = sellPrice(0, 1);
				accessory = true;
				return;
			case 3121:
				width = 24;
				height = 28;
				rare = 3;
				value = sellPrice(0, 3);
				accessory = true;
				return;
			case 3122:
				width = 24;
				height = 28;
				rare = 3;
				value = sellPrice(0, 3);
				accessory = true;
				return;
			case 3123:
				width = 24;
				height = 28;
				rare = 5;
				value = sellPrice(0, 5);
				accessory = true;
				return;
			case 3124:
				width = 24;
				height = 28;
				rare = 7;
				value = sellPrice(0, 8);
				useTurn = true;
				useStyle = 4;
				useTime = 90;
				UseSound = SoundID.Item6;
				useAnimation = 90;
				return;
			case 3159:
			case 3160:
			case 3161:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 90;
				switch (type)
				{
				case 3159:
					placeStyle = 28;
					break;
				case 3160:
					placeStyle = 30;
					break;
				case 3161:
					placeStyle = 29;
					break;
				}
				width = 20;
				height = 20;
				value = 300;
				return;
			case 3162:
			case 3163:
			case 3164:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				autoReuse = true;
				createTile = 79;
				width = 28;
				height = 20;
				value = 2000;
				switch (type)
				{
				case 3162:
					placeStyle = 28;
					break;
				case 3163:
					placeStyle = 30;
					break;
				case 3164:
					placeStyle = 29;
					break;
				}
				return;
			case 3165:
			case 3166:
			case 3167:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				switch (type)
				{
				case 3165:
					placeStyle = 29;
					break;
				case 3166:
					placeStyle = 31;
					break;
				case 3167:
					placeStyle = 30;
					break;
				}
				return;
			case 3168:
			case 3169:
			case 3170:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 100;
				width = 20;
				height = 20;
				value = 1500;
				switch (type)
				{
				case 3168:
					placeStyle = 28;
					break;
				case 3169:
					placeStyle = 30;
					break;
				case 3170:
					placeStyle = 29;
					break;
				}
				return;
			case 3171:
			case 3172:
			case 3173:
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 33;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				switch (type)
				{
				case 3171:
					placeStyle = 27;
					break;
				case 3172:
					placeStyle = 29;
					break;
				case 3173:
					placeStyle = 28;
					break;
				}
				return;
			case 3174:
			case 3175:
			case 3176:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				width = 12;
				height = 30;
				value = 150;
				switch (type)
				{
				case 3174:
					placeStyle = 33;
					break;
				case 3175:
					placeStyle = 35;
					break;
				case 3176:
					placeStyle = 34;
					break;
				}
				return;
			case 3177:
			case 3178:
			case 3179:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				width = 26;
				height = 26;
				value = 3000;
				switch (type)
				{
				case 3177:
					placeStyle = 34;
					break;
				case 3178:
					placeStyle = 36;
					break;
				case 3179:
					placeStyle = 35;
					break;
				}
				return;
			case 3125:
			case 3180:
			case 3181:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 21;
				width = 26;
				height = 22;
				value = 500;
				switch (type)
				{
				case 3180:
					placeStyle = 49;
					break;
				case 3181:
					placeStyle = 51;
					break;
				case 3125:
					placeStyle = 50;
					break;
				}
				return;
			case 3126:
			case 3127:
			case 3128:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 104;
				width = 20;
				height = 20;
				value = 300;
				switch (type)
				{
				case 3126:
					placeStyle = 25;
					break;
				case 3127:
					placeStyle = 27;
					break;
				case 3128:
					placeStyle = 26;
					break;
				}
				return;
			case 3129:
			case 3130:
			case 3131:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				width = 14;
				height = 28;
				value = 200;
				switch (type)
				{
				case 3129:
					placeStyle = 33;
					break;
				case 3130:
					placeStyle = 35;
					break;
				case 3131:
					placeStyle = 34;
					break;
				}
				return;
			case 3132:
			case 3133:
			case 3134:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 88;
				width = 20;
				height = 20;
				value = 300;
				switch (type)
				{
				case 3132:
					placeStyle = 25;
					break;
				case 3133:
					placeStyle = 27;
					break;
				case 3134:
					placeStyle = 26;
					break;
				}
				return;
			case 3135:
			case 3136:
			case 3137:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 93;
				width = 10;
				height = 24;
				value = 500;
				switch (type)
				{
				case 3135:
					placeStyle = 28;
					break;
				case 3136:
					placeStyle = 30;
					break;
				case 3137:
					placeStyle = 29;
					break;
				}
				return;
			case 3138:
			case 3139:
			case 3140:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				switch (type)
				{
				case 3138:
					placeStyle = 34;
					break;
				case 3139:
					placeStyle = 36;
					break;
				case 3140:
					placeStyle = 35;
					break;
				}
				value = 150;
				return;
			case 3141:
			case 3142:
			case 3143:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 87;
				width = 20;
				height = 20;
				value = 300;
				switch (type)
				{
				case 3141:
					placeStyle = 27;
					break;
				case 3142:
					placeStyle = 29;
					break;
				case 3143:
					placeStyle = 28;
					break;
				}
				return;
			case 3144:
			case 3145:
			case 3146:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				width = 8;
				height = 10;
				switch (type)
				{
				case 3144:
					placeStyle = 27;
					break;
				case 3145:
					placeStyle = 29;
					break;
				case 3146:
					placeStyle = 28;
					break;
				}
				return;
			case 3147:
			case 3148:
			case 3149:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 172;
				width = 20;
				height = 20;
				value = 300;
				switch (type)
				{
				case 3147:
					placeStyle = 29;
					break;
				case 3148:
					placeStyle = 31;
					break;
				case 3149:
					placeStyle = 30;
					break;
				}
				return;
			case 3150:
			case 3151:
			case 3152:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 89;
				width = 20;
				height = 20;
				value = 300;
				switch (type)
				{
				case 3150:
					placeStyle = 30;
					break;
				case 3151:
					placeStyle = 32;
					break;
				case 3152:
					placeStyle = 31;
					break;
				}
				return;
			case 3153:
			case 3154:
			case 3155:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 14;
				width = 26;
				height = 20;
				value = 300;
				switch (type)
				{
				case 3153:
					placeStyle = 32;
					break;
				case 3154:
					placeStyle = 34;
					break;
				case 3155:
					placeStyle = 33;
					break;
				}
				return;
			case 3156:
			case 3157:
			case 3158:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				width = 28;
				height = 14;
				value = 150;
				switch (type)
				{
				case 3156:
					placeStyle = 28;
					break;
				case 3157:
					placeStyle = 30;
					break;
				case 3158:
					placeStyle = 29;
					break;
				}
				return;
			case 3182:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 373;
				width = 24;
				height = 24;
				value = sellPrice(0, 0, 0, 40);
				return;
			case 3183:
				useTurn = true;
				useStyle = 1;
				useTime = 18;
				useAnimation = 18;
				width = 24;
				height = 28;
				UseSound = SoundID.Item1;
				value = sellPrice(0, 5);
				autoReuse = true;
				rare = 4;
				scale = 1.15f;
				return;
			case 3184:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 374;
				width = 24;
				height = 24;
				value = sellPrice(0, 0, 0, 40);
				return;
			case 3185:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 375;
				width = 24;
				height = 24;
				value = sellPrice(0, 0, 0, 40);
				return;
			case 3186:
				maxStack = CommonMaxStack;
				width = 24;
				height = 24;
				value = buyPrice(0, 0, 1);
				return;
			case 3187:
				width = 18;
				height = 18;
				defense = 5;
				headSlot = 180;
				value = 17500;
				return;
			case 3188:
				width = 18;
				height = 18;
				defense = 6;
				bodySlot = 182;
				value = 14000;
				return;
			case 3189:
				width = 18;
				height = 18;
				defense = 5;
				legSlot = 122;
				value = 10500;
				return;
			case 3191:
			case 3192:
			case 3193:
			case 3194:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = (short)(484 + type - 3191);
				noUseGraphic = true;
				switch (type)
				{
				case 3192:
					bait = 15;
					break;
				case 3193:
					bait = 25;
					break;
				case 3194:
					bait = 40;
					break;
				default:
					bait = 35;
					break;
				}
				return;
			}
			switch (type)
			{
			case 3196:
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 519;
				width = 26;
				height = 26;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 25;
				useTime = 25;
				noUseGraphic = true;
				noMelee = true;
				value = sellPrice(0, 0, 2);
				damage = 0;
				rare = 1;
				return;
			case 3197:
				rare = 1;
				useStyle = 1;
				shootSpeed = 12.5f;
				shoot = 520;
				damage = 17;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 13;
				useTime = 13;
				noUseGraphic = true;
				noMelee = true;
				value = 80;
				knockBack = 3.5f;
				ranged = true;
				return;
			case 3198:
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 377;
				width = 28;
				height = 22;
				value = 100000;
				return;
			case 3199:
				useTurn = true;
				width = 20;
				height = 20;
				useStyle = 4;
				useTime = 90;
				UseSound = SoundID.Item6;
				useAnimation = 90;
				rare = 1;
				value = 50000;
				return;
			case 3200:
				width = 28;
				height = 24;
				accessory = true;
				rare = 1;
				value = 50000;
				shoeSlot = 17;
				return;
			case 3201:
				width = 16;
				height = 24;
				accessory = true;
				rare = 1;
				value = 50000;
				waistSlot = 11;
				return;
			case 3202:
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 378;
				width = 20;
				height = 30;
				value = sellPrice(0, 0, 1);
				return;
			case 3214:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 379;
				width = 12;
				height = 12;
				value = buyPrice(0, 0, 2);
				return;
			case 3215:
			case 3216:
			case 3217:
			case 3218:
			case 3219:
			case 3220:
			case 3221:
			case 3222:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 380;
				placeStyle = type - 3215;
				width = 24;
				height = 20;
				value = buyPrice(0, 0, 1);
				return;
			}
			switch (type)
			{
			case 3223:
				width = 22;
				height = 22;
				accessory = true;
				rare = 1;
				value = 100000;
				expert = true;
				return;
			case 3224:
				width = 22;
				height = 22;
				accessory = true;
				rare = 1;
				value = 100000;
				neckSlot = 8;
				expert = true;
				return;
			case 3225:
				width = 14;
				height = 28;
				rare = 1;
				value = sellPrice(0, 2, 50);
				accessory = true;
				balloonSlot = 11;
				return;
			case 3226:
				width = 28;
				height = 20;
				headSlot = 181;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				return;
			case 3227:
				width = 18;
				height = 14;
				bodySlot = 183;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				return;
			case 3228:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 28;
				value = 400000;
				return;
			case 3229:
			case 3230:
			case 3231:
			case 3232:
			case 3233:
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 85;
				placeStyle = 6 + type - 3229;
				width = 20;
				height = 20;
				return;
			}
			switch (type)
			{
			case 3234:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 385;
				width = 12;
				height = 12;
				return;
			case 3235:
			case 3236:
			case 3237:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 33 + type - 3235;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				return;
			}
			switch (type)
			{
			case 3238:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 186;
				width = 12;
				height = 12;
				return;
			case 3239:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 387;
				width = 20;
				height = 12;
				return;
			case 3240:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 388;
				width = 18;
				height = 26;
				return;
			case 3241:
				width = 14;
				height = 28;
				rare = 1;
				value = sellPrice(0, 3);
				accessory = true;
				balloonSlot = 12;
				return;
			case 3242:
				width = 18;
				height = 18;
				value = buyPrice(0, 3);
				vanity = true;
				headSlot = 182;
				return;
			case 3243:
				width = 18;
				height = 18;
				value = buyPrice(0, 3);
				vanity = true;
				bodySlot = 184;
				return;
			case 3244:
				width = 18;
				height = 18;
				value = buyPrice(0, 3);
				vanity = true;
				legSlot = 124;
				return;
			case 3245:
				width = 16;
				height = 16;
				value = sellPrice(0, 2);
				rare = 2;
				handOnSlot = 22;
				handOffSlot = 14;
				accessory = true;
				expert = true;
				return;
			case 3246:
				width = 18;
				height = 18;
				value = buyPrice(0, 3);
				vanity = true;
				bodySlot = 185;
				return;
			case 3247:
				width = 18;
				height = 18;
				value = buyPrice(0, 3);
				vanity = true;
				legSlot = 125;
				return;
			case 3248:
				width = 18;
				height = 18;
				value = buyPrice(0, 3);
				vanity = true;
				headSlot = 183;
				return;
			case 3249:
				mana = 10;
				damage = 40;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 533;
				buffType = 161;
				width = 26;
				height = 28;
				UseSound = SoundID.Item113;
				useAnimation = 36;
				useTime = 36;
				rare = 8;
				noMelee = true;
				knockBack = 2f;
				value = eclipsePostPlanteraPrice;
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				return;
			case 3250:
			case 3251:
			case 3252:
				width = 20;
				height = 22;
				rare = 4;
				value = buyPrice(0, 15);
				accessory = true;
				balloonSlot = (sbyte)(13 + type - 3250);
				return;
			case 3253:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 390;
				width = 12;
				height = 30;
				value = buyPrice(0, 2);
				rare = 1;
				glowMask = 129;
				return;
			case 3254:
			case 3255:
			case 3256:
			case 3257:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 391 + type - 3254;
				width = 12;
				height = 12;
				return;
			}
			switch (type)
			{
			case 3258:
				useStyle = 1;
				useAnimation = 20;
				useTime = 20;
				autoReuse = true;
				knockBack = 20f;
				width = 36;
				height = 36;
				damage = 55;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = buyPrice(0, 25);
				melee = true;
				crit = 15;
				return;
			case 3260:
				useStyle = 4;
				channel = true;
				width = 34;
				height = 34;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				mountType = 10;
				value = sellPrice(0, 5);
				return;
			case 3259:
				width = 20;
				height = 26;
				maxStack = CommonMaxStack;
				rare = 3;
				value = buyPrice(0, 30);
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				consumable = true;
				return;
			case 3261:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 7;
				value = sellPrice(0, 1);
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 21;
				return;
			default:
				if (type < 3315 || type > 3317)
				{
					break;
				}
				goto case 3262;
			case 3262:
			case 3278:
			case 3279:
			case 3280:
			case 3281:
			case 3282:
			case 3283:
			case 3284:
			case 3285:
			case 3286:
			case 3287:
			case 3288:
			case 3289:
			case 3290:
			case 3291:
			case 3292:
				useStyle = 5;
				width = 24;
				height = 24;
				noUseGraphic = true;
				UseSound = SoundID.Item1;
				melee = true;
				channel = true;
				noMelee = true;
				shoot = 541 + type - 3278;
				useAnimation = 25;
				useTime = 25;
				shootSpeed = 16f;
				switch (type)
				{
				case 3278:
					knockBack = 2.5f;
					damage = 9;
					value = sellPrice(0, 0, 1);
					rare = 0;
					break;
				case 3285:
					knockBack = 3.5f;
					damage = 14;
					value = sellPrice(0, 0, 50);
					rare = 1;
					break;
				case 3279:
					knockBack = 4.5f;
					damage = 16;
					value = sellPrice(0, 1);
					rare = 1;
					break;
				case 3280:
					knockBack = 4f;
					damage = 17;
					value = sellPrice(0, 1);
					rare = 1;
					break;
				case 3281:
					knockBack = 3.75f;
					damage = 18;
					value = sellPrice(0, 1, 30);
					rare = 3;
					break;
				case 3317:
					knockBack = 3.85f;
					damage = 28;
					value = dungeonPrice;
					rare = 3;
					shoot = 564;
					break;
				case 3282:
					knockBack = 4.3f;
					damage = 27;
					value = sellPrice(0, 1, 80);
					rare = 3;
					break;
				case 3262:
					knockBack = 3.25f;
					damage = 21;
					value = buyPrice(0, 5);
					rare = 2;
					shoot = 534;
					break;
				case 3315:
					knockBack = 3.25f;
					damage = 39;
					value = sellPrice(0, 4);
					rare = 3;
					shoot = 562;
					break;
				case 3316:
					knockBack = 3.8f;
					damage = 49;
					value = sellPrice(0, 4);
					rare = 3;
					shoot = 563;
					break;
				case 3283:
					knockBack = 3.3f;
					damage = 39;
					value = sellPrice(0, 4);
					rare = 4;
					break;
				case 3289:
					knockBack = 2.8f;
					damage = 43;
					value = sellPrice(0, 4);
					rare = 4;
					break;
				case 3290:
					knockBack = 4.5f;
					damage = 41;
					value = sellPrice(0, 4);
					rare = 4;
					break;
				case 3284:
					knockBack = 3.8f;
					damage = 54;
					value = buyPrice(0, 25);
					rare = 5;
					break;
				case 3286:
					knockBack = 3.1f;
					damage = 60;
					value = sellPrice(0, 5);
					rare = 7;
					break;
				case 3291:
					knockBack = 4.3f;
					damage = 95;
					value = sellPrice(0, 11);
					rare = 8;
					crit += 10;
					break;
				case 3287:
				case 3288:
					knockBack = 4.5f;
					damage = 70;
					rare = 9;
					value = sellPrice(0, 4);
					break;
				case 3292:
					knockBack = 3.5f;
					damage = 115;
					value = eclipseMothronPrice;
					rare = 8;
					break;
				default:
					knockBack = 4f;
					damage = 15;
					rare = 2;
					value = sellPrice(0, 1);
					break;
				}
				return;
			}
			switch (type)
			{
			case 3389:
				useStyle = 5;
				width = 24;
				height = 24;
				noUseGraphic = true;
				UseSound = SoundID.Item1;
				melee = true;
				channel = true;
				noMelee = true;
				shoot = 603;
				useAnimation = 25;
				useTime = 25;
				shootSpeed = 16f;
				damage = 190;
				knockBack = 6.5f;
				value = sellPrice(0, 10);
				crit = 10;
				rare = 10;
				return;
			case 3293:
			case 3294:
			case 3295:
			case 3296:
			case 3297:
			case 3298:
			case 3299:
			case 3300:
			case 3301:
			case 3302:
			case 3303:
			case 3304:
			case 3305:
			case 3306:
			case 3307:
			case 3308:
				width = 24;
				height = 24;
				rare = 1;
				value = sellPrice(0, 0, 3);
				accessory = true;
				switch (type)
				{
				case 3307:
					stringColor = 27;
					break;
				case 3306:
					stringColor = 14;
					break;
				case 3308:
					stringColor = 13;
					break;
				case 3305:
					stringColor = 28;
					break;
				default:
					stringColor = 1 + type - 3293;
					break;
				}
				hasVanityEffects = true;
				return;
			}
			if (type >= 3309 && type <= 3314)
			{
				width = 24;
				height = 24;
				rare = 2;
				value = buyPrice(0, 5);
				accessory = true;
				return;
			}
			switch (type)
			{
			case 3263:
				width = 18;
				height = 18;
				value = buyPrice(0, 3);
				vanity = true;
				headSlot = 184;
				return;
			case 3264:
				width = 18;
				height = 18;
				value = buyPrice(0, 3);
				vanity = true;
				bodySlot = 186;
				return;
			case 3265:
				width = 18;
				height = 18;
				value = buyPrice(0, 3);
				vanity = true;
				legSlot = 126;
				return;
			case 3266:
				width = 18;
				height = 18;
				value = 4500;
				headSlot = 185;
				defense = 4;
				rare = 1;
				return;
			case 3267:
				width = 18;
				height = 18;
				value = 4500;
				bodySlot = 187;
				defense = 6;
				rare = 1;
				return;
			case 3268:
				width = 18;
				height = 18;
				value = 4500;
				legSlot = 127;
				defense = 5;
				rare = 1;
				return;
			case 3269:
				useStyle = 4;
				useAnimation = 20;
				useTime = 20;
				autoReuse = true;
				reuseDelay = 10;
				shootSpeed = 1f;
				knockBack = 2f;
				width = 16;
				height = 16;
				damage = 40;
				UseSound = null;
				shoot = 535;
				mana = 15;
				rare = 4;
				value = sellPrice(0, 4);
				noMelee = true;
				noUseGraphic = true;
				magic = true;
				channel = true;
				return;
			case 3270:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 395;
				width = 28;
				height = 28;
				return;
			case 3272:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 397;
				width = 12;
				height = 12;
				return;
			case 3271:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 396;
				width = 12;
				height = 12;
				return;
			case 3273:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 275;
				width = 12;
				height = 12;
				return;
			case 3344:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 308;
				width = 12;
				height = 12;
				return;
			case 3345:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 309;
				width = 12;
				height = 12;
				return;
			case 3346:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 310;
				width = 12;
				height = 12;
				return;
			case 3340:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 304;
				width = 12;
				height = 12;
				return;
			case 3341:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 305;
				width = 12;
				height = 12;
				return;
			case 3342:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 306;
				width = 12;
				height = 12;
				return;
			case 3343:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 307;
				width = 12;
				height = 12;
				return;
			case 3277:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 401;
				width = 12;
				height = 12;
				return;
			case 3276:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 400;
				width = 12;
				height = 12;
				return;
			case 3275:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 399;
				width = 12;
				height = 12;
				return;
			case 3274:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 398;
				width = 12;
				height = 12;
				return;
			case 3339:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 403;
				width = 12;
				height = 12;
				return;
			case 3338:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 402;
				width = 12;
				height = 12;
				return;
			case 3347:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 404;
				width = 12;
				height = 12;
				return;
			case 3348:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 311;
				width = 12;
				height = 12;
				return;
			case 3318:
			case 3319:
			case 3320:
			case 3321:
			case 3322:
			case 3323:
			case 3324:
			case 3325:
			case 3326:
			case 3327:
			case 3328:
			case 3329:
			case 3330:
			case 3331:
			case 3332:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 24;
				height = 24;
				rare = 1;
				if (type == 3320)
				{
					rare = 2;
				}
				if (type == 3321)
				{
					rare = 2;
				}
				if (type == 3322)
				{
					rare = 3;
				}
				if (type == 3323)
				{
					rare = 3;
				}
				if (type == 3324)
				{
					rare = 4;
				}
				if (type == 3325)
				{
					rare = 5;
				}
				if (type == 3326)
				{
					rare = 5;
				}
				if (type == 3327)
				{
					rare = 5;
				}
				if (type == 3328)
				{
					rare = 6;
				}
				if (type == 3329)
				{
					rare = 7;
				}
				if (type == 3330)
				{
					rare = 7;
				}
				if (type == 3331)
				{
					rare = 8;
				}
				if (type == 3332)
				{
					rare = 8;
				}
				expert = true;
				return;
			}
			switch (type)
			{
			case 3333:
				width = 22;
				height = 22;
				accessory = true;
				rare = 3;
				value = sellPrice(0, 2);
				backSlot = 9;
				expert = true;
				return;
			case 3334:
				width = 22;
				height = 22;
				accessory = true;
				rare = 4;
				value = buyPrice(0, 50);
				handOffSlot = 11;
				handOnSlot = 18;
				return;
			case 3335:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 18;
				height = 18;
				useStyle = 4;
				useTime = 30;
				UseSound = SoundID.Item4;
				useAnimation = 30;
				rare = 4;
				value = sellPrice(0, 2);
				expert = true;
				return;
			case 3336:
				width = 22;
				height = 22;
				accessory = true;
				rare = 8;
				value = sellPrice(0, 4);
				expert = true;
				return;
			case 3337:
				width = 22;
				height = 22;
				accessory = true;
				rare = 8;
				value = sellPrice(0, 5);
				expert = true;
				return;
			case 3353:
				width = 36;
				height = 26;
				mountType = 11;
				rare = 6;
				value = sellPrice(0, 1);
				expert = true;
				return;
			case 3354:
			case 3355:
			case 3356:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 5;
				value = sellPrice(0, 0, 50);
				expert = true;
				return;
			case 3357:
			case 3358:
			case 3359:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 56 + type - 3357;
				rare = 1;
				return;
			case 3360:
				tileWand = 620;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				createTile = 383;
				width = 8;
				height = 10;
				rare = 1;
				value = sellPrice(0, 0, 25);
				return;
			case 3361:
				tileWand = 620;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				createTile = 384;
				width = 8;
				height = 10;
				rare = 1;
				value = sellPrice(0, 0, 25);
				return;
			case 3362:
				width = 28;
				height = 20;
				bodySlot = 188;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 25);
				return;
			case 3363:
				width = 28;
				height = 20;
				legSlot = 128;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 25);
				return;
			case 3364:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 405;
				width = 28;
				height = 28;
				rare = 1;
				return;
			case 3365:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 406;
				width = 28;
				height = 28;
				rare = 1;
				return;
			case 3366:
				width = 24;
				height = 24;
				rare = 4;
				value = buyPrice(0, 50);
				accessory = true;
				return;
			case 3367:
				useStyle = 4;
				channel = true;
				width = 34;
				height = 34;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				mountType = 12;
				value = sellPrice(0, 5);
				expert = true;
				return;
			case 3368:
				width = 14;
				height = 38;
				useAnimation = 25;
				useTime = 15;
				useStyle = 5;
				rare = 9;
				noUseGraphic = true;
				channel = true;
				noMelee = true;
				damage = 25;
				knockBack = 4f;
				autoReuse = false;
				noMelee = true;
				melee = true;
				shoot = 595;
				shootSpeed = 15f;
				value = sellPrice(0, 5);
				return;
			case 3369:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 209;
				placeStyle = 2;
				width = 12;
				height = 12;
				rare = 3;
				value = buyPrice(0, 25);
				return;
			case 3370:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 36;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				return;
			case 3371:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 37;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				return;
			case 3372:
			case 3373:
				width = 28;
				height = 20;
				headSlot = type + 186 - 3372;
				rare = 1;
				value = sellPrice(0, 0, 75);
				vanity = true;
				return;
			}
			switch (type)
			{
			case 3374:
				width = 18;
				height = 18;
				defense = 4;
				headSlot = 188;
				rare = 1;
				value = sellPrice(0, 0, 30);
				return;
			case 3375:
				width = 18;
				height = 18;
				defense = 5;
				bodySlot = 189;
				rare = 1;
				value = sellPrice(0, 0, 50);
				return;
			case 3376:
				width = 18;
				height = 18;
				defense = 4;
				legSlot = 129;
				rare = 1;
				value = sellPrice(0, 0, 40);
				return;
			case 3377:
				mana = 7;
				UseSound = SoundID.Item43;
				useStyle = 5;
				damage = 21;
				useAnimation = 28;
				useTime = 28;
				width = 40;
				height = 40;
				shoot = 597;
				shootSpeed = 9f;
				knockBack = 4.75f;
				magic = true;
				autoReuse = true;
				value = 20000;
				rare = 1;
				noMelee = true;
				return;
			case 3378:
				shoot = 598;
				shootSpeed = 10f;
				damage = 20;
				knockBack = 5f;
				ranged = true;
				useStyle = 1;
				UseSound = SoundID.Item1;
				useAnimation = 25;
				useTime = 25;
				width = 30;
				height = 30;
				maxStack = CommonMaxStack;
				consumable = true;
				noUseGraphic = true;
				noMelee = true;
				autoReuse = true;
				value = 50;
				rare = 1;
				return;
			case 3379:
				autoReuse = true;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 599;
				damage = 14;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 14;
				useTime = 14;
				noUseGraphic = true;
				noMelee = true;
				value = 50;
				knockBack = 1.5f;
				ranged = true;
				rare = 1;
				return;
			case 3380:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 407;
				width = 12;
				height = 12;
				rare = 1;
				return;
			case 3381:
				width = 18;
				height = 18;
				defense = 10;
				headSlot = 189;
				rare = 10;
				value = sellPrice(0, 7);
				return;
			case 3382:
				width = 18;
				height = 18;
				defense = 16;
				bodySlot = 190;
				rare = 10;
				value = sellPrice(0, 7) * 2;
				return;
			case 3383:
				width = 18;
				height = 18;
				defense = 12;
				legSlot = 130;
				rare = 10;
				value = (int)((double)sellPrice(0, 7) * 1.5);
				return;
			case 3384:
				useStyle = 5;
				useAnimation = 20;
				useTime = 20;
				shootSpeed = 24f;
				knockBack = 2f;
				width = 16;
				height = 16;
				UseSound = null;
				shoot = 600;
				rare = 8;
				value = sellPrice(0, 10);
				noMelee = true;
				noUseGraphic = true;
				channel = true;
				autoReuse = true;
				return;
			case 3385:
			case 3386:
			case 3387:
			case 3388:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = -11;
				placeStyle = type - 3385 + 8;
				createTile = 227;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				return;
			}
			if (type >= 3390 && type <= 3452)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 207 + type - 3390;
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				return;
			}
			if (type >= 3453 && type <= 3455)
			{
				width = 12;
				height = 12;
				switch (type)
				{
				case 3453:
					buffType = 179;
					break;
				case 3454:
					buffType = 173;
					break;
				case 3455:
					buffType = 176;
					break;
				}
				return;
			}
			if (type >= 3456 && type <= 3459)
			{
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 20);
				rare = 9;
				return;
			}
			switch (type)
			{
			case 3460:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 408;
				width = 12;
				height = 12;
				rare = 10;
				value = sellPrice(0, 1, 20) / 4;
				return;
			case 3461:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 409;
				rare = 9;
				width = 12;
				height = 12;
				return;
			case 3462:
				SetDefaults3(2772);
				type = 3462;
				glowMask = 174;
				return;
			case 3463:
				SetDefaults3(2773);
				type = 3463;
				shoot = 610;
				glowMask = 175;
				return;
			case 3464:
				SetDefaults3(2774);
				type = 3464;
				shoot = 609;
				glowMask = 176;
				return;
			case 3465:
				SetDefaults3(2775);
				type = 3465;
				glowMask = 177;
				return;
			case 3466:
				SetDefaults3(2776);
				type = 3466;
				glowMask = 178;
				return;
			case 3467:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 10;
				value = sellPrice(0, 1, 20);
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 239;
				placeStyle = 22;
				return;
			case 3468:
			case 3469:
			case 3470:
			case 3471:
				width = 22;
				height = 20;
				accessory = true;
				value = buyPrice(0, 40);
				rare = 10;
				wingSlot = (sbyte)(29 + type - 3468);
				return;
			}
			switch (type)
			{
			case 3472:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 224;
				rare = 9;
				width = 12;
				height = 12;
				return;
			case 3473:
				useStyle = 5;
				useAnimation = 20;
				useTime = 20;
				shootSpeed = 24f;
				knockBack = 2f;
				width = 16;
				height = 16;
				shoot = 611;
				rare = 10;
				value = sellPrice(0, 10);
				noMelee = true;
				noUseGraphic = true;
				channel = true;
				autoReuse = true;
				melee = true;
				damage = 105;
				return;
			case 3474:
				mana = 10;
				damage = 60;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 613;
				width = 26;
				height = 28;
				UseSound = SoundID.Item44;
				useAnimation = 36;
				useTime = 36;
				rare = 10;
				noMelee = true;
				knockBack = 2f;
				buffType = 182;
				value = sellPrice(0, 10);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				return;
			case 3475:
				useStyle = 5;
				useAnimation = 20;
				useTime = 20;
				shootSpeed = 20f;
				knockBack = 2f;
				width = 20;
				height = 12;
				damage = 50;
				UseSound = null;
				shoot = 615;
				rare = 10;
				value = sellPrice(0, 10);
				noMelee = true;
				noUseGraphic = true;
				ranged = true;
				channel = true;
				glowMask = 191;
				useAmmo = AmmoID.Bullet;
				autoReuse = true;
				return;
			case 3476:
				mana = 30;
				damage = 70;
				useStyle = 5;
				shootSpeed = 7f;
				shoot = 617;
				width = 26;
				height = 28;
				UseSound = SoundID.Item117;
				useAnimation = 30;
				useTime = 30;
				autoReuse = true;
				noMelee = true;
				knockBack = 5f;
				rare = 10;
				value = sellPrice(0, 10);
				magic = true;
				glowMask = 194;
				holdStyle = 1;
				return;
			case 3477:
				useStyle = 1;
				shootSpeed = 9f;
				rare = 3;
				damage = 20;
				shoot = 621;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				knockBack = 3f;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 100;
				return;
			case 3478:
				width = 18;
				height = 18;
				headSlot = 190;
				value = 5000;
				vanity = true;
				return;
			case 3479:
				width = 18;
				height = 18;
				bodySlot = 191;
				value = 5000;
				vanity = true;
				return;
			case 3522:
			case 3523:
			case 3524:
			case 3525:
				useTurn = true;
				autoReuse = true;
				useStyle = 1;
				useAnimation = 28;
				useTime = 7;
				knockBack = 7f;
				width = 42;
				height = 42;
				damage = 60;
				axe = 30;
				hammer = 100;
				UseSound = SoundID.Item1;
				rare = 10;
				value = sellPrice(0, 5);
				melee = true;
				tileBoost += 4;
				switch (type)
				{
				case 3523:
					glowMask = 196;
					break;
				case 3524:
					glowMask = 197;
					break;
				case 3525:
					glowMask = 198;
					break;
				case 3522:
					break;
				}
				return;
			}
			switch (type)
			{
			case 3521:
				SetDefaults1(1);
				this.type = type;
				useTime = 17;
				pick = 55;
				useAnimation = 20;
				scale = 1.05f;
				damage = 6;
				value = 10000;
				return;
			case 3520:
				SetDefaults1(4);
				this.type = type;
				useAnimation = 18;
				damage = 15;
				knockBack = 6.5f;
				scale = 1.25f;
				value = 9000;
				return;
			case 3519:
				SetDefaults1(6);
				this.type = type;
				damage = 12;
				useAnimation = 11;
				useTime = 11;
				knockBack = 5f;
				shoot = 944;
				scale = 0.95f;
				value = 7000;
				return;
			case 3517:
				SetDefaults1(7);
				this.type = type;
				useAnimation = 28;
				useTime = 23;
				scale = 1.25f;
				damage = 9;
				hammer = 55;
				value = 8000;
				return;
			case 3518:
				SetDefaults1(10);
				this.type = type;
				useTime = 18;
				axe = 11;
				useAnimation = 26;
				scale = 1.15f;
				damage = 7;
				value = 8000;
				return;
			case 3516:
				SetDefaults1(99);
				this.type = type;
				useAnimation = 26;
				useTime = 26;
				damage = 11;
				value = 7000;
				return;
			case 3515:
				SetDefaults1(1);
				this.type = type;
				useTime = 11;
				pick = 45;
				useAnimation = 19;
				scale = 1.05f;
				damage = 6;
				value = 5000;
				return;
			case 3514:
				SetDefaults1(4);
				this.type = type;
				useAnimation = 20;
				damage = 14;
				value = 4500;
				knockBack = 6f;
				scale = 1.2f;
				return;
			case 3513:
				SetDefaults1(6);
				this.type = type;
				damage = 9;
				useAnimation = 12;
				useTime = 12;
				knockBack = 4f;
				shoot = 942;
				scale = 0.95f;
				value = 3500;
				return;
			case 3511:
				SetDefaults1(7);
				this.type = type;
				useAnimation = 29;
				useTime = 19;
				scale = 1.25f;
				damage = 9;
				hammer = 45;
				value = 4000;
				return;
			case 3512:
				SetDefaults1(10);
				this.type = type;
				useTime = 18;
				axe = 10;
				useAnimation = 26;
				scale = 1.15f;
				damage = 6;
				value = 4000;
				return;
			case 3510:
				SetDefaults1(99);
				this.type = type;
				useAnimation = 27;
				useTime = 27;
				damage = 9;
				value = 3500;
				return;
			case 3509:
				SetDefaults1(1);
				this.type = type;
				useTime = 15;
				pick = 35;
				useAnimation = 23;
				damage = 4;
				scale = 0.9f;
				tileBoost = -1;
				value = 500;
				return;
			case 3508:
				SetDefaults1(4);
				this.type = type;
				useAnimation = 21;
				damage = 9;
				value = 450;
				scale = 1.1f;
				return;
			case 3507:
				SetDefaults1(6);
				this.type = type;
				damage = 5;
				useAnimation = 13;
				useTime = 13;
				knockBack = 4f;
				shoot = 938;
				scale = 0.8f;
				value = 350;
				return;
			case 3505:
				SetDefaults1(7);
				this.type = type;
				useAnimation = 33;
				useTime = 23;
				scale = 1.1f;
				damage = 4;
				hammer = 35;
				tileBoost = -1;
				value = 400;
				return;
			case 3506:
				SetDefaults1(10);
				this.type = type;
				useTime = 21;
				axe = 7;
				useAnimation = 30;
				scale = 1f;
				damage = 3;
				tileBoost = -1;
				value = 400;
				return;
			case 3504:
				SetDefaults1(99);
				this.type = type;
				useAnimation = 29;
				useTime = 29;
				damage = 6;
				value = 350;
				return;
			case 3503:
				SetDefaults1(1);
				this.type = type;
				useTime = 14;
				pick = 35;
				useAnimation = 21;
				damage = 5;
				scale = 0.95f;
				value = 750;
				return;
			case 3502:
				SetDefaults1(4);
				this.type = type;
				useAnimation = 20;
				damage = 10;
				value = 675;
				scale = 1.1f;
				return;
			case 3501:
				SetDefaults1(6);
				this.type = type;
				damage = 7;
				useAnimation = 12;
				useTime = 12;
				knockBack = 4f;
				shoot = 939;
				scale = 0.85f;
				value = 525;
				return;
			case 3499:
				SetDefaults1(7);
				this.type = type;
				useAnimation = 31;
				useTime = 21;
				scale = 1.15f;
				damage = 6;
				hammer = 38;
				value = 600;
				return;
			case 3500:
				SetDefaults1(10);
				this.type = type;
				useTime = 20;
				axe = 8;
				useAnimation = 28;
				scale = 1.05f;
				damage = 4;
				value = 600;
				return;
			case 3498:
				SetDefaults1(99);
				this.type = type;
				useAnimation = 28;
				useTime = 28;
				damage = 7;
				value = 525;
				return;
			case 3497:
				SetDefaults1(1);
				this.type = type;
				useTime = 12;
				pick = 43;
				useAnimation = 19;
				damage = 6;
				scale = 1.025f;
				value = 3000;
				return;
			case 3496:
				SetDefaults1(4);
				this.type = type;
				useAnimation = 20;
				damage = 13;
				value = 2700;
				scale = 1.15f;
				knockBack = 5.5f;
				return;
			case 3495:
				SetDefaults1(6);
				this.type = type;
				damage = 9;
				useAnimation = 12;
				useTime = 12;
				knockBack = 4f;
				shoot = 941;
				scale = 0.925f;
				value = 2100;
				return;
			case 3493:
				SetDefaults1(7);
				this.type = type;
				useAnimation = 29;
				useTime = 19;
				scale = 1.225f;
				damage = 8;
				hammer = 43;
				value = 2400;
				return;
			case 3494:
				SetDefaults1(10);
				this.type = type;
				useTime = 19;
				axe = 10;
				useAnimation = 28;
				scale = 1.125f;
				damage = 6;
				value = 2400;
				return;
			case 3492:
				SetDefaults1(99);
				this.type = type;
				useAnimation = 27;
				useTime = 27;
				damage = 9;
				value = 2100;
				return;
			case 3491:
				SetDefaults1(1);
				this.type = type;
				useTime = 19;
				pick = 50;
				useAnimation = 21;
				scale = 1.05f;
				damage = 6;
				value = 7500;
				return;
			case 3490:
				SetDefaults1(4);
				this.type = type;
				useAnimation = 19;
				damage = 14;
				knockBack = 6f;
				scale = 1.2f;
				value = 6750;
				return;
			case 3489:
				SetDefaults1(6);
				this.type = type;
				damage = 10;
				useAnimation = 11;
				useTime = 11;
				knockBack = 4f;
				shoot = 943;
				scale = 0.95f;
				value = 5250;
				return;
			case 3487:
				SetDefaults1(7);
				this.type = type;
				useAnimation = 28;
				useTime = 25;
				scale = 1.25f;
				damage = 9;
				hammer = 50;
				value = 6000;
				return;
			case 3488:
				SetDefaults1(10);
				this.type = type;
				useTime = 18;
				axe = 11;
				useAnimation = 26;
				scale = 1.15f;
				damage = 7;
				value = 6000;
				return;
			case 3486:
				SetDefaults1(99);
				this.type = type;
				useAnimation = 26;
				useTime = 26;
				damage = 10;
				value = 5250;
				return;
			case 3485:
				SetDefaults1(1);
				this.type = type;
				useTime = 15;
				pick = 59;
				useAnimation = 19;
				scale = 1.05f;
				damage = 7;
				value = 15000;
				return;
			case 3484:
				SetDefaults1(4);
				this.type = type;
				useAnimation = 17;
				damage = 16;
				knockBack = 6.5f;
				scale = 1.25f;
				value = 13500;
				return;
			case 3483:
				SetDefaults1(6);
				this.type = type;
				damage = 13;
				useAnimation = 10;
				useTime = 10;
				knockBack = 5f;
				shoot = 945;
				scale = 0.975f;
				value = 10500;
				return;
			case 3481:
				SetDefaults1(7);
				this.type = type;
				useAnimation = 27;
				useTime = 21;
				scale = 1.275f;
				damage = 10;
				hammer = 59;
				value = 12000;
				return;
			case 3482:
				SetDefaults1(10);
				this.type = type;
				useTime = 17;
				axe = 12;
				useAnimation = 25;
				scale = 1.175f;
				damage = 8;
				value = 12000;
				return;
			case 3480:
				SetDefaults1(99);
				this.type = type;
				useAnimation = 25;
				useTime = 25;
				damage = 13;
				value = 10500;
				return;
			case 3526:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 2, 50);
				rare = 4;
				return;
			case 3527:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 2, 50);
				rare = 4;
				return;
			case 3528:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 2, 50);
				rare = 4;
				return;
			case 3529:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 2, 50);
				rare = 4;
				return;
			case 3530:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 2, 50);
				rare = 4;
				return;
			case 3531:
				mana = 10;
				damage = 40;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 625;
				width = 26;
				height = 28;
				UseSound = SoundID.Item44;
				useAnimation = 36;
				useTime = 36;
				rare = 10;
				noMelee = true;
				knockBack = 2f;
				buffType = 188;
				value = sellPrice(0, 10);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				return;
			case 3540:
				useStyle = 5;
				useAnimation = 12;
				useTime = 12;
				shootSpeed = 20f;
				knockBack = 2f;
				width = 20;
				height = 12;
				damage = 50;
				UseSound = SoundID.Item5;
				shoot = 630;
				rare = 10;
				value = sellPrice(0, 10);
				noMelee = true;
				noUseGraphic = true;
				ranged = true;
				channel = true;
				glowMask = 200;
				useAmmo = AmmoID.Arrow;
				autoReuse = true;
				return;
			case 3541:
				useStyle = 5;
				useAnimation = 10;
				useTime = 10;
				reuseDelay = 5;
				shootSpeed = 30f;
				knockBack = 0.25f;
				width = 16;
				height = 16;
				damage = 100;
				UseSound = null;
				shoot = 633;
				mana = 12;
				rare = 10;
				value = sellPrice(0, 10);
				noMelee = true;
				noUseGraphic = true;
				magic = true;
				channel = true;
				return;
			case 3533:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3534:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3535:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3536:
				width = 22;
				height = 32;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 410;
				placeStyle = 0;
				rare = 9;
				accessory = true;
				vanity = true;
				value = buyPrice(1);
				return;
			case 3537:
				width = 22;
				height = 32;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 410;
				placeStyle = 1;
				rare = 9;
				accessory = true;
				vanity = true;
				value = buyPrice(1);
				return;
			case 3538:
				width = 22;
				height = 32;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 410;
				placeStyle = 2;
				rare = 9;
				accessory = true;
				vanity = true;
				value = buyPrice(1);
				return;
			case 3539:
				width = 22;
				height = 32;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 410;
				placeStyle = 3;
				rare = 9;
				accessory = true;
				vanity = true;
				value = buyPrice(1);
				return;
			case 3542:
				useStyle = 5;
				useAnimation = 12;
				useTime = 12;
				shootSpeed = 6f;
				knockBack = 3f;
				width = 16;
				height = 16;
				damage = 130;
				UseSound = SoundID.Item20;
				shoot = 634;
				mana = 12;
				rare = 10;
				value = sellPrice(0, 10);
				noMelee = true;
				magic = true;
				autoReuse = true;
				noUseGraphic = true;
				glowMask = 207;
				return;
			case 3543:
				shoot = 636;
				shootSpeed = 10f;
				damage = 150;
				knockBack = 5f;
				melee = true;
				useStyle = 1;
				UseSound = SoundID.Item1;
				useAnimation = 16;
				useTime = 16;
				width = 30;
				height = 30;
				noUseGraphic = true;
				noMelee = true;
				autoReuse = true;
				value = sellPrice(0, 10);
				rare = 10;
				return;
			case 3544:
				UseSound = SoundID.Item3;
				healLife = 200;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				potion = true;
				width = 14;
				height = 24;
				rare = 7;
				value = sellPrice(0, 0, 30);
				return;
			case 3545:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 411;
				width = 28;
				height = 28;
				rare = 1;
				mech = true;
				value = sellPrice(0, 0, 20);
				return;
			case 3547:
				useStyle = 1;
				shootSpeed = 4f;
				shoot = 637;
				width = 8;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 40;
				useTime = 40;
				noUseGraphic = true;
				noMelee = true;
				value = buyPrice(0, 0, 20);
				rare = 1;
				return;
			case 3546:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 30;
				useTime = 30;
				useAmmo = AmmoID.Rocket;
				width = 50;
				height = 20;
				shoot = 134;
				UseSound = SoundID.Item156;
				damage = 25;
				shootSpeed = 15f;
				noMelee = true;
				value = buyPrice(0, 80);
				knockBack = 4f;
				rare = 8;
				ranged = true;
				return;
			case 3350:
				useStyle = 5;
				useAnimation = 24;
				useTime = 9;
				width = 24;
				height = 14;
				shoot = 587;
				UseSound = null;
				damage = 12;
				shootSpeed = 10f;
				noMelee = true;
				value = sellPrice(0, 0, 50);
				knockBack = 1.25f;
				scale = 0.85f;
				rare = 2;
				ranged = true;
				crit = 7;
				return;
			case 3352:
				SetDefaults1(4);
				this.type = type;
				useAnimation = 12;
				useTime = 12;
				damage = 14;
				width = (height = 32);
				knockBack = 5f;
				rare = 2;
				value = sellPrice(0, 0, 50);
				return;
			case 3351:
				SetDefaults1(4);
				this.type = type;
				useAnimation = 15;
				useTime = 15;
				damage = 16;
				width = (height = 28);
				knockBack = 3.5f;
				rare = 2;
				value = sellPrice(0, 0, 50);
				return;
			case 3349:
				SetDefaults1(4);
				this.type = type;
				useAnimation = 18;
				useTime = 18;
				damage = 20;
				width = (height = 32);
				knockBack = 4.25f;
				rare = 2;
				value = sellPrice(0, 0, 50);
				return;
			case 3548:
				useStyle = 5;
				shootSpeed = 6f;
				shoot = 588;
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				noMelee = true;
				value = sellPrice(0, 0, 0, 50);
				damage = 30;
				knockBack = 6f;
				rare = 2;
				ranged = true;
				return;
			case 3549:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 412;
				width = 28;
				height = 28;
				SetShopValues(ItemRarityColor.StrongRed10, sellPrice(0, 5));
				return;
			case 3563:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				makeNPC = 538;
				return;
			case 3564:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				makeNPC = 539;
				noUseGraphic = true;
				value = sellPrice(0, 10);
				rare = 3;
				return;
			case 3565:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 413;
				width = 12;
				height = 12;
				return;
			case 3566:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 414;
				width = 12;
				height = 12;
				value = sellPrice(0, 10);
				rare = 3;
				return;
			case 3550:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 3551:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 3552:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 3553:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3554:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3555:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 75);
				rare = 2;
				return;
			case 3556:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3557:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 3558:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 3559:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 1;
				return;
			case 3560:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = 10000;
				rare = 2;
				return;
			case 3561:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3562:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				return;
			case 3567:
				shootSpeed = 2f;
				shoot = 638;
				damage = 20;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 3f;
				value = 7;
				ranged = true;
				rare = 9;
				value = sellPrice(0, 0, 0, 2);
				return;
			case 3568:
				shootSpeed = 3f;
				shoot = 639;
				damage = 15;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 3.5f;
				value = 5;
				ranged = true;
				rare = 9;
				value = sellPrice(0, 0, 0, 2);
				return;
			case 3569:
				mana = 10;
				damage = 100;
				useStyle = 1;
				shootSpeed = 14f;
				shoot = 641;
				width = 18;
				height = 20;
				UseSound = SoundID.Item78;
				useAnimation = 30;
				useTime = 30;
				noMelee = true;
				value = sellPrice(0, 10);
				knockBack = 7.5f;
				rare = 10;
				summon = true;
				sentry = true;
				return;
			case 3571:
				mana = 10;
				damage = 130;
				useStyle = 1;
				shootSpeed = 14f;
				shoot = 643;
				width = 18;
				height = 20;
				UseSound = SoundID.Item78;
				useAnimation = 30;
				useTime = 30;
				noMelee = true;
				value = sellPrice(0, 10);
				knockBack = 7.5f;
				rare = 10;
				summon = true;
				sentry = true;
				return;
			case 3570:
				autoReuse = true;
				mana = 9;
				useStyle = 5;
				damage = 100;
				useAnimation = 10;
				useTime = 10;
				width = 40;
				height = 40;
				shoot = 645;
				shootSpeed = 10f;
				knockBack = 4.5f;
				value = sellPrice(0, 10);
				magic = true;
				rare = 10;
				noMelee = true;
				UseSound = SoundID.Item88;
				return;
			case 3572:
				noUseGraphic = true;
				damage = 0;
				useStyle = 5;
				shootSpeed = 18f;
				shoot = 646;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 10;
				noMelee = true;
				value = sellPrice(0, 10);
				return;
			case 3573:
			case 3574:
			case 3575:
			case 3576:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 415 + type - 3573;
				width = 12;
				height = 12;
				return;
			}
			switch (type)
			{
			case 3577:
				channel = true;
				damage = 0;
				useStyle = 4;
				shoot = 650;
				width = 24;
				height = 24;
				UseSound = SoundID.Item8;
				useAnimation = 20;
				useTime = 20;
				rare = 10;
				noMelee = true;
				value = sellPrice(0, 10);
				buffType = 190;
				expert = true;
				return;
			case 3578:
				width = 28;
				height = 20;
				bodySlot = 192;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				return;
			case 3579:
				width = 18;
				height = 14;
				legSlot = 132;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				return;
			case 3580:
				width = 18;
				height = 14;
				wingSlot = 33;
				rare = 9;
				accessory = true;
				value = 400000;
				return;
			case 3581:
				width = 18;
				height = 14;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				accessory = true;
				hasVanityEffects = true;
				return;
			case 3582:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 34;
				value = 400000;
				return;
			case 3583:
				width = 28;
				height = 20;
				headSlot = 191;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				return;
			case 3584:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 60;
				width = 12;
				height = 12;
				return;
			case 3585:
				width = 28;
				height = 20;
				headSlot = 192;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				return;
			case 3586:
				width = 28;
				height = 20;
				bodySlot = 193;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				return;
			case 3587:
				width = 18;
				height = 14;
				legSlot = 133;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				return;
			case 3588:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 35;
				value = 400000;
				return;
			case 3589:
				width = 28;
				height = 20;
				headSlot = 193;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				return;
			case 3590:
				width = 28;
				height = 20;
				bodySlot = 194;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				return;
			case 3591:
				width = 18;
				height = 14;
				legSlot = 134;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				return;
			case 3592:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 36;
				value = 400000;
				return;
			case 3593:
			case 3594:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 270 + type - 3593;
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				return;
			}
			switch (type)
			{
			case 3595:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 59;
				rare = 1;
				return;
			case 3596:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = buyPrice(0, 3);
				placeStyle = 36;
				return;
			case 3601:
				useStyle = 4;
				width = 22;
				height = 14;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				maxStack = CommonMaxStack;
				rare = 10;
				return;
			case 3602:
				createTile = 419;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				mech = true;
				value = buyPrice(0, 0, 10);
				return;
			case 3603:
			case 3604:
			case 3605:
			case 3606:
			case 3607:
			case 3608:
				createTile = 420;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				placeStyle = type - 3603;
				mech = true;
				value = buyPrice(0, 2);
				return;
			}
			switch (type)
			{
			case 3609:
				createTile = 421;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				value = buyPrice(0, 0, 5);
				return;
			case 3610:
				createTile = 422;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				value = buyPrice(0, 0, 5);
				return;
			case 3611:
				useStyle = 5;
				useAnimation = 10;
				useTime = 10;
				width = 20;
				height = 20;
				shoot = 651;
				channel = true;
				shootSpeed = 10f;
				value = sellPrice(0, 4);
				rare = 2;
				UseSound = SoundID.Item64;
				mech = true;
				return;
			case 3612:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 5;
				autoReuse = true;
				width = 24;
				height = 28;
				rare = 1;
				value = 20000;
				tileBoost = 20;
				mech = true;
				return;
			case 3613:
			case 3614:
			case 3615:
				createTile = 423;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				placeStyle = type - 3613;
				mech = true;
				return;
			}
			switch (type)
			{
			case 3616:
				createTile = 424;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				mech = true;
				value = buyPrice(0, 0, 2);
				return;
			case 3617:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 425;
				width = 28;
				height = 28;
				mech = true;
				return;
			case 3618:
				createTile = 419;
				placeStyle = 1;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				mech = true;
				value = buyPrice(0, 0, 10);
				return;
			case 3619:
				width = 24;
				height = 28;
				rare = 3;
				value = buyPrice(0, 1);
				accessory = true;
				return;
			case 3620:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 5;
				autoReuse = true;
				width = 24;
				height = 28;
				rare = 1;
				value = 20000;
				tileBoost = 20;
				mech = true;
				return;
			case 3621:
				createTile = 426;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				value = buyPrice(0, 0, 1);
				return;
			case 3622:
				createTile = 427;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				value = buyPrice(0, 0, 1);
				return;
			case 3623:
				noUseGraphic = true;
				damage = 0;
				useStyle = 5;
				shootSpeed = 16f;
				shoot = 652;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 10;
				noMelee = true;
				value = sellPrice(0, 10);
				return;
			case 3624:
				width = 30;
				height = 30;
				accessory = true;
				rare = 3;
				value = buyPrice(0, 10);
				return;
			case 3625:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 5;
				autoReuse = true;
				width = 24;
				height = 28;
				rare = 1;
				value = buyPrice(0, 12);
				tileBoost = 20;
				mech = true;
				return;
			case 3626:
				createTile = 428;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				placeStyle = 3;
				mech = true;
				return;
			case 3627:
				width = 18;
				height = 18;
				headSlot = 194;
				value = buyPrice(0, 1);
				vanity = true;
				return;
			case 3628:
				channel = true;
				damage = 0;
				useStyle = 4;
				shoot = 653;
				width = 24;
				height = 24;
				UseSound = SoundID.Item8;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = buyPrice(5);
				buffType = 191;
				return;
			case 3629:
				createTile = 429;
				width = 16;
				height = 16;
				rare = 2;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				mech = true;
				value = buyPrice(0, 5);
				return;
			case 3630:
				createTile = 428;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				placeStyle = 0;
				mech = true;
				return;
			case 3631:
				createTile = 428;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				placeStyle = 2;
				mech = true;
				return;
			case 3632:
				createTile = 428;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				placeStyle = 1;
				mech = true;
				return;
			case 3633:
			case 3634:
			case 3635:
			case 3636:
			case 3637:
				createTile = 430 + (type - 3633);
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				value = buyPrice(0, 0, 1);
				return;
			}
			if (type >= 3638 && type <= 3642)
			{
				createTile = 435 + (type - 3638);
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				value = buyPrice(0, 0, 1);
				return;
			}
			switch (type)
			{
			case 3643:
				width = 20;
				height = 20;
				rare = 1;
				return;
			case 3644:
			case 3645:
			case 3646:
			case 3647:
			case 3648:
			case 3649:
			case 3650:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 440;
				placeStyle = type - 3644;
				width = 22;
				height = 22;
				value = sellPrice(0, 0, 1);
				return;
			}
			if (type >= 3651 && type <= 3662)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 51 + type - 3651;
				return;
			}
			switch (type)
			{
			case 3663:
				createTile = 419;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				mech = true;
				placeStyle = 2;
				value = buyPrice(0, 2);
				return;
			case 3664:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 209;
				placeStyle = 3;
				width = 12;
				height = 12;
				rare = 3;
				value = buyPrice(0, 10);
				return;
			case 3665:
			case 3666:
			case 3667:
			case 3668:
			case 3669:
			case 3670:
			case 3671:
			case 3672:
			case 3673:
			case 3674:
			case 3675:
			case 3676:
			case 3677:
			case 3678:
			case 3679:
			case 3680:
			case 3681:
			case 3682:
			case 3683:
			case 3684:
			case 3685:
			case 3686:
			case 3687:
			case 3688:
			case 3689:
			case 3690:
			case 3691:
			case 3692:
			case 3693:
			case 3694:
			case 3695:
			case 3696:
			case 3697:
			case 3698:
			case 3699:
			case 3700:
			case 3701:
			case 3702:
			case 3703:
			case 3704:
			case 3705:
			case 3706:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 441;
				placeStyle = type - 3665 + (type > 3666).ToInt() + (type > 3667).ToInt() * 3 + (type > 3683).ToInt() * 5 + (type > 3691).ToInt() + (type > 3692).ToInt() + (type > 3693).ToInt();
				width = 26;
				height = 22;
				value = 500;
				return;
			}
			switch (type)
			{
			case 3707:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 442;
				width = 12;
				height = 12;
				placeStyle = 0;
				mech = true;
				value = buyPrice(0, 2);
				mech = true;
				return;
			case 3708:
			case 3709:
			case 3710:
			case 3711:
			case 3712:
			case 3713:
			case 3714:
			case 3715:
			case 3716:
			case 3717:
			case 3718:
			case 3719:
			case 3720:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 105;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 63 + type - 3708;
				return;
			}
			switch (type)
			{
			case 3721:
				width = 26;
				height = 30;
				maxStack = 1;
				value = sellPrice(0, 3);
				rare = 3;
				accessory = true;
				backSlot = 10;
				return;
			case 3722:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 443;
				width = 20;
				height = 12;
				value = 10000;
				mech = true;
				return;
			case 3723:
			case 3724:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 215;
				placeStyle = 6 + type - 3723;
				width = 12;
				height = 12;
				return;
			}
			switch (type)
			{
			case 3725:
				createTile = 445;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				mech = true;
				value = buyPrice(0, 0, 2);
				return;
			case 3726:
			case 3727:
			case 3728:
			case 3729:
				createTile = 423;
				width = 16;
				height = 16;
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				placeStyle = type - 3726 + 3;
				mech = true;
				return;
			}
			switch (type)
			{
			case 3730:
			case 3731:
				width = 20;
				height = 22;
				rare = 1;
				value = buyPrice(0, 2);
				accessory = true;
				vanity = true;
				balloonSlot = (sbyte)(16 + type - 3730);
				return;
			case 3732:
				width = 18;
				height = 18;
				headSlot = 195;
				value = buyPrice(0, 1);
				vanity = true;
				return;
			case 3733:
				width = 18;
				height = 18;
				headSlot = 196;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 3734:
				width = 28;
				height = 20;
				bodySlot = 195;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 3735:
				width = 18;
				height = 14;
				legSlot = 138;
				value = buyPrice(0, 3);
				vanity = true;
				return;
			case 3736:
			case 3737:
			case 3738:
				createTile = 446 + (type - 3736);
				width = 16;
				height = 16;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				value = buyPrice(0, 0, 1);
				return;
			}
			if (type >= 3739 && type <= 3741)
			{
				createTile = 449 + (type - 3739);
				width = 16;
				height = 16;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				value = buyPrice(0, 0, 0, 50);
				tileBoost += 3;
				return;
			}
			switch (type)
			{
			case 3742:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 452;
				width = 26;
				height = 20;
				value = buyPrice(0, 5);
				rare = 1;
				return;
			case 3743:
			case 3744:
			case 3745:
				createTile = 453;
				placeStyle = type - 3743;
				if (3744 == type)
				{
					placeStyle = 0;
				}
				if (3745 == type)
				{
					placeStyle = 2;
				}
				if (3743 == type)
				{
					placeStyle = 4;
				}
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 30;
				value = buyPrice(0, 0, 10);
				return;
			}
			switch (type)
			{
			case 3746:
				createTile = 454;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 30;
				value = buyPrice(0, 1);
				return;
			case 3747:
				createTile = 455;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 30;
				value = buyPrice(0, 20);
				rare = 3;
				return;
			case 3748:
				createTile = 456;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 30;
				value = buyPrice(0, 0, 20);
				return;
			case 3749:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 457;
				width = 26;
				height = 20;
				value = buyPrice(0, 0, 20);
				rare = 1;
				return;
			case 3750:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 621;
				width = 22;
				height = 22;
				value = sellPrice(0, 0, 50);
				rare = 3;
				return;
			case 3751:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 225;
				width = 12;
				height = 12;
				return;
			case 3752:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 226;
				width = 12;
				height = 12;
				return;
			case 3753:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 227;
				width = 12;
				height = 12;
				return;
			case 3754:
				createTile = 458;
				width = 16;
				height = 16;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				value = buyPrice(0, 0, 0, 5);
				return;
			case 3755:
				createTile = 459;
				width = 16;
				height = 16;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				return;
			case 3756:
				createTile = 460;
				width = 16;
				height = 16;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				return;
			case 3757:
				width = 18;
				height = 18;
				headSlot = 197;
				value = sellPrice(0, 0, 30);
				vanity = true;
				rare = 9;
				return;
			case 3758:
				width = 28;
				height = 20;
				bodySlot = 196;
				value = sellPrice(0, 0, 30);
				vanity = true;
				rare = 9;
				return;
			case 3759:
				width = 18;
				height = 14;
				legSlot = 139;
				value = sellPrice(0, 0, 30);
				vanity = true;
				rare = 9;
				return;
			case 3760:
			case 3761:
			case 3762:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 228 + (type - 3760);
				width = 12;
				height = 12;
				return;
			}
			switch (type)
			{
			case 3763:
				width = 18;
				height = 18;
				headSlot = 198;
				value = sellPrice(0, 1);
				vanity = true;
				rare = 9;
				return;
			case 3764:
			case 3765:
			case 3766:
			case 3767:
			case 3768:
			case 3769:
				SetDefaults(198);
				this.type = type;
				damage = 48;
				useTime = 16;
				useAnimation = 16;
				scale = 1.15f;
				autoReuse = true;
				useTurn = true;
				rare = 4;
				value = sellPrice(0, 1);
				return;
			}
			switch (type)
			{
			case 3770:
				width = 18;
				height = 14;
				legSlot = 140;
				value = sellPrice(0, 1);
				rare = 4;
				return;
			case 3771:
				useStyle = 4;
				channel = true;
				width = 34;
				height = 34;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				mountType = 14;
				value = sellPrice(0, 5);
				return;
			case 3772:
				useStyle = 1;
				useTurn = true;
				autoReuse = true;
				useAnimation = 18;
				useTime = 18;
				width = 28;
				height = 28;
				damage = 16;
				knockBack = 4.5f;
				UseSound = SoundID.Item1;
				scale = 1.1f;
				melee = true;
				value = sellPrice(0, 0, 10);
				rare = 2;
				return;
			case 3773:
				width = 18;
				height = 18;
				headSlot = 199;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				return;
			case 3774:
				width = 18;
				height = 18;
				bodySlot = 197;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				return;
			case 3775:
				width = 18;
				height = 18;
				legSlot = 141;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				return;
			case 3776:
				width = 18;
				height = 18;
				defense = 6;
				headSlot = 200;
				rare = 5;
				value = 250000;
				return;
			case 3777:
				width = 18;
				height = 18;
				defense = 12;
				bodySlot = 198;
				rare = 5;
				value = 200000;
				return;
			case 3778:
				width = 18;
				height = 18;
				defense = 8;
				legSlot = 142;
				rare = 5;
				value = 150000;
				return;
			case 3779:
				mana = 14;
				damage = 85;
				useStyle = 5;
				shootSpeed = 3f;
				shoot = 659;
				width = 26;
				height = 28;
				UseSound = SoundID.Item117;
				useAnimation = 22;
				useTime = 22;
				autoReuse = true;
				noMelee = true;
				knockBack = 5f;
				rare = 4;
				value = sellPrice(0, 1);
				magic = true;
				glowMask = 218;
				return;
			case 3780:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 272;
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				return;
			case 3781:
				width = 24;
				height = 28;
				rare = 3;
				value = 100000;
				accessory = true;
				return;
			case 3784:
				width = 18;
				height = 18;
				legSlot = 143;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				return;
			case 3785:
				width = 18;
				height = 18;
				bodySlot = 199;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				return;
			case 3786:
				width = 18;
				height = 18;
				headSlot = 201;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				return;
			case 3782:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 461;
				width = 24;
				height = 24;
				value = sellPrice(0, 0, 0, 40);
				return;
			case 3787:
				useStyle = 5;
				useAnimation = 12;
				useTime = 4;
				reuseDelay = useAnimation + 6;
				shootSpeed = 14f;
				knockBack = 6f;
				width = 16;
				height = 16;
				damage = 38;
				UseSound = SoundID.Item9;
				crit = 20;
				shoot = 660;
				mana = 17;
				rare = 4;
				value = 300000;
				noMelee = true;
				magic = true;
				autoReuse = true;
				return;
			case 3788:
				knockBack = 6.5f;
				useStyle = 5;
				useAnimation = 48;
				useTime = 48;
				width = 50;
				height = 14;
				shoot = 10;
				useAmmo = AmmoID.Bullet;
				UseSound = SoundID.Item36;
				damage = 24;
				shootSpeed = 7f;
				noMelee = true;
				value = 250000;
				rare = 4;
				ranged = true;
				return;
			case 3783:
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				value = 50000;
				rare = 5;
				return;
			case 3789:
			case 3790:
			case 3791:
			case 3792:
			case 3793:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 273 + type - 3789;
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				return;
			}
			switch (type)
			{
			case 3794:
				width = 18;
				height = 18;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 1);
				rare = 1;
				return;
			case 3795:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 462;
				width = 26;
				height = 18;
				value = sellPrice(0, 0, 50);
				rare = 3;
				return;
			case 3796:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 38;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				return;
			case 3797:
				width = 18;
				height = 18;
				headSlot = 203;
				rare = 8;
				defense = 7;
				value = sellPrice(0, 3);
				return;
			case 3798:
				width = 18;
				height = 18;
				bodySlot = 200;
				rare = 8;
				defense = 15;
				value = sellPrice(0, 3);
				return;
			case 3799:
				width = 18;
				height = 18;
				legSlot = 144;
				rare = 8;
				defense = 10;
				value = sellPrice(0, 3);
				return;
			case 3800:
				width = 18;
				height = 18;
				headSlot = 204;
				rare = 8;
				defense = 13;
				value = sellPrice(0, 3);
				return;
			case 3801:
				width = 18;
				height = 18;
				bodySlot = 201;
				rare = 8;
				defense = 27;
				value = sellPrice(0, 3);
				return;
			case 3802:
				width = 18;
				height = 18;
				legSlot = 145;
				rare = 8;
				defense = 18;
				value = sellPrice(0, 3);
				return;
			case 3803:
				width = 18;
				height = 18;
				headSlot = 205;
				rare = 8;
				defense = 7;
				value = sellPrice(0, 3);
				return;
			case 3804:
				width = 18;
				height = 18;
				bodySlot = 202;
				rare = 8;
				defense = 17;
				value = sellPrice(0, 3);
				return;
			case 3805:
				width = 18;
				height = 18;
				legSlot = 146;
				rare = 8;
				defense = 12;
				value = sellPrice(0, 3);
				return;
			case 3806:
				width = 18;
				height = 18;
				headSlot = 206;
				rare = 8;
				defense = 8;
				value = sellPrice(0, 3);
				return;
			case 3807:
				width = 18;
				height = 18;
				bodySlot = 203;
				rare = 8;
				defense = 22;
				value = sellPrice(0, 3);
				return;
			case 3808:
				width = 18;
				height = 18;
				legSlot = 148;
				rare = 8;
				defense = 16;
				value = sellPrice(0, 3);
				return;
			case 3809:
				width = 22;
				height = 22;
				accessory = true;
				rare = 5;
				value = sellPrice(0, 3);
				neckSlot = 9;
				return;
			case 3810:
				width = 22;
				height = 22;
				accessory = true;
				rare = 5;
				value = sellPrice(0, 3);
				shieldSlot = 6;
				return;
			case 3811:
				width = 22;
				height = 22;
				accessory = true;
				rare = 5;
				value = sellPrice(0, 3);
				handOnSlot = 19;
				return;
			case 3812:
				width = 22;
				height = 22;
				accessory = true;
				rare = 5;
				value = sellPrice(0, 3);
				waistSlot = 12;
				return;
			case 3813:
				rare = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 463;
				width = 12;
				height = 12;
				value = 100000;
				glowMask = 244;
				return;
			case 3814:
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 464;
				width = 12;
				height = 12;
				value = 100000;
				return;
			case 3815:
				rare = 1;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 465;
				width = 12;
				height = 12;
				value = 100000;
				return;
			case 3816:
				rare = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 466;
				width = 12;
				height = 12;
				value = buyPrice(0, 1);
				return;
			case 3817:
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				value = 0;
				rare = 3;
				return;
			case 3818:
			case 3819:
			case 3820:
			case 3824:
			case 3825:
			case 3826:
			case 3829:
			case 3830:
			case 3831:
			case 3832:
			case 3833:
			case 3834:
				width = 18;
				height = 20;
				UseSound = SoundID.DD2_DefenseTowerSpawn;
				useStyle = 1;
				useAnimation = 30;
				useTime = 30;
				shootSpeed = 1f;
				noMelee = true;
				value = sellPrice(0, 1);
				rare = 3;
				shoot = 663;
				summon = true;
				damage = 17;
				knockBack = 3f;
				mana = 5;
				DD2Summon = true;
				sentry = true;
				switch (type)
				{
				case 3819:
					shoot = 665;
					damage = 42;
					rare = 5;
					mana = 10;
					value = sellPrice(0, 5);
					break;
				case 3820:
					shoot = 667;
					damage = 88;
					rare = 8;
					mana = 15;
					value = sellPrice(0, 15);
					break;
				case 3824:
					shoot = 677;
					damage = 30;
					knockBack = 4.7f;
					break;
				case 3825:
					shoot = 678;
					damage = 74;
					rare = 5;
					mana = 10;
					knockBack = 4.7f;
					value = sellPrice(0, 5);
					break;
				case 3826:
					shoot = 679;
					damage = 156;
					rare = 8;
					mana = 15;
					knockBack = 4.7f;
					value = sellPrice(0, 15);
					break;
				case 3832:
					shoot = 691;
					damage = 24;
					knockBack = 0.5f;
					break;
				case 3833:
					shoot = 692;
					damage = 59;
					rare = 5;
					mana = 10;
					knockBack = 0.5f;
					value = sellPrice(0, 5);
					break;
				case 3834:
					shoot = 693;
					damage = 126;
					rare = 8;
					mana = 15;
					knockBack = 0.5f;
					value = sellPrice(0, 15);
					break;
				case 3829:
					shoot = 688;
					damage = 4;
					knockBack = 0.25f;
					break;
				case 3830:
					shoot = 689;
					damage = 11;
					rare = 5;
					mana = 10;
					knockBack = 0.25f;
					value = sellPrice(0, 5);
					break;
				case 3831:
					shoot = 690;
					damage = 34;
					rare = 8;
					mana = 15;
					knockBack = 0.25f;
					value = sellPrice(0, 15);
					break;
				case 3821:
				case 3822:
				case 3823:
				case 3827:
				case 3828:
					break;
				}
				return;
			case 3821:
				shootSpeed = 6.5f;
				shoot = 669;
				width = 20;
				height = 20;
				maxStack = 1;
				UseSound = SoundID.Item1;
				useStyle = 5;
				useAnimation = 40;
				useTime = 40;
				noUseGraphic = true;
				noMelee = true;
				value = sellPrice(0, 0, 1);
				damage = 20;
				knockBack = 7f;
				ranged = true;
				rare = 1;
				useAmmo = 353;
				return;
			case 3822:
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				value = 0;
				return;
			case 3823:
				UseSound = SoundID.Item1;
				useStyle = 1;
				damage = 95;
				useAnimation = 20;
				useTime = 20;
				width = 34;
				height = 34;
				scale = 1.1f;
				knockBack = 6.5f;
				melee = true;
				rare = 5;
				value = sellPrice(0, 1);
				autoReuse = true;
				flame = true;
				useTurn = true;
				return;
			case 3828:
				rare = 3;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 22;
				height = 18;
				value = buyPrice(0, 0, 25);
				return;
			case 3835:
				useStyle = 5;
				useAnimation = 30;
				useTime = 30;
				shootSpeed = 24f;
				knockBack = 7f;
				width = 16;
				height = 16;
				UseSound = SoundID.DD2_MonkStaffSwing;
				shoot = 697;
				rare = 5;
				value = sellPrice(0, 1);
				noMelee = true;
				noUseGraphic = true;
				channel = true;
				autoReuse = true;
				melee = true;
				damage = 50;
				return;
			case 3836:
				useStyle = 5;
				useAnimation = 27;
				useTime = 27;
				shootSpeed = 42f;
				knockBack = 7f;
				width = 16;
				height = 16;
				UseSound = SoundID.DD2_GhastlyGlaivePierce;
				shoot = 699;
				rare = 5;
				value = sellPrice(0, 1);
				noMelee = true;
				noUseGraphic = true;
				channel = true;
				melee = true;
				damage = 45;
				return;
			case 3837:
			case 3838:
			case 3839:
			case 3840:
			case 3841:
			case 3842:
			case 3843:
			case 3844:
			case 3845:
			case 3846:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 278 + type - 3837;
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				return;
			}
			switch (type)
			{
			case 3855:
			case 3856:
			case 3857:
				damage = 0;
				useStyle = 1;
				width = 16;
				height = 30;
				UseSound = SoundID.Item2;
				useAnimation = 20;
				useTime = 20;
				rare = 3;
				noMelee = true;
				value = sellPrice(0, 2);
				buffType = 200;
				shoot = 703;
				switch (type)
				{
				case 3856:
					buffType = 201;
					shoot = 702;
					break;
				case 3857:
					buffType = 202;
					shoot = 701;
					break;
				}
				return;
			case 3854:
				useStyle = 5;
				useAnimation = 18;
				useTime = 18;
				shootSpeed = 20f;
				knockBack = 2f;
				width = 20;
				height = 12;
				damage = 32;
				UseSound = SoundID.Item5;
				shoot = 705;
				rare = 5;
				value = sellPrice(0, 1);
				noMelee = true;
				noUseGraphic = true;
				ranged = true;
				channel = true;
				useAmmo = AmmoID.Arrow;
				autoReuse = true;
				return;
			case 3827:
				rare = 8;
				UseSound = SoundID.DD2_SonicBoomBladeSlash;
				useStyle = 1;
				damage = 180;
				useAnimation = 20;
				useTime = 20;
				width = 30;
				height = 30;
				knockBack = 5.5f;
				melee = true;
				value = sellPrice(0, 5);
				autoReuse = true;
				useTurn = false;
				glowMask = 227;
				shoot = 684;
				shootSpeed = 17f;
				return;
			case 3852:
				useStyle = 5;
				useAnimation = 25;
				useTime = 3;
				shootSpeed = 11f;
				knockBack = 9f;
				width = 16;
				height = 16;
				damage = 36;
				UseSound = SoundID.DD2_BookStaffCast;
				shoot = 712;
				mana = 20;
				rare = 5;
				value = sellPrice(0, 1);
				noMelee = true;
				magic = true;
				autoReuse = true;
				return;
			case 3858:
				useStyle = 5;
				useAnimation = 30;
				useTime = 30;
				shootSpeed = 24f;
				knockBack = 5f;
				width = 16;
				height = 16;
				UseSound = SoundID.DD2_SkyDragonsFurySwing;
				shoot = 707;
				rare = 8;
				value = sellPrice(0, 5);
				noMelee = true;
				noUseGraphic = true;
				channel = true;
				autoReuse = true;
				melee = true;
				damage = 140;
				return;
			case 3859:
				autoReuse = true;
				useStyle = 5;
				useAnimation = 30;
				useTime = 30;
				width = 14;
				height = 32;
				shoot = 710;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item102;
				damage = 39;
				shootSpeed = 11f;
				knockBack = 4.5f;
				rare = 8;
				crit = 3;
				noMelee = true;
				value = sellPrice(0, 5);
				ranged = true;
				glowMask = 234;
				return;
			case 3860:
			case 3861:
			case 3862:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 24;
				height = 24;
				rare = 1;
				if (type == 3860)
				{
					rare = 8;
				}
				if (type == 3862)
				{
					rare = 3;
				}
				if (type == 3861)
				{
					rare = 5;
				}
				expert = true;
				return;
			case 3863:
			case 3864:
			case 3865:
				width = 28;
				height = 20;
				rare = 1;
				vanity = true;
				value = sellPrice(0, 0, 75);
				switch (type)
				{
				case 3863:
					headSlot = 207;
					break;
				case 3865:
					headSlot = 209;
					break;
				case 3864:
					headSlot = 208;
					break;
				}
				return;
			}
			switch (type)
			{
			case 3866:
			case 3867:
			case 3868:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				placeStyle = 60;
				if (type == 3866)
				{
					placeStyle = 61;
				}
				if (type == 3868)
				{
					placeStyle = 62;
				}
				rare = 1;
				return;
			case 3869:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				consumable = true;
				createTile = 139;
				placeStyle = 39;
				width = 24;
				height = 24;
				rare = 4;
				value = 100000;
				accessory = true;
				hasVanityEffects = true;
				return;
			case 3870:
				useStyle = 5;
				useAnimation = 20;
				useTime = 20;
				reuseDelay = 10;
				shootSpeed = 14f;
				knockBack = 7f;
				width = 16;
				height = 16;
				damage = 100;
				UseSound = SoundID.DD2_BetsysWrathShot;
				shoot = 711;
				mana = 14;
				rare = 8;
				value = sellPrice(0, 5);
				noMelee = true;
				magic = true;
				autoReuse = true;
				glowMask = 238;
				return;
			case 3871:
				width = 18;
				height = 18;
				rare = 8;
				defense = 20;
				value = sellPrice(0, 3);
				headSlot = 210;
				return;
			case 3872:
				width = 18;
				height = 18;
				rare = 8;
				defense = 24;
				value = sellPrice(0, 3);
				bodySlot = 204;
				return;
			case 3873:
				width = 18;
				height = 18;
				rare = 8;
				defense = 24;
				value = sellPrice(0, 3);
				legSlot = 152;
				return;
			case 3874:
				width = 18;
				height = 18;
				rare = 8;
				defense = 7;
				value = sellPrice(0, 3);
				headSlot = 211;
				return;
			case 3875:
				width = 18;
				height = 18;
				rare = 8;
				defense = 21;
				value = sellPrice(0, 3);
				bodySlot = 205;
				return;
			case 3876:
				width = 18;
				height = 18;
				rare = 8;
				defense = 14;
				value = sellPrice(0, 3);
				legSlot = 153;
				return;
			case 3877:
				width = 18;
				height = 18;
				rare = 8;
				defense = 8;
				value = sellPrice(0, 3);
				headSlot = 212;
				return;
			case 3878:
				width = 18;
				height = 18;
				rare = 8;
				defense = 24;
				value = sellPrice(0, 3);
				bodySlot = 206;
				return;
			case 3879:
				width = 18;
				height = 18;
				rare = 8;
				defense = 16;
				value = sellPrice(0, 3);
				legSlot = 154;
				return;
			case 3880:
				width = 18;
				height = 18;
				rare = 8;
				defense = 10;
				value = sellPrice(0, 3);
				headSlot = 213;
				return;
			case 3881:
				width = 18;
				height = 18;
				rare = 8;
				defense = 26;
				value = sellPrice(0, 3);
				bodySlot = 207;
				return;
			case 3882:
				width = 18;
				height = 18;
				rare = 8;
				defense = 18;
				value = sellPrice(0, 3);
				legSlot = 156;
				return;
			case 3883:
				width = 22;
				height = 20;
				accessory = true;
				value = buyPrice(0, 40);
				rare = 8;
				wingSlot = 37;
				return;
			case 3884:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 467;
				width = 26;
				height = 22;
				value = 500;
				placeStyle = 0;
				return;
			case 3885:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 467;
				width = 26;
				height = 22;
				value = buyPrice(0, 10);
				placeStyle = 1;
				return;
			case 3886:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 468;
				width = 26;
				height = 22;
				value = 500;
				placeStyle = 0;
				return;
			case 3887:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 468;
				width = 26;
				height = 22;
				value = 500;
				placeStyle = 1;
				return;
			case 3888:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				width = 14;
				height = 28;
				value = 200;
				placeStyle = 36;
				return;
			case 3889:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				width = 12;
				height = 30;
				placeStyle = 36;
				value = 150;
				return;
			case 3890:
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 33;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				placeStyle = 30;
				return;
			case 3891:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 37;
				value = 150;
				return;
			case 3892:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 93;
				width = 10;
				height = 24;
				value = 500;
				placeStyle = 31;
				return;
			case 3893:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 100;
				width = 20;
				height = 20;
				value = 1500;
				placeStyle = 31;
				return;
			case 3894:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				width = 26;
				height = 26;
				value = 3000;
				placeStyle = 37;
				return;
			case 3895:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 90;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 31;
				return;
			case 3896:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 172;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 32;
				return;
			case 3897:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				autoReuse = true;
				createTile = 79;
				width = 28;
				height = 20;
				value = 2000;
				placeStyle = 31;
				return;
			case 3898:
			case 3899:
			case 3900:
			case 3901:
			case 3902:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 104;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 28 + type - 3898;
				return;
			}
			if (type >= 3903 && type <= 3908)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				width = 8;
				height = 10;
				placeStyle = 30 + type - 3903;
				return;
			}
			switch (type)
			{
			case 3909:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				width = 28;
				height = 14;
				value = 150;
				placeStyle = 31;
				return;
			case 3910:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				width = 28;
				height = 14;
				value = buyPrice(0, 10);
				placeStyle = 32;
				return;
			case 3911:
			case 3912:
			case 3913:
			case 3914:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 88;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 28 + type - 3911;
				return;
			}
			if (type >= 3915 && type <= 3916)
			{
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 87;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 30 + type - 3915;
				return;
			}
			switch (type)
			{
			case 3917:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 32;
				break;
			case 3918:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 89;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 33;
				break;
			case 3919:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 89;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 34;
				break;
			case 3920:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 469;
				width = 26;
				height = 20;
				value = 300;
				placeStyle = 0;
				break;
			case 3921:
				width = 28;
				height = 20;
				headSlot = 214;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				break;
			case 3922:
				width = 28;
				height = 20;
				bodySlot = 208;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				break;
			case 3923:
				width = 18;
				height = 14;
				legSlot = 158;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				break;
			case 3924:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 38;
				value = 400000;
				break;
			case 3925:
				width = 28;
				height = 20;
				headSlot = 215;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				break;
			case 3926:
				width = 28;
				height = 20;
				bodySlot = 209;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				break;
			case 3927:
				width = 18;
				height = 14;
				legSlot = 159;
				rare = 9;
				value = sellPrice(0, 5);
				vanity = true;
				break;
			case 3928:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 39;
				value = 400000;
				break;
			case 3929:
				width = 18;
				height = 14;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				accessory = true;
				hasVanityEffects = true;
				break;
			case 3930:
				useStyle = 5;
				useAnimation = 6;
				useTime = 6;
				shootSpeed = 17f;
				knockBack = 10f;
				width = 20;
				height = 12;
				damage = 50;
				UseSound = null;
				shoot = 714;
				rare = 10;
				value = sellPrice(0, 10);
				noMelee = true;
				noUseGraphic = true;
				ranged = true;
				channel = true;
				autoReuse = true;
				useAmmo = AmmoID.Rocket;
				break;
			case 3931:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 90;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 32;
				break;
			case 3932:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				autoReuse = true;
				createTile = 79;
				width = 28;
				height = 20;
				value = 2000;
				placeStyle = 32;
				break;
			case 3933:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 33;
				break;
			case 3934:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 88;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 32;
				break;
			case 3935:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 100;
				width = 20;
				height = 20;
				value = 1500;
				placeStyle = 32;
				break;
			case 3936:
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 33;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				placeStyle = 31;
				break;
			case 3937:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				width = 12;
				height = 30;
				placeStyle = 37;
				value = 150;
				break;
			case 3938:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				width = 26;
				height = 26;
				value = 3000;
				placeStyle = 38;
				break;
			case 3939:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 467;
				width = 26;
				height = 22;
				value = 500;
				placeStyle = 2;
				break;
			case 3940:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 104;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 33;
				break;
			case 3941:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				width = 14;
				height = 28;
				value = 200;
				placeStyle = 37;
				break;
			case 3942:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 93;
				width = 10;
				height = 24;
				value = 500;
				placeStyle = 32;
				break;
			case 3943:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 38;
				value = 150;
				break;
			case 3944:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 87;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 32;
				break;
			case 3945:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				width = 8;
				height = 10;
				placeStyle = 36;
				break;
			case 3946:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 172;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 33;
				break;
			case 3947:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 89;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 35;
				break;
			case 3948:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 469;
				width = 26;
				height = 20;
				value = 300;
				placeStyle = 1;
				break;
			case 3949:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				width = 28;
				height = 14;
				value = 150;
				placeStyle = 33;
				break;
			case 3950:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 468;
				width = 26;
				height = 22;
				value = 500;
				placeStyle = 2;
				break;
			case 3951:
				createTile = 472;
				width = 16;
				height = 16;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				break;
			case 3952:
				createWall = 231;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				break;
			case 3953:
				createTile = 473;
				width = 16;
				height = 16;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				break;
			case 3954:
				createWall = 232;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				break;
			case 3955:
				createTile = 474;
				width = 16;
				height = 16;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				break;
			case 3956:
				createWall = 233;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				break;
			case 3957:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 19;
				width = 8;
				height = 10;
				placeStyle = 37;
				break;
			case 3958:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 90;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 33;
				break;
			case 3959:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				autoReuse = true;
				createTile = 79;
				width = 28;
				height = 20;
				value = 2000;
				placeStyle = 33;
				break;
			case 3960:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 101;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 34;
				break;
			case 3968:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 88;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 33;
				break;
			case 3961:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 100;
				width = 20;
				height = 20;
				value = 1500;
				placeStyle = 33;
				break;
			case 3962:
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 33;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				placeStyle = 32;
				break;
			case 3963:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 15;
				width = 12;
				height = 30;
				placeStyle = 38;
				value = 150;
				break;
			case 3964:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 34;
				width = 26;
				height = 26;
				value = 3000;
				placeStyle = 39;
				break;
			case 3965:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 467;
				width = 26;
				height = 22;
				value = 500;
				placeStyle = 3;
				break;
			case 3966:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 104;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 34;
				break;
			case 3967:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 10;
				width = 14;
				height = 28;
				value = 200;
				placeStyle = 38;
				break;
			case 3969:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 93;
				width = 10;
				height = 24;
				value = 500;
				placeStyle = 33;
				break;
			case 3970:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 42;
				width = 12;
				height = 28;
				placeStyle = 39;
				value = 150;
				break;
			case 3971:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 87;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 33;
				break;
			case 3972:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 172;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 34;
				break;
			case 3973:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 89;
				width = 20;
				height = 20;
				value = 300;
				placeStyle = 36;
				break;
			case 3974:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 469;
				width = 26;
				height = 20;
				value = 300;
				placeStyle = 2;
				break;
			case 3975:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 18;
				width = 28;
				height = 14;
				value = 150;
				placeStyle = 34;
				break;
			case 3976:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 468;
				width = 26;
				height = 22;
				value = 500;
				placeStyle = 3;
				break;
			case 3977:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 269;
				width = 22;
				height = 32;
				createTile = 475;
				break;
			case 3978:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				break;
			case 3979:
				width = 12;
				height = 12;
				rare = 1;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 10);
				createTile = 376;
				placeStyle = 9;
				useAnimation = 15;
				useTime = 15;
				autoReuse = true;
				useStyle = 1;
				consumable = true;
				break;
			case 3980:
				width = 12;
				height = 12;
				rare = 2;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 0, 50);
				createTile = 376;
				placeStyle = 10;
				useAnimation = 15;
				useTime = 15;
				autoReuse = true;
				useStyle = 1;
				consumable = true;
				break;
			case 3981:
				width = 12;
				height = 12;
				rare = 3;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 2);
				createTile = 376;
				placeStyle = 11;
				useAnimation = 15;
				useTime = 15;
				autoReuse = true;
				useStyle = 1;
				consumable = true;
				break;
			case 3982:
			case 3983:
			case 3984:
			case 3985:
			case 3986:
			case 3987:
				width = 12;
				height = 12;
				rare = 2;
				maxStack = CommonMaxStack;
				createTile = 376;
				placeStyle = 12 + type - 3982;
				useAnimation = 15;
				useTime = 15;
				autoReuse = true;
				useStyle = 1;
				consumable = true;
				value = sellPrice(0, 1);
				break;
			default:
				switch (type)
				{
				case 3988:
					useStyle = 1;
					useTurn = true;
					useAnimation = 15;
					useTime = 10;
					autoReuse = true;
					maxStack = CommonMaxStack;
					consumable = true;
					createTile = 467;
					width = 26;
					height = 22;
					value = 500;
					placeStyle = 4;
					break;
				case 3989:
					DefaultToGolfBall(721);
					break;
				}
				break;
			}
		}

		public void DefaultToGolfBall(int projid)
		{
			shoot = projid;
			useStyle = 1;
			shootSpeed = 12f;
			width = 18;
			height = 20;
			maxStack = 1;
			UseSound = SoundID.Item1;
			useAnimation = 15;
			useTime = 15;
			noUseGraphic = true;
			noMelee = true;
			value = 0;
			accessory = true;
			SetShopValues(ItemRarityColor.Green2, buyPrice(0, 1));
			hasVanityEffects = true;
		}

		public void SetDefaults5(int type)
		{
			switch (type)
			{
			case 3990:
				DefaultToAccessory(36, 28);
				shoeSlot = 18;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 2));
				break;
			case 3991:
				DefaultToAccessory(30, 42);
				faceSlot = 9;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				break;
			case 3992:
				defense = 8;
				DefaultToAccessory(20, 40);
				handOnSlot = 20;
				handOffSlot = 12;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				break;
			case 3993:
				DefaultToAccessory(34, 30);
				shoeSlot = 19;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				break;
			case 3994:
				DefaultToAccessory(24, 30);
				shoeSlot = 20;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 2));
				break;
			case 3995:
				DefaultToAccessory(34, 32);
				handOnSlot = 21;
				handOffSlot = 13;
				shoeSlot = 20;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 5));
				break;
			case 3996:
				DefaultToAccessory(28, 30);
				handOnSlot = 21;
				handOffSlot = 13;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 2));
				break;
			case 3997:
				defense = 6;
				DefaultToAccessory(36, 38);
				shieldSlot = 7;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 8));
				break;
			case 3998:
				defense = 10;
				DefaultToAccessory(36, 40);
				shieldSlot = 8;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				break;
			case 3999:
				DefaultToAccessory(22, 32);
				faceSlot = 10;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 2, 50));
				break;
			case 4000:
				DefaultToAccessory(28, 32);
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 4));
				break;
			case 4001:
				DefaultToAccessory(26, 36);
				backSlot = 14;
				frontSlot = 5;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 3));
				break;
			case 4002:
				DefaultToAccessory(34, 36);
				backSlot = 15;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 7, 50));
				break;
			case 4003:
				DefaultToAccessory(30, 34);
				faceSlot = 11;
				SetShopValues(ItemRarityColor.LightPurple6, sellPrice(0, 5));
				break;
			case 4004:
				DefaultToAccessory(30, 32);
				faceSlot = 13;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 3));
				break;
			case 4005:
				DefaultToAccessory(30, 30);
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				break;
			case 4006:
				DefaultToAccessory(36, 38);
				backSlot = 16;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				break;
			case 4007:
				DefaultToAccessory(26, 30);
				neckSlot = 10;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 3));
				break;
			case 4008:
				defense = 4;
				DefaultToHeadgear(24, 22, 216);
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 2));
				break;
			case 4038:
				DefaultToAccessory(28, 34);
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 7, 50));
				break;
			case 4039:
				DefaultToGolfClub(20, 20);
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 1));
				break;
			case 4094:
				DefaultToGolfClub(20, 20);
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 1));
				break;
			case 4092:
				DefaultToGolfClub(20, 20);
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 1));
				break;
			case 4093:
				DefaultToGolfClub(20, 20);
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 1));
				break;
			case 4040:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 476;
				width = 12;
				height = 12;
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 1));
				break;
			case 4041:
			case 4042:
			case 4043:
			case 4044:
			case 4045:
			case 4046:
			case 4047:
			case 4048:
			case 4241:
				DefaultToPlaceableTile((ushort)3, 0);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 5));
				autoReuse = false;
				useTime = useAnimation;
				break;
			case 4049:
				DefaultToLawnMower(20, 20);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4050:
				DefaultToPlaceableTile((ushort)478, 0);
				break;
			case 4051:
				DefaultToPlaceableTile((ushort)479, 0);
				break;
			case 4052:
				DefaultToPlaceableWall(234);
				break;
			case 4053:
				DefaultToPlaceableWall(235);
				break;
			case 4054:
				DefaultToPlaceableTile((ushort)480, 0);
				width = 22;
				height = 32;
				rare = 3;
				value = sellPrice(0, 1);
				accessory = true;
				vanity = true;
				break;
			case 4055:
				DefaultToAccessory(34, 30);
				shoeSlot = 21;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4056:
				DefaultToAccessory(30, 30);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4057:
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 1));
				DefaultToGuitar();
				break;
			case 4058:
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 10));
				DefaultToBow(17, 11f);
				SetWeaponValues(8, 5f);
				break;
			case 4059:
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 30));
				SetWeaponValues(8, 4f);
				width = 16;
				height = 16;
				melee = true;
				autoReuse = true;
				useTurn = true;
				useTime = 14;
				useAnimation = 18;
				useStyle = 1;
				pick = 55;
				UseSound = SoundID.Item1;
				break;
			case 4060:
				width = 42;
				height = 20;
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 50));
				DefaultToRangedWeapon(728, AmmoID.FallenStar, 18, 20f, hasAutoReuse: true);
				SetWeaponValues(60, 5f);
				if (Variant == ItemVariants.RebalancedVariant)
				{
					damage = (int)((double)damage * 0.9);
					useTime = (int)((double)useTime * 1.1);
				}
				break;
			case 4063:
				DefaultToPlaceableTile((ushort)486, 0);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4064:
				DefaultToPlaceableTile((ushort)487, 0);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 0, 80));
				break;
			case 4065:
				DefaultToPlaceableTile((ushort)487, 1);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 1));
				break;
			case 4061:
				DefaultToSpear(730, 3.5f, 28);
				SetWeaponValues(14, 6f);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 30));
				break;
			case 4062:
				DefaultToStaff(731, 8f, 17, 7);
				SetWeaponValues(20, 3f);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 30));
				break;
			case 4066:
				DefaultToMount(15);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 50));
				break;
			case 4067:
				DefaultToMount(16);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4068:
				DefaultToCapturedCritter(583);
				rare = 1;
				value = sellPrice(0, 1);
				break;
			case 4069:
				DefaultToCapturedCritter(584);
				rare = 1;
				value = sellPrice(0, 1);
				break;
			case 4070:
				DefaultToCapturedCritter(585);
				rare = 1;
				value = sellPrice(0, 1);
				break;
			case 4071:
			case 4072:
			case 4073:
				DefaultToSeaShelll();
				break;
			case 4074:
				DefaultToPlaceableTile((ushort)489, 0);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 20));
				break;
			case 4075:
				DefaultToPlaceableTile((ushort)490, 0);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 10));
				break;
			case 4076:
				rare = 3;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 491;
				width = 12;
				height = 12;
				value = 100000;
				break;
			case 4091:
				DefaultToPlaceableTile((ushort)496, 0);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 1));
				break;
			case 4090:
				DefaultToPlaceableTile((ushort)495, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 1));
				break;
			case 4095:
				maxStack = 1;
				consumable = false;
				width = 18;
				height = 18;
				useStyle = 4;
				useTime = 10;
				UseSound = SoundID.Item128;
				useAnimation = 10;
				rare = 4;
				value = sellPrice(0, 2);
				break;
			case 4077:
				DefaultToMusicBox(43);
				break;
			case 4078:
				DefaultToMusicBox(41);
				break;
			case 4079:
				DefaultToMusicBox(42);
				break;
			case 4080:
				DefaultToMusicBox(44);
				break;
			case 4081:
				DefaultToMusicBox(45);
				break;
			case 4082:
				DefaultToMusicBox(40);
				break;
			case 4089:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 494;
				width = 12;
				height = 12;
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 1));
				break;
			case 4083:
			case 4084:
			case 4085:
			case 4086:
			case 4087:
			case 4088:
				DefaultToPlaceableTile((ushort)493, type - 4083);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 20));
				break;
			case 4096:
			case 4097:
			case 4098:
			case 4099:
			case 4100:
			case 4101:
			case 4102:
			case 4103:
			case 4104:
			case 4105:
			case 4106:
			case 4107:
			case 4108:
			case 4109:
			case 4110:
			case 4111:
			case 4112:
			case 4113:
			case 4114:
			case 4115:
			case 4116:
			case 4117:
			case 4118:
			case 4119:
			case 4120:
			case 4121:
			case 4122:
			case 4123:
			case 4124:
			case 4125:
			case 4126:
				DefaultToPlaceableTile((ushort)497, type - 4096);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 4127:
				DefaultToPlaceableTile((ushort)497, type - 4096);
				maxStack = CommonMaxStack;
				value = 100000;
				break;
			case 4128:
				width = 18;
				height = 18;
				headSlot = 217;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4129:
				width = 18;
				height = 18;
				bodySlot = 210;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4130:
				width = 18;
				height = 18;
				legSlot = 180;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4131:
			case 5325:
				useStyle = 1;
				shootSpeed = 4f;
				shoot = 734;
				width = 26;
				height = 24;
				UseSound = SoundID.Item130;
				useAnimation = 28;
				useTime = 28;
				rare = 3;
				value = sellPrice(0, 2);
				break;
			case 4132:
				width = 18;
				height = 18;
				headSlot = 218;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4133:
				width = 18;
				height = 18;
				bodySlot = 211;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4134:
				width = 18;
				height = 18;
				legSlot = 184;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4135:
				width = 18;
				height = 18;
				headSlot = 219;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4136:
				width = 18;
				height = 18;
				bodySlot = 212;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4137:
				width = 18;
				height = 18;
				legSlot = 185;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4138:
				width = 18;
				height = 18;
				headSlot = 220;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4141:
				DefaultToPlaceableTile((ushort)497, 32);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 4142:
				DefaultToPlaceableTile((ushort)499, 0);
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.White0, 100000);
				break;
			case 4139:
				DefaultToPlaceableTile((ushort)498, 0);
				SetShopValues(ItemRarityColor.White0, 0);
				break;
			case 4140:
				DefaultToPlaceableWall(236);
				SetShopValues(ItemRarityColor.White0, 0);
				break;
			case 4143:
				width = 12;
				height = 12;
				break;
			case 4144:
				width = 14;
				height = 38;
				useAnimation = 25;
				useTime = 15;
				useStyle = 5;
				rare = 2;
				noUseGraphic = true;
				channel = true;
				noMelee = true;
				damage = 17;
				knockBack = 3f;
				autoReuse = false;
				noMelee = true;
				melee = true;
				shoot = 735;
				shootSpeed = 15f;
				value = sellPrice(0, 5);
				break;
			case 4145:
				DefaultToPlaceableTile((ushort)90, 34);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4146:
				DefaultToPlaceableTile((ushort)79, 34);
				SetShopValues(ItemRarityColor.White0, 2000);
				maxStack = CommonMaxStack;
				width = 28;
				height = 20;
				break;
			case 4147:
				DefaultToPlaceableTile((ushort)101, 35);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4148:
				DefaultToPlaceableTile((ushort)88, 34);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4149:
				DefaultToPlaceableTile((ushort)100, 34);
				SetShopValues(ItemRarityColor.White0, 1500);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4150:
				DefaultToPlaceableTile((ushort)33, 33);
				SetShopValues(ItemRarityColor.White0, 0);
				maxStack = CommonMaxStack;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				noWet = true;
				break;
			case 4151:
				DefaultToPlaceableTile((ushort)15, 39);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 30;
				break;
			case 4152:
				DefaultToPlaceableTile((ushort)34, 40);
				SetShopValues(ItemRarityColor.White0, 3000);
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 4153:
				DefaultToPlaceableTile((ushort)467, 5);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4154:
				DefaultToPlaceableTile((ushort)104, 35);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4155:
				DefaultToPlaceableTile((ushort)10, 39);
				SetShopValues(ItemRarityColor.White0, 200);
				maxStack = CommonMaxStack;
				width = 14;
				height = 28;
				break;
			case 4156:
				DefaultToPlaceableTile((ushort)93, 34);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 10;
				height = 24;
				break;
			case 4157:
				DefaultToPlaceableTile((ushort)42, 40);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 28;
				break;
			case 4158:
				DefaultToPlaceableTile((ushort)87, 34);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4159:
				DefaultToPlaceableTile((ushort)19, 38);
				SetShopValues(ItemRarityColor.White0, 0);
				width = 8;
				height = 10;
				break;
			case 4160:
				DefaultToPlaceableTile((ushort)172, 35);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4161:
				DefaultToPlaceableTile((ushort)89, 37);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4162:
				DefaultToPlaceableTile((ushort)469, 3);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 26;
				height = 20;
				break;
			case 4163:
				DefaultToPlaceableTile((ushort)18, 35);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 28;
				height = 14;
				break;
			case 4164:
				DefaultToPlaceableTile((ushort)468, 5);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4165:
				DefaultToPlaceableTile((ushort)497, 33);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 4166:
				DefaultToPlaceableTile((ushort)90, 35);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4167:
				DefaultToPlaceableTile((ushort)79, 35);
				SetShopValues(ItemRarityColor.White0, 2000);
				maxStack = CommonMaxStack;
				width = 28;
				height = 20;
				break;
			case 4168:
				DefaultToPlaceableTile((ushort)101, 36);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4169:
				DefaultToPlaceableTile((ushort)88, 35);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4170:
				DefaultToPlaceableTile((ushort)100, 35);
				SetShopValues(ItemRarityColor.White0, 1500);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4171:
				DefaultToPlaceableTile((ushort)33, 34);
				SetShopValues(ItemRarityColor.White0, 0);
				maxStack = CommonMaxStack;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				noWet = true;
				break;
			case 4172:
				DefaultToPlaceableTile((ushort)15, 40);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 30;
				break;
			case 4173:
				DefaultToPlaceableTile((ushort)34, 41);
				SetShopValues(ItemRarityColor.White0, 3000);
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 4174:
				DefaultToPlaceableTile((ushort)467, 6);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4175:
				DefaultToPlaceableTile((ushort)104, 36);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4176:
				DefaultToPlaceableTile((ushort)10, 40);
				SetShopValues(ItemRarityColor.White0, 200);
				maxStack = CommonMaxStack;
				width = 14;
				height = 28;
				break;
			case 4177:
				DefaultToPlaceableTile((ushort)93, 35);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 10;
				height = 24;
				break;
			case 4178:
				DefaultToPlaceableTile((ushort)42, 41);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 28;
				break;
			case 4179:
				DefaultToPlaceableTile((ushort)87, 35);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4180:
				DefaultToPlaceableTile((ushort)19, 39);
				SetShopValues(ItemRarityColor.White0, 0);
				width = 8;
				height = 10;
				break;
			case 4181:
				DefaultToPlaceableTile((ushort)172, 36);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4182:
				DefaultToPlaceableTile((ushort)89, 38);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4183:
				DefaultToPlaceableTile((ushort)469, 4);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 26;
				height = 20;
				break;
			case 4184:
				DefaultToPlaceableTile((ushort)18, 36);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 28;
				height = 14;
				break;
			case 4185:
				DefaultToPlaceableTile((ushort)468, 6);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4186:
				DefaultToPlaceableTile((ushort)497, 34);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 4187:
				DefaultToPlaceableTile((ushort)90, 36);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4188:
				DefaultToPlaceableTile((ushort)79, 36);
				SetShopValues(ItemRarityColor.White0, 2000);
				maxStack = CommonMaxStack;
				width = 28;
				height = 20;
				break;
			case 4189:
				DefaultToPlaceableTile((ushort)101, 37);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4190:
				DefaultToPlaceableTile((ushort)88, 36);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4191:
				DefaultToPlaceableTile((ushort)100, 36);
				SetShopValues(ItemRarityColor.White0, 1500);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4192:
				DefaultToPlaceableTile((ushort)33, 35);
				SetShopValues(ItemRarityColor.White0, 0);
				maxStack = CommonMaxStack;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				noWet = true;
				break;
			case 4193:
				DefaultToPlaceableTile((ushort)15, 41);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 30;
				break;
			case 4194:
				DefaultToPlaceableTile((ushort)34, 42);
				SetShopValues(ItemRarityColor.White0, 3000);
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 4195:
				DefaultToPlaceableTile((ushort)467, 7);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4196:
				DefaultToPlaceableTile((ushort)104, 37);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4197:
				DefaultToPlaceableTile((ushort)10, 41);
				SetShopValues(ItemRarityColor.White0, 200);
				maxStack = CommonMaxStack;
				width = 14;
				height = 28;
				break;
			case 4198:
				DefaultToPlaceableTile((ushort)93, 36);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 10;
				height = 24;
				break;
			case 4199:
				DefaultToPlaceableTile((ushort)42, 42);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 28;
				break;
			case 4200:
				DefaultToPlaceableTile((ushort)87, 36);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4201:
				DefaultToPlaceableTile((ushort)19, 40);
				SetShopValues(ItemRarityColor.White0, 0);
				width = 8;
				height = 10;
				break;
			case 4202:
				DefaultToPlaceableTile((ushort)172, 37);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4203:
				DefaultToPlaceableTile((ushort)89, 39);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4204:
				DefaultToPlaceableTile((ushort)469, 5);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 26;
				height = 20;
				break;
			case 4205:
				DefaultToPlaceableTile((ushort)18, 37);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 28;
				height = 14;
				break;
			case 4206:
				DefaultToPlaceableTile((ushort)468, 7);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4207:
				DefaultToPlaceableTile((ushort)497, 35);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 4208:
				DefaultToPlaceableTile((ushort)90, 37);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4209:
				DefaultToPlaceableTile((ushort)79, 37);
				SetShopValues(ItemRarityColor.White0, 2000);
				maxStack = CommonMaxStack;
				width = 28;
				height = 20;
				break;
			case 4210:
				DefaultToPlaceableTile((ushort)101, 38);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4211:
				DefaultToPlaceableTile((ushort)88, 37);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4212:
				DefaultToPlaceableTile((ushort)100, 37);
				SetShopValues(ItemRarityColor.White0, 1500);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4213:
				DefaultToPlaceableTile((ushort)33, 36);
				SetShopValues(ItemRarityColor.White0, 0);
				maxStack = CommonMaxStack;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				noWet = true;
				break;
			case 4214:
				DefaultToPlaceableTile((ushort)15, 42);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 30;
				break;
			case 4215:
				DefaultToPlaceableTile((ushort)34, 43);
				SetShopValues(ItemRarityColor.White0, 3000);
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 4216:
				DefaultToPlaceableTile((ushort)467, 8);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4217:
				DefaultToPlaceableTile((ushort)104, 38);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4218:
				DefaultToPlaceableTile((ushort)10, 42);
				SetShopValues(ItemRarityColor.White0, 200);
				maxStack = CommonMaxStack;
				width = 14;
				height = 28;
				break;
			case 4219:
				DefaultToPlaceableTile((ushort)93, 37);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 10;
				height = 24;
				break;
			case 4220:
				DefaultToPlaceableTile((ushort)42, 43);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 28;
				break;
			case 4221:
				DefaultToPlaceableTile((ushort)87, 37);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4222:
				DefaultToPlaceableTile((ushort)19, 41);
				SetShopValues(ItemRarityColor.White0, 0);
				width = 8;
				height = 10;
				break;
			case 4223:
				DefaultToPlaceableTile((ushort)172, 38);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4224:
				DefaultToPlaceableTile((ushort)89, 40);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4225:
				DefaultToPlaceableTile((ushort)469, 6);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 26;
				height = 20;
				break;
			case 4226:
				DefaultToPlaceableTile((ushort)18, 38);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 28;
				height = 14;
				break;
			case 4227:
				DefaultToPlaceableTile((ushort)468, 8);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4228:
				DefaultToPlaceableTile((ushort)497, 36);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 4229:
				DefaultToPlaceableTile((ushort)500, 0);
				break;
			case 4230:
				DefaultToPlaceableTile((ushort)501, 0);
				break;
			case 4231:
				DefaultToPlaceableTile((ushort)502, 0);
				break;
			case 4232:
				DefaultToPlaceableTile((ushort)503, 0);
				break;
			case 4233:
				DefaultToPlaceableWall(237);
				break;
			case 4234:
				DefaultToPlaceableWall(238);
				break;
			case 4235:
				DefaultToPlaceableWall(239);
				break;
			case 4236:
				DefaultToPlaceableWall(240);
				break;
			case 4237:
				DefaultToMusicBox(46);
				break;
			case 4238:
				DefaultToPlaceableTile((ushort)481, 0);
				break;
			case 4239:
				DefaultToPlaceableTile((ushort)482, 0);
				break;
			case 4240:
				DefaultToPlaceableTile((ushort)483, 0);
				break;
			case 4242:
				DefaultToGolfBall(739);
				break;
			case 4243:
				DefaultToGolfBall(740);
				break;
			case 4244:
				DefaultToGolfBall(741);
				break;
			case 4245:
				DefaultToGolfBall(742);
				break;
			case 4246:
				DefaultToGolfBall(743);
				break;
			case 4247:
				DefaultToGolfBall(744);
				break;
			case 4248:
				DefaultToGolfBall(745);
				break;
			case 4249:
				DefaultToGolfBall(746);
				break;
			case 4250:
				DefaultToGolfBall(747);
				break;
			case 4251:
				DefaultToGolfBall(748);
				break;
			case 4252:
				DefaultToGolfBall(749);
				break;
			case 4253:
				DefaultToGolfBall(750);
				break;
			case 4254:
				DefaultToGolfBall(751);
				break;
			case 4255:
				DefaultToGolfBall(752);
				break;
			case 4257:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 12.5f;
				shoot = 753;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = 20000;
				break;
			case 4256:
				defense = 3;
				width = 18;
				height = 14;
				bodySlot = 213;
				value = sellPrice(0, 0, 50) * 6;
				rare = 2;
				break;
			case 4258:
				useStyle = 1;
				useTime = 18;
				useAnimation = 18;
				knockBack = 3f;
				width = 40;
				height = 40;
				damage = 26;
				scale = 1f;
				UseSound = SoundID.Item15;
				rare = 1;
				value = 27000;
				melee = true;
				break;
			case 4259:
				SetDefaults(198);
				this.type = type;
				damage = 48;
				useTime = 16;
				useAnimation = 16;
				scale = 1.15f;
				autoReuse = true;
				useTurn = true;
				rare = 4;
				value = sellPrice(0, 1);
				break;
			case 4260:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 7;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createWall = 241;
				width = 12;
				height = 12;
				value = sellPrice(0, 0, 1, 60);
				break;
			case 4261:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 135;
				width = 12;
				height = 12;
				placeStyle = 7;
				mech = true;
				value = 5000;
				break;
			case 4262:
				useTurn = true;
				width = 20;
				height = 20;
				useStyle = 5;
				useTime = 90;
				UseSound = SoundID.Item151;
				useAnimation = 90;
				rare = 1;
				value = 50000;
				shoot = 754;
				break;
			case 4263:
				useTurn = true;
				width = 20;
				height = 20;
				useStyle = 4;
				useTime = 90;
				UseSound = SoundID.Item6;
				useAnimation = 90;
				rare = 1;
				value = 50000;
				break;
			case 4264:
				useStyle = 4;
				channel = true;
				width = 34;
				height = 34;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				mountType = 17;
				value = sellPrice(0, 10);
				break;
			case 4265:
				DefaultToPlaceableTile((ushort)467, 9);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 3));
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4266:
				DefaultToPlaceableTile((ushort)468, 9);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4267:
				DefaultToPlaceableTile((ushort)467, 10);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4268:
				DefaultToPlaceableTile((ushort)468, 10);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4269:
				mana = 10;
				damage = 35;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 755;
				buffType = 213;
				width = 26;
				height = 28;
				UseSound = SoundID.Item83;
				useAnimation = 36;
				useTime = 36;
				rare = 4;
				noMelee = true;
				knockBack = 3f;
				value = sellPrice(0, 5);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				break;
			case 4270:
				mana = 20;
				damage = 34;
				useStyle = 4;
				shootSpeed = 32f;
				shoot = 756;
				width = 26;
				height = 28;
				useAnimation = 33;
				useTime = 11;
				rare = 4;
				noMelee = true;
				knockBack = 1f;
				value = sellPrice(0, 4);
				magic = true;
				autoReuse = true;
				break;
			case 4271:
				useStyle = 4;
				width = 22;
				height = 14;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				maxStack = CommonMaxStack;
				rare = 2;
				break;
			case 4272:
				noMelee = true;
				useStyle = 5;
				useAnimation = 40;
				useTime = 40;
				knockBack = 6.5f;
				width = 30;
				height = 10;
				damage = 55;
				scale = 1.1f;
				noUseGraphic = true;
				shoot = 757;
				shootSpeed = 15.9f;
				UseSound = SoundID.Item1;
				rare = 4;
				value = sellPrice(0, 4);
				melee = true;
				channel = true;
				break;
			case 4273:
				mana = 10;
				damage = 11;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 758;
				width = 26;
				height = 28;
				UseSound = SoundID.Item44;
				useAnimation = 36;
				useTime = 36;
				noMelee = true;
				knockBack = 5f;
				buffType = 214;
				value = sellPrice(0, 1);
				rare = 3;
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				break;
			case 4274:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				value = sellPrice(0, 10);
				rare = 3;
				makeNPC = 592;
				break;
			case 4275:
				width = 18;
				height = 18;
				headSlot = 221;
				value = sellPrice(0, 10);
				rare = 3;
				vanity = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 505;
				width = 12;
				height = 12;
				break;
			case 4276:
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 506;
				width = 20;
				height = 20;
				break;
			case 4277:
				DefaultToPlaceableTile((ushort)507, 0);
				break;
			case 4278:
				DefaultToPlaceableTile((ushort)508, 0);
				break;
			case 4279:
				DefaultToPlaceableWall(242);
				break;
			case 4280:
				DefaultToPlaceableWall(243);
				break;
			case 4281:
				mana = 10;
				damage = 7;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 759;
				buffType = 216;
				width = 26;
				height = 28;
				UseSound = SoundID.Item44;
				useAnimation = 36;
				useTime = 36;
				rare = 1;
				noMelee = true;
				knockBack = 4f;
				value = sellPrice(0, 1);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				break;
			case 4298:
				DefaultToPlaceableTile((ushort)90, 38);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4299:
				DefaultToPlaceableTile((ushort)79, 38);
				SetShopValues(ItemRarityColor.White0, 2000);
				maxStack = CommonMaxStack;
				width = 28;
				height = 20;
				break;
			case 4300:
				DefaultToPlaceableTile((ushort)101, 39);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4301:
				DefaultToPlaceableTile((ushort)88, 38);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4302:
				DefaultToPlaceableTile((ushort)100, 38);
				SetShopValues(ItemRarityColor.White0, 1500);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4303:
				DefaultToPlaceableTile((ushort)33, 37);
				SetShopValues(ItemRarityColor.White0, 0);
				maxStack = CommonMaxStack;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				noWet = true;
				break;
			case 4304:
				DefaultToPlaceableTile((ushort)15, 43);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 30;
				break;
			case 4305:
				DefaultToPlaceableTile((ushort)34, 44);
				SetShopValues(ItemRarityColor.White0, 3000);
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 4306:
				DefaultToPlaceableTile((ushort)104, 39);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4307:
				DefaultToPlaceableTile((ushort)10, 43);
				SetShopValues(ItemRarityColor.White0, 200);
				maxStack = CommonMaxStack;
				width = 14;
				height = 28;
				break;
			case 4308:
				DefaultToPlaceableTile((ushort)93, 38);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 10;
				height = 24;
				break;
			case 4309:
				DefaultToPlaceableTile((ushort)42, 44);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 28;
				break;
			case 4310:
				DefaultToPlaceableTile((ushort)87, 38);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4311:
				DefaultToPlaceableTile((ushort)19, 42);
				SetShopValues(ItemRarityColor.White0, 0);
				width = 8;
				height = 10;
				break;
			case 4312:
				DefaultToPlaceableTile((ushort)172, 39);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4313:
				DefaultToPlaceableTile((ushort)89, 41);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4314:
				DefaultToPlaceableTile((ushort)469, 7);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 26;
				height = 20;
				break;
			case 4315:
				DefaultToPlaceableTile((ushort)18, 39);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 28;
				height = 14;
				break;
			case 4316:
				DefaultToPlaceableTile((ushort)497, 37);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 4317:
				SetShopValues(ItemRarityColor.LightRed4, sellPrice(0, 2));
				SetWeaponValues(30, 7f);
				melee = true;
				autoReuse = true;
				useTime = 11;
				useAnimation = 27;
				useStyle = 1;
				hammer = 80;
				axe = 30;
				UseSound = SoundID.Item1;
				width = 20;
				height = 20;
				break;
			case 4318:
				DefaultToPlaceableTile((ushort)509, 0);
				width = 22;
				height = 32;
				rare = 9;
				value = sellPrice(0, 25);
				accessory = true;
				vanity = true;
				break;
			case 4319:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 510;
				width = 28;
				height = 28;
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 20));
				break;
			case 4320:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 511;
				width = 28;
				height = 28;
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 20));
				break;
			case 4321:
				width = 18;
				height = 18;
				bodySlot = 214;
				rare = 3;
				vanity = true;
				value = buyPrice(0, 5);
				break;
			case 4322:
				width = 18;
				height = 18;
				legSlot = 188;
				rare = 3;
				vanity = true;
				value = buyPrice(0, 5);
				break;
			case 4323:
				width = 18;
				height = 18;
				headSlot = 222;
				rare = 3;
				vanity = true;
				value = buyPrice(0, 50);
				break;
			case 4324:
				width = 18;
				height = 14;
				bodySlot = 215;
				value = buyPrice(0, 50);
				rare = 3;
				vanity = true;
				break;
			case 4325:
				useStyle = 1;
				useAnimation = 8;
				useTime = 8;
				width = 24;
				height = 28;
				UseSound = SoundID.Item1;
				shoot = 760;
				fishingPole = 25;
				shootSpeed = 15f;
				rare = 2;
				value = sellPrice(0, 2);
				break;
			case 4326:
				DefaultToPlaceableTile((ushort)520, 0);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 4327:
			case 4328:
			case 4329:
			case 4330:
			case 4331:
			case 4332:
				DefaultToPlaceableTile((ushort)(type - 4327 + 521));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4333:
				DefaultToPlaceableTile((ushort)527, 0);
				value = sellPrice(0, 10);
				rare = 3;
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4334:
			case 4335:
			case 4336:
			case 4337:
			case 4338:
			case 4339:
				DefaultToCapturedCritter((short)(type - 4334 + 595));
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 10));
				width = 20;
				height = 20;
				bait = 20;
				break;
			case 4340:
				DefaultToCapturedCritter(601);
				value = sellPrice(0, 10);
				rare = 3;
				width = 20;
				height = 20;
				bait = 50;
				break;
			case 4341:
				DefaultToAccessory(30, 30);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 50));
				break;
			case 4342:
				DefaultToPlaceableTile((ushort)105, 78);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 60));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4343:
				width = 22;
				height = 16;
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 10));
				DefaultToThrownWeapon(761, 17, 5f);
				SetWeaponValues(4, 2f);
				break;
			case 4344:
				width = 22;
				height = 16;
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 10));
				DefaultToThrownWeapon(762, 17, 5f);
				SetWeaponValues(4, 2f);
				break;
			case 4345:
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 5));
				maxStack = CommonMaxStack;
				width = 12;
				height = 12;
				break;
			case 4346:
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 5));
				width = 30;
				height = 30;
				break;
			case 4347:
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 17));
				DefaultToMagicWeapon(876, 36, 15f);
				mana = 16;
				width = 40;
				height = 40;
				knockBack = 6f;
				scale = 0.75f;
				damage = 42;
				UseSound = SoundID.Item158;
				break;
			case 4348:
				SetShopValues(ItemRarityColor.Pink5, buyPrice(0, 50));
				DefaultToMagicWeapon(876, 36, 15f);
				mana = 16;
				width = 40;
				height = 40;
				knockBack = 6f;
				scale = 0.75f;
				damage = 100;
				UseSound = SoundID.Item158;
				break;
			case 4349:
			case 4350:
			case 4351:
			case 4352:
			case 4353:
				DefaultToPlaceableTile(179 + type - 4349);
				break;
			case 4354:
				DefaultToPlaceableTile((ushort)381, 0);
				rare = 1;
				break;
			case 4355:
				DefaultToPlaceableTile((ushort)531, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 60));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4356:
				DefaultToMusicBox(47);
				break;
			case 4357:
				DefaultToMusicBox(48);
				break;
			case 4358:
				DefaultToMusicBox(49);
				break;
			case 4359:
				DefaultToCapturedCritter(602);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 7, 50));
				width = 20;
				height = 20;
				break;
			case 4360:
				DefaultToPlaceableTile((ushort)105, 77);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 60));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4361:
				DefaultToCapturedCritter(604);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 10));
				width = 20;
				height = 20;
				bait = 17;
				break;
			case 4362:
				DefaultToCapturedCritter(605);
				value = sellPrice(0, 10);
				rare = 3;
				width = 20;
				height = 20;
				bait = 50;
				break;
			case 4363:
				DefaultToCapturedCritter(606);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 5));
				width = 12;
				height = 12;
				bait = 22;
				break;
			case 4364:
				DefaultToPlaceableTile((ushort)532, 0);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4365:
				DefaultToVanitypet(764, 217);
				SetShopValues(ItemRarityColor.Pink5, buyPrice(1));
				break;
			case 4366:
				DefaultToVanitypet(765, 218);
				break;
			case 4372:
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 1));
				DefaultToGuitar();
				break;
			case 4367:
			case 4368:
			case 4369:
			case 4370:
			case 4371:
				DefaultTokite(type - 4367 + 766);
				break;
			case 4373:
				DefaultToCapturedCritter(607);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 7, 50));
				width = 12;
				height = 12;
				break;
			case 4374:
				DefaultToCapturedCritter(608);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 7, 50));
				width = 12;
				height = 12;
				break;
			case 4375:
				DefaultToCapturedCritter(610);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 10));
				width = 12;
				height = 12;
				break;
			case 4376:
				DefaultToPlaceableTile((ushort)533, 0);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4377:
				DefaultToPlaceableTile((ushort)534, 0);
				rare = 1;
				break;
			case 4378:
				DefaultToPlaceableTile((ushort)536, 0);
				rare = 1;
				break;
			case 4389:
				DefaultToPlaceableTile((ushort)539, 0);
				rare = 1;
				break;
			case 4379:
				DefaultTokite(771);
				break;
			case 4380:
				DefaultToPlaceableTile((ushort)538, 0);
				maxStack = CommonMaxStack;
				break;
			case 4382:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				rare = 2;
				break;
			case 4383:
				DefaultToTorch(16);
				break;
			case 4384:
				DefaultToTorch(17, allowWaterPlacement: true);
				break;
			case 4385:
				DefaultToTorch(18);
				break;
			case 4386:
				DefaultToTorch(19);
				break;
			case 4387:
				DefaultToTorch(20);
				break;
			case 4388:
				DefaultToTorch(21);
				break;
			case 4390:
				DefaultToPlaceableTile((ushort)484, 0);
				break;
			case 4391:
				DefaultToPlaceableTile((ushort)162, 0);
				break;
			case 4392:
				DefaultToPlaceableTile((ushort)541, 0);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 10));
				break;
			case 4393:
			case 4394:
				DefaultToQuestFish();
				break;
			case 4395:
				DefaultToCapturedCritter(611);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 10));
				width = 12;
				height = 12;
				break;
			case 4396:
				DefaultToPlaceableTile((ushort)542, 0);
				maxStack = CommonMaxStack;
				break;
			case 4397:
				DefaultToPlaceableTile((ushort)105, 76);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 60));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4398:
				DefaultToPlaceableTile((ushort)543, 0);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				value = sellPrice(0, 0, 20);
				break;
			case 4399:
				DefaultToPlaceableTile((ushort)544, 0);
				value = sellPrice(0, 10);
				rare = 3;
				maxStack = CommonMaxStack;
				break;
			case 4400:
				useStyle = 1;
				shootSpeed = 7f;
				shoot = 772;
				width = 22;
				height = 22;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				noUseGraphic = true;
				useTime = 15;
				rare = 2;
				value = sellPrice(0, 0, 5);
				consumable = true;
				maxStack = CommonMaxStack;
				break;
			case 4401:
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 7, 50));
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 4402:
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 50));
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 4404:
				DefaultToAccessory(20, 12);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 1));
				hasVanityEffects = true;
				break;
			case 4405:
			case 4406:
			case 4407:
			case 4408:
				DefaultToPlaceableTile((ushort)376, 18 + type - 4405);
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 1));
				maxStack = CommonMaxStack;
				break;
			case 4409:
				DefaultToAccessory(28);
				faceSlot = 14;
				SetShopValues(ItemRarityColor.Pink5, buyPrice(0, 10));
				break;
			case 4410:
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 1));
				maxStack = CommonMaxStack;
				width = 32;
				height = 22;
				break;
			case 4412:
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				maxStack = CommonMaxStack;
				width = 22;
				height = 22;
				break;
			case 4413:
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 3));
				maxStack = CommonMaxStack;
				width = 22;
				height = 22;
				break;
			case 4414:
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 15));
				maxStack = CommonMaxStack;
				width = 22;
				height = 22;
				break;
			case 4418:
				DefaultToCapturedCritter(612);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 10));
				width = 20;
				height = 20;
				bait = 17;
				break;
			case 4419:
				DefaultToCapturedCritter(613);
				value = sellPrice(0, 10);
				rare = 3;
				width = 20;
				height = 20;
				bait = 50;
				break;
			case 4415:
				DefaultToPlaceableTile((ushort)10, 44);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 40));
				maxStack = CommonMaxStack;
				width = 14;
				height = 28;
				break;
			case 4416:
				DefaultToPlaceableTile((ushort)19, 43);
				SetShopValues(ItemRarityColor.White0, 0);
				width = 8;
				height = 10;
				break;
			case 4417:
				DefaultToPlaceableTile((ushort)207, 9);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 4));
				width = 8;
				height = 10;
				break;
			case 4420:
				DefaultToPlaceableTile((ushort)545, 0);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 5));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4421:
				DefaultToMusicBox(50);
				break;
			case 4422:
				DefaultToPlaceableTile((ushort)546, 0);
				break;
			case 4423:
				useStyle = 1;
				shootSpeed = 5f;
				shoot = 773;
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				UseSound = SoundID.Item1;
				consumable = true;
				useAnimation = 25;
				noUseGraphic = true;
				useTime = 25;
				value = sellPrice(0, 0, 3);
				rare = 1;
				break;
			case 4424:
				DefaultToPlaceableWall(245);
				break;
			case 4425:
				DefaultToVanitypet(774, 219);
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 1));
				break;
			case 4426:
				DefaultToMount(18);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4427:
				DefaultToMount(19);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 50));
				break;
			case 4428:
				DefaultToMount(20);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4429:
				DefaultToMount(21);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 50));
				break;
			case 4430:
				DefaultToPlaceableTile((ushort)547, 0);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4431:
				DefaultToPlaceableTile((ushort)547, 1);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4432:
				DefaultToPlaceableTile((ushort)547, 2);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4433:
				DefaultToPlaceableTile((ushort)547, 3);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4434:
				DefaultToPlaceableTile((ushort)547, 4);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4435:
				DefaultToPlaceableTile((ushort)548, 0);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4436:
				DefaultToPlaceableTile((ushort)548, 1);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4437:
				DefaultToPlaceableTile((ushort)548, 2);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4438:
				DefaultToPlaceableTile((ushort)548, 3);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4439:
				DefaultToPlaceableTile((ushort)548, 4);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4440:
				DefaultToPlaceableTile((ushort)548, 5);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4441:
				DefaultToPlaceableTile((ushort)548, 6);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4442:
				useStyle = 1;
				useAnimation = 8;
				useTime = 8;
				width = 24;
				height = 28;
				UseSound = SoundID.Item1;
				shoot = 775;
				fishingPole = 30;
				shootSpeed = 15f;
				rare = 1;
				value = sellPrice(0, 2);
				break;
			case 4443:
				DefaultToMount(22);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4444:
				useStyle = 4;
				channel = true;
				width = 34;
				height = 34;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				mountType = 23;
				value = sellPrice(0, 5);
				expert = true;
				break;
			case 4445:
				damage = 50;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 4f;
				value = buyPrice(0, 0, 7, 50);
				ranged = true;
				break;
			case 4446:
				damage = 50;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 4f;
				value = buyPrice(0, 0, 15);
				ranged = true;
				break;
			case 4447:
				damage = 40;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 4f;
				value = sellPrice(0, 0, 10);
				ranged = true;
				break;
			case 4448:
				damage = 40;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 4f;
				value = sellPrice(0, 0, 10);
				ranged = true;
				break;
			case 4449:
				damage = 40;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 4f;
				value = sellPrice(0, 0, 10);
				ranged = true;
				break;
			case 4450:
				DefaultToMount(24);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4451:
				DefaultToMount(25);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4452:
				DefaultToMount(26);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4453:
				DefaultToMount(27);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4454:
				DefaultToMount(28);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4455:
				DefaultToMount(29);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4456:
				DefaultToMount(30);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4457:
				damage = 75;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 4f;
				value = buyPrice(0, 0, 5);
				ranged = true;
				break;
			case 4458:
				damage = 75;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 4f;
				value = buyPrice(0, 0, 10);
				ranged = true;
				break;
			case 4459:
				damage = 40;
				width = 20;
				height = 14;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Rocket;
				knockBack = 4f;
				value = buyPrice(0, 0, 50);
				ranged = true;
				break;
			case 4460:
				tileWand = 169;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				createTile = 552;
				width = 8;
				height = 10;
				rare = 1;
				value = sellPrice(0, 1);
				break;
			case 4463:
				autoReuse = false;
				useStyle = 13;
				useAnimation = 21;
				useTime = 7;
				width = 50;
				height = 18;
				shoot = 802;
				UseSound = SoundID.Item1;
				damage = 15;
				shootSpeed = 2.4f;
				noMelee = true;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 30));
				melee = true;
				knockBack = 3f;
				noUseGraphic = true;
				break;
			case 4464:
				DefaultToCapturedCritter(616);
				value = sellPrice(0, 0, 10);
				break;
			case 4465:
				DefaultToCapturedCritter(617);
				value = sellPrice(0, 0, 10);
				break;
			case 4466:
				DefaultToPlaceableTile((ushort)105, 79);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 60));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4461:
				DefaultToPlaceableTile((ushort)550, 0);
				maxStack = CommonMaxStack;
				break;
			case 4462:
				DefaultToPlaceableTile((ushort)551, 0);
				maxStack = CommonMaxStack;
				break;
			case 4467:
				DefaultToMount(31);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4473:
				DefaultToPlaceableTile((ushort)553, 0);
				maxStack = CommonMaxStack;
				break;
			case 4474:
				DefaultToPlaceableTile((ushort)554, 0);
				maxStack = CommonMaxStack;
				break;
			case 4475:
				DefaultToPlaceableTile((ushort)555, 0);
				maxStack = CommonMaxStack;
				break;
			case 4476:
				DefaultToPlaceableTile((ushort)556, 0);
				maxStack = CommonMaxStack;
				value = sellPrice(0, 10);
				rare = 3;
				break;
			case 4468:
				DefaultToMount(32);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 4));
				break;
			case 4469:
				DefaultToMount(33);
				SetShopValues(ItemRarityColor.StrongRed10, sellPrice(0, 10));
				break;
			case 4470:
				DefaultToMount(34);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4471:
				DefaultToMount(35);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4472:
				DefaultToMount(36);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4477:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 257;
				buffTime = 18000;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4478:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 257;
				buffTime = 36000;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 5));
				break;
			case 4479:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 257;
				buffTime = 54000;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 25));
				break;
			case 4480:
				DefaultToCapturedCritter(626);
				value = sellPrice(0, 0, 15);
				break;
			case 4481:
				DefaultToPlaceableTile((ushort)558, 0);
				maxStack = CommonMaxStack;
				break;
			case 4482:
				DefaultToCapturedCritter(627);
				value = sellPrice(0, 10);
				rare = 3;
				break;
			case 4483:
				DefaultToPlaceableTile((ushort)559, 0);
				value = sellPrice(0, 10);
				rare = 3;
				maxStack = CommonMaxStack;
				break;
			case 4484:
			case 4485:
				mech = true;
				noWet = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 144;
				placeStyle = type - 4484 + 3;
				width = 10;
				height = 12;
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 2));
				break;
			case 4541:
			case 4542:
			case 4543:
			case 4544:
			case 4545:
			case 4546:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 288 + (type - 4541);
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				break;
			case 4486:
			case 4487:
			case 4488:
			case 4489:
			case 4490:
			case 4491:
			case 4492:
			case 4493:
			case 4494:
			case 4495:
			case 4496:
			case 4497:
			case 4498:
			case 4499:
			case 4500:
			case 4501:
			case 4502:
			case 4503:
				DefaultToPlaceableWall((ushort)(246 + type - 4486));
				break;
			case 4504:
			case 4505:
				DefaultToPlaceableWall((ushort)(264 + type - 4504));
				value = 250;
				break;
			case 4506:
			case 4507:
				DefaultToPlaceableWall((ushort)(266 + type - 4506));
				break;
			case 4508:
				DefaultToPlaceableWall((ushort)(268 + type - 4508));
				value = 250;
				break;
			case 4509:
			case 4510:
			case 4511:
				DefaultToPlaceableWall((ushort)(269 + type - 4509));
				break;
			case 4512:
				DefaultToPlaceableWall(274);
				break;
			case 4513:
			case 4514:
			case 4515:
			case 4516:
			case 4517:
			case 4518:
			case 4519:
			case 4520:
			case 4521:
			case 4522:
			case 4523:
			case 4524:
			case 4525:
			case 4526:
			case 4527:
			case 4528:
			case 4529:
			case 4530:
			case 4531:
			case 4532:
			case 4533:
			case 4534:
			case 4535:
			case 4536:
			case 4537:
			case 4538:
			case 4539:
			case 4540:
				DefaultToPlaceableWall((ushort)(276 + type - 4513));
				break;
			case 4599:
			case 4600:
			case 4601:
				DefaultToPlaceableTile((ushort)560, type - 4599);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 1));
				maxStack = CommonMaxStack;
				break;
			case 4587:
			case 4588:
			case 4589:
			case 4590:
				DefaultToGolfClub(20, 20);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 10));
				break;
			case 4591:
			case 4592:
			case 4593:
			case 4594:
				DefaultToGolfClub(20, 20);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 10));
				break;
			case 4595:
			case 4596:
			case 4597:
			case 4598:
				DefaultToGolfClub(20, 20);
				SetShopValues(ItemRarityColor.LightRed4, buyPrice(0, 25));
				break;
			case 4602:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 294;
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				break;
			case 4606:
				DefaultToMusicBox(51);
				break;
			case 4603:
				DefaultToVanitypet(815, 258);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(1));
				break;
			case 4604:
				DefaultToVanitypet(816, 259);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(1));
				break;
			case 4605:
				DefaultToVanitypet(817, 260);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(1));
				break;
			case 4566:
				DefaultToPlaceableTile((ushort)90, 39);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4567:
				DefaultToPlaceableTile((ushort)79, 39);
				SetShopValues(ItemRarityColor.White0, 2000);
				maxStack = CommonMaxStack;
				width = 28;
				height = 20;
				break;
			case 4568:
				DefaultToPlaceableTile((ushort)101, 40);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4569:
				DefaultToPlaceableTile((ushort)88, 39);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4570:
				DefaultToPlaceableTile((ushort)100, 39);
				SetShopValues(ItemRarityColor.White0, 1500);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4571:
				DefaultToPlaceableTile((ushort)33, 38);
				SetShopValues(ItemRarityColor.White0, 0);
				maxStack = CommonMaxStack;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				noWet = true;
				break;
			case 4572:
				DefaultToPlaceableTile((ushort)15, 44);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 30;
				break;
			case 4573:
				DefaultToPlaceableTile((ushort)34, 45);
				SetShopValues(ItemRarityColor.White0, 3000);
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 4574:
				DefaultToPlaceableTile((ushort)467, 11);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4575:
				DefaultToPlaceableTile((ushort)104, 40);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4576:
				DefaultToPlaceableTile((ushort)10, 45);
				SetShopValues(ItemRarityColor.White0, 200);
				maxStack = CommonMaxStack;
				width = 14;
				height = 28;
				break;
			case 4577:
				DefaultToPlaceableTile((ushort)93, 39);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 10;
				height = 24;
				break;
			case 4578:
				DefaultToPlaceableTile((ushort)42, 45);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 28;
				break;
			case 4579:
				DefaultToPlaceableTile((ushort)87, 39);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4580:
				DefaultToPlaceableTile((ushort)19, 44);
				SetShopValues(ItemRarityColor.White0, 0);
				width = 8;
				height = 10;
				break;
			case 4581:
				DefaultToPlaceableTile((ushort)172, 40);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4582:
				DefaultToPlaceableTile((ushort)89, 42);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4583:
				DefaultToPlaceableTile((ushort)469, 8);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 26;
				height = 20;
				break;
			case 4584:
				DefaultToPlaceableTile((ushort)18, 40);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 28;
				height = 14;
				break;
			case 4586:
				DefaultToPlaceableTile((ushort)497, 38);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 4585:
				DefaultToPlaceableTile((ushort)468, 11);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4554:
				DefaultToPlaceableTile((ushort)561, 0);
				break;
			case 4564:
				DefaultToPlaceableTile((ushort)562, 0);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 1));
				break;
			case 4565:
				DefaultToPlaceableWall(312);
				break;
			case 4547:
				DefaultToPlaceableTile((ushort)563, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 20));
				break;
			case 4548:
				DefaultToPlaceableWall(313);
				break;
			case 4607:
				mana = 10;
				damage = 41;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 831;
				buffType = 263;
				width = 26;
				height = 28;
				UseSound = SoundID.Item44;
				useAnimation = 36;
				useTime = 36;
				rare = 8;
				noMelee = true;
				knockBack = 4f;
				value = sellPrice(0, 20);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				break;
			case 4381:
				DefaultToBow(19, 9f, hasAutoReuse: true);
				SetWeaponValues(14, 3f);
				value = sellPrice(0, 1);
				rare = 3;
				break;
			case 4549:
				width = 18;
				height = 18;
				headSlot = 223;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4560:
				width = 18;
				height = 18;
				headSlot = 224;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 3);
				break;
			case 4561:
				width = 18;
				height = 18;
				headSlot = 225;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4563:
				DefaultToAccessory(18, 18);
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				hasVanityEffects = true;
				break;
			case 4562:
				width = 18;
				height = 18;
				headSlot = 226;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4555:
				width = 18;
				height = 18;
				headSlot = 227;
				rare = 0;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4556:
				width = 18;
				height = 18;
				bodySlot = 216;
				rare = 0;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4557:
				width = 18;
				height = 18;
				legSlot = 190;
				rare = 0;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4553:
				DefaultToPlaceableTile((ushort)564, 0);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4551:
				DefaultToVanitypet(821, 261);
				break;
			case 4609:
				DefaultToPlaceableTile((ushort)567, 0);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 4608:
				useStyle = 1;
				shootSpeed = 7f;
				shoot = 820;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 19;
				useTime = 19;
				noMelee = true;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 5));
				break;
			case 4610:
				DefaultTokite(822);
				break;
			case 4611:
				DefaultTokite(823);
				break;
			case 4612:
				DefaultTokite(824);
				break;
			case 4558:
				width = 18;
				height = 18;
				headSlot = 228;
				rare = 3;
				vanity = true;
				value = buyPrice(0, 2);
				break;
			case 4559:
				width = 18;
				height = 18;
				headSlot = 229;
				rare = 3;
				vanity = true;
				value = buyPrice(0, 2);
				break;
			case 4550:
				DefaultToVanitypet(825, 262);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(1));
				break;
			case 4613:
				DefaultTokite(826);
				break;
			case 4626:
			case 4627:
			case 4628:
			case 4629:
			case 4630:
			case 4631:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 63 + (type - 4626);
				break;
			case 4632:
			case 4633:
			case 4634:
			case 4635:
			case 4636:
			case 4637:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 37 + (type - 4632);
				break;
			case 4638:
			case 4639:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 7 + (type - 4638);
				break;
			case 4640:
				DefaultToPlaceableTile((ushort)67, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 1));
				break;
			case 4641:
				DefaultToPlaceableTile((ushort)66, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 1));
				break;
			case 4642:
				DefaultToPlaceableTile((ushort)63, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 1));
				break;
			case 4643:
				DefaultToPlaceableTile((ushort)65, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 1));
				break;
			case 4644:
				DefaultToPlaceableTile((ushort)64, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 1));
				break;
			case 4645:
				DefaultToPlaceableTile((ushort)68, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 1));
				break;
			case 4646:
				DefaultToPlaceableTile((ushort)566, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 1));
				break;
			case 4647:
				DefaultToPlaceableWall(314);
				break;
			case 4648:
			case 4649:
			case 4650:
			case 4651:
				DefaultTokite(827 + (type - 4648));
				break;
			case 4652:
				width = 18;
				height = 18;
				headSlot = 230;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4653:
				width = 18;
				height = 18;
				bodySlot = 217;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4654:
				width = 18;
				height = 18;
				legSlot = 191;
				rare = 3;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4655:
				DefaultToPlaceableTile((ushort)568, 0);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4656:
				DefaultToPlaceableTile((ushort)569, 0);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4657:
				DefaultToPlaceableTile((ushort)570, 0);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 4552:
				DefaultToPlaceableTile((ushort)565, 0);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 4));
				break;
			case 4658:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 43;
				break;
			case 4659:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 9;
				break;
			case 4660:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 69;
				break;
			case 4661:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 246;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 19;
				break;
			case 4662:
			case 4663:
			case 4778:
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				value = sellPrice(0, 1, 50);
				rare = 3;
				break;
			case 4664:
				width = 18;
				height = 18;
				bodySlot = 218;
				rare = 3;
				vanity = true;
				value = buyPrice(0, 20);
				break;
			case 4665:
				width = 18;
				height = 18;
				legSlot = 193;
				rare = 3;
				vanity = true;
				value = buyPrice(0, 20);
				break;
			case 4666:
				width = 18;
				height = 18;
				headSlot = 232;
				rare = 3;
				vanity = true;
				value = buyPrice(0, 15);
				break;
			case 4667:
				DefaultToPlaceableWall(315);
				break;
			case 4668:
				paintCoating = 1;
				width = 24;
				height = 24;
				value = buyPrice(0, 0, 2);
				maxStack = CommonMaxStack;
				break;
			case 4669:
				DefaultTokite(838);
				break;
			case 4670:
				DefaultTokite(839);
				break;
			case 4671:
				DefaultTokite(840);
				break;
			case 4672:
				DefaultToWhip(841, 14, 1f, 4f);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4673:
				autoReuse = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 12;
				useTime = 12;
				width = 16;
				height = 16;
				scale = 1f;
				value = buyPrice(0, 0, 50);
				break;
			case 4674:
				DefaultTokite(843);
				break;
			case 4675:
				DefaultTokite(844);
				break;
			case 4676:
				DefaultTokite(845);
				value = buyPrice(0, 2);
				break;
			case 4677:
				DefaultTokite(846);
				break;
			case 4678:
				DefaultToWhip(847, 55, 2f, 4f, 28);
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 4, 60));
				break;
			case 4679:
				DefaultToWhip(848, 180, 11f, 4f, 35);
				SetShopValues(ItemRarityColor.Yellow8, sellPrice(0, 6));
				crit = 10;
				break;
			case 4680:
				DefaultToWhip(849, 100, 3f, 4f, 27);
				SetShopValues(ItemRarityColor.Yellow8, sellPrice(0, 10));
				break;
			case 4681:
				DefaultTokite(850);
				value = buyPrice(0, 2);
				break;
			case 4682:
				width = 20;
				height = 24;
				DefaultToThrownWeapon(851, 25, 4f);
				value = buyPrice(0, 0, 1);
				ranged = false;
				noUseGraphic = true;
				break;
			case 4683:
				DefaultTokite(852);
				break;
			case 4684:
				DefaultTokite(853);
				break;
			case 4685:
				width = 18;
				height = 18;
				headSlot = 231;
				vanity = true;
				value = buyPrice(0, 10);
				break;
			case 4686:
				width = 18;
				height = 18;
				bodySlot = 219;
				vanity = true;
				value = buyPrice(0, 10);
				break;
			case 4687:
			case 4688:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 295 + (type - 4687);
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				break;
			case 4689:
			case 4690:
			case 4691:
			case 4692:
			case 4693:
			case 4694:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 215;
				placeStyle = 8 + type - 4689;
				width = 12;
				height = 12;
				break;
			case 4695:
			case 4696:
			case 4697:
			case 4698:
			case 4699:
			case 4700:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 572;
				placeStyle = type - 4695;
				width = 12;
				height = 28;
				if (type >= 4695 && type <= 4697)
				{
					value = 1000;
				}
				else
				{
					value = 40000;
				}
				break;
			case 4701:
				DefaultToVanitypet(854, 264);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 50));
				break;
			case 4702:
				width = 20;
				height = 24;
				DefaultToThrownWeapon(855, 25, 4f);
				value = buyPrice(0, 0, 1);
				ranged = false;
				noUseGraphic = true;
				break;
			case 4703:
				DefaultToRangedWeapon(14, AmmoID.Bullet, 55, 7f);
				knockBack = 6.5f;
				width = 50;
				height = 14;
				UseSound = SoundID.Item36;
				damage = 14;
				value = buyPrice(0, 35);
				rare = 3;
				break;
			case 4704:
				width = 18;
				height = 18;
				headSlot = 233;
				vanity = true;
				value = buyPrice(0, 10);
				break;
			case 4705:
				width = 18;
				height = 18;
				bodySlot = 220;
				vanity = true;
				value = buyPrice(0, 10);
				break;
			case 4706:
				width = 18;
				height = 18;
				legSlot = 197;
				vanity = true;
				value = buyPrice(0, 10);
				break;
			case 4707:
				width = 44;
				height = 44;
				rare = 2;
				value = buyPrice(0, 10);
				holdStyle = 2;
				useStyle = 3;
				useAnimation = 22;
				useTime = 22;
				damage = 15;
				knockBack = 5f;
				UseSound = SoundID.Item1;
				melee = true;
				break;
			case 4708:
				width = 18;
				height = 18;
				headSlot = 234;
				vanity = true;
				value = buyPrice(0, 10);
				break;
			case 4709:
				width = 18;
				height = 18;
				bodySlot = 221;
				vanity = true;
				value = buyPrice(0, 10);
				break;
			case 4710:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 573;
				width = 28;
				height = 28;
				break;
			case 4711:
				useStyle = 1;
				useTurn = true;
				useAnimation = 22;
				useTime = 14;
				autoReuse = true;
				width = 24;
				height = 28;
				damage = 12;
				UseSound = SoundID.Item1;
				knockBack = 3.5f;
				rare = 1;
				value = sellPrice(0, 0, 10);
				melee = true;
				break;
			case 4712:
				DefaultToPlaceableTile((ushort)467, 12);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 25));
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4713:
				DefaultToPlaceableTile((ushort)468, 12);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 5));
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 4714:
				width = 14;
				height = 20;
				maxStack = CommonMaxStack;
				rare = 8;
				break;
			case 4715:
				SetShopValues(ItemRarityColor.Pink5, buyPrice(0, 50));
				DefaultToGuitar();
				useAnimation = (useTime = 12);
				useTime /= 2;
				shoot = 856;
				damage = 85;
				magic = true;
				shootSpeed = 1f;
				crit = 20;
				knockBack = 1.5f;
				mana = 12;
				noMelee = true;
				break;
			case 4716:
				useStyle = 4;
				channel = true;
				width = 34;
				height = 34;
				UseSound = SoundID.Item43;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				mountType = 37;
				value = buyPrice(0, 20);
				break;
			case 4717:
			case 4718:
			case 4719:
			case 4720:
			case 4721:
				DefaultToPlaceableTile(574 + type - 4717);
				break;
			case 4722:
				useStyle = 5;
				width = 24;
				height = 24;
				noUseGraphic = true;
				UseSound = SoundID.Item1;
				autoReuse = true;
				melee = true;
				channel = true;
				noMelee = true;
				shoot = 857;
				useAnimation = 35;
				useTime = useAnimation / 5;
				shootSpeed = 16f;
				damage = 190;
				knockBack = 6.5f;
				value = sellPrice(0, 20);
				crit = 10;
				rare = 10;
				glowMask = 271;
				break;
			case 4723:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 70;
				break;
			case 4724:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 71;
				break;
			case 4725:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 44;
				break;
			case 4726:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 10;
				break;
			case 4727:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 11;
				break;
			case 4728:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 12;
				break;
			case 4729:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 246;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 20;
				break;
			case 4730:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 40;
				value = 400000;
				break;
			case 4731:
				DefaultToPlaceableTile((ushort)497, 39);
				maxStack = CommonMaxStack;
				value = 150;
				rare = 8;
				break;
			case 4732:
				width = 18;
				height = 18;
				headSlot = 235;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4733:
				width = 18;
				height = 18;
				bodySlot = 222;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4734:
				width = 18;
				height = 18;
				legSlot = 203;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4735:
				DefaultToVanitypet(858, 266);
				value = buyPrice(0, 50);
				break;
			case 4736:
				DefaultToVanitypet(859, 267);
				value = buyPrice(0, 30);
				break;
			case 4737:
				DefaultToVanitypet(860, 268);
				break;
			case 4738:
				width = 18;
				height = 18;
				headSlot = 236;
				rare = 2;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4739:
				width = 18;
				height = 18;
				bodySlot = 223;
				rare = 2;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4740:
				width = 18;
				height = 18;
				headSlot = 237;
				rare = 2;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4741:
				width = 18;
				height = 18;
				bodySlot = 224;
				rare = 2;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4742:
				width = 18;
				height = 18;
				legSlot = 205;
				rare = 2;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4743:
				useStyle = 1;
				shootSpeed = 11f;
				shoot = 861;
				damage = 0;
				width = 10;
				height = 10;
				maxStack = 1;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 20;
				break;
			case 4744:
				DefaultToAccessory(26, 36);
				backSlot = 24;
				frontSlot = 8;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 3));
				vanity = true;
				break;
			case 4745:
				DefaultToMount(38);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 20));
				break;
			case 4750:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 42;
				value = 400000;
				break;
			case 4751:
				width = 18;
				height = 18;
				headSlot = 239;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4752:
				width = 18;
				height = 18;
				bodySlot = 226;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4753:
				width = 18;
				height = 18;
				legSlot = 209;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4746:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 41;
				value = 400000;
				break;
			case 4747:
				width = 18;
				height = 18;
				headSlot = 238;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4748:
				width = 18;
				height = 18;
				bodySlot = 225;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4749:
				width = 18;
				height = 18;
				legSlot = 208;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4754:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 43;
				value = 400000;
				break;
			case 4755:
				width = 18;
				height = 18;
				headSlot = 240;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4756:
				width = 18;
				height = 18;
				bodySlot = 227;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4757:
				width = 18;
				height = 18;
				legSlot = 210;
				rare = 9;
				vanity = true;
				value = sellPrice(0, 5);
				break;
			case 4758:
				mana = 10;
				damage = 6;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 864;
				buffType = 271;
				width = 26;
				height = 28;
				UseSound = SoundID.Item44;
				useAnimation = 36;
				useTime = 36;
				rare = 5;
				noMelee = true;
				knockBack = 0f;
				value = sellPrice(0, 1);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				break;
			case 4759:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 11.5f;
				shoot = 865;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 1;
				noMelee = true;
				value = 20000;
				break;
			case 4760:
				damage = 80;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 866;
				width = 26;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 36;
				useTime = 36;
				rare = 5;
				noMelee = true;
				knockBack = 2f;
				value = buyPrice(0, 35);
				melee = true;
				noUseGraphic = true;
				break;
			case 4761:
				width = 12;
				height = 12;
				headSlot = 241;
				rare = 3;
				vanity = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 579;
				break;
			case 4762:
				width = 24;
				height = 24;
				accessory = true;
				vanity = true;
				rare = 1;
				value = buyPrice(0, 10);
				hasVanityEffects = true;
				break;
			case 4763:
				DefaultToMount(39);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 50));
				break;
			case 4764:
				noMelee = true;
				useStyle = 1;
				shootSpeed = 10.5f;
				shoot = 867;
				damage = 23;
				knockBack = 7f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				noUseGraphic = true;
				rare = 1;
				value = 30000;
				melee = true;
				break;
			case 4765:
			case 4766:
				DefaultToThrownWeapon(type - 4765 + 868, 20, 8f);
				UseSound = SoundID.Item106;
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 3));
				width = 18;
				height = 18;
				break;
			case 4767:
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 5));
				width = 30;
				height = 30;
				break;
			case 4768:
				width = 18;
				height = 18;
				headSlot = 242;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 3);
				break;
			case 4769:
				DefaultToAccessory(18, 18);
				backSlot = 25;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 3);
				break;
			case 4770:
				width = 18;
				height = 18;
				headSlot = 243;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 3);
				break;
			case 4771:
				DefaultToAccessory(18, 18);
				backSlot = 26;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 3);
				break;
			case 4772:
				width = 18;
				height = 18;
				headSlot = 244;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 3);
				break;
			case 4773:
				DefaultToAccessory(18, 18);
				backSlot = 27;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 3);
				break;
			case 4774:
				width = 18;
				height = 18;
				headSlot = 245;
				rare = 1;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 4775:
				DefaultToAccessory(18, 18);
				backSlot = 28;
				rare = 1;
				vanity = true;
				value = buyPrice(0, 3);
				break;
			case 4776:
				color = new Color(255, 255, 255, 0);
				useStyle = 1;
				shootSpeed = 6f;
				shoot = 870;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noMelee = true;
				rare = 1;
				value = buyPrice(0, 0, 0, 75);
				holdStyle = 1;
				break;
			case 4777:
				DefaultToVanitypet(875, 274);
				value = buyPrice(0, 50);
				break;
			case 4779:
				width = 18;
				height = 18;
				headSlot = 250;
				rare = 1;
				vanity = true;
				break;
			case 4780:
				width = 18;
				height = 18;
				bodySlot = 228;
				rare = 1;
				vanity = true;
				break;
			case 4781:
				width = 18;
				height = 18;
				legSlot = 211;
				rare = 1;
				vanity = true;
				break;
			case 4782:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 24;
				height = 24;
				rare = 6;
				expert = true;
				break;
			case 4784:
				width = 18;
				height = 18;
				headSlot = 251;
				rare = 1;
				value = sellPrice(0, 0, 75);
				vanity = true;
				break;
			case 4783:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				rare = 1;
				placeStyle = 72;
				break;
			case 4785:
			case 4786:
			case 4787:
				useStyle = 4;
				channel = true;
				width = 34;
				height = 34;
				UseSound = SoundID.Item76;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				mountType = 40 + (type - 4785);
				value = sellPrice(0, 5);
				break;
			case 4788:
				DefaultToSpear(877, 3.5f, 24);
				SetWeaponValues(60, 12f);
				SetShopValues(ItemRarityColor.LightRed4, buyPrice(0, 6));
				channel = true;
				break;
			case 4790:
				DefaultToSpear(879, 3.5f, 24);
				SetWeaponValues(90, 13f);
				SetShopValues(ItemRarityColor.Pink5, 230000);
				channel = true;
				break;
			case 4789:
				DefaultToSpear(878, 3.5f, 24);
				SetWeaponValues(130, 14f);
				SetShopValues(ItemRarityColor.Yellow8, sellPrice(0, 10));
				channel = true;
				break;
			case 4791:
				useStyle = 4;
				channel = true;
				width = 10;
				height = 32;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				noUseGraphic = true;
				mountType = 43;
				value = sellPrice(0, 5);
				break;
			case 4792:
			case 4793:
			case 4794:
			case 4795:
			case 4796:
				useStyle = 4;
				channel = true;
				width = 10;
				height = 32;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = -13;
				noMelee = true;
				noUseGraphic = true;
				mountType = 44 + type - 4792;
				value = sellPrice(0, 5);
				break;
			case 4797:
			case 4798:
			case 4799:
			case 4800:
			case 4801:
			case 4802:
			case 4803:
			case 4804:
			case 4805:
			case 4806:
			case 4807:
			case 4808:
			case 4809:
			case 4810:
			case 4811:
			case 4812:
			case 4813:
			case 4814:
			case 4815:
			case 4816:
			case 4817:
				DefaultToVanitypet(881 + type - 4797, 284 + type - 4797);
				value = buyPrice(0, 25);
				rare = -13;
				break;
			case 4818:
				noMelee = true;
				useStyle = 1;
				shootSpeed = 8.5f;
				shoot = 902;
				damage = 25;
				knockBack = 3.5f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				rare = 2;
				value = sellPrice(0, 0, 50);
				melee = true;
				break;
			case 4819:
				useTurn = true;
				width = 20;
				height = 20;
				useStyle = 4;
				useTime = 90;
				UseSound = SoundID.Item6;
				useAnimation = 90;
				SetShopValues(ItemRarityColor.LightRed4, buyPrice(0, 5));
				break;
			case 4820:
				useStyle = 1;
				useTurn = true;
				useAnimation = 12;
				useTime = 5;
				width = 20;
				height = 20;
				autoReuse = true;
				rare = 7;
				value = sellPrice(0, 10);
				tileBoost += 2;
				break;
			case 4821:
				useTurn = true;
				useStyle = 1;
				useTime = 21;
				useAnimation = 21;
				width = 24;
				height = 28;
				UseSound = SoundID.Item1;
				value = sellPrice(0, 5);
				autoReuse = true;
				rare = 3;
				scale = 0.85f;
				break;
			case 4822:
				DefaultToAccessory(34, 30);
				shoeSlot = 22;
				SetShopValues(ItemRarityColor.Orange3, sellPrice(0, 2));
				vanity = true;
				break;
			case 4823:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 44;
				value = buyPrice(0, 40);
				break;
			case 4824:
			case 4825:
			case 4826:
			case 4827:
				useStyle = 1;
				shootSpeed = 5f;
				shoot = 903 + (type - 4824);
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				UseSound = SoundID.Item1;
				consumable = true;
				useAnimation = 25;
				noUseGraphic = true;
				useTime = 25;
				value = sellPrice(0, 0, 5);
				rare = 1;
				break;
			case 4828:
				useStyle = 4;
				channel = true;
				width = 10;
				height = 32;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				noUseGraphic = true;
				mountType = 49;
				value = sellPrice(0, 5);
				break;
			case 4829:
			case 4830:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 5));
				break;
			case 4831:
			case 4832:
			case 4833:
			case 4834:
			case 4835:
			case 4836:
			case 4837:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				value = sellPrice(0, 0, 10);
				makeNPC = (short)(639 + (type - 4831));
				break;
			case 4838:
			case 4839:
			case 4840:
			case 4841:
			case 4842:
			case 4843:
			case 4844:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				value = sellPrice(0, 0, 10);
				makeNPC = (short)(646 + (type - 4838));
				break;
			case 4845:
				DefaultToCapturedCritter(653);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 20));
				width = 12;
				height = 12;
				bait = 15;
				break;
			case 4846:
				DefaultToPlaceableTile((ushort)580, 0);
				maxStack = CommonMaxStack;
				break;
			case 4847:
				DefaultToCapturedCritter(654);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 20));
				width = 12;
				height = 12;
				bait = 25;
				break;
			case 4848:
				DefaultToPlaceableTile((ushort)581, 0);
				maxStack = CommonMaxStack;
				break;
			case 4849:
				DefaultToCapturedCritter(655);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 50));
				width = 12;
				height = 12;
				bait = 35;
				break;
			case 4850:
				DefaultToPlaceableTile((ushort)582, 0);
				maxStack = CommonMaxStack;
				break;
			case 4851:
				DefaultToPlaceableTile((ushort)590, 0);
				value = sellPrice(0, 0, 7, 50);
				break;
			case 4852:
				DefaultToPlaceableTile((ushort)590, 1);
				value = sellPrice(0, 0, 3, 75);
				break;
			case 4853:
				DefaultToPlaceableTile((ushort)590, 2);
				value = sellPrice(0, 0, 11, 25);
				break;
			case 4854:
				DefaultToPlaceableTile((ushort)590, 3);
				value = sellPrice(0, 0, 15);
				break;
			case 4855:
				DefaultToPlaceableTile((ushort)590, 4);
				value = sellPrice(0, 0, 22, 50);
				break;
			case 4856:
				DefaultToPlaceableTile((ushort)590, 5);
				value = sellPrice(0, 0, 30);
				break;
			case 4857:
				DefaultToPlaceableTile((ushort)590, 6);
				value = sellPrice(0, 0, 30);
				break;
			case 4858:
			case 4859:
			case 4860:
			case 4861:
			case 4862:
			case 4863:
			case 4864:
			case 4865:
			case 4866:
				DefaultToPlaceableTile((ushort)591, type - 4858);
				value = sellPrice(0, 0, 25);
				break;
			case 4867:
				DefaultToPlaceableTile((ushort)592, 0);
				value = sellPrice(0, 0, 25);
				break;
			case 4868:
			case 4869:
				DefaultToPlaceableTile(593 + (type - 4868));
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4870:
				UseSound = SoundID.Item6;
				useStyle = 6;
				useTurn = true;
				useTime = (useAnimation = 30);
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				value = 1000;
				rare = 1;
				break;
			case 4871:
				DefaultToPlaceableTile((ushort)595, 0);
				value = buyPrice(0, 1);
				break;
			case 4872:
				useStyle = 1;
				useTurn = true;
				useAnimation = 12;
				useTime = 5;
				width = 20;
				height = 20;
				autoReuse = true;
				rare = 7;
				value = sellPrice(0, 10);
				tileBoost += 2;
				break;
			case 4873:
				width = 18;
				height = 18;
				defense = 1;
				headSlot = 254;
				rare = 5;
				value = 250000;
				break;
			case 4874:
				DefaultToAccessory(34, 30);
				shoeSlot = 23;
				SetShopValues(ItemRarityColor.Lime7, sellPrice(0, 12));
				hasVanityEffects = true;
				break;
			case 4876:
				DefaultToPlaceableTile((ushort)597, 0);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4875:
				DefaultToPlaceableTile((ushort)597, 1);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4877:
			case 4878:
				DefaultToPlaceableTile((ushort)376, 22 + type - 4877);
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 1));
				maxStack = CommonMaxStack;
				break;
			case 4879:
				width = 12;
				height = 12;
				rare = 2;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 2);
				break;
			case 4880:
				DefaultToPlaceableTile((ushort)598, 0);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				value = sellPrice(0, 0, 20);
				break;
			case 4881:
				DefaultToAccessory(34, 30);
				SetShopValues(ItemRarityColor.Lime7, sellPrice(0, 2));
				break;
			case 4882:
			case 4883:
			case 4884:
			case 4885:
			case 4886:
			case 4887:
			case 4888:
			case 4889:
			case 4890:
			case 4891:
			case 4892:
			case 4893:
			case 4894:
			case 4895:
				DefaultToPlaceableTile(599 + (type - 4882));
				maxStack = CommonMaxStack;
				break;
			case 4896:
				width = 18;
				height = 18;
				defense = 24;
				headSlot = 255;
				rare = 5;
				value = 250000;
				break;
			case 4897:
				width = 18;
				height = 18;
				defense = 9;
				headSlot = 256;
				rare = 5;
				value = 250000;
				break;
			case 4898:
				width = 18;
				height = 18;
				defense = 5;
				headSlot = 257;
				rare = 5;
				value = 250000;
				break;
			case 4899:
				width = 18;
				height = 18;
				defense = 1;
				headSlot = 258;
				rare = 5;
				value = 250000;
				break;
			case 4900:
				width = 18;
				height = 18;
				defense = 15;
				bodySlot = 229;
				rare = 5;
				value = 200000;
				break;
			case 4901:
				width = 18;
				height = 18;
				defense = 11;
				legSlot = 212;
				rare = 5;
				value = 150000;
				break;
			case 4902:
				DefaultToPlaceableTile((ushort)548, 7);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 3));
				break;
			case 4903:
				DefaultToPlaceableTile((ushort)548, 8);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 3));
				break;
			case 4904:
				DefaultToPlaceableTile((ushort)613, 0);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 3));
				break;
			case 4905:
				DefaultToPlaceableTile((ushort)613, 1);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 3));
				break;
			case 4906:
				DefaultToPlaceableTile((ushort)614, 0);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 3));
				break;
			case 4907:
				DefaultToPlaceableTile((ushort)615, 0);
				value = buyPrice(0, 1);
				break;
			case 4908:
			case 4909:
				useStyle = 1;
				shootSpeed = 5f;
				shoot = 910 + (type - 4908);
				width = 20;
				height = 20;
				maxStack = CommonMaxStack;
				UseSound = SoundID.Item1;
				consumable = true;
				useAnimation = 25;
				noUseGraphic = true;
				useTime = 25;
				value = sellPrice(0, 0, 1);
				rare = 1;
				break;
			case 4910:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 5));
				break;
			case 4911:
				DefaultToWhip(912, 45, 1.5f, 4f);
				SetShopValues(ItemRarityColor.LightRed4, sellPrice(0, 4));
				break;
			case 4912:
				DefaultToWhip(913, 37, 2f, 4f);
				SetShopValues(ItemRarityColor.LightRed4, sellPrice(0, 3));
				break;
			case 4913:
				DefaultToWhip(914, 18, 1.5f, 4f);
				SetShopValues(ItemRarityColor.Orange3, sellPrice(0, 1));
				break;
			case 4914:
				DefaultToWhip(915, 180, 4f, 4f);
				SetShopValues(ItemRarityColor.Yellow8, sellPrice(0, 5));
				break;
			case 4915:
				shootSpeed = 4.5f;
				shoot = 14;
				damage = 9;
				width = 8;
				height = 8;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Bullet;
				knockBack = 4f;
				value = 18;
				ranged = true;
				break;
			case 4916:
				DefaultToPlaceableTile((ushort)597, 2);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4917:
				DefaultToPlaceableTile((ushort)597, 3);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4918:
				DefaultToPlaceableTile((ushort)597, 4);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4919:
				DefaultToPlaceableTile((ushort)597, 5);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4920:
				DefaultToPlaceableTile((ushort)597, 6);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4921:
				DefaultToPlaceableTile((ushort)597, 7);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 10));
				break;
			case 4922:
				DefaultToPlaceableTile((ushort)207, 8);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 4));
				width = 8;
				height = 10;
				break;
			case 4923:
				width = 14;
				height = 38;
				rare = 8;
				noUseGraphic = true;
				channel = true;
				noMelee = true;
				damage = 80;
				crit = 10;
				knockBack = 4f;
				autoReuse = false;
				noMelee = true;
				melee = true;
				shoot = 927;
				shootSpeed = 15f;
				value = sellPrice(0, 5);
				useStyle = 13;
				useAnimation = 18;
				useTime = 6;
				break;
			case 4924:
			case 4925:
			case 4926:
			case 4927:
			case 4928:
			case 4929:
			case 4930:
			case 4931:
			case 4932:
			case 4933:
			case 4934:
			case 4935:
			case 4936:
			case 4937:
			case 4938:
			case 4939:
			case 4940:
			case 4941:
			case 4942:
			case 4943:
			case 4944:
			case 4945:
			case 4946:
			case 4947:
			case 4948:
			case 4949:
			case 4950:
				DefaultToPlaceableTile((ushort)617, type - 4924);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 5));
				rare = -13;
				maxStack = CommonMaxStack;
				break;
			case 4951:
				DefaultToPlaceableTile((ushort)597, 8);
				SetShopValues(ItemRarityColor.Cyan9, buyPrice(1));
				break;
			case 4954:
				width = 24;
				height = 8;
				accessory = true;
				rare = 9;
				wingSlot = 45;
				value = sellPrice(0, 10);
				expert = true;
				break;
			case 4952:
				autoReuse = true;
				useStyle = 14;
				holdStyle = 6;
				scale = 0.7f;
				useAnimation = 36;
				useTime = 2;
				width = 36;
				height = 22;
				shoot = 931;
				mana = 23;
				UseSound = SoundID.Item82;
				knockBack = 2.5f;
				damage = 50;
				shootSpeed = 17f;
				noMelee = true;
				rare = 8;
				magic = true;
				value = sellPrice(0, 5);
				flame = true;
				break;
			case 4953:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 30;
				useTime = 2;
				width = 50;
				height = 18;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 50;
				shootSpeed = 10f;
				noMelee = true;
				value = sellPrice(0, 5);
				ranged = true;
				rare = 8;
				knockBack = 2f;
				break;
			case 4955:
				width = 18;
				height = 18;
				headSlot = 259;
				value = buyPrice(0, 15);
				rare = 5;
				vanity = true;
				break;
			case 4956:
				useStyle = 1;
				width = 24;
				height = 24;
				UseSound = null;
				autoReuse = true;
				melee = true;
				melee = true;
				shoot = 933;
				useAnimation = 30;
				useTime = useAnimation / 3;
				shootSpeed = 16f;
				damage = 190;
				knockBack = 6.5f;
				value = sellPrice(0, 20);
				crit = 10;
				rare = 10;
				noUseGraphic = true;
				noMelee = true;
				break;
			case 4957:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 24;
				height = 24;
				rare = 6;
				expert = true;
				break;
			case 4958:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				rare = 1;
				placeStyle = 73;
				break;
			case 4959:
				width = 18;
				height = 18;
				headSlot = 260;
				rare = 1;
				value = sellPrice(0, 0, 75);
				vanity = true;
				break;
			case 4960:
				DefaultToVanitypet(934, 317);
				value = buyPrice(0, 25);
				rare = -13;
				break;
			case 4961:
				DefaultToCapturedCritter(661);
				SetShopValues(ItemRarityColor.Orange3, sellPrice(0, 5));
				width = 12;
				height = 12;
				break;
			case 4962:
				DefaultToPlaceableTile((ushort)618, 0);
				break;
			case 4963:
				DefaultToPlaceableTile((ushort)619, 0);
				maxStack = CommonMaxStack;
				break;
			case 4964:
				DefaultToPlaceableTile((ushort)620, 0);
				maxStack = CommonMaxStack;
				break;
			case 4965:
			case 4966:
			case 4967:
			case 4968:
			case 4969:
			case 4970:
			case 4971:
			case 4972:
			case 4973:
			case 4974:
			case 4975:
			case 4976:
			case 4977:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 297 + (type - 4965);
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				break;
			case 4978:
				width = 24;
				height = 8;
				accessory = true;
				rare = 0;
				wingSlot = 46;
				value = 2000;
				break;
			case 4979:
				DefaultToMusicBox(52);
				break;
			case 4980:
				noUseGraphic = true;
				damage = 0;
				knockBack = 7f;
				useStyle = 5;
				shootSpeed = 4f;
				shoot = 935;
				width = 18;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 20;
				useTime = 20;
				rare = 5;
				noMelee = true;
				value = sellPrice(0, 5);
				break;
			case 4981:
				useStyle = 4;
				channel = true;
				width = 10;
				height = 32;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				noUseGraphic = true;
				mountType = 50;
				value = sellPrice(0, 5);
				break;
			case 4982:
				width = 18;
				height = 18;
				defense = 12;
				headSlot = 261;
				rare = 5;
				value = sellPrice(0, 2);
				break;
			case 4983:
				width = 18;
				height = 18;
				defense = 14;
				bodySlot = 230;
				rare = 5;
				value = sellPrice(0, 2);
				break;
			case 4984:
				width = 18;
				height = 18;
				defense = 10;
				legSlot = 213;
				rare = 5;
				value = sellPrice(0, 2);
				break;
			case 4985:
				DefaultToMusicBox(53);
				break;
			case 4986:
				useStyle = 1;
				shootSpeed = 9f;
				shoot = 936;
				width = 18;
				height = 20;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				useTime = 15;
				noUseGraphic = true;
				noMelee = true;
				value = 200;
				break;
			case 4987:
				width = 16;
				height = 24;
				accessory = true;
				rare = 6;
				value = sellPrice(0, 5);
				expert = true;
				break;
			case 4988:
				useStyle = 4;
				width = 20;
				height = 20;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				maxStack = CommonMaxStack;
				rare = 6;
				value = sellPrice(0, 1);
				break;
			case 4989:
				width = 22;
				height = 22;
				accessory = true;
				rare = 1;
				value = sellPrice(0, 10);
				expert = true;
				break;
			case 4990:
				DefaultToMusicBox(54);
				break;
			case 4991:
				DefaultToMusicBox(55);
				break;
			case 4992:
				DefaultToMusicBox(56);
				break;
			case 4993:
				DefaultToPlaceableTile((ushort)89, 43);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 50));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				rare = 9;
				break;
			case 4994:
				width = 18;
				height = 14;
				headSlot = 262;
				rare = 2;
				value = buyPrice(0, 10);
				vanity = true;
				break;
			case 4995:
				width = 18;
				height = 14;
				headSlot = 263;
				rare = 2;
				value = buyPrice(0, 10);
				vanity = true;
				break;
			case 4996:
				width = 18;
				height = 14;
				headSlot = 264;
				rare = 2;
				value = buyPrice(0, 10);
				vanity = true;
				break;
			case 4997:
				width = 18;
				height = 14;
				vanity = true;
				bodySlot = 231;
				value = buyPrice(0, 10);
				rare = 2;
				break;
			case 4998:
				width = 18;
				height = 14;
				vanity = true;
				bodySlot = 232;
				value = buyPrice(0, 10);
				rare = 2;
				break;
			case 4999:
				width = 18;
				height = 14;
				vanity = true;
				bodySlot = 233;
				value = buyPrice(0, 10);
				rare = 2;
				break;
			case 5000:
				DefaultToAccessory(34, 30);
				shoeSlot = 24;
				SetShopValues(ItemRarityColor.Lime7, sellPrice(0, 15));
				hasVanityEffects = true;
				break;
			case 5002:
			case 5003:
				DefaultToPlaceableTile((ushort)376, 24 + type - 5002);
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 1));
				maxStack = CommonMaxStack;
				break;
			case 5005:
				mana = 10;
				damage = 90;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 946;
				buffType = 322;
				width = 26;
				height = 28;
				UseSound = SoundID.Item82;
				useAnimation = 36;
				useTime = 36;
				rare = 5;
				noMelee = true;
				knockBack = 4f;
				value = sellPrice(0, 20);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				break;
			case 5004:
				width = 18;
				height = 14;
				headSlot = 265;
				rare = 9;
				value = sellPrice(0, 0, 30);
				vanity = true;
				break;
			case 5006:
				DefaultToMusicBox(57);
				break;
			case 5007:
				width = 18;
				height = 18;
				bodySlot = 234;
				rare = 2;
				value = sellPrice(0, 0, 20);
				vanity = true;
				break;
			case 5008:
				DefaultToPlaceableTile((ushort)622, 0);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 2));
				break;
			case 5001:
				width = 18;
				height = 14;
				defense = 3;
				legSlot = 217;
				value = buyPrice(0, 10);
				rare = 2;
				break;
			case 5010:
				DefaultToAccessory(34, 30);
				waistSlot = 16;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 3));
				break;
			case 5011:
			case 5012:
				noMelee = true;
				useStyle = 5;
				useAnimation = 45;
				useTime = 45;
				knockBack = 4.6f;
				width = 28;
				height = 28;
				damage = 9;
				scale = 1f;
				noUseGraphic = true;
				shoot = 947;
				if (type == 5012)
				{
					shoot = 948;
				}
				shootSpeed = 11f;
				UseSound = SoundID.Item1;
				rare = 1;
				value = sellPrice(0, 2);
				melee = true;
				channel = true;
				break;
			case 5014:
				DefaultToMusicBox(58);
				break;
			case 5015:
				DefaultToMusicBox(59);
				break;
			case 5016:
				DefaultToMusicBox(60);
				break;
			case 5017:
				DefaultToMusicBox(61);
				break;
			case 5018:
				DefaultToMusicBox(62);
				break;
			case 5019:
				DefaultToMusicBox(63);
				break;
			case 5020:
				DefaultToMusicBox(64);
				break;
			case 5021:
				DefaultToMusicBox(65);
				break;
			case 5022:
				DefaultToMusicBox(66);
				break;
			case 5023:
				DefaultToMusicBox(67);
				break;
			case 5024:
				DefaultToMusicBox(68);
				break;
			case 5025:
				DefaultToMusicBox(69);
				break;
			case 5026:
				DefaultToMusicBox(70);
				break;
			case 5027:
				DefaultToMusicBox(71);
				break;
			case 5028:
				DefaultToMusicBox(72);
				break;
			case 5029:
				DefaultToMusicBox(73);
				break;
			case 5030:
				DefaultToMusicBox(74);
				break;
			case 5031:
				DefaultToMusicBox(75);
				break;
			case 5032:
				DefaultToMusicBox(76);
				break;
			case 5033:
				DefaultToMusicBox(77);
				break;
			case 5034:
				DefaultToMusicBox(78);
				break;
			case 5035:
				DefaultToMusicBox(79);
				break;
			case 5036:
				DefaultToMusicBox(80);
				break;
			case 5037:
				DefaultToMusicBox(81);
				break;
			case 5038:
				DefaultToMusicBox(82);
				break;
			case 5039:
				DefaultToMusicBox(83);
				break;
			case 5040:
				DefaultToMusicBox(84);
				break;
			case 5043:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 18;
				height = 18;
				useStyle = 4;
				useTime = 30;
				UseSound = SoundID.Item4;
				useAnimation = 30;
				rare = 4;
				value = sellPrice(0, 2);
				break;
			case 5044:
				DefaultToMusicBox(85);
				break;
			case 5045:
				width = 18;
				height = 14;
				headSlot = 266;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5046:
				width = 18;
				height = 14;
				bodySlot = 235;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5047:
				width = 18;
				height = 14;
				legSlot = 218;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5048:
				width = 18;
				height = 14;
				headSlot = 267;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5049:
				width = 18;
				height = 14;
				bodySlot = 236;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5050:
				width = 18;
				height = 14;
				legSlot = 219;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5051:
				width = 18;
				height = 14;
				headSlot = 268;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5052:
				width = 18;
				height = 14;
				bodySlot = 237;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5053:
				width = 18;
				height = 14;
				legSlot = 222;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5054:
				width = 18;
				height = 14;
				headSlot = 269;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5055:
				width = 18;
				height = 14;
				bodySlot = 238;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5056:
				width = 18;
				height = 14;
				legSlot = 224;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5057:
				width = 18;
				height = 14;
				headSlot = 270;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5058:
				width = 18;
				height = 14;
				bodySlot = 239;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5059:
				width = 18;
				height = 14;
				legSlot = 225;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5060:
				width = 18;
				height = 14;
				legSlot = 226;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5061:
				width = 18;
				height = 14;
				headSlot = 271;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5062:
				width = 18;
				height = 14;
				bodySlot = 240;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5063:
				width = 18;
				height = 14;
				legSlot = 227;
				rare = 2;
				value = sellPrice(0, 0, 1);
				vanity = true;
				break;
			case 5064:
				width = 26;
				height = 30;
				maxStack = 1;
				value = sellPrice(0, 4);
				rare = 8;
				accessory = true;
				backSlot = 33;
				break;
			case 5065:
				DefaultToStaff(950, 10f, 25, 18);
				SetWeaponValues(70, 5f);
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				break;
			case 5066:
				DefaultToPlaceableTile((ushort)444, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 50));
				break;
			case 5067:
				DefaultToPlaceableTile((ushort)485, 0);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 50));
				break;
			case 5068:
				width = 18;
				height = 18;
				bodySlot = 241;
				defense = 1;
				rare = 2;
				value = sellPrice(0, 2, 50);
				break;
			case 5069:
				mana = 5;
				damage = 8;
				useStyle = 1;
				shootSpeed = 10f;
				shoot = 951;
				buffType = 325;
				width = 26;
				height = 28;
				UseSound = SoundID.Item44;
				useAnimation = 36;
				useTime = 36;
				rare = 3;
				noMelee = true;
				knockBack = 2f;
				value = sellPrice(0, 0, 50);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				break;
			case 5070:
				width = 16;
				height = 16;
				maxStack = CommonMaxStack;
				value = buyPrice(0, 0, 5);
				rare = 1;
				break;
			case 5071:
				width = 18;
				height = 14;
				headSlot = 272;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				vanity = true;
				break;
			case 5072:
				width = 18;
				height = 14;
				bodySlot = 242;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				vanity = true;
				break;
			case 5073:
				width = 18;
				height = 14;
				legSlot = 228;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				vanity = true;
				break;
			case 5074:
				DefaultToWhip(952, 27, 2f, 5f);
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 1, 50));
				break;
			case 5075:
				width = 24;
				height = 24;
				accessory = true;
				vanity = true;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 1));
				hasVanityEffects = true;
				break;
			case 5076:
				width = 16;
				height = 24;
				accessory = true;
				vanity = true;
				balloonSlot = 18;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 20));
				hasVanityEffects = true;
				break;
			case 5077:
				width = 16;
				height = 16;
				accessory = true;
				vanity = true;
				shoeSlot = 25;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 20));
				break;
			case 5078:
				width = 18;
				height = 14;
				bodySlot = 243;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				vanity = true;
				break;
			case 5079:
				width = 18;
				height = 14;
				legSlot = 230;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				vanity = true;
				break;
			case 5080:
				width = 16;
				height = 16;
				accessory = true;
				vanity = true;
				frontSlot = 11;
				backSlot = 34;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 10));
				break;
			case 5081:
				DefaultToPlaceableTile((ushort)623, 0);
				SetShopValues(ItemRarityColor.Pink5, buyPrice(0, 10));
				break;
			case 5082:
				DefaultToPlaceableTile((ushort)623, 1);
				SetShopValues(ItemRarityColor.Pink5, buyPrice(0, 10));
				break;
			case 5083:
				DefaultToPlaceableTile((ushort)623, 2);
				SetShopValues(ItemRarityColor.Pink5, buyPrice(0, 10));
				break;
			case 5084:
				DefaultToPlaceableTile((ushort)623, 3);
				SetShopValues(ItemRarityColor.Pink5, buyPrice(0, 10));
				break;
			case 5085:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 2);
				placeStyle = 45;
				break;
			case 5086:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 2);
				placeStyle = 13;
				break;
			case 5087:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 2);
				placeStyle = 74;
				break;
			case 5088:
				DefaultToVanitypet(956, 327);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 25));
				value = buyPrice(0, 25);
				break;
			case 5089:
				DefaultToVanitypet(957, 328);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 25));
				value = buyPrice(0, 25);
				break;
			case 5090:
				DefaultToVanitypet(958, 329);
				value = buyPrice(0, 25);
				rare = -13;
				break;
			case 5091:
				DefaultToVanitypet(959, 330);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 5));
				break;
			case 5094:
				useStyle = 1;
				useTurn = false;
				useAnimation = 21;
				useTime = 21;
				width = 24;
				height = 28;
				damage = 20;
				knockBack = 5.5f;
				UseSound = SoundID.Item1;
				scale = 1.2f;
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 0, 50));
				melee = true;
				break;
			case 5095:
				useStyle = 1;
				autoReuse = true;
				useAnimation = 15;
				useTime = 15;
				width = 24;
				height = 28;
				damage = 27;
				knockBack = 5f;
				UseSound = SoundID.Item1;
				scale = 1.2f;
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 1, 50));
				melee = true;
				axe = 30;
				crit = 10;
				break;
			case 5096:
				useStyle = 1;
				useTurn = true;
				autoReuse = true;
				useAnimation = 20;
				useTime = 20;
				width = 24;
				height = 28;
				damage = 57;
				knockBack = 6.5f;
				UseSound = SoundID.Item1;
				scale = 1.2f;
				SetShopValues(ItemRarityColor.LightRed4, sellPrice(0, 1));
				melee = true;
				break;
			case 5097:
				useStyle = 1;
				useTurn = false;
				useAnimation = 45;
				useTime = 45;
				width = 24;
				height = 28;
				damage = 36;
				knockBack = 5.5f;
				UseSound = SoundID.Item1;
				scale = 1.15f;
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 0, 25));
				melee = true;
				break;
			case 5098:
				DefaultToVanitypet(960, 331);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 10));
				break;
			case 5099:
				width = 18;
				height = 14;
				headSlot = 273;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 1));
				vanity = true;
				break;
			case 5100:
				width = 18;
				height = 14;
				faceSlot = 19;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 2));
				accessory = true;
				expert = true;
				break;
			case 5101:
				width = 28;
				height = 20;
				headSlot = 275;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 50));
				vanity = true;
				break;
			case 5102:
				width = 18;
				height = 14;
				bodySlot = 244;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 40));
				vanity = true;
				break;
			case 5103:
				width = 18;
				height = 14;
				legSlot = 231;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 40));
				vanity = true;
				break;
			case 5104:
			case 5105:
			case 5106:
				width = 18;
				height = 14;
				maxStack = 1;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				beardSlot = (sbyte)(2 + (type - 5104));
				color = Main.player[Main.myPlayer].hairColor;
				accessory = true;
				vanity = true;
				break;
			case 5107:
				DefaultToAccessory(26, 30);
				neckSlot = 11;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				break;
			case 5109:
				width = 18;
				height = 18;
				headSlot = 276;
				rare = 1;
				value = sellPrice(0, 0, 75);
				vanity = true;
				break;
			case 5108:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 1);
				rare = 1;
				placeStyle = 75;
				break;
			case 5110:
				DefaultToPlaceableTile((ushort)617, 27);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 5));
				rare = -13;
				maxStack = CommonMaxStack;
				break;
			case 5111:
				maxStack = CommonMaxStack;
				consumable = true;
				width = 24;
				height = 24;
				rare = 3;
				expert = true;
				break;
			case 5112:
				DefaultToMusicBox(86);
				break;
			case 5113:
				DefaultToAccessory(26, 30);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				vanity = true;
				hasVanityEffects = true;
				break;
			case 5114:
				mana = 10;
				damage = 6;
				useStyle = 4;
				shootSpeed = 10f;
				shoot = 970;
				buffType = 335;
				width = 26;
				height = 28;
				UseSound = SoundID.AbigailSummon;
				useAnimation = 36;
				useTime = 36;
				rare = 3;
				noMelee = true;
				knockBack = 2f;
				value = sellPrice(0, 0, 50);
				summon = true;
				autoReuse = true;
				reuseDelay = 2;
				break;
			case 5115:
				width = 18;
				height = 14;
				bodySlot = 245;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 40));
				vanity = true;
				break;
			case 5116:
				width = 18;
				height = 14;
				legSlot = 232;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 40));
				vanity = true;
				break;
			case 5117:
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 1, 50));
				autoReuse = true;
				useStyle = 5;
				useAnimation = 15;
				useTime = 15;
				width = 24;
				height = 24;
				shoot = 968;
				UseSound = SoundID.Item61;
				useAmmo = AmmoID.Bullet;
				damage = 20;
				shootSpeed = 14f;
				knockBack = 1f;
				ranged = true;
				noMelee = true;
				break;
			case 5118:
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 1, 50));
				useStyle = 5;
				useAnimation = 45;
				useTime = 45;
				width = 24;
				height = 24;
				shoot = 969;
				UseSound = SoundID.Item66;
				damage = 13;
				shootSpeed = 1f;
				knockBack = 1f;
				magic = true;
				noMelee = true;
				mana = 30;
				break;
			case 5119:
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 1, 50));
				useStyle = 1;
				shootSpeed = 14f;
				shoot = 966;
				damage = 24;
				width = 18;
				height = 20;
				UseSound = SoundID.Item1;
				useAnimation = 30;
				useTime = 30;
				noMelee = true;
				knockBack = 7.5f;
				summon = true;
				mana = 20;
				sentry = true;
				break;
			case 5120:
				SetShopValues(ItemRarityColor.Blue1, 0);
				useStyle = 4;
				width = 22;
				height = 14;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				maxStack = CommonMaxStack;
				break;
			case 5121:
			case 5122:
			case 5123:
			case 5124:
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 20));
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				placeStyle = 46 + (type - 5121);
				break;
			case 5125:
				DefaultToMount(51);
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 0, 50));
				break;
			case 5126:
				DefaultToAccessory(26, 30);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 8));
				backSlot = 35;
				handOnSlot = 23;
				handOffSlot = 15;
				break;
			case 5131:
				DefaultToVanitypet(881, 341);
				value = buyPrice(0, 25);
				rare = -13;
				break;
			case 5127:
				DefaultToPlaceableTile((ushort)625, 0);
				SetShopValues(ItemRarityColor.Blue1, 0);
				break;
			case 5128:
				DefaultToPlaceableTile((ushort)627, 0);
				SetShopValues(ItemRarityColor.Blue1, 0);
				break;
			case 5132:
				DefaultToCapturedCritter(669);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 10));
				width = 20;
				height = 20;
				bait = 10;
				break;
			case 5133:
				DefaultToPlaceableTile((ushort)629, 0);
				maxStack = CommonMaxStack;
				break;
			case 5147:
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 15));
				useStyle = 1;
				useAnimation = 26;
				useTime = 26;
				width = 26;
				height = 28;
				shoot = 979;
				UseSound = SoundID.Item8;
				damage = 15;
				shootSpeed = 7f;
				magic = true;
				noMelee = true;
				mana = 2;
				crit = 10;
				if (Variant == ItemVariants.StrongerVariant)
				{
					value = sellPrice(0, 5, 5);
					rare = 4;
					damage = 42;
					useAnimation = 10;
					useTime = 10;
					mana = 6;
					shootSpeed = 12f;
					autoReuse = true;
				}
				break;
			case 5211:
				UseSound = SoundID.Item3;
				useStyle = 9;
				useTurn = true;
				useAnimation = 17;
				useTime = 17;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 14;
				height = 24;
				buffType = 343;
				buffTime = 18000;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 0, 2));
				break;
			case 5135:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 137;
				placeStyle = 5;
				width = 12;
				height = 12;
				mech = true;
				SetShopValues(ItemRarityColor.LightRed4, sellPrice(0, 0, 60));
				break;
			case 5212:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				makeNPC = 671;
				break;
			case 5213:
				DefaultToPlaceableTile((ushort)632, 0);
				maxStack = CommonMaxStack;
				break;
			case 5136:
				width = 18;
				height = 18;
				headSlot = 274;
				color = Main.player[Main.myPlayer].skinColor;
				rare = 1;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 5134:
				useStyle = 5;
				autoReuse = true;
				useAnimation = 30;
				useTime = 5;
				width = 50;
				height = 18;
				shoot = 145;
				useAmmo = AmmoID.Solution;
				UseSound = SoundID.Item34;
				knockBack = 0.3f;
				shootSpeed = 11f;
				noMelee = true;
				SetShopValues(ItemRarityColor.Green2, buyPrice(2));
				rare = 10;
				break;
			case 5214:
				autoReuse = true;
				useTurn = true;
				useStyle = 1;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 633;
				width = 14;
				height = 14;
				value = 150;
				break;
			case 5215:
				DefaultToPlaceableTile((ushort)635, 0);
				break;
			case 5216:
				DefaultToPlaceableWall(316);
				break;
			case 5217:
				DefaultToPlaceableWall(317);
				break;
			case 5148:
				DefaultToPlaceableTile((ushort)90, 40);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5149:
				DefaultToPlaceableTile((ushort)79, 40);
				SetShopValues(ItemRarityColor.White0, 2000);
				maxStack = CommonMaxStack;
				width = 28;
				height = 20;
				break;
			case 5150:
				DefaultToPlaceableTile((ushort)101, 41);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5151:
				DefaultToPlaceableTile((ushort)88, 40);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5152:
				DefaultToPlaceableTile((ushort)100, 40);
				SetShopValues(ItemRarityColor.White0, 1500);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5153:
				DefaultToPlaceableTile((ushort)33, 39);
				SetShopValues(ItemRarityColor.White0, 0);
				maxStack = CommonMaxStack;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				noWet = true;
				break;
			case 5154:
				DefaultToPlaceableTile((ushort)15, 45);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 30;
				break;
			case 5155:
				DefaultToPlaceableTile((ushort)34, 46);
				SetShopValues(ItemRarityColor.White0, 3000);
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 5156:
				DefaultToPlaceableTile((ushort)467, 14);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 5157:
				DefaultToPlaceableTile((ushort)104, 41);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5158:
				DefaultToPlaceableTile((ushort)10, 46);
				SetShopValues(ItemRarityColor.White0, 200);
				maxStack = CommonMaxStack;
				width = 14;
				height = 28;
				break;
			case 5159:
				DefaultToPlaceableTile((ushort)93, 40);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 10;
				height = 24;
				break;
			case 5160:
				DefaultToPlaceableTile((ushort)42, 46);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 28;
				break;
			case 5161:
				DefaultToPlaceableTile((ushort)87, 40);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5162:
				DefaultToPlaceableTile((ushort)19, 45);
				SetShopValues(ItemRarityColor.White0, 0);
				width = 8;
				height = 10;
				break;
			case 5163:
				DefaultToPlaceableTile((ushort)172, 41);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5164:
				DefaultToPlaceableTile((ushort)89, 44);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5165:
				DefaultToPlaceableTile((ushort)469, 9);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 26;
				height = 20;
				break;
			case 5166:
				DefaultToPlaceableTile((ushort)18, 41);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 28;
				height = 14;
				break;
			case 5168:
				DefaultToPlaceableTile((ushort)497, 40);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 5167:
				DefaultToPlaceableTile((ushort)468, 14);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 5169:
				DefaultToPlaceableTile((ushort)90, 41);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5170:
				DefaultToPlaceableTile((ushort)79, 41);
				SetShopValues(ItemRarityColor.White0, 2000);
				maxStack = CommonMaxStack;
				width = 28;
				height = 20;
				break;
			case 5171:
				DefaultToPlaceableTile((ushort)101, 42);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5172:
				DefaultToPlaceableTile((ushort)88, 41);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5173:
				DefaultToPlaceableTile((ushort)100, 41);
				SetShopValues(ItemRarityColor.White0, 1500);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5174:
				DefaultToPlaceableTile((ushort)33, 40);
				SetShopValues(ItemRarityColor.White0, 0);
				maxStack = CommonMaxStack;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				noWet = true;
				break;
			case 5175:
				DefaultToPlaceableTile((ushort)15, 46);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 30;
				break;
			case 5176:
				DefaultToPlaceableTile((ushort)34, 47);
				SetShopValues(ItemRarityColor.White0, 3000);
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 5177:
				DefaultToPlaceableTile((ushort)467, 15);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 5178:
				DefaultToPlaceableTile((ushort)104, 42);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5179:
				DefaultToPlaceableTile((ushort)10, 47);
				SetShopValues(ItemRarityColor.White0, 200);
				maxStack = CommonMaxStack;
				width = 14;
				height = 28;
				break;
			case 5180:
				DefaultToPlaceableTile((ushort)93, 41);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 10;
				height = 24;
				break;
			case 5181:
				DefaultToPlaceableTile((ushort)42, 47);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 28;
				break;
			case 5182:
				DefaultToPlaceableTile((ushort)87, 41);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5183:
				DefaultToPlaceableTile((ushort)19, 46);
				SetShopValues(ItemRarityColor.White0, 0);
				width = 8;
				height = 10;
				break;
			case 5184:
				DefaultToPlaceableTile((ushort)172, 42);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5185:
				DefaultToPlaceableTile((ushort)89, 45);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5186:
				DefaultToPlaceableTile((ushort)469, 10);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 26;
				height = 20;
				break;
			case 5187:
				DefaultToPlaceableTile((ushort)18, 42);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 28;
				height = 14;
				break;
			case 5189:
				DefaultToPlaceableTile((ushort)497, 41);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 5188:
				DefaultToPlaceableTile((ushort)468, 15);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 5190:
				DefaultToPlaceableTile((ushort)90, 42);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5191:
				DefaultToPlaceableTile((ushort)79, 42);
				SetShopValues(ItemRarityColor.White0, 2000);
				maxStack = CommonMaxStack;
				width = 28;
				height = 20;
				break;
			case 5192:
				DefaultToPlaceableTile((ushort)101, 43);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5193:
				DefaultToPlaceableTile((ushort)88, 42);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5194:
				DefaultToPlaceableTile((ushort)100, 42);
				SetShopValues(ItemRarityColor.White0, 1500);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5195:
				DefaultToPlaceableTile((ushort)33, 41);
				SetShopValues(ItemRarityColor.White0, 0);
				maxStack = CommonMaxStack;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 0, 60);
				noWet = true;
				break;
			case 5196:
				DefaultToPlaceableTile((ushort)15, 47);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 30;
				break;
			case 5197:
				DefaultToPlaceableTile((ushort)34, 48);
				SetShopValues(ItemRarityColor.White0, 3000);
				maxStack = CommonMaxStack;
				width = 26;
				height = 26;
				break;
			case 5198:
				DefaultToPlaceableTile((ushort)467, 16);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 5199:
				DefaultToPlaceableTile((ushort)104, 43);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5200:
				DefaultToPlaceableTile((ushort)10, 48);
				SetShopValues(ItemRarityColor.White0, 200);
				maxStack = CommonMaxStack;
				width = 14;
				height = 28;
				break;
			case 5201:
				DefaultToPlaceableTile((ushort)93, 42);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 10;
				height = 24;
				break;
			case 5202:
				DefaultToPlaceableTile((ushort)42, 48);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 12;
				height = 28;
				break;
			case 5203:
				DefaultToPlaceableTile((ushort)87, 42);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5204:
				DefaultToPlaceableTile((ushort)19, 47);
				SetShopValues(ItemRarityColor.White0, 0);
				width = 8;
				height = 10;
				break;
			case 5205:
				DefaultToPlaceableTile((ushort)172, 43);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5206:
				DefaultToPlaceableTile((ushort)89, 46);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5207:
				DefaultToPlaceableTile((ushort)469, 11);
				SetShopValues(ItemRarityColor.White0, 300);
				maxStack = CommonMaxStack;
				width = 26;
				height = 20;
				break;
			case 5208:
				DefaultToPlaceableTile((ushort)18, 43);
				SetShopValues(ItemRarityColor.White0, 150);
				maxStack = CommonMaxStack;
				width = 28;
				height = 14;
				break;
			case 5210:
				DefaultToPlaceableTile((ushort)497, 42);
				maxStack = CommonMaxStack;
				value = 150;
				break;
			case 5209:
				DefaultToPlaceableTile((ushort)468, 16);
				SetShopValues(ItemRarityColor.White0, 500);
				maxStack = CommonMaxStack;
				width = 26;
				height = 22;
				break;
			case 5130:
				useStyle = 4;
				channel = true;
				width = 10;
				height = 32;
				UseSound = SoundID.Item25;
				useAnimation = 20;
				useTime = 20;
				rare = 8;
				noMelee = true;
				noUseGraphic = true;
				mountType = 52;
				value = sellPrice(0, 5);
				break;
			case 5137:
				DefaultToPlaceableTile((ushort)630, 0);
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 0, 10));
				maxStack = CommonMaxStack;
				break;
			case 5138:
				DefaultToPlaceableTile((ushort)631, 0);
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 0, 10));
				maxStack = CommonMaxStack;
				break;
			case 5218:
			case 5219:
			case 5220:
			case 5221:
			case 5222:
			case 5223:
			case 5224:
			case 5225:
			case 5226:
			case 5227:
			case 5228:
			case 5229:
			case 5230:
			case 5231:
			case 5232:
			case 5233:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 76 + (type - 5218);
				if (type == 5231)
				{
					value = sellPrice(0, 0, 20);
				}
				if (type == 5228)
				{
					value = sellPrice(0, 2);
				}
				if (type == 5222)
				{
					value = sellPrice(0, 2);
				}
				break;
			case 5234:
			case 5235:
			case 5236:
			case 5237:
			case 5238:
			case 5239:
			case 5240:
			case 5241:
			case 5242:
			case 5243:
			case 5244:
			case 5245:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 50 + (type - 5234);
				if (type == 5243)
				{
					value = sellPrice(0, 0, 60);
				}
				if (type == 5245)
				{
					value = sellPrice(0, 0, 20);
				}
				if (type == 5235)
				{
					value = sellPrice(0, 0, 50);
				}
				break;
			case 5246:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 14;
				break;
			case 5247:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 15;
				break;
			case 5248:
			case 5249:
			case 5250:
			case 5251:
			case 5252:
			case 5253:
			case 5254:
			case 5255:
			case 5256:
			case 5257:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 245;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 19 + (type - 5248);
				if (type == 5251)
				{
					value = sellPrice(0, 0, 20);
				}
				if (type == 5253)
				{
					value = sellPrice(0, 0, 20);
				}
				if (type == 5257)
				{
					value = sellPrice(0, 0, 20);
				}
				if (type == 5252)
				{
					value = sellPrice(0, 0, 50);
				}
				if (type == 5256)
				{
					value = sellPrice(0, 0, 50);
				}
				break;
			case 5258:
			case 5259:
			case 5260:
			case 5261:
			case 5262:
			case 5263:
			case 5264:
			case 5265:
			case 5266:
			case 5267:
			case 5268:
			case 5269:
			case 5270:
			case 5271:
			case 5272:
			case 5273:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 246;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 21 + (type - 5258);
				if (type == 5266)
				{
					value = sellPrice(0, 2);
				}
				if (type == 5259)
				{
					value = sellPrice(0, 0, 50);
				}
				if (type == 5265)
				{
					value = sellPrice(0, 0, 50);
				}
				if (type == 5264)
				{
					value = sellPrice(0, 0, 50);
				}
				if (type == 5263)
				{
					value = sellPrice(0, 0, 50);
				}
				break;
			case 5129:
				SetWeaponValues(15, 5f);
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 0, 35));
				useStyle = 1;
				useTurn = false;
				useAnimation = 17;
				useTime = 17;
				width = 24;
				height = 28;
				UseSound = SoundID.Item1;
				scale = 1f;
				melee = true;
				break;
			case 5274:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 62;
				break;
			case 5139:
				DefaultToAccessory(14, 30);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 50));
				break;
			case 5140:
			case 5141:
			case 5142:
			case 5143:
			case 5144:
			case 5145:
			case 5146:
				DefaultToAccessory(14, 30);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 1));
				switch (type)
				{
				case 5140:
					glowMask = 318;
					break;
				case 5141:
					glowMask = 319;
					break;
				case 5142:
					glowMask = 320;
					break;
				case 5143:
					glowMask = 321;
					break;
				case 5144:
					glowMask = 322;
					break;
				case 5145:
					glowMask = 323;
					break;
				case 5146:
					glowMask = 324;
					break;
				}
				break;
			case 5276:
				DefaultToVanitypet(994, 345);
				width = 32;
				height = 32;
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 1));
				break;
			case 5279:
				width = 18;
				height = 18;
				defense = 2;
				headSlot = 278;
				if (Variant == ItemVariants.WeakerVariant)
				{
					defense = 1;
				}
				break;
			case 5280:
				width = 18;
				height = 18;
				defense = 3;
				bodySlot = 246;
				if (Variant == ItemVariants.WeakerVariant)
				{
					defense = 1;
				}
				break;
			case 5281:
				width = 18;
				height = 18;
				defense = 2;
				legSlot = 234;
				if (Variant == ItemVariants.WeakerVariant)
				{
					defense = 1;
				}
				break;
			case 5282:
				useStyle = 5;
				useAnimation = 25;
				useTime = 25;
				width = 12;
				height = 28;
				shoot = 1;
				useAmmo = AmmoID.Arrow;
				UseSound = SoundID.Item5;
				damage = 10;
				shootSpeed = 6.6f;
				noMelee = true;
				value = 100;
				ranged = true;
				if (Variant == ItemVariants.WeakerVariant)
				{
					damage = 6;
					useAnimation = 29;
					useTime = 29;
				}
				break;
			case 5283:
				autoReuse = true;
				useStyle = 1;
				useTurn = true;
				useAnimation = 30;
				useTime = 20;
				hammer = 45;
				width = 24;
				height = 28;
				damage = 9;
				knockBack = 5.5f;
				scale = 1.1f;
				UseSound = SoundID.Item1;
				value = 50;
				melee = true;
				if (Variant == ItemVariants.WeakerVariant)
				{
					damage = 4;
					hammer = 35;
				}
				break;
			case 5284:
				useStyle = 1;
				useTurn = false;
				useAnimation = 17;
				useTime = 17;
				width = 24;
				height = 28;
				damage = 13;
				knockBack = 5f;
				UseSound = SoundID.Item1;
				scale = 1f;
				value = sellPrice(0, 0, 0, 20);
				melee = true;
				if (Variant == ItemVariants.WeakerVariant)
				{
					damage = 8;
					useAnimation = 23;
					useTime = 23;
				}
				break;
			case 5285:
				DefaultToThrownWeapon(996, 20, 8f);
				UseSound = SoundID.Item106;
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 3));
				width = 18;
				height = 18;
				break;
			case 5286:
				DefaultToPlaceableTile((ushort)12, 0);
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 7, 50));
				break;
			case 5287:
				DefaultToPlaceableTile((ushort)639, 0);
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 0, 25));
				break;
			case 5288:
				DefaultToMount(53);
				SetShopValues(ItemRarityColor.Yellow8, buyPrice(0, 5));
				break;
			case 5289:
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 2));
				maxStack = CommonMaxStack;
				consumable = true;
				width = 18;
				height = 18;
				useStyle = 4;
				useTime = 30;
				UseSound = SoundID.Item4;
				useAnimation = 30;
				expert = true;
				break;
			case 5290:
				width = 28;
				height = 20;
				headSlot = 279;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 50));
				vanity = true;
				break;
			case 5294:
				useStyle = 5;
				width = 24;
				height = 24;
				noUseGraphic = true;
				UseSound = SoundID.Item1;
				melee = true;
				channel = true;
				noMelee = true;
				shoot = 999;
				useAnimation = 25;
				useTime = 25;
				shootSpeed = 10f;
				knockBack = 3.75f;
				damage = 24;
				value = sellPrice(0, 1, 80);
				rare = 3;
				break;
			case 5293:
				DefaultToTorch(22);
				break;
			case 5299:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 215;
				placeStyle = 14;
				width = 12;
				height = 12;
				break;
			case 5298:
				noMelee = true;
				useStyle = 1;
				shootSpeed = 11f;
				shoot = 1000;
				damage = 21;
				knockBack = 3f;
				width = 14;
				height = 28;
				UseSound = SoundID.Item1;
				useAnimation = 22;
				useTime = 22;
				noUseGraphic = true;
				SetShopValues(ItemRarityColor.Orange3, sellPrice(0, 2));
				melee = true;
				autoReuse = true;
				break;
			case 5300:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				makeNPC = 672;
				break;
			case 5301:
				DefaultToPlaceableTile((ushort)640, 0);
				maxStack = CommonMaxStack;
				break;
			case 5302:
				useStyle = 1;
				useTurn = true;
				useAnimation = 12;
				useTime = 5;
				width = 20;
				height = 20;
				autoReuse = true;
				rare = 7;
				value = sellPrice(0, 10);
				tileBoost += 2;
				break;
			case 5303:
				useStyle = 1;
				useTurn = true;
				useAnimation = 12;
				useTime = 5;
				width = 20;
				height = 20;
				autoReuse = true;
				rare = 7;
				value = sellPrice(0, 10);
				tileBoost += 2;
				break;
			case 5304:
				useStyle = 1;
				useTurn = true;
				useAnimation = 8;
				useTime = 3;
				width = 20;
				height = 20;
				autoReuse = true;
				rare = 8;
				value = sellPrice(0, 30);
				tileBoost += 3;
				break;
			case 5292:
				DefaultToPlaceableTile((ushort)19, 48);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 1));
				width = 8;
				height = 10;
				break;
			case 5291:
				DefaultToPlaceableWall(318);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 0, 50));
				break;
			case 5306:
				DefaultToPlaceableTile((ushort)641, 0);
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 0, 20));
				break;
			case 5307:
				DefaultToPlaceableWall(319);
				break;
			case 5296:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 642;
				width = 26;
				height = 20;
				rare = 7;
				value = 100000;
				break;
			case 5295:
				useStyle = 1;
				autoReuse = true;
				useAnimation = 24;
				useTime = 12;
				width = 24;
				height = 28;
				damage = 20;
				knockBack = 5f;
				UseSound = SoundID.Item1;
				scale = 1.2f;
				SetShopValues(ItemRarityColor.LightRed4, sellPrice(0, 1, 50));
				melee = true;
				axe = 30;
				createTile = 2;
				break;
			case 5305:
				width = 18;
				height = 18;
				headSlot = 277;
				color = Main.player[Main.myPlayer].skinColor;
				rare = 1;
				vanity = true;
				value = sellPrice(0, 0, 50);
				break;
			case 5297:
				DefaultToVanitypet(998, 349);
				width = 32;
				height = 32;
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 25));
				break;
			case 5308:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 20);
				placeStyle = 63;
				break;
			case 5309:
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 5));
				width = 30;
				height = 30;
				break;
			case 5310:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 2);
				placeStyle = 64;
				break;
			case 5311:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				makeNPC = 673;
				break;
			case 5314:
				DefaultToPlaceableTile((ushort)643, 0);
				maxStack = CommonMaxStack;
				break;
			case 5312:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				makeNPC = 674;
				break;
			case 5315:
				DefaultToPlaceableTile((ushort)644, 0);
				maxStack = CommonMaxStack;
				break;
			case 5313:
				useStyle = 1;
				autoReuse = true;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				noUseGraphic = true;
				makeNPC = 675;
				break;
			case 5316:
				DefaultToPlaceableTile((ushort)645, 0);
				maxStack = CommonMaxStack;
				break;
			case 5317:
				DefaultToPlaceableTile((ushort)105, 80);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 60));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5318:
				DefaultToPlaceableTile((ushort)105, 81);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 60));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5319:
				DefaultToPlaceableTile((ushort)105, 82);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 60));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5320:
				DefaultToPlaceableTile((ushort)13, 1);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 60));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5321:
				DefaultToPlaceableTile((ushort)13, 2);
				SetShopValues(ItemRarityColor.White0, sellPrice(0, 0, 0, 20));
				maxStack = CommonMaxStack;
				width = 20;
				height = 20;
				break;
			case 5322:
				DefaultToPlaceableTile((ushort)646, 0);
				SetShopValues(ItemRarityColor.Blue1, 0);
				maxStack = CommonMaxStack;
				width = 8;
				height = 18;
				value = sellPrice(0, 0, 1);
				holdStyle = 1;
				noWet = true;
				flame = true;
				break;
			case 5323:
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 10));
				width = 30;
				height = 30;
				break;
			case 5324:
			case 5329:
			case 5330:
				DefaultToPlaceableTile((ushort)647, 0);
				maxStack = 1;
				SetShopValues(ItemRarityColor.StrongRed10, sellPrice(0, 5));
				consumable = false;
				tileBoost = 3;
				break;
			case 5326:
				DefaultToFood(22, 22, 0, 0);
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 10));
				maxStack = 1;
				break;
			case 5327:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 654;
				width = 28;
				height = 14;
				break;
			case 5328:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 8;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				width = 12;
				height = 12;
				break;
			case 5331:
				width = 16;
				height = 24;
				accessory = true;
				balloonSlot = 19;
				SetShopValues(ItemRarityColor.Yellow8, sellPrice(0, 4));
				hasVanityEffects = true;
				break;
			case 5332:
				DefaultToVanitypet(1003, 351);
				width = 32;
				height = 32;
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 10));
				break;
			case 5333:
				DefaultToVanitypet(1004, 352);
				width = 32;
				height = 32;
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 10));
				break;
			case 5334:
				width = 22;
				height = 14;
				if (Variant == ItemVariants.EnabledVariant)
				{
					useStyle = 4;
					consumable = true;
					useAnimation = 45;
					useTime = 45;
				}
				maxStack = CommonMaxStack;
				rare = 3;
				break;
			case 5335:
				autoReuse = false;
				useStyle = 1;
				useAnimation = 20;
				useTime = 20;
				width = 20;
				height = 20;
				UseSound = SoundID.Item8;
				SetShopValues(ItemRarityColor.StrongRed10, sellPrice(0, 10));
				break;
			case 5336:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.LightPurple6, 0);
				break;
			case 5337:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.LightPurple6, sellPrice(0, 1, 50));
				break;
			case 5338:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.LightPurple6, sellPrice(0, 2));
				break;
			case 5339:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.LightPurple6, sellPrice(0, 0, 25));
				break;
			case 5340:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.LightPurple6, sellPrice(0, 15));
				break;
			case 5341:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.LightPurple6, sellPrice(0, 10));
				break;
			case 5342:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.LightPurple6, sellPrice(0, 0, 50));
				break;
			case 5343:
				useStyle = 4;
				consumable = true;
				useAnimation = 45;
				useTime = 45;
				UseSound = SoundID.Item92;
				width = 28;
				height = 28;
				maxStack = CommonMaxStack;
				SetShopValues(ItemRarityColor.LightPurple6, sellPrice(0, 0, 25));
				break;
			case 5345:
				DefaultToPlaceableTile((ushort)657, 0);
				width = 22;
				height = 32;
				rare = 9;
				value = sellPrice(0, 1);
				accessory = true;
				vanity = true;
				break;
			case 5344:
				paintCoating = 2;
				width = 24;
				height = 24;
				value = buyPrice(0, 0, 2);
				maxStack = CommonMaxStack;
				break;
			case 5346:
				width = 18;
				height = 18;
				SetShopValues(ItemRarityColor.Green2, sellPrice(0, 0, 30));
				break;
			case 5356:
				width = 18;
				height = 18;
				SetShopValues(ItemRarityColor.TrashMinus1, 0);
				break;
			case 5347:
				DefaultToPlaceableTile((ushort)658, 0);
				width = 22;
				height = 32;
				rare = 9;
				value = sellPrice(0, 1);
				accessory = true;
				vanity = true;
				break;
			case 5348:
				shootSpeed = 3f;
				shoot = 1006;
				damage = 12;
				width = 10;
				height = 28;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Arrow;
				knockBack = 2f;
				ranged = true;
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 0, 0, 10));
				break;
			case 5349:
				DefaultToPlaceableTile((ushort)659, 0);
				rare = 1;
				break;
			case 5350:
				DefaultToCapturedCritter(677);
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 0, 2, 50));
				width = 12;
				height = 12;
				break;
			case 5351:
				DefaultToPlaceableTile((ushort)660, 0);
				maxStack = CommonMaxStack;
				break;
			case 5352:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 91;
				placeStyle = 310;
				width = 10;
				height = 24;
				value = 1000;
				rare = 1;
				break;
			case 5353:
				DefaultToTorch(23);
				break;
			case 5354:
				DefaultToAccessory(28, 32);
				faceSlot = 20;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 2));
				break;
			case 5355:
				DefaultToAccessory(26, 36);
				backSlot = 36;
				frontSlot = 12;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 2));
				break;
			case 5357:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 215;
				placeStyle = 15;
				width = 12;
				height = 12;
				break;
			case 5358:
			case 5437:
				useTurn = true;
				width = 20;
				height = 20;
				useStyle = 4;
				useTime = 90;
				UseSound = SoundID.Item6;
				useAnimation = 90;
				SetShopValues(ItemRarityColor.Yellow8, sellPrice(0, 10));
				break;
			case 5359:
				useTurn = true;
				width = 20;
				height = 20;
				useStyle = 4;
				useTime = 90;
				UseSound = SoundID.Item6;
				useAnimation = 90;
				SetShopValues(ItemRarityColor.Yellow8, sellPrice(0, 10));
				break;
			case 5360:
				useTurn = true;
				width = 20;
				height = 20;
				useStyle = 4;
				useTime = 90;
				UseSound = SoundID.Item6;
				useAnimation = 90;
				SetShopValues(ItemRarityColor.Yellow8, sellPrice(0, 10));
				break;
			case 5361:
				useTurn = true;
				width = 20;
				height = 20;
				useStyle = 4;
				useTime = 90;
				UseSound = SoundID.Item6;
				useAnimation = 90;
				SetShopValues(ItemRarityColor.Yellow8, sellPrice(0, 10));
				break;
			case 5362:
				DefaultToMusicBox(87);
				break;
			case 5363:
				DefaultToPlaceableWall(62);
				break;
			case 5364:
				useStyle = 1;
				useTurn = true;
				useAnimation = 12;
				useTime = 5;
				width = 20;
				height = 20;
				autoReuse = true;
				rare = 10;
				value = sellPrice(0, 10);
				tileBoost += 2;
				break;
			case 5365:
				DefaultToPlaceableWall(7);
				break;
			case 5366:
				DefaultToPlaceableWall(94);
				break;
			case 5367:
				DefaultToPlaceableWall(95);
				break;
			case 5368:
				DefaultToPlaceableWall(9);
				break;
			case 5369:
				DefaultToPlaceableWall(96);
				break;
			case 5370:
				DefaultToPlaceableWall(97);
				break;
			case 5371:
				DefaultToPlaceableWall(8);
				break;
			case 5372:
				DefaultToPlaceableWall(98);
				break;
			case 5373:
				DefaultToPlaceableWall(99);
				break;
			case 5374:
				DefaultToPlaceableWall(187);
				break;
			case 5375:
				DefaultToPlaceableWall(216);
				break;
			case 5376:
				DefaultToPlaceableWall(87);
				break;
			case 5377:
				shootSpeed = 6f;
				shoot = 1008;
				damage = 1;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Flare;
				knockBack = 1.5f;
				rare = 1;
				value = buyPrice(0, 0, 1, 50);
				ranged = true;
				break;
			case 5378:
				shootSpeed = 6f;
				shoot = 1009;
				damage = 1;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Flare;
				knockBack = 1.5f;
				value = 7;
				ranged = true;
				break;
			case 5379:
				shootSpeed = 6f;
				shoot = 1010;
				damage = 1;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Flare;
				knockBack = 1.5f;
				value = 7;
				ranged = true;
				break;
			case 5380:
				shootSpeed = 6f;
				shoot = 1011;
				damage = 1;
				width = 12;
				height = 12;
				maxStack = CommonMaxStack;
				consumable = true;
				ammo = AmmoID.Flare;
				knockBack = 1.5f;
				value = 7;
				ranged = true;
				break;
			case 5381:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 663;
				width = 18;
				height = 34;
				SetShopValues(ItemRarityColor.Lime7, sellPrice(0, 3));
				break;
			case 5382:
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 3));
				autoReuse = true;
				UseSound = SoundID.Item1;
				useStyle = 1;
				damage = 50;
				useAnimation = 23;
				useTime = 23;
				width = 30;
				height = 30;
				shoot = 1012;
				shootSpeed = 11f;
				knockBack = 4.75f;
				melee = true;
				shootsEveryUse = true;
				break;
			case 5383:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 664;
				width = 18;
				height = 18;
				SetShopValues(ItemRarityColor.Pink5, sellPrice(0, 0, 0, 15));
				break;
			case 5384:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 665;
				width = 18;
				height = 18;
				SetShopValues(ItemRarityColor.Green2, buyPrice(0, 7, 50));
				break;
			case 5385:
				width = 28;
				height = 20;
				headSlot = 280;
				SetShopValues(ItemRarityColor.Blue1, sellPrice(0, 0, 50));
				vanity = true;
				break;
			case 5386:
				width = 18;
				height = 14;
				bodySlot = 247;
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 1));
				vanity = true;
				break;
			case 5387:
				width = 18;
				height = 14;
				legSlot = 235;
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 1));
				vanity = true;
				break;
			case 5389:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 242;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 65;
				break;
			case 5388:
				useStyle = 1;
				useTurn = true;
				useAnimation = 15;
				useTime = 10;
				autoReuse = true;
				maxStack = CommonMaxStack;
				consumable = true;
				createTile = 240;
				width = 30;
				height = 30;
				value = sellPrice(0, 0, 10);
				placeStyle = 92;
				break;
			case 5390:
				width = 28;
				height = 20;
				headSlot = 281;
				SetShopValues(ItemRarityColor.Orange3, buyPrice(0, 1));
				vanity = true;
				break;
			case 5391:
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 5));
				width = 30;
				height = 30;
				break;
			case 5397:
				DefaultToPlaceableWall(321);
				break;
			case 5398:
				DefaultToPlaceableTile((ushort)667, 0);
				rare = 1;
				break;
			case 5399:
				DefaultToPlaceableWall(322);
				break;
			case 5392:
				DefaultToSolution(1015);
				break;
			case 5393:
				DefaultToSolution(1016);
				break;
			case 5394:
				DefaultToSolution(1017);
				break;
			case 5401:
				DefaultToPlaceableTile((ushort)669, 0);
				rare = 9;
				break;
			case 5402:
				DefaultToPlaceableTile((ushort)670, 0);
				rare = 9;
				break;
			case 5403:
				DefaultToPlaceableTile((ushort)671, 0);
				rare = 9;
				break;
			case 5404:
				DefaultToPlaceableTile((ushort)672, 0);
				rare = 9;
				break;
			case 5405:
				DefaultToPlaceableTile((ushort)673, 0);
				rare = 9;
				break;
			case 5406:
				DefaultToPlaceableTile((ushort)674, 0);
				rare = 9;
				break;
			case 5407:
				DefaultToPlaceableTile((ushort)675, 0);
				rare = 9;
				break;
			case 5408:
				DefaultToPlaceableTile((ushort)676, 0);
				rare = 9;
				break;
			case 5409:
				DefaultToPlaceableWall(323);
				rare = 9;
				break;
			case 5410:
				DefaultToPlaceableWall(324);
				rare = 9;
				break;
			case 5411:
				DefaultToPlaceableWall(325);
				rare = 9;
				break;
			case 5412:
				DefaultToPlaceableWall(326);
				rare = 9;
				break;
			case 5413:
				DefaultToPlaceableWall(327);
				rare = 9;
				break;
			case 5414:
				DefaultToPlaceableWall(328);
				rare = 9;
				break;
			case 5415:
				DefaultToPlaceableWall(329);
				rare = 9;
				break;
			case 5416:
				DefaultToPlaceableWall(330);
				rare = 9;
				break;
			case 5417:
				DefaultToPlaceableTile((ushort)677, 0);
				break;
			case 5418:
				DefaultToPlaceableWall(331);
				break;
			case 5419:
				DefaultToPlaceableTile((ushort)678, 0);
				break;
			case 5420:
				DefaultToPlaceableWall(332);
				break;
			case 5421:
				DefaultToPlaceableTile((ushort)679, 0);
				break;
			case 5422:
				DefaultToPlaceableWall(333);
				break;
			case 5423:
				DefaultToPlaceableTile((ushort)680, 0);
				break;
			case 5424:
				DefaultToPlaceableWall(334);
				break;
			case 5425:
				DefaultToPlaceableTile((ushort)681, 0);
				break;
			case 5426:
				DefaultToPlaceableWall(335);
				break;
			case 5427:
				DefaultToPlaceableTile((ushort)682, 0);
				break;
			case 5428:
				DefaultToPlaceableWall(336);
				break;
			case 5429:
				DefaultToPlaceableTile((ushort)685, 0);
				break;
			case 5430:
				DefaultToPlaceableWall(339);
				break;
			case 5431:
				DefaultToPlaceableTile((ushort)686, 0);
				break;
			case 5432:
				DefaultToPlaceableWall(340);
				break;
			case 5433:
				DefaultToPlaceableTile((ushort)683, 0);
				break;
			case 5434:
				DefaultToPlaceableWall(337);
				break;
			case 5435:
				DefaultToPlaceableTile((ushort)684, 0);
				break;
			case 5436:
				DefaultToPlaceableWall(338);
				break;
			case 5400:
				DefaultToVanitypet(1018, 354);
				width = 16;
				height = 16;
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 10));
				break;
			case 5395:
				DefaultToPlaceableTile((ushort)666, 0);
				break;
			case 5396:
				DefaultToPlaceableWall(320);
				break;
			case 5439:
				DefaultToPlaceableTile((ushort)687, 0);
				break;
			case 5440:
				DefaultToPlaceableTile((ushort)688, 0);
				break;
			case 5441:
				DefaultToPlaceableTile((ushort)689, 0);
				break;
			case 5442:
				DefaultToPlaceableTile((ushort)690, 0);
				break;
			case 5443:
				DefaultToPlaceableTile((ushort)691, 0);
				break;
			case 5444:
				DefaultToPlaceableTile((ushort)692, 0);
				break;
			case 5445:
				DefaultToPlaceableWall(341);
				break;
			case 5446:
				DefaultToPlaceableWall(342);
				break;
			case 5447:
				DefaultToPlaceableWall(343);
				break;
			case 5448:
				DefaultToPlaceableWall(344);
				break;
			case 5449:
				DefaultToPlaceableWall(345);
				break;
			case 5450:
				DefaultToPlaceableWall(346);
				break;
			case 5438:
				useStyle = 1;
				shootSpeed = 3f;
				shoot = 1019;
				width = 16;
				height = 24;
				maxStack = CommonMaxStack;
				consumable = true;
				UseSound = SoundID.Item1;
				useAnimation = 15;
				value = sellPrice(0, 0, 0, 10);
				useTime = 15;
				noMelee = true;
				break;
			case 5451:
				useStyle = 1;
				autoReuse = false;
				useAnimation = 32;
				holdStyle = 7;
				useTime = 32;
				width = 32;
				height = 20;
				noUseGraphic = true;
				shoot = 1020;
				UseSound = SoundID.Item1;
				shootSpeed = 9f;
				value = buyPrice(0, 10);
				rare = 3;
				break;
			case 5452:
				width = 16;
				height = 24;
				accessory = true;
				rare = 3;
				value = buyPrice(0, 10);
				faceSlot = 21;
				break;
			case 4009:
			case 4010:
			case 4011:
			case 4012:
			case 4013:
			case 4014:
			case 4015:
			case 4016:
			case 4017:
			case 4018:
			case 4019:
			case 4020:
			case 4021:
			case 4022:
			case 4023:
			case 4024:
			case 4025:
			case 4026:
			case 4027:
			case 4028:
			case 4029:
			case 4030:
			case 4031:
			case 4032:
			case 4033:
			case 4034:
			case 4035:
			case 4036:
			case 4037:
			case 4282:
			case 4283:
			case 4284:
			case 4285:
			case 4286:
			case 4287:
			case 4288:
			case 4289:
			case 4290:
			case 4291:
			case 4292:
			case 4293:
			case 4294:
			case 4295:
			case 4296:
			case 4297:
			case 4403:
			case 4411:
			case 4614:
			case 4615:
			case 4616:
			case 4617:
			case 4618:
			case 4619:
			case 4620:
			case 4621:
			case 4622:
			case 4623:
			case 4624:
			case 4625:
			case 5009:
			case 5013:
			case 5041:
			case 5042:
			case 5092:
			case 5093:
			case 5275:
			case 5277:
			case 5278:
				break;
			}
		}

		public void DefaultToSolution(int projectileId)
		{
			shoot = projectileId - 145;
			ammo = AmmoID.Solution;
			width = 10;
			height = 12;
			value = buyPrice(0, 0, 15);
			rare = 3;
			maxStack = CommonMaxStack;
			consumable = true;
		}

		public void DefaultToWhip(int projectileId, int dmg, float kb, float shootspeed, int animationTotalTime = 30)
		{
			autoReuse = false;
			useStyle = 1;
			useAnimation = animationTotalTime;
			useTime = animationTotalTime;
			width = 18;
			height = 18;
			shoot = projectileId;
			UseSound = SoundID.Item152;
			noMelee = true;
			summon = true;
			noUseGraphic = true;
			damage = dmg;
			knockBack = kb;
			shootSpeed = shootspeed;
		}

		public void DefaultTokite(int projId)
		{
			width = 20;
			height = 28;
			DefaultToThrownWeapon(projId, 30, 2f);
			consumable = false;
			ranged = false;
			noUseGraphic = true;
			maxStack = 1;
			SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
		}

		public void ChangeItemType(int to)
		{
			bool flag = favorited;
			SetDefaults(to);
			favorited = flag;
		}

		public void DefaultToVanitypet(int projId, int buffID)
		{
			damage = 0;
			useStyle = 1;
			width = 16;
			height = 30;
			UseSound = SoundID.Item2;
			useAnimation = 20;
			useTime = 20;
			rare = 3;
			noMelee = true;
			value = sellPrice(0, 2);
			buffType = buffID;
			shoot = projId;
		}

		public static bool IsAGolfingItem(Item item)
		{
			if (ProjectileID.Sets.IsAGolfBall[item.shoot])
			{
				return true;
			}
			int num = item.type;
			if (num == 4039 || (uint)(num - 4092) <= 3u || (uint)(num - 4587) <= 11u)
			{
				return true;
			}
			return false;
		}

		private void DefaultToSeaShelll()
		{
			useStyle = 1;
			autoReuse = true;
			useAnimation = 15;
			useTime = 10;
			maxStack = CommonMaxStack;
			consumable = true;
			createTile = 324;
			width = 22;
			height = 22;
			switch (type)
			{
			case 4071:
				placeStyle = 4;
				value = sellPrice(0, 1);
				break;
			case 4073:
				placeStyle = 3;
				value = sellPrice(0, 0, 20);
				break;
			case 4072:
				placeStyle = 2;
				value = sellPrice(0, 0, 20);
				break;
			case 2626:
				placeStyle = 1;
				value = sellPrice(0, 0, 10);
				break;
			default:
				value = sellPrice(0, 0, 5);
				break;
			}
		}

		public void DefaultToCapturedCritter(short npcIdToSpawnOnUse)
		{
			useStyle = 1;
			autoReuse = true;
			useTurn = true;
			useAnimation = 15;
			useTime = 10;
			maxStack = CommonMaxStack;
			consumable = true;
			width = 12;
			height = 12;
			noUseGraphic = true;
			makeNPC = npcIdToSpawnOnUse;
		}

		public void DefaultToStaff(int projType, float pushForwardSpeed, int singleShotTime, int manaPerShot)
		{
			DefaultToMagicWeapon(projType, singleShotTime, pushForwardSpeed, hasAutoReuse: true);
			mana = manaPerShot;
			width = 40;
			height = 40;
			UseSound = SoundID.Item43;
		}

		public void DefaultToSpear(int projType, float pushForwardSpeed, int animationTime)
		{
			useStyle = 5;
			useAnimation = 31;
			useTime = 31;
			shootSpeed = pushForwardSpeed;
			width = 32;
			height = 32;
			UseSound = SoundID.Item1;
			shoot = projType;
			noMelee = true;
			noUseGraphic = true;
			melee = true;
			useAnimation = (useTime = animationTime);
		}

		private void SetFoodDefaults(int type)
		{
			switch (type)
			{
			case 4022:
				DefaultToFood(22, 22, 207, 172800);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 50));
				break;
			case 1919:
				DefaultToFood(22, 22, 207, 14400);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 25));
				break;
			case 1920:
				DefaultToFood(22, 22, 207, 14400);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 25));
				break;
			case 4011:
				DefaultToFood(22, 22, 207, 57600);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 3532:
				DefaultToFood(22, 22, 207, 86400);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 5));
				break;
			case 1911:
				DefaultToFood(22, 22, 207, 14400, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 10));
				break;
			case 4013:
				DefaultToFood(22, 22, 207, 86400);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 5));
				break;
			case 4615:
				DefaultToFood(22, 22, 207, 57600, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 4));
				break;
			case 4027:
				DefaultToFood(22, 22, 207, 57600, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4025:
				DefaultToFood(22, 22, 207, 43200);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4029:
				DefaultToFood(22, 22, 207, 28800);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4036:
				DefaultToFood(22, 22, 207, 28800);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4037:
				DefaultToFood(22, 22, 207, 57600);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4015:
				DefaultToFood(22, 22, 207, 28800);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 2426:
				DefaultToFood(22, 22, 206, 36000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 75));
				break;
			case 2427:
				DefaultToFood(22, 22, 206, 28800);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 25));
				break;
			case 4034:
				DefaultToFood(22, 22, 206, 50400);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 50));
				break;
			case 357:
				DefaultToFood(22, 22, 206, 28800, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 1787:
				DefaultToFood(22, 22, 206, 28800);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 10));
				break;
			case 4012:
				DefaultToFood(22, 22, 206, 36000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4016:
				DefaultToFood(22, 22, 206, 50400);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4017:
				DefaultToFood(22, 22, 206, 72000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 5042:
				DefaultToFood(22, 22, 206, 36000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4018:
				DefaultToFood(22, 22, 206, 57600, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4288:
				DefaultToFood(22, 22, 206, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4019:
				DefaultToFood(22, 22, 206, 36000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4020:
				DefaultToFood(22, 22, 206, 50400);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4021:
				DefaultToFood(22, 22, 206, 36000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4023:
				DefaultToFood(22, 22, 206, 57600);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 3195:
				DefaultToFood(22, 22, 206, 50400, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4026:
				DefaultToFood(22, 22, 206, 50400, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4028:
				DefaultToFood(22, 22, 206, 57600);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4032:
				DefaultToFood(22, 22, 206, 36000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4035:
				DefaultToFood(22, 22, 206, 64800);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2, 50));
				break;
			case 4403:
				DefaultToFood(22, 22, 206, 36000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 2267:
				DefaultToFood(22, 22, 206, 28800);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 55));
				break;
			case 4623:
				DefaultToFood(22, 22, 206, 72000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 2268:
				DefaultToFood(22, 22, 206, 36000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 75));
				break;
			case 4297:
				DefaultToFood(22, 22, 206, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 5092:
				DefaultToFood(22, 22, 206, 28800);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 50));
				break;
			case 5093:
				DefaultToFood(22, 22, 206, 21600);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4009:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4614:
				DefaultToFood(22, 22, 26, 36000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4014:
				DefaultToFood(22, 22, 26, 36000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4024:
				DefaultToFood(22, 22, 26, 36000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4030:
				DefaultToFood(22, 22, 26, 90000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1, 50));
				break;
			case 4031:
				DefaultToFood(22, 22, 26, 36000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 969:
				DefaultToFood(12, 12, 26, 36000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 2));
				useStyle = 9;
				break;
			case 2425:
				DefaultToFood(22, 22, 26, 28800);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 25));
				break;
			case 4282:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4283:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4617:
				DefaultToFood(22, 22, 26, 54000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4284:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4285:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4621:
				DefaultToFood(22, 22, 26, 72000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4286:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4287:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4289:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4624:
				DefaultToFood(22, 22, 26, 54000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4625:
				DefaultToFood(22, 22, 26, 90000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 3));
				break;
			case 4290:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4291:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4616:
				DefaultToFood(22, 22, 26, 36000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4292:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 967:
				DefaultToFood(12, 12, 26, 3600);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 1));
				useStyle = 9;
				break;
			case 4293:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4618:
				DefaultToFood(22, 22, 26, 36000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4619:
				DefaultToFood(22, 22, 26, 72000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4294:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4295:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4296:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4622:
				DefaultToFood(22, 22, 26, 72000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 4411:
				DefaultToFood(22, 22, 26, 36000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4620:
				DefaultToFood(22, 22, 26, 72000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 2));
				break;
			case 5009:
				DefaultToFood(22, 22, 26, 18000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 0, 20));
				break;
			case 5041:
				DefaultToFood(22, 22, 26, 72000, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 4033:
				DefaultToFood(22, 22, 26, 36000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 50));
				break;
			case 5275:
				DefaultToFood(22, 22, 26, 7200, useGulpSound: true);
				SetShopValues(ItemRarityColor.White0, buyPrice(0, 0, 1, 25));
				break;
			case 5277:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 5278:
				DefaultToFood(22, 22, 26, 18000);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 1));
				break;
			case 2266:
				DefaultToFood(22, 22, 25, 14400, useGulpSound: true);
				SetShopValues(ItemRarityColor.Blue1, buyPrice(0, 0, 5));
				break;
			case 353:
				DefaultToFood(22, 22, 25, 7200, useGulpSound: true);
				SetShopValues(ItemRarityColor.White0, 100);
				maxStack = CommonMaxStack;
				holdStyle = 1;
				ammo = 353;
				notAmmo = true;
				break;
			case 1912:
				DefaultToHealingPotion(22, 22, 80);
				SetShopValues(ItemRarityColor.Blue1, 40);
				break;
			}
			float num = 0f;
			num = ((buffType == 207) ? (num + 12f) : ((buffType != 206) ? (num + 3f) : (num + 8f)));
			num += (float)(buffTime / 14400);
			rare = (int)(num / 4f);
		}

		public void DefaultToMount(int mount)
		{
			width = 36;
			height = 26;
			mountType = mount;
		}

		public void DefaultToPlaceableWall(ushort wallToPlace)
		{
			useStyle = 1;
			useTurn = true;
			useAnimation = 15;
			useTime = 7;
			autoReuse = true;
			maxStack = CommonMaxStack;
			consumable = true;
			createWall = wallToPlace;
			width = 12;
			height = 12;
		}

		public void SetWeaponValues(int dmg, float knockback, int bonusCritChance = 0)
		{
			damage = dmg;
			knockBack = knockback;
			crit = bonusCritChance;
		}

		public void DefaultToBow(int singleShotTime, float shotVelocity, bool hasAutoReuse = false)
		{
			DefaultToRangedWeapon(1, AmmoID.Arrow, singleShotTime, shotVelocity, hasAutoReuse);
			width = 14;
			height = 30;
			UseSound = SoundID.Item5;
		}

		public void DefaultToMagicWeapon(int projType, int singleShotTime, float shotVelocity, bool hasAutoReuse = false)
		{
			autoReuse = hasAutoReuse;
			useStyle = 5;
			useAnimation = singleShotTime;
			useTime = singleShotTime;
			shoot = projType;
			shootSpeed = shotVelocity;
			noMelee = true;
			magic = true;
		}

		public void DefaultToRangedWeapon(int baseProjType, int ammoID, int singleShotTime, float shotVelocity, bool hasAutoReuse = false)
		{
			autoReuse = hasAutoReuse;
			useStyle = 5;
			useAnimation = singleShotTime;
			useTime = singleShotTime;
			shoot = baseProjType;
			useAmmo = ammoID;
			shootSpeed = shotVelocity;
			noMelee = true;
			ranged = true;
		}

		public void DefaultToThrownWeapon(int baseProjType, int singleShotTime, float shotVelocity, bool hasAutoReuse = false)
		{
			autoReuse = hasAutoReuse;
			useStyle = 1;
			useAnimation = singleShotTime;
			useTime = singleShotTime;
			shoot = baseProjType;
			shootSpeed = shotVelocity;
			noMelee = true;
			ranged = true;
			consumable = true;
			maxStack = CommonMaxStack;
		}

		private void DefaultToTorch(int tileStyleToPlace, bool allowWaterPlacement = false)
		{
			flame = true;
			noWet = !allowWaterPlacement;
			holdStyle = 1;
			autoReuse = true;
			maxStack = CommonMaxStack;
			consumable = true;
			createTile = 4;
			placeStyle = tileStyleToPlace;
			width = 10;
			height = 12;
			value = 60;
			useStyle = 1;
			useTurn = true;
			useAnimation = 15;
			useTime = 10;
		}

		public void DefaultToPlaceableTile(int tileIDToPlace, int tileStyleToPlace = 0)
		{
			DefaultToPlaceableTile((ushort)tileIDToPlace, tileStyleToPlace);
		}

		public void DefaultToPlaceableTile(ushort tileIDToPlace, int tileStyleToPlace = 0)
		{
			createTile = tileIDToPlace;
			placeStyle = tileStyleToPlace;
			width = 14;
			height = 14;
			useStyle = 1;
			useAnimation = 15;
			useTime = 10;
			maxStack = CommonMaxStack;
			useTurn = true;
			autoReuse = true;
			consumable = true;
		}

		public void DefaultToGolfClub(int newwidth, int newheight)
		{
			width = newwidth;
			height = newheight;
			channel = true;
			useStyle = 8;
			holdStyle = 4;
			shootSpeed = 6f;
			shoot = 722;
			UseSound = null;
			useAnimation = (useTime = 12);
			noMelee = true;
		}

		public void DefaultToLawnMower(int newwidth, int newheight)
		{
			width = newwidth;
			height = newheight;
			holdStyle = 1;
			useStyle = 11;
			useAnimation = 30;
			useTime = 10;
			UseSound = SoundID.Item23;
			autoReuse = true;
		}

		public void DefaultToFood(int newwidth, int newheight, int foodbuff, int foodbuffduration, bool useGulpSound = false, int animationTime = 17)
		{
			if (useGulpSound)
			{
				UseSound = SoundID.Item3;
			}
			else
			{
				UseSound = SoundID.Item2;
			}
			if (useGulpSound)
			{
				useStyle = 9;
			}
			else
			{
				useStyle = 2;
			}
			useTurn = true;
			useAnimation = (useTime = animationTime);
			maxStack = CommonMaxStack;
			consumable = true;
			width = newwidth;
			height = newheight;
			buffType = foodbuff;
			buffTime = foodbuffduration;
			rare = 1;
			value = buyPrice(0, 0, 20);
		}

		public void DefaultToHealingPotion(int newwidth, int newheight, int healingAmount, int animationTime = 17)
		{
			UseSound = SoundID.Item3;
			useStyle = 9;
			useTurn = true;
			useAnimation = (useTime = animationTime);
			maxStack = CommonMaxStack;
			consumable = true;
			width = newwidth;
			height = newheight;
			rare = 1;
			value = buyPrice(0, 0, 20);
			potion = true;
			healLife = healingAmount;
		}

		public void SetShopValues(ItemRarityColor rarity, int coinValue)
		{
			rare = (int)rarity;
			value = coinValue;
		}

		public void DefaultToHeadgear(int newwidth, int newheight, int helmetArtID)
		{
			width = newwidth;
			height = newheight;
			headSlot = helmetArtID;
		}

		public void DefaultToAccessory(int newwidth = 24, int newheight = 24)
		{
			width = newwidth;
			height = newheight;
			accessory = true;
		}

		public void DefaultToGuitar(int newwidth = 24, int newheight = 24)
		{
			width = newwidth;
			height = newheight;
			autoReuse = true;
			holdStyle = 5;
			useStyle = 12;
			useAnimation = (useTime = 12);
		}

		public void DefaultToMusicBox(int style)
		{
			useStyle = 1;
			useTurn = true;
			useAnimation = 15;
			useTime = 10;
			autoReuse = true;
			consumable = true;
			createTile = 139;
			placeStyle = style;
			width = 24;
			height = 24;
			rare = 4;
			value = 100000;
			accessory = true;
			hasVanityEffects = true;
		}

		public void SetDefaults(int Type = 0)
		{
			SetDefaults(Type, noMatCheck: false, null);
		}

		public void SetDefaults(int Type, bool noMatCheck = false, ItemVariant variant = null)
		{
			if (Type < 0)
			{
				netDefaults(Type);
				return;
			}
			if (Main.netMode == 1 || Main.netMode == 2)
			{
				playerIndexTheItemIsReservedFor = 255;
			}
			else
			{
				playerIndexTheItemIsReservedFor = Main.myPlayer;
			}
			ResetStats(Type);
			if (type >= 5453)
			{
				type = 0;
			}
			if (variant == null)
			{
				variant = ItemVariants.SelectVariant(Type);
			}
			else if (!ItemVariants.HasVariant(Type, variant))
			{
				variant = null;
			}
			Variant = variant;
			if (type == 0)
			{
				netID = 0;
				stack = 0;
			}
			else if (ItemID.Sets.IsFood[type])
			{
				SetFoodDefaults(type);
			}
			else if (type <= 1000)
			{
				SetDefaults1(type);
			}
			else if (type <= 2001)
			{
				SetDefaults2(type);
			}
			else if (type <= 3000)
			{
				SetDefaults3(type);
			}
			else if (type <= 3989)
			{
				SetDefaults4(type);
			}
			else
			{
				SetDefaults5(type);
			}
			dye = (byte)GameShaders.Armor.GetShaderIdFromItemId(type);
			if (hairDye != 0)
			{
				hairDye = GameShaders.Hair.GetShaderIdFromItemId(type);
			}
			if (type == 2015)
			{
				value = sellPrice(0, 0, 5);
			}
			if (type == 2016)
			{
				value = sellPrice(0, 0, 7, 50);
			}
			if (type == 2017)
			{
				value = sellPrice(0, 0, 7, 50);
			}
			if (type == 5212)
			{
				value = sellPrice(0, 0, 7, 50);
			}
			if (type == 5300)
			{
				value = sellPrice(0, 0, 7, 50);
			}
			if (type == 5311)
			{
				value = sellPrice(0, 0, 7, 50);
			}
			if (type == 5312)
			{
				value = sellPrice(0, 0, 7, 50);
			}
			if (type == 5313)
			{
				value = sellPrice(0, 0, 7, 50);
			}
			if (type == 2019)
			{
				value = sellPrice(0, 0, 5);
			}
			if (type == 2018)
			{
				value = sellPrice(0, 0, 5);
			}
			if (type == 3563)
			{
				value = sellPrice(0, 0, 5);
			}
			if (type == 261)
			{
				value = sellPrice(0, 0, 7, 50);
			}
			if (type == 2205)
			{
				value = sellPrice(0, 0, 12, 50);
			}
			if (type == 2123)
			{
				value = sellPrice(0, 0, 7, 50);
			}
			if (type == 2122)
			{
				value = sellPrice(0, 0, 7, 50);
			}
			if (type == 2003)
			{
				value = sellPrice(0, 0, 10);
			}
			if (type == 2156)
			{
				value = sellPrice(0, 0, 15);
			}
			if (type == 2157)
			{
				value = sellPrice(0, 0, 15);
			}
			if (type == 2121)
			{
				value = sellPrice(0, 0, 10);
			}
			if (type == 1992)
			{
				value = sellPrice(0, 0, 3);
			}
			if (type == 2004)
			{
				value = sellPrice(0, 0, 5);
			}
			if (type == 2002)
			{
				value = sellPrice(0, 0, 5);
			}
			if (type == 2740)
			{
				value = sellPrice(0, 0, 2, 50);
			}
			if (type == 2006)
			{
				value = sellPrice(0, 0, 10);
			}
			if (type == 3191)
			{
				value = sellPrice(0, 0, 20);
			}
			if (type == 3192)
			{
				value = sellPrice(0, 0, 2, 50);
			}
			if (type == 3193)
			{
				value = sellPrice(0, 0, 5);
			}
			if (type == 3194)
			{
				value = sellPrice(0, 0, 10);
			}
			if (type == 2007)
			{
				value = sellPrice(0, 0, 50);
			}
			if (type == 2673)
			{
				value = sellPrice(0, 10);
			}
			if (bait > 0)
			{
				if (bait >= 50)
				{
					rare = 3;
				}
				else if (bait >= 30)
				{
					rare = 2;
				}
				else if (bait >= 15)
				{
					rare = 1;
				}
			}
			if (type >= 1994 && type <= 2001)
			{
				int num = type - 1994;
				if (num == 0)
				{
					value = sellPrice(0, 0, 5);
				}
				if (num == 4)
				{
					value = sellPrice(0, 0, 10);
				}
				if (num == 6)
				{
					value = sellPrice(0, 0, 15);
				}
				if (num == 3)
				{
					value = sellPrice(0, 0, 20);
				}
				if (num == 7)
				{
					value = sellPrice(0, 0, 30);
				}
				if (num == 2)
				{
					value = sellPrice(0, 0, 40);
				}
				if (num == 1)
				{
					value = sellPrice(0, 0, 75);
				}
				if (num == 5)
				{
					value = sellPrice(0, 1);
				}
			}
			if (type == 2663 || type == 1720 || type == 2137 || type == 2155 || type == 2151 || type == 1704 || type == 2143 || type == 1710 || type == 2238 || type == 2133 || type == 2147 || type == 2405 || type == 1716 || type == 1705)
			{
				value = sellPrice(0, 2);
			}
			if (Main.projHook[shoot])
			{
				useStyle = 0;
				useTime = 0;
				useAnimation = 0;
			}
			if (ItemID.Sets.IsDrill[type] || ItemID.Sets.IsChainsaw[type] || type == 1262)
			{
				useTime = (int)((double)useTime * 0.6);
				if (useTime < 1)
				{
					useTime = 1;
				}
				useAnimation = (int)((double)useAnimation * 0.6);
				if (useAnimation < 1)
				{
					useAnimation = 1;
				}
				tileBoost--;
			}
			if (ItemID.Sets.IsFood[type])
			{
				holdStyle = 1;
			}
			if (type >= 1803 && type <= 1807)
			{
				SetDefaults(1533 + type - 1803);
			}
			if (dye > 0)
			{
				maxStack = CommonMaxStack;
			}
			if (createTile == 19)
			{
				maxStack = CommonMaxStack;
			}
			netID = type;
			if (!noMatCheck)
			{
				material = ItemID.Sets.IsAMaterial[type];
			}
			RebuildTooltip();
			if (type > 0 && type < 5453 && ItemID.Sets.Deprecated[type])
			{
				netID = 0;
				type = 0;
				stack = 0;
			}
		}

		public void OnCreated(ItemCreationContext context)
		{
			if (type == 5437)
			{
				SetDefaults(5358);
			}
		}

		public void ResetStats(int Type)
		{
			tooltipContext = -1;
			BestiaryNotes = null;
			sentry = false;
			hasVanityEffects = false;
			DD2Summon = false;
			shopSpecialCurrency = -1;
			shopCustomPrice = null;
			expert = false;
			isAShopItem = false;
			expertOnly = false;
			instanced = false;
			questItem = false;
			fishingPole = 0;
			bait = 0;
			hairDye = -1;
			makeNPC = 0;
			dye = 0;
			paint = 0;
			paintCoating = 0;
			tileWand = -1;
			notAmmo = false;
			netID = 0;
			prefix = 0;
			crit = 0;
			mech = false;
			flame = false;
			reuseDelay = 0;
			melee = false;
			magic = false;
			ranged = false;
			summon = false;
			placeStyle = 0;
			buffTime = 0;
			buffType = 0;
			mountType = -1;
			cartTrack = false;
			material = false;
			noWet = false;
			vanity = false;
			mana = 0;
			wet = false;
			wetCount = 0;
			lavaWet = false;
			channel = false;
			manaIncrease = 0;
			timeSinceTheItemHasBeenReservedForSomeone = 0;
			noMelee = false;
			noUseGraphic = false;
			lifeRegen = 0;
			shootSpeed = 0f;
			active = true;
			alpha = 0;
			ammo = AmmoID.None;
			useAmmo = AmmoID.None;
			autoReuse = false;
			accessory = false;
			axe = 0;
			healMana = 0;
			bodySlot = -1;
			legSlot = -1;
			headSlot = -1;
			potion = false;
			color = default(Color);
			glowMask = -1;
			consumable = false;
			createTile = -1;
			createWall = -1;
			damage = -1;
			defense = 0;
			hammer = 0;
			healLife = 0;
			holdStyle = 0;
			knockBack = 0f;
			maxStack = 1;
			pick = 0;
			rare = 0;
			scale = 1f;
			shoot = 0;
			stack = 1;
			ToolTip = null;
			tileBoost = 0;
			useStyle = 0;
			UseSound = null;
			useTime = 100;
			useAnimation = 100;
			value = 0;
			useTurn = false;
			buy = false;
			handOnSlot = -1;
			handOffSlot = -1;
			backSlot = -1;
			frontSlot = -1;
			shoeSlot = -1;
			waistSlot = -1;
			wingSlot = -1;
			shieldSlot = -1;
			neckSlot = -1;
			faceSlot = -1;
			balloonSlot = -1;
			beardSlot = -1;
			uniqueStack = false;
			favorited = false;
			shootsEveryUse = false;
			Variant = null;
			type = Type;
		}

		public Color GetAlpha(Color newColor)
		{
			if (ItemID.Sets.BossBag[type])
			{
				return Color.Lerp(newColor, Color.White, 0.4f);
			}
			switch (type)
			{
			case 1326:
			case 5335:
				return Color.Lerp(newColor, Color.White, 0.75f);
			case 5043:
				return new Color(255, 255, 255, newColor.A - alpha);
			case 3065:
			case 4956:
				return new Color(255, 255, 255, newColor.A - alpha);
			case 75:
			case 3858:
				return new Color(255, 255, 255, 255);
			case 119:
			case 120:
			case 121:
			case 122:
			case 217:
			case 218:
			case 219:
			case 220:
				return new Color(255, 255, 255, 255);
			case 501:
				return new Color(200, 200, 200, 50);
			case 757:
			case 1306:
			case 3456:
			case 3457:
			case 3458:
			case 3459:
				return new Color(255, 255, 255, 200);
			case 520:
			case 521:
			case 522:
			case 547:
			case 548:
			case 549:
			case 575:
			case 1332:
			case 3453:
			case 3454:
			case 3455:
			case 3580:
				return new Color(255, 255, 255, 50);
			case 58:
			case 184:
			case 1734:
			case 1735:
			case 1867:
			case 1868:
				return new Color(200, 200, 200, 200);
			case 1572:
				return new Color(200, 200, 255, 125);
			case 787:
				return new Color(255, 255, 255, 175);
			case 1826:
				return new Color(255, 255, 255, 200);
			case 1508:
				return new Color(200, 200, 200, 0);
			case 502:
				return new Color(255, 255, 255, 150);
			case 51:
				return new Color(255, 255, 255, 0);
			case 1260:
				return new Color(255, 255, 255, 175);
			case 1446:
			case 1506:
			case 1507:
			case 1543:
			case 1544:
			case 1545:
				return new Color(newColor.R, newColor.G, newColor.B, Main.gFade);
			case 198:
			case 199:
			case 200:
			case 201:
			case 202:
			case 203:
				return Color.White;
			case 2763:
			case 2764:
			case 2765:
			case 2782:
			case 2783:
			case 2784:
			case 2785:
			case 2786:
			case 3522:
				return new Color(250, 250, 250, 255 - alpha);
			case 3191:
				return new Color(250, 250, 250, 200);
			case 3822:
				return Color.Lerp(Color.White, newColor, 0.5f) * ((255f - (float)alpha) / 255f);
			case 4143:
				return Color.Lerp(Color.White, newColor, 0f) * ((255f - (float)alpha) / 255f);
			case 4354:
			case 4377:
			case 4378:
			case 4389:
			case 5127:
			case 5128:
			{
				Color color = default(Color);
				color = type switch
				{
					4377 => new Color(50, 255, 50, 200), 
					4378 => new Color(50, 200, 255, 255), 
					4389 => new Color(255, 50, 125, 200), 
					5127 => new Color(150, 50, 250, 200), 
					5128 => new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 200), 
					_ => new Color(255, 150, 150, 200), 
				};
				if (newColor.R > color.R)
				{
					color.R = newColor.R;
				}
				if (newColor.G > color.G)
				{
					color.G = newColor.G;
				}
				if (newColor.B > color.B)
				{
					color.B = newColor.B;
				}
				if (newColor.A > color.A)
				{
					color.A = newColor.A;
				}
				return color;
			}
			default:
			{
				float num = (float)(255 - alpha) / 255f;
				int r = (int)((float)(int)newColor.R * num);
				int g = (int)((float)(int)newColor.G * num);
				int b = (int)((float)(int)newColor.B * num);
				int num2 = newColor.A - alpha;
				if (num2 < 0)
				{
					num2 = 0;
				}
				if (num2 > 255)
				{
					num2 = 255;
				}
				return new Color(r, g, b, num2);
			}
			}
		}

		public Color GetColor(Color newColor)
		{
			int num = color.R - (255 - newColor.R);
			int num2 = color.G - (255 - newColor.G);
			int num3 = color.B - (255 - newColor.B);
			int num4 = color.A - (255 - newColor.A);
			if (num < 0)
			{
				num = 0;
			}
			if (num > 255)
			{
				num = 255;
			}
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 > 255)
			{
				num2 = 255;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num3 > 255)
			{
				num3 = 255;
			}
			if (num4 < 0)
			{
				num4 = 0;
			}
			if (num4 > 255)
			{
				num4 = 255;
			}
			return new Color(num, num2, num3, num4);
		}

		public static bool MechSpawn(float x, float y, int type)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < 200; i++)
			{
				if (Main.item[i].active && Main.item[i].type == type)
				{
					num++;
					Vector2 vector = new Vector2(x, y);
					float num4 = Main.item[i].position.X - vector.X;
					float num5 = Main.item[i].position.Y - vector.Y;
					float num6 = (float)Math.Sqrt(num4 * num4 + num5 * num5);
					if (num6 < 300f)
					{
						num2++;
					}
					if (num6 < 800f)
					{
						num3++;
					}
				}
			}
			if (num2 >= 3 || num3 >= 6 || num >= 10)
			{
				return false;
			}
			return true;
		}

		public static int buyPrice(int platinum = 0, int gold = 0, int silver = 0, int copper = 0)
		{
			return copper + silver * 100 + gold * 100 * 100 + platinum * 100 * 100 * 100;
		}

		public static int sellPrice(int platinum = 0, int gold = 0, int silver = 0, int copper = 0)
		{
			return (copper + silver * 100 + gold * 100 * 100 + platinum * 100 * 100 * 100) * 5;
		}

		private void Shimmering()
		{
			bool flag = false;
			if ((type == 1326 || type == 779 || type == 3031 || type == 5364) && NPC.downedMoonlord)
			{
				flag = true;
			}
			if (type == 3461)
			{
				flag = true;
			}
			if (flag || ItemID.Sets.ShimmerTransformToItem[type] > 0 || FindDecraftAmount() > 0 || ItemID.Sets.CommonCoin[type] || makeNPC > 0)
			{
				int num = (int)(base.Center.X / 16f);
				int num2 = (int)(position.Y / 16f - 1f);
				if (!WorldGen.InWorld(num, num2) || Main.tile[num, num2] == null || Main.tile[num, num2].liquid <= 0 || !Main.tile[num, num2].shimmer())
				{
					return;
				}
				if (playerIndexTheItemIsReservedFor == Main.myPlayer && Main.netMode != 1)
				{
					shimmerTime += 0.01f;
					if (shimmerTime > 0.9f)
					{
						shimmerTime = 0.9f;
						GetShimmered();
					}
				}
				else
				{
					shimmerTime += 0.01f;
					if (shimmerTime > 1f)
					{
						shimmerTime = 1f;
					}
				}
			}
			else if (shimmerTime > 0f)
			{
				shimmerTime -= 0.01f;
				if (shimmerTime < 0f)
				{
					shimmerTime = 0f;
				}
			}
		}

		private int FindDecraftAmount()
		{
			if (ItemID.Sets.IsCrafted[type] < 0)
			{
				return -1;
			}
			return stack / Main.recipe[ItemID.Sets.IsCrafted[type]].createItem.stack;
		}

		private void GetShimmered()
		{
			if (ItemID.Sets.CommonCoin[type])
			{
				if (type == 72)
				{
					stack *= 100;
				}
				else if (type == 73)
				{
					stack *= 10000;
				}
				else if (type == 74)
				{
					if (stack > 1)
					{
						stack = 1;
					}
					stack *= 1000000;
				}
				Main.player[Main.myPlayer].AddCoinLuck(base.Center, stack);
				NetMessage.SendData(146, -1, -1, null, 1, (int)base.Center.X, (int)base.Center.Y, stack);
				type = 0;
				stack = 0;
			}
			else if (type == 1326 && NPC.downedMoonlord)
			{
				int num = stack;
				SetDefaults(5335);
				stack = num;
				shimmered = true;
			}
			else if (type == 779 && NPC.downedMoonlord)
			{
				int num2 = stack;
				SetDefaults(5134);
				stack = num2;
				shimmered = true;
			}
			else if (type == 3031 && NPC.downedMoonlord)
			{
				int num3 = stack;
				SetDefaults(5364);
				stack = num3;
				shimmered = true;
			}
			else if (type == 5364 && NPC.downedMoonlord)
			{
				int num4 = stack;
				SetDefaults(3031);
				stack = num4;
				shimmered = true;
			}
			else if (type == 3461)
			{
				short num5 = 3461;
				num5 = Main.GetMoonPhase() switch
				{
					MoonPhase.QuarterAtRight => 5407, 
					MoonPhase.HalfAtRight => 5405, 
					MoonPhase.ThreeQuartersAtRight => 5404, 
					MoonPhase.Full => 5408, 
					MoonPhase.ThreeQuartersAtLeft => 5401, 
					MoonPhase.HalfAtLeft => 5403, 
					MoonPhase.QuarterAtLeft => 5402, 
					_ => 5406, 
				};
				int num6 = stack;
				SetDefaults(num5);
				stack = num6;
				shimmered = true;
			}
			else if (ItemID.Sets.ShimmerTransformToItem[type] > 0)
			{
				int num7 = stack;
				SetDefaults(ItemID.Sets.ShimmerTransformToItem[type]);
				stack = num7;
				shimmered = true;
			}
			else if (makeNPC > 0)
			{
				int num8 = 50;
				int num9 = NPC.GetAvailableAmountOfNPCsToSpawnUpToSlot(stack, 50);
				while (num8 > 0 && num9 > 0 && stack > 0)
				{
					num8--;
					num9--;
					stack--;
					int num10 = -1;
					num10 = ((NPCID.Sets.ShimmerTransformToNPC[makeNPC] < 0) ? NPC.ReleaseNPC((int)base.Center.X, (int)base.Bottom.Y, makeNPC, placeStyle, Main.myPlayer) : NPC.ReleaseNPC((int)base.Center.X, (int)base.Bottom.Y, NPCID.Sets.ShimmerTransformToNPC[makeNPC], 0, Main.myPlayer));
					if (num10 >= 0)
					{
						Main.npc[num10].shimmerTransparency = 1f;
						NetMessage.SendData(146, -1, -1, null, 2, num10);
					}
				}
				shimmered = true;
				if (stack <= 0)
				{
					type = 0;
				}
			}
			else if (ItemID.Sets.IsCrafted[type] >= 0)
			{
				int num11 = FindDecraftAmount();
				Recipe recipe = Main.recipe[ItemID.Sets.IsCrafted[type]];
				if (WorldGen.crimson && ItemID.Sets.IsCraftedCrimson[type] >= 0)
				{
					recipe = Main.recipe[ItemID.Sets.IsCraftedCrimson[type]];
				}
				else if (!WorldGen.crimson && ItemID.Sets.IsCraftedCorruption[type] >= 0)
				{
					recipe = Main.recipe[ItemID.Sets.IsCraftedCorruption[type]];
				}
				int i = 0;
				int num12 = 0;
				for (; recipe.requiredItem[i].type > 0; i++)
				{
					num12++;
					int num13 = num11 * recipe.requiredItem[i].stack;
					if (recipe.alchemy)
					{
						for (int num14 = num13; num14 > 0; num14--)
						{
							if (Main.rand.Next(3) == 0)
							{
								num13--;
							}
						}
					}
					while (num13 > 0)
					{
						int num15 = num13;
						if (num15 > 9999)
						{
							num15 = 9999;
						}
						num13 -= num15;
						int num16 = NewItem(GetItemSource_Misc(8), (int)position.X, (int)position.Y, width, height, recipe.requiredItem[i].type);
						Main.item[num16].stack = num15;
						Main.item[num16].shimmerTime = 1f;
						Main.item[num16].shimmered = true;
						Main.item[num16].shimmerWet = true;
						Main.item[num16].wet = true;
						Main.item[num16].velocity *= 0.1f;
						Main.item[num16].playerIndexTheItemIsReservedFor = Main.myPlayer;
						if (recipe.requiredItem[1].stack > 0)
						{
							Main.item[num16].velocity.X = 1f * (float)num12;
							Main.item[num16].velocity.X *= 1f + (float)num12 * 0.05f;
							if (i % 2 == 0)
							{
								Main.item[num16].velocity.X *= -1f;
							}
						}
						NetMessage.SendData(145, -1, -1, null, num16, 1f);
					}
				}
				stack -= num11 * recipe.createItem.stack;
				if (stack <= 0)
				{
					stack = 0;
					type = 0;
				}
			}
			if (stack > 0)
			{
				shimmerTime = 1f;
			}
			else
			{
				shimmerTime = 0f;
			}
			shimmerWet = true;
			wet = true;
			velocity *= 0.1f;
			if (Main.netMode == 0)
			{
				ShimmerEffect(base.Center);
			}
			else
			{
				NetMessage.SendData(146, -1, -1, null, 0, (int)base.Center.X, (int)base.Center.Y);
				NetMessage.SendData(145, -1, -1, null, whoAmI, 1f);
			}
			AchievementsHelper.NotifyProgressionEvent(27);
			if (stack == 0)
			{
				makeNPC = -1;
				active = false;
			}
		}

		public static void ShimmerEffect(Vector2 shimmerPositon)
		{
			SoundEngine.PlaySound(SoundID.Item176, (int)shimmerPositon.X, (int)shimmerPositon.Y);
			for (int i = 0; i < 20; i++)
			{
				int num = Dust.NewDust(shimmerPositon, 1, 1, 309);
				Main.dust[num].scale *= 1.2f;
				switch (Main.rand.Next(6))
				{
				case 0:
					Main.dust[num].color = new Color(255, 255, 210);
					break;
				case 1:
					Main.dust[num].color = new Color(190, 245, 255);
					break;
				case 2:
					Main.dust[num].color = new Color(255, 150, 255);
					break;
				default:
					Main.dust[num].color = new Color(190, 175, 255);
					break;
				}
			}
		}

		public void FixAgainstExploit()
		{
			if (ItemID.Sets.ItemsThatShouldNotBeInInventory[type])
			{
				SetDefaults();
				return;
			}
			if (stack > maxStack)
			{
				stack = maxStack;
			}
			if (prefix != 0 && !CanRollPrefix(prefix))
			{
				ResetPrefix();
			}
		}

		public void UpdateItem(int i)
		{
			whoAmI = i;
			if (Main.timeItemSlotCannotBeReusedFor[i] > 0)
			{
				if (Main.netMode == 2)
				{
					Main.timeItemSlotCannotBeReusedFor[i]--;
					return;
				}
				Main.timeItemSlotCannotBeReusedFor[i] = 0;
			}
			if (!active)
			{
				return;
			}
			if (instanced)
			{
				if (Main.netMode == 2)
				{
					active = false;
					return;
				}
				keepTime = 6000;
				ownTime = 0;
				noGrabDelay = 0;
				playerIndexTheItemIsReservedFor = Main.myPlayer;
			}
			if (Main.netMode == 0)
			{
				playerIndexTheItemIsReservedFor = Main.myPlayer;
			}
			float gravity = 0.1f;
			float maxFallSpeed = 7f;
			if (Main.netMode == 1)
			{
				int num = (int)(position.X + (float)(width / 2)) / 16;
				int num2 = (int)(position.Y + (float)(height / 2)) / 16;
				if (num >= 0 && num2 >= 0 && num < Main.maxTilesX && num2 < Main.maxTilesY && Main.tile[num, num2] == null)
				{
					gravity = 0f;
					velocity.X = 0f;
					velocity.Y = 0f;
				}
			}
			Vector2 wetVelocity = velocity * 0.5f;
			if (shimmerWet)
			{
				gravity = 0.065f;
				maxFallSpeed = 4f;
				wetVelocity = velocity * 0.375f;
			}
			else if (honeyWet)
			{
				gravity = 0.05f;
				maxFallSpeed = 3f;
				wetVelocity = velocity * 0.25f;
			}
			else if (wet)
			{
				gravity = 0.08f;
				maxFallSpeed = 5f;
			}
			if (ownTime > 0)
			{
				ownTime--;
			}
			else
			{
				ownIgnore = -1;
			}
			if (keepTime > 0)
			{
				keepTime--;
			}
			if (!beingGrabbed)
			{
				if (shimmered)
				{
					if (Main.rand.Next(30) == 0)
					{
						int num3 = Dust.NewDust(position, width, height, 309);
						Main.dust[num3].position.X += Main.rand.Next(-8, 5);
						Main.dust[num3].position.Y += Main.rand.Next(-8, 5);
						Main.dust[num3].scale *= 1.1f;
						Main.dust[num3].velocity *= 0.3f;
						switch (Main.rand.Next(6))
						{
						case 0:
							Main.dust[num3].color = new Color(255, 255, 210);
							break;
						case 1:
							Main.dust[num3].color = new Color(190, 245, 255);
							break;
						case 2:
							Main.dust[num3].color = new Color(255, 150, 255);
							break;
						default:
							Main.dust[num3].color = new Color(190, 175, 255);
							break;
						}
					}
					Lighting.AddLight(base.Center, (1f - shimmerTime) * 0.8f, (1f - shimmerTime) * 0.8f, (1f - shimmerTime) * 0.8f);
					gravity = 0f;
					if (shimmerWet)
					{
						if (velocity.Y > -4f)
						{
							velocity.Y -= 0.05f;
						}
					}
					else
					{
						int num4 = 2;
						int num5 = (int)(base.Center.X / 16f);
						int num6 = (int)(base.Center.Y / 16f);
						bool flag = false;
						for (int j = num6; j < num6 + num4; j++)
						{
							if (WorldGen.InWorld(num5, j) && Main.tile[num5, j] != null && Main.tile[num5, j].shimmer() && Main.tile[num5, j].liquid > 0)
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							if (velocity.Y > -4f)
							{
								velocity.Y -= 0.05f;
							}
						}
						else
						{
							velocity.Y *= 0.9f;
						}
					}
				}
				if (shimmerWet && !shimmered)
				{
					Shimmering();
				}
				else if (shimmerTime > 0f)
				{
					shimmerTime -= 0.01f;
					if (shimmerTime < 0f)
					{
						shimmerTime = 0f;
					}
				}
				if (shimmerTime == 0f)
				{
					TryCombiningIntoNearbyItems(i);
				}
				if (timeLeftInWhichTheItemCannotBeTakenByEnemies > 0)
				{
					timeLeftInWhichTheItemCannotBeTakenByEnemies--;
				}
				if (timeLeftInWhichTheItemCannotBeTakenByEnemies == 0 && Main.netMode != 2 && playerIndexTheItemIsReservedFor == Main.myPlayer)
				{
					GetPickedUpByMonsters_Special(i);
					if (Main.expertMode && IsACoin)
					{
						GetPickedUpByMonsters_Money(i);
					}
				}
				MoveInWorld(gravity, maxFallSpeed, ref wetVelocity, i);
				if (lavaWet)
				{
					CheckLavaDeath(i);
				}
				DespawnIfMeetingConditions(i);
			}
			else
			{
				beingGrabbed = false;
			}
			UpdateItem_VisualEffects();
			if (timeSinceItemSpawned < 2147483547)
			{
				int num7 = ItemID.Sets.ItemSpawnDecaySpeed[type];
				timeSinceItemSpawned += num7;
			}
			if (Main.netMode == 2 && playerIndexTheItemIsReservedFor != Main.myPlayer)
			{
				timeSinceTheItemHasBeenReservedForSomeone++;
				if (timeSinceTheItemHasBeenReservedForSomeone >= 300)
				{
					timeSinceTheItemHasBeenReservedForSomeone = 0;
					NetMessage.SendData(39, playerIndexTheItemIsReservedFor, -1, null, i);
					playerIndexTheItemIsReservedFor = 255;
				}
			}
			if (wet)
			{
				position += wetVelocity;
			}
			else
			{
				position += velocity;
			}
			if (noGrabDelay > 0)
			{
				noGrabDelay--;
			}
		}

		private void DespawnIfMeetingConditions(int i)
		{
			if (type == 75 && Main.dayTime && !Main.remixWorld)
			{
				for (int j = 0; j < 10; j++)
				{
					Dust.NewDust(position, width, height, 15, velocity.X, velocity.Y, 150, default(Color), 1.2f);
				}
				for (int k = 0; k < 3; k++)
				{
					Gore.NewGore(position, new Vector2(velocity.X, velocity.Y), Main.rand.Next(16, 18));
				}
				active = false;
				type = 0;
				stack = 0;
				if (Main.netMode == 2)
				{
					NetMessage.SendData(21, -1, -1, null, i);
				}
			}
			if (type == 4143 && timeSinceItemSpawned > 300)
			{
				for (int l = 0; l < 20; l++)
				{
					Dust.NewDust(position, width, height, 15, velocity.X, velocity.Y, 150, Color.Lerp(Color.CornflowerBlue, Color.Indigo, Main.rand.NextFloat()), 1.2f);
				}
				active = false;
				type = 0;
				stack = 0;
				if (Main.netMode == 2)
				{
					NetMessage.SendData(21, -1, -1, null, i);
				}
			}
			if (type == 3822 && !DD2Event.Ongoing)
			{
				int num = Main.rand.Next(18, 24);
				for (int m = 0; m < num; m++)
				{
					int num2 = Dust.NewDust(base.Center, 0, 0, 61, 0f, 0f, 0, default(Color), 1.7f);
					Main.dust[num2].velocity *= 8f;
					Main.dust[num2].velocity.Y -= 1f;
					Main.dust[num2].position = Vector2.Lerp(Main.dust[num2].position, base.Center, 0.5f);
					Main.dust[num2].noGravity = true;
					Main.dust[num2].noLight = true;
				}
				active = false;
				type = 0;
				stack = 0;
				if (Main.netMode == 2)
				{
					NetMessage.SendData(21, -1, -1, null, i);
				}
			}
		}

		public void TryCombiningIntoNearbyItems(int i)
		{
			bool flag = true;
			int num = type;
			if ((uint)(num - 71) <= 3u)
			{
				flag = false;
			}
			if (ItemID.Sets.NebulaPickup[type])
			{
				flag = false;
			}
			if (playerIndexTheItemIsReservedFor == Main.myPlayer && flag)
			{
				CombineWithNearbyItems(i);
			}
		}

		public FlexibleTileWand GetFlexibleTileWand()
		{
			return type switch
			{
				5324 => FlexibleTileWand.RubblePlacementSmall, 
				5329 => FlexibleTileWand.RubblePlacementMedium, 
				5330 => FlexibleTileWand.RubblePlacementLarge, 
				_ => null, 
			};
		}

		private void CheckLavaDeath(int i)
		{
			if (type == 267)
			{
				if (Main.netMode == 1)
				{
					return;
				}
				int num = stack;
				active = false;
				type = 0;
				stack = 0;
				bool flag = false;
				for (int j = 0; j < 200; j++)
				{
					if (Main.npc[j].active && Main.npc[j].type == 22)
					{
						int num2 = -Main.npc[j].direction;
						if (Main.npc[j].IsNPCValidForBestiaryKillCredit())
						{
							Main.BestiaryTracker.Kills.RegisterKill(Main.npc[j]);
						}
						Main.npc[j].StrikeNPCNoInteraction(9999, 10f, -num2);
						num--;
						flag = true;
						if (Main.netMode == 2)
						{
							NetMessage.SendData(28, -1, -1, null, j, 9999f, 10f, -num2);
						}
						NPC.SpawnWOF(position);
					}
				}
				if (flag)
				{
					List<int> list = new List<int>();
					for (int k = 0; k < 200; k++)
					{
						if (num <= 0)
						{
							break;
						}
						NPC nPC = Main.npc[k];
						if (nPC.active && nPC.isLikeATownNPC)
						{
							list.Add(k);
						}
					}
					while (num > 0 && list.Count > 0)
					{
						int index = Main.rand.Next(list.Count);
						int num3 = list[index];
						list.RemoveAt(index);
						int num4 = -Main.npc[num3].direction;
						if (Main.npc[num3].IsNPCValidForBestiaryKillCredit())
						{
							Main.BestiaryTracker.Kills.RegisterKill(Main.npc[num3]);
						}
						Main.npc[num3].StrikeNPCNoInteraction(9999, 10f, -num4);
						num--;
						if (Main.netMode == 2)
						{
							NetMessage.SendData(28, -1, -1, null, num3, 9999f, 10f, -num4);
						}
					}
				}
				NetMessage.SendData(21, -1, -1, null, i);
			}
			else if (playerIndexTheItemIsReservedFor == Main.myPlayer && type != 312 && type != 318 && type != 173 && type != 174 && type != 4422 && type != 175 && type != 2701 && type != 205 && type != 206 && type != 207 && type != 1128 && type != 2340 && type != 2739 && type != 2492 && type != 1127 && rare == 0)
			{
				active = false;
				type = 0;
				stack = 0;
				if (Main.netMode != 0)
				{
					NetMessage.SendData(21, -1, -1, null, i);
				}
			}
		}

		private void MoveInWorld(float gravity, float maxFallSpeed, ref Vector2 wetVelocity, int i)
		{
			if (!shimmered && ItemID.Sets.ItemNoGravity[type])
			{
				velocity.X *= 0.95f;
				if ((double)velocity.X < 0.1 && (double)velocity.X > -0.1)
				{
					velocity.X = 0f;
				}
				velocity.Y *= 0.95f;
				if ((double)velocity.Y < 0.1 && (double)velocity.Y > -0.1)
				{
					velocity.Y = 0f;
				}
			}
			else
			{
				bool flag = false;
				if (shimmered && active)
				{
					int num = 50;
					for (int j = 0; j < 400; j++)
					{
						if (i == j || !Main.item[j].active || !Main.item[j].shimmered)
						{
							continue;
						}
						if (num-- <= 0)
						{
							break;
						}
						float num2 = (width + Main.item[j].width) / 2;
						if (!(Math.Abs(base.Center.X - Main.item[j].Center.X) <= num2) || !(Math.Abs(base.Center.Y - Main.item[j].Center.Y) <= num2))
						{
							continue;
						}
						flag = true;
						float num3 = Vector2.Distance(base.Center, Main.item[j].Center);
						num2 /= num3;
						if (num2 > 10f)
						{
							num2 = 10f;
						}
						if (base.Center.X < Main.item[j].Center.X)
						{
							if (velocity.X > -3f * num2)
							{
								velocity.X -= 0.1f * num2;
							}
							if (Main.item[j].velocity.X < 3f)
							{
								Main.item[j].velocity.X += 0.1f * num2;
							}
						}
						else if (base.Center.X > Main.item[j].Center.X)
						{
							if (velocity.X < 3f * num2)
							{
								velocity.X += 0.1f * num2;
							}
							if (Main.item[j].velocity.X > -3f)
							{
								Main.item[j].velocity.X -= 0.1f * num2;
							}
						}
						else if (i < j)
						{
							if (velocity.X > -3f * num2)
							{
								velocity.X -= 0.1f * num2;
							}
							if (Main.item[j].velocity.X < 3f * num2)
							{
								Main.item[j].velocity.X += 0.1f * num2;
							}
						}
					}
				}
				velocity.Y += gravity;
				if (velocity.Y > maxFallSpeed)
				{
					velocity.Y = maxFallSpeed;
				}
				velocity.X *= 0.95f;
				if ((double)velocity.X < 0.1 && (double)velocity.X > -0.1)
				{
					velocity.X = 0f;
				}
				if (flag)
				{
					velocity.X *= 0.8f;
				}
			}
			bool flag2 = Collision.LavaCollision(position, width, height);
			if (flag2)
			{
				lavaWet = true;
			}
			bool num4 = Collision.WetCollision(position, width, height);
			if (Collision.honey)
			{
				honeyWet = true;
			}
			if (Collision.shimmer)
			{
				shimmerWet = true;
			}
			if (num4)
			{
				if (!wet)
				{
					if (wetCount == 0)
					{
						wetCount = 20;
						if (!flag2)
						{
							if (shimmerWet)
							{
								for (int k = 0; k < 10; k++)
								{
									int num5 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 308);
									Main.dust[num5].velocity.Y -= 4f;
									Main.dust[num5].velocity.X *= 2.5f;
									Main.dust[num5].scale = 0.8f;
									Main.dust[num5].noGravity = true;
									switch (Main.rand.Next(6))
									{
									case 0:
										Main.dust[num5].color = new Color(255, 255, 210);
										break;
									case 1:
										Main.dust[num5].color = new Color(190, 245, 255);
										break;
									case 2:
										Main.dust[num5].color = new Color(255, 150, 255);
										break;
									default:
										Main.dust[num5].color = new Color(190, 175, 255);
										break;
									}
								}
								SoundEngine.PlaySound(19, (int)position.X, (int)position.Y, 4);
							}
							else if (honeyWet)
							{
								for (int l = 0; l < 5; l++)
								{
									int num6 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 152);
									Main.dust[num6].velocity.Y -= 1f;
									Main.dust[num6].velocity.X *= 2.5f;
									Main.dust[num6].scale = 1.3f;
									Main.dust[num6].alpha = 100;
									Main.dust[num6].noGravity = true;
								}
								SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
							}
							else
							{
								for (int m = 0; m < 10; m++)
								{
									int num7 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, Dust.dustWater());
									Main.dust[num7].velocity.Y -= 4f;
									Main.dust[num7].velocity.X *= 2.5f;
									Main.dust[num7].scale *= 0.8f;
									Main.dust[num7].alpha = 100;
									Main.dust[num7].noGravity = true;
								}
								SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
							}
						}
						else
						{
							for (int n = 0; n < 5; n++)
							{
								int num8 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 35);
								Main.dust[num8].velocity.Y -= 1.5f;
								Main.dust[num8].velocity.X *= 2.5f;
								Main.dust[num8].scale = 1.3f;
								Main.dust[num8].alpha = 100;
								Main.dust[num8].noGravity = true;
							}
							SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
						}
					}
					wet = true;
				}
			}
			else if (wet)
			{
				wet = false;
				if (wetCount == 0)
				{
					wetCount = 20;
					if (!lavaWet)
					{
						if (shimmerWet)
						{
							for (int num9 = 0; num9 < 10; num9++)
							{
								int num10 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 308);
								Main.dust[num10].velocity.Y -= 4f;
								Main.dust[num10].velocity.X *= 2.5f;
								Main.dust[num10].scale = 0.8f;
								Main.dust[num10].noGravity = true;
								switch (Main.rand.Next(6))
								{
								case 0:
									Main.dust[num10].color = new Color(255, 255, 210);
									break;
								case 1:
									Main.dust[num10].color = new Color(190, 245, 255);
									break;
								case 2:
									Main.dust[num10].color = new Color(255, 150, 255);
									break;
								default:
									Main.dust[num10].color = new Color(190, 175, 255);
									break;
								}
							}
							SoundEngine.PlaySound(19, (int)position.X, (int)position.Y, 5);
						}
						else if (honeyWet)
						{
							for (int num11 = 0; num11 < 5; num11++)
							{
								int num12 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 152);
								Main.dust[num12].velocity.Y -= 1f;
								Main.dust[num12].velocity.X *= 2.5f;
								Main.dust[num12].scale = 1.3f;
								Main.dust[num12].alpha = 100;
								Main.dust[num12].noGravity = true;
							}
							SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
						}
						else
						{
							for (int num13 = 0; num13 < 10; num13++)
							{
								int num14 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2)), width + 12, 24, Dust.dustWater());
								Main.dust[num14].velocity.Y -= 4f;
								Main.dust[num14].velocity.X *= 2.5f;
								Main.dust[num14].scale *= 0.8f;
								Main.dust[num14].alpha = 100;
								Main.dust[num14].noGravity = true;
							}
							SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
						}
					}
					else
					{
						for (int num15 = 0; num15 < 5; num15++)
						{
							int num16 = Dust.NewDust(new Vector2(position.X - 6f, position.Y + (float)(height / 2) - 8f), width + 12, 24, 35);
							Main.dust[num16].velocity.Y -= 1.5f;
							Main.dust[num16].velocity.X *= 2.5f;
							Main.dust[num16].scale = 1.3f;
							Main.dust[num16].alpha = 100;
							Main.dust[num16].noGravity = true;
						}
						SoundEngine.PlaySound(19, (int)position.X, (int)position.Y);
					}
				}
			}
			if (!wet)
			{
				lavaWet = false;
				honeyWet = false;
				shimmerWet = false;
			}
			if (wetCount > 0)
			{
				wetCount--;
			}
			if (wet)
			{
				if (wet)
				{
					Vector2 vector = velocity;
					velocity = Collision.TileCollision(position, velocity, width, height);
					if (velocity.X != vector.X)
					{
						wetVelocity.X = velocity.X;
					}
					if (velocity.Y != vector.Y)
					{
						wetVelocity.Y = velocity.Y;
					}
				}
			}
			else
			{
				velocity = Collision.TileCollision(position, velocity, width, height);
			}
			Vector4 vector2 = Collision.SlopeCollision(position, velocity, width, height, gravity);
			position.X = vector2.X;
			position.Y = vector2.Y;
			velocity.X = vector2.Z;
			velocity.Y = vector2.W;
			Collision.StepConveyorBelt(this, 1f);
		}

		private void GetPickedUpByMonsters_Special(int i)
		{
			bool flag = false;
			bool flag2 = false;
			int num = type;
			if ((num == 89 || num == 3507) && !NPC.unlockedSlimeCopperSpawn)
			{
				flag = true;
				flag2 = true;
			}
			if (!flag2)
			{
				return;
			}
			bool flag3 = false;
			Rectangle hitbox = base.Hitbox;
			for (int j = 0; j < 200; j++)
			{
				NPC nPC = Main.npc[j];
				if (nPC.active && flag && nPC.type == 1 && hitbox.Intersects(nPC.Hitbox))
				{
					flag3 = true;
					NPC.TransformCopperSlime(j);
					break;
				}
			}
			if (flag3)
			{
				SetDefaults();
				active = false;
				NetMessage.SendData(21, -1, -1, null, i);
			}
		}

		private void GetPickedUpByMonsters_Money(int i)
		{
			Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, width, height);
			for (int j = 0; j < 200; j++)
			{
				NPC nPC = Main.npc[j];
				if (!nPC.active || nPC.lifeMax <= 5 || nPC.friendly || nPC.immortal || nPC.dontTakeDamage || NPCID.Sets.CantTakeLunchMoney[nPC.type])
				{
					continue;
				}
				float num = stack;
				float num2 = 1f;
				if (type == 72)
				{
					num2 = 100f;
				}
				if (type == 73)
				{
					num2 = 10000f;
				}
				if (type == 74)
				{
					num2 = 1000000f;
				}
				num *= num2;
				float num3 = nPC.extraValue;
				int num4 = nPC.realLife;
				if (num4 >= 0 && Main.npc[num4].active)
				{
					num3 = Main.npc[num4].extraValue;
				}
				else
				{
					num4 = -1;
				}
				if (!(num3 < num) || !(num3 + num < 999000000f))
				{
					continue;
				}
				Rectangle rectangle2 = new Rectangle((int)nPC.position.X, (int)nPC.position.Y, nPC.width, nPC.height);
				if (rectangle.Intersects(rectangle2))
				{
					float num5 = (float)Main.rand.Next(50, 76) * 0.01f;
					if (type == 71)
					{
						num5 += (float)Main.rand.Next(51) * 0.01f;
					}
					if (type == 72)
					{
						num5 += (float)Main.rand.Next(26) * 0.01f;
					}
					if (num5 > 1f)
					{
						num5 = 1f;
					}
					int num6 = (int)((float)stack * num5);
					if (num6 < 1)
					{
						num6 = 1;
					}
					if (num6 > stack)
					{
						num6 = stack;
					}
					stack -= num6;
					int num7 = (int)((float)num6 * num2);
					int number = j;
					if (num4 >= 0)
					{
						number = num4;
					}
					nPC.extraValue += num7;
					if (Main.netMode == 0)
					{
						nPC.moneyPing(position);
					}
					else
					{
						NetMessage.SendData(92, -1, -1, null, number, num7, position.X, position.Y);
					}
					if (stack <= 0)
					{
						SetDefaults();
						active = false;
					}
					NetMessage.SendData(21, -1, -1, null, i);
				}
			}
		}

		private void CombineWithNearbyItems(int myItemIndex)
		{
			if (!CanCombineStackInWorld() || stack >= maxStack)
			{
				return;
			}
			for (int i = myItemIndex + 1; i < 400; i++)
			{
				Item item = Main.item[i];
				if (!item.active || item.type != type || item.shimmered != shimmered || item.stack <= 0 || item.playerIndexTheItemIsReservedFor != playerIndexTheItemIsReservedFor)
				{
					continue;
				}
				float num = Math.Abs(position.X + (float)(width / 2) - (item.position.X + (float)(item.width / 2))) + Math.Abs(position.Y + (float)(height / 2) - (item.position.Y + (float)(item.height / 2)));
				int num2 = 30;
				if ((double)numberOfNewItems > 40.0)
				{
					num2 *= 2;
				}
				if ((double)numberOfNewItems > 80.0)
				{
					num2 *= 2;
				}
				if ((double)numberOfNewItems > 120.0)
				{
					num2 *= 2;
				}
				if ((double)numberOfNewItems > 160.0)
				{
					num2 *= 2;
				}
				if ((double)numberOfNewItems > 200.0)
				{
					num2 *= 2;
				}
				if ((double)numberOfNewItems > 240.0)
				{
					num2 *= 2;
				}
				if (num < (float)num2)
				{
					position = (position + item.position) / 2f;
					velocity = (velocity + item.velocity) / 2f;
					int num3 = item.stack;
					if (num3 > maxStack - stack)
					{
						num3 = maxStack - stack;
					}
					item.stack -= num3;
					stack += num3;
					if (item.stack <= 0)
					{
						item.SetDefaults();
						item.active = false;
					}
					if (Main.netMode != 0 && playerIndexTheItemIsReservedFor == Main.myPlayer)
					{
						NetMessage.SendData(21, -1, -1, null, myItemIndex);
						NetMessage.SendData(21, -1, -1, null, i);
					}
				}
			}
		}

		public bool CanCombineStackInWorld()
		{
			int num = type;
			if (num == 75)
			{
				return false;
			}
			if (createTile < 0 && createWall <= 0 && (ammo <= 0 || notAmmo) && !consumable && (type < 205 || type > 207) && type != 1128 && type != 530 && dye <= 0 && !PaintOrCoating)
			{
				return material;
			}
			return true;
		}

		private void UpdateItem_VisualEffects()
		{
			if (type == 5043)
			{
				float num = (float)Main.rand.Next(90, 111) * 0.01f;
				num *= (Main.essScale + 0.5f) / 2f;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.25f * num, 0.25f * num, 0.25f * num);
			}
			else if (type == 3191)
			{
				float num2 = (float)Main.rand.Next(90, 111) * 0.01f;
				num2 *= (Main.essScale + 0.5f) / 2f;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.3f * num2, 0.1f * num2, 0.25f * num2);
			}
			else if (type == 520 || type == 3454)
			{
				float num3 = (float)Main.rand.Next(90, 111) * 0.01f;
				num3 *= Main.essScale;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f * num3, 0.1f * num3, 0.25f * num3);
			}
			else if (type == 521 || type == 3455)
			{
				float num4 = (float)Main.rand.Next(90, 111) * 0.01f;
				num4 *= Main.essScale;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.25f * num4, 0.1f * num4, 0.5f * num4);
			}
			else if (type == 547 || type == 3453)
			{
				float num5 = (float)Main.rand.Next(90, 111) * 0.01f;
				num5 *= Main.essScale;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f * num5, 0.3f * num5, 0.05f * num5);
			}
			else if (type == 548)
			{
				float num6 = (float)Main.rand.Next(90, 111) * 0.01f;
				num6 *= Main.essScale;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.1f * num6, 0.1f * num6, 0.6f * num6);
			}
			else if (type == 575)
			{
				float num7 = (float)Main.rand.Next(90, 111) * 0.01f;
				num7 *= Main.essScale;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.1f * num7, 0.3f * num7, 0.5f * num7);
			}
			else if (type == 549)
			{
				float num8 = (float)Main.rand.Next(90, 111) * 0.01f;
				num8 *= Main.essScale;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.1f * num8, 0.5f * num8, 0.2f * num8);
			}
			else if (type == 58 || type == 1734 || type == 1867)
			{
				float num9 = (float)Main.rand.Next(90, 111) * 0.01f;
				num9 *= Main.essScale * 0.5f;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f * num9, 0.1f * num9, 0.1f * num9);
			}
			else if (type == 184 || type == 1735 || type == 1868 || type == 4143)
			{
				float num10 = (float)Main.rand.Next(90, 111) * 0.01f;
				num10 *= Main.essScale * 0.5f;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.1f * num10, 0.1f * num10, 0.5f * num10);
			}
			else if (type == 522)
			{
				float num11 = (float)Main.rand.Next(90, 111) * 0.01f;
				num11 *= Main.essScale * 0.2f;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f * num11, 1f * num11, 0.1f * num11);
			}
			else if (type == 1332)
			{
				float num12 = (float)Main.rand.Next(90, 111) * 0.01f;
				num12 *= Main.essScale * 0.2f;
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f * num12, 1f * num12, 0.1f * num12);
			}
			else if (type == 3456)
			{
				Lighting.AddLight(base.Center, new Vector3(0.2f, 0.4f, 0.5f) * Main.essScale);
			}
			else if (type == 3457)
			{
				Lighting.AddLight(base.Center, new Vector3(0.4f, 0.2f, 0.5f) * Main.essScale);
			}
			else if (type == 3458)
			{
				Lighting.AddLight(base.Center, new Vector3(0.5f, 0.4f, 0.2f) * Main.essScale);
			}
			else if (type == 3459)
			{
				Lighting.AddLight(base.Center, new Vector3(0.2f, 0.2f, 0.5f) * Main.essScale);
			}
			else if (type == 501)
			{
				if (Main.rand.Next(6) == 0)
				{
					int num13 = Dust.NewDust(position, width, height, 55, 0f, 0f, 200, color);
					Main.dust[num13].velocity *= 0.3f;
					Main.dust[num13].scale *= 0.5f;
				}
			}
			else if (type == 3822)
			{
				Lighting.AddLight(base.Center, 0.1f, 0.3f, 0.1f);
			}
			else if (type == 1970)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0f, 0.75f);
			}
			else if (type == 1972)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0f, 0.75f);
			}
			else if (type == 1971)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0.75f, 0f);
			}
			else if (type == 1973)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0.75f, 0f);
			}
			else if (type == 1974)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0f, 0f);
			}
			else if (type == 1975)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0.75f, 0.75f);
			}
			else if (type == 1976)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0.375f, 0f);
			}
			else if (type == 2679)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.6f, 0f, 0.6f);
			}
			else if (type == 2687)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0f, 0.6f);
			}
			else if (type == 2689)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.6f, 0.6f, 0f);
			}
			else if (type == 2683)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0.6f, 0f);
			}
			else if (type == 2685)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.6f, 0f, 0f);
			}
			else if (type == 2681)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.6f, 0.6f, 0.6f);
			}
			else if (type == 2677)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.6f, 0.375f, 0f);
			}
			else if (type == 105)
			{
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f, 0.95f, 0.8f);
				}
			}
			else if (type == 2701)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.7f, 0.65f, 0.55f);
			}
			else if (createTile == 4)
			{
				int torchID = placeStyle;
				if ((!wet && ItemID.Sets.Torches[type]) || ItemID.Sets.WaterTorches[type])
				{
					Lighting.AddLight(base.Center, torchID);
				}
			}
			else if (type == 3114)
			{
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f, 0f, 1f);
				}
			}
			else if (type == 1245)
			{
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f, 0.5f, 0f);
				}
			}
			else if (type == 433)
			{
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch), 0.3f, 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch));
				}
			}
			else if (type == 523)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.85f, 1.2f, 0.7f);
			}
			else if (type == 974)
			{
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.75f, 0.85f, 1.4f);
				}
			}
			else if (type == 1333)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1.25f, 1.25f, 0.7f);
			}
			else if (type == 4383)
			{
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1.4f, 0.85f, 0.55f);
				}
			}
			else if (type == 5293)
			{
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.25f, 0.65f, 1f);
				}
			}
			else if (type == 5353)
			{
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.81f, 0.72f, 1f);
				}
			}
			else if (type == 4384)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.25f, 1.3f, 0.8f);
			}
			else if (type == 3045)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), (float)Main.DiscoR / 255f, (float)Main.DiscoG / 255f, (float)Main.DiscoB / 255f);
			}
			else if (type == 3004)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.95f, 0.65f, 1.3f);
			}
			else if (type == 2274)
			{
				float r = 0.75f;
				float g = 1.3499999f;
				float b = 1.5f;
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), r, g, b);
				}
			}
			else if (type >= 427 && type <= 432)
			{
				if (!wet)
				{
					float r2 = 0f;
					float g2 = 0f;
					float b2 = 0f;
					int num14 = type - 426;
					if (num14 == 1)
					{
						r2 = 0.1f;
						g2 = 0.2f;
						b2 = 1.1f;
					}
					if (num14 == 2)
					{
						r2 = 1f;
						g2 = 0.1f;
						b2 = 0.1f;
					}
					if (num14 == 3)
					{
						r2 = 0f;
						g2 = 1f;
						b2 = 0.1f;
					}
					if (num14 == 4)
					{
						r2 = 0.9f;
						g2 = 0f;
						b2 = 0.9f;
					}
					if (num14 == 5)
					{
						r2 = 1.3f;
						g2 = 1.3f;
						b2 = 1.3f;
					}
					if (num14 == 6)
					{
						r2 = 0.9f;
						g2 = 0.9f;
						b2 = 0f;
					}
					Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), r2, g2, b2);
				}
			}
			else if (type == 2777 || type == 2778 || type == 2779 || type == 2780 || type == 2781 || type == 2760 || type == 2761 || type == 2762 || type == 3524)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.4f, 0.16f, 0.36f);
			}
			else if (type == 2772 || type == 2773 || type == 2774 || type == 2775 || type == 2776 || type == 2757 || type == 2758 || type == 2759 || type == 3523)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0f, 0.36f, 0.4f);
			}
			else if (type == 2782 || type == 2783 || type == 2784 || type == 2785 || type == 2786 || type == 2763 || type == 2764 || type == 2765 || type == 3522)
			{
				Lighting.AddLight((int)((position.X + (float)(width / 2)) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.5f, 0.25f, 0.05f);
			}
			else if (type == 3462 || type == 3463 || type == 3464 || type == 3465 || type == 3466 || type == 3381 || type == 3382 || type == 3383 || type == 3525)
			{
				Lighting.AddLight(base.Center, 0.3f, 0.3f, 0.2f);
			}
			else if (type == 41)
			{
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f, 0.75f, 0.55f);
				}
			}
			else if (type == 988)
			{
				if (!wet)
				{
					Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.35f, 0.65f, 1f);
				}
			}
			else if (type == 1326)
			{
				Lighting.AddLight((int)base.Center.X / 16, (int)base.Center.Y / 16, 1f, 0.1f, 0.8f);
			}
			else if (type == 5335)
			{
				Lighting.AddLight((int)base.Center.X / 16, (int)base.Center.Y / 16, 0.85f, 0.1f, 0.8f);
			}
			else if (type >= 5140 && type <= 5146)
			{
				float num15 = 1f;
				float num16 = 1f;
				float num17 = 1f;
				switch (type)
				{
				case 5140:
					num15 *= 0.9f;
					num16 *= 0.8f;
					num17 *= 0.1f;
					break;
				case 5141:
					num15 *= 0.25f;
					num16 *= 0.1f;
					num17 *= 0f;
					break;
				case 5142:
					num15 *= 0f;
					num16 *= 0.25f;
					num17 *= 0f;
					break;
				case 5143:
					num15 *= 0f;
					num16 *= 0.16f;
					num17 *= 0.34f;
					break;
				case 5144:
					num15 *= 0.3f;
					num16 *= 0f;
					num17 *= 0.17f;
					break;
				case 5145:
					num15 *= 0.3f;
					num16 *= 0f;
					num17 *= 0.35f;
					break;
				case 5146:
					num15 *= (float)Main.DiscoR / 255f;
					num16 *= (float)Main.DiscoG / 255f;
					num17 *= (float)Main.DiscoB / 255f;
					break;
				}
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), num15, num16, num17);
			}
			else if (type == 282)
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.7f, 1f, 0.8f);
			}
			else if (type == 286)
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.7f, 0.8f, 1f);
			}
			else if (type == 3112)
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1f, 0.6f, 0.85f);
			}
			else if (type == 4776)
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.7f, 0f, 1f);
			}
			else if (type == 3002)
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 1.05f, 0.95f, 0.55f);
			}
			else if (type == 331)
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.55f, 0.75f, 0.6f);
			}
			else if (type == 183)
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.15f, 0.45f, 0.9f);
			}
			else if (type == 75)
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.8f, 0.7f, 0.1f);
				if (timeSinceItemSpawned % 12 == 0)
				{
					Dust dust = Dust.NewDustPerfect(base.Center + new Vector2(0f, (float)height * 0.2f) + Main.rand.NextVector2CircularEdge(width, (float)height * 0.6f) * (0.3f + Main.rand.NextFloat() * 0.5f), 228, new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.3f - 1.5f), 127);
					dust.scale = 0.5f;
					dust.fadeIn = 1.1f;
					dust.noGravity = true;
					dust.noLight = true;
				}
			}
			else if (ItemID.Sets.BossBag[type])
			{
				Lighting.AddLight((int)((position.X + (float)width) / 16f), (int)((position.Y + (float)(height / 2)) / 16f), 0.4f, 0.4f, 0.4f);
				if (timeSinceItemSpawned % 12 == 0)
				{
					Dust dust2 = Dust.NewDustPerfect(base.Center + new Vector2(0f, (float)height * -0.1f) + Main.rand.NextVector2CircularEdge((float)width * 0.6f, (float)height * 0.6f) * (0.3f + Main.rand.NextFloat() * 0.5f), 279, new Vector2(0f, (0f - Main.rand.NextFloat()) * 0.3f - 1.5f), 127);
					dust2.scale = 0.5f;
					dust2.fadeIn = 1.1f;
					dust2.noGravity = true;
					dust2.noLight = true;
					dust2.alpha = 0;
				}
			}
		}

		public static Rectangle GetDrawHitbox(int type, Player user)
		{
			Main.instance.LoadItem(type);
			if (ItemID.Sets.IsFood[type])
			{
				return TextureAssets.Item[type].Frame(1, 3, 0, 1);
			}
			switch (type)
			{
			case 75:
				return TextureAssets.Item[type].Frame(1, 8);
			case 520:
			case 521:
			case 547:
			case 548:
			case 549:
			case 575:
			case 3453:
			case 3454:
			case 3455:
			case 3580:
			case 3581:
			case 4068:
			case 4069:
			case 4070:
				return TextureAssets.Item[type].Frame(1, 4);
			default:
				return TextureAssets.Item[type].Frame();
			}
		}

		public static int NewItem(IEntitySource source, Vector2 pos, Vector2 randomBox, int Type, int Stack = 1, bool noBroadcast = false, int prefixGiven = 0, bool noGrabDelay = false, bool reverseLookup = false)
		{
			return NewItem(source, (int)pos.X, (int)pos.Y, (int)randomBox.X, (int)randomBox.Y, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
		}

		public static int NewItem(IEntitySource source, Vector2 pos, int Width, int Height, int Type, int Stack = 1, bool noBroadcast = false, int prefixGiven = 0, bool noGrabDelay = false, bool reverseLookup = false)
		{
			return NewItem(source, (int)pos.X, (int)pos.Y, Width, Height, Type, Stack, noBroadcast, prefixGiven, noGrabDelay, reverseLookup);
		}

		public static int NewItem(IEntitySource source, int X, int Y, int Width, int Height, int Type, int Stack = 1, bool noBroadcast = false, int pfix = 0, bool noGrabDelay = false, bool reverseLookup = false)
		{
			if (WorldGen.gen)
			{
				return 0;
			}
			if (Main.rand == null)
			{
				Main.rand = new UnifiedRandom();
			}
			if (Main.tenthAnniversaryWorld)
			{
				if (Type == 58)
				{
					Type = Main.rand.NextFromList(new short[3] { 1734, 1867, 58 });
				}
				if (Type == 184)
				{
					Type = Main.rand.NextFromList(new short[3] { 1735, 1868, 184 });
				}
			}
			if (Main.halloween)
			{
				if (Type == 58)
				{
					Type = 1734;
				}
				if (Type == 184)
				{
					Type = 1735;
				}
			}
			if (Main.xMas)
			{
				if (Type == 58)
				{
					Type = 1867;
				}
				if (Type == 184)
				{
					Type = 1868;
				}
			}
			if (Type > 0 && cachedItemSpawnsByType[Type] != -1)
			{
				cachedItemSpawnsByType[Type] += Stack;
				return 400;
			}
			Main.item[400] = new Item();
			int num = 400;
			if (Main.netMode != 1)
			{
				num = PickAnItemSlotToSpawnItemOn(reverseLookup, num);
			}
			Main.timeItemSlotCannotBeReusedFor[num] = 0;
			Main.item[num] = new Item();
			Item item = Main.item[num];
			item.SetDefaults(Type);
			item.Prefix(pfix);
			item.stack = Stack;
			item.position.X = X + Width / 2 - item.width / 2;
			item.position.Y = Y + Height / 2 - item.height / 2;
			item.wet = Collision.WetCollision(item.position, item.width, item.height);
			item.velocity.X = (float)Main.rand.Next(-30, 31) * 0.1f;
			item.velocity.Y = (float)Main.rand.Next(-40, -15) * 0.1f;
			if (Type == 859 || Type == 4743)
			{
				item.velocity *= 0f;
			}
			if (Type == 520 || Type == 521 || (item.type >= 0 && ItemID.Sets.NebulaPickup[item.type]))
			{
				item.velocity.X = (float)Main.rand.Next(-30, 31) * 0.1f;
				item.velocity.Y = (float)Main.rand.Next(-30, 31) * 0.1f;
			}
			item.active = true;
			item.timeSinceItemSpawned = ItemID.Sets.NewItemSpawnPriority[item.type];
			numberOfNewItems++;
			if (ItemSlot.Options.HighlightNewItems && item.type >= 0 && !ItemID.Sets.NeverAppearsAsNewInInventory[item.type])
			{
				item.newAndShiny = true;
			}
			if (Main.netMode == 2 && !noBroadcast)
			{
				NetMessage.SendData(21, -1, -1, null, num, noGrabDelay.ToInt());
			}
			else if (Main.netMode == 0)
			{
				item.playerIndexTheItemIsReservedFor = Main.myPlayer;
			}
			return num;
		}

		private static int PickAnItemSlotToSpawnItemOn(bool reverseLookup, int nextItem)
		{
			int num = 0;
			int num2 = 400;
			int num3 = 1;
			if (reverseLookup)
			{
				num = 399;
				num2 = -1;
				num3 = -1;
			}
			bool flag = false;
			for (int i = num; i != num2; i += num3)
			{
				if (!Main.item[i].active && Main.timeItemSlotCannotBeReusedFor[i] == 0)
				{
					nextItem = i;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				int num4 = 0;
				for (int j = 0; j < 400; j++)
				{
					if (Main.timeItemSlotCannotBeReusedFor[j] == 0 && !Main.item[j].instanced && Main.item[j].timeSinceItemSpawned > num4)
					{
						num4 = Main.item[j].timeSinceItemSpawned;
						nextItem = j;
						flag = true;
					}
				}
				if (!flag)
				{
					for (int k = 0; k < 400; k++)
					{
						if (Main.item[k].timeSinceItemSpawned - Main.timeItemSlotCannotBeReusedFor[k] > num4)
						{
							num4 = Main.item[k].timeSinceItemSpawned - Main.timeItemSlotCannotBeReusedFor[k];
							nextItem = k;
						}
					}
				}
			}
			return nextItem;
		}

		public void FindOwner(int whoAmI)
		{
			if (Main.netMode == 1 && shimmerTime > 0f)
			{
				keepTime = 0;
			}
			if (keepTime > 0)
			{
				return;
			}
			int num = playerIndexTheItemIsReservedFor;
			playerIndexTheItemIsReservedFor = 255;
			bool flag = true;
			if (type == 267 && ownIgnore != -1)
			{
				flag = false;
			}
			if (shimmerTime > 0f)
			{
				playerIndexTheItemIsReservedFor = 255;
			}
			else if (flag)
			{
				float num2 = NPC.sWidth;
				for (int i = 0; i < 255; i++)
				{
					if (ownIgnore == i)
					{
						continue;
					}
					Player player = Main.player[i];
					if (!player.active)
					{
						continue;
					}
					Player.ItemSpaceStatus status = player.ItemSpace(Main.item[whoAmI]);
					if (player.CanPullItem(Main.item[whoAmI], status))
					{
						float num3 = Math.Abs(player.position.X + (float)(player.width / 2) - position.X - (float)(width / 2)) + Math.Abs(player.position.Y + (float)(player.height / 2) - position.Y - (float)height);
						if (player.manaMagnet && (type == 184 || type == 1735 || type == 1868))
						{
							num3 -= (float)manaGrabRange;
						}
						if (player.lifeMagnet && (type == 58 || type == 1734 || type == 1867))
						{
							num3 -= (float)lifeGrabRange;
						}
						if (type == 4143)
						{
							num3 -= (float)manaGrabRange;
						}
						if (num2 > num3)
						{
							num2 = num3;
							playerIndexTheItemIsReservedFor = i;
						}
					}
				}
			}
			if (playerIndexTheItemIsReservedFor != num && ((num == Main.myPlayer && Main.netMode == 1) || (num == 255 && Main.netMode == 2) || (num != 255 && !Main.player[num].active)))
			{
				NetMessage.SendData(21, -1, -1, null, whoAmI);
				if (active)
				{
					NetMessage.SendData(22, -1, -1, null, whoAmI);
				}
			}
		}

		public Item Clone()
		{
			return (Item)MemberwiseClone();
		}

		public Item DeepClone()
		{
			return (Item)MemberwiseClone();
		}

		public bool IsTheSameAs(Item compareItem)
		{
			if (netID == compareItem.netID)
			{
				return type == compareItem.type;
			}
			return false;
		}

		public bool IsNotTheSameAs(Item compareItem)
		{
			if (netID == compareItem.netID && stack == compareItem.stack)
			{
				return prefix != compareItem.prefix;
			}
			return true;
		}

		public void SetNameOverride(string name)
		{
			_nameOverride = name;
		}

		public void ClearNameOverride()
		{
			_nameOverride = null;
		}

		public void TurnToAir(bool fullReset = false)
		{
			if (fullReset)
			{
				SetDefaults();
				return;
			}
			type = 0;
			stack = 0;
			prefix = 0;
			netID = 0;
			dye = 0;
			shoot = 0;
			mountType = -1;
		}

		public void OnPurchase(Item item)
		{
			if (item.shopCustomPrice.HasValue)
			{
				item.shopSpecialCurrency = -1;
				item.shopCustomPrice = null;
			}
		}

		public int GetStoreValue()
		{
			if (shopCustomPrice.HasValue)
			{
				return shopCustomPrice.Value;
			}
			return value;
		}

		public void Serialize(BinaryWriter writer, ItemSerializationContext context)
		{
			if (context == ItemSerializationContext.SavingAndLoading)
			{
				writer.Write(netID);
				writer.Write(stack);
				writer.Write(prefix);
			}
		}

		public void DeserializeFrom(BinaryReader reader, ItemSerializationContext context)
		{
			if (context == ItemSerializationContext.SavingAndLoading)
			{
				netDefaults(reader.ReadInt32());
				stack = reader.ReadInt32();
				Prefix(reader.ReadByte());
			}
			if (type >= 5453)
			{
				TurnToAir();
			}
		}

		public void ResetPrefix()
		{
			if (prefix != 0)
			{
				prefix = 0;
				Refresh(onlyIfVariantChanged: false);
			}
		}

		public void Refresh(bool onlyIfVariantChanged = true)
		{
			if (!IsAir && (!onlyIfVariantChanged || ItemVariants.SelectVariant(type) != Variant))
			{
				bool flag = favorited;
				int num = stack;
				int num2 = netID;
				int prefixWeWant = prefix;
				netDefaults(num2);
				Prefix(prefixWeWant);
				stack = num;
				favorited = flag;
			}
		}
	}
}
