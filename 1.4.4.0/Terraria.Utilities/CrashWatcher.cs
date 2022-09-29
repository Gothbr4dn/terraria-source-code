using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;

namespace Terraria.Utilities
{
	public static class CrashWatcher
	{
		public static bool LogAllExceptions { get; set; }

		public static bool DumpOnException { get; set; }

		public static bool DumpOnCrash { get; private set; }

		public static CrashDump.Options CrashDumpOptions { get; private set; }

		private static string DumpPath => Path.Combine(Main.SavePath, "Dumps");

		public static void Inititialize()
		{
			Console.WriteLine("Error Logging Enabled.");
			AppDomain.CurrentDomain.FirstChanceException += delegate(object sender, FirstChanceExceptionEventArgs exceptionArgs)
			{
				if (LogAllExceptions && 0 == 0)
				{
					string text2 = exceptionArgs.Exception.ToString();
					Console.Write(string.Concat("================\r\n" + $"{DateTime.Now}: First-Chance Exception\r\nThread: {Thread.CurrentThread.ManagedThreadId} [{Thread.CurrentThread.Name}]\r\nCulture: {Thread.CurrentThread.CurrentCulture.Name}\r\nException: {text2}\r\n", "================\r\n\r\n"));
				}
			};
			AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs exceptionArgs)
			{
				string text = exceptionArgs.ExceptionObject.ToString();
				Console.Write(string.Concat("================\r\n" + $"{DateTime.Now}: Unhandled Exception\r\nThread: {Thread.CurrentThread.ManagedThreadId} [{Thread.CurrentThread.Name}]\r\nCulture: {Thread.CurrentThread.CurrentCulture.Name}\r\nException: {text}\r\n", "================\r\n"));
				if (DumpOnCrash)
				{
					CrashDump.WriteException(CrashDumpOptions, DumpPath);
				}
			};
		}

		public static void EnableCrashDumps(CrashDump.Options options)
		{
			DumpOnCrash = true;
			CrashDumpOptions = options;
		}

		public static void DisableCrashDumps()
		{
			DumpOnCrash = false;
		}

		[Conditional("DEBUG")]
		private static void HookDebugExceptionDialog()
		{
		}
	}
}
