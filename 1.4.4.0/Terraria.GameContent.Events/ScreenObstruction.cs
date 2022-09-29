using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Events
{
	public class ScreenObstruction
	{
		public static float screenObstruction;

		public static void Update()
		{
			float value = 0f;
			float amount = 0.1f;
			if (Main.player[Main.myPlayer].headcovered)
			{
				value = 0.95f;
				amount = 0.3f;
			}
			screenObstruction = MathHelper.Lerp(screenObstruction, value, amount);
		}

		public static void Draw(SpriteBatch spriteBatch)
		{
			if (screenObstruction != 0f)
			{
				Color color = Color.Black * screenObstruction;
				int num = TextureAssets.Extra[49].Width();
				int num2 = 10;
				Rectangle rect = Main.player[Main.myPlayer].getRect();
				rect.Inflate((num - rect.Width) / 2, (num - rect.Height) / 2 + num2 / 2);
				rect.Offset(-(int)Main.screenPosition.X, -(int)Main.screenPosition.Y + (int)Main.player[Main.myPlayer].gfxOffY - num2);
				Rectangle destinationRectangle = Rectangle.Union(new Rectangle(0, 0, 1, 1), new Rectangle(rect.Right - 1, rect.Top - 1, 1, 1));
				Rectangle destinationRectangle2 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, 0, 1, 1), new Rectangle(rect.Right, rect.Bottom - 1, 1, 1));
				Rectangle destinationRectangle3 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left, rect.Bottom, 1, 1));
				Rectangle destinationRectangle4 = Rectangle.Union(new Rectangle(0, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left - 1, rect.Top, 1, 1));
				spriteBatch.Draw(TextureAssets.MagicPixel.get_Value(), destinationRectangle, new Rectangle(0, 0, 1, 1), color);
				spriteBatch.Draw(TextureAssets.MagicPixel.get_Value(), destinationRectangle2, new Rectangle(0, 0, 1, 1), color);
				spriteBatch.Draw(TextureAssets.MagicPixel.get_Value(), destinationRectangle3, new Rectangle(0, 0, 1, 1), color);
				spriteBatch.Draw(TextureAssets.MagicPixel.get_Value(), destinationRectangle4, new Rectangle(0, 0, 1, 1), color);
				spriteBatch.Draw(TextureAssets.Extra[49].get_Value(), rect, color);
			}
		}
	}
}
