namespace Terraria.WorldBuilding
{
	public abstract class GenModShape : GenShape
	{
		protected ShapeData _data;

		public GenModShape(ShapeData data)
		{
			_data = data;
		}
	}
}
