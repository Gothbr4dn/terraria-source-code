using Terraria.Audio;

namespace Terraria.DataStructures
{
	public class RejectionMenuInfo
	{
		public ReturnFromRejectionMenuAction ExitAction;

		public string TextToShow;

		public void DefaultExitAction()
		{
			SoundEngine.PlaySound(11);
			Main.menuMode = 0;
			Main.netMode = 0;
		}
	}
}
