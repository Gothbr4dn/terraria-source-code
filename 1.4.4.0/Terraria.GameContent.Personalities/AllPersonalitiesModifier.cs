using System.Collections.Generic;

namespace Terraria.GameContent.Personalities
{
	public class AllPersonalitiesModifier : IShopPersonalityTrait
	{
		public void ModifyShopPrice(HelperInfo info, ShopHelper shopHelperInstance)
		{
			ModifyShopPrice_Relationships(info, shopHelperInstance);
		}

		private static void ModifyShopPrice_Relationships(HelperInfo info, ShopHelper shopHelperInstance)
		{
			bool[] nearbyNPCsByType = info.nearbyNPCsByType;
			if (info.npc.type == 663)
			{
				List<int> list = new List<int>();
				for (int i = 0; i < nearbyNPCsByType.Length; i++)
				{
					if (nearbyNPCsByType[i])
					{
						list.Add(i);
					}
				}
				for (int j = 0; j < 3; j++)
				{
					if (list.Count <= 0)
					{
						break;
					}
					int index = Main.rand.Next(list.Count);
					int npcType = list[index];
					list.RemoveAt(index);
					shopHelperInstance.LoveNPCByTypeName(npcType);
				}
			}
			if (info.npc.type != 663 && nearbyNPCsByType[663])
			{
				shopHelperInstance.LikePrincess();
			}
			switch (info.npc.type)
			{
			case 17:
				if (nearbyNPCsByType[588])
				{
					shopHelperInstance.LikeNPC(588);
				}
				if (nearbyNPCsByType[18])
				{
					shopHelperInstance.LikeNPC(18);
				}
				if (nearbyNPCsByType[441])
				{
					shopHelperInstance.DislikeNPC(441);
				}
				if (nearbyNPCsByType[369])
				{
					shopHelperInstance.HateNPC(369);
				}
				break;
			case 18:
				if (nearbyNPCsByType[19])
				{
					shopHelperInstance.LoveNPC(19);
				}
				if (nearbyNPCsByType[108])
				{
					shopHelperInstance.LikeNPC(108);
				}
				if (nearbyNPCsByType[208])
				{
					shopHelperInstance.DislikeNPC(208);
				}
				if (nearbyNPCsByType[20])
				{
					shopHelperInstance.DislikeNPC(20);
				}
				if (nearbyNPCsByType[633])
				{
					shopHelperInstance.HateNPC(633);
				}
				break;
			case 227:
				if (nearbyNPCsByType[20])
				{
					shopHelperInstance.LoveNPC(20);
				}
				if (nearbyNPCsByType[208])
				{
					shopHelperInstance.LikeNPC(208);
				}
				if (nearbyNPCsByType[209])
				{
					shopHelperInstance.DislikeNPC(209);
				}
				if (nearbyNPCsByType[160])
				{
					shopHelperInstance.DislikeNPC(160);
				}
				break;
			case 207:
				if (nearbyNPCsByType[19])
				{
					shopHelperInstance.LikeNPC(19);
				}
				if (nearbyNPCsByType[227])
				{
					shopHelperInstance.LikeNPC(227);
				}
				if (nearbyNPCsByType[178])
				{
					shopHelperInstance.DislikeNPC(178);
				}
				if (nearbyNPCsByType[229])
				{
					shopHelperInstance.HateNPC(229);
				}
				break;
			case 208:
				if (nearbyNPCsByType[108])
				{
					shopHelperInstance.LoveNPC(108);
				}
				if (nearbyNPCsByType[353])
				{
					shopHelperInstance.LikeNPC(353);
				}
				if (nearbyNPCsByType[17])
				{
					shopHelperInstance.DislikeNPC(17);
				}
				if (nearbyNPCsByType[441])
				{
					shopHelperInstance.HateNPC(441);
				}
				if (nearbyNPCsByType[633])
				{
					shopHelperInstance.LoveNPC(633);
				}
				break;
			case 369:
				if (nearbyNPCsByType[208])
				{
					shopHelperInstance.LikeNPC(208);
				}
				if (nearbyNPCsByType[38])
				{
					shopHelperInstance.LikeNPC(38);
				}
				if (nearbyNPCsByType[441])
				{
					shopHelperInstance.LikeNPC(441);
				}
				if (nearbyNPCsByType[550])
				{
					shopHelperInstance.HateNPC(550);
				}
				break;
			case 353:
				if (nearbyNPCsByType[207])
				{
					shopHelperInstance.LoveNPC(207);
				}
				if (nearbyNPCsByType[229])
				{
					shopHelperInstance.LikeNPC(229);
				}
				if (nearbyNPCsByType[550])
				{
					shopHelperInstance.DislikeNPC(550);
				}
				if (nearbyNPCsByType[107])
				{
					shopHelperInstance.HateNPC(107);
				}
				break;
			case 38:
				if (nearbyNPCsByType[550])
				{
					shopHelperInstance.LoveNPC(550);
				}
				if (nearbyNPCsByType[124])
				{
					shopHelperInstance.LikeNPC(124);
				}
				if (nearbyNPCsByType[107])
				{
					shopHelperInstance.DislikeNPC(107);
				}
				if (nearbyNPCsByType[19])
				{
					shopHelperInstance.DislikeNPC(19);
				}
				break;
			case 20:
				if (nearbyNPCsByType[228])
				{
					shopHelperInstance.LikeNPC(228);
				}
				if (nearbyNPCsByType[160])
				{
					shopHelperInstance.LikeNPC(160);
				}
				if (nearbyNPCsByType[369])
				{
					shopHelperInstance.DislikeNPC(369);
				}
				if (nearbyNPCsByType[588])
				{
					shopHelperInstance.HateNPC(588);
				}
				break;
			case 550:
				if (nearbyNPCsByType[38])
				{
					shopHelperInstance.LoveNPC(38);
				}
				if (nearbyNPCsByType[107])
				{
					shopHelperInstance.LikeNPC(107);
				}
				if (nearbyNPCsByType[22])
				{
					shopHelperInstance.DislikeNPC(22);
				}
				if (nearbyNPCsByType[207])
				{
					shopHelperInstance.HateNPC(207);
				}
				break;
			case 19:
				if (nearbyNPCsByType[18])
				{
					shopHelperInstance.LoveNPC(18);
				}
				if (nearbyNPCsByType[178])
				{
					shopHelperInstance.LikeNPC(178);
				}
				if (nearbyNPCsByType[588])
				{
					shopHelperInstance.DislikeNPC(588);
				}
				if (nearbyNPCsByType[38])
				{
					shopHelperInstance.HateNPC(38);
				}
				break;
			case 107:
				if (nearbyNPCsByType[124])
				{
					shopHelperInstance.LoveNPC(124);
				}
				if (nearbyNPCsByType[207])
				{
					shopHelperInstance.LikeNPC(207);
				}
				if (nearbyNPCsByType[54])
				{
					shopHelperInstance.DislikeNPC(54);
				}
				if (nearbyNPCsByType[353])
				{
					shopHelperInstance.HateNPC(353);
				}
				break;
			case 228:
				if (nearbyNPCsByType[20])
				{
					shopHelperInstance.LikeNPC(20);
				}
				if (nearbyNPCsByType[22])
				{
					shopHelperInstance.LikeNPC(22);
				}
				if (nearbyNPCsByType[18])
				{
					shopHelperInstance.DislikeNPC(18);
				}
				if (nearbyNPCsByType[160])
				{
					shopHelperInstance.HateNPC(160);
				}
				break;
			case 54:
				if (nearbyNPCsByType[160])
				{
					shopHelperInstance.LoveNPC(160);
				}
				if (nearbyNPCsByType[441])
				{
					shopHelperInstance.LikeNPC(441);
				}
				if (nearbyNPCsByType[18])
				{
					shopHelperInstance.DislikeNPC(18);
				}
				if (nearbyNPCsByType[124])
				{
					shopHelperInstance.HateNPC(124);
				}
				break;
			case 124:
				if (nearbyNPCsByType[107])
				{
					shopHelperInstance.LoveNPC(107);
				}
				if (nearbyNPCsByType[209])
				{
					shopHelperInstance.LikeNPC(209);
				}
				if (nearbyNPCsByType[19])
				{
					shopHelperInstance.DislikeNPC(19);
				}
				if (nearbyNPCsByType[54])
				{
					shopHelperInstance.HateNPC(54);
				}
				break;
			case 441:
				if (nearbyNPCsByType[17])
				{
					shopHelperInstance.LoveNPC(17);
				}
				if (nearbyNPCsByType[208])
				{
					shopHelperInstance.LikeNPC(208);
				}
				if (nearbyNPCsByType[38])
				{
					shopHelperInstance.DislikeNPC(38);
				}
				if (nearbyNPCsByType[124])
				{
					shopHelperInstance.DislikeNPC(124);
				}
				if (nearbyNPCsByType[142])
				{
					shopHelperInstance.HateNPC(142);
				}
				break;
			case 229:
				if (nearbyNPCsByType[369])
				{
					shopHelperInstance.LoveNPC(369);
				}
				if (nearbyNPCsByType[550])
				{
					shopHelperInstance.LikeNPC(550);
				}
				if (nearbyNPCsByType[353])
				{
					shopHelperInstance.DislikeNPC(353);
				}
				if (nearbyNPCsByType[22])
				{
					shopHelperInstance.HateNPC(22);
				}
				break;
			case 108:
				if (nearbyNPCsByType[588])
				{
					shopHelperInstance.LoveNPC(588);
				}
				if (nearbyNPCsByType[17])
				{
					shopHelperInstance.LikeNPC(17);
				}
				if (nearbyNPCsByType[228])
				{
					shopHelperInstance.DislikeNPC(228);
				}
				if (nearbyNPCsByType[209])
				{
					shopHelperInstance.HateNPC(209);
				}
				break;
			case 178:
				if (nearbyNPCsByType[209])
				{
					shopHelperInstance.LoveNPC(209);
				}
				if (nearbyNPCsByType[227])
				{
					shopHelperInstance.LikeNPC(227);
				}
				if (nearbyNPCsByType[208])
				{
					shopHelperInstance.DislikeNPC(208);
				}
				if (nearbyNPCsByType[108])
				{
					shopHelperInstance.DislikeNPC(108);
				}
				if (nearbyNPCsByType[20])
				{
					shopHelperInstance.DislikeNPC(20);
				}
				break;
			case 209:
				if (nearbyNPCsByType[353])
				{
					shopHelperInstance.LikeNPC(353);
				}
				if (nearbyNPCsByType[229])
				{
					shopHelperInstance.LikeNPC(229);
				}
				if (nearbyNPCsByType[178])
				{
					shopHelperInstance.LikeNPC(178);
				}
				if (nearbyNPCsByType[108])
				{
					shopHelperInstance.HateNPC(108);
				}
				if (nearbyNPCsByType[633])
				{
					shopHelperInstance.DislikeNPC(633);
				}
				break;
			case 142:
				if (nearbyNPCsByType[441])
				{
					shopHelperInstance.HateNPC(441);
				}
				break;
			case 588:
				if (nearbyNPCsByType[227])
				{
					shopHelperInstance.LikeNPC(227);
				}
				if (nearbyNPCsByType[369])
				{
					shopHelperInstance.LoveNPC(369);
				}
				if (nearbyNPCsByType[17])
				{
					shopHelperInstance.HateNPC(17);
				}
				if (nearbyNPCsByType[229])
				{
					shopHelperInstance.DislikeNPC(229);
				}
				if (nearbyNPCsByType[633])
				{
					shopHelperInstance.LikeNPC(633);
				}
				break;
			case 22:
				if (nearbyNPCsByType[54])
				{
					shopHelperInstance.LikeNPC(54);
				}
				if (nearbyNPCsByType[178])
				{
					shopHelperInstance.DislikeNPC(178);
				}
				if (nearbyNPCsByType[227])
				{
					shopHelperInstance.HateNPC(227);
				}
				if (nearbyNPCsByType[633])
				{
					shopHelperInstance.LikeNPC(633);
				}
				break;
			case 160:
				if (nearbyNPCsByType[22])
				{
					shopHelperInstance.LoveNPC(22);
				}
				if (nearbyNPCsByType[20])
				{
					shopHelperInstance.LikeNPC(20);
				}
				if (nearbyNPCsByType[54])
				{
					shopHelperInstance.DislikeNPC(54);
				}
				if (nearbyNPCsByType[228])
				{
					shopHelperInstance.HateNPC(228);
				}
				break;
			case 633:
				if (nearbyNPCsByType[369])
				{
					shopHelperInstance.DislikeNPC(369);
				}
				if (nearbyNPCsByType[19])
				{
					shopHelperInstance.HateNPC(19);
				}
				if (nearbyNPCsByType[228])
				{
					shopHelperInstance.LoveNPC(228);
				}
				if (nearbyNPCsByType[588])
				{
					shopHelperInstance.LikeNPC(588);
				}
				break;
			}
		}
	}
}
