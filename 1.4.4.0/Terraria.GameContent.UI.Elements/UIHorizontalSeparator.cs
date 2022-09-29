using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIHorizontalSeparator : UIElement
	{
		private Asset<Texture2D> _texture;

		public Color Color;

		public int EdgeWidth;

		public UIHorizontalSeparator(int EdgeWidth = 2, bool highlightSideUp = true)
		{
			Color = Color.White;
			if (highlightSideUp)
			{
				_texture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Separator1", (AssetRequestMode)1);
			}
			else
			{
				_texture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Separator2", (AssetRequestMode)1);
			}
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			Utils.DrawPanel(_texture.get_Value(), EdgeWidth, 0, spriteBatch, dimensions.Position(), dimensions.Width, Color);
		}

		public override bool ContainsPoint(Vector2 point)
		{
			return false;
		}
	}
}
