using Microsoft.Xna.Framework;

namespace Terraria.Graphics
{
	public struct VertexColors
	{
		public Color TopLeftColor;

		public Color TopRightColor;

		public Color BottomLeftColor;

		public Color BottomRightColor;

		public VertexColors(Color color)
		{
			TopLeftColor = color;
			TopRightColor = color;
			BottomRightColor = color;
			BottomLeftColor = color;
		}

		public VertexColors(Color topLeft, Color topRight, Color bottomRight, Color bottomLeft)
		{
			TopLeftColor = topLeft;
			TopRightColor = topRight;
			BottomLeftColor = bottomLeft;
			BottomRightColor = bottomRight;
		}
	}
}
