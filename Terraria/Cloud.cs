using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Utilities;

namespace Terraria
{
	public class Cloud
	{
		public Vector2 position;

		public float scale;

		public float rotation;

		public float rSpeed;

		public float sSpeed;

		public bool active;

		public SpriteEffects spriteDir;

		public int type;

		public int width;

		public int height;

		public float Alpha;

		public bool kill;

		private static UnifiedRandom rand = new UnifiedRandom();

		public static void resetClouds()
		{
			if (!Main.dedServ)
			{
				Main.windSpeedCurrent = Main.windSpeedTarget;
				for (int i = 0; i < 200; i++)
				{
					Main.cloud[i].active = false;
				}
				for (int j = 0; j < Main.numClouds; j++)
				{
					addCloud();
					Main.cloud[j].Alpha = 1f;
				}
				for (int k = 0; k < 200; k++)
				{
					Main.cloud[k].Alpha = 1f;
				}
			}
		}

		public static void addCloud()
		{
			if (Main.netMode == 2)
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < 200; i++)
			{
				if (!Main.cloud[i].active)
				{
					num = i;
					break;
				}
			}
			if (num < 0)
			{
				return;
			}
			Main.cloud[num].kill = false;
			Main.cloud[num].rSpeed = 0f;
			Main.cloud[num].sSpeed = 0f;
			Main.cloud[num].scale = (float)rand.Next(70, 131) * 0.01f;
			Main.cloud[num].rotation = (float)rand.Next(-10, 11) * 0.01f;
			Main.cloud[num].width = (int)((float)TextureAssets.Cloud[Main.cloud[num].type].Width() * Main.cloud[num].scale);
			Main.cloud[num].height = (int)((float)TextureAssets.Cloud[Main.cloud[num].type].Height() * Main.cloud[num].scale);
			Main.cloud[num].Alpha = 0f;
			Main.cloud[num].spriteDir = SpriteEffects.None;
			if (rand.Next(2) == 0)
			{
				Main.cloud[num].spriteDir = SpriteEffects.FlipHorizontally;
			}
			float num2 = Main.windSpeedCurrent;
			if (!Main.gameMenu)
			{
				num2 = Main.windSpeedCurrent - Main.player[Main.myPlayer].velocity.X * 0.1f;
			}
			int num3 = 0;
			int num4 = 0;
			if (num2 > 0f)
			{
				num3 -= 200;
			}
			if (num2 < 0f)
			{
				num4 += 200;
			}
			int num5 = 300;
			float x = rand.Next(num3 - num5, Main.screenWidth + num4 + num5);
			Main.cloud[num].Alpha = 0f;
			Main.cloud[num].position.Y = rand.Next((int)((float)(-Main.screenHeight) * 0.25f), (int)((float)Main.screenHeight * 0.15f));
			if (rand.Next(3) == 0)
			{
				Main.cloud[num].position.Y -= rand.Next((int)((float)Main.screenHeight * 0.1f));
			}
			Main.cloud[num].type = rand.Next(4);
			if ((Main.cloudAlpha > 0f && rand.Next(4) != 0) || (Main.cloudBGActive >= 1f && rand.Next(2) == 0))
			{
				Main.cloud[num].type = rand.Next(18, 22);
				if ((double)Main.cloud[num].scale >= 1.15)
				{
					Main.cloud[num].position.Y -= 150f;
				}
				if (Main.cloud[num].scale >= 1f)
				{
					Main.cloud[num].position.Y -= 150f;
				}
			}
			else if (Main.cloudBGActive <= 0f && Main.cloudAlpha == 0f && Main.cloud[num].scale < 1f && Main.cloud[num].position.Y < (float)(-Main.screenHeight) * 0.15f && (double)Main.numClouds <= 80.0)
			{
				Main.cloud[num].type = rand.Next(9, 14);
			}
			else if ((((double)Main.cloud[num].scale < 1.15 && Main.cloud[num].position.Y < (float)(-Main.screenHeight) * 0.3f) || ((double)Main.cloud[num].scale < 0.85 && Main.cloud[num].position.Y < (float)Main.screenHeight * 0.15f)) && ((double)Main.numClouds > 70.0 || Main.cloudBGActive >= 1f))
			{
				Main.cloud[num].type = rand.Next(4, 9);
			}
			else if (Main.cloud[num].position.Y > (float)(-Main.screenHeight) * 0.15f && rand.Next(2) == 0 && (double)Main.numClouds > 20.0)
			{
				Main.cloud[num].type = rand.Next(14, 18);
			}
			if (rand.Next((Main.dontStarveWorld || Main.tenthAnniversaryWorld) ? 25 : 150) == 0)
			{
				Main.cloud[num].type = RollRareCloud();
			}
			else if (Main.tenthAnniversaryWorld && rand.Next(3) == 0)
			{
				Main.cloud[num].type = RollRareCloud();
			}
			if ((double)Main.cloud[num].scale > 1.2)
			{
				Main.cloud[num].position.Y += 100f;
			}
			if ((double)Main.cloud[num].scale > 1.3)
			{
				Main.cloud[num].scale = 1.3f;
			}
			if ((double)Main.cloud[num].scale < 0.7)
			{
				Main.cloud[num].scale = 0.7f;
			}
			Main.cloud[num].active = true;
			Main.cloud[num].position.X = x;
			if (Main.cloud[num].position.X > (float)(Main.screenWidth + 400))
			{
				Main.cloud[num].Alpha = 1f;
			}
			if (Main.cloud[num].position.X + (float)TextureAssets.Cloud[Main.cloud[num].type].Width() * Main.cloud[num].scale < -400f)
			{
				Main.cloud[num].Alpha = 1f;
			}
			Rectangle rectangle = new Rectangle((int)Main.cloud[num].position.X, (int)Main.cloud[num].position.Y, Main.cloud[num].width, Main.cloud[num].height);
			for (int j = 0; j < 200; j++)
			{
				if (num != j && Main.cloud[j].active)
				{
					Rectangle value = new Rectangle((int)Main.cloud[j].position.X, (int)Main.cloud[j].position.Y, Main.cloud[j].width, Main.cloud[j].height);
					if (rectangle.Intersects(value))
					{
						Main.cloud[num].active = false;
					}
				}
			}
		}

