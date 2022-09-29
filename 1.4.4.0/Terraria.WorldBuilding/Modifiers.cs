using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;

namespace Terraria.WorldBuilding
{
	public static class Modifiers
	{
		public class ShapeScale : GenAction
		{
			private int _scale;

			public ShapeScale(int scale)
			{
				_scale = scale;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				bool flag = false;
				for (int i = 0; i < _scale; i++)
				{
					for (int j = 0; j < _scale; j++)
					{
						flag |= !UnitApply(origin, (x - origin.X << 1) + i + origin.X, (y - origin.Y << 1) + j + origin.Y);
					}
				}
				return !flag;
			}
		}

		public class Expand : GenAction
		{
			private int _xExpansion;

			private int _yExpansion;

			public Expand(int expansion)
			{
				_xExpansion = expansion;
				_yExpansion = expansion;
			}

			public Expand(int xExpansion, int yExpansion)
			{
				_xExpansion = xExpansion;
				_yExpansion = yExpansion;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				bool flag = false;
				for (int i = -_xExpansion; i <= _xExpansion; i++)
				{
					for (int j = -_yExpansion; j <= _yExpansion; j++)
					{
						flag |= !UnitApply(origin, x + i, y + j, args);
					}
				}
				return !flag;
			}
		}

		public class RadialDither : GenAction
		{
			private double _innerRadius;

			private double _outerRadius;

			public RadialDither(double innerRadius, double outerRadius)
			{
				_innerRadius = innerRadius;
				_outerRadius = outerRadius;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				Vector2D val = default(Vector2D);
				((Vector2D)(ref val))._002Ector((double)origin.X, (double)origin.Y);
				double num = Vector2D.Distance(new Vector2D((double)x, (double)y), val);
				double num2 = Math.Max(0.0, Math.Min(1.0, (num - _innerRadius) / (_outerRadius - _innerRadius)));
				if (GenBase._random.NextDouble() > num2)
				{
					return UnitApply(origin, x, y, args);
				}
				return Fail();
			}
		}

		public class Blotches : GenAction
		{
			private int _minX;

			private int _minY;

			private int _maxX;

			private int _maxY;

			private double _chance;

			public Blotches(int scale = 2, double chance = 0.3)
			{
				_minX = scale;
				_minY = scale;
				_maxX = scale;
				_maxY = scale;
				_chance = chance;
			}

			public Blotches(int xScale, int yScale, double chance = 0.3)
			{
				_minX = xScale;
				_maxX = xScale;
				_minY = yScale;
				_maxY = yScale;
				_chance = chance;
			}

			public Blotches(int leftScale, int upScale, int rightScale, int downScale, double chance = 0.3)
			{
				_minX = leftScale;
				_maxX = rightScale;
				_minY = upScale;
				_maxY = downScale;
				_chance = chance;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._random.NextDouble();
				if (GenBase._random.NextDouble() < _chance)
				{
					bool flag = false;
					int num = GenBase._random.Next(1 - _minX, 1);
					int num2 = GenBase._random.Next(0, _maxX);
					int num3 = GenBase._random.Next(1 - _minY, 1);
					int num4 = GenBase._random.Next(0, _maxY);
					for (int i = num; i <= num2; i++)
					{
						for (int j = num3; j <= num4; j++)
						{
							flag |= !UnitApply(origin, x + i, y + j, args);
						}
					}
					return !flag;
				}
				return UnitApply(origin, x, y, args);
			}
		}

		public class InShape : GenAction
		{
			private readonly ShapeData _shapeData;

			public InShape(ShapeData shapeData)
			{
				_shapeData = shapeData;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (!_shapeData.Contains(x - origin.X, y - origin.Y))
				{
					return Fail();
				}
				return UnitApply(origin, x, y, args);
			}
		}

		public class NotInShape : GenAction
		{
			private readonly ShapeData _shapeData;

