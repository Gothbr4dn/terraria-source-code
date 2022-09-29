using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class VineShader : ChromaShader
	{
		private readonly Vector4 _backgroundColor = new Color(46, 17, 6).ToVector4();

		private readonly Vector4 _vineColor = Color.Green.ToVector4();

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				fragment.GetCanvasPositionOfIndex(i);
				fragment.SetColor(i, _backgroundColor);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float staticNoise = NoiseHelper.GetStaticNoise(gridPositionOfIndex.X);
				staticNoise = (staticNoise * 10f + time * 0.4f) % 10f;
				float num = 1f;
				if (staticNoise > 1f)
				{
					num = 1f - MathHelper.Clamp((staticNoise - 0.4f - 1f) / 0.4f, 0f, 1f);
					staticNoise = 1f;
				}
				float num2 = staticNoise - canvasPositionOfIndex.Y / 1f;
				Vector4 vector = _backgroundColor;
				if (num2 > 0f)
				{
					float num3 = 1f;
					if (num2 < 0.2f)
					{
						num3 = num2 / 0.2f;
					}
					vector = Vector4.Lerp(_backgroundColor, _vineColor, num3 * num);
				}
				fragment.SetColor(i, vector);
			}
		}
	}
}
