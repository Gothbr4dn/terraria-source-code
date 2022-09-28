namespace Terraria.Physics
{
	public struct BallPassThroughEvent
	{
		public readonly Tile Tile;

		public readonly Entity Entity;

		public readonly BallPassThroughType Type;

		public readonly float TimeScale;

		public BallPassThroughEvent(float timeScale, Tile tile, Entity entity, BallPassThroughType type)
		{
			Tile = tile;
			Entity = entity;
			Type = type;
			TimeScale = timeScale;
		}
	}
}
