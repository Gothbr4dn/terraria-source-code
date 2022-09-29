using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	internal class UITextBox : UITextPanel<string>
	{
		private int _cursor;

		private int _frameCount;

		private int _maxLength = 20;

		public bool ShowInputTicker = true;

		public bool HideSelf;

		public UITextBox(string text, float textScale = 1f, bool large = false)
			: base(text, textScale, large)
		{
		}

		public void Write(string text)
		{
			SetText(base.Text.Insert(_cursor, text));
			_cursor += text.Length;
		}

		public override void SetText(string text, float textScale, bool large)
		{
			if (text == null)
			{
				text = "";
			}
			if (text.Length > _maxLength)
			{
				text = text.Substring(0, _maxLength);
			}
			base.SetText(text, textScale, large);
			_cursor = Math.Min(base.Text.Length, _cursor);
		}

		public void SetTextMaxLength(int maxLength)
		{
			_maxLength = maxLength;
		}

		public void Backspace()
		{
			if (_cursor != 0)
			{
				SetText(base.Text.Substring(0, base.Text.Length - 1));
			}
		}

		public void CursorLeft()
		{
			if (_cursor != 0)
			{
				_cursor--;
			}
		}

		public void CursorRight()
		{
			if (_cursor < base.Text.Length)
			{
				_cursor++;
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (HideSelf)
			{
				return;
			}
			_cursor = base.Text.Length;
			base.DrawSelf(spriteBatch);
			_frameCount++;
			if ((_frameCount %= 40) <= 20 && ShowInputTicker)
			{
				CalculatedStyle innerDimensions = GetInnerDimensions();
				Vector2 pos = innerDimensions.Position();
				Vector2 vector = new Vector2((base.IsLarge ? FontAssets.DeathText.get_Value() : FontAssets.MouseText.get_Value()).MeasureString(base.Text.Substring(0, _cursor)).X, base.IsLarge ? 32f : 16f) * base.TextScale;
				if (base.IsLarge)
				{
					pos.Y -= 8f * base.TextScale;
				}
				else
				{
					pos.Y -= 2f * base.TextScale;
				}
				pos.X += (innerDimensions.Width - base.TextSize.X) * TextHAlign + vector.X - (base.IsLarge ? 8f : 4f) * base.TextScale + 6f;
				if (base.IsLarge)
				{
					Utils.DrawBorderStringBig(spriteBatch, "|", pos, base.TextColor, base.TextScale);
				}
				else
				{
					Utils.DrawBorderString(spriteBatch, "|", pos, base.TextColor, base.TextScale);
				}
			}
		}
	}
}
