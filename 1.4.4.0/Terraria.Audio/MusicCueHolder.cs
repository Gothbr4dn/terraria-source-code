using System;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	public class MusicCueHolder
	{
		private SoundBank _soundBank;

		private string _cueName;

		private Cue _loadedCue;

		private float _lastSetVolume;

		public bool IsPlaying
		{
			get
			{
				if (_loadedCue != null)
				{
					return _loadedCue.IsPlaying;
				}
				return false;
			}
		}

		public bool IsOngoing
		{
			get
			{
				if (_loadedCue != null)
				{
					if (!_loadedCue.IsPlaying)
					{
						return !_loadedCue.IsStopped;
					}
					return true;
				}
				return false;
			}
		}

		public MusicCueHolder(SoundBank soundBank, string cueName)
		{
			_soundBank = soundBank;
			_cueName = cueName;
			_loadedCue = null;
		}

		public void Pause()
		{
			if (_loadedCue == null || _loadedCue.IsPaused || !_loadedCue.IsPlaying)
			{
				return;
			}
			try
			{
				_loadedCue.Pause();
			}
			catch (Exception)
			{
			}
		}

		public void Resume()
		{
			if (_loadedCue == null || !_loadedCue.IsPaused)
			{
				return;
			}
			try
			{
				_loadedCue.Resume();
			}
			catch (Exception)
			{
			}
		}

		public void Stop()
		{
			if (_loadedCue != null)
			{
				SetVolume(0f);
				_loadedCue.Stop(AudioStopOptions.Immediate);
			}
		}

		public void RestartAndTryPlaying(float volume)
		{
			PurgeCue();
			TryPlaying(volume);
		}

		private void PurgeCue()
		{
			if (_loadedCue != null)
			{
				_loadedCue.Stop(AudioStopOptions.Immediate);
				_loadedCue.Dispose();
				_loadedCue = null;
			}
		}

		public void Play(float volume)
		{
			LoadTrack(forceReload: false);
			SetVolume(volume);
			_loadedCue.Play();
		}

		public void TryPlaying(float volume)
		{
			LoadTrack(forceReload: false);
			if (_loadedCue.IsPrepared)
			{
				SetVolume(volume);
				if (!_loadedCue.IsPlaying)
				{
					_loadedCue.Play();
				}
			}
		}

		public void SetVolume(float volume)
		{
			_lastSetVolume = volume;
			if (_loadedCue != null)
			{
				_loadedCue.SetVariable("Volume", _lastSetVolume);
			}
		}

		private void LoadTrack(bool forceReload)
		{
			if (forceReload || _loadedCue == null)
			{
				_loadedCue = _soundBank.GetCue(_cueName);
			}
		}
	}
}
