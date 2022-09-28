namespace Terraria
{
	public struct ShoppingSettings
	{
		public double PriceAdjustment;

		public string HappinessReport;

		public static ShoppingSettings NotInShop
		{
			get
			{
				ShoppingSettings result = default(ShoppingSettings);
				result.PriceAdjustment = 1.0;
				result.HappinessReport = "";
				return result;
			}
		}
	}
}
