namespace Terraria.Utilities
{
	public struct Bits64
	{
		private ulong v;

		public bool this[int i]
		{
			get
			{
				return (v & (ulong)(1L << i)) != 0;
			}
			set
			{
				if (value)
				{
					v |= (ulong)(1L << i);
				}
				else
				{
					v &= (ulong)(~(1L << i));
				}
			}
		}

		public bool IsEmpty => v == 0;

		public static implicit operator ulong(Bits64 b)
		{
			return b.v;
		}

		public static implicit operator Bits64(ulong v)
		{
			Bits64 result = default(Bits64);
			result.v = v;
			return result;
		}
	}
}
