using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
	public class TwilightHairDyeShaderData : HairShaderData
	{
		public TwilightHairDyeShaderData(Ref<Effect> shader, string passName)
			: base(shader, passName)
		{
		}

		public override void Apply(Player player, DrawData? drawData = null)
		{
			if (drawData.HasValue)
			{
				UseTargetPosition(Main.screenPosition + drawData.Value.position);
			}
			base.Apply(player, drawData);
		}
	}
}
