using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	[ChatCommand("AllPVPDeath")]
	public class AllPVPDeathCommand : IChatCommand
	{
		private static readonly Color RESPONSE_COLOR = new Color(255, 25, 25);

		public void ProcessIncomingMessage(string text, byte clientId)
		{
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i] != null && Main.player[i].active)
				{
					NetworkText text2 = NetworkText.FromKey("LegacyMultiplayer.24", Main.player[i].name, Main.player[i].numberOfDeathsPVP);
					if (Main.player[clientId].numberOfDeathsPVP == 1)
					{
						text2 = NetworkText.FromKey("LegacyMultiplayer.26", Main.player[clientId].name, Main.player[clientId].numberOfDeathsPVP);
					}
					ChatHelper.BroadcastChatMessage(text2, RESPONSE_COLOR);
				}
			}
		}

		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}
	}
}
