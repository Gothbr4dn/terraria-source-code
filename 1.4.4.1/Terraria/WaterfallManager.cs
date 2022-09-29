using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Liquid;
using Terraria.ID;
using Terraria.IO;

namespace Terraria
{
	public class WaterfallManager
	{
		public struct WaterfallData
		{
			public int x;

			public int y;

			public int type;

			public int stopAtStep;
		}

		private const int minWet = 160;

		private const int maxWaterfallCountDefault = 1000;

		private const int maxLength = 100;

		private const int maxTypes = 26;

		public int maxWaterfallCount = 1000;

		private int qualityMax;

		private int currentMax;

		private WaterfallData[] waterfalls;

		private Asset<Texture2D>[] waterfallTexture = new Asset<Texture2D>[26];

		private int wFallFrCounter;

		private int regularFrame;

		private int wFallFrCounter2;

		private int slowFrame;

		private int rainFrameCounter;

		private int rainFrameForeground;

		private int rainFrameBackground;

		private int snowFrameCounter;

		private int snowFrameForeground;

		private int findWaterfallCount;

		private int waterfallDist = 100;

		public WaterfallManager()
		{
			waterfalls = new WaterfallData[1000];
			Main.Configuration.OnLoad += delegate(Preferences preferences)
			{
				maxWaterfallCount = Math.Max(0, preferences.Get("WaterfallDrawLimit", 1000));
				waterfalls = new WaterfallData[maxWaterfallCount];
			};
		}

		public void LoadContent()
		{
			for (int i = 0; i < 26; i++)
			{
				waterfallTexture[i] = Main.Assets.Request<Texture2D>("Images/Waterfall_" + i, (AssetRequestMode)2);
			}
		}

		public bool CheckForWaterfall(int i, int j)
		{
			for (int k = 0; k < currentMax; k++)
			{
				if (waterfalls[k].x == i && waterfalls[k].y == j)
				{
					return true;
				}
			}
			return false;
		}

