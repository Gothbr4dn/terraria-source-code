using System.Collections.Generic;

namespace Terraria.Graphics.Renderers
{
	public class ParticlePool<T> where T : IPooledParticle
	{
		public delegate T ParticleInstantiator();

		private ParticleInstantiator _instantiator;

		private List<T> _particles;

		public int CountParticlesInUse()
		{
			int num = 0;
			for (int i = 0; i < num; i++)
			{
				if (!_particles[i].IsRestingInPool)
				{
					num++;
				}
			}
			return num;
		}

		public ParticlePool(int initialPoolSize, ParticleInstantiator instantiator)
		{
			_particles = new List<T>(initialPoolSize);
			_instantiator = instantiator;
		}

		public T RequestParticle()
		{
			int count = _particles.Count;
			for (int i = 0; i < count; i++)
			{
				if (_particles[i].IsRestingInPool)
				{
					_particles[i].FetchFromPool();
					return _particles[i];
				}
			}
			T val = _instantiator();
			_particles.Add(val);
			val.FetchFromPool();
			return val;
		}
	}
}
