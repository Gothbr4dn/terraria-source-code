using Terraria.DataStructures;

namespace Terraria.Modules
{
	public class TilePlacementHooksModule
	{
		public PlacementHook check;

		public PlacementHook postPlaceEveryone;

		public PlacementHook postPlaceMyPlayer;

		public PlacementHook placeOverride;

		public TilePlacementHooksModule(TilePlacementHooksModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				check = default(PlacementHook);
				postPlaceEveryone = default(PlacementHook);
				postPlaceMyPlayer = default(PlacementHook);
				placeOverride = default(PlacementHook);
			}
			else
			{
				check = copyFrom.check;
				postPlaceEveryone = copyFrom.postPlaceEveryone;
				postPlaceMyPlayer = copyFrom.postPlaceMyPlayer;
				placeOverride = copyFrom.placeOverride;
			}
		}
	}
}
