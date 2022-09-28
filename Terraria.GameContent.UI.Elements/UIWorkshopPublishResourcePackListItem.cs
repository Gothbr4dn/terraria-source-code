using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIWorkshopPublishResourcePackListItem : UIPanel
	{
		private ResourcePack _data;

		private Asset<Texture2D> _dividerTexture;

		private Asset<Texture2D> _workshopIconTexture;

		private Asset<Texture2D> _iconBorderTexture;

		private Asset<Texture2D> _innerPanelTexture;

		private UIElement _iconArea;

		private UIElement _publishButton;

		private int _orderInList;

		private UIState _ownerState;

		private const int ICON_SIZE = 64;

		private const int ICON_BORDER_PADDING = 4;

		private const int HEIGHT_FLUFF = 10;

		private bool _canPublish;

		public UIWorkshopPublishResourcePackListItem(UIState ownerState, ResourcePack data, int orderInList, bool canBePublished)
		{
			_ownerState = ownerState;
			_orderInList = orderInList;
			_data = data;
			_canPublish = canBePublished;
			LoadTextures();
			InitializeAppearance();
			UIElement uIElement = new UIElement
			{
				Width = new StyleDimension(72f, 0f),
				Height = new StyleDimension(72f, 0f),
				Left = new StyleDimension(4f, 0f),
				VAlign = 0.5f
			};
			uIElement.OnDoubleClick += PublishButtonClick_ImportResourcePackToLocalFiles;
			UIImage element = new UIImage(data.Icon)
			{
				Width = new StyleDimension(-6f, 1f),
				Height = new StyleDimension(-6f, 1f),
				HAlign = 0.5f,
				VAlign = 0.5f,
				ScaleToFit = true,
				AllowResizingDimensions = false,
				IgnoresMouseInteraction = true
			};
			UIImage element2 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", (AssetRequestMode)1))
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				IgnoresMouseInteraction = true
			};
			uIElement.Append(element);
			uIElement.Append(element2);
			Append(uIElement);
			_iconArea = uIElement;
			_ = 4f;
			_publishButton = new UIIconTextButton(Language.GetText("Workshop.Publish"), Color.White, "Images/UI/Workshop/Publish");
			_publishButton.HAlign = 1f;
			_publishButton.VAlign = 1f;
			_publishButton.OnClick += PublishButtonClick_ImportResourcePackToLocalFiles;
			base.OnDoubleClick += PublishButtonClick_ImportResourcePackToLocalFiles;
			Append(_publishButton);
			_publishButton.SetSnapPoint("Publish", orderInList);
		}

		private void LoadTextures()
		{
			_dividerTexture = Main.Assets.Request<Texture2D>("Images/UI/Divider", (AssetRequestMode)1);
			_innerPanelTexture = Main.Assets.Request<Texture2D>("Images/UI/InnerPanelBackground", (AssetRequestMode)1);
			_iconBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", (AssetRequestMode)1);
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
			if (!_canPublish)
			{
				BorderColor = new Color(150, 150, 150) * 1f;
				BackgroundColor = Color.Lerp(BackgroundColor, new Color(120, 120, 120), 0.5f) * 1f;
			}
		}

		private void SetColorsToNotHovered()
		{
			BackgroundColor = new Color(63, 82, 151) * 0.7f;
			BorderColor = new Color(89, 116, 213) * 0.7f;
			if (!_canPublish)
			{
				BorderColor = new Color(127, 127, 127) * 0.7f;
				BackgroundColor = Color.Lerp(new Color(63, 82, 151), new Color(80, 80, 80), 0.5f) * 0.7f;
			}
		}

		private void PublishButtonClick_ImportResourcePackToLocalFiles(UIMouseEvent evt, UIElement listeningElement)
		{
			if (listeningElement == evt.Target && !TryMovingToRejectionMenuIfNeeded())
			{
				Main.MenuUI.SetState(new WorkshopPublishInfoStateForResourcePack(_ownerState, _data));
			}
		}

		private bool TryMovingToRejectionMenuIfNeeded()
		{
			if (!_canPublish)
			{
				SoundEngine.PlaySound(10);
				Main.instance.RejectionMenuInfo = new RejectionMenuInfo
				{
					TextToShow = Language.GetTextValue("Workshop.ReportIssue_CannotPublishZips"),
					ExitAction = RejectionMenuExitAction
				};
				Main.menuMode = 1000001;
				return true;
			}
			return false;
		}

		private void RejectionMenuExitAction()
		{
			SoundEngine.PlaySound(11);
			if (_ownerState == null)
			{
				Main.menuMode = 0;
				return;
			}
			Main.menuMode = 888;
			Main.MenuUI.SetState(_ownerState);
		}

		public override int CompareTo(object obj)
		{
			if (obj is UIWorkshopPublishResourcePackListItem uIWorkshopPublishResourcePackListItem)
			{
				return _orderInList.CompareTo(uIWorkshopPublishResourcePackListItem._orderInList);
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
			CalculatedStyle dimensions = _iconArea.GetDimensions();
			float num = dimensions.X + dimensions.Width;
			Color white = Color.White;
			Utils.DrawBorderString(spriteBatch, _data.Name, new Vector2(num + 8f, innerDimensions.Y + 3f), white);
			float num2 = (innerDimensions.Width - 22f - dimensions.Width - _publishButton.GetDimensions().Width) / 2f;
			float height = _publishButton.GetDimensions().Height;
			Vector2 vector = new Vector2(num + 8f, innerDimensions.Y + innerDimensions.Height - height);
			float num3 = num2;
			DrawPanel(spriteBatch, vector, num3, height);
			string textValue = Language.GetTextValue("UI.Author", _data.Author);
			Color white2 = Color.White;
			Vector2 vector2 = FontAssets.MouseText.get_Value().MeasureString(textValue);
			float x = vector2.X;
			float y = vector2.Y;
			float x2 = num3 * 0.5f - x * 0.5f;
			float num4 = height * 0.5f - y * 0.5f;
			Utils.DrawBorderString(spriteBatch, textValue, vector + new Vector2(x2, num4 + 3f), white2);
			vector.X += num3 + 5f;
			float num5 = num2;
			DrawPanel(spriteBatch, vector, num5, height);
			string textValue2 = Language.GetTextValue("UI.Version", _data.Version.GetFormattedVersion());
			Color white3 = Color.White;
			Vector2 vector3 = FontAssets.MouseText.get_Value().MeasureString(textValue2);
			float x3 = vector3.X;
			float y2 = vector3.Y;
			float x4 = num5 * 0.5f - x3 * 0.5f;
			float num6 = height * 0.5f - y2 * 0.5f;
			Utils.DrawBorderString(spriteBatch, textValue2, vector + new Vector2(x4, num6 + 3f), white3);
			vector.X += num5 + 5f;
		}
	}
}
