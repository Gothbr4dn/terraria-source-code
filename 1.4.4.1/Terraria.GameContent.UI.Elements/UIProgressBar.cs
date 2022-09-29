using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIProgressBar : UIElement
	{
		private class UIInnerProgressBar : UIElement
		{
			protected override void DrawSelf(SpriteBatch spriteBatch)
			{
				CalculatedStyle dimensions = GetDimensions();
				spriteBatch.Draw(TextureAssets.MagicPixel.get_Value(), new Vector2(dimensions.X, dimensions.Y), null, Color.Blue, 0f, Vector2.Zero, new Vector2(dimensions.Width, dimensions.Height / 1000f), SpriteEffects.None, 0f);
			}
		}

		private UIInnerProgressBar _progressBar = new UIInnerProgressBar();

		private float _visualProgress;

		private float _targetProgress;

		public UIProgressBar()
		{
			_progressBar.Height.Precent = 1f;
			_progressBar.Recalculate();
			Append(_progressBar);
		}

		public void SetProgress(float value)
		{
			_targetProgress = value;
			if (value < _visualProgress)
			{
				_visualProgress = value;
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			_visualProgress = _visualProgress * 0.95f + 0.05f * _targetProgress;
			_progressBar.Width.Precent = _visualProgress;
			_progressBar.Recalculate();
		}
	}
}
