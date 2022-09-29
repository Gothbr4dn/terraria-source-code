using System.Collections.Generic;
using Terraria.ObjectData;

namespace Terraria.Modules
{
	public class TileObjectSubTilesModule
	{
		public List<TileObjectData> data;

		public TileObjectSubTilesModule(TileObjectSubTilesModule copyFrom = null, List<TileObjectData> newData = null)
		{
			if (copyFrom == null)
			{
				data = null;
				return;
			}
			if (copyFrom.data == null)
			{
				data = null;
				return;
			}
			data = new List<TileObjectData>(copyFrom.data.Count);
			for (int i = 0; i < data.Count; i++)
			{
				data.Add(new TileObjectData(copyFrom.data[i]));
			}
		}
	}
}
