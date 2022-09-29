using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class MeteoriteShader : ChromaShader
	{
		private readonly Vector4 _baseColor = new Color(39, 15, 26).ToVector4();

		private readonly Vector4 _secondaryColor = new Color(69, 50, 43).ToVector4();

		private readonly Vector4 _glowColor = Color.DarkOrange.ToVector4();

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f, value1: _baseColor, value2: _secondaryColor);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector4 baseColor = _baseColor;
				float dynamicNoise = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 10f);
				baseColor = Vector4.Lerp(baseColor, _secondaryColor, dynamicNoise * dynamicNoise);
				float dynamicNoise2 = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.5f + new Vector2(0f, time * 0.05f), time / 20f);
				dynamicNoise2 = Math.Max(0f, 1f - dynamicNoise2 * 2f);
				baseColor = Vector4.Lerp(baseColor, _glowColor, (float)Math.Sqrt(dynamicNoise2) * 0.75f);
				fragment.SetColor(i, baseColor);
			}
		}
	}
}
