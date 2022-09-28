namespace Terraria.Social.Base
{
	public abstract class OverlaySocialModule : ISocialModule
	{
		public abstract void Initialize();

		public abstract void Shutdown();

		public abstract bool IsGamepadTextInputActive();

		public abstract bool ShowGamepadTextInput(string description, uint maxLength, bool multiLine = false, string existingText = "", bool password = false);

		public abstract string GetGamepadText();
	}
}
