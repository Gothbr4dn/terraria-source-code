using System;
using System.Collections.Generic;
using rail;

namespace Terraria.Social.WeGame
{
	public class WeGameP2PWriter
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

		private RailID _local_id;

		private Dictionary<RailID, Queue<WriteInformation>> _pendingSendData = new Dictionary<RailID, Queue<WriteInformation>>();

		private Dictionary<RailID, Queue<WriteInformation>> _pendingSendDataSwap = new Dictionary<RailID, Queue<WriteInformation>>();

		private Queue<byte[]> _bufferPool = new Queue<byte[]>();

		private object _lock = new object();

		public void QueueSend(RailID user, byte[] data, int length)
		{
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

		public void ClearUser(RailID user)
		{
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

		public void SetLocalPeer(RailID rail_id)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			if ((RailComparableID)(object)_local_id == (RailComparableID)null)
			{
				_local_id = new RailID();
			}
			((RailComparableID)_local_id).id_ = ((RailComparableID)rail_id).id_;
		}

		private RailID GetLocalPeer()
		{
			return _local_id;
		}

		private bool IsValid()
		{
			if ((RailComparableID)(object)_local_id != (RailComparableID)null)
			{
				return ((RailComparableID)_local_id).IsValid();
			}
			return false;
		}

		public void SendAll()
		{
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Invalid comparison between Unknown and I4
			if (!IsValid())
			{
				return;
			}
			lock (_lock)
			{
				Utils.Swap(ref _pendingSendData, ref _pendingSendDataSwap);
			}
			foreach (KeyValuePair<RailID, Queue<WriteInformation>> item in _pendingSendDataSwap)
			{
				Queue<WriteInformation> value = item.Value;
				while (value.Count > 0)
				{
					WriteInformation writeInformation = value.Dequeue();
					_ = (int)rail_api.RailFactory().RailNetworkHelper().SendData(GetLocalPeer(), item.Key, writeInformation.Data, (uint)writeInformation.Size) == 0;
					_bufferPool.Enqueue(writeInformation.Data);
				}
			}
		}
	}
}
