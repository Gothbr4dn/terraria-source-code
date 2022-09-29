using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Renderers
{
	public class CreativeSacrificeParticle : IParticle
	{
		public Vector2 AccelerationPerFrame;

		public Vector2 Velocity;

		public Vector2 LocalPosition;

		public float ScaleOffsetPerFrame;

		public float StopWhenBelowXScale;

		private Asset<Texture2D> _texture;

		private Rectangle _frame;

		private Vector2 _origin;

		private float _scale;

		public bool ShouldBeRemovedFromRenderer { get; private set; }

		public CreativeSacrificeParticle(Asset<Texture2D> textureAsset, Rectangle? frame, Vector2 initialVelocity, Vector2 initialLocalPosition)
		{
			_texture = textureAsset;
			_frame = (frame.HasValue ? frame.Value : _texture.Frame());
			_origin = _frame.Size() / 2f;
			Velocity = initialVelocity;
			LocalPosition = initialLocalPosition;
			StopWhenBelowXScale = 0f;
			ShouldBeRemovedFromRenderer = false;
			_scale = 0.6f;
		}

		public void Update(ref ParticleRendererSettings settings)
		{
			Velocity += AccelerationPerFrame;
			LocalPosition += Velocity;
			_scale += ScaleOffsetPerFrame;
			if (_scale <= StopWhenBelowXScale)
			{
				ShouldBeRemovedFromRenderer = true;
			}
		}

		public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Color color = Color.Lerp(Color.White, new Color(255, 255, 255, 0), Utils.GetLerpValue(0.1f, 0.5f, _scale));
			spritebatch.Draw(_texture.get_Value(), settings.AnchorPosition + LocalPosition, _frame, color, 0f, _origin, _scale, SpriteEffects.None, 0f);
		}
	}
}
