using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	public static class SoundInstanceGarbageCollector
	{
		private static readonly List<SoundEffectInstance> _activeSounds = new List<SoundEffectInstance>(128);

		public static void Track(SoundEffectInstance sound)
		{
			if (Program.IsFna)
			{
				_activeSounds.Add(sound);
			}
		}

		public static void Update()
		{
			for (int i = 0; i < _activeSounds.Count; i++)
			{
				if (_activeSounds[i] == null)
				{
					_activeSounds.RemoveAt(i);
					i--;
				}
				else if (_activeSounds[i].State == SoundState.Stopped)
				{
					_activeSounds[i].Dispose();
					_activeSounds.RemoveAt(i);
					i--;
				}
			}
		}
	}
}
