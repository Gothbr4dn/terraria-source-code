namespace Terraria.Social.Base
{
	public abstract class FriendsSocialModule : ISocialModule
	{
		public abstract string GetUsername();

		public abstract void OpenJoinInterface();

		public abstract void Initialize();

		public abstract void Shutdown();
	}
}
