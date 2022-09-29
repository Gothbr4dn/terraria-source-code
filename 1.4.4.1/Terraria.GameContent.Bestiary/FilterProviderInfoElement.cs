using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class FilterProviderInfoElement : IFilterInfoProvider, IProvideSearchFilterString, IBestiaryInfoElement
	{
		private const int framesPerRow = 16;

		private const int framesPerColumn = 5;

		private Point _filterIconFrame;

		private string _key;

		public int DisplayTextPriority { get; set; }

		public bool HideInPortraitInfo { get; set; }

		public FilterProviderInfoElement(string nameLanguageKey, int filterIconFrame)
		{
			_key = nameLanguageKey;
			_filterIconFrame.X = filterIconFrame % 16;
			_filterIconFrame.Y = filterIconFrame / 16;
		}

		public UIElement GetFilterImage()
		{
			Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow", (AssetRequestMode)1);
			return new UIImageFramed(obj, obj.Frame(16, 5, _filterIconFrame.X, _filterIconFrame.Y))
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
		}

		public string GetSearchString(ref BestiaryUICollectionInfo info)
		{
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			return Language.GetText(_key).Value;
		}

		public string GetDisplayNameKey()
		{
			return _key;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			if (HideInPortraitInfo)
			{
				return null;
			}
			if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
			{
				return null;
			}
			UIElement uIElement = new UIPanel(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Panel", (AssetRequestMode)1), null, 12, 7)
			{
				Width = new StyleDimension(-14f, 1f),
				Height = new StyleDimension(34f, 0f),
				BackgroundColor = new Color(43, 56, 101),
				BorderColor = Color.Transparent,
				Left = new StyleDimension(5f, 0f)
			};
			uIElement.SetPadding(0f);
			uIElement.PaddingRight = 5f;
			UIElement filterImage = GetFilterImage();
			filterImage.HAlign = 0f;
			filterImage.Left = new StyleDimension(5f, 0f);
			UIText element = new UIText(Language.GetText(GetDisplayNameKey()), 0.8f)
			{
				HAlign = 0f,
				Left = new StyleDimension(38f, 0f),
				TextOriginX = 0f,
				TextOriginY = 0f,
				VAlign = 0.5f,
				DynamicallyScaleDownToWidth = true
			};
			if (filterImage != null)
			{
				uIElement.Append(filterImage);
			}
			uIElement.Append(element);
			AddOnHover(uIElement);
			return uIElement;
		}

		private void AddOnHover(UIElement button)
		{
			button.OnUpdate += delegate(UIElement e)
			{
				ShowButtonName(e);
			};
		}

		private void ShowButtonName(UIElement element)
		{
			if (element.IsMouseHovering)
			{
				string textValue = Language.GetTextValue(GetDisplayNameKey());
				Main.instance.MouseText(textValue, 0, 0);
			}
		}
	}
}
