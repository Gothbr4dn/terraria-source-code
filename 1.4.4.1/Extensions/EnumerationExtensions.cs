using System;

namespace Extensions
{
	public static class EnumerationExtensions
	{
		private class _Value
		{
			private static Type _UInt64 = typeof(ulong);

			private static Type _UInt32 = typeof(long);

			public long? Signed;

			public ulong? Unsigned;

			public _Value(object value, Type type)
			{
				if (!type.IsEnum)
				{
					throw new ArgumentException("Value provided is not an enumerated type!");
				}
				Type underlyingType = Enum.GetUnderlyingType(type);
				if (underlyingType.Equals(_UInt32) || underlyingType.Equals(_UInt64))
				{
					Unsigned = Convert.ToUInt64(value);
				}
				else
				{
					Signed = Convert.ToInt64(value);
				}
			}
		}

		public static T Include<T>(this Enum value, T append)
		{
			Type type = value.GetType();
			object obj = value;
			_Value value2 = new _Value(append, type);
			if (value2.Signed is long)
			{
				obj = Convert.ToInt64(value) | value2.Signed.Value;
			}
			else if (value2.Unsigned is ulong)
			{
				obj = Convert.ToUInt64(value) | value2.Unsigned.Value;
			}
			return (T)Enum.Parse(type, obj.ToString());
		}

		public static T Remove<T>(this Enum value, T remove)
		{
			Type type = value.GetType();
			object obj = value;
			_Value value2 = new _Value(remove, type);
			if (value2.Signed is long)
			{
				obj = Convert.ToInt64(value) & ~value2.Signed.Value;
			}
			else if (value2.Unsigned is ulong)
			{
				obj = Convert.ToUInt64(value) & ~value2.Unsigned.Value;
			}
			return (T)Enum.Parse(type, obj.ToString());
		}

		public static bool Has<T>(this Enum value, T check)
		{
			Type type = value.GetType();
			_Value value2 = new _Value(check, type);
			if (value2.Signed is long)
			{
				return (Convert.ToInt64(value) & value2.Signed.Value) == value2.Signed.Value;
			}
			if (value2.Unsigned is ulong)
			{
				return (Convert.ToUInt64(value) & value2.Unsigned.Value) == value2.Unsigned.Value;
			}
			return false;
		}

		public static bool Missing<T>(this Enum obj, T value)
		{
			return !obj.Has(value);
		}
	}
}
