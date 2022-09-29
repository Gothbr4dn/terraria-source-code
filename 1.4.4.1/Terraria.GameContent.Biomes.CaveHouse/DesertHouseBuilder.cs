using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse
{
	public class DesertHouseBuilder : HouseBuilder
	{
		public DesertHouseBuilder(IEnumerable<Rectangle> rooms)
			: base(HouseType.Desert, rooms)
		{
			base.TileType = 396;
			base.WallType = 187;
			base.BeamType = 577;
			base.PlatformStyle = 42;
			base.DoorStyle = 43;
			base.TableStyle = 7;
			base.UsesTables2 = true;
			base.WorkbenchStyle = 39;
			base.PianoStyle = 38;
			base.BookcaseStyle = 39;
			base.ChairStyle = 43;
			base.ChestStyle = 1;
		}

		protected override void AgeRoom(Rectangle room)
		{
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.8), new Modifiers.Blotches(2, 0.2), new Modifiers.OnlyTiles(base.TileType), new Actions.SetTileKeepWall(396, setSelfFrames: true), new Modifiers.Dither(), new Actions.SetTileKeepWall(397, setSelfFrames: true)));
			WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(), new Modifiers.OnlyTiles(397, 396), new Modifiers.Offset(0, 1), new ActionStalagtite()));
			WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(), new Modifiers.OnlyTiles(397, 396), new Modifiers.Offset(0, 1), new ActionStalagtite()));
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.8), new Modifiers.Blotches(), new Modifiers.OnlyWalls(base.WallType), new Actions.PlaceWall(216)));
		}
	}
}
