using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class NPCNetIdBestiaryInfoElement : IBestiaryInfoElement, IBestiaryEntryDisplayIndex
	{
		public int NetId { get; private set; }

		public int BestiaryDisplayIndex => ContentSamples.NpcBestiarySortingId[NetId];

		public NPCNetIdBestiaryInfoElement(int npcNetId)
		{
			NetId = npcNetId;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}
	}
}
