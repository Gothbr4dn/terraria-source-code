namespace Terraria.Modules
{
	public class TileObjectDrawModule
	{
		public int xOffset;

		public int yOffset;

		public bool flipHorizontal;

		public bool flipVertical;

		public int stepDown;

		public TileObjectDrawModule(TileObjectDrawModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				xOffset = 0;
				yOffset = 0;
				flipHorizontal = false;
				flipVertical = false;
				stepDown = 0;
			}
			else
			{
				xOffset = copyFrom.xOffset;
				yOffset = copyFrom.yOffset;
				flipHorizontal = copyFrom.flipHorizontal;
				flipVertical = copyFrom.flipVertical;
				stepDown = copyFrom.stepDown;
			}
		}
	}
}
