using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.Initializers
{
	public class UILinksInitializer
	{
		public class SomeVarsForUILinkers
		{
			public static Recipe SequencedCraftingCurrent;

			public static int HairMoveCD;
		}

		public static bool NothingMoreImportantThanNPCChat()
		{
			if (!Main.hairWindow && Main.npcShop == 0)
			{
				return Main.player[Main.myPlayer].chest == -1;
			}
			return false;
		}

		public static float HandleSliderHorizontalInput(float currentValue, float min, float max, float deadZone = 0.2f, float sensitivity = 0.5f)
		{
			float x = PlayerInput.GamepadThumbstickLeft.X;
			x = ((!(x < 0f - deadZone) && !(x > deadZone)) ? 0f : (MathHelper.Lerp(0f, sensitivity / 60f, (Math.Abs(x) - deadZone) / (1f - deadZone)) * (float)Math.Sign(x)));
			return MathHelper.Clamp((currentValue - min) / (max - min) + x, 0f, 1f) * (max - min) + min;
		}

		public static float HandleSliderVerticalInput(float currentValue, float min, float max, float deadZone = 0.2f, float sensitivity = 0.5f)
		{
			float num = 0f - PlayerInput.GamepadThumbstickLeft.Y;
			num = ((!(num < 0f - deadZone) && !(num > deadZone)) ? 0f : (MathHelper.Lerp(0f, sensitivity / 60f, (Math.Abs(num) - deadZone) / (1f - deadZone)) * (float)Math.Sign(num)));
			return MathHelper.Clamp((currentValue - min) / (max - min) + num, 0f, 1f) * (max - min) + min;
		}

		public static bool CanExecuteInputCommand()
		{
			return PlayerInput.AllowExecutionOfGamepadInstructions;
		}

		public static void Load()
		{
			Func<string> value = () => PlayerInput.BuildCommand(Lang.misc[53].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			UILinkPage uILinkPage = new UILinkPage();
			uILinkPage.UpdateEvent += delegate
			{
				PlayerInput.GamepadAllowScrolling = true;
			};
			for (int i = 0; i < 20; i++)
			{
				uILinkPage.LinkMap.Add(2000 + i, new UILinkPoint(2000 + i, enabled: true, -3, -4, -1, -2));
			}
			uILinkPage.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[53].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[82].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]);
			uILinkPage.UpdateEvent += delegate
			{
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Inventory)
				{
					FancyExit();
				}
				UILinkPointNavigator.Shortcuts.BackButtonInUse = PlayerInput.Triggers.JustPressed.Inventory;
				HandleOptionsSpecials();
			};
			uILinkPage.IsValidEvent += () => Main.gameMenu && !Main.MenuUI.IsVisible;
			uILinkPage.CanEnterEvent += () => Main.gameMenu && !Main.MenuUI.IsVisible;
			UILinkPointNavigator.RegisterPage(uILinkPage, 1000);
			UILinkPage cp20 = new UILinkPage();
			cp20.LinkMap.Add(2500, new UILinkPoint(2500, enabled: true, -3, 2501, -1, -2));
			cp20.LinkMap.Add(2501, new UILinkPoint(2501, enabled: true, 2500, 2502, -1, -2));
			cp20.LinkMap.Add(2502, new UILinkPoint(2502, enabled: true, 2501, 2503, -1, -2));
			cp20.LinkMap.Add(2503, new UILinkPoint(2503, enabled: true, 2502, -4, -1, -2));
			cp20.UpdateEvent += delegate
			{
				cp20.LinkMap[2501].Right = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight ? 2502 : (-4));
				if (cp20.LinkMap[2501].Right == -4 && UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight2)
				{
					cp20.LinkMap[2501].Right = 2503;
				}
				cp20.LinkMap[2502].Right = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight2 ? 2503 : (-4));
				cp20.LinkMap[2503].Left = (UILinkPointNavigator.Shortcuts.NPCCHAT_ButtonsRight ? 2502 : 2501);
			};
			cp20.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[53].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[56].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]);
			cp20.IsValidEvent += () => (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1) && NothingMoreImportantThanNPCChat();
			cp20.CanEnterEvent += () => (Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1) && NothingMoreImportantThanNPCChat();
			cp20.EnterEvent += delegate
			{
				Main.player[Main.myPlayer].releaseInventory = false;
			};
			cp20.LeaveEvent += delegate
			{
				Main.npcChatRelease = false;
				Main.player[Main.myPlayer].LockGamepadTileInteractions();
			};
			UILinkPointNavigator.RegisterPage(cp20, 1003);
			UILinkPage cp19 = new UILinkPage();
			cp19.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value2 = delegate
			{
				int currentPoint5 = UILinkPointNavigator.CurrentPoint;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 0, currentPoint5);
			};
			Func<string> value3 = () => ItemSlot.GetGamepadInstructions(ref Main.player[Main.myPlayer].trashItem, 6);
			for (int j = 0; j <= 49; j++)
			{
				UILinkPoint uILinkPoint = new UILinkPoint(j, enabled: true, j - 1, j + 1, j - 10, j + 10);
				uILinkPoint.OnSpecialInteracts += value2;
				int num = j;
				if (num < 10)
				{
					uILinkPoint.Up = -1;
				}
				if (num >= 40)
				{
					uILinkPoint.Down = -2;
				}
				if (num % 10 == 9)
				{
					uILinkPoint.Right = -4;
				}
				if (num % 10 == 0)
				{
					uILinkPoint.Left = -3;
				}
				cp19.LinkMap.Add(j, uILinkPoint);
			}
			cp19.LinkMap[9].Right = 0;
			cp19.LinkMap[19].Right = 50;
			cp19.LinkMap[29].Right = 51;
			cp19.LinkMap[39].Right = 52;
			cp19.LinkMap[49].Right = 53;
			cp19.LinkMap[0].Left = 9;
			cp19.LinkMap[10].Left = 54;
			cp19.LinkMap[20].Left = 55;
			cp19.LinkMap[30].Left = 56;
			cp19.LinkMap[40].Left = 57;
			cp19.LinkMap.Add(300, new UILinkPoint(300, enabled: true, 309, 310, 49, -2));
			cp19.LinkMap.Add(309, new UILinkPoint(309, enabled: true, 310, 300, 302, 54));
			cp19.LinkMap.Add(310, new UILinkPoint(310, enabled: true, 300, 309, 301, 50));
			cp19.LinkMap.Add(301, new UILinkPoint(301, enabled: true, 300, 302, 53, 50));
			cp19.LinkMap.Add(302, new UILinkPoint(302, enabled: true, 301, 310, 57, 54));
			cp19.LinkMap.Add(311, new UILinkPoint(311, enabled: true, -3, -4, 40, -2));
			cp19.LinkMap[301].OnSpecialInteracts += value;
			cp19.LinkMap[302].OnSpecialInteracts += value;
			cp19.LinkMap[309].OnSpecialInteracts += value;
			cp19.LinkMap[310].OnSpecialInteracts += value;
			cp19.LinkMap[300].OnSpecialInteracts += value3;
			cp19.UpdateEvent += delegate
			{
				bool inReforgeMenu = Main.InReforgeMenu;
				bool flag7 = Main.player[Main.myPlayer].chest != -1;
				bool flag8 = Main.npcShop != 0;
				TileEntity tileEntity = Main.LocalPlayer.tileEntityAnchor.GetTileEntity();
				bool flag9 = tileEntity is TEHatRack;
				bool flag10 = tileEntity is TEDisplayDoll;
				for (int num65 = 40; num65 <= 49; num65++)
				{
					if (inReforgeMenu)
					{
						cp19.LinkMap[num65].Down = ((num65 < 45) ? 303 : 304);
					}
					else if (flag7)
					{
						cp19.LinkMap[num65].Down = 400 + num65 - 40;
					}
					else if (flag8)
					{
						cp19.LinkMap[num65].Down = 2700 + num65 - 40;
					}
					else if (num65 == 40)
					{
						cp19.LinkMap[num65].Down = 311;
					}
					else
					{
						cp19.LinkMap[num65].Down = -2;
					}
				}
				if (flag10)
				{
					for (int num66 = 40; num66 <= 47; num66++)
					{
						cp19.LinkMap[num66].Down = 5100 + num66 - 40;
					}
				}
				if (flag9)
				{
					for (int num67 = 44; num67 <= 45; num67++)
					{
						cp19.LinkMap[num67].Down = 5000 + num67 - 44;
					}
				}
				if (flag7)
				{
					cp19.LinkMap[300].Up = 439;
					cp19.LinkMap[300].Right = -4;
					cp19.LinkMap[300].Left = 309;
					cp19.LinkMap[309].Up = 438;
					cp19.LinkMap[309].Right = 300;
					cp19.LinkMap[309].Left = 310;
					cp19.LinkMap[310].Up = 437;
					cp19.LinkMap[310].Right = 309;
					cp19.LinkMap[310].Left = -3;
				}
				else if (flag8)
				{
					cp19.LinkMap[300].Up = 2739;
					cp19.LinkMap[300].Right = -4;
					cp19.LinkMap[300].Left = 309;
					cp19.LinkMap[309].Up = 2738;
					cp19.LinkMap[309].Right = 300;
					cp19.LinkMap[309].Left = 310;
					cp19.LinkMap[310].Up = 2737;
					cp19.LinkMap[310].Right = 309;
					cp19.LinkMap[310].Left = -3;
				}
				else
				{
					cp19.LinkMap[49].Down = 300;
					cp19.LinkMap[48].Down = 309;
					cp19.LinkMap[47].Down = 310;
					cp19.LinkMap[300].Up = 49;
					cp19.LinkMap[300].Right = 301;
					cp19.LinkMap[300].Left = 309;
					cp19.LinkMap[309].Up = 48;
					cp19.LinkMap[309].Right = 300;
					cp19.LinkMap[309].Left = 310;
					cp19.LinkMap[310].Up = 47;
					cp19.LinkMap[310].Right = 309;
					cp19.LinkMap[310].Left = 302;
				}
				cp19.LinkMap[0].Left = 9;
				cp19.LinkMap[10].Left = 54;
				cp19.LinkMap[20].Left = 55;
				cp19.LinkMap[30].Left = 56;
				cp19.LinkMap[40].Left = 57;
				if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 0)
				{
					cp19.LinkMap[0].Left = 6000;
				}
				if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 2)
				{
					cp19.LinkMap[10].Left = 6002;
				}
				if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 4)
				{
					cp19.LinkMap[20].Left = 6004;
				}
				if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 6)
				{
					cp19.LinkMap[30].Left = 6006;
				}
				if (UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 8)
				{
					cp19.LinkMap[40].Left = 6008;
				}
				cp19.PageOnLeft = 9;
				if (Main.CreativeMenu.Enabled)
				{
					cp19.PageOnLeft = 1005;
				}
				if (Main.InReforgeMenu)
				{
					cp19.PageOnLeft = 5;
				}
			};
			cp19.IsValidEvent += () => Main.playerInventory;
			cp19.PageOnLeft = 9;
			cp19.PageOnRight = 2;
			UILinkPointNavigator.RegisterPage(cp19, 0);
			UILinkPage cp18 = new UILinkPage();
			cp18.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value4 = delegate
			{
				int currentPoint4 = UILinkPointNavigator.CurrentPoint;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 1, currentPoint4);
			};
			for (int k = 50; k <= 53; k++)
			{
				UILinkPoint uILinkPoint2 = new UILinkPoint(k, enabled: true, -3, -4, k - 1, k + 1);
				uILinkPoint2.OnSpecialInteracts += value4;
				cp18.LinkMap.Add(k, uILinkPoint2);
			}
			cp18.LinkMap[50].Left = 19;
			cp18.LinkMap[51].Left = 29;
			cp18.LinkMap[52].Left = 39;
			cp18.LinkMap[53].Left = 49;
			cp18.LinkMap[50].Right = 54;
			cp18.LinkMap[51].Right = 55;
			cp18.LinkMap[52].Right = 56;
			cp18.LinkMap[53].Right = 57;
			cp18.LinkMap[50].Up = -1;
			cp18.LinkMap[53].Down = -2;
			cp18.UpdateEvent += delegate
			{
				if (Main.player[Main.myPlayer].chest == -1 && Main.npcShop == 0)
				{
					cp18.LinkMap[50].Up = 301;
					cp18.LinkMap[53].Down = 301;
				}
				else
				{
					cp18.LinkMap[50].Up = 504;
					cp18.LinkMap[53].Down = 500;
				}
			};
			cp18.IsValidEvent += () => Main.playerInventory;
			cp18.PageOnLeft = 0;
			cp18.PageOnRight = 2;
			UILinkPointNavigator.RegisterPage(cp18, 1);
			UILinkPage cp17 = new UILinkPage();
			cp17.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value5 = delegate
			{
				int currentPoint3 = UILinkPointNavigator.CurrentPoint;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].inventory, 2, currentPoint3);
			};
			for (int l = 54; l <= 57; l++)
			{
				UILinkPoint uILinkPoint3 = new UILinkPoint(l, enabled: true, -3, -4, l - 1, l + 1);
				uILinkPoint3.OnSpecialInteracts += value5;
				cp17.LinkMap.Add(l, uILinkPoint3);
			}
			cp17.LinkMap[54].Left = 50;
			cp17.LinkMap[55].Left = 51;
			cp17.LinkMap[56].Left = 52;
			cp17.LinkMap[57].Left = 53;
			cp17.LinkMap[54].Right = 10;
			cp17.LinkMap[55].Right = 20;
			cp17.LinkMap[56].Right = 30;
			cp17.LinkMap[57].Right = 40;
			cp17.LinkMap[54].Up = -1;
			cp17.LinkMap[57].Down = -2;
			cp17.UpdateEvent += delegate
			{
				if (Main.player[Main.myPlayer].chest == -1 && Main.npcShop == 0)
				{
					cp17.LinkMap[54].Up = 302;
					cp17.LinkMap[57].Down = 302;
				}
				else
				{
					cp17.LinkMap[54].Up = 504;
					cp17.LinkMap[57].Down = 500;
				}
			};
			cp17.PageOnLeft = 0;
			cp17.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp17, 2);
			UILinkPage cp16 = new UILinkPage();
			cp16.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value6 = delegate
			{
				int num64 = UILinkPointNavigator.CurrentPoint - 100;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].armor, (num64 < 10) ? 8 : 9, num64);
			};
			Func<string> value7 = delegate
			{
				int slot11 = UILinkPointNavigator.CurrentPoint - 120;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].dye, 12, slot11);
			};
			for (int m = 100; m <= 119; m++)
			{
				UILinkPoint uILinkPoint4 = new UILinkPoint(m, enabled: true, m + 10, m - 10, m - 1, m + 1);
				uILinkPoint4.OnSpecialInteracts += value6;
				int num2 = m - 100;
				if (num2 == 0)
				{
					uILinkPoint4.Up = 305;
				}
				if (num2 == 10)
				{
					uILinkPoint4.Up = 306;
				}
				if (num2 == 9 || num2 == 19)
				{
					uILinkPoint4.Down = -2;
				}
				if (num2 >= 10)
				{
					uILinkPoint4.Left = 120 + num2 % 10;
				}
				else if (num2 >= 3)
				{
					uILinkPoint4.Right = -4;
				}
				else
				{
					uILinkPoint4.Right = 312 + num2;
				}
				cp16.LinkMap.Add(m, uILinkPoint4);
			}
			for (int n = 120; n <= 129; n++)
			{
				UILinkPoint uILinkPoint4 = new UILinkPoint(n, enabled: true, -3, -10 + n, n - 1, n + 1);
				uILinkPoint4.OnSpecialInteracts += value7;
				int num3 = n - 120;
				if (num3 == 0)
				{
					uILinkPoint4.Up = 307;
				}
				if (num3 == 9)
				{
					uILinkPoint4.Down = 308;
					uILinkPoint4.Left = 1557;
				}
				if (num3 == 8)
				{
					uILinkPoint4.Left = 1570;
				}
				cp16.LinkMap.Add(n, uILinkPoint4);
			}
			for (int num4 = 312; num4 <= 314; num4++)
			{
				int num5 = num4 - 312;
				UILinkPoint uILinkPoint4 = new UILinkPoint(num4, enabled: true, 100 + num5, -4, num4 - 1, num4 + 1);
				if (num5 == 0)
				{
					uILinkPoint4.Up = -1;
				}
				if (num5 == 2)
				{
					uILinkPoint4.Down = -2;
				}
				uILinkPoint4.OnSpecialInteracts += value;
				cp16.LinkMap.Add(num4, uILinkPoint4);
			}
			cp16.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 0;
			cp16.UpdateEvent += delegate
			{
				int num60 = 107;
				int amountOfExtraAccessorySlotsToShow = Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow();
				for (int num61 = 0; num61 < amountOfExtraAccessorySlotsToShow; num61++)
				{
					cp16.LinkMap[num60 + num61].Down = num60 + num61 + 1;
					cp16.LinkMap[num60 - 100 + 120 + num61].Down = num60 - 100 + 120 + num61 + 1;
					cp16.LinkMap[num60 + 10 + num61].Down = num60 + 10 + num61 + 1;
				}
				cp16.LinkMap[num60 + amountOfExtraAccessorySlotsToShow].Down = 308;
				cp16.LinkMap[num60 + 10 + amountOfExtraAccessorySlotsToShow].Down = 308;
				cp16.LinkMap[num60 - 100 + 120 + amountOfExtraAccessorySlotsToShow].Down = 308;
				bool shouldPVPDraw = Main.ShouldPVPDraw;
				for (int num62 = 120; num62 <= 129; num62++)
				{
					UILinkPoint uILinkPoint17 = cp16.LinkMap[num62];
					int num63 = num62 - 120;
					uILinkPoint17.Left = -3;
					if (num63 == 0)
					{
						uILinkPoint17.Left = (shouldPVPDraw ? 1550 : (-3));
					}
					if (num63 == 1)
					{
						uILinkPoint17.Left = (shouldPVPDraw ? 1552 : (-3));
					}
					if (num63 == 2)
					{
						uILinkPoint17.Left = (shouldPVPDraw ? 1556 : (-3));
					}
					if (num63 == 3)
					{
						uILinkPoint17.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 1) ? 1558 : (-3));
					}
					if (num63 == 4)
					{
						uILinkPoint17.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 5) ? 1562 : (-3));
					}
					if (num63 == 5)
					{
						uILinkPoint17.Left = ((UILinkPointNavigator.Shortcuts.INFOACCCOUNT >= 9) ? 1566 : (-3));
					}
				}
				cp16.LinkMap[num60 - 100 + 120 + amountOfExtraAccessorySlotsToShow].Left = 1557;
				cp16.LinkMap[num60 - 100 + 120 + amountOfExtraAccessorySlotsToShow - 1].Left = 1570;
			};
			cp16.PageOnLeft = 8;
			cp16.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp16, 3);
			UILinkPage uILinkPage2 = new UILinkPage();
			uILinkPage2.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value8 = delegate
			{
				int slot10 = UILinkPointNavigator.CurrentPoint - 400;
				int context = 4;
				Item[] item = Main.player[Main.myPlayer].bank.item;
				switch (Main.player[Main.myPlayer].chest)
				{
				case -1:
					return "";
				case -3:
					item = Main.player[Main.myPlayer].bank2.item;
					break;
				case -4:
					item = Main.player[Main.myPlayer].bank3.item;
					break;
				case -5:
					item = Main.player[Main.myPlayer].bank4.item;
					context = 32;
					break;
				default:
					item = Main.chest[Main.player[Main.myPlayer].chest].item;
					context = 3;
					break;
				case -2:
					break;
				}
				return ItemSlot.GetGamepadInstructions(item, context, slot10);
			};
			for (int num6 = 400; num6 <= 439; num6++)
			{
				UILinkPoint uILinkPoint5 = new UILinkPoint(num6, enabled: true, num6 - 1, num6 + 1, num6 - 10, num6 + 10);
				uILinkPoint5.OnSpecialInteracts += value8;
				int num7 = num6 - 400;
				if (num7 < 10)
				{
					uILinkPoint5.Up = 40 + num7;
				}
				if (num7 >= 30)
				{
					uILinkPoint5.Down = -2;
				}
				if (num7 % 10 == 9)
				{
					uILinkPoint5.Right = -4;
				}
				if (num7 % 10 == 0)
				{
					uILinkPoint5.Left = -3;
				}
				uILinkPage2.LinkMap.Add(num6, uILinkPoint5);
			}
			uILinkPage2.LinkMap.Add(500, new UILinkPoint(500, enabled: true, 409, -4, 53, 501));
			uILinkPage2.LinkMap.Add(501, new UILinkPoint(501, enabled: true, 419, -4, 500, 502));
			uILinkPage2.LinkMap.Add(502, new UILinkPoint(502, enabled: true, 429, -4, 501, 503));
			uILinkPage2.LinkMap.Add(503, new UILinkPoint(503, enabled: true, 439, -4, 502, 505));
			uILinkPage2.LinkMap.Add(505, new UILinkPoint(505, enabled: true, 439, -4, 503, 504));
			uILinkPage2.LinkMap.Add(504, new UILinkPoint(504, enabled: true, 439, -4, 505, 50));
			uILinkPage2.LinkMap.Add(506, new UILinkPoint(506, enabled: true, 439, -4, 505, 50));
			uILinkPage2.LinkMap[500].OnSpecialInteracts += value;
			uILinkPage2.LinkMap[501].OnSpecialInteracts += value;
			uILinkPage2.LinkMap[502].OnSpecialInteracts += value;
			uILinkPage2.LinkMap[503].OnSpecialInteracts += value;
			uILinkPage2.LinkMap[504].OnSpecialInteracts += value;
			uILinkPage2.LinkMap[505].OnSpecialInteracts += value;
			uILinkPage2.LinkMap[506].OnSpecialInteracts += value;
			uILinkPage2.LinkMap[409].Right = 500;
			uILinkPage2.LinkMap[419].Right = 501;
			uILinkPage2.LinkMap[429].Right = 502;
			uILinkPage2.LinkMap[439].Right = 503;
			uILinkPage2.LinkMap[439].Down = 300;
			uILinkPage2.LinkMap[438].Down = 309;
			uILinkPage2.LinkMap[437].Down = 310;
			uILinkPage2.PageOnLeft = 0;
			uILinkPage2.PageOnRight = 0;
			uILinkPage2.DefaultPoint = 400;
			UILinkPointNavigator.RegisterPage(uILinkPage2, 4, automatedDefault: false);
			uILinkPage2.IsValidEvent += () => Main.playerInventory && Main.player[Main.myPlayer].chest != -1;
			UILinkPage uILinkPage3 = new UILinkPage();
			uILinkPage3.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value9 = delegate
			{
				int slot9 = UILinkPointNavigator.CurrentPoint - 5100;
				return (!(Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEDisplayDoll tEDisplayDoll)) ? "" : tEDisplayDoll.GetItemGamepadInstructions(slot9);
			};
			for (int num8 = 5100; num8 <= 5115; num8++)
			{
				UILinkPoint uILinkPoint6 = new UILinkPoint(num8, enabled: true, num8 - 1, num8 + 1, num8 - 8, num8 + 8);
				uILinkPoint6.OnSpecialInteracts += value9;
				int num9 = num8 - 5100;
				if (num9 < 8)
				{
					uILinkPoint6.Up = 40 + num9;
				}
				if (num9 >= 8)
				{
					uILinkPoint6.Down = -2;
				}
				if (num9 % 8 == 7)
				{
					uILinkPoint6.Right = -4;
				}
				if (num9 % 8 == 0)
				{
					uILinkPoint6.Left = -3;
				}
				uILinkPage3.LinkMap.Add(num8, uILinkPoint6);
			}
			uILinkPage3.PageOnLeft = 0;
			uILinkPage3.PageOnRight = 0;
			uILinkPage3.DefaultPoint = 5100;
			UILinkPointNavigator.RegisterPage(uILinkPage3, 20, automatedDefault: false);
			uILinkPage3.IsValidEvent += () => Main.playerInventory && Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEDisplayDoll;
			UILinkPage uILinkPage4 = new UILinkPage();
			uILinkPage4.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value10 = delegate
			{
				int slot8 = UILinkPointNavigator.CurrentPoint - 5000;
				return (!(Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEHatRack tEHatRack)) ? "" : tEHatRack.GetItemGamepadInstructions(slot8);
			};
			for (int num10 = 5000; num10 <= 5003; num10++)
			{
				UILinkPoint uILinkPoint7 = new UILinkPoint(num10, enabled: true, num10 - 1, num10 + 1, num10 - 2, num10 + 2);
				uILinkPoint7.OnSpecialInteracts += value10;
				int num11 = num10 - 5000;
				if (num11 < 2)
				{
					uILinkPoint7.Up = 44 + num11;
				}
				if (num11 >= 2)
				{
					uILinkPoint7.Down = -2;
				}
				if (num11 % 2 == 1)
				{
					uILinkPoint7.Right = -4;
				}
				if (num11 % 2 == 0)
				{
					uILinkPoint7.Left = -3;
				}
				uILinkPage4.LinkMap.Add(num10, uILinkPoint7);
			}
			uILinkPage4.PageOnLeft = 0;
			uILinkPage4.PageOnRight = 0;
			uILinkPage4.DefaultPoint = 5000;
			UILinkPointNavigator.RegisterPage(uILinkPage4, 21, automatedDefault: false);
			uILinkPage4.IsValidEvent += () => Main.playerInventory && Main.LocalPlayer.tileEntityAnchor.GetTileEntity() is TEHatRack;
			UILinkPage uILinkPage5 = new UILinkPage();
			uILinkPage5.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value11 = delegate
			{
				if (Main.npcShop == 0)
				{
					return "";
				}
				int slot7 = UILinkPointNavigator.CurrentPoint - 2700;
				return ItemSlot.GetGamepadInstructions(Main.instance.shop[Main.npcShop].item, 15, slot7);
			};
			for (int num12 = 2700; num12 <= 2739; num12++)
			{
				UILinkPoint uILinkPoint8 = new UILinkPoint(num12, enabled: true, num12 - 1, num12 + 1, num12 - 10, num12 + 10);
				uILinkPoint8.OnSpecialInteracts += value11;
				int num13 = num12 - 2700;
				if (num13 < 10)
				{
					uILinkPoint8.Up = 40 + num13;
				}
				if (num13 >= 30)
				{
					uILinkPoint8.Down = -2;
				}
				if (num13 % 10 == 9)
				{
					uILinkPoint8.Right = -4;
				}
				if (num13 % 10 == 0)
				{
					uILinkPoint8.Left = -3;
				}
				uILinkPage5.LinkMap.Add(num12, uILinkPoint8);
			}
			uILinkPage5.LinkMap[2739].Down = 300;
			uILinkPage5.LinkMap[2738].Down = 309;
			uILinkPage5.LinkMap[2737].Down = 310;
			uILinkPage5.PageOnLeft = 0;
			uILinkPage5.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(uILinkPage5, 13);
			uILinkPage5.IsValidEvent += () => Main.playerInventory && Main.npcShop != 0;
			UILinkPage cp15 = new UILinkPage();
			cp15.LinkMap.Add(303, new UILinkPoint(303, enabled: true, 304, 304, 40, -2));
			cp15.LinkMap.Add(304, new UILinkPoint(304, enabled: true, 303, 303, 40, -2));
			cp15.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value12 = () => ItemSlot.GetGamepadInstructions(ref Main.reforgeItem, 5);
			cp15.LinkMap[303].OnSpecialInteracts += value12;
			cp15.LinkMap[304].OnSpecialInteracts += () => Lang.misc[53].Value;
			cp15.UpdateEvent += delegate
			{
				if (Main.reforgeItem.type > 0)
				{
					cp15.LinkMap[303].Left = (cp15.LinkMap[303].Right = 304);
				}
				else
				{
					if (UILinkPointNavigator.OverridePoint == -1 && cp15.CurrentPoint == 304)
					{
						UILinkPointNavigator.ChangePoint(303);
					}
					cp15.LinkMap[303].Left = -3;
					cp15.LinkMap[303].Right = -4;
				}
			};
			cp15.IsValidEvent += () => Main.playerInventory && Main.InReforgeMenu;
			cp15.PageOnLeft = 0;
			cp15.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(cp15, 5);
			UILinkPage cp14 = new UILinkPage();
			cp14.OnSpecialInteracts += delegate
			{
				bool flag5 = UILinkPointNavigator.CurrentPoint == 600;
				bool flag6 = !flag5 && WorldGen.IsNPCEvictable(UILinkPointNavigator.Shortcuts.NPCS_LastHovered);
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
				{
					Point point = Main.player[Main.myPlayer].Center.ToTileCoordinates();
					if (flag5)
					{
						if (WorldGen.MoveTownNPC(point.X, point.Y, -1))
						{
							Main.NewText(Lang.inter[39].Value, byte.MaxValue, 240, 20);
						}
						SoundEngine.PlaySound(12);
					}
					else if (WorldGen.MoveTownNPC(point.X, point.Y, UILinkPointNavigator.Shortcuts.NPCS_LastHovered))
					{
						WorldGen.moveRoom(point.X, point.Y, UILinkPointNavigator.Shortcuts.NPCS_LastHovered);
						SoundEngine.PlaySound(12);
					}
					PlayerInput.LockGamepadButtons("Grapple");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
				{
					UILinkPointNavigator.Shortcuts.NPCS_IconsDisplay = !UILinkPointNavigator.Shortcuts.NPCS_IconsDisplay;
					PlayerInput.LockGamepadButtons("SmartSelect");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				if (flag6 && CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseRight)
				{
					WorldGen.kickOut(UILinkPointNavigator.Shortcuts.NPCS_LastHovered);
				}
				return PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]) + PlayerInput.BuildCommand(Lang.misc[70].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]) + PlayerInput.BuildCommand(Lang.misc[69].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]) + (flag6 ? PlayerInput.BuildCommand("Evict", false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]) : "");
			};
			for (int num14 = 600; num14 <= 650; num14++)
			{
				UILinkPoint value13 = new UILinkPoint(num14, enabled: true, num14 + 10, num14 - 10, num14 - 1, num14 + 1);
				cp14.LinkMap.Add(num14, value13);
			}
			cp14.UpdateEvent += delegate
			{
				int num58 = UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn;
				if (num58 == 0)
				{
					num58 = 100;
				}
				for (int num59 = 0; num59 < 50; num59++)
				{
					cp14.LinkMap[600 + num59].Up = ((num59 % num58 == 0) ? (-1) : (600 + num59 - 1));
					if (cp14.LinkMap[600 + num59].Up == -1)
					{
						if (num59 >= num58 * 2)
						{
							cp14.LinkMap[600 + num59].Up = 307;
						}
						else if (num59 >= num58)
						{
							cp14.LinkMap[600 + num59].Up = 306;
						}
						else
						{
							cp14.LinkMap[600 + num59].Up = 305;
						}
					}
					cp14.LinkMap[600 + num59].Down = (((num59 + 1) % num58 == 0 || num59 == UILinkPointNavigator.Shortcuts.NPCS_IconsTotal - 1) ? 308 : (600 + num59 + 1));
					cp14.LinkMap[600 + num59].Left = ((num59 < UILinkPointNavigator.Shortcuts.NPCS_IconsTotal - num58) ? (600 + num59 + num58) : (-3));
					cp14.LinkMap[600 + num59].Right = ((num59 < num58) ? (-4) : (600 + num59 - num58));
				}
			};
			cp14.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 1;
			cp14.PageOnLeft = 8;
			cp14.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp14, 6);
			UILinkPage cp13 = new UILinkPage();
			cp13.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value14 = delegate
			{
				int slot6 = UILinkPointNavigator.CurrentPoint - 180;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 20, slot6);
			};
			Func<string> value15 = delegate
			{
				int slot5 = UILinkPointNavigator.CurrentPoint - 180;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 19, slot5);
			};
			Func<string> value16 = delegate
			{
				int slot4 = UILinkPointNavigator.CurrentPoint - 180;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 18, slot4);
			};
			Func<string> value17 = delegate
			{
				int slot3 = UILinkPointNavigator.CurrentPoint - 180;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 17, slot3);
			};
			Func<string> value18 = delegate
			{
				int slot2 = UILinkPointNavigator.CurrentPoint - 180;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscEquips, 16, slot2);
			};
			Func<string> value19 = delegate
			{
				int slot = UILinkPointNavigator.CurrentPoint - 185;
				return ItemSlot.GetGamepadInstructions(Main.player[Main.myPlayer].miscDyes, 33, slot);
			};
			for (int num15 = 180; num15 <= 184; num15++)
			{
				UILinkPoint uILinkPoint9 = new UILinkPoint(num15, enabled: true, 185 + num15 - 180, -4, num15 - 1, num15 + 1);
				int num16 = num15 - 180;
				if (num16 == 0)
				{
					uILinkPoint9.Up = 305;
				}
				if (num16 == 4)
				{
					uILinkPoint9.Down = 308;
				}
				cp13.LinkMap.Add(num15, uILinkPoint9);
				switch (num15)
				{
				case 180:
					uILinkPoint9.OnSpecialInteracts += value15;
					break;
				case 181:
					uILinkPoint9.OnSpecialInteracts += value14;
					break;
				case 182:
					uILinkPoint9.OnSpecialInteracts += value16;
					break;
				case 183:
					uILinkPoint9.OnSpecialInteracts += value17;
					break;
				case 184:
					uILinkPoint9.OnSpecialInteracts += value18;
					break;
				}
			}
			for (int num17 = 185; num17 <= 189; num17++)
			{
				UILinkPoint uILinkPoint9 = new UILinkPoint(num17, enabled: true, -3, -5 + num17, num17 - 1, num17 + 1);
				uILinkPoint9.OnSpecialInteracts += value19;
				int num18 = num17 - 185;
				if (num18 == 0)
				{
					uILinkPoint9.Up = 306;
				}
				if (num18 == 4)
				{
					uILinkPoint9.Down = 308;
				}
				cp13.LinkMap.Add(num17, uILinkPoint9);
			}
			cp13.UpdateEvent += delegate
			{
				cp13.LinkMap[184].Down = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 308);
				cp13.LinkMap[189].Down = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 308);
			};
			cp13.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 2;
			cp13.PageOnLeft = 8;
			cp13.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp13, 7);
			UILinkPage cp12 = new UILinkPage();
			cp12.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			cp12.LinkMap.Add(305, new UILinkPoint(305, enabled: true, 306, -4, 308, -2));
			cp12.LinkMap.Add(306, new UILinkPoint(306, enabled: true, 307, 305, 308, -2));
			cp12.LinkMap.Add(307, new UILinkPoint(307, enabled: true, -3, 306, 308, -2));
			cp12.LinkMap.Add(308, new UILinkPoint(308, enabled: true, -3, -4, -1, 305));
			cp12.LinkMap[305].OnSpecialInteracts += value;
			cp12.LinkMap[306].OnSpecialInteracts += value;
			cp12.LinkMap[307].OnSpecialInteracts += value;
			cp12.LinkMap[308].OnSpecialInteracts += value;
			cp12.UpdateEvent += delegate
			{
				switch (Main.EquipPage)
				{
				case 0:
					cp12.LinkMap[305].Down = 100;
					cp12.LinkMap[306].Down = 110;
					cp12.LinkMap[307].Down = 120;
					cp12.LinkMap[308].Up = 108 + Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow() - 1;
					break;
				case 1:
				{
					cp12.LinkMap[305].Down = 600;
					cp12.LinkMap[306].Down = ((UILinkPointNavigator.Shortcuts.NPCS_IconsTotal / UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn > 0) ? (600 + UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn) : (-2));
					cp12.LinkMap[307].Down = ((UILinkPointNavigator.Shortcuts.NPCS_IconsTotal / UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn > 1) ? (600 + UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn * 2) : (-2));
					int num57 = UILinkPointNavigator.Shortcuts.NPCS_IconsPerColumn;
					if (num57 == 0)
					{
						num57 = 100;
					}
					if (num57 == 100)
					{
						num57 = UILinkPointNavigator.Shortcuts.NPCS_IconsTotal;
					}
					cp12.LinkMap[308].Up = 600 + num57 - 1;
					break;
				}
				case 2:
					cp12.LinkMap[305].Down = 180;
					cp12.LinkMap[306].Down = 185;
					cp12.LinkMap[307].Down = -2;
					cp12.LinkMap[308].Up = ((UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0) ? 9000 : 184);
					break;
				}
				cp12.PageOnRight = GetCornerWrapPageIdFromRightToLeft();
			};
			cp12.IsValidEvent += () => Main.playerInventory;
			cp12.PageOnLeft = 0;
			cp12.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(cp12, 8);
			UILinkPage cp11 = new UILinkPage();
			cp11.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value20 = () => ItemSlot.GetGamepadInstructions(ref Main.guideItem, 7);
			Func<string> HandleItem2 = () => (Main.mouseItem.type < 1) ? "" : ItemSlot.GetGamepadInstructions(ref Main.mouseItem, 22);
			for (int num19 = 1500; num19 < 1550; num19++)
			{
				UILinkPoint uILinkPoint10 = new UILinkPoint(num19, enabled: true, num19, num19, -1, -2);
				if (num19 != 1500)
				{
					uILinkPoint10.OnSpecialInteracts += HandleItem2;
				}
				cp11.LinkMap.Add(num19, uILinkPoint10);
			}
			cp11.LinkMap[1500].OnSpecialInteracts += value20;
			cp11.UpdateEvent += delegate
			{
				int num54 = UILinkPointNavigator.Shortcuts.CRAFT_CurrentIngredientsCount;
				int num55 = num54;
				if (Main.numAvailableRecipes > 0)
				{
					num55 += 2;
				}
				if (num54 < num55)
				{
					num54 = num55;
				}
				if (UILinkPointNavigator.OverridePoint == -1 && cp11.CurrentPoint > 1500 + num54)
				{
					UILinkPointNavigator.ChangePoint(1500);
				}
				if (UILinkPointNavigator.OverridePoint == -1 && cp11.CurrentPoint == 1500 && !Main.InGuideCraftMenu)
				{
					UILinkPointNavigator.ChangePoint(1501);
				}
				for (int num56 = 1; num56 < num54; num56++)
				{
					cp11.LinkMap[1500 + num56].Left = 1500 + num56 - 1;
					cp11.LinkMap[1500 + num56].Right = ((num56 == num54 - 2) ? (-4) : (1500 + num56 + 1));
				}
				cp11.LinkMap[1501].Left = -3;
				if (num54 > 0)
				{
					cp11.LinkMap[1500 + num54 - 1].Right = -4;
				}
				cp11.LinkMap[1500].Down = ((num54 >= 2) ? 1502 : (-2));
				cp11.LinkMap[1500].Left = ((num54 >= 1) ? 1501 : (-3));
				cp11.LinkMap[1502].Up = (Main.InGuideCraftMenu ? 1500 : (-1));
			};
			cp11.LinkMap[1501].OnSpecialInteracts += delegate
			{
				if (Main.InGuideCraftMenu)
				{
					return "";
				}
				string text2 = "";
				Player player2 = Main.player[Main.myPlayer];
				bool flag3 = false;
				Item createItem = Main.recipe[Main.availableRecipe[Main.focusRecipe]].createItem;
				if (Main.mouseItem.type == 0 && createItem.maxStack > 1 && player2.ItemSpace(createItem).CanTakeItemToPersonalInventory && !player2.HasLockedInventory())
				{
					flag3 = true;
					if (CanExecuteInputCommand() && PlayerInput.Triggers.Current.Grapple && Main.stackSplit <= 1)
					{
						if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							SomeVarsForUILinkers.SequencedCraftingCurrent = Main.recipe[Main.availableRecipe[Main.focusRecipe]];
						}
						ItemSlot.RefreshStackSplitCooldown();
						Main.preventStackSplitReset = true;
						if (SomeVarsForUILinkers.SequencedCraftingCurrent == Main.recipe[Main.availableRecipe[Main.focusRecipe]])
						{
							Main.CraftItem(Main.recipe[Main.availableRecipe[Main.focusRecipe]]);
							Main.mouseItem = player2.GetItem(player2.whoAmI, Main.mouseItem, new GetItemSettings(LongText: false, NoText: true));
						}
					}
				}
				else if (Main.mouseItem.type > 0 && Main.mouseItem.maxStack == 1 && ItemSlot.Equippable(ref Main.mouseItem))
				{
					text2 += PlayerInput.BuildCommand(Lang.misc[67].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						ItemSlot.SwapEquip(ref Main.mouseItem);
						if (Main.player[Main.myPlayer].ItemSpace(Main.mouseItem).CanTakeItemToPersonalInventory)
						{
							Main.mouseItem = player2.GetItem(player2.whoAmI, Main.mouseItem, GetItemSettings.InventoryUIToInventorySettings);
						}
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				bool flag4 = Main.mouseItem.stack <= 0;
				if (flag4 || (Main.mouseItem.type == createItem.type && Main.mouseItem.stack < Main.mouseItem.maxStack))
				{
					text2 = ((!flag4) ? (text2 + PlayerInput.BuildCommand(Lang.misc[72].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"])) : (text2 + PlayerInput.BuildCommand(Lang.misc[72].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"], PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"])));
				}
				if (!flag4 && Main.mouseItem.type == createItem.type && Main.mouseItem.stack < Main.mouseItem.maxStack)
				{
					text2 += PlayerInput.BuildCommand(Lang.misc[93].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
				}
				if (flag3)
				{
					text2 += PlayerInput.BuildCommand(Lang.misc[71].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
				}
				return text2 + HandleItem2();
			};
			cp11.ReachEndEvent += delegate(int current, int next)
			{
				switch (current)
				{
				case 1501:
					switch (next)
					{
					case -1:
						if (Main.focusRecipe > 0)
						{
							Main.focusRecipe--;
						}
						break;
					case -2:
						if (Main.focusRecipe < Main.numAvailableRecipes - 1)
						{
							Main.focusRecipe++;
						}
						break;
					}
					break;
				default:
					switch (next)
					{
					case -1:
						if (Main.focusRecipe > 0)
						{
							UILinkPointNavigator.ChangePoint(1501);
							Main.focusRecipe--;
						}
						break;
					case -2:
						if (Main.focusRecipe < Main.numAvailableRecipes - 1)
						{
							UILinkPointNavigator.ChangePoint(1501);
							Main.focusRecipe++;
						}
						break;
					}
					break;
				case 1500:
					break;
				}
			};
			cp11.EnterEvent += delegate
			{
				Main.recBigList = false;
			};
			cp11.CanEnterEvent += () => Main.playerInventory && (Main.numAvailableRecipes > 0 || Main.InGuideCraftMenu);
			cp11.IsValidEvent += () => Main.playerInventory && (Main.numAvailableRecipes > 0 || Main.InGuideCraftMenu);
			cp11.PageOnLeft = 10;
			cp11.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(cp11, 9);
			UILinkPage cp10 = new UILinkPage();
			cp10.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			for (int num20 = 700; num20 < 1500; num20++)
			{
				UILinkPoint uILinkPoint11 = new UILinkPoint(num20, enabled: true, num20, num20, num20, num20);
				int IHateLambda = num20;
				uILinkPoint11.OnSpecialInteracts += delegate
				{
					string text = "";
					bool flag2 = false;
					Player player = Main.player[Main.myPlayer];
					if (IHateLambda + Main.recStart < Main.numAvailableRecipes)
					{
						int num53 = Main.recStart + IHateLambda - 700;
						if (Main.mouseItem.type == 0 && Main.recipe[Main.availableRecipe[num53]].createItem.maxStack > 1 && player.ItemSpace(Main.recipe[Main.availableRecipe[num53]].createItem).CanTakeItemToPersonalInventory && !player.HasLockedInventory())
						{
							flag2 = true;
							if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Grapple)
							{
								SomeVarsForUILinkers.SequencedCraftingCurrent = Main.recipe[Main.availableRecipe[num53]];
							}
							if (CanExecuteInputCommand() && PlayerInput.Triggers.Current.Grapple && Main.stackSplit <= 1)
							{
								ItemSlot.RefreshStackSplitCooldown();
								if (SomeVarsForUILinkers.SequencedCraftingCurrent == Main.recipe[Main.availableRecipe[num53]])
								{
									Main.CraftItem(Main.recipe[Main.availableRecipe[num53]]);
									Main.mouseItem = player.GetItem(player.whoAmI, Main.mouseItem, GetItemSettings.InventoryUIToInventorySettings);
								}
							}
						}
					}
					text += PlayerInput.BuildCommand(Lang.misc[73].Value, !flag2, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					if (flag2)
					{
						text += PlayerInput.BuildCommand(Lang.misc[71].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					}
					return text;
				};
				cp10.LinkMap.Add(num20, uILinkPoint11);
			}
			cp10.UpdateEvent += delegate
			{
				int num50 = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
				int cRAFT_IconsPerColumn = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerColumn;
				if (num50 == 0)
				{
					num50 = 100;
				}
				int num51 = num50 * cRAFT_IconsPerColumn;
				if (num51 > 800)
				{
					num51 = 800;
				}
				if (num51 > Main.numAvailableRecipes)
				{
					num51 = Main.numAvailableRecipes;
				}
				for (int num52 = 0; num52 < num51; num52++)
				{
					cp10.LinkMap[700 + num52].Left = ((num52 % num50 == 0) ? (-3) : (700 + num52 - 1));
					cp10.LinkMap[700 + num52].Right = (((num52 + 1) % num50 == 0 || num52 == Main.numAvailableRecipes - 1) ? (-4) : (700 + num52 + 1));
					cp10.LinkMap[700 + num52].Down = ((num52 < num51 - num50) ? (700 + num52 + num50) : (-2));
					cp10.LinkMap[700 + num52].Up = ((num52 < num50) ? (-1) : (700 + num52 - num50));
				}
				cp10.PageOnLeft = GetCornerWrapPageIdFromLeftToRight();
			};
			cp10.ReachEndEvent += delegate(int current, int next)
			{
				int cRAFT_IconsPerRow = UILinkPointNavigator.Shortcuts.CRAFT_IconsPerRow;
				switch (next)
				{
				case -1:
					Main.recStart -= cRAFT_IconsPerRow;
					if (Main.recStart < 0)
					{
						Main.recStart = 0;
					}
					break;
				case -2:
					Main.recStart += cRAFT_IconsPerRow;
					SoundEngine.PlaySound(12);
					if (Main.recStart > Main.numAvailableRecipes - cRAFT_IconsPerRow)
					{
						Main.recStart = Main.numAvailableRecipes - cRAFT_IconsPerRow;
					}
					break;
				}
			};
			cp10.EnterEvent += delegate
			{
				Main.recBigList = true;
			};
			cp10.LeaveEvent += delegate
			{
				Main.recBigList = false;
			};
			cp10.CanEnterEvent += () => Main.playerInventory && Main.numAvailableRecipes > 0;
			cp10.IsValidEvent += () => Main.playerInventory && Main.recBigList && Main.numAvailableRecipes > 0;
			cp10.PageOnLeft = 0;
			cp10.PageOnRight = 9;
			UILinkPointNavigator.RegisterPage(cp10, 10);
			UILinkPage cp9 = new UILinkPage();
			cp9.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			for (int num21 = 2605; num21 < 2620; num21++)
			{
				UILinkPoint uILinkPoint12 = new UILinkPoint(num21, enabled: true, num21, num21, num21, num21);
				uILinkPoint12.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[73].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				cp9.LinkMap.Add(num21, uILinkPoint12);
			}
			cp9.UpdateEvent += delegate
			{
				int num46 = 5;
				int num47 = 3;
				int num48 = num46 * num47;
				int count = Main.Hairstyles.AvailableHairstyles.Count;
				for (int num49 = 0; num49 < num48; num49++)
				{
					cp9.LinkMap[2605 + num49].Left = ((num49 % num46 == 0) ? (-3) : (2605 + num49 - 1));
					cp9.LinkMap[2605 + num49].Right = (((num49 + 1) % num46 == 0 || num49 == count - 1) ? (-4) : (2605 + num49 + 1));
					cp9.LinkMap[2605 + num49].Down = ((num49 < num48 - num46) ? (2605 + num49 + num46) : (-2));
					cp9.LinkMap[2605 + num49].Up = ((num49 < num46) ? (-1) : (2605 + num49 - num46));
				}
			};
			cp9.ReachEndEvent += delegate(int current, int next)
			{
				int num45 = 5;
				switch (next)
				{
				case -1:
					Main.hairStart -= num45;
					SoundEngine.PlaySound(12);
					break;
				case -2:
					Main.hairStart += num45;
					SoundEngine.PlaySound(12);
					break;
				}
			};
			cp9.CanEnterEvent += () => Main.hairWindow;
			cp9.IsValidEvent += () => Main.hairWindow;
			cp9.PageOnLeft = 12;
			cp9.PageOnRight = 12;
			UILinkPointNavigator.RegisterPage(cp9, 11);
			UILinkPage uILinkPage6 = new UILinkPage();
			uILinkPage6.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			uILinkPage6.LinkMap.Add(2600, new UILinkPoint(2600, enabled: true, -3, -4, -1, 2601));
			uILinkPage6.LinkMap.Add(2601, new UILinkPoint(2601, enabled: true, -3, -4, 2600, 2602));
			uILinkPage6.LinkMap.Add(2602, new UILinkPoint(2602, enabled: true, -3, -4, 2601, 2603));
			uILinkPage6.LinkMap.Add(2603, new UILinkPoint(2603, enabled: true, -3, 2604, 2602, -2));
			uILinkPage6.LinkMap.Add(2604, new UILinkPoint(2604, enabled: true, 2603, -4, 2602, -2));
			uILinkPage6.UpdateEvent += delegate
			{
				Vector3 value23 = Main.rgbToHsl(Main.selColor);
				float interfaceDeadzoneX2 = PlayerInput.CurrentProfile.InterfaceDeadzoneX;
				float x2 = PlayerInput.GamepadThumbstickLeft.X;
				x2 = ((!(x2 < 0f - interfaceDeadzoneX2) && !(x2 > interfaceDeadzoneX2)) ? 0f : (MathHelper.Lerp(0f, 1f / 120f, (Math.Abs(x2) - interfaceDeadzoneX2) / (1f - interfaceDeadzoneX2)) * (float)Math.Sign(x2)));
				int currentPoint2 = UILinkPointNavigator.CurrentPoint;
				if (currentPoint2 == 2600)
				{
					Main.hBar = MathHelper.Clamp(Main.hBar + x2, 0f, 1f);
				}
				if (currentPoint2 == 2601)
				{
					Main.sBar = MathHelper.Clamp(Main.sBar + x2, 0f, 1f);
				}
				if (currentPoint2 == 2602)
				{
					Main.lBar = MathHelper.Clamp(Main.lBar + x2, 0.15f, 1f);
				}
				Vector3.Clamp(value23, Vector3.Zero, Vector3.One);
				if (x2 != 0f)
				{
					if (Main.hairWindow)
					{
						Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
					}
					SoundEngine.PlaySound(12);
				}
			};
			uILinkPage6.CanEnterEvent += () => Main.hairWindow;
			uILinkPage6.IsValidEvent += () => Main.hairWindow;
			uILinkPage6.PageOnLeft = 11;
			uILinkPage6.PageOnRight = 11;
			UILinkPointNavigator.RegisterPage(uILinkPage6, 12);
			UILinkPage cp8 = new UILinkPage();
			for (int num22 = 0; num22 < 30; num22++)
			{
				cp8.LinkMap.Add(2900 + num22, new UILinkPoint(2900 + num22, enabled: true, -3, -4, -1, -2));
				cp8.LinkMap[2900 + num22].OnSpecialInteracts += value;
			}
			cp8.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			cp8.TravelEvent += delegate
			{
				if (UILinkPointNavigator.CurrentPage == cp8.ID)
				{
					int num44 = cp8.CurrentPoint - 2900;
					if (num44 < 5)
					{
						IngameOptions.category = num44;
					}
				}
			};
			cp8.UpdateEvent += delegate
			{
				int num40 = UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_LEFT;
				if (num40 == 0)
				{
					num40 = 5;
				}
				if (UILinkPointNavigator.OverridePoint == -1 && cp8.CurrentPoint < 2930 && cp8.CurrentPoint > 2900 + num40 - 1)
				{
					UILinkPointNavigator.ChangePoint(2900);
				}
				for (int num41 = 2900; num41 < 2900 + num40; num41++)
				{
					cp8.LinkMap[num41].Up = num41 - 1;
					cp8.LinkMap[num41].Down = num41 + 1;
				}
				cp8.LinkMap[2900].Up = 2900 + num40 - 1;
				cp8.LinkMap[2900 + num40 - 1].Down = 2900;
				int num42 = cp8.CurrentPoint - 2900;
				if (num42 < 4 && CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseLeft)
				{
					IngameOptions.category = num42;
					UILinkPointNavigator.ChangePage(1002);
				}
				int num43 = ((SocialAPI.Network != null && SocialAPI.Network.CanInvite()) ? 1 : 0);
				if (num42 == 4 + num43 && CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseLeft)
				{
					UILinkPointNavigator.ChangePage(1004);
				}
			};
			cp8.EnterEvent += delegate
			{
				cp8.CurrentPoint = 2900 + IngameOptions.category;
			};
			cp8.PageOnLeft = (cp8.PageOnRight = 1002);
			cp8.IsValidEvent += () => Main.ingameOptionsWindow && !Main.InGameUI.IsVisible;
			cp8.CanEnterEvent += () => Main.ingameOptionsWindow && !Main.InGameUI.IsVisible;
			UILinkPointNavigator.RegisterPage(cp8, 1001);
			UILinkPage cp7 = new UILinkPage();
			for (int num23 = 0; num23 < 30; num23++)
			{
				cp7.LinkMap.Add(2930 + num23, new UILinkPoint(2930 + num23, enabled: true, -3, -4, -1, -2));
				cp7.LinkMap[2930 + num23].OnSpecialInteracts += value;
			}
			cp7.EnterEvent += delegate
			{
				Main.mouseLeftRelease = false;
			};
			cp7.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			cp7.UpdateEvent += delegate
			{
				int num38 = UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT;
				if (num38 == 0)
				{
					num38 = 5;
				}
				if (UILinkPointNavigator.OverridePoint == -1 && cp7.CurrentPoint >= 2930 && cp7.CurrentPoint > 2930 + num38 - 1)
				{
					UILinkPointNavigator.ChangePoint(2930);
				}
				for (int num39 = 2930; num39 < 2930 + num38; num39++)
				{
					cp7.LinkMap[num39].Up = num39 - 1;
					cp7.LinkMap[num39].Down = num39 + 1;
				}
				cp7.LinkMap[2930].Up = -1;
				cp7.LinkMap[2930 + num38 - 1].Down = -2;
				HandleOptionsSpecials();
			};
			cp7.PageOnLeft = (cp7.PageOnRight = 1001);
			cp7.IsValidEvent += () => Main.ingameOptionsWindow;
			cp7.CanEnterEvent += () => Main.ingameOptionsWindow;
			UILinkPointNavigator.RegisterPage(cp7, 1002);
			UILinkPage cp6 = new UILinkPage();
			cp6.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			for (int num24 = 1550; num24 < 1558; num24++)
			{
				UILinkPoint uILinkPoint13 = new UILinkPoint(num24, enabled: true, -3, -4, -1, -2);
				switch (num24 - 1550)
				{
				case 1:
				case 3:
				case 5:
					uILinkPoint13.Up = uILinkPoint13.ID - 2;
					uILinkPoint13.Down = uILinkPoint13.ID + 2;
					uILinkPoint13.Right = uILinkPoint13.ID + 1;
					break;
				case 2:
				case 4:
				case 6:
					uILinkPoint13.Up = uILinkPoint13.ID - 2;
					uILinkPoint13.Down = uILinkPoint13.ID + 2;
					uILinkPoint13.Left = uILinkPoint13.ID - 1;
					break;
				}
				cp6.LinkMap.Add(num24, uILinkPoint13);
			}
			cp6.LinkMap[1550].Down = 1551;
			cp6.LinkMap[1550].Right = 120;
			cp6.LinkMap[1550].Up = 307;
			cp6.LinkMap[1551].Up = 1550;
			cp6.LinkMap[1552].Up = 1550;
			cp6.LinkMap[1552].Right = 121;
			cp6.LinkMap[1554].Right = 121;
			cp6.LinkMap[1555].Down = 1570;
			cp6.LinkMap[1556].Down = 1570;
			cp6.LinkMap[1556].Right = 122;
			cp6.LinkMap[1557].Up = 1570;
			cp6.LinkMap[1557].Down = 308;
			cp6.LinkMap[1557].Right = 127;
			cp6.LinkMap.Add(1570, new UILinkPoint(1570, enabled: true, -3, -4, -1, -2));
			cp6.LinkMap[1570].Up = 1555;
			cp6.LinkMap[1570].Down = 1557;
			cp6.LinkMap[1570].Right = 126;
			for (int num25 = 0; num25 < 7; num25++)
			{
				cp6.LinkMap[1550 + num25].OnSpecialInteracts += value;
			}
			cp6.UpdateEvent += delegate
			{
				if (!Main.ShouldPVPDraw)
				{
					if (UILinkPointNavigator.OverridePoint == -1 && cp6.CurrentPoint != 1557 && cp6.CurrentPoint != 1570)
					{
						UILinkPointNavigator.ChangePoint(1557);
					}
					cp6.LinkMap[1570].Up = -1;
					cp6.LinkMap[1557].Down = 308;
					cp6.LinkMap[1557].Right = 127;
				}
				else
				{
					cp6.LinkMap[1570].Up = 1555;
					cp6.LinkMap[1557].Down = 308;
					cp6.LinkMap[1557].Right = 127;
				}
				int iNFOACCCOUNT2 = UILinkPointNavigator.Shortcuts.INFOACCCOUNT;
				if (iNFOACCCOUNT2 > 0)
				{
					cp6.LinkMap[1570].Up = 1558 + (iNFOACCCOUNT2 - 1) / 2 * 2;
				}
				if (Main.ShouldPVPDraw)
				{
					if (iNFOACCCOUNT2 >= 1)
					{
						cp6.LinkMap[1555].Down = 1558;
						cp6.LinkMap[1556].Down = 1558;
					}
					else
					{
						cp6.LinkMap[1555].Down = 1570;
						cp6.LinkMap[1556].Down = 1570;
					}
					if (iNFOACCCOUNT2 >= 2)
					{
						cp6.LinkMap[1556].Down = 1559;
					}
					else
					{
						cp6.LinkMap[1556].Down = 1570;
					}
				}
			};
			cp6.IsValidEvent += () => Main.playerInventory;
			cp6.PageOnLeft = 8;
			cp6.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp6, 16);
			UILinkPage cp5 = new UILinkPage();
			cp5.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			for (int num26 = 1558; num26 < 1570; num26++)
			{
				UILinkPoint uILinkPoint14 = new UILinkPoint(num26, enabled: true, -3, -4, -1, -2);
				uILinkPoint14.OnSpecialInteracts += value;
				switch (num26 - 1558)
				{
				case 1:
				case 3:
				case 5:
					uILinkPoint14.Up = uILinkPoint14.ID - 2;
					uILinkPoint14.Down = uILinkPoint14.ID + 2;
					uILinkPoint14.Right = uILinkPoint14.ID + 1;
					break;
				case 2:
				case 4:
				case 6:
					uILinkPoint14.Up = uILinkPoint14.ID - 2;
					uILinkPoint14.Down = uILinkPoint14.ID + 2;
					uILinkPoint14.Left = uILinkPoint14.ID - 1;
					break;
				}
				cp5.LinkMap.Add(num26, uILinkPoint14);
			}
			cp5.UpdateEvent += delegate
			{
				int iNFOACCCOUNT = UILinkPointNavigator.Shortcuts.INFOACCCOUNT;
				if (UILinkPointNavigator.OverridePoint == -1 && cp5.CurrentPoint - 1558 >= iNFOACCCOUNT)
				{
					UILinkPointNavigator.ChangePoint(1558 + iNFOACCCOUNT - 1);
				}
				for (int num36 = 0; num36 < iNFOACCCOUNT; num36++)
				{
					bool flag = num36 % 2 == 0;
					int num37 = num36 + 1558;
					cp5.LinkMap[num37].Down = ((num36 < iNFOACCCOUNT - 2) ? (num37 + 2) : 1570);
					cp5.LinkMap[num37].Up = ((num36 > 1) ? (num37 - 2) : (Main.ShouldPVPDraw ? (flag ? 1555 : 1556) : (-1)));
					cp5.LinkMap[num37].Right = ((flag && num36 + 1 < iNFOACCCOUNT) ? (num37 + 1) : (123 + num36 / 4));
					cp5.LinkMap[num37].Left = (flag ? (-3) : (num37 - 1));
				}
			};
			cp5.IsValidEvent += () => Main.playerInventory && UILinkPointNavigator.Shortcuts.INFOACCCOUNT > 0;
			cp5.PageOnLeft = 8;
			cp5.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp5, 17);
			UILinkPage cp4 = new UILinkPage();
			cp4.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			for (int num27 = 6000; num27 < 6012; num27++)
			{
				UILinkPoint uILinkPoint15 = new UILinkPoint(num27, enabled: true, -3, -4, -1, -2);
				switch (num27)
				{
				case 6000:
					uILinkPoint15.Right = 0;
					break;
				case 6001:
				case 6002:
					uILinkPoint15.Right = 10;
					break;
				case 6003:
				case 6004:
					uILinkPoint15.Right = 20;
					break;
				case 6005:
				case 6006:
					uILinkPoint15.Right = 30;
					break;
				default:
					uILinkPoint15.Right = 40;
					break;
				}
				cp4.LinkMap.Add(num27, uILinkPoint15);
			}
			cp4.UpdateEvent += delegate
			{
				int bUILDERACCCOUNT = UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT;
				if (UILinkPointNavigator.OverridePoint == -1 && cp4.CurrentPoint - 6000 >= bUILDERACCCOUNT)
				{
					UILinkPointNavigator.ChangePoint(6000 + bUILDERACCCOUNT - 1);
				}
				for (int num34 = 0; num34 < bUILDERACCCOUNT; num34++)
				{
					_ = num34 % 2;
					int num35 = num34 + 6000;
					cp4.LinkMap[num35].Down = ((num34 < bUILDERACCCOUNT - 1) ? (num35 + 1) : (-2));
					cp4.LinkMap[num35].Up = ((num34 > 0) ? (num35 - 1) : (-1));
				}
			};
			cp4.IsValidEvent += () => Main.playerInventory && UILinkPointNavigator.Shortcuts.BUILDERACCCOUNT > 0;
			cp4.PageOnLeft = 8;
			cp4.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp4, 18);
			UILinkPage uILinkPage7 = new UILinkPage();
			uILinkPage7.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			uILinkPage7.LinkMap.Add(2806, new UILinkPoint(2806, enabled: true, 2805, 2807, -1, 2808));
			uILinkPage7.LinkMap.Add(2807, new UILinkPoint(2807, enabled: true, 2806, 2810, -1, 2809));
			uILinkPage7.LinkMap.Add(2808, new UILinkPoint(2808, enabled: true, 2805, 2809, 2806, -2));
			uILinkPage7.LinkMap.Add(2809, new UILinkPoint(2809, enabled: true, 2808, 2811, 2807, -2));
			uILinkPage7.LinkMap.Add(2810, new UILinkPoint(2810, enabled: true, 2807, -4, -1, 2811));
			uILinkPage7.LinkMap.Add(2811, new UILinkPoint(2811, enabled: true, 2809, -4, 2810, -2));
			uILinkPage7.LinkMap.Add(2805, new UILinkPoint(2805, enabled: true, -3, 2806, -1, -2));
			uILinkPage7.LinkMap[2806].OnSpecialInteracts += value;
			uILinkPage7.LinkMap[2807].OnSpecialInteracts += value;
			uILinkPage7.LinkMap[2808].OnSpecialInteracts += value;
			uILinkPage7.LinkMap[2809].OnSpecialInteracts += value;
			uILinkPage7.LinkMap[2805].OnSpecialInteracts += value;
			uILinkPage7.CanEnterEvent += () => Main.clothesWindow;
			uILinkPage7.IsValidEvent += () => Main.clothesWindow;
			uILinkPage7.EnterEvent += delegate
			{
				Main.player[Main.myPlayer].releaseInventory = false;
			};
			uILinkPage7.LeaveEvent += delegate
			{
				Main.player[Main.myPlayer].LockGamepadTileInteractions();
			};
			uILinkPage7.PageOnLeft = 15;
			uILinkPage7.PageOnRight = 15;
			UILinkPointNavigator.RegisterPage(uILinkPage7, 14);
			UILinkPage uILinkPage8 = new UILinkPage();
			uILinkPage8.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, true, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			uILinkPage8.LinkMap.Add(2800, new UILinkPoint(2800, enabled: true, -3, -4, -1, 2801));
			uILinkPage8.LinkMap.Add(2801, new UILinkPoint(2801, enabled: true, -3, -4, 2800, 2802));
			uILinkPage8.LinkMap.Add(2802, new UILinkPoint(2802, enabled: true, -3, -4, 2801, 2803));
			uILinkPage8.LinkMap.Add(2803, new UILinkPoint(2803, enabled: true, -3, 2804, 2802, -2));
			uILinkPage8.LinkMap.Add(2804, new UILinkPoint(2804, enabled: true, 2803, -4, 2802, -2));
			uILinkPage8.LinkMap[2800].OnSpecialInteracts += value;
			uILinkPage8.LinkMap[2801].OnSpecialInteracts += value;
			uILinkPage8.LinkMap[2802].OnSpecialInteracts += value;
			uILinkPage8.LinkMap[2803].OnSpecialInteracts += value;
			uILinkPage8.LinkMap[2804].OnSpecialInteracts += value;
			uILinkPage8.UpdateEvent += delegate
			{
				Vector3 value22 = Main.rgbToHsl(Main.selColor);
				float interfaceDeadzoneX = PlayerInput.CurrentProfile.InterfaceDeadzoneX;
				float x = PlayerInput.GamepadThumbstickLeft.X;
				x = ((!(x < 0f - interfaceDeadzoneX) && !(x > interfaceDeadzoneX)) ? 0f : (MathHelper.Lerp(0f, 1f / 120f, (Math.Abs(x) - interfaceDeadzoneX) / (1f - interfaceDeadzoneX)) * (float)Math.Sign(x)));
				int currentPoint = UILinkPointNavigator.CurrentPoint;
				if (currentPoint == 2800)
				{
					Main.hBar = MathHelper.Clamp(Main.hBar + x, 0f, 1f);
				}
				if (currentPoint == 2801)
				{
					Main.sBar = MathHelper.Clamp(Main.sBar + x, 0f, 1f);
				}
				if (currentPoint == 2802)
				{
					Main.lBar = MathHelper.Clamp(Main.lBar + x, 0.15f, 1f);
				}
				Vector3.Clamp(value22, Vector3.Zero, Vector3.One);
				if (x != 0f)
				{
					if (Main.clothesWindow)
					{
						Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar);
						switch (Main.selClothes)
						{
						case 0:
							Main.player[Main.myPlayer].shirtColor = Main.selColor;
							break;
						case 1:
							Main.player[Main.myPlayer].underShirtColor = Main.selColor;
							break;
						case 2:
							Main.player[Main.myPlayer].pantsColor = Main.selColor;
							break;
						case 3:
							Main.player[Main.myPlayer].shoeColor = Main.selColor;
							break;
						}
					}
					SoundEngine.PlaySound(12);
				}
			};
			uILinkPage8.CanEnterEvent += () => Main.clothesWindow;
			uILinkPage8.IsValidEvent += () => Main.clothesWindow;
			uILinkPage8.EnterEvent += delegate
			{
				Main.player[Main.myPlayer].releaseInventory = false;
			};
			uILinkPage8.LeaveEvent += delegate
			{
				Main.player[Main.myPlayer].LockGamepadTileInteractions();
			};
			uILinkPage8.PageOnLeft = 14;
			uILinkPage8.PageOnRight = 14;
			UILinkPointNavigator.RegisterPage(uILinkPage8, 15);
			UILinkPage cp3 = new UILinkPage();
			cp3.UpdateEvent += delegate
			{
				PlayerInput.GamepadAllowScrolling = true;
			};
			for (int num28 = 3000; num28 <= 4999; num28++)
			{
				cp3.LinkMap.Add(num28, new UILinkPoint(num28, enabled: true, -3, -4, -1, -2));
			}
			cp3.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[53].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[82].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + FancyUISpecialInstructions();
			cp3.UpdateEvent += delegate
			{
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Inventory)
				{
					FancyExit();
				}
				UILinkPointNavigator.Shortcuts.BackButtonInUse = false;
			};
			cp3.EnterEvent += delegate
			{
				cp3.CurrentPoint = 3002;
			};
			cp3.CanEnterEvent += () => Main.MenuUI.IsVisible || Main.InGameUI.IsVisible;
			cp3.IsValidEvent += () => Main.MenuUI.IsVisible || Main.InGameUI.IsVisible;
			cp3.OnPageMoveAttempt += OnFancyUIPageMoveAttempt;
			UILinkPointNavigator.RegisterPage(cp3, 1004);
			UILinkPage cp2 = new UILinkPage();
			cp2.UpdateEvent += delegate
			{
				PlayerInput.GamepadAllowScrolling = true;
			};
			for (int num29 = 10000; num29 <= 11000; num29++)
			{
				cp2.LinkMap.Add(num29, new UILinkPoint(num29, enabled: true, -3, -4, -1, -2));
			}
			for (int num30 = 15000; num30 <= 15000; num30++)
			{
				cp2.LinkMap.Add(num30, new UILinkPoint(num30, enabled: true, -3, -4, -1, -2));
			}
			cp2.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[53].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]) + PlayerInput.BuildCommand(Lang.misc[82].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + FancyUISpecialInstructions();
			cp2.UpdateEvent += delegate
			{
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.Inventory)
				{
					FancyExit();
				}
				UILinkPointNavigator.Shortcuts.BackButtonInUse = false;
			};
			cp2.EnterEvent += delegate
			{
				cp2.CurrentPoint = 10000;
			};
			cp2.CanEnterEvent += CanEnterCreativeMenu;
			cp2.IsValidEvent += CanEnterCreativeMenu;
			cp2.OnPageMoveAttempt += OnFancyUIPageMoveAttempt;
			cp2.PageOnLeft = 8;
			cp2.PageOnRight = 0;
			UILinkPointNavigator.RegisterPage(cp2, 1005);
			UILinkPage cp = new UILinkPage();
			cp.OnSpecialInteracts += () => PlayerInput.BuildCommand(Lang.misc[56].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Inventory"]) + PlayerInput.BuildCommand(Lang.misc[64].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"], PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
			Func<string> value21 = () => PlayerInput.BuildCommand(Lang.misc[94].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			for (int num31 = 9000; num31 <= 9050; num31++)
			{
				UILinkPoint uILinkPoint16 = new UILinkPoint(num31, enabled: true, num31 + 10, num31 - 10, num31 - 1, num31 + 1);
				cp.LinkMap.Add(num31, uILinkPoint16);
				uILinkPoint16.OnSpecialInteracts += value21;
			}
			cp.UpdateEvent += delegate
			{
				int num32 = UILinkPointNavigator.Shortcuts.BUFFS_PER_COLUMN;
				if (num32 == 0)
				{
					num32 = 100;
				}
				for (int num33 = 0; num33 < 50; num33++)
				{
					cp.LinkMap[9000 + num33].Up = ((num33 % num32 == 0) ? (-1) : (9000 + num33 - 1));
					if (cp.LinkMap[9000 + num33].Up == -1)
					{
						if (num33 >= num32)
						{
							cp.LinkMap[9000 + num33].Up = 184;
						}
						else
						{
							cp.LinkMap[9000 + num33].Up = 189;
						}
					}
					cp.LinkMap[9000 + num33].Down = (((num33 + 1) % num32 == 0 || num33 == UILinkPointNavigator.Shortcuts.BUFFS_DRAWN - 1) ? 308 : (9000 + num33 + 1));
					cp.LinkMap[9000 + num33].Left = ((num33 < UILinkPointNavigator.Shortcuts.BUFFS_DRAWN - num32) ? (9000 + num33 + num32) : (-3));
					cp.LinkMap[9000 + num33].Right = ((num33 < num32) ? (-4) : (9000 + num33 - num32));
				}
			};
			cp.IsValidEvent += () => Main.playerInventory && Main.EquipPage == 2 && UILinkPointNavigator.Shortcuts.BUFFS_DRAWN > 0;
			cp.PageOnLeft = 8;
			cp.PageOnRight = 8;
			UILinkPointNavigator.RegisterPage(cp, 19);
			UILinkPage uILinkPage9 = UILinkPointNavigator.Pages[UILinkPointNavigator.CurrentPage];
			uILinkPage9.CurrentPoint = uILinkPage9.DefaultPoint;
			uILinkPage9.Enter();
		}

		private static bool CanEnterCreativeMenu()
		{
			if (Main.LocalPlayer.chest != -1)
			{
				return false;
			}
			if (Main.LocalPlayer.talkNPC != -1)
			{
				return false;
			}
			if (Main.playerInventory)
			{
				return Main.CreativeMenu.Enabled;
			}
			return false;
		}

		private static int GetCornerWrapPageIdFromLeftToRight()
		{
			return 8;
		}

		private static int GetCornerWrapPageIdFromRightToLeft()
		{
			if (Main.CreativeMenu.Enabled)
			{
				return 1005;
			}
			return 10;
		}

		private static void OnFancyUIPageMoveAttempt(int direction)
		{
			if (Main.MenuUI.CurrentState is UICharacterCreation uICharacterCreation)
			{
				uICharacterCreation.TryMovingCategory(direction);
			}
			if (UserInterface.ActiveInstance.CurrentState is UIBestiaryTest uIBestiaryTest)
			{
				uIBestiaryTest.TryMovingPages(direction);
			}
		}

		public static void FancyExit()
		{
			switch (UILinkPointNavigator.Shortcuts.BackButtonCommand)
			{
			case 1:
				SoundEngine.PlaySound(11);
				Main.menuMode = 0;
				break;
			case 2:
				SoundEngine.PlaySound(11);
				Main.menuMode = ((!Main.menuMultiplayer) ? 1 : 12);
				break;
			case 3:
				Main.menuMode = 0;
				IngameFancyUI.Close();
				break;
			case 4:
				SoundEngine.PlaySound(11);
				Main.menuMode = 11;
				break;
			case 5:
				SoundEngine.PlaySound(11);
				Main.menuMode = 11;
				break;
			case 6:
				UIVirtualKeyboard.Cancel();
				break;
			case 7:
				if (Main.MenuUI.CurrentState is IHaveBackButtonCommand haveBackButtonCommand)
				{
					haveBackButtonCommand.HandleBackButtonUsage();
				}
				break;
			}
		}

		public static string FancyUISpecialInstructions()
		{
			string text = "";
			int fANCYUI_SPECIAL_INSTRUCTIONS = UILinkPointNavigator.Shortcuts.FANCYUI_SPECIAL_INSTRUCTIONS;
			if (fANCYUI_SPECIAL_INSTRUCTIONS == 1)
			{
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.HotbarMinus)
				{
					UIVirtualKeyboard.CycleSymbols();
					PlayerInput.LockGamepadButtons("HotbarMinus");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				text += PlayerInput.BuildCommand(Lang.menu[235].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarMinus"]);
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.MouseRight)
				{
					UIVirtualKeyboard.BackSpace();
					PlayerInput.LockGamepadButtons("MouseRight");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				text += PlayerInput.BuildCommand(Lang.menu[236].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
				if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
				{
					UIVirtualKeyboard.Write(" ");
					PlayerInput.LockGamepadButtons("SmartCursor");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				text += PlayerInput.BuildCommand(Lang.menu[238].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
				if (UIVirtualKeyboard.CanSubmit)
				{
					if (CanExecuteInputCommand() && PlayerInput.Triggers.JustPressed.HotbarPlus)
					{
						UIVirtualKeyboard.Submit();
						PlayerInput.LockGamepadButtons("HotbarPlus");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
					text += PlayerInput.BuildCommand(Lang.menu[237].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["HotbarPlus"]);
				}
			}
			return text;
		}

		public static void HandleOptionsSpecials()
		{
			switch (UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE)
			{
			case 1:
				Main.bgScroll = (int)HandleSliderHorizontalInput(Main.bgScroll, 0f, 100f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 1f);
				Main.caveParallax = 1f - (float)Main.bgScroll / 500f;
				break;
			case 2:
				Main.musicVolume = HandleSliderHorizontalInput(Main.musicVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				break;
			case 3:
				Main.soundVolume = HandleSliderHorizontalInput(Main.soundVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				break;
			case 4:
				Main.ambientVolume = HandleSliderHorizontalInput(Main.ambientVolume, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				break;
			case 5:
			{
				float hBar = Main.hBar;
				float num3 = (Main.hBar = HandleSliderHorizontalInput(hBar, 0f, 1f));
				if (hBar != num3)
				{
					switch (Main.menuMode)
					{
					case 17:
						Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 18:
						Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 19:
						Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 21:
						Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 22:
						Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 23:
						Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 24:
						Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 25:
						Main.mouseColorSlider.Hue = num3;
						break;
					case 252:
						Main.mouseBorderColorSlider.Hue = num3;
						break;
					}
					SoundEngine.PlaySound(12);
				}
				break;
			}
			case 6:
			{
				float sBar = Main.sBar;
				float num2 = (Main.sBar = HandleSliderHorizontalInput(sBar, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX));
				if (sBar != num2)
				{
					switch (Main.menuMode)
					{
					case 17:
						Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 18:
						Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 19:
						Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 21:
						Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 22:
						Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 23:
						Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 24:
						Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 25:
						Main.mouseColorSlider.Saturation = num2;
						break;
					case 252:
						Main.mouseBorderColorSlider.Saturation = num2;
						break;
					}
					SoundEngine.PlaySound(12);
				}
				break;
			}
			case 7:
			{
				float lBar = Main.lBar;
				float min = 0.15f;
				if (Main.menuMode == 252)
				{
					min = 0f;
				}
				Main.lBar = HandleSliderHorizontalInput(lBar, min, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX);
				float lBar2 = Main.lBar;
				if (lBar != lBar2)
				{
					switch (Main.menuMode)
					{
					case 17:
						Main.player[Main.myPlayer].hairColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 18:
						Main.player[Main.myPlayer].eyeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 19:
						Main.player[Main.myPlayer].skinColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 21:
						Main.player[Main.myPlayer].shirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 22:
						Main.player[Main.myPlayer].underShirtColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 23:
						Main.player[Main.myPlayer].pantsColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 24:
						Main.player[Main.myPlayer].shoeColor = (Main.selColor = Main.hslToRgb(Main.hBar, Main.sBar, Main.lBar));
						break;
					case 25:
						Main.mouseColorSlider.Luminance = lBar2;
						break;
					case 252:
						Main.mouseBorderColorSlider.Luminance = lBar2;
						break;
					}
					SoundEngine.PlaySound(12);
				}
				break;
			}
			case 8:
			{
				float aBar = Main.aBar;
				float num4 = (Main.aBar = HandleSliderHorizontalInput(aBar, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX));
				if (aBar != num4)
				{
					int menuMode = Main.menuMode;
					if (menuMode == 252)
					{
						Main.mouseBorderColorSlider.Alpha = num4;
					}
					SoundEngine.PlaySound(12);
				}
				break;
			}
			case 9:
			{
				bool left = PlayerInput.Triggers.Current.Left;
				bool right = PlayerInput.Triggers.Current.Right;
				if (PlayerInput.Triggers.JustPressed.Left || PlayerInput.Triggers.JustPressed.Right)
				{
					SomeVarsForUILinkers.HairMoveCD = 0;
				}
				else if (SomeVarsForUILinkers.HairMoveCD > 0)
				{
					SomeVarsForUILinkers.HairMoveCD--;
				}
				if (SomeVarsForUILinkers.HairMoveCD == 0 && (left || right))
				{
					if (left)
					{
						Main.PendingPlayer.hair--;
					}
					if (right)
					{
						Main.PendingPlayer.hair++;
					}
					SomeVarsForUILinkers.HairMoveCD = 12;
				}
				int num = 51;
				if (Main.PendingPlayer.hair >= num)
				{
					Main.PendingPlayer.hair = 0;
				}
				if (Main.PendingPlayer.hair < 0)
				{
					Main.PendingPlayer.hair = num - 1;
				}
				break;
			}
			case 10:
				Main.GameZoomTarget = HandleSliderHorizontalInput(Main.GameZoomTarget, 1f, 2f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				break;
			case 11:
				Main.UIScale = HandleSliderHorizontalInput(Main.UIScaleWanted, 0.5f, 2f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
				Main.temporaryGUIScaleSlider = Main.UIScaleWanted;
				break;
			case 12:
				Main.MapScale = HandleSliderHorizontalInput(Main.MapScale, 0.5f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.7f);
				break;
			}
		}
	}
}
