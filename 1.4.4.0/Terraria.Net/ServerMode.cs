using System;

namespace Terraria.Net
{
	[Flags]
	public enum ServerMode : byte
	{
		None = 0,
		Lobby = 1,
		FriendsCanJoin = 2,
		FriendsOfFriends = 4
	}
}
