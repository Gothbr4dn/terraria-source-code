namespace Terraria.GameContent.UI.ResourceSets
{
	public class CommonResourceBarMethods
	{
		public static void DrawLifeMouseOver()
		{
			if (!Main.mouseText)
			{
				Player localPlayer = Main.LocalPlayer;
				localPlayer.cursorItemIconEnabled = false;
				string text = localPlayer.statLife + "/" + localPlayer.statLifeMax2;
				Main.instance.MouseTextHackZoom(text);
				Main.mouseText = true;
			}
		}

		public static void DrawManaMouseOver()
		{
			if (!Main.mouseText)
			{
				Player localPlayer = Main.LocalPlayer;
				localPlayer.cursorItemIconEnabled = false;
				string text = localPlayer.statMana + "/" + localPlayer.statManaMax2;
				Main.instance.MouseTextHackZoom(text);
				Main.mouseText = true;
			}
		}
	}
}
