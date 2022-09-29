using Terraria.Localization;

namespace Terraria.GameContent.Items
{
	public class ItemVariant
	{
		public readonly NetworkText Description;

		public ItemVariant(NetworkText description)
		{
			Description = description;
		}

		public override string ToString()
		{
			return Description.ToString();
		}
	}
}
