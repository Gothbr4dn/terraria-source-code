using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class PlanteraShader : ChromaShader
	{
		private readonly Vector4 _bulbColor;

		private readonly Vector4 _vineColor;

		private readonly Vector4 _backgroundColor;

		public PlanteraShader(Color bulbColor, Color vineColor, Color backgroundColor)
		{
			_bulbColor = bulbColor.ToVector4();
			_vineColor = vineColor.ToVector4();
			_backgroundColor = backgroundColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time * 2f + fragment.GetCanvasPositionOfIndex(i).X * 10f) * 0.5f + 0.5f, value1: _bulbColor, value2: _vineColor);
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
				canvasPositionOfIndex.X -= 1.8f;
				if (canvasPositionOfIndex.X < 0f)
				{
					canvasPositionOfIndex.X *= -1f;
					gridPositionOfIndex.Y += 101;
				}
				float staticNoise = NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y);
				staticNoise = (staticNoise * 5f + time * 0.4f) % 5f;
				float num = 1f;
				if (staticNoise > 1f)
				{
					num = 1f - MathHelper.Clamp((staticNoise - 0.4f - 1f) / 0.4f, 0f, 1f);
					staticNoise = 1f;
				}
				float num2 = staticNoise - canvasPositionOfIndex.X / 5f;
				Vector4 vector = _backgroundColor;
				if (num2 > 0f)
				{
					float num3 = 1f;
					if (num2 < 0.2f)
					{
						num3 = num2 / 0.2f;
					}
					vector = (((gridPositionOfIndex.X + 7 * gridPositionOfIndex.Y) % 5 != 0) ? Vector4.Lerp(_backgroundColor, _vineColor, num3 * num) : Vector4.Lerp(_backgroundColor, _bulbColor, num3 * num));
				}
				fragment.SetColor(i, vector);
			}
		}
	}
}
