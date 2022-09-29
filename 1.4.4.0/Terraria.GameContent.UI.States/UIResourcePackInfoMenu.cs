using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States
{
	public class UIResourcePackInfoMenu : UIState
	{
		private UIResourcePackSelectionMenu _resourceMenu;

		private ResourcePack _pack;

		private UIElement _container;

		private UIList _list;

		private UIScrollbar _scrollbar;

		private bool _isScrollbarAttached;

		private const string _backPointName = "GoBack";

		private UIGamepadHelper _helper;

		public UIResourcePackInfoMenu(UIResourcePackSelectionMenu parent, ResourcePack pack)
		{
			_resourceMenu = parent;
			_pack = pack;
			BuildPage();
		}

		private void BuildPage()
		{
			RemoveAllChildren();
			UIElement uIElement = new UIElement();
			uIElement.Width.Set(0f, 0.8f);
			uIElement.MaxWidth.Set(500f, 0f);
			uIElement.MinWidth.Set(300f, 0f);
			uIElement.Top.Set(230f, 0f);
			uIElement.Height.Set(0f - uIElement.Top.Pixels, 1f);
			uIElement.HAlign = 0.5f;
			Append(uIElement);
			UIPanel uIPanel = new UIPanel();
			uIPanel.Width.Set(0f, 1f);
			uIPanel.Height.Set(-110f, 1f);
			uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
			uIElement.Append(uIPanel);
			UIElement uIElement2 = new UIElement
			{
				Width = StyleDimension.Fill,
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f)
			};
			uIPanel.Append(uIElement2);
			UIElement uIElement3 = new UIElement
			{
				Width = new StyleDimension(0f, 1f),
				Height = new StyleDimension(52f, 0f)
			};
			uIElement3.SetPadding(0f);
			uIElement2.Append(uIElement3);
			UIText uIText = new UIText(_pack.Name, 0.7f, large: true)
			{
				TextColor = Color.Gold
			};
			uIText.HAlign = 0.5f;
			uIText.VAlign = 0f;
			uIElement3.Append(uIText);
			UIText uIText2 = new UIText(Language.GetTextValue("UI.Author", _pack.Author), 0.9f)
			{
				HAlign = 0f,
				VAlign = 1f
			};
			uIText2.Top.Set(-6f, 0f);
			uIElement3.Append(uIText2);
			UIText uIText3 = new UIText(Language.GetTextValue("UI.Version", _pack.Version.GetFormattedVersion()), 0.9f)
			{
				HAlign = 1f,
				VAlign = 1f,
				TextColor = Color.Silver
			};
			uIText3.Top.Set(-6f, 0f);
			uIElement3.Append(uIText3);
			Asset<Texture2D> val = Main.Assets.Request<Texture2D>("Images/UI/Divider", (AssetRequestMode)1);
			UIImage uIImage = new UIImage(val)
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixels(val.Height()),
				ScaleToFit = true
			};
			uIImage.Top.Set(52f, 0f);
			uIImage.SetPadding(6f);
			uIElement2.Append(uIImage);
			UIElement uIElement4 = new UIElement
			{
				HAlign = 0.5f,
				VAlign = 1f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(-74f, 1f)
			};
			uIElement2.Append(uIElement4);
			_container = uIElement4;
			UIText item = new UIText(_pack.Description)
			{
				HAlign = 0.5f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 0f),
				IsWrapped = true,
				WrappedTextBottomPadding = 0f
			};
			UIList uIList = new UIList
			{
				HAlign = 0.5f,
				VAlign = 0f,
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
				PaddingRight = 20f
			};
			uIList.ListPadding = 5f;
			uIList.Add(item);
			uIElement4.Append(uIList);
			_list = uIList;
			UIScrollbar uIScrollbar = new UIScrollbar();
			uIScrollbar.SetView(100f, 1000f);
			uIScrollbar.Height.Set(0f, 1f);
			uIScrollbar.HAlign = 1f;
			_scrollbar = uIScrollbar;
			uIList.SetScrollbar(uIScrollbar);
			UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
			uITextPanel.Width.Set(-10f, 0.5f);
			uITextPanel.Height.Set(50f, 0f);
			uITextPanel.VAlign = 1f;
			uITextPanel.HAlign = 0.5f;
			uITextPanel.Top.Set(-45f, 0f);
			uITextPanel.OnMouseOver += FadedMouseOver;
			uITextPanel.OnMouseOut += FadedMouseOut;
			uITextPanel.OnClick += GoBackClick;
			uITextPanel.SetSnapPoint("GoBack", 0);
			uIElement.Append(uITextPanel);
		}

		public override void Recalculate()
		{
			if (_scrollbar != null)
			{
				if (_isScrollbarAttached && !_scrollbar.CanScroll)
				{
					_container.RemoveChild(_scrollbar);
					_isScrollbarAttached = false;
					_list.Width.Set(0f, 1f);
				}
				else if (!_isScrollbarAttached && _scrollbar.CanScroll)
				{
					_container.Append(_scrollbar);
					_isScrollbarAttached = true;
					_list.Width.Set(-25f, 1f);
				}
			}
			base.Recalculate();
		}

		private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.MenuUI.SetState(_resourceMenu);
		}

		private static void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12);
			((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
			((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		private static void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
			((UIPanel)evt.Target).BorderColor = Color.Black;
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			SetupGamepadPoints(spriteBatch);
		}

		private void SetupGamepadPoints(SpriteBatch spriteBatch)
		{
			UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
			int num = 3000;
			int idRangeEndExclusive = num;
			List<SnapPoint> snapPoints = GetSnapPoints();
			for (int i = 0; i < snapPoints.Count; i++)
			{
				SnapPoint snapPoint = snapPoints[i];
				string name = snapPoint.Name;
				if (name == "GoBack")
				{
					_helper.MakeLinkPointFromSnapPoint(idRangeEndExclusive++, snapPoint);
				}
			}
			_helper.MoveToVisuallyClosestPoint(num, idRangeEndExclusive);
		}
	}
}
