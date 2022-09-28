using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
	public class HairShaderData : ShaderData
	{
		protected Vector3 _uColor = Vector3.One;

		protected Vector3 _uSecondaryColor = Vector3.One;

		protected float _uSaturation = 1f;

		protected float _uOpacity = 1f;

		protected Asset<Texture2D> _uImage;

		protected bool _shaderDisabled;

		private Vector2 _uTargetPosition = Vector2.One;

		public bool ShaderDisabled => _shaderDisabled;

		public HairShaderData(Ref<Effect> shader, string passName)
			: base(shader, passName)
		{
		}

		public virtual void Apply(Player player, DrawData? drawData = null)
		{
			if (!_shaderDisabled)
			{
				base.Shader.Parameters["uColor"].SetValue(_uColor);
				base.Shader.Parameters["uSaturation"].SetValue(_uSaturation);
				base.Shader.Parameters["uSecondaryColor"].SetValue(_uSecondaryColor);
				base.Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
				base.Shader.Parameters["uOpacity"].SetValue(_uOpacity);
				base.Shader.Parameters["uTargetPosition"].SetValue(_uTargetPosition);
				if (drawData.HasValue)
				{
					DrawData value = drawData.Value;
					Vector4 value2 = new Vector4(value.sourceRect.Value.X, value.sourceRect.Value.Y, value.sourceRect.Value.Width, value.sourceRect.Value.Height);
					base.Shader.Parameters["uSourceRect"].SetValue(value2);
					base.Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + value.position);
					base.Shader.Parameters["uImageSize0"].SetValue(new Vector2(value.texture.Width, value.texture.Height));
				}
				else
				{
					base.Shader.Parameters["uSourceRect"].SetValue(new Vector4(0f, 0f, 4f, 4f));
				}
				if (_uImage != null)
				{
					Main.graphics.GraphicsDevice.Textures[1] = _uImage.get_Value();
					base.Shader.Parameters["uImageSize1"].SetValue(new Vector2(_uImage.Width(), _uImage.Height()));
				}
				if (player != null)
				{
					base.Shader.Parameters["uDirection"].SetValue((float)player.direction);
				}
				Apply();
			}
		}

		public virtual Color GetColor(Player player, Color lightColor)
		{
			return new Color(lightColor.ToVector4() * player.hairColor.ToVector4());
		}

		public HairShaderData UseColor(float r, float g, float b)
		{
			return UseColor(new Vector3(r, g, b));
		}

		public HairShaderData UseColor(Color color)
		{
			return UseColor(color.ToVector3());
		}

		public HairShaderData UseColor(Vector3 color)
		{
			_uColor = color;
			return this;
		}

		public HairShaderData UseImage(string path)
		{
			_uImage = Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
			return this;
		}

		public HairShaderData UseOpacity(float alpha)
		{
			_uOpacity = alpha;
			return this;
		}

		public HairShaderData UseSecondaryColor(float r, float g, float b)
		{
			return UseSecondaryColor(new Vector3(r, g, b));
		}

		public HairShaderData UseSecondaryColor(Color color)
		{
			return UseSecondaryColor(color.ToVector3());
		}

		public HairShaderData UseSecondaryColor(Vector3 color)
		{
			_uSecondaryColor = color;
			return this;
		}

		public HairShaderData UseSaturation(float saturation)
		{
			_uSaturation = saturation;
			return this;
		}

		public HairShaderData UseTargetPosition(Vector2 position)
		{
			_uTargetPosition = position;
			return this;
		}
	}
}
