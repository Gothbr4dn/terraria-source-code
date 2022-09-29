using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers
{
	public class RandomizedFrameParticle : ABasicParticle
	{
		public float FadeInNormalizedTime;

		public float FadeOutNormalizedTime = 1f;

		public Color ColorTint = Color.White;

		public int AnimationFramesAmount;

		public int GameFramesPerAnimationFrame;

		private float _timeTolive;

		private float _timeSinceSpawn;

		private int _gameFramesCounted;

		public override void FetchFromPool()
		{
			base.FetchFromPool();
			FadeInNormalizedTime = 0f;
			FadeOutNormalizedTime = 1f;
			ColorTint = Color.White;
			AnimationFramesAmount = 0;
			GameFramesPerAnimationFrame = 0;
			_timeTolive = 0f;
			_timeSinceSpawn = 0f;
			_gameFramesCounted = 0;
		}

		public void SetTypeInfo(int animationFramesAmount, int gameFramesPerAnimationFrame, float timeToLive)
		{
			_timeTolive = timeToLive;
			GameFramesPerAnimationFrame = gameFramesPerAnimationFrame;
			AnimationFramesAmount = animationFramesAmount;
			RandomizeFrame();
		}

		private void RandomizeFrame()
		{
			_frame = _texture.Frame(1, AnimationFramesAmount, 0, Main.rand.Next(AnimationFramesAmount));
			_origin = _frame.Size() / 2f;
		}

		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			_timeSinceSpawn += 1f;
			if (_timeSinceSpawn >= _timeTolive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
			if (++_gameFramesCounted >= GameFramesPerAnimationFrame)
			{
				_gameFramesCounted = 0;
				RandomizeFrame();
			}
		}

		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Color color = ColorTint * Utils.GetLerpValue(0f, FadeInNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true);
			spritebatch.Draw(_texture.get_Value(), settings.AnchorPosition + LocalPosition, _frame, color, Rotation, _origin, Scale, SpriteEffects.None, 0f);
		}
	}
}
