using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Enums;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;

namespace Terraria.GameContent
{
	public class SmartCursorHelper
	{
		private class SmartCursorUsageInfo
		{
			public Player player;

			public Item item;

			public Vector2 mouse;

			public Vector2 position;

			public Vector2 Center;

			public int screenTargetX;

			public int screenTargetY;

			public int reachableStartX;

			public int reachableEndX;

			public int reachableStartY;

			public int reachableEndY;

			public int paintLookup;

			public int paintCoatingLookup;
		}

		private static List<Tuple<int, int>> _targets = new List<Tuple<int, int>>();

		private static List<Tuple<int, int>> _grappleTargets = new List<Tuple<int, int>>();

		private static List<Tuple<int, int>> _points = new List<Tuple<int, int>>();

		private static List<Tuple<int, int>> _endpoints = new List<Tuple<int, int>>();

		private static List<Tuple<int, int>> _toRemove = new List<Tuple<int, int>>();

		private static List<Tuple<int, int>> _targets2 = new List<Tuple<int, int>>();

		public static void SmartCursorLookup(Player player)
		{
			Main.SmartCursorShowing = false;
			if (!Main.SmartCursorIsUsed)
			{
				return;
			}
			SmartCursorUsageInfo smartCursorUsageInfo = new SmartCursorUsageInfo
			{
				player = player,
				item = player.inventory[player.selectedItem],
				mouse = Main.MouseWorld,
				position = player.position,
				Center = player.Center
			};
			_ = player.gravDir;
			int tileTargetX = Player.tileTargetX;
			int tileTargetY = Player.tileTargetY;
			int tileRangeX = Player.tileRangeX;
			int tileRangeY = Player.tileRangeY;
			smartCursorUsageInfo.screenTargetX = Utils.Clamp(tileTargetX, 10, Main.maxTilesX - 10);
			smartCursorUsageInfo.screenTargetY = Utils.Clamp(tileTargetY, 10, Main.maxTilesY - 10);
			if (Main.tile[smartCursorUsageInfo.screenTargetX, smartCursorUsageInfo.screenTargetY] == null)
			{
				return;
			}
			bool num = IsHoveringOverAnInteractibleTileThatBlocksSmartCursor(smartCursorUsageInfo);
			TryFindingPaintInplayerInventory(smartCursorUsageInfo, out smartCursorUsageInfo.paintLookup, out smartCursorUsageInfo.paintCoatingLookup);
			int tileBoost = smartCursorUsageInfo.item.tileBoost;
			smartCursorUsageInfo.reachableStartX = (int)(player.position.X / 16f) - tileRangeX - tileBoost + 1;
			smartCursorUsageInfo.reachableEndX = (int)((player.position.X + (float)player.width) / 16f) + tileRangeX + tileBoost - 1;
			smartCursorUsageInfo.reachableStartY = (int)(player.position.Y / 16f) - tileRangeY - tileBoost + 1;
			smartCursorUsageInfo.reachableEndY = (int)((player.position.Y + (float)player.height) / 16f) + tileRangeY + tileBoost - 2;
			smartCursorUsageInfo.reachableStartX = Utils.Clamp(smartCursorUsageInfo.reachableStartX, 10, Main.maxTilesX - 10);
			smartCursorUsageInfo.reachableEndX = Utils.Clamp(smartCursorUsageInfo.reachableEndX, 10, Main.maxTilesX - 10);
			smartCursorUsageInfo.reachableStartY = Utils.Clamp(smartCursorUsageInfo.reachableStartY, 10, Main.maxTilesY - 10);
			smartCursorUsageInfo.reachableEndY = Utils.Clamp(smartCursorUsageInfo.reachableEndY, 10, Main.maxTilesY - 10);
			if (!num || smartCursorUsageInfo.screenTargetX < smartCursorUsageInfo.reachableStartX || smartCursorUsageInfo.screenTargetX > smartCursorUsageInfo.reachableEndX || smartCursorUsageInfo.screenTargetY < smartCursorUsageInfo.reachableStartY || smartCursorUsageInfo.screenTargetY > smartCursorUsageInfo.reachableEndY)
			{
				_grappleTargets.Clear();
				int[] grappling = player.grappling;
				int grapCount = player.grapCount;
				for (int i = 0; i < grapCount; i++)
				{
					Projectile obj = Main.projectile[grappling[i]];
					int item = (int)obj.Center.X / 16;
					int item2 = (int)obj.Center.Y / 16;
					_grappleTargets.Add(new Tuple<int, int>(item, item2));
				}
				int fX = -1;
				int fY = -1;
				if (!Player.SmartCursorSettings.SmartAxeAfterPickaxe)
				{
					Step_Axe(smartCursorUsageInfo, ref fX, ref fY);
				}
				Step_ForceCursorToAnyMinableThing(smartCursorUsageInfo, ref fX, ref fY);
				Step_Pickaxe_MineShinies(smartCursorUsageInfo, ref fX, ref fY);
				Step_Pickaxe_MineSolids(player, smartCursorUsageInfo, _grappleTargets, ref fX, ref fY);
				if (Player.SmartCursorSettings.SmartAxeAfterPickaxe)
				{
					Step_Axe(smartCursorUsageInfo, ref fX, ref fY);
				}
				Step_ColoredWrenches(smartCursorUsageInfo, ref fX, ref fY);
				Step_MulticolorWrench(smartCursorUsageInfo, ref fX, ref fY);
				Step_Hammers(smartCursorUsageInfo, ref fX, ref fY);
				Step_ActuationRod(smartCursorUsageInfo, ref fX, ref fY);
				Step_WireCutter(smartCursorUsageInfo, ref fX, ref fY);
				Step_Platforms(smartCursorUsageInfo, ref fX, ref fY);
				Step_MinecartTracks(smartCursorUsageInfo, ref fX, ref fY);
				Step_Walls(smartCursorUsageInfo, ref fX, ref fY);
				Step_PumpkinSeeds(smartCursorUsageInfo, ref fX, ref fY);
				Step_Pigronata(smartCursorUsageInfo, ref fX, ref fY);
				Step_Boulders(smartCursorUsageInfo, ref fX, ref fY);
				Step_Torch(smartCursorUsageInfo, ref fX, ref fY);
				Step_LawnMower(smartCursorUsageInfo, ref fX, ref fY);
				Step_BlocksFilling(smartCursorUsageInfo, ref fX, ref fY);
				Step_BlocksLines(smartCursorUsageInfo, ref fX, ref fY);
				Step_PaintRoller(smartCursorUsageInfo, ref fX, ref fY);
				Step_PaintBrush(smartCursorUsageInfo, ref fX, ref fY);
				Step_PaintScrapper(smartCursorUsageInfo, ref fX, ref fY);
				Step_Acorns(smartCursorUsageInfo, ref fX, ref fY);
				Step_GemCorns(smartCursorUsageInfo, ref fX, ref fY);
				Step_EmptyBuckets(smartCursorUsageInfo, ref fX, ref fY);
				Step_Actuators(smartCursorUsageInfo, ref fX, ref fY);
				Step_AlchemySeeds(smartCursorUsageInfo, ref fX, ref fY);
				Step_PlanterBox(smartCursorUsageInfo, ref fX, ref fY);
				Step_ClayPots(smartCursorUsageInfo, ref fX, ref fY);
				Step_StaffOfRegrowth(smartCursorUsageInfo, ref fX, ref fY);
				if (fX != -1 && fY != -1)
				{
					Main.SmartCursorX = (Player.tileTargetX = fX);
					Main.SmartCursorY = (Player.tileTargetY = fY);
					Main.SmartCursorShowing = true;
				}
				_grappleTargets.Clear();
			}
		}

		private static void TryFindingPaintInplayerInventory(SmartCursorUsageInfo providedInfo, out int paintLookup, out int coatingLookup)
		{
			_ = providedInfo.player.inventory;
			paintLookup = 0;
			coatingLookup = 0;
			if (providedInfo.item.type == 1071 || providedInfo.item.type == 1543 || providedInfo.item.type == 1072 || providedInfo.item.type == 1544)
			{
				Item item = providedInfo.player.FindPaintOrCoating();
				if (item != null)
				{
					coatingLookup = item.paintCoating;
					paintLookup = item.paint;
				}
			}
		}

