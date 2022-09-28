using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIColoredImageButton : UIElement
	{
		private Asset<Texture2D> _backPanelTexture;

		private Asset<Texture2D> _texture;

		private Asset<Texture2D> _middleTexture;

		private Asset<Texture2D> _backPanelHighlightTexture;

		private Asset<Texture2D> _backPanelBorderTexture;

		private Color _color;

		private float _visibilityActive = 1f;

		private float _visibilityInactive = 0.4f;

		private bool _selected;

		private bool _hovered;

		public UIColoredImageButton(Asset<Texture2D> texture, bool isSmall = false)
		{
			_color = Color.White;
			_texture = texture;
			if (isSmall)
			{
				_backPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/SmallPanel", (AssetRequestMode)1);
			}
			else
			{
				_backPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", (AssetRequestMode)1);
			}
			Width.Set(_backPanelTexture.Width(), 0f);
			Height.Set(_backPanelTexture.Height(), 0f);
			_backPanelHighlightTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1);
			if (isSmall)
			{
				_backPanelBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/SmallPanelBorder", (AssetRequestMode)1);
			}
			else
			{
				_backPanelBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode)1);
			}
		}

		public void SetImage(Asset<Texture2D> texture)
		{
			_texture = texture;
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}

		public void SetImageWithoutSettingSize(Asset<Texture2D> texture)
		{
			_texture = texture;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			Vector2 position = dimensions.Position() + new Vector2(dimensions.Width, dimensions.Height) / 2f;
			spriteBatch.Draw(_backPanelTexture.get_Value(), position, null, Color.White * (base.IsMouseHovering ? _visibilityActive : _visibilityInactive), 0f, _backPanelTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
			_ = Color.White;
			if (_hovered)
			{
				spriteBatch.Draw(_backPanelBorderTexture.get_Value(), position, null, Color.White, 0f, _backPanelBorderTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
			}
			if (_selected)
			{
				spriteBatch.Draw(_backPanelHighlightTexture.get_Value(), position, null, Color.White, 0f, _backPanelHighlightTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
			}
			if (_middleTexture != null)
			{
				spriteBatch.Draw(_middleTexture.get_Value(), position, null, Color.White, 0f, _middleTexture.Size() / 2f, 1f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(_texture.get_Value(), position, null, _color, 0f, _texture.Size() / 2f, 1f, SpriteEffects.None, 0f);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			SoundEngine.PlaySound(12);
			_hovered = true;
		}

		public void SetVisibility(float whenActive, float whenInactive)
		{
			_visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
			_visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
		}

		public void SetColor(Color color)
		{
			_color = color;
		}

		public void SetMiddleTexture(Asset<Texture2D> texAsset)
		{
			_middleTexture = texAsset;
		}

		public void SetSelected(bool selected)
		{
			_selected = selected;
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			_hovered = false;
		}
	}
}
