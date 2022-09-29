using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Events
{
	public class Sandstorm
	{
		private const int SANDSTORM_DURATION_MINIMUM = 28800;

		private const int SANDSTORM_DURATION_MAXIMUM = 86400;

		public static bool Happening;

		public static int TimeLeft;

		public static float Severity;

		public static float IntendedSeverity;

		private static bool _effectsUp;

		private static bool HasSufficientWind()
		{
			return Math.Abs(Main.windSpeedCurrent) >= 0.6f;
		}

		public static void WorldClear()
		{
			Happening = false;
		}

		public static void UpdateTime()
		{
			if (Main.netMode != 1)
			{
				if (Happening)
				{
					if (TimeLeft > 86400)
					{
						TimeLeft = 0;
					}
					TimeLeft -= Main.dayRate;
					if (!HasSufficientWind())
					{
						TimeLeft -= 15 * Main.dayRate;
					}
					if (Main.windSpeedCurrent == 0f)
					{
						TimeLeft = 0;
					}
					if (TimeLeft <= 0)
					{
						StopSandstorm();
					}
				}
				else
				{
					int num = 21600;
					num = ((!Main.hardMode) ? (num * 3) : (num * 2));
					if (HasSufficientWind())
					{
						for (int i = 0; i < Main.dayRate; i++)
						{
							if (Main.rand.Next(num) == 0)
							{
								StartSandstorm();
							}
						}
					}
				}
				if (Main.rand.Next(18000) == 0)
				{
					ChangeSeverityIntentions();
				}
			}
			UpdateSeverity();
		}

		private static void ChangeSeverityIntentions()
		{
			if (Happening)
			{
				IntendedSeverity = 0.4f + Main.rand.NextFloat();
			}
			else if (Main.rand.Next(3) == 0)
			{
				IntendedSeverity = 0f;
			}
			else
			{
				IntendedSeverity = Main.rand.NextFloat() * 0.3f;
			}
			if (Main.netMode != 1)
			{
				NetMessage.SendData(7);
			}
		}

		private static void UpdateSeverity()
		{
			if (float.IsNaN(Severity))
			{
				Severity = 0f;
			}
			if (float.IsNaN(IntendedSeverity))
			{
				IntendedSeverity = 0f;
			}
			int num = Math.Sign(IntendedSeverity - Severity);
			Severity = MathHelper.Clamp(Severity + 0.003f * (float)num, 0f, 1f);
			int num2 = Math.Sign(IntendedSeverity - Severity);
			if (num != num2)
			{
				Severity = IntendedSeverity;
			}
		}

		private static void StartSandstorm()
		{
			Happening = true;
			TimeLeft = Main.rand.Next(28800, 86401);
			ChangeSeverityIntentions();
		}

		private static void StopSandstorm()
		{
			Happening = false;
			TimeLeft = 0;
			ChangeSeverityIntentions();
		}

		public static void HandleEffectAndSky(bool toState)
		{
			if (toState != _effectsUp)
			{
				_effectsUp = toState;
				Vector2 center = Main.player[Main.myPlayer].Center;
				if (_effectsUp)
				{
					SkyManager.Instance.Activate("Sandstorm", center);
					Filters.Scene.Activate("Sandstorm", center);
					Overlays.Scene.Activate("Sandstorm", center);
				}
				else
				{
					SkyManager.Instance.Deactivate("Sandstorm");
					Filters.Scene.Deactivate("Sandstorm");
					Overlays.Scene.Deactivate("Sandstorm");
				}
			}
		}

		public static bool ShouldSandstormDustPersist()
		{
			if (Happening && Main.player[Main.myPlayer].ZoneSandstorm && (Main.bgStyle == 2 || Main.bgStyle == 5))
			{
				return Main.bgDelay < 50;
			}
			return false;
		}

		public static void EmitDust()
		{
			if (Main.gamePaused)
			{
				return;
			}
			int sandTileCount = Main.SceneMetrics.SandTileCount;
			Player player = Main.player[Main.myPlayer];
			bool flag = ShouldSandstormDustPersist();
			HandleEffectAndSky(flag && Main.UseStormEffects);
			if (sandTileCount < 100 || (double)player.position.Y > Main.worldSurface * 16.0 || player.ZoneBeach)
			{
				return;
			}
			int maxValue = 1;
			if (!flag || Main.rand.Next(maxValue) != 0)
			{
				return;
			}
			int num = Math.Sign(Main.windSpeedCurrent);
			float num2 = Math.Abs(Main.windSpeedCurrent);
			if (num2 < 0.01f)
			{
				return;
			}
			float num3 = (float)num * MathHelper.Lerp(0.9f, 1f, num2);
			float num4 = 2000f / (float)sandTileCount;
			float value = 3f / num4;
			value = MathHelper.Clamp(value, 0.77f, 1f);
			int num5 = (int)num4;
			float num6 = (float)Main.screenWidth / (float)Main.maxScreenW;
			int num7 = (int)(1000f * num6);
			float num8 = 20f * Severity;
			float num9 = (float)num7 * (Main.gfxQuality * 0.5f + 0.5f) + (float)num7 * 0.1f - (float)Dust.SandStormCount;
			if (num9 <= 0f)
			{
				return;
			}
			float num10 = (float)Main.screenWidth + 1000f;
			float num11 = Main.screenHeight;
			Vector2 vector = Main.screenPosition + player.velocity;
			WeightedRandom<Color> weightedRandom = new WeightedRandom<Color>();
			weightedRandom.Add(new Color(200, 160, 20, 180), Main.SceneMetrics.GetTileCount(53) + Main.SceneMetrics.GetTileCount(396) + Main.SceneMetrics.GetTileCount(397));
			weightedRandom.Add(new Color(103, 98, 122, 180), Main.SceneMetrics.GetTileCount(112) + Main.SceneMetrics.GetTileCount(400) + Main.SceneMetrics.GetTileCount(398));
			weightedRandom.Add(new Color(135, 43, 34, 180), Main.SceneMetrics.GetTileCount(234) + Main.SceneMetrics.GetTileCount(401) + Main.SceneMetrics.GetTileCount(399));
			weightedRandom.Add(new Color(213, 196, 197, 180), Main.SceneMetrics.GetTileCount(116) + Main.SceneMetrics.GetTileCount(403) + Main.SceneMetrics.GetTileCount(402));
			float num12 = MathHelper.Lerp(0.2f, 0.35f, Severity);
			float num13 = MathHelper.Lerp(0.5f, 0.7f, Severity);
			float amount = (value - 0.77f) / 0.23000002f;
			int maxValue2 = (int)MathHelper.Lerp(1f, 10f, amount);
			for (int i = 0; (float)i < num8; i++)
			{
				if (Main.rand.Next(num5 / 4) != 0)
				{
					continue;
				}
				Vector2 position = new Vector2(Main.rand.NextFloat() * num10 - 500f, Main.rand.NextFloat() * -50f);
				if (Main.rand.Next(3) == 0 && num == 1)
				{
					position.X = Main.rand.Next(500) - 500;
				}
				else if (Main.rand.Next(3) == 0 && num == -1)
				{
					position.X = Main.rand.Next(500) + Main.screenWidth;
				}
				if (position.X < 0f || position.X > (float)Main.screenWidth)
				{
					position.Y += Main.rand.NextFloat() * num11 * 0.9f;
				}
				position += vector;
				int num14 = (int)position.X / 16;
				int num15 = (int)position.Y / 16;
				if (!WorldGen.InWorld(num14, num15, 10) || Main.tile[num14, num15] == null || Main.tile[num14, num15].wall != 0)
				{
					continue;
				}
				for (int j = 0; j < 1; j++)
				{
					Dust dust = Main.dust[Dust.NewDust(position, 10, 10, 268)];
					dust.velocity.Y = 2f + Main.rand.NextFloat() * 0.2f;
					dust.velocity.Y *= dust.scale;
					dust.velocity.Y *= 0.35f;
					dust.velocity.X = num3 * 5f + Main.rand.NextFloat() * 1f;
					dust.velocity.X += num3 * num13 * 20f;
					dust.fadeIn += num13 * 0.2f;
					dust.velocity *= 1f + num12 * 0.5f;
					dust.color = weightedRandom;
					dust.velocity *= 1f + num12;
					dust.velocity *= value;
					dust.scale = 0.9f;
					num9 -= 1f;
					if (num9 <= 0f)
					{
						break;
					}
					if (Main.rand.Next(maxValue2) != 0)
					{
						j--;
						position += Utils.RandomVector2(Main.rand, -10f, 10f) + dust.velocity * -1.1f;
						num14 = (int)position.X / 16;
						num15 = (int)position.Y / 16;
						if (WorldGen.InWorld(num14, num15, 10) && Main.tile[num14, num15] != null)
						{
							_ = Main.tile[num14, num15].wall;
						}
					}
				}
				if (num9 <= 0f)
				{
					break;
				}
			}
		}
	}
}
