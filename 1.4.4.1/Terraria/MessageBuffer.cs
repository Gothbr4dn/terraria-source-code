using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Golf;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Testing;
using Terraria.UI;

namespace Terraria
{
	public class MessageBuffer
	{
		public const int readBufferMax = 131070;

		public const int writeBufferMax = 131070;

		public bool broadcast;

		public byte[] readBuffer = new byte[131070];

		public byte[] writeBuffer = new byte[131070];

		public bool writeLocked;

		public int messageLength;

		public int totalData;

		public int whoAmI;

		public int spamCount;

		public int maxSpam;

		public bool checkBytes;

		public MemoryStream readerStream;

		public MemoryStream writerStream;

		public BinaryReader reader;

		public BinaryWriter writer;

		public PacketHistory History = new PacketHistory();

		private float[] _temporaryProjectileAI = new float[Projectile.maxAI];

		private float[] _temporaryNPCAI = new float[NPC.maxAI];

		public static event TileChangeReceivedEvent OnTileChangeReceived;

		public void Reset()
		{
			Array.Clear(readBuffer, 0, readBuffer.Length);
			Array.Clear(writeBuffer, 0, writeBuffer.Length);
			writeLocked = false;
			messageLength = 0;
			totalData = 0;
			spamCount = 0;
			broadcast = false;
			checkBytes = false;
			ResetReader();
			ResetWriter();
		}

		public void ResetReader()
		{
			if (readerStream != null)
			{
				readerStream.Close();
			}
			readerStream = new MemoryStream(readBuffer);
			reader = new BinaryReader(readerStream);
		}

		public void ResetWriter()
		{
			if (writerStream != null)
			{
				writerStream.Close();
			}
			writerStream = new MemoryStream(writeBuffer);
			writer = new BinaryWriter(writerStream);
		}

		private float[] ReUseTemporaryProjectileAI()
		{
			for (int i = 0; i < _temporaryProjectileAI.Length; i++)
			{
				_temporaryProjectileAI[i] = 0f;
			}
			return _temporaryProjectileAI;
		}

		private float[] ReUseTemporaryNPCAI()
		{
			for (int i = 0; i < _temporaryNPCAI.Length; i++)
			{
				_temporaryNPCAI[i] = 0f;
			}
			return _temporaryNPCAI;
		}

