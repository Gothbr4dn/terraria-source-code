using System;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	public class ShapeRoot : GenShape
	{
		private double _angle;

		private double _startingSize;

		private double _endingSize;

		private double _distance;

		public ShapeRoot(double angle, double distance = 10.0, double startingSize = 4.0, double endingSize = 1.0)
		{
			_angle = angle;
			_distance = distance;
			_startingSize = startingSize;
			_endingSize = endingSize;
		}

		private bool DoRoot(Point origin, GenAction action, double angle, double distance, double startingSize)
		{
			double num = origin.X;
			double num2 = origin.Y;
			for (double num3 = 0.0; num3 < distance * 0.85; num3 += 1.0)
			{
				double num4 = num3 / distance;
				double num5 = Utils.Lerp(startingSize, _endingSize, num4);
				num += Math.Cos(angle);
				num2 += Math.Sin(angle);
				angle += (double)GenBase._random.NextFloat() - 0.5 + (double)GenBase._random.NextFloat() * (_angle - 1.5707963705062866) * 0.1 * (1.0 - num4);
				angle = angle * 0.4 + 0.45 * Utils.Clamp(angle, _angle - 2.0 * (1.0 - 0.5 * num4), _angle + 2.0 * (1.0 - 0.5 * num4)) + Utils.Lerp(_angle, 1.5707963705062866, num4) * 0.15;
				for (int i = 0; i < (int)num5; i++)
				{
					for (int j = 0; j < (int)num5; j++)
					{
						if (!UnitApply(action, origin, (int)num + i, (int)num2 + j) && _quitOnFail)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			return DoRoot(origin, action, _angle, _distance, _startingSize);
		}
	}
}
