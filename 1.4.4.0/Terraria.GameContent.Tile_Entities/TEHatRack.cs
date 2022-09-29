using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.GameContent.Tile_Entities
{
	public class TEHatRack : TileEntity, IFixLoadedData
	{
		private static byte _myEntityID;

		private const int MyTileID = 475;

		private const int entityTileWidth = 3;

		private const int entityTileHeight = 4;

		private Player _dollPlayer;

		private Item[] _items;

		private Item[] _dyes;

		private static int hatTargetSlot;

		public TEHatRack()
		{
			_items = new Item[2];
			for (int i = 0; i < _items.Length; i++)
			{
				_items[i] = new Item();
			}
			_dyes = new Item[2];
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
			return new TEHatRack();
		}

		public override void NetPlaceEntityAttempt(int x, int y)
		{
			int number = Place(x, y);
			NetMessage.SendData(86, -1, -1, null, number, x, y);
		}

		public static int Place(int x, int y)
		{
			TEHatRack tEHatRack = new TEHatRack();
			tEHatRack.Position = new Point16(x, y);
			tEHatRack.ID = TileEntity.AssignNewID();
			tEHatRack.type = _myEntityID;
			lock (TileEntity.EntityCreationLock)
			{
				TileEntity.ByID[tEHatRack.ID] = tEHatRack;
				TileEntity.ByPosition[tEHatRack.Position] = tEHatRack;
			}
			return tEHatRack.ID;
		}

		public static int Hook_AfterPlacement(int x, int y, int type = 475, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x - 1, y - 3, 3, 4);
				NetMessage.SendData(87, -1, -1, null, x + -1, y + -3, (int)_myEntityID);
				return -1;
			}
			return Place(x + -1, y + -3);
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
			bitsByte[2] = !_dyes[0].IsAir;
			bitsByte[3] = !_dyes[1].IsAir;
			writer.Write(bitsByte);
			for (int i = 0; i < 2; i++)
			{
				Item item = _items[i];
				if (!item.IsAir)
				{
					writer.Write((short)item.netID);
					writer.Write(item.prefix);
					writer.Write((short)item.stack);
				}
			}
			for (int j = 0; j < 2; j++)
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
			for (int i = 0; i < 2; i++)
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
			for (int j = 0; j < 2; j++)
			{
				_dyes[j] = new Item();
				Item item2 = _dyes[j];
				if (bitsByte[j + 2])
				{
					item2.netDefaults(reader.ReadInt16());
					item2.Prefix(reader.ReadByte());
					item2.stack = reader.ReadInt16();
				}
			}
		}

		public override string ToString()
		{
			return string.Concat(Position.X, "x  ", Position.Y, "y item: ", _items[0], " ", _items[1]);
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
			num -= tileSafely.frameX / 18 % 3;
			num2 -= tileSafely.frameY / 18 % 4;
			bool flag = false;
			for (int i = num; i < num + 3; i++)
			{
				for (int j = num2; j < num2 + 4; j++)
				{
					Tile tile = Main.tile[i, j];
					if (!tile.active() || tile.type != 475)
					{
						flag = true;
					}
				}
			}
			if (!WorldGen.SolidTileAllowBottomSlope(num, num2 + 4) || !WorldGen.SolidTileAllowBottomSlope(num + 1, num2 + 4) || !WorldGen.SolidTileAllowBottomSlope(num + 2, num2 + 4))
			{
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			Kill(num, num2);
			Item.NewItem(new EntitySource_TileBreak(num, num2), num * 16, num2 * 16, 48, 64, 3977);
			WorldGen.destroyObject = true;
			for (int k = num; k < num + 3; k++)
			{
				for (int l = num2; l < num2 + 4; l++)
				{
					if (Main.tile[k, l].active() && Main.tile[k, l].type == 475)
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
			dollPlayer.direction = -1;
			dollPlayer.Male = true;
			if (Framing.GetTileSafely(tileLeftX, tileTopY).frameX % 216 == 54)
			{
				dollPlayer.direction = 1;
			}
			dollPlayer.isDisplayDollOrInanimate = true;
			dollPlayer.isHatRackDoll = true;
			dollPlayer.armor[0] = _items[0];
			dollPlayer.dye[0] = _dyes[0];
			dollPlayer.ResetEffects();
			dollPlayer.ResetVisibleAccessories();
			dollPlayer.invis = true;
			dollPlayer.UpdateDyes();
			dollPlayer.DisplayDollUpdate();
			dollPlayer.PlayerFrame();
			Vector2 vector = new Vector2((float)tileLeftX + 1.5f, tileTopY + 4) * 16f;
			dollPlayer.direction *= -1;
			Vector2 vector2 = new Vector2(-dollPlayer.width / 2, -dollPlayer.height - 6) + new Vector2(dollPlayer.direction * 14, -2f);
			dollPlayer.position = vector + vector2;
			Main.PlayerRenderer.DrawPlayer(Main.Camera, dollPlayer, dollPlayer.position, 0f, dollPlayer.fullRotationOrigin);
			dollPlayer.armor[0] = _items[1];
			dollPlayer.dye[0] = _dyes[1];
			dollPlayer.ResetEffects();
			dollPlayer.ResetVisibleAccessories();
			dollPlayer.invis = true;
			dollPlayer.UpdateDyes();
			dollPlayer.DisplayDollUpdate();
			dollPlayer.skipAnimatingValuesInPlayerFrame = true;
			dollPlayer.PlayerFrame();
			dollPlayer.skipAnimatingValuesInPlayerFrame = false;
			dollPlayer.direction *= -1;
			vector2 = new Vector2(-dollPlayer.width / 2, -dollPlayer.height - 6) + new Vector2(dollPlayer.direction * 12, 16f);
			dollPlayer.position = vector + vector2;
			Main.PlayerRenderer.DrawPlayer(Main.Camera, dollPlayer, dollPlayer.position, 0f, dollPlayer.fullRotationOrigin);
		}

		public override string GetItemGamepadInstructions(int slot = 0)
		{
			Item[] inv = _items;
			int num = slot;
			int context = 26;
			if (slot >= 2)
			{
				num -= 2;
				inv = _dyes;
				context = 27;
			}
			return ItemSlot.GetGamepadInstructions(inv, context, num);
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
				if (FitsHatRack(item))
				{
					instruction = Lang.misc[76].Value;
					return true;
				}
				break;
			case 26:
			case 27:
				if (Main.player[Main.myPlayer].ItemSpace(item).CanTakeItemToPersonalInventory)
				{
					instruction = Lang.misc[68].Value;
					return true;
				}
				break;
			}
			return false;
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
			num -= Main.tile[num, num2].frameX % 54 / 18;
			num2 -= Main.tile[num, num2].frameY / 18;
			int num3 = Find(num, num2);
			if (num3 != -1)
			{
				num2++;
				num++;
				TileEntity.BasicOpenCloseInteraction(player, num, num2, num3);
			}
		}

		public override void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
		{
			if (Main.tile[player.tileEntityAnchor.X, player.tileEntityAnchor.Y].type != 475)
			{
				player.tileEntityAnchor.Clear();
				Recipe.FindRecipes();
			}
			else
			{
				DrawInner(player, spriteBatch);
			}
		}

		private void DrawInner(Player player, SpriteBatch spriteBatch)
		{
			Main.inventoryScale = 0.72f;
			DrawSlotPairSet(player, spriteBatch, 2, 0, 3.5f, 0.5f, 26);
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
						num = 27;
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
			if (!item.IsAir && !inv[slot].favorited && context == 0 && FitsHatRack(item))
			{
				Main.cursorOverride = 9;
				return true;
			}
			if (!item.IsAir && (context == 26 || context == 27) && Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
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
				if (Main.cursorOverride == 9 && !item.IsAir && !item.favorited && context == 0 && FitsHatRack(item))
				{
					return TryFitting(inv, context, slot);
				}
			}
			if ((Main.cursorOverride == 8 && context == 23) || context == 26 || context == 27)
			{
				inv[slot] = Main.player[Main.myPlayer].GetItem(Main.myPlayer, inv[slot], GetItemSettings.InventoryEntityToPlayerInventorySettings);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(124, -1, -1, null, Main.myPlayer, ID, slot);
				}
				return true;
			}
			return false;
		}

		public static bool FitsHatRack(Item item)
		{
			if (item.maxStack > 1)
			{
				return false;
			}
			return item.headSlot > 0;
		}

		private bool TryFitting(Item[] inv, int context = 0, int slot = 0, bool justCheck = false)
		{
			if (!FitsHatRack(inv[slot]))
			{
				return false;
			}
			if (justCheck)
			{
				return true;
			}
			int num = hatTargetSlot;
			hatTargetSlot++;
			for (int i = 0; i < 2; i++)
			{
				if (_items[i].IsAir)
				{
					num = i;
					hatTargetSlot = i + 1;
					break;
				}
			}
			for (int j = 0; j < 2; j++)
			{
				if (inv[slot].type == _items[j].type)
				{
					num = j;
				}
			}
			if (hatTargetSlot >= 2)
			{
				hatTargetSlot = 0;
			}
			SoundEngine.PlaySound(7);
			Utils.Swap(ref _items[num], ref inv[slot]);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, ID, num);
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
			if (!Main.tile[x, y].active() || Main.tile[x, y].type != 475 || Main.tile[x, y].frameY != 0 || Main.tile[x, y].frameX % 54 != 0)
			{
				return false;
			}
			return true;
		}

		public static bool IsBreakable(int clickX, int clickY)
		{
			int num = clickX;
			int num2 = clickY;
			num -= Main.tile[num, num2].frameX % 54 / 18;
			num2 -= Main.tile[num, num2].frameY / 18;
			int num3 = Find(num, num2);
			if (num3 != -1)
			{
				return !(TileEntity.ByID[num3] as TEHatRack).ContainsItems();
			}
			return true;
		}

		public bool ContainsItems()
		{
			for (int i = 0; i < 2; i++)
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
