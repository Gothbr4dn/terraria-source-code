using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UICharacterNameButton : UIElement
	{
		private readonly Asset<Texture2D> _BasePanelTexture;

		private readonly Asset<Texture2D> _selectedBorderTexture;

		private readonly Asset<Texture2D> _hoveredBorderTexture;

		private bool _hovered;

		private bool _soundedHover;

		private readonly LocalizedText _textToShowWhenEmpty;

		private string actualContents;

		private UIText _text;

		private UIText _title;

		public readonly LocalizedText Description;

		public float DistanceFromTitleToOption = 20f;

		public UICharacterNameButton(LocalizedText titleText, LocalizedText emptyContentText, LocalizedText description = null)
		{
			Width = StyleDimension.FromPixels(400f);
			Height = StyleDimension.FromPixels(40f);
			Description = description;
			_BasePanelTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", (AssetRequestMode)1);
			_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1);
			_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder", (AssetRequestMode)1);
			_textToShowWhenEmpty = emptyContentText;
			float textScale = 1f;
			UIText uIText = new UIText(titleText, textScale)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = StyleDimension.FromPixels(10f)
			};
			Append(uIText);
			_title = uIText;
			UIText uIText2 = new UIText(Language.GetText("UI.PlayerNameSlot"), textScale)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = StyleDimension.FromPixels(150f)
			};
			Append(uIText2);
			_text = uIText2;
			SetContents(null);
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
			Utils.DrawSplicedPanel(spriteBatch, _BasePanelTexture.get_Value(), (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.White * 0.5f);
			if (_hovered)
			{
				Utils.DrawSplicedPanel(spriteBatch, _hoveredBorderTexture.get_Value(), (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height, 10, 10, 10, 10, Color.White);
			}
		}

		public void SetContents(string name)
		{
			actualContents = name;
			if (string.IsNullOrEmpty(actualContents))
			{
				_text.TextColor = Color.Gray;
				_text.SetText(_textToShowWhenEmpty);
			}
			else
			{
				_text.TextColor = Color.White;
				_text.SetText(actualContents);
			}
			_text.Left = StyleDimension.FromPixels(_title.GetInnerDimensions().Width + DistanceFromTitleToOption);
		}

		public void TrimDisplayIfOverElementDimensions(int padding)
		{
			CalculatedStyle dimensions = GetDimensions();
			Point point = new Point((int)dimensions.X, (int)dimensions.Y);
			Point point2 = new Point(point.X + (int)dimensions.Width, point.Y + (int)dimensions.Height);
			Rectangle rectangle = new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
			CalculatedStyle dimensions2 = _text.GetDimensions();
			Point point3 = new Point((int)dimensions2.X, (int)dimensions2.Y);
			Point point4 = new Point(point3.X + (int)dimensions2.Width, point3.Y + (int)dimensions2.Height);
			Rectangle rectangle2 = new Rectangle(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
			int num = 0;
			while (rectangle2.Right > rectangle.Right - padding)
			{
				_text.SetText(_text.Text.Substring(0, _text.Text.Length - 1));
				num++;
				RecalculateChildren();
				dimensions2 = _text.GetDimensions();
				point3 = new Point((int)dimensions2.X, (int)dimensions2.Y);
				point4 = new Point(point3.X + (int)dimensions2.Width, point3.Y + (int)dimensions2.Height);
				rectangle2 = new Rectangle(point3.X, point3.Y, point4.X - point3.X, point4.Y - point3.Y);
			}
			if (num > 0)
			{
				_text.SetText(_text.Text.Substring(0, _text.Text.Length - 1) + "…");
			}
		}

		public override void MouseDown(UIMouseEvent evt)
		{
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
