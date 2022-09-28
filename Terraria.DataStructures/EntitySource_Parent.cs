namespace Terraria.DataStructures
{
	public class EntitySource_Parent : IEntitySource
	{
		public readonly Entity Entity;

		public EntitySource_Parent(Entity entity)
		{
			Entity = entity;
		}
	}
}
