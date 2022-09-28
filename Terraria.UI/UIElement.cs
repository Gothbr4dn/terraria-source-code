using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent.UI.Elements;

namespace Terraria.UI
{
	public class UIElement : IComparable
	{
		public delegate void MouseEvent(UIMouseEvent evt, UIElement listeningElement);

		public delegate void ScrollWheelEvent(UIScrollWheelEvent evt, UIElement listeningElement);

		public delegate void ElementEvent(UIElement affectedElement);

		public delegate void UIElementAction(UIElement element);

		protected readonly List<UIElement> Elements = new List<UIElement>();

		public StyleDimension Top;

		public StyleDimension Left;

		public StyleDimension Width;

		public StyleDimension Height;

		public StyleDimension MaxWidth = StyleDimension.Fill;

		public StyleDimension MaxHeight = StyleDimension.Fill;

		public StyleDimension MinWidth = StyleDimension.Empty;

		public StyleDimension MinHeight = StyleDimension.Empty;

		private bool _isInitialized;

		public bool IgnoresMouseInteraction;

		public bool OverflowHidden;

		public SamplerState OverrideSamplerState;

		public float PaddingTop;

		public float PaddingLeft;

		public float PaddingRight;

		public float PaddingBottom;

		public float MarginTop;

		public float MarginLeft;

		public float MarginRight;

		public float MarginBottom;

		public float HAlign;

		public float VAlign;

		private CalculatedStyle _innerDimensions;

		private CalculatedStyle _dimensions;

		private CalculatedStyle _outerDimensions;

		private static readonly RasterizerState OverflowHiddenRasterizerState = new RasterizerState
		{
			CullMode = CullMode.None,
			ScissorTestEnable = true
		};

		public bool UseImmediateMode;

		private SnapPoint _snapPoint;

		private static int _idCounter = 0;

		public UIElement Parent { get; private set; }

		public int UniqueId { get; private set; }

		public IEnumerable<UIElement> Children => Elements;

		public bool IsMouseHovering { get; private set; }

		public event MouseEvent OnMouseDown;

		public event MouseEvent OnMouseUp;

		public event MouseEvent OnClick;

		public event MouseEvent OnMouseOver;

		public event MouseEvent OnMouseOut;

		public event MouseEvent OnDoubleClick;

		public event ScrollWheelEvent OnScrollWheel;

		public event ElementEvent OnUpdate;

		public UIElement()
		{
			UniqueId = _idCounter++;
		}

		public void SetSnapPoint(string name, int id, Vector2? anchor = null, Vector2? offset = null)
		{
			if (!anchor.HasValue)
			{
				anchor = new Vector2(0.5f);
			}
			if (!offset.HasValue)
			{
				offset = Vector2.Zero;
			}
			_snapPoint = new SnapPoint(name, id, anchor.Value, offset.Value);
		}

		public bool GetSnapPoint(out SnapPoint point)
		{
			point = _snapPoint;
			if (_snapPoint != null)
			{
				_snapPoint.Calculate(this);
			}
			return _snapPoint != null;
		}

		public virtual void ExecuteRecursively(UIElementAction action)
		{
			action(this);
			foreach (UIElement element in Elements)
			{
				element.ExecuteRecursively(action);
			}
		}

		protected virtual void DrawSelf(SpriteBatch spriteBatch)
		{
		}

		protected virtual void DrawChildren(SpriteBatch spriteBatch)
		{
			foreach (UIElement element in Elements)
			{
				element.Draw(spriteBatch);
			}
		}

		public void Append(UIElement element)
		{
			element.Remove();
			element.Parent = this;
			Elements.Add(element);
			element.Recalculate();
		}

		public void Remove()
		{
			if (Parent != null)
			{
				Parent.RemoveChild(this);
			}
		}

		public void RemoveChild(UIElement child)
		{
			Elements.Remove(child);
			child.Parent = null;
		}

