using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent.Bestiary
{
	public class SpawnConditionBestiaryInfoElement : FilterProviderInfoElement, IBestiaryBackgroundImagePathAndColorProvider, IBestiaryPrioritizedElement
	{
		private string _backgroundImagePath;

		private Color? _backgroundColor;

		public float OrderPriority { get; set; }

		public SpawnConditionBestiaryInfoElement(string nameLanguageKey, int filterIconFrame, string backgroundImagePath = null, Color? backgroundColor = null)
			: base(nameLanguageKey, filterIconFrame)
		{
			_backgroundImagePath = backgroundImagePath;
			_backgroundColor = backgroundColor;
		}

		public Asset<Texture2D> GetBackgroundImage()
		{
			if (_backgroundImagePath == null)
			{
				return null;
			}
			return Main.Assets.Request<Texture2D>(_backgroundImagePath, (AssetRequestMode)1);
		}

		public Color? GetBackgroundColor()
		{
			return _backgroundColor;
		}
	}
}
