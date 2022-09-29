using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	public class RemadeChatMonitor : IChatMonitor
	{
		private const int MaxMessages = 500;

		private int _showCount;

		private int _startChatLine;

		private List<ChatMessageContainer> _messages;

		private bool _recalculateOnNextUpdate;

		public RemadeChatMonitor()
		{
			_showCount = 10;
			_startChatLine = 0;
			_messages = new List<ChatMessageContainer>();
		}

		public void NewText(string newText, byte R = byte.MaxValue, byte G = byte.MaxValue, byte B = byte.MaxValue)
		{
			AddNewMessage(newText, new Color(R, G, B));
		}

		public void NewTextMultiline(string text, bool force = false, Color c = default(Color), int WidthLimit = -1)
		{
			AddNewMessage(text, c, WidthLimit);
		}

		public void AddNewMessage(string text, Color color, int widthLimitInPixels = -1)
		{
			ChatMessageContainer chatMessageContainer = new ChatMessageContainer();
			chatMessageContainer.SetContents(text, color, widthLimitInPixels);
			_messages.Insert(0, chatMessageContainer);
			while (_messages.Count > 500)
			{
				_messages.RemoveAt(_messages.Count - 1);
			}
		}

		public void DrawChat(bool drawingPlayerChat)
		{
			int num = _startChatLine;
			int num2 = 0;
			int num3 = 0;
			while (num > 0 && num2 < _messages.Count)
			{
				int num4 = Math.Min(num, _messages[num2].LineCount);
				num -= num4;
				num3 += num4;
				if (num3 == _messages[num2].LineCount)
				{
					num3 = 0;
					num2++;
				}
			}
			int num5 = 0;
			int? num6 = null;
			int snippetIndex = -1;
			int? num7 = null;
			int hoveredSnippet = -1;
			while (num5 < _showCount && num2 < _messages.Count)
			{
				ChatMessageContainer chatMessageContainer = _messages[num2];
				if (!chatMessageContainer.Prepared || !(drawingPlayerChat | chatMessageContainer.CanBeShownWhenChatIsClosed))
				{
					break;
				}
				TextSnippet[] snippetWithInversedIndex = chatMessageContainer.GetSnippetWithInversedIndex(num3);
				ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, FontAssets.MouseText.get_Value(), snippetWithInversedIndex, new Vector2(88f, Main.screenHeight - 30 - 28 - num5 * 21), 0f, Vector2.Zero, Vector2.One, out hoveredSnippet);
				if (hoveredSnippet >= 0)
				{
					num7 = hoveredSnippet;
					num6 = num2;
					snippetIndex = num3;
				}
				num5++;
				num3++;
				if (num3 >= chatMessageContainer.LineCount)
				{
					num3 = 0;
					num2++;
				}
			}
			if (num6.HasValue && num7.HasValue)
			{
				TextSnippet[] snippetWithInversedIndex2 = _messages[num6.Value].GetSnippetWithInversedIndex(snippetIndex);
				snippetWithInversedIndex2[num7.Value].OnHover();
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					snippetWithInversedIndex2[num7.Value].OnClick();
				}
			}
		}

		public void Clear()
		{
			_messages.Clear();
		}

		public void Update()
		{
			if (_recalculateOnNextUpdate)
			{
				_recalculateOnNextUpdate = false;
				for (int i = 0; i < _messages.Count; i++)
				{
					_messages[i].MarkToNeedRefresh();
				}
			}
			for (int j = 0; j < _messages.Count; j++)
			{
				_messages[j].Update();
			}
		}

		public void Offset(int linesOffset)
		{
			_startChatLine += linesOffset;
			ClampMessageIndex();
		}

		private void ClampMessageIndex()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = _startChatLine + _showCount;
			while (num < num4 && num2 < _messages.Count)
			{
				int num5 = Math.Min(num4 - num, _messages[num2].LineCount);
				num += num5;
				if (num < num4)
				{
					num2++;
					num3 = 0;
				}
				else
				{
					num3 = num5;
				}
			}
			int num6 = _showCount;
			while (num6 > 0 && num > 0)
			{
				num3--;
				num6--;
				num--;
				if (num3 < 0)
				{
					num2--;
					if (num2 == -1)
					{
						break;
					}
					num3 = _messages[num2].LineCount - 1;
				}
			}
			_startChatLine = num;
		}

		public void ResetOffset()
		{
			_startChatLine = 0;
		}

		public void OnResolutionChange()
		{
			_recalculateOnNextUpdate = true;
		}
	}
}
