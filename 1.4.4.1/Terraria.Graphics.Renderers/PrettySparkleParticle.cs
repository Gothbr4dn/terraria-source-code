using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers
{
	public class PrettySparkleParticle : ABasicParticle
	{
		public float FadeInNormalizedTime = 0.05f;

		public float FadeOutNormalizedTime = 0.9f;

		public float TimeToLive = 60f;

		public Color ColorTint;

		public float Opacity;

		public float AdditiveAmount = 1f;

		public float FadeInEnd = 20f;

		public float FadeOutStart = 30f;

		public float FadeOutEnd = 45f;

		public bool DrawHorizontalAxis = true;

		public bool DrawVerticalAxis = true;

		private float _timeSinceSpawn;

		public override void FetchFromPool()
		{
			base.FetchFromPool();
			ColorTint = Color.Transparent;
			_timeSinceSpawn = 0f;
			Opacity = 0f;
			FadeInNormalizedTime = 0.05f;
			FadeOutNormalizedTime = 0.9f;
			TimeToLive = 60f;
			AdditiveAmount = 1f;
			FadeInEnd = 20f;
			FadeOutStart = 30f;
			FadeOutEnd = 45f;
			DrawVerticalAxis = (DrawHorizontalAxis = true);
		}

		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			_timeSinceSpawn += 1f;
			Opacity = Utils.GetLerpValue(0f, FadeInNormalizedTime, _timeSinceSpawn / TimeToLive, clamped: true) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, _timeSinceSpawn / TimeToLive, clamped: true);
			if (_timeSinceSpawn >= TimeToLive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
		}

		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Color color = Color.White * Opacity * 0.9f;
			color.A /= 2;
			Texture2D value = TextureAssets.Extra[98].get_Value();
			Color color2 = ColorTint * Opacity * 0.5f;
			color2.A = (byte)((float)(int)color2.A * (1f - AdditiveAmount));
			Vector2 origin = value.Size() / 2f;
			Color color3 = color * 0.5f;
			float t = _timeSinceSpawn / TimeToLive * 60f;
			float num = Utils.GetLerpValue(0f, FadeInEnd, t, clamped: true) * Utils.GetLerpValue(FadeOutEnd, FadeOutStart, t, clamped: true);
			Vector2 vector = new Vector2(0.3f, 2f) * num * Scale;
			Vector2 vector2 = new Vector2(0.3f, 1f) * num * Scale;
			color2 *= num;
			color3 *= num;
			Vector2 position = settings.AnchorPosition + LocalPosition;
			SpriteEffects effects = SpriteEffects.None;
			if (DrawHorizontalAxis)
			{
				spritebatch.Draw(value, position, null, color2, MathF.PI / 2f + Rotation, origin, vector, effects, 0f);
			}
			if (DrawVerticalAxis)
			{
				spritebatch.Draw(value, position, null, color2, 0f + Rotation, origin, vector2, effects, 0f);
			}
			if (DrawHorizontalAxis)
			{
				spritebatch.Draw(value, position, null, color3, MathF.PI / 2f + Rotation, origin, vector * 0.6f, effects, 0f);
			}
			if (DrawVerticalAxis)
			{
				spritebatch.Draw(value, position, null, color3, 0f + Rotation, origin, vector2 * 0.6f, effects, 0f);
			}
		}
	}
}
