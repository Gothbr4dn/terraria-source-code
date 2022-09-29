namespace Terraria.DataStructures
{
	public class EntitySource_BossSpawn : IEntitySource
	{
		public readonly Entity Entity;

		public EntitySource_BossSpawn(Entity entity)
		{
			Entity = entity;
		}
	}
}
