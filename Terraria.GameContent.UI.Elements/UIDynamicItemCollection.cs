using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.Elements
{
	public class UIDynamicItemCollection : UIElement
	{
		private List<int> _itemIdsAvailableToShow = new List<int>();

		private List<int> _itemIdsToLoadTexturesFor = new List<int>();

		private int _itemsPerLine;

		private const int sizePerEntryX = 44;

		private const int sizePerEntryY = 44;

		private List<SnapPoint> _dummySnapPoints = new List<SnapPoint>();

		private Item _item = new Item();

		public UIDynamicItemCollection()
		{
			Width = new StyleDimension(0f, 1f);
			HAlign = 0.5f;
			UpdateSize();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			Main.inventoryScale = 0.84615386f;
			GetGridParameters(out var startX, out var startY, out var startItemIndex, out var endItemIndex);
			int num = _itemsPerLine;
			for (int i = startItemIndex; i < endItemIndex; i++)
			{
				int num2 = _itemIdsAvailableToShow[i];
				Rectangle itemSlotHitbox = GetItemSlotHitbox(startX, startY, startItemIndex, i);
				Item inv = ContentSamples.ItemsByType[num2];
				int context = 29;
				if ((int)TextureAssets.Item[num2].get_State() == 0)
				{
					num--;
				}
				bool cREATIVE_ItemSlotShouldHighlightAsSelected = false;
				if (base.IsMouseHovering && itemSlotHitbox.Contains(Main.MouseScreen.ToPoint()) && !PlayerInput.IgnoreMouseInterface)
				{
					_item.SetDefaults(inv.type);
					inv = _item;
					Main.LocalPlayer.mouseInterface = true;
					ItemSlot.OverrideHover(ref inv, context);
					ItemSlot.LeftClick(ref inv, context);
					ItemSlot.RightClick(ref inv, context);
					ItemSlot.MouseHover(ref inv, context);
					cREATIVE_ItemSlotShouldHighlightAsSelected = true;
				}
				UILinkPointNavigator.Shortcuts.CREATIVE_ItemSlotShouldHighlightAsSelected = cREATIVE_ItemSlotShouldHighlightAsSelected;
				ItemSlot.Draw(spriteBatch, ref inv, context, itemSlotHitbox.TopLeft());
				if (num <= 0)
				{
					break;
				}
			}
			while (_itemIdsToLoadTexturesFor.Count > 0 && num > 0)
			{
				int num3 = _itemIdsToLoadTexturesFor[0];
				_itemIdsToLoadTexturesFor.RemoveAt(0);
				if ((int)TextureAssets.Item[num3].get_State() == 0)
				{
					Main.instance.LoadItem(num3);
					num -= 4;
				}
			}
		}

		private Rectangle GetItemSlotHitbox(int startX, int startY, int startItemIndex, int i)
		{
			int num = i - startItemIndex;
			int num2 = num % _itemsPerLine;
			int num3 = num / _itemsPerLine;
			return new Rectangle(startX + num2 * 44, startY + num3 * 44, 44, 44);
		}

		private void GetGridParameters(out int startX, out int startY, out int startItemIndex, out int endItemIndex)
		{
			Rectangle rectangle = GetDimensions().ToRectangle();
			Rectangle viewCullingArea = base.Parent.GetViewCullingArea();
			int x = rectangle.Center.X;
			startX = x - (int)((float)(44 * _itemsPerLine) * 0.5f);
			startY = rectangle.Top;
			startItemIndex = 0;
			endItemIndex = _itemIdsAvailableToShow.Count;
			int num = (Math.Min(viewCullingArea.Top, rectangle.Top) - viewCullingArea.Top) / 44;
			startY += -num * 44;
			startItemIndex += -num * _itemsPerLine;
			int num2 = (int)Math.Ceiling((float)viewCullingArea.Height / 44f) * _itemsPerLine;
			if (endItemIndex > num2 + startItemIndex + _itemsPerLine)
			{
				endItemIndex = num2 + startItemIndex + _itemsPerLine;
			}
		}

		public override void Recalculate()
		{
			base.Recalculate();
			UpdateSize();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (base.IsMouseHovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
		}

		public void SetContentsToShow(List<int> itemIdsToShow)
		{
			_itemIdsAvailableToShow.Clear();
			_itemIdsToLoadTexturesFor.Clear();
			_itemIdsAvailableToShow.AddRange(itemIdsToShow);
			_itemIdsToLoadTexturesFor.AddRange(itemIdsToShow);
			UpdateSize();
		}

		public int GetItemsPerLine()
		{
			return _itemsPerLine;
		}

		public override List<SnapPoint> GetSnapPoints()
		{
			List<SnapPoint> list = new List<SnapPoint>();
			GetGridParameters(out var startX, out var startY, out var startItemIndex, out var endItemIndex);
			_ = _itemsPerLine;
			Rectangle viewCullingArea = base.Parent.GetViewCullingArea();
			int num = endItemIndex - startItemIndex;
			while (_dummySnapPoints.Count < num)
			{
				_dummySnapPoints.Add(new SnapPoint("CreativeInfinitesSlot", 0, Vector2.Zero, Vector2.Zero));
			}
			int num2 = 0;
			Vector2 vector = GetDimensions().Position();
			for (int i = startItemIndex; i < endItemIndex; i++)
			{
				Point center = GetItemSlotHitbox(startX, startY, startItemIndex, i).Center;
				if (viewCullingArea.Contains(center))
				{
					SnapPoint snapPoint = _dummySnapPoints[num2];
					snapPoint.ThisIsAHackThatChangesTheSnapPointsInfo(Vector2.Zero, center.ToVector2() - vector, num2);
					snapPoint.Calculate(this);
					num2++;
					list.Add(snapPoint);
				}
			}
			foreach (UIElement element in Elements)
			{
				list.AddRange(element.GetSnapPoints());
			}
			return list;
		}

		public void UpdateSize()
		{
			int num = (_itemsPerLine = GetDimensions().ToRectangle().Width / 44);
			int num2 = (int)Math.Ceiling((float)_itemIdsAvailableToShow.Count / (float)num);
			MinHeight.Set(44 * num2, 0f);
		}
	}
}
