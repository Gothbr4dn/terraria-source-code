using System.Collections.Generic;
using System.Runtime.Serialization;
using rail;

namespace Terraria.Social.WeGame
{
	[DataContract]
	public class WeGameFriendListInfo
	{
		[DataMember]
		public List<RailFriendInfo> _friendList;
	}
}
