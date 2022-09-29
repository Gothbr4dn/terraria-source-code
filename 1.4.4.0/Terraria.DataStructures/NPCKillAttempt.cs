namespace Terraria.DataStructures
{
	public struct NPCKillAttempt
	{
		public readonly NPC npc;

		public readonly int netId;

		public readonly bool active;

		public NPCKillAttempt(NPC target)
		{
			npc = target;
			netId = target.netID;
			active = target.active;
		}

		public bool DidNPCDie()
		{
			return !npc.active;
		}

		public bool DidNPCDieOrTransform()
		{
			if (!DidNPCDie())
			{
				return npc.netID != netId;
			}
			return true;
		}
	}
}
