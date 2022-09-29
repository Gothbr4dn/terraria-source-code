using Steamworks;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	public class FriendsSocialModule : Terraria.Social.Base.FriendsSocialModule
	{
		public override void Initialize()
		{
		}

		public override void Shutdown()
		{
		}

		public override string GetUsername()
		{
			return SteamFriends.GetPersonaName();
		}

		public override void OpenJoinInterface()
		{
			SteamFriends.ActivateGameOverlay("Friends");
		}
	}
}
