using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Initializers
{
	public class LinkButtonsInitializer
	{
		public static void Load()
		{
			List<TitleLinkButton> titleLinks = Main.TitleLinks;
			titleLinks.Add(MakeSimpleButton("TitleLinks.Discord", "https://discord.gg/terraria", 0));
			titleLinks.Add(MakeSimpleButton("TitleLinks.Instagram", "https://www.instagram.com/terraria_logic/", 1));
			titleLinks.Add(MakeSimpleButton("TitleLinks.Reddit", "https://www.reddit.com/r/Terraria/", 2));
			titleLinks.Add(MakeSimpleButton("TitleLinks.Twitter", "https://twitter.com/Terraria_Logic", 3));
			titleLinks.Add(MakeSimpleButton("TitleLinks.Forums", "https://forums.terraria.org/index.php", 4));
			titleLinks.Add(MakeSimpleButton("TitleLinks.Merch", "https://terraria.org/store", 5));
			titleLinks.Add(MakeSimpleButton("TitleLinks.Wiki", "https://terraria.wiki.gg/", 6));
		}

		private static TitleLinkButton MakeSimpleButton(string textKey, string linkUrl, int horizontalFrameIndex)
		{
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/TitleLinkButtons", (AssetRequestMode)1);
			Rectangle value = val.Frame(7, 2, horizontalFrameIndex);
			Rectangle value2 = val.Frame(7, 2, horizontalFrameIndex, 1);
			value.Width--;
			value.Height--;
			value2.Width--;
			value2.Height--;
			return new TitleLinkButton
			{
				TooltipTextKey = textKey,
				LinkUrl = linkUrl,
				FrameWehnSelected = value2,
				FrameWhenNotSelected = value,
				Image = val
			};
		}
	}
}
