namespace Terraria.Chat.Commands
{
	public interface IChatCommand
	{
		void ProcessIncomingMessage(string text, byte clientId);

		void ProcessOutgoingMessage(ChatMessage message);
	}
}
