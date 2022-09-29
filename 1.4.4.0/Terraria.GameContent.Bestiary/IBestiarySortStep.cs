using System.Collections.Generic;
using Terraria.DataStructures;

namespace Terraria.GameContent.Bestiary
{
	public interface IBestiarySortStep : IEntrySortStep<BestiaryEntry>, IComparer<BestiaryEntry>
	{
		bool HiddenFromSortOptions { get; }
	}
}
