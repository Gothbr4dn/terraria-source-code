using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.DataStructures
{
	public class EntryFilterer<T, U> where T : new()where U : IEntryFilter<T>
	{
		public List<U> AvailableFilters;

		public List<U> ActiveFilters;

		public List<U> AlwaysActiveFilters;

		private ISearchFilter<T> _searchFilter;

		private ISearchFilter<T> _searchFilterFromConstructor;

		public EntryFilterer()
		{
			AvailableFilters = new List<U>();
			ActiveFilters = new List<U>();
			AlwaysActiveFilters = new List<U>();
		}

		public void AddFilters(List<U> filters)
		{
			AvailableFilters.AddRange(filters);
		}

		public bool FitsFilter(T entry)
		{
			if (_searchFilter != null && !_searchFilter.FitsFilter(entry))
			{
				return false;
			}
			for (int i = 0; i < AlwaysActiveFilters.Count; i++)
			{
				if (!AlwaysActiveFilters[i].FitsFilter(entry))
				{
					return false;
				}
			}
			if (ActiveFilters.Count == 0)
			{
				return true;
			}
			for (int j = 0; j < ActiveFilters.Count; j++)
			{
				if (ActiveFilters[j].FitsFilter(entry))
				{
					return true;
				}
			}
			return false;
		}

		public void ToggleFilter(int filterIndex)
		{
			U item = AvailableFilters[filterIndex];
			if (ActiveFilters.Contains(item))
			{
				ActiveFilters.Remove(item);
			}
			else
			{
				ActiveFilters.Add(item);
			}
		}

		public bool IsFilterActive(int filterIndex)
		{
			if (!AvailableFilters.IndexInRange(filterIndex))
			{
				return false;
			}
			U item = AvailableFilters[filterIndex];
			return ActiveFilters.Contains(item);
		}

		public void SetSearchFilterObject<Z>(Z searchFilter) where Z : ISearchFilter<T>, U
		{
			_searchFilterFromConstructor = searchFilter;
		}

		public void SetSearchFilter(string searchFilter)
		{
			if (string.IsNullOrWhiteSpace(searchFilter))
			{
				_searchFilter = null;
				return;
			}
			_searchFilter = _searchFilterFromConstructor;
			_searchFilter.SetSearch(searchFilter);
		}

		public string GetDisplayName()
		{
			object obj = new { ActiveFilters.Count };
			return Language.GetTextValueWith("BestiaryInfo.Filters", obj);
		}
	}
}
