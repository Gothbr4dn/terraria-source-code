using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class JungleShader : ChromaShader
	{
		private readonly Vector4 _backgroundColor = new Color(40, 80, 0).ToVector4();

		private readonly Vector4 _sporeColor = new Color(255, 255, 0).ToVector4();

		private readonly Vector4[] _flowerColors = new Vector4[5]
		{
			Color.Yellow.ToVector4(),
			Color.Pink.ToVector4(),
			Color.Purple.ToVector4(),
			Color.Red.ToVector4(),
			Color.Blue.ToVector4()
		};

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				float dynamicNoise = NoiseHelper.GetDynamicNoise(fragment.GetCanvasPositionOfIndex(i) * 0.3f, time / 5f);
				Vector4 vector = Vector4.Lerp(_backgroundColor, _sporeColor, dynamicNoise);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Invalid comparison between Unknown and I4
			bool flag = (int)device.Type == 0 || (int)device.Type == 6;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.3f, time / 5f);
				dynamicNoise = Math.Max(0f, 1f - dynamicNoise * 2.5f);
				Vector4 vector = Vector4.Lerp(_backgroundColor, _sporeColor, dynamicNoise);
				if (flag)
				{
					float dynamicNoise2 = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 100f);
					dynamicNoise2 = Math.Max(0f, 1f - dynamicNoise2 * 20f);
					vector = Vector4.Lerp(vector, _flowerColors[((gridPositionOfIndex.Y * 47 + gridPositionOfIndex.X) % 5 + 5) % 5], dynamicNoise2);
				}
				fragment.SetColor(i, vector);
			}
		}
	}
}
