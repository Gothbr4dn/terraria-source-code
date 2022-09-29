namespace Terraria.Chat
{
	public interface IChatProcessor
	{
		void ProcessIncomingMessage(ChatMessage message, int clientId);

		ChatMessage CreateOutgoingMessage(string text);
	}
}
