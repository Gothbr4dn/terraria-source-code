using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIDifficultyButton : UIElement
	{
		private readonly Player _player;

		private readonly Asset<Texture2D> _BasePanelTexture;

		private readonly Asset<Texture2D> _selectedBorderTexture;

		private readonly Asset<Texture2D> _hoveredBorderTexture;

		private readonly byte _difficulty;

		private readonly Color _color;

		private bool _hovered;

		private bool _soundedHover;

		public UIDifficultyButton(Player player, LocalizedText title, LocalizedText description, byte difficulty, Color color)
		{
			_player = player;
			_difficulty = difficulty;
			Width = StyleDimension.FromPixels(44f);
			Height = StyleDimension.FromPixels(110f);
			_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale", (AssetRequestMode)1);
			_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1);
			_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode)1);
			_color = color;
			UIText element = new UIText(title, 0.9f)
			{
				HAlign = 0.5f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
				Top = StyleDimension.FromPixels(5f)
			};
			Append(element);
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
			CalculatedStyle dimensions = GetDimensions();
			int num = 7;
			if (dimensions.Height < 30f)
			{
				num = 5;
			}
			int num2 = 10;
			int num3 = 10;
			bool num4 = _difficulty == _player.difficulty;
			Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.get_Value(), (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num2, num2, num3, num3, Color.Lerp(Color.Black, _color, 0.8f) * 0.5f);
			if (num4)
			{
				Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.get_Value(), (int)dimensions.X + num, (int)dimensions.Y + num - 2, (int)dimensions.Width - num * 2, (int)dimensions.Height - num * 2, num2, num2, num3, num3, Color.Lerp(_color, Color.White, 0.7f) * 0.5f);
			}
			if (_hovered)
			{
				Utils.DrawSplicedPanel(spriteBatch, _hoveredBorderTexture.get_Value(), (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, num2, num2, num3, num3, Color.White);
			}
		}

		public override void MouseDown(UIMouseEvent evt)
		{
			_player.difficulty = _difficulty;
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
