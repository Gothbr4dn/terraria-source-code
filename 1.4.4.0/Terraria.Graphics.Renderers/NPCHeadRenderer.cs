using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers
{
	public class NPCHeadRenderer : INeedRenderTargetContent
	{
		private NPCHeadDrawRenderTargetContent[] _contents;

		private Asset<Texture2D>[] _matchingArray;

		public bool IsReady => false;

		public NPCHeadRenderer(Asset<Texture2D>[] matchingArray)
		{
			_matchingArray = matchingArray;
			Reset();
		}

		public void Reset()
		{
			_contents = new NPCHeadDrawRenderTargetContent[_matchingArray.Length];
		}

		public void DrawWithOutlines(Entity entity, int headId, Vector2 position, Color color, float rotation, float scale, SpriteEffects effects)
		{
			if (_contents[headId] == null)
			{
				_contents[headId] = new NPCHeadDrawRenderTargetContent();
				_contents[headId].SetTexture(_matchingArray[headId].get_Value());
			}
			NPCHeadDrawRenderTargetContent nPCHeadDrawRenderTargetContent = _contents[headId];
			if (nPCHeadDrawRenderTargetContent.IsReady)
			{
				RenderTarget2D target = nPCHeadDrawRenderTargetContent.GetTarget();
				Main.spriteBatch.Draw(target, position, null, color, rotation, ((Texture2D)target).Size() / 2f, scale, effects, 0f);
			}
			else
			{
				nPCHeadDrawRenderTargetContent.Request();
			}
		}

		public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			for (int i = 0; i < _contents.Length; i++)
			{
				if (_contents[i] != null && !_contents[i].IsReady)
				{
					_contents[i].PrepareRenderTarget(device, spriteBatch);
				}
			}
		}
	}
}