		public void GetData(int start, int length, out int messageType)
		{
			if (whoAmI < 256)
			{
				Netplay.Clients[whoAmI].TimeOutTimer = 0;
			}
			else
			{
				Netplay.Connection.TimeOutTimer = 0;
			}
			byte b = 0;
			int num = 0;
			num = start + 1;
			b = (byte)(messageType = readBuffer[start]);
			if (b >= 149)
			{
				return;
			}
			Main.ActiveNetDiagnosticsUI.CountReadMessage(b, length);
			if (Main.netMode == 1 && Netplay.Connection.StatusMax > 0)
			{
				Netplay.Connection.StatusCount++;
			}
			if (Main.verboseNetplay)
			{
				for (int i = start; i < start + length; i++)
				{
				}
				for (int j = start; j < start + length; j++)
				{
					_ = readBuffer[j];
				}
			}
			if (Main.netMode == 2 && b != 38 && Netplay.Clients[whoAmI].State == -1)
			{
				NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[1].ToNetworkText());
				return;
			}
			if (Main.netMode == 2)
			{
				if (Netplay.Clients[whoAmI].State < 10 && b > 12 && b != 93 && b != 16 && b != 42 && b != 50 && b != 38 && b != 68 && b != 147)
				{
					NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
				}
				if (Netplay.Clients[whoAmI].State == 0 && b != 1)
				{
					NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
				}
			}
			if (reader == null)
			{
				ResetReader();
			}
			reader.BaseStream.Position = num;
			switch (b)
			{
			case 1:
				if (Main.netMode != 2)
				{
					break;
				}
				if (Main.dedServ && Netplay.IsBanned(Netplay.Clients[whoAmI].Socket.GetRemoteAddress()))
				{
					NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[3].ToNetworkText());
				}
				else
				{
					if (Netplay.Clients[whoAmI].State != 0)
					{
						break;
					}
					if (reader.ReadString() == "Terraria" + 270)
					{
						if (string.IsNullOrEmpty(Netplay.ServerPassword))
						{
							Netplay.Clients[whoAmI].State = 1;
							NetMessage.TrySendData(3, whoAmI);
						}
						else
						{
							Netplay.Clients[whoAmI].State = -1;
							NetMessage.TrySendData(37, whoAmI);
						}
					}
					else
					{
						NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[4].ToNetworkText());
					}
				}
				break;
			case 2:
				if (Main.netMode == 1)
				{
					Netplay.Disconnect = true;
					Main.statusText = NetworkText.Deserialize(reader).ToString();
				}
				break;
			case 3:
				if (Main.netMode == 1)
				{
					if (Netplay.Connection.State == 1)
					{
						Netplay.Connection.State = 2;
					}
					int num76 = reader.ReadByte();
					bool value4 = reader.ReadBoolean();
					Netplay.Connection.ServerSpecialFlags[2] = value4;
					if (num76 != Main.myPlayer)
					{
						Main.player[num76] = Main.ActivePlayerFileData.Player;
						Main.player[Main.myPlayer] = new Player();
					}
					Main.player[num76].whoAmI = num76;
					Main.myPlayer = num76;
					Player player7 = Main.player[num76];
					NetMessage.TrySendData(4, -1, -1, null, num76);
					NetMessage.TrySendData(68, -1, -1, null, num76);
					NetMessage.TrySendData(16, -1, -1, null, num76);
					NetMessage.TrySendData(42, -1, -1, null, num76);
					NetMessage.TrySendData(50, -1, -1, null, num76);
					NetMessage.TrySendData(147, -1, -1, null, num76, player7.CurrentLoadoutIndex);
					for (int num77 = 0; num77 < 59; num77++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num76, PlayerItemSlotID.Inventory0 + num77, (int)player7.inventory[num77].prefix);
					}
					TrySendingItemArray(num76, player7.armor, PlayerItemSlotID.Armor0);
					TrySendingItemArray(num76, player7.dye, PlayerItemSlotID.Dye0);
					TrySendingItemArray(num76, player7.miscEquips, PlayerItemSlotID.Misc0);
					TrySendingItemArray(num76, player7.miscDyes, PlayerItemSlotID.MiscDye0);
					TrySendingItemArray(num76, player7.bank.item, PlayerItemSlotID.Bank1_0);
					TrySendingItemArray(num76, player7.bank2.item, PlayerItemSlotID.Bank2_0);
					NetMessage.TrySendData(5, -1, -1, null, num76, PlayerItemSlotID.TrashItem, (int)player7.trashItem.prefix);
					TrySendingItemArray(num76, player7.bank3.item, PlayerItemSlotID.Bank3_0);
					TrySendingItemArray(num76, player7.bank4.item, PlayerItemSlotID.Bank4_0);
					TrySendingItemArray(num76, player7.Loadouts[0].Armor, PlayerItemSlotID.Loadout1_Armor_0);
					TrySendingItemArray(num76, player7.Loadouts[0].Dye, PlayerItemSlotID.Loadout1_Dye_0);
					TrySendingItemArray(num76, player7.Loadouts[1].Armor, PlayerItemSlotID.Loadout2_Armor_0);
					TrySendingItemArray(num76, player7.Loadouts[1].Dye, PlayerItemSlotID.Loadout2_Dye_0);
					TrySendingItemArray(num76, player7.Loadouts[2].Armor, PlayerItemSlotID.Loadout3_Armor_0);
					TrySendingItemArray(num76, player7.Loadouts[2].Dye, PlayerItemSlotID.Loadout3_Dye_0);
					NetMessage.TrySendData(6);
					if (Netplay.Connection.State == 2)
					{
						Netplay.Connection.State = 3;
					}
				}
				break;
			case 4:
			{
				int num69 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num69 = whoAmI;
				}
				if (num69 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					break;
				}
				Player player6 = Main.player[num69];
				player6.whoAmI = num69;
				player6.skinVariant = reader.ReadByte();
				player6.skinVariant = (int)MathHelper.Clamp(player6.skinVariant, 0f, 11f);
				player6.hair = reader.ReadByte();
				if (player6.hair >= 165)
				{
					player6.hair = 0;
				}
				player6.name = reader.ReadString().Trim().Trim();
				player6.hairDye = reader.ReadByte();
				BitsByte bitsByte7 = reader.ReadByte();
				for (int num70 = 0; num70 < 8; num70++)
				{
					player6.hideVisibleAccessory[num70] = bitsByte7[num70];
				}
				bitsByte7 = reader.ReadByte();
				for (int num71 = 0; num71 < 2; num71++)
				{
					player6.hideVisibleAccessory[num71 + 8] = bitsByte7[num71];
				}
				player6.hideMisc = reader.ReadByte();
				player6.hairColor = reader.ReadRGB();
				player6.skinColor = reader.ReadRGB();
				player6.eyeColor = reader.ReadRGB();
				player6.shirtColor = reader.ReadRGB();
				player6.underShirtColor = reader.ReadRGB();
				player6.pantsColor = reader.ReadRGB();
				player6.shoeColor = reader.ReadRGB();
				BitsByte bitsByte8 = reader.ReadByte();
				player6.difficulty = 0;
				if (bitsByte8[0])
				{
					player6.difficulty = 1;
				}
				if (bitsByte8[1])
				{
					player6.difficulty = 2;
				}
				if (bitsByte8[3])
				{
					player6.difficulty = 3;
				}
				if (player6.difficulty > 3)
				{
					player6.difficulty = 3;
				}
				player6.extraAccessory = bitsByte8[2];
				BitsByte bitsByte9 = reader.ReadByte();
				player6.UsingBiomeTorches = bitsByte9[0];
				player6.happyFunTorchTime = bitsByte9[1];
				player6.unlockedBiomeTorches = bitsByte9[2];
				player6.unlockedSuperCart = bitsByte9[3];
				player6.enabledSuperCart = bitsByte9[4];
				BitsByte bitsByte10 = reader.ReadByte();
				player6.usedAegisCrystal = bitsByte10[0];
				player6.usedAegisFruit = bitsByte10[1];
				player6.usedArcaneCrystal = bitsByte10[2];
				player6.usedGalaxyPearl = bitsByte10[3];
				player6.usedGummyWorm = bitsByte10[4];
				player6.usedAmbrosia = bitsByte10[5];
				if (Main.netMode != 2)
				{
					break;
				}
				bool flag3 = false;
				if (Netplay.Clients[whoAmI].State < 10)
				{
					for (int num72 = 0; num72 < 255; num72++)
					{
						if (num72 != num69 && player6.name == Main.player[num72].name && Netplay.Clients[num72].IsActive)
						{
							flag3 = true;
						}
					}
				}
				if (flag3)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey(Lang.mp[5].Key, player6.name));
				}
				else if (player6.name.Length > Player.nameLen)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.NameTooLong"));
				}
				else if (player6.name == "")
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.EmptyName"));
				}
				else if (player6.difficulty == 3 && !Main.GameModeInfo.IsJourneyMode)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.PlayerIsCreativeAndWorldIsNotCreative"));
				}
				else if (player6.difficulty != 3 && Main.GameModeInfo.IsJourneyMode)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.PlayerIsNotCreativeAndWorldIsCreative"));
				}
				else
				{
					Netplay.Clients[whoAmI].Name = player6.name;
					Netplay.Clients[whoAmI].Name = player6.name;
					NetMessage.TrySendData(4, -1, whoAmI, null, num69);
				}
				break;
			}
			case 5:
			{
				int num147 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num147 = whoAmI;
				}
				if (num147 == Main.myPlayer && !Main.ServerSideCharacter && !Main.player[num147].HasLockedInventory())
				{
					break;
				}
				Player player12 = Main.player[num147];
				lock (player12)
				{
					int num148 = reader.ReadInt16();
					int stack4 = reader.ReadInt16();
					int num149 = reader.ReadByte();
					int type7 = reader.ReadInt16();
					Item[] array2 = null;
					Item[] array3 = null;
					int num150 = 0;
					bool flag9 = false;
					Player clientPlayer = Main.clientPlayer;
					if (num148 >= PlayerItemSlotID.Loadout3_Dye_0)
					{
						num150 = num148 - PlayerItemSlotID.Loadout3_Dye_0;
						array2 = player12.Loadouts[2].Dye;
						array3 = clientPlayer.Loadouts[2].Dye;
					}
					else if (num148 >= PlayerItemSlotID.Loadout3_Armor_0)
					{
						num150 = num148 - PlayerItemSlotID.Loadout3_Armor_0;
						array2 = player12.Loadouts[2].Armor;
						array3 = clientPlayer.Loadouts[2].Armor;
					}
					else if (num148 >= PlayerItemSlotID.Loadout2_Dye_0)
					{
						num150 = num148 - PlayerItemSlotID.Loadout2_Dye_0;
						array2 = player12.Loadouts[1].Dye;
						array3 = clientPlayer.Loadouts[1].Dye;
					}
					else if (num148 >= PlayerItemSlotID.Loadout2_Armor_0)
					{
						num150 = num148 - PlayerItemSlotID.Loadout2_Armor_0;
						array2 = player12.Loadouts[1].Armor;
						array3 = clientPlayer.Loadouts[1].Armor;
					}
					else if (num148 >= PlayerItemSlotID.Loadout1_Dye_0)
					{
						num150 = num148 - PlayerItemSlotID.Loadout1_Dye_0;
						array2 = player12.Loadouts[0].Dye;
						array3 = clientPlayer.Loadouts[0].Dye;
					}
					else if (num148 >= PlayerItemSlotID.Loadout1_Armor_0)
					{
						num150 = num148 - PlayerItemSlotID.Loadout1_Armor_0;
						array2 = player12.Loadouts[0].Armor;
						array3 = clientPlayer.Loadouts[0].Armor;
					}
					else if (num148 >= PlayerItemSlotID.Bank4_0)
					{
						num150 = num148 - PlayerItemSlotID.Bank4_0;
						array2 = player12.bank4.item;
						array3 = clientPlayer.bank4.item;
						if (Main.netMode == 1 && player12.disableVoidBag == num150)
						{
							player12.disableVoidBag = -1;
							Recipe.FindRecipes(canDelayCheck: true);
						}
					}
					else if (num148 >= PlayerItemSlotID.Bank3_0)
					{
						num150 = num148 - PlayerItemSlotID.Bank3_0;
						array2 = player12.bank3.item;
						array3 = clientPlayer.bank3.item;
					}
					else if (num148 >= PlayerItemSlotID.TrashItem)
					{
						flag9 = true;
					}
					else if (num148 >= PlayerItemSlotID.Bank2_0)
					{
						num150 = num148 - PlayerItemSlotID.Bank2_0;
						array2 = player12.bank2.item;
						array3 = clientPlayer.bank2.item;
					}
					else if (num148 >= PlayerItemSlotID.Bank1_0)
					{
						num150 = num148 - PlayerItemSlotID.Bank1_0;
						array2 = player12.bank.item;
						array3 = clientPlayer.bank.item;
					}
					else if (num148 >= PlayerItemSlotID.MiscDye0)
					{
						num150 = num148 - PlayerItemSlotID.MiscDye0;
						array2 = player12.miscDyes;
						array3 = clientPlayer.miscDyes;
					}
					else if (num148 >= PlayerItemSlotID.Misc0)
					{
						num150 = num148 - PlayerItemSlotID.Misc0;
						array2 = player12.miscEquips;
						array3 = clientPlayer.miscEquips;
					}
					else if (num148 >= PlayerItemSlotID.Dye0)
					{
						num150 = num148 - PlayerItemSlotID.Dye0;
						array2 = player12.dye;
						array3 = clientPlayer.dye;
					}
					else if (num148 >= PlayerItemSlotID.Armor0)
					{
						num150 = num148 - PlayerItemSlotID.Armor0;
						array2 = player12.armor;
						array3 = clientPlayer.armor;
					}
					else
					{
						num150 = num148 - PlayerItemSlotID.Inventory0;
						array2 = player12.inventory;
						array3 = clientPlayer.inventory;
					}
					if (flag9)
					{
						player12.trashItem = new Item();
						player12.trashItem.netDefaults(type7);
						player12.trashItem.stack = stack4;
						player12.trashItem.Prefix(num149);
						if (num147 == Main.myPlayer && !Main.ServerSideCharacter)
						{
							clientPlayer.trashItem = player12.trashItem.Clone();
						}
					}
					else if (num148 <= 58)
					{
						int type8 = array2[num150].type;
						int stack5 = array2[num150].stack;
						array2[num150] = new Item();
						array2[num150].netDefaults(type7);
						array2[num150].stack = stack4;
						array2[num150].Prefix(num149);
						if (num147 == Main.myPlayer && !Main.ServerSideCharacter)
						{
							array3[num150] = array2[num150].Clone();
						}
						if (num147 == Main.myPlayer && num150 == 58)
						{
							Main.mouseItem = array2[num150].Clone();
						}
						if (num147 == Main.myPlayer && Main.netMode == 1)
						{
							Main.player[num147].inventoryChestStack[num148] = false;
							if (array2[num150].stack != stack5 || array2[num150].type != type8)
							{
								Recipe.FindRecipes(canDelayCheck: true);
							}
						}
					}
					else
					{
						array2[num150] = new Item();
						array2[num150].netDefaults(type7);
						array2[num150].stack = stack4;
						array2[num150].Prefix(num149);
						if (num147 == Main.myPlayer && !Main.ServerSideCharacter)
						{
							array3[num150] = array2[num150].Clone();
						}
					}
					if (Main.netMode == 2 && num147 == whoAmI && num148 <= 58 + player12.armor.Length + player12.dye.Length + player12.miscEquips.Length + player12.miscDyes.Length)
					{
						NetMessage.TrySendData(5, -1, whoAmI, null, num147, num148, num149);
					}
					break;
				}
			}
			case 6:
				if (Main.netMode == 2)
				{
					if (Netplay.Clients[whoAmI].State == 1)
					{
						Netplay.Clients[whoAmI].State = 2;
					}
					NetMessage.TrySendData(7, whoAmI);
					Main.SyncAnInvasion(whoAmI);
				}
				break;
			case 7:
				if (Main.netMode == 1)
				{
					Main.time = reader.ReadInt32();
					BitsByte bitsByte17 = reader.ReadByte();
					Main.dayTime = bitsByte17[0];
					Main.bloodMoon = bitsByte17[1];
					Main.eclipse = bitsByte17[2];
					Main.moonPhase = reader.ReadByte();
					Main.maxTilesX = reader.ReadInt16();
					Main.maxTilesY = reader.ReadInt16();
					Main.spawnTileX = reader.ReadInt16();
					Main.spawnTileY = reader.ReadInt16();
					Main.worldSurface = reader.ReadInt16();
					Main.rockLayer = reader.ReadInt16();
					Main.worldID = reader.ReadInt32();
					Main.worldName = reader.ReadString();
					Main.GameMode = reader.ReadByte();
					Main.ActiveWorldFileData.UniqueId = new Guid(reader.ReadBytes(16));
					Main.ActiveWorldFileData.WorldGeneratorVersion = reader.ReadUInt64();
					Main.moonType = reader.ReadByte();
					WorldGen.setBG(0, reader.ReadByte());
					WorldGen.setBG(10, reader.ReadByte());
					WorldGen.setBG(11, reader.ReadByte());
					WorldGen.setBG(12, reader.ReadByte());
					WorldGen.setBG(1, reader.ReadByte());
					WorldGen.setBG(2, reader.ReadByte());
					WorldGen.setBG(3, reader.ReadByte());
					WorldGen.setBG(4, reader.ReadByte());
					WorldGen.setBG(5, reader.ReadByte());
					WorldGen.setBG(6, reader.ReadByte());
					WorldGen.setBG(7, reader.ReadByte());
					WorldGen.setBG(8, reader.ReadByte());
					WorldGen.setBG(9, reader.ReadByte());
					Main.iceBackStyle = reader.ReadByte();
					Main.jungleBackStyle = reader.ReadByte();
					Main.hellBackStyle = reader.ReadByte();
					Main.windSpeedTarget = reader.ReadSingle();
					Main.numClouds = reader.ReadByte();
					for (int num204 = 0; num204 < 3; num204++)
					{
						Main.treeX[num204] = reader.ReadInt32();
					}
					for (int num205 = 0; num205 < 4; num205++)
					{
						Main.treeStyle[num205] = reader.ReadByte();
					}
					for (int num206 = 0; num206 < 3; num206++)
					{
						Main.caveBackX[num206] = reader.ReadInt32();
					}
					for (int num207 = 0; num207 < 4; num207++)
					{
						Main.caveBackStyle[num207] = reader.ReadByte();
					}
					WorldGen.TreeTops.SyncReceive(reader);
					WorldGen.BackgroundsCache.UpdateCache();
					Main.maxRaining = reader.ReadSingle();
					Main.raining = Main.maxRaining > 0f;
					BitsByte bitsByte18 = reader.ReadByte();
					WorldGen.shadowOrbSmashed = bitsByte18[0];
					NPC.downedBoss1 = bitsByte18[1];
					NPC.downedBoss2 = bitsByte18[2];
					NPC.downedBoss3 = bitsByte18[3];
					Main.hardMode = bitsByte18[4];
					NPC.downedClown = bitsByte18[5];
					Main.ServerSideCharacter = bitsByte18[6];
					NPC.downedPlantBoss = bitsByte18[7];
					if (Main.ServerSideCharacter)
					{
						Main.ActivePlayerFileData.MarkAsServerSide();
					}
					BitsByte bitsByte19 = reader.ReadByte();
					NPC.downedMechBoss1 = bitsByte19[0];
					NPC.downedMechBoss2 = bitsByte19[1];
					NPC.downedMechBoss3 = bitsByte19[2];
					NPC.downedMechBossAny = bitsByte19[3];
					Main.cloudBGActive = (bitsByte19[4] ? 1 : 0);
					WorldGen.crimson = bitsByte19[5];
					Main.pumpkinMoon = bitsByte19[6];
					Main.snowMoon = bitsByte19[7];
					BitsByte bitsByte20 = reader.ReadByte();
					Main.fastForwardTimeToDawn = bitsByte20[1];
					Main.UpdateTimeRate();
					bool num208 = bitsByte20[2];
					NPC.downedSlimeKing = bitsByte20[3];
					NPC.downedQueenBee = bitsByte20[4];
					NPC.downedFishron = bitsByte20[5];
					NPC.downedMartians = bitsByte20[6];
					NPC.downedAncientCultist = bitsByte20[7];
					BitsByte bitsByte21 = reader.ReadByte();
					NPC.downedMoonlord = bitsByte21[0];
					NPC.downedHalloweenKing = bitsByte21[1];
					NPC.downedHalloweenTree = bitsByte21[2];
					NPC.downedChristmasIceQueen = bitsByte21[3];
					NPC.downedChristmasSantank = bitsByte21[4];
					NPC.downedChristmasTree = bitsByte21[5];
					NPC.downedGolemBoss = bitsByte21[6];
					BirthdayParty.ManualParty = bitsByte21[7];
					BitsByte bitsByte22 = reader.ReadByte();
					NPC.downedPirates = bitsByte22[0];
					NPC.downedFrost = bitsByte22[1];
					NPC.downedGoblins = bitsByte22[2];
					Sandstorm.Happening = bitsByte22[3];
					DD2Event.Ongoing = bitsByte22[4];
					DD2Event.DownedInvasionT1 = bitsByte22[5];
					DD2Event.DownedInvasionT2 = bitsByte22[6];
					DD2Event.DownedInvasionT3 = bitsByte22[7];
					BitsByte bitsByte23 = reader.ReadByte();
					NPC.combatBookWasUsed = bitsByte23[0];
					LanternNight.ManualLanterns = bitsByte23[1];
					NPC.downedTowerSolar = bitsByte23[2];
					NPC.downedTowerVortex = bitsByte23[3];
					NPC.downedTowerNebula = bitsByte23[4];
					NPC.downedTowerStardust = bitsByte23[5];
					Main.forceHalloweenForToday = bitsByte23[6];
					Main.forceXMasForToday = bitsByte23[7];
					BitsByte bitsByte24 = reader.ReadByte();
					NPC.boughtCat = bitsByte24[0];
					NPC.boughtDog = bitsByte24[1];
					NPC.boughtBunny = bitsByte24[2];
					NPC.freeCake = bitsByte24[3];
					Main.drunkWorld = bitsByte24[4];
					NPC.downedEmpressOfLight = bitsByte24[5];
					NPC.downedQueenSlime = bitsByte24[6];
					Main.getGoodWorld = bitsByte24[7];
					BitsByte bitsByte25 = reader.ReadByte();
					Main.tenthAnniversaryWorld = bitsByte25[0];
					Main.dontStarveWorld = bitsByte25[1];
					NPC.downedDeerclops = bitsByte25[2];
					Main.notTheBeesWorld = bitsByte25[3];
					Main.remixWorld = bitsByte25[4];
					NPC.unlockedSlimeBlueSpawn = bitsByte25[5];
					NPC.combatBookVolumeTwoWasUsed = bitsByte25[6];
					NPC.peddlersSatchelWasUsed = bitsByte25[7];
					BitsByte bitsByte26 = reader.ReadByte();
					NPC.unlockedSlimeGreenSpawn = bitsByte26[0];
					NPC.unlockedSlimeOldSpawn = bitsByte26[1];
					NPC.unlockedSlimePurpleSpawn = bitsByte26[2];
					NPC.unlockedSlimeRainbowSpawn = bitsByte26[3];
					NPC.unlockedSlimeRedSpawn = bitsByte26[4];
					NPC.unlockedSlimeYellowSpawn = bitsByte26[5];
					NPC.unlockedSlimeCopperSpawn = bitsByte26[6];
					Main.fastForwardTimeToDusk = bitsByte26[7];
					BitsByte bitsByte27 = reader.ReadByte();
					Main.noTrapsWorld = bitsByte27[0];
					Main.zenithWorld = bitsByte27[1];
					Main.sundialCooldown = reader.ReadByte();
					Main.moondialCooldown = reader.ReadByte();
					WorldGen.SavedOreTiers.Copper = reader.ReadInt16();
					WorldGen.SavedOreTiers.Iron = reader.ReadInt16();
					WorldGen.SavedOreTiers.Silver = reader.ReadInt16();
					WorldGen.SavedOreTiers.Gold = reader.ReadInt16();
					WorldGen.SavedOreTiers.Cobalt = reader.ReadInt16();
					WorldGen.SavedOreTiers.Mythril = reader.ReadInt16();
					WorldGen.SavedOreTiers.Adamantite = reader.ReadInt16();
					if (num208)
					{
						Main.StartSlimeRain();
					}
					else
					{
						Main.StopSlimeRain();
					}
					Main.invasionType = reader.ReadSByte();
					Main.LobbyId = reader.ReadUInt64();
					Sandstorm.IntendedSeverity = reader.ReadSingle();
					if (Netplay.Connection.State == 3)
					{
						Main.windSpeedCurrent = Main.windSpeedTarget;
						Netplay.Connection.State = 4;
					}
					Main.checkHalloween();
					Main.checkXMas();
				}
				break;
			case 8:
			{
				if (Main.netMode != 2)
				{
					break;
				}
				NetMessage.TrySendData(7, whoAmI);
				int num125 = reader.ReadInt32();
				int num126 = reader.ReadInt32();
				bool flag8 = true;
				if (num125 == -1 || num126 == -1)
				{
					flag8 = false;
				}
				else if (num125 < 10 || num125 > Main.maxTilesX - 10)
				{
					flag8 = false;
				}
				else if (num126 < 10 || num126 > Main.maxTilesY - 10)
				{
					flag8 = false;
				}
				int num127 = Netplay.GetSectionX(Main.spawnTileX) - 2;
				int num128 = Netplay.GetSectionY(Main.spawnTileY) - 1;
				int num129 = num127 + 5;
				int num130 = num128 + 3;
				if (num127 < 0)
				{
					num127 = 0;
				}
				if (num129 >= Main.maxSectionsX)
				{
					num129 = Main.maxSectionsX;
				}
				if (num128 < 0)
				{
					num128 = 0;
				}
				if (num130 >= Main.maxSectionsY)
				{
					num130 = Main.maxSectionsY;
				}
				int num131 = (num129 - num127) * (num130 - num128);
				List<Point> list = new List<Point>();
				for (int num132 = num127; num132 < num129; num132++)
				{
					for (int num133 = num128; num133 < num130; num133++)
					{
						list.Add(new Point(num132, num133));
					}
				}
				int num134 = -1;
				int num135 = -1;
				if (flag8)
				{
					num125 = Netplay.GetSectionX(num125) - 2;
					num126 = Netplay.GetSectionY(num126) - 1;
					num134 = num125 + 5;
					num135 = num126 + 3;
					if (num125 < 0)
					{
						num125 = 0;
					}
					if (num134 >= Main.maxSectionsX)
					{
						num134 = Main.maxSectionsX - 1;
					}
					if (num126 < 0)
					{
						num126 = 0;
					}
					if (num135 >= Main.maxSectionsY)
					{
						num135 = Main.maxSectionsY - 1;
					}
					for (int num136 = num125; num136 <= num134; num136++)
					{
						for (int num137 = num126; num137 <= num135; num137++)
						{
							if (num136 < num127 || num136 >= num129 || num137 < num128 || num137 >= num130)
							{
								list.Add(new Point(num136, num137));
								num131++;
							}
						}
					}
				}
				PortalHelper.SyncPortalsOnPlayerJoin(whoAmI, 1, list, out var portalSections);
				num131 += portalSections.Count;
				if (Netplay.Clients[whoAmI].State == 2)
				{
					Netplay.Clients[whoAmI].State = 3;
				}
				NetMessage.TrySendData(9, whoAmI, -1, Lang.inter[44].ToNetworkText(), num131);
				Netplay.Clients[whoAmI].StatusText2 = Language.GetTextValue("Net.IsReceivingTileData");
				Netplay.Clients[whoAmI].StatusMax += num131;
				for (int num138 = num127; num138 < num129; num138++)
				{
					for (int num139 = num128; num139 < num130; num139++)
					{
						NetMessage.SendSection(whoAmI, num138, num139);
					}
				}
				if (flag8)
				{
					for (int num140 = num125; num140 <= num134; num140++)
					{
						for (int num141 = num126; num141 <= num135; num141++)
						{
							NetMessage.SendSection(whoAmI, num140, num141);
						}
					}
				}
				for (int num142 = 0; num142 < portalSections.Count; num142++)
				{
					NetMessage.SendSection(whoAmI, portalSections[num142].X, portalSections[num142].Y);
				}
				for (int num143 = 0; num143 < 400; num143++)
				{
					if (Main.item[num143].active)
					{
						NetMessage.TrySendData(21, whoAmI, -1, null, num143);
						NetMessage.TrySendData(22, whoAmI, -1, null, num143);
					}
				}
				for (int num144 = 0; num144 < 200; num144++)
				{
					if (Main.npc[num144].active)
					{
						NetMessage.TrySendData(23, whoAmI, -1, null, num144);
					}
				}
				for (int num145 = 0; num145 < 1000; num145++)
				{
					if (Main.projectile[num145].active && (Main.projPet[Main.projectile[num145].type] || Main.projectile[num145].netImportant))
					{
						NetMessage.TrySendData(27, whoAmI, -1, null, num145);
					}
				}
				for (int num146 = 0; num146 < 290; num146++)
				{
					NetMessage.TrySendData(83, whoAmI, -1, null, num146);
				}
				NetMessage.TrySendData(57, whoAmI);
				NetMessage.TrySendData(103);
				NetMessage.TrySendData(101, whoAmI);
				NetMessage.TrySendData(136, whoAmI);
				NetMessage.TrySendData(49, whoAmI);
				Main.BestiaryTracker.OnPlayerJoining(whoAmI);
				CreativePowerManager.Instance.SyncThingsToJoiningPlayer(whoAmI);
				Main.PylonSystem.OnPlayerJoining(whoAmI);
				break;
			}
			case 9:
				if (Main.netMode == 1)
				{
					Netplay.Connection.StatusMax += reader.ReadInt32();
					Netplay.Connection.StatusText = NetworkText.Deserialize(reader).ToString();
					BitsByte bitsByte33 = reader.ReadByte();
					BitsByte serverSpecialFlags = Netplay.Connection.ServerSpecialFlags;
					serverSpecialFlags[0] = bitsByte33[0];
					serverSpecialFlags[1] = bitsByte33[1];
					Netplay.Connection.ServerSpecialFlags = serverSpecialFlags;
				}
				break;
			case 10:
				if (Main.netMode == 1)
				{
					NetMessage.DecompressTileBlock(reader.BaseStream);
				}
				break;
			case 11:
				if (Main.netMode == 1)
				{
					WorldGen.SectionTileFrame(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
				}
				break;
			case 12:
			{
				int num265 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num265 = whoAmI;
				}
				Player player17 = Main.player[num265];
				player17.SpawnX = reader.ReadInt16();
				player17.SpawnY = reader.ReadInt16();
				player17.respawnTimer = reader.ReadInt32();
				player17.numberOfDeathsPVE = reader.ReadInt16();
				player17.numberOfDeathsPVP = reader.ReadInt16();
				if (player17.respawnTimer > 0)
				{
					player17.dead = true;
				}
				PlayerSpawnContext playerSpawnContext = (PlayerSpawnContext)reader.ReadByte();
				player17.Spawn(playerSpawnContext);
				if (Main.netMode != 2 || Netplay.Clients[whoAmI].State < 3)
				{
					break;
				}
				if (Netplay.Clients[whoAmI].State == 3)
				{
					Netplay.Clients[whoAmI].State = 10;
					NetMessage.buffer[whoAmI].broadcast = true;
					NetMessage.SyncConnectedPlayer(whoAmI);
					bool flag16 = NetMessage.DoesPlayerSlotCountAsAHost(whoAmI);
					Main.countsAsHostForGameplay[whoAmI] = flag16;
					if (NetMessage.DoesPlayerSlotCountAsAHost(whoAmI))
					{
						NetMessage.TrySendData(139, whoAmI, -1, null, whoAmI, flag16.ToInt());
					}
					NetMessage.TrySendData(12, -1, whoAmI, null, whoAmI, (int)(byte)playerSpawnContext);
					NetMessage.TrySendData(74, whoAmI, -1, NetworkText.FromLiteral(Main.player[whoAmI].name), Main.anglerQuest);
					NetMessage.TrySendData(129, whoAmI);
					NetMessage.greetPlayer(whoAmI);
					if (Main.player[num265].unlockedBiomeTorches)
					{
						NPC nPC7 = new NPC();
						nPC7.SetDefaults(664);
						Main.BestiaryTracker.Kills.RegisterKill(nPC7);
					}
				}
				else
				{
					NetMessage.TrySendData(12, -1, whoAmI, null, whoAmI, (int)(byte)playerSpawnContext);
				}
				break;
			}
			case 13:
			{
				int num78 = reader.ReadByte();
				if (num78 != Main.myPlayer || Main.ServerSideCharacter)
				{
					if (Main.netMode == 2)
					{
						num78 = whoAmI;
					}
					Player player8 = Main.player[num78];
					BitsByte bitsByte11 = reader.ReadByte();
					BitsByte bitsByte12 = reader.ReadByte();
					BitsByte bitsByte13 = reader.ReadByte();
					BitsByte bitsByte14 = reader.ReadByte();
					player8.controlUp = bitsByte11[0];
					player8.controlDown = bitsByte11[1];
					player8.controlLeft = bitsByte11[2];
					player8.controlRight = bitsByte11[3];
					player8.controlJump = bitsByte11[4];
					player8.controlUseItem = bitsByte11[5];
					player8.direction = (bitsByte11[6] ? 1 : (-1));
					if (bitsByte12[0])
					{
						player8.pulley = true;
						player8.pulleyDir = (byte)((!bitsByte12[1]) ? 1u : 2u);
					}
					else
					{
						player8.pulley = false;
					}
					player8.vortexStealthActive = bitsByte12[3];
					player8.gravDir = (bitsByte12[4] ? 1 : (-1));
					player8.TryTogglingShield(bitsByte12[5]);
					player8.ghost = bitsByte12[6];
					player8.selectedItem = reader.ReadByte();
					player8.position = reader.ReadVector2();
					if (bitsByte12[2])
					{
						player8.velocity = reader.ReadVector2();
					}
					else
					{
						player8.velocity = Vector2.Zero;
					}
					if (bitsByte13[6])
					{
						player8.PotionOfReturnOriginalUsePosition = reader.ReadVector2();
						player8.PotionOfReturnHomePosition = reader.ReadVector2();
					}
					else
					{
						player8.PotionOfReturnOriginalUsePosition = null;
						player8.PotionOfReturnHomePosition = null;
					}
					player8.tryKeepingHoveringUp = bitsByte13[0];
					player8.IsVoidVaultEnabled = bitsByte13[1];
					player8.sitting.isSitting = bitsByte13[2];
					player8.downedDD2EventAnyDifficulty = bitsByte13[3];
					player8.isPettingAnimal = bitsByte13[4];
					player8.isTheAnimalBeingPetSmall = bitsByte13[5];
					player8.tryKeepingHoveringDown = bitsByte13[7];
					player8.sleeping.SetIsSleepingAndAdjustPlayerRotation(player8, bitsByte14[0]);
					player8.autoReuseAllWeapons = bitsByte14[1];
					player8.controlDownHold = bitsByte14[2];
					player8.isOperatingAnotherEntity = bitsByte14[3];
					if (Main.netMode == 2 && Netplay.Clients[whoAmI].State == 10)
					{
						NetMessage.TrySendData(13, -1, whoAmI, null, num78);
					}
				}
				break;
			}
			case 14:
			{
				int num48 = reader.ReadByte();
				int num49 = reader.ReadByte();
				if (Main.netMode != 1)
				{
					break;
				}
				bool active = Main.player[num48].active;
				if (num49 == 1)
				{
					if (!Main.player[num48].active)
					{
						Main.player[num48] = new Player();
					}
					Main.player[num48].active = true;
				}
				else
				{
					Main.player[num48].active = false;
				}
				if (active != Main.player[num48].active)
				{
					if (Main.player[num48].active)
					{
						Player.Hooks.PlayerConnect(num48);
					}
					else
					{
						Player.Hooks.PlayerDisconnect(num48);
					}
				}
				break;
			}
			case 16:
			{
				int num221 = reader.ReadByte();
				if (num221 != Main.myPlayer || Main.ServerSideCharacter)
				{
					if (Main.netMode == 2)
					{
						num221 = whoAmI;
					}
					Player player14 = Main.player[num221];
					player14.statLife = reader.ReadInt16();
					player14.statLifeMax = reader.ReadInt16();
					if (player14.statLifeMax < 100)
					{
						player14.statLifeMax = 100;
					}
					player14.dead = player14.statLife <= 0;
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(16, -1, whoAmI, null, num221);
					}
				}
				break;
			}
			case 17:
			{
				byte b7 = reader.ReadByte();
				int num186 = reader.ReadInt16();
				int num187 = reader.ReadInt16();
				short num188 = reader.ReadInt16();
				int num189 = reader.ReadByte();
				bool flag10 = num188 == 1;
				if (!WorldGen.InWorld(num186, num187, 3))
				{
					break;
				}
				if (Main.tile[num186, num187] == null)
				{
					Main.tile[num186, num187] = new Tile();
				}
				if (Main.netMode == 2)
				{
					if (!flag10)
					{
						if (b7 == 0 || b7 == 2 || b7 == 4)
						{
							Netplay.Clients[whoAmI].SpamDeleteBlock += 1f;
						}
						if (b7 == 1 || b7 == 3)
						{
							Netplay.Clients[whoAmI].SpamAddBlock += 1f;
						}
					}
					if (!Netplay.Clients[whoAmI].TileSections[Netplay.GetSectionX(num186), Netplay.GetSectionY(num187)])
					{
						flag10 = true;
					}
				}
				if (b7 == 0)
				{
					WorldGen.KillTile(num186, num187, flag10);
					if (Main.netMode == 1 && !flag10)
					{
						HitTile.ClearAllTilesAtThisLocation(num186, num187);
					}
				}
				bool flag11 = false;
				if (b7 == 1)
				{
					bool forced = true;
					if (WorldGen.CheckTileBreakability2_ShouldTileSurvive(num186, num187))
					{
						flag11 = true;
						forced = false;
					}
					WorldGen.PlaceTile(num186, num187, num188, mute: false, forced, -1, num189);
				}
				if (b7 == 2)
				{
					WorldGen.KillWall(num186, num187, flag10);
				}
				if (b7 == 3)
				{
					WorldGen.PlaceWall(num186, num187, num188);
				}
				if (b7 == 4)
				{
					WorldGen.KillTile(num186, num187, flag10, effectOnly: false, noItem: true);
				}
				if (b7 == 5)
				{
					WorldGen.PlaceWire(num186, num187);
				}
				if (b7 == 6)
				{
					WorldGen.KillWire(num186, num187);
				}
				if (b7 == 7)
				{
					WorldGen.PoundTile(num186, num187);
				}
				if (b7 == 8)
				{
					WorldGen.PlaceActuator(num186, num187);
				}
				if (b7 == 9)
				{
					WorldGen.KillActuator(num186, num187);
				}
				if (b7 == 10)
				{
					WorldGen.PlaceWire2(num186, num187);
				}
				if (b7 == 11)
				{
					WorldGen.KillWire2(num186, num187);
				}
				if (b7 == 12)
				{
					WorldGen.PlaceWire3(num186, num187);
				}
				if (b7 == 13)
				{
					WorldGen.KillWire3(num186, num187);
				}
				if (b7 == 14)
				{
					WorldGen.SlopeTile(num186, num187, num188);
				}
				if (b7 == 15)
				{
					Minecart.FrameTrack(num186, num187, pound: true);
				}
				if (b7 == 16)
				{
					WorldGen.PlaceWire4(num186, num187);
				}
				if (b7 == 17)
				{
					WorldGen.KillWire4(num186, num187);
				}
				switch (b7)
				{
				case 18:
					Wiring.SetCurrentUser(whoAmI);
					Wiring.PokeLogicGate(num186, num187);
					Wiring.SetCurrentUser();
					return;
				case 19:
					Wiring.SetCurrentUser(whoAmI);
					Wiring.Actuate(num186, num187);
					Wiring.SetCurrentUser();
					return;
				case 20:
					if (WorldGen.InWorld(num186, num187, 2))
					{
						int type12 = Main.tile[num186, num187].type;
						WorldGen.KillTile(num186, num187, flag10);
						num188 = (short)((Main.tile[num186, num187].active() && Main.tile[num186, num187].type == type12) ? 1 : 0);
						if (Main.netMode == 2)
						{
							NetMessage.TrySendData(17, -1, -1, null, b7, num186, num187, num188, num189);
						}
					}
					return;
				case 21:
					WorldGen.ReplaceTile(num186, num187, (ushort)num188, num189);
					break;
				}
				if (b7 == 22)
				{
					WorldGen.ReplaceWall(num186, num187, (ushort)num188);
				}
				if (b7 == 23)
				{
					WorldGen.SlopeTile(num186, num187, num188);
					WorldGen.PoundTile(num186, num187);
				}
				if (Main.netMode != 2)
				{
					break;
				}
				if (flag11)
				{
					NetMessage.SendTileSquare(-1, num186, num187, 5);
					break;
				}
				NetMessage.TrySendData(17, -1, whoAmI, null, b7, num186, num187, num188, num189);
				if ((b7 == 1 || b7 == 21) && TileID.Sets.Falling[num188])
				{
					NetMessage.SendTileSquare(-1, num186, num187);
				}
				break;
			}
			case 18:
				if (Main.netMode == 1)
				{
					Main.dayTime = reader.ReadByte() == 1;
					Main.time = reader.ReadInt32();
					Main.sunModY = reader.ReadInt16();
					Main.moonModY = reader.ReadInt16();
				}
				break;
			case 19:
			{
				byte b11 = reader.ReadByte();
				int num209 = reader.ReadInt16();
				int num210 = reader.ReadInt16();
				if (WorldGen.InWorld(num209, num210, 3))
				{
					int num211 = ((reader.ReadByte() != 0) ? 1 : (-1));
					switch (b11)
					{
					case 0:
						WorldGen.OpenDoor(num209, num210, num211);
						break;
					case 1:
						WorldGen.CloseDoor(num209, num210, forced: true);
						break;
					case 2:
						WorldGen.ShiftTrapdoor(num209, num210, num211 == 1, 1);
						break;
					case 3:
						WorldGen.ShiftTrapdoor(num209, num210, num211 == 1, 0);
						break;
					case 4:
						WorldGen.ShiftTallGate(num209, num210, closing: false, forced: true);
						break;
					case 5:
						WorldGen.ShiftTallGate(num209, num210, closing: true, forced: true);
						break;
					}
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(19, -1, whoAmI, null, b11, num209, num210, (num211 == 1) ? 1 : 0);
					}
				}
				break;
			}
			case 20:
			{
				int num237 = reader.ReadInt16();
				int num238 = reader.ReadInt16();
				ushort num239 = reader.ReadByte();
				ushort num240 = reader.ReadByte();
				byte b13 = reader.ReadByte();
				if (!WorldGen.InWorld(num237, num238, 3))
				{
					break;
				}
				TileChangeType type14 = TileChangeType.None;
				if (Enum.IsDefined(typeof(TileChangeType), b13))
				{
					type14 = (TileChangeType)b13;
				}
				if (MessageBuffer.OnTileChangeReceived != null)
				{
					MessageBuffer.OnTileChangeReceived(num237, num238, Math.Max(num239, num240), type14);
				}
				BitsByte bitsByte29 = (byte)0;
				BitsByte bitsByte30 = (byte)0;
				BitsByte bitsByte31 = (byte)0;
				Tile tile4 = null;
				for (int num241 = num237; num241 < num237 + num239; num241++)
				{
					for (int num242 = num238; num242 < num238 + num240; num242++)
					{
						if (Main.tile[num241, num242] == null)
						{
							Main.tile[num241, num242] = new Tile();
						}
						tile4 = Main.tile[num241, num242];
						bool flag13 = tile4.active();
						bitsByte29 = reader.ReadByte();
						bitsByte30 = reader.ReadByte();
						bitsByte31 = reader.ReadByte();
						tile4.active(bitsByte29[0]);
						tile4.wall = (byte)(bitsByte29[2] ? 1u : 0u);
						bool flag14 = bitsByte29[3];
						if (Main.netMode != 2)
						{
							tile4.liquid = (byte)(flag14 ? 1u : 0u);
						}
						tile4.wire(bitsByte29[4]);
						tile4.halfBrick(bitsByte29[5]);
						tile4.actuator(bitsByte29[6]);
						tile4.inActive(bitsByte29[7]);
						tile4.wire2(bitsByte30[0]);
						tile4.wire3(bitsByte30[1]);
						if (bitsByte30[2])
						{
							tile4.color(reader.ReadByte());
						}
						if (bitsByte30[3])
						{
							tile4.wallColor(reader.ReadByte());
						}
						if (tile4.active())
						{
							int type15 = tile4.type;
							tile4.type = reader.ReadUInt16();
							if (Main.tileFrameImportant[tile4.type])
							{
								tile4.frameX = reader.ReadInt16();
								tile4.frameY = reader.ReadInt16();
							}
							else if (!flag13 || tile4.type != type15)
							{
								tile4.frameX = -1;
								tile4.frameY = -1;
							}
							byte b14 = 0;
							if (bitsByte30[4])
							{
								b14 = (byte)(b14 + 1);
							}
							if (bitsByte30[5])
							{
								b14 = (byte)(b14 + 2);
							}
							if (bitsByte30[6])
							{
								b14 = (byte)(b14 + 4);
							}
							tile4.slope(b14);
						}
						tile4.wire4(bitsByte30[7]);
						tile4.fullbrightBlock(bitsByte31[0]);
						tile4.fullbrightWall(bitsByte31[1]);
						tile4.invisibleBlock(bitsByte31[2]);
						tile4.invisibleWall(bitsByte31[3]);
						if (tile4.wall > 0)
						{
							tile4.wall = reader.ReadUInt16();
						}
						if (flag14)
						{
							tile4.liquid = reader.ReadByte();
							tile4.liquidType(reader.ReadByte());
						}
					}
				}
				WorldGen.RangeFrame(num237, num238, num237 + num239, num238 + num240);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, whoAmI, null, num237, num238, (int)num239, (int)num240, b13);
				}
				break;
			}
			case 21:
			case 90:
			case 145:
			case 148:
			{
				int num91 = reader.ReadInt16();
				Vector2 position3 = reader.ReadVector2();
				Vector2 velocity3 = reader.ReadVector2();
				int stack3 = reader.ReadInt16();
				int prefixWeWant2 = reader.ReadByte();
				int num92 = reader.ReadByte();
				int num93 = reader.ReadInt16();
				bool shimmered = false;
				float shimmerTime = 0f;
				int timeLeftInWhichTheItemCannotBeTakenByEnemies = 0;
				if (b == 145)
				{
					shimmered = reader.ReadBoolean();
					shimmerTime = reader.ReadSingle();
				}
				if (b == 148)
				{
					timeLeftInWhichTheItemCannotBeTakenByEnemies = reader.ReadByte();
				}
				if (Main.netMode == 1)
				{
					if (num93 == 0)
					{
						Main.item[num91].active = false;
						break;
					}
					int num94 = num91;
					Item item2 = Main.item[num94];
					ItemSyncPersistentStats itemSyncPersistentStats = default(ItemSyncPersistentStats);
					itemSyncPersistentStats.CopyFrom(item2);
					bool newAndShiny = (item2.newAndShiny || item2.netID != num93) && ItemSlot.Options.HighlightNewItems && (num93 < 0 || num93 >= 5453 || !ItemID.Sets.NeverAppearsAsNewInInventory[num93]);
					item2.netDefaults(num93);
					item2.newAndShiny = newAndShiny;
					item2.Prefix(prefixWeWant2);
					item2.stack = stack3;
					item2.position = position3;
					item2.velocity = velocity3;
					item2.active = true;
					item2.shimmered = shimmered;
					item2.shimmerTime = shimmerTime;
					if (b == 90)
					{
						item2.instanced = true;
						item2.playerIndexTheItemIsReservedFor = Main.myPlayer;
						item2.keepTime = 600;
					}
					item2.timeLeftInWhichTheItemCannotBeTakenByEnemies = timeLeftInWhichTheItemCannotBeTakenByEnemies;
					item2.wet = Collision.WetCollision(item2.position, item2.width, item2.height);
					itemSyncPersistentStats.PasteInto(item2);
				}
				else
				{
					if (Main.timeItemSlotCannotBeReusedFor[num91] > 0)
					{
						break;
					}
					if (num93 == 0)
					{
						if (num91 < 400)
						{
							Main.item[num91].active = false;
							NetMessage.TrySendData(21, -1, -1, null, num91);
						}
						break;
					}
					bool flag5 = false;
					if (num91 == 400)
					{
						flag5 = true;
					}
					if (flag5)
					{
						Item item3 = new Item();
						item3.netDefaults(num93);
						num91 = Item.NewItem(new EntitySource_Sync(), (int)position3.X, (int)position3.Y, item3.width, item3.height, item3.type, stack3, noBroadcast: true);
					}
					Item item4 = Main.item[num91];
					item4.netDefaults(num93);
					item4.Prefix(prefixWeWant2);
					item4.stack = stack3;
					item4.position = position3;
					item4.velocity = velocity3;
					item4.active = true;
					item4.playerIndexTheItemIsReservedFor = Main.myPlayer;
					item4.timeLeftInWhichTheItemCannotBeTakenByEnemies = timeLeftInWhichTheItemCannotBeTakenByEnemies;
					if (b == 145)
					{
						item4.shimmered = shimmered;
						item4.shimmerTime = shimmerTime;
					}
					if (flag5)
					{
						NetMessage.TrySendData(b, -1, -1, null, num91);
						if (num92 == 0)
						{
							Main.item[num91].ownIgnore = whoAmI;
							Main.item[num91].ownTime = 100;
						}
						Main.item[num91].FindOwner(num91);
					}
					else
					{
						NetMessage.TrySendData(b, -1, whoAmI, null, num91);
					}
				}
				break;
			}
			case 22:
			{
				int num42 = reader.ReadInt16();
				int num43 = reader.ReadByte();
				if (Main.netMode != 2 || Main.item[num42].playerIndexTheItemIsReservedFor == whoAmI)
				{
					Main.item[num42].playerIndexTheItemIsReservedFor = num43;
					if (num43 == Main.myPlayer)
					{
						Main.item[num42].keepTime = 15;
					}
					else
					{
						Main.item[num42].keepTime = 0;
					}
					if (Main.netMode == 2)
					{
						Main.item[num42].playerIndexTheItemIsReservedFor = 255;
						Main.item[num42].keepTime = 15;
						NetMessage.TrySendData(22, -1, -1, null, num42);
					}
				}
				break;
			}
			case 23:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int num164 = reader.ReadInt16();
				Vector2 vector5 = reader.ReadVector2();
				Vector2 velocity5 = reader.ReadVector2();
				int num165 = reader.ReadUInt16();
				if (num165 == 65535)
				{
					num165 = 0;
				}
				BitsByte bitsByte15 = reader.ReadByte();
				BitsByte bitsByte16 = reader.ReadByte();
				float[] array4 = ReUseTemporaryNPCAI();
				for (int num166 = 0; num166 < NPC.maxAI; num166++)
				{
					if (bitsByte15[num166 + 2])
					{
						array4[num166] = reader.ReadSingle();
					}
					else
					{
						array4[num166] = 0f;
					}
				}
				int num167 = reader.ReadInt16();
				int? playerCountForMultiplayerDifficultyOverride = 1;
				if (bitsByte16[0])
				{
					playerCountForMultiplayerDifficultyOverride = reader.ReadByte();
				}
				float value8 = 1f;
				if (bitsByte16[2])
				{
					value8 = reader.ReadSingle();
				}
				int num168 = 0;
				if (!bitsByte15[7])
				{
					num168 = reader.ReadByte() switch
					{
						2 => reader.ReadInt16(), 
						4 => reader.ReadInt32(), 
						_ => reader.ReadSByte(), 
					};
				}
				int num169 = -1;
				NPC nPC4 = Main.npc[num164];
				if (nPC4.active && Main.multiplayerNPCSmoothingRange > 0 && Vector2.DistanceSquared(nPC4.position, vector5) < 640000f)
				{
					nPC4.netOffset += nPC4.position - vector5;
				}
				if (!nPC4.active || nPC4.netID != num167)
				{
					nPC4.netOffset *= 0f;
					if (nPC4.active)
					{
						num169 = nPC4.type;
					}
					nPC4.active = true;
					nPC4.SetDefaults(num167, new NPCSpawnParams
					{
						playerCountForMultiplayerDifficultyOverride = playerCountForMultiplayerDifficultyOverride,
						strengthMultiplierOverride = value8
					});
				}
				nPC4.position = vector5;
				nPC4.velocity = velocity5;
				nPC4.target = num165;
				nPC4.direction = (bitsByte15[0] ? 1 : (-1));
				nPC4.directionY = (bitsByte15[1] ? 1 : (-1));
				nPC4.spriteDirection = (bitsByte15[6] ? 1 : (-1));
				if (bitsByte15[7])
				{
					num168 = (nPC4.life = nPC4.lifeMax);
				}
				else
				{
					nPC4.life = num168;
				}
				if (num168 <= 0)
				{
					nPC4.active = false;
				}
				nPC4.SpawnedFromStatue = bitsByte16[1];
				if (nPC4.SpawnedFromStatue)
				{
					nPC4.value = 0f;
				}
				for (int num170 = 0; num170 < NPC.maxAI; num170++)
				{
					nPC4.ai[num170] = array4[num170];
				}
				if (num169 > -1 && num169 != nPC4.type)
				{
					nPC4.TransformVisuals(num169, nPC4.type);
				}
				if (num167 == 262)
				{
					NPC.plantBoss = num164;
				}
				if (num167 == 245)
				{
					NPC.golemBoss = num164;
				}
				if (num167 == 668)
				{
					NPC.deerclopsBoss = num164;
				}
				if (nPC4.type >= 0 && nPC4.type < 688 && Main.npcCatchable[nPC4.type])
				{
					nPC4.releaseOwner = reader.ReadByte();
				}
				break;
			}
			case 24:
			{
				int num106 = reader.ReadInt16();
				int num107 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num107 = whoAmI;
				}
				Player player11 = Main.player[num107];
				Main.npc[num106].StrikeNPC(player11.inventory[player11.selectedItem].damage, player11.inventory[player11.selectedItem].knockBack, player11.direction);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(24, -1, whoAmI, null, num106, num107);
					NetMessage.TrySendData(23, -1, -1, null, num106);
				}
				break;
			}
			case 27:
			{
				int num34 = reader.ReadInt16();
				Vector2 position = reader.ReadVector2();
				Vector2 velocity2 = reader.ReadVector2();
				int num35 = reader.ReadByte();
				int num36 = reader.ReadInt16();
				BitsByte bitsByte2 = reader.ReadByte();
				BitsByte bitsByte3 = (byte)(bitsByte2[2] ? reader.ReadByte() : 0);
				float[] array = ReUseTemporaryProjectileAI();
				array[0] = (bitsByte2[0] ? reader.ReadSingle() : 0f);
				array[1] = (bitsByte2[1] ? reader.ReadSingle() : 0f);
				int bannerIdToRespondTo = (bitsByte2[3] ? reader.ReadUInt16() : 0);
				int damage2 = (bitsByte2[4] ? reader.ReadInt16() : 0);
				float knockBack2 = (bitsByte2[5] ? reader.ReadSingle() : 0f);
				int originalDamage = (bitsByte2[6] ? reader.ReadInt16() : 0);
				int num37 = (bitsByte2[7] ? reader.ReadInt16() : (-1));
				if (num37 >= 1000)
				{
					num37 = -1;
				}
				array[2] = (bitsByte3[0] ? reader.ReadSingle() : 0f);
				if (Main.netMode == 2)
				{
					if (num36 == 949)
					{
						num35 = 255;
					}
					else
					{
						num35 = whoAmI;
						if (Main.projHostile[num36])
						{
							break;
						}
					}
				}
				int num38 = 1000;
				for (int n = 0; n < 1000; n++)
				{
					if (Main.projectile[n].owner == num35 && Main.projectile[n].identity == num34 && Main.projectile[n].active)
					{
						num38 = n;
						break;
					}
				}
				if (num38 == 1000)
				{
					for (int num39 = 0; num39 < 1000; num39++)
					{
						if (!Main.projectile[num39].active)
						{
							num38 = num39;
							break;
						}
					}
				}
				if (num38 == 1000)
				{
					num38 = Projectile.FindOldestProjectile();
				}
				Projectile projectile = Main.projectile[num38];
				if (!projectile.active || projectile.type != num36)
				{
					projectile.SetDefaults(num36);
					if (Main.netMode == 2)
					{
						Netplay.Clients[whoAmI].SpamProjectile += 1f;
					}
				}
				projectile.identity = num34;
				projectile.position = position;
				projectile.velocity = velocity2;
				projectile.type = num36;
				projectile.damage = damage2;
				projectile.bannerIdToRespondTo = bannerIdToRespondTo;
				projectile.originalDamage = originalDamage;
				projectile.knockBack = knockBack2;
				projectile.owner = num35;
				for (int num40 = 0; num40 < Projectile.maxAI; num40++)
				{
					projectile.ai[num40] = array[num40];
				}
				if (num37 >= 0)
				{
					projectile.projUUID = num37;
					Main.projectileIdentity[num35, num37] = num38;
				}
				projectile.ProjectileFixDesperation();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(27, -1, whoAmI, null, num38);
				}
				break;
			}
			case 28:
			{
				int num200 = reader.ReadInt16();
				int num201 = reader.ReadInt16();
				float num202 = reader.ReadSingle();
				int num203 = reader.ReadByte() - 1;
				byte b10 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					if (num201 < 0)
					{
						num201 = 0;
					}
					Main.npc[num200].PlayerInteraction(whoAmI);
				}
				if (num201 >= 0)
				{
					Main.npc[num200].StrikeNPC(num201, num202, num203, b10 == 1, noEffect: false, fromNet: true);
				}
				else
				{
					Main.npc[num200].life = 0;
					Main.npc[num200].HitEffect();
					Main.npc[num200].active = false;
				}
				if (Main.netMode != 2)
				{
					break;
				}
				NetMessage.TrySendData(28, -1, whoAmI, null, num200, num201, num202, num203, b10);
				if (Main.npc[num200].life <= 0)
				{
					NetMessage.TrySendData(23, -1, -1, null, num200);
				}
				else
				{
					Main.npc[num200].netUpdate = true;
				}
				if (Main.npc[num200].realLife >= 0)
				{
					if (Main.npc[Main.npc[num200].realLife].life <= 0)
					{
						NetMessage.TrySendData(23, -1, -1, null, Main.npc[num200].realLife);
					}
					else
					{
						Main.npc[Main.npc[num200].realLife].netUpdate = true;
					}
				}
				break;
			}
			case 29:
			{
				int num84 = reader.ReadInt16();
				int num85 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num85 = whoAmI;
				}
				for (int num86 = 0; num86 < 1000; num86++)
				{
					if (Main.projectile[num86].owner == num85 && Main.projectile[num86].identity == num84 && Main.projectile[num86].active)
					{
						Main.projectile[num86].Kill();
						break;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(29, -1, whoAmI, null, num84, num85);
				}
				break;
			}
			case 30:
			{
				int num124 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num124 = whoAmI;
				}
				bool flag7 = reader.ReadBoolean();
				Main.player[num124].hostile = flag7;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(30, -1, whoAmI, null, num124);
					LocalizedText obj4 = (flag7 ? Lang.mp[11] : Lang.mp[12]);
					ChatHelper.BroadcastChatMessage(color: Main.teamColor[Main.player[num124].team], text: NetworkText.FromKey(obj4.Key, Main.player[num124].name));
				}
				break;
			}
			case 31:
			{
				if (Main.netMode != 2)
				{
					break;
				}
				int num228 = reader.ReadInt16();
				int num229 = reader.ReadInt16();
				int num230 = Chest.FindChest(num228, num229);
				if (num230 > -1 && Chest.UsingChest(num230) == -1)
				{
					for (int num231 = 0; num231 < 40; num231++)
					{
						NetMessage.TrySendData(32, whoAmI, -1, null, num230, num231);
					}
					NetMessage.TrySendData(33, whoAmI, -1, null, num230);
					Main.player[whoAmI].chest = num230;
					if (Main.myPlayer == whoAmI)
					{
						Main.recBigList = false;
					}
					NetMessage.TrySendData(80, -1, whoAmI, null, whoAmI, num230);
					if (Main.netMode == 2 && WorldGen.IsChestRigged(num228, num229))
					{
						Wiring.SetCurrentUser(whoAmI);
						Wiring.HitSwitch(num228, num229);
						Wiring.SetCurrentUser();
						NetMessage.TrySendData(59, -1, whoAmI, null, num228, num229);
					}
				}
				break;
			}
			case 32:
			{
				int num174 = reader.ReadInt16();
				int num175 = reader.ReadByte();
				int stack7 = reader.ReadInt16();
				int prefixWeWant3 = reader.ReadByte();
				int type10 = reader.ReadInt16();
				if (num174 >= 0 && num174 < 8000)
				{
					if (Main.chest[num174] == null)
					{
						Main.chest[num174] = new Chest();
					}
					if (Main.chest[num174].item[num175] == null)
					{
						Main.chest[num174].item[num175] = new Item();
					}
					Main.chest[num174].item[num175].netDefaults(type10);
					Main.chest[num174].item[num175].Prefix(prefixWeWant3);
					Main.chest[num174].item[num175].stack = stack7;
					Recipe.FindRecipes(canDelayCheck: true);
				}
				break;
			}
			case 33:
			{
				int num2 = reader.ReadInt16();
				int num3 = reader.ReadInt16();
				int num4 = reader.ReadInt16();
				int num5 = reader.ReadByte();
				string name = string.Empty;
				if (num5 != 0)
				{
					if (num5 <= 20)
					{
						name = reader.ReadString();
					}
					else if (num5 != 255)
					{
						num5 = 0;
					}
				}
				if (Main.netMode == 1)
				{
					Player player = Main.player[Main.myPlayer];
					if (player.chest == -1)
					{
						Main.playerInventory = true;
						SoundEngine.PlaySound(10);
					}
					else if (player.chest != num2 && num2 != -1)
					{
						Main.playerInventory = true;
						SoundEngine.PlaySound(12);
						Main.recBigList = false;
					}
					else if (player.chest != -1 && num2 == -1)
					{
						SoundEngine.PlaySound(11);
						Main.recBigList = false;
					}
					player.chest = num2;
					player.chestX = num3;
					player.chestY = num4;
					Recipe.FindRecipes(canDelayCheck: true);
					if (Main.tile[num3, num4].frameX >= 36 && Main.tile[num3, num4].frameX < 72)
					{
						AchievementsHelper.HandleSpecialEvent(Main.player[Main.myPlayer], 16);
					}
				}
				else
				{
					if (num5 != 0)
					{
						int chest = Main.player[whoAmI].chest;
						Chest chest2 = Main.chest[chest];
						chest2.name = name;
						NetMessage.TrySendData(69, -1, whoAmI, null, chest, chest2.x, chest2.y);
					}
					Main.player[whoAmI].chest = num2;
					Recipe.FindRecipes(canDelayCheck: true);
					NetMessage.TrySendData(80, -1, whoAmI, null, whoAmI, num2);
				}
				break;
			}
			case 34:
			{
				byte b12 = reader.ReadByte();
				int num213 = reader.ReadInt16();
				int num214 = reader.ReadInt16();
				int num215 = reader.ReadInt16();
				int num216 = reader.ReadInt16();
				if (Main.netMode == 2)
				{
					num216 = 0;
				}
				if (Main.netMode == 2)
				{
					switch (b12)
					{
					case 0:
					{
						int num219 = WorldGen.PlaceChest(num213, num214, 21, notNearOtherChests: false, num215);
						if (num219 == -1)
						{
							NetMessage.TrySendData(34, whoAmI, -1, null, b12, num213, num214, num215, num219);
							Item.NewItem(new EntitySource_TileBreak(num213, num214), num213 * 16, num214 * 16, 32, 32, Chest.chestItemSpawn[num215], 1, noBroadcast: true);
						}
						else
						{
							NetMessage.TrySendData(34, -1, -1, null, b12, num213, num214, num215, num219);
						}
						break;
					}
					case 1:
						if (Main.tile[num213, num214].type == 21)
						{
							Tile tile = Main.tile[num213, num214];
							if (tile.frameX % 36 != 0)
							{
								num213--;
							}
							if (tile.frameY % 36 != 0)
							{
								num214--;
							}
							int number = Chest.FindChest(num213, num214);
							WorldGen.KillTile(num213, num214);
							if (!tile.active())
							{
								NetMessage.TrySendData(34, -1, -1, null, b12, num213, num214, 0f, number);
							}
							break;
						}
						goto default;
					default:
						switch (b12)
						{
						case 2:
						{
							int num217 = WorldGen.PlaceChest(num213, num214, 88, notNearOtherChests: false, num215);
							if (num217 == -1)
							{
								NetMessage.TrySendData(34, whoAmI, -1, null, b12, num213, num214, num215, num217);
								Item.NewItem(new EntitySource_TileBreak(num213, num214), num213 * 16, num214 * 16, 32, 32, Chest.dresserItemSpawn[num215], 1, noBroadcast: true);
							}
							else
							{
								NetMessage.TrySendData(34, -1, -1, null, b12, num213, num214, num215, num217);
							}
							break;
						}
						case 3:
							if (Main.tile[num213, num214].type == 88)
							{
								Tile tile2 = Main.tile[num213, num214];
								num213 -= tile2.frameX % 54 / 18;
								if (tile2.frameY % 36 != 0)
								{
									num214--;
								}
								int number2 = Chest.FindChest(num213, num214);
								WorldGen.KillTile(num213, num214);
								if (!tile2.active())
								{
									NetMessage.TrySendData(34, -1, -1, null, b12, num213, num214, 0f, number2);
								}
								break;
							}
							goto default;
						default:
							switch (b12)
							{
							case 4:
							{
								int num218 = WorldGen.PlaceChest(num213, num214, 467, notNearOtherChests: false, num215);
								if (num218 == -1)
								{
									NetMessage.TrySendData(34, whoAmI, -1, null, b12, num213, num214, num215, num218);
									Item.NewItem(new EntitySource_TileBreak(num213, num214), num213 * 16, num214 * 16, 32, 32, Chest.chestItemSpawn2[num215], 1, noBroadcast: true);
								}
								else
								{
									NetMessage.TrySendData(34, -1, -1, null, b12, num213, num214, num215, num218);
								}
								break;
							}
							case 5:
								if (Main.tile[num213, num214].type == 467)
								{
									Tile tile3 = Main.tile[num213, num214];
									if (tile3.frameX % 36 != 0)
									{
										num213--;
									}
									if (tile3.frameY % 36 != 0)
									{
										num214--;
									}
									int number3 = Chest.FindChest(num213, num214);
									WorldGen.KillTile(num213, num214);
									if (!tile3.active())
									{
										NetMessage.TrySendData(34, -1, -1, null, b12, num213, num214, 0f, number3);
									}
								}
								break;
							}
							break;
						}
						break;
					}
					break;
				}
				switch (b12)
				{
				case 0:
					if (num216 == -1)
					{
						WorldGen.KillTile(num213, num214);
						break;
					}
					SoundEngine.PlaySound(0, num213 * 16, num214 * 16);
					WorldGen.PlaceChestDirect(num213, num214, 21, num215, num216);
					break;
				case 2:
					if (num216 == -1)
					{
						WorldGen.KillTile(num213, num214);
						break;
					}
					SoundEngine.PlaySound(0, num213 * 16, num214 * 16);
					WorldGen.PlaceDresserDirect(num213, num214, 88, num215, num216);
					break;
				case 4:
					if (num216 == -1)
					{
						WorldGen.KillTile(num213, num214);
						break;
					}
					SoundEngine.PlaySound(0, num213 * 16, num214 * 16);
					WorldGen.PlaceChestDirect(num213, num214, 467, num215, num216);
					break;
				default:
					Chest.DestroyChestDirect(num213, num214, num216);
					WorldGen.KillTile(num213, num214);
					break;
				}
				break;
			}
			case 35:
			{
				int num155 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num155 = whoAmI;
				}
				int num156 = reader.ReadInt16();
				if (num155 != Main.myPlayer || Main.ServerSideCharacter)
				{
					Main.player[num155].HealEffect(num156);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(35, -1, whoAmI, null, num155, num156);
				}
				break;
			}
			case 36:
			{
				int num87 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num87 = whoAmI;
				}
				Player player9 = Main.player[num87];
				bool flag4 = player9.zone5[0];
				player9.zone1 = reader.ReadByte();
				player9.zone2 = reader.ReadByte();
				player9.zone3 = reader.ReadByte();
				player9.zone4 = reader.ReadByte();
				player9.zone5 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					if (!flag4 && player9.zone5[0])
					{
						NPC.SpawnFaelings(num87);
					}
					NetMessage.TrySendData(36, -1, whoAmI, null, num87);
				}
				break;
			}
			case 37:
				if (Main.netMode == 1)
				{
					if (Main.autoPass)
					{
						NetMessage.TrySendData(38);
						Main.autoPass = false;
					}
					else
					{
						Netplay.ServerPassword = "";
						Main.menuMode = 31;
					}
				}
				break;
			case 38:
				if (Main.netMode == 2)
				{
					if (reader.ReadString() == Netplay.ServerPassword)
					{
						Netplay.Clients[whoAmI].State = 1;
						NetMessage.TrySendData(3, whoAmI);
					}
					else
					{
						NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[1].ToNetworkText());
					}
				}
				break;
			case 39:
				if (Main.netMode == 1)
				{
					int num23 = reader.ReadInt16();
					Main.item[num23].playerIndexTheItemIsReservedFor = 255;
					NetMessage.TrySendData(22, -1, -1, null, num23);
				}
				break;
			case 40:
			{
				int num262 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num262 = whoAmI;
				}
				int npcIndex = reader.ReadInt16();
				Main.player[num262].SetTalkNPC(npcIndex, fromNet: true);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(40, -1, whoAmI, null, num262);
				}
				break;
			}
			case 41:
			{
				int num235 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num235 = whoAmI;
				}
				Player player15 = Main.player[num235];
				float itemRotation = reader.ReadSingle();
				int itemAnimation = reader.ReadInt16();
				player15.itemRotation = itemRotation;
				player15.itemAnimation = itemAnimation;
				player15.channel = player15.inventory[player15.selectedItem].channel;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(41, -1, whoAmI, null, num235);
				}
				if (Main.netMode == 1)
				{
					Item item6 = player15.inventory[player15.selectedItem];
					if (item6.UseSound != null)
					{
						SoundEngine.PlaySound(item6.UseSound, player15.Center);
					}
				}
				break;
			}
			case 42:
			{
				int num212 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num212 = whoAmI;
				}
				else if (Main.myPlayer == num212 && !Main.ServerSideCharacter)
				{
					break;
				}
				int statMana = reader.ReadInt16();
				int statManaMax = reader.ReadInt16();
				Main.player[num212].statMana = statMana;
				Main.player[num212].statManaMax = statManaMax;
				break;
			}
			case 43:
			{
				int num160 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num160 = whoAmI;
				}
				int num161 = reader.ReadInt16();
				if (num160 != Main.myPlayer)
				{
					Main.player[num160].ManaEffect(num161);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(43, -1, whoAmI, null, num160, num161);
				}
				break;
			}
			case 45:
			{
				int num103 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num103 = whoAmI;
				}
				int num104 = reader.ReadByte();
				Player player10 = Main.player[num103];
				int team = player10.team;
				player10.team = num104;
				Color color = Main.teamColor[num104];
				if (Main.netMode != 2)
				{
					break;
				}
				NetMessage.TrySendData(45, -1, whoAmI, null, num103);
				LocalizedText localizedText = Lang.mp[13 + num104];
				if (num104 == 5)
				{
					localizedText = Lang.mp[22];
				}
				for (int num105 = 0; num105 < 255; num105++)
				{
					if (num105 == whoAmI || (team > 0 && Main.player[num105].team == team) || (num104 > 0 && Main.player[num105].team == num104))
					{
						ChatHelper.SendChatMessageToClient(NetworkText.FromKey(localizedText.Key, player10.name), color, num105);
					}
				}
				break;
			}
			case 46:
				if (Main.netMode == 2)
				{
					short i3 = reader.ReadInt16();
					int j3 = reader.ReadInt16();
					int num97 = Sign.ReadSign(i3, j3);
					if (num97 >= 0)
					{
						NetMessage.TrySendData(47, whoAmI, -1, null, num97, whoAmI);
					}
				}
				break;
			case 47:
			{
				int num8 = reader.ReadInt16();
				int x = reader.ReadInt16();
				int y = reader.ReadInt16();
				string text = reader.ReadString();
				int num9 = reader.ReadByte();
				BitsByte bitsByte = reader.ReadByte();
				if (num8 >= 0 && num8 < 1000)
				{
					string text2 = null;
					if (Main.sign[num8] != null)
					{
						text2 = Main.sign[num8].text;
					}
					Main.sign[num8] = new Sign();
					Main.sign[num8].x = x;
					Main.sign[num8].y = y;
					Sign.TextSign(num8, text);
					if (Main.netMode == 2 && text2 != text)
					{
						num9 = whoAmI;
						NetMessage.TrySendData(47, -1, whoAmI, null, num8, num9);
					}
					if (Main.netMode == 1 && num9 == Main.myPlayer && Main.sign[num8] != null && !bitsByte[0])
					{
						Main.playerInventory = false;
						Main.player[Main.myPlayer].SetTalkNPC(-1, fromNet: true);
						Main.npcChatCornerItem = 0;
						Main.editSign = false;
						SoundEngine.PlaySound(10);
						Main.player[Main.myPlayer].sign = num8;
						Main.npcChatText = Main.sign[num8].text;
					}
				}
				break;
			}
			case 48:
			{
				int num246 = reader.ReadInt16();
				int num247 = reader.ReadInt16();
				byte b15 = reader.ReadByte();
				byte liquidType = reader.ReadByte();
				if (Main.netMode == 2 && Netplay.SpamCheck)
				{
					int num248 = whoAmI;
					int num249 = (int)(Main.player[num248].position.X + (float)(Main.player[num248].width / 2));
					int num250 = (int)(Main.player[num248].position.Y + (float)(Main.player[num248].height / 2));
					int num251 = 10;
					int num252 = num249 - num251;
					int num253 = num249 + num251;
					int num254 = num250 - num251;
					int num255 = num250 + num251;
					if (num246 < num252 || num246 > num253 || num247 < num254 || num247 > num255)
					{
						Netplay.Clients[whoAmI].SpamWater += 1f;
					}
				}
				if (Main.tile[num246, num247] == null)
				{
					Main.tile[num246, num247] = new Tile();
				}
				lock (Main.tile[num246, num247])
				{
					Main.tile[num246, num247].liquid = b15;
					Main.tile[num246, num247].liquidType(liquidType);
					if (Main.netMode == 2)
					{
						WorldGen.SquareTileFrame(num246, num247);
						if (b15 == 0)
						{
							NetMessage.SendData(48, -1, whoAmI, null, num246, num247);
						}
					}
					break;
				}
			}
			case 49:
				if (Netplay.Connection.State == 6)
				{
					Netplay.Connection.State = 10;
					Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
				}
				break;
			case 50:
			{
				int num190 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num190 = whoAmI;
				}
				else if (num190 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					break;
				}
				Player player13 = Main.player[num190];
				for (int num191 = 0; num191 < 44; num191++)
				{
					player13.buffType[num191] = reader.ReadUInt16();
					if (player13.buffType[num191] > 0)
					{
						player13.buffTime[num191] = 60;
					}
					else
					{
						player13.buffTime[num191] = 0;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(50, -1, whoAmI, null, num190);
				}
				break;
			}
			case 51:
			{
				byte b8 = reader.ReadByte();
				byte b9 = reader.ReadByte();
				switch (b9)
				{
				case 1:
					NPC.SpawnSkeletron(b8);
					break;
				case 2:
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(51, -1, whoAmI, null, b8, (int)b9);
					}
					else
					{
						SoundEngine.PlaySound(SoundID.Item1, (int)Main.player[b8].position.X, (int)Main.player[b8].position.Y);
					}
					break;
				case 3:
					if (Main.netMode == 2)
					{
						Main.Sundialing();
					}
					break;
				case 4:
					Main.npc[b8].BigMimicSpawnSmoke();
					break;
				case 5:
					if (Main.netMode == 2)
					{
						NPC nPC5 = new NPC();
						nPC5.SetDefaults(664);
						Main.BestiaryTracker.Kills.RegisterKill(nPC5);
					}
					break;
				case 6:
					if (Main.netMode == 2)
					{
						Main.Moondialing();
					}
					break;
				}
				break;
			}
			case 52:
			{
				int num157 = reader.ReadByte();
				int num158 = reader.ReadInt16();
				int num159 = reader.ReadInt16();
				if (num157 == 1)
				{
					Chest.Unlock(num158, num159);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(52, -1, whoAmI, null, 0, num157, num158, num159);
						NetMessage.SendTileSquare(-1, num158, num159, 2);
					}
				}
				if (num157 == 2)
				{
					WorldGen.UnlockDoor(num158, num159);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(52, -1, whoAmI, null, 0, num157, num158, num159);
						NetMessage.SendTileSquare(-1, num158, num159, 2);
					}
				}
				if (num157 == 3)
				{
					Chest.Lock(num158, num159);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(52, -1, whoAmI, null, 0, num157, num158, num159);
						NetMessage.SendTileSquare(-1, num158, num159, 2);
					}
				}
				break;
			}
			case 53:
			{
				int num108 = reader.ReadInt16();
				int type6 = reader.ReadUInt16();
				int time2 = reader.ReadInt16();
				Main.npc[num108].AddBuff(type6, time2, quiet: true);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(54, -1, -1, null, num108);
				}
				break;
			}
			case 54:
				if (Main.netMode == 1)
				{
					int num95 = reader.ReadInt16();
					NPC nPC3 = Main.npc[num95];
					for (int num96 = 0; num96 < 20; num96++)
					{
						nPC3.buffType[num96] = reader.ReadUInt16();
						nPC3.buffTime[num96] = reader.ReadInt16();
					}
				}
				break;
			case 55:
			{
				int num50 = reader.ReadByte();
				int num51 = reader.ReadUInt16();
				int num52 = reader.ReadInt32();
				if (Main.netMode != 2 || num50 == whoAmI || Main.pvpBuff[num51])
				{
					if (Main.netMode == 1 && num50 == Main.myPlayer)
					{
						Main.player[num50].AddBuff(num51, num52);
					}
					else if (Main.netMode == 2)
					{
						NetMessage.TrySendData(55, -1, -1, null, num50, num51, num52);
					}
				}
				break;
			}
			case 56:
			{
				int num27 = reader.ReadInt16();
				if (num27 >= 0 && num27 < 200)
				{
					if (Main.netMode == 1)
					{
						string givenName = reader.ReadString();
						Main.npc[num27].GivenName = givenName;
						int townNpcVariationIndex = reader.ReadInt32();
						Main.npc[num27].townNpcVariationIndex = townNpcVariationIndex;
					}
					else if (Main.netMode == 2)
					{
						NetMessage.TrySendData(56, whoAmI, -1, null, num27);
					}
				}
				break;
			}
			case 57:
				if (Main.netMode == 1)
				{
					WorldGen.tGood = reader.ReadByte();
					WorldGen.tEvil = reader.ReadByte();
					WorldGen.tBlood = reader.ReadByte();
				}
				break;
			case 58:
			{
				int num263 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num263 = whoAmI;
				}
				float num264 = reader.ReadSingle();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(58, -1, whoAmI, null, whoAmI, num264);
					break;
				}
				Player player16 = Main.player[num263];
				int type16 = player16.inventory[player16.selectedItem].type;
				switch (type16)
				{
				case 4057:
				case 4372:
				case 4715:
					player16.PlayGuitarChord(num264);
					break;
				case 4673:
					player16.PlayDrums(num264);
					break;
				default:
				{
					Main.musicPitch = num264;
					LegacySoundStyle type17 = SoundID.Item26;
					if (type16 == 507)
					{
						type17 = SoundID.Item35;
					}
					if (type16 == 1305)
					{
						type17 = SoundID.Item47;
					}
					SoundEngine.PlaySound(type17, player16.position);
					break;
				}
				}
				break;
			}
			case 59:
			{
				int num10 = reader.ReadInt16();
				int num11 = reader.ReadInt16();
				Wiring.SetCurrentUser(whoAmI);
				Wiring.HitSwitch(num10, num11);
				Wiring.SetCurrentUser();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(59, -1, whoAmI, null, num10, num11);
				}
				break;
			}
			case 60:
			{
				int num259 = reader.ReadInt16();
				int num260 = reader.ReadInt16();
				int num261 = reader.ReadInt16();
				byte b16 = reader.ReadByte();
				if (num259 >= 200)
				{
					NetMessage.BootPlayer(whoAmI, NetworkText.FromKey("Net.CheatingInvalid"));
					break;
				}
				NPC nPC6 = Main.npc[num259];
				bool isLikeATownNPC = nPC6.isLikeATownNPC;
				if (Main.netMode == 1)
				{
					nPC6.homeless = b16 == 1;
					nPC6.homeTileX = num260;
					nPC6.homeTileY = num261;
				}
				if (!isLikeATownNPC)
				{
					break;
				}
				if (Main.netMode == 1)
				{
					switch (b16)
					{
					case 1:
						WorldGen.TownManager.KickOut(nPC6.type);
						break;
					case 2:
						WorldGen.TownManager.SetRoom(nPC6.type, num260, num261);
						break;
					}
				}
				else if (b16 == 1)
				{
					WorldGen.kickOut(num259);
				}
				else
				{
					WorldGen.moveRoom(num260, num261, num259);
				}
				break;
			}
			case 61:
			{
				int num224 = reader.ReadInt16();
				int num225 = reader.ReadInt16();
				if (Main.netMode != 2)
				{
					break;
				}
				if (num225 >= 0 && num225 < 688 && NPCID.Sets.MPAllowedEnemies[num225])
				{
					if (!NPC.AnyNPCs(num225))
					{
						NPC.SpawnOnPlayer(num224, num225);
					}
				}
				else if (num225 == -4)
				{
					if (!Main.dayTime && !DD2Event.Ongoing)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[31].Key), new Color(50, 255, 130));
						Main.startPumpkinMoon();
						NetMessage.TrySendData(7);
						NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 2f, 1f);
					}
				}
				else if (num225 == -5)
				{
					if (!Main.dayTime && !DD2Event.Ongoing)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[34].Key), new Color(50, 255, 130));
						Main.startSnowMoon();
						NetMessage.TrySendData(7);
						NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 1f, 1f);
					}
				}
				else if (num225 == -6)
				{
					if (Main.dayTime && !Main.eclipse)
					{
						if (Main.remixWorld)
						{
							ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[106].Key), new Color(50, 255, 130));
						}
						else
						{
							ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[20].Key), new Color(50, 255, 130));
						}
						Main.eclipse = true;
						NetMessage.TrySendData(7);
					}
				}
				else if (num225 == -7)
				{
					Main.invasionDelay = 0;
					Main.StartInvasion(4);
					NetMessage.TrySendData(7);
					NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
				}
				else if (num225 == -8)
				{
					if (NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger() && !NPC.AnyoneNearCultists())
					{
						WorldGen.StartImpendingDoom(720);
						NetMessage.TrySendData(7);
					}
				}
				else if (num225 == -10)
				{
					if (!Main.dayTime && !Main.bloodMoon)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[8].Key), new Color(50, 255, 130));
						Main.bloodMoon = true;
						if (Main.GetMoonPhase() == MoonPhase.Empty)
						{
							Main.moonPhase = 5;
						}
						AchievementsHelper.NotifyProgressionEvent(4);
						NetMessage.TrySendData(7);
					}
				}
				else if (num225 == -11)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookUsed"), new Color(50, 255, 130));
					NPC.combatBookWasUsed = true;
					NetMessage.TrySendData(7);
				}
				else if (num225 == -12)
				{
					NPC.UnlockOrExchangePet(ref NPC.boughtCat, 637, "Misc.LicenseCatUsed", num225);
				}
				else if (num225 == -13)
				{
					NPC.UnlockOrExchangePet(ref NPC.boughtDog, 638, "Misc.LicenseDogUsed", num225);
				}
				else if (num225 == -14)
				{
					NPC.UnlockOrExchangePet(ref NPC.boughtBunny, 656, "Misc.LicenseBunnyUsed", num225);
				}
				else if (num225 == -15)
				{
					NPC.UnlockOrExchangePet(ref NPC.unlockedSlimeBlueSpawn, 670, "Misc.LicenseSlimeUsed", num225);
				}
				else if (num225 == -16)
				{
					NPC.SpawnMechQueen(num224);
				}
				else if (num225 == -17)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookVolumeTwoUsed"), new Color(50, 255, 130));
					NPC.combatBookVolumeTwoWasUsed = true;
					NetMessage.TrySendData(7);
				}
				else if (num225 == -18)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PeddlersSatchelUsed"), new Color(50, 255, 130));
					NPC.peddlersSatchelWasUsed = true;
					NetMessage.TrySendData(7);
				}
				else if (num225 < 0)
				{
					int num226 = 1;
					if (num225 > -5)
					{
						num226 = -num225;
					}
					if (num226 > 0 && Main.invasionType == 0)
					{
						Main.invasionDelay = 0;
						Main.StartInvasion(num226);
					}
					NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
				}
				break;
			}
			case 62:
			{
				int num176 = reader.ReadByte();
				int num177 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num176 = whoAmI;
				}
				if (num177 == 1)
				{
					Main.player[num176].NinjaDodge();
				}
				if (num177 == 2)
				{
					Main.player[num176].ShadowDodge();
				}
				if (num177 == 4)
				{
					Main.player[num176].BrainOfConfusionDodge();
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(62, -1, whoAmI, null, num176, num177);
				}
				break;
			}
			case 63:
			{
				int num153 = reader.ReadInt16();
				int num154 = reader.ReadInt16();
				byte b5 = reader.ReadByte();
				byte b6 = reader.ReadByte();
				if (b6 == 0)
				{
					WorldGen.paintTile(num153, num154, b5);
				}
				else
				{
					WorldGen.paintCoatTile(num153, num154, b5);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(63, -1, whoAmI, null, num153, num154, (int)b5, (int)b6);
				}
				break;
			}
			case 64:
			{
				int num118 = reader.ReadInt16();
				int num119 = reader.ReadInt16();
				byte b3 = reader.ReadByte();
				byte b4 = reader.ReadByte();
				if (b4 == 0)
				{
					WorldGen.paintWall(num118, num119, b3);
				}
				else
				{
					WorldGen.paintCoatWall(num118, num119, b3);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(64, -1, whoAmI, null, num118, num119, (int)b3, (int)b4);
				}
				break;
			}
			case 65:
			{
				BitsByte bitsByte6 = reader.ReadByte();
				int num53 = reader.ReadInt16();
				if (Main.netMode == 2)
				{
					num53 = whoAmI;
				}
				Vector2 vector = reader.ReadVector2();
				int num54 = 0;
				num54 = reader.ReadByte();
				int num55 = 0;
				if (bitsByte6[0])
				{
					num55++;
				}
				if (bitsByte6[1])
				{
					num55 += 2;
				}
				bool flag2 = false;
				if (bitsByte6[2])
				{
					flag2 = true;
				}
				int num56 = 0;
				if (bitsByte6[3])
				{
					num56 = reader.ReadInt32();
				}
				if (flag2)
				{
					vector = Main.player[num53].position;
				}
				switch (num55)
				{
				case 0:
					Main.player[num53].Teleport(vector, num54, num56);
					break;
				case 1:
					Main.npc[num53].Teleport(vector, num54, num56);
					break;
				case 2:
				{
					Main.player[num53].Teleport(vector, num54, num56);
					if (Main.netMode != 2)
					{
						break;
					}
					RemoteClient.CheckSection(whoAmI, vector);
					NetMessage.TrySendData(65, -1, -1, null, 0, num53, vector.X, vector.Y, num54, flag2.ToInt(), num56);
					int num57 = -1;
					float num58 = 9999f;
					for (int num59 = 0; num59 < 255; num59++)
					{
						if (Main.player[num59].active && num59 != whoAmI)
						{
							Vector2 vector2 = Main.player[num59].position - Main.player[whoAmI].position;
							if (vector2.Length() < num58)
							{
								num58 = vector2.Length();
								num57 = num59;
							}
						}
					}
					if (num57 >= 0)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Game.HasTeleportedTo", Main.player[whoAmI].name, Main.player[num57].name), new Color(250, 250, 0));
					}
					break;
				}
				}
				if (Main.netMode == 2 && num55 == 0)
				{
					NetMessage.TrySendData(65, -1, whoAmI, null, num55, num53, vector.X, vector.Y, num54, flag2.ToInt(), num56);
				}
				break;
			}
			case 66:
			{
				int num30 = reader.ReadByte();
				int num31 = reader.ReadInt16();
				if (num31 > 0)
				{
					Player player3 = Main.player[num30];
					player3.statLife += num31;
					if (player3.statLife > player3.statLifeMax2)
					{
						player3.statLife = player3.statLifeMax2;
					}
					player3.HealEffect(num31, broadcast: false);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(66, -1, whoAmI, null, num30, num31);
					}
				}
				break;
			}
			case 68:
				reader.ReadString();
				break;
			case 69:
			{
				int num256 = reader.ReadInt16();
				int num257 = reader.ReadInt16();
				int num258 = reader.ReadInt16();
				if (Main.netMode == 1)
				{
					if (num256 >= 0 && num256 < 8000)
					{
						Chest chest3 = Main.chest[num256];
						if (chest3 == null)
						{
							chest3 = new Chest();
							chest3.x = num257;
							chest3.y = num258;
							Main.chest[num256] = chest3;
						}
						else if (chest3.x != num257 || chest3.y != num258)
						{
							break;
						}
						chest3.name = reader.ReadString();
					}
				}
				else
				{
					if (num256 < -1 || num256 >= 8000)
					{
						break;
					}
					if (num256 == -1)
					{
						num256 = Chest.FindChest(num257, num258);
						if (num256 == -1)
						{
							break;
						}
					}
					Chest chest4 = Main.chest[num256];
					if (chest4.x == num257 && chest4.y == num258)
					{
						NetMessage.TrySendData(69, whoAmI, -1, null, num256, num257, num258);
					}
				}
				break;
			}
			case 70:
				if (Main.netMode == 2)
				{
					int num236 = reader.ReadInt16();
					int who = reader.ReadByte();
					if (Main.netMode == 2)
					{
						who = whoAmI;
					}
					if (num236 < 200 && num236 >= 0)
					{
						NPC.CatchNPC(num236, who);
					}
				}
				break;
			case 71:
				if (Main.netMode == 2)
				{
					int x13 = reader.ReadInt32();
					int y13 = reader.ReadInt32();
					int type13 = reader.ReadInt16();
					byte style3 = reader.ReadByte();
					NPC.ReleaseNPC(x13, y13, type13, style3, whoAmI);
				}
				break;
			case 72:
				if (Main.netMode == 1)
				{
					for (int num220 = 0; num220 < 40; num220++)
					{
						Main.travelShop[num220] = reader.ReadInt16();
					}
				}
				break;
			case 73:
				switch (reader.ReadByte())
				{
				case 0:
					Main.player[whoAmI].TeleportationPotion();
					break;
				case 1:
					Main.player[whoAmI].MagicConch();
					break;
				case 2:
					Main.player[whoAmI].DemonConch();
					break;
				case 3:
					Main.player[whoAmI].Shellphone_Spawn();
					break;
				}
				break;
			case 74:
				if (Main.netMode == 1)
				{
					Main.anglerQuest = reader.ReadByte();
					Main.anglerQuestFinished = reader.ReadBoolean();
				}
				break;
			case 75:
				if (Main.netMode == 2)
				{
					string name2 = Main.player[whoAmI].name;
					if (!Main.anglerWhoFinishedToday.Contains(name2))
					{
						Main.anglerWhoFinishedToday.Add(name2);
					}
				}
				break;
			case 76:
			{
				int num181 = reader.ReadByte();
				if (num181 != Main.myPlayer || Main.ServerSideCharacter)
				{
					if (Main.netMode == 2)
					{
						num181 = whoAmI;
					}
					Player obj6 = Main.player[num181];
					obj6.anglerQuestsFinished = reader.ReadInt32();
					obj6.golferScoreAccumulated = reader.ReadInt32();
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(76, -1, whoAmI, null, num181);
					}
				}
				break;
			}
			case 77:
			{
				short type11 = reader.ReadInt16();
				ushort tileType = reader.ReadUInt16();
				short x11 = reader.ReadInt16();
				short y11 = reader.ReadInt16();
				Animation.NewTemporaryAnimation(type11, tileType, x11, y11);
				break;
			}
			case 78:
				if (Main.netMode == 1)
				{
					Main.ReportInvasionProgress(reader.ReadInt32(), reader.ReadInt32(), reader.ReadSByte(), reader.ReadSByte());
				}
				break;
			case 79:
			{
				int x9 = reader.ReadInt16();
				int y9 = reader.ReadInt16();
				short type9 = reader.ReadInt16();
				int style2 = reader.ReadInt16();
				int num151 = reader.ReadByte();
				int random = reader.ReadSByte();
				int direction = (reader.ReadBoolean() ? 1 : (-1));
				if (Main.netMode == 2)
				{
					Netplay.Clients[whoAmI].SpamAddBlock += 1f;
					if (!WorldGen.InWorld(x9, y9, 10) || !Netplay.Clients[whoAmI].TileSections[Netplay.GetSectionX(x9), Netplay.GetSectionY(y9)])
					{
						break;
					}
				}
				WorldGen.PlaceObject(x9, y9, type9, mute: false, style2, num151, random, direction);
				if (Main.netMode == 2)
				{
					NetMessage.SendObjectPlacement(whoAmI, x9, y9, type9, style2, num151, random, direction);
				}
				break;
			}
			case 80:
				if (Main.netMode == 1)
				{
					int num113 = reader.ReadByte();
					int num114 = reader.ReadInt16();
					if (num114 >= -3 && num114 < 8000)
					{
						Main.player[num113].chest = num114;
						Recipe.FindRecipes(canDelayCheck: true);
					}
				}
				break;
			case 81:
				if (Main.netMode == 1)
				{
					int x7 = (int)reader.ReadSingle();
					int y7 = (int)reader.ReadSingle();
					CombatText.NewText(color: reader.ReadRGB(), amount: reader.ReadInt32(), location: new Rectangle(x7, y7, 0, 0));
				}
				break;
			case 119:
				if (Main.netMode == 1)
				{
					int x8 = (int)reader.ReadSingle();
					int y8 = (int)reader.ReadSingle();
					CombatText.NewText(color: reader.ReadRGB(), text: NetworkText.Deserialize(reader).ToString(), location: new Rectangle(x8, y8, 0, 0));
				}
				break;
			case 82:
				NetManager.Instance.Read(reader, whoAmI, length);
				break;
			case 83:
				if (Main.netMode == 1)
				{
					int num89 = reader.ReadInt16();
					int num90 = reader.ReadInt32();
					if (num89 >= 0 && num89 < 290)
					{
						NPC.killCount[num89] = num90;
					}
				}
				break;
			case 84:
			{
				int num88 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num88 = whoAmI;
				}
				float stealth = reader.ReadSingle();
				Main.player[num88].stealth = stealth;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(84, -1, whoAmI, null, num88);
				}
				break;
			}
			case 85:
			{
				int num83 = whoAmI;
				int slot = reader.ReadInt16();
				if (Main.netMode == 2 && num83 < 255)
				{
					Chest.ServerPlaceItem(whoAmI, slot);
				}
				break;
			}
			case 86:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int num73 = reader.ReadInt32();
				if (!reader.ReadBoolean())
				{
					if (TileEntity.ByID.TryGetValue(num73, out var value3))
					{
						TileEntity.ByID.Remove(num73);
						TileEntity.ByPosition.Remove(value3.Position);
					}
				}
				else
				{
					TileEntity tileEntity = TileEntity.Read(reader, networkSend: true);
					tileEntity.ID = num73;
					TileEntity.ByID[tileEntity.ID] = tileEntity;
					TileEntity.ByPosition[tileEntity.Position] = tileEntity;
				}
				break;
			}
			case 87:
				if (Main.netMode == 2)
				{
					int x6 = reader.ReadInt16();
					int y6 = reader.ReadInt16();
					int type3 = reader.ReadByte();
					if (WorldGen.InWorld(x6, y6) && !TileEntity.ByPosition.ContainsKey(new Point16(x6, y6)))
					{
						TileEntity.PlaceEntityNet(x6, y6, type3);
					}
				}
				break;
			case 88:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int num227 = reader.ReadInt16();
				if (num227 < 0 || num227 > 400)
				{
					break;
				}
				Item item5 = Main.item[num227];
				BitsByte bitsByte28 = reader.ReadByte();
				if (bitsByte28[0])
				{
					item5.color.PackedValue = reader.ReadUInt32();
				}
				if (bitsByte28[1])
				{
					item5.damage = reader.ReadUInt16();
				}
				if (bitsByte28[2])
				{
					item5.knockBack = reader.ReadSingle();
				}
				if (bitsByte28[3])
				{
					item5.useAnimation = reader.ReadUInt16();
				}
				if (bitsByte28[4])
				{
					item5.useTime = reader.ReadUInt16();
				}
				if (bitsByte28[5])
				{
					item5.shoot = reader.ReadInt16();
				}
				if (bitsByte28[6])
				{
					item5.shootSpeed = reader.ReadSingle();
				}
				if (bitsByte28[7])
				{
					bitsByte28 = reader.ReadByte();
					if (bitsByte28[0])
					{
						item5.width = reader.ReadInt16();
					}
					if (bitsByte28[1])
					{
						item5.height = reader.ReadInt16();
					}
					if (bitsByte28[2])
					{
						item5.scale = reader.ReadSingle();
					}
					if (bitsByte28[3])
					{
						item5.ammo = reader.ReadInt16();
					}
					if (bitsByte28[4])
					{
						item5.useAmmo = reader.ReadInt16();
					}
					if (bitsByte28[5])
					{
						item5.notAmmo = reader.ReadBoolean();
					}
				}
				break;
			}
			case 89:
				if (Main.netMode == 2)
				{
					short x12 = reader.ReadInt16();
					int y12 = reader.ReadInt16();
					int netid3 = reader.ReadInt16();
					int prefix3 = reader.ReadByte();
					int stack8 = reader.ReadInt16();
					TEItemFrame.TryPlacing(x12, y12, netid3, prefix3, stack8);
				}
				break;
			case 91:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int num195 = reader.ReadInt32();
				int num196 = reader.ReadByte();
				if (num196 == 255)
				{
					if (EmoteBubble.byID.ContainsKey(num195))
					{
						EmoteBubble.byID.Remove(num195);
					}
					break;
				}
				int num197 = reader.ReadUInt16();
				int num198 = reader.ReadUInt16();
				int num199 = reader.ReadByte();
				int metadata = 0;
				if (num199 < 0)
				{
					metadata = reader.ReadInt16();
				}
				WorldUIAnchor worldUIAnchor = EmoteBubble.DeserializeNetAnchor(num196, num197);
				if (num196 == 1)
				{
					Main.player[num197].emoteTime = 360;
				}
				lock (EmoteBubble.byID)
				{
					if (!EmoteBubble.byID.ContainsKey(num195))
					{
						EmoteBubble.byID[num195] = new EmoteBubble(num199, worldUIAnchor, num198);
					}
					else
					{
						EmoteBubble.byID[num195].lifeTime = num198;
						EmoteBubble.byID[num195].lifeTimeStart = num198;
						EmoteBubble.byID[num195].emote = num199;
						EmoteBubble.byID[num195].anchor = worldUIAnchor;
					}
					EmoteBubble.byID[num195].ID = num195;
					EmoteBubble.byID[num195].metadata = metadata;
					EmoteBubble.OnBubbleChange(num195);
					break;
				}
			}
			case 92:
			{
				int num182 = reader.ReadInt16();
				int num183 = reader.ReadInt32();
				float num184 = reader.ReadSingle();
				float num185 = reader.ReadSingle();
				if (num182 >= 0 && num182 <= 200)
				{
					if (Main.netMode == 1)
					{
						Main.npc[num182].moneyPing(new Vector2(num184, num185));
						Main.npc[num182].extraValue = num183;
					}
					else
					{
						Main.npc[num182].extraValue += num183;
						NetMessage.TrySendData(92, -1, -1, null, num182, Main.npc[num182].extraValue, num184, num185);
					}
				}
				break;
			}
			case 95:
			{
				ushort num178 = reader.ReadUInt16();
				int num179 = reader.ReadByte();
				if (Main.netMode != 2)
				{
					break;
				}
				for (int num180 = 0; num180 < 1000; num180++)
				{
					if (Main.projectile[num180].owner == num178 && Main.projectile[num180].active && Main.projectile[num180].type == 602 && Main.projectile[num180].ai[1] == (float)num179)
					{
						Main.projectile[num180].Kill();
						NetMessage.TrySendData(29, -1, -1, null, Main.projectile[num180].identity, (int)num178);
						break;
					}
				}
				break;
			}
			case 96:
			{
				int num171 = reader.ReadByte();
				Player obj5 = Main.player[num171];
				int num172 = reader.ReadInt16();
				Vector2 newPos2 = reader.ReadVector2();
				Vector2 velocity6 = reader.ReadVector2();
				int num173 = (obj5.lastPortalColorIndex = num172 + ((num172 % 2 == 0) ? 1 : (-1)));
				obj5.Teleport(newPos2, 4, num172);
				obj5.velocity = velocity6;
				if (Main.netMode == 2)
				{
					NetMessage.SendData(96, -1, -1, null, num171, newPos2.X, newPos2.Y, num172);
				}
				break;
			}
			case 97:
				if (Main.netMode == 1)
				{
					AchievementsHelper.NotifyNPCKilledDirect(Main.player[Main.myPlayer], reader.ReadInt16());
				}
				break;
			case 98:
				if (Main.netMode == 1)
				{
					AchievementsHelper.NotifyProgressionEvent(reader.ReadInt16());
				}
				break;
			case 99:
			{
				int num152 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num152 = whoAmI;
				}
				Main.player[num152].MinionRestTargetPoint = reader.ReadVector2();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(99, -1, whoAmI, null, num152);
				}
				break;
			}
			case 115:
			{
				int num123 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num123 = whoAmI;
				}
				Main.player[num123].MinionAttackTargetNPC = reader.ReadInt16();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(115, -1, whoAmI, null, num123);
				}
				break;
			}
			case 100:
			{
				int num115 = reader.ReadUInt16();
				NPC obj3 = Main.npc[num115];
				int num116 = reader.ReadInt16();
				Vector2 newPos = reader.ReadVector2();
				Vector2 velocity4 = reader.ReadVector2();
				int num117 = (obj3.lastPortalColorIndex = num116 + ((num116 % 2 == 0) ? 1 : (-1)));
				obj3.Teleport(newPos, 4, num116);
				obj3.velocity = velocity4;
				obj3.netOffset *= 0f;
				break;
			}
			case 101:
				if (Main.netMode != 2)
				{
					NPC.ShieldStrengthTowerSolar = reader.ReadUInt16();
					NPC.ShieldStrengthTowerVortex = reader.ReadUInt16();
					NPC.ShieldStrengthTowerNebula = reader.ReadUInt16();
					NPC.ShieldStrengthTowerStardust = reader.ReadUInt16();
					if (NPC.ShieldStrengthTowerSolar < 0)
					{
						NPC.ShieldStrengthTowerSolar = 0;
					}
					if (NPC.ShieldStrengthTowerVortex < 0)
					{
						NPC.ShieldStrengthTowerVortex = 0;
					}
					if (NPC.ShieldStrengthTowerNebula < 0)
					{
						NPC.ShieldStrengthTowerNebula = 0;
					}
					if (NPC.ShieldStrengthTowerStardust < 0)
					{
						NPC.ShieldStrengthTowerStardust = 0;
					}
					if (NPC.ShieldStrengthTowerSolar > NPC.LunarShieldPowerMax)
					{
						NPC.ShieldStrengthTowerSolar = NPC.LunarShieldPowerMax;
					}
					if (NPC.ShieldStrengthTowerVortex > NPC.LunarShieldPowerMax)
					{
						NPC.ShieldStrengthTowerVortex = NPC.LunarShieldPowerMax;
					}
					if (NPC.ShieldStrengthTowerNebula > NPC.LunarShieldPowerMax)
					{
						NPC.ShieldStrengthTowerNebula = NPC.LunarShieldPowerMax;
					}
					if (NPC.ShieldStrengthTowerStardust > NPC.LunarShieldPowerMax)
					{
						NPC.ShieldStrengthTowerStardust = NPC.LunarShieldPowerMax;
					}
				}
				break;
			case 102:
			{
				int num60 = reader.ReadByte();
				ushort num61 = reader.ReadUInt16();
				Vector2 other = reader.ReadVector2();
				if (Main.netMode == 2)
				{
					num60 = whoAmI;
					NetMessage.TrySendData(102, -1, -1, null, num60, (int)num61, other.X, other.Y);
					break;
				}
				Player player4 = Main.player[num60];
				for (int num62 = 0; num62 < 255; num62++)
				{
					Player player5 = Main.player[num62];
					if (!player5.active || player5.dead || (player4.team != 0 && player4.team != player5.team) || !(player5.Distance(other) < 700f))
					{
						continue;
					}
					Vector2 value2 = player4.Center - player5.Center;
					Vector2 vector3 = Vector2.Normalize(value2);
					if (!vector3.HasNaNs())
					{
						int type4 = 90;
						float num63 = 0f;
						float num64 = MathF.PI / 15f;
						Vector2 spinningpoint = new Vector2(0f, -8f);
						Vector2 vector4 = new Vector2(-3f);
						float num65 = 0f;
						float num66 = 0.005f;
						switch (num61)
						{
						case 179:
							type4 = 86;
							break;
						case 173:
							type4 = 90;
							break;
						case 176:
							type4 = 88;
							break;
						}
						for (int num67 = 0; (float)num67 < value2.Length() / 6f; num67++)
						{
							Vector2 position2 = player5.Center + 6f * (float)num67 * vector3 + spinningpoint.RotatedBy(num63) + vector4;
							num63 += num64;
							int num68 = Dust.NewDust(position2, 6, 6, type4, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num68].noGravity = true;
							Main.dust[num68].velocity = Vector2.Zero;
							num65 = (Main.dust[num68].fadeIn = num65 + num66);
							Main.dust[num68].velocity += vector3 * 1.5f;
						}
					}
					player5.NebulaLevelup(num61);
				}
				break;
			}
			case 103:
				if (Main.netMode == 1)
				{
					NPC.MaxMoonLordCountdown = reader.ReadInt32();
					NPC.MoonLordCountdown = reader.ReadInt32();
				}
				break;
			case 104:
				if (Main.netMode == 1 && Main.npcShop > 0)
				{
					Item[] item = Main.instance.shop[Main.npcShop].item;
					int num47 = reader.ReadByte();
					int type2 = reader.ReadInt16();
					int stack2 = reader.ReadInt16();
					int prefixWeWant = reader.ReadByte();
					int value = reader.ReadInt32();
					BitsByte bitsByte5 = reader.ReadByte();
					if (num47 < item.Length)
					{
						item[num47] = new Item();
						item[num47].netDefaults(type2);
						item[num47].stack = stack2;
						item[num47].Prefix(prefixWeWant);
						item[num47].value = value;
						item[num47].buyOnce = bitsByte5[0];
					}
				}
				break;
			case 105:
				if (Main.netMode != 1)
				{
					short i2 = reader.ReadInt16();
					int j2 = reader.ReadInt16();
					bool on = reader.ReadBoolean();
					WorldGen.ToggleGemLock(i2, j2, on);
				}
				break;
			case 106:
				if (Main.netMode == 1)
				{
					HalfVector2 halfVector = default(HalfVector2);
					halfVector.PackedValue = reader.ReadUInt32();
					Utils.PoofOfSmoke(halfVector.ToVector2());
				}
				break;
			case 107:
				if (Main.netMode == 1)
				{
					Color c = reader.ReadRGB();
					string text3 = NetworkText.Deserialize(reader).ToString();
					int widthLimit = reader.ReadInt16();
					Main.NewTextMultiline(text3, force: false, c, widthLimit);
				}
				break;
			case 108:
				if (Main.netMode == 1)
				{
					int damage = reader.ReadInt16();
					float knockBack = reader.ReadSingle();
					int x4 = reader.ReadInt16();
					int y4 = reader.ReadInt16();
					int angle = reader.ReadInt16();
					int ammo = reader.ReadInt16();
					int num32 = reader.ReadByte();
					if (num32 == Main.myPlayer)
					{
						WorldGen.ShootFromCannon(x4, y4, angle, ammo, damage, knockBack, num32, fromWire: true);
					}
				}
				break;
			case 109:
				if (Main.netMode == 2)
				{
					short x2 = reader.ReadInt16();
					int y2 = reader.ReadInt16();
					int x3 = reader.ReadInt16();
					int y3 = reader.ReadInt16();
					byte toolMode = reader.ReadByte();
					int num29 = whoAmI;
					WiresUI.Settings.MultiToolMode toolMode2 = WiresUI.Settings.ToolMode;
					WiresUI.Settings.ToolMode = (WiresUI.Settings.MultiToolMode)toolMode;
					Wiring.MassWireOperation(new Point(x2, y2), new Point(x3, y3), Main.player[num29]);
					WiresUI.Settings.ToolMode = toolMode2;
				}
				break;
			case 110:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int type = reader.ReadInt16();
				int num19 = reader.ReadInt16();
				int num20 = reader.ReadByte();
				if (num20 == Main.myPlayer)
				{
					Player player2 = Main.player[num20];
					for (int k = 0; k < num19; k++)
					{
						player2.ConsumeItem(type);
					}
					player2.wireOperationsCooldown = 0;
				}
				break;
			}
			case 111:
				if (Main.netMode == 2)
				{
					BirthdayParty.ToggleManualParty();
				}
				break;
			case 112:
			{
				int num13 = reader.ReadByte();
				int num14 = reader.ReadInt32();
				int num15 = reader.ReadInt32();
				int num16 = reader.ReadByte();
				int num17 = reader.ReadInt16();
				switch (num13)
				{
				case 1:
					if (Main.netMode == 1)
					{
						WorldGen.TreeGrowFX(num14, num15, num16, num17);
					}
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(b, -1, -1, null, num13, num14, num15, num16, num17);
					}
					break;
				case 2:
					NPC.FairyEffects(new Vector2(num14, num15), num17);
					break;
				}
				break;
			}
			case 113:
			{
				int x14 = reader.ReadInt16();
				int y14 = reader.ReadInt16();
				if (Main.netMode == 2 && !Main.snowMoon && !Main.pumpkinMoon)
				{
					if (DD2Event.WouldFailSpawningHere(x14, y14))
					{
						DD2Event.FailureMessage(whoAmI);
					}
					DD2Event.SummonCrystal(x14, y14, whoAmI);
				}
				break;
			}
			case 114:
				if (Main.netMode == 1)
				{
					DD2Event.WipeEntities();
				}
				break;
			case 116:
				if (Main.netMode == 1)
				{
					DD2Event.TimeLeftBetweenWaves = reader.ReadInt32();
				}
				break;
			case 117:
			{
				int num243 = reader.ReadByte();
				if (Main.netMode != 2 || whoAmI == num243 || (Main.player[num243].hostile && Main.player[whoAmI].hostile))
				{
					PlayerDeathReason playerDeathReason2 = PlayerDeathReason.FromReader(reader);
					int damage3 = reader.ReadInt16();
					int num244 = reader.ReadByte() - 1;
					BitsByte bitsByte32 = reader.ReadByte();
					bool flag15 = bitsByte32[0];
					bool pvp2 = bitsByte32[1];
					int num245 = reader.ReadSByte();
					Main.player[num243].Hurt(playerDeathReason2, damage3, num244, pvp2, quiet: true, flag15, num245);
					if (Main.netMode == 2)
					{
						NetMessage.SendPlayerHurt(num243, playerDeathReason2, damage3, num244, flag15, pvp2, num245, -1, whoAmI);
					}
				}
				break;
			}
			case 118:
			{
				int num232 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num232 = whoAmI;
				}
				PlayerDeathReason playerDeathReason = PlayerDeathReason.FromReader(reader);
				int num233 = reader.ReadInt16();
				int num234 = reader.ReadByte() - 1;
				bool pvp = ((BitsByte)reader.ReadByte())[0];
				Main.player[num232].KillMe(playerDeathReason, num233, num234, pvp);
				if (Main.netMode == 2)
				{
					NetMessage.SendPlayerDeath(num232, playerDeathReason, num233, num234, pvp, -1, whoAmI);
				}
				break;
			}
			case 120:
			{
				int num222 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num222 = whoAmI;
				}
				int num223 = reader.ReadByte();
				if (num223 >= 0 && num223 < 151 && Main.netMode == 2)
				{
					EmoteBubble.NewBubble(num223, new WorldUIAnchor(Main.player[num222]), 360);
					EmoteBubble.CheckForNPCsToReactToEmoteBubble(num223, Main.player[num222]);
				}
				break;
			}
			case 121:
			{
				int num192 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num192 = whoAmI;
				}
				int num193 = reader.ReadInt32();
				int num194 = reader.ReadByte();
				bool flag12 = false;
				if (num194 >= 8)
				{
					flag12 = true;
					num194 -= 8;
				}
				if (!TileEntity.ByID.TryGetValue(num193, out var value9))
				{
					reader.ReadInt32();
					reader.ReadByte();
					break;
				}
				if (num194 >= 8)
				{
					value9 = null;
				}
				if (value9 is TEDisplayDoll tEDisplayDoll)
				{
					tEDisplayDoll.ReadItem(num194, reader, flag12);
				}
				else
				{
					reader.ReadInt32();
					reader.ReadByte();
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, num192, null, num192, num193, num194, flag12.ToInt());
				}
				break;
			}
			case 122:
			{
				int num162 = reader.ReadInt32();
				int num163 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num163 = whoAmI;
				}
				if (Main.netMode == 2)
				{
					if (num162 == -1)
					{
						Main.player[num163].tileEntityAnchor.Clear();
						NetMessage.TrySendData(b, -1, -1, null, num162, num163);
						break;
					}
					if (!TileEntity.IsOccupied(num162, out var _) && TileEntity.ByID.TryGetValue(num162, out var value6))
					{
						Main.player[num163].tileEntityAnchor.Set(num162, value6.Position.X, value6.Position.Y);
						NetMessage.TrySendData(b, -1, -1, null, num162, num163);
					}
				}
				if (Main.netMode == 1)
				{
					TileEntity value7;
					if (num162 == -1)
					{
						Main.player[num163].tileEntityAnchor.Clear();
					}
					else if (TileEntity.ByID.TryGetValue(num162, out value7))
					{
						TileEntity.SetInteractionAnchor(Main.player[num163], value7.Position.X, value7.Position.Y, num162);
					}
				}
				break;
			}
			case 123:
				if (Main.netMode == 2)
				{
					short x10 = reader.ReadInt16();
					int y10 = reader.ReadInt16();
					int netid2 = reader.ReadInt16();
					int prefix2 = reader.ReadByte();
					int stack6 = reader.ReadInt16();
					TEWeaponsRack.TryPlacing(x10, y10, netid2, prefix2, stack6);
				}
				break;
			case 124:
			{
				int num120 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num120 = whoAmI;
				}
				int num121 = reader.ReadInt32();
				int num122 = reader.ReadByte();
				bool flag6 = false;
				if (num122 >= 2)
				{
					flag6 = true;
					num122 -= 2;
				}
				if (!TileEntity.ByID.TryGetValue(num121, out var value5))
				{
					reader.ReadInt32();
					reader.ReadByte();
					break;
				}
				if (num122 >= 2)
				{
					value5 = null;
				}
				if (value5 is TEHatRack tEHatRack)
				{
					tEHatRack.ReadItem(num122, reader, flag6);
				}
				else
				{
					reader.ReadInt32();
					reader.ReadByte();
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, num120, null, num120, num121, num122, flag6.ToInt());
				}
				break;
			}
			case 125:
			{
				int num109 = reader.ReadByte();
				int num110 = reader.ReadInt16();
				int num111 = reader.ReadInt16();
				int num112 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num109 = whoAmI;
				}
				if (Main.netMode == 1)
				{
					Main.player[Main.myPlayer].GetOtherPlayersPickTile(num110, num111, num112);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(125, -1, num109, null, num109, num110, num111, num112);
				}
				break;
			}
			case 126:
				if (Main.netMode == 1)
				{
					NPC.RevengeManager.AddMarkerFromReader(reader);
				}
				break;
			case 127:
			{
				int markerUniqueID = reader.ReadInt32();
				if (Main.netMode == 1)
				{
					NPC.RevengeManager.DestroyMarker(markerUniqueID);
				}
				break;
			}
			case 128:
			{
				int num98 = reader.ReadByte();
				int num99 = reader.ReadUInt16();
				int num100 = reader.ReadUInt16();
				int num101 = reader.ReadUInt16();
				int num102 = reader.ReadUInt16();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(128, -1, num98, null, num98, num101, num102, 0f, num99, num100);
				}
				else
				{
					GolfHelper.ContactListener.PutBallInCup_TextAndEffects(new Point(num99, num100), num98, num101, num102);
				}
				break;
			}
			case 129:
				if (Main.netMode == 1)
				{
					Main.FixUIScale();
					Main.TrySetPreparationState(Main.WorldPreparationState.ProcessingData);
				}
				break;
			case 130:
			{
				if (Main.netMode != 2)
				{
					break;
				}
				int num79 = reader.ReadUInt16();
				int num80 = reader.ReadUInt16();
				int num81 = reader.ReadInt16();
				if (num81 == 682)
				{
					if (NPC.unlockedSlimeRedSpawn)
					{
						break;
					}
					NPC.unlockedSlimeRedSpawn = true;
					NetMessage.TrySendData(7);
				}
				num79 *= 16;
				num80 *= 16;
				NPC nPC2 = new NPC();
				nPC2.SetDefaults(num81);
				int type5 = nPC2.type;
				int netID = nPC2.netID;
				int num82 = NPC.NewNPC(new EntitySource_FishedOut(Main.player[whoAmI]), num79, num80, num81);
				if (netID != type5)
				{
					Main.npc[num82].SetDefaults(netID);
					NetMessage.TrySendData(23, -1, -1, null, num82);
				}
				if (num81 == 682)
				{
					WorldGen.CheckAchievement_RealEstateAndTownSlimes();
				}
				break;
			}
			case 131:
				if (Main.netMode == 1)
				{
					int num74 = reader.ReadUInt16();
					NPC nPC = null;
					nPC = ((num74 >= 200) ? new NPC() : Main.npc[num74]);
					int num75 = reader.ReadByte();
					if (num75 == 1)
					{
						int time = reader.ReadInt32();
						int fromWho = reader.ReadInt16();
						nPC.GetImmuneTime(fromWho, time);
					}
				}
				break;
			case 132:
				if (Main.netMode == 1)
				{
					Point point = reader.ReadVector2().ToPoint();
					ushort key = reader.ReadUInt16();
					LegacySoundStyle legacySoundStyle = SoundID.SoundByIndex[key];
					BitsByte bitsByte4 = reader.ReadByte();
					int num44 = -1;
					float num45 = 1f;
					float num46 = 0f;
					SoundEngine.PlaySound(Style: (!bitsByte4[0]) ? legacySoundStyle.Style : reader.ReadInt32(), volumeScale: (!bitsByte4[1]) ? legacySoundStyle.Volume : MathHelper.Clamp(reader.ReadSingle(), 0f, 1f), pitchOffset: (!bitsByte4[2]) ? legacySoundStyle.GetRandomPitch() : MathHelper.Clamp(reader.ReadSingle(), -1f, 1f), type: legacySoundStyle.SoundId, x: point.X, y: point.Y);
				}
				break;
			case 133:
				if (Main.netMode == 2)
				{
					short x5 = reader.ReadInt16();
					int y5 = reader.ReadInt16();
					int netid = reader.ReadInt16();
					int prefix = reader.ReadByte();
					int stack = reader.ReadInt16();
					TEFoodPlatter.TryPlacing(x5, y5, netid, prefix, stack);
				}
				break;
			case 134:
			{
				int num41 = reader.ReadByte();
				int ladyBugLuckTimeLeft = reader.ReadInt32();
				float torchLuck = reader.ReadSingle();
				byte luckPotion = reader.ReadByte();
				bool hasGardenGnomeNearby = reader.ReadBoolean();
				float equipmentBasedLuckBonus = reader.ReadSingle();
				float coinLuck = reader.ReadSingle();
				if (Main.netMode == 2)
				{
					num41 = whoAmI;
				}
				Player obj2 = Main.player[num41];
				obj2.ladyBugLuckTimeLeft = ladyBugLuckTimeLeft;
				obj2.torchLuck = torchLuck;
				obj2.luckPotion = luckPotion;
				obj2.HasGardenGnomeNearby = hasGardenGnomeNearby;
				obj2.equipmentBasedLuckBonus = equipmentBasedLuckBonus;
				obj2.coinLuck = coinLuck;
				obj2.RecalculateLuck();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(134, -1, num41, null, num41);
				}
				break;
			}
			case 135:
			{
				int num33 = reader.ReadByte();
				if (Main.netMode == 1)
				{
					Main.player[num33].immuneAlpha = 255;
				}
				break;
			}
			case 136:
			{
				for (int l = 0; l < 2; l++)
				{
					for (int m = 0; m < 3; m++)
					{
						NPC.cavernMonsterType[l, m] = reader.ReadUInt16();
					}
				}
				break;
			}
			case 137:
				if (Main.netMode == 2)
				{
					int num28 = reader.ReadInt16();
					int buffTypeToRemove = reader.ReadUInt16();
					if (num28 >= 0 && num28 < 200)
					{
						Main.npc[num28].RequestBuffRemoval(buffTypeToRemove);
					}
				}
				break;
			case 139:
				if (Main.netMode != 2)
				{
					int num26 = reader.ReadByte();
					bool flag = reader.ReadBoolean();
					Main.countsAsHostForGameplay[num26] = flag;
				}
				break;
			case 140:
			{
				int num24 = reader.ReadByte();
				int num25 = reader.ReadInt32();
				switch (num24)
				{
				case 0:
					if (Main.netMode == 1)
					{
						CreditsRollEvent.SetRemainingTimeDirect(num25);
					}
					break;
				case 1:
					if (Main.netMode == 2)
					{
						NPC.TransformCopperSlime(num25);
					}
					break;
				case 2:
					if (Main.netMode == 2)
					{
						NPC.TransformElderSlime(num25);
					}
					break;
				}
				break;
			}
			case 141:
			{
				LucyAxeMessage.MessageSource messageSource = (LucyAxeMessage.MessageSource)reader.ReadByte();
				byte b2 = reader.ReadByte();
				Vector2 velocity = reader.ReadVector2();
				int num21 = reader.ReadInt32();
				int num22 = reader.ReadInt32();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(141, -1, whoAmI, null, (int)messageSource, (int)b2, velocity.X, velocity.Y, num21, num22);
				}
				else
				{
					LucyAxeMessage.CreateFromNet(messageSource, b2, new Vector2(num21, num22), velocity);
				}
				break;
			}
			case 142:
			{
				int num18 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num18 = whoAmI;
				}
				Player obj = Main.player[num18];
				obj.piggyBankProjTracker.TryReading(reader);
				obj.voidLensChest.TryReading(reader);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(142, -1, whoAmI, null, num18);
				}
				break;
			}
			case 143:
				if (Main.netMode == 2)
				{
					DD2Event.AttemptToSkipWaitTime();
				}
				break;
			case 144:
				if (Main.netMode == 2)
				{
					NPC.HaveDryadDoStardewAnimation();
				}
				break;
			case 146:
				switch (reader.ReadByte())
				{
				case 0:
					Item.ShimmerEffect(reader.ReadVector2());
					break;
				case 1:
				{
					Vector2 coinPosition = reader.ReadVector2();
					int coinAmount = reader.ReadInt32();
					Main.player[Main.myPlayer].AddCoinLuck(coinPosition, coinAmount);
					break;
				}
				case 2:
				{
					int num12 = reader.ReadInt32();
					Main.npc[num12].SetNetShimmerEffect();
					break;
				}
				}
				break;
			case 147:
			{
				int num6 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num6 = whoAmI;
				}
				int num7 = reader.ReadByte();
				Main.player[num6].TrySwitchingLoadout(num7);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, num6, null, num6, num7);
				}
				break;
			}
			default:
				if (Netplay.Clients[whoAmI].State == 0)
				{
					NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
				}
				break;
			case 15:
			case 25:
			case 26:
			case 44:
			case 67:
			case 93:
				break;
			}
		}

		private static void TrySendingItemArray(int plr, Item[] array, int slotStartIndex)
		{
			for (int i = 0; i < array.Length; i++)
			{
				NetMessage.TrySendData(5, -1, -1, null, plr, slotStartIndex + i, (int)array[i].prefix);
			}
		}
	}
}
