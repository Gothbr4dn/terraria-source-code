using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent
{
	public interface ITownNPCProfile
	{
		int RollVariation();

		string GetNameForVariant(NPC npc);

		Asset<Texture2D> GetTextureNPCShouldUse(NPC npc);

		int GetHeadTextureIndex(NPC npc);
	}
}
