using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class GolemShader : ChromaShader
	{
		private readonly Vector4 _glowColor;

		private readonly Vector4 _coreColor;

		private readonly Vector4 _backgroundColor;

		public GolemShader(Color glowColor, Color coreColor, Color backgroundColor)
		{
			_glowColor = glowColor.ToVector4();
			_coreColor = coreColor.ToVector4();
			_backgroundColor = backgroundColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 value = Vector4.Lerp(_backgroundColor, _coreColor, Math.Max(0f, (float)Math.Sin(time * 0.5f)));
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: Math.Max(0f, (float)Math.Sin(fragment.GetCanvasPositionOfIndex(i).X * 2f + time + 101f)), value1: value, value2: _glowColor);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			float num = 0.5f + (float)Math.Sin(time * 3f) * 0.1f;
			Vector2 vector = new Vector2(1.6f, 0.5f);
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector4 vector2 = _backgroundColor;
				float num2 = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y) * 10f + time * 2f) % 10f - Math.Abs(canvasPositionOfIndex.X - vector.X);
				if (num2 > 0f)
				{
					float amount = Math.Max(0f, 1.2f - num2);
					if (num2 < 0.2f)
					{
						amount = num2 * 5f;
					}
					vector2 = Vector4.Lerp(vector2, _glowColor, amount);
				}
				float num3 = (canvasPositionOfIndex - vector).Length();
				if (num3 < num)
				{
					float amount2 = 1f - MathHelper.Clamp((num3 - num + 0.1f) / 0.1f, 0f, 1f);
					vector2 = Vector4.Lerp(vector2, _coreColor, amount2);
				}
				fragment.SetColor(i, vector2);
			}
		}
	}
}
