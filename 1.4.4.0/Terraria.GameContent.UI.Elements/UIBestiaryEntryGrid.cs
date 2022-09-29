using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIBestiaryEntryGrid : UIElement
	{
		private List<BestiaryEntry> _workingSetEntries;

		private MouseEvent _clickOnEntryEvent;

		private int _atEntryIndex;

		private int _lastEntry;

		public event Action OnGridContentsChanged;

		public UIBestiaryEntryGrid(List<BestiaryEntry> workingSet, MouseEvent clickOnEntryEvent)
		{
			Width = new StyleDimension(0f, 1f);
			Height = new StyleDimension(0f, 1f);
			_workingSetEntries = workingSet;
			_clickOnEntryEvent = clickOnEntryEvent;
			SetPadding(0f);
			UpdateEntries();
			FillBestiarySpaceWithEntries();
		}

		public void UpdateEntries()
		{
			_lastEntry = _workingSetEntries.Count;
		}

		public void FillBestiarySpaceWithEntries()
		{
			RemoveAllChildren();
			UpdateEntries();
			GetEntriesToShow(out var maxEntriesWidth, out var maxEntriesHeight, out var maxEntriesToHave);
			FixBestiaryRange(0, maxEntriesToHave);
			int atEntryIndex = _atEntryIndex;
			int num = Math.Min(_lastEntry, atEntryIndex + maxEntriesToHave);
			List<BestiaryEntry> list = new List<BestiaryEntry>();
			for (int i = atEntryIndex; i < num; i++)
			{
				list.Add(_workingSetEntries[i]);
			}
			int num2 = 0;
			float num3 = 0.5f / (float)maxEntriesWidth;
			float num4 = 0.5f / (float)maxEntriesHeight;
			for (int j = 0; j < maxEntriesHeight; j++)
			{
				for (int k = 0; k < maxEntriesWidth; k++)
				{
					if (num2 >= list.Count)
					{
						break;
					}
					UIElement uIElement = new UIBestiaryEntryButton(list[num2], isAPrettyPortrait: false);
					num2++;
					uIElement.OnClick += _clickOnEntryEvent;
					uIElement.VAlign = (uIElement.HAlign = 0.5f);
					uIElement.Left.Set(0f, (float)k / (float)maxEntriesWidth - 0.5f + num3);
					uIElement.Top.Set(0f, (float)j / (float)maxEntriesHeight - 0.5f + num4);
					uIElement.SetSnapPoint("Entries", num2, new Vector2(0.2f, 0.7f));
					Append(uIElement);
				}
			}
		}

		public override void Recalculate()
		{
			base.Recalculate();
			FillBestiarySpaceWithEntries();
		}

		public void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
		{
			Rectangle rectangle = GetDimensions().ToRectangle();
			maxEntriesWidth = rectangle.Width / 72;
			maxEntriesHeight = rectangle.Height / 72;
			int num = 0;
			maxEntriesToHave = maxEntriesWidth * maxEntriesHeight - num;
		}

		public string GetRangeText()
		{
			GetEntriesToShow(out var _, out var _, out var maxEntriesToHave);
			int atEntryIndex = _atEntryIndex;
			int num = Math.Min(_lastEntry, atEntryIndex + maxEntriesToHave);
			int num2 = Math.Min(atEntryIndex + 1, num);
			return $"{num2}-{num} ({_lastEntry})";
		}

		public void MakeButtonGoByOffset(UIElement element, int howManyPages)
		{
			element.OnClick += delegate
			{
				OffsetLibraryByPages(howManyPages);
			};
		}

		public void OffsetLibraryByPages(int howManyPages)
		{
			GetEntriesToShow(out var _, out var _, out var maxEntriesToHave);
			OffsetLibrary(howManyPages * maxEntriesToHave);
		}

		public void OffsetLibrary(int offset)
		{
			GetEntriesToShow(out var _, out var _, out var maxEntriesToHave);
			FixBestiaryRange(offset, maxEntriesToHave);
			FillBestiarySpaceWithEntries();
		}

		private void FixBestiaryRange(int offset, int maxEntriesToHave)
		{
			_atEntryIndex = Utils.Clamp(_atEntryIndex + offset, 0, Math.Max(0, _lastEntry - maxEntriesToHave));
			if (this.OnGridContentsChanged != null)
			{
				this.OnGridContentsChanged();
			}
		}
	}
}
