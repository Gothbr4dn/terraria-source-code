namespace Terraria.GameContent.ObjectInteractions
{
	public interface ISmartInteractBlockReasonProvider
	{
		bool ShouldBlockSmartInteract(SmartInteractScanSettings settings);
	}
}
