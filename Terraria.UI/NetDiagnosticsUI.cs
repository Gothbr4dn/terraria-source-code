using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace Terraria.UI
{
	public class NetDiagnosticsUI : INetDiagnosticsUI
	{
		private struct CounterForMessage
		{
			public int timesReceived;

			public int timesSent;

			public int bytesReceived;

			public int bytesSent;

			public bool exemptFromBadScoreTest;

			public void Reset()
			{
				timesReceived = 0;
				timesSent = 0;
				bytesReceived = 0;
				bytesSent = 0;
			}

			public void CountReadMessage(int messageLength)
			{
				timesReceived++;
				bytesReceived += messageLength;
			}

			public void CountSentMessage(int messageLength)
			{
				timesSent++;
				bytesSent += messageLength;
			}
		}

		private CounterForMessage[] _counterByMessageId = new CounterForMessage[149];

		private Dictionary<int, CounterForMessage> _counterByModuleId = new Dictionary<int, CounterForMessage>();

		private int _highestFoundReadBytes = 1;

		private int _highestFoundReadCount = 1;

		public void Reset()
		{
			for (int i = 0; i < _counterByMessageId.Length; i++)
			{
				_counterByMessageId[i].Reset();
			}
			_counterByModuleId.Clear();
			_counterByMessageId[10].exemptFromBadScoreTest = true;
			_counterByMessageId[82].exemptFromBadScoreTest = true;
		}

		public void CountReadMessage(int messageId, int messageLength)
		{
			_counterByMessageId[messageId].CountReadMessage(messageLength);
		}

		public void CountSentMessage(int messageId, int messageLength)
		{
			_counterByMessageId[messageId].CountSentMessage(messageLength);
		}

		public void CountReadModuleMessage(int moduleMessageId, int messageLength)
		{
			_counterByModuleId.TryGetValue(moduleMessageId, out var value);
			value.CountReadMessage(messageLength);
			_counterByModuleId[moduleMessageId] = value;
		}

		public void CountSentModuleMessage(int moduleMessageId, int messageLength)
		{
			_counterByModuleId.TryGetValue(moduleMessageId, out var value);
			value.CountSentMessage(messageLength);
			_counterByModuleId[moduleMessageId] = value;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			int num = _counterByMessageId.Length + _counterByModuleId.Count;
			for (int i = 0; i <= num / 51; i++)
			{
				Utils.DrawInvBG(spriteBatch, 190 + 400 * i, 110, 390, 683);
			}
			Vector2 position = default(Vector2);
			for (int j = 0; j < _counterByMessageId.Length; j++)
			{
				int num2 = j / 51;
				int num3 = j - num2 * 51;
				position.X = 200 + num2 * 400;
				position.Y = 120 + num3 * 13;
				DrawCounter(spriteBatch, ref _counterByMessageId[j], j.ToString(), position);
			}
			int num4 = _counterByMessageId.Length + 1;
			foreach (KeyValuePair<int, CounterForMessage> item in _counterByModuleId)
			{
				int num5 = num4 / 51;
				int num6 = num4 - num5 * 51;
				position.X = 200 + num5 * 400;
				position.Y = 120 + num6 * 13;
				CounterForMessage counter = item.Value;
				DrawCounter(spriteBatch, ref counter, ".." + item.Key, position);
				num4++;
			}
		}

		private void DrawCounter(SpriteBatch spriteBatch, ref CounterForMessage counter, string title, Vector2 position)
		{
			if (!counter.exemptFromBadScoreTest)
			{
				if (_highestFoundReadCount < counter.timesReceived)
				{
					_highestFoundReadCount = counter.timesReceived;
				}
				if (_highestFoundReadBytes < counter.bytesReceived)
				{
					_highestFoundReadBytes = counter.bytesReceived;
				}
			}
			Vector2 pos = position;
			string text = title + ": ";
			float num = Utils.Remap(counter.bytesReceived, 0f, _highestFoundReadBytes, 0f, 1f);
			Color color = Main.hslToRgb(0.3f * (1f - num), 1f, 0.5f);
			if (counter.exemptFromBadScoreTest)
			{
				color = Color.White;
			}
			string text2 = "";
			text2 = text;
			DrawText(spriteBatch, text2, pos, color);
			pos.X += 30f;
			text2 = "rx:" + string.Format("{0,0}", counter.timesReceived);
			DrawText(spriteBatch, text2, pos, color);
			pos.X += 70f;
			text2 = string.Format("{0,0}", counter.bytesReceived);
			DrawText(spriteBatch, text2, pos, color);
			pos.X += 70f;
			text2 = text;
			DrawText(spriteBatch, text2, pos, color);
			pos.X += 30f;
			text2 = "tx:" + string.Format("{0,0}", counter.timesSent);
			DrawText(spriteBatch, text2, pos, color);
			pos.X += 70f;
			text2 = string.Format("{0,0}", counter.bytesSent);
			DrawText(spriteBatch, text2, pos, color);
		}

		private void DrawText(SpriteBatch spriteBatch, string text, Vector2 pos, Color color)
		{
			DynamicSpriteFontExtensionMethods.DrawString(spriteBatch, FontAssets.MouseText.get_Value(), text, pos, color, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);
		}
	}
}
