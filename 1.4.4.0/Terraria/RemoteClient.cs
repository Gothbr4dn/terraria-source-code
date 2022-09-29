using System;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.Net.Sockets;

namespace Terraria
{
	public class RemoteClient
	{
		public ISocket Socket;

		public int Id;

		public string Name = "Anonymous";

		public bool IsActive;

		public bool PendingTermination;

		public bool PendingTerminationApproved;

		public bool IsAnnouncementCompleted;

		public int State;

		public int TimeOutTimer;

		public string StatusText = "";

		public string StatusText2;

		public int StatusCount;

		public int StatusMax;

		public bool[,] TileSections = new bool[Main.maxTilesX / 200 + 1, Main.maxTilesY / 150 + 1];

		public byte[] ReadBuffer;

		public float SpamProjectile;

		public float SpamAddBlock;

		public float SpamDeleteBlock;

		public float SpamWater;

		public float SpamProjectileMax = 100f;

		public float SpamAddBlockMax = 100f;

		public float SpamDeleteBlockMax = 500f;

		public float SpamWaterMax = 50f;

		private volatile bool _isReading;

		public bool IsConnected()
		{
			if (Socket != null)
			{
				return Socket.IsConnected();
			}
			return false;
		}

		public void SpamUpdate()
		{
			if (!Netplay.SpamCheck)
			{
				SpamProjectile = 0f;
				SpamDeleteBlock = 0f;
				SpamAddBlock = 0f;
				SpamWater = 0f;
				return;
			}
			if (SpamProjectile > SpamProjectileMax)
			{
				NetMessage.BootPlayer(Id, NetworkText.FromKey("Net.CheatingProjectileSpam"));
			}
			if (SpamAddBlock > SpamAddBlockMax)
			{
				NetMessage.BootPlayer(Id, NetworkText.FromKey("Net.CheatingTileSpam"));
			}
			if (SpamDeleteBlock > SpamDeleteBlockMax)
			{
				NetMessage.BootPlayer(Id, NetworkText.FromKey("Net.CheatingTileRemovalSpam"));
			}
			if (SpamWater > SpamWaterMax)
			{
				NetMessage.BootPlayer(Id, NetworkText.FromKey("Net.CheatingLiquidSpam"));
			}
			SpamProjectile -= 0.4f;
			if (SpamProjectile < 0f)
			{
				SpamProjectile = 0f;
			}
			SpamAddBlock -= 0.3f;
			if (SpamAddBlock < 0f)
			{
				SpamAddBlock = 0f;
			}
			SpamDeleteBlock -= 5f;
			if (SpamDeleteBlock < 0f)
			{
				SpamDeleteBlock = 0f;
			}
			SpamWater -= 0.2f;
			if (SpamWater < 0f)
			{
				SpamWater = 0f;
			}
		}

		public void SpamClear()
		{
			SpamProjectile = 0f;
			SpamAddBlock = 0f;
			SpamDeleteBlock = 0f;
			SpamWater = 0f;
		}

		public static void CheckSection(int playerIndex, Vector2 position, int fluff = 1)
		{
			int sectionX = Netplay.GetSectionX((int)(position.X / 16f));
			int sectionY = Netplay.GetSectionY((int)(position.Y / 16f));
			int num = 0;
			for (int i = sectionX - fluff; i < sectionX + fluff + 1; i++)
			{
				for (int j = sectionY - fluff; j < sectionY + fluff + 1; j++)
				{
					if (i >= 0 && i < Main.maxSectionsX && j >= 0 && j < Main.maxSectionsY && !Netplay.Clients[playerIndex].TileSections[i, j])
					{
						num++;
					}
				}
			}
			if (num <= 0)
			{
				return;
			}
			int num2 = num;
			NetMessage.SendData(9, playerIndex, -1, Lang.inter[44].ToNetworkText(), num2);
			Netplay.Clients[playerIndex].StatusText2 = Language.GetTextValue("Net.IsReceivingTileData");
			Netplay.Clients[playerIndex].StatusMax += num2;
			for (int k = sectionX - fluff; k < sectionX + fluff + 1; k++)
			{
				for (int l = sectionY - fluff; l < sectionY + fluff + 1; l++)
				{
					NetMessage.SendSection(playerIndex, k, l);
				}
			}
		}

