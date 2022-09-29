using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class BlizzardShader : ChromaShader
	{
		private readonly Vector4 _backColor = new Vector4(0.1f, 0.1f, 0.3f, 1f);

		private readonly Vector4 _frontColor = new Vector4(1f, 1f, 1f, 1f);

		private readonly float _timeScaleX;

		private readonly float _timeScaleY;

		public BlizzardShader(Vector4 frontColor, Vector4 backColor, float panSpeedX, float panSpeedY)
		{
			_frontColor = frontColor;
			_backColor = backColor;
			_timeScaleX = panSpeedX;
			_timeScaleY = panSpeedY;
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			if ((int)quality == 0)
			{
				time *= 0.25f;
			}
			for (int i = 0; i < fragment.Count; i++)
			{
				float staticNoise = NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(i) * new Vector2(0.2f, 0.4f) + new Vector2(time * _timeScaleX, time * _timeScaleY));
				Vector4 vector = Vector4.Lerp(_backColor, _frontColor, staticNoise * staticNoise);
				fragment.SetColor(i, vector);
			}
		}
	}
}
