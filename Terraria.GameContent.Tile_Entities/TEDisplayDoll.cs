using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.GameContent.Tile_Entities
{
	public class TEDisplayDoll : TileEntity, IFixLoadedData
	{
		private static byte _myEntityID;

		private const int MyTileID = 470;

		private const int entityTileWidth = 2;

		private const int entityTileHeight = 3;

		private Player _dollPlayer;

		private Item[] _items;

		private Item[] _dyes;

		private static int accessoryTargetSlot = 3;

		public TEDisplayDoll()
		{
			_items = new Item[8];
			for (int i = 0; i < _items.Length; i++)
			{
				_items[i] = new Item();
			}
			_dyes = new Item[8];
			for (int j = 0; j < _dyes.Length; j++)
			{
				_dyes[j] = new Item();
			}
			_dollPlayer = new Player();
			_dollPlayer.hair = 15;
			_dollPlayer.skinColor = Color.White;
			_dollPlayer.skinVariant = 10;
		}

		public override void RegisterTileEntityID(int assignedID)
		{
			_myEntityID = (byte)assignedID;
		}

		public override TileEntity GenerateInstance()
		{
			return new TEDisplayDoll();
		}

		public override void NetPlaceEntityAttempt(int x, int y)
		{
			int number = Place(x, y);
			NetMessage.SendData(86, -1, -1, null, number, x, y);
		}

		public static int Place(int x, int y)
		{
			TEDisplayDoll tEDisplayDoll = new TEDisplayDoll();
			tEDisplayDoll.Position = new Point16(x, y);
			tEDisplayDoll.ID = TileEntity.AssignNewID();
			tEDisplayDoll.type = _myEntityID;
			lock (TileEntity.EntityCreationLock)
			{
				TileEntity.ByID[tEDisplayDoll.ID] = tEDisplayDoll;
				TileEntity.ByPosition[tEDisplayDoll.Position] = tEDisplayDoll;
			}
			return tEDisplayDoll.ID;
		}

		public static int Hook_AfterPlacement(int x, int y, int type = 470, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y - 2, 2, 3);
				NetMessage.SendData(87, -1, -1, null, x, y - 2, (int)_myEntityID);
				return -1;
			}
			return Place(x, y - 2);
		}

		public static void Kill(int x, int y)
		{
			if (TileEntity.ByPosition.TryGetValue(new Point16(x, y), out var value) && value.type == _myEntityID)
			{
				lock (TileEntity.EntityCreationLock)
				{
					TileEntity.ByID.Remove(value.ID);
					TileEntity.ByPosition.Remove(new Point16(x, y));
				}
			}
		}

		public static int Find(int x, int y)
		{
			if (TileEntity.ByPosition.TryGetValue(new Point16(x, y), out var value) && value.type == _myEntityID)
			{
				return value.ID;
			}
			return -1;
		}

		public override void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
			BitsByte bitsByte = (byte)0;
			bitsByte[0] = !_items[0].IsAir;
			bitsByte[1] = !_items[1].IsAir;
			bitsByte[2] = !_items[2].IsAir;
			bitsByte[3] = !_items[3].IsAir;
			bitsByte[4] = !_items[4].IsAir;
			bitsByte[5] = !_items[5].IsAir;
			bitsByte[6] = !_items[6].IsAir;
			bitsByte[7] = !_items[7].IsAir;
			BitsByte bitsByte2 = (byte)0;
			bitsByte2[0] = !_dyes[0].IsAir;
			bitsByte2[1] = !_dyes[1].IsAir;
			bitsByte2[2] = !_dyes[2].IsAir;
			bitsByte2[3] = !_dyes[3].IsAir;
			bitsByte2[4] = !_dyes[4].IsAir;
			bitsByte2[5] = !_dyes[5].IsAir;
			bitsByte2[6] = !_dyes[6].IsAir;
			bitsByte2[7] = !_dyes[7].IsAir;
			writer.Write(bitsByte);
			writer.Write(bitsByte2);
			for (int i = 0; i < 8; i++)
			{
				Item item = _items[i];
				if (!item.IsAir)
				{
					writer.Write((short)item.netID);
					writer.Write(item.prefix);
					writer.Write((short)item.stack);
				}
			}
			for (int j = 0; j < 8; j++)
			{
				Item item2 = _dyes[j];
				if (!item2.IsAir)
				{
					writer.Write((short)item2.netID);
					writer.Write(item2.prefix);
					writer.Write((short)item2.stack);
				}
			}
		}

		public override void ReadExtraData(BinaryReader reader, bool networkSend)
		{
			BitsByte bitsByte = reader.ReadByte();
			BitsByte bitsByte2 = reader.ReadByte();
			for (int i = 0; i < 8; i++)
			{
				_items[i] = new Item();
				Item item = _items[i];
				if (bitsByte[i])
				{
					item.netDefaults(reader.ReadInt16());
					item.Prefix(reader.ReadByte());
					item.stack = reader.ReadInt16();
				}
			}
			for (int j = 0; j < 8; j++)
			{
				_dyes[j] = new Item();
				Item item2 = _dyes[j];
				if (bitsByte2[j])
				{
					item2.netDefaults(reader.ReadInt16());
					item2.Prefix(reader.ReadByte());
					item2.stack = reader.ReadInt16();
				}
			}
		}

		public override string ToString()
		{
			return string.Concat(Position.X, "x  ", Position.Y, "y item: ", _items[0], " ", _items[1], " ", _items[2]);
		}

		public static void Framing_CheckTile(int callX, int callY)
		{
			if (WorldGen.destroyObject)
			{
				return;
			}
			int num = callX;
			int num2 = callY;
			Tile tileSafely = Framing.GetTileSafely(callX, callY);
			num -= tileSafely.frameX / 18 % 2;
			num2 -= tileSafely.frameY / 18 % 3;
			bool flag = false;
			for (int i = num; i < num + 2; i++)
			{
				for (int j = num2; j < num2 + 3; j++)
				{
					Tile tile = Main.tile[i, j];
					if (!tile.active() || tile.type != 470)
					{
						flag = true;
					}
				}
			}
			if (!WorldGen.SolidTileAllowBottomSlope(num, num2 + 3) || !WorldGen.SolidTileAllowBottomSlope(num + 1, num2 + 3))
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			Kill(num, num2);
			if (Main.tile[callX, callY].frameX / 72 != 1)
			{
				Item.NewItem(new EntitySource_TileBreak(num, num2), num * 16, num2 * 16, 32, 48, 498);
			}
			else
			{
				Item.NewItem(new EntitySource_TileBreak(num, num2), num * 16, num2 * 16, 32, 48, 1989);
			}
			WorldGen.destroyObject = true;
			for (int k = num; k < num + 2; k++)
			{
				for (int l = num2; l < num2 + 3; l++)
				{
					if (Main.tile[k, l].active() && Main.tile[k, l].type == 470)
					{
						WorldGen.KillTile(k, l);
					}
				}
			}
			WorldGen.destroyObject = false;
		}

		public void Draw(int tileLeftX, int tileTopY)
		{
			Player dollPlayer = _dollPlayer;
			for (int i = 0; i < 8; i++)
			{
				dollPlayer.armor[i] = _items[i];
				dollPlayer.dye[i] = _dyes[i];
			}
			dollPlayer.direction = -1;
			dollPlayer.Male = true;
			Tile tileSafely = Framing.GetTileSafely(tileLeftX, tileTopY);
			if (tileSafely.frameX % 72 == 36)
			{
				dollPlayer.direction = 1;
			}
			if (tileSafely.frameX / 72 == 1)
			{
				dollPlayer.Male = false;
			}
			dollPlayer.isDisplayDollOrInanimate = true;
			dollPlayer.ResetEffects();
			dollPlayer.ResetVisibleAccessories();
			dollPlayer.UpdateDyes();
			dollPlayer.DisplayDollUpdate();
			dollPlayer.UpdateSocialShadow();
			dollPlayer.PlayerFrame();
			Vector2 vector = (dollPlayer.position = new Vector2(tileLeftX + 1, tileTopY + 3) * 16f + new Vector2(-dollPlayer.width / 2, -dollPlayer.height - 6));
			dollPlayer.isFullbright = tileSafely.fullbrightBlock();
			dollPlayer.skinDyePacked = PlayerDrawHelper.PackShader(tileSafely.color(), PlayerDrawHelper.ShaderConfiguration.TilePaintID);
			Main.PlayerRenderer.DrawPlayer(Main.Camera, dollPlayer, dollPlayer.position, 0f, dollPlayer.fullRotationOrigin);
		}

		public override void OnPlayerUpdate(Player player)
		{
			if (!player.InInteractionRange(player.tileEntityAnchor.X, player.tileEntityAnchor.Y, TileReachCheckSettings.Simple) || player.chest != -1 || player.talkNPC != -1)
			{
				if (player.chest == -1 && player.talkNPC == -1)
				{
					SoundEngine.PlaySound(11);
				}
				player.tileEntityAnchor.Clear();
				Recipe.FindRecipes();
			}
		}

		public static void OnPlayerInteraction(Player player, int clickX, int clickY)
		{
			int num = clickX;
			int num2 = clickY;
			if (Main.tile[num, num2].frameX % 36 != 0)
			{
				num--;
			}
			num2 -= Main.tile[num, num2].frameY / 18;
			int num3 = Find(num, num2);
			if (num3 != -1)
			{
				num2++;
				accessoryTargetSlot = 3;
				TileEntity.BasicOpenCloseInteraction(player, num, num2, num3);
			}
		}

		public override void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
		{
			if (Main.tile[player.tileEntityAnchor.X, player.tileEntityAnchor.Y].type != 470)
			{
				player.tileEntityAnchor.Clear();
				Recipe.FindRecipes();
			}
			else
			{
				DrawInner(player, spriteBatch);
			}
		}

		public override bool TryGetItemGamepadOverrideInstructions(Item[] inv, int context, int slot, out string instruction)
		{
			instruction = "";
			Item item = inv[slot];
			if (item.IsAir || item.favorited)
			{
				return false;
			}
			switch (context)
			{
			case 0:
				if (FitsDisplayDoll(item))
				{
					instruction = Lang.misc[76].Value;
					return true;
				}
				break;
			case 23:
			case 24:
			case 25:
				if (Main.player[Main.myPlayer].ItemSpace(item).CanTakeItemToPersonalInventory)
				{
					instruction = Lang.misc[68].Value;
					return true;
				}
				break;
			}
			return false;
		}

		public override string GetItemGamepadInstructions(int slot = 0)
		{
			Item[] inv = _items;
			int num = slot;
			int context = 23;
			if (slot >= 8)
			{
				num -= 8;
				inv = _dyes;
				context = 25;
			}
			else if (slot >= 3)
			{
				inv = _items;
				context = 24;
			}
			return ItemSlot.GetGamepadInstructions(inv, context, num);
		}

		private void DrawInner(Player player, SpriteBatch spriteBatch)
		{
			Main.inventoryScale = 0.72f;
			DrawSlotPairSet(player, spriteBatch, 3, 0, 0f, 0.5f, 23);
			DrawSlotPairSet(player, spriteBatch, 5, 3, 3f, 0.5f, 24);
		}

		private void DrawSlotPairSet(Player player, SpriteBatch spriteBatch, int slotsToShowLine, int slotsArrayOffset, float offsetX, float offsetY, int inventoryContextTarget)
		{
			Item[] items = _items;
			int num = inventoryContextTarget;
			for (int i = 0; i < slotsToShowLine; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					int num2 = (int)(73f + ((float)i + offsetX) * 56f * Main.inventoryScale);
					int num3 = (int)((float)Main.instance.invBottom + ((float)j + offsetY) * 56f * Main.inventoryScale);
					if (j == 0)
					{
						items = _items;
						num = inventoryContextTarget;
					}
					else
					{
						items = _dyes;
						num = 25;
					}
					if (Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, num2, num3, (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale, (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
					{
						player.mouseInterface = true;
						ItemSlot.Handle(items, num, i + slotsArrayOffset);
					}
					ItemSlot.Draw(spriteBatch, items, num, i + slotsArrayOffset, new Vector2(num2, num3));
				}
			}
		}

		public override bool OverrideItemSlotHover(Item[] inv, int context = 0, int slot = 0)
		{
			Item item = inv[slot];
			if (!item.IsAir && !inv[slot].favorited && context == 0 && FitsDisplayDoll(item))
			{
				Main.cursorOverride = 9;
				return true;
			}
			if (!item.IsAir && (context == 23 || context == 24 || context == 25) && Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
			{
				Main.cursorOverride = 8;
				return true;
			}
			return false;
		}

		public override bool OverrideItemSlotLeftClick(Item[] inv, int context = 0, int slot = 0)
		{
			if (!ItemSlot.ShiftInUse)
			{
				return false;
			}
			if (Main.cursorOverride == 9 && context == 0)
			{
				Item item = inv[slot];
				if (!item.IsAir && !item.favorited && FitsDisplayDoll(item))
				{
					return TryFitting(inv, context, slot);
				}
			}
			if ((Main.cursorOverride == 8 && context == 23) || context == 24 || context == 25)
			{
				inv[slot] = Main.player[Main.myPlayer].GetItem(Main.myPlayer, inv[slot], GetItemSettings.InventoryEntityToPlayerInventorySettings);
				if (Main.netMode == 1)
				{
					if (context == 25)
					{
						NetMessage.SendData(121, -1, -1, null, Main.myPlayer, ID, slot, 1f);
					}
					else
					{
						NetMessage.SendData(121, -1, -1, null, Main.myPlayer, ID, slot);
					}
				}
				return true;
			}
			return false;
		}

		public static bool FitsDisplayDoll(Item item)
		{
			if (item.maxStack > 1)
			{
				return false;
			}
			if (item.headSlot <= 0 && item.bodySlot <= 0 && item.legSlot <= 0)
			{
				return item.accessory;
			}
			return true;
		}

		private bool TryFitting(Item[] inv, int context = 0, int slot = 0, bool justCheck = false)
		{
			Item item = inv[slot];
			int num = -1;
			if (item.headSlot > 0)
			{
				num = 0;
			}
			if (item.bodySlot > 0)
			{
				num = 1;
			}
			if (item.legSlot > 0)
			{
				num = 2;
			}
			if (item.accessory)
			{
				num = accessoryTargetSlot;
			}
			if (num == -1)
			{
				return false;
			}
			if (justCheck)
			{
				return true;
			}
			if (item.accessory)
			{
				accessoryTargetSlot++;
				if (accessoryTargetSlot >= 8)
				{
					accessoryTargetSlot = 3;
				}
				for (int i = 3; i < 8; i++)
				{
					if (_items[i].IsAir)
					{
						num = i;
						accessoryTargetSlot = i;
						break;
					}
				}
				for (int j = 3; j < 8; j++)
				{
					if (inv[slot].type == _items[j].type)
					{
						num = j;
					}
				}
			}
			SoundEngine.PlaySound(7);
			Utils.Swap(ref _items[num], ref inv[slot]);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, ID, num);
			}
			return true;
		}

		public void WriteItem(int itemIndex, BinaryWriter writer, bool dye)
		{
			Item item = _items[itemIndex];
			if (dye)
			{
				item = _dyes[itemIndex];
			}
			writer.Write((ushort)item.netID);
			writer.Write((ushort)item.stack);
			writer.Write(item.prefix);
		}

		public void ReadItem(int itemIndex, BinaryReader reader, bool dye)
		{
			int defaults = reader.ReadUInt16();
			int stack = reader.ReadUInt16();
			int prefixWeWant = reader.ReadByte();
			Item item = _items[itemIndex];
			if (dye)
			{
				item = _dyes[itemIndex];
			}
			item.SetDefaults(defaults);
			item.stack = stack;
			item.Prefix(prefixWeWant);
		}

		public override bool IsTileValidForEntity(int x, int y)
		{
			if (!Main.tile[x, y].active() || Main.tile[x, y].type != 470 || Main.tile[x, y].frameY != 0 || Main.tile[x, y].frameX % 36 != 0)
			{
				return false;
			}
			return true;
		}

		public void SetInventoryFromMannequin(int headFrame, int shirtFrame, int legFrame)
		{
			headFrame /= 100;
			shirtFrame /= 100;
			legFrame /= 100;
			if (headFrame >= 0 && headFrame < Item.headType.Length)
			{
				_items[0].SetDefaults(Item.headType[headFrame]);
			}
			if (shirtFrame >= 0 && shirtFrame < Item.bodyType.Length)
			{
				_items[1].SetDefaults(Item.bodyType[shirtFrame]);
			}
			if (legFrame >= 0 && legFrame < Item.legType.Length)
			{
				_items[2].SetDefaults(Item.legType[legFrame]);
			}
		}

		public static bool IsBreakable(int clickX, int clickY)
		{
			int num = clickX;
			int num2 = clickY;
			if (Main.tile[num, num2].frameX % 36 != 0)
			{
				num--;
			}
			num2 -= Main.tile[num, num2].frameY / 18;
			int num3 = Find(num, num2);
			if (num3 != -1)
			{
				return !(TileEntity.ByID[num3] as TEDisplayDoll).ContainsItems();
			}
			return true;
		}

		public bool ContainsItems()
		{
			for (int i = 0; i < 8; i++)
			{
				if (!_items[i].IsAir || !_dyes[i].IsAir)
				{
					return true;
				}
			}
			return false;
		}

		public void FixLoadedData()
		{
			Item[] items = _items;
			for (int i = 0; i < items.Length; i++)
			{
				items[i].FixAgainstExploit();
			}
			items = _dyes;
			for (int i = 0; i < items.Length; i++)
			{
				items[i].FixAgainstExploit();
			}
		}
	}
}
