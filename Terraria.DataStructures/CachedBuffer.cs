using System.IO;

namespace Terraria.DataStructures
{
	public class CachedBuffer
	{
		public readonly byte[] Data;

		public readonly BinaryWriter Writer;

		public readonly BinaryReader Reader;

		private readonly MemoryStream _memoryStream;

		private bool _isActive = true;

		public int Length => Data.Length;

		public bool IsActive => _isActive;

		public CachedBuffer(byte[] data)
		{
			Data = data;
			_memoryStream = new MemoryStream(data);
			Writer = new BinaryWriter(_memoryStream);
			Reader = new BinaryReader(_memoryStream);
		}

		internal CachedBuffer Activate()
		{
			_isActive = true;
			_memoryStream.Position = 0L;
			return this;
		}

		public void Recycle()
		{
			if (_isActive)
			{
				_isActive = false;
				BufferPool.Recycle(this);
			}
		}
	}
}
