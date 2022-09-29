using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Localization;

namespace Terraria
{
	public class PopupText
	{
		public Vector2 position;

		public Vector2 velocity;

		public float alpha;

		public int alphaDir = 1;

		public string name;

		public long stack;

		public float scale = 1f;

		public float rotation;

		public Color color;

		public bool active;

		public int lifeTime;

		public static int activeTime = 60;

		public static int numActive;

		public bool NoStack;

		public bool coinText;

		public long coinValue;

		public static int sonarText = -1;

		public bool expert;

		public bool master;

		public bool sonar;

		public PopupTextContext context;

		public int npcNetID;

		public bool freeAdvanced;

		public bool notActuallyAnItem
		{
			get
			{
				if (npcNetID == 0)
				{
					return freeAdvanced;
				}
				return true;
			}
		}

		public static float TargetScale => Main.UIScale / Main.GameViewMatrix.Zoom.X;

		public static void ClearSonarText()
		{
			if (sonarText >= 0 && Main.popupText[sonarText].sonar)
			{
				Main.popupText[sonarText].active = false;
				sonarText = -1;
			}
		}

		public static void ResetText(PopupText text)
		{
			text.NoStack = false;
			text.coinText = false;
			text.coinValue = 0L;
			text.sonar = false;
			text.npcNetID = 0;
			text.expert = false;
			text.master = false;
			text.freeAdvanced = false;
			text.scale = 0f;
			text.rotation = 0f;
			text.alpha = 1f;
			text.alphaDir = -1;
		}

		public static int NewText(AdvancedPopupRequest request, Vector2 position)
		{
			if (!Main.showItemText)
			{
				return -1;
			}
			if (Main.netMode == 2)
			{
				return -1;
			}
			int num = FindNextItemTextSlot();
			if (num >= 0)
			{
				string text = request.Text;
				Vector2 vector = FontAssets.MouseText.get_Value().MeasureString(text);
				PopupText obj = Main.popupText[num];
				ResetText(obj);
				obj.active = true;
				obj.position = position - vector / 2f;
				obj.name = text;
				obj.stack = 1L;
				obj.velocity = request.Velocity;
				obj.lifeTime = request.DurationInFrames;
				obj.context = PopupTextContext.Advanced;
				obj.freeAdvanced = true;
				obj.color = request.Color;
			}
			return num;
		}

		public static int NewText(PopupTextContext context, int npcNetID, Vector2 position, bool stay5TimesLonger)
		{
			if (!Main.showItemText)
			{
				return -1;
			}
			if (npcNetID == 0)
			{
				return -1;
			}
			if (Main.netMode == 2)
			{
				return -1;
			}
			int num = FindNextItemTextSlot();
			if (num >= 0)
			{
				NPC nPC = new NPC();
				nPC.SetDefaults(npcNetID);
				string typeName = nPC.TypeName;
				Vector2 vector = FontAssets.MouseText.get_Value().MeasureString(typeName);
				PopupText popupText = Main.popupText[num];
				ResetText(popupText);
				popupText.active = true;
				popupText.position = position - vector / 2f;
				popupText.name = typeName;
				popupText.stack = 1L;
				popupText.velocity.Y = -7f;
				popupText.lifeTime = 60;
				popupText.context = context;
				if (stay5TimesLonger)
				{
					popupText.lifeTime *= 5;
				}
				popupText.npcNetID = npcNetID;
				popupText.color = Color.White;
				if (context == PopupTextContext.SonarAlert)
				{
					popupText.color = Color.Lerp(Color.White, Color.Crimson, 0.5f);
				}
			}
			return num;
		}

