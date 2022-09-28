using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIResourcePack : UIPanel
	{
		private const int PANEL_PADDING = 5;

		private const int ICON_SIZE = 64;

		private const int ICON_BORDER_PADDING = 4;

		private const int HEIGHT_FLUFF = 10;

		private const float HEIGHT = 102f;

		private const float MIN_WIDTH = 102f;

		private static readonly Color DefaultBackgroundColor = new Color(26, 40, 89) * 0.8f;

		private static readonly Color DefaultBorderColor = new Color(13, 20, 44) * 0.8f;

		private static readonly Color HoverBackgroundColor = new Color(46, 60, 119);

		private static readonly Color HoverBorderColor = new Color(20, 30, 56);

		public readonly ResourcePack ResourcePack;

		private readonly Asset<Texture2D> _iconBorderTexture;

		public int Order { get; set; }

		public UIElement ContentPanel { get; private set; }

		public UIResourcePack(ResourcePack pack, int order)
		{
			ResourcePack = pack;
			Order = order;
			BackgroundColor = DefaultBackgroundColor;
			BorderColor = DefaultBorderColor;
			Height = StyleDimension.FromPixels(102f);
			MinHeight = Height;
			MaxHeight = Height;
			MinWidth = StyleDimension.FromPixels(102f);
			Width = StyleDimension.FromPercent(1f);
			SetPadding(5f);
			_iconBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", (AssetRequestMode)1);
			OverflowHidden = true;
			BuildChildren();
		}

		private void BuildChildren()
		{
			StyleDimension left = StyleDimension.FromPixels(77f);
			StyleDimension top = StyleDimension.FromPixels(4f);
			UIText uIText = new UIText(ResourcePack.Name)
			{
				Left = left,
				Top = top
			};
			Append(uIText);
			top.Pixels += uIText.GetOuterDimensions().Height + 6f;
			UIText uIText2 = new UIText(Language.GetTextValue("UI.Author", ResourcePack.Author), 0.7f)
			{
				Left = left,
				Top = top
			};
			Append(uIText2);
			top.Pixels += uIText2.GetOuterDimensions().Height + 10f;
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/Divider", (AssetRequestMode)1);
			UIImage uIImage = new UIImage(val)
			{
				Left = StyleDimension.FromPixels(72f),
				Top = top,
				Height = StyleDimension.FromPixels(val.Height()),
				Width = StyleDimension.FromPixelsAndPercent(-80f, 1f),
				ScaleToFit = true
			};
			Recalculate();
			Append(uIImage);
			top.Pixels += uIImage.GetOuterDimensions().Height + 5f;
			UIElement uIElement = new UIElement
			{
				Left = left,
				Top = top,
				Height = StyleDimension.FromPixels(92f - top.Pixels),
				Width = StyleDimension.FromPixelsAndPercent(0f - left.Pixels, 1f)
			};
			Append(uIElement);
			ContentPanel = uIElement;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			DrawIcon(spriteBatch);
			if (ResourcePack.Branding == ResourcePack.BrandingType.SteamWorkshop)
			{
				Asset<Texture2D> val = TextureAssets.Extra[243];
				spriteBatch.Draw(val.get_Value(), new Vector2(GetDimensions().X + GetDimensions().Width - (float)val.Width() - 3f, GetDimensions().Y + 2f), val.Frame(), Color.White);
			}
		}

		private void DrawIcon(SpriteBatch spriteBatch)
		{
			CalculatedStyle innerDimensions = GetInnerDimensions();
			spriteBatch.Draw(ResourcePack.Icon, new Rectangle((int)innerDimensions.X + 4, (int)innerDimensions.Y + 4 + 10, 64, 64), Color.White);
			spriteBatch.Draw(_iconBorderTexture.get_Value(), new Rectangle((int)innerDimensions.X, (int)innerDimensions.Y + 10, 72, 72), Color.White);
		}

		public override int CompareTo(object obj)
		{
			return Order.CompareTo(((UIResourcePack)obj).Order);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			BackgroundColor = HoverBackgroundColor;
			BorderColor = HoverBorderColor;
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			BackgroundColor = DefaultBackgroundColor;
			BorderColor = DefaultBorderColor;
		}
	}
}
