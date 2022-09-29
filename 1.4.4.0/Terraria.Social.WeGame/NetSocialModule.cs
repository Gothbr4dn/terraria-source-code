using System.Collections.Concurrent;
using rail;
using Terraria.Net;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame
{
	public abstract class NetSocialModule : Terraria.Social.Base.NetSocialModule
	{
		public enum ConnectionState
		{
			Inactive,
			Authenticating,
			Connected
		}

		protected const int LobbyMessageJoin = 1;

		protected Lobby _lobby = new Lobby();

		protected WeGameP2PReader _reader;

		protected WeGameP2PWriter _writer;

		protected ConcurrentDictionary<RailID, ConnectionState> _connectionStateMap = new ConcurrentDictionary<RailID, ConnectionState>();

		protected static readonly byte[] _handshake = new byte[10] { 10, 0, 93, 114, 101, 108, 111, 103, 105, 99 };

		protected NetSocialModule()
		{
			_reader = new WeGameP2PReader();
			_writer = new WeGameP2PWriter();
		}

		public override void Initialize()
		{
			CoreSocialModule.OnTick += _reader.ReadTick;
			CoreSocialModule.OnTick += _writer.SendAll;
		}

		public override void Shutdown()
		{
			_lobby.Leave();
		}

		public override bool IsConnected(RemoteAddress address)
		{
			if (address == null)
			{
				return false;
			}
			RailID key = RemoteAddressToRailId(address);
			if (!_connectionStateMap.ContainsKey(key) || _connectionStateMap[key] != ConnectionState.Connected)
			{
				return false;
			}
			return true;
		}

		protected RailID GetLocalPeer()
		{
			return rail_api.RailFactory().RailPlayer().GetRailID();
		}

		protected bool GetSessionState(RailID userId, RailNetworkSessionState state)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			IRailNetwork val = rail_api.RailFactory().RailNetworkHelper();
			if ((int)val.GetSessionState(userId, state) != 0)
			{
				WeGameHelper.WriteDebugString("GetSessionState Failed user:{0}", ((RailComparableID)userId).id_);
				return false;
			}
			return true;
		}

		protected RailID RemoteAddressToRailId(RemoteAddress address)
		{
			return ((WeGameAddress)address).rail_id;
		}

		public override bool Send(RemoteAddress address, byte[] data, int length)
		{
			RailID user = RemoteAddressToRailId(address);
			_writer.QueueSend(user, data, length);
			return true;
		}

		public override int Receive(RemoteAddress address, byte[] data, int offset, int length)
		{
			if (address == null)
			{
				return 0;
			}
			RailID user = RemoteAddressToRailId(address);
			return _reader.Receive(user, data, offset, length);
		}

		public override bool IsDataAvailable(RemoteAddress address)
		{
			RailID id = RemoteAddressToRailId(address);
			return _reader.IsDataAvailable(id);
		}
	}
}
