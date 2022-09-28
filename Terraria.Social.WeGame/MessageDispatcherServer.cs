using System;

namespace Terraria.Social.WeGame
{
	public class MessageDispatcherServer
	{
		private IPCServer _ipcSever = new IPCServer();

		public event Action OnIPCClientAccess;

		public event Action<IPCMessage> OnMessage;

		public void Init(string serverName)
		{
			_ipcSever.Init(serverName);
			_ipcSever.OnDataArrive += OnDataArrive;
			_ipcSever.OnClientAccess += OnClientAccess;
		}

		public void OnClientAccess()
		{
			if (this.OnIPCClientAccess != null)
			{
				this.OnIPCClientAccess();
			}
		}

		public void Start()
		{
			_ipcSever.StartListen();
		}

		private void OnDataArrive(byte[] data)
		{
			IPCMessage iPCMessage = new IPCMessage();
			iPCMessage.BuildFrom(data);
			if (this.OnMessage != null)
			{
				this.OnMessage(iPCMessage);
			}
		}

		public void Tick()
		{
			_ipcSever.Tick();
		}

		public bool SendMessage(IPCMessage msg)
		{
			return _ipcSever.Send(msg.GetBytes());
		}
	}
}
