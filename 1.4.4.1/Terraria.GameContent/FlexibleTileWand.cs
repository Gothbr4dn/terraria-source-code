using System.Collections.Generic;
using Terraria.Utilities;

namespace Terraria.GameContent
{
	public class FlexibleTileWand
	{
		private class OptionBucket
		{
			public int ItemTypeToConsume;

			public List<PlacementOption> Options;

			public OptionBucket(int itemTypeToConsume)
			{
				ItemTypeToConsume = itemTypeToConsume;
				Options = new List<PlacementOption>();
			}

			public PlacementOption GetRandomOption(UnifiedRandom random)
			{
				return Options[random.Next(Options.Count)];
			}

			public PlacementOption GetOptionWithCycling(int cycleOffset)
			{
				int count = Options.Count;
				int index = (cycleOffset % count + count) % count;
				return Options[index];
			}
		}

		public class PlacementOption
		{
			public int TileIdToPlace;

			public int TileStyleToPlace;
		}

		public static FlexibleTileWand RubblePlacementSmall = CreateRubblePlacerSmall();

		public static FlexibleTileWand RubblePlacementMedium = CreateRubblePlacerMedium();

		public static FlexibleTileWand RubblePlacementLarge = CreateRubblePlacerLarge();

		private UnifiedRandom _random = new UnifiedRandom();

		private Dictionary<int, OptionBucket> _options = new Dictionary<int, OptionBucket>();

		public void AddVariation(int itemType, int tileIdToPlace, int tileStyleToPlace)
		{
			if (!_options.TryGetValue(itemType, out var value))
			{
				OptionBucket optionBucket2 = (_options[itemType] = new OptionBucket(itemType));
				value = optionBucket2;
			}
			value.Options.Add(new PlacementOption
			{
				TileIdToPlace = tileIdToPlace,
				TileStyleToPlace = tileStyleToPlace
			});
		}

		public void AddVariations(int itemType, int tileIdToPlace, params int[] stylesToPlace)
		{
			foreach (int tileStyleToPlace in stylesToPlace)
			{
				AddVariation(itemType, tileIdToPlace, tileStyleToPlace);
			}
		}

		public void AddVariations_ByRow(int itemType, int tileIdToPlace, int variationsPerRow, params int[] rows)
		{
			for (int i = 0; i < rows.Length; i++)
			{
				for (int j = 0; j < variationsPerRow; j++)
				{
					int tileStyleToPlace = rows[i] * variationsPerRow + j;
					AddVariation(itemType, tileIdToPlace, tileStyleToPlace);
				}
			}
		}

		public bool TryGetPlacementOption(Player player, int randomSeed, int selectCycleOffset, out PlacementOption option, out Item itemToConsume)
		{
			option = null;
			itemToConsume = null;
			Item[] inventory = player.inventory;
			for (int i = 0; i < 58; i++)
			{
				if (i < 50 || i >= 54)
				{
					Item item = inventory[i];
					if (!item.IsAir && _options.TryGetValue(item.type, out var value))
					{
						_random.SetSeed(randomSeed);
						option = value.GetOptionWithCycling(selectCycleOffset);
						itemToConsume = item;
						return true;
					}
				}
			}
			return false;
		}

