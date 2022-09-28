using Newtonsoft.Json;

namespace Terraria.GameContent.Metadata
{
	public class TileGolfPhysics
	{
		[JsonProperty]
		public float DirectImpactDampening { get; private set; }

		[JsonProperty]
		public float SideImpactDampening { get; private set; }

		[JsonProperty]
		public float ClubImpactDampening { get; private set; }

		[JsonProperty]
		public float PassThroughDampening { get; private set; }

		[JsonProperty]
		public float ImpactDampeningResistanceEfficiency { get; private set; }
	}
}
