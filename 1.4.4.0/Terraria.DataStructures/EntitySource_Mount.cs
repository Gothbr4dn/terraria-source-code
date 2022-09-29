namespace Terraria.DataStructures
{
	public class EntitySource_Mount : IEntitySource
	{
		public readonly Entity Entity;

		public readonly int MountId;

		public EntitySource_Mount(Entity entity, int mountId)
		{
			Entity = entity;
			MountId = mountId;
		}
	}
}
