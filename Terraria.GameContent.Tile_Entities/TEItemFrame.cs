using System.IO;
using Terraria.DataStructures;

namespace Terraria.GameContent.Tile_Entities
{
	public class TEItemFrame : TileEntity, IFixLoadedData
	{
		private static byte _myEntityID;

		public Item item;

		public override void RegisterTileEntityID(int assignedID)
		{
			_myEntityID = (byte)assignedID;
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

		public override TileEntity GenerateInstance()
		{
			return new TEItemFrame();
		}

		public TEItemFrame()
		{
			item = new Item();
		}

		public static int Place(int x, int y)
		{
			TEItemFrame tEItemFrame = new TEItemFrame();
			tEItemFrame.Position = new Point16(x, y);
			tEItemFrame.ID = TileEntity.AssignNewID();
			tEItemFrame.type = _myEntityID;
			lock (TileEntity.EntityCreationLock)
			{
				TileEntity.ByID[tEItemFrame.ID] = tEItemFrame;
				TileEntity.ByPosition[tEItemFrame.Position] = tEItemFrame;
			}
			return tEItemFrame.ID;
		}

		public override bool IsTileValidForEntity(int x, int y)
		{
			return ValidTile(x, y);
		}

		public static int Hook_AfterPlacement(int x, int y, int type = 395, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y, 2, 2);
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

		public static bool ValidTile(int x, int y)
		{
			if (!Main.tile[x, y].active() || Main.tile[x, y].type != 395 || Main.tile[x, y].frameY != 0 || Main.tile[x, y].frameX % 36 != 0)
			{
				return false;
			}
			return true;
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
			WorldGen.RangeFrame(x, y, x + 2, y + 2);
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
			TEItemFrame tEItemFrame = (TEItemFrame)TileEntity.ByID[num];
			if (tEItemFrame.item.stack > 0)
			{
				tEItemFrame.DropItem();
			}
			tEItemFrame.item = new Item();
			tEItemFrame.item.netDefaults(netid);
			tEItemFrame.item.Prefix(prefix);
			tEItemFrame.item.stack = stack;
			NetMessage.SendData(86, -1, -1, null, tEItemFrame.ID, x, y);
		}

		public static void OnPlayerInteraction(Player player, int clickX, int clickY)
		{
			if (FitsItemFrame(player.inventory[player.selectedItem]) && !player.inventory[player.selectedItem].favorited)
			{
				player.GamepadEnableGrappleCooldown();
				PlaceItemInFrame(player, clickX, clickY);
				Recipe.FindRecipes();
				return;
			}
			int num = clickX;
			int num2 = clickY;
			if (Main.tile[num, num2].frameX % 36 != 0)
			{
				num--;
			}
			if (Main.tile[num, num2].frameY % 36 != 0)
			{
				num2--;
			}
			int num3 = Find(num, num2);
			if (num3 != -1 && ((TEItemFrame)TileEntity.ByID[num3]).item.stack > 0)
			{
				player.GamepadEnableGrappleCooldown();
				WorldGen.KillTile(clickX, clickY, fail: true);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, num, num2, 1f);
				}
			}
		}

		public static bool FitsItemFrame(Item i)
		{
			return i.stack > 0;
		}

		public static void PlaceItemInFrame(Player player, int x, int y)
		{
			if (!player.ItemTimeIsZero)
			{
				return;
			}
			if (Main.tile[x, y].frameX % 36 != 0)
			{
				x--;
			}
			if (Main.tile[x, y].frameY % 36 != 0)
			{
				y--;
			}
			int num = Find(x, y);
			if (num == -1)
			{
				return;
			}
			if (((TEItemFrame)TileEntity.ByID[num]).item.stack > 0)
			{
				WorldGen.KillTile(x, y, fail: true);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, Player.tileTargetX, y, 1f);
				}
			}
			if (Main.netMode == 1)
			{
				NetMessage.SendData(89, -1, -1, null, x, y, player.selectedItem, player.whoAmI, 1);
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
			WorldGen.RangeFrame(x, y, x + 2, y + 2);
		}

		public void FixLoadedData()
		{
			item.FixAgainstExploit();
		}
	}
}
