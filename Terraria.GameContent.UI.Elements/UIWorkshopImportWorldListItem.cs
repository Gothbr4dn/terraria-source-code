using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIWorkshopImportWorldListItem : AWorldListItem
	{
		private Asset<Texture2D> _dividerTexture;

		private Asset<Texture2D> _workshopIconTexture;

		private Asset<Texture2D> _innerPanelTexture;

		private UIElement _worldIcon;

		private UIText _buttonLabel;

		private Asset<Texture2D> _buttonImportTexture;

		private int _orderInList;

		public UIState _ownerState;

		public UIWorkshopImportWorldListItem(UIState ownerState, WorldFileData data, int orderInList)
		{
			_ownerState = ownerState;
			_orderInList = orderInList;
			_data = data;
			LoadTextures();
			InitializeAppearance();
			_worldIcon = GetIconElement();
			_worldIcon.Left.Set(4f, 0f);
			_worldIcon.OnDoubleClick += ImportButtonClick_ImportWorldToLocalFiles;
			Append(_worldIcon);
			float num = 4f;
			UIImageButton uIImageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay", (AssetRequestMode)1));
			uIImageButton.VAlign = 1f;
			uIImageButton.Left.Set(num, 0f);
			uIImageButton.OnClick += ImportButtonClick_ImportWorldToLocalFiles;
			base.OnDoubleClick += ImportButtonClick_ImportWorldToLocalFiles;
			uIImageButton.OnMouseOver += PlayMouseOver;
			uIImageButton.OnMouseOut += ButtonMouseOut;
			Append(uIImageButton);
			num += 24f;
			_buttonLabel = new UIText("");
			_buttonLabel.VAlign = 1f;
			_buttonLabel.Left.Set(num, 0f);
			_buttonLabel.Top.Set(-3f, 0f);
			Append(_buttonLabel);
			uIImageButton.SetSnapPoint("Import", orderInList);
		}

		private void LoadTextures()
		{
			_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", (AssetRequestMode)1);
			_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1);
			_workshopIconTexture = TextureAssets.Extra[243];
		}

		private void InitializeAppearance()
		{
			Height.Set(96f, 0f);
			Width.Set(0f, 1f);
			SetPadding(6f);
			SetColorsToNotHovered();
		}

		private void SetColorsToHovered()
		{
			BackgroundColor = new Color(73, 94, 171);
			BorderColor = new Color(89, 116, 213);
		}

		private void SetColorsToNotHovered()
		{
			BackgroundColor = new Color(63, 82, 151) * 0.7f;
			BorderColor = new Color(89, 116, 213) * 0.7f;
		}

		private void PlayMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			_buttonLabel.SetText(Language.GetTextValue("UI.Import"));
		}

		private void ButtonMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			_buttonLabel.SetText("");
		}

		private void ImportButtonClick_ImportWorldToLocalFiles(UIMouseEvent evt, UIElement listeningElement)
		{
			if (listeningElement == evt.Target)
			{
				SoundEngine.PlaySound(10);
				Main.clrInput();
				UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Language.GetTextValue("Workshop.EnterNewNameForImportedWorld"), _data.Name, OnFinishedSettingName, GoToMainMenu, 0, allowEmpty: true);
				uIVirtualKeyboard.SetMaxInputLength(27);
				uIVirtualKeyboard.Text = _data.Name;
				Main.MenuUI.SetState(uIVirtualKeyboard);
			}
		}

		private void OnFinishedSettingName(string name)
		{
			string newDisplayName = name.Trim();
			if (SocialAPI.Workshop != null)
			{
				SocialAPI.Workshop.ImportDownloadedWorldToLocalSaves(_data, null, newDisplayName);
			}
		}

		private void GoToMainMenu()
		{
			SoundEngine.PlaySound(11);
			Main.menuMode = 0;
		}

		public override int CompareTo(object obj)
		{
			if (obj is UIWorkshopImportWorldListItem uIWorkshopImportWorldListItem)
			{
				return _orderInList.CompareTo(uIWorkshopImportWorldListItem._orderInList);
			}
			return base.CompareTo(obj);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			SetColorsToHovered();
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			SetColorsToNotHovered();
		}

		private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
		{
			spriteBatch.Draw(_innerPanelTexture.get_Value(), position, new Rectangle(0, 0, 8, _innerPanelTexture.Height()), Color.White);
			spriteBatch.Draw(_innerPanelTexture.get_Value(), new Vector2(position.X + 8f, position.Y), new Rectangle(8, 0, 8, _innerPanelTexture.Height()), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
			spriteBatch.Draw(_innerPanelTexture.get_Value(), new Vector2(position.X + width - 8f, position.Y), new Rectangle(16, 0, 8, _innerPanelTexture.Height()), Color.White);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle innerDimensions = GetInnerDimensions();
			CalculatedStyle dimensions = _worldIcon.GetDimensions();
			float num = dimensions.X + dimensions.Width;
			Color color = (_data.IsValid ? Color.White : Color.Gray);
			string worldName = _data.GetWorldName(allowCropping: true);
			if (worldName != null)
			{
				Utils.DrawBorderString(spriteBatch, worldName, new Vector2(num + 6f, dimensions.Y - 2f), color);
			}
			spriteBatch.Draw(_workshopIconTexture.get_Value(), new Vector2(GetDimensions().X + GetDimensions().Width - (float)_workshopIconTexture.Width() - 3f, GetDimensions().Y + 2f), _workshopIconTexture.Frame(), Color.White);
			spriteBatch.Draw(_dividerTexture.get_Value(), new Vector2(num, innerDimensions.Y + 21f), null, Color.White, 0f, Vector2.Zero, new Vector2((GetDimensions().X + GetDimensions().Width - num) / 8f, 1f), SpriteEffects.None, 0f);
			Vector2 vector = new Vector2(num + 6f, innerDimensions.Y + 29f);
			float num2 = 100f;
			DrawPanel(spriteBatch, vector, num2);
			string text = "";
			Color color2 = Color.White;
			switch (_data.GameMode)
			{
			case 1:
				text = Language.GetTextValue("UI.Expert");
				color2 = Main.mcColor;
				break;
			case 2:
				text = Language.GetTextValue("UI.Master");
				color2 = Main.hcColor;
				break;
			case 3:
				text = Language.GetTextValue("UI.Creative");
				color2 = Main.creativeModeColor;
				break;
			default:
				text = Language.GetTextValue("UI.Normal");
				break;
			}
			float x = FontAssets.MouseText.get_Value().MeasureString(text).X;
			float x2 = num2 * 0.5f - x * 0.5f;
			Utils.DrawBorderString(spriteBatch, text, vector + new Vector2(x2, 3f), color2);
			vector.X += num2 + 5f;
			if (_data._worldSizeName != null)
			{
				float num3 = 150f;
				if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
				{
					num3 += 40f;
				}
				DrawPanel(spriteBatch, vector, num3);
				string textValue = Language.GetTextValue("UI.WorldSizeFormat", _data.WorldSizeName);
				float x3 = FontAssets.MouseText.get_Value().MeasureString(textValue).X;
				float x4 = num3 * 0.5f - x3 * 0.5f;
				Utils.DrawBorderString(spriteBatch, textValue, vector + new Vector2(x4, 3f), Color.White);
				vector.X += num3 + 5f;
			}
			float num4 = innerDimensions.X + innerDimensions.Width - vector.X;
			DrawPanel(spriteBatch, vector, num4);
			string arg = ((!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive) ? _data.CreationTime.ToShortDateString() : _data.CreationTime.ToString("d MMMM yyyy"));
			string textValue2 = Language.GetTextValue("UI.WorldCreatedFormat", arg);
			float x5 = FontAssets.MouseText.get_Value().MeasureString(textValue2).X;
			float x6 = num4 * 0.5f - x5 * 0.5f;
			Utils.DrawBorderString(spriteBatch, textValue2, vector + new Vector2(x6, 3f), Color.White);
			vector.X += num4 + 5f;
		}
	}
}
