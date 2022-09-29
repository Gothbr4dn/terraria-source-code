namespace Terraria.Graphics
{
	public class WindowStateController
	{
		public bool CanMoveWindowAcrossScreens => false;

		public string ScreenDeviceName => "";

		public void TryMovingToScreen(string screenDeviceName)
		{
			_ = CanMoveWindowAcrossScreens;
		}
	}
}
