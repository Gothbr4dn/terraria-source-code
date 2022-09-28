using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class LavaIndicatorShader : ChromaShader
	{
		private readonly Vector4 _backgroundColor;

		private readonly Vector4 _primaryColor;

		private readonly Vector4 _secondaryColor;

		public LavaIndicatorShader(Color backgroundColor, Color primaryColor, Color secondaryColor)
		{
			_backgroundColor = backgroundColor.ToVector4();
			_primaryColor = primaryColor.ToVector4();
			_secondaryColor = secondaryColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float staticNoise = NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(i) * 0.3f + new Vector2(12.5f, time * 0.2f));
				staticNoise = Math.Max(0f, 1f - staticNoise * staticNoise * 4f * staticNoise);
				staticNoise = MathHelper.Clamp(staticNoise, 0f, 1f);
				Vector4 vector = Vector4.Lerp(_primaryColor, _secondaryColor, staticNoise);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = _backgroundColor;
				float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.2f, time * 0.5f);
				float num = 0.4f;
				num += dynamicNoise * 0.4f;
				float num2 = 1.1f - canvasPositionOfIndex.Y;
				if (num2 < num)
				{
					float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.3f + new Vector2(12.5f, time * 0.2f));
					staticNoise = Math.Max(0f, 1f - staticNoise * staticNoise * 4f * staticNoise);
					staticNoise = MathHelper.Clamp(staticNoise, 0f, 1f);
					Vector4 value = Vector4.Lerp(_primaryColor, _secondaryColor, staticNoise);
					float amount = 1f - MathHelper.Clamp((num2 - num + 0.2f) / 0.2f, 0f, 1f);
					vector = Vector4.Lerp(vector, value, amount);
				}
				fragment.SetColor(i, vector);
			}
		}
	}
}
