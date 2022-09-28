using System;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent
{
	public abstract class ARenderTargetContentByRequest : INeedRenderTargetContent
	{
		protected RenderTarget2D _target;

		protected bool _wasPrepared;

		private bool _wasRequested;

		public bool IsReady => _wasPrepared;

		public void Request()
		{
			_wasRequested = true;
		}

		public RenderTarget2D GetTarget()
		{
			return _target;
		}

		public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
		{
			_wasPrepared = false;
			if (_wasRequested)
			{
				_wasRequested = false;
				HandleUseReqest(device, spriteBatch);
			}
		}

		protected abstract void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch);

		protected void PrepareARenderTarget_AndListenToEvents(ref RenderTarget2D target, GraphicsDevice device, int neededWidth, int neededHeight, RenderTargetUsage usage)
		{
			if (target == null || target.IsDisposed || target.Width != neededWidth || target.Height != neededHeight)
			{
				if (target != null)
				{
					target.ContentLost -= target_ContentLost;
					target.Disposing -= target_Disposing;
				}
				target = new RenderTarget2D(device, neededWidth, neededHeight, mipMap: false, device.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, usage);
				target.ContentLost += target_ContentLost;
				target.Disposing += target_Disposing;
			}
		}

		private void target_Disposing(object sender, EventArgs e)
		{
			_wasPrepared = false;
			_target = null;
		}

		private void target_ContentLost(object sender, EventArgs e)
		{
			_wasPrepared = false;
		}

		public void Reset()
		{
			_wasPrepared = false;
			_wasRequested = false;
			_target = null;
		}

		protected void PrepareARenderTarget_WithoutListeningToEvents(ref RenderTarget2D target, GraphicsDevice device, int neededWidth, int neededHeight, RenderTargetUsage usage)
		{
			if (target == null || target.IsDisposed || target.Width != neededWidth || target.Height != neededHeight)
			{
				target = new RenderTarget2D(device, neededWidth, neededHeight, mipMap: false, device.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, usage);
			}
		}
	}
}
