using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	public struct EntityShadowInfo
	{
		public Vector2 Position;

		public float Rotation;

		public Vector2 Origin;

		public int Direction;

		public int GravityDirection;

		public int BodyFrameIndex;

		public Vector2 HeadgearOffset => Main.OffsetsPlayerHeadgear[BodyFrameIndex];

		public void CopyPlayer(Player player)
		{
			Position = player.position;
			Rotation = player.fullRotation;
			Origin = player.fullRotationOrigin;
			Direction = player.direction;
			GravityDirection = (int)player.gravDir;
			BodyFrameIndex = player.bodyFrame.Y / player.bodyFrame.Height;
		}
	}
}
