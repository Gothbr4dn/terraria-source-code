using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent.Ambience;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	public class AmbientSky : CustomSky
	{
		private abstract class SkyEntity
		{
			public Vector2 Position;

			public Asset<Texture2D> Texture;

			public SpriteFrame Frame;

			public float Depth;

			public SpriteEffects Effects;

			public bool IsActive = true;

			public float Rotation;

			public Rectangle SourceRectangle => Frame.GetSourceRectangle(Texture.get_Value());

			protected void NextFrame()
			{
				Frame.CurrentRow = (byte)((Frame.CurrentRow + 1) % (int)Frame.RowCount);
			}

			public abstract Color GetColor(Color backgroundColor);

			public abstract void Update(int frameCount);

			protected void SetPositionInWorldBasedOnScreenSpace(Vector2 actualWorldSpace)
			{
				Vector2 vector = actualWorldSpace - Main.Camera.Center;
				Vector2 vector2 = (Position = Main.Camera.Center + vector * (Depth / 3f));
			}

			public abstract Vector2 GetDrawPosition();

			public virtual void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
			{
				CommonDraw(spriteBatch, depthScale, minDepth, maxDepth);
			}

			public void CommonDraw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
			{
				if (!(Depth <= minDepth) && !(Depth > maxDepth))
				{
					Vector2 drawPositionByDepth = GetDrawPositionByDepth();
					Color color = GetColor(Main.ColorOfTheSkies) * Main.atmo;
					Vector2 origin = SourceRectangle.Size() / 2f;
					float scale = depthScale / Depth;
					spriteBatch.Draw(Texture.get_Value(), drawPositionByDepth - Main.Camera.UnscaledPosition, SourceRectangle, color, Rotation, origin, scale, Effects, 0f);
				}
			}

			internal Vector2 GetDrawPositionByDepth()
			{
				return (GetDrawPosition() - Main.Camera.Center) * new Vector2(1f / Depth, 0.9f / Depth) + Main.Camera.Center;
			}

			internal float Helper_GetOpacityWithAccountingForOceanWaterLine()
			{
				Vector2 vector = GetDrawPositionByDepth() - Main.Camera.UnscaledPosition;
				int num = SourceRectangle.Height / 2;
				float t = vector.Y + (float)num;
				float yScreenPosition = AmbientSkyDrawCache.Instance.OceanLineInfo.YScreenPosition;
				float lerpValue = Utils.GetLerpValue(yScreenPosition - 10f, yScreenPosition - 2f, t, clamped: true);
				lerpValue *= AmbientSkyDrawCache.Instance.OceanLineInfo.OceanOpacity;
				return 1f - lerpValue;
			}
		}

		private class FadingSkyEntity : SkyEntity
		{
			protected int LifeTime;

			protected Vector2 Velocity;

			protected int FramingSpeed;

			protected int TimeEntitySpawnedIn;

			protected float Opacity;

			protected float BrightnessLerper;

			protected float FinalOpacityMultiplier;

			protected float OpacityNormalizedTimeToFadeIn;

			protected float OpacityNormalizedTimeToFadeOut;

			protected int FrameOffset;

			public FadingSkyEntity()
			{
				Opacity = 0f;
				TimeEntitySpawnedIn = -1;
				BrightnessLerper = 0f;
				FinalOpacityMultiplier = 1f;
				OpacityNormalizedTimeToFadeIn = 0.1f;
				OpacityNormalizedTimeToFadeOut = 0.9f;
			}

			public override void Update(int frameCount)
			{
				if (!IsMovementDone(frameCount))
				{
					UpdateOpacity(frameCount);
					if ((frameCount + FrameOffset) % FramingSpeed == 0)
					{
						NextFrame();
					}
					UpdateVelocity(frameCount);
					Position += Velocity;
				}
			}

			public virtual void UpdateVelocity(int frameCount)
			{
			}

			private void UpdateOpacity(int frameCount)
			{
				int num = frameCount - TimeEntitySpawnedIn;
				if ((float)num >= (float)LifeTime * OpacityNormalizedTimeToFadeOut)
				{
					Opacity = Utils.GetLerpValue(LifeTime, (float)LifeTime * OpacityNormalizedTimeToFadeOut, num, clamped: true);
				}
				else
				{
					Opacity = Utils.GetLerpValue(0f, (float)LifeTime * OpacityNormalizedTimeToFadeIn, num, clamped: true);
				}
			}

			private bool IsMovementDone(int frameCount)
			{
				if (TimeEntitySpawnedIn == -1)
				{
					TimeEntitySpawnedIn = frameCount;
				}
				if (frameCount - TimeEntitySpawnedIn >= LifeTime)
				{
					IsActive = false;
					return true;
				}
				return false;
			}

			public override Color GetColor(Color backgroundColor)
			{
				return Color.Lerp(backgroundColor, Color.White, BrightnessLerper) * Opacity * FinalOpacityMultiplier * Helper_GetOpacityWithAccountingForOceanWaterLine();
			}

			public void StartFadingOut(int currentFrameCount)
			{
				int num = (int)((float)LifeTime * OpacityNormalizedTimeToFadeOut);
				int num2 = currentFrameCount - num;
				if (num2 < TimeEntitySpawnedIn)
				{
					TimeEntitySpawnedIn = num2;
				}
			}

			public override Vector2 GetDrawPosition()
			{
				return Position;
			}
		}

		private class ButterfliesSkyEntity : FadingSkyEntity
		{
			public ButterfliesSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 4000f) + 4000f;
				Depth = random.NextFloat() * 3f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				int num2 = random.Next(2) + 1;
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/ButterflySwarm" + num2, (AssetRequestMode)1);
				Frame = new SpriteFrame(1, (byte)((num2 == 2) ? 19u : 17u));
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.15f;
				OpacityNormalizedTimeToFadeOut = 0.85f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 5;
			}

			public override void UpdateVelocity(int frameCount)
			{
				float num = 0.1f + Math.Abs(Main.WindForVisuals) * 0.05f;
				Velocity = new Vector2(num * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1)), 0f);
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					StartFadingOut(frameCount);
				}
			}
		}

		private class LostKiteSkyEntity : FadingSkyEntity
		{
			public LostKiteSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				Depth = random.NextFloat() * 3f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/LostKite", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 42);
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.15f;
				OpacityNormalizedTimeToFadeOut = 0.85f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 6;
				int num2 = random.Next(Frame.RowCount);
				for (int i = 0; i < num2; i++)
				{
					NextFrame();
				}
			}

			public override void UpdateVelocity(int frameCount)
			{
				float num = 1.2f + Math.Abs(Main.WindForVisuals) * 3f;
				if (Main.IsItStorming)
				{
					num *= 1.5f;
				}
				Velocity = new Vector2(num * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1)), 0f);
			}

			public override void Update(int frameCount)
			{
				if (Main.IsItStorming)
				{
					FramingSpeed = 4;
				}
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				base.Update(frameCount);
				if (!Main.dayTime || Main.eclipse)
				{
					StartFadingOut(frameCount);
				}
			}
		}

		private class PegasusSkyEntity : FadingSkyEntity
		{
			public PegasusSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				Depth = random.NextFloat() * 3f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Pegasus", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 11);
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.15f;
				OpacityNormalizedTimeToFadeOut = 0.85f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 5;
			}

			public override void UpdateVelocity(int frameCount)
			{
				float num = 1.5f + Math.Abs(Main.WindForVisuals) * 0.6f;
				Velocity = new Vector2(num * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1)), 0f);
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					StartFadingOut(frameCount);
				}
			}

			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[6];
			}
		}

		private class VultureSkyEntity : FadingSkyEntity
		{
			public VultureSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				Depth = random.NextFloat() * 3f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Vulture", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 10);
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.15f;
				OpacityNormalizedTimeToFadeOut = 0.85f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 5;
			}

			public override void UpdateVelocity(int frameCount)
			{
				float num = 3f + Math.Abs(Main.WindForVisuals) * 0.8f;
				Velocity = new Vector2(num * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1)), 0f);
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					StartFadingOut(frameCount);
				}
			}

			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Math.Max(Main.bgAlphaFrontLayer[2], Main.bgAlphaFrontLayer[5]);
			}
		}

		private class PixiePosseSkyEntity : FadingSkyEntity
		{
			private int pixieType = 1;

			public PixiePosseSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 4000f) + 4000f;
				Depth = random.NextFloat() * 3f + 2f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				if (!Main.dayTime)
				{
					pixieType = 2;
				}
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/PixiePosse" + pixieType, (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 25);
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.15f;
				OpacityNormalizedTimeToFadeOut = 0.85f;
				BrightnessLerper = 0.6f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 5;
			}

			public override void UpdateVelocity(int frameCount)
			{
				float num = 0.12f + Math.Abs(Main.WindForVisuals) * 0.08f;
				Velocity = new Vector2(num * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1)), 0f);
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if ((pixieType == 1 && !Main.dayTime) || (pixieType == 2 && Main.dayTime) || Main.IsItRaining || Main.eclipse || Main.bloodMoon || Main.pumpkinMoon || Main.snowMoon)
				{
					StartFadingOut(frameCount);
				}
			}

			public override void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
			{
				CommonDraw(spriteBatch, depthScale - 0.1f, minDepth, maxDepth);
			}
		}

		private class BirdsPackSkyEntity : FadingSkyEntity
		{
			public BirdsPackSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				Depth = random.NextFloat() * 3f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/BirdsVShape", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 4);
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.15f;
				OpacityNormalizedTimeToFadeOut = 0.85f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 5;
			}

			public override void UpdateVelocity(int frameCount)
			{
				float num = 3f + Math.Abs(Main.WindForVisuals) * 0.8f;
				Velocity = new Vector2(num * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1)), 0f);
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					StartFadingOut(frameCount);
				}
			}
		}

		private class SeagullsGroupSkyEntity : FadingSkyEntity
		{
			private Vector2 _magnetAccelerations;

			private Vector2 _magnetPointTarget;

			private Vector2 _positionVsMagnet;

			private Vector2 _velocityVsMagnet;

			public SeagullsGroupSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				Depth = random.NextFloat() * 3f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Seagull", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 9);
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.1f;
				OpacityNormalizedTimeToFadeOut = 0.9f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 4;
				FrameOffset = random.Next(0, Frame.RowCount);
				int num2 = random.Next(Frame.RowCount);
				for (int i = 0; i < num2; i++)
				{
					NextFrame();
				}
			}

			public override void UpdateVelocity(int frameCount)
			{
				Vector2 vector = _magnetAccelerations * new Vector2(Math.Sign(_magnetPointTarget.X - _positionVsMagnet.X), Math.Sign(_magnetPointTarget.Y - _positionVsMagnet.Y));
				_velocityVsMagnet += vector;
				_positionVsMagnet += _velocityVsMagnet;
				float x = 4f * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1));
				Velocity = new Vector2(x, 0f) + _velocityVsMagnet;
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					StartFadingOut(frameCount);
				}
			}

			public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
			{
				_magnetAccelerations = accelerations;
				_magnetPointTarget = targetOffset;
			}

			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[4];
			}

			public override void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
			{
				CommonDraw(spriteBatch, depthScale - 1.5f, minDepth, maxDepth);
			}

			public static List<SeagullsGroupSkyEntity> CreateGroup(Player player, FastRandom random)
			{
				List<SeagullsGroupSkyEntity> list = new List<SeagullsGroupSkyEntity>();
				int num = 100;
				int num2 = random.Next(5, 9);
				float num3 = 100f;
				VirtualCamera virtualCamera = new VirtualCamera(player);
				SpriteEffects spriteEffects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				Vector2 vector = default(Vector2);
				if (spriteEffects == SpriteEffects.FlipHorizontally)
				{
					vector.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					vector.X = virtualCamera.Position.X - (float)num;
				}
				vector.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				float num4 = random.NextFloat() * 2f + 1f;
				int num5 = random.Next(30, 61) * 60;
				Vector2 vector2 = new Vector2(random.NextFloat() * 0.5f + 0.5f, random.NextFloat() * 0.5f + 0.5f);
				Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * num3;
				for (int i = 0; i < num2; i++)
				{
					SeagullsGroupSkyEntity seagullsGroupSkyEntity = new SeagullsGroupSkyEntity(player, random);
					seagullsGroupSkyEntity.Depth = num4 + random.NextFloat() * 0.5f;
					seagullsGroupSkyEntity.Position = vector + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 50f;
					seagullsGroupSkyEntity.Effects = spriteEffects;
					seagullsGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(seagullsGroupSkyEntity.Position);
					seagullsGroupSkyEntity.LifeTime = num5 + random.Next(301);
					seagullsGroupSkyEntity.SetMagnetization(vector2 * (random.NextFloat() * 0.3f + 0.85f) * 0.05f, targetOffset);
					list.Add(seagullsGroupSkyEntity);
				}
				return list;
			}
		}

		private class GastropodGroupSkyEntity : FadingSkyEntity
		{
			private Vector2 _magnetAccelerations;

			private Vector2 _magnetPointTarget;

			private Vector2 _positionVsMagnet;

			private Vector2 _velocityVsMagnet;

			public GastropodGroupSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 3200f) + 3200f;
				Depth = random.NextFloat() * 3f + 2f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Gastropod", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 1);
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.1f;
				OpacityNormalizedTimeToFadeOut = 0.9f;
				BrightnessLerper = 0.75f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = int.MaxValue;
			}

			public override void UpdateVelocity(int frameCount)
			{
				Vector2 vector = _magnetAccelerations * new Vector2(Math.Sign(_magnetPointTarget.X - _positionVsMagnet.X), Math.Sign(_magnetPointTarget.Y - _positionVsMagnet.Y));
				_velocityVsMagnet += vector;
				_positionVsMagnet += _velocityVsMagnet;
				float x = (1.5f + Math.Abs(Main.WindForVisuals) * 0.2f) * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1));
				Velocity = new Vector2(x, 0f) + _velocityVsMagnet;
				Rotation = Velocity.X * 0.1f;
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || Main.dayTime || Main.bloodMoon || Main.pumpkinMoon || Main.snowMoon)
				{
					StartFadingOut(frameCount);
				}
			}

			public override Color GetColor(Color backgroundColor)
			{
				return Color.Lerp(backgroundColor, Colors.AmbientNPCGastropodLight, BrightnessLerper) * Opacity * FinalOpacityMultiplier;
			}

			public override void Draw(SpriteBatch spriteBatch, float depthScale, float minDepth, float maxDepth)
			{
				CommonDraw(spriteBatch, depthScale - 0.1f, minDepth, maxDepth);
			}

			public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
			{
				_magnetAccelerations = accelerations;
				_magnetPointTarget = targetOffset;
			}

			public static List<GastropodGroupSkyEntity> CreateGroup(Player player, FastRandom random)
			{
				List<GastropodGroupSkyEntity> list = new List<GastropodGroupSkyEntity>();
				int num = 100;
				int num2 = random.Next(3, 8);
				VirtualCamera virtualCamera = new VirtualCamera(player);
				SpriteEffects spriteEffects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				Vector2 vector = default(Vector2);
				if (spriteEffects == SpriteEffects.FlipHorizontally)
				{
					vector.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					vector.X = virtualCamera.Position.X - (float)num;
				}
				vector.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 3200f) + 3200f;
				float num3 = random.NextFloat() * 3f + 2f;
				int num4 = random.Next(30, 61) * 60;
				Vector2 vector2 = new Vector2(random.NextFloat() * 0.1f + 0.1f, random.NextFloat() * 0.3f + 0.3f);
				Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 120f;
				for (int i = 0; i < num2; i++)
				{
					GastropodGroupSkyEntity gastropodGroupSkyEntity = new GastropodGroupSkyEntity(player, random);
					gastropodGroupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
					gastropodGroupSkyEntity.Position = vector + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 60f;
					gastropodGroupSkyEntity.Effects = spriteEffects;
					gastropodGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(gastropodGroupSkyEntity.Position);
					gastropodGroupSkyEntity.LifeTime = num4 + random.Next(301);
					gastropodGroupSkyEntity.SetMagnetization(vector2 * (random.NextFloat() * 0.5f) * 0.05f, targetOffset);
					list.Add(gastropodGroupSkyEntity);
				}
				return list;
			}
		}

		private class SlimeBalloonGroupSkyEntity : FadingSkyEntity
		{
			private Vector2 _magnetAccelerations;

			private Vector2 _magnetPointTarget;

			private Vector2 _positionVsMagnet;

			private Vector2 _velocityVsMagnet;

			public SlimeBalloonGroupSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 4000f) + 4000f;
				Depth = random.NextFloat() * 3f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/SlimeBalloons", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 7);
				Frame.CurrentRow = (byte)random.Next(7);
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.025f;
				OpacityNormalizedTimeToFadeOut = 0.975f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = int.MaxValue;
			}

			public override void UpdateVelocity(int frameCount)
			{
				Vector2 vector = _magnetAccelerations * new Vector2(Math.Sign(_magnetPointTarget.X - _positionVsMagnet.X), Math.Sign(_magnetPointTarget.Y - _positionVsMagnet.Y));
				_velocityVsMagnet += vector;
				_positionVsMagnet += _velocityVsMagnet;
				float x = (1f + Math.Abs(Main.WindForVisuals) * 1f) * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1));
				Velocity = new Vector2(x, -0.01f) + _velocityVsMagnet;
				Rotation = Velocity.X * 0.1f;
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				if (!Main.IsItAHappyWindyDay || Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					StartFadingOut(frameCount);
				}
			}

			public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
			{
				_magnetAccelerations = accelerations;
				_magnetPointTarget = targetOffset;
			}

			public static List<SlimeBalloonGroupSkyEntity> CreateGroup(Player player, FastRandom random)
			{
				List<SlimeBalloonGroupSkyEntity> list = new List<SlimeBalloonGroupSkyEntity>();
				int num = 100;
				int num2 = random.Next(5, 10);
				VirtualCamera virtualCamera = new VirtualCamera(player);
				SpriteEffects spriteEffects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				Vector2 vector = default(Vector2);
				if (spriteEffects == SpriteEffects.FlipHorizontally)
				{
					vector.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					vector.X = virtualCamera.Position.X - (float)num;
				}
				vector.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				float num3 = random.NextFloat() * 3f + 3f;
				int num4 = random.Next(80, 121) * 60;
				Vector2 vector2 = new Vector2(random.NextFloat() * 0.1f + 0.1f, random.NextFloat() * 0.1f + 0.1f);
				Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 150f;
				for (int i = 0; i < num2; i++)
				{
					SlimeBalloonGroupSkyEntity slimeBalloonGroupSkyEntity = new SlimeBalloonGroupSkyEntity(player, random);
					slimeBalloonGroupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
					slimeBalloonGroupSkyEntity.Position = vector + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 80f;
					slimeBalloonGroupSkyEntity.Effects = spriteEffects;
					slimeBalloonGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(slimeBalloonGroupSkyEntity.Position);
					slimeBalloonGroupSkyEntity.LifeTime = num4 + random.Next(301);
					slimeBalloonGroupSkyEntity.SetMagnetization(vector2 * (random.NextFloat() * 0.2f) * 0.05f, targetOffset);
					list.Add(slimeBalloonGroupSkyEntity);
				}
				return list;
			}
		}

		private class HellBatsGoupSkyEntity : FadingSkyEntity
		{
			private Vector2 _magnetAccelerations;

			private Vector2 _magnetPointTarget;

			private Vector2 _positionVsMagnet;

			private Vector2 _velocityVsMagnet;

			public HellBatsGoupSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * 400f + (float)(Main.UnderworldLayer * 16);
				Depth = random.NextFloat() * 5f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/HellBat" + random.Next(1, 3), (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 10);
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.1f;
				OpacityNormalizedTimeToFadeOut = 0.9f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 4;
				FrameOffset = random.Next(0, Frame.RowCount);
				int num2 = random.Next(Frame.RowCount);
				for (int i = 0; i < num2; i++)
				{
					NextFrame();
				}
			}

			public override void UpdateVelocity(int frameCount)
			{
				Vector2 vector = _magnetAccelerations * new Vector2(Math.Sign(_magnetPointTarget.X - _positionVsMagnet.X), Math.Sign(_magnetPointTarget.Y - _positionVsMagnet.Y));
				_velocityVsMagnet += vector;
				_positionVsMagnet += _velocityVsMagnet;
				float x = (3f + Math.Abs(Main.WindForVisuals) * 0.8f) * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1));
				Velocity = new Vector2(x, 0f) + _velocityVsMagnet;
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
			}

			public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
			{
				_magnetAccelerations = accelerations;
				_magnetPointTarget = targetOffset;
			}

			public override Color GetColor(Color backgroundColor)
			{
				return Color.Lerp(Color.White, Color.Gray, Depth / 15f) * Opacity * FinalOpacityMultiplier * Helper_GetOpacityWithAccountingForBackgroundsOff();
			}

			public static List<HellBatsGoupSkyEntity> CreateGroup(Player player, FastRandom random)
			{
				List<HellBatsGoupSkyEntity> list = new List<HellBatsGoupSkyEntity>();
				int num = 100;
				int num2 = random.Next(20, 40);
				VirtualCamera virtualCamera = new VirtualCamera(player);
				SpriteEffects spriteEffects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				Vector2 vector = default(Vector2);
				if (spriteEffects == SpriteEffects.FlipHorizontally)
				{
					vector.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					vector.X = virtualCamera.Position.X - (float)num;
				}
				vector.Y = random.NextFloat() * 800f + (float)(Main.UnderworldLayer * 16);
				float num3 = random.NextFloat() * 5f + 3f;
				int num4 = random.Next(30, 61) * 60;
				Vector2 vector2 = new Vector2(random.NextFloat() * 0.5f + 0.5f, random.NextFloat() * 0.5f + 0.5f);
				Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 100f;
				for (int i = 0; i < num2; i++)
				{
					HellBatsGoupSkyEntity hellBatsGoupSkyEntity = new HellBatsGoupSkyEntity(player, random);
					hellBatsGoupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
					hellBatsGoupSkyEntity.Position = vector + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 50f;
					hellBatsGoupSkyEntity.Effects = spriteEffects;
					hellBatsGoupSkyEntity.SetPositionInWorldBasedOnScreenSpace(hellBatsGoupSkyEntity.Position);
					hellBatsGoupSkyEntity.LifeTime = num4 + random.Next(301);
					hellBatsGoupSkyEntity.SetMagnetization(vector2 * (random.NextFloat() * 0.3f + 0.85f) * 0.05f, targetOffset);
					list.Add(hellBatsGoupSkyEntity);
				}
				return list;
			}

			internal float Helper_GetOpacityWithAccountingForBackgroundsOff()
			{
				if (Main.netMode == 2 || Main.BackgroundEnabled)
				{
					return 1f;
				}
				return 0f;
			}
		}

		private class BatsGroupSkyEntity : FadingSkyEntity
		{
			private Vector2 _magnetAccelerations;

			private Vector2 _magnetPointTarget;

			private Vector2 _positionVsMagnet;

			private Vector2 _velocityVsMagnet;

			public BatsGroupSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				Depth = random.NextFloat() * 3f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Bat" + random.Next(1, 4), (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 10);
				LifeTime = random.Next(60, 121) * 60;
				OpacityNormalizedTimeToFadeIn = 0.1f;
				OpacityNormalizedTimeToFadeOut = 0.9f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 4;
				FrameOffset = random.Next(0, Frame.RowCount);
				int num2 = random.Next(Frame.RowCount);
				for (int i = 0; i < num2; i++)
				{
					NextFrame();
				}
			}

			public override void UpdateVelocity(int frameCount)
			{
				Vector2 vector = _magnetAccelerations * new Vector2(Math.Sign(_magnetPointTarget.X - _positionVsMagnet.X), Math.Sign(_magnetPointTarget.Y - _positionVsMagnet.Y));
				_velocityVsMagnet += vector;
				_positionVsMagnet += _velocityVsMagnet;
				float x = (3f + Math.Abs(Main.WindForVisuals) * 0.8f) * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1));
				Velocity = new Vector2(x, 0f) + _velocityVsMagnet;
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					StartFadingOut(frameCount);
				}
			}

			public void SetMagnetization(Vector2 accelerations, Vector2 targetOffset)
			{
				_magnetAccelerations = accelerations;
				_magnetPointTarget = targetOffset;
			}

			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Utils.Max<float>(Main.bgAlphaFrontLayer[3], Main.bgAlphaFrontLayer[0], Main.bgAlphaFrontLayer[10], Main.bgAlphaFrontLayer[11], Main.bgAlphaFrontLayer[12]);
			}

			public static List<BatsGroupSkyEntity> CreateGroup(Player player, FastRandom random)
			{
				List<BatsGroupSkyEntity> list = new List<BatsGroupSkyEntity>();
				int num = 100;
				int num2 = random.Next(20, 40);
				VirtualCamera virtualCamera = new VirtualCamera(player);
				SpriteEffects spriteEffects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				Vector2 vector = default(Vector2);
				if (spriteEffects == SpriteEffects.FlipHorizontally)
				{
					vector.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					vector.X = virtualCamera.Position.X - (float)num;
				}
				vector.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				float num3 = random.NextFloat() * 3f + 3f;
				int num4 = random.Next(30, 61) * 60;
				Vector2 vector2 = new Vector2(random.NextFloat() * 0.5f + 0.5f, random.NextFloat() * 0.5f + 0.5f);
				Vector2 targetOffset = new Vector2(random.NextFloat() * 2f - 1f, random.NextFloat() * 2f - 1f) * 100f;
				for (int i = 0; i < num2; i++)
				{
					BatsGroupSkyEntity batsGroupSkyEntity = new BatsGroupSkyEntity(player, random);
					batsGroupSkyEntity.Depth = num3 + random.NextFloat() * 0.5f;
					batsGroupSkyEntity.Position = vector + new Vector2(random.NextFloat() * 20f - 10f, random.NextFloat() * 3f) * 50f;
					batsGroupSkyEntity.Effects = spriteEffects;
					batsGroupSkyEntity.SetPositionInWorldBasedOnScreenSpace(batsGroupSkyEntity.Position);
					batsGroupSkyEntity.LifeTime = num4 + random.Next(301);
					batsGroupSkyEntity.SetMagnetization(vector2 * (random.NextFloat() * 0.3f + 0.85f) * 0.05f, targetOffset);
					list.Add(batsGroupSkyEntity);
				}
				return list;
			}
		}

		private class WyvernSkyEntity : FadingSkyEntity
		{
			public WyvernSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((!(Main.WindForVisuals > 0f)) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				Depth = random.NextFloat() * 3f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Wyvern", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 5);
				LifeTime = random.Next(40, 71) * 60;
				OpacityNormalizedTimeToFadeIn = 0.15f;
				OpacityNormalizedTimeToFadeOut = 0.85f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 4;
			}

			public override void UpdateVelocity(int frameCount)
			{
				float num = 3f + Math.Abs(Main.WindForVisuals) * 0.8f;
				Velocity = new Vector2(num * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1)), 0f);
			}
		}

		private class NormalizedBackgroundLayerSpaceSkyEntity : SkyEntity
		{
			public override Color GetColor(Color backgroundColor)
			{
				return Color.Lerp(backgroundColor, Color.White, 0.3f);
			}

			public override Vector2 GetDrawPosition()
			{
				return Position;
			}

			public override void Update(int frameCount)
			{
			}
		}

		private class BoneSerpentSkyEntity : NormalizedBackgroundLayerSpaceSkyEntity
		{
		}

		private class AirshipSkyEntity : FadingSkyEntity
		{
			public AirshipSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera virtualCamera = new VirtualCamera(player);
				Effects = ((random.Next(2) != 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				int num = 100;
				if (Effects == SpriteEffects.FlipHorizontally)
				{
					Position.X = virtualCamera.Position.X + virtualCamera.Size.X + (float)num;
				}
				else
				{
					Position.X = virtualCamera.Position.X - (float)num;
				}
				Position.Y = random.NextFloat() * ((float)Main.worldSurface * 16f - 1600f - 2400f) + 2400f;
				Depth = random.NextFloat() * 3f + 3f;
				SetPositionInWorldBasedOnScreenSpace(Position);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/FlyingShip", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 4);
				LifeTime = random.Next(40, 71) * 60;
				OpacityNormalizedTimeToFadeIn = 0.05f;
				OpacityNormalizedTimeToFadeOut = 0.95f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 4;
			}

			public override void UpdateVelocity(int frameCount)
			{
				float num = 6f + Math.Abs(Main.WindForVisuals) * 1.6f;
				Velocity = new Vector2(num * (float)((Effects != SpriteEffects.FlipHorizontally) ? 1 : (-1)), 0f);
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					StartFadingOut(frameCount);
				}
			}
		}

		private class AirBalloonSkyEntity : FadingSkyEntity
		{
			private const int RANDOM_TILE_SPAWN_RANGE = 100;

			public AirBalloonSkyEntity(Player player, FastRandom random)
			{
				new VirtualCamera(player);
				int x = player.Center.ToTileCoordinates().X;
				Effects = ((random.Next(2) != 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				Position.X = ((float)x + 100f * (random.NextFloat() * 2f - 1f)) * 16f;
				Position.Y = (float)Main.worldSurface * 16f - (float)random.Next(50, 81) * 16f;
				Depth = random.NextFloat() * 3f + 3f;
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/AirBalloons_" + ((random.Next(2) == 0) ? "Large" : "Small"), (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 5);
				Frame.CurrentRow = (byte)random.Next(5);
				LifeTime = random.Next(20, 51) * 60;
				OpacityNormalizedTimeToFadeIn = 0.05f;
				OpacityNormalizedTimeToFadeOut = 0.95f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = int.MaxValue;
			}

			public override void UpdateVelocity(int frameCount)
			{
				float x = Main.WindForVisuals * 4f;
				float num = 3f + Math.Abs(Main.WindForVisuals) * 1f;
				if ((double)Position.Y < Main.worldSurface * 12.0)
				{
					num *= 0.5f;
				}
				if ((double)Position.Y < Main.worldSurface * 8.0)
				{
					num *= 0.5f;
				}
				if ((double)Position.Y < Main.worldSurface * 4.0)
				{
					num *= 0.5f;
				}
				Velocity = new Vector2(x, 0f - num);
			}

			public override void Update(int frameCount)
			{
				base.Update(frameCount);
				if (Main.IsItRaining || !Main.dayTime || Main.eclipse)
				{
					StartFadingOut(frameCount);
				}
			}
		}

		private class CrimeraSkyEntity : EOCSkyEntity
		{
			public CrimeraSkyEntity(Player player, FastRandom random)
				: base(player, random)
			{
				int num = 3;
				if (Depth <= 6f)
				{
					num = 2;
				}
				if (Depth <= 5f)
				{
					num = 1;
				}
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Crimera" + num, (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 3);
			}

			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[8];
			}
		}

		private class EOSSkyEntity : EOCSkyEntity
		{
			public EOSSkyEntity(Player player, FastRandom random)
				: base(player, random)
			{
				int num = 3;
				if (Depth <= 6f)
				{
					num = 2;
				}
				if (Depth <= 5f)
				{
					num = 1;
				}
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/EOS" + num, (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 4);
			}

			public override Color GetColor(Color backgroundColor)
			{
				return base.GetColor(backgroundColor) * Main.bgAlphaFrontLayer[1];
			}
		}

		private class EOCSkyEntity : FadingSkyEntity
		{
			private const int STATE_ZIGZAG = 1;

			private const int STATE_GOOVERPLAYER = 2;

			private int _state;

			private int _direction;

			private float _waviness;

			public EOCSkyEntity(Player player, FastRandom random)
			{
				VirtualCamera camera = new VirtualCamera(player);
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/EOC", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 3);
				Depth = random.NextFloat() * 3f + 4.5f;
				if (random.Next(4) != 0)
				{
					BeginZigZag(ref random, camera, (random.Next(2) == 1) ? 1 : (-1));
				}
				else
				{
					BeginChasingPlayer(ref random, camera);
				}
				SetPositionInWorldBasedOnScreenSpace(Position);
				OpacityNormalizedTimeToFadeIn = 0.1f;
				OpacityNormalizedTimeToFadeOut = 0.9f;
				BrightnessLerper = 0.2f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 5;
			}

			private void BeginZigZag(ref FastRandom random, VirtualCamera camera, int direction)
			{
				_state = 1;
				LifeTime = random.Next(18, 31) * 60;
				_direction = direction;
				_waviness = random.NextFloat() * 1f + 1f;
				Position.Y = camera.Position.Y;
				int num = 100;
				if (_direction == 1)
				{
					Position.X = camera.Position.X - (float)num;
				}
				else
				{
					Position.X = camera.Position.X + camera.Size.X + (float)num;
				}
			}

			private void BeginChasingPlayer(ref FastRandom random, VirtualCamera camera)
			{
				_state = 2;
				LifeTime = random.Next(18, 31) * 60;
				Position = camera.Position + camera.Size * new Vector2(random.NextFloat(), random.NextFloat());
			}

			public override void UpdateVelocity(int frameCount)
			{
				switch (_state)
				{
				case 1:
					ZigzagMove(frameCount);
					break;
				case 2:
					ChasePlayerTop(frameCount);
					break;
				}
				Rotation = Velocity.ToRotation();
			}

			private void ZigzagMove(int frameCount)
			{
				Velocity = new Vector2(_direction * 3, (float)Math.Cos((float)frameCount / 1200f * (MathF.PI * 2f)) * _waviness);
			}

			private void ChasePlayerTop(int frameCount)
			{
				Vector2 vector = Main.LocalPlayer.Center + new Vector2(0f, -500f) - Position;
				if (vector.Length() >= 100f)
				{
					Velocity.X += 0.1f * (float)Math.Sign(vector.X);
					Velocity.Y += 0.1f * (float)Math.Sign(vector.Y);
					Velocity = Vector2.Clamp(Velocity, new Vector2(-18f), new Vector2(18f));
				}
			}
		}

		private class MeteorSkyEntity : FadingSkyEntity
		{
			public MeteorSkyEntity(Player player, FastRandom random)
			{
				new VirtualCamera(player);
				Effects = ((random.Next(2) != 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
				Depth = random.NextFloat() * 3f + 3f;
				Texture = Main.Assets.Request<Texture2D>("Images/Backgrounds/Ambience/Meteor", (AssetRequestMode)1);
				Frame = new SpriteFrame(1, 4);
				Vector2 vector = (MathF.PI / 4f + random.NextFloat() * (MathF.PI / 2f)).ToRotationVector2();
				float num = (float)(Main.worldSurface * 16.0 - 0.0) / vector.Y;
				float num2 = 1200f;
				float num3 = num / num2;
				Vector2 vector2 = (Velocity = vector * num3);
				int num4 = 100;
				Vector2 vector3 = (Position = player.Center + new Vector2(random.Next(-num4, num4 + 1), random.Next(-num4, num4 + 1)) - Velocity * num2 * 0.5f);
				LifeTime = (int)num2;
				OpacityNormalizedTimeToFadeIn = 0.05f;
				OpacityNormalizedTimeToFadeOut = 0.95f;
				BrightnessLerper = 0.5f;
				FinalOpacityMultiplier = 1f;
				FramingSpeed = 5;
				Rotation = Velocity.ToRotation() + MathF.PI / 2f;
			}
		}

		private delegate SkyEntity EntityFactoryMethod(Player player, int seed);

		private bool _isActive;

		private readonly SlotVector<SkyEntity> _entities = new SlotVector<SkyEntity>(500);

		private int _frameCounter;

		public override void Activate(Vector2 position, params object[] args)
		{
			_isActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			_isActive = false;
		}

		private bool AnActiveSkyConflictsWithAmbience()
		{
			if (!SkyManager.Instance["MonolithMoonLord"].IsActive())
			{
				return SkyManager.Instance["MoonLord"].IsActive();
			}
			return true;
		}

		public override void Update(GameTime gameTime)
		{
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			if (Main.gamePaused)
			{
				return;
			}
			_frameCounter++;
			if (Main.netMode != 2 && AnActiveSkyConflictsWithAmbience() && SkyManager.Instance["Ambience"].IsActive())
			{
				SkyManager.Instance.Deactivate("Ambience");
			}
			foreach (ItemPair<SkyEntity> item in (IEnumerable<ItemPair<SkyEntity>>)_entities)
			{
				SkyEntity value = item.Value;
				value.Update(_frameCounter);
				if (!value.IsActive)
				{
					_entities.Remove(item.Id);
					if (Main.netMode != 2 && _entities.get_Count() == 0 && SkyManager.Instance["Ambience"].IsActive())
					{
						SkyManager.Instance.Deactivate("Ambience");
					}
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			if (Main.gameMenu && Main.netMode == 0 && SkyManager.Instance["Ambience"].IsActive())
			{
				_entities.Clear();
				SkyManager.Instance.Deactivate("Ambience");
			}
			foreach (ItemPair<SkyEntity> item in (IEnumerable<ItemPair<SkyEntity>>)_entities)
			{
				item.Value.Draw(spriteBatch, 3f, minDepth, maxDepth);
			}
		}

		public override bool IsActive()
		{
			return _isActive;
		}

		public override void Reset()
		{
		}

		public void Spawn(Player player, SkyEntityType type, int seed)
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			FastRandom random = new FastRandom(seed);
			switch (type)
			{
			case SkyEntityType.AirBalloon:
				_entities.Add((SkyEntity)new AirBalloonSkyEntity(player, random));
				break;
			case SkyEntityType.Airship:
				_entities.Add((SkyEntity)new AirshipSkyEntity(player, random));
				break;
			case SkyEntityType.BirdsV:
				_entities.Add((SkyEntity)new BirdsPackSkyEntity(player, random));
				break;
			case SkyEntityType.Eyeball:
				_entities.Add((SkyEntity)new EOCSkyEntity(player, random));
				break;
			case SkyEntityType.Meteor:
				_entities.Add((SkyEntity)new MeteorSkyEntity(player, random));
				break;
			case SkyEntityType.Wyvern:
				_entities.Add((SkyEntity)new WyvernSkyEntity(player, random));
				break;
			case SkyEntityType.Bats:
			{
				List<BatsGroupSkyEntity> list5 = BatsGroupSkyEntity.CreateGroup(player, random);
				for (int m = 0; m < list5.Count; m++)
				{
					_entities.Add((SkyEntity)list5[m]);
				}
				break;
			}
			case SkyEntityType.Butterflies:
				_entities.Add((SkyEntity)new ButterfliesSkyEntity(player, random));
				break;
			case SkyEntityType.LostKite:
				_entities.Add((SkyEntity)new LostKiteSkyEntity(player, random));
				break;
			case SkyEntityType.Vulture:
				_entities.Add((SkyEntity)new VultureSkyEntity(player, random));
				break;
			case SkyEntityType.PixiePosse:
				_entities.Add((SkyEntity)new PixiePosseSkyEntity(player, random));
				break;
			case SkyEntityType.Seagulls:
			{
				List<SeagullsGroupSkyEntity> list4 = SeagullsGroupSkyEntity.CreateGroup(player, random);
				for (int l = 0; l < list4.Count; l++)
				{
					_entities.Add((SkyEntity)list4[l]);
				}
				break;
			}
			case SkyEntityType.SlimeBalloons:
			{
				List<SlimeBalloonGroupSkyEntity> list3 = SlimeBalloonGroupSkyEntity.CreateGroup(player, random);
				for (int k = 0; k < list3.Count; k++)
				{
					_entities.Add((SkyEntity)list3[k]);
				}
				break;
			}
			case SkyEntityType.Gastropods:
			{
				List<GastropodGroupSkyEntity> list2 = GastropodGroupSkyEntity.CreateGroup(player, random);
				for (int j = 0; j < list2.Count; j++)
				{
					_entities.Add((SkyEntity)list2[j]);
				}
				break;
			}
			case SkyEntityType.Pegasus:
				_entities.Add((SkyEntity)new PegasusSkyEntity(player, random));
				break;
			case SkyEntityType.EaterOfSouls:
				_entities.Add((SkyEntity)new EOSSkyEntity(player, random));
				break;
			case SkyEntityType.Crimera:
				_entities.Add((SkyEntity)new CrimeraSkyEntity(player, random));
				break;
			case SkyEntityType.Hellbats:
			{
				List<HellBatsGoupSkyEntity> list = HellBatsGoupSkyEntity.CreateGroup(player, random);
				for (int i = 0; i < list.Count; i++)
				{
					_entities.Add((SkyEntity)list[i]);
				}
				break;
			}
			}
			if (Main.netMode != 2 && !AnActiveSkyConflictsWithAmbience() && !SkyManager.Instance["Ambience"].IsActive())
			{
				SkyManager.Instance.Activate("Ambience", default(Vector2));
			}
		}
	}
}
