using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Achievements;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI.Gamepad;

namespace Terraria.UI
{
	public class IngameFancyUI
	{
		private static bool CoverForOneUIFrame;

		public static void CoverNextFrame()
		{
			CoverForOneUIFrame = true;
		}

		public static bool CanCover()
		{
			if (CoverForOneUIFrame)
			{
				CoverForOneUIFrame = false;
				return true;
			}
			return false;
		}

		public static void OpenAchievements()
		{
			CoverNextFrame();
			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";
			Main.inFancyUI = true;
			ClearChat();
			Main.InGameUI.SetState(Main.AchievementsMenu);
		}

		public static void OpenAchievementsAndGoto(Achievement achievement)
		{
			OpenAchievements();
			Main.AchievementsMenu.GotoAchievement(achievement);
		}

		private static void ClearChat()
		{
			Main.ClosePlayerChat();
			Main.chatText = "";
		}

		public static void OpenKeybinds()
		{
			OpenUIState(Main.ManageControlsMenu);
		}

		public static void OpenUIState(UIState uiState)
		{
			CoverNextFrame();
			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";
			Main.inFancyUI = true;
			ClearChat();
			Main.InGameUI.SetState(uiState);
		}

		public static bool CanShowVirtualKeyboard(int context)
		{
			return UIVirtualKeyboard.CanDisplay(context);
		}

		public static void OpenVirtualKeyboard(int keyboardContext)
		{
			CoverNextFrame();
			ClearChat();
			SoundEngine.PlaySound(12);
			string labelText = "";
			switch (keyboardContext)
			{
			case 1:
				Main.editSign = true;
				labelText = Language.GetTextValue("UI.EnterMessage");
				break;
			case 2:
			{
				labelText = Language.GetTextValue("UI.EnterNewName");
				Player player = Main.player[Main.myPlayer];
				Main.npcChatText = Main.chest[player.chest].name;
				Tile tile = Main.tile[player.chestX, player.chestY];
				if (tile.type == 21)
				{
					Main.defaultChestName = Lang.chestType[tile.frameX / 36].Value;
				}
				else if (tile.type == 467 && tile.frameX / 36 == 4)
				{
					Main.defaultChestName = Lang.GetItemNameValue(3988);
				}
				else if (tile.type == 467)
				{
					Main.defaultChestName = Lang.chestType2[tile.frameX / 36].Value;
				}
				else if (tile.type == 88)
				{
					Main.defaultChestName = Lang.dresserType[tile.frameX / 54].Value;
				}
				if (Main.npcChatText == "")
				{
					Main.npcChatText = Main.defaultChestName;
				}
				Main.editChest = true;
				break;
			}
			}
			Main.clrInput();
			if (!CanShowVirtualKeyboard(keyboardContext))
			{
				return;
			}
			Main.inFancyUI = true;
			switch (keyboardContext)
			{
			case 1:
				Main.InGameUI.SetState(new UIVirtualKeyboard(labelText, Main.npcChatText, delegate
				{
					Main.SubmitSignText();
					Close();
				}, delegate
				{
					Main.InputTextSignCancel();
					Close();
				}, keyboardContext));
				break;
			case 2:
				Main.InGameUI.SetState(new UIVirtualKeyboard(labelText, Main.npcChatText, delegate
				{
					ChestUI.RenameChestSubmit(Main.player[Main.myPlayer]);
					Close();
				}, delegate
				{
					ChestUI.RenameChestCancel();
					Close();
				}, keyboardContext));
				break;
			}
			UILinkPointNavigator.GoToDefaultPage(1);
		}

		public static void Close()
		{
			Main.inFancyUI = false;
			SoundEngine.PlaySound(11);
			bool flag = !Main.gameMenu;
			bool flag2 = !(Main.InGameUI.CurrentState is UIVirtualKeyboard);
			bool flag3 = false;
			int keyboardContext = UIVirtualKeyboard.KeyboardContext;
			if ((uint)(keyboardContext - 2) <= 1u)
			{
				flag3 = true;
			}
			if (flag && !(flag2 || flag3))
			{
				flag = false;
			}
			if (flag)
			{
				Main.playerInventory = true;
			}
			if (!Main.gameMenu && Main.InGameUI.CurrentState is UIEmotesMenu)
			{
				Main.playerInventory = false;
			}
			Main.LocalPlayer.releaseInventory = false;
			Main.InGameUI.SetState(null);
			UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS = 0;
		}

		public static bool Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			bool result = false;
			if (Main.InGameUI.CurrentState is UIVirtualKeyboard && UIVirtualKeyboard.KeyboardContext > 0)
			{
				if (!Main.inFancyUI)
				{
					Main.InGameUI.SetState(null);
				}
				if (Main.screenWidth >= 1705 || !PlayerInput.UsingGamepad)
				{
					result = true;
				}
			}
			if (!Main.gameMenu)
			{
				Main.mouseText = false;
				if (Main.InGameUI != null && Main.InGameUI.IsElementUnderMouse())
				{
					Main.player[Main.myPlayer].mouseInterface = true;
				}
				Main.instance.GUIBarsDraw();
				if (Main.InGameUI.CurrentState is UIVirtualKeyboard && UIVirtualKeyboard.KeyboardContext > 0)
				{
					Main.instance.GUIChatDraw();
				}
				if (!Main.inFancyUI)
				{
					Main.InGameUI.SetState(null);
				}
				Main.instance.DrawMouseOver();
				Main.DrawCursor(Main.DrawThickCursor());
			}
			return result;
		}

		public static void MouseOver()
		{
			if (Main.inFancyUI && Main.InGameUI.IsElementUnderMouse())
			{
				Main.mouseText = true;
			}
		}
	}
}
