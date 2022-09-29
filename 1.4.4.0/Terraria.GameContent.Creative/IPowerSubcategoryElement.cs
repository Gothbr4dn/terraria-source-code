using Terraria.GameContent.UI.Elements;

namespace Terraria.GameContent.Creative
{
	public interface IPowerSubcategoryElement
	{
		GroupOptionButton<int> GetOptionButton(CreativePowerUIElementRequestInfo info, int optionIndex, int currentOptionIndex);
	}
}
