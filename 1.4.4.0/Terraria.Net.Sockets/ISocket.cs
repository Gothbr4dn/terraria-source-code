namespace Terraria.Net.Sockets
{
	public interface ISocket
	{
		void Close();

		bool IsConnected();

		void Connect(RemoteAddress address);

		void AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state = null);

		void AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state = null);

		bool IsDataAvailable();

		void SendQueuedPackets();

		bool StartListening(SocketConnectionAccepted callback);

		void StopListening();

		RemoteAddress GetRemoteAddress();
	}
}
