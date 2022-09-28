using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.UI.Chat;
using Terraria.Localization;
using Terraria.Net;

namespace Terraria.Chat
{
	public static class ChatHelper
	{
		private static List<Tuple<string, Color>> _cachedMessages = new List<Tuple<string, Color>>();

		public static void DisplayMessageOnClient(NetworkText text, Color color, int playerId)
		{
			DisplayMessage(text, color, byte.MaxValue);
		}

		public static void SendChatMessageToClient(NetworkText text, Color color, int playerId)
		{
			SendChatMessageToClientAs(byte.MaxValue, text, color, playerId);
		}

		public static void SendChatMessageToClientAs(byte messageAuthor, NetworkText text, Color color, int playerId)
		{
			if (playerId == Main.myPlayer)
			{
				DisplayMessage(text, color, messageAuthor);
			}
		}

		public static void BroadcastChatMessage(NetworkText text, Color color, int excludedPlayer = -1)
		{
			BroadcastChatMessageAs(byte.MaxValue, text, color, excludedPlayer);
		}

		public static void BroadcastChatMessageAs(byte messageAuthor, NetworkText text, Color color, int excludedPlayer = -1)
		{
			if (excludedPlayer != Main.myPlayer)
			{
				DisplayMessage(text, color, messageAuthor);
			}
		}

		public static bool OnlySendToPlayersWhoAreLoggedIn(int clientIndex)
		{
			return Netplay.Clients[clientIndex].State == 10;
		}

		public static void SendChatMessageFromClient(ChatMessage message)
		{
			if (!message.IsConsumed)
			{
				NetPacket packet = NetTextModule.SerializeClientMessage(message);
				NetManager.Instance.SendToServer(packet);
			}
		}

		public static void DisplayMessage(NetworkText text, Color color, byte messageAuthor)
		{
			string text2 = text.ToString();
			if (messageAuthor < byte.MaxValue)
			{
				Main.player[messageAuthor].chatOverhead.NewMessage(text2, Main.PlayerOverheadChatMessageDisplayTime);
				Main.player[messageAuthor].chatOverhead.color = color;
				text2 = NameTagHandler.GenerateTag(Main.player[messageAuthor].name) + " " + text2;
			}
			if (ShouldCacheMessage())
			{
				CacheMessage(text2, color);
			}
			else
			{
				Main.NewTextMultiline(text2, force: false, color);
			}
		}

		private static void CacheMessage(string message, Color color)
		{
			_cachedMessages.Add(new Tuple<string, Color>(message, color));
		}

		public static void ShowCachedMessages()
		{
			lock (_cachedMessages)
			{
				foreach (Tuple<string, Color> cachedMessage in _cachedMessages)
				{
					Main.NewTextMultiline(cachedMessage.Item1, force: false, cachedMessage.Item2);
				}
			}
		}

		public static void ClearDelayedMessagesCache()
		{
			_cachedMessages.Clear();
		}

		private static bool ShouldCacheMessage()
		{
			if (Main.netMode == 1)
			{
				return Main.gameMenu;
			}
			return false;
		}
	}
}
