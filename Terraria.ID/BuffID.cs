using ReLogic.Reflection;

namespace Terraria.ID
{
	public class BuffID
	{
		public class Sets
		{
			public class BuffMountData
			{
				public int mountID;

				public bool faceLeft;
			}

			public static SetFactory Factory = new SetFactory(355);

			public static bool[] IsWellFed = Factory.CreateBoolSet(26, 206, 207);

			public static bool[] IsFedState = Factory.CreateBoolSet(26, 206, 207, 332, 333, 334);

			public static bool[] IsAnNPCWhipDebuff = Factory.CreateBoolSet(307, 313, 319, 316, 310, 309, 315, 326, 340);

			public static bool[] TimeLeftDoesNotDecrease = Factory.CreateBoolSet(28, 334, 29, 159, 150, 93, 348);

			public static bool[] CanBeRemovedByNetMessage = Factory.CreateBoolSet(313);

			public static bool[] IsAFlaskBuff = Factory.CreateBoolSet(71, 72, 73, 74, 75, 76, 77, 78, 79);

			public static bool[] NurseCannotRemoveDebuff = Factory.CreateBoolSet(28, 34, 87, 89, 21, 86, 199, 332, 333, 334, 165, 146, 48, 158, 157, 350, 215, 147);

			public static BuffMountData[] BasicMountData = Factory.CreateCustomSet<BuffMountData>(null, new object[108]
			{
				118,
				new BuffMountData
				{
					mountID = 6,
					faceLeft = true
				},
				138,
				new BuffMountData
				{
					mountID = 6,
					faceLeft = false
				},
				184,
				new BuffMountData
				{
					mountID = 13,
					faceLeft = true
				},
				185,
				new BuffMountData
				{
					mountID = 13,
					faceLeft = false
				},
				211,
				new BuffMountData
				{
					mountID = 16,
					faceLeft = true
				},
				210,
				new BuffMountData
				{
					mountID = 16,
					faceLeft = false
				},
				209,
				new BuffMountData
				{
					mountID = 15,
					faceLeft = true
				},
				208,
				new BuffMountData
				{
					mountID = 15,
					faceLeft = false
				},
				221,
				new BuffMountData
				{
					mountID = 18,
					faceLeft = true
				},
				220,
				new BuffMountData
				{
					mountID = 18,
					faceLeft = false
				},
				223,
				new BuffMountData
				{
					mountID = 19,
					faceLeft = true
				},
				222,
				new BuffMountData
				{
					mountID = 19,
					faceLeft = false
				},
				225,
				new BuffMountData
				{
					mountID = 20,
					faceLeft = true
				},
				224,
				new BuffMountData
				{
					mountID = 20,
					faceLeft = false
				},
				227,
				new BuffMountData
				{
					mountID = 21,
					faceLeft = true
				},
				226,
				new BuffMountData
				{
					mountID = 21,
					faceLeft = false
				},
				229,
				new BuffMountData
				{
					mountID = 22,
					faceLeft = true
				},
				228,
				new BuffMountData
				{
					mountID = 22,
					faceLeft = false
				},
				232,
				new BuffMountData
				{
					mountID = 24,
					faceLeft = true
				},
				231,
				new BuffMountData
				{
					mountID = 24,
					faceLeft = false
				},
				234,
				new BuffMountData
				{
					mountID = 25,
					faceLeft = true
				},
				233,
				new BuffMountData
				{
					mountID = 25,
					faceLeft = false
				},
				236,
				new BuffMountData
				{
					mountID = 26,
					faceLeft = true
				},
				235,
				new BuffMountData
				{
					mountID = 26,
					faceLeft = false
				},
				238,
				new BuffMountData
				{
					mountID = 27,
					faceLeft = true
				},
				237,
				new BuffMountData
				{
					mountID = 27,
					faceLeft = false
				},
				240,
				new BuffMountData
				{
					mountID = 28,
					faceLeft = true
				},
				239,
				new BuffMountData
				{
					mountID = 28,
					faceLeft = false
				},
				242,
				new BuffMountData
				{
					mountID = 29,
					faceLeft = true
				},
				241,
				new BuffMountData
				{
					mountID = 29,
					faceLeft = false
				},
				244,
				new BuffMountData
				{
					mountID = 30,
					faceLeft = true
				},
				243,
				new BuffMountData
				{
					mountID = 30,
					faceLeft = false
				},
				246,
				new BuffMountData
				{
					mountID = 31,
					faceLeft = true
				},
				245,
				new BuffMountData
				{
					mountID = 31,
					faceLeft = false
				},
				248,
				new BuffMountData
				{
					mountID = 32,
					faceLeft = true
				},
				247,
				new BuffMountData
				{
					mountID = 32,
					faceLeft = false
				},
				250,
				new BuffMountData
				{
					mountID = 33,
					faceLeft = true
				},
				249,
				new BuffMountData
				{
					mountID = 33,
					faceLeft = false
				},
				252,
				new BuffMountData
				{
					mountID = 34,
					faceLeft = true
				},
				251,
				new BuffMountData
				{
					mountID = 34,
					faceLeft = false
				},
				254,
				new BuffMountData
				{
					mountID = 35,
					faceLeft = true
				},
				253,
				new BuffMountData
				{
					mountID = 35,
					faceLeft = false
				},
				256,
				new BuffMountData
				{
					mountID = 36,
					faceLeft = true
				},
				255,
				new BuffMountData
				{
					mountID = 36,
					faceLeft = false
				},
				270,
				new BuffMountData
				{
					mountID = 38,
					faceLeft = true
				},
				269,
				new BuffMountData
				{
					mountID = 38,
					faceLeft = false
				},
				273,
				new BuffMountData
				{
					mountID = 39,
					faceLeft = true
				},
				272,
				new BuffMountData
				{
					mountID = 39,
					faceLeft = false
				},
				339,
				new BuffMountData
				{
					mountID = 51,
					faceLeft = true
				},
				338,
				new BuffMountData
				{
					mountID = 51,
					faceLeft = false
				},
				347,
				new BuffMountData
				{
					mountID = 53,
					faceLeft = true
				},
				346,
				new BuffMountData
				{
					mountID = 53,
					faceLeft = false
				},
				167,
				new BuffMountData
				{
					mountID = 11,
					faceLeft = true
				},
				166,
				new BuffMountData
				{
					mountID = 11,
					faceLeft = false
				}
			});
		}

