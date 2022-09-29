using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Physics;

namespace Terraria.GameContent.Golf
{
	public static class GolfHelper
	{
		public struct ClubProperties
		{
			public readonly Vector2 MinimumStrength;

			public readonly Vector2 MaximumStrength;

			public readonly float RoughLandResistance;

			public ClubProperties(Vector2 minimumStrength, Vector2 maximumStrength, float roughLandResistance)
			{
				MinimumStrength = minimumStrength;
				MaximumStrength = maximumStrength;
				RoughLandResistance = roughLandResistance;
			}
		}

		public struct ShotStrength
		{
			public readonly float AbsoluteStrength;

			public readonly float RelativeStrength;

			public readonly float RoughLandResistance;

			public ShotStrength(float absoluteStrength, float relativeStrength, float roughLandResistance)
			{
				AbsoluteStrength = absoluteStrength;
				RelativeStrength = relativeStrength;
				RoughLandResistance = roughLandResistance;
			}
		}

		public class ContactListener : IBallContactListener
		{
			public void OnCollision(PhysicsProperties properties, ref Vector2 position, ref Vector2 velocity, ref BallCollisionEvent collision)
			{
				TileMaterial byTileId = TileMaterials.GetByTileId(collision.Tile.type);
				Vector2 vector = velocity * byTileId.GolfPhysics.SideImpactDampening;
				Vector2 vector2 = collision.Normal * Vector2.Dot(velocity, collision.Normal) * (byTileId.GolfPhysics.DirectImpactDampening - byTileId.GolfPhysics.SideImpactDampening);
				velocity = vector + vector2;
				Projectile projectile = collision.Entity as Projectile;
				switch (collision.Tile.type)
				{
				case 421:
				case 422:
				{
					float num2 = 2.5f * collision.TimeScale;
					Vector2 vector3 = new Vector2(0f - collision.Normal.Y, collision.Normal.X);
					if (collision.Tile.type == 422)
					{
						vector3 = -vector3;
					}
					float num3 = Vector2.Dot(velocity, vector3);
					if (num3 < num2)
					{
						velocity += vector3 * MathHelper.Clamp(num2 - num3, 0f, num2 * 0.5f);
					}
					break;
				}
				case 476:
				{
					float num = velocity.Length() / collision.TimeScale;
					if (!(collision.Normal.Y > -0.01f) && !(num > 100f))
					{
						velocity *= 0f;
						if (projectile != null && projectile.active)
						{
							PutBallInCup(projectile, collision);
						}
					}
					break;
				}
				}
				if (projectile != null && velocity.Y < -0.3f && velocity.Y > -2f && velocity.Length() > 1f)
				{
					Dust dust = Dust.NewDustPerfect(collision.Entity.Center, 31, collision.Normal, 127);
					dust.scale = 0.7f;
					dust.fadeIn = 1f;
					dust.velocity = dust.velocity * 0.5f + Main.rand.NextVector2CircularEdge(0.5f, 0.4f);
				}
			}

			public void PutBallInCup(Projectile proj, BallCollisionEvent collision)
			{
				if (proj.owner == Main.myPlayer && Main.LocalGolfState.ShouldScoreHole)
				{
					Point hitLocation = (collision.ImpactPoint - collision.Normal * 0.5f).ToTileCoordinates();
					int owner = proj.owner;
					int num = (int)proj.ai[1];
					int type = proj.type;
					if (num > 1)
					{
						Main.LocalGolfState.SetScoreTime();
					}
					Main.LocalGolfState.RecordBallInfo(proj);
					Main.LocalGolfState.LandBall(proj);
					int golfBallScore = Main.LocalGolfState.GetGolfBallScore(proj);
					if (num > 0)
					{
						Main.player[owner].AccumulateGolfingScore(golfBallScore);
					}
					PutBallInCup_TextAndEffects(hitLocation, owner, num, type);
					Main.LocalGolfState.ResetScoreTime();
					Wiring.HitSwitch(hitLocation.X, hitLocation.Y);
					NetMessage.SendData(59, -1, -1, null, hitLocation.X, hitLocation.Y);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(128, -1, -1, null, owner, num, type, 0f, hitLocation.X, hitLocation.Y);
					}
				}
				proj.Kill();
			}