		public static FlexibleTileWand CreateRubblePlacerLarge()
		{
			FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
			int tileIdToPlace = 647;
			flexibleTileWand.AddVariations(154, tileIdToPlace, 0, 1, 2, 3, 4, 5, 6);
			flexibleTileWand.AddVariations(3, tileIdToPlace, 7, 8, 9, 10, 11, 12, 13, 14, 15);
			flexibleTileWand.AddVariations(71, tileIdToPlace, 16, 17);
			flexibleTileWand.AddVariations(72, tileIdToPlace, 18, 19);
			flexibleTileWand.AddVariations(73, tileIdToPlace, 20, 21);
			flexibleTileWand.AddVariations(9, tileIdToPlace, 22, 23, 24, 25);
			flexibleTileWand.AddVariations(593, tileIdToPlace, 26, 27, 28, 29, 30, 31);
			flexibleTileWand.AddVariations(183, tileIdToPlace, 32, 33, 34);
			tileIdToPlace = 648;
			flexibleTileWand.AddVariations(195, tileIdToPlace, 0, 1, 2);
			flexibleTileWand.AddVariations(195, tileIdToPlace, 3, 4, 5);
			flexibleTileWand.AddVariations(174, tileIdToPlace, 6, 7, 8);
			flexibleTileWand.AddVariations(150, tileIdToPlace, 9, 10, 11, 12, 13);
			flexibleTileWand.AddVariations(3, tileIdToPlace, 14, 15, 16);
			flexibleTileWand.AddVariations(989, tileIdToPlace, 17);
			flexibleTileWand.AddVariations(1101, tileIdToPlace, 18, 19, 20);
			flexibleTileWand.AddVariations(9, tileIdToPlace, 21, 22);
			flexibleTileWand.AddVariations(9, tileIdToPlace, 23, 24, 25, 26, 27, 28);
			flexibleTileWand.AddVariations(3271, tileIdToPlace, 29, 30, 31, 32, 33, 34);
			flexibleTileWand.AddVariations(3086, tileIdToPlace, 35, 36, 37, 38, 39, 40);
			flexibleTileWand.AddVariations(3081, tileIdToPlace, 41, 42, 43, 44, 45, 46);
			flexibleTileWand.AddVariations(62, tileIdToPlace, 47, 48, 49);
			flexibleTileWand.AddVariations(62, tileIdToPlace, 50, 51);
			flexibleTileWand.AddVariations(154, tileIdToPlace, 52, 53, 54);
			tileIdToPlace = 651;
			flexibleTileWand.AddVariations(195, tileIdToPlace, 0, 1, 2);
			flexibleTileWand.AddVariations(62, tileIdToPlace, 3, 4, 5);
			flexibleTileWand.AddVariations(331, tileIdToPlace, 6, 7, 8);
			return flexibleTileWand;
		}

		public static FlexibleTileWand CreateRubblePlacerMedium()
		{
			FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
			ushort tileIdToPlace = 652;
			flexibleTileWand.AddVariations(195, tileIdToPlace, 0, 1, 2);
			flexibleTileWand.AddVariations(62, tileIdToPlace, 3, 4, 5);
			flexibleTileWand.AddVariations(331, tileIdToPlace, 6, 7, 8, 9, 10, 11);
			tileIdToPlace = 649;
			flexibleTileWand.AddVariations(3, tileIdToPlace, 0, 1, 2, 3, 4, 5);
			flexibleTileWand.AddVariations(154, tileIdToPlace, 6, 7, 8, 9, 10);
			flexibleTileWand.AddVariations(154, tileIdToPlace, 11, 12, 13, 14, 15);
			flexibleTileWand.AddVariations(71, tileIdToPlace, 16);
			flexibleTileWand.AddVariations(72, tileIdToPlace, 17);
			flexibleTileWand.AddVariations(73, tileIdToPlace, 18);
			flexibleTileWand.AddVariations(181, tileIdToPlace, 19);
			flexibleTileWand.AddVariations(180, tileIdToPlace, 20);
			flexibleTileWand.AddVariations(177, tileIdToPlace, 21);
			flexibleTileWand.AddVariations(179, tileIdToPlace, 22);
			flexibleTileWand.AddVariations(178, tileIdToPlace, 23);
			flexibleTileWand.AddVariations(182, tileIdToPlace, 24);
			flexibleTileWand.AddVariations(593, tileIdToPlace, 25, 26, 27, 28, 29, 30);
			flexibleTileWand.AddVariations(9, tileIdToPlace, 31, 32, 33);
			flexibleTileWand.AddVariations(150, tileIdToPlace, 34, 35, 36, 37);
			flexibleTileWand.AddVariations(3, tileIdToPlace, 38, 39, 40);
			flexibleTileWand.AddVariations(3271, tileIdToPlace, 41, 42, 43, 44, 45, 46);
			flexibleTileWand.AddVariations(3086, tileIdToPlace, 47, 48, 49, 50, 51, 52);
			flexibleTileWand.AddVariations(3081, tileIdToPlace, 53, 54, 55, 56, 57, 58);
			flexibleTileWand.AddVariations(62, tileIdToPlace, 59, 60, 61);
			flexibleTileWand.AddVariations(169, tileIdToPlace, 62, 63, 64);
			return flexibleTileWand;
		}

