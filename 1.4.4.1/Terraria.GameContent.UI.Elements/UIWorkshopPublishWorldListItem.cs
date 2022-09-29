using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIWorkshopPublishWorldListItem : AWorldListItem
	{
		private Asset<Texture2D> _workshopIconTexture;

		private Asset<Texture2D> _innerPanelTexture;

		private UIElement _worldIcon;

		private UIElement _publishButton;

		private int _orderInList;

		private UIState _ownerState;

		public UIWorkshopPublishWorldListItem(UIState ownerState, WorldFileData data, int orderInList)
		{
			_ownerState = ownerState;
			_orderInList = orderInList;
			_data = data;
			LoadTextures();
			InitializeAppearance();
			_worldIcon = GetIconElement();
			_worldIcon.Left.Set(4f, 0f);
			_worldIcon.VAlign = 0.5f;
			_worldIcon.OnDoubleClick += PublishButtonClick_ImportWorldToLocalFiles;
			Append(_worldIcon);
			_ = 4f;
			_publishButton = new UIIconTextButton(Language.GetText("Workshop.Publish"), Color.White, "Images/UI/Workshop/Publish");
			_publishButton.HAlign = 1f;
			_publishButton.VAlign = 1f;
			_publishButton.OnClick += PublishButtonClick_ImportWorldToLocalFiles;
			base.OnDoubleClick += PublishButtonClick_ImportWorldToLocalFiles;
			Append(_publishButton);
			_publishButton.SetSnapPoint("Publish", orderInList);
		}

		private void LoadTextures()
		{
			_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1);
			_workshopIconTexture = TextureAssets.Extra[243];
		}

		private void InitializeAppearance()
		{
			Height.Set(82f, 0f);
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

		private void PublishButtonClick_ImportWorldToLocalFiles(UIMouseEvent evt, UIElement listeningElement)
		{
			if (listeningElement == evt.Target)
			{
				Main.MenuUI.SetState(new WorkshopPublishInfoStateForWorld(_ownerState, _data));
			}
		}

		public override int CompareTo(object obj)
		{
			if (obj is UIWorkshopPublishWorldListItem uIWorkshopPublishWorldListItem)
			{
				return _orderInList.CompareTo(uIWorkshopPublishWorldListItem._orderInList);
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

		private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width, float height)
		{
			Utils.DrawSplicedPanel(spriteBatch, _innerPanelTexture.get_Value(), (int)position.X, (int)position.Y, (int)width, (int)height, 10, 10, 10, 10, Color.White);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle innerDimensions = GetInnerDimensions();
			CalculatedStyle dimensions = _worldIcon.GetDimensions();
			float num = dimensions.X + dimensions.Width;
			Color color = (_data.IsValid ? Color.White : Color.Gray);
			string worldName = _data.GetWorldName(allowCropping: true);
			Utils.DrawBorderString(spriteBatch, worldName, new Vector2(num + 6f, innerDimensions.Y + 3f), color);
			float num2 = (innerDimensions.Width - 22f - dimensions.Width - _publishButton.GetDimensions().Width) / 2f;
			float height = _publishButton.GetDimensions().Height;
			Vector2 vector = new Vector2(num + 6f, innerDimensions.Y + innerDimensions.Height - height);
			float num3 = num2;
			DrawPanel(spriteBatch, vector, num3, height);
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
			Vector2 vector2 = FontAssets.MouseText.get_Value().MeasureString(text);
			float x = vector2.X;
			float y = vector2.Y;
			float x2 = num3 * 0.5f - x * 0.5f;
			float num4 = height * 0.5f - y * 0.5f;
			Utils.DrawBorderString(spriteBatch, text, vector + new Vector2(x2, num4 + 3f), color2);
			vector.X += num3 + 5f;
			float num5 = num2;
			if (!GameCulture.FromCultureName(GameCulture.CultureName.English).IsActive)
			{
				num5 += 40f;
			}
			DrawPanel(spriteBatch, vector, num5, height);
			string textValue = Language.GetTextValue("UI.WorldSizeFormat", _data.WorldSizeName);
			Vector2 vector3 = FontAssets.MouseText.get_Value().MeasureString(textValue);
			float x3 = vector3.X;
			float y2 = vector3.Y;
			float x4 = num5 * 0.5f - x3 * 0.5f;
			float num6 = height * 0.5f - y2 * 0.5f;
			Utils.DrawBorderString(spriteBatch, textValue, vector + new Vector2(x4, num6 + 3f), Color.White);
			vector.X += num5 + 5f;
		}
	}
}
