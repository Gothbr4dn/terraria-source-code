using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class RainShader : ChromaShader
	{
		private bool _inBloodMoon;

		public override void Update(float elapsedTime)
		{
			_inBloodMoon = Main.bloodMoon;
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 value = ((!_inBloodMoon) ? new Vector4(0f, 0f, 1f, 1f) : new Vector4(1f, 0f, 0f, 1f));
			Vector4 vector = new Vector4(0f, 0f, 0f, 0.75f);
			for (int i = 0; i < fragment.Count; i++)
			{
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 10f + time) % 10f - canvasPositionOfIndex.Y;
				Vector4 vector2 = vector;
				if (num > 0f)
				{
					float amount = Math.Max(0f, 1.2f - num);
					if (num < 0.2f)
					{
						amount = num * 5f;
					}
					vector2 = Vector4.Lerp(vector2, value, amount);
				}
				fragment.SetColor(i, vector2);
			}
		}
	}
}
