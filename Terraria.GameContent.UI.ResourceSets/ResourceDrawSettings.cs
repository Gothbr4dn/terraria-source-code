using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.UI.ResourceSets
{
	public struct ResourceDrawSettings
	{
		public delegate void TextureGetter(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> texture, out Vector2 drawOffset, out float drawScale, out Rectangle? sourceRect);

		public Vector2 TopLeftAnchor;

		public int ElementCount;

		public int ElementIndexOffset;

		public TextureGetter GetTextureMethod;

		public Vector2 OffsetPerDraw;

		public Vector2 OffsetPerDrawByTexturePercentile;

		public Vector2 OffsetSpriteAnchor;

		public Vector2 OffsetSpriteAnchorByTexturePercentile;

		public void Draw(SpriteBatch spriteBatch, ref bool isHovered)
		{
			int elementCount = ElementCount;
			Vector2 topLeftAnchor = TopLeftAnchor;
			Point value = Main.MouseScreen.ToPoint();
			for (int i = 0; i < elementCount; i++)
			{
				int elementIndex = i + ElementIndexOffset;
				GetTextureMethod(elementIndex, ElementIndexOffset, ElementIndexOffset + elementCount - 1, out var texture, out var drawOffset, out var drawScale, out var sourceRect);
				Rectangle rectangle = texture.Frame();
				if (sourceRect.HasValue)
				{
					rectangle = sourceRect.Value;
				}
				Vector2 position = topLeftAnchor + drawOffset;
				Vector2 origin = OffsetSpriteAnchor + rectangle.Size() * OffsetSpriteAnchorByTexturePercentile;
				Rectangle rectangle2 = rectangle;
				rectangle2.X += (int)(position.X - origin.X);
				rectangle2.Y += (int)(position.Y - origin.Y);
				if (rectangle2.Contains(value))
				{
					isHovered = true;
				}
				spriteBatch.Draw(texture.get_Value(), position, rectangle, Color.White, 0f, origin, drawScale, SpriteEffects.None, 0f);
				topLeftAnchor += OffsetPerDraw + rectangle.Size() * OffsetPerDrawByTexturePercentile;
			}
		}
	}
}
