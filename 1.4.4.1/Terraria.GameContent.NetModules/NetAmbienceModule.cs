using System.IO;
using Terraria.GameContent.Ambience;
using Terraria.GameContent.Skies;
using Terraria.Graphics.Effects;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	public class NetAmbienceModule : NetModule
	{
		public static NetPacket SerializeSkyEntitySpawn(Player player, SkyEntityType type)
		{
			int value = Main.rand.Next();
			NetPacket result = NetModule.CreatePacket<NetAmbienceModule>(6);
			result.Writer.Write((byte)player.whoAmI);
			result.Writer.Write(value);
			result.Writer.Write((byte)type);
			return result;
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			if (Main.dedServ)
			{
				return false;
			}
			byte playerId = reader.ReadByte();
			int seed = reader.ReadInt32();
			SkyEntityType type = (SkyEntityType)reader.ReadByte();
			if (Main.remixWorld)
			{
				return true;
			}
			Main.QueueMainThreadAction(delegate
			{
				((AmbientSky)SkyManager.Instance["Ambience"]).Spawn(Main.player[playerId], type, seed);
			});
			return true;
		}
	}
}
