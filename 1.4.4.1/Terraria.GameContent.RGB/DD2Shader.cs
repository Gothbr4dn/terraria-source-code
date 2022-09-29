using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class DD2Shader : ChromaShader
	{
		private readonly Vector4 _darkGlowColor;

		private readonly Vector4 _lightGlowColor;

		public DD2Shader(Color darkGlowColor, Color lightGlowColor)
		{
			_darkGlowColor = darkGlowColor.ToVector4();
			_lightGlowColor = lightGlowColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			Vector2 vector = fragment.get_CanvasCenter();
			if ((int)quality == 0)
			{
				vector = new Vector2(1.7f, 0.5f);
			}
			time *= 0.5f;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 value = new Vector4(0f, 0f, 0f, 1f);
				float num = (canvasPositionOfIndex - vector).Length();
				float num2 = num * num * 0.75f;
				float num3 = (num - time) % 1f;
				if (num3 < 0f)
				{
					num3 += 1f;
				}
				num3 = ((!(num3 > 0.8f)) ? (num3 / 0.8f) : (num3 * (1f - (num3 - 1f + 0.2f) / 0.2f)));
				Vector4 value2 = Vector4.Lerp(_darkGlowColor, _lightGlowColor, num3 * num3);
				num3 *= MathHelper.Clamp(1f - num2, 0f, 1f) * 0.75f + 0.25f;
				value = Vector4.Lerp(value, value2, num3);
				if (num < 0.5f)
				{
					float amount = 1f - MathHelper.Clamp((num - 0.5f + 0.4f) / 0.4f, 0f, 1f);
					value = Vector4.Lerp(value, _lightGlowColor, amount);
				}
				fragment.SetColor(i, value);
			}
		}
	}
}
