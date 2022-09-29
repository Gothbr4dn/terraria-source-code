using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace Terraria.GameContent.UI
{
	public class WiresUI
	{
		public static class Settings
		{
			[Flags]
			public enum MultiToolMode
			{
				Red = 1,
				Green = 2,
				Blue = 4,
				Yellow = 8,
				Actuator = 0x10,
				Cutter = 0x20
			}

			public static MultiToolMode ToolMode = MultiToolMode.Red;

			private static int _lastActuatorEnabled;

			public static bool DrawWires
			{
				get
				{
					if (Main.getGoodWorld && !NPC.downedBoss3)
					{
						return false;
					}
					if (!Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].mech)
					{
						if (Main.player[Main.myPlayer].InfoAccMechShowWires)
						{
							return Main.player[Main.myPlayer].builderAccStatus[8] == 0;
						}
						return false;
					}
					return true;
				}
			}

			public static bool HideWires => Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type == 3620;

			public static bool DrawToolModeUI
			{
				get
				{
					int type = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type;
					if (type != 3611)
					{
						return type == 3625;
					}
					return true;
				}
			}

			public static bool DrawToolAllowActuators
			{
				get
				{
					int type = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type;
					if (type == 3611)
					{
						_lastActuatorEnabled = 2;
					}
					if (type == 3625)
					{
						_lastActuatorEnabled = 1;
					}
					return _lastActuatorEnabled == 2;
				}
			}
		}

		public class WiresRadial
		{
			public Vector2 position;

			public bool active;

			public bool OnWiresMenu;

			private float _lineOpacity;

			public void Update()
			{
				FlowerUpdate();
				LineUpdate();
			}

			private void LineUpdate()
			{
				bool value = true;
				float min = 0.75f;
				Player player = Main.player[Main.myPlayer];
				if (!Settings.DrawToolModeUI || Main.drawingPlayerChat)
				{
					value = false;
					min = 0f;
				}
				if (player.dead || Main.mouseItem.type > 0)
				{
					value = false;
					_lineOpacity = 0f;
					return;
				}
				if (player.cursorItemIconEnabled && player.cursorItemIconID != 0 && player.cursorItemIconID != 3625)
				{
					value = false;
					_lineOpacity = 0f;
					return;
				}
				if ((!player.cursorItemIconEnabled && ((!PlayerInput.UsingGamepad && !Settings.DrawToolAllowActuators) || player.mouseInterface || player.lastMouseInterface)) || Main.ingameOptionsWindow || Main.InGameUI.IsVisible)
				{
					value = false;
					_lineOpacity = 0f;
					return;
				}
				float num = Utils.Clamp(_lineOpacity + 0.05f * (float)value.ToDirectionInt(), min, 1f);
				_lineOpacity += 0.05f * (float)Math.Sign(num - _lineOpacity);
				if (Math.Abs(_lineOpacity - num) < 0.05f)
				{
					_lineOpacity = num;
				}
			}

			private void FlowerUpdate()
			{
				Player player = Main.player[Main.myPlayer];
				if (!Settings.DrawToolModeUI)
				{
					active = false;
					return;
				}
				if ((player.mouseInterface || player.lastMouseInterface) && !OnWiresMenu)
				{
					active = false;
					return;
				}
				if (player.dead || Main.mouseItem.type > 0)
				{
					active = false;
					OnWiresMenu = false;
					return;
				}
				OnWiresMenu = false;
				if (!Main.mouseRight || !Main.mouseRightRelease || PlayerInput.LockGamepadTileUseButton || player.noThrow != 0 || Main.HoveringOverAnNPC || player.talkNPC != -1)
				{
					return;
				}
				if (active)
				{
					active = false;
				}
				else if (!Main.SmartInteractShowingGenuine)
				{
					active = true;
					position = Main.MouseScreen;
					if (PlayerInput.UsingGamepad && Main.SmartCursorWanted)
					{
						position = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
					}
				}
			}

			public void Draw(SpriteBatch spriteBatch)
			{
				DrawFlower(spriteBatch);
				DrawCursorArea(spriteBatch);
			}

			private void DrawLine(SpriteBatch spriteBatch)
			{
				if (active || _lineOpacity == 0f)
				{
					return;
				}
				Vector2 vector = Main.MouseScreen;
				Vector2 vector2 = new Vector2(Main.screenWidth / 2, Main.screenHeight - 70);
				if (PlayerInput.UsingGamepad)
				{
					vector = Vector2.Zero;
				}
				Vector2 vector3 = vector - vector2;
				Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitX);
				Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitY);
				vector3.ToRotation();
				vector3.Length();
				bool flag = false;
				bool drawToolAllowActuators = Settings.DrawToolAllowActuators;
				for (int i = 0; i < 6; i++)
				{
					if (!drawToolAllowActuators && i == 5)
					{
						continue;
					}
					bool flag2 = Settings.ToolMode.HasFlag((Settings.MultiToolMode)(1 << i));
					if (i == 5)
					{
						flag2 = Settings.ToolMode.HasFlag(Settings.MultiToolMode.Actuator);
					}
					Vector2 vector4 = vector2 + Vector2.UnitX * (45f * ((float)i - 1.5f));
					int num = i;
					if (i == 0)
					{
						num = 3;
					}
					if (i == 3)
					{
						num = 0;
					}
					switch (num)
					{
					case 0:
					case 1:
						vector4 = vector2 + new Vector2((45f + (float)(drawToolAllowActuators ? 15 : 0)) * (float)(2 - num), 0f) * _lineOpacity;
						break;
					case 2:
					case 3:
						vector4 = vector2 + new Vector2((0f - (45f + (float)(drawToolAllowActuators ? 15 : 0))) * (float)(num - 1), 0f) * _lineOpacity;
						break;
					case 5:
						vector4 = vector2 + new Vector2(0f, 22f) * _lineOpacity;
						break;
					case 4:
						flag2 = false;
						vector4 = vector2 - new Vector2(0f, drawToolAllowActuators ? 22f : 0f) * _lineOpacity;
						break;
					}
					bool flag3 = false;
					if (!PlayerInput.UsingGamepad)
					{
						flag3 = Vector2.Distance(vector4, vector) < 19f * _lineOpacity;
					}
					if (flag)
					{
						flag3 = false;
					}
					if (flag3)
					{
						flag = true;
					}
					Texture2D value = TextureAssets.WireUi[(Settings.ToolMode.HasFlag(Settings.MultiToolMode.Cutter) ? 8 : 0) + (flag3 ? 1 : 0)].get_Value();
					Texture2D texture2D = null;
					switch (i)
					{
					case 0:
					case 1:
					case 2:
					case 3:
						texture2D = TextureAssets.WireUi[2 + i].get_Value();
						break;
					case 4:
						texture2D = TextureAssets.WireUi[Settings.ToolMode.HasFlag(Settings.MultiToolMode.Cutter) ? 7 : 6].get_Value();
						break;
					case 5:
						texture2D = TextureAssets.WireUi[10].get_Value();
						break;
					}
					Color color = Color.White;
					Color color2 = Color.White;
					if (!flag2 && i != 4)
					{
						if (flag3)
						{
							color2 = new Color(100, 100, 100);
							color2 = new Color(120, 120, 120);
							color = new Color(200, 200, 200);
						}
						else
						{
							color2 = new Color(150, 150, 150);
							color2 = new Color(80, 80, 80);
							color = new Color(100, 100, 100);
						}
					}
					Utils.CenteredRectangle(vector4, new Vector2(40f));
					if (flag3)
					{
						if (Main.mouseLeft && Main.mouseLeftRelease)
						{
							switch (i)
							{
							case 0:
								Settings.ToolMode ^= Settings.MultiToolMode.Red;
								break;
							case 1:
								Settings.ToolMode ^= Settings.MultiToolMode.Green;
								break;
							case 2:
								Settings.ToolMode ^= Settings.MultiToolMode.Blue;
								break;
							case 3:
								Settings.ToolMode ^= Settings.MultiToolMode.Yellow;
								break;
							case 4:
								Settings.ToolMode ^= Settings.MultiToolMode.Cutter;
								break;
							case 5:
								Settings.ToolMode ^= Settings.MultiToolMode.Actuator;
								break;
							}
						}
						if (!Main.mouseLeft || Main.player[Main.myPlayer].mouseInterface)
						{
							Main.player[Main.myPlayer].mouseInterface = true;
						}
						OnWiresMenu = true;
					}
					spriteBatch.Draw(value, vector4, null, color * _lineOpacity, 0f, value.Size() / 2f, _lineOpacity, SpriteEffects.None, 0f);
					spriteBatch.Draw(texture2D, vector4, null, color2 * _lineOpacity, 0f, texture2D.Size() / 2f, _lineOpacity, SpriteEffects.None, 0f);
				}
				if (Main.mouseLeft && Main.mouseLeftRelease && !flag)
				{
					active = false;
				}
			}

			private void DrawFlower(SpriteBatch spriteBatch)
			{
				if (!active)
				{
					return;
				}
				Vector2 vector = Main.MouseScreen;
				Vector2 vector2 = position;
				if (PlayerInput.UsingGamepad && Main.SmartCursorWanted)
				{
					vector = ((PlayerInput.GamepadThumbstickRight != Vector2.Zero) ? (position + PlayerInput.GamepadThumbstickRight * 40f) : ((!(PlayerInput.GamepadThumbstickLeft != Vector2.Zero)) ? position : (position + PlayerInput.GamepadThumbstickLeft * 40f)));
				}
				Vector2 vector3 = vector - vector2;
				Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitX);
				Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitY);
				float num = vector3.ToRotation();
				float num2 = vector3.Length();
				bool flag = false;
				bool drawToolAllowActuators = Settings.DrawToolAllowActuators;
				float num3 = 4 + drawToolAllowActuators.ToInt();
				float num4 = (drawToolAllowActuators ? 11f : (-0.5f));
				for (int i = 0; i < 6; i++)
				{
					if (!drawToolAllowActuators && i == 5)
					{
						continue;
					}
					bool flag2 = Settings.ToolMode.HasFlag((Settings.MultiToolMode)(1 << i));
					if (i == 5)
					{
						flag2 = Settings.ToolMode.HasFlag(Settings.MultiToolMode.Actuator);
					}
					Vector2 vector4 = vector2 + Vector2.UnitX * (45f * ((float)i - 1.5f));
					switch (i)
					{
					case 0:
					case 1:
					case 2:
					case 3:
					{
						float num5 = i;
						if (i == 0)
						{
							num5 = 3f;
						}
						if (i == 3)
						{
							num5 = 0f;
						}
						vector4 = vector2 + Vector2.UnitX.RotatedBy(num5 * (MathF.PI * 2f) / num3 - MathF.PI / num4) * 45f;
						break;
					}
					case 5:
						vector4 = vector2 + Vector2.UnitX.RotatedBy((float)(i - 1) * (MathF.PI * 2f) / num3 - MathF.PI / num4) * 45f;
						break;
					case 4:
						flag2 = false;
						vector4 = vector2;
						break;
					}
					bool flag3 = false;
					if (i == 4)
					{
						flag3 = num2 < 20f;
					}
					switch (i)
					{
					case 4:
						flag3 = num2 < 20f;
						break;
					case 0:
					case 1:
					case 2:
					case 3:
					case 5:
					{
						float value = (vector4 - vector2).ToRotation().AngleTowards(num, MathF.PI * 2f / (num3 * 2f)) - num;
						if (num2 >= 20f && Math.Abs(value) < 0.01f)
						{
							flag3 = true;
						}
						break;
					}
					}
					if (!PlayerInput.UsingGamepad)
					{
						flag3 = Vector2.Distance(vector4, vector) < 19f;
					}
					if (flag)
					{
						flag3 = false;
					}
					if (flag3)
					{
						flag = true;
					}
					Texture2D value2 = TextureAssets.WireUi[(Settings.ToolMode.HasFlag(Settings.MultiToolMode.Cutter) ? 8 : 0) + (flag3 ? 1 : 0)].get_Value();
					Texture2D texture2D = null;
					switch (i)
					{
					case 0:
					case 1:
					case 2:
					case 3:
						texture2D = TextureAssets.WireUi[2 + i].get_Value();
						break;
					case 4:
						texture2D = TextureAssets.WireUi[Settings.ToolMode.HasFlag(Settings.MultiToolMode.Cutter) ? 7 : 6].get_Value();
						break;
					case 5:
						texture2D = TextureAssets.WireUi[10].get_Value();
						break;
					}
					Color color = Color.White;
					Color color2 = Color.White;
					if (!flag2 && i != 4)
					{
						if (flag3)
						{
							color2 = new Color(100, 100, 100);
							color2 = new Color(120, 120, 120);
							color = new Color(200, 200, 200);
						}
						else
						{
							color2 = new Color(150, 150, 150);
							color2 = new Color(80, 80, 80);
							color = new Color(100, 100, 100);
						}
					}
					Utils.CenteredRectangle(vector4, new Vector2(40f));
					if (flag3)
					{
						if (Main.mouseLeft && Main.mouseLeftRelease)
						{
							switch (i)
							{
							case 0:
								Settings.ToolMode ^= Settings.MultiToolMode.Red;
								break;
							case 1:
								Settings.ToolMode ^= Settings.MultiToolMode.Green;
								break;
							case 2:
								Settings.ToolMode ^= Settings.MultiToolMode.Blue;
								break;
							case 3:
								Settings.ToolMode ^= Settings.MultiToolMode.Yellow;
								break;
							case 4:
								Settings.ToolMode ^= Settings.MultiToolMode.Cutter;
								break;
							case 5:
								Settings.ToolMode ^= Settings.MultiToolMode.Actuator;
								break;
							}
						}
						Main.player[Main.myPlayer].mouseInterface = true;
						OnWiresMenu = true;
					}
					spriteBatch.Draw(value2, vector4, null, color, 0f, value2.Size() / 2f, 1f, SpriteEffects.None, 0f);
					spriteBatch.Draw(texture2D, vector4, null, color2, 0f, texture2D.Size() / 2f, 1f, SpriteEffects.None, 0f);
				}
				if (Main.mouseLeft && Main.mouseLeftRelease && !flag)
				{
					active = false;
				}
			}

			private void DrawCursorArea(SpriteBatch spriteBatch)
			{
				if (active || _lineOpacity == 0f)
				{
					return;
				}
				Vector2 vector = Main.MouseScreen + new Vector2(10 - 9 * PlayerInput.UsingGamepad.ToInt(), 25f);
				Color color = new Color(50, 50, 50);
				bool drawToolAllowActuators = Settings.DrawToolAllowActuators;
				if (!drawToolAllowActuators)
				{
					if (!PlayerInput.UsingGamepad)
					{
						vector += new Vector2(-20f, 10f);
					}
					else
					{
						vector += new Vector2(0f, 10f);
					}
				}
				Texture2D value = TextureAssets.BuilderAcc.get_Value();
				Texture2D texture = value;
				Rectangle rectangle = new Rectangle(140, 2, 6, 6);
				Rectangle rectangle2 = new Rectangle(148, 2, 6, 6);
				Rectangle rectangle3 = new Rectangle(128, 0, 10, 10);
				float num = 1f;
				float scale = 1f;
				bool flag = false;
				if (flag && !drawToolAllowActuators)
				{
					num *= Main.cursorScale;
				}
				float num2 = _lineOpacity;
				if (PlayerInput.UsingGamepad)
				{
					num2 *= Main.GamepadCursorAlpha;
				}
				for (int i = 0; i < 5; i++)
				{
					if (!drawToolAllowActuators && i == 4)
					{
						continue;
					}
					float num3 = num2;
					Vector2 vec = vector + Vector2.UnitX * (45f * ((float)i - 1.5f));
					int num4 = i;
					if (i == 0)
					{
						num4 = 3;
					}
					if (i == 1)
					{
						num4 = 2;
					}
					if (i == 2)
					{
						num4 = 1;
					}
					if (i == 3)
					{
						num4 = 0;
					}
					if (i == 4)
					{
						num4 = 5;
					}
					int num5 = num4;
					switch (num5)
					{
					case 2:
						num5 = 1;
						break;
					case 1:
						num5 = 2;
						break;
					}
					bool flag2 = Settings.ToolMode.HasFlag((Settings.MultiToolMode)(1 << num5));
					if (num5 == 5)
					{
						flag2 = Settings.ToolMode.HasFlag(Settings.MultiToolMode.Actuator);
					}
					Color color2 = Color.HotPink;
					switch (num4)
					{
					case 0:
						color2 = new Color(253, 58, 61);
						break;
					case 1:
						color2 = new Color(83, 180, 253);
						break;
					case 2:
						color2 = new Color(83, 253, 153);
						break;
					case 3:
						color2 = new Color(253, 254, 83);
						break;
					case 5:
						color2 = Color.WhiteSmoke;
						break;
					}
					if (!flag2)
					{
						color2 = Color.Lerp(color2, Color.Black, 0.65f);
					}
					if (flag)
					{
						if (drawToolAllowActuators)
						{
							switch (num4)
							{
							case 0:
								vec = vector + new Vector2(-12f, 0f) * num;
								break;
							case 3:
								vec = vector + new Vector2(12f, 0f) * num;
								break;
							case 1:
								vec = vector + new Vector2(-6f, 12f) * num;
								break;
							case 2:
								vec = vector + new Vector2(6f, 12f) * num;
								break;
							case 5:
								vec = vector + new Vector2(0f, 0f) * num;
								break;
							}
						}
						else
						{
							vec = vector + new Vector2(12 * (num4 + 1), 12 * (3 - num4)) * num;
						}
					}
					else if (drawToolAllowActuators)
					{
						switch (num4)
						{
						case 0:
							vec = vector + new Vector2(-12f, 0f) * num;
							break;
						case 3:
							vec = vector + new Vector2(12f, 0f) * num;
							break;
						case 1:
							vec = vector + new Vector2(-6f, 12f) * num;
							break;
						case 2:
							vec = vector + new Vector2(6f, 12f) * num;
							break;
						case 5:
							vec = vector + new Vector2(0f, 0f) * num;
							break;
						}
					}
					else
					{
						float num6 = 0.7f;
						switch (num4)
						{
						case 0:
							vec = vector + new Vector2(0f, -12f) * num * num6;
							break;
						case 3:
							vec = vector + new Vector2(12f, 0f) * num * num6;
							break;
						case 1:
							vec = vector + new Vector2(-12f, 0f) * num * num6;
							break;
						case 2:
							vec = vector + new Vector2(0f, 12f) * num * num6;
							break;
						}
					}
					vec = vec.Floor();
					spriteBatch.Draw(texture, vec, rectangle3, color * num3, 0f, rectangle3.Size() / 2f, scale, SpriteEffects.None, 0f);
					spriteBatch.Draw(value, vec, rectangle, color2 * num3, 0f, rectangle.Size() / 2f, scale, SpriteEffects.None, 0f);
					if (Settings.ToolMode.HasFlag(Settings.MultiToolMode.Cutter))
					{
						spriteBatch.Draw(value, vec, rectangle2, color * num3, 0f, rectangle2.Size() / 2f, scale, SpriteEffects.None, 0f);
					}
				}
			}
		}

		private static WiresRadial radial = new WiresRadial();

		public static bool Open => radial.active;

		public static void HandleWiresUI(SpriteBatch spriteBatch)
		{
			radial.Update();
			radial.Draw(spriteBatch);
		}
	}
}
