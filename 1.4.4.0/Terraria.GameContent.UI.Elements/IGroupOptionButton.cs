using Microsoft.Xna.Framework;

namespace Terraria.GameContent.UI.Elements
{
	public interface IGroupOptionButton
	{
		void SetColorsBasedOnSelectionState(Color pickedColor, Color unpickedColor, float opacityPicked, float opacityNotPicked);

		void SetBorderColor(Color color);
	}
}