		public void FindWaterfalls(bool forced = false)
		{
			findWaterfallCount++;
			if (findWaterfallCount < 30 && !forced)
			{
				return;
			}
			findWaterfallCount = 0;
			waterfallDist = (int)(75f * Main.gfxQuality) + 25;
			qualityMax = (int)((float)maxWaterfallCount * Main.gfxQuality);
			currentMax = 0;
			int num = (int)(Main.screenPosition.X / 16f - 1f);
			int num2 = (int)((Main.screenPosition.X + (float)Main.screenWidth) / 16f) + 2;
			int num3 = (int)(Main.screenPosition.Y / 16f - 1f);
			int num4 = (int)((Main.screenPosition.Y + (float)Main.screenHeight) / 16f) + 2;
			num -= waterfallDist;
			num2 += waterfallDist;
			num3 -= waterfallDist;
			num4 += 20;
			if (num < 0)
			{
				num = 0;
			}
			if (num2 > Main.maxTilesX)
			{
				num2 = Main.maxTilesX;
			}
			if (num3 < 0)
			{
				num3 = 0;
			}
			if (num4 > Main.maxTilesY)
			{
				num4 = Main.maxTilesY;
			}
			for (int i = num; i < num2; i++)
			{
				for (int j = num3; j < num4; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile == null)
					{
						tile = new Tile();
						Main.tile[i, j] = tile;
					}
					if (!tile.active())
					{
						continue;
					}
					if (tile.halfBrick())
					{
						Tile tile2 = Main.tile[i, j - 1];
						if (tile2 == null)
						{
							tile2 = new Tile();
							Main.tile[i, j - 1] = tile2;
						}
						if (tile2.liquid < 16 || WorldGen.SolidTile(tile2))
						{
							Tile tile3 = Main.tile[i - 1, j];
							if (tile3 == null)
							{
								tile3 = new Tile();
								Main.tile[i - 1, j] = tile3;
							}
							Tile tile4 = Main.tile[i + 1, j];
							if (tile4 == null)
							{
								tile4 = new Tile();
								Main.tile[i + 1, j] = tile4;
							}
							if ((tile3.liquid > 160 || tile4.liquid > 160) && ((tile3.liquid == 0 && !WorldGen.SolidTile(tile3) && tile3.slope() == 0) || (tile4.liquid == 0 && !WorldGen.SolidTile(tile4) && tile4.slope() == 0)) && currentMax < qualityMax)
							{
								waterfalls[currentMax].type = 0;
								if (tile2.lava() || tile4.lava() || tile3.lava())
								{
									waterfalls[currentMax].type = 1;
								}
								else if (tile2.honey() || tile4.honey() || tile3.honey())
								{
									waterfalls[currentMax].type = 14;
								}
								else if (tile2.shimmer() || tile4.shimmer() || tile3.shimmer())
								{
									waterfalls[currentMax].type = 25;
								}
								else
								{
									waterfalls[currentMax].type = 0;
								}
								waterfalls[currentMax].x = i;
								waterfalls[currentMax].y = j;
								currentMax++;
							}
						}
					}
					if (tile.type == 196)
					{
						Tile tile5 = Main.tile[i, j + 1];
						if (tile5 == null)
						{
							tile5 = new Tile();
							Main.tile[i, j + 1] = tile5;
						}
						if (!WorldGen.SolidTile(tile5) && tile5.slope() == 0 && currentMax < qualityMax)
						{
							waterfalls[currentMax].type = 11;
							waterfalls[currentMax].x = i;
							waterfalls[currentMax].y = j + 1;
							currentMax++;
						}
					}
					if (tile.type == 460)
					{
						Tile tile6 = Main.tile[i, j + 1];
						if (tile6 == null)
						{
							tile6 = new Tile();
							Main.tile[i, j + 1] = tile6;
						}
						if (!WorldGen.SolidTile(tile6) && tile6.slope() == 0 && currentMax < qualityMax)
						{
							waterfalls[currentMax].type = 22;
							waterfalls[currentMax].x = i;
							waterfalls[currentMax].y = j + 1;
							currentMax++;
						}
					}
				}
			}
		}

		public void UpdateFrame()
		{
			wFallFrCounter++;
			if (wFallFrCounter > 2)
			{
				wFallFrCounter = 0;
				regularFrame++;
				if (regularFrame > 15)
				{
					regularFrame = 0;
				}
			}
			wFallFrCounter2++;
			if (wFallFrCounter2 > 6)
			{
				wFallFrCounter2 = 0;
				slowFrame++;
				if (slowFrame > 15)
				{
					slowFrame = 0;
				}
			}
			rainFrameCounter++;
			if (rainFrameCounter > 0)
			{
				rainFrameForeground++;
				if (rainFrameForeground > 7)
				{
					rainFrameForeground -= 8;
				}
				if (rainFrameCounter > 2)
				{
					rainFrameCounter = 0;
					rainFrameBackground--;
					if (rainFrameBackground < 0)
					{
						rainFrameBackground = 7;
					}
				}
			}
			if (++snowFrameCounter > 3)
			{
				snowFrameCounter = 0;
				if (++snowFrameForeground > 7)
				{
					snowFrameForeground = 0;
				}
			}
		}

