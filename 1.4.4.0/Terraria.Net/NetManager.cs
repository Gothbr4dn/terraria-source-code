using System.Collections.Generic;
using System.IO;
using Terraria.Net.Sockets;

namespace Terraria.Net
{
	public class NetManager
	{
		private class PacketTypeStorage<T> where T : NetModule
		{
			public static ushort Id;

			public static T Module;
		}

		public delegate bool BroadcastCondition(int clientIndex);

		public static readonly NetManager Instance = new NetManager();

		private Dictionary<ushort, NetModule> _modules = new Dictionary<ushort, NetModule>();

		private ushort _moduleCount;

		private NetManager()
		{
		}

		public void Register<T>() where T : NetModule, new()
		{
			T val = new T();
			PacketTypeStorage<T>.Id = _moduleCount;
			PacketTypeStorage<T>.Module = val;
			_modules[_moduleCount] = val;
			_moduleCount++;
		}

		public NetModule GetModule<T>() where T : NetModule
		{
			return PacketTypeStorage<T>.Module;
		}

		public ushort GetId<T>() where T : NetModule
		{
			return PacketTypeStorage<T>.Id;
		}

		public void Read(BinaryReader reader, int userId, int readLength)
		{
			ushort num = reader.ReadUInt16();
			if (_modules.ContainsKey(num))
			{
				_modules[num].Deserialize(reader, userId);
			}
			Main.ActiveNetDiagnosticsUI.CountReadModuleMessage(num, readLength);
		}

		public void Broadcast(NetPacket packet, int ignoreClient = -1)
		{
			for (int i = 0; i < 256; i++)
			{
				if (i != ignoreClient && Netplay.Clients[i].IsConnected())
				{
					SendData(Netplay.Clients[i].Socket, packet);
				}
			}
		}

		public void Broadcast(NetPacket packet, BroadcastCondition conditionToBroadcast, int ignoreClient = -1)
		{
			for (int i = 0; i < 256; i++)
			{
				if (i != ignoreClient && Netplay.Clients[i].IsConnected() && conditionToBroadcast(i))
				{
					SendData(Netplay.Clients[i].Socket, packet);
				}
			}
		}

		public void SendToSelf(NetPacket packet)
		{
			packet.Reader.BaseStream.Position = 3L;
			Read(packet.Reader, Main.myPlayer, packet.Length);
			SendCallback(packet);
			Main.ActiveNetDiagnosticsUI.CountSentModuleMessage(packet.Id, packet.Length);
		}

		public void BroadcastOrLoopback(NetPacket packet)
		{
			if (Main.netMode == 2)
			{
				Broadcast(packet);
			}
			else if (Main.netMode == 0)
			{
				SendToSelf(packet);
			}
		}

		public void SendToServerOrLoopback(NetPacket packet)
		{
			if (Main.netMode == 1)
			{
				SendToServer(packet);
			}
			else if (Main.netMode == 0)
			{
				SendToSelf(packet);
			}
		}

		public void SendToServerAndSelf(NetPacket packet)
		{
			if (Main.netMode == 1)
			{
				SendToServer(packet);
				SendToSelf(packet);
			}
			else if (Main.netMode == 0)
			{
				SendToSelf(packet);
			}
		}

		public void SendToServer(NetPacket packet)
		{
			SendData(Netplay.Connection.Socket, packet);
		}

		public void SendToClient(NetPacket packet, int playerId)
		{
			SendData(Netplay.Clients[playerId].Socket, packet);
		}

		private void SendData(ISocket socket, NetPacket packet)
		{
			if (Main.netMode != 0)
			{
				packet.ShrinkToFit();
				try
				{
					socket.AsyncSend(packet.Buffer.Data, 0, packet.Length, SendCallback, packet);
				}
				catch
				{
				}
				Main.ActiveNetDiagnosticsUI.CountSentModuleMessage(packet.Id, packet.Length);
			}
		}

		public static void SendCallback(object state)
		{
			((NetPacket)state).Recycle();
		}
	}
}
