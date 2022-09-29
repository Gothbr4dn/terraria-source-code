namespace Terraria.Net
{
	public abstract class RemoteAddress
	{
		public AddressType Type;

		public abstract string GetIdentifier();

		public abstract string GetFriendlyName();

		public abstract bool IsLocalHost();
	}
}
