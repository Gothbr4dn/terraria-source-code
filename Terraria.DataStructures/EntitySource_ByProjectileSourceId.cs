namespace Terraria.DataStructures
{
	public class EntitySource_ByProjectileSourceId : IEntitySource
	{
		public readonly int SourceId;

		public EntitySource_ByProjectileSourceId(int projectileSourceId)
		{
			SourceId = projectileSourceId;
		}
	}
}
