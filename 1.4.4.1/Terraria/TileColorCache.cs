namespace Terraria
{
	public struct TileColorCache
	{
		public byte Color;

		public bool FullBright;

		public bool Invisible;

		public void ApplyToBlock(Tile tile)
		{
			tile.color(Color);
			tile.fullbrightBlock(FullBright);
			tile.invisibleBlock(Invisible);
		}

		public void ApplyToWall(Tile tile)
		{
			tile.wallColor(Color);
			tile.fullbrightWall(FullBright);
			tile.invisibleWall(Invisible);
		}
	}
}
