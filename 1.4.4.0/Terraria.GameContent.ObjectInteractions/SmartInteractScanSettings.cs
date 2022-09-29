using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	public struct SmartInteractScanSettings
	{
		public Player player;

		public bool DemandOnlyZeroDistanceTargets;

		public bool FullInteraction;

		public Vector2 mousevec;

		public int LX;

		public int HX;

		public int LY;

		public int HY;
	}
}
