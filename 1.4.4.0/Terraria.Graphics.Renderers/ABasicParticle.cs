using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Renderers
{
	public abstract class ABasicParticle : IPooledParticle, IParticle
	{
		public Vector2 AccelerationPerFrame;

		public Vector2 Velocity;

		public Vector2 LocalPosition;

		protected Asset<Texture2D> _texture;

		protected Rectangle _frame;

		protected Vector2 _origin;

		public float Rotation;

		public float RotationVelocity;

		public float RotationAcceleration;

		public Vector2 Scale;

		public Vector2 ScaleVelocity;

		public Vector2 ScaleAcceleration;

		public bool ShouldBeRemovedFromRenderer { get; protected set; }

		public bool IsRestingInPool { get; private set; }

		public ABasicParticle()
		{
			_texture = null;
			_frame = Rectangle.Empty;
			_origin = Vector2.Zero;
			Velocity = Vector2.Zero;
			LocalPosition = Vector2.Zero;
			ShouldBeRemovedFromRenderer = false;
		}

		public virtual void SetBasicInfo(Asset<Texture2D> textureAsset, Rectangle? frame, Vector2 initialVelocity, Vector2 initialLocalPosition)
		{
			_texture = textureAsset;
			_frame = (frame.HasValue ? frame.Value : _texture.Frame());
			_origin = _frame.Size() / 2f;
			Velocity = initialVelocity;
			LocalPosition = initialLocalPosition;
			ShouldBeRemovedFromRenderer = false;
		}

		public virtual void Update(ref ParticleRendererSettings settings)
		{
			Velocity += AccelerationPerFrame;
			LocalPosition += Velocity;
			RotationVelocity += RotationAcceleration;
			Rotation += RotationVelocity;
			ScaleVelocity += ScaleAcceleration;
			Scale += ScaleVelocity;
		}

		public abstract void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch);

		public void RestInPool()
		{
			IsRestingInPool = true;
		}

		public virtual void FetchFromPool()
		{
			IsRestingInPool = false;
			ShouldBeRemovedFromRenderer = false;
			AccelerationPerFrame = Vector2.Zero;
			Velocity = Vector2.Zero;
			LocalPosition = Vector2.Zero;
			_texture = null;
			_frame = Rectangle.Empty;
			_origin = Vector2.Zero;
			Rotation = 0f;
			RotationVelocity = 0f;
			RotationAcceleration = 0f;
			Scale = Vector2.Zero;
			ScaleVelocity = Vector2.Zero;
			ScaleAcceleration = Vector2.Zero;
		}
	}
}
