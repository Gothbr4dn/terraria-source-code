namespace Terraria.DataStructures
{
	public class EntitySource_TileEntity : IEntitySource
	{
		public TileEntity TileEntity;

		public EntitySource_TileEntity(TileEntity tileEntity)
		{
			TileEntity = tileEntity;
		}
	}
}
