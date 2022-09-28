using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace Terraria.GameContent.ItemDropRules
{
	public class ItemDropDatabase
	{
		private List<IItemDropRule> _globalEntries = new List<IItemDropRule>();

		private Dictionary<int, List<IItemDropRule>> _entriesByNpcNetId = new Dictionary<int, List<IItemDropRule>>();

		private Dictionary<int, List<int>> _npcNetIdsByType = new Dictionary<int, List<int>>();

		private int _masterModeDropRng = 4;

		public void PrepareNPCNetIDsByTypeDictionary()
		{
			_npcNetIdsByType.Clear();
			foreach (KeyValuePair<int, NPC> item in ContentSamples.NpcsByNetId.Where((KeyValuePair<int, NPC> x) => x.Key < 0))
			{
				if (!_npcNetIdsByType.ContainsKey(item.Value.type))
				{
					_npcNetIdsByType[item.Value.type] = new List<int>();
				}
				_npcNetIdsByType[item.Value.type].Add(item.Value.netID);
			}
		}

		public void TrimDuplicateRulesForNegativeIDs()
		{
			for (int i = -65; i < 0; i++)
			{
				if (_entriesByNpcNetId.TryGetValue(i, out var value))
				{
					_entriesByNpcNetId[i] = value.Distinct().ToList();
				}
			}
		}

		public List<IItemDropRule> GetRulesForNPCID(int npcNetId, bool includeGlobalDrops = true)
		{
			List<IItemDropRule> list = new List<IItemDropRule>();
			if (includeGlobalDrops)
			{
				list.AddRange(_globalEntries);
			}
			if (_entriesByNpcNetId.TryGetValue(npcNetId, out var value))
			{
				list.AddRange(value);
			}
			return list;
		}

		public IItemDropRule RegisterToGlobal(IItemDropRule entry)
		{
			_globalEntries.Add(entry);
			return entry;
		}

		public IItemDropRule RegisterToNPC(int type, IItemDropRule entry)
		{
			RegisterToNPCNetId(type, entry);
			if (type > 0 && _npcNetIdsByType.TryGetValue(type, out var value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					RegisterToNPCNetId(value[i], entry);
				}
			}
			return entry;
		}

		private void RegisterToNPCNetId(int npcNetId, IItemDropRule entry)
		{
			if (!_entriesByNpcNetId.ContainsKey(npcNetId))
			{
				_entriesByNpcNetId[npcNetId] = new List<IItemDropRule>();
			}
			_entriesByNpcNetId[npcNetId].Add(entry);
		}

		public IItemDropRule RegisterToMultipleNPCs(IItemDropRule entry, params int[] npcNetIds)
		{
			for (int i = 0; i < npcNetIds.Length; i++)
			{
				RegisterToNPC(npcNetIds[i], entry);
			}
			return entry;
		}

		public IItemDropRule RegisterToMultipleNPCsNotRemixSeed(IItemDropRule entry, params int[] npcNetIds)
		{
			for (int i = 0; i < npcNetIds.Length; i++)
			{
				RegisterToNPC(npcNetIds[i], new LeadingConditionRule(new Conditions.NotRemixSeed())).OnSuccess(entry);
			}
			return entry;
		}

		public IItemDropRule RegisterToMultipleNPCsRemixSeed(IItemDropRule entry, params int[] npcNetIds)
		{
			for (int i = 0; i < npcNetIds.Length; i++)
			{
				RegisterToNPC(npcNetIds[i], new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(entry);
			}
			return entry;
		}

		private void RemoveFromNPCNetId(int npcNetId, IItemDropRule entry)
		{
			if (_entriesByNpcNetId.ContainsKey(npcNetId))
			{
				_entriesByNpcNetId[npcNetId].Remove(entry);
			}
		}

		public IItemDropRule RemoveFromNPC(int type, IItemDropRule entry)
		{
			RemoveFromNPCNetId(type, entry);
			if (type > 0 && _npcNetIdsByType.TryGetValue(type, out var value))
			{
				for (int i = 0; i < value.Count; i++)
				{
					RemoveFromNPCNetId(value[i], entry);
				}
			}
			return entry;
		}

		public IItemDropRule RemoveFromMultipleNPCs(IItemDropRule entry, params int[] npcNetIds)
		{
			for (int i = 0; i < npcNetIds.Length; i++)
			{
				RemoveFromNPC(npcNetIds[i], entry);
			}
			return entry;
		}

		public void Populate()
		{
			PrepareNPCNetIDsByTypeDictionary();
			RegisterGlobalRules();
			RegisterFoodDrops();
			RegisterWeirdRules();
			RegisterTownNPCDrops();
			RegisterDD2EventDrops();
			RegisterMiscDrops();
			RegisterHardmodeFeathers();
			RegisterYoyos();
			RegisterStatusImmunityItems();
			RegisterPirateDrops();
			RegisterBloodMoonFishingEnemies();
			RegisterMartianDrops();
			RegisterBossTrophies();
			RegisterBosses();
			RegisterHardmodeDungeonDrops();
			RegisterMimic();
			RegisterEclipse();
			RegisterBloodMoonFishing();
			TrimDuplicateRulesForNegativeIDs();
		}

		private void RegisterBloodMoonFishing()
		{
			RegisterToMultipleNPCs(ItemDropRule.Common(4608, 2, 4, 6), 587, 586);
			RegisterToMultipleNPCs(ItemDropRule.Common(4608, 2, 7, 10), 620, 621, 618);
			RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(8, 4273), 587, 586);
			RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(8, 4381), 587, 586);
			RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(8, 4325), 587, 586);
			RegisterToMultipleNPCs(ItemDropRule.Common(3213, 15), 587, 586);
			RegisterToNPC(620, ItemDropRule.Common(4270, 8));
			RegisterToNPC(620, ItemDropRule.Common(4317, 8));
			RegisterToNPC(621, ItemDropRule.Common(4272, 8));
			RegisterToNPC(621, ItemDropRule.Common(4317, 8));
			RegisterToNPC(618, ItemDropRule.NormalvsExpert(4269, 2, 1));
			RegisterToNPC(618, ItemDropRule.Common(4054, 10));
			RegisterToNPC(618, ItemDropRule.NormalvsExpert(4271, 2, 1));
			RegisterToMultipleNPCs(ItemDropRule.Common(4271, 5), 53, 536);
			Conditions.IsBloodMoonAndNotFromStatue condition = new Conditions.IsBloodMoonAndNotFromStatue();
			RegisterToMultipleNPCs(ItemDropRule.ByCondition(condition, 4271, 100), 489, 490);
			RegisterToMultipleNPCs(ItemDropRule.ByCondition(condition, 4271, 25), 587, 586, 621, 620);
		}

		private void RegisterEclipse()
		{
			RegisterToNPC(461, ItemDropRule.ExpertGetsRerolls(497, 50, 1));
			RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(900, 35, 1), 159, 158);
			RegisterToNPC(251, ItemDropRule.ExpertGetsRerolls(1311, 15, 1));
			RegisterToNPC(251, ItemDropRule.Common(5239, 15));
			RegisterToNPC(251, ItemDropRule.Common(5236, 15));
			RegisterToNPC(477, ItemDropRule.Common(5237, 15));
			RegisterToNPC(253, ItemDropRule.Common(5223, 60));
			RegisterToNPC(460, ItemDropRule.Common(5227, 60));
			RegisterToNPC(469, ItemDropRule.Common(5260, 60));
			RegisterToMultipleNPCs(ItemDropRule.Common(5261, 450), 166, 162);
			RegisterToNPC(462, ItemDropRule.Common(5262, 60));
			Conditions.DownedAllMechBosses condition = new Conditions.DownedAllMechBosses();
			Conditions.DownedPlantera condition2 = new Conditions.DownedPlantera();
			IItemDropRule rule = RegisterToNPC(477, new LeadingConditionRule(condition));
			IItemDropRule rule2 = rule.OnSuccess(new LeadingConditionRule(condition2));
			rule.OnSuccess(ItemDropRule.ExpertGetsRerolls(1570, 4, 1));
			rule2.OnSuccess(ItemDropRule.ExpertGetsRerolls(2770, 20, 1));
			rule2.OnSuccess(ItemDropRule.ExpertGetsRerolls(3292, 3, 1));
			RegisterToNPC(253, new LeadingConditionRule(condition)).OnSuccess(ItemDropRule.ExpertGetsRerolls(1327, 40, 1));
			RegisterToNPC(460, new LeadingConditionRule(condition2)).OnSuccess(ItemDropRule.ExpertGetsRerolls(3098, 40, 1));
			RegisterToNPC(460, ItemDropRule.ExpertGetsRerolls(4740, 50, 1));
			RegisterToNPC(460, ItemDropRule.ExpertGetsRerolls(4741, 50, 1));
			RegisterToNPC(460, ItemDropRule.ExpertGetsRerolls(4742, 50, 1));
			RegisterToNPC(468, new LeadingConditionRule(condition2)).OnSuccess(ItemDropRule.ExpertGetsRerolls(3105, 40, 1));
			RegisterToNPC(468, ItemDropRule.ExpertGetsRerolls(4738, 50, 1));
			RegisterToNPC(468, ItemDropRule.ExpertGetsRerolls(4739, 50, 1));
			RegisterToNPC(466, new LeadingConditionRule(condition2)).OnSuccess(ItemDropRule.ExpertGetsRerolls(3106, 40, 1));
			RegisterToNPC(467, new LeadingConditionRule(condition2)).OnSuccess(ItemDropRule.ExpertGetsRerolls(3249, 30, 1));
			IItemDropRule itemDropRule = ItemDropRule.Common(3107, 25);
			IItemDropRule itemDropRule2 = ItemDropRule.WithRerolls(3107, 1, 25);
			itemDropRule.OnSuccess(ItemDropRule.Common(3108, 1, 100, 200), hideLootReport: true);
			itemDropRule2.OnSuccess(ItemDropRule.Common(3108, 1, 100, 200), hideLootReport: true);
			RegisterToNPC(463, new LeadingConditionRule(condition2)).OnSuccess(new DropBasedOnExpertMode(itemDropRule, itemDropRule2));
		}

		private void RegisterMimic()
		{
			RegisterToNPC(85, new LeadingConditionRule(new Conditions.NotRemixSeed())).OnSuccess(ItemDropRule.OneFromOptions(1, 437, 517, 535, 536, 532, 554));
			RegisterToNPC(85, new LeadingConditionRule(new Conditions.RemixSeedHardmode())).OnSuccess(ItemDropRule.OneFromOptions(1, 437, 3069, 535, 536, 532, 554));
			RegisterToNPC(85, new LeadingConditionRule(new Conditions.RemixSeedEasymode())).OnSuccess(ItemDropRule.OneFromOptions(1, 49, 50, 53, 54, 5011, 975));
			IItemDropRule itemDropRule = ItemDropRule.Common(1312, 20);
			itemDropRule.OnFailedRoll(new LeadingConditionRule(new Conditions.NotRemixSeed())).OnSuccess(ItemDropRule.OneFromOptions(1, 676, 725, 1264));
			itemDropRule.OnFailedRoll(new LeadingConditionRule(new Conditions.RemixSeedHardmode())).OnSuccess(ItemDropRule.OneFromOptions(1, 676, 1319, 1264));
			itemDropRule.OnFailedRoll(new LeadingConditionRule(new Conditions.RemixSeedEasymode())).OnSuccess(ItemDropRule.OneFromOptions(1, 670, 724, 950, 725, 987, 1579));
			RegisterToNPC(629, itemDropRule);
		}

		private void RegisterHardmodeDungeonDrops()
		{
			int[] npcNetIds = new int[12]
			{
				269, 270, 271, 272, 273, 274, 275, 276, 277, 278,
				279, 280
			};
			RegisterToNPC(290, ItemDropRule.ExpertGetsRerolls(1513, 15, 1));
			RegisterToNPC(290, ItemDropRule.ExpertGetsRerolls(938, 10, 1));
			RegisterToNPC(287, ItemDropRule.ExpertGetsRerolls(977, 12, 1));
			RegisterToNPC(287, ItemDropRule.ExpertGetsRerolls(963, 12, 1));
			RegisterToNPC(291, ItemDropRule.ExpertGetsRerolls(1300, 12, 1));
			RegisterToNPC(291, ItemDropRule.ExpertGetsRerolls(1254, 12, 1));
			RegisterToNPC(292, ItemDropRule.ExpertGetsRerolls(1514, 12, 1));
			RegisterToNPC(292, ItemDropRule.ExpertGetsRerolls(679, 12, 1));
			RegisterToNPC(293, ItemDropRule.ExpertGetsRerolls(759, 18, 1));
			RegisterToNPC(289, ItemDropRule.ExpertGetsRerolls(4789, 25, 1));
			RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(1446, 20, 1), 281, 282);
			RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(1444, 20, 1), 283, 284);
			RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(1445, 20, 1), 285, 286);
			RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(1183, 400, 1), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(1266, 300, 1), npcNetIds);
			RegisterToMultipleNPCsNotRemixSeed(ItemDropRule.ExpertGetsRerolls(671, 200, 1), npcNetIds);
			RegisterToMultipleNPCsRemixSeed(ItemDropRule.ExpertGetsRerolls(2273, 200, 1), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.ExpertGetsRerolls(4679, 200, 1), npcNetIds);
			RegisterToNPC(288, ItemDropRule.Common(1508, 1, 1, 2));
		}

		private void RegisterBosses()
		{
			RegisterBoss_EOC();
			RegisterBoss_BOC();
			RegisterBoss_EOW();
			RegisterBoss_QueenBee();
			RegisterBoss_Skeletron();
			RegisterBoss_WOF();
			RegisterBoss_AncientCultist();
			RegisterBoss_MoonLord();
			RegisterBoss_LunarTowers();
			RegisterBoss_Betsy();
			RegisterBoss_Golem();
			RegisterBoss_DukeFishron();
			RegisterBoss_SkeletronPrime();
			RegisterBoss_TheDestroyer();
			RegisterBoss_Twins();
			RegisterBoss_Plantera();
			RegisterBoss_KingSlime();
			RegisterBoss_FrostMoon();
			RegisterBoss_PumpkinMoon();
			RegisterBoss_HallowBoss();
			RegisterBoss_QueenSlime();
			RegisterBoss_Deerclops();
		}

		private void RegisterBoss_QueenSlime()
		{
			short type = 657;
			RegisterToNPC(type, ItemDropRule.BossBag(4957));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4950));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4960, _masterModeDropRng));
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
			RegisterToNPC(type, leadingConditionRule);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4986, 1, 25, 75));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4959, 7));
			leadingConditionRule.OnSuccess(ItemDropRule.OneFromOptions(1, 4982, 4983, 4984));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4758, 4));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4981, 4));
			leadingConditionRule.OnSuccess(ItemDropRule.NotScalingWithLuck(4980, 3));
		}

		private void RegisterBoss_HallowBoss()
		{
			short type = 636;
			RegisterToNPC(type, ItemDropRule.BossBag(4782));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4949));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4811, _masterModeDropRng));
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
			RegisterToNPC(type, leadingConditionRule).OnSuccess(ItemDropRule.OneFromOptions(1, 4923, 4952, 4953, 4914));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4823, 15));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4778, 4));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4715, 50));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(4784, 7));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(5075, 20));
			LeadingConditionRule entry = new LeadingConditionRule(new Conditions.EmpressOfLightIsGenuinelyEnraged());
			RegisterToNPC(type, entry).OnSuccess(ItemDropRule.Common(5005));
		}

		private void RegisterBoss_PumpkinMoon()
		{
			Conditions.PumpkinMoonDropGatingChance condition = new Conditions.PumpkinMoonDropGatingChance();
			Conditions.PumpkinMoonDropGateForTrophies condition2 = new Conditions.PumpkinMoonDropGateForTrophies();
			new Conditions.IsPumpkinMoon();
			new Conditions.FromCertainWaveAndAbove(15);
			RegisterToNPC(315, ItemDropRule.ByCondition(condition, 1857, 20));
			int[] npcNetIds = new int[10] { 305, 306, 307, 308, 309, 310, 311, 312, 313, 314 };
			RegisterToMultipleNPCs(new LeadingConditionRule(condition), npcNetIds).OnSuccess(ItemDropRule.OneFromOptions(10, 1788, 1789, 1790));
			IItemDropRule rule = RegisterToNPC(325, new LeadingConditionRule(condition));
			IItemDropRule itemDropRule = ItemDropRule.Common(1835);
			itemDropRule.OnSuccess(ItemDropRule.Common(1836, 1, 30, 60), hideLootReport: true);
			rule.OnSuccess(new OneFromRulesRule(1, ItemDropRule.Common(1829), ItemDropRule.Common(1831), itemDropRule, ItemDropRule.Common(1837), ItemDropRule.Common(1845)));
			rule.OnSuccess(ItemDropRule.ByCondition(condition2, 1855));
			rule.OnSuccess(ItemDropRule.ByCondition(new Conditions.IsExpert(), 4444, 5));
			rule.OnSuccess(ItemDropRule.MasterModeCommonDrop(4941));
			rule.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4793, _masterModeDropRng));
			IItemDropRule itemDropRule2 = ItemDropRule.Common(1782);
			itemDropRule2.OnSuccess(ItemDropRule.Common(1783, 1, 50, 100), hideLootReport: true);
			IItemDropRule itemDropRule3 = ItemDropRule.Common(1784);
			itemDropRule3.OnSuccess(ItemDropRule.Common(1785, 1, 25, 50), hideLootReport: true);
			IItemDropRule rule2 = RegisterToNPC(327, new LeadingConditionRule(condition));
			rule2.OnSuccess(new OneFromRulesRule(1, itemDropRule2, itemDropRule3, ItemDropRule.Common(1811), ItemDropRule.Common(1826), ItemDropRule.Common(1801), ItemDropRule.Common(1802), ItemDropRule.Common(4680), ItemDropRule.Common(1798)));
			rule2.OnSuccess(ItemDropRule.ByCondition(condition2, 1856));
			rule2.OnSuccess(ItemDropRule.MasterModeCommonDrop(4942));
			rule2.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4812, _masterModeDropRng));
			RegisterToNPC(326, new DropBasedOnMasterAndExpertMode(new CommonDrop(1729, 1, 1, 3), new CommonDrop(1729, 1, 1, 4), new CommonDrop(1729, 1, 2, 4)));
			RegisterToNPC(325, new DropBasedOnMasterAndExpertMode(new CommonDrop(1729, 1, 15, 30), new CommonDrop(1729, 1, 25, 40), new CommonDrop(1729, 1, 30, 50)));
		}

		private void RegisterBoss_FrostMoon()
		{
			Conditions.FrostMoonDropGatingChance condition = new Conditions.FrostMoonDropGatingChance();
			Conditions.FrostMoonDropGateForTrophies condition2 = new Conditions.FrostMoonDropGateForTrophies();
			Conditions.FromCertainWaveAndAbove condition3 = new Conditions.FromCertainWaveAndAbove(15);
			IItemDropRule rule = RegisterToNPC(344, new LeadingConditionRule(condition));
			rule.OnSuccess(ItemDropRule.ByCondition(condition2, 1962));
			rule.OnSuccess(ItemDropRule.Common(1871, 15)).OnFailedRoll(ItemDropRule.OneFromOptions(1, 1916, 1928, 1930));
			rule.OnSuccess(ItemDropRule.MasterModeCommonDrop(4944));
			rule.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4813, _masterModeDropRng));
			IItemDropRule rule2 = RegisterToNPC(345, new LeadingConditionRule(condition));
			rule2.OnSuccess(ItemDropRule.ByCondition(condition2, 1960));
			rule2.OnSuccess(ItemDropRule.ByCondition(condition3, 1914, 15));
			rule2.OnSuccess(ItemDropRule.Common(1959, 15)).OnFailedRoll(ItemDropRule.OneFromOptions(1, 1931, 1946, 1947));
			rule2.OnSuccess(ItemDropRule.MasterModeCommonDrop(4943));
			rule2.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4814, _masterModeDropRng));
			IItemDropRule rule3 = RegisterToNPC(346, new LeadingConditionRule(condition));
			rule3.OnSuccess(ItemDropRule.ByCondition(condition2, 1961));
			rule3.OnSuccess(ItemDropRule.OneFromOptions(1, 1910, 1929));
			rule3.OnSuccess(ItemDropRule.MasterModeCommonDrop(4945));
			rule3.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4794, _masterModeDropRng));
			int[] npcNetIds = new int[3] { 338, 339, 340 };
			RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(200, 1943, 1944, 1945), npcNetIds);
			RegisterToNPC(341, ItemDropRule.ByCondition(new Conditions.IsChristmas(), 1869));
		}

		private void RegisterBoss_KingSlime()
		{
			short type = 50;
			RegisterToNPC(type, ItemDropRule.BossBag(3318));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4929));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4797, _masterModeDropRng));
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
			RegisterToNPC(type, leadingConditionRule);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(2430, 4));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(2493, 7));
			leadingConditionRule.OnSuccess(ItemDropRule.OneFromOptions(1, 256, 257, 258));
			leadingConditionRule.OnSuccess(ItemDropRule.NotScalingWithLuck(2585, 3)).OnFailedRoll(ItemDropRule.Common(2610));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(998));
		}

		private void RegisterBoss_Plantera()
		{
			short type = 262;
			RegisterToNPC(type, ItemDropRule.BossBag(3328));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4934));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4806, _masterModeDropRng));
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NotExpert());
			RegisterToNPC(type, leadingConditionRule);
			LeadingConditionRule leadingConditionRule2 = new LeadingConditionRule(new Conditions.FirstTimeKillingPlantera());
			leadingConditionRule.OnSuccess(leadingConditionRule2);
			leadingConditionRule.OnSuccess(ItemDropRule.Common(2109, 7));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(1141));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(1182, 20));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(1305, 50));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(1157, 4));
			leadingConditionRule.OnSuccess(ItemDropRule.Common(3021, 10));
			IItemDropRule itemDropRule = ItemDropRule.Common(758);
			itemDropRule.OnSuccess(ItemDropRule.Common(771, 1, 50, 150), hideLootReport: true);
			leadingConditionRule2.OnSuccess(itemDropRule, hideLootReport: true);
			leadingConditionRule2.OnFailedConditions(new OneFromRulesRule(1, itemDropRule, ItemDropRule.Common(1255), ItemDropRule.Common(788), ItemDropRule.Common(1178), ItemDropRule.Common(1259), ItemDropRule.Common(1155), ItemDropRule.Common(3018)));
		}

		private void RegisterBoss_SkeletronPrime()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 127;
			RegisterToNPC(type, ItemDropRule.BossBag(3327));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4933));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4805, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2107, 7));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 1225, 1, 15, 30));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 547, 1, 25, 40));
			RegisterToNPC(type, ItemDropRule.ByCondition(new Conditions.MechdusaKill(), 5382));
		}

		private void RegisterBoss_TheDestroyer()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 134;
			RegisterToNPC(type, ItemDropRule.BossBag(3325));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4932));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4803, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2113, 7));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 1225, 1, 15, 30));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 548, 1, 25, 40));
			RegisterToNPC(type, ItemDropRule.ByCondition(new Conditions.MechdusaKill(), 5382));
		}

		private void RegisterBoss_Twins()
		{
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.MissingTwin());
			LeadingConditionRule leadingConditionRule2 = new LeadingConditionRule(new Conditions.NotExpert());
			leadingConditionRule.OnSuccess(ItemDropRule.BossBag(3326));
			leadingConditionRule.OnSuccess(leadingConditionRule2);
			leadingConditionRule2.OnSuccess(ItemDropRule.Common(2106, 7));
			leadingConditionRule2.OnSuccess(ItemDropRule.Common(1225, 1, 15, 30));
			leadingConditionRule2.OnSuccess(ItemDropRule.Common(549, 1, 25, 40));
			leadingConditionRule.OnSuccess(ItemDropRule.MasterModeCommonDrop(4931));
			leadingConditionRule.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4804, _masterModeDropRng));
			RegisterToMultipleNPCs(leadingConditionRule, 126, 125);
			RegisterToMultipleNPCs(ItemDropRule.ByCondition(new Conditions.MechdusaKill(), 5382), 126, 125);
		}

		private void RegisterBoss_EOC()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			Conditions.IsCrimsonAndNotExpert condition2 = new Conditions.IsCrimsonAndNotExpert();
			Conditions.IsCorruptionAndNotExpert condition3 = new Conditions.IsCorruptionAndNotExpert();
			short type = 4;
			RegisterToNPC(type, ItemDropRule.BossBag(3319));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4924));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(3763));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4798, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2112, 7));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 1299, 40));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition2, 880, 1, 30, 90));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition2, 2171, 1, 1, 3));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition3, 47, 1, 20, 50));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition3, 56, 1, 30, 90));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition3, 59, 1, 1, 3));
		}

		private void RegisterBoss_BOC()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 266;
			RegisterToNPC(type, ItemDropRule.BossBag(3321));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4926));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4800, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 880, 1, 40, 90));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2104, 7));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 3060, 20));
			short type2 = 267;
			RegisterToNPC(type2, new DropBasedOnMasterAndExpertMode(new CommonDrop(1329, 3, 2, 5, 2), new CommonDrop(1329, 3, 1, 3, 2), new CommonDrop(1329, 4, 1, 2, 2)));
			RegisterToNPC(type2, new DropBasedOnMasterAndExpertMode(new CommonDrop(880, 3, 5, 12, 2), new CommonDrop(880, 3, 5, 7, 2), new CommonDrop(880, 3, 2, 4, 2)));
		}

		private void RegisterBoss_EOW()
		{
			Conditions.LegacyHack_IsBossAndExpert condition = new Conditions.LegacyHack_IsBossAndExpert();
			Conditions.LegacyHack_IsBossAndNotExpert condition2 = new Conditions.LegacyHack_IsBossAndNotExpert();
			int[] npcNetIds = new int[3] { 13, 14, 15 };
			RegisterToMultipleNPCs(new DropBasedOnMasterAndExpertMode(ItemDropRule.Common(86, 2, 1, 2), ItemDropRule.Common(86, 5, 1, 2), ItemDropRule.Common(86, 10, 1, 2)), npcNetIds);
			RegisterToMultipleNPCs(new DropBasedOnMasterAndExpertMode(ItemDropRule.Common(56, 2, 2, 5), ItemDropRule.Common(56, 2, 1, 3), ItemDropRule.Common(56, 3, 1, 2)), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.BossBagByCondition(condition, 3320), npcNetIds);
			IItemDropRule rule = RegisterToMultipleNPCs(new LeadingConditionRule(new Conditions.LegacyHack_IsABoss()), npcNetIds);
			rule.OnSuccess(ItemDropRule.MasterModeCommonDrop(4925));
			rule.OnSuccess(ItemDropRule.MasterModeDropOnAllPlayers(4799, _masterModeDropRng));
			RegisterToMultipleNPCs(ItemDropRule.ByCondition(condition2, 56, 1, 20, 60), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.ByCondition(condition2, 994, 20), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.ByCondition(condition2, 2111, 7), npcNetIds);
		}

		private void RegisterBoss_Deerclops()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 668;
			RegisterToNPC(type, ItemDropRule.BossBag(5111));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(5110));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(5090, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 5109, 7));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 5098, 3));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 5101, 3));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 5113, 3));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 5385, 14));
			RegisterToNPC(type, new LeadingConditionRule(condition)).OnSuccess(new OneFromRulesRule(1, ItemDropRule.OneFromOptionsNotScalingWithLuck(1, 5117, 5118, 5119, 5095)));
		}

		private void RegisterBoss_QueenBee()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 222;
			RegisterToNPC(type, ItemDropRule.BossBag(3322));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4928));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4802, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2108, 7));
			RegisterToNPC(type, new DropBasedOnExpertMode(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, 1121, 1123, 2888), ItemDropRule.DropNothing()));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 1132, 3));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 1170, 15));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2502, 20));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 1129, 3)).OnFailedRoll(ItemDropRule.OneFromOptionsNotScalingWithLuck(2, 842, 843, 844));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 1130, 4, 10, 30, 3));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2431, 1, 16, 26));
		}

		private void RegisterBoss_Skeletron()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 35;
			RegisterToNPC(type, ItemDropRule.BossBag(3323));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4927));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4801, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 1281, 7)).OnFailedRoll(ItemDropRule.Common(1273, 7)).OnFailedRoll(ItemDropRule.Common(1313, 7));
			RegisterToNPC(type, ItemDropRule.Common(4993, 7));
		}

		private void RegisterBoss_WOF()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 113;
			RegisterToNPC(type, ItemDropRule.BossBag(3324));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4930));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4795, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2105, 7));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 367));
			RegisterToNPC(type, new LeadingConditionRule(condition)).OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, 490, 491, 489, 2998));
			RegisterToNPC(type, new LeadingConditionRule(condition)).OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, 426, 434, 514, 4912));
		}

		private void RegisterBoss_AncientCultist()
		{
			short type = 439;
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4937));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4809, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.Common(3372, 7));
			RegisterToNPC(type, ItemDropRule.Common(3549));
		}

		private void RegisterBoss_MoonLord()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 398;
			RegisterToNPC(type, ItemDropRule.BossBag(3332));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4938));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4810, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 3373, 7));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 4469, 10));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 3384));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 3460, 1, 70, 90));
			RegisterToNPC(type, new LeadingConditionRule(condition)).OnSuccess(new FromOptionsWithoutRepeatsDropRule(2, 3063, 3389, 3065, 1553, 3930, 3541, 3570, 3571, 3569));
		}

		private void RegisterBoss_LunarTowers()
		{
			DropOneByOne.Parameters parameters = default(DropOneByOne.Parameters);
			parameters.MinimumItemDropsCount = 12;
			parameters.MaximumItemDropsCount = 20;
			parameters.ChanceNumerator = 1;
			parameters.ChanceDenominator = 1;
			parameters.MinimumStackPerChunkBase = 1;
			parameters.MaximumStackPerChunkBase = 3;
			parameters.BonusMinDropsPerChunkPerPlayer = 0;
			parameters.BonusMaxDropsPerChunkPerPlayer = 0;
			DropOneByOne.Parameters parameters2 = parameters;
			DropOneByOne.Parameters parameters3 = parameters2;
			parameters3.BonusMinDropsPerChunkPerPlayer = 1;
			parameters3.BonusMaxDropsPerChunkPerPlayer = 1;
			parameters3.MinimumStackPerChunkBase = (int)((float)parameters2.MinimumStackPerChunkBase * 1.5f);
			parameters3.MaximumStackPerChunkBase = (int)((float)parameters2.MaximumStackPerChunkBase * 1.5f);
			RegisterToNPC(517, new DropBasedOnExpertMode(new DropOneByOne(3458, parameters2), new DropOneByOne(3458, parameters3)));
			RegisterToNPC(422, new DropBasedOnExpertMode(new DropOneByOne(3456, parameters2), new DropOneByOne(3456, parameters3)));
			RegisterToNPC(507, new DropBasedOnExpertMode(new DropOneByOne(3457, parameters2), new DropOneByOne(3457, parameters3)));
			RegisterToNPC(493, new DropBasedOnExpertMode(new DropOneByOne(3459, parameters2), new DropOneByOne(3459, parameters3)));
		}

		private void RegisterBoss_Betsy()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 551;
			RegisterToNPC(type, ItemDropRule.BossBag(3860));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4948));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4817, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 3863, 7));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 3883, 4));
			RegisterToNPC(type, new LeadingConditionRule(condition)).OnSuccess(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, 3827, 3859, 3870, 3858));
		}

		private void RegisterBoss_Golem()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 245;
			RegisterToNPC(type, ItemDropRule.BossBag(3329));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4935));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4807, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2110, 7));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 1294, 4));
			IItemDropRule itemDropRule = ItemDropRule.Common(1258);
			itemDropRule.OnSuccess(ItemDropRule.Common(1261, 1, 60, 180), hideLootReport: true);
			RegisterToNPC(type, new LeadingConditionRule(condition)).OnSuccess(new OneFromRulesRule(1, itemDropRule, ItemDropRule.Common(1122), ItemDropRule.Common(899), ItemDropRule.Common(1248), ItemDropRule.Common(1295), ItemDropRule.Common(1296), ItemDropRule.Common(1297)));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2218, 1, 4, 8));
		}

		private void RegisterBoss_DukeFishron()
		{
			Conditions.NotExpert condition = new Conditions.NotExpert();
			short type = 370;
			RegisterToNPC(type, ItemDropRule.BossBag(3330));
			RegisterToNPC(type, ItemDropRule.MasterModeCommonDrop(4936));
			RegisterToNPC(type, ItemDropRule.MasterModeDropOnAllPlayers(4808, _masterModeDropRng));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2588, 7));
			RegisterToNPC(type, ItemDropRule.ByCondition(condition, 2609, 15));
			RegisterToNPC(type, new LeadingConditionRule(new Conditions.NotRemixSeed())).OnSuccess(new LeadingConditionRule(condition)).OnSuccess(ItemDropRule.OneFromOptions(1, 2611, 2624, 2622, 2621, 2623));
			RegisterToNPC(type, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(new LeadingConditionRule(condition)).OnSuccess(ItemDropRule.OneFromOptions(1, 2611, 2624, 2622, 2621, 157));
		}

		private void RegisterWeirdRules()
		{
			RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(3260, 40, 30), 86);
		}

		private void RegisterGlobalRules()
		{
			RegisterToGlobal(new MechBossSpawnersDropRule());
			RegisterToGlobal(new SlimeBodyItemDropRule());
			RegisterToGlobal(ItemDropRule.ByCondition(new Conditions.HalloweenWeapons(), 1825, 2000)).OnFailedRoll(ItemDropRule.Common(1827, 2000));
			RegisterToGlobal(new ItemDropWithConditionRule(1533, 2500, 1, 1, new Conditions.JungleKeyCondition()));
			RegisterToGlobal(new ItemDropWithConditionRule(1534, 2500, 1, 1, new Conditions.CorruptKeyCondition()));
			RegisterToGlobal(new ItemDropWithConditionRule(1535, 2500, 1, 1, new Conditions.CrimsonKeyCondition()));
			RegisterToGlobal(new ItemDropWithConditionRule(1536, 2500, 1, 1, new Conditions.HallowKeyCondition()));
			RegisterToGlobal(new ItemDropWithConditionRule(1537, 2500, 1, 1, new Conditions.FrozenKeyCondition()));
			RegisterToGlobal(new ItemDropWithConditionRule(4714, 2500, 1, 1, new Conditions.DesertKeyCondition()));
			RegisterToGlobal(new ItemDropWithConditionRule(1774, 80, 1, 1, new Conditions.HalloweenGoodieBagDrop()));
			RegisterToGlobal(new ItemDropWithConditionRule(1869, 13, 1, 1, new Conditions.XmasPresentDrop()));
			RegisterToGlobal(new ItemDropWithConditionRule(2701, 50, 20, 50, new Conditions.LivingFlames()));
			RegisterToGlobal(new ItemDropWithConditionRule(520, 5, 1, 1, new Conditions.SoulOfLight()));
			RegisterToGlobal(new ItemDropWithConditionRule(521, 5, 1, 1, new Conditions.SoulOfNight()));
			RegisterToGlobal(ItemDropRule.ByCondition(new Conditions.PirateMap(), 1315, 100));
		}

		private void RegisterFoodDrops()
		{
			RegisterToNPC(48, ItemDropRule.Food(4016, 50));
			RegisterToNPC(224, ItemDropRule.Food(4021, 50));
			RegisterToNPC(44, ItemDropRule.Food(4037, 10));
			RegisterToNPC(469, ItemDropRule.Food(4037, 100));
			RegisterToMultipleNPCs(ItemDropRule.Food(4020, 30), 163, 238, 164, 165, 530, 531);
			RegisterToMultipleNPCs(ItemDropRule.Food(4029, 50), 480, 481);
			RegisterToMultipleNPCs(ItemDropRule.Food(4030, 75), 498, 499, 500, 501, 502, 503, 504, 505, 506, 496, 497, 494, 495);
			RegisterToMultipleNPCs(ItemDropRule.Food(4036, 50), 482, 483);
			RegisterToMultipleNPCs(ItemDropRule.Food(4015, 100), 6, 173);
			RegisterToMultipleNPCs(ItemDropRule.Food(4026, 150), 150, 147, 184);
			RegisterToMultipleNPCs(ItemDropRule.Food(4027, 75), 154, 206);
			RegisterToMultipleNPCs(ItemDropRule.Food(3532, 15), 170, 180, 171);
			RegisterToNPC(289, ItemDropRule.Food(4018, 35));
			RegisterToNPC(34, ItemDropRule.Food(4018, 70));
			RegisterToMultipleNPCs(ItemDropRule.Food(4013, 21), 293, 291, 292);
			RegisterToMultipleNPCs(ItemDropRule.Food(5042, 30), 43, 175, 56);
			RegisterToNPC(287, ItemDropRule.Food(5042, 10));
			RegisterToMultipleNPCs(ItemDropRule.Food(5041, 150), 21, 201, 202, 203, 322, 323, 324, 635, 449, 450, 451, 452);
			RegisterToNPC(290, ItemDropRule.Food(4013, 7));
			RegisterToMultipleNPCs(ItemDropRule.Food(4025, 30), 39, 156);
			RegisterToMultipleNPCs(ItemDropRule.Food(4023, 40), 177, 152);
			RegisterToMultipleNPCs(ItemDropRule.Food(4012, 50), 581, 509, 580, 508, 69);
			RegisterToMultipleNPCs(ItemDropRule.Food(4028, 30), 546, 542, 544, 543, 545);
			RegisterToMultipleNPCs(ItemDropRule.Food(4035, 50), 67, 65);
			RegisterToMultipleNPCs(ItemDropRule.Food(4011, 150), 120, 137, 138);
			RegisterToNPC(122, ItemDropRule.Food(4017, 75));
		}

		private void RegisterTownNPCDrops()
		{
			RegisterToNPC(22, new ItemDropWithConditionRule(867, 1, 1, 1, new Conditions.NamedNPC("Andrew")));
			RegisterToNPC(178, new ItemDropWithConditionRule(4372, 1, 1, 1, new Conditions.NamedNPC("Whitney")));
			RegisterToNPC(227, new ItemDropWithConditionRule(5290, 1, 1, 1, new Conditions.NamedNPC("Jim")));
			RegisterToNPC(353, ItemDropRule.Common(3352, 8));
			RegisterToNPC(441, ItemDropRule.Common(3351, 8));
			RegisterToNPC(227, ItemDropRule.Common(3350, 10));
			RegisterToNPC(550, ItemDropRule.Common(3821, 6));
			RegisterToNPC(208, ItemDropRule.Common(3548, 4, 30, 60));
			RegisterToNPC(207, ItemDropRule.Common(3349, 8));
			RegisterToNPC(124, ItemDropRule.Common(4818, 8));
			RegisterToNPC(663, ItemDropRule.ByCondition(new Conditions.IsHardmode(), 5065, 8));
			RegisterToNPC(54, ItemDropRule.Common(260));
			RegisterToNPC(368, ItemDropRule.Common(2222));
		}

		private void RegisterDD2EventDrops()
		{
			RegisterToNPC(576, new CommonDropNotScalingWithLuck(3865, 7, 1, 1));
			RegisterToNPC(576, ItemDropRule.NormalvsExpertOneFromOptionsNotScalingWithLuck(3, 2, 3811, 3812));
			RegisterToNPC(576, ItemDropRule.NormalvsExpertOneFromOptionsNotScalingWithLuck(2, 1, 3852, 3854, 3823, 3835, 3836));
			RegisterToNPC(576, ItemDropRule.NormalvsExpertNotScalingWithLuck(3856, 5, 4));
			RegisterToNPC(577, new CommonDropNotScalingWithLuck(3865, 14, 1, 1));
			RegisterToNPC(577, ItemDropRule.MasterModeCommonDrop(4947));
			RegisterToNPC(577, ItemDropRule.MasterModeDropOnAllPlayers(4816, _masterModeDropRng));
			RegisterToNPC(577, ItemDropRule.OneFromOptionsNotScalingWithLuck(6, 3811, 3812));
			RegisterToNPC(577, ItemDropRule.OneFromOptionsNotScalingWithLuck(4, 3852, 3854, 3823, 3835, 3836));
			RegisterToNPC(577, ItemDropRule.Common(3856, 10));
			RegisterToNPC(564, ItemDropRule.Common(3864, 7));
			RegisterToNPC(564, ItemDropRule.MasterModeDropOnAllPlayers(4796, _masterModeDropRng));
			RegisterToNPC(564, ItemDropRule.NormalvsExpertOneFromOptionsNotScalingWithLuck(2, 1, 3810, 3809));
			RegisterToNPC(564, new OneFromRulesRule(5, ItemDropRule.NotScalingWithLuck(3814), ItemDropRule.NotScalingWithLuck(3815, 1, 4, 4)));
			RegisterToNPC(564, ItemDropRule.NormalvsExpertOneFromOptionsNotScalingWithLuck(3, 2, 3857, 3855));
			RegisterToNPC(565, ItemDropRule.Common(3864, 14));
			RegisterToNPC(565, ItemDropRule.MasterModeCommonDrop(4946));
			RegisterToNPC(565, ItemDropRule.MasterModeDropOnAllPlayers(4796, _masterModeDropRng));
			RegisterToNPC(565, ItemDropRule.OneFromOptionsNotScalingWithLuck(6, 3810, 3809));
			RegisterToNPC(565, new OneFromRulesRule(10, ItemDropRule.NotScalingWithLuck(3814), ItemDropRule.NotScalingWithLuck(3815, 1, 4, 4)));
			RegisterToNPC(565, ItemDropRule.OneFromOptionsNotScalingWithLuck(6, 3857, 3855));
		}

		private void RegisterHardmodeFeathers()
		{
			RegisterToNPC(156, ItemDropRule.Common(1518, 50));
			RegisterToNPC(243, ItemDropRule.Common(1519, 3));
			RegisterToMultipleNPCs(ItemDropRule.Common(1517, 300), 269, 270, 271, 272, 273, 274, 275, 276, 277, 278, 279, 280);
			RegisterToMultipleNPCs(ItemDropRule.Common(1520, 40), 159, 158);
			RegisterToNPC(48, ItemDropRule.Common(1516, 150));
			RegisterToNPC(176, new ItemDropWithConditionRule(1521, 100, 1, 1, new Conditions.BeatAnyMechBoss()));
			RegisterToNPC(205, new ItemDropWithConditionRule(1611, 2, 1, 1, new Conditions.BeatAnyMechBoss()));
		}

		private void RegisterYoyos()
		{
			RegisterToGlobal(new ItemDropWithConditionRule(3282, 400, 1, 1, new Conditions.YoyoCascade()));
			RegisterToGlobal(new ItemDropWithConditionRule(3289, 300, 1, 1, new Conditions.YoyosAmarok()));
			RegisterToGlobal(new ItemDropWithConditionRule(3286, 200, 1, 1, new Conditions.YoyosYelets()));
			RegisterToGlobal(new ItemDropWithConditionRule(3291, 400, 1, 1, new Conditions.YoyosKraken()));
			RegisterToGlobal(new ItemDropWithConditionRule(3290, 400, 1, 1, new Conditions.YoyosHelFire()));
		}

		private void RegisterStatusImmunityItems()
		{
			RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(885, 100), 104, 102, 269, 270, 271, 272);
			RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(886, 100), 77, 273, 274, 275, 276);
			RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(887, 100), 141, 176, 42, 231, 232, 233, 234, 235);
			RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(888, 100), 81, 79, 183, 630);
			RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(889, 100), 78, 82, 75);
			RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(890, 100), 103, 75, 79, 630);
			RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(891, 100), 34, 83, 84, 179, 289);
			RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(892, 100), 94, 182);
			RegisterToMultipleNPCs(ItemDropRule.StatusImmunityItem(893, 100), 93, 109, 80);
		}

		private void RegisterPirateDrops()
		{
			int[] npcNetIds = new int[4] { 212, 213, 214, 215 };
			RegisterToMultipleNPCs(ItemDropRule.Common(905, 4000), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(855, 2000), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(854, 1000), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2584, 1000), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(3033, 500), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(672, 200), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(1277, 500), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(1278, 500), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(1279, 500), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(1280, 500), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(1704, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(1705, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(1710, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(1716, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(1720, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2379, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2389, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2405, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2843, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(3885, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2663, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(3904, 150, 80, 130), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(3910, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2238, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2133, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2137, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2143, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2147, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2151, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2155, 300), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(3263, 500), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(3264, 500), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(3265, 500), npcNetIds);
			RegisterToNPC(216, ItemDropRule.Common(905, 1000));
			RegisterToNPC(216, ItemDropRule.Common(855, 500));
			RegisterToNPC(216, ItemDropRule.Common(854, 250));
			RegisterToNPC(216, ItemDropRule.Common(2584, 250));
			RegisterToNPC(216, ItemDropRule.Common(3033, 125));
			RegisterToNPC(216, ItemDropRule.Common(672, 50));
			RegisterToNPC(491, ItemDropRule.Common(905, 50));
			RegisterToNPC(491, ItemDropRule.Common(855, 15));
			RegisterToNPC(491, ItemDropRule.Common(854, 15));
			RegisterToNPC(491, ItemDropRule.Common(2584, 15));
			RegisterToNPC(491, ItemDropRule.Common(3033, 15));
			RegisterToNPC(491, ItemDropRule.Common(4471, 20));
			RegisterToNPC(491, ItemDropRule.Common(672, 10));
			RegisterToNPC(491, ItemDropRule.MasterModeCommonDrop(4940));
			RegisterToNPC(491, ItemDropRule.MasterModeDropOnAllPlayers(4792, _masterModeDropRng));
			RegisterToNPC(491, ItemDropRule.OneFromOptions(1, 1704, 1705, 1710, 1716, 1720, 2379, 2389, 2405, 2843, 3885, 2663, 3910, 2238, 2133, 2137, 2143, 2147, 2151, 2155));
		}

		private void RegisterBloodMoonFishingEnemies()
		{
		}

		private void RegisterBossTrophies()
		{
			Conditions.LegacyHack_IsABoss condition = new Conditions.LegacyHack_IsABoss();
			RegisterToNPC(4, ItemDropRule.ByCondition(condition, 1360, 10));
			RegisterToNPC(13, ItemDropRule.ByCondition(condition, 1361, 10));
			RegisterToNPC(14, ItemDropRule.ByCondition(condition, 1361, 10));
			RegisterToNPC(15, ItemDropRule.ByCondition(condition, 1361, 10));
			RegisterToNPC(266, ItemDropRule.ByCondition(condition, 1362, 10));
			RegisterToNPC(35, ItemDropRule.ByCondition(condition, 1363, 10));
			RegisterToNPC(222, ItemDropRule.ByCondition(condition, 1364, 10));
			RegisterToNPC(113, ItemDropRule.ByCondition(condition, 1365, 10));
			RegisterToNPC(134, ItemDropRule.ByCondition(condition, 1366, 10));
			RegisterToNPC(127, ItemDropRule.ByCondition(condition, 1367, 10));
			RegisterToNPC(262, ItemDropRule.ByCondition(condition, 1370, 10));
			RegisterToNPC(245, ItemDropRule.ByCondition(condition, 1371, 10));
			RegisterToNPC(50, ItemDropRule.ByCondition(condition, 2489, 10));
			RegisterToNPC(370, ItemDropRule.ByCondition(condition, 2589, 10));
			RegisterToNPC(439, ItemDropRule.ByCondition(condition, 3357, 10));
			RegisterToNPC(395, ItemDropRule.ByCondition(condition, 3358, 10));
			RegisterToNPC(398, ItemDropRule.ByCondition(condition, 3595, 10));
			RegisterToNPC(636, ItemDropRule.ByCondition(condition, 4783, 10));
			RegisterToNPC(657, ItemDropRule.ByCondition(condition, 4958, 10));
			RegisterToNPC(668, ItemDropRule.ByCondition(condition, 5108, 10));
			RegisterToNPC(125, ItemDropRule.Common(1368, 10));
			RegisterToNPC(126, ItemDropRule.Common(1369, 10));
			RegisterToNPC(491, ItemDropRule.Common(3359, 10));
			RegisterToNPC(551, ItemDropRule.Common(3866, 10));
			RegisterToNPC(564, ItemDropRule.Common(3867, 10));
			RegisterToNPC(565, ItemDropRule.Common(3867, 10));
			RegisterToNPC(576, ItemDropRule.Common(3868, 10));
			RegisterToNPC(577, ItemDropRule.Common(3868, 10));
		}

		private void RegisterMartianDrops()
		{
			RegisterToMultipleNPCs(ItemDropRule.Common(2860, 8, 8, 20), 520, 383, 389, 385, 382, 381, 390, 386);
			int[] npcNetIds = new int[8] { 520, 383, 389, 385, 382, 381, 390, 386 };
			RegisterToMultipleNPCs(ItemDropRule.Common(2798, 800), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2800, 800), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(2882, 800), npcNetIds);
			int[] npcNetIds2 = new int[3] { 383, 389, 386 };
			RegisterToMultipleNPCs(ItemDropRule.Common(2806, 200), npcNetIds2);
			RegisterToMultipleNPCs(ItemDropRule.Common(2807, 200), npcNetIds2);
			RegisterToMultipleNPCs(ItemDropRule.Common(2808, 200), npcNetIds2);
			int[] npcNetIds3 = new int[4] { 385, 382, 381, 390 };
			RegisterToMultipleNPCs(ItemDropRule.Common(2803, 200), npcNetIds3);
			RegisterToMultipleNPCs(ItemDropRule.Common(2804, 200), npcNetIds3);
			RegisterToMultipleNPCs(ItemDropRule.Common(2805, 200), npcNetIds3);
			RegisterToNPC(395, ItemDropRule.OneFromOptionsNotScalingWithLuck(1, 2797, 2749, 2795, 2796, 2880, 2769));
			RegisterToNPC(395, ItemDropRule.MasterModeCommonDrop(4939));
			RegisterToNPC(395, ItemDropRule.MasterModeDropOnAllPlayers(4815, _masterModeDropRng));
			RegisterToNPC(390, ItemDropRule.Common(2771, 30));
		}

		private void RegisterMiscDrops()
		{
			RegisterToNPC(68, ItemDropRule.Common(1169));
			RegisterToMultipleNPCs(ItemDropRule.Common(3086, 1, 5, 10), 483, 482);
			RegisterToNPC(77, ItemDropRule.Common(723, 150));
			RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(3102, 2, 1), 195, 196);
			RegisterToNPC(471, ItemDropRule.NormalvsExpertOneFromOptions(2, 1, 3052, 3053, 3054));
			RegisterToNPC(153, ItemDropRule.Common(1328, 12));
			RegisterToNPC(59, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(ItemDropRule.Common(23, 1, 1, 2));
			RegisterToNPC(59, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(ItemDropRule.NormalvsExpert(1309, 8000, 5600));
			RegisterToNPC(120, new LeadingConditionRule(new Conditions.TenthAnniversaryIsUp())).OnSuccess(ItemDropRule.Common(1326, 100));
			RegisterToNPC(120, new LeadingConditionRule(new Conditions.TenthAnniversaryIsNotUp())).OnSuccess(ItemDropRule.NormalvsExpert(1326, 500, 400));
			RegisterToNPC(49, new LeadingConditionRule(new Conditions.NotRemixSeed())).OnSuccess(ItemDropRule.Common(1325, 250));
			RegisterToNPC(49, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(ItemDropRule.Common(1314, 250));
			RegisterToNPC(109, new LeadingConditionRule(new Conditions.NotRemixSeed())).OnSuccess(ItemDropRule.Common(1314, 5));
			RegisterToNPC(109, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(ItemDropRule.Common(1325, 5));
			RegisterToNPC(156, new LeadingConditionRule(new Conditions.NotRemixSeed())).OnSuccess(ItemDropRule.Common(683, 30));
			RegisterToNPC(156, new LeadingConditionRule(new Conditions.RemixSeed())).OnSuccess(ItemDropRule.Common(112, 30));
			RegisterToNPC(634, ItemDropRule.Common(4764, 40));
			RegisterToNPC(185, ItemDropRule.Common(951, 150));
			RegisterToNPC(185, new DropBasedOnExpertMode(ItemDropRule.Common(5070, 1, 1, 2), new CommonDrop(5070, 1, 1, 3)));
			RegisterToNPC(44, ItemDropRule.Common(1320, 20));
			RegisterToNPC(44, ItemDropRule.Common(88, 20));
			RegisterToNPC(60, ItemDropRule.Common(1322, 150));
			RegisterToNPC(151, ItemDropRule.Common(1322, 50));
			RegisterToNPC(24, ItemDropRule.Common(1323, 20));
			RegisterToNPC(109, ItemDropRule.Common(1324, 10));
			RegisterToNPC(109, ItemDropRule.Common(4271, 10));
			int[] npcNetIds = new int[2] { 163, 238 };
			RegisterToMultipleNPCs(ItemDropRule.Common(1308, 40), npcNetIds);
			RegisterToMultipleNPCs(new DropBasedOnExpertMode(ItemDropRule.Common(2607, 2, 1, 3), new CommonDrop(2607, 10, 1, 3, 9)), npcNetIds);
			RegisterToMultipleNPCs(ItemDropRule.Common(1306, 100), 197, 206, 169, 154);
			RegisterToNPC(244, ItemDropRule.Common(23, 1, 1, 20));
			RegisterToNPC(244, ItemDropRule.Common(662, 1, 30, 60));
			RegisterToNPC(250, ItemDropRule.Common(1244, 15));
			RegisterToNPC(172, ItemDropRule.Common(754));
			RegisterToNPC(172, ItemDropRule.Common(755));
			RegisterToNPC(110, ItemDropRule.Common(682, 200));
			RegisterToNPC(110, ItemDropRule.Common(1321, 80));
			RegisterToMultipleNPCs(ItemDropRule.Common(4428, 100), 170, 180, 171);
			RegisterToMultipleNPCs(new ItemDropWithConditionRule(4613, 25, 1, 1, new Conditions.WindyEnoughForKiteDrops()), 170, 180, 171);
			RegisterToMultipleNPCs(new ItemDropWithConditionRule(5096, 10, 1, 1, new Conditions.DontStarveIsUp()), 170, 180, 171);
			RegisterToMultipleNPCs(new ItemDropWithConditionRule(5096, 25, 1, 1, new Conditions.DontStarveIsNotUp()), 170, 180, 171);
			RegisterToNPC(154, ItemDropRule.Common(1253, 50));
			RegisterToMultipleNPCs(ItemDropRule.Common(726, 50), 169, 206);
			RegisterToNPC(243, ItemDropRule.Common(2161));
			RegisterToNPC(155, ItemDropRule.NormalvsExpert(5130, 30, 25));
			RegisterToNPC(480, ItemDropRule.Common(3269, 25));
			RegisterToNPC(480, ItemDropRule.NormalvsExpert(3781, 40, 20));
			int[] npcNetIds2 = new int[3] { 198, 199, 226 };
			RegisterToMultipleNPCs(ItemDropRule.Common(1172, 1000), npcNetIds2);
			RegisterToMultipleNPCs(ItemDropRule.Common(1293, 50), npcNetIds2);
			RegisterToMultipleNPCs(ItemDropRule.Common(2766, 7, 1, 2), npcNetIds2);
			int[] npcNetIds3 = new int[4] { 78, 79, 80, 630 };
			RegisterToMultipleNPCs(ItemDropRule.Common(870, 75), npcNetIds3);
			RegisterToMultipleNPCs(ItemDropRule.Common(871, 75), npcNetIds3);
			RegisterToMultipleNPCs(ItemDropRule.Common(872, 75), npcNetIds3);
			RegisterToNPC(473, ItemDropRule.OneFromOptions(1, 3008, 3014, 3012, 3015, 3023));
			RegisterToNPC(474, ItemDropRule.OneFromOptions(1, 3006, 3007, 3013, 3016, 3020));
			RegisterToNPC(475, ItemDropRule.OneFromOptions(1, 3029, 3030, 3051, 3022));
			RegisterToNPC(476, ItemDropRule.Common(52, 3));
			RegisterToNPC(476, ItemDropRule.Common(1724, 3));
			RegisterToNPC(476, ItemDropRule.Common(2353, 3, 5, 10));
			RegisterToNPC(476, ItemDropRule.Common(1922, 3));
			RegisterToNPC(476, ItemDropRule.Common(678, 3, 3, 5));
			RegisterToNPC(476, ItemDropRule.Common(1336, 3));
			RegisterToNPC(476, ItemDropRule.Common(2676, 3, 2, 4));
			RegisterToNPC(476, ItemDropRule.Common(2272, 3));
			RegisterToNPC(476, ItemDropRule.Common(4731, 3));
			RegisterToNPC(476, ItemDropRule.Common(4986, 3, 69, 69));
			int[] npcNetIds4 = new int[3] { 473, 474, 475 };
			RegisterToMultipleNPCs(ItemDropRule.Common(499, 1, 5, 10), npcNetIds4);
			RegisterToMultipleNPCs(ItemDropRule.Common(500, 1, 5, 15), npcNetIds4);
			RegisterToNPC(87, new ItemDropWithConditionRule(4379, 25, 1, 1, new Conditions.WindyEnoughForKiteDrops()));
			RegisterToNPC(87, new DropBasedOnExpertMode(ItemDropRule.Common(575, 1, 5, 10), ItemDropRule.Common(575, 1, 10, 20)));
			RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(10, 803, 804, 805), 161, 431);
			RegisterToNPC(217, ItemDropRule.Common(1115));
			RegisterToNPC(218, ItemDropRule.Common(1116));
			RegisterToNPC(219, ItemDropRule.Common(1117));
			RegisterToNPC(220, ItemDropRule.Common(1118));
			RegisterToNPC(221, ItemDropRule.Common(1119));
			RegisterToNPC(167, ItemDropRule.Common(879, 50));
			RegisterToNPC(628, ItemDropRule.Common(313, 2, 1, 2));
			int[] npcNetIds5 = new int[3] { 143, 144, 145 };
			RegisterToMultipleNPCs(ItemDropRule.Common(593, 1, 5, 10), npcNetIds5);
			RegisterToMultipleNPCs(ItemDropRule.Common(527, 10), 79, 630);
			RegisterToNPC(80, ItemDropRule.Common(528, 10));
			RegisterToNPC(524, ItemDropRule.Common(3794, 10, 1, 3));
			RegisterToNPC(525, ItemDropRule.Common(3794, 10));
			RegisterToNPC(525, ItemDropRule.Common(522, 3, 1, 3));
			RegisterToNPC(525, ItemDropRule.Common(527, 15));
			RegisterToNPC(526, ItemDropRule.Common(3794, 10));
			RegisterToNPC(526, ItemDropRule.Common(1332, 3, 1, 3));
			RegisterToNPC(526, ItemDropRule.Common(527, 15));
			RegisterToNPC(527, ItemDropRule.Common(3794, 10));
			RegisterToNPC(527, ItemDropRule.Common(528, 15));
			RegisterToNPC(513, ItemDropRule.Common(3380, 2, 1, 2));
			RegisterToNPC(532, ItemDropRule.Common(3380, 1, 1, 3));
			RegisterToNPC(532, ItemDropRule.Common(3771, 50));
			RegisterToNPC(528, ItemDropRule.Common(2802, 25));
			RegisterToNPC(528, ItemDropRule.OneFromOptions(60, 3786, 3785, 3784));
			RegisterToNPC(529, ItemDropRule.Common(2801, 25));
			RegisterToNPC(529, ItemDropRule.OneFromOptions(40, 3786, 3785, 3784));
			RegisterToMultipleNPCs(ItemDropRule.Common(18, 100), 49, 51, 150, 93, 634);
			RegisterToMultipleNPCs(new ItemDropWithConditionRule(5097, 250, 1, 1, new Conditions.DontStarveIsNotUp()), 49, 51, 150, 93, 634, 151, 60, 137, 152);
			RegisterToMultipleNPCs(new ItemDropWithConditionRule(5097, 100, 1, 1, new Conditions.DontStarveIsUp()), 49, 51, 150, 93, 634, 151, 60, 137, 152);
			RegisterToMultipleNPCs(ItemDropRule.Common(393, 50), 16, 185, 167, 197);
			RegisterToNPC(58, ItemDropRule.Common(393, 75));
			int[] npcNetIds6 = new int[13]
			{
				494, 495, 496, 497, 498, 499, 500, 501, 502, 503,
				504, 505, 506
			};
			RegisterToMultipleNPCs(ItemDropRule.Common(18, 80), npcNetIds6).OnFailedRoll(ItemDropRule.Common(393, 80)).OnFailedRoll(ItemDropRule.Common(3285, 25));
			int[] npcNetIds7 = new int[12]
			{
				21, 201, 202, 203, 322, 323, 324, 635, 449, 450,
				451, 452
			};
			RegisterToMultipleNPCs(ItemDropRule.Common(954, 100), npcNetIds7).OnFailedRoll(ItemDropRule.Common(955, 200)).OnFailedRoll(ItemDropRule.Common(1166, 200)).OnFailedRoll(ItemDropRule.Common(1274, 500));
			RegisterToNPC(6, ItemDropRule.OneFromOptions(175, 956, 957, 958));
			int[] npcNetIds8 = new int[7] { 42, 43, 231, 232, 233, 234, 235 };
			RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(100, 960, 961, 962), npcNetIds8);
			int[] npcNetIds9 = new int[5] { 31, 32, 294, 295, 296 };
			RegisterToMultipleNPCs(ItemDropRule.Common(959, 450), npcNetIds9);
			RegisterToMultipleNPCs(ItemDropRule.Common(1307, 300), npcNetIds9);
			RegisterToMultipleNPCs(ItemDropRule.Common(996, 200), 174, 179, 182, 183, 98, 83, 94, 81, 101);
			RegisterToMultipleNPCs(ItemDropRule.Common(522, 1, 2, 5), 101, 98);
			RegisterToNPC(98, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4611, 25));
			RegisterToNPC(86, ItemDropRule.Common(526));
			RegisterToNPC(86, ItemDropRule.Common(856, 100));
			RegisterToNPC(86, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4684, 25));
			RegisterToNPC(224, ItemDropRule.Common(4057, 100));
			RegisterToMultipleNPCs(ItemDropRule.Common(40, 1, 1, 9), 186, 432);
			RegisterToNPC(225, ItemDropRule.Common(1243, 45)).OnFailedRoll(ItemDropRule.Common(23, 1, 2, 6));
			RegisterToNPC(537, ItemDropRule.Common(23, 1, 2, 3));
			RegisterToNPC(537, ItemDropRule.NormalvsExpert(1309, 8000, 5600));
			int[] npcNetIds10 = new int[4] { 335, 336, 333, 334 };
			RegisterToMultipleNPCs(ItemDropRule.Common(1906, 20), npcNetIds10);
			RegisterToNPC(-4, ItemDropRule.Common(3111, 1, 25, 50));
			RegisterToNPC(-4, ItemDropRule.NormalvsExpert(1309, 100, 70));
			int[] npcNetIds11 = new int[18]
			{
				1, 16, 138, 141, 147, 184, 187, 433, 204, 302,
				333, 334, 335, 336, 535, 658, 659, 660
			};
			int[] npcNetIds12 = new int[4] { -6, -7, -8, -9 };
			int[] npcNetIds13 = new int[5] { -6, -7, -8, -9, -4 };
			IItemDropRule entry = RegisterToMultipleNPCs(ItemDropRule.Common(23, 1, 1, 2), npcNetIds11);
			RemoveFromMultipleNPCs(entry, npcNetIds13);
			RegisterToMultipleNPCs(ItemDropRule.Common(23, 1, 2, 5), npcNetIds12);
			IItemDropRule entry2 = RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(1309, 10000, 7000), npcNetIds11);
			RemoveFromMultipleNPCs(entry2, npcNetIds13);
			RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(1309, 10000, 7000), npcNetIds12);
			RegisterToNPC(75, ItemDropRule.Common(501, 1, 1, 3));
			RegisterToMultipleNPCs(ItemDropRule.Common(23, 1, 2, 4), 81, 183);
			RegisterToNPC(122, ItemDropRule.Common(23, 1, 5, 10));
			RegisterToNPC(71, ItemDropRule.Common(327));
			int[] npcNetIds14 = new int[9] { 2, 317, 318, 190, 191, 192, 193, 194, 133 };
			RegisterToMultipleNPCs(ItemDropRule.Common(236, 100), npcNetIds14).OnFailedRoll(ItemDropRule.Common(38, 3));
			RegisterToNPC(133, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4683, 25));
			RegisterToNPC(104, ItemDropRule.Common(485, 60));
			RegisterToNPC(58, ItemDropRule.Common(263, 250)).OnFailedRoll(ItemDropRule.Common(118, 30));
			RegisterToNPC(102, ItemDropRule.Common(263, 250));
			int[] npcNetIds15 = new int[23]
			{
				3, 591, 590, 331, 332, 132, 161, 186, 187, 188,
				189, 200, 223, 319, 320, 321, 430, 431, 432, 433,
				434, 435, 436
			};
			RegisterToMultipleNPCs(ItemDropRule.Common(216, 50), npcNetIds15);
			RegisterToMultipleNPCs(ItemDropRule.Common(1304, 250), npcNetIds15);
			RegisterToMultipleNPCs(ItemDropRule.Common(5332, 1500), npcNetIds15);
			RegisterToMultipleNPCs(ItemDropRule.Common(8, 1, 5, 20), 590, 591);
			RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(3212, 150, 75), 489, 490);
			RegisterToMultipleNPCs(ItemDropRule.NormalvsExpert(3213, 200, 100), 489, 490);
			RegisterToNPC(223, ItemDropRule.OneFromOptions(20, 1135, 1136));
			RegisterToNPC(66, ItemDropRule.Common(267));
			RegisterToMultipleNPCs(ItemDropRule.Common(272, 35), 62, 66);
			RegisterToNPC(52, ItemDropRule.Common(251));
			RegisterToNPC(53, ItemDropRule.Common(239));
			RegisterToNPC(536, ItemDropRule.Common(3478));
			RegisterToNPC(536, ItemDropRule.Common(3479));
			RegisterToMultipleNPCs(ItemDropRule.Common(323, 3, 1, 2), 69, 581, 580, 508, 509);
			RegisterToNPC(582, ItemDropRule.Common(323, 6));
			RegisterToMultipleNPCs(ItemDropRule.Common(3772, 50), 581, 580, 508, 509);
			RegisterToNPC(73, ItemDropRule.Common(362, 1, 1, 2));
			int[] npcNetIds16 = new int[2] { 483, 482 };
			RegisterToMultipleNPCs(ItemDropRule.Common(3109, 30), npcNetIds16);
			RegisterToMultipleNPCs(ItemDropRule.Common(4400, 20), npcNetIds16);
			RegisterToMultipleNPCs(ItemDropRule.Common(68, 3), 6, 94);
			RegisterToMultipleNPCs(ItemDropRule.Common(1330, 3), 181, 173, 239, 182, 240);
			RegisterToMultipleNPCs(ItemDropRule.Common(68, 3, 1, 2), 7, 8, 9);
			RegisterToMultipleNPCs(ItemDropRule.Common(69, 1, 3, 8), 7, 8, 9);
			RegisterToMultipleNPCs(new ItemDropWithConditionRule(5094, 100, 1, 1, new Conditions.DontStarveIsUp()), 6, 7, 8, 9, 173, 181, 239, 240);
			RegisterToMultipleNPCs(new ItemDropWithConditionRule(5094, 525, 1, 1, new Conditions.DontStarveIsNotUp()), 6, 7, 8, 9, 173, 181, 239, 240);
			RegisterToMultipleNPCs(new ItemDropWithConditionRule(5091, 500, 1, 1, new Conditions.DontStarveIsUp()), 6, 7, 8, 9, 94, 81, 121, 101, 173, 181, 239, 240, 174, 183, 242, 241, 268, 182, 98, 99, 100);
			RegisterToMultipleNPCs(new ItemDropWithConditionRule(5091, 1500, 1, 1, new Conditions.DontStarveIsNotUp()), 6, 7, 8, 9, 94, 81, 121, 101, 173, 181, 239, 240, 174, 183, 242, 241, 268, 182, 98, 99, 100);
			RegisterToMultipleNPCs(new DropBasedOnExpertMode(ItemDropRule.Common(215, 50), ItemDropRule.WithRerolls(215, 1, 50)), 10, 11, 12, 95, 96, 97);
			RegisterToMultipleNPCs(ItemDropRule.Common(243, 75), 47, 464);
			RegisterToMultipleNPCs(ItemDropRule.OneFromOptions(50, 3757, 3758, 3759), 168, 470);
			RegisterToNPC(533, ItemDropRule.Common(3795, 40)).OnFailedRoll(ItemDropRule.Common(3770, 30));
			int[] npcNetIds17 = new int[3] { 63, 103, 64 };
			RegisterToMultipleNPCs(ItemDropRule.Common(1303, 100), npcNetIds17);
			RegisterToMultipleNPCs(ItemDropRule.Common(282, 1, 1, 4), npcNetIds17);
			RegisterToMultipleNPCs(ItemDropRule.Common(282, 1, 1, 4), 223);
			RegisterToMultipleNPCs(ItemDropRule.Common(282, 1, 1, 4), 224);
			RegisterToNPC(63, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4649, 50));
			RegisterToNPC(64, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4650, 50));
			RegisterToNPC(481, ItemDropRule.Common(3094, 2, 40, 80));
			RegisterToNPC(481, ItemDropRule.OneFromOptions(7, 3187, 3188, 3189));
			RegisterToNPC(481, ItemDropRule.Common(4463, 20));
			int[] npcNetIds18 = new int[13]
			{
				21, 167, 201, 202, 481, 203, 322, 323, 324, 449,
				450, 451, 452
			};
			RegisterToMultipleNPCs(ItemDropRule.Common(118, 25), npcNetIds18);
			RegisterToNPC(44, ItemDropRule.Common(118, 25)).OnFailedRoll(ItemDropRule.OneFromOptions(4, 410, 411)).OnFailedRoll(ItemDropRule.Common(166, 1, 1, 3));
			RegisterToNPC(45, ItemDropRule.Common(238));
			RegisterToNPC(23, ItemDropRule.Common(116, 50));
			RegisterToNPC(24, ItemDropRule.Common(244, 250));
			int[] npcNetIds19 = new int[6] { 31, 32, 34, 294, 295, 296 };
			RegisterToMultipleNPCs(ItemDropRule.Common(932, 250), npcNetIds19).OnFailedRoll(ItemDropRule.Common(3095, 100)).OnFailedRoll(ItemDropRule.Common(327, 65)).OnFailedRoll(ItemDropRule.ByCondition(new Conditions.NotExpert(), 154, 1, 1, 3));
			RegisterToMultipleNPCs(ItemDropRule.ByCondition(new Conditions.IsExpert(), 154, 1, 2, 6), npcNetIds19);
			int[] npcNetIds20 = new int[5] { 26, 27, 28, 29, 111 };
			RegisterToMultipleNPCs(ItemDropRule.Common(160, 200), npcNetIds20).OnFailedRoll(ItemDropRule.Common(161, 2, 1, 5));
			RegisterToNPC(175, ItemDropRule.Common(1265, 100));
			RegisterToNPC(175, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4675, 25));
			RegisterToMultipleNPCs(new DropBasedOnExpertMode(new CommonDrop(209, 3, 1, 1, 2), ItemDropRule.Common(209)), 42, 231, 232, 233, 234, 235);
			RegisterToNPC(176, ItemDropRule.Common(209, 6));
			RegisterToNPC(177, new ItemDropWithConditionRule(5089, 100, 1, 1, new Conditions.DontStarveIsNotUp()));
			RegisterToNPC(177, new ItemDropWithConditionRule(5089, 40, 1, 1, new Conditions.DontStarveIsUp()));
			RegisterToNPC(204, ItemDropRule.NormalvsExpert(209, 2, 1));
			RegisterToNPC(43, ItemDropRule.NormalvsExpert(210, 2, 1));
			RegisterToNPC(43, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4648, 25));
			RegisterToNPC(39, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4610, 15));
			RegisterToNPC(65, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4651, 25));
			RegisterToNPC(65, ItemDropRule.Common(268, 20)).OnFailedRoll(ItemDropRule.Common(319));
			RegisterToNPC(48, ItemDropRule.NotScalingWithLuck(320, 2));
			RegisterToNPC(541, ItemDropRule.Common(3783));
			RegisterToMultipleNPCs(ItemDropRule.Common(319, 8), 542, 543, 544, 545);
			RegisterToMultipleNPCs(ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4669, 25), 542, 543, 544, 545);
			RegisterToNPC(543, ItemDropRule.Common(527, 25));
			RegisterToNPC(544, ItemDropRule.Common(527, 25));
			RegisterToNPC(545, ItemDropRule.Common(528, 25));
			RegisterToNPC(47, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4670, 25));
			RegisterToNPC(464, ItemDropRule.ByCondition(new Conditions.WindyEnoughForKiteDrops(), 4671, 25));
			RegisterToNPC(268, ItemDropRule.Common(1332, 1, 2, 5));
			RegisterToNPC(631, ItemDropRule.Common(3, 1, 10, 20));
			RegisterToNPC(631, ItemDropRule.Common(4761, 10));
			int[] npcNetIds21 = new int[1] { 594 };
			LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.NeverTrue());
			int[] options = new int[0];
			IItemDropRule rule = leadingConditionRule.OnSuccess(ItemDropRule.OneFromOptions(8, options));
			int chanceDenominator = 9;
			rule.OnSuccess(new CommonDrop(4367, chanceDenominator));
			rule.OnSuccess(new CommonDrop(4368, chanceDenominator));
			rule.OnSuccess(new CommonDrop(4369, chanceDenominator));
			rule.OnSuccess(new CommonDrop(4370, chanceDenominator));
			rule.OnSuccess(new CommonDrop(4371, chanceDenominator));
			rule.OnSuccess(new CommonDrop(4612, chanceDenominator));
			rule.OnSuccess(new CommonDrop(4674, chanceDenominator));
			rule.OnSuccess(new CommonDrop(4343, chanceDenominator, 2, 5));
			rule.OnSuccess(new CommonDrop(4344, chanceDenominator, 2, 5));
			RegisterToMultipleNPCs(leadingConditionRule, npcNetIds21);
		}
	}
}
