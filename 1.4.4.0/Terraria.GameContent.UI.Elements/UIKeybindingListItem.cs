using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
	public class UIKeybindingListItem : UIElement
	{
		private InputMode _inputmode;

		private Color _color;

		private string _keybind;

		public UIKeybindingListItem(string bind, InputMode mode, Color color)
		{
			_keybind = bind;
			_inputmode = mode;
			_color = color;
			base.OnClick += OnClickMethod;
		}

		public void OnClickMethod(UIMouseEvent evt, UIElement listeningElement)
		{
			if (PlayerInput.ListeningTrigger != _keybind)
			{
				if (PlayerInput.CurrentProfile.AllowEditting)
				{
					PlayerInput.ListenFor(_keybind, _inputmode);
				}
				else
				{
					PlayerInput.ListenFor(null, _inputmode);
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			float num = 6f;
			base.DrawSelf(spriteBatch);
			CalculatedStyle dimensions = GetDimensions();
			float num2 = dimensions.Width + 1f;
			Vector2 vector = new Vector2(dimensions.X, dimensions.Y);
			bool flag = PlayerInput.ListeningTrigger == _keybind;
			Vector2 baseScale = new Vector2(0.8f);
			Color value = (flag ? Color.Gold : (base.IsMouseHovering ? Color.White : Color.Silver));
			value = Color.Lerp(value, Color.White, base.IsMouseHovering ? 0.5f : 0f);
			Color color = (base.IsMouseHovering ? _color : _color.MultiplyRGBA(new Color(180, 180, 180)));
			Vector2 position = vector;
			Utils.DrawSettingsPanel(spriteBatch, position, num2, color);
			position.X += 8f;
			position.Y += 2f + num;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), GetFriendlyName(), position, value, 0f, Vector2.Zero, baseScale, num2);
			position.X -= 17f;
			List<string> list = PlayerInput.CurrentProfile.InputModes[_inputmode].KeyStatus[_keybind];
			string text = GenInput(list);
			if (string.IsNullOrEmpty(text))
			{
				text = Lang.menu[195].Value;
				if (!flag)
				{
					value = new Color(80, 80, 80);
				}
			}
			Vector2 stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.get_Value(), text, baseScale);
			position = new Vector2(dimensions.X + dimensions.Width - stringSize.X - 10f, dimensions.Y + 2f + num);
			if (_inputmode == InputMode.XBoxGamepad || _inputmode == InputMode.XBoxGamepadUI)
			{
				position += new Vector2(0f, -3f);
			}
			GlyphTagHandler.GlyphsScale = 0.85f;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), text, position, value, 0f, Vector2.Zero, baseScale, num2);
			GlyphTagHandler.GlyphsScale = 1f;
		}

		private string GenInput(List<string> list)
		{
			if (list.Count == 0)
			{
				return "";
			}
			string text = "";
			switch (_inputmode)
			{
			case InputMode.XBoxGamepad:
			case InputMode.XBoxGamepadUI:
			{
				text = GlyphTagHandler.GenerateTag(list[0]);
				for (int j = 1; j < list.Count; j++)
				{
					text = text + "/" + GlyphTagHandler.GenerateTag(list[j]);
				}
				break;
			}
			case InputMode.Keyboard:
			case InputMode.KeyboardUI:
			case InputMode.Mouse:
			{
				text = list[0];
				for (int i = 1; i < list.Count; i++)
				{
					text = text + "/" + list[i];
				}
				break;
			}
			}
			return text;
		}

		private string GetFriendlyName()
		{
			return _keybind switch
			{
				"MouseLeft" => Lang.menu[162].Value, 
				"MouseRight" => Lang.menu[163].Value, 
				"Up" => Lang.menu[148].Value, 
				"Down" => Lang.menu[149].Value, 
				"Left" => Lang.menu[150].Value, 
				"Right" => Lang.menu[151].Value, 
				"Jump" => Lang.menu[152].Value, 
				"Throw" => Lang.menu[153].Value, 
				"Inventory" => Lang.menu[154].Value, 
				"Grapple" => Lang.menu[155].Value, 
				"SmartSelect" => Lang.menu[160].Value, 
				"SmartCursor" => Lang.menu[161].Value, 
				"QuickMount" => Lang.menu[158].Value, 
				"QuickHeal" => Lang.menu[159].Value, 
				"QuickMana" => Lang.menu[156].Value, 
				"QuickBuff" => Lang.menu[157].Value, 
				"MapZoomIn" => Lang.menu[168].Value, 
				"MapZoomOut" => Lang.menu[169].Value, 
				"MapAlphaUp" => Lang.menu[171].Value, 
				"MapAlphaDown" => Lang.menu[170].Value, 
				"MapFull" => Lang.menu[173].Value, 
				"MapStyle" => Lang.menu[172].Value, 
				"Hotbar1" => Lang.menu[176].Value, 
				"Hotbar2" => Lang.menu[177].Value, 
				"Hotbar3" => Lang.menu[178].Value, 
				"Hotbar4" => Lang.menu[179].Value, 
				"Hotbar5" => Lang.menu[180].Value, 
				"Hotbar6" => Lang.menu[181].Value, 
				"Hotbar7" => Lang.menu[182].Value, 
				"Hotbar8" => Lang.menu[183].Value, 
				"Hotbar9" => Lang.menu[184].Value, 
				"Hotbar10" => Lang.menu[185].Value, 
				"HotbarMinus" => Lang.menu[174].Value, 
				"HotbarPlus" => Lang.menu[175].Value, 
				"DpadRadial1" => Lang.menu[186].Value, 
				"DpadRadial2" => Lang.menu[187].Value, 
				"DpadRadial3" => Lang.menu[188].Value, 
				"DpadRadial4" => Lang.menu[189].Value, 
				"RadialHotbar" => Lang.menu[190].Value, 
				"RadialQuickbar" => Lang.menu[244].Value, 
				"DpadSnap1" => Lang.menu[191].Value, 
				"DpadSnap2" => Lang.menu[192].Value, 
				"DpadSnap3" => Lang.menu[193].Value, 
				"DpadSnap4" => Lang.menu[194].Value, 
				"LockOn" => Lang.menu[231].Value, 
				"ViewZoomIn" => Language.GetTextValue("UI.ZoomIn"), 
				"ViewZoomOut" => Language.GetTextValue("UI.ZoomOut"), 
				"ToggleCreativeMenu" => Language.GetTextValue("UI.ToggleCreativeMenu"), 
				"Loadout1" => Language.GetTextValue("UI.Loadout1"), 
				"Loadout2" => Language.GetTextValue("UI.Loadout2"), 
				"Loadout3" => Language.GetTextValue("UI.Loadout3"), 
				"ToggleCameraMode" => Language.GetTextValue("UI.ToggleCameraMode"), 
				_ => _keybind, 
			};
		}
	}
}