		public static readonly IdDictionary Search = IdDictionary.Create<BuffID, int>();

		public const int ObsidianSkin = 1;

		public const int Regeneration = 2;

		public const int Swiftness = 3;

		public const int Gills = 4;

		public const int Ironskin = 5;

		public const int ManaRegeneration = 6;

		public const int MagicPower = 7;

		public const int Featherfall = 8;

		public const int Spelunker = 9;

		public const int Invisibility = 10;

		public const int Shine = 11;

		public const int NightOwl = 12;

		public const int Battle = 13;

		public const int Thorns = 14;

		public const int WaterWalking = 15;

		public const int Archery = 16;

		public const int Hunter = 17;

		public const int Gravitation = 18;

		public const int ShadowOrb = 19;

		public const int Poisoned = 20;

		public const int PotionSickness = 21;

		public const int Darkness = 22;

		public const int Cursed = 23;

		public const int OnFire = 24;

		public const int Tipsy = 25;

		public const int WellFed = 26;

		public const int FairyBlue = 27;

		public const int Werewolf = 28;

		public const int Clairvoyance = 29;

		public const int Bleeding = 30;

		public const int Confused = 31;

		public const int Slow = 32;

		public const int Weak = 33;

		public const int Merfolk = 34;

		public const int Silenced = 35;

		public const int BrokenArmor = 36;

		public const int Horrified = 37;

		public const int TheTongue = 38;

		public const int CursedInferno = 39;

		public const int PetBunny = 40;

		public const int BabyPenguin = 41;

		public const int PetTurtle = 42;

		public const int PaladinsShield = 43;

		public const int Frostburn = 44;

		public const int BabyEater = 45;

		public const int Chilled = 46;

		public const int Frozen = 47;

		public const int Honey = 48;

		public const int Pygmies = 49;

		public const int BabySkeletronHead = 50;

		public const int BabyHornet = 51;

		public const int TikiSpirit = 52;

