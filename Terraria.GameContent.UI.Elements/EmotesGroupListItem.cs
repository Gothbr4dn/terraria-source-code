using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class EmotesGroupListItem : UIElement
	{
		private const int TITLE_HEIGHT = 20;

		private const int SEPARATOR_HEIGHT = 10;

		private const int SIZE_PER_EMOTE = 36;

		private Asset<Texture2D> _tempTex;

		private int _groupIndex;

		private int _maxEmotesPerRow = 10;

		public EmotesGroupListItem(LocalizedText groupTitle, int groupIndex, int maxEmotesPerRow, params int[] emotes)
		{
			maxEmotesPerRow = 14;
			SetPadding(0f);
			_groupIndex = groupIndex;
			_maxEmotesPerRow = maxEmotesPerRow;
			_tempTex = Main.Assets.Request<Texture2D>("Images/UI/ButtonFavoriteInactive", (AssetRequestMode)1);
			int num = emotes.Length / _maxEmotesPerRow;
			if (emotes.Length % _maxEmotesPerRow != 0)
			{
				num++;
			}
			Height.Set(30 + 36 * num, 0f);
			Width.Set(0f, 1f);
			UIElement uIElement = new UIElement
			{
				Height = StyleDimension.FromPixels(30f),
				Width = StyleDimension.FromPixelsAndPercent(-20f, 1f),
				HAlign = 0.5f
			};
			uIElement.SetPadding(0f);
			Append(uIElement);
			UIHorizontalSeparator element = new UIHorizontalSeparator
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				VAlign = 1f,
				HAlign = 0.5f,
				Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
			};
			uIElement.Append(element);
			UIText element2 = new UIText(groupTitle)
			{
				VAlign = 1f,
				HAlign = 0.5f,
				Top = StyleDimension.FromPixels(-6f)
			};
			uIElement.Append(element2);
			float num2 = 6f;
			for (int i = 0; i < emotes.Length; i++)
			{
				int emoteIndex = emotes[i];
				int num3 = i / _maxEmotesPerRow;
				int num4 = i % _maxEmotesPerRow;
				int num5 = emotes.Length % _maxEmotesPerRow;
				if (emotes.Length / _maxEmotesPerRow != num3)
				{
					num5 = _maxEmotesPerRow;
				}
				if (num5 == 0)
				{
					num5 = _maxEmotesPerRow;
				}
				float num6 = 36f * ((float)num5 / 2f);
				num6 -= 16f;
				num6 = -16f;
				EmoteButton emoteButton = new EmoteButton(emoteIndex)
				{
					HAlign = 0f,
					VAlign = 0f,
					Top = StyleDimension.FromPixels((float)(30 + num3 * 36) + num2),
					Left = StyleDimension.FromPixels((float)(36 * num4) - num6)
				};
				Append(emoteButton);
				emoteButton.SetSnapPoint("Group " + groupIndex, i);
			}
		}

		public override int CompareTo(object obj)
		{
			if (obj is EmotesGroupListItem emotesGroupListItem)
			{
				return _groupIndex.CompareTo(emotesGroupListItem._groupIndex);
			}
			return base.CompareTo(obj);
		}
	}
}
