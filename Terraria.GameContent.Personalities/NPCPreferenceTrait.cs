namespace Terraria.GameContent.Personalities
{
	public class NPCPreferenceTrait : IShopPersonalityTrait
	{
		public AffectionLevel Level;

		public int NpcId;

		public void ModifyShopPrice(HelperInfo info, ShopHelper shopHelperInstance)
		{
			if (info.nearbyNPCsByType[NpcId])
			{
				switch (Level)
				{
				case AffectionLevel.Love:
					shopHelperInstance.LoveNPC(NpcId);
					break;
				case AffectionLevel.Like:
					shopHelperInstance.LikeNPC(NpcId);
					break;
				case AffectionLevel.Dislike:
					shopHelperInstance.DislikeNPC(NpcId);
					break;
				case AffectionLevel.Hate:
					shopHelperInstance.HateNPC(NpcId);
					break;
				}
			}
		}
	}
}
