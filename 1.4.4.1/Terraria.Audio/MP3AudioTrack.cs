using System.IO;
using Microsoft.Xna.Framework.Audio;
using XPT.Core.Audio.MP3Sharp;

namespace Terraria.Audio
{
	public class MP3AudioTrack : ASoundEffectBasedAudioTrack
	{
		private Stream _stream;

		private MP3Stream _mp3Stream;

		public MP3AudioTrack(Stream stream)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			_stream = stream;
			MP3Stream val = new MP3Stream(stream);
			int frequency = val.get_Frequency();
			_mp3Stream = val;
			CreateSoundEffect(frequency, AudioChannels.Stereo);
		}

		public override void Reuse()
		{
			((Stream)(object)_mp3Stream).Position = 0L;
		}

		public override void Dispose()
		{
			_soundEffectInstance.Dispose();
			((Stream)(object)_mp3Stream).Dispose();
			_stream.Dispose();
		}

		protected override void ReadAheadPutAChunkIntoTheBuffer()
		{
			byte[] bufferToSubmit = _bufferToSubmit;
			if (((Stream)(object)_mp3Stream).Read(bufferToSubmit, 0, bufferToSubmit.Length) < 1)
			{
				Stop(AudioStopOptions.Immediate);
			}
			else
			{
				_soundEffectInstance.SubmitBuffer(_bufferToSubmit);
			}
		}
	}
}