		public static FlexibleTileWand CreateRubblePlacerSmall()
		{
			FlexibleTileWand flexibleTileWand = new FlexibleTileWand();
			ushort tileIdToPlace = 650;
			flexibleTileWand.AddVariations(3, tileIdToPlace, 0, 1, 2, 3, 4, 5);
			flexibleTileWand.AddVariations(2, tileIdToPlace, 6, 7, 8, 9, 10, 11);
			flexibleTileWand.AddVariations(154, tileIdToPlace, 12, 13, 14, 15, 16, 17, 18, 19);
			flexibleTileWand.AddVariations(154, tileIdToPlace, 20, 21, 22, 23, 24, 25, 26, 27);
			flexibleTileWand.AddVariations(9, tileIdToPlace, 28, 29, 30, 31, 32);
			flexibleTileWand.AddVariations(9, tileIdToPlace, 33, 34, 35);
			flexibleTileWand.AddVariations(593, tileIdToPlace, 36, 37, 38, 39, 40, 41);
			flexibleTileWand.AddVariations(664, tileIdToPlace, 42, 43, 44, 45, 46, 47);
			flexibleTileWand.AddVariations(150, tileIdToPlace, 48, 49, 50, 51, 52, 53);
			flexibleTileWand.AddVariations(3271, tileIdToPlace, 54, 55, 56, 57, 58, 59);
			flexibleTileWand.AddVariations(3086, tileIdToPlace, 60, 61, 62, 63, 64, 65);
			flexibleTileWand.AddVariations(3081, tileIdToPlace, 66, 67, 68, 69, 70, 71);
			flexibleTileWand.AddVariations(62, tileIdToPlace, 72);
			flexibleTileWand.AddVariations(169, tileIdToPlace, 73, 74, 75, 76);
			return flexibleTileWand;
		}

		public static void ForModders_AddPotsToWand(FlexibleTileWand wand, ref int echoPileStyle, ref ushort tileType)
		{
			int variationsPerRow = 3;
			echoPileStyle = 0;
			tileType = 653;
			wand.AddVariations_ByRow(133, tileType, variationsPerRow, 0, 1, 2, 3);
			wand.AddVariations_ByRow(664, tileType, variationsPerRow, 4, 5, 6);
			wand.AddVariations_ByRow(176, tileType, variationsPerRow, 7, 8, 9);
			wand.AddVariations_ByRow(154, tileType, variationsPerRow, 10, 11, 12);
			wand.AddVariations_ByRow(173, tileType, variationsPerRow, 13, 14, 15);
			wand.AddVariations_ByRow(61, tileType, variationsPerRow, 16, 17, 18);
			wand.AddVariations_ByRow(150, tileType, variationsPerRow, 19, 20, 21);
			wand.AddVariations_ByRow(836, tileType, variationsPerRow, 22, 23, 24);
			wand.AddVariations_ByRow(607, tileType, variationsPerRow, 25, 26, 27);
			wand.AddVariations_ByRow(1101, tileType, variationsPerRow, 28, 29, 30);
			wand.AddVariations_ByRow(3081, tileType, variationsPerRow, 31, 32, 33);
			wand.AddVariations_ByRow(607, tileType, variationsPerRow, 34, 35, 36);
		}
	}
}
