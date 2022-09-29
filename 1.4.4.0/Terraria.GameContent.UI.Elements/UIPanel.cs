using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIPanel : UIElement
	{
		private int _cornerSize = 12;

		private int _barSize = 4;

		private Asset<Texture2D> _borderTexture;

		private Asset<Texture2D> _backgroundTexture;

		public Color BorderColor = Color.Black;

		public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;

		public UIPanel()
		{
			if (_borderTexture == null)
			{
				_borderTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder", (AssetRequestMode)1);
			}
			if (_backgroundTexture == null)
			{
				_backgroundTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground", (AssetRequestMode)1);
			}
			SetPadding(_cornerSize);
		}

		public UIPanel(Asset<Texture2D> customBackground, Asset<Texture2D> customborder, int customCornerSize = 12, int customBarSize = 4)
		{
			if (_borderTexture == null)
			{
				_borderTexture = customborder;
			}
			if (_backgroundTexture == null)
			{
				_backgroundTexture = customBackground;
			}
			_cornerSize = customCornerSize;
			_barSize = customBarSize;
			SetPadding(_cornerSize);
		}

		private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
		{
			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			Point point2 = new Point(point.X + (int)dimensions.Width - _cornerSize, point.Y + (int)dimensions.Height - _cornerSize);
			int width = point2.X - point.X - _cornerSize;
			int height = point2.Y - point.Y - _cornerSize;
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, _cornerSize, _cornerSize), new Rectangle(0, 0, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, _cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, 0, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, _cornerSize, _cornerSize), new Rectangle(0, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, _cornerSize, _cornerSize), new Rectangle(_cornerSize + _barSize, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y, width, _cornerSize), new Rectangle(_cornerSize, 0, _barSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point2.Y, width, _cornerSize), new Rectangle(_cornerSize, _cornerSize + _barSize, _barSize, _cornerSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + _cornerSize, _cornerSize, height), new Rectangle(0, _cornerSize, _cornerSize, _barSize), color);
			spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + _cornerSize, _cornerSize, height), new Rectangle(_cornerSize + _barSize, _cornerSize, _cornerSize, _barSize), color);
			spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y + _cornerSize, width, height), new Rectangle(_cornerSize, _cornerSize, _barSize, _barSize), color);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (_backgroundTexture != null)
			{
				DrawPanel(spriteBatch, _backgroundTexture.get_Value(), BackgroundColor);
			}
			if (_borderTexture != null)
			{
				DrawPanel(spriteBatch, _borderTexture.get_Value(), BorderColor);
			}
		}
	}
}
