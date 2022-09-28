namespace Terraria.GameContent.UI.BigProgressBar
{
	public struct BigProgressBarCache
	{
		public float LifeCurrent;

		public float LifeMax;

		public float ShieldCurrent;

		public float ShieldMax;

		public void SetLife(float current, float max)
		{
			LifeCurrent = current;
			LifeMax = max;
		}

		public void SetShield(float current, float max)
		{
			ShieldCurrent = current;
			ShieldMax = max;
		}
	}
}
