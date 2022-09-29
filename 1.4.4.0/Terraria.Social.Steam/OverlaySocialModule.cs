using Steamworks;
using Terraria.Social.Base;

namespace Terraria.Social.Steam
{
	public class OverlaySocialModule : Terraria.Social.Base.OverlaySocialModule
	{
		private Callback<GamepadTextInputDismissed_t> _gamepadTextInputDismissed;

		private bool _gamepadTextInputActive;

		public override void Initialize()
		{
			_gamepadTextInputDismissed = Callback<GamepadTextInputDismissed_t>.Create((DispatchDelegate<GamepadTextInputDismissed_t>)OnGamepadTextInputDismissed);
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
			if (_gamepadTextInputActive)
			{
				return false;
			}
			bool num = SteamUtils.ShowGamepadTextInput((EGamepadTextInputMode)(password ? 1 : 0), (EGamepadTextInputLineMode)(multiLine ? 1 : 0), description, maxLength, existingText);
			if (num)
			{
				_gamepadTextInputActive = true;
			}
			return num;
		}

		public override string GetGamepadText()
		{
			uint enteredGamepadTextLength = SteamUtils.GetEnteredGamepadTextLength();
			string result = default(string);
			SteamUtils.GetEnteredGamepadTextInput(ref result, enteredGamepadTextLength);
			return result;
		}

		private void OnGamepadTextInputDismissed(GamepadTextInputDismissed_t result)
		{
			_gamepadTextInputActive = false;
		}
	}
}
