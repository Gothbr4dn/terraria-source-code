using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using rail;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace Terraria.Social.WeGame
{
	public class NetClientSocialModule : NetSocialModule
	{
		private RailCallBackHelper _callbackHelper = new RailCallBackHelper();

		private bool _hasLocalHost;

		private IPCServer server = new IPCServer();

		private readonly string _serverIDMedataKey = "terraria.serverid";

		private RailID _inviter_id = new RailID();

		private List<PlayerPersonalInfo> _player_info_list;

		private MessageDispatcherServer _msgServer;

		private void OnIPCClientAccess()
		{
			WeGameHelper.WriteDebugString("IPC client access");
			SendFriendListToLocalServer();
		}

		private void LazyCreateWeGameMsgServer()
		{
			if (_msgServer == null)
			{
				_msgServer = new MessageDispatcherServer();
				_msgServer.Init("WeGame.Terraria.Message.Server");
				_msgServer.OnMessage += OnWegameMessage;
				_msgServer.OnIPCClientAccess += OnIPCClientAccess;
				CoreSocialModule.OnTick += _msgServer.Tick;
				_msgServer.Start();
			}
		}

		private void OnWegameMessage(IPCMessage message)
		{
			if (message.GetCmd() == IPCMessageType.IPCMessageTypeReportServerID)
			{
				message.Parse<ReportServerID>(out var value);
				OnReportServerID(value);
			}
		}

		private void OnReportServerID(ReportServerID reportServerID)
		{
			WeGameHelper.WriteDebugString("OnReportServerID - " + reportServerID._serverID);
			AsyncSetMyMetaData(_serverIDMedataKey, reportServerID._serverID);
			AsyncSetInviteCommandLine(reportServerID._serverID);
		}

		public override void Initialize()
		{
			base.Initialize();
			RegisterRailEvent();
			AsyncGetFriendsInfo();
			_reader.SetReadEvent(OnPacketRead);
			_reader.SetLocalPeer(GetLocalPeer());
			_writer.SetLocalPeer(GetLocalPeer());
			Main.OnEngineLoad += CheckParameters;
		}

		private void AsyncSetInviteCommandLine(string cmdline)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			rail_api.RailFactory().RailFriends().AsyncSetInviteCommandLine(cmdline, "");
		}

		private void AsyncSetMyMetaData(string key, string value)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			List<RailKeyValue> list = new List<RailKeyValue>();
			RailKeyValue val = new RailKeyValue();
			val.key = key;
			val.value = value;
			list.Add(val);
			rail_api.RailFactory().RailFriends().AsyncSetMyMetadata(list, "");
		}

		private bool TryAuthUserByRecvData(RailID user, byte[] data, int length)
		{
			WeGameHelper.WriteDebugString("TryAuthUserByRecvData user:{0}", ((RailComparableID)user).id_);
			if (length < 3)
			{
				WeGameHelper.WriteDebugString("Failed to validate authentication packet: Too short. (Length: " + length + ")");
				return false;
			}
			int num = (data[1] << 8) | data[0];
			if (num != length)
			{
				WeGameHelper.WriteDebugString("Failed to validate authentication packet: Packet size mismatch. (" + num + "!=" + length + ")");
				return false;
			}
			if (data[2] != 93)
			{
				WeGameHelper.WriteDebugString("Failed to validate authentication packet: Packet type is not correct. (Type: " + data[2] + ")");
				return false;
			}
			return true;
		}

		private bool OnPacketRead(byte[] data, int size, RailID user)
		{
			if (!_connectionStateMap.ContainsKey(user))
			{
				return false;
			}
			ConnectionState connectionState = _connectionStateMap[user];
			if (connectionState == ConnectionState.Authenticating)
			{
				if (!TryAuthUserByRecvData(user, data, size))
				{
					WeGameHelper.WriteDebugString(" Auth Server Ticket Failed");
					Close(user);
				}
				else
				{
					WeGameHelper.WriteDebugString("OnRailAuthSessionTicket Auth Success..");
					OnAuthSuccess(user);
				}
				return false;
			}
			return connectionState == ConnectionState.Connected;
		}

		private void OnAuthSuccess(RailID remote_peer)
		{
			if (_connectionStateMap.ContainsKey(remote_peer))
			{
				_connectionStateMap[remote_peer] = ConnectionState.Connected;
				AsyncSetPlayWith(_inviter_id);
				AsyncSetMyMetaData("status", Language.GetTextValue("Social.StatusInGame"));
				AsyncSetMyMetaData(_serverIDMedataKey, ((RailComparableID)remote_peer).id_.ToString());
				Main.clrInput();
				Netplay.ServerPassword = "";
				Main.GetInputText("");
				Main.autoPass = false;
				Main.netMode = 1;
				Netplay.OnConnectedToSocialServer(new SocialSocket(new WeGameAddress(remote_peer, GetFriendNickname(_inviter_id))));
				WeGameHelper.WriteDebugString("OnConnectToSocialServer server:" + ((RailComparableID)remote_peer).id_);
			}
		}

		private bool GetRailConnectIDFromCmdLine(RailID server_id)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			foreach (string text in commandLineArgs)
			{
				string text2 = "--rail_connect_cmd=";
				int num = text.IndexOf(text2);
				if (num != -1)
				{
					ulong result = 0uL;
					if (ulong.TryParse(text.Substring(num + text2.Length), out result))
					{
						((RailComparableID)server_id).id_ = result;
						return true;
					}
				}
			}
			return false;
		}

		private void CheckParameters()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			RailID server_id = new RailID();
			if (!GetRailConnectIDFromCmdLine(server_id))
			{
				return;
			}
			if (((RailComparableID)server_id).IsValid())
			{
				Main.OpenPlayerSelect(delegate(PlayerFileData playerData)
				{
					Main.ServerSideCharacter = false;
					playerData.SetAsActive();
					Main.menuMode = 882;
					Main.statusText = Language.GetTextValue("Social.Joining");
					WeGameHelper.WriteDebugString(" CheckParameters， lobby.join");
					JoinServer(server_id);
				});
			}
			else
			{
				WeGameHelper.WriteDebugString("Invalid RailID passed to +connect_lobby");
			}
		}

		public override void LaunchLocalServer(Process process, ServerMode mode)
		{
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			if (_lobby.State != 0)
			{
				_lobby.Leave();
			}
			LazyCreateWeGameMsgServer();
			ProcessStartInfo startInfo = process.StartInfo;
			startInfo.Arguments = startInfo.Arguments + " -wegame -localwegameid " + ((RailComparableID)GetLocalPeer()).id_;
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
			string text = default(string);
			rail_api.RailFactory().RailUtils().GetLaunchAppParameters((EnumRailLaunchAppType)2, ref text);
			ProcessStartInfo startInfo2 = process.StartInfo;
			startInfo2.Arguments = startInfo2.Arguments + " " + text;
			WeGameHelper.WriteDebugString("LaunchLocalServer,cmd_line:" + process.StartInfo.Arguments);
			AsyncSetMyMetaData("status", Language.GetTextValue("Social.StatusInGame"));
			Netplay.OnDisconnect += OnDisconnect;
			process.Start();
		}

		public override void Shutdown()
		{
			AsyncSetInviteCommandLine("");
			CleanMyMetaData();
			UnRegisterRailEvent();
			base.Shutdown();
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
			CleanMyMetaData();
			RailID remote_peer = RemoteAddressToRailId(address);
			Close(remote_peer);
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

		private void Close(RailID remote_peer)
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			if (_connectionStateMap.ContainsKey(remote_peer))
			{
				WeGameHelper.WriteDebugString("CloseRemotePeer, remote:{0}", ((RailComparableID)remote_peer).id_);
				rail_api.RailFactory().RailNetworkHelper().CloseSession(GetLocalPeer(), remote_peer);
				_connectionStateMap[remote_peer] = ConnectionState.Inactive;
				_lobby.Leave();
				_reader.ClearUser(remote_peer);
				_writer.ClearUser(remote_peer);
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

		private void RegisterRailEvent()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			RAILEventID[] array = (RAILEventID[])(object)new RAILEventID[7]
			{
				(RAILEventID)16001,
				(RAILEventID)16002,
				(RAILEventID)13503,
				(RAILEventID)13501,
				(RAILEventID)12003,
				(RAILEventID)12002,
				(RAILEventID)12010
			};
			foreach (RAILEventID val in array)
			{
				_callbackHelper.RegisterCallback(val, new RailEventCallBackHandler(OnRailEvent));
			}
		}

		private void UnRegisterRailEvent()
		{
			_callbackHelper.UnregisterAllCallback();
		}

		public void OnRailEvent(RAILEventID id, EventBase data)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Invalid comparison between Unknown and I4
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Invalid comparison between Unknown and I4
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Invalid comparison between Unknown and I4
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Invalid comparison between Unknown and I4
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Invalid comparison between Unknown and I4
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Invalid comparison between Unknown and I4
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Invalid comparison between Unknown and I4
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Invalid comparison between Unknown and I4
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Expected O, but got Unknown
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected O, but got Unknown
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			WeGameHelper.WriteDebugString("OnRailEvent,id=" + ((object)(RAILEventID)(ref id)).ToString() + " ,result=" + ((object)(RailResult)(ref data.result)).ToString());
			if ((int)id <= 12010)
			{
				if ((int)id != 12002)
				{
					if ((int)id != 12003)
					{
						if ((int)id == 12010)
						{
							OnFriendlistChange((RailFriendsListChanged)data);
						}
					}
					else
					{
						OnGetFriendMetaData((RailFriendsGetMetadataResult)data);
					}
				}
				else
				{
					OnRailSetMetaData((RailFriendsSetMetadataResult)data);
				}
			}
			else if ((int)id <= 13503)
			{
				if ((int)id != 13501)
				{
					if ((int)id == 13503)
					{
						OnRailRespondInvation((RailUsersRespondInvitation)data);
					}
				}
				else
				{
					OnRailGetUsersInfo((RailUsersInfoData)data);
				}
			}
			else if ((int)id != 16001)
			{
				if ((int)id == 16002)
				{
					OnRailCreateSessionFailed((CreateSessionFailed)data);
				}
			}
			else
			{
				OnRailCreateSessionRequest((CreateSessionRequest)data);
			}
		}

		private string DumpMataDataString(List<RailKeyValueResult> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (RailKeyValueResult item in list)
			{
				stringBuilder.Append("key: " + item.key + " value: " + item.value);
			}
			return stringBuilder.ToString();
		}

		private string GetValueByKey(string key, List<RailKeyValueResult> list)
		{
			string result = null;
			foreach (RailKeyValueResult item in list)
			{
				if (item.key == key)
				{
					return item.value;
				}
			}
			return result;
		}

		private bool SendFriendListToLocalServer()
		{
			bool result = false;
			if (_hasLocalHost)
			{
				List<RailFriendInfo> list = new List<RailFriendInfo>();
				if (GetRailFriendList(list))
				{
					WeGameFriendListInfo t = new WeGameFriendListInfo
					{
						_friendList = list
					};
					IPCMessage iPCMessage = new IPCMessage();
					iPCMessage.Build(IPCMessageType.IPCMessageTypeNotifyFriendList, t);
					result = _msgServer.SendMessage(iPCMessage);
					WeGameHelper.WriteDebugString("NotifyFriendListToServer: " + result);
				}
			}
			return result;
		}

		private bool GetRailFriendList(List<RailFriendInfo> list)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			bool result = false;
			IRailFriends val = rail_api.RailFactory().RailFriends();
			if (val != null)
			{
				result = (int)val.GetFriendsList(list) == 0;
			}
			return result;
		}

		private void OnGetFriendMetaData(RailFriendsGetMetadataResult data)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			if ((int)((EventBase)data).result != 0 || data.friend_kvs.Count <= 0)
			{
				return;
			}
			WeGameHelper.WriteDebugString("OnGetFriendMetaData - " + DumpMataDataString(data.friend_kvs));
			string valueByKey = GetValueByKey(_serverIDMedataKey, data.friend_kvs);
			if (valueByKey == null)
			{
				return;
			}
			if (valueByKey.Length > 0)
			{
				RailID val = new RailID();
				((RailComparableID)val).id_ = ulong.Parse(valueByKey);
				if (((RailComparableID)val).IsValid())
				{
					JoinServer(val);
				}
				else
				{
					WeGameHelper.WriteDebugString("JoinServer failed, invalid server id");
				}
			}
			else
			{
				WeGameHelper.WriteDebugString("can not find server id key");
			}
		}

		private void JoinServer(RailID server_id)
		{
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			WeGameHelper.WriteDebugString("JoinServer:{0}", ((RailComparableID)server_id).id_);
			_connectionStateMap[server_id] = ConnectionState.Authenticating;
			int num = 3;
			byte[] array = new byte[num];
			array[0] = (byte)((uint)num & 0xFFu);
			array[1] = (byte)((uint)(num >> 8) & 0xFFu);
			array[2] = 93;
			rail_api.RailFactory().RailNetworkHelper().SendReliableData(GetLocalPeer(), server_id, array, (uint)num);
		}

		private string GetFriendNickname(RailID rail_id)
		{
			if (_player_info_list != null)
			{
				foreach (PlayerPersonalInfo item in _player_info_list)
				{
					if ((RailComparableID)(object)item.rail_id == (RailComparableID)(object)rail_id)
					{
						return item.rail_name;
					}
				}
			}
			return "";
		}

		private void OnRailGetUsersInfo(RailUsersInfoData data)
		{
			_player_info_list = data.user_info_list;
		}

		private void OnFriendlistChange(RailFriendsListChanged data)
		{
			if (_hasLocalHost)
			{
				SendFriendListToLocalServer();
			}
		}

		private void AsyncGetFriendsInfo()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			IRailFriends val = rail_api.RailFactory().RailFriends();
			if (val == null)
			{
				return;
			}
			List<RailFriendInfo> list = new List<RailFriendInfo>();
			val.GetFriendsList(list);
			List<RailID> list2 = new List<RailID>();
			foreach (RailFriendInfo item in list)
			{
				list2.Add(item.friend_rail_id);
			}
			val.AsyncGetPersonalInfo(list2, "");
		}

		private void AsyncSetPlayWith(RailID rail_id)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			List<RailUserPlayedWith> list = new List<RailUserPlayedWith>();
			RailUserPlayedWith val = new RailUserPlayedWith();
			val.rail_id = rail_id;
			list.Add(val);
			IRailFriends val2 = rail_api.RailFactory().RailFriends();
			if (val2 != null)
			{
				val2.AsyncReportPlayedWithUserList(list, "");
			}
		}

		private void OnRailSetMetaData(RailFriendsSetMetadataResult data)
		{
			WeGameHelper.WriteDebugString("OnRailSetMetaData - " + ((object)(RailResult)(ref ((EventBase)data).result)).ToString());
		}

		private void OnRailRespondInvation(RailUsersRespondInvitation data)
		{
			WeGameHelper.WriteDebugString(" request join game");
			if (_lobby.State != 0)
			{
				_lobby.Leave();
			}
			_inviter_id = data.inviter_id;
			Main.OpenPlayerSelect(delegate(PlayerFileData playerData)
			{
				Main.ServerSideCharacter = false;
				playerData.SetAsActive();
				Main.menuMode = 882;
				Main.statusText = Language.GetTextValue("Social.JoiningFriend", GetFriendNickname(data.inviter_id));
				AsyncGetServerIDByOwener(data.inviter_id);
				WeGameHelper.WriteDebugString("inviter_id: " + ((RailComparableID)data.inviter_id).id_);
			});
		}

		private void AsyncGetServerIDByOwener(RailID ownerID)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			List<string> list = new List<string>();
			list.Add(_serverIDMedataKey);
			IRailFriends val = rail_api.RailFactory().RailFriends();
			if (val != null)
			{
				val.AsyncGetFriendMetadata(ownerID, list, "");
			}
		}

		private void OnRailCreateSessionRequest(CreateSessionRequest result)
		{
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			WeGameHelper.WriteDebugString("OnRailCreateSessionRequest");
			if (_connectionStateMap.ContainsKey(result.remote_peer) && _connectionStateMap[result.remote_peer] != 0)
			{
				WeGameHelper.WriteDebugString("AcceptSessionRequest, local{0}, remote:{1}", ((RailComparableID)result.local_peer).id_, ((RailComparableID)result.remote_peer).id_);
				rail_api.RailFactory().RailNetworkHelper().AcceptSessionRequest(result.local_peer, result.remote_peer);
			}
		}

		private void OnRailCreateSessionFailed(CreateSessionFailed result)
		{
			WeGameHelper.WriteDebugString("OnRailCreateSessionFailed, CloseRemote: local:{0}, remote:{1}", ((RailComparableID)result.local_peer).id_, ((RailComparableID)result.remote_peer).id_);
			Close(result.remote_peer);
		}

		private void CleanMyMetaData()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			IRailFriends val = rail_api.RailFactory().RailFriends();
			if (val != null)
			{
				val.AsyncClearAllMyMetadata("");
			}
		}

		private void OnDisconnect()
		{
			CleanMyMetaData();
			_hasLocalHost = false;
			Netplay.OnDisconnect -= OnDisconnect;
		}
	}
}
