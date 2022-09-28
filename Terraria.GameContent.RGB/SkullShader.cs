using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class SkullShader : ChromaShader
	{
		private readonly Vector4 _skullColor;

		private readonly Vector4 _bloodDark;

		private readonly Vector4 _bloodLight;

		private readonly Vector4 _backgroundColor = Color.Black.ToVector4();

		public SkullShader(Color skullColor, Color bloodDark, Color bloodLight)
		{
			_skullColor = skullColor.ToVector4();
			_bloodDark = bloodDark.ToVector4();
			_bloodLight = bloodLight.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time * 2f + fragment.GetCanvasPositionOfIndex(i).X * 2f) * 0.5f + 0.5f, value1: _skullColor, value2: _bloodLight);
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
				Vector4 value = _backgroundColor;
				float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 10f + time * 0.75f) % 10f + canvasPositionOfIndex.Y - 1f;
				if (num > 0f)
				{
					float amount = Math.Max(0f, 1.2f - num);
					if (num < 0.2f)
					{
						amount = num * 5f;
					}
					value = Vector4.Lerp(value, _skullColor, amount);
				}
				float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.5f + new Vector2(12.5f, time * 0.2f));
				staticNoise = Math.Max(0f, 1f - staticNoise * staticNoise * 4f * staticNoise * (1f - canvasPositionOfIndex.Y * canvasPositionOfIndex.Y)) * canvasPositionOfIndex.Y * canvasPositionOfIndex.Y;
				staticNoise = MathHelper.Clamp(staticNoise, 0f, 1f);
				Vector4 value2 = Vector4.Lerp(_bloodDark, _bloodLight, staticNoise);
				value = Vector4.Lerp(value, value2, staticNoise);
				fragment.SetColor(i, value);
			}
		}
	}
}
