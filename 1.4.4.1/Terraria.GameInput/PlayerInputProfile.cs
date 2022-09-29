using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Terraria.GameInput
{
	public class PlayerInputProfile
	{
		public Dictionary<InputMode, KeyConfiguration> InputModes = new Dictionary<InputMode, KeyConfiguration>
		{
			{
				InputMode.Keyboard,
				new KeyConfiguration()
			},
			{
				InputMode.KeyboardUI,
				new KeyConfiguration()
			},
			{
				InputMode.XBoxGamepad,
				new KeyConfiguration()
			},
			{
				InputMode.XBoxGamepadUI,
				new KeyConfiguration()
			}
		};

		public string Name = "";

		public bool AllowEditting = true;

		public int HotbarRadialHoldTimeRequired = 16;

		public float TriggersDeadzone = 0.3f;

		public float InterfaceDeadzoneX = 0.2f;

		public float LeftThumbstickDeadzoneX = 0.25f;

		public float LeftThumbstickDeadzoneY = 0.4f;

		public float RightThumbstickDeadzoneX;

		public float RightThumbstickDeadzoneY;

		public bool LeftThumbstickInvertX;

		public bool LeftThumbstickInvertY;

		public bool RightThumbstickInvertX;

		public bool RightThumbstickInvertY;

		public int InventoryMoveCD = 6;

		public string ShowName => Name;

		public bool HotbarAllowsRadial => HotbarRadialHoldTimeRequired != -1;

		public PlayerInputProfile(string name)
		{
			Name = name;
		}

		public void Initialize(PresetProfiles style)
		{
			foreach (KeyValuePair<InputMode, KeyConfiguration> inputMode in InputModes)
			{
				inputMode.Value.SetupKeys();
				PlayerInput.Reset(inputMode.Value, style, inputMode.Key);
			}
		}

		public bool Load(Dictionary<string, object> dict)
		{
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Expected O, but got Unknown
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Expected O, but got Unknown
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Expected O, but got Unknown
			int num = 0;
			if (dict.TryGetValue("Last Launched Version", out var value))
			{
				num = (int)(long)value;
			}
			if (dict.TryGetValue("Mouse And Keyboard", out value))
			{
				InputModes[InputMode.Keyboard].ReadPreferences(JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(((object)(JObject)value).ToString()));
			}
			if (dict.TryGetValue("Gamepad", out value))
			{
				InputModes[InputMode.XBoxGamepad].ReadPreferences(JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(((object)(JObject)value).ToString()));
			}
			if (dict.TryGetValue("Mouse And Keyboard UI", out value))
			{
				InputModes[InputMode.KeyboardUI].ReadPreferences(JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(((object)(JObject)value).ToString()));
			}
			if (dict.TryGetValue("Gamepad UI", out value))
			{
				InputModes[InputMode.XBoxGamepadUI].ReadPreferences(JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(((object)(JObject)value).ToString()));
			}
			if (num < 190)
			{
				InputModes[InputMode.Keyboard].KeyStatus["ViewZoomIn"] = new List<string>();
				InputModes[InputMode.Keyboard].KeyStatus["ViewZoomIn"].AddRange(PlayerInput.OriginalProfiles["Redigit's Pick"].InputModes[InputMode.Keyboard].KeyStatus["ViewZoomIn"]);
				InputModes[InputMode.Keyboard].KeyStatus["ViewZoomOut"] = new List<string>();
				InputModes[InputMode.Keyboard].KeyStatus["ViewZoomOut"].AddRange(PlayerInput.OriginalProfiles["Redigit's Pick"].InputModes[InputMode.Keyboard].KeyStatus["ViewZoomOut"]);
			}
			if (num < 218)
			{
				InputModes[InputMode.Keyboard].KeyStatus["ToggleCreativeMenu"] = new List<string>();
				InputModes[InputMode.Keyboard].KeyStatus["ToggleCreativeMenu"].AddRange(PlayerInput.OriginalProfiles["Redigit's Pick"].InputModes[InputMode.Keyboard].KeyStatus["ToggleCreativeMenu"]);
			}
			if (num < 227)
			{
				List<string> list = InputModes[InputMode.KeyboardUI].KeyStatus["MouseLeft"];
				string item = "Mouse1";
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			if (num < 265)
			{
				string[] array = new string[4] { "Loadout1", "Loadout2", "Loadout3", "ToggleCameraMode" };
				foreach (string key in array)
				{
					InputModes[InputMode.Keyboard].KeyStatus[key] = new List<string>(PlayerInput.OriginalProfiles["Redigit's Pick"].InputModes[InputMode.Keyboard].KeyStatus[key]);
				}
			}
			if (dict.TryGetValue("Settings", out value))
			{
				Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(((object)(JObject)value).ToString());
				if (dictionary.TryGetValue("Edittable", out value))
				{
					AllowEditting = (bool)value;
				}
				if (dictionary.TryGetValue("Gamepad - HotbarRadialHoldTime", out value))
				{
					HotbarRadialHoldTimeRequired = (int)(long)value;
				}
				if (dictionary.TryGetValue("Gamepad - LeftThumbstickDeadzoneX", out value))
				{
					LeftThumbstickDeadzoneX = (float)(double)value;
				}
				if (dictionary.TryGetValue("Gamepad - LeftThumbstickDeadzoneY", out value))
				{
					LeftThumbstickDeadzoneY = (float)(double)value;
				}
				if (dictionary.TryGetValue("Gamepad - RightThumbstickDeadzoneX", out value))
				{
					RightThumbstickDeadzoneX = (float)(double)value;
				}
				if (dictionary.TryGetValue("Gamepad - RightThumbstickDeadzoneY", out value))
				{
					RightThumbstickDeadzoneY = (float)(double)value;
				}
				if (dictionary.TryGetValue("Gamepad - LeftThumbstickInvertX", out value))
				{
					LeftThumbstickInvertX = (bool)value;
				}
				if (dictionary.TryGetValue("Gamepad - LeftThumbstickInvertY", out value))
				{
					LeftThumbstickInvertY = (bool)value;
				}
				if (dictionary.TryGetValue("Gamepad - RightThumbstickInvertX", out value))
				{
					RightThumbstickInvertX = (bool)value;
				}
				if (dictionary.TryGetValue("Gamepad - RightThumbstickInvertY", out value))
				{
					RightThumbstickInvertY = (bool)value;
				}
				if (dictionary.TryGetValue("Gamepad - TriggersDeadzone", out value))
				{
					TriggersDeadzone = (float)(double)value;
				}
				if (dictionary.TryGetValue("Gamepad - InterfaceDeadzoneX", out value))
				{
					InterfaceDeadzoneX = (float)(double)value;
				}
				if (dictionary.TryGetValue("Gamepad - InventoryMoveCD", out value))
				{
					InventoryMoveCD = (int)(long)value;
				}
			}
			return true;
		}

		public Dictionary<string, object> Save()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary.Add("Last Launched Version", 270);
			dictionary2.Add("Edittable", AllowEditting);
			dictionary2.Add("Gamepad - HotbarRadialHoldTime", HotbarRadialHoldTimeRequired);
			dictionary2.Add("Gamepad - LeftThumbstickDeadzoneX", LeftThumbstickDeadzoneX);
			dictionary2.Add("Gamepad - LeftThumbstickDeadzoneY", LeftThumbstickDeadzoneY);
			dictionary2.Add("Gamepad - RightThumbstickDeadzoneX", RightThumbstickDeadzoneX);
			dictionary2.Add("Gamepad - RightThumbstickDeadzoneY", RightThumbstickDeadzoneY);
			dictionary2.Add("Gamepad - LeftThumbstickInvertX", LeftThumbstickInvertX);
			dictionary2.Add("Gamepad - LeftThumbstickInvertY", LeftThumbstickInvertY);
			dictionary2.Add("Gamepad - RightThumbstickInvertX", RightThumbstickInvertX);
			dictionary2.Add("Gamepad - RightThumbstickInvertY", RightThumbstickInvertY);
			dictionary2.Add("Gamepad - TriggersDeadzone", TriggersDeadzone);
			dictionary2.Add("Gamepad - InterfaceDeadzoneX", InterfaceDeadzoneX);
			dictionary2.Add("Gamepad - InventoryMoveCD", InventoryMoveCD);
			dictionary.Add("Settings", dictionary2);
			dictionary.Add("Mouse And Keyboard", InputModes[InputMode.Keyboard].WritePreferences());
			dictionary.Add("Gamepad", InputModes[InputMode.XBoxGamepad].WritePreferences());
			dictionary.Add("Mouse And Keyboard UI", InputModes[InputMode.KeyboardUI].WritePreferences());
			dictionary.Add("Gamepad UI", InputModes[InputMode.XBoxGamepadUI].WritePreferences());
			return dictionary;
		}

		public void ConditionalAddProfile(Dictionary<string, object> dicttouse, string k, InputMode nm, Dictionary<string, List<string>> dict)
		{
			if (PlayerInput.OriginalProfiles.ContainsKey(Name))
			{
				foreach (KeyValuePair<string, List<string>> item in PlayerInput.OriginalProfiles[Name].InputModes[nm].WritePreferences())
				{
					bool flag = true;
					if (dict.TryGetValue(item.Key, out var value))
					{
						if (value.Count != item.Value.Count)
						{
							flag = false;
						}
						if (!flag)
						{
							for (int i = 0; i < value.Count; i++)
							{
								if (value[i] != item.Value[i])
								{
									flag = false;
									break;
								}
							}
						}
					}
					else
					{
						flag = false;
					}
					if (flag)
					{
						dict.Remove(item.Key);
					}
				}
			}
			if (dict.Count > 0)
			{
				dicttouse.Add(k, dict);
			}
		}

		public void ConditionalAdd(Dictionary<string, object> dicttouse, string a, object b, Func<PlayerInputProfile, bool> check)
		{
			if (!PlayerInput.OriginalProfiles.ContainsKey(Name) || !check(PlayerInput.OriginalProfiles[Name]))
			{
				dicttouse.Add(a, b);
			}
		}

		public void CopyGameplaySettingsFrom(PlayerInputProfile profile, InputMode mode)
		{
			string[] keysToCopy = new string[23]
			{
				"MouseLeft", "MouseRight", "Up", "Down", "Left", "Right", "Jump", "Grapple", "SmartSelect", "SmartCursor",
				"QuickMount", "QuickHeal", "QuickMana", "QuickBuff", "Throw", "Inventory", "ViewZoomIn", "ViewZoomOut", "Loadout1", "Loadout2",
				"Loadout3", "ToggleCreativeMenu", "ToggleCameraMode"
			};
			CopyKeysFrom(profile, mode, keysToCopy);
		}

		public void CopyHotbarSettingsFrom(PlayerInputProfile profile, InputMode mode)
		{
			string[] keysToCopy = new string[12]
			{
				"HotbarMinus", "HotbarPlus", "Hotbar1", "Hotbar2", "Hotbar3", "Hotbar4", "Hotbar5", "Hotbar6", "Hotbar7", "Hotbar8",
				"Hotbar9", "Hotbar10"
			};
			CopyKeysFrom(profile, mode, keysToCopy);
		}

		public void CopyMapSettingsFrom(PlayerInputProfile profile, InputMode mode)
		{
			string[] keysToCopy = new string[6] { "MapZoomIn", "MapZoomOut", "MapAlphaUp", "MapAlphaDown", "MapFull", "MapStyle" };
			CopyKeysFrom(profile, mode, keysToCopy);
		}

		public void CopyGamepadSettingsFrom(PlayerInputProfile profile, InputMode mode)
		{
			string[] keysToCopy = new string[10] { "RadialHotbar", "RadialQuickbar", "DpadSnap1", "DpadSnap2", "DpadSnap3", "DpadSnap4", "DpadRadial1", "DpadRadial2", "DpadRadial3", "DpadRadial4" };
			CopyKeysFrom(profile, InputMode.XBoxGamepad, keysToCopy);
			CopyKeysFrom(profile, InputMode.XBoxGamepadUI, keysToCopy);
		}

		public void CopyGamepadAdvancedSettingsFrom(PlayerInputProfile profile, InputMode mode)
		{
			TriggersDeadzone = profile.TriggersDeadzone;
			InterfaceDeadzoneX = profile.InterfaceDeadzoneX;
			LeftThumbstickDeadzoneX = profile.LeftThumbstickDeadzoneX;
			LeftThumbstickDeadzoneY = profile.LeftThumbstickDeadzoneY;
			RightThumbstickDeadzoneX = profile.RightThumbstickDeadzoneX;
			RightThumbstickDeadzoneY = profile.RightThumbstickDeadzoneY;
			LeftThumbstickInvertX = profile.LeftThumbstickInvertX;
			LeftThumbstickInvertY = profile.LeftThumbstickInvertY;
			RightThumbstickInvertX = profile.RightThumbstickInvertX;
			RightThumbstickInvertY = profile.RightThumbstickInvertY;
			InventoryMoveCD = profile.InventoryMoveCD;
		}

		private void CopyKeysFrom(PlayerInputProfile profile, InputMode mode, string[] keysToCopy)
		{
			for (int i = 0; i < keysToCopy.Length; i++)
			{
				if (profile.InputModes[mode].KeyStatus.TryGetValue(keysToCopy[i], out var value))
				{
					InputModes[mode].KeyStatus[keysToCopy[i]].Clear();
					InputModes[mode].KeyStatus[keysToCopy[i]].AddRange(value);
				}
			}
		}

		public bool UsingDpadHotbar()
		{
			if (InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial1"].Contains(Buttons.DPadUp.ToString()) && InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial2"].Contains(Buttons.DPadRight.ToString()) && InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial3"].Contains(Buttons.DPadDown.ToString()) && InputModes[InputMode.XBoxGamepad].KeyStatus["DpadRadial4"].Contains(Buttons.DPadLeft.ToString()) && InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial1"].Contains(Buttons.DPadUp.ToString()) && InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial2"].Contains(Buttons.DPadRight.ToString()) && InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial3"].Contains(Buttons.DPadDown.ToString()))
			{
				return InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadRadial4"].Contains(Buttons.DPadLeft.ToString());
			}
			return false;
		}

		public bool UsingDpadMovekeys()
		{
			if (InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap1"].Contains(Buttons.DPadUp.ToString()) && InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap2"].Contains(Buttons.DPadRight.ToString()) && InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap3"].Contains(Buttons.DPadDown.ToString()) && InputModes[InputMode.XBoxGamepad].KeyStatus["DpadSnap4"].Contains(Buttons.DPadLeft.ToString()) && InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap1"].Contains(Buttons.DPadUp.ToString()) && InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap2"].Contains(Buttons.DPadRight.ToString()) && InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap3"].Contains(Buttons.DPadDown.ToString()))
			{
				return InputModes[InputMode.XBoxGamepadUI].KeyStatus["DpadSnap4"].Contains(Buttons.DPadLeft.ToString());
			}
			return false;
		}
	}
}
