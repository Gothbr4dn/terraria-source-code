using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.WorldBuilding
{
	public class TileFont
	{
		public struct DrawMode
		{
			public readonly ushort ForegroundTile;

			public readonly ushort BackgroundTile;

			public readonly bool HasBackground;

			public DrawMode(ushort foregroundTile)
			{
				ForegroundTile = foregroundTile;
				HasBackground = false;
				BackgroundTile = 0;
			}

			public DrawMode(ushort foregroundTile, ushort backgroundTile)
			{
				ForegroundTile = foregroundTile;
				BackgroundTile = backgroundTile;
				HasBackground = true;
			}
		}

		private static readonly Dictionary<char, byte[]> MicroFont = new Dictionary<char, byte[]>
		{
			{
				'A',
				new byte[5] { 124, 68, 68, 124, 68 }
			},
			{
				'B',
				new byte[5] { 124, 68, 120, 68, 124 }
			},
			{
				'C',
				new byte[5] { 124, 64, 64, 64, 124 }
			},
			{
				'D',
				new byte[5] { 120, 68, 68, 68, 120 }
			},
			{
				'E',
				new byte[5] { 124, 64, 120, 64, 124 }
			},
			{
				'F',
				new byte[5] { 124, 64, 112, 64, 64 }
			},
			{
				'G',
				new byte[5] { 124, 64, 76, 68, 124 }
			},
			{
				'H',
				new byte[5] { 68, 68, 124, 68, 68 }
			},
			{
				'I',
				new byte[5] { 124, 16, 16, 16, 124 }
			},
			{
				'J',
				new byte[5] { 12, 4, 4, 68, 124 }
			},
			{
				'K',
				new byte[5] { 68, 72, 112, 72, 68 }
			},
			{
				'L',
				new byte[5] { 64, 64, 64, 64, 124 }
			},
			{
				'M',
				new byte[5] { 68, 108, 84, 68, 68 }
			},
			{
				'N',
				new byte[5] { 68, 100, 84, 76, 68 }
			},
			{
				'O',
				new byte[5] { 124, 68, 68, 68, 124 }
			},
			{
				'P',
				new byte[5] { 120, 68, 120, 64, 64 }
			},
			{
				'Q',
				new byte[5] { 124, 68, 68, 124, 16 }
			},
			{
				'R',
				new byte[5] { 120, 68, 120, 68, 68 }
			},
			{
				'S',
				new byte[5] { 124, 64, 124, 4, 124 }
			},
			{
				'T',
				new byte[5] { 124, 16, 16, 16, 16 }
			},
			{
				'U',
				new byte[5] { 68, 68, 68, 68, 124 }
			},
			{
				'V',
				new byte[5] { 68, 68, 40, 40, 16 }
			},
			{
				'W',
				new byte[5] { 68, 68, 84, 84, 40 }
			},
			{
				'X',
				new byte[5] { 68, 40, 16, 40, 68 }
			},
			{
				'Y',
				new byte[5] { 68, 68, 40, 16, 16 }
			},
			{
				'Z',
				new byte[5] { 124, 8, 16, 32, 124 }
			},
			{
				'a',
				new byte[5] { 56, 4, 60, 68, 60 }
			},
			{
				'b',
				new byte[5] { 64, 120, 68, 68, 120 }
			},
			{
				'c',
				new byte[5] { 56, 68, 64, 68, 56 }
			},
			{
				'd',
				new byte[5] { 4, 60, 68, 68, 60 }
			},
			{
				'e',
				new byte[5] { 56, 68, 124, 64, 60 }
			},
			{
				'f',
				new byte[5] { 28, 32, 120, 32, 32 }
			},
			{
				'g',
				new byte[5] { 56, 68, 60, 4, 120 }
			},
			{
				'h',
				new byte[5] { 64, 64, 120, 68, 68 }
			},
			{
				'i',
				new byte[5] { 16, 0, 16, 16, 16 }
			},
			{
				'j',
				new byte[5] { 4, 4, 4, 4, 120 }
			},
			{
				'k',
				new byte[5] { 64, 72, 112, 72, 68 }
			},
			{
				'l',
				new byte[5] { 64, 64, 64, 64, 60 }
			},
			{
				'm',
				new byte[5] { 40, 84, 84, 84, 84 }
			},
			{
				'n',
				new byte[5] { 120, 68, 68, 68, 68 }
			},
			{
				'o',
				new byte[5] { 56, 68, 68, 68, 56 }
			},
			{
				'p',
				new byte[5] { 56, 68, 68, 120, 64 }
			},
			{
				'q',
				new byte[5] { 56, 68, 68, 60, 4 }
			},
			{
				'r',
				new byte[5] { 88, 100, 64, 64, 64 }
			},
			{
				's',
				new byte[5] { 60, 64, 56, 4, 120 }
			},
			{
				't',
				new byte[5] { 64, 112, 64, 68, 56 }
			},
			{
				'u',
				new byte[5] { 0, 68, 68, 68, 56 }
			},
			{
				'v',
				new byte[5] { 0, 68, 68, 40, 16 }
			},
			{
				'w',
				new byte[5] { 84, 84, 84, 84, 40 }
			},
			{
				'x',
				new byte[5] { 68, 68, 56, 68, 68 }
			},
			{
				'y',
				new byte[5] { 68, 68, 60, 4, 120 }
			},
			{
				'z',
				new byte[5] { 124, 4, 56, 64, 124 }
			},
			{
				'0',
				new byte[5] { 124, 76, 84, 100, 124 }
			},
			{
				'1',
				new byte[5] { 16, 48, 16, 16, 56 }
			},
			{
				'2',
				new byte[5] { 120, 4, 56, 64, 124 }
			},
			{
				'3',
				new byte[5] { 124, 4, 56, 4, 124 }
			},
			{
				'4',
				new byte[5] { 64, 64, 80, 124, 16 }
			},
			{
				'5',
				new byte[5] { 124, 64, 120, 4, 120 }
			},
			{
				'6',
				new byte[5] { 124, 64, 124, 68, 124 }
			},
			{
				'7',
				new byte[5] { 124, 4, 8, 16, 16 }
			},
			{
				'8',
				new byte[5] { 124, 68, 124, 68, 124 }
			},
			{
				'9',
				new byte[5] { 124, 68, 124, 4, 124 }
			},
			{
				'-',
				new byte[5] { 0, 0, 124, 0, 0 }
			},
			{
				' ',
				new byte[5]
			}
		};

		public static void DrawString(Point start, string text, DrawMode mode)
		{
			Point position = start;
			foreach (char c in text)
			{
				if (c == '\n')
				{
					position.X = start.X;
					position.Y += 6;
				}
				if (MicroFont.TryGetValue(c, out var value))
				{
					DrawChar(position, value, mode);
					position.X += 6;
				}
			}
		}

		private static void DrawChar(Point position, byte[] charData, DrawMode mode)
		{
			if (mode.HasBackground)
			{
				for (int i = -1; i < charData.Length + 1; i++)
				{
					for (int j = -1; j < 6; j++)
					{
						Main.tile[position.X + j, position.Y + i].ResetToType(mode.BackgroundTile);
						WorldGen.TileFrame(position.X + j, position.Y + i);
					}
				}
			}
			for (int k = 0; k < charData.Length; k++)
			{
				int num = charData[k] << 1;
				for (int l = 0; l < 5; l++)
				{
					if ((num & 0x80) == 128)
					{
						Main.tile[position.X + l, position.Y + k].ResetToType(mode.ForegroundTile);
						WorldGen.TileFrame(position.X + l, position.Y + k);
					}
					num <<= 1;
				}
			}
		}

		public static Point MeasureString(string text)
		{
			Point zero = Point.Zero;
			Point point = zero;
			Point result = new Point(0, 5);
			foreach (char c in text)
			{
				if (c == '\n')
				{
					point.X = zero.X;
					point.Y += 6;
					result.Y = point.Y + 5;
				}
				if (MicroFont.TryGetValue(c, out var _))
				{
					point.X += 6;
					result.X = Math.Max(result.X, point.X - 1);
				}
			}
			return result;
		}

		public static void HLineLabel(Point start, int width, string text, DrawMode mode, bool rightSideText = false)
		{
			Point point = MeasureString(text);
			for (int i = start.X; i < start.X + width; i++)
			{
				Main.tile[i, start.Y].ResetToType(mode.ForegroundTile);
				WorldGen.TileFrame(i, start.Y);
			}
			DrawString(new Point(rightSideText ? (start.X + width + 1) : (start.X - point.X - 1), start.Y - point.Y / 2), text, mode);
		}

		public static void VLineLabel(Point start, int height, string text, DrawMode mode, bool bottomText = false)
		{
			Point point = MeasureString(text);
			for (int i = start.Y; i < start.Y + height; i++)
			{
				Main.tile[start.X, i].ResetToType(mode.ForegroundTile);
				WorldGen.TileFrame(start.X, i);
			}
			DrawString(new Point(start.X - point.X / 2, bottomText ? (start.Y + height + 1) : (start.Y - point.Y - 1)), text, mode);
		}
	}
}
