using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;

namespace Terraria.UI
{
	public class UserInterface
	{
		private const double DOUBLE_CLICK_TIME = 500.0;

		private const double STATE_CHANGE_CLICK_DISABLE_TIME = 200.0;

		private const int MAX_HISTORY_SIZE = 32;

		private const int HISTORY_PRUNE_SIZE = 4;

		public static UserInterface ActiveInstance = new UserInterface();

		private List<UIState> _history = new List<UIState>();

		public Vector2 MousePosition;

		private bool _wasMouseDown;

		private UIElement _lastElementHover;

		private UIElement _lastElementDown;

		private UIElement _lastElementClicked;

		private double _lastMouseDownTime;

		private double _clickDisabledTimeRemaining;

		private bool _isStateDirty;

		public bool IsVisible;

		private UIState _currentState;

		public UIState CurrentState => _currentState;

		public void ResetLasts()
		{
			if (_lastElementHover != null)
			{
				_lastElementHover.MouseOut(new UIMouseEvent(_lastElementHover, MousePosition));
			}
			_lastElementHover = null;
			_lastElementDown = null;
			_lastElementClicked = null;
		}

		public void EscapeElements()
		{
			ResetLasts();
		}

		public UserInterface()
		{
			ActiveInstance = this;
		}

		public void Use()
		{
			if (ActiveInstance != this)
			{
				ActiveInstance = this;
				Recalculate();
			}
			else
			{
				ActiveInstance = this;
			}
		}

		private void ResetState()
		{
			GetMousePosition();
			_wasMouseDown = Main.mouseLeft;
			if (_lastElementHover != null)
			{
				_lastElementHover.MouseOut(new UIMouseEvent(_lastElementHover, MousePosition));
			}
			_lastElementHover = null;
			_lastElementDown = null;
			_lastElementClicked = null;
			_lastMouseDownTime = 0.0;
			_clickDisabledTimeRemaining = Math.Max(_clickDisabledTimeRemaining, 200.0);
		}

		private void GetMousePosition()
		{
			MousePosition = new Vector2(Main.mouseX, Main.mouseY);
		}

		public void Update(GameTime time)
		{
			if (_currentState == null)
			{
				return;
			}
			GetMousePosition();
			bool flag = Main.mouseLeft && Main.hasFocus;
			UIElement uIElement = (Main.hasFocus ? _currentState.GetElementAt(MousePosition) : null);
			_clickDisabledTimeRemaining = Math.Max(0.0, _clickDisabledTimeRemaining - time.ElapsedGameTime.TotalMilliseconds);
			bool flag2 = _clickDisabledTimeRemaining > 0.0;
			if (uIElement != _lastElementHover)
			{
				if (_lastElementHover != null)
				{
					_lastElementHover.MouseOut(new UIMouseEvent(_lastElementHover, MousePosition));
				}
				uIElement?.MouseOver(new UIMouseEvent(uIElement, MousePosition));
				_lastElementHover = uIElement;
			}
			if (flag && !_wasMouseDown && uIElement != null && !flag2)
			{
				_lastElementDown = uIElement;
				uIElement.MouseDown(new UIMouseEvent(uIElement, MousePosition));
				if (_lastElementClicked == uIElement && time.TotalGameTime.TotalMilliseconds - _lastMouseDownTime < 500.0)
				{
					uIElement.DoubleClick(new UIMouseEvent(uIElement, MousePosition));
					_lastElementClicked = null;
				}
				_lastMouseDownTime = time.TotalGameTime.TotalMilliseconds;
			}
			else if (!flag && _wasMouseDown && _lastElementDown != null && !flag2)
			{
				UIElement lastElementDown = _lastElementDown;
				if (lastElementDown.ContainsPoint(MousePosition))
				{
					lastElementDown.Click(new UIMouseEvent(lastElementDown, MousePosition));
					_lastElementClicked = _lastElementDown;
				}
				lastElementDown.MouseUp(new UIMouseEvent(lastElementDown, MousePosition));
				_lastElementDown = null;
			}
			if (PlayerInput.ScrollWheelDeltaForUI != 0)
			{
				uIElement?.ScrollWheel(new UIScrollWheelEvent(uIElement, MousePosition, PlayerInput.ScrollWheelDeltaForUI));
				PlayerInput.ScrollWheelDeltaForUI = 0;
			}
			_wasMouseDown = flag;
			if (_currentState != null)
			{
				_currentState.Update(time);
			}
		}

		public void Draw(SpriteBatch spriteBatch, GameTime time)
		{
			Use();
			if (_currentState != null)
			{
				if (_isStateDirty)
				{
					_currentState.Recalculate();
					_isStateDirty = false;
				}
				_currentState.Draw(spriteBatch);
			}
		}

		public void DrawDebugHitbox(BasicDebugDrawer drawer)
		{
			_ = _currentState;
		}

		public void SetState(UIState state)
		{
			if (state == _currentState)
			{
				return;
			}
			if (state != null)
			{
				AddToHistory(state);
			}
			if (_currentState != null)
			{
				if (_lastElementHover != null)
				{
					_lastElementHover.MouseOut(new UIMouseEvent(_lastElementHover, MousePosition));
				}
				_currentState.Deactivate();
			}
			_currentState = state;
			ResetState();
			if (state != null)
			{
				_isStateDirty = true;
				state.Activate();
				state.Recalculate();
			}
		}

		public void GoBack()
		{
			if (_history.Count >= 2)
			{
				UIState state = _history[_history.Count - 2];
				_history.RemoveRange(_history.Count - 2, 2);
				SetState(state);
			}
		}

		private void AddToHistory(UIState state)
		{
			_history.Add(state);
			if (_history.Count > 32)
			{
				_history.RemoveRange(0, 4);
			}
		}

		public void Recalculate()
		{
			if (_currentState != null)
			{
				_currentState.Recalculate();
			}
		}

		public CalculatedStyle GetDimensions()
		{
			Vector2 originalScreenSize = PlayerInput.OriginalScreenSize;
			return new CalculatedStyle(0f, 0f, originalScreenSize.X / Main.UIScale, originalScreenSize.Y / Main.UIScale);
		}

		internal void RefreshState()
		{
			if (_currentState != null)
			{
				_currentState.Deactivate();
			}
			ResetState();
			_currentState.Activate();
			_currentState.Recalculate();
		}

		public bool IsElementUnderMouse()
		{
			if (IsVisible && _lastElementHover != null)
			{
				return !(_lastElementHover is UIState);
			}
			return false;
		}
	}
}
