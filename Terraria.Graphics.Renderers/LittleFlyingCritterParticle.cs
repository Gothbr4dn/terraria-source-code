using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers
{
	public class LittleFlyingCritterParticle : IPooledParticle, IParticle
	{
		private int _lifeTimeCounted;

		private int _lifeTimeTotal;

		private Vector2 _spawnPosition;

		private Vector2 _localPosition;

		private Vector2 _velocity;

		private float _neverGoBelowThis;

		public bool IsRestingInPool { get; private set; }

		public bool ShouldBeRemovedFromRenderer { get; private set; }

		public void Prepare(Vector2 position, int duration)
		{
			_spawnPosition = position;
			_localPosition = position + Main.rand.NextVector2Circular(4f, 8f);
			_neverGoBelowThis = position.Y + 8f;
			RandomizeVelocity();
			_lifeTimeCounted = 0;
			_lifeTimeTotal = 300 + Main.rand.Next(6) * 60;
		}

		private void RandomizeVelocity()
		{
			_velocity = Main.rand.NextVector2Circular(1f, 1f);
		}

		public void RestInPool()
		{
			IsRestingInPool = true;
		}

		public virtual void FetchFromPool()
		{
			IsRestingInPool = false;
			ShouldBeRemovedFromRenderer = false;
		}

		public void Update(ref ParticleRendererSettings settings)
		{
			if (++_lifeTimeCounted >= _lifeTimeTotal)
			{
				ShouldBeRemovedFromRenderer = true;
			}
			_velocity += new Vector2((float)Math.Sign(_spawnPosition.X - _localPosition.X) * 0.02f, (float)Math.Sign(_spawnPosition.Y - _localPosition.Y) * 0.02f);
			if (_lifeTimeCounted % 30 == 0 && Main.rand.Next(2) == 0)
			{
				RandomizeVelocity();
				if (Main.rand.Next(2) == 0)
				{
					_velocity /= 2f;
				}
			}
			_localPosition += _velocity;
			if (_localPosition.Y > _neverGoBelowThis)
			{
				_localPosition.Y = _neverGoBelowThis;
				if (_velocity.Y > 0f)
				{
					_velocity.Y *= -1f;
				}
			}
		}

		public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Vector2 vector = settings.AnchorPosition + _localPosition;
			if (vector.X < -10f || vector.X > (float)(Main.screenWidth + 10) || vector.Y < -10f || vector.Y > (float)(Main.screenHeight + 10))
			{
				ShouldBeRemovedFromRenderer = true;
				return;
			}
			Texture2D value = TextureAssets.Extra[262].get_Value();
			int frameY = _lifeTimeCounted % 6 / 3;
			Rectangle value2 = value.Frame(1, 2, 0, frameY);
			Vector2 origin = new Vector2((!(_velocity.X > 0f)) ? 1 : 3, 3f);
			float num = Utils.Remap(_lifeTimeCounted, 0f, 90f, 0f, 1f) * Utils.Remap(_lifeTimeCounted, _lifeTimeTotal - 90, _lifeTimeTotal, 1f, 0f);
			spritebatch.Draw(value, settings.AnchorPosition + _localPosition, value2, Lighting.GetColor(_localPosition.ToTileCoordinates()) * num, 0f, origin, 1f, (_velocity.X > 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
	}
}
