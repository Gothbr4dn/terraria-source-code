using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class DukeFishronShader : ChromaShader
	{
		private readonly Vector4 _primaryColor;

		private readonly Vector4 _secondaryColor;

		public DukeFishronShader(Color primaryColor, Color secondaryColor)
		{
			_primaryColor = primaryColor.ToVector4();
			_secondaryColor = secondaryColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(amount: Math.Max(0f, (float)Math.Sin(time * 2f + fragment.GetCanvasPositionOfIndex(i).X)), value1: _primaryColor, value2: _secondaryColor);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float dynamicNoise = NoiseHelper.GetDynamicNoise(fragment.GetGridPositionOfIndex(i).Y, time);
				float val = (float)Math.Sin(canvasPositionOfIndex.X + 2f * time + dynamicNoise) - 0.2f;
				val = Math.Max(0f, val);
				Vector4 vector = Vector4.Lerp(_primaryColor, _secondaryColor, val);
				fragment.SetColor(i, vector);
			}
		}
	}
}
