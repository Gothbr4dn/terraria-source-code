using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.DataStructures
{
	public class PlayerDrawHelper
	{
		public enum ShaderConfiguration
		{
			ArmorShader,
			HairShader,
			TileShader,
			TilePaintID
		}

		public static Color DISPLAY_DOLL_DEFAULT_SKIN_COLOR = new Color(163, 121, 92);

		public static int PackShader(int localShaderIndex, ShaderConfiguration shaderType)
		{
			return localShaderIndex + (int)shaderType * 1000;
		}

		public static void UnpackShader(int packedShaderIndex, out int localShaderIndex, out ShaderConfiguration shaderType)
		{
			shaderType = (ShaderConfiguration)(packedShaderIndex / 1000);
			localShaderIndex = packedShaderIndex % 1000;
		}

		public static void SetShaderForData(Player player, int cHead, ref DrawData cdd)
		{
			UnpackShader(cdd.shader, out var localShaderIndex, out var shaderType);
			switch (shaderType)
			{
			case ShaderConfiguration.ArmorShader:
				GameShaders.Hair.Apply(0, player, cdd);
				GameShaders.Armor.Apply(localShaderIndex, player, cdd);
				break;
			case ShaderConfiguration.HairShader:
				if (player.head == 0)
				{
					GameShaders.Hair.Apply(0, player, cdd);
					GameShaders.Armor.Apply(cHead, player, cdd);
				}
				else
				{
					GameShaders.Armor.Apply(0, player, cdd);
					GameShaders.Hair.Apply((short)localShaderIndex, player, cdd);
				}
				break;
			case ShaderConfiguration.TileShader:
				Main.tileShader.CurrentTechnique.Passes[localShaderIndex].Apply();
				break;
			case ShaderConfiguration.TilePaintID:
			{
				int index = Main.ConvertPaintIdToTileShaderIndex(localShaderIndex, isUsedForPaintingGrass: false, useWallShaderHacks: false);
				Main.tileShader.CurrentTechnique.Passes[index].Apply();
				break;
			}
			}
		}
	}
}
