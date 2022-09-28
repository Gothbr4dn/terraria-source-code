using Microsoft.Xna.Framework;
using Terraria.Enums;

namespace Terraria.DataStructures
{
	public struct NPCAimedTarget
	{
		public NPCTargetType Type;

		public Rectangle Hitbox;

		public int Width;

		public int Height;

		public Vector2 Position;

		public Vector2 Velocity;

		public bool Invalid => Type == NPCTargetType.None;

		public Vector2 Center => Position + Size / 2f;

		public Vector2 Size => new Vector2(Width, Height);

		public NPCAimedTarget(NPC npc)
		{
			Type = NPCTargetType.NPC;
			Hitbox = npc.Hitbox;
			Width = npc.width;
			Height = npc.height;
			Position = npc.position;
			Velocity = npc.velocity;
		}

		public NPCAimedTarget(Player player, bool ignoreTank = true)
		{
			Type = NPCTargetType.Player;
			Hitbox = player.Hitbox;
			Width = player.width;
			Height = player.height;
			Position = player.position;
			Velocity = player.velocity;
			if (!ignoreTank && player.tankPet > -1)
			{
				Projectile projectile = Main.projectile[player.tankPet];
				Type = NPCTargetType.PlayerTankPet;
				Hitbox = projectile.Hitbox;
				Width = projectile.width;
				Height = projectile.height;
				Position = projectile.position;
				Velocity = projectile.velocity;
			}
		}
	}
}
