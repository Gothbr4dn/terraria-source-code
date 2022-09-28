using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Shaders
{
	public class ScreenShaderData : ShaderData
	{
		private Vector3 _uColor = Vector3.One;

		private Vector3 _uSecondaryColor = Vector3.One;

		private float _uOpacity = 1f;

		private float _globalOpacity = 1f;

		private float _uIntensity = 1f;

		private Vector2 _uTargetPosition = Vector2.One;

		private Vector2 _uDirection = new Vector2(0f, 1f);

		private float _uProgress;

		private Vector2 _uImageOffset = Vector2.Zero;

		private Asset<Texture2D>[] _uAssetImages = new Asset<Texture2D>[3];

		private Texture2D[] _uCustomImages = new Texture2D[3];

		private SamplerState[] _samplerStates = new SamplerState[3];

		private Vector2[] _imageScales = new Vector2[3]
		{
			Vector2.One,
			Vector2.One,
			Vector2.One
		};

		public float Intensity => _uIntensity;

		public float CombinedOpacity => _uOpacity * _globalOpacity;

		public ScreenShaderData(string passName)
			: base(Main.ScreenShaderRef, passName)
		{
		}

		public ScreenShaderData(Ref<Effect> shader, string passName)
			: base(shader, passName)
		{
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public override void Apply()
		{
			Vector2 vector = new Vector2(Main.offScreenRange, Main.offScreenRange);
			Vector2 value = new Vector2(Main.screenWidth, Main.screenHeight) / Main.GameViewMatrix.Zoom;
			Vector2 vector2 = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
			Vector2 vector3 = Main.screenPosition + vector2 * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
			base.Shader.Parameters["uColor"].SetValue(_uColor);
			base.Shader.Parameters["uOpacity"].SetValue(CombinedOpacity);
			base.Shader.Parameters["uSecondaryColor"].SetValue(_uSecondaryColor);
			base.Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
			base.Shader.Parameters["uScreenResolution"].SetValue(value);
			base.Shader.Parameters["uScreenPosition"].SetValue(vector3 - vector);
			base.Shader.Parameters["uTargetPosition"].SetValue(_uTargetPosition - vector);
			base.Shader.Parameters["uImageOffset"].SetValue(_uImageOffset);
			base.Shader.Parameters["uIntensity"].SetValue(_uIntensity);
			base.Shader.Parameters["uProgress"].SetValue(_uProgress);
			base.Shader.Parameters["uDirection"].SetValue(_uDirection);
			base.Shader.Parameters["uZoom"].SetValue(Main.GameViewMatrix.Zoom);
			for (int i = 0; i < _uAssetImages.Length; i++)
			{
				Texture2D texture2D = _uCustomImages[i];
				if (_uAssetImages[i] != null && _uAssetImages[i].get_IsLoaded())
				{
					texture2D = _uAssetImages[i].get_Value();
				}
				if (texture2D != null)
				{
					Main.graphics.GraphicsDevice.Textures[i + 1] = texture2D;
					int width = texture2D.Width;
					int height = texture2D.Height;
					if (_samplerStates[i] != null)
					{
						Main.graphics.GraphicsDevice.SamplerStates[i + 1] = _samplerStates[i];
					}
					else if (Utils.IsPowerOfTwo(width) && Utils.IsPowerOfTwo(height))
					{
						Main.graphics.GraphicsDevice.SamplerStates[i + 1] = SamplerState.LinearWrap;
					}
					else
					{
						Main.graphics.GraphicsDevice.SamplerStates[i + 1] = SamplerState.AnisotropicClamp;
					}
					base.Shader.Parameters["uImageSize" + (i + 1)].SetValue(new Vector2(width, height) * _imageScales[i]);
				}
			}
			base.Apply();
		}

		public ScreenShaderData UseImageOffset(Vector2 offset)
		{
			_uImageOffset = offset;
			return this;
		}

		public ScreenShaderData UseIntensity(float intensity)
		{
			_uIntensity = intensity;
			return this;
		}

		public ScreenShaderData UseColor(float r, float g, float b)
		{
			return UseColor(new Vector3(r, g, b));
		}

		public ScreenShaderData UseProgress(float progress)
		{
			_uProgress = progress;
			return this;
		}

		public ScreenShaderData UseImage(Texture2D image, int index = 0, SamplerState samplerState = null)
		{
			_samplerStates[index] = samplerState;
			_uAssetImages[index] = null;
			_uCustomImages[index] = image;
			return this;
		}

		public ScreenShaderData UseImage(string path, int index = 0, SamplerState samplerState = null)
		{
			_uAssetImages[index] = Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
			_uCustomImages[index] = null;
			_samplerStates[index] = samplerState;
			return this;
		}

		public ScreenShaderData UseColor(Color color)
		{
			return UseColor(color.ToVector3());
		}

		public ScreenShaderData UseColor(Vector3 color)
		{
			_uColor = color;
			return this;
		}

		public ScreenShaderData UseDirection(Vector2 direction)
		{
			_uDirection = direction;
			return this;
		}

		public ScreenShaderData UseGlobalOpacity(float opacity)
		{
			_globalOpacity = opacity;
			return this;
		}

		public ScreenShaderData UseTargetPosition(Vector2 position)
		{
			_uTargetPosition = position;
			return this;
		}

		public ScreenShaderData UseSecondaryColor(float r, float g, float b)
		{
			return UseSecondaryColor(new Vector3(r, g, b));
		}

		public ScreenShaderData UseSecondaryColor(Color color)
		{
			return UseSecondaryColor(color.ToVector3());
		}

		public ScreenShaderData UseSecondaryColor(Vector3 color)
		{
			_uSecondaryColor = color;
			return this;
		}

		public ScreenShaderData UseOpacity(float opacity)
		{
			_uOpacity = opacity;
			return this;
		}

		public ScreenShaderData UseImageScale(Vector2 scale, int index = 0)
		{
			_imageScales[index] = scale;
			return this;
		}

		public virtual ScreenShaderData GetSecondaryShader(Player player)
		{
			return this;
		}
	}
}
