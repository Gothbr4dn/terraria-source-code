using Microsoft.Xna.Framework;

namespace Terraria.GameContent
{
	public class BackgroundChangeFlashInfo
	{
		private int[] _variations = new int[13];

		private float[] _flashPower = new float[13];

		public void UpdateCache()
		{
			UpdateVariation(0, WorldGen.treeBG1);
			UpdateVariation(1, WorldGen.treeBG2);
			UpdateVariation(2, WorldGen.treeBG3);
			UpdateVariation(3, WorldGen.treeBG4);
			UpdateVariation(4, WorldGen.corruptBG);
			UpdateVariation(5, WorldGen.jungleBG);
			UpdateVariation(6, WorldGen.snowBG);
			UpdateVariation(7, WorldGen.hallowBG);
			UpdateVariation(8, WorldGen.crimsonBG);
			UpdateVariation(9, WorldGen.desertBG);
			UpdateVariation(10, WorldGen.oceanBG);
			UpdateVariation(11, WorldGen.mushroomBG);
			UpdateVariation(12, WorldGen.underworldBG);
		}

		private void UpdateVariation(int areaId, int newVariationValue)
		{
			int num = _variations[areaId];
			_variations[areaId] = newVariationValue;
			if (num != newVariationValue)
			{
				ValueChanged(areaId);
			}
		}

		private void ValueChanged(int areaId)
		{
			if (!Main.gameMenu)
			{
				_flashPower[areaId] = 1f;
			}
		}

		public void UpdateFlashValues()
		{
			for (int i = 0; i < _flashPower.Length; i++)
			{
				_flashPower[i] = MathHelper.Clamp(_flashPower[i] - 0.05f, 0f, 1f);
			}
		}

		public float GetFlashPower(int areaId)
		{
			return _flashPower[areaId];
		}
	}
}
