using Terraria.DataStructures;

namespace Terraria.GameContent.Bestiary
{
	public interface IBestiaryEntryFilter : IEntryFilter<BestiaryEntry>
	{
		bool? ForcedDisplay { get; }
	}
}
