using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria
{
	public class Gore
	{
		public static int goreTime = 600;

		public Vector2 position;

		public Vector2 velocity;

		public float rotation;

		public float scale;

		public int alpha;

		public int type;

		public float light;

		public bool active;

		public bool sticky = true;

		public int timeLeft = goreTime;

		public bool behindTiles;

		public byte frameCounter;

		public SpriteFrame Frame = new SpriteFrame(1, 1);

		public float Width
		{
			get
			{
				if (TextureAssets.Gore[type].get_IsLoaded())
				{
					return scale * (float)Frame.GetSourceRectangle(TextureAssets.Gore[type].get_Value()).Width;
				}
				return 1f;
			}
		}

		public float Height
		{
			get
			{
				if (TextureAssets.Gore[type].get_IsLoaded())
				{
					return scale * (float)Frame.GetSourceRectangle(TextureAssets.Gore[type].get_Value()).Height;
				}
				return 1f;
			}
		}

		public Rectangle AABBRectangle
		{
			get
			{
				if (TextureAssets.Gore[type].get_IsLoaded())
				{
					Rectangle sourceRectangle = Frame.GetSourceRectangle(TextureAssets.Gore[type].get_Value());
					return new Rectangle((int)position.X, (int)position.Y, (int)((float)sourceRectangle.Width * scale), (int)((float)sourceRectangle.Height * scale));
				}
				return new Rectangle(0, 0, 1, 1);
			}
		}

		[Old("Please use Frame instead.")]
		public byte frame
		{
			get
			{
				return Frame.CurrentRow;
			}
			set
			{
				Frame.CurrentRow = value;
			}
		}

		[Old("Please use Frame instead.")]
		public byte numFrames
		{
			get
			{
				return Frame.RowCount;
			}
			set
			{
				SpriteFrame spriteFrame = new SpriteFrame(Frame.ColumnCount, value);
				spriteFrame.CurrentColumn = Frame.CurrentColumn;
				spriteFrame.CurrentRow = Frame.CurrentRow;
				SpriteFrame spriteFrame2 = (Frame = spriteFrame);
			}
		}

		private void UpdateAmbientFloorCloud()
		{
			timeLeft -= GoreID.Sets.DisappearSpeed[type];
			if (timeLeft <= 0)
			{
				active = false;
				return;
			}
			bool flag = false;
			Point point = (position + new Vector2(15f, 0f)).ToTileCoordinates();
			Tile tile = Main.tile[point.X, point.Y];
			Tile tile2 = Main.tile[point.X, point.Y + 1];
			Tile tile3 = Main.tile[point.X, point.Y + 2];
			if (tile == null || tile2 == null || tile3 == null)
			{
				active = false;
				return;
			}
			if (WorldGen.SolidTile(tile) || (!WorldGen.SolidTile(tile2) && !WorldGen.SolidTile(tile3)))
			{
				flag = true;
			}
			if (timeLeft <= 30)
			{
				flag = true;
			}
			velocity.X = 0.4f * Main.WindForVisuals;
			if (!flag)
			{
				if (alpha > 220)
				{
					alpha--;
				}
			}
			else
			{
				alpha++;
				if (alpha >= 255)
				{
					active = false;
					return;
				}
			}
			position += velocity;
		}

		private void UpdateAmbientAirborneCloud()
		{
			timeLeft -= GoreID.Sets.DisappearSpeed[type];
			if (timeLeft <= 0)
			{
				active = false;
				return;
			}
			bool flag = false;
			Point point = (position + new Vector2(15f, 0f)).ToTileCoordinates();
			rotation = velocity.ToRotation();
			Tile tile = Main.tile[point.X, point.Y];
			if (tile == null)
			{
				active = false;
				return;
			}
			if (WorldGen.SolidTile(tile))
			{
				flag = true;
			}
			if (timeLeft <= 60)
			{
				flag = true;
			}
			if (!flag)
			{
				if (alpha > 240 && Main.rand.Next(5) == 0)
				{
					alpha--;
				}
			}
			else
			{
				if (Main.rand.Next(5) == 0)
				{
					alpha++;
				}
				if (alpha >= 255)
				{
					active = false;
					return;
				}
			}
			position += velocity;
		}

		private void UpdateFogMachineCloud()
		{
			timeLeft -= GoreID.Sets.DisappearSpeed[type];
			if (timeLeft <= 0)
			{
				active = false;
				return;
			}
			bool flag = false;
			Point point = (position + new Vector2(15f, 0f)).ToTileCoordinates();
			if (WorldGen.SolidTile(Main.tile[point.X, point.Y]))
			{
				flag = true;
			}
			if (timeLeft <= 240)
			{
				flag = true;
			}
			if (!flag)
			{
				if (alpha > 225 && Main.rand.Next(2) == 0)
				{
					alpha--;
				}
			}
			else
			{
				if (Main.rand.Next(2) == 0)
				{
					alpha++;
				}
				if (alpha >= 255)
				{
					active = false;
					return;
				}
			}
			position += velocity;
		}

		private void UpdateLightningBunnySparks()
		{
			if (frameCounter == 0)
			{
				frameCounter = 1;
				Frame.CurrentRow = (byte)Main.rand.Next(3);
			}
			timeLeft -= GoreID.Sets.DisappearSpeed[type];
			if (timeLeft <= 0)
			{
				active = false;
				return;
			}
			alpha = (int)MathHelper.Lerp(255f, 0f, (float)timeLeft / 15f);
			float num = (255f - (float)alpha) / 255f;
			num *= scale;
			Lighting.AddLight(position + new Vector2(Width / 2f, Height / 2f), num * 0.4f, num, num);
			position += velocity;
		}

		private float ChumFloatingChunk_GetWaterLine(int X, int Y)
		{
			float result = position.Y + Height;
			if (Main.tile[X, Y - 1] == null)
			{
				Main.tile[X, Y - 1] = new Tile();
			}
			if (Main.tile[X, Y] == null)
			{
				Main.tile[X, Y] = new Tile();
			}
			if (Main.tile[X, Y + 1] == null)
			{
				Main.tile[X, Y + 1] = new Tile();
			}
			if (Main.tile[X, Y - 1].liquid > 0)
			{
				result = Y * 16;
				result -= (float)((int)Main.tile[X, Y - 1].liquid / 16);
			}
			else if (Main.tile[X, Y].liquid > 0)
			{
				result = (Y + 1) * 16;
				result -= (float)((int)Main.tile[X, Y].liquid / 16);
			}
			else if (Main.tile[X, Y + 1].liquid > 0)
			{
				result = (Y + 2) * 16;
				result -= (float)((int)Main.tile[X, Y + 1].liquid / 16);
			}
			return result;
		}

		private bool DeactivateIfOutsideOfWorld()
		{
			Point point = position.ToTileCoordinates();
			if (!WorldGen.InWorld(point.X, point.Y))
			{
				active = false;
				return true;
			}
			if (Main.tile[point.X, point.Y] == null)
			{
				active = false;
				return true;
			}
			return false;
		}

		public void Update()
		{
			if (Main.netMode == 2 || !active)
			{
				return;
			}
			if (sticky)
			{
				if (DeactivateIfOutsideOfWorld())
				{
					return;
				}
				float num = velocity.Length();
				if (num > 32f)
				{
					velocity *= 32f / num;
				}
			}
			switch (GoreID.Sets.SpecialAI[type])
			{
			case 4:
				UpdateAmbientFloorCloud();
				return;
			case 5:
				UpdateAmbientAirborneCloud();
				return;
			case 6:
				UpdateFogMachineCloud();
				return;
			case 7:
				UpdateLightningBunnySparks();
				return;
			}
			if ((type == 1217 || type == 1218) && frameCounter == 0)
			{
				frameCounter = 1;
				Frame.CurrentRow = (byte)Main.rand.Next(3);
			}
			bool flag = type >= 1024 && type <= 1026;
			if (type >= 276 && type <= 282)
			{
				velocity.X *= 0.98f;
				velocity.Y *= 0.98f;
				if (velocity.Y < scale)
				{
					velocity.Y += 0.05f;
				}
				if ((double)velocity.Y > 0.1)
				{
					if (velocity.X > 0f)
					{
						rotation += 0.01f;
					}
					else
					{
						rotation -= 0.01f;
					}
				}
			}
			if (type >= 570 && type <= 572)
			{
				scale -= 0.001f;
				if ((double)scale <= 0.01)
				{
					scale = 0.01f;
					timeLeft = 0;
				}
				sticky = false;
				rotation = velocity.X * 0.1f;
			}
			else if ((type >= 706 && type <= 717) || type == 943 || type == 1147 || (type >= 1160 && type <= 1162))
			{
				if (type == 943 || (type >= 1160 && type <= 1162))
				{
					alpha = 0;
				}
				else if ((double)position.Y < Main.worldSurface * 16.0 + 8.0)
				{
					alpha = 0;
				}
				else
				{
					alpha = 100;
				}
				int num2 = 4;
				frameCounter++;
				if (frame <= 4)
				{
					int num3 = (int)(position.X / 16f);
					int num4 = (int)(position.Y / 16f) - 1;
					if (WorldGen.InWorld(num3, num4) && !Main.tile[num3, num4].active())
					{
						active = false;
					}
					if (frame == 0)
					{
						num2 = 24 + Main.rand.Next(256);
					}
					if (frame == 1)
					{
						num2 = 24 + Main.rand.Next(256);
					}
					if (frame == 2)
					{
						num2 = 24 + Main.rand.Next(256);
					}
					if (frame == 3)
					{
						num2 = 24 + Main.rand.Next(96);
					}
					if (frame == 5)
					{
						num2 = 16 + Main.rand.Next(64);
					}
					if (type == 716)
					{
						num2 *= 2;
					}
					if (type == 717)
					{
						num2 *= 4;
					}
					if ((type == 943 || (type >= 1160 && type <= 1162)) && frame < 6)
					{
						num2 = 4;
					}
					if (frameCounter >= num2)
					{
						frameCounter = 0;
						frame++;
						if (frame == 5)
						{
							int num5 = NewGore(position, velocity, type);
							Main.gore[num5].frame = 9;
							Main.gore[num5].velocity *= 0f;
						}
					}
				}
				else if (frame <= 6)
				{
					num2 = 8;
					if (type == 716)
					{
						num2 *= 2;
					}
					if (type == 717)
					{
						num2 *= 3;
					}
					if (frameCounter >= num2)
					{
						frameCounter = 0;
						frame++;
						if (frame == 7)
						{
							active = false;
						}
					}
				}
				else if (frame <= 9)
				{
					num2 = 6;
					if (type == 716)
					{
						num2 = (int)((double)num2 * 1.5);
						velocity.Y += 0.175f;
					}
					else if (type == 717)
					{
						num2 *= 2;
						velocity.Y += 0.15f;
					}
					else if (type == 943)
					{
						num2 = (int)((double)num2 * 1.5);
						velocity.Y += 0.2f;
					}
					else
					{
						velocity.Y += 0.2f;
					}
					if ((double)velocity.Y < 0.5)
					{
						velocity.Y = 0.5f;
					}
					if (velocity.Y > 12f)
					{
						velocity.Y = 12f;
					}
					if (frameCounter >= num2)
					{
						frameCounter = 0;
						frame++;
					}
					if (frame > 9)
					{
						frame = 7;
					}
				}
				else
				{
					if (type == 716)
					{
						num2 *= 2;
					}
					else if (type == 717)
					{
						num2 *= 6;
					}
					velocity.Y += 0.1f;
					if (frameCounter >= num2)
					{
						frameCounter = 0;
						frame++;
					}
					velocity *= 0f;
					if (frame > 14)
					{
						active = false;
					}
				}
			}
			else if (type == 11 || type == 12 || type == 13 || type == 61 || type == 62 || type == 63 || type == 99 || type == 220 || type == 221 || type == 222 || (type >= 375 && type <= 377) || (type >= 435 && type <= 437) || (type >= 861 && type <= 862))
			{
				velocity.Y *= 0.98f;
				velocity.X *= 0.98f;
				scale -= 0.007f;
				if ((double)scale < 0.1)
				{
					scale = 0.1f;
					alpha = 255;
				}
			}
			else if (type == 16 || type == 17)
			{
				velocity.Y *= 0.98f;
				velocity.X *= 0.98f;
				scale -= 0.01f;
				if ((double)scale < 0.1)
				{
					scale = 0.1f;
					alpha = 255;
				}
			}
			else if (type == 1201)
			{
				if (frameCounter == 0)
				{
					frameCounter = 1;
					Frame.CurrentRow = (byte)Main.rand.Next(4);
				}
				scale -= 0.002f;
				if ((double)scale < 0.1)
				{
					scale = 0.1f;
					alpha = 255;
				}
				rotation += velocity.X * 0.1f;
				int num6 = (int)(position.X + 6f) / 16;
				int num7 = (int)(position.Y - 6f) / 16;
				if (Main.tile[num6, num7] == null || Main.tile[num6, num7].liquid <= 0)
				{
					velocity.Y += 0.2f;
					if (velocity.Y < 0f)
					{
						velocity *= 0.92f;
					}
				}
				else
				{
					velocity.Y += 0.005f;
					float num8 = velocity.Length();
					if (num8 > 1f)
					{
						velocity *= 0.1f;
					}
					else if (num8 > 0.1f)
					{
						velocity *= 0.98f;
					}
				}
			}
			else if (type == 1208)
			{
				if (frameCounter == 0)
				{
					frameCounter = 1;
					Frame.CurrentRow = (byte)Main.rand.Next(4);
				}
				Vector2 vector = position + new Vector2(Width, Height) / 2f;
				int num9 = (int)vector.X / 16;
				int num10 = (int)vector.Y / 16;
				bool flag2 = Main.tile[num9, num10] != null && Main.tile[num9, num10].liquid > 0;
				scale -= 0.0005f;
				if ((double)scale < 0.1)
				{
					scale = 0.1f;
					alpha = 255;
				}
				rotation += velocity.X * 0.1f;
				if (flag2)
				{
					velocity.X *= 0.9f;
					int num11 = (int)vector.X / 16;
					int num12 = (int)(vector.Y / 16f);
					_ = position.Y / 16f;
					int num13 = (int)((position.Y + Height) / 16f);
					if (Main.tile[num11, num12] == null)
					{
						Main.tile[num11, num12] = new Tile();
					}
					if (Main.tile[num11, num13] == null)
					{
						Main.tile[num11, num13] = new Tile();
					}
					if (velocity.Y > 0f)
					{
						velocity.Y *= 0.5f;
					}
					num11 = (int)(vector.X / 16f);
					num12 = (int)(vector.Y / 16f);
					float num14 = ChumFloatingChunk_GetWaterLine(num11, num12);
					if (vector.Y > num14)
					{
						velocity.Y -= 0.1f;
						if (velocity.Y < -8f)
						{
							velocity.Y = -8f;
						}
						if (vector.Y + velocity.Y < num14)
						{
							velocity.Y = num14 - vector.Y;
						}
					}
					else
					{
						velocity.Y = num14 - vector.Y;
					}
					bool flag3 = !flag2 && velocity.Length() < 0.8f;
					int maxValue = (flag2 ? 270 : 15);
					if (Main.rand.Next(maxValue) == 0 && !flag3)
					{
						Gore gore = NewGoreDirect(position + Vector2.UnitY * 6f, Vector2.Zero, 1201, scale * 0.7f);
						if (flag2)
						{
							gore.velocity = Vector2.UnitX * Main.rand.NextFloatDirection() * 0.5f + Vector2.UnitY * Main.rand.NextFloat();
						}
						else if (gore.velocity.Y < 0f)
						{
							gore.velocity.Y = 0f - gore.velocity.Y;
						}
					}
				}
				else
				{
					if (velocity.Y == 0f)
					{
						velocity.X *= 0.95f;
					}
					velocity.X *= 0.98f;
					velocity.Y += 0.3f;
					if (velocity.Y > 15.9f)
					{
						velocity.Y = 15.9f;
					}
				}
			}
			else if (type == 331)
			{
				alpha += 5;
				velocity.Y *= 0.95f;
				velocity.X *= 0.95f;
				rotation = velocity.X * 0.1f;
			}
			else if (GoreID.Sets.SpecialAI[type] == 3)
			{
				if (++frameCounter >= 8 && velocity.Y > 0.2f)
				{
					frameCounter = 0;
					int num15 = (int)Frame.CurrentRow / 4;
					if (++Frame.CurrentRow >= 4 + num15 * 4)
					{
						Frame.CurrentRow = (byte)(num15 * 4);
					}
				}
			}
			else if (GoreID.Sets.SpecialAI[type] != 1 && GoreID.Sets.SpecialAI[type] != 2)
			{
				if (type >= 907 && type <= 909)
				{
					rotation = 0f;
					velocity.X *= 0.98f;
					if (velocity.Y > 0f && velocity.Y < 0.001f)
					{
						velocity.Y = -0.5f + Main.rand.NextFloat() * -3f;
					}
					if (velocity.Y > -1f)
					{
						velocity.Y -= 0.1f;
					}
					if (scale < 1f)
					{
						scale += 0.1f;
					}
					if (++frameCounter >= 8)
					{
						frameCounter = 0;
						if (++frame >= 3)
						{
							frame = 0;
						}
					}
				}
				else if (type == 1218)
				{
					if (timeLeft > 8)
					{
						timeLeft = 8;
					}
					velocity.X *= 0.95f;
					if (Math.Abs(velocity.X) <= 0.1f)
					{
						velocity.X = 0f;
					}
					if (alpha < 100 && velocity.Length() > 0f && Main.rand.Next(5) == 0)
					{
						int num16 = 246;
						switch (Frame.CurrentRow)
						{
						case 0:
							num16 = 246;
							break;
						case 1:
							num16 = 245;
							break;
						case 2:
							num16 = 244;
							break;
						}
						int num17 = Dust.NewDust(position + new Vector2(6f, 4f), 4, 4, num16);
						Main.dust[num17].alpha = 255;
						Main.dust[num17].scale = 0.8f;
						Main.dust[num17].velocity = Vector2.Zero;
					}
					velocity.Y += 0.2f;
					rotation = 0f;
				}
				else if (type < 411 || type > 430)
				{
					velocity.Y += 0.2f;
					rotation += velocity.X * 0.05f;
				}
				else if (GoreID.Sets.SpecialAI[type] != 3)
				{
					rotation += velocity.X * 0.1f;
				}
			}
			if (type >= 580 && type <= 582)
			{
				rotation = 0f;
				velocity.X *= 0.95f;
			}
			if (GoreID.Sets.SpecialAI[type] == 2)
			{
				if (timeLeft < 60)
				{
					alpha += Main.rand.Next(1, 7);
				}
				else if (alpha > 100)
				{
					alpha -= Main.rand.Next(1, 4);
				}
				if (alpha < 0)
				{
					alpha = 0;
				}
				if (alpha > 255)
				{
					timeLeft = 0;
				}
				velocity.X = (velocity.X * 50f + Main.WindForVisuals * 2f + (float)Main.rand.Next(-10, 11) * 0.1f) / 51f;
				float num18 = 0f;
				if (velocity.X < 0f)
				{
					num18 = velocity.X * 0.2f;
				}
				velocity.Y = (velocity.Y * 50f + -0.35f + num18 + (float)Main.rand.Next(-10, 11) * 0.2f) / 51f;
				rotation = velocity.X * 0.6f;
				float num19 = -1f;
				if (TextureAssets.Gore[type].get_IsLoaded())
				{
					Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, (int)((float)TextureAssets.Gore[type].Width() * scale), (int)((float)TextureAssets.Gore[type].Height() * scale));
					for (int i = 0; i < 255; i++)
					{
						if (Main.player[i].active && !Main.player[i].dead)
						{
							Rectangle value = new Rectangle((int)Main.player[i].position.X, (int)Main.player[i].position.Y, Main.player[i].width, Main.player[i].height);
							if (rectangle.Intersects(value))
							{
								timeLeft = 0;
								num19 = Main.player[i].velocity.Length();
								break;
							}
						}
					}
				}
				if (timeLeft > 0)
				{
					if (Main.rand.Next(2) == 0)
					{
						timeLeft--;
					}
					if (Main.rand.Next(50) == 0)
					{
						timeLeft -= 5;
					}
					if (Main.rand.Next(100) == 0)
					{
						timeLeft -= 10;
					}
				}
				else
				{
					alpha = 255;
					if (TextureAssets.Gore[type].get_IsLoaded() && num19 != -1f)
					{
						float num20 = (float)TextureAssets.Gore[type].Width() * scale * 0.8f;
						float x = position.X;
						float y = position.Y;
						float num21 = (float)TextureAssets.Gore[type].Width() * scale;
						float num22 = (float)TextureAssets.Gore[type].Height() * scale;
						int num23 = 31;
						for (int j = 0; (float)j < num20; j++)
						{
							int num24 = Dust.NewDust(new Vector2(x, y), (int)num21, (int)num22, num23);
							Main.dust[num24].velocity *= (1f + num19) / 3f;
							Main.dust[num24].noGravity = true;
							Main.dust[num24].alpha = 100;
							Main.dust[num24].scale = scale;
						}
					}
				}
			}
			if (type >= 411 && type <= 430)
			{
				alpha = 50;
				velocity.X = (velocity.X * 50f + Main.WindForVisuals * 2f + (float)Main.rand.Next(-10, 11) * 0.1f) / 51f;
				velocity.Y = (velocity.Y * 50f + -0.25f + (float)Main.rand.Next(-10, 11) * 0.2f) / 51f;
				rotation = velocity.X * 0.3f;
				if (TextureAssets.Gore[type].get_IsLoaded())
				{
					Rectangle rectangle2 = new Rectangle((int)position.X, (int)position.Y, (int)((float)TextureAssets.Gore[type].Width() * scale), (int)((float)TextureAssets.Gore[type].Height() * scale));
					for (int k = 0; k < 255; k++)
					{
						if (Main.player[k].active && !Main.player[k].dead)
						{
							Rectangle value2 = new Rectangle((int)Main.player[k].position.X, (int)Main.player[k].position.Y, Main.player[k].width, Main.player[k].height);
							if (rectangle2.Intersects(value2))
							{
								timeLeft = 0;
							}
						}
					}
					if (Collision.SolidCollision(position, (int)((float)TextureAssets.Gore[type].Width() * scale), (int)((float)TextureAssets.Gore[type].Height() * scale)))
					{
						timeLeft = 0;
					}
				}
				if (timeLeft > 0)
				{
					if (Main.rand.Next(2) == 0)
					{
						timeLeft--;
					}
					if (Main.rand.Next(50) == 0)
					{
						timeLeft -= 5;
					}
					if (Main.rand.Next(100) == 0)
					{
						timeLeft -= 10;
					}
				}
				else
				{
					alpha = 255;
					if (TextureAssets.Gore[type].get_IsLoaded())
					{
						float num25 = (float)TextureAssets.Gore[type].Width() * scale * 0.8f;
						float x2 = position.X;
						float y2 = position.Y;
						float num26 = (float)TextureAssets.Gore[type].Width() * scale;
						float num27 = (float)TextureAssets.Gore[type].Height() * scale;
						int num28 = 176;
						if (type >= 416 && type <= 420)
						{
							num28 = 177;
						}
						if (type >= 421 && type <= 425)
						{
							num28 = 178;
						}
						if (type >= 426 && type <= 430)
						{
							num28 = 179;
						}
						for (int l = 0; (float)l < num25; l++)
						{
							int num29 = Dust.NewDust(new Vector2(x2, y2), (int)num26, (int)num27, num28);
							Main.dust[num29].noGravity = true;
							Main.dust[num29].alpha = 100;
							Main.dust[num29].scale = scale;
						}
					}
				}
			}
			else if (GoreID.Sets.SpecialAI[type] != 3 && GoreID.Sets.SpecialAI[type] != 1)
			{
				if ((type >= 706 && type <= 717) || type == 943 || type == 1147 || (type >= 1160 && type <= 1162))
				{
					if (type == 716)
					{
						float num30 = 1f;
						float num31 = 1f;
						float num32 = 1f;
						float num33 = 0.6f;
						num33 = ((frame == 0) ? (num33 * 0.1f) : ((frame == 1) ? (num33 * 0.2f) : ((frame == 2) ? (num33 * 0.3f) : ((frame == 3) ? (num33 * 0.4f) : ((frame == 4) ? (num33 * 0.5f) : ((frame == 5) ? (num33 * 0.4f) : ((frame == 6) ? (num33 * 0.2f) : ((frame <= 9) ? (num33 * 0.5f) : ((frame == 10) ? (num33 * 0.5f) : ((frame == 11) ? (num33 * 0.4f) : ((frame == 12) ? (num33 * 0.3f) : ((frame == 13) ? (num33 * 0.2f) : ((frame != 14) ? 0f : (num33 * 0.1f))))))))))))));
						num30 = 1f * num33;
						num31 = 0.5f * num33;
						num32 = 0.1f * num33;
						Lighting.AddLight(position + new Vector2(8f, 8f), num30, num31, num32);
					}
					Vector2 vector2 = velocity;
					velocity = Collision.TileCollision(position, velocity, 16, 14);
					if (velocity != vector2)
					{
						if (frame < 10)
						{
							frame = 10;
							frameCounter = 0;
							if (type != 716 && type != 717 && type != 943 && (type < 1160 || type > 1162))
							{
								SoundEngine.PlaySound(39, (int)position.X + 8, (int)position.Y + 8, Main.rand.Next(2));
							}
						}
					}
					else if (Collision.WetCollision(position + velocity, 16, 14))
					{
						if (frame < 10)
						{
							frame = 10;
							frameCounter = 0;
							if (type != 716 && type != 717 && type != 943 && (type < 1160 || type > 1162))
							{
								SoundEngine.PlaySound(39, (int)position.X + 8, (int)position.Y + 8, 2);
							}
							((WaterShaderData)Filters.Scene["WaterDistortion"].GetShader()).QueueRipple(position + new Vector2(8f, 8f));
						}
						int num34 = (int)(position.X + 8f) / 16;
						int num35 = (int)(position.Y + 14f) / 16;
						if (Main.tile[num34, num35] != null && Main.tile[num34, num35].liquid > 0)
						{
							velocity *= 0f;
							position.Y = num35 * 16 - (int)Main.tile[num34, num35].liquid / 16;
						}
					}
				}
				else if (sticky)
				{
					int num36 = 32;
					if (TextureAssets.Gore[type].get_IsLoaded())
					{
						num36 = TextureAssets.Gore[type].Width();
						if (TextureAssets.Gore[type].Height() < num36)
						{
							num36 = TextureAssets.Gore[type].Height();
						}
					}
					if (flag)
					{
						num36 = 4;
					}
					num36 = (int)((float)num36 * 0.9f);
					_ = velocity;
					velocity = Collision.TileCollision(position, velocity, (int)((float)num36 * scale), (int)((float)num36 * scale));
					if (velocity.Y == 0f)
					{
						if (flag)
						{
							velocity.X *= 0.94f;
						}
						else
						{
							velocity.X *= 0.97f;
						}
						if ((double)velocity.X > -0.01 && (double)velocity.X < 0.01)
						{
							velocity.X = 0f;
						}
					}
					if (timeLeft > 0)
					{
						timeLeft -= GoreID.Sets.DisappearSpeed[type];
					}
					else
					{
						alpha += GoreID.Sets.DisappearSpeedAlpha[type];
					}
				}
				else
				{
					alpha += 2 * GoreID.Sets.DisappearSpeedAlpha[type];
				}
			}
			if (type >= 907 && type <= 909)
			{
				int num37 = 32;
				if (TextureAssets.Gore[type].get_IsLoaded())
				{
					num37 = TextureAssets.Gore[type].Width();
					if (TextureAssets.Gore[type].Height() < num37)
					{
						num37 = TextureAssets.Gore[type].Height();
					}
				}
				num37 = (int)((float)num37 * 0.9f);
				Vector4 vector3 = Collision.SlopeCollision(position, velocity, num37, num37, 0f, fall: true);
				position.X = vector3.X;
				position.Y = vector3.Y;
				velocity.X = vector3.Z;
				velocity.Y = vector3.W;
			}
			if (GoreID.Sets.SpecialAI[type] == 1)
			{
				Gore_UpdateSail();
			}
			else if (GoreID.Sets.SpecialAI[type] == 3)
			{
				Gore_UpdateLeaf();
			}
			else
			{
				position += velocity;
			}
			if (alpha >= 255)
			{
				active = false;
			}
			if (light > 0f)
			{
				float num38 = light * scale;
				float num39 = light * scale;
				float num40 = light * scale;
				if (type == 16)
				{
					num40 *= 0.3f;
					num39 *= 0.8f;
				}
				else if (type == 17)
				{
					num39 *= 0.6f;
					num38 *= 0.3f;
				}
				if (TextureAssets.Gore[type].get_IsLoaded())
				{
					Lighting.AddLight((int)((position.X + (float)TextureAssets.Gore[type].Width() * scale / 2f) / 16f), (int)((position.Y + (float)TextureAssets.Gore[type].Height() * scale / 2f) / 16f), num38, num39, num40);
				}
				else
				{
					Lighting.AddLight((int)((position.X + 32f * scale / 2f) / 16f), (int)((position.Y + 32f * scale / 2f) / 16f), num38, num39, num40);
				}
			}
		}

		private void Gore_UpdateLeaf()
		{
			Vector2 vector = position + new Vector2(12f) / 2f - new Vector2(4f) / 2f;
			vector.Y -= 4f;
			Vector2 vector2 = position - vector;
			if (velocity.Y < 0f)
			{
				Vector2 vector3 = new Vector2(velocity.X, -0.2f);
				int num = 4;
				num = (int)((float)num * 0.9f);
				Point point = (new Vector2(num, num) / 2f + vector).ToTileCoordinates();
				if (!WorldGen.InWorld(point.X, point.Y))
				{
					active = false;
					return;
				}
				Tile tile = Main.tile[point.X, point.Y];
				if (tile == null)
				{
					active = false;
					return;
				}
				int num2 = 6;
				Rectangle rectangle = new Rectangle(point.X * 16, point.Y * 16 + (int)tile.liquid / 16, 16, 16 - (int)tile.liquid / 16);
				Rectangle value = new Rectangle((int)vector.X, (int)vector.Y + num2, num, num);
				bool flag = tile != null && tile.liquid > 0 && rectangle.Intersects(value);
				if (flag)
				{
					if (tile.honey())
					{
						vector3.X = 0f;
					}
					else if (tile.lava())
					{
						active = false;
						for (int i = 0; i < 5; i++)
						{
							Dust.NewDust(position, num, num, 31, 0f, -0.2f);
						}
					}
					else
					{
						vector3.X = Main.WindForVisuals;
					}
					if ((double)position.Y > Main.worldSurface * 16.0)
					{
						vector3.X = 0f;
					}
				}
				if (!WorldGen.SolidTile(point.X, point.Y + 1) && !flag)
				{
					velocity.Y = 0.1f;
					timeLeft = 0;
					alpha += 20;
				}
				vector3 = Collision.TileCollision(vector, vector3, num, num);
				if (flag)
				{
					rotation = vector3.ToRotation() + MathF.PI / 2f;
				}
				vector3.X *= 0.94f;
				if (!flag || ((double)vector3.X > -0.01 && (double)vector3.X < 0.01))
				{
					vector3.X = 0f;
				}
				if (timeLeft > 0)
				{
					timeLeft -= GoreID.Sets.DisappearSpeed[type];
				}
				else
				{
					alpha += GoreID.Sets.DisappearSpeedAlpha[type];
				}
				velocity.X = vector3.X;
				position.X += velocity.X;
				return;
			}
			velocity.Y += MathF.PI / 180f;
			Vector2 vector4 = new Vector2(Vector2.UnitY.RotatedBy(velocity.Y).X * 1f, Math.Abs(Vector2.UnitY.RotatedBy(velocity.Y).Y) * 1f);
			int num3 = 4;
			if ((double)position.Y < Main.worldSurface * 16.0)
			{
				vector4.X += Main.WindForVisuals * 4f;
			}
			Vector2 vector5 = vector4;
			vector4 = Collision.TileCollision(vector, vector4, num3, num3);
			Vector4 vector6 = Collision.SlopeCollision(vector, vector4, num3, num3, 1f);
			position.X = vector6.X;
			position.Y = vector6.Y;
			vector4.X = vector6.Z;
			vector4.Y = vector6.W;
			position += vector2;
			if (vector4 != vector5)
			{
				velocity.Y = -1f;
			}
			Point point2 = (new Vector2(Width, Height) * 0.5f + position).ToTileCoordinates();
			if (!WorldGen.InWorld(point2.X, point2.Y))
			{
				active = false;
				return;
			}
			Tile tile2 = Main.tile[point2.X, point2.Y];
			if (tile2 == null)
			{
				active = false;
				return;
			}
			int num4 = 6;
			Rectangle rectangle2 = new Rectangle(point2.X * 16, point2.Y * 16 + (int)tile2.liquid / 16, 16, 16 - (int)tile2.liquid / 16);
			Rectangle value2 = new Rectangle((int)vector.X, (int)vector.Y + num4, num3, num3);
			if (tile2 != null && tile2.liquid > 0 && rectangle2.Intersects(value2))
			{
				velocity.Y = -1f;
			}
			position += vector4;
			rotation = vector4.ToRotation() + MathF.PI / 2f;
			if (timeLeft > 0)
			{
				timeLeft -= GoreID.Sets.DisappearSpeed[type];
			}
			else
			{
				alpha += GoreID.Sets.DisappearSpeedAlpha[type];
			}
		}

		private void Gore_UpdateSail()
		{
			if (velocity.Y < 0f)
			{
				Vector2 vector = new Vector2(velocity.X, 0.6f);
				int num = 32;
				if (TextureAssets.Gore[type].get_IsLoaded())
				{
					num = TextureAssets.Gore[type].Width();
					if (TextureAssets.Gore[type].Height() < num)
					{
						num = TextureAssets.Gore[type].Height();
					}
				}
				num = (int)((float)num * 0.9f);
				vector = Collision.TileCollision(position, vector, (int)((float)num * scale), (int)((float)num * scale));
				vector.X *= 0.97f;
				if ((double)vector.X > -0.01 && (double)vector.X < 0.01)
				{
					vector.X = 0f;
				}
				if (timeLeft > 0)
				{
					timeLeft--;
				}
				else
				{
					alpha++;
				}
				velocity.X = vector.X;
				return;
			}
			velocity.Y += MathF.PI / 60f;
			Vector2 vector2 = new Vector2(Vector2.UnitY.RotatedBy(velocity.Y).X * 2f, Math.Abs(Vector2.UnitY.RotatedBy(velocity.Y).Y) * 3f);
			vector2 *= 2f;
			int num2 = 32;
			if (TextureAssets.Gore[type].get_IsLoaded())
			{
				num2 = TextureAssets.Gore[type].Width();
				if (TextureAssets.Gore[type].Height() < num2)
				{
					num2 = TextureAssets.Gore[type].Height();
				}
			}
			Vector2 vector3 = vector2;
			vector2 = Collision.TileCollision(position, vector2, (int)((float)num2 * scale), (int)((float)num2 * scale));
			if (vector2 != vector3)
			{
				velocity.Y = -1f;
			}
			position += vector2;
			rotation = vector2.ToRotation() + MathF.PI;
			if (timeLeft > 0)
			{
				timeLeft--;
			}
			else
			{
				alpha++;
			}
		}

		public static Gore NewGorePerfect(Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
		{
			Gore gore = NewGoreDirect(Position, Velocity, Type, Scale);
			gore.position = Position;
			gore.velocity = Velocity;
			return gore;
		}

		public static Gore NewGoreDirect(Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
		{
			return Main.gore[NewGore(Position, Velocity, Type, Scale)];
		}

		public static int NewGore(Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
		{
			if (Main.netMode == 2)
			{
				return 600;
			}
			if (Main.gamePaused)
			{
				return 600;
			}
			if (WorldGen.gen)
			{
				return 600;
			}
			if (Main.rand == null)
			{
				Main.rand = new UnifiedRandom();
			}
			if (Type == -1)
			{
				return 600;
			}
			int num = 600;
			for (int i = 0; i < 600; i++)
			{
				if (!Main.gore[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == 600)
			{
				return num;
			}
			Main.gore[num].Frame = new SpriteFrame(1, 1);
			Main.gore[num].frameCounter = 0;
			Main.gore[num].behindTiles = false;
			Main.gore[num].light = 0f;
			Main.gore[num].position = Position;
			Main.gore[num].velocity = Velocity;
			Main.gore[num].velocity.Y -= (float)Main.rand.Next(10, 31) * 0.1f;
			Main.gore[num].velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
			Main.gore[num].type = Type;
			Main.gore[num].active = true;
			Main.gore[num].alpha = 0;
			Main.gore[num].rotation = 0f;
			Main.gore[num].scale = Scale;
			if (!ChildSafety.Disabled && ChildSafety.DangerousGore(Type))
			{
				Main.gore[num].type = Main.rand.Next(11, 14);
				Main.gore[num].scale = Main.rand.NextFloat() * 0.5f + 0.5f;
				Main.gore[num].velocity /= 2f;
			}
			if (goreTime == 0 || Type == 11 || Type == 12 || Type == 13 || Type == 16 || Type == 17 || Type == 61 || Type == 62 || Type == 63 || Type == 99 || Type == 220 || Type == 221 || Type == 222 || Type == 435 || Type == 436 || Type == 437 || (Type >= 861 && Type <= 862))
			{
				Main.gore[num].sticky = false;
			}
			else if (Type >= 375 && Type <= 377)
			{
				Main.gore[num].sticky = false;
				Main.gore[num].alpha = 100;
			}
			else
			{
				Main.gore[num].sticky = true;
				Main.gore[num].timeLeft = goreTime;
			}
			if ((Type >= 706 && Type <= 717) || Type == 943 || Type == 1147 || (Type >= 1160 && Type <= 1162))
			{
				Main.gore[num].numFrames = 15;
				Main.gore[num].behindTiles = true;
				Main.gore[num].timeLeft = goreTime * 3;
			}
			if (Type == 16 || Type == 17)
			{
				Main.gore[num].alpha = 100;
				Main.gore[num].scale = 0.7f;
				Main.gore[num].light = 1f;
			}
			if (Type >= 570 && Type <= 572)
			{
				Main.gore[num].velocity = Velocity;
			}
			if (Type == 1201 || Type == 1208)
			{
				Main.gore[num].Frame = new SpriteFrame(1, 4);
			}
			if (Type == 1217 || Type == 1218)
			{
				Main.gore[num].Frame = new SpriteFrame(1, 3);
			}
			if (Type == 1225)
			{
				Main.gore[num].Frame = new SpriteFrame(1, 3);
				Main.gore[num].timeLeft = 10 + Main.rand.Next(6);
				Main.gore[num].sticky = false;
				if (TextureAssets.Gore[Type].get_IsLoaded())
				{
					Main.gore[num].position.X = Position.X - (float)(TextureAssets.Gore[Type].Width() / 2) * Scale;
					Main.gore[num].position.Y = Position.Y - (float)TextureAssets.Gore[Type].Height() * Scale / 2f;
				}
			}
			int num2 = GoreID.Sets.SpecialAI[Type];
			if (num2 == 3)
			{
				Main.gore[num].velocity = new Vector2((Main.rand.NextFloat() - 0.5f) * 1f, Main.rand.NextFloat() * (MathF.PI * 2f));
				bool flag = (Type >= 910 && Type <= 925) || (Type >= 1113 && Type <= 1121) || (Type >= 1248 && Type <= 1255) || Type == 1257 || Type == 1278;
				SpriteFrame spriteFrame = (Main.gore[num].Frame = new SpriteFrame((byte)((!flag) ? 1u : 32u), 8)
				{
					CurrentRow = (byte)Main.rand.Next(8)
				});
				Main.gore[num].frameCounter = (byte)Main.rand.Next(8);
			}
			if (num2 == 1)
			{
				Main.gore[num].velocity = new Vector2((Main.rand.NextFloat() - 0.5f) * 3f, Main.rand.NextFloat() * (MathF.PI * 2f));
			}
			if (Type >= 411 && Type <= 430 && TextureAssets.Gore[Type].get_IsLoaded())
			{
				Main.gore[num].position.X = Position.X - (float)(TextureAssets.Gore[Type].Width() / 2) * Scale;
				Main.gore[num].position.Y = Position.Y - (float)TextureAssets.Gore[Type].Height() * Scale;
				Main.gore[num].velocity.Y *= (float)Main.rand.Next(90, 150) * 0.01f;
				Main.gore[num].velocity.X *= (float)Main.rand.Next(40, 90) * 0.01f;
				int num3 = Main.rand.Next(4) * 5;
				Main.gore[num].type += num3;
				Main.gore[num].timeLeft = Main.rand.Next(goreTime / 2, goreTime * 2);
				Main.gore[num].sticky = true;
				if (goreTime == 0)
				{
					Main.gore[num].timeLeft = Main.rand.Next(150, 600);
				}
			}
			if (Type >= 907 && Type <= 909)
			{
				Main.gore[num].sticky = true;
				Main.gore[num].numFrames = 3;
				Main.gore[num].frame = (byte)Main.rand.Next(3);
				Main.gore[num].frameCounter = (byte)Main.rand.Next(5);
				Main.gore[num].rotation = 0f;
			}
			if (num2 == 2)
			{
				Main.gore[num].sticky = false;
				if (TextureAssets.Gore[Type].get_IsLoaded())
				{
					Main.gore[num].alpha = 150;
					Main.gore[num].velocity = Velocity;
					Main.gore[num].position.X = Position.X - (float)(TextureAssets.Gore[Type].Width() / 2) * Scale;
					Main.gore[num].position.Y = Position.Y - (float)TextureAssets.Gore[Type].Height() * Scale / 2f;
					Main.gore[num].timeLeft = Main.rand.Next(goreTime / 2, goreTime + 1);
				}
			}
			if (num2 == 4)
			{
				Main.gore[num].alpha = 254;
				Main.gore[num].timeLeft = 300;
			}
			if (num2 == 5)
			{
				Main.gore[num].alpha = 254;
				Main.gore[num].timeLeft = 240;
			}
			if (num2 == 6)
			{
				Main.gore[num].alpha = 254;
				Main.gore[num].timeLeft = 480;
			}
			if (Main.gore[num].DeactivateIfOutsideOfWorld())
			{
				return 600;
			}
			return num;
		}

		public Color GetAlpha(Color newColor)
		{
			float num = (float)(255 - alpha) / 255f;
			int r;
			int g;
			int b;
			if (type == 16 || type == 17)
			{
				r = newColor.R;
				g = newColor.G;
				b = newColor.B;
			}
			else
			{
				if (type == 716)
				{
					return new Color(255, 255, 255, 200);
				}
				if (type >= 570 && type <= 572)
				{
					byte b2 = (byte)(255 - alpha);
					return new Color(b2, b2, b2, (int)b2 / 2);
				}
				if (type == 331)
				{
					return new Color(255, 255, 255, 50);
				}
				if (type == 1225)
				{
					return new Color(num, num, num, num);
				}
				r = (int)((float)(int)newColor.R * num);
				g = (int)((float)(int)newColor.G * num);
				b = (int)((float)(int)newColor.B * num);
			}
			int num2 = newColor.A - alpha;
			if (num2 < 0)
			{
				num2 = 0;
			}
			if (num2 > 255)
			{
				num2 = 255;
			}
			if (type >= 1202 && type <= 1204)
			{
				return new Color(r, g, b, (num2 < 20) ? num2 : 20);
			}
			return new Color(r, g, b, num2);
		}
	}
}
