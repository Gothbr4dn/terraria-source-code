using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;

namespace Terraria
{
	public class Rain
	{
		public Vector2 position;

		public Vector2 velocity;

		public float scale;

		public float rotation;

		public int alpha;

		public bool active;

		public byte type;

		public static void ClearRain()
		{
			for (int i = 0; i < Main.maxRain; i++)
			{
				Main.rain[i].active = false;
			}
		}

		public static void MakeRain()
		{
			if (Main.shimmerAlpha > 0f || Main.netMode == 2 || Main.gamePaused)
			{
				return;
			}
			if (Main.remixWorld)
			{
				if (!((double)(Main.player[Main.myPlayer].position.Y / 16f) > Main.rockLayer) || !(Main.player[Main.myPlayer].position.Y / 16f < (float)(Main.maxTilesY - 350)) || Main.player[Main.myPlayer].ZoneDungeon)
				{
					return;
				}
			}
			else if ((double)Main.screenPosition.Y > Main.worldSurface * 16.0)
			{
				return;
			}
			if (Main.gameMenu)
			{
				return;
			}
			float num = (float)Main.screenWidth / 1920f;
			num *= 25f;
			num *= 0.25f + 1f * Main.cloudAlpha;
			if (Filters.Scene["Sandstorm"].IsActive())
			{
				return;
			}
			Vector2 vector = default(Vector2);
			for (int i = 0; (float)i < num; i++)
			{
				int num2 = 600;
				if (Main.player[Main.myPlayer].velocity.Y < 0f)
				{
					num2 += (int)(Math.Abs(Main.player[Main.myPlayer].velocity.Y) * 30f);
				}
				vector.X = Main.rand.Next((int)Main.screenPosition.X - num2, (int)Main.screenPosition.X + Main.screenWidth + num2);
				vector.Y = Main.screenPosition.Y - (float)Main.rand.Next(20, 100);
				vector.X -= Main.windSpeedCurrent * 15f * 40f;
				vector.X += Main.player[Main.myPlayer].velocity.X * 40f;
				if (vector.X < 0f)
				{
					vector.X = 0f;
				}
				if (vector.X > (float)((Main.maxTilesX - 1) * 16))
				{
					vector.X = (Main.maxTilesX - 1) * 16;
				}
				int num3 = (int)vector.X / 16;
				int num4 = (int)vector.Y / 16;
				if (num3 < 0)
				{
					num3 = 0;
				}
				if (num3 > Main.maxTilesX - 1)
				{
					num3 = Main.maxTilesX - 1;
				}
				if (num4 < 0)
				{
					num4 = 0;
				}
				if (num4 > Main.maxTilesY - 1)
				{
					num4 = Main.maxTilesY - 1;
				}
				if (Main.remixWorld || Main.gameMenu || (!WorldGen.SolidTile(num3, num4) && Main.tile[num3, num4].wall <= 0))
				{
					Vector2 rainFallVelocity = GetRainFallVelocity();
					NewRain(vector, rainFallVelocity);
				}
			}
		}

		public static Vector2 GetRainFallVelocity()
		{
			return new Vector2(Main.windSpeedCurrent * 18f, 14f);
		}

		public void Update()
		{
			if (Main.gamePaused)
			{
				return;
			}
			position += velocity;
			if (Main.gameMenu)
			{
				if (position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 2000f)
				{
					active = false;
				}
			}
			else if (Main.remixWorld)
			{
				if (position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 100f)
				{
					active = false;
				}
			}
			else if (Collision.SolidCollision(position, 2, 2) || position.Y > Main.screenPosition.Y + (float)Main.screenHeight + 100f || Collision.WetCollision(position, 2, 2))
			{
				active = false;
				if ((float)Main.rand.Next(100) < Main.gfxQuality * 100f)
				{
					int num = Dust.NewDust(position - velocity, 2, 2, Dust.dustWater());
					Main.dust[num].position.X -= 2f;
					Main.dust[num].position.Y += 2f;
					Main.dust[num].alpha = 38;
					Main.dust[num].velocity *= 0.1f;
					Main.dust[num].velocity += -velocity * 0.025f;
					Main.dust[num].velocity.Y -= 2f;
					Main.dust[num].scale = 0.6f;
					Main.dust[num].noGravity = true;
				}
			}
		}

		public static int NewRainForced(Vector2 Position, Vector2 Velocity)
		{
			int num = -1;
			int num2 = Main.maxRain;
			float num3 = (1f + Main.gfxQuality) / 2f;
			if (num3 < 0.9f)
			{
				num2 = (int)((float)num2 * num3);
			}
			for (int i = 0; i < num2; i++)
			{
				if (!Main.rain[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return Main.maxRain;
			}
			Rain rain = Main.rain[num];
			rain.active = true;
			rain.position = Position;
			rain.scale = 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
			rain.velocity = Velocity * rain.scale;
			rain.rotation = (float)Math.Atan2(rain.velocity.X, 0f - rain.velocity.Y);
			rain.type = (byte)(Main.waterStyle * 3 + Main.rand.Next(3));
			return num;
		}

		private static int NewRain(Vector2 Position, Vector2 Velocity)
		{
			int num = -1;
			int num2 = (int)((float)Main.maxRain * Main.cloudAlpha);
			if (num2 > Main.maxRain)
			{
				num2 = Main.maxRain;
			}
			float num3 = (float)Main.maxTilesX / 6400f;
			Math.Max(0f, Math.Min(1f, (Main.player[Main.myPlayer].position.Y / 16f - 85f * num3) / (60f * num3)));
			float num4 = (1f + Main.gfxQuality) / 2f;
			if ((double)num4 < 0.9)
			{
				num2 = (int)((float)num2 * num4);
			}
			float num5 = 800 - Main.SceneMetrics.SnowTileCount;
			if (num5 < 0f)
			{
				num5 = 0f;
			}
			num5 /= 800f;
			num2 = (int)((float)num2 * num5);
			num2 = (int)((double)num2 * Math.Pow(Main.atmo, 9.0));
			if ((double)Main.atmo < 0.4)
			{
				num2 = 0;
			}
			for (int i = 0; i < num2; i++)
			{
				if (!Main.rain[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return Main.maxRain;
			}
			Rain rain = Main.rain[num];
			rain.active = true;
			rain.position = Position;
			rain.scale = 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
			rain.velocity = Velocity * rain.scale;
			rain.rotation = (float)Math.Atan2(rain.velocity.X, 0f - rain.velocity.Y);
			rain.type = (byte)(Main.waterStyle * 3 + Main.rand.Next(3));
			return num;
		}
	}
}
