using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	public struct DrillDebugDraw
	{
		public Vector2 point;

		public Color color;

		public DrillDebugDraw(Vector2 p, Color c)
		{
			point = p;
			color = c;
		}
	}
}
