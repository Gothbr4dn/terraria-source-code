using System;
using System.IO;
using System.Text;
using Terraria.Chat.Commands;

namespace Terraria.Chat
{
	public sealed class ChatMessage
	{
		public ChatCommandId CommandId { get; private set; }

		public string Text { get; set; }

		public bool IsConsumed { get; private set; }

		public ChatMessage(string message)
		{
			CommandId = ChatCommandId.FromType<SayChatCommand>();
			Text = message;
			IsConsumed = false;
		}

		private ChatMessage(string message, ChatCommandId commandId)
		{
			CommandId = commandId;
			Text = message;
		}

		public void Serialize(BinaryWriter writer)
		{
			if (IsConsumed)
			{
				throw new InvalidOperationException("Message has already been consumed.");
			}
			CommandId.Serialize(writer);
			writer.Write(Text);
		}

		public int GetMaxSerializedSize()
		{
			if (IsConsumed)
			{
				throw new InvalidOperationException("Message has already been consumed.");
			}
			return 0 + CommandId.GetMaxSerializedSize() + (4 + Encoding.UTF8.GetByteCount(Text));
		}

		public static ChatMessage Deserialize(BinaryReader reader)
		{
			ChatCommandId commandId = ChatCommandId.Deserialize(reader);
			return new ChatMessage(reader.ReadString(), commandId);
		}

		public void SetCommand(ChatCommandId commandId)
		{
			if (IsConsumed)
			{
				throw new InvalidOperationException("Message has already been consumed.");
			}
			CommandId = commandId;
		}

		public void SetCommand<T>() where T : IChatCommand
		{
			if (IsConsumed)
			{
				throw new InvalidOperationException("Message has already been consumed.");
			}
			CommandId = ChatCommandId.FromType<T>();
		}

		public void Consume()
		{
			IsConsumed = true;
		}
	}
}
