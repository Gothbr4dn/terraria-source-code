namespace Terraria.DataStructures
{
	public class EntitySource_DropAsItem : IEntitySource
	{
		public readonly Entity Entity;

		public EntitySource_DropAsItem(Entity entity)
		{
			Entity = entity;
		}
	}
}
