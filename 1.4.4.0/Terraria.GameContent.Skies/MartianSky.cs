using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	public class MartianSky : CustomSky
	{
		private abstract class IUfoController
		{
			public abstract void InitializeUfo(ref Ufo ufo);

			public abstract bool Update(ref Ufo ufo);
		}

		private class ZipBehavior : IUfoController
		{
			private Vector2 _speed;

			private int _ticks;

			private int _maxTicks;

			public override void InitializeUfo(ref Ufo ufo)
			{
				ufo.Position.X = (float)(Ufo.Random.NextDouble() * (double)(Main.maxTilesX << 4));
				ufo.Position.Y = (float)(Ufo.Random.NextDouble() * 5000.0);
				ufo.Opacity = 0f;
				float num = (float)Ufo.Random.NextDouble() * 5f + 10f;
				double num2 = Ufo.Random.NextDouble() * 0.6000000238418579 - 0.30000001192092896;
				ufo.Rotation = (float)num2;
				if (Ufo.Random.Next(2) == 0)
				{
					num2 += 3.1415927410125732;
				}
				_speed = new Vector2((float)Math.Cos(num2) * num, (float)Math.Sin(num2) * num);
				_ticks = 0;
				_maxTicks = Ufo.Random.Next(400, 500);
			}

			public override bool Update(ref Ufo ufo)
			{
				if (_ticks < 10)
				{
					ufo.Opacity += 0.1f;
				}
				else if (_ticks > _maxTicks - 10)
				{
					ufo.Opacity -= 0.1f;
				}
				ufo.Position += _speed;
				if (_ticks == _maxTicks)
				{
					return false;
				}
				_ticks++;
				return true;
			}
		}

		private class HoverBehavior : IUfoController
		{
			private int _ticks;

			private int _maxTicks;

			public override void InitializeUfo(ref Ufo ufo)
			{
				ufo.Position.X = (float)(Ufo.Random.NextDouble() * (double)(Main.maxTilesX << 4));
				ufo.Position.Y = (float)(Ufo.Random.NextDouble() * 5000.0);
				ufo.Opacity = 0f;
				ufo.Rotation = 0f;
				_ticks = 0;
				_maxTicks = Ufo.Random.Next(120, 240);
			}

			public override bool Update(ref Ufo ufo)
			{
				if (_ticks < 10)
				{
					ufo.Opacity += 0.1f;
				}
				else if (_ticks > _maxTicks - 10)
				{
					ufo.Opacity -= 0.1f;
				}
				if (_ticks == _maxTicks)
				{
					return false;
				}
				_ticks++;
				return true;
			}
		}

		private struct Ufo
		{
			private const int MAX_FRAMES = 3;

			private const int FRAME_RATE = 4;

			public static UnifiedRandom Random = new UnifiedRandom();

			private int _frame;

			private Texture2D _texture;

			private IUfoController _controller;

			public Texture2D GlowTexture;

			public Vector2 Position;

			public int FrameHeight;

			public int FrameWidth;

			public float Depth;

			public float Scale;

			public float Opacity;

			public bool IsActive;

			public float Rotation;

			public int Frame
			{
				get
				{
					return _frame;
				}
				set
				{
					_frame = value % 12;
				}
			}

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
					FrameHeight = value.Height / 3;
				}
			}

			public IUfoController Controller
			{
				get
				{
					return _controller;
				}
				set
				{
					_controller = value;
					value.InitializeUfo(ref this);
				}
			}

			public Ufo(Texture2D texture, float depth = 1f)
			{
				_frame = 0;
				Position = Vector2.Zero;
				_texture = texture;
				Depth = depth;
				Scale = 1f;
				FrameWidth = texture.Width;
				FrameHeight = texture.Height / 3;
				GlowTexture = null;
				Opacity = 0f;
				Rotation = 0f;
				IsActive = false;
				_controller = null;
			}

			public Rectangle GetSourceRectangle()
			{
				return new Rectangle(0, _frame / 4 * FrameHeight, FrameWidth, FrameHeight);
			}

			public bool Update()
			{
				return Controller.Update(ref this);
			}

			public void AssignNewBehavior()
			{
				switch (Random.Next(2))
				{
				case 0:
					Controller = new ZipBehavior();
					break;
				case 1:
					Controller = new HoverBehavior();
					break;
				}
			}
		}

		private Ufo[] _ufos;

		private UnifiedRandom _random = new UnifiedRandom();

		private int _maxUfos;

		private bool _active;

		private bool _leaving;

		private int _activeUfos;

		public override void Update(GameTime gameTime)
		{
			if (Main.gamePaused || !Main.hasFocus)
			{
				return;
			}
			int num = _activeUfos;
			for (int i = 0; i < _ufos.Length; i++)
			{
				Ufo ufo = _ufos[i];
				if (ufo.IsActive)
				{
					ufo.Frame++;
					if (!ufo.Update())
					{
						if (!_leaving)
						{
							ufo.AssignNewBehavior();
						}
						else
						{
							ufo.IsActive = false;
							num--;
						}
					}
				}
				_ufos[i] = ufo;
			}
			if (!_leaving && num != _maxUfos)
			{
				_ufos[num].IsActive = true;
				_ufos[num++].AssignNewBehavior();
			}
			_active = !_leaving || num != 0;
			_activeUfos = num;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (Main.screenPosition.Y > 10000f)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < _ufos.Length; i++)
			{
				float depth = _ufos[i].Depth;
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
			Color color = new Color(Main.ColorOfTheSkies.ToVector4() * 0.9f + new Vector4(0.1f));
			Vector2 vector = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
			Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
			for (int j = num; j < num2; j++)
			{
				Vector2 vector2 = new Vector2(1f / _ufos[j].Depth, 0.9f / _ufos[j].Depth);
				Vector2 position = _ufos[j].Position;
				position = (position - vector) * vector2 + vector - Main.screenPosition;
				if (_ufos[j].IsActive && rectangle.Contains((int)position.X, (int)position.Y))
				{
					spriteBatch.Draw(_ufos[j].Texture, position, _ufos[j].GetSourceRectangle(), color * _ufos[j].Opacity, _ufos[j].Rotation, Vector2.Zero, vector2.X * 5f * _ufos[j].Scale, SpriteEffects.None, 0f);
					if (_ufos[j].GlowTexture != null)
					{
						spriteBatch.Draw(_ufos[j].GlowTexture, position, _ufos[j].GetSourceRectangle(), Color.White * _ufos[j].Opacity, _ufos[j].Rotation, Vector2.Zero, vector2.X * 5f * _ufos[j].Scale, SpriteEffects.None, 0f);
					}
				}
			}
		}

		private void GenerateUfos()
		{
			float num = (float)Main.maxTilesX / 4200f;
			_maxUfos = (int)(256f * num);
			_ufos = new Ufo[_maxUfos];
			int num2 = _maxUfos >> 4;
			for (int i = 0; i < num2; i++)
			{
				_ = (float)i / (float)num2;
				_ufos[i] = new Ufo(TextureAssets.Extra[5].get_Value(), (float)Main.rand.NextDouble() * 4f + 6.6f);
				_ufos[i].GlowTexture = TextureAssets.GlowMask[90].get_Value();
			}
			for (int j = num2; j < _ufos.Length; j++)
			{
				_ = (float)(j - num2) / (float)(_ufos.Length - num2);
				_ufos[j] = new Ufo(TextureAssets.Extra[6].get_Value(), (float)Main.rand.NextDouble() * 5f + 1.6f);
				_ufos[j].Scale = 0.5f;
				_ufos[j].GlowTexture = TextureAssets.GlowMask[91].get_Value();
			}
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			_activeUfos = 0;
			GenerateUfos();
			Array.Sort(_ufos, (Ufo ufo1, Ufo ufo2) => ufo2.Depth.CompareTo(ufo1.Depth));
			_active = true;
			_leaving = false;
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
