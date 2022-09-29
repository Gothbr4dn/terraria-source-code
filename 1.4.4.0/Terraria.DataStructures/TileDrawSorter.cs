using System;
using System.Collections.Generic;

namespace Terraria.DataStructures
{
	public class TileDrawSorter
	{
		public struct TileTexPoint
		{
			public int X;

			public int Y;

			public int TileType;

			public override string ToString()
			{
				return $"X:{X}, Y:{Y}, Type:{TileType}";
			}
		}

		public class CustomComparer : Comparer<TileTexPoint>
		{
			public override int Compare(TileTexPoint x, TileTexPoint y)
			{
				return x.TileType.CompareTo(y.TileType);
			}
		}

		public TileTexPoint[] tilesToDraw;

		private int _holderLength;

		private int _currentCacheIndex;

		private CustomComparer _tileComparer = new CustomComparer();

		public TileDrawSorter()
		{
			_currentCacheIndex = 0;
			_holderLength = 9000;
			tilesToDraw = new TileTexPoint[_holderLength];
		}

		public void reset()
		{
			_currentCacheIndex = 0;
		}

		public void Cache(int x, int y, int type)
		{
			int num = _currentCacheIndex++;
			tilesToDraw[num].X = x;
			tilesToDraw[num].Y = y;
			tilesToDraw[num].TileType = type;
			if (_currentCacheIndex == _holderLength)
			{
				IncreaseArraySize();
			}
		}

		private void IncreaseArraySize()
		{
			_holderLength *= 2;
			Array.Resize(ref tilesToDraw, _holderLength);
		}

		public void Sort()
		{
			Array.Sort(tilesToDraw, 0, _currentCacheIndex, _tileComparer);
		}

		public int GetAmountToDraw()
		{
			return _currentCacheIndex;
		}
	}
}
