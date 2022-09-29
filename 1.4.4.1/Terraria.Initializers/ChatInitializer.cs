using Terraria.Chat.Commands;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Chat;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.Initializers
{
	public static class ChatInitializer
	{
		public static void Load()
		{
			ChatManager.Register<ColorTagHandler>(new string[2] { "c", "color" });
			ChatManager.Register<ItemTagHandler>(new string[2] { "i", "item" });
			ChatManager.Register<NameTagHandler>(new string[2] { "n", "name" });
			ChatManager.Register<AchievementTagHandler>(new string[2] { "a", "achievement" });
			ChatManager.Register<GlyphTagHandler>(new string[2] { "g", "glyph" });
			ChatManager.Commands.AddCommand<PartyChatCommand>().AddCommand<RollCommand>().AddCommand<EmoteCommand>()
				.AddCommand<ListPlayersCommand>()
				.AddCommand<RockPaperScissorsCommand>()
				.AddCommand<EmojiCommand>()
				.AddCommand<HelpCommand>()
				.AddCommand<DeathCommand>()
				.AddCommand<PVPDeathCommand>()
				.AddCommand<AllDeathCommand>()
				.AddCommand<AllPVPDeathCommand>()
				.AddDefaultCommand<SayChatCommand>();
			PrepareAliases();
		}

		public static void PrepareAliases()
		{
			ChatManager.Commands.ClearAliases();
			for (int i = 0; i < 151; i++)
			{
				string name = EmoteID.Search.GetName(i);
				string key = "EmojiCommand." + name;
				ChatManager.Commands.AddAlias(Language.GetText(key), NetworkText.FromFormattable("{0} {1}", Language.GetText("ChatCommand.Emoji_1"), Language.GetText("EmojiName." + name)));
			}
		}
	}
}
