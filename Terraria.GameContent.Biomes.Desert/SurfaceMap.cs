using System;

namespace Terraria.GameContent.Biomes.Desert
{
	public class SurfaceMap
	{
		public readonly double Average;

		public readonly int Bottom;

		public readonly int Top;

		public readonly int X;

		private readonly short[] _heights;

		public int Width => _heights.Length;

		public short this[int absoluteX] => _heights[absoluteX - X];

		private SurfaceMap(short[] heights, int x)
		{
			_heights = heights;
			X = x;
			int num = 0;
			int num2 = int.MaxValue;
			int num3 = 0;
			for (int i = 0; i < heights.Length; i++)
			{
				num3 += heights[i];
				num = Math.Max(num, heights[i]);
				num2 = Math.Min(num2, heights[i]);
			}
			if ((double)num > Main.worldSurface - 10.0)
			{
				num = (int)Main.worldSurface - 10;
			}
			Bottom = num;
			Top = num2;
			Average = (double)num3 / (double)_heights.Length;
		}

		public static SurfaceMap FromArea(int startX, int width)
		{
			int num = Main.maxTilesY / 2;
			short[] array = new short[width];
			for (int i = startX; i < startX + width; i++)
			{
				bool flag = false;
				int num2 = 0;
				for (int j = 50; j < 50 + num; j++)
				{
					if (Main.tile[i, j].active())
					{
						if (Main.tile[i, j].type == 189 || Main.tile[i, j].type == 196 || Main.tile[i, j].type == 460)
						{
							flag = false;
						}
						else if (!flag)
						{
							num2 = j;
							flag = true;
						}
					}
					if (!flag)
					{
						num2 = num + 50;
					}
				}
				array[i - startX] = (short)num2;
			}
			return new SurfaceMap(array, startX);
		}
	}
}
