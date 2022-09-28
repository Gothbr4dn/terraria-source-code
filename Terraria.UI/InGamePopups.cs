using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Achievements;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Social.Base;

namespace Terraria.UI
{
	public class InGamePopups
	{
		public class AchievementUnlockedPopup : IInGameNotification
		{
			private Achievement _theAchievement;

			private Asset<Texture2D> _achievementTexture;

			private Asset<Texture2D> _achievementBorderTexture;

			private const int _iconSize = 64;

			private const int _iconSizeWithSpace = 66;

			private const int _iconsPerRow = 8;

			private int _iconIndex;

			private Rectangle _achievementIconFrame;

			private string _title;

			private int _ingameDisplayTimeLeft;

			public bool ShouldBeRemoved { get; private set; }

			public object CreationObject { get; private set; }

			private float Scale
			{
				get
				{
					if (_ingameDisplayTimeLeft < 30)
					{
						return MathHelper.Lerp(0f, 1f, (float)_ingameDisplayTimeLeft / 30f);
					}
					if (_ingameDisplayTimeLeft > 285)
					{
						return MathHelper.Lerp(1f, 0f, ((float)_ingameDisplayTimeLeft - 285f) / 15f);
					}
					return 1f;
				}
			}

			private float Opacity
			{
				get
				{
					float scale = Scale;
					if (scale <= 0.5f)
					{
						return 0f;
					}
					return (scale - 0.5f) / 0.5f;
				}
			}

			public AchievementUnlockedPopup(Achievement achievement)
			{
				CreationObject = achievement;
				_ingameDisplayTimeLeft = 300;
				_theAchievement = achievement;
				_title = achievement.FriendlyName.Value;
				int num = (_iconIndex = Main.Achievements.GetIconIndex(achievement.Name));
				_achievementIconFrame = new Rectangle(num % 8 * 66, num / 8 * 66, 64, 64);
				_achievementTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievements", (AssetRequestMode)2);
				_achievementBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", (AssetRequestMode)2);
			}

			public void Update()
			{
				_ingameDisplayTimeLeft--;
				if (_ingameDisplayTimeLeft < 0)
				{
					_ingameDisplayTimeLeft = 0;
				}
			}

			public void PushAnchor(ref Vector2 anchorPosition)
			{
				float num = 50f * Opacity;
				anchorPosition.Y -= num;
			}

