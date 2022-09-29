using Newtonsoft.Json;

namespace Terraria.GameContent.Metadata
{
	public class TileMaterial
	{
		[JsonProperty]
		public TileGolfPhysics GolfPhysics { get; private set; }
	}
}
