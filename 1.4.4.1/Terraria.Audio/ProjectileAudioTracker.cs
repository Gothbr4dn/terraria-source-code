namespace Terraria.Audio
{
	public class ProjectileAudioTracker
	{
		private int _expectedType;

		private int _expectedIndex;

		public ProjectileAudioTracker(Projectile proj)
		{
			_expectedIndex = proj.whoAmI;
			_expectedType = proj.type;
		}

		public bool IsActiveAndInGame()
		{
			if (Main.gameMenu)
			{
				return false;
			}
			Projectile projectile = Main.projectile[_expectedIndex];
			if (projectile.active)
			{
				return projectile.type == _expectedType;
			}
			return false;
		}
	}
}
