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
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	public class UICharacterSelect : UIState
	{
		private UIList _playerList;

		private UITextPanel<LocalizedText> _backPanel;

		private UITextPanel<LocalizedText> _newPanel;

		private UIPanel _containerPanel;

		private UIScrollbar _scrollbar;

		private bool _isScrollbarAttached;

		private List<Tuple<string, bool>> favoritesCache = new List<Tuple<string, bool>>();

		private bool skipDraw;

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
			_containerPanel = uIPanel;
			uIElement.Append(uIPanel);
			_playerList = new UIList();
			_playerList.Width.Set(0f, 1f);
			_playerList.Height.Set(0f, 1f);
			_playerList.ListPadding = 5f;
			uIPanel.Append(_playerList);
			_scrollbar = new UIScrollbar();
			_scrollbar.SetView(100f, 1000f);
			_scrollbar.Height.Set(0f, 1f);
			_scrollbar.HAlign = 1f;
			_playerList.SetScrollbar(_scrollbar);
			UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.SelectPlayer"), 0.8f, large: true);
			uITextPanel.HAlign = 0.5f;
			uITextPanel.Top.Set(-40f, 0f);
			uITextPanel.SetPadding(15f);
			uITextPanel.BackgroundColor = new Color(73, 94, 171);
			uIElement.Append(uITextPanel);
			UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
			uITextPanel2.Width.Set(-10f, 0.5f);
			uITextPanel2.Height.Set(50f, 0f);
			uITextPanel2.VAlign = 1f;
			uITextPanel2.Top.Set(-45f, 0f);
			uITextPanel2.OnMouseOver += FadedMouseOver;
			uITextPanel2.OnMouseOut += FadedMouseOut;
			uITextPanel2.OnClick += GoBackClick;
			uITextPanel2.SetSnapPoint("Back", 0);
			uIElement.Append(uITextPanel2);
			_backPanel = uITextPanel2;
			UITextPanel<LocalizedText> uITextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("UI.New"), 0.7f, large: true);
			uITextPanel3.CopyStyle(uITextPanel2);
			uITextPanel3.HAlign = 1f;
			uITextPanel3.OnMouseOver += FadedMouseOver;
			uITextPanel3.OnMouseOut += FadedMouseOut;
			uITextPanel3.OnClick += NewCharacterClick;
			uIElement.Append(uITextPanel3);
			uITextPanel2.SetSnapPoint("New", 0);
			_newPanel = uITextPanel3;
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
					_playerList.Width.Set(0f, 1f);
				}
				else if (!_isScrollbarAttached && _scrollbar.CanScroll)
				{
					_containerPanel.Append(_scrollbar);
					_isScrollbarAttached = true;
					_playerList.Width.Set(-25f, 1f);
				}
			}
			base.Recalculate();
		}

		private void NewCharacterClick(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(10);
			Main.PendingPlayer = new Player();
			Main.menuMode = 888;
			Main.MenuUI.SetState(new UICharacterCreation(Main.PendingPlayer));
		}

		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(11);
			Main.menuMode = 0;
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
			Main.LoadPlayers();
			Main.ActivePlayerFileData = new PlayerFileData();
			UpdatePlayersList();
			if (PlayerInput.UsingGamepadUI)
			{
				UILinkPointNavigator.ChangePoint(3000 + ((_playerList.Count == 0) ? 1 : 2));
			}
		}

		private void UpdatePlayersList()
		{
			_playerList.Clear();
			List<PlayerFileData> list = new List<PlayerFileData>(Main.PlayerList);
			list.Sort(delegate(PlayerFileData x, PlayerFileData y)
			{
				if (x.IsFavorite && !y.IsFavorite)
				{
					return -1;
				}
				if (!x.IsFavorite && y.IsFavorite)
				{
					return 1;
				}
				return (x.Name.CompareTo(y.Name) != 0) ? x.Name.CompareTo(y.Name) : x.GetFileName().CompareTo(y.GetFileName());
			});
			int num = 0;
			foreach (PlayerFileData item in list)
			{
				_playerList.Add(new UICharacterListItem(item, num++));
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (skipDraw)
			{
				skipDraw = false;
				return;
			}
			if (UpdateFavoritesCache())
			{
				skipDraw = true;
				Main.MenuUI.Draw(spriteBatch, new GameTime());
			}
			base.Draw(spriteBatch);
			SetupGamepadPoints(spriteBatch);
		}

		private bool UpdateFavoritesCache()
		{
			List<PlayerFileData> list = new List<PlayerFileData>(Main.PlayerList);
			list.Sort(delegate(PlayerFileData x, PlayerFileData y)
			{
				if (x.IsFavorite && !y.IsFavorite)
				{
					return -1;
				}
				if (!x.IsFavorite && y.IsFavorite)
				{
					return 1;
				}
				return (x.Name.CompareTo(y.Name) != 0) ? x.Name.CompareTo(y.Name) : x.GetFileName().CompareTo(y.GetFileName());
			});
			bool flag = false;
			if (!flag && list.Count != favoritesCache.Count)
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = 0; i < favoritesCache.Count; i++)
				{
					Tuple<string, bool> tuple = favoritesCache[i];
					if (!(list[i].Name == tuple.Item1) || list[i].IsFavorite != tuple.Item2)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				favoritesCache.Clear();
				foreach (PlayerFileData item in list)
				{
					favoritesCache.Add(Tuple.Create(item.Name, item.IsFavorite));
				}
				UpdatePlayersList();
			}
			return flag;
		}

		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
			int num = 3000;
			UILinkPointNavigator.SetPosition(num, _backPanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
			UILinkPointNavigator.SetPosition(num + 1, _newPanel.GetInnerDimensions().ToRectangle().Center.ToVector2());
			int num2 = num;
			UILinkPoint uILinkPoint = UILinkPointNavigator.Points[num2];
			uILinkPoint.Unlink();
			uILinkPoint.Right = num2 + 1;
			num2 = num + 1;
			uILinkPoint = UILinkPointNavigator.Points[num2];
			uILinkPoint.Unlink();
			uILinkPoint.Left = num2 - 1;
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
			int num4 = 5;
			SnapPoint[,] array = new SnapPoint[_playerList.Count, num4];
			foreach (SnapPoint item in snapPoints.Where((SnapPoint a) => a.Name == "Play"))
			{
				array[item.Id, 0] = item;
			}
			foreach (SnapPoint item2 in snapPoints.Where((SnapPoint a) => a.Name == "Favorite"))
			{
				array[item2.Id, 1] = item2;
			}
			foreach (SnapPoint item3 in snapPoints.Where((SnapPoint a) => a.Name == "Cloud"))
			{
				array[item3.Id, 2] = item3;
			}
			foreach (SnapPoint item4 in snapPoints.Where((SnapPoint a) => a.Name == "Rename"))
			{
				array[item4.Id, 3] = item4;
			}
			foreach (SnapPoint item5 in snapPoints.Where((SnapPoint a) => a.Name == "Delete"))
			{
				array[item5.Id, 4] = item5;
			}
			num2 = num + 2;
			int[] array2 = new int[_playerList.Count];
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
			if (PlayerInput.UsingGamepadUI && _playerList.Count == 0 && UILinkPointNavigator.CurrentPoint > 3001)
			{
				UILinkPointNavigator.ChangePoint(3001);
			}
		}
	}
}
