using System;
using System.Text;

namespace Terraria.Utilities
{
	public struct Vertical64BitStrips
	{
		private Bits64[] arr;

		public Bits64 this[int x]
		{
			get
			{
				return arr[x];
			}
			set
			{
				arr[x] = value;
			}
		}

		public Vertical64BitStrips(int len)
		{
			arr = new Bits64[len];
		}

		public void Clear()
		{
			Array.Clear(arr, 0, arr.Length);
		}

		public void Expand3x3()
		{
			for (int i = 0; i < arr.Length - 1; i++)
			{
				ref Bits64 reference = ref arr[i];
				reference = (ulong)reference | (ulong)arr[i + 1];
			}
			for (int num = arr.Length - 1; num > 0; num--)
			{
				ref Bits64 reference2 = ref arr[num];
				reference2 = (ulong)reference2 | (ulong)arr[num - 1];
			}
			for (int j = 0; j < arr.Length; j++)
			{
				Bits64 bits = arr[j];
				arr[j] = ((ulong)bits << 1) | (ulong)bits | ((ulong)bits >> 1);
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(arr.Length * 65);
			for (int i = 0; i < 64; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append('\n');
				}
				for (int j = 0; j < arr.Length; j++)
				{
					stringBuilder.Append(this[j][i] ? 'x' : ' ');
				}
			}
			return stringBuilder.ToString();
		}
	}
}
