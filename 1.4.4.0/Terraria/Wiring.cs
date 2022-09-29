using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria
{
	public static class Wiring
	{
		public static bool blockPlayerTeleportationForOneIteration;

		public static bool running;

		private static Dictionary<Point16, bool> _wireSkip;

		private static DoubleStack<Point16> _wireList;

		private static DoubleStack<byte> _wireDirectionList;

		private static Dictionary<Point16, byte> _toProcess;

		private static Queue<Point16> _GatesCurrent;

		private static Queue<Point16> _LampsToCheck;

		private static Queue<Point16> _GatesNext;

		private static Dictionary<Point16, bool> _GatesDone;

		private static Dictionary<Point16, byte> _PixelBoxTriggers;

		private static Vector2[] _teleport;

		private const int MaxPump = 20;

		private static int[] _inPumpX;

		private static int[] _inPumpY;

		private static int _numInPump;

		private static int[] _outPumpX;

		private static int[] _outPumpY;

		private static int _numOutPump;

		private const int MaxMech = 1000;

		private static int[] _mechX;

		private static int[] _mechY;

		private static int _numMechs;

		private static int[] _mechTime;

		private static int _currentWireColor;

		private static int CurrentUser = 255;

		public static void SetCurrentUser(int plr = -1)
		{
			if (plr < 0 || plr > 255)
			{
				plr = 255;
			}
			if (Main.netMode == 0)
			{
				plr = Main.myPlayer;
			}
			CurrentUser = plr;
		}

		public static void Initialize()
		{
			_wireSkip = new Dictionary<Point16, bool>();
			_wireList = new DoubleStack<Point16>();
			_wireDirectionList = new DoubleStack<byte>();
			_toProcess = new Dictionary<Point16, byte>();
			_GatesCurrent = new Queue<Point16>();
			_GatesNext = new Queue<Point16>();
			_GatesDone = new Dictionary<Point16, bool>();
			_LampsToCheck = new Queue<Point16>();
			_PixelBoxTriggers = new Dictionary<Point16, byte>();
			_inPumpX = new int[20];
			_inPumpY = new int[20];
			_outPumpX = new int[20];
			_outPumpY = new int[20];
			_teleport = new Vector2[2]
			{
				Vector2.One * -1f,
				Vector2.One * -1f
			};
			_mechX = new int[1000];
			_mechY = new int[1000];
			_mechTime = new int[1000];
		}

		public static void SkipWire(int x, int y)
		{
			_wireSkip[new Point16(x, y)] = true;
		}

		public static void SkipWire(Point16 point)
		{
			_wireSkip[point] = true;
		}

		public static void ClearAll()
		{
			for (int i = 0; i < 20; i++)
			{
				_inPumpX[i] = 0;
				_inPumpY[i] = 0;
				_outPumpX[i] = 0;
				_outPumpY[i] = 0;
			}
			_numInPump = 0;
			_numOutPump = 0;
			for (int j = 0; j < 1000; j++)
			{
				_mechTime[j] = 0;
				_mechX[j] = 0;
				_mechY[j] = 0;
			}
			_numMechs = 0;
		}

		public static void UpdateMech()
		{
			SetCurrentUser();
			for (int num = _numMechs - 1; num >= 0; num--)
			{
				_mechTime[num]--;
				int num2 = _mechX[num];
				int num3 = _mechY[num];
				if (!WorldGen.InWorld(num2, num3, 1))
				{
					_numMechs--;
				}
				else
				{
					Tile tile = Main.tile[num2, num3];
					if (tile == null)
					{
						_numMechs--;
					}
					else
					{
						if (tile.active() && tile.type == 144)
						{
							if (tile.frameY == 0)
							{
								_mechTime[num] = 0;
							}
							else
							{
								int num4 = tile.frameX / 18;
								switch (num4)
								{
								case 0:
									num4 = 60;
									break;
								case 1:
									num4 = 180;
									break;
								case 2:
									num4 = 300;
									break;
								case 3:
									num4 = 30;
									break;
								case 4:
									num4 = 15;
									break;
								}
								if (Math.IEEERemainder(_mechTime[num], num4) == 0.0)
								{
									_mechTime[num] = 18000;
									TripWire(_mechX[num], _mechY[num], 1, 1);
								}
							}
						}
						if (_mechTime[num] <= 0)
						{
							if (tile.active() && tile.type == 144)
							{
								tile.frameY = 0;
								NetMessage.SendTileSquare(-1, _mechX[num], _mechY[num]);
							}
							if (tile.active() && tile.type == 411)
							{
								int num5 = tile.frameX % 36 / 18;
								int num6 = tile.frameY % 36 / 18;
								int num7 = _mechX[num] - num5;
								int num8 = _mechY[num] - num6;
								int num9 = 36;
								if (Main.tile[num7, num8].frameX >= 36)
								{
									num9 = -36;
								}
								for (int i = num7; i < num7 + 2; i++)
								{
									for (int j = num8; j < num8 + 2; j++)
									{
										if (WorldGen.InWorld(i, j, 1))
										{
											Tile tile2 = Main.tile[i, j];
											if (tile2 != null)
											{
												tile2.frameX = (short)(tile2.frameX + num9);
											}
										}
									}
								}
								NetMessage.SendTileSquare(-1, num7, num8, 2, 2);
							}
							for (int k = num; k < _numMechs; k++)
							{
								_mechX[k] = _mechX[k + 1];
								_mechY[k] = _mechY[k + 1];
								_mechTime[k] = _mechTime[k + 1];
							}
							_numMechs--;
						}
					}
				}
			}
		}

		public static void HitSwitch(int i, int j)
		{
			if (!WorldGen.InWorld(i, j) || Main.tile[i, j] == null)
			{
				return;
			}
			if (Main.tile[i, j].type == 135 || Main.tile[i, j].type == 314 || Main.tile[i, j].type == 423 || Main.tile[i, j].type == 428 || Main.tile[i, j].type == 442 || Main.tile[i, j].type == 476)
			{
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
				TripWire(i, j, 1, 1);
			}
			else if (Main.tile[i, j].type == 440)
			{
				SoundEngine.PlaySound(28, i * 16 + 16, j * 16 + 16, 0);
				TripWire(i, j, 3, 3);
			}
			else if (Main.tile[i, j].type == 136)
			{
				if (Main.tile[i, j].frameY == 0)
				{
					Main.tile[i, j].frameY = 18;
				}
				else
				{
					Main.tile[i, j].frameY = 0;
				}
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
				TripWire(i, j, 1, 1);
			}
			else if (Main.tile[i, j].type == 443)
			{
				GeyserTrap(i, j);
			}
			else if (Main.tile[i, j].type == 144)
			{
				if (Main.tile[i, j].frameY == 0)
				{
					Main.tile[i, j].frameY = 18;
					if (Main.netMode != 1)
					{
						CheckMech(i, j, 18000);
					}
				}
				else
				{
					Main.tile[i, j].frameY = 0;
				}
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
			}
			else if (Main.tile[i, j].type == 441 || Main.tile[i, j].type == 468)
			{
				int num = Main.tile[i, j].frameX / 18 * -1;
				int num2 = Main.tile[i, j].frameY / 18 * -1;
				num %= 4;
				if (num < -1)
				{
					num += 2;
				}
				num += i;
				num2 += j;
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
				TripWire(num, num2, 2, 2);
			}
			else if (Main.tile[i, j].type == 467)
			{
				if (Main.tile[i, j].frameX / 36 == 4)
				{
					int num3 = Main.tile[i, j].frameX / 18 * -1;
					int num4 = Main.tile[i, j].frameY / 18 * -1;
					num3 %= 4;
					if (num3 < -1)
					{
						num3 += 2;
					}
					num3 += i;
					num4 += j;
					SoundEngine.PlaySound(28, i * 16, j * 16, 0);
					TripWire(num3, num4, 2, 2);
				}
			}
			else
			{
				if (Main.tile[i, j].type != 132 && Main.tile[i, j].type != 411)
				{
					return;
				}
				short num5 = 36;
				int num6 = Main.tile[i, j].frameX / 18 * -1;
				int num7 = Main.tile[i, j].frameY / 18 * -1;
				num6 %= 4;
				if (num6 < -1)
				{
					num6 += 2;
					num5 = -36;
				}
				num6 += i;
				num7 += j;
				if (Main.netMode != 1 && Main.tile[num6, num7].type == 411)
				{
					CheckMech(num6, num7, 60);
				}
				for (int k = num6; k < num6 + 2; k++)
				{
					for (int l = num7; l < num7 + 2; l++)
					{
						if (Main.tile[k, l].type == 132 || Main.tile[k, l].type == 411)
						{
							Main.tile[k, l].frameX += num5;
						}
					}
				}
				WorldGen.TileFrame(num6, num7);
				SoundEngine.PlaySound(28, i * 16, j * 16, 0);
				TripWire(num6, num7, 2, 2);
			}
		}

		public static void PokeLogicGate(int lampX, int lampY)
		{
			if (Main.netMode != 1)
			{
				_LampsToCheck.Enqueue(new Point16(lampX, lampY));
				LogicGatePass();
			}
		}

		public static bool Actuate(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (!tile.actuator())
			{
				return false;
			}
			if (tile.inActive())
			{
				ReActive(i, j);
			}
			else
			{
				DeActive(i, j);
			}
			return true;
		}

		public static void ActuateForced(int i, int j)
		{
			if (Main.tile[i, j].inActive())
			{
				ReActive(i, j);
			}
			else
			{
				DeActive(i, j);
			}
		}

		public static void MassWireOperation(Point ps, Point pe, Player master)
		{
			int wireCount = 0;
			int actuatorCount = 0;
			for (int i = 0; i < 58; i++)
			{
				if (master.inventory[i].type == 530)
				{
					wireCount += master.inventory[i].stack;
				}
				if (master.inventory[i].type == 849)
				{
					actuatorCount += master.inventory[i].stack;
				}
			}
			int num = wireCount;
			int num2 = actuatorCount;
			MassWireOperationInner(master, ps, pe, master.Center, master.direction == 1, ref wireCount, ref actuatorCount);
			int num3 = num - wireCount;
			int num4 = num2 - actuatorCount;
			if (Main.netMode == 2)
			{
				NetMessage.SendData(110, master.whoAmI, -1, null, 530, num3, master.whoAmI);
				NetMessage.SendData(110, master.whoAmI, -1, null, 849, num4, master.whoAmI);
				return;
			}
			for (int j = 0; j < num3; j++)
			{
				master.ConsumeItem(530);
			}
			for (int k = 0; k < num4; k++)
			{
				master.ConsumeItem(849);
			}
		}

		private static bool CheckMech(int i, int j, int time)
		{
			for (int k = 0; k < _numMechs; k++)
			{
				if (_mechX[k] == i && _mechY[k] == j)
				{
					return false;
				}
			}
			if (_numMechs < 999)
			{
				_mechX[_numMechs] = i;
				_mechY[_numMechs] = j;
				_mechTime[_numMechs] = time;
				_numMechs++;
				return true;
			}
			return false;
		}

		private static void XferWater()
		{
			for (int i = 0; i < _numInPump; i++)
			{
				int num = _inPumpX[i];
				int num2 = _inPumpY[i];
				int liquid = Main.tile[num, num2].liquid;
				if (liquid <= 0)
				{
					continue;
				}
				bool flag = Main.tile[num, num2].lava();
				bool flag2 = Main.tile[num, num2].honey();
				bool flag3 = Main.tile[num, num2].shimmer();
				for (int j = 0; j < _numOutPump; j++)
				{
					int num3 = _outPumpX[j];
					int num4 = _outPumpY[j];
					int liquid2 = Main.tile[num3, num4].liquid;
					if (liquid2 >= 255)
					{
						continue;
					}
					bool flag4 = Main.tile[num3, num4].lava();
					bool flag5 = Main.tile[num3, num4].honey();
					bool flag6 = Main.tile[num3, num4].shimmer();
					if (liquid2 == 0)
					{
						flag4 = flag;
						flag5 = flag2;
						flag6 = flag3;
					}
					if (flag == flag4 && flag2 == flag5 && flag3 == flag6)
					{
						int num5 = liquid;
						if (num5 + liquid2 > 255)
						{
							num5 = 255 - liquid2;
						}
						Main.tile[num3, num4].liquid += (byte)num5;
						Main.tile[num, num2].liquid -= (byte)num5;
						liquid = Main.tile[num, num2].liquid;
						Main.tile[num3, num4].lava(flag);
						Main.tile[num3, num4].honey(flag2);
						Main.tile[num3, num4].shimmer(flag3);
						WorldGen.SquareTileFrame(num3, num4);
						if (Main.tile[num, num2].liquid == 0)
						{
							Main.tile[num, num2].liquidType(0);
							WorldGen.SquareTileFrame(num, num2);
							break;
						}
					}
				}
				WorldGen.SquareTileFrame(num, num2);
			}
		}

		private static void TripWire(int left, int top, int width, int height)
		{
			if (Main.netMode == 1)
			{
				return;
			}
			running = true;
			if (_wireList.Count != 0)
			{
				_wireList.Clear(quickClear: true);
			}
			if (_wireDirectionList.Count != 0)
			{
				_wireDirectionList.Clear(quickClear: true);
			}
			Vector2[] array = new Vector2[8];
			int num = 0;
			for (int i = left; i < left + width; i++)
			{
				for (int j = top; j < top + height; j++)
				{
					Point16 back = new Point16(i, j);
					Tile tile = Main.tile[i, j];
					if (tile != null && tile.wire())
					{
						_wireList.PushBack(back);
					}
				}
			}
			_teleport[0].X = -1f;
			_teleport[0].Y = -1f;
			_teleport[1].X = -1f;
			_teleport[1].Y = -1f;
			if (_wireList.Count > 0)
			{
				_numInPump = 0;
				_numOutPump = 0;
				HitWire(_wireList, 1);
				if (_numInPump > 0 && _numOutPump > 0)
				{
					XferWater();
				}
			}
			array[num++] = _teleport[0];
			array[num++] = _teleport[1];
			for (int k = left; k < left + width; k++)
			{
				for (int l = top; l < top + height; l++)
				{
					Point16 back = new Point16(k, l);
					Tile tile2 = Main.tile[k, l];
					if (tile2 != null && tile2.wire2())
					{
						_wireList.PushBack(back);
					}
				}
			}
			_teleport[0].X = -1f;
			_teleport[0].Y = -1f;
			_teleport[1].X = -1f;
			_teleport[1].Y = -1f;
			if (_wireList.Count > 0)
			{
				_numInPump = 0;
				_numOutPump = 0;
				HitWire(_wireList, 2);
				if (_numInPump > 0 && _numOutPump > 0)
				{
					XferWater();
				}
			}
			array[num++] = _teleport[0];
			array[num++] = _teleport[1];
			_teleport[0].X = -1f;
			_teleport[0].Y = -1f;
			_teleport[1].X = -1f;
			_teleport[1].Y = -1f;
			for (int m = left; m < left + width; m++)
			{
				for (int n = top; n < top + height; n++)
				{
					Point16 back = new Point16(m, n);
					Tile tile3 = Main.tile[m, n];
					if (tile3 != null && tile3.wire3())
					{
						_wireList.PushBack(back);
					}
				}
			}
			if (_wireList.Count > 0)
			{
				_numInPump = 0;
				_numOutPump = 0;
				HitWire(_wireList, 3);
				if (_numInPump > 0 && _numOutPump > 0)
				{
					XferWater();
				}
			}
			array[num++] = _teleport[0];
			array[num++] = _teleport[1];
			_teleport[0].X = -1f;
			_teleport[0].Y = -1f;
			_teleport[1].X = -1f;
			_teleport[1].Y = -1f;
			for (int num2 = left; num2 < left + width; num2++)
			{
				for (int num3 = top; num3 < top + height; num3++)
				{
					Point16 back = new Point16(num2, num3);
					Tile tile4 = Main.tile[num2, num3];
					if (tile4 != null && tile4.wire4())
					{
						_wireList.PushBack(back);
					}
				}
			}
			if (_wireList.Count > 0)
			{
				_numInPump = 0;
				_numOutPump = 0;
				HitWire(_wireList, 4);
				if (_numInPump > 0 && _numOutPump > 0)
				{
					XferWater();
				}
			}
			array[num++] = _teleport[0];
			array[num++] = _teleport[1];
			running = false;
			for (int num4 = 0; num4 < 8; num4 += 2)
			{
				_teleport[0] = array[num4];
				_teleport[1] = array[num4 + 1];
				if (_teleport[0].X >= 0f && _teleport[1].X >= 0f)
				{
					Teleport();
				}
			}
			PixelBoxPass();
			LogicGatePass();
		}

		private static void PixelBoxPass()
		{
			foreach (KeyValuePair<Point16, byte> pixelBoxTrigger in _PixelBoxTriggers)
			{
				if (pixelBoxTrigger.Value == 3)
				{
					Tile tile = Main.tile[pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y];
					tile.frameX = (short)((tile.frameX != 18) ? 18 : 0);
					NetMessage.SendTileSquare(-1, pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y);
				}
			}
			_PixelBoxTriggers.Clear();
		}

		private static void LogicGatePass()
		{
			if (_GatesCurrent.Count != 0)
			{
				return;
			}
			_GatesDone.Clear();
			while (_LampsToCheck.Count > 0)
			{
				while (_LampsToCheck.Count > 0)
				{
					Point16 point = _LampsToCheck.Dequeue();
					CheckLogicGate(point.X, point.Y);
				}
				while (_GatesNext.Count > 0)
				{
					Utils.Swap(ref _GatesCurrent, ref _GatesNext);
					while (_GatesCurrent.Count > 0)
					{
						Point16 key = _GatesCurrent.Peek();
						if (_GatesDone.TryGetValue(key, out var value) && value)
						{
							_GatesCurrent.Dequeue();
							continue;
						}
						_GatesDone.Add(key, value: true);
						TripWire(key.X, key.Y, 1, 1);
						_GatesCurrent.Dequeue();
					}
				}
			}
			_GatesDone.Clear();
			if (blockPlayerTeleportationForOneIteration)
			{
				blockPlayerTeleportationForOneIteration = false;
			}
		}

		private static void CheckLogicGate(int lampX, int lampY)
		{
			if (!WorldGen.InWorld(lampX, lampY, 1))
			{
				return;
			}
			for (int i = lampY; i < Main.maxTilesY; i++)
			{
				Tile tile = Main.tile[lampX, i];
				if (!tile.active())
				{
					break;
				}
				if (tile.type == 420)
				{
					_GatesDone.TryGetValue(new Point16(lampX, i), out var value);
					int num = tile.frameY / 18;
					bool flag = tile.frameX == 18;
					bool flag2 = tile.frameX == 36;
					if (num < 0)
					{
						break;
					}
					int num2 = 0;
					int num3 = 0;
					bool flag3 = false;
					for (int num4 = i - 1; num4 > 0; num4--)
					{
						Tile tile2 = Main.tile[lampX, num4];
						if (!tile2.active() || tile2.type != 419)
						{
							break;
						}
						if (tile2.frameX == 36)
						{
							flag3 = true;
							break;
						}
						num2++;
						num3 += (tile2.frameX == 18).ToInt();
					}
					bool flag4 = false;
					switch (num)
					{
					default:
						return;
					case 0:
						flag4 = num2 == num3;
						break;
					case 2:
						flag4 = num2 != num3;
						break;
					case 1:
						flag4 = num3 > 0;
						break;
					case 3:
						flag4 = num3 == 0;
						break;
					case 4:
						flag4 = num3 == 1;
						break;
					case 5:
						flag4 = num3 != 1;
						break;
					}
					bool flag5 = !flag3 && flag2;
					bool flag6 = false;
					if (flag3 && Framing.GetTileSafely(lampX, lampY).frameX == 36)
					{
						flag6 = true;
					}
					if (!(flag4 != flag || flag5 || flag6))
					{
						break;
					}
					_ = tile.frameX % 18 / 18;
					tile.frameX = (short)(18 * flag4.ToInt());
					if (flag3)
					{
						tile.frameX = 36;
					}
					SkipWire(lampX, i);
					WorldGen.SquareTileFrame(lampX, i);
					NetMessage.SendTileSquare(-1, lampX, i);
					bool flag7 = !flag3 || flag6;
					if (flag6)
					{
						if (num3 == 0 || num2 == 0)
						{
							flag7 = false;
						}
						flag7 = Main.rand.NextFloat() < (float)num3 / (float)num2;
					}
					if (flag5)
					{
						flag7 = false;
					}
					if (flag7)
					{
						if (!value)
						{
							_GatesNext.Enqueue(new Point16(lampX, i));
							break;
						}
						Vector2 position = new Vector2(lampX, i) * 16f - new Vector2(10f);
						Utils.PoofOfSmoke(position);
						NetMessage.SendData(106, -1, -1, null, (int)position.X, position.Y);
					}
					break;
				}
				if (tile.type != 419)
				{
					break;
				}
			}
		}

		private static void HitWire(DoubleStack<Point16> next, int wireType)
		{
			_wireDirectionList.Clear(quickClear: true);
			for (int i = 0; i < next.Count; i++)
			{
				Point16 point = next.PopFront();
				SkipWire(point);
				_toProcess.Add(point, 4);
				next.PushBack(point);
				_wireDirectionList.PushBack(0);
			}
			_currentWireColor = wireType;
			while (next.Count > 0)
			{
				Point16 key = next.PopFront();
				int num = _wireDirectionList.PopFront();
				int x = key.X;
				int y = key.Y;
				if (!_wireSkip.ContainsKey(key))
				{
					HitWireSingle(x, y);
				}
				for (int j = 0; j < 4; j++)
				{
					int num2;
					int num3;
					switch (j)
					{
					case 0:
						num2 = x;
						num3 = y + 1;
						break;
					case 1:
						num2 = x;
						num3 = y - 1;
						break;
					case 2:
						num2 = x + 1;
						num3 = y;
						break;
					case 3:
						num2 = x - 1;
						num3 = y;
						break;
					default:
						num2 = x;
						num3 = y + 1;
						break;
					}
					if (num2 < 2 || num2 >= Main.maxTilesX - 2 || num3 < 2 || num3 >= Main.maxTilesY - 2)
					{
						continue;
					}
					Tile tile = Main.tile[num2, num3];
					if (tile == null)
					{
						continue;
					}
					Tile tile2 = Main.tile[x, y];
					if (tile2 == null)
					{
						continue;
					}
					byte b = 3;
					if (tile.type == 424 || tile.type == 445)
					{
						b = 0;
					}
					if (tile2.type == 424)
					{
						switch (tile2.frameX / 18)
						{
						case 0:
							if (j != num)
							{
								continue;
							}
							break;
						case 1:
							if ((num != 0 || j != 3) && (num != 3 || j != 0) && (num != 1 || j != 2) && (num != 2 || j != 1))
							{
								continue;
							}
							break;
						case 2:
							if ((num != 0 || j != 2) && (num != 2 || j != 0) && (num != 1 || j != 3) && (num != 3 || j != 1))
							{
								continue;
							}
							break;
						}
					}
					if (tile2.type == 445)
					{
						if (j != num)
						{
							continue;
						}
						if (_PixelBoxTriggers.ContainsKey(key))
						{
							_PixelBoxTriggers[key] |= (byte)((!(j == 0 || j == 1)) ? 1 : 2);
						}
						else
						{
							_PixelBoxTriggers[key] = (byte)((!(j == 0 || j == 1)) ? 1u : 2u);
						}
					}
					if (wireType switch
					{
						1 => tile.wire() ? 1 : 0, 
						2 => tile.wire2() ? 1 : 0, 
						3 => tile.wire3() ? 1 : 0, 
						4 => tile.wire4() ? 1 : 0, 
						_ => 0, 
					} == 0)
					{
						continue;
					}
					Point16 point2 = new Point16(num2, num3);
					if (_toProcess.TryGetValue(point2, out var value))
					{
						value = (byte)(value - 1);
						if (value == 0)
						{
							_toProcess.Remove(point2);
						}
						else
						{
							_toProcess[point2] = value;
						}
						continue;
					}
					next.PushBack(point2);
					_wireDirectionList.PushBack((byte)j);
					if (b > 0)
					{
						_toProcess.Add(point2, b);
					}
				}
			}
			_wireSkip.Clear();
			_toProcess.Clear();
		}

		public static IEntitySource GetProjectileSource(int sourceTileX, int sourceTileY)
		{
			return new EntitySource_Wiring(sourceTileX, sourceTileY);
		}

		public static IEntitySource GetNPCSource(int sourceTileX, int sourceTileY)
		{
			return new EntitySource_Wiring(sourceTileX, sourceTileY);
		}

		public static IEntitySource GetItemSource(int sourceTileX, int sourceTileY)
		{
			return new EntitySource_Wiring(sourceTileX, sourceTileY);
		}

		private static void HitWireSingle(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			bool? forcedStateWhereTrueIsOn = null;
			bool doSkipWires = true;
			int type = tile.type;
			if (tile.actuator())
			{
				ActuateForced(i, j);
			}
			if (!tile.active())
			{
				return;
			}
			switch (type)
			{
			case 144:
				HitSwitch(i, j);
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
				break;
			case 421:
				if (!tile.actuator())
				{
					tile.type = 422;
					WorldGen.SquareTileFrame(i, j);
					NetMessage.SendTileSquare(-1, i, j);
				}
				break;
			case 422:
				if (!tile.actuator())
				{
					tile.type = 421;
					WorldGen.SquareTileFrame(i, j);
					NetMessage.SendTileSquare(-1, i, j);
				}
				break;
			}
			if (type >= 255 && type <= 268)
			{
				if (!tile.actuator())
				{
					if (type >= 262)
					{
						tile.type -= 7;
					}
					else
					{
						tile.type += 7;
					}
					WorldGen.SquareTileFrame(i, j);
					NetMessage.SendTileSquare(-1, i, j);
				}
				return;
			}
			if (type == 419)
			{
				int num = 18;
				if (tile.frameX >= num)
				{
					num = -num;
				}
				if (tile.frameX == 36)
				{
					num = 0;
				}
				SkipWire(i, j);
				tile.frameX = (short)(tile.frameX + num);
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
				_LampsToCheck.Enqueue(new Point16(i, j));
				return;
			}
			if (type == 406)
			{
				int num2 = tile.frameX % 54 / 18;
				int num3 = tile.frameY % 54 / 18;
				int num4 = i - num2;
				int num5 = j - num3;
				int num6 = 54;
				if (Main.tile[num4, num5].frameY >= 108)
				{
					num6 = -108;
				}
				for (int k = num4; k < num4 + 3; k++)
				{
					for (int l = num5; l < num5 + 3; l++)
					{
						SkipWire(k, l);
						Main.tile[k, l].frameY = (short)(Main.tile[k, l].frameY + num6);
					}
				}
				NetMessage.SendTileSquare(-1, num4 + 1, num5 + 1, 3);
				return;
			}
			if (type == 452)
			{
				int num7 = tile.frameX % 54 / 18;
				int num8 = tile.frameY % 54 / 18;
				int num9 = i - num7;
				int num10 = j - num8;
				int num11 = 54;
				if (Main.tile[num9, num10].frameX >= 54)
				{
					num11 = -54;
				}
				for (int m = num9; m < num9 + 3; m++)
				{
					for (int n = num10; n < num10 + 3; n++)
					{
						SkipWire(m, n);
						Main.tile[m, n].frameX = (short)(Main.tile[m, n].frameX + num11);
					}
				}
				NetMessage.SendTileSquare(-1, num9 + 1, num10 + 1, 3);
				return;
			}
			if (type == 411)
			{
				int num12 = tile.frameX % 36 / 18;
				int num13 = tile.frameY % 36 / 18;
				int num14 = i - num12;
				int num15 = j - num13;
				int num16 = 36;
				if (Main.tile[num14, num15].frameX >= 36)
				{
					num16 = -36;
				}
				for (int num17 = num14; num17 < num14 + 2; num17++)
				{
					for (int num18 = num15; num18 < num15 + 2; num18++)
					{
						SkipWire(num17, num18);
						Main.tile[num17, num18].frameX = (short)(Main.tile[num17, num18].frameX + num16);
					}
				}
				NetMessage.SendTileSquare(-1, num14, num15, 2, 2);
				return;
			}
			if (type == 356)
			{
				int num19 = tile.frameX % 36 / 18;
				int num20 = tile.frameY % 54 / 18;
				int num21 = i - num19;
				int num22 = j - num20;
				for (int num23 = num21; num23 < num21 + 2; num23++)
				{
					for (int num24 = num22; num24 < num22 + 3; num24++)
					{
						SkipWire(num23, num24);
					}
				}
				if (!Main.fastForwardTimeToDawn && Main.sundialCooldown == 0)
				{
					Main.Sundialing();
				}
				NetMessage.SendTileSquare(-1, num21, num22, 2, 2);
				return;
			}
			if (type == 663)
			{
				int num25 = tile.frameX % 36 / 18;
				int num26 = tile.frameY % 54 / 18;
				int num27 = i - num25;
				int num28 = j - num26;
				for (int num29 = num27; num29 < num27 + 2; num29++)
				{
					for (int num30 = num28; num30 < num28 + 3; num30++)
					{
						SkipWire(num29, num30);
					}
				}
				if (!Main.fastForwardTimeToDusk && Main.moondialCooldown == 0)
				{
					Main.Moondialing();
				}
				NetMessage.SendTileSquare(-1, num27, num28, 2, 2);
				return;
			}
			if (type == 425)
			{
				int num31 = tile.frameX % 36 / 18;
				int num32 = tile.frameY % 36 / 18;
				int num33 = i - num31;
				int num34 = j - num32;
				for (int num35 = num33; num35 < num33 + 2; num35++)
				{
					for (int num36 = num34; num36 < num34 + 2; num36++)
					{
						SkipWire(num35, num36);
					}
				}
				if (Main.AnnouncementBoxDisabled)
				{
					return;
				}
				Color pink = Color.Pink;
				int num37 = Sign.ReadSign(num33, num34, CreateIfMissing: false);
				if (num37 == -1 || Main.sign[num37] == null || string.IsNullOrWhiteSpace(Main.sign[num37].text))
				{
					return;
				}
				if (Main.AnnouncementBoxRange == -1)
				{
					if (Main.netMode == 0)
					{
						Main.NewTextMultiline(Main.sign[num37].text, force: false, pink, 460);
					}
					else if (Main.netMode == 2)
					{
						NetMessage.SendData(107, -1, -1, NetworkText.FromLiteral(Main.sign[num37].text), 255, (int)pink.R, (int)pink.G, (int)pink.B, 460);
					}
				}
				else if (Main.netMode == 0)
				{
					if (Main.player[Main.myPlayer].Distance(new Vector2(num33 * 16 + 16, num34 * 16 + 16)) <= (float)Main.AnnouncementBoxRange)
					{
						Main.NewTextMultiline(Main.sign[num37].text, force: false, pink, 460);
					}
				}
				else
				{
					if (Main.netMode != 2)
					{
						return;
					}
					for (int num38 = 0; num38 < 255; num38++)
					{
						if (Main.player[num38].active && Main.player[num38].Distance(new Vector2(num33 * 16 + 16, num34 * 16 + 16)) <= (float)Main.AnnouncementBoxRange)
						{
							NetMessage.SendData(107, num38, -1, NetworkText.FromLiteral(Main.sign[num37].text), 255, (int)pink.R, (int)pink.G, (int)pink.B, 460);
						}
					}
				}
				return;
			}
			if (type == 405)
			{
				ToggleFirePlace(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				return;
			}
			if (type == 209)
			{
				int num39 = tile.frameX % 72 / 18;
				int num40 = tile.frameY % 54 / 18;
				int num41 = i - num39;
				int num42 = j - num40;
				int num43 = tile.frameY / 54;
				int num44 = tile.frameX / 72;
				int num45 = -1;
				if (num39 == 1 || num39 == 2)
				{
					num45 = num40;
				}
				int num46 = 0;
				if (num39 == 3)
				{
					num46 = -54;
				}
				if (num39 == 0)
				{
					num46 = 54;
				}
				if (num43 >= 8 && num46 > 0)
				{
					num46 = 0;
				}
				if (num43 == 0 && num46 < 0)
				{
					num46 = 0;
				}
				bool flag = false;
				if (num46 != 0)
				{
					for (int num47 = num41; num47 < num41 + 4; num47++)
					{
						for (int num48 = num42; num48 < num42 + 3; num48++)
						{
							SkipWire(num47, num48);
							Main.tile[num47, num48].frameY = (short)(Main.tile[num47, num48].frameY + num46);
						}
					}
					flag = true;
				}
				if ((num44 == 3 || num44 == 4) && (num45 == 0 || num45 == 1))
				{
					num46 = ((num44 == 3) ? 72 : (-72));
					for (int num49 = num41; num49 < num41 + 4; num49++)
					{
						for (int num50 = num42; num50 < num42 + 3; num50++)
						{
							SkipWire(num49, num50);
							Main.tile[num49, num50].frameX = (short)(Main.tile[num49, num50].frameX + num46);
						}
					}
					flag = true;
				}
				if (flag)
				{
					NetMessage.SendTileSquare(-1, num41, num42, 4, 3);
				}
				if (num45 != -1)
				{
					bool flag2 = true;
					if ((num44 == 3 || num44 == 4) && num45 < 2)
					{
						flag2 = false;
					}
					if (CheckMech(num41, num42, 30) && flag2)
					{
						WorldGen.ShootFromCannon(num41, num42, num43, num44 + 1, 0, 0f, CurrentUser, fromWire: true);
					}
				}
				return;
			}
			if (type == 212)
			{
				int num51 = tile.frameX % 54 / 18;
				int num52 = tile.frameY % 54 / 18;
				int num53 = i - num51;
				int num54 = j - num52;
				int num55 = tile.frameX / 54;
				int num56 = -1;
				if (num51 == 1)
				{
					num56 = num52;
				}
				int num57 = 0;
				if (num51 == 0)
				{
					num57 = -54;
				}
				if (num51 == 2)
				{
					num57 = 54;
				}
				if (num55 >= 1 && num57 > 0)
				{
					num57 = 0;
				}
				if (num55 == 0 && num57 < 0)
				{
					num57 = 0;
				}
				bool flag3 = false;
				if (num57 != 0)
				{
					for (int num58 = num53; num58 < num53 + 3; num58++)
					{
						for (int num59 = num54; num59 < num54 + 3; num59++)
						{
							SkipWire(num58, num59);
							Main.tile[num58, num59].frameX = (short)(Main.tile[num58, num59].frameX + num57);
						}
					}
					flag3 = true;
				}
				if (flag3)
				{
					NetMessage.SendTileSquare(-1, num53, num54, 3, 3);
				}
				if (num56 != -1 && CheckMech(num53, num54, 10))
				{
					float num60 = 12f + (float)Main.rand.Next(450) * 0.01f;
					float num61 = Main.rand.Next(85, 105);
					float num62 = Main.rand.Next(-35, 11);
					int type2 = 166;
					int damage = 0;
					float knockBack = 0f;
					Vector2 vector = new Vector2((num53 + 2) * 16 - 8, (num54 + 2) * 16 - 8);
					if (tile.frameX / 54 == 0)
					{
						num61 *= -1f;
						vector.X -= 12f;
					}
					else
					{
						vector.X += 12f;
					}
					float num63 = num61;
					float num64 = num62;
					float num65 = (float)Math.Sqrt(num63 * num63 + num64 * num64);
					num65 = num60 / num65;
					num63 *= num65;
					num64 *= num65;
					Projectile.NewProjectile(GetProjectileSource(num53, num54), vector.X, vector.Y, num63, num64, type2, damage, knockBack, CurrentUser);
				}
				return;
			}
			if (type == 215)
			{
				ToggleCampFire(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				return;
			}
			if (type == 130)
			{
				if (Main.tile[i, j - 1] == null || !Main.tile[i, j - 1].active() || (!TileID.Sets.BasicChest[Main.tile[i, j - 1].type] && !TileID.Sets.BasicChestFake[Main.tile[i, j - 1].type] && Main.tile[i, j - 1].type != 88))
				{
					tile.type = 131;
					WorldGen.SquareTileFrame(i, j);
					NetMessage.SendTileSquare(-1, i, j);
				}
				return;
			}
			if (type == 131)
			{
				tile.type = 130;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
				return;
			}
			if (type == 387 || type == 386)
			{
				bool value = type == 387;
				int num66 = WorldGen.ShiftTrapdoor(i, j, playerAbove: true).ToInt();
				if (num66 == 0)
				{
					num66 = -WorldGen.ShiftTrapdoor(i, j, playerAbove: false).ToInt();
				}
				if (num66 != 0)
				{
					NetMessage.SendData(19, -1, -1, null, 3 - value.ToInt(), i, j, num66);
				}
				return;
			}
			if (type == 389 || type == 388)
			{
				bool flag4 = type == 389;
				WorldGen.ShiftTallGate(i, j, flag4);
				NetMessage.SendData(19, -1, -1, null, 4 + flag4.ToInt(), i, j);
				return;
			}
			if (type == 11)
			{
				if (WorldGen.CloseDoor(i, j, forced: true))
				{
					NetMessage.SendData(19, -1, -1, null, 1, i, j);
				}
				return;
			}
			if (type == 10)
			{
				int num67 = 1;
				if (Main.rand.Next(2) == 0)
				{
					num67 = -1;
				}
				if (!WorldGen.OpenDoor(i, j, num67))
				{
					if (WorldGen.OpenDoor(i, j, -num67))
					{
						NetMessage.SendData(19, -1, -1, null, 0, i, j, -num67);
					}
				}
				else
				{
					NetMessage.SendData(19, -1, -1, null, 0, i, j, num67);
				}
				return;
			}
			if (type == 216)
			{
				WorldGen.LaunchRocket(i, j, fromWiring: true);
				SkipWire(i, j);
				return;
			}
			if (type == 497 || (type == 15 && tile.frameY / 40 == 1) || (type == 15 && tile.frameY / 40 == 20))
			{
				int num68 = j - tile.frameY % 40 / 18;
				SkipWire(i, num68);
				SkipWire(i, num68 + 1);
				if (CheckMech(i, num68, 60))
				{
					Projectile.NewProjectile(GetProjectileSource(i, num68), i * 16 + 8, num68 * 16 + 12, 0f, 0f, 733, 0, 0f, Main.myPlayer);
				}
				return;
			}
			switch (type)
			{
			case 335:
			{
				int num156 = j - tile.frameY / 18;
				int num157 = i - tile.frameX / 18;
				SkipWire(num157, num156);
				SkipWire(num157, num156 + 1);
				SkipWire(num157 + 1, num156);
				SkipWire(num157 + 1, num156 + 1);
				if (CheckMech(num157, num156, 30))
				{
					WorldGen.LaunchRocketSmall(num157, num156, fromWiring: true);
				}
				break;
			}
			case 338:
			{
				int num75 = j - tile.frameY / 18;
				int num76 = i - tile.frameX / 18;
				SkipWire(num76, num75);
				SkipWire(num76, num75 + 1);
				if (!CheckMech(num76, num75, 30))
				{
					break;
				}
				bool flag5 = false;
				for (int num77 = 0; num77 < 1000; num77++)
				{
					if (Main.projectile[num77].active && Main.projectile[num77].aiStyle == 73 && Main.projectile[num77].ai[0] == (float)num76 && Main.projectile[num77].ai[1] == (float)num75)
					{
						flag5 = true;
						break;
					}
				}
				if (!flag5)
				{
					int type3 = 419 + Main.rand.Next(4);
					Projectile.NewProjectile(GetProjectileSource(num76, num75), num76 * 16 + 8, num75 * 16 + 2, 0f, 0f, type3, 0, 0f, Main.myPlayer, num76, num75);
				}
				break;
			}
			case 235:
			{
				int num107 = i - tile.frameX / 18;
				if (tile.wall == 87 && (double)j > Main.worldSurface && !NPC.downedPlantBoss)
				{
					break;
				}
				if (_teleport[0].X == -1f)
				{
					_teleport[0].X = num107;
					_teleport[0].Y = j;
					if (tile.halfBrick())
					{
						_teleport[0].Y += 0.5f;
					}
				}
				else if (_teleport[0].X != (float)num107 || _teleport[0].Y != (float)j)
				{
					_teleport[1].X = num107;
					_teleport[1].Y = j;
					if (tile.halfBrick())
					{
						_teleport[1].Y += 0.5f;
					}
				}
				break;
			}
			case 4:
				ToggleTorch(i, j, tile, forcedStateWhereTrueIsOn);
				break;
			case 429:
			{
				int num78 = Main.tile[i, j].frameX / 18;
				bool flag6 = num78 % 2 >= 1;
				bool flag7 = num78 % 4 >= 2;
				bool flag8 = num78 % 8 >= 4;
				bool flag9 = num78 % 16 >= 8;
				bool flag10 = false;
				short num79 = 0;
				switch (_currentWireColor)
				{
				case 1:
					num79 = 18;
					flag10 = !flag6;
					break;
				case 2:
					num79 = 72;
					flag10 = !flag8;
					break;
				case 3:
					num79 = 36;
					flag10 = !flag7;
					break;
				case 4:
					num79 = 144;
					flag10 = !flag9;
					break;
				}
				if (flag10)
				{
					tile.frameX += num79;
				}
				else
				{
					tile.frameX -= num79;
				}
				NetMessage.SendTileSquare(-1, i, j);
				break;
			}
			case 149:
				ToggleHolidayLight(i, j, tile, forcedStateWhereTrueIsOn);
				break;
			case 244:
			{
				int num131;
				for (num131 = tile.frameX / 18; num131 >= 3; num131 -= 3)
				{
				}
				int num132;
				for (num132 = tile.frameY / 18; num132 >= 3; num132 -= 3)
				{
				}
				int num133 = i - num131;
				int num134 = j - num132;
				int num135 = 54;
				if (Main.tile[num133, num134].frameX >= 54)
				{
					num135 = -54;
				}
				for (int num136 = num133; num136 < num133 + 3; num136++)
				{
					for (int num137 = num134; num137 < num134 + 2; num137++)
					{
						SkipWire(num136, num137);
						Main.tile[num136, num137].frameX = (short)(Main.tile[num136, num137].frameX + num135);
					}
				}
				NetMessage.SendTileSquare(-1, num133, num134, 3, 2);
				break;
			}
			case 565:
			{
				int num98;
				for (num98 = tile.frameX / 18; num98 >= 2; num98 -= 2)
				{
				}
				int num99;
				for (num99 = tile.frameY / 18; num99 >= 2; num99 -= 2)
				{
				}
				int num100 = i - num98;
				int num101 = j - num99;
				int num102 = 36;
				if (Main.tile[num100, num101].frameX >= 36)
				{
					num102 = -36;
				}
				for (int num103 = num100; num103 < num100 + 2; num103++)
				{
					for (int num104 = num101; num104 < num101 + 2; num104++)
					{
						SkipWire(num103, num104);
						Main.tile[num103, num104].frameX = (short)(Main.tile[num103, num104].frameX + num102);
					}
				}
				NetMessage.SendTileSquare(-1, num100, num101, 2, 2);
				break;
			}
			case 42:
				ToggleHangingLantern(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				break;
			case 93:
				ToggleLamp(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				break;
			case 95:
			case 100:
			case 126:
			case 173:
			case 564:
				Toggle2x2Light(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				break;
			case 593:
			{
				SkipWire(i, j);
				short num105 = (short)((Main.tile[i, j].frameX != 0) ? (-18) : 18);
				Main.tile[i, j].frameX += num105;
				if (Main.netMode == 2)
				{
					NetMessage.SendTileSquare(-1, i, j, 1, 1);
				}
				int num106 = ((num105 > 0) ? 4 : 3);
				Animation.NewTemporaryAnimation(num106, 593, i, j);
				NetMessage.SendTemporaryAnimation(-1, num106, 593, i, j);
				break;
			}
			case 594:
			{
				int num80;
				for (num80 = tile.frameY / 18; num80 >= 2; num80 -= 2)
				{
				}
				num80 = j - num80;
				int num81 = tile.frameX / 18;
				if (num81 > 1)
				{
					num81 -= 2;
				}
				num81 = i - num81;
				SkipWire(num81, num80);
				SkipWire(num81, num80 + 1);
				SkipWire(num81 + 1, num80);
				SkipWire(num81 + 1, num80 + 1);
				short num82 = (short)((Main.tile[num81, num80].frameX != 0) ? (-36) : 36);
				for (int num83 = 0; num83 < 2; num83++)
				{
					for (int num84 = 0; num84 < 2; num84++)
					{
						Main.tile[num81 + num83, num80 + num84].frameX += num82;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendTileSquare(-1, num81, num80, 2, 2);
				}
				int num85 = ((num82 > 0) ? 4 : 3);
				Animation.NewTemporaryAnimation(num85, 594, num81, num80);
				NetMessage.SendTemporaryAnimation(-1, num85, 594, num81, num80);
				break;
			}
			case 34:
				ToggleChandelier(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				break;
			case 314:
				if (CheckMech(i, j, 5))
				{
					Minecart.FlipSwitchTrack(i, j);
				}
				break;
			case 33:
			case 49:
			case 174:
			case 372:
			case 646:
				ToggleCandle(i, j, tile, forcedStateWhereTrueIsOn);
				break;
			case 92:
				ToggleLampPost(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
				break;
			case 137:
			{
				int num138 = tile.frameY / 18;
				Vector2 vector3 = Vector2.Zero;
				float speedX = 0f;
				float speedY = 0f;
				int num139 = 0;
				int damage3 = 0;
				switch (num138)
				{
				case 0:
				case 1:
				case 2:
				case 5:
					if (CheckMech(i, j, 200))
					{
						int num147 = ((tile.frameX == 0) ? (-1) : ((tile.frameX == 18) ? 1 : 0));
						int num148 = ((tile.frameX >= 36) ? ((tile.frameX >= 72) ? 1 : (-1)) : 0);
						vector3 = new Vector2(i * 16 + 8 + 10 * num147, j * 16 + 8 + 10 * num148);
						float num149 = 3f;
						if (num138 == 0)
						{
							num139 = 98;
							damage3 = 20;
							num149 = 12f;
						}
						if (num138 == 1)
						{
							num139 = 184;
							damage3 = 40;
							num149 = 12f;
						}
						if (num138 == 2)
						{
							num139 = 187;
							damage3 = 40;
							num149 = 5f;
						}
						if (num138 == 5)
						{
							num139 = 980;
							damage3 = 30;
							num149 = 12f;
						}
						speedX = (float)num147 * num149;
						speedY = (float)num148 * num149;
					}
					break;
				case 3:
				{
					if (!CheckMech(i, j, 300))
					{
						break;
					}
					int num142 = 200;
					for (int num143 = 0; num143 < 1000; num143++)
					{
						if (Main.projectile[num143].active && Main.projectile[num143].type == num139)
						{
							float num144 = (new Vector2(i * 16 + 8, j * 18 + 8) - Main.projectile[num143].Center).Length();
							num142 = ((!(num144 < 50f)) ? ((!(num144 < 100f)) ? ((!(num144 < 200f)) ? ((!(num144 < 300f)) ? ((!(num144 < 400f)) ? ((!(num144 < 500f)) ? ((!(num144 < 700f)) ? ((!(num144 < 900f)) ? ((!(num144 < 1200f)) ? (num142 - 1) : (num142 - 2)) : (num142 - 3)) : (num142 - 4)) : (num142 - 5)) : (num142 - 6)) : (num142 - 8)) : (num142 - 10)) : (num142 - 15)) : (num142 - 50));
						}
					}
					if (num142 > 0)
					{
						num139 = 185;
						damage3 = 40;
						int num145 = 0;
						int num146 = 0;
						switch (tile.frameX / 18)
						{
						case 0:
						case 1:
							num145 = 0;
							num146 = 1;
							break;
						case 2:
							num145 = 0;
							num146 = -1;
							break;
						case 3:
							num145 = -1;
							num146 = 0;
							break;
						case 4:
							num145 = 1;
							num146 = 0;
							break;
						}
						speedX = (float)(4 * num145) + (float)Main.rand.Next(-20 + ((num145 == 1) ? 20 : 0), 21 - ((num145 == -1) ? 20 : 0)) * 0.05f;
						speedY = (float)(4 * num146) + (float)Main.rand.Next(-20 + ((num146 == 1) ? 20 : 0), 21 - ((num146 == -1) ? 20 : 0)) * 0.05f;
						vector3 = new Vector2(i * 16 + 8 + 14 * num145, j * 16 + 8 + 14 * num146);
					}
					break;
				}
				case 4:
					if (CheckMech(i, j, 90))
					{
						int num140 = 0;
						int num141 = 0;
						switch (tile.frameX / 18)
						{
						case 0:
						case 1:
							num140 = 0;
							num141 = 1;
							break;
						case 2:
							num140 = 0;
							num141 = -1;
							break;
						case 3:
							num140 = -1;
							num141 = 0;
							break;
						case 4:
							num140 = 1;
							num141 = 0;
							break;
						}
						speedX = 8 * num140;
						speedY = 8 * num141;
						damage3 = 60;
						num139 = 186;
						vector3 = new Vector2(i * 16 + 8 + 18 * num140, j * 16 + 8 + 18 * num141);
					}
					break;
				}
				switch (num138)
				{
				case -10:
					if (CheckMech(i, j, 200))
					{
						int num154 = -1;
						if (tile.frameX != 0)
						{
							num154 = 1;
						}
						speedX = 12 * num154;
						damage3 = 20;
						num139 = 98;
						vector3 = new Vector2(i * 16 + 8, j * 16 + 7);
						vector3.X += 10 * num154;
						vector3.Y += 2f;
					}
					break;
				case -9:
					if (CheckMech(i, j, 200))
					{
						int num150 = -1;
						if (tile.frameX != 0)
						{
							num150 = 1;
						}
						speedX = 12 * num150;
						damage3 = 40;
						num139 = 184;
						vector3 = new Vector2(i * 16 + 8, j * 16 + 7);
						vector3.X += 10 * num150;
						vector3.Y += 2f;
					}
					break;
				case -8:
					if (CheckMech(i, j, 200))
					{
						int num155 = -1;
						if (tile.frameX != 0)
						{
							num155 = 1;
						}
						speedX = 5 * num155;
						damage3 = 40;
						num139 = 187;
						vector3 = new Vector2(i * 16 + 8, j * 16 + 7);
						vector3.X += 10 * num155;
						vector3.Y += 2f;
					}
					break;
				case -7:
				{
					if (!CheckMech(i, j, 300))
					{
						break;
					}
					num139 = 185;
					int num151 = 200;
					for (int num152 = 0; num152 < 1000; num152++)
					{
						if (Main.projectile[num152].active && Main.projectile[num152].type == num139)
						{
							float num153 = (new Vector2(i * 16 + 8, j * 18 + 8) - Main.projectile[num152].Center).Length();
							num151 = ((!(num153 < 50f)) ? ((!(num153 < 100f)) ? ((!(num153 < 200f)) ? ((!(num153 < 300f)) ? ((!(num153 < 400f)) ? ((!(num153 < 500f)) ? ((!(num153 < 700f)) ? ((!(num153 < 900f)) ? ((!(num153 < 1200f)) ? (num151 - 1) : (num151 - 2)) : (num151 - 3)) : (num151 - 4)) : (num151 - 5)) : (num151 - 6)) : (num151 - 8)) : (num151 - 10)) : (num151 - 15)) : (num151 - 50));
						}
					}
					if (num151 > 0)
					{
						speedX = (float)Main.rand.Next(-20, 21) * 0.05f;
						speedY = 4f + (float)Main.rand.Next(0, 21) * 0.05f;
						damage3 = 40;
						vector3 = new Vector2(i * 16 + 8, j * 16 + 16);
						vector3.Y += 6f;
						Projectile.NewProjectile(GetProjectileSource(i, j), (int)vector3.X, (int)vector3.Y, speedX, speedY, num139, damage3, 2f, Main.myPlayer);
					}
					break;
				}
				case -6:
					if (CheckMech(i, j, 90))
					{
						speedX = 0f;
						speedY = 8f;
						damage3 = 60;
						num139 = 186;
						vector3 = new Vector2(i * 16 + 8, j * 16 + 16);
						vector3.Y += 10f;
					}
					break;
				}
				if (num139 != 0)
				{
					Projectile.NewProjectile(GetProjectileSource(i, j), (int)vector3.X, (int)vector3.Y, speedX, speedY, num139, damage3, 2f, Main.myPlayer);
				}
				break;
			}
			case 443:
				GeyserTrap(i, j);
				break;
			case 531:
			{
				int num126 = tile.frameX / 36;
				int num127 = tile.frameY / 54;
				int num128 = i - (tile.frameX - num126 * 36) / 18;
				int num129 = j - (tile.frameY - num127 * 54) / 18;
				if (CheckMech(num128, num129, 900))
				{
					Vector2 vector2 = new Vector2(num128 + 1, num129) * 16f;
					vector2.Y += 28f;
					int num130 = 99;
					int damage2 = 70;
					float knockBack2 = 10f;
					if (num130 != 0)
					{
						Projectile.NewProjectile(GetProjectileSource(num128, num129), (int)vector2.X, (int)vector2.Y, 0f, 0f, num130, damage2, knockBack2, Main.myPlayer);
					}
				}
				break;
			}
			case 35:
			case 139:
				WorldGen.SwitchMB(i, j);
				break;
			case 207:
				WorldGen.SwitchFountain(i, j);
				break;
			case 410:
			case 480:
			case 509:
			case 657:
			case 658:
				WorldGen.SwitchMonolith(i, j);
				break;
			case 455:
				BirthdayParty.ToggleManualParty();
				break;
			case 141:
				WorldGen.KillTile(i, j, fail: false, effectOnly: false, noItem: true);
				NetMessage.SendTileSquare(-1, i, j);
				Projectile.NewProjectile(GetProjectileSource(i, j), i * 16 + 8, j * 16 + 8, 0f, 0f, 108, 500, 10f, Main.myPlayer);
				break;
			case 210:
				WorldGen.ExplodeMine(i, j, fromWiring: true);
				break;
			case 142:
			case 143:
			{
				int num92 = j - tile.frameY / 18;
				int num93 = tile.frameX / 18;
				if (num93 > 1)
				{
					num93 -= 2;
				}
				num93 = i - num93;
				SkipWire(num93, num92);
				SkipWire(num93, num92 + 1);
				SkipWire(num93 + 1, num92);
				SkipWire(num93 + 1, num92 + 1);
				if (type == 142)
				{
					for (int num94 = 0; num94 < 4; num94++)
					{
						if (_numInPump >= 19)
						{
							break;
						}
						int num95;
						int num96;
						switch (num94)
						{
						case 0:
							num95 = num93;
							num96 = num92 + 1;
							break;
						case 1:
							num95 = num93 + 1;
							num96 = num92 + 1;
							break;
						case 2:
							num95 = num93;
							num96 = num92;
							break;
						default:
							num95 = num93 + 1;
							num96 = num92;
							break;
						}
						_inPumpX[_numInPump] = num95;
						_inPumpY[_numInPump] = num96;
						_numInPump++;
					}
					break;
				}
				for (int num97 = 0; num97 < 4; num97++)
				{
					if (_numOutPump >= 19)
					{
						break;
					}
					int num95;
					int num96;
					switch (num97)
					{
					case 0:
						num95 = num93;
						num96 = num92 + 1;
						break;
					case 1:
						num95 = num93 + 1;
						num96 = num92 + 1;
						break;
					case 2:
						num95 = num93;
						num96 = num92;
						break;
					default:
						num95 = num93 + 1;
						num96 = num92;
						break;
					}
					_outPumpX[_numOutPump] = num95;
					_outPumpY[_numOutPump] = num96;
					_numOutPump++;
				}
				break;
			}
			case 105:
			{
				int num108 = j - tile.frameY / 18;
				int num109 = tile.frameX / 18;
				int num110 = 0;
				while (num109 >= 2)
				{
					num109 -= 2;
					num110++;
				}
				num109 = i - num109;
				num109 = i - tile.frameX % 36 / 18;
				num108 = j - tile.frameY % 54 / 18;
				int num111 = tile.frameY / 54;
				num111 %= 3;
				num110 = tile.frameX / 36 + num111 * 55;
				SkipWire(num109, num108);
				SkipWire(num109, num108 + 1);
				SkipWire(num109, num108 + 2);
				SkipWire(num109 + 1, num108);
				SkipWire(num109 + 1, num108 + 1);
				SkipWire(num109 + 1, num108 + 2);
				int num112 = num109 * 16 + 16;
				int num113 = (num108 + 3) * 16;
				int num114 = -1;
				int num115 = -1;
				bool flag11 = true;
				bool flag12 = false;
				switch (num110)
				{
				case 5:
					num115 = 73;
					break;
				case 13:
					num115 = 24;
					break;
				case 30:
					num115 = 6;
					break;
				case 35:
					num115 = 2;
					break;
				case 51:
					num115 = Utils.SelectRandom(Main.rand, new short[2] { 299, 538 });
					break;
				case 52:
					num115 = 356;
					break;
				case 53:
					num115 = 357;
					break;
				case 54:
					num115 = Utils.SelectRandom(Main.rand, new short[2] { 355, 358 });
					break;
				case 55:
					num115 = Utils.SelectRandom(Main.rand, new short[2] { 367, 366 });
					break;
				case 56:
					num115 = Utils.SelectRandom(Main.rand, new short[5] { 359, 359, 359, 359, 360 });
					break;
				case 57:
					num115 = 377;
					break;
				case 58:
					num115 = 300;
					break;
				case 59:
					num115 = Utils.SelectRandom(Main.rand, new short[2] { 364, 362 });
					break;
				case 60:
					num115 = 148;
					break;
				case 61:
					num115 = 361;
					break;
				case 62:
					num115 = Utils.SelectRandom(Main.rand, new short[3] { 487, 486, 485 });
					break;
				case 63:
					num115 = 164;
					flag11 &= NPC.MechSpawn(num112, num113, 165);
					break;
				case 64:
					num115 = 86;
					flag12 = true;
					break;
				case 65:
					num115 = 490;
					break;
				case 66:
					num115 = 82;
					break;
				case 67:
					num115 = 449;
					break;
				case 68:
					num115 = 167;
					break;
				case 69:
					num115 = 480;
					break;
				case 70:
					num115 = 48;
					break;
				case 71:
					num115 = Utils.SelectRandom(Main.rand, new short[3] { 170, 180, 171 });
					flag12 = true;
					break;
				case 72:
					num115 = 481;
					break;
				case 73:
					num115 = 482;
					break;
				case 74:
					num115 = 430;
					break;
				case 75:
					num115 = 489;
					break;
				case 76:
					num115 = 611;
					break;
				case 77:
					num115 = 602;
					break;
				case 78:
					num115 = Utils.SelectRandom(Main.rand, new short[6] { 595, 596, 599, 597, 600, 598 });
					break;
				case 79:
					num115 = Utils.SelectRandom(Main.rand, new short[2] { 616, 617 });
					break;
				case 80:
					num115 = Utils.SelectRandom(Main.rand, new short[2] { 671, 672 });
					break;
				case 81:
					num115 = 673;
					break;
				case 82:
					num115 = Utils.SelectRandom(Main.rand, new short[2] { 674, 675 });
					break;
				}
				if (num115 != -1 && CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, num115) && flag11)
				{
					if (!flag12 || !Collision.SolidTiles(num109 - 2, num109 + 3, num108, num108 + 2))
					{
						num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113, num115);
					}
					else
					{
						Vector2 position = new Vector2(num112 - 4, num113 - 22) - new Vector2(10f);
						Utils.PoofOfSmoke(position);
						NetMessage.SendData(106, -1, -1, null, (int)position.X, position.Y);
					}
				}
				if (num114 <= -1)
				{
					switch (num110)
					{
					case 4:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 1))
						{
							num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113 - 12, 1);
						}
						break;
					case 7:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 49))
						{
							num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112 - 4, num113 - 6, 49);
						}
						break;
					case 8:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 55))
						{
							num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113 - 12, 55);
						}
						break;
					case 9:
					{
						int type4 = 46;
						if (BirthdayParty.PartyIsUp)
						{
							type4 = 540;
						}
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, type4))
						{
							num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113 - 12, type4);
						}
						break;
					}
					case 10:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 21))
						{
							num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113, 21);
						}
						break;
					case 16:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 42))
						{
							if (!Collision.SolidTiles(num109 - 1, num109 + 1, num108, num108 + 1))
							{
								num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113 - 12, 42);
								break;
							}
							Vector2 position3 = new Vector2(num112 - 4, num113 - 22) - new Vector2(10f);
							Utils.PoofOfSmoke(position3);
							NetMessage.SendData(106, -1, -1, null, (int)position3.X, position3.Y);
						}
						break;
					case 18:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 67))
						{
							num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113 - 12, 67);
						}
						break;
					case 23:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 63))
						{
							num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113 - 12, 63);
						}
						break;
					case 27:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 85))
						{
							num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112 - 9, num113, 85);
						}
						break;
					case 28:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 74))
						{
							num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113 - 12, Utils.SelectRandom(Main.rand, new short[3] { 74, 297, 298 }));
						}
						break;
					case 34:
					{
						for (int num124 = 0; num124 < 2; num124++)
						{
							for (int num125 = 0; num125 < 3; num125++)
							{
								Tile tile2 = Main.tile[num109 + num124, num108 + num125];
								tile2.type = 349;
								tile2.frameX = (short)(num124 * 18 + 216);
								tile2.frameY = (short)(num125 * 18);
							}
						}
						Animation.NewTemporaryAnimation(0, 349, num109, num108);
						if (Main.netMode == 2)
						{
							NetMessage.SendTileSquare(-1, num109, num108, 2, 3);
						}
						break;
					}
					case 42:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 58))
						{
							num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113 - 12, 58);
						}
						break;
					case 37:
						if (CheckMech(num109, num108, 600) && Item.MechSpawn(num112, num113, 58) && Item.MechSpawn(num112, num113, 1734) && Item.MechSpawn(num112, num113, 1867))
						{
							Item.NewItem(GetItemSource(num112, num113), num112, num113 - 16, 0, 0, 58);
						}
						break;
					case 50:
						if (CheckMech(num109, num108, 30) && NPC.MechSpawn(num112, num113, 65))
						{
							if (!Collision.SolidTiles(num109 - 2, num109 + 3, num108, num108 + 2))
							{
								num114 = NPC.NewNPC(GetNPCSource(num109, num108), num112, num113 - 12, 65);
								break;
							}
							Vector2 position2 = new Vector2(num112 - 4, num113 - 22) - new Vector2(10f);
							Utils.PoofOfSmoke(position2);
							NetMessage.SendData(106, -1, -1, null, (int)position2.X, position2.Y);
						}
						break;
					case 2:
						if (CheckMech(num109, num108, 600) && Item.MechSpawn(num112, num113, 184) && Item.MechSpawn(num112, num113, 1735) && Item.MechSpawn(num112, num113, 1868))
						{
							Item.NewItem(GetItemSource(num112, num113), num112, num113 - 16, 0, 0, 184);
						}
						break;
					case 17:
						if (CheckMech(num109, num108, 600) && Item.MechSpawn(num112, num113, 166))
						{
							Item.NewItem(GetItemSource(num112, num113), num112, num113 - 20, 0, 0, 166);
						}
						break;
					case 40:
					{
						if (!CheckMech(num109, num108, 300))
						{
							break;
						}
						int num120 = 50;
						int[] array2 = new int[num120];
						int num121 = 0;
						for (int num122 = 0; num122 < 200; num122++)
						{
							if (Main.npc[num122].active && (Main.npc[num122].type == 17 || Main.npc[num122].type == 19 || Main.npc[num122].type == 22 || Main.npc[num122].type == 38 || Main.npc[num122].type == 54 || Main.npc[num122].type == 107 || Main.npc[num122].type == 108 || Main.npc[num122].type == 142 || Main.npc[num122].type == 160 || Main.npc[num122].type == 207 || Main.npc[num122].type == 209 || Main.npc[num122].type == 227 || Main.npc[num122].type == 228 || Main.npc[num122].type == 229 || Main.npc[num122].type == 368 || Main.npc[num122].type == 369 || Main.npc[num122].type == 550 || Main.npc[num122].type == 441 || Main.npc[num122].type == 588))
							{
								array2[num121] = num122;
								num121++;
								if (num121 >= num120)
								{
									break;
								}
							}
						}
						if (num121 > 0)
						{
							int num123 = array2[Main.rand.Next(num121)];
							Main.npc[num123].position.X = num112 - Main.npc[num123].width / 2;
							Main.npc[num123].position.Y = num113 - Main.npc[num123].height - 1;
							NetMessage.SendData(23, -1, -1, null, num123);
						}
						break;
					}
					case 41:
					{
						if (!CheckMech(num109, num108, 300))
						{
							break;
						}
						int num116 = 50;
						int[] array = new int[num116];
						int num117 = 0;
						for (int num118 = 0; num118 < 200; num118++)
						{
							if (Main.npc[num118].active && (Main.npc[num118].type == 18 || Main.npc[num118].type == 20 || Main.npc[num118].type == 124 || Main.npc[num118].type == 178 || Main.npc[num118].type == 208 || Main.npc[num118].type == 353 || Main.npc[num118].type == 633 || Main.npc[num118].type == 663))
							{
								array[num117] = num118;
								num117++;
								if (num117 >= num116)
								{
									break;
								}
							}
						}
						if (num117 > 0)
						{
							int num119 = array[Main.rand.Next(num117)];
							Main.npc[num119].position.X = num112 - Main.npc[num119].width / 2;
							Main.npc[num119].position.Y = num113 - Main.npc[num119].height - 1;
							NetMessage.SendData(23, -1, -1, null, num119);
						}
						break;
					}
					}
				}
				if (num114 >= 0)
				{
					Main.npc[num114].value = 0f;
					Main.npc[num114].npcSlots = 0f;
					Main.npc[num114].SpawnedFromStatue = true;
				}
				break;
			}
			case 349:
			{
				int num86 = tile.frameY / 18;
				num86 %= 3;
				int num87 = j - num86;
				int num88;
				for (num88 = tile.frameX / 18; num88 >= 2; num88 -= 2)
				{
				}
				num88 = i - num88;
				SkipWire(num88, num87);
				SkipWire(num88, num87 + 1);
				SkipWire(num88, num87 + 2);
				SkipWire(num88 + 1, num87);
				SkipWire(num88 + 1, num87 + 1);
				SkipWire(num88 + 1, num87 + 2);
				short num89 = (short)((Main.tile[num88, num87].frameX != 0) ? (-216) : 216);
				for (int num90 = 0; num90 < 2; num90++)
				{
					for (int num91 = 0; num91 < 3; num91++)
					{
						Main.tile[num88 + num90, num87 + num91].frameX += num89;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendTileSquare(-1, num88, num87, 2, 3);
				}
				Animation.NewTemporaryAnimation((num89 <= 0) ? 1 : 0, 349, num88, num87);
				break;
			}
			case 506:
			{
				int num69 = tile.frameY / 18;
				num69 %= 3;
				int num70 = j - num69;
				int num71;
				for (num71 = tile.frameX / 18; num71 >= 2; num71 -= 2)
				{
				}
				num71 = i - num71;
				SkipWire(num71, num70);
				SkipWire(num71, num70 + 1);
				SkipWire(num71, num70 + 2);
				SkipWire(num71 + 1, num70);
				SkipWire(num71 + 1, num70 + 1);
				SkipWire(num71 + 1, num70 + 2);
				short num72 = (short)((Main.tile[num71, num70].frameX >= 72) ? (-72) : 72);
				for (int num73 = 0; num73 < 2; num73++)
				{
					for (int num74 = 0; num74 < 3; num74++)
					{
						Main.tile[num71 + num73, num70 + num74].frameX += num72;
					}
				}
				if (Main.netMode == 2)
				{
					NetMessage.SendTileSquare(-1, num71, num70, 2, 3);
				}
				break;
			}
			case 546:
				tile.type = 557;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
				break;
			case 557:
				tile.type = 546;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
				break;
			}
		}

		public static void ToggleHolidayLight(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
		{
			bool flag = tileCache.frameX >= 54;
			if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
			{
				if (tileCache.frameX < 54)
				{
					tileCache.frameX += 54;
				}
				else
				{
					tileCache.frameX -= 54;
				}
				NetMessage.SendTileSquare(-1, i, j);
			}
		}

		public static void ToggleHangingLantern(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int num;
			for (num = tileCache.frameY / 18; num >= 2; num -= 2)
			{
			}
			int num2 = j - num;
			short num3 = 18;
			if (tileCache.frameX > 0)
			{
				num3 = -18;
			}
			bool flag = tileCache.frameX > 0;
			if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
			{
				Main.tile[i, num2].frameX += num3;
				Main.tile[i, num2 + 1].frameX += num3;
				if (doSkipWires)
				{
					SkipWire(i, num2);
					SkipWire(i, num2 + 1);
				}
				NetMessage.SendTileSquare(-1, i, j, 1, 2);
			}
		}

		public static void Toggle2x2Light(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int num;
			for (num = tileCache.frameY / 18; num >= 2; num -= 2)
			{
			}
			num = j - num;
			int num2 = tileCache.frameX / 18;
			if (num2 > 1)
			{
				num2 -= 2;
			}
			num2 = i - num2;
			short num3 = 36;
			if (Main.tile[num2, num].frameX > 0)
			{
				num3 = -36;
			}
			bool flag = Main.tile[num2, num].frameX > 0;
			if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
			{
				Main.tile[num2, num].frameX += num3;
				Main.tile[num2, num + 1].frameX += num3;
				Main.tile[num2 + 1, num].frameX += num3;
				Main.tile[num2 + 1, num + 1].frameX += num3;
				if (doSkipWires)
				{
					SkipWire(num2, num);
					SkipWire(num2 + 1, num);
					SkipWire(num2, num + 1);
					SkipWire(num2 + 1, num + 1);
				}
				NetMessage.SendTileSquare(-1, num2, num, 2, 2);
			}
		}

		public static void ToggleLampPost(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int num = j - tileCache.frameY / 18;
			short num2 = 18;
			if (tileCache.frameX > 0)
			{
				num2 = -18;
			}
			bool flag = tileCache.frameX > 0;
			if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			for (int k = num; k < num + 6; k++)
			{
				Main.tile[i, k].frameX += num2;
				if (doSkipWires)
				{
					SkipWire(i, k);
				}
			}
			NetMessage.SendTileSquare(-1, i, num, 1, 6);
		}

		public static void ToggleTorch(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
		{
			bool flag = tileCache.frameX >= 66;
			if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
			{
				if (tileCache.frameX < 66)
				{
					tileCache.frameX += 66;
				}
				else
				{
					tileCache.frameX -= 66;
				}
				NetMessage.SendTileSquare(-1, i, j);
			}
		}

		public static void ToggleCandle(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
		{
			short num = 18;
			if (tileCache.frameX > 0)
			{
				num = -18;
			}
			bool flag = tileCache.frameX > 0;
			if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
			{
				tileCache.frameX += num;
				NetMessage.SendTileSquare(-1, i, j, 3);
			}
		}

		public static void ToggleLamp(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int num;
			for (num = tileCache.frameY / 18; num >= 3; num -= 3)
			{
			}
			num = j - num;
			short num2 = 18;
			if (tileCache.frameX > 0)
			{
				num2 = -18;
			}
			bool flag = tileCache.frameX > 0;
			if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
			{
				Main.tile[i, num].frameX += num2;
				Main.tile[i, num + 1].frameX += num2;
				Main.tile[i, num + 2].frameX += num2;
				if (doSkipWires)
				{
					SkipWire(i, num);
					SkipWire(i, num + 1);
					SkipWire(i, num + 2);
				}
				NetMessage.SendTileSquare(-1, i, num, 1, 3);
			}
		}

		public static void ToggleChandelier(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int num;
			for (num = tileCache.frameY / 18; num >= 3; num -= 3)
			{
			}
			int num2 = j - num;
			int num3 = tileCache.frameX % 108 / 18;
			if (num3 > 2)
			{
				num3 -= 3;
			}
			num3 = i - num3;
			short num4 = 54;
			if (Main.tile[num3, num2].frameX % 108 > 0)
			{
				num4 = -54;
			}
			bool flag = Main.tile[num3, num2].frameX % 108 > 0;
			if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			for (int k = num3; k < num3 + 3; k++)
			{
				for (int l = num2; l < num2 + 3; l++)
				{
					Main.tile[k, l].frameX += num4;
					if (doSkipWires)
					{
						SkipWire(k, l);
					}
				}
			}
			NetMessage.SendTileSquare(-1, num3 + 1, num2 + 1, 3);
		}

		public static void ToggleCampFire(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int num = tileCache.frameX % 54 / 18;
			int num2 = tileCache.frameY % 36 / 18;
			int num3 = i - num;
			int num4 = j - num2;
			bool flag = Main.tile[num3, num4].frameY >= 36;
			if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			int num5 = 36;
			if (Main.tile[num3, num4].frameY >= 36)
			{
				num5 = -36;
			}
			for (int k = num3; k < num3 + 3; k++)
			{
				for (int l = num4; l < num4 + 2; l++)
				{
					if (doSkipWires)
					{
						SkipWire(k, l);
					}
					Main.tile[k, l].frameY = (short)(Main.tile[k, l].frameY + num5);
				}
			}
			NetMessage.SendTileSquare(-1, num3, num4, 3, 2);
		}

		public static void ToggleFirePlace(int i, int j, Tile theBlock, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
		{
			int num = theBlock.frameX % 54 / 18;
			int num2 = theBlock.frameY % 36 / 18;
			int num3 = i - num;
			int num4 = j - num2;
			bool flag = Main.tile[num3, num4].frameX >= 54;
			if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
			{
				return;
			}
			int num5 = 54;
			if (Main.tile[num3, num4].frameX >= 54)
			{
				num5 = -54;
			}
			for (int k = num3; k < num3 + 3; k++)
			{
				for (int l = num4; l < num4 + 2; l++)
				{
					if (doSkipWires)
					{
						SkipWire(k, l);
					}
					Main.tile[k, l].frameX = (short)(Main.tile[k, l].frameX + num5);
				}
			}
			NetMessage.SendTileSquare(-1, num3, num4, 3, 2);
		}

		private static void GeyserTrap(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			if (tile.type != 443)
			{
				return;
			}
			int num = tile.frameX / 36;
			int num2 = i - (tile.frameX - num * 36) / 18;
			if (CheckMech(num2, j, 200))
			{
				Vector2 zero = Vector2.Zero;
				Vector2 zero2 = Vector2.Zero;
				int num3 = 654;
				int damage = 20;
				if (num < 2)
				{
					zero = new Vector2(num2 + 1, j) * 16f;
					zero2 = new Vector2(0f, -8f);
				}
				else
				{
					zero = new Vector2(num2 + 1, j + 1) * 16f;
					zero2 = new Vector2(0f, 8f);
				}
				if (num3 != 0)
				{
					Projectile.NewProjectile(GetProjectileSource(num2, j), (int)zero.X, (int)zero.Y, zero2.X, zero2.Y, num3, damage, 2f, Main.myPlayer);
				}
			}
		}

		private static void Teleport()
		{
			if (_teleport[0].X < _teleport[1].X + 3f && _teleport[0].X > _teleport[1].X - 3f && _teleport[0].Y > _teleport[1].Y - 3f && _teleport[0].Y < _teleport[1].Y)
			{
				return;
			}
			Rectangle[] array = new Rectangle[2];
			array[0].X = (int)(_teleport[0].X * 16f);
			array[0].Width = 48;
			array[0].Height = 48;
			array[0].Y = (int)(_teleport[0].Y * 16f - (float)array[0].Height);
			array[1].X = (int)(_teleport[1].X * 16f);
			array[1].Width = 48;
			array[1].Height = 48;
			array[1].Y = (int)(_teleport[1].Y * 16f - (float)array[1].Height);
			for (int i = 0; i < 2; i++)
			{
				Vector2 vector = new Vector2(array[1].X - array[0].X, array[1].Y - array[0].Y);
				if (i == 1)
				{
					vector = new Vector2(array[0].X - array[1].X, array[0].Y - array[1].Y);
				}
				if (!blockPlayerTeleportationForOneIteration)
				{
					for (int j = 0; j < 255; j++)
					{
						if (Main.player[j].active && !Main.player[j].dead && !Main.player[j].teleporting && TeleporterHitboxIntersects(array[i], Main.player[j].Hitbox))
						{
							Vector2 vector2 = Main.player[j].position + vector;
							Main.player[j].teleporting = true;
							if (Main.netMode == 2)
							{
								RemoteClient.CheckSection(j, vector2);
							}
							Main.player[j].Teleport(vector2);
							if (Main.netMode == 2)
							{
								NetMessage.SendData(65, -1, -1, null, 0, j, vector2.X, vector2.Y);
							}
						}
					}
				}
				for (int k = 0; k < 200; k++)
				{
					if (Main.npc[k].active && !Main.npc[k].teleporting && Main.npc[k].lifeMax > 5 && !Main.npc[k].boss && !Main.npc[k].noTileCollide)
					{
						int type = Main.npc[k].type;
						if (!NPCID.Sets.TeleportationImmune[type] && TeleporterHitboxIntersects(array[i], Main.npc[k].Hitbox))
						{
							Main.npc[k].teleporting = true;
							Main.npc[k].Teleport(Main.npc[k].position + vector);
						}
					}
				}
			}
			for (int l = 0; l < 255; l++)
			{
				Main.player[l].teleporting = false;
			}
			for (int m = 0; m < 200; m++)
			{
				Main.npc[m].teleporting = false;
			}
		}

		private static bool TeleporterHitboxIntersects(Rectangle teleporter, Rectangle entity)
		{
			Rectangle rectangle = Rectangle.Union(teleporter, entity);
			if (rectangle.Width <= teleporter.Width + entity.Width)
			{
				return rectangle.Height <= teleporter.Height + entity.Height;
			}
			return false;
		}

		private static void DeActive(int i, int j)
		{
			if (!Main.tile[i, j].active() || (Main.tile[i, j].type == 226 && (double)j > Main.worldSurface && !NPC.downedPlantBoss))
			{
				return;
			}
			bool flag = Main.tileSolid[Main.tile[i, j].type] && !TileID.Sets.NotReallySolid[Main.tile[i, j].type];
			ushort type = Main.tile[i, j].type;
			if (type == 314 || (uint)(type - 386) <= 3u || type == 476)
			{
				flag = false;
			}
			if (flag && (!Main.tile[i, j - 1].active() || (!TileID.Sets.BasicChest[Main.tile[i, j - 1].type] && Main.tile[i, j - 1].type != 26 && Main.tile[i, j - 1].type != 77 && Main.tile[i, j - 1].type != 88 && Main.tile[i, j - 1].type != 470 && Main.tile[i, j - 1].type != 475 && Main.tile[i, j - 1].type != 237 && Main.tile[i, j - 1].type != 597 && WorldGen.CanKillTile(i, j - 1))))
			{
				Main.tile[i, j].inActive(inActive: true);
				WorldGen.SquareTileFrame(i, j, resetFrame: false);
				if (Main.netMode != 1)
				{
					NetMessage.SendTileSquare(-1, i, j);
				}
			}
		}

		private static void ReActive(int i, int j)
		{
			Main.tile[i, j].inActive(inActive: false);
			WorldGen.SquareTileFrame(i, j, resetFrame: false);
			if (Main.netMode != 1)
			{
				NetMessage.SendTileSquare(-1, i, j);
			}
		}

		private static void MassWireOperationInner(Player user, Point ps, Point pe, Vector2 dropPoint, bool dir, ref int wireCount, ref int actuatorCount)
		{
			Math.Abs(ps.X - pe.X);
			Math.Abs(ps.Y - pe.Y);
			int num = Math.Sign(pe.X - ps.X);
			int num2 = Math.Sign(pe.Y - ps.Y);
			WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
			Point pt = default(Point);
			bool flag = false;
			Item.StartCachingType(530);
			Item.StartCachingType(849);
			bool flag2 = dir;
			int num3;
			int num4;
			int num5;
			if (flag2)
			{
				pt.X = ps.X;
				num3 = ps.Y;
				num4 = pe.Y;
				num5 = num2;
			}
			else
			{
				pt.Y = ps.Y;
				num3 = ps.X;
				num4 = pe.X;
				num5 = num;
			}
			for (int i = num3; i != num4; i += num5)
			{
				if (flag)
				{
					break;
				}
				if (flag2)
				{
					pt.Y = i;
				}
				else
				{
					pt.X = i;
				}
				bool? flag3 = MassWireOperationStep(user, pt, toolMode, ref wireCount, ref actuatorCount);
				if (flag3.HasValue && !flag3.Value)
				{
					flag = true;
					break;
				}
			}
			if (flag2)
			{
				pt.Y = pe.Y;
				num3 = ps.X;
				num4 = pe.X;
				num5 = num;
			}
			else
			{
				pt.X = pe.X;
				num3 = ps.Y;
				num4 = pe.Y;
				num5 = num2;
			}
			for (int j = num3; j != num4; j += num5)
			{
				if (flag)
				{
					break;
				}
				if (!flag2)
				{
					pt.Y = j;
				}
				else
				{
					pt.X = j;
				}
				bool? flag4 = MassWireOperationStep(user, pt, toolMode, ref wireCount, ref actuatorCount);
				if (flag4.HasValue && !flag4.Value)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				MassWireOperationStep(user, pe, toolMode, ref wireCount, ref actuatorCount);
			}
			EntitySource_ByItemSourceId reason = new EntitySource_ByItemSourceId(user, 5);
			Item.DropCache(reason, dropPoint, Vector2.Zero, 530);
			Item.DropCache(reason, dropPoint, Vector2.Zero, 849);
		}

		private static bool? MassWireOperationStep(Player user, Point pt, WiresUI.Settings.MultiToolMode mode, ref int wiresLeftToConsume, ref int actuatorsLeftToConstume)
		{
			if (!WorldGen.InWorld(pt.X, pt.Y, 1))
			{
				return null;
			}
			Tile tile = Main.tile[pt.X, pt.Y];
			if (tile == null)
			{
				return null;
			}
			if (user != null && !user.CanDoWireStuffHere(pt.X, pt.Y))
			{
				return null;
			}
			if (!mode.HasFlag(WiresUI.Settings.MultiToolMode.Cutter))
			{
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Red) && !tile.wire())
				{
					if (wiresLeftToConsume <= 0)
					{
						return false;
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 5, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Green) && !tile.wire3())
				{
					if (wiresLeftToConsume <= 0)
					{
						return false;
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire3(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 12, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Blue) && !tile.wire2())
				{
					if (wiresLeftToConsume <= 0)
					{
						return false;
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire2(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 10, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Yellow) && !tile.wire4())
				{
					if (wiresLeftToConsume <= 0)
					{
						return false;
					}
					wiresLeftToConsume--;
					WorldGen.PlaceWire4(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 16, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Actuator) && !tile.actuator())
				{
					if (actuatorsLeftToConstume <= 0)
					{
						return false;
					}
					actuatorsLeftToConstume--;
					WorldGen.PlaceActuator(pt.X, pt.Y);
					NetMessage.SendData(17, -1, -1, null, 8, pt.X, pt.Y);
				}
			}
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Cutter))
			{
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Red) && tile.wire() && WorldGen.KillWire(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 6, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Green) && tile.wire3() && WorldGen.KillWire3(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 13, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Blue) && tile.wire2() && WorldGen.KillWire2(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 11, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Yellow) && tile.wire4() && WorldGen.KillWire4(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 17, pt.X, pt.Y);
				}
				if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Actuator) && tile.actuator() && WorldGen.KillActuator(pt.X, pt.Y))
				{
					NetMessage.SendData(17, -1, -1, null, 9, pt.X, pt.Y);
				}
			}
			return true;
		}
	}
}
