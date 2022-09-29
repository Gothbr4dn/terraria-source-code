using System.IO;
using Microsoft.Xna.Framework;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	public class NetPingModule : NetModule
	{
		public static NetPacket Serialize(Vector2 position)
		{
			NetPacket result = NetModule.CreatePacket<NetPingModule>(8);
			result.Writer.WriteVector2(position);
			return result;
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			Vector2 position = reader.ReadVector2();
			Main.Pings.Add(position);
			return true;
		}
	}
}
