using Microsoft.Xna.Framework;

namespace Terraria.UI.Chat
{
	public class ChatLine
	{
		public Color color = Color.White;

		public int showTime;

		public string originalText = "";

		public TextSnippet[] parsedText = new TextSnippet[0];

		private int? parsingPixelLimit;

		private bool needsParsing;

		public void UpdateTimeLeft()
		{
			if (showTime > 0)
			{
				showTime--;
			}
			if (needsParsing)
			{
				needsParsing = false;
			}
		}

		public void Copy(ChatLine other)
		{
			needsParsing = other.needsParsing;
			parsingPixelLimit = other.parsingPixelLimit;
			originalText = other.originalText;
			parsedText = other.parsedText;
			showTime = other.showTime;
			color = other.color;
		}

		public void FlagAsNeedsReprocessing()
		{
			needsParsing = true;
		}
	}
}
