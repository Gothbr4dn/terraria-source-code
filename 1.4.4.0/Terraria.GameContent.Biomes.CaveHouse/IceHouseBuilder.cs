using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse
{
	public class IceHouseBuilder : HouseBuilder
	{
		public IceHouseBuilder(IEnumerable<Rectangle> rooms)
			: base(HouseType.Ice, rooms)
		{
			base.TileType = 321;
			base.WallType = 149;
			base.BeamType = 574;
			base.DoorStyle = 30;
			base.PlatformStyle = 19;
			base.TableStyle = 28;
			base.WorkbenchStyle = 23;
			base.PianoStyle = 23;
			base.BookcaseStyle = 25;
			base.ChairStyle = 30;
			base.ChestStyle = 11;
		}

		protected override void AgeRoom(Rectangle room)
		{
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.6), new Modifiers.Blotches(2, 0.6), new Modifiers.OnlyTiles(base.TileType), new Actions.SetTileKeepWall(161, setSelfFrames: true), new Modifiers.Dither(0.8), new Actions.SetTileKeepWall(147, setSelfFrames: true)));
			WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(), new Modifiers.OnlyTiles(161), new Modifiers.Offset(0, 1), new ActionStalagtite()));
			WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(), new Modifiers.OnlyTiles(161), new Modifiers.Offset(0, 1), new ActionStalagtite()));
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.85), new Modifiers.Blotches(2, 0.8), new Modifiers.SkipTiles(SkipTilesDuringWallAging), ((double)room.Y > Main.worldSurface) ? ((GenAction)new Actions.ClearWall(frameNeighbors: true)) : ((GenAction)new Actions.PlaceWall(40))));
		}
	}
}
