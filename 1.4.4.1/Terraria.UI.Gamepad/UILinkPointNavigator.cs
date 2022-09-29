using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameInput;

namespace Terraria.UI.Gamepad
{
	public class UILinkPointNavigator
	{
		public static class Shortcuts
		{
			public static int NPCS_IconsPerColumn = 100;

			public static int NPCS_IconsTotal = 0;

			public static int NPCS_LastHovered = -2;

			public static bool NPCS_IconsDisplay = false;

			public static int CRAFT_IconsPerRow = 100;

			public static int CRAFT_IconsPerColumn = 100;

			public static int CRAFT_CurrentIngredientsCount = 0;

			public static int CRAFT_CurrentRecipeBig = 0;

			public static int CRAFT_CurrentRecipeSmall = 0;

			public static bool NPCCHAT_ButtonsLeft = false;

			public static bool NPCCHAT_ButtonsMiddle = false;

			public static bool NPCCHAT_ButtonsRight = false;

			public static bool NPCCHAT_ButtonsRight2 = false;

			public static int INGAMEOPTIONS_BUTTONS_LEFT = 0;

			public static int INGAMEOPTIONS_BUTTONS_RIGHT = 0;

			public static bool CREATIVE_ItemSlotShouldHighlightAsSelected = false;

			public static int OPTIONS_BUTTON_SPECIALFEATURE;

			public static int BackButtonCommand;

			public static bool BackButtonInUse = false;

			public static bool BackButtonLock;

			public static int FANCYUI_HIGHEST_INDEX = 1;

			public static int FANCYUI_SPECIAL_INSTRUCTIONS = 0;

			public static int INFOACCCOUNT = 0;

			public static int BUILDERACCCOUNT = 0;

			public static int BUFFS_PER_COLUMN = 0;

			public static int BUFFS_DRAWN = 0;

			public static int INV_MOVE_OPTION_CD = 0;
		}

		public static Dictionary<int, UILinkPage> Pages = new Dictionary<int, UILinkPage>();

		public static Dictionary<int, UILinkPoint> Points = new Dictionary<int, UILinkPoint>();

		public static int CurrentPage = 1000;

		public static int OldPage = 1000;

		private static int XCooldown;

		private static int YCooldown;

		private static Vector2 LastInput;

		private static int PageLeftCD;

		private static int PageRightCD;

		public static bool InUse;

		public static int OverridePoint = -1;

		private static int? _suggestedPointID;

		private static int? _preSuggestionPoint;

		public static int CurrentPoint => Pages[CurrentPage].CurrentPoint;

		public static bool Available
		{
			get
			{
				if (!Main.playerInventory && !Main.ingameOptionsWindow && Main.player[Main.myPlayer].talkNPC == -1 && Main.player[Main.myPlayer].sign == -1 && !Main.mapFullscreen && !Main.clothesWindow && !Main.MenuUI.IsVisible)
				{
					return Main.InGameUI.IsVisible;
				}
				return true;
			}
		}

		public static void SuggestUsage(int PointID)
		{
			if (Points.ContainsKey(PointID))
			{
				_suggestedPointID = PointID;
			}
		}

		public static void ConsumeSuggestion()
		{
			if (_suggestedPointID.HasValue)
			{
				int value = _suggestedPointID.Value;
				ClearSuggestion();
				CurrentPage = Points[value].Page;
				OverridePoint = value;
				ProcessChanges();
				PlayerInput.Triggers.Current.UsedMovementKey = true;
			}
		}

		public static void ClearSuggestion()
		{
			_suggestedPointID = null;
		}

		public static void GoToDefaultPage(int specialFlag = 0)
		{
			TileEntity tileEntity = Main.LocalPlayer.tileEntityAnchor.GetTileEntity();
			if (Main.MenuUI.IsVisible)
			{
				CurrentPage = 1004;
			}
			else if (Main.InGameUI.IsVisible || specialFlag == 1)
			{
				CurrentPage = 1004;
			}
			else if (Main.gameMenu)
			{
				CurrentPage = 1000;
			}
			else if (Main.ingameOptionsWindow)
			{
				CurrentPage = 1001;
			}
			else if (Main.CreativeMenu.Enabled)
			{
				CurrentPage = 1005;
			}
			else if (Main.hairWindow)
			{
				CurrentPage = 12;
			}
			else if (Main.clothesWindow)
			{
				CurrentPage = 15;
			}
			else if (Main.npcShop != 0)
			{
				CurrentPage = 13;
			}
			else if (Main.InGuideCraftMenu)
			{
				CurrentPage = 9;
			}
			else if (Main.InReforgeMenu)
			{
				CurrentPage = 5;
			}
			else if (Main.player[Main.myPlayer].chest != -1)
			{
				CurrentPage = 4;
			}
			else if (tileEntity is TEDisplayDoll)
			{
				CurrentPage = 20;
			}
			else if (tileEntity is TEHatRack)
			{
				CurrentPage = 21;
			}
			else if (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1)
			{
				CurrentPage = 1003;
			}
			else
			{
				CurrentPage = 0;
			}
		}

