using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.Creative
{
	public static class ItemFilters
	{
		public class BySearch : IItemEntryFilter, IEntryFilter<Item>, ISearchFilter<Item>
		{
			private const int _tooltipMaxLines = 30;

			private string[] _toolTipLines = new string[30];

			private bool[] _unusedPrefixLine = new bool[30];

			private bool[] _unusedBadPrefixLines = new bool[30];

			private int _unusedYoyoLogo;

			private int _unusedResearchLine;

			private string _search;

			public bool FitsFilter(Item entry)
			{
				if (_search == null)
				{
					return true;
				}
				int numLines = 1;
				float knockBack = entry.knockBack;
				Main.MouseText_DrawItemTooltip_GetLinesInfo(entry, ref _unusedYoyoLogo, ref _unusedResearchLine, knockBack, ref numLines, _toolTipLines, _unusedPrefixLine, _unusedBadPrefixLines);
				for (int i = 0; i < numLines; i++)
				{
					if (_toolTipLines[i].ToLower().IndexOf(_search, StringComparison.OrdinalIgnoreCase) != -1)
					{
						return true;
					}
				}
				return false;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabSearch";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Icon_Rank_Light", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame())
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}

			public void SetSearch(string searchText)
			{
				_search = searchText;
			}
		}

		public class BuildingBlock : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				if (entry.createWall != -1)
				{
					return true;
				}
				if (entry.tileWand != -1)
				{
					return true;
				}
				if (entry.createTile == -1)
				{
					return false;
				}
				return !Main.tileFrameImportant[entry.createTile];
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabBlocks";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 4).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Furniture : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				int createTile = entry.createTile;
				if (createTile == -1)
				{
					return false;
				}
				return Main.tileFrameImportant[createTile];
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabFurniture";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 7).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Tools : IItemEntryFilter, IEntryFilter<Item>
		{
			private HashSet<int> _itemIdsThatAreAccepted = new HashSet<int>
			{
				509, 850, 851, 3612, 3625, 3611, 510, 849, 3620, 1071,
				1543, 1072, 1544, 1100, 1545, 50, 3199, 3124, 5358, 5359,
				5360, 5361, 5437, 1326, 5335, 3384, 4263, 4819, 4262, 946,
				4707, 205, 206, 207, 1128, 3031, 4820, 5302, 5364, 4460,
				4608, 4872, 3032, 5303, 5304, 1991, 4821, 3183, 779, 5134,
				1299, 4711, 4049, 114
			};

			public bool FitsFilter(Item entry)
			{
				if (entry.pick > 0)
				{
					return true;
				}
				if (entry.axe > 0)
				{
					return true;
				}
				if (entry.hammer > 0)
				{
					return true;
				}
				if (entry.fishingPole > 0)
				{
					return true;
				}
				if (entry.tileWand != -1)
				{
					return true;
				}
				if (_itemIdsThatAreAccepted.Contains(entry.type))
				{
					return true;
				}
				return false;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabTools";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 6).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Weapon : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return entry.damage > 0;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabWeapons";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public abstract class AArmor
		{
			public bool IsAnArmorThatMatchesSocialState(Item entry, bool shouldBeSocial)
			{
				if (entry.bodySlot != -1 || entry.headSlot != -1 || entry.legSlot != -1)
				{
					return entry.vanity == shouldBeSocial;
				}
				return false;
			}
		}

		public class Armor : AArmor, IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return IsAnArmorThatMatchesSocialState(entry, shouldBeSocial: false);
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabArmor";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 2).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Vanity : AArmor, IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return IsAnArmorThatMatchesSocialState(entry, shouldBeSocial: true);
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabVanity";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 8).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public abstract class AAccessories
		{
			public enum AccessoriesCategory
			{
				Misc,
				NonMisc
			}

			public bool IsAnAccessoryOfType(Item entry, AccessoriesCategory categoryType)
			{
				bool flag = ItemSlot.IsMiscEquipment(entry);
				if (flag && categoryType == AccessoriesCategory.Misc)
				{
					return true;
				}
				if (!flag && categoryType == AccessoriesCategory.NonMisc && entry.accessory)
				{
					return true;
				}
				return false;
			}
		}

		public class Accessories : AAccessories, IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return IsAnAccessoryOfType(entry, AccessoriesCategory.NonMisc);
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabAccessories";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 1).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class MiscAccessories : AAccessories, IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return IsAnAccessoryOfType(entry, AccessoriesCategory.Misc);
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabAccessoriesMisc";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 9).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Consumables : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				int type = entry.type;
				if (type == 267 || type == 1307)
				{
					return true;
				}
				bool flag = entry.createTile != -1 || entry.createWall != -1 || entry.tileWand != -1;
				if (entry.consumable)
				{
					return !flag;
				}
				return false;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabConsumables";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 3).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class GameplayItems : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return ItemID.Sets.SortingPriorityBossSpawns[entry.type] != -1;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabMisc";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 5).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class MiscFallback : IItemEntryFilter, IEntryFilter<Item>
		{
			private bool[] _fitsFilterByItemType;

			public MiscFallback(List<IItemEntryFilter> otherFiltersToCheckAgainst)
			{
				short num = 5453;
				_fitsFilterByItemType = new bool[num];
				for (int i = 1; i < num; i++)
				{
					_fitsFilterByItemType[i] = true;
					Item entry = ContentSamples.ItemsByType[i];
					if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(i, out var _))
					{
						_fitsFilterByItemType[i] = false;
						continue;
					}
					for (int j = 0; j < otherFiltersToCheckAgainst.Count; j++)
					{
						if (otherFiltersToCheckAgainst[j].FitsFilter(entry))
						{
							_fitsFilterByItemType[i] = false;
							break;
						}
					}
				}
			}

			public bool FitsFilter(Item entry)
			{
				if (_fitsFilterByItemType.IndexInRange(entry.type))
				{
					return _fitsFilterByItemType[entry.type];
				}
				return false;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabMisc";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 5).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		public class Materials : IItemEntryFilter, IEntryFilter<Item>
		{
			public bool FitsFilter(Item entry)
			{
				return entry.material;
			}

			public string GetDisplayNameKey()
			{
				return "CreativePowers.TabMaterials";
			}

			public UIElement GetImage()
			{
				Asset<Texture2D> obj = Main.Assets.Request<Texture2D>("Images/UI/Creative/Infinite_Icons", (AssetRequestMode)1);
				return new UIImageFramed(obj, obj.Frame(11, 1, 10).OffsetSize(-2, 0))
				{
					HAlign = 0.5f,
					VAlign = 0.5f
				};
			}
		}

		private const int framesPerRow = 11;

		private const int framesPerColumn = 1;

		private const int frameSizeOffsetX = -2;

		private const int frameSizeOffsetY = 0;
	}
}