		public const int PetLizard = 53;

		public const int PetParrot = 54;

		public const int BabyTruffle = 55;

		public const int PetSapling = 56;

		public const int Wisp = 57;

		public const int RapidHealing = 58;

		public const int ShadowDodge = 59;

		public const int LeafCrystal = 60;

		public const int BabyDinosaur = 61;

		public const int IceBarrier = 62;

		public const int Panic = 63;

		public const int BabySlime = 64;

		public const int EyeballSpring = 65;

		public const int BabySnowman = 66;

		public const int Burning = 67;

		public const int Suffocation = 68;

		public const int Ichor = 69;

		public const int Venom = 70;

		public const int WeaponImbueVenom = 71;

		public const int Midas = 72;

		public const int WeaponImbueCursedFlames = 73;

		public const int WeaponImbueFire = 74;

		public const int WeaponImbueGold = 75;

		public const int WeaponImbueIchor = 76;

		public const int WeaponImbueNanites = 77;

		public const int WeaponImbueConfetti = 78;

		public const int WeaponImbuePoison = 79;

		public const int Blackout = 80;

		public const int PetSpider = 81;

		public const int Squashling = 82;

		public const int Ravens = 83;

		public const int BlackCat = 84;

		public const int CursedSapling = 85;

		public const int WaterCandle = 86;

		public const int Campfire = 87;

		public const int ChaosState = 88;

		public const int HeartLamp = 89;

		public const int Rudolph = 90;

		public const int Puppy = 91;

		public const int BabyGrinch = 92;

		public const int AmmoBox = 93;

		public const int ManaSickness = 94;

		public const int BeetleEndurance1 = 95;

		public const int BeetleEndurance2 = 96;

		public const int BeetleEndurance3 = 97;

		public const int BeetleMight1 = 98;

		public const int BeetleMight2 = 99;

		public const int BeetleMight3 = 100;

		public const int FairyRed = 101;

		public const int FairyGreen = 102;

		public const int Wet = 103;

		public const int Mining = 104;

		public const int Heartreach = 105;

		public const int Calm = 106;

		public const int Builder = 107;

		public const int Titan = 108;

		public const int Flipper = 109;

		public const int Summoning = 110;

		public const int Dangersense = 111;

		public const int AmmoReservation = 112;

		public const int Lifeforce = 113;

		public const int Endurance = 114;

		public const int Rage = 115;

		public const int Inferno = 116;

		public const int Wrath = 117;

		public const int MinecartLeft = 118;

		public const int Lovestruck = 119;

		public const int Stinky = 120;

		public const int Fishing = 121;

		public const int Sonar = 122;

		public const int Crate = 123;

		public const int Warmth = 124;

		public const int HornetMinion = 125;

		public const int ImpMinion = 126;

		public const int ZephyrFish = 127;

		public const int BunnyMount = 128;

		public const int PigronMount = 129;

		public const int SlimeMount = 130;

		public const int TurtleMount = 131;

		public const int BeeMount = 132;

		public const int SpiderMinion = 133;

		public const int TwinEyesMinion = 134;

		public const int PirateMinion = 135;

		public const int MiniMinotaur = 136;

		public const int Slimed = 137;

		public const int MinecartRight = 138;

		public const int SharknadoMinion = 139;

		public const int UFOMinion = 140;

		public const int UFOMount = 141;

		public const int DrillMount = 142;

		public const int ScutlixMount = 143;

		public const int Electrified = 144;

		public const int MoonLeech = 145;

		public const int Sunflower = 146;

		public const int MonsterBanner = 147;

		public const int Rabies = 148;

		public const int Webbed = 149;

		public const int Bewitched = 150;

		public const int SoulDrain = 151;

		public const int MagicLantern = 152;

		public const int ShadowFlame = 153;

		public const int BabyFaceMonster = 154;

		public const int CrimsonHeart = 155;

		public const int Stoned = 156;

		public const int PeaceCandle = 157;

		public const int StarInBottle = 158;

		public const int Sharpened = 159;

		public const int Dazed = 160;

		public const int DeadlySphere = 161;

		public const int UnicornMount = 162;

		public const int Obstructed = 163;

		public const int VortexDebuff = 164;

