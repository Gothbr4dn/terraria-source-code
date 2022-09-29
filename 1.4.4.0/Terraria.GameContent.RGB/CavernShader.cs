using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class CavernShader : ChromaShader
	{
		private readonly Vector4 _backColor;

		private readonly Vector4 _frontColor;

		private readonly float _speed;

		public CavernShader(Color backColor, Color frontColor, float speed)
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
			time *= _speed * 0.5f;
			float num = time % 1f;
			bool num2 = time % 2f > 1f;
			Vector4 vector = (num2 ? _frontColor : _backColor);
			Vector4 value = (num2 ? _backColor : _frontColor);
			num *= 1.2f;
			for (int i = 0; i < fragment.Count; i++)
			{
				float staticNoise = NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(i) * 0.5f + new Vector2(0f, time * 0.5f));
				Vector4 vector2 = vector;
				staticNoise += num;
				if (staticNoise > 0.999f)
				{
					float amount = MathHelper.Clamp((staticNoise - 0.999f) / 0.2f, 0f, 1f);
					vector2 = Vector4.Lerp(vector2, value, amount);
				}
				fragment.SetColor(i, vector2);
			}
		}
	}
}
