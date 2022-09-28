using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class UndergroundHallowShader : ChromaShader
	{
		private readonly Vector4 _baseColor = new Color(0.05f, 0.05f, 0.05f).ToVector4();

		private readonly Vector4 _pinkCrystalColor = Color.HotPink.ToVector4();

		private readonly Vector4 _blueCrystalColor = Color.DeepSkyBlue.ToVector4();

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time * 2f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f, value1: _pinkCrystalColor, value2: _blueCrystalColor);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 baseColor = _baseColor;
				float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.4f, time * 0.05f);
				dynamicNoise = Math.Max(0f, 1f - 2.5f * dynamicNoise);
				float dynamicNoise2 = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.4f + new Vector2(0.05f, 0f), time * 0.05f);
				dynamicNoise2 = Math.Max(0f, 1f - 2.5f * dynamicNoise2);
				baseColor = ((!(dynamicNoise > dynamicNoise2)) ? Vector4.Lerp(baseColor, _blueCrystalColor, dynamicNoise2) : Vector4.Lerp(baseColor, _pinkCrystalColor, dynamicNoise));
				fragment.SetColor(i, baseColor);
			}
		}
	}
}
