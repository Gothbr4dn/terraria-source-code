using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content.Sources;

namespace Terraria.Audio
{
	public class LegacyAudioSystem : IAudioSystem, IDisposable
	{
		public IAudioTrack[] AudioTracks;

		public int MusicReplayDelay;

		public AudioEngine Engine;

		public SoundBank SoundBank;

		public WaveBank WaveBank;

		public Dictionary<int, string> TrackNamesByIndex;

		public Dictionary<int, IAudioTrack> DefaultTrackByIndex;

		public List<IContentSource> FileSources;

		public void LoadFromSources()
		{
			List<IContentSource> fileSources = FileSources;
			for (int i = 0; i < AudioTracks.Length; i++)
			{
				if (TrackNamesByIndex.TryGetValue(i, out var value))
				{
					char directorySeparatorChar = Path.DirectorySeparatorChar;
					string assetPath = "Music" + directorySeparatorChar + value;
					IAudioTrack audioTrack = DefaultTrackByIndex[i];
					IAudioTrack audioTrack2 = audioTrack;
					IAudioTrack audioTrack3 = FindReplacementTrack(fileSources, assetPath);
					if (audioTrack3 != null)
					{
						audioTrack2 = audioTrack3;
					}
					if (AudioTracks[i] != audioTrack2)
					{
						AudioTracks[i].Stop(AudioStopOptions.Immediate);
					}
					if (AudioTracks[i] != audioTrack)
					{
						AudioTracks[i].Dispose();
					}
					AudioTracks[i] = audioTrack2;
				}
			}
		}

		public void UseSources(List<IContentSource> sourcesFromLowestToHighest)
		{
			FileSources = sourcesFromLowestToHighest;
			LoadFromSources();
		}

		public void Update()
		{
			if (!WaveBank.IsPrepared)
			{
				return;
			}
			for (int i = 0; i < AudioTracks.Length; i++)
			{
				if (AudioTracks[i] != null)
				{
					AudioTracks[i].Update();
				}
			}
		}

		private IAudioTrack FindReplacementTrack(List<IContentSource> sources, string assetPath)
		{
			IAudioTrack audioTrack = null;
			for (int i = 0; i < sources.Count; i++)
			{
				IContentSource val = sources[i];
				if (!val.HasAsset(assetPath))
				{
					continue;
				}
				string extension = val.GetExtension(assetPath);
				try
				{
					IAudioTrack audioTrack2 = null;
					switch (extension)
					{
					case ".ogg":
						audioTrack2 = new OGGAudioTrack(val.OpenStream(assetPath));
						break;
					case ".wav":
						audioTrack2 = new WAVAudioTrack(val.OpenStream(assetPath));
						break;
					case ".mp3":
						audioTrack2 = new MP3AudioTrack(val.OpenStream(assetPath));
						break;
					}
					if (audioTrack2 != null)
					{
						audioTrack?.Dispose();
						audioTrack = audioTrack2;
					}
				}
				catch
				{
					string textToShow = "A resource pack failed to load " + assetPath + "!";
					Main.IssueReporter.AddReport(textToShow);
					Main.IssueReporterIndicator.AttemptLettingPlayerKnow();
				}
			}
			return audioTrack;
		}

		public LegacyAudioSystem()
		{
			Engine = new AudioEngine("Content\\TerrariaMusic.xgs");
			SoundBank = new SoundBank(Engine, "Content\\Sound Bank.xsb");
			Engine.Update();
			WaveBank = new WaveBank(Engine, "Content\\Wave Bank.xwb", 0, 512);
			Engine.Update();
			AudioTracks = new IAudioTrack[92];
			TrackNamesByIndex = new Dictionary<int, string>();
			DefaultTrackByIndex = new Dictionary<int, IAudioTrack>();
		}

		public IEnumerator PrepareWaveBank()
		{
			while (!WaveBank.IsPrepared)
			{
				Engine.Update();
				yield return null;
			}
		}

		public void LoadCue(int cueIndex, string cueName)
		{
			CueAudioTrack cueAudioTrack = new CueAudioTrack(SoundBank, cueName);
			TrackNamesByIndex[cueIndex] = cueName;
			DefaultTrackByIndex[cueIndex] = cueAudioTrack;
			AudioTracks[cueIndex] = cueAudioTrack;
		}

		public void UpdateMisc()
		{
			if (Main.curMusic != Main.newMusic)
			{
				MusicReplayDelay = 0;
			}
			if (MusicReplayDelay > 0)
			{
				MusicReplayDelay--;
			}
		}

