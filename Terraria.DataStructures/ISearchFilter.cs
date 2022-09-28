namespace Terraria.DataStructures
{
	public interface ISearchFilter<T> : IEntryFilter<T>
	{
		void SetSearch(string searchText);
	}
}
