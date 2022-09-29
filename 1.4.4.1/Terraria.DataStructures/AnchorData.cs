using Terraria.Enums;

namespace Terraria.DataStructures
{
	public struct AnchorData
	{
		public AnchorType type;

		public int tileCount;

		public int checkStart;

		public static AnchorData Empty;

		public AnchorData(AnchorType type, int count, int start)
		{
			this.type = type;
			tileCount = count;
			checkStart = start;
		}

		public static bool operator ==(AnchorData data1, AnchorData data2)
		{
			if (data1.type == data2.type && data1.tileCount == data2.tileCount)
			{
				return data1.checkStart == data2.checkStart;
			}
			return false;
		}

		public static bool operator !=(AnchorData data1, AnchorData data2)
		{
			if (data1.type == data2.type && data1.tileCount == data2.tileCount)
			{
				return data1.checkStart != data2.checkStart;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj is AnchorData)
			{
				if (type == ((AnchorData)obj).type && tileCount == ((AnchorData)obj).tileCount)
				{
					return checkStart == ((AnchorData)obj).checkStart;
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			byte b = (byte)checkStart;
			byte b2 = (byte)tileCount;
			return ((ushort)type << 16) | (b2 << 8) | b;
		}
	}
}
