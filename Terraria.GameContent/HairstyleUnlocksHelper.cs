using System.Collections.Generic;

namespace Terraria.GameContent
{
	public class HairstyleUnlocksHelper
	{
		public List<int> AvailableHairstyles = new List<int>();

		private bool _defeatedMartians;

		private bool _defeatedMoonlord;

		private bool _defeatedPlantera;

		private bool _isAtStylist;

		private bool _isAtCharacterCreation;

		public void UpdateUnlocks()
		{
			if (ListWarrantsRemake())
			{
				RebuildList();
			}
		}

		private bool ListWarrantsRemake()
		{
			bool flag = NPC.downedMartians && !Main.gameMenu;
			bool flag2 = NPC.downedMoonlord && !Main.gameMenu;
			bool flag3 = NPC.downedPlantBoss && !Main.gameMenu;
			bool flag4 = Main.hairWindow && !Main.gameMenu;
			bool gameMenu = Main.gameMenu;
			bool result = false;
			if (_defeatedMartians != flag || _defeatedMoonlord != flag2 || _defeatedPlantera != flag3 || _isAtStylist != flag4 || _isAtCharacterCreation != gameMenu)
			{
				result = true;
			}
			_defeatedMartians = flag;
			_defeatedMoonlord = flag2;
			_defeatedPlantera = flag3;
			_isAtStylist = flag4;
			_isAtCharacterCreation = gameMenu;
			return result;
		}

		private void RebuildList()
		{
			List<int> availableHairstyles = AvailableHairstyles;
			availableHairstyles.Clear();
			if (_isAtCharacterCreation || _isAtStylist)
			{
				for (int i = 0; i < 51; i++)
				{
					availableHairstyles.Add(i);
				}
				availableHairstyles.Add(136);
				availableHairstyles.Add(137);
				availableHairstyles.Add(138);
				availableHairstyles.Add(139);
				availableHairstyles.Add(140);
				availableHairstyles.Add(141);
				availableHairstyles.Add(142);
				availableHairstyles.Add(143);
				availableHairstyles.Add(144);
				availableHairstyles.Add(147);
				availableHairstyles.Add(148);
				availableHairstyles.Add(149);
				availableHairstyles.Add(150);
				availableHairstyles.Add(151);
				availableHairstyles.Add(154);
				availableHairstyles.Add(155);
				availableHairstyles.Add(157);
				availableHairstyles.Add(158);
				availableHairstyles.Add(161);
			}
			for (int j = 51; j < 123; j++)
			{
				availableHairstyles.Add(j);
			}
			availableHairstyles.Add(134);
			availableHairstyles.Add(135);
			availableHairstyles.Add(146);
			availableHairstyles.Add(152);
			availableHairstyles.Add(153);
			availableHairstyles.Add(156);
			availableHairstyles.Add(159);
			availableHairstyles.Add(160);
			if (_defeatedPlantera)
			{
				availableHairstyles.Add(162);
				availableHairstyles.Add(164);
				availableHairstyles.Add(163);
				availableHairstyles.Add(145);
			}
			if (_defeatedMartians)
			{
				availableHairstyles.AddRange(new int[10] { 132, 131, 130, 129, 128, 127, 126, 125, 124, 123 });
			}
			if (_defeatedMartians && _defeatedMoonlord)
			{
				availableHairstyles.Add(133);
			}
		}
	}
}
