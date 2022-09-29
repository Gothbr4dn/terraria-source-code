using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;

namespace Terraria.Audio
{
	public abstract class SoundStyle
	{
		private static UnifiedRandom _random = new UnifiedRandom();

		private float _volume;

		private float _pitchVariance;

		private SoundType _type;

		public float Volume => _volume;

		public float PitchVariance => _pitchVariance;

		public SoundType Type => _type;

		public abstract bool IsTrackable { get; }

		public SoundStyle(float volume, float pitchVariance, SoundType type = SoundType.Sound)
		{
			_volume = volume;
			_pitchVariance = pitchVariance;
			_type = type;
		}

		public SoundStyle(SoundType type = SoundType.Sound)
		{
			_volume = 1f;
			_pitchVariance = 0f;
			_type = type;
		}

		public float GetRandomPitch()
		{
			return _random.NextFloat() * PitchVariance - PitchVariance * 0.5f;
		}

		public abstract SoundEffect GetRandomSound();
	}
}
