using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
	public class TeamArmorShaderData : ArmorShaderData
	{
		private static bool isInitialized;

		private static ArmorShaderData[] dustShaderData;

		public TeamArmorShaderData(Ref<Effect> shader, string passName)
			: base(shader, passName)
		{
			if (!isInitialized)
			{
				isInitialized = true;
				dustShaderData = new ArmorShaderData[Main.teamColor.Length];
				for (int i = 1; i < Main.teamColor.Length; i++)
				{
					dustShaderData[i] = new ArmorShaderData(shader, passName).UseColor(Main.teamColor[i]);
				}
				dustShaderData[0] = new ArmorShaderData(shader, "Default");
			}
		}

		public override void Apply(Entity entity, DrawData? drawData)
		{
			Player player = entity as Player;
			if (player == null || player.team == 0)
			{
				dustShaderData[0].Apply(player, drawData);
				return;
			}
			UseColor(Main.teamColor[player.team]);
			base.Apply(player, drawData);
		}

		public override ArmorShaderData GetSecondaryShader(Entity entity)
		{
			Player player = entity as Player;
			return dustShaderData[player.team];
		}
	}
}
