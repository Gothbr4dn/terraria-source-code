using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class DeathShader : ChromaShader
	{
		private readonly Vector4 _primaryColor;

		private readonly Vector4 _secondaryColor;

		public DeathShader(Color primaryColor, Color secondaryColor)
		{
			_primaryColor = primaryColor.ToVector4();
			_secondaryColor = secondaryColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			time *= 3f;
			float amount = 0f;
			float num = time % (MathF.PI * 4f);
			if (num < MathF.PI)
			{
				amount = (float)Math.Sin(num);
			}
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(_primaryColor, _secondaryColor, amount);
				fragment.SetColor(i, vector);
			}
		}
	}
}
