using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.Effects
{
	public abstract class EffectManager<T> where T : GameEffect
	{
		protected bool _isLoaded;

		protected Dictionary<string, T> _effects = new Dictionary<string, T>();

		public bool IsLoaded => _isLoaded;

		public T this[string key]
		{
			get
			{
				if (_effects.TryGetValue(key, out var value))
				{
					return value;
				}
				return null;
			}
			set
			{
				Bind(key, value);
			}
		}

		public void Bind(string name, T effect)
		{
			_effects[name] = effect;
			if (_isLoaded)
			{
				effect.Load();
			}
		}

		public void Load()
		{
			if (_isLoaded)
			{
				return;
			}
			_isLoaded = true;
			foreach (T value in _effects.Values)
			{
				value.Load();
			}
		}

		public T Activate(string name, Vector2 position = default(Vector2), params object[] args)
		{
			if (!_effects.ContainsKey(name))
			{
				throw new MissingEffectException(string.Concat("Unable to find effect named: ", name, ". Type: ", typeof(T), "."));
			}
			T val = _effects[name];
			OnActivate(val, position);
			val.Activate(position, args);
			return val;
		}

		public void Deactivate(string name, params object[] args)
		{
			if (!_effects.ContainsKey(name))
			{
				throw new MissingEffectException(string.Concat("Unable to find effect named: ", name, ". Type: ", typeof(T), "."));
			}
			T val = _effects[name];
			OnDeactivate(val);
			val.Deactivate(args);
		}

		public virtual void OnActivate(T effect, Vector2 position)
		{
		}

		public virtual void OnDeactivate(T effect)
		{
		}
	}
}
