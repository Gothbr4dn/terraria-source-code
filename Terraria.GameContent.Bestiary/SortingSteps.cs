using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.Bestiary
{
	public static class SortingSteps
	{
		public class ByNetId : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			public bool HiddenFromSortOptions => true;

			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				NPCNetIdBestiaryInfoElement nPCNetIdBestiaryInfoElement = x.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				NPCNetIdBestiaryInfoElement nPCNetIdBestiaryInfoElement2 = y.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				if (nPCNetIdBestiaryInfoElement == null && nPCNetIdBestiaryInfoElement2 != null)
				{
					return 1;
				}
				if (nPCNetIdBestiaryInfoElement2 == null && nPCNetIdBestiaryInfoElement != null)
				{
					return -1;
				}
				if (nPCNetIdBestiaryInfoElement == null || nPCNetIdBestiaryInfoElement2 == null)
				{
					return 0;
				}
				return nPCNetIdBestiaryInfoElement.NetId.CompareTo(nPCNetIdBestiaryInfoElement2.NetId);
			}

			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_ID";
			}
		}

		public class ByUnlockState : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			public bool HiddenFromSortOptions => true;

			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				BestiaryUICollectionInfo entryUICollectionInfo = x.UIInfoProvider.GetEntryUICollectionInfo();
				BestiaryUICollectionInfo entryUICollectionInfo2 = y.UIInfoProvider.GetEntryUICollectionInfo();
				return y.Icon.GetUnlockState(entryUICollectionInfo2).CompareTo(x.Icon.GetUnlockState(entryUICollectionInfo));
			}

			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Unlocks";
			}
		}

		public class ByBestiarySortingId : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			public bool HiddenFromSortOptions => false;

			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				NPCNetIdBestiaryInfoElement nPCNetIdBestiaryInfoElement = x.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				NPCNetIdBestiaryInfoElement nPCNetIdBestiaryInfoElement2 = y.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				if (nPCNetIdBestiaryInfoElement == null && nPCNetIdBestiaryInfoElement2 != null)
				{
					return 1;
				}
				if (nPCNetIdBestiaryInfoElement2 == null && nPCNetIdBestiaryInfoElement != null)
				{
					return -1;
				}
				if (nPCNetIdBestiaryInfoElement == null || nPCNetIdBestiaryInfoElement2 == null)
				{
					return 0;
				}
				int num = ContentSamples.NpcBestiarySortingId[nPCNetIdBestiaryInfoElement.NetId];
				int value = ContentSamples.NpcBestiarySortingId[nPCNetIdBestiaryInfoElement2.NetId];
				return num.CompareTo(value);
			}

			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_BestiaryID";
			}
		}

		public class ByBestiaryRarity : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			public bool HiddenFromSortOptions => false;

			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				NPCNetIdBestiaryInfoElement nPCNetIdBestiaryInfoElement = x.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				NPCNetIdBestiaryInfoElement nPCNetIdBestiaryInfoElement2 = y.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				if (nPCNetIdBestiaryInfoElement == null && nPCNetIdBestiaryInfoElement2 != null)
				{
					return 1;
				}
				if (nPCNetIdBestiaryInfoElement2 == null && nPCNetIdBestiaryInfoElement != null)
				{
					return -1;
				}
				if (nPCNetIdBestiaryInfoElement == null || nPCNetIdBestiaryInfoElement2 == null)
				{
					return 0;
				}
				int value = ContentSamples.NpcBestiaryRarityStars[nPCNetIdBestiaryInfoElement.NetId];
				return ContentSamples.NpcBestiaryRarityStars[nPCNetIdBestiaryInfoElement2.NetId].CompareTo(value);
			}

			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Rarity";
			}
		}

		public class Alphabetical : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			public bool HiddenFromSortOptions => false;

			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				NPCNetIdBestiaryInfoElement nPCNetIdBestiaryInfoElement = x.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				NPCNetIdBestiaryInfoElement nPCNetIdBestiaryInfoElement2 = y.Info.FirstOrDefault((IBestiaryInfoElement element) => element is NPCNetIdBestiaryInfoElement) as NPCNetIdBestiaryInfoElement;
				if (nPCNetIdBestiaryInfoElement == null && nPCNetIdBestiaryInfoElement2 != null)
				{
					return 1;
				}
				if (nPCNetIdBestiaryInfoElement2 == null && nPCNetIdBestiaryInfoElement != null)
				{
					return -1;
				}
				if (nPCNetIdBestiaryInfoElement == null || nPCNetIdBestiaryInfoElement2 == null)
				{
					return 0;
				}
				string textValue = Language.GetTextValue(ContentSamples.NpcsByNetId[nPCNetIdBestiaryInfoElement.NetId].TypeName);
				string textValue2 = Language.GetTextValue(ContentSamples.NpcsByNetId[nPCNetIdBestiaryInfoElement2.NetId].TypeName);
				return textValue.CompareTo(textValue2);
			}

			public string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Alphabetical";
			}
		}

		public abstract class ByStat : IBestiarySortStep, IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
		{
			public bool HiddenFromSortOptions => false;

			public int Compare(BestiaryEntry x, BestiaryEntry y)
			{
				NPCStatsReportInfoElement nPCStatsReportInfoElement = x.Info.FirstOrDefault((IBestiaryInfoElement element) => IsAStatsCardINeed(element)) as NPCStatsReportInfoElement;
				NPCStatsReportInfoElement nPCStatsReportInfoElement2 = y.Info.FirstOrDefault((IBestiaryInfoElement element) => IsAStatsCardINeed(element)) as NPCStatsReportInfoElement;
				if (nPCStatsReportInfoElement == null && nPCStatsReportInfoElement2 != null)
				{
					return 1;
				}
				if (nPCStatsReportInfoElement2 == null && nPCStatsReportInfoElement != null)
				{
					return -1;
				}
				if (nPCStatsReportInfoElement == null || nPCStatsReportInfoElement2 == null)
				{
					return 0;
				}
				return Compare(nPCStatsReportInfoElement, nPCStatsReportInfoElement2);
			}

			public abstract int Compare(NPCStatsReportInfoElement cardX, NPCStatsReportInfoElement cardY);

			public abstract string GetDisplayNameKey();

			private bool IsAStatsCardINeed(IBestiaryInfoElement element)
			{
				if (!(element is NPCStatsReportInfoElement))
				{
					return false;
				}
				return true;
			}
		}

		public class ByAttack : ByStat
		{
			public override int Compare(NPCStatsReportInfoElement cardX, NPCStatsReportInfoElement cardY)
			{
				return cardY.Damage.CompareTo(cardX.Damage);
			}

			public override string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Attack";
			}
		}

		public class ByDefense : ByStat
		{
			public override int Compare(NPCStatsReportInfoElement cardX, NPCStatsReportInfoElement cardY)
			{
				return cardY.Defense.CompareTo(cardX.Defense);
			}

			public override string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Defense";
			}
		}

		public class ByCoins : ByStat
		{
			public override int Compare(NPCStatsReportInfoElement cardX, NPCStatsReportInfoElement cardY)
			{
				return cardY.MonetaryValue.CompareTo(cardX.MonetaryValue);
			}

			public override string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_Coins";
			}
		}

		public class ByHP : ByStat
		{
			public override int Compare(NPCStatsReportInfoElement cardX, NPCStatsReportInfoElement cardY)
			{
				return cardY.LifeMax.CompareTo(cardX.LifeMax);
			}

			public override string GetDisplayNameKey()
			{
				return "BestiaryInfo.Sort_HitPoints";
			}
		}
	}
}
