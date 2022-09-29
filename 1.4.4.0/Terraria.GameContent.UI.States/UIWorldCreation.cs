using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	public class UIWorldCreation : UIState
	{
		private enum WorldSizeId
		{
			Small,
			Medium,
			Large
		}

		private enum WorldDifficultyId
		{
			Normal,
			Expert,
			Master,
			Creative
		}

		private enum WorldEvilId
		{
			Random,
			Corruption,
			Crimson
		}

		private WorldSizeId _optionSize;

		private WorldDifficultyId _optionDifficulty;

		private WorldEvilId _optionEvil;

		private string _optionwWorldName;

		private string _optionSeed;

		private UICharacterNameButton _namePlate;

		private UICharacterNameButton _seedPlate;

		private UIWorldCreationPreview _previewPlate;

		private GroupOptionButton<WorldSizeId>[] _sizeButtons;

		private GroupOptionButton<WorldDifficultyId>[] _difficultyButtons;

		private GroupOptionButton<WorldEvilId>[] _evilButtons;

		private UIText _descriptionText;

		public const int MAX_NAME_LENGTH = 27;

		public const int MAX_SEED_LENGTH = 40;

		public UIWorldCreation()
		{
			BuildPage();
		}

		private void BuildPage()
		{
			int num = 18;
			RemoveAllChildren();
			UIElement uIElement = new UIElement
			{
				Width = StyleDimension.FromPixels(500f),
				Height = StyleDimension.FromPixels(434f + (float)num),
				Top = StyleDimension.FromPixels(170f - (float)num),
				HAlign = 0.5f,
				VAlign = 0f
			};
			uIElement.SetPadding(0f);
			Append(uIElement);
			UIPanel uIPanel = new UIPanel
			{
				Width = StyleDimension.FromPercent(1f),
				Height = StyleDimension.FromPixels(280 + num),
				Top = StyleDimension.FromPixels(50f),
				BackgroundColor = new Color(33, 43, 79) * 0.8f
			};
			uIPanel.SetPadding(0f);
			uIElement.Append(uIPanel);
			MakeBackAndCreatebuttons(uIElement);
			UIElement uIElement2 = new UIElement
			{
				Top = StyleDimension.FromPixelsAndPercent(0f, 0f),
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				HAlign = 1f
			};
			uIElement2.SetPadding(0f);
			uIElement2.PaddingTop = 8f;
			uIElement2.PaddingBottom = 12f;
			uIPanel.Append(uIElement2);
			MakeInfoMenu(uIElement2);
		}

		private void MakeInfoMenu(UIElement parentContainer)
		{
			UIElement uIElement = new UIElement
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				HAlign = 0.5f,
				VAlign = 0f
			};
			uIElement.SetPadding(10f);
			uIElement.PaddingBottom = 0f;
			uIElement.PaddingTop = 0f;
			parentContainer.Append(uIElement);
			float num = 0f;
			float num2 = 44f;
			float num3 = 88f + num2;
			float pixels = num2;
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, Language.GetText("UI.WorldCreationRandomizeNameDescription"), Color.White, "Images/UI/WorldCreation/IconRandomName")
			{
				Width = StyleDimension.FromPixelsAndPercent(40f, 0f),
				Height = new StyleDimension(40f, 0f),
				HAlign = 0f,
				Top = StyleDimension.FromPixelsAndPercent(num, 0f),
				ShowHighlightWhenSelected = false
			};
			groupOptionButton.OnMouseDown += ClickRandomizeName;
			groupOptionButton.OnMouseOver += ShowOptionDescription;
			groupOptionButton.OnMouseOut += ClearOptionDescription;
			groupOptionButton.SetSnapPoint("RandomizeName", 0);
			uIElement.Append(groupOptionButton);
			UICharacterNameButton uICharacterNameButton = new UICharacterNameButton(Language.GetText("UI.WorldCreationName"), Language.GetText("UI.WorldCreationNameEmpty"), Language.GetText("UI.WorldDescriptionName"))
			{
				Width = StyleDimension.FromPixelsAndPercent(0f - num3, 1f),
				HAlign = 0f,
				Left = new StyleDimension(pixels, 0f),
				Top = StyleDimension.FromPixelsAndPercent(num, 0f)
			};
			uICharacterNameButton.OnMouseDown += Click_SetName;
			uICharacterNameButton.OnMouseOver += ShowOptionDescription;
			uICharacterNameButton.OnMouseOut += ClearOptionDescription;
			uICharacterNameButton.SetSnapPoint("Name", 0);
			uIElement.Append(uICharacterNameButton);
			_namePlate = uICharacterNameButton;
			num += uICharacterNameButton.GetDimensions().Height + 4f;
			GroupOptionButton<bool> groupOptionButton2 = new GroupOptionButton<bool>(option: true, null, Language.GetText("UI.WorldCreationRandomizeSeedDescription"), Color.White, "Images/UI/WorldCreation/IconRandomSeed")
			{
				Width = StyleDimension.FromPixelsAndPercent(40f, 0f),
				Height = new StyleDimension(40f, 0f),
				HAlign = 0f,
				Top = StyleDimension.FromPixelsAndPercent(num, 0f),
				ShowHighlightWhenSelected = false
			};
			groupOptionButton2.OnMouseDown += ClickRandomizeSeed;
			groupOptionButton2.OnMouseOver += ShowOptionDescription;
			groupOptionButton2.OnMouseOut += ClearOptionDescription;
			groupOptionButton2.SetSnapPoint("RandomizeSeed", 0);
			uIElement.Append(groupOptionButton2);
			UICharacterNameButton uICharacterNameButton2 = new UICharacterNameButton(Language.GetText("UI.WorldCreationSeed"), Language.GetText("UI.WorldCreationSeedEmpty"), Language.GetText("UI.WorldDescriptionSeed"))
			{
				Width = StyleDimension.FromPixelsAndPercent(0f - num3, 1f),
				HAlign = 0f,
				Left = new StyleDimension(pixels, 0f),
				Top = StyleDimension.FromPixelsAndPercent(num, 0f),
				DistanceFromTitleToOption = 29f
			};
			uICharacterNameButton2.OnMouseDown += Click_SetSeed;
			uICharacterNameButton2.OnMouseOver += ShowOptionDescription;
			uICharacterNameButton2.OnMouseOut += ClearOptionDescription;
			uICharacterNameButton2.SetSnapPoint("Seed", 0);
			uIElement.Append(uICharacterNameButton2);
			_seedPlate = uICharacterNameButton2;
			UIWorldCreationPreview uIWorldCreationPreview = new UIWorldCreationPreview
			{
				Width = StyleDimension.FromPixels(84f),
				Height = StyleDimension.FromPixels(84f),
				HAlign = 1f,
				VAlign = 0f
			};
			uIElement.Append(uIWorldCreationPreview);
			_previewPlate = uIWorldCreationPreview;
			num += uICharacterNameButton2.GetDimensions().Height + 10f;
			AddHorizontalSeparator(uIElement, num + 2f);
			float usableWidthPercent = 1f;
			AddWorldSizeOptions(uIElement, num, ClickSizeOption, "size", usableWidthPercent);
			num += 48f;
			AddHorizontalSeparator(uIElement, num);
			AddWorldDifficultyOptions(uIElement, num, ClickDifficultyOption, "difficulty", usableWidthPercent);
			num += 48f;
			AddHorizontalSeparator(uIElement, num);
			AddWorldEvilOptions(uIElement, num, ClickEvilOption, "evil", usableWidthPercent);
			num += 48f;
			AddHorizontalSeparator(uIElement, num);
			AddDescriptionPanel(uIElement, num, "desc");
			SetDefaultOptions();
		}

		private static void AddHorizontalSeparator(UIElement Container, float accumualtedHeight)
		{
			UIHorizontalSeparator element = new UIHorizontalSeparator
			{
				Width = StyleDimension.FromPercent(1f),
				Top = StyleDimension.FromPixels(accumualtedHeight - 8f),
				Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
			};
			Container.Append(element);
		}

		private void SetDefaultOptions()
		{
			AssignRandomWorldName();
			AssignRandomWorldSeed();
			UpdateInputFields();
			GroupOptionButton<WorldSizeId>[] sizeButtons = _sizeButtons;
			for (int i = 0; i < sizeButtons.Length; i++)
			{
				sizeButtons[i].SetCurrentOption(WorldSizeId.Medium);
			}
			_optionSize = WorldSizeId.Medium;
			if (Main.ActivePlayerFileData.Player.difficulty == 3)
			{
				GroupOptionButton<WorldDifficultyId>[] difficultyButtons = _difficultyButtons;
				for (int i = 0; i < difficultyButtons.Length; i++)
				{
					difficultyButtons[i].SetCurrentOption(WorldDifficultyId.Creative);
				}
				_optionDifficulty = WorldDifficultyId.Creative;
				UpdatePreviewPlate();
			}
			else
			{
				GroupOptionButton<WorldDifficultyId>[] difficultyButtons = _difficultyButtons;
				for (int i = 0; i < difficultyButtons.Length; i++)
				{
					difficultyButtons[i].SetCurrentOption(WorldDifficultyId.Normal);
				}
			}
			GroupOptionButton<WorldEvilId>[] evilButtons = _evilButtons;
			for (int i = 0; i < evilButtons.Length; i++)
			{
				evilButtons[i].SetCurrentOption(WorldEvilId.Random);
			}
		}

		private void AddDescriptionPanel(UIElement container, float accumulatedHeight, string tagGroup)
		{
			float num = 0f;
			UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1))
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f),
				Left = StyleDimension.FromPixels(0f - num),
				Height = StyleDimension.FromPixelsAndPercent(40f, 0f),
				Top = StyleDimension.FromPixels(2f)
			};
			uISlicedImage.SetSliceDepths(10);
			uISlicedImage.Color = Color.LightGray * 0.7f;
			container.Append(uISlicedImage);
			UIText uIText = new UIText(Language.GetText("UI.WorldDescriptionDefault"), 0.82f)
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Top = StyleDimension.FromPixelsAndPercent(5f, 0f)
			};
			uIText.PaddingLeft = 20f;
			uIText.PaddingRight = 20f;
			uIText.PaddingTop = 6f;
			uISlicedImage.Append(uIText);
			_descriptionText = uIText;
		}

		private void AddWorldSizeOptions(UIElement container, float accumualtedHeight, MouseEvent clickEvent, string tagGroup, float usableWidthPercent)
		{
			WorldSizeId[] array = new WorldSizeId[3]
			{
				WorldSizeId.Small,
				WorldSizeId.Medium,
				WorldSizeId.Large
			};
			LocalizedText[] array2 = new LocalizedText[3]
			{
				Lang.menu[92],
				Lang.menu[93],
				Lang.menu[94]
			};
			LocalizedText[] array3 = new LocalizedText[3]
			{
				Language.GetText("UI.WorldDescriptionSizeSmall"),
				Language.GetText("UI.WorldDescriptionSizeMedium"),
				Language.GetText("UI.WorldDescriptionSizeLarge")
			};
			Color[] array4 = new Color[3]
			{
				Color.Cyan,
				Color.Lerp(Color.Cyan, Color.LimeGreen, 0.5f),
				Color.LimeGreen
			};
			string[] array5 = new string[3] { "Images/UI/WorldCreation/IconSizeSmall", "Images/UI/WorldCreation/IconSizeMedium", "Images/UI/WorldCreation/IconSizeLarge" };
			GroupOptionButton<WorldSizeId>[] array6 = new GroupOptionButton<WorldSizeId>[array.Length];
			for (int i = 0; i < array6.Length; i++)
			{
				GroupOptionButton<WorldSizeId> groupOptionButton = new GroupOptionButton<WorldSizeId>(array[i], array2[i], array3[i], array4[i], array5[i], 1f, 1f, 16f);
				groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(-4 * (array6.Length - 1), 1f / (float)array6.Length * usableWidthPercent);
				groupOptionButton.Left = StyleDimension.FromPercent(1f - usableWidthPercent);
				groupOptionButton.HAlign = (float)i / (float)(array6.Length - 1);
				groupOptionButton.Top.Set(accumualtedHeight, 0f);
				groupOptionButton.OnMouseDown += clickEvent;
				groupOptionButton.OnMouseOver += ShowOptionDescription;
				groupOptionButton.OnMouseOut += ClearOptionDescription;
				groupOptionButton.SetSnapPoint(tagGroup, i);
				container.Append(groupOptionButton);
				array6[i] = groupOptionButton;
			}
			_sizeButtons = array6;
		}

		private void AddWorldDifficultyOptions(UIElement container, float accumualtedHeight, MouseEvent clickEvent, string tagGroup, float usableWidthPercent)
		{
			WorldDifficultyId[] array = new WorldDifficultyId[4]
			{
				WorldDifficultyId.Creative,
				WorldDifficultyId.Normal,
				WorldDifficultyId.Expert,
				WorldDifficultyId.Master
			};
			LocalizedText[] array2 = new LocalizedText[4]
			{
				Language.GetText("UI.Creative"),
				Language.GetText("UI.Normal"),
				Language.GetText("UI.Expert"),
				Language.GetText("UI.Master")
			};
			LocalizedText[] array3 = new LocalizedText[4]
			{
				Language.GetText("UI.WorldDescriptionCreative"),
				Language.GetText("UI.WorldDescriptionNormal"),
				Language.GetText("UI.WorldDescriptionExpert"),
				Language.GetText("UI.WorldDescriptionMaster")
			};
			Color[] array4 = new Color[4]
			{
				Main.creativeModeColor,
				Color.White,
				Main.mcColor,
				Main.hcColor
			};
			string[] array5 = new string[4] { "Images/UI/WorldCreation/IconDifficultyCreative", "Images/UI/WorldCreation/IconDifficultyNormal", "Images/UI/WorldCreation/IconDifficultyExpert", "Images/UI/WorldCreation/IconDifficultyMaster" };
			GroupOptionButton<WorldDifficultyId>[] array6 = new GroupOptionButton<WorldDifficultyId>[array.Length];
			for (int i = 0; i < array6.Length; i++)
			{
				GroupOptionButton<WorldDifficultyId> groupOptionButton = new GroupOptionButton<WorldDifficultyId>(array[i], array2[i], array3[i], array4[i], array5[i], 1f, 1f, 16f);
				groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(-1 * (array6.Length - 1), 1f / (float)array6.Length * usableWidthPercent);
				groupOptionButton.Left = StyleDimension.FromPercent(1f - usableWidthPercent);
				groupOptionButton.HAlign = (float)i / (float)(array6.Length - 1);
				groupOptionButton.Top.Set(accumualtedHeight, 0f);
				groupOptionButton.OnMouseDown += clickEvent;
				groupOptionButton.OnMouseOver += ShowOptionDescription;
				groupOptionButton.OnMouseOut += ClearOptionDescription;
				groupOptionButton.SetSnapPoint(tagGroup, i);
				container.Append(groupOptionButton);
				array6[i] = groupOptionButton;
			}
			_difficultyButtons = array6;
		}

		private void AddWorldEvilOptions(UIElement container, float accumualtedHeight, MouseEvent clickEvent, string tagGroup, float usableWidthPercent)
		{
			WorldEvilId[] array = new WorldEvilId[3]
			{
				WorldEvilId.Random,
				WorldEvilId.Corruption,
				WorldEvilId.Crimson
			};
			LocalizedText[] array2 = new LocalizedText[3]
			{
				Lang.misc[103],
				Lang.misc[101],
				Lang.misc[102]
			};
			LocalizedText[] array3 = new LocalizedText[3]
			{
				Language.GetText("UI.WorldDescriptionEvilRandom"),
				Language.GetText("UI.WorldDescriptionEvilCorrupt"),
				Language.GetText("UI.WorldDescriptionEvilCrimson")
			};
			Color[] array4 = new Color[3]
			{
				Color.White,
				Color.MediumPurple,
				Color.IndianRed
			};
			string[] array5 = new string[3] { "Images/UI/WorldCreation/IconEvilRandom", "Images/UI/WorldCreation/IconEvilCorruption", "Images/UI/WorldCreation/IconEvilCrimson" };
			GroupOptionButton<WorldEvilId>[] array6 = new GroupOptionButton<WorldEvilId>[array.Length];
			for (int i = 0; i < array6.Length; i++)
			{
				GroupOptionButton<WorldEvilId> groupOptionButton = new GroupOptionButton<WorldEvilId>(array[i], array2[i], array3[i], array4[i], array5[i], 1f, 1f, 16f);
				groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(-4 * (array6.Length - 1), 1f / (float)array6.Length * usableWidthPercent);
				groupOptionButton.Left = StyleDimension.FromPercent(1f - usableWidthPercent);
				groupOptionButton.HAlign = (float)i / (float)(array6.Length - 1);
				groupOptionButton.Top.Set(accumualtedHeight, 0f);
				groupOptionButton.OnMouseDown += clickEvent;
				groupOptionButton.OnMouseOver += ShowOptionDescription;
				groupOptionButton.OnMouseOut += ClearOptionDescription;
				groupOptionButton.SetSnapPoint(tagGroup, i);
				container.Append(groupOptionButton);
				array6[i] = groupOptionButton;
			}
			_evilButtons = array6;
		}

		private void ClickRandomizeName(UIMouseEvent evt, UIElement listeningElement)
		{
			AssignRandomWorldName();
			UpdateInputFields();
			UpdateSliders();
			UpdatePreviewPlate();
		}

		private void ClickRandomizeSeed(UIMouseEvent evt, UIElement listeningElement)
		{
			AssignRandomWorldSeed();
			UpdateInputFields();
			UpdateSliders();
			UpdatePreviewPlate();
		}

		private void ClickSizeOption(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<WorldSizeId> groupOptionButton = (GroupOptionButton<WorldSizeId>)listeningElement;
			_optionSize = groupOptionButton.OptionValue;
			GroupOptionButton<WorldSizeId>[] sizeButtons = _sizeButtons;
			for (int i = 0; i < sizeButtons.Length; i++)
			{
				sizeButtons[i].SetCurrentOption(groupOptionButton.OptionValue);
			}
			UpdatePreviewPlate();
		}

		private void ClickDifficultyOption(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<WorldDifficultyId> groupOptionButton = (GroupOptionButton<WorldDifficultyId>)listeningElement;
			_optionDifficulty = groupOptionButton.OptionValue;
			GroupOptionButton<WorldDifficultyId>[] difficultyButtons = _difficultyButtons;
			for (int i = 0; i < difficultyButtons.Length; i++)
			{
				difficultyButtons[i].SetCurrentOption(groupOptionButton.OptionValue);
			}
			UpdatePreviewPlate();
		}

		private void ClickEvilOption(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<WorldEvilId> groupOptionButton = (GroupOptionButton<WorldEvilId>)listeningElement;
			_optionEvil = groupOptionButton.OptionValue;
			GroupOptionButton<WorldEvilId>[] evilButtons = _evilButtons;
			for (int i = 0; i < evilButtons.Length; i++)
			{
				evilButtons[i].SetCurrentOption(groupOptionButton.OptionValue);
			}
			UpdatePreviewPlate();
		}

		private void UpdatePreviewPlate()
		{
			_previewPlate.UpdateOption((byte)_optionDifficulty, (byte)_optionEvil, (byte)_optionSize);
		}

		private void UpdateSliders()
		{
			GroupOptionButton<WorldSizeId>[] sizeButtons = _sizeButtons;
			for (int i = 0; i < sizeButtons.Length; i++)
			{
				sizeButtons[i].SetCurrentOption(_optionSize);
			}
			GroupOptionButton<WorldDifficultyId>[] difficultyButtons = _difficultyButtons;
			for (int i = 0; i < difficultyButtons.Length; i++)
			{
				difficultyButtons[i].SetCurrentOption(_optionDifficulty);
			}
			GroupOptionButton<WorldEvilId>[] evilButtons = _evilButtons;
			for (int i = 0; i < evilButtons.Length; i++)
			{
				evilButtons[i].SetCurrentOption(_optionEvil);
			}
		}

		public void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			LocalizedText localizedText = null;
			if (listeningElement is GroupOptionButton<WorldSizeId> groupOptionButton)
			{
				localizedText = groupOptionButton.Description;
			}
			if (listeningElement is GroupOptionButton<WorldDifficultyId> groupOptionButton2)
			{
				localizedText = groupOptionButton2.Description;
			}
			if (listeningElement is GroupOptionButton<WorldEvilId> groupOptionButton3)
			{
				localizedText = groupOptionButton3.Description;
			}
			if (listeningElement is UICharacterNameButton uICharacterNameButton)
			{
				localizedText = uICharacterNameButton.Description;
			}
			if (listeningElement is GroupOptionButton<bool> groupOptionButton4)
			{
				localizedText = groupOptionButton4.Description;
			}
			if (localizedText != null)
			{
				_descriptionText.SetText(localizedText);
			}
		}

		public void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			_descriptionText.SetText(Language.GetText("UI.WorldDescriptionDefault"));
		}

		private void MakeBackAndCreatebuttons(UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true)
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
				Height = StyleDimension.FromPixels(50f),
				VAlign = 1f,
				HAlign = 0f,
				Top = StyleDimension.FromPixels(-45f)
			};
			uITextPanel.OnMouseOver += FadedMouseOver;
			uITextPanel.OnMouseOut += FadedMouseOut;
			uITextPanel.OnMouseDown += Click_GoBack;
			uITextPanel.SetSnapPoint("Back", 0);
			outerContainer.Append(uITextPanel);
			UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Create"), 0.7f, large: true)
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
				Height = StyleDimension.FromPixels(50f),
				VAlign = 1f,
				HAlign = 1f,
				Top = StyleDimension.FromPixels(-45f)
			};
			uITextPanel2.OnMouseOver += FadedMouseOver;
			uITextPanel2.OnMouseOut += FadedMouseOut;
			uITextPanel2.OnMouseDown += Click_NamingAndCreating;
			uITextPanel2.SetSnapPoint("Create", 0);
			outerContainer.Append(uITextPanel2);
		}

		private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(11);
			Main.OpenWorldSelectUI();
		}

		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		private void Click_SetName(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10);
			Main.clrInput();
			UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Lang.menu[48].Value, "", OnFinishedSettingName, GoBackHere, 0, allowEmpty: true);
			uIVirtualKeyboard.SetMaxInputLength(27);
			Main.MenuUI.SetState(uIVirtualKeyboard);
		}

		private void Click_SetSeed(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10);
			Main.clrInput();
			UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Language.GetTextValue("UI.EnterSeed"), "", OnFinishedSettingSeed, GoBackHere, 0, allowEmpty: true);
			uIVirtualKeyboard.SetMaxInputLength(40);
			Main.MenuUI.SetState(uIVirtualKeyboard);
		}

		private void Click_NamingAndCreating(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10);
			if (string.IsNullOrEmpty(_optionwWorldName))
			{
				_optionwWorldName = "";
				Main.clrInput();
				UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Lang.menu[48].Value, "", OnFinishedNamingAndCreating, GoBackHere);
				uIVirtualKeyboard.SetMaxInputLength(27);
				Main.MenuUI.SetState(uIVirtualKeyboard);
			}
			else
			{
				FinishCreatingWorld();
			}
		}

		private void OnFinishedSettingName(string name)
		{
			_optionwWorldName = name.Trim();
			UpdateInputFields();
			GoBackHere();
		}

		private void UpdateInputFields()
		{
			_namePlate.SetContents(_optionwWorldName);
			_namePlate.Recalculate();
			_namePlate.TrimDisplayIfOverElementDimensions(27);
			_namePlate.Recalculate();
			_seedPlate.SetContents(_optionSeed);
			_seedPlate.Recalculate();
			_seedPlate.TrimDisplayIfOverElementDimensions(40);
			_seedPlate.Recalculate();
		}

		private void OnFinishedSettingSeed(string seed)
		{
			_optionSeed = seed.Trim();
			ProcessSeed(out var processedSeed);
			_optionSeed = processedSeed;
			UpdateInputFields();
			UpdateSliders();
			UpdatePreviewPlate();
			GoBackHere();
		}

		private void GoBackHere()
		{
			Main.MenuUI.SetState(this);
		}

		private void OnFinishedNamingAndCreating(string name)
		{
			OnFinishedSettingName(name);
			FinishCreatingWorld();
		}

		private void FinishCreatingWorld()
		{
			ProcessSeed(out var processedSeed);
			switch (_optionSize)
			{
			case WorldSizeId.Small:
				Main.maxTilesX = 4200;
				Main.maxTilesY = 1200;
				break;
			case WorldSizeId.Medium:
				Main.maxTilesX = 6400;
				Main.maxTilesY = 1800;
				break;
			case WorldSizeId.Large:
				Main.maxTilesX = 8400;
				Main.maxTilesY = 2400;
				break;
			}
			WorldGen.setWorldSize();
			switch (_optionDifficulty)
			{
			case WorldDifficultyId.Creative:
				Main.GameMode = 3;
				break;
			case WorldDifficultyId.Normal:
				Main.GameMode = 0;
				break;
			case WorldDifficultyId.Expert:
				Main.GameMode = 1;
				break;
			case WorldDifficultyId.Master:
				Main.GameMode = 2;
				break;
			}
			switch (_optionEvil)
			{
			case WorldEvilId.Random:
				WorldGen.WorldGenParam_Evil = -1;
				break;
			case WorldEvilId.Corruption:
				WorldGen.WorldGenParam_Evil = 0;
				break;
			case WorldEvilId.Crimson:
				WorldGen.WorldGenParam_Evil = 1;
				break;
			}
			Main.ActiveWorldFileData = WorldFile.CreateMetadata(Main.worldName = _optionwWorldName.Trim(), SocialAPI.Cloud != null && SocialAPI.Cloud.EnabledByDefault, Main.GameMode);
			if (processedSeed.Length == 0)
			{
				Main.ActiveWorldFileData.SetSeedToRandom();
			}
			else
			{
				Main.ActiveWorldFileData.SetSeed(processedSeed);
			}
			Main.menuMode = 10;
			WorldGen.CreateNewWorld();
		}

		public static void ProcessSpecialWorldSeeds(string processedSeed)
		{
			WorldGen.noTrapsWorldGen = false;
			WorldGen.notTheBees = false;
			WorldGen.getGoodWorldGen = false;
			WorldGen.tenthAnniversaryWorldGen = false;
			WorldGen.dontStarveWorldGen = false;
			WorldGen.tempRemixWorldGen = false;
			WorldGen.tempTenthAnniversaryWorldGen = false;
			WorldGen.everythingWorldGen = false;
			if (processedSeed.ToLower() == "no traps" || processedSeed.ToLower() == "notraps")
			{
				WorldGen.noTrapsWorldGen = true;
			}
			if (processedSeed.ToLower() == "not the bees" || processedSeed.ToLower() == "not the bees!" || processedSeed.ToLower() == "notthebees")
			{
				WorldGen.notTheBees = true;
			}
			if (processedSeed.ToLower() == "for the worthy" || processedSeed.ToLower() == "fortheworthy")
			{
				WorldGen.getGoodWorldGen = true;
			}
			if (processedSeed.ToLower() == "don't dig up" || processedSeed.ToLower() == "dont dig up" || processedSeed.ToLower() == "dontdigup")
			{
				WorldGen.tempRemixWorldGen = true;
			}
			if (processedSeed.ToLower() == "celebrationmk10")
			{
				WorldGen.tempTenthAnniversaryWorldGen = true;
			}
			if (processedSeed.ToLower() == "constant" || processedSeed.ToLower() == "theconstant" || processedSeed.ToLower() == "the constant" || processedSeed.ToLower() == "eye4aneye" || processedSeed.ToLower() == "eyeforaneye")
			{
				WorldGen.dontStarveWorldGen = true;
			}
			if (processedSeed.ToLower() == "get fixed boi" || processedSeed.ToLower() == "getfixedboi")
			{
				WorldGen.noTrapsWorldGen = true;
				WorldGen.notTheBees = true;
				WorldGen.getGoodWorldGen = true;
				WorldGen.tempTenthAnniversaryWorldGen = true;
				WorldGen.dontStarveWorldGen = true;
				WorldGen.tempRemixWorldGen = true;
				WorldGen.everythingWorldGen = true;
			}
		}

		private void ProcessSeed(out string processedSeed)
		{
			processedSeed = _optionSeed;
			ProcessSpecialWorldSeeds(processedSeed);
			string[] array = _optionSeed.Split(new char[1] { '.' });
			if (array.Length != 4)
			{
				return;
			}
			if (int.TryParse(array[0], out var result))
			{
				switch (result)
				{
				case 1:
					_optionSize = WorldSizeId.Small;
					break;
				case 2:
					_optionSize = WorldSizeId.Medium;
					break;
				case 3:
					_optionSize = WorldSizeId.Large;
					break;
				}
			}
			if (int.TryParse(array[1], out result))
			{
				switch (result)
				{
				case 1:
					_optionDifficulty = WorldDifficultyId.Normal;
					break;
				case 2:
					_optionDifficulty = WorldDifficultyId.Expert;
					break;
				case 3:
					_optionDifficulty = WorldDifficultyId.Master;
					break;
				case 4:
					_optionDifficulty = WorldDifficultyId.Creative;
					break;
				}
			}
			if (int.TryParse(array[2], out result))
			{
				switch (result)
				{
				case 1:
					_optionEvil = WorldEvilId.Corruption;
					break;
				case 2:
					_optionEvil = WorldEvilId.Crimson;
					break;
				}
			}
			processedSeed = array[3];
		}

		private void AssignRandomWorldName()
		{
			do
			{
				LocalizedText localizedText = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Composition."));
				LocalizedText localizedText2 = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Adjective."));
				LocalizedText localizedText3 = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Location."));
				LocalizedText localizedText4 = Language.SelectRandom(Lang.CreateDialogFilter("RandomWorldName_Noun."));
				var obj = new
				{
					Adjective = localizedText2.Value,
					Location = localizedText3.Value,
					Noun = localizedText4.Value
				};
				_optionwWorldName = localizedText.FormatWith(obj);
				if (Main.rand.Next(10000) == 0)
				{
					_optionwWorldName = Language.GetTextValue("SpecialWorldName.TheConstant");
				}
			}
			while (_optionwWorldName.Length > 27);
		}

		private void AssignRandomWorldSeed()
		{
			_optionSeed = Main.rand.Next().ToString();
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			SetupGamepadPoints(spriteBatch);
		}

		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
			int num = 3000;
			List<SnapPoint> snapPoints = GetSnapPoints();
			SnapPoint snapPoint = null;
			SnapPoint snapPoint2 = null;
			SnapPoint snapPoint3 = null;
			SnapPoint snapPoint4 = null;
			SnapPoint snapPoint5 = null;
			SnapPoint snapPoint6 = null;
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint7 = snapPoints[i];
				switch (snapPoint7.Name)
				{
				case "Back":
					snapPoint = snapPoint7;
					break;
				case "Create":
					snapPoint2 = snapPoint7;
					break;
				case "Name":
					snapPoint3 = snapPoint7;
					break;
				case "Seed":
					snapPoint4 = snapPoint7;
					break;
				case "RandomizeName":
					snapPoint5 = snapPoint7;
					break;
				case "RandomizeSeed":
					snapPoint6 = snapPoint7;
					break;
				}
			}
			List<SnapPoint> snapGroup = GetSnapGroup(snapPoints, "size");
			List<SnapPoint> snapGroup2 = GetSnapGroup(snapPoints, "difficulty");
			List<SnapPoint> snapGroup3 = GetSnapGroup(snapPoints, "evil");
			UILinkPointNavigator.SetPosition(num, snapPoint.Position);
			UILinkPoint uILinkPoint = UILinkPointNavigator.Points[num];
			uILinkPoint.Unlink();
			UILinkPoint uILinkPoint2 = uILinkPoint;
			num++;
			UILinkPointNavigator.SetPosition(num, snapPoint2.Position);
			uILinkPoint = UILinkPointNavigator.Points[num];
			uILinkPoint.Unlink();
			UILinkPoint uILinkPoint3 = uILinkPoint;
			num++;
			UILinkPointNavigator.SetPosition(num, snapPoint5.Position);
			uILinkPoint = UILinkPointNavigator.Points[num];
			uILinkPoint.Unlink();
			UILinkPoint uILinkPoint4 = uILinkPoint;
			num++;
			UILinkPointNavigator.SetPosition(num, snapPoint3.Position);
			uILinkPoint = UILinkPointNavigator.Points[num];
			uILinkPoint.Unlink();
			UILinkPoint uILinkPoint5 = uILinkPoint;
			num++;
			UILinkPointNavigator.SetPosition(num, snapPoint6.Position);
			uILinkPoint = UILinkPointNavigator.Points[num];
			uILinkPoint.Unlink();
			UILinkPoint uILinkPoint6 = uILinkPoint;
			num++;
			UILinkPointNavigator.SetPosition(num, snapPoint4.Position);
			uILinkPoint = UILinkPointNavigator.Points[num];
			uILinkPoint.Unlink();
			UILinkPoint uILinkPoint7 = uILinkPoint;
			num++;
			UILinkPoint[] array = new UILinkPoint[snapGroup.Count];
			for (int j = 0; j < snapGroup.Count; j++)
			{
				UILinkPointNavigator.SetPosition(num, snapGroup[j].Position);
				uILinkPoint = UILinkPointNavigator.Points[num];
				uILinkPoint.Unlink();
				array[j] = uILinkPoint;
				num++;
			}
			UILinkPoint[] array2 = new UILinkPoint[snapGroup2.Count];
			for (int k = 0; k < snapGroup2.Count; k++)
			{
				UILinkPointNavigator.SetPosition(num, snapGroup2[k].Position);
				uILinkPoint = UILinkPointNavigator.Points[num];
				uILinkPoint.Unlink();
				array2[k] = uILinkPoint;
				num++;
			}
			UILinkPoint[] array3 = new UILinkPoint[snapGroup3.Count];
			for (int l = 0; l < snapGroup3.Count; l++)
			{
				UILinkPointNavigator.SetPosition(num, snapGroup3[l].Position);
				uILinkPoint = UILinkPointNavigator.Points[num];
				uILinkPoint.Unlink();
				array3[l] = uILinkPoint;
				num++;
			}
			LoopHorizontalLineLinks(array);
			LoopHorizontalLineLinks(array2);
			EstablishUpDownRelationship(array, array2);
			for (int m = 0; m < array.Length; m++)
			{
				array[m].Up = uILinkPoint7.ID;
			}
			if (true)
			{
				LoopHorizontalLineLinks(array3);
				EstablishUpDownRelationship(array2, array3);
				for (int n = 0; n < array3.Length; n++)
				{
					array3[n].Down = uILinkPoint2.ID;
				}
				array3[^1].Down = uILinkPoint3.ID;
				uILinkPoint3.Up = array3[^1].ID;
				uILinkPoint2.Up = array3[0].ID;
			}
			else
			{
				for (int num2 = 0; num2 < array2.Length; num2++)
				{
					array2[num2].Down = uILinkPoint2.ID;
				}
				array2[^1].Down = uILinkPoint3.ID;
				uILinkPoint3.Up = array2[^1].ID;
				uILinkPoint2.Up = array2[0].ID;
			}
			uILinkPoint3.Left = uILinkPoint2.ID;
			uILinkPoint2.Right = uILinkPoint3.ID;
			uILinkPoint5.Down = uILinkPoint7.ID;
			uILinkPoint5.Left = uILinkPoint4.ID;
			uILinkPoint4.Right = uILinkPoint5.ID;
			uILinkPoint7.Up = uILinkPoint5.ID;
			uILinkPoint7.Down = array[0].ID;
			uILinkPoint7.Left = uILinkPoint6.ID;
			uILinkPoint6.Right = uILinkPoint7.ID;
			uILinkPoint6.Up = uILinkPoint4.ID;
			uILinkPoint6.Down = array[0].ID;
			uILinkPoint4.Down = uILinkPoint6.ID;
		}

		private void EstablishUpDownRelationship(UILinkPoint[] topSide, UILinkPoint[] bottomSide)
		{
			int num = Math.Max(topSide.Length, bottomSide.Length);
			for (int i = 0; i < num; i++)
			{
				int num2 = Math.Min(i, topSide.Length - 1);
				int num3 = Math.Min(i, bottomSide.Length - 1);
				topSide[num2].Down = bottomSide[num3].ID;
				bottomSide[num3].Up = topSide[num2].ID;
			}
		}

		private void LoopHorizontalLineLinks(UILinkPoint[] pointsLine)
		{
			for (int i = 1; i < pointsLine.Length - 1; i++)
			{
				pointsLine[i - 1].Right = pointsLine[i].ID;
				pointsLine[i].Left = pointsLine[i - 1].ID;
				pointsLine[i].Right = pointsLine[i + 1].ID;
				pointsLine[i + 1].Left = pointsLine[i].ID;
			}
		}

		private List<SnapPoint> GetSnapGroup(List<SnapPoint> ptsOnPage, string groupName)
		{
			List<SnapPoint> list = ptsOnPage.Where((SnapPoint a) => a.Name == groupName).ToList();
			list.Sort(SortPoints);
			return list;
		}

		private int SortPoints(SnapPoint a, SnapPoint b)
		{
			return a.Id.CompareTo(b.Id);
		}
	}
}
