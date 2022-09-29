using System.Collections.Generic;
using Terraria.ObjectData;

namespace Terraria.Modules
{
	public class TileObjectAlternatesModule
	{
		public List<TileObjectData> data;

		public TileObjectAlternatesModule(TileObjectAlternatesModule copyFrom = null)
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
			for (int i = 0; i < copyFrom.data.Count; i++)
			{
				data.Add(new TileObjectData(copyFrom.data[i]));
			}
		}
	}
}
