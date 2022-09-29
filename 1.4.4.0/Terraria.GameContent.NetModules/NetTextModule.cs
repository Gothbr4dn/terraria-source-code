using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.Net;
using Terraria.UI.Chat;

namespace Terraria.GameContent.NetModules
{
	public class NetTextModule : NetModule
	{
		public static NetPacket SerializeClientMessage(ChatMessage message)
		{
			NetPacket result = NetModule.CreatePacket<NetTextModule>(message.GetMaxSerializedSize());
			message.Serialize(result.Writer);
			return result;
		}

		public static NetPacket SerializeServerMessage(NetworkText text, Color color)
		{
			return SerializeServerMessage(text, color, byte.MaxValue);
		}

		public static NetPacket SerializeServerMessage(NetworkText text, Color color, byte authorId)
		{
			NetPacket result = NetModule.CreatePacket<NetTextModule>(1 + text.GetMaxSerializedSize() + 3);
			result.Writer.Write(authorId);
			text.Serialize(result.Writer);
			result.Writer.WriteRGB(color);
			return result;
		}

		private bool DeserializeAsClient(BinaryReader reader, int senderPlayerId)
		{
			byte messageAuthor = reader.ReadByte();
			NetworkText text = NetworkText.Deserialize(reader);
			Color color = reader.ReadRGB();
			ChatHelper.DisplayMessage(text, color, messageAuthor);
			return true;
		}

		private bool DeserializeAsServer(BinaryReader reader, int senderPlayerId)
		{
			ChatMessage message = ChatMessage.Deserialize(reader);
			ChatManager.Commands.ProcessIncomingMessage(message, senderPlayerId);
			return true;
		}

		public override bool Deserialize(BinaryReader reader, int senderPlayerId)
		{
			return DeserializeAsClient(reader, senderPlayerId);
		}
	}
}
