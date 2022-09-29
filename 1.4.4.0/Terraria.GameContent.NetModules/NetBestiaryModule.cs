using System.IO;
using Terraria.ID;
using Terraria.Net;

namespace Terraria.GameContent.NetModules
{
	public class NetBestiaryModule : NetModule
	{
		private enum BestiaryUnlockType : byte
		{
			Kill,
			Sight,
			Chat
		}

		public static NetPacket SerializeKillCount(int npcNetId, int killcount)
		{
			NetPacket result = NetModule.CreatePacket<NetBestiaryModule>(5);
			result.Writer.Write((byte)0);
			result.Writer.Write((short)npcNetId);
			result.Writer.Write((ushort)killcount);
			return result;
		}

		public static NetPacket SerializeSight(int npcNetId)
		{
			NetPacket result = NetModule.CreatePacket<NetBestiaryModule>(3);
			result.Writer.Write((byte)1);
			result.Writer.Write((short)npcNetId);
			return result;
		}

		public static NetPacket SerializeChat(int npcNetId)
		{
			NetPacket result = NetModule.CreatePacket<NetBestiaryModule>(3);
			result.Writer.Write((byte)2);
			result.Writer.Write((short)npcNetId);
			return result;
		}

		public override bool Deserialize(BinaryReader reader, int userId)
		{
			switch (reader.ReadByte())
			{
			case 0:
			{
				short key3 = reader.ReadInt16();
				string bestiaryCreditId3 = ContentSamples.NpcsByNetId[key3].GetBestiaryCreditId();
				ushort killCount = reader.ReadUInt16();
				Main.BestiaryTracker.Kills.SetKillCountDirectly(bestiaryCreditId3, killCount);
				break;
			}
			case 2:
			{
				short key2 = reader.ReadInt16();
				string bestiaryCreditId2 = ContentSamples.NpcsByNetId[key2].GetBestiaryCreditId();
				Main.BestiaryTracker.Chats.SetWasChatWithDirectly(bestiaryCreditId2);
				break;
			}
			case 1:
			{
				short key = reader.ReadInt16();
				string bestiaryCreditId = ContentSamples.NpcsByNetId[key].GetBestiaryCreditId();
				Main.BestiaryTracker.Sights.SetWasSeenDirectly(bestiaryCreditId);
				break;
			}
			}
			return true;
		}
	}
}
