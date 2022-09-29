using Terraria.Localization;

namespace Terraria.GameContent.ItemDropRules
{
	public class Conditions
	{
		public class NeverTrue : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class IsUsingSpecificAIValues : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public int aiSlotToCheck;

			public float valueToMatch;

			public IsUsingSpecificAIValues(int aislot, float valueToMatch)
			{
				aiSlotToCheck = aislot;
				this.valueToMatch = valueToMatch;
			}

			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.ai[aiSlotToCheck] == valueToMatch;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class FrostMoonDropGatingChance : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.snowMoon)
				{
					return false;
				}
				int num = NPC.waveNumber;
				if (Main.expertMode)
				{
					num += 5;
				}
				int num2 = (int)((double)(28 - num) / 2.5);
				if (Main.expertMode)
				{
					num2 -= 2;
				}
				if (num2 < 1)
				{
					num2 = 1;
				}
				return info.player.RollLuck(num2) == 0;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.WaveBasedDrop");
			}
		}

		public class PumpkinMoonDropGatingChance : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.pumpkinMoon)
				{
					return false;
				}
				int num = NPC.waveNumber;
				if (Main.expertMode)
				{
					num += 5;
				}
				int num2 = (int)((double)(24 - num) / 2.5);
				if (Main.expertMode)
				{
					num2--;
				}
				if (num2 < 1)
				{
					num2 = 1;
				}
				return info.player.RollLuck(num2) == 0;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.WaveBasedDrop");
			}
		}

		public class FrostMoonDropGateForTrophies : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.snowMoon)
				{
					return false;
				}
				int waveNumber = NPC.waveNumber;
				if (NPC.waveNumber < 15)
				{
					return false;
				}
				int num = 4;
				if (waveNumber == 16)
				{
					num = 4;
				}
				if (waveNumber == 17)
				{
					num = 3;
				}
				if (waveNumber == 18)
				{
					num = 3;
				}
				if (waveNumber == 19)
				{
					num = 2;
				}
				if (waveNumber >= 20)
				{
					num = 2;
				}
				if (Main.expertMode && Main.rand.Next(3) == 0)
				{
					num--;
				}
				return info.rng.Next(num) == 0;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class PumpkinMoonDropGateForTrophies : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.pumpkinMoon)
				{
					return false;
				}
				int waveNumber = NPC.waveNumber;
				if (NPC.waveNumber < 15)
				{
					return false;
				}
				int num = 4;
				if (waveNumber == 16)
				{
					num = 4;
				}
				if (waveNumber == 17)
				{
					num = 3;
				}
				if (waveNumber == 18)
				{
					num = 3;
				}
				if (waveNumber == 19)
				{
					num = 2;
				}
				if (waveNumber >= 20)
				{
					num = 2;
				}
				if (Main.expertMode && Main.rand.Next(3) == 0)
				{
					num--;
				}
				return info.rng.Next(num) == 0;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class IsPumpkinMoon : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.pumpkinMoon;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class FromCertainWaveAndAbove : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public int neededWave;

			public FromCertainWaveAndAbove(int neededWave)
			{
				this.neededWave = neededWave;
			}

			public bool CanDrop(DropAttemptInfo info)
			{
				return NPC.waveNumber >= neededWave;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.PastWaveBasedDrop", neededWave);
			}
		}

		public class IsBloodMoonAndNotFromStatue : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.dayTime && Main.bloodMoon && !info.npc.SpawnedFromStatue)
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class DownedAllMechBosses : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (NPC.downedMechBoss1 && NPC.downedMechBoss2)
				{
					return NPC.downedMechBoss3;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class DownedPlantera : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return NPC.downedPlantBoss;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class IsHardmode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.hardMode;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class FirstTimeKillingPlantera : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return !NPC.downedPlantBoss;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class MechanicalBossesDummyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return true;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class PirateMap : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.value > 0f && Main.hardMode && (double)(info.npc.position.Y / 16f) < Main.worldSurface + 10.0 && (info.npc.Center.X / 16f < 380f || info.npc.Center.X / 16f > (float)(Main.maxTilesX - 380)))
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.PirateMap");
			}
		}

		public class IsChristmas : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.xMas;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsChristmas");
			}
		}

		public class NotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.expertMode;
			}

			public bool CanShowItemDropInUI()
			{
				return !Main.expertMode;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.NotExpert");
			}
		}

		public class NotMasterMode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.masterMode;
			}

			public bool CanShowItemDropInUI()
			{
				return !Main.masterMode;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.NotMasterMode");
			}
		}

		public class MissingTwin : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				int type = 125;
				if (info.npc.type == 125)
				{
					type = 126;
				}
				return !NPC.AnyNPCs(type);
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class EmpressOfLightIsGenuinelyEnraged : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.AI_120_HallowBoss_IsGenuinelyEnraged();
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.EmpressOfLightOnlyTookDamageWhileEnraged");
			}
		}

		public class PlayerNeedsHealing : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.player.statLife < info.player.statLifeMax2;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.PlayerNeedsHealing");
			}
		}

		public class MechdusaKill : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			private static int[] _targetList = new int[4] { 127, 126, 125, 134 };

			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.remixWorld || !Main.getGoodWorld)
				{
					return false;
				}
				for (int i = 0; i < _targetList.Length; i++)
				{
					if (_targetList[i] != info.npc.type && NPC.AnyNPCs(_targetList[i]))
					{
						return false;
					}
				}
				return true;
			}

			public bool CanShowItemDropInUI()
			{
				if (Main.remixWorld)
				{
					return Main.getGoodWorld;
				}
				return false;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class LegacyHack_IsBossAndExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.boss)
				{
					return Main.expertMode;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return Main.expertMode;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.LegacyHack_IsBossAndExpert");
			}
		}

		public class LegacyHack_IsBossAndNotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.boss)
				{
					return !Main.expertMode;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return !Main.expertMode;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.LegacyHack_IsBossAndNotExpert");
			}
		}

		public class LegacyHack_IsABoss : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.boss;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class IsExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.expertMode;
			}

			public bool CanShowItemDropInUI()
			{
				return Main.expertMode;
			}

			public string GetConditionDescription()
			{
				if (Main.masterMode)
				{
					return Language.GetTextValue("Bestiary_ItemDropConditions.IsMasterMode");
				}
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsExpert");
			}
		}

		public class IsMasterMode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.masterMode;
			}

			public bool CanShowItemDropInUI()
			{
				return Main.masterMode;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsMasterMode");
			}
		}

		public class IsCrimson : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return WorldGen.crimson;
			}

			public bool CanShowItemDropInUI()
			{
				return WorldGen.crimson;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsCrimson");
			}
		}

		public class IsCorruption : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return !WorldGen.crimson;
			}

			public bool CanShowItemDropInUI()
			{
				return !WorldGen.crimson;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsCorruption");
			}
		}

		public class IsCrimsonAndNotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (WorldGen.crimson)
				{
					return !Main.expertMode;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				if (WorldGen.crimson)
				{
					return !Main.expertMode;
				}
				return false;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsCrimsonAndNotExpert");
			}
		}

		public class IsCorruptionAndNotExpert : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!WorldGen.crimson)
				{
					return !Main.expertMode;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				if (!WorldGen.crimson)
				{
					return !Main.expertMode;
				}
				return false;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsCorruptionAndNotExpert");
			}
		}

		public class HalloweenWeapons : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				float num = 500f * Main.GameModeInfo.EnemyMoneyDropMultiplier;
				float num2 = 40f * Main.GameModeInfo.EnemyDamageMultiplier;
				float num3 = 20f * Main.GameModeInfo.EnemyDefenseMultiplier;
				if (Main.halloween && info.npc.value > 0f && info.npc.value < num && (float)info.npc.damage < num2 && (float)info.npc.defense < num3)
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.HalloweenWeapons");
			}
		}

		public class SoulOfNight : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (SoulOfWhateverConditionCanDrop(info))
				{
					if (!info.player.ZoneCorrupt)
					{
						return info.player.ZoneCrimson;
					}
					return true;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.SoulOfNight");
			}
		}

		public class SoulOfLight : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (SoulOfWhateverConditionCanDrop(info))
				{
					return info.player.ZoneHallow;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.SoulOfLight");
			}
		}

		public class NotFromStatue : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return !info.npc.SpawnedFromStatue;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.NotFromStatue");
			}
		}

		public class HalloweenGoodieBagDrop : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Main.halloween && info.npc.lifeMax > 1 && info.npc.damage > 0 && !info.npc.friendly && info.npc.type != 121 && info.npc.type != 23 && info.npc.value > 0f)
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.HalloweenGoodieBagDrop");
			}
		}

		public class XmasPresentDrop : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Main.xMas && info.npc.lifeMax > 1 && info.npc.damage > 0 && !info.npc.friendly && info.npc.type != 121 && info.npc.type != 23 && info.npc.value > 0f)
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.XmasPresentDrop");
			}
		}

		public class LivingFlames : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.lifeMax > 5 && info.npc.value > 0f && !info.npc.friendly && Main.hardMode && info.npc.position.Y / 16f > (float)Main.UnderworldLayer)
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.LivingFlames");
			}
		}

		public class NamedNPC : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public string neededName;

			public NamedNPC(string neededName)
			{
				this.neededName = neededName;
			}

			public bool CanDrop(DropAttemptInfo info)
			{
				return info.npc.GivenOrTypeName == neededName;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.NamedNPC");
			}
		}

		public class HallowKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.value > 0f && Main.hardMode && !info.IsInSimulation)
				{
					return info.player.ZoneHallow;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.HallowKeyCondition");
			}
		}

		public class JungleKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.value > 0f && Main.hardMode && !info.IsInSimulation)
				{
					return info.player.ZoneJungle;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.JungleKeyCondition");
			}
		}

		public class CorruptKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.value > 0f && Main.hardMode && !info.IsInSimulation)
				{
					return info.player.ZoneCorrupt;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.CorruptKeyCondition");
			}
		}

		public class CrimsonKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.value > 0f && Main.hardMode && !info.IsInSimulation)
				{
					return info.player.ZoneCrimson;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.CrimsonKeyCondition");
			}
		}

		public class FrozenKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.value > 0f && Main.hardMode && !info.IsInSimulation)
				{
					return info.player.ZoneSnow;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.FrozenKeyCondition");
			}
		}

		public class DesertKeyCondition : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (info.npc.value > 0f && Main.hardMode && !info.IsInSimulation && info.player.ZoneDesert)
				{
					return !info.player.ZoneBeach;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.DesertKeyCondition");
			}
		}

		public class BeatAnyMechBoss : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return NPC.downedMechBossAny;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.BeatAnyMechBoss");
			}
		}

		public class YoyoCascade : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (!Main.hardMode && info.npc.HasPlayerTarget && info.npc.lifeMax > 5 && !info.npc.friendly && info.npc.position.Y / 16f > (float)(Main.maxTilesY - 350) && NPC.downedBoss3)
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.YoyoCascade");
			}
		}

		public class YoyosAmarok : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Main.hardMode && info.npc.HasPlayerTarget && info.player.ZoneSnow && info.npc.lifeMax > 5 && !info.npc.friendly && info.npc.value > 0f)
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.YoyosAmarok");
			}
		}

		public class YoyosYelets : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Main.hardMode && info.player.ZoneJungle && NPC.downedMechBossAny && info.npc.lifeMax > 5 && info.npc.HasPlayerTarget && !info.npc.friendly && info.npc.value > 0f)
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.YoyosYelets");
			}
		}

		public class YoyosKraken : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Main.hardMode && info.player.ZoneDungeon && NPC.downedPlantBoss && info.npc.lifeMax > 5 && info.npc.HasPlayerTarget && !info.npc.friendly && info.npc.value > 0f)
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.YoyosKraken");
			}
		}

		public class YoyosHelFire : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Main.hardMode && !info.player.ZoneDungeon && (double)(info.npc.position.Y / 16f) > (Main.rockLayer + (double)(Main.maxTilesY * 2)) / 3.0 && info.npc.lifeMax > 5 && info.npc.HasPlayerTarget && !info.npc.friendly && info.npc.value > 0f)
				{
					return !info.IsInSimulation;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.YoyosHelFire");
			}
		}

		public class WindyEnoughForKiteDrops : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.WindyEnoughForKiteDrops;
			}

			public bool CanShowItemDropInUI()
			{
				return true;
			}

			public string GetConditionDescription()
			{
				return Language.GetTextValue("Bestiary_ItemDropConditions.IsItAHappyWindyDay");
			}
		}

		public class RemixSeedEasymode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Main.remixWorld)
				{
					return !Main.hardMode;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				if (Main.remixWorld)
				{
					return !Main.hardMode;
				}
				return false;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class RemixSeedHardmode : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				if (Main.remixWorld)
				{
					return Main.hardMode;
				}
				return false;
			}

			public bool CanShowItemDropInUI()
			{
				if (Main.remixWorld)
				{
					return Main.hardMode;
				}
				return false;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class RemixSeed : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.remixWorld;
			}

			public bool CanShowItemDropInUI()
			{
				return Main.remixWorld;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class NotRemixSeed : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.remixWorld;
			}

			public bool CanShowItemDropInUI()
			{
				return !Main.remixWorld;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class TenthAnniversaryIsUp : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.tenthAnniversaryWorld;
			}

			public bool CanShowItemDropInUI()
			{
				return Main.tenthAnniversaryWorld;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class TenthAnniversaryIsNotUp : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.tenthAnniversaryWorld;
			}

			public bool CanShowItemDropInUI()
			{
				return !Main.tenthAnniversaryWorld;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class DontStarveIsUp : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return Main.dontStarveWorld;
			}

			public bool CanShowItemDropInUI()
			{
				return Main.dontStarveWorld;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public class DontStarveIsNotUp : IItemDropRuleCondition, IProvideItemConditionDescription
		{
			public bool CanDrop(DropAttemptInfo info)
			{
				return !Main.dontStarveWorld;
			}

			public bool CanShowItemDropInUI()
			{
				return !Main.dontStarveWorld;
			}

			public string GetConditionDescription()
			{
				return null;
			}
		}

		public static bool SoulOfWhateverConditionCanDrop(DropAttemptInfo info)
		{
			if (info.npc.boss)
			{
				return false;
			}
			switch (info.npc.type)
			{
			case 1:
			case 13:
			case 14:
			case 15:
			case 121:
			case 535:
				return false;
			default:
				if (Main.remixWorld)
				{
					if (!Main.hardMode || info.npc.lifeMax <= 1 || info.npc.friendly || info.npc.value < 1f)
					{
						return false;
					}
				}
				else if (!Main.hardMode || info.npc.lifeMax <= 1 || info.npc.friendly || (double)info.npc.position.Y <= Main.rockLayer * 16.0 || info.npc.value < 1f)
				{
					return false;
				}
				return true;
			}
		}
	}
}
