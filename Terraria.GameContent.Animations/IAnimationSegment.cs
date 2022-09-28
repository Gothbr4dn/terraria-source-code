namespace Terraria.GameContent.Animations
{
	public interface IAnimationSegment
	{
		float DedicatedTimeNeeded { get; }

		void Draw(ref GameAnimationSegment info);
	}
}
