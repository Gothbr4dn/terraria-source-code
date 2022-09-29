using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI
{
	public class GameTipsDisplay
	{
		private class GameTip
		{
			private const float APPEAR_FROM = 2.5f;

			private const float APPEAR_TO = 0.5f;

			private const float DISAPPEAR_TO = -1.5f;

			private const float APPEAR_TIME = 0.5f;

			private const float DISAPPEAR_TIME = 1f;

			private const float DURATION = 16.5f;

			private LocalizedText _textKey;

			private string _formattedText;

			public float ScreenAnchorX;

			public readonly float Duration;

			public readonly double SpawnTime;

			public string Text
			{
				get
				{
					if (_textKey == null)
					{
						return "What?!";
					}
					return _formattedText;
				}
			}

			public bool IsExpired(double currentTime)
			{
				return currentTime >= SpawnTime + (double)Duration;
			}

			public bool IsExpiring(double currentTime)
			{
				return currentTime >= SpawnTime + (double)Duration - 1.0;
			}

			public GameTip(string textKey, double spawnTime)
			{
				_textKey = Language.GetText(textKey);
				SpawnTime = spawnTime;
				ScreenAnchorX = 2.5f;
				Duration = 16.5f;
				object obj = Lang.CreateDialogSubstitutionObject();
				_formattedText = _textKey.FormatWith(obj);
			}

			public void Update(double currentTime)
			{
				double num = currentTime - SpawnTime;
				if (num < 0.5)
				{
					ScreenAnchorX = MathHelper.SmoothStep(2.5f, 0.5f, (float)Utils.GetLerpValue(0.0, 0.5, num, clamped: true));
				}
				else if (num >= (double)(Duration - 1f))
				{
					ScreenAnchorX = MathHelper.SmoothStep(0.5f, -1.5f, (float)Utils.GetLerpValue(Duration - 1f, Duration, num, clamped: true));
				}
				else
				{
					ScreenAnchorX = 0.5f;
				}
			}
		}

		private LocalizedText[] _tipsDefault;

		private LocalizedText[] _tipsGamepad;

		private LocalizedText[] _tipsKeyboard;

		private readonly List<GameTip> _currentTips = new List<GameTip>();

		private LocalizedText _lastTip;

		public GameTipsDisplay()
		{
			_tipsDefault = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_Default."));
			_tipsGamepad = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_GamePad."));
			_tipsKeyboard = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_Keyboard."));
			_lastTip = null;
		}

		public void Update()
		{
			double time = Main.gameTimeCache.TotalGameTime.TotalSeconds;
			_currentTips.RemoveAll((GameTip x) => x.IsExpired(time));
			bool flag = true;
			foreach (GameTip currentTip in _currentTips)
			{
				if (!currentTip.IsExpiring(time))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				AddNewTip(time);
			}
			foreach (GameTip currentTip2 in _currentTips)
			{
				currentTip2.Update(time);
			}
		}

		public void ClearTips()
		{
			_currentTips.Clear();
		}

		public void Draw()
		{
			SpriteBatch spriteBatch = Main.spriteBatch;
			float num = Main.screenWidth;
			float y = Main.screenHeight - 150;
			float num2 = (float)Main.screenWidth * 0.5f;
			foreach (GameTip currentTip in _currentTips)
			{
				if (currentTip.ScreenAnchorX < -0.5f || currentTip.ScreenAnchorX > 1.5f)
				{
					continue;
				}
				DynamicSpriteFont value = FontAssets.MouseText.get_Value();
				string text = value.CreateWrappedText(currentTip.Text, num2, Language.ActiveCulture.CultureInfo);
				if (text.Split(new char[1] { '\n' }).Length > 2)
				{
					text = value.CreateWrappedText(currentTip.Text, num2 * 1.5f - 50f, Language.ActiveCulture.CultureInfo);
				}
				if (WorldGen.getGoodWorldGen)
				{
					string text2 = "";
					for (int num3 = text.Length - 1; num3 >= 0; num3--)
					{
						text2 += text.Substring(num3, 1);
					}
					text = text2;
				}
				else if (WorldGen.drunkWorldGenText)
				{
					text = string.Concat(Main.rand.Next(999999999));
					for (int i = 0; i < 14; i++)
					{
						if (Main.rand.Next(2) == 0)
						{
							text += Main.rand.Next(999999999);
						}
					}
				}
				Vector2 vector = value.MeasureString(text);
				float num4 = 1.1f;
				float num5 = 110f;
				if (vector.Y > num5)
				{
					num4 = num5 / vector.Y;
				}
				Vector2 position = new Vector2(num * currentTip.ScreenAnchorX, y);
				position -= vector * num4 * 0.5f;
				if (WorldGen.tenthAnniversaryWorldGen && !WorldGen.remixWorldGen)
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, value, text, position, Color.HotPink, 0f, Vector2.Zero, new Vector2(num4, num4));
				}
				else
				{
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, value, text, position, Color.White, 0f, Vector2.Zero, new Vector2(num4, num4));
				}
			}
		}

		private void AddNewTip(double currentTime)
		{
			string textKey = "UI.Back";
			List<LocalizedText> list = new List<LocalizedText>();
			list.AddRange(_tipsDefault);
			if (PlayerInput.UsingGamepad)
			{
				list.AddRange(_tipsGamepad);
			}
			else
			{
				list.AddRange(_tipsKeyboard);
			}
			if (_lastTip != null)
			{
				list.Remove(_lastTip);
			}
			string key = (_lastTip = ((list.Count != 0) ? list[Main.rand.Next(list.Count)] : LocalizedText.Empty)).Key;
			if (Language.Exists(key))
			{
				textKey = key;
			}
			_currentTips.Add(new GameTip(textKey, currentTime));
		}
	}
}
