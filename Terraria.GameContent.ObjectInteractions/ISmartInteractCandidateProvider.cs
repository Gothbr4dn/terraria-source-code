namespace Terraria.GameContent.ObjectInteractions
{
	public interface ISmartInteractCandidateProvider
	{
		void ClearSelfAndPrepareForCheck();

		bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate);
	}
}
