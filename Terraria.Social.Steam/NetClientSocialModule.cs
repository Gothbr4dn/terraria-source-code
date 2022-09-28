using System.Diagnostics;
using Steamworks;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;
using Terraria.Social.WeGame;

namespace Terraria.Social.Steam
{
	public class NetClientSocialModule : NetSocialModule
	{
		private Callback<GameLobbyJoinRequested_t> _gameLobbyJoinRequested;

		private Callback<P2PSessionRequest_t> _p2pSessionRequest;

		private Callback<P2PSessionConnectFail_t> _p2pSessionConnectfail;

		private HAuthTicket _authTicket = HAuthTicket.Invalid;

		private byte[] _authData = new byte[1021];

		private uint _authDataLength;

		private bool _hasLocalHost;

		public NetClientSocialModule()
			: base(2, 1)
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)


		public override void Initialize()
		{
			base.Initialize();
			_gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create((DispatchDelegate<GameLobbyJoinRequested_t>)OnLobbyJoinRequest);
			_p2pSessionRequest = Callback<P2PSessionRequest_t>.Create((DispatchDelegate<P2PSessionRequest_t>)OnP2PSessionRequest);
			_p2pSessionConnectfail = Callback<P2PSessionConnectFail_t>.Create((DispatchDelegate<P2PSessionConnectFail_t>)OnSessionConnectFail);
			Main.OnEngineLoad += CheckParameters;
		}

		private void CheckParameters()
		{
			if (Program.LaunchParameters.ContainsKey("+connect_lobby") && ulong.TryParse(Program.LaunchParameters["+connect_lobby"], out var result))
			{
				ConnectToLobby(result);
			}
		}

