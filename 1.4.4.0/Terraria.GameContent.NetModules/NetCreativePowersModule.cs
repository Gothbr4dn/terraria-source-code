using System.IO;
using Terraria.GameContent.Creative;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	public class NetCreativePowersModule : NetModule
	{
		public static NetPacket PreparePacket(ushort powerId, int specificInfoBytesInPacketCount)
		{
			NetPacket result = NetModule.CreatePacket<NetCreativePowersModule>(specificInfoBytesInPacketCount + 2);
			result.Writer.Write(powerId);
			return result;
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			ushort id = reader.ReadUInt16();
			if (!CreativePowerManager.Instance.TryGetPower(id, out var power))
			{
				return false;
			}
			power.DeserializeNetMessage(reader, userId);
			return true;
		}
	}
}
