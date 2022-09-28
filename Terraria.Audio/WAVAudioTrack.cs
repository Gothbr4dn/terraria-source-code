using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio
{
	public class WAVAudioTrack : ASoundEffectBasedAudioTrack
	{
		private long _streamContentStartIndex = -1L;

		private Stream _stream;

		private const uint JUNK = 1263424842u;

		private const uint FMT = 544501094u;

		public WAVAudioTrack(Stream stream)
		{
			_stream = stream;
			BinaryReader binaryReader = new BinaryReader(stream);
			binaryReader.ReadInt32();
			binaryReader.ReadInt32();
			binaryReader.ReadInt32();
			AudioChannels channels = AudioChannels.Mono;
			uint sampleRate = 0u;
			bool flag = false;
			int num = 0;
			while (!flag && num < 10)
			{
				uint num2 = binaryReader.ReadUInt32();
				int chunkSize = binaryReader.ReadInt32();
				switch (num2)
				{
				case 1263424842u:
					SkipJunk(binaryReader, chunkSize);
					break;
				case 544501094u:
					binaryReader.ReadInt16();
					channels = (AudioChannels)binaryReader.ReadUInt16();
					sampleRate = binaryReader.ReadUInt32();
					binaryReader.ReadInt32();
					binaryReader.ReadInt16();
					binaryReader.ReadInt16();
					flag = true;
					break;
				}
				if (!flag)
				{
					num++;
				}
			}
			binaryReader.ReadInt32();
			binaryReader.ReadInt32();
			_streamContentStartIndex = stream.Position;
			CreateSoundEffect((int)sampleRate, channels);
		}

		private static void SkipJunk(BinaryReader reader, int chunkSize)
		{
			int num = chunkSize;
			if (num % 2 != 0)
			{
				num++;
			}
			reader.ReadBytes(num);
		}

		protected override void ReadAheadPutAChunkIntoTheBuffer()
		{
			byte[] bufferToSubmit = _bufferToSubmit;
			if (_stream.Read(bufferToSubmit, 0, bufferToSubmit.Length) < 1)
			{
				Stop(AudioStopOptions.Immediate);
			}
			else
			{
				_soundEffectInstance.SubmitBuffer(_bufferToSubmit);
			}
		}

		public override void Reuse()
		{
			_stream.Position = _streamContentStartIndex;
		}

		public override void Dispose()
		{
			_soundEffectInstance.Dispose();
			_stream.Dispose();
		}
	}
}
