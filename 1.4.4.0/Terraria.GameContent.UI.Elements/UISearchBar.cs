using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UISearchBar : UIElement
	{
		private readonly LocalizedText _textToShowWhenEmpty;

		private UITextBox _text;

		private string actualContents;

		private float _textScale;

		private bool isWritingText;

		public bool HasContents => !string.IsNullOrWhiteSpace(actualContents);

		public bool IsWritingText => isWritingText;

		public event Action<string> OnContentsChanged;

		public event Action OnStartTakingInput;

		public event Action OnEndTakingInput;

		public event Action OnCanceledTakingInput;

		public event Action OnNeedingVirtualKeyboard;

		public UISearchBar(LocalizedText emptyContentText, float scale)
		{
			_textToShowWhenEmpty = emptyContentText;
			_textScale = scale;
			UITextBox uITextBox = new UITextBox("", scale)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				BackgroundColor = Color.Transparent,
				BorderColor = Color.Transparent,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				TextHAlign = 0f,
				ShowInputTicker = false
			};
			uITextBox.SetTextMaxLength(50);
			Append(uITextBox);
			_text = uITextBox;
		}

		public void SetContents(string contents, bool forced = false)
		{
			if (!(actualContents == contents) || forced)
			{
				actualContents = contents;
				if (string.IsNullOrEmpty(actualContents))
				{
					_text.TextColor = Color.Gray;
					_text.SetText(_textToShowWhenEmpty.Value, _textScale, large: false);
				}
				else
				{
					_text.TextColor = Color.White;
					_text.SetText(actualContents);
					actualContents = _text.Text;
				}
				TrimDisplayIfOverElementDimensions(0);
				if (this.OnContentsChanged != null)
				{
					this.OnContentsChanged(contents);
				}
			}
		}

		public void TrimDisplayIfOverElementDimensions(int padding)
		{
			CalculatedStyle dimensions = GetDimensions();
			if (dimensions.Width != 0f || dimensions.Height != 0f)
			{
				Point point = new Point((int)dimensions.X, (int)dimensions.Y);
				Point point2 = new Point(point.X + (int)dimensions.Width, point.Y + (int)dimensions.Height);
				Rectangle rectangle = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
				CalculatedStyle dimensions2 = _text.GetDimensions();
				Point point3 = new Point((int)dimensions2.X, (int)dimensions2.Y);
				Point point4 = new Point(point3.X + (int)_text.MinWidth.Pixels, point3.Y + (int)_text.MinHeight.Pixels);
				Rectangle rectangle2 = new Rectangle(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
				int num = 0;
				while (rectangle2.Right > rectangle.Right - padding && _text.Text.Length > 0)
				{
					_text.SetText(_text.Text.Substring(0, _text.Text.Length - 1));
					num++;
					RecalculateChildren();
					dimensions2 = _text.GetDimensions();
					point3 = new Point((int)dimensions2.X, (int)dimensions2.Y);
					point4 = new Point(point3.X + (int)_text.MinWidth.Pixels, point3.Y + (int)_text.MinHeight.Pixels);
					rectangle2 = new Rectangle(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
					actualContents = _text.Text;
				}
			}
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			base.MouseDown(evt);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			SoundEngine.PlaySound(12);
		}

		public override void Update(GameTime gameTime)
		{
			if (isWritingText)
			{
				if (NeedsVirtualkeyboard())
				{
					if (this.OnNeedingVirtualKeyboard != null)
					{
						this.OnNeedingVirtualKeyboard();
					}
					return;
				}
				PlayerInput.WritingText = true;
				Main.CurrentInputTextTakerOverride = this;
			}
			base.Update(gameTime);
		}

		private bool NeedsVirtualkeyboard()
		{
			return PlayerInput.SettingsForUI.ShowGamepadHints;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			if (!isWritingText)
			{
				return;
			}
			PlayerInput.WritingText = true;
			Main.instance.HandleIME();
			Vector2 position = new Vector2(Main.screenWidth / 2, _text.GetDimensions().ToRectangle().Bottom + 32);
			Main.instance.DrawWindowsIMEPanel(position, 0.5f);
			string inputText = Main.GetInputText(actualContents);
			if (Main.inputTextEnter)
			{
				ToggleTakingText();
			}
			else if (Main.inputTextEscape)
			{
				ToggleTakingText();
				if (this.OnCanceledTakingInput != null)
				{
					this.OnCanceledTakingInput();
				}
			}
			SetContents(inputText);
			position = new Vector2(Main.screenWidth / 2, _text.GetDimensions().ToRectangle().Bottom + 32);
			Main.instance.DrawWindowsIMEPanel(position, 0.5f);
		}

		public void ToggleTakingText()
		{
			isWritingText = !isWritingText;
			_text.ShowInputTicker = isWritingText;
			Main.clrInput();
			if (isWritingText)
			{
				if (this.OnStartTakingInput != null)
				{
					this.OnStartTakingInput();
				}
			}
			else if (this.OnEndTakingInput != null)
			{
				this.OnEndTakingInput();
			}
		}
	}
}
