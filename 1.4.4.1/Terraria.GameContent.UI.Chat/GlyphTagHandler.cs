using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat
{
	public class GlyphTagHandler : ITagHandler
	{
		private class GlyphSnippet : TextSnippet
		{
			private int _glyphIndex;

			public GlyphSnippet(int index)
			{
				_glyphIndex = index;
				Color = Color.White;
			}

			public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
			{
				if (!justCheckingString && color != Color.Black)
				{
					int num = _glyphIndex;
					int glyphIndex = _glyphIndex;
					if (glyphIndex == 25)
					{
						num = ((Main.GlobalTimeWrappedHourly % 0.6f < 0.3f) ? 17 : 18);
					}
					Texture2D value = TextureAssets.TextGlyph[0].get_Value();
					spriteBatch.Draw(value, position, value.Frame(25, 1, num, num / 25), color, 0f, Vector2.Zero, GlyphsScale, SpriteEffects.None, 0f);
				}
				size = new Vector2(26f) * GlyphsScale;
				return true;
			}

			public override float GetStringLength(DynamicSpriteFont font)
			{
				return 26f * GlyphsScale;
			}
		}

		private const int GlyphsPerLine = 25;

		private const int MaxGlyphs = 26;

		public static float GlyphsScale = 1f;

		private static Dictionary<string, int> GlyphIndexes = new Dictionary<string, int>
		{
			{
				Buttons.A.ToString(),
				0
			},
			{
				Buttons.B.ToString(),
				1
			},
			{
				Buttons.Back.ToString(),
				4
			},
			{
				Buttons.DPadDown.ToString(),
				15
			},
			{
				Buttons.DPadLeft.ToString(),
				14
			},
			{
				Buttons.DPadRight.ToString(),
				13
			},
			{
				Buttons.DPadUp.ToString(),
				16
			},
			{
				Buttons.LeftShoulder.ToString(),
				6
			},
			{
				Buttons.LeftStick.ToString(),
				10
			},
			{
				Buttons.LeftThumbstickDown.ToString(),
				20
			},
			{
				Buttons.LeftThumbstickLeft.ToString(),
				17
			},
			{
				Buttons.LeftThumbstickRight.ToString(),
				18
			},
			{
				Buttons.LeftThumbstickUp.ToString(),
				19
			},
			{
				Buttons.LeftTrigger.ToString(),
				8
			},
			{
				Buttons.RightShoulder.ToString(),
				7
			},
			{
				Buttons.RightStick.ToString(),
				11
			},
			{
				Buttons.RightThumbstickDown.ToString(),
				24
			},
			{
				Buttons.RightThumbstickLeft.ToString(),
				21
			},
			{
				Buttons.RightThumbstickRight.ToString(),
				22
			},
			{
				Buttons.RightThumbstickUp.ToString(),
				23
			},
			{
				Buttons.RightTrigger.ToString(),
				9
			},
			{
				Buttons.Start.ToString(),
				5
			},
			{
				Buttons.X.ToString(),
				2
			},
			{
				Buttons.Y.ToString(),
				3
			},
			{ "LR", 25 }
		};

		TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
		{
			if (!int.TryParse(text, out var result) || result >= 26)
			{
				return new TextSnippet(text);
			}
			return new GlyphSnippet(result)
			{
				DeleteWhole = true,
				Text = "[g:" + result + "]"
			};
		}

		public static string GenerateTag(int index)
		{
			string text = "[g";
			return text + ":" + index + "]";
		}

		public static string GenerateTag(string keyname)
		{
			if (GlyphIndexes.TryGetValue(keyname, out var value))
			{
				return GenerateTag(value);
			}
			return keyname;
		}
	}
}
