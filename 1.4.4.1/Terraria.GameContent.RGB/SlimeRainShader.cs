using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class SlimeRainShader : ChromaShader
	{
		private static readonly Vector4[] _colors = new Vector4[3]
		{
			Color.Blue.ToVector4(),
			Color.Green.ToVector4(),
			Color.Purple.ToVector4()
		};

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 vector = new Vector4(0f, 0f, 0f, 0.75f);
			for (int i = 0; i < fragment.Count; i++)
			{
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 7f + time * 0.4f) % 7f - canvasPositionOfIndex.Y;
				Vector4 vector2 = vector;
				if (num > 0f)
				{
					float amount = Math.Max(0f, 1.2f - num);
					if (num < 0.4f)
					{
						amount = num / 0.4f;
					}
					int num2 = (gridPositionOfIndex.X % _colors.Length + _colors.Length) % _colors.Length;
					vector2 = Vector4.Lerp(vector2, _colors[num2], amount);
				}
				fragment.SetColor(i, vector2);
			}
		}
	}
}
