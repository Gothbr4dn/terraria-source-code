namespace Terraria.GameContent.ObjectInteractions
{
	public interface ISmartInteractCandidate
	{
		float DistanceFromCursor { get; }

		void WinCandidacy();
	}
}
