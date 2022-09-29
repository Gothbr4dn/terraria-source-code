using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar
{
	public class BigProgressBarHelper
	{
		private const string _bossBarTexturePath = "Images/UI/UI_BossBar";

		public static void DrawBareBonesBar(SpriteBatch spriteBatch, float lifePercent)
		{
			Rectangle rectangle = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), new Vector2(400f, 20f));
			Rectangle destinationRectangle = rectangle;
			destinationRectangle.Inflate(2, 2);
			Texture2D value = TextureAssets.MagicPixel.get_Value();
			Rectangle value2 = new Rectangle(0, 0, 1, 1);
			Rectangle destinationRectangle2 = rectangle;
			destinationRectangle2.Width = (int)((float)destinationRectangle2.Width * lifePercent);
			spriteBatch.Draw(value, destinationRectangle, value2, Color.White * 0.6f);
			spriteBatch.Draw(value, rectangle, value2, Color.Black * 0.6f);
			spriteBatch.Draw(value, destinationRectangle2, value2, Color.LimeGreen * 0.5f);
		}

		public static void DrawFancyBar(SpriteBatch spriteBatch, float lifeAmount, float lifeMax, Texture2D barIconTexture, Rectangle barIconFrame)
		{
			Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/UI_BossBar", (AssetRequestMode)1).get_Value();
			Point p = new Point(456, 22);
			Point p2 = new Point(32, 24);
			int verticalFrames = 6;
			Rectangle value2 = value.Frame(1, verticalFrames, 0, 3);
			Color color = Color.White * 0.2f;
			float num = lifeAmount / lifeMax;
			int num2 = (int)((float)p.X * num);
			num2 -= num2 % 2;
			Rectangle value3 = value.Frame(1, verticalFrames, 0, 2);
			value3.X += p2.X;
			value3.Y += p2.Y;
			value3.Width = 2;
			value3.Height = p.Y;
			Rectangle value4 = value.Frame(1, verticalFrames, 0, 1);
			value4.X += p2.X;
			value4.Y += p2.Y;
			value4.Width = 2;
			value4.Height = p.Y;
			Rectangle rectangle = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), p.ToVector2());
			Vector2 vector = rectangle.TopLeft() - p2.ToVector2();
			spriteBatch.Draw(value, vector, value2, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle.TopLeft(), value3, Color.White, 0f, Vector2.Zero, new Vector2(num2 / value3.Width, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle.TopLeft() + new Vector2(num2 - 2, 0f), value4, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Rectangle value5 = value.Frame(1, verticalFrames);
			spriteBatch.Draw(value, vector, value5, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Vector2 vector2 = new Vector2(4f, 20f) + new Vector2(26f, 28f) / 2f;
			spriteBatch.Draw(barIconTexture, vector + vector2, barIconFrame, Color.White, 0f, barIconFrame.Size() / 2f, 1f, SpriteEffects.None, 0f);
			if (BigProgressBarSystem.ShowText)
			{
				DrawHealthText(spriteBatch, rectangle, lifeAmount, lifeMax);
			}
		}

		private static void DrawHealthText(SpriteBatch spriteBatch, Rectangle area, float current, float max)
		{
			DynamicSpriteFont value = FontAssets.ItemStack.get_Value();
			Vector2 vector = area.Center.ToVector2();
			vector.Y += 1f;
			string text = "/";
			Vector2 vector2 = value.MeasureString(text);
			Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X, vector.Y, Color.White, Color.Black, vector2 * 0.5f);
			text = ((int)current).ToString();
			vector2 = value.MeasureString(text);
			Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X - 5f, vector.Y, Color.White, Color.Black, vector2 * new Vector2(1f, 0.5f));
			text = ((int)max).ToString();
			vector2 = value.MeasureString(text);
			Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X + 5f, vector.Y, Color.White, Color.Black, vector2 * new Vector2(0f, 0.5f));
		}

		public static void DrawFancyBar(SpriteBatch spriteBatch, float lifeAmount, float lifeMax, Texture2D barIconTexture, Rectangle barIconFrame, float shieldCurrent, float shieldMax)
		{
			Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/UI_BossBar", (AssetRequestMode)1).get_Value();
			Point p = new Point(456, 22);
			Point p2 = new Point(32, 24);
			int verticalFrames = 6;
			Rectangle value2 = value.Frame(1, verticalFrames, 0, 3);
			Color color = Color.White * 0.2f;
			float num = lifeAmount / lifeMax;
			int num2 = (int)((float)p.X * num);
			num2 -= num2 % 2;
			Rectangle value3 = value.Frame(1, verticalFrames, 0, 2);
			value3.X += p2.X;
			value3.Y += p2.Y;
			value3.Width = 2;
			value3.Height = p.Y;
			Rectangle value4 = value.Frame(1, verticalFrames, 0, 1);
			value4.X += p2.X;
			value4.Y += p2.Y;
			value4.Width = 2;
			value4.Height = p.Y;
			float num3 = shieldCurrent / shieldMax;
			int num4 = (int)((float)p.X * num3);
			num4 -= num4 % 2;
			Rectangle value5 = value.Frame(1, verticalFrames, 0, 5);
			value5.X += p2.X;
			value5.Y += p2.Y;
			value5.Width = 2;
			value5.Height = p.Y;
			Rectangle value6 = value.Frame(1, verticalFrames, 0, 4);
			value6.X += p2.X;
			value6.Y += p2.Y;
			value6.Width = 2;
			value6.Height = p.Y;
			Rectangle rectangle = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), p.ToVector2());
			Vector2 vector = rectangle.TopLeft() - p2.ToVector2();
			spriteBatch.Draw(value, vector, value2, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle.TopLeft(), value3, Color.White, 0f, Vector2.Zero, new Vector2(num2 / value3.Width, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle.TopLeft() + new Vector2(num2 - 2, 0f), value4, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle.TopLeft(), value5, Color.White, 0f, Vector2.Zero, new Vector2(num4 / value5.Width, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, rectangle.TopLeft() + new Vector2(num4 - 2, 0f), value6, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Rectangle value7 = value.Frame(1, verticalFrames);
			spriteBatch.Draw(value, vector, value7, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			Vector2 vector2 = new Vector2(4f, 20f) + barIconFrame.Size() / 2f;
			spriteBatch.Draw(barIconTexture, vector + vector2, barIconFrame, Color.White, 0f, barIconFrame.Size() / 2f, 1f, SpriteEffects.None, 0f);
			if (BigProgressBarSystem.ShowText)
			{
				if (shieldCurrent > 0f)
				{
					DrawHealthText(spriteBatch, rectangle, shieldCurrent, shieldMax);
				}
				else
				{
					DrawHealthText(spriteBatch, rectangle, lifeAmount, lifeMax);
				}
			}
		}
	}
}
