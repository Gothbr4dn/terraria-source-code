using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.UI
{
	public class ItemSlot
	{
		public class Options
		{
			public static bool DisableLeftShiftTrashCan = true;

			public static bool DisableQuickTrash = false;

			public static bool HighlightNewItems = true;
		}

		public class Context
		{
			public const int InventoryItem = 0;

			public const int InventoryCoin = 1;

			public const int InventoryAmmo = 2;

			public const int ChestItem = 3;

			public const int BankItem = 4;

			public const int PrefixItem = 5;

			public const int TrashItem = 6;

			public const int GuideItem = 7;

			public const int EquipArmor = 8;

			public const int EquipArmorVanity = 9;

			public const int EquipAccessory = 10;

			public const int EquipAccessoryVanity = 11;

			public const int EquipDye = 12;

			public const int HotbarItem = 13;

			public const int ChatItem = 14;

			public const int ShopItem = 15;

			public const int EquipGrapple = 16;

			public const int EquipMount = 17;

			public const int EquipMinecart = 18;

			public const int EquipPet = 19;

			public const int EquipLight = 20;

			public const int MouseItem = 21;

			public const int CraftingMaterial = 22;

			public const int DisplayDollArmor = 23;

			public const int DisplayDollAccessory = 24;

			public const int DisplayDollDye = 25;

			public const int HatRackHat = 26;

			public const int HatRackDye = 27;

			public const int GoldDebug = 28;

			public const int CreativeInfinite = 29;

			public const int CreativeSacrifice = 30;

			public const int InWorld = 31;

			public const int VoidItem = 32;

			public const int EquipMiscDye = 33;

			public const int Count = 34;
		}

		public struct ItemTransferInfo
		{
			public int ItemType;

			public int TransferAmount;

			public int FromContenxt;

			public int ToContext;

			public ItemTransferInfo(Item itemAfter, int fromContext, int toContext, int transferAmount = 0)
			{
				ItemType = itemAfter.type;
				TransferAmount = itemAfter.stack;
				if (transferAmount != 0)
				{
					TransferAmount = transferAmount;
				}
				FromContenxt = fromContext;
				ToContext = toContext;
			}
		}

		public delegate void ItemTransferEvent(ItemTransferInfo info);

		public static bool DrawGoldBGForCraftingMaterial;

		public static bool ShiftForcedOn;

		private static Item[] singleSlotArray;

		private static bool[] canFavoriteAt;

		private static bool[] canShareAt;

		private static float[] inventoryGlowHue;

		private static int[] inventoryGlowTime;

		private static float[] inventoryGlowHueChest;

		private static int[] inventoryGlowTimeChest;

		private static int _customCurrencyForSavings;

		public static bool forceClearGlowsOnChest;

		private static double _lastTimeForVisualEffectsThatLoadoutWasChanged;

		private static Color[,] LoadoutSlotColors;

		private static int dyeSlotCount;

		private static int accSlotToSwapTo;

		public static float CircularRadialOpacity;

		public static float QuicksRadialOpacity;

		public static bool ShiftInUse
		{
			get
			{
				if (!Main.keyState.PressingShift())
				{
					return ShiftForcedOn;
				}
				return true;
			}
		}

		public static bool ControlInUse => Main.keyState.PressingControl();

		public static bool NotUsingGamepad => !PlayerInput.UsingGamepad;

		public static event ItemTransferEvent OnItemTransferred;

		static ItemSlot()
		{
			DrawGoldBGForCraftingMaterial = false;
			singleSlotArray = new Item[1];
			canFavoriteAt = new bool[34];
			canShareAt = new bool[34];
			inventoryGlowHue = new float[58];
			inventoryGlowTime = new int[58];
			inventoryGlowHueChest = new float[58];
			inventoryGlowTimeChest = new int[58];
			_customCurrencyForSavings = -1;
			forceClearGlowsOnChest = false;
			LoadoutSlotColors = new Color[3, 3]
			{
				{
					new Color(50, 106, 64),
					new Color(46, 106, 98),
					new Color(45, 85, 105)
				},
				{
					new Color(35, 106, 126),
					new Color(50, 89, 140),
					new Color(57, 70, 128)
				},
				{
					new Color(122, 63, 83),
					new Color(104, 46, 85),
					new Color(84, 37, 87)
				}
			};
			canFavoriteAt[0] = true;
			canFavoriteAt[1] = true;
			canFavoriteAt[2] = true;
			canFavoriteAt[32] = true;
			canShareAt[15] = true;
			canShareAt[4] = true;
			canShareAt[32] = true;
			canShareAt[5] = true;
			canShareAt[6] = true;
			canShareAt[7] = true;
			canShareAt[27] = true;
			canShareAt[26] = true;
			canShareAt[23] = true;
			canShareAt[24] = true;
			canShareAt[25] = true;
			canShareAt[22] = true;
			canShareAt[3] = true;
			canShareAt[8] = true;
			canShareAt[9] = true;
			canShareAt[10] = true;
			canShareAt[11] = true;
			canShareAt[12] = true;
			canShareAt[33] = true;
			canShareAt[16] = true;
			canShareAt[20] = true;
			canShareAt[18] = true;
			canShareAt[19] = true;
			canShareAt[17] = true;
			canShareAt[29] = true;
			canShareAt[30] = true;
		}

		public static void AnnounceTransfer(ItemTransferInfo info)
		{
			if (ItemSlot.OnItemTransferred != null)
			{
				ItemSlot.OnItemTransferred(info);
			}
		}

		public static void SetGlow(int index, float hue, bool chest)
		{
			if (chest)
			{
				if (hue < 0f)
				{
					inventoryGlowTimeChest[index] = 0;
					inventoryGlowHueChest[index] = 0f;
				}
				else
				{
					inventoryGlowTimeChest[index] = 300;
					inventoryGlowHueChest[index] = hue;
				}
			}
			else
			{
				inventoryGlowTime[index] = 300;
				inventoryGlowHue[index] = hue;
			}
		}

		public static void UpdateInterface()
		{
			if (!Main.playerInventory || Main.player[Main.myPlayer].talkNPC == -1)
			{
				_customCurrencyForSavings = -1;
			}
			for (int i = 0; i < inventoryGlowTime.Length; i++)
			{
				if (inventoryGlowTime[i] > 0)
				{
					inventoryGlowTime[i]--;
					if (inventoryGlowTime[i] == 0)
					{
						inventoryGlowHue[i] = 0f;
					}
				}
			}
			for (int j = 0; j < inventoryGlowTimeChest.Length; j++)
			{
				if (inventoryGlowTimeChest[j] > 0)
				{
					inventoryGlowTimeChest[j]--;
					if (inventoryGlowTimeChest[j] == 0 || forceClearGlowsOnChest)
					{
						inventoryGlowHueChest[j] = 0f;
					}
				}
			}
			forceClearGlowsOnChest = false;
		}

		public static void Handle(ref Item inv, int context = 0)
		{
			singleSlotArray[0] = inv;
			Handle(singleSlotArray, context);
			inv = singleSlotArray[0];
			Recipe.FindRecipes();
		}

		public static void Handle(Item[] inv, int context = 0, int slot = 0)
		{
			OverrideHover(inv, context, slot);
			LeftClick(inv, context, slot);
			RightClick(inv, context, slot);
			if (Main.mouseLeftRelease && Main.mouseLeft)
			{
				Recipe.FindRecipes();
			}
			MouseHover(inv, context, slot);
		}

		public static void OverrideHover(ref Item inv, int context = 0)
		{
			singleSlotArray[0] = inv;
			OverrideHover(singleSlotArray, context);
			inv = singleSlotArray[0];
		}

		public static bool isEquipLocked(int type)
		{
			if (Main.npcShop > 0 && (type == 854 || type == 3035))
			{
				return true;
			}
			return false;
		}

		public static void OverrideHover(Item[] inv, int context = 0, int slot = 0)
		{
			Item item = inv[slot];
			if (!PlayerInput.UsingGamepad)
			{
				UILinkPointNavigator.SuggestUsage(GetGamepadPointForSlot(inv, context, slot));
			}
			bool shiftForcedOn = ShiftForcedOn;
			if (NotUsingGamepad && Options.DisableLeftShiftTrashCan && !shiftForcedOn)
			{
				if (ControlInUse && !Options.DisableQuickTrash)
				{
					if (item.type > 0 && item.stack > 0 && !inv[slot].favorited)
					{
						switch (context)
						{
						case 0:
						case 1:
						case 2:
							if (Main.npcShop > 0 && !item.favorited)
							{
								Main.cursorOverride = 10;
							}
							else
							{
								Main.cursorOverride = 6;
							}
							break;
						case 3:
						case 4:
						case 7:
						case 32:
							if (Main.player[Main.myPlayer].ItemSpace(item).CanTakeItemToPersonalInventory)
							{
								Main.cursorOverride = 6;
							}
							break;
						}
					}
				}
				else if (ShiftInUse)
				{
					bool flag = false;
					if (Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity())
					{
						flag = Main.LocalPlayer.tileEntityAnchor.GetTileEntity().OverrideItemSlotHover(inv, context, slot);
					}
					if (item.type > 0 && item.stack > 0 && !inv[slot].favorited && !flag)
					{
						switch (context)
						{
						case 0:
							if (Main.CreativeMenu.IsShowingResearchMenu())
							{
								Main.cursorOverride = 9;
								break;
							}
							goto case 1;
						case 1:
						case 2:
							if (context == 0 && Main.InReforgeMenu)
							{
								if (item.maxStack == 1 && item.Prefix(-3))
								{
									Main.cursorOverride = 9;
								}
							}
							else if (context == 0 && Main.InGuideCraftMenu)
							{
								if (item.material)
								{
									Main.cursorOverride = 9;
								}
							}
							else if (Main.player[Main.myPlayer].chest != -1 && ChestUI.TryPlacingInChest(item, justCheck: true, context))
							{
								Main.cursorOverride = 9;
							}
							break;
						case 3:
						case 4:
						case 32:
							if (Main.player[Main.myPlayer].ItemSpace(item).CanTakeItemToPersonalInventory)
							{
								Main.cursorOverride = 8;
							}
							break;
						case 5:
						case 7:
						case 8:
						case 9:
						case 10:
						case 11:
						case 12:
						case 16:
						case 17:
						case 18:
						case 19:
						case 20:
						case 23:
						case 24:
						case 25:
						case 26:
						case 27:
						case 29:
						case 33:
							if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
							{
								Main.cursorOverride = 7;
							}
							break;
						}
					}
				}
			}
			else if (ShiftInUse)
			{
				bool flag2 = false;
				if (Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity())
				{
					flag2 = Main.LocalPlayer.tileEntityAnchor.GetTileEntity().OverrideItemSlotHover(inv, context, slot);
				}
				if (item.type > 0 && item.stack > 0 && !inv[slot].favorited && !flag2)
				{
					switch (context)
					{
					case 0:
					case 1:
					case 2:
						if (Main.npcShop > 0 && !item.favorited)
						{
							if (!Options.DisableQuickTrash)
							{
								Main.cursorOverride = 10;
							}
						}
						else if (context == 0 && Main.CreativeMenu.IsShowingResearchMenu())
						{
							Main.cursorOverride = 9;
						}
						else if (context == 0 && Main.InReforgeMenu)
						{
							if (item.maxStack == 1 && item.Prefix(-3))
							{
								Main.cursorOverride = 9;
							}
						}
						else if (context == 0 && Main.InGuideCraftMenu)
						{
							if (item.material)
							{
								Main.cursorOverride = 9;
							}
						}
						else if (Main.player[Main.myPlayer].chest != -1)
						{
							if (ChestUI.TryPlacingInChest(item, justCheck: true, context))
							{
								Main.cursorOverride = 9;
							}
						}
						else if (!Options.DisableQuickTrash)
						{
							Main.cursorOverride = 6;
						}
						break;
					case 3:
					case 4:
					case 32:
						if (Main.player[Main.myPlayer].ItemSpace(item).CanTakeItemToPersonalInventory)
						{
							Main.cursorOverride = 8;
						}
						break;
					case 5:
					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 18:
					case 19:
					case 20:
					case 23:
					case 24:
					case 25:
					case 26:
					case 27:
					case 29:
					case 33:
						if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
						{
							Main.cursorOverride = 7;
						}
						break;
					}
				}
			}
			if (Main.keyState.IsKeyDown(Main.FavoriteKey) && (canFavoriteAt[context] || (Main.drawingPlayerChat && canShareAt[context])))
			{
				if (item.type > 0 && item.stack > 0 && Main.drawingPlayerChat)
				{
					Main.cursorOverride = 2;
				}
				else if (item.type > 0 && item.stack > 0)
				{
					Main.cursorOverride = 3;
				}
			}
		}

		private static bool OverrideLeftClick(Item[] inv, int context = 0, int slot = 0)
		{
			if (context == 10 && isEquipLocked(inv[slot].type))
			{
				return true;
			}
			if (Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity() && Main.LocalPlayer.tileEntityAnchor.GetTileEntity().OverrideItemSlotLeftClick(inv, context, slot))
			{
				return true;
			}
			Item item = inv[slot];
			if (Main.cursorOverride == 2)
			{
				if (ChatManager.AddChatText(FontAssets.MouseText.get_Value(), ItemTagHandler.GenerateTag(item), Vector2.One))
				{
					SoundEngine.PlaySound(12);
				}
				return true;
			}
			if (Main.cursorOverride == 3)
			{
				if (!canFavoriteAt[context])
				{
					return false;
				}
				item.favorited = !item.favorited;
				SoundEngine.PlaySound(12);
				return true;
			}
			if (Main.cursorOverride == 7)
			{
				if (context == 29)
				{
					Item item2 = new Item();
					item2.SetDefaults(inv[slot].netID);
					item2.stack = item2.maxStack;
					item2.OnCreated(new JourneyDuplicationItemCreationContext());
					item2 = Main.player[Main.myPlayer].GetItem(Main.myPlayer, item2, GetItemSettings.InventoryEntityToPlayerInventorySettings);
					SoundEngine.PlaySound(12);
					return true;
				}
				inv[slot] = Main.player[Main.myPlayer].GetItem(Main.myPlayer, inv[slot], GetItemSettings.InventoryEntityToPlayerInventorySettings);
				SoundEngine.PlaySound(12);
				return true;
			}
			if (Main.cursorOverride == 8)
			{
				inv[slot] = Main.player[Main.myPlayer].GetItem(Main.myPlayer, inv[slot], GetItemSettings.InventoryEntityToPlayerInventorySettings);
				if (Main.player[Main.myPlayer].chest > -1)
				{
					NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, slot);
				}
				return true;
			}
			if (Main.cursorOverride == 9)
			{
				if (Main.CreativeMenu.IsShowingResearchMenu())
				{
					Main.CreativeMenu.SwapItem(ref inv[slot]);
					SoundEngine.PlaySound(7);
					Main.CreativeMenu.SacrificeItemInSacrificeSlot();
				}
				else if (Main.InReforgeMenu)
				{
					Utils.Swap(ref inv[slot], ref Main.reforgeItem);
					SoundEngine.PlaySound(7);
				}
				else if (Main.InGuideCraftMenu)
				{
					Utils.Swap(ref inv[slot], ref Main.guideItem);
					Recipe.FindRecipes();
					SoundEngine.PlaySound(7);
				}
				else
				{
					ChestUI.TryPlacingInChest(inv[slot], justCheck: false, context);
				}
				return true;
			}
			return false;
		}

		public static void LeftClick(ref Item inv, int context = 0)
		{
			singleSlotArray[0] = inv;
			LeftClick(singleSlotArray, context);
			inv = singleSlotArray[0];
		}

		public static void LeftClick(Item[] inv, int context = 0, int slot = 0)
		{
			Player player = Main.player[Main.myPlayer];
			bool flag = Main.mouseLeftRelease && Main.mouseLeft;
			if (flag)
			{
				if (OverrideLeftClick(inv, context, slot))
				{
					return;
				}
				inv[slot].newAndShiny = false;
				if (LeftClick_SellOrTrash(inv, context, slot) || player.itemAnimation != 0 || player.itemTime != 0)
				{
					return;
				}
			}
			int num = PickItemMovementAction(inv, context, slot, Main.mouseItem);
			if (num != 3 && !flag)
			{
				return;
			}
			switch (num)
			{
			case 0:
				if (context == 6 && Main.mouseItem.type != 0)
				{
					inv[slot].SetDefaults();
				}
				if (context == 11 && !inv[slot].FitsAccessoryVanitySlot)
				{
					break;
				}
				Utils.Swap(ref inv[slot], ref Main.mouseItem);
				if (inv[slot].stack > 0)
				{
					AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, context, inv[slot].stack));
				}
				else
				{
					AnnounceTransfer(new ItemTransferInfo(Main.mouseItem, context, 21, Main.mouseItem.stack));
				}
				if (inv[slot].stack > 0)
				{
					switch (context)
					{
					case 0:
						AchievementsHelper.NotifyItemPickup(player, inv[slot]);
						break;
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 25:
					case 27:
					case 33:
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
						break;
					}
				}
				if (inv[slot].type == 0 || inv[slot].stack < 1)
				{
					inv[slot] = new Item();
				}
				if (Main.mouseItem.IsTheSameAs(inv[slot]))
				{
					Utils.Swap(ref inv[slot].favorited, ref Main.mouseItem.favorited);
					if (inv[slot].stack != inv[slot].maxStack && Main.mouseItem.stack != Main.mouseItem.maxStack)
					{
						if (Main.mouseItem.stack + inv[slot].stack <= Main.mouseItem.maxStack)
						{
							inv[slot].stack += Main.mouseItem.stack;
							Main.mouseItem.stack = 0;
							AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, context, inv[slot].stack));
						}
						else
						{
							int num2 = Main.mouseItem.maxStack - inv[slot].stack;
							inv[slot].stack += num2;
							Main.mouseItem.stack -= num2;
							AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, context, num2));
						}
					}
				}
				if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
				{
					Main.mouseItem = new Item();
				}
				if (Main.mouseItem.type > 0 || inv[slot].type > 0)
				{
					Recipe.FindRecipes();
					SoundEngine.PlaySound(7);
				}
				if (context == 3 && Main.netMode == 1)
				{
					NetMessage.SendData(32, -1, -1, null, player.chest, slot);
				}
				break;
			case 1:
				if (Main.mouseItem.stack == 1 && Main.mouseItem.type > 0 && inv[slot].type > 0 && inv[slot].IsNotTheSameAs(Main.mouseItem) && (context != 11 || Main.mouseItem.FitsAccessoryVanitySlot))
				{
					Utils.Swap(ref inv[slot], ref Main.mouseItem);
					SoundEngine.PlaySound(7);
					if (inv[slot].stack > 0)
					{
						switch (context)
						{
						case 0:
							AchievementsHelper.NotifyItemPickup(player, inv[slot]);
							break;
						case 8:
						case 9:
						case 10:
						case 11:
						case 12:
						case 16:
						case 17:
						case 25:
						case 27:
						case 33:
							AchievementsHelper.HandleOnEquip(player, inv[slot], context);
							break;
						}
					}
				}
				else if (Main.mouseItem.type == 0 && inv[slot].type > 0)
				{
					Utils.Swap(ref inv[slot], ref Main.mouseItem);
					if (inv[slot].type == 0 || inv[slot].stack < 1)
					{
						inv[slot] = new Item();
					}
					if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
					{
						Main.mouseItem = new Item();
					}
					if (Main.mouseItem.type > 0 || inv[slot].type > 0)
					{
						Recipe.FindRecipes();
						SoundEngine.PlaySound(7);
					}
				}
				else if (Main.mouseItem.type > 0 && inv[slot].type == 0 && (context != 11 || Main.mouseItem.FitsAccessoryVanitySlot))
				{
					if (Main.mouseItem.stack == 1)
					{
						Utils.Swap(ref inv[slot], ref Main.mouseItem);
						if (inv[slot].type == 0 || inv[slot].stack < 1)
						{
							inv[slot] = new Item();
						}
						if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
						{
							Main.mouseItem = new Item();
						}
						if (Main.mouseItem.type > 0 || inv[slot].type > 0)
						{
							Recipe.FindRecipes();
							SoundEngine.PlaySound(7);
						}
					}
					else
					{
						Main.mouseItem.stack--;
						inv[slot].SetDefaults(Main.mouseItem.type);
						Recipe.FindRecipes();
						SoundEngine.PlaySound(7);
					}
					if (inv[slot].stack > 0)
					{
						switch (context)
						{
						case 0:
							AchievementsHelper.NotifyItemPickup(player, inv[slot]);
							break;
						case 8:
						case 9:
						case 10:
						case 11:
						case 12:
						case 16:
						case 17:
						case 25:
						case 27:
						case 33:
							AchievementsHelper.HandleOnEquip(player, inv[slot], context);
							break;
						}
					}
				}
				if ((context == 23 || context == 24) && Main.netMode == 1)
				{
					NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
				}
				if (context == 26 && Main.netMode == 1)
				{
					NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
				}
				break;
			case 2:
				if (Main.mouseItem.stack == 1 && Main.mouseItem.dye > 0 && inv[slot].type > 0 && inv[slot].type != Main.mouseItem.type)
				{
					Utils.Swap(ref inv[slot], ref Main.mouseItem);
					SoundEngine.PlaySound(7);
					if (inv[slot].stack > 0)
					{
						switch (context)
						{
						case 0:
							AchievementsHelper.NotifyItemPickup(player, inv[slot]);
							break;
						case 8:
						case 9:
						case 10:
						case 11:
						case 12:
						case 16:
						case 17:
						case 25:
						case 27:
						case 33:
							AchievementsHelper.HandleOnEquip(player, inv[slot], context);
							break;
						}
					}
				}
				else if (Main.mouseItem.type == 0 && inv[slot].type > 0)
				{
					Utils.Swap(ref inv[slot], ref Main.mouseItem);
					if (inv[slot].type == 0 || inv[slot].stack < 1)
					{
						inv[slot] = new Item();
					}
					if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
					{
						Main.mouseItem = new Item();
					}
					if (Main.mouseItem.type > 0 || inv[slot].type > 0)
					{
						Recipe.FindRecipes();
						SoundEngine.PlaySound(7);
					}
				}
				else if (Main.mouseItem.dye > 0 && inv[slot].type == 0)
				{
					if (Main.mouseItem.stack == 1)
					{
						Utils.Swap(ref inv[slot], ref Main.mouseItem);
						if (inv[slot].type == 0 || inv[slot].stack < 1)
						{
							inv[slot] = new Item();
						}
						if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
						{
							Main.mouseItem = new Item();
						}
						if (Main.mouseItem.type > 0 || inv[slot].type > 0)
						{
							Recipe.FindRecipes();
							SoundEngine.PlaySound(7);
						}
					}
					else
					{
						Main.mouseItem.stack--;
						inv[slot].SetDefaults(Main.mouseItem.type);
						Recipe.FindRecipes();
						SoundEngine.PlaySound(7);
					}
					if (inv[slot].stack > 0)
					{
						switch (context)
						{
						case 0:
							AchievementsHelper.NotifyItemPickup(player, inv[slot]);
							break;
						case 8:
						case 9:
						case 10:
						case 11:
						case 12:
						case 16:
						case 17:
						case 25:
						case 27:
						case 33:
							AchievementsHelper.HandleOnEquip(player, inv[slot], context);
							break;
						}
					}
				}
				if (context == 25 && Main.netMode == 1)
				{
					NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
				}
				if (context == 27 && Main.netMode == 1)
				{
					NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
				}
				break;
			case 3:
				HandleShopSlot(inv, slot, rightClickIsValid: false, leftClickIsValid: true);
				break;
			case 4:
			{
				Chest chest = Main.instance.shop[Main.npcShop];
				if (player.SellItem(Main.mouseItem))
				{
					chest.AddItemToShop(Main.mouseItem);
					Main.mouseItem.SetDefaults();
					SoundEngine.PlaySound(18);
					AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, 15));
				}
				else if (Main.mouseItem.value == 0)
				{
					chest.AddItemToShop(Main.mouseItem);
					Main.mouseItem.SetDefaults();
					SoundEngine.PlaySound(7);
					AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, 15));
				}
				Recipe.FindRecipes();
				Main.stackSplit = 9999;
				break;
			}
			case 5:
				if (Main.mouseItem.IsAir)
				{
					SoundEngine.PlaySound(7);
					Main.mouseItem.SetDefaults(inv[slot].netID);
					Main.mouseItem.stack = Main.mouseItem.maxStack;
					Main.mouseItem.OnCreated(new JourneyDuplicationItemCreationContext());
					AnnounceTransfer(new ItemTransferInfo(inv[slot], 29, 21));
				}
				break;
			}
			if ((uint)context > 2u && context != 5 && context != 32)
			{
				inv[slot].favorited = false;
			}
		}

		private static bool DisableTrashing()
		{
			if (Options.DisableLeftShiftTrashCan)
			{
				return !PlayerInput.SteamDeckIsUsed;
			}
			return false;
		}

		private static bool LeftClick_SellOrTrash(Item[] inv, int context, int slot)
		{
			bool flag = false;
			bool result = false;
			if (NotUsingGamepad && Options.DisableLeftShiftTrashCan)
			{
				if (!Options.DisableQuickTrash)
				{
					if ((uint)context <= 4u || context == 7 || context == 32)
					{
						flag = true;
					}
					if (ControlInUse && flag)
					{
						SellOrTrash(inv, context, slot);
						result = true;
					}
				}
			}
			else
			{
				if ((uint)context <= 4u || context == 32)
				{
					flag = Main.player[Main.myPlayer].chest == -1;
				}
				if (ShiftInUse && flag && (!NotUsingGamepad || !Options.DisableQuickTrash))
				{
					SellOrTrash(inv, context, slot);
					result = true;
				}
			}
			return result;
		}

		private static void SellOrTrash(Item[] inv, int context, int slot)
		{
			Player player = Main.player[Main.myPlayer];
			if (inv[slot].type <= 0)
			{
				return;
			}
			if (Main.npcShop > 0 && !inv[slot].favorited)
			{
				Chest chest = Main.instance.shop[Main.npcShop];
				if (inv[slot].type < 71 || inv[slot].type > 74)
				{
					if (player.SellItem(inv[slot]))
					{
						chest.AddItemToShop(inv[slot]);
						AnnounceTransfer(new ItemTransferInfo(inv[slot], context, 15));
						inv[slot].TurnToAir();
						SoundEngine.PlaySound(18);
						Recipe.FindRecipes();
					}
					else if (inv[slot].value == 0)
					{
						chest.AddItemToShop(inv[slot]);
						AnnounceTransfer(new ItemTransferInfo(inv[slot], context, 15));
						inv[slot].TurnToAir();
						SoundEngine.PlaySound(7);
						Recipe.FindRecipes();
					}
				}
			}
			else if (!inv[slot].favorited)
			{
				SoundEngine.PlaySound(7);
				player.trashItem = inv[slot].Clone();
				AnnounceTransfer(new ItemTransferInfo(player.trashItem, context, 6));
				inv[slot].TurnToAir();
				if (context == 3 && Main.netMode == 1)
				{
					NetMessage.SendData(32, -1, -1, null, player.chest, slot);
				}
				Recipe.FindRecipes();
			}
		}

		private static string GetOverrideInstructions(Item[] inv, int context, int slot)
		{
			Player player = Main.player[Main.myPlayer];
			TileEntity tileEntity = player.tileEntityAnchor.GetTileEntity();
			if (tileEntity != null && tileEntity.TryGetItemGamepadOverrideInstructions(inv, context, slot, out var instruction))
			{
				return instruction;
			}
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (!inv[slot].favorited)
				{
					switch (context)
					{
					case 0:
					case 1:
					case 2:
						if (Main.npcShop > 0 && !inv[slot].favorited)
						{
							return Lang.misc[75].Value;
						}
						if (Main.player[Main.myPlayer].chest != -1)
						{
							if (ChestUI.TryPlacingInChest(inv[slot], justCheck: true, context))
							{
								return Lang.misc[76].Value;
							}
							break;
						}
						if (Main.InGuideCraftMenu && inv[slot].material)
						{
							return Lang.misc[76].Value;
						}
						return Lang.misc[74].Value;
					case 3:
					case 4:
					case 32:
						if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
						{
							return Lang.misc[76].Value;
						}
						break;
					case 5:
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 18:
					case 19:
					case 20:
					case 25:
					case 27:
					case 33:
						if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
						{
							return Lang.misc[68].Value;
						}
						break;
					}
				}
				bool flag = false;
				if ((uint)context <= 4u || context == 32)
				{
					flag = player.chest == -1;
				}
				if (flag)
				{
					if (Main.npcShop > 0 && !inv[slot].favorited)
					{
						_ = Main.instance.shop[Main.npcShop];
						if (inv[slot].type >= 71 && inv[slot].type <= 74)
						{
							return "";
						}
						return Lang.misc[75].Value;
					}
					if (!inv[slot].favorited)
					{
						return Lang.misc[74].Value;
					}
				}
			}
			return "";
		}

		public static int PickItemMovementAction(Item[] inv, int context, int slot, Item checkItem)
		{
			_ = Main.player[Main.myPlayer];
			int result = -1;
			switch (context)
			{
			case 0:
				result = 0;
				break;
			case 1:
				if (checkItem.type == 0 || checkItem.type == 71 || checkItem.type == 72 || checkItem.type == 73 || checkItem.type == 74)
				{
					result = 0;
				}
				break;
			case 2:
				if (checkItem.FitsAmmoSlot())
				{
					result = 0;
				}
				break;
			case 3:
				result = 0;
				break;
			case 4:
			case 32:
			{
				ChestUI.GetContainerUsageInfo(out var _, out var chestinv);
				if (!ChestUI.IsBlockedFromTransferIntoChest(checkItem, chestinv))
				{
					result = 0;
				}
				break;
			}
			case 5:
				if (checkItem.Prefix(-3) || checkItem.type == 0)
				{
					result = 0;
				}
				break;
			case 6:
				result = 0;
				break;
			case 7:
				if (checkItem.material || checkItem.type == 0)
				{
					result = 0;
				}
				break;
			case 8:
				if (checkItem.type == 0 || (checkItem.headSlot > -1 && slot == 0) || (checkItem.bodySlot > -1 && slot == 1) || (checkItem.legSlot > -1 && slot == 2))
				{
					result = 1;
				}
				break;
			case 23:
				if (checkItem.type == 0 || (checkItem.headSlot > 0 && slot == 0) || (checkItem.bodySlot > 0 && slot == 1) || (checkItem.legSlot > 0 && slot == 2))
				{
					result = 1;
				}
				break;
			case 26:
				if (checkItem.type == 0 || checkItem.headSlot > 0)
				{
					result = 1;
				}
				break;
			case 9:
				if (checkItem.type == 0 || (checkItem.headSlot > -1 && slot == 10) || (checkItem.bodySlot > -1 && slot == 11) || (checkItem.legSlot > -1 && slot == 12))
				{
					result = 1;
				}
				break;
			case 10:
				if (checkItem.type == 0 || (checkItem.accessory && !AccCheck(Main.LocalPlayer.armor, checkItem, slot)))
				{
					result = 1;
				}
				break;
			case 24:
				if (checkItem.type == 0 || (checkItem.accessory && !AccCheck(inv, checkItem, slot)))
				{
					result = 1;
				}
				break;
			case 11:
				if (checkItem.type == 0 || (checkItem.accessory && !AccCheck(Main.LocalPlayer.armor, checkItem, slot)))
				{
					result = 1;
				}
				break;
			case 12:
			case 25:
			case 27:
			case 33:
				result = 2;
				break;
			case 15:
				if (checkItem.type == 0 && inv[slot].type > 0)
				{
					result = 3;
				}
				else if (checkItem.type == inv[slot].type && checkItem.type > 0 && checkItem.stack < checkItem.maxStack && inv[slot].stack > 0)
				{
					result = 3;
				}
				else if (inv[slot].type == 0 && checkItem.type > 0 && (checkItem.type < 71 || checkItem.type > 74))
				{
					result = 4;
				}
				break;
			case 16:
				if (checkItem.type == 0 || Main.projHook[checkItem.shoot])
				{
					result = 1;
				}
				break;
			case 17:
				if (checkItem.type == 0 || (checkItem.mountType != -1 && !MountID.Sets.Cart[checkItem.mountType]))
				{
					result = 1;
				}
				break;
			case 19:
				if (checkItem.type == 0 || (checkItem.buffType > 0 && Main.vanityPet[checkItem.buffType] && !Main.lightPet[checkItem.buffType]))
				{
					result = 1;
				}
				break;
			case 18:
				if (checkItem.type == 0 || (checkItem.mountType != -1 && MountID.Sets.Cart[checkItem.mountType]))
				{
					result = 1;
				}
				break;
			case 20:
				if (checkItem.type == 0 || (checkItem.buffType > 0 && Main.lightPet[checkItem.buffType]))
				{
					result = 1;
				}
				break;
			case 29:
				if (checkItem.type == 0 && inv[slot].type > 0)
				{
					result = 5;
				}
				break;
			}
			if (context == 30)
			{
				result = 0;
			}
			return result;
		}

		public static void RightClick(ref Item inv, int context = 0)
		{
			singleSlotArray[0] = inv;
			RightClick(singleSlotArray, context);
			inv = singleSlotArray[0];
		}

		public static void RightClick(Item[] inv, int context = 0, int slot = 0)
		{
			Player player = Main.player[Main.myPlayer];
			inv[slot].newAndShiny = false;
			if (player.itemAnimation > 0)
			{
				return;
			}
			if (context == 15)
			{
				HandleShopSlot(inv, slot, rightClickIsValid: true, leftClickIsValid: false);
			}
			else
			{
				if (!Main.mouseRight)
				{
					return;
				}
				if (context == 0 && Main.mouseRightRelease)
				{
					TryItemSwap(inv[slot]);
				}
				if (context == 0 && ItemID.Sets.OpenableBag[inv[slot].type])
				{
					if (Main.mouseRightRelease)
					{
						TryOpenContainer(inv[slot], player);
					}
					return;
				}
				switch (context)
				{
				case 9:
				case 11:
					if (Main.mouseRightRelease)
					{
						SwapVanityEquip(inv, context, slot, player);
					}
					break;
				case 12:
				case 25:
				case 27:
				case 33:
					if (Main.mouseRightRelease)
					{
						TryPickupDyeToCursor(context, inv, slot, player);
					}
					break;
				case 0:
				case 3:
				case 4:
				case 32:
					if (inv[slot].maxStack == 1)
					{
						if (Main.mouseRightRelease)
						{
							SwapEquip(inv, context, slot);
						}
						break;
					}
					goto default;
				default:
					if (Main.stackSplit <= 1)
					{
						bool flag = true;
						bool flag2 = inv[slot].maxStack <= 1 && inv[slot].stack <= 1;
						if (context == 0 && flag2)
						{
							flag = false;
						}
						if (context == 3 && flag2)
						{
							flag = false;
						}
						if (context == 4 && flag2)
						{
							flag = false;
						}
						if (context == 32 && flag2)
						{
							flag = false;
						}
						if (flag && (Main.mouseItem.IsTheSameAs(inv[slot]) || Main.mouseItem.type == 0) && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
						{
							PickupItemIntoMouse(inv, context, slot, player);
							SoundEngine.PlaySound(12);
							RefreshStackSplitCooldown();
						}
					}
					break;
				}
			}
		}

		public static void PickupItemIntoMouse(Item[] inv, int context, int slot, Player player)
		{
			if (Main.mouseItem.type == 0)
			{
				Main.mouseItem = inv[slot].Clone();
				if (context == 29)
				{
					Main.mouseItem.SetDefaults(Main.mouseItem.type);
					Main.mouseItem.OnCreated(new JourneyDuplicationItemCreationContext());
				}
				Main.mouseItem.stack = 0;
				if (inv[slot].favorited && inv[slot].stack == 1)
				{
					Main.mouseItem.favorited = true;
				}
				else
				{
					Main.mouseItem.favorited = false;
				}
				AnnounceTransfer(new ItemTransferInfo(inv[slot], context, 21));
			}
			Main.mouseItem.stack++;
			if (context != 29)
			{
				inv[slot].stack--;
			}
			if (inv[slot].stack <= 0)
			{
				inv[slot] = new Item();
			}
			Recipe.FindRecipes();
			if (context == 3 && Main.netMode == 1)
			{
				NetMessage.SendData(32, -1, -1, null, player.chest, slot);
			}
			if ((context == 23 || context == 24) && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
			}
			if (context == 25 && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
			}
			if (context == 26 && Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
			}
			if (context == 27 && Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
			}
		}

		public static void RefreshStackSplitCooldown()
		{
			if (Main.stackSplit == 0)
			{
				Main.stackSplit = 30;
			}
			else
			{
				Main.stackSplit = Main.stackDelay;
			}
		}

		private static void TryOpenContainer(Item item, Player player)
		{
			if (ItemID.Sets.BossBag[item.type])
			{
				player.OpenBossBag(item.type);
			}
			else if (ItemID.Sets.IsFishingCrate[item.type])
			{
				player.OpenFishingCrate(item.type);
			}
			else if (item.type == 3093)
			{
				player.OpenHerbBag(3093);
			}
			else if (item.type == 4345)
			{
				player.OpenCanofWorms(item.type);
			}
			else if (item.type == 4410)
			{
				player.OpenOyster(item.type);
			}
			else if (item.type == 1774)
			{
				player.OpenGoodieBag(1774);
			}
			else if (item.type == 3085)
			{
				if (!player.ConsumeItem(327, reverseOrder: false, includeVoidBag: true))
				{
					return;
				}
				player.OpenLockBox(3085);
			}
			else if (item.type == 4879)
			{
				if (!player.HasItemInInventoryOrOpenVoidBag(329))
				{
					return;
				}
				player.OpenShadowLockbox(4879);
			}
			else if (item.type == 1869)
			{
				player.OpenPresent(1869);
			}
			else
			{
				if (item.type != 599 && item.type != 600 && item.type != 601)
				{
					return;
				}
				player.OpenLegacyPresent(item.type);
			}
			item.stack--;
			if (item.stack == 0)
			{
				item.SetDefaults();
			}
			SoundEngine.PlaySound(7);
			Main.stackSplit = 30;
			Main.mouseRightRelease = false;
			Recipe.FindRecipes();
		}

		private static void SwapVanityEquip(Item[] inv, int context, int slot, Player player)
		{
			if (Main.npcShop > 0 || ((inv[slot].type <= 0 || inv[slot].stack <= 0) && (inv[slot - 10].type <= 0 || inv[slot - 10].stack <= 0)))
			{
				return;
			}
			Item item = inv[slot - 10];
			bool flag = context != 11 || item.FitsAccessoryVanitySlot || item.IsAir;
			if (flag && context == 11 && inv[slot].wingSlot > 0)
			{
				for (int i = 3; i < 10; i++)
				{
					if (inv[i].wingSlot > 0 && i != slot - 10)
					{
						flag = false;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			Utils.Swap(ref inv[slot], ref inv[slot - 10]);
			SoundEngine.PlaySound(7);
			Recipe.FindRecipes();
			if (inv[slot].stack > 0)
			{
				switch (context)
				{
				case 0:
					AchievementsHelper.NotifyItemPickup(player, inv[slot]);
					break;
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 16:
				case 17:
				case 25:
				case 27:
				case 33:
					AchievementsHelper.HandleOnEquip(player, inv[slot], context);
					break;
				}
			}
		}

		private static void TryPickupDyeToCursor(int context, Item[] inv, int slot, Player player)
		{
			bool flag = false;
			if (!flag && ((Main.mouseItem.stack < Main.mouseItem.maxStack && Main.mouseItem.type > 0) || Main.mouseItem.IsAir) && inv[slot].type > 0 && (Main.mouseItem.type == inv[slot].type || Main.mouseItem.IsAir))
			{
				flag = true;
				if (Main.mouseItem.IsAir)
				{
					Main.mouseItem = inv[slot].Clone();
				}
				else
				{
					Main.mouseItem.stack++;
				}
				inv[slot].SetDefaults();
				SoundEngine.PlaySound(7);
			}
			if (flag)
			{
				if (context == 25 && Main.netMode == 1)
				{
					NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
				}
				if (context == 27 && Main.netMode == 1)
				{
					NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
				}
			}
		}

		private static void TryItemSwap(Item item)
		{
			int type = item.type;
			switch (type)
			{
			case 4131:
			case 5325:
				item.ChangeItemType((item.type == 5325) ? 4131 : 5325);
				AfterItemSwap(type, item.type);
				break;
			case 5059:
			case 5060:
				item.ChangeItemType((item.type == 5059) ? 5060 : 5059);
				AfterItemSwap(type, item.type);
				break;
			case 5324:
				item.ChangeItemType(5329);
				AfterItemSwap(type, item.type);
				break;
			case 5329:
				item.ChangeItemType(5330);
				AfterItemSwap(type, item.type);
				break;
			case 5330:
				item.ChangeItemType(5324);
				AfterItemSwap(type, item.type);
				break;
			case 4346:
			case 5391:
				item.ChangeItemType((item.type == 4346) ? 5391 : 4346);
				AfterItemSwap(type, item.type);
				break;
			case 5358:
				item.ChangeItemType(5360);
				AfterItemSwap(type, item.type);
				break;
			case 5360:
				item.ChangeItemType(5361);
				AfterItemSwap(type, item.type);
				break;
			case 5361:
				item.ChangeItemType(5359);
				AfterItemSwap(type, item.type);
				break;
			case 5359:
				item.ChangeItemType(5358);
				AfterItemSwap(type, item.type);
				break;
			case 5437:
				item.ChangeItemType(5358);
				AfterItemSwap(type, item.type);
				break;
			}
		}

		private static void AfterItemSwap(int oldType, int newType)
		{
			if (newType == 5324 || newType == 5329 || newType == 5330 || newType == 4346 || newType == 5391 || newType == 5358 || newType == 5361 || newType == 5360 || newType == 5359)
			{
				SoundEngine.PlaySound(22);
			}
			else
			{
				SoundEngine.PlaySound(7);
			}
			Main.stackSplit = 30;
			Main.mouseRightRelease = false;
			Recipe.FindRecipes();
		}

		private static void HandleShopSlot(Item[] inv, int slot, bool rightClickIsValid, bool leftClickIsValid)
		{
			if (Main.cursorOverride == 2)
			{
				return;
			}
			_ = Main.instance.shop[Main.npcShop];
			bool flag = (Main.mouseRight && rightClickIsValid) || (Main.mouseLeft && leftClickIsValid);
			if (!(Main.stackSplit <= 1 && flag) || inv[slot].type <= 0 || (!Main.mouseItem.IsTheSameAs(inv[slot]) && Main.mouseItem.type != 0))
			{
				return;
			}
			int num = Main.superFastStack + 1;
			Player localPlayer = Main.LocalPlayer;
			for (int i = 0; i < num; i++)
			{
				if (Main.mouseItem.stack >= Main.mouseItem.maxStack && Main.mouseItem.type != 0)
				{
					continue;
				}
				localPlayer.GetItemExpectedPrice(inv[slot], out var _, out var calcForBuying);
				if (!localPlayer.BuyItem(calcForBuying, inv[slot].shopSpecialCurrency) || inv[slot].stack <= 0)
				{
					continue;
				}
				if (i == 0)
				{
					if (inv[slot].value > 0)
					{
						SoundEngine.PlaySound(18);
					}
					else
					{
						SoundEngine.PlaySound(7);
					}
				}
				if (Main.mouseItem.type == 0)
				{
					Main.mouseItem.netDefaults(inv[slot].netID);
					if (inv[slot].prefix != 0)
					{
						Main.mouseItem.Prefix(inv[slot].prefix);
					}
					Main.mouseItem.stack = 0;
				}
				if (!inv[slot].buyOnce)
				{
					Main.shopSellbackHelper.Add(inv[slot]);
				}
				Main.mouseItem.stack++;
				RefreshStackSplitCooldown();
				if (inv[slot].buyOnce && --inv[slot].stack <= 0)
				{
					inv[slot].SetDefaults();
				}
				AnnounceTransfer(new ItemTransferInfo(Main.mouseItem, 15, 21));
			}
		}

		public static void Draw(SpriteBatch spriteBatch, ref Item inv, int context, Vector2 position, Color lightColor = default(Color))
		{
			singleSlotArray[0] = inv;
			Draw(spriteBatch, singleSlotArray, context, 0, position, lightColor);
			inv = singleSlotArray[0];
		}

		public static void Draw(SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor = default(Color))
		{
			Player player = Main.player[Main.myPlayer];
			Item item = inv[slot];
			float inventoryScale = Main.inventoryScale;
			Color color = Color.White;
			if (lightColor != Color.Transparent)
			{
				color = lightColor;
			}
			bool flag = false;
			int num = 0;
			int gamepadPointForSlot = GetGamepadPointForSlot(inv, context, slot);
			if (PlayerInput.UsingGamepadUI)
			{
				flag = UILinkPointNavigator.CurrentPoint == gamepadPointForSlot;
				if (PlayerInput.SettingsForUI.PreventHighlightsForGamepad)
				{
					flag = false;
				}
				if (context == 0)
				{
					num = player.DpadRadial.GetDrawMode(slot);
					if (num > 0 && !PlayerInput.CurrentProfile.UsingDpadHotbar())
					{
						num = 0;
					}
				}
			}
			Texture2D value = TextureAssets.InventoryBack.get_Value();
			Color color2 = Main.inventoryBack;
			bool flag2 = false;
			bool highlightThingsForMouse = PlayerInput.SettingsForUI.HighlightThingsForMouse;
			if (item.type > 0 && item.stack > 0 && item.favorited && context != 13 && context != 21 && context != 22 && context != 14)
			{
				value = TextureAssets.InventoryBack10.get_Value();
				if (context == 32)
				{
					value = TextureAssets.InventoryBack19.get_Value();
				}
				if (context == 0 && slot < 10 && player.selectedItem == slot)
				{
					color2 = Color.White;
					value = TextureAssets.InventoryBack17.get_Value();
				}
			}
			else if (item.type > 0 && item.stack > 0 && Options.HighlightNewItems && item.newAndShiny && context != 13 && context != 21 && context != 14 && context != 22)
			{
				value = TextureAssets.InventoryBack15.get_Value();
				float num2 = (float)(int)Main.mouseTextColor / 255f;
				num2 = num2 * 0.2f + 0.8f;
				color2 = color2.MultiplyRGBA(new Color(num2, num2, num2));
			}
			else if (!highlightThingsForMouse && item.type > 0 && item.stack > 0 && num != 0 && context != 13 && context != 21 && context != 22)
			{
				value = TextureAssets.InventoryBack15.get_Value();
				float num3 = (float)(int)Main.mouseTextColor / 255f;
				num3 = num3 * 0.2f + 0.8f;
				color2 = ((num != 1) ? color2.MultiplyRGBA(new Color(num3 / 2f, num3, num3 / 2f)) : color2.MultiplyRGBA(new Color(num3, num3 / 2f, num3 / 2f)));
			}
			else if (context == 0 && slot < 10)
			{
				value = TextureAssets.InventoryBack9.get_Value();
				if (player.selectedItem == slot && highlightThingsForMouse)
				{
					value = TextureAssets.InventoryBack14.get_Value();
					color2 = Color.White;
				}
			}
			else
			{
				switch (context)
				{
				case 28:
					value = TextureAssets.InventoryBack7.get_Value();
					color2 = Color.White;
					break;
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
					value = TextureAssets.InventoryBack3.get_Value();
					break;
				case 8:
				case 10:
					value = TextureAssets.InventoryBack13.get_Value();
					color2 = GetColorByLoadout(slot, context);
					break;
				case 23:
				case 24:
				case 26:
					value = TextureAssets.InventoryBack8.get_Value();
					break;
				case 9:
				case 11:
					value = TextureAssets.InventoryBack13.get_Value();
					color2 = GetColorByLoadout(slot, context);
					break;
				case 25:
				case 27:
				case 33:
					value = TextureAssets.InventoryBack12.get_Value();
					break;
				case 12:
					value = TextureAssets.InventoryBack13.get_Value();
					color2 = GetColorByLoadout(slot, context);
					break;
				case 3:
					value = TextureAssets.InventoryBack5.get_Value();
					break;
				case 4:
				case 32:
					value = TextureAssets.InventoryBack2.get_Value();
					break;
				case 5:
				case 7:
					value = TextureAssets.InventoryBack4.get_Value();
					break;
				case 6:
					value = TextureAssets.InventoryBack7.get_Value();
					break;
				case 13:
				{
					byte b = 200;
					if (slot == Main.player[Main.myPlayer].selectedItem)
					{
						value = TextureAssets.InventoryBack14.get_Value();
						b = byte.MaxValue;
					}
					color2 = new Color(b, b, b, b);
					break;
				}
				case 14:
				case 21:
					flag2 = true;
					break;
				case 15:
					value = TextureAssets.InventoryBack6.get_Value();
					break;
				case 29:
					color2 = new Color(53, 69, 127, 255);
					value = TextureAssets.InventoryBack18.get_Value();
					break;
				case 30:
					flag2 = !flag;
					break;
				case 22:
					value = TextureAssets.InventoryBack4.get_Value();
					if (DrawGoldBGForCraftingMaterial)
					{
						DrawGoldBGForCraftingMaterial = false;
						value = TextureAssets.InventoryBack14.get_Value();
						float num4 = (float)(int)color2.A / 255f;
						num4 = ((!(num4 < 0.7f)) ? 1f : Utils.GetLerpValue(0f, 0.7f, num4, clamped: true));
						color2 = Color.White * num4;
					}
					break;
				}
			}
			if ((context == 0 || context == 2) && inventoryGlowTime[slot] > 0 && !inv[slot].favorited && !inv[slot].IsAir)
			{
				float num5 = Main.invAlpha / 255f;
				Color value2 = new Color(63, 65, 151, 255) * num5;
				Color value3 = Main.hslToRgb(inventoryGlowHue[slot], 1f, 0.5f) * num5;
				float num6 = (float)inventoryGlowTime[slot] / 300f;
				num6 *= num6;
				color2 = Color.Lerp(value2, value3, num6 / 2f);
				value = TextureAssets.InventoryBack13.get_Value();
			}
			if ((context == 4 || context == 32 || context == 3) && inventoryGlowTimeChest[slot] > 0 && !inv[slot].favorited && !inv[slot].IsAir)
			{
				float num7 = Main.invAlpha / 255f;
				Color value4 = new Color(130, 62, 102, 255) * num7;
				if (context == 3)
				{
					value4 = new Color(104, 52, 52, 255) * num7;
				}
				Color value5 = Main.hslToRgb(inventoryGlowHueChest[slot], 1f, 0.5f) * num7;
				float num8 = (float)inventoryGlowTimeChest[slot] / 300f;
				num8 *= num8;
				color2 = Color.Lerp(value4, value5, num8 / 2f);
				value = TextureAssets.InventoryBack13.get_Value();
			}
			if (flag)
			{
				value = TextureAssets.InventoryBack14.get_Value();
				color2 = Color.White;
			}
			if (context == 28 && Main.MouseScreen.Between(position, position + value.Size() * inventoryScale) && !player.mouseInterface)
			{
				value = TextureAssets.InventoryBack14.get_Value();
				color2 = Color.White;
			}
			if (!flag2)
			{
				spriteBatch.Draw(value, position, null, color2, 0f, default(Vector2), inventoryScale, SpriteEffects.None, 0f);
			}
			int num9 = -1;
			switch (context)
			{
			case 8:
			case 23:
				if (slot == 0)
				{
					num9 = 0;
				}
				if (slot == 1)
				{
					num9 = 6;
				}
				if (slot == 2)
				{
					num9 = 12;
				}
				break;
			case 26:
				num9 = 0;
				break;
			case 9:
				if (slot == 10)
				{
					num9 = 3;
				}
				if (slot == 11)
				{
					num9 = 9;
				}
				if (slot == 12)
				{
					num9 = 15;
				}
				break;
			case 10:
			case 24:
				num9 = 11;
				break;
			case 11:
				num9 = 2;
				break;
			case 12:
			case 25:
			case 27:
			case 33:
				num9 = 1;
				break;
			case 16:
				num9 = 4;
				break;
			case 17:
				num9 = 13;
				break;
			case 19:
				num9 = 10;
				break;
			case 18:
				num9 = 7;
				break;
			case 20:
				num9 = 17;
				break;
			}
			if ((item.type <= 0 || item.stack <= 0) && num9 != -1)
			{
				Texture2D value6 = TextureAssets.Extra[54].get_Value();
				Rectangle rectangle = value6.Frame(3, 6, num9 % 3, num9 / 3);
				rectangle.Width -= 2;
				rectangle.Height -= 2;
				spriteBatch.Draw(value6, position + value.Size() / 2f * inventoryScale, rectangle, Color.White * 0.35f, 0f, rectangle.Size() / 2f, inventoryScale, SpriteEffects.None, 0f);
			}
			Vector2 vector = value.Size() * inventoryScale;
			if (item.type > 0 && item.stack > 0)
			{
				float scale = DrawItemIcon(item, context, spriteBatch, position + vector / 2f, inventoryScale, 32f, color);
				if (ItemID.Sets.TrapSigned[item.type])
				{
					spriteBatch.Draw(TextureAssets.Wire.get_Value(), position + new Vector2(40f, 40f) * inventoryScale, new Rectangle(4, 58, 8, 8), color, 0f, new Vector2(4f), 1f, SpriteEffects.None, 0f);
				}
				if (ItemID.Sets.DrawUnsafeIndicator[item.type])
				{
					Vector2 vector2 = new Vector2(-4f, -4f) * inventoryScale;
					Texture2D value7 = TextureAssets.Extra[258].get_Value();
					Rectangle rectangle2 = value7.Frame();
					spriteBatch.Draw(value7, position + vector2 + new Vector2(40f, 40f) * inventoryScale, rectangle2, color, 0f, rectangle2.Size() / 2f, 1f, SpriteEffects.None, 0f);
				}
				if (item.type == 5324 || item.type == 5329 || item.type == 5330)
				{
					Vector2 vector3 = new Vector2(2f, -6f) * inventoryScale;
					switch (item.type)
					{
					case 5324:
					{
						Texture2D value10 = TextureAssets.Extra[257].get_Value();
						Rectangle rectangle5 = value10.Frame(3, 1, 2);
						spriteBatch.Draw(value10, position + vector3 + new Vector2(40f, 40f) * inventoryScale, rectangle5, color, 0f, rectangle5.Size() / 2f, 1f, SpriteEffects.None, 0f);
						break;
					}
					case 5329:
					{
						Texture2D value9 = TextureAssets.Extra[257].get_Value();
						Rectangle rectangle4 = value9.Frame(3, 1, 1);
						spriteBatch.Draw(value9, position + vector3 + new Vector2(40f, 40f) * inventoryScale, rectangle4, color, 0f, rectangle4.Size() / 2f, 1f, SpriteEffects.None, 0f);
						break;
					}
					case 5330:
					{
						Texture2D value8 = TextureAssets.Extra[257].get_Value();
						Rectangle rectangle3 = value8.Frame(3);
						spriteBatch.Draw(value8, position + vector3 + new Vector2(40f, 40f) * inventoryScale, rectangle3, color, 0f, rectangle3.Size() / 2f, 1f, SpriteEffects.None, 0f);
						break;
					}
					}
				}
				if (item.stack > 1)
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), item.stack.ToString(), position + new Vector2(10f, 26f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
				}
				int num10 = -1;
				if (context == 13)
				{
					if (item.DD2Summon)
					{
						for (int i = 0; i < 58; i++)
						{
							if (inv[i].type == 3822)
							{
								num10 += inv[i].stack;
							}
						}
						if (num10 >= 0)
						{
							num10++;
						}
					}
					if (item.useAmmo > 0)
					{
						int useAmmo = item.useAmmo;
						num10 = 0;
						for (int j = 0; j < 58; j++)
						{
							if (inv[j].ammo == useAmmo)
							{
								num10 += inv[j].stack;
							}
						}
					}
					if (item.fishingPole > 0)
					{
						num10 = 0;
						for (int k = 0; k < 58; k++)
						{
							if (inv[k].bait > 0)
							{
								num10 += inv[k].stack;
							}
						}
					}
					if (item.tileWand > 0)
					{
						int tileWand = item.tileWand;
						num10 = 0;
						for (int l = 0; l < 58; l++)
						{
							if (inv[l].type == tileWand)
							{
								num10 += inv[l].stack;
							}
						}
					}
					if (item.type == 509 || item.type == 851 || item.type == 850 || item.type == 3612 || item.type == 3625 || item.type == 3611)
					{
						num10 = 0;
						for (int m = 0; m < 58; m++)
						{
							if (inv[m].type == 530)
							{
								num10 += inv[m].stack;
							}
						}
					}
				}
				if (num10 != -1)
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), num10.ToString(), position + new Vector2(8f, 30f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale * 0.8f), -1f, inventoryScale);
				}
				if (context == 13)
				{
					string text = string.Concat(slot + 1);
					if (text == "10")
					{
						text = "0";
					}
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), text, position + new Vector2(8f, 4f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
				}
				if (context == 13 && item.potion)
				{
					Vector2 position2 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.get_Value().Size() * inventoryScale / 2f;
					Color color3 = item.GetAlpha(color) * ((float)player.potionDelay / (float)player.potionDelayTime);
					spriteBatch.Draw(TextureAssets.Cd.get_Value(), position2, null, color3, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
				}
				if ((context == 10 || context == 18) && item.expertOnly && !Main.expertMode)
				{
					Vector2 position3 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.get_Value().Size() * inventoryScale / 2f;
					Color white = Color.White;
					spriteBatch.Draw(TextureAssets.Cd.get_Value(), position3, null, white, 0f, default(Vector2), scale, SpriteEffects.None, 0f);
				}
			}
			else if (context == 6)
			{
				Texture2D value11 = TextureAssets.Trash.get_Value();
				Vector2 position4 = position + value.Size() * inventoryScale / 2f - value11.Size() * inventoryScale / 2f;
				spriteBatch.Draw(value11, position4, null, new Color(100, 100, 100, 100), 0f, default(Vector2), inventoryScale, SpriteEffects.None, 0f);
			}
			if (context == 0 && slot < 10)
			{
				float num11 = inventoryScale;
				string text2 = string.Concat(slot + 1);
				if (text2 == "10")
				{
					text2 = "0";
				}
				Color baseColor = Main.inventoryBack;
				int num12 = 0;
				if (Main.player[Main.myPlayer].selectedItem == slot)
				{
					baseColor = Color.White;
					baseColor.A = 200;
					num12 -= 2;
					num11 *= 1.4f;
				}
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.get_Value(), text2, position + new Vector2(6f, 4 + num12) * inventoryScale, baseColor, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
			}
			if (gamepadPointForSlot != -1)
			{
				UILinkPointNavigator.SetPosition(gamepadPointForSlot, position + vector * 0.75f);
			}
		}

		public static Color GetColorByLoadout(int slot, int context)
		{
			Color color = Color.White;
			if (TryGetSlotColor(Main.LocalPlayer.CurrentLoadoutIndex, context, out var color2))
			{
				color = color2;
			}
			Color value = new Color(color.ToVector4() * Main.inventoryBack.ToVector4());
			float num = Utils.Remap((float)(Main.timeForVisualEffects - _lastTimeForVisualEffectsThatLoadoutWasChanged), 0f, 30f, 0.5f, 0f);
			return Color.Lerp(value, Color.White, num * num * num);
		}

		public static void RecordLoadoutChange()
		{
			_lastTimeForVisualEffectsThatLoadoutWasChanged = Main.timeForVisualEffects;
		}

		public static bool TryGetSlotColor(int loadoutIndex, int context, out Color color)
		{
			color = default(Color);
			if (loadoutIndex < 0 || loadoutIndex >= 3)
			{
				return false;
			}
			int num = -1;
			switch (context)
			{
			case 8:
			case 10:
				num = 0;
				break;
			case 9:
			case 11:
				num = 1;
				break;
			case 12:
				num = 2;
				break;
			}
			if (num == -1)
			{
				return false;
			}
			color = LoadoutSlotColors[loadoutIndex, num];
			return true;
		}

		public static float ShiftHueByLoadout(float hue, int loadoutIndex)
		{
			return (hue + (float)loadoutIndex / 8f) % 1f;
		}

		public static Color GetLoadoutColor(int loadoutIndex)
		{
			return Main.hslToRgb(ShiftHueByLoadout(0.41f, loadoutIndex), 0.7f, 0.5f);
		}

		public static float DrawItemIcon(Item item, int context, SpriteBatch spriteBatch, Vector2 screenPositionForItemCenter, float scale, float sizeLimit, Color environmentColor)
		{
			int num = item.type;
			if ((uint)(num - 5358) <= 3u && context == 31)
			{
				num = 5437;
			}
			Main.instance.LoadItem(num);
			Texture2D value = TextureAssets.Item[num].get_Value();
			Rectangle frame = ((Main.itemAnimations[num] == null) ? value.Frame() : Main.itemAnimations[num].GetFrame(value));
			DrawItem_GetColorAndScale(item, scale, ref environmentColor, sizeLimit, ref frame, out var itemLight, out var finalDrawScale);
			SpriteEffects effects = SpriteEffects.None;
			Vector2 origin = frame.Size() / 2f;
			spriteBatch.Draw(value, screenPositionForItemCenter, frame, item.GetAlpha(itemLight), 0f, origin, finalDrawScale, effects, 0f);
			if (item.color != Color.Transparent)
			{
				Color newColor = environmentColor;
				if (context == 13)
				{
					newColor.A = byte.MaxValue;
				}
				spriteBatch.Draw(value, screenPositionForItemCenter, frame, item.GetColor(newColor), 0f, origin, finalDrawScale, effects, 0f);
			}
			switch (num)
			{
			case 5140:
			case 5141:
			case 5142:
			case 5143:
			case 5144:
			case 5145:
			{
				Texture2D value3 = TextureAssets.GlowMask[item.glowMask].get_Value();
				Color white = Color.White;
				spriteBatch.Draw(value3, screenPositionForItemCenter, frame, white, 0f, origin, finalDrawScale, effects, 0f);
				break;
			}
			case 5146:
			{
				Texture2D value2 = TextureAssets.GlowMask[324].get_Value();
				spriteBatch.Draw(color: new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB), texture: value2, position: screenPositionForItemCenter, sourceRectangle: frame, rotation: 0f, origin: origin, scale: finalDrawScale, effects: effects, layerDepth: 0f);
				break;
			}
			}
			return finalDrawScale;
		}

		public static void DrawItem_GetColorAndScale(Item item, float scale, ref Color currentWhite, float sizeLimit, ref Rectangle frame, out Color itemLight, out float finalDrawScale)
		{
			itemLight = currentWhite;
			float scale2 = 1f;
			GetItemLight(ref itemLight, ref scale2, item);
			float num = 1f;
			if ((float)frame.Width > sizeLimit || (float)frame.Height > sizeLimit)
			{
				num = ((frame.Width <= frame.Height) ? (sizeLimit / (float)frame.Height) : (sizeLimit / (float)frame.Width));
			}
			finalDrawScale = scale * num * scale2;
		}

		private static int GetGamepadPointForSlot(Item[] inv, int context, int slot)
		{
			Player localPlayer = Main.LocalPlayer;
			int result = -1;
			switch (context)
			{
			case 0:
			case 1:
			case 2:
				result = slot;
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			{
				int num2 = slot;
				if (num2 % 10 == 9 && !localPlayer.CanDemonHeartAccessoryBeShown())
				{
					num2--;
				}
				result = 100 + num2;
				break;
			}
			case 12:
				if (inv == localPlayer.dye)
				{
					int num = slot;
					if (num % 10 == 9 && !localPlayer.CanDemonHeartAccessoryBeShown())
					{
						num--;
					}
					result = 120 + num;
				}
				break;
			case 33:
				if (inv == localPlayer.miscDyes)
				{
					result = 185 + slot;
				}
				break;
			case 19:
				result = 180;
				break;
			case 20:
				result = 181;
				break;
			case 18:
				result = 182;
				break;
			case 17:
				result = 183;
				break;
			case 16:
				result = 184;
				break;
			case 3:
			case 4:
			case 32:
				result = 400 + slot;
				break;
			case 15:
				result = 2700 + slot;
				break;
			case 6:
				result = 300;
				break;
			case 22:
				if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig != -1)
				{
					result = 700 + UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig;
				}
				if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall != -1)
				{
					result = 1500 + UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall + 1;
				}
				break;
			case 7:
				result = 1500;
				break;
			case 5:
				result = 303;
				break;
			case 23:
				result = 5100 + slot;
				break;
			case 24:
				result = 5100 + slot;
				break;
			case 25:
				result = 5108 + slot;
				break;
			case 26:
				result = 5000 + slot;
				break;
			case 27:
				result = 5002 + slot;
				break;
			case 29:
				result = 3000 + slot;
				if (UILinkPointNavigator.Shortcuts.CREATIVE_ItemSlotShouldHighlightAsSelected)
				{
					result = UILinkPointNavigator.CurrentPoint;
				}
				break;
			case 30:
				result = 15000 + slot;
				break;
			}
			return result;
		}

		public static void MouseHover(int context = 0)
		{
			singleSlotArray[0] = Main.HoverItem;
			MouseHover(singleSlotArray, context);
		}

		public static void MouseHover(ref Item inv, int context = 0)
		{
			singleSlotArray[0] = inv;
			MouseHover(singleSlotArray, context);
			inv = singleSlotArray[0];
		}

		public static void MouseHover(Item[] inv, int context = 0, int slot = 0)
		{
			if (context == 6 && Main.hoverItemName == null)
			{
				Main.hoverItemName = Lang.inter[3].Value;
			}
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				_customCurrencyForSavings = inv[slot].shopSpecialCurrency;
				Main.hoverItemName = inv[slot].Name;
				if (inv[slot].stack > 1)
				{
					Main.hoverItemName = Main.hoverItemName + " (" + inv[slot].stack + ")";
				}
				Main.HoverItem = inv[slot].Clone();
				Main.HoverItem.tooltipContext = context;
				if (context == 8 && slot <= 2)
				{
					Main.HoverItem.wornArmor = true;
					return;
				}
				switch (context)
				{
				case 9:
				case 11:
					Main.HoverItem.social = true;
					break;
				case 15:
					Main.HoverItem.buy = true;
					break;
				}
				return;
			}
			if (context == 10 || context == 11 || context == 24)
			{
				Main.hoverItemName = Lang.inter[9].Value;
			}
			if (context == 11)
			{
				Main.hoverItemName = Lang.inter[11].Value + " " + Main.hoverItemName;
			}
			if (context == 8 || context == 9 || context == 23 || context == 26)
			{
				if (slot == 0 || slot == 10 || context == 26)
				{
					Main.hoverItemName = Lang.inter[12].Value;
				}
				else if (slot == 1 || slot == 11)
				{
					Main.hoverItemName = Lang.inter[13].Value;
				}
				else if (slot == 2 || slot == 12)
				{
					Main.hoverItemName = Lang.inter[14].Value;
				}
				else if (slot >= 10)
				{
					Main.hoverItemName = Lang.inter[11].Value + " " + Main.hoverItemName;
				}
			}
			if (context == 12 || context == 25 || context == 27 || context == 33)
			{
				Main.hoverItemName = Lang.inter[57].Value;
			}
			if (context == 16)
			{
				Main.hoverItemName = Lang.inter[90].Value;
			}
			if (context == 17)
			{
				Main.hoverItemName = Lang.inter[91].Value;
			}
			if (context == 19)
			{
				Main.hoverItemName = Lang.inter[92].Value;
			}
			if (context == 18)
			{
				Main.hoverItemName = Lang.inter[93].Value;
			}
			if (context == 20)
			{
				Main.hoverItemName = Lang.inter[94].Value;
			}
		}

		public static void SwapEquip(ref Item inv, int context = 0)
		{
			singleSlotArray[0] = inv;
			SwapEquip(singleSlotArray, context, 0);
			inv = singleSlotArray[0];
		}

		public static void SwapEquip(Item[] inv, int context, int slot)
		{
			Player player = Main.player[Main.myPlayer];
			if (isEquipLocked(inv[slot].type))
			{
				return;
			}
			bool success;
			if (inv[slot].dye > 0)
			{
				inv[slot] = DyeSwap(inv[slot], out success);
				if (success)
				{
					Main.EquipPageSelected = 0;
					AchievementsHelper.HandleOnEquip(player, inv[slot], 12);
				}
			}
			else if (Main.projHook[inv[slot].shoot])
			{
				inv[slot] = EquipSwap(inv[slot], player.miscEquips, 4, out success);
				if (success)
				{
					Main.EquipPageSelected = 2;
					AchievementsHelper.HandleOnEquip(player, inv[slot], 16);
				}
			}
			else if (inv[slot].mountType != -1 && !MountID.Sets.Cart[inv[slot].mountType])
			{
				inv[slot] = EquipSwap(inv[slot], player.miscEquips, 3, out success);
				if (success)
				{
					Main.EquipPageSelected = 2;
					AchievementsHelper.HandleOnEquip(player, inv[slot], 17);
				}
			}
			else if (inv[slot].mountType != -1 && MountID.Sets.Cart[inv[slot].mountType])
			{
				inv[slot] = EquipSwap(inv[slot], player.miscEquips, 2, out success);
				if (success)
				{
					Main.EquipPageSelected = 2;
				}
			}
			else if (inv[slot].buffType > 0 && Main.lightPet[inv[slot].buffType])
			{
				inv[slot] = EquipSwap(inv[slot], player.miscEquips, 1, out success);
				if (success)
				{
					Main.EquipPageSelected = 2;
				}
			}
			else if (inv[slot].buffType > 0 && Main.vanityPet[inv[slot].buffType])
			{
				inv[slot] = EquipSwap(inv[slot], player.miscEquips, 0, out success);
				if (success)
				{
					Main.EquipPageSelected = 2;
				}
			}
			else
			{
				Item item = inv[slot];
				inv[slot] = ArmorSwap(inv[slot], out success);
				if (success)
				{
					Main.EquipPageSelected = 0;
					AchievementsHelper.HandleOnEquip(player, item, item.accessory ? 10 : 8);
				}
			}
			Recipe.FindRecipes();
			if (context == 3 && Main.netMode == 1)
			{
				NetMessage.SendData(32, -1, -1, null, player.chest, slot);
			}
		}

		public static bool Equippable(ref Item inv, int context = 0)
		{
			singleSlotArray[0] = inv;
			bool result = Equippable(singleSlotArray, context, 0);
			inv = singleSlotArray[0];
			return result;
		}

		public static bool Equippable(Item[] inv, int context, int slot)
		{
			_ = Main.player[Main.myPlayer];
			if (inv[slot].dye > 0 || Main.projHook[inv[slot].shoot] || inv[slot].mountType != -1 || (inv[slot].buffType > 0 && Main.lightPet[inv[slot].buffType]) || (inv[slot].buffType > 0 && Main.vanityPet[inv[slot].buffType]) || inv[slot].headSlot >= 0 || inv[slot].bodySlot >= 0 || inv[slot].legSlot >= 0 || inv[slot].accessory)
			{
				return true;
			}
			return false;
		}

		public static bool IsMiscEquipment(Item item)
		{
			if (item.mountType == -1 && (item.buffType <= 0 || !Main.lightPet[item.buffType]) && (item.buffType <= 0 || !Main.vanityPet[item.buffType]))
			{
				return Main.projHook[item.shoot];
			}
			return true;
		}

		private static bool AccCheck(Item[] itemCollection, Item item, int slot)
		{
			if (isEquipLocked(item.type))
			{
				return true;
			}
			if (slot != -1)
			{
				if (itemCollection[slot].IsTheSameAs(item))
				{
					return false;
				}
				if (itemCollection[slot].wingSlot > 0 && item.wingSlot > 0)
				{
					return false;
				}
			}
			for (int i = 0; i < itemCollection.Length; i++)
			{
				if (slot < 10 && i < 10)
				{
					if (item.wingSlot > 0 && itemCollection[i].wingSlot > 0)
					{
						return true;
					}
					if (slot >= 10 && i >= 10 && item.wingSlot > 0 && itemCollection[i].wingSlot > 0)
					{
						return true;
					}
				}
				if (item.IsTheSameAs(itemCollection[i]))
				{
					return true;
				}
			}
			return false;
		}

		private static Item DyeSwap(Item item, out bool success)
		{
			success = false;
			if (item.dye <= 0)
			{
				return item;
			}
			Player player = Main.player[Main.myPlayer];
			Item item2 = item;
			for (int i = 0; i < 10; i++)
			{
				if (player.dye[i].type == 0)
				{
					dyeSlotCount = i;
					break;
				}
			}
			if (dyeSlotCount >= 10)
			{
				dyeSlotCount = 0;
			}
			if (dyeSlotCount < 0)
			{
				dyeSlotCount = 9;
			}
			item2 = player.dye[dyeSlotCount].Clone();
			player.dye[dyeSlotCount] = item.Clone();
			dyeSlotCount++;
			if (dyeSlotCount >= 10)
			{
				accSlotToSwapTo = 0;
			}
			SoundEngine.PlaySound(7);
			Recipe.FindRecipes();
			success = true;
			return item2;
		}

		private static Item ArmorSwap(Item item, out bool success)
		{
			success = false;
			if (item.stack < 1)
			{
				return item;
			}
			if (item.headSlot == -1 && item.bodySlot == -1 && item.legSlot == -1 && !item.accessory)
			{
				return item;
			}
			Player player = Main.player[Main.myPlayer];
			int num = ((item.vanity && !item.accessory) ? 10 : 0);
			item.favorited = false;
			Item result = item;
			if (item.headSlot != -1)
			{
				result = player.armor[num].Clone();
				player.armor[num] = item.Clone();
			}
			else if (item.bodySlot != -1)
			{
				result = player.armor[num + 1].Clone();
				player.armor[num + 1] = item.Clone();
			}
			else if (item.legSlot != -1)
			{
				result = player.armor[num + 2].Clone();
				player.armor[num + 2] = item.Clone();
			}
			else if (item.accessory)
			{
				int num2 = 3;
				for (int i = 3; i < 10; i++)
				{
					if (player.IsItemSlotUnlockedAndUsable(i))
					{
						num2 = i;
						if (player.armor[i].type == 0)
						{
							accSlotToSwapTo = i - 3;
							break;
						}
					}
				}
				for (int j = 0; j < player.armor.Length; j++)
				{
					if (item.IsTheSameAs(player.armor[j]))
					{
						accSlotToSwapTo = j - 3;
					}
					if (j < 10 && item.wingSlot > 0 && player.armor[j].wingSlot > 0)
					{
						accSlotToSwapTo = j - 3;
					}
				}
				if (accSlotToSwapTo > num2)
				{
					return item;
				}
				if (accSlotToSwapTo < 0)
				{
					accSlotToSwapTo = num2 - 3;
				}
				int num3 = 3 + accSlotToSwapTo;
				if (isEquipLocked(player.armor[num3].type))
				{
					return item;
				}
				for (int k = 0; k < player.armor.Length; k++)
				{
					if (item.IsTheSameAs(player.armor[k]))
					{
						num3 = k;
					}
				}
				result = player.armor[num3].Clone();
				player.armor[num3] = item.Clone();
				accSlotToSwapTo = 0;
			}
			SoundEngine.PlaySound(7);
			Recipe.FindRecipes();
			success = true;
			return result;
		}

		private static Item EquipSwap(Item item, Item[] inv, int slot, out bool success)
		{
			success = false;
			_ = Main.player[Main.myPlayer];
			item.favorited = false;
			Item result = inv[slot].Clone();
			inv[slot] = item.Clone();
			SoundEngine.PlaySound(7);
			Recipe.FindRecipes();
			success = true;
			return result;
		}

		public static void DrawMoney(SpriteBatch sb, string text, float shopx, float shopy, int[] coinsArray, bool horizontal = false)
		{
			Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.get_Value(), text, shopx, shopy + 40f, Color.White * ((float)(int)Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
			if (horizontal)
			{
				for (int i = 0; i < 4; i++)
				{
					Main.instance.LoadItem(74 - i);
					if (i == 0)
					{
						_ = coinsArray[3 - i];
						_ = 99;
					}
					Vector2 position = new Vector2(shopx + ChatManager.GetStringSize(FontAssets.MouseText.get_Value(), text, Vector2.One).X + (float)(24 * i) + 45f, shopy + 50f);
					sb.Draw(TextureAssets.Item[74 - i].get_Value(), position, null, Color.White, 0f, TextureAssets.Item[74 - i].get_Value().Size() / 2f, 1f, SpriteEffects.None, 0f);
					Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.get_Value(), coinsArray[3 - i].ToString(), position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					Main.instance.LoadItem(74 - j);
					int num = ((j == 0 && coinsArray[3 - j] > 99) ? (-6) : 0);
					sb.Draw(TextureAssets.Item[74 - j].get_Value(), new Vector2(shopx + 11f + (float)(24 * j), shopy + 75f), null, Color.White, 0f, TextureAssets.Item[74 - j].get_Value().Size() / 2f, 1f, SpriteEffects.None, 0f);
					Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.get_Value(), coinsArray[3 - j].ToString(), shopx + (float)(24 * j) + (float)num, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
				}
			}
		}

		public static void DrawSavings(SpriteBatch sb, float shopx, float shopy, bool horizontal = false)
		{
			Player player = Main.player[Main.myPlayer];
			if (_customCurrencyForSavings != -1)
			{
				CustomCurrencyManager.DrawSavings(sb, _customCurrencyForSavings, shopx, shopy, horizontal);
				return;
			}
			bool overFlowing;
			long num = Utils.CoinsCount(out overFlowing, player.bank.item);
			long num2 = Utils.CoinsCount(out overFlowing, player.bank2.item);
			long num3 = Utils.CoinsCount(out overFlowing, player.bank3.item);
			long num4 = Utils.CoinsCount(out overFlowing, player.bank4.item);
			long num5 = Utils.CoinsCombineStacks(out overFlowing, num, num2, num3, num4);
			if (num5 > 0)
			{
				Main.GetItemDrawFrame(4076, out var itemTexture, out var itemFrame);
				Main.GetItemDrawFrame(3813, out var itemTexture2, out var itemFrame2);
				Main.GetItemDrawFrame(346, out var itemTexture3, out var _);
				Main.GetItemDrawFrame(87, out var itemTexture4, out var _);
				if (num4 > 0)
				{
					sb.Draw(itemTexture, Utils.CenteredRectangle(new Vector2(shopx + 92f, shopy + 45f), itemFrame.Size() * 0.65f), null, Color.White);
				}
				if (num3 > 0)
				{
					sb.Draw(itemTexture2, Utils.CenteredRectangle(new Vector2(shopx + 92f, shopy + 45f), itemFrame2.Size() * 0.65f), null, Color.White);
				}
				if (num2 > 0)
				{
					sb.Draw(itemTexture3, Utils.CenteredRectangle(new Vector2(shopx + 80f, shopy + 50f), itemTexture3.Size() * 0.65f), null, Color.White);
				}
				if (num > 0)
				{
					sb.Draw(itemTexture4, Utils.CenteredRectangle(new Vector2(shopx + 70f, shopy + 60f), itemTexture4.Size() * 0.65f), null, Color.White);
				}
				DrawMoney(sb, Lang.inter[66].Value, shopx, shopy, Utils.CoinsSplit(num5), horizontal);
			}
		}

		public static void GetItemLight(ref Color currentColor, Item item, bool outInTheWorld = false)
		{
			float scale = 1f;
			GetItemLight(ref currentColor, ref scale, item, outInTheWorld);
		}

		public static void GetItemLight(ref Color currentColor, int type, bool outInTheWorld = false)
		{
			float scale = 1f;
			GetItemLight(ref currentColor, ref scale, type, outInTheWorld);
		}

		public static void GetItemLight(ref Color currentColor, ref float scale, Item item, bool outInTheWorld = false)
		{
			GetItemLight(ref currentColor, ref scale, item.type, outInTheWorld);
		}

		public static Color GetItemLight(ref Color currentColor, ref float scale, int type, bool outInTheWorld = false)
		{
			if (type < 0 || type > 5453)
			{
				return currentColor;
			}
			if (type == 662 || type == 663 || type == 5444 || type == 5450)
			{
				currentColor.R = (byte)Main.DiscoR;
				currentColor.G = (byte)Main.DiscoG;
				currentColor.B = (byte)Main.DiscoB;
				currentColor.A = byte.MaxValue;
			}
			if (type == 5128)
			{
				currentColor.R = (byte)Main.DiscoR;
				currentColor.G = (byte)Main.DiscoG;
				currentColor.B = (byte)Main.DiscoB;
				currentColor.A = byte.MaxValue;
			}
			else if (ItemID.Sets.ItemIconPulse[type])
			{
				scale = Main.essScale;
				currentColor.R = (byte)((float)(int)currentColor.R * scale);
				currentColor.G = (byte)((float)(int)currentColor.G * scale);
				currentColor.B = (byte)((float)(int)currentColor.B * scale);
				currentColor.A = (byte)((float)(int)currentColor.A * scale);
			}
			else if (type == 58 || type == 184 || type == 4143)
			{
				scale = Main.essScale * 0.25f + 0.75f;
				currentColor.R = (byte)((float)(int)currentColor.R * scale);
				currentColor.G = (byte)((float)(int)currentColor.G * scale);
				currentColor.B = (byte)((float)(int)currentColor.B * scale);
				currentColor.A = (byte)((float)(int)currentColor.A * scale);
			}
			return currentColor;
		}

		public static void DrawRadialCircular(SpriteBatch sb, Vector2 position, Player.SelectionRadial radial, Item[] items)
		{
			CircularRadialOpacity = MathHelper.Clamp(CircularRadialOpacity + ((PlayerInput.UsingGamepad && PlayerInput.Triggers.Current.RadialHotbar) ? 0.25f : (-0.15f)), 0f, 1f);
			if (CircularRadialOpacity == 0f)
			{
				return;
			}
			Texture2D value = TextureAssets.HotbarRadial[2].get_Value();
			float num = CircularRadialOpacity * 0.9f;
			float num2 = CircularRadialOpacity * 1f;
			float num3 = (float)(int)Main.mouseTextColor / 255f;
			float num4 = 1f - (1f - num3) * (1f - num3);
			num4 *= 0.785f;
			Color color = Color.White * num4 * num;
			value = TextureAssets.HotbarRadial[1].get_Value();
			float num5 = MathF.PI * 2f / (float)radial.RadialCount;
			float num6 = -MathF.PI / 2f;
			for (int i = 0; i < radial.RadialCount; i++)
			{
				int num7 = radial.Bindings[i];
				Vector2 vector = new Vector2(150f, 0f).RotatedBy(num6 + num5 * (float)i) * num2;
				float num8 = 0.85f;
				if (radial.SelectedBinding == i)
				{
					num8 = 1.7f;
				}
				sb.Draw(value, position + vector, null, color * num8, 0f, value.Size() / 2f, num2 * num8, SpriteEffects.None, 0f);
				if (num7 != -1)
				{
					float inventoryScale = Main.inventoryScale;
					Main.inventoryScale = num2 * num8;
					Draw(sb, items, 14, num7, position + vector + new Vector2(-26f * num2 * num8), Color.White);
					Main.inventoryScale = inventoryScale;
				}
			}
		}

		public static void DrawRadialQuicks(SpriteBatch sb, Vector2 position)
		{
			QuicksRadialOpacity = MathHelper.Clamp(QuicksRadialOpacity + ((PlayerInput.UsingGamepad && PlayerInput.Triggers.Current.RadialQuickbar) ? 0.25f : (-0.15f)), 0f, 1f);
			if (QuicksRadialOpacity == 0f)
			{
				return;
			}
			Player player = Main.player[Main.myPlayer];
			Texture2D value = TextureAssets.HotbarRadial[2].get_Value();
			Texture2D value2 = TextureAssets.QuicksIcon.get_Value();
			float num = QuicksRadialOpacity * 0.9f;
			float num2 = QuicksRadialOpacity * 1f;
			float num3 = (float)(int)Main.mouseTextColor / 255f;
			float num4 = 1f - (1f - num3) * (1f - num3);
			num4 *= 0.785f;
			Color color = Color.White * num4 * num;
			float num5 = MathF.PI * 2f / (float)player.QuicksRadial.RadialCount;
			float num6 = -MathF.PI / 2f;
			Item item = player.QuickHeal_GetItemToUse();
			Item item2 = player.QuickMana_GetItemToUse();
			Item item3 = null;
			Item item4 = null;
			if (item == null)
			{
				item = new Item();
				item.SetDefaults(28);
			}
			if (item2 == null)
			{
				item2 = new Item();
				item2.SetDefaults(110);
			}
			if (item3 == null)
			{
				item3 = new Item();
				item3.SetDefaults(292);
			}
			if (item4 == null)
			{
				item4 = new Item();
				item4.SetDefaults(2428);
			}
			for (int i = 0; i < player.QuicksRadial.RadialCount; i++)
			{
				Item inv = item4;
				if (i == 1)
				{
					inv = item;
				}
				if (i == 2)
				{
					inv = item3;
				}
				if (i == 3)
				{
					inv = item2;
				}
				_ = player.QuicksRadial.Bindings[i];
				Vector2 vector = new Vector2(120f, 0f).RotatedBy(num6 + num5 * (float)i) * num2;
				float num7 = 0.85f;
				if (player.QuicksRadial.SelectedBinding == i)
				{
					num7 = 1.7f;
				}
				sb.Draw(value, position + vector, null, color * num7, 0f, value.Size() / 2f, num2 * num7 * 1.3f, SpriteEffects.None, 0f);
				float inventoryScale = Main.inventoryScale;
				Main.inventoryScale = num2 * num7;
				Draw(sb, ref inv, 14, position + vector + new Vector2(-26f * num2 * num7), Color.White);
				Main.inventoryScale = inventoryScale;
				sb.Draw(value2, position + vector + new Vector2(34f, 20f) * 0.85f * num2 * num7, null, color * num7, 0f, value.Size() / 2f, num2 * num7 * 1.3f, SpriteEffects.None, 0f);
			}
		}

		public static void DrawRadialDpad(SpriteBatch sb, Vector2 position)
		{
			if (!PlayerInput.UsingGamepad || !PlayerInput.CurrentProfile.UsingDpadHotbar())
			{
				return;
			}
			Player player = Main.player[Main.myPlayer];
			if (player.chest != -1)
			{
				return;
			}
			Texture2D value = TextureAssets.HotbarRadial[0].get_Value();
			float num = (float)(int)Main.mouseTextColor / 255f;
			float num2 = 1f - (1f - num) * (1f - num);
			num2 *= 0.785f;
			Color color = Color.White * num2;
			sb.Draw(value, position, null, color, 0f, value.Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);
			for (int i = 0; i < 4; i++)
			{
				int num3 = player.DpadRadial.Bindings[i];
				if (num3 != -1)
				{
					Draw(sb, player.inventory, 14, num3, position + new Vector2(value.Width / 3, 0f).RotatedBy(-MathF.PI / 2f + MathF.PI / 2f * (float)i) + new Vector2(-26f * Main.inventoryScale), Color.White);
				}
			}
		}

		public static string GetGamepadInstructions(ref Item inv, int context = 0)
		{
			singleSlotArray[0] = inv;
			string gamepadInstructions = GetGamepadInstructions(singleSlotArray, context);
			inv = singleSlotArray[0];
			return gamepadInstructions;
		}

		public static bool CanExecuteCommand()
		{
			return PlayerInput.AllowExecutionOfGamepadInstructions;
		}

		public static string GetGamepadInstructions(Item[] inv, int context = 0, int slot = 0)
		{
			Player player = Main.player[Main.myPlayer];
			string s = "";
			if (inv == null || inv[slot] == null || Main.mouseItem == null)
			{
				return s;
			}
			if (context == 0 || context == 1 || context == 2)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
						if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
						{
							s += PlayerInput.BuildCommand(Lang.misc[55].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
						}
					}
					else
					{
						if (context == 0 && player.chest == -1)
						{
							player.DpadRadial.ChangeBinding(slot);
						}
						s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
						if (inv[slot].maxStack > 1)
						{
							s += PlayerInput.BuildCommand(Lang.misc[55].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
						}
					}
					if (inv[slot].maxStack == 1 && Equippable(inv, context, slot))
					{
						s += PlayerInput.BuildCommand(Lang.misc[67].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
						if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							SwapEquip(inv, context, slot);
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					s += PlayerInput.BuildCommand(Lang.misc[83].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
					{
						inv[slot].favorited = !inv[slot].favorited;
						PlayerInput.LockGamepadButtons("SmartCursor");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				else if (Main.mouseItem.type > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
			}
			if (context == 3 || context == 4 || context == 32)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
						if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
						{
							s += PlayerInput.BuildCommand(Lang.misc[55].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
						}
					}
					else
					{
						s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
						if (inv[slot].maxStack > 1)
						{
							s += PlayerInput.BuildCommand(Lang.misc[55].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
						}
					}
					if (inv[slot].maxStack == 1 && Equippable(inv, context, slot))
					{
						s += PlayerInput.BuildCommand(Lang.misc[67].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
						if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							SwapEquip(inv, context, slot);
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					if (context == 32)
					{
						s += PlayerInput.BuildCommand(Lang.misc[83].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
						if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
						{
							inv[slot].favorited = !inv[slot].favorited;
							PlayerInput.LockGamepadButtons("SmartCursor");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
				}
				else if (Main.mouseItem.type > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
			}
			if (context == 15)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
						{
							s += PlayerInput.BuildCommand(Lang.misc[91].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
						}
					}
					else
					{
						s += PlayerInput.BuildCommand(Lang.misc[90].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"], PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				else if (Main.mouseItem.type > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[92].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
			}
			if (context == 8 || context == 9 || context == 16 || context == 17 || context == 18 || context == 19 || context == 20)
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						if (Equippable(ref Main.mouseItem, context))
						{
							s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
						}
					}
					else if (context != 8 || !isEquipLocked(inv[slot].type))
					{
						s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
					if (context == 8 && slot >= 3)
					{
						bool flag = player.hideVisibleAccessory[slot];
						s += PlayerInput.BuildCommand(Lang.misc[flag ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
						if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							player.hideVisibleAccessory[slot] = !player.hideVisibleAccessory[slot];
							SoundEngine.PlaySound(12);
							if (Main.netMode == 1)
							{
								NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
							}
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					if ((context == 16 || context == 17 || context == 18 || context == 19 || context == 20) && slot < 2)
					{
						bool flag2 = player.hideMisc[slot];
						s += PlayerInput.BuildCommand(Lang.misc[flag2 ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
						if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							if (slot == 0)
							{
								player.TogglePet();
							}
							if (slot == 1)
							{
								player.ToggleLight();
							}
							SoundEngine.PlaySound(12);
							if (Main.netMode == 1)
							{
								NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
							}
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
				}
				else
				{
					if (Main.mouseItem.type > 0 && Equippable(ref Main.mouseItem, context))
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
					if (context == 8 && slot >= 3)
					{
						int num = slot;
						if (num % 10 == 8 && !player.CanDemonHeartAccessoryBeShown())
						{
							num++;
						}
						bool flag3 = player.hideVisibleAccessory[num];
						s += PlayerInput.BuildCommand(Lang.misc[flag3 ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
						if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							player.hideVisibleAccessory[num] = !player.hideVisibleAccessory[num];
							SoundEngine.PlaySound(12);
							if (Main.netMode == 1)
							{
								NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
							}
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					if ((context == 16 || context == 17 || context == 18 || context == 19 || context == 20) && slot < 2)
					{
						bool flag4 = player.hideMisc[slot];
						s += PlayerInput.BuildCommand(Lang.misc[flag4 ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
						if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
						{
							if (slot == 0)
							{
								player.TogglePet();
							}
							if (slot == 1)
							{
								player.ToggleLight();
							}
							Main.mouseLeftRelease = false;
							SoundEngine.PlaySound(12);
							if (Main.netMode == 1)
							{
								NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
							}
							PlayerInput.LockGamepadButtons("Grapple");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
				}
			}
			switch (context)
			{
			case 12:
			case 25:
			case 27:
			case 33:
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						if (Main.mouseItem.dye > 0)
						{
							s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
						}
					}
					else
					{
						s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
					if (context == 12 || context == 25 || context == 27 || context == 33)
					{
						int num2 = -1;
						if (inv == player.dye)
						{
							num2 = slot;
						}
						if (inv == player.miscDyes)
						{
							num2 = 10 + slot;
						}
						if (num2 != -1)
						{
							if (num2 < 10)
							{
								bool flag7 = player.hideVisibleAccessory[slot];
								s += PlayerInput.BuildCommand(Lang.misc[flag7 ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
								if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
								{
									player.hideVisibleAccessory[slot] = !player.hideVisibleAccessory[slot];
									SoundEngine.PlaySound(12);
									if (Main.netMode == 1)
									{
										NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
									}
									PlayerInput.LockGamepadButtons("Grapple");
									PlayerInput.SettingsForUI.TryRevertingToMouseMode();
								}
							}
							else
							{
								bool flag8 = player.hideMisc[slot];
								s += PlayerInput.BuildCommand(Lang.misc[flag8 ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
								if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
								{
									player.hideMisc[slot] = !player.hideMisc[slot];
									SoundEngine.PlaySound(12);
									if (Main.netMode == 1)
									{
										NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
									}
									PlayerInput.LockGamepadButtons("Grapple");
									PlayerInput.SettingsForUI.TryRevertingToMouseMode();
								}
							}
						}
					}
				}
				else if (Main.mouseItem.type > 0 && Main.mouseItem.dye > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
				return s;
			case 18:
			{
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						if (Main.mouseItem.dye > 0)
						{
							s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
						}
					}
					else
					{
						s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
				}
				else if (Main.mouseItem.type > 0 && Main.mouseItem.dye > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
				bool enabledSuperCart = player.enabledSuperCart;
				s += PlayerInput.BuildCommand(Language.GetTextValue((!enabledSuperCart) ? "UI.EnableSuperCart" : "UI.DisableSuperCart"), false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
				if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
				{
					player.enabledSuperCart = !player.enabledSuperCart;
					SoundEngine.PlaySound(12);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
					}
					PlayerInput.LockGamepadButtons("Grapple");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
				return s;
			}
			case 6:
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					s = ((Main.mouseItem.type <= 0) ? (s + PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"])) : (s + PlayerInput.BuildCommand(Lang.misc[74].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"])));
				}
				else if (Main.mouseItem.type > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[74].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
				return s;
			case 5:
			case 7:
			{
				bool flag6 = false;
				if (context == 5)
				{
					flag6 = Main.mouseItem.Prefix(-3) || Main.mouseItem.type == 0;
				}
				if (context == 7)
				{
					flag6 = Main.mouseItem.material;
				}
				if (inv[slot].type > 0 && inv[slot].stack > 0)
				{
					if (Main.mouseItem.type > 0)
					{
						if (flag6)
						{
							s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
						}
					}
					else
					{
						s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
				}
				else if (Main.mouseItem.type > 0 && flag6)
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
				return s;
			}
			default:
			{
				string overrideInstructions = GetOverrideInstructions(inv, context, slot);
				bool flag5 = Main.mouseItem.type > 0 && (context == 0 || context == 1 || context == 2 || context == 6 || context == 15 || context == 7 || context == 4 || context == 32 || context == 3);
				if (context != 8 || !isEquipLocked(inv[slot].type))
				{
					if (flag5 && string.IsNullOrEmpty(overrideInstructions))
					{
						s += PlayerInput.BuildCommand(Lang.inter[121].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]);
						if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
						{
							player.DropSelectedItem();
							PlayerInput.LockGamepadButtons("SmartSelect");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					else if (!string.IsNullOrEmpty(overrideInstructions))
					{
						ShiftForcedOn = true;
						int cursorOverride = Main.cursorOverride;
						OverrideHover(inv, context, slot);
						if (-1 != Main.cursorOverride)
						{
							s += PlayerInput.BuildCommand(overrideInstructions, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]);
							if (CanDoSimulatedClickAction() && CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
							{
								bool mouseLeft = Main.mouseLeft;
								Main.mouseLeft = true;
								LeftClick(inv, context, slot);
								Main.mouseLeft = mouseLeft;
								PlayerInput.LockGamepadButtons("SmartSelect");
								PlayerInput.SettingsForUI.TryRevertingToMouseMode();
							}
						}
						Main.cursorOverride = cursorOverride;
						ShiftForcedOn = false;
					}
				}
				if (!TryEnteringFastUseMode(inv, context, slot, player, ref s))
				{
					TryEnteringBuildingMode(inv, context, slot, player, ref s);
				}
				return s;
			}
			}
		}

		private static bool CanDoSimulatedClickAction()
		{
			if (PlayerInput.SteamDeckIsUsed)
			{
				return UILinkPointNavigator.InUse;
			}
			return true;
		}

		private static bool TryEnteringFastUseMode(Item[] inv, int context, int slot, Player player, ref string s)
		{
			int num = 0;
			if (Main.mouseItem.CanBeQuickUsed)
			{
				num = 1;
			}
			if (num == 0 && Main.mouseItem.stack <= 0 && context == 0 && inv[slot].CanBeQuickUsed)
			{
				num = 2;
			}
			if (num > 0)
			{
				s += PlayerInput.BuildCommand(Language.GetTextValue("UI.QuickUseItem"), false, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
				if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.QuickMount)
				{
					switch (num)
					{
					case 1:
						PlayerInput.TryEnteringFastUseModeForMouseItem();
						break;
					case 2:
						PlayerInput.TryEnteringFastUseModeForInventorySlot(slot);
						break;
					}
				}
				return true;
			}
			return false;
		}

		private static bool TryEnteringBuildingMode(Item[] inv, int context, int slot, Player player, ref string s)
		{
			int num = 0;
			if (IsABuildingItem(Main.mouseItem))
			{
				num = 1;
			}
			if (num == 0 && Main.mouseItem.stack <= 0 && context == 0 && IsABuildingItem(inv[slot]))
			{
				num = 2;
			}
			if (num > 0)
			{
				Item item = Main.mouseItem;
				if (num == 1)
				{
					item = Main.mouseItem;
				}
				if (num == 2)
				{
					item = inv[slot];
				}
				if (num != 1 || player.ItemSpace(item).CanTakeItemToPersonalInventory)
				{
					if (item.damage > 0 && item.ammo == 0)
					{
						s += PlayerInput.BuildCommand(Lang.misc[60].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
					}
					else if (item.createTile >= 0 || item.createWall > 0)
					{
						s += PlayerInput.BuildCommand(Lang.misc[61].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
					}
					else
					{
						s += PlayerInput.BuildCommand(Lang.misc[63].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
					}
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.QuickMount)
					{
						PlayerInput.EnterBuildingMode();
					}
					return true;
				}
			}
			return false;
		}

		public static bool IsABuildingItem(Item item)
		{
			if (item.type > 0 && item.stack > 0 && item.useStyle != 0)
			{
				return item.useTime > 0;
			}
			return false;
		}

		public static void SelectEquipPage(Item item)
		{
			Main.EquipPage = -1;
			if (!item.IsAir)
			{
				if (Main.projHook[item.shoot])
				{
					Main.EquipPage = 2;
				}
				else if (item.mountType != -1)
				{
					Main.EquipPage = 2;
				}
				else if (item.buffType > 0 && Main.vanityPet[item.buffType])
				{
					Main.EquipPage = 2;
				}
				else if (item.buffType > 0 && Main.lightPet[item.buffType])
				{
					Main.EquipPage = 2;
				}
				else if (item.dye > 0 && Main.EquipPageSelected == 1)
				{
					Main.EquipPage = 0;
				}
				else if (item.legSlot != -1 || item.headSlot != -1 || item.bodySlot != -1 || item.accessory)
				{
					Main.EquipPage = 0;
				}
			}
		}
	}
}
