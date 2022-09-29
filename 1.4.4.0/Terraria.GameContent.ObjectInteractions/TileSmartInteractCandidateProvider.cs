using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.ObjectInteractions
{
	public class TileSmartInteractCandidateProvider : ISmartInteractCandidateProvider
	{
		private class ReusableCandidate : ISmartInteractCandidate
		{
			private bool _strictSettings;

			private int _aimedX;

			private int _aimedY;

			private int _hx;

			private int _hy;

			private int _lx;

			private int _ly;

			public float DistanceFromCursor { get; private set; }

			public void Reuse(bool strictSettings, float distanceFromCursor, int AimedX, int AimedY, int LX, int LY, int HX, int HY)
			{
				DistanceFromCursor = distanceFromCursor;
				_strictSettings = strictSettings;
				_aimedX = AimedX;
				_aimedY = AimedY;
				_lx = LX;
				_ly = LY;
				_hx = HX;
				_hy = HY;
			}

			public void WinCandidacy()
			{
				Main.SmartInteractX = _aimedX;
				Main.SmartInteractY = _aimedY;
				if (_strictSettings)
				{
					Main.SmartInteractShowingFake = Main.SmartInteractTileCoords.Count > 0;
				}
				else
				{
					Main.SmartInteractShowingGenuine = true;
				}
				Main.TileInteractionLX = _lx - 10;
				Main.TileInteractionLY = _ly - 10;
				Main.TileInteractionHX = _hx + 10;
				Main.TileInteractionHY = _hy + 10;
			}
		}

		private List<Tuple<int, int>> targets = new List<Tuple<int, int>>();

		private ReusableCandidate _candidate = new ReusableCandidate();

		public void ClearSelfAndPrepareForCheck()
		{
			Main.SmartInteractX = -1;
			Main.SmartInteractY = -1;
			Main.TileInteractionLX = -1;
			Main.TileInteractionHX = -1;
			Main.TileInteractionLY = -1;
			Main.TileInteractionHY = -1;
			Main.SmartInteractTileCoords.Clear();
			Main.SmartInteractTileCoordsSelected.Clear();
			targets.Clear();
		}

		public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
		{
			candidate = null;
			Point point = settings.mousevec.ToTileCoordinates();
			FillPotentialTargetTiles(settings);
			int num = -1;
			int num2 = -1;
			int num3 = -1;
			int num4 = -1;
			if (targets.Count > 0)
			{
				float num5 = -1f;
				Tuple<int, int> tuple = targets[0];
				for (int i = 0; i < targets.Count; i++)
				{
					float num6 = Vector2.Distance(new Vector2(targets[i].Item1, targets[i].Item2) * 16f + Vector2.One * 8f, settings.mousevec);
					if (num5 == -1f || num6 <= num5)
					{
						num5 = num6;
						tuple = targets[i];
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, settings.LX, settings.LY, settings.HX, settings.HY))
				{
					num = tuple.Item1;
					num2 = tuple.Item2;
				}
			}
			bool flag = false;
			for (int j = 0; j < targets.Count; j++)
			{
				int item = targets[j].Item1;
				int item2 = targets[j].Item2;
				Tile tile = Main.tile[item, item2];
				int num7 = 0;
				int num8 = 0;
				int num9 = 18;
				int num10 = 18;
				int num11 = 2;
				switch (tile.type)
				{
				case 136:
				case 144:
				case 494:
					num7 = 1;
					num8 = 1;
					num11 = 0;
					break;
				case 216:
				case 338:
					num7 = 1;
					num8 = 2;
					break;
				case 15:
				case 497:
					num7 = 1;
					num8 = 2;
					num11 = 4;
					break;
				case 10:
					num7 = 1;
					num8 = 3;
					num11 = 0;
					break;
				case 388:
				case 389:
					num7 = 1;
					num8 = 5;
					break;
				case 29:
				case 387:
					num7 = 2;
					num8 = 1;
					break;
				case 21:
				case 55:
				case 85:
				case 97:
				case 125:
				case 132:
				case 287:
				case 335:
				case 386:
				case 411:
				case 425:
				case 441:
				case 467:
				case 468:
				case 573:
				case 621:
					num7 = 2;
					num8 = 2;
					break;
				case 79:
				case 139:
				case 510:
				case 511:
					num7 = 2;
					num8 = 2;
					num11 = 0;
					break;
				case 11:
				case 356:
				case 410:
				case 470:
				case 480:
				case 509:
				case 657:
				case 658:
				case 663:
					num7 = 2;
					num8 = 3;
					num11 = 0;
					break;
				case 207:
					num7 = 2;
					num8 = 4;
					num11 = 0;
					break;
				case 104:
					num7 = 2;
					num8 = 5;
					break;
				case 88:
					num7 = 3;
					num8 = 1;
					num11 = 0;
					break;
				case 89:
				case 215:
				case 237:
				case 377:
					num7 = 3;
					num8 = 2;
					break;
				case 354:
				case 455:
				case 491:
					num7 = 3;
					num8 = 3;
					num11 = 0;
					break;
				case 487:
					num7 = 4;
					num8 = 2;
					num11 = 0;
					break;
				case 212:
					num7 = 4;
					num8 = 3;
					break;
				case 209:
					num7 = 4;
					num8 = 3;
					num11 = 0;
					break;
				case 102:
				case 463:
				case 475:
				case 597:
					num7 = 3;
					num8 = 4;
					break;
				case 464:
					num7 = 5;
					num8 = 4;
					break;
				}
				if (num7 == 0 || num8 == 0)
				{
					continue;
				}
				int num12 = item - tile.frameX % (num9 * num7) / num9;
				int num13 = item2 - tile.frameY % (num10 * num8 + num11) / num10;
				bool flag2 = Collision.InTileBounds(num, num2, num12, num13, num12 + num7 - 1, num13 + num8 - 1);
				bool flag3 = Collision.InTileBounds(point.X, point.Y, num12, num13, num12 + num7 - 1, num13 + num8 - 1);
				if (flag3)
				{
					num3 = point.X;
					num4 = point.Y;
				}
				if (!settings.FullInteraction)
				{
					flag2 = flag2 && flag3;
				}
				if (flag)
				{
					flag2 = false;
				}
				for (int k = num12; k < num12 + num7; k++)
				{
					for (int l = num13; l < num13 + num8; l++)
					{
						Point item3 = new Point(k, l);
						if (!Main.SmartInteractTileCoords.Contains(item3))
						{
							if (flag2)
							{
								Main.SmartInteractTileCoordsSelected.Add(item3);
							}
							if (flag2 || settings.FullInteraction)
							{
								Main.SmartInteractTileCoords.Add(item3);
							}
						}
					}
				}
				if (!flag && flag2)
				{
					flag = true;
				}
			}
			if (settings.DemandOnlyZeroDistanceTargets)
			{
				if (num3 != -1 && num4 != -1)
				{
					_candidate.Reuse(strictSettings: true, 0f, num3, num4, settings.LX - 10, settings.LY - 10, settings.HX + 10, settings.HY + 10);
					candidate = _candidate;
					return true;
				}
				return false;
			}
			if (num != -1 && num2 != -1)
			{
				float distanceFromCursor = new Rectangle(num * 16, num2 * 16, 16, 16).ClosestPointInRect(settings.mousevec).Distance(settings.mousevec);
				_candidate.Reuse(strictSettings: false, distanceFromCursor, num, num2, settings.LX - 10, settings.LY - 10, settings.HX + 10, settings.HY + 10);
				candidate = _candidate;
				return true;
			}
			return false;
		}

		private void FillPotentialTargetTiles(SmartInteractScanSettings settings)
		{
			for (int i = settings.LX; i <= settings.HX; i++)
			{
				for (int j = settings.LY; j <= settings.HY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile == null || !tile.active())
					{
						continue;
					}
					switch (tile.type)
					{
					case 10:
					case 11:
					case 21:
					case 29:
					case 55:
					case 79:
					case 85:
					case 88:
					case 89:
					case 97:
					case 102:
					case 104:
					case 125:
					case 132:
					case 136:
					case 139:
					case 144:
					case 207:
					case 209:
					case 215:
					case 216:
					case 287:
					case 335:
					case 338:
					case 354:
					case 377:
					case 386:
					case 387:
					case 388:
					case 389:
					case 410:
					case 411:
					case 425:
					case 441:
					case 455:
					case 463:
					case 464:
					case 467:
					case 468:
					case 470:
					case 475:
					case 480:
					case 487:
					case 491:
					case 494:
					case 509:
					case 510:
					case 511:
					case 573:
					case 597:
					case 621:
					case 657:
					case 658:
						targets.Add(new Tuple<int, int>(i, j));
						break;
					case 15:
					case 497:
						if (settings.player.IsWithinSnappngRangeToTile(i, j, 40))
						{
							targets.Add(new Tuple<int, int>(i, j));
						}
						break;
					case 237:
						if (settings.player.HasItem(1293))
						{
							targets.Add(new Tuple<int, int>(i, j));
						}
						break;
					case 212:
						if (settings.player.HasItem(949))
						{
							targets.Add(new Tuple<int, int>(i, j));
						}
						break;
					case 356:
						if (!Main.fastForwardTimeToDawn && (Main.netMode == 1 || Main.sundialCooldown == 0))
						{
							targets.Add(new Tuple<int, int>(i, j));
						}
						break;
					case 663:
						if (!Main.fastForwardTimeToDusk && (Main.netMode == 1 || Main.moondialCooldown == 0))
						{
							targets.Add(new Tuple<int, int>(i, j));
						}
						break;
					}
				}
			}
		}
	}
}
