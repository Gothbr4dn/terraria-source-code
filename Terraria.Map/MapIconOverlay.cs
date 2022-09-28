using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Map
{
	public class MapIconOverlay
	{
		private readonly List<IMapLayer> _layers = new List<IMapLayer>();

		public MapIconOverlay AddLayer(IMapLayer layer)
		{
			_layers.Add(layer);
			return this;
		}

		public void Draw(Vector2 mapPosition, Vector2 mapOffset, Rectangle? clippingRect, float mapScale, float drawScale, ref string text)
		{
			MapOverlayDrawContext context = new MapOverlayDrawContext(mapPosition, mapOffset, clippingRect, mapScale, drawScale);
			foreach (IMapLayer layer in _layers)
			{
				layer.Draw(ref context, ref text);
			}
		}
	}
}
