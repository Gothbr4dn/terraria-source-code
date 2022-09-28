using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class MoonLordPortraitBackgroundProviderBestiaryInfoElement : IBestiaryInfoElement, IBestiaryBackgroundImagePathAndColorProvider
	{
		public Asset<Texture2D> GetBackgroundImage()
		{
			return Main.Assets.Request<Texture2D>("Images/MapBG1", (AssetRequestMode)1);
		}

		public Color? GetBackgroundColor()
		{
			return Color.Black;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}
	}
}
