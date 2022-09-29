using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIBestiaryEntryInfoPage : UIPanel
	{
		private enum BestiaryInfoCategory
		{
			Nameplate,
			Portrait,
			FlavorText,
			Stats,
			ItemsFromCatchingNPC,
			ItemsFromDrops,
			Misc
		}

		private UIList _list;

		private UIScrollbar _scrollbar;

		private bool _isScrollbarAttached;

		public UIBestiaryEntryInfoPage()
		{
			Width.Set(230f, 0f);
			Height.Set(0f, 1f);
			SetPadding(0f);
			BorderColor = new Color(89, 116, 213, 255);
			BackgroundColor = new Color(73, 94, 171);
			UIList uIList = new UIList
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f)
			};
			uIList.SetPadding(2f);
			uIList.PaddingBottom = 4f;
			uIList.PaddingTop = 4f;
			Append(uIList);
			_list = uIList;
			uIList.ListPadding = 4f;
			uIList.ManualSortMethod = ManualIfnoSortingMethod;
			UIScrollbar uIScrollbar = new UIScrollbar();
			uIScrollbar.SetView(100f, 1000f);
			uIScrollbar.Height.Set(-20f, 1f);
			uIScrollbar.HAlign = 1f;
			uIScrollbar.VAlign = 0.5f;
			uIScrollbar.Left.Set(-6f, 0f);
			_scrollbar = uIScrollbar;
			_list.SetScrollbar(_scrollbar);
			CheckScrollBar();
			AppendBorderOverEverything();
		}

		public void UpdateScrollbar(int scrollWheelValue)
		{
			if (_scrollbar != null)
			{
				_scrollbar.ViewPosition -= scrollWheelValue;
			}
		}

		private void AppendBorderOverEverything()
		{
			UIPanel uIPanel = new UIPanel
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				IgnoresMouseInteraction = true
			};
			uIPanel.BorderColor = new Color(89, 116, 213, 255);
			uIPanel.BackgroundColor = Color.Transparent;
			Append(uIPanel);
		}

		private void ManualIfnoSortingMethod(List<UIElement> list)
		{
		}

		public override void Recalculate()
		{
			base.Recalculate();
			CheckScrollBar();
		}

		private void CheckScrollBar()
		{
			if (_scrollbar != null)
			{
				bool canScroll = _scrollbar.CanScroll;
				canScroll = true;
				if (_isScrollbarAttached && !canScroll)
				{
					RemoveChild(_scrollbar);
					_isScrollbarAttached = false;
					_list.Width.Set(0f, 1f);
				}
				else if (!_isScrollbarAttached && canScroll)
				{
					Append(_scrollbar);
					_isScrollbarAttached = true;
					_list.Width.Set(-20f, 1f);
				}
			}
		}

		public void FillInfoForEntry(BestiaryEntry entry, ExtraBestiaryInfoPageInformation extraInfo)
		{
			_list.Clear();
			if (entry != null)
			{
				AddInfoToList(entry, extraInfo);
				Recalculate();
			}
		}

		private BestiaryUICollectionInfo GetUICollectionInfo(BestiaryEntry entry, ExtraBestiaryInfoPageInformation extraInfo)
		{
			BestiaryUICollectionInfo result = entry.UIInfoProvider?.GetEntryUICollectionInfo() ?? default(BestiaryUICollectionInfo);
			result.OwnerEntry = entry;
			return result;
		}

		private void AddInfoToList(BestiaryEntry entry, ExtraBestiaryInfoPageInformation extraInfo)
		{
			BestiaryUICollectionInfo uICollectionInfo = GetUICollectionInfo(entry, extraInfo);
			IOrderedEnumerable<IGrouping<BestiaryInfoCategory, IBestiaryInfoElement>> orderedEnumerable = from x in new List<IBestiaryInfoElement>(entry.Info).GroupBy(GetBestiaryInfoCategory)
				orderby x.Key
				select x;
			UIElement item = null;
			foreach (IGrouping<BestiaryInfoCategory, IBestiaryInfoElement> item2 in orderedEnumerable)
			{
				if (item2.Count() == 0)
				{
					continue;
				}
				bool flag = false;
				foreach (IBestiaryInfoElement item3 in item2.OrderByDescending(GetIndividualElementPriority))
				{
					UIElement uIElement = item3.ProvideUIElement(uICollectionInfo);
					if (uIElement != null)
					{
						_list.Add(uIElement);
						flag = true;
					}
				}
				if (flag)
				{
					UIHorizontalSeparator uIHorizontalSeparator = new UIHorizontalSeparator
					{
						Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
						Color = new Color(89, 116, 213, 255) * 0.9f
					};
					_list.Add(uIHorizontalSeparator);
					item = uIHorizontalSeparator;
				}
			}
			_list.Remove(item);
		}

		private float GetIndividualElementPriority(IBestiaryInfoElement element)
		{
			if (element is IBestiaryPrioritizedElement bestiaryPrioritizedElement)
			{
				return bestiaryPrioritizedElement.OrderPriority;
			}
			return 0f;
		}

		private BestiaryInfoCategory GetBestiaryInfoCategory(IBestiaryInfoElement element)
		{
			if (element is NPCPortraitInfoElement)
			{
				return BestiaryInfoCategory.Portrait;
			}
			if (element is FlavorTextBestiaryInfoElement)
			{
				return BestiaryInfoCategory.FlavorText;
			}
			if (element is NamePlateInfoElement)
			{
				return BestiaryInfoCategory.Nameplate;
			}
			if (element is ItemFromCatchingNPCBestiaryInfoElement)
			{
				return BestiaryInfoCategory.ItemsFromCatchingNPC;
			}
			if (element is ItemDropBestiaryInfoElement)
			{
				return BestiaryInfoCategory.ItemsFromDrops;
			}
			if (element is NPCStatsReportInfoElement || element is NPCKillCounterInfoElement)
			{
				return BestiaryInfoCategory.Stats;
			}
			return BestiaryInfoCategory.Misc;
		}
	}
}
