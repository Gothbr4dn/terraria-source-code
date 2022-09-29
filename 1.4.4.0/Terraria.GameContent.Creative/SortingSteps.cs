using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Creative
{
	public static class SortingSteps
	{
		public abstract class ACreativeItemSortStep : ICreativeItemSortStep, IEntrySortStep<int>, IComparer<int>, IComparer<Item>
		{
			public abstract string GetDisplayNameKey();

			public int Compare(int x, int y)
			{
				return Compare(ContentSamples.ItemsByType[x], ContentSamples.ItemsByType[y]);
			}

			public abstract int Compare(Item x, Item y);
		}

		public abstract class AStepByFittingFilter : ACreativeItemSortStep
		{
			public override int Compare(Item x, Item y)
			{
				int num = FitsFilter(x).CompareTo(FitsFilter(y));
				if (num == 0)
				{
					num = 1;
				}
				return num;
			}

			public abstract bool FitsFilter(Item item);

			public virtual int CompareWhenBothFit(Item x, Item y)
			{
				return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
			}
		}

		public class Blocks : AStepByFittingFilter
		{
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_Blocks";
			}

			public override bool FitsFilter(Item item)
			{
				if (item.createTile >= 0)
				{
					return !Main.tileFrameImportant[item.createTile];
				}
				return false;
			}
		}

		public class Walls : AStepByFittingFilter
		{
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_Walls";
			}

			public override bool FitsFilter(Item item)
			{
				return item.createWall >= 0;
			}
		}

		public class PlacableObjects : AStepByFittingFilter
		{
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_PlacableObjects";
			}

			public override bool FitsFilter(Item item)
			{
				if (item.createTile >= 0)
				{
					return Main.tileFrameImportant[item.createTile];
				}
				return false;
			}
		}

		public class ByCreativeSortingId : ACreativeItemSortStep
		{
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_SortingID";
			}

			public override int Compare(Item x, Item y)
			{
				ContentSamples.CreativeHelper.ItemGroupAndOrderInGroup itemGroupAndOrderInGroup = ContentSamples.ItemCreativeSortingId[x.type];
				ContentSamples.CreativeHelper.ItemGroupAndOrderInGroup itemGroupAndOrderInGroup2 = ContentSamples.ItemCreativeSortingId[y.type];
				int num = itemGroupAndOrderInGroup.Group.CompareTo(itemGroupAndOrderInGroup2.Group);
				if (num == 0)
				{
					num = itemGroupAndOrderInGroup.OrderInGroup.CompareTo(itemGroupAndOrderInGroup2.OrderInGroup);
				}
				return num;
			}
		}

		public class Alphabetical : ACreativeItemSortStep
		{
			public override string GetDisplayNameKey()
			{
				return "CreativePowers.Sort_Alphabetical";
			}

			public override int Compare(Item x, Item y)
			{
				string name = x.Name;
				string name2 = y.Name;
				return name.CompareTo(name2);
			}
		}
	}
}
