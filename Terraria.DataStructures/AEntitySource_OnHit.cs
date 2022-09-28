namespace Terraria.DataStructures
{
	public abstract class AEntitySource_OnHit : IEntitySource
	{
		public readonly Entity EntityStriking;

		public readonly Entity EntityStruck;

		public AEntitySource_OnHit(Entity entityStriking, Entity entityStruck)
		{
			EntityStriking = entityStriking;
			EntityStruck = entityStruck;
		}
	}
}
