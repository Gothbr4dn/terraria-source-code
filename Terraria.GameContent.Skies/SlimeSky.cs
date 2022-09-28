using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	public class SlimeSky : CustomSky
	{
		private struct Slime
		{
			private const int MAX_FRAMES = 4;

			private const int FRAME_RATE = 6;

			private Texture2D _texture;

			public Vector2 Position;

			public float Depth;

			public int FrameHeight;

			public int FrameWidth;

			public float Speed;

			public bool Active;

			private int _frame;

			public Texture2D Texture
			{
				get
				{
					return _texture;
				}
				set
				{
					_texture = value;
					FrameWidth = value.Width;
					FrameHeight = value.Height / 4;
				}
			}

			public int Frame
			{
				get
				{
					return _frame;
				}
				set
				{
					_frame = value % 24;
				}
			}

			public Rectangle GetSourceRectangle()
			{
				return new Rectangle(0, _frame / 6 * FrameHeight, FrameWidth, FrameHeight);
			}
		}

		private Asset<Texture2D>[] _textures;

		private Slime[] _slimes;

		private UnifiedRandom _random = new UnifiedRandom();

		private int _slimesRemaining;

		private bool _isActive;

		private bool _isLeaving;

		public override void OnLoad()
		{
			_textures = new Asset<Texture2D>[4];
			for (int i = 0; i < 4; i++)
			{
				_textures[i] = Main.Assets.Request<Texture2D>("Images/Misc/Sky_Slime_" + (i + 1), (AssetRequestMode)1);
			}
			GenerateSlimes();
		}

		private void GenerateSlimes()
		{
			_slimes = new Slime[Main.maxTilesY / 6];
			for (int i = 0; i < _slimes.Length; i++)
			{
				int num = (int)((double)Main.screenPosition.Y * 0.7 - (double)Main.screenHeight);
				int minValue = (int)((double)num - Main.worldSurface * 16.0);
				_slimes[i].Position = new Vector2(_random.Next(0, Main.maxTilesX) * 16, _random.Next(minValue, num));
				_slimes[i].Speed = 5f + 3f * (float)_random.NextDouble();
				_slimes[i].Depth = (float)i / (float)_slimes.Length * 1.75f + 1.6f;
				_slimes[i].Texture = _textures[_random.Next(2)].get_Value();
				if (_random.Next(60) == 0)
				{
					_slimes[i].Texture = _textures[3].get_Value();
					_slimes[i].Speed = 6f + 3f * (float)_random.NextDouble();
					_slimes[i].Depth += 0.5f;
				}
				else if (_random.Next(30) == 0)
				{
					_slimes[i].Texture = _textures[2].get_Value();
					_slimes[i].Speed = 6f + 2f * (float)_random.NextDouble();
				}
				_slimes[i].Active = true;
			}
			_slimesRemaining = _slimes.Length;
		}

		public override void Update(GameTime gameTime)
		{
			if (Main.gamePaused || !Main.hasFocus)
			{
				return;
			}
			for (int i = 0; i < _slimes.Length; i++)
			{
				if (!_slimes[i].Active)
				{
					continue;
				}
				_slimes[i].Frame++;
				_slimes[i].Position.Y += _slimes[i].Speed;
				if (!((double)_slimes[i].Position.Y > Main.worldSurface * 16.0))
				{
					continue;
				}
				if (!_isLeaving)
				{
					_slimes[i].Depth = (float)i / (float)_slimes.Length * 1.75f + 1.6f;
					_slimes[i].Position = new Vector2(_random.Next(0, Main.maxTilesX) * 16, -100f);
					_slimes[i].Texture = _textures[_random.Next(2)].get_Value();
					_slimes[i].Speed = 5f + 3f * (float)_random.NextDouble();
					if (_random.Next(60) == 0)
					{
						_slimes[i].Texture = _textures[3].get_Value();
						_slimes[i].Speed = 6f + 3f * (float)_random.NextDouble();
						_slimes[i].Depth += 0.5f;
					}
					else if (_random.Next(30) == 0)
					{
						_slimes[i].Texture = _textures[2].get_Value();
						_slimes[i].Speed = 6f + 2f * (float)_random.NextDouble();
					}
				}
				else
				{
					_slimes[i].Active = false;
					_slimesRemaining--;
				}
			}
			if (_slimesRemaining == 0)
			{
				_isActive = false;
			}
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (Main.screenPosition.Y > 10000f || Main.gameMenu)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < _slimes.Length; i++)
			{
				float depth = _slimes[i].Depth;
				if (num == -1 && depth < maxDepth)
				{
					num = i;
				}
				if (depth <= minDepth)
				{
					break;
				}
				num2 = i;
			}
			if (num == -1)
			{
				return;
			}
			Vector2 vector = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
			Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
			for (int j = num; j < num2; j++)
			{
				if (_slimes[j].Active)
				{
					Color color = new Color(Main.ColorOfTheSkies.ToVector4() * 0.9f + new Vector4(0.1f)) * 0.8f;
					float num3 = 1f;
					if (_slimes[j].Depth > 3f)
					{
						num3 = 0.6f;
					}
					else if ((double)_slimes[j].Depth > 2.5)
					{
						num3 = 0.7f;
					}
					else if (_slimes[j].Depth > 2f)
					{
						num3 = 0.8f;
					}
					else if ((double)_slimes[j].Depth > 1.5)
					{
						num3 = 0.9f;
					}
					num3 *= 0.8f;
					color = new Color((int)((float)(int)color.R * num3), (int)((float)(int)color.G * num3), (int)((float)(int)color.B * num3), (int)((float)(int)color.A * num3));
					Vector2 vector2 = new Vector2(1f / _slimes[j].Depth, 0.9f / _slimes[j].Depth);
					Vector2 position = _slimes[j].Position;
					position = (position - vector) * vector2 + vector - Main.screenPosition;
					position.X = (position.X + 500f) % 4000f;
					if (position.X < 0f)
					{
						position.X += 4000f;
					}
					position.X -= 500f;
					if (rectangle.Contains((int)position.X, (int)position.Y))
					{
						spriteBatch.Draw(_slimes[j].Texture, position, _slimes[j].GetSourceRectangle(), color, 0f, Vector2.Zero, vector2.X * 2f, SpriteEffects.None, 0f);
					}
				}
			}
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			GenerateSlimes();
			_isActive = true;
			_isLeaving = false;
		}

		public override void Deactivate(params object[] args)
		{
			_isLeaving = true;
		}

		public override void Reset()
		{
			_isActive = false;
		}

		public override bool IsActive()
		{
			return _isActive;
		}
	}
}
