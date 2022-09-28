using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIBestiaryInfoLine<T> : UIElement, IManuallyOrderedUIElement
	{
		private T _text;

		private float _textScale = 1f;

		private Vector2 _textSize = Vector2.Zero;

		private Color _color = Color.White;

		public int OrderInUIList { get; set; }

		public float TextScale
		{
			get
			{
				return _textScale;
			}
			set
			{
				_textScale = value;
			}
		}

		public Vector2 TextSize => _textSize;

		public string Text
		{
			get
			{
				if (_text != null)
				{
					return _text.ToString();
				}
				return "";
			}
		}

		public Color TextColor
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
			}
		}

		public UIBestiaryInfoLine(T text, float textScale = 1f)
		{
			SetText(text, textScale);
		}

		public override void Recalculate()
		{
			SetText(_text, _textScale);
			base.Recalculate();
		}

		public void SetText(T text)
		{
			SetText(text, _textScale);
		}

		public virtual void SetText(T text, float textScale)
		{
			Vector2 textSize = new Vector2(FontAssets.MouseText.get_Value().MeasureString(text.ToString()).X, 16f) * textScale;
			_text = text;
			_textScale = textScale;
			_textSize = textSize;
			MinWidth.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
			MinHeight.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle innerDimensions = GetInnerDimensions();
			Vector2 pos = innerDimensions.Position();
			pos.Y -= 2f * _textScale;
			pos.X += (innerDimensions.Width - _textSize.X) * 0.5f;
			Utils.DrawBorderString(spriteBatch, Text, pos, _color, _textScale);
		}

		public override int CompareTo(object obj)
		{
			if (obj is IManuallyOrderedUIElement manuallyOrderedUIElement)
			{
				return OrderInUIList.CompareTo(manuallyOrderedUIElement.OrderInUIList);
			}
			return base.CompareTo(obj);
		}
	}
}
