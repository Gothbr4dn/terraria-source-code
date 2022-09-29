namespace Terraria.Physics
{
	public class PhysicsProperties
	{
		public readonly float Gravity;

		public readonly float Drag;

		public PhysicsProperties(float gravity, float drag)
		{
			Gravity = gravity;
			Drag = drag;
		}
	}
}
