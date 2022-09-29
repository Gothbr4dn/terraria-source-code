using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace Terraria.GameContent.UI.Minimap
{
	public class MinimapFrame : IConfigKeyHolder
	{
		private class Button
		{
			public bool IsHighlighted;

			private readonly Vector2 _position;

			private readonly Asset<Texture2D> _hoverTexture;

			private readonly Action _onMouseDown;

			private Vector2 Size => new Vector2(_hoverTexture.Width(), _hoverTexture.Height());

			public Button(Asset<Texture2D> hoverTexture, Vector2 position, Action mouseDownCallback)
			{
				_position = position;
				_hoverTexture = hoverTexture;
				_onMouseDown = mouseDownCallback;
			}

			public void Click()
			{
				_onMouseDown();
			}

			public void Draw(SpriteBatch spriteBatch, Vector2 parentPosition)
			{
				if (IsHighlighted)
				{
					spriteBatch.Draw(_hoverTexture.get_Value(), _position + parentPosition, Color.White);
				}
			}

			public bool IsTouchingPoint(Vector2 testPoint, Vector2 parentPosition)
			{
				Vector2 vector = _position + parentPosition + Size * 0.5f;
				Vector2 vector2 = Vector2.Max(Size, new Vector2(22f, 22f)) * 0.5f;
				Vector2 vector3 = testPoint - vector;
				if (Math.Abs(vector3.X) < vector2.X)
				{
					return Math.Abs(vector3.Y) < vector2.Y;
				}
				return false;
			}
		}

		private const float DEFAULT_ZOOM = 1.05f;

		private const float ZOOM_OUT_MULTIPLIER = 0.975f;

		private const float ZOOM_IN_MULTIPLIER = 1.025f;

		private readonly Asset<Texture2D> _frameTexture;

		private readonly Vector2 _frameOffset;

		private Button _resetButton;

		private Button _zoomInButton;

		private Button _zoomOutButton;

		public string ConfigKey { get; set; }

		public string NameKey { get; set; }

		public Vector2 MinimapPosition { get; set; }

		private Vector2 FramePosition
		{
			get
			{
				return MinimapPosition + _frameOffset;
			}
			set
			{
				MinimapPosition = value - _frameOffset;
			}
		}

		public MinimapFrame(Asset<Texture2D> frameTexture, Vector2 frameOffset)
		{
			_frameTexture = frameTexture;
			_frameOffset = frameOffset;
		}

		public void SetResetButton(Asset<Texture2D> hoverTexture, Vector2 position)
		{
			_resetButton = new Button(hoverTexture, position, delegate
			{
				ResetZoom();
			});
		}

		private void ResetZoom()
		{
			Main.mapMinimapScale = 1.05f;
		}

		public void SetZoomInButton(Asset<Texture2D> hoverTexture, Vector2 position)
		{
			_zoomInButton = new Button(hoverTexture, position, delegate
			{
				ZoomInButton();
			});
		}

		private void ZoomInButton()
		{
			Main.mapMinimapScale *= 1.025f;
		}

		public void SetZoomOutButton(Asset<Texture2D> hoverTexture, Vector2 position)
		{
			_zoomOutButton = new Button(hoverTexture, position, delegate
			{
				ZoomOutButton();
			});
		}

		private void ZoomOutButton()
		{
			Main.mapMinimapScale *= 0.975f;
		}

		public void Update()
		{
			Button buttonUnderMouse = GetButtonUnderMouse();
			_zoomInButton.IsHighlighted = buttonUnderMouse == _zoomInButton;
			_zoomOutButton.IsHighlighted = buttonUnderMouse == _zoomOutButton;
			_resetButton.IsHighlighted = buttonUnderMouse == _resetButton;
			if (buttonUnderMouse == null || Main.LocalPlayer.lastMouseInterface)
			{
				return;
			}
			buttonUnderMouse.IsHighlighted = true;
			if (PlayerInput.IgnoreMouseInterface)
			{
				return;
			}
			Main.LocalPlayer.mouseInterface = true;
			if (Main.mouseLeft)
			{
				buttonUnderMouse.Click();
				if (Main.mouseLeftRelease)
				{
					SoundEngine.PlaySound(12);
				}
			}
		}

		public void DrawBackground(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(TextureAssets.MagicPixel.get_Value(), new Rectangle((int)MinimapPosition.X - 6, (int)MinimapPosition.Y - 6, 244, 244), Color.Black * Main.mapMinimapAlpha);
		}

		public void DrawForeground(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_frameTexture.get_Value(), FramePosition, Color.White);
			_zoomInButton.Draw(spriteBatch, FramePosition);
			_zoomOutButton.Draw(spriteBatch, FramePosition);
			_resetButton.Draw(spriteBatch, FramePosition);
		}

		private Button GetButtonUnderMouse()
		{
			Vector2 testPoint = new Vector2(Main.mouseX, Main.mouseY);
			if (_zoomInButton.IsTouchingPoint(testPoint, FramePosition))
			{
				return _zoomInButton;
			}
			if (_zoomOutButton.IsTouchingPoint(testPoint, FramePosition))
			{
				return _zoomOutButton;
			}
			if (_resetButton.IsTouchingPoint(testPoint, FramePosition))
			{
				return _resetButton;
			}
			return null;
		}

		[Conditional("DEBUG")]
		private void ValidateState()
		{
		}
	}
}
