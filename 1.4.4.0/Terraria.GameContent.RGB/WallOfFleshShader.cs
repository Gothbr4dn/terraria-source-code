using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class WallOfFleshShader : ChromaShader
	{
		private readonly Vector4 _primaryColor;

		private readonly Vector4 _secondaryColor;

		public WallOfFleshShader(Color primaryColor, Color secondaryColor)
		{
			_primaryColor = primaryColor.ToVector4();
			_secondaryColor = secondaryColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 secondaryColor = _secondaryColor;
				float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.3f, time / 5f);
				dynamicNoise = Math.Max(0f, 1f - dynamicNoise * 2f);
				secondaryColor = Vector4.Lerp(secondaryColor, _primaryColor, (float)Math.Sqrt(dynamicNoise) * 0.75f);
				fragment.SetColor(i, secondaryColor);
			}
		}
	}
}
