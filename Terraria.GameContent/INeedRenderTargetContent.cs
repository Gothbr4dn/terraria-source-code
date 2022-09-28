using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent
{
	public interface INeedRenderTargetContent
	{
		bool IsReady { get; }

		void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch);

		void Reset();
	}
}
