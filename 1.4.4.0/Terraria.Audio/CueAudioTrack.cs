using System;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	public class CueAudioTrack : IAudioTrack, IDisposable
	{
		private Cue _cue;

		private string _cueName;

		private SoundBank _soundBank;

		public bool IsPlaying => _cue.IsPlaying;

		public bool IsStopped => _cue.IsStopped;

		public bool IsPaused => _cue.IsPaused;

		public CueAudioTrack(SoundBank bank, string cueName)
		{
			_soundBank = bank;
			_cueName = cueName;
			Reuse();
		}

		public void Stop(AudioStopOptions options)
		{
			_cue.Stop(options);
		}

		public void Play()
		{
			_cue.Play();
		}

		public void SetVariable(string variableName, float value)
		{
			_cue.SetVariable(variableName, value);
		}

		public void Resume()
		{
			_cue.Resume();
		}

		public void Reuse()
		{
			if (_cue != null)
			{
				Stop(AudioStopOptions.Immediate);
			}
			_cue = _soundBank.GetCue(_cueName);
		}

		public void Pause()
		{
			_cue.Pause();
		}

		public void Dispose()
		{
		}

		public void Update()
		{
		}
	}
}
