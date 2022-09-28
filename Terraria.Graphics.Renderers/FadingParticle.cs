using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers
{
	public class FadingParticle : ABasicParticle
	{
		public float FadeInNormalizedTime;

		public float FadeOutNormalizedTime = 1f;

		public Color ColorTint = Color.White;

		private float _timeTolive;

		private float _timeSinceSpawn;

		public override void FetchFromPool()
		{
			base.FetchFromPool();
			FadeInNormalizedTime = 0f;
			FadeOutNormalizedTime = 1f;
			ColorTint = Color.White;
			_timeTolive = 0f;
			_timeSinceSpawn = 0f;
		}

		public void SetTypeInfo(float timeToLive)
		{
			_timeTolive = timeToLive;
		}

		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			_timeSinceSpawn += 1f;
			if (_timeSinceSpawn >= _timeTolive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
		}

		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Color color = ColorTint * Utils.GetLerpValue(0f, FadeInNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true);
			spritebatch.Draw(_texture.get_Value(), settings.AnchorPosition + LocalPosition, _frame, color, Rotation, _origin, Scale, SpriteEffects.None, 0f);
		}
	}
}
