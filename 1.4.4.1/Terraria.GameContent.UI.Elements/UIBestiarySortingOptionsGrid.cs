using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIBestiarySortingOptionsGrid : UIPanel
	{
		private EntrySorter<BestiaryEntry, IBestiarySortStep> _sorter;

		private List<GroupOptionButton<int>> _buttonsBySorting;

		private int _currentSelected = -1;

		private int _defaultStepIndex;

		public event Action OnClickingOption;

		public UIBestiarySortingOptionsGrid(EntrySorter<BestiaryEntry, IBestiarySortStep> sorter)
		{
			_sorter = sorter;
			_buttonsBySorting = new List<GroupOptionButton<int>>();
			Width = new StyleDimension(0f, 1f);
			Height = new StyleDimension(0f, 1f);
			BackgroundColor = new Color(35, 40, 83) * 0.5f;
			BorderColor = new Color(35, 40, 83) * 0.5f;
			IgnoresMouseInteraction = false;
			SetPadding(0f);
			BuildGrid();
		}

		private void BuildGrid()
		{
			int num = 2;
			int num2 = 26 + num;
			int num3 = 0;
			for (int i = 0; i < _sorter.Steps.Count; i++)
			{
				if (!_sorter.Steps[i].HiddenFromSortOptions)
				{
					num3++;
				}
			}
			UIPanel uIPanel = new UIPanel
			{
				Width = new StyleDimension(126f, 0f),
				Height = new StyleDimension(num3 * num2 + 5 + 3, 0f),
				HAlign = 1f,
				VAlign = 0f,
				Left = new StyleDimension(-118f, 0f),
				Top = new StyleDimension(0f, 0f)
			};
			uIPanel.BorderColor = new Color(89, 116, 213, 255) * 0.9f;
			uIPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
			uIPanel.SetPadding(0f);
			Append(uIPanel);
			int num4 = 0;
			for (int j = 0; j < _sorter.Steps.Count; j++)
			{
				IBestiarySortStep bestiarySortStep = _sorter.Steps[j];
				if (!bestiarySortStep.HiddenFromSortOptions)
				{
					GroupOptionButton<int> groupOptionButton = new GroupOptionButton<int>(j, Language.GetText(bestiarySortStep.GetDisplayNameKey()), null, Color.White, null, 0.8f)
					{
						Width = new StyleDimension(114f, 0f),
						Height = new StyleDimension(num2 - num, 0f),
						HAlign = 0.5f,
						Top = new StyleDimension(5 + num2 * num4, 0f)
					};
					groupOptionButton.ShowHighlightWhenSelected = false;
					groupOptionButton.OnClick += ClickOption;
					groupOptionButton.SetSnapPoint("SortSteps", num4);
					uIPanel.Append(groupOptionButton);
					_buttonsBySorting.Add(groupOptionButton);
					num4++;
				}
			}
			foreach (GroupOptionButton<int> item in _buttonsBySorting)
			{
				item.SetCurrentOption(-1);
			}
		}

		private void ClickOption(UIMouseEvent evt, UIElement listeningElement)
		{
			int num = ((GroupOptionButton<int>)listeningElement).OptionValue;
			if (num == _currentSelected)
			{
				num = _defaultStepIndex;
			}
			foreach (GroupOptionButton<int> item in _buttonsBySorting)
			{
				bool flag = num == item.OptionValue;
				item.SetCurrentOption(flag ? num : (-1));
				if (flag)
				{
					item.SetColor(new Color(152, 175, 235), 1f);
				}
				else
				{
					item.SetColor(Colors.InventoryDefaultColor, 0.7f);
				}
			}
			_currentSelected = num;
			_sorter.SetPrioritizedStepIndex(num);
			if (this.OnClickingOption != null)
			{
				this.OnClickingOption();
			}
		}

		public void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
		{
			maxEntriesWidth = 1;
			maxEntriesHeight = _buttonsBySorting.Count;
			maxEntriesToHave = _buttonsBySorting.Count;
		}
	}
}
