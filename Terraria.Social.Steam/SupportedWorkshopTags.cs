using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	public class SupportedWorkshopTags : AWorkshopTagsCollection
	{
		public SupportedWorkshopTags()
		{
			AddWorldTag("WorkshopTags.AdventureWorlds", "Adventure Worlds");
			AddWorldTag("WorkshopTags.GolfWorlds", "Golf Worlds");
			AddWorldTag("WorkshopTags.AllItemsWorlds", "All Items Worlds");
			AddWorldTag("WorkshopTags.StarterWorlds", "Starter Worlds");
			AddWorldTag("WorkshopTags.JourneyWorlds", "Journey Worlds");
			AddWorldTag("WorkshopTags.ClassicWorlds", "Classic Worlds");
			AddWorldTag("WorkshopTags.ExpertWorlds", "Expert Worlds");
			AddWorldTag("WorkshopTags.MasterWorlds", "Master Worlds");
			AddWorldTag("WorkshopTags.ChallengeWorlds", "Challenge Worlds");
			AddWorldTag("WorkshopTags.CorruptionWorlds", "Corruption Worlds");
			AddWorldTag("WorkshopTags.CrimsonWorlds", "Crimson Worlds");
			AddWorldTag("WorkshopTags.SmallWorlds", "Small Worlds");
			AddWorldTag("WorkshopTags.MediumWorlds", "Medium Worlds");
			AddWorldTag("WorkshopTags.LargeWorlds", "Large Worlds");
			AddWorldTag("WorkshopTags.OtherWorlds", "Other Worlds");
			AddResourcePackTag("WorkshopTags.FromTerrariaMods", "From Terraria Mods");
			AddResourcePackTag("WorkshopTags.PopularCulture", "Popular Culture");
			AddResourcePackTag("WorkshopTags.FunSilly", "Fun/Silly");
			AddResourcePackTag("WorkshopTags.Music", "Music");
			AddResourcePackTag("WorkshopTags.LanguageTranslations", "Language/Translations");
			AddResourcePackTag("WorkshopTags.HighResolution", "High Resolution");
			AddResourcePackTag("WorkshopTags.Overhaul", "Overhaul");
			AddResourcePackTag("WorkshopTags.Tweaks", "Tweaks");
			AddResourcePackTag("WorkshopTags.OtherResourcePacks", "Other Packs");
		}
	}
}
