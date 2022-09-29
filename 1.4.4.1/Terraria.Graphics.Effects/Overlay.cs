using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Effects
{
	public abstract class Overlay : GameEffect
	{
		public OverlayMode Mode = OverlayMode.Inactive;

		private RenderLayers _layer = RenderLayers.All;

		public RenderLayers Layer => _layer;

		public Overlay(EffectPriority priority, RenderLayers layer)
		{
			_priority = priority;
			_layer = layer;
		}

		public abstract void Draw(SpriteBatch spriteBatch);

		public abstract void Update(GameTime gameTime);
	}
}
