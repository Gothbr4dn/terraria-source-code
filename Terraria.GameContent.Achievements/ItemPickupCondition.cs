using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	public class ItemPickupCondition : AchievementCondition
	{
		private const string Identifier = "ITEM_PICKUP";

		private static Dictionary<short, List<ItemPickupCondition>> _listeners = new Dictionary<short, List<ItemPickupCondition>>();

		private static bool _isListenerHooked;

		private short[] _itemIds;

		private ItemPickupCondition(short itemId)
			: base("ITEM_PICKUP_" + itemId)
		{
			_itemIds = new short[1] { itemId };
			ListenForPickup(this);
		}

		private ItemPickupCondition(short[] itemIds)
			: base("ITEM_PICKUP_" + itemIds[0])
		{
			_itemIds = itemIds;
			ListenForPickup(this);
		}

		private static void ListenForPickup(ItemPickupCondition condition)
		{
			if (!_isListenerHooked)
			{
				AchievementsHelper.OnItemPickup += ItemPickupListener;
				_isListenerHooked = true;
			}
			for (int i = 0; i < condition._itemIds.Length; i++)
			{
				if (!_listeners.ContainsKey(condition._itemIds[i]))
				{
					_listeners[condition._itemIds[i]] = new List<ItemPickupCondition>();
				}
				_listeners[condition._itemIds[i]].Add(condition);
			}
		}

		private static void ItemPickupListener(Player player, short itemId, int count)
		{
			if (player.whoAmI != Main.myPlayer || !_listeners.ContainsKey(itemId))
			{
				return;
			}
			foreach (ItemPickupCondition item in _listeners[itemId])
			{
				item.Complete();
			}
		}

		public static AchievementCondition Create(params short[] items)
		{
			return new ItemPickupCondition(items);
		}

		public static AchievementCondition Create(short item)
		{
			return new ItemPickupCondition(item);
		}

		public static AchievementCondition[] CreateMany(params short[] items)
		{
			AchievementCondition[] array = new AchievementCondition[items.Length];
			for (int i = 0; i < items.Length; i++)
			{
				array[i] = new ItemPickupCondition(items[i]);
			}
			return array;
		}
	}
}
