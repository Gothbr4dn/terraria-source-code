using System.Collections.Generic;

namespace Terraria.GameContent
{
	public class ItemTrader
	{
		public class TradeOption
		{
			public int TakingItemType;

			public int TakingItemStack;

			public int GivingITemType;

			public int GivingItemStack;

			public bool WillTradeFor(int offeredItemType, int offeredItemStack)
			{
				if (offeredItemType != TakingItemType || offeredItemStack < TakingItemStack)
				{
					return false;
				}
				return true;
			}
		}

		public static ItemTrader ChlorophyteExtractinator = CreateChlorophyteExtractinator();

		private List<TradeOption> _options = new List<TradeOption>();

		public void AddOption_Interchangable(int itemType1, int itemType2)
		{
			AddOption_OneWay(itemType1, 1, itemType2, 1);
			AddOption_OneWay(itemType2, 1, itemType1, 1);
		}

		public void AddOption_CyclicLoop(params int[] typesInOrder)
		{
			for (int i = 0; i < typesInOrder.Length - 1; i++)
			{
				AddOption_OneWay(typesInOrder[i], 1, typesInOrder[i + 1], 1);
			}
			AddOption_OneWay(typesInOrder[^1], 1, typesInOrder[0], 1);
		}

		public void AddOption_FromAny(int givingItemType, params int[] takingItemTypes)
		{
			for (int i = 0; i < takingItemTypes.Length; i++)
			{
				AddOption_OneWay(takingItemTypes[i], 1, givingItemType, 1);
			}
		}

		public void AddOption_OneWay(int takingItemType, int takingItemStack, int givingItemType, int givingItemStack)
		{
			_options.Add(new TradeOption
			{
				TakingItemType = takingItemType,
				TakingItemStack = takingItemStack,
				GivingITemType = givingItemType,
				GivingItemStack = givingItemStack
			});
		}

		public bool TryGetTradeOption(Item item, out TradeOption option)
		{
			option = null;
			int type = item.type;
			int stack = item.stack;
			for (int i = 0; i < _options.Count; i++)
			{
				TradeOption tradeOption = _options[i];
				if (tradeOption.WillTradeFor(type, stack))
				{
					option = tradeOption;
					return true;
				}
			}
			return false;
		}

		public static ItemTrader CreateChlorophyteExtractinator()
		{
			ItemTrader itemTrader = new ItemTrader();
			itemTrader.AddOption_Interchangable(12, 699);
			itemTrader.AddOption_Interchangable(11, 700);
			itemTrader.AddOption_Interchangable(14, 701);
			itemTrader.AddOption_Interchangable(13, 702);
			itemTrader.AddOption_Interchangable(56, 880);
			itemTrader.AddOption_Interchangable(364, 1104);
			itemTrader.AddOption_Interchangable(365, 1105);
			itemTrader.AddOption_Interchangable(366, 1106);
			itemTrader.AddOption_CyclicLoop(134, 137, 139);
			itemTrader.AddOption_Interchangable(20, 703);
			itemTrader.AddOption_Interchangable(22, 704);
			itemTrader.AddOption_Interchangable(21, 705);
			itemTrader.AddOption_Interchangable(19, 706);
			itemTrader.AddOption_Interchangable(57, 1257);
			itemTrader.AddOption_Interchangable(381, 1184);
			itemTrader.AddOption_Interchangable(382, 1191);
			itemTrader.AddOption_Interchangable(391, 1198);
			itemTrader.AddOption_Interchangable(86, 1329);
			itemTrader.AddOption_FromAny(3, 61, 836, 409);
			itemTrader.AddOption_FromAny(169, 370, 1246, 408);
			itemTrader.AddOption_FromAny(664, 833, 835, 834);
			itemTrader.AddOption_FromAny(3271, 3276, 3277, 3339);
			itemTrader.AddOption_FromAny(3272, 3274, 3275, 3338);
			return itemTrader;
		}
	}
}
