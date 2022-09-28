using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Terraria.Utilities;

namespace Terraria.WorldBuilding
{
	public class WorldGenRange
	{
		public enum ScalingMode
		{
			None,
			WorldArea,
			WorldWidth
		}

		public static readonly WorldGenRange Empty = new WorldGenRange(0, 0);

		[JsonProperty("Min")]
		public readonly int Minimum;

		[JsonProperty("Max")]
		public readonly int Maximum;

		[JsonProperty]
		[JsonConverter(typeof(StringEnumConverter))]
		public readonly ScalingMode ScaleWith;

		public int ScaledMinimum => ScaleValue(Minimum);

		public int ScaledMaximum => ScaleValue(Maximum);

		public WorldGenRange(int minimum, int maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		public int GetRandom(UnifiedRandom random)
		{
			return random.Next(ScaledMinimum, ScaledMaximum + 1);
		}

		private int ScaleValue(int value)
		{
			double num = 1.0;
			switch (ScaleWith)
			{
			case ScalingMode.WorldArea:
				num = (double)(Main.maxTilesX * Main.maxTilesY) / 5040000.0;
				break;
			case ScalingMode.WorldWidth:
				num = (double)Main.maxTilesX / 4200.0;
				break;
			case ScalingMode.None:
				num = 1.0;
				break;
			}
			return (int)(num * (double)value);
		}
	}
}
