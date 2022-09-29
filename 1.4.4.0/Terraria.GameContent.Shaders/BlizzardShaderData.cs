using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
	public class BlizzardShaderData : ScreenShaderData
	{
		private Vector2 _texturePosition = Vector2.Zero;

		private float windSpeed = 0.1f;

		public BlizzardShaderData(string passName)
			: base(passName)
		{
		}

		public override void Update(GameTime gameTime)
		{
			float num = Main.windSpeedCurrent;
			if (num >= 0f && num <= 0.1f)
			{
				num = 0.1f;
			}
			else if (num <= 0f && num >= -0.1f)
			{
				num = -0.1f;
			}
			windSpeed = num * 0.05f + windSpeed * 0.95f;
			Vector2 vector = new Vector2(0f - windSpeed, -1f) * new Vector2(10f, 2f);
			vector.Normalize();
			vector *= new Vector2(0.8f, 0.6f);
			if (!Main.gamePaused && Main.hasFocus)
			{
				_texturePosition += vector * (float)gameTime.ElapsedGameTime.TotalSeconds;
			}
			_texturePosition.X %= 10f;
			_texturePosition.Y %= 10f;
			UseDirection(vector);
			UseTargetPosition(_texturePosition);
			base.Update(gameTime);
		}

		public override void Apply()
		{
			UseTargetPosition(_texturePosition);
			base.Apply();
		}
	}
}
