using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	public struct LineSegment
	{
		public Vector2 Start;

		public Vector2 End;

		public LineSegment(Vector2 start, Vector2 end)
		{
			Start = start;
			End = end;
		}
	}
}
