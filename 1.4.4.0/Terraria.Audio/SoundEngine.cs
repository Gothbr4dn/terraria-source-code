using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;

namespace Terraria.Audio
{
	public static class SoundEngine
	{
		public static LegacySoundPlayer LegacySoundPlayer;

		public static SoundPlayer SoundPlayer;

		public static bool AreSoundsPaused;

		public static bool IsAudioSupported { get; private set; }

		public static void Initialize()
		{
			IsAudioSupported = TestAudioSupport();
		}

		public static void Load(IServiceProvider services)
		{
			if (IsAudioSupported)
			{
				LegacySoundPlayer = new LegacySoundPlayer(services);
				SoundPlayer = new SoundPlayer();
			}
		}

		public static void Update()
		{
			if (IsAudioSupported)
			{
				if (Main.audioSystem != null)
				{
					Main.audioSystem.UpdateAudioEngine();
				}
				SoundInstanceGarbageCollector.Update();
				bool flag = (!Main.hasFocus || Main.gamePaused) && Main.netMode == 0;
				if (!AreSoundsPaused && flag)
				{
					SoundPlayer.PauseAll();
				}
				else if (AreSoundsPaused && !flag)
				{
					SoundPlayer.ResumeAll();
				}
				AreSoundsPaused = flag;
				SoundPlayer.Update();
			}
		}

		public static void Reload()
		{
			if (IsAudioSupported)
			{
				if (LegacySoundPlayer != null)
				{
					LegacySoundPlayer.Reload();
				}
				if (SoundPlayer != null)
				{
					SoundPlayer.Reload();
				}
			}
		}

		public static void PlaySound(int type, Vector2 position, int style = 1)
		{
			PlaySound(type, (int)position.X, (int)position.Y, style);
		}

		public static SoundEffectInstance PlaySound(LegacySoundStyle type, Vector2 position)
		{
			return PlaySound(type, (int)position.X, (int)position.Y);
		}

		public static SoundEffectInstance PlaySound(LegacySoundStyle type, int x = -1, int y = -1)
		{
			if (type == null)
			{
				return null;
			}
			return PlaySound(type.SoundId, x, y, type.Style, type.Volume, type.GetRandomPitch());
		}

		public static SoundEffectInstance PlaySound(int type, int x = -1, int y = -1, int Style = 1, float volumeScale = 1f, float pitchOffset = 0f)
		{
			if (!IsAudioSupported)
			{
				return null;
			}
			return LegacySoundPlayer.PlaySound(type, x, y, Style, volumeScale, pitchOffset);
		}

		public static ActiveSound GetActiveSound(SlotId id)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (!IsAudioSupported)
			{
				return null;
			}
			return SoundPlayer.GetActiveSound(id);
		}

		public static SlotId PlayTrackedSound(SoundStyle style, Vector2 position)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (!IsAudioSupported)
			{
				return SlotId.Invalid;
			}
			return SoundPlayer.Play(style, position);
		}

		public static SlotId PlayTrackedLoopedSound(SoundStyle style, Vector2 position, ActiveSound.LoopedPlayCondition loopingCondition = null)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (!IsAudioSupported)
			{
				return SlotId.Invalid;
			}
			return SoundPlayer.PlayLooped(style, position, loopingCondition);
		}

		public static SlotId PlayTrackedSound(SoundStyle style)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (!IsAudioSupported)
			{
				return SlotId.Invalid;
			}
			return SoundPlayer.Play(style);
		}

		public static void StopTrackedSounds()
		{
			if (IsAudioSupported)
			{
				SoundPlayer.StopAll();
			}
		}

		public static SoundEffect GetTrackableSoundByStyleId(int id)
		{
			if (!IsAudioSupported)
			{
				return null;
			}
			return LegacySoundPlayer.GetTrackableSoundByStyleId(id);
		}

		public static void StopAmbientSounds()
		{
			if (IsAudioSupported && LegacySoundPlayer != null)
			{
				LegacySoundPlayer.StopAmbientSounds();
			}
		}

		public static ActiveSound FindActiveSound(SoundStyle style)
		{
			if (!IsAudioSupported)
			{
				return null;
			}
			return SoundPlayer.FindActiveSound(style);
		}

		private static bool TestAudioSupport()
		{
			byte[] buffer = new byte[166]
			{
				82, 73, 70, 70, 158, 0, 0, 0, 87, 65,
				86, 69, 102, 109, 116, 32, 16, 0, 0, 0,
				1, 0, 1, 0, 68, 172, 0, 0, 136, 88,
				1, 0, 2, 0, 16, 0, 76, 73, 83, 84,
				26, 0, 0, 0, 73, 78, 70, 79, 73, 83,
				70, 84, 14, 0, 0, 0, 76, 97, 118, 102,
				53, 54, 46, 52, 48, 46, 49, 48, 49, 0,
				100, 97, 116, 97, 88, 0, 0, 0, 0, 0,
				126, 4, 240, 8, 64, 13, 95, 17, 67, 21,
				217, 24, 23, 28, 240, 30, 94, 33, 84, 35,
				208, 36, 204, 37, 71, 38, 64, 38, 183, 37,
				180, 36, 58, 35, 79, 33, 1, 31, 86, 28,
				92, 25, 37, 22, 185, 18, 42, 15, 134, 11,
				222, 7, 68, 4, 196, 0, 112, 253, 86, 250,
				132, 247, 6, 245, 230, 242, 47, 241, 232, 239,
				25, 239, 194, 238, 231, 238, 139, 239, 169, 240,
				61, 242, 67, 244, 180, 246
			};
			try
			{
				using MemoryStream stream = new MemoryStream(buffer);
				SoundEffect.FromStream(stream);
			}
			catch (NoAudioHardwareException)
			{
				Console.WriteLine("No audio hardware found. Disabling all audio.");
				return false;
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
