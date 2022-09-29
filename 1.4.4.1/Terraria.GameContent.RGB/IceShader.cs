using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class IceShader : ChromaShader
	{
		private readonly Vector4 _baseColor;

		private readonly Vector4 _iceColor;

		private readonly Vector4 _shineColor = new Vector4(1f, 1f, 0.7f, 1f);

		public IceShader(Color baseColor, Color iceColor)
		{
			_baseColor = baseColor.ToVector4();
			_iceColor = iceColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float dynamicNoise = NoiseHelper.GetDynamicNoise(new Vector2((canvasPositionOfIndex.X - canvasPositionOfIndex.Y) * 0.2f, 0f), time / 5f);
				dynamicNoise = Math.Max(0f, 1f - dynamicNoise * 1.5f);
				float dynamicNoise2 = NoiseHelper.GetDynamicNoise(new Vector2((canvasPositionOfIndex.X - canvasPositionOfIndex.Y) * 0.3f, 0.3f), time / 20f);
				dynamicNoise2 = Math.Max(0f, 1f - dynamicNoise2 * 5f);
				Vector4 value = Vector4.Lerp(_baseColor, _iceColor, dynamicNoise);
				value = Vector4.Lerp(value, _shineColor, dynamicNoise2);
				fragment.SetColor(i, value);
			}
		}
	}
}
