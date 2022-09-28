namespace Terraria.DataStructures
{
	public class EntitySource_Buff : IEntitySource
	{
		public readonly Entity Entity;

		public readonly int BuffId;

		public readonly int BuffIndex;

		public EntitySource_Buff(Entity entity, int buffId, int buffIndex)
		{
			Entity = entity;
			BuffId = buffId;
			BuffIndex = buffIndex;
		}
	}
}
