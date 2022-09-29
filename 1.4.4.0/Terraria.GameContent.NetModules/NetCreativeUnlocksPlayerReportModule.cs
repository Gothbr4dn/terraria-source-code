using System.IO;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	public class NetCreativeUnlocksPlayerReportModule : NetModule
	{
		private const byte _requestItemSacrificeId = 0;

		public static NetPacket SerializeSacrificeRequest(int itemId, int amount)
		{
			NetPacket result = NetModule.CreatePacket<NetCreativeUnlocksPlayerReportModule>(5);
			result.Writer.Write((byte)0);
			result.Writer.Write((ushort)itemId);
			result.Writer.Write((ushort)amount);
			return result;
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			if (reader.ReadByte() == 0)
			{
				reader.ReadUInt16();
				reader.ReadUInt16();
			}
			return true;
		}
	}
}
