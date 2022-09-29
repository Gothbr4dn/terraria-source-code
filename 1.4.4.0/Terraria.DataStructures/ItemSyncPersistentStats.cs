using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	public struct ItemSyncPersistentStats
	{
		private Color color;

		private int type;

		public void CopyFrom(Item item)
		{
			type = item.type;
			color = item.color;
		}

		public void PasteInto(Item item)
		{
			if (type == item.type)
			{
				item.color = color;
			}
		}
	}
}
