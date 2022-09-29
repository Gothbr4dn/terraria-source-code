using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.Bestiary
{
	public class BestiaryEntry
	{
		public IEntryIcon Icon;

		public IBestiaryUICollectionInfoProvider UIInfoProvider;

		public List<IBestiaryInfoElement> Info { get; private set; }

		public BestiaryEntry()
		{
			Info = new List<IBestiaryInfoElement>();
		}

		public static BestiaryEntry Enemy(int npcNetId)
		{
			NPC nPC = ContentSamples.NpcsByNetId[npcNetId];
			List<IBestiaryInfoElement> list = new List<IBestiaryInfoElement>
			{
				new NPCNetIdBestiaryInfoElement(npcNetId),
				new NamePlateInfoElement(Lang.GetNPCName(npcNetId).Key, npcNetId),
				new NPCPortraitInfoElement(ContentSamples.NpcBestiaryRarityStars[npcNetId]),
				new NPCKillCounterInfoElement(npcNetId)
			};
			list.Add(new NPCStatsReportInfoElement(npcNetId));
			if (nPC.rarity != 0)
			{
				list.Add(new RareSpawnBestiaryInfoElement(nPC.rarity));
			}
			IBestiaryUICollectionInfoProvider uIInfoProvider;
			if (nPC.boss || NPCID.Sets.ShouldBeCountedAsBoss[nPC.type])
			{
				list.Add(new BossBestiaryInfoElement());
				uIInfoProvider = new CommonEnemyUICollectionInfoProvider(nPC.GetBestiaryCreditId(), quickUnlock: true);
			}
			else
			{
				uIInfoProvider = new CommonEnemyUICollectionInfoProvider(nPC.GetBestiaryCreditId(), quickUnlock: false);
			}
			string key = Lang.GetNPCName(nPC.netID).Key;
			key = key.Replace("NPCName.", "");
			string text = "Bestiary_FlavorText.npc_" + key;
			if (Language.Exists(text))
			{
				list.Add(new FlavorTextBestiaryInfoElement(text));
			}
			return new BestiaryEntry
			{
				Icon = new UnlockableNPCEntryIcon(npcNetId),
				Info = list,
				UIInfoProvider = uIInfoProvider
			};
		}

		public static BestiaryEntry TownNPC(int npcNetId)
		{
			NPC nPC = ContentSamples.NpcsByNetId[npcNetId];
			List<IBestiaryInfoElement> list = new List<IBestiaryInfoElement>
			{
				new NPCNetIdBestiaryInfoElement(npcNetId),
				new NamePlateInfoElement(Lang.GetNPCName(npcNetId).Key, npcNetId),
				new NPCPortraitInfoElement(ContentSamples.NpcBestiaryRarityStars[npcNetId]),
				new NPCKillCounterInfoElement(npcNetId)
			};
			string key = Lang.GetNPCName(nPC.netID).Key;
			key = key.Replace("NPCName.", "");
			string text = "Bestiary_FlavorText.npc_" + key;
			if (Language.Exists(text))
			{
				list.Add(new FlavorTextBestiaryInfoElement(text));
			}
			return new BestiaryEntry
			{
				Icon = new UnlockableNPCEntryIcon(npcNetId),
				Info = list,
				UIInfoProvider = new TownNPCUICollectionInfoProvider(nPC.GetBestiaryCreditId())
			};
		}

		public static BestiaryEntry Critter(int npcNetId)
		{
			NPC nPC = ContentSamples.NpcsByNetId[npcNetId];
			List<IBestiaryInfoElement> list = new List<IBestiaryInfoElement>
			{
				new NPCNetIdBestiaryInfoElement(npcNetId),
				new NamePlateInfoElement(Lang.GetNPCName(npcNetId).Key, npcNetId),
				new NPCPortraitInfoElement(ContentSamples.NpcBestiaryRarityStars[npcNetId]),
				new NPCKillCounterInfoElement(npcNetId)
			};
			string key = Lang.GetNPCName(nPC.netID).Key;
			key = key.Replace("NPCName.", "");
			string text = "Bestiary_FlavorText.npc_" + key;
			if (Language.Exists(text))
			{
				list.Add(new FlavorTextBestiaryInfoElement(text));
			}
			return new BestiaryEntry
			{
				Icon = new UnlockableNPCEntryIcon(npcNetId),
				Info = list,
				UIInfoProvider = new CritterUICollectionInfoProvider(nPC.GetBestiaryCreditId())
			};
		}

		public static BestiaryEntry Biome(string nameLanguageKey, string texturePath, Func<bool> unlockCondition)
		{
			return new BestiaryEntry
			{
				Icon = new CustomEntryIcon(nameLanguageKey, texturePath, unlockCondition),
				Info = new List<IBestiaryInfoElement>()
			};
		}

		public void AddTags(params IBestiaryInfoElement[] elements)
		{
			Info.AddRange(elements);
		}
	}
}
