using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Achievements;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
	public class UIAchievementListItem : UIPanel
	{
		private Achievement _achievement;

		private UIImageFramed _achievementIcon;

		private UIImage _achievementIconBorders;

		private const int _iconSize = 64;

		private const int _iconSizeWithSpace = 66;

		private const int _iconsPerRow = 8;

		private int _iconIndex;

		private Rectangle _iconFrame;

		private Rectangle _iconFrameUnlocked;

		private Rectangle _iconFrameLocked;

		private Asset<Texture2D> _innerPanelTopTexture;

		private Asset<Texture2D> _innerPanelBottomTexture;

		private Asset<Texture2D> _categoryTexture;

		private bool _locked;

		private bool _large;

		public UIAchievementListItem(Achievement achievement, bool largeForOtherLanguages)
		{
			_large = largeForOtherLanguages;
			BackgroundColor = new Color(26, 40, 89) * 0.8f;
			BorderColor = new Color(13, 20, 44) * 0.8f;
			float num = 16 + _large.ToInt() * 20;
			float num2 = _large.ToInt() * 6;
			float num3 = _large.ToInt() * 12;
			_achievement = achievement;
			Height.Set(66f + num, 0f);
			Width.Set(0f, 1f);
			PaddingTop = 8f;
			PaddingLeft = 9f;
			int num4 = (_iconIndex = Main.Achievements.GetIconIndex(achievement.Name));
			_iconFrameUnlocked = new Rectangle(num4 % 8 * 66, num4 / 8 * 66, 64, 64);
			_iconFrameLocked = _iconFrameUnlocked;
			_iconFrameLocked.X += 528;
			_iconFrame = _iconFrameLocked;
			UpdateIconFrame();
			_achievementIcon = new UIImageFramed(Main.Assets.Request<Texture2D>("Images/UI/Achievements", (AssetRequestMode)1), _iconFrame);
			_achievementIcon.Left.Set(num2, 0f);
			_achievementIcon.Top.Set(num3, 0f);
			Append(_achievementIcon);
			_achievementIconBorders = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", (AssetRequestMode)1));
			_achievementIconBorders.Left.Set(-4f + num2, 0f);
			_achievementIconBorders.Top.Set(-4f + num3, 0f);
			Append(_achievementIconBorders);
			_innerPanelTopTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_InnerPanelTop", (AssetRequestMode)1);
			if (_large)
			{
				_innerPanelBottomTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_InnerPanelBottom_Large", (AssetRequestMode)1);
			}
			else
			{
				_innerPanelBottomTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_InnerPanelBottom", (AssetRequestMode)1);
			}
			_categoryTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Categories", (AssetRequestMode)1);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			int num = _large.ToInt() * 6;
			Vector2 vector = new Vector2(num, 0f);
			_locked = !_achievement.IsCompleted;
			UpdateIconFrame();
			CalculatedStyle innerDimensions = GetInnerDimensions();
			CalculatedStyle dimensions = _achievementIconBorders.GetDimensions();
			float num2 = dimensions.X + dimensions.Width;
			Vector2 vector2 = new Vector2(num2 + 7f, innerDimensions.Y);
			Tuple<decimal, decimal> trackerValues = GetTrackerValues();
			bool flag = false;
			if ((!(trackerValues.Item1 == 0m) || !(trackerValues.Item2 == 0m)) && _locked)
			{
				flag = true;
			}
			float num3 = innerDimensions.Width - dimensions.Width + 1f - (float)(num * 2);
			Vector2 baseScale = new Vector2(0.85f);
			Vector2 baseScale2 = new Vector2(0.92f);
			string text = FontAssets.ItemStack.get_Value().CreateWrappedText(_achievement.Description.Value, (num3 - 20f) * (1f / baseScale2.X), Language.ActiveCulture.CultureInfo);
			Vector2 stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.get_Value(), text, baseScale2, num3);
			if (!_large)
			{
				stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.get_Value(), _achievement.Description.Value, baseScale2, num3);
			}
			float num4 = 38f + (float)(_large ? 20 : 0);
			if (stringSize.Y > num4)
			{
				baseScale2.Y *= num4 / stringSize.Y;
			}
			Color value = (_locked ? Color.Silver : Color.Gold);
			value = Color.Lerp(value, Color.White, base.IsMouseHovering ? 0.5f : 0f);
			Color value2 = (_locked ? Color.DarkGray : Color.Silver);
			value2 = Color.Lerp(value2, Color.White, base.IsMouseHovering ? 1f : 0f);
			Color color = (base.IsMouseHovering ? Color.White : Color.Gray);
			Vector2 vector3 = vector2 - Vector2.UnitY * 2f + vector;
			DrawPanelTop(spriteBatch, vector3, num3, color);
			AchievementCategory category = _achievement.Category;
			vector3.Y += 2f;
			vector3.X += 4f;
			spriteBatch.Draw(_categoryTexture.get_Value(), vector3, _categoryTexture.Frame(4, 2, (int)category), base.IsMouseHovering ? Color.White : Color.Silver, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
			vector3.X += 4f;
			vector3.X += 17f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), _achievement.FriendlyName.Value, vector3, value, 0f, Vector2.Zero, baseScale, num3);
			vector3.X -= 17f;
			Vector2 position = vector2 + Vector2.UnitY * 27f + vector;
			DrawPanelBottom(spriteBatch, position, num3, color);
			position.X += 8f;
			position.Y += 4f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), text, position, value2, 0f, Vector2.Zero, baseScale2);
			if (flag)
			{
				Vector2 vector4 = vector3 + Vector2.UnitX * num3 + Vector2.UnitY;
				string text2 = (int)trackerValues.Item1 + "/" + (int)trackerValues.Item2;
				Vector2 baseScale3 = new Vector2(0.75f);
				Vector2 stringSize2 = ChatManager.GetStringSize(FontAssets.ItemStack.get_Value(), text2, baseScale3);
				float progress = (float)(trackerValues.Item1 / trackerValues.Item2);
				float num5 = 80f;
				Color color2 = new Color(100, 255, 100);
				if (!base.IsMouseHovering)
				{
					color2 = Color.Lerp(color2, Color.Black, 0.25f);
				}
				Color color3 = new Color(255, 255, 255);
				if (!base.IsMouseHovering)
				{
					color3 = Color.Lerp(color3, Color.Black, 0.25f);
				}
				DrawProgressBar(spriteBatch, progress, vector4 - Vector2.UnitX * num5 * 0.7f, num5, color3, color2, color2.MultiplyRGBA(new Color(new Vector4(1f, 1f, 1f, 0.5f))));
				vector4.X -= num5 * 1.4f + stringSize2.X;
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), text2, vector4, value, 0f, new Vector2(0f, 0f), baseScale3, 90f);
			}
		}

		private void UpdateIconFrame()
		{
			if (!_locked)
			{
				_iconFrame = _iconFrameUnlocked;
			}
			else
			{
				_iconFrame = _iconFrameLocked;
			}
			if (_achievementIcon != null)
			{
				_achievementIcon.SetFrame(_iconFrame);
			}
		}

		private void DrawPanelTop(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
		{
			spriteBatch.Draw(_innerPanelTopTexture.get_Value(), position, new Rectangle(0, 0, 2, _innerPanelTopTexture.Height()), color);
			spriteBatch.Draw(_innerPanelTopTexture.get_Value(), new Vector2(position.X + 2f, position.Y), new Rectangle(2, 0, 2, _innerPanelTopTexture.Height()), color, 0f, Vector2.Zero, new Vector2((width - 4f) / 2f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(_innerPanelTopTexture.get_Value(), new Vector2(position.X + width - 2f, position.Y), new Rectangle(4, 0, 2, _innerPanelTopTexture.Height()), color);
		}

		private void DrawPanelBottom(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
		{
			spriteBatch.Draw(_innerPanelBottomTexture.get_Value(), position, new Rectangle(0, 0, 6, _innerPanelBottomTexture.Height()), color);
			spriteBatch.Draw(_innerPanelBottomTexture.get_Value(), new Vector2(position.X + 6f, position.Y), new Rectangle(6, 0, 7, _innerPanelBottomTexture.Height()), color, 0f, Vector2.Zero, new Vector2((width - 12f) / 7f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(_innerPanelBottomTexture.get_Value(), new Vector2(position.X + width - 6f, position.Y), new Rectangle(13, 0, 6, _innerPanelBottomTexture.Height()), color);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			BackgroundColor = new Color(46, 60, 119);
			BorderColor = new Color(20, 30, 56);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			BackgroundColor = new Color(26, 40, 89) * 0.8f;
			BorderColor = new Color(13, 20, 44) * 0.8f;
		}

		public Achievement GetAchievement()
		{
			return _achievement;
		}

		private Tuple<decimal, decimal> GetTrackerValues()
		{
			if (!_achievement.HasTracker)
			{
				return Tuple.Create(0m, 0m);
			}
			IAchievementTracker tracker = _achievement.GetTracker();
			if (tracker.GetTrackerType() == TrackerType.Int)
			{
				AchievementTracker<int> achievementTracker = (AchievementTracker<int>)tracker;
				return Tuple.Create((decimal)achievementTracker.Value, (decimal)achievementTracker.MaxValue);
			}
			if (tracker.GetTrackerType() == TrackerType.Float)
			{
				AchievementTracker<float> achievementTracker2 = (AchievementTracker<float>)tracker;
				return Tuple.Create((decimal)achievementTracker2.Value, (decimal)achievementTracker2.MaxValue);
			}
			return Tuple.Create(0m, 0m);
		}

		private void DrawProgressBar(SpriteBatch spriteBatch, float progress, Vector2 spot, float Width = 169f, Color BackColor = default(Color), Color FillingColor = default(Color), Color BlipColor = default(Color))
		{
			if (BlipColor == Color.Transparent)
			{
				BlipColor = new Color(255, 165, 0, 127);
			}
			if (FillingColor == Color.Transparent)
			{
				FillingColor = new Color(255, 241, 51);
			}
			if (BackColor == Color.Transparent)
			{
				FillingColor = new Color(255, 255, 255);
			}
			Texture2D value = TextureAssets.ColorBar.get_Value();
			TextureAssets.ColorBlip.get_Value();
			Texture2D value2 = TextureAssets.MagicPixel.get_Value();
			float num = MathHelper.Clamp(progress, 0f, 1f);
			float num2 = Width * 1f;
			float num3 = 8f;
			float num4 = num2 / 169f;
			Vector2 position = spot + Vector2.UnitY * num3 + Vector2.UnitX * 1f;
			spriteBatch.Draw(value, spot, new Rectangle(5, 0, value.Width - 9, value.Height), BackColor, 0f, new Vector2(84.5f, 0f), new Vector2(num4, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(value, spot + new Vector2((0f - num4) * 84.5f - 5f, 0f), new Rectangle(0, 0, 5, value.Height), BackColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
			spriteBatch.Draw(value, spot + new Vector2(num4 * 84.5f, 0f), new Rectangle(value.Width - 4, 0, 4, value.Height), BackColor, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
			position += Vector2.UnitX * (num - 0.5f) * num2;
			position.X -= 1f;
			spriteBatch.Draw(value2, position, new Rectangle(0, 0, 1, 1), FillingColor, 0f, new Vector2(1f, 0.5f), new Vector2(num2 * num, num3), SpriteEffects.None, 0f);
			if (progress != 0f)
			{
				spriteBatch.Draw(value2, position, new Rectangle(0, 0, 1, 1), BlipColor, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num3), SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(value2, position, new Rectangle(0, 0, 1, 1), Color.Black, 0f, new Vector2(0f, 0.5f), new Vector2(num2 * (1f - num), num3), SpriteEffects.None, 0f);
		}

		public override int CompareTo(object obj)
		{
			if (!(obj is UIAchievementListItem uIAchievementListItem))
			{
				return 0;
			}
			if (_achievement.IsCompleted && !uIAchievementListItem._achievement.IsCompleted)
			{
				return -1;
			}
			if (!_achievement.IsCompleted && uIAchievementListItem._achievement.IsCompleted)
			{
				return 1;
			}
			int id = _achievement.Id;
			return id.CompareTo(uIAchievementListItem._achievement.Id);
		}
	}
}
