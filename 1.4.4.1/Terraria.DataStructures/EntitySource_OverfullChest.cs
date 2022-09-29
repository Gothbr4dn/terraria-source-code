namespace Terraria.DataStructures
{
	public class EntitySource_OverfullChest : AEntitySource_Tile
	{
		public readonly Chest Chest;

		public EntitySource_OverfullChest(int tileCoordsX, int tileCoordsY, Chest chest)
			: base(tileCoordsX, tileCoordsY)
		{
			Chest = chest;
		}
	}
}
