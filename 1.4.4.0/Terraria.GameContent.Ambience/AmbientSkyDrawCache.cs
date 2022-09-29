namespace Terraria.GameContent.Ambience
{
	public class AmbientSkyDrawCache
	{
		public struct UnderworldCache
		{
			public float Scale;
		}

		public struct OceanLineCache
		{
			public float YScreenPosition;

			public float OceanOpacity;
		}

		public static AmbientSkyDrawCache Instance = new AmbientSkyDrawCache();

		public UnderworldCache[] Underworld = new UnderworldCache[5];

		public OceanLineCache OceanLineInfo;

		public void SetUnderworldInfo(int drawIndex, float scale)
		{
			Underworld[drawIndex] = new UnderworldCache
			{
				Scale = scale
			};
		}

		public void SetOceanLineInfo(float yScreenPosition, float oceanOpacity)
		{
			OceanLineInfo = new OceanLineCache
			{
				YScreenPosition = yScreenPosition,
				OceanOpacity = oceanOpacity
			};
		}
	}
}
