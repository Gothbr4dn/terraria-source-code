using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse
{
	public class HouseBuilder
	{
		private const int VERTICAL_EXIT_WIDTH = 3;

		public static readonly HouseBuilder Invalid = new HouseBuilder();

		public readonly HouseType Type;

		public readonly bool IsValid;

		protected ushort[] SkipTilesDuringWallAging = new ushort[5] { 245, 246, 240, 241, 242 };

		public double ChestChance { get; set; }

		public ushort TileType { get; protected set; }

		public ushort WallType { get; protected set; }

		public ushort BeamType { get; protected set; }

		public int PlatformStyle { get; protected set; }

		public int DoorStyle { get; protected set; }

		public int TableStyle { get; protected set; }

		public bool UsesTables2 { get; protected set; }

		public int WorkbenchStyle { get; protected set; }

		public int PianoStyle { get; protected set; }

		public int BookcaseStyle { get; protected set; }

		public int ChairStyle { get; protected set; }

		public int ChestStyle { get; protected set; }

		public ReadOnlyCollection<Rectangle> Rooms { get; private set; }

		public Rectangle TopRoom => Rooms.First();

		public Rectangle BottomRoom => Rooms.Last();

		private UnifiedRandom _random => WorldGen.genRand;

		private Tile[,] _tiles => Main.tile;

		private HouseBuilder()
		{
			IsValid = false;
		}

		protected HouseBuilder(HouseType type, IEnumerable<Rectangle> rooms)
		{
			Type = type;
			IsValid = true;
			List<Rectangle> list = rooms.ToList();
			list.Sort((Rectangle lhs, Rectangle rhs) => lhs.Top.CompareTo(rhs.Top));
			Rooms = list.AsReadOnly();
		}

		protected virtual void AgeRoom(Rectangle room)
		{
		}

		public virtual void Place(HouseBuilderContext context, StructureMap structures)
		{
			PlaceEmptyRooms();
			foreach (Rectangle room in Rooms)
			{
				structures.AddProtectedStructure(room, 8);
			}
			PlaceStairs();
			PlaceDoors();
			PlacePlatforms();
			PlaceSupportBeams();
			PlaceBiomeSpecificPriorityTool(context);
			FillRooms();
			foreach (Rectangle room2 in Rooms)
			{
				AgeRoom(room2);
			}
			PlaceChests();
			PlaceBiomeSpecificTool(context);
		}

		private void PlaceEmptyRooms()
		{
			foreach (Rectangle room in Rooms)
			{
				WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Actions.SetTileKeepWall(TileType), new Actions.SetFrames(frameNeighbors: true)));
				WorldUtils.Gen(new Point(room.X + 1, room.Y + 1), new Shapes.Rectangle(room.Width - 2, room.Height - 2), Actions.Chain(new Actions.ClearTile(frameNeighbors: true), new Actions.PlaceWall(WallType)));
			}
		}

		private void FillRooms()
		{
			int x = 14;
			if (UsesTables2)
			{
				x = 469;
			}
			Point[] choices = new Point[7]
			{
				new Point(x, TableStyle),
				new Point(16, 0),
				new Point(18, WorkbenchStyle),
				new Point(86, 0),
				new Point(87, PianoStyle),
				new Point(94, 0),
				new Point(101, BookcaseStyle)
			};
			foreach (Rectangle room in Rooms)
			{
				int num = room.Width / 8;
				int num2 = room.Width / (num + 1);
				int num3 = _random.Next(2);
				for (int i = 0; i < num; i++)
				{
					int num4 = (i + 1) * num2 + room.X;
					switch (i + num3 % 2)
					{
					case 0:
					{
						int num5 = room.Y + Math.Min(room.Height / 2, room.Height - 5);
						PaintingEntry paintingEntry = ((Type == HouseType.Desert) ? WorldGen.RandHousePictureDesert() : WorldGen.RandHousePicture());
						WorldGen.PlaceTile(num4, num5, paintingEntry.tileType, mute: true, forced: false, -1, paintingEntry.style);
						break;
					}
					case 1:
					{
						int num5 = room.Y + 1;
						WorldGen.PlaceTile(num4, num5, 34, mute: true, forced: false, -1, _random.Next(6));
						for (int j = -1; j < 2; j++)
						{
							for (int k = 0; k < 3; k++)
							{
								_tiles[j + num4, k + num5].frameX += 54;
							}
						}
						break;
					}
					}
				}
				int num6 = room.Width / 8 + 3;
				WorldGen.SetupStatueList();
				while (num6 > 0)
				{
					int num7 = _random.Next(room.Width - 3) + 1 + room.X;
					int num8 = room.Y + room.Height - 2;
					switch (_random.Next(4))
					{
					case 0:
						WorldGen.PlaceSmallPile(num7, num8, _random.Next(31, 34), 1, 185);
						break;
					case 1:
						WorldGen.PlaceTile(num7, num8, 186, mute: true, forced: false, -1, _random.Next(22, 26));
						break;
					case 2:
					{
						int num9 = _random.Next(2, GenVars.statueList.Length);
						WorldGen.PlaceTile(num7, num8, GenVars.statueList[num9].X, mute: true, forced: false, -1, GenVars.statueList[num9].Y);
						if (GenVars.StatuesWithTraps.Contains(num9))
						{
							WorldGen.PlaceStatueTrap(num7, num8);
						}
						break;
					}
					case 3:
					{
						Point point = Utils.SelectRandom(_random, choices);
						WorldGen.PlaceTile(num7, num8, point.X, mute: true, forced: false, -1, point.Y);
						break;
					}
					}
					num6--;
				}
			}
		}

		private void PlaceStairs()
		{
			foreach (Tuple<Point, Point> item3 in CreateStairsList())
			{
				Point item = item3.Item1;
				Point item2 = item3.Item2;
				int num = ((item2.X > item.X) ? 1 : (-1));
				ShapeData shapeData = new ShapeData();
				for (int i = 0; i < item2.Y - item.Y; i++)
				{
					shapeData.Add(num * (i + 1), i);
				}
				WorldUtils.Gen(item, new ModShapes.All(shapeData), Actions.Chain(new Actions.PlaceTile(19, PlatformStyle), new Actions.SetSlope((num == 1) ? 1 : 2), new Actions.SetFrames(frameNeighbors: true)));
				WorldUtils.Gen(new Point(item.X + ((num == 1) ? 1 : (-4)), item.Y - 1), new Shapes.Rectangle(4, 1), Actions.Chain(new Actions.Clear(), new Actions.PlaceWall(WallType), new Actions.PlaceTile(19, PlatformStyle), new Actions.SetFrames(frameNeighbors: true)));
			}
		}

		private List<Tuple<Point, Point>> CreateStairsList()
		{
			List<Tuple<Point, Point>> list = new List<Tuple<Point, Point>>();
			for (int i = 1; i < Rooms.Count; i++)
			{
				Rectangle rectangle = Rooms[i];
				Rectangle rectangle2 = Rooms[i - 1];
				int num = rectangle2.X - rectangle.X;
				int num2 = rectangle.X + rectangle.Width - (rectangle2.X + rectangle2.Width);
				if (num > num2)
				{
					list.Add(new Tuple<Point, Point>(new Point(rectangle.X + rectangle.Width - 1, rectangle.Y + 1), new Point(rectangle.X + rectangle.Width - rectangle.Height + 1, rectangle.Y + rectangle.Height - 1)));
				}
				else
				{
					list.Add(new Tuple<Point, Point>(new Point(rectangle.X, rectangle.Y + 1), new Point(rectangle.X + rectangle.Height - 1, rectangle.Y + rectangle.Height - 1)));
				}
			}
			return list;
		}

		private void PlaceDoors()
		{
			foreach (Point item in CreateDoorList())
			{
				WorldUtils.Gen(item, new Shapes.Rectangle(1, 3), new Actions.ClearTile(frameNeighbors: true));
				WorldGen.PlaceTile(item.X, item.Y, 10, mute: true, forced: true, -1, DoorStyle);
			}
		}

		private List<Point> CreateDoorList()
		{
			List<Point> list = new List<Point>();
			foreach (Rectangle room in Rooms)
			{
				if (FindSideExit(new Rectangle(room.X + room.Width, room.Y + 1, 1, room.Height - 2), isLeft: false, out var exitY))
				{
					list.Add(new Point(room.X + room.Width - 1, exitY));
				}
				if (FindSideExit(new Rectangle(room.X, room.Y + 1, 1, room.Height - 2), isLeft: true, out exitY))
				{
					list.Add(new Point(room.X, exitY));
				}
			}
			return list;
		}

		private void PlacePlatforms()
		{
			foreach (Point item in CreatePlatformsList())
			{
				WorldUtils.Gen(item, new Shapes.Rectangle(3, 1), Actions.Chain(new Actions.ClearMetadata(), new Actions.PlaceTile(19, PlatformStyle), new Actions.SetFrames(frameNeighbors: true)));
			}
		}

		private List<Point> CreatePlatformsList()
		{
			List<Point> list = new List<Point>();
			Rectangle topRoom = TopRoom;
			Rectangle bottomRoom = BottomRoom;
			if (FindVerticalExit(new Rectangle(topRoom.X + 2, topRoom.Y, topRoom.Width - 4, 1), isUp: true, out var exitX))
			{
				list.Add(new Point(exitX, topRoom.Y));
			}
			if (FindVerticalExit(new Rectangle(bottomRoom.X + 2, bottomRoom.Y + bottomRoom.Height - 1, bottomRoom.Width - 4, 1), isUp: false, out exitX))
			{
				list.Add(new Point(exitX, bottomRoom.Y + bottomRoom.Height - 1));
			}
			return list;
		}

		private void PlaceSupportBeams()
		{
			foreach (Rectangle item in CreateSupportBeamList())
			{
				if (item.Height > 1 && _tiles[item.X, item.Y - 1].type != 19)
				{
					WorldUtils.Gen(new Point(item.X, item.Y), new Shapes.Rectangle(item.Width, item.Height), Actions.Chain(new Actions.SetTileKeepWall(BeamType), new Actions.SetFrames(frameNeighbors: true)));
					Tile tile = _tiles[item.X, item.Y + item.Height];
					tile.slope(0);
					tile.halfBrick(halfBrick: false);
				}
			}
		}

		private List<Rectangle> CreateSupportBeamList()
		{
			List<Rectangle> list = new List<Rectangle>();
			int num = Rooms.Min((Rectangle room) => room.Left);
			int num2 = Rooms.Max((Rectangle room) => room.Right) - 1;
			int num3 = 6;
			while (num3 > 4 && (num2 - num) % num3 != 0)
			{
				num3--;
			}
			for (int i = num; i <= num2; i += num3)
			{
				for (int j = 0; j < Rooms.Count; j++)
				{
					Rectangle rectangle = Rooms[j];
					if (i < rectangle.X || i >= rectangle.X + rectangle.Width)
					{
						continue;
					}
					int num4 = rectangle.Y + rectangle.Height;
					int num5 = 50;
					for (int k = j + 1; k < Rooms.Count; k++)
					{
						if (i >= Rooms[k].X && i < Rooms[k].X + Rooms[k].Width)
						{
							num5 = Math.Min(num5, Rooms[k].Y - num4);
						}
					}
					if (num5 > 0)
					{
						Point result;
						bool flag = WorldUtils.Find(new Point(i, num4), Searches.Chain(new Searches.Down(num5), new Conditions.IsSolid()), out result);
						if (num5 < 50)
						{
							flag = true;
							result = new Point(i, num4 + num5);
						}
						if (flag)
						{
							list.Add(new Rectangle(i, num4, 1, result.Y - num4));
						}
					}
				}
			}
			return list;
		}

		private static bool FindVerticalExit(Rectangle wall, bool isUp, out int exitX)
		{
			Point result;
			bool result2 = WorldUtils.Find(new Point(wall.X + wall.Width - 3, wall.Y + (isUp ? (-5) : 0)), Searches.Chain(new Searches.Left(wall.Width - 3), new Conditions.IsSolid().Not().AreaOr(3, 5)), out result);
			exitX = result.X;
			return result2;
		}

		private static bool FindSideExit(Rectangle wall, bool isLeft, out int exitY)
		{
			Point result;
			bool result2 = WorldUtils.Find(new Point(wall.X + (isLeft ? (-4) : 0), wall.Y + wall.Height - 3), Searches.Chain(new Searches.Up(wall.Height - 3), new Conditions.IsSolid().Not().AreaOr(4, 3)), out result);
			exitY = result.Y;
			return result2;
		}

		private void PlaceChests()
		{
			if (_random.NextDouble() > ChestChance)
			{
				return;
			}
			bool flag = false;
			foreach (Rectangle room in Rooms)
			{
				int num = room.Height - 1 + room.Y;
				int style = ((num > (int)Main.worldSurface) ? ChestStyle : 0);
				for (int i = 0; i < 10; i++)
				{
					if (flag = WorldGen.AddBuriedChest(_random.Next(2, room.Width - 2) + room.X, num, 0, notNearOtherChests: false, style, trySlope: false, 0))
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
				for (int j = room.X + 2; j <= room.X + room.Width - 2; j++)
				{
					if (flag = WorldGen.AddBuriedChest(j, num, 0, notNearOtherChests: false, style, trySlope: false, 0))
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				foreach (Rectangle room2 in Rooms)
				{
					int num2 = room2.Y - 1;
					int style2 = ((num2 > (int)Main.worldSurface) ? ChestStyle : 0);
					for (int k = 0; k < 10; k++)
					{
						if (flag = WorldGen.AddBuriedChest(_random.Next(2, room2.Width - 2) + room2.X, num2, 0, notNearOtherChests: false, style2, trySlope: false, 0))
						{
							break;
						}
					}
					if (flag)
					{
						break;
					}
					for (int l = room2.X + 2; l <= room2.X + room2.Width - 2; l++)
					{
						if (flag = WorldGen.AddBuriedChest(l, num2, 0, notNearOtherChests: false, style2, trySlope: false, 0))
						{
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				return;
			}
			for (int m = 0; m < 1000; m++)
			{
				int i2 = _random.Next(Rooms[0].X - 30, Rooms[0].X + 30);
				int num3 = _random.Next(Rooms[0].Y - 30, Rooms[0].Y + 30);
				int style3 = ((num3 > (int)Main.worldSurface) ? ChestStyle : 0);
				if (flag = WorldGen.AddBuriedChest(i2, num3, 0, notNearOtherChests: false, style3, trySlope: false, 0))
				{
					break;
				}
			}
		}

		private void PlaceBiomeSpecificPriorityTool(HouseBuilderContext context)
		{
			if (Type != HouseType.Desert || GenVars.extraBastStatueCount >= GenVars.extraBastStatueCountMax)
			{
				return;
			}
			bool flag = false;
			foreach (Rectangle room in Rooms)
			{
				int num = room.Height - 2 + room.Y;
				if (WorldGen.remixWorldGen && (double)num > Main.rockLayer)
				{
					return;
				}
				for (int i = 0; i < 10; i++)
				{
					int num2 = _random.Next(2, room.Width - 2) + room.X;
					WorldGen.PlaceTile(num2, num, 506, mute: true, forced: true);
					if (flag = _tiles[num2, num].active() && _tiles[num2, num].type == 506)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
				for (int j = room.X + 2; j <= room.X + room.Width - 2; j++)
				{
					if (flag = WorldGen.PlaceTile(j, num, 506, mute: true, forced: true))
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				foreach (Rectangle room2 in Rooms)
				{
					int num3 = room2.Y - 1;
					for (int k = 0; k < 10; k++)
					{
						int num4 = _random.Next(2, room2.Width - 2) + room2.X;
						WorldGen.PlaceTile(num4, num3, 506, mute: true, forced: true);
						if (flag = _tiles[num4, num3].active() && _tiles[num4, num3].type == 506)
						{
							break;
						}
					}
					if (flag)
					{
						break;
					}
					for (int l = room2.X + 2; l <= room2.X + room2.Width - 2; l++)
					{
						if (flag = WorldGen.PlaceTile(l, num3, 506, mute: true, forced: true))
						{
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				GenVars.extraBastStatueCount++;
			}
		}

		private void PlaceBiomeSpecificTool(HouseBuilderContext context)
		{
			if (Type == HouseType.Jungle && context.SharpenerCount < _random.Next(2, 5))
			{
				bool flag = false;
				foreach (Rectangle room in Rooms)
				{
					int num = room.Height - 2 + room.Y;
					for (int i = 0; i < 10; i++)
					{
						int num2 = _random.Next(2, room.Width - 2) + room.X;
						WorldGen.PlaceTile(num2, num, 377, mute: true, forced: true);
						if (flag = _tiles[num2, num].active() && _tiles[num2, num].type == 377)
						{
							break;
						}
					}
					if (flag)
					{
						break;
					}
					for (int j = room.X + 2; j <= room.X + room.Width - 2; j++)
					{
						if (flag = WorldGen.PlaceTile(j, num, 377, mute: true, forced: true))
						{
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					context.SharpenerCount++;
				}
			}
			if (Type != HouseType.Desert || context.ExtractinatorCount >= _random.Next(2, 5))
			{
				return;
			}
			bool flag2 = false;
			foreach (Rectangle room2 in Rooms)
			{
				int num3 = room2.Height - 2 + room2.Y;
				for (int k = 0; k < 10; k++)
				{
					int num4 = _random.Next(2, room2.Width - 2) + room2.X;
					WorldGen.PlaceTile(num4, num3, 219, mute: true, forced: true);
					if (flag2 = _tiles[num4, num3].active() && _tiles[num4, num3].type == 219)
					{
						break;
					}
				}
				if (flag2)
				{
					break;
				}
				for (int l = room2.X + 2; l <= room2.X + room2.Width - 2; l++)
				{
					if (flag2 = WorldGen.PlaceTile(l, num3, 219, mute: true, forced: true))
					{
						break;
					}
				}
				if (flag2)
				{
					break;
				}
			}
			if (flag2)
			{
				context.ExtractinatorCount++;
			}
		}
	}
}
