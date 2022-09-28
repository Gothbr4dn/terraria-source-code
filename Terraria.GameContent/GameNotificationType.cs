using System;

namespace Terraria.GameContent
{
	[Flags]
	public enum GameNotificationType
	{
		None = 0,
		Damage = 1,
		SpawnOrDeath = 2,
		WorldGen = 4,
		All = 7
	}
}