		private void DrawWaterfall(int Style = 0, float Alpha = 1f)
		{
			Main.tileSolid[546] = false;
			float num = 0f;
			float num2 = 99999f;
			float num3 = 99999f;
			int num4 = -1;
			int num5 = -1;
			float num6 = 0f;
			float num7 = 99999f;
			float num8 = 99999f;
			int num9 = -1;
			int num10 = -1;
			for (int i = 0; i < currentMax; i++)
			{
				int num11 = 0;
				int num12 = waterfalls[i].type;
				int num13 = waterfalls[i].x;
				int num14 = waterfalls[i].y;
				int num15 = 0;
				int num16 = 0;
				int num17 = 0;
				int num18 = 0;
				int num19 = 0;
				int num20 = 0;
				int num21;
				int num22;
				if (num12 == 1 || num12 == 14 || num12 == 25)
				{
					if (Main.drewLava || waterfalls[i].stopAtStep == 0)
					{
						continue;
					}
					num21 = 32 * slowFrame;
				}
				else
				{
					switch (num12)
					{
					case 11:
					case 22:
					{
						if (Main.drewLava)
						{
							continue;
						}
						num22 = waterfallDist / 4;
						if (num12 == 22)
						{
							num22 = waterfallDist / 2;
						}
						if (waterfalls[i].stopAtStep > num22)
						{
							waterfalls[i].stopAtStep = num22;
						}
						if (waterfalls[i].stopAtStep == 0 || (float)(num14 + num22) < Main.screenPosition.Y / 16f || (float)num13 < Main.screenPosition.X / 16f - 20f || (float)num13 > (Main.screenPosition.X + (float)Main.screenWidth) / 16f + 20f)
						{
							continue;
						}
						int num23;
						int num24;
						if (num13 % 2 == 0)
						{
							num23 = rainFrameForeground + 3;
							if (num23 > 7)
							{
								num23 -= 8;
							}
							num24 = rainFrameBackground + 2;
							if (num24 > 7)
							{
								num24 -= 8;
							}
							if (num12 == 22)
							{
								num23 = snowFrameForeground + 3;
								if (num23 > 7)
								{
									num23 -= 8;
								}
							}
						}
						else
						{
							num23 = rainFrameForeground;
							num24 = rainFrameBackground;
							if (num12 == 22)
							{
								num23 = snowFrameForeground;
							}
						}
						Rectangle value = new Rectangle(num24 * 18, 0, 16, 16);
						Rectangle value2 = new Rectangle(num23 * 18, 0, 16, 16);
						Vector2 origin = new Vector2(8f, 8f);
						Vector2 position = ((num14 % 2 != 0) ? (new Vector2(num13 * 16 + 8, num14 * 16 + 8) - Main.screenPosition) : (new Vector2(num13 * 16 + 9, num14 * 16 + 8) - Main.screenPosition));
						Tile tile = Main.tile[num13, num14 - 1];
						if (tile.active() && tile.bottomSlope())
						{
							position.Y -= 16f;
						}
						bool flag = false;
						float rotation = 0f;
						for (int j = 0; j < num22; j++)
						{
							Color color = Lighting.GetColor(num13, num14);
							float num25 = 0.6f;
							float num26 = 0.3f;
							if (j > num22 - 8)
							{
								float num27 = (float)(num22 - j) / 8f;
								num25 *= num27;
								num26 *= num27;
							}
							Color color2 = color * num25;
							Color color3 = color * num26;
							if (num12 == 22)
							{
								Main.spriteBatch.Draw(waterfallTexture[22].get_Value(), position, value2, color2, 0f, origin, 1f, SpriteEffects.None, 0f);
							}
							else
							{
								Main.spriteBatch.Draw(waterfallTexture[12].get_Value(), position, value, color3, rotation, origin, 1f, SpriteEffects.None, 0f);
								Main.spriteBatch.Draw(waterfallTexture[11].get_Value(), position, value2, color2, rotation, origin, 1f, SpriteEffects.None, 0f);
							}
							if (flag)
							{
								break;
							}
							num14++;
							Tile tile2 = Main.tile[num13, num14];
							if (WorldGen.SolidTile(tile2))
							{
								flag = true;
							}
							if (tile2.liquid > 0)
							{
								int num28 = (int)(16f * ((float)(int)tile2.liquid / 255f)) & 0xFE;
								if (num28 >= 15)
								{
									break;
								}
								value2.Height -= num28;
								value.Height -= num28;
							}
							if (num14 % 2 == 0)
							{
								position.X += 1f;
							}
							else
							{
								position.X -= 1f;
							}
							position.Y += 16f;
						}
						waterfalls[i].stopAtStep = 0;
						continue;
					}
					case 0:
						num12 = Style;
						break;
					case 2:
						if (Main.drewLava)
						{
							continue;
						}
						break;
					}
					num21 = 32 * regularFrame;
				}
				int num29 = 0;
				num22 = waterfallDist;
				Color color4 = Color.White;
				for (int k = 0; k < num22; k++)
				{
					if (num29 >= 2)
					{
						break;
					}
					AddLight(num12, num13, num14);
					Tile tile3 = Main.tile[num13, num14];
					if (tile3 == null)
					{
						tile3 = new Tile();
						Main.tile[num13, num14] = tile3;
					}
					if (tile3.nactive() && Main.tileSolid[tile3.type] && !Main.tileSolidTop[tile3.type] && !TileID.Sets.Platforms[tile3.type] && tile3.blockType() == 0)
					{
						break;
					}
					Tile tile4 = Main.tile[num13 - 1, num14];
					if (tile4 == null)
					{
						tile4 = new Tile();
						Main.tile[num13 - 1, num14] = tile4;
					}
					Tile tile5 = Main.tile[num13, num14 + 1];
					if (tile5 == null)
					{
						tile5 = new Tile();
						Main.tile[num13, num14 + 1] = tile5;
					}
					Tile tile6 = Main.tile[num13 + 1, num14];
					if (tile6 == null)
					{
						tile6 = new Tile();
						Main.tile[num13 + 1, num14] = tile6;
					}
					if (WorldGen.SolidTile(tile5) && !tile3.halfBrick())
					{
						num11 = 8;
					}
					else if (num16 != 0)
					{
						num11 = 0;
					}
					int num30 = 0;
					int num31 = num18;
					int num32 = 0;
					int num33 = 0;
					if (tile5.topSlope() && !tile3.halfBrick() && tile5.type != 19)
					{
						if (tile5.slope() == 1)
						{
							num30 = 1;
							num32 = 1;
							num17 = 1;
							num18 = num17;
						}
						else
						{
							num30 = -1;
							num32 = -1;
							num17 = -1;
							num18 = num17;
						}
						num33 = 1;
					}
					else if ((!WorldGen.SolidTile(tile5) && !tile5.bottomSlope() && !tile3.halfBrick()) || (!tile5.active() && !tile3.halfBrick()))
					{
						num29 = 0;
						num33 = 1;
						num32 = 0;
					}
					else if ((WorldGen.SolidTile(tile4) || tile4.topSlope() || tile4.liquid > 0) && !WorldGen.SolidTile(tile6) && tile6.liquid == 0)
					{
						if (num17 == -1)
						{
							num29++;
						}
						num32 = 1;
						num33 = 0;
						num17 = 1;
					}
					else if ((WorldGen.SolidTile(tile6) || tile6.topSlope() || tile6.liquid > 0) && !WorldGen.SolidTile(tile4) && tile4.liquid == 0)
					{
						if (num17 == 1)
						{
							num29++;
						}
						num32 = -1;
						num33 = 0;
						num17 = -1;
					}
					else if (((!WorldGen.SolidTile(tile6) && !tile3.topSlope()) || tile6.liquid == 0) && !WorldGen.SolidTile(tile4) && !tile3.topSlope() && tile4.liquid == 0)
					{
						num33 = 0;
						num32 = num17;
					}
					else
					{
						num29++;
						num33 = 0;
						num32 = 0;
					}
					if (num29 >= 2)
					{
						num17 *= -1;
						num32 *= -1;
					}
					int num34 = -1;
					if (num12 != 1 && num12 != 14 && num12 != 25)
					{
						if (tile5.active())
						{
							num34 = tile5.type;
						}
						if (tile3.active())
						{
							num34 = tile3.type;
						}
					}
					switch (num34)
					{
					case 160:
						num12 = 2;
						break;
					case 262:
					case 263:
					case 264:
					case 265:
					case 266:
					case 267:
					case 268:
						num12 = 15 + num34 - 262;
						break;
					}
					Color color5 = Lighting.GetColor(num13, num14);
					if (k > 50)
					{
						TrySparkling(num13, num14, num17, color5);
					}
					float alpha = GetAlpha(Alpha, num22, num12, num14, k, tile3);
					color5 = StylizeColor(alpha, num22, num12, num14, k, tile3, color5);
					if (num12 == 1)
					{
						float num35 = Math.Abs((float)(num13 * 16 + 8) - (Main.screenPosition.X + (float)(Main.screenWidth / 2)));
						float num36 = Math.Abs((float)(num14 * 16 + 8) - (Main.screenPosition.Y + (float)(Main.screenHeight / 2)));
						if (num35 < (float)(Main.screenWidth * 2) && num36 < (float)(Main.screenHeight * 2))
						{
							float num37 = (float)Math.Sqrt(num35 * num35 + num36 * num36);
							float num38 = 1f - num37 / ((float)Main.screenWidth * 0.75f);
							if (num38 > 0f)
							{
								num6 += num38;
							}
						}
						if (num35 < num7)
						{
							num7 = num35;
							num9 = num13 * 16 + 8;
						}
						if (num36 < num8)
						{
							num8 = num35;
							num10 = num14 * 16 + 8;
						}
					}
					else if (num12 != 1 && num12 != 14 && num12 != 25 && num12 != 11 && num12 != 12 && num12 != 22)
					{
						float num39 = Math.Abs((float)(num13 * 16 + 8) - (Main.screenPosition.X + (float)(Main.screenWidth / 2)));
						float num40 = Math.Abs((float)(num14 * 16 + 8) - (Main.screenPosition.Y + (float)(Main.screenHeight / 2)));
						if (num39 < (float)(Main.screenWidth * 2) && num40 < (float)(Main.screenHeight * 2))
						{
							float num41 = (float)Math.Sqrt(num39 * num39 + num40 * num40);
							float num42 = 1f - num41 / ((float)Main.screenWidth * 0.75f);
							if (num42 > 0f)
							{
								num += num42;
							}
						}
						if (num39 < num2)
						{
							num2 = num39;
							num4 = num13 * 16 + 8;
						}
						if (num40 < num3)
						{
							num3 = num39;
							num5 = num14 * 16 + 8;
						}
					}
					int num43 = (int)tile3.liquid / 16;
					if (num15 == 0 && num30 != 0 && num16 == 1 && num17 != num18)
					{
						num30 = 0;
						num17 = num18;
						color5 = Color.White;
						if (num17 == 1)
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16 + 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.FlipHorizontally);
						}
						else
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16 + 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color5, SpriteEffects.FlipHorizontally);
						}
					}
					if (num19 != 0 && num32 == 0 && num33 == 1)
					{
						if (num17 == 1)
						{
							if (num20 != num12)
							{
								DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11 + 8) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43 - 8), color4, SpriteEffects.FlipHorizontally);
							}
							else
							{
								DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11 + 8) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43 - 8), color5, SpriteEffects.FlipHorizontally);
							}
						}
						else
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11 + 8) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43 - 8), color5, SpriteEffects.None);
						}
					}
					if (num11 == 8 && num16 == 1 && num19 == 0)
					{
						if (num18 == -1)
						{
							if (num20 != num12)
							{
								DrawWaterfall(num20, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color4, SpriteEffects.None);
							}
							else
							{
								DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color5, SpriteEffects.None);
							}
						}
						else if (num20 != num12)
						{
							DrawWaterfall(num20, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color4, SpriteEffects.FlipHorizontally);
						}
						else
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 8), color5, SpriteEffects.FlipHorizontally);
						}
					}
					if (num30 != 0 && num15 == 0)
					{
						if (num31 == 1)
						{
							if (num20 != num12)
							{
								DrawWaterfall(num20, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color4, SpriteEffects.FlipHorizontally);
							}
							else
							{
								DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.FlipHorizontally);
							}
						}
						else if (num20 != num12)
						{
							DrawWaterfall(num20, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color4, SpriteEffects.None);
						}
						else
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.None);
						}
					}
					if (num33 == 1 && num30 == 0 && num19 == 0)
					{
						if (num17 == -1)
						{
							if (num16 == 0)
							{
								DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43), color5, SpriteEffects.None);
							}
							else if (num20 != num12)
							{
								DrawWaterfall(num20, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color4, SpriteEffects.None);
							}
							else
							{
								DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.None);
							}
						}
						else if (num16 == 0)
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11) - Main.screenPosition, new Rectangle(num21, 0, 16, 16 - num43), color5, SpriteEffects.FlipHorizontally);
						}
						else if (num20 != num12)
						{
							DrawWaterfall(num20, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color4, SpriteEffects.FlipHorizontally);
						}
						else
						{
							DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16 - 16, num14 * 16) - Main.screenPosition, new Rectangle(num21, 24, 32, 16 - num43), color5, SpriteEffects.FlipHorizontally);
						}
					}
					else
					{
						switch (num32)
						{
						case 1:
							if (Main.tile[num13, num14].liquid > 0 && !Main.tile[num13, num14].halfBrick())
							{
								break;
							}
							if (num30 == 1)
							{
								for (int m = 0; m < 8; m++)
								{
									int num47 = m * 2;
									int num48 = 14 - m * 2;
									int num49 = num47;
									num11 = 8;
									if (num15 == 0 && m < 2)
									{
										num49 = 4;
									}
									DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16 + num47, num14 * 16 + num11 + num49) - Main.screenPosition, new Rectangle(16 + num21 + num48, 0, 2, 16 - num11), color5, SpriteEffects.FlipHorizontally);
								}
							}
							else
							{
								int height2 = 16;
								if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14].type])
								{
									height2 = 8;
								}
								else if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14 + 1].type])
								{
									height2 = 8;
								}
								DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11) - Main.screenPosition, new Rectangle(16 + num21, 0, 16, height2), color5, SpriteEffects.FlipHorizontally);
							}
							break;
						case -1:
							if (Main.tile[num13, num14].liquid > 0 && !Main.tile[num13, num14].halfBrick())
							{
								break;
							}
							if (num30 == -1)
							{
								for (int l = 0; l < 8; l++)
								{
									int num44 = l * 2;
									int num45 = l * 2;
									int num46 = 14 - l * 2;
									num11 = 8;
									if (num15 == 0 && l > 5)
									{
										num46 = 4;
									}
									DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16 + num44, num14 * 16 + num11 + num46) - Main.screenPosition, new Rectangle(16 + num21 + num45, 0, 2, 16 - num11), color5, SpriteEffects.FlipHorizontally);
								}
							}
							else
							{
								int height = 16;
								if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14].type])
								{
									height = 8;
								}
								else if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num13, num14 + 1].type])
								{
									height = 8;
								}
								DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11) - Main.screenPosition, new Rectangle(16 + num21, 0, 16, height), color5, SpriteEffects.None);
							}
							break;
						case 0:
							if (num33 == 0)
							{
								if (Main.tile[num13, num14].liquid <= 0 || Main.tile[num13, num14].halfBrick())
								{
									DrawWaterfall(num12, num13, num14, alpha, new Vector2(num13 * 16, num14 * 16 + num11) - Main.screenPosition, new Rectangle(16 + num21, 0, 16, 16), color5, SpriteEffects.None);
								}
								k = 1000;
							}
							break;
						}
					}
					if (tile3.liquid > 0 && !tile3.halfBrick())
					{
						k = 1000;
					}
					num16 = num33;
					num18 = num17;
					num15 = num32;
					num13 += num32;
					num14 += num33;
					num19 = num30;
					color4 = color5;
					if (num20 != num12)
					{
						num20 = num12;
					}
					if ((tile4.active() && (tile4.type == 189 || tile4.type == 196)) || (tile6.active() && (tile6.type == 189 || tile6.type == 196)) || (tile5.active() && (tile5.type == 189 || tile5.type == 196)))
					{
						num22 = (int)(40f * ((float)Main.maxTilesX / 4200f) * Main.gfxQuality);
					}
				}
			}
			Main.ambientWaterfallX = num4;
			Main.ambientWaterfallY = num5;
			Main.ambientWaterfallStrength = num;
			Main.ambientLavafallX = num9;
			Main.ambientLavafallY = num10;
			Main.ambientLavafallStrength = num6;
			Main.tileSolid[546] = true;
		}

		private void DrawWaterfall(int waterfallType, int x, int y, float opacity, Vector2 position, Rectangle sourceRect, Color color, SpriteEffects effects)
		{
			Texture2D value = waterfallTexture[waterfallType].get_Value();
			if (waterfallType == 25)
			{
				Lighting.GetCornerColors(x, y, out var vertices);
				LiquidRenderer.SetShimmerVertexColors(ref vertices, opacity, x, y);
				Main.tileBatch.Draw(value, position + new Vector2(0f, 0f), sourceRect, vertices, default(Vector2), 1f, effects);
				sourceRect.Y += 42;
				LiquidRenderer.SetShimmerVertexColors_Sparkle(ref vertices, opacity, x, y, top: true);
				Main.tileBatch.Draw(value, position + new Vector2(0f, 0f), sourceRect, vertices, default(Vector2), 1f, effects);
			}
			else
			{
				Main.spriteBatch.Draw(value, position, sourceRect, color, 0f, default(Vector2), 1f, effects, 0f);
			}
		}

		private static Color StylizeColor(float alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache, Color aColor)
		{
			float num = (float)(int)aColor.R * alpha;
			float num2 = (float)(int)aColor.G * alpha;
			float num3 = (float)(int)aColor.B * alpha;
			float num4 = (float)(int)aColor.A * alpha;
			switch (waterfallType)
			{
			case 1:
				if (num < 190f * alpha)
				{
					num = 190f * alpha;
				}
				if (num2 < 190f * alpha)
				{
					num2 = 190f * alpha;
				}
				if (num3 < 190f * alpha)
				{
					num3 = 190f * alpha;
				}
				break;
			case 2:
				num = (float)Main.DiscoR * alpha;
				num2 = (float)Main.DiscoG * alpha;
				num3 = (float)Main.DiscoB * alpha;
				break;
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
				num = 255f * alpha;
				num2 = 255f * alpha;
				num3 = 255f * alpha;
				break;
			}
			aColor = new Color((int)num, (int)num2, (int)num3, (int)num4);
			return aColor;
		}

		private static float GetAlpha(float Alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache)
		{
			float num = waterfallType switch
			{
				1 => 1f, 
				14 => 0.8f, 
				25 => 0.75f, 
				_ => (tileCache.wall != 0 || !((double)y < Main.worldSurface)) ? (0.6f * Alpha) : Alpha, 
			};
			if (s > maxSteps - 10)
			{
				num *= (float)(maxSteps - s) / 10f;
			}
			return num;
		}

		private static void TrySparkling(int x, int y, int direction, Color aColor2)
		{
			if (aColor2.R > 20 || aColor2.B > 20 || aColor2.G > 20)
			{
				float num = (int)aColor2.R;
				if ((float)(int)aColor2.G > num)
				{
					num = (int)aColor2.G;
				}
				if ((float)(int)aColor2.B > num)
				{
					num = (int)aColor2.B;
				}
				if ((float)Main.rand.Next(20000) < num / 30f)
				{
					int num2 = Dust.NewDust(new Vector2(x * 16 - direction * 7, y * 16 + 6), 10, 8, 43, 0f, 0f, 254, Color.White, 0.5f);
					Main.dust[num2].velocity *= 0f;
				}
			}
		}

		private static void AddLight(int waterfallType, int x, int y)
		{
			switch (waterfallType)
			{
			case 1:
			{
				float r;
				float num3 = (r = (0.55f + (float)(270 - Main.mouseTextColor) / 900f) * 0.4f);
				float g = num3 * 0.3f;
				float b = num3 * 0.1f;
				Lighting.AddLight(x, y, r, g, b);
				break;
			}
			case 2:
			{
				float r = (float)Main.DiscoR / 255f;
				float g = (float)Main.DiscoG / 255f;
				float b = (float)Main.DiscoB / 255f;
				r *= 0.2f;
				g *= 0.2f;
				b *= 0.2f;
				Lighting.AddLight(x, y, r, g, b);
				break;
			}
			case 15:
			{
				float r = 0f;
				float g = 0f;
				float b = 0.2f;
				Lighting.AddLight(x, y, r, g, b);
				break;
			}
			case 16:
			{
				float r = 0f;
				float g = 0.2f;
				float b = 0f;
				Lighting.AddLight(x, y, r, g, b);
				break;
			}
			case 17:
			{
				float r = 0f;
				float g = 0f;
				float b = 0.2f;
				Lighting.AddLight(x, y, r, g, b);
				break;
			}
			case 18:
			{
				float r = 0f;
				float g = 0.2f;
				float b = 0f;
				Lighting.AddLight(x, y, r, g, b);
				break;
			}
			case 19:
			{
				float r = 0.2f;
				float g = 0f;
				float b = 0f;
				Lighting.AddLight(x, y, r, g, b);
				break;
			}
			case 20:
				Lighting.AddLight(x, y, 0.2f, 0.2f, 0.2f);
				break;
			case 21:
			{
				float r = 0.2f;
				float g = 0f;
				float b = 0f;
				Lighting.AddLight(x, y, r, g, b);
				break;
			}
			case 25:
			{
				float num = 0.7f;
				float num2 = 0.7f;
				num += (float)(270 - Main.mouseTextColor) / 900f;
				num2 += (float)(270 - Main.mouseTextColor) / 125f;
				Lighting.AddLight(x, y, num * 0.6f, num2 * 0.25f, num * 0.9f);
				break;
			}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < currentMax; i++)
			{
				waterfalls[i].stopAtStep = waterfallDist;
			}
			Main.drewLava = false;
			if (Main.liquidAlpha[0] > 0f)
			{
				DrawWaterfall(0, Main.liquidAlpha[0]);
			}
			if (Main.liquidAlpha[2] > 0f)
			{
				DrawWaterfall(3, Main.liquidAlpha[2]);
			}
			if (Main.liquidAlpha[3] > 0f)
			{
				DrawWaterfall(4, Main.liquidAlpha[3]);
			}
			if (Main.liquidAlpha[4] > 0f)
			{
				DrawWaterfall(5, Main.liquidAlpha[4]);
			}
			if (Main.liquidAlpha[5] > 0f)
			{
				DrawWaterfall(6, Main.liquidAlpha[5]);
			}
			if (Main.liquidAlpha[6] > 0f)
			{
				DrawWaterfall(7, Main.liquidAlpha[6]);
			}
			if (Main.liquidAlpha[7] > 0f)
			{
				DrawWaterfall(8, Main.liquidAlpha[7]);
			}
			if (Main.liquidAlpha[8] > 0f)
			{
				DrawWaterfall(9, Main.liquidAlpha[8]);
			}
			if (Main.liquidAlpha[9] > 0f)
			{
				DrawWaterfall(10, Main.liquidAlpha[9]);
			}
			if (Main.liquidAlpha[10] > 0f)
			{
				DrawWaterfall(13, Main.liquidAlpha[10]);
			}
			if (Main.liquidAlpha[12] > 0f)
			{
				DrawWaterfall(23, Main.liquidAlpha[12]);
			}
			if (Main.liquidAlpha[13] > 0f)
			{
				DrawWaterfall(24, Main.liquidAlpha[13]);
			}
		}
	}
}
