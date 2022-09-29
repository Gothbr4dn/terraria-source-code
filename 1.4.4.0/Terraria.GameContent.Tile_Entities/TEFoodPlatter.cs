using System.IO;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent.Tile_Entities
{
	public class TEFoodPlatter : TileEntity, IFixLoadedData
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
			return new TEFoodPlatter();
		}

		public TEFoodPlatter()
		{
			item = new Item();
		}

		public static int Place(int x, int y)
		{
			TEFoodPlatter tEFoodPlatter = new TEFoodPlatter();
			tEFoodPlatter.Position = new Point16(x, y);
			tEFoodPlatter.ID = TileEntity.AssignNewID();
			tEFoodPlatter.type = _myEntityID;
			lock (TileEntity.EntityCreationLock)
			{
				TileEntity.ByID[tEFoodPlatter.ID] = tEFoodPlatter;
				TileEntity.ByPosition[tEFoodPlatter.Position] = tEFoodPlatter;
			}
			return tEFoodPlatter.ID;
		}

		public override bool IsTileValidForEntity(int x, int y)
		{
			return ValidTile(x, y);
		}

		public static int Hook_AfterPlacement(int x, int y, int type = 520, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x, y);
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
			if (!Main.tile[x, y].active() || Main.tile[x, y].type != 520 || Main.tile[x, y].frameY != 0)
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
				Item.NewItem(new EntitySource_TileBreak(Position.X, Position.Y), Position.X * 16, Position.Y * 16, 16, 16, item.netID, 1, noBroadcast: false, item.prefix);
			}
			item = new Item();
		}

		public static void TryPlacing(int x, int y, int netid, int prefix, int stack)
		{
			WorldGen.RangeFrame(x, y, x + 1, y + 1);
			int num = Find(x, y);
			if (num == -1)
			{
				int num2 = Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 16, 16, 1);
				Main.item[num2].netDefaults(netid);
				Main.item[num2].Prefix(prefix);
				Main.item[num2].stack = stack;
				NetMessage.SendData(21, -1, -1, null, num2);
				return;
			}
			TEFoodPlatter tEFoodPlatter = (TEFoodPlatter)TileEntity.ByID[num];
			if (tEFoodPlatter.item.stack > 0)
			{
				tEFoodPlatter.DropItem();
			}
			tEFoodPlatter.item = new Item();
			tEFoodPlatter.item.netDefaults(netid);
			tEFoodPlatter.item.Prefix(prefix);
			tEFoodPlatter.item.stack = stack;
			NetMessage.SendData(86, -1, -1, null, tEFoodPlatter.ID, x, y);
		}

		public static void OnPlayerInteraction(Player player, int clickX, int clickY)
		{
			if (FitsFoodPlatter(player.inventory[player.selectedItem]) && !player.inventory[player.selectedItem].favorited)
			{
				player.GamepadEnableGrappleCooldown();
				PlaceItemInFrame(player, clickX, clickY);
				Recipe.FindRecipes();
				return;
			}
			int num = Find(clickX, clickY);
			if (num != -1 && ((TEFoodPlatter)TileEntity.ByID[num]).item.stack > 0)
			{
				player.GamepadEnableGrappleCooldown();
				WorldGen.KillTile(clickX, clickY, fail: true);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, clickX, clickY, 1f);
				}
			}
		}

		public static bool FitsFoodPlatter(Item i)
		{
			if (i.stack > 0)
			{
				return ItemID.Sets.IsFood[i.type];
			}
			return false;
		}

		public static void PlaceItemInFrame(Player player, int x, int y)
		{
			if (!player.ItemTimeIsZero)
			{
				return;
			}
			int num = Find(x, y);
			if (num == -1)
			{
				return;
			}
			if (((TEFoodPlatter)TileEntity.ByID[num]).item.stack > 0)
			{
				WorldGen.KillTile(x, y, fail: true);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(17, -1, -1, null, 0, Player.tileTargetX, y, 1f);
				}
			}
			if (Main.netMode == 1)
			{
				NetMessage.SendData(133, -1, -1, null, x, y, player.selectedItem, player.whoAmI, 1);
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
			WorldGen.RangeFrame(x, y, x + 1, y + 1);
		}

		public void FixLoadedData()
		{
			item.FixAgainstExploit();
		}
	}
}
