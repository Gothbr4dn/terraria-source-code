using System;
using System.Collections.Generic;

namespace Terraria
{
	public class RecipeGroup
	{
		public Func<string> GetText;

		public HashSet<int> ValidItems;

		public int IconicItemId;

		public int RegisteredId;

		public static Dictionary<int, RecipeGroup> recipeGroups = new Dictionary<int, RecipeGroup>();

		public static Dictionary<string, int> recipeGroupIDs = new Dictionary<string, int>();

		public static int nextRecipeGroupIndex;

		public RecipeGroup(Func<string> getName, params int[] validItems)
		{
			GetText = getName;
			ValidItems = new HashSet<int>(validItems);
			IconicItemId = validItems[0];
		}

		public static int RegisterGroup(string name, RecipeGroup rec)
		{
			int num = (rec.RegisteredId = nextRecipeGroupIndex++);
			recipeGroups.Add(num, rec);
			recipeGroupIDs.Add(name, num);
			return num;
		}

		public int CountUsableItems(Dictionary<int, int> itemStacksAvailable)
		{
			int num = 0;
			foreach (int validItem in ValidItems)
			{
				if (itemStacksAvailable.TryGetValue(validItem, out var value))
				{
					num += value;
				}
			}
			return num;
		}

		public int GetGroupFakeItemId()
		{
			return RegisteredId + 1000000;
		}
	}
}
