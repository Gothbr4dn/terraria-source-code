using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	public static class BufferPool
	{
		private const int SMALL_BUFFER_SIZE = 32;

		private const int MEDIUM_BUFFER_SIZE = 256;

		private const int LARGE_BUFFER_SIZE = 16384;

		private static object bufferLock = new object();

		private static Queue<CachedBuffer> SmallBufferQueue = new Queue<CachedBuffer>();

		private static Queue<CachedBuffer> MediumBufferQueue = new Queue<CachedBuffer>();

		private static Queue<CachedBuffer> LargeBufferQueue = new Queue<CachedBuffer>();

		public static CachedBuffer Request(int size)
		{
			lock (bufferLock)
			{
				if (size <= 32)
				{
					if (SmallBufferQueue.Count == 0)
					{
						return new CachedBuffer(new byte[32]);
					}
					return SmallBufferQueue.Dequeue().Activate();
				}
				if (size <= 256)
				{
					if (MediumBufferQueue.Count == 0)
					{
						return new CachedBuffer(new byte[256]);
					}
					return MediumBufferQueue.Dequeue().Activate();
				}
				if (size <= 16384)
				{
					if (LargeBufferQueue.Count == 0)
					{
						return new CachedBuffer(new byte[16384]);
					}
					return LargeBufferQueue.Dequeue().Activate();
				}
				return new CachedBuffer(new byte[size]);
			}
		}

		public static CachedBuffer Request(byte[] data, int offset, int size)
		{
			CachedBuffer cachedBuffer = Request(size);
			Buffer.BlockCopy(data, offset, cachedBuffer.Data, 0, size);
			return cachedBuffer;
		}

		public static void Recycle(CachedBuffer buffer)
		{
			int length = buffer.Length;
			lock (bufferLock)
			{
				if (length <= 32)
				{
					SmallBufferQueue.Enqueue(buffer);
				}
				else if (length <= 256)
				{
					MediumBufferQueue.Enqueue(buffer);
				}
				else if (length <= 16384)
				{
					LargeBufferQueue.Enqueue(buffer);
				}
			}
		}

		public static void PrintBufferSizes()
		{
			lock (bufferLock)
			{
				Console.WriteLine("SmallBufferQueue.Count: " + SmallBufferQueue.Count);
				Console.WriteLine("MediumBufferQueue.Count: " + MediumBufferQueue.Count);
				Console.WriteLine("LargeBufferQueue.Count: " + LargeBufferQueue.Count);
				Console.WriteLine("");
			}
		}
	}
}
