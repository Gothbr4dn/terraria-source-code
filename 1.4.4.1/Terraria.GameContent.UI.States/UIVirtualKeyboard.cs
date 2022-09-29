using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	public class UIVirtualKeyboard : UIState
	{
		public delegate void KeyboardSubmitEvent(string text);

		public enum KeyState
		{
			Default,
			Symbol,
			Shift
		}

		private static UIVirtualKeyboard _currentInstance;

		private static string _cancelCacheSign = "";

		private static string _cancelCacheChest = "";

		private const string DEFAULT_KEYS = "1234567890qwertyuiopasdfghjkl'zxcvbnm,.?";

		private const string SHIFT_KEYS = "1234567890QWERTYUIOPASDFGHJKL'ZXCVBNM,.?";

		private const string SYMBOL_KEYS = "1234567890!@#$%^&*()-_+=/\\{}[]<>;:\"`|~£¥";

		private const float KEY_SPACING = 4f;

		private const float KEY_WIDTH = 48f;

		private const float KEY_HEIGHT = 37f;

		private UITextPanel<object>[] _keyList = new UITextPanel<object>[50];

		private UITextPanel<object> _shiftButton;

		private UITextPanel<object> _symbolButton;

		private UITextBox _textBox;

		private UITextPanel<LocalizedText> _submitButton;

		private UITextPanel<LocalizedText> _cancelButton;

		private UIText _label;

		private UITextPanel<object> _enterButton;

		private UITextPanel<object> _spacebarButton;

		private UITextPanel<object> _restoreButton;

		private Asset<Texture2D> _textureShift;

		private Asset<Texture2D> _textureBackspace;

		private Color _internalBorderColor = new Color(89, 116, 213);

		private Color _internalBorderColorSelected = Main.OurFavoriteColor;

		private UITextPanel<LocalizedText> _submitButton2;

		private UITextPanel<LocalizedText> _cancelButton2;

		private UIElement outerLayer1;

		private UIElement outerLayer2;

		private bool _allowEmpty;

		private KeyState _keyState;

		private KeyboardSubmitEvent _submitAction;

		private Action _cancelAction;

		private int _lastOffsetDown;

		public static int OffsetDown;

		public static bool ShouldHideText;

		private int _keyboardContext;

		private bool _edittingSign;

		private bool _edittingChest;

		private float _textBoxHeight;

		private float _labelHeight;

		public Func<string, bool> CustomTextValidationForUpdate;

		public Func<string, bool> CustomTextValidationForSubmit;

		public Func<bool> CustomEscapeAttempt;

		private bool _canSubmit;

		public string Text
		{
			get
			{
				return _textBox.Text;
			}
			set
			{
				_textBox.SetText(value);
				ValidateText();
			}
		}

		public bool HideContents
		{
			get
			{
				return _textBox.HideContents;
			}
			set
			{
				_textBox.HideContents = value;
			}
		}

		public static bool CanSubmit
		{
			get
			{
				if (_currentInstance != null)
				{
					return _currentInstance._canSubmit;
				}
				return false;
			}
		}

		public static int KeyboardContext
		{
			get
			{
				if (_currentInstance == null)
				{
					return -1;
				}
				return _currentInstance._keyboardContext;
			}
		}

		public UIVirtualKeyboard(string labelText, string startingText, KeyboardSubmitEvent submitAction, Action cancelAction, int inputMode = 0, bool allowEmpty = false)
		{
			_keyboardContext = inputMode;
			_allowEmpty = allowEmpty;
			OffsetDown = 0;
			ShouldHideText = false;
			_lastOffsetDown = 0;
			_edittingSign = _keyboardContext == 1;
			_edittingChest = _keyboardContext == 2;
			_currentInstance = this;
			_submitAction = submitAction;
			_cancelAction = cancelAction;
			_textureShift = Main.Assets.Request<Texture2D>("Images/UI/VK_Shift", (AssetRequestMode)1);
			_textureBackspace = Main.Assets.Request<Texture2D>("Images/UI/VK_Backspace", (AssetRequestMode)1);
			Top.Pixels = _lastOffsetDown;
			float num = 0.5f;
			float num2 = -20f;
			float num3 = -5000 * _edittingSign.ToInt();
			num2 = 270f;
			num = 0f;
			float num4 = 516f;
			UIElement uIElement = new UIElement();
			uIElement.Width.Pixels = num4 + 8f + 16f;
			uIElement.Top.Precent = num;
			uIElement.Top.Pixels = num2;
			uIElement.Height.Pixels = 266f;
			uIElement.HAlign = 0.5f;
			uIElement.SetPadding(0f);
			outerLayer1 = uIElement;
			UIElement uIElement2 = new UIElement();
			uIElement2.Width.Pixels = num4 + 8f + 16f;
			uIElement2.Top.Precent = num;
			uIElement2.Top.Pixels = num2;
			uIElement2.Height.Pixels = 266f;
			uIElement2.HAlign = 0.5f;
			uIElement2.SetPadding(0f);
			outerLayer2 = uIElement2;
			UIPanel uIPanel = new UIPanel
			{
				Width = 
				{
					Precent = 1f
				},
				Height = 
				{
					Pixels = 225f
				},
				BackgroundColor = new Color(23, 33, 69) * 0.7f
			};
			uIElement.Append(uIPanel);
			float num5 = -50f;
			_textBox = new UITextBox("", 0.78f, large: true);
			_textBox.BackgroundColor = Color.Transparent;
			_textBox.BorderColor = Color.Transparent;
			_textBox.HAlign = 0.5f;
			_textBox.Width.Pixels = num4;
			_textBox.Top.Pixels = num5 + num2 - 10f + num3;
			_textBox.Top.Precent = num;
			_textBox.Height.Pixels = 37f;
			Append(_textBox);
			for (int i = 0; i < 10; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int index = j * 10 + i;
					UITextPanel<object> uITextPanel = CreateKeyboardButton("1234567890qwertyuiopasdfghjkl'zxcvbnm,.?"[index].ToString(), i, j);
					uITextPanel.OnClick += TypeText;
					uIPanel.Append(uITextPanel);
				}
			}
			_shiftButton = CreateKeyboardButton("", 0, 4, 1, style: false);
			_shiftButton.PaddingLeft = 0f;
			_shiftButton.PaddingRight = 0f;
			_shiftButton.PaddingBottom = (_shiftButton.PaddingTop = 0f);
			_shiftButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			_shiftButton.BorderColor = _internalBorderColor * 0.7f;
			_shiftButton.OnMouseOver += delegate
			{
				_shiftButton.BorderColor = _internalBorderColorSelected;
				if (_keyState != KeyState.Shift)
				{
					_shiftButton.BackgroundColor = new Color(73, 94, 171);
				}
			};
			_shiftButton.OnMouseOut += delegate
			{
				_shiftButton.BorderColor = _internalBorderColor * 0.7f;
				if (_keyState != KeyState.Shift)
				{
					_shiftButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
				}
			};
			_shiftButton.OnClick += delegate
			{
				SoundEngine.PlaySound(12);
				SetKeyState((_keyState != KeyState.Shift) ? KeyState.Shift : KeyState.Default);
			};
			UIImage element = new UIImage(_textureShift)
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				ImageScale = 0.85f
			};
			_shiftButton.Append(element);
			uIPanel.Append(_shiftButton);
			_symbolButton = CreateKeyboardButton("@%", 1, 4, 1, style: false);
			_symbolButton.PaddingLeft = 0f;
			_symbolButton.PaddingRight = 0f;
			_symbolButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			_symbolButton.BorderColor = _internalBorderColor * 0.7f;
			_symbolButton.OnMouseOver += delegate
			{
				_symbolButton.BorderColor = _internalBorderColorSelected;
				if (_keyState != KeyState.Symbol)
				{
					_symbolButton.BackgroundColor = new Color(73, 94, 171);
				}
			};
			_symbolButton.OnMouseOut += delegate
			{
				_symbolButton.BorderColor = _internalBorderColor * 0.7f;
				if (_keyState != KeyState.Symbol)
				{
					_symbolButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
				}
			};
			_symbolButton.OnClick += delegate
			{
				SoundEngine.PlaySound(12);
				SetKeyState((_keyState != KeyState.Symbol) ? KeyState.Symbol : KeyState.Default);
			};
			uIPanel.Append(_symbolButton);
			BuildSpaceBarArea(uIPanel);
			_submitButton = new UITextPanel<LocalizedText>((_edittingSign || _edittingChest) ? Language.GetText("UI.Save") : Language.GetText("UI.Submit"), 0.4f, large: true);
			_submitButton.Height.Pixels = 37f;
			_submitButton.Width.Precent = 0.4f;
			_submitButton.HAlign = 1f;
			_submitButton.VAlign = 1f;
			_submitButton.PaddingLeft = 0f;
			_submitButton.PaddingRight = 0f;
			ValidateText();
			_submitButton.OnMouseOver += FadedMouseOver;
			_submitButton.OnMouseOut += FadedMouseOut;
			_submitButton.OnMouseOver += delegate
			{
				ValidateText();
			};
			_submitButton.OnMouseOut += delegate
			{
				ValidateText();
			};
			_submitButton.OnClick += delegate
			{
				Submit();
			};
			uIElement.Append(_submitButton);
			_cancelButton = new UITextPanel<LocalizedText>(Language.GetText("UI.Cancel"), 0.4f, large: true);
			StyleKey(_cancelButton, external: true);
			_cancelButton.Height.Pixels = 37f;
			_cancelButton.Width.Precent = 0.4f;
			_cancelButton.VAlign = 1f;
			_cancelButton.OnClick += delegate
			{
				SoundEngine.PlaySound(11);
				_cancelAction();
			};
			_cancelButton.OnMouseOver += FadedMouseOver;
			_cancelButton.OnMouseOut += FadedMouseOut;
			uIElement.Append(_cancelButton);
			_submitButton2 = new UITextPanel<LocalizedText>((_edittingSign || _edittingChest) ? Language.GetText("UI.Save") : Language.GetText("UI.Submit"), 0.72f, large: true);
			_submitButton2.TextColor = Color.Silver;
			_submitButton2.DrawPanel = false;
			_submitButton2.Height.Pixels = 60f;
			_submitButton2.Width.Precent = 0.4f;
			_submitButton2.HAlign = 0.5f;
			_submitButton2.VAlign = 0f;
			_submitButton2.OnMouseOver += delegate(UIMouseEvent a, UIElement b)
			{
				((UITextPanel<LocalizedText>)b).TextScale = 0.85f;
				((UITextPanel<LocalizedText>)b).TextColor = Color.White;
			};
			_submitButton2.OnMouseOut += delegate(UIMouseEvent a, UIElement b)
			{
				((UITextPanel<LocalizedText>)b).TextScale = 0.72f;
				((UITextPanel<LocalizedText>)b).TextColor = Color.Silver;
			};
			_submitButton2.Top.Pixels = 50f;
			_submitButton2.PaddingLeft = 0f;
			_submitButton2.PaddingRight = 0f;
			ValidateText();
			_submitButton2.OnMouseOver += delegate
			{
				ValidateText();
			};
			_submitButton2.OnMouseOut += delegate
			{
				ValidateText();
			};
			_submitButton2.OnMouseOver += FadedMouseOver;
			_submitButton2.OnMouseOut += FadedMouseOut;
			_submitButton2.OnClick += delegate
			{
				if (TextIsValidForSubmit())
				{
					SoundEngine.PlaySound(10);
					_submitAction(Text.Trim());
				}
			};
			outerLayer2.Append(_submitButton2);
			_cancelButton2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Cancel"), 0.72f, large: true);
			_cancelButton2.TextColor = Color.Silver;
			_cancelButton2.DrawPanel = false;
			_cancelButton2.OnMouseOver += delegate(UIMouseEvent a, UIElement b)
			{
				((UITextPanel<LocalizedText>)b).TextScale = 0.85f;
				((UITextPanel<LocalizedText>)b).TextColor = Color.White;
			};
			_cancelButton2.OnMouseOut += delegate(UIMouseEvent a, UIElement b)
			{
				((UITextPanel<LocalizedText>)b).TextScale = 0.72f;
				((UITextPanel<LocalizedText>)b).TextColor = Color.Silver;
			};
			_cancelButton2.Height.Pixels = 60f;
			_cancelButton2.Width.Precent = 0.4f;
			_cancelButton2.Top.Pixels = 114f;
			_cancelButton2.VAlign = 0f;
			_cancelButton2.HAlign = 0.5f;
			_cancelButton2.OnClick += delegate
			{
				SoundEngine.PlaySound(11);
				_cancelAction();
			};
			outerLayer2.Append(_cancelButton2);
			UITextPanel<object> uITextPanel2 = CreateKeyboardButton("", 8, 4, 2);
			uITextPanel2.OnClick += delegate
			{
				SoundEngine.PlaySound(12);
				_textBox.Backspace();
				ValidateText();
			};
			uITextPanel2.PaddingLeft = 0f;
			uITextPanel2.PaddingRight = 0f;
			uITextPanel2.PaddingBottom = (uITextPanel2.PaddingTop = 0f);
			uITextPanel2.Append(new UIImage(_textureBackspace)
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				ImageScale = 0.92f
			});
			uIPanel.Append(uITextPanel2);
			UIText uIText = new UIText(labelText, 0.75f, large: true)
			{
				HAlign = 0.5f,
				Width = 
				{
					Pixels = num4
				},
				Top = 
				{
					Pixels = num5 - 37f - 4f + num2 + num3,
					Precent = num
				},
				Height = 
				{
					Pixels = 37f
				}
			};
			Append(uIText);
			_label = uIText;
			Append(uIElement);
			int textMaxLength = (_edittingSign ? 1200 : 20);
			_textBox.SetTextMaxLength(textMaxLength);
			Text = startingText;
			if (Text.Length == 0)
			{
				SetKeyState(KeyState.Shift);
			}
			ShouldHideText = true;
			OffsetDown = 9999;
			UpdateOffsetDown();
		}

		public void SetMaxInputLength(int length)
		{
			_textBox.SetTextMaxLength(length);
		}

		private void BuildSpaceBarArea(UIPanel mainPanel)
		{
			Action createTheseTwo = delegate
			{
				bool flag = CanRestore();
				int x = (flag ? 4 : 5);
				bool edittingSign = _edittingSign;
				int num = ((flag && edittingSign) ? 2 : 3);
				UITextPanel<object> uITextPanel = CreateKeyboardButton(Language.GetText("UI.SpaceButton"), 2, 4, (_edittingSign || (_edittingChest && flag)) ? num : 6);
				uITextPanel.OnClick += delegate
				{
					PressSpace();
				};
				mainPanel.Append(uITextPanel);
				_spacebarButton = uITextPanel;
				if (edittingSign)
				{
					UITextPanel<object> uITextPanel2 = CreateKeyboardButton(Language.GetText("UI.EnterButton"), x, 4, num);
					uITextPanel2.OnClick += delegate
					{
						SoundEngine.PlaySound(12);
						_textBox.Write("\n");
						ValidateText();
					};
					mainPanel.Append(uITextPanel2);
					_enterButton = uITextPanel2;
				}
			};
			createTheseTwo();
			if (CanRestore())
			{
				UITextPanel<object> restoreBar = CreateKeyboardButton(Language.GetText("UI.RestoreButton"), 6, 4, 2);
				restoreBar.OnClick += delegate
				{
					SoundEngine.PlaySound(12);
					RestoreCanceledInput(_keyboardContext);
					ValidateText();
					restoreBar.Remove();
					_enterButton.Remove();
					_spacebarButton.Remove();
					createTheseTwo();
				};
				mainPanel.Append(restoreBar);
				_restoreButton = restoreBar;
			}
		}

		private void PressSpace()
		{
			string text = " ";
			if (CustomTextValidationForUpdate != null && !CustomTextValidationForUpdate(Text + text))
			{
				SoundEngine.PlaySound(11);
				return;
			}
			SoundEngine.PlaySound(12);
			_textBox.Write(text);
			ValidateText();
		}

		private bool CanRestore()
		{
			if (_edittingSign)
			{
				return _cancelCacheSign.Length > 0;
			}
			if (_edittingChest)
			{
				return _cancelCacheChest.Length > 0;
			}
			return false;
		}

		private void TypeText(UIMouseEvent evt, UIElement listeningElement)
		{
			string text = ((UITextPanel<object>)listeningElement).Text;
			if (CustomTextValidationForUpdate != null && !CustomTextValidationForUpdate(Text + text))
			{
				SoundEngine.PlaySound(11);
				return;
			}
			SoundEngine.PlaySound(12);
			bool num = Text.Length == 0;
			_textBox.Write(text);
			ValidateText();
			if (num && Text.Length > 0 && _keyState == KeyState.Shift)
			{
				SetKeyState(KeyState.Default);
			}
		}

		public void SetKeyState(KeyState keyState)
		{
			UITextPanel<object> uITextPanel = null;
			switch (_keyState)
			{
			case KeyState.Shift:
				uITextPanel = _shiftButton;
				break;
			case KeyState.Symbol:
				uITextPanel = _symbolButton;
				break;
			}
			if (uITextPanel != null)
			{
				if (uITextPanel.IsMouseHovering)
				{
					uITextPanel.BackgroundColor = new Color(73, 94, 171);
				}
				else
				{
					uITextPanel.BackgroundColor = new Color(63, 82, 151) * 0.7f;
				}
			}
			string text = null;
			UITextPanel<object> uITextPanel2 = null;
			switch (keyState)
			{
			case KeyState.Default:
				text = "1234567890qwertyuiopasdfghjkl'zxcvbnm,.?";
				break;
			case KeyState.Shift:
				text = "1234567890QWERTYUIOPASDFGHJKL'ZXCVBNM,.?";
				uITextPanel2 = _shiftButton;
				break;
			case KeyState.Symbol:
				text = "1234567890!@#$%^&*()-_+=/\\{}[]<>;:\"`|~£¥";
				uITextPanel2 = _symbolButton;
				break;
			}
			for (int i = 0; i < text.Length; i++)
			{
				_keyList[i].SetText(text[i].ToString());
			}
			_keyState = keyState;
			if (uITextPanel2 != null)
			{
				uITextPanel2.BackgroundColor = new Color(93, 114, 191);
			}
		}

		private void ValidateText()
		{
			if (TextIsValidForSubmit())
			{
				_canSubmit = true;
				_submitButton.TextColor = Color.White;
				if (_submitButton.IsMouseHovering)
				{
					_submitButton.BackgroundColor = new Color(73, 94, 171);
				}
				else
				{
					_submitButton.BackgroundColor = new Color(63, 82, 151) * 0.7f;
				}
			}
			else
			{
				_canSubmit = false;
				_submitButton.TextColor = Color.Gray;
				if (_submitButton.IsMouseHovering)
				{
					_submitButton.BackgroundColor = new Color(180, 60, 60) * 0.85f;
				}
				else
				{
					_submitButton.BackgroundColor = new Color(150, 40, 40) * 0.85f;
				}
			}
		}

		private bool TextIsValidForSubmit()
		{
			if (CustomTextValidationForUpdate != null)
			{
				return CustomTextValidationForUpdate(Text);
			}
			if (Text.Trim().Length <= 0 && !_edittingSign && !_edittingChest)
			{
				return _allowEmpty;
			}
			return true;
		}

		private void StyleKey<T>(UITextPanel<T> button, bool external = false)
		{
			button.PaddingLeft = 0f;
			button.PaddingRight = 0f;
			button.BackgroundColor = new Color(63, 82, 151) * 0.7f;
			if (!external)
			{
				button.BorderColor = _internalBorderColor * 0.7f;
			}
			button.OnMouseOver += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				((UITextPanel<T>)listeningElement).BackgroundColor = new Color(73, 94, 171) * 0.85f;
				if (!external)
				{
					((UITextPanel<T>)listeningElement).BorderColor = _internalBorderColorSelected * 0.85f;
				}
			};
			button.OnMouseOut += delegate(UIMouseEvent evt, UIElement listeningElement)
			{
				((UITextPanel<T>)listeningElement).BackgroundColor = new Color(63, 82, 151) * 0.7f;
				if (!external)
				{
					((UITextPanel<T>)listeningElement).BorderColor = _internalBorderColor * 0.7f;
				}
			};
		}

		private UITextPanel<object> CreateKeyboardButton(object text, int x, int y, int width = 1, bool style = true)
		{
			float num = 516f;
			UITextPanel<object> uITextPanel = new UITextPanel<object>(text, 0.4f, large: true);
			uITextPanel.Width.Pixels = 48f * (float)width + 4f * (float)(width - 1);
			uITextPanel.Height.Pixels = 37f;
			uITextPanel.Left.Precent = 0.5f;
			uITextPanel.Left.Pixels = 52f * (float)x - num * 0.5f;
			uITextPanel.Top.Pixels = 41f * (float)y;
			if (style)
			{
				StyleKey(uITextPanel);
			}
			for (int i = 0; i < width; i++)
			{
				_keyList[y * 10 + x + i] = uITextPanel;
			}
			return uITextPanel;
		}

		private bool ShouldShowKeyboard()
		{
			return PlayerInput.SettingsForUI.ShowGamepadHints;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (Main.gameMenu)
			{
				if (ShouldShowKeyboard())
				{
					outerLayer2.Remove();
					if (!Elements.Contains(outerLayer1))
					{
						Append(outerLayer1);
					}
					outerLayer1.Activate();
					outerLayer2.Deactivate();
					Recalculate();
					RecalculateChildren();
					if (_labelHeight != 0f)
					{
						_textBox.Top.Pixels = _textBoxHeight;
						_label.Top.Pixels = _labelHeight;
						_textBox.Recalculate();
						_label.Recalculate();
						_labelHeight = (_textBoxHeight = 0f);
						UserInterface.ActiveInstance.ResetLasts();
					}
				}
				else
				{
					outerLayer1.Remove();
					if (!Elements.Contains(outerLayer2))
					{
						Append(outerLayer2);
					}
					outerLayer2.Activate();
					outerLayer1.Deactivate();
					Recalculate();
					RecalculateChildren();
					if (_textBoxHeight == 0f)
					{
						_textBoxHeight = _textBox.Top.Pixels;
						_labelHeight = _label.Top.Pixels;
						_textBox.Top.Pixels += 50f;
						_label.Top.Pixels += 50f;
						_textBox.Recalculate();
						_label.Recalculate();
						UserInterface.ActiveInstance.ResetLasts();
					}
				}
			}
			if (!Main.editSign && _edittingSign)
			{
				IngameFancyUI.Close();
				return;
			}
			if (!Main.editChest && _edittingChest)
			{
				IngameFancyUI.Close();
				return;
			}
			base.DrawSelf(spriteBatch);
			UpdateOffsetDown();
			OffsetDown = 0;
			ShouldHideText = false;
			SetupGamepadPoints(spriteBatch);
			PlayerInput.WritingText = true;
			Main.instance.HandleIME();
			Vector2 position = new Vector2(Main.screenWidth / 2, _textBox.GetDimensions().ToRectangle().Bottom + 32);
			Main.instance.DrawWindowsIMEPanel(position, 0.5f);
			string text = Main.GetInputText(Text, _edittingSign);
			if (_edittingSign && Main.inputTextEnter)
			{
				text += "\n";
			}
			else
			{
				if (_edittingChest && Main.inputTextEnter)
				{
					ChestUI.RenameChestSubmit(Main.player[Main.myPlayer]);
					IngameFancyUI.Close();
					return;
				}
				if (Main.inputTextEnter && CanSubmit)
				{
					Submit();
				}
				else if (_edittingChest && Main.player[Main.myPlayer].chest < 0)
				{
					ChestUI.RenameChestCancel();
				}
				else if (Main.inputTextEscape && TryEscapingMenu())
				{
					return;
				}
			}
			if (IngameFancyUI.CanShowVirtualKeyboard(_keyboardContext))
			{
				if (text != Text)
				{
					if (CustomTextValidationForUpdate == null || CustomTextValidationForUpdate(text))
					{
						Text = text;
					}
					else
					{
						SoundEngine.PlaySound(11);
					}
				}
				if (_edittingSign)
				{
					CopyTextToSign();
				}
				if (_edittingChest)
				{
					CopyTextToChest();
				}
			}
			byte b = (byte)((255 + Main.tileColor.R * 2) / 3);
			Color value = new Color(b, b, b, 255);
			_textBox.TextColor = Color.Lerp(Color.White, value, 0.2f);
			_label.TextColor = Color.Lerp(Color.White, value, 0.2f);
			position = new Vector2(Main.screenWidth / 2, _textBox.GetDimensions().ToRectangle().Bottom + 32);
			Main.instance.DrawWindowsIMEPanel(position, 0.5f);
		}

		private bool TryEscapingMenu()
		{
			if (CustomEscapeAttempt != null)
			{
				return CustomEscapeAttempt();
			}
			if (_edittingSign)
			{
				Main.InputTextSignCancel();
			}
			if (_edittingChest)
			{
				ChestUI.RenameChestCancel();
			}
			if (Main.gameMenu && _cancelAction != null)
			{
				Cancel();
			}
			else
			{
				IngameFancyUI.Close();
			}
			return true;
		}

		private void UpdateOffsetDown()
		{
			_textBox.HideSelf = ShouldHideText;
			int num = OffsetDown - _lastOffsetDown;
			int num2 = num;
			if (Math.Abs(num) < 10)
			{
				num2 = num;
			}
			_lastOffsetDown += num2;
			if (num2 != 0)
			{
				Top.Pixels += num2;
				Recalculate();
			}
		}

		public override void OnActivate()
		{
			if (PlayerInput.UsingGamepadUI && _restoreButton != null)
			{
				UILinkPointNavigator.ChangePoint(3002);
			}
		}

		public override void OnDeactivate()
		{
			base.OnDeactivate();
			PlayerInput.WritingText = false;
			UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 0;
		}

		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 6;
			UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 1;
			int num = 3002;
			int num2 = num;
			int num3 = 5;
			int num4 = 10;
			int num5 = num4 * num3 - 1;
			int num6 = num4 * (num3 - 1);
			UILinkPointNavigator.SetPosition(3000, _cancelButton.GetDimensions().Center());
			UILinkPoint uILinkPoint = UILinkPointNavigator.Points[3000];
			uILinkPoint.Unlink();
			uILinkPoint.Right = 3001;
			uILinkPoint.Up = num + num6;
			UILinkPointNavigator.SetPosition(3001, _submitButton.GetDimensions().Center());
			uILinkPoint = UILinkPointNavigator.Points[3001];
			uILinkPoint.Unlink();
			uILinkPoint.Left = 3000;
			uILinkPoint.Up = num + num5;
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < num4; j++)
				{
					int num7 = i * num4 + j;
					num2 = num + num7;
					if (_keyList[num7] != null)
					{
						UILinkPointNavigator.SetPosition(num2, _keyList[num7].GetDimensions().Center());
						uILinkPoint = UILinkPointNavigator.Points[num2];
						uILinkPoint.Unlink();
						int num8 = j - 1;
						while (num8 >= 0 && _keyList[i * num4 + num8] == _keyList[num7])
						{
							num8--;
						}
						if (num8 != -1)
						{
							uILinkPoint.Left = i * num4 + num8 + num;
						}
						else
						{
							uILinkPoint.Left = i * num4 + (num4 - 1) + num;
						}
						int k;
						for (k = j + 1; k <= num4 - 1 && _keyList[i * num4 + k] == _keyList[num7]; k++)
						{
						}
						if (k != num4 && _keyList[num7] != _keyList[k])
						{
							uILinkPoint.Right = i * num4 + k + num;
						}
						else
						{
							uILinkPoint.Right = i * num4 + num;
						}
						if (i != 0)
						{
							uILinkPoint.Up = num2 - num4;
						}
						if (i != num3 - 1)
						{
							uILinkPoint.Down = num2 + num4;
						}
						else
						{
							uILinkPoint.Down = ((j < num3) ? 3000 : 3001);
						}
					}
				}
			}
		}

		public static void CycleSymbols()
		{
			if (_currentInstance != null)
			{
				switch (_currentInstance._keyState)
				{
				case KeyState.Default:
					_currentInstance.SetKeyState(KeyState.Shift);
					break;
				case KeyState.Shift:
					_currentInstance.SetKeyState(KeyState.Symbol);
					break;
				case KeyState.Symbol:
					_currentInstance.SetKeyState(KeyState.Default);
					break;
				}
			}
		}

		public static void BackSpace()
		{
			if (_currentInstance != null)
			{
				SoundEngine.PlaySound(12);
				_currentInstance._textBox.Backspace();
				_currentInstance.ValidateText();
			}
		}

		public static void Submit()
		{
			if (_currentInstance != null)
			{
				_currentInstance.InternalSubmit();
			}
		}

		private void InternalSubmit()
		{
			string text = Text.Trim();
			if (TextIsValidForSubmit())
			{
				SoundEngine.PlaySound(10);
				_submitAction(text);
			}
		}

		public static void Cancel()
		{
			if (_currentInstance != null)
			{
				SoundEngine.PlaySound(11);
				_currentInstance._cancelAction();
			}
		}

		public static void Write(string text)
		{
			if (_currentInstance != null)
			{
				SoundEngine.PlaySound(12);
				bool num = _currentInstance.Text.Length == 0;
				_currentInstance._textBox.Write(text);
				_currentInstance.ValidateText();
				if (num && _currentInstance.Text.Length > 0 && _currentInstance._keyState == KeyState.Shift)
				{
					_currentInstance.SetKeyState(KeyState.Default);
				}
			}
		}

		public static void CursorLeft()
		{
			if (_currentInstance != null)
			{
				SoundEngine.PlaySound(12);
				_currentInstance._textBox.CursorLeft();
			}
		}

		public static void CursorRight()
		{
			if (_currentInstance != null)
			{
				SoundEngine.PlaySound(12);
				_currentInstance._textBox.CursorRight();
			}
		}

		public static bool CanDisplay(int keyboardContext)
		{
			if (keyboardContext == 1)
			{
				return Main.screenHeight > 700;
			}
			return true;
		}

		public static void CacheCanceledInput(int cacheMode)
		{
			if (cacheMode == 1)
			{
				_cancelCacheSign = Main.npcChatText;
			}
		}

		private void RestoreCanceledInput(int cacheMode)
		{
			if (cacheMode == 1)
			{
				Main.npcChatText = _cancelCacheSign;
				Text = Main.npcChatText;
				_cancelCacheSign = "";
			}
		}

		private void CopyTextToSign()
		{
			if (_edittingSign)
			{
				int sign = Main.player[Main.myPlayer].sign;
				if (sign >= 0 && Main.sign[sign] != null)
				{
					Main.npcChatText = Text;
				}
			}
		}

		private void CopyTextToChest()
		{
			if (_edittingChest)
			{
				Main.npcChatText = Text;
			}
		}

		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}
	}
}
