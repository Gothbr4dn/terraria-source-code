using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Terraria.WorldBuilding
{
	public static class Actions
	{
		public class ContinueWrapper : GenAction
		{
			private GenAction _action;

			public ContinueWrapper(GenAction action)
			{
				_action = action;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				_action.Apply(origin, x, y, args);
				return UnitApply(origin, x, y, args);
			}
		}

		public class Count : GenAction
		{
			private Ref<int> _count;

			public Count(Ref<int> count)
			{
				_count = count;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				_count.Value++;
				return UnitApply(origin, x, y, args);
			}
		}

		public class Scanner : GenAction
		{
			private Ref<int> _count;

			public Scanner(Ref<int> count)
			{
				_count = count;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				_count.Value++;
				return UnitApply(origin, x, y, args);
			}
		}

		public class TileScanner : GenAction
		{
			private ushort[] _tileIds;

			private Dictionary<ushort, int> _tileCounts;

			public TileScanner(params ushort[] tiles)
			{
				_tileIds = tiles;
				_tileCounts = new Dictionary<ushort, int>();
				for (int i = 0; i < tiles.Length; i++)
				{
					_tileCounts[_tileIds[i]] = 0;
				}
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile tile = GenBase._tiles[x, y];
				if (tile.active() && _tileCounts.ContainsKey(tile.type))
				{
					_tileCounts[tile.type]++;
				}
				return UnitApply(origin, x, y, args);
			}

			public TileScanner Output(Dictionary<ushort, int> resultsOutput)
			{
				_tileCounts = resultsOutput;
				for (int i = 0; i < _tileIds.Length; i++)
				{
					if (!_tileCounts.ContainsKey(_tileIds[i]))
					{
						_tileCounts[_tileIds[i]] = 0;
					}
				}
				return this;
			}

			public Dictionary<ushort, int> GetResults()
			{
				return _tileCounts;
			}

			public int GetCount(ushort tileId)
			{
				if (!_tileCounts.ContainsKey(tileId))
				{
					return -1;
				}
				return _tileCounts[tileId];
			}
		}

		public class Blank : GenAction
		{
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				return UnitApply(origin, x, y, args);
			}
		}

		public class Custom : GenAction
		{
			private CustomPerUnitAction _perUnit;

			public Custom(CustomPerUnitAction perUnit)
			{
				_perUnit = perUnit;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				return _perUnit(x, y, args) | UnitApply(origin, x, y, args);
			}
		}

		public class ClearMetadata : GenAction
		{
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].ClearMetadata();
				return UnitApply(origin, x, y, args);
			}
		}

		public class Clear : GenAction
		{
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].ClearEverything();
				return UnitApply(origin, x, y, args);
			}
		}

		public class ClearTile : GenAction
		{
			private bool _frameNeighbors;

			public ClearTile(bool frameNeighbors = false)
			{
				_frameNeighbors = frameNeighbors;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				WorldUtils.ClearTile(x, y, _frameNeighbors);
				return UnitApply(origin, x, y, args);
			}
		}

		public class ClearWall : GenAction
		{
			private bool _frameNeighbors;

			public ClearWall(bool frameNeighbors = false)
			{
				_frameNeighbors = frameNeighbors;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				WorldUtils.ClearWall(x, y, _frameNeighbors);
				return UnitApply(origin, x, y, args);
			}
		}

		public class HalfBlock : GenAction
		{
			private bool _value;

			public HalfBlock(bool value = true)
			{
				_value = value;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].halfBrick(_value);
				return UnitApply(origin, x, y, args);
			}
		}

		public class SetTile : GenAction
		{
			private ushort _type;

			private bool _doFraming;

			private bool _doNeighborFraming;

			public SetTile(ushort type, bool setSelfFrames = false, bool setNeighborFrames = true)
			{
				_type = type;
				_doFraming = setSelfFrames;
				_doNeighborFraming = setNeighborFrames;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].Clear(~(TileDataType.Wiring | TileDataType.Actuator));
				GenBase._tiles[x, y].type = _type;
				GenBase._tiles[x, y].active(active: true);
				if (_doFraming)
				{
					WorldUtils.TileFrame(x, y, _doNeighborFraming);
				}
				return UnitApply(origin, x, y, args);
			}
		}

		public class SetTileKeepWall : GenAction
		{
			private ushort _type;

			private bool _doFraming;

			private bool _doNeighborFraming;

			public SetTileKeepWall(ushort type, bool setSelfFrames = false, bool setNeighborFrames = true)
			{
				_type = type;
				_doFraming = setSelfFrames;
				_doNeighborFraming = setNeighborFrames;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				ushort wall = GenBase._tiles[x, y].wall;
				int wallFrameX = GenBase._tiles[x, y].wallFrameX();
				int wallFrameY = GenBase._tiles[x, y].wallFrameY();
				GenBase._tiles[x, y].Clear(~(TileDataType.Wiring | TileDataType.Actuator));
				GenBase._tiles[x, y].type = _type;
				GenBase._tiles[x, y].active(active: true);
				if (wall > 0)
				{
					GenBase._tiles[x, y].wall = wall;
					GenBase._tiles[x, y].wallFrameX(wallFrameX);
					GenBase._tiles[x, y].wallFrameY(wallFrameY);
				}
				if (_doFraming)
				{
					WorldUtils.TileFrame(x, y, _doNeighborFraming);
				}
				return UnitApply(origin, x, y, args);
			}
		}

		public class DebugDraw : GenAction
		{
			private Color _color;

			private SpriteBatch _spriteBatch;

			public DebugDraw(SpriteBatch spriteBatch, Color color = default(Color))
			{
				_spriteBatch = spriteBatch;
				_color = color;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				_spriteBatch.Draw(TextureAssets.MagicPixel.get_Value(), new Rectangle((x << 4) - (int)Main.screenPosition.X, (y << 4) - (int)Main.screenPosition.Y, 16, 16), _color);
				return UnitApply(origin, x, y, args);
			}
		}

		public class SetSlope : GenAction
		{
			private int _slope;

			public SetSlope(int slope)
			{
				_slope = slope;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				WorldGen.SlopeTile(x, y, _slope);
				return UnitApply(origin, x, y, args);
			}
		}

		public class SetHalfTile : GenAction
		{
			private bool _halfTile;

			public SetHalfTile(bool halfTile)
			{
				_halfTile = halfTile;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].halfBrick(_halfTile);
				return UnitApply(origin, x, y, args);
			}
		}

		public class SetTileAndWallRainbowPaint : GenAction
		{
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				byte paintIDForPosition = GetPaintIDForPosition(x, y);
				GenBase._tiles[x, y].color(paintIDForPosition);
				GenBase._tiles[x, y].wallColor(paintIDForPosition);
				return UnitApply(origin, x, y, args);
			}

			private byte GetPaintIDForPosition(int x, int y)
			{
				int num = x % 52 + y % 52;
				num %= 26;
				if (num > 12)
				{
					num = 12 - (num - 12);
				}
				num = Math.Min(12, Math.Max(1, num));
				return (byte)(12 + num);
			}
		}

		public class PlaceTile : GenAction
		{
			private ushort _type;

			private int _style;

			public PlaceTile(ushort type, int style = 0)
			{
				_type = type;
				_style = style;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				WorldGen.PlaceTile(x, y, _type, mute: true, forced: false, -1, _style);
				return UnitApply(origin, x, y, args);
			}
		}

		public class RemoveWall : GenAction
		{
			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].wall = 0;
				return UnitApply(origin, x, y, args);
			}
		}

		public class PlaceWall : GenAction
		{
			private ushort _type;

			private bool _neighbors;

			public PlaceWall(ushort type, bool neighbors = true)
			{
				_type = type;
				_neighbors = neighbors;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].wall = _type;
				WorldGen.SquareWallFrame(x, y);
				if (_neighbors)
				{
					WorldGen.SquareWallFrame(x + 1, y);
					WorldGen.SquareWallFrame(x - 1, y);
					WorldGen.SquareWallFrame(x, y - 1);
					WorldGen.SquareWallFrame(x, y + 1);
				}
				return UnitApply(origin, x, y, args);
			}
		}

		public class SetLiquid : GenAction
		{
			private int _type;

			private byte _value;

			public SetLiquid(int type = 0, byte value = byte.MaxValue)
			{
				_value = value;
				_type = type;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				GenBase._tiles[x, y].liquidType(_type);
				GenBase._tiles[x, y].liquid = _value;
				return UnitApply(origin, x, y, args);
			}
		}

		public class SwapSolidTile : GenAction
		{
			private ushort _type;

			public SwapSolidTile(ushort type)
			{
				_type = type;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile tile = GenBase._tiles[x, y];
				if (WorldGen.SolidTile(tile))
				{
					tile.ResetToType(_type);
					return UnitApply(origin, x, y, args);
				}
				return Fail();
			}
		}

		public class SetFrames : GenAction
		{
			private bool _frameNeighbors;

			public SetFrames(bool frameNeighbors = false)
			{
				_frameNeighbors = frameNeighbors;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				WorldUtils.TileFrame(x, y, _frameNeighbors);
				return UnitApply(origin, x, y, args);
			}
		}

		public class Smooth : GenAction
		{
			private bool _applyToNeighbors;

			public Smooth(bool applyToNeighbors = false)
			{
				_applyToNeighbors = applyToNeighbors;
			}

			public override bool Apply(Point origin, int x, int y, params object[] args)
			{
				Tile.SmoothSlope(x, y, _applyToNeighbors);
				return UnitApply(origin, x, y, args);
			}
		}

		public static GenAction Chain(params GenAction[] actions)
		{
			for (int i = 0; i < actions.Length - 1; i++)
			{
				actions[i].NextAction = actions[i + 1];
			}
			return actions[0];
		}

		public static GenAction Continue(GenAction action)
		{
			return new ContinueWrapper(action);
		}
	}
}
