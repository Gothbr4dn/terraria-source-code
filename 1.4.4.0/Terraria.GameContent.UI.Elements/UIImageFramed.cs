using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIImageFramed : UIElement, IColorable
	{
		private Asset<Texture2D> _texture;

		private Rectangle _frame;

		public Color Color { get; set; }

		public UIImageFramed(Asset<Texture2D> texture, Rectangle frame)
		{
			_texture = texture;
			_frame = frame;
			Width.Set(_frame.Width, 0f);
			Height.Set(_frame.Height, 0f);
			Color = Color.White;
		}

		public void SetImage(Asset<Texture2D> texture, Rectangle frame)
		{
			_texture = texture;
			_frame = frame;
			Width.Set(_frame.Width, 0f);
			Height.Set(_frame.Height, 0f);
		}

		public void SetFrame(Rectangle frame)
		{
			_frame = frame;
			Width.Set(_frame.Width, 0f);
			Height.Set(_frame.Height, 0f);
		}

		public void SetFrame(int frameCountHorizontal, int frameCountVertical, int frameX, int frameY, int sizeOffsetX, int sizeOffsetY)
		{
			SetFrame(_texture.Frame(frameCountHorizontal, frameCountVertical, frameX, frameY).OffsetSize(sizeOffsetX, sizeOffsetY));
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(position: GetDimensions().Position(), texture: _texture.get_Value(), sourceRectangle: _frame, color: Color);
		}
	}
}
