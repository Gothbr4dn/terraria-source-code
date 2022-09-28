using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class EyeOfCthulhuShader : ChromaShader
	{
		private readonly Vector4 _eyeColor;

		private readonly Vector4 _veinColor;

		private readonly Vector4 _backgroundColor;

		public EyeOfCthulhuShader(Color eyeColor, Color veinColor, Color backgroundColor)
		{
			_eyeColor = eyeColor.ToVector4();
			_veinColor = veinColor.ToVector4();
			_backgroundColor = backgroundColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time + fragment.GetCanvasPositionOfIndex(i).X * 4f) * 0.5f + 0.5f, value1: _veinColor, value2: _eyeColor);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if ((int)device.Type != 0 && (int)device.Type != 6)
			{
				ProcessLowDetail(device, fragment, quality, time);
				return;
			}
			float num = time * 0.2f % 2f;
			int num2 = 1;
			if (num > 1f)
			{
				num = 2f - num;
				num2 = -1;
			}
			Vector2 vector = new Vector2(num * 7f - 3.5f, 0f) + fragment.get_CanvasCenter();
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector2 = _backgroundColor;
				Vector2 vector3 = canvasPositionOfIndex - vector;
				float num3 = vector3.Length();
				if (num3 < 0.5f)
				{
					float amount = 1f - MathHelper.Clamp((num3 - 0.5f + 0.2f) / 0.2f, 0f, 1f);
					float num4 = MathHelper.Clamp((vector3.X + 0.5f - 0.2f) / 0.6f, 0f, 1f);
					if (num2 == 1)
					{
						num4 = 1f - num4;
					}
					Vector4 value = Vector4.Lerp(_eyeColor, _veinColor, num4);
					vector2 = Vector4.Lerp(vector2, value, amount);
				}
				fragment.SetColor(i, vector2);
			}
		}
	}
}
