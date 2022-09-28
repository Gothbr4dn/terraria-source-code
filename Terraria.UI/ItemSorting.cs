using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;

namespace Terraria.UI
{
	public class ItemSorting
	{
		private class ItemSortingLayer
		{
			public readonly string Name;

			public readonly Func<ItemSortingLayer, Item[], List<int>, List<int>> SortingMethod;

			public ItemSortingLayer(string name, Func<ItemSortingLayer, Item[], List<int>, List<int>> method)
			{
				Name = name;
				SortingMethod = method;
			}

			public void Validate(ref List<int> indexesSortable, Item[] inv)
			{
				if (_layerWhiteLists.TryGetValue(Name, out var list))
				{
					indexesSortable = indexesSortable.Where((int i) => list.Contains(inv[i].netID)).ToList();
				}
			}

			public override string ToString()
			{
				return Name;
			}
		}

		private class ItemSortingLayers
		{
			public static ItemSortingLayer WeaponsMelee = new ItemSortingLayer("Weapons - Melee", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable38 = itemsToSort.Where((int i) => inv[i].maxStack == 1 && inv[i].damage > 0 && inv[i].ammo == 0 && inv[i].melee && inv[i].pick < 1 && inv[i].hammer < 1 && inv[i].axe < 1).ToList();
				layer.Validate(ref indexesSortable38, inv);
				foreach (int item in indexesSortable38)
				{
					itemsToSort.Remove(item);
				}
				indexesSortable38.Sort(delegate(int x, int y)
				{
					int num33 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num33 == 0)
					{
						num33 = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					if (num33 == 0)
					{
						num33 = x.CompareTo(y);
					}
					return num33;
				});
				return indexesSortable38;
			});

			public static ItemSortingLayer WeaponsRanged = new ItemSortingLayer("Weapons - Ranged", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable37 = itemsToSort.Where((int i) => inv[i].maxStack == 1 && inv[i].damage > 0 && inv[i].ammo == 0 && inv[i].ranged).ToList();
				layer.Validate(ref indexesSortable37, inv);
				foreach (int item2 in indexesSortable37)
				{
					itemsToSort.Remove(item2);
				}
				indexesSortable37.Sort(delegate(int x, int y)
				{
					int num32 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num32 == 0)
					{
						num32 = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					if (num32 == 0)
					{
						num32 = x.CompareTo(y);
					}
					return num32;
				});
				return indexesSortable37;
			});

