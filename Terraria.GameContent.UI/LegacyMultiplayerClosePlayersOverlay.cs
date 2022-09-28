using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI
{
	public class LegacyMultiplayerClosePlayersOverlay : IMultiplayerClosePlayersOverlay
	{
		public void Draw()
		{
			int teamNamePlateDistance = Main.teamNamePlateDistance;
			if (teamNamePlateDistance <= 0)
			{
				return;
			}
			SpriteBatch spriteBatch = Main.spriteBatch;
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
			PlayerInput.SetZoom_World();
			int screenWidth = Main.screenWidth;
			int screenHeight = Main.screenHeight;
			Vector2 screenPosition = Main.screenPosition;
			PlayerInput.SetZoom_UI();
			float uIScale = Main.UIScale;
			int num = teamNamePlateDistance * 8;
			Player[] player = Main.player;
			int myPlayer = Main.myPlayer;
			SpriteViewMatrix gameViewMatrix = Main.GameViewMatrix;
			byte mouseTextColor = Main.mouseTextColor;
			Color[] teamColor = Main.teamColor;
			Camera camera = Main.Camera;
			IPlayerRenderer playerRenderer = Main.PlayerRenderer;
			Vector2 screenPosition2 = Main.screenPosition;
			for (int i = 0; i < 255; i++)
			{
				if (!player[i].active || myPlayer == i || player[i].dead || player[myPlayer].team <= 0 || player[myPlayer].team != player[i].team)
				{
					continue;
				}
				string name = player[i].name;
				Vector2 namePlatePos = FontAssets.MouseText.get_Value().MeasureString(name);
				float num2 = 0f;
				if (player[i].chatOverhead.timeLeft > 0)
				{
					num2 = (0f - namePlatePos.Y) * uIScale;
				}
				else if (player[i].emoteTime > 0)
				{
					num2 = (0f - namePlatePos.Y) * uIScale;
				}
				Vector2 vector = new Vector2((float)(screenWidth / 2) + screenPosition.X, (float)(screenHeight / 2) + screenPosition.Y);
				Vector2 position = player[i].position;
				position += (position - vector) * (gameViewMatrix.Zoom - Vector2.One);
				float num3 = 0f;
				float num4 = (float)(int)mouseTextColor / 255f;
				Color namePlateColor = new Color((byte)((float)(int)teamColor[player[i].team].R * num4), (byte)((float)(int)teamColor[player[i].team].G * num4), (byte)((float)(int)teamColor[player[i].team].B * num4), mouseTextColor);
				float num5 = position.X + (float)(player[i].width / 2) - vector.X;
				float num6 = position.Y - namePlatePos.Y - 2f + num2 - vector.Y;
				float num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
				int num8 = screenHeight;
				if (screenHeight > screenWidth)
				{
					num8 = screenWidth;
				}
				num8 = num8 / 2 - 50;
				if (num8 < 100)
				{
					num8 = 100;
				}
				if (num7 < (float)num8)
				{
					namePlatePos.X = position.X + (float)(player[i].width / 2) - namePlatePos.X / 2f - screenPosition.X;
					namePlatePos.Y = position.Y - namePlatePos.Y - 2f + num2 - screenPosition.Y;
				}
				else
				{
					num3 = num7;
					num7 = (float)num8 / num7;
					namePlatePos.X = (float)(screenWidth / 2) + num5 * num7 - namePlatePos.X / 2f;
					namePlatePos.Y = (float)(screenHeight / 2) + num6 * num7 + 40f * uIScale;
				}
				Vector2 vector2 = FontAssets.MouseText.get_Value().MeasureString(name);
				namePlatePos += vector2 / 2f;
				namePlatePos *= 1f / uIScale;
				namePlatePos -= vector2 / 2f;
				if (player[myPlayer].gravDir == -1f)
				{
					namePlatePos.Y = (float)screenHeight - namePlatePos.Y;
				}
				if (num3 > 0f)
				{
					float num9 = 20f;
					float num10 = -27f;
					num10 -= (vector2.X - 85f) / 2f;
					num5 = player[i].Center.X - player[myPlayer].Center.X;
					num6 = player[i].Center.Y - player[myPlayer].Center.Y;
					float num11 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
					if (!(num11 > (float)num))
					{
						string textValue = Language.GetTextValue("GameUI.PlayerDistance", (int)(num11 / 16f * 2f));
						Vector2 npDistPos = FontAssets.MouseText.get_Value().MeasureString(textValue);
						npDistPos.X = namePlatePos.X - num10;
						npDistPos.Y = namePlatePos.Y + vector2.Y / 2f - npDistPos.Y / 2f - num9;
						DrawPlayerName2(spriteBatch, ref namePlateColor, textValue, ref npDistPos);
						Color playerHeadBordersColor = Main.GetPlayerHeadBordersColor(player[i]);
						Vector2 position2 = new Vector2(namePlatePos.X, namePlatePos.Y - num9);
						position2.X -= 22f + num10;
						position2.Y += 8f;
						playerRenderer.DrawPlayerHead(camera, player[i], position2, 1f, 0.8f, playerHeadBordersColor);
						Vector2 vector3 = npDistPos + screenPosition2 + new Vector2(26f, 20f);
						if (player[i].statLife != player[i].statLifeMax2)
						{
							Main.instance.DrawHealthBar(vector3.X, vector3.Y, player[i].statLife, player[i].statLifeMax2, 1f, 1.25f, noFlip: true);
						}
						ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.get_Value(), name, namePlatePos + new Vector2(0f, -40f), namePlateColor, 0f, Vector2.Zero, Vector2.One);
					}
				}
				else
				{
					DrawPlayerName(spriteBatch, name, ref namePlatePos, ref namePlateColor);
				}
			}
		}

		private static void DrawPlayerName2(SpriteBatch spriteBatch, ref Color namePlateColor, string npDist, ref Vector2 npDistPos)
		{
			float num = 0.85f;
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), npDist, new Vector2(npDistPos.X - 2f, npDistPos.Y), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), npDist, new Vector2(npDistPos.X + 2f, npDistPos.Y), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), npDist, new Vector2(npDistPos.X, npDistPos.Y - 2f), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), npDist, new Vector2(npDistPos.X, npDistPos.Y + 2f), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), npDist, npDistPos, namePlateColor, 0f, default(Vector2), num, SpriteEffects.None, 0f);
		}

		private static void DrawPlayerName(SpriteBatch spriteBatch, string namePlate, ref Vector2 namePlatePos, ref Color namePlateColor)
		{
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), namePlate, new Vector2(namePlatePos.X - 2f, namePlatePos.Y), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), namePlate, new Vector2(namePlatePos.X + 2f, namePlatePos.Y), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), namePlate, new Vector2(namePlatePos.X, namePlatePos.Y - 2f), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), namePlate, new Vector2(namePlatePos.X, namePlatePos.Y + 2f), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), namePlate, namePlatePos, namePlateColor, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
		}
	}
}
