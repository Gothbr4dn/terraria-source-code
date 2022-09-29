namespace Terraria.DataStructures
{
	public class EntitySource_OnHit_ByProjectileSourceID : AEntitySource_OnHit
	{
		public readonly int SourceId;

		public EntitySource_OnHit_ByProjectileSourceID(Entity entityStriking, Entity entityStruck, int projectileSourceId)
			: base(entityStriking, entityStruck)
		{
			SourceId = projectileSourceId;
		}
	}
}
