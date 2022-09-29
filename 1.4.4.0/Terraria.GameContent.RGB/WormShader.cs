using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class WormShader : ChromaShader
	{
		private readonly Vector4 _skinColor;

		private readonly Vector4 _eyeColor;

		private readonly Vector4 _innerEyeColor;

		public WormShader()
		{
		}

		public WormShader(Color skinColor, Color eyeColor, Color innerEyeColor)
		{
			_skinColor = skinColor.ToVector4();
			_eyeColor = eyeColor.ToVector4();
			_innerEyeColor = innerEyeColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float amount = Math.Max(0f, (float)Math.Sin(time * -3f + fragment.GetCanvasPositionOfIndex(i).X));
				Vector4 vector = Vector4.Lerp(_skinColor, _eyeColor, amount);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			time *= 0.25f;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				canvasPositionOfIndex.X -= time * 1.5f;
				canvasPositionOfIndex.X %= 2f;
				if (canvasPositionOfIndex.X < 0f)
				{
					canvasPositionOfIndex.X += 2f;
				}
				float num = (canvasPositionOfIndex - new Vector2(0.5f)).Length();
				Vector4 vector = _skinColor;
				if (num < 0.5f)
				{
					float num2 = MathHelper.Clamp((num - 0.5f + 0.2f) / 0.2f, 0f, 1f);
					vector = Vector4.Lerp(vector, _eyeColor, 1f - num2);
					if (num < 0.4f)
					{
						num2 = MathHelper.Clamp((num - 0.4f + 0.2f) / 0.2f, 0f, 1f);
						vector = Vector4.Lerp(vector, _innerEyeColor, 1f - num2);
					}
				}
				fragment.SetColor(i, vector);
			}
		}
	}
}
