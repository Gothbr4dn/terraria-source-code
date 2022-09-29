using System.IO;
using Terraria.DataStructures;

namespace Terraria.Net
{
	public struct NetPacket
	{
		private const int HEADER_SIZE = 5;

		public readonly ushort Id;

		public readonly CachedBuffer Buffer;

		public int Length { get; private set; }

		public BinaryWriter Writer => Buffer.Writer;

		public BinaryReader Reader => Buffer.Reader;

		public NetPacket(ushort id, int size)
		{
			this = default(NetPacket);
			Id = id;
			Buffer = BufferPool.Request(size + 5);
			Length = size + 5;
			Writer.Write((ushort)(size + 5));
			Writer.Write((byte)82);
			Writer.Write(id);
		}

		public void Recycle()
		{
			Buffer.Recycle();
		}

		public void ShrinkToFit()
		{
			if (Length != (int)Writer.BaseStream.Position)
			{
				Length = (int)Writer.BaseStream.Position;
				Writer.Seek(0, SeekOrigin.Begin);
				Writer.Write((ushort)Length);
				Writer.Seek(Length, SeekOrigin.Begin);
			}
		}
	}
}
