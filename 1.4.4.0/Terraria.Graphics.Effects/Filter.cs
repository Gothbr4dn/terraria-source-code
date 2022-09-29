using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.Graphics.Effects
{
	public class Filter : GameEffect
	{
		public bool Active;

		private ScreenShaderData _shader;

		public bool IsHidden;

		public Filter(ScreenShaderData shader, EffectPriority priority = EffectPriority.VeryLow)
		{
			_shader = shader;
			_priority = priority;
		}

		public void Update(GameTime gameTime)
		{
			_shader.UseGlobalOpacity(Opacity);
			_shader.Update(gameTime);
		}

		public void Apply()
		{
			_shader.Apply();
		}

		public ScreenShaderData GetShader()
		{
			return _shader;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			_shader.UseGlobalOpacity(Opacity);
			_shader.UseTargetPosition(position);
			Active = true;
		}

		public override void Deactivate(params object[] args)
		{
			Active = false;
		}

		public bool IsInUse()
		{
			if (!Active)
			{
				return Opacity > 0f;
			}
			return true;
		}

		public bool IsActive()
		{
			return Active;
		}

		public override bool IsVisible()
		{
			if (GetShader().CombinedOpacity > 0f)
			{
				return !IsHidden;
			}
			return false;
		}
	}
}