		public static int NewText(PopupTextContext context, Item newItem, int stack, bool noStack = false, bool longText = false)
		{
			if (!Main.showItemText)
			{
				return -1;
			}
			if (newItem.Name == null || !newItem.active)
			{
				return -1;
			}
			if (Main.netMode == 2)
			{
				return -1;
			}
			bool flag = newItem.type >= 71 && newItem.type <= 74;
			for (int i = 0; i < 20; i++)
			{
				PopupText popupText = Main.popupText[i];
				if (!popupText.active || popupText.notActuallyAnItem || (!(popupText.name == newItem.AffixName()) && (!flag || !popupText.coinText)) || popupText.NoStack || noStack)
				{
					continue;
				}
				string text = newItem.Name + " (" + (popupText.stack + stack) + ")";
				string text2 = newItem.Name;
				if (popupText.stack > 1)
				{
					text2 = text2 + " (" + popupText.stack + ")";
				}
				Vector2 vector = FontAssets.MouseText.get_Value().MeasureString(text2);
				vector = FontAssets.MouseText.get_Value().MeasureString(text);
				if (popupText.lifeTime < 0)
				{
					popupText.scale = 1f;
				}
				if (popupText.lifeTime < 60)
				{
					popupText.lifeTime = 60;
				}
				if (flag && popupText.coinText)
				{
					long num = 0L;
					if (newItem.type == 71)
					{
						num += stack;
					}
					else if (newItem.type == 72)
					{
						num += 100 * stack;
					}
					else if (newItem.type == 73)
					{
						num += 10000 * stack;
					}
					else if (newItem.type == 74)
					{
						num += 1000000 * stack;
					}
					popupText.AddToCoinValue(num);
					text = ValueToName(popupText.coinValue);
					vector = FontAssets.MouseText.get_Value().MeasureString(text);
					popupText.name = text;
					if (popupText.coinValue >= 1000000)
					{
						if (popupText.lifeTime < 300)
						{
							popupText.lifeTime = 300;
						}
						popupText.color = new Color(220, 220, 198);
					}
					else if (popupText.coinValue >= 10000)
					{
						if (popupText.lifeTime < 240)
						{
							popupText.lifeTime = 240;
						}
						popupText.color = new Color(224, 201, 92);
					}
					else if (popupText.coinValue >= 100)
					{
						if (popupText.lifeTime < 180)
						{
							popupText.lifeTime = 180;
						}
						popupText.color = new Color(181, 192, 193);
					}
					else if (popupText.coinValue >= 1)
					{
						if (popupText.lifeTime < 120)
						{
							popupText.lifeTime = 120;
						}
						popupText.color = new Color(246, 138, 96);
					}
				}
				popupText.stack += stack;
				popupText.scale = 0f;
				popupText.rotation = 0f;
				popupText.position.X = newItem.position.X + (float)newItem.width * 0.5f - vector.X * 0.5f;
				popupText.position.Y = newItem.position.Y + (float)newItem.height * 0.25f - vector.Y * 0.5f;
				popupText.velocity.Y = -7f;
				popupText.context = context;
				popupText.npcNetID = 0;
				if (popupText.coinText)
				{
					popupText.stack = 1L;
				}
				return i;
			}
			int num2 = FindNextItemTextSlot();
			if (num2 >= 0)
			{
				string text3 = newItem.AffixName();
				if (stack > 1)
				{
					text3 = text3 + " (" + stack + ")";
				}
				Vector2 vector2 = FontAssets.MouseText.get_Value().MeasureString(text3);
				PopupText popupText2 = Main.popupText[num2];
				ResetText(popupText2);
				popupText2.active = true;
				popupText2.position.X = newItem.position.X + (float)newItem.width * 0.5f - vector2.X * 0.5f;
				popupText2.position.Y = newItem.position.Y + (float)newItem.height * 0.25f - vector2.Y * 0.5f;
				popupText2.color = Color.White;
				if (newItem.rare == 1)
				{
					popupText2.color = new Color(150, 150, 255);
				}
				else if (newItem.rare == 2)
				{
					popupText2.color = new Color(150, 255, 150);
				}
				else if (newItem.rare == 3)
				{
					popupText2.color = new Color(255, 200, 150);
				}
				else if (newItem.rare == 4)
				{
					popupText2.color = new Color(255, 150, 150);
				}
				else if (newItem.rare == 5)
				{
					popupText2.color = new Color(255, 150, 255);
				}
				else if (newItem.rare == -13)
				{
					popupText2.master = true;
				}
				else if (newItem.rare == -11)
				{
					popupText2.color = new Color(255, 175, 0);
				}
				else if (newItem.rare == -1)
				{
					popupText2.color = new Color(130, 130, 130);
				}
				else if (newItem.rare == 6)
				{
					popupText2.color = new Color(210, 160, 255);
				}
				else if (newItem.rare == 7)
				{
					popupText2.color = new Color(150, 255, 10);
				}
				else if (newItem.rare == 8)
				{
					popupText2.color = new Color(255, 255, 10);
				}
				else if (newItem.rare == 9)
				{
					popupText2.color = new Color(5, 200, 255);
				}
				else if (newItem.rare == 10)
				{
					popupText2.color = new Color(255, 40, 100);
				}
				else if (newItem.rare >= 11)
				{
					popupText2.color = new Color(180, 40, 255);
				}
				popupText2.expert = newItem.expert;
				popupText2.name = newItem.AffixName();
				popupText2.stack = stack;
				popupText2.velocity.Y = -7f;
				popupText2.lifeTime = 60;
				popupText2.context = context;
				if (longText)
				{
					popupText2.lifeTime *= 5;
				}
				popupText2.coinValue = 0L;
				popupText2.coinText = newItem.type >= 71 && newItem.type <= 74;
				if (popupText2.coinText)
				{
					long num3 = 0L;
					if (newItem.type == 71)
					{
						num3 += popupText2.stack;
					}
					else if (newItem.type == 72)
					{
						num3 += 100 * popupText2.stack;
					}
					else if (newItem.type == 73)
					{
						num3 += 10000 * popupText2.stack;
					}
					else if (newItem.type == 74)
					{
						num3 += 1000000 * popupText2.stack;
					}
					popupText2.AddToCoinValue(num3);
					popupText2.ValueToName();
					popupText2.stack = 1L;
					if (popupText2.coinValue >= 1000000)
					{
						if (popupText2.lifeTime < 300)
						{
							popupText2.lifeTime = 300;
						}
						popupText2.color = new Color(220, 220, 198);
					}
					else if (popupText2.coinValue >= 10000)
					{
						if (popupText2.lifeTime < 240)
						{
							popupText2.lifeTime = 240;
						}
						popupText2.color = new Color(224, 201, 92);
					}
					else if (popupText2.coinValue >= 100)
					{
						if (popupText2.lifeTime < 180)
						{
							popupText2.lifeTime = 180;
						}
						popupText2.color = new Color(181, 192, 193);
					}
					else if (popupText2.coinValue >= 1)
					{
						if (popupText2.lifeTime < 120)
						{
							popupText2.lifeTime = 120;
						}
						popupText2.color = new Color(246, 138, 96);
					}
				}
			}
			return num2;
		}

