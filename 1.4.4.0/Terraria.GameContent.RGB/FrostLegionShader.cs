using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB
{
	public class FrostLegionShader : ChromaShader
	{
		private readonly Vector4 _primaryColor;

		private readonly Vector4 _secondaryColor;

		public FrostLegionShader(Color primaryColor, Color secondaryColor)
		{
			_primaryColor = primaryColor.ToVector4();
			_secondaryColor = secondaryColor.ToVector4();
		}

		[RgbProcessor(/*Could not decode attribute arguments.*/)]
		private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
		{
			for (int i = 0; i < fragment.Count; i++)
			{
				Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
				float staticNoise = NoiseHelper.GetStaticNoise(fragment.GetGridPositionOfIndex(i).X / 2);
				float num = (canvasPositionOfIndex.Y + canvasPositionOfIndex.X / 2f - staticNoise + time) % 2f;
				if (num < 0f)
				{
					num += 2f;
				}
				if (num < 0.2f)
				{
					num = 1f - num / 0.2f;
				}
				float amount = num / 2f;
				Vector4 vector = Vector4.Lerp(_primaryColor, _secondaryColor, amount);
				fragment.SetColor(i, vector);
			}
		}
	}
}
