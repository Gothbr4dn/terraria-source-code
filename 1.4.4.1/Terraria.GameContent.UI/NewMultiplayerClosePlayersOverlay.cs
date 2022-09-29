using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI
{
	public class NewMultiplayerClosePlayersOverlay : IMultiplayerClosePlayersOverlay
	{
		private struct PlayerOnScreenCache
		{
			private string _name;

			private Vector2 _pos;

			private Color _color;

			public PlayerOnScreenCache(string name, Vector2 pos, Color color)
			{
				_name = name;
				_pos = pos;
				_color = color;
			}

			public void DrawPlayerName_WhenPlayerIsOnScreen(SpriteBatch spriteBatch)
			{
				_pos = _pos.Floor();
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), _name, new Vector2(_pos.X - 2f, _pos.Y), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), _name, new Vector2(_pos.X + 2f, _pos.Y), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), _name, new Vector2(_pos.X, _pos.Y - 2f), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), _name, new Vector2(_pos.X, _pos.Y + 2f), Color.Black, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), _name, _pos, _color, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
		}

		private struct PlayerOffScreenCache
		{
			private Player player;

			private string nameToShow;

			private Vector2 namePlatePos;

			private Color namePlateColor;

			private Vector2 distanceDrawPosition;

			private string distanceString;

			private Vector2 measurement;

			public PlayerOffScreenCache(string name, Vector2 pos, Color color, Vector2 npDistPos, string npDist, Player thePlayer, Vector2 theMeasurement)
			{
				nameToShow = name;
				namePlatePos = pos.Floor();
				namePlateColor = color;
				distanceDrawPosition = npDistPos.Floor();
				distanceString = npDist;
				player = thePlayer;
				measurement = theMeasurement;
			}

			public void DrawPlayerName(SpriteBatch spriteBatch)
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.get_Value(), nameToShow, namePlatePos + new Vector2(0f, -40f), namePlateColor, 0f, Vector2.Zero, Vector2.One);
			}

			public void DrawPlayerHead()
			{
				float num = 20f;
				float num2 = -27f;
				num2 -= (measurement.X - 85f) / 2f;
				Color playerHeadBordersColor = Main.GetPlayerHeadBordersColor(player);
				Vector2 vec = new Vector2(namePlatePos.X, namePlatePos.Y - num);
				vec.X -= 22f + num2;
				vec.Y += 8f;
				vec = vec.Floor();
				Main.MapPlayerRenderer.DrawPlayerHead(Main.Camera, player, vec, 1f, 0.8f, playerHeadBordersColor);
			}

			public void DrawLifeBar()
			{
				Vector2 vector = Main.screenPosition + distanceDrawPosition + new Vector2(26f, 20f);
				if (player.statLife != player.statLifeMax2)
				{
					Main.instance.DrawHealthBar(vector.X, vector.Y, player.statLife, player.statLifeMax2, 1f, 1.25f, noFlip: true);
				}
			}

			public void DrawPlayerDistance(SpriteBatch spriteBatch)
			{
				float num = 0.85f;
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), distanceString, new Vector2(distanceDrawPosition.X - 2f, distanceDrawPosition.Y), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), distanceString, new Vector2(distanceDrawPosition.X + 2f, distanceDrawPosition.Y), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), distanceString, new Vector2(distanceDrawPosition.X, distanceDrawPosition.Y - 2f), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), distanceString, new Vector2(distanceDrawPosition.X, distanceDrawPosition.Y + 2f), Color.Black, 0f, default(Vector2), num, SpriteEffects.None, 0f);
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), distanceString, distanceDrawPosition, namePlateColor, 0f, default(Vector2), num, SpriteEffects.None, 0f);
			}
		}

		private List<PlayerOnScreenCache> _playerOnScreenCache = new List<PlayerOnScreenCache>();

		private List<PlayerOffScreenCache> _playerOffScreenCache = new List<PlayerOffScreenCache>();

		public void Draw()
		{
			int teamNamePlateDistance = Main.teamNamePlateDistance;
			if (teamNamePlateDistance <= 0)
			{
				return;
			}
			_playerOnScreenCache.Clear();
			_playerOffScreenCache.Clear();
			SpriteBatch spriteBatch = Main.spriteBatch;
			PlayerInput.SetZoom_World();
			int screenWidth = Main.screenWidth;
			int screenHeight = Main.screenHeight;
			Vector2 screenPosition = Main.screenPosition;
			PlayerInput.SetZoom_UI();
			int num = teamNamePlateDistance * 8;
			Player[] player = Main.player;
			int myPlayer = Main.myPlayer;
			byte mouseTextColor = Main.mouseTextColor;
			Color[] teamColor = Main.teamColor;
			_ = Main.screenPosition;
			Player player2 = player[myPlayer];
			float num2 = (float)(int)mouseTextColor / 255f;
			if (player2.team == 0)
			{
				return;
			}
			DynamicSpriteFont value = FontAssets.MouseText.get_Value();
			for (int i = 0; i < 255; i++)
			{
				if (i == myPlayer)
				{
					continue;
				}
				Player player3 = player[i];
				if (!player3.active || player3.dead || player3.team != player2.team)
				{
					continue;
				}
				string name = player3.name;
				GetDistance(screenWidth, screenHeight, screenPosition, player2, value, player3, name, out var namePlatePos, out var namePlateDist, out var measurement);
				Color color = new Color((byte)((float)(int)teamColor[player3.team].R * num2), (byte)((float)(int)teamColor[player3.team].G * num2), (byte)((float)(int)teamColor[player3.team].B * num2), mouseTextColor);
				if (namePlateDist > 0f)
				{
					float num3 = player3.Distance(player2.Center);
					if (!(num3 > (float)num))
					{
						float num4 = 20f;
						float num5 = -27f;
						num5 -= (measurement.X - 85f) / 2f;
						string textValue = Language.GetTextValue("GameUI.PlayerDistance", (int)(num3 / 16f * 2f));
						Vector2 npDistPos = value.MeasureString(textValue);
						npDistPos.X = namePlatePos.X - num5;
						npDistPos.Y = namePlatePos.Y + measurement.Y / 2f - npDistPos.Y / 2f - num4;
						_playerOffScreenCache.Add(new PlayerOffScreenCache(name, namePlatePos, color, npDistPos, textValue, player3, measurement));
					}
				}
				else
				{
					_playerOnScreenCache.Add(new PlayerOnScreenCache(name, namePlatePos, color));
				}
			}
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			for (int j = 0; j < _playerOnScreenCache.Count; j++)
			{
				_playerOnScreenCache[j].DrawPlayerName_WhenPlayerIsOnScreen(spriteBatch);
			}
			for (int k = 0; k < _playerOffScreenCache.Count; k++)
			{
				_playerOffScreenCache[k].DrawPlayerName(spriteBatch);
			}
			for (int l = 0; l < _playerOffScreenCache.Count; l++)
			{
				_playerOffScreenCache[l].DrawPlayerDistance(spriteBatch);
			}
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
			for (int m = 0; m < _playerOffScreenCache.Count; m++)
			{
				_playerOffScreenCache[m].DrawLifeBar();
			}
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);
			for (int n = 0; n < _playerOffScreenCache.Count; n++)
			{
				_playerOffScreenCache[n].DrawPlayerHead();
			}
		}

		private static void GetDistance(int testWidth, int testHeight, Vector2 testPosition, Player localPlayer, DynamicSpriteFont font, Player player, string nameToShow, out Vector2 namePlatePos, out float namePlateDist, out Vector2 measurement)
		{
			float uIScale = Main.UIScale;
			SpriteViewMatrix gameViewMatrix = Main.GameViewMatrix;
			namePlatePos = font.MeasureString(nameToShow);
			float num = 0f;
			if (player.chatOverhead.timeLeft > 0)
			{
				num = (0f - namePlatePos.Y) * uIScale;
			}
			else if (player.emoteTime > 0)
			{
				num = (0f - namePlatePos.Y) * uIScale;
			}
			Vector2 vector = new Vector2((float)(testWidth / 2) + testPosition.X, (float)(testHeight / 2) + testPosition.Y);
			Vector2 position = player.position;
			position += (position - vector) * (gameViewMatrix.Zoom - Vector2.One);
			namePlateDist = 0f;
			float num2 = position.X + (float)(player.width / 2) - vector.X;
			float num3 = position.Y - namePlatePos.Y - 2f + num - vector.Y;
			float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
			int num5 = testHeight;
			if (testHeight > testWidth)
			{
				num5 = testWidth;
			}
			num5 = num5 / 2 - 50;
			if (num5 < 100)
			{
				num5 = 100;
			}
			if (num4 < (float)num5)
			{
				namePlatePos.X = position.X + (float)(player.width / 2) - namePlatePos.X / 2f - testPosition.X;
				namePlatePos.Y = position.Y - namePlatePos.Y - 2f + num - testPosition.Y;
			}
			else
			{
				namePlateDist = num4;
				num4 = (float)num5 / num4;
				namePlatePos.X = (float)(testWidth / 2) + num2 * num4 - namePlatePos.X / 2f;
				namePlatePos.Y = (float)(testHeight / 2) + num3 * num4 + 40f * uIScale;
			}
			measurement = font.MeasureString(nameToShow);
			namePlatePos += measurement / 2f;
			namePlatePos *= 1f / uIScale;
			namePlatePos -= measurement / 2f;
			if (localPlayer.gravDir == -1f)
			{
				namePlatePos.Y = (float)testHeight - namePlatePos.Y;
			}
		}
	}
}
