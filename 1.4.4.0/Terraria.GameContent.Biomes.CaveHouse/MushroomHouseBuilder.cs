using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse
{
	public class MushroomHouseBuilder : HouseBuilder
	{
		public MushroomHouseBuilder(IEnumerable<Rectangle> rooms)
			: base(HouseType.Mushroom, rooms)
		{
			base.TileType = 190;
			base.WallType = 74;
			base.BeamType = 578;
			base.PlatformStyle = 18;
			base.DoorStyle = 6;
			base.TableStyle = 27;
			base.WorkbenchStyle = 7;
			base.PianoStyle = 22;
			base.BookcaseStyle = 24;
			base.ChairStyle = 9;
			base.ChestStyle = 32;
		}

		protected override void AgeRoom(Rectangle room)
		{
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.7), new Modifiers.Blotches(2, 0.5), new Modifiers.OnlyTiles(base.TileType), new Actions.SetTileKeepWall(70, setSelfFrames: true)));
			WorldUtils.Gen(new Point(room.X + 1, room.Y), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(0.6), new Modifiers.OnlyTiles(70), new Modifiers.Offset(0, -1), new Modifiers.IsEmpty(), new Actions.SetTile(71)));
			WorldUtils.Gen(new Point(room.X + 1, room.Y + room.Height - 1), new Shapes.Rectangle(room.Width - 2, 1), Actions.Chain(new Modifiers.Dither(0.6), new Modifiers.OnlyTiles(70), new Modifiers.Offset(0, -1), new Modifiers.IsEmpty(), new Actions.SetTile(71)));
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.85), new Modifiers.Blotches(), new Actions.ClearWall()));
		}
	}
}
