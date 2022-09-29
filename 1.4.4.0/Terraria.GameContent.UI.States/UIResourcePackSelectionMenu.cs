using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	public class UIResourcePackSelectionMenu : UIState, IHaveBackButtonCommand
	{
		private readonly AssetSourceController _sourceController;

		private UIList _availablePacksList;

		private UIList _enabledPacksList;

		private ResourcePackList _packsList;

		private UIText _titleAvailable;

		private UIText _titleEnabled;

		private UIState _uiStateToGoBackTo;

		private const string _snapCategory_ToggleFromOffToOn = "ToggleToOn";

		private const string _snapCategory_ToggleFromOnToOff = "ToggleToOff";

		private const string _snapCategory_InfoWhenOff = "InfoOff";

		private const string _snapCategory_InfoWhenOn = "InfoOn";

		private const string _snapCategory_OffsetOrderUp = "OrderUp";

		private const string _snapCategory_OffsetOrderDown = "OrderDown";

		private const string _snapPointName_goBack = "GoBack";

		private const string _snapPointName_openFolder = "OpenFolder";

		private UIGamepadHelper _helper;

		public UIResourcePackSelectionMenu(UIState uiStateToGoBackTo, AssetSourceController sourceController, ResourcePackList currentResourcePackList)
		{
			_sourceController = sourceController;
			_uiStateToGoBackTo = uiStateToGoBackTo;
			BuildPage();
			_packsList = currentResourcePackList;
			PopulatePackList();
		}

		private void PopulatePackList()
		{
			_availablePacksList.Clear();
			_enabledPacksList.Clear();
			CleanUpResourcePackPriority();
			IEnumerable<ResourcePack> enabledPacks = _packsList.EnabledPacks;
			IEnumerable<ResourcePack> disabledPacks = _packsList.DisabledPacks;
			int num = 0;
			foreach (ResourcePack item in disabledPacks)
			{
				UIResourcePack uIResourcePack = new UIResourcePack(item, num)
				{
					Width = StyleDimension.FromPixelsAndPercent(0f, 1f)
				};
				UIElement uIElement = CreatePackToggleButton(item);
				uIElement.OnUpdate += EnablePackUpdate;
				uIElement.SetSnapPoint("ToggleToOn", num);
				uIResourcePack.ContentPanel.Append(uIElement);
				uIElement = CreatePackInfoButton(item);
				uIElement.OnUpdate += SeeInfoUpdate;
				uIElement.SetSnapPoint("InfoOff", num);
				uIResourcePack.ContentPanel.Append(uIElement);
				_availablePacksList.Add(uIResourcePack);
				num++;
			}
			num = 0;
			foreach (ResourcePack item2 in enabledPacks)
			{
				UIResourcePack uIResourcePack2 = new UIResourcePack(item2, num)
				{
					Width = StyleDimension.FromPixelsAndPercent(0f, 1f)
				};
				if (item2.IsEnabled)
				{
					UIElement uIElement2 = CreatePackToggleButton(item2);
					uIElement2.Left = new StyleDimension(0f, 0f);
					uIElement2.Width = new StyleDimension(0f, 0.5f);
					uIElement2.OnUpdate += DisablePackUpdate;
					uIElement2.SetSnapPoint("ToggleToOff", num);
					uIResourcePack2.ContentPanel.Append(uIElement2);
					uIElement2 = CreatePackInfoButton(item2);
					uIElement2.OnUpdate += SeeInfoUpdate;
					uIElement2.Left = new StyleDimension(0f, 0.5f);
					uIElement2.Width = new StyleDimension(0f, 1f / 6f);
					uIElement2.SetSnapPoint("InfoOn", num);
					uIResourcePack2.ContentPanel.Append(uIElement2);
					uIElement2 = CreateOffsetButton(item2, -1);
					uIElement2.Left = new StyleDimension(0f, 2f / 3f);
					uIElement2.Width = new StyleDimension(0f, 1f / 6f);
					uIElement2.SetSnapPoint("OrderUp", num);
					uIResourcePack2.ContentPanel.Append(uIElement2);
					uIElement2 = CreateOffsetButton(item2, 1);
					uIElement2.Left = new StyleDimension(0f, 0.8333334f);
					uIElement2.Width = new StyleDimension(0f, 1f / 6f);
					uIElement2.SetSnapPoint("OrderDown", num);
					uIResourcePack2.ContentPanel.Append(uIElement2);
				}
				_enabledPacksList.Add(uIResourcePack2);
				num++;
			}
			UpdateTitles();
		}

		private UIElement CreateOffsetButton(ResourcePack resourcePack, int offset)
		{
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, null, Color.White, null, 0.8f)
			{
				Left = StyleDimension.FromPercent(0.5f),
				Width = StyleDimension.FromPixelsAndPercent(0f, 0.5f),
				Height = StyleDimension.Fill
			};
			bool num = (offset == -1 && resourcePack.SortingOrder == 0) | (offset == 1 && resourcePack.SortingOrder == _packsList.EnabledPacks.Count() - 1);
			Color lightCyan = Color.LightCyan;
			groupOptionButton.SetColorsBasedOnSelectionState(lightCyan, lightCyan, 0.7f, 0.7f);
			groupOptionButton.ShowHighlightWhenSelected = false;
			groupOptionButton.SetPadding(0f);
			Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/TexturePackButtons", (AssetRequestMode)1);
			UIImageFramed element = new UIImageFramed(obj, obj.Frame(2, 2, (offset == 1) ? 1 : 0))
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				IgnoresMouseInteraction = true
			};
			groupOptionButton.Append(element);
			groupOptionButton.OnMouseOver += delegate
			{
				SoundEngine.PlaySound(12);
			};
			int offsetLocalForLambda = offset;
			if (num)
			{
				groupOptionButton.OnClick += delegate
				{
					SoundEngine.PlaySound(12);
				};
			}
			else
			{
				groupOptionButton.OnClick += delegate
				{
					SoundEngine.PlaySound(12);
					OffsetResourcePackPriority(resourcePack, offsetLocalForLambda);
					PopulatePackList();
					Main.instance.ResetAllContentBasedRenderTargets();
				};
			}
			if (offset == 1)
			{
				groupOptionButton.OnUpdate += OffsetFrontwardUpdate;
			}
			else
			{
				groupOptionButton.OnUpdate += OffsetBackwardUpdate;
			}
			return groupOptionButton;
		}

		private UIElement CreatePackToggleButton(ResourcePack resourcePack)
		{
			Language.GetText(resourcePack.IsEnabled ? "GameUI.Enabled" : "GameUI.Disabled");
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, null, Color.White, null, 0.8f);
			groupOptionButton.Left = StyleDimension.FromPercent(0.5f);
			groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(0f, 0.5f);
			groupOptionButton.Height = StyleDimension.Fill;
			groupOptionButton.SetColorsBasedOnSelectionState(Color.LightGreen, Color.PaleVioletRed, 0.7f, 0.7f);
			groupOptionButton.SetCurrentOption(resourcePack.IsEnabled);
			groupOptionButton.ShowHighlightWhenSelected = false;
			groupOptionButton.SetPadding(0f);
			Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/TexturePackButtons", (AssetRequestMode)1);
			UIImageFramed element = new UIImageFramed(obj, obj.Frame(2, 2, (!resourcePack.IsEnabled) ? 1 : 0, 1))
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				IgnoresMouseInteraction = true
			};
			groupOptionButton.Append(element);
			groupOptionButton.OnMouseOver += delegate
			{
				SoundEngine.PlaySound(12);
			};
			groupOptionButton.OnClick += delegate
			{
				SoundEngine.PlaySound(12);
				resourcePack.IsEnabled = !resourcePack.IsEnabled;
				SetResourcePackAsTopPriority(resourcePack);
				PopulatePackList();
				Main.instance.ResetAllContentBasedRenderTargets();
			};
			return groupOptionButton;
		}

		private void SetResourcePackAsTopPriority(ResourcePack resourcePack)
		{
			if (!resourcePack.IsEnabled)
			{
				return;
			}
			int num = -1;
			foreach (ResourcePack enabledPack in _packsList.EnabledPacks)
			{
				if (num < enabledPack.SortingOrder && enabledPack != resourcePack)
				{
					num = enabledPack.SortingOrder;
				}
			}
			resourcePack.SortingOrder = num + 1;
		}

		private void OffsetResourcePackPriority(ResourcePack resourcePack, int offset)
		{
			if (resourcePack.IsEnabled)
			{
				List<ResourcePack> list = _packsList.EnabledPacks.ToList();
				int num = list.IndexOf(resourcePack);
				int num2 = Utils.Clamp(num + offset, 0, list.Count - 1);
				if (num2 != num)
				{
					int sortingOrder = list[num].SortingOrder;
					list[num].SortingOrder = list[num2].SortingOrder;
					list[num2].SortingOrder = sortingOrder;
				}
			}
		}

		private UIElement CreatePackInfoButton(ResourcePack resourcePack)
		{
			UIResourcePackInfoButton<string> uIResourcePackInfoButton = new UIResourcePackInfoButton<string>("", 0.8f);
			uIResourcePackInfoButton.Width = StyleDimension.FromPixelsAndPercent(0f, 0.5f);
			uIResourcePackInfoButton.Height = StyleDimension.Fill;
			uIResourcePackInfoButton.ResourcePack = resourcePack;
			uIResourcePackInfoButton.SetPadding(0f);
			UIImage element = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CharInfo", (AssetRequestMode)1))
			{
				HAlign = 0.5f,
				VAlign = 0.5f,
				IgnoresMouseInteraction = true
			};
			uIResourcePackInfoButton.Append(element);
			uIResourcePackInfoButton.OnMouseOver += delegate
			{
				SoundEngine.PlaySound(12);
			};
			uIResourcePackInfoButton.OnClick += Click_Info;
			return uIResourcePackInfoButton;
		}

		private void Click_Info(UIMouseEvent evt, UIElement listeningElement)
		{
			if (listeningElement is UIResourcePackInfoButton<string> uIResourcePackInfoButton)
			{
				SoundEngine.PlaySound(10);
				Main.MenuUI.SetState(new UIResourcePackInfoMenu(this, uIResourcePackInfoButton.ResourcePack));
			}
		}

		private void ApplyListChanges()
		{
			_sourceController.UseResourcePacks(new ResourcePackList(_enabledPacksList.Select((UIElement uiPack) => ((UIResourcePack)uiPack).ResourcePack)));
		}

		private void CleanUpResourcePackPriority()
		{
			IOrderedEnumerable<ResourcePack> orderedEnumerable = _packsList.EnabledPacks.OrderBy((ResourcePack pack) => pack.SortingOrder);
			int num = 0;
			foreach (ResourcePack item in orderedEnumerable)
			{
				item.SortingOrder = num++;
			}
		}

		private void BuildPage()
		{
			RemoveAllChildren();
			UIElement uIElement = new UIElement();
			uIElement.Width.Set(0f, 0.8f);
			uIElement.MaxWidth.Set(800f, 0f);
			uIElement.MinWidth.Set(600f, 0f);
			uIElement.Top.Set(240f, 0f);
			uIElement.Height.Set(-240f, 1f);
			uIElement.HAlign = 0.5f;
			Append(uIElement);
			UIPanel uIPanel = new UIPanel
			{
				Width = StyleDimension.Fill,
				Height = new StyleDimension(-110f, 1f),
				BackgroundColor = new Color(33, 43, 79) * 0.8f,
				PaddingRight = 0f,
				PaddingLeft = 0f
			};
			uIElement.Append(uIPanel);
			int num = 35;
			int num2 = num;
			int num3 = 30;
			UIElement uIElement2 = new UIElement
			{
				Width = StyleDimension.Fill,
				Height = StyleDimension.FromPixelsAndPercent(-(num3 + 4 + 5), 1f),
				VAlign = 1f
			};
			uIElement2.SetPadding(0f);
			uIPanel.Append(uIElement2);
			UIElement uIElement3 = new UIElement
			{
				Width = new StyleDimension(-20f, 0.5f),
				Height = new StyleDimension(0f, 1f),
				Left = new StyleDimension(10f, 0f)
			};
			uIElement3.SetPadding(0f);
			uIElement2.Append(uIElement3);
			UIElement uIElement4 = new UIElement
			{
				Width = new StyleDimension(-20f, 0.5f),
				Height = new StyleDimension(0f, 1f),
				Left = new StyleDimension(-10f, 0f),
				HAlign = 1f
			};
			uIElement4.SetPadding(0f);
			uIElement2.Append(uIElement4);
			UIList uIList = new UIList
			{
				Width = new StyleDimension(-25f, 1f),
				Height = new StyleDimension(0f, 1f),
				ListPadding = 5f,
				HAlign = 1f
			};
			uIElement3.Append(uIList);
			_availablePacksList = uIList;
			UIList uIList2 = new UIList
			{
				Width = new StyleDimension(-25f, 1f),
				Height = new StyleDimension(0f, 1f),
				ListPadding = 5f,
				HAlign = 0f,
				Left = new StyleDimension(0f, 0f)
			};
			uIElement4.Append(uIList2);
			_enabledPacksList = uIList2;
			uIPanel.Append(_titleAvailable = new UIText(Language.GetText("UI.AvailableResourcePacksTitle"))
			{
				HAlign = 0f,
				Left = new StyleDimension(25f, 0f),
				Width = new StyleDimension(-25f, 0.5f),
				VAlign = 0f,
				Top = new StyleDimension(10f, 0f)
			});
			uIPanel.Append(_titleEnabled = new UIText(Language.GetText("UI.EnabledResourcePacksTitle"))
			{
				HAlign = 1f,
				Left = new StyleDimension(-25f, 0f),
				Width = new StyleDimension(-25f, 0.5f),
				VAlign = 0f,
				Top = new StyleDimension(10f, 0f)
			});
			UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.ResourcePacks"), 1f, large: true)
			{
				HAlign = 0.5f,
				VAlign = 0f,
				Top = new StyleDimension(-44f, 0f),
				BackgroundColor = new Color(73, 94, 171)
			};
			uITextPanel.SetPadding(13f);
			uIElement.Append(uITextPanel);
			UIScrollbar uIScrollbar = new UIScrollbar
			{
				Height = new StyleDimension(0f, 1f),
				HAlign = 0f,
				Left = new StyleDimension(0f, 0f)
			};
			uIElement3.Append(uIScrollbar);
			_availablePacksList.SetScrollbar(uIScrollbar);
			UIVerticalSeparator element = new UIVerticalSeparator
			{
				Height = new StyleDimension(-12f, 1f),
				HAlign = 0.5f,
				VAlign = 1f,
				Color = new Color(89, 116, 213, 255) * 0.9f
			};
			uIPanel.Append(element);
			new UIHorizontalSeparator
			{
				Width = new StyleDimension(-num2, 0.5f),
				VAlign = 0f,
				HAlign = 0f,
				Color = new Color(89, 116, 213, 255) * 0.9f,
				Top = new StyleDimension(num3, 0f),
				Left = new StyleDimension(num, 0f)
			};
			new UIHorizontalSeparator
			{
				Width = new StyleDimension(-num2, 0.5f),
				VAlign = 0f,
				HAlign = 1f,
				Color = new Color(89, 116, 213, 255) * 0.9f,
				Top = new StyleDimension(num3, 0f),
				Left = new StyleDimension(-num, 0f)
			};
			UIScrollbar uIScrollbar2 = new UIScrollbar
			{
				Height = new StyleDimension(0f, 1f),
				HAlign = 1f
			};
			uIElement4.Append(uIScrollbar2);
			_enabledPacksList.SetScrollbar(uIScrollbar2);
			AddBackAndFolderButtons(uIElement);
		}

		private void UpdateTitles()
		{
			_titleAvailable.SetText(Language.GetText("UI.AvailableResourcePacksTitle").FormatWith(new
			{
				Amount = _availablePacksList.Count
			}));
			_titleEnabled.SetText(Language.GetText("UI.EnabledResourcePacksTitle").FormatWith(new
			{
				Amount = _enabledPacksList.Count
			}));
		}

		private void AddBackAndFolderButtons(UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true)
			{
				Width = new StyleDimension(-8f, 0.5f),
				Height = new StyleDimension(50f, 0f),
				VAlign = 1f,
				HAlign = 0f,
				Top = new StyleDimension(-45f, 0f)
			};
			uITextPanel.OnMouseOver += FadedMouseOver;
			uITextPanel.OnMouseOut += FadedMouseOut;
			uITextPanel.OnClick += GoBackClick;
			uITextPanel.SetSnapPoint("GoBack", 0);
			outerContainer.Append(uITextPanel);
			UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("GameUI.OpenFileFolder"), 0.7f, large: true)
			{
				Width = new StyleDimension(-8f, 0.5f),
				Height = new StyleDimension(50f, 0f),
				VAlign = 1f,
				HAlign = 1f,
				Top = new StyleDimension(-45f, 0f)
			};
			uITextPanel2.OnMouseOver += FadedMouseOver;
			uITextPanel2.OnMouseOut += FadedMouseOut;
			uITextPanel2.OnClick += OpenFoldersClick;
			uITextPanel2.SetSnapPoint("OpenFolder", 0);
			outerContainer.Append(uITextPanel2);
		}

		private void OpenFoldersClick(UIMouseEvent evt, UIElement listeningElement)
		{
			AssetInitializer.GetResourcePacksFolderPathAndConfirmItExists(out var _, out var resourcePackFolder);
			SoundEngine.PlaySound(12);
			Utils.OpenFolder(resourcePackFolder);
		}

		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			HandleBackButtonUsage();
		}

		public void HandleBackButtonUsage()
		{
			SoundEngine.PlaySound(11);
			ApplyListChanges();
			Main.SaveSettings();
			if (_uiStateToGoBackTo != null)
			{
				Main.MenuUI.SetState(_uiStateToGoBackTo);
				return;
			}
			Main.menuMode = 0;
			IngameFancyUI.Close();
		}

		private static void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		private static void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		private void OffsetBackwardUpdate(UIElement affectedElement)
		{
			DisplayMouseTextIfHovered(affectedElement, "UI.OffsetTexturePackPriorityDown");
		}

		private void OffsetFrontwardUpdate(UIElement affectedElement)
		{
			DisplayMouseTextIfHovered(affectedElement, "UI.OffsetTexturePackPriorityUp");
		}

		private void EnablePackUpdate(UIElement affectedElement)
		{
			DisplayMouseTextIfHovered(affectedElement, "UI.EnableTexturePack");
		}

		private void DisablePackUpdate(UIElement affectedElement)
		{
			DisplayMouseTextIfHovered(affectedElement, "UI.DisableTexturePack");
		}

		private void SeeInfoUpdate(UIElement affectedElement)
		{
			DisplayMouseTextIfHovered(affectedElement, "UI.SeeTexturePackInfo");
		}

		private static void DisplayMouseTextIfHovered(UIElement affectedElement, string textKey)
		{
			if (affectedElement.IsMouseHovering)
			{
				string textValue = Language.GetTextValue(textKey);
				Main.instance.MouseText(textValue, 0, 0);
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
			int currentID = num;
			List<SnapPoint> snapPoints = GetSnapPoints();
			List<SnapPoint> snapPoints2 = _availablePacksList.GetSnapPoints();
			_helper.CullPointsOutOfElementArea(spriteBatch, snapPoints2, _availablePacksList);
			List<SnapPoint> snapPoints3 = _enabledPacksList.GetSnapPoints();
			_helper.CullPointsOutOfElementArea(spriteBatch, snapPoints3, _enabledPacksList);
			UILinkPoint[] verticalStripFromCategoryName = _helper.GetVerticalStripFromCategoryName(ref currentID, snapPoints2, "ToggleToOn");
			UILinkPoint[] verticalStripFromCategoryName2 = _helper.GetVerticalStripFromCategoryName(ref currentID, snapPoints2, "InfoOff");
			UILinkPoint[] verticalStripFromCategoryName3 = _helper.GetVerticalStripFromCategoryName(ref currentID, snapPoints3, "ToggleToOff");
			UILinkPoint[] verticalStripFromCategoryName4 = _helper.GetVerticalStripFromCategoryName(ref currentID, snapPoints3, "InfoOn");
			UILinkPoint[] verticalStripFromCategoryName5 = _helper.GetVerticalStripFromCategoryName(ref currentID, snapPoints3, "OrderUp");
			UILinkPoint[] verticalStripFromCategoryName6 = _helper.GetVerticalStripFromCategoryName(ref currentID, snapPoints3, "OrderDown");
			UILinkPoint uILinkPoint = null;
			UILinkPoint uILinkPoint2 = null;
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint = snapPoints[i];
				string name = snapPoint.Name;
				if (!(name == "GoBack"))
				{
					if (name == "OpenFolder")
					{
						uILinkPoint2 = _helper.MakeLinkPointFromSnapPoint(currentID++, snapPoint);
					}
				}
				else
				{
					uILinkPoint = _helper.MakeLinkPointFromSnapPoint(currentID++, snapPoint);
				}
			}
			_helper.LinkVerticalStrips(verticalStripFromCategoryName2, verticalStripFromCategoryName, 0);
			_helper.LinkVerticalStrips(verticalStripFromCategoryName, verticalStripFromCategoryName3, 0);
			_helper.LinkVerticalStrips(verticalStripFromCategoryName3, verticalStripFromCategoryName4, 0);
			_helper.LinkVerticalStrips(verticalStripFromCategoryName4, verticalStripFromCategoryName5, 0);
			_helper.LinkVerticalStrips(verticalStripFromCategoryName5, verticalStripFromCategoryName6, 0);
			_helper.LinkVerticalStripBottomSideToSingle(verticalStripFromCategoryName, uILinkPoint);
			_helper.LinkVerticalStripBottomSideToSingle(verticalStripFromCategoryName2, uILinkPoint);
			_helper.LinkVerticalStripBottomSideToSingle(verticalStripFromCategoryName5, uILinkPoint2);
			_helper.LinkVerticalStripBottomSideToSingle(verticalStripFromCategoryName6, uILinkPoint2);
			_helper.LinkVerticalStripBottomSideToSingle(verticalStripFromCategoryName3, uILinkPoint2);
			_helper.LinkVerticalStripBottomSideToSingle(verticalStripFromCategoryName4, uILinkPoint2);
			_helper.PairLeftRight(uILinkPoint, uILinkPoint2);
			_helper.MoveToVisuallyClosestPoint(num, currentID);
		}
	}
}
