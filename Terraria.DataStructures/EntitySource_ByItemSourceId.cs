namespace Terraria.DataStructures
{
	public class EntitySource_ByItemSourceId : IEntitySource
	{
		public readonly Entity Entity;

		public readonly int SourceId;

		public EntitySource_ByItemSourceId(Entity entity, int itemSourceId)
		{
			Entity = entity;
			SourceId = itemSourceId;
		}
	}
}
