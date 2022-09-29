namespace Terraria.Modules
{
	public class LiquidDeathModule
	{
		public bool water;

		public bool lava;

		public LiquidDeathModule(LiquidDeathModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				water = false;
				lava = false;
			}
			else
			{
				water = copyFrom.water;
				lava = copyFrom.lava;
			}
		}
	}
}
