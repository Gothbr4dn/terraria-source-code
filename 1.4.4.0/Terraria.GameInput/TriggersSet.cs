using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Terraria.GameInput
{
	public class TriggersSet
	{
		public Dictionary<string, bool> KeyStatus = new Dictionary<string, bool>();

		public Dictionary<string, InputMode> LatestInputMode = new Dictionary<string, InputMode>();

		public bool UsedMovementKey = true;

		public int HotbarScrollCD;

		public int HotbarHoldTime;

		public bool MouseLeft
		{
			get
			{
				return KeyStatus["MouseLeft"];
			}
			set
			{
				KeyStatus["MouseLeft"] = value;
			}
		}

		public bool MouseRight
		{
			get
			{
				return KeyStatus["MouseRight"];
			}
			set
			{
				KeyStatus["MouseRight"] = value;
			}
		}

		public bool Up
		{
			get
			{
				return KeyStatus["Up"];
			}
			set
			{
				KeyStatus["Up"] = value;
			}
		}

		public bool Down
		{
			get
			{
				return KeyStatus["Down"];
			}
			set
			{
				KeyStatus["Down"] = value;
			}
		}

		public bool Left
		{
			get
			{
				return KeyStatus["Left"];
			}
			set
			{
				KeyStatus["Left"] = value;
			}
		}

		public bool Right
		{
			get
			{
				return KeyStatus["Right"];
			}
			set
			{
				KeyStatus["Right"] = value;
			}
		}

		public bool Jump
		{
			get
			{
				return KeyStatus["Jump"];
			}
			set
			{
				KeyStatus["Jump"] = value;
			}
		}

		public bool Throw
		{
			get
			{
				return KeyStatus["Throw"];
			}
			set
			{
				KeyStatus["Throw"] = value;
			}
		}

		public bool Inventory
		{
			get
			{
				return KeyStatus["Inventory"];
			}
			set
			{
				KeyStatus["Inventory"] = value;
			}
		}

		public bool Grapple
		{
			get
			{
				return KeyStatus["Grapple"];
			}
			set
			{
				KeyStatus["Grapple"] = value;
			}
		}

		public bool SmartSelect
		{
			get
			{
				return KeyStatus["SmartSelect"];
			}
			set
			{
				KeyStatus["SmartSelect"] = value;
			}
		}

		public bool SmartCursor
		{
			get
			{
				return KeyStatus["SmartCursor"];
			}
			set
			{
				KeyStatus["SmartCursor"] = value;
			}
		}

		public bool QuickMount
		{
			get
			{
				return KeyStatus["QuickMount"];
			}
			set
			{
				KeyStatus["QuickMount"] = value;
			}
		}

		public bool QuickHeal
		{
			get
			{
				return KeyStatus["QuickHeal"];
			}
			set
			{
				KeyStatus["QuickHeal"] = value;
			}
		}

		public bool QuickMana
		{
			get
			{
				return KeyStatus["QuickMana"];
			}
			set
			{
				KeyStatus["QuickMana"] = value;
			}
		}

		public bool QuickBuff
		{
			get
			{
				return KeyStatus["QuickBuff"];
			}
			set
			{
				KeyStatus["QuickBuff"] = value;
			}
		}

		public bool Loadout1
		{
			get
			{
				return KeyStatus["Loadout1"];
			}
			set
			{
				KeyStatus["Loadout1"] = value;
			}
		}

		public bool Loadout2
		{
			get
			{
				return KeyStatus["Loadout2"];
			}
			set
			{
				KeyStatus["Loadout2"] = value;
			}
		}

		public bool Loadout3
		{
			get
			{
				return KeyStatus["Loadout3"];
			}
			set
			{
				KeyStatus["Loadout3"] = value;
			}
		}

		public bool MapZoomIn
		{
			get
			{
				return KeyStatus["MapZoomIn"];
			}
			set
			{
				KeyStatus["MapZoomIn"] = value;
			}
		}

		public bool MapZoomOut
		{
			get
			{
				return KeyStatus["MapZoomOut"];
			}
			set
			{
				KeyStatus["MapZoomOut"] = value;
			}
		}

		public bool MapAlphaUp
		{
			get
			{
				return KeyStatus["MapAlphaUp"];
			}
			set
			{
				KeyStatus["MapAlphaUp"] = value;
			}
		}

		public bool MapAlphaDown
		{
			get
			{
				return KeyStatus["MapAlphaDown"];
			}
			set
			{
				KeyStatus["MapAlphaDown"] = value;
			}
		}

		public bool MapFull
		{
			get
			{
				return KeyStatus["MapFull"];
			}
			set
			{
				KeyStatus["MapFull"] = value;
			}
		}

		public bool MapStyle
		{
			get
			{
				return KeyStatus["MapStyle"];
			}
			set
			{
				KeyStatus["MapStyle"] = value;
			}
		}

		public bool Hotbar1
		{
			get
			{
				return KeyStatus["Hotbar1"];
			}
			set
			{
				KeyStatus["Hotbar1"] = value;
			}
		}

		public bool Hotbar2
		{
			get
			{
				return KeyStatus["Hotbar2"];
			}
			set
			{
				KeyStatus["Hotbar2"] = value;
			}
		}

		public bool Hotbar3
		{
			get
			{
				return KeyStatus["Hotbar3"];
			}
			set
			{
				KeyStatus["Hotbar3"] = value;
			}
		}

		public bool Hotbar4
		{
			get
			{
				return KeyStatus["Hotbar4"];
			}
			set
			{
				KeyStatus["Hotbar4"] = value;
			}
		}

		public bool Hotbar5
		{
			get
			{
				return KeyStatus["Hotbar5"];
			}
			set
			{
				KeyStatus["Hotbar5"] = value;
			}
		}

		public bool Hotbar6
		{
			get
			{
				return KeyStatus["Hotbar6"];
			}
			set
			{
				KeyStatus["Hotbar6"] = value;
			}
		}

		public bool Hotbar7
		{
			get
			{
				return KeyStatus["Hotbar7"];
			}
			set
			{
				KeyStatus["Hotbar7"] = value;
			}
		}

		public bool Hotbar8
		{
			get
			{
				return KeyStatus["Hotbar8"];
			}
			set
			{
				KeyStatus["Hotbar8"] = value;
			}
		}

		public bool Hotbar9
		{
			get
			{
				return KeyStatus["Hotbar9"];
			}
			set
			{
				KeyStatus["Hotbar9"] = value;
			}
		}

		public bool Hotbar10
		{
			get
			{
				return KeyStatus["Hotbar10"];
			}
			set
			{
				KeyStatus["Hotbar10"] = value;
			}
		}

		public bool HotbarMinus
		{
			get
			{
				return KeyStatus["HotbarMinus"];
			}
			set
			{
				KeyStatus["HotbarMinus"] = value;
			}
		}

		public bool HotbarPlus
		{
			get
			{
				return KeyStatus["HotbarPlus"];
			}
			set
			{
				KeyStatus["HotbarPlus"] = value;
			}
		}

		public bool DpadRadial1
		{
			get
			{
				return KeyStatus["DpadRadial1"];
			}
			set
			{
				KeyStatus["DpadRadial1"] = value;
			}
		}

		public bool DpadRadial2
		{
			get
			{
				return KeyStatus["DpadRadial2"];
			}
			set
			{
				KeyStatus["DpadRadial2"] = value;
			}
		}

		public bool DpadRadial3
		{
			get
			{
				return KeyStatus["DpadRadial3"];
			}
			set
			{
				KeyStatus["DpadRadial3"] = value;
			}
		}

		public bool DpadRadial4
		{
			get
			{
				return KeyStatus["DpadRadial4"];
			}
			set
			{
				KeyStatus["DpadRadial4"] = value;
			}
		}

		public bool RadialHotbar
		{
			get
			{
				return KeyStatus["RadialHotbar"];
			}
			set
			{
				KeyStatus["RadialHotbar"] = value;
			}
		}

		public bool RadialQuickbar
		{
			get
			{
				return KeyStatus["RadialQuickbar"];
			}
			set
			{
				KeyStatus["RadialQuickbar"] = value;
			}
		}

		public bool DpadMouseSnap1
		{
			get
			{
				return KeyStatus["DpadSnap1"];
			}
			set
			{
				KeyStatus["DpadSnap1"] = value;
			}
		}

		public bool DpadMouseSnap2
		{
			get
			{
				return KeyStatus["DpadSnap2"];
			}
			set
			{
				KeyStatus["DpadSnap2"] = value;
			}
		}

		public bool DpadMouseSnap3
		{
			get
			{
				return KeyStatus["DpadSnap3"];
			}
			set
			{
				KeyStatus["DpadSnap3"] = value;
			}
		}

		public bool DpadMouseSnap4
		{
			get
			{
				return KeyStatus["DpadSnap4"];
			}
			set
			{
				KeyStatus["DpadSnap4"] = value;
			}
		}

		public bool MenuUp
		{
			get
			{
				return KeyStatus["MenuUp"];
			}
			set
			{
				KeyStatus["MenuUp"] = value;
			}
		}

		public bool MenuDown
		{
			get
			{
				return KeyStatus["MenuDown"];
			}
			set
			{
				KeyStatus["MenuDown"] = value;
			}
		}

		public bool MenuLeft
		{
			get
			{
				return KeyStatus["MenuLeft"];
			}
			set
			{
				KeyStatus["MenuLeft"] = value;
			}
		}

		public bool MenuRight
		{
			get
			{
				return KeyStatus["MenuRight"];
			}
			set
			{
				KeyStatus["MenuRight"] = value;
			}
		}

		public bool LockOn
		{
			get
			{
				return KeyStatus["LockOn"];
			}
			set
			{
				KeyStatus["LockOn"] = value;
			}
		}

		public bool ViewZoomIn
		{
			get
			{
				return KeyStatus["ViewZoomIn"];
			}
			set
			{
				KeyStatus["ViewZoomIn"] = value;
			}
		}

		public bool ViewZoomOut
		{
			get
			{
				return KeyStatus["ViewZoomOut"];
			}
			set
			{
				KeyStatus["ViewZoomOut"] = value;
			}
		}

		public bool OpenCreativePowersMenu
		{
			get
			{
				return KeyStatus["ToggleCreativeMenu"];
			}
			set
			{
				KeyStatus["ToggleCreativeMenu"] = value;
			}
		}

		public bool ToggleCameraMode
		{
			get
			{
				return KeyStatus["ToggleCameraMode"];
			}
			set
			{
				KeyStatus["ToggleCameraMode"] = value;
			}
		}

		public Vector2 DirectionsRaw => new Vector2(Right.ToInt() - Left.ToInt(), Down.ToInt() - Up.ToInt());

		public void Reset()
		{
			string[] array = KeyStatus.Keys.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				KeyStatus[array[i]] = false;
			}
		}

		public void CloneFrom(TriggersSet other)
		{
			KeyStatus.Clear();
			LatestInputMode.Clear();
			foreach (KeyValuePair<string, bool> item in other.KeyStatus)
			{
				KeyStatus.Add(item.Key, item.Value);
			}
			UsedMovementKey = other.UsedMovementKey;
			HotbarScrollCD = other.HotbarScrollCD;
			HotbarHoldTime = other.HotbarHoldTime;
		}

		public void SetupKeys()
		{
			KeyStatus.Clear();
			foreach (string knownTrigger in PlayerInput.KnownTriggers)
			{
				KeyStatus.Add(knownTrigger, value: false);
			}
		}

		public Vector2 GetNavigatorDirections()
		{
			bool flag = Main.gameMenu || Main.ingameOptionsWindow || Main.editChest || Main.editSign || ((Main.playerInventory || Main.LocalPlayer.talkNPC != -1) && PlayerInput.CurrentProfile.UsingDpadMovekeys());
			bool value = Up || (flag && MenuUp);
			bool value2 = Right || (flag && MenuRight);
			bool value3 = Down || (flag && MenuDown);
			bool value4 = Left || (flag && MenuLeft);
			return new Vector2(value2.ToInt() - value4.ToInt(), value3.ToInt() - value.ToInt());
		}

		public void CopyInto(Player p)
		{
			if (PlayerInput.CurrentInputMode != InputMode.XBoxGamepadUI && !PlayerInput.CursorIsBusy)
			{
				p.controlUp = Up;
				p.controlDown = Down;
				p.controlLeft = Left;
				p.controlRight = Right;
				p.controlJump = Jump;
				p.controlHook = Grapple;
				p.controlTorch = SmartSelect;
				p.controlSmart = SmartCursor;
				p.controlMount = QuickMount;
				p.controlQuickHeal = QuickHeal;
				p.controlQuickMana = QuickMana;
				p.controlCreativeMenu = OpenCreativePowersMenu;
				if (QuickBuff)
				{
					p.QuickBuff();
				}
				if (Loadout1)
				{
					p.TrySwitchingLoadout(0);
				}
				if (Loadout2)
				{
					p.TrySwitchingLoadout(1);
				}
				if (Loadout3)
				{
					p.TrySwitchingLoadout(2);
				}
			}
			p.controlInv = Inventory;
			p.controlThrow = Throw;
			p.mapZoomIn = MapZoomIn;
			p.mapZoomOut = MapZoomOut;
			p.mapAlphaUp = MapAlphaUp;
			p.mapAlphaDown = MapAlphaDown;
			p.mapFullScreen = MapFull;
			p.mapStyle = MapStyle;
			if (MouseLeft)
			{
				if (!Main.blockMouse && !p.mouseInterface)
				{
					p.controlUseItem = true;
				}
			}
			else
			{
				Main.blockMouse = false;
			}
			if (!MouseRight && !Main.playerInventory)
			{
				PlayerInput.LockGamepadTileUseButton = false;
			}
			if (MouseRight && !p.mouseInterface && !Main.blockMouse && !ShouldLockTileUsage() && !PlayerInput.InBuildingMode)
			{
				p.controlUseTile = true;
			}
			if (PlayerInput.InBuildingMode && MouseRight)
			{
				p.controlInv = true;
			}
			if (SmartSelect && LatestInputMode.TryGetValue("SmartSelect", out var value) && IsInputFromGamepad(value))
			{
				PlayerInput.SettingsForUI.SetCursorMode(CursorMode.Gamepad);
			}
			bool flag = PlayerInput.Triggers.Current.HotbarPlus || PlayerInput.Triggers.Current.HotbarMinus;
			if (flag)
			{
				HotbarHoldTime++;
			}
			else
			{
				HotbarHoldTime = 0;
			}
			if (HotbarScrollCD > 0 && (!(HotbarScrollCD == 1 && flag) || PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired <= 0))
			{
				HotbarScrollCD--;
			}
		}

		public void CopyIntoDuringChat(Player p)
		{
			if (MouseLeft)
			{
				if (!Main.blockMouse && !p.mouseInterface)
				{
					p.controlUseItem = true;
				}
			}
			else
			{
				Main.blockMouse = false;
			}
			if (!MouseRight && !Main.playerInventory)
			{
				PlayerInput.LockGamepadTileUseButton = false;
			}
			if (MouseRight && !p.mouseInterface && !Main.blockMouse && !ShouldLockTileUsage() && !PlayerInput.InBuildingMode)
			{
				p.controlUseTile = true;
			}
			bool flag = PlayerInput.Triggers.Current.HotbarPlus || PlayerInput.Triggers.Current.HotbarMinus;
			if (flag)
			{
				HotbarHoldTime++;
			}
			else
			{
				HotbarHoldTime = 0;
			}
			if (HotbarScrollCD > 0 && (!(HotbarScrollCD == 1 && flag) || PlayerInput.CurrentProfile.HotbarRadialHoldTimeRequired <= 0))
			{
				HotbarScrollCD--;
			}
		}

		private bool ShouldLockTileUsage()
		{
			if (PlayerInput.LockGamepadTileUseButton)
			{
				return PlayerInput.UsingGamepad;
			}
			return false;
		}

		private bool IsInputFromGamepad(InputMode mode)
		{
			if ((uint)mode <= 2u)
			{
				return false;
			}
			return true;
		}
	}
}
