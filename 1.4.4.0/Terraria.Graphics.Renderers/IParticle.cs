using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers
{
	public interface IParticle
	{
		bool ShouldBeRemovedFromRenderer { get; }

		void Update(ref ParticleRendererSettings settings);

		void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch);
	}
}
