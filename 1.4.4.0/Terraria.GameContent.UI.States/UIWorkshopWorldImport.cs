using System;
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
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	public class UIWorkshopWorldImport : UIState, IHaveBackButtonCommand
	{
		private UIList _worldList;

		private UITextPanel<LocalizedText> _backPanel;

		private UIPanel _containerPanel;

		private UIScrollbar _scrollbar;

		private bool _isScrollbarAttached;

		private List<Tuple<string, bool>> favoritesCache = new List<Tuple<string, bool>>();

		private UIState _uiStateToGoBackTo;

		public static List<WorldFileData> WorkshopWorldList = new List<WorldFileData>();

		private bool skipDraw;

		public UIWorkshopWorldImport(UIState uiStateToGoBackTo)
		{
			_uiStateToGoBackTo = uiStateToGoBackTo;
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
			_worldList = new UIList();
			_worldList.Width.Set(0f, 1f);
			_worldList.Height.Set(0f, 1f);
			_worldList.ListPadding = 5f;
			uIPanel.Append(_worldList);
			_scrollbar = new UIScrollbar();
			_scrollbar.SetView(100f, 1000f);
			_scrollbar.Height.Set(0f, 1f);
			_scrollbar.HAlign = 1f;
			_worldList.SetScrollbar(_scrollbar);
			UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.WorkshopImportWorld"), 0.8f, large: true);
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
					_worldList.Width.Set(0f, 1f);
				}
				else if (!_isScrollbarAttached && _scrollbar.CanScroll)
				{
					_containerPanel.Append(_scrollbar);
					_isScrollbarAttached = true;
					_worldList.Width.Set(-25f, 1f);
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
			Main.MenuUI.SetState(_uiStateToGoBackTo);
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
			Main.LoadWorlds();
			UpdateWorkshopWorldList();
			UpdateWorldsList();
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.ChangePoint(3000 + ((_worldList.Count == 0) ? 1 : 2));
			}
		}

		public void UpdateWorkshopWorldList()
		{
			WorkshopWorldList.Clear();
			if (SocialAPI.Workshop == null)
			{
				return;
			}
			foreach (string listOfSubscribedWorldPath in SocialAPI.Workshop.GetListOfSubscribedWorldPaths())
			{
				WorldFileData allMetadata = WorldFile.GetAllMetadata(listOfSubscribedWorldPath, cloudSave: false);
				if (allMetadata != null)
				{
					WorkshopWorldList.Add(allMetadata);
				}
				else
				{
					WorkshopWorldList.Add(WorldFileData.FromInvalidWorld(listOfSubscribedWorldPath, cloudSave: false));
				}
			}
		}

		private void UpdateWorldsList()
		{
			_worldList.Clear();
			IOrderedEnumerable<WorldFileData> orderedEnumerable = from x in new List<WorldFileData>(WorkshopWorldList)
				orderby x.IsFavorite descending, x.Name, x.GetFileName()
				select x;
			int num = 0;
			foreach (WorldFileData item in orderedEnumerable)
			{
				_worldList.Add(new UIWorkshopImportWorldListItem(this, item, num++));
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
			int key = num;
			UILinkPoint uILinkPoint = UILinkPointNavigator.Points[key];
			uILinkPoint.Unlink();
			float num2 = 1f / Main.UIScale;
			Rectangle clippingRectangle = _containerPanel.GetClippingRectangle(spriteBatch);
			Vector2 minimum = clippingRectangle.TopLeft() * num2;
			Vector2 maximum = clippingRectangle.BottomRight() * num2;
			List<SnapPoint> snapPoints = GetSnapPoints();
			for (int i = 0; i < snapPoints.Count; i++)
			{
				if (!snapPoints[i].Position.Between(minimum, maximum))
				{
					snapPoints.Remove(snapPoints[i]);
					i--;
				}
			}
			SnapPoint[,] array = new SnapPoint[_worldList.Count, 1];
			foreach (SnapPoint item in snapPoints.Where((SnapPoint a) => a.Name == "Import"))
			{
				array[item.Id, 0] = item;
			}
			key = num + 2;
			int[] array2 = new int[_worldList.Count];
			for (int j = 0; j < array2.Length; j++)
			{
				array2[j] = -1;
			}
			for (int k = 0; k < 1; k++)
			{
				int num3 = -1;
				for (int l = 0; l < array.GetLength(0); l++)
				{
					if (array[l, k] != null)
					{
						uILinkPoint = UILinkPointNavigator.Points[key];
						uILinkPoint.Unlink();
						UILinkPointNavigator.SetPosition(key, array[l, k].Position);
						if (num3 != -1)
						{
							uILinkPoint.Up = num3;
							UILinkPointNavigator.Points[num3].Down = key;
						}
						if (array2[l] != -1)
						{
							uILinkPoint.Left = array2[l];
							UILinkPointNavigator.Points[array2[l]].Right = key;
						}
						uILinkPoint.Down = num;
						if (k == 0)
						{
							UILinkPointNavigator.Points[num].Up = (UILinkPointNavigator.Points[num + 1].Up = key);
						}
						num3 = key;
						array2[l] = key;
						UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = key;
						key++;
					}
				}
			}
			if (PlayerInput.UsingGamepadUI && _worldList.Count == 0 && UILinkPointNavigator.CurrentPoint > 3001)
			{
				UILinkPointNavigator.ChangePoint(3001);
			}
		}
	}
}
