using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent.UI
{
	public class ItemRarity
	{
		private static Dictionary<int, Color> _rarities = new Dictionary<int, Color>();

		public static void Initialize()
		{
			_rarities.Clear();
			_rarities.Add(-11, Colors.RarityAmber);
			_rarities.Add(-1, Colors.RarityTrash);
			_rarities.Add(1, Colors.RarityBlue);
			_rarities.Add(2, Colors.RarityGreen);
			_rarities.Add(3, Colors.RarityOrange);
			_rarities.Add(4, Colors.RarityRed);
			_rarities.Add(5, Colors.RarityPink);
			_rarities.Add(6, Colors.RarityPurple);
			_rarities.Add(7, Colors.RarityLime);
			_rarities.Add(8, Colors.RarityYellow);
			_rarities.Add(9, Colors.RarityCyan);
		}

		public static Color GetColor(int rarity)
		{
			Color result = new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
			if (_rarities.ContainsKey(rarity))
			{
				return _rarities[rarity];
			}
			return result;
		}
	}
}
