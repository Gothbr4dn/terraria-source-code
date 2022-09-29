using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIBestiaryInfoItemLine : UIPanel, IManuallyOrderedUIElement
	{
		private Item _infoDisplayItem;

		private bool _hideMouseOver;

		public int OrderInUIList { get; set; }

		public UIBestiaryInfoItemLine(DropRateInfo info, BestiaryUICollectionInfo uiinfo, float textScale = 1f)
		{
			_infoDisplayItem = new Item();
			_infoDisplayItem.SetDefaults(info.itemId);
			SetBestiaryNotesOnItemCache(info);
			SetPadding(0f);
			PaddingLeft = 10f;
			PaddingRight = 10f;
			Width.Set(-14f, 1f);
			Height.Set(32f, 0f);
			Left.Set(5f, 0f);
			base.OnMouseOver += MouseOver;
			base.OnMouseOut += MouseOut;
			BorderColor = new Color(89, 116, 213, 255);
			GetDropInfo(info, uiinfo, out var stackRange, out var droprate);
			if (uiinfo.UnlockState < BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3)
			{
				_hideMouseOver = true;
				Asset<Texture2D> texture = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Locked", (AssetRequestMode)1);
				UIElement uIElement = new UIElement
				{
					Height = new StyleDimension(0f, 1f),
					Width = new StyleDimension(0f, 1f),
					HAlign = 0.5f,
					VAlign = 0.5f
				};
				uIElement.SetPadding(0f);
				UIImage element = new UIImage(texture)
				{
					ImageScale = 0.55f,
					HAlign = 0.5f,
					VAlign = 0.5f
				};
				uIElement.Append(element);
				Append(uIElement);
			}
			else
			{
				UIItemIcon element2 = new UIItemIcon(_infoDisplayItem, uiinfo.UnlockState < BestiaryEntryUnlockState.CanShowDropsWithoutDropRates_3)
				{
					IgnoresMouseInteraction = true,
					HAlign = 0f,
					Left = new StyleDimension(4f, 0f)
				};
				Append(element2);
				if (!string.IsNullOrEmpty(stackRange))
				{
					droprate = stackRange + " " + droprate;
				}
				UITextPanel<string> element3 = new UITextPanel<string>(droprate, textScale)
				{
					IgnoresMouseInteraction = true,
					DrawPanel = false,
					HAlign = 1f,
					Top = new StyleDimension(-4f, 0f)
				};
				Append(element3);
			}
		}

		protected void GetDropInfo(DropRateInfo dropRateInfo, BestiaryUICollectionInfo uiinfo, out string stackRange, out string droprate)
		{
			if (dropRateInfo.stackMin != dropRateInfo.stackMax)
			{
				stackRange = $" ({dropRateInfo.stackMin}-{dropRateInfo.stackMax})";
			}
			else if (dropRateInfo.stackMin == 1)
			{
				stackRange = "";
			}
			else
			{
				stackRange = " (" + dropRateInfo.stackMin + ")";
			}
			string originalFormat = "P";
			if ((double)dropRateInfo.dropRate < 0.001)
			{
				originalFormat = "P4";
			}
			if (dropRateInfo.dropRate != 1f)
			{
				droprate = Utils.PrettifyPercentDisplay(dropRateInfo.dropRate, originalFormat);
			}
			else
			{
				droprate = "100%";
			}
			if (uiinfo.UnlockState != BestiaryEntryUnlockState.CanShowDropsWithDropRates_4)
			{
				droprate = "???";
				stackRange = "";
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			if (base.IsMouseHovering && !_hideMouseOver)
			{
				DrawMouseOver();
			}
		}

		private void DrawMouseOver()
		{
			Main.HoverItem = _infoDisplayItem;
			Main.instance.MouseText("", 0, 0);
			Main.mouseText = true;
		}

		public override int CompareTo(object obj)
		{
			if (obj is IManuallyOrderedUIElement manuallyOrderedUIElement)
			{
				return OrderInUIList.CompareTo(manuallyOrderedUIElement.OrderInUIList);
			}
			return base.CompareTo(obj);
		}

		private void SetBestiaryNotesOnItemCache(DropRateInfo info)
		{
			List<string> list = new List<string>();
			if (info.conditions == null)
			{
				return;
			}
			foreach (IItemDropRuleCondition condition in info.conditions)
			{
				if (condition != null)
				{
					string conditionDescription = condition.GetConditionDescription();
					if (!string.IsNullOrWhiteSpace(conditionDescription))
					{
						list.Add(conditionDescription);
					}
				}
			}
			_infoDisplayItem.BestiaryNotes = string.Join("\n", list);
		}

		private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12);
			BorderColor = Colors.FancyUIFatButtonMouseOver;
		}

		private void MouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			BorderColor = new Color(89, 116, 213, 255);
		}
	}
}
