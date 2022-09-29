using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	public class CampsiteBiome : MicroBiome
	{
		public override bool Place(Point origin, StructureMap structures)
		{
			Ref<int> @ref = new Ref<int>(0);
			Ref<int> ref2 = new Ref<int>(0);
			WorldUtils.Gen(origin, new Shapes.Circle(10), Actions.Chain(new Actions.Scanner(ref2), new Modifiers.IsSolid(), new Actions.Scanner(@ref)));
			if (@ref.Value < ref2.Value - 5)
			{
				return false;
			}
			int num = GenBase._random.Next(6, 10);
			int num2 = GenBase._random.Next(1, 5);
			if (!structures.CanPlace(new Rectangle(origin.X - num, origin.Y - num, num * 2, num * 2)))
			{
				return false;
			}
			ushort type = (byte)(196 + WorldGen.genRand.Next(4));
			for (int i = origin.X - num; i <= origin.X + num; i++)
			{
				for (int j = origin.Y - num; j <= origin.Y + num; j++)
				{
					if (Main.tile[i, j].active())
					{
						int type2 = Main.tile[i, j].type;
						if (type2 == 53 || type2 == 396 || type2 == 397 || type2 == 404)
						{
							type = 171;
						}
						if (type2 == 161 || type2 == 147)
						{
							type = 40;
						}
						if (type2 == 60)
						{
							type = (byte)(204 + WorldGen.genRand.Next(4));
						}
						if (type2 == 367)
						{
							type = 178;
						}
						if (type2 == 368)
						{
							type = 180;
						}
					}
				}
			}
			ShapeData data = new ShapeData();
			WorldUtils.Gen(origin, new Shapes.Slime(num), Actions.Chain(new Modifiers.Blotches(num2, num2, num2, 1, 1.0).Output(data), new Modifiers.Offset(0, -2), new Modifiers.OnlyTiles(53), new Actions.SetTile(397, setSelfFrames: true), new Modifiers.OnlyWalls(default(ushort)), new Actions.PlaceWall(type)));
			WorldUtils.Gen(origin, new ModShapes.All(data), Actions.Chain(new Actions.ClearTile(), new Actions.SetLiquid(0, 0), new Actions.SetFrames(frameNeighbors: true), new Modifiers.OnlyWalls(default(ushort)), new Actions.PlaceWall(type)));
			if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(10), new Conditions.IsSolid()), out var result))
			{
				return false;
			}
			int num3 = result.Y - 1;
			bool flag = GenBase._random.Next() % 2 == 0;
			if (GenBase._random.Next() % 10 != 0)
			{
				int num4 = GenBase._random.Next(1, 4);
				int num5 = (flag ? 4 : (-(num >> 1)));
				for (int k = 0; k < num4; k++)
				{
					int num6 = GenBase._random.Next(1, 3);
					for (int l = 0; l < num6; l++)
					{
						WorldGen.PlaceTile(origin.X + num5 - k, num3 - l, 332, mute: true);
					}
				}
			}
			int num7 = (num - 3) * ((!flag) ? 1 : (-1));
			if (GenBase._random.Next() % 10 != 0)
			{
				WorldGen.PlaceTile(origin.X + num7, num3, 186);
			}
			if (GenBase._random.Next() % 10 != 0)
			{
				WorldGen.PlaceTile(origin.X, num3, 215, mute: true);
				if (GenBase._tiles[origin.X, num3].active() && GenBase._tiles[origin.X, num3].type == 215)
				{
					GenBase._tiles[origin.X, num3].frameY += 36;
					GenBase._tiles[origin.X - 1, num3].frameY += 36;
					GenBase._tiles[origin.X + 1, num3].frameY += 36;
					GenBase._tiles[origin.X, num3 - 1].frameY += 36;
					GenBase._tiles[origin.X - 1, num3 - 1].frameY += 36;
					GenBase._tiles[origin.X + 1, num3 - 1].frameY += 36;
				}
			}
			structures.AddProtectedStructure(new Rectangle(origin.X - num, origin.Y - num, num * 2, num * 2), 4);
			return true;
		}
	}
}
