using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	public class ThinIceBiome : MicroBiome
	{
		public override bool Place(Point origin, StructureMap structures)
		{
			Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
			WorldUtils.Gen(new Point(origin.X - 25, origin.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(0, 59, 147, 1, 225).Output(dictionary));
			int num = dictionary[0] + dictionary[1];
			int num2 = dictionary[59];
			int num3 = dictionary[147];
			if (dictionary[225] > 0)
			{
				return false;
			}
			if (num3 <= num2 || num3 <= num)
			{
				return false;
			}
			int num4 = 0;
			for (int num5 = GenBase._random.Next(10, 15); num5 > 5; num5--)
			{
				int num6 = GenBase._random.Next(-5, 5);
				WorldUtils.Gen(new Point(origin.X + num6, origin.Y + num4), new Shapes.Circle(num5), Actions.Chain(new Modifiers.Blotches(4), new Modifiers.OnlyTiles(147, 161, 224, 0, 1), new Actions.SetTile(162, setSelfFrames: true)));
				WorldUtils.Gen(new Point(origin.X + num6, origin.Y + num4), new Shapes.Circle(num5), Actions.Chain(new Modifiers.Blotches(4), new Modifiers.HasLiquid(), new Actions.SetTile(162, setSelfFrames: true), new Actions.SetLiquid(0, 0)));
				num4 += num5 - 2;
			}
			structures.AddStructure(new Rectangle(origin.X - 25, origin.Y - 25, 50, 50), 8);
			return true;
		}
	}
}