		public static void Update()
		{
			bool inUse = InUse;
			InUse = false;
			bool flag = true;
			if (flag)
			{
				InputMode currentInputMode = PlayerInput.CurrentInputMode;
				if ((uint)currentInputMode <= 2u && !Main.gameMenu)
				{
					flag = false;
				}
			}
			if (flag && PlayerInput.NavigatorRebindingLock > 0)
			{
				flag = false;
			}
			if (flag && !Main.gameMenu && !PlayerInput.UsingGamepadUI)
			{
				flag = false;
			}
			if (flag && !Main.gameMenu && PlayerInput.InBuildingMode)
			{
				flag = false;
			}
			if (flag && !Main.gameMenu && !Available)
			{
				flag = false;
			}
			bool flag2 = false;
			if (!Pages.TryGetValue(CurrentPage, out var value))
			{
				flag2 = true;
			}
			else if (!value.IsValid())
			{
				flag2 = true;
			}
			if (flag2)
			{
				GoToDefaultPage();
				ProcessChanges();
				flag = false;
			}
			if (inUse != flag)
			{
				if (!flag)
				{
					value.Leave();
					GoToDefaultPage();
					ProcessChanges();
				}
				else
				{
					GoToDefaultPage();
					ProcessChanges();
					ConsumeSuggestion();
					value.Enter();
				}
				if (flag)
				{
					if (!PlayerInput.SteamDeckIsUsed || PlayerInput.PreventCursorModeSwappingToGamepad)
					{
						Main.player[Main.myPlayer].releaseInventory = false;
					}
					Main.player[Main.myPlayer].releaseUseTile = false;
					PlayerInput.LockGamepadTileUseButton = true;
				}
				if (!Main.gameMenu)
				{
					if (flag)
					{
						PlayerInput.NavigatorCachePosition();
					}
					else
					{
						PlayerInput.NavigatorUnCachePosition();
					}
				}
			}
			ClearSuggestion();
			if (!flag)
			{
				return;
			}
			InUse = true;
			OverridePoint = -1;
			if (PageLeftCD > 0)
			{
				PageLeftCD--;
			}
			if (PageRightCD > 0)
			{
				PageRightCD--;
			}
			Vector2 navigatorDirections = PlayerInput.Triggers.Current.GetNavigatorDirections();
			bool num = PlayerInput.Triggers.Current.HotbarMinus && !PlayerInput.Triggers.Current.HotbarPlus;
			bool flag3 = PlayerInput.Triggers.Current.HotbarPlus && !PlayerInput.Triggers.Current.HotbarMinus;
			if (!num)
			{
				PageLeftCD = 0;
			}
			if (!flag3)
			{
				PageRightCD = 0;
			}
			bool num2 = num && PageLeftCD == 0;
			flag3 = flag3 && PageRightCD == 0;
			if (LastInput.X != navigatorDirections.X)
			{
				XCooldown = 0;
			}
			if (LastInput.Y != navigatorDirections.Y)
			{
				YCooldown = 0;
			}
			if (XCooldown > 0)
			{
				XCooldown--;
			}
			if (YCooldown > 0)
			{
				YCooldown--;
			}
			LastInput = navigatorDirections;
			if (num2)
			{
				PageLeftCD = 16;
			}
			if (flag3)
			{
				PageRightCD = 16;
			}
			Pages[CurrentPage].Update();
			int num3 = 10;
			if (!Main.gameMenu && Main.playerInventory && !Main.ingameOptionsWindow && !Main.inFancyUI && (CurrentPage == 0 || CurrentPage == 4 || CurrentPage == 2 || CurrentPage == 1 || CurrentPage == 20 || CurrentPage == 21))
			{
				num3 = PlayerInput.CurrentProfile.InventoryMoveCD;
			}
			if (navigatorDirections.X == -1f && XCooldown == 0)
			{
				XCooldown = num3;
				Pages[CurrentPage].TravelLeft();
			}
			if (navigatorDirections.X == 1f && XCooldown == 0)
			{
				XCooldown = num3;
				Pages[CurrentPage].TravelRight();
			}
			if (navigatorDirections.Y == -1f && YCooldown == 0)
			{
				YCooldown = num3;
				Pages[CurrentPage].TravelUp();
			}
			if (navigatorDirections.Y == 1f && YCooldown == 0)
			{
				YCooldown = num3;
				Pages[CurrentPage].TravelDown();
			}
			XCooldown = (YCooldown = Math.Max(XCooldown, YCooldown));
			if (num2)
			{
				Pages[CurrentPage].SwapPageLeft();
			}
			if (flag3)
			{
				Pages[CurrentPage].SwapPageRight();
			}
			if (PlayerInput.Triggers.Current.UsedMovementKey)
			{
				Vector2 position = Points[CurrentPoint].Position;
				Vector2 value2 = new Vector2(PlayerInput.MouseX, PlayerInput.MouseY);
				float amount = 0.3f;
				if (PlayerInput.InvisibleGamepadInMenus)
				{
					amount = 1f;
				}
				Vector2 vector = Vector2.Lerp(value2, position, amount);
				if (Main.gameMenu)
				{
					if (Math.Abs(vector.X - position.X) <= 5f)
					{
						vector.X = position.X;
					}
					if (Math.Abs(vector.Y - position.Y) <= 5f)
					{
						vector.Y = position.Y;
					}
				}
				PlayerInput.MouseX = (int)vector.X;
				PlayerInput.MouseY = (int)vector.Y;
			}
			ResetFlagsEnd();
		}

