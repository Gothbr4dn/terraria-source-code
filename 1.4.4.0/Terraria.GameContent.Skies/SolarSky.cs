using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	public class SolarSky : CustomSky
	{
		private struct Meteor
		{
			public Vector2 Position;

			public float Depth;

			public int FrameCounter;

			public float Scale;

			public float StartX;
		}

		private UnifiedRandom _random = new UnifiedRandom();

		private Asset<Texture2D> _planetTexture;

		private Asset<Texture2D> _bgTexture;

		private Asset<Texture2D> _meteorTexture;

		private bool _isActive;

		private Meteor[] _meteors;

		private float _fadeOpacity;

		public override void OnLoad()
		{
			_planetTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Planet", (AssetRequestMode)1);
			_bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Background", (AssetRequestMode)1);
			_meteorTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Meteor", (AssetRequestMode)1);
		}

		public override void Update(GameTime gameTime)
		{
			if (_isActive)
			{
				_fadeOpacity = Math.Min(1f, 0.01f + _fadeOpacity);
			}
			else
			{
				_fadeOpacity = Math.Max(0f, _fadeOpacity - 0.01f);
			}
			float num = 1200f;
			for (int i = 0; i < _meteors.Length; i++)
			{
				_meteors[i].Position.X -= num * (float)gameTime.ElapsedGameTime.TotalSeconds;
				_meteors[i].Position.Y += num * (float)gameTime.ElapsedGameTime.TotalSeconds;
				if ((double)_meteors[i].Position.Y > Main.worldSurface * 16.0)
				{
					_meteors[i].Position.X = _meteors[i].StartX;
					_meteors[i].Position.Y = -10000f;
				}
			}
		}

		public override Color OnTileColor(Color inColor)
		{
			return new Color(Vector4.Lerp(inColor.ToVector4(), Vector4.One, _fadeOpacity * 0.5f));
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= float.MaxValue && minDepth < float.MaxValue)
			{
				spriteBatch.Draw(TextureAssets.BlackTile.get_Value(), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * _fadeOpacity);
				spriteBatch.Draw(_bgTexture.get_Value(), new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 2400.0) * 0.10000000149011612)), Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f * _fadeOpacity));
				Vector2 vector = new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
				Vector2 vector2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
				spriteBatch.Draw(_planetTexture.get_Value(), vector + new Vector2(-200f, -200f) + vector2, null, Color.White * 0.9f * _fadeOpacity, 0f, new Vector2(_planetTexture.Width() >> 1, _planetTexture.Height() >> 1), 1f, SpriteEffects.None, 1f);
			}
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < _meteors.Length; i++)
			{
				float depth = _meteors[i].Depth;
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
			float num3 = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
			Vector2 vector3 = Main.screenPosition + new Vector2(Main.screenWidth >> 1, Main.screenHeight >> 1);
			Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 4000);
			for (int j = num; j < num2; j++)
			{
				Vector2 vector4 = new Vector2(1f / _meteors[j].Depth, 0.9f / _meteors[j].Depth);
				Vector2 position = (_meteors[j].Position - vector3) * vector4 + vector3 - Main.screenPosition;
				int num4 = _meteors[j].FrameCounter / 3;
				_meteors[j].FrameCounter = (_meteors[j].FrameCounter + 1) % 12;
				if (rectangle.Contains((int)position.X, (int)position.Y))
				{
					spriteBatch.Draw(_meteorTexture.get_Value(), position, new Rectangle(0, num4 * (_meteorTexture.Height() / 4), _meteorTexture.Width(), _meteorTexture.Height() / 4), Color.White * num3 * _fadeOpacity, 0f, Vector2.Zero, vector4.X * 5f * _meteors[j].Scale, SpriteEffects.None, 0f);
				}
			}
		}

		public override float GetCloudAlpha()
		{
			return (1f - _fadeOpacity) * 0.3f + 0.7f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			_fadeOpacity = 0.002f;
			_isActive = true;
			_meteors = new Meteor[150];
			for (int i = 0; i < _meteors.Length; i++)
			{
				float num = (float)i / (float)_meteors.Length;
				_meteors[i].Position.X = num * ((float)Main.maxTilesX * 16f) + _random.NextFloat() * 40f - 20f;
				_meteors[i].Position.Y = _random.NextFloat() * (0f - ((float)Main.worldSurface * 16f + 10000f)) - 10000f;
				if (_random.Next(3) != 0)
				{
					_meteors[i].Depth = _random.NextFloat() * 3f + 1.8f;
				}
				else
				{
					_meteors[i].Depth = _random.NextFloat() * 5f + 4.8f;
				}
				_meteors[i].FrameCounter = _random.Next(12);
				_meteors[i].Scale = _random.NextFloat() * 0.5f + 1f;
				_meteors[i].StartX = _meteors[i].Position.X;
			}
			Array.Sort(_meteors, SortMethod);
		}

		private int SortMethod(Meteor meteor1, Meteor meteor2)
		{
			return meteor2.Depth.CompareTo(meteor1.Depth);
		}

		public override void Deactivate(params object[] args)
		{
			_isActive = false;
		}

		public override void Reset()
		{
			_isActive = false;
		}

		public override bool IsActive()
		{
			if (!_isActive)
			{
				return _fadeOpacity > 0.001f;
			}
			return true;
		}
	}
}