		public void RemoveAllChildren()
		{
			foreach (UIElement element in Elements)
			{
				element.Parent = null;
			}
			Elements.Clear();
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			bool overflowHidden = OverflowHidden;
			bool useImmediateMode = UseImmediateMode;
			RasterizerState rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
			Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
			SamplerState anisotropicClamp = SamplerState.AnisotropicClamp;
			if (useImmediateMode || OverrideSamplerState != null)
			{
				spriteBatch.End();
				spriteBatch.Begin(useImmediateMode ? SpriteSortMode.Immediate : SpriteSortMode.Deferred, BlendState.AlphaBlend, (OverrideSamplerState != null) ? OverrideSamplerState : anisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);
				DrawSelf(spriteBatch);
				spriteBatch.End();
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);
			}
			else
			{
				DrawSelf(spriteBatch);
			}
			if (overflowHidden)
			{
				spriteBatch.End();
				Rectangle clippingRectangle = GetClippingRectangle(spriteBatch);
				spriteBatch.GraphicsDevice.ScissorRectangle = clippingRectangle;
				spriteBatch.GraphicsDevice.RasterizerState = OverflowHiddenRasterizerState;
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, OverflowHiddenRasterizerState, null, Main.UIScaleMatrix);
			}
			DrawChildren(spriteBatch);
			if (overflowHidden)
			{
				spriteBatch.End();
				spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
				spriteBatch.GraphicsDevice.RasterizerState = rasterizerState;
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, rasterizerState, null, Main.UIScaleMatrix);
			}
		}

		public virtual void Update(GameTime gameTime)
		{
			if (this.OnUpdate != null)
			{
				this.OnUpdate(this);
			}
			foreach (UIElement element in Elements)
			{
				element.Update(gameTime);
			}
		}

		public Rectangle GetClippingRectangle(SpriteBatch spriteBatch)
		{
			Vector2 vector = new Vector2(_innerDimensions.X, _innerDimensions.Y);
			Vector2 position = new Vector2(_innerDimensions.Width, _innerDimensions.Height) + vector;
			vector = Vector2.Transform(vector, Main.UIScaleMatrix);
			position = Vector2.Transform(position, Main.UIScaleMatrix);
			Rectangle rectangle = new Rectangle((int)vector.X, (int)vector.Y, (int)(position.X - vector.X), (int)(position.Y - vector.Y));
			int num = (int)((float)Main.screenWidth * Main.UIScale);
			int num2 = (int)((float)Main.screenHeight * Main.UIScale);
			rectangle.X = Utils.Clamp(rectangle.X, 0, num);
			rectangle.Y = Utils.Clamp(rectangle.Y, 0, num2);
			rectangle.Width = Utils.Clamp(rectangle.Width, 0, num - rectangle.X);
			rectangle.Height = Utils.Clamp(rectangle.Height, 0, num2 - rectangle.Y);
			Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
			int num3 = Utils.Clamp(rectangle.Left, scissorRectangle.Left, scissorRectangle.Right);
			int num4 = Utils.Clamp(rectangle.Top, scissorRectangle.Top, scissorRectangle.Bottom);
			int num5 = Utils.Clamp(rectangle.Right, scissorRectangle.Left, scissorRectangle.Right);
			int num6 = Utils.Clamp(rectangle.Bottom, scissorRectangle.Top, scissorRectangle.Bottom);
			return new Rectangle(num3, num4, num5 - num3, num6 - num4);
		}

		public virtual List<SnapPoint> GetSnapPoints()
		{
			List<SnapPoint> list = new List<SnapPoint>();
			if (GetSnapPoint(out var point))
			{
				list.Add(point);
			}
			foreach (UIElement element in Elements)
			{
				list.AddRange(element.GetSnapPoints());
			}
			return list;
		}

		public virtual void Recalculate()
		{
			CalculatedStyle parentDimensions = ((Parent == null) ? UserInterface.ActiveInstance.GetDimensions() : Parent.GetInnerDimensions());
			if (Parent != null && Parent is UIList)
			{
				parentDimensions.Height = float.MaxValue;
			}
			CalculatedStyle calculatedStyle = (_outerDimensions = GetDimensionsBasedOnParentDimensions(parentDimensions));
			calculatedStyle.X += MarginLeft;
			calculatedStyle.Y += MarginTop;
			calculatedStyle.Width -= MarginLeft + MarginRight;
			calculatedStyle.Height -= MarginTop + MarginBottom;
			_dimensions = calculatedStyle;
			calculatedStyle.X += PaddingLeft;
			calculatedStyle.Y += PaddingTop;
			calculatedStyle.Width -= PaddingLeft + PaddingRight;
			calculatedStyle.Height -= PaddingTop + PaddingBottom;
			_innerDimensions = calculatedStyle;
			RecalculateChildren();
		}

		private CalculatedStyle GetDimensionsBasedOnParentDimensions(CalculatedStyle parentDimensions)
		{
			CalculatedStyle result = default(CalculatedStyle);
			result.X = Left.GetValue(parentDimensions.Width) + parentDimensions.X;
			result.Y = Top.GetValue(parentDimensions.Height) + parentDimensions.Y;
			float value = MinWidth.GetValue(parentDimensions.Width);
			float value2 = MaxWidth.GetValue(parentDimensions.Width);
			float value3 = MinHeight.GetValue(parentDimensions.Height);
			float value4 = MaxHeight.GetValue(parentDimensions.Height);
			result.Width = MathHelper.Clamp(Width.GetValue(parentDimensions.Width), value, value2);
			result.Height = MathHelper.Clamp(Height.GetValue(parentDimensions.Height), value3, value4);
			result.Width += MarginLeft + MarginRight;
			result.Height += MarginTop + MarginBottom;
			result.X += parentDimensions.Width * HAlign - result.Width * HAlign;
			result.Y += parentDimensions.Height * VAlign - result.Height * VAlign;
			return result;
		}

		public UIElement GetElementAt(Vector2 point)
		{
			UIElement uIElement = null;
			for (int num = Elements.Count - 1; num >= 0; num--)
			{
				UIElement uIElement2 = Elements[num];
				if (!uIElement2.IgnoresMouseInteraction && uIElement2.ContainsPoint(point))
				{
					uIElement = uIElement2;
					break;
				}
			}
			if (uIElement != null)
			{
				return uIElement.GetElementAt(point);
			}
			if (IgnoresMouseInteraction)
			{
				return null;
			}
			if (ContainsPoint(point))
			{
				return this;
			}
			return null;
		}

		public virtual bool ContainsPoint(Vector2 point)
		{
			if (point.X > _dimensions.X && point.Y > _dimensions.Y && point.X < _dimensions.X + _dimensions.Width)
			{
				return point.Y < _dimensions.Y + _dimensions.Height;
			}
			return false;
		}

		public virtual Rectangle GetViewCullingArea()
		{
			return _dimensions.ToRectangle();
		}

		public void SetPadding(float pixels)
		{
			PaddingBottom = pixels;
			PaddingLeft = pixels;
			PaddingRight = pixels;
			PaddingTop = pixels;
		}

		public virtual void RecalculateChildren()
		{
			foreach (UIElement element in Elements)
			{
				element.Recalculate();
			}
		}

		public CalculatedStyle GetInnerDimensions()
		{
			return _innerDimensions;
		}

		public CalculatedStyle GetDimensions()
		{
			return _dimensions;
		}

		public CalculatedStyle GetOuterDimensions()
		{
			return _outerDimensions;
		}

		public void CopyStyle(UIElement element)
		{
			Top = element.Top;
			Left = element.Left;
			Width = element.Width;
			Height = element.Height;
			PaddingBottom = element.PaddingBottom;
			PaddingLeft = element.PaddingLeft;
			PaddingRight = element.PaddingRight;
			PaddingTop = element.PaddingTop;
			HAlign = element.HAlign;
			VAlign = element.VAlign;
			MinWidth = element.MinWidth;
			MaxWidth = element.MaxWidth;
			MinHeight = element.MinHeight;
			MaxHeight = element.MaxHeight;
			Recalculate();
		}

		public virtual void MouseDown(UIMouseEvent evt)
		{
			if (this.OnMouseDown != null)
			{
				this.OnMouseDown(evt, this);
			}
			if (Parent != null)
			{
				Parent.MouseDown(evt);
			}
		}

		public virtual void MouseUp(UIMouseEvent evt)
		{
			if (this.OnMouseUp != null)
			{
				this.OnMouseUp(evt, this);
			}
			if (Parent != null)
			{
				Parent.MouseUp(evt);
			}
		}

		public virtual void MouseOver(UIMouseEvent evt)
		{
			IsMouseHovering = true;
			if (this.OnMouseOver != null)
			{
				this.OnMouseOver(evt, this);
			}
			if (Parent != null)
			{
				Parent.MouseOver(evt);
			}
		}

		public virtual void MouseOut(UIMouseEvent evt)
		{
			IsMouseHovering = false;
			if (this.OnMouseOut != null)
			{
				this.OnMouseOut(evt, this);
			}
			if (Parent != null)
			{
				Parent.MouseOut(evt);
			}
		}

		public virtual void Click(UIMouseEvent evt)
		{
			if (this.OnClick != null)
			{
				this.OnClick(evt, this);
			}
			if (Parent != null)
			{
				Parent.Click(evt);
			}
		}

		public virtual void DoubleClick(UIMouseEvent evt)
		{
			if (this.OnDoubleClick != null)
			{
				this.OnDoubleClick(evt, this);
			}
			if (Parent != null)
			{
				Parent.DoubleClick(evt);
			}
		}

		public virtual void ScrollWheel(UIScrollWheelEvent evt)
		{
			if (this.OnScrollWheel != null)
			{
				this.OnScrollWheel(evt, this);
			}
			if (Parent != null)
			{
				Parent.ScrollWheel(evt);
			}
		}

		public void Activate()
		{
			if (!_isInitialized)
			{
				Initialize();
			}
			OnActivate();
			foreach (UIElement element in Elements)
			{
				element.Activate();
			}
		}

		public virtual void OnActivate()
		{
		}

		[Conditional("DEBUG")]
		public void DrawDebugHitbox(BasicDebugDrawer drawer, float colorIntensity = 0f)
		{
			if (IsMouseHovering)
			{
				colorIntensity += 0.1f;
			}
			Color color = Main.hslToRgb(colorIntensity, colorIntensity, 0.5f);
			CalculatedStyle innerDimensions = GetInnerDimensions();
			drawer.DrawLine(innerDimensions.Position(), innerDimensions.Position() + new Vector2(innerDimensions.Width, 0f), 2f, color);
			drawer.DrawLine(innerDimensions.Position() + new Vector2(innerDimensions.Width, 0f), innerDimensions.Position() + new Vector2(innerDimensions.Width, innerDimensions.Height), 2f, color);
			drawer.DrawLine(innerDimensions.Position() + new Vector2(innerDimensions.Width, innerDimensions.Height), innerDimensions.Position() + new Vector2(0f, innerDimensions.Height), 2f, color);
			drawer.DrawLine(innerDimensions.Position() + new Vector2(0f, innerDimensions.Height), innerDimensions.Position(), 2f, color);
			foreach (UIElement element in Elements)
			{
				_ = element;
			}
		}

		public void Deactivate()
		{
			OnDeactivate();
			foreach (UIElement element in Elements)
			{
				element.Deactivate();
			}
		}

		public virtual void OnDeactivate()
		{
		}

		public void Initialize()
		{
			OnInitialize();
			_isInitialized = true;
		}

		public virtual void OnInitialize()
		{
		}

		public virtual int CompareTo(object obj)
		{
			return 0;
		}
	}
}
