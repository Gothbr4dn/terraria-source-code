using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class DrippingShader : ChromaShader
	{
		private readonly Vector4 _baseColor;

		private readonly Vector4 _liquidColor;

		private readonly float _viscosity;

		public DrippingShader(Color baseColor, Color liquidColor, float viscosity = 1f)
		{
			_baseColor = baseColor.ToVector4();
			_liquidColor = liquidColor.ToVector4();
			_viscosity = viscosity;
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time * 0.5f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f, value1: _baseColor, value2: _liquidColor);
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
				float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * new Vector2(0.7f * _viscosity, 0.075f) + new Vector2(0f, time * -0.1f * _viscosity));
				staticNoise = Math.Max(0f, 1f - (canvasPositionOfIndex.Y * 4.5f + 0.5f) * staticNoise);
				Vector4 baseColor = _baseColor;
				baseColor = Vector4.Lerp(baseColor, _liquidColor, staticNoise);
				fragment.SetColor(i, baseColor);
			}
		}
	}
}
