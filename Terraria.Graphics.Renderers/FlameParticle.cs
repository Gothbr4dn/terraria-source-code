using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Renderers
{
	public class FlameParticle : ABasicParticle
	{
		public float FadeOutNormalizedTime = 1f;

		private float _timeTolive;

		private float _timeSinceSpawn;

		private int _indexOfPlayerWhoSpawnedThis;

		private int _packedShaderIndex;

		public override void FetchFromPool()
		{
			base.FetchFromPool();
			FadeOutNormalizedTime = 1f;
			_timeTolive = 0f;
			_timeSinceSpawn = 0f;
			_indexOfPlayerWhoSpawnedThis = 0;
			_packedShaderIndex = 0;
		}

		public override void SetBasicInfo(Asset<Texture2D> textureAsset, Rectangle? frame, Vector2 initialVelocity, Vector2 initialLocalPosition)
		{
			base.SetBasicInfo(textureAsset, frame, initialVelocity, initialLocalPosition);
			_origin = new Vector2(_frame.Width / 2, _frame.Height - 2);
		}

		public void SetTypeInfo(float timeToLive, int indexOfPlayerWhoSpawnedIt, int packedShaderIndex)
		{
			_timeTolive = timeToLive;
			_indexOfPlayerWhoSpawnedThis = indexOfPlayerWhoSpawnedIt;
			_packedShaderIndex = packedShaderIndex;
		}

		public override void Update(ref ParticleRendererSettings settings)
		{
			base.Update(ref settings);
			_timeSinceSpawn += 1f;
			if (_timeSinceSpawn >= _timeTolive)
			{
				base.ShouldBeRemovedFromRenderer = true;
			}
		}

		public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			Color color = new Color(120, 120, 120, 60) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true);
			Vector2 vector = settings.AnchorPosition + LocalPosition;
			ulong seed = Main.TileFrameSeed ^ (((ulong)LocalPosition.X << 32) | (uint)LocalPosition.Y);
			Player player = Main.player[_indexOfPlayerWhoSpawnedThis];
			for (int i = 0; i < 4; i++)
			{
				DrawData drawData = new DrawData(position: vector + new Vector2(Utils.RandomInt(ref seed, -2, 3), Utils.RandomInt(ref seed, -2, 3)) * Scale, texture: _texture.get_Value(), sourceRect: _frame, color: color, rotation: Rotation, origin: _origin, scale: Scale, effect: SpriteEffects.None);
				drawData.shader = _packedShaderIndex;
				DrawData cdd = drawData;
				PlayerDrawHelper.SetShaderForData(player, 0, ref cdd);
				cdd.Draw(spritebatch);
			}
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}
	}
}
