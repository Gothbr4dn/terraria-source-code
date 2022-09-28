using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.IO;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public abstract class AWorldListItem : UIPanel
	{
		protected WorldFileData _data;

		protected int _glitchFrameCounter;

		protected int _glitchFrame;

		protected int _glitchVariation;

		private void UpdateGlitchAnimation(UIElement affectedElement)
		{
			_ = _glitchFrame;
			int minValue = 3;
			int num = 3;
			if (_glitchFrame == 0)
			{
				minValue = 15;
				num = 120;
			}
			if (++_glitchFrameCounter >= Main.rand.Next(minValue, num + 1))
			{
				_glitchFrameCounter = 0;
				_glitchFrame = (_glitchFrame + 1) % 16;
				if ((_glitchFrame == 4 || _glitchFrame == 8 || _glitchFrame == 12) && Main.rand.Next(3) == 0)
				{
					_glitchVariation = Main.rand.Next(7);
				}
			}
			(affectedElement as UIImageFramed).SetFrame(7, 16, _glitchVariation, _glitchFrame, 0, 0);
		}

		protected Asset<Texture2D> GetIcon()
		{
			if (_data.ZenithWorld)
			{
				return Main.Assets.Request<Texture2D>("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + "Everything", (AssetRequestMode)1);
			}
			if (_data.DrunkWorld)
			{
				return Main.Assets.Request<Texture2D>("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + "CorruptionCrimson", (AssetRequestMode)1);
			}
			if (_data.ForTheWorthy)
			{
				return GetSeedIcon("FTW");
			}
			if (_data.NotTheBees)
			{
				return GetSeedIcon("NotTheBees");
			}
			if (_data.Anniversary)
			{
				return GetSeedIcon("Anniversary");
			}
			if (_data.DontStarve)
			{
				return GetSeedIcon("DontStarve");
			}
			if (_data.RemixWorld)
			{
				return GetSeedIcon("Remix");
			}
			if (_data.NoTrapsWorld)
			{
				return GetSeedIcon("Traps");
			}
			return Main.Assets.Request<Texture2D>("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + (_data.HasCorruption ? "Corruption" : "Crimson"), (AssetRequestMode)1);
		}

		protected UIElement GetIconElement()
		{
			if (_data.DrunkWorld && _data.RemixWorld)
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/IconEverythingAnimated", (AssetRequestMode)1);
				UIImageFramed uIImageFramed = new UIImageFramed(obj, obj.Frame(7, 16));
				uIImageFramed.Left = new StyleDimension(4f, 0f);
				uIImageFramed.OnUpdate += UpdateGlitchAnimation;
				return uIImageFramed;
			}
			return new UIImage(GetIcon())
			{
				Left = new StyleDimension(4f, 0f)
			};
		}

		private Asset<Texture2D> GetSeedIcon(string seed)
		{
			return Main.Assets.Request<Texture2D>("Images/UI/Icon" + (_data.IsHardMode ? "Hallow" : "") + (_data.HasCorruption ? "Corruption" : "Crimson") + seed, (AssetRequestMode)1);
		}
	}
}
