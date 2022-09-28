using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.IO;

namespace Terraria.Graphics.Effects
{
	public class FilterManager : EffectManager<Filter>
	{
		private const float OPACITY_RATE = 1f;

		private LinkedList<Filter> _activeFilters = new LinkedList<Filter>();

		private int _filterLimit = 16;

		private EffectPriority _priorityThreshold;

		private int _activeFilterCount;

		private bool _captureThisFrame;

		public event Action OnPostDraw;

		public FilterManager()
		{
			Main.Configuration.OnLoad += delegate(Preferences preferences)
			{
				_filterLimit = preferences.Get("FilterLimit", 16);
				if (Enum.TryParse<EffectPriority>(preferences.Get("FilterPriorityThreshold", "VeryLow"), out var result))
				{
					_priorityThreshold = result;
				}
			};
			Main.Configuration.OnSave += delegate(Preferences preferences)
			{
				preferences.Put("FilterLimit", _filterLimit);
				preferences.Put("FilterPriorityThreshold", Enum.GetName(typeof(EffectPriority), _priorityThreshold));
			};
		}

		public override void OnActivate(Filter effect, Vector2 position)
		{
			if (_activeFilters.Contains(effect))
			{
				if (effect.Active)
				{
					return;
				}
				if (effect.Priority >= _priorityThreshold)
				{
					_activeFilterCount--;
				}
				_activeFilters.Remove(effect);
			}
			else
			{
				effect.Opacity = 0f;
			}
			if (effect.Priority >= _priorityThreshold)
			{
				_activeFilterCount++;
			}
			if (_activeFilters.Count == 0)
			{
				_activeFilters.AddLast(effect);
				return;
			}
			for (LinkedListNode<Filter> linkedListNode = _activeFilters.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				Filter value = linkedListNode.Value;
				if (effect.Priority <= value.Priority)
				{
					_activeFilters.AddAfter(linkedListNode, effect);
					return;
				}
			}
			_activeFilters.AddLast(effect);
		}

		public void BeginCapture(RenderTarget2D screenTarget1, Color clearColor)
		{
			if (_activeFilterCount == 0 && this.OnPostDraw == null)
			{
				_captureThisFrame = false;
				return;
			}
			_captureThisFrame = true;
			Main.instance.GraphicsDevice.SetRenderTarget(screenTarget1);
			Main.instance.GraphicsDevice.Clear(clearColor);
		}

		public void Update(GameTime gameTime)
		{
			LinkedListNode<Filter> linkedListNode = _activeFilters.First;
			_ = _activeFilters.Count;
			int num = 0;
			while (linkedListNode != null)
			{
				Filter value = linkedListNode.Value;
				LinkedListNode<Filter> next = linkedListNode.Next;
				bool flag = false;
				if (value.Priority >= _priorityThreshold)
				{
					num++;
					if (num > _activeFilterCount - _filterLimit)
					{
						value.Update(gameTime);
						flag = true;
					}
				}
				if (value.Active && flag)
				{
					value.Opacity = Math.Min(value.Opacity + (float)gameTime.ElapsedGameTime.TotalSeconds * 1f, 1f);
				}
				else
				{
					value.Opacity = Math.Max(value.Opacity - (float)gameTime.ElapsedGameTime.TotalSeconds * 1f, 0f);
				}
				if (!value.Active && value.Opacity == 0f)
				{
					if (value.Priority >= _priorityThreshold)
					{
						_activeFilterCount--;
					}
					_activeFilters.Remove(linkedListNode);
				}
				linkedListNode = next;
			}
		}

		public void EndCapture(RenderTarget2D finalTexture, RenderTarget2D screenTarget1, RenderTarget2D screenTarget2, Color clearColor)
		{
			if (!_captureThisFrame)
			{
				return;
			}
			LinkedListNode<Filter> linkedListNode = _activeFilters.First;
			_ = _activeFilters.Count;
			Filter filter = null;
			RenderTarget2D renderTarget2D = null;
			RenderTarget2D renderTarget2D2 = screenTarget1;
			GraphicsDevice graphicsDevice = Main.instance.GraphicsDevice;
			int num = 0;
			if (Main.player[Main.myPlayer].gravDir == -1f)
			{
				renderTarget2D = screenTarget2;
				graphicsDevice.SetRenderTarget(renderTarget2D);
				graphicsDevice.Clear(clearColor);
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Invert(Main.GameViewMatrix.EffectMatrix));
				Main.spriteBatch.Draw(renderTarget2D2, Vector2.Zero, Color.White);
				Main.spriteBatch.End();
				renderTarget2D2 = screenTarget2;
			}
			while (linkedListNode != null)
			{
				Filter value = linkedListNode.Value;
				LinkedListNode<Filter> next = linkedListNode.Next;
				if (value.Priority >= _priorityThreshold)
				{
					num++;
					if (num > _activeFilterCount - _filterLimit && value.IsVisible())
					{
						if (filter != null)
						{
							renderTarget2D = ((renderTarget2D2 != screenTarget1) ? screenTarget1 : screenTarget2);
							graphicsDevice.SetRenderTarget(renderTarget2D);
							graphicsDevice.Clear(clearColor);
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
							filter.Apply();
							Main.spriteBatch.Draw(renderTarget2D2, Vector2.Zero, Main.ColorOfTheSkies);
							Main.spriteBatch.End();
							renderTarget2D2 = ((renderTarget2D2 != screenTarget1) ? screenTarget1 : screenTarget2);
						}
						filter = value;
					}
				}
				linkedListNode = next;
			}
			graphicsDevice.SetRenderTarget(finalTexture);
			graphicsDevice.Clear(clearColor);
			if (Main.player[Main.myPlayer].gravDir == -1f)
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.EffectMatrix);
			}
			else
			{
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
			}
			if (filter != null)
			{
				filter.Apply();
				Main.spriteBatch.Draw(renderTarget2D2, Vector2.Zero, Main.ColorOfTheSkies);
			}
			else
			{
				Main.spriteBatch.Draw(renderTarget2D2, Vector2.Zero, Color.White);
			}
			Main.spriteBatch.End();
			for (int i = 0; i < 8; i++)
			{
				graphicsDevice.Textures[i] = null;
			}
			if (this.OnPostDraw != null)
			{
				this.OnPostDraw();
			}
		}

		public bool HasActiveFilter()
		{
			return _activeFilters.Count != 0;
		}

		public bool CanCapture()
		{
			if (!HasActiveFilter())
			{
				return this.OnPostDraw != null;
			}
			return true;
		}
	}
}
