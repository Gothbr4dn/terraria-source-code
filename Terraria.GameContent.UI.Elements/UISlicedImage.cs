using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UISlicedImage : UIElement
	{
		private Asset<Texture2D> _texture;

		private Color _color;

		private int _leftSliceDepth;

		private int _rightSliceDepth;

		private int _topSliceDepth;

		private int _bottomSliceDepth;

		public Color Color
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

		public UISlicedImage(Asset<Texture2D> texture)
		{
			_texture = texture;
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}

		public void SetImage(Asset<Texture2D> texture)
		{
			_texture = texture;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			Utils.DrawSplicedPanel(spriteBatch, _texture.get_Value(), (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, _leftSliceDepth, _rightSliceDepth, _topSliceDepth, _bottomSliceDepth, _color);
		}

		public void SetSliceDepths(int top, int bottom, int left, int right)
		{
			_leftSliceDepth = left;
			_rightSliceDepth = right;
			_topSliceDepth = top;
			_bottomSliceDepth = bottom;
		}

		public void SetSliceDepths(int fluff)
		{
			_leftSliceDepth = fluff;
			_rightSliceDepth = fluff;
			_topSliceDepth = fluff;
			_bottomSliceDepth = fluff;
		}
	}
}
