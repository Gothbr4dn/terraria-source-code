using Microsoft.Xna.Framework;

namespace Terraria.UI.Chat
{
	public interface ITagHandler
	{
		TextSnippet Parse(string text, Color baseColor = default(Color), string options = null);
	}
}