			public static ItemSortingLayer WeaponsMagic = new ItemSortingLayer("Weapons - Magic", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable36 = itemsToSort.Where((int i) => inv[i].maxStack == 1 && inv[i].damage > 0 && inv[i].ammo == 0 && inv[i].magic).ToList();
				layer.Validate(ref indexesSortable36, inv);
				foreach (int item3 in indexesSortable36)
				{
					itemsToSort.Remove(item3);
				}
				indexesSortable36.Sort(delegate(int x, int y)
				{
					int num31 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num31 == 0)
					{
						num31 = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					if (num31 == 0)
					{
						num31 = x.CompareTo(y);
					}
					return num31;
				});
				return indexesSortable36;
			});

			public static ItemSortingLayer WeaponsMinions = new ItemSortingLayer("Weapons - Minions", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable35 = itemsToSort.Where((int i) => inv[i].maxStack == 1 && inv[i].damage > 0 && inv[i].summon).ToList();
				layer.Validate(ref indexesSortable35, inv);
				foreach (int item4 in indexesSortable35)
				{
					itemsToSort.Remove(item4);
				}
				indexesSortable35.Sort(delegate(int x, int y)
				{
					int num30 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num30 == 0)
					{
						num30 = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					if (num30 == 0)
					{
						num30 = x.CompareTo(y);
					}
					return num30;
				});
				return indexesSortable35;
			});

			public static ItemSortingLayer WeaponsAssorted = new ItemSortingLayer("Weapons - Assorted", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable34 = itemsToSort.Where((int i) => inv[i].damage > 0 && inv[i].ammo == 0 && inv[i].pick == 0 && inv[i].axe == 0 && inv[i].hammer == 0).ToList();
				layer.Validate(ref indexesSortable34, inv);
				foreach (int item5 in indexesSortable34)
				{
					itemsToSort.Remove(item5);
				}
				indexesSortable34.Sort(delegate(int x, int y)
				{
					int num29 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num29 == 0)
					{
						num29 = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					if (num29 == 0)
					{
						num29 = x.CompareTo(y);
					}
					return num29;
				});
				return indexesSortable34;
			});

			public static ItemSortingLayer WeaponsAmmo = new ItemSortingLayer("Weapons - Ammo", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable33 = itemsToSort.Where((int i) => inv[i].ammo > 0 && inv[i].damage > 0).ToList();
				layer.Validate(ref indexesSortable33, inv);
				foreach (int item6 in indexesSortable33)
				{
					itemsToSort.Remove(item6);
				}
				indexesSortable33.Sort(delegate(int x, int y)
				{
					int num28 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num28 == 0)
					{
						num28 = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					if (num28 == 0)
					{
						num28 = x.CompareTo(y);
					}
					return num28;
				});
				return indexesSortable33;
			});

			public static ItemSortingLayer ToolsPicksaws = new ItemSortingLayer("Tools - Picksaws", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable32 = itemsToSort.Where((int i) => inv[i].pick > 0 && inv[i].axe > 0).ToList();
				layer.Validate(ref indexesSortable32, inv);
				foreach (int item7 in indexesSortable32)
				{
					itemsToSort.Remove(item7);
				}
				indexesSortable32.Sort((int x, int y) => inv[x].pick.CompareTo(inv[y].pick));
				return indexesSortable32;
			});

			public static ItemSortingLayer ToolsHamaxes = new ItemSortingLayer("Tools - Hamaxes", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable31 = itemsToSort.Where((int i) => inv[i].hammer > 0 && inv[i].axe > 0).ToList();
				layer.Validate(ref indexesSortable31, inv);
				foreach (int item8 in indexesSortable31)
				{
					itemsToSort.Remove(item8);
				}
				indexesSortable31.Sort((int x, int y) => inv[x].axe.CompareTo(inv[y].axe));
				return indexesSortable31;
			});

			public static ItemSortingLayer ToolsPickaxes = new ItemSortingLayer("Tools - Pickaxes", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable30 = itemsToSort.Where((int i) => inv[i].pick > 0).ToList();
				layer.Validate(ref indexesSortable30, inv);
				foreach (int item9 in indexesSortable30)
				{
					itemsToSort.Remove(item9);
				}
				indexesSortable30.Sort((int x, int y) => inv[x].pick.CompareTo(inv[y].pick));
				return indexesSortable30;
			});

			public static ItemSortingLayer ToolsAxes = new ItemSortingLayer("Tools - Axes", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable29 = itemsToSort.Where((int i) => inv[i].pick > 0).ToList();
				layer.Validate(ref indexesSortable29, inv);
				foreach (int item10 in indexesSortable29)
				{
					itemsToSort.Remove(item10);
				}
				indexesSortable29.Sort((int x, int y) => inv[x].axe.CompareTo(inv[y].axe));
				return indexesSortable29;
			});

			public static ItemSortingLayer ToolsHammers = new ItemSortingLayer("Tools - Hammers", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable28 = itemsToSort.Where((int i) => inv[i].hammer > 0).ToList();
				layer.Validate(ref indexesSortable28, inv);
				foreach (int item11 in indexesSortable28)
				{
					itemsToSort.Remove(item11);
				}
				indexesSortable28.Sort((int x, int y) => inv[x].hammer.CompareTo(inv[y].hammer));
				return indexesSortable28;
			});

			public static ItemSortingLayer ToolsTerraforming = new ItemSortingLayer("Tools - Terraforming", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable27 = itemsToSort.Where((int i) => inv[i].netID > 0 && ItemID.Sets.SortingPriorityTerraforming[inv[i].netID] > -1).ToList();
				layer.Validate(ref indexesSortable27, inv);
				foreach (int item12 in indexesSortable27)
				{
					itemsToSort.Remove(item12);
				}
				indexesSortable27.Sort(delegate(int x, int y)
				{
					int num27 = ItemID.Sets.SortingPriorityTerraforming[inv[x].netID].CompareTo(ItemID.Sets.SortingPriorityTerraforming[inv[y].netID]);
					if (num27 == 0)
					{
						num27 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num27 == 0)
					{
						num27 = x.CompareTo(y);
					}
					return num27;
				});
				return indexesSortable27;
			});

			public static ItemSortingLayer ToolsAmmoLeftovers = new ItemSortingLayer("Weapons - Ammo Leftovers", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable26 = itemsToSort.Where((int i) => inv[i].ammo > 0).ToList();
				layer.Validate(ref indexesSortable26, inv);
				foreach (int item13 in indexesSortable26)
				{
					itemsToSort.Remove(item13);
				}
				indexesSortable26.Sort(delegate(int x, int y)
				{
					int num26 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num26 == 0)
					{
						num26 = inv[y].OriginalDamage.CompareTo(inv[x].OriginalDamage);
					}
					if (num26 == 0)
					{
						num26 = x.CompareTo(y);
					}
					return num26;
				});
				return indexesSortable26;
			});

			public static ItemSortingLayer ArmorCombat = new ItemSortingLayer("Armor - Combat", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable25 = itemsToSort.Where((int i) => (inv[i].bodySlot >= 0 || inv[i].headSlot >= 0 || inv[i].legSlot >= 0) && !inv[i].vanity).ToList();
				layer.Validate(ref indexesSortable25, inv);
				foreach (int item14 in indexesSortable25)
				{
					itemsToSort.Remove(item14);
				}
				indexesSortable25.Sort(delegate(int x, int y)
				{
					int num25 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num25 == 0)
					{
						num25 = inv[y].OriginalDefense.CompareTo(inv[x].OriginalDefense);
					}
					if (num25 == 0)
					{
						num25 = inv[x].netID.CompareTo(inv[y].netID);
					}
					return num25;
				});
				return indexesSortable25;
			});

			public static ItemSortingLayer ArmorVanity = new ItemSortingLayer("Armor - Vanity", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable24 = itemsToSort.Where((int i) => (inv[i].bodySlot >= 0 || inv[i].headSlot >= 0 || inv[i].legSlot >= 0) && inv[i].vanity).ToList();
				layer.Validate(ref indexesSortable24, inv);
				foreach (int item15 in indexesSortable24)
				{
					itemsToSort.Remove(item15);
				}
				indexesSortable24.Sort(delegate(int x, int y)
				{
					int num24 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num24 == 0)
					{
						num24 = inv[x].netID.CompareTo(inv[y].netID);
					}
					return num24;
				});
				return indexesSortable24;
			});

			public static ItemSortingLayer ArmorAccessories = new ItemSortingLayer("Armor - Accessories", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable23 = itemsToSort.Where((int i) => inv[i].accessory).ToList();
				layer.Validate(ref indexesSortable23, inv);
				foreach (int item16 in indexesSortable23)
				{
					itemsToSort.Remove(item16);
				}
				indexesSortable23.Sort(delegate(int x, int y)
				{
					int num23 = inv[x].vanity.CompareTo(inv[y].vanity);
					if (num23 == 0)
					{
						num23 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num23 == 0)
					{
						num23 = inv[y].OriginalDefense.CompareTo(inv[x].OriginalDefense);
					}
					if (num23 == 0)
					{
						num23 = inv[x].netID.CompareTo(inv[y].netID);
					}
					return num23;
				});
				return indexesSortable23;
			});

			public static ItemSortingLayer EquipGrapple = new ItemSortingLayer("Equip - Grapple", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable22 = itemsToSort.Where((int i) => Main.projHook[inv[i].shoot]).ToList();
				layer.Validate(ref indexesSortable22, inv);
				foreach (int item17 in indexesSortable22)
				{
					itemsToSort.Remove(item17);
				}
				indexesSortable22.Sort(delegate(int x, int y)
				{
					int num22 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num22 == 0)
					{
						num22 = inv[x].netID.CompareTo(inv[y].netID);
					}
					if (num22 == 0)
					{
						num22 = x.CompareTo(y);
					}
					return num22;
				});
				return indexesSortable22;
			});

			public static ItemSortingLayer EquipMount = new ItemSortingLayer("Equip - Mount", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable21 = itemsToSort.Where((int i) => inv[i].mountType != -1 && !MountID.Sets.Cart[inv[i].mountType]).ToList();
				layer.Validate(ref indexesSortable21, inv);
				foreach (int item18 in indexesSortable21)
				{
					itemsToSort.Remove(item18);
				}
				indexesSortable21.Sort(delegate(int x, int y)
				{
					int num21 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num21 == 0)
					{
						num21 = inv[x].netID.CompareTo(inv[y].netID);
					}
					if (num21 == 0)
					{
						num21 = x.CompareTo(y);
					}
					return num21;
				});
				return indexesSortable21;
			});

			public static ItemSortingLayer EquipCart = new ItemSortingLayer("Equip - Cart", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable20 = itemsToSort.Where((int i) => inv[i].mountType != -1 && MountID.Sets.Cart[inv[i].mountType]).ToList();
				layer.Validate(ref indexesSortable20, inv);
				foreach (int item19 in indexesSortable20)
				{
					itemsToSort.Remove(item19);
				}
				indexesSortable20.Sort(delegate(int x, int y)
				{
					int num20 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num20 == 0)
					{
						num20 = inv[x].netID.CompareTo(inv[y].netID);
					}
					if (num20 == 0)
					{
						num20 = x.CompareTo(y);
					}
					return num20;
				});
				return indexesSortable20;
			});

			public static ItemSortingLayer EquipLightPet = new ItemSortingLayer("Equip - Light Pet", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable19 = itemsToSort.Where((int i) => inv[i].buffType > 0 && Main.lightPet[inv[i].buffType]).ToList();
				layer.Validate(ref indexesSortable19, inv);
				foreach (int item20 in indexesSortable19)
				{
					itemsToSort.Remove(item20);
				}
				indexesSortable19.Sort(delegate(int x, int y)
				{
					int num19 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num19 == 0)
					{
						num19 = inv[x].netID.CompareTo(inv[y].netID);
					}
					if (num19 == 0)
					{
						num19 = x.CompareTo(y);
					}
					return num19;
				});
				return indexesSortable19;
			});

			public static ItemSortingLayer EquipVanityPet = new ItemSortingLayer("Equip - Vanity Pet", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable18 = itemsToSort.Where((int i) => inv[i].buffType > 0 && Main.vanityPet[inv[i].buffType]).ToList();
				layer.Validate(ref indexesSortable18, inv);
				foreach (int item21 in indexesSortable18)
				{
					itemsToSort.Remove(item21);
				}
				indexesSortable18.Sort(delegate(int x, int y)
				{
					int num18 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num18 == 0)
					{
						num18 = inv[x].netID.CompareTo(inv[y].netID);
					}
					if (num18 == 0)
					{
						num18 = x.CompareTo(y);
					}
					return num18;
				});
				return indexesSortable18;
			});

			public static ItemSortingLayer PotionsLife = new ItemSortingLayer("Potions - Life", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable17 = itemsToSort.Where((int i) => inv[i].consumable && inv[i].healLife > 0 && inv[i].healMana < 1).ToList();
				layer.Validate(ref indexesSortable17, inv);
				foreach (int item22 in indexesSortable17)
				{
					itemsToSort.Remove(item22);
				}
				indexesSortable17.Sort(delegate(int x, int y)
				{
					int num17 = inv[y].healLife.CompareTo(inv[x].healLife);
					if (num17 == 0)
					{
						num17 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num17 == 0)
					{
						num17 = x.CompareTo(y);
					}
					return num17;
				});
				return indexesSortable17;
			});

			public static ItemSortingLayer PotionsMana = new ItemSortingLayer("Potions - Mana", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable16 = itemsToSort.Where((int i) => inv[i].consumable && inv[i].healLife < 1 && inv[i].healMana > 0).ToList();
				layer.Validate(ref indexesSortable16, inv);
				foreach (int item23 in indexesSortable16)
				{
					itemsToSort.Remove(item23);
				}
				indexesSortable16.Sort(delegate(int x, int y)
				{
					int num16 = inv[y].healMana.CompareTo(inv[x].healMana);
					if (num16 == 0)
					{
						num16 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num16 == 0)
					{
						num16 = x.CompareTo(y);
					}
					return num16;
				});
				return indexesSortable16;
			});

			public static ItemSortingLayer PotionsElixirs = new ItemSortingLayer("Potions - Elixirs", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable15 = itemsToSort.Where((int i) => inv[i].consumable && inv[i].healLife > 0 && inv[i].healMana > 0).ToList();
				layer.Validate(ref indexesSortable15, inv);
				foreach (int item24 in indexesSortable15)
				{
					itemsToSort.Remove(item24);
				}
				indexesSortable15.Sort(delegate(int x, int y)
				{
					int num15 = inv[y].healLife.CompareTo(inv[x].healLife);
					if (num15 == 0)
					{
						num15 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num15 == 0)
					{
						num15 = x.CompareTo(y);
					}
					return num15;
				});
				return indexesSortable15;
			});

			public static ItemSortingLayer PotionsBuffs = new ItemSortingLayer("Potions - Buffs", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable14 = itemsToSort.Where((int i) => inv[i].consumable && inv[i].buffType > 0).ToList();
				layer.Validate(ref indexesSortable14, inv);
				foreach (int item25 in indexesSortable14)
				{
					itemsToSort.Remove(item25);
				}
				indexesSortable14.Sort(delegate(int x, int y)
				{
					int num14 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num14 == 0)
					{
						num14 = inv[x].netID.CompareTo(inv[y].netID);
					}
					if (num14 == 0)
					{
						num14 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num14 == 0)
					{
						num14 = x.CompareTo(y);
					}
					return num14;
				});
				return indexesSortable14;
			});

			public static ItemSortingLayer PotionsDyes = new ItemSortingLayer("Potions - Dyes", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable13 = itemsToSort.Where((int i) => inv[i].dye > 0).ToList();
				layer.Validate(ref indexesSortable13, inv);
				foreach (int item26 in indexesSortable13)
				{
					itemsToSort.Remove(item26);
				}
				indexesSortable13.Sort(delegate(int x, int y)
				{
					int num13 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num13 == 0)
					{
						num13 = inv[y].dye.CompareTo(inv[x].dye);
					}
					if (num13 == 0)
					{
						num13 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num13 == 0)
					{
						num13 = x.CompareTo(y);
					}
					return num13;
				});
				return indexesSortable13;
			});

			public static ItemSortingLayer PotionsHairDyes = new ItemSortingLayer("Potions - Hair Dyes", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable12 = itemsToSort.Where((int i) => inv[i].hairDye >= 0).ToList();
				layer.Validate(ref indexesSortable12, inv);
				foreach (int item27 in indexesSortable12)
				{
					itemsToSort.Remove(item27);
				}
				indexesSortable12.Sort(delegate(int x, int y)
				{
					int num12 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num12 == 0)
					{
						num12 = inv[y].hairDye.CompareTo(inv[x].hairDye);
					}
					if (num12 == 0)
					{
						num12 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num12 == 0)
					{
						num12 = x.CompareTo(y);
					}
					return num12;
				});
				return indexesSortable12;
			});

			public static ItemSortingLayer MiscValuables = new ItemSortingLayer("Misc - Importants", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable11 = itemsToSort.Where((int i) => inv[i].netID > 0 && ItemID.Sets.SortingPriorityBossSpawns[inv[i].netID] > -1).ToList();
				layer.Validate(ref indexesSortable11, inv);
				foreach (int item28 in indexesSortable11)
				{
					itemsToSort.Remove(item28);
				}
				indexesSortable11.Sort(delegate(int x, int y)
				{
					int num11 = ItemID.Sets.SortingPriorityBossSpawns[inv[x].netID].CompareTo(ItemID.Sets.SortingPriorityBossSpawns[inv[y].netID]);
					if (num11 == 0)
					{
						num11 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num11 == 0)
					{
						num11 = x.CompareTo(y);
					}
					return num11;
				});
				return indexesSortable11;
			});

			public static ItemSortingLayer MiscWiring = new ItemSortingLayer("Misc - Wiring", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable10 = itemsToSort.Where((int i) => (inv[i].netID > 0 && ItemID.Sets.SortingPriorityWiring[inv[i].netID] > -1) || inv[i].mech).ToList();
				layer.Validate(ref indexesSortable10, inv);
				foreach (int item29 in indexesSortable10)
				{
					itemsToSort.Remove(item29);
				}
				indexesSortable10.Sort(delegate(int x, int y)
				{
					int num10 = ItemID.Sets.SortingPriorityWiring[inv[y].netID].CompareTo(ItemID.Sets.SortingPriorityWiring[inv[x].netID]);
					if (num10 == 0)
					{
						num10 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					}
					if (num10 == 0)
					{
						num10 = inv[y].netID.CompareTo(inv[x].netID);
					}
					if (num10 == 0)
					{
						num10 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num10 == 0)
					{
						num10 = x.CompareTo(y);
					}
					return num10;
				});
				return indexesSortable10;
			});

			public static ItemSortingLayer MiscMaterials = new ItemSortingLayer("Misc - Materials", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable9 = itemsToSort.Where((int i) => inv[i].netID > 0 && ItemID.Sets.SortingPriorityMaterials[inv[i].netID] > -1).ToList();
				layer.Validate(ref indexesSortable9, inv);
				foreach (int item30 in indexesSortable9)
				{
					itemsToSort.Remove(item30);
				}
				indexesSortable9.Sort(delegate(int x, int y)
				{
					int num9 = ItemID.Sets.SortingPriorityMaterials[inv[y].netID].CompareTo(ItemID.Sets.SortingPriorityMaterials[inv[x].netID]);
					if (num9 == 0)
					{
						num9 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num9 == 0)
					{
						num9 = x.CompareTo(y);
					}
					return num9;
				});
				return indexesSortable9;
			});

			public static ItemSortingLayer MiscExtractinator = new ItemSortingLayer("Misc - Extractinator", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable8 = itemsToSort.Where((int i) => inv[i].netID > 0 && ItemID.Sets.SortingPriorityExtractibles[inv[i].netID] > -1).ToList();
				layer.Validate(ref indexesSortable8, inv);
				foreach (int item31 in indexesSortable8)
				{
					itemsToSort.Remove(item31);
				}
				indexesSortable8.Sort(delegate(int x, int y)
				{
					int num8 = ItemID.Sets.SortingPriorityExtractibles[inv[y].netID].CompareTo(ItemID.Sets.SortingPriorityExtractibles[inv[x].netID]);
					if (num8 == 0)
					{
						num8 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num8 == 0)
					{
						num8 = x.CompareTo(y);
					}
					return num8;
				});
				return indexesSortable8;
			});

			public static ItemSortingLayer MiscPainting = new ItemSortingLayer("Misc - Painting", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable7 = itemsToSort.Where((int i) => (inv[i].netID > 0 && ItemID.Sets.SortingPriorityPainting[inv[i].netID] > -1) || inv[i].paint > 0).ToList();
				layer.Validate(ref indexesSortable7, inv);
				foreach (int item32 in indexesSortable7)
				{
					itemsToSort.Remove(item32);
				}
				indexesSortable7.Sort(delegate(int x, int y)
				{
					int num7 = ItemID.Sets.SortingPriorityPainting[inv[y].netID].CompareTo(ItemID.Sets.SortingPriorityPainting[inv[x].netID]);
					if (num7 == 0)
					{
						num7 = inv[x].paint.CompareTo(inv[y].paint);
					}
					if (num7 == 0)
					{
						num7 = inv[x].paintCoating.CompareTo(inv[y].paintCoating);
					}
					if (num7 == 0)
					{
						num7 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num7 == 0)
					{
						num7 = x.CompareTo(y);
					}
					return num7;
				});
				return indexesSortable7;
			});

			public static ItemSortingLayer MiscRopes = new ItemSortingLayer("Misc - Ropes", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable6 = itemsToSort.Where((int i) => inv[i].netID > 0 && ItemID.Sets.SortingPriorityRopes[inv[i].netID] > -1).ToList();
				layer.Validate(ref indexesSortable6, inv);
				foreach (int item33 in indexesSortable6)
				{
					itemsToSort.Remove(item33);
				}
				indexesSortable6.Sort(delegate(int x, int y)
				{
					int num6 = ItemID.Sets.SortingPriorityRopes[inv[y].netID].CompareTo(ItemID.Sets.SortingPriorityRopes[inv[x].netID]);
					if (num6 == 0)
					{
						num6 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num6 == 0)
					{
						num6 = x.CompareTo(y);
					}
					return num6;
				});
				return indexesSortable6;
			});

			public static ItemSortingLayer LastMaterials = new ItemSortingLayer("Last - Materials", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable5 = itemsToSort.Where((int i) => inv[i].createTile < 0 && inv[i].createWall < 1).ToList();
				layer.Validate(ref indexesSortable5, inv);
				foreach (int item34 in indexesSortable5)
				{
					itemsToSort.Remove(item34);
				}
				indexesSortable5.Sort(delegate(int x, int y)
				{
					int num5 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num5 == 0)
					{
						num5 = inv[y].value.CompareTo(inv[x].value);
					}
					if (num5 == 0)
					{
						num5 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num5 == 0)
					{
						num5 = x.CompareTo(y);
					}
					return num5;
				});
				return indexesSortable5;
			});

			public static ItemSortingLayer LastTilesImportant = new ItemSortingLayer("Last - Tiles (Frame Important)", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable4 = itemsToSort.Where((int i) => inv[i].createTile >= 0 && Main.tileFrameImportant[inv[i].createTile]).ToList();
				layer.Validate(ref indexesSortable4, inv);
				foreach (int item35 in indexesSortable4)
				{
					itemsToSort.Remove(item35);
				}
				indexesSortable4.Sort(delegate(int x, int y)
				{
					int num4 = string.Compare(inv[x].Name, inv[y].Name, StringComparison.OrdinalIgnoreCase);
					if (num4 == 0)
					{
						num4 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num4 == 0)
					{
						num4 = x.CompareTo(y);
					}
					return num4;
				});
				return indexesSortable4;
			});

			public static ItemSortingLayer LastTilesCommon = new ItemSortingLayer("Last - Tiles (Common), Walls", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable3 = itemsToSort.Where((int i) => inv[i].createWall > 0 || inv[i].createTile >= 0).ToList();
				layer.Validate(ref indexesSortable3, inv);
				foreach (int item36 in indexesSortable3)
				{
					itemsToSort.Remove(item36);
				}
				indexesSortable3.Sort(delegate(int x, int y)
				{
					int num3 = string.Compare(inv[x].Name, inv[y].Name, StringComparison.OrdinalIgnoreCase);
					if (num3 == 0)
					{
						num3 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num3 == 0)
					{
						num3 = x.CompareTo(y);
					}
					return num3;
				});
				return indexesSortable3;
			});

			public static ItemSortingLayer LastNotTrash = new ItemSortingLayer("Last - Not Trash", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable2 = itemsToSort.Where((int i) => inv[i].OriginalRarity >= 0).ToList();
				layer.Validate(ref indexesSortable2, inv);
				foreach (int item37 in indexesSortable2)
				{
					itemsToSort.Remove(item37);
				}
				indexesSortable2.Sort(delegate(int x, int y)
				{
					int num2 = inv[y].OriginalRarity.CompareTo(inv[x].OriginalRarity);
					if (num2 == 0)
					{
						num2 = string.Compare(inv[x].Name, inv[y].Name, StringComparison.OrdinalIgnoreCase);
					}
					if (num2 == 0)
					{
						num2 = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num2 == 0)
					{
						num2 = x.CompareTo(y);
					}
					return num2;
				});
				return indexesSortable2;
			});

			public static ItemSortingLayer LastTrash = new ItemSortingLayer("Last - Trash", delegate(ItemSortingLayer layer, Item[] inv, List<int> itemsToSort)
			{
				List<int> indexesSortable = new List<int>(itemsToSort);
				layer.Validate(ref indexesSortable, inv);
				foreach (int item38 in indexesSortable)
				{
					itemsToSort.Remove(item38);
				}
				indexesSortable.Sort(delegate(int x, int y)
				{
					int num = inv[y].value.CompareTo(inv[x].value);
					if (num == 0)
					{
						num = inv[y].stack.CompareTo(inv[x].stack);
					}
					if (num == 0)
					{
						num = x.CompareTo(y);
					}
					return num;
				});
				return indexesSortable;
			});
		}

		private static List<ItemSortingLayer> _layerList = new List<ItemSortingLayer>();

		private static Dictionary<string, List<int>> _layerWhiteLists = new Dictionary<string, List<int>>();

		public static void SetupWhiteLists()
		{
			_layerWhiteLists.Clear();
			List<ItemSortingLayer> list = new List<ItemSortingLayer>();
			List<Item> list2 = new List<Item>();
			List<int> list3 = new List<int>();
			list.Add(ItemSortingLayers.WeaponsMelee);
			list.Add(ItemSortingLayers.WeaponsRanged);
			list.Add(ItemSortingLayers.WeaponsMagic);
			list.Add(ItemSortingLayers.WeaponsMinions);
			list.Add(ItemSortingLayers.WeaponsAssorted);
			list.Add(ItemSortingLayers.WeaponsAmmo);
			list.Add(ItemSortingLayers.ToolsPicksaws);
			list.Add(ItemSortingLayers.ToolsHamaxes);
			list.Add(ItemSortingLayers.ToolsPickaxes);
			list.Add(ItemSortingLayers.ToolsAxes);
			list.Add(ItemSortingLayers.ToolsHammers);
			list.Add(ItemSortingLayers.ToolsTerraforming);
			list.Add(ItemSortingLayers.ToolsAmmoLeftovers);
			list.Add(ItemSortingLayers.ArmorCombat);
			list.Add(ItemSortingLayers.ArmorVanity);
			list.Add(ItemSortingLayers.ArmorAccessories);
			list.Add(ItemSortingLayers.EquipGrapple);
			list.Add(ItemSortingLayers.EquipMount);
			list.Add(ItemSortingLayers.EquipCart);
			list.Add(ItemSortingLayers.EquipLightPet);
			list.Add(ItemSortingLayers.EquipVanityPet);
			list.Add(ItemSortingLayers.PotionsDyes);
			list.Add(ItemSortingLayers.PotionsHairDyes);
			list.Add(ItemSortingLayers.PotionsLife);
			list.Add(ItemSortingLayers.PotionsMana);
			list.Add(ItemSortingLayers.PotionsElixirs);
			list.Add(ItemSortingLayers.PotionsBuffs);
			list.Add(ItemSortingLayers.MiscValuables);
			list.Add(ItemSortingLayers.MiscPainting);
			list.Add(ItemSortingLayers.MiscWiring);
			list.Add(ItemSortingLayers.MiscMaterials);
			list.Add(ItemSortingLayers.MiscRopes);
			list.Add(ItemSortingLayers.MiscExtractinator);
			list.Add(ItemSortingLayers.LastMaterials);
			list.Add(ItemSortingLayers.LastTilesImportant);
			list.Add(ItemSortingLayers.LastTilesCommon);
			list.Add(ItemSortingLayers.LastNotTrash);
			list.Add(ItemSortingLayers.LastTrash);
			for (int i = -48; i < 5453; i++)
			{
				Item item = new Item();
				item.netDefaults(i);
				list2.Add(item);
				list3.Add(i + 48);
			}
			Item[] array = list2.ToArray();
			foreach (ItemSortingLayer item2 in list)
			{
				List<int> list4 = item2.SortingMethod(item2, array, list3);
				List<int> list5 = new List<int>();
				for (int j = 0; j < list4.Count; j++)
				{
					list5.Add(array[list4[j]].netID);
				}
				_layerWhiteLists.Add(item2.Name, list5);
			}
		}

		private static void SetupSortingPriorities()
		{
			Player player = Main.player[Main.myPlayer];
			_layerList.Clear();
			List<float> list = new List<float> { player.meleeDamage, player.rangedDamage, player.magicDamage, player.minionDamage };
			list.Sort((float x, float y) => y.CompareTo(x));
			for (int i = 0; i < 5; i++)
			{
				if (!_layerList.Contains(ItemSortingLayers.WeaponsMelee) && player.meleeDamage == list[0])
				{
					list.RemoveAt(0);
					_layerList.Add(ItemSortingLayers.WeaponsMelee);
				}
				if (!_layerList.Contains(ItemSortingLayers.WeaponsRanged) && player.rangedDamage == list[0])
				{
					list.RemoveAt(0);
					_layerList.Add(ItemSortingLayers.WeaponsRanged);
				}
				if (!_layerList.Contains(ItemSortingLayers.WeaponsMagic) && player.magicDamage == list[0])
				{
					list.RemoveAt(0);
					_layerList.Add(ItemSortingLayers.WeaponsMagic);
				}
				if (!_layerList.Contains(ItemSortingLayers.WeaponsMinions) && player.minionDamage == list[0])
				{
					list.RemoveAt(0);
					_layerList.Add(ItemSortingLayers.WeaponsMinions);
				}
			}
			_layerList.Add(ItemSortingLayers.WeaponsAssorted);
			_layerList.Add(ItemSortingLayers.WeaponsAmmo);
			_layerList.Add(ItemSortingLayers.ToolsPicksaws);
			_layerList.Add(ItemSortingLayers.ToolsHamaxes);
			_layerList.Add(ItemSortingLayers.ToolsPickaxes);
			_layerList.Add(ItemSortingLayers.ToolsAxes);
			_layerList.Add(ItemSortingLayers.ToolsHammers);
			_layerList.Add(ItemSortingLayers.ToolsTerraforming);
			_layerList.Add(ItemSortingLayers.ToolsAmmoLeftovers);
			_layerList.Add(ItemSortingLayers.ArmorCombat);
			_layerList.Add(ItemSortingLayers.ArmorVanity);
			_layerList.Add(ItemSortingLayers.ArmorAccessories);
			_layerList.Add(ItemSortingLayers.EquipGrapple);
			_layerList.Add(ItemSortingLayers.EquipMount);
			_layerList.Add(ItemSortingLayers.EquipCart);
			_layerList.Add(ItemSortingLayers.EquipLightPet);
			_layerList.Add(ItemSortingLayers.EquipVanityPet);
			_layerList.Add(ItemSortingLayers.PotionsDyes);
			_layerList.Add(ItemSortingLayers.PotionsHairDyes);
			_layerList.Add(ItemSortingLayers.PotionsLife);
			_layerList.Add(ItemSortingLayers.PotionsMana);
			_layerList.Add(ItemSortingLayers.PotionsElixirs);
			_layerList.Add(ItemSortingLayers.PotionsBuffs);
			_layerList.Add(ItemSortingLayers.MiscValuables);
			_layerList.Add(ItemSortingLayers.MiscPainting);
			_layerList.Add(ItemSortingLayers.MiscWiring);
			_layerList.Add(ItemSortingLayers.MiscMaterials);
			_layerList.Add(ItemSortingLayers.MiscRopes);
			_layerList.Add(ItemSortingLayers.MiscExtractinator);
			_layerList.Add(ItemSortingLayers.LastMaterials);
			_layerList.Add(ItemSortingLayers.LastTilesImportant);
			_layerList.Add(ItemSortingLayers.LastTilesCommon);
			_layerList.Add(ItemSortingLayers.LastNotTrash);
			_layerList.Add(ItemSortingLayers.LastTrash);
		}

		private static void Sort(Item[] inv, params int[] ignoreSlots)
		{
			SetupSortingPriorities();
			List<int> list = new List<int>();
			for (int i = 0; i < inv.Length; i++)
			{
				if (!ignoreSlots.Contains(i))
				{
					Item item = inv[i];
					if (item != null && item.stack != 0 && item.type != 0 && !item.favorited)
					{
						list.Add(i);
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				Item item2 = inv[list[j]];
				if (item2.stack >= item2.maxStack)
				{
					continue;
				}
				int num = item2.maxStack - item2.stack;
				for (int k = j; k < list.Count; k++)
				{
					if (j == k)
					{
						continue;
					}
					Item item3 = inv[list[k]];
					if (item2.type == item3.type && item3.stack != item3.maxStack)
					{
						int num2 = item3.stack;
						if (num < num2)
						{
							num2 = num;
						}
						item2.stack += num2;
						item3.stack -= num2;
						num -= num2;
						if (item3.stack == 0)
						{
							inv[list[k]] = new Item();
							list.Remove(list[k]);
							j--;
							k--;
							break;
						}
						if (num == 0)
						{
							break;
						}
					}
				}
			}
			List<int> list2 = new List<int>(list);
			for (int l = 0; l < inv.Length; l++)
			{
				if (!ignoreSlots.Contains(l) && !list2.Contains(l))
				{
					Item item4 = inv[l];
					if (item4 == null || item4.stack == 0 || item4.type == 0)
					{
						list2.Add(l);
					}
				}
			}
			list2.Sort();
			List<int> list3 = new List<int>();
			List<int> list4 = new List<int>();
			foreach (ItemSortingLayer layer in _layerList)
			{
				List<int> list5 = layer.SortingMethod(layer, inv, list);
				if (list5.Count > 0)
				{
					list4.Add(list5.Count);
				}
				list3.AddRange(list5);
			}
			list3.AddRange(list);
			List<Item> list6 = new List<Item>();
			foreach (int item5 in list3)
			{
				list6.Add(inv[item5]);
				inv[item5] = new Item();
			}
			float num3 = 1f / (float)list4.Count;
			float num4 = num3 / 2f;
			for (int m = 0; m < list6.Count; m++)
			{
				int num5 = list2[0];
				ItemSlot.SetGlow(num5, num4, Main.player[Main.myPlayer].chest != -1);
				list4[0]--;
				if (list4[0] == 0)
				{
					list4.RemoveAt(0);
					num4 += num3;
				}
				inv[num5] = list6[m];
				list2.Remove(num5);
			}
		}

		public static void SortInventory()
		{
			if (!Main.LocalPlayer.HasItem(905))
			{
				SortCoins();
			}
			SortAmmo();
			Sort(Main.player[Main.myPlayer].inventory, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 50, 51, 52, 53, 54, 55, 56, 57, 58);
		}

		public static void SortChest()
		{
			int chest = Main.player[Main.myPlayer].chest;
			if (chest == -1)
			{
				return;
			}
			Item[] item = Main.player[Main.myPlayer].bank.item;
			if (chest == -3)
			{
				item = Main.player[Main.myPlayer].bank2.item;
			}
			if (chest == -4)
			{
				item = Main.player[Main.myPlayer].bank3.item;
			}
			if (chest == -5)
			{
				item = Main.player[Main.myPlayer].bank4.item;
			}
			if (chest > -1)
			{
				item = Main.chest[chest].item;
			}
			Tuple<int, int, int>[] array = new Tuple<int, int, int>[40];
			for (int i = 0; i < 40; i++)
			{
				array[i] = Tuple.Create(item[i].netID, item[i].stack, (int)item[i].prefix);
			}
			Sort(item);
			Tuple<int, int, int>[] array2 = new Tuple<int, int, int>[40];
			for (int j = 0; j < 40; j++)
			{
				array2[j] = Tuple.Create(item[j].netID, item[j].stack, (int)item[j].prefix);
			}
			if (Main.netMode != 1 || Main.player[Main.myPlayer].chest <= -1)
			{
				return;
			}
			for (int k = 0; k < 40; k++)
			{
				if (array2[k] != array[k])
				{
					NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, k);
				}
			}
		}

		public static void SortAmmo()
		{
			ClearAmmoSlotSpaces();
			FillAmmoFromInventory();
		}

		public static void FillAmmoFromInventory()
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			Item[] inventory = Main.player[Main.myPlayer].inventory;
			for (int i = 54; i < 58; i++)
			{
				ItemSlot.SetGlow(i, 0.31f, chest: false);
				Item item = inventory[i];
				if (item.IsAir)
				{
					list2.Add(i);
				}
				else if (item.ammo != AmmoID.None)
				{
					if (!list.Contains(item.type))
					{
						list.Add(item.type);
					}
					RefillItemStack(inventory, inventory[i], 0, 50);
				}
			}
			if (list2.Count < 1)
			{
				return;
			}
			for (int j = 0; j < 50; j++)
			{
				Item item2 = inventory[j];
				if (item2.stack >= 1 && item2.CanFillEmptyAmmoSlot() && list.Contains(item2.type))
				{
					int num = list2[0];
					list2.Remove(num);
					Utils.Swap(ref inventory[j], ref inventory[num]);
					RefillItemStack(inventory, inventory[num], 0, 50);
					if (list2.Count == 0)
					{
						break;
					}
				}
			}
			if (list2.Count < 1)
			{
				return;
			}
			for (int k = 0; k < 50; k++)
			{
				Item item3 = inventory[k];
				if (item3.stack >= 1 && item3.CanFillEmptyAmmoSlot() && item3.FitsAmmoSlot())
				{
					int num2 = list2[0];
					list2.Remove(num2);
					Utils.Swap(ref inventory[k], ref inventory[num2]);
					RefillItemStack(inventory, inventory[num2], 0, 50);
					if (list2.Count == 0)
					{
						break;
					}
				}
			}
		}

		public static void ClearAmmoSlotSpaces()
		{
			Item[] inventory = Main.player[Main.myPlayer].inventory;
			for (int i = 54; i < 58; i++)
			{
				Item item = inventory[i];
				if (!item.IsAir && item.ammo != AmmoID.None && item.stack < item.maxStack)
				{
					RefillItemStack(inventory, item, i + 1, 58);
				}
			}
			for (int j = 54; j < 58; j++)
			{
				if (inventory[j].type > 0)
				{
					TrySlidingUp(inventory, j, 54);
				}
			}
		}

		private static void SortCoins()
		{
			Item[] inventory = Main.LocalPlayer.inventory;
			bool overFlowing;
			long count = Utils.CoinsCount(out overFlowing, inventory, 58);
			int commonMaxStack = Item.CommonMaxStack;
			if (overFlowing)
			{
				return;
			}
			int[] array = Utils.CoinsSplit(count);
			int num = 0;
			for (int i = 0; i < 3; i++)
			{
				int num2 = array[i];
				while (num2 > 0)
				{
					num2 -= 99;
					num++;
				}
			}
			int num3 = array[3];
			while (num3 > commonMaxStack)
			{
				num3 -= commonMaxStack;
				num++;
			}
			int num4 = 0;
			for (int j = 0; j < 58; j++)
			{
				if (inventory[j].type >= 71 && inventory[j].type <= 74 && inventory[j].stack > 0)
				{
					num4++;
				}
			}
			if (num4 < num)
			{
				return;
			}
			for (int k = 0; k < 58; k++)
			{
				if (inventory[k].type >= 71 && inventory[k].type <= 74 && inventory[k].stack > 0)
				{
					inventory[k].TurnToAir();
				}
			}
			int num5 = 100;
			while (true)
			{
				int num6 = -1;
				for (int num7 = 3; num7 >= 0; num7--)
				{
					if (array[num7] > 0)
					{
						num6 = num7;
						break;
					}
				}
				if (num6 == -1)
				{
					break;
				}
				int num8 = array[num6];
				if (num6 == 3 && num8 > commonMaxStack)
				{
					num8 = commonMaxStack;
				}
				bool flag = false;
				if (!flag)
				{
					for (int l = 50; l < 54; l++)
					{
						if (inventory[l].IsAir)
						{
							inventory[l].SetDefaults(71 + num6);
							inventory[l].stack = num8;
							array[num6] -= num8;
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					for (int m = 0; m < 50; m++)
					{
						if (inventory[m].IsAir)
						{
							inventory[m].SetDefaults(71 + num6);
							inventory[m].stack = num8;
							array[num6] -= num8;
							flag = true;
							break;
						}
					}
				}
				num5--;
				if (num5 > 0)
				{
					continue;
				}
				for (int num9 = 3; num9 >= 0; num9--)
				{
					if (array[num9] > 0)
					{
						Main.LocalPlayer.QuickSpawnItem(Main.LocalPlayer.GetItemSource_Misc(7), 71 + num9, array[num9]);
					}
				}
				break;
			}
		}

		private static void RefillItemStack(Item[] inv, Item itemToRefill, int loopStartIndex, int loopEndIndex)
		{
			int num = itemToRefill.maxStack - itemToRefill.stack;
			if (num <= 0)
			{
				return;
			}
			for (int i = loopStartIndex; i < loopEndIndex; i++)
			{
				Item item = inv[i];
				if (item.stack >= 1 && item.type == itemToRefill.type)
				{
					int num2 = item.stack;
					if (num2 > num)
					{
						num2 = num;
					}
					num -= num2;
					itemToRefill.stack += num2;
					item.stack -= num2;
					if (item.stack <= 0)
					{
						item.TurnToAir();
					}
					if (num <= 0)
					{
						break;
					}
				}
			}
		}

		private static void TrySlidingUp(Item[] inv, int slot, int minimumIndex)
		{
			for (int num = slot; num > minimumIndex; num--)
			{
				if (inv[num - 1].IsAir)
				{
					Utils.Swap(ref inv[num], ref inv[num - 1]);
				}
			}
		}
	}
}
