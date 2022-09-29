using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes
{
	public class JunglePass : GenPass
	{
		private double _worldScale;

		public JunglePass()
			: base("Jungle", 10154.65234375)
		{
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = Lang.gen[11].Value;
			_worldScale = (double)Main.maxTilesX / 4200.0 * 1.5;
			double worldScale = _worldScale;
			Point point = CreateStartPoint();
			int x = point.X;
			int y = point.Y;
			Point zero = Point.Zero;
			ApplyRandomMovement(ref x, ref y, 100, 100);
			zero.X += x;
			zero.Y += y;
			PlaceFirstPassMud(x, y, 3);
			PlaceGemsAt(x, y, 63, 2);
			progress.Set(0.15);
			ApplyRandomMovement(ref x, ref y, 250, 150);
			zero.X += x;
			zero.Y += y;
			PlaceFirstPassMud(x, y, 0);
			PlaceGemsAt(x, y, 65, 2);
			progress.Set(0.3);
			int oldX = x;
			int oldY = y;
			ApplyRandomMovement(ref x, ref y, 400, 150);
			zero.X += x;
			zero.Y += y;
			PlaceFirstPassMud(x, y, -3);
			PlaceGemsAt(x, y, 67, 2);
			progress.Set(0.45);
			x = zero.X / 3;
			y = zero.Y / 3;
			int num = GenBase._random.Next((int)(400.0 * worldScale), (int)(600.0 * worldScale));
			int num2 = (int)(25.0 * worldScale);
			x = Utils.Clamp(x, GenVars.leftBeachEnd + num / 2 + num2, GenVars.rightBeachStart - num / 2 - num2);
			GenVars.mudWall = true;
			WorldGen.TileRunner(x, y, num, 10000, 59, addTile: false, 0.0, -20.0, noYChange: true);
			GenerateTunnelToSurface(x, y);
			GenVars.mudWall = false;
			progress.Set(0.6);
			GenerateHolesInMudWalls();
			GenerateFinishingTouches(progress, oldX, oldY);
		}

		private void PlaceGemsAt(int x, int y, ushort baseGem, int gemVariants)
		{
			for (int i = 0; (double)i < 6.0 * _worldScale; i++)
			{
				WorldGen.TileRunner(x + GenBase._random.Next(-(int)(125.0 * _worldScale), (int)(125.0 * _worldScale)), y + GenBase._random.Next(-(int)(125.0 * _worldScale), (int)(125.0 * _worldScale)), GenBase._random.Next(3, 7), GenBase._random.Next(3, 8), GenBase._random.Next(baseGem, baseGem + gemVariants));
			}
		}

		private void PlaceFirstPassMud(int x, int y, int xSpeedScale)
		{
			GenVars.mudWall = true;
			WorldGen.TileRunner(x, y, GenBase._random.Next((int)(250.0 * _worldScale), (int)(500.0 * _worldScale)), GenBase._random.Next(50, 150), 59, addTile: false, GenVars.dungeonSide * xSpeedScale);
			GenVars.mudWall = false;
		}

		private Point CreateStartPoint()
		{
			return new Point(GenVars.jungleOriginX, (int)((double)Main.maxTilesY + Main.rockLayer) / 2);
		}

		private void ApplyRandomMovement(ref int x, ref int y, int xRange, int yRange)
		{
			x += GenBase._random.Next((int)((double)(-xRange) * _worldScale), 1 + (int)((double)xRange * _worldScale));
			y += GenBase._random.Next((int)((double)(-yRange) * _worldScale), 1 + (int)((double)yRange * _worldScale));
			y = Utils.Clamp(y, (int)Main.rockLayer, Main.maxTilesY);
		}

		private void GenerateTunnelToSurface(int i, int j)
		{
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_041b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
			double num = GenBase._random.Next(5, 11);
			Vector2D val = default(Vector2D);
			val.X = i;
			val.Y = j;
			Vector2D val2 = default(Vector2D);
			val2.X = (double)GenBase._random.Next(-10, 11) * 0.1;
			val2.Y = (double)GenBase._random.Next(10, 20) * 0.1;
			int num2 = 0;
			bool flag = true;
			while (flag)
			{
				if (val.Y < Main.worldSurface)
				{
					if (WorldGen.drunkWorldGen)
					{
						flag = false;
					}
					int value = (int)val.X;
					int value2 = (int)val.Y;
					value = Utils.Clamp(value, 10, Main.maxTilesX - 10);
					value2 = Utils.Clamp(value2, 10, Main.maxTilesY - 10);
					if (value2 < 5)
					{
						value2 = 5;
					}
					if (Main.tile[value, value2].wall == 0 && !Main.tile[value, value2].active() && Main.tile[value, value2 - 3].wall == 0 && !Main.tile[value, value2 - 3].active() && Main.tile[value, value2 - 1].wall == 0 && !Main.tile[value, value2 - 1].active() && Main.tile[value, value2 - 4].wall == 0 && !Main.tile[value, value2 - 4].active() && Main.tile[value, value2 - 2].wall == 0 && !Main.tile[value, value2 - 2].active() && Main.tile[value, value2 - 5].wall == 0 && !Main.tile[value, value2 - 5].active())
					{
						flag = false;
					}
				}
				GenVars.JungleX = (int)val.X;
				num += (double)GenBase._random.Next(-20, 21) * 0.1;
				if (num < 5.0)
				{
					num = 5.0;
				}
				if (num > 10.0)
				{
					num = 10.0;
				}
				int value3 = (int)(val.X - num * 0.5);
				int value4 = (int)(val.X + num * 0.5);
				int value5 = (int)(val.Y - num * 0.5);
				int value6 = (int)(val.Y + num * 0.5);
				int num3 = Utils.Clamp(value3, 10, Main.maxTilesX - 10);
				value4 = Utils.Clamp(value4, 10, Main.maxTilesX - 10);
				value5 = Utils.Clamp(value5, 10, Main.maxTilesY - 10);
				value6 = Utils.Clamp(value6, 10, Main.maxTilesY - 10);
				for (int k = num3; k < value4; k++)
				{
					for (int l = value5; l < value6; l++)
					{
						if (Math.Abs((double)k - val.X) + Math.Abs((double)l - val.Y) < num * 0.5 * (1.0 + (double)GenBase._random.Next(-10, 11) * 0.015))
						{
							WorldGen.KillTile(k, l);
						}
					}
				}
				num2++;
				if (num2 > 10 && GenBase._random.Next(50) < num2)
				{
					num2 = 0;
					int num4 = -2;
					if (GenBase._random.Next(2) == 0)
					{
						num4 = 2;
					}
					WorldGen.TileRunner((int)val.X, (int)val.Y, GenBase._random.Next(3, 20), GenBase._random.Next(10, 100), -1, addTile: false, num4);
				}
				val += val2;
				val2.Y += (double)GenBase._random.Next(-10, 11) * 0.01;
				if (val2.Y > 0.0)
				{
					val2.Y = 0.0;
				}
				if (val2.Y < -2.0)
				{
					val2.Y = -2.0;
				}
				val2.X += (double)GenBase._random.Next(-10, 11) * 0.1;
				if (val.X < (double)(i - 200))
				{
					val2.X += (double)GenBase._random.Next(5, 21) * 0.1;
				}
				if (val.X > (double)(i + 200))
				{
					val2.X -= (double)GenBase._random.Next(5, 21) * 0.1;
				}
				if (val2.X > 1.5)
				{
					val2.X = 1.5;
				}
				if (val2.X < -1.5)
				{
					val2.X = -1.5;
				}
			}
		}

		private void GenerateHolesInMudWalls()
		{
			for (int i = 0; i < Main.maxTilesX / 4; i++)
			{
				int num = GenBase._random.Next(20, Main.maxTilesX - 20);
				int num2 = GenBase._random.Next((int)GenVars.worldSurface + 10, Main.UnderworldLayer);
				while (Main.tile[num, num2].wall != 64 && Main.tile[num, num2].wall != 15)
				{
					num = GenBase._random.Next(20, Main.maxTilesX - 20);
					num2 = GenBase._random.Next((int)GenVars.worldSurface + 10, Main.UnderworldLayer);
				}
				WorldGen.MudWallRunner(num, num2);
			}
		}

		private void GenerateFinishingTouches(GenerationProgress progress, int oldX, int oldY)
		{
			int num = oldX;
			int num2 = oldY;
			double worldScale = _worldScale;
			for (int i = 0; (double)i <= 20.0 * worldScale; i++)
			{
				progress.Set((60.0 + (double)i / worldScale) * 0.01);
				num += GenBase._random.Next((int)(-5.0 * worldScale), (int)(6.0 * worldScale));
				num2 += GenBase._random.Next((int)(-5.0 * worldScale), (int)(6.0 * worldScale));
				WorldGen.TileRunner(num, num2, GenBase._random.Next(40, 100), GenBase._random.Next(300, 500), 59);
			}
			for (int j = 0; (double)j <= 10.0 * worldScale; j++)
			{
				progress.Set((80.0 + (double)j / worldScale * 2.0) * 0.01);
				num = oldX + GenBase._random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
				num2 = oldY + GenBase._random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
				while (num < 1 || num >= Main.maxTilesX - 1 || num2 < 1 || num2 >= Main.maxTilesY - 1 || Main.tile[num, num2].type != 59)
				{
					num = oldX + GenBase._random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
					num2 = oldY + GenBase._random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
				}
				for (int k = 0; (double)k < 8.0 * worldScale; k++)
				{
					num += GenBase._random.Next(-30, 31);
					num2 += GenBase._random.Next(-30, 31);
					int type = -1;
					if (GenBase._random.Next(7) == 0)
					{
						type = -2;
					}
					WorldGen.TileRunner(num, num2, GenBase._random.Next(10, 20), GenBase._random.Next(30, 70), type);
				}
			}
			for (int l = 0; (double)l <= 300.0 * worldScale; l++)
			{
				num = oldX + GenBase._random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
				num2 = oldY + GenBase._random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
				while (num < 1 || num >= Main.maxTilesX - 1 || num2 < 1 || num2 >= Main.maxTilesY - 1 || Main.tile[num, num2].type != 59)
				{
					num = oldX + GenBase._random.Next((int)(-600.0 * worldScale), (int)(600.0 * worldScale));
					num2 = oldY + GenBase._random.Next((int)(-200.0 * worldScale), (int)(200.0 * worldScale));
				}
				WorldGen.TileRunner(num, num2, GenBase._random.Next(4, 10), GenBase._random.Next(5, 30), 1);
				if (GenBase._random.Next(4) == 0)
				{
					int type2 = GenBase._random.Next(63, 69);
					WorldGen.TileRunner(num + GenBase._random.Next(-1, 2), num2 + GenBase._random.Next(-1, 2), GenBase._random.Next(3, 7), GenBase._random.Next(4, 8), type2);
				}
			}
		}
	}
}
