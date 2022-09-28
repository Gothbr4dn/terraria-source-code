using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UICreativeItemsInfiniteFilteringOptions : UIElement
	{
		private EntryFilterer<Item, IItemEntryFilter> _filterer;

		private Dictionary<UIImageFramed, IItemEntryFilter> _filtersByButtons = new Dictionary<UIImageFramed, IItemEntryFilter>();

		private Dictionary<UIImageFramed, UIElement> _iconsByButtons = new Dictionary<UIImageFramed, UIElement>();

		private const int barFramesX = 2;

		private const int barFramesY = 4;

		public event Action OnClickingOption;

		public UICreativeItemsInfiniteFilteringOptions(EntryFilterer<Item, IItemEntryFilter> filterer, string snapPointsName)
		{
			_filterer = filterer;
			int num = 40;
			int count = _filterer.AvailableFilters.Count;
			int num2 = num * count;
			Height = new StyleDimension(num, 0f);
			Width = new StyleDimension(num2, 0f);
			Top = new StyleDimension(4f, 0f);
			SetPadding(0f);
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Tabs_B", (AssetRequestMode)1);
			for (int i = 0; i < _filterer.AvailableFilters.Count; i++)
			{
				IItemEntryFilter itemEntryFilter = _filterer.AvailableFilters[i];
				val.Frame(2, 4).OffsetSize(-2, -2);
				UIImageFramed uIImageFramed = new UIImageFramed(val, val.Frame(2, 4).OffsetSize(-2, -2));
				uIImageFramed.Left.Set(num * i, 0f);
				uIImageFramed.OnClick += singleFilterButtonClick;
				uIImageFramed.OnMouseOver += button_OnMouseOver;
				uIImageFramed.SetPadding(0f);
				uIImageFramed.SetSnapPoint(snapPointsName, i);
				AddOnHover(itemEntryFilter, uIImageFramed, i);
				UIElement image = itemEntryFilter.GetImage();
				image.IgnoresMouseInteraction = true;
				image.Left = new StyleDimension(6f, 0f);
				image.HAlign = 0f;
				uIImageFramed.Append(image);
				_filtersByButtons[uIImageFramed] = itemEntryFilter;
				_iconsByButtons[uIImageFramed] = image;
				Append(uIImageFramed);
				UpdateVisuals(uIImageFramed, i);
			}
		}

		private void button_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12);
		}

		private void singleFilterButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!(evt.Target is UIImageFramed uIImageFramed) || !_filtersByButtons.TryGetValue(uIImageFramed, out var value))
			{
				return;
			}
			int num = _filterer.AvailableFilters.IndexOf(value);
			if (num != -1)
			{
				if (!_filterer.ActiveFilters.Contains(value))
				{
					_filterer.ActiveFilters.Clear();
				}
				_filterer.ToggleFilter(num);
				UpdateVisuals(uIImageFramed, num);
				if (this.OnClickingOption != null)
				{
					this.OnClickingOption();
				}
			}
		}

		private void UpdateVisuals(UIImageFramed button, int indexOfFilter)
		{
			bool flag = _filterer.IsFilterActive(indexOfFilter);
			bool isMouseHovering = button.IsMouseHovering;
			int frameX = flag.ToInt();
			int frameY = flag.ToInt() * 2 + isMouseHovering.ToInt();
			button.SetFrame(2, 4, frameX, frameY, -2, -2);
			if (_iconsByButtons[button] is IColorable colorable)
			{
				colorable.Color = (flag ? Color.White : (Color.White * 0.5f));
			}
		}

		private void AddOnHover(IItemEntryFilter filter, UIElement button, int indexOfFilter)
		{
			button.OnUpdate += delegate(UIElement element)
			{
				ShowButtonName(element, filter, indexOfFilter);
			};
			button.OnUpdate += delegate
			{
				UpdateVisuals(button as UIImageFramed, indexOfFilter);
			};
		}

		private void ShowButtonName(UIElement element, IItemEntryFilter number, int indexOfFilter)
		{
			if (element.IsMouseHovering)
			{
				string textValue = Language.GetTextValue(number.GetDisplayNameKey());
				Main.instance.MouseText(textValue, 0, 0);
			}
		}
	}
}
