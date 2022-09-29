using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIColoredSliderSimple : UIElement
	{
		public float FillPercent;

		public Color FilledColor = Main.OurFavoriteColor;

		public Color EmptyColor = Color.Black;

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			DrawValueBarDynamicWidth(spriteBatch);
		}

		private void DrawValueBarDynamicWidth(SpriteBatch sb)
		{
			Texture2D value = TextureAssets.ColorBar.get_Value();
			Rectangle rectangle = GetDimensions().ToRectangle();
			Rectangle rectangle2 = new Rectangle(5, 4, 4, 4);
			Utils.DrawSplicedPanel(sb, value, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, rectangle2.X, rectangle2.Width, rectangle2.Y, rectangle2.Height, Color.White);
			Rectangle rectangle3 = rectangle;
			rectangle3.X += rectangle2.Left;
			rectangle3.Width -= rectangle2.Right;
			rectangle3.Y += rectangle2.Top;
			rectangle3.Height -= rectangle2.Bottom;
			Texture2D value2 = TextureAssets.MagicPixel.get_Value();
			Rectangle value3 = new Rectangle(0, 0, 1, 1);
			sb.Draw(value2, rectangle3, value3, EmptyColor);
			Rectangle destinationRectangle = rectangle3;
			destinationRectangle.Width = (int)((float)destinationRectangle.Width * FillPercent);
			sb.Draw(value2, destinationRectangle, value3, FilledColor);
		}
	}
}
