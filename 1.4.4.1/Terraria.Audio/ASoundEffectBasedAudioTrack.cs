using System;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	public abstract class ASoundEffectBasedAudioTrack : IAudioTrack, IDisposable
	{
		protected const int bufferLength = 4096;

		protected const int bufferCountPerSubmit = 2;

		protected const int buffersToCoverFor = 8;

		protected byte[] _bufferToSubmit = new byte[4096];

		protected float[] _temporaryBuffer = new float[2048];

		private int _sampleRate;

		private AudioChannels _channels;

		protected DynamicSoundEffectInstance _soundEffectInstance;

		public bool IsPlaying => _soundEffectInstance.State == SoundState.Playing;

		public bool IsStopped => _soundEffectInstance.State == SoundState.Stopped;

		public bool IsPaused => _soundEffectInstance.State == SoundState.Paused;

		public ASoundEffectBasedAudioTrack()
		{
		}

		protected void CreateSoundEffect(int sampleRate, AudioChannels channels)
		{
			_sampleRate = sampleRate;
			_channels = channels;
			_soundEffectInstance = new DynamicSoundEffectInstance(_sampleRate, _channels);
		}

		private void _soundEffectInstance_BufferNeeded(object sender, EventArgs e)
		{
			PrepareBuffer();
		}

		public void Update()
		{
			if (IsPlaying && _soundEffectInstance.PendingBufferCount < 8)
			{
				PrepareBuffer();
			}
		}

		protected void ResetBuffer()
		{
			for (int i = 0; i < _bufferToSubmit.Length; i++)
			{
				_bufferToSubmit[i] = 0;
			}
		}

		protected void PrepareBuffer()
		{
			for (int i = 0; i < 2; i++)
			{
				ReadAheadPutAChunkIntoTheBuffer();
			}
		}

		public void Stop(AudioStopOptions options)
		{
			_soundEffectInstance.Stop(options == AudioStopOptions.Immediate);
		}

		public void Play()
		{
			PrepareToPlay();
			_soundEffectInstance.Play();
		}

		public void Pause()
		{
			_soundEffectInstance.Pause();
		}

		public void SetVariable(string variableName, float value)
		{
			switch (variableName)
			{
			case "Volume":
			{
				float volume = ReMapVolumeToMatchXact(value);
				_soundEffectInstance.Volume = volume;
				break;
			}
			case "Pitch":
				_soundEffectInstance.Pitch = value;
				break;
			case "Pan":
				_soundEffectInstance.Pan = value;
				break;
			}
		}

		private float ReMapVolumeToMatchXact(float musicVolume)
		{
			double num = 31.0 * (double)musicVolume - 25.0 - 11.94;
			return (float)Math.Pow(10.0, num / 20.0);
		}

		public void Resume()
		{
			_soundEffectInstance.Resume();
		}

		protected virtual void PrepareToPlay()
		{
			ResetBuffer();
		}

		public abstract void Reuse();

		protected virtual void ReadAheadPutAChunkIntoTheBuffer()
		{
		}

		public abstract void Dispose();
	}
}
