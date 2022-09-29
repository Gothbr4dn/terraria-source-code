using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;

namespace Terraria.GameContent.UI.Elements
{
	public class UIVerticalSlider : UISliderBase
	{
		public float FillPercent;

		public Color FilledColor = Main.OurFavoriteColor;

		public Color EmptyColor = Color.Black;

		private Func<float> _getSliderValue;

		private Action<float> _slideKeyboardAction;

		private Func<float, Color> _blipFunc;

		private Action _slideGamepadAction;

		private bool _isReallyMouseOvered;

		private bool _soundedUsage;

		private bool _alreadyHovered;

		public UIVerticalSlider(Func<float> getStatus, Action<float> setStatusKeyboard, Action setStatusGamepad, Color color)
		{
			_getSliderValue = ((getStatus != null) ? getStatus : ((Func<float>)(() => 0f)));
			_slideKeyboardAction = ((setStatusKeyboard != null) ? setStatusKeyboard : ((Action<float>)delegate
			{
			}));
			_slideGamepadAction = setStatusGamepad;
			_isReallyMouseOvered = false;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			UISliderBase.CurrentAimedSlider = null;
			if (!Main.mouseLeft)
			{
				UISliderBase.CurrentLockedSlider = null;
			}
			GetUsageLevel();
			FillPercent = _getSliderValue();
			float sliderValueThatWasSet = FillPercent;
			bool flag = false;
			if (DrawValueBarDynamicWidth(spriteBatch, out sliderValueThatWasSet))
			{
				flag = true;
			}
			if (UISliderBase.CurrentLockedSlider == this || flag)
			{
				UISliderBase.CurrentAimedSlider = this;
				if (PlayerInput.Triggers.Current.MouseLeft && !PlayerInput.UsingGamepad && UISliderBase.CurrentLockedSlider == this)
				{
					_slideKeyboardAction(sliderValueThatWasSet);
					if (!_soundedUsage)
					{
						SoundEngine.PlaySound(12);
					}
					_soundedUsage = true;
				}
				else
				{
					_soundedUsage = false;
				}
			}
			if (UISliderBase.CurrentAimedSlider != null && UISliderBase.CurrentLockedSlider == null)
			{
				UISliderBase.CurrentLockedSlider = UISliderBase.CurrentAimedSlider;
			}
			if (_isReallyMouseOvered)
			{
				_slideGamepadAction();
			}
		}

		private bool DrawValueBarDynamicWidth(SpriteBatch spriteBatch, out float sliderValueThatWasSet)
		{
			sliderValueThatWasSet = 0f;
			Texture2D value = TextureAssets.ColorBar.get_Value();
			Rectangle rectangle = GetDimensions().ToRectangle();
			Rectangle rectangle2 = new Rectangle(5, 4, 4, 4);
			Utils.DrawSplicedPanel(spriteBatch, value, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, rectangle2.X, rectangle2.Width, rectangle2.Y, rectangle2.Height, Color.White);
			Rectangle rectangle3 = rectangle;
			rectangle3.X += rectangle2.Left;
			rectangle3.Width -= rectangle2.Right;
			rectangle3.Y += rectangle2.Top;
			rectangle3.Height -= rectangle2.Bottom;
			Texture2D value2 = TextureAssets.MagicPixel.get_Value();
			Rectangle value3 = new Rectangle(0, 0, 1, 1);
			spriteBatch.Draw(value2, rectangle3, value3, EmptyColor);
			Rectangle destinationRectangle = rectangle3;
			destinationRectangle.Height = (int)((float)destinationRectangle.Height * FillPercent);
			destinationRectangle.Y += rectangle3.Height - destinationRectangle.Height;
			spriteBatch.Draw(value2, destinationRectangle, value3, FilledColor);
			Vector2 center = new Vector2(destinationRectangle.Center.X + 1, destinationRectangle.Top);
			Vector2 size = new Vector2(destinationRectangle.Width + 16, 4f);
			Rectangle rectangle4 = Utils.CenteredRectangle(center, size);
			Rectangle destinationRectangle2 = rectangle4;
			destinationRectangle2.Inflate(2, 2);
			spriteBatch.Draw(value2, destinationRectangle2, value3, Color.Black);
			spriteBatch.Draw(value2, rectangle4, value3, Color.White);
			Rectangle rectangle5 = rectangle3;
			rectangle5.Inflate(4, 0);
			bool flag = (_isReallyMouseOvered = rectangle5.Contains(Main.MouseScreen.ToPoint()));
			if (IgnoresMouseInteraction)
			{
				flag = false;
			}
			int usageLevel = GetUsageLevel();
			if (usageLevel == 2)
			{
				flag = false;
			}
			if (usageLevel == 1)
			{
				flag = true;
			}
			if (flag || usageLevel == 1)
			{
				if (!_alreadyHovered)
				{
					SoundEngine.PlaySound(12);
				}
				_alreadyHovered = true;
			}
			else
			{
				_alreadyHovered = false;
			}
			if (flag)
			{
				sliderValueThatWasSet = Utils.GetLerpValue(rectangle3.Bottom, rectangle3.Top, Main.mouseY, clamped: true);
				return true;
			}
			return false;
		}
	}
}
