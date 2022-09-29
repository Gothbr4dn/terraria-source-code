using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	public struct Point16
	{
		public readonly short X;

		public readonly short Y;

		public static Point16 Zero = new Point16(0, 0);

		public static Point16 NegativeOne = new Point16(-1, -1);

		public Point16(Point point)
		{
			X = (short)point.X;
			Y = (short)point.Y;
		}

		public Point16(int X, int Y)
		{
			this.X = (short)X;
			this.Y = (short)Y;
		}

		public Point16(short X, short Y)
		{
			this.X = X;
			this.Y = Y;
		}

		public static Point16 Max(int firstX, int firstY, int secondX, int secondY)
		{
			return new Point16((firstX > secondX) ? firstX : secondX, (firstY > secondY) ? firstY : secondY);
		}

		public Point16 Max(int compareX, int compareY)
		{
			return new Point16((X > compareX) ? X : compareX, (Y > compareY) ? Y : compareY);
		}

		public Point16 Max(Point16 compareTo)
		{
			return new Point16((X > compareTo.X) ? X : compareTo.X, (Y > compareTo.Y) ? Y : compareTo.Y);
		}

		public static bool operator ==(Point16 first, Point16 second)
		{
			if (first.X == second.X)
			{
				return first.Y == second.Y;
			}
			return false;
		}

		public static bool operator !=(Point16 first, Point16 second)
		{
			if (first.X == second.X)
			{
				return first.Y != second.Y;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			Point16 point = (Point16)obj;
			if (X != point.X || Y != point.Y)
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			return (X << 16) | (ushort)Y;
		}

		public override string ToString()
		{
			return $"{{{X}, {Y}}}";
		}
	}
}