		public void ConnectToLobby(ulong lobbyId)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			CSteamID lobbySteamId = new CSteamID(lobbyId);
			if (((CSteamID)(ref lobbySteamId)).IsValid())
			{
				Main.OpenPlayerSelect(delegate(PlayerFileData playerData)
				{
					//IL_0041: Unknown result type (might be due to invalid IL or missing references)
					Main.ServerSideCharacter = false;
					playerData.SetAsActive();
					Main.menuMode = 882;
					Main.statusText = Language.GetTextValue("Social.Joining");
					WeGameHelper.WriteDebugString(" CheckParameters， lobby.join");
					_lobby.Join(lobbySteamId, OnLobbyEntered);
				});
			}
		}

		public override void LaunchLocalServer(Process process, ServerMode mode)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			WeGameHelper.WriteDebugString("LaunchLocalServer");
			if (_lobby.State != 0)
			{
				_lobby.Leave();
			}
			ProcessStartInfo startInfo = process.StartInfo;
			startInfo.Arguments = startInfo.Arguments + " -steam -localsteamid " + SteamUser.GetSteamID().m_SteamID;
			if (mode.HasFlag(ServerMode.Lobby))
			{
				_hasLocalHost = true;
				if (mode.HasFlag(ServerMode.FriendsCanJoin))
				{
					process.StartInfo.Arguments += " -lobby friends";
				}
				else
				{
					process.StartInfo.Arguments += " -lobby private";
				}
				if (mode.HasFlag(ServerMode.FriendsOfFriends))
				{
					process.StartInfo.Arguments += " -friendsoffriends";
				}
			}
			SteamFriends.SetRichPresence("status", Language.GetTextValue("Social.StatusInGame"));
			Netplay.OnDisconnect += OnDisconnect;
			process.Start();
		}

		public override ulong GetLobbyId()
		{
			return 0uL;
		}

		public override bool StartListening(SocketConnectionAccepted callback)
		{
			return false;
		}

		public override void StopListening()
		{
		}

		public override void Close(RemoteAddress address)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			SteamFriends.ClearRichPresence();
			CSteamID user = RemoteAddressToSteamId(address);
			Close(user);
		}

		public override bool CanInvite()
		{
			if (_hasLocalHost || _lobby.State == LobbyState.Active || Main.LobbyId != 0L)
			{
				return Main.netMode != 0;
			}
			return false;
		}

		public override void OpenInviteInterface()
		{
			_lobby.OpenInviteOverlay();
		}

		private void Close(CSteamID user)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			if (_connectionStateMap.ContainsKey(user))
			{
				SteamNetworking.CloseP2PSessionWithUser(user);
				ClearAuthTicket();
				_connectionStateMap[user] = ConnectionState.Inactive;
				_lobby.Leave();
				_reader.ClearUser(user);
				_writer.ClearUser(user);
			}
		}

		public override void Connect(RemoteAddress address)
		{
		}

		public override void CancelJoin()
		{
			if (_lobby.State != 0)
			{
				_lobby.Leave();
			}
		}

		private void OnLobbyJoinRequest(GameLobbyJoinRequested_t result)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			WeGameHelper.WriteDebugString(" OnLobbyJoinRequest");
			if (_lobby.State != 0)
			{
				_lobby.Leave();
			}
			string friendName = SteamFriends.GetFriendPersonaName(result.m_steamIDFriend);
			Main.QueueMainThreadAction(delegate
			{
				Main.OpenPlayerSelect(delegate(PlayerFileData playerData)
				{
					//IL_003c: Unknown result type (might be due to invalid IL or missing references)
					Main.ServerSideCharacter = false;
					playerData.SetAsActive();
					Main.menuMode = 882;
					Main.statusText = Language.GetTextValue("Social.JoiningFriend", friendName);
					_lobby.Join(result.m_steamIDLobby, OnLobbyEntered);
				});
			});
		}

		private void OnLobbyEntered(LobbyEnter_t result, bool failure)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			WeGameHelper.WriteDebugString(" OnLobbyEntered");
			SteamNetworking.AllowP2PPacketRelay(true);
			SendAuthTicket(_lobby.Owner);
			int num = 0;
			P2PSessionState_t val = default(P2PSessionState_t);
			while (SteamNetworking.GetP2PSessionState(_lobby.Owner, ref val) && val.m_bConnectionActive != 1)
			{
				switch (val.m_eP2PSessionError)
				{
				case 2:
					ClearAuthTicket();
					return;
				case 1:
					ClearAuthTicket();
					return;
				case 3:
					ClearAuthTicket();
					return;
				case 5:
					ClearAuthTicket();
					return;
				case 4:
					if (++num > 5)
					{
						ClearAuthTicket();
						return;
					}
					SteamNetworking.CloseP2PSessionWithUser(_lobby.Owner);
					SendAuthTicket(_lobby.Owner);
					break;
				}
			}
			_connectionStateMap[_lobby.Owner] = ConnectionState.Connected;
			SteamFriends.SetPlayedWith(_lobby.Owner);
			SteamFriends.SetRichPresence("status", Language.GetTextValue("Social.StatusInGame"));
			Main.clrInput();
			Netplay.ServerPassword = "";
			Main.GetInputText("");
			Main.autoPass = false;
			Main.netMode = 1;
			Netplay.OnConnectedToSocialServer(new SocialSocket(new SteamAddress(_lobby.Owner)));
		}

		private void SendAuthTicket(CSteamID address)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			WeGameHelper.WriteDebugString(" SendAuthTicket");
			if (_authTicket == HAuthTicket.Invalid)
			{
				_authTicket = SteamUser.GetAuthSessionTicket(_authData, _authData.Length, ref _authDataLength);
			}
			int num = (int)(_authDataLength + 3);
			byte[] array = new byte[num];
			array[0] = (byte)((uint)num & 0xFFu);
			array[1] = (byte)((uint)(num >> 8) & 0xFFu);
			array[2] = 93;
			for (int i = 0; i < _authDataLength; i++)
			{
				array[i + 3] = _authData[i];
			}
			SteamNetworking.SendP2PPacket(address, array, (uint)num, (EP2PSend)2, 1);
		}

		private void ClearAuthTicket()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (_authTicket != HAuthTicket.Invalid)
			{
				SteamUser.CancelAuthTicket(_authTicket);
			}
			_authTicket = HAuthTicket.Invalid;
			for (int i = 0; i < _authData.Length; i++)
			{
				_authData[i] = 0;
			}
			_authDataLength = 0u;
		}

		private void OnDisconnect()
		{
			SteamFriends.ClearRichPresence();
			_hasLocalHost = false;
			Netplay.OnDisconnect -= OnDisconnect;
		}

		private void OnSessionConnectFail(P2PSessionConnectFail_t result)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			WeGameHelper.WriteDebugString(" OnSessionConnectFail");
			Close(result.m_steamIDRemote);
		}

		private void OnP2PSessionRequest(P2PSessionRequest_t result)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			WeGameHelper.WriteDebugString(" OnP2PSessionRequest");
			CSteamID steamIDRemote = result.m_steamIDRemote;
			if (_connectionStateMap.ContainsKey(steamIDRemote) && _connectionStateMap[steamIDRemote] != 0)
			{
				SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
			}
		}
	}
}
