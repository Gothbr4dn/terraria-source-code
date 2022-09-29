using Newtonsoft.Json;

namespace Terraria.Utilities.Terraria.Utilities
{
	public struct FloatRange
	{
		[JsonProperty("Min")]
		public readonly float Minimum;

		[JsonProperty("Max")]
		public readonly float Maximum;

		public FloatRange(float minimum, float maximum)
		{
			Minimum = minimum;
			Maximum = maximum;
		}

		public static FloatRange operator *(FloatRange range, float scale)
		{
			return new FloatRange(range.Minimum * scale, range.Maximum * scale);
		}

		public static FloatRange operator *(float scale, FloatRange range)
		{
			return range * scale;
		}

		public static FloatRange operator /(FloatRange range, float scale)
		{
			return new FloatRange(range.Minimum / scale, range.Maximum / scale);
		}

		public static FloatRange operator /(float scale, FloatRange range)
		{
			return range / scale;
		}
	}
}
