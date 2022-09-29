using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class HallowSurfaceShader : ChromaShader
	{
		private readonly Vector4 _skyColor = new Color(150, 220, 220).ToVector4();

		private readonly Vector4 _groundColor = new Vector4(1f, 0.2f, 0.25f, 1f);

		private readonly Vector4 _pinkFlowerColor = new Vector4(1f, 0.2f, 0.25f, 1f);

		private readonly Vector4 _yellowFlowerColor = new Vector4(1f, 1f, 0f, 1f);

		private Vector4 _lightColor;

		public override void Update(float elapsedTime)
		{
			_lightColor = Main.ColorOfTheSkies.ToVector4() * 0.75f + Vector4.One * 0.25f;
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: (float)Math.Sin(time + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f, value1: _skyColor, value2: _groundColor);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 vector = _skyColor * _lightColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				float dynamicNoise = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 20f);
				dynamicNoise = Math.Max(0f, 1f - dynamicNoise * 5f);
				Vector4 value = vector;
				value = (((gridPositionOfIndex.X * 100 + gridPositionOfIndex.Y) % 2 != 0) ? Vector4.Lerp(value, _pinkFlowerColor, dynamicNoise) : Vector4.Lerp(value, _yellowFlowerColor, dynamicNoise));
				float num = (float)Math.Sin(canvasPositionOfIndex.X) * 0.3f + 0.7f;
				if (canvasPositionOfIndex.Y > num)
				{
					value = _groundColor;
				}
				fragment.SetColor(i, value);
			}
		}
	}
}
