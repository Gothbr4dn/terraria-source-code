using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse
{
	public class JungleHouseBuilder : HouseBuilder
	{
		public JungleHouseBuilder(IEnumerable<Rectangle> rooms)
			: base(HouseType.Jungle, rooms)
		{
			base.TileType = 158;
			base.WallType = 42;
			base.BeamType = 575;
			base.PlatformStyle = 2;
			base.DoorStyle = 2;
			base.TableStyle = 2;
			base.WorkbenchStyle = 2;
			base.PianoStyle = 2;
			base.BookcaseStyle = 12;
			base.ChairStyle = 3;
			base.ChestStyle = 8;
		}

		protected override void AgeRoom(Rectangle room)
		{
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.6), new Modifiers.Blotches(2, 0.6), new Modifiers.OnlyTiles(base.TileType), new Actions.SetTileKeepWall(60, setSelfFrames: true), new Modifiers.Dither(0.8), new Actions.SetTileKeepWall(59, setSelfFrames: true)));
			WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(), new Modifiers.OnlyTiles(60), new Modifiers.Offset(0, 1), new Modifiers.IsEmpty(), new ActionVines(3, room.Height, 62)));
			WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(), new Modifiers.OnlyTiles(60), new Modifiers.Offset(0, 1), new Modifiers.IsEmpty(), new ActionVines(3, room.Height, 62)));
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.85), new Modifiers.Blotches(), new Actions.PlaceWall(64)));
		}
	}
}
