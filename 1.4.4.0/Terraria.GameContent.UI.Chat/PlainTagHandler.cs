using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	public class PlainTagHandler : ITagHandler
	{
		public class PlainSnippet : TextSnippet
		{
			public PlainSnippet(string text = "")
				: base(text)
			{
			}

			public PlainSnippet(string text, Color color, float scale = 1f)
				: base(text, color, scale)
			{
			}

			public override Color GetVisibleColor()
			{
				return Color;
			}
		}

		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			return new PlainSnippet(text);
		}
	}
}
