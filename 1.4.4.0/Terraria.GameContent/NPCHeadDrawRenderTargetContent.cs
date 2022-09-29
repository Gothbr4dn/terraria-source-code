using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent
{
	public class NPCHeadDrawRenderTargetContent : AnOutlinedDrawRenderTargetContent
	{
		private Texture2D _theTexture;

		public void SetTexture(Texture2D texture)
		{
			if (_theTexture != texture)
			{
				_theTexture = texture;
				_wasPrepared = false;
				width = texture.Width + 8;
				height = texture.Height + 8;
			}
		}

		internal override void DrawTheContent(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_theTexture, new Vector2(4f, 4f), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}
}