		private void AddToCoinValue(long addedValue)
		{
			long val = coinValue + addedValue;
			coinValue = Math.Min(999999999L, Math.Max(0L, val));
		}

		private static int FindNextItemTextSlot()
		{
			int num = -1;
			for (int i = 0; i < 20; i++)
			{
				if (!Main.popupText[i].active)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				double num2 = Main.bottomWorld;
				for (int j = 0; j < 20; j++)
				{
					if (num2 > (double)Main.popupText[j].position.Y)
					{
						num = j;
						num2 = Main.popupText[j].position.Y;
					}
				}
			}
			return num;
		}

		public static void AssignAsSonarText(int sonarTextIndex)
		{
			sonarText = sonarTextIndex;
			if (sonarText > -1)
			{
				Main.popupText[sonarText].sonar = true;
			}
		}

		public static string ValueToName(long coinValue)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			string text = "";
			long num5 = coinValue;
			while (num5 > 0)
			{
				if (num5 >= 1000000)
				{
					num5 -= 1000000;
					num++;
				}
				else if (num5 >= 10000)
				{
					num5 -= 10000;
					num2++;
				}
				else if (num5 >= 100)
				{
					num5 -= 100;
					num3++;
				}
				else if (num5 >= 1)
				{
					num5--;
					num4++;
				}
			}
			text = "";
			if (num > 0)
			{
				text = text + num + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));
			}
			if (num2 > 0)
			{
				text = text + num2 + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));
			}
			if (num3 > 0)
			{
				text = text + num3 + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));
			}
			if (num4 > 0)
			{
				text = text + num4 + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));
			}
			if (text.Length > 1)
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		private void ValueToName()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			long num5 = coinValue;
			while (num5 > 0)
			{
				if (num5 >= 1000000)
				{
					num5 -= 1000000;
					num++;
				}
				else if (num5 >= 10000)
				{
					num5 -= 10000;
					num2++;
				}
				else if (num5 >= 100)
				{
					num5 -= 100;
					num3++;
				}
				else if (num5 >= 1)
				{
					num5--;
					num4++;
				}
			}
			name = "";
			if (num > 0)
			{
				name = name + num + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));
			}
			if (num2 > 0)
			{
				name = name + num2 + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));
			}
			if (num3 > 0)
			{
				name = name + num3 + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));
			}
			if (num4 > 0)
			{
				name = name + num4 + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));
			}
			if (name.Length > 1)
			{
				name = name.Substring(0, name.Length - 1);
			}
		}

		public void Update(int whoAmI)
		{
			if (!active)
			{
				return;
			}
			float targetScale = TargetScale;
			alpha += (float)alphaDir * 0.01f;
			if ((double)alpha <= 0.7)
			{
				alpha = 0.7f;
				alphaDir = 1;
			}
			if (alpha >= 1f)
			{
				alpha = 1f;
				alphaDir = -1;
			}
			if (expert)
			{
				color = new Color((byte)Main.DiscoR, (byte)Main.DiscoG, (byte)Main.DiscoB, Main.mouseTextColor);
			}
			else if (master)
			{
				color = new Color(255, (byte)(Main.masterColor * 200f), 0, Main.mouseTextColor);
			}
			bool flag = false;
			Vector2 textHitbox = GetTextHitbox();
			Rectangle rectangle = new Rectangle((int)(position.X - textHitbox.X / 2f), (int)(position.Y - textHitbox.Y / 2f), (int)textHitbox.X, (int)textHitbox.Y);
			for (int i = 0; i < 20; i++)
			{
				PopupText popupText = Main.popupText[i];
				if (!popupText.active || i == whoAmI)
				{
					continue;
				}
				Vector2 textHitbox2 = popupText.GetTextHitbox();
				Rectangle value = new Rectangle((int)(popupText.position.X - textHitbox2.X / 2f), (int)(popupText.position.Y - textHitbox2.Y / 2f), (int)textHitbox2.X, (int)textHitbox2.Y);
				if (rectangle.Intersects(value) && (position.Y < popupText.position.Y || (position.Y == popupText.position.Y && whoAmI < i)))
				{
					flag = true;
					int num = numActive;
					if (num > 3)
					{
						num = 3;
					}
					popupText.lifeTime = activeTime + 15 * num;
					lifeTime = activeTime + 15 * num;
				}
			}
			if (!flag)
			{
				velocity.Y *= 0.86f;
				if (scale == targetScale)
				{
					velocity.Y *= 0.4f;
				}
			}
			else if (velocity.Y > -6f)
			{
				velocity.Y -= 0.2f;
			}
			else
			{
				velocity.Y *= 0.86f;
			}
			velocity.X *= 0.93f;
			position += velocity;
			lifeTime--;
			if (lifeTime <= 0)
			{
				scale -= 0.03f * targetScale;
				if ((double)scale < 0.1 * (double)targetScale)
				{
					active = false;
					if (sonarText == whoAmI)
					{
						sonarText = -1;
					}
				}
				lifeTime = 0;
			}
			else
			{
				if (scale < targetScale)
				{
					scale += 0.1f * targetScale;
				}
				if (scale > targetScale)
				{
					scale = targetScale;
				}
			}
		}

		private Vector2 GetTextHitbox()
		{
			string text = name;
			if (stack > 1)
			{
				text = text + " (" + stack + ")";
			}
			Vector2 result = FontAssets.MouseText.get_Value().MeasureString(text);
			result *= scale;
			result.Y *= 0.8f;
			return result;
		}

		public static void UpdateItemText()
		{
			int num = 0;
			for (int i = 0; i < 20; i++)
			{
				if (Main.popupText[i].active)
				{
					num++;
					Main.popupText[i].Update(i);
				}
			}
			numActive = num;
		}

		public static void ClearAll()
		{
			for (int i = 0; i < 20; i++)
			{
				Main.popupText[i] = new PopupText();
			}
			numActive = 0;
		}
	}
}
