using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct LightDiscDrawer
	{
		private static VertexStrip _vertexStrip = new VertexStrip();

		public void Draw(Projectile proj)
		{
			MiscShaderData miscShaderData = GameShaders.Misc["LightDisc"];
			miscShaderData.UseSaturation(-2.8f);
			miscShaderData.UseOpacity(2f);
			miscShaderData.Apply();
			_vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
			_vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		private Color StripColors(float progressOnStrip)
		{
			float num = 1f - progressOnStrip;
			Color result = new Color(48, 63, 150) * (num * num * num * num) * 0.5f;
			result.A = 0;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			return 16f;
		}
	}
}
