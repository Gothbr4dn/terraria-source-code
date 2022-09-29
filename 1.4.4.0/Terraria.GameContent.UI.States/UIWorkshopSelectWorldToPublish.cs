using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	public class UIWorkshopSelectWorldToPublish : UIState, IHaveBackButtonCommand
	{
		private UIList _entryList;

		private UITextPanel<LocalizedText> _backPanel;

		private UIPanel _containerPanel;

		private UIScrollbar _scrollbar;

		private bool _isScrollbarAttached;

		private UIState _menuToGoBackTo;

		private bool skipDraw;

		public UIWorkshopSelectWorldToPublish(UIState menuToGoBackTo)
		{
			_menuToGoBackTo = menuToGoBackTo;
		}

		public override void OnInitialize()
		{
			UIElement uIElement = new UIElement();
			uIElement.Width.Set(0f, 0.8f);
			uIElement.MaxWidth.Set(650f, 0f);
			uIElement.Top.Set(220f, 0f);
			uIElement.Height.Set(-220f, 1f);
			uIElement.HAlign = 0.5f;
			UIPanel uIPanel = new UIPanel();
			uIPanel.Width.Set(0f, 1f);
			uIPanel.Height.Set(-110f, 1f);
			uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			uIElement.Append(uIPanel);
			_containerPanel = uIPanel;
			_entryList = new UIList();
			_entryList.Width.Set(0f, 1f);
			_entryList.Height.Set(0f, 1f);
			_entryList.ListPadding = 5f;
			uIPanel.Append(_entryList);
			_scrollbar = new UIScrollbar();
			_scrollbar.SetView(100f, 1000f);
			_scrollbar.Height.Set(0f, 1f);
			_scrollbar.HAlign = 1f;
			_entryList.SetScrollbar(_scrollbar);
			UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.WorkshopSelectWorldToPublishMenuTitle"), 0.8f, large: true);
			uITextPanel.HAlign = 0.5f;
			uITextPanel.Top.Set(-40f, 0f);
			uITextPanel.SetPadding(15f);
			uITextPanel.BackgroundColor = new Color(73, 94, 171);
			uIElement.Append(uITextPanel);
			UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
			uITextPanel2.Width.Set(-10f, 0.5f);
			uITextPanel2.Height.Set(50f, 0f);
			uITextPanel2.VAlign = 1f;
			uITextPanel2.HAlign = 0.5f;
			uITextPanel2.Top.Set(-45f, 0f);
			uITextPanel2.OnMouseOver += FadedMouseOver;
			uITextPanel2.OnMouseOut += FadedMouseOut;
			uITextPanel2.OnClick += GoBackClick;
			uIElement.Append(uITextPanel2);
			_backPanel = uITextPanel2;
			Append(uIElement);
		}

		public override void Recalculate()
		{
			if (_scrollbar != null)
			{
				if (_isScrollbarAttached && !_scrollbar.CanScroll)
				{
					_containerPanel.RemoveChild(_scrollbar);
					_isScrollbarAttached = false;
					_entryList.Width.Set(0f, 1f);
				}
				else if (!_isScrollbarAttached && _scrollbar.CanScroll)
				{
					_containerPanel.Append(_scrollbar);
					_isScrollbarAttached = true;
					_entryList.Width.Set(-25f, 1f);
				}
			}
			base.Recalculate();
		}

		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			HandleBackButtonUsage();
		}

		public void HandleBackButtonUsage()
		{
			SoundEngine.PlaySound(11);
			Main.MenuUI.SetState(_menuToGoBackTo);
		}

		private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		public override void OnActivate()
		{
			PopulateEntries();
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.ChangePoint(3000 + ((_entryList.Count != 0) ? 1 : 0));
			}
		}

		private void PopulateEntries()
		{
			Main.LoadWorlds();
			_entryList.Clear();
			IOrderedEnumerable<WorldFileData> orderedEnumerable = from x in new List<WorldFileData>(Main.WorldList)
				orderby x.IsFavorite descending, x.Name, x.GetFileName()
				select x;
			_entryList.Clear();
			int num = 0;
			foreach (WorldFileData item in orderedEnumerable)
			{
				_entryList.Add(new UIWorkshopPublishWorldListItem(this, item, num++));
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (skipDraw)
			{
				skipDraw = false;
				return;
			}
			base.Draw(spriteBatch);
			SetupGamepadPoints(spriteBatch);
		}

		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
			int num = 3000;
			UILinkPointNavigator.SetPosition(num, _backPanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
			int num2 = num;
			UILinkPoint uILinkPoint = UILinkPointNavigator.Points[num2];
			uILinkPoint.Unlink();
			uILinkPoint.Right = num2;
			float num3 = 1f / Main.UIScale;
			Rectangle clippingRectangle = _containerPanel.GetClippingRectangle(spriteBatch);
			Vector2 minimum = clippingRectangle.TopLeft() * num3;
			Vector2 maximum = clippingRectangle.BottomRight() * num3;
			List<SnapPoint> snapPoints = GetSnapPoints();
			for (int i = 0; i < snapPoints.Count; i++)
			{
				if (!snapPoints[i].Position.Between(minimum, maximum))
				{
					snapPoints.Remove(snapPoints[i]);
					i--;
				}
			}
			int num4 = 1;
			SnapPoint[,] array = new SnapPoint[_entryList.Count, num4];
			foreach (SnapPoint item in snapPoints.Where((SnapPoint a) => a.Name == "Publish"))
			{
				array[item.Id, 0] = item;
			}
			num2 = num + 1;
			int[] array2 = new int[_entryList.Count];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = -1;
			}
			for (int k = 0; k < num4; k++)
			{
				int num5 = -1;
				for (int l = 0; l < array.GetLength(0); l++)
				{
					if (array[l, k] != null)
					{
						uILinkPoint = UILinkPointNavigator.Points[num2];
						uILinkPoint.Unlink();
						UILinkPointNavigator.SetPosition(num2, array[l, k].Position);
						if (num5 != -1)
						{
							uILinkPoint.Up = num5;
							UILinkPointNavigator.Points[num5].Down = num2;
						}
						if (array2[l] != -1)
						{
							uILinkPoint.Left = array2[l];
							UILinkPointNavigator.Points[array2[l]].Right = num2;
						}
						uILinkPoint.Down = num;
						if (k == 0)
						{
							UILinkPointNavigator.Points[num].Up = (UILinkPointNavigator.Points[num + 1].Up = num2);
						}
						num5 = num2;
						array2[l] = num2;
						UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2;
						num2++;
					}
				}
			}
			if (PlayerInput.UsingGamepadUI && _entryList.Count == 0 && UILinkPointNavigator.CurrentPoint > 3000)
			{
				UILinkPointNavigator.ChangePoint(3000);
			}
		}
	}
}
