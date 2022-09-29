using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria.UI.Chat
{
	public class ChatMessageContainer
	{
		public string OriginalText;

		private bool _prepared;

		private List<TextSnippet[]> _parsedText;

		private Color _color;

		private int _widthLimitInPixels;

		private int _timeLeft;

		public int LineCount => _parsedText.Count;

		public bool CanBeShownWhenChatIsClosed => _timeLeft > 0;

		public bool Prepared => _prepared;

		public void SetContents(string text, Color color, int widthLimitInPixels)
		{
			OriginalText = text;
			_color = color;
			_widthLimitInPixels = widthLimitInPixels;
			MarkToNeedRefresh();
			_parsedText = new List<TextSnippet[]>();
			_timeLeft = 600;
			Refresh();
		}

		public void MarkToNeedRefresh()
		{
			_prepared = false;
		}

		public void Update()
		{
			if (_timeLeft > 0)
			{
				_timeLeft--;
			}
			Refresh();
		}

		public TextSnippet[] GetSnippetWithInversedIndex(int snippetIndex)
		{
			int index = _parsedText.Count - 1 - snippetIndex;
			return _parsedText[index];
		}

		public void Refresh()
		{
			if (!_prepared)
			{
				_prepared = true;
				int num = _widthLimitInPixels;
				if (num == -1)
				{
					num = Main.screenWidth - 320;
				}
				List<List<TextSnippet>> list = Utils.WordwrapStringSmart(OriginalText, _color, FontAssets.MouseText.get_Value(), num, 10);
				_parsedText.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					_parsedText.Add(list[i].ToArray());
				}
			}
		}
	}
}