		public bool SectionRange(int size, int firstX, int firstY)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = firstX;
				int num2 = firstY;
				if (i == 1)
				{
					num += size;
				}
				if (i == 2)
				{
					num2 += size;
				}
				if (i == 3)
				{
					num += size;
					num2 += size;
				}
				int sectionX = Netplay.GetSectionX(num);
				int sectionY = Netplay.GetSectionY(num2);
				if (TileSections[sectionX, sectionY])
				{
					return true;
				}
			}
			return false;
		}

		public void ResetSections()
		{
			for (int i = 0; i < Main.maxSectionsX; i++)
			{
				for (int j = 0; j < Main.maxSectionsY; j++)
				{
					TileSections[i, j] = false;
				}
			}
		}

		public void Reset()
		{
			ResetSections();
			if (Id < 255)
			{
				Main.player[Id] = new Player();
			}
			TimeOutTimer = 0;
			StatusCount = 0;
			StatusMax = 0;
			StatusText2 = "";
			StatusText = "";
			State = 0;
			_isReading = false;
			PendingTermination = false;
			PendingTerminationApproved = false;
			SpamClear();
			IsActive = false;
			NetMessage.buffer[Id].Reset();
			if (Socket != null)
			{
				Socket.Close();
			}
		}

		public void ServerWriteCallBack(object state)
		{
			NetMessage.buffer[Id].spamCount--;
			if (StatusMax > 0)
			{
				StatusCount++;
			}
		}

		public void Update()
		{
			if (!IsActive)
			{
				State = 0;
				IsActive = true;
			}
			TryRead();
			UpdateStatusText();
		}

		private void TryRead()
		{
			if (_isReading)
			{
				return;
			}
			try
			{
				if (Socket.IsDataAvailable())
				{
					_isReading = true;
					Socket.AsyncReceive(ReadBuffer, 0, ReadBuffer.Length, ServerReadCallBack);
				}
			}
			catch
			{
				PendingTermination = true;
			}
		}

		private void ServerReadCallBack(object state, int length)
		{
			if (!Netplay.Disconnect)
			{
				if (length == 0)
				{
					PendingTermination = true;
				}
				else
				{
					try
					{
						NetMessage.ReceiveBytes(ReadBuffer, length, Id);
					}
					catch
					{
						if (!Main.ignoreErrors)
						{
							throw;
						}
					}
				}
			}
			_isReading = false;
		}

		private void UpdateStatusText()
		{
			if (StatusMax > 0 && StatusText2 != "")
			{
				if (StatusCount >= StatusMax)
				{
					StatusText = Language.GetTextValue("Net.ClientStatusComplete", Socket.GetRemoteAddress(), Name, StatusText2);
					StatusText2 = "";
					StatusMax = 0;
					StatusCount = 0;
					return;
				}
				StatusText = string.Concat("(", Socket.GetRemoteAddress(), ") ", Name, " ", StatusText2, ": ", (int)((float)StatusCount / (float)StatusMax * 100f), "%");
			}
			else if (State == 0)
			{
				StatusText = Language.GetTextValue("Net.ClientConnecting", $"({Socket.GetRemoteAddress()}) {Name}");
			}
			else if (State == 1)
			{
				StatusText = Language.GetTextValue("Net.ClientSendingData", Socket.GetRemoteAddress(), Name);
			}
			else if (State == 2)
			{
				StatusText = Language.GetTextValue("Net.ClientRequestedWorldInfo", Socket.GetRemoteAddress(), Name);
			}
			else if (State != 3 && State == 10)
			{
				try
				{
					StatusText = Language.GetTextValue("Net.ClientPlaying", Socket.GetRemoteAddress(), Name);
				}
				catch (Exception)
				{
					PendingTermination = true;
				}
			}
		}
	}
}
