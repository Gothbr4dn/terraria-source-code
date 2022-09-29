using System;
using System.Collections.Generic;
using System.IO;

namespace Terraria
{
	public struct BitsByte
	{
		private static bool Null;

		private byte value;

		public bool this[int key]
		{
			get
			{
				return (value & (1 << key)) != 0;
			}
			set
			{
				if (value)
				{
					this.value |= (byte)(1 << key);
				}
				else
				{
					this.value &= (byte)(~(1 << key));
				}
			}
		}

		public BitsByte(bool b1 = false, bool b2 = false, bool b3 = false, bool b4 = false, bool b5 = false, bool b6 = false, bool b7 = false, bool b8 = false)
		{
			value = 0;
			this[0] = b1;
			this[1] = b2;
			this[2] = b3;
			this[3] = b4;
			this[4] = b5;
			this[5] = b6;
			this[6] = b7;
			this[7] = b8;
		}

		public void ClearAll()
		{
			value = 0;
		}

		public void SetAll()
		{
			value = byte.MaxValue;
		}

		public void Retrieve(ref bool b0)
		{
			Retrieve(ref b0, ref Null, ref Null, ref Null, ref Null, ref Null, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1)
		{
			Retrieve(ref b0, ref b1, ref Null, ref Null, ref Null, ref Null, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2)
		{
			Retrieve(ref b0, ref b1, ref b2, ref Null, ref Null, ref Null, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3)
		{
			Retrieve(ref b0, ref b1, ref b2, ref b3, ref Null, ref Null, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4)
		{
			Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref Null, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5)
		{
			Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref b5, ref Null, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5, ref bool b6)
		{
			Retrieve(ref b0, ref b1, ref b2, ref b3, ref b4, ref b5, ref b6, ref Null);
		}

		public void Retrieve(ref bool b0, ref bool b1, ref bool b2, ref bool b3, ref bool b4, ref bool b5, ref bool b6, ref bool b7)
		{
			b0 = this[0];
			b1 = this[1];
			b2 = this[2];
			b3 = this[3];
			b4 = this[4];
			b5 = this[5];
			b6 = this[6];
			b7 = this[7];
		}

		public static implicit operator byte(BitsByte bb)
		{
			return bb.value;
		}

		public static implicit operator BitsByte(byte b)
		{
			BitsByte result = default(BitsByte);
			result.value = b;
			return result;
		}

		public static BitsByte[] ComposeBitsBytesChain(bool optimizeLength, params bool[] flags)
		{
			int num = flags.Length;
			int num2 = 0;
			while (num > 0)
			{
				num2++;
				num -= 7;
			}
			BitsByte[] array = new BitsByte[num2];
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < flags.Length; i++)
			{
				array[num4][num3] = flags[i];
				num3++;
				if (num3 == 7 && num4 < num2 - 1)
				{
					array[num4][num3] = true;
					num3 = 0;
					num4++;
				}
			}
			if (optimizeLength)
			{
				int num5 = array.Length - 1;
				while ((byte)array[num5] == 0 && num5 > 0)
				{
					array[num5 - 1][7] = false;
					num5--;
				}
				Array.Resize(ref array, num5 + 1);
			}
			return array;
		}

		public static BitsByte[] DecomposeBitsBytesChain(BinaryReader reader)
		{
			List<BitsByte> list = new List<BitsByte>();
			BitsByte item;
			do
			{
				item = reader.ReadByte();
				list.Add(item);
			}
			while (item[7]);
			return list.ToArray();
		}
	}
}
