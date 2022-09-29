using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIHeader : UIElement
	{
		private string _text;

		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (_text != value)
				{
					_text = value;
					if (!Main.dedServ)
					{
						Vector2 vector = FontAssets.DeathText.get_Value().MeasureString(Text);
						Width.Pixels = vector.X;
						Height.Pixels = vector.Y;
					}
					Width.Precent = 0f;
					Height.Precent = 0f;
					Recalculate();
				}
			}
		}

		public UIHeader()
		{
			Text = "";
		}

		public UIHeader(string text)
		{
			Text = text;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			float num = 1.2f;
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.get_Value(), Text, new Vector2(dimensions.X - num, dimensions.Y - num), Color.Black);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.get_Value(), Text, new Vector2(dimensions.X + num, dimensions.Y - num), Color.Black);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.get_Value(), Text, new Vector2(dimensions.X - num, dimensions.Y + num), Color.Black);
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.get_Value(), Text, new Vector2(dimensions.X + num, dimensions.Y + num), Color.Black);
			if (WorldGen.tenthAnniversaryWorldGen && !WorldGen.remixWorldGen)
			{
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.get_Value(), Text, new Vector2(dimensions.X, dimensions.Y), Color.HotPink);
			}
			else
			{
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.DeathText.get_Value(), Text, new Vector2(dimensions.X, dimensions.Y), Color.White);
			}
		}
	}
}
