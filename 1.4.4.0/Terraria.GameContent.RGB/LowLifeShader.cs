using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class LowLifeShader : ChromaShader
	{
		private static Vector4 _baseColor = new Color(40, 0, 8, 255).ToVector4();

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessAnyDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			float num = (float)Math.Cos(time * MathF.PI) * 0.3f + 0.7f;
			Vector4 vector = _baseColor * num;
			vector.W = _baseColor.W;
			for (int i = 0; i < fragment.Count; i++)
			{
				fragment.SetColor(i, vector);
			}
		}
	}
}
