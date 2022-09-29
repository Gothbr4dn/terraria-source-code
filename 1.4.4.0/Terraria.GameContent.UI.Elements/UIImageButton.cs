using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIImageButton : UIElement
	{
		private Asset<Texture2D> _texture;

		private float _visibilityActive = 1f;

		private float _visibilityInactive = 0.4f;

		private Asset<Texture2D> _borderTexture;

		public UIImageButton(Asset<Texture2D> texture)
		{
			_texture = texture;
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}

		public void SetHoverImage(Asset<Texture2D> texture)
		{
			_borderTexture = texture;
		}

		public void SetImage(Asset<Texture2D> texture)
		{
			_texture = texture;
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(_texture.get_Value(), dimensions.Position(), Color.White * (base.IsMouseHovering ? _visibilityActive : _visibilityInactive));
			if (_borderTexture != null && base.IsMouseHovering)
			{
				spriteBatch.Draw(_borderTexture.get_Value(), dimensions.Position(), Color.White);
			}
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			SoundEngine.PlaySound(12);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
		}

		public void SetVisibility(float whenActive, float whenInactive)
		{
			_visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
			_visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
		}
	}
}
