using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;

namespace Terraria.UI.Chat
{
	public static class ChatManager
	{
		public static class Regexes
		{
			public static readonly Regex Format = new Regex("(?<!\\\\)\\[(?<tag>[a-zA-Z]{1,10})(\\/(?<options>[^:]+))?:(?<text>.+?)(?<!\\\\)\\]", RegexOptions.Compiled);
		}

		public static readonly ChatCommandProcessor Commands = new ChatCommandProcessor();

		private static ConcurrentDictionary<string, ITagHandler> _handlers = new ConcurrentDictionary<string, ITagHandler>();

		public static readonly Vector2[] ShadowDirections = new Vector2[4]
		{
			-Vector2.UnitX,
			Vector2.UnitX,
			-Vector2.UnitY,
			Vector2.UnitY
		};

		public static Color WaveColor(Color color)
		{
			float num = (float)(int)Main.mouseTextColor / 255f;
			color = Color.Lerp(color, Color.Black, 1f - num);
			color.A = Main.mouseTextColor;
			return color;
		}

		public static void ConvertNormalSnippets(TextSnippet[] snippets)
		{
			for (int i = 0; i < snippets.Length; i++)
			{
				TextSnippet textSnippet = snippets[i];
				if (snippets[i].GetType() == typeof(TextSnippet))
				{
					PlainTagHandler.PlainSnippet plainSnippet = (PlainTagHandler.PlainSnippet)(snippets[i] = new PlainTagHandler.PlainSnippet(textSnippet.Text, textSnippet.Color, textSnippet.Scale));
				}
			}
		}

		public static void Register<T>(params string[] names) where T : ITagHandler, new()
		{
			T val = new T();
			for (int i = 0; i < names.Length; i++)
			{
				_handlers[names[i].ToLower()] = val;
			}
		}

		private static ITagHandler GetHandler(string tagName)
		{
			string key = tagName.ToLower();
			if (_handlers.ContainsKey(key))
			{
				return _handlers[key];
			}
			return null;
		}

		public static List<TextSnippet> ParseMessage(string text, Color baseColor)
		{
			text = text.Replace("\r", "");
			MatchCollection matchCollection = Regexes.Format.Matches(text);
			List<TextSnippet> list = new List<TextSnippet>();
			int num = 0;
			foreach (Match item in matchCollection)
			{
				if (item.Index > num)
				{
					list.Add(new TextSnippet(text.Substring(num, item.Index - num), baseColor));
				}
				num = item.Index + item.Length;
				string value = item.Groups["tag"].Value;
				string value2 = item.Groups["text"].Value;
				string value3 = item.Groups["options"].Value;
				ITagHandler handler = GetHandler(value);
				if (handler != null)
				{
					list.Add(handler.Parse(value2, baseColor, value3));
					list[list.Count - 1].TextOriginal = item.ToString();
				}
				else
				{
					list.Add(new TextSnippet(value2, baseColor));
				}
			}
			if (text.Length > num)
			{
				list.Add(new TextSnippet(text.Substring(num, text.Length - num), baseColor));
			}
			return list;
		}

		public static bool AddChatText(DynamicSpriteFont font, string text, Vector2 baseScale)
		{
			int num = 470;
			num = Main.screenWidth - 330;
			if (GetStringSize(font, Main.chatText + text, baseScale).X > (float)num)
			{
				return false;
			}
			Main.chatText += text;
			return true;
		}

		public static Vector2 GetStringSize(DynamicSpriteFont font, string text, Vector2 baseScale, float maxWidth = -1f)
		{
			TextSnippet[] snippets = ParseMessage(text, Color.White).ToArray();
			return GetStringSize(font, snippets, baseScale, maxWidth);
		}

