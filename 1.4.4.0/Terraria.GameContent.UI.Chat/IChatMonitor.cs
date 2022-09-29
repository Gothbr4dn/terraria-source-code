using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI.Chat
{
	public interface IChatMonitor
	{
		void NewText(string newText, byte R = byte.MaxValue, byte G = byte.MaxValue, byte B = byte.MaxValue);

		void NewTextMultiline(string text, bool force = false, Color c = default(Color), int WidthLimit = -1);

		void DrawChat(bool drawingPlayerChat);

		void Clear();

		void Update();

		void Offset(int linesOffset);

		void ResetOffset();

		void OnResolutionChange();
	}
}
