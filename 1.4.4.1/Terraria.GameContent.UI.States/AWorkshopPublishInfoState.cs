using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Social;
using Terraria.Social.Base;
using Terraria.UI;
using Terraria.UI.Gamepad;
using Terraria.Utilities.FileBrowser;

namespace Terraria.GameContent.UI.States
{
	public abstract class AWorkshopPublishInfoState<TPublishedObjectType> : UIState, IHaveBackButtonCommand
	{
		protected UIState _previousUIState;

		protected TPublishedObjectType _dataObject;

		protected string _publishedObjectNameDescriptorTexKey;

		protected string _instructionsTextKey;

		private UIElement _uiListContainer;

		private UIElement _uiListRect;

		private UIScrollbar _scrollbar;

		private bool _isScrollbarAttached;

		private UIText _descriptionText;

		private UIElement _listContainer;

		private UIElement _backButton;

		private UIElement _publishButton;

		private WorkshopItemPublicSettingId _optionPublicity = WorkshopItemPublicSettingId.Public;

		private GroupOptionButton<WorkshopItemPublicSettingId>[] _publicityOptions;

		private List<GroupOptionButton<WorkshopTagOption>> _tagOptions;

		private UICharacterNameButton _previewImagePathPlate;

		private Texture2D _previewImageTransientTexture;

		private UIImage _previewImageUIElement;

		private string _previewImagePath;

		private Asset<Texture2D> _defaultPreviewImageTexture;

		private UIElement _steamDisclaimerButton;

		private UIText _disclaimerText;

		private UIGamepadHelper _helper;

		public AWorkshopPublishInfoState(UIState stateToGoBackTo, TPublishedObjectType dataObject)
		{
			_previousUIState = stateToGoBackTo;
			_dataObject = dataObject;
		}

		public override void OnInitialize()
		{
			base.OnInitialize();
			int num = 40;
			int num2 = 200;
			int num3 = 50 + num + 10;
			int num4 = 70;
			UIElement uIElement = new UIElement();
			uIElement.Width.Set(600f, 0f);
			uIElement.Top.Set(num2, 0f);
			uIElement.Height.Set(-num2, 1f);
			uIElement.HAlign = 0.5f;
			UIPanel uIPanel = new UIPanel();
			uIPanel.Width.Set(0f, 1f);
			uIPanel.Height.Set(-num3, 1f);
			uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			AddBackButton(num, uIElement);
			AddPublishButton(num, uIElement);
			int num5 = 6 + num4;
			UIList uiList = AddUIList(uIPanel, num5);
			FillUIList(uiList);
			AddHorizontalSeparator(uIPanel, 0f).Top = new StyleDimension(-num4 + 3, 1f);
			AddDescriptionPanel(uIPanel, num4 - 6, "desc");
			uIElement.Append(uIPanel);
			Append(uIElement);
			SetDefaultOptions();
		}

		private void SetDefaultOptions()
		{
			_optionPublicity = WorkshopItemPublicSettingId.Public;
			GroupOptionButton<WorkshopItemPublicSettingId>[] publicityOptions = _publicityOptions;
			for (int i = 0; i < publicityOptions.Length; i++)
			{
				publicityOptions[i].SetCurrentOption(_optionPublicity);
			}
			SetTagsFromFoundEntry();
			UpdateImagePreview();
		}

		private void FillUIList(UIList uiList)
		{
			UIElement uIElement = new UIElement
			{
				Width = new StyleDimension(0f, 0f),
				Height = new StyleDimension(0f, 0f)
			};
			uIElement.SetPadding(0f);
			uiList.Add(uIElement);
			uiList.Add(CreateSteamDisclaimer("disclaimer"));
			uiList.Add(CreatePreviewImageSelectionPanel("image"));
			uiList.Add(CreatePublicSettingsRow(0f, 44f, "public"));
			uiList.Add(CreateTagOptionsPanel(0f, 44, "tags"));
		}

