using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIVerticalSeparator : UIElement
	{
		private Asset<Texture2D> _texture;

		public Color Color;

		public int EdgeWidth;

		public UIVerticalSeparator()
		{
			Color = Color.White;
			_texture = Main.Assets.Request<Texture2D>("Images/UI/OnePixel", (AssetRequestMode)1);
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(destinationRectangle: GetDimensions().ToRectangle(), texture: _texture.get_Value(), color: Color);
		}

		public override bool ContainsPoint(Vector2 point)
		{
			return false;
		}
	}
}