			public static void PutBallInCup_TextAndEffects(Point hitLocation, int plr, int numberOfHits, int projid)
			{
				if (numberOfHits != 0)
				{
					EmitGolfballExplosion(hitLocation.ToWorldCoordinates(8f, 0f));
					string key = "Game.BallBounceResultGolf_Single";
					NetworkText networkText;
					if (numberOfHits != 1)
					{
						key = "Game.BallBounceResultGolf_Plural";
						networkText = NetworkText.FromKey(key, Main.player[plr].name, NetworkText.FromKey(Lang.GetProjectileName(projid).Key), numberOfHits);
					}
					else
					{
						networkText = NetworkText.FromKey(key, Main.player[plr].name, NetworkText.FromKey(Lang.GetProjectileName(projid).Key));
					}
					if (Main.netMode == 0 || Main.netMode == 1)
					{
						Main.NewText(networkText.ToString(), byte.MaxValue, 240, 20);
					}
					else if (Main.netMode == 2)
					{
						ChatHelper.BroadcastChatMessage(networkText, new Color(255, 240, 20));
					}
				}
			}

			public void OnPassThrough(PhysicsProperties properties, ref Vector2 position, ref Vector2 velocity, ref float angularVelocity, ref BallPassThroughEvent collision)
			{
				switch (collision.Type)
				{
				case BallPassThroughType.Water:
					velocity *= 0.91f;
					angularVelocity *= 0.91f;
					break;
				case BallPassThroughType.Honey:
					velocity *= 0.8f;
					angularVelocity *= 0.8f;
					break;
				case BallPassThroughType.Tile:
				{
					TileMaterial byTileId = TileMaterials.GetByTileId(collision.Tile.type);
					velocity *= byTileId.GolfPhysics.PassThroughDampening;
					angularVelocity *= byTileId.GolfPhysics.PassThroughDampening;
					break;
				}
				case BallPassThroughType.Lava:
					break;
				}
			}

			public static void EmitGolfballExplosion_Old(Vector2 Center)
			{
				EmitGolfballExplosion(Center);
			}

