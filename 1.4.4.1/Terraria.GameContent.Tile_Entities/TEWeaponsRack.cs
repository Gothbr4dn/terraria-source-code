using System.IO;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	public class TEWeaponsRack : TileEntity, IFixLoadedData
	{
		private static byte _myEntityID;

		public Item item;

		private const int MyTileID = 471;

		public TEWeaponsRack()
		{
			item = new Item();
		}

		public override void RegisterTileEntityID(int assignedID)
		{
			_myEntityID = (byte)assignedID;
		}

		public override TileEntity GenerateInstance()
		{
			return new TEWeaponsRack();
		}

		public override void NetPlaceEntityAttempt(int x, int y)
		{
			NetPlaceEntity(x, y);
		}

		public static void NetPlaceEntity(int x, int y)
		{
			int number = Place(x, y);
			NetMessage.SendData(86, -1, -1, null, number, x, y);
		}

		public override bool IsTileValidForEntity(int x, int y)
		{
			return ValidTile(x, y);
		}

		public static bool ValidTile(int x, int y)
		{
			if (!Main.tile[x, y].active() || Main.tile[x, y].type != 471 || Main.tile[x, y].frameY != 0 || Main.tile[x, y].frameX % 54 != 0)
			{
				return false;
			}
			return true;
		}

		public static int Place(int x, int y)
		{
			TEWeaponsRack tEWeaponsRack = new TEWeaponsRack();
			tEWeaponsRack.Position = new Point16(x, y);
			tEWeaponsRack.ID = TileEntity.AssignNewID();
			tEWeaponsRack.type = _myEntityID;
			lock (TileEntity.EntityCreationLock)
			{
				TileEntity.ByID[tEWeaponsRack.ID] = tEWeaponsRack;
				TileEntity.ByPosition[tEWeaponsRack.Position] = tEWeaponsRack;
			}
			return tEWeaponsRack.ID;
		}

		public static int Hook_AfterPlacement(int x, int y, int type = 471, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y, 3, 3);
				NetMessage.SendData(87, -1, -1, null, x, y, (int)_myEntityID);
				return -1;
			}
			return Place(x, y);
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
			writer.Write((short)item.netID);
			writer.Write(item.prefix);
			writer.Write((short)item.stack);
		}

		public override void ReadExtraData(BinaryReader reader, bool networkSend)
		{
			item = new Item();
			item.netDefaults(reader.ReadInt16());
			item.Prefix(reader.ReadByte());
			item.stack = reader.ReadInt16();
		}

		public override string ToString()
		{
			return Position.X + "x  " + Position.Y + "y item: " + item;
		}

		public static void Framing_CheckTile(int callX, int callY)
		{
			int num = 3;
			int num2 = 3;
			if (WorldGen.destroyObject)
			{
				return;
			}
			int num3 = callX;
			int num4 = callY;
			Tile tileSafely = Framing.GetTileSafely(callX, callY);
			num3 -= tileSafely.frameX / 18 % num;
			num4 -= tileSafely.frameY / 18 % num2;
			bool flag = false;
			for (int i = num3; i < num3 + num; i++)
			{
				for (int j = num4; j < num4 + num2; j++)
				{
					Tile tile = Main.tile[i, j];
					if (!tile.active() || tile.type != 471 || tile.wall == 0)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			int num5 = Find(num3, num4);
			if (num5 != -1 && ((TEWeaponsRack)TileEntity.ByID[num5]).item.stack > 0)
			{
				((TEWeaponsRack)TileEntity.ByID[num5]).DropItem();
				if (Main.netMode != 2)
				{
					Main.LocalPlayer.InterruptItemUsageIfOverTile(471);
				}
			}
			WorldGen.destroyObject = true;
			for (int k = num3; k < num3 + num; k++)
			{
				for (int l = num4; l < num4 + num2; l++)
				{
					if (Main.tile[k, l].active() && Main.tile[k, l].type == 471)
					{
						WorldGen.KillTile(k, l);
					}
				}
			}
			Item.NewItem(new EntitySource_TileBreak(num3, num4), num3 * 16, num4 * 16, 48, 48, 2699);
			Kill(num3, num4);
			WorldGen.destroyObject = false;
		}

		public void DropItem()
		{
			if (Main.netMode != 1)
			{
				Item.NewItem(new EntitySource_TileBreak(Position.X, Position.Y), Position.X * 16, Position.Y * 16, 32, 32, item.netID, 1, noBroadcast: false, item.prefix);
			}
			item = new Item();
		}

		public static void TryPlacing(int x, int y, int netid, int prefix, int stack)
		{
			WorldGen.RangeFrame(x, y, x + 3, y + 3);
			int num = Find(x, y);
			if (num == -1)
			{
				int num2 = Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 32, 32, 1);
				Main.item[num2].netDefaults(netid);
				Main.item[num2].Prefix(prefix);
				Main.item[num2].stack = stack;
				NetMessage.SendData(21, -1, -1, null, num2);
				return;
			}
			TEWeaponsRack tEWeaponsRack = (TEWeaponsRack)TileEntity.ByID[num];
			if (tEWeaponsRack.item.stack > 0)
			{
				tEWeaponsRack.DropItem();
			}
			tEWeaponsRack.item = new Item();
			tEWeaponsRack.item.netDefaults(netid);
			tEWeaponsRack.item.Prefix(prefix);
			tEWeaponsRack.item.stack = stack;
			NetMessage.SendData(86, -1, -1, null, tEWeaponsRack.ID, x, y);
		}

		public static void OnPlayerInteraction(Player player, int clickX, int clickY)
		{
			if (FitsWeaponFrame(player.inventory[player.selectedItem]) && !player.inventory[player.selectedItem].favorited)
			{
				player.GamepadEnableGrappleCooldown();
				PlaceItemInFrame(player, clickX, clickY);
				Recipe.FindRecipes();
				return;
			}
			int num = clickX;
			int num2 = clickY;
			num -= Main.tile[num, num2].frameX % 54 / 18;
			num2 -= Main.tile[num, num2].frameY % 54 / 18;
			int num3 = Find(num, num2);
			if (num3 != -1 && ((TEWeaponsRack)TileEntity.ByID[num3]).item.stack > 0)
			{
				player.GamepadEnableGrappleCooldown();
				WorldGen.KillTile(num, num2, fail: true);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, num, num2, 1f);
				}
			}
		}

		public static bool FitsWeaponFrame(Item i)
		{
			if (!i.IsAir && (i.fishingPole > 0 || ItemID.Sets.CanBePlacedOnWeaponRacks[i.type]))
			{
				return true;
			}
			if (i.damage > 0 && i.useStyle != 0)
			{
				return i.stack > 0;
			}
			return false;
		}

		private static void PlaceItemInFrame(Player player, int x, int y)
		{
			if (!player.ItemTimeIsZero)
			{
				return;
			}
			x -= Main.tile[x, y].frameX % 54 / 18;
			y -= Main.tile[x, y].frameY % 54 / 18;
			int num = Find(x, y);
			if (num == -1)
			{
				return;
			}
			if (((TEWeaponsRack)TileEntity.ByID[num]).item.stack > 0)
			{
				WorldGen.KillTile(x, y, fail: true);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, Player.tileTargetX, y, 1f);
				}
			}
			if (Main.netMode == 1)
			{
				NetMessage.SendData(123, -1, -1, null, x, y, player.selectedItem, player.whoAmI, 1);
			}
			else
			{
				TryPlacing(x, y, player.inventory[player.selectedItem].netID, player.inventory[player.selectedItem].prefix, 1);
			}
			player.inventory[player.selectedItem].stack--;
			if (player.inventory[player.selectedItem].stack <= 0)
			{
				player.inventory[player.selectedItem].SetDefaults();
				Main.mouseItem.SetDefaults();
			}
			if (player.selectedItem == 58)
			{
				Main.mouseItem = player.inventory[player.selectedItem].Clone();
			}
			player.releaseUseItem = false;
			player.mouseInterface = true;
			player.PlayDroppedItemAnimation(20);
			WorldGen.RangeFrame(x, y, x + 3, y + 3);
		}

		public void FixLoadedData()
		{
			item.FixAgainstExploit();
		}
	}
}
