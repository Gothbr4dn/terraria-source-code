using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Terraria.Audio;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.Net;
using Terraria.Net.Sockets;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria
{
	public class Netplay
	{
		private class SetRemoteIPRequestInfo
		{
			public int RequestId;

			public Action SuccessCallback;

			public string RemoteAddress;
		}

		public const int MaxConnections = 256;

		public const int NetBufferSize = 1024;

		public static string BanFilePath = "banlist.txt";

		public static string ServerPassword = "";

		public static RemoteClient[] Clients = new RemoteClient[256];

		public static RemoteServer Connection = new RemoteServer();

		public static IPAddress ServerIP;

		public static string ServerIPText = "";

		public static ISocket TcpListener;

		public static int ListenPort = 7777;

		public static bool IsListening = true;

		public static bool UseUPNP = true;

		public static bool Disconnect;

		public static bool SpamCheck = false;

		public static bool HasClients;

		private static Thread _serverThread;

		public static MessageBuffer fullBuffer = new MessageBuffer();

		private static int _currentRequestId;

		private static UdpClient BroadcastClient = null;

		private static Thread broadcastThread = null;

		public static event Action OnDisconnect;

		private static void UpdateServerInMainThread()
		{
			for (int i = 0; i < 256; i++)
			{
				if (NetMessage.buffer[i].checkBytes)
				{
					NetMessage.CheckBytes(i);
				}
			}
		}

		private static string GetLocalIPAddress()
		{
			string result = "";
			IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
			foreach (IPAddress iPAddress in addressList)
			{
				if (iPAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					result = iPAddress.ToString();
					break;
				}
			}
			return result;
		}

		private static void ResetNetDiag()
		{
			Main.ActiveNetDiagnosticsUI.Reset();
		}

		public static void ResetSections()
		{
			for (int i = 0; i < 256; i++)
			{
				for (int j = 0; j < Main.maxSectionsX; j++)
				{
					for (int k = 0; k < Main.maxSectionsY; k++)
					{
						Clients[i].TileSections[j, k] = false;
					}
				}
			}
		}

		public static void AddBan(int plr)
		{
			RemoteAddress remoteAddress = Clients[plr].Socket.GetRemoteAddress();
			using StreamWriter streamWriter = new StreamWriter(BanFilePath, append: true);
			streamWriter.WriteLine("//" + Main.player[plr].name);
			streamWriter.WriteLine(remoteAddress.GetIdentifier());
		}

		public static bool IsBanned(RemoteAddress address)
		{
			try
			{
				string identifier = address.GetIdentifier();
				if (File.Exists(BanFilePath))
				{
					using StreamReader streamReader = new StreamReader(BanFilePath);
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						if (text == identifier)
						{
							return true;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		private static void OpenPort(int port)
		{
		}

		private static void ClosePort(int port)
		{
		}

		private static void ServerFullWriteCallBack(object state)
		{
		}

		private static void OnConnectionAccepted(ISocket client)
		{
			int num = FindNextOpenClientSlot();
			if (num != -1)
			{
				Clients[num].Reset();
				Clients[num].Socket = client;
			}
			else
			{
				lock (fullBuffer)
				{
					KickClient(client, NetworkText.FromKey("CLI.ServerIsFull"));
				}
			}
			if (FindNextOpenClientSlot() == -1)
			{
				StopListening();
				IsListening = false;
			}
		}

		private static void KickClient(ISocket client, NetworkText kickMessage)
		{
			BinaryWriter writer = fullBuffer.writer;
			if (writer == null)
			{
				fullBuffer.ResetWriter();
				writer = fullBuffer.writer;
			}
			writer.BaseStream.Position = 0L;
			long position = writer.BaseStream.Position;
			writer.BaseStream.Position += 2L;
			writer.Write((byte)2);
			kickMessage.Serialize(writer);
			if (Main.dedServ)
			{
				Console.WriteLine(Language.GetTextValue("CLI.ClientWasBooted", client.GetRemoteAddress().ToString(), kickMessage));
			}
			int num = (int)writer.BaseStream.Position;
			writer.BaseStream.Position = position;
			writer.Write((short)num);
			writer.BaseStream.Position = num;
			client.AsyncSend(fullBuffer.writeBuffer, 0, num, ServerFullWriteCallBack, client);
		}

		public static void OnConnectedToSocialServer(ISocket client)
		{
			StartSocialClient(client);
		}

		private static bool StartListening()
		{
			if (SocialAPI.Network != null)
			{
				SocialAPI.Network.StartListening(OnConnectionAccepted);
			}
			return TcpListener.StartListening(OnConnectionAccepted);
		}

		private static void StopListening()
		{
			if (SocialAPI.Network != null)
			{
				SocialAPI.Network.StopListening();
			}
			TcpListener.StopListening();
		}

		public static void StartServer()
		{
			InitializeServer();
			_serverThread = new Thread(ServerLoop)
			{
				IsBackground = true,
				Name = "Server Loop Thread"
			};
			_serverThread.Start();
		}

		private static void InitializeServer()
		{
			Connection.ResetSpecialFlags();
			ResetNetDiag();
			if (Main.rand == null)
			{
				Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
			}
			Main.myPlayer = 255;
			ServerIP = IPAddress.Any;
			Main.menuMode = 14;
			Main.statusText = Lang.menu[8].Value;
			Main.netMode = 2;
			Disconnect = false;
			for (int i = 0; i < 256; i++)
			{
				Clients[i] = new RemoteClient();
				Clients[i].Reset();
				Clients[i].Id = i;
				Clients[i].ReadBuffer = new byte[1024];
			}
			TcpListener = new TcpSocket();
			if (!Disconnect)
			{
				if (!StartListening())
				{
					Main.menuMode = 15;
					Main.statusText = Language.GetTextValue("Error.TriedToRunServerTwice");
					Disconnect = true;
				}
				Main.statusText = Language.GetTextValue("CLI.ServerStarted");
			}
			if (!UseUPNP)
			{
				return;
			}
			try
			{
				OpenPort(ListenPort);
			}
			catch (Exception)
			{
			}
		}

		private static void CleanupServer()
		{
			StopListening();
			try
			{
				ClosePort(ListenPort);
			}
			catch
			{
			}
			for (int i = 0; i < 256; i++)
			{
				Clients[i].Reset();
			}
			if (Main.menuMode != 15)
			{
				Main.netMode = 0;
				Main.menuMode = 10;
				WorldFile.SaveWorld();
				Main.menuMode = 0;
			}
			else
			{
				Main.netMode = 0;
			}
			Main.myPlayer = 0;
		}

		private static void ServerLoop()
		{
			int num = 0;
			StartBroadCasting();
			while (!Disconnect)
			{
				StartListeningIfNeeded();
				UpdateConnectedClients();
				num = (num + 1) % 10;
				Thread.Sleep((num == 0) ? 1 : 0);
			}
			StopBroadCasting();
			CleanupServer();
		}

		private static void UpdateConnectedClients()
		{
			int num = 0;
			for (int i = 0; i < 256; i++)
			{
				if (Clients[i].PendingTermination)
				{
					if (Clients[i].PendingTerminationApproved)
					{
						Clients[i].Reset();
						NetMessage.SyncDisconnectedPlayer(i);
					}
					continue;
				}
				if (Clients[i].IsConnected())
				{
					Clients[i].Update();
					num++;
					continue;
				}
				if (Clients[i].IsActive)
				{
					Clients[i].PendingTermination = true;
					Clients[i].PendingTerminationApproved = true;
					continue;
				}
				Clients[i].StatusText2 = "";
				if (i < 255)
				{
					bool active = Main.player[i].active;
					Main.player[i].active = false;
					if (active)
					{
						Player.Hooks.PlayerDisconnect(i);
					}
				}
			}
			HasClients = num != 0;
		}

		private static void StartListeningIfNeeded()
		{
			if (IsListening || !Clients.Any((RemoteClient client) => !client.IsConnected()))
			{
				return;
			}
			try
			{
				StartListening();
				IsListening = true;
			}
			catch
			{
				if (!Main.ignoreErrors)
				{
					throw;
				}
			}
		}

		private static void UpdateClientInMainThread()
		{
			if (Main.netMode == 1 && Connection.Socket.IsConnected() && !Connection.ServerWantsToRunCheckBytesInClientLoopThread && NetMessage.buffer[256].checkBytes)
			{
				NetMessage.CheckBytes();
			}
		}

		public static void AddCurrentServerToRecentList()
		{
			if (Connection.Socket.GetRemoteAddress().Type != 0)
			{
				return;
			}
			for (int i = 0; i < Main.maxMP; i++)
			{
				if (Main.recentIP[i].ToLower() == ServerIPText.ToLower() && Main.recentPort[i] == ListenPort)
				{
					for (int j = i; j < Main.maxMP - 1; j++)
					{
						Main.recentIP[j] = Main.recentIP[j + 1];
						Main.recentPort[j] = Main.recentPort[j + 1];
						Main.recentWorld[j] = Main.recentWorld[j + 1];
					}
				}
			}
			for (int num = Main.maxMP - 1; num > 0; num--)
			{
				Main.recentIP[num] = Main.recentIP[num - 1];
				Main.recentPort[num] = Main.recentPort[num - 1];
				Main.recentWorld[num] = Main.recentWorld[num - 1];
			}
			Main.recentIP[0] = ServerIPText;
			Main.recentPort[0] = ListenPort;
			Main.recentWorld[0] = Main.worldName;
			Main.SaveRecent();
		}

		public static void SocialClientLoop(object threadContext)
		{
			ISocket socket = (ISocket)threadContext;
			ClientLoopSetup(socket.GetRemoteAddress());
			Connection.Socket = socket;
			InnerClientLoop();
		}

		public static void TcpClientLoop()
		{
			ClientLoopSetup(new TcpAddress(ServerIP, ListenPort));
			Main.menuMode = 14;
			bool flag = true;
			while (flag)
			{
				flag = false;
				try
				{
					Connection.Socket.Connect(new TcpAddress(ServerIP, ListenPort));
					flag = false;
				}
				catch
				{
					Thread.Sleep(200);
					Connection.Socket.Close();
					Connection.Socket = new TcpSocket();
					if (!Disconnect && Main.gameMenu)
					{
						flag = true;
					}
				}
			}
			InnerClientLoop();
		}

		private static void ClientLoopSetup(RemoteAddress address)
		{
			Connection.ResetSpecialFlags();
			ResetNetDiag();
			Main.ServerSideCharacter = false;
			if (Main.rand == null)
			{
				Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
			}
			Main.player[Main.myPlayer].hostile = false;
			Main.clientPlayer = Main.player[Main.myPlayer].clientClone();
			for (int i = 0; i < 255; i++)
			{
				if (i != Main.myPlayer)
				{
					Main.player[i] = new Player();
				}
			}
			Main.netMode = 1;
			Main.menuMode = 14;
			if (!Main.autoPass)
			{
				Main.statusText = Language.GetTextValue("Net.ConnectingTo", address.GetFriendlyName());
			}
			Disconnect = false;
			Connection = new RemoteServer();
			Connection.ReadBuffer = new byte[1024];
		}

		private static void InnerClientLoop()
		{
			try
			{
				NetMessage.buffer[256].Reset();
				int num = -1;
				while (!Disconnect)
				{
					if (Connection.Socket.IsConnected())
					{
						if (Connection.ServerWantsToRunCheckBytesInClientLoopThread && NetMessage.buffer[256].checkBytes)
						{
							NetMessage.CheckBytes();
						}
						Connection.IsActive = true;
						if (Connection.State == 0)
						{
							Main.statusText = Language.GetTextValue("Net.FoundServer");
							Connection.State = 1;
							NetMessage.SendData(1);
						}
						if (Connection.State == 2 && num != Connection.State)
						{
							Main.statusText = Language.GetTextValue("Net.SendingPlayerData");
						}
						if (Connection.State == 3 && num != Connection.State)
						{
							Main.statusText = Language.GetTextValue("Net.RequestingWorldInformation");
						}
						if (Connection.State == 4)
						{
							WorldGen.worldCleared = false;
							Connection.State = 5;
							if (Main.cloudBGActive >= 1f)
							{
								Main.cloudBGAlpha = 1f;
							}
							else
							{
								Main.cloudBGAlpha = 0f;
							}
							Main.windSpeedCurrent = Main.windSpeedTarget;
							Cloud.resetClouds();
							Main.cloudAlpha = Main.maxRaining;
							Main.ToggleGameplayUpdates(state: false);
							WorldGen.clearWorld();
							if (Main.mapEnabled)
							{
								Main.Map.Load();
							}
						}
						if (Connection.State == 5 && Main.loadMapLock)
						{
							float num2 = (float)Main.loadMapLastX / (float)Main.maxTilesX;
							Main.statusText = Lang.gen[68].Value + " " + (int)(num2 * 100f + 1f) + "%";
						}
						else if (Connection.State == 5 && WorldGen.worldCleared)
						{
							Connection.State = 6;
							Main.player[Main.myPlayer].FindSpawn();
							NetMessage.SendData(8, -1, -1, null, Main.player[Main.myPlayer].SpawnX, Main.player[Main.myPlayer].SpawnY);
						}
						if (Connection.State == 6 && num != Connection.State)
						{
							Main.statusText = Language.GetTextValue("Net.RequestingTileData");
						}
						if (!Connection.IsReading && !Disconnect && Connection.Socket.IsDataAvailable())
						{
							Connection.IsReading = true;
							Connection.Socket.AsyncReceive(Connection.ReadBuffer, 0, Connection.ReadBuffer.Length, Connection.ClientReadCallBack);
						}
						if (Connection.StatusMax > 0 && Connection.StatusText != "")
						{
							if (Connection.StatusCount >= Connection.StatusMax)
							{
								Main.statusText = Language.GetTextValue("Net.StatusComplete", Connection.StatusText);
								Connection.StatusText = "";
								Connection.StatusMax = 0;
								Connection.StatusCount = 0;
							}
							else
							{
								Main.statusText = Connection.StatusText + ": " + (int)((float)Connection.StatusCount / (float)Connection.StatusMax * 100f) + "%";
							}
						}
						Thread.Sleep(1);
					}
					else if (Connection.IsActive)
					{
						Main.statusText = Language.GetTextValue("Net.LostConnection");
						Disconnect = true;
					}
					num = Connection.State;
				}
				try
				{
					Connection.Socket.Close();
				}
				catch
				{
				}
				if (!Main.gameMenu)
				{
					Main.gameMenu = true;
					Main.SwitchNetMode(0);
					MapHelper.noStatusText = true;
					Player.SavePlayer(Main.ActivePlayerFileData);
					Player.ClearPlayerTempInfo();
					Main.ActivePlayerFileData.StopPlayTimer();
					SoundEngine.StopTrackedSounds();
					MapHelper.noStatusText = false;
					Main.menuMode = 14;
				}
				NetMessage.buffer[256].Reset();
				if (Main.menuMode == 15 && Main.statusText == Language.GetTextValue("Net.LostConnection"))
				{
					Main.menuMode = 14;
				}
				if (Connection.StatusText != "" && Connection.StatusText != null)
				{
					Main.statusText = Language.GetTextValue("Net.LostConnection");
				}
				Connection.StatusCount = 0;
				Connection.StatusMax = 0;
				Connection.StatusText = "";
				Main.SwitchNetMode(0);
			}
			catch (Exception value)
			{
				try
				{
					using StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true);
					streamWriter.WriteLine(DateTime.Now);
					streamWriter.WriteLine(value);
					streamWriter.WriteLine("");
				}
				catch
				{
				}
				Disconnect = true;
			}
			if (Netplay.OnDisconnect != null)
			{
				Netplay.OnDisconnect();
			}
		}

		private static int FindNextOpenClientSlot()
		{
			for (int i = 0; i < Main.maxNetPlayers; i++)
			{
				if (!Clients[i].IsConnected())
				{
					return i;
				}
			}
			return -1;
		}

		public static void StartSocialClient(ISocket socket)
		{
			Thread thread = new Thread(SocialClientLoop);
			thread.Name = "Social Client Thread";
			thread.IsBackground = true;
			thread.Start(socket);
		}

		public static void StartTcpClient()
		{
			Thread thread = new Thread(TcpClientLoop);
			thread.Name = "TCP Client Thread";
			thread.IsBackground = true;
			thread.Start();
		}

		public static bool SetRemoteIP(string remoteAddress)
		{
			return SetRemoteIPOld(remoteAddress);
		}

		public static bool SetRemoteIPOld(string remoteAddress)
		{
			try
			{
				if (IPAddress.TryParse(remoteAddress, out var address))
				{
					ServerIP = address;
					ServerIPText = address.ToString();
					return true;
				}
				IPAddress[] addressList = Dns.GetHostEntry(remoteAddress).AddressList;
				for (int i = 0; i < addressList.Length; i++)
				{
					if (addressList[i].AddressFamily == AddressFamily.InterNetwork)
					{
						ServerIP = addressList[i];
						ServerIPText = remoteAddress;
						return true;
					}
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		public static void SetRemoteIPAsync(string remoteAddress, Action successCallBack)
		{
			try
			{
				if (IPAddress.TryParse(remoteAddress, out var address))
				{
					ServerIP = address;
					ServerIPText = address.ToString();
					successCallBack();
				}
				else
				{
					InvalidateAllOngoingIPSetAttempts();
					Dns.BeginGetHostAddresses(remoteAddress, SetRemoteIPAsyncCallback, new SetRemoteIPRequestInfo
					{
						RequestId = _currentRequestId,
						SuccessCallback = successCallBack,
						RemoteAddress = remoteAddress
					});
				}
			}
			catch (Exception)
			{
			}
		}

		public static void InvalidateAllOngoingIPSetAttempts()
		{
			_currentRequestId++;
		}

		private static void SetRemoteIPAsyncCallback(IAsyncResult ar)
		{
			SetRemoteIPRequestInfo setRemoteIPRequestInfo = (SetRemoteIPRequestInfo)ar.AsyncState;
			if (setRemoteIPRequestInfo.RequestId != _currentRequestId)
			{
				return;
			}
			try
			{
				bool flag = false;
				IPAddress[] array = Dns.EndGetHostAddresses(ar);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].AddressFamily == AddressFamily.InterNetwork)
					{
						ServerIP = array[i];
						ServerIPText = setRemoteIPRequestInfo.RemoteAddress;
						flag = true;
						break;
					}
				}
				if (flag)
				{
					setRemoteIPRequestInfo.SuccessCallback();
				}
			}
			catch (Exception)
			{
			}
		}

		public static void Initialize()
		{
			NetMessage.buffer[256] = new MessageBuffer();
			NetMessage.buffer[256].whoAmI = 256;
		}

		public static void UpdateInMainThread()
		{
			UpdateClientInMainThread();
		}

		public static int GetSectionX(int x)
		{
			return x / 200;
		}

		public static int GetSectionY(int y)
		{
			return y / 150;
		}

		private static void BroadcastThread()
		{
			BroadcastClient = new UdpClient();
			new IPEndPoint(IPAddress.Any, 0);
			BroadcastClient.EnableBroadcast = true;
			new DateTime(0L);
			long num = 0L;
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				int value = 1010;
				binaryWriter.Write(value);
				binaryWriter.Write(ListenPort);
				binaryWriter.Write(Main.worldName);
				string text = Dns.GetHostName();
				if (text == "localhost")
				{
					text = Environment.MachineName;
				}
				binaryWriter.Write(text);
				binaryWriter.Write((ushort)Main.maxTilesX);
				binaryWriter.Write(Main.ActiveWorldFileData.HasCrimson);
				binaryWriter.Write(Main.ActiveWorldFileData.GameMode);
				binaryWriter.Write((byte)Main.maxNetPlayers);
				num = memoryStream.Position;
				binaryWriter.Write((byte)0);
				binaryWriter.Write(Main.ActiveWorldFileData.IsHardMode);
				binaryWriter.Flush();
				array = memoryStream.ToArray();
			}
			while (true)
			{
				int num2 = 0;
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active)
					{
						num2++;
					}
				}
				array[(int)num] = (byte)num2;
				try
				{
					BroadcastClient.Send(array, array.Length, new IPEndPoint(IPAddress.Broadcast, 8888));
				}
				catch
				{
				}
				Thread.Sleep(1000);
			}
		}

		public static void StartBroadCasting()
		{
			if (broadcastThread != null)
			{
				StopBroadCasting();
			}
			broadcastThread = new Thread(BroadcastThread);
			broadcastThread.Start();
		}

		public static void StopBroadCasting()
		{
			if (broadcastThread != null)
			{
				broadcastThread.Abort();
				broadcastThread = null;
			}
			if (BroadcastClient != null)
			{
				BroadcastClient.Close();
				BroadcastClient = null;
			}
		}
	}
}
