using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.Graphics.Renderers
{
	public class ItemTransferParticle : IPooledParticle, IParticle
	{
		public Vector2 StartPosition;

		public Vector2 EndPosition;

		public Vector2 BezierHelper1;

		public Vector2 BezierHelper2;

		private Item _itemInstance;

		private int _lifeTimeCounted;

		private int _lifeTimeTotal;

		public bool ShouldBeRemovedFromRenderer { get; private set; }

		public bool IsRestingInPool { get; private set; }

		public ItemTransferParticle()
		{
			_itemInstance = new Item();
		}

		public void Update(ref ParticleRendererSettings settings)
		{
			if (++_lifeTimeCounted >= _lifeTimeTotal)
			{
				ShouldBeRemovedFromRenderer = true;
			}
		}

		public void Prepare(int itemType, int lifeTimeTotal, Vector2 playerPosition, Vector2 chestPosition)
		{
			_itemInstance.SetDefaults(itemType);
			_lifeTimeTotal = lifeTimeTotal;
			StartPosition = playerPosition;
			EndPosition = chestPosition;
			Vector2 vector = (EndPosition - StartPosition).SafeNormalize(Vector2.UnitY).RotatedBy(1.5707963705062866);
			bool num = vector.Y < 0f;
			bool flag = vector.Y == 0f;
			if (!num || (flag && Main.rand.Next(2) == 0))
			{
				vector *= -1f;
			}
			vector = new Vector2(0f, -1f);
			float num2 = Vector2.Distance(EndPosition, StartPosition);
			BezierHelper1 = vector * num2 + Main.rand.NextVector2Circular(32f, 32f);
			BezierHelper2 = -vector * num2 + Main.rand.NextVector2Circular(32f, 32f);
		}

		public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
		{
			float fromValue = (float)_lifeTimeCounted / (float)_lifeTimeTotal;
			float toMin = Utils.Remap(fromValue, 0.1f, 0.5f, 0f, 0.85f);
			toMin = Utils.Remap(fromValue, 0.5f, 0.9f, toMin, 1f);
			Vector2.Hermite(ref StartPosition, ref BezierHelper1, ref EndPosition, ref BezierHelper2, toMin, out var result);
			float toMin2 = Utils.Remap(fromValue, 0f, 0.1f, 0f, 1f);
			toMin2 = Utils.Remap(fromValue, 0.85f, 0.95f, toMin2, 0f);
			float num = Utils.Remap(fromValue, 0f, 0.25f, 0f, 1f) * Utils.Remap(fromValue, 0.85f, 0.95f, 1f, 0f);
			ItemSlot.DrawItemIcon(_itemInstance, 31, Main.spriteBatch, settings.AnchorPosition + result, _itemInstance.scale * toMin2, 100f, Color.White * num);
		}

		public void RestInPool()
		{
			IsRestingInPool = true;
		}

		public virtual void FetchFromPool()
		{
			_lifeTimeCounted = 0;
			_lifeTimeTotal = 0;
			IsRestingInPool = false;
			ShouldBeRemovedFromRenderer = false;
			StartPosition = (EndPosition = (BezierHelper1 = (BezierHelper2 = Vector2.Zero)));
		}
	}
}