		private static int RollRareCloud()
		{
			int num = -1;
			bool flag = false;
			while (!flag)
			{
				num = ((!Main.tenthAnniversaryWorld) ? rand.Next(22, 41) : rand.Next(22, 37));
				switch (num)
				{
				default:
					flag = true;
					break;
				case 31:
					flag = NPC.downedBoss3;
					break;
				case 36:
					flag = NPC.downedBoss2 && WorldGen.crimson;
					break;
				case 25:
				case 26:
					flag = NPC.downedBoss1;
					break;
				case 30:
				case 35:
					flag = Main.hardMode;
					break;
				case 28:
					if (rand.Next(10) == 0)
					{
						flag = true;
					}
					break;
				case 37:
				case 38:
				case 39:
				case 40:
					if (Main.dontStarveWorld || rand.Next(10) == 0)
					{
						flag = true;
					}
					break;
				}
			}
			return num;
		}

		public Color cloudColor(Color bgColor)
		{
			float num = scale * Alpha;
			if (num > 1f)
			{
				num = 1f;
			}
			float num2 = (int)((float)(int)bgColor.R * num);
			float num3 = (int)((float)(int)bgColor.G * num);
			float num4 = (int)((float)(int)bgColor.B * num);
			float num5 = (int)((float)(int)bgColor.A * num);
			return new Color((byte)num2, (byte)num3, (byte)num4, (byte)num5);
		}

		public object Clone()
		{
			return MemberwiseClone();
		}

