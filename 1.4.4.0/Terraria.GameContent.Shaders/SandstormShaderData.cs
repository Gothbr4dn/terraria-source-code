using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
	public class SandstormShaderData : ScreenShaderData
	{
		private Vector2 _texturePosition = Vector2.Zero;

		public SandstormShaderData(string passName)
			: base(passName)
		{
		}

		public override void Update(GameTime gameTime)
		{
			Vector2 vector = new Vector2(0f - Main.windSpeedCurrent, -1f) * new Vector2(20f, 0.1f);
			vector.Normalize();
			vector *= new Vector2(2f, 0.2f);
			if (!Main.gamePaused && Main.hasFocus)
			{
				_texturePosition += vector * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			_texturePosition.X %= 10f;
			_texturePosition.Y %= 10f;
			UseDirection(vector);
			base.Update(gameTime);
		}

		public override void Apply()
		{
			UseTargetPosition(_texturePosition);
			base.Apply();
		}
	}
}