		private static bool IsHoveringOverAnInteractibleTileThatBlocksSmartCursor(SmartCursorUsageInfo providedInfo)
		{
			bool result = false;
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active())
			{
				switch (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type)
				{
				case 4:
				case 10:
				case 11:
				case 13:
				case 21:
				case 29:
				case 33:
				case 49:
				case 50:
				case 55:
				case 79:
				case 85:
				case 88:
				case 97:
				case 104:
				case 125:
				case 132:
				case 136:
				case 139:
				case 144:
				case 174:
				case 207:
				case 209:
				case 212:
				case 216:
				case 219:
				case 237:
				case 287:
				case 334:
				case 335:
				case 338:
				case 354:
				case 386:
				case 387:
				case 388:
				case 389:
				case 411:
				case 425:
				case 441:
				case 463:
				case 464:
				case 467:
				case 468:
				case 491:
				case 494:
				case 510:
				case 511:
				case 573:
				case 621:
				case 642:
					result = true;
					break;
				case 314:
					if (providedInfo.player.gravDir == 1f)
					{
						result = true;
					}
					break;
				}
			}
			return result;
		}

		private static void Step_StaffOfRegrowth(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if ((providedInfo.item.type != 213 && providedInfo.item.type != 5295) || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					bool flag = !Main.tile[i - 1, j].active() || !Main.tile[i, j + 1].active() || !Main.tile[i + 1, j].active() || !Main.tile[i, j - 1].active();
					bool flag2 = !Main.tile[i - 1, j - 1].active() || !Main.tile[i - 1, j + 1].active() || !Main.tile[i + 1, j + 1].active() || !Main.tile[i + 1, j - 1].active();
					if (tile.active() && !tile.inActive() && tile.type == 0 && (flag || (tile.type == 0 && flag2)))
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_ClayPots(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile != 78 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			bool flag = false;
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active())
			{
				flag = true;
			}
			if (!Collision.InTileBounds(providedInfo.screenTargetX, providedInfo.screenTargetY, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						Tile tile2 = Main.tile[i, j + 1];
						if ((!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type]) && tile2.nactive() && !tile2.halfBrick() && tile2.slope() == 0 && Main.tileSolid[tile2.type])
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					if (Collision.EmptyTile(_targets[k].Item1, _targets[k].Item2, ignoreTiles: true))
					{
						float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							tuple = _targets[k];
						}
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_PlanterBox(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile != 380 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			bool flag = false;
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 380)
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.active() && tile.type == 380)
						{
							if (!Main.tile[i - 1, j].active() || Main.tileCut[Main.tile[i - 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j].type])
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (!Main.tile[i + 1, j].active() || Main.tileCut[Main.tile[i + 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j].type])
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_AlchemySeeds(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile != 82 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			int placeStyle = providedInfo.item.placeStyle;
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					Tile tile2 = Main.tile[i, j + 1];
					bool num = !tile.active() || TileID.Sets.BreakableWhenPlacing[tile.type] || (Main.tileCut[tile.type] && tile.type != 82 && tile.type != 83) || WorldGen.IsHarvestableHerbWithSeed(tile.type, tile.frameX / 18);
					bool flag = tile2.nactive() && !tile2.halfBrick() && tile2.slope() == 0;
					if (!num || !flag)
					{
						continue;
					}
					switch (placeStyle)
					{
					case 0:
						if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 2 && tile2.type != 477 && tile2.type != 109 && tile2.type != 492) || tile.liquid > 0)
						{
							continue;
						}
						break;
					case 1:
						if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 60) || tile.liquid > 0)
						{
							continue;
						}
						break;
					case 2:
						if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 0 && tile2.type != 59) || tile.liquid > 0)
						{
							continue;
						}
						break;
					case 3:
						if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 203 && tile2.type != 199 && tile2.type != 23 && tile2.type != 25) || tile.liquid > 0)
						{
							continue;
						}
						break;
					case 4:
						if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 53 && tile2.type != 116) || (tile.liquid > 0 && tile.lava()))
						{
							continue;
						}
						break;
					case 5:
						if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 57) || (tile.liquid > 0 && !tile.lava()))
						{
							continue;
						}
						break;
					case 6:
						if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 147 && tile2.type != 161 && tile2.type != 163 && tile2.type != 164 && tile2.type != 200) || (tile.liquid > 0 && tile.lava()))
						{
							continue;
						}
						break;
					}
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
			if (_targets.Count > 0)
			{
				float num2 = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num3 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num2 == -1f || num3 < num2)
					{
						num2 = num3;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_Actuators(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.type != 849 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if ((tile.wire() || tile.wire2() || tile.wire3() || tile.wire4()) && !tile.actuator() && tile.active())
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_EmptyBuckets(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.type != 205 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.liquid <= 0)
					{
						continue;
					}
					int num = tile.liquidType();
					int num2 = 0;
					for (int k = i - 1; k <= i + 1; k++)
					{
						for (int l = j - 1; l <= j + 1; l++)
						{
							if (Main.tile[k, l].liquidType() == num)
							{
								num2 += Main.tile[k, l].liquid;
							}
						}
					}
					if (num2 > 100)
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num3 = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int m = 0; m < _targets.Count; m++)
				{
					float num4 = Vector2.Distance(new Vector2(_targets[m].Item1, _targets[m].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num3 == -1f || num4 < num3)
					{
						num3 = num4;
						tuple = _targets[m];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_PaintScrapper(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (!ItemID.Sets.IsPaintScraper[providedInfo.item.type] || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					bool flag = false;
					if (tile.active())
					{
						flag |= tile.color() > 0;
						flag |= tile.type == 184;
						flag |= tile.fullbrightBlock();
						flag |= tile.invisibleBlock();
					}
					if (tile.wall > 0)
					{
						flag |= tile.wallColor() > 0;
						flag |= tile.fullbrightWall();
						flag |= tile.invisibleWall();
					}
					if (flag)
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_PaintBrush(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if ((providedInfo.item.type != 1071 && providedInfo.item.type != 1543) || (providedInfo.paintLookup == 0 && providedInfo.paintCoatingLookup == 0) || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			int paintLookup = providedInfo.paintLookup;
			int paintCoatingLookup = providedInfo.paintCoatingLookup;
			if (paintLookup != 0 || paintCoatingLookup != 0)
			{
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.active() && (false | (paintLookup != 0 && tile.color() != paintLookup) | (paintCoatingLookup == 1 && !tile.fullbrightBlock()) | (paintCoatingLookup == 2 && !tile.invisibleBlock())))
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_PaintRoller(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if ((providedInfo.item.type != 1072 && providedInfo.item.type != 1544) || (providedInfo.paintLookup == 0 && providedInfo.paintCoatingLookup == 0) || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			int paintLookup = providedInfo.paintLookup;
			int paintCoatingLookup = providedInfo.paintCoatingLookup;
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.wall > 0 && (!tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && (false | (paintLookup != 0 && tile.wallColor() != paintLookup) | (paintCoatingLookup == 1 && !tile.fullbrightWall()) | (paintCoatingLookup == 2 && !tile.invisibleWall())))
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_BlocksLines(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (!Player.SmartCursorSettings.SmartBlocksEnabled || providedInfo.item.createTile <= -1 || providedInfo.item.type == 213 || providedInfo.item.type == 5295 || !Main.tileSolid[providedInfo.item.createTile] || Main.tileSolidTop[providedInfo.item.createTile] || Main.tileFrameImportant[providedInfo.item.createTile] || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			bool flag = false;
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active())
			{
				flag = true;
			}
			if (!Collision.InTileBounds(providedInfo.screenTargetX, providedInfo.screenTargetY, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type])
						{
							bool flag2 = false;
							if (Main.tile[i - 1, j].active() && Main.tileSolid[Main.tile[i - 1, j].type] && !Main.tileSolidTop[Main.tile[i - 1, j].type])
							{
								flag2 = true;
							}
							if (Main.tile[i + 1, j].active() && Main.tileSolid[Main.tile[i + 1, j].type] && !Main.tileSolidTop[Main.tile[i + 1, j].type])
							{
								flag2 = true;
							}
							if (Main.tile[i, j - 1].active() && Main.tileSolid[Main.tile[i, j - 1].type] && !Main.tileSolidTop[Main.tile[i, j - 1].type])
							{
								flag2 = true;
							}
							if (Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type] && !Main.tileSolidTop[Main.tile[i, j + 1].type])
							{
								flag2 = true;
							}
							if (flag2)
							{
								_targets.Add(new Tuple<int, int>(i, j));
							}
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					if (Collision.EmptyTile(_targets[k].Item1, _targets[k].Item2))
					{
						float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num == -1f || num2 < num)
						{
							num = num2;
							tuple = _targets[k];
						}
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_Boulders(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile <= -1 || providedInfo.item.createTile >= 693 || !TileID.Sets.Boulders[providedInfo.item.createTile] || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j + 1];
					Tile tile2 = Main.tile[i - 1, j + 1];
					bool flag = true;
					if (!tile2.nactive() || !tile.nactive())
					{
						flag = false;
					}
					if (tile2.slope() > 0 || tile.slope() > 0 || tile2.halfBrick() || tile.halfBrick())
					{
						flag = false;
					}
					if (Main.tileNoAttach[tile2.type] || Main.tileNoAttach[tile.type])
					{
						flag = false;
					}
					for (int k = i - 1; k <= i; k++)
					{
						for (int l = j - 1; l <= j; l++)
						{
							Tile tile3 = Main.tile[k, l];
							if (tile3.active() && !Main.tileCut[tile3.type])
							{
								flag = false;
							}
						}
					}
					int x = i * 16 - 16;
					int y = j * 16 - 16;
					int width = 32;
					int height = 32;
					Rectangle value = new Rectangle(x, y, width, height);
					for (int m = 0; m < 255; m++)
					{
						Player player = Main.player[m];
						if (player.active && !player.dead && player.Hitbox.Intersects(value))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int n = 0; n < _targets.Count; n++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[n].Item1, _targets[n].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[n];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_Pigronata(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile != 454 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY && !((double)j > Main.worldSurface - 2.0); j++)
				{
					bool flag = true;
					for (int k = i - 2; k <= i + 1; k++)
					{
						for (int l = j - 1; l <= j + 2; l++)
						{
							Tile tile = Main.tile[k, l];
							if (l == j - 1)
							{
								if (!WorldGen.SolidTile(tile))
								{
									flag = false;
								}
							}
							else if (tile.active() && (!Main.tileCut[tile.type] || tile.type == 454))
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int m = 0; m < _targets.Count; m++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[m].Item1, _targets[m].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[m];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_PumpkinSeeds(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile != 254 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j + 1];
					Tile tile2 = Main.tile[i + 1, j + 1];
					if ((double)j > Main.worldSurface - 2.0)
					{
						break;
					}
					bool flag = true;
					if (!tile2.active() || !tile.active())
					{
						flag = false;
					}
					if (tile2.slope() > 0 || tile.slope() > 0 || tile2.halfBrick() || tile.halfBrick())
					{
						flag = false;
					}
					if (tile2.type != 2 && tile2.type != 477 && tile2.type != 109 && tile2.type != 492)
					{
						flag = false;
					}
					if (tile.type != 2 && tile.type != 477 && tile.type != 109 && tile.type != 492)
					{
						flag = false;
					}
					for (int k = i; k <= i + 1; k++)
					{
						for (int l = j - 1; l <= j; l++)
						{
							Tile tile3 = Main.tile[k, l];
							if (tile3.active() && (tile3.type < 0 || tile3.type >= 693 || Main.tileSolid[tile3.type] || !WorldGen.CanCutTile(k, l, TileCuttingContext.TilePlacement)))
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int m = 0; m < _targets.Count; m++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[m].Item1, _targets[m].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[m];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_Walls(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			int width = providedInfo.player.width;
			int height = providedInfo.player.height;
			if (providedInfo.item.createWall <= 0 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.wall == 0 && (!tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && Collision.CanHitWithCheck(providedInfo.position, width, height, new Vector2(i, j) * 16f, 16, 16, DelegateMethods.NotDoorStand))
					{
						bool flag = false;
						if (Main.tile[i - 1, j].active() || Main.tile[i - 1, j].wall > 0)
						{
							flag = true;
						}
						if (Main.tile[i + 1, j].active() || Main.tile[i + 1, j].wall > 0)
						{
							flag = true;
						}
						if (Main.tile[i, j - 1].active() || Main.tile[i, j - 1].wall > 0)
						{
							flag = true;
						}
						if (Main.tile[i, j + 1].active() || Main.tile[i, j + 1].wall > 0)
						{
							flag = true;
						}
						if (WorldGen.IsOpenDoorAnchorFrame(i, j))
						{
							flag = false;
						}
						if (flag)
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_MinecartTracks(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if ((providedInfo.item.type == 2340 || providedInfo.item.type == 2739) && focusedX == -1 && focusedY == -1)
			{
				_targets.Clear();
				Vector2 value = (Main.MouseWorld - providedInfo.Center).SafeNormalize(Vector2.UnitY);
				float num = Vector2.Dot(value, -Vector2.UnitY);
				bool flag = num >= 0.5f;
				bool flag2 = num <= -0.5f;
				float num2 = Vector2.Dot(value, Vector2.UnitX);
				bool flag3 = num2 >= 0.5f;
				bool flag4 = num2 <= -0.5f;
				bool flag5 = flag && flag4;
				bool flag6 = flag && flag3;
				bool flag7 = flag2 && flag4;
				bool flag8 = flag2 && flag3;
				if (flag5)
				{
					flag4 = false;
				}
				if (flag6)
				{
					flag3 = false;
				}
				if (flag7)
				{
					flag4 = false;
				}
				if (flag8)
				{
					flag3 = false;
				}
				bool flag9 = false;
				if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 314)
				{
					flag9 = true;
				}
				if (!flag9)
				{
					for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
					{
						for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
						{
							Tile tile = Main.tile[i, j];
							if (tile.active() && tile.type == 314)
							{
								bool flag10 = Main.tile[i + 1, j + 1].active() && Main.tile[i + 1, j + 1].type == 314;
								bool flag11 = Main.tile[i + 1, j - 1].active() && Main.tile[i + 1, j - 1].type == 314;
								bool flag12 = Main.tile[i - 1, j + 1].active() && Main.tile[i - 1, j + 1].type == 314;
								bool flag13 = Main.tile[i - 1, j - 1].active() && Main.tile[i - 1, j - 1].type == 314;
								if (flag5 && (!Main.tile[i - 1, j - 1].active() || Main.tileCut[Main.tile[i - 1, j - 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j - 1].type]) && !(!flag10 && flag11) && !flag12)
								{
									_targets.Add(new Tuple<int, int>(i - 1, j - 1));
								}
								if (flag4 && (!Main.tile[i - 1, j].active() || Main.tileCut[Main.tile[i - 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j].type]))
								{
									_targets.Add(new Tuple<int, int>(i - 1, j));
								}
								if (flag7 && (!Main.tile[i - 1, j + 1].active() || Main.tileCut[Main.tile[i - 1, j + 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j + 1].type]) && !(!flag11 && flag10) && !flag13)
								{
									_targets.Add(new Tuple<int, int>(i - 1, j + 1));
								}
								if (flag6 && (!Main.tile[i + 1, j - 1].active() || Main.tileCut[Main.tile[i + 1, j - 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j - 1].type]) && !(!flag12 && flag13) && !flag10)
								{
									_targets.Add(new Tuple<int, int>(i + 1, j - 1));
								}
								if (flag3 && (!Main.tile[i + 1, j].active() || Main.tileCut[Main.tile[i + 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j].type]))
								{
									_targets.Add(new Tuple<int, int>(i + 1, j));
								}
								if (flag8 && (!Main.tile[i + 1, j + 1].active() || Main.tileCut[Main.tile[i + 1, j + 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j + 1].type]) && !(!flag13 && flag12) && !flag11)
								{
									_targets.Add(new Tuple<int, int>(i + 1, j + 1));
								}
							}
						}
					}
				}
				if (_targets.Count > 0)
				{
					float num3 = -1f;
					Tuple<int, int> tuple = _targets[0];
					for (int k = 0; k < _targets.Count; k++)
					{
						if ((!Main.tile[_targets[k].Item1, _targets[k].Item2 - 1].active() || Main.tile[_targets[k].Item1, _targets[k].Item2 - 1].type != 314) && (!Main.tile[_targets[k].Item1, _targets[k].Item2 + 1].active() || Main.tile[_targets[k].Item1, _targets[k].Item2 + 1].type != 314))
						{
							float num4 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
							if (num3 == -1f || num4 < num3)
							{
								num3 = num4;
								tuple = _targets[k];
							}
						}
					}
					if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num3 != -1f)
					{
						focusedX = tuple.Item1;
						focusedY = tuple.Item2;
					}
				}
				_targets.Clear();
			}
			if (providedInfo.item.type != 2492 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			bool flag14 = false;
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 314)
			{
				flag14 = true;
			}
			if (!flag14)
			{
				for (int l = providedInfo.reachableStartX; l <= providedInfo.reachableEndX; l++)
				{
					for (int m = providedInfo.reachableStartY; m <= providedInfo.reachableEndY; m++)
					{
						Tile tile2 = Main.tile[l, m];
						if (tile2.active() && tile2.type == 314)
						{
							if (!Main.tile[l - 1, m].active() || Main.tileCut[Main.tile[l - 1, m].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[l - 1, m].type])
							{
								_targets.Add(new Tuple<int, int>(l - 1, m));
							}
							if (!Main.tile[l + 1, m].active() || Main.tileCut[Main.tile[l + 1, m].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[l + 1, m].type])
							{
								_targets.Add(new Tuple<int, int>(l + 1, m));
							}
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num5 = -1f;
				Tuple<int, int> tuple2 = _targets[0];
				for (int n = 0; n < _targets.Count; n++)
				{
					if ((!Main.tile[_targets[n].Item1, _targets[n].Item2 - 1].active() || Main.tile[_targets[n].Item1, _targets[n].Item2 - 1].type != 314) && (!Main.tile[_targets[n].Item1, _targets[n].Item2 + 1].active() || Main.tile[_targets[n].Item1, _targets[n].Item2 + 1].type != 314))
					{
						float num6 = Vector2.Distance(new Vector2(_targets[n].Item1, _targets[n].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num5 == -1f || num6 < num5)
						{
							num5 = num6;
							tuple2 = _targets[n];
						}
					}
				}
				if (Collision.InTileBounds(tuple2.Item1, tuple2.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num5 != -1f)
				{
					focusedX = tuple2.Item1;
					focusedY = tuple2.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_Platforms(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.createTile < 0 || !TileID.Sets.Platforms[providedInfo.item.createTile] || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			bool flag = false;
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && TileID.Sets.Platforms[Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type])
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.active() && TileID.Sets.Platforms[tile.type])
						{
							byte num = tile.slope();
							if (num != 2 && !Main.tile[i - 1, j - 1].active())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j - 1));
							}
							if (!Main.tile[i - 1, j].active())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (num != 1 && !Main.tile[i - 1, j + 1].active())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j + 1));
							}
							if (num != 1 && !Main.tile[i + 1, j - 1].active())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j - 1));
							}
							if (!Main.tile[i + 1, j].active())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
							if (num != 2 && !Main.tile[i + 1, j + 1].active())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j + 1));
							}
						}
						if (!tile.active())
						{
							int num2 = 0;
							int num3 = 0;
							num2 = 0;
							num3 = 1;
							Tile tile2 = Main.tile[i + num2, j + num3];
							if (tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type])
							{
								_targets.Add(new Tuple<int, int>(i, j));
							}
							num2 = -1;
							num3 = 0;
							tile2 = Main.tile[i + num2, j + num3];
							if (tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type])
							{
								_targets.Add(new Tuple<int, int>(i, j));
							}
							num2 = 1;
							num3 = 0;
							tile2 = Main.tile[i + num2, j + num3];
							if (tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type])
							{
								_targets.Add(new Tuple<int, int>(i, j));
							}
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num4 = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num5 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num4 == -1f || num5 < num4)
					{
						num4 = num5;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_WireCutter(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.type != 510 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.wire() || tile.wire2() || tile.wire3() || tile.wire4() || tile.actuator())
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_ActuationRod(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			bool actuationRodLock = providedInfo.player.ActuationRodLock;
			bool actuationRodLockSetting = providedInfo.player.ActuationRodLockSetting;
			if (providedInfo.item.type != 3620 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && tile.actuator() && (!actuationRodLock || actuationRodLockSetting == tile.inActive()))
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_Hammers(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			int width = providedInfo.player.width;
			int height = providedInfo.player.height;
			if (providedInfo.item.hammer > 0 && focusedX == -1 && focusedY == -1)
			{
				Vector2 vector = providedInfo.mouse - providedInfo.Center;
				int num = Math.Sign(vector.X);
				int num2 = Math.Sign(vector.Y);
				if (Math.Abs(vector.X) > Math.Abs(vector.Y) * 3f)
				{
					num2 = 0;
					providedInfo.mouse.Y = providedInfo.Center.Y;
				}
				if (Math.Abs(vector.Y) > Math.Abs(vector.X) * 3f)
				{
					num = 0;
					providedInfo.mouse.X = providedInfo.Center.X;
				}
				_ = (int)providedInfo.Center.X / 16;
				_ = (int)providedInfo.Center.Y / 16;
				_points.Clear();
				_endpoints.Clear();
				int num3 = 1;
				if (num2 == -1 && num != 0)
				{
					num3 = -1;
				}
				int num4 = (int)((providedInfo.position.X + (float)(width / 2) + (float)((width / 2 - 1) * num)) / 16f);
				int num5 = (int)(((double)providedInfo.position.Y + 0.1) / 16.0);
				if (num3 == -1)
				{
					num5 = (int)((providedInfo.position.Y + (float)height - 1f) / 16f);
				}
				int num6 = width / 16 + ((width % 16 != 0) ? 1 : 0);
				int num7 = height / 16 + ((height % 16 != 0) ? 1 : 0);
				if (num != 0)
				{
					for (int i = 0; i < num7; i++)
					{
						if (Main.tile[num4, num5 + i * num3] != null)
						{
							_points.Add(new Tuple<int, int>(num4, num5 + i * num3));
						}
					}
				}
				if (num2 != 0)
				{
					for (int j = 0; j < num6; j++)
					{
						if (Main.tile[(int)(providedInfo.position.X / 16f) + j, num5] != null)
						{
							_points.Add(new Tuple<int, int>((int)(providedInfo.position.X / 16f) + j, num5));
						}
					}
				}
				int num8 = (int)((providedInfo.mouse.X + (float)((width / 2 - 1) * num)) / 16f);
				int num9 = (int)(((double)providedInfo.mouse.Y + 0.1 - (double)(height / 2 + 1)) / 16.0);
				if (num3 == -1)
				{
					num9 = (int)((providedInfo.mouse.Y + (float)(height / 2) - 1f) / 16f);
				}
				if (providedInfo.player.gravDir == -1f && num2 == 0)
				{
					num9++;
				}
				if (num9 < 10)
				{
					num9 = 10;
				}
				if (num9 > Main.maxTilesY - 10)
				{
					num9 = Main.maxTilesY - 10;
				}
				int num10 = width / 16 + ((width % 16 != 0) ? 1 : 0);
				int num11 = height / 16 + ((height % 16 != 0) ? 1 : 0);
				if (num != 0)
				{
					for (int k = 0; k < num11; k++)
					{
						if (Main.tile[num8, num9 + k * num3] != null)
						{
							_endpoints.Add(new Tuple<int, int>(num8, num9 + k * num3));
						}
					}
				}
				if (num2 != 0)
				{
					for (int l = 0; l < num10; l++)
					{
						if (Main.tile[(int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num9] != null)
						{
							_endpoints.Add(new Tuple<int, int>((int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num9));
						}
					}
				}
				_targets.Clear();
				while (_points.Count > 0)
				{
					Tuple<int, int> tuple = _points[0];
					Tuple<int, int> tuple2 = _endpoints[0];
					Tuple<int, int> tuple3 = Collision.TupleHitLineWall(tuple.Item1, tuple.Item2, tuple2.Item1, tuple2.Item2);
					if (tuple3.Item1 == -1 || tuple3.Item2 == -1)
					{
						_points.Remove(tuple);
						_endpoints.Remove(tuple2);
						continue;
					}
					if (tuple3.Item1 != tuple2.Item1 || tuple3.Item2 != tuple2.Item2)
					{
						_targets.Add(tuple3);
					}
					_ = Main.tile[tuple3.Item1, tuple3.Item2];
					if (Collision.HitWallSubstep(tuple3.Item1, tuple3.Item2))
					{
						_targets.Add(tuple3);
					}
					_points.Remove(tuple);
					_endpoints.Remove(tuple2);
				}
				if (_targets.Count > 0)
				{
					float num12 = -1f;
					Tuple<int, int> tuple4 = null;
					for (int m = 0; m < _targets.Count; m++)
					{
						if (!Main.tile[_targets[m].Item1, _targets[m].Item2].active() || Main.tile[_targets[m].Item1, _targets[m].Item2].type != 26)
						{
							float num13 = Vector2.Distance(new Vector2(_targets[m].Item1, _targets[m].Item2) * 16f + Vector2.One * 8f, providedInfo.Center);
							if (num12 == -1f || num13 < num12)
							{
								num12 = num13;
								tuple4 = _targets[m];
							}
						}
					}
					if (tuple4 != null && Collision.InTileBounds(tuple4.Item1, tuple4.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
					{
						providedInfo.player.poundRelease = false;
						focusedX = tuple4.Item1;
						focusedY = tuple4.Item2;
					}
				}
				_targets.Clear();
				_points.Clear();
				_endpoints.Clear();
			}
			if (providedInfo.item.hammer <= 0 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int n = providedInfo.reachableStartX; n <= providedInfo.reachableEndX; n++)
			{
				for (int num14 = providedInfo.reachableStartY; num14 <= providedInfo.reachableEndY; num14++)
				{
					if (Main.tile[n, num14].wall > 0 && Collision.HitWallSubstep(n, num14))
					{
						_targets.Add(new Tuple<int, int>(n, num14));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num15 = -1f;
				Tuple<int, int> tuple5 = null;
				for (int num16 = 0; num16 < _targets.Count; num16++)
				{
					if (!Main.tile[_targets[num16].Item1, _targets[num16].Item2].active() || Main.tile[_targets[num16].Item1, _targets[num16].Item2].type != 26)
					{
						float num17 = Vector2.Distance(new Vector2(_targets[num16].Item1, _targets[num16].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num15 == -1f || num17 < num15)
						{
							num15 = num17;
							tuple5 = _targets[num16];
						}
					}
				}
				if (tuple5 != null && Collision.InTileBounds(tuple5.Item1, tuple5.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					providedInfo.player.poundRelease = false;
					focusedX = tuple5.Item1;
					focusedY = tuple5.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_MulticolorWrench(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.type != 3625 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
			WiresUI.Settings.MultiToolMode multiToolMode = (WiresUI.Settings.MultiToolMode)0;
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire())
			{
				multiToolMode |= WiresUI.Settings.MultiToolMode.Red;
			}
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire2())
			{
				multiToolMode |= WiresUI.Settings.MultiToolMode.Blue;
			}
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire3())
			{
				multiToolMode |= WiresUI.Settings.MultiToolMode.Green;
			}
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire4())
			{
				multiToolMode |= WiresUI.Settings.MultiToolMode.Yellow;
			}
			toolMode &= ~WiresUI.Settings.MultiToolMode.Cutter;
			bool num = toolMode == multiToolMode;
			toolMode = WiresUI.Settings.ToolMode;
			if (!num)
			{
				bool flag = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Red);
				bool flag2 = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Blue);
				bool flag3 = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Green);
				bool flag4 = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Yellow);
				bool flag5 = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Cutter);
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (flag5)
						{
							if ((tile.wire() && flag) || (tile.wire2() && flag2) || (tile.wire3() && flag3) || (tile.wire4() && flag4))
							{
								_targets.Add(new Tuple<int, int>(i, j));
							}
						}
						else
						{
							if (!(tile.wire() && flag) && !(tile.wire2() && flag2) && !(tile.wire3() && flag3) && !(tile.wire4() && flag4))
							{
								continue;
							}
							if (flag)
							{
								if (!Main.tile[i - 1, j].wire())
								{
									_targets.Add(new Tuple<int, int>(i - 1, j));
								}
								if (!Main.tile[i + 1, j].wire())
								{
									_targets.Add(new Tuple<int, int>(i + 1, j));
								}
								if (!Main.tile[i, j - 1].wire())
								{
									_targets.Add(new Tuple<int, int>(i, j - 1));
								}
								if (!Main.tile[i, j + 1].wire())
								{
									_targets.Add(new Tuple<int, int>(i, j + 1));
								}
							}
							if (flag2)
							{
								if (!Main.tile[i - 1, j].wire2())
								{
									_targets.Add(new Tuple<int, int>(i - 1, j));
								}
								if (!Main.tile[i + 1, j].wire2())
								{
									_targets.Add(new Tuple<int, int>(i + 1, j));
								}
								if (!Main.tile[i, j - 1].wire2())
								{
									_targets.Add(new Tuple<int, int>(i, j - 1));
								}
								if (!Main.tile[i, j + 1].wire2())
								{
									_targets.Add(new Tuple<int, int>(i, j + 1));
								}
							}
							if (flag3)
							{
								if (!Main.tile[i - 1, j].wire3())
								{
									_targets.Add(new Tuple<int, int>(i - 1, j));
								}
								if (!Main.tile[i + 1, j].wire3())
								{
									_targets.Add(new Tuple<int, int>(i + 1, j));
								}
								if (!Main.tile[i, j - 1].wire3())
								{
									_targets.Add(new Tuple<int, int>(i, j - 1));
								}
								if (!Main.tile[i, j + 1].wire3())
								{
									_targets.Add(new Tuple<int, int>(i, j + 1));
								}
							}
							if (flag4)
							{
								if (!Main.tile[i - 1, j].wire4())
								{
									_targets.Add(new Tuple<int, int>(i - 1, j));
								}
								if (!Main.tile[i + 1, j].wire4())
								{
									_targets.Add(new Tuple<int, int>(i + 1, j));
								}
								if (!Main.tile[i, j - 1].wire4())
								{
									_targets.Add(new Tuple<int, int>(i, j - 1));
								}
								if (!Main.tile[i, j + 1].wire4())
								{
									_targets.Add(new Tuple<int, int>(i, j + 1));
								}
							}
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num2 = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num3 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num2 == -1f || num3 < num2)
					{
						num2 = num3;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_ColoredWrenches(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if ((providedInfo.item.type != 509 && providedInfo.item.type != 850 && providedInfo.item.type != 851 && providedInfo.item.type != 3612) || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			_targets.Clear();
			int num = 0;
			if (providedInfo.item.type == 509)
			{
				num = 1;
			}
			if (providedInfo.item.type == 850)
			{
				num = 2;
			}
			if (providedInfo.item.type == 851)
			{
				num = 3;
			}
			if (providedInfo.item.type == 3612)
			{
				num = 4;
			}
			bool flag = false;
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire() && num == 1)
			{
				flag = true;
			}
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire2() && num == 2)
			{
				flag = true;
			}
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire3() && num == 3)
			{
				flag = true;
			}
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire4() && num == 4)
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if ((!tile.wire() || num != 1) && (!tile.wire2() || num != 2) && (!tile.wire3() || num != 3) && (!tile.wire4() || num != 4))
						{
							continue;
						}
						if (num == 1)
						{
							if (!Main.tile[i - 1, j].wire())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire())
							{
								_targets.Add(new Tuple<int, int>(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire())
							{
								_targets.Add(new Tuple<int, int>(i, j + 1));
							}
						}
						if (num == 2)
						{
							if (!Main.tile[i - 1, j].wire2())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire2())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire2())
							{
								_targets.Add(new Tuple<int, int>(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire2())
							{
								_targets.Add(new Tuple<int, int>(i, j + 1));
							}
						}
						if (num == 3)
						{
							if (!Main.tile[i - 1, j].wire3())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire3())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire3())
							{
								_targets.Add(new Tuple<int, int>(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire3())
							{
								_targets.Add(new Tuple<int, int>(i, j + 1));
							}
						}
						if (num == 4)
						{
							if (!Main.tile[i - 1, j].wire4())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire4())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire4())
							{
								_targets.Add(new Tuple<int, int>(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire4())
							{
								_targets.Add(new Tuple<int, int>(i, j + 1));
							}
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num2 = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num3 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num2 == -1f || num3 < num2)
					{
						num2 = num3;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_Acorns(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (providedInfo.item.type != 27 || focusedX != -1 || focusedY != -1 || providedInfo.reachableStartY <= 20)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					Tile tile2 = Main.tile[i, j - 1];
					Tile tile3 = Main.tile[i, j + 1];
					Tile tile4 = Main.tile[i - 1, j];
					Tile tile5 = Main.tile[i + 1, j];
					Tile tile6 = Main.tile[i - 2, j];
					Tile tile7 = Main.tile[i + 2, j];
					Tile tile8 = Main.tile[i - 3, j];
					Tile tile9 = Main.tile[i + 3, j];
					if ((tile.active() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type]) || (tile2.active() && !Main.tileCut[tile2.type] && !TileID.Sets.BreakableWhenPlacing[tile2.type]) || (tile4.active() && TileID.Sets.CommonSapling[tile4.type]) || (tile5.active() && TileID.Sets.CommonSapling[tile5.type]) || (tile6.active() && TileID.Sets.CommonSapling[tile6.type]) || (tile7.active() && TileID.Sets.CommonSapling[tile7.type]) || (tile8.active() && TileID.Sets.CommonSapling[tile8.type]) || (tile9.active() && TileID.Sets.CommonSapling[tile9.type]) || !tile3.active() || !WorldGen.SolidTile2(tile3))
					{
						continue;
					}
					switch (tile3.type)
					{
					case 60:
						if (WorldGen.EmptyTileCheck(i - 2, i + 2, j - 20, j, 20))
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
						break;
					case 2:
					case 23:
					case 53:
					case 109:
					case 112:
					case 116:
					case 147:
					case 199:
					case 234:
					case 477:
					case 492:
					case 633:
					case 661:
					case 662:
						if (tile4.liquid == 0 && tile.liquid == 0 && tile5.liquid == 0 && WorldGen.EmptyTileCheck(i - 2, i + 2, j - 20, j, 20))
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
						break;
					}
				}
			}
			_toRemove.Clear();
			for (int k = 0; k < _targets.Count; k++)
			{
				bool flag = false;
				for (int l = -1; l < 2; l += 2)
				{
					Tile tile10 = Main.tile[_targets[k].Item1 + l, _targets[k].Item2 + 1];
					if (tile10.active())
					{
						switch (tile10.type)
						{
						case 2:
						case 23:
						case 53:
						case 60:
						case 109:
						case 112:
						case 116:
						case 147:
						case 199:
						case 234:
						case 477:
						case 492:
						case 633:
						case 661:
						case 662:
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					_toRemove.Add(_targets[k]);
				}
			}
			for (int m = 0; m < _toRemove.Count; m++)
			{
				_targets.Remove(_toRemove[m]);
			}
			_toRemove.Clear();
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int n = 0; n < _targets.Count; n++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[n].Item1, _targets[n].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[n];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_GemCorns(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
		{
			if (!WorldGen.GrowTreeSettings.Profiles.TryGetFromItemId(providedInfo.item.type, out var profile) || focusedX != -1 || focusedY != -1 || providedInfo.reachableStartY <= 20)
			{
				return;
			}
			_targets.Clear();
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					Tile tile2 = Main.tile[i, j - 1];
					Tile tile3 = Main.tile[i, j + 1];
					Tile tile4 = Main.tile[i - 1, j];
					Tile tile5 = Main.tile[i + 1, j];
					Tile tile6 = Main.tile[i - 2, j];
					Tile tile7 = Main.tile[i + 2, j];
					Tile tile8 = Main.tile[i - 3, j];
					Tile tile9 = Main.tile[i + 3, j];
					if (profile.GroundTest(tile3.type) && (!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type]) && (!tile2.active() || Main.tileCut[tile2.type] || TileID.Sets.BreakableWhenPlacing[tile2.type]) && (!tile4.active() || !TileID.Sets.CommonSapling[tile4.type]) && (!tile5.active() || !TileID.Sets.CommonSapling[tile5.type]) && (!tile6.active() || !TileID.Sets.CommonSapling[tile6.type]) && (!tile7.active() || !TileID.Sets.CommonSapling[tile7.type]) && (!tile8.active() || !TileID.Sets.CommonSapling[tile8.type]) && (!tile9.active() || !TileID.Sets.CommonSapling[tile9.type]) && tile3.active() && WorldGen.SolidTile2(tile3) && tile4.liquid == 0 && tile.liquid == 0 && tile5.liquid == 0 && WorldGen.EmptyTileCheck(i - 2, i + 2, j - profile.TreeHeightMax, j, profile.SaplingTileType))
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			_toRemove.Clear();
			for (int k = 0; k < _targets.Count; k++)
			{
				bool flag = false;
				for (int l = -1; l < 2; l += 2)
				{
					Tile tile10 = Main.tile[_targets[k].Item1 + l, _targets[k].Item2 + 1];
					if (tile10.active() && profile.GroundTest(tile10.type))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					_toRemove.Add(_targets[k]);
				}
			}
			for (int m = 0; m < _toRemove.Count; m++)
			{
				_targets.Remove(_toRemove[m]);
			}
			_toRemove.Clear();
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int n = 0; n < _targets.Count; n++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[n].Item1, _targets[n].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[n];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_ForceCursorToAnyMinableThing(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			_ = providedInfo.screenTargetX;
			_ = providedInfo.screenTargetY;
			Vector2 mouse = providedInfo.mouse;
			Item item = providedInfo.item;
			if (fX != -1 || fY != -1 || PlayerInput.UsingGamepad)
			{
				return;
			}
			Point point = mouse.ToTileCoordinates();
			int x = point.X;
			int y = point.Y;
			if (Collision.InTileBounds(x, y, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
			{
				Tile tile = Main.tile[x, y];
				bool flag = tile.active() && WorldGen.CanKillTile(x, y) && (!Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
				if (flag && Main.tileAxe[tile.type] && item.axe < 1)
				{
					flag = false;
				}
				if (flag && Main.tileHammer[tile.type] && item.hammer < 1)
				{
					flag = false;
				}
				if (flag && !Main.tileHammer[tile.type] && !Main.tileAxe[tile.type] && item.pick < 1)
				{
					flag = false;
				}
				if (flag)
				{
					fX = x;
					fY = y;
				}
			}
		}

		private static void Step_Pickaxe_MineShinies(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			_ = providedInfo.screenTargetX;
			_ = providedInfo.screenTargetY;
			Item item = providedInfo.item;
			Vector2 mouse = providedInfo.mouse;
			if (item.pick <= 0 || fX != -1 || fY != -1)
			{
				return;
			}
			_targets.Clear();
			if (item.type != 1333 && item.type != 523)
			{
				_ = item.type != 4384;
			}
			else
				_ = 0;
			int num = 0;
			for (int i = reachableStartX; i <= reachableEndX; i++)
			{
				for (int j = reachableStartY; j <= reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					_ = Main.tile[i - 1, j];
					_ = Main.tile[i + 1, j];
					_ = Main.tile[i, j + 1];
					if (!tile.active())
					{
						continue;
					}
					int num2 = (num2 = TileID.Sets.SmartCursorPickaxePriorityOverride[tile.type]);
					if (num2 > 0)
					{
						if (num < num2)
						{
							num = num2;
						}
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			_targets2.Clear();
			foreach (Tuple<int, int> item2 in _targets2)
			{
				Tile tile2 = Main.tile[item2.Item1, item2.Item2];
				if (TileID.Sets.SmartCursorPickaxePriorityOverride[tile2.type] < num)
				{
					_targets2.Add(item2);
				}
			}
			foreach (Tuple<int, int> item3 in _targets2)
			{
				_targets.Remove(item3);
			}
			if (_targets.Count > 0)
			{
				float num3 = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num4 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, mouse);
					if (num3 == -1f || num4 < num3)
					{
						num3 = num4;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
				{
					fX = tuple.Item1;
					fY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_Pickaxe_MineSolids(Player player, SmartCursorUsageInfo providedInfo, List<Tuple<int, int>> grappleTargets, ref int focusedX, ref int focusedY)
		{
			int width = player.width;
			int height = player.height;
			int direction = player.direction;
			Vector2 center = player.Center;
			Vector2 position = player.position;
			float gravDir = player.gravDir;
			int whoAmI = player.whoAmI;
			if (providedInfo.item.pick <= 0 || focusedX != -1 || focusedY != -1)
			{
				return;
			}
			if (PlayerInput.UsingGamepad)
			{
				Vector2 navigatorDirections = PlayerInput.Triggers.Current.GetNavigatorDirections();
				Vector2 gamepadThumbstickLeft = PlayerInput.GamepadThumbstickLeft;
				Vector2 gamepadThumbstickRight = PlayerInput.GamepadThumbstickRight;
				if (navigatorDirections == Vector2.Zero && gamepadThumbstickLeft.Length() < 0.05f && gamepadThumbstickRight.Length() < 0.05f)
				{
					providedInfo.mouse = center + new Vector2(direction * 1000, 0f);
				}
			}
			Vector2 vector = providedInfo.mouse - center;
			int num = Math.Sign(vector.X);
			int num2 = Math.Sign(vector.Y);
			if (Math.Abs(vector.X) > Math.Abs(vector.Y) * 3f)
			{
				num2 = 0;
				providedInfo.mouse.Y = center.Y;
			}
			if (Math.Abs(vector.Y) > Math.Abs(vector.X) * 3f)
			{
				num = 0;
				providedInfo.mouse.X = center.X;
			}
			_ = (int)center.X / 16;
			_ = (int)center.Y / 16;
			_points.Clear();
			_endpoints.Clear();
			int num3 = 1;
			if (num2 == -1 && num != 0)
			{
				num3 = -1;
			}
			int num4 = (int)((position.X + (float)(width / 2) + (float)((width / 2 - 1) * num)) / 16f);
			int num5 = (int)(((double)position.Y + 0.1) / 16.0);
			if (num3 == -1)
			{
				num5 = (int)((position.Y + (float)height - 1f) / 16f);
			}
			int num6 = width / 16 + ((width % 16 != 0) ? 1 : 0);
			int num7 = height / 16 + ((height % 16 != 0) ? 1 : 0);
			if (num != 0)
			{
				for (int i = 0; i < num7; i++)
				{
					if (Main.tile[num4, num5 + i * num3] != null)
					{
						_points.Add(new Tuple<int, int>(num4, num5 + i * num3));
					}
				}
			}
			if (num2 != 0)
			{
				for (int j = 0; j < num6; j++)
				{
					if (Main.tile[(int)(position.X / 16f) + j, num5] != null)
					{
						_points.Add(new Tuple<int, int>((int)(position.X / 16f) + j, num5));
					}
				}
			}
			int num8 = (int)((providedInfo.mouse.X + (float)((width / 2 - 1) * num)) / 16f);
			int num9 = (int)(((double)providedInfo.mouse.Y + 0.1 - (double)(height / 2 + 1)) / 16.0);
			if (num3 == -1)
			{
				num9 = (int)((providedInfo.mouse.Y + (float)(height / 2) - 1f) / 16f);
			}
			if (gravDir == -1f && num2 == 0)
			{
				num9++;
			}
			if (num9 < 10)
			{
				num9 = 10;
			}
			if (num9 > Main.maxTilesY - 10)
			{
				num9 = Main.maxTilesY - 10;
			}
			int num10 = width / 16 + ((width % 16 != 0) ? 1 : 0);
			int num11 = height / 16 + ((height % 16 != 0) ? 1 : 0);
			if (WorldGen.InWorld(num8, num9, 40))
			{
				if (num != 0)
				{
					for (int k = 0; k < num11; k++)
					{
						if (Main.tile[num8, num9 + k * num3] != null)
						{
							_endpoints.Add(new Tuple<int, int>(num8, num9 + k * num3));
						}
					}
				}
				if (num2 != 0)
				{
					for (int l = 0; l < num10; l++)
					{
						if (Main.tile[(int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num9] != null)
						{
							_endpoints.Add(new Tuple<int, int>((int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num9));
						}
					}
				}
			}
			_targets.Clear();
			while (_points.Count > 0 && _endpoints.Count > 0)
			{
				Tuple<int, int> tuple = _points[0];
				Tuple<int, int> tuple2 = _endpoints[0];
				if (!Collision.TupleHitLine(tuple.Item1, tuple.Item2, tuple2.Item1, tuple2.Item2, num * (int)gravDir, -num2 * (int)gravDir, grappleTargets, out var col))
				{
					_points.Remove(tuple);
					_endpoints.Remove(tuple2);
					continue;
				}
				if (col.Item1 != tuple2.Item1 || col.Item2 != tuple2.Item2)
				{
					_targets.Add(col);
				}
				Tile tile = Main.tile[col.Item1, col.Item2];
				if (!tile.inActive() && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type] && !grappleTargets.Contains(col))
				{
					_targets.Add(col);
				}
				_points.Remove(tuple);
				_endpoints.Remove(tuple2);
			}
			_toRemove.Clear();
			for (int m = 0; m < _targets.Count; m++)
			{
				if (!WorldGen.CanKillTile(_targets[m].Item1, _targets[m].Item2))
				{
					_toRemove.Add(_targets[m]);
				}
			}
			for (int n = 0; n < _toRemove.Count; n++)
			{
				_targets.Remove(_toRemove[n]);
			}
			_toRemove.Clear();
			if (_targets.Count > 0)
			{
				float num12 = -1f;
				Tuple<int, int> tuple3 = _targets[0];
				Vector2 value = center;
				if (Main.netMode == 1)
				{
					int num13 = 0;
					int num14 = 0;
					int num15 = 0;
					for (int num16 = 0; num16 < whoAmI; num16++)
					{
						Player player2 = Main.player[num16];
						if (player2.active && !player2.dead && player2.HeldItem.pick > 0 && player2.itemAnimation > 0)
						{
							if (player.Distance(player2.Center) <= 8f)
							{
								num13++;
							}
							if (player.Distance(player2.Center) <= 80f && Math.Abs(player2.Center.Y - center.Y) <= 12f)
							{
								num14++;
							}
						}
					}
					for (int num17 = whoAmI + 1; num17 < 255; num17++)
					{
						Player player3 = Main.player[num17];
						if (player3.active && !player3.dead && player3.HeldItem.pick > 0 && player3.itemAnimation > 0 && player.Distance(player3.Center) <= 8f)
						{
							num15++;
						}
					}
					if (num13 > 0)
					{
						if (num13 % 2 == 1)
						{
							value.X += 12f;
						}
						else
						{
							value.X -= 12f;
						}
						if (num14 % 2 == 1)
						{
							value.Y -= 12f;
						}
					}
					if (num15 > 0 && num13 == 0)
					{
						if (num15 % 2 == 1)
						{
							value.X -= 12f;
						}
						else
						{
							value.X += 12f;
						}
					}
				}
				for (int num18 = 0; num18 < _targets.Count; num18++)
				{
					float num19 = Vector2.Distance(new Vector2(_targets[num18].Item1, _targets[num18].Item2) * 16f + Vector2.One * 8f, value);
					if (num12 == -1f || num19 < num12)
					{
						num12 = num19;
						tuple3 = _targets[num18];
					}
				}
				if (Collision.InTileBounds(tuple3.Item1, tuple3.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					focusedX = tuple3.Item1;
					focusedY = tuple3.Item2;
				}
			}
			_points.Clear();
			_endpoints.Clear();
			_targets.Clear();
		}

		private static void Step_Axe(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			_ = providedInfo.screenTargetX;
			_ = providedInfo.screenTargetY;
			if (providedInfo.item.axe <= 0 || fX != -1 || fY != -1)
			{
				return;
			}
			float num = -1f;
			for (int i = reachableStartX; i <= reachableEndX; i++)
			{
				for (int j = reachableStartY; j <= reachableEndY; j++)
				{
					if (!Main.tile[i, j].active())
					{
						continue;
					}
					Tile tile = Main.tile[i, j];
					if (!Main.tileAxe[tile.type] || TileID.Sets.IgnoreSmartCursorPriorityAxe[tile.type])
					{
						continue;
					}
					int num2 = i;
					int k = j;
					int type = tile.type;
					if (TileID.Sets.IsATreeTrunk[type])
					{
						if (Collision.InTileBounds(num2 + 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
						{
							if (Main.tile[num2, k].frameY >= 198 && Main.tile[num2, k].frameX == 44)
							{
								num2++;
							}
							if (Main.tile[num2, k].frameX == 66 && Main.tile[num2, k].frameY <= 44)
							{
								num2++;
							}
							if (Main.tile[num2, k].frameX == 44 && Main.tile[num2, k].frameY >= 132 && Main.tile[num2, k].frameY <= 176)
							{
								num2++;
							}
						}
						if (Collision.InTileBounds(num2 - 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
						{
							if (Main.tile[num2, k].frameY >= 198 && Main.tile[num2, k].frameX == 66)
							{
								num2--;
							}
							if (Main.tile[num2, k].frameX == 88 && Main.tile[num2, k].frameY >= 66 && Main.tile[num2, k].frameY <= 110)
							{
								num2--;
							}
							if (Main.tile[num2, k].frameX == 22 && Main.tile[num2, k].frameY >= 132 && Main.tile[num2, k].frameY <= 176)
							{
								num2--;
							}
						}
						for (; Main.tile[num2, k].active() && Main.tile[num2, k].type == type && Main.tile[num2, k + 1].type == type && Collision.InTileBounds(num2, k + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY); k++)
						{
						}
					}
					if (tile.type == 80)
					{
						if (Collision.InTileBounds(num2 + 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
						{
							if (Main.tile[num2, k].frameX == 54)
							{
								num2++;
							}
							if (Main.tile[num2, k].frameX == 108 && Main.tile[num2, k].frameY == 36)
							{
								num2++;
							}
						}
						if (Collision.InTileBounds(num2 - 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
						{
							if (Main.tile[num2, k].frameX == 36)
							{
								num2--;
							}
							if (Main.tile[num2, k].frameX == 108 && Main.tile[num2, k].frameY == 18)
							{
								num2--;
							}
						}
						for (; Main.tile[num2, k].active() && Main.tile[num2, k].type == 80 && Main.tile[num2, k + 1].type == 80 && Collision.InTileBounds(num2, k + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY); k++)
						{
						}
					}
					if (tile.type == 323 || tile.type == 72)
					{
						for (; Main.tile[num2, k].active() && ((Main.tile[num2, k].type == 323 && Main.tile[num2, k + 1].type == 323) || (Main.tile[num2, k].type == 72 && Main.tile[num2, k + 1].type == 72)) && Collision.InTileBounds(num2, k + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY); k++)
						{
						}
					}
					float num3 = Vector2.Distance(new Vector2(num2, k) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num3 < num)
					{
						num = num3;
						fX = num2;
						fY = k;
					}
				}
			}
		}

		private static void Step_BlocksFilling(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			if (!Player.SmartCursorSettings.SmartBlocksEnabled)
			{
				return;
			}
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			int screenTargetX = providedInfo.screenTargetX;
			int screenTargetY = providedInfo.screenTargetY;
			if (Player.SmartCursorSettings.SmartBlocksEnabled || providedInfo.item.createTile <= -1 || providedInfo.item.type == 213 || providedInfo.item.type == 5295 || !Main.tileSolid[providedInfo.item.createTile] || Main.tileSolidTop[providedInfo.item.createTile] || Main.tileFrameImportant[providedInfo.item.createTile] || fX != -1 || fY != -1)
			{
				return;
			}
			_targets.Clear();
			bool flag = false;
			if (Main.tile[screenTargetX, screenTargetY].active())
			{
				flag = true;
			}
			if (!Collision.InTileBounds(screenTargetX, screenTargetY, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
			{
				flag = true;
			}
			if (!flag)
			{
				for (int i = reachableStartX; i <= reachableEndX; i++)
				{
					for (int j = reachableStartY; j <= reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type])
						{
							int num = 0;
							if (Main.tile[i - 1, j].active() && Main.tileSolid[Main.tile[i - 1, j].type] && !Main.tileSolidTop[Main.tile[i - 1, j].type])
							{
								num++;
							}
							if (Main.tile[i + 1, j].active() && Main.tileSolid[Main.tile[i + 1, j].type] && !Main.tileSolidTop[Main.tile[i + 1, j].type])
							{
								num++;
							}
							if (Main.tile[i, j - 1].active() && Main.tileSolid[Main.tile[i, j - 1].type] && !Main.tileSolidTop[Main.tile[i, j - 1].type])
							{
								num++;
							}
							if (Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type] && !Main.tileSolidTop[Main.tile[i, j + 1].type])
							{
								num++;
							}
							if (num >= 2)
							{
								_targets.Add(new Tuple<int, int>(i, j));
							}
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num2 = -1f;
				float num3 = float.PositiveInfinity;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					if (Collision.EmptyTile(_targets[k].Item1, _targets[k].Item2, ignoreTiles: true))
					{
						Vector2 vector = new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f - providedInfo.mouse;
						bool flag2 = false;
						float num4 = Math.Abs(vector.X);
						float num5 = vector.Length();
						if (num4 < num3)
						{
							flag2 = true;
						}
						if (num4 == num3 && (num2 == -1f || num5 < num2))
						{
							flag2 = true;
						}
						if (flag2)
						{
							num2 = num5;
							num3 = num4;
							tuple = _targets[k];
						}
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY) && num2 != -1f)
				{
					fX = tuple.Item1;
					fY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_Torch(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			_ = providedInfo.screenTargetX;
			_ = providedInfo.screenTargetY;
			if (providedInfo.item.createTile != 4 || fX != -1 || fY != -1)
			{
				return;
			}
			_targets.Clear();
			bool flag = providedInfo.item.type != 1333 && providedInfo.item.type != 523 && providedInfo.item.type != 4384;
			for (int i = reachableStartX; i <= reachableEndX; i++)
			{
				for (int j = reachableStartY; j <= reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					Tile tile2 = Main.tile[i - 1, j];
					Tile tile3 = Main.tile[i + 1, j];
					Tile tile4 = Main.tile[i, j + 1];
					if (tile.active() && !TileID.Sets.BreakableWhenPlacing[tile.type] && (!Main.tileCut[tile.type] || tile.type == 82 || tile.type == 83))
					{
						continue;
					}
					bool flag2 = false;
					for (int k = i - 8; k <= i + 8; k++)
					{
						for (int l = j - 8; l <= j + 8; l++)
						{
							if (Main.tile[k, l] != null && Main.tile[k, l].type == 4)
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							break;
						}
					}
					if (!flag2 && (!flag || tile.liquid <= 0) && (tile.wall > 0 || (tile2.active() && (tile2.slope() == 0 || (int)tile2.slope() % 2 != 1) && ((Main.tileSolid[tile2.type] && !Main.tileNoAttach[tile2.type] && !Main.tileSolidTop[tile2.type] && !TileID.Sets.NotReallySolid[tile2.type]) || TileID.Sets.IsBeam[tile2.type] || (WorldGen.IsTreeType(tile2.type) && WorldGen.IsTreeType(Main.tile[i - 1, j - 1].type) && WorldGen.IsTreeType(Main.tile[i - 1, j + 1].type)))) || (tile3.active() && (tile3.slope() == 0 || (int)tile3.slope() % 2 != 0) && ((Main.tileSolid[tile3.type] && !Main.tileNoAttach[tile3.type] && !Main.tileSolidTop[tile3.type] && !TileID.Sets.NotReallySolid[tile3.type]) || TileID.Sets.IsBeam[tile3.type] || (WorldGen.IsTreeType(tile3.type) && WorldGen.IsTreeType(Main.tile[i + 1, j - 1].type) && WorldGen.IsTreeType(Main.tile[i + 1, j + 1].type)))) || (tile4.active() && Main.tileSolid[tile4.type] && !Main.tileNoAttach[tile4.type] && (!Main.tileSolidTop[tile4.type] || (TileID.Sets.Platforms[tile4.type] && tile4.slope() == 0)) && !TileID.Sets.NotReallySolid[tile4.type] && !tile4.halfBrick() && tile4.slope() == 0)) && tile.type != 4)
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int m = 0; m < _targets.Count; m++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[m].Item1, _targets[m].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[m];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
				{
					fX = tuple.Item1;
					fY = tuple.Item2;
				}
			}
			_targets.Clear();
		}

		private static void Step_LawnMower(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
		{
			int reachableStartX = providedInfo.reachableStartX;
			int reachableStartY = providedInfo.reachableStartY;
			int reachableEndX = providedInfo.reachableEndX;
			int reachableEndY = providedInfo.reachableEndY;
			_ = providedInfo.screenTargetX;
			_ = providedInfo.screenTargetY;
			if (providedInfo.item.type != 4049 || fX != -1 || fY != -1)
			{
				return;
			}
			_targets.Clear();
			for (int i = reachableStartX; i <= reachableEndX; i++)
			{
				for (int j = reachableStartY; j <= reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && (tile.type == 2 || tile.type == 109))
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					float num2 = Vector2.Distance(new Vector2(_targets[k].Item1, _targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
				{
					fX = tuple.Item1;
					fY = tuple.Item2;
				}
			}
			_targets.Clear();
		}
	}
}
