using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	public class ProgressionEventCondition : AchievementCondition
	{
		private const string Identifier = "PROGRESSION_EVENT";

		private static Dictionary<int, List<ProgressionEventCondition>> _listeners = new Dictionary<int, List<ProgressionEventCondition>>();

		private static bool _isListenerHooked;

		private int[] _eventIDs;

		private ProgressionEventCondition(int eventID)
			: base("PROGRESSION_EVENT_" + eventID)
		{
			_eventIDs = new int[1] { eventID };
			ListenForPickup(this);
		}

		private ProgressionEventCondition(int[] eventIDs)
			: base("PROGRESSION_EVENT_" + eventIDs[0])
		{
			_eventIDs = eventIDs;
			ListenForPickup(this);
		}

		private static void ListenForPickup(ProgressionEventCondition condition)
		{
			if (!_isListenerHooked)
			{
				AchievementsHelper.OnProgressionEvent += ProgressionEventListener;
				_isListenerHooked = true;
			}
			for (int i = 0; i < condition._eventIDs.Length; i++)
			{
				if (!_listeners.ContainsKey(condition._eventIDs[i]))
				{
					_listeners[condition._eventIDs[i]] = new List<ProgressionEventCondition>();
				}
				_listeners[condition._eventIDs[i]].Add(condition);
			}
		}

		private static void ProgressionEventListener(int eventID)
		{
			if (!_listeners.ContainsKey(eventID))
			{
				return;
			}
			foreach (ProgressionEventCondition item in _listeners[eventID])
			{
				item.Complete();
			}
		}

		public static ProgressionEventCondition Create(params int[] eventIDs)
		{
			return new ProgressionEventCondition(eventIDs);
		}

		public static ProgressionEventCondition Create(int eventID)
		{
			return new ProgressionEventCondition(eventID);
		}

		public static ProgressionEventCondition[] CreateMany(params int[] eventIDs)
		{
			ProgressionEventCondition[] array = new ProgressionEventCondition[eventIDs.Length];
			for (int i = 0; i < eventIDs.Length; i++)
			{
				array[i] = new ProgressionEventCondition(eventIDs[i]);
			}
			return array;
		}
	}
}
