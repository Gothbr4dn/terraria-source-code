using System;

namespace Terraria.Social.WeGame
{
	public class MessageDispatcherClient
	{
		private IPCClient _ipcClient = new IPCClient();

		private string _severName;

		private string _clientName;

		public event Action<IPCMessage> OnMessage;

		public event Action OnConnected;

		public void Init(string clientName, string serverName)
		{
			_clientName = clientName;
			_severName = serverName;
			_ipcClient.Init(clientName);
			_ipcClient.OnDataArrive += OnDataArrive;
			_ipcClient.OnConnected += OnServerConnected;
		}

		public void Start()
		{
			_ipcClient.ConnectTo(_severName);
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

		private void OnServerConnected()
		{
			if (this.OnConnected != null)
			{
				this.OnConnected();
			}
		}

		public void Tick()
		{
			_ipcClient.Tick();
		}

		public bool SendMessage(IPCMessage msg)
		{
			return _ipcClient.Send(msg.GetBytes());
		}
	}
}
