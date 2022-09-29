namespace Terraria.DataStructures
{
	public struct WingStats
	{
		public static readonly WingStats Default;

		public int FlyTime;

		public float AccRunSpeedOverride;

		public float AccRunAccelerationMult;

		public bool HasDownHoverStats;

		public float DownHoverSpeedOverride;

		public float DownHoverAccelerationMult;

		public WingStats(int flyTime = 100, float flySpeedOverride = -1f, float accelerationMultiplier = 1f, bool hasHoldDownHoverFeatures = false, float hoverFlySpeedOverride = -1f, float hoverAccelerationMultiplier = 1f)
		{
			FlyTime = flyTime;
			AccRunSpeedOverride = flySpeedOverride;
			AccRunAccelerationMult = accelerationMultiplier;
			HasDownHoverStats = hasHoldDownHoverFeatures;
			DownHoverSpeedOverride = hoverFlySpeedOverride;
			DownHoverAccelerationMult = hoverAccelerationMultiplier;
		}

		public WingStats WithSpeedBoost(float multiplier)
		{
			return new WingStats(FlyTime, AccRunSpeedOverride * multiplier, AccRunAccelerationMult, HasDownHoverStats, DownHoverSpeedOverride * multiplier, DownHoverAccelerationMult);
		}
	}
}
