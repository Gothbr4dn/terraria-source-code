using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class DesertShader : ChromaShader
	{
		private readonly Vector4 _baseColor;

		private readonly Vector4 _sandColor;

		public DesertShader(Color baseColor, Color sandColor)
		{
			_baseColor = baseColor.ToVector4();
			_sandColor = sandColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				fragment.GetGridPositionOfIndex(i);
				canvasPositionOfIndex.Y += (float)Math.Sin(canvasPositionOfIndex.X * 2f + time * 2f) * 0.2f;
				float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * new Vector2(0.1f, 0.5f));
				Vector4 vector = Vector4.Lerp(_baseColor, _sandColor, staticNoise * staticNoise);
				fragment.SetColor(i, vector);
			}
		}
	}
}