			public static void EmitGolfballExplosion(Vector2 Center)
			{
				SoundEngine.PlaySound(SoundID.Item129, Center);
				for (float num = 0f; num < 1f; num += 0.085f)
				{
					Dust dust = Dust.NewDustPerfect(Center, 278, (num * (MathF.PI * 2f)).ToRotationVector2() * new Vector2(2f, 0.5f));
					dust.fadeIn = 1.2f;
					dust.noGravity = true;
					dust.velocity.X *= 0.7f;
					dust.velocity.Y -= 1.5f;
					dust.position.Y += 8f;
					dust.velocity.X *= 2f;
					dust.color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.5f);
				}
				float num2 = Main.rand.NextFloat();
				float num3 = Main.rand.Next(5, 10);
				for (int i = 0; (float)i < num3; i++)
				{
					int num4 = Main.rand.Next(5, 22);
					Vector2 value = (((float)i - num3 / 2f) * (MathF.PI * 2f) / 256f - MathF.PI / 2f).ToRotationVector2() * new Vector2(5f, 1f) * (0.25f + Main.rand.NextFloat() * 0.05f);
					Color color = Main.hslToRgb((num2 + (float)i / num3) % 1f, 0.7f, 0.7f);
					color.A = 127;
					for (int j = 0; j < num4; j++)
					{
						Dust dust2 = Dust.NewDustPerfect(Center + new Vector2((float)i - num3 / 2f, 0f) * 2f, 278, value);
						dust2.fadeIn = 0.7f;
						dust2.scale = 0.7f;
						dust2.noGravity = true;
						dust2.position.Y += -1f;
						dust2.velocity *= (float)j;
						dust2.scale += 0.2f - (float)j * 0.03f;
						dust2.velocity += Main.rand.NextVector2Circular(0.05f, 0.05f);
						dust2.color = color;
					}
				}
				for (float num5 = 0f; num5 < 1f; num5 += 0.2f)
				{
					Dust dust3 = Dust.NewDustPerfect(Center, 278, (num5 * (MathF.PI * 2f)).ToRotationVector2() * new Vector2(1f, 0.5f));
					dust3.fadeIn = 1.2f;
					dust3.noGravity = true;
					dust3.velocity.X *= 0.7f;
					dust3.velocity.Y -= 0.5f;
					dust3.position.Y += 8f;
					dust3.velocity.X *= 2f;
					dust3.color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.3f);
				}
				float num6 = Main.rand.NextFloatDirection();
				for (float num7 = 0f; num7 < 1f; num7 += 0.15f)
				{
					Dust dust4 = Dust.NewDustPerfect(Center, 278, (num6 + num7 * (MathF.PI * 2f)).ToRotationVector2() * 4f);
					dust4.fadeIn = 1.5f;
					dust4.velocity *= 0.5f + num7 * 0.8f;
					dust4.noGravity = true;
					dust4.velocity.X *= 0.35f;
					dust4.velocity.Y *= 2f;
					dust4.velocity.Y -= 1f;
					dust4.velocity.Y = 0f - Math.Abs(dust4.velocity.Y);
					dust4.position += dust4.velocity * 3f;
					dust4.color = Main.hslToRgb(Main.rand.NextFloat(), 1f, 0.6f + Main.rand.NextFloat() * 0.2f);
				}
			}

			public static void EmitGolfballExplosion_v1(Vector2 Center)
			{
				for (float num = 0f; num < 1f; num += 0.085f)
				{
					Dust dust = Dust.NewDustPerfect(Center, 278, (num * (MathF.PI * 2f)).ToRotationVector2() * new Vector2(2f, 0.5f));
					dust.fadeIn = 1.2f;
					dust.noGravity = true;
					dust.velocity.X *= 0.7f;
					dust.velocity.Y -= 1.5f;
					dust.position.Y += 8f;
					dust.color = Color.Lerp(Color.Silver, Color.White, 0.5f);
				}
				for (float num2 = 0f; num2 < 1f; num2 += 0.2f)
				{
					Dust dust2 = Dust.NewDustPerfect(Center, 278, (num2 * (MathF.PI * 2f)).ToRotationVector2() * new Vector2(1f, 0.5f));
					dust2.fadeIn = 1.2f;
					dust2.noGravity = true;
					dust2.velocity.X *= 0.7f;
					dust2.velocity.Y -= 0.5f;
					dust2.position.Y += 8f;
					dust2.color = Color.Lerp(Color.Silver, Color.White, 0.5f);
				}
				float num3 = Main.rand.NextFloatDirection();
				for (float num4 = 0f; num4 < 1f; num4 += 0.15f)
				{
					Dust dust3 = Dust.NewDustPerfect(Center, 278, (num3 + num4 * (MathF.PI * 2f)).ToRotationVector2() * 4f);
					dust3.fadeIn = 1.5f;
					dust3.velocity *= 0.5f + num4 * 0.8f;
					dust3.noGravity = true;
					dust3.velocity.X *= 0.35f;
					dust3.velocity.Y *= 2f;
					dust3.velocity.Y -= 1f;
					dust3.velocity.Y = 0f - Math.Abs(dust3.velocity.Y);
					dust3.position += dust3.velocity * 3f;
					dust3.color = Color.Lerp(Color.Silver, Color.White, 0.5f);
				}
			}
		}

		public const int PointsNeededForLevel1 = 500;

		public const int PointsNeededForLevel2 = 1000;

		public const int PointsNeededForLevel3 = 2000;

		public static readonly PhysicsProperties PhysicsProperties = new PhysicsProperties(0.3f, 0.99f);

		public static readonly ContactListener Listener = new ContactListener();

		public static FancyGolfPredictionLine PredictionLine;

		public static BallStepResult StepGolfBall(Entity entity, ref float angularVelocity)
		{
			return BallCollision.Step(PhysicsProperties, entity, ref angularVelocity, Listener);
		}

		public static Vector2 FindVectorOnOval(Vector2 vector, Vector2 radius)
		{
			if (Math.Abs(radius.X) < 0.0001f || Math.Abs(radius.Y) < 0.0001f)
			{
				return Vector2.Zero;
			}
			return Vector2.Normalize(vector / radius) * radius;
		}

		public static ShotStrength CalculateShotStrength(Vector2 shotVector, ClubProperties clubProperties)
		{
			Vector2.Normalize(shotVector);
			float value = shotVector.Length();
			float num = FindVectorOnOval(shotVector, clubProperties.MaximumStrength).Length();
			float num2 = FindVectorOnOval(shotVector, clubProperties.MinimumStrength).Length();
			float num3 = MathHelper.Clamp(value, num2, num);
			float relativeStrength = Math.Max((num3 - num2) / (num - num2), 0.001f);
			return new ShotStrength(num3 * 32f, relativeStrength, clubProperties.RoughLandResistance);
		}

		public static bool IsPlayerHoldingClub(Player player)
		{
			if (player == null || player.HeldItem == null)
			{
				return false;
			}
			int type = player.HeldItem.type;
			if (type == 4039 || (uint)(type - 4092) <= 2u || (uint)(type - 4587) <= 11u)
			{
				return true;
			}
			return false;
		}

		public static ShotStrength CalculateShotStrength(Projectile golfHelper, Entity golfBall)
		{
			int num = Main.screenWidth;
			if (num > Main.screenHeight)
			{
				num = Main.screenHeight;
			}
			int num2 = 150;
			num -= num2;
			num /= 2;
			if (num < 200)
			{
				num = 200;
			}
			float num3 = num;
			num3 = 300f;
			if (golfHelper.ai[0] != 0f)
			{
				return default(ShotStrength);
			}
			Vector2 shotVector = (golfHelper.Center - golfBall.Center) / num3;
			ClubProperties clubPropertiesFromGolfHelper = GetClubPropertiesFromGolfHelper(golfHelper);
			return CalculateShotStrength(shotVector, clubPropertiesFromGolfHelper);
		}

		public static ClubProperties GetClubPropertiesFromGolfHelper(Projectile golfHelper)
		{
			return GetClubProperties((short)Main.player[golfHelper.owner].HeldItem.type);
		}

		public static ClubProperties GetClubProperties(short itemId)
		{
			Vector2 vector = new Vector2(0.25f, 0.25f);
			return itemId switch
			{
				4039 => new ClubProperties(vector, Vector2.One, 0f), 
				4092 => new ClubProperties(Vector2.Zero, vector, 0f), 
				4093 => new ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f), 
				4094 => new ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f), 
				4587 => new ClubProperties(vector, Vector2.One, 0f), 
				4588 => new ClubProperties(Vector2.Zero, vector, 0f), 
				4589 => new ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f), 
				4590 => new ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f), 
				4591 => new ClubProperties(vector, Vector2.One, 0f), 
				4592 => new ClubProperties(Vector2.Zero, vector, 0f), 
				4593 => new ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f), 
				4594 => new ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f), 
				4595 => new ClubProperties(vector, Vector2.One, 0f), 
				4596 => new ClubProperties(Vector2.Zero, vector, 0f), 
				4597 => new ClubProperties(vector, new Vector2(0.65f, 1.5f), 1f), 
				4598 => new ClubProperties(vector, new Vector2(1.5f, 0.65f), 0f), 
				_ => default(ClubProperties), 
			};
		}

		public static Projectile FindHelperFromGolfBall(Projectile golfBall)
		{
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.type == 722 && projectile.owner == golfBall.owner)
				{
					return Main.projectile[i];
				}
			}
			return null;
		}

		public static Projectile FindGolfBallForHelper(Projectile golfHelper)
		{
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				Vector2 shotVector = golfHelper.Center - projectile.Center;
				if (projectile.active && ProjectileID.Sets.IsAGolfBall[projectile.type] && projectile.owner == golfHelper.owner && ValidateShot(projectile, Main.player[golfHelper.owner], ref shotVector))
				{
					return Main.projectile[i];
				}
			}
			return null;
		}

		public static bool IsGolfBallResting(Projectile golfBall)
		{
			if ((int)golfBall.localAI[1] != 0)
			{
				return Vector2.Distance(golfBall.position, golfBall.oldPos[golfBall.oldPos.Length - 1]) < 1f;
			}
			return true;
		}

		public static bool IsGolfShotValid(Entity golfBall, Player player)
		{
			Vector2 vector = golfBall.Center - player.Bottom;
			if (player.direction == -1)
			{
				vector.X *= -1f;
			}
			if (vector.X >= -16f && vector.X <= 32f && vector.Y <= 16f)
			{
				return vector.Y >= -16f;
			}
			return false;
		}

		public static bool ValidateShot(Entity golfBall, Player player, ref Vector2 shotVector)
		{
			Vector2 vector = golfBall.Center - player.Bottom;
			if (player.direction == -1)
			{
				vector.X *= -1f;
				shotVector.X *= -1f;
			}
			float num = shotVector.ToRotation();
			if (num > 0f)
			{
				shotVector = shotVector.Length() * new Vector2((float)Math.Cos(0.0), (float)Math.Sin(0.0));
			}
			else if (num < -1.5207964f)
			{
				shotVector = shotVector.Length() * new Vector2((float)Math.Cos(-1.5207964181900024), (float)Math.Sin(-1.5207964181900024));
			}
			if (player.direction == -1)
			{
				shotVector.X *= -1f;
			}
			if (vector.X >= -16f && vector.X <= 32f && vector.Y <= 16f)
			{
				return vector.Y >= -16f;
			}
			return false;
		}

		public static void HitGolfBall(Entity entity, Vector2 velocity, float roughLandResistance)
		{
			Vector2 bottom = entity.Bottom;
			bottom.Y += 1f;
			Point point = bottom.ToTileCoordinates();
			Tile tile = Main.tile[point.X, point.Y];
			if (tile != null && tile.active())
			{
				TileMaterial byTileId = TileMaterials.GetByTileId(tile.type);
				velocity = Vector2.Lerp(velocity * byTileId.GolfPhysics.ClubImpactDampening, velocity, byTileId.GolfPhysics.ImpactDampeningResistanceEfficiency * roughLandResistance);
			}
			entity.velocity = velocity;
			if (entity is Projectile projectile)
			{
				projectile.timeLeft = 18000;
				if (projectile.ai[1] < 0f)
				{
					projectile.ai[1] = 0f;
				}
				projectile.ai[1] += 1f;
				projectile.localAI[1] = 1f;
				Main.LocalGolfState.RecordSwing(projectile);
			}
		}

		public static void DrawPredictionLine(Entity golfBall, Vector2 impactVelocity, float chargeProgress, float roughLandResistance)
		{
			if (PredictionLine == null)
			{
				PredictionLine = new FancyGolfPredictionLine(20);
			}
			PredictionLine.Update(golfBall, impactVelocity, roughLandResistance);
			PredictionLine.Draw(Main.Camera, Main.spriteBatch, chargeProgress);
		}
	}
}
