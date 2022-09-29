using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	public class ColorSlidersSet
	{
		public float Hue;

		public float Saturation;

		public float Luminance;

		public float Alpha = 1f;

		public void SetHSL(Color color)
		{
			Vector3 vector = Main.rgbToHsl(color);
			Hue = vector.X;
			Saturation = vector.Y;
			Luminance = vector.Z;
		}

		public void SetHSL(Vector3 vector)
		{
			Hue = vector.X;
			Saturation = vector.Y;
			Luminance = vector.Z;
		}

		public Color GetColor()
		{
			Color result = Main.hslToRgb(Hue, Saturation, Luminance);
			result.A = (byte)(Alpha * 255f);
			return result;
		}

		public Vector3 GetHSLVector()
		{
			return new Vector3(Hue, Saturation, Luminance);
		}

		public void ApplyToMainLegacyBars()
		{
			Main.hBar = Hue;
			Main.sBar = Saturation;
			Main.lBar = Luminance;
			Main.aBar = Alpha;
		}
	}
}
