using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class UndergroundCorruptionShader : ChromaShader
	{
		private readonly Vector4 _corruptionColor = new Vector4(Color.Purple.ToVector3() * 0.2f, 1f);

		private readonly Vector4 _flameColor = Color.Green.ToVector4();

		private readonly Vector4 _flameTipColor = Color.Yellow.ToVector4();

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 value = Vector4.Lerp(_flameColor, _flameTipColor, 0.25f);
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f, value1: _corruptionColor, value2: value);
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
				float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.3f + new Vector2(12.5f, time * 0.05f), time * 0.1f);
				dynamicNoise = Math.Max(0f, 1f - dynamicNoise * dynamicNoise * 4f * (1.2f - canvasPositionOfIndex.Y)) * canvasPositionOfIndex.Y;
				dynamicNoise = MathHelper.Clamp(dynamicNoise, 0f, 1f);
				Vector4 value = Vector4.Lerp(_flameColor, _flameTipColor, dynamicNoise);
				Vector4 vector = Vector4.Lerp(_corruptionColor, value, dynamicNoise);
				fragment.SetColor(i, vector);
			}
		}
	}
}
