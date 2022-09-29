namespace Terraria.GameContent.Animations
{
	public interface IAnimationSegmentAction<T>
	{
		int ExpectedLengthOfActionInFrames { get; }

		void BindTo(T obj);

		void ApplyTo(T obj, float localTimeForObj);

		void SetDelay(float delay);
	}
}