		public const int DryadsWard = 165;

		public const int MinecartRightMech = 166;

		public const int MinecartLeftMech = 167;

		public const int CuteFishronMount = 168;

		public const int BoneJavelin = 169;

		public const int SolarShield1 = 170;

		public const int SolarShield2 = 171;

		public const int SolarShield3 = 172;

		public const int NebulaUpLife1 = 173;

		public const int NebulaUpLife2 = 174;

		public const int NebulaUpLife3 = 175;

		public const int NebulaUpMana1 = 176;

		public const int NebulaUpMana2 = 177;

		public const int NebulaUpMana3 = 178;

		public const int NebulaUpDmg1 = 179;

		public const int NebulaUpDmg2 = 180;

		public const int NebulaUpDmg3 = 181;

		public const int StardustMinion = 182;

		public const int StardustMinionBleed = 183;

		public const int MinecartLeftWood = 184;

		public const int MinecartRightWood = 185;

		public const int DryadsWardDebuff = 186;

		public const int StardustGuardianMinion = 187;

		public const int StardustDragonMinion = 188;

		public const int Daybreak = 189;

		public const int SuspiciousTentacle = 190;

		public const int CompanionCube = 191;

		public const int SugarRush = 192;

		public const int BasiliskMount = 193;

		public const int WindPushed = 194;

		public const int WitheredArmor = 195;

		public const int WitheredWeapon = 196;

		public const int OgreSpit = 197;

		public const int ParryDamageBuff = 198;

		public const int NoBuilding = 199;

		public const int PetDD2Gato = 200;

		public const int PetDD2Ghost = 201;

		public const int PetDD2Dragon = 202;

		public const int BetsysCurse = 203;

		public const int Oiled = 204;

		public const int BallistaPanic = 205;

		public const int WellFed2 = 206;

		public const int WellFed3 = 207;

		public const int DesertMinecartRight = 208;

		public const int DesertMinecartLeft = 209;

		public const int FishMinecartRight = 210;

		public const int FishMinecartLeft = 211;

		public const int GolfCartMount = 212;

		public const int BatOfLight = 213;

		public const int VampireFrog = 214;

		public const int CatBast = 215;

		public const int BabyBird = 216;

		public const int UpbeatStar = 217;

		public const int SugarGlider = 218;

		public const int SharkPup = 219;

		public const int BeeMinecartRight = 220;

		public const int BeeMinecartLeft = 221;

		public const int LadybugMinecartRight = 222;

		public const int LadybugMinecartLeft = 223;

		public const int PigronMinecartRight = 224;

		public const int PigronMinecartLeft = 225;

		public const int SunflowerMinecartRight = 226;

		public const int SunflowerMinecartLeft = 227;

		public const int HellMinecartRight = 228;

		public const int HellMinecartLeft = 229;

		public const int WitchBroom = 230;

		public const int ShroomMinecartRight = 231;

		public const int ShroomMinecartLeft = 232;

		public const int AmethystMinecartRight = 233;

		public const int AmethystMinecartLeft = 234;

		public const int TopazMinecartRight = 235;

		public const int TopazMinecartLeft = 236;

		public const int SapphireMinecartRight = 237;

		public const int SapphireMinecartLeft = 238;

		public const int EmeraldMinecartRight = 239;

		public const int EmeraldMinecartLeft = 240;

		public const int RubyMinecartRight = 241;

		public const int RubyMinecartLeft = 242;

		public const int DiamondMinecartRight = 243;

		public const int DiamondMinecartLeft = 244;

		public const int AmberMinecartRight = 245;

		public const int AmberMinecartLeft = 246;

		public const int BeetleMinecartRight = 247;

		public const int BeetleMinecartLeft = 248;

		public const int MeowmereMinecartRight = 249;

		public const int MeowmereMinecartLeft = 250;

		public const int PartyMinecartRight = 251;

		public const int PartyMinecartLeft = 252;

		public const int PirateMinecartRight = 253;

		public const int PirateMinecartLeft = 254;

		public const int SteampunkMinecartRight = 255;

		public const int SteampunkMinecartLeft = 256;

		public const int Lucky = 257;

		public const int LilHarpy = 258;

		public const int FennecFox = 259;

