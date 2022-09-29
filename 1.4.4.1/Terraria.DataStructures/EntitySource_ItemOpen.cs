namespace Terraria.DataStructures
{
	public class EntitySource_ItemOpen : IEntitySource
	{
		public readonly Entity Entity;

		public readonly int ItemType;

		public EntitySource_ItemOpen(Entity entity, int itemType)
		{
			Entity = entity;
			ItemType = itemType;
		}
	}
}
