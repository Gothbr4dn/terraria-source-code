using System.Net;

namespace Terraria.Net
{
	public class TcpAddress : RemoteAddress
	{
		public IPAddress Address;

		public int Port;

		public TcpAddress(IPAddress address, int port)
		{
			Type = AddressType.Tcp;
			Address = address;
			Port = port;
		}

		public override string GetIdentifier()
		{
			return Address.ToString();
		}

		public override bool IsLocalHost()
		{
			return Address.Equals(IPAddress.Loopback);
		}

		public override string ToString()
		{
			return new IPEndPoint(Address, Port).ToString();
		}

		public override string GetFriendlyName()
		{
			return ToString();
		}
	}
}
