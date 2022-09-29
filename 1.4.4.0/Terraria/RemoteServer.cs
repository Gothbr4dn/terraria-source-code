using System;
using System.IO;
using Terraria.Localization;
using Terraria.Net.Sockets;

namespace Terraria
{
	public class RemoteServer
	{
		public ISocket Socket = new TcpSocket();

		public bool IsActive;

		public int State;

		public int TimeOutTimer;

		public bool IsReading;

		public byte[] ReadBuffer;

		public string StatusText;

		public int StatusCount;

		public int StatusMax;

		public BitsByte ServerSpecialFlags;

		public bool HideStatusTextPercent => ServerSpecialFlags[0];

		public bool StatusTextHasShadows => ServerSpecialFlags[1];

		public bool ServerWantsToRunCheckBytesInClientLoopThread => ServerSpecialFlags[2];

		public void ResetSpecialFlags()
		{
			ServerSpecialFlags = (byte)0;
		}

		public void ClientWriteCallBack(object state)
		{
			NetMessage.buffer[256].spamCount--;
		}

		public void ClientReadCallBack(object state, int length)
		{
			try
			{
				if (!Netplay.Disconnect)
				{
					if (length == 0)
					{
						Netplay.Disconnect = true;
						Main.statusText = Language.GetTextValue("Net.LostConnection");
					}
					else if (Main.ignoreErrors)
					{
						try
						{
							NetMessage.ReceiveBytes(ReadBuffer, length);
						}
						catch
						{
						}
					}
					else
					{
						NetMessage.ReceiveBytes(ReadBuffer, length);
					}
				}
				IsReading = false;
			}
			catch (Exception value)
			{
				try
				{
					using StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true);
					streamWriter.WriteLine(DateTime.Now);
					streamWriter.WriteLine(value);
					streamWriter.WriteLine("");
				}
				catch
				{
				}
				Netplay.Disconnect = true;
			}
		}
	}
}
