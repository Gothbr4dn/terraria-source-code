using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.Localization;

namespace Terraria.DataStructures
{
	public class TitleLinkButton
	{
		private static Item _fakeItem = new Item();

		public string TooltipTextKey;

		public string LinkUrl;

		public Asset<Texture2D> Image;

		public Rectangle? FrameWhenNotSelected;

		public Rectangle? FrameWehnSelected;

		public void Draw(SpriteBatch spriteBatch, Vector2 anchorPosition)
		{
			Rectangle r = Image.Frame();
			if (FrameWhenNotSelected.HasValue)
			{
				r = FrameWhenNotSelected.Value;
			}
			Vector2 vector = r.Size();
			Vector2 vector2 = anchorPosition - vector / 2f;
			bool flag = false;
			if (Main.MouseScreen.Between(vector2, vector2 + vector))
			{
				Main.LocalPlayer.mouseInterface = true;
				flag = true;
				DrawTooltip();
				TryClicking();
			}
			Rectangle? rectangle = (flag ? FrameWehnSelected : FrameWhenNotSelected);
			Rectangle rectangle2 = Image.Frame();
			if (rectangle.HasValue)
			{
				rectangle2 = rectangle.Value;
			}
			Texture2D value = Image.get_Value();
			spriteBatch.Draw(value, anchorPosition, rectangle2, Color.White, 0f, rectangle2.Size() / 2f, 1f, SpriteEffects.None, 0f);
		}

		private void DrawTooltip()
		{
			Item fakeItem = _fakeItem;
			fakeItem.SetDefaults(0, noMatCheck: true);
			string textValue = Language.GetTextValue(TooltipTextKey);
			fakeItem.SetNameOverride(textValue);
			fakeItem.type = 1;
			fakeItem.scale = 0f;
			fakeItem.rare = 8;
			fakeItem.value = -1;
			Main.HoverItem = _fakeItem;
			Main.instance.MouseText("", 0, 0);
			Main.mouseText = true;
		}

		private void TryClicking()
		{
			if (!PlayerInput.IgnoreMouseInterface && Main.mouseLeft && Main.mouseLeftRelease)
			{
				SoundEngine.PlaySound(10);
				Main.mouseLeftRelease = false;
				OpenLink();
			}
		}

		private void OpenLink()
		{
			try
			{
				Platform.Get<IPathService>().OpenURL(LinkUrl);
			}
			catch
			{
				Console.WriteLine("Failed to open link?!");
			}
		}
	}
}
