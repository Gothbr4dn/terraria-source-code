using System;
using System.Threading;
using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame
{
	public class AchievementsSocialModule : Terraria.Social.Base.AchievementsSocialModule
	{
		private const string FILE_NAME = "/achievements-wegame.dat";

		private bool _areStatsReceived;

		private bool _areAchievementReceived;

		private RailCallBackHelper _callbackHelper = new RailCallBackHelper();

		private IRailPlayerAchievement _playerAchievement;

		private IRailPlayerStats _playerStats;

		public override void Initialize()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			_callbackHelper.RegisterCallback((RAILEventID)2001, new RailEventCallBackHandler(RailEventCallBack));
			_callbackHelper.RegisterCallback((RAILEventID)2101, new RailEventCallBackHandler(RailEventCallBack));
			IRailPlayerStats myPlayerStats = GetMyPlayerStats();
			IRailPlayerAchievement myPlayerAchievement = GetMyPlayerAchievement();
			if (myPlayerStats != null && myPlayerAchievement != null)
			{
				myPlayerStats.AsyncRequestStats("");
				myPlayerAchievement.AsyncRequestAchievement("");
				while (!_areStatsReceived && !_areAchievementReceived)
				{
					CoreSocialModule.RailEventTick();
					Thread.Sleep(10);
				}
			}
		}

		public override void Shutdown()
		{
			StoreStats();
		}

		private IRailPlayerStats GetMyPlayerStats()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			if (_playerStats == null)
			{
				IRailStatisticHelper val = rail_api.RailFactory().RailStatisticHelper();
				if (val != null)
				{
					RailID val2 = new RailID();
					((RailComparableID)val2).id_ = 0uL;
					_playerStats = val.CreatePlayerStats(val2);
				}
			}
			return _playerStats;
		}

		private IRailPlayerAchievement GetMyPlayerAchievement()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			if (_playerAchievement == null)
			{
				IRailAchievementHelper val = rail_api.RailFactory().RailAchievementHelper();
				if (val != null)
				{
					RailID val2 = new RailID();
					((RailComparableID)val2).id_ = 0uL;
					_playerAchievement = val.CreatePlayerAchievement(val2);
				}
			}
			return _playerAchievement;
		}

		public void RailEventCallBack(RAILEventID eventId, EventBase data)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Invalid comparison between Unknown and I4
			if ((int)eventId != 2001)
			{
				if ((int)eventId == 2101)
				{
					_areAchievementReceived = true;
				}
			}
			else
			{
				_areStatsReceived = true;
			}
		}

		public override bool IsAchievementCompleted(string name)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Invalid comparison between Unknown and I4
			bool flag = false;
			RailResult val = (RailResult)1;
			IRailPlayerAchievement myPlayerAchievement = GetMyPlayerAchievement();
			if (myPlayerAchievement != null)
			{
				val = myPlayerAchievement.HasAchieved(name, ref flag);
			}
			if (flag)
			{
				return (int)val == 0;
			}
			return false;
		}

		public override byte[] GetEncryptionKey()
		{
			RailID railID = rail_api.RailFactory().RailPlayer().GetRailID();
			byte[] array = new byte[16];
			byte[] bytes = BitConverter.GetBytes(((RailComparableID)railID).id_);
			Array.Copy(bytes, array, 8);
			Array.Copy(bytes, 0, array, 8, 8);
			return array;
		}

		public override string GetSavePath()
		{
			return "/achievements-wegame.dat";
		}

		private int GetIntStat(string name)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			int result = 0;
			IRailPlayerStats myPlayerStats = GetMyPlayerStats();
			if (myPlayerStats != null)
			{
				myPlayerStats.GetStatValue(name, ref result);
			}
			return result;
		}

		private float GetFloatStat(string name)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			double num = 0.0;
			IRailPlayerStats myPlayerStats = GetMyPlayerStats();
			if (myPlayerStats != null)
			{
				myPlayerStats.GetStatValue(name, ref num);
			}
			return (float)num;
		}

		private bool SetFloatStat(string name, float value)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			IRailPlayerStats myPlayerStats = GetMyPlayerStats();
			RailResult val = (RailResult)1;
			if (myPlayerStats != null)
			{
				val = myPlayerStats.SetStatValue(name, (double)value);
			}
			return (int)val == 0;
		}

		public override void UpdateIntStat(string name, int value)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			IRailPlayerStats myPlayerStats = GetMyPlayerStats();
			if (myPlayerStats != null)
			{
				int num = 0;
				if ((int)myPlayerStats.GetStatValue(name, ref num) == 0 && num < value)
				{
					myPlayerStats.SetStatValue(name, value);
				}
			}
		}

		private bool SetIntStat(string name, int value)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Invalid comparison between Unknown and I4
			IRailPlayerStats myPlayerStats = GetMyPlayerStats();
			RailResult val = (RailResult)1;
			if (myPlayerStats != null)
			{
				val = myPlayerStats.SetStatValue(name, value);
			}
			return (int)val == 0;
		}

		public override void UpdateFloatStat(string name, float value)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			IRailPlayerStats myPlayerStats = GetMyPlayerStats();
			if (myPlayerStats != null)
			{
				double num = 0.0;
				if ((int)myPlayerStats.GetStatValue(name, ref num) == 0 && (float)num < value)
				{
					myPlayerStats.SetStatValue(name, (double)value);
				}
			}
		}

		public override void StoreStats()
		{
			SaveStats();
			SaveAchievement();
		}

		private void SaveStats()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			IRailPlayerStats myPlayerStats = GetMyPlayerStats();
			if (myPlayerStats != null)
			{
				myPlayerStats.AsyncStoreStats("");
			}
		}

		private void SaveAchievement()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			IRailPlayerAchievement myPlayerAchievement = GetMyPlayerAchievement();
			if (myPlayerAchievement != null)
			{
				myPlayerAchievement.AsyncStoreAchievement("");
			}
		}

		public override void CompleteAchievement(string name)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			IRailPlayerAchievement myPlayerAchievement = GetMyPlayerAchievement();
			if (myPlayerAchievement != null)
			{
				myPlayerAchievement.MakeAchievement(name);
			}
		}
	}
}