		public static void ResetFlagsEnd()
		{
			Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 0;
			Shortcuts.BackButtonLock = false;
			Shortcuts.BackButtonCommand = 0;
		}

		public static string GetInstructions()
		{
			UILinkPage uILinkPage = Pages[CurrentPage];
			UILinkPoint uILinkPoint = Points[CurrentPoint];
			if (_suggestedPointID.HasValue)
			{
				SwapToSuggestion();
				uILinkPoint = Points[_suggestedPointID.Value];
				uILinkPage = Pages[uILinkPoint.Page];
				CurrentPage = uILinkPage.ID;
				uILinkPage.CurrentPoint = _suggestedPointID.Value;
			}
			string text = uILinkPage.SpecialInteractions();
			if ((PlayerInput.SettingsForUI.CurrentCursorMode == CursorMode.Gamepad && PlayerInput.Triggers.Current.UsedMovementKey && InUse) || _suggestedPointID.HasValue)
			{
				string text2 = uILinkPoint.SpecialInteractions();
				if (!string.IsNullOrEmpty(text2))
				{
					if (string.IsNullOrEmpty(text))
					{
						return text2;
					}
					text = text + "   " + text2;
				}
			}
			ConsumeSuggestionSwap();
			return text;
		}

		public static void SwapToSuggestion()
		{
			_preSuggestionPoint = CurrentPoint;
		}

		public static void ConsumeSuggestionSwap()
		{
			if (_preSuggestionPoint.HasValue)
			{
				int value = _preSuggestionPoint.Value;
				CurrentPage = Points[value].Page;
				Pages[CurrentPage].CurrentPoint = value;
			}
			_preSuggestionPoint = null;
		}

		public static void ForceMovementCooldown(int time)
		{
			LastInput = PlayerInput.Triggers.Current.GetNavigatorDirections();
			XCooldown = time;
			YCooldown = time;
		}

		public static void SetPosition(int ID, Vector2 Position)
		{
			Points[ID].Position = Position * Main.UIScale;
		}

		public static void RegisterPage(UILinkPage page, int ID, bool automatedDefault = true)
		{
			if (automatedDefault)
			{
				page.DefaultPoint = page.LinkMap.Keys.First();
			}
			page.CurrentPoint = page.DefaultPoint;
			page.ID = ID;
			Pages.Add(page.ID, page);
			foreach (KeyValuePair<int, UILinkPoint> item in page.LinkMap)
			{
				item.Value.SetPage(ID);
				Points.Add(item.Key, item.Value);
			}
		}

		public static void ChangePage(int PageID)
		{
			if (Pages.ContainsKey(PageID) && Pages[PageID].CanEnter())
			{
				SoundEngine.PlaySound(12);
				CurrentPage = PageID;
				ProcessChanges();
			}
		}

		public static void ChangePoint(int PointID)
		{
			if (Points.ContainsKey(PointID))
			{
				CurrentPage = Points[PointID].Page;
				OverridePoint = PointID;
				ProcessChanges();
			}
		}

		public static void ProcessChanges()
		{
			UILinkPage value = Pages[OldPage];
			if (OldPage != CurrentPage)
			{
				value.Leave();
				if (!Pages.TryGetValue(CurrentPage, out value))
				{
					GoToDefaultPage();
					ProcessChanges();
					OverridePoint = -1;
				}
				value.CurrentPoint = value.DefaultPoint;
				value.Enter();
				value.Update();
				OldPage = CurrentPage;
			}
			if (OverridePoint != -1 && value.LinkMap.ContainsKey(OverridePoint))
			{
				value.CurrentPoint = OverridePoint;
			}
		}
	}
}
