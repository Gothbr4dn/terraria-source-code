using System;

namespace Terraria.Utilities
{
	[Serializable]
	public class UnifiedRandom
	{
		private const int MBIG = int.MaxValue;

		private const int MSEED = 161803398;

		private const int MZ = 0;

		private int inext;

		private int inextp;

		private int[] SeedArray = new int[56];

		public UnifiedRandom()
			: this(Environment.TickCount)
		{
		}

		public UnifiedRandom(int Seed)
		{
			SetSeed(Seed);
		}

		public void SetSeed(int Seed)
		{
			for (int i = 0; i < SeedArray.Length; i++)
			{
				SeedArray[i] = 0;
			}
			int num = ((Seed == int.MinValue) ? int.MaxValue : Math.Abs(Seed));
			int num2 = 161803398 - num;
			SeedArray[55] = num2;
			int num3 = 1;
			for (int j = 1; j < 55; j++)
			{
				int num4 = 21 * j % 55;
				SeedArray[num4] = num3;
				num3 = num2 - num3;
				if (num3 < 0)
				{
					num3 += int.MaxValue;
				}
				num2 = SeedArray[num4];
			}
			for (int k = 1; k < 5; k++)
			{
				for (int l = 1; l < 56; l++)
				{
					SeedArray[l] -= SeedArray[1 + (l + 30) % 55];
					if (SeedArray[l] < 0)
					{
						SeedArray[l] += int.MaxValue;
					}
				}
			}
			inext = 0;
			inextp = 21;
		}

		protected virtual double Sample()
		{
			return (double)InternalSample() * 4.656612875245797E-10;
		}

		private int InternalSample()
		{
			int num = inext;
			int num2 = inextp;
			if (++num >= 56)
			{
				num = 1;
			}
			if (++num2 >= 56)
			{
				num2 = 1;
			}
			int num3 = SeedArray[num] - SeedArray[num2];
			if (num3 == int.MaxValue)
			{
				num3--;
			}
			if (num3 < 0)
			{
				num3 += int.MaxValue;
			}
			SeedArray[num] = num3;
			inext = num;
			inextp = num2;
			return num3;
		}

		public virtual int Next()
		{
			return InternalSample();
		}

		private double GetSampleForLargeRange()
		{
			int num = InternalSample();
			if ((InternalSample() % 2 == 0) ? true : false)
			{
				num = -num;
			}
			return ((double)num + 2147483646.0) / 4294967293.0;
		}

		public virtual int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
			{
				throw new ArgumentOutOfRangeException("minValue", "minValue must be less than maxValue");
			}
			long num = (long)maxValue - (long)minValue;
			if (num <= int.MaxValue)
			{
				return (int)(Sample() * (double)num) + minValue;
			}
			return (int)((long)(GetSampleForLargeRange() * (double)num) + minValue);
		}

		public virtual int Next(int maxValue)
		{
			if (maxValue < 0)
			{
				throw new ArgumentOutOfRangeException("maxValue", "maxValue must be positive.");
			}
			return (int)(Sample() * (double)maxValue);
		}

		public virtual double NextDouble()
		{
			return Sample();
		}

		public virtual void NextBytes(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = (byte)(InternalSample() % 256);
			}
		}
	}
}
