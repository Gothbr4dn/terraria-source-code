using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Utilities;

namespace Terraria
{
	public class HitTile
	{
		public class HitTileObject
		{
			public int X;

			public int Y;

			public int damage;

			public int type;

			public int timeToLive;

			public int crackStyle;

			public int animationTimeElapsed;

			public Vector2 animationDirection;

			public HitTileObject()
			{
				Clear();
			}

			public void Clear()
			{
				X = 0;
				Y = 0;
				damage = 0;
				type = 0;
				timeToLive = 0;
				if (rand == null)
				{
					rand = new UnifiedRandom((int)DateTime.Now.Ticks);
				}
				for (crackStyle = rand.Next(4); crackStyle == lastCrack; crackStyle = rand.Next(4))
				{
				}
				lastCrack = crackStyle;
			}
		}

		internal const int UNUSED = 0;

		internal const int TILE = 1;

		internal const int WALL = 2;

		internal const int MAX_HITTILES = 500;

		internal const int TIMETOLIVE = 60;

		private static UnifiedRandom rand;

		private static int lastCrack = -1;

		public HitTileObject[] data;

		private int[] order;

		private int bufferLocation;

		public static void ClearAllTilesAtThisLocation(int x, int y)
		{
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					Main.player[i].hitTile.ClearThisTile(x, y);
				}
			}
		}

		public void ClearThisTile(int x, int y)
		{
			for (int i = 0; i <= 500; i++)
			{
				int num = order[i];
				HitTileObject hitTileObject = data[num];
				if (hitTileObject.X == x && hitTileObject.Y == y)
				{
					Clear(i);
					Prune();
				}
			}
		}

		public HitTile()
		{
			rand = new UnifiedRandom();
			data = new HitTileObject[501];
			order = new int[501];
			for (int i = 0; i <= 500; i++)
			{
				data[i] = new HitTileObject();
				order[i] = i;
			}
			bufferLocation = 0;
		}

		public int TryFinding(int x, int y, int hitType)
		{
			for (int i = 0; i <= 500; i++)
			{
				int num = order[i];
				HitTileObject hitTileObject = data[num];
				if (hitTileObject.type == hitType)
				{
					if (hitTileObject.X == x && hitTileObject.Y == y)
					{
						return num;
					}
				}
				else if (i != 0 && hitTileObject.type == 0)
				{
					break;
				}
			}
			return -1;
		}

		public void TryClearingAndPruning(int x, int y, int hitType)
		{
			int num = TryFinding(x, y, hitType);
			if (num != -1)
			{
				Clear(num);
				Prune();
			}
		}

		public int HitObject(int x, int y, int hitType)
		{
			HitTileObject hitTileObject;
			for (int i = 0; i <= 500; i++)
			{
				int num = order[i];
				hitTileObject = data[num];
				if (hitTileObject.type == hitType)
				{
					if (hitTileObject.X == x && hitTileObject.Y == y)
					{
						return num;
					}
				}
				else if (i != 0 && hitTileObject.type == 0)
				{
					break;
				}
			}
			hitTileObject = data[bufferLocation];
			hitTileObject.X = x;
			hitTileObject.Y = y;
			hitTileObject.type = hitType;
			return bufferLocation;
		}

		public void UpdatePosition(int tileId, int x, int y)
		{
			if (tileId >= 0 && tileId <= 500)
			{
				HitTileObject obj = data[tileId];
				obj.X = x;
				obj.Y = y;
			}
		}

		public int AddDamage(int tileId, int damageAmount, bool updateAmount = true)
		{
			if (tileId < 0 || tileId > 500)
			{
				return 0;
			}
			if (tileId == bufferLocation && damageAmount == 0)
			{
				return 0;
			}
			HitTileObject hitTileObject = data[tileId];
			if (!updateAmount)
			{
				return hitTileObject.damage + damageAmount;
			}
			hitTileObject.damage += damageAmount;
			hitTileObject.timeToLive = 60;
			hitTileObject.animationTimeElapsed = 0;
			hitTileObject.animationDirection = (Main.rand.NextFloat() * (MathF.PI * 2f)).ToRotationVector2() * 2f;
			SortSlots(tileId);
			return hitTileObject.damage;
		}

		private void SortSlots(int tileId)
		{
			if (tileId == bufferLocation)
			{
				bufferLocation = order[500];
				if (tileId != bufferLocation)
				{
					data[bufferLocation].Clear();
				}
				for (int num = 500; num > 0; num--)
				{
					order[num] = order[num - 1];
				}
				order[0] = bufferLocation;
			}
			else
			{
				int num;
				for (num = 0; num <= 500 && order[num] != tileId; num++)
				{
				}
				while (num > 1)
				{
					int num2 = order[num - 1];
					order[num - 1] = order[num];
					order[num] = num2;
					num--;
				}
				order[1] = tileId;
			}
		}

		public void Clear(int tileId)
		{
			if (tileId >= 0 && tileId <= 500)
			{
				data[tileId].Clear();
				int i;
				for (i = 0; i < 500 && order[i] != tileId; i++)
				{
				}
				for (; i < 500; i++)
				{
					order[i] = order[i + 1];
				}
				order[500] = tileId;
			}
		}

		public void Prune()
		{
			bool flag = false;
			for (int i = 0; i <= 500; i++)
			{
				HitTileObject hitTileObject = data[i];
				if (hitTileObject.type == 0)
				{
					continue;
				}
				Tile tile = Main.tile[hitTileObject.X, hitTileObject.Y];
				if (hitTileObject.timeToLive <= 1)
				{
					hitTileObject.Clear();
					flag = true;
					continue;
				}
				hitTileObject.timeToLive--;
				if ((double)hitTileObject.timeToLive < 12.0)
				{
					hitTileObject.damage -= 10;
				}
				else if ((double)hitTileObject.timeToLive < 24.0)
				{
					hitTileObject.damage -= 7;
				}
				else if ((double)hitTileObject.timeToLive < 36.0)
				{
					hitTileObject.damage -= 5;
				}
				else if ((double)hitTileObject.timeToLive < 48.0)
				{
					hitTileObject.damage -= 2;
				}
				if (hitTileObject.damage < 0)
				{
					hitTileObject.Clear();
					flag = true;
				}
				else if (hitTileObject.type == 1)
				{
					if (!tile.active())
					{
						hitTileObject.Clear();
						flag = true;
					}
				}
				else if (tile.wall == 0)
				{
					hitTileObject.Clear();
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			int num = 1;
			while (flag)
			{
				flag = false;
				for (int j = num; j < 500; j++)
				{
					if (data[order[j]].type == 0 && data[order[j + 1]].type != 0)
					{
						int num2 = order[j];
						order[j] = order[j + 1];
						order[j + 1] = num2;
						flag = true;
					}
				}
			}
		}

		public void DrawFreshAnimations(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < data.Length; i++)
			{
				data[i].animationTimeElapsed++;
			}
			if (!Main.SettingsEnabled_MinersWobble)
			{
				return;
			}
			int num = 1;
			Vector2 vector = new Vector2(Main.offScreenRange);
			if (Main.drawToScreen)
			{
				vector = Vector2.Zero;
			}
			vector = Vector2.Zero;
			bool flag = Main.ShouldShowInvisibleWalls();
			for (int j = 0; j < data.Length; j++)
			{
				if (data[j].type != num)
				{
					continue;
				}
				int damage = data[j].damage;
				if (damage < 20)
				{
					continue;
				}
				int x = data[j].X;
				int y = data[j].Y;
				if (!WorldGen.InWorld(x, y))
				{
					continue;
				}
				Tile tile = Main.tile[x, y];
				bool flag2 = tile != null;
				if (flag2 && num == 1)
				{
					flag2 = flag2 && tile.active() && Main.tileSolid[Main.tile[x, y].type] && (!tile.invisibleBlock() || flag);
				}
				if (flag2 && num == 2)
				{
					flag2 = flag2 && tile.wall != 0 && (!tile.invisibleWall() || flag);
				}
				if (!flag2)
				{
					continue;
				}
				bool flag3 = false;
				bool flag4 = false;
				if (tile.type == 10)
				{
					flag3 = false;
				}
				else if (Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
				{
					flag3 = true;
				}
				else if (WorldGen.IsTreeType(tile.type))
				{
					flag4 = true;
					int num2 = tile.frameX / 22;
					int num3 = tile.frameY / 22;
					if (num3 < 9)
					{
						flag3 = ((num2 != 1 && num2 != 2) || num3 < 6 || num3 > 8) && (num2 != 3 || num3 > 2) && (num2 != 4 || num3 < 3 || num3 > 5) && ((num2 != 5 || num3 < 6 || num3 > 8) ? true : false);
					}
				}
				else if (tile.type == 72)
				{
					flag4 = true;
					if (tile.frameX <= 34)
					{
						flag3 = true;
					}
				}
				if (!flag3 || tile.slope() != 0 || tile.halfBrick())
				{
					continue;
				}
				int num4 = 0;
				if (damage >= 80)
				{
					num4 = 3;
				}
				else if (damage >= 60)
				{
					num4 = 2;
				}
				else if (damage >= 40)
				{
					num4 = 1;
				}
				else if (damage >= 20)
				{
					num4 = 0;
				}
				Rectangle value = new Rectangle(data[j].crackStyle * 18, num4 * 18, 16, 16);
				value.Inflate(-2, -2);
				if (flag4)
				{
					value.X = (4 + data[j].crackStyle / 2) * 18;
				}
				int animationTimeElapsed = data[j].animationTimeElapsed;
				if (!((float)animationTimeElapsed >= 10f))
				{
					float num5 = (float)animationTimeElapsed / 10f;
					Color color = Lighting.GetColor(x, y);
					float rotation = 0f;
					Vector2 zero = Vector2.Zero;
					float num6 = 0.5f;
					float num7 = num5 % num6;
					num7 *= 1f / num6;
					if ((int)(num5 / num6) % 2 == 1)
					{
						num7 = 1f - num7;
					}
					Tile tileSafely = Framing.GetTileSafely(x, y);
					Tile tile2 = tileSafely;
					Texture2D texture2D = Main.instance.TilePaintSystem.TryGetTileAndRequestIfNotReady(tileSafely.type, 0, tileSafely.color());
					if (texture2D != null)
					{
						Vector2 vector2 = new Vector2(8f);
						Vector2 vector3 = new Vector2(1f);
						float num8 = num7 * 0.2f + 1f;
						float num9 = 1f - num7;
						num9 = 1f;
						color *= num9 * num9 * 0.8f;
						Vector2 scale = num8 * vector3;
						Vector2 position = (new Vector2(x * 16 - (int)Main.screenPosition.X, y * 16 - (int)Main.screenPosition.Y) + vector + vector2 + zero).Floor();
						spriteBatch.Draw(texture2D, position, new Rectangle(tile2.frameX, tile2.frameY, 16, 16), color, rotation, vector2, scale, SpriteEffects.None, 0f);
						color.A = 180;
						spriteBatch.Draw(TextureAssets.TileCrack.get_Value(), position, value, color, rotation, vector2, scale, SpriteEffects.None, 0f);
					}
				}
			}
		}
	}
}
