namespace Terraria.DataStructures
{
	public struct PortableStoolUsage
	{
		public bool HasAStool;

		public bool IsInUse;

		public int HeightBoost;

		public int VisualYOffset;

		public int MapYOffset;

		public void Reset()
		{
			HasAStool = false;
			IsInUse = false;
			HeightBoost = 0;
			VisualYOffset = 0;
			MapYOffset = 0;
		}

		public void SetStats(int heightBoost, int visualYOffset, int mapYOffset)
		{
			HasAStool = true;
			HeightBoost = heightBoost;
			VisualYOffset = visualYOffset;
			MapYOffset = mapYOffset;
		}
	}
}
