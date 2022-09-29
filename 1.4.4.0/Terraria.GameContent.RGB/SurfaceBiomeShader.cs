using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class SurfaceBiomeShader : ChromaShader
	{
		private readonly Vector4 _primaryColor;

		private readonly Vector4 _secondaryColor;

		private Vector4 _surfaceColor;

		private float _starVisibility;

		public SurfaceBiomeShader(Color primaryColor, Color secondaryColor)
		{
			_primaryColor = primaryColor.ToVector4();
			_secondaryColor = secondaryColor.ToVector4();
		}

		public override void Update(float elapsedTime)
		{
			_surfaceColor = Main.ColorOfTheSkies.ToVector4() * 0.75f + Vector4.One * 0.25f;
			if (Main.dayTime)
			{
				float num = (float)(Main.time / 54000.0);
				if (num < 0.25f)
				{
					_starVisibility = 1f - num / 0.25f;
				}
				else if (num > 0.75f)
				{
					_starVisibility = (num - 0.75f) / 0.25f;
				}
			}
			else
			{
				_starVisibility = 1f;
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 value = _primaryColor * _surfaceColor;
			Vector4 value2 = _secondaryColor * _surfaceColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector4 vector = Vector4.Lerp(value, value2, (float)Math.Sin(time * 0.5f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f);
				fragment.SetColor(i, vector);
			}
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			Vector4 value = _primaryColor * _surfaceColor;
			Vector4 value2 = _secondaryColor * _surfaceColor;
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				float amount = (float)Math.Sin(canvasPositionOfIndex.X * 1.5f + canvasPositionOfIndex.Y + time) * 0.5f + 0.5f;
				Vector4 value3 = Vector4.Lerp(value, value2, amount);
				float dynamicNoise = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 60f);
				dynamicNoise = Math.Max(0f, 1f - dynamicNoise * 20f);
				dynamicNoise *= 1f - _surfaceColor.X;
				value3 = Vector4.Max(value3, new Vector4(dynamicNoise * _starVisibility));
				fragment.SetColor(i, value3);
			}
		}
	}
}
