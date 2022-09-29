using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using rail;

namespace Terraria.Social.WeGame
{
	public class CoreSocialModule : ISocialModule
	{
		private RailCallBackHelper _callbackHelper = new RailCallBackHelper();

		private static object _railTickLock = new object();

		private bool isRailValid;

		public static event Action OnTick;

		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern uint GetCurrentThreadId();

		public void Initialize()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			RailGameID val = new RailGameID();
			((RailComparableID)val).id_ = 2000328uL;
			string[] array = new string[1] { " " };
			if (rail_api.RailNeedRestartAppForCheckingEnvironment(val, array.Length, array))
			{
				Environment.Exit(1);
			}
			if (!rail_api.RailInitialize())
			{
				Environment.Exit(1);
			}
			_callbackHelper.RegisterCallback((RAILEventID)2, new RailEventCallBackHandler(RailEventCallBack));
			isRailValid = true;
			ThreadPool.QueueUserWorkItem(TickThread, null);
			Main.OnTickForThirdPartySoftwareOnly += RailEventTick;
		}

		public static void RailEventTick()
		{
			rail_api.RailFireEvents();
			if (Monitor.TryEnter(_railTickLock))
			{
				Monitor.Pulse(_railTickLock);
				Monitor.Exit(_railTickLock);
			}
		}

		private void TickThread(object context)
		{
			Monitor.Enter(_railTickLock);
			while (isRailValid)
			{
				if (CoreSocialModule.OnTick != null)
				{
					CoreSocialModule.OnTick();
				}
				Monitor.Wait(_railTickLock);
			}
			Monitor.Exit(_railTickLock);
		}

		public void Shutdown()
		{
			isRailValid = false;
			AppDomain.CurrentDomain.ProcessExit += delegate
			{
				isRailValid = false;
			};
			_callbackHelper.UnregisterAllCallback();
			rail_api.RailFinalize();
		}

		public static void RailEventCallBack(RAILEventID eventId, EventBase data)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if ((int)eventId == 2)
			{
				ProcessRailSystemStateChange(((RailSystemStateChanged)data).state);
			}
		}

		public static void SaveAndQuitCallBack()
		{
			Main.WeGameRequireExitGame();
		}

		private static void ProcessRailSystemStateChange(RailSystemState state)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Invalid comparison between Unknown and I4
			if ((int)state == 2 || (int)state == 3)
			{
				MessageBox.Show("检测到WeGame异常，游戏将自动保存进度并退出游戏", "Terraria--WeGame Error");
				WorldGen.SaveAndQuit(SaveAndQuitCallBack);
			}
		}
	}
}
