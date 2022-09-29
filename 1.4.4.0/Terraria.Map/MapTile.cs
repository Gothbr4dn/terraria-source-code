namespace Terraria.Map
{
	public struct MapTile
	{
		public ushort Type;

		public byte Light;

		private byte _extraData;

		public bool IsChanged
		{
			get
			{
				return (_extraData & 0x80) == 128;
			}
			set
			{
				if (value)
				{
					_extraData |= 128;
				}
				else
				{
					_extraData &= 127;
				}
			}
		}

		public byte Color
		{
			get
			{
				return (byte)(_extraData & 0x7Fu);
			}
			set
			{
				_extraData = (byte)((_extraData & 0x80u) | (value & 0x7Fu));
			}
		}

		private MapTile(ushort type, byte light, byte extraData)
		{
			Type = type;
			Light = light;
			_extraData = extraData;
		}

		public bool Equals(ref MapTile other)
		{
			if (Light == other.Light && Type == other.Type)
			{
				return Color == other.Color;
			}
			return false;
		}

		public bool EqualsWithoutLight(ref MapTile other)
		{
			if (Type == other.Type)
			{
				return Color == other.Color;
			}
			return false;
		}

		public void Clear()
		{
			Type = 0;
			Light = 0;
			_extraData = 0;
		}

		public MapTile WithLight(byte light)
		{
			return new MapTile(Type, light, (byte)(_extraData | 0x80u));
		}

		public static MapTile Create(ushort type, byte light, byte color)
		{
			return new MapTile(type, light, (byte)(color | 0x80u));
		}
	}
}
