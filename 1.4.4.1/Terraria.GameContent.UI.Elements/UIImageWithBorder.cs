using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIImageWithBorder : UIImage
	{
		private Asset<Texture2D> _borderTexture;

		private Texture2D _nonReloadingBorderTexture;

		public UIImageWithBorder(Asset<Texture2D> texture, Asset<Texture2D> borderTexture)
			: base(texture)
		{
			SetBorder(borderTexture);
		}

		public UIImageWithBorder(Texture2D nonReloadingTexture, Texture2D nonReloadingBorderTexture)
			: base(nonReloadingTexture)
		{
			SetBorder(nonReloadingBorderTexture);
		}

		public void SetBorder(Asset<Texture2D> texture)
		{
			_borderTexture = texture;
			_nonReloadingBorderTexture = null;
			Width.Set(_borderTexture.Width(), 0f);
			Height.Set(_borderTexture.Height(), 0f);
		}

		public void SetBorder(Texture2D nonReloadingTexture)
		{
			_borderTexture = null;
			_nonReloadingBorderTexture = nonReloadingTexture;
			Width.Set(_nonReloadingBorderTexture.Width, 0f);
			Height.Set(_nonReloadingBorderTexture.Height, 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle dimensions = GetDimensions();
			Texture2D texture2D = null;
			if (_borderTexture != null)
			{
				texture2D = _borderTexture.get_Value();
			}
			if (_nonReloadingBorderTexture != null)
			{
				texture2D = _nonReloadingBorderTexture;
			}
			if (ScaleToFit)
			{
				spriteBatch.Draw(texture2D, dimensions.ToRectangle(), Color);
				return;
			}
			Vector2 vector = texture2D.Size();
			Vector2 vector2 = dimensions.Position() + vector * (1f - ImageScale) / 2f + vector * NormalizedOrigin;
			if (RemoveFloatingPointsFromDrawPosition)
			{
				vector2 = vector2.Floor();
			}
			spriteBatch.Draw(texture2D, vector2, null, Color, Rotation, vector * NormalizedOrigin, ImageScale, SpriteEffects.None, 0f);
		}
	}
}
