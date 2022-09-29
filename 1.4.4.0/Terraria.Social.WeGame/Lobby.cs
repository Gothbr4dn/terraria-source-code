using System;
using rail;

namespace Terraria.Social.WeGame
{
	public class Lobby
	{
		private RailCallBackHelper _callbackHelper = new RailCallBackHelper();

		public LobbyState State;

		private bool _gameServerInitSuccess;

		private IRailGameServer _gameServer;

		public Action<RailID> _lobbyCreatedExternalCallback;

		private RailID _server_id;

		private IRailGameServer RailServerHelper
		{
			get
			{
				if (_gameServerInitSuccess)
				{
					return _gameServer;
				}
				return null;
			}
			set
			{
				_gameServer = value;
			}
		}

		private IRailGameServerHelper GetRailServerHelper()
		{
			return rail_api.RailFactory().RailGameServerHelper();
		}

		private void RegisterGameServerEvent()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			if (_callbackHelper != null)
			{
				_callbackHelper.RegisterCallback((RAILEventID)3002, new RailEventCallBackHandler(OnRailEvent));
			}
		}

		public void OnRailEvent(RAILEventID id, EventBase data)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Invalid comparison between Unknown and I4
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			WeGameHelper.WriteDebugString("OnRailEvent,id=" + ((object)(RAILEventID)(ref id)).ToString() + " ,result=" + ((object)(RailResult)(ref data.result)).ToString());
			if ((int)id == 3002)
			{
				OnGameServerCreated((CreateGameServerResult)data);
			}
		}

		private void OnGameServerCreated(CreateGameServerResult result)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			if ((int)((EventBase)result).result == 0)
			{
				_gameServerInitSuccess = true;
				_lobbyCreatedExternalCallback(result.game_server_id);
				_server_id = result.game_server_id;
			}
		}

		public void Create(bool inviteOnly)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			if (State == LobbyState.Inactive)
			{
				RegisterGameServerEvent();
			}
			IRailGameServerHelper obj = rail_api.RailFactory().RailGameServerHelper();
			CreateGameServerOptions val = new CreateGameServerOptions();
			val.has_password = false;
			val.enable_team_voice = false;
			IRailGameServer val3 = (RailServerHelper = obj.AsyncCreateGameServer(val, "terraria", "terraria"));
			State = LobbyState.Creating;
		}

		public void OpenInviteOverlay()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			WeGameHelper.WriteDebugString("OpenInviteOverlay by wegame");
			rail_api.RailFactory().RailFloatingWindow().AsyncShowRailFloatingWindow((EnumRailWindowType)10, "");
		}

		public void Join(RailID local_peer, RailID remote_peer)
		{
			if (State != 0)
			{
				WeGameHelper.WriteDebugString("Lobby connection attempted while already in a lobby. This should never happen?");
			}
			else
			{
				State = LobbyState.Connecting;
			}
		}

		public byte[] GetMessage(int index)
		{
			return null;
		}

		public int GetUserCount()
		{
			return 0;
		}

		public RailID GetUserByIndex(int index)
		{
			return null;
		}

		public bool SendMessage(byte[] data)
		{
			return SendMessage(data, data.Length);
		}

		public bool SendMessage(byte[] data, int length)
		{
			return false;
		}

		public void Set(RailID lobbyId)
		{
		}

		public void SetPlayedWith(RailID userId)
		{
		}

		public void Leave()
		{
			State = LobbyState.Inactive;
		}

		public IRailGameServer GetServer()
		{
			return RailServerHelper;
		}
	}
}
