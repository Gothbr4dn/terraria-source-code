using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace Terraria.UI
{
	public class LegacyNetDiagnosticsUI : INetDiagnosticsUI
	{
		public static bool netDiag;

		public static int txData = 0;

		public static int rxData = 0;

		public static int txMsg = 0;

		public static int rxMsg = 0;

		private const int maxMsg = 150;

		public static int[] rxMsgType = new int[150];

		public static int[] rxDataType = new int[150];

		public static int[] txMsgType = new int[150];

		public static int[] txDataType = new int[150];

		public void Reset()
		{
			rxMsg = 0;
			rxData = 0;
			txMsg = 0;
			txData = 0;
			for (int i = 0; i < 150; i++)
			{
				rxMsgType[i] = 0;
				rxDataType[i] = 0;
				txMsgType[i] = 0;
				txDataType[i] = 0;
			}
		}

		public void CountReadMessage(int messageId, int messageLength)
		{
			rxMsg++;
			rxData += messageLength;
			rxMsgType[messageId]++;
			rxDataType[messageId] += messageLength;
		}

		public void CountSentMessage(int messageId, int messageLength)
		{
			txMsg++;
			txData += messageLength;
			txMsgType[messageId]++;
			txDataType[messageId] += messageLength;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			DrawTitles(spriteBatch);
			DrawMesageLines(spriteBatch);
		}

		private static void DrawMesageLines(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < 150; i++)
			{
				int num = 200;
				int num2 = 120;
				int num3 = i / 50;
				num += num3 * 400;
				num2 += (i - num3 * 50) * 13;
				PrintNetDiagnosticsLineForMessage(spriteBatch, i, num, num2);
			}
		}

		private static void DrawTitles(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < 4; i++)
			{
				string text = "";
				int num = 20;
				int num2 = 220;
				switch (i)
				{
				case 0:
					text = "RX Msgs: " + $"{rxMsg:0,0}";
					num2 += i * 20;
					break;
				case 1:
					text = "RX Bytes: " + $"{rxData:0,0}";
					num2 += i * 20;
					break;
				case 2:
					text = "TX Msgs: " + $"{txMsg:0,0}";
					num2 += i * 20;
					break;
				case 3:
					text = "TX Bytes: " + $"{txData:0,0}";
					num2 += i * 20;
					break;
				}
				DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), text, new Vector2(num, num2), Color.White, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			}
		}

		private static void PrintNetDiagnosticsLineForMessage(SpriteBatch spriteBatch, int msgId, int x, int y)
		{
			float num = 0.7f;
			string text = "";
			text = msgId + ": ";
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), text, new Vector2(x, y), Color.White, 0f, default(Vector2), num, SpriteEffects.None, 0f);
			x += 30;
			text = "rx:" + $"{rxMsgType[msgId]:0,0}";
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), text, new Vector2(x, y), Color.White, 0f, default(Vector2), num, SpriteEffects.None, 0f);
			x += 70;
			text = $"{rxDataType[msgId]:0,0}";
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), text, new Vector2(x, y), Color.White, 0f, default(Vector2), num, SpriteEffects.None, 0f);
			x += 70;
			text = msgId + ": ";
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), text, new Vector2(x, y), Color.White, 0f, default(Vector2), num, SpriteEffects.None, 0f);
			x += 30;
			text = "tx:" + $"{txMsgType[msgId]:0,0}";
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), text, new Vector2(x, y), Color.White, 0f, default(Vector2), num, SpriteEffects.None, 0f);
			x += 70;
			text = $"{txDataType[msgId]:0,0}";
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), text, new Vector2(x, y), Color.White, 0f, default(Vector2), num, SpriteEffects.None, 0f);
		}

		public void CountReadModuleMessage(int moduleMessageId, int messageLength)
		{
		}

		public void CountSentModuleMessage(int moduleMessageId, int messageLength)
		{
		}
	}
}
