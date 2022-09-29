using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	public class BlizzardSky : CustomSky
	{
		private UnifiedRandom _random = new UnifiedRandom();

		private bool _isActive;

		private bool _isLeaving;

		private float _opacity;

		public override void OnLoad()
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (Main.gamePaused || !Main.hasFocus)
			{
				return;
			}
			if (_isLeaving)
			{
				_opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (_opacity < 0f)
				{
					_isActive = false;
					_opacity = 0f;
				}
			}
			else
			{
				_opacity += (float)gameTime.ElapsedGameTime.TotalSeconds;
				if (_opacity > 1f)
				{
					_opacity = 1f;
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (minDepth < 1f || maxDepth == float.MaxValue)
			{
				float num = Math.Min(1f, Main.cloudAlpha * 2f);
				Color color = new Color(new Vector4(1f) * Main.ColorOfTheSkies.ToVector4()) * _opacity * 0.7f * num;
				spriteBatch.Draw(TextureAssets.MagicPixel.get_Value(), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);
			}
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			_isActive = true;
			_isLeaving = false;
		}

		public override void Deactivate(params object[] args)
		{
			_isLeaving = true;
		}

		public override void Reset()
		{
			_opacity = 0f;
			_isActive = false;
		}

		public override bool IsActive()
		{
			return _isActive;
		}
	}
}
