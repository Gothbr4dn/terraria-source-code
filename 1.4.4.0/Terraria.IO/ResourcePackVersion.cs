using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Terraria.IO
{
	[DebuggerDisplay("Version {Major}.{Minor}")]
	public struct ResourcePackVersion : IComparable, IComparable<ResourcePackVersion>
	{
		[JsonProperty("major")]
		public int Major { get; private set; }

		[JsonProperty("minor")]
		public int Minor { get; private set; }

		public static ResourcePackVersion Create(int major, int minor)
		{
			ResourcePackVersion result = default(ResourcePackVersion);
			result.Major = major;
			result.Minor = minor;
			return result;
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is ResourcePackVersion))
			{
				throw new ArgumentException("A RatingInformation object is required for comparison.", "obj");
			}
			return CompareTo((ResourcePackVersion)obj);
		}

		public int CompareTo(ResourcePackVersion other)
		{
			int num = Major.CompareTo(other.Major);
			if (num != 0)
			{
				return num;
			}
			return Minor.CompareTo(other.Minor);
		}

		public static bool operator ==(ResourcePackVersion lhs, ResourcePackVersion rhs)
		{
			return lhs.CompareTo(rhs) == 0;
		}

		public static bool operator !=(ResourcePackVersion lhs, ResourcePackVersion rhs)
		{
			return !(lhs == rhs);
		}

		public static bool operator <(ResourcePackVersion lhs, ResourcePackVersion rhs)
		{
			return lhs.CompareTo(rhs) < 0;
		}

		public static bool operator >(ResourcePackVersion lhs, ResourcePackVersion rhs)
		{
			return lhs.CompareTo(rhs) > 0;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ResourcePackVersion))
			{
				return false;
			}
			return CompareTo((ResourcePackVersion)obj) == 0;
		}

		public override int GetHashCode()
		{
			long num = Major;
			long num2 = Minor;
			return ((num << 32) | num2).GetHashCode();
		}

		public string GetFormattedVersion()
		{
			return Major + "." + Minor;
		}
	}
}
