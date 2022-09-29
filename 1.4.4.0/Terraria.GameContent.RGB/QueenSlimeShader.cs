using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class QueenSlimeShader : ChromaShader
	{
		private readonly Vector4 _slimeColor;

		private readonly Vector4 _debrisColor;

		public QueenSlimeShader(Color slimeColor, Color debrisColor)
		{
			_slimeColor = slimeColor.ToVector4();
			_debrisColor = debrisColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float dynamicNoise = NoiseHelper.GetDynamicNoise(fragment.GetCanvasPositionOfIndex(i), time * 0.25f);
				dynamicNoise = Math.Max(0f, 1f - dynamicNoise * 2f);
				Vector4 vector = Vector4.Lerp(_slimeColor, _debrisColor, dynamicNoise);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			new Vector2(1.6f, 0.5f);
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 slimeColor = _slimeColor;
				float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.3f + new Vector2(0f, time * 0.1f));
				staticNoise = Math.Max(0f, 1f - staticNoise * 3f);
				staticNoise = (float)Math.Sqrt(staticNoise);
				slimeColor = Vector4.Lerp(slimeColor, _debrisColor, staticNoise);
				fragment.SetColor(i, slimeColor);
			}
		}
	}
}
