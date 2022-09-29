using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class TempleShader : ChromaShader
	{
		private readonly Vector4 _backgroundColor = new Vector4(0.05f, 0.025f, 0f, 1f);

		private readonly Vector4 _glowColor = Color.Orange.ToVector4();

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector4 vector = _backgroundColor;
				float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y * 7) * 10f + time) % 10f - (canvasPositionOfIndex.X + 2f);
				if (num > 0f)
				{
					float amount = Math.Max(0f, 1.2f - num);
					if (num < 0.2f)
					{
						amount = num * 5f;
					}
					vector = Vector4.Lerp(vector, _glowColor, amount);
				}
				fragment.SetColor(i, vector);
			}
		}
	}
}
