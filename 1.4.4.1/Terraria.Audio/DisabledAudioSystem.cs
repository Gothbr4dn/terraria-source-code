using System;
using System.Collections;
using System.Collections.Generic;
using ReLogic.Content.Sources;

namespace Terraria.Audio
{
	public class DisabledAudioSystem : IAudioSystem, IDisposable
	{
		public void LoadFromSources()
		{
		}

		public void UseSources(List<IContentSource> sources)
		{
		}

		public void Update()
		{
		}

		public void UpdateMisc()
		{
		}

		public IEnumerator PrepareWaveBank()
		{
			yield break;
		}

		public void LoadCue(int cueIndex, string cueName)
		{
		}

		public void PauseAll()
		{
		}

		public void ResumeAll()
		{
		}

		public void UpdateAmbientCueState(int i, bool gameIsActive, ref float trackVolume, float systemVolume)
		{
		}

		public void UpdateAmbientCueTowardStopping(int i, float stoppingSpeed, ref float trackVolume, float systemVolume)
		{
		}

		public bool IsTrackPlaying(int trackIndex)
		{
			return false;
		}

		public void UpdateCommonTrack(bool active, int i, float totalVolume, ref float tempFade)
		{
		}

		public void UpdateCommonTrackTowardStopping(int i, float totalVolume, ref float tempFade, bool isMainTrackAudible)
		{
		}

		public void UpdateAudioEngine()
		{
		}

		public void Dispose()
		{
		}
	}
}
