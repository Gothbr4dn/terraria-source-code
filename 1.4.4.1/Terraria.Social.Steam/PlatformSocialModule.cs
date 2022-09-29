using Steamworks;
using Terraria.GameInput;
using Terraria.Social.Base;
using Terraria.UI.Gamepad;

namespace Terraria.Social.Steam
{
	public class PlatformSocialModule : Terraria.Social.Base.PlatformSocialModule
	{
		public override void Initialize()
		{
			if (!Main.dedServ)
			{
				bool num = (PlayerInput.UseSteamDeckIfPossible = SteamUtils.IsSteamRunningOnSteamDeck());
				if (num)
				{
					PlayerInput.SettingsForUI.SetCursorMode(CursorMode.Gamepad);
					PlayerInput.CurrentInputMode = InputMode.XBoxGamepadUI;
					GamepadMainMenuHandler.MoveCursorOnNextRun = true;
					PlayerInput.PreventFirstMousePositionGrab = true;
				}
				if (num)
				{
					Main.graphics.PreferredBackBufferWidth = (Main.screenWidth = 1280);
					Main.graphics.PreferredBackBufferHeight = (Main.screenHeight = 800);
					Main.startFullscreen = true;
					Main.toggleFullscreen = true;
					Main.screenBorderless = false;
					Main.screenMaximized = false;
					Main.InitialMapScale = (Main.MapScale = 0.73f);
					Main.UIScale = 1.07f;
				}
			}
		}

		public override void Shutdown()
		{
		}
	}
}
