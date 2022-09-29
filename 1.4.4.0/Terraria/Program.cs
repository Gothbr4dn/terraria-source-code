using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ReLogic.IO;
using ReLogic.OS;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria
{
	public static class Program
	{
		public const bool IsServer = false;

		public const bool IsXna = false;

		public const bool IsFna = true;

		public const bool IsDebug = false;

		public static Dictionary<string, string> LaunchParameters = new Dictionary<string, string>();

		private static int ThingsToLoad;

		private static int ThingsLoaded;

		public static bool LoadedEverything;

		public static IntPtr JitForcedMethodCache;

		public static float LoadedPercentage
		{
			get
			{
				if (ThingsToLoad == 0)
				{
					return 1f;
				}
				return (float)ThingsLoaded / (float)ThingsToLoad;
			}
		}

		public static void StartForceLoad()
		{
			if (!Main.SkipAssemblyLoad)
			{
				Thread thread = new Thread(ForceLoadThread);
				thread.IsBackground = true;
				thread.Start();
			}
			else
			{
				LoadedEverything = true;
			}
		}

		public static void ForceLoadThread(object threadContext)
		{
			ForceLoadAssembly(Assembly.GetExecutingAssembly(), initializeStaticMembers: true);
			LoadedEverything = true;
		}

		private static void ForceJITOnAssembly(Assembly assembly)
		{
			Type[] types = assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				MethodInfo[] methods = types[i].GetMethods();
				foreach (MethodInfo methodInfo in methods)
				{
					if (!methodInfo.IsAbstract && !methodInfo.ContainsGenericParameters && methodInfo.GetMethodBody() != null)
					{
						JitForcedMethodCache = methodInfo.MethodHandle.GetFunctionPointer();
					}
				}
				ThingsLoaded++;
			}
		}

		private static void ForceStaticInitializers(Assembly assembly)
		{
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (!type.IsGenericType)
				{
					RuntimeHelpers.RunClassConstructor(type.TypeHandle);
				}
			}
		}

		private static void ForceLoadAssembly(Assembly assembly, bool initializeStaticMembers)
		{
			ThingsToLoad = assembly.GetTypes().Length;
			ForceJITOnAssembly(assembly);
			if (initializeStaticMembers)
			{
				ForceStaticInitializers(assembly);
			}
		}

		private static void ForceLoadAssembly(string name, bool initializeStaticMembers)
		{
			Assembly assembly = null;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				if (assemblies[i].GetName().Name.Equals(name))
				{
					assembly = assemblies[i];
					break;
				}
			}
			if (assembly == null)
			{
				assembly = Assembly.Load(name);
			}
			ForceLoadAssembly(assembly, initializeStaticMembers);
		}

		private static void SetupLogging()
		{
			if (LaunchParameters.ContainsKey("-logfile"))
			{
				string text = LaunchParameters["-logfile"];
				text = ((text != null && !(text.Trim() == "")) ? Path.Combine(text, $"Log_{DateTime.Now:yyyyMMddHHmmssfff}.log") : Path.Combine(Main.SavePath, "Logs", $"Log_{DateTime.Now:yyyyMMddHHmmssfff}.log"));
				ConsoleOutputMirror.ToFile(text);
			}
			CrashWatcher.Inititialize();
			CrashWatcher.DumpOnException = LaunchParameters.ContainsKey("-minidump");
			CrashWatcher.LogAllExceptions = LaunchParameters.ContainsKey("-logerrors");
			if (LaunchParameters.ContainsKey("-fulldump"))
			{
				Console.WriteLine("Full Dump logs enabled.");
				CrashWatcher.EnableCrashDumps(CrashDump.Options.WithFullMemory);
			}
		}

		private static void InitializeConsoleOutput()
		{
			if (Debugger.IsAttached)
			{
				return;
			}
			try
			{
				Console.OutputEncoding = Encoding.UTF8;
				if (Platform.get_IsWindows())
				{
					Console.InputEncoding = Encoding.Unicode;
				}
				else
				{
					Console.InputEncoding = Encoding.UTF8;
				}
			}
			catch
			{
			}
		}

		public static void LaunchGame(string[] args, bool monoArgs = false)
		{
			Thread.CurrentThread.Name = "Main Thread";
			if (monoArgs)
			{
				args = Utils.ConvertMonoArgsToDotNet(args);
			}
			TrySettingFNAToOpenGL(args);
			if (Platform.get_IsOSX())
			{
				Main.OnEngineLoad += delegate
				{
					Main.instance.IsMouseVisible = false;
				};
			}
			LaunchParameters = Utils.ParseArguements(args);
			ThreadPool.SetMinThreads(8, 8);
			LanguageManager.Instance.SetLanguage(GameCulture.DefaultCulture);
			InitializeConsoleOutput();
			SetupLogging();
			Platform.Get<IWindowService>().SetQuickEditEnabled(false);
			using Main main = new Main();
			try
			{
				Lang.InitializeLegacyLocalization();
				SocialAPI.Initialize();
				LaunchInitializer.LoadParameters(main);
				Main.OnEnginePreload += StartForceLoad;
				main.Run();
			}
			catch (Exception e)
			{
				DisplayException(e);
			}
		}

		private static void TrySettingFNAToOpenGL(string[] args)
		{
			bool flag = false;
			for (int i = 0; i < args.Length; i++)
			{
				if (args[i].Contains("gldevice"))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Environment.SetEnvironmentVariable("FNA3D_FORCE_DRIVER", "OpenGL");
			}
		}

		private static void DisplayException(Exception e)
		{
			try
			{
				string text = e.ToString();
				if (WorldGen.gen)
				{
					try
					{
						text = $"Creating world - Seed: {Main.ActiveWorldFileData.Seed} Width: {Main.maxTilesX}, Height: {Main.maxTilesY}, Evil: {WorldGen.WorldGenParam_Evil}, IsExpert: {Main.expertMode}\n{text}";
					}
					catch
					{
					}
				}
				using (StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true))
				{
					streamWriter.WriteLine(DateTime.Now);
					streamWriter.WriteLine(text);
					streamWriter.WriteLine("");
				}
				MessageBox.Show(text, "Terraria: Error");
			}
			catch
			{
			}
		}
	}
}