			public NotInShape(ShapeData shapeData)
			{
				_shapeData = shapeData;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (_shapeData.Contains(x - origin.X, y - origin.Y))
				{
					return Fail();
				}
				return UnitApply(origin, x, y, args);
			}
		}

		public class Conditions : GenAction
		{
			private readonly GenCondition[] _conditions;

			public Conditions(params GenCondition[] conditions)
			{
				_conditions = conditions;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				bool flag = true;
				for (int i = 0; i < _conditions.Length; i++)
				{
					flag &= _conditions[i].IsValid(x, y);
				}
				if (flag)
				{
					return UnitApply(origin, x, y, args);
				}
				return Fail();
			}
		}

		public class OnlyWalls : GenAction
		{
			private ushort[] _types;

			public OnlyWalls(params ushort[] types)
			{
				_types = types;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				for (int i = 0; i < _types.Length; i++)
				{
					if (GenBase._tiles[x, y].wall == _types[i])
					{
						return UnitApply(origin, x, y, args);
					}
				}
				return Fail();
			}
		}

		public class OnlyTiles : GenAction
		{
			private ushort[] _types;

			public OnlyTiles(params ushort[] types)
			{
				_types = types;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (!GenBase._tiles[x, y].active())
				{
					return Fail();
				}
				for (int i = 0; i < _types.Length; i++)
				{
					if (GenBase._tiles[x, y].type == _types[i])
					{
						return UnitApply(origin, x, y, args);
					}
				}
				return Fail();
			}
		}

		public class IsTouching : GenAction
		{
			private static readonly int[] DIRECTIONS = new int[16]
			{
				0, -1, 1, 0, -1, 0, 0, 1, -1, -1,
				1, -1, -1, 1, 1, 1
			};

			private bool _useDiagonals;

			private ushort[] _tileIds;

			public IsTouching(bool useDiagonals, params ushort[] tileIds)
			{
				_useDiagonals = useDiagonals;
				_tileIds = tileIds;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				int num = (_useDiagonals ? 16 : 8);
				for (int i = 0; i < num; i += 2)
				{
					Tile tile = GenBase._tiles[x + DIRECTIONS[i], y + DIRECTIONS[i + 1]];
					if (!tile.active())
					{
						continue;
					}
					for (int j = 0; j < _tileIds.Length; j++)
					{
						if (tile.type == _tileIds[j])
						{
							return UnitApply(origin, x, y, args);
						}
					}
				}
				return Fail();
			}
		}

		public class NotTouching : GenAction
		{
			private static readonly int[] DIRECTIONS = new int[16]
			{
				0, -1, 1, 0, -1, 0, 0, 1, -1, -1,
				1, -1, -1, 1, 1, 1
			};

			private bool _useDiagonals;

			private ushort[] _tileIds;

			public NotTouching(bool useDiagonals, params ushort[] tileIds)
			{
				_useDiagonals = useDiagonals;
				_tileIds = tileIds;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				int num = (_useDiagonals ? 16 : 8);
				for (int i = 0; i < num; i += 2)
				{
					Tile tile = GenBase._tiles[x + DIRECTIONS[i], y + DIRECTIONS[i + 1]];
					if (!tile.active())
					{
						continue;
					}
					for (int j = 0; j < _tileIds.Length; j++)
					{
						if (tile.type == _tileIds[j])
						{
							return Fail();
						}
					}
				}
				return UnitApply(origin, x, y, args);
			}
		}

		public class IsTouchingAir : GenAction
		{
			private static readonly int[] DIRECTIONS = new int[16]
			{
				0, -1, 1, 0, -1, 0, 0, 1, -1, -1,
				1, -1, -1, 1, 1, 1
			};

			private bool _useDiagonals;

