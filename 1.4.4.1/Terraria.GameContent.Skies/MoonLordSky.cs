using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies
{
	public class MoonLordSky : CustomSky
	{
		private UnifiedRandom _random = new UnifiedRandom();

		private bool _isActive;

		private int _moonLordIndex = -1;

		private bool _forPlayer;

		private float _fadeOpacity;

		public MoonLordSky(bool forPlayer)
		{
			_forPlayer = forPlayer;
		}

		public override void OnLoad()
		{
		}

		public override void Update(GameTime gameTime)
		{
			if (_forPlayer)
			{
				if (_isActive)
				{
					_fadeOpacity = Math.Min(1f, 0.01f + _fadeOpacity);
				}
				else
				{
					_fadeOpacity = Math.Max(0f, _fadeOpacity - 0.01f);
				}
			}
		}

		private float GetIntensity()
		{
			if (_forPlayer)
			{
				return _fadeOpacity;
			}
			if (UpdateMoonLordIndex())
			{
				float x = 0f;
				if (_moonLordIndex != -1)
				{
					x = Vector2.Distance(Main.player[Main.myPlayer].Center, Main.npc[_moonLordIndex].Center);
				}
				return 1f - Utils.SmoothStep(3000f, 6000f, x);
			}
			return 0f;
		}

		public override Color OnTileColor(Color inColor)
		{
			float intensity = GetIntensity();
			return new Color(Vector4.Lerp(new Vector4(0.5f, 0.8f, 1f, 1f), inColor.ToVector4(), 1f - intensity));
		}

		private bool UpdateMoonLordIndex()
		{
			if (_moonLordIndex >= 0 && Main.npc[_moonLordIndex].active && Main.npc[_moonLordIndex].type == 398)
			{
				return true;
			}
			int num = -1;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == 398)
				{
					num = i;
					break;
				}
			}
			_moonLordIndex = num;
			return num != -1;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 0f && minDepth < 0f)
			{
				float intensity = GetIntensity();
				spriteBatch.Draw(TextureAssets.BlackTile.get_Value(), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * intensity);
			}
		}

		public override float GetCloudAlpha()
		{
			return 1f - _fadeOpacity;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			_isActive = true;
			if (_forPlayer)
			{
				_fadeOpacity = 0.002f;
			}
			else
			{
				_fadeOpacity = 1f;
			}
		}

		public override void Deactivate(params object[] args)
		{
			_isActive = false;
			if (!_forPlayer)
			{
				_fadeOpacity = 0f;
			}
		}

		public override void Reset()
		{
			_isActive = false;
			_fadeOpacity = 0f;
		}

		public override bool IsActive()
		{
			if (!_isActive)
			{
				return _fadeOpacity > 0.001f;
			}
			return true;
		}
	}
}
