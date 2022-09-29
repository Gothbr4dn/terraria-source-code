using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Creative
{
	public class CreativePowersHelper
	{
		public class CreativePowerIconLocations
		{
			public static readonly Point Unassigned = new Point(0, 0);

			public static readonly Point Deprecated = new Point(0, 0);

			public static readonly Point ItemDuplication = new Point(0, 0);

			public static readonly Point ItemResearch = new Point(1, 0);

			public static readonly Point TimeCategory = new Point(2, 0);

			public static readonly Point WeatherCategory = new Point(3, 0);

			public static readonly Point EnemyStrengthSlider = new Point(4, 0);

			public static readonly Point GameEvents = new Point(5, 0);

			public static readonly Point Godmode = new Point(6, 0);

			public static readonly Point BlockPlacementRange = new Point(7, 0);

			public static readonly Point StopBiomeSpread = new Point(8, 0);

			public static readonly Point EnemySpawnRate = new Point(9, 0);

			public static readonly Point FreezeTime = new Point(10, 0);

			public static readonly Point TimeDawn = new Point(11, 0);

			public static readonly Point TimeNoon = new Point(12, 0);

			public static readonly Point TimeDusk = new Point(13, 0);

			public static readonly Point TimeMidnight = new Point(14, 0);

			public static readonly Point WindDirection = new Point(15, 0);

			public static readonly Point WindFreeze = new Point(16, 0);

			public static readonly Point RainStrength = new Point(17, 0);

			public static readonly Point RainFreeze = new Point(18, 0);

			public static readonly Point ModifyTime = new Point(19, 0);

			public static readonly Point PersonalCategory = new Point(20, 0);
		}

		public const int TextureIconColumns = 21;

		public const int TextureIconRows = 1;

		public static Color CommonSelectedColor = new Color(152, 175, 235);

		private static Asset<Texture2D> GetPowerIconAsset(string path)
		{
			return Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
		}

		public static UIImageFramed GetIconImage(Point iconLocation)
		{
			Asset<Texture2D> powerIconAsset = GetPowerIconAsset("Images/UI/Creative/Infinite_Powers");
			return new UIImageFramed(powerIconAsset, powerIconAsset.Frame(21, 1, iconLocation.X, iconLocation.Y))
			{
				MarginLeft = 4f,
				MarginTop = 4f,
				VAlign = 0.5f,
				HAlign = 1f,
				IgnoresMouseInteraction = true
			};
		}

		public static GroupOptionButton<bool> CreateToggleButton(CreativePowerUIElementRequestInfo info)
		{
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, null, Color.White, null, 0.8f);
			groupOptionButton.Width = new StyleDimension(info.PreferredButtonWidth, 0f);
			groupOptionButton.Height = new StyleDimension(info.PreferredButtonHeight, 0f);
			groupOptionButton.ShowHighlightWhenSelected = false;
			groupOptionButton.SetCurrentOption(option: false);
			groupOptionButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
			groupOptionButton.SetColorsBasedOnSelectionState(Main.OurFavoriteColor, Colors.InventoryDefaultColor, 1f, 0.7f);
			return groupOptionButton;
		}

		public static GroupOptionButton<bool> CreateSimpleButton(CreativePowerUIElementRequestInfo info)
		{
			GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, null, Color.White, null, 0.8f);
			groupOptionButton.Width = new StyleDimension(info.PreferredButtonWidth, 0f);
			groupOptionButton.Height = new StyleDimension(info.PreferredButtonHeight, 0f);
			groupOptionButton.ShowHighlightWhenSelected = false;
			groupOptionButton.SetCurrentOption(option: false);
			groupOptionButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
			return groupOptionButton;
		}

		public static GroupOptionButton<T> CreateCategoryButton<T>(CreativePowerUIElementRequestInfo info, T option, T currentOption) where T : IConvertible, IEquatable<T>
		{
			GroupOptionButton<T> groupOptionButton = new GroupOptionButton<T>(option, null, null, Color.White, null, 0.8f);
			groupOptionButton.Width = new StyleDimension(info.PreferredButtonWidth, 0f);
			groupOptionButton.Height = new StyleDimension(info.PreferredButtonHeight, 0f);
			groupOptionButton.ShowHighlightWhenSelected = false;
			groupOptionButton.SetCurrentOption(currentOption);
			groupOptionButton.SetColorsBasedOnSelectionState(new Color(152, 175, 235), Colors.InventoryDefaultColor, 1f, 0.7f);
			return groupOptionButton;
		}

		public static void AddPermissionTextIfNeeded(ICreativePower power, ref string originalText)
		{
			if (!IsAvailableForPlayer(power, Main.myPlayer))
			{
				string textValue = Language.GetTextValue("CreativePowers.CantUsePowerBecauseOfNoPermissionFromServer");
				originalText = originalText + "\n" + textValue;
			}
		}

		public static void AddDescriptionIfNeeded(ref string originalText, string descriptionKey)
		{
			if (CreativePowerSettings.ShouldPowersBeElaborated)
			{
				string textValue = Language.GetTextValue(descriptionKey);
				originalText = originalText + "\n" + textValue;
			}
		}

		public static void AddUnlockTextIfNeeded(ref string originalText, bool needed, string descriptionKey)
		{
			if (!needed)
			{
				string textValue = Language.GetTextValue(descriptionKey);
				originalText = originalText + "\n" + textValue;
			}
		}

		public static UIVerticalSlider CreateSlider(Func<float> GetSliderValueMethod, Action<float> SetValueKeyboardMethod, Action SetValueGamepadMethod)
		{
			return new UIVerticalSlider(GetSliderValueMethod, SetValueKeyboardMethod, SetValueGamepadMethod, Color.Red)
			{
				Width = new StyleDimension(12f, 0f),
				Height = new StyleDimension(-10f, 1f),
				Left = new StyleDimension(6f, 0f),
				HAlign = 0f,
				VAlign = 0.5f,
				EmptyColor = Color.OrangeRed,
				FilledColor = Color.CornflowerBlue
			};
		}

		public static void UpdateUseMouseInterface(UIElement affectedElement)
		{
			if (affectedElement.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		public static void UpdateUnlockStateByPower(ICreativePower power, UIElement button, Color colorWhenSelected)
		{
			IGroupOptionButton asButton = button as IGroupOptionButton;
			if (asButton != null)
			{
				button.OnUpdate += delegate
				{
					UpdateUnlockStateByPowerInternal(power, colorWhenSelected, asButton);
				};
			}
		}

		public static bool IsAvailableForPlayer(ICreativePower power, int playerIndex)
		{
			switch (power.CurrentPermissionLevel)
			{
			default:
				return false;
			case PowerPermissionLevel.CanBeChangedByHostAlone:
				if (Main.netMode == 0)
				{
					return true;
				}
				return Main.countsAsHostForGameplay[playerIndex];
			case PowerPermissionLevel.CanBeChangedByEveryone:
				return true;
			}
		}

		private static void UpdateUnlockStateByPowerInternal(ICreativePower power, Color colorWhenSelected, IGroupOptionButton asButton)
		{
			bool isUnlocked = power.GetIsUnlocked();
			bool flag = !IsAvailableForPlayer(power, Main.myPlayer);
			asButton.SetBorderColor(flag ? Color.DimGray : Color.White);
			if (flag)
			{
				asButton.SetColorsBasedOnSelectionState(new Color(60, 60, 60), new Color(60, 60, 60), 0.7f, 0.7f);
			}
			else if (isUnlocked)
			{
				asButton.SetColorsBasedOnSelectionState(colorWhenSelected, Colors.InventoryDefaultColor, 1f, 0.7f);
			}
			else
			{
				asButton.SetColorsBasedOnSelectionState(Color.Crimson, Color.Red, 0.7f, 0.7f);
			}
		}
	}
}
