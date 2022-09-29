using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert
{
	public static class AnthillEntrance
	{
		public static void Place(DesertDescription description)
		{
			int num = WorldGen.genRand.Next(2, 4);
			for (int i = 0; i < num; i++)
			{
				int holeRadius = WorldGen.genRand.Next(15, 18);
				int num2 = (int)((double)(i + 1) / (double)(num + 1) * (double)description.Surface.Width);
				num2 += description.Desert.Left;
				int y = description.Surface[num2];
				PlaceAt(description, new Point(num2, y), holeRadius);
			}
		}

		private static void PlaceAt(DesertDescription description, Point position, int holeRadius)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			ShapeData data = new ShapeData();
			Point origin = new Point(position.X, position.Y + 6);
			WorldUtils.Gen(origin, new Shapes.Tail(holeRadius * 2, new Vector2D(0.0, (double)(-holeRadius) * 1.5)), Actions.Chain(new Actions.SetTile(53).Output(data)));
			GenShapeActionPair genShapeActionPair = new GenShapeActionPair(new Shapes.Rectangle(1, 1), Actions.Chain(new Modifiers.Blotches(), new Modifiers.IsSolid(), new Actions.Clear(), new Actions.PlaceWall(187)));
			GenShapeActionPair genShapeActionPair2 = new GenShapeActionPair(new Shapes.Rectangle(1, 1), Actions.Chain(new Modifiers.IsSolid(), new Actions.Clear(), new Actions.PlaceWall(187)));
			GenShapeActionPair pair = new GenShapeActionPair(new Shapes.Circle(2, 3), Actions.Chain(new Modifiers.IsSolid(), new Actions.SetTile(397), new Actions.PlaceWall(187)));
			GenShapeActionPair pair2 = new GenShapeActionPair(new Shapes.Circle(holeRadius, 3), Actions.Chain(new Modifiers.SkipWalls(187), new Actions.SetTile(53)));
			GenShapeActionPair pair3 = new GenShapeActionPair(new Shapes.Circle(holeRadius - 2, 3), Actions.Chain(new Actions.PlaceWall(187)));
			int num = position.X;
			for (int i = position.Y - holeRadius - 3; i < description.Hive.Top + (position.Y - description.Desert.Top) * 2 + 12; i++)
			{
				WorldUtils.Gen(new Point(num, i), (i < position.Y) ? genShapeActionPair2 : genShapeActionPair);
				WorldUtils.Gen(new Point(num, i), pair);
				if (i % 3 == 0 && i >= position.Y)
				{
					num += WorldGen.genRand.Next(-1, 2);
					WorldUtils.Gen(new Point(num, i), genShapeActionPair);
					if (i >= position.Y + 5)
					{
						WorldUtils.Gen(new Point(num, i), pair2);
						WorldUtils.Gen(new Point(num, i), pair3);
					}
					WorldUtils.Gen(new Point(num, i), pair);
				}
			}
			WorldUtils.Gen(new Point(origin.X, origin.Y - (int)((double)holeRadius * 1.5) + 3), new Shapes.Circle(holeRadius / 2, holeRadius / 3), Actions.Chain(Actions.Chain(new Actions.ClearTile(), new Modifiers.Expand(1), new Actions.PlaceWall(0))));
			WorldUtils.Gen(origin, new ModShapes.All(data), new Actions.Smooth());
		}
	}
}
