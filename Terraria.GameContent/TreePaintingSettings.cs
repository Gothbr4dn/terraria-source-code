using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent
{
	public class TreePaintingSettings
	{
		public float SpecialGroupMinimalHueValue;

		public float SpecialGroupMaximumHueValue;

		public float SpecialGroupMinimumSaturationValue;

		public float SpecialGroupMaximumSaturationValue;

		public float HueTestOffset;

		public bool UseSpecialGroups;

		public bool UseWallShaderHacks;

		public bool InvertSpecialGroupResult;

		public void ApplyShader(int paintColor, Effect shader)
		{
			shader.Parameters["leafHueTestOffset"].SetValue(HueTestOffset);
			shader.Parameters["leafMinHue"].SetValue(SpecialGroupMinimalHueValue);
			shader.Parameters["leafMaxHue"].SetValue(SpecialGroupMaximumHueValue);
			shader.Parameters["leafMinSat"].SetValue(SpecialGroupMinimumSaturationValue);
			shader.Parameters["leafMaxSat"].SetValue(SpecialGroupMaximumSaturationValue);
			shader.Parameters["invertSpecialGroupResult"].SetValue(InvertSpecialGroupResult);
			int index = Main.ConvertPaintIdToTileShaderIndex(paintColor, UseSpecialGroups, UseWallShaderHacks);
			shader.CurrentTechnique.Passes[index].Apply();
		}
	}
}