			public void DrawInGame(SpriteBatch sb, Vector2 bottomAnchorPosition)
			{
				float opacity = Opacity;
				if (opacity > 0f)
				{
					float num = Scale * 1.1f;
					Vector2 size = (FontAssets.ItemStack.get_Value().MeasureString(_title) + new Vector2(58f, 10f)) * num;
					Rectangle r = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - size.Y) * 0.5f), size);
					Vector2 mouseScreen = Main.MouseScreen;
					bool num2 = r.Contains(mouseScreen.ToPoint());
					Utils.DrawInvBG(c: num2 ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f), sb: sb, R: r);
					float num3 = num * 0.3f;
					Vector2 vector = r.Right() - Vector2.UnitX * num * (12f + num3 * (float)_achievementIconFrame.Width);
					sb.Draw(_achievementTexture.get_Value(), vector, _achievementIconFrame, Color.White * opacity, 0f, new Vector2(0f, _achievementIconFrame.Height / 2), num3, SpriteEffects.None, 0f);
					sb.Draw(_achievementBorderTexture.get_Value(), vector, null, Color.White * opacity, 0f, new Vector2(0f, _achievementIconFrame.Height / 2), num3, SpriteEffects.None, 0f);
					Utils.DrawBorderString(color: new Color(Main.mouseTextColor, Main.mouseTextColor, (int)Main.mouseTextColor / 5, Main.mouseTextColor) * opacity, sb: sb, text: _title, pos: vector - Vector2.UnitX * 10f, scale: num * 0.9f, anchorx: 1f, anchory: 0.4f);
					if (num2)
					{
						OnMouseOver();
					}
				}
			}

			private void OnMouseOver()
			{
				if (!PlayerInput.IgnoreMouseInterface)
				{
					Main.player[Main.myPlayer].mouseInterface = true;
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						Main.mouseLeftRelease = false;
						IngameFancyUI.OpenAchievementsAndGoto(_theAchievement);
						_ingameDisplayTimeLeft = 0;
						ShouldBeRemoved = true;
					}
				}
			}

			public void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse)
			{
				Utils.DrawInvBG(spriteBatch, area, Color.Red);
			}
		}

		public class PlayerWantsToJoinGamePopup : IInGameNotification
		{
			private int _timeLeft;

			private const int _timeLeftMax = 1800;

			private string _displayTextWithoutTime;

			private UserJoinToServerRequest _request;

			private float Scale
			{
				get
				{
					if (_timeLeft < 30)
					{
						return MathHelper.Lerp(0f, 1f, (float)_timeLeft / 30f);
					}
					if (_timeLeft > 1785)
					{
						return MathHelper.Lerp(1f, 0f, ((float)_timeLeft - 1785f) / 15f);
					}
					return 1f;
				}
			}

			private float Opacity
			{
				get
				{
					float scale = Scale;
					if (scale <= 0.5f)
					{
						return 0f;
					}
					return (scale - 0.5f) / 0.5f;
				}
			}

			public object CreationObject { get; private set; }

			public bool ShouldBeRemoved => _timeLeft <= 0;

			public PlayerWantsToJoinGamePopup(UserJoinToServerRequest request)
			{
				_request = request;
				CreationObject = request;
				_timeLeft = 1800;
				switch (Main.rand.Next(5))
				{
				default:
					_displayTextWithoutTime = "This Bloke Wants to Join you";
					break;
				case 1:
					_displayTextWithoutTime = "This Fucker Wants to Join you";
					break;
				case 2:
					_displayTextWithoutTime = "This Weirdo Wants to Join you";
					break;
				case 3:
					_displayTextWithoutTime = "This Great Gal Wants to Join you";
					break;
				case 4:
					_displayTextWithoutTime = "The one guy who beat you up 30 years ago Wants to Join you";
					break;
				}
			}

			public void Update()
			{
				_timeLeft--;
			}

			public void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition)
			{
				float opacity = Opacity;
				if (opacity > 0f)
				{
					string text = Utils.FormatWith(_request.GetUserWrapperText(), new
					{
						DisplayName = _request.UserDisplayName,
						FullId = _request.UserFullIdentifier
					});
					float num = Scale * 1.1f;
					Vector2 size = (FontAssets.ItemStack.get_Value().MeasureString(text) + new Vector2(58f, 10f)) * num;
					Rectangle r = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - size.Y) * 0.5f), size);
					Vector2 mouseScreen = Main.MouseScreen;
					Color c = (r.Contains(mouseScreen.ToPoint()) ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f));
					Utils.DrawInvBG(spriteBatch, r, c);
					Vector2 vector = new Vector2(r.Left, r.Center.Y);
					vector.X += 32f;
					Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", (AssetRequestMode)1).get_Value();
					Vector2 vector2 = new Vector2(r.Left + 7, MathHelper.Lerp(r.Top, r.Bottom, 0.5f) - (float)(value.Height / 2) - 1f);
					bool flag = Utils.CenteredRectangle(vector2 + new Vector2(value.Width / 2, 0f), value.Size()).Contains(mouseScreen.ToPoint());
					spriteBatch.Draw(value, vector2, null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, SpriteEffects.None, 0f);
					if (flag)
					{
						OnMouseOver();
					}
					value = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", (AssetRequestMode)1).get_Value();
					vector2 = new Vector2(r.Left + 7, MathHelper.Lerp(r.Top, r.Bottom, 0.5f) + (float)(value.Height / 2) + 1f);
					flag = Utils.CenteredRectangle(vector2 + new Vector2(value.Width / 2, 0f), value.Size()).Contains(mouseScreen.ToPoint());
					spriteBatch.Draw(value, vector2, null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, SpriteEffects.None, 0f);
					if (flag)
					{
						OnMouseOver(reject: true);
					}
					Utils.DrawBorderString(color: new Color(Main.mouseTextColor, Main.mouseTextColor, (int)Main.mouseTextColor / 5, Main.mouseTextColor) * opacity, sb: spriteBatch, text: text, pos: r.Center.ToVector2() + new Vector2(10f, 0f), scale: num * 0.9f, anchorx: 0.5f, anchory: 0.4f);
				}
			}

			private void OnMouseOver(bool reject = false)
			{
				if (PlayerInput.IgnoreMouseInterface)
				{
					return;
				}
				Main.player[Main.myPlayer].mouseInterface = true;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Main.mouseLeftRelease = false;
					_timeLeft = 0;
					if (reject)
					{
						_request.Reject();
					}
					else
					{
						_request.Accept();
					}
				}
			}

			public void PushAnchor(ref Vector2 positionAnchorBottom)
			{
				float num = 70f * Opacity;
				positionAnchorBottom.Y -= num;
			}

			public void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse)
			{
				string userWrapperText = _request.GetUserWrapperText();
				string text = _request.UserDisplayName;
				Utils.TrimTextIfNeeded(ref text, FontAssets.MouseText.get_Value(), 0.9f, area.Width / 4);
				string text2 = Utils.FormatWith(userWrapperText, new
				{
					DisplayName = text,
					FullId = _request.UserFullIdentifier
				});
				Vector2 mouseScreen = Main.MouseScreen;
				Color c = (area.Contains(mouseScreen.ToPoint()) ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f));
				Utils.DrawInvBG(spriteBatch, area, c);
				Vector2 pos = new Vector2(area.Left, area.Center.Y);
				pos.X += 32f;
				Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", (AssetRequestMode)1).get_Value();
				Vector2 vector = new Vector2(area.Left + 7, MathHelper.Lerp(area.Top, area.Bottom, 0.5f) - (float)(value.Height / 2) - 1f);
				bool flag = Utils.CenteredRectangle(vector + new Vector2(value.Width / 2, 0f), value.Size()).Contains(mouseScreen.ToPoint());
				spriteBatch.Draw(value, vector, null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, SpriteEffects.None, 0f);
				if (flag)
				{
					OnMouseOver();
				}
				value = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete", (AssetRequestMode)1).get_Value();
				vector = new Vector2(area.Left + 7, MathHelper.Lerp(area.Top, area.Bottom, 0.5f) + (float)(value.Height / 2) + 1f);
				flag = Utils.CenteredRectangle(vector + new Vector2(value.Width / 2, 0f), value.Size()).Contains(mouseScreen.ToPoint());
				spriteBatch.Draw(value, vector, null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, SpriteEffects.None, 0f);
				if (flag)
				{
					OnMouseOver(reject: true);
				}
				pos.X += 6f;
				Utils.DrawBorderString(color: new Color(Main.mouseTextColor, Main.mouseTextColor, (int)Main.mouseTextColor / 5, Main.mouseTextColor), sb: spriteBatch, text: text2, pos: pos, scale: 0.9f, anchorx: 0f, anchory: 0.4f);
			}
		}
	}
}
