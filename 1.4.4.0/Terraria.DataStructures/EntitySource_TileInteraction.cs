namespace Terraria.DataStructures
{
	public class EntitySource_TileInteraction : AEntitySource_Tile
	{
		public readonly Entity Entity;

		public EntitySource_TileInteraction(Entity entity, int tileCoordsX, int tileCoordsY)
			: base(tileCoordsX, tileCoordsY)
		{
			Entity = entity;
		}
	}
}
