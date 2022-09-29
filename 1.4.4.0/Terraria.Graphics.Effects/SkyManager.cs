using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Effects
{
	public class SkyManager : EffectManager<CustomSky>
	{
		public static SkyManager Instance = new SkyManager();

		private float _lastDepth;

		private LinkedList<CustomSky> _activeSkies = new LinkedList<CustomSky>();

		public void Reset()
		{
			foreach (CustomSky value in _effects.Values)
			{
				value.Reset();
			}
			_activeSkies.Clear();
		}

		public void Update(GameTime gameTime)
		{
			int num = Main.dayRate;
			if (num < 1)
			{
				num = 1;
			}
			for (int i = 0; i < num; i++)
			{
				LinkedListNode<CustomSky> linkedListNode = _activeSkies.First;
				while (linkedListNode != null)
				{
					CustomSky value = linkedListNode.Value;
					LinkedListNode<CustomSky> next = linkedListNode.Next;
					value.Update(gameTime);
					if (!value.IsActive())
					{
						_activeSkies.Remove(linkedListNode);
					}
					linkedListNode = next;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			DrawDepthRange(spriteBatch, float.MinValue, float.MaxValue);
		}

		public void DrawToDepth(SpriteBatch spriteBatch, float minDepth)
		{
			if (!(_lastDepth <= minDepth))
			{
				DrawDepthRange(spriteBatch, minDepth, _lastDepth);
				_lastDepth = minDepth;
			}
		}

		public void DrawDepthRange(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			foreach (CustomSky activeSky in _activeSkies)
			{
				activeSky.Draw(spriteBatch, minDepth, maxDepth);
			}
		}

		public void DrawRemainingDepth(SpriteBatch spriteBatch)
		{
			DrawDepthRange(spriteBatch, float.MinValue, _lastDepth);
			_lastDepth = float.MinValue;
		}

		public void ResetDepthTracker()
		{
			_lastDepth = float.MaxValue;
		}

		public void SetStartingDepth(float depth)
		{
			_lastDepth = depth;
		}

		public override void OnActivate(CustomSky effect, Vector2 position)
		{
			_activeSkies.Remove(effect);
			_activeSkies.AddLast(effect);
		}

		public Color ProcessTileColor(Color color)
		{
			foreach (CustomSky activeSky in _activeSkies)
			{
				color = activeSky.OnTileColor(color);
			}
			return color;
		}

		public float ProcessCloudAlpha()
		{
			float num = 1f;
			foreach (CustomSky activeSky in _activeSkies)
			{
				num *= activeSky.GetCloudAlpha();
			}
			return MathHelper.Clamp(num, 0f, 1f);
		}
	}
}
