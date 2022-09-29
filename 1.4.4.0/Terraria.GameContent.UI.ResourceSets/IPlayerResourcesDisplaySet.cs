using Terraria.DataStructures;

namespace Terraria.GameContent.UI.ResourceSets
{
	public interface IPlayerResourcesDisplaySet : IConfigKeyHolder
	{
		void Draw();

		void TryToHover();
	}
}
