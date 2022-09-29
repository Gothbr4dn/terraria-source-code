using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class UnderworldShader : ChromaShader
	{
		private readonly Vector4 _backColor;

		private readonly Vector4 _frontColor;

		private readonly float _speed;

		public UnderworldShader(Color backColor, Color frontColor, float speed)
		{
			_backColor = backColor.ToVector4();
			_frontColor = frontColor.ToVector4();
			_speed = speed;
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector = Vector4.Lerp(_backColor, _frontColor, (float)Math.Sin(time * _speed + canvasPositionOfIndex.X) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float dynamicNoise = NoiseHelper.GetDynamicNoise(fragment.GetCanvasPositionOfIndex(i) * 0.5f, time * _speed / 3f);
				Vector4 vector = Vector4.Lerp(_backColor, _frontColor, dynamicNoise);
				fragment.SetColor(i, vector);
			}
		}
	}
}
