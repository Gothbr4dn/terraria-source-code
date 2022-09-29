using System;
using System.Collections.Generic;

namespace Terraria.Net
{
	public class LegacyNetBufferPool
	{
		private const int SMALL_BUFFER_SIZE = 256;

		private const int MEDIUM_BUFFER_SIZE = 1024;

		private const int LARGE_BUFFER_SIZE = 16384;

		private static object bufferLock = new object();

		private static Queue<byte[]> _smallBufferQueue = new Queue<byte[]>();

		private static Queue<byte[]> _mediumBufferQueue = new Queue<byte[]>();

		private static Queue<byte[]> _largeBufferQueue = new Queue<byte[]>();

		private static int _smallBufferCount;

		private static int _mediumBufferCount;

		private static int _largeBufferCount;

		private static int _customBufferCount;

		public static byte[] RequestBuffer(int size)
		{
			lock (bufferLock)
			{
				if (size <= 256)
				{
					if (_smallBufferQueue.Count == 0)
					{
						_smallBufferCount++;
						return new byte[256];
					}
					return _smallBufferQueue.Dequeue();
				}
				if (size <= 1024)
				{
					if (_mediumBufferQueue.Count == 0)
					{
						_mediumBufferCount++;
						return new byte[1024];
					}
					return _mediumBufferQueue.Dequeue();
				}
				if (size <= 16384)
				{
					if (_largeBufferQueue.Count == 0)
					{
						_largeBufferCount++;
						return new byte[16384];
					}
					return _largeBufferQueue.Dequeue();
				}
				_customBufferCount++;
				return new byte[size];
			}
		}

		public static byte[] RequestBuffer(byte[] data, int offset, int size)
		{
			byte[] array = RequestBuffer(size);
			Buffer.BlockCopy(data, offset, array, 0, size);
			return array;
		}

		public static void ReturnBuffer(byte[] buffer)
		{
			int num = buffer.Length;
			lock (bufferLock)
			{
				if (num <= 256)
				{
					_smallBufferQueue.Enqueue(buffer);
				}
				else if (num <= 1024)
				{
					_mediumBufferQueue.Enqueue(buffer);
				}
				else if (num <= 16384)
				{
					_largeBufferQueue.Enqueue(buffer);
				}
			}
		}

		public static void DisplayBufferSizes()
		{
			lock (bufferLock)
			{
				Main.NewText("Small Buffers:  " + _smallBufferQueue.Count + " queued of " + _smallBufferCount);
				Main.NewText("Medium Buffers: " + _mediumBufferQueue.Count + " queued of " + _mediumBufferCount);
				Main.NewText("Large Buffers:  " + _largeBufferQueue.Count + " queued of " + _largeBufferCount);
				Main.NewText("Custom Buffers: 0 queued of " + _customBufferCount);
			}
		}

		public static void PrintBufferSizes()
		{
			lock (bufferLock)
			{
				Console.WriteLine("Small Buffers:  " + _smallBufferQueue.Count + " queued of " + _smallBufferCount);
				Console.WriteLine("Medium Buffers: " + _mediumBufferQueue.Count + " queued of " + _mediumBufferCount);
				Console.WriteLine("Large Buffers:  " + _largeBufferQueue.Count + " queued of " + _largeBufferCount);
				Console.WriteLine("Custom Buffers: 0 queued of " + _customBufferCount);
				Console.WriteLine("");
			}
		}
	}
}
