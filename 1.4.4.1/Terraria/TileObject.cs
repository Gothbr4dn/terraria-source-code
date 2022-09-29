using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria
{
	public struct TileObject
	{
		public int xCoord;

		public int yCoord;

		public int type;

		public int style;

		public int alternate;

		public int random;

		public static TileObject Empty = default(TileObject);

		public static TileObjectPreviewData objectPreview = new TileObjectPreviewData();

		public static bool Place(TileObject toBePlaced)
		{
			TileObjectData tileData = TileObjectData.GetTileData(toBePlaced.type, toBePlaced.style, toBePlaced.alternate);
			if (tileData == null)
			{
				return false;
			}
			if (tileData.HookPlaceOverride.hook != null)
			{
				int arg;
				int arg2;
				if (tileData.HookPlaceOverride.processedCoordinates)
				{
					arg = toBePlaced.xCoord;
					arg2 = toBePlaced.yCoord;
				}
				else
				{
					arg = toBePlaced.xCoord + tileData.Origin.X;
					arg2 = toBePlaced.yCoord + tileData.Origin.Y;
				}
				if (tileData.HookPlaceOverride.hook(arg, arg2, toBePlaced.type, toBePlaced.style, 1, toBePlaced.alternate) == tileData.HookPlaceOverride.badReturn)
				{
					return false;
				}
			}
			else
			{
				ushort num = (ushort)toBePlaced.type;
				int num2 = 0;
				int num3 = 0;
				int num4 = tileData.CalculatePlacementStyle(toBePlaced.style, toBePlaced.alternate, toBePlaced.random);
				int num5 = 0;
				if (tileData.StyleWrapLimit > 0)
				{
					num5 = num4 / tileData.StyleWrapLimit * tileData.StyleLineSkip;
					num4 %= tileData.StyleWrapLimit;
				}
				if (tileData.StyleHorizontal)
				{
					num2 = tileData.CoordinateFullWidth * num4;
					num3 = tileData.CoordinateFullHeight * num5;
				}
				else
				{
					num2 = tileData.CoordinateFullWidth * num5;
					num3 = tileData.CoordinateFullHeight * num4;
				}
				int num6 = toBePlaced.xCoord;
				int num7 = toBePlaced.yCoord;
				for (int i = 0; i < tileData.Width; i++)
				{
					for (int j = 0; j < tileData.Height; j++)
					{
						Tile tileSafely = Framing.GetTileSafely(num6 + i, num7 + j);
						if (tileSafely.active() && tileSafely.type != 484 && (Main.tileCut[tileSafely.type] || TileID.Sets.BreakableWhenPlacing[tileSafely.type]))
						{
							WorldGen.KillTile(num6 + i, num7 + j);
							if (!Main.tile[num6 + i, num7 + j].active() && Main.netMode != 0)
							{
								NetMessage.SendData(17, -1, -1, null, 0, num6 + i, num7 + j);
							}
						}
					}
				}
				for (int k = 0; k < tileData.Width; k++)
				{
					int num8 = num2 + k * (tileData.CoordinateWidth + tileData.CoordinatePadding);
					int num9 = num3;
					for (int l = 0; l < tileData.Height; l++)
					{
						Tile tileSafely2 = Framing.GetTileSafely(num6 + k, num7 + l);
						if (!tileSafely2.active())
						{
							tileSafely2.active(active: true);
							tileSafely2.frameX = (short)num8;
							tileSafely2.frameY = (short)num9;
							tileSafely2.type = num;
						}
						num9 += tileData.CoordinateHeights[l] + tileData.CoordinatePadding;
					}
				}
			}
			if (tileData.FlattenAnchors)
			{
				AnchorData anchorBottom = tileData.AnchorBottom;
				if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
				{
					int num10 = toBePlaced.xCoord + anchorBottom.checkStart;
					int j2 = toBePlaced.yCoord + tileData.Height;
					for (int m = 0; m < anchorBottom.tileCount; m++)
					{
						Tile tileSafely3 = Framing.GetTileSafely(num10 + m, j2);
						if (Main.tileSolid[tileSafely3.type] && !Main.tileSolidTop[tileSafely3.type] && tileSafely3.blockType() != 0)
						{
							WorldGen.SlopeTile(num10 + m, j2);
						}
					}
				}
				anchorBottom = tileData.AnchorTop;
				if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
				{
					int num11 = toBePlaced.xCoord + anchorBottom.checkStart;
					int j3 = toBePlaced.yCoord - 1;
					for (int n = 0; n < anchorBottom.tileCount; n++)
					{
						Tile tileSafely4 = Framing.GetTileSafely(num11 + n, j3);
						if (Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type] && tileSafely4.blockType() != 0)
						{
							WorldGen.SlopeTile(num11 + n, j3);
						}
					}
				}
				anchorBottom = tileData.AnchorRight;
				if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
				{
					int i2 = toBePlaced.xCoord + tileData.Width;
					int num12 = toBePlaced.yCoord + anchorBottom.checkStart;
					for (int num13 = 0; num13 < anchorBottom.tileCount; num13++)
					{
						Tile tileSafely5 = Framing.GetTileSafely(i2, num12 + num13);
						if (Main.tileSolid[tileSafely5.type] && !Main.tileSolidTop[tileSafely5.type] && tileSafely5.blockType() != 0)
						{
							WorldGen.SlopeTile(i2, num12 + num13);
						}
					}
				}
				anchorBottom = tileData.AnchorLeft;
				if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
				{
					int i3 = toBePlaced.xCoord - 1;
					int num14 = toBePlaced.yCoord + anchorBottom.checkStart;
					for (int num15 = 0; num15 < anchorBottom.tileCount; num15++)
					{
						Tile tileSafely6 = Framing.GetTileSafely(i3, num14 + num15);
						if (Main.tileSolid[tileSafely6.type] && !Main.tileSolidTop[tileSafely6.type] && tileSafely6.blockType() != 0)
						{
							WorldGen.SlopeTile(i3, num14 + num15);
						}
					}
				}
			}
			return true;
		}

		public static bool CanPlace(int x, int y, int type, int style, int dir, out TileObject objectData, bool onlyCheck = false, int? forcedRandom = null)
		{
			TileObjectData tileData = TileObjectData.GetTileData(type, style);
			objectData = Empty;
			if (tileData == null)
			{
				return false;
			}
			int num = x - tileData.Origin.X;
			int num2 = y - tileData.Origin.Y;
			if (num < 0 || num + tileData.Width >= Main.maxTilesX || num2 < 0 || num2 + tileData.Height >= Main.maxTilesY)
			{
				return false;
			}
			bool flag = tileData.RandomStyleRange > 0;
			if (TileObjectPreviewData.placementCache == null)
			{
				TileObjectPreviewData.placementCache = new TileObjectPreviewData();
			}
			TileObjectPreviewData.placementCache.Reset();
			int num3 = 0;
			if (tileData.AlternatesCount != 0)
			{
				num3 = tileData.AlternatesCount;
			}
			float num4 = -1f;
			float num5 = -1f;
			int num6 = 0;
			TileObjectData tileObjectData = null;
			int num7 = -1;
			while (num7 < num3)
			{
				num7++;
				TileObjectData tileData2 = TileObjectData.GetTileData(type, style, num7);
				if (tileData2.Direction != 0 && ((tileData2.Direction == TileObjectDirection.PlaceLeft && dir == 1) || (tileData2.Direction == TileObjectDirection.PlaceRight && dir == -1)))
				{
					continue;
				}
				int num8 = x - tileData2.Origin.X;
				int num9 = y - tileData2.Origin.Y;
				if (num8 < 5 || num8 + tileData2.Width > Main.maxTilesX - 5 || num9 < 5 || num9 + tileData2.Height > Main.maxTilesY - 5)
				{
					return false;
				}
				Rectangle rectangle = new Rectangle(0, 0, tileData2.Width, tileData2.Height);
				int num10 = 0;
				int num11 = 0;
				if (tileData2.AnchorTop.tileCount != 0)
				{
					if (rectangle.Y == 0)
					{
						rectangle.Y = -1;
						rectangle.Height++;
						num11++;
					}
					int checkStart = tileData2.AnchorTop.checkStart;
					if (checkStart < rectangle.X)
					{
						rectangle.Width += rectangle.X - checkStart;
						num10 += rectangle.X - checkStart;
						rectangle.X = checkStart;
					}
					int num12 = checkStart + tileData2.AnchorTop.tileCount - 1;
					int num13 = rectangle.X + rectangle.Width - 1;
					if (num12 > num13)
					{
						rectangle.Width += num12 - num13;
					}
				}
				if (tileData2.AnchorBottom.tileCount != 0)
				{
					if (rectangle.Y + rectangle.Height == tileData2.Height)
					{
						rectangle.Height++;
					}
					int checkStart2 = tileData2.AnchorBottom.checkStart;
					if (checkStart2 < rectangle.X)
					{
						rectangle.Width += rectangle.X - checkStart2;
						num10 += rectangle.X - checkStart2;
						rectangle.X = checkStart2;
					}
					int num14 = checkStart2 + tileData2.AnchorBottom.tileCount - 1;
					int num15 = rectangle.X + rectangle.Width - 1;
					if (num14 > num15)
					{
						rectangle.Width += num14 - num15;
					}
				}
				if (tileData2.AnchorLeft.tileCount != 0)
				{
					if (rectangle.X == 0)
					{
						rectangle.X = -1;
						rectangle.Width++;
						num10++;
					}
					int num16 = tileData2.AnchorLeft.checkStart;
					if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
					{
						num16--;
					}
					if (num16 < rectangle.Y)
					{
						rectangle.Width += rectangle.Y - num16;
						num11 += rectangle.Y - num16;
						rectangle.Y = num16;
					}
					int num17 = num16 + tileData2.AnchorLeft.tileCount - 1;
					if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
					{
						num17 += 2;
					}
					int num18 = rectangle.Y + rectangle.Height - 1;
					if (num17 > num18)
					{
						rectangle.Height += num17 - num18;
					}
				}
				if (tileData2.AnchorRight.tileCount != 0)
				{
					if (rectangle.X + rectangle.Width == tileData2.Width)
					{
						rectangle.Width++;
					}
					int num19 = tileData2.AnchorLeft.checkStart;
					if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
					{
						num19--;
					}
					if (num19 < rectangle.Y)
					{
						rectangle.Width += rectangle.Y - num19;
						num11 += rectangle.Y - num19;
						rectangle.Y = num19;
					}
					int num20 = num19 + tileData2.AnchorRight.tileCount - 1;
					if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
					{
						num20 += 2;
					}
					int num21 = rectangle.Y + rectangle.Height - 1;
					if (num20 > num21)
					{
						rectangle.Height += num20 - num21;
					}
				}
				if (onlyCheck)
				{
					objectPreview.Reset();
					objectPreview.Active = true;
					objectPreview.Type = (ushort)type;
					objectPreview.Style = (short)style;
					objectPreview.Alternate = num7;
					objectPreview.Size = new Point16(rectangle.Width, rectangle.Height);
					objectPreview.ObjectStart = new Point16(num10, num11);
					objectPreview.Coordinates = new Point16(num8 - num10, num9 - num11);
				}
				float num22 = 0f;
				float num23 = tileData2.Width * tileData2.Height;
				float num24 = 0f;
				float num25 = 0f;
				for (int i = 0; i < tileData2.Width; i++)
				{
					for (int j = 0; j < tileData2.Height; j++)
					{
						Tile tileSafely = Framing.GetTileSafely(num8 + i, num9 + j);
						bool flag2 = !tileData2.LiquidPlace(tileSafely);
						bool flag3 = false;
						if (tileData2.AnchorWall)
						{
							num25 += 1f;
							if (!tileData2.isValidWallAnchor(tileSafely.wall))
							{
								flag3 = true;
							}
							else
							{
								num24 += 1f;
							}
						}
						bool flag4 = false;
						if (tileSafely.active() && (!Main.tileCut[tileSafely.type] || tileSafely.type == 484 || tileSafely.type == 654) && !TileID.Sets.BreakableWhenPlacing[tileSafely.type])
						{
							flag4 = true;
						}
						if (flag4 || flag2 || flag3)
						{
							if (onlyCheck)
							{
								objectPreview[i + num10, j + num11] = 2;
							}
							continue;
						}
						if (onlyCheck)
						{
							objectPreview[i + num10, j + num11] = 1;
						}
						num22 += 1f;
					}
				}
				AnchorData anchorBottom = tileData2.AnchorBottom;
				if (anchorBottom.tileCount != 0)
				{
					num25 += (float)anchorBottom.tileCount;
					int height = tileData2.Height;
					for (int k = 0; k < anchorBottom.tileCount; k++)
					{
						int num26 = anchorBottom.checkStart + k;
						Tile tileSafely = Framing.GetTileSafely(num8 + num26, num9 + height);
						bool flag5 = false;
						if (tileSafely.nactive())
						{
							if ((anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type] && !Main.tileNoAttach[tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
							{
								flag5 = tileData2.isValidTileAnchor(tileSafely.type);
							}
							if (!flag5 && ((anchorBottom.type & AnchorType.SolidWithTop) == AnchorType.SolidWithTop || (anchorBottom.type & AnchorType.Table) == AnchorType.Table))
							{
								if (TileID.Sets.Platforms[tileSafely.type])
								{
									_ = tileSafely.frameX / TileObjectData.PlatformFrameWidth();
									if (!tileSafely.halfBrick() && WorldGen.PlatformProperTopFrame(tileSafely.frameX))
									{
										flag5 = true;
									}
								}
								else if (Main.tileSolid[tileSafely.type] && Main.tileSolidTop[tileSafely.type])
								{
									flag5 = true;
								}
							}
							if (!flag5 && (anchorBottom.type & AnchorType.Table) == AnchorType.Table && !TileID.Sets.Platforms[tileSafely.type] && Main.tileTable[tileSafely.type] && tileSafely.blockType() == 0)
							{
								flag5 = true;
							}
							if (!flag5 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type])
							{
								int num27 = tileSafely.blockType();
								if ((uint)(num27 - 4) <= 1u)
								{
									flag5 = tileData2.isValidTileAnchor(tileSafely.type);
								}
							}
							if (!flag5 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely.type))
							{
								flag5 = true;
							}
						}
						else if (!flag5 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
						{
							flag5 = true;
						}
						if (!flag5)
						{
							if (onlyCheck)
							{
								objectPreview[num26 + num10, height + num11] = 2;
							}
							continue;
						}
						if (onlyCheck)
						{
							objectPreview[num26 + num10, height + num11] = 1;
						}
						num24 += 1f;
					}
				}
				anchorBottom = tileData2.AnchorTop;
				if (anchorBottom.tileCount != 0)
				{
					num25 += (float)anchorBottom.tileCount;
					int num28 = -1;
					for (int l = 0; l < anchorBottom.tileCount; l++)
					{
						int num29 = anchorBottom.checkStart + l;
						Tile tileSafely = Framing.GetTileSafely(num8 + num29, num9 + num28);
						bool flag6 = false;
						if (tileSafely.nactive())
						{
							if (Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type] && !Main.tileNoAttach[tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
							{
								flag6 = tileData2.isValidTileAnchor(tileSafely.type);
							}
							if (!flag6 && (anchorBottom.type & AnchorType.SolidBottom) == AnchorType.SolidBottom && ((Main.tileSolid[tileSafely.type] && (!Main.tileSolidTop[tileSafely.type] || (TileID.Sets.Platforms[tileSafely.type] && (tileSafely.halfBrick() || tileSafely.topSlope())))) || tileSafely.halfBrick() || tileSafely.topSlope()) && !TileID.Sets.NotReallySolid[tileSafely.type] && !tileSafely.bottomSlope())
							{
								flag6 = tileData2.isValidTileAnchor(tileSafely.type);
							}
							if (!flag6 && (anchorBottom.type & AnchorType.Platform) == AnchorType.Platform && TileID.Sets.Platforms[tileSafely.type])
							{
								flag6 = tileData2.isValidTileAnchor(tileSafely.type);
							}
							if (!flag6 && (anchorBottom.type & AnchorType.PlanterBox) == AnchorType.PlanterBox && tileSafely.type == 380)
							{
								flag6 = tileData2.isValidTileAnchor(tileSafely.type);
							}
							if (!flag6 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type])
							{
								int num27 = tileSafely.blockType();
								if ((uint)(num27 - 2) <= 1u)
								{
									flag6 = tileData2.isValidTileAnchor(tileSafely.type);
								}
							}
							if (!flag6 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely.type))
							{
								flag6 = true;
							}
						}
						else if (!flag6 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
						{
							flag6 = true;
						}
						if (!flag6)
						{
							if (onlyCheck)
							{
								objectPreview[num29 + num10, num28 + num11] = 2;
							}
							continue;
						}
						if (onlyCheck)
						{
							objectPreview[num29 + num10, num28 + num11] = 1;
						}
						num24 += 1f;
					}
				}
				anchorBottom = tileData2.AnchorRight;
				if (anchorBottom.tileCount != 0)
				{
					num25 += (float)anchorBottom.tileCount;
					int width = tileData2.Width;
					for (int m = 0; m < anchorBottom.tileCount; m++)
					{
						int num30 = anchorBottom.checkStart + m;
						Tile tileSafely = Framing.GetTileSafely(num8 + width, num9 + num30);
						bool flag7 = false;
						if (tileSafely.nactive())
						{
							if (Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type] && !Main.tileNoAttach[tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
							{
								flag7 = tileData2.isValidTileAnchor(tileSafely.type);
							}
							if (!flag7 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type])
							{
								int num27 = tileSafely.blockType();
								if (num27 == 2 || num27 == 4)
								{
									flag7 = tileData2.isValidTileAnchor(tileSafely.type);
								}
							}
							if (!flag7 && (anchorBottom.type & AnchorType.Tree) == AnchorType.Tree && TileID.Sets.IsATreeTrunk[tileSafely.type])
							{
								flag7 = true;
								if (m == 0)
								{
									num25 += 1f;
									Tile tileSafely2 = Framing.GetTileSafely(num8 + width, num9 + num30 - 1);
									if (tileSafely2.nactive() && TileID.Sets.IsATreeTrunk[tileSafely2.type])
									{
										num24 += 1f;
										if (onlyCheck)
										{
											objectPreview[width + num10, num30 + num11 - 1] = 1;
										}
									}
									else if (onlyCheck)
									{
										objectPreview[width + num10, num30 + num11 - 1] = 2;
									}
								}
								if (m == anchorBottom.tileCount - 1)
								{
									num25 += 1f;
									Tile tileSafely3 = Framing.GetTileSafely(num8 + width, num9 + num30 + 1);
									if (tileSafely3.nactive() && TileID.Sets.IsATreeTrunk[tileSafely3.type])
									{
										num24 += 1f;
										if (onlyCheck)
										{
											objectPreview[width + num10, num30 + num11 + 1] = 1;
										}
									}
									else if (onlyCheck)
									{
										objectPreview[width + num10, num30 + num11 + 1] = 2;
									}
								}
							}
							if (!flag7 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely.type))
							{
								flag7 = true;
							}
						}
						else if (!flag7 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
						{
							flag7 = true;
						}
						if (!flag7)
						{
							if (onlyCheck)
							{
								objectPreview[width + num10, num30 + num11] = 2;
							}
							continue;
						}
						if (onlyCheck)
						{
							objectPreview[width + num10, num30 + num11] = 1;
						}
						num24 += 1f;
					}
				}
				anchorBottom = tileData2.AnchorLeft;
				if (anchorBottom.tileCount != 0)
				{
					num25 += (float)anchorBottom.tileCount;
					int num31 = -1;
					for (int n = 0; n < anchorBottom.tileCount; n++)
					{
						int num32 = anchorBottom.checkStart + n;
						Tile tileSafely = Framing.GetTileSafely(num8 + num31, num9 + num32);
						bool flag8 = false;
						if (tileSafely.nactive())
						{
							if (Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type] && !Main.tileNoAttach[tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
							{
								flag8 = tileData2.isValidTileAnchor(tileSafely.type);
							}
							if (!flag8 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type])
							{
								int num27 = tileSafely.blockType();
								if (num27 == 3 || num27 == 5)
								{
									flag8 = tileData2.isValidTileAnchor(tileSafely.type);
								}
							}
							if (!flag8 && (anchorBottom.type & AnchorType.Tree) == AnchorType.Tree && TileID.Sets.IsATreeTrunk[tileSafely.type])
							{
								flag8 = true;
								if (n == 0)
								{
									num25 += 1f;
									Tile tileSafely4 = Framing.GetTileSafely(num8 + num31, num9 + num32 - 1);
									if (tileSafely4.nactive() && TileID.Sets.IsATreeTrunk[tileSafely4.type])
									{
										num24 += 1f;
										if (onlyCheck)
										{
											objectPreview[num31 + num10, num32 + num11 - 1] = 1;
										}
									}
									else if (onlyCheck)
									{
										objectPreview[num31 + num10, num32 + num11 - 1] = 2;
									}
								}
								if (n == anchorBottom.tileCount - 1)
								{
									num25 += 1f;
									Tile tileSafely5 = Framing.GetTileSafely(num8 + num31, num9 + num32 + 1);
									if (tileSafely5.nactive() && TileID.Sets.IsATreeTrunk[tileSafely5.type])
									{
										num24 += 1f;
										if (onlyCheck)
										{
											objectPreview[num31 + num10, num32 + num11 + 1] = 1;
										}
									}
									else if (onlyCheck)
									{
										objectPreview[num31 + num10, num32 + num11 + 1] = 2;
									}
								}
							}
							if (!flag8 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely.type))
							{
								flag8 = true;
							}
						}
						else if (!flag8 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
						{
							flag8 = true;
						}
						if (!flag8)
						{
							if (onlyCheck)
							{
								objectPreview[num31 + num10, num32 + num11] = 2;
							}
							continue;
						}
						if (onlyCheck)
						{
							objectPreview[num31 + num10, num32 + num11] = 1;
						}
						num24 += 1f;
					}
				}
				if (tileData2.HookCheckIfCanPlace.hook != null)
				{
					if (tileData2.HookCheckIfCanPlace.processedCoordinates)
					{
						_ = tileData2.Origin;
						_ = tileData2.Origin;
					}
					if (tileData2.HookCheckIfCanPlace.hook(x, y, type, style, dir, num7) == tileData2.HookCheckIfCanPlace.badReturn && tileData2.HookCheckIfCanPlace.badResponse == 0)
					{
						num24 = 0f;
						num22 = 0f;
						objectPreview.AllInvalid();
					}
				}
				float num33 = num24 / num25;
				float num34 = num22 / num23;
				if (num34 == 1f && num25 == 0f)
				{
					num23 = 1f;
					num25 = 1f;
					num33 = 1f;
					num34 = 1f;
				}
				if (num33 == 1f && num34 == 1f)
				{
					num4 = 1f;
					num5 = 1f;
					num6 = num7;
					tileObjectData = tileData2;
					break;
				}
				if (num33 > num4 || (num33 == num4 && num34 > num5))
				{
					TileObjectPreviewData.placementCache.CopyFrom(objectPreview);
					num4 = num33;
					num5 = num34;
					tileObjectData = tileData2;
					num6 = num7;
				}
			}
			int num35 = -1;
			if (flag)
			{
				if (TileObjectPreviewData.randomCache == null)
				{
					TileObjectPreviewData.randomCache = new TileObjectPreviewData();
				}
				bool flag9 = false;
				if (TileObjectPreviewData.randomCache.Type == type)
				{
					Point16 coordinates = TileObjectPreviewData.randomCache.Coordinates;
					Point16 objectStart = TileObjectPreviewData.randomCache.ObjectStart;
					int num36 = coordinates.X + objectStart.X;
					int num37 = coordinates.Y + objectStart.Y;
					int num38 = x - tileData.Origin.X;
					int num39 = y - tileData.Origin.Y;
					if (num36 != num38 || num37 != num39)
					{
						flag9 = true;
					}
				}
				else
				{
					flag9 = true;
				}
				int randomStyleRange = tileData.RandomStyleRange;
				int num40 = Main.rand.Next(tileData.RandomStyleRange);
				if (forcedRandom.HasValue)
				{
					num40 = (forcedRandom.Value % randomStyleRange + randomStyleRange) % randomStyleRange;
				}
				num35 = ((!flag9 && !forcedRandom.HasValue) ? TileObjectPreviewData.randomCache.Random : num40);
			}
			if (tileData.SpecificRandomStyles != null)
			{
				if (TileObjectPreviewData.randomCache == null)
				{
					TileObjectPreviewData.randomCache = new TileObjectPreviewData();
				}
				bool flag10 = false;
				if (TileObjectPreviewData.randomCache.Type == type)
				{
					Point16 coordinates2 = TileObjectPreviewData.randomCache.Coordinates;
					Point16 objectStart2 = TileObjectPreviewData.randomCache.ObjectStart;
					int num41 = coordinates2.X + objectStart2.X;
					int num42 = coordinates2.Y + objectStart2.Y;
					int num43 = x - tileData.Origin.X;
					int num44 = y - tileData.Origin.Y;
					if (num41 != num43 || num42 != num44)
					{
						flag10 = true;
					}
				}
				else
				{
					flag10 = true;
				}
				int num45 = tileData.SpecificRandomStyles.Length;
				int num46 = Main.rand.Next(num45);
				if (forcedRandom.HasValue)
				{
					num46 = (forcedRandom.Value % num45 + num45) % num45;
				}
				num35 = ((!flag10 && !forcedRandom.HasValue) ? TileObjectPreviewData.randomCache.Random : (tileData.SpecificRandomStyles[num46] - style));
			}
			if (onlyCheck)
			{
				if (num4 != 1f || num5 != 1f)
				{
					objectPreview.CopyFrom(TileObjectPreviewData.placementCache);
					num7 = num6;
				}
				objectPreview.Random = num35;
				if (tileData.RandomStyleRange > 0 || tileData.SpecificRandomStyles != null)
				{
					TileObjectPreviewData.randomCache.CopyFrom(objectPreview);
				}
			}
			if (!onlyCheck)
			{
				objectData.xCoord = x - tileObjectData.Origin.X;
				objectData.yCoord = y - tileObjectData.Origin.Y;
				objectData.type = type;
				objectData.style = style;
				objectData.alternate = num7;
				objectData.random = num35;
			}
			if (num4 == 1f)
			{
				return num5 == 1f;
			}
			return false;
		}

		public static void DrawPreview(SpriteBatch sb, TileObjectPreviewData op, Vector2 position)
		{
			Point16 coordinates = op.Coordinates;
			Texture2D value = TextureAssets.Tile[op.Type].get_Value();
			TileObjectData tileData = TileObjectData.GetTileData(op.Type, op.Style, op.Alternate);
			int num = 0;
			int num2 = 0;
			int num3 = tileData.CalculatePlacementStyle(op.Style, op.Alternate, op.Random);
			int num4 = 0;
			int num5 = tileData.DrawYOffset;
			int drawXOffset = tileData.DrawXOffset;
			num3 += tileData.DrawStyleOffset;
			int num6 = tileData.StyleWrapLimit;
			int num7 = tileData.StyleLineSkip;
			if (tileData.StyleWrapLimitVisualOverride.HasValue)
			{
				num6 = tileData.StyleWrapLimitVisualOverride.Value;
			}
			if (tileData.styleLineSkipVisualOverride.HasValue)
			{
				num7 = tileData.styleLineSkipVisualOverride.Value;
			}
			if (num6 > 0)
			{
				num4 = num3 / num6 * num7;
				num3 %= num6;
			}
			if (tileData.StyleHorizontal)
			{
				num = tileData.CoordinateFullWidth * num3;
				num2 = tileData.CoordinateFullHeight * num4;
			}
			else
			{
				num = tileData.CoordinateFullWidth * num4;
				num2 = tileData.CoordinateFullHeight * num3;
			}
			for (int i = 0; i < op.Size.X; i++)
			{
				int x = num + (i - op.ObjectStart.X) * (tileData.CoordinateWidth + tileData.CoordinatePadding);
				int num8 = num2;
				for (int j = 0; j < op.Size.Y; j++)
				{
					int num9 = coordinates.X + i;
					int num10 = coordinates.Y + j;
					if (j == 0 && tileData.DrawStepDown != 0 && WorldGen.SolidTile(Framing.GetTileSafely(num9, num10 - 1)))
					{
						num5 += tileData.DrawStepDown;
					}
					if (op.Type == 567)
					{
						num5 = ((j != 0) ? tileData.DrawYOffset : (tileData.DrawYOffset - 2));
					}
					int num11 = op[i, j];
					Color color;
					if (num11 != 1)
					{
						if (num11 != 2)
						{
							continue;
						}
						color = Color.Red * 0.7f;
					}
					else
					{
						color = Color.White;
					}
					color *= 0.5f;
					if (i >= op.ObjectStart.X && i < op.ObjectStart.X + tileData.Width && j >= op.ObjectStart.Y && j < op.ObjectStart.Y + tileData.Height)
					{
						SpriteEffects spriteEffects = SpriteEffects.None;
						if (tileData.DrawFlipHorizontal && num9 % 2 == 0)
						{
							spriteEffects |= SpriteEffects.FlipHorizontally;
						}
						if (tileData.DrawFlipVertical && num10 % 2 == 0)
						{
							spriteEffects |= SpriteEffects.FlipVertically;
						}
						int coordinateWidth = tileData.CoordinateWidth;
						int num12 = tileData.CoordinateHeights[j - op.ObjectStart.Y];
						if (op.Type == 114 && j == 1)
						{
							num12 += 2;
						}
						sb.Draw(sourceRectangle: new Rectangle(x, num8, coordinateWidth, num12), texture: value, position: new Vector2(num9 * 16 - (int)(position.X + (float)(coordinateWidth - 16) / 2f) + drawXOffset, num10 * 16 - (int)position.Y + num5), color: color, rotation: 0f, origin: Vector2.Zero, scale: 1f, effects: spriteEffects, layerDepth: 0f);
						num8 += num12 + tileData.CoordinatePadding;
					}
				}
			}
		}
	}
}
