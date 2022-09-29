using Microsoft.Xna.Framework;

namespace Terraria.Physics
{
	public interface IBallContactListener
	{
		void OnCollision(PhysicsProperties properties, ref Vector2 position, ref Vector2 velocity, ref BallCollisionEvent collision);

		void OnPassThrough(PhysicsProperties properties, ref Vector2 position, ref Vector2 velocity, ref float angularVelocity, ref BallPassThroughEvent passThrough);
	}
}
