namespace Terraria.DataStructures
{
	public class EntitySource_ItemUse : IEntitySource
	{
		public readonly Entity Entity;

		public readonly Item Item;

		public EntitySource_ItemUse(Entity entity, Item item)
		{
			Entity = entity;
			Item = item;
		}
	}
}
