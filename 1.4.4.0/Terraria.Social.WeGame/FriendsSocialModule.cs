using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame
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
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			string text = default(string);
			rail_api.RailFactory().RailPlayer().GetPlayerName(ref text);
			WeGameHelper.WriteDebugString("GetUsername by wegame" + text);
			return text;
		}

		public override void OpenJoinInterface()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			WeGameHelper.WriteDebugString("OpenJoinInterface by wegame");
			rail_api.RailFactory().RailFloatingWindow().AsyncShowRailFloatingWindow((EnumRailWindowType)10, "");
		}
	}
}
