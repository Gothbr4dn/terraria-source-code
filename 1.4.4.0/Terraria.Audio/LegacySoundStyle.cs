using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;

namespace Terraria.Audio
{
	public class LegacySoundStyle : SoundStyle
	{
		private static readonly UnifiedRandom Random = new UnifiedRandom();

		private readonly int _style;

		public readonly int Variations;

		public readonly int SoundId;

		public int Style
		{
			get
			{
				if (Variations != 1)
				{
					return Random.Next(_style, _style + Variations);
				}
				return _style;
			}
		}

		public override bool IsTrackable => SoundId == 42;

		public LegacySoundStyle(int soundId, int style, SoundType type = SoundType.Sound)
			: base(type)
		{
			_style = style;
			Variations = 1;
			SoundId = soundId;
		}

		public LegacySoundStyle(int soundId, int style, int variations, SoundType type = SoundType.Sound)
			: base(type)
		{
			_style = style;
			Variations = variations;
			SoundId = soundId;
		}

		private LegacySoundStyle(int soundId, int style, int variations, SoundType type, float volume, float pitchVariance)
			: base(volume, pitchVariance, type)
		{
			_style = style;
			Variations = variations;
			SoundId = soundId;
		}

		public LegacySoundStyle WithVolume(float volume)
		{
			return new LegacySoundStyle(SoundId, _style, Variations, base.Type, volume, base.PitchVariance);
		}

		public LegacySoundStyle WithPitchVariance(float pitchVariance)
		{
			return new LegacySoundStyle(SoundId, _style, Variations, base.Type, base.Volume, pitchVariance);
		}

		public LegacySoundStyle AsMusic()
		{
			return new LegacySoundStyle(SoundId, _style, Variations, SoundType.Music, base.Volume, base.PitchVariance);
		}

		public LegacySoundStyle AsAmbient()
		{
			return new LegacySoundStyle(SoundId, _style, Variations, SoundType.Ambient, base.Volume, base.PitchVariance);
		}

		public LegacySoundStyle AsSound()
		{
			return new LegacySoundStyle(SoundId, _style, Variations, SoundType.Sound, base.Volume, base.PitchVariance);
		}

		public bool Includes(int soundId, int style)
		{
			if (SoundId == soundId && style >= _style)
			{
				return style < _style + Variations;
			}
			return false;
		}

		public override SoundEffect GetRandomSound()
		{
			if (IsTrackable)
			{
				return SoundEngine.GetTrackableSoundByStyleId(Style);
			}
			return null;
		}
	}
}
