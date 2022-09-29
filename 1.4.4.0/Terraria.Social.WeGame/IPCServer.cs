using System;
using System.IO;
using System.IO.Pipes;
using System.Threading;

namespace Terraria.Social.WeGame
{
	public class IPCServer : IPCBase
	{
		private string _serverName;

		private bool _haveClientAccessFlag;

		public event Action OnClientAccess;

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

		private NamedPipeServerStream GetPipeStream()
		{
			return (NamedPipeServerStream)_pipeStream;
		}

		public void Init(string serverName)
		{
			_serverName = serverName;
		}

		private void LazyCreatePipe()
		{
			if (GetPipeStream() == null)
			{
				_pipeStream = new NamedPipeServerStream(_serverName, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
				_cancelTokenSrc = new CancellationTokenSource();
			}
		}

		public override void ReadCallback(IAsyncResult result)
		{
			IPCContent obj = (IPCContent)result.AsyncState;
			base.ReadCallback(result);
			if (!obj.CancelToken.IsCancellationRequested)
			{
				ContinueReadOrWait();
			}
			else
			{
				WeGameHelper.WriteDebugString("servcer.ReadCallback cancel");
			}
		}

		public void StartListen()
		{
			LazyCreatePipe();
			WeGameHelper.WriteDebugString("begin listen");
			GetPipeStream().BeginWaitForConnection(ConnectionCallback, _cancelTokenSrc.Token);
		}

		private void RestartListen()
		{
			StartListen();
		}

		private void ConnectionCallback(IAsyncResult result)
		{
			try
			{
				_haveClientAccessFlag = true;
				WeGameHelper.WriteDebugString("Connected in");
				GetPipeStream().EndWaitForConnection(result);
				if (!((CancellationToken)result.AsyncState).IsCancellationRequested)
				{
					BeginReadData();
				}
				else
				{
					WeGameHelper.WriteDebugString("ConnectionCallback but user cancel");
				}
			}
			catch (IOException ex)
			{
				_pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("ConnectionCallback Exception, {0}", ex.Message);
			}
		}

		public void ContinueReadOrWait()
		{
			if (GetPipeStream().IsConnected)
			{
				BeginReadData();
				return;
			}
			try
			{
				GetPipeStream().BeginWaitForConnection(ConnectionCallback, null);
			}
			catch (IOException ex)
			{
				_pipeBrokenFlag = true;
				WeGameHelper.WriteDebugString("ContinueReadOrWait Exception, {0}", ex.Message);
			}
		}

		private void ProcessClientAccessEvent()
		{
			if (_haveClientAccessFlag)
			{
				if (this.OnClientAccess != null)
				{
					this.OnClientAccess();
				}
				_haveClientAccessFlag = false;
			}
		}

		private void CheckFlagAndFireEvent()
		{
			ProcessClientAccessEvent();
			ProcessDataArriveEvent();
			ProcessPipeBrokenEvent();
		}

		private void ProcessPipeBrokenEvent()
		{
			if (_pipeBrokenFlag)
			{
				Reset();
				_pipeBrokenFlag = false;
				RestartListen();
			}
		}

		public void Tick()
		{
			CheckFlagAndFireEvent();
		}
	}
}
