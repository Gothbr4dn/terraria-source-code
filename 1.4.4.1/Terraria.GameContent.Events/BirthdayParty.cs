using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Achievements;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.Events
{
	public class BirthdayParty
	{
		public static bool ManualParty;

		public static bool GenuineParty;

		public static int PartyDaysOnCooldown;

		public static List<int> CelebratingNPCs = new List<int>();

		private static bool _wasCelebrating;

		public static bool PartyIsUp
		{
			get
			{
				if (!GenuineParty)
				{
					return ManualParty;
				}
				return true;
			}
		}

		public static void CheckMorning()
		{
			NaturalAttempt();
		}

		public static void CheckNight()
		{
			bool flag = false;
			if (GenuineParty)
			{
				flag = true;
				GenuineParty = false;
				CelebratingNPCs.Clear();
			}
			if (ManualParty)
			{
				flag = true;
				ManualParty = false;
			}
			if (flag)
			{
				WorldGen.BroadcastText(color: new Color(255, 0, 160), text: NetworkText.FromKey(Lang.misc[99].Key));
			}
		}

		private static bool CanNPCParty(NPC n)
		{
			if (!n.active || !n.townNPC || n.aiStyle == 0 || n.type == 37 || n.type == 453 || n.type == 441)
			{
				return false;
			}
			if (NPCID.Sets.IsTownPet[n.type])
			{
				return false;
			}
			return true;
		}

		private static void NaturalAttempt()
		{
			if (Main.netMode == 1 || !NPC.AnyNPCs(208))
			{
				return;
			}
			if (PartyDaysOnCooldown > 0)
			{
				PartyDaysOnCooldown--;
				return;
			}
			int maxValue = 10;
			if (Main.tenthAnniversaryWorld)
			{
				maxValue = 7;
			}
			if (Main.rand.Next(maxValue) != 0)
			{
				return;
			}
			List<NPC> list = new List<NPC>();
			for (int j = 0; j < 200; j++)
			{
				NPC nPC = Main.npc[j];
				if (CanNPCParty(nPC))
				{
					list.Add(nPC);
				}
			}
			if (list.Count >= 5)
			{
				GenuineParty = true;
				PartyDaysOnCooldown = Main.rand.Next(5, 11);
				NPC.freeCake = true;
				CelebratingNPCs.Clear();
				List<int> list2 = new List<int>();
				int num = 1;
				if (Main.rand.Next(5) == 0 && list.Count > 12)
				{
					num = 3;
				}
				else if (Main.rand.Next(3) == 0)
				{
					num = 2;
				}
				list = list.OrderBy((NPC i) => Main.rand.Next()).ToList();
				for (int k = 0; k < num; k++)
				{
					list2.Add(k);
				}
				for (int l = 0; l < list2.Count; l++)
				{
					CelebratingNPCs.Add(list[list2[l]].whoAmI);
				}
				Color color = new Color(255, 0, 160);
				if (CelebratingNPCs.Count == 3)
				{
					WorldGen.BroadcastText(NetworkText.FromKey("Game.BirthdayParty_3", Main.npc[CelebratingNPCs[0]].GetGivenOrTypeNetName(), Main.npc[CelebratingNPCs[1]].GetGivenOrTypeNetName(), Main.npc[CelebratingNPCs[2]].GetGivenOrTypeNetName()), color);
				}
				else if (CelebratingNPCs.Count == 2)
				{
					WorldGen.BroadcastText(NetworkText.FromKey("Game.BirthdayParty_2", Main.npc[CelebratingNPCs[0]].GetGivenOrTypeNetName(), Main.npc[CelebratingNPCs[1]].GetGivenOrTypeNetName()), color);
				}
				else
				{
					WorldGen.BroadcastText(NetworkText.FromKey("Game.BirthdayParty_1", Main.npc[CelebratingNPCs[0]].GetGivenOrTypeNetName()), color);
				}
				NetMessage.SendData(7);
				CheckForAchievement();
			}
		}

		public static void ToggleManualParty()
		{
			bool partyIsUp = PartyIsUp;
			if (Main.netMode != 1)
			{
				ManualParty = !ManualParty;
			}
			else
			{
				NetMessage.SendData(111);
			}
			if (partyIsUp != PartyIsUp)
			{
				if (Main.netMode == 2)
				{
					NetMessage.SendData(7);
				}
				CheckForAchievement();
			}
		}

		private static void CheckForAchievement()
		{
			if (PartyIsUp)
			{
				AchievementsHelper.NotifyProgressionEvent(25);
			}
		}

		public static void WorldClear()
		{
			ManualParty = false;
			GenuineParty = false;
			PartyDaysOnCooldown = 0;
			CelebratingNPCs.Clear();
			_wasCelebrating = false;
		}

		public static void UpdateTime()
		{
			if (_wasCelebrating != PartyIsUp)
			{
				if (Main.netMode != 2)
				{
					if (PartyIsUp)
					{
						SkyManager.Instance.Activate("Party", default(Vector2));
					}
					else
					{
						SkyManager.Instance.Deactivate("Party");
					}
				}
				if (Main.netMode != 1 && CelebratingNPCs.Count > 0)
				{
					for (int i = 0; i < CelebratingNPCs.Count; i++)
					{
						if (!CanNPCParty(Main.npc[CelebratingNPCs[i]]))
						{
							CelebratingNPCs.RemoveAt(i);
						}
					}
					if (CelebratingNPCs.Count == 0)
					{
						GenuineParty = false;
						if (!ManualParty)
						{
							WorldGen.BroadcastText(color: new Color(255, 0, 160), text: NetworkText.FromKey(Lang.misc[99].Key));
							NetMessage.SendData(7);
						}
					}
				}
			}
			_wasCelebrating = PartyIsUp;
		}
	}
}
