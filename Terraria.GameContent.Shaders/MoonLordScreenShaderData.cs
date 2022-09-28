using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
	public class MoonLordScreenShaderData : ScreenShaderData
	{
		private int _moonLordIndex = -1;

		private bool _aimAtPlayer;

		public MoonLordScreenShaderData(string passName, bool aimAtPlayer)
			: base(passName)
		{
			_aimAtPlayer = aimAtPlayer;
		}

		private void UpdateMoonLordIndex()
		{
			if (_aimAtPlayer || (_moonLordIndex >= 0 && Main.npc[_moonLordIndex].active && Main.npc[_moonLordIndex].type == 398))
			{
				return;
			}
			int moonLordIndex = -1;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == 398)
				{
					moonLordIndex = i;
					break;
				}
			}
			_moonLordIndex = moonLordIndex;
		}

		public override void Apply()
		{
			UpdateMoonLordIndex();
			if (_aimAtPlayer)
			{
				UseTargetPosition(Main.LocalPlayer.Center);
			}
			else if (_moonLordIndex != -1)
			{
				UseTargetPosition(Main.npc[_moonLordIndex].Center);
			}
			base.Apply();
		}
	}
}
