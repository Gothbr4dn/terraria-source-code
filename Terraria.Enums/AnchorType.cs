using System;

namespace Terraria.Enums
{
	[Flags]
	public enum AnchorType
	{
		None = 0,
		SolidTile = 1,
		SolidWithTop = 2,
		Table = 4,
		SolidSide = 8,
		Tree = 0x10,
		AlternateTile = 0x20,
		EmptyTile = 0x40,
		SolidBottom = 0x80,
		Platform = 0x100,
		PlanterBox = 0x200
	}
}
