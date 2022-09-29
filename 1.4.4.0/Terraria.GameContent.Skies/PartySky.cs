using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	public class PartySky : CustomSky
	{
		private struct Balloon
		{
			private const int MAX_FRAMES_X = 3;

			private const int MAX_FRAMES_Y = 3;

			private const int FRAME_RATE = 14;

			public int Variant;

			private Texture2D _texture;

			public Vector2 Position;

			public float Depth;

			public int FrameHeight;

			public int FrameWidth;

			public float Speed;

			public bool Active;

			private int _frameCounter;

			public Texture2D Texture
			{
				get
				{
					return _texture;
				}
				set
				{
					_texture = value;
					FrameWidth = value.Width / 3;
					FrameHeight = value.Height / 3;
				}
			}

			public int Frame
			{
				get
				{
					return _frameCounter;
				}
				set
				{
					_frameCounter = value % 42;
				}
			}

			public Rectangle GetSourceRectangle()
			{
				return new Rectangle(FrameWidth * Variant, _frameCounter / 14 * FrameHeight, FrameWidth, FrameHeight);
			}
		}

		public static bool MultipleSkyWorkaroundFix;

		private bool _active;

		private bool _leaving;

		private float _opacity;

		private Asset<Texture2D>[] _textures;

		private Balloon[] _balloons;

		private UnifiedRandom _random = new UnifiedRandom();

		private int _balloonsDrawing;

		public override void OnLoad()
		{
			_textures = new Asset<Texture2D>[3];
			for (int i = 0; i < _textures.Length; i++)
			{
				_textures[i] = TextureAssets.Extra[69 + i];
			}
			GenerateBalloons(onlyMissing: false);
		}

		private void GenerateBalloons(bool onlyMissing)
		{
			if (!onlyMissing)
			{
				_balloons = new Balloon[Main.maxTilesY / 4];
			}
			for (int i = 0; i < _balloons.Length; i++)
			{
				if (!onlyMissing || !_balloons[i].Active)
				{
					int num = (int)((double)Main.screenPosition.Y * 0.7 - (double)Main.screenHeight);
					int minValue = (int)((double)num - Main.worldSurface * 16.0);
					_balloons[i].Position = new Vector2(_random.Next(0, Main.maxTilesX) * 16, _random.Next(minValue, num));
					ResetBalloon(i);
					_balloons[i].Active = true;
				}
			}
			_balloonsDrawing = _balloons.Length;
		}

		public void ResetBalloon(int i)
		{
			_balloons[i].Depth = (float)i / (float)_balloons.Length * 1.75f + 1.6f;
			_balloons[i].Speed = -1.5f - 2.5f * (float)_random.NextDouble();
			_balloons[i].Texture = _textures[_random.Next(2)].get_Value();
			_balloons[i].Variant = _random.Next(3);
			if (_random.Next(30) == 0)
			{
				_balloons[i].Texture = _textures[2].get_Value();
			}
		}

		private bool IsNearParty()
		{
			if (!(Main.player[Main.myPlayer].townNPCs > 0f))
			{
				return Main.SceneMetrics.PartyMonolithCount > 0;
			}
			return true;
		}

		public override void Update(GameTime gameTime)
		{
			if (!MultipleSkyWorkaroundFix && Main.dayRate == 0)
			{
				return;
			}
			MultipleSkyWorkaroundFix = false;
			if (Main.gamePaused || !Main.hasFocus)
			{
				return;
			}
			_opacity = Utils.Clamp(_opacity + (float)IsNearParty().ToDirectionInt() * 0.01f, 0f, 1f);
			for (int i = 0; i < _balloons.Length; i++)
			{
				if (!_balloons[i].Active)
				{
					continue;
				}
				_balloons[i].Frame++;
				_balloons[i].Position.Y += _balloons[i].Speed;
				_balloons[i].Position.X += Main.windSpeedCurrent * (3f - _balloons[i].Speed);
				if (!(_balloons[i].Position.Y < 300f))
				{
					continue;
				}
				if (!_leaving)
				{
					ResetBalloon(i);
					_balloons[i].Position = new Vector2(_random.Next(0, Main.maxTilesX) * 16, (float)Main.worldSurface * 16f + 1600f);
					if (_random.Next(30) == 0)
					{
						_balloons[i].Texture = _textures[2].get_Value();
					}
				}
				else
				{
					_balloons[i].Active = false;
					_balloonsDrawing--;
				}
			}
			if (_balloonsDrawing == 0)
			{
				_active = false;
			}
			_active = true;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (Main.gameMenu && _active)
			{
				_active = false;
				_leaving = false;
				for (int i = 0; i < _balloons.Length; i++)
				{
					_balloons[i].Active = false;
				}
			}
			if ((double)Main.screenPosition.Y > Main.worldSurface * 16.0 || Main.gameMenu || _opacity <= 0f)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			for (int j = 0; j < _balloons.Length; j++)
			{
				float depth = _balloons[j].Depth;
				if (num == -1 && depth < maxDepth)
				{
					num = j;
				}
				if (depth <= minDepth)
				{
					break;
				}
				num2 = j;
			}
			if (num == -1)
			{
				return;
			}
			Vector2 vector = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
			Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
			for (int k = num; k < num2; k++)
			{
				if (_balloons[k].Active)
				{
					Color color = new Color(Main.ColorOfTheSkies.ToVector4() * 0.9f + new Vector4(0.1f)) * 0.8f;
					float num3 = 1f;
					if (_balloons[k].Depth > 3f)
					{
						num3 = 0.6f;
					}
					else if ((double)_balloons[k].Depth > 2.5)
					{
						num3 = 0.7f;
					}
					else if (_balloons[k].Depth > 2f)
					{
						num3 = 0.8f;
					}
					else if ((double)_balloons[k].Depth > 1.5)
					{
						num3 = 0.9f;
					}
					num3 *= 0.9f;
					color = new Color((int)((float)(int)color.R * num3), (int)((float)(int)color.G * num3), (int)((float)(int)color.B * num3), (int)((float)(int)color.A * num3));
					Vector2 vector2 = new Vector2(1f / _balloons[k].Depth, 0.9f / _balloons[k].Depth);
					Vector2 position = _balloons[k].Position;
					position = (position - vector) * vector2 + vector - Main.screenPosition;
					position.X = (position.X + 500f) % 4000f;
					if (position.X < 0f)
					{
						position.X += 4000f;
					}
					position.X -= 500f;
					if (rectangle.Contains((int)position.X, (int)position.Y))
					{
						spriteBatch.Draw(_balloons[k].Texture, position, _balloons[k].GetSourceRectangle(), color * _opacity, 0f, Vector2.Zero, vector2.X * 2f, SpriteEffects.None, 0f);
					}
				}
			}
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			if (_active)
			{
				_leaving = false;
				GenerateBalloons(onlyMissing: true);
			}
			else
			{
				GenerateBalloons(onlyMissing: false);
				_active = true;
				_leaving = false;
			}
		}

		public override void Deactivate(params object[] args)
		{
			_leaving = true;
		}

		public override bool IsActive()
		{
			return _active;
		}

		public override void Reset()
		{
			_active = false;
		}
	}
}
