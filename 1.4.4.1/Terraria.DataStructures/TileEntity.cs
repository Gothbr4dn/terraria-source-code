using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;

namespace Terraria.DataStructures
{
	public abstract class TileEntity
	{
		public static TileEntitiesManager manager;

		public const int MaxEntitiesPerChunk = 1000;

		public static object EntityCreationLock = new object();

		public static Dictionary<int, TileEntity> ByID = new Dictionary<int, TileEntity>();

		public static Dictionary<Point16, TileEntity> ByPosition = new Dictionary<Point16, TileEntity>();

		public static int TileEntitiesNextID;

		public int ID;

		public Point16 Position;

		public byte type;

		public static event Action _UpdateStart;

		public static event Action _UpdateEnd;

		public static int AssignNewID()
		{
			return TileEntitiesNextID++;
		}

		public static void Clear()
		{
			ByID.Clear();
			ByPosition.Clear();
			TileEntitiesNextID = 0;
		}

		public static void UpdateStart()
		{
			if (TileEntity._UpdateStart != null)
			{
				TileEntity._UpdateStart();
			}
		}

		public static void UpdateEnd()
		{
			if (TileEntity._UpdateEnd != null)
			{
				TileEntity._UpdateEnd();
			}
		}

		public static void InitializeAll()
		{
			manager = new TileEntitiesManager();
			manager.RegisterAll();
		}

		public static void PlaceEntityNet(int x, int y, int type)
		{
			if (WorldGen.InWorld(x, y) && !ByPosition.ContainsKey(new Point16(x, y)))
			{
				manager.NetPlaceEntity(type, x, y);
			}
		}

		public virtual void Update()
		{
		}

		public static void Write(BinaryWriter writer, TileEntity ent, bool networkSend = false)
		{
			writer.Write(ent.type);
			ent.WriteInner(writer, networkSend);
		}

		public static TileEntity Read(BinaryReader reader, bool networkSend = false)
		{
			byte id = reader.ReadByte();
			TileEntity tileEntity = manager.GenerateInstance(id);
			tileEntity.type = id;
			tileEntity.ReadInner(reader, networkSend);
			return tileEntity;
		}

		private void WriteInner(BinaryWriter writer, bool networkSend)
		{
			if (!networkSend)
			{
				writer.Write(ID);
			}
			writer.Write(Position.X);
			writer.Write(Position.Y);
			WriteExtraData(writer, networkSend);
		}

		private void ReadInner(BinaryReader reader, bool networkSend)
		{
			if (!networkSend)
			{
				ID = reader.ReadInt32();
			}
			Position = new Point16(reader.ReadInt16(), reader.ReadInt16());
			ReadExtraData(reader, networkSend);
		}

		public virtual void WriteExtraData(BinaryWriter writer, bool networkSend)
		{
		}

		public virtual void ReadExtraData(BinaryReader reader, bool networkSend)
		{
		}

		public virtual void OnPlayerUpdate(Player player)
		{
		}

		public static bool IsOccupied(int id, out int interactingPlayer)
		{
			interactingPlayer = -1;
			for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if (player.active && !player.dead && player.tileEntityAnchor.interactEntityID == id)
				{
					interactingPlayer = i;
					return true;
				}
			}
			return false;
		}

		public virtual void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
		{
		}

		public virtual string GetItemGamepadInstructions(int slot = 0)
		{
			return "";
		}

		public virtual bool TryGetItemGamepadOverrideInstructions(Item[] inv, int context, int slot, out string instruction)
		{
			instruction = null;
			return false;
		}

		public virtual bool OverrideItemSlotHover(Item[] inv, int context = 0, int slot = 0)
		{
			return false;
		}

		public virtual bool OverrideItemSlotLeftClick(Item[] inv, int context = 0, int slot = 0)
		{
			return false;
		}

		public static void BasicOpenCloseInteraction(Player player, int x, int y, int id)
		{
			player.CloseSign();
			int interactingPlayer;
			if (Main.netMode != 1)
			{
				Main.stackSplit = 600;
				player.GamepadEnableGrappleCooldown();
				if (IsOccupied(id, out interactingPlayer))
				{
					if (interactingPlayer == player.whoAmI)
					{
						Recipe.FindRecipes();
						SoundEngine.PlaySound(11);
						player.tileEntityAnchor.Clear();
					}
				}
				else
				{
					SetInteractionAnchor(player, x, y, id);
				}
				return;
			}
			Main.stackSplit = 600;
			player.GamepadEnableGrappleCooldown();
			if (IsOccupied(id, out interactingPlayer))
			{
				if (interactingPlayer == player.whoAmI)
				{
					Recipe.FindRecipes();
					SoundEngine.PlaySound(11);
					player.tileEntityAnchor.Clear();
					NetMessage.SendData(122, -1, -1, null, -1, Main.myPlayer);
				}
			}
			else
			{
				NetMessage.SendData(122, -1, -1, null, id, Main.myPlayer);
			}
		}

		public static void SetInteractionAnchor(Player player, int x, int y, int id)
		{
			player.chest = -1;
			player.SetTalkNPC(-1);
			if (player.whoAmI == Main.myPlayer)
			{
				Main.playerInventory = true;
				Main.recBigList = false;
				Main.CreativeMenu.CloseMenu();
				if (PlayerInput.GrappleAndInteractAreShared)
				{
					PlayerInput.Triggers.JustPressed.Grapple = false;
				}
				if (player.tileEntityAnchor.interactEntityID != -1)
				{
					SoundEngine.PlaySound(12);
				}
				else
				{
					SoundEngine.PlaySound(10);
				}
			}
			player.tileEntityAnchor.Set(id, x, y);
		}

		public virtual void RegisterTileEntityID(int assignedID)
		{
		}

		public virtual void NetPlaceEntityAttempt(int x, int y)
		{
		}

		public virtual bool IsTileValidForEntity(int x, int y)
		{
			return false;
		}

		public virtual TileEntity GenerateInstance()
		{
			return null;
		}
	}
}
