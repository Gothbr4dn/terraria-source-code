using Terraria.GameContent.NetModules;
using Terraria.Net;

namespace Terraria.Initializers
{
	public static class NetworkInitializer
	{
		public static void Load()
		{
			NetManager.Instance.Register<NetLiquidModule>();
			NetManager.Instance.Register<NetTextModule>();
			NetManager.Instance.Register<NetPingModule>();
			NetManager.Instance.Register<NetAmbienceModule>();
			NetManager.Instance.Register<NetBestiaryModule>();
			NetManager.Instance.Register<NetCreativeUnlocksModule>();
			NetManager.Instance.Register<NetCreativePowersModule>();
			NetManager.Instance.Register<NetCreativeUnlocksPlayerReportModule>();
			NetManager.Instance.Register<NetTeleportPylonModule>();
			NetManager.Instance.Register<NetParticlesModule>();
			NetManager.Instance.Register<NetCreativePowerPermissionsModule>();
		}
	}
}
