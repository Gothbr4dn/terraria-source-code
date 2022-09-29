using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.UI;

namespace Terraria.Map
{
	public struct MapOverlayDrawContext
	{
		public struct DrawResult
		{
			public static readonly DrawResult Culled = new DrawResult(isMouseOver: false);

			public readonly bool IsMouseOver;

			public DrawResult(bool isMouseOver)
			{
				IsMouseOver = isMouseOver;
			}
		}

		private readonly Vector2 _mapPosition;

		private readonly Vector2 _mapOffset;

		private readonly Rectangle? _clippingRect;

		private readonly float _mapScale;

		private readonly float _drawScale;

		public MapOverlayDrawContext(Vector2 mapPosition, Vector2 mapOffset, Rectangle? clippingRect, float mapScale, float drawScale)
		{
			_mapPosition = mapPosition;
			_mapOffset = mapOffset;
			_clippingRect = clippingRect;
			_mapScale = mapScale;
			_drawScale = drawScale;
		}

		public DrawResult Draw(Texture2D texture, Vector2 position, Alignment alignment)
		{
			return Draw(texture, position, new SpriteFrame(1, 1), alignment);
		}

		public DrawResult Draw(Texture2D texture, Vector2 position, SpriteFrame frame, Alignment alignment)
		{
			position = (position - _mapPosition) * _mapScale + _mapOffset;
			if (_clippingRect.HasValue && !_clippingRect.Value.Contains(position.ToPoint()))
			{
				return DrawResult.Culled;
			}
			Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
			Vector2 vector = sourceRectangle.Size() * alignment.OffsetMultiplier;
			Main.spriteBatch.Draw(texture, position, sourceRectangle, Color.White, 0f, vector, _drawScale, SpriteEffects.None, 0f);
			position -= vector * _drawScale;
			return new DrawResult(new Rectangle((int)position.X, (int)position.Y, (int)((float)texture.Width * _drawScale), (int)((float)texture.Height * _drawScale)).Contains(Main.MouseScreen.ToPoint()));
		}

		public DrawResult Draw(Texture2D texture, Vector2 position, Color color, SpriteFrame frame, float scaleIfNotSelected, float scaleIfSelected, Alignment alignment)
		{
			position = (position - _mapPosition) * _mapScale + _mapOffset;
			if (_clippingRect.HasValue && !_clippingRect.Value.Contains(position.ToPoint()))
			{
				return DrawResult.Culled;
			}
			Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
			Vector2 vector = sourceRectangle.Size() * alignment.OffsetMultiplier;
			Vector2 position2 = position;
			float num = _drawScale * scaleIfNotSelected;
			Vector2 vector2 = position - vector * num;
			bool num2 = new Rectangle((int)vector2.X, (int)vector2.Y, (int)((float)sourceRectangle.Width * num), (int)((float)sourceRectangle.Height * num)).Contains(Main.MouseScreen.ToPoint());
			float scale = num;
			if (num2)
			{
				scale = _drawScale * scaleIfSelected;
			}
			Main.spriteBatch.Draw(texture, position2, sourceRectangle, color, 0f, vector, scale, SpriteEffects.None, 0f);
			return new DrawResult(num2);
		}
	}
}
