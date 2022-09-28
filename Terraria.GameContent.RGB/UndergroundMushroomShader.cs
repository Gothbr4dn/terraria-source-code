using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class UndergroundMushroomShader : ChromaShader
	{
		private readonly Vector4 _baseColor = new Color(10, 10, 10).ToVector4();

		private readonly Vector4 _edgeGlowColor = new Color(0, 0, 255).ToVector4();

		private readonly Vector4 _sporeColor = new Color(255, 230, 150).ToVector4();

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time * 0.5f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f, value1: _edgeGlowColor, value2: _sporeColor);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 value = _baseColor;
				float num = ((NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 10f + time * 0.2f) % 10f - (1f - canvasPositionOfIndex.Y)) * 2f;
				if (num > 0f)
				{
					float amount = Math.Max(0f, 1.5f - num);
					if (num < 0.5f)
					{
						amount = num * 2f;
					}
					value = Vector4.Lerp(value, _sporeColor, amount);
				}
				float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.3f + new Vector2(0f, time * 0.1f));
				staticNoise = Math.Max(0f, 1f - staticNoise * (1f + (1f - canvasPositionOfIndex.Y) * 4f));
				staticNoise *= Math.Max(0f, (canvasPositionOfIndex.Y - 0.3f) / 0.7f);
				value = Vector4.Lerp(value, _edgeGlowColor, staticNoise);
				fragment.SetColor(i, value);
			}
		}
	}
}