		public static Vector2 GetStringSize(DynamicSpriteFont font, TextSnippet[] snippets, Vector2 baseScale, float maxWidth = -1f)
		{
			Vector2 vec = new Vector2(Main.mouseX, Main.mouseY);
			Vector2 zero = Vector2.Zero;
			Vector2 vector = zero;
			Vector2 result = vector;
			float x = font.MeasureString(" ").X;
			float num = 1f;
			float num2 = 0f;
			foreach (TextSnippet textSnippet in snippets)
			{
				textSnippet.Update();
				num = textSnippet.Scale;
				if (textSnippet.UniqueDraw(justCheckingString: true, out var size, null))
				{
					vector.X += size.X * baseScale.X * num;
					result.X = Math.Max(result.X, vector.X);
					result.Y = Math.Max(result.Y, vector.Y + size.Y);
					continue;
				}
				string[] array = textSnippet.Text.Split(new char[1] { '\n' });
				string[] array2 = array;
				for (int j = 0; j < array2.Length; j++)
				{
					string[] array3 = array2[j].Split(new char[1] { ' ' });
					for (int k = 0; k < array3.Length; k++)
					{
						if (k != 0)
						{
							vector.X += x * baseScale.X * num;
						}
						if (maxWidth > 0f)
						{
							float num3 = font.MeasureString(array3[k]).X * baseScale.X * num;
							if (vector.X - zero.X + num3 > maxWidth)
							{
								vector.X = zero.X;
								vector.Y += (float)font.get_LineSpacing() * num2 * baseScale.Y;
								result.Y = Math.Max(result.Y, vector.Y);
								num2 = 0f;
							}
						}
						if (num2 < num)
						{
							num2 = num;
						}
						Vector2 vector2 = font.MeasureString(array3[k]);
						vec.Between(vector, vector + vector2);
						vector.X += vector2.X * baseScale.X * num;
						result.X = Math.Max(result.X, vector.X);
						result.Y = Math.Max(result.Y, vector.Y + vector2.Y);
					}
					if (array.Length > 1)
					{
						vector.X = zero.X;
						vector.Y += (float)font.get_LineSpacing() * num2 * baseScale.Y;
						result.Y = Math.Max(result.Y, vector.Y);
						num2 = 0f;
					}
				}
			}
			return result;
		}

		public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
		{
			for (int i = 0; i < ShadowDirections.Length; i++)
			{
				DrawColorCodedString(spriteBatch, font, snippets, position + ShadowDirections[i] * spread, baseColor, rotation, origin, baseScale, out var _, maxWidth, ignoreColors: true);
			}
		}

