using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes
{
	public class TwilightDyeShaderData : ArmorShaderData
	{
		public TwilightDyeShaderData(Ref<Effect> shader, string passName)
			: base(shader, passName)
		{
		}

		public override void Apply(Entity entity, DrawData? drawData)
		{
			if (drawData.HasValue)
			{
				if (entity is Player player && !player.isDisplayDollOrInanimate && !player.isHatRackDoll)
				{
					UseTargetPosition(Main.screenPosition + drawData.Value.position);
				}
				else if (entity is Projectile)
				{
					UseTargetPosition(Main.screenPosition + drawData.Value.position);
				}
				else
				{
					UseTargetPosition(drawData.Value.position);
				}
			}
			base.Apply(entity, drawData);
		}
	}
}