		public const int GlitteryButterfly = 260;

		public const int BabyImp = 261;

		public const int BabyRedPanda = 262;

		public const int StormTiger = 263;

		public const int Plantero = 264;

		public const int Flamingo = 265;

		public const int DynamiteKitten = 266;

		public const int BabyWerewolf = 267;

		public const int ShadowMimic = 268;

		public const int CoffinMinecartRight = 269;

		public const int CoffinMinecartLeft = 270;

		public const int Smolstar = 271;

		public const int DiggingMoleMinecartRight = 272;

		public const int DiggingMoleMinecartLeft = 273;

		public const int VoltBunny = 274;

		public const int PaintedHorseMount = 275;

		public const int MajesticHorseMount = 276;

		public const int DarkHorseMount = 277;

		public const int PogoStickMount = 278;

		public const int PirateShipMount = 279;

		public const int SpookyWoodMount = 280;

		public const int SantankMount = 281;

		public const int WallOfFleshGoatMount = 282;

		public const int DarkMageBookMount = 283;

		public const int KingSlimePet = 284;

		public const int EyeOfCthulhuPet = 285;

		public const int EaterOfWorldsPet = 286;

		public const int BrainOfCthulhuPet = 287;

		public const int SkeletronPet = 288;

		public const int QueenBeePet = 289;

		public const int DestroyerPet = 290;

		public const int TwinsPet = 291;

		public const int SkeletronPrimePet = 292;

		public const int PlanteraPet = 293;

		public const int GolemPet = 294;

		public const int DukeFishronPet = 295;

		public const int LunaticCultistPet = 296;

		public const int MoonLordPet = 297;

		public const int FairyQueenPet = 298;

		public const int PumpkingPet = 299;

		public const int EverscreamPet = 300;

		public const int IceQueenPet = 301;

		public const int MartianPet = 302;

		public const int DD2OgrePet = 303;

		public const int DD2BetsyPet = 304;

		public const int LavaSharkMount = 305;

		public const int TitaniumStorm = 306;

		public const int BlandWhipEnemyDebuff = 307;

		public const int SwordWhipPlayerBuff = 308;

		public const int SwordWhipNPCDebuff = 309;

		public const int ScytheWhipEnemyDebuff = 310;

		public const int ScytheWhipPlayerBuff = 311;

		public const int CoolWhipPlayerBuff = 312;

		public const int FlameWhipEnemyDebuff = 313;

		public const int ThornWhipPlayerBuff = 314;

		public const int ThornWhipNPCDebuff = 315;

		public const int RainbowWhipNPCDebuff = 316;

		public const int QueenSlimePet = 317;

		public const int QueenSlimeMount = 318;

		public const int MaceWhipNPCDebuff = 319;

		public const int GelBalloonBuff = 320;

		public const int BrainOfConfusionBuff = 321;

		public const int EmpressBlade = 322;

		public const int OnFire3 = 323;

		public const int Frostburn2 = 324;

		public const int FlinxMinion = 325;

		public const int BoneWhipNPCDebuff = 326;

		public const int BerniePet = 327;

		public const int GlommerPet = 328;

		public const int DeerclopsPet = 329;

		public const int PigPet = 330;

		public const int ChesterPet = 331;

		public const int NeutralHunger = 332;

		public const int Hunger = 333;

		public const int Starving = 334;

		public const int AbigailMinion = 335;

		public const int HeartyMeal = 336;

		public const int TentacleSpike = 337;

		public const int FartMinecartRight = 338;

		public const int FartMinecartLeft = 339;

		public const int CoolWhipNPCDebuff = 340;

		public const int DualSlimePet = 341;

		public const int WolfMount = 342;

		public const int BiomeSight = 343;

		public const int BloodButcherer = 344;

		public const int JunimoPet = 345;

		public const int TerraFartMinecartRight = 346;

		public const int TerraFartMinecartLeft = 347;

		public const int WarTable = 348;

		public const int BlueChickenPet = 349;

		public const int ShadowCandle = 350;

		public const int Spiffo = 351;

		public const int CavelingGardener = 352;

		public const int Shimmer = 353;

		public const int DirtiestBlock = 354;

		public const int Count = 355;
	}
}
