using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.ID;

namespace Terraria.GameContent.Drawing
{
	public class WallDrawing
	{
		private static VertexColors _glowPaintColors = new VertexColors(Color.White);

		private Tile[,] _tileArray;

		private TilePaintSystemV2 _paintSystem;

		private bool _shouldShowInvisibleWalls;

		public void LerpVertexColorsWithColor(ref VertexColors colors, Color lerpColor, float percent)
		{
			colors.TopLeftColor = Color.Lerp(colors.TopLeftColor, lerpColor, percent);
			colors.TopRightColor = Color.Lerp(colors.TopRightColor, lerpColor, percent);
			colors.BottomLeftColor = Color.Lerp(colors.BottomLeftColor, lerpColor, percent);
			colors.BottomRightColor = Color.Lerp(colors.BottomRightColor, lerpColor, percent);
		}

		public WallDrawing(TilePaintSystemV2 paintSystem)
		{
			_paintSystem = paintSystem;
		}

		public void Update()
		{
			if (!Main.dedServ)
			{
				_shouldShowInvisibleWalls = Main.ShouldShowInvisibleWalls();
			}
		}

		public void DrawWalls()
		{
			float gfxQuality = Main.gfxQuality;
			int offScreenRange = Main.offScreenRange;
			bool drawToScreen = Main.drawToScreen;
			Vector2 screenPosition = Main.screenPosition;
			int screenWidth = Main.screenWidth;
			int screenHeight = Main.screenHeight;
			int maxTilesX = Main.maxTilesX;
			int maxTilesY = Main.maxTilesY;
			int[] wallBlend = Main.wallBlend;
			SpriteBatch spriteBatch = Main.spriteBatch;
			TileBatch tileBatch = Main.tileBatch;
			_tileArray = Main.tile;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int num = (int)(120f * (1f - gfxQuality) + 40f * gfxQuality);
			int num2 = (int)((float)num * 0.4f);
			int num3 = (int)((float)num * 0.35f);
			int num4 = (int)((float)num * 0.3f);
			Vector2 vector = new Vector2(offScreenRange, offScreenRange);
			if (drawToScreen)
			{
				vector = Vector2.Zero;
			}
			int num5 = (int)((screenPosition.X - vector.X) / 16f - 1f);
			int num6 = (int)((screenPosition.X + (float)screenWidth + vector.X) / 16f) + 2;
			int num7 = (int)((screenPosition.Y - vector.Y) / 16f - 1f);
			int num8 = (int)((screenPosition.Y + (float)screenHeight + vector.Y) / 16f) + 5;
			int num9 = offScreenRange / 16;
			int num10 = offScreenRange / 16;
			if (num5 - num9 < 4)
			{
				num5 = num9 + 4;
			}
			if (num6 + num9 > maxTilesX - 4)
			{
				num6 = maxTilesX - num9 - 4;
			}
			if (num7 - num10 < 4)
			{
				num7 = num10 + 4;
			}
			if (num8 + num10 > maxTilesY - 4)
			{
				num8 = maxTilesY - num10 - 4;
			}
			VertexColors vertices = default(VertexColors);
			Rectangle value = new Rectangle(0, 0, 32, 32);
			int underworldLayer = Main.UnderworldLayer;
			Point screenOverdrawOffset = Main.GetScreenOverdrawOffset();
			for (int i = num7 - num10 + screenOverdrawOffset.Y; i < num8 + num10 - screenOverdrawOffset.Y; i++)
			{
				for (int j = num5 - num9 + screenOverdrawOffset.X; j < num6 + num9 - screenOverdrawOffset.X; j++)
				{
					Tile tile = _tileArray[j, i];
					if (tile == null)
					{
						tile = new Tile();
						_tileArray[j, i] = tile;
					}
					ushort wall = tile.wall;
					if (wall <= 0 || FullTile(j, i) || (wall == 318 && !_shouldShowInvisibleWalls) || (tile.invisibleWall() && !_shouldShowInvisibleWalls))
					{
						continue;
					}
					Color color = Lighting.GetColor(j, i);
					if (tile.fullbrightWall())
					{
						color = Color.White;
					}
					if (wall == 318)
					{
						color = Color.White;
					}
					if (color.R == 0 && color.G == 0 && color.B == 0 && i < underworldLayer)
					{
						continue;
					}
					Main.instance.LoadWall(wall);
					value.X = tile.wallFrameX();
					value.Y = tile.wallFrameY() + Main.wallFrame[wall] * 180;
					ushort wall2 = tile.wall;
					if ((uint)(wall2 - 242) <= 1u)
					{
						int num11 = 20;
						int num12 = (Main.wallFrameCounter[wall] + j * 11 + i * 27) % (num11 * 8);
						value.Y = tile.wallFrameY() + 180 * (num12 / num11);
					}
					if (Lighting.NotRetro && !Main.wallLight[wall] && tile.wall != 241 && (tile.wall < 88 || tile.wall > 93) && !WorldGen.SolidTile(tile))
					{
						Texture2D tileDrawTexture = GetTileDrawTexture(tile, j, i);
						if (tile.wall == 346)
						{
							vertices.TopRightColor = (vertices.TopLeftColor = (vertices.BottomRightColor = (vertices.BottomLeftColor = new Color((byte)Main.DiscoR, (byte)Main.DiscoG, (byte)Main.DiscoB))));
						}
						else if (tile.wall == 44)
						{
							vertices.TopRightColor = (vertices.TopLeftColor = (vertices.BottomRightColor = (vertices.BottomLeftColor = new Color((byte)Main.DiscoR, (byte)Main.DiscoG, (byte)Main.DiscoB))));
						}
						else
						{
							Lighting.GetCornerColors(j, i, out vertices);
							wall2 = tile.wall;
							if ((uint)(wall2 - 341) <= 4u)
							{
								LerpVertexColorsWithColor(ref vertices, Color.White, 0.5f);
							}
							if (tile.fullbrightWall())
							{
								vertices = _glowPaintColors;
							}
						}
						tileBatch.Draw(tileDrawTexture, new Vector2(j * 16 - (int)screenPosition.X - 8, i * 16 - (int)screenPosition.Y - 8) + vector, value, vertices, Vector2.Zero, 1f, SpriteEffects.None);
					}
					else
					{
						Color color2 = color;
						if (wall == 44 || wall == 346)
						{
							color2 = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
						}
						if ((uint)(wall - 341) <= 4u)
						{
							color2 = Color.Lerp(color2, Color.White, 0.5f);
						}
						Texture2D tileDrawTexture2 = GetTileDrawTexture(tile, j, i);
						spriteBatch.Draw(tileDrawTexture2, new Vector2(j * 16 - (int)screenPosition.X - 8, i * 16 - (int)screenPosition.Y - 8) + vector, value, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					if (color.R > num2 || color.G > num3 || color.B > num4)
					{
						bool num13 = _tileArray[j - 1, i].wall > 0 && wallBlend[_tileArray[j - 1, i].wall] != wallBlend[tile.wall];
						bool flag = _tileArray[j + 1, i].wall > 0 && wallBlend[_tileArray[j + 1, i].wall] != wallBlend[tile.wall];
						bool flag2 = _tileArray[j, i - 1].wall > 0 && wallBlend[_tileArray[j, i - 1].wall] != wallBlend[tile.wall];
						bool flag3 = _tileArray[j, i + 1].wall > 0 && wallBlend[_tileArray[j, i + 1].wall] != wallBlend[tile.wall];
						if (num13)
						{
							spriteBatch.Draw(TextureAssets.WallOutline.get_Value(), new Vector2(j * 16 - (int)screenPosition.X, i * 16 - (int)screenPosition.Y) + vector, new Rectangle(0, 0, 2, 16), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						}
						if (flag)
						{
							spriteBatch.Draw(TextureAssets.WallOutline.get_Value(), new Vector2(j * 16 - (int)screenPosition.X + 14, i * 16 - (int)screenPosition.Y) + vector, new Rectangle(14, 0, 2, 16), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						}
						if (flag2)
						{
							spriteBatch.Draw(TextureAssets.WallOutline.get_Value(), new Vector2(j * 16 - (int)screenPosition.X, i * 16 - (int)screenPosition.Y) + vector, new Rectangle(0, 0, 16, 2), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						}
						if (flag3)
						{
							spriteBatch.Draw(TextureAssets.WallOutline.get_Value(), new Vector2(j * 16 - (int)screenPosition.X, i * 16 - (int)screenPosition.Y + 14) + vector, new Rectangle(0, 14, 16, 2), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						}
					}
				}
			}
			Main.instance.DrawTileCracks(2, Main.LocalPlayer.hitReplace);
			Main.instance.DrawTileCracks(2, Main.LocalPlayer.hitTile);
			TimeLogger.DrawTime(2, stopwatch.Elapsed.TotalMilliseconds);
		}

		private Texture2D GetTileDrawTexture(Tile tile, int tileX, int tileY)
		{
			Texture2D result = TextureAssets.Wall[tile.wall].get_Value();
			int wall = tile.wall;
			Texture2D texture2D = _paintSystem.TryGetWallAndRequestIfNotReady(wall, tile.wallColor());
			if (texture2D != null)
			{
				result = texture2D;
			}
			return result;
		}

		protected bool FullTile(int x, int y)
		{
			if (_tileArray[x - 1, y] == null || _tileArray[x - 1, y].blockType() != 0 || _tileArray[x + 1, y] == null || _tileArray[x + 1, y].blockType() != 0)
			{
				return false;
			}
			Tile tile = _tileArray[x, y];
			if (tile == null)
			{
				return false;
			}
			if (tile.active())
			{
				if (tile.type < TileID.Sets.DrawsWalls.Length && TileID.Sets.DrawsWalls[tile.type])
				{
					return false;
				}
				if (tile.invisibleBlock() && !_shouldShowInvisibleWalls)
				{
					return false;
				}
				if (Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
				{
					int frameX = tile.frameX;
					int frameY = tile.frameY;
					if (Main.tileLargeFrames[tile.type] > 0)
					{
						if (frameY == 18 || frameY == 108)
						{
							if (frameX >= 18 && frameX <= 54)
							{
								return true;
							}
							if (frameX >= 108 && frameX <= 144)
							{
								return true;
							}
						}
					}
					else if (frameY == 18)
					{
						if (frameX >= 18 && frameX <= 54)
						{
							return true;
						}
						if (frameX >= 108 && frameX <= 144)
						{
							return true;
						}
					}
					else if (frameY >= 90 && frameY <= 196)
					{
						if (frameX <= 70)
						{
							return true;
						}
						if (frameX >= 144 && frameX <= 232)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
