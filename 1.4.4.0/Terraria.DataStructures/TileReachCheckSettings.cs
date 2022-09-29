namespace Terraria.DataStructures
{
	public struct TileReachCheckSettings
	{
		public int TileRangeMultiplier;

		public int? TileReachLimit;

		public int? OverrideXReach;

		public int? OverrideYReach;

		public static TileReachCheckSettings Simple
		{
			get
			{
				TileReachCheckSettings result = default(TileReachCheckSettings);
				result.TileRangeMultiplier = 1;
				result.TileReachLimit = 20;
				return result;
			}
		}

		public static TileReachCheckSettings Pylons
		{
			get
			{
				TileReachCheckSettings result = default(TileReachCheckSettings);
				result.OverrideXReach = 60;
				result.OverrideYReach = 60;
				return result;
			}
		}

		public void GetRanges(Player player, out int x, out int y)
		{
			x = Player.tileRangeX * TileRangeMultiplier;
			y = Player.tileRangeY * TileRangeMultiplier;
			int? tileReachLimit = TileReachLimit;
			if (tileReachLimit.HasValue)
			{
				int value = tileReachLimit.Value;
				if (x > value)
				{
					x = value;
				}
				if (y > value)
				{
					y = value;
				}
			}
			if (OverrideXReach.HasValue)
			{
				x = OverrideXReach.Value;
			}
			if (OverrideYReach.HasValue)
			{
				y = OverrideYReach.Value;
			}
		}
	}
}
