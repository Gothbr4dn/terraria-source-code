using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class QueenBeeShader : ChromaShader
	{
		private readonly Vector4 _primaryColor;

		private readonly Vector4 _secondaryColor;

		public QueenBeeShader(Color primaryColor, Color secondaryColor)
		{
			_primaryColor = primaryColor.ToVector4();
			_secondaryColor = secondaryColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time * 2f + fragment.GetCanvasPositionOfIndex(i).X * 10f) * 0.5f + 0.5f, value1: _primaryColor, value2: _secondaryColor);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			time *= 0.5f;
			for (int i = 0; i < fragment.Count; i++)
			{
				float amount = MathHelper.Clamp((float)Math.Sin((double)fragment.GetCanvasPositionOfIndex(i).X * 5.0 - (double)(4f * time)) * 1.5f, 0f, 1f);
				Vector4 vector = Vector4.Lerp(_primaryColor, _secondaryColor, amount);
				fragment.SetColor(i, vector);
			}
		}
	}
}
