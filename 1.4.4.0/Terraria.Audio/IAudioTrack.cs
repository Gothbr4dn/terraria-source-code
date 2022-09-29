using System;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	public interface IAudioTrack : IDisposable
	{
		bool IsPlaying { get; }

		bool IsStopped { get; }

		bool IsPaused { get; }

		void Stop(AudioStopOptions options);

		void Play();

		void Pause();

		void SetVariable(string variableName, float value);

		void Resume();

		void Reuse();

		void Update();
	}
}
