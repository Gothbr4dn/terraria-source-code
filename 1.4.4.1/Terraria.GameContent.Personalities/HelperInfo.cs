using System.Collections.Generic;

namespace Terraria.GameContent.Personalities
{
	public struct HelperInfo
	{
		public Player player;

		public NPC npc;

		public List<NPC> NearbyNPCs;

		public bool[] nearbyNPCsByType;
	}
}
