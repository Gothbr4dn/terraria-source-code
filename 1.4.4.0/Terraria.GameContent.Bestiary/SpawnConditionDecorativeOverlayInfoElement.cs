using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary
{
	public class SpawnConditionDecorativeOverlayInfoElement : IBestiaryInfoElement, IBestiaryBackgroundOverlayAndColorProvider
	{
		private string _overlayImagePath;

		private Color? _overlayColor;

		public float DisplayPriority { get; set; }

		public SpawnConditionDecorativeOverlayInfoElement(string overlayImagePath = null, Color? overlayColor = null)
		{
			_overlayImagePath = overlayImagePath;
			_overlayColor = overlayColor;
		}

		public Asset<Texture2D> GetBackgroundOverlayImage()
		{
			if (_overlayImagePath == null)
			{
				return null;
			}
			return Main.Assets.Request<Texture2D>(_overlayImagePath, (AssetRequestMode)1);
		}

		public Color? GetBackgroundOverlayColor()
		{
			return _overlayColor;
		}

		public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
		{
			return null;
		}
	}
}
