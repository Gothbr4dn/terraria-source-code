using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.Graphics.Renderers
{
	public class MapHeadRenderer : INeedRenderTargetContent
	{
		private bool _anyDirty;

		private PlayerHeadDrawRenderTargetContent[] _playerRenders = new PlayerHeadDrawRenderTargetContent[255];

		private readonly List<DrawData> _drawData = new List<DrawData>();

		public bool IsReady => !_anyDirty;

		public MapHeadRenderer()
		{
			for (int i = 0; i < _playerRenders.Length; i++)
			{
				_playerRenders[i] = new PlayerHeadDrawRenderTargetContent();
			}
		}

		public void Reset()
		{
			_anyDirty = false;
			_drawData.Clear();
			for (int i = 0; i < _playerRenders.Length; i++)
			{
				_playerRenders[i].Reset();
			}
		}

		public void DrawPlayerHead(Camera camera, Player drawPlayer, Vector2 position, float alpha = 1f, float scale = 1f, Color borderColor = default(Color))
		{
			PlayerHeadDrawRenderTargetContent playerHeadDrawRenderTargetContent = _playerRenders[drawPlayer.whoAmI];
			playerHeadDrawRenderTargetContent.UsePlayer(drawPlayer);
			playerHeadDrawRenderTargetContent.UseColor(borderColor);
			playerHeadDrawRenderTargetContent.Request();
			_anyDirty = true;
			_drawData.Clear();
			if (playerHeadDrawRenderTargetContent.IsReady)
			{
				RenderTarget2D target = playerHeadDrawRenderTargetContent.GetTarget();
				_drawData.Add(new DrawData(target, position, null, Color.White, 0f, ((Texture2D)target).Size() / 2f, scale, SpriteEffects.None));
				RenderDrawData(drawPlayer);
			}
		}

		private void RenderDrawData(Player drawPlayer)
		{
			Effect pixelShader = Main.pixelShader;
			_ = Main.projectile;
			SpriteBatch spriteBatch = Main.spriteBatch;
			for (int i = 0; i < _drawData.Count; i++)
			{
				DrawData cdd = _drawData[i];
				if (!cdd.sourceRect.HasValue)
				{
					cdd.sourceRect = cdd.texture.Frame();
				}
				PlayerDrawHelper.SetShaderForData(drawPlayer, drawPlayer.cHead, ref cdd);
				if (cdd.texture != null)
				{
					cdd.Draw(spriteBatch);
				}
			}
			pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			if (_anyDirty)
			{
				for (int i = 0; i < _playerRenders.Length; i++)
				{
					_playerRenders[i].PrepareRenderTarget(device, spriteBatch);
				}
				_anyDirty = false;
			}
		}

		private void CreateOutlines(float alpha, float scale, Color borderColor)
		{
			if (!(borderColor != Color.Transparent))
			{
				return;
			}
			List<DrawData> collection = new List<DrawData>(_drawData);
			List<DrawData> list = new List<DrawData>(_drawData);
			_drawData.Clear();
			float num = 2f * scale;
			Color color = borderColor;
			color *= alpha * alpha;
			Color black = Color.Black;
			black *= alpha * alpha;
			int colorOnlyShaderIndex = ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex;
			for (int i = 0; i < list.Count; i++)
			{
				DrawData value = list[i];
				value.shader = colorOnlyShaderIndex;
				value.color = black;
				list[i] = value;
			}
			int num2 = 2;
			Vector2 vector;
			for (int j = -num2; j <= num2; j++)
			{
				for (int k = -num2; k <= num2; k++)
				{
					if (Math.Abs(j) + Math.Abs(k) == num2)
					{
						vector = new Vector2((float)j * num, (float)k * num);
						for (int l = 0; l < list.Count; l++)
						{
							DrawData item = list[l];
							item.position += vector;
							_drawData.Add(item);
						}
					}
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				DrawData value2 = list[m];
				value2.shader = colorOnlyShaderIndex;
				value2.color = color;
				list[m] = value2;
			}
			vector = Vector2.Zero;
			num2 = 1;
			for (int n = -num2; n <= num2; n++)
			{
				for (int num3 = -num2; num3 <= num2; num3++)
				{
					if (Math.Abs(n) + Math.Abs(num3) == num2)
					{
						vector = new Vector2((float)n * num, (float)num3 * num);
						for (int num4 = 0; num4 < list.Count; num4++)
						{
							DrawData item2 = list[num4];
							item2.position += vector;
							_drawData.Add(item2);
						}
					}
				}
			}
			_drawData.AddRange(collection);
		}
	}
}
