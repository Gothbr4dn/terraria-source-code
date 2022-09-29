using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	public class HoneyPatchBiome : MicroBiome
	{
		public override bool Place(Point origin, StructureMap structures)
		{
			if (GenBase._tiles[origin.X, origin.Y].active() && WorldGen.SolidTile(origin.X, origin.Y))
			{
				return false;
			}
			if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(80), new Conditions.IsSolid()), out var result))
			{
				return false;
			}
			result.Y += 2;
			Ref<int> @ref = new Ref<int>(0);
			WorldUtils.Gen(result, new Shapes.Circle(8), Actions.Chain(new Modifiers.IsSolid(), new Actions.Scanner(@ref)));
			if (@ref.Value < 20)
			{
				return false;
			}
			if (!structures.CanPlace(new Rectangle(result.X - 8, result.Y - 8, 16, 16)))
			{
				return false;
			}
			WorldUtils.Gen(result, new Shapes.Circle(8), Actions.Chain(new Modifiers.RadialDither(0.0, 10.0), new Modifiers.IsSolid(), new Actions.SetTile(229, setSelfFrames: true)));
			ShapeData data = new ShapeData();
			WorldUtils.Gen(result, new Shapes.Circle(4, 3), Actions.Chain(new Modifiers.Blotches(), new Modifiers.IsSolid(), new Actions.ClearTile(frameNeighbors: true), new Modifiers.RectangleMask(-6, 6, 0, 3).Output(data), new Actions.SetLiquid(2)));
			WorldUtils.Gen(new Point(result.X, result.Y + 1), new ModShapes.InnerOutline(data), Actions.Chain(new Modifiers.IsEmpty(), new Modifiers.RectangleMask(-6, 6, 1, 3), new Actions.SetTile(59, setSelfFrames: true)));
			structures.AddProtectedStructure(new Rectangle(result.X - 8, result.Y - 8, 16, 16));
			return true;
		}
	}
}
