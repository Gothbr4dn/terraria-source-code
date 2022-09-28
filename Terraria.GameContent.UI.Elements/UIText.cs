using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements
{
	public class UIText : UIElement
	{
		private object _text = "";

		private float _textScale = 1f;

		private Vector2 _textSize = Vector2.Zero;

		private bool _isLarge;

		private Color _color = Color.White;

		private Color _shadowColor = Color.Black;

		private bool _isWrapped;

		public bool DynamicallyScaleDownToWidth;

		private string _visibleText;

		private string _lastTextReference;

		public string Text => _text.ToString();

		public float TextOriginX { get; set; }

		public float TextOriginY { get; set; }

		public float WrappedTextBottomPadding { get; set; }

		public bool IsWrapped
		{
			get
			{
				return _isWrapped;
			}
			set
			{
				_isWrapped = value;
				InternalSetText(_text, _textScale, _isLarge);
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

		public Color ShadowColor
		{
			get
			{
				return _shadowColor;
			}
			set
			{
				_shadowColor = value;
			}
		}

		public event Action OnInternalTextChange;

		public UIText(string text, float textScale = 1f, bool large = false)
		{
			TextOriginX = 0.5f;
			TextOriginY = 0f;
			IsWrapped = false;
			WrappedTextBottomPadding = 20f;
			InternalSetText(text, textScale, large);
		}

		public UIText(LocalizedText text, float textScale = 1f, bool large = false)
		{
			TextOriginX = 0.5f;
			TextOriginY = 0f;
			IsWrapped = false;
			WrappedTextBottomPadding = 20f;
			InternalSetText(text, textScale, large);
		}

		public override void Recalculate()
		{
			InternalSetText(_text, _textScale, _isLarge);
			base.Recalculate();
		}

		public void SetText(string text)
		{
			InternalSetText(text, _textScale, _isLarge);
		}

		public void SetText(LocalizedText text)
		{
			InternalSetText(text, _textScale, _isLarge);
		}

		public void SetText(string text, float textScale, bool large)
		{
			InternalSetText(text, textScale, large);
		}

		public void SetText(LocalizedText text, float textScale, bool large)
		{
			InternalSetText(text, textScale, large);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			VerifyTextState();
			CalculatedStyle innerDimensions = GetInnerDimensions();
			Vector2 position = innerDimensions.Position();
			if (_isLarge)
			{
				position.Y -= 10f * _textScale;
			}
			else
			{
				position.Y -= 2f * _textScale;
			}
			position.X += (innerDimensions.Width - _textSize.X) * TextOriginX;
			position.Y += (innerDimensions.Height - _textSize.Y) * TextOriginY;
			float num = _textScale;
			if (DynamicallyScaleDownToWidth && _textSize.X > innerDimensions.Width)
			{
				num *= innerDimensions.Width / _textSize.X;
			}
			DynamicSpriteFont value = (_isLarge ? FontAssets.DeathText : FontAssets.MouseText).get_Value();
			Vector2 vector = value.MeasureString(_visibleText);
			Color baseColor = _shadowColor * ((float)(int)_color.A / 255f);
			Vector2 origin = new Vector2(0f, 0f) * vector;
			Vector2 baseScale = new Vector2(num);
			TextSnippet[] snippets = ChatManager.ParseMessage(_visibleText, _color).ToArray();
			ChatManager.ConvertNormalSnippets(snippets);
			ChatManager.DrawColorCodedStringShadow(spriteBatch, value, snippets, position, baseColor, 0f, origin, baseScale, -1f, 1.5f);
			ChatManager.DrawColorCodedString(spriteBatch, value, snippets, position, Color.White, 0f, origin, baseScale, out var _, -1f);
		}

		private void VerifyTextState()
		{
			if ((object)_lastTextReference != Text)
			{
				InternalSetText(_text, _textScale, _isLarge);
			}
		}

		private void InternalSetText(object text, float textScale, bool large)
		{
			DynamicSpriteFont val = (large ? FontAssets.DeathText.get_Value() : FontAssets.MouseText.get_Value());
			_text = text;
			_isLarge = large;
			_textScale = textScale;
			_lastTextReference = _text.ToString();
			if (IsWrapped)
			{
				_visibleText = val.CreateWrappedText(_lastTextReference, GetInnerDimensions().Width / _textScale);
			}
			else
			{
				_visibleText = _lastTextReference;
			}
			Vector2 vector = val.MeasureString(_visibleText);
			Vector2 vector2 = (_textSize = ((!IsWrapped) ? (new Vector2(vector.X, large ? 32f : 16f) * textScale) : (new Vector2(vector.X, vector.Y + WrappedTextBottomPadding) * textScale)));
			MinWidth.Set(vector2.X + PaddingLeft + PaddingRight, 0f);
			MinHeight.Set(vector2.Y + PaddingTop + PaddingBottom, 0f);
			if (this.OnInternalTextChange != null)
			{
				this.OnInternalTextChange();
			}
		}
	}
}
