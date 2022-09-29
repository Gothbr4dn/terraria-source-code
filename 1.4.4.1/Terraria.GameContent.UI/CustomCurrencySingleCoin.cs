using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI
{
	public class CustomCurrencySingleCoin : CustomCurrencySystem
	{
		public float CurrencyDrawScale = 0.8f;

		public string CurrencyTextKey = "Currency.DefenderMedals";

		public Color CurrencyTextColor = new Color(240, 100, 120);

		public CustomCurrencySingleCoin(int coinItemID, long currencyCap)
		{
			Include(coinItemID, 1);
			SetCurrencyCap(currencyCap);
		}

		public override bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
		{
			List<Tuple<Point, Item>> cache = ItemCacheCreate(inv);
			long num = price;
			for (int i = 0; i < slotCoins.Count; i++)
			{
				Point item = slotCoins[i];
				long num2 = num;
				if (inv[item.X][item.Y].stack < num2)
				{
					num2 = inv[item.X][item.Y].stack;
				}
				num -= num2;
				inv[item.X][item.Y].stack -= (int)num2;
				if (inv[item.X][item.Y].stack == 0)
				{
					switch (item.X)
					{
					case 0:
						slotsEmpty.Add(item);
						break;
					case 1:
						slotEmptyBank.Add(item);
						break;
					case 2:
						slotEmptyBank2.Add(item);
						break;
					case 3:
						slotEmptyBank3.Add(item);
						break;
					case 4:
						slotEmptyBank4.Add(item);
						break;
					}
					slotCoins.Remove(item);
					i--;
				}
				if (num == 0L)
				{
					break;
				}
			}
			if (num != 0L)
			{
				ItemCacheRestore(cache, inv);
				return false;
			}
			return true;
		}

		public override void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
		{
			int num = _valuePerUnit.Keys.ElementAt(0);
			Main.instance.LoadItem(num);
			Texture2D value = TextureAssets.Item[num].get_Value();
			if (horizontal)
			{
				_ = 99;
				Vector2 position = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.get_Value(), text, Vector2.One).X + 45f, shopy + 50f);
				sb.Draw(value, position, null, Color.White, 0f, value.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
				Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.get_Value(), totalCoins.ToString(), position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
			}
			else
			{
				int num2 = ((totalCoins > 99) ? (-6) : 0);
				sb.Draw(value, new Vector2(shopx + 11f, shopy + 75f), null, Color.White, 0f, value.Size() / 2f, CurrencyDrawScale, SpriteEffects.None, 0f);
				Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.get_Value(), totalCoins.ToString(), shopx + (float)num2, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
			}
		}

		public override void GetPriceText(string[] lines, ref int currentLine, long price)
		{
			Color color = CurrencyTextColor * ((float)(int)Main.mouseTextColor / 255f);
			lines[currentLine++] = $"[c/{color.R:X2}{color.G:X2}{color.B:X2}:{Lang.tip[50].Value} {price} {Language.GetTextValue(CurrencyTextKey).ToLower()}]";
		}
	}
}
