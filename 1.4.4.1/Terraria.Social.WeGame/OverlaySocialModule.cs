using Terraria.Social.Base;

namespace Terraria.Social.WeGame
{
	public class OverlaySocialModule : Terraria.Social.Base.OverlaySocialModule
	{
		private bool _gamepadTextInputActive;

		public override void Initialize()
		{
		}

		public override void Shutdown()
		{
		}

		public override bool IsGamepadTextInputActive()
		{
			return _gamepadTextInputActive;
		}

		public override bool ShowGamepadTextInput(string description, uint maxLength, bool multiLine = false, string existingText = "", bool password = false)
		{
			return false;
		}

		public override string GetGamepadText()
		{
			return "";
		}
	}
}
