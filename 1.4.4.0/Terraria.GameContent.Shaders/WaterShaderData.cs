using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.Graphics.Light;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria.GameContent.Shaders
{
	public class WaterShaderData : ScreenShaderData
	{
		private struct Ripple
		{
			private static readonly Rectangle[] RIPPLE_SHAPE_SOURCE_RECTS = new Rectangle[3]
			{
				new Rectangle(0, 0, 0, 0),
				new Rectangle(1, 1, 62, 62),
				new Rectangle(1, 65, 62, 62)
			};

			public readonly Vector2 Position;

			public readonly Color WaveData;

			public readonly Vector2 Size;

			public readonly RippleShape Shape;

			public readonly float Rotation;

			public Rectangle SourceRectangle => RIPPLE_SHAPE_SOURCE_RECTS[(int)Shape];

			public Ripple(Vector2 position, Color waveData, Vector2 size, RippleShape shape, float rotation)
			{
				Position = position;
				WaveData = waveData;
				Size = size;
				Shape = shape;
				Rotation = rotation;
			}
		}

		private const float DISTORTION_BUFFER_SCALE = 0.25f;

		private const float WAVE_FRAMERATE = 1f / 60f;

		private const int MAX_RIPPLES_QUEUED = 200;

		public bool _useViscosityFilter = true;

		private RenderTarget2D _distortionTarget;

		private RenderTarget2D _distortionTargetSwap;

		private bool _usingRenderTargets;

		private Vector2 _lastDistortionDrawOffset = Vector2.Zero;

		private float _progress;

		private Ripple[] _rippleQueue = new Ripple[200];

		private int _rippleQueueCount;

		private int _lastScreenWidth;

		private int _lastScreenHeight;

		public bool _useProjectileWaves = true;

		private bool _useNPCWaves = true;

		private bool _usePlayerWaves = true;

		private bool _useRippleWaves = true;

		private bool _useCustomWaves = true;

		private bool _clearNextFrame = true;

		private Texture2D[] _viscosityMaskChain = new Texture2D[3];

		private int _activeViscosityMask;

		private Asset<Texture2D> _rippleShapeTexture;

		private bool _isWaveBufferDirty = true;

		private int _queuedSteps;

		private const int MAX_QUEUED_STEPS = 2;

		public event Action<TileBatch> OnWaveDraw;

		public WaterShaderData(string passName)
			: base(passName)
		{
			Main.OnRenderTargetsInitialized += InitRenderTargets;
			Main.OnRenderTargetsReleased += ReleaseRenderTargets;
			_rippleShapeTexture = Main.Assets.Request<Texture2D>("Images/Misc/Ripples", (AssetRequestMode)1);
			Main.OnPreDraw += PreDraw;
		}

		public override void Update(GameTime gameTime)
		{
			_useViscosityFilter = Main.WaveQuality >= 3;
			_useProjectileWaves = Main.WaveQuality >= 3;
			_usePlayerWaves = Main.WaveQuality >= 2;
			_useRippleWaves = Main.WaveQuality >= 2;
			_useCustomWaves = Main.WaveQuality >= 2;
			if (!Main.gamePaused && Main.hasFocus)
			{
				_progress += (float)gameTime.ElapsedGameTime.TotalSeconds * base.Intensity * 0.75f;
				_progress %= 86400f;
				if (_useProjectileWaves || _useRippleWaves || _useCustomWaves || _usePlayerWaves)
				{
					_queuedSteps++;
				}
				base.Update(gameTime);
			}
		}

		private void StepLiquids()
		{
			_isWaveBufferDirty = true;
			Vector2 vector = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange));
			Vector2 vector2 = vector - Main.screenPosition;
			TileBatch tileBatch = Main.tileBatch;
			GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
			graphicsDevice.SetRenderTarget(_distortionTarget);
			if (_clearNextFrame)
			{
				graphicsDevice.Clear(new Color(0.5f, 0.5f, 0f, 1f));
				_clearNextFrame = false;
			}
			DrawWaves();
			graphicsDevice.SetRenderTarget(_distortionTargetSwap);
			graphicsDevice.Clear(new Color(0.5f, 0.5f, 0.5f, 1f));
			Main.tileBatch.Begin();
			vector2 *= 0.25f;
			vector2.X = (float)Math.Floor(vector2.X);
			vector2.Y = (float)Math.Floor(vector2.Y);
			Vector2 vector3 = vector2 - _lastDistortionDrawOffset;
			_lastDistortionDrawOffset = vector2;
			tileBatch.Draw(_distortionTarget, new Vector4(vector3.X, vector3.Y, _distortionTarget.Width, _distortionTarget.Height), new VertexColors(Color.White));
			GameShaders.Misc["WaterProcessor"].Apply(new DrawData(_distortionTarget, Vector2.Zero, Color.White));
			tileBatch.End();
			RenderTarget2D distortionTarget = _distortionTarget;
			_distortionTarget = _distortionTargetSwap;
			_distortionTargetSwap = distortionTarget;
			if (_useViscosityFilter)
			{
				LiquidRenderer.Instance.SetWaveMaskData(ref _viscosityMaskChain[_activeViscosityMask]);
				tileBatch.Begin();
				Rectangle cachedDrawArea = LiquidRenderer.Instance.GetCachedDrawArea();
				Rectangle rectangle = new Rectangle(0, 0, cachedDrawArea.Height, cachedDrawArea.Width);
				Vector4 destination = new Vector4(cachedDrawArea.X + cachedDrawArea.Width, cachedDrawArea.Y, cachedDrawArea.Height, cachedDrawArea.Width);
				destination *= 16f;
				destination.X -= vector.X;
				destination.Y -= vector.Y;
				destination *= 0.25f;
				destination.X += vector2.X;
				destination.Y += vector2.Y;
				graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
				tileBatch.Draw(_viscosityMaskChain[_activeViscosityMask], destination, rectangle, new VertexColors(Color.White), rectangle.Size(), SpriteEffects.FlipHorizontally, -MathF.PI / 2f);
				tileBatch.End();
				_activeViscosityMask++;
				_activeViscosityMask %= _viscosityMaskChain.Length;
			}
			graphicsDevice.SetRenderTarget(null);
		}

		private void DrawWaves()
		{
			Vector2 screenPosition = Main.screenPosition;
			Vector2 vector = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange));
			Vector2 vector2 = -_lastDistortionDrawOffset / 0.25f + vector;
			TileBatch tileBatch = Main.tileBatch;
			_ = Main.instance.GraphicsDevice;
			Vector2 dimensions = new Vector2(Main.screenWidth, Main.screenHeight);
			Vector2 vector3 = new Vector2(16f, 16f);
			tileBatch.Begin();
			GameShaders.Misc["WaterDistortionObject"].Apply();
			if (_useNPCWaves)
			{
				for (int i = 0; i < 200; i++)
				{
					if (Main.npc[i] == null || !Main.npc[i].active || (!Main.npc[i].wet && Main.npc[i].wetCount == 0) || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, Main.npc[i].position - vector3, Main.npc[i].Size + vector3))
					{
						continue;
					}
					NPC nPC = Main.npc[i];
					Vector2 vector4 = nPC.Center - vector2;
					Vector2 vector5 = nPC.velocity.RotatedBy(0f - nPC.rotation) / new Vector2(nPC.height, nPC.width);
					float num = vector5.LengthSquared();
					num = num * 0.3f + 0.7f * num * (1024f / (float)(nPC.height * nPC.width));
					num = Math.Min(num, 0.08f);
					num += (nPC.velocity - nPC.oldVelocity).Length() * 0.5f;
					vector5.Normalize();
					Vector2 velocity = nPC.velocity;
					velocity.Normalize();
					vector4 -= velocity * 10f;
					if (!_useViscosityFilter && (nPC.honeyWet || nPC.lavaWet))
					{
						num *= 0.3f;
					}
					if (nPC.wet)
					{
						tileBatch.Draw(TextureAssets.MagicPixel.get_Value(), new Vector4(vector4.X, vector4.Y, (float)nPC.width * 2f, (float)nPC.height * 2f) * 0.25f, null, new VertexColors(new Color(vector5.X * 0.5f + 0.5f, vector5.Y * 0.5f + 0.5f, 0.5f * num)), new Vector2((float)TextureAssets.MagicPixel.Width() / 2f, (float)TextureAssets.MagicPixel.Height() / 2f), SpriteEffects.None, nPC.rotation);
					}
					if (nPC.wetCount != 0)
					{
						num = nPC.velocity.Length();
						num = 0.195f * (float)Math.Sqrt(num);
						float num2 = 5f;
						if (!nPC.wet)
						{
							num2 = -20f;
						}
						QueueRipple(nPC.Center + velocity * num2, new Color(0.5f, (nPC.wet ? num : (0f - num)) * 0.5f + 0.5f, 0f, 1f) * 0.5f, new Vector2(nPC.width, (float)nPC.height * ((float)(int)nPC.wetCount / 9f)) * MathHelper.Clamp(num * 10f, 0f, 1f), RippleShape.Circle);
					}
				}
			}
			if (_usePlayerWaves)
			{
				for (int j = 0; j < 255; j++)
				{
					if (Main.player[j] == null || !Main.player[j].active || (!Main.player[j].wet && Main.player[j].wetCount == 0) || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, Main.player[j].position - vector3, Main.player[j].Size + vector3))
					{
						continue;
					}
					Player player = Main.player[j];
					Vector2 vector6 = player.Center - vector2;
					float num3 = player.velocity.Length();
					num3 = 0.05f * (float)Math.Sqrt(num3);
					Vector2 velocity2 = player.velocity;
					velocity2.Normalize();
					vector6 -= velocity2 * 10f;
					if (!_useViscosityFilter && (player.honeyWet || player.lavaWet))
					{
						num3 *= 0.3f;
					}
					if (player.wet)
					{
						tileBatch.Draw(TextureAssets.MagicPixel.get_Value(), new Vector4(vector6.X - (float)player.width * 2f * 0.5f, vector6.Y - (float)player.height * 2f * 0.5f, (float)player.width * 2f, (float)player.height * 2f) * 0.25f, new VertexColors(new Color(velocity2.X * 0.5f + 0.5f, velocity2.Y * 0.5f + 0.5f, 0.5f * num3)));
					}
					if (player.wetCount != 0)
					{
						float num4 = 5f;
						if (!player.wet)
						{
							num4 = -20f;
						}
						num3 *= 3f;
						QueueRipple(player.Center + velocity2 * num4, player.wet ? num3 : (0f - num3), new Vector2(player.width, (float)player.height * ((float)(int)player.wetCount / 9f)) * MathHelper.Clamp(num3 * 10f, 0f, 1f), RippleShape.Circle);
					}
				}
			}
			if (_useProjectileWaves)
			{
				for (int k = 0; k < 1000; k++)
				{
					Projectile projectile = Main.projectile[k];
					if (projectile.wet && !projectile.lavaWet)
					{
						_ = !projectile.honeyWet;
					}
					else
						_ = 0;
					bool flag = projectile.lavaWet;
					bool flag2 = projectile.honeyWet;
					bool flag3 = projectile.wet;
					if (projectile.ignoreWater)
					{
						flag3 = true;
					}
					if (!(projectile != null && projectile.active && ProjectileID.Sets.CanDistortWater[projectile.type] && flag3) || ProjectileID.Sets.NoLiquidDistortion[projectile.type] || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, projectile.position - vector3, projectile.Size + vector3))
					{
						continue;
					}
					if (projectile.ignoreWater)
					{
						bool num5 = Collision.LavaCollision(projectile.position, projectile.width, projectile.height);
						flag = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
						flag2 = Collision.honey;
						if (!(num5 || flag || flag2))
						{
							continue;
						}
					}
					Vector2 vector7 = projectile.Center - vector2;
					float num6 = projectile.velocity.Length();
					num6 = 2f * (float)Math.Sqrt(0.05f * num6);
					Vector2 velocity3 = projectile.velocity;
					velocity3.Normalize();
					if (!_useViscosityFilter && (flag2 || flag))
					{
						num6 *= 0.3f;
					}
					float num7 = Math.Max(12f, (float)projectile.width * 0.75f);
					float num8 = Math.Max(12f, (float)projectile.height * 0.75f);
					tileBatch.Draw(TextureAssets.MagicPixel.get_Value(), new Vector4(vector7.X - num7 * 0.5f, vector7.Y - num8 * 0.5f, num7, num8) * 0.25f, new VertexColors(new Color(velocity3.X * 0.5f + 0.5f, velocity3.Y * 0.5f + 0.5f, num6 * 0.5f)));
				}
			}
			tileBatch.End();
			if (_useRippleWaves)
			{
				tileBatch.Begin();
				for (int l = 0; l < _rippleQueueCount; l++)
				{
					Vector2 vector8 = _rippleQueue[l].Position - vector2;
					Vector2 size = _rippleQueue[l].Size;
					Rectangle sourceRectangle = _rippleQueue[l].SourceRectangle;
					Texture2D value = _rippleShapeTexture.get_Value();
					tileBatch.Draw(value, new Vector4(vector8.X, vector8.Y, size.X, size.Y) * 0.25f, sourceRectangle, new VertexColors(_rippleQueue[l].WaveData), new Vector2(sourceRectangle.Width / 2, sourceRectangle.Height / 2), SpriteEffects.None, _rippleQueue[l].Rotation);
				}
				tileBatch.End();
			}
			_rippleQueueCount = 0;
			if (_useCustomWaves && this.OnWaveDraw != null)
			{
				tileBatch.Begin();
				this.OnWaveDraw(tileBatch);
				tileBatch.End();
			}
		}

		private void PreDraw(GameTime gameTime)
		{
			ValidateRenderTargets();
			if (!_usingRenderTargets || !Main.IsGraphicsDeviceAvailable)
			{
				return;
			}
			if (_useProjectileWaves || _useRippleWaves || _useCustomWaves || _usePlayerWaves)
			{
				for (int i = 0; i < Math.Min(_queuedSteps, 2); i++)
				{
					StepLiquids();
				}
			}
			else if (_isWaveBufferDirty || _clearNextFrame)
			{
				GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
				graphicsDevice.SetRenderTarget(_distortionTarget);
				graphicsDevice.Clear(new Color(0.5f, 0.5f, 0f, 1f));
				_clearNextFrame = false;
				_isWaveBufferDirty = false;
				graphicsDevice.SetRenderTarget(null);
			}
			_queuedSteps = 0;
		}

		public override void Apply()
		{
			if (_usingRenderTargets && Main.IsGraphicsDeviceAvailable)
			{
				UseProgress(_progress);
				Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
				Vector2 vector = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
				Vector2 vector2 = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)) - Main.screenPosition - vector;
				UseImage(_distortionTarget, 1);
				UseImage(Main.waterTarget, 2, SamplerState.PointClamp);
				UseTargetPosition(Main.screenPosition - Main.sceneWaterPos + new Vector2(Main.offScreenRange, Main.offScreenRange) + vector);
				UseImageOffset(-(vector2 * 0.25f - _lastDistortionDrawOffset) / new Vector2(_distortionTarget.Width, _distortionTarget.Height));
				base.Apply();
			}
		}

		private void ValidateRenderTargets()
		{
			int backBufferWidth = Main.instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
			int backBufferHeight = Main.instance.GraphicsDevice.PresentationParameters.BackBufferHeight;
			bool flag = !Main.drawToScreen;
			if (_usingRenderTargets && !flag)
			{
				ReleaseRenderTargets();
			}
			else if (!_usingRenderTargets && flag)
			{
				InitRenderTargets(backBufferWidth, backBufferHeight);
			}
			else if (_usingRenderTargets && flag && (_distortionTarget.IsContentLost || _distortionTargetSwap.IsContentLost))
			{
				_clearNextFrame = true;
			}
		}

		private void InitRenderTargets(int width, int height)
		{
			_lastScreenWidth = width;
			_lastScreenHeight = height;
			width = (int)((float)width * 0.25f);
			height = (int)((float)height * 0.25f);
			try
			{
				_distortionTarget = new RenderTarget2D(Main.instance.GraphicsDevice, width, height, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				_distortionTargetSwap = new RenderTarget2D(Main.instance.GraphicsDevice, width, height, mipMap: false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
				_usingRenderTargets = true;
				_clearNextFrame = true;
			}
			catch (Exception ex)
			{
				Lighting.Mode = LightMode.Retro;
				_usingRenderTargets = false;
				Console.WriteLine("Failed to create water distortion render targets. " + ex);
			}
		}

		private void ReleaseRenderTargets()
		{
			try
			{
				if (_distortionTarget != null)
				{
					_distortionTarget.Dispose();
				}
				if (_distortionTargetSwap != null)
				{
					_distortionTargetSwap.Dispose();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error disposing of water distortion render targets. " + ex);
			}
			_distortionTarget = null;
			_distortionTargetSwap = null;
			_usingRenderTargets = false;
		}

		public void QueueRipple(Vector2 position, float strength = 1f, RippleShape shape = RippleShape.Square, float rotation = 0f)
		{
			float g = strength * 0.5f + 0.5f;
			float num = Math.Min(Math.Abs(strength), 1f);
			QueueRipple(position, new Color(0.5f, g, 0f, 1f) * num, new Vector2(4f * Math.Max(Math.Abs(strength), 1f)), shape, rotation);
		}

		public void QueueRipple(Vector2 position, float strength, Vector2 size, RippleShape shape = RippleShape.Square, float rotation = 0f)
		{
			float g = strength * 0.5f + 0.5f;
			float num = Math.Min(Math.Abs(strength), 1f);
			QueueRipple(position, new Color(0.5f, g, 0f, 1f) * num, size, shape, rotation);
		}

		public void QueueRipple(Vector2 position, Color waveData, Vector2 size, RippleShape shape = RippleShape.Square, float rotation = 0f)
		{
			if (!_useRippleWaves || Main.drawToScreen)
			{
				_rippleQueueCount = 0;
			}
			else if (_rippleQueueCount < _rippleQueue.Length)
			{
				_rippleQueue[_rippleQueueCount++] = new Ripple(position, waveData, size, shape, rotation);
			}
		}
	}
}
