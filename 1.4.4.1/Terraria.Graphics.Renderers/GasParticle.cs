using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers
{
	public class GasParticle : ABasicParticle
	{
		public float FadeInNormalizedTime = 0.25f;

		public float FadeOutNormalizedTime = 0.75f;

		public float TimeToLive = 80f;

		public Color ColorTint;

		public float Opacity;

		public float AdditiveAmount = 1f;

		public float FadeInEnd = 20f;

		public float FadeOutStart = 30f;

		public float FadeOutEnd = 45f;

		public float SlowdownScalar = 0.95f;

		private float _timeSinceSpawn;

		public Color LightColorTint;

		private int _internalIndentifier;

		public float InitialScale = 1f;

		public override void FetchFromPool()
		{
			base.FetchFromPool();
			ColorTint = Color.Transparent;
			_timeSinceSpawn = 0f;
			Opacity = 0f;
			FadeInNormalizedTime = 0.25f;
			FadeOutNormalizedTime = 0.75f;
			TimeToLive = 80f;
			_internalIndentifier = Main.rand.Next(255);
			SlowdownScalar = 0.95f;
			LightColorTint = Color.Transparent;
			InitialScale = 1f;
		}

		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			_timeSinceSpawn += 1f;
			float fromValue = _timeSinceSpawn / TimeToLive;
			Scale = Vector2.One * InitialScale * Utils.Remap(fromValue, 0f, 0.95f, 1f, 1.3f);
			Opacity = MathHelper.Clamp(Utils.Remap(fromValue, 0f, FadeInNormalizedTime, 0f, 1f) * Utils.Remap(fromValue, FadeOutNormalizedTime, 1f, 1f, 0f), 0f, 1f) * 0.85f;
			Rotation = (float)_internalIndentifier * 0.4002029f + _timeSinceSpawn * (MathF.PI * 2f) / 480f * 0.5f;
			Velocity *= SlowdownScalar;
			if (LightColorTint != Color.Transparent)
			{
				Color color = LightColorTint * Opacity;
				Lighting.AddLight(LocalPosition, (float)(int)color.R / 255f, (float)(int)color.G / 255f, (float)(int)color.B / 255f);
			}
			if (_timeSinceSpawn >= TimeToLive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
		}

		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Main.instance.LoadProjectile(1007);
			Texture2D value = TextureAssets.Projectile[1007].get_Value();
			Vector2 origin = new Vector2(value.Width / 2, value.Height / 2);
			Vector2 position = settings.AnchorPosition + LocalPosition;
			Color color = Color.Lerp(Lighting.GetColor(LocalPosition.ToTileCoordinates()), ColorTint, 0.2f) * Opacity;
			Vector2 scale = Scale;
			spritebatch.Draw(value, position, value.Frame(), color, Rotation, origin, scale, SpriteEffects.None, 0f);
			spritebatch.Draw(value, position, value.Frame(), color * 0.25f, Rotation, origin, scale * (1f + Opacity * 1.5f), SpriteEffects.None, 0f);
		}
	}
}
