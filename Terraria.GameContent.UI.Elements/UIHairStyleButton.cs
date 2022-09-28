using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIHairStyleButton : UIImageButton
	{
		private readonly Player _player;

		public readonly int HairStyleId;

		private readonly Asset<Texture2D> _selectedBorderTexture;

		private readonly Asset<Texture2D> _hoveredBorderTexture;

		private bool _hovered;

		private bool _soundedHover;

		private int _framesToSkip;

		public UIHairStyleButton(Player player, int hairStyleId)
			: base(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", (AssetRequestMode)1))
		{
			_player = player;
			HairStyleId = hairStyleId;
			Width = StyleDimension.FromPixels(44f);
			Height = StyleDimension.FromPixels(44f);
			_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1);
			_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode)1);
		}

		public void SkipRenderingContent(int timeInFrames)
		{
			_framesToSkip = timeInFrames;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (_hovered)
			{
				if (!_soundedHover)
				{
					SoundEngine.PlaySound(12);
				}
				_soundedHover = true;
			}
			else
			{
				_soundedHover = false;
			}
			Vector2 vector = new Vector2(-5f, -5f);
			base.DrawSelf(spriteBatch);
			if (_player.hair == HairStyleId)
			{
				spriteBatch.Draw(_selectedBorderTexture.get_Value(), GetDimensions().Center() - _selectedBorderTexture.Size() / 2f, Color.White);
			}
			if (_hovered)
			{
				spriteBatch.Draw(_hoveredBorderTexture.get_Value(), GetDimensions().Center() - _hoveredBorderTexture.Size() / 2f, Color.White);
			}
			if (_framesToSkip > 0)
			{
				_framesToSkip--;
				return;
			}
			int hair = _player.hair;
			_player.hair = HairStyleId;
			Main.PlayerRenderer.DrawPlayerHead(Main.Camera, _player, GetDimensions().Center() + vector);
			_player.hair = hair;
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			_player.hair = HairStyleId;
			SoundEngine.PlaySound(12);
			base.MouseDown(evt);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			_hovered = true;
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			_hovered = false;
		}
	}
}
