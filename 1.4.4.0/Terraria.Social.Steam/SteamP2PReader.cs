using System;
using System.Collections.Generic;
using Steamworks;

namespace Terraria.Social.Steam
{
	public class SteamP2PReader
	{
		public class ReadResult
		{
			public byte[] Data;

			public uint Size;

			public uint Offset;

			public ReadResult(byte[] data, uint size)
			{
				Data = data;
				Size = size;
				Offset = 0u;
			}
		}

		public delegate bool OnReadEvent(byte[] data, int size, CSteamID user);

		public object SteamLock = new object();

		private const int BUFFER_SIZE = 4096;

		private Dictionary<CSteamID, Queue<ReadResult>> _pendingReadBuffers = new Dictionary<CSteamID, Queue<ReadResult>>();

		private Queue<CSteamID> _deletionQueue = new Queue<CSteamID>();

		private Queue<byte[]> _bufferPool = new Queue<byte[]>();

		private int _channel;

		private OnReadEvent _readEvent;

		public SteamP2PReader(int channel)
		{
			_channel = channel;
		}

		public void ClearUser(CSteamID id)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			lock (_pendingReadBuffers)
			{
				_deletionQueue.Enqueue(id);
			}
		}

		public bool IsDataAvailable(CSteamID id)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			lock (_pendingReadBuffers)
			{
				if (!_pendingReadBuffers.ContainsKey(id))
				{
					return false;
				}
				Queue<ReadResult> queue = _pendingReadBuffers[id];
				if (queue.Count == 0 || queue.Peek().Size == 0)
				{
					return false;
				}
				return true;
			}
		}

		public void SetReadEvent(OnReadEvent method)
		{
			_readEvent = method;
		}

		private bool IsPacketAvailable(out uint size)
		{
			lock (SteamLock)
			{
				return SteamNetworking.IsP2PPacketAvailable(ref size, _channel);
			}
		}

		public void ReadTick()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			lock (_pendingReadBuffers)
			{
				while (_deletionQueue.Count > 0)
				{
					_pendingReadBuffers.Remove(_deletionQueue.Dequeue());
				}
				uint size;
				uint size2 = default(uint);
				CSteamID val = default(CSteamID);
				while (IsPacketAvailable(out size))
				{
					byte[] array = ((_bufferPool.Count != 0) ? _bufferPool.Dequeue() : new byte[Math.Max(size, 4096u)]);
					bool flag;
					lock (SteamLock)
					{
						flag = SteamNetworking.ReadP2PPacket(array, (uint)array.Length, ref size2, ref val, _channel);
					}
					if (!flag)
					{
						continue;
					}
					if (_readEvent == null || _readEvent(array, (int)size2, val))
					{
						if (!_pendingReadBuffers.ContainsKey(val))
						{
							_pendingReadBuffers[val] = new Queue<ReadResult>();
						}
						_pendingReadBuffers[val].Enqueue(new ReadResult(array, size2));
					}
					else
					{
						_bufferPool.Enqueue(array);
					}
				}
			}
		}

		public int Receive(CSteamID user, byte[] buffer, int bufferOffset, int bufferSize)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			uint num = 0u;
			lock (_pendingReadBuffers)
			{
				if (!_pendingReadBuffers.ContainsKey(user))
				{
					return 0;
				}
				Queue<ReadResult> queue = _pendingReadBuffers[user];
				while (queue.Count > 0)
				{
					ReadResult readResult = queue.Peek();
					uint num2 = Math.Min((uint)bufferSize - num, readResult.Size - readResult.Offset);
					if (num2 == 0)
					{
						return (int)num;
					}
					Array.Copy(readResult.Data, readResult.Offset, buffer, bufferOffset + num, num2);
					if (num2 == readResult.Size - readResult.Offset)
					{
						_bufferPool.Enqueue(queue.Dequeue().Data);
					}
					else
					{
						readResult.Offset += num2;
					}
					num += num2;
				}
				return (int)num;
			}
		}
	}
}
