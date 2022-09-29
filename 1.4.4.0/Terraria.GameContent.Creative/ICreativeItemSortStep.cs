using System.Collections.Generic;
using Terraria.DataStructures;

namespace Terraria.GameContent.Creative
{
	public interface ICreativeItemSortStep : IEntrySortStep<int>, IComparer<int>
	{
	}
}
