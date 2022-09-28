namespace Terraria.DataStructures
{
	public class EntitySource_Loot : IEntitySource
	{
		public readonly Entity Entity;

		public EntitySource_Loot(Entity entity)
		{
			Entity = entity;
		}
	}
}
