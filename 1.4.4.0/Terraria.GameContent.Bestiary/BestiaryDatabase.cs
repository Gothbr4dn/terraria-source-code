using System.Collections.Generic;
using Terraria.GameContent.ItemDropRules;

namespace Terraria.GameContent.Bestiary
{
	public class BestiaryDatabase
	{
		public delegate void BestiaryEntriesPass(BestiaryEntry entry);

		private List<BestiaryEntry> _entries = new List<BestiaryEntry>();

		private List<IBestiaryEntryFilter> _filters = new List<IBestiaryEntryFilter>();

		private List<IBestiarySortStep> _sortSteps = new List<IBestiarySortStep>();

		private Dictionary<int, BestiaryEntry> _byNpcId = new Dictionary<int, BestiaryEntry>();

		private BestiaryEntry _trashEntry = new BestiaryEntry();

		public List<BestiaryEntry> Entries => _entries;

		public List<IBestiaryEntryFilter> Filters => _filters;

		public List<IBestiarySortStep> SortSteps => _sortSteps;

		public BestiaryEntry Register(BestiaryEntry entry)
		{
			_entries.Add(entry);
			for (int i = 0; i < entry.Info.Count; i++)
			{
				if (entry.Info[i] is NPCNetIdBestiaryInfoElement nPCNetIdBestiaryInfoElement)
				{
					_byNpcId[nPCNetIdBestiaryInfoElement.NetId] = entry;
				}
			}
			return entry;
		}

		public IBestiaryEntryFilter Register(IBestiaryEntryFilter filter)
		{
			_filters.Add(filter);
			return filter;
		}

		public IBestiarySortStep Register(IBestiarySortStep sortStep)
		{
			_sortSteps.Add(sortStep);
			return sortStep;
		}

		public BestiaryEntry FindEntryByNPCID(int npcNetId)
		{
			if (_byNpcId.TryGetValue(npcNetId, out var value))
			{
				return value;
			}
			_trashEntry.Info.Clear();
			return _trashEntry;
		}

		public void Merge(ItemDropDatabase dropsDatabase)
		{
			for (int i = -65; i < 688; i++)
			{
				ExtractDropsForNPC(dropsDatabase, i);
			}
		}

		private void ExtractDropsForNPC(ItemDropDatabase dropsDatabase, int npcId)
		{
			BestiaryEntry bestiaryEntry = FindEntryByNPCID(npcId);
			if (bestiaryEntry == null)
			{
				return;
			}
			List<IItemDropRule> rulesForNPCID = dropsDatabase.GetRulesForNPCID(npcId, includeGlobalDrops: false);
			List<DropRateInfo> list = new List<DropRateInfo>();
			DropRateInfoChainFeed ratesInfo = new DropRateInfoChainFeed(1f);
			foreach (IItemDropRule item in rulesForNPCID)
			{
				item.ReportDroprates(list, ratesInfo);
			}
			foreach (DropRateInfo item2 in list)
			{
				bestiaryEntry.Info.Add(new ItemDropBestiaryInfoElement(item2));
			}
		}

		public void ApplyPass(BestiaryEntriesPass pass)
		{
			for (int i = 0; i < _entries.Count; i++)
			{
				pass(_entries[i]);
			}
		}
	}
}
