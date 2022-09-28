using System;
using System.Collections.Generic;

namespace Terraria.Net
{
	public class NetGroupInfo
	{
		public enum InfoProviderId
		{
			IPAddress,
			Steam
		}

		private interface INetGroupInfoProvider
		{
			InfoProviderId Id { get; }

			bool HasValidInfo { get; }

			string ProvideInfoNeededToJoin();
		}

		private class IPAddressInfoProvider : INetGroupInfoProvider
		{
			public InfoProviderId Id => InfoProviderId.IPAddress;

			public bool HasValidInfo => false;

			public string ProvideInfoNeededToJoin()
			{
				return "";
			}
		}

		private class SteamLobbyInfoProvider : INetGroupInfoProvider
		{
			public InfoProviderId Id => InfoProviderId.Steam;

			public bool HasValidInfo => Main.LobbyId != 0;

			public string ProvideInfoNeededToJoin()
			{
				return Main.LobbyId.ToString();
			}
		}

		private readonly string[] _separatorBetweenInfos = new string[1] { ", " };

		private readonly string[] _separatorBetweenIdAndInfo = new string[1] { ":" };

		private List<INetGroupInfoProvider> _infoProviders;

		public NetGroupInfo()
		{
			_infoProviders = new List<INetGroupInfoProvider>();
			_infoProviders.Add(new IPAddressInfoProvider());
			_infoProviders.Add(new SteamLobbyInfoProvider());
		}

		public string ComposeInfo()
		{
			List<string> list = new List<string>();
			foreach (INetGroupInfoProvider infoProvider in _infoProviders)
			{
				if (infoProvider.HasValidInfo)
				{
					string text = (int)infoProvider.Id + _separatorBetweenIdAndInfo[0] + infoProvider.ProvideInfoNeededToJoin();
					string item = ConvertToSafeInfo(text);
					list.Add(item);
				}
			}
			return string.Join(_separatorBetweenInfos[0], list.ToArray());
		}

		public Dictionary<InfoProviderId, string> DecomposeInfo(string info)
		{
			Dictionary<InfoProviderId, string> dictionary = new Dictionary<InfoProviderId, string>();
			string[] array = info.Split(_separatorBetweenInfos, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in array)
			{
				string[] array2 = ConvertFromSafeInfo(text).Split(_separatorBetweenIdAndInfo, StringSplitOptions.RemoveEmptyEntries);
				if (array2.Length == 2 && int.TryParse(array2[0], out var result))
				{
					dictionary[(InfoProviderId)result] = array2[1];
				}
			}
			return dictionary;
		}

		private string ConvertToSafeInfo(string text)
		{
			return Uri.EscapeDataString(text);
		}

		private string ConvertFromSafeInfo(string text)
		{
			return Uri.UnescapeDataString(text);
		}
	}
}
