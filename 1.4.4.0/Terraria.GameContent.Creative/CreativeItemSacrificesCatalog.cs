using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria.ID;

namespace Terraria.GameContent.Creative
{
	public class CreativeItemSacrificesCatalog
	{
		public static CreativeItemSacrificesCatalog Instance = new CreativeItemSacrificesCatalog();

		private Dictionary<int, int> _sacrificeCountNeededByItemId = new Dictionary<int, int>();

		public Dictionary<int, int> SacrificeCountNeededByItemId => _sacrificeCountNeededByItemId;

		public void Initialize()
		{
			_sacrificeCountNeededByItemId.Clear();
			string[] array = Regex.Split(Utils.ReadEmbeddedResource("Terraria.GameContent.Creative.Content.Sacrifices.tsv"), "\r\n|\r|\n");
			int key = default(int);
			foreach (string text in array)
			{
				if (text.StartsWith("//"))
				{
					continue;
				}
				string[] array2 = text.Split(new char[1] { '\t' });
				if (array2.Length >= 3 && ItemID.Search.TryGetId(array2[0], ref key))
				{
					int value = 0;
					bool flag = false;
					string text2 = array2[1].ToLower();
					switch (text2)
					{
					case "":
					case "a":
						value = 50;
						break;
					case "b":
						value = 25;
						break;
					case "c":
						value = 5;
						break;
					case "d":
						value = 1;
						break;
					case "e":
						flag = true;
						break;
					case "f":
						value = 2;
						break;
					case "g":
						value = 3;
						break;
					case "h":
						value = 10;
						break;
					case "i":
						value = 15;
						break;
					case "j":
						value = 30;
						break;
					case "k":
						value = 99;
						break;
					case "l":
						value = 100;
						break;
					case "m":
						value = 200;
						break;
					case "n":
						value = 20;
						break;
					case "o":
						value = 400;
						break;
					default:
						throw new Exception("There is no category for this item: " + array2[0] + ", category: " + text2);
					}
					if (!flag)
					{
						_sacrificeCountNeededByItemId[key] = value;
					}
				}
			}
		}

		public bool TryGetSacrificeCountCapToUnlockInfiniteItems(int itemId, out int amountNeeded)
		{
			if (ContentSamples.CreativeResearchItemPersistentIdOverride.TryGetValue(itemId, out var value))
			{
				itemId = value;
			}
			return _sacrificeCountNeededByItemId.TryGetValue(itemId, out amountNeeded);
		}
	}
}
