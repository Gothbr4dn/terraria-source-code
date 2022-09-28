using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent.UI.ResourceSets
{
	public class ClassicPlayerResourcesDisplaySet : IPlayerResourcesDisplaySet, IConfigKeyHolder
	{
		private int UIDisplay_ManaPerStar = 20;

		private float UIDisplay_LifePerHeart = 20f;

		private int UI_ScreenAnchorX;

		public string NameKey { get; private set; }

		public string ConfigKey { get; private set; }

		public ClassicPlayerResourcesDisplaySet(string nameKey, string configKey)
		{
			NameKey = nameKey;
			ConfigKey = configKey;
		}

		public void Draw()
		{
			UI_ScreenAnchorX = Main.screenWidth - 800;
			DrawLife();
			DrawMana();
		}

		private void DrawLife()
		{
			Player localPlayer = Main.LocalPlayer;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Color color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
			UIDisplay_LifePerHeart = 20f;
			if (localPlayer.ghost)
			{
				return;
			}
			int num = localPlayer.statLifeMax / 20;
			int num2 = (localPlayer.statLifeMax - 400) / 5;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 > 0)
			{
				num = localPlayer.statLifeMax / (20 + num2 / 4);
				UIDisplay_LifePerHeart = (float)localPlayer.statLifeMax / 20f;
			}
			int num3 = localPlayer.statLifeMax2 - localPlayer.statLifeMax;
			UIDisplay_LifePerHeart += num3 / num;
			int num4 = (int)((float)localPlayer.statLifeMax2 / UIDisplay_LifePerHeart);
			if (num4 >= 10)
			{
				num4 = 10;
			}
			string text = Lang.inter[0].Value + " " + localPlayer.statLifeMax2 + "/" + localPlayer.statLifeMax2;
			Vector2 vector = FontAssets.MouseText.get_Value().MeasureString(text);
			if (!localPlayer.ghost)
			{
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), Lang.inter[0].Value, new Vector2((float)(500 + 13 * num4) - vector.X * 0.5f + (float)UI_ScreenAnchorX, 6f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), localPlayer.statLife + "/" + localPlayer.statLifeMax2, new Vector2((float)(500 + 13 * num4) + vector.X * 0.5f + (float)UI_ScreenAnchorX, 6f), color, 0f, new Vector2(FontAssets.MouseText.get_Value().MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, SpriteEffects.None, 0f);
			}
			for (int i = 1; i < (int)((float)localPlayer.statLifeMax2 / UIDisplay_LifePerHeart) + 1; i++)
			{
				int num5 = 255;
				float num6 = 1f;
				bool flag = false;
				if ((float)localPlayer.statLife >= (float)i * UIDisplay_LifePerHeart)
				{
					num5 = 255;
					if ((float)localPlayer.statLife == (float)i * UIDisplay_LifePerHeart)
					{
						flag = true;
					}
				}
				else
				{
					float num7 = ((float)localPlayer.statLife - (float)(i - 1) * UIDisplay_LifePerHeart) / UIDisplay_LifePerHeart;
					num5 = (int)(30f + 225f * num7);
					if (num5 < 30)
					{
						num5 = 30;
					}
					num6 = num7 / 4f + 0.75f;
					if ((double)num6 < 0.75)
					{
						num6 = 0.75f;
					}
					if (num7 > 0f)
					{
						flag = true;
					}
				}
				if (flag)
				{
					num6 += Main.cursorScale - 1f;
				}
				int num8 = 0;
				int num9 = 0;
				if (i > 10)
				{
					num8 -= 260;
					num9 += 26;
				}
				int alpha = (int)((double)num5 * 0.9);
				if (!localPlayer.ghost)
				{
					if (num2 > 0)
					{
						num2--;
						spriteBatch.Draw(TextureAssets.Heart2.get_Value(), new Vector2(500 + 26 * (i - 1) + num8 + UI_ScreenAnchorX + TextureAssets.Heart.Width() / 2, 32f + ((float)TextureAssets.Heart.Height() - (float)TextureAssets.Heart.Height() * num6) / 2f + (float)num9 + (float)(TextureAssets.Heart.Height() / 2)), new Rectangle(0, 0, TextureAssets.Heart.Width(), TextureAssets.Heart.Height()), new Color(num5, num5, num5, alpha), 0f, new Vector2(TextureAssets.Heart.Width() / 2, TextureAssets.Heart.Height() / 2), num6, SpriteEffects.None, 0f);
					}
					else
					{
						spriteBatch.Draw(TextureAssets.Heart.get_Value(), new Vector2(500 + 26 * (i - 1) + num8 + UI_ScreenAnchorX + TextureAssets.Heart.Width() / 2, 32f + ((float)TextureAssets.Heart.Height() - (float)TextureAssets.Heart.Height() * num6) / 2f + (float)num9 + (float)(TextureAssets.Heart.Height() / 2)), new Rectangle(0, 0, TextureAssets.Heart.Width(), TextureAssets.Heart.Height()), new Color(num5, num5, num5, alpha), 0f, new Vector2(TextureAssets.Heart.Width() / 2, TextureAssets.Heart.Height() / 2), num6, SpriteEffects.None, 0f);
					}
				}
			}
		}

		private void DrawMana()
		{
			Player localPlayer = Main.LocalPlayer;
			SpriteBatch spriteBatch = Main.spriteBatch;
			Color color = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
			UIDisplay_ManaPerStar = 20;
			if (localPlayer.ghost || localPlayer.statManaMax2 <= 0)
			{
				return;
			}
			_ = localPlayer.statManaMax2 / 20;
			Vector2 vector = FontAssets.MouseText.get_Value().MeasureString(Lang.inter[2].Value);
			int num = 50;
			if (vector.X >= 45f)
			{
				num = (int)vector.X + 5;
			}
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), Lang.inter[2].Value, new Vector2(800 - num + UI_ScreenAnchorX, 6f), color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			for (int i = 1; i < localPlayer.statManaMax2 / UIDisplay_ManaPerStar + 1; i++)
			{
				int num2 = 255;
				bool flag = false;
				float num3 = 1f;
				if (localPlayer.statMana >= i * UIDisplay_ManaPerStar)
				{
					num2 = 255;
					if (localPlayer.statMana == i * UIDisplay_ManaPerStar)
					{
						flag = true;
					}
				}
				else
				{
					float num4 = (float)(localPlayer.statMana - (i - 1) * UIDisplay_ManaPerStar) / (float)UIDisplay_ManaPerStar;
					num2 = (int)(30f + 225f * num4);
					if (num2 < 30)
					{
						num2 = 30;
					}
					num3 = num4 / 4f + 0.75f;
					if ((double)num3 < 0.75)
					{
						num3 = 0.75f;
					}
					if (num4 > 0f)
					{
						flag = true;
					}
				}
				if (flag)
				{
					num3 += Main.cursorScale - 1f;
				}
				int alpha = (int)((double)num2 * 0.9);
				spriteBatch.Draw(TextureAssets.Mana.get_Value(), new Vector2(775 + UI_ScreenAnchorX, (float)(30 + TextureAssets.Mana.Height() / 2) + ((float)TextureAssets.Mana.Height() - (float)TextureAssets.Mana.Height() * num3) / 2f + (float)(28 * (i - 1))), new Rectangle(0, 0, TextureAssets.Mana.Width(), TextureAssets.Mana.Height()), new Color(num2, num2, num2, alpha), 0f, new Vector2(TextureAssets.Mana.Width() / 2, TextureAssets.Mana.Height() / 2), num3, SpriteEffects.None, 0f);
			}
		}

		public void TryToHover()
		{
			Vector2 mouseScreen = Main.MouseScreen;
			Player localPlayer = Main.LocalPlayer;
			int num = 26 * localPlayer.statLifeMax2 / (int)UIDisplay_LifePerHeart;
			int num2 = 0;
			if (localPlayer.statLifeMax2 > 200)
			{
				num = 260;
				num2 += 26;
			}
			if (mouseScreen.X > (float)(500 + UI_ScreenAnchorX) && mouseScreen.X < (float)(500 + num + UI_ScreenAnchorX) && mouseScreen.Y > 32f && mouseScreen.Y < (float)(32 + TextureAssets.Heart.Height() + num2))
			{
				CommonResourceBarMethods.DrawLifeMouseOver();
			}
			num = 24;
			num2 = 28 * localPlayer.statManaMax2 / UIDisplay_ManaPerStar;
			if (mouseScreen.X > (float)(762 + UI_ScreenAnchorX) && mouseScreen.X < (float)(762 + num + UI_ScreenAnchorX) && mouseScreen.Y > 30f && mouseScreen.Y < (float)(30 + num2))
			{
				CommonResourceBarMethods.DrawManaMouseOver();
			}
		}
	}
}
