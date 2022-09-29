using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.GameContent.Tile_Entities
{
	public class TETrainingDummy : TileEntity
	{
		private static Dictionary<int, Rectangle> playerBox = new Dictionary<int, Rectangle>();

		private static bool playerBoxFilled;

		private static byte _myEntityID;

		public int npc;

		public override void RegisterTileEntityID(int assignedID)
		{
			_myEntityID = (byte)assignedID;
			TileEntity._UpdateStart += ClearBoxes;
		}

		public override void NetPlaceEntityAttempt(int x, int y)
		{
			NetPlaceEntity(x, y);
		}

		public static void NetPlaceEntity(int x, int y)
		{
			Place(x, y);
		}

		public override TileEntity GenerateInstance()
		{
			return new TETrainingDummy();
		}

		public override bool IsTileValidForEntity(int x, int y)
		{
			return ValidTile(x, y);
		}

		public static void ClearBoxes()
		{
			playerBox.Clear();
			playerBoxFilled = false;
		}

		public override void Update()
		{
			Rectangle value = new Rectangle(0, 0, 32, 48);
			value.Inflate(1600, 1600);
			int x = value.X;
			int y = value.Y;
			if (npc != -1)
			{
				if (!Main.npc[npc].active || Main.npc[npc].type != 488 || Main.npc[npc].ai[0] != (float)Position.X || Main.npc[npc].ai[1] != (float)Position.Y)
				{
					Deactivate();
				}
				return;
			}
			FillPlayerHitboxes();
			value.X = Position.X * 16 + x;
			value.Y = Position.Y * 16 + y;
			bool flag = false;
			foreach (KeyValuePair<int, Rectangle> item in playerBox)
			{
				if (item.Value.Intersects(value))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				Activate();
			}
		}

		private static void FillPlayerHitboxes()
		{
			if (playerBoxFilled)
			{
				return;
			}
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					playerBox[i] = Main.player[i].getRect();
				}
			}
			playerBoxFilled = true;
		}

		public static bool ValidTile(int x, int y)
		{
			if (!Main.tile[x, y].active() || Main.tile[x, y].type != 378 || Main.tile[x, y].frameY != 0 || Main.tile[x, y].frameX % 36 != 0)
			{
				return false;
			}
			return true;
		}

		public TETrainingDummy()
		{
			npc = -1;
		}

		public static int Place(int x, int y)
		{
			TETrainingDummy tETrainingDummy = new TETrainingDummy();
			tETrainingDummy.Position = new Point16(x, y);
			tETrainingDummy.ID = TileEntity.AssignNewID();
			tETrainingDummy.type = _myEntityID;
			lock (TileEntity.EntityCreationLock)
			{
				TileEntity.ByID[tETrainingDummy.ID] = tETrainingDummy;
				TileEntity.ByPosition[tETrainingDummy.Position] = tETrainingDummy;
			}
			return tETrainingDummy.ID;
		}

		public static int Hook_AfterPlacement(int x, int y, int type = 378, int style = 0, int direction = 1, int alternate = 0)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(Main.myPlayer, x - 1, y - 2, 2, 3);
				NetMessage.SendData(87, -1, -1, null, x - 1, y - 2, (int)_myEntityID);
				return -1;
			}
			return Place(x - 1, y - 2);
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
			writer.Write((short)npc);
		}

		public override void ReadExtraData(BinaryReader reader, bool networkSend)
		{
			npc = reader.ReadInt16();
		}

		public void Activate()
		{
			int num = NPC.NewNPC(new EntitySource_TileEntity(this), Position.X * 16 + 16, Position.Y * 16 + 48, 488, 100);
			Main.npc[num].ai[0] = Position.X;
			Main.npc[num].ai[1] = Position.Y;
			Main.npc[num].netUpdate = true;
			npc = num;
			if (Main.netMode != 1)
			{
				NetMessage.SendData(86, -1, -1, null, ID, Position.X, Position.Y);
			}
		}

		public void Deactivate()
		{
			if (npc != -1)
			{
				Main.npc[npc].active = false;
			}
			npc = -1;
			if (Main.netMode != 1)
			{
				NetMessage.SendData(86, -1, -1, null, ID, Position.X, Position.Y);
			}
		}

		public override string ToString()
		{
			return Position.X + "x  " + Position.Y + "y npc: " + npc;
		}
	}
}
