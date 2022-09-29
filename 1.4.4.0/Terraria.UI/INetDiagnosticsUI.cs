using Microsoft.Xna.Framework.Graphics;

namespace Terraria.UI
{
	public interface INetDiagnosticsUI
	{
		void Reset();

		void Draw(SpriteBatch spriteBatch);

		void CountReadMessage(int messageId, int messageLength);

		void CountSentMessage(int messageId, int messageLength);

		void CountReadModuleMessage(int moduleMessageId, int messageLength);

		void CountSentModuleMessage(int moduleMessageId, int messageLength);
	}
}
