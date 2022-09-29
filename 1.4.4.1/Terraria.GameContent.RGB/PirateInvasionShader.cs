using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class PirateInvasionShader : ChromaShader
	{
		private readonly Vector4 _cannonBallColor;

		private readonly Vector4 _splashColor;

		private readonly Vector4 _waterColor;

		private readonly Vector4 _backgroundColor;

		public PirateInvasionShader(Color cannonBallColor, Color splashColor, Color waterColor, Color backgroundColor)
		{
			_cannonBallColor = cannonBallColor.ToVector4();
			_splashColor = splashColor.ToVector4();
			_waterColor = waterColor.ToVector4();
			_backgroundColor = backgroundColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time * 0.5f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f, value1: _waterColor, value2: _cannonBallColor);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				gridPositionOfIndex.X /= 2;
				float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 40f + time * 1f) % 40f;
				float amount = 0f;
				float num2 = num - canvasPositionOfIndex.Y / 1.2f;
				if (num > 1f)
				{
					float num3 = 1f - canvasPositionOfIndex.Y / 1.2f;
					amount = (1f - Math.Min(1f, num2 - num3)) * (1f - Math.Min(1f, num3 / 1f));
				}
				Vector4 vector = _backgroundColor;
				if (num2 > 0f)
				{
					float amount2 = Math.Max(0f, 1.2f - num2 * 4f);
					if (num2 < 0.1f)
					{
						amount2 = num2 / 0.1f;
					}
					vector = Vector4.Lerp(vector, _cannonBallColor, amount2);
					vector = Vector4.Lerp(vector, _splashColor, amount);
				}
				if (canvasPositionOfIndex.Y > 0.8f)
				{
					vector = _waterColor;
				}
				fragment.SetColor(i, vector);
			}
		}
	}
}
