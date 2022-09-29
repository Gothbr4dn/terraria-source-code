using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class MartianMadnessShader : ChromaShader
	{
		private readonly Vector4 _metalColor;

		private readonly Vector4 _glassColor;

		private readonly Vector4 _beamColor;

		private readonly Vector4 _backgroundColor;

		public MartianMadnessShader(Color metalColor, Color glassColor, Color beamColor, Color backgroundColor)
		{
			_metalColor = metalColor.ToVector4();
			_glassColor = glassColor.ToVector4();
			_beamColor = beamColor.ToVector4();
			_backgroundColor = backgroundColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				float amount = (float)Math.Sin(time * 2f + canvasPositionOfIndex.X * 5f) * 0.5f + 0.5f;
				int num = (gridPositionOfIndex.X + gridPositionOfIndex.Y) % 2;
				if (num < 0)
				{
					num += 2;
				}
				Vector4 vector = ((num == 1) ? Vector4.Lerp(_glassColor, _beamColor, amount) : _metalColor);
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
			float num = time * 0.5f % (MathF.PI * 2f);
			if (num > MathF.PI)
			{
				num = MathF.PI * 2f - num;
			}
			Vector2 vector = new Vector2(1.7f + (float)Math.Cos(num) * 2f, -0.5f + (float)Math.Sin(num) * 1.1f);
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Vector4 vector2 = _backgroundColor;
				float num2 = Math.Abs(vector.X - canvasPositionOfIndex.X);
				if (canvasPositionOfIndex.Y > vector.Y && num2 < 0.2f)
				{
					float num3 = 1f - MathHelper.Clamp((num2 - 0.2f + 0.2f) / 0.2f, 0f, 1f);
					float num4 = Math.Abs((num - MathF.PI / 2f) / (MathF.PI / 2f));
					num4 = Math.Max(0f, 1f - num4 * 3f);
					vector2 = Vector4.Lerp(vector2, _beamColor, num3 * num4);
				}
				Vector2 vector3 = vector - canvasPositionOfIndex;
				vector3.X /= 1f;
				vector3.Y /= 0.2f;
				float num5 = vector3.Length();
				if (num5 < 1f)
				{
					float amount = 1f - MathHelper.Clamp((num5 - 1f + 0.2f) / 0.2f, 0f, 1f);
					vector2 = Vector4.Lerp(vector2, _metalColor, amount);
				}
				Vector2 vector4 = vector - canvasPositionOfIndex + new Vector2(0f, -0.1f);
				vector4.X /= 0.3f;
				vector4.Y /= 0.3f;
				if (vector4.Y < 0f)
				{
					vector4.Y *= 2f;
				}
				float num6 = vector4.Length();
				if (num6 < 1f)
				{
					float amount2 = 1f - MathHelper.Clamp((num6 - 1f + 0.2f) / 0.2f, 0f, 1f);
					vector2 = Vector4.Lerp(vector2, _glassColor, amount2);
				}
				fragment.SetColor(i, vector2);
			}
		}
	}
}
