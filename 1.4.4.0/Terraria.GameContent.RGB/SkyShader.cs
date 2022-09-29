using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class SkyShader : ChromaShader
	{
		private readonly Vector4 _baseSkyColor;

		private readonly Vector4 _baseSpaceColor;

		private Vector4 _processedSkyColor;

		private Vector4 _processedCloudColor;

		private float _backgroundTransition;

		private float _starVisibility;

		public SkyShader(Color skyColor, Color spaceColor)
		{
			_baseSkyColor = skyColor.ToVector4();
			_baseSpaceColor = spaceColor.ToVector4();
		}

		public override void Update(float elapsedTime)
		{
			float num = Main.player[Main.myPlayer].position.Y / 16f;
			_backgroundTransition = MathHelper.Clamp((num - (float)Main.worldSurface * 0.25f) / ((float)Main.worldSurface * 0.1f), 0f, 1f);
			_processedSkyColor = _baseSkyColor * (Main.ColorOfTheSkies.ToVector4() * 0.75f + Vector4.One * 0.25f);
			_processedCloudColor = Main.ColorOfTheSkies.ToVector4() * 0.75f + Vector4.One * 0.25f;
			if (Main.dayTime)
			{
				float num2 = (float)(Main.time / 54000.0);
				if (num2 < 0.25f)
				{
					_starVisibility = 1f - num2 / 0.25f;
				}
				else if (num2 > 0.75f)
				{
					_starVisibility = (num2 - 0.75f) / 0.25f;
				}
				else
				{
					_starVisibility = 0f;
				}
			}
			else
			{
				_starVisibility = 1f;
			}
			_starVisibility = Math.Max(1f - _backgroundTransition, _starVisibility);
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessAnyDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
				float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * new Vector2(0.1f, 0.5f) + new Vector2(time * 0.05f, 0f), time / 20f);
				dynamicNoise = (float)Math.Sqrt(Math.Max(0f, 1f - 2f * dynamicNoise));
				Vector4 value = Vector4.Lerp(_processedSkyColor, _processedCloudColor, dynamicNoise);
				value = Vector4.Lerp(_baseSpaceColor, value, _backgroundTransition);
				float dynamicNoise2 = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 60f);
				dynamicNoise2 = Math.Max(0f, 1f - dynamicNoise2 * 20f);
				value = Vector4.Lerp(value, Vector4.One, dynamicNoise2 * 0.98f * _starVisibility);
				fragment.SetColor(i, value);
			}
		}
	}
}
