using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class BrainShader : ChromaShader
	{
		private readonly Vector4 _brainColor;

		private readonly Vector4 _veinColor;

		public BrainShader(Color brainColor, Color veinColor)
		{
			_brainColor = brainColor.ToVector4();
			_veinColor = veinColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 vector = Vector4.Lerp(_brainColor, _veinColor, Math.Max(0f, (float)Math.Sin(time * 3f)));
			for (int i = 0; i < fragment.Count; i++)
			{
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			new Vector2(1.6f, 0.5f);
			Vector4 value = Vector4.Lerp(_brainColor, _veinColor, Math.Max(0f, (float)Math.Sin(time * 3f)) * 0.5f + 0.5f);
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 brainColor = _brainColor;
				float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.15f + new Vector2(time * 0.002f), time * 0.03f);
				dynamicNoise = (float)Math.Sin(dynamicNoise * 10f) * 0.5f + 0.5f;
				dynamicNoise = Math.Max(0f, 1f - 5f * dynamicNoise);
				brainColor = Vector4.Lerp(brainColor, value, dynamicNoise);
				fragment.SetColor(i, brainColor);
			}
		}
	}
}
