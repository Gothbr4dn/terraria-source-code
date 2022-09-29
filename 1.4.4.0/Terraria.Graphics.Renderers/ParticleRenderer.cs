using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers
{
	public class ParticleRenderer
	{
		public ParticleRendererSettings Settings;

		public List<IParticle> Particles = new List<IParticle>();

		public ParticleRenderer()
		{
			Settings = default(ParticleRendererSettings);
		}

		public void Add(IParticle particle)
		{
			Particles.Add(particle);
		}

		public void Clear()
		{
			Particles.Clear();
		}

		public void Update()
		{
			for (int i = 0; i < Particles.Count; i++)
			{
				if (Particles[i].ShouldBeRemovedFromRenderer)
				{
					if (Particles[i] is IPooledParticle pooledParticle)
					{
						pooledParticle.RestInPool();
					}
					Particles.RemoveAt(i);
					i--;
				}
				else
				{
					Particles[i].Update(ref Settings);
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < Particles.Count; i++)
			{
				if (!Particles[i].ShouldBeRemovedFromRenderer)
				{
					Particles[i].Draw(ref Settings, spriteBatch);
				}
			}
		}
	}
}
