using Terraria.DataStructures;

namespace Terraria.Modules
{
	public class AnchorDataModule
	{
		public AnchorData top;

		public AnchorData bottom;

		public AnchorData left;

		public AnchorData right;

		public bool wall;

		public AnchorDataModule(AnchorDataModule copyFrom = null)
		{
			if (copyFrom == null)
			{
				top = default(AnchorData);
				bottom = default(AnchorData);
				left = default(AnchorData);
				right = default(AnchorData);
				wall = false;
			}
			else
			{
				top = copyFrom.top;
				bottom = copyFrom.bottom;
				left = copyFrom.left;
				right = copyFrom.right;
				wall = copyFrom.wall;
			}
		}
	}
}
