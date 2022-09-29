using Terraria.Localization;

namespace Terraria.GameContent.Items
{
	public class ItemVariantCondition
	{
		public delegate bool Condition();

		public readonly NetworkText Description;

		public readonly Condition IsMet;

		public ItemVariantCondition(NetworkText description, Condition condition)
		{
			Description = description;
			IsMet = condition;
		}

		public override string ToString()
		{
			return Description.ToString();
		}
	}
}
