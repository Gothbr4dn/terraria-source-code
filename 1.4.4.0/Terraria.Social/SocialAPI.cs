using System;
using System.Collections.Generic;
using Terraria.Social.Base;
using Terraria.Social.Steam;
using Terraria.Social.WeGame;

namespace Terraria.Social
{
	public static class SocialAPI
	{
		private static SocialMode _mode;

		public static Terraria.Social.Base.FriendsSocialModule Friends;

		public static Terraria.Social.Base.AchievementsSocialModule Achievements;

		public static Terraria.Social.Base.CloudSocialModule Cloud;

		public static Terraria.Social.Base.NetSocialModule Network;

		public static Terraria.Social.Base.OverlaySocialModule Overlay;

		public static Terraria.Social.Base.WorkshopSocialModule Workshop;

		public static ServerJoinRequestsManager JoinRequests;

		public static Terraria.Social.Base.PlatformSocialModule Platform;

		private static List<ISocialModule> _modules;

		public static SocialMode Mode => _mode;

		public static void Initialize(SocialMode? mode = null)
		{
			if (!mode.HasValue)
			{
				mode = SocialMode.None;
				mode = SocialMode.Steam;
			}
			_mode = mode.Value;
			_modules = new List<ISocialModule>();
			JoinRequests = new ServerJoinRequestsManager();
			Main.OnTickForInternalCodeOnly += JoinRequests.Update;
			switch (Mode)
			{
			case SocialMode.Steam:
				LoadSteam();
				break;
			case SocialMode.WeGame:
				LoadWeGame();
				break;
			}
			foreach (ISocialModule module in _modules)
			{
				module.Initialize();
			}
		}

		public static void Shutdown()
		{
			_modules.Reverse();
			foreach (ISocialModule module in _modules)
			{
				module.Shutdown();
			}
		}

		private static T LoadModule<T>() where T : ISocialModule, new()
		{
			T val = new T();
			_modules.Add(val);
			return val;
		}

		private static T LoadModule<T>(T module) where T : ISocialModule
		{
			_modules.Add(module);
			return module;
		}

		private static void LoadDiscord()
		{
			if (Environment.Is64BitOperatingSystem)
			{
				_ = Environment.Is64BitProcess;
			}
		}

		private static void LoadSteam()
		{
			LoadModule<Terraria.Social.Steam.CoreSocialModule>();
			Friends = LoadModule<Terraria.Social.Steam.FriendsSocialModule>();
			Achievements = LoadModule<Terraria.Social.Steam.AchievementsSocialModule>();
			Cloud = LoadModule<Terraria.Social.Steam.CloudSocialModule>();
			Overlay = LoadModule<Terraria.Social.Steam.OverlaySocialModule>();
			Workshop = LoadModule<Terraria.Social.Steam.WorkshopSocialModule>();
			Platform = LoadModule<Terraria.Social.Steam.PlatformSocialModule>();
			Network = LoadModule<Terraria.Social.Steam.NetClientSocialModule>();
			WeGameHelper.WriteDebugString("LoadSteam modules");
		}

		private static void LoadWeGame()
		{
			LoadModule<Terraria.Social.WeGame.CoreSocialModule>();
			Cloud = LoadModule<Terraria.Social.WeGame.CloudSocialModule>();
			Friends = LoadModule<Terraria.Social.WeGame.FriendsSocialModule>();
			Overlay = LoadModule<Terraria.Social.WeGame.OverlaySocialModule>();
			Network = LoadModule<Terraria.Social.WeGame.NetClientSocialModule>();
			WeGameHelper.WriteDebugString("LoadWeGame modules");
		}
	}
}
