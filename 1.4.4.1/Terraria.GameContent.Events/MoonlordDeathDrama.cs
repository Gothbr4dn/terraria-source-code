using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Utilities;

namespace Terraria.GameContent.Events
{
	public class MoonlordDeathDrama
	{
		public class MoonlordPiece
		{
			private Texture2D _texture;

			private Vector2 _position;

			private Vector2 _velocity;

			private Vector2 _origin;

			private float _rotation;

			private float _rotationVelocity;

			public bool Dead
			{
				get
				{
					if (!(_position.Y > (float)(Main.maxTilesY * 16) - 480f) && !(_position.X < 480f))
					{
						return _position.X >= (float)(Main.maxTilesX * 16) - 480f;
					}
					return true;
				}
			}

			public MoonlordPiece(Texture2D pieceTexture, Vector2 textureOrigin, Vector2 centerPos, Vector2 velocity, float rot, float angularVelocity)
			{
				_texture = pieceTexture;
				_origin = textureOrigin;
				_position = centerPos;
				_velocity = velocity;
				_rotation = rot;
				_rotationVelocity = angularVelocity;
			}

			public void Update()
			{
				_velocity.Y += 0.3f;
				_rotation += _rotationVelocity;
				_rotationVelocity *= 0.99f;
				_position += _velocity;
			}

			public void Draw(SpriteBatch sp)
			{
				Color light = GetLight();
				sp.Draw(_texture, _position - Main.screenPosition, null, light, _rotation, _origin, 1f, SpriteEffects.None, 0f);
			}

			public bool InDrawRange(Rectangle playerScreen)
			{
				return playerScreen.Contains(_position.ToPoint());
			}

			public Color GetLight()
			{
				Vector3 zero = Vector3.Zero;
				float num = 0f;
				int num2 = 5;
				Point point = _position.ToTileCoordinates();
				for (int i = point.X - num2; i <= point.X + num2; i++)
				{
					for (int j = point.Y - num2; j <= point.Y + num2; j++)
					{
						zero += Lighting.GetColor(i, j).ToVector3();
						num += 1f;
					}
				}
				if (num == 0f)
				{
					return Color.White;
				}
				return new Color(zero / num);
			}
		}

		public class MoonlordExplosion
		{
			private Texture2D _texture;

			private Vector2 _position;

			private Vector2 _origin;

			private Rectangle _frame;

			private int _frameCounter;

			private int _frameSpeed;

			public bool Dead
			{
				get
				{
					if (!(_position.Y > (float)(Main.maxTilesY * 16) - 480f) && !(_position.X < 480f) && !(_position.X >= (float)(Main.maxTilesX * 16) - 480f))
					{
						return _frameCounter >= _frameSpeed * 7;
					}
					return true;
				}
			}

			public MoonlordExplosion(Texture2D pieceTexture, Vector2 centerPos, int frameSpeed)
			{
				_texture = pieceTexture;
				_position = centerPos;
				_frameSpeed = frameSpeed;
				_frameCounter = 0;
				_frame = _texture.Frame(1, 7);
				_origin = _frame.Size() / 2f;
			}

			public void Update()
			{
				_frameCounter++;
				_frame = _texture.Frame(1, 7, 0, _frameCounter / _frameSpeed);
			}

			public void Draw(SpriteBatch sp)
			{
				Color light = GetLight();
				sp.Draw(_texture, _position - Main.screenPosition, _frame, light, 0f, _origin, 1f, SpriteEffects.None, 0f);
			}

			public bool InDrawRange(Rectangle playerScreen)
			{
				return playerScreen.Contains(_position.ToPoint());
			}

			public Color GetLight()
			{
				return new Color(255, 255, 255, 127);
			}
		}

		private static List<MoonlordPiece> _pieces = new List<MoonlordPiece>();

		private static List<MoonlordExplosion> _explosions = new List<MoonlordExplosion>();

		private static List<Vector2> _lightSources = new List<Vector2>();

		private static float whitening;

		private static float requestedLight;

