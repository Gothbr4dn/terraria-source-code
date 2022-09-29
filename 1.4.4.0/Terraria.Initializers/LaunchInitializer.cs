using System;
using System.Diagnostics;
using Terraria.Localization;
using Terraria.Social;

namespace Terraria.Initializers
{
	public static class LaunchInitializer
	{
		public static void LoadParameters(Main game)
		{
			LoadSharedParameters(game);
			LoadClientParameters(game);
		}

		private static void LoadSharedParameters(Main game)
		{
			string path;
			if ((path = TryParameter("-loadlib")) != null)
			{
				game.loadLib(path);
			}
			string s;
			if ((s = TryParameter("-p", "-port")) != null && int.TryParse(s, out var result))
			{
				Netplay.ListenPort = result;
			}
		}

		private static void LoadClientParameters(Main game)
		{
			string iP;
			if ((iP = TryParameter("-j", "-join")) != null)
			{
				game.AutoJoin(iP);
			}
			string arg;
			if ((arg = TryParameter("-pass", "-password")) != null)
			{
				Netplay.ServerPassword = Main.ConvertFromSafeArgument(arg);
				game.AutoPass();
			}
			if (HasParameter("-host"))
			{
				game.AutoHost();
			}
		}

		private static void LoadServerParameters(Main game)
		{
			try
			{
				string s;
				if ((s = TryParameter("-forcepriority")) != null)
				{
					Process currentProcess = Process.GetCurrentProcess();
					if (int.TryParse(s, out var result))
					{
						switch (result)
						{
						case 0:
							currentProcess.PriorityClass = ProcessPriorityClass.RealTime;
							break;
						case 1:
							currentProcess.PriorityClass = ProcessPriorityClass.High;
							break;
						case 2:
							currentProcess.PriorityClass = ProcessPriorityClass.AboveNormal;
							break;
						case 3:
							currentProcess.PriorityClass = ProcessPriorityClass.Normal;
							break;
						case 4:
							currentProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
							break;
						case 5:
							currentProcess.PriorityClass = ProcessPriorityClass.Idle;
							break;
						default:
							currentProcess.PriorityClass = ProcessPriorityClass.High;
							break;
						}
					}
					else
					{
						currentProcess.PriorityClass = ProcessPriorityClass.High;
					}
				}
				else
				{
					Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
				}
			}
			catch
			{
			}
			string value;
			if ((value = TryParameter("-maxplayers", "-players")) != null)
			{
				int num = Convert.ToInt32(value);
				if (num <= 255 && num >= 1)
				{
					game.SetNetPlayers(num);
				}
			}
			string arg;
			if ((arg = TryParameter("-pass", "-password")) != null)
			{
				Netplay.ServerPassword = Main.ConvertFromSafeArgument(arg);
			}
			string s2;
			if ((s2 = TryParameter("-lang")) != null && int.TryParse(s2, out var result2))
			{
				LanguageManager.Instance.SetLanguage(result2);
			}
			if ((s2 = TryParameter("-language")) != null)
			{
				LanguageManager.Instance.SetLanguage(s2);
			}
			string worldName;
			if ((worldName = TryParameter("-worldname")) != null)
			{
				game.SetWorldName(worldName);
			}
			string newMOTD;
			if ((newMOTD = TryParameter("-motd")) != null)
			{
				game.NewMOTD(newMOTD);
			}
			string banFilePath;
			if ((banFilePath = TryParameter("-banlist")) != null)
			{
				Netplay.BanFilePath = banFilePath;
			}
			if (HasParameter("-autoshutdown"))
			{
				game.EnableAutoShutdown();
			}
			if (HasParameter("-secure"))
			{
				Netplay.SpamCheck = true;
			}
			string serverWorldRollbacks;
			if ((serverWorldRollbacks = TryParameter("-worldrollbackstokeep")) != null)
			{
				game.setServerWorldRollbacks(serverWorldRollbacks);
			}
			string worldSize;
			if ((worldSize = TryParameter("-autocreate")) != null)
			{
				game.autoCreate(worldSize);
			}
			if (HasParameter("-noupnp"))
			{
				Netplay.UseUPNP = false;
			}
			if (HasParameter("-experimental"))
			{
				Main.UseExperimentalFeatures = true;
			}
			string world;
			if ((world = TryParameter("-world")) != null)
			{
				game.SetWorld(world, cloud: false);
			}
			else if (SocialAPI.Mode == SocialMode.Steam && (world = TryParameter("-cloudworld")) != null)
			{
				game.SetWorld(world, cloud: true);
			}
			string configPath;
			if ((configPath = TryParameter("-config")) != null)
			{
				game.LoadDedConfig(configPath);
			}
			string autogenSeedName;
			if ((autogenSeedName = TryParameter("-seed")) != null)
			{
				Main.AutogenSeedName = autogenSeedName;
			}
		}

		private static bool HasParameter(params string[] keys)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				if (Program.LaunchParameters.ContainsKey(keys[i]))
				{
					return true;
				}
			}
			return false;
		}

		private static string TryParameter(params string[] keys)
		{
			for (int i = 0; i < keys.Length; i++)
			{
				if (Program.LaunchParameters.TryGetValue(keys[i], out var value))
				{
					if (value == null)
					{
						return "";
					}
					return value;
				}
			}
			return null;
		}
	}
}
