using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	public class NameTagHandler : ITagHandler
	{
		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			return new TextSnippet("<" + text.Replace("\\[", "[").Replace("\\]", "]") + ">", baseColor);
		}

		public static string GenerateTag(string name)
		{
			return "[n:" + name.Replace("[", "\\[").Replace("]", "\\]") + "]";
		}
	}
}