		private UIElement CreatePreviewImageSelectionPanel(string tagGroup)
		{
			UIElement obj = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(80f, 0f)
			};
			UIElement uIElement = new UIElement
			{
				Width = new StyleDimension(72f, 0f),
				Height = new StyleDimension(72f, 0f),
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension(-6f, 0f),
				Top = new StyleDimension(0f, 0f)
			};
			uIElement.SetPadding(0f);
			obj.Append(uIElement);
			float num = 86f;
			_defaultPreviewImageTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/DefaultPreviewImage", (AssetRequestMode)1);
			UIImage uIImage = new UIImage(_defaultPreviewImageTexture)
			{
				Width = new StyleDimension(-4f, 1f),
				Height = new StyleDimension(-4f, 1f),
				HAlign = 0.5f,
				VAlign = 0.5f,
				ScaleToFit = true,
				AllowResizingDimensions = false
			};
			UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", (AssetRequestMode)1))
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			uIElement.Append(uIImage);
			uIElement.Append(element);
			_previewImageUIElement = uIImage;
			UICharacterNameButton uICharacterNameButton = new UICharacterNameButton(Language.GetText("Workshop.PreviewImagePathTitle"), Language.GetText("Workshop.PreviewImagePathEmpty"), Language.GetText("Workshop.PreviewImagePathDescription"))
			{
				Width = StyleDimension.FromPixelsAndPercent(0f - num, 1f),
				Height = new StyleDimension(0f, 1f)
			};
			uICharacterNameButton.OnMouseDown += Click_SetPreviewImage;
			uICharacterNameButton.OnMouseOver += ShowOptionDescription;
			uICharacterNameButton.OnMouseOut += ClearOptionDescription;
			uICharacterNameButton.SetSnapPoint(tagGroup, 0);
			obj.Append(uICharacterNameButton);
			_previewImagePathPlate = uICharacterNameButton;
			return obj;
		}

		private void SetTagsFromFoundEntry()
		{
			if (!TryFindingTags(out var info))
			{
				return;
			}
			if (info.tags != null)
			{
				foreach (GroupOptionButton<WorkshopTagOption> tagOption in _tagOptions)
				{
					bool flag = info.tags.Contains(tagOption.OptionValue.InternalNameForAPIs);
					tagOption.SetCurrentOption(flag ? tagOption.OptionValue : null);
					tagOption.SetColor(tagOption.IsSelected ? new Color(152, 175, 235) : Colors.InventoryDefaultColor, 1f);
				}
			}
			GroupOptionButton<WorkshopItemPublicSettingId>[] publicityOptions = _publicityOptions;
			for (int i = 0; i < publicityOptions.Length; i++)
			{
				publicityOptions[i].SetCurrentOption(info.publicity);
			}
		}

		protected abstract bool TryFindingTags(out FoundWorkshopEntryInfo info);

