using System.Collections.Generic;
using System.Linq;
using ReLogic.Utilities;
using Terraria.Chat.Commands;
using Terraria.Localization;

namespace Terraria.Chat
{
	public class ChatCommandProcessor : IChatProcessor
	{
		private readonly Dictionary<LocalizedText, ChatCommandId> _localizedCommands = new Dictionary<LocalizedText, ChatCommandId>();

		private readonly Dictionary<ChatCommandId, IChatCommand> _commands = new Dictionary<ChatCommandId, IChatCommand>();

		private readonly Dictionary<LocalizedText, NetworkText> _aliases = new Dictionary<LocalizedText, NetworkText>();

		private IChatCommand _defaultCommand;

		public ChatCommandProcessor AddCommand<T>() where T : IChatCommand, new()
		{
			ChatCommandAttribute cacheableAttribute = AttributeUtilities.GetCacheableAttribute<T, ChatCommandAttribute>();
			string commandKey = "ChatCommand." + cacheableAttribute.Name;
			ChatCommandId chatCommandId = ChatCommandId.FromType<T>();
			_commands[chatCommandId] = new T();
			if (Language.Exists(commandKey))
			{
				_localizedCommands.Add(Language.GetText(commandKey), chatCommandId);
			}
			else
			{
				commandKey += "_";
				LocalizedText[] array = Language.FindAll((string key, LocalizedText text) => key.StartsWith(commandKey));
				foreach (LocalizedText key2 in array)
				{
					_localizedCommands.Add(key2, chatCommandId);
				}
			}
			return this;
		}

		public void AddAlias(LocalizedText text, NetworkText result)
		{
			_aliases[text] = result;
		}

		public void ClearAliases()
		{
			_aliases.Clear();
		}

		public ChatCommandProcessor AddDefaultCommand<T>() where T : IChatCommand, new()
		{
			AddCommand<T>();
			ChatCommandId key = ChatCommandId.FromType<T>();
			_defaultCommand = _commands[key];
			return this;
		}

		private static bool HasLocalizedCommand(ChatMessage message, LocalizedText command)
		{
			string text = message.Text.ToLower();
			string value = command.Value;
			if (!text.StartsWith(value))
			{
				return false;
			}
			if (text.Length == value.Length)
			{
				return true;
			}
			return text[value.Length] == ' ';
		}

		private static string RemoveCommandPrefix(string messageText, LocalizedText command)
		{
			string value = command.Value;
			if (!messageText.StartsWith(value))
			{
				return "";
			}
			if (messageText.Length == value.Length)
			{
				return "";
			}
			if (messageText[value.Length] == ' ')
			{
				return messageText.Substring(value.Length + 1);
			}
			return "";
		}

		public ChatMessage CreateOutgoingMessage(string text)
		{
			ChatMessage message = new ChatMessage(text);
			KeyValuePair<LocalizedText, ChatCommandId> keyValuePair = _localizedCommands.FirstOrDefault((KeyValuePair<LocalizedText, ChatCommandId> pair) => HasLocalizedCommand(message, pair.Key));
			ChatCommandId value = keyValuePair.Value;
			if (keyValuePair.Key != null)
			{
				message.SetCommand(value);
				message.Text = RemoveCommandPrefix(message.Text, keyValuePair.Key);
				_commands[value].ProcessOutgoingMessage(message);
			}
			else
			{
				bool flag = false;
				KeyValuePair<LocalizedText, NetworkText> keyValuePair2 = _aliases.FirstOrDefault((KeyValuePair<LocalizedText, NetworkText> pair) => HasLocalizedCommand(message, pair.Key));
				while (keyValuePair2.Key != null)
				{
					flag = true;
					message = new ChatMessage(keyValuePair2.Value.ToString());
					keyValuePair2 = _aliases.FirstOrDefault((KeyValuePair<LocalizedText, NetworkText> pair) => HasLocalizedCommand(message, pair.Key));
				}
				if (flag)
				{
					return CreateOutgoingMessage(message.Text);
				}
			}
			return message;
		}

		public void ProcessIncomingMessage(ChatMessage message, int clientId)
		{
			if (_commands.TryGetValue(message.CommandId, out var value))
			{
				value.ProcessIncomingMessage(message.Text, (byte)clientId);
				message.Consume();
			}
			else if (_defaultCommand != null)
			{
				_defaultCommand.ProcessIncomingMessage(message.Text, (byte)clientId);
				message.Consume();
			}
		}
	}
}
