using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UISliderBase : UIElement
	{
		internal const int UsageLevel_NotSelected = 0;

		internal const int UsageLevel_SelectedAndLocked = 1;

		internal const int UsageLevel_OtherElementIsLocked = 2;

		internal static UIElement CurrentLockedSlider;

		internal static UIElement CurrentAimedSlider;

		internal int GetUsageLevel()
		{
			int result = 0;
			if (CurrentLockedSlider == this)
			{
				result = 1;
			}
			else if (CurrentLockedSlider != null)
			{
				result = 2;
			}
			return result;
		}

		public static void EscapeElements()
		{
			CurrentLockedSlider = null;
			CurrentAimedSlider = null;
		}
	}
}
