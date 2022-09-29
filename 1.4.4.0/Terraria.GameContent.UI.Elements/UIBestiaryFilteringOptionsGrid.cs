using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIBestiaryFilteringOptionsGrid : UIPanel
	{
		private EntryFilterer<BestiaryEntry, IBestiaryEntryFilter> _filterer;

		private List<GroupOptionButton<int>> _filterButtons;

		private List<bool> _areFiltersAvailable;

		private List<List<BestiaryEntry>> _filterAvailabilityTests;

		private UIElement _container;

		public event Action OnClickingOption;

		public UIBestiaryFilteringOptionsGrid(EntryFilterer<BestiaryEntry, IBestiaryEntryFilter> filterer)
		{
			_filterer = filterer;
			_filterButtons = new List<GroupOptionButton<int>>();
			_areFiltersAvailable = new List<bool>();
			_filterAvailabilityTests = new List<List<BestiaryEntry>>();
			Width = new StyleDimension(0f, 1f);
			Height = new StyleDimension(0f, 1f);
			BackgroundColor = new Color(35, 40, 83) * 0.5f;
			BorderColor = new Color(35, 40, 83) * 0.5f;
			IgnoresMouseInteraction = false;
			SetPadding(0f);
			BuildContainer();
		}

		private void BuildContainer()
		{
			GetDisplaySettings(out var _, out var _, out var widthWithSpacing, out var heightWithSpacing, out var perRow, out var _, out var _, out var howManyRows);
			UIPanel uIPanel = new UIPanel
			{
				Width = new StyleDimension(perRow * widthWithSpacing + 10, 0f),
				Height = new StyleDimension(howManyRows * heightWithSpacing + 10, 0f),
				HAlign = 1f,
				VAlign = 0f,
				Left = new StyleDimension(0f, 0f),
				Top = new StyleDimension(0f, 0f)
			};
			uIPanel.BorderColor = new Color(89, 116, 213, 255) * 0.9f;
			uIPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
			uIPanel.SetPadding(0f);
			Append(uIPanel);
			_container = uIPanel;
		}

		public void SetupAvailabilityTest(List<BestiaryEntry> allAvailableEntries)
		{
			_filterAvailabilityTests.Clear();
			for (int i = 0; i < _filterer.AvailableFilters.Count; i++)
			{
				List<BestiaryEntry> list = new List<BestiaryEntry>();
				_filterAvailabilityTests.Add(list);
				IBestiaryEntryFilter bestiaryEntryFilter = _filterer.AvailableFilters[i];
				for (int j = 0; j < allAvailableEntries.Count; j++)
				{
					if (bestiaryEntryFilter.FitsFilter(allAvailableEntries[j]))
					{
						list.Add(allAvailableEntries[j]);
					}
				}
			}
		}

		public void UpdateAvailability()
		{
			GetDisplaySettings(out var widthPerButton, out var heightPerButton, out var widthWithSpacing, out var heightWithSpacing, out var perRow, out var offsetLeft, out var offsetTop, out var _);
			_container.RemoveAllChildren();
			_filterButtons.Clear();
			_areFiltersAvailable.Clear();
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < _filterer.AvailableFilters.Count; i++)
			{
				int num3 = i / perRow;
				int num4 = i % perRow;
				IBestiaryEntryFilter bestiaryEntryFilter = _filterer.AvailableFilters[i];
				List<BestiaryEntry> entries = _filterAvailabilityTests[i];
				if (GetIsFilterAvailableForEntries(bestiaryEntryFilter, entries))
				{
					GroupOptionButton<int> groupOptionButton = new GroupOptionButton<int>(i, null, null, Color.White, null)
					{
						Width = new StyleDimension(widthPerButton, 0f),
						Height = new StyleDimension(heightPerButton, 0f),
						HAlign = 0f,
						VAlign = 0f,
						Top = new StyleDimension(offsetTop + (float)(num3 * heightWithSpacing), 0f),
						Left = new StyleDimension(offsetLeft + (float)(num4 * widthWithSpacing), 0f)
					};
					groupOptionButton.OnClick += ClickOption;
					groupOptionButton.SetSnapPoint("Filters", i);
					groupOptionButton.ShowHighlightWhenSelected = false;
					AddOnHover(bestiaryEntryFilter, groupOptionButton);
					_container.Append(groupOptionButton);
					UIElement image = bestiaryEntryFilter.GetImage();
					if (image != null)
					{
						image.Left = new StyleDimension(num, 0f);
						image.Top = new StyleDimension(num2, 0f);
						groupOptionButton.Append(image);
					}
					_filterButtons.Add(groupOptionButton);
				}
				else
				{
					_filterer.ActiveFilters.Remove(bestiaryEntryFilter);
					GroupOptionButton<int> groupOptionButton2 = new GroupOptionButton<int>(-2, null, null, Color.White, null)
					{
						Width = new StyleDimension(widthPerButton, 0f),
						Height = new StyleDimension(heightPerButton, 0f),
						HAlign = 0f,
						VAlign = 0f,
						Top = new StyleDimension(offsetTop + (float)(num3 * heightWithSpacing), 0f),
						Left = new StyleDimension(offsetLeft + (float)(num4 * widthWithSpacing), 0f),
						FadeFromBlack = 0.5f
					};
					groupOptionButton2.ShowHighlightWhenSelected = false;
					groupOptionButton2.SetPadding(0f);
					groupOptionButton2.SetSnapPoint("Filters", i);
					Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Tags_Shadow", (AssetRequestMode)1);
					UIImageFramed uIImageFramed = new UIImageFramed(obj, obj.Frame(16, 5, 0, 4))
					{
						HAlign = 0.5f,
						VAlign = 0.5f,
						Color = Color.White * 0.2f
					};
					uIImageFramed.Left = new StyleDimension(num, 0f);
					uIImageFramed.Top = new StyleDimension(num2, 0f);
					groupOptionButton2.Append(uIImageFramed);
					_filterButtons.Add(groupOptionButton2);
					_container.Append(groupOptionButton2);
				}
			}
			UpdateButtonSelections();
		}

		public void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
		{
			GetDisplaySettings(out var _, out var _, out var _, out var _, out var perRow, out var _, out var _, out var howManyRows);
			maxEntriesWidth = perRow;
			maxEntriesHeight = howManyRows;
			maxEntriesToHave = _filterer.AvailableFilters.Count;
		}

		private void GetDisplaySettings(out int widthPerButton, out int heightPerButton, out int widthWithSpacing, out int heightWithSpacing, out int perRow, out float offsetLeft, out float offsetTop, out int howManyRows)
		{
			widthPerButton = 32;
			heightPerButton = 32;
			int num = 2;
			widthWithSpacing = widthPerButton + num;
			heightWithSpacing = heightPerButton + num;
			perRow = (int)Math.Ceiling(Math.Sqrt(_filterer.AvailableFilters.Count));
			perRow = 12;
			howManyRows = (int)Math.Ceiling((float)_filterer.AvailableFilters.Count / (float)perRow);
			offsetLeft = (float)(perRow * widthWithSpacing - num) * 0.5f;
			offsetTop = (float)(howManyRows * heightWithSpacing - num) * 0.5f;
			offsetLeft = 6f;
			offsetTop = 6f;
		}

		private void UpdateButtonSelections()
		{
			foreach (GroupOptionButton<int> filterButton in _filterButtons)
			{
				bool flag = _filterer.IsFilterActive(filterButton.OptionValue);
				filterButton.SetCurrentOption(flag ? filterButton.OptionValue : (-1));
				if (flag)
				{
					filterButton.SetColor(new Color(152, 175, 235), 1f);
				}
				else
				{
					filterButton.SetColor(Colors.InventoryDefaultColor, 0.7f);
				}
			}
		}

		private bool GetIsFilterAvailableForEntries(IBestiaryEntryFilter filter, List<BestiaryEntry> entries)
		{
			bool? forcedDisplay = filter.ForcedDisplay;
			if (forcedDisplay.HasValue)
			{
				return forcedDisplay.Value;
			}
			for (int i = 0; i < entries.Count; i++)
			{
				if (filter.FitsFilter(entries[i]) && entries[i].UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0)
				{
					return true;
				}
			}
			return false;
		}

		private void AddOnHover(IBestiaryEntryFilter filter, UIElement button)
		{
			button.OnUpdate += delegate(UIElement element)
			{
				ShowButtonName(element, filter);
			};
		}

		private void ShowButtonName(UIElement element, IBestiaryEntryFilter number)
		{
			if (element.IsMouseHovering)
			{
				string textValue = Language.GetTextValue(number.GetDisplayNameKey());
				Main.instance.MouseText(textValue, 0, 0);
			}
		}

		private void ClickOption(UIMouseEvent evt, UIElement listeningElement)
		{
			int optionValue = ((GroupOptionButton<int>)listeningElement).OptionValue;
			_filterer.ToggleFilter(optionValue);
			UpdateButtonSelections();
			if (this.OnClickingOption != null)
			{
				this.OnClickingOption();
			}
		}
	}
}
