using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class GroupOptionButton<T> : UIElement, IGroupOptionButton
	{
		private T _currentOption;

		private readonly Asset<Texture2D> _BasePanelTexture;

		private readonly Asset<Texture2D> _selectedBorderTexture;

		private readonly Asset<Texture2D> _hoveredBorderTexture;

		private readonly Asset<Texture2D> _iconTexture;

		private readonly T _myOption;

		private Color _color;

		private Color _borderColor;

		public float FadeFromBlack = 1f;

		private float _whiteLerp = 0.7f;

		private float _opacity = 0.7f;

		private bool _hovered;

		private bool _soundedHover;

		public bool ShowHighlightWhenSelected = true;

		private bool _UseOverrideColors;

		private Color _overrideUnpickedColor = Color.White;

		private Color _overridePickedColor = Color.White;

		private float _overrideOpacityPicked;

		private float _overrideOpacityUnpicked;

		public readonly LocalizedText Description;

		private UIText _title;

		public T OptionValue => _myOption;

		public bool IsSelected
		{
			get
			{
				if (_currentOption != null)
				{
					return _currentOption.Equals(_myOption);
				}
				return false;
			}
		}

		public GroupOptionButton(T option, LocalizedText title, LocalizedText description, Color textColor, string iconTexturePath, float textSize = 1f, float titleAlignmentX = 0.5f, float titleWidthReduction = 10f)
		{
			_borderColor = Color.White;
			_currentOption = option;
			_myOption = option;
			Description = description;
			Width = StyleDimension.FromPixels(44f);
			Height = StyleDimension.FromPixels(34f);
			_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/PanelGrayscale", (AssetRequestMode)1);
			_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1);
			_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode)1);
			if (iconTexturePath != null)
			{
				_iconTexture = Main.Assets.Request<Texture2D>(iconTexturePath, (AssetRequestMode)1);
			}
			_color = Colors.InventoryDefaultColor;
			if (title != null)
			{
				UIText uIText = new UIText(title, textSize)
				{
					HAlign = titleAlignmentX,
					VAlign = 0.5f,
					Width = StyleDimension.FromPixelsAndPercent(0f - titleWidthReduction, 1f),
					Top = StyleDimension.FromPixels(0f)
				};
				uIText.TextColor = textColor;
				Append(uIText);
				_title = uIText;
			}
		}

		public void SetText(LocalizedText text, float textSize, Color color)
		{
			if (_title != null)
			{
				_title.Remove();
			}
			UIText uIText = new UIText(text, textSize)
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
				Top = StyleDimension.FromPixels(0f)
			};
			uIText.TextColor = color;
			Append(uIText);
			_title = uIText;
		}

		public void SetCurrentOption(T option)
		{
			_currentOption = option;
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
			Color color = _color;
			float num = _opacity;
			bool isSelected = IsSelected;
			if (_UseOverrideColors)
			{
				color = (isSelected ? _overridePickedColor : _overrideUnpickedColor);
				num = (isSelected ? _overrideOpacityPicked : _overrideOpacityUnpicked);
			}
			Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.get_Value(), (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.Lerp(Color.Black, color, FadeFromBlack) * num);
			if (isSelected && ShowHighlightWhenSelected)
			{
				Utils.DrawSplicedPanel(spriteBatch, _selectedBorderTexture.get_Value(), (int)dimensions.X + 7, (int)dimensions.Y + 7, (int)dimensions.Width - 14, (int)dimensions.Height - 14, 10, 10, 10, 10, Color.Lerp(color, Color.White, _whiteLerp) * num);
			}
			if (_hovered)
			{
				Utils.DrawSplicedPanel(spriteBatch, _hoveredBorderTexture.get_Value(), (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, _borderColor);
			}
			if (_iconTexture != null)
			{
				Color color2 = Color.White;
				if (!_hovered && !isSelected)
				{
					color2 = Color.Lerp(color, Color.White, _whiteLerp) * num;
				}
				spriteBatch.Draw(_iconTexture.get_Value(), new Vector2(dimensions.X + 1f, dimensions.Y + 1f), color2);
			}
		}

		public override void MouseDown(UIMouseEvent evt)
		{
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

		public void SetColor(Color color, float opacity)
		{
			_color = color;
			_opacity = opacity;
		}

		public void SetColorsBasedOnSelectionState(Color pickedColor, Color unpickedColor, float opacityPicked, float opacityNotPicked)
		{
			_UseOverrideColors = true;
			_overridePickedColor = pickedColor;
			_overrideUnpickedColor = unpickedColor;
			_overrideOpacityPicked = opacityPicked;
			_overrideOpacityUnpicked = opacityNotPicked;
		}

		public void SetBorderColor(Color color)
		{
			_borderColor = color;
		}
	}
}
