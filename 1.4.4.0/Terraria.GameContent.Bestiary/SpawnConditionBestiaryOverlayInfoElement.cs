using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Bestiary
{
	public class SpawnConditionBestiaryOverlayInfoElement : FilterProviderInfoElement, IBestiaryBackgroundOverlayAndColorProvider, IBestiaryPrioritizedElement
	{
		private string _overlayImagePath;

		private Color? _overlayColor;

		public float DisplayPriority { get; set; }

		public float OrderPriority { get; set; }

		public SpawnConditionBestiaryOverlayInfoElement(string nameLanguageKey, int filterIconFrame, string overlayImagePath = null, Color? overlayColor = null)
			: base(nameLanguageKey, filterIconFrame)
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
	}
}
