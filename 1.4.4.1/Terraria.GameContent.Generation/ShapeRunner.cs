using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation
{
	public class ShapeRunner : GenShape
	{
		private double _startStrength;

		private int _steps;

		private Vector2D _startVelocity;

		public ShapeRunner(double strength, int steps, Vector2D velocity)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			_startStrength = strength;
			_steps = steps;
			_startVelocity = velocity;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			double num = _steps;
			double num2 = _steps;
			double num3 = _startStrength;
			Vector2D val = default(Vector2D);
			((Vector2D)(ref val))._002Ector((double)origin.X, (double)origin.Y);
			Vector2D val2 = ((_startVelocity == Vector2D.get_Zero()) ? Utils.RandomVector2D(GenBase._random, -1.0, 1.0) : _startVelocity);
			while (num > 0.0 && num3 > 0.0)
			{
				num3 = _startStrength * (num / num2);
				num -= 1.0;
				int num4 = Math.Max(1, (int)(val.X - num3 * 0.5));
				int num5 = Math.Max(1, (int)(val.Y - num3 * 0.5));
				int num6 = Math.Min(GenBase._worldWidth, (int)(val.X + num3 * 0.5));
				int num7 = Math.Min(GenBase._worldHeight, (int)(val.Y + num3 * 0.5));
				for (int i = num4; i < num6; i++)
				{
					for (int j = num5; j < num7; j++)
					{
						if (!(Math.Abs((double)i - val.X) + Math.Abs((double)j - val.Y) >= num3 * 0.5 * (1.0 + (double)GenBase._random.Next(-10, 11) * 0.015)))
						{
							UnitApply(action, origin, i, j);
						}
					}
				}
				int num8 = (int)(num3 / 50.0) + 1;
				num -= (double)num8;
				val += val2;
				for (int k = 0; k < num8; k++)
				{
					val += val2;
					val2 += Utils.RandomVector2D(GenBase._random, -0.5, 0.5);
				}
				val2 += Utils.RandomVector2D(GenBase._random, -0.5, 0.5);
				val2 = Vector2D.Clamp(val2, -Vector2D.get_One(), Vector2D.get_One());
			}
			return true;
		}
	}
}
