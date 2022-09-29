namespace Terraria.DataStructures
{
	public class EntitySource_OnHit_ByItemSourceID : AEntitySource_OnHit
	{
		public readonly int SourceId;

		public EntitySource_OnHit_ByItemSourceID(Entity entityStriking, Entity entityStruck, int itemSourceId)
			: base(entityStriking, entityStruck)
		{
			SourceId = itemSourceId;
		}
	}
}
