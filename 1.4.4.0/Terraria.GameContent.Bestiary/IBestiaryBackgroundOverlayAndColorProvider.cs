using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Bestiary
{
	public interface IBestiaryBackgroundOverlayAndColorProvider
	{
		float DisplayPriority { get; }

		Asset<Texture2D> GetBackgroundOverlayImage();

		Color? GetBackgroundOverlayColor();
	}
}
