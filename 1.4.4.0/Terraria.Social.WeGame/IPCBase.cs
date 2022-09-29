using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;

namespace Terraria.Social.WeGame
{
	public abstract class IPCBase
	{
		private List<List<byte>> _producer = new List<List<byte>>();

		private List<List<byte>> _consumer = new List<List<byte>>();

		private List<byte> _totalData = new List<byte>();

		private object _listLock = new object();

		private volatile bool _haveDataToReadFlag;

		protected volatile bool _pipeBrokenFlag;

		protected PipeStream _pipeStream;

		protected CancellationTokenSource _cancelTokenSrc;

		protected Action<byte[]> _onDataArrive;

		public int BufferSize { get; set; }

		public virtual event Action<byte[]> OnDataArrive
		{
			add
			{
				_onDataArrive = (Action<byte[]>)Delegate.Combine(_onDataArrive, value);
			}
			remove
			{
				_onDataArrive = (Action<byte[]>)Delegate.Remove(_onDataArrive, value);
			}
		}

		public IPCBase()
		{
			BufferSize = 256;
		}

		protected void AddPackToList(List<byte> pack)
		{
			lock (_listLock)
			{
				_producer.Add(pack);
				_haveDataToReadFlag = true;
			}
		}

		protected List<List<byte>> GetPackList()
		{
			List<List<byte>> list = null;
			lock (_listLock)
			{
				List<List<byte>> producer = _producer;
				_producer = _consumer;
				_consumer = producer;
				_producer.Clear();
				list = _consumer;
				_haveDataToReadFlag = false;
				return list;
			}
		}

		protected bool HaveDataToRead()
		{
			return _haveDataToReadFlag;
		}

		public virtual void Reset()
		{
			_cancelTokenSrc.Cancel();
			_pipeStream.Dispose();
			_pipeStream = null;
		}

		public virtual void ProcessDataArriveEvent()
		{
			if (!HaveDataToRead())
			{
				return;
			}
			List<List<byte>> packList = GetPackList();
			if (packList == null || _onDataArrive == null)
			{
				return;
			}
			foreach (List<byte> item in packList)
			{
				_onDataArrive(item.ToArray());
			}
		}

		protected virtual bool BeginReadData()
		{
			bool result = false;
			IPCContent iPCContent = new IPCContent
			{
				data = new byte[BufferSize],
				CancelToken = _cancelTokenSrc.Token
			};
			WeGameHelper.WriteDebugString("BeginReadData");
			try
			{
				if (_pipeStream != null)
				{
					_pipeStream.BeginRead(iPCContent.data, 0, BufferSize, ReadCallback, iPCContent);
					result = true;
					return result;
				}
				return result;
			}
			catch (IOException ex)
			{
				_pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("BeginReadData Exception, {0}", ex.Message);
				return result;
			}
		}

		public virtual void ReadCallback(IAsyncResult result)
		{
			WeGameHelper.WriteDebugString("ReadCallback: " + Thread.CurrentThread.ManagedThreadId);
			IPCContent iPCContent = (IPCContent)result.AsyncState;
			try
			{
				int num = _pipeStream.EndRead(result);
				if (!iPCContent.CancelToken.IsCancellationRequested)
				{
					if (num > 0)
					{
						_totalData.AddRange(iPCContent.data.Take(num));
						if (_pipeStream.IsMessageComplete)
						{
							AddPackToList(_totalData);
							_totalData = new List<byte>();
						}
					}
				}
				else
				{
					WeGameHelper.WriteDebugString("IPCBase.ReadCallback.cancel");
				}
			}
			catch (IOException ex)
			{
				_pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("ReadCallback Exception, {0}", ex.Message);
			}
			catch (InvalidOperationException ex2)
			{
				_pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("ReadCallback Exception, {0}", ex2.Message);
			}
		}

		public virtual bool Send(string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			return Send(bytes);
		}

		public virtual bool Send(byte[] data)
		{
			bool result = false;
			if (_pipeStream != null && _pipeStream.IsConnected)
			{
				try
				{
					_pipeStream.BeginWrite(data, 0, data.Length, SendCallback, null);
					result = true;
					return result;
				}
				catch (IOException ex)
				{
					_pipeBrokenFlag = true;
					WeGameHelper.WriteDebugString("Send Exception, {0}", ex.Message);
					return result;
				}
			}
			return result;
		}

		protected virtual void SendCallback(IAsyncResult result)
		{
			try
			{
				if (_pipeStream != null)
				{
					_pipeStream.EndWrite(result);
				}
			}
			catch (IOException ex)
			{
				_pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("SendCallback Exception, {0}", ex.Message);
			}
		}
	}
}
