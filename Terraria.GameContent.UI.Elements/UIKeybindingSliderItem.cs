using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
	public class UIKeybindingSliderItem : UIElement
	{
		private Color _color;

		private Func<string> _TextDisplayFunction;

		private Func<float> _GetStatusFunction;

		private Action<float> _SlideKeyboardAction;

		private Action _SlideGamepadAction;

		private int _sliderIDInPage;

		private Asset<Texture2D> _toggleTexture;

		public UIKeybindingSliderItem(Func<string> getText, Func<float> getStatus, Action<float> setStatusKeyboard, Action setStatusGamepad, int sliderIDInPage, Color color)
		{
			_color = color;
			_toggleTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle", (AssetRequestMode)1);
			_TextDisplayFunction = ((getText != null) ? getText : ((Func<string>)(() => "???")));
			_GetStatusFunction = ((getStatus != null) ? getStatus : ((Func<float>)(() => 0f)));
			_SlideKeyboardAction = ((setStatusKeyboard != null) ? setStatusKeyboard : ((Action<float>)delegate
			{
			}));
			_SlideGamepadAction = ((setStatusGamepad != null) ? setStatusGamepad : ((Action)delegate
			{
			}));
			_sliderIDInPage = sliderIDInPage;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float num = 6f;
			base.DrawSelf(spriteBatch);
			int num2 = 0;
			IngameOptions.rightHover = -1;
			if (!Main.mouseLeft)
			{
				IngameOptions.rightLock = -1;
			}
			if (IngameOptions.rightLock == _sliderIDInPage)
			{
				num2 = 1;
			}
			else if (IngameOptions.rightLock != -1)
			{
				num2 = 2;
			}
			CalculatedStyle dimensions = GetDimensions();
			float num3 = dimensions.Width + 1f;
			Vector2 vector = new Vector2(dimensions.X, dimensions.Y);
			bool flag = base.IsMouseHovering;
			if (num2 == 1)
			{
				flag = true;
			}
			if (num2 == 2)
			{
				flag = false;
			}
			Vector2 baseScale = new Vector2(0.8f);
			Color value = (false ? Color.Gold : (flag ? Color.White : Color.Silver));
			value = Color.Lerp(value, Color.White, flag ? 0.5f : 0f);
			Color color = (flag ? _color : _color.MultiplyRGBA(new Color(180, 180, 180)));
			Vector2 position = vector;
			Utils.DrawSettingsPanel(spriteBatch, position, num3, color);
			position.X += 8f;
			position.Y += 2f + num;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), _TextDisplayFunction(), position, value, 0f, Vector2.Zero, baseScale, num3);
			position.X -= 17f;
			TextureAssets.ColorBar.Frame();
			position = (IngameOptions.valuePosition = new Vector2(dimensions.X + dimensions.Width - 10f, dimensions.Y + 10f + num));
			float obj = IngameOptions.DrawValueBar(spriteBatch, 1f, _GetStatusFunction(), num2);
			if (IngameOptions.inBar || IngameOptions.rightLock == _sliderIDInPage)
			{
				IngameOptions.rightHover = _sliderIDInPage;
				if (PlayerInput.Triggers.Current.MouseLeft && PlayerInput.CurrentProfile.AllowEditting && !PlayerInput.UsingGamepad && IngameOptions.rightLock == _sliderIDInPage)
				{
					_SlideKeyboardAction(obj);
				}
			}
			if (IngameOptions.rightHover != -1 && IngameOptions.rightLock == -1)
			{
				IngameOptions.rightLock = IngameOptions.rightHover;
			}
			if (base.IsMouseHovering && PlayerInput.CurrentProfile.AllowEditting)
			{
				_SlideGamepadAction();
			}
		}
	}
}