		public static void UpdateClouds()
		{
			if (Main.netMode == 2)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < 200; i++)
			{
				if (Main.cloud[i].active)
				{
					Main.cloud[i].Update();
					if (!Main.cloud[i].kill)
					{
						num++;
					}
				}
			}
			for (int j = 0; j < 200; j++)
			{
				if (Main.cloud[j].active)
				{
					if (j > 1 && (!Main.cloud[j - 1].active || (double)Main.cloud[j - 1].scale > (double)Main.cloud[j].scale + 0.02))
					{
						Cloud cloud = (Cloud)Main.cloud[j - 1].Clone();
						Main.cloud[j - 1] = (Cloud)Main.cloud[j].Clone();
						Main.cloud[j] = cloud;
					}
					if (j < 199 && (!Main.cloud[j].active || (double)Main.cloud[j + 1].scale < (double)Main.cloud[j].scale - 0.02))
					{
						Cloud cloud2 = (Cloud)Main.cloud[j + 1].Clone();
						Main.cloud[j + 1] = (Cloud)Main.cloud[j].Clone();
						Main.cloud[j] = cloud2;
					}
				}
			}
			if (num < Main.numClouds)
			{
				addCloud();
			}
			else if (num > Main.numClouds)
			{
				int num2 = rand.Next(num);
				int num3 = 0;
				while (Main.cloud[num2].kill && num3 < 100)
				{
					num3++;
					num2 = rand.Next(num);
				}
				Main.cloud[num2].kill = true;
			}
		}

		public void Update()
		{
			if (WorldGen.drunkWorldGenText && Main.gameMenu)
			{
				type = 28;
			}
			if (scale == 1f)
			{
				scale -= 0.0001f;
			}
			if ((double)scale == 1.15)
			{
				scale -= 0.0001f;
			}
			float num = 0.13f;
			if (scale < 1f)
			{
				num = 0.07f;
				float num2 = scale + 0.15f;
				num2 = (num2 + 1f) / 2f;
				num2 *= num2;
				num *= num2;
			}
			else if ((double)scale <= 1.15)
			{
				num = 0.19f;
				float num3 = scale - 0.075f;
				num3 *= num3;
				num *= num3;
			}
			else
			{
				num = 0.23f;
				float num4 = scale - 0.15f - 0.075f;
				num4 *= num4;
				num *= num4;
			}
			position.X += Main.windSpeedCurrent * 9f * num * (float)Main.dayRate;
			float num5 = Main.screenPosition.X - Main.screenLastPosition.X;
			position.X -= num5 * num;
			float num6 = 600f;
			if (Main.bgAlphaFrontLayer[4] == 1f && position.Y > 200f)
			{
				kill = true;
				Alpha -= 0.005f * (float)Main.dayRate;
			}
			if (!kill)
			{
				if (Alpha < 1f)
				{
					Alpha += 0.001f * (float)Main.dayRate;
					if (Alpha > 1f)
					{
						Alpha = 1f;
					}
				}
			}
			else
			{
				Alpha -= 0.001f * (float)Main.dayRate;
				if (Alpha <= 0f)
				{
					active = false;
				}
			}
			if (position.X + (float)TextureAssets.Cloud[type].Width() * scale < 0f - num6 || position.X > (float)Main.screenWidth + num6)
			{
				active = false;
			}
			rSpeed += (float)rand.Next(-10, 11) * 2E-05f;
			if ((double)rSpeed > 0.0002)
			{
				rSpeed = 0.0002f;
			}
			if ((double)rSpeed < -0.0002)
			{
				rSpeed = -0.0002f;
			}
			if ((double)rotation > 0.02)
			{
				rotation = 0.02f;
			}
			if ((double)rotation < -0.02)
			{
				rotation = -0.02f;
			}
			rotation += rSpeed;
			width = (int)((float)TextureAssets.Cloud[type].Width() * scale);
			height = (int)((float)TextureAssets.Cloud[type].Height() * scale);
			if (type >= 9 && type <= 13 && (Main.cloudAlpha > 0f || Main.cloudBGActive >= 1f))
			{
				kill = true;
			}
		}
	}
}