			public IsTouchingAir(bool useDiagonals = false)
			{
				_useDiagonals = useDiagonals;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				int num = (_useDiagonals ? 16 : 8);
				for (int i = 0; i < num; i += 2)
				{
					if (!GenBase._tiles[x + DIRECTIONS[i], y + DIRECTIONS[i + 1]].active())
					{
						return UnitApply(origin, x, y, args);
					}
				}
				return Fail();
			}
		}

		public class SkipTiles : GenAction
		{
			private ushort[] _types;

			public SkipTiles(params ushort[] types)
			{
				_types = types;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (!GenBase._tiles[x, y].active())
				{
					return UnitApply(origin, x, y, args);
				}
				for (int i = 0; i < _types.Length; i++)
				{
					if (GenBase._tiles[x, y].type == _types[i])
					{
						return Fail();
					}
				}
				return UnitApply(origin, x, y, args);
			}
		}

		public class HasLiquid : GenAction
		{
			private int _liquidType;

			private int _liquidLevel;

			public HasLiquid(int liquidLevel = -1, int liquidType = -1)
			{
				_liquidType = liquidType;
				_liquidLevel = liquidLevel;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile tile = GenBase._tiles[x, y];
				if ((_liquidType == -1 || _liquidType == tile.liquidType()) && ((_liquidLevel == -1 && tile.liquid != 0) || _liquidLevel == tile.liquid))
				{
					return UnitApply(origin, x, y, args);
				}
				return Fail();
			}
		}

		public class SkipWalls : GenAction
		{
			private ushort[] _types;

			public SkipWalls(params ushort[] types)
			{
				_types = types;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				for (int i = 0; i < _types.Length; i++)
				{
					if (GenBase._tiles[x, y].wall == _types[i])
					{
						return Fail();
					}
				}
				return UnitApply(origin, x, y, args);
			}
		}

		public class IsEmpty : GenAction
		{
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (!GenBase._tiles[x, y].active())
				{
					return UnitApply(origin, x, y, args);
				}
				return Fail();
			}
		}

		public class IsSolid : GenAction
		{
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (GenBase._tiles[x, y].active() && WorldGen.SolidOrSlopedTile(x, y))
				{
					return UnitApply(origin, x, y, args);
				}
				return Fail();
			}
		}

		public class IsNotSolid : GenAction
		{
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (!GenBase._tiles[x, y].active() || !WorldGen.SolidOrSlopedTile(x, y))
				{
					return UnitApply(origin, x, y, args);
				}
				return Fail();
			}
		}

		public class RectangleMask : GenAction
		{
			private int _xMin;

			private int _yMin;

			private int _xMax;

			private int _yMax;

			public RectangleMask(int xMin, int xMax, int yMin, int yMax)
			{
				_xMin = xMin;
				_yMin = yMin;
				_xMax = xMax;
				_yMax = yMax;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (x >= _xMin + origin.X && x <= _xMax + origin.X && y >= _yMin + origin.Y && y <= _yMax + origin.Y)
				{
					return UnitApply(origin, x, y, args);
				}
				return Fail();
			}
		}

		public class Offset : GenAction
		{
			private int _xOffset;

			private int _yOffset;

			public Offset(int x, int y)
			{
				_xOffset = x;
				_yOffset = y;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				return UnitApply(origin, x + _xOffset, y + _yOffset, args);
			}
		}

		public class Dither : GenAction
		{
			private double _failureChance;

			public Dither(double failureChance = 0.5)
			{
				_failureChance = failureChance;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (GenBase._random.NextDouble() >= _failureChance)
				{
					return UnitApply(origin, x, y, args);
				}
				return Fail();
			}
		}

		public class Flip : GenAction
		{
			private bool _flipX;

			private bool _flipY;

			public Flip(bool flipX, bool flipY)
			{
				_flipX = flipX;
				_flipY = flipY;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				if (_flipX)
				{
					x = origin.X * 2 - x;
				}
				if (_flipY)
				{
					y = origin.Y * 2 - y;
				}
				return UnitApply(origin, x, y, args);
			}
		}
	}
}
