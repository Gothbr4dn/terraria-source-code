using System;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Terraria.Social.WeGame
{
	public class IPCClient : IPCBase
	{
		private bool _connectedFlag;

		public event Action OnConnected;

		public override event Action<byte[]> OnDataArrive
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

		private NamedPipeClientStream GetPipeStream()
		{
			return (NamedPipeClientStream)_pipeStream;
		}

		private void ProcessConnectedEvent()
		{
			if (_connectedFlag)
			{
				if (this.OnConnected != null)
				{
					this.OnConnected();
				}
				_connectedFlag = false;
			}
		}

		private void ProcessPipeBrokenEvent()
		{
			if (_pipeBrokenFlag)
			{
				Reset();
				_pipeBrokenFlag = false;
			}
		}

		private void CheckFlagAndFireEvent()
		{
			ProcessConnectedEvent();
			ProcessDataArriveEvent();
			ProcessPipeBrokenEvent();
		}

		public void Init(string clientName)
		{
		}

		public void ConnectTo(string serverName)
		{
			if (GetPipeStream() != null)
			{
				return;
			}
			_pipeStream = new NamedPipeClientStream(".", serverName, PipeDirection.InOut, PipeOptions.Asynchronous);
			_cancelTokenSrc = new CancellationTokenSource();
			Task.Factory.StartNew(delegate(object content)
			{
				GetPipeStream().Connect();
				if (!((CancellationToken)content).IsCancellationRequested)
				{
					GetPipeStream().ReadMode = PipeTransmissionMode.Message;
					BeginReadData();
					_connectedFlag = true;
				}
			}, _cancelTokenSrc.Token);
		}

		public void Tick()
		{
			CheckFlagAndFireEvent();
		}

		public override void ReadCallback(IAsyncResult result)
		{
			IPCContent obj = (IPCContent)result.AsyncState;
			base.ReadCallback(result);
			if (!obj.CancelToken.IsCancellationRequested)
			{
				if (GetPipeStream().IsConnected)
				{
					BeginReadData();
				}
			}
			else
			{
				WeGameHelper.WriteDebugString("ReadCallback cancel");
			}
		}
	}
}
