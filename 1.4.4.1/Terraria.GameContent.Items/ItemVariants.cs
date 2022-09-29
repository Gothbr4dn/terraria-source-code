using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;

namespace Terraria.GameContent.Items
{
	public static class ItemVariants
	{
		public class VariantEntry
		{
			public readonly ItemVariant Variant;

			private readonly List<ItemVariantCondition> _conditions = new List<ItemVariantCondition>();

			public IEnumerable<ItemVariantCondition> Conditions => _conditions;

			public VariantEntry(ItemVariant variant)
			{
				Variant = variant;
			}

			internal void AddConditions(IEnumerable<ItemVariantCondition> conditions)
			{
				_conditions.AddRange(conditions);
			}

			public bool AnyConditionMet()
			{
				return Conditions.Any((ItemVariantCondition c) => c.IsMet());
			}
		}

		private static List<VariantEntry>[] _variants;

		public static ItemVariant StrongerVariant;

		public static ItemVariant WeakerVariant;

		public static ItemVariant RebalancedVariant;

		public static ItemVariant EnabledVariant;

		public static ItemVariant DisabledBossSummonVariant;

		public static ItemVariantCondition RemixWorld;

		public static ItemVariantCondition GetGoodWorld;

		public static ItemVariantCondition EverythingWorld;

		public static IEnumerable<VariantEntry> GetVariants(int itemId)
		{
			if (!_variants.IndexInRange(itemId))
			{
				return Enumerable.Empty<VariantEntry>();
			}
			IEnumerable<VariantEntry> enumerable = _variants[itemId];
			return enumerable ?? Enumerable.Empty<VariantEntry>();
		}

		private static VariantEntry GetEntry(int itemId, ItemVariant variant)
		{
			return GetVariants(itemId).SingleOrDefault((VariantEntry v) => v.Variant == variant);
		}

		public static void AddVariant(int itemId, ItemVariant variant, params ItemVariantCondition[] conditions)
		{
			VariantEntry variantEntry = GetEntry(itemId, variant);
			if (variantEntry == null)
			{
				List<VariantEntry> list = _variants[itemId];
				if (list == null)
				{
					list = (_variants[itemId] = new List<VariantEntry>());
				}
				list.Add(variantEntry = new VariantEntry(variant));
			}
			variantEntry.AddConditions(conditions);
		}

		public static bool HasVariant(int itemId, ItemVariant variant)
		{
			return GetEntry(itemId, variant) != null;
		}

		public static ItemVariant SelectVariant(int itemId)
		{
			if (!_variants.IndexInRange(itemId))
			{
				return null;
			}
			List<VariantEntry> list = _variants[itemId];
			if (list == null)
			{
				return null;
			}
			foreach (VariantEntry item in list)
			{
				if (item.AnyConditionMet())
				{
					return item.Variant;
				}
			}
			return null;
		}

		static ItemVariants()
		{
			_variants = new List<VariantEntry>[5453];
			StrongerVariant = new ItemVariant(NetworkText.FromKey("ItemVariant.Stronger"));
			WeakerVariant = new ItemVariant(NetworkText.FromKey("ItemVariant.Weaker"));
			RebalancedVariant = new ItemVariant(NetworkText.FromKey("ItemVariant.Rebalanced"));
			EnabledVariant = new ItemVariant(NetworkText.FromKey("ItemVariant.Enabled"));
			DisabledBossSummonVariant = new ItemVariant(NetworkText.FromKey("ItemVariant.DisabledBossSummon"));
			RemixWorld = new ItemVariantCondition(NetworkText.FromKey("ItemVariantCondition.RemixWorld"), () => Main.remixWorld);
			GetGoodWorld = new ItemVariantCondition(NetworkText.FromKey("ItemVariantCondition.GetGoodWorld"), () => Main.getGoodWorld);
			EverythingWorld = new ItemVariantCondition(NetworkText.FromKey("ItemVariantCondition.EverythingWorld"), () => Main.getGoodWorld && Main.remixWorld);
			AddVariant(112, StrongerVariant, RemixWorld);
			AddVariant(157, StrongerVariant, RemixWorld);
			AddVariant(1319, StrongerVariant, RemixWorld);
			AddVariant(1325, StrongerVariant, RemixWorld);
			AddVariant(2273, StrongerVariant, RemixWorld);
			AddVariant(3069, StrongerVariant, RemixWorld);
			AddVariant(5147, StrongerVariant, RemixWorld);
			AddVariant(517, WeakerVariant, RemixWorld);
			AddVariant(671, WeakerVariant, RemixWorld);
			AddVariant(683, WeakerVariant, RemixWorld);
			AddVariant(725, WeakerVariant, RemixWorld);
			AddVariant(1314, WeakerVariant, RemixWorld);
			AddVariant(2623, WeakerVariant, RemixWorld);
			AddVariant(5279, WeakerVariant, RemixWorld);
			AddVariant(5280, WeakerVariant, RemixWorld);
			AddVariant(5281, WeakerVariant, RemixWorld);
			AddVariant(5282, WeakerVariant, RemixWorld);
			AddVariant(5283, WeakerVariant, RemixWorld);
			AddVariant(5284, WeakerVariant, RemixWorld);
			AddVariant(197, RebalancedVariant, GetGoodWorld);
			AddVariant(4060, RebalancedVariant, GetGoodWorld);
			AddVariant(556, DisabledBossSummonVariant, EverythingWorld);
			AddVariant(557, DisabledBossSummonVariant, EverythingWorld);
			AddVariant(544, DisabledBossSummonVariant, EverythingWorld);
			AddVariant(5334, EnabledVariant, EverythingWorld);
		}
	}
}
