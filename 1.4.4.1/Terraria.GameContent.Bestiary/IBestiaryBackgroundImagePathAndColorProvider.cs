using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Bestiary
{
	public interface IBestiaryBackgroundImagePathAndColorProvider
	{
		Asset<Texture2D> GetBackgroundImage();

		Color? GetBackgroundColor();
	}
}
