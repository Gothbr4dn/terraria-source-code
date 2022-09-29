using System.Collections.Generic;

namespace Terraria.GameContent
{
	public class ItemShopSellbackHelper
	{
		private class ItemMemo
		{
			public readonly int itemNetID;

			public readonly int itemPrefix;

			public int stack;

			public ItemMemo(Item item)
			{
				itemNetID = item.netID;
				itemPrefix = item.prefix;
				stack = item.stack;
			}

			public bool Matches(Item item)
			{
				if (item.netID == itemNetID)
				{
					return item.prefix == itemPrefix;
				}
				return false;
			}
		}

		private List<ItemMemo> _memos = new List<ItemMemo>();

		public void Add(Item item)
		{
			ItemMemo itemMemo = _memos.Find((ItemMemo x) => x.Matches(item));
			if (itemMemo != null)
			{
				itemMemo.stack += item.stack;
			}
			else
			{
				_memos.Add(new ItemMemo(item));
			}
		}

		public void Clear()
		{
			_memos.Clear();
		}

		public int GetAmount(Item item)
		{
			return _memos.Find((ItemMemo x) => x.Matches(item))?.stack ?? 0;
		}

		public int Remove(Item item)
		{
			ItemMemo itemMemo = _memos.Find((ItemMemo x) => x.Matches(item));
			if (itemMemo == null)
			{
				return 0;
			}
			int stack = itemMemo.stack;
			itemMemo.stack -= item.stack;
			if (itemMemo.stack <= 0)
			{
				_memos.Remove(itemMemo);
				return stack;
			}
			return stack - itemMemo.stack;
		}
	}
}
