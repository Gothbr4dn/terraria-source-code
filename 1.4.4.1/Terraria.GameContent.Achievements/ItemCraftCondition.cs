using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	public class ItemCraftCondition : AchievementCondition
	{
		private const string Identifier = "ITEM_PICKUP";

		private static Dictionary<short, List<ItemCraftCondition>> _listeners = new Dictionary<short, List<ItemCraftCondition>>();

		private static bool _isListenerHooked;

		private short[] _itemIds;

		private ItemCraftCondition(short itemId)
			: base("ITEM_PICKUP_" + itemId)
		{
			_itemIds = new short[1] { itemId };
			ListenForCraft(this);
		}

		private ItemCraftCondition(short[] itemIds)
			: base("ITEM_PICKUP_" + itemIds[0])
		{
			_itemIds = itemIds;
			ListenForCraft(this);
		}

		private static void ListenForCraft(ItemCraftCondition condition)
		{
			if (!_isListenerHooked)
			{
				AchievementsHelper.OnItemCraft += ItemCraftListener;
				_isListenerHooked = true;
			}
			for (int i = 0; i < condition._itemIds.Length; i++)
			{
				if (!_listeners.ContainsKey(condition._itemIds[i]))
				{
					_listeners[condition._itemIds[i]] = new List<ItemCraftCondition>();
				}
				_listeners[condition._itemIds[i]].Add(condition);
			}
		}

		private static void ItemCraftListener(short itemId, int count)
		{
			if (!_listeners.ContainsKey(itemId))
			{
				return;
			}
			foreach (ItemCraftCondition item in _listeners[itemId])
			{
				item.Complete();
			}
		}

		public static AchievementCondition Create(params short[] items)
		{
			return new ItemCraftCondition(items);
		}

		public static AchievementCondition Create(short item)
		{
			return new ItemCraftCondition(item);
		}

		public static AchievementCondition[] CreateMany(params short[] items)
		{
			AchievementCondition[] array = new AchievementCondition[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				array[i] = new ItemCraftCondition(items[i]);
			}
			return array;
		}
	}
}
