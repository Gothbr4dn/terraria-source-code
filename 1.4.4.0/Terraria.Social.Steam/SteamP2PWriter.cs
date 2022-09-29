using System;
using System.Collections.Generic;
using Steamworks;

namespace Terraria.Social.Steam
{
	public class SteamP2PWriter
	{
		public class WriteInformation
		{
			public byte[] Data;

			public int Size;

			public WriteInformation()
			{
				Data = new byte[1024];
				Size = 0;
			}

			public WriteInformation(byte[] data)
			{
				Data = data;
				Size = 0;
			}
		}

		private const int BUFFER_SIZE = 1024;

		private Dictionary<CSteamID, Queue<WriteInformation>> _pendingSendData = new Dictionary<CSteamID, Queue<WriteInformation>>();

		private Dictionary<CSteamID, Queue<WriteInformation>> _pendingSendDataSwap = new Dictionary<CSteamID, Queue<WriteInformation>>();

		private Queue<byte[]> _bufferPool = new Queue<byte[]>();

		private int _channel;

		private object _lock = new object();

		public SteamP2PWriter(int channel)
		{
			_channel = channel;
		}

		public void QueueSend(CSteamID user, byte[] data, int length)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			lock (_lock)
			{
				Queue<WriteInformation> queue2 = (_pendingSendData.ContainsKey(user) ? _pendingSendData[user] : (_pendingSendData[user] = new Queue<WriteInformation>()));
				int num = length;
				int num2 = 0;
				while (num > 0)
				{
					WriteInformation writeInformation;
					if (queue2.Count == 0 || 1024 - queue2.Peek().Size == 0)
					{
						writeInformation = ((_bufferPool.Count <= 0) ? new WriteInformation() : new WriteInformation(_bufferPool.Dequeue()));
						queue2.Enqueue(writeInformation);
					}
					else
					{
						writeInformation = queue2.Peek();
					}
					int num3 = Math.Min(num, 1024 - writeInformation.Size);
					Array.Copy(data, num2, writeInformation.Data, writeInformation.Size, num3);
					writeInformation.Size += num3;
					num -= num3;
					num2 += num3;
				}
			}
		}

		public void ClearUser(CSteamID user)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			lock (_lock)
			{
				if (_pendingSendData.ContainsKey(user))
				{
					Queue<WriteInformation> queue = _pendingSendData[user];
					while (queue.Count > 0)
					{
						_bufferPool.Enqueue(queue.Dequeue().Data);
					}
				}
				if (_pendingSendDataSwap.ContainsKey(user))
				{
					Queue<WriteInformation> queue2 = _pendingSendDataSwap[user];
					while (queue2.Count > 0)
					{
						_bufferPool.Enqueue(queue2.Dequeue().Data);
					}
				}
			}
		}

		public void SendAll()
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			lock (_lock)
			{
				Utils.Swap(ref _pendingSendData, ref _pendingSendDataSwap);
			}
			foreach (KeyValuePair<CSteamID, Queue<WriteInformation>> item in _pendingSendDataSwap)
			{
				Queue<WriteInformation> value = item.Value;
				while (value.Count > 0)
				{
					WriteInformation writeInformation = value.Dequeue();
					SteamNetworking.SendP2PPacket(item.Key, writeInformation.Data, (uint)writeInformation.Size, (EP2PSend)2, _channel);
					_bufferPool.Enqueue(writeInformation.Data);
				}
			}
		}
	}
}
