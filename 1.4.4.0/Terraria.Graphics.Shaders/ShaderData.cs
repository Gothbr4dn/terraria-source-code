using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Shaders
{
	public class ShaderData
	{
		private readonly Ref<Effect> _shader;

		private string _passName;

		private EffectPass _effectPass;

		public Effect Shader
		{
			get
			{
				if (_shader != null)
				{
					return _shader.Value;
				}
				return null;
			}
		}

		public ShaderData(Ref<Effect> shader, string passName)
		{
			_passName = passName;
			_shader = shader;
		}

		public void SwapProgram(string passName)
		{
			_passName = passName;
			if (passName != null)
			{
				_effectPass = Shader.CurrentTechnique.Passes[passName];
			}
		}

		public virtual void Apply()
		{
			if (_shader != null && Shader != null && _passName != null)
			{
				_effectPass = Shader.CurrentTechnique.Passes[_passName];
			}
			_effectPass.Apply();
		}
	}
}
