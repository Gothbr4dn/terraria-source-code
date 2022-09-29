using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIBestiaryEntryButton : UIElement
	{
		private UIImage _bordersGlow;

		private UIImage _bordersOverlay;

		private UIImage _borders;

		private UIBestiaryEntryIcon _icon;

		public BestiaryEntry Entry { get; private set; }

		public UIBestiaryEntryButton(BestiaryEntry entry, bool isAPrettyPortrait)
		{
			Entry = entry;
			Height.Set(72f, 0f);
			Width.Set(72f, 0f);
			SetPadding(0f);
			UIElement uIElement = new UIElement
			{
				Width = new StyleDimension(-4f, 1f),
				Height = new StyleDimension(-4f, 1f),
				IgnoresMouseInteraction = true,
				OverflowHidden = true,
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			uIElement.SetPadding(0f);
			uIElement.Append(new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Back", (AssetRequestMode)1))
			{
				VAlign = 0.5f,
				HAlign = 0.5f
			});
			if (isAPrettyPortrait)
			{
				Asset<Texture2D> val = TryGettingBackgroundImageProvider(entry);
				if (val != null)
				{
					uIElement.Append(new UIImage(val)
					{
						HAlign = 0.5f,
						VAlign = 0.5f
					});
				}
			}
			UIBestiaryEntryIcon uIBestiaryEntryIcon = new UIBestiaryEntryIcon(entry, isAPrettyPortrait);
			uIElement.Append(uIBestiaryEntryIcon);
			Append(uIElement);
			_icon = uIBestiaryEntryIcon;
			int? num = TryGettingDisplayIndex(entry);
			if (num.HasValue)
			{
				UIText element = new UIText(num.Value.ToString(), 0.9f)
				{
					Top = new StyleDimension(10f, 0f),
					Left = new StyleDimension(10f, 0f),
					IgnoresMouseInteraction = true
				};
				Append(element);
			}
			_bordersGlow = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Selection", (AssetRequestMode)1))
			{
				VAlign = 0.5f,
				HAlign = 0.5f,
				IgnoresMouseInteraction = true
			};
			_bordersOverlay = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Overlay", (AssetRequestMode)1))
			{
				VAlign = 0.5f,
				HAlign = 0.5f,
				IgnoresMouseInteraction = true,
				Color = Color.White * 0.6f
			};
			Append(_bordersOverlay);
			UIImage uIImage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Slot_Front", (AssetRequestMode)1))
			{
				VAlign = 0.5f,
				HAlign = 0.5f,
				IgnoresMouseInteraction = true
			};
			Append(uIImage);
			_borders = uIImage;
			if (isAPrettyPortrait)
			{
				RemoveChild(_bordersOverlay);
			}
			if (!isAPrettyPortrait)
			{
				base.OnMouseOver += MouseOver;
				base.OnMouseOut += MouseOut;
			}
		}

		private Asset<Texture2D> TryGettingBackgroundImageProvider(BestiaryEntry entry)
		{
			IEnumerable<IBestiaryBackgroundImagePathAndColorProvider> enumerable = from x in entry.Info
				where x is IBestiaryBackgroundImagePathAndColorProvider
				select x as IBestiaryBackgroundImagePathAndColorProvider;
			IEnumerable<IPreferenceProviderElement> preferences = entry.Info.OfType<IPreferenceProviderElement>();
			IEnumerable<IBestiaryBackgroundImagePathAndColorProvider> enumerable2 = enumerable.Where((IBestiaryBackgroundImagePathAndColorProvider provider) => preferences.Any((IPreferenceProviderElement preference) => preference.Matches(provider)));
			Asset<Texture2D> val = null;
			foreach (IBestiaryBackgroundImagePathAndColorProvider item in enumerable2)
			{
				val = item.GetBackgroundImage();
				if (val != null)
				{
					return val;
				}
			}
			foreach (IBestiaryBackgroundImagePathAndColorProvider item2 in enumerable)
			{
				val = item2.GetBackgroundImage();
				if (val != null)
				{
					return val;
				}
			}
			return null;
		}

		private int? TryGettingDisplayIndex(BestiaryEntry entry)
		{
			int? result = null;
			IBestiaryInfoElement bestiaryInfoElement = entry.Info.FirstOrDefault((IBestiaryInfoElement x) => x is IBestiaryEntryDisplayIndex);
			if (bestiaryInfoElement != null)
			{
				result = (bestiaryInfoElement as IBestiaryEntryDisplayIndex).BestiaryDisplayIndex;
			}
			return result;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			if (base.IsMouseHovering)
			{
				Main.instance.MouseText(_icon.GetHoverText(), 0, 0);
			}
		}

		private void MouseOver(UIMouseEvent evt, UIElement listeningElement)
		{
			SoundEngine.PlaySound(12);
			RemoveChild(_borders);
			RemoveChild(_bordersGlow);
			RemoveChild(_bordersOverlay);
			Append(_borders);
			Append(_bordersGlow);
			_icon.ForceHover = true;
		}

		private void MouseOut(UIMouseEvent evt, UIElement listeningElement)
		{
			RemoveChild(_borders);
			RemoveChild(_bordersGlow);
			RemoveChild(_bordersOverlay);
			Append(_bordersOverlay);
			Append(_borders);
			_icon.ForceHover = false;
		}
	}
}