		public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth, bool ignoreColors = false)
		{
			int num = -1;
			Vector2 vec = new Vector2(Main.mouseX, Main.mouseY);
			Vector2 vector = position;
			Vector2 result = vector;
			float x = font.MeasureString(" ").X;
			Color color = baseColor;
			float num2 = 1f;
			float num3 = 0f;
			for (int i = 0; i < snippets.Length; i++)
			{
				TextSnippet textSnippet = snippets[i];
				textSnippet.Update();
				if (!ignoreColors)
				{
					color = textSnippet.GetVisibleColor();
				}
				num2 = textSnippet.Scale;
				if (textSnippet.UniqueDraw(justCheckingString: false, out var size, spriteBatch, vector, color, num2))
				{
					if (vec.Between(vector, vector + size))
					{
						num = i;
					}
					vector.X += size.X * baseScale.X * num2;
					result.X = Math.Max(result.X, vector.X);
					continue;
				}
				string[] array = textSnippet.Text.Split(new char[1] { '\n' });
				array = Regex.Split(textSnippet.Text, "(\n)");
				bool flag = true;
				foreach (string text in array)
				{
					string[] array2 = Regex.Split(text, "( )");
					array2 = text.Split(new char[1] { ' ' });
					if (text == "\n")
					{
						vector.Y += (float)font.get_LineSpacing() * num3 * baseScale.Y;
						vector.X = position.X;
						result.Y = Math.Max(result.Y, vector.Y);
						num3 = 0f;
						flag = false;
						continue;
					}
					for (int k = 0; k < array2.Length; k++)
					{
						if (k != 0)
						{
							vector.X += x * baseScale.X * num2;
						}
						if (maxWidth > 0f)
						{
							float num4 = font.MeasureString(array2[k]).X * baseScale.X * num2;
							if (vector.X - position.X + num4 > maxWidth)
							{
								vector.X = position.X;
								vector.Y += (float)font.get_LineSpacing() * num3 * baseScale.Y;
								result.Y = Math.Max(result.Y, vector.Y);
								num3 = 0f;
							}
						}
						if (num3 < num2)
						{
							num3 = num2;
						}
						DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, array2[k], vector, color, rotation, origin, baseScale * textSnippet.Scale * num2, SpriteEffects.None, 0f);
						Vector2 vector2 = font.MeasureString(array2[k]);
						if (vec.Between(vector, vector + vector2))
						{
							num = i;
						}
						vector.X += vector2.X * baseScale.X * num2;
						result.X = Math.Max(result.X, vector.X);
					}
					if (array.Length > 1 && flag)
					{
						vector.Y += (float)font.get_LineSpacing() * num3 * baseScale.Y;
						vector.X = position.X;
						result.Y = Math.Max(result.Y, vector.Y);
						num3 = 0f;
					}
					flag = true;
				}
			}
			hoveredSnippet = num;
			return result;
		}

		public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
		{
			DrawColorCodedStringShadow(spriteBatch, font, snippets, position, Color.Black, rotation, origin, baseScale, maxWidth, spread);
			return DrawColorCodedString(spriteBatch, font, snippets, position, Color.White, rotation, origin, baseScale, out hoveredSnippet, maxWidth);
		}

		public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, float rotation, Color color, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
		{
			DrawColorCodedStringShadow(spriteBatch, font, snippets, position, Color.Black, rotation, origin, baseScale, maxWidth, spread);
			return DrawColorCodedString(spriteBatch, font, snippets, position, color, rotation, origin, baseScale, out hoveredSnippet, maxWidth, ignoreColors: true);
		}

		public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
		{
			for (int i = 0; i < ShadowDirections.Length; i++)
			{
				DrawColorCodedString(spriteBatch, font, text, position + ShadowDirections[i] * spread, baseColor, rotation, origin, baseScale, maxWidth, ignoreColors: true);
			}
		}

		public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, bool ignoreColors = false)
		{
			Vector2 vector = position;
			Vector2 result = vector;
			string[] array = text.Split(new char[1] { '\n' });
			float x = font.MeasureString(" ").X;
			Color color = baseColor;
			float num = 1f;
			float num2 = 0f;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string[] array3 = array2[i].Split(new char[1] { ':' });
				foreach (string text2 in array3)
				{
					if (text2.StartsWith("sss"))
					{
						if (text2.StartsWith("sss1"))
						{
							if (!ignoreColors)
							{
								color = Color.Red;
							}
						}
						else if (text2.StartsWith("sss2"))
						{
							if (!ignoreColors)
							{
								color = Color.Blue;
							}
						}
						else if (text2.StartsWith("sssr") && !ignoreColors)
						{
							color = Color.White;
						}
						continue;
					}
					string[] array4 = text2.Split(new char[1] { ' ' });
					for (int k = 0; k < array4.Length; k++)
					{
						if (k != 0)
						{
							vector.X += x * baseScale.X * num;
						}
						if (maxWidth > 0f)
						{
							float num3 = font.MeasureString(array4[k]).X * baseScale.X * num;
							if (vector.X - position.X + num3 > maxWidth)
							{
								vector.X = position.X;
								vector.Y += (float)font.get_LineSpacing() * num2 * baseScale.Y;
								result.Y = Math.Max(result.Y, vector.Y);
								num2 = 0f;
							}
						}
						if (num2 < num)
						{
							num2 = num;
						}
						DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, font, array4[k], vector, color, rotation, origin, baseScale * num, SpriteEffects.None, 0f);
						vector.X += font.MeasureString(array4[k]).X * baseScale.X * num;
						result.X = Math.Max(result.X, vector.X);
					}
				}
				vector.X = position.X;
				vector.Y += (float)font.get_LineSpacing() * num2 * baseScale.Y;
				result.Y = Math.Max(result.Y, vector.Y);
				num2 = 0f;
			}
			return result;
		}

		public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
		{
			TextSnippet[] snippets = ParseMessage(text, baseColor).ToArray();
			ConvertNormalSnippets(snippets);
			DrawColorCodedStringShadow(spriteBatch, font, snippets, position, new Color(0, 0, 0, baseColor.A), rotation, origin, baseScale, maxWidth, spread);
			int hoveredSnippet;
			return DrawColorCodedString(spriteBatch, font, snippets, position, Color.White, rotation, origin, baseScale, out hoveredSnippet, maxWidth);
		}
	}
}
