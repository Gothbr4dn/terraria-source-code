using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace Terraria.UI.Gamepad
{
	public class GamepadMainMenuHandler
	{
		public static int LastMainMenu = -1;

		public static List<Vector2> MenuItemPositions = new List<Vector2>(20);

		public static int LastDrew = -1;

		public static bool CanRun = false;

		public static bool MoveCursorOnNextRun = false;

		public static void Update()
		{
			if (!CanRun)
			{
				UILinkPage uILinkPage = UILinkPointNavigator.Pages[1000];
				uILinkPage.CurrentPoint = uILinkPage.DefaultPoint;
				Vector2 vector = new Vector2((float)Math.Cos(Main.GlobalTimeWrappedHourly * (MathF.PI * 2f)), (float)Math.Sin(Main.GlobalTimeWrappedHourly * (MathF.PI * 2f) * 2f)) * new Vector2(30f, 15f) + Vector2.UnitY * 20f;
				UILinkPointNavigator.SetPosition(2000, new Vector2(Main.screenWidth, Main.screenHeight) / 2f + vector);
			}
			else
			{
				if (!Main.gameMenu || Main.MenuUI.IsVisible || LastDrew != Main.menuMode)
				{
					return;
				}
				int lastMainMenu = LastMainMenu;
				LastMainMenu = Main.menuMode;
				switch (Main.menuMode)
				{
				case 17:
				case 18:
				case 19:
				case 21:
				case 22:
				case 23:
				case 24:
				case 26:
					if (MenuItemPositions.Count >= 4)
					{
						Vector2 item = MenuItemPositions[3];
						MenuItemPositions.RemoveAt(3);
						if (Main.menuMode == 17)
						{
							MenuItemPositions.Insert(0, item);
						}
					}
					break;
				case 28:
					if (MenuItemPositions.Count >= 3)
					{
						MenuItemPositions.RemoveAt(1);
					}
					break;
				}
				UILinkPage uILinkPage2 = UILinkPointNavigator.Pages[1000];
				if (lastMainMenu != Main.menuMode)
				{
					uILinkPage2.CurrentPoint = uILinkPage2.DefaultPoint;
				}
				for (int i = 0; i < MenuItemPositions.Count; i++)
				{
					Vector2 position = MenuItemPositions[i] * Main.UIScale;
					if (i == 0 && lastMainMenu != LastMainMenu && PlayerInput.UsingGamepad && Main.InvisibleCursorForGamepad)
					{
						Main.mouseX = (PlayerInput.MouseX = (int)position.X);
						Main.mouseY = (PlayerInput.MouseY = (int)position.Y);
						Main.menuFocus = -1;
					}
					UILinkPoint uILinkPoint = uILinkPage2.LinkMap[2000 + i];
					uILinkPoint.Position = position;
					if (i == 0)
					{
						uILinkPoint.Up = -1;
					}
					else
					{
						uILinkPoint.Up = 2000 + i - 1;
					}
					uILinkPoint.Left = -3;
					uILinkPoint.Right = -4;
					if (i == MenuItemPositions.Count - 1)
					{
						uILinkPoint.Down = -2;
					}
					else
					{
						uILinkPoint.Down = 2000 + i + 1;
					}
					if (MoveCursorOnNextRun)
					{
						MoveCursorOnNextRun = false;
						UILinkPointNavigator.ChangePoint(uILinkPoint.ID);
					}
				}
				MenuItemPositions.Clear();
			}
		}
	}
}
