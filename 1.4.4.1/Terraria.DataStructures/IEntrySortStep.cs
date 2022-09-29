using System.Collections.Generic;

namespace Terraria.DataStructures
{
	public interface IEntrySortStep<T> : IComparer<T>
	{
		string GetDisplayNameKey();
	}
}
