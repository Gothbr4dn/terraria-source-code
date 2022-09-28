using Terraria.ID;

namespace Terraria.DataStructures
{
	public class NPCDebuffImmunityData
	{
		public bool ImmuneToWhips;

		public bool ImmuneToAllBuffsThatAreNotWhips;

		public int[] SpecificallyImmuneTo;

		public void ApplyToNPC(NPC npc)
		{
			if (ImmuneToWhips || ImmuneToAllBuffsThatAreNotWhips)
			{
				for (int i = 1; i < 355; i++)
				{
					bool flag = BuffID.Sets.IsAnNPCWhipDebuff[i];
					bool flag2 = false;
					flag2 |= flag && ImmuneToWhips;
					flag2 |= !flag && ImmuneToAllBuffsThatAreNotWhips;
					npc.buffImmune[i] = flag2;
				}
			}
			if (SpecificallyImmuneTo != null)
			{
				for (int j = 0; j < SpecificallyImmuneTo.Length; j++)
				{
					int num = SpecificallyImmuneTo[j];
					npc.buffImmune[num] = true;
				}
			}
		}
	}
}
