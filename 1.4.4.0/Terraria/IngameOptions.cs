using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria
{
	public static class IngameOptions
	{
		public const int width = 670;

		public const int height = 480;

		public static float[] leftScale = new float[10] { 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f };

		public static float[] rightScale = new float[17]
		{
			0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f,
			0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f, 0.7f
		};

		private static Dictionary<int, int> _leftSideCategoryMapping = new Dictionary<int, int>
		{
			{ 0, 0 },
			{ 1, 1 },
			{ 2, 2 },
			{ 3, 3 }
		};

		public static bool[] skipRightSlot = new bool[20];

		public static int leftHover = -1;

		public static int rightHover = -1;

		public static int oldLeftHover = -1;

		public static int oldRightHover = -1;

		public static int rightLock = -1;

		public static bool inBar;

		public static bool notBar;

		public static bool noSound;

		private static Rectangle _GUIHover;

		public static int category;

		public static Vector2 valuePosition = Vector2.Zero;

		private static string _mouseOverText;

		private static bool _canConsumeHover;

		public static void Open()
		{
			Main.ClosePlayerChat();
			Main.chatText = "";
			Main.playerInventory = false;
			Main.editChest = false;
			Main.npcChatText = "";
			SoundEngine.PlaySound(10);
			Main.ingameOptionsWindow = true;
			category = 0;
			for (int i = 0; i < leftScale.Length; i++)
			{
				leftScale[i] = 0f;
			}
			for (int j = 0; j < rightScale.Length; j++)
			{
				rightScale[j] = 0f;
			}
			leftHover = -1;
			rightHover = -1;
			oldLeftHover = -1;
			oldRightHover = -1;
			rightLock = -1;
			inBar = false;
			notBar = false;
			noSound = false;
		}

		public static void Close()
		{
			if (Main.setKey == -1)
			{
				Main.ingameOptionsWindow = false;
				SoundEngine.PlaySound(11);
				Recipe.FindRecipes();
				Main.playerInventory = true;
				Main.SaveSettings();
			}
		}

		public static void Draw(Main mainInstance, SpriteBatch sb)
		{
			_canConsumeHover = true;
			for (int i = 0; i < skipRightSlot.Length; i++)
			{
				skipRightSlot[i] = false;
			}
			bool flag = GameCulture.FromCultureName(GameCulture.CultureName.Russian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Portuguese).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.French).IsActive;
			bool isActive = GameCulture.FromCultureName(GameCulture.CultureName.Polish).IsActive;
			bool isActive2 = GameCulture.FromCultureName(GameCulture.CultureName.German).IsActive;
			bool flag2 = GameCulture.FromCultureName(GameCulture.CultureName.Italian).IsActive || GameCulture.FromCultureName(GameCulture.CultureName.Spanish).IsActive;
			bool flag3 = false;
			int num = 70;
			float scale = 0.75f;
			float num2 = 60f;
			float num3 = 300f;
			if (flag)
			{
				flag3 = true;
			}
			if (isActive)
			{
				num3 = 200f;
			}
			new Vector2(Main.mouseX, Main.mouseY);
			bool flag4 = Main.mouseLeft && Main.mouseLeftRelease;
			Vector2 vector = new Vector2(Main.screenWidth, Main.screenHeight);
			Vector2 vector2 = new Vector2(670f, 480f);
			Vector2 vector3 = vector / 2f - vector2 / 2f;
			int num4 = 20;
			_GUIHover = new Rectangle((int)(vector3.X - (float)num4), (int)(vector3.Y - (float)num4), (int)(vector2.X + (float)(num4 * 2)), (int)(vector2.Y + (float)(num4 * 2)));
			Utils.DrawInvBG(sb, vector3.X - (float)num4, vector3.Y - (float)num4, vector2.X + (float)(num4 * 2), vector2.Y + (float)(num4 * 2), new Color(33, 15, 91, 255) * 0.685f);
			if (new Rectangle((int)vector3.X - num4, (int)vector3.Y - num4, (int)vector2.X + num4 * 2, (int)vector2.Y + num4 * 2).Contains(new Point(Main.mouseX, Main.mouseY)))
			{
				Main.player[Main.myPlayer].mouseInterface = true;
			}
			Utils.DrawBorderString(sb, Language.GetTextValue("GameUI.SettingsMenu"), vector3 + vector2 * new Vector2(0.5f, 0f), Color.White, 1f, 0.5f);
			if (flag)
			{
				Utils.DrawInvBG(sb, vector3.X + (float)(num4 / 2), vector3.Y + (float)(num4 * 5 / 2), vector2.X / 3f - (float)num4, vector2.Y - (float)(num4 * 3));
				Utils.DrawInvBG(sb, vector3.X + vector2.X / 3f + (float)num4, vector3.Y + (float)(num4 * 5 / 2), vector2.X * 2f / 3f - (float)(num4 * 3 / 2), vector2.Y - (float)(num4 * 3));
			}
			else
			{
				Utils.DrawInvBG(sb, vector3.X + (float)(num4 / 2), vector3.Y + (float)(num4 * 5 / 2), vector2.X / 2f - (float)num4, vector2.Y - (float)(num4 * 3));
				Utils.DrawInvBG(sb, vector3.X + vector2.X / 2f + (float)num4, vector3.Y + (float)(num4 * 5 / 2), vector2.X / 2f - (float)(num4 * 3 / 2), vector2.Y - (float)(num4 * 3));
			}
			float num5 = 0.7f;
			float num6 = 0.8f;
			float num7 = 0.01f;
			if (flag)
			{
				num5 = 0.4f;
				num6 = 0.44f;
			}
			if (isActive2)
			{
				num5 = 0.55f;
				num6 = 0.6f;
			}
			if (oldLeftHover != leftHover && leftHover != -1)
			{
				SoundEngine.PlaySound(12);
			}
			if (oldRightHover != rightHover && rightHover != -1)
			{
				SoundEngine.PlaySound(12);
			}
			if (flag4 && rightHover != -1 && !noSound)
			{
				SoundEngine.PlaySound(12);
			}
			oldLeftHover = leftHover;
			oldRightHover = rightHover;
			noSound = false;
			bool flag5 = SocialAPI.Network != null && SocialAPI.Network.CanInvite();
			int num8 = (flag5 ? 1 : 0);
			int num9 = 5 + num8 + 2;
			Vector2 vector4 = new Vector2(vector3.X + vector2.X / 4f, vector3.Y + (float)(num4 * 5 / 2));
			Vector2 vector5 = new Vector2(0f, vector2.Y - (float)(num4 * 5)) / (num9 + 1);
			if (flag)
			{
				vector4.X -= 55f;
			}
			UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_LEFT = num9 + 1;
			for (int j = 0; j <= num9; j++)
			{
				bool flag6 = false;
				if (_leftSideCategoryMapping.TryGetValue(j, out var value))
				{
					flag6 = category == value;
				}
				if (leftHover == j || flag6)
				{
					leftScale[j] += num7;
				}
				else
				{
					leftScale[j] -= num7;
				}
				if (leftScale[j] < num5)
				{
					leftScale[j] = num5;
				}
				if (leftScale[j] > num6)
				{
					leftScale[j] = num6;
				}
			}
			leftHover = -1;
			int num10 = category;
			int num11 = 0;
			if (DrawLeftSide(sb, Lang.menu[114].Value, num11, vector4, vector5, leftScale))
			{
				leftHover = num11;
				if (flag4)
				{
					category = 0;
					SoundEngine.PlaySound(10);
				}
			}
			num11++;
			if (DrawLeftSide(sb, Lang.menu[210].Value, num11, vector4, vector5, leftScale))
			{
				leftHover = num11;
				if (flag4)
				{
					category = 1;
					SoundEngine.PlaySound(10);
				}
			}
			num11++;
			if (DrawLeftSide(sb, Lang.menu[63].Value, num11, vector4, vector5, leftScale))
			{
				leftHover = num11;
				if (flag4)
				{
					category = 2;
					SoundEngine.PlaySound(10);
				}
			}
			num11++;
			if (DrawLeftSide(sb, Lang.menu[218].Value, num11, vector4, vector5, leftScale))
			{
				leftHover = num11;
				if (flag4)
				{
					category = 3;
					SoundEngine.PlaySound(10);
				}
			}
			num11++;
			if (DrawLeftSide(sb, Lang.menu[66].Value, num11, vector4, vector5, leftScale))
			{
				leftHover = num11;
				if (flag4)
				{
					Close();
					IngameFancyUI.OpenKeybinds();
				}
			}
			num11++;
			if (flag5 && DrawLeftSide(sb, Lang.menu[147].Value, num11, vector4, vector5, leftScale))
			{
				leftHover = num11;
				if (flag4)
				{
					Close();
					SocialAPI.Network.OpenInviteInterface();
				}
			}
			if (flag5)
			{
				num11++;
			}
			if (DrawLeftSide(sb, Lang.menu[131].Value, num11, vector4, vector5, leftScale))
			{
				leftHover = num11;
				if (flag4)
				{
					Close();
					IngameFancyUI.OpenAchievements();
				}
			}
			num11++;
			if (DrawLeftSide(sb, Lang.menu[118].Value, num11, vector4, vector5, leftScale))
			{
				leftHover = num11;
				if (flag4)
				{
					Close();
				}
			}
			num11++;
			if (DrawLeftSide(sb, Lang.inter[35].Value, num11, vector4, vector5, leftScale))
			{
				leftHover = num11;
				if (flag4)
				{
					Close();
					Main.menuMode = 10;
					Main.gameMenu = true;
					WorldGen.SaveAndQuit();
				}
			}
			num11++;
			if (num10 != category)
			{
				for (int k = 0; k < rightScale.Length; k++)
				{
					rightScale[k] = 0f;
				}
			}
			int num12 = 0;
			int num13 = 0;
			switch (category)
			{
			case 0:
				num13 = 16;
				num5 = 1f;
				num6 = 1.001f;
				num7 = 0.001f;
				break;
			case 1:
				num13 = 11;
				num5 = 1f;
				num6 = 1.001f;
				num7 = 0.001f;
				break;
			case 2:
				num13 = 12;
				num5 = 1f;
				num6 = 1.001f;
				num7 = 0.001f;
				break;
			case 3:
				num13 = 15;
				num5 = 1f;
				num6 = 1.001f;
				num7 = 0.001f;
				break;
			}
			if (flag)
			{
				num5 -= 0.1f;
				num6 -= 0.1f;
			}
			if (isActive2 && category == 3)
			{
				num5 -= 0.15f;
				num6 -= 0.15f;
			}
			if (flag2 && (category == 0 || category == 3))
			{
				num5 -= 0.2f;
				num6 -= 0.2f;
			}
			UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT = num13;
			Vector2 vector6 = new Vector2(vector3.X + vector2.X * 3f / 4f, vector3.Y + (float)(num4 * 5 / 2));
			Vector2 vector7 = new Vector2(0f, vector2.Y - (float)(num4 * 3)) / (num13 + 1);
			if (category == 2)
			{
				vector7.Y -= 2f;
			}
			new Vector2(8f, 0f);
			if (flag)
			{
				vector6.X = vector3.X + vector2.X * 2f / 3f;
			}
			for (int l = 0; l < rightScale.Length; l++)
			{
				if (rightLock == l || (rightHover == l && rightLock == -1))
				{
					rightScale[l] += num7;
				}
				else
				{
					rightScale[l] -= num7;
				}
				if (rightScale[l] < num5)
				{
					rightScale[l] = num5;
				}
				if (rightScale[l] > num6)
				{
					rightScale[l] = num6;
				}
			}
			inBar = false;
			rightHover = -1;
			if (!Main.mouseLeft)
			{
				rightLock = -1;
			}
			if (rightLock == -1)
			{
				notBar = false;
			}
			if (category == 0)
			{
				int num14 = 0;
				DrawRightSide(sb, Lang.menu[65].Value, num14, vector6, vector7, rightScale[num14], 1f);
				skipRightSlot[num14] = true;
				num14++;
				vector6.X -= num;
				if (DrawRightSide(sb, Lang.menu[99].Value + " " + Math.Round(Main.musicVolume * 100f) + "%", num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					noSound = true;
					rightHover = num14;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				float musicVolume = DrawValueBar(sb, scale, Main.musicVolume);
				if ((inBar || rightLock == num14) && !notBar)
				{
					rightHover = num14;
					if (Main.mouseLeft && rightLock == num14)
					{
						Main.musicVolume = musicVolume;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num14;
				}
				if (rightHover == num14)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 2;
				}
				num14++;
				if (DrawRightSide(sb, Lang.menu[98].Value + " " + Math.Round(Main.soundVolume * 100f) + "%", num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num14;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				float soundVolume = DrawValueBar(sb, scale, Main.soundVolume);
				if ((inBar || rightLock == num14) && !notBar)
				{
					rightHover = num14;
					if (Main.mouseLeft && rightLock == num14)
					{
						Main.soundVolume = soundVolume;
						noSound = true;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num14;
				}
				if (rightHover == num14)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 3;
				}
				num14++;
				if (DrawRightSide(sb, Lang.menu[119].Value + " " + Math.Round(Main.ambientVolume * 100f) + "%", num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num14;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				float ambientVolume = DrawValueBar(sb, scale, Main.ambientVolume);
				if ((inBar || rightLock == num14) && !notBar)
				{
					rightHover = num14;
					if (Main.mouseLeft && rightLock == num14)
					{
						Main.ambientVolume = ambientVolume;
						noSound = true;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num14;
				}
				if (rightHover == num14)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 4;
				}
				num14++;
				vector6.X += num;
				DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], 1f);
				skipRightSlot[num14] = true;
				num14++;
				DrawRightSide(sb, Language.GetTextValue("GameUI.ZoomCategory"), num14, vector6, vector7, rightScale[num14], 1f);
				skipRightSlot[num14] = true;
				num14++;
				vector6.X -= num;
				string text = Language.GetTextValue("GameUI.GameZoom", Math.Round(Main.GameZoomTarget * 100f), Math.Round(Main.GameViewMatrix.Zoom.X * 100f));
				if (flag3)
				{
					text = FontAssets.ItemStack.get_Value().CreateWrappedText(text, num3, Language.ActiveCulture.CultureInfo);
				}
				if (DrawRightSide(sb, text, num14, vector6, vector7, rightScale[num14] * 0.85f, (rightScale[num14] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num14;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				float num15 = DrawValueBar(sb, scale, Main.GameZoomTarget - 1f);
				if ((inBar || rightLock == num14) && !notBar)
				{
					rightHover = num14;
					if (Main.mouseLeft && rightLock == num14)
					{
						Main.GameZoomTarget = num15 + 1f;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num14;
				}
				if (rightHover == num14)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 10;
				}
				num14++;
				bool flag7 = false;
				if (Main.temporaryGUIScaleSlider == -1f)
				{
					Main.temporaryGUIScaleSlider = Main.UIScaleWanted;
				}
				string text2 = Language.GetTextValue("GameUI.UIScale", Math.Round(Main.temporaryGUIScaleSlider * 100f), Math.Round(Main.UIScale * 100f));
				if (flag3)
				{
					text2 = FontAssets.ItemStack.get_Value().CreateWrappedText(text2, num3, Language.ActiveCulture.CultureInfo);
				}
				if (DrawRightSide(sb, text2, num14, vector6, vector7, rightScale[num14] * 0.75f, (rightScale[num14] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num14;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				float num16 = DrawValueBar(sb, scale, MathHelper.Clamp((Main.temporaryGUIScaleSlider - 0.5f) / 1.5f, 0f, 1f));
				if ((inBar || rightLock == num14) && !notBar)
				{
					rightHover = num14;
					if (Main.mouseLeft && rightLock == num14)
					{
						Main.temporaryGUIScaleSlider = num16 * 1.5f + 0.5f;
						Main.temporaryGUIScaleSlider = (float)(int)(Main.temporaryGUIScaleSlider * 100f) / 100f;
						Main.temporaryGUIScaleSliderUpdate = true;
						flag7 = true;
					}
				}
				if (!flag7 && Main.temporaryGUIScaleSliderUpdate && Main.temporaryGUIScaleSlider != -1f)
				{
					Main.UIScale = Main.temporaryGUIScaleSlider;
					Main.temporaryGUIScaleSliderUpdate = false;
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num14;
				}
				if (rightHover == num14)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 11;
				}
				num14++;
				vector6.X += num;
				DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], 1f);
				skipRightSlot[num14] = true;
				num14++;
				DrawRightSide(sb, Language.GetTextValue("GameUI.Gameplay"), num14, vector6, vector7, rightScale[num14], 1f);
				skipRightSlot[num14] = true;
				num14++;
				if (DrawRightSide(sb, Main.autoSave ? Lang.menu[67].Value : Lang.menu[68].Value, num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
				{
					rightHover = num14;
					if (flag4)
					{
						Main.autoSave = !Main.autoSave;
					}
				}
				num14++;
				if (DrawRightSide(sb, Main.autoPause ? Lang.menu[69].Value : Lang.menu[70].Value, num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
				{
					rightHover = num14;
					if (flag4)
					{
						Main.autoPause = !Main.autoPause;
					}
				}
				num14++;
				if (DrawRightSide(sb, Main.ReversedUpDownArmorSetBonuses ? Lang.menu[220].Value : Lang.menu[221].Value, num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
				{
					rightHover = num14;
					if (flag4)
					{
						Main.ReversedUpDownArmorSetBonuses = !Main.ReversedUpDownArmorSetBonuses;
					}
				}
				num14++;
				if (DrawRightSide(sb, DoorOpeningHelper.PreferenceSettings switch
				{
					DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForEverything => Language.GetTextValue("UI.SmartDoorsEnabled"), 
					DoorOpeningHelper.DoorAutoOpeningPreference.EnabledForGamepadOnly => Language.GetTextValue("UI.SmartDoorsGamepad"), 
					_ => Language.GetTextValue("UI.SmartDoorsDisabled"), 
				}, num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
				{
					rightHover = num14;
					if (flag4)
					{
						DoorOpeningHelper.CyclePreferences();
					}
				}
				num14++;
				string textValue;
				if (Player.Settings.HoverControl != 0)
				{
					_ = 1;
					textValue = Language.GetTextValue("UI.HoverControlSettingIsClick");
				}
				else
				{
					textValue = Language.GetTextValue("UI.HoverControlSettingIsHold");
				}
				if (DrawRightSide(sb, textValue, num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
				{
					rightHover = num14;
					if (flag4)
					{
						Player.Settings.CycleHoverControl();
					}
				}
				num14++;
				if (DrawRightSide(sb, Language.GetTextValue(Main.SettingsEnabled_AutoReuseAllItems ? "UI.AutoReuseAllOn" : "UI.AutoReuseAllOff"), num14, vector6, vector7, rightScale[num14], (rightScale[num14] - num5) / (num6 - num5)))
				{
					rightHover = num14;
					if (flag4)
					{
						Main.SettingsEnabled_AutoReuseAllItems = !Main.SettingsEnabled_AutoReuseAllItems;
					}
				}
				num14++;
				DrawRightSide(sb, "", num14, vector6, vector7, rightScale[num14], 1f);
				skipRightSlot[num14] = true;
				num14++;
			}
			if (category == 1)
			{
				int num17 = 0;
				if (DrawRightSide(sb, Main.showItemText ? Lang.menu[71].Value : Lang.menu[72].Value, num17, vector6, vector7, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
				{
					rightHover = num17;
					if (flag4)
					{
						Main.showItemText = !Main.showItemText;
					}
				}
				num17++;
				if (DrawRightSide(sb, Lang.menu[123].Value + " " + Lang.menu[124 + Main.invasionProgressMode], num17, vector6, vector7, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
				{
					rightHover = num17;
					if (flag4)
					{
						Main.invasionProgressMode++;
						if (Main.invasionProgressMode >= 3)
						{
							Main.invasionProgressMode = 0;
						}
					}
				}
				num17++;
				if (DrawRightSide(sb, Main.placementPreview ? Lang.menu[128].Value : Lang.menu[129].Value, num17, vector6, vector7, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
				{
					rightHover = num17;
					if (flag4)
					{
						Main.placementPreview = !Main.placementPreview;
					}
				}
				num17++;
				if (DrawRightSide(sb, ItemSlot.Options.HighlightNewItems ? Lang.inter[117].Value : Lang.inter[116].Value, num17, vector6, vector7, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
				{
					rightHover = num17;
					if (flag4)
					{
						ItemSlot.Options.HighlightNewItems = !ItemSlot.Options.HighlightNewItems;
					}
				}
				num17++;
				if (DrawRightSide(sb, Main.MouseShowBuildingGrid ? Lang.menu[229].Value : Lang.menu[230].Value, num17, vector6, vector7, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
				{
					rightHover = num17;
					if (flag4)
					{
						Main.MouseShowBuildingGrid = !Main.MouseShowBuildingGrid;
					}
				}
				num17++;
				if (DrawRightSide(sb, Main.GamepadDisableInstructionsDisplay ? Lang.menu[241].Value : Lang.menu[242].Value, num17, vector6, vector7, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
				{
					rightHover = num17;
					if (flag4)
					{
						Main.GamepadDisableInstructionsDisplay = !Main.GamepadDisableInstructionsDisplay;
					}
				}
				num17++;
				string textValue2 = Language.GetTextValue("UI.MinimapFrame_" + Main.MinimapFrameManagerInstance.ActiveSelectionKeyName);
				if (DrawRightSide(sb, Language.GetTextValue("UI.SelectMapBorder", textValue2), num17, vector6, vector7, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
				{
					rightHover = num17;
					if (flag4)
					{
						Main.MinimapFrameManagerInstance.CycleSelection();
					}
				}
				num17++;
				vector6.X -= num;
				string text3 = Language.GetTextValue("GameUI.MapScale", Math.Round(Main.MapScale * 100f));
				if (flag3)
				{
					text3 = FontAssets.ItemStack.get_Value().CreateWrappedText(text3, num3, Language.ActiveCulture.CultureInfo);
				}
				if (DrawRightSide(sb, text3, num17, vector6, vector7, rightScale[num17] * 0.85f, (rightScale[num17] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num17;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				float num18 = DrawValueBar(sb, scale, (Main.MapScale - 0.5f) / 0.5f);
				if ((inBar || rightLock == num17) && !notBar)
				{
					rightHover = num17;
					if (Main.mouseLeft && rightLock == num17)
					{
						Main.MapScale = num18 * 0.5f + 0.5f;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num17;
				}
				if (rightHover == num17)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 12;
				}
				num17++;
				vector6.X += num;
				string activeSetKeyName = Main.ResourceSetsManager.ActiveSetKeyName;
				string textValue3 = Language.GetTextValue("UI.HealthManaStyle_" + activeSetKeyName);
				if (DrawRightSide(sb, Language.GetTextValue("UI.SelectHealthStyle", textValue3), num17, vector6, vector7, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
				{
					rightHover = num17;
					if (flag4)
					{
						Main.ResourceSetsManager.CycleResourceSet();
					}
				}
				num17++;
				string textValue4 = Language.GetTextValue(BigProgressBarSystem.ShowText ? "UI.ShowBossLifeTextOn" : "UI.ShowBossLifeTextOff");
				if (DrawRightSide(sb, textValue4, num17, vector6, vector7, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
				{
					rightHover = num17;
					if (flag4)
					{
						BigProgressBarSystem.ToggleShowText();
					}
				}
				num17++;
				if (DrawRightSide(sb, Main.SettingsEnabled_OpaqueBoxBehindTooltips ? Language.GetTextValue("GameUI.HoverTextBoxesOn") : Language.GetTextValue("GameUI.HoverTextBoxesOff"), num17, vector6, vector7, rightScale[num17], (rightScale[num17] - num5) / (num6 - num5)))
				{
					rightHover = num17;
					if (flag4)
					{
						Main.SettingsEnabled_OpaqueBoxBehindTooltips = !Main.SettingsEnabled_OpaqueBoxBehindTooltips;
					}
				}
				num17++;
			}
			if (category == 2)
			{
				int num19 = 0;
				if (DrawRightSide(sb, Main.graphics.IsFullScreen ? Lang.menu[49].Value : Lang.menu[50].Value, num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						Main.ToggleFullScreen();
					}
				}
				num19++;
				if (DrawRightSide(sb, Lang.menu[51].Value + ": " + Main.PendingResolutionWidth + "x" + Main.PendingResolutionHeight, num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						int num20 = 0;
						for (int m = 0; m < Main.numDisplayModes; m++)
						{
							if (Main.displayWidth[m] == Main.PendingResolutionWidth && Main.displayHeight[m] == Main.PendingResolutionHeight)
							{
								num20 = m;
								break;
							}
						}
						num20++;
						if (num20 >= Main.numDisplayModes)
						{
							num20 = 0;
						}
						Main.PendingResolutionWidth = Main.displayWidth[num20];
						Main.PendingResolutionHeight = Main.displayHeight[num20];
						Main.SetResolution(Main.PendingResolutionWidth, Main.PendingResolutionHeight);
					}
				}
				num19++;
				vector6.X -= num;
				if (DrawRightSide(sb, Lang.menu[52].Value + ": " + Main.bgScroll + "%", num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					noSound = true;
					rightHover = num19;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				float num21 = DrawValueBar(sb, scale, (float)Main.bgScroll / 100f);
				if ((inBar || rightLock == num19) && !notBar)
				{
					rightHover = num19;
					if (Main.mouseLeft && rightLock == num19)
					{
						Main.bgScroll = (int)(num21 * 100f);
						Main.caveParallax = 1f - (float)Main.bgScroll / 500f;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num19;
				}
				if (rightHover == num19)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 1;
				}
				num19++;
				vector6.X += num;
				if (DrawRightSide(sb, Lang.menu[(int)(247 + Main.FrameSkipMode)].Value, num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						Main.CycleFrameSkipMode();
					}
				}
				num19++;
				if (DrawRightSide(sb, Language.GetTextValue("UI.LightMode_" + Lighting.Mode), num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						Lighting.NextLightMode();
					}
				}
				num19++;
				if (DrawRightSide(sb, Lang.menu[59 + Main.qaStyle].Value, num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						Main.qaStyle++;
						if (Main.qaStyle > 3)
						{
							Main.qaStyle = 0;
						}
					}
				}
				num19++;
				if (DrawRightSide(sb, Main.BackgroundEnabled ? Lang.menu[100].Value : Lang.menu[101].Value, num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						Main.BackgroundEnabled = !Main.BackgroundEnabled;
					}
				}
				num19++;
				if (DrawRightSide(sb, ChildSafety.Disabled ? Lang.menu[132].Value : Lang.menu[133].Value, num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						ChildSafety.Disabled = !ChildSafety.Disabled;
					}
				}
				num19++;
				if (DrawRightSide(sb, Language.GetTextValue("GameUI.HeatDistortion", Main.UseHeatDistortion ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						Main.UseHeatDistortion = !Main.UseHeatDistortion;
					}
				}
				num19++;
				if (DrawRightSide(sb, Language.GetTextValue("GameUI.StormEffects", Main.UseStormEffects ? Language.GetTextValue("GameUI.Enabled") : Language.GetTextValue("GameUI.Disabled")), num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						Main.UseStormEffects = !Main.UseStormEffects;
					}
				}
				num19++;
				if (DrawRightSide(sb, Language.GetTextValue("GameUI.WaveQuality", Main.WaveQuality switch
				{
					1 => Language.GetTextValue("GameUI.QualityLow"), 
					2 => Language.GetTextValue("GameUI.QualityMedium"), 
					3 => Language.GetTextValue("GameUI.QualityHigh"), 
					_ => Language.GetTextValue("GameUI.QualityOff"), 
				}), num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						Main.WaveQuality = (Main.WaveQuality + 1) % 4;
					}
				}
				num19++;
				if (DrawRightSide(sb, Language.GetTextValue("UI.TilesSwayInWind" + (Main.SettingsEnabled_TilesSwayInWind ? "On" : "Off")), num19, vector6, vector7, rightScale[num19], (rightScale[num19] - num5) / (num6 - num5)))
				{
					rightHover = num19;
					if (flag4)
					{
						Main.SettingsEnabled_TilesSwayInWind = !Main.SettingsEnabled_TilesSwayInWind;
					}
				}
				num19++;
			}
			if (category == 3)
			{
				int num22 = 0;
				float num23 = num;
				if (flag)
				{
					num2 = 126f;
				}
				Vector3 hSLVector = Main.mouseColorSlider.GetHSLVector();
				Main.mouseColorSlider.ApplyToMainLegacyBars();
				DrawRightSide(sb, Lang.menu[64].Value, num22, vector6, vector7, rightScale[num22], 1f);
				skipRightSlot[num22] = true;
				num22++;
				vector6.X -= num23;
				if (DrawRightSide(sb, "", num22, vector6, vector7, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				valuePosition.X -= num2;
				DelegateMethods.v3_1 = hSLVector;
				float x = DrawValueBar(sb, scale, hSLVector.X, 0, DelegateMethods.ColorLerp_HSL_H);
				if ((inBar || rightLock == num22) && !notBar)
				{
					rightHover = num22;
					if (Main.mouseLeft && rightLock == num22)
					{
						hSLVector.X = x;
						noSound = true;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				if (rightHover == num22)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 5;
					Main.menuMode = 25;
				}
				num22++;
				if (DrawRightSide(sb, "", num22, vector6, vector7, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				valuePosition.X -= num2;
				DelegateMethods.v3_1 = hSLVector;
				x = DrawValueBar(sb, scale, hSLVector.Y, 0, DelegateMethods.ColorLerp_HSL_S);
				if ((inBar || rightLock == num22) && !notBar)
				{
					rightHover = num22;
					if (Main.mouseLeft && rightLock == num22)
					{
						hSLVector.Y = x;
						noSound = true;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				if (rightHover == num22)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 6;
					Main.menuMode = 25;
				}
				num22++;
				if (DrawRightSide(sb, "", num22, vector6, vector7, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				valuePosition.X -= num2;
				DelegateMethods.v3_1 = hSLVector;
				DelegateMethods.v3_1.Z = Utils.GetLerpValue(0.15f, 1f, DelegateMethods.v3_1.Z, clamped: true);
				x = DrawValueBar(sb, scale, DelegateMethods.v3_1.Z, 0, DelegateMethods.ColorLerp_HSL_L);
				if ((inBar || rightLock == num22) && !notBar)
				{
					rightHover = num22;
					if (Main.mouseLeft && rightLock == num22)
					{
						hSLVector.Z = x * 0.85f + 0.15f;
						noSound = true;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				if (rightHover == num22)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 7;
					Main.menuMode = 25;
				}
				num22++;
				if (hSLVector.Z < 0.15f)
				{
					hSLVector.Z = 0.15f;
				}
				Main.mouseColorSlider.SetHSL(hSLVector);
				Main.mouseColor = Main.mouseColorSlider.GetColor();
				vector6.X += num23;
				DrawRightSide(sb, "", num22, vector6, vector7, rightScale[num22], 1f);
				skipRightSlot[num22] = true;
				num22++;
				hSLVector = Main.mouseBorderColorSlider.GetHSLVector();
				if (PlayerInput.UsingGamepad && rightHover == -1)
				{
					Main.mouseBorderColorSlider.ApplyToMainLegacyBars();
				}
				DrawRightSide(sb, Lang.menu[217].Value, num22, vector6, vector7, rightScale[num22], 1f);
				skipRightSlot[num22] = true;
				num22++;
				vector6.X -= num23;
				if (DrawRightSide(sb, "", num22, vector6, vector7, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				valuePosition.X -= num2;
				DelegateMethods.v3_1 = hSLVector;
				x = DrawValueBar(sb, scale, hSLVector.X, 0, DelegateMethods.ColorLerp_HSL_H);
				if ((inBar || rightLock == num22) && !notBar)
				{
					rightHover = num22;
					if (Main.mouseLeft && rightLock == num22)
					{
						hSLVector.X = x;
						noSound = true;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				if (rightHover == num22)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 5;
					Main.menuMode = 252;
				}
				num22++;
				if (DrawRightSide(sb, "", num22, vector6, vector7, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				valuePosition.X -= num2;
				DelegateMethods.v3_1 = hSLVector;
				x = DrawValueBar(sb, scale, hSLVector.Y, 0, DelegateMethods.ColorLerp_HSL_S);
				if ((inBar || rightLock == num22) && !notBar)
				{
					rightHover = num22;
					if (Main.mouseLeft && rightLock == num22)
					{
						hSLVector.Y = x;
						noSound = true;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				if (rightHover == num22)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 6;
					Main.menuMode = 252;
				}
				num22++;
				if (DrawRightSide(sb, "", num22, vector6, vector7, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				valuePosition.X -= num2;
				DelegateMethods.v3_1 = hSLVector;
				x = DrawValueBar(sb, scale, hSLVector.Z, 0, DelegateMethods.ColorLerp_HSL_L);
				if ((inBar || rightLock == num22) && !notBar)
				{
					rightHover = num22;
					if (Main.mouseLeft && rightLock == num22)
					{
						hSLVector.Z = x;
						noSound = true;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				if (rightHover == num22)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 7;
					Main.menuMode = 252;
				}
				num22++;
				if (DrawRightSide(sb, "", num22, vector6, vector7, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				valuePosition.X = vector3.X + vector2.X - (float)(num4 / 2) - 20f;
				valuePosition.Y -= 3f;
				valuePosition.X -= num2;
				DelegateMethods.v3_1 = hSLVector;
				float num24 = Main.mouseBorderColorSlider.Alpha;
				x = DrawValueBar(sb, scale, num24, 0, DelegateMethods.ColorLerp_HSL_O);
				if ((inBar || rightLock == num22) && !notBar)
				{
					rightHover = num22;
					if (Main.mouseLeft && rightLock == num22)
					{
						num24 = x;
						noSound = true;
					}
				}
				if ((float)Main.mouseX > vector3.X + vector2.X * 2f / 3f + (float)num4 && (float)Main.mouseX < valuePosition.X + 3.75f && (float)Main.mouseY > valuePosition.Y - 10f && (float)Main.mouseY <= valuePosition.Y + 10f)
				{
					if (rightLock == -1)
					{
						notBar = true;
					}
					rightHover = num22;
				}
				if (rightHover == num22)
				{
					UILinkPointNavigator.Shortcuts.OPTIONS_BUTTON_SPECIALFEATURE = 8;
					Main.menuMode = 252;
				}
				num22++;
				Main.mouseBorderColorSlider.SetHSL(hSLVector);
				Main.mouseBorderColorSlider.Alpha = num24;
				Main.MouseBorderColor = Main.mouseBorderColorSlider.GetColor();
				vector6.X += num23;
				DrawRightSide(sb, "", num22, vector6, vector7, rightScale[num22], 1f);
				skipRightSlot[num22] = true;
				num22++;
				string txt = "";
				switch (LockOnHelper.UseMode)
				{
				case LockOnHelper.LockOnMode.FocusTarget:
					txt = Lang.menu[232].Value;
					break;
				case LockOnHelper.LockOnMode.TargetClosest:
					txt = Lang.menu[233].Value;
					break;
				case LockOnHelper.LockOnMode.ThreeDS:
					txt = Lang.menu[234].Value;
					break;
				}
				if (DrawRightSide(sb, txt, num22, vector6, vector7, rightScale[num22] * 0.9f, (rightScale[num22] - num5) / (num6 - num5)))
				{
					rightHover = num22;
					if (flag4)
					{
						LockOnHelper.CycleUseModes();
					}
				}
				num22++;
				if (DrawRightSide(sb, Player.SmartCursorSettings.SmartBlocksEnabled ? Lang.menu[215].Value : Lang.menu[216].Value, num22, vector6, vector7, rightScale[num22] * 0.9f, (rightScale[num22] - num5) / (num6 - num5)))
				{
					rightHover = num22;
					if (flag4)
					{
						Player.SmartCursorSettings.SmartBlocksEnabled = !Player.SmartCursorSettings.SmartBlocksEnabled;
					}
				}
				num22++;
				if (DrawRightSide(sb, Main.cSmartCursorModeIsToggleAndNotHold ? Lang.menu[121].Value : Lang.menu[122].Value, num22, vector6, vector7, rightScale[num22], (rightScale[num22] - num5) / (num6 - num5)))
				{
					rightHover = num22;
					if (flag4)
					{
						Main.cSmartCursorModeIsToggleAndNotHold = !Main.cSmartCursorModeIsToggleAndNotHold;
					}
				}
				num22++;
				if (DrawRightSide(sb, Player.SmartCursorSettings.SmartAxeAfterPickaxe ? Lang.menu[214].Value : Lang.menu[213].Value, num22, vector6, vector7, rightScale[num22] * 0.9f, (rightScale[num22] - num5) / (num6 - num5)))
				{
					rightHover = num22;
					if (flag4)
					{
						Player.SmartCursorSettings.SmartAxeAfterPickaxe = !Player.SmartCursorSettings.SmartAxeAfterPickaxe;
					}
				}
				num22++;
			}
			if (rightHover != -1 && rightLock == -1)
			{
				rightLock = rightHover;
			}
			for (int n = 0; n < num9 + 1; n++)
			{
				UILinkPointNavigator.SetPosition(2900 + n, vector4 + vector5 * (n + 1));
			}
			Vector2 zero = Vector2.Zero;
			if (flag)
			{
				zero.X = -40f;
			}
			for (int num25 = 0; num25 < num13; num25++)
			{
				if (!skipRightSlot[num25])
				{
					UILinkPointNavigator.SetPosition(2930 + num12, vector6 + zero + vector7 * (num25 + 1));
					num12++;
				}
			}
			UILinkPointNavigator.Shortcuts.INGAMEOPTIONS_BUTTONS_RIGHT = num12;
			Main.DrawInterface_29_SettingsButton();
			Main.DrawGamepadInstructions();
			Main.mouseText = false;
			Main.instance.GUIBarsDraw();
			Main.instance.DrawMouseOver();
			Main.DrawCursor(Main.DrawThickCursor());
		}

		public static void MouseOver()
		{
			if (Main.ingameOptionsWindow)
			{
				if (_GUIHover.Contains(Main.MouseScreen.ToPoint()))
				{
					Main.mouseText = true;
				}
				if (_mouseOverText != null)
				{
					Main.instance.MouseText(_mouseOverText, 0, 0);
				}
				_mouseOverText = null;
			}
		}

		public static bool DrawLeftSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float[] scales, float minscale = 0.7f, float maxscale = 0.8f, float scalespeed = 0.01f)
		{
			bool flag = false;
			if (_leftSideCategoryMapping.TryGetValue(i, out var value))
			{
				flag = category == value;
			}
			Color color = Color.Lerp(Color.Gray, Color.White, (scales[i] - minscale) / (maxscale - minscale));
			if (flag)
			{
				color = Color.Gold;
			}
			Vector2 vector = Utils.DrawBorderStringBig(sb, txt, anchor + offset * (1 + i), color, scales[i], 0.5f, 0.5f);
			bool flag2 = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));
			if (!_canConsumeHover)
			{
				return false;
			}
			if (flag2)
			{
				_canConsumeHover = false;
				return true;
			}
			return false;
		}

		public static bool DrawRightSide(SpriteBatch sb, string txt, int i, Vector2 anchor, Vector2 offset, float scale, float colorScale, Color over = default(Color))
		{
			Color color = Color.Lerp(Color.Gray, Color.White, colorScale);
			if (over != default(Color))
			{
				color = over;
			}
			Vector2 vector = Utils.DrawBorderString(sb, txt, anchor + offset * (1 + i), color, scale, 0.5f, 0.5f);
			valuePosition = anchor + offset * (1 + i) + vector * new Vector2(0.5f, 0f);
			bool flag = new Rectangle((int)anchor.X - (int)vector.X / 2, (int)anchor.Y + (int)(offset.Y * (float)(1 + i)) - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));
			if (!_canConsumeHover)
			{
				return false;
			}
			if (flag)
			{
				_canConsumeHover = false;
				return true;
			}
			return false;
		}

		public static Rectangle GetExpectedRectangleForNotification(int itemIndex, Vector2 anchor, Vector2 offset, int areaWidth)
		{
			return Utils.CenteredRectangle(anchor + offset * (1 + itemIndex), new Vector2(areaWidth, offset.Y - 4f));
		}

		public static bool DrawValue(SpriteBatch sb, string txt, int i, float scale, Color over = default(Color))
		{
			Color color = Color.Gray;
			Vector2 vector = FontAssets.MouseText.get_Value().MeasureString(txt) * scale;
			bool flag = new Rectangle((int)valuePosition.X, (int)valuePosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y).Contains(new Point(Main.mouseX, Main.mouseY));
			if (flag)
			{
				color = Color.White;
			}
			if (over != default(Color))
			{
				color = over;
			}
			Utils.DrawBorderString(sb, txt, valuePosition, color, scale, 0f, 0.5f);
			valuePosition.X += vector.X;
			if (!_canConsumeHover)
			{
				return false;
			}
			if (flag)
			{
				_canConsumeHover = false;
				return true;
			}
			return false;
		}

		public static float DrawValueBar(SpriteBatch sb, float scale, float perc, int lockState = 0, Utils.ColorLerpMethod colorMethod = null)
		{
			if (colorMethod == null)
			{
				colorMethod = Utils.ColorLerp_BlackToWhite;
			}
			Texture2D value = TextureAssets.ColorBar.get_Value();
			Vector2 vector = new Vector2(value.Width, value.Height) * scale;
			valuePosition.X -= (int)vector.X;
			Rectangle rectangle = new Rectangle((int)valuePosition.X, (int)valuePosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y);
			Rectangle destinationRectangle = rectangle;
			sb.Draw(value, rectangle, Color.White);
			int num = 167;
			float num2 = (float)rectangle.X + 5f * scale;
			float num3 = (float)rectangle.Y + 4f * scale;
			for (float num4 = 0f; num4 < (float)num; num4 += 1f)
			{
				float percent = num4 / (float)num;
				sb.Draw(TextureAssets.ColorBlip.get_Value(), new Vector2(num2 + num4 * scale, num3), null, colorMethod(percent), 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			}
			rectangle.Inflate((int)(-5f * scale), 0);
			bool flag = rectangle.Contains(new Point(Main.mouseX, Main.mouseY));
			if (lockState == 2)
			{
				flag = false;
			}
			if (flag || lockState == 1)
			{
				sb.Draw(TextureAssets.ColorHighlight.get_Value(), destinationRectangle, Main.OurFavoriteColor);
			}
			sb.Draw(TextureAssets.ColorSlider.get_Value(), new Vector2(num2 + 167f * scale * perc, num3 + 4f * scale), null, Color.White, 0f, new Vector2(0.5f * (float)TextureAssets.ColorSlider.Width(), 0.5f * (float)TextureAssets.ColorSlider.Height()), scale, SpriteEffects.None, 0f);
			if (Main.mouseX >= rectangle.X && Main.mouseX <= rectangle.X + rectangle.Width)
			{
				inBar = flag;
				return (float)(Main.mouseX - rectangle.X) / (float)rectangle.Width;
			}
			inBar = false;
			if (rectangle.X >= Main.mouseX)
			{
				return 0f;
			}
			return 1f;
		}
	}
}
