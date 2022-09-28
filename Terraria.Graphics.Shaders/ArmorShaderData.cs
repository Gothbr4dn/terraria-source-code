using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
	public class ArmorShaderData : ShaderData
	{
		private Vector3 _uColor = Vector3.One;

		private Vector3 _uSecondaryColor = Vector3.One;

		private float _uSaturation = 1f;

		private float _uOpacity = 1f;

		private Asset<Texture2D> _uImage;

		private Vector2 _uTargetPosition = Vector2.One;

		public ArmorShaderData(Ref<Effect> shader, string passName)
			: base(shader, passName)
		{
		}

		public virtual void Apply(Entity entity, DrawData? drawData = null)
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
				Vector4 value2 = ((!value.sourceRect.HasValue) ? new Vector4(0f, 0f, value.texture.Width, value.texture.Height) : new Vector4(value.sourceRect.Value.X, value.sourceRect.Value.Y, value.sourceRect.Value.Width, value.sourceRect.Value.Height));
				base.Shader.Parameters["uSourceRect"].SetValue(value2);
				base.Shader.Parameters["uLegacyArmorSourceRect"].SetValue(value2);
				base.Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + value.position);
				base.Shader.Parameters["uImageSize0"].SetValue(new Vector2(value.texture.Width, value.texture.Height));
				base.Shader.Parameters["uLegacyArmorSheetSize"].SetValue(new Vector2(value.texture.Width, value.texture.Height));
				base.Shader.Parameters["uRotation"].SetValue(value.rotation * (value.effect.HasFlag(SpriteEffects.FlipHorizontally) ? (-1f) : 1f));
				base.Shader.Parameters["uDirection"].SetValue((!value.effect.HasFlag(SpriteEffects.FlipHorizontally)) ? 1 : (-1));
			}
			else
			{
				Vector4 value3 = new Vector4(0f, 0f, 4f, 4f);
				base.Shader.Parameters["uSourceRect"].SetValue(value3);
				base.Shader.Parameters["uLegacyArmorSourceRect"].SetValue(value3);
				base.Shader.Parameters["uRotation"].SetValue(0f);
			}
			if (_uImage != null)
			{
				Main.graphics.GraphicsDevice.Textures[1] = _uImage.get_Value();
				base.Shader.Parameters["uImageSize1"].SetValue(new Vector2(_uImage.Width(), _uImage.Height()));
			}
			if (entity != null)
			{
				base.Shader.Parameters["uDirection"].SetValue((float)entity.direction);
			}
			if (entity is Player player)
			{
				Rectangle bodyFrame = player.bodyFrame;
				base.Shader.Parameters["uLegacyArmorSourceRect"].SetValue(new Vector4(bodyFrame.X, bodyFrame.Y, bodyFrame.Width, bodyFrame.Height));
				base.Shader.Parameters["uLegacyArmorSheetSize"].SetValue(new Vector2(40f, 1120f));
			}
			Apply();
		}

		public ArmorShaderData UseColor(float r, float g, float b)
		{
			return UseColor(new Vector3(r, g, b));
		}

		public ArmorShaderData UseColor(Color color)
		{
			return UseColor(color.ToVector3());
		}

		public ArmorShaderData UseColor(Vector3 color)
		{
			_uColor = color;
			return this;
		}

		public ArmorShaderData UseImage(string path)
		{
			_uImage = Main.Assets.Request<Texture2D>(path, (AssetRequestMode)1);
			return this;
		}

		public ArmorShaderData UseOpacity(float alpha)
		{
			_uOpacity = alpha;
			return this;
		}

		public ArmorShaderData UseTargetPosition(Vector2 position)
		{
			_uTargetPosition = position;
			return this;
		}

		public ArmorShaderData UseSecondaryColor(float r, float g, float b)
		{
			return UseSecondaryColor(new Vector3(r, g, b));
		}

		public ArmorShaderData UseSecondaryColor(Color color)
		{
			return UseSecondaryColor(color.ToVector3());
		}

		public ArmorShaderData UseSecondaryColor(Vector3 color)
		{
			_uSecondaryColor = color;
			return this;
		}

		public ArmorShaderData UseSaturation(float saturation)
		{
			_uSaturation = saturation;
			return this;
		}

		public virtual ArmorShaderData GetSecondaryShader(Entity entity)
		{
			return this;
		}
	}
}