		public void PauseAll()
		{
			if (!WaveBank.IsPrepared)
			{
				return;
			}
			float[] musicFade = Main.musicFade;
			for (int i = 0; i < AudioTracks.Length; i++)
			{
				if (AudioTracks[i] != null && !AudioTracks[i].IsPaused && AudioTracks[i].IsPlaying && musicFade[i] > 0f)
				{
					try
					{
						AudioTracks[i].Pause();
					}
					catch (Exception)
					{
					}
				}
			}
		}

		public void ResumeAll()
		{
			if (!WaveBank.IsPrepared)
			{
				return;
			}
			float[] musicFade = Main.musicFade;
			for (int i = 0; i < AudioTracks.Length; i++)
			{
				if (AudioTracks[i] != null && AudioTracks[i].IsPaused && musicFade[i] > 0f)
				{
					try
					{
						AudioTracks[i].Resume();
					}
					catch (Exception)
					{
					}
				}
			}
		}

		public void UpdateAmbientCueState(int i, bool gameIsActive, ref float trackVolume, float systemVolume)
		{
			if (!WaveBank.IsPrepared)
			{
				return;
			}
			if (systemVolume == 0f)
			{
				if (AudioTracks[i].IsPlaying)
				{
					AudioTracks[i].Stop(AudioStopOptions.Immediate);
				}
				return;
			}
			if (!AudioTracks[i].IsPlaying)
			{
				AudioTracks[i].Reuse();
				AudioTracks[i].Play();
				AudioTracks[i].SetVariable("Volume", trackVolume * systemVolume);
				return;
			}
			if (AudioTracks[i].IsPaused && gameIsActive)
			{
				AudioTracks[i].Resume();
				return;
			}
			trackVolume += 0.005f;
			if (trackVolume > 1f)
			{
				trackVolume = 1f;
			}
			AudioTracks[i].SetVariable("Volume", trackVolume * systemVolume);
		}

		public void UpdateAmbientCueTowardStopping(int i, float stoppingSpeed, ref float trackVolume, float systemVolume)
		{
			if (!WaveBank.IsPrepared)
			{
				return;
			}
			if (!AudioTracks[i].IsPlaying)
			{
				trackVolume = 0f;
				return;
			}
			if (trackVolume > 0f)
			{
				trackVolume -= stoppingSpeed;
				if (trackVolume < 0f)
				{
					trackVolume = 0f;
				}
			}
			if (trackVolume <= 0f)
			{
				AudioTracks[i].Stop(AudioStopOptions.Immediate);
			}
			else
			{
				AudioTracks[i].SetVariable("Volume", trackVolume * systemVolume);
			}
		}

		public bool IsTrackPlaying(int trackIndex)
		{
			if (!WaveBank.IsPrepared)
			{
				return false;
			}
			return AudioTracks[trackIndex].IsPlaying;
		}

		public void UpdateCommonTrack(bool active, int i, float totalVolume, ref float tempFade)
		{
			if (!WaveBank.IsPrepared)
			{
				return;
			}
			tempFade += 0.005f;
			if (tempFade > 1f)
			{
				tempFade = 1f;
			}
			if (!AudioTracks[i].IsPlaying && active)
			{
				if (MusicReplayDelay == 0)
				{
					if (Main.SettingMusicReplayDelayEnabled)
					{
						MusicReplayDelay = Main.rand.Next(14400, 21601);
					}
					AudioTracks[i].Reuse();
					AudioTracks[i].SetVariable("Volume", totalVolume);
					AudioTracks[i].Play();
				}
			}
			else
			{
				AudioTracks[i].SetVariable("Volume", totalVolume);
			}
		}

		public void UpdateCommonTrackTowardStopping(int i, float totalVolume, ref float tempFade, bool isMainTrackAudible)
		{
			if (!WaveBank.IsPrepared)
			{
				return;
			}
			if (AudioTracks[i].IsPlaying || !AudioTracks[i].IsStopped)
			{
				if (isMainTrackAudible)
				{
					tempFade -= 0.005f;
				}
				else if (Main.curMusic == 0)
				{
					tempFade = 0f;
				}
				if (tempFade <= 0f)
				{
					tempFade = 0f;
					AudioTracks[i].SetVariable("Volume", 0f);
					AudioTracks[i].Stop(AudioStopOptions.Immediate);
				}
				else
				{
					AudioTracks[i].SetVariable("Volume", totalVolume);
				}
			}
			else
			{
				tempFade = 0f;
			}
		}

		public void UpdateAudioEngine()
		{
			Engine.Update();
		}

		public void Dispose()
		{
			SoundBank.Dispose();
			WaveBank.Dispose();
			Engine.Dispose();
		}
	}
}
