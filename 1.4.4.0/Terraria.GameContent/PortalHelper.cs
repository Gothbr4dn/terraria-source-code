using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent
{
	public class PortalHelper
	{
		public const int PORTALS_PER_PERSON = 2;

		private static int[,] FoundPortals;

		private static int[] PortalCooldownForPlayers;

		private static int[] PortalCooldownForNPCs;

		private static readonly Vector2[] EDGES;

		private static readonly Vector2[] SLOPE_EDGES;

		private static readonly Point[] SLOPE_OFFSETS;

		private static bool anyPortalAtAll;

		static PortalHelper()
		{
			FoundPortals = new int[256, 2];
			PortalCooldownForPlayers = new int[256];
			PortalCooldownForNPCs = new int[200];
			EDGES = new Vector2[4]
			{
				new Vector2(0f, 1f),
				new Vector2(0f, -1f),
				new Vector2(1f, 0f),
				new Vector2(-1f, 0f)
			};
			SLOPE_EDGES = new Vector2[4]
			{
				new Vector2(1f, -1f),
				new Vector2(-1f, -1f),
				new Vector2(1f, 1f),
				new Vector2(-1f, 1f)
			};
			SLOPE_OFFSETS = new Point[4]
			{
				new Point(1, -1),
				new Point(-1, -1),
				new Point(1, 1),
				new Point(-1, 1)
			};
			anyPortalAtAll = false;
			for (int i = 0; i < SLOPE_EDGES.Length; i++)
			{
				SLOPE_EDGES[i].Normalize();
			}
			for (int j = 0; j < FoundPortals.GetLength(0); j++)
			{
				FoundPortals[j, 0] = -1;
				FoundPortals[j, 1] = -1;
			}
		}

		public static void UpdatePortalPoints()
		{
			anyPortalAtAll = false;
			for (int i = 0; i < FoundPortals.GetLength(0); i++)
			{
				FoundPortals[i, 0] = -1;
				FoundPortals[i, 1] = -1;
			}
			for (int j = 0; j < PortalCooldownForPlayers.Length; j++)
			{
				if (PortalCooldownForPlayers[j] > 0)
				{
					PortalCooldownForPlayers[j]--;
				}
			}
			for (int k = 0; k < PortalCooldownForNPCs.Length; k++)
			{
				if (PortalCooldownForNPCs[k] > 0)
				{
					PortalCooldownForNPCs[k]--;
				}
			}
			for (int l = 0; l < 1000; l++)
			{
				Projectile projectile = Main.projectile[l];
				if (projectile.active && projectile.type == 602 && projectile.ai[1] >= 0f && projectile.ai[1] <= 1f && projectile.owner >= 0 && projectile.owner <= 255)
				{
					FoundPortals[projectile.owner, (int)projectile.ai[1]] = l;
					if (FoundPortals[projectile.owner, 0] != -1 && FoundPortals[projectile.owner, 1] != -1)
					{
						anyPortalAtAll = true;
					}
				}
			}
		}

		public static void TryGoingThroughPortals(Entity ent)
		{
			if (!anyPortalAtAll)
			{
				return;
			}
			float collisionPoint = 0f;
			_ = ent.velocity;
			int width = ent.width;
			int height = ent.height;
			int num = 1;
			if (ent is Player)
			{
				num = (int)((Player)ent).gravDir;
			}
			for (int i = 0; i < FoundPortals.GetLength(0); i++)
			{
				if (FoundPortals[i, 0] == -1 || FoundPortals[i, 1] == -1 || (ent is Player && (i >= PortalCooldownForPlayers.Length || PortalCooldownForPlayers[i] > 0)) || (ent is NPC && (i >= PortalCooldownForNPCs.Length || PortalCooldownForNPCs[i] > 0)))
				{
					continue;
				}
				for (int j = 0; j < 2; j++)
				{
					Projectile projectile = Main.projectile[FoundPortals[i, j]];
					GetPortalEdges(projectile.Center, projectile.ai[0], out var start, out var end);
					if (!Collision.CheckAABBvLineCollision(ent.position + ent.velocity, ent.Size, start, end, 2f, ref collisionPoint))
					{
						continue;
					}
					Projectile projectile2 = Main.projectile[FoundPortals[i, 1 - j]];
					float num2 = ent.Hitbox.Distance(projectile.Center);
					int bonusX;
					int bonusY;
					Vector2 vector = GetPortalOutingPoint(ent.Size, projectile2.Center, projectile2.ai[0], out bonusX, out bonusY) + Vector2.Normalize(new Vector2(bonusX, bonusY)) * num2;
					Vector2 vector2 = Vector2.UnitX * 16f;
					if (Collision.TileCollision(vector - vector2, vector2, width, height, fallThrough: true, fall2: true, num) != vector2)
					{
						continue;
					}
					vector2 = -Vector2.UnitX * 16f;
					if (Collision.TileCollision(vector - vector2, vector2, width, height, fallThrough: true, fall2: true, num) != vector2)
					{
						continue;
					}
					vector2 = Vector2.UnitY * 16f;
					if (Collision.TileCollision(vector - vector2, vector2, width, height, fallThrough: true, fall2: true, num) != vector2)
					{
						continue;
					}
					vector2 = -Vector2.UnitY * 16f;
					if (Collision.TileCollision(vector - vector2, vector2, width, height, fallThrough: true, fall2: true, num) != vector2)
					{
						continue;
					}
					float num3 = 0.1f;
					if (bonusY == -num)
					{
						num3 = 0.1f;
					}
					if (ent.velocity == Vector2.Zero)
					{
						ent.velocity = (projectile.ai[0] - MathF.PI / 2f).ToRotationVector2() * num3;
					}
					if (ent.velocity.Length() < num3)
					{
						ent.velocity.Normalize();
						ent.velocity *= num3;
					}
					Vector2 vector3 = Vector2.Normalize(new Vector2(bonusX, bonusY));
					if (vector3.HasNaNs() || vector3 == Vector2.Zero)
					{
						vector3 = Vector2.UnitX * ent.direction;
					}
					ent.velocity = vector3 * ent.velocity.Length();
					if ((bonusY == -num && Math.Sign(ent.velocity.Y) != -num) || Math.Abs(ent.velocity.Y) < 0.1f)
					{
						ent.velocity.Y = (float)(-num) * 0.1f;
					}
					int num4 = (int)((float)(projectile2.owner * 2) + projectile2.ai[1]);
					int lastPortalColorIndex = num4 + ((num4 % 2 == 0) ? 1 : (-1));
					if (ent is Player)
					{
						Player player = (Player)ent;
						player.lastPortalColorIndex = lastPortalColorIndex;
						player.Teleport(vector, 4, num4);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(96, -1, -1, null, player.whoAmI, vector.X, vector.Y, num4);
							NetMessage.SendData(13, -1, -1, null, player.whoAmI);
						}
						PortalCooldownForPlayers[i] = 10;
					}
					else if (ent is NPC)
					{
						NPC nPC = (NPC)ent;
						nPC.lastPortalColorIndex = lastPortalColorIndex;
						nPC.Teleport(vector, 4, num4);
						if (Main.netMode == 2)
						{
							NetMessage.SendData(100, -1, -1, null, nPC.whoAmI, vector.X, vector.Y, num4);
							NetMessage.SendData(23, -1, -1, null, nPC.whoAmI);
						}
						PortalCooldownForPlayers[i] = 10;
						if (bonusY == -1 && ent.velocity.Y > -3f)
						{
							ent.velocity.Y = -3f;
						}
					}
					return;
				}
			}
		}

		public static int TryPlacingPortal(Projectile theBolt, Vector2 velocity, Vector2 theCrashVelocity)
		{
			Vector2 vector = velocity / velocity.Length();
			Point position = FindCollision(theBolt.position, theBolt.position + velocity + vector * 32f).ToTileCoordinates();
			Tile tile = Main.tile[position.X, position.Y];
			Vector2 vector2 = new Vector2(position.X * 16 + 8, position.Y * 16 + 8);
			if (!WorldGen.SolidOrSlopedTile(tile))
			{
				return -1;
			}
			int num = tile.slope();
			bool flag = tile.halfBrick();
			for (int i = 0; i < (flag ? 2 : EDGES.Length); i++)
			{
				if (Vector2.Dot(EDGES[i], vector) > 0f && FindValidLine(position, (int)EDGES[i].Y, (int)(0f - EDGES[i].X), out var bestPosition))
				{
					vector2 = new Vector2(bestPosition.X * 16 + 8, bestPosition.Y * 16 + 8);
					return AddPortal(theBolt, vector2 - EDGES[i] * (flag ? 0f : 8f), (float)Math.Atan2(EDGES[i].Y, EDGES[i].X) + MathF.PI / 2f, (int)theBolt.ai[0], theBolt.direction);
				}
			}
			if (num != 0)
			{
				Vector2 value = SLOPE_EDGES[num - 1];
				if (Vector2.Dot(value, -vector) > 0f && FindValidLine(position, -SLOPE_OFFSETS[num - 1].Y, SLOPE_OFFSETS[num - 1].X, out var bestPosition2))
				{
					vector2 = new Vector2(bestPosition2.X * 16 + 8, bestPosition2.Y * 16 + 8);
					return AddPortal(theBolt, vector2, (float)Math.Atan2(value.Y, value.X) - MathF.PI / 2f, (int)theBolt.ai[0], theBolt.direction);
				}
			}
			return -1;
		}

		private static bool FindValidLine(Point position, int xOffset, int yOffset, out Point bestPosition)
		{
			bestPosition = position;
			if (IsValidLine(position, xOffset, yOffset))
			{
				return true;
			}
			Point point = new Point(position.X - xOffset, position.Y - yOffset);
			if (IsValidLine(point, xOffset, yOffset))
			{
				bestPosition = point;
				return true;
			}
			Point point2 = new Point(position.X + xOffset, position.Y + yOffset);
			if (IsValidLine(point2, xOffset, yOffset))
			{
				bestPosition = point2;
				return true;
			}
			return false;
		}

		private static bool IsValidLine(Point position, int xOffset, int yOffset)
		{
			Tile tile = Main.tile[position.X, position.Y];
			Tile tile2 = Main.tile[position.X - xOffset, position.Y - yOffset];
			Tile tile3 = Main.tile[position.X + xOffset, position.Y + yOffset];
			if (BlockPortals(Main.tile[position.X + yOffset, position.Y - xOffset]) || BlockPortals(Main.tile[position.X + yOffset - xOffset, position.Y - xOffset - yOffset]) || BlockPortals(Main.tile[position.X + yOffset + xOffset, position.Y - xOffset + yOffset]))
			{
				return false;
			}
			if (CanPlacePortalOn(tile) && CanPlacePortalOn(tile2) && CanPlacePortalOn(tile3) && tile2.HasSameSlope(tile) && tile3.HasSameSlope(tile))
			{
				return true;
			}
			return false;
		}

		private static bool CanPlacePortalOn(Tile t)
		{
			if (!DoesTileTypeSupportPortals(t.type))
			{
				return false;
			}
			return WorldGen.SolidOrSlopedTile(t);
		}

		private static bool DoesTileTypeSupportPortals(ushort tileType)
		{
			if (tileType == 496)
			{
				return false;
			}
			return true;
		}

		private static bool BlockPortals(Tile t)
		{
			if (t.active() && !Main.tileCut[t.type] && !TileID.Sets.BreakableWhenPlacing[t.type] && Main.tileSolid[t.type])
			{
				return true;
			}
			return false;
		}

		private static Vector2 FindCollision(Vector2 startPosition, Vector2 stopPosition)
		{
			int lastX = 0;
			int lastY = 0;
			Utils.PlotLine(startPosition.ToTileCoordinates(), stopPosition.ToTileCoordinates(), delegate(int x, int y)
			{
				lastX = x;
				lastY = y;
				return !WorldGen.SolidOrSlopedTile(x, y);
			}, jump: false);
			return new Vector2((float)lastX * 16f, (float)lastY * 16f);
		}

		private static int AddPortal(Projectile sourceProjectile, Vector2 position, float angle, int form, int direction)
		{
			if (!SupportedTilesAreFine(position, angle))
			{
				return -1;
			}
			RemoveMyOldPortal(form);
			RemoveIntersectingPortals(position, angle);
			int num = Projectile.NewProjectile(Projectile.InheritSource(sourceProjectile), position.X, position.Y, 0f, 0f, 602, 0, 0f, Main.myPlayer, angle, form);
			Main.projectile[num].direction = direction;
			Main.projectile[num].netUpdate = true;
			return num;
		}

		private static void RemoveMyOldPortal(int form)
		{
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (projectile.active && projectile.type == 602 && projectile.owner == Main.myPlayer && projectile.ai[1] == (float)form)
				{
					projectile.Kill();
					break;
				}
			}
		}

		private static void RemoveIntersectingPortals(Vector2 position, float angle)
		{
			GetPortalEdges(position, angle, out var start, out var end);
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (!projectile.active || projectile.type != 602)
				{
					continue;
				}
				GetPortalEdges(projectile.Center, projectile.ai[0], out var start2, out var end2);
				if (Collision.CheckLinevLine(start, end, start2, end2).Length != 0)
				{
					if (projectile.owner != Main.myPlayer && Main.netMode != 2)
					{
						NetMessage.SendData(95, -1, -1, null, projectile.owner, (int)projectile.ai[1]);
					}
					projectile.Kill();
				}
			}
		}

		public static Color GetPortalColor(int colorIndex)
		{
			return GetPortalColor(colorIndex / 2, colorIndex % 2);
		}

		public static Color GetPortalColor(int player, int portal)
		{
			Color white = Color.White;
			if (Main.netMode == 0)
			{
				white = ((portal != 0) ? Main.hslToRgb(0.52f, 1f, 0.6f) : Main.hslToRgb(0.12f, 1f, 0.5f));
			}
			else
			{
				float num = 0.08f;
				white = Main.hslToRgb((0.5f + (float)player * (num * 2f) + (float)portal * num) % 1f, 1f, 0.5f);
			}
			white.A = 66;
			return white;
		}

		private static void GetPortalEdges(Vector2 position, float angle, out Vector2 start, out Vector2 end)
		{
			Vector2 vector = angle.ToRotationVector2();
			start = position + vector * -22f;
			end = position + vector * 22f;
		}

		private static Vector2 GetPortalOutingPoint(Vector2 objectSize, Vector2 portalPosition, float portalAngle, out int bonusX, out int bonusY)
		{
			int num = (int)Math.Round(MathHelper.WrapAngle(portalAngle) / (MathF.PI / 4f));
			switch (num)
			{
			case -2:
			case 2:
				bonusX = ((num != 2) ? 1 : (-1));
				bonusY = 0;
				return portalPosition + new Vector2((num == 2) ? (0f - objectSize.X) : 0f, (0f - objectSize.Y) / 2f);
			case 0:
			case 4:
				bonusX = 0;
				bonusY = ((num == 0) ? 1 : (-1));
				return portalPosition + new Vector2((0f - objectSize.X) / 2f, (num == 0) ? 0f : (0f - objectSize.Y));
			case -3:
			case 3:
				bonusX = ((num == -3) ? 1 : (-1));
				bonusY = -1;
				return portalPosition + new Vector2((num == -3) ? 0f : (0f - objectSize.X), 0f - objectSize.Y);
			case -1:
			case 1:
				bonusX = ((num == -1) ? 1 : (-1));
				bonusY = 1;
				return portalPosition + new Vector2((num == -1) ? 0f : (0f - objectSize.X), 0f);
			default:
				bonusX = 0;
				bonusY = 0;
				return portalPosition;
			}
		}

		public static void SyncPortalsOnPlayerJoin(int plr, int fluff, List<Point> dontInclude, out List<Point> portalSections)
		{
			portalSections = new List<Point>();
			for (int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if (!projectile.active || (projectile.type != 602 && projectile.type != 601))
				{
					continue;
				}
				Vector2 center = projectile.Center;
				int sectionX = Netplay.GetSectionX((int)(center.X / 16f));
				int sectionY = Netplay.GetSectionY((int)(center.Y / 16f));
				for (int j = sectionX - fluff; j < sectionX + fluff + 1; j++)
				{
					for (int k = sectionY - fluff; k < sectionY + fluff + 1; k++)
					{
						if (j >= 0 && j < Main.maxSectionsX && k >= 0 && k < Main.maxSectionsY && !Netplay.Clients[plr].TileSections[j, k] && !dontInclude.Contains(new Point(j, k)))
						{
							portalSections.Add(new Point(j, k));
						}
					}
				}
			}
		}

		public static void SyncPortalSections(Vector2 portalPosition, int fluff)
		{
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active)
				{
					RemoteClient.CheckSection(i, portalPosition, fluff);
				}
			}
		}

		public static bool SupportedTilesAreFine(Vector2 portalCenter, float portalAngle)
		{
			Point point = portalCenter.ToTileCoordinates();
			int num = (int)Math.Round(MathHelper.WrapAngle(portalAngle) / (MathF.PI / 4f));
			int num2;
			int num3;
			switch (num)
			{
			case -2:
			case 2:
				num2 = ((num != 2) ? 1 : (-1));
				num3 = 0;
				break;
			case 0:
			case 4:
				num2 = 0;
				num3 = ((num == 0) ? 1 : (-1));
				break;
			case -3:
			case 3:
				num2 = ((num == -3) ? 1 : (-1));
				num3 = -1;
				break;
			case -1:
			case 1:
				num2 = ((num == -1) ? 1 : (-1));
				num3 = 1;
				break;
			default:
				Main.NewText("Broken portal! (over4s = " + num + " , " + portalAngle + ")");
				return false;
			}
			if (num2 != 0 && num3 != 0)
			{
				int num4 = 3;
				if (num2 == -1 && num3 == 1)
				{
					num4 = 5;
				}
				if (num2 == 1 && num3 == -1)
				{
					num4 = 2;
				}
				if (num2 == 1 && num3 == 1)
				{
					num4 = 4;
				}
				num4--;
				if (SupportedSlope(point.X, point.Y, num4) && SupportedSlope(point.X + num2, point.Y - num3, num4))
				{
					return SupportedSlope(point.X - num2, point.Y + num3, num4);
				}
				return false;
			}
			if (num2 != 0)
			{
				if (num2 == 1)
				{
					point.X--;
				}
				if (SupportedNormal(point.X, point.Y) && SupportedNormal(point.X, point.Y - 1))
				{
					return SupportedNormal(point.X, point.Y + 1);
				}
				return false;
			}
			if (num3 != 0)
			{
				if (num3 == 1)
				{
					point.Y--;
				}
				if (!SupportedNormal(point.X, point.Y) || !SupportedNormal(point.X + 1, point.Y) || !SupportedNormal(point.X - 1, point.Y))
				{
					if (SupportedHalfbrick(point.X, point.Y) && SupportedHalfbrick(point.X + 1, point.Y))
					{
						return SupportedHalfbrick(point.X - 1, point.Y);
					}
					return false;
				}
				return true;
			}
			return true;
		}

		private static bool SupportedSlope(int x, int y, int slope)
		{
			Tile tile = Main.tile[x, y];
			if (tile != null && tile.nactive() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type] && Main.tileSolid[tile.type] && tile.slope() == slope)
			{
				return DoesTileTypeSupportPortals(tile.type);
			}
			return false;
		}

		private static bool SupportedHalfbrick(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			if (tile != null && tile.nactive() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type] && Main.tileSolid[tile.type] && tile.halfBrick())
			{
				return DoesTileTypeSupportPortals(tile.type);
			}
			return false;
		}

		private static bool SupportedNormal(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			if (tile != null && tile.nactive() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type] && Main.tileSolid[tile.type] && !TileID.Sets.NotReallySolid[tile.type] && !tile.halfBrick() && tile.slope() == 0)
			{
				return DoesTileTypeSupportPortals(tile.type);
			}
			return false;
		}
	}
}
