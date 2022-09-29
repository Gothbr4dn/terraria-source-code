using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	public class UIBestiaryTest : UIState
	{
		private UIElement _bestiarySpace;

		private UIBestiaryEntryInfoPage _infoSpace;

		private UIBestiaryEntryButton _selectedEntryButton;

		private List<BestiaryEntry> _originalEntriesList;

		private List<BestiaryEntry> _workingSetEntries;

		private UIText _indexesRangeText;

		private EntryFilterer<BestiaryEntry, IBestiaryEntryFilter> _filterer = new EntryFilterer<BestiaryEntry, IBestiaryEntryFilter>();

		private EntrySorter<BestiaryEntry, IBestiarySortStep> _sorter = new EntrySorter<BestiaryEntry, IBestiarySortStep>();

		private UIBestiaryEntryGrid _entryGrid;

		private UIBestiarySortingOptionsGrid _sortingGrid;

		private UIBestiaryFilteringOptionsGrid _filteringGrid;

		private UISearchBar _searchBar;

		private UIPanel _searchBoxPanel;

		private UIText _sortingText;

		private UIText _filteringText;

		private string _searchString;

		private BestiaryUnlockProgressReport _progressReport;

		private UIText _progressPercentText;

		private UIColoredSliderSimple _unlocksProgressBar;

		private bool _didClickSomething;

		private bool _didClickSearchBar;

		public UIBestiaryTest(BestiaryDatabase database)
		{
			_filterer.SetSearchFilterObject(new Filters.BySearch());
			_originalEntriesList = new List<BestiaryEntry>(database.Entries);
			_workingSetEntries = new List<BestiaryEntry>(_originalEntriesList);
			_filterer.AddFilters(database.Filters);
			_sorter.AddSortSteps(database.SortSteps);
			BuildPage();
		}

		public void OnOpenPage()
		{
			UpdateBestiaryContents();
		}

		private void BuildPage()
		{
			RemoveAllChildren();
			int num = Utils.ToInt(value: true) * 100;
			UIElement uIElement = new UIElement();
			uIElement.Width.Set(0f, 0.875f);
			uIElement.MaxWidth.Set(800f + (float)num, 0f);
			uIElement.MinWidth.Set(600f + (float)num, 0f);
			uIElement.Top.Set(220f, 0f);
			uIElement.Height.Set(-220f, 1f);
			uIElement.HAlign = 0.5f;
			Append(uIElement);
			MakeExitButton(uIElement);
			UIPanel uIPanel = new UIPanel();
			uIPanel.Width.Set(0f, 1f);
			uIPanel.Height.Set(-90f, 1f);
			uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			uIElement.Append(uIPanel);
			uIPanel.PaddingTop -= 4f;
			uIPanel.PaddingBottom -= 4f;
			int num2 = 24;
			UIElement uIElement2 = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(num2, 0f),
				VAlign = 0f
			};
			uIElement2.SetPadding(0f);
			uIPanel.Append(uIElement2);
			UIBestiaryEntryInfoPage uIBestiaryEntryInfoPage = new UIBestiaryEntryInfoPage
			{
				Height = new StyleDimension(12f, 1f),
				HAlign = 1f
			};
			AddSortAndFilterButtons(uIElement2, uIBestiaryEntryInfoPage);
			AddSearchBar(uIElement2, uIBestiaryEntryInfoPage);
			int num3 = 20;
			UIElement uIElement3 = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(-num2 - 6 - num3, 1f),
				VAlign = 1f,
				Top = new StyleDimension(-num3, 0f)
			};
			uIElement3.SetPadding(0f);
			uIPanel.Append(uIElement3);
			UIElement uIElement4 = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(20f, 0f),
				VAlign = 1f
			};
			uIPanel.Append(uIElement4);
			uIElement4.SetPadding(0f);
			FillProgressBottomBar(uIElement4);
			UIElement uIElement5 = new UIElement
			{
				Width = new StyleDimension(-12f - uIBestiaryEntryInfoPage.Width.Pixels, 1f),
				Height = new StyleDimension(-4f, 1f),
				VAlign = 1f
			};
			uIElement3.Append(uIElement5);
			uIElement5.SetPadding(0f);
			_bestiarySpace = uIElement5;
			UIBestiaryEntryGrid uIBestiaryEntryGrid = new UIBestiaryEntryGrid(_workingSetEntries, Click_SelectEntryButton);
			uIElement5.Append(uIBestiaryEntryGrid);
			_entryGrid = uIBestiaryEntryGrid;
			_entryGrid.OnGridContentsChanged += UpdateBestiaryGridRange;
			uIElement3.Append(uIBestiaryEntryInfoPage);
			_infoSpace = uIBestiaryEntryInfoPage;
			AddBackAndForwardButtons(uIElement2);
			_sortingGrid = new UIBestiarySortingOptionsGrid(_sorter);
			_sortingGrid.OnClick += Click_CloseSortingGrid;
			_sortingGrid.OnClickingOption += UpdateBestiaryContents;
			_filteringGrid = new UIBestiaryFilteringOptionsGrid(_filterer);
			_filteringGrid.OnClick += Click_CloseFilteringGrid;
			_filteringGrid.OnClickingOption += UpdateBestiaryContents;
			_filteringGrid.SetupAvailabilityTest(_originalEntriesList);
			_searchBar.SetContents(null, forced: true);
			UpdateBestiaryContents();
		}

		private void FillProgressBottomBar(UIElement container)
		{
			UIText uIText = (_progressPercentText = new UIText("", 0.8f)
			{
				HAlign = 0f,
				VAlign = 1f,
				TextOriginX = 0f,
				TextOriginY = 0f
			});
			UIColoredSliderSimple uIColoredSliderSimple = new UIColoredSliderSimple
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(15f, 0f),
				VAlign = 1f,
				FilledColor = new Color(51, 137, 255),
				EmptyColor = new Color(35, 43, 81),
				FillPercent = 0f
			};
			uIColoredSliderSimple.OnUpdate += ShowStats_Completion;
			_unlocksProgressBar = uIColoredSliderSimple;
			container.Append(uIColoredSliderSimple);
		}

		private void ShowStats_Completion(UIElement element)
		{
			if (element.IsMouseHovering)
			{
				string completionPercentText = GetCompletionPercentText();
				Main.instance.MouseText(completionPercentText, 0, 0);
			}
		}

		private string GetCompletionPercentText()
		{
			string percent = Utils.PrettifyPercentDisplay(GetProgressPercent(), "P2");
			return Language.GetTextValueWith("BestiaryInfo.PercentCollected", new
			{
				Percent = percent
			});
		}

		private float GetProgressPercent()
		{
			return _progressReport.CompletionPercent;
		}

		private void EmptyInteraction(float input)
		{
		}

		private void EmptyInteraction2()
		{
		}

		private Color GetColorAtBlip(float percentile)
		{
			if (percentile < GetProgressPercent())
			{
				return new Color(51, 137, 255);
			}
			return new Color(35, 40, 83);
		}

		private void AddBackAndForwardButtons(UIElement innerTopContainer)
		{
			UIImageButton uIImageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Back", (AssetRequestMode)1));
			uIImageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border", (AssetRequestMode)1));
			uIImageButton.SetVisibility(1f, 1f);
			uIImageButton.SetSnapPoint("BackPage", 0);
			_entryGrid.MakeButtonGoByOffset(uIImageButton, -1);
			innerTopContainer.Append(uIImageButton);
			UIImageButton uIImageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Forward", (AssetRequestMode)1))
			{
				Left = new StyleDimension(uIImageButton.Width.Pixels + 1f, 0f)
			};
			uIImageButton2.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Border", (AssetRequestMode)1));
			uIImageButton2.SetVisibility(1f, 1f);
			uIImageButton2.SetSnapPoint("NextPage", 0);
			_entryGrid.MakeButtonGoByOffset(uIImageButton2, 1);
			innerTopContainer.Append(uIImageButton2);
			UIPanel uIPanel = new UIPanel
			{
				Left = new StyleDimension(uIImageButton.Width.Pixels + 1f + uIImageButton2.Width.Pixels + 3f, 0f),
				Width = new StyleDimension(135f, 0f),
				Height = new StyleDimension(0f, 1f),
				VAlign = 0.5f
			};
			uIPanel.BackgroundColor = new Color(35, 40, 83);
			uIPanel.BorderColor = new Color(35, 40, 83);
			uIPanel.SetPadding(0f);
			innerTopContainer.Append(uIPanel);
			UIText uIText = new UIText("9000-9999 (9001)", 0.8f)
			{
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			uIPanel.Append(uIText);
			_indexesRangeText = uIText;
		}

		private void AddSortAndFilterButtons(UIElement innerTopContainer, UIBestiaryEntryInfoPage infoSpace)
		{
			int num = 17;
			UIImageButton uIImageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Filtering", (AssetRequestMode)1))
			{
				Left = new StyleDimension(0f - infoSpace.Width.Pixels - (float)num, 0f),
				HAlign = 1f
			};
			uIImageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Wide_Border", (AssetRequestMode)1));
			uIImageButton.SetVisibility(1f, 1f);
			uIImageButton.SetSnapPoint("FilterButton", 0);
			uIImageButton.OnClick += OpenOrCloseFilteringGrid;
			innerTopContainer.Append(uIImageButton);
			UIText uIText = new UIText("", 0.8f)
			{
				Left = new StyleDimension(34f, 0f),
				Top = new StyleDimension(2f, 0f),
				VAlign = 0.5f,
				TextOriginX = 0f,
				TextOriginY = 0f
			};
			uIImageButton.Append(uIText);
			_filteringText = uIText;
			UIImageButton uIImageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Sorting", (AssetRequestMode)1))
			{
				Left = new StyleDimension(0f - infoSpace.Width.Pixels - uIImageButton.Width.Pixels - 3f - (float)num, 0f),
				HAlign = 1f
			};
			uIImageButton2.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Wide_Border", (AssetRequestMode)1));
			uIImageButton2.SetVisibility(1f, 1f);
			uIImageButton2.SetSnapPoint("SortButton", 0);
			uIImageButton2.OnClick += OpenOrCloseSortingOptions;
			innerTopContainer.Append(uIImageButton2);
			UIText uIText2 = new UIText("", 0.8f)
			{
				Left = new StyleDimension(34f, 0f),
				Top = new StyleDimension(2f, 0f),
				VAlign = 0.5f,
				TextOriginX = 0f,
				TextOriginY = 0f
			};
			uIImageButton2.Append(uIText2);
			_sortingText = uIText2;
		}

		private void AddSearchBar(UIElement innerTopContainer, UIBestiaryEntryInfoPage infoSpace)
		{
			UIImageButton uIImageButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search", (AssetRequestMode)1))
			{
				Left = new StyleDimension(0f - infoSpace.Width.Pixels, 1f),
				VAlign = 0.5f
			};
			uIImageButton.OnClick += Click_SearchArea;
			uIImageButton.SetHoverImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Button_Search_Border", (AssetRequestMode)1));
			uIImageButton.SetVisibility(1f, 1f);
			uIImageButton.SetSnapPoint("SearchButton", 0);
			innerTopContainer.Append(uIImageButton);
			UIPanel uIPanel = (_searchBoxPanel = new UIPanel
			{
				Left = new StyleDimension(0f - infoSpace.Width.Pixels + uIImageButton.Width.Pixels + 3f, 1f),
				Width = new StyleDimension(infoSpace.Width.Pixels - uIImageButton.Width.Pixels - 3f, 0f),
				Height = new StyleDimension(0f, 1f),
				VAlign = 0.5f
			});
			uIPanel.BackgroundColor = new Color(35, 40, 83);
			uIPanel.BorderColor = new Color(35, 40, 83);
			uIPanel.SetPadding(0f);
			innerTopContainer.Append(uIPanel);
			UISearchBar uISearchBar = (_searchBar = new UISearchBar(Language.GetText("UI.PlayerNameSlot"), 0.8f)
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(0f, 1f),
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(0f, 0f),
				IgnoresMouseInteraction = true
			});
			uIPanel.OnClick += Click_SearchArea;
			uISearchBar.OnContentsChanged += OnSearchContentsChanged;
			uIPanel.Append(uISearchBar);
			uISearchBar.OnStartTakingInput += OnStartTakingInput;
			uISearchBar.OnEndTakingInput += OnEndTakingInput;
			uISearchBar.OnNeedingVirtualKeyboard += OpenVirtualKeyboardWhenNeeded;
			UIImageButton uIImageButton2 = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel", (AssetRequestMode)1))
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension(-2f, 0f)
			};
			uIImageButton2.OnMouseOver += searchCancelButton_OnMouseOver;
			uIImageButton2.OnClick += searchCancelButton_OnClick;
			uIPanel.Append(uIImageButton2);
		}

		private void searchCancelButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (_searchBar.HasContents)
			{
				_searchBar.SetContents(null, forced: true);
				SoundEngine.PlaySound(11);
			}
			else
			{
				SoundEngine.PlaySound(12);
			}
		}

		private void searchCancelButton_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12);
		}

		private void OpenVirtualKeyboardWhenNeeded()
		{
			int maxInputLength = 40;
			UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Language.GetText("UI.PlayerNameSlot").Value, _searchString, OnFinishedSettingName, GoBackHere, 0, allowEmpty: true);
			uIVirtualKeyboard.SetMaxInputLength(maxInputLength);
			UserInterface.ActiveInstance.SetState(uIVirtualKeyboard);
		}

		private void OnFinishedSettingName(string name)
		{
			string contents = name.Trim();
			_searchBar.SetContents(contents);
			GoBackHere();
		}

		private void GoBackHere()
		{
			UserInterface.ActiveInstance.SetState(this);
			_searchBar.ToggleTakingText();
		}

		private void OnStartTakingInput()
		{
			_searchBoxPanel.BorderColor = Main.OurFavoriteColor;
		}

		private void OnEndTakingInput()
		{
			_searchBoxPanel.BorderColor = new Color(35, 40, 83);
		}

		private void OnSearchContentsChanged(string contents)
		{
			_searchString = contents;
			_filterer.SetSearchFilter(contents);
			UpdateBestiaryContents();
		}

		private void Click_SearchArea(UIMouseEvent evt, UIElement listeningElement)
		{
			if (evt.Target.Parent != _searchBoxPanel)
			{
				_searchBar.ToggleTakingText();
				_didClickSearchBar = true;
			}
		}

		public override void Click(UIMouseEvent evt)
		{
			base.Click(evt);
			AttemptStoppingUsingSearchbar(evt);
		}

		private void AttemptStoppingUsingSearchbar(UIMouseEvent evt)
		{
			_didClickSomething = true;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (_didClickSomething && !_didClickSearchBar && _searchBar.IsWritingText)
			{
				_searchBar.ToggleTakingText();
			}
			_didClickSomething = false;
			_didClickSearchBar = false;
		}

		private void FilterEntries()
		{
			_workingSetEntries.Clear();
			_workingSetEntries.AddRange(_originalEntriesList.Where(_filterer.FitsFilter));
		}

		private void SortEntries()
		{
			_workingSetEntries.Sort(_sorter);
		}

		private void FillBestiarySpaceWithEntries()
		{
			if (_entryGrid != null && _entryGrid.Parent != null)
			{
				DeselectEntryButton();
				_progressReport = GetUnlockProgress();
				_entryGrid.FillBestiarySpaceWithEntries();
			}
		}

		public void UpdateBestiaryGridRange()
		{
			_indexesRangeText.SetText(_entryGrid.GetRangeText());
		}

		public override void Recalculate()
		{
			base.Recalculate();
			FillBestiarySpaceWithEntries();
		}

		private void GetEntriesToShow(out int maxEntriesWidth, out int maxEntriesHeight, out int maxEntriesToHave)
		{
			Rectangle rectangle = _bestiarySpace.GetDimensions().ToRectangle();
			maxEntriesWidth = rectangle.Width / 72;
			maxEntriesHeight = rectangle.Height / 72;
			int num = 0;
			maxEntriesToHave = maxEntriesWidth * maxEntriesHeight - num;
		}

		private void MakeExitButton(UIElement outerContainer)
		{
			UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true)
			{
				Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
				Height = StyleDimension.FromPixels(50f),
				VAlign = 1f,
				HAlign = 0.5f,
				Top = StyleDimension.FromPixels(-25f)
			};
			uITextPanel.OnMouseOver += FadedMouseOver;
			uITextPanel.OnMouseOut += FadedMouseOut;
			uITextPanel.OnMouseDown += Click_GoBack;
			uITextPanel.SetSnapPoint("ExitButton", 0);
			outerContainer.Append(uITextPanel);
		}

		private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(11);
			if (Main.gameMenu)
			{
				Main.menuMode = 0;
			}
			else
			{
				IngameFancyUI.Close();
			}
		}

		private void OpenOrCloseSortingOptions(UIMouseEvent evt, UIElement listeningElement)
		{
			if (_sortingGrid.Parent != null)
			{
				CloseSortingGrid();
				return;
			}
			_bestiarySpace.RemoveChild(_sortingGrid);
			_bestiarySpace.RemoveChild(_filteringGrid);
			_bestiarySpace.Append(_sortingGrid);
		}

		private void OpenOrCloseFilteringGrid(UIMouseEvent evt, UIElement listeningElement)
		{
			if (_filteringGrid.Parent != null)
			{
				CloseFilteringGrid();
				return;
			}
			_bestiarySpace.RemoveChild(_sortingGrid);
			_bestiarySpace.RemoveChild(_filteringGrid);
			_bestiarySpace.Append(_filteringGrid);
		}

		private void Click_CloseFilteringGrid(UIMouseEvent evt, UIElement listeningElement)
		{
			if (evt.Target == _filteringGrid)
			{
				CloseFilteringGrid();
			}
		}

		private void CloseFilteringGrid()
		{
			UpdateBestiaryContents();
			_bestiarySpace.RemoveChild(_filteringGrid);
		}

		private void UpdateBestiaryContents()
		{
			_filteringGrid.UpdateAvailability();
			_sortingText.SetText(_sorter.GetDisplayName());
			_filteringText.SetText(_filterer.GetDisplayName());
			FilterEntries();
			SortEntries();
			FillBestiarySpaceWithEntries();
			_progressReport = GetUnlockProgress();
			string completionPercentText = GetCompletionPercentText();
			_progressPercentText.SetText(completionPercentText);
			_unlocksProgressBar.FillPercent = GetProgressPercent();
		}

		private void Click_CloseSortingGrid(UIMouseEvent evt, UIElement listeningElement)
		{
			if (evt.Target == _sortingGrid)
			{
				CloseSortingGrid();
			}
		}

		private void CloseSortingGrid()
		{
			UpdateBestiaryContents();
			_bestiarySpace.RemoveChild(_sortingGrid);
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

		private void Click_SelectEntryButton(UIMouseEvent evt, UIElement listeningElement)
		{
			UIBestiaryEntryButton uIBestiaryEntryButton = (UIBestiaryEntryButton)listeningElement;
			if (uIBestiaryEntryButton != null)
			{
				SelectEntryButton(uIBestiaryEntryButton);
			}
		}

		private void SelectEntryButton(UIBestiaryEntryButton button)
		{
			DeselectEntryButton();
			_selectedEntryButton = button;
			_infoSpace.FillInfoForEntry(button.Entry, new ExtraBestiaryInfoPageInformation
			{
				BestiaryProgressReport = _progressReport
			});
		}

		private void DeselectEntryButton()
		{
			_infoSpace.FillInfoForEntry(null, default(ExtraBestiaryInfoPageInformation));
		}

		public BestiaryUnlockProgressReport GetUnlockProgress()
		{
			float num = 0f;
			int num2 = 0;
			List<BestiaryEntry> originalEntriesList = _originalEntriesList;
			for (int i = 0; i < originalEntriesList.Count; i++)
			{
				int num3 = ((originalEntriesList[i].UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0) ? 1 : 0);
				num2++;
				num += (float)num3;
			}
			BestiaryUnlockProgressReport result = default(BestiaryUnlockProgressReport);
			result.EntriesTotal = num2;
			result.CompletionAmountTotal = num;
			return result;
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
			int currentID = num;
			List<SnapPoint> snapPoints = GetSnapPoints();
			SnapPoint snap = null;
			SnapPoint snap2 = null;
			SnapPoint snap3 = null;
			SnapPoint snap4 = null;
			SnapPoint snap5 = null;
			SnapPoint snap6 = null;
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint = snapPoints[i];
				switch (snapPoint.Name)
				{
				case "BackPage":
					snap = snapPoint;
					break;
				case "NextPage":
					snap2 = snapPoint;
					break;
				case "ExitButton":
					snap6 = snapPoint;
					break;
				case "FilterButton":
					snap4 = snapPoint;
					break;
				case "SortButton":
					snap3 = snapPoint;
					break;
				case "SearchButton":
					snap5 = snapPoint;
					break;
				}
			}
			UILinkPoint uILinkPoint = MakeLinkPointFromSnapPoint(currentID++, snap);
			UILinkPoint uILinkPoint2 = MakeLinkPointFromSnapPoint(currentID++, snap2);
			UILinkPoint uILinkPoint3 = MakeLinkPointFromSnapPoint(currentID++, snap6);
			UILinkPoint uILinkPoint4 = MakeLinkPointFromSnapPoint(currentID++, snap4);
			UILinkPoint uILinkPoint5 = MakeLinkPointFromSnapPoint(currentID++, snap3);
			UILinkPoint rightSide = MakeLinkPointFromSnapPoint(currentID++, snap5);
			PairLeftRight(uILinkPoint, uILinkPoint2);
			PairLeftRight(uILinkPoint2, uILinkPoint5);
			PairLeftRight(uILinkPoint5, uILinkPoint4);
			PairLeftRight(uILinkPoint4, rightSide);
			uILinkPoint3.Up = uILinkPoint2.ID;
			UILinkPoint[,] gridPoints = new UILinkPoint[1, 1];
			int gridWidth;
			int gridHeight;
			if (_filteringGrid.Parent != null)
			{
				SetupPointsForFilterGrid(ref currentID, snapPoints, out gridWidth, out gridHeight, out gridPoints);
				PairUpDown(uILinkPoint2, uILinkPoint3);
				PairUpDown(uILinkPoint, uILinkPoint3);
				for (int num2 = gridWidth - 1; num2 >= 0; num2--)
				{
					UILinkPoint uILinkPoint6 = gridPoints[num2, gridHeight - 1];
					if (uILinkPoint6 != null)
					{
						PairUpDown(uILinkPoint6, uILinkPoint3);
					}
					UILinkPoint uILinkPoint7 = gridPoints[num2, gridHeight - 2];
					if (uILinkPoint7 != null && uILinkPoint6 == null)
					{
						PairUpDown(uILinkPoint7, uILinkPoint3);
					}
					UILinkPoint uILinkPoint8 = gridPoints[num2, 0];
					if (uILinkPoint8 != null)
					{
						if (num2 < gridWidth - 3)
						{
							PairUpDown(uILinkPoint5, uILinkPoint8);
						}
						else
						{
							PairUpDown(uILinkPoint4, uILinkPoint8);
						}
					}
				}
			}
			else if (_sortingGrid.Parent != null)
			{
				SetupPointsForSortingGrid(ref currentID, snapPoints, out gridWidth, out gridHeight, out gridPoints);
				PairUpDown(uILinkPoint2, uILinkPoint3);
				PairUpDown(uILinkPoint, uILinkPoint3);
				for (int num3 = gridWidth - 1; num3 >= 0; num3--)
				{
					UILinkPoint uILinkPoint9 = gridPoints[num3, gridHeight - 1];
					if (uILinkPoint9 != null)
					{
						PairUpDown(uILinkPoint9, uILinkPoint3);
					}
					UILinkPoint uILinkPoint10 = gridPoints[num3, 0];
					if (uILinkPoint10 != null)
					{
						PairUpDown(uILinkPoint4, uILinkPoint10);
						PairUpDown(uILinkPoint5, uILinkPoint10);
					}
				}
			}
			else if (_entryGrid.Parent != null)
			{
				SetupPointsForEntryGrid(ref currentID, snapPoints, out gridWidth, out gridHeight, out gridPoints);
				for (int j = 0; j < gridWidth; j++)
				{
					if (gridHeight - 1 >= 0)
					{
						UILinkPoint uILinkPoint11 = gridPoints[j, gridHeight - 1];
						if (uILinkPoint11 != null)
						{
							PairUpDown(uILinkPoint11, uILinkPoint3);
						}
						if (gridHeight - 2 >= 0)
						{
							UILinkPoint uILinkPoint12 = gridPoints[j, gridHeight - 2];
							if (uILinkPoint12 != null && uILinkPoint11 == null)
							{
								PairUpDown(uILinkPoint12, uILinkPoint3);
							}
						}
					}
					UILinkPoint uILinkPoint13 = gridPoints[j, 0];
					if (uILinkPoint13 != null)
					{
						if (j < gridWidth / 2)
						{
							PairUpDown(uILinkPoint2, uILinkPoint13);
						}
						else if (j == gridWidth - 1)
						{
							PairUpDown(uILinkPoint4, uILinkPoint13);
						}
						else
						{
							PairUpDown(uILinkPoint5, uILinkPoint13);
						}
					}
				}
				UILinkPoint uILinkPoint14 = gridPoints[0, 0];
				if (uILinkPoint14 != null)
				{
					PairUpDown(uILinkPoint2, uILinkPoint14);
					PairUpDown(uILinkPoint, uILinkPoint14);
				}
				else
				{
					PairUpDown(uILinkPoint2, uILinkPoint3);
					PairUpDown(uILinkPoint, uILinkPoint3);
					PairUpDown(uILinkPoint4, uILinkPoint3);
					PairUpDown(uILinkPoint5, uILinkPoint3);
				}
			}
			List<UILinkPoint> list = new List<UILinkPoint>();
			for (int k = num; k < currentID; k++)
			{
				list.Add(UILinkPointNavigator.Points[k]);
			}
			if (PlayerInput.UsingGamepadUI && UILinkPointNavigator.CurrentPoint >= currentID)
			{
				MoveToVisuallyClosestPoint(list);
			}
		}

		private void MoveToVisuallyClosestPoint(List<UILinkPoint> lostrefpoints)
		{
			_ = UILinkPointNavigator.Points;
			Vector2 mouseScreen = Main.MouseScreen;
			UILinkPoint uILinkPoint = null;
			foreach (UILinkPoint lostrefpoint in lostrefpoints)
			{
				if (uILinkPoint == null || Vector2.Distance(mouseScreen, uILinkPoint.Position) > Vector2.Distance(mouseScreen, lostrefpoint.Position))
				{
					uILinkPoint = lostrefpoint;
				}
			}
			if (uILinkPoint != null)
			{
				UILinkPointNavigator.ChangePoint(uILinkPoint.ID);
			}
		}

		private void SetupPointsForEntryGrid(ref int currentID, List<SnapPoint> pts, out int gridWidth, out int gridHeight, out UILinkPoint[,] gridPoints)
		{
			List<SnapPoint> orderedPointsByCategoryName = GetOrderedPointsByCategoryName(pts, "Entries");
			_entryGrid.GetEntriesToShow(out gridWidth, out gridHeight, out var _);
			gridPoints = new UILinkPoint[gridWidth, gridHeight];
			for (int i = 0; i < orderedPointsByCategoryName.Count; i++)
			{
				int num = i % gridWidth;
				int num2 = i / gridWidth;
				gridPoints[num, num2] = MakeLinkPointFromSnapPoint(currentID++, orderedPointsByCategoryName[i]);
			}
			for (int j = 0; j < gridWidth; j++)
			{
				for (int k = 0; k < gridHeight; k++)
				{
					UILinkPoint uILinkPoint = gridPoints[j, k];
					if (j < gridWidth - 1)
					{
						UILinkPoint uILinkPoint2 = gridPoints[j + 1, k];
						if (uILinkPoint != null && uILinkPoint2 != null)
						{
							PairLeftRight(uILinkPoint, uILinkPoint2);
						}
					}
					if (k < gridHeight - 1)
					{
						UILinkPoint uILinkPoint3 = gridPoints[j, k + 1];
						if (uILinkPoint != null && uILinkPoint3 != null)
						{
							PairUpDown(uILinkPoint, uILinkPoint3);
						}
					}
				}
			}
		}

		private void SetupPointsForFilterGrid(ref int currentID, List<SnapPoint> pts, out int gridWidth, out int gridHeight, out UILinkPoint[,] gridPoints)
		{
			List<SnapPoint> orderedPointsByCategoryName = GetOrderedPointsByCategoryName(pts, "Filters");
			_filteringGrid.GetEntriesToShow(out gridWidth, out gridHeight, out var _);
			gridPoints = new UILinkPoint[gridWidth, gridHeight];
			for (int i = 0; i < orderedPointsByCategoryName.Count; i++)
			{
				int num = i % gridWidth;
				int num2 = i / gridWidth;
				gridPoints[num, num2] = MakeLinkPointFromSnapPoint(currentID++, orderedPointsByCategoryName[i]);
			}
			for (int j = 0; j < gridWidth; j++)
			{
				for (int k = 0; k < gridHeight; k++)
				{
					UILinkPoint uILinkPoint = gridPoints[j, k];
					if (j < gridWidth - 1)
					{
						UILinkPoint uILinkPoint2 = gridPoints[j + 1, k];
						if (uILinkPoint != null && uILinkPoint2 != null)
						{
							PairLeftRight(uILinkPoint, uILinkPoint2);
						}
					}
					if (k < gridHeight - 1)
					{
						UILinkPoint uILinkPoint3 = gridPoints[j, k + 1];
						if (uILinkPoint != null && uILinkPoint3 != null)
						{
							PairUpDown(uILinkPoint, uILinkPoint3);
						}
					}
				}
			}
		}

		private void SetupPointsForSortingGrid(ref int currentID, List<SnapPoint> pts, out int gridWidth, out int gridHeight, out UILinkPoint[,] gridPoints)
		{
			List<SnapPoint> orderedPointsByCategoryName = GetOrderedPointsByCategoryName(pts, "SortSteps");
			_sortingGrid.GetEntriesToShow(out gridWidth, out gridHeight, out var _);
			gridPoints = new UILinkPoint[gridWidth, gridHeight];
			for (int i = 0; i < orderedPointsByCategoryName.Count; i++)
			{
				int num = i % gridWidth;
				int num2 = i / gridWidth;
				gridPoints[num, num2] = MakeLinkPointFromSnapPoint(currentID++, orderedPointsByCategoryName[i]);
			}
			for (int j = 0; j < gridWidth; j++)
			{
				for (int k = 0; k < gridHeight; k++)
				{
					UILinkPoint uILinkPoint = gridPoints[j, k];
					if (j < gridWidth - 1)
					{
						UILinkPoint uILinkPoint2 = gridPoints[j + 1, k];
						if (uILinkPoint != null && uILinkPoint2 != null)
						{
							PairLeftRight(uILinkPoint, uILinkPoint2);
						}
					}
					if (k < gridHeight - 1)
					{
						UILinkPoint uILinkPoint3 = gridPoints[j, k + 1];
						if (uILinkPoint != null && uILinkPoint3 != null)
						{
							PairUpDown(uILinkPoint, uILinkPoint3);
						}
					}
				}
			}
		}

		private static List<SnapPoint> GetOrderedPointsByCategoryName(List<SnapPoint> pts, string name)
		{
			return (from x in pts
				where x.Name == name
				orderby x.Id
				select x).ToList();
		}

		private void PairLeftRight(UILinkPoint leftSide, UILinkPoint rightSide)
		{
			leftSide.Right = rightSide.ID;
			rightSide.Left = leftSide.ID;
		}

		private void PairUpDown(UILinkPoint upSide, UILinkPoint downSide)
		{
			upSide.Down = downSide.ID;
			downSide.Up = upSide.ID;
		}

		private UILinkPoint MakeLinkPointFromSnapPoint(int id, SnapPoint snap)
		{
			UILinkPointNavigator.SetPosition(id, snap.Position);
			UILinkPoint uILinkPoint = UILinkPointNavigator.Points[id];
			uILinkPoint.Unlink();
			return uILinkPoint;
		}

		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			base.ScrollWheel(evt);
			_infoSpace.UpdateScrollbar(evt.ScrollWheelValue);
		}

		public void TryMovingPages(int direction)
		{
			_entryGrid.OffsetLibraryByPages(direction);
		}
	}
}
