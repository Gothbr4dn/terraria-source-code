namespace Terraria.Social.Base
{
	public abstract class PlatformSocialModule : ISocialModule
	{
		public abstract void Initialize();

		public abstract void Shutdown();
	}
}
