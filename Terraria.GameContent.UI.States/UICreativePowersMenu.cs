using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	public class UICreativePowersMenu : UIState
	{
		private class MenuTree<TEnum> where TEnum : struct, IConvertible
		{
			public int CurrentOption;

			public Dictionary<int, GroupOptionButton<int>> Buttons = new Dictionary<int, GroupOptionButton<int>>();

			public Dictionary<int, UIElement> Sliders = new Dictionary<int, UIElement>();

			public MenuTree(TEnum defaultValue)
			{
				CurrentOption = defaultValue.ToInt32(null);
			}
		}

		private enum OpenMainSubCategory
		{
			None,
			InfiniteItems,
			ResearchWindow,
			Time,
			Weather,
			EnemyStrengthSlider,
			PersonalPowers
		}

		private enum WeatherSubcategory
		{
			None,
			WindSlider,
			RainSlider
		}

		private enum TimeSubcategory
		{
			None,
			TimeRate
		}

		private enum PersonalSubcategory
		{
			None,
			EnemySpawnRateSlider
		}

		private bool _hovered;

		private PowerStripUIElement _mainPowerStrip;

		private PowerStripUIElement _timePowersStrip;

		private PowerStripUIElement _weatherPowersStrip;

		private PowerStripUIElement _personalPowersStrip;

		private UICreativeInfiniteItemsDisplay _infiniteItemsWindow;

		private UIElement _container;

		private MenuTree<OpenMainSubCategory> _mainCategory = new MenuTree<OpenMainSubCategory>(OpenMainSubCategory.None);

		private MenuTree<WeatherSubcategory> _weatherCategory = new MenuTree<WeatherSubcategory>(WeatherSubcategory.None);

		private MenuTree<TimeSubcategory> _timeCategory = new MenuTree<TimeSubcategory>(TimeSubcategory.None);

		private MenuTree<PersonalSubcategory> _personalCategory = new MenuTree<PersonalSubcategory>(PersonalSubcategory.None);

		private const int INITIAL_LEFT_PIXELS = 20;

		private const int LEFT_PIXELS_PER_STRIP_DEPTH = 60;

		private const string STRIP_MAIN = "strip 0";

		private const string STRIP_DEPTH_1 = "strip 1";

		private const string STRIP_DEPTH_2 = "strip 2";

		private UIGamepadHelper _helper;

		public bool IsShowingResearchMenu => _mainCategory.CurrentOption == 2;

		public override void OnActivate()
		{
			InitializePage();
		}

		private void InitializePage()
		{
			int num = 270;
			int num2 = 20;
			_container = new UIElement
			{
				HAlign = 0f,
				VAlign = 0f,
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(-num - num2, 1f),
				Top = new StyleDimension(num, 0f)
			};
			Append(_container);
			List<UIElement> buttons = CreateMainPowerStrip();
			PowerStripUIElement powerStripUIElement = new PowerStripUIElement("strip 0", buttons)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(20f, 0f)
			};
			powerStripUIElement.OnMouseOver += strip_OnMouseOver;
			powerStripUIElement.OnMouseOut += strip_OnMouseOut;
			_mainPowerStrip = powerStripUIElement;
			List<UIElement> buttons2 = CreateTimePowerStrip();
			PowerStripUIElement powerStripUIElement2 = new PowerStripUIElement("strip 1", buttons2)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(80f, 0f)
			};
			powerStripUIElement2.OnMouseOver += strip_OnMouseOver;
			powerStripUIElement2.OnMouseOut += strip_OnMouseOut;
			_timePowersStrip = powerStripUIElement2;
			List<UIElement> buttons3 = CreateWeatherPowerStrip();
			PowerStripUIElement powerStripUIElement3 = new PowerStripUIElement("strip 1", buttons3)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(80f, 0f)
			};
			powerStripUIElement3.OnMouseOver += strip_OnMouseOver;
			powerStripUIElement3.OnMouseOut += strip_OnMouseOut;
			_weatherPowersStrip = powerStripUIElement3;
			List<UIElement> buttons4 = CreatePersonalPowerStrip();
			PowerStripUIElement powerStripUIElement4 = new PowerStripUIElement("strip 1", buttons4)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(80f, 0f)
			};
			powerStripUIElement4.OnMouseOver += strip_OnMouseOver;
			powerStripUIElement4.OnMouseOut += strip_OnMouseOut;
			_personalPowersStrip = powerStripUIElement4;
			_infiniteItemsWindow = new UICreativeInfiniteItemsDisplay(this)
			{
				HAlign = 0f,
				VAlign = 0.5f,
				Left = new StyleDimension(80f, 0f),
				Width = new StyleDimension(480f, 0f),
				Height = new StyleDimension(-88f, 1f)
			};
			RefreshElementsOrder();
			base.OnUpdate += UICreativePowersMenu_OnUpdate;
		}

		private List<UIElement> CreateMainPowerStrip()
		{
			MenuTree<OpenMainSubCategory> mainCategory = _mainCategory;
			mainCategory.Buttons.Clear();
			List<UIElement> list = new List<UIElement>();
			CreativePowerUIElementRequestInfo creativePowerUIElementRequestInfo = default(CreativePowerUIElementRequestInfo);
			creativePowerUIElementRequestInfo.PreferredButtonWidth = 40;
			creativePowerUIElementRequestInfo.PreferredButtonHeight = 40;
			CreativePowerUIElementRequestInfo request = creativePowerUIElementRequestInfo;
			GroupOptionButton<int> groupOptionButton = CreativePowersHelper.CreateCategoryButton(request, 1, 0);
			groupOptionButton.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.ItemDuplication));
			groupOptionButton.OnClick += MainCategoryButtonClick;
			groupOptionButton.OnUpdate += itemsWindowButton_OnUpdate;
			mainCategory.Buttons.Add(1, groupOptionButton);
			list.Add(groupOptionButton);
			GroupOptionButton<int> groupOptionButton2 = CreativePowersHelper.CreateCategoryButton(request, 2, 0);
			groupOptionButton2.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.ItemResearch));
			groupOptionButton2.OnClick += MainCategoryButtonClick;
			groupOptionButton2.OnUpdate += researchWindowButton_OnUpdate;
			mainCategory.Buttons.Add(2, groupOptionButton2);
			list.Add(groupOptionButton2);
			GroupOptionButton<int> groupOptionButton3 = CreativePowersHelper.CreateCategoryButton(request, 3, 0);
			groupOptionButton3.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.TimeCategory));
			groupOptionButton3.OnClick += MainCategoryButtonClick;
			groupOptionButton3.OnUpdate += timeCategoryButton_OnUpdate;
			mainCategory.Buttons.Add(3, groupOptionButton3);
			list.Add(groupOptionButton3);
			GroupOptionButton<int> groupOptionButton4 = CreativePowersHelper.CreateCategoryButton(request, 4, 0);
			groupOptionButton4.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.WeatherCategory));
			groupOptionButton4.OnClick += MainCategoryButtonClick;
			groupOptionButton4.OnUpdate += weatherCategoryButton_OnUpdate;
			mainCategory.Buttons.Add(4, groupOptionButton4);
			list.Add(groupOptionButton4);
			GroupOptionButton<int> groupOptionButton5 = CreativePowersHelper.CreateCategoryButton(request, 6, 0);
			groupOptionButton5.Append(CreativePowersHelper.GetIconImage(CreativePowersHelper.CreativePowerIconLocations.PersonalCategory));
			groupOptionButton5.OnClick += MainCategoryButtonClick;
			groupOptionButton5.OnUpdate += personalCategoryButton_OnUpdate;
			mainCategory.Buttons.Add(6, groupOptionButton5);
			list.Add(groupOptionButton5);
			CreativePowerManager.Instance.GetPower<CreativePowers.StopBiomeSpreadPower>().ProvidePowerButtons(request, list);
			GroupOptionButton<int> groupOptionButton6 = CreateSubcategoryButton<CreativePowers.DifficultySliderPower>(ref request, 1, "strip 1", 5, 0, mainCategory.Buttons, mainCategory.Sliders);
			groupOptionButton6.OnClick += MainCategoryButtonClick;
			list.Add(groupOptionButton6);
			return list;
		}

		private static void CategoryButton_OnUpdate_DisplayTooltips(UIElement affectedElement, string categoryNameKey)
		{
			GroupOptionButton<int> groupOptionButton = affectedElement as GroupOptionButton<int>;
			if (affectedElement.IsMouseHovering)
			{
				string originalText = Language.GetTextValue(groupOptionButton.IsSelected ? (categoryNameKey + "Opened") : (categoryNameKey + "Closed"));
				CreativePowersHelper.AddDescriptionIfNeeded(ref originalText, categoryNameKey);
				Main.instance.MouseTextNoOverride(originalText, 0, 0);
			}
		}

		private void itemsWindowButton_OnUpdate(UIElement affectedElement)
		{
			CategoryButton_OnUpdate_DisplayTooltips(affectedElement, "CreativePowers.InfiniteItemsCategory");
		}

		private void researchWindowButton_OnUpdate(UIElement affectedElement)
		{
			CategoryButton_OnUpdate_DisplayTooltips(affectedElement, "CreativePowers.ResearchItemsCategory");
		}

		private void timeCategoryButton_OnUpdate(UIElement affectedElement)
		{
			CategoryButton_OnUpdate_DisplayTooltips(affectedElement, "CreativePowers.TimeCategory");
		}

		private void weatherCategoryButton_OnUpdate(UIElement affectedElement)
		{
			CategoryButton_OnUpdate_DisplayTooltips(affectedElement, "CreativePowers.WeatherCategory");
		}

		private void personalCategoryButton_OnUpdate(UIElement affectedElement)
		{
			CategoryButton_OnUpdate_DisplayTooltips(affectedElement, "CreativePowers.PersonalCategory");
		}

		private void UICreativePowersMenu_OnUpdate(UIElement affectedElement)
		{
			if (_hovered)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		private void strip_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			_hovered = false;
		}

		private void strip_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			_hovered = true;
		}

		private void MainCategoryButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<int> groupOptionButton = listeningElement as GroupOptionButton<int>;
			ToggleMainCategory(groupOptionButton.OptionValue);
			RefreshElementsOrder();
		}

		private void ToggleMainCategory(int option)
		{
			ToggleCategory(_mainCategory, option, OpenMainSubCategory.None);
		}

		private void ToggleWeatherCategory(int option)
		{
			ToggleCategory(_weatherCategory, option, WeatherSubcategory.None);
		}

		private void ToggleTimeCategory(int option)
		{
			ToggleCategory(_timeCategory, option, TimeSubcategory.None);
		}

		private void TogglePersonalCategory(int option)
		{
			ToggleCategory(_personalCategory, option, PersonalSubcategory.None);
		}

		public void SacrificeWhatsInResearchMenu()
		{
			_infiniteItemsWindow.SacrificeWhatYouCan();
		}

		public void StopPlayingResearchAnimations()
		{
			_infiniteItemsWindow.StopPlayingAnimation();
		}

		private void ToggleCategory<TEnum>(MenuTree<TEnum> tree, int option, TEnum defaultOption) where TEnum : struct, IConvertible
		{
			if (tree.CurrentOption == option)
			{
				option = defaultOption.ToInt32(null);
			}
			tree.CurrentOption = option;
			foreach (GroupOptionButton<int> value in tree.Buttons.Values)
			{
				value.SetCurrentOption(option);
			}
		}

		private List<UIElement> CreateTimePowerStrip()
		{
			MenuTree<TimeSubcategory> timeCategory = _timeCategory;
			List<UIElement> list = new List<UIElement>();
			CreativePowerUIElementRequestInfo creativePowerUIElementRequestInfo = default(CreativePowerUIElementRequestInfo);
			creativePowerUIElementRequestInfo.PreferredButtonWidth = 40;
			creativePowerUIElementRequestInfo.PreferredButtonHeight = 40;
			CreativePowerUIElementRequestInfo request = creativePowerUIElementRequestInfo;
			CreativePowerManager.Instance.GetPower<CreativePowers.FreezeTime>().ProvidePowerButtons(request, list);
			CreativePowerManager.Instance.GetPower<CreativePowers.StartDayImmediately>().ProvidePowerButtons(request, list);
			CreativePowerManager.Instance.GetPower<CreativePowers.StartNoonImmediately>().ProvidePowerButtons(request, list);
			CreativePowerManager.Instance.GetPower<CreativePowers.StartNightImmediately>().ProvidePowerButtons(request, list);
			CreativePowerManager.Instance.GetPower<CreativePowers.StartMidnightImmediately>().ProvidePowerButtons(request, list);
			GroupOptionButton<int> groupOptionButton = CreateSubcategoryButton<CreativePowers.ModifyTimeRate>(ref request, 2, "strip 2", 1, 0, timeCategory.Buttons, timeCategory.Sliders);
			groupOptionButton.OnClick += TimeCategoryButtonClick;
			list.Add(groupOptionButton);
			return list;
		}

		private List<UIElement> CreatePersonalPowerStrip()
		{
			MenuTree<PersonalSubcategory> personalCategory = _personalCategory;
			List<UIElement> list = new List<UIElement>();
			CreativePowerUIElementRequestInfo creativePowerUIElementRequestInfo = default(CreativePowerUIElementRequestInfo);
			creativePowerUIElementRequestInfo.PreferredButtonWidth = 40;
			creativePowerUIElementRequestInfo.PreferredButtonHeight = 40;
			CreativePowerUIElementRequestInfo request = creativePowerUIElementRequestInfo;
			CreativePowerManager.Instance.GetPower<CreativePowers.GodmodePower>().ProvidePowerButtons(request, list);
			CreativePowerManager.Instance.GetPower<CreativePowers.FarPlacementRangePower>().ProvidePowerButtons(request, list);
			GroupOptionButton<int> groupOptionButton = CreateSubcategoryButton<CreativePowers.SpawnRateSliderPerPlayerPower>(ref request, 2, "strip 2", 1, 0, personalCategory.Buttons, personalCategory.Sliders);
			groupOptionButton.OnClick += PersonalCategoryButtonClick;
			list.Add(groupOptionButton);
			return list;
		}

		private List<UIElement> CreateWeatherPowerStrip()
		{
			MenuTree<WeatherSubcategory> weatherCategory = _weatherCategory;
			List<UIElement> list = new List<UIElement>();
			CreativePowerUIElementRequestInfo creativePowerUIElementRequestInfo = default(CreativePowerUIElementRequestInfo);
			creativePowerUIElementRequestInfo.PreferredButtonWidth = 40;
			creativePowerUIElementRequestInfo.PreferredButtonHeight = 40;
			CreativePowerUIElementRequestInfo request = creativePowerUIElementRequestInfo;
			GroupOptionButton<int> groupOptionButton = CreateSubcategoryButton<CreativePowers.ModifyWindDirectionAndStrength>(ref request, 2, "strip 2", 1, 0, weatherCategory.Buttons, weatherCategory.Sliders);
			groupOptionButton.OnClick += WeatherCategoryButtonClick;
			list.Add(groupOptionButton);
			CreativePowerManager.Instance.GetPower<CreativePowers.FreezeWindDirectionAndStrength>().ProvidePowerButtons(request, list);
			GroupOptionButton<int> groupOptionButton2 = CreateSubcategoryButton<CreativePowers.ModifyRainPower>(ref request, 2, "strip 2", 2, 0, weatherCategory.Buttons, weatherCategory.Sliders);
			groupOptionButton2.OnClick += WeatherCategoryButtonClick;
			list.Add(groupOptionButton2);
			CreativePowerManager.Instance.GetPower<CreativePowers.FreezeRainPower>().ProvidePowerButtons(request, list);
			return list;
		}

		private GroupOptionButton<int> CreateSubcategoryButton<T>(ref CreativePowerUIElementRequestInfo request, int subcategoryDepth, string subcategoryName, int subcategoryIndex, int currentSelectedInSubcategory, Dictionary<int, GroupOptionButton<int>> subcategoryButtons, Dictionary<int, UIElement> slidersSet) where T : ICreativePower, IProvideSliderElement, IPowerSubcategoryElement
		{
			T power = CreativePowerManager.Instance.GetPower<T>();
			UIElement uIElement = power.ProvideSlider();
			uIElement.Left = new StyleDimension(20 + subcategoryDepth * 60, 0f);
			slidersSet[subcategoryIndex] = uIElement;
			uIElement.SetSnapPoint(subcategoryName, 0, new Vector2(0f, 0.5f), new Vector2(28f, 0f));
			GroupOptionButton<int> groupOptionButton = (subcategoryButtons[subcategoryIndex] = power.GetOptionButton(request, subcategoryIndex, currentSelectedInSubcategory));
			CreativePowersHelper.UpdateUnlockStateByPower(power, groupOptionButton, CreativePowersHelper.CommonSelectedColor);
			return groupOptionButton;
		}

		private void WeatherCategoryButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<int> groupOptionButton = listeningElement as GroupOptionButton<int>;
			switch (groupOptionButton.OptionValue)
			{
			case 2:
				if (!CreativePowerManager.Instance.GetPower<CreativePowers.ModifyRainPower>().GetIsUnlocked())
				{
					return;
				}
				break;
			case 1:
				if (!CreativePowerManager.Instance.GetPower<CreativePowers.ModifyWindDirectionAndStrength>().GetIsUnlocked())
				{
					return;
				}
				break;
			}
			ToggleWeatherCategory(groupOptionButton.OptionValue);
			RefreshElementsOrder();
		}

		private void TimeCategoryButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<int> groupOptionButton = listeningElement as GroupOptionButton<int>;
			int optionValue = groupOptionButton.OptionValue;
			if (optionValue != 1 || CreativePowerManager.Instance.GetPower<CreativePowers.ModifyTimeRate>().GetIsUnlocked())
			{
				ToggleTimeCategory(groupOptionButton.OptionValue);
				RefreshElementsOrder();
			}
		}

		private void PersonalCategoryButtonClick(UIMouseEvent evt, UIElement listeningElement)
		{
			GroupOptionButton<int> groupOptionButton = listeningElement as GroupOptionButton<int>;
			int optionValue = groupOptionButton.OptionValue;
			if (optionValue != 1 || CreativePowerManager.Instance.GetPower<CreativePowers.SpawnRateSliderPerPlayerPower>().GetIsUnlocked())
			{
				TogglePersonalCategory(groupOptionButton.OptionValue);
				RefreshElementsOrder();
			}
		}

		private void RefreshElementsOrder()
		{
			_container.RemoveAllChildren();
			_container.Append(_mainPowerStrip);
			UIElement value = null;
			MenuTree<OpenMainSubCategory> mainCategory = _mainCategory;
			if (mainCategory.Sliders.TryGetValue(mainCategory.CurrentOption, out value))
			{
				_container.Append(value);
			}
			if (mainCategory.CurrentOption == 1)
			{
				_infiniteItemsWindow.SetPageTypeToShow(UICreativeInfiniteItemsDisplay.InfiniteItemsDisplayPage.InfiniteItemsPickup);
				_container.Append(_infiniteItemsWindow);
			}
			if (mainCategory.CurrentOption == 2)
			{
				_infiniteItemsWindow.SetPageTypeToShow(UICreativeInfiniteItemsDisplay.InfiniteItemsDisplayPage.InfiniteItemsResearch);
				_container.Append(_infiniteItemsWindow);
			}
			if (mainCategory.CurrentOption == 3)
			{
				_container.Append(_timePowersStrip);
				MenuTree<TimeSubcategory> timeCategory = _timeCategory;
				if (timeCategory.Sliders.TryGetValue(timeCategory.CurrentOption, out value))
				{
					_container.Append(value);
				}
			}
			if (mainCategory.CurrentOption == 4)
			{
				_container.Append(_weatherPowersStrip);
				MenuTree<WeatherSubcategory> weatherCategory = _weatherCategory;
				if (weatherCategory.Sliders.TryGetValue(weatherCategory.CurrentOption, out value))
				{
					_container.Append(value);
				}
			}
			if (mainCategory.CurrentOption == 6)
			{
				_container.Append(_personalPowersStrip);
				MenuTree<PersonalSubcategory> personalCategory = _personalCategory;
				if (personalCategory.Sliders.TryGetValue(personalCategory.CurrentOption, out value))
				{
					_container.Append(value);
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
			int currentID = 10000;
			List<SnapPoint> snapPoints = GetSnapPoints();
			List<SnapPoint> orderedPointsByCategoryName = _helper.GetOrderedPointsByCategoryName(snapPoints, "strip 0");
			List<SnapPoint> orderedPointsByCategoryName2 = _helper.GetOrderedPointsByCategoryName(snapPoints, "strip 1");
			List<SnapPoint> orderedPointsByCategoryName3 = _helper.GetOrderedPointsByCategoryName(snapPoints, "strip 2");
			UILinkPoint[] array = null;
			UILinkPoint[] array2 = null;
			UILinkPoint[] array3 = null;
			if (orderedPointsByCategoryName.Count > 0)
			{
				array = _helper.CreateUILinkStripVertical(ref currentID, orderedPointsByCategoryName);
			}
			if (orderedPointsByCategoryName2.Count > 0)
			{
				array2 = _helper.CreateUILinkStripVertical(ref currentID, orderedPointsByCategoryName2);
			}
			if (orderedPointsByCategoryName3.Count > 0)
			{
				array3 = _helper.CreateUILinkStripVertical(ref currentID, orderedPointsByCategoryName3);
			}
			if (array != null && array2 != null)
			{
				_helper.LinkVerticalStrips(array, array2, (array.Length - array2.Length) / 2);
			}
			if (array2 != null && array3 != null)
			{
				_helper.LinkVerticalStrips(array2, array3, (array.Length - array2.Length) / 2);
			}
			UILinkPoint uILinkPoint = null;
			UILinkPoint uILinkPoint2 = null;
			UILinkPoint uILinkPoint3 = null;
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint = snapPoints[i];
				string name = snapPoint.Name;
				if (!(name == "CreativeSacrificeConfirm"))
				{
					if (name == "CreativeInfinitesSearch")
					{
						uILinkPoint3 = _helper.MakeLinkPointFromSnapPoint(currentID++, snapPoint);
						Main.CreativeMenu.GamepadPointIdForInfiniteItemSearchHack = uILinkPoint3.ID;
					}
				}
				else
				{
					uILinkPoint2 = _helper.MakeLinkPointFromSnapPoint(currentID++, snapPoint);
				}
			}
			uILinkPoint = UILinkPointNavigator.Points[15000];
			List<SnapPoint> orderedPointsByCategoryName4 = _helper.GetOrderedPointsByCategoryName(snapPoints, "CreativeInfinitesFilter");
			UILinkPoint[] array4 = null;
			if (orderedPointsByCategoryName4.Count > 0)
			{
				array4 = _helper.CreateUILinkStripHorizontal(ref currentID, orderedPointsByCategoryName4);
				if (uILinkPoint3 != null)
				{
					uILinkPoint3.Up = array4[0].ID;
					for (int j = 0; j < array4.Length; j++)
					{
						array4[j].Down = uILinkPoint3.ID;
					}
				}
			}
			List<SnapPoint> orderedPointsByCategoryName5 = _helper.GetOrderedPointsByCategoryName(snapPoints, "CreativeInfinitesSlot");
			UILinkPoint[,] array5 = null;
			if (orderedPointsByCategoryName5.Count > 0)
			{
				array5 = _helper.CreateUILinkPointGrid(ref currentID, orderedPointsByCategoryName5, _infiniteItemsWindow.GetItemsPerLine(), uILinkPoint3, array[0], null, null);
				_helper.LinkVerticalStripRightSideToSingle(array, array5[0, 0]);
			}
			else if (uILinkPoint3 != null)
			{
				_helper.LinkVerticalStripRightSideToSingle(array, uILinkPoint3);
			}
			if (uILinkPoint3 != null && array5 != null)
			{
				_helper.PairUpDown(uILinkPoint3, array5[0, 0]);
			}
			if (uILinkPoint != null && IsShowingResearchMenu)
			{
				_helper.LinkVerticalStripRightSideToSingle(array, uILinkPoint);
			}
			if (uILinkPoint2 != null)
			{
				_helper.PairUpDown(uILinkPoint, uILinkPoint2);
				uILinkPoint2.Left = array[0].ID;
			}
			if (Main.CreativeMenu.GamepadMoveToSearchButtonHack)
			{
				Main.CreativeMenu.GamepadMoveToSearchButtonHack = false;
				if (uILinkPoint3 != null)
				{
					UILinkPointNavigator.ChangePoint(uILinkPoint3.ID);
				}
			}
		}
	}
}