		private void Click_SetPreviewImage(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10);
			OpenFileDialogueToSelectPreviewImage();
		}

		private UIElement CreateSteamDisclaimer(string tagGroup)
		{
			float num = 60f;
			float num2 = 0f + num;
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, null, Color.White, null, 1f, 0.5f, 16f);
			groupOptionButton.HAlign = 0.5f;
			groupOptionButton.VAlign = 0f;
			groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(0f, 1f);
			groupOptionButton.Left = StyleDimension.FromPixels(0f);
			groupOptionButton.Height = StyleDimension.FromPixelsAndPercent(num2 + 4f, 0f);
			groupOptionButton.Top = StyleDimension.FromPixels(0f);
			groupOptionButton.ShowHighlightWhenSelected = false;
			groupOptionButton.SetCurrentOption(option: false);
			groupOptionButton.Width.Set(0f, 1f);
			UIElement uIElement = new UIElement
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(num, 0f)
			};
			groupOptionButton.Append(uIElement);
			UIText uIText = new UIText(Language.GetText("Workshop.SteamDisclaimer"))
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-40f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				TextColor = Color.Cyan,
				IgnoresMouseInteraction = true
			};
			uIText.PaddingLeft = 20f;
			uIText.PaddingRight = 20f;
			uIText.PaddingTop = 4f;
			uIText.IsWrapped = true;
			_disclaimerText = uIText;
			groupOptionButton.OnClick += steamDisclaimerText_OnClick;
			groupOptionButton.OnMouseOver += steamDisclaimerText_OnMouseOver;
			groupOptionButton.OnMouseOut += steamDisclaimerText_OnMouseOut;
			uIElement.Append(uIText);
			uIText.SetSnapPoint(tagGroup, 0);
			_steamDisclaimerButton = uIText;
			return groupOptionButton;
		}

		private void steamDisclaimerText_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			_disclaimerText.TextColor = Color.Cyan;
			ClearOptionDescription(evt, listeningElement);
		}

		private void steamDisclaimerText_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12);
			_disclaimerText.TextColor = Color.LightCyan;
			ShowOptionDescription(evt, listeningElement);
		}

		private void steamDisclaimerText_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			try
			{
				Platform.Get<IPathService>().OpenURL("https://steamcommunity.com/sharedfiles/workshoplegalagreement");
			}
			catch (Exception)
			{
			}
		}

		public override void Recalculate()
		{
			UpdateScrollbar();
			base.Recalculate();
		}

		private void UpdateScrollbar()
		{
			if (_scrollbar != null)
			{
				if (_isScrollbarAttached && !_scrollbar.CanScroll)
				{
					_uiListContainer.RemoveChild(_scrollbar);
					_isScrollbarAttached = false;
					_uiListRect.Width.Set(0f, 1f);
				}
				else if (!_isScrollbarAttached && _scrollbar.CanScroll)
				{
					_uiListContainer.Append(_scrollbar);
					_isScrollbarAttached = true;
					_uiListRect.Width.Set(-25f, 1f);
				}
			}
		}

		private UIList AddUIList(UIElement container, float antiHeight)
		{
			_uiListContainer = container;
			float num = 0f;
			UIElement uIElement = (_listContainer = new UIElement
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f),
				Left = StyleDimension.FromPixels(0f - num),
				Height = StyleDimension.FromPixelsAndPercent(-2f - antiHeight, 1f),
				OverflowHidden = true
			});
			UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/Workshop/ListBackground", (AssetRequestMode)1))
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				Color = Color.White * 0.7f
			};
			uISlicedImage.SetSliceDepths(4);
			container.Append(uIElement);
			uIElement.Append(uISlicedImage);
			UIList uIList = new UIList
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-4f, 1f),
				HAlign = 0.5f,
				VAlign = 0.5f,
				OverflowHidden = true
			};
			uIList.ManualSortMethod = ManualIfnoSortingMethod;
			uIList.ListPadding = 5f;
			uIElement.Append(uIList);
			UIScrollbar uIScrollbar = new UIScrollbar
			{
				HAlign = 1f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f),
				Left = StyleDimension.FromPixels(0f - num),
				Height = StyleDimension.FromPixelsAndPercent(-14f - antiHeight, 1f),
				Top = StyleDimension.FromPixels(6f)
			};
			uIScrollbar.SetView(100f, 1000f);
			uIList.SetScrollbar(uIScrollbar);
			_uiListRect = uIElement;
			_scrollbar = uIScrollbar;
			return uIList;
		}

		private void ManualIfnoSortingMethod(List<UIElement> list)
		{
		}

		private UIElement CreatePublicSettingsRow(float accumulatedHeight, float height, string tagGroup)
		{
			CreateStylizedCategoryPanel(height, "Workshop.CategoryTitlePublicity", out var entirePanel, out var innerPanel);
			WorkshopItemPublicSettingId[] array = new WorkshopItemPublicSettingId[3]
			{
				WorkshopItemPublicSettingId.Public,
				WorkshopItemPublicSettingId.FriendsOnly,
				WorkshopItemPublicSettingId.Private
			};
			LocalizedText[] array2 = new LocalizedText[3]
			{
				Language.GetText("Workshop.SettingsPublicityPublic"),
				Language.GetText("Workshop.SettingsPublicityFriendsOnly"),
				Language.GetText("Workshop.SettingsPublicityPrivate")
			};
			LocalizedText[] array3 = new LocalizedText[3]
			{
				Language.GetText("Workshop.SettingsPublicityPublicDescription"),
				Language.GetText("Workshop.SettingsPublicityFriendsOnlyDescription"),
				Language.GetText("Workshop.SettingsPublicityPrivateDescription")
			};
			Color[] array4 = new Color[3]
			{
				Color.White,
				Color.White,
				Color.White
			};
			string[] array5 = new string[3] { "Images/UI/Workshop/PublicityPublic", "Images/UI/Workshop/PublicityFriendsOnly", "Images/UI/Workshop/PublicityPrivate" };
			float num = 0.98f;
			GroupOptionButton<WorkshopItemPublicSettingId>[] array6 = new GroupOptionButton<WorkshopItemPublicSettingId>[array.Length];
			for (int i = 0; i < array6.Length; i++)
			{
				GroupOptionButton<WorkshopItemPublicSettingId> groupOptionButton = new GroupOptionButton<WorkshopItemPublicSettingId>(array[i], array2[i], array3[i], array4[i], array5[i], 1f, 1f, 16f);
				groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(-4 * (array6.Length - 1), 1f / (float)array6.Length * num);
				groupOptionButton.HAlign = (float)i / (float)(array6.Length - 1);
				groupOptionButton.Left = StyleDimension.FromPercent((1f - num) * (1f - groupOptionButton.HAlign * 2f));
				groupOptionButton.Top.Set(accumulatedHeight, 0f);
				groupOptionButton.OnMouseDown += ClickPublicityOption;
				groupOptionButton.OnMouseOver += ShowOptionDescription;
				groupOptionButton.OnMouseOut += ClearOptionDescription;
				groupOptionButton.SetSnapPoint(tagGroup, i);
				innerPanel.Append(groupOptionButton);
				array6[i] = groupOptionButton;
			}
			_publicityOptions = array6;
			return entirePanel;
		}

		private UIElement CreateTagOptionsPanel(float accumulatedHeight, int heightPerRow, string tagGroup)
		{
			List<WorkshopTagOption> tagsToShow = GetTagsToShow();
			int num = 3;
			int num2 = (int)Math.Ceiling((float)tagsToShow.Count / (float)num);
			int num3 = heightPerRow * num2;
			CreateStylizedCategoryPanel(num3, "Workshop.CategoryTitleTags", out var entirePanel, out var innerPanel);
			float num4 = 0.98f;
			List<GroupOptionButton<WorkshopTagOption>> list = new List<GroupOptionButton<WorkshopTagOption>>();
			for (int i = 0; i < tagsToShow.Count; i++)
			{
				WorkshopTagOption workshopTagOption = tagsToShow[i];
				GroupOptionButton<WorkshopTagOption> groupOptionButton = new GroupOptionButton<WorkshopTagOption>(workshopTagOption, Language.GetText(workshopTagOption.NameKey), Language.GetText(workshopTagOption.NameKey + "Description"), Color.White, null, 1f, 0.5f, 16f);
				groupOptionButton.ShowHighlightWhenSelected = false;
				groupOptionButton.SetCurrentOption(null);
				int num5 = i / num;
				int num6 = i - num5 * num;
				groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(-4 * (num - 1), 1f / (float)num * num4);
				groupOptionButton.HAlign = (float)num6 / (float)(num - 1);
				groupOptionButton.Left = StyleDimension.FromPercent((1f - num4) * (1f - groupOptionButton.HAlign * 2f));
				groupOptionButton.Top.Set(num5 * heightPerRow, 0f);
				groupOptionButton.OnMouseDown += ClickTagOption;
				groupOptionButton.OnMouseOver += ShowOptionDescription;
				groupOptionButton.OnMouseOut += ClearOptionDescription;
				groupOptionButton.SetSnapPoint(tagGroup, i);
				innerPanel.Append(groupOptionButton);
				list.Add(groupOptionButton);
			}
			_tagOptions = list;
			return entirePanel;
		}

		private void CreateStylizedCategoryPanel(float height, string titleTextKey, out UIElement entirePanel, out UIElement innerPanel)
		{
			float num = 44f;
			UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel", (AssetRequestMode)1))
			{
				HAlign = 0.5f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Left = StyleDimension.FromPixels(0f),
				Height = StyleDimension.FromPixelsAndPercent(height + num + 4f, 0f),
				Top = StyleDimension.FromPixels(0f)
			};
			uISlicedImage.SetSliceDepths(8);
			uISlicedImage.Color = Color.White * 0.7f;
			innerPanel = new UIElement
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(height, 0f)
			};
			uISlicedImage.Append(innerPanel);
			AddHorizontalSeparator(uISlicedImage, num, 4);
			UIText uIText = new UIText(Language.GetText(titleTextKey))
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-40f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(num, 0f),
				Top = StyleDimension.FromPixelsAndPercent(5f, 0f)
			};
			uIText.PaddingLeft = 20f;
			uIText.PaddingRight = 20f;
			uIText.PaddingTop = 6f;
			uIText.IsWrapped = false;
			uISlicedImage.Append(uIText);
			entirePanel = uISlicedImage;
		}

		private void ClickTagOption(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<WorkshopTagOption> groupOptionButton = (GroupOptionButton<WorkshopTagOption>)listeningElement;
			groupOptionButton.SetCurrentOption(groupOptionButton.IsSelected ? null : groupOptionButton.OptionValue);
			groupOptionButton.SetColor(groupOptionButton.IsSelected ? new Color(152, 175, 235) : Colors.InventoryDefaultColor, 1f);
		}

		private void ClickPublicityOption(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<WorkshopItemPublicSettingId> groupOptionButton = (GroupOptionButton<WorkshopItemPublicSettingId>)listeningElement;
			_optionPublicity = groupOptionButton.OptionValue;
			GroupOptionButton<WorkshopItemPublicSettingId>[] publicityOptions = _publicityOptions;
			for (int i = 0; i < publicityOptions.Length; i++)
			{
				publicityOptions[i].SetCurrentOption(groupOptionButton.OptionValue);
			}
		}

		public void ShowOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			LocalizedText localizedText = null;
			if (listeningElement is GroupOptionButton<WorkshopItemPublicSettingId> groupOptionButton)
			{
				localizedText = groupOptionButton.Description;
			}
			if (listeningElement is UICharacterNameButton uICharacterNameButton)
			{
				localizedText = uICharacterNameButton.Description;
			}
			if (listeningElement is GroupOptionButton<bool> groupOptionButton2)
			{
				localizedText = groupOptionButton2.Description;
			}
			if (listeningElement is GroupOptionButton<WorkshopTagOption> groupOptionButton3)
			{
				localizedText = groupOptionButton3.Description;
			}
			if (listeningElement == _steamDisclaimerButton)
			{
				localizedText = Language.GetText("Workshop.SteamDisclaimerDescrpition");
			}
			if (localizedText != null)
			{
				_descriptionText.SetText(localizedText);
			}
		}

		public void ClearOptionDescription(UIMouseEvent evt, UIElement listeningElement)
		{
			_descriptionText.SetText(Language.GetText("Workshop.InfoDescriptionDefault"));
		}

		private UIElement CreateInsturctionsPanel(float accumulatedHeight, float height, string tagGroup)
		{
			float num = 0f;
			UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1));
			uISlicedImage.HAlign = 0.5f;
			uISlicedImage.VAlign = 0f;
			uISlicedImage.Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f);
			uISlicedImage.Left = StyleDimension.FromPixels(0f - num);
			uISlicedImage.Height = StyleDimension.FromPixelsAndPercent(height, 0f);
			uISlicedImage.Top = StyleDimension.FromPixels(accumulatedHeight);
			uISlicedImage.SetSliceDepths(10);
			uISlicedImage.Color = Color.LightGray * 0.7f;
			UIText uIText = new UIText(Language.GetText(_instructionsTextKey))
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(-40f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Top = StyleDimension.FromPixelsAndPercent(5f, 0f)
			};
			uIText.PaddingLeft = 20f;
			uIText.PaddingRight = 20f;
			uIText.PaddingTop = 6f;
			uIText.IsWrapped = true;
			uISlicedImage.Append(uIText);
			return uISlicedImage;
		}

		private void AddDescriptionPanel(UIElement container, float height, string tagGroup)
		{
			float num = 0f;
			UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight", (AssetRequestMode)1))
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f),
				Left = StyleDimension.FromPixels(0f - num),
				Height = StyleDimension.FromPixelsAndPercent(height, 0f),
				Top = StyleDimension.FromPixels(2f)
			};
			uISlicedImage.SetSliceDepths(10);
			uISlicedImage.Color = Color.LightGray * 0.7f;
			container.Append(uISlicedImage);
			UIText uIText = new UIText(Language.GetText("Workshop.InfoDescriptionDefault"), 0.85f)
			{
				HAlign = 0f,
				VAlign = 1f,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f)
			};
			uIText.PaddingLeft = 4f;
			uIText.PaddingRight = 4f;
			uIText.PaddingTop = 4f;
			uIText.IsWrapped = true;
			uISlicedImage.Append(uIText);
			_descriptionText = uIText;
		}

		protected abstract string GetPublishedObjectDisplayName();

		protected abstract List<WorkshopTagOption> GetTagsToShow();

		private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
		{
			HandleBackButtonUsage();
		}

		public void HandleBackButtonUsage()
		{
			if (_previousUIState == null)
			{
				Main.menuMode = 0;
				return;
			}
			Main.menuMode = 888;
			Main.MenuUI.SetState(_previousUIState);
		}

		private void Click_Publish(UIMouseEvent evt, UIElement listeningElement)
		{
			GoToPublishConfirmation();
		}

		protected abstract void GoToPublishConfirmation();

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

		private void AddPublishButton(int backButtonYLift, UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("Workshop.Publish"), 0.7f, large: true);
			uITextPanel.Width.Set(-10f, 0.5f);
			uITextPanel.Height.Set(50f, 0f);
			uITextPanel.VAlign = 1f;
			uITextPanel.Top.Set(-backButtonYLift, 0f);
			uITextPanel.HAlign = 1f;
			uITextPanel.OnMouseOver += FadedMouseOver;
			uITextPanel.OnMouseOut += FadedMouseOut;
			uITextPanel.OnClick += Click_Publish;
			uITextPanel.SetSnapPoint("publish", 0);
			outerContainer.Append(uITextPanel);
			_publishButton = uITextPanel;
		}

		private void AddBackButton(int backButtonYLift, UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
			uITextPanel.Width.Set(-10f, 0.5f);
			uITextPanel.Height.Set(50f, 0f);
			uITextPanel.VAlign = 1f;
			uITextPanel.Top.Set(-backButtonYLift, 0f);
			uITextPanel.HAlign = 0f;
			uITextPanel.OnMouseOver += FadedMouseOver;
			uITextPanel.OnMouseOut += FadedMouseOut;
			uITextPanel.OnClick += Click_GoBack;
			uITextPanel.SetSnapPoint("back", 0);
			outerContainer.Append(uITextPanel);
			_backButton = uITextPanel;
		}

		private UIElement AddHorizontalSeparator(UIElement Container, float accumualtedHeight, int widthReduction = 0)
		{
			UIHorizontalSeparator uIHorizontalSeparator = new UIHorizontalSeparator
			{
				Width = StyleDimension.FromPixelsAndPercent(-widthReduction, 1f),
				HAlign = 0.5f,
				Top = StyleDimension.FromPixels(accumualtedHeight - 8f),
				Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
			};
			Container.Append(uIHorizontalSeparator);
			return uIHorizontalSeparator;
		}

		protected WorkshopItemPublishSettings GetPublishSettings()
		{
			return new WorkshopItemPublishSettings
			{
				Publicity = _optionPublicity,
				UsedTags = (from x in _tagOptions
					where x.IsSelected
					select x.OptionValue).ToArray(),
				PreviewImagePath = _previewImagePath
			};
		}

		private void OpenFileDialogueToSelectPreviewImage()
		{
			ExtensionFilter[] extensions = new ExtensionFilter[1]
			{
				new ExtensionFilter("Image files", "png", "jpg", "jpeg")
			};
			string text = FileBrowser.OpenFilePanel("Open icon", extensions);
			if (text != null)
			{
				_previewImagePath = text;
				UpdateImagePreview();
			}
		}

		private string PrettifyPath(string path)
		{
			if (path == null)
			{
				return path;
			}
			char[] anyOf = new char[2]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			};
			int num = path.LastIndexOfAny(anyOf);
			if (num != -1)
			{
				path = path.Substring(num + 1);
			}
			if (path.Length > 30)
			{
				path = path.Substring(0, 30) + "…";
			}
			return path;
		}

		private void UpdateImagePreview()
		{
			Texture2D texture2D = null;
			string contents = PrettifyPath(_previewImagePath);
			_previewImagePathPlate.SetContents(contents);
			if (_previewImagePath != null)
			{
				try
				{
					FileStream stream = File.OpenRead(_previewImagePath);
					texture2D = Texture2D.FromStream(Main.graphics.GraphicsDevice, stream);
				}
				catch (Exception exception)
				{
					FancyErrorPrinter.ShowFailedToLoadAssetError(exception, _previewImagePath);
				}
			}
			if (texture2D != null && (texture2D.Width > 512 || texture2D.Height > 512))
			{
				object obj = new { texture2D.Width, texture2D.Height };
				string textValueWith = Language.GetTextValueWith("Workshop.ReportIssue_FailedToPublish_ImageSizeIsTooLarge", obj);
				if (SocialAPI.Workshop != null)
				{
					SocialAPI.Workshop.IssueReporter.ReportInstantUploadProblemFromValue(textValueWith);
				}
				_previewImagePath = null;
				_previewImagePathPlate.SetContents(null);
				_previewImageUIElement.SetImage(_defaultPreviewImageTexture);
			}
			else
			{
				if (_previewImageTransientTexture != null)
				{
					_previewImageTransientTexture.Dispose();
					_previewImageTransientTexture = null;
				}
				if (texture2D != null)
				{
					_previewImageUIElement.SetImage(texture2D);
					_previewImageTransientTexture = texture2D;
				}
				else
				{
					_previewImageUIElement.SetImage(_defaultPreviewImageTexture);
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			SetupGamepadPoints(spriteBatch);
		}

		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
			int num = 3000;
			int id = num;
			List<SnapPoint> snapPoints = GetSnapPoints();
			_helper.RemovePointsOutOfView(snapPoints, _listContainer, spriteBatch);
			UILinkPoint linkPoint = _helper.GetLinkPoint(id++, _backButton);
			UILinkPoint linkPoint2 = _helper.GetLinkPoint(id++, _publishButton);
			SnapPoint snap = null;
			SnapPoint snap2 = null;
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint = snapPoints[i];
				string name = snapPoint.Name;
				if (!(name == "disclaimer"))
				{
					if (name == "image")
					{
						snap2 = snapPoint;
					}
				}
				else
				{
					snap = snapPoint;
				}
			}
			UILinkPoint upSide = _helper.TryMakeLinkPoint(ref id, snap);
			UILinkPoint uILinkPoint = _helper.TryMakeLinkPoint(ref id, snap2);
			_helper.PairLeftRight(linkPoint, linkPoint2);
			_helper.PairUpDown(upSide, uILinkPoint);
			UILinkPoint[] array = _helper.CreateUILinkStripHorizontal(ref id, snapPoints.Where((SnapPoint x) => x.Name == "public").ToList());
			if (array.Length != 0)
			{
				_helper.LinkHorizontalStripUpSideToSingle(array, uILinkPoint);
			}
			UILinkPoint topLinkPoint = ((array.Length != 0) ? array[0] : null);
			UILinkPoint bottomLinkPoint = linkPoint;
			List<SnapPoint> pointsForGrid = snapPoints.Where((SnapPoint x) => x.Name == "tags").ToList();
			UILinkPoint[,] array2 = _helper.CreateUILinkPointGrid(ref id, pointsForGrid, 3, topLinkPoint, null, null, bottomLinkPoint);
			int num2 = array2.GetLength(1) - 1;
			if (num2 >= 0)
			{
				_helper.LinkHorizontalStripBottomSideToSingle(array, array2[0, 0]);
				for (int num3 = array2.GetLength(0) - 1; num3 >= 0; num3--)
				{
					if (array2[num3, num2] != null)
					{
						_helper.PairUpDown(array2[num3, num2], linkPoint2);
						break;
					}
				}
			}
			UILinkPoint upSide2 = UILinkPointNavigator.Points[id - 1];
			_helper.PairUpDown(upSide2, linkPoint);
			_helper.MoveToVisuallyClosestPoint(num, id);
		}
	}
}
