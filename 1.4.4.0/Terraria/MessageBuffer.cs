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
			if (b >= 148)
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
					if (reader.ReadString() == "Terraria" + 269)
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
					int num100 = reader.ReadByte();
					bool value4 = reader.ReadBoolean();
					Netplay.Connection.ServerSpecialFlags[2] = value4;
					if (num100 != Main.myPlayer)
					{
						Main.player[num100] = Main.ActivePlayerFileData.Player;
						Main.player[Main.myPlayer] = new Player();
					}
					Main.player[num100].whoAmI = num100;
					Main.myPlayer = num100;
					Player player5 = Main.player[num100];
					NetMessage.TrySendData(4, -1, -1, null, num100);
					NetMessage.TrySendData(68, -1, -1, null, num100);
					NetMessage.TrySendData(16, -1, -1, null, num100);
					NetMessage.TrySendData(42, -1, -1, null, num100);
					NetMessage.TrySendData(50, -1, -1, null, num100);
					NetMessage.TrySendData(147, -1, -1, null, num100, player5.CurrentLoadoutIndex);
					for (int num101 = 0; num101 < 59; num101++)
					{
						NetMessage.TrySendData(5, -1, -1, null, num100, PlayerItemSlotID.Inventory0 + num101, (int)player5.inventory[num101].prefix);
					}
					TrySendingItemArray(num100, player5.armor, PlayerItemSlotID.Armor0);
					TrySendingItemArray(num100, player5.dye, PlayerItemSlotID.Dye0);
					TrySendingItemArray(num100, player5.miscEquips, PlayerItemSlotID.Misc0);
					TrySendingItemArray(num100, player5.miscDyes, PlayerItemSlotID.MiscDye0);
					TrySendingItemArray(num100, player5.bank.item, PlayerItemSlotID.Bank1_0);
					TrySendingItemArray(num100, player5.bank2.item, PlayerItemSlotID.Bank2_0);
					NetMessage.TrySendData(5, -1, -1, null, num100, PlayerItemSlotID.TrashItem, (int)player5.trashItem.prefix);
					TrySendingItemArray(num100, player5.bank3.item, PlayerItemSlotID.Bank3_0);
					TrySendingItemArray(num100, player5.bank4.item, PlayerItemSlotID.Bank4_0);
					TrySendingItemArray(num100, player5.Loadouts[0].Armor, PlayerItemSlotID.Loadout1_Armor_0);
					TrySendingItemArray(num100, player5.Loadouts[0].Dye, PlayerItemSlotID.Loadout1_Dye_0);
					TrySendingItemArray(num100, player5.Loadouts[1].Armor, PlayerItemSlotID.Loadout2_Armor_0);
					TrySendingItemArray(num100, player5.Loadouts[1].Dye, PlayerItemSlotID.Loadout2_Dye_0);
					TrySendingItemArray(num100, player5.Loadouts[2].Armor, PlayerItemSlotID.Loadout3_Armor_0);
					TrySendingItemArray(num100, player5.Loadouts[2].Dye, PlayerItemSlotID.Loadout3_Dye_0);
					NetMessage.TrySendData(6);
					if (Netplay.Connection.State == 2)
					{
						Netplay.Connection.State = 3;
					}
				}
				break;
			case 4:
			{
				int num253 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num253 = whoAmI;
				}
				if (num253 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					break;
				}
				Player player15 = Main.player[num253];
				player15.whoAmI = num253;
				player15.skinVariant = reader.ReadByte();
				player15.skinVariant = (int)MathHelper.Clamp(player15.skinVariant, 0f, 11f);
				player15.hair = reader.ReadByte();
				if (player15.hair >= 165)
				{
					player15.hair = 0;
				}
				player15.name = reader.ReadString().Trim().Trim();
				player15.hairDye = reader.ReadByte();
				BitsByte bitsByte29 = reader.ReadByte();
				for (int num254 = 0; num254 < 8; num254++)
				{
					player15.hideVisibleAccessory[num254] = bitsByte29[num254];
				}
				bitsByte29 = reader.ReadByte();
				for (int num255 = 0; num255 < 2; num255++)
				{
					player15.hideVisibleAccessory[num255 + 8] = bitsByte29[num255];
				}
				player15.hideMisc = reader.ReadByte();
				player15.hairColor = reader.ReadRGB();
				player15.skinColor = reader.ReadRGB();
				player15.eyeColor = reader.ReadRGB();
				player15.shirtColor = reader.ReadRGB();
				player15.underShirtColor = reader.ReadRGB();
				player15.pantsColor = reader.ReadRGB();
				player15.shoeColor = reader.ReadRGB();
				BitsByte bitsByte30 = reader.ReadByte();
				player15.difficulty = 0;
				if (bitsByte30[0])
				{
					player15.difficulty = 1;
				}
				if (bitsByte30[1])
				{
					player15.difficulty = 2;
				}
				if (bitsByte30[3])
				{
					player15.difficulty = 3;
				}
				if (player15.difficulty > 3)
				{
					player15.difficulty = 3;
				}
				player15.extraAccessory = bitsByte30[2];
				BitsByte bitsByte31 = reader.ReadByte();
				player15.UsingBiomeTorches = bitsByte31[0];
				player15.happyFunTorchTime = bitsByte31[1];
				player15.unlockedBiomeTorches = bitsByte31[2];
				player15.unlockedSuperCart = bitsByte31[3];
				player15.enabledSuperCart = bitsByte31[4];
				BitsByte bitsByte32 = reader.ReadByte();
				player15.usedAegisCrystal = bitsByte32[0];
				player15.usedAegisFruit = bitsByte32[1];
				player15.usedArcaneCrystal = bitsByte32[2];
				player15.usedGalaxyPearl = bitsByte32[3];
				player15.usedGummyWorm = bitsByte32[4];
				player15.usedAmbrosia = bitsByte32[5];
				if (Main.netMode != 2)
				{
					break;
				}
				bool flag16 = false;
				if (Netplay.Clients[whoAmI].State < 10)
				{
					for (int num256 = 0; num256 < 255; num256++)
					{
						if (num256 != num253 && player15.name == Main.player[num256].name && Netplay.Clients[num256].IsActive)
						{
							flag16 = true;
						}
					}
				}
				if (flag16)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey(Lang.mp[5].Key, player15.name));
				}
				else if (player15.name.Length > Player.nameLen)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.NameTooLong"));
				}
				else if (player15.name == "")
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.EmptyName"));
				}
				else if (player15.difficulty == 3 && !Main.GameModeInfo.IsJourneyMode)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.PlayerIsCreativeAndWorldIsNotCreative"));
				}
				else if (player15.difficulty != 3 && Main.GameModeInfo.IsJourneyMode)
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.PlayerIsNotCreativeAndWorldIsCreative"));
				}
				else
				{
					Netplay.Clients[whoAmI].Name = player15.name;
					Netplay.Clients[whoAmI].Name = player15.name;
					NetMessage.TrySendData(4, -1, whoAmI, null, num253);
				}
				break;
			}
			case 5:
			{
				int num214 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num214 = whoAmI;
				}
				if (num214 == Main.myPlayer && !Main.ServerSideCharacter && !Main.player[num214].HasLockedInventory())
				{
					break;
				}
				Player player12 = Main.player[num214];
				lock (player12)
				{
					int num215 = reader.ReadInt16();
					int stack7 = reader.ReadInt16();
					int num216 = reader.ReadByte();
					int type13 = reader.ReadInt16();
					Item[] array3 = null;
					Item[] array4 = null;
					int num217 = 0;
					bool flag13 = false;
					Player clientPlayer = Main.clientPlayer;
					if (num215 >= PlayerItemSlotID.Loadout3_Dye_0)
					{
						num217 = num215 - PlayerItemSlotID.Loadout3_Dye_0;
						array3 = player12.Loadouts[2].Dye;
						array4 = clientPlayer.Loadouts[2].Dye;
					}
					else if (num215 >= PlayerItemSlotID.Loadout3_Armor_0)
					{
						num217 = num215 - PlayerItemSlotID.Loadout3_Armor_0;
						array3 = player12.Loadouts[2].Armor;
						array4 = clientPlayer.Loadouts[2].Armor;
					}
					else if (num215 >= PlayerItemSlotID.Loadout2_Dye_0)
					{
						num217 = num215 - PlayerItemSlotID.Loadout2_Dye_0;
						array3 = player12.Loadouts[1].Dye;
						array4 = clientPlayer.Loadouts[1].Dye;
					}
					else if (num215 >= PlayerItemSlotID.Loadout2_Armor_0)
					{
						num217 = num215 - PlayerItemSlotID.Loadout2_Armor_0;
						array3 = player12.Loadouts[1].Armor;
						array4 = clientPlayer.Loadouts[1].Armor;
					}
					else if (num215 >= PlayerItemSlotID.Loadout1_Dye_0)
					{
						num217 = num215 - PlayerItemSlotID.Loadout1_Dye_0;
						array3 = player12.Loadouts[0].Dye;
						array4 = clientPlayer.Loadouts[0].Dye;
					}
					else if (num215 >= PlayerItemSlotID.Loadout1_Armor_0)
					{
						num217 = num215 - PlayerItemSlotID.Loadout1_Armor_0;
						array3 = player12.Loadouts[0].Armor;
						array4 = clientPlayer.Loadouts[0].Armor;
					}
					else if (num215 >= PlayerItemSlotID.Bank4_0)
					{
						num217 = num215 - PlayerItemSlotID.Bank4_0;
						array3 = player12.bank4.item;
						array4 = clientPlayer.bank4.item;
						if (Main.netMode == 1 && player12.disableVoidBag == num217)
						{
							player12.disableVoidBag = -1;
							Recipe.FindRecipes(canDelayCheck: true);
						}
					}
					else if (num215 >= PlayerItemSlotID.Bank3_0)
					{
						num217 = num215 - PlayerItemSlotID.Bank3_0;
						array3 = player12.bank3.item;
						array4 = clientPlayer.bank3.item;
					}
					else if (num215 >= PlayerItemSlotID.TrashItem)
					{
						flag13 = true;
					}
					else if (num215 >= PlayerItemSlotID.Bank2_0)
					{
						num217 = num215 - PlayerItemSlotID.Bank2_0;
						array3 = player12.bank2.item;
						array4 = clientPlayer.bank2.item;
					}
					else if (num215 >= PlayerItemSlotID.Bank1_0)
					{
						num217 = num215 - PlayerItemSlotID.Bank1_0;
						array3 = player12.bank.item;
						array4 = clientPlayer.bank.item;
					}
					else if (num215 >= PlayerItemSlotID.MiscDye0)
					{
						num217 = num215 - PlayerItemSlotID.MiscDye0;
						array3 = player12.miscDyes;
						array4 = clientPlayer.miscDyes;
					}
					else if (num215 >= PlayerItemSlotID.Misc0)
					{
						num217 = num215 - PlayerItemSlotID.Misc0;
						array3 = player12.miscEquips;
						array4 = clientPlayer.miscEquips;
					}
					else if (num215 >= PlayerItemSlotID.Dye0)
					{
						num217 = num215 - PlayerItemSlotID.Dye0;
						array3 = player12.dye;
						array4 = clientPlayer.dye;
					}
					else if (num215 >= PlayerItemSlotID.Armor0)
					{
						num217 = num215 - PlayerItemSlotID.Armor0;
						array3 = player12.armor;
						array4 = clientPlayer.armor;
					}
					else
					{
						num217 = num215 - PlayerItemSlotID.Inventory0;
						array3 = player12.inventory;
						array4 = clientPlayer.inventory;
					}
					if (flag13)
					{
						player12.trashItem = new Item();
						player12.trashItem.netDefaults(type13);
						player12.trashItem.stack = stack7;
						player12.trashItem.Prefix(num216);
						if (num214 == Main.myPlayer && !Main.ServerSideCharacter)
						{
							clientPlayer.trashItem = player12.trashItem.Clone();
						}
					}
					else if (num215 <= 58)
					{
						int type14 = array3[num217].type;
						int stack8 = array3[num217].stack;
						array3[num217] = new Item();
						array3[num217].netDefaults(type13);
						array3[num217].stack = stack7;
						array3[num217].Prefix(num216);
						if (num214 == Main.myPlayer && !Main.ServerSideCharacter)
						{
							array4[num217] = array3[num217].Clone();
						}
						if (num214 == Main.myPlayer && num217 == 58)
						{
							Main.mouseItem = array3[num217].Clone();
						}
						if (num214 == Main.myPlayer && Main.netMode == 1)
						{
							Main.player[num214].inventoryChestStack[num215] = false;
							if (array3[num217].stack != stack8 || array3[num217].type != type14)
							{
								Recipe.FindRecipes(canDelayCheck: true);
							}
						}
					}
					else
					{
						array3[num217] = new Item();
						array3[num217].netDefaults(type13);
						array3[num217].stack = stack7;
						array3[num217].Prefix(num216);
						if (num214 == Main.myPlayer && !Main.ServerSideCharacter)
						{
							array4[num217] = array3[num217].Clone();
						}
					}
					if (Main.netMode == 2 && num214 == whoAmI && num215 <= 58 + player12.armor.Length + player12.dye.Length + player12.miscEquips.Length + player12.miscDyes.Length)
					{
						NetMessage.TrySendData(5, -1, whoAmI, null, num214, num215, num216);
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
					BitsByte bitsByte5 = reader.ReadByte();
					Main.dayTime = bitsByte5[0];
					Main.bloodMoon = bitsByte5[1];
					Main.eclipse = bitsByte5[2];
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
					for (int m = 0; m < 3; m++)
					{
						Main.treeX[m] = reader.ReadInt32();
					}
					for (int n = 0; n < 4; n++)
					{
						Main.treeStyle[n] = reader.ReadByte();
					}
					for (int num16 = 0; num16 < 3; num16++)
					{
						Main.caveBackX[num16] = reader.ReadInt32();
					}
					for (int num17 = 0; num17 < 4; num17++)
					{
						Main.caveBackStyle[num17] = reader.ReadByte();
					}
					WorldGen.TreeTops.SyncReceive(reader);
					WorldGen.BackgroundsCache.UpdateCache();
					Main.maxRaining = reader.ReadSingle();
					Main.raining = Main.maxRaining > 0f;
					BitsByte bitsByte6 = reader.ReadByte();
					WorldGen.shadowOrbSmashed = bitsByte6[0];
					NPC.downedBoss1 = bitsByte6[1];
					NPC.downedBoss2 = bitsByte6[2];
					NPC.downedBoss3 = bitsByte6[3];
					Main.hardMode = bitsByte6[4];
					NPC.downedClown = bitsByte6[5];
					Main.ServerSideCharacter = bitsByte6[6];
					NPC.downedPlantBoss = bitsByte6[7];
					if (Main.ServerSideCharacter)
					{
						Main.ActivePlayerFileData.MarkAsServerSide();
					}
					BitsByte bitsByte7 = reader.ReadByte();
					NPC.downedMechBoss1 = bitsByte7[0];
					NPC.downedMechBoss2 = bitsByte7[1];
					NPC.downedMechBoss3 = bitsByte7[2];
					NPC.downedMechBossAny = bitsByte7[3];
					Main.cloudBGActive = (bitsByte7[4] ? 1 : 0);
					WorldGen.crimson = bitsByte7[5];
					Main.pumpkinMoon = bitsByte7[6];
					Main.snowMoon = bitsByte7[7];
					BitsByte bitsByte8 = reader.ReadByte();
					Main.fastForwardTimeToDawn = bitsByte8[1];
					Main.UpdateTimeRate();
					bool num18 = bitsByte8[2];
					NPC.downedSlimeKing = bitsByte8[3];
					NPC.downedQueenBee = bitsByte8[4];
					NPC.downedFishron = bitsByte8[5];
					NPC.downedMartians = bitsByte8[6];
					NPC.downedAncientCultist = bitsByte8[7];
					BitsByte bitsByte9 = reader.ReadByte();
					NPC.downedMoonlord = bitsByte9[0];
					NPC.downedHalloweenKing = bitsByte9[1];
					NPC.downedHalloweenTree = bitsByte9[2];
					NPC.downedChristmasIceQueen = bitsByte9[3];
					NPC.downedChristmasSantank = bitsByte9[4];
					NPC.downedChristmasTree = bitsByte9[5];
					NPC.downedGolemBoss = bitsByte9[6];
					BirthdayParty.ManualParty = bitsByte9[7];
					BitsByte bitsByte10 = reader.ReadByte();
					NPC.downedPirates = bitsByte10[0];
					NPC.downedFrost = bitsByte10[1];
					NPC.downedGoblins = bitsByte10[2];
					Sandstorm.Happening = bitsByte10[3];
					DD2Event.Ongoing = bitsByte10[4];
					DD2Event.DownedInvasionT1 = bitsByte10[5];
					DD2Event.DownedInvasionT2 = bitsByte10[6];
					DD2Event.DownedInvasionT3 = bitsByte10[7];
					BitsByte bitsByte11 = reader.ReadByte();
					NPC.combatBookWasUsed = bitsByte11[0];
					LanternNight.ManualLanterns = bitsByte11[1];
					NPC.downedTowerSolar = bitsByte11[2];
					NPC.downedTowerVortex = bitsByte11[3];
					NPC.downedTowerNebula = bitsByte11[4];
					NPC.downedTowerStardust = bitsByte11[5];
					Main.forceHalloweenForToday = bitsByte11[6];
					Main.forceXMasForToday = bitsByte11[7];
					BitsByte bitsByte12 = reader.ReadByte();
					NPC.boughtCat = bitsByte12[0];
					NPC.boughtDog = bitsByte12[1];
					NPC.boughtBunny = bitsByte12[2];
					NPC.freeCake = bitsByte12[3];
					Main.drunkWorld = bitsByte12[4];
					NPC.downedEmpressOfLight = bitsByte12[5];
					NPC.downedQueenSlime = bitsByte12[6];
					Main.getGoodWorld = bitsByte12[7];
					BitsByte bitsByte13 = reader.ReadByte();
					Main.tenthAnniversaryWorld = bitsByte13[0];
					Main.dontStarveWorld = bitsByte13[1];
					NPC.downedDeerclops = bitsByte13[2];
					Main.notTheBeesWorld = bitsByte13[3];
					Main.remixWorld = bitsByte13[4];
					NPC.unlockedSlimeBlueSpawn = bitsByte13[5];
					NPC.combatBookVolumeTwoWasUsed = bitsByte13[6];
					NPC.peddlersSatchelWasUsed = bitsByte13[7];
					BitsByte bitsByte14 = reader.ReadByte();
					NPC.unlockedSlimeGreenSpawn = bitsByte14[0];
					NPC.unlockedSlimeOldSpawn = bitsByte14[1];
					NPC.unlockedSlimePurpleSpawn = bitsByte14[2];
					NPC.unlockedSlimeRainbowSpawn = bitsByte14[3];
					NPC.unlockedSlimeRedSpawn = bitsByte14[4];
					NPC.unlockedSlimeYellowSpawn = bitsByte14[5];
					NPC.unlockedSlimeCopperSpawn = bitsByte14[6];
					Main.fastForwardTimeToDusk = bitsByte14[7];
					BitsByte bitsByte15 = reader.ReadByte();
					Main.noTrapsWorld = bitsByte15[0];
					Main.zenithWorld = bitsByte15[1];
					Main.sundialCooldown = reader.ReadByte();
					Main.moondialCooldown = reader.ReadByte();
					WorldGen.SavedOreTiers.Copper = reader.ReadInt16();
					WorldGen.SavedOreTiers.Iron = reader.ReadInt16();
					WorldGen.SavedOreTiers.Silver = reader.ReadInt16();
					WorldGen.SavedOreTiers.Gold = reader.ReadInt16();
					WorldGen.SavedOreTiers.Cobalt = reader.ReadInt16();
					WorldGen.SavedOreTiers.Mythril = reader.ReadInt16();
					WorldGen.SavedOreTiers.Adamantite = reader.ReadInt16();
					if (num18)
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
				int num65 = reader.ReadInt32();
				int num66 = reader.ReadInt32();
				bool flag5 = true;
				if (num65 == -1 || num66 == -1)
				{
					flag5 = false;
				}
				else if (num65 < 10 || num65 > Main.maxTilesX - 10)
				{
					flag5 = false;
				}
				else if (num66 < 10 || num66 > Main.maxTilesY - 10)
				{
					flag5 = false;
				}
				int num67 = Netplay.GetSectionX(Main.spawnTileX) - 2;
				int num68 = Netplay.GetSectionY(Main.spawnTileY) - 1;
				int num69 = num67 + 5;
				int num70 = num68 + 3;
				if (num67 < 0)
				{
					num67 = 0;
				}
				if (num69 >= Main.maxSectionsX)
				{
					num69 = Main.maxSectionsX;
				}
				if (num68 < 0)
				{
					num68 = 0;
				}
				if (num70 >= Main.maxSectionsY)
				{
					num70 = Main.maxSectionsY;
				}
				int num71 = (num69 - num67) * (num70 - num68);
				List<Point> list = new List<Point>();
				for (int num72 = num67; num72 < num69; num72++)
				{
					for (int num73 = num68; num73 < num70; num73++)
					{
						list.Add(new Point(num72, num73));
					}
				}
				int num74 = -1;
				int num75 = -1;
				if (flag5)
				{
					num65 = Netplay.GetSectionX(num65) - 2;
					num66 = Netplay.GetSectionY(num66) - 1;
					num74 = num65 + 5;
					num75 = num66 + 3;
					if (num65 < 0)
					{
						num65 = 0;
					}
					if (num74 >= Main.maxSectionsX)
					{
						num74 = Main.maxSectionsX - 1;
					}
					if (num66 < 0)
					{
						num66 = 0;
					}
					if (num75 >= Main.maxSectionsY)
					{
						num75 = Main.maxSectionsY - 1;
					}
					for (int num76 = num65; num76 <= num74; num76++)
					{
						for (int num77 = num66; num77 <= num75; num77++)
						{
							if (num76 < num67 || num76 >= num69 || num77 < num68 || num77 >= num70)
							{
								list.Add(new Point(num76, num77));
								num71++;
							}
						}
					}
				}
				PortalHelper.SyncPortalsOnPlayerJoin(whoAmI, 1, list, out var portalSections);
				num71 += portalSections.Count;
				if (Netplay.Clients[whoAmI].State == 2)
				{
					Netplay.Clients[whoAmI].State = 3;
				}
				NetMessage.TrySendData(9, whoAmI, -1, Lang.inter[44].ToNetworkText(), num71);
				Netplay.Clients[whoAmI].StatusText2 = Language.GetTextValue("Net.IsReceivingTileData");
				Netplay.Clients[whoAmI].StatusMax += num71;
				for (int num78 = num67; num78 < num69; num78++)
				{
					for (int num79 = num68; num79 < num70; num79++)
					{
						NetMessage.SendSection(whoAmI, num78, num79);
					}
				}
				if (flag5)
				{
					for (int num80 = num65; num80 <= num74; num80++)
					{
						for (int num81 = num66; num81 <= num75; num81++)
						{
							NetMessage.SendSection(whoAmI, num80, num81);
						}
					}
				}
				for (int num82 = 0; num82 < portalSections.Count; num82++)
				{
					NetMessage.SendSection(whoAmI, portalSections[num82].X, portalSections[num82].Y);
				}
				for (int num83 = 0; num83 < 400; num83++)
				{
					if (Main.item[num83].active)
					{
						NetMessage.TrySendData(21, whoAmI, -1, null, num83);
						NetMessage.TrySendData(22, whoAmI, -1, null, num83);
					}
				}
				for (int num84 = 0; num84 < 200; num84++)
				{
					if (Main.npc[num84].active)
					{
						NetMessage.TrySendData(23, whoAmI, -1, null, num84);
					}
				}
				for (int num85 = 0; num85 < 1000; num85++)
				{
					if (Main.projectile[num85].active && (Main.projPet[Main.projectile[num85].type] || Main.projectile[num85].netImportant))
					{
						NetMessage.TrySendData(27, whoAmI, -1, null, num85);
					}
				}
				for (int num86 = 0; num86 < 290; num86++)
				{
					NetMessage.TrySendData(83, whoAmI, -1, null, num86);
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
				int num233 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num233 = whoAmI;
				}
				Player player14 = Main.player[num233];
				player14.SpawnX = reader.ReadInt16();
				player14.SpawnY = reader.ReadInt16();
				player14.respawnTimer = reader.ReadInt32();
				player14.numberOfDeathsPVE = reader.ReadInt16();
				player14.numberOfDeathsPVP = reader.ReadInt16();
				if (player14.respawnTimer > 0)
				{
					player14.dead = true;
				}
				PlayerSpawnContext playerSpawnContext = (PlayerSpawnContext)reader.ReadByte();
				player14.Spawn(playerSpawnContext);
				if (Main.netMode != 2 || Netplay.Clients[whoAmI].State < 3)
				{
					break;
				}
				if (Netplay.Clients[whoAmI].State == 3)
				{
					Netplay.Clients[whoAmI].State = 10;
					NetMessage.buffer[whoAmI].broadcast = true;
					NetMessage.SyncConnectedPlayer(whoAmI);
					bool flag14 = NetMessage.DoesPlayerSlotCountAsAHost(whoAmI);
					Main.countsAsHostForGameplay[whoAmI] = flag14;
					if (NetMessage.DoesPlayerSlotCountAsAHost(whoAmI))
					{
						NetMessage.TrySendData(139, whoAmI, -1, null, whoAmI, flag14.ToInt());
					}
					NetMessage.TrySendData(12, -1, whoAmI, null, whoAmI, (int)(byte)playerSpawnContext);
					NetMessage.TrySendData(74, whoAmI, -1, NetworkText.FromLiteral(Main.player[whoAmI].name), Main.anglerQuest);
					NetMessage.TrySendData(129, whoAmI);
					NetMessage.greetPlayer(whoAmI);
					if (Main.player[num233].unlockedBiomeTorches)
					{
						NPC nPC6 = new NPC();
						nPC6.SetDefaults(664);
						Main.BestiaryTracker.Kills.RegisterKill(nPC6);
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
				int num106 = reader.ReadByte();
				if (num106 != Main.myPlayer || Main.ServerSideCharacter)
				{
					if (Main.netMode == 2)
					{
						num106 = whoAmI;
					}
					Player player7 = Main.player[num106];
					BitsByte bitsByte21 = reader.ReadByte();
					BitsByte bitsByte22 = reader.ReadByte();
					BitsByte bitsByte23 = reader.ReadByte();
					BitsByte bitsByte24 = reader.ReadByte();
					player7.controlUp = bitsByte21[0];
					player7.controlDown = bitsByte21[1];
					player7.controlLeft = bitsByte21[2];
					player7.controlRight = bitsByte21[3];
					player7.controlJump = bitsByte21[4];
					player7.controlUseItem = bitsByte21[5];
					player7.direction = (bitsByte21[6] ? 1 : (-1));
					if (bitsByte22[0])
					{
						player7.pulley = true;
						player7.pulleyDir = (byte)((!bitsByte22[1]) ? 1u : 2u);
					}
					else
					{
						player7.pulley = false;
					}
					player7.vortexStealthActive = bitsByte22[3];
					player7.gravDir = (bitsByte22[4] ? 1 : (-1));
					player7.TryTogglingShield(bitsByte22[5]);
					player7.ghost = bitsByte22[6];
					player7.selectedItem = reader.ReadByte();
					player7.position = reader.ReadVector2();
					if (bitsByte22[2])
					{
						player7.velocity = reader.ReadVector2();
					}
					else
					{
						player7.velocity = Vector2.Zero;
					}
					if (bitsByte23[6])
					{
						player7.PotionOfReturnOriginalUsePosition = reader.ReadVector2();
						player7.PotionOfReturnHomePosition = reader.ReadVector2();
					}
					else
					{
						player7.PotionOfReturnOriginalUsePosition = null;
						player7.PotionOfReturnHomePosition = null;
					}
					player7.tryKeepingHoveringUp = bitsByte23[0];
					player7.IsVoidVaultEnabled = bitsByte23[1];
					player7.sitting.isSitting = bitsByte23[2];
					player7.downedDD2EventAnyDifficulty = bitsByte23[3];
					player7.isPettingAnimal = bitsByte23[4];
					player7.isTheAnimalBeingPetSmall = bitsByte23[5];
					player7.tryKeepingHoveringDown = bitsByte23[7];
					player7.sleeping.SetIsSleepingAndAdjustPlayerRotation(player7, bitsByte24[0]);
					player7.autoReuseAllWeapons = bitsByte24[1];
					player7.controlDownHold = bitsByte24[2];
					player7.isOperatingAnotherEntity = bitsByte24[3];
					if (Main.netMode == 2 && Netplay.Clients[whoAmI].State == 10)
					{
						NetMessage.TrySendData(13, -1, whoAmI, null, num106);
					}
				}
				break;
			}
			case 14:
			{
				int num24 = reader.ReadByte();
				int num25 = reader.ReadByte();
				if (Main.netMode != 1)
				{
					break;
				}
				bool active = Main.player[num24].active;
				if (num25 == 1)
				{
					if (!Main.player[num24].active)
					{
						Main.player[num24] = new Player();
					}
					Main.player[num24].active = true;
				}
				else
				{
					Main.player[num24].active = false;
				}
				if (active != Main.player[num24].active)
				{
					if (Main.player[num24].active)
					{
						Player.Hooks.PlayerConnect(num24);
					}
					else
					{
						Player.Hooks.PlayerDisconnect(num24);
					}
				}
				break;
			}
			case 16:
			{
				int num104 = reader.ReadByte();
				if (num104 != Main.myPlayer || Main.ServerSideCharacter)
				{
					if (Main.netMode == 2)
					{
						num104 = whoAmI;
					}
					Player player6 = Main.player[num104];
					player6.statLife = reader.ReadInt16();
					player6.statLifeMax = reader.ReadInt16();
					if (player6.statLifeMax < 100)
					{
						player6.statLifeMax = 100;
					}
					player6.dead = player6.statLife <= 0;
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(16, -1, whoAmI, null, num104);
					}
				}
				break;
			}
			case 17:
			{
				byte b8 = reader.ReadByte();
				int num154 = reader.ReadInt16();
				int num155 = reader.ReadInt16();
				short num156 = reader.ReadInt16();
				int num157 = reader.ReadByte();
				bool flag10 = num156 == 1;
				if (!WorldGen.InWorld(num154, num155, 3))
				{
					break;
				}
				if (Main.tile[num154, num155] == null)
				{
					Main.tile[num154, num155] = new Tile();
				}
				if (Main.netMode == 2)
				{
					if (!flag10)
					{
						if (b8 == 0 || b8 == 2 || b8 == 4)
						{
							Netplay.Clients[whoAmI].SpamDeleteBlock += 1f;
						}
						if (b8 == 1 || b8 == 3)
						{
							Netplay.Clients[whoAmI].SpamAddBlock += 1f;
						}
					}
					if (!Netplay.Clients[whoAmI].TileSections[Netplay.GetSectionX(num154), Netplay.GetSectionY(num155)])
					{
						flag10 = true;
					}
				}
				if (b8 == 0)
				{
					WorldGen.KillTile(num154, num155, flag10);
					if (Main.netMode == 1 && !flag10)
					{
						HitTile.ClearAllTilesAtThisLocation(num154, num155);
					}
				}
				bool flag11 = false;
				if (b8 == 1)
				{
					bool forced = true;
					if (WorldGen.CheckTileBreakability2_ShouldTileSurvive(num154, num155))
					{
						flag11 = true;
						forced = false;
					}
					WorldGen.PlaceTile(num154, num155, num156, mute: false, forced, -1, num157);
				}
				if (b8 == 2)
				{
					WorldGen.KillWall(num154, num155, flag10);
				}
				if (b8 == 3)
				{
					WorldGen.PlaceWall(num154, num155, num156);
				}
				if (b8 == 4)
				{
					WorldGen.KillTile(num154, num155, flag10, effectOnly: false, noItem: true);
				}
				if (b8 == 5)
				{
					WorldGen.PlaceWire(num154, num155);
				}
				if (b8 == 6)
				{
					WorldGen.KillWire(num154, num155);
				}
				if (b8 == 7)
				{
					WorldGen.PoundTile(num154, num155);
				}
				if (b8 == 8)
				{
					WorldGen.PlaceActuator(num154, num155);
				}
				if (b8 == 9)
				{
					WorldGen.KillActuator(num154, num155);
				}
				if (b8 == 10)
				{
					WorldGen.PlaceWire2(num154, num155);
				}
				if (b8 == 11)
				{
					WorldGen.KillWire2(num154, num155);
				}
				if (b8 == 12)
				{
					WorldGen.PlaceWire3(num154, num155);
				}
				if (b8 == 13)
				{
					WorldGen.KillWire3(num154, num155);
				}
				if (b8 == 14)
				{
					WorldGen.SlopeTile(num154, num155, num156);
				}
				if (b8 == 15)
				{
					Minecart.FrameTrack(num154, num155, pound: true);
				}
				if (b8 == 16)
				{
					WorldGen.PlaceWire4(num154, num155);
				}
				if (b8 == 17)
				{
					WorldGen.KillWire4(num154, num155);
				}
				switch (b8)
				{
				case 18:
					Wiring.SetCurrentUser(whoAmI);
					Wiring.PokeLogicGate(num154, num155);
					Wiring.SetCurrentUser();
					return;
				case 19:
					Wiring.SetCurrentUser(whoAmI);
					Wiring.Actuate(num154, num155);
					Wiring.SetCurrentUser();
					return;
				case 20:
					if (WorldGen.InWorld(num154, num155, 2))
					{
						int type10 = Main.tile[num154, num155].type;
						WorldGen.KillTile(num154, num155, flag10);
						num156 = (short)((Main.tile[num154, num155].active() && Main.tile[num154, num155].type == type10) ? 1 : 0);
						if (Main.netMode == 2)
						{
							NetMessage.TrySendData(17, -1, -1, null, b8, num154, num155, num156, num157);
						}
					}
					return;
				case 21:
					WorldGen.ReplaceTile(num154, num155, (ushort)num156, num157);
					break;
				}
				if (b8 == 22)
				{
					WorldGen.ReplaceWall(num154, num155, (ushort)num156);
				}
				if (b8 == 23)
				{
					WorldGen.SlopeTile(num154, num155, num156);
					WorldGen.PoundTile(num154, num155);
				}
				if (Main.netMode != 2)
				{
					break;
				}
				if (flag11)
				{
					NetMessage.SendTileSquare(-1, num154, num155, 5);
					break;
				}
				NetMessage.TrySendData(17, -1, whoAmI, null, b8, num154, num155, num156, num157);
				if ((b8 == 1 || b8 == 21) && TileID.Sets.Falling[num156])
				{
					NetMessage.SendTileSquare(-1, num154, num155);
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
				byte b5 = reader.ReadByte();
				int num97 = reader.ReadInt16();
				int num98 = reader.ReadInt16();
				if (WorldGen.InWorld(num97, num98, 3))
				{
					int num99 = ((reader.ReadByte() != 0) ? 1 : (-1));
					switch (b5)
					{
					case 0:
						WorldGen.OpenDoor(num97, num98, num99);
						break;
					case 1:
						WorldGen.CloseDoor(num97, num98, forced: true);
						break;
					case 2:
						WorldGen.ShiftTrapdoor(num97, num98, num99 == 1, 1);
						break;
					case 3:
						WorldGen.ShiftTrapdoor(num97, num98, num99 == 1, 0);
						break;
					case 4:
						WorldGen.ShiftTallGate(num97, num98, closing: false, forced: true);
						break;
					case 5:
						WorldGen.ShiftTallGate(num97, num98, closing: true, forced: true);
						break;
					}
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(19, -1, whoAmI, null, b5, num97, num98, (num99 == 1) ? 1 : 0);
					}
				}
				break;
			}
			case 20:
			{
				int num11 = reader.ReadInt16();
				int num12 = reader.ReadInt16();
				ushort num13 = reader.ReadByte();
				ushort num14 = reader.ReadByte();
				byte b2 = reader.ReadByte();
				if (!WorldGen.InWorld(num11, num12, 3))
				{
					break;
				}
				TileChangeType type = TileChangeType.None;
				if (Enum.IsDefined(typeof(TileChangeType), b2))
				{
					type = (TileChangeType)b2;
				}
				if (MessageBuffer.OnTileChangeReceived != null)
				{
					MessageBuffer.OnTileChangeReceived(num11, num12, Math.Max(num13, num14), type);
				}
				BitsByte bitsByte2 = (byte)0;
				BitsByte bitsByte3 = (byte)0;
				BitsByte bitsByte4 = (byte)0;
				Tile tile = null;
				for (int k = num11; k < num11 + num13; k++)
				{
					for (int l = num12; l < num12 + num14; l++)
					{
						if (Main.tile[k, l] == null)
						{
							Main.tile[k, l] = new Tile();
						}
						tile = Main.tile[k, l];
						bool flag = tile.active();
						bitsByte2 = reader.ReadByte();
						bitsByte3 = reader.ReadByte();
						bitsByte4 = reader.ReadByte();
						tile.active(bitsByte2[0]);
						tile.wall = (byte)(bitsByte2[2] ? 1u : 0u);
						bool flag2 = bitsByte2[3];
						if (Main.netMode != 2)
						{
							tile.liquid = (byte)(flag2 ? 1u : 0u);
						}
						tile.wire(bitsByte2[4]);
						tile.halfBrick(bitsByte2[5]);
						tile.actuator(bitsByte2[6]);
						tile.inActive(bitsByte2[7]);
						tile.wire2(bitsByte3[0]);
						tile.wire3(bitsByte3[1]);
						if (bitsByte3[2])
						{
							tile.color(reader.ReadByte());
						}
						if (bitsByte3[3])
						{
							tile.wallColor(reader.ReadByte());
						}
						if (tile.active())
						{
							int type2 = tile.type;
							tile.type = reader.ReadUInt16();
							if (Main.tileFrameImportant[tile.type])
							{
								tile.frameX = reader.ReadInt16();
								tile.frameY = reader.ReadInt16();
							}
							else if (!flag || tile.type != type2)
							{
								tile.frameX = -1;
								tile.frameY = -1;
							}
							byte b3 = 0;
							if (bitsByte3[4])
							{
								b3 = (byte)(b3 + 1);
							}
							if (bitsByte3[5])
							{
								b3 = (byte)(b3 + 2);
							}
							if (bitsByte3[6])
							{
								b3 = (byte)(b3 + 4);
							}
							tile.slope(b3);
						}
						tile.wire4(bitsByte3[7]);
						tile.fullbrightBlock(bitsByte4[0]);
						tile.fullbrightWall(bitsByte4[1]);
						tile.invisibleBlock(bitsByte4[2]);
						tile.invisibleWall(bitsByte4[3]);
						if (tile.wall > 0)
						{
							tile.wall = reader.ReadUInt16();
						}
						if (flag2)
						{
							tile.liquid = reader.ReadByte();
							tile.liquidType(reader.ReadByte());
						}
					}
				}
				WorldGen.RangeFrame(num11, num12, num11 + num13, num12 + num14);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, whoAmI, null, num11, num12, (int)num13, (int)num14, b2);
				}
				break;
			}
			case 21:
			case 90:
			case 145:
			{
				int num115 = reader.ReadInt16();
				Vector2 position3 = reader.ReadVector2();
				Vector2 velocity3 = reader.ReadVector2();
				int stack3 = reader.ReadInt16();
				int prefixWeWant2 = reader.ReadByte();
				int num116 = reader.ReadByte();
				int num117 = reader.ReadInt16();
				bool shimmered = false;
				float shimmerTime = 0f;
				if (b == 145)
				{
					shimmered = reader.ReadBoolean();
					shimmerTime = reader.ReadSingle();
				}
				if (Main.netMode == 1)
				{
					if (num117 == 0)
					{
						Main.item[num115].active = false;
						break;
					}
					int num118 = num115;
					Item item2 = Main.item[num118];
					ItemSyncPersistentStats itemSyncPersistentStats = default(ItemSyncPersistentStats);
					itemSyncPersistentStats.CopyFrom(item2);
					bool newAndShiny = (item2.newAndShiny || item2.netID != num117) && ItemSlot.Options.HighlightNewItems && (num117 < 0 || num117 >= 5453 || !ItemID.Sets.NeverAppearsAsNewInInventory[num117]);
					item2.netDefaults(num117);
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
					item2.wet = Collision.WetCollision(item2.position, item2.width, item2.height);
					itemSyncPersistentStats.PasteInto(item2);
				}
				else
				{
					if (Main.timeItemSlotCannotBeReusedFor[num115] > 0)
					{
						break;
					}
					if (num117 == 0)
					{
						if (num115 < 400)
						{
							Main.item[num115].active = false;
							NetMessage.TrySendData(21, -1, -1, null, num115);
						}
						break;
					}
					bool flag6 = false;
					if (num115 == 400)
					{
						flag6 = true;
					}
					if (flag6)
					{
						Item item3 = new Item();
						item3.netDefaults(num117);
						num115 = Item.NewItem(new EntitySource_Sync(), (int)position3.X, (int)position3.Y, item3.width, item3.height, item3.type, stack3, noBroadcast: true);
					}
					Item item4 = Main.item[num115];
					item4.netDefaults(num117);
					item4.Prefix(prefixWeWant2);
					item4.stack = stack3;
					item4.position = position3;
					item4.velocity = velocity3;
					item4.active = true;
					item4.playerIndexTheItemIsReservedFor = Main.myPlayer;
					if (b == 145)
					{
						item4.shimmered = shimmered;
						item4.shimmerTime = shimmerTime;
					}
					if (flag6)
					{
						NetMessage.TrySendData(b, -1, -1, null, num115);
						if (num116 == 0)
						{
							Main.item[num115].ownIgnore = whoAmI;
							Main.item[num115].ownTime = 100;
						}
						Main.item[num115].FindOwner(num115);
					}
					else
					{
						NetMessage.TrySendData(b, -1, whoAmI, null, num115);
					}
				}
				break;
			}
			case 22:
			{
				int num48 = reader.ReadInt16();
				int num49 = reader.ReadByte();
				if (Main.netMode != 2 || Main.item[num48].playerIndexTheItemIsReservedFor == whoAmI)
				{
					Main.item[num48].playerIndexTheItemIsReservedFor = num49;
					if (num49 == Main.myPlayer)
					{
						Main.item[num48].keepTime = 15;
					}
					else
					{
						Main.item[num48].keepTime = 0;
					}
					if (Main.netMode == 2)
					{
						Main.item[num48].playerIndexTheItemIsReservedFor = 255;
						Main.item[num48].keepTime = 15;
						NetMessage.TrySendData(22, -1, -1, null, num48);
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
				int num169 = reader.ReadInt16();
				Vector2 vector5 = reader.ReadVector2();
				Vector2 velocity5 = reader.ReadVector2();
				int num170 = reader.ReadUInt16();
				if (num170 == 65535)
				{
					num170 = 0;
				}
				BitsByte bitsByte25 = reader.ReadByte();
				BitsByte bitsByte26 = reader.ReadByte();
				float[] array2 = ReUseTemporaryNPCAI();
				for (int num171 = 0; num171 < NPC.maxAI; num171++)
				{
					if (bitsByte25[num171 + 2])
					{
						array2[num171] = reader.ReadSingle();
					}
					else
					{
						array2[num171] = 0f;
					}
				}
				int num172 = reader.ReadInt16();
				int? playerCountForMultiplayerDifficultyOverride = 1;
				if (bitsByte26[0])
				{
					playerCountForMultiplayerDifficultyOverride = reader.ReadByte();
				}
				float value8 = 1f;
				if (bitsByte26[2])
				{
					value8 = reader.ReadSingle();
				}
				int num173 = 0;
				if (!bitsByte25[7])
				{
					num173 = reader.ReadByte() switch
					{
						2 => reader.ReadInt16(), 
						4 => reader.ReadInt32(), 
						_ => reader.ReadSByte(), 
					};
				}
				int num174 = -1;
				NPC nPC4 = Main.npc[num169];
				if (nPC4.active && Main.multiplayerNPCSmoothingRange > 0 && Vector2.DistanceSquared(nPC4.position, vector5) < 640000f)
				{
					nPC4.netOffset += nPC4.position - vector5;
				}
				if (!nPC4.active || nPC4.netID != num172)
				{
					nPC4.netOffset *= 0f;
					if (nPC4.active)
					{
						num174 = nPC4.type;
					}
					nPC4.active = true;
					nPC4.SetDefaults(num172, new NPCSpawnParams
					{
						playerCountForMultiplayerDifficultyOverride = playerCountForMultiplayerDifficultyOverride,
						strengthMultiplierOverride = value8
					});
				}
				nPC4.position = vector5;
				nPC4.velocity = velocity5;
				nPC4.target = num170;
				nPC4.direction = (bitsByte25[0] ? 1 : (-1));
				nPC4.directionY = (bitsByte25[1] ? 1 : (-1));
				nPC4.spriteDirection = (bitsByte25[6] ? 1 : (-1));
				if (bitsByte25[7])
				{
					num173 = (nPC4.life = nPC4.lifeMax);
				}
				else
				{
					nPC4.life = num173;
				}
				if (num173 <= 0)
				{
					nPC4.active = false;
				}
				nPC4.SpawnedFromStatue = bitsByte26[1];
				if (nPC4.SpawnedFromStatue)
				{
					nPC4.value = 0f;
				}
				for (int num175 = 0; num175 < NPC.maxAI; num175++)
				{
					nPC4.ai[num175] = array2[num175];
				}
				if (num174 > -1 && num174 != nPC4.type)
				{
					nPC4.TransformVisuals(num174, nPC4.type);
				}
				if (num172 == 262)
				{
					NPC.plantBoss = num169;
				}
				if (num172 == 245)
				{
					NPC.golemBoss = num169;
				}
				if (num172 == 668)
				{
					NPC.deerclopsBoss = num169;
				}
				if (nPC4.type >= 0 && nPC4.type < 688 && Main.npcCatchable[nPC4.type])
				{
					nPC4.releaseOwner = reader.ReadByte();
				}
				break;
			}
			case 24:
			{
				int num133 = reader.ReadInt16();
				int num134 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num134 = whoAmI;
				}
				Player player10 = Main.player[num134];
				Main.npc[num133].StrikeNPC(player10.inventory[player10.selectedItem].damage, player10.inventory[player10.selectedItem].knockBack, player10.direction);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(24, -1, whoAmI, null, num133, num134);
					NetMessage.TrySendData(23, -1, -1, null, num133);
				}
				break;
			}
			case 27:
			{
				int num37 = reader.ReadInt16();
				Vector2 position = reader.ReadVector2();
				Vector2 velocity2 = reader.ReadVector2();
				int num38 = reader.ReadByte();
				int num39 = reader.ReadInt16();
				BitsByte bitsByte16 = reader.ReadByte();
				BitsByte bitsByte17 = (byte)(bitsByte16[2] ? reader.ReadByte() : 0);
				float[] array = ReUseTemporaryProjectileAI();
				array[0] = (bitsByte16[0] ? reader.ReadSingle() : 0f);
				array[1] = (bitsByte16[1] ? reader.ReadSingle() : 0f);
				int bannerIdToRespondTo = (bitsByte16[3] ? reader.ReadUInt16() : 0);
				int damage2 = (bitsByte16[4] ? reader.ReadInt16() : 0);
				float knockBack2 = (bitsByte16[5] ? reader.ReadSingle() : 0f);
				int originalDamage = (bitsByte16[6] ? reader.ReadInt16() : 0);
				int num40 = (bitsByte16[7] ? reader.ReadInt16() : (-1));
				if (num40 >= 1000)
				{
					num40 = -1;
				}
				array[2] = (bitsByte17[0] ? reader.ReadSingle() : 0f);
				if (Main.netMode == 2)
				{
					if (num39 == 949)
					{
						num38 = 255;
					}
					else
					{
						num38 = whoAmI;
						if (Main.projHostile[num39])
						{
							break;
						}
					}
				}
				int num41 = 1000;
				for (int num42 = 0; num42 < 1000; num42++)
				{
					if (Main.projectile[num42].owner == num38 && Main.projectile[num42].identity == num37 && Main.projectile[num42].active)
					{
						num41 = num42;
						break;
					}
				}
				if (num41 == 1000)
				{
					for (int num43 = 0; num43 < 1000; num43++)
					{
						if (!Main.projectile[num43].active)
						{
							num41 = num43;
							break;
						}
					}
				}
				if (num41 == 1000)
				{
					num41 = Projectile.FindOldestProjectile();
				}
				Projectile projectile = Main.projectile[num41];
				if (!projectile.active || projectile.type != num39)
				{
					projectile.SetDefaults(num39);
					if (Main.netMode == 2)
					{
						Netplay.Clients[whoAmI].SpamProjectile += 1f;
					}
				}
				projectile.identity = num37;
				projectile.position = position;
				projectile.velocity = velocity2;
				projectile.type = num39;
				projectile.damage = damage2;
				projectile.bannerIdToRespondTo = bannerIdToRespondTo;
				projectile.originalDamage = originalDamage;
				projectile.knockBack = knockBack2;
				projectile.owner = num38;
				for (int num44 = 0; num44 < Projectile.maxAI; num44++)
				{
					projectile.ai[num44] = array[num44];
				}
				if (num40 >= 0)
				{
					projectile.projUUID = num40;
					Main.projectileIdentity[num38, num40] = num41;
				}
				projectile.ProjectileFixDesperation();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(27, -1, whoAmI, null, num41);
				}
				break;
			}
			case 28:
			{
				int num201 = reader.ReadInt16();
				int num202 = reader.ReadInt16();
				float num203 = reader.ReadSingle();
				int num204 = reader.ReadByte() - 1;
				byte b13 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					if (num202 < 0)
					{
						num202 = 0;
					}
					Main.npc[num201].PlayerInteraction(whoAmI);
				}
				if (num202 >= 0)
				{
					Main.npc[num201].StrikeNPC(num202, num203, num204, b13 == 1, noEffect: false, fromNet: true);
				}
				else
				{
					Main.npc[num201].life = 0;
					Main.npc[num201].HitEffect();
					Main.npc[num201].active = false;
				}
				if (Main.netMode != 2)
				{
					break;
				}
				NetMessage.TrySendData(28, -1, whoAmI, null, num201, num202, num203, num204, b13);
				if (Main.npc[num201].life <= 0)
				{
					NetMessage.TrySendData(23, -1, -1, null, num201);
				}
				else
				{
					Main.npc[num201].netUpdate = true;
				}
				if (Main.npc[num201].realLife >= 0)
				{
					if (Main.npc[Main.npc[num201].realLife].life <= 0)
					{
						NetMessage.TrySendData(23, -1, -1, null, Main.npc[num201].realLife);
					}
					else
					{
						Main.npc[Main.npc[num201].realLife].netUpdate = true;
					}
				}
				break;
			}
			case 29:
			{
				int num112 = reader.ReadInt16();
				int num113 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num113 = whoAmI;
				}
				for (int num114 = 0; num114 < 1000; num114++)
				{
					if (Main.projectile[num114].owner == num113 && Main.projectile[num114].identity == num112 && Main.projectile[num114].active)
					{
						Main.projectile[num114].Kill();
						break;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(29, -1, whoAmI, null, num112, num113);
				}
				break;
			}
			case 30:
			{
				int num151 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num151 = whoAmI;
				}
				bool flag9 = reader.ReadBoolean();
				Main.player[num151].hostile = flag9;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(30, -1, whoAmI, null, num151);
					LocalizedText obj4 = (flag9 ? Lang.mp[11] : Lang.mp[12]);
					ChatHelper.BroadcastChatMessage(color: Main.teamColor[Main.player[num151].team], text: NetworkText.FromKey(obj4.Key, Main.player[num151].name));
				}
				break;
			}
			case 31:
			{
				if (Main.netMode != 2)
				{
					break;
				}
				int num224 = reader.ReadInt16();
				int num225 = reader.ReadInt16();
				int num226 = Chest.FindChest(num224, num225);
				if (num226 > -1 && Chest.UsingChest(num226) == -1)
				{
					for (int num227 = 0; num227 < 40; num227++)
					{
						NetMessage.TrySendData(32, whoAmI, -1, null, num226, num227);
					}
					NetMessage.TrySendData(33, whoAmI, -1, null, num226);
					Main.player[whoAmI].chest = num226;
					if (Main.myPlayer == whoAmI)
					{
						Main.recBigList = false;
					}
					NetMessage.TrySendData(80, -1, whoAmI, null, whoAmI, num226);
					if (Main.netMode == 2 && WorldGen.IsChestRigged(num224, num225))
					{
						Wiring.SetCurrentUser(whoAmI);
						Wiring.HitSwitch(num224, num225);
						Wiring.SetCurrentUser();
						NetMessage.TrySendData(59, -1, whoAmI, null, num224, num225);
					}
				}
				break;
			}
			case 32:
			{
				int num179 = reader.ReadInt16();
				int num180 = reader.ReadByte();
				int stack5 = reader.ReadInt16();
				int prefixWeWant3 = reader.ReadByte();
				int type11 = reader.ReadInt16();
				if (num179 >= 0 && num179 < 8000)
				{
					if (Main.chest[num179] == null)
					{
						Main.chest[num179] = new Chest();
					}
					if (Main.chest[num179].item[num180] == null)
					{
						Main.chest[num179].item[num180] = new Item();
					}
					Main.chest[num179].item[num180].netDefaults(type11);
					Main.chest[num179].item[num180].Prefix(prefixWeWant3);
					Main.chest[num179].item[num180].stack = stack5;
					Recipe.FindRecipes(canDelayCheck: true);
				}
				break;
			}
			case 33:
			{
				int num260 = reader.ReadInt16();
				int num261 = reader.ReadInt16();
				int num262 = reader.ReadInt16();
				int num263 = reader.ReadByte();
				string name2 = string.Empty;
				if (num263 != 0)
				{
					if (num263 <= 20)
					{
						name2 = reader.ReadString();
					}
					else if (num263 != 255)
					{
						num263 = 0;
					}
				}
				if (Main.netMode == 1)
				{
					Player player17 = Main.player[Main.myPlayer];
					if (player17.chest == -1)
					{
						Main.playerInventory = true;
						SoundEngine.PlaySound(10);
					}
					else if (player17.chest != num260 && num260 != -1)
					{
						Main.playerInventory = true;
						SoundEngine.PlaySound(12);
						Main.recBigList = false;
					}
					else if (player17.chest != -1 && num260 == -1)
					{
						SoundEngine.PlaySound(11);
						Main.recBigList = false;
					}
					player17.chest = num260;
					player17.chestX = num261;
					player17.chestY = num262;
					Recipe.FindRecipes(canDelayCheck: true);
					if (Main.tile[num261, num262].frameX >= 36 && Main.tile[num261, num262].frameX < 72)
					{
						AchievementsHelper.HandleSpecialEvent(Main.player[Main.myPlayer], 16);
					}
				}
				else
				{
					if (num263 != 0)
					{
						int chest3 = Main.player[whoAmI].chest;
						Chest chest4 = Main.chest[chest3];
						chest4.name = name2;
						NetMessage.TrySendData(69, -1, whoAmI, null, chest3, chest4.x, chest4.y);
					}
					Main.player[whoAmI].chest = num260;
					Recipe.FindRecipes(canDelayCheck: true);
					NetMessage.TrySendData(80, -1, whoAmI, null, whoAmI, num260);
				}
				break;
			}
			case 34:
			{
				byte b14 = reader.ReadByte();
				int num206 = reader.ReadInt16();
				int num207 = reader.ReadInt16();
				int num208 = reader.ReadInt16();
				int num209 = reader.ReadInt16();
				if (Main.netMode == 2)
				{
					num209 = 0;
				}
				if (Main.netMode == 2)
				{
					switch (b14)
					{
					case 0:
					{
						int num212 = WorldGen.PlaceChest(num206, num207, 21, notNearOtherChests: false, num208);
						if (num212 == -1)
						{
							NetMessage.TrySendData(34, whoAmI, -1, null, b14, num206, num207, num208, num212);
							Item.NewItem(new EntitySource_TileBreak(num206, num207), num206 * 16, num207 * 16, 32, 32, Chest.chestItemSpawn[num208], 1, noBroadcast: true);
						}
						else
						{
							NetMessage.TrySendData(34, -1, -1, null, b14, num206, num207, num208, num212);
						}
						break;
					}
					case 1:
						if (Main.tile[num206, num207].type == 21)
						{
							Tile tile2 = Main.tile[num206, num207];
							if (tile2.frameX % 36 != 0)
							{
								num206--;
							}
							if (tile2.frameY % 36 != 0)
							{
								num207--;
							}
							int number = Chest.FindChest(num206, num207);
							WorldGen.KillTile(num206, num207);
							if (!tile2.active())
							{
								NetMessage.TrySendData(34, -1, -1, null, b14, num206, num207, 0f, number);
							}
							break;
						}
						goto default;
					default:
						switch (b14)
						{
						case 2:
						{
							int num210 = WorldGen.PlaceChest(num206, num207, 88, notNearOtherChests: false, num208);
							if (num210 == -1)
							{
								NetMessage.TrySendData(34, whoAmI, -1, null, b14, num206, num207, num208, num210);
								Item.NewItem(new EntitySource_TileBreak(num206, num207), num206 * 16, num207 * 16, 32, 32, Chest.dresserItemSpawn[num208], 1, noBroadcast: true);
							}
							else
							{
								NetMessage.TrySendData(34, -1, -1, null, b14, num206, num207, num208, num210);
							}
							break;
						}
						case 3:
							if (Main.tile[num206, num207].type == 88)
							{
								Tile tile3 = Main.tile[num206, num207];
								num206 -= tile3.frameX % 54 / 18;
								if (tile3.frameY % 36 != 0)
								{
									num207--;
								}
								int number2 = Chest.FindChest(num206, num207);
								WorldGen.KillTile(num206, num207);
								if (!tile3.active())
								{
									NetMessage.TrySendData(34, -1, -1, null, b14, num206, num207, 0f, number2);
								}
								break;
							}
							goto default;
						default:
							switch (b14)
							{
							case 4:
							{
								int num211 = WorldGen.PlaceChest(num206, num207, 467, notNearOtherChests: false, num208);
								if (num211 == -1)
								{
									NetMessage.TrySendData(34, whoAmI, -1, null, b14, num206, num207, num208, num211);
									Item.NewItem(new EntitySource_TileBreak(num206, num207), num206 * 16, num207 * 16, 32, 32, Chest.chestItemSpawn2[num208], 1, noBroadcast: true);
								}
								else
								{
									NetMessage.TrySendData(34, -1, -1, null, b14, num206, num207, num208, num211);
								}
								break;
							}
							case 5:
								if (Main.tile[num206, num207].type == 467)
								{
									Tile tile4 = Main.tile[num206, num207];
									if (tile4.frameX % 36 != 0)
									{
										num206--;
									}
									if (tile4.frameY % 36 != 0)
									{
										num207--;
									}
									int number3 = Chest.FindChest(num206, num207);
									WorldGen.KillTile(num206, num207);
									if (!tile4.active())
									{
										NetMessage.TrySendData(34, -1, -1, null, b14, num206, num207, 0f, number3);
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
				switch (b14)
				{
				case 0:
					if (num209 == -1)
					{
						WorldGen.KillTile(num206, num207);
						break;
					}
					SoundEngine.PlaySound(0, num206 * 16, num207 * 16);
					WorldGen.PlaceChestDirect(num206, num207, 21, num208, num209);
					break;
				case 2:
					if (num209 == -1)
					{
						WorldGen.KillTile(num206, num207);
						break;
					}
					SoundEngine.PlaySound(0, num206 * 16, num207 * 16);
					WorldGen.PlaceDresserDirect(num206, num207, 88, num208, num209);
					break;
				case 4:
					if (num209 == -1)
					{
						WorldGen.KillTile(num206, num207);
						break;
					}
					SoundEngine.PlaySound(0, num206 * 16, num207 * 16);
					WorldGen.PlaceChestDirect(num206, num207, 467, num208, num209);
					break;
				default:
					Chest.DestroyChestDirect(num206, num207, num209);
					WorldGen.KillTile(num206, num207);
					break;
				}
				break;
			}
			case 35:
			{
				int num160 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num160 = whoAmI;
				}
				int num161 = reader.ReadInt16();
				if (num160 != Main.myPlayer || Main.ServerSideCharacter)
				{
					Main.player[num160].HealEffect(num161);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(35, -1, whoAmI, null, num160, num161);
				}
				break;
			}
			case 36:
			{
				int num121 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num121 = whoAmI;
				}
				Player player8 = Main.player[num121];
				bool flag7 = player8.zone5[0];
				player8.zone1 = reader.ReadByte();
				player8.zone2 = reader.ReadByte();
				player8.zone3 = reader.ReadByte();
				player8.zone4 = reader.ReadByte();
				player8.zone5 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					if (!flag7 && player8.zone5[0])
					{
						NPC.SpawnFaelings(num121);
					}
					NetMessage.TrySendData(36, -1, whoAmI, null, num121);
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
				int num257 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num257 = whoAmI;
				}
				int npcIndex = reader.ReadInt16();
				Main.player[num257].SetTalkNPC(npcIndex, fromNet: true);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(40, -1, whoAmI, null, num257);
				}
				break;
			}
			case 41:
			{
				int num231 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num231 = whoAmI;
				}
				Player player13 = Main.player[num231];
				float itemRotation = reader.ReadSingle();
				int itemAnimation = reader.ReadInt16();
				player13.itemRotation = itemRotation;
				player13.itemAnimation = itemAnimation;
				player13.channel = player13.inventory[player13.selectedItem].channel;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(41, -1, whoAmI, null, num231);
				}
				if (Main.netMode == 1)
				{
					Item item6 = player13.inventory[player13.selectedItem];
					if (item6.UseSound != null)
					{
						SoundEngine.PlaySound(item6.UseSound, player13.Center);
					}
				}
				break;
			}
			case 42:
			{
				int num205 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num205 = whoAmI;
				}
				else if (Main.myPlayer == num205 && !Main.ServerSideCharacter)
				{
					break;
				}
				int statMana = reader.ReadInt16();
				int statManaMax = reader.ReadInt16();
				Main.player[num205].statMana = statMana;
				Main.player[num205].statManaMax = statManaMax;
				break;
			}
			case 43:
			{
				int num165 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num165 = whoAmI;
				}
				int num166 = reader.ReadInt16();
				if (num165 != Main.myPlayer)
				{
					Main.player[num165].ManaEffect(num166);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(43, -1, whoAmI, null, num165, num166);
				}
				break;
			}
			case 45:
			{
				int num130 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num130 = whoAmI;
				}
				int num131 = reader.ReadByte();
				Player player9 = Main.player[num130];
				int team = player9.team;
				player9.team = num131;
				Color color = Main.teamColor[num131];
				if (Main.netMode != 2)
				{
					break;
				}
				NetMessage.TrySendData(45, -1, whoAmI, null, num130);
				LocalizedText localizedText = Lang.mp[13 + num131];
				if (num131 == 5)
				{
					localizedText = Lang.mp[22];
				}
				for (int num132 = 0; num132 < 255; num132++)
				{
					if (num132 == whoAmI || (team > 0 && Main.player[num132].team == team) || (num131 > 0 && Main.player[num132].team == num131))
					{
						ChatHelper.SendChatMessageToClient(NetworkText.FromKey(localizedText.Key, player9.name), color, num132);
					}
				}
				break;
			}
			case 46:
				if (Main.netMode == 2)
				{
					short i3 = reader.ReadInt16();
					int j3 = reader.ReadInt16();
					int num124 = Sign.ReadSign(i3, j3);
					if (num124 >= 0)
					{
						NetMessage.TrySendData(47, whoAmI, -1, null, num124, whoAmI);
					}
				}
				break;
			case 47:
			{
				int num2 = reader.ReadInt16();
				int x = reader.ReadInt16();
				int y = reader.ReadInt16();
				string text = reader.ReadString();
				int num3 = reader.ReadByte();
				BitsByte bitsByte = reader.ReadByte();
				if (num2 >= 0 && num2 < 1000)
				{
					string text2 = null;
					if (Main.sign[num2] != null)
					{
						text2 = Main.sign[num2].text;
					}
					Main.sign[num2] = new Sign();
					Main.sign[num2].x = x;
					Main.sign[num2].y = y;
					Sign.TextSign(num2, text);
					if (Main.netMode == 2 && text2 != text)
					{
						num3 = whoAmI;
						NetMessage.TrySendData(47, -1, whoAmI, null, num2, num3);
					}
					if (Main.netMode == 1 && num3 == Main.myPlayer && Main.sign[num2] != null && !bitsByte[0])
					{
						Main.playerInventory = false;
						Main.player[Main.myPlayer].SetTalkNPC(-1, fromNet: true);
						Main.npcChatCornerItem = 0;
						Main.editSign = false;
						SoundEngine.PlaySound(10);
						Main.player[Main.myPlayer].sign = num2;
						Main.npcChatText = Main.sign[num2].text;
					}
				}
				break;
			}
			case 48:
			{
				int num237 = reader.ReadInt16();
				int num238 = reader.ReadInt16();
				byte b15 = reader.ReadByte();
				byte liquidType = reader.ReadByte();
				if (Main.netMode == 2 && Netplay.SpamCheck)
				{
					int num239 = whoAmI;
					int num240 = (int)(Main.player[num239].position.X + (float)(Main.player[num239].width / 2));
					int num241 = (int)(Main.player[num239].position.Y + (float)(Main.player[num239].height / 2));
					int num242 = 10;
					int num243 = num240 - num242;
					int num244 = num240 + num242;
					int num245 = num241 - num242;
					int num246 = num241 + num242;
					if (num237 < num243 || num237 > num244 || num238 < num245 || num238 > num246)
					{
						Netplay.Clients[whoAmI].SpamWater += 1f;
					}
				}
				if (Main.tile[num237, num238] == null)
				{
					Main.tile[num237, num238] = new Tile();
				}
				lock (Main.tile[num237, num238])
				{
					Main.tile[num237, num238].liquid = b15;
					Main.tile[num237, num238].liquidType(liquidType);
					if (Main.netMode == 2)
					{
						WorldGen.SquareTileFrame(num237, num238);
						if (b15 == 0)
						{
							NetMessage.SendData(48, -1, whoAmI, null, num237, num238);
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
				int num191 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num191 = whoAmI;
				}
				else if (num191 == Main.myPlayer && !Main.ServerSideCharacter)
				{
					break;
				}
				Player player11 = Main.player[num191];
				for (int num192 = 0; num192 < 44; num192++)
				{
					player11.buffType[num192] = reader.ReadUInt16();
					if (player11.buffType[num192] > 0)
					{
						player11.buffTime[num192] = 60;
					}
					else
					{
						player11.buffTime[num192] = 0;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(50, -1, whoAmI, null, num191);
				}
				break;
			}
			case 51:
			{
				byte b11 = reader.ReadByte();
				byte b12 = reader.ReadByte();
				switch (b12)
				{
				case 1:
					NPC.SpawnSkeletron(b11);
					break;
				case 2:
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(51, -1, whoAmI, null, b11, (int)b12);
					}
					else
					{
						SoundEngine.PlaySound(SoundID.Item1, (int)Main.player[b11].position.X, (int)Main.player[b11].position.Y);
					}
					break;
				case 3:
					if (Main.netMode == 2)
					{
						Main.Sundialing();
					}
					break;
				case 4:
					Main.npc[b11].BigMimicSpawnSmoke();
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
				int num162 = reader.ReadByte();
				int num163 = reader.ReadInt16();
				int num164 = reader.ReadInt16();
				if (num162 == 1)
				{
					Chest.Unlock(num163, num164);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(52, -1, whoAmI, null, 0, num162, num163, num164);
						NetMessage.SendTileSquare(-1, num163, num164, 2);
					}
				}
				if (num162 == 2)
				{
					WorldGen.UnlockDoor(num163, num164);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(52, -1, whoAmI, null, 0, num162, num163, num164);
						NetMessage.SendTileSquare(-1, num163, num164, 2);
					}
				}
				if (num162 == 3)
				{
					Chest.Lock(num163, num164);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(52, -1, whoAmI, null, 0, num162, num163, num164);
						NetMessage.SendTileSquare(-1, num163, num164, 2);
					}
				}
				break;
			}
			case 53:
			{
				int num135 = reader.ReadInt16();
				int type8 = reader.ReadUInt16();
				int time2 = reader.ReadInt16();
				Main.npc[num135].AddBuff(type8, time2, quiet: true);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(54, -1, -1, null, num135);
				}
				break;
			}
			case 54:
				if (Main.netMode == 1)
				{
					int num122 = reader.ReadInt16();
					NPC nPC3 = Main.npc[num122];
					for (int num123 = 0; num123 < 20; num123++)
					{
						nPC3.buffType[num123] = reader.ReadUInt16();
						nPC3.buffTime[num123] = reader.ReadInt16();
					}
				}
				break;
			case 55:
			{
				int num62 = reader.ReadByte();
				int num63 = reader.ReadUInt16();
				int num64 = reader.ReadInt32();
				if (Main.netMode != 2 || num62 == whoAmI || Main.pvpBuff[num63])
				{
					if (Main.netMode == 1 && num62 == Main.myPlayer)
					{
						Main.player[num62].AddBuff(num63, num64);
					}
					else if (Main.netMode == 2)
					{
						NetMessage.TrySendData(55, -1, -1, null, num62, num63, num64);
					}
				}
				break;
			}
			case 56:
			{
				int num28 = reader.ReadInt16();
				if (num28 >= 0 && num28 < 200)
				{
					if (Main.netMode == 1)
					{
						string givenName = reader.ReadString();
						Main.npc[num28].GivenName = givenName;
						int townNpcVariationIndex = reader.ReadInt32();
						Main.npc[num28].townNpcVariationIndex = townNpcVariationIndex;
					}
					else if (Main.netMode == 2)
					{
						NetMessage.TrySendData(56, whoAmI, -1, null, num28);
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
				int num258 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num258 = whoAmI;
				}
				float num259 = reader.ReadSingle();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(58, -1, whoAmI, null, whoAmI, num259);
					break;
				}
				Player player16 = Main.player[num258];
				int type16 = player16.inventory[player16.selectedItem].type;
				switch (type16)
				{
				case 4057:
				case 4372:
				case 4715:
					player16.PlayGuitarChord(num259);
					break;
				case 4673:
					player16.PlayDrums(num259);
					break;
				default:
				{
					Main.musicPitch = num259;
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
				int num264 = reader.ReadInt16();
				int num265 = reader.ReadInt16();
				Wiring.SetCurrentUser(whoAmI);
				Wiring.HitSwitch(num264, num265);
				Wiring.SetCurrentUser();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(59, -1, whoAmI, null, num264, num265);
				}
				break;
			}
			case 60:
			{
				int num250 = reader.ReadInt16();
				int num251 = reader.ReadInt16();
				int num252 = reader.ReadInt16();
				byte b16 = reader.ReadByte();
				if (num250 >= 200)
				{
					NetMessage.BootPlayer(whoAmI, NetworkText.FromKey("Net.CheatingInvalid"));
					break;
				}
				NPC nPC7 = Main.npc[num250];
				bool isLikeATownNPC = nPC7.isLikeATownNPC;
				if (Main.netMode == 1)
				{
					nPC7.homeless = b16 == 1;
					nPC7.homeTileX = num251;
					nPC7.homeTileY = num252;
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
						WorldGen.TownManager.KickOut(nPC7.type);
						break;
					case 2:
						WorldGen.TownManager.SetRoom(nPC7.type, num251, num252);
						break;
					}
				}
				else if (b16 == 1)
				{
					WorldGen.kickOut(num250);
				}
				else
				{
					WorldGen.moveRoom(num251, num252, num250);
				}
				break;
			}
			case 61:
			{
				int num220 = reader.ReadInt16();
				int num221 = reader.ReadInt16();
				if (Main.netMode != 2)
				{
					break;
				}
				if (num221 >= 0 && num221 < 688 && NPCID.Sets.MPAllowedEnemies[num221])
				{
					if (!NPC.AnyNPCs(num221))
					{
						NPC.SpawnOnPlayer(num220, num221);
					}
				}
				else if (num221 == -4)
				{
					if (!Main.dayTime && !DD2Event.Ongoing)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[31].Key), new Color(50, 255, 130));
						Main.startPumpkinMoon();
						NetMessage.TrySendData(7);
						NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 2f, 1f);
					}
				}
				else if (num221 == -5)
				{
					if (!Main.dayTime && !DD2Event.Ongoing)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[34].Key), new Color(50, 255, 130));
						Main.startSnowMoon();
						NetMessage.TrySendData(7);
						NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 1f, 1f);
					}
				}
				else if (num221 == -6)
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
				else if (num221 == -7)
				{
					Main.invasionDelay = 0;
					Main.StartInvasion(4);
					NetMessage.TrySendData(7);
					NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
				}
				else if (num221 == -8)
				{
					if (NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger() && !NPC.AnyoneNearCultists())
					{
						WorldGen.StartImpendingDoom(720);
						NetMessage.TrySendData(7);
					}
				}
				else if (num221 == -10)
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
				else if (num221 == -11)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookUsed"), new Color(50, 255, 130));
					NPC.combatBookWasUsed = true;
					NetMessage.TrySendData(7);
				}
				else if (num221 == -12)
				{
					NPC.UnlockOrExchangePet(ref NPC.boughtCat, 637, "Misc.LicenseCatUsed", num221);
				}
				else if (num221 == -13)
				{
					NPC.UnlockOrExchangePet(ref NPC.boughtDog, 638, "Misc.LicenseDogUsed", num221);
				}
				else if (num221 == -14)
				{
					NPC.UnlockOrExchangePet(ref NPC.boughtBunny, 656, "Misc.LicenseBunnyUsed", num221);
				}
				else if (num221 == -15)
				{
					NPC.UnlockOrExchangePet(ref NPC.unlockedSlimeBlueSpawn, 670, "Misc.LicenseSlimeUsed", num221);
				}
				else if (num221 == -16)
				{
					NPC.SpawnMechQueen(num220);
				}
				else if (num221 == -17)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookVolumeTwoUsed"), new Color(50, 255, 130));
					NPC.combatBookVolumeTwoWasUsed = true;
					NetMessage.TrySendData(7);
				}
				else if (num221 == -18)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PeddlersSatchelUsed"), new Color(50, 255, 130));
					NPC.peddlersSatchelWasUsed = true;
					NetMessage.TrySendData(7);
				}
				else if (num221 < 0)
				{
					int num222 = 1;
					if (num221 > -5)
					{
						num222 = -num221;
					}
					if (num222 > 0 && Main.invasionType == 0)
					{
						Main.invasionDelay = 0;
						Main.StartInvasion(num222);
					}
					NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
				}
				break;
			}
			case 62:
			{
				int num181 = reader.ReadByte();
				int num182 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num181 = whoAmI;
				}
				if (num182 == 1)
				{
					Main.player[num181].NinjaDodge();
				}
				if (num182 == 2)
				{
					Main.player[num181].ShadowDodge();
				}
				if (num182 == 4)
				{
					Main.player[num181].BrainOfConfusionDodge();
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(62, -1, whoAmI, null, num181, num182);
				}
				break;
			}
			case 63:
			{
				int num158 = reader.ReadInt16();
				int num159 = reader.ReadInt16();
				byte b9 = reader.ReadByte();
				byte b10 = reader.ReadByte();
				if (b10 == 0)
				{
					WorldGen.paintTile(num158, num159, b9);
				}
				else
				{
					WorldGen.paintCoatTile(num158, num159, b9);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(63, -1, whoAmI, null, num158, num159, (int)b9, (int)b10);
				}
				break;
			}
			case 64:
			{
				int num145 = reader.ReadInt16();
				int num146 = reader.ReadInt16();
				byte b6 = reader.ReadByte();
				byte b7 = reader.ReadByte();
				if (b7 == 0)
				{
					WorldGen.paintWall(num145, num146, b6);
				}
				else
				{
					WorldGen.paintCoatWall(num145, num146, b6);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(64, -1, whoAmI, null, num145, num146, (int)b6, (int)b7);
				}
				break;
			}
			case 65:
			{
				BitsByte bitsByte20 = reader.ReadByte();
				int num55 = reader.ReadInt16();
				if (Main.netMode == 2)
				{
					num55 = whoAmI;
				}
				Vector2 vector = reader.ReadVector2();
				int num56 = 0;
				num56 = reader.ReadByte();
				int num57 = 0;
				if (bitsByte20[0])
				{
					num57++;
				}
				if (bitsByte20[1])
				{
					num57 += 2;
				}
				bool flag4 = false;
				if (bitsByte20[2])
				{
					flag4 = true;
				}
				int num58 = 0;
				if (bitsByte20[3])
				{
					num58 = reader.ReadInt32();
				}
				if (flag4)
				{
					vector = Main.player[num55].position;
				}
				switch (num57)
				{
				case 0:
					Main.player[num55].Teleport(vector, num56, num58);
					break;
				case 1:
					Main.npc[num55].Teleport(vector, num56, num58);
					break;
				case 2:
				{
					Main.player[num55].Teleport(vector, num56, num58);
					if (Main.netMode != 2)
					{
						break;
					}
					RemoteClient.CheckSection(whoAmI, vector);
					NetMessage.TrySendData(65, -1, -1, null, 0, num55, vector.X, vector.Y, num56, flag4.ToInt(), num58);
					int num59 = -1;
					float num60 = 9999f;
					for (int num61 = 0; num61 < 255; num61++)
					{
						if (Main.player[num61].active && num61 != whoAmI)
						{
							Vector2 vector2 = Main.player[num61].position - Main.player[whoAmI].position;
							if (vector2.Length() < num60)
							{
								num60 = vector2.Length();
								num59 = num61;
							}
						}
					}
					if (num59 >= 0)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Game.HasTeleportedTo", Main.player[whoAmI].name, Main.player[num59].name), new Color(250, 250, 0));
					}
					break;
				}
				}
				if (Main.netMode == 2 && num57 == 0)
				{
					NetMessage.TrySendData(65, -1, whoAmI, null, num57, num55, vector.X, vector.Y, num56, flag4.ToInt(), num58);
				}
				break;
			}
			case 66:
			{
				int num33 = reader.ReadByte();
				int num34 = reader.ReadInt16();
				if (num34 > 0)
				{
					Player player2 = Main.player[num33];
					player2.statLife += num34;
					if (player2.statLife > player2.statLifeMax2)
					{
						player2.statLife = player2.statLifeMax2;
					}
					player2.HealEffect(num34, broadcast: false);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(66, -1, whoAmI, null, num33, num34);
					}
				}
				break;
			}
			case 68:
				reader.ReadString();
				break;
			case 69:
			{
				int num247 = reader.ReadInt16();
				int num248 = reader.ReadInt16();
				int num249 = reader.ReadInt16();
				if (Main.netMode == 1)
				{
					if (num247 >= 0 && num247 < 8000)
					{
						Chest chest = Main.chest[num247];
						if (chest == null)
						{
							chest = new Chest();
							chest.x = num248;
							chest.y = num249;
							Main.chest[num247] = chest;
						}
						else if (chest.x != num248 || chest.y != num249)
						{
							break;
						}
						chest.name = reader.ReadString();
					}
				}
				else
				{
					if (num247 < -1 || num247 >= 8000)
					{
						break;
					}
					if (num247 == -1)
					{
						num247 = Chest.FindChest(num248, num249);
						if (num247 == -1)
						{
							break;
						}
					}
					Chest chest2 = Main.chest[num247];
					if (chest2.x == num248 && chest2.y == num249)
					{
						NetMessage.TrySendData(69, whoAmI, -1, null, num247, num248, num249);
					}
				}
				break;
			}
			case 70:
				if (Main.netMode == 2)
				{
					int num232 = reader.ReadInt16();
					int who = reader.ReadByte();
					if (Main.netMode == 2)
					{
						who = whoAmI;
					}
					if (num232 < 200 && num232 >= 0)
					{
						NPC.CatchNPC(num232, who);
					}
				}
				break;
			case 71:
				if (Main.netMode == 2)
				{
					int x13 = reader.ReadInt32();
					int y13 = reader.ReadInt32();
					int type15 = reader.ReadInt16();
					byte style3 = reader.ReadByte();
					NPC.ReleaseNPC(x13, y13, type15, style3, whoAmI);
				}
				break;
			case 72:
				if (Main.netMode == 1)
				{
					for (int num213 = 0; num213 < 40; num213++)
					{
						Main.travelShop[num213] = reader.ReadInt16();
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
					string name = Main.player[whoAmI].name;
					if (!Main.anglerWhoFinishedToday.Contains(name))
					{
						Main.anglerWhoFinishedToday.Add(name);
					}
				}
				break;
			case 76:
			{
				int num186 = reader.ReadByte();
				if (num186 != Main.myPlayer || Main.ServerSideCharacter)
				{
					if (Main.netMode == 2)
					{
						num186 = whoAmI;
					}
					Player obj6 = Main.player[num186];
					obj6.anglerQuestsFinished = reader.ReadInt32();
					obj6.golferScoreAccumulated = reader.ReadInt32();
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(76, -1, whoAmI, null, num186);
					}
				}
				break;
			}
			case 77:
			{
				short type12 = reader.ReadInt16();
				ushort tileType = reader.ReadUInt16();
				short x11 = reader.ReadInt16();
				short y11 = reader.ReadInt16();
				Animation.NewTemporaryAnimation(type12, tileType, x11, y11);
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
				int num152 = reader.ReadByte();
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
				WorldGen.PlaceObject(x9, y9, type9, mute: false, style2, num152, random, direction);
				if (Main.netMode == 2)
				{
					NetMessage.SendObjectPlacement(whoAmI, x9, y9, type9, style2, num152, random, direction);
				}
				break;
			}
			case 80:
				if (Main.netMode == 1)
				{
					int num140 = reader.ReadByte();
					int num141 = reader.ReadInt16();
					if (num141 >= -3 && num141 < 8000)
					{
						Main.player[num140].chest = num141;
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
					int num119 = reader.ReadInt16();
					int num120 = reader.ReadInt32();
					if (num119 >= 0 && num119 < 290)
					{
						NPC.killCount[num119] = num120;
					}
				}
				break;
			case 84:
			{
				int num107 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num107 = whoAmI;
				}
				float stealth = reader.ReadSingle();
				Main.player[num107].stealth = stealth;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(84, -1, whoAmI, null, num107);
				}
				break;
			}
			case 85:
			{
				int num105 = whoAmI;
				int slot = reader.ReadInt16();
				if (Main.netMode == 2 && num105 < 255)
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
				int num87 = reader.ReadInt32();
				if (!reader.ReadBoolean())
				{
					if (TileEntity.ByID.TryGetValue(num87, out var value2))
					{
						TileEntity.ByID.Remove(num87);
						TileEntity.ByPosition.Remove(value2.Position);
					}
				}
				else
				{
					TileEntity tileEntity = TileEntity.Read(reader, networkSend: true);
					tileEntity.ID = num87;
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
					int type4 = reader.ReadByte();
					if (WorldGen.InWorld(x6, y6) && !TileEntity.ByPosition.ContainsKey(new Point16(x6, y6)))
					{
						TileEntity.PlaceEntityNet(x6, y6, type4);
					}
				}
				break;
			case 88:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int num223 = reader.ReadInt16();
				if (num223 < 0 || num223 > 400)
				{
					break;
				}
				Item item5 = Main.item[num223];
				BitsByte bitsByte27 = reader.ReadByte();
				if (bitsByte27[0])
				{
					item5.color.PackedValue = reader.ReadUInt32();
				}
				if (bitsByte27[1])
				{
					item5.damage = reader.ReadUInt16();
				}
				if (bitsByte27[2])
				{
					item5.knockBack = reader.ReadSingle();
				}
				if (bitsByte27[3])
				{
					item5.useAnimation = reader.ReadUInt16();
				}
				if (bitsByte27[4])
				{
					item5.useTime = reader.ReadUInt16();
				}
				if (bitsByte27[5])
				{
					item5.shoot = reader.ReadInt16();
				}
				if (bitsByte27[6])
				{
					item5.shootSpeed = reader.ReadSingle();
				}
				if (bitsByte27[7])
				{
					bitsByte27 = reader.ReadByte();
					if (bitsByte27[0])
					{
						item5.width = reader.ReadInt16();
					}
					if (bitsByte27[1])
					{
						item5.height = reader.ReadInt16();
					}
					if (bitsByte27[2])
					{
						item5.scale = reader.ReadSingle();
					}
					if (bitsByte27[3])
					{
						item5.ammo = reader.ReadInt16();
					}
					if (bitsByte27[4])
					{
						item5.useAmmo = reader.ReadInt16();
					}
					if (bitsByte27[5])
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
					int stack6 = reader.ReadInt16();
					TEItemFrame.TryPlacing(x12, y12, netid3, prefix3, stack6);
				}
				break;
			case 91:
			{
				if (Main.netMode != 1)
				{
					break;
				}
				int num196 = reader.ReadInt32();
				int num197 = reader.ReadByte();
				if (num197 == 255)
				{
					if (EmoteBubble.byID.ContainsKey(num196))
					{
						EmoteBubble.byID.Remove(num196);
					}
					break;
				}
				int num198 = reader.ReadUInt16();
				int num199 = reader.ReadUInt16();
				int num200 = reader.ReadByte();
				int metadata = 0;
				if (num200 < 0)
				{
					metadata = reader.ReadInt16();
				}
				WorldUIAnchor worldUIAnchor = EmoteBubble.DeserializeNetAnchor(num197, num198);
				if (num197 == 1)
				{
					Main.player[num198].emoteTime = 360;
				}
				lock (EmoteBubble.byID)
				{
					if (!EmoteBubble.byID.ContainsKey(num196))
					{
						EmoteBubble.byID[num196] = new EmoteBubble(num200, worldUIAnchor, num199);
					}
					else
					{
						EmoteBubble.byID[num196].lifeTime = num199;
						EmoteBubble.byID[num196].lifeTimeStart = num199;
						EmoteBubble.byID[num196].emote = num200;
						EmoteBubble.byID[num196].anchor = worldUIAnchor;
					}
					EmoteBubble.byID[num196].ID = num196;
					EmoteBubble.byID[num196].metadata = metadata;
					EmoteBubble.OnBubbleChange(num196);
					break;
				}
			}
			case 92:
			{
				int num187 = reader.ReadInt16();
				int num188 = reader.ReadInt32();
				float num189 = reader.ReadSingle();
				float num190 = reader.ReadSingle();
				if (num187 >= 0 && num187 <= 200)
				{
					if (Main.netMode == 1)
					{
						Main.npc[num187].moneyPing(new Vector2(num189, num190));
						Main.npc[num187].extraValue = num188;
					}
					else
					{
						Main.npc[num187].extraValue += num188;
						NetMessage.TrySendData(92, -1, -1, null, num187, Main.npc[num187].extraValue, num189, num190);
					}
				}
				break;
			}
			case 95:
			{
				ushort num183 = reader.ReadUInt16();
				int num184 = reader.ReadByte();
				if (Main.netMode != 2)
				{
					break;
				}
				for (int num185 = 0; num185 < 1000; num185++)
				{
					if (Main.projectile[num185].owner == num183 && Main.projectile[num185].active && Main.projectile[num185].type == 602 && Main.projectile[num185].ai[1] == (float)num184)
					{
						Main.projectile[num185].Kill();
						NetMessage.TrySendData(29, -1, -1, null, Main.projectile[num185].identity, (int)num183);
						break;
					}
				}
				break;
			}
			case 96:
			{
				int num176 = reader.ReadByte();
				Player obj5 = Main.player[num176];
				int num177 = reader.ReadInt16();
				Vector2 newPos2 = reader.ReadVector2();
				Vector2 velocity6 = reader.ReadVector2();
				int num178 = (obj5.lastPortalColorIndex = num177 + ((num177 % 2 == 0) ? 1 : (-1)));
				obj5.Teleport(newPos2, 4, num177);
				obj5.velocity = velocity6;
				if (Main.netMode == 2)
				{
					NetMessage.SendData(96, -1, -1, null, num176, newPos2.X, newPos2.Y, num177);
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
				int num153 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num153 = whoAmI;
				}
				Main.player[num153].MinionRestTargetPoint = reader.ReadVector2();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(99, -1, whoAmI, null, num153);
				}
				break;
			}
			case 115:
			{
				int num150 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num150 = whoAmI;
				}
				Main.player[num150].MinionAttackTargetNPC = reader.ReadInt16();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(115, -1, whoAmI, null, num150);
				}
				break;
			}
			case 100:
			{
				int num142 = reader.ReadUInt16();
				NPC obj3 = Main.npc[num142];
				int num143 = reader.ReadInt16();
				Vector2 newPos = reader.ReadVector2();
				Vector2 velocity4 = reader.ReadVector2();
				int num144 = (obj3.lastPortalColorIndex = num143 + ((num143 % 2 == 0) ? 1 : (-1)));
				obj3.Teleport(newPos, 4, num143);
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
				int num88 = reader.ReadByte();
				ushort num89 = reader.ReadUInt16();
				Vector2 other = reader.ReadVector2();
				if (Main.netMode == 2)
				{
					num88 = whoAmI;
					NetMessage.TrySendData(102, -1, -1, null, num88, (int)num89, other.X, other.Y);
					break;
				}
				Player player3 = Main.player[num88];
				for (int num90 = 0; num90 < 255; num90++)
				{
					Player player4 = Main.player[num90];
					if (!player4.active || player4.dead || (player3.team != 0 && player3.team != player4.team) || !(player4.Distance(other) < 700f))
					{
						continue;
					}
					Vector2 value3 = player3.Center - player4.Center;
					Vector2 vector3 = Vector2.Normalize(value3);
					if (!vector3.HasNaNs())
					{
						int type6 = 90;
						float num91 = 0f;
						float num92 = MathF.PI / 15f;
						Vector2 spinningpoint = new Vector2(0f, -8f);
						Vector2 vector4 = new Vector2(-3f);
						float num93 = 0f;
						float num94 = 0.005f;
						switch (num89)
						{
						case 179:
							type6 = 86;
							break;
						case 173:
							type6 = 90;
							break;
						case 176:
							type6 = 88;
							break;
						}
						for (int num95 = 0; (float)num95 < value3.Length() / 6f; num95++)
						{
							Vector2 position2 = player4.Center + 6f * (float)num95 * vector3 + spinningpoint.RotatedBy(num91) + vector4;
							num91 += num92;
							int num96 = Dust.NewDust(position2, 6, 6, type6, 0f, 0f, 100, default(Color), 1.5f);
							Main.dust[num96].noGravity = true;
							Main.dust[num96].velocity = Vector2.Zero;
							num93 = (Main.dust[num96].fadeIn = num93 + num94);
							Main.dust[num96].velocity += vector3 * 1.5f;
						}
					}
					player4.NebulaLevelup(num89);
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
					int num54 = reader.ReadByte();
					int type5 = reader.ReadInt16();
					int stack2 = reader.ReadInt16();
					int prefixWeWant = reader.ReadByte();
					int value = reader.ReadInt32();
					BitsByte bitsByte19 = reader.ReadByte();
					if (num54 < item.Length)
					{
						item[num54] = new Item();
						item[num54].netDefaults(type5);
						item[num54].stack = stack2;
						item[num54].Prefix(prefixWeWant);
						item[num54].value = value;
						item[num54].buyOnce = bitsByte19[0];
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
					int num36 = reader.ReadByte();
					if (num36 == Main.myPlayer)
					{
						WorldGen.ShootFromCannon(x4, y4, angle, ammo, damage, knockBack, num36, fromWire: true);
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
				int type3 = reader.ReadInt16();
				int num19 = reader.ReadInt16();
				int num20 = reader.ReadByte();
				if (num20 == Main.myPlayer)
				{
					Player player = Main.player[num20];
					for (int num21 = 0; num21 < num19; num21++)
					{
						player.ConsumeItem(type3);
					}
					player.wireOperationsCooldown = 0;
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
				int num6 = reader.ReadByte();
				int num7 = reader.ReadInt32();
				int num8 = reader.ReadInt32();
				int num9 = reader.ReadByte();
				int num10 = reader.ReadInt16();
				switch (num6)
				{
				case 1:
					if (Main.netMode == 1)
					{
						WorldGen.TreeGrowFX(num7, num8, num9, num10);
					}
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(b, -1, -1, null, num6, num7, num8, num9, num10);
					}
					break;
				case 2:
					NPC.FairyEffects(new Vector2(num7, num8), num10);
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
				int num234 = reader.ReadByte();
				if (Main.netMode != 2 || whoAmI == num234 || (Main.player[num234].hostile && Main.player[whoAmI].hostile))
				{
					PlayerDeathReason playerDeathReason2 = PlayerDeathReason.FromReader(reader);
					int damage3 = reader.ReadInt16();
					int num235 = reader.ReadByte() - 1;
					BitsByte bitsByte28 = reader.ReadByte();
					bool flag15 = bitsByte28[0];
					bool pvp2 = bitsByte28[1];
					int num236 = reader.ReadSByte();
					Main.player[num234].Hurt(playerDeathReason2, damage3, num235, pvp2, quiet: true, flag15, num236);
					if (Main.netMode == 2)
					{
						NetMessage.SendPlayerHurt(num234, playerDeathReason2, damage3, num235, flag15, pvp2, num236, -1, whoAmI);
					}
				}
				break;
			}
			case 118:
			{
				int num228 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num228 = whoAmI;
				}
				PlayerDeathReason playerDeathReason = PlayerDeathReason.FromReader(reader);
				int num229 = reader.ReadInt16();
				int num230 = reader.ReadByte() - 1;
				bool pvp = ((BitsByte)reader.ReadByte())[0];
				Main.player[num228].KillMe(playerDeathReason, num229, num230, pvp);
				if (Main.netMode == 2)
				{
					NetMessage.SendPlayerDeath(num228, playerDeathReason, num229, num230, pvp, -1, whoAmI);
				}
				break;
			}
			case 120:
			{
				int num218 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num218 = whoAmI;
				}
				int num219 = reader.ReadByte();
				if (num219 >= 0 && num219 < 151 && Main.netMode == 2)
				{
					EmoteBubble.NewBubble(num219, new WorldUIAnchor(Main.player[num218]), 360);
					EmoteBubble.CheckForNPCsToReactToEmoteBubble(num219, Main.player[num218]);
				}
				break;
			}
			case 121:
			{
				int num193 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num193 = whoAmI;
				}
				int num194 = reader.ReadInt32();
				int num195 = reader.ReadByte();
				bool flag12 = false;
				if (num195 >= 8)
				{
					flag12 = true;
					num195 -= 8;
				}
				if (!TileEntity.ByID.TryGetValue(num194, out var value9))
				{
					reader.ReadInt32();
					reader.ReadByte();
					break;
				}
				if (num195 >= 8)
				{
					value9 = null;
				}
				if (value9 is TEDisplayDoll tEDisplayDoll)
				{
					tEDisplayDoll.ReadItem(num195, reader, flag12);
				}
				else
				{
					reader.ReadInt32();
					reader.ReadByte();
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, num193, null, num193, num194, num195, flag12.ToInt());
				}
				break;
			}
			case 122:
			{
				int num167 = reader.ReadInt32();
				int num168 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num168 = whoAmI;
				}
				if (Main.netMode == 2)
				{
					if (num167 == -1)
					{
						Main.player[num168].tileEntityAnchor.Clear();
						NetMessage.TrySendData(b, -1, -1, null, num167, num168);
						break;
					}
					if (!TileEntity.IsOccupied(num167, out var _) && TileEntity.ByID.TryGetValue(num167, out var value6))
					{
						Main.player[num168].tileEntityAnchor.Set(num167, value6.Position.X, value6.Position.Y);
						NetMessage.TrySendData(b, -1, -1, null, num167, num168);
					}
				}
				if (Main.netMode == 1)
				{
					TileEntity value7;
					if (num167 == -1)
					{
						Main.player[num168].tileEntityAnchor.Clear();
					}
					else if (TileEntity.ByID.TryGetValue(num167, out value7))
					{
						TileEntity.SetInteractionAnchor(Main.player[num168], value7.Position.X, value7.Position.Y, num167);
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
					int stack4 = reader.ReadInt16();
					TEWeaponsRack.TryPlacing(x10, y10, netid2, prefix2, stack4);
				}
				break;
			case 124:
			{
				int num147 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num147 = whoAmI;
				}
				int num148 = reader.ReadInt32();
				int num149 = reader.ReadByte();
				bool flag8 = false;
				if (num149 >= 2)
				{
					flag8 = true;
					num149 -= 2;
				}
				if (!TileEntity.ByID.TryGetValue(num148, out var value5))
				{
					reader.ReadInt32();
					reader.ReadByte();
					break;
				}
				if (num149 >= 2)
				{
					value5 = null;
				}
				if (value5 is TEHatRack tEHatRack)
				{
					tEHatRack.ReadItem(num149, reader, flag8);
				}
				else
				{
					reader.ReadInt32();
					reader.ReadByte();
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, num147, null, num147, num148, num149, flag8.ToInt());
				}
				break;
			}
			case 125:
			{
				int num136 = reader.ReadByte();
				int num137 = reader.ReadInt16();
				int num138 = reader.ReadInt16();
				int num139 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num136 = whoAmI;
				}
				if (Main.netMode == 1)
				{
					Main.player[Main.myPlayer].GetOtherPlayersPickTile(num137, num138, num139);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(125, -1, num136, null, num136, num137, num138, num139);
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
				int num125 = reader.ReadByte();
				int num126 = reader.ReadUInt16();
				int num127 = reader.ReadUInt16();
				int num128 = reader.ReadUInt16();
				int num129 = reader.ReadUInt16();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(128, -1, num125, null, num125, num128, num129, 0f, num126, num127);
				}
				else
				{
					GolfHelper.ContactListener.PutBallInCup_TextAndEffects(new Point(num126, num127), num125, num128, num129);
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
				int num108 = reader.ReadUInt16();
				int num109 = reader.ReadUInt16();
				int num110 = reader.ReadInt16();
				if (num110 == 682)
				{
					if (NPC.unlockedSlimeRedSpawn)
					{
						break;
					}
					NPC.unlockedSlimeRedSpawn = true;
					NetMessage.TrySendData(7);
				}
				num108 *= 16;
				num109 *= 16;
				NPC nPC2 = new NPC();
				nPC2.SetDefaults(num110);
				int type7 = nPC2.type;
				int netID = nPC2.netID;
				int num111 = NPC.NewNPC(new EntitySource_FishedOut(Main.player[whoAmI]), num108, num109, num110);
				if (netID != type7)
				{
					Main.npc[num111].SetDefaults(netID);
					NetMessage.TrySendData(23, -1, -1, null, num111);
				}
				break;
			}
			case 131:
				if (Main.netMode == 1)
				{
					int num102 = reader.ReadUInt16();
					NPC nPC = null;
					nPC = ((num102 >= 200) ? new NPC() : Main.npc[num102]);
					int num103 = reader.ReadByte();
					if (num103 == 1)
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
					BitsByte bitsByte18 = reader.ReadByte();
					int num51 = -1;
					float num52 = 1f;
					float num53 = 0f;
					SoundEngine.PlaySound(Style: (!bitsByte18[0]) ? legacySoundStyle.Style : reader.ReadInt32(), volumeScale: (!bitsByte18[1]) ? legacySoundStyle.Volume : MathHelper.Clamp(reader.ReadSingle(), 0f, 1f), pitchOffset: (!bitsByte18[2]) ? legacySoundStyle.GetRandomPitch() : MathHelper.Clamp(reader.ReadSingle(), -1f, 1f), type: legacySoundStyle.SoundId, x: point.X, y: point.Y);
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
				int num50 = reader.ReadByte();
				int ladyBugLuckTimeLeft = reader.ReadInt32();
				float torchLuck = reader.ReadSingle();
				byte luckPotion = reader.ReadByte();
				bool hasGardenGnomeNearby = reader.ReadBoolean();
				float equipmentBasedLuckBonus = reader.ReadSingle();
				float coinLuck = reader.ReadSingle();
				if (Main.netMode == 2)
				{
					num50 = whoAmI;
				}
				Player obj2 = Main.player[num50];
				obj2.ladyBugLuckTimeLeft = ladyBugLuckTimeLeft;
				obj2.torchLuck = torchLuck;
				obj2.luckPotion = luckPotion;
				obj2.HasGardenGnomeNearby = hasGardenGnomeNearby;
				obj2.equipmentBasedLuckBonus = equipmentBasedLuckBonus;
				obj2.coinLuck = coinLuck;
				obj2.RecalculateLuck();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(134, -1, num50, null, num50);
				}
				break;
			}
			case 135:
			{
				int num47 = reader.ReadByte();
				if (Main.netMode == 1)
				{
					Main.player[num47].immuneAlpha = 255;
				}
				break;
			}
			case 136:
			{
				for (int num45 = 0; num45 < 2; num45++)
				{
					for (int num46 = 0; num46 < 3; num46++)
					{
						NPC.cavernMonsterType[num45, num46] = reader.ReadUInt16();
					}
				}
				break;
			}
			case 137:
				if (Main.netMode == 2)
				{
					int num35 = reader.ReadInt16();
					int buffTypeToRemove = reader.ReadUInt16();
					if (num35 >= 0 && num35 < 200)
					{
						Main.npc[num35].RequestBuffRemoval(buffTypeToRemove);
					}
				}
				break;
			case 139:
				if (Main.netMode != 2)
				{
					int num32 = reader.ReadByte();
					bool flag3 = reader.ReadBoolean();
					Main.countsAsHostForGameplay[num32] = flag3;
				}
				break;
			case 140:
			{
				int num30 = reader.ReadByte();
				int num31 = reader.ReadInt32();
				switch (num30)
				{
				case 0:
					if (Main.netMode == 1)
					{
						CreditsRollEvent.SetRemainingTimeDirect(num31);
					}
					break;
				case 1:
					if (Main.netMode == 2)
					{
						NPC.TransformCopperSlime(num31);
					}
					break;
				case 2:
					if (Main.netMode == 2)
					{
						NPC.TransformElderSlime(num31);
					}
					break;
				}
				break;
			}
			case 141:
			{
				LucyAxeMessage.MessageSource messageSource = (LucyAxeMessage.MessageSource)reader.ReadByte();
				byte b4 = reader.ReadByte();
				Vector2 velocity = reader.ReadVector2();
				int num26 = reader.ReadInt32();
				int num27 = reader.ReadInt32();
				if (Main.netMode == 2)
				{
					NetMessage.SendData(141, -1, whoAmI, null, (int)messageSource, (int)b4, velocity.X, velocity.Y, num26, num27);
				}
				else
				{
					LucyAxeMessage.CreateFromNet(messageSource, b4, new Vector2(num26, num27), velocity);
				}
				break;
			}
			case 142:
			{
				int num22 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num22 = whoAmI;
				}
				Player obj = Main.player[num22];
				obj.piggyBankProjTracker.TryReading(reader);
				obj.voidLensChest.TryReading(reader);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(142, -1, whoAmI, null, num22);
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
					int num15 = reader.ReadInt32();
					Main.npc[num15].SetNetShimmerEffect();
					break;
				}
				}
				break;
			case 147:
			{
				int num4 = reader.ReadByte();
				if (Main.netMode == 2)
				{
					num4 = whoAmI;
				}
				int num5 = reader.ReadByte();
				Main.player[num4].TrySwitchingLoadout(num5);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, num4, null, num4, num5);
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
