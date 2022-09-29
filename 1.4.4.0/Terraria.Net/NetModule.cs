using System.IO;

namespace Terraria.Net
{
	public abstract class NetModule
	{
		public abstract bool Deserialize(BinaryReader reader, int userId);

		protected static NetPacket CreatePacket<T>(int maxSize) where T : NetModule
		{
			ushort id = NetManager.Instance.GetId<T>();
			return new NetPacket(id, maxSize);
		}
	}
}
