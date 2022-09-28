using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.UI
{
	public class CustomCurrencyManager
	{
		private static int _nextCurrencyIndex;

		private static Dictionary<int, CustomCurrencySystem> _currencies = new Dictionary<int, CustomCurrencySystem>();

		public static void Initialize()
		{
			_nextCurrencyIndex = 0;
			CustomCurrencyID.DefenderMedals = RegisterCurrency(new CustomCurrencySingleCoin(3817, 999L));
		}

		public static int RegisterCurrency(CustomCurrencySystem collection)
		{
			int nextCurrencyIndex = _nextCurrencyIndex;
			_nextCurrencyIndex++;
			_currencies[nextCurrencyIndex] = collection;
			return nextCurrencyIndex;
		}

		public static void DrawSavings(SpriteBatch sb, int currencyIndex, float shopx, float shopy, bool horizontal = false)
		{
			CustomCurrencySystem customCurrencySystem = _currencies[currencyIndex];
			Player player = Main.player[Main.myPlayer];
			bool overFlowing;
			long num = customCurrencySystem.CountCurrency(out overFlowing, player.bank.item);
			long num2 = customCurrencySystem.CountCurrency(out overFlowing, player.bank2.item);
			long num3 = customCurrencySystem.CountCurrency(out overFlowing, player.bank3.item);
			long num4 = customCurrencySystem.CountCurrency(out overFlowing, player.bank4.item);
			long num5 = customCurrencySystem.CombineStacks(out overFlowing, num, num2, num3, num4);
			if (num5 > 0)
			{
				Main.GetItemDrawFrame(4076, out var itemTexture, out var itemFrame);
				Main.GetItemDrawFrame(3813, out var itemTexture2, out var itemFrame2);
				Main.GetItemDrawFrame(346, out var itemTexture3, out var itemFrame3);
				Main.GetItemDrawFrame(87, out var itemTexture4, out var itemFrame4);
				if (num4 > 0)
				{
					sb.Draw(itemTexture, Utils.CenteredRectangle(new Vector2(shopx + 96f, shopy + 50f), itemFrame.Size() * 0.65f), null, Color.White);
				}
				if (num3 > 0)
				{
					sb.Draw(itemTexture2, Utils.CenteredRectangle(new Vector2(shopx + 80f, shopy + 50f), itemFrame2.Size() * 0.65f), null, Color.White);
				}
				if (num2 > 0)
				{
					sb.Draw(itemTexture3, Utils.CenteredRectangle(new Vector2(shopx + 80f, shopy + 50f), itemFrame3.Size() * 0.65f), null, Color.White);
				}
				if (num > 0)
				{
					sb.Draw(itemTexture4, Utils.CenteredRectangle(new Vector2(shopx + 70f, shopy + 60f), itemFrame4.Size() * 0.65f), null, Color.White);
				}
				Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.get_Value(), Lang.inter[66].Value, shopx, shopy + 40f, Color.White * ((float)(int)Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
				customCurrencySystem.DrawSavingsMoney(sb, Lang.inter[66].Value, shopx, shopy, num5, horizontal);
			}
		}

		public static void GetPriceText(int currencyIndex, string[] lines, ref int currentLine, long price)
		{
			_currencies[currencyIndex].GetPriceText(lines, ref currentLine, price);
		}

		public static bool BuyItem(Player player, long price, int currencyIndex)
		{
			CustomCurrencySystem customCurrencySystem = _currencies[currencyIndex];
			bool overFlowing;
			long num = customCurrencySystem.CountCurrency(out overFlowing, player.inventory, 58, 57, 56, 55, 54);
			long num2 = customCurrencySystem.CountCurrency(out overFlowing, player.bank.item);
			long num3 = customCurrencySystem.CountCurrency(out overFlowing, player.bank2.item);
			long num4 = customCurrencySystem.CountCurrency(out overFlowing, player.bank3.item);
			long num5 = customCurrencySystem.CountCurrency(out overFlowing, player.bank4.item);
			if (customCurrencySystem.CombineStacks(out overFlowing, num, num2, num3, num4, num5) < price)
			{
				return false;
			}
			List<Item[]> list = new List<Item[]>();
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			List<Point> list2 = new List<Point>();
			List<Point> list3 = new List<Point>();
			List<Point> list4 = new List<Point>();
			List<Point> list5 = new List<Point>();
			List<Point> list6 = new List<Point>();
			List<Point> list7 = new List<Point>();
			list.Add(player.inventory);
			list.Add(player.bank.item);
			list.Add(player.bank2.item);
			list.Add(player.bank3.item);
			list.Add(player.bank4.item);
			for (int i = 0; i < list.Count; i++)
			{
				dictionary[i] = new List<int>();
			}
			dictionary[0] = new List<int> { 58, 57, 56, 55, 54 };
			for (int j = 0; j < list.Count; j++)
			{
				for (int k = 0; k < list[j].Length; k++)
				{
					if (!dictionary[j].Contains(k) && customCurrencySystem.Accepts(list[j][k]))
					{
						list3.Add(new Point(j, k));
					}
				}
			}
			FindEmptySlots(list, dictionary, list2, 0);
			FindEmptySlots(list, dictionary, list4, 1);
			FindEmptySlots(list, dictionary, list5, 2);
			FindEmptySlots(list, dictionary, list6, 3);
			FindEmptySlots(list, dictionary, list7, 4);
			if (!customCurrencySystem.TryPurchasing(price, list, list3, list2, list4, list5, list6, list7))
			{
				return false;
			}
			return true;
		}

		private static void FindEmptySlots(List<Item[]> inventories, Dictionary<int, List<int>> slotsToIgnore, List<Point> emptySlots, int currentInventoryIndex)
		{
			for (int num = inventories[currentInventoryIndex].Length - 1; num >= 0; num--)
			{
				if (!slotsToIgnore[currentInventoryIndex].Contains(num) && (inventories[currentInventoryIndex][num].type == 0 || inventories[currentInventoryIndex][num].stack == 0))
				{
					emptySlots.Add(new Point(currentInventoryIndex, num));
				}
			}
		}

		public static bool IsCustomCurrency(Item item)
		{
			foreach (KeyValuePair<int, CustomCurrencySystem> currency in _currencies)
			{
				if (currency.Value.Accepts(item))
				{
					return true;
				}
			}
			return false;
		}

		public static void GetPrices(Item item, out long calcForSelling, out long calcForBuying)
		{
			_currencies[item.shopSpecialCurrency].GetItemExpectedPrice(item, out calcForSelling, out calcForBuying);
		}
	}
}
