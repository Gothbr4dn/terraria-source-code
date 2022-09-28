using System;

namespace Terraria.DataStructures
{
	public class TileObjectPreviewData
	{
		private ushort _type;

		private short _style;

		private int _alternate;

		private int _random;

		private bool _active;

		private Point16 _size;

		private Point16 _coordinates;

		private Point16 _objectStart;

		private int[,] _data;

		private Point16 _dataSize;

		private float _percentValid;

		public static TileObjectPreviewData placementCache;

		public static TileObjectPreviewData randomCache;

		public const int None = 0;

		public const int ValidSpot = 1;

		public const int InvalidSpot = 2;

		public bool Active
		{
			get
			{
				return _active;
			}
			set
			{
				_active = value;
			}
		}

		public ushort Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		public short Style
		{
			get
			{
				return _style;
			}
			set
			{
				_style = value;
			}
		}

		public int Alternate
		{
			get
			{
				return _alternate;
			}
			set
			{
				_alternate = value;
			}
		}

		public int Random
		{
			get
			{
				return _random;
			}
			set
			{
				_random = value;
			}
		}

		public Point16 Size
		{
			get
			{
				return _size;
			}
			set
			{
				if (value.X <= 0 || value.Y <= 0)
				{
					throw new FormatException("PlacementData.Size was set to a negative value.");
				}
				if (value.X > _dataSize.X || value.Y > _dataSize.Y)
				{
					int num = ((value.X > _dataSize.X) ? value.X : _dataSize.X);
					int num2 = ((value.Y > _dataSize.Y) ? value.Y : _dataSize.Y);
					int[,] array = new int[num, num2];
					if (_data != null)
					{
						for (int i = 0; i < _dataSize.X; i++)
						{
							for (int j = 0; j < _dataSize.Y; j++)
							{
								array[i, j] = _data[i, j];
							}
						}
					}
					_data = array;
					_dataSize = new Point16(num, num2);
				}
				_size = value;
			}
		}

		public Point16 Coordinates
		{
			get
			{
				return _coordinates;
			}
			set
			{
				_coordinates = value;
			}
		}

		public Point16 ObjectStart
		{
			get
			{
				return _objectStart;
			}
			set
			{
				_objectStart = value;
			}
		}

		public int this[int x, int y]
		{
			get
			{
				if (x < 0 || y < 0 || x >= _size.X || y >= _size.Y)
				{
					throw new IndexOutOfRangeException();
				}
				return _data[x, y];
			}
			set
			{
				if (x < 0 || y < 0 || x >= _size.X || y >= _size.Y)
				{
					throw new IndexOutOfRangeException();
				}
				_data[x, y] = value;
			}
		}

		public void Reset()
		{
			_active = false;
			_size = Point16.Zero;
			_coordinates = Point16.Zero;
			_objectStart = Point16.Zero;
			_percentValid = 0f;
			_type = 0;
			_style = 0;
			_alternate = -1;
			_random = -1;
			if (_data != null)
			{
				Array.Clear(_data, 0, _dataSize.X * _dataSize.Y);
			}
		}

		public void CopyFrom(TileObjectPreviewData copy)
		{
			_type = copy._type;
			_style = copy._style;
			_alternate = copy._alternate;
			_random = copy._random;
			_active = copy._active;
			_size = copy._size;
			_coordinates = copy._coordinates;
			_objectStart = copy._objectStart;
			_percentValid = copy._percentValid;
			if (_data == null)
			{
				_data = new int[copy._dataSize.X, copy._dataSize.Y];
				_dataSize = copy._dataSize;
			}
			else
			{
				Array.Clear(_data, 0, _data.Length);
			}
			if (_dataSize.X < copy._dataSize.X || _dataSize.Y < copy._dataSize.Y)
			{
				int num = ((copy._dataSize.X > _dataSize.X) ? copy._dataSize.X : _dataSize.X);
				int num2 = ((copy._dataSize.Y > _dataSize.Y) ? copy._dataSize.Y : _dataSize.Y);
				_data = new int[num, num2];
				_dataSize = new Point16(num, num2);
			}
			for (int i = 0; i < copy._dataSize.X; i++)
			{
				for (int j = 0; j < copy._dataSize.Y; j++)
				{
					_data[i, j] = copy._data[i, j];
				}
			}
		}

		public void AllInvalid()
		{
			for (int i = 0; i < _size.X; i++)
			{
				for (int j = 0; j < _size.Y; j++)
				{
					if (_data[i, j] != 0)
					{
						_data[i, j] = 2;
					}
				}
			}
		}
	}
}