		public static void Update()
		{
			for (int i = 0; i < _pieces.Count; i++)
			{
				MoonlordPiece moonlordPiece = _pieces[i];
				moonlordPiece.Update();
				if (moonlordPiece.Dead)
				{
					_pieces.Remove(moonlordPiece);
					i--;
				}
			}
			for (int j = 0; j < _explosions.Count; j++)
			{
				MoonlordExplosion moonlordExplosion = _explosions[j];
				moonlordExplosion.Update();
				if (moonlordExplosion.Dead)
				{
					_explosions.Remove(moonlordExplosion);
					j--;
				}
			}
			bool flag = false;
			for (int k = 0; k < _lightSources.Count; k++)
			{
				if (Main.player[Main.myPlayer].Distance(_lightSources[k]) < 2000f)
				{
					flag = true;
					break;
				}
			}
			_lightSources.Clear();
			if (!flag)
			{
				requestedLight = 0f;
			}
			if (requestedLight != whitening)
			{
				if (Math.Abs(requestedLight - whitening) < 0.02f)
				{
					whitening = requestedLight;
				}
				else
				{
					whitening += (float)Math.Sign(requestedLight - whitening) * 0.02f;
				}
			}
			requestedLight = 0f;
		}

		public static void DrawPieces(SpriteBatch spriteBatch)
		{
			Rectangle playerScreen = Utils.CenteredRectangle(Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f, new Vector2(Main.screenWidth + 1000, Main.screenHeight + 1000));
			for (int i = 0; i < _pieces.Count; i++)
			{
				if (_pieces[i].InDrawRange(playerScreen))
				{
					_pieces[i].Draw(spriteBatch);
				}
			}
		}

		public static void DrawExplosions(SpriteBatch spriteBatch)
		{
			Rectangle playerScreen = Utils.CenteredRectangle(Main.screenPosition + new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f, new Vector2(Main.screenWidth + 1000, Main.screenHeight + 1000));
			for (int i = 0; i < _explosions.Count; i++)
			{
				if (_explosions[i].InDrawRange(playerScreen))
				{
					_explosions[i].Draw(spriteBatch);
				}
			}
		}

		public static void DrawWhite(SpriteBatch spriteBatch)
		{
			if (whitening != 0f)
			{
				Color color = Color.White * whitening;
				spriteBatch.Draw(TextureAssets.MagicPixel.get_Value(), new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), new Rectangle(0, 0, 1, 1), color);
			}
		}

		public static void ThrowPieces(Vector2 MoonlordCoreCenter, int DramaSeed)
		{
			UnifiedRandom r = new UnifiedRandom(DramaSeed);
			Vector2 vector = Vector2.UnitY.RotatedBy(r.NextFloat() * (MathF.PI / 2f) - MathF.PI / 4f + MathF.PI);
			_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Spine", (AssetRequestMode)1).get_Value(), new Vector2(64f, 150f), MoonlordCoreCenter + new Vector2(0f, 50f), vector * 6f, 0f, r.NextFloat() * 0.1f - 0.05f));
			vector = Vector2.UnitY.RotatedBy(r.NextFloat() * (MathF.PI / 2f) - MathF.PI / 4f + MathF.PI);
			_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Shoulder", (AssetRequestMode)1).get_Value(), new Vector2(40f, 120f), MoonlordCoreCenter + new Vector2(50f, -120f), vector * 10f, 0f, r.NextFloat() * 0.1f - 0.05f));
			vector = Vector2.UnitY.RotatedBy(r.NextFloat() * (MathF.PI / 2f) - MathF.PI / 4f + MathF.PI);
			_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Torso", (AssetRequestMode)1).get_Value(), new Vector2(192f, 252f), MoonlordCoreCenter, vector * 8f, 0f, r.NextFloat() * 0.1f - 0.05f));
			vector = Vector2.UnitY.RotatedBy(r.NextFloat() * (MathF.PI / 2f) - MathF.PI / 4f + MathF.PI);
			_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Head", (AssetRequestMode)1).get_Value(), new Vector2(138f, 185f), MoonlordCoreCenter - new Vector2(0f, 200f), vector * 12f, 0f, r.NextFloat() * 0.1f - 0.05f));
		}

		public static void AddExplosion(Vector2 spot)
		{
			_explosions.Add(new MoonlordExplosion(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Explosion", (AssetRequestMode)1).get_Value(), spot, Main.rand.Next(2, 4)));
		}

		public static void RequestLight(float light, Vector2 spot)
		{
			_lightSources.Add(spot);
			if (light > 1f)
			{
				light = 1f;
			}
			if (requestedLight < light)
			{
				requestedLight = light;
			}
		}
	}
}
