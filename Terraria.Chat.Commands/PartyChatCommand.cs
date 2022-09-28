using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands
{
	[ChatCommand("Party")]
	public class PartyChatCommand : IChatCommand
	{
		private static readonly Color ERROR_COLOR = new Color(255, 240, 20);

		public void ProcessIncomingMessage(string text, byte clientId)
		{
			int team = Main.player[clientId].team;
			Color color = Main.teamColor[team];
			if (team == 0)
			{
				SendNoTeamError(clientId);
			}
			else
			{
				if (text == "")
				{
					return;
				}
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].team == team)
					{
						ChatHelper.SendChatMessageToClientAs(clientId, NetworkText.FromLiteral(text), color, i);
					}
				}
			}
		}

		public void ProcessOutgoingMessage(ChatMessage message)
		{
		}

		private void SendNoTeamError(byte clientId)
		{
			ChatHelper.SendChatMessageToClient(Lang.mp[10].ToNetworkText(), ERROR_COLOR, clientId);
		}
	}
}
