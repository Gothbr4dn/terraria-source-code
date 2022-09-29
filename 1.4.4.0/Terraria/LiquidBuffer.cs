namespace Terraria
{
	public class LiquidBuffer
	{
		public static int numLiquidBuffer;

		public int x;

		public int y;

		public static void AddBuffer(int x, int y)
		{
			if (numLiquidBuffer < 49998 && !Main.tile[x, y].checkingLiquid())
			{
				Main.tile[x, y].checkingLiquid(checkingLiquid: true);
				Main.liquidBuffer[numLiquidBuffer].x = x;
				Main.liquidBuffer[numLiquidBuffer].y = y;
				numLiquidBuffer++;
			}
		}

		public static void DelBuffer(int l)
		{
			numLiquidBuffer--;
			Main.liquidBuffer[l].x = Main.liquidBuffer[numLiquidBuffer].x;
			Main.liquidBuffer[l].y = Main.liquidBuffer[numLiquidBuffer].y;
		}
	}
}
