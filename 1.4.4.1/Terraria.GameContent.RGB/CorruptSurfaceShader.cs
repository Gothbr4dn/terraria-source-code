using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class CorruptSurfaceShader : ChromaShader
	{
		private readonly Vector4 _baseColor;

		private readonly Vector4 _skyColor;

		private Vector4 _lightColor;

		public CorruptSurfaceShader(Color color)
		{
			_baseColor = color.ToVector4();
			_skyColor = Vector4.Lerp(_baseColor, Color.DeepSkyBlue.ToVector4(), 0.5f);
		}

		public CorruptSurfaceShader(Color vineColor, Color skyColor)
		{
			_baseColor = vineColor.ToVector4();
			_skyColor = skyColor.ToVector4();
		}

		public override void Update(float elapsedTime)
		{
			_lightColor = Main.ColorOfTheSkies.ToVector4() * 0.75f + Vector4.One * 0.25f;
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 value = _skyColor * _lightColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time * 0.5f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f, value1: _baseColor, value2: value);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 vector = _skyColor * _lightColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float staticNoise = NoiseHelper.GetStaticNoise(gridPositionOfIndex.X);
				staticNoise = (staticNoise * 10f + time * 0.4f) % 10f;
				float num = 1f;
				if (staticNoise > 1f)
				{
					num = MathHelper.Clamp(1f - (staticNoise - 1.4f), 0f, 1f);
					staticNoise = 1f;
				}
				float num2 = (float)Math.Sin(canvasPositionOfIndex.X) * 0.3f + 0.7f;
				float num3 = staticNoise - (1f - canvasPositionOfIndex.Y);
				Vector4 vector2 = vector;
				if (num3 > 0f)
				{
					float num4 = 1f;
					if (num3 < 0.2f)
					{
						num4 = num3 * 5f;
					}
					vector2 = Vector4.Lerp(vector2, _baseColor, num4 * num);
				}
				if (canvasPositionOfIndex.Y > num2)
				{
					vector2 = _baseColor;
				}
				fragment.SetColor(i, vector2);
			}
		}
	}
}
