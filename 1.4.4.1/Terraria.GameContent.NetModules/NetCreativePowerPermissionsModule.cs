using System.IO;
using Terraria.GameContent.Creative;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	public class NetCreativePowerPermissionsModule : NetModule
	{
		private const byte _setPermissionLevelId = 0;

		public static NetPacket SerializeCurrentPowerPermissionLevel(ushort powerId, int level)
		{
			NetPacket result = NetModule.CreatePacket<NetCreativePowerPermissionsModule>(4);
			result.Writer.Write((byte)0);
			result.Writer.Write(powerId);
			result.Writer.Write((byte)level);
			return result;
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			if (reader.ReadByte() == 0)
			{
				ushort id = reader.ReadUInt16();
				int currentPermissionLevel = reader.ReadByte();
				if (Main.netMode == 2)
				{
					return false;
				}
				if (!CreativePowerManager.Instance.TryGetPower(id, out var power))
				{
					return false;
				}
				power.CurrentPermissionLevel = (PowerPermissionLevel)currentPermissionLevel;
			}
			return true;
		}
	}
}
