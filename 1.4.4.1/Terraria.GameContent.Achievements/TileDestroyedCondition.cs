using System.Collections.Generic;
using Terraria.Achievements;

namespace Terraria.GameContent.Achievements
{
	public class TileDestroyedCondition : AchievementCondition
	{
		private const string Identifier = "TILE_DESTROYED";

		private static Dictionary<ushort, List<TileDestroyedCondition>> _listeners = new Dictionary<ushort, List<TileDestroyedCondition>>();

		private static bool _isListenerHooked;

		private ushort[] _tileIds;

		private TileDestroyedCondition(ushort[] tileIds)
			: base("TILE_DESTROYED_" + tileIds[0])
		{
			_tileIds = tileIds;
			ListenForDestruction(this);
		}

		private static void ListenForDestruction(TileDestroyedCondition condition)
		{
			if (!_isListenerHooked)
			{
				AchievementsHelper.OnTileDestroyed += TileDestroyedListener;
				_isListenerHooked = true;
			}
			for (int i = 0; i < condition._tileIds.Length; i++)
			{
				if (!_listeners.ContainsKey(condition._tileIds[i]))
				{
					_listeners[condition._tileIds[i]] = new List<TileDestroyedCondition>();
				}
				_listeners[condition._tileIds[i]].Add(condition);
			}
		}

		private static void TileDestroyedListener(Player player, ushort tileId)
		{
			if (player.whoAmI != Main.myPlayer || !_listeners.ContainsKey(tileId))
			{
				return;
			}
			foreach (TileDestroyedCondition item in _listeners[tileId])
			{
				item.Complete();
			}
		}

		public static AchievementCondition Create(params ushort[] tileIds)
		{
			return new TileDestroyedCondition(tileIds);
		}
	}
}
