using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Terraria.Localization;

namespace Terraria.Net.Sockets
{
	public class TcpSocket : ISocket
	{
		private byte[] _packetBuffer = new byte[1024];

		private List<object> _callbackBuffer = new List<object>();

		private int _messagesInQueue;

		private TcpClient _connection;

		private TcpListener _listener;

		private SocketConnectionAccepted _listenerCallback;

		private RemoteAddress _remoteAddress;

		private bool _isListening;

		public int MessagesInQueue => _messagesInQueue;

		public TcpSocket()
		{
			_connection = new TcpClient
			{
				NoDelay = true
			};
		}

		public TcpSocket(TcpClient tcpClient)
		{
			_connection = tcpClient;
			_connection.NoDelay = true;
			IPEndPoint iPEndPoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;
			_remoteAddress = new TcpAddress(iPEndPoint.Address, iPEndPoint.Port);
		}

		void ISocket.Close()
		{
			_remoteAddress = null;
			_connection.Close();
		}

		bool ISocket.IsConnected()
		{
			if (_connection == null || _connection.Client == null)
			{
				return false;
			}
			return _connection.Connected;
		}

		void ISocket.Connect(RemoteAddress address)
		{
			TcpAddress tcpAddress = (TcpAddress)address;
			_connection.Connect(tcpAddress.Address, tcpAddress.Port);
			_remoteAddress = address;
		}

		private void ReadCallback(IAsyncResult result)
		{
			Tuple<SocketReceiveCallback, object> tuple = (Tuple<SocketReceiveCallback, object>)result.AsyncState;
			tuple.Item1(tuple.Item2, _connection.GetStream().EndRead(result));
		}

		private void SendCallback(IAsyncResult result)
		{
			object[] obj = (object[])result.AsyncState;
			LegacyNetBufferPool.ReturnBuffer((byte[])obj[1]);
			Tuple<SocketSendCallback, object> tuple = (Tuple<SocketSendCallback, object>)obj[0];
			try
			{
				_connection.GetStream().EndWrite(result);
				tuple.Item1(tuple.Item2);
			}
			catch (Exception)
			{
				((ISocket)this).Close();
			}
		}

		void ISocket.SendQueuedPackets()
		{
		}

		void ISocket.AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state)
		{
			byte[] array = LegacyNetBufferPool.RequestBuffer(data, offset, size);
			_connection.GetStream().BeginWrite(array, 0, size, SendCallback, new object[2]
			{
				new Tuple<SocketSendCallback, object>(callback, state),
				array
			});
		}

		void ISocket.AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state)
		{
			_connection.GetStream().BeginRead(data, offset, size, ReadCallback, new Tuple<SocketReceiveCallback, object>(callback, state));
		}

		bool ISocket.IsDataAvailable()
		{
			if (!_connection.Connected)
			{
				return false;
			}
			return _connection.GetStream().DataAvailable;
		}

		RemoteAddress ISocket.GetRemoteAddress()
		{
			return _remoteAddress;
		}

		bool ISocket.StartListening(SocketConnectionAccepted callback)
		{
			IPAddress address = IPAddress.Any;
			if (Program.LaunchParameters.TryGetValue("-ip", out var value) && !IPAddress.TryParse(value, out address))
			{
				address = IPAddress.Any;
			}
			_isListening = true;
			_listenerCallback = callback;
			if (_listener == null)
			{
				_listener = new TcpListener(address, Netplay.ListenPort);
			}
			try
			{
				_listener.Start();
			}
			catch (Exception)
			{
				return false;
			}
			Thread thread = new Thread(ListenLoop);
			thread.IsBackground = true;
			thread.Name = "TCP Listen Thread";
			thread.Start();
			return true;
		}

		void ISocket.StopListening()
		{
			_isListening = false;
		}

		private void ListenLoop()
		{
			while (_isListening && !Netplay.Disconnect)
			{
				try
				{
					ISocket socket = new TcpSocket(_listener.AcceptTcpClient());
					Console.WriteLine(Language.GetTextValue("Net.ClientConnecting", socket.GetRemoteAddress()));
					_listenerCallback(socket);
				}
				catch (Exception)
				{
				}
			}
			_listener.Stop();
		}
	}
}
