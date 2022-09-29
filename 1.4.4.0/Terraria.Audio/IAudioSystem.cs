using System;
using System.Collections;
using System.Collections.Generic;
using ReLogic.Content.Sources;

namespace Terraria.Audio
{
	public interface IAudioSystem : IDisposable
	{
		void LoadCue(int cueIndex, string cueName);

		void PauseAll();

		void ResumeAll();

		void UpdateMisc();

		void UpdateAudioEngine();

		void UpdateAmbientCueState(int i, bool gameIsActive, ref float trackVolume, float systemVolume);

		void UpdateAmbientCueTowardStopping(int i, float stoppingSpeed, ref float trackVolume, float systemVolume);

		void UpdateCommonTrack(bool active, int i, float totalVolume, ref float tempFade);

		void UpdateCommonTrackTowardStopping(int i, float totalVolume, ref float tempFade, bool isMainTrackAudible);

		bool IsTrackPlaying(int trackIndex);

		void UseSources(List<IContentSource> sources);

		IEnumerator PrepareWaveBank();

		void LoadFromSources();

		void Update();
	}
}
