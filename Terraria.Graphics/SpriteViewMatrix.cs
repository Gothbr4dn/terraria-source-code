using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics
{
	public class SpriteViewMatrix
	{
		private Vector2 _zoom = Vector2.One;

		private Vector2 _translation = Vector2.Zero;

		private Matrix _zoomMatrix = Matrix.Identity;

		private Matrix _transformationMatrix = Matrix.Identity;

		private Matrix _normalizedTransformationMatrix = Matrix.Identity;

		private SpriteEffects _effects;

		private Matrix _effectMatrix;

		private GraphicsDevice _graphicsDevice;

		private Viewport _viewport;

		private bool _overrideSystemViewport;

		private bool _needsRebuild = true;

		public Vector2 Zoom
		{
			get
			{
				return _zoom;
			}
			set
			{
				if (_zoom != value)
				{
					_zoom = value;
					_needsRebuild = true;
				}
			}
		}

		public Vector2 Translation
		{
			get
			{
				if (ShouldRebuild())
				{
					Rebuild();
				}
				return _translation;
			}
		}

		public Matrix ZoomMatrix
		{
			get
			{
				if (ShouldRebuild())
				{
					Rebuild();
				}
				return _zoomMatrix;
			}
		}

		public Matrix TransformationMatrix
		{
			get
			{
				if (ShouldRebuild())
				{
					Rebuild();
				}
				return _transformationMatrix;
			}
		}

		public Matrix NormalizedTransformationmatrix
		{
			get
			{
				if (ShouldRebuild())
				{
					Rebuild();
				}
				return _normalizedTransformationMatrix;
			}
		}

		public SpriteEffects Effects
		{
			get
			{
				return _effects;
			}
			set
			{
				if (_effects != value)
				{
					_effects = value;
					_needsRebuild = true;
				}
			}
		}

		public Matrix EffectMatrix
		{
			get
			{
				if (ShouldRebuild())
				{
					Rebuild();
				}
				return _effectMatrix;
			}
		}

		public SpriteViewMatrix(GraphicsDevice graphicsDevice)
		{
			_graphicsDevice = graphicsDevice;
		}

		private void Rebuild()
		{
			if (!_overrideSystemViewport)
			{
				_viewport = _graphicsDevice.Viewport;
			}
			Vector2 vector = new Vector2(_viewport.Width, _viewport.Height);
			Matrix identity = Matrix.Identity;
			if (_effects.HasFlag(SpriteEffects.FlipHorizontally))
			{
				identity *= Matrix.CreateScale(-1f, 1f, 1f) * Matrix.CreateTranslation(vector.X, 0f, 0f);
			}
			if (_effects.HasFlag(SpriteEffects.FlipVertically))
			{
				identity *= Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0f, vector.Y, 0f);
			}
			Vector2 vector2 = vector * 0.5f;
			Vector2 translation = vector2 - vector2 / _zoom;
			Matrix matrix = Matrix.CreateOrthographicOffCenter(0f, vector.X, vector.Y, 0f, 0f, 1f);
			_translation = translation;
			_zoomMatrix = Matrix.CreateTranslation(0f - translation.X, 0f - translation.Y, 0f) * Matrix.CreateScale(_zoom.X, _zoom.Y, 1f);
			_effectMatrix = identity;
			_transformationMatrix = identity * _zoomMatrix;
			_normalizedTransformationMatrix = Matrix.Invert(identity) * _zoomMatrix * matrix;
			_needsRebuild = false;
		}

		public void SetViewportOverride(Viewport viewport)
		{
			_viewport = viewport;
			_overrideSystemViewport = true;
		}

		public void ClearViewportOverride()
		{
			_overrideSystemViewport = false;
		}

		private bool ShouldRebuild()
		{
			if (!_needsRebuild)
			{
				if (!_overrideSystemViewport)
				{
					if (_graphicsDevice.Viewport.Width == _viewport.Width)
					{
						return _graphicsDevice.Viewport.Height != _viewport.Height;
					}
					return true;
				}
				return false;
			}
			return true;
		}
	}
}
