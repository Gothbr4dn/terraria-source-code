using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIToggleImage : UIElement
	{
		private Asset<Texture2D> _onTexture;

		private Asset<Texture2D> _offTexture;

		private int _drawWidth;

		private int _drawHeight;

		private Point _onTextureOffset = Point.Zero;

		private Point _offTextureOffset = Point.Zero;

		private bool _isOn;

		public bool IsOn => _isOn;

		public UIToggleImage(Asset<Texture2D> texture, int width, int height, Point onTextureOffset, Point offTextureOffset)
		{
			_onTexture = texture;
			_offTexture = texture;
			_offTextureOffset = offTextureOffset;
			_onTextureOffset = onTextureOffset;
			_drawWidth = width;
			_drawHeight = height;
			Width.Set(width, 0f);
			Height.Set(height, 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			Texture2D value;
			Point point;
			if (_isOn)
			{
				value = _onTexture.get_Value();
				point = _onTextureOffset;
			}
			else
			{
				value = _offTexture.get_Value();
				point = _offTextureOffset;
			}
			Color color = (base.IsMouseHovering ? Color.White : Color.Silver);
			spriteBatch.Draw(value, new Rectangle((int)dimensions.X, (int)dimensions.Y, _drawWidth, _drawHeight), new Rectangle(point.X, point.Y, _drawWidth, _drawHeight), color);
		}

		public override void Click(UIMouseEvent evt)
		{
			Toggle();
			base.Click(evt);
		}

		public void SetState(bool value)
		{
			_isOn = value;
		}

		public void Toggle()
		{
			_isOn = !_isOn;
		}
	}
}
