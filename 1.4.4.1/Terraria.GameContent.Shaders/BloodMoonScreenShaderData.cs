using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
	public class BloodMoonScreenShaderData : ScreenShaderData
	{
		public BloodMoonScreenShaderData(string passName)
			: base(passName)
		{
		}

		public override void Update(GameTime gameTime)
		{
			float num = 1f - Utils.SmoothStep((float)Main.worldSurface + 50f, (float)Main.rockLayer + 100f, (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f);
			if (Main.remixWorld)
			{
				num = Utils.SmoothStep((float)(Main.rockLayer + Main.worldSurface) / 2f, (float)Main.rockLayer, (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f);
			}
			if (Main.shimmerAlpha > 0f)
			{
				num *= 1f - Main.shimmerAlpha;
			}
			UseOpacity(num * 0.75f);
		}
	}
}
