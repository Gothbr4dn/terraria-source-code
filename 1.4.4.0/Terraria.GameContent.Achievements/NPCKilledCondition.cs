using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	public class NPCKilledCondition : AchievementCondition
	{
		private const string Identifier = "NPC_KILLED";

		private static Dictionary<short, List<NPCKilledCondition>> _listeners = new Dictionary<short, List<NPCKilledCondition>>();

		private static bool _isListenerHooked;

		private short[] _npcIds;

		private NPCKilledCondition(short npcId)
			: base("NPC_KILLED_" + npcId)
		{
			_npcIds = new short[1] { npcId };
			ListenForPickup(this);
		}

		private NPCKilledCondition(short[] npcIds)
			: base("NPC_KILLED_" + npcIds[0])
		{
			_npcIds = npcIds;
			ListenForPickup(this);
		}

		private static void ListenForPickup(NPCKilledCondition condition)
		{
			if (!_isListenerHooked)
			{
				AchievementsHelper.OnNPCKilled += NPCKilledListener;
				_isListenerHooked = true;
			}
			for (int i = 0; i < condition._npcIds.Length; i++)
			{
				if (!_listeners.ContainsKey(condition._npcIds[i]))
				{
					_listeners[condition._npcIds[i]] = new List<NPCKilledCondition>();
				}
				_listeners[condition._npcIds[i]].Add(condition);
			}
		}

		private static void NPCKilledListener(Player player, short npcId)
		{
			if (player.whoAmI != Main.myPlayer || !_listeners.ContainsKey(npcId))
			{
				return;
			}
			foreach (NPCKilledCondition item in _listeners[npcId])
			{
				item.Complete();
			}
		}

		public static AchievementCondition Create(params short[] npcIds)
		{
			return new NPCKilledCondition(npcIds);
		}

		public static AchievementCondition Create(short npcId)
		{
			return new NPCKilledCondition(npcId);
		}

		public static AchievementCondition[] CreateMany(params short[] npcs)
		{
			AchievementCondition[] array = new AchievementCondition[npcs.Length];
			for (int i = 0; i < npcs.Length; i++)
			{
				array[i] = new NPCKilledCondition(npcs[i]);
			}
			return array;
		}
	}
}
