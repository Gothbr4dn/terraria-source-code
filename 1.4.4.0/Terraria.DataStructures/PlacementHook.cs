using System;

namespace Terraria.DataStructures
{
	public struct PlacementHook
	{
		public Func<int, int, int, int, int, int, int> hook;

		public int badReturn;

		public int badResponse;

		public bool processedCoordinates;

		public static PlacementHook Empty = new PlacementHook(null, 0, 0, processedCoordinates: false);

		public const int Response_AllInvalid = 0;

		public PlacementHook(Func<int, int, int, int, int, int, int> hook, int badReturn, int badResponse, bool processedCoordinates)
		{
			this.hook = hook;
			this.badResponse = badResponse;
			this.badReturn = badReturn;
			this.processedCoordinates = processedCoordinates;
		}

		public static bool operator ==(PlacementHook first, PlacementHook second)
		{
			if (first.hook == second.hook && first.badResponse == second.badResponse && first.badReturn == second.badReturn)
			{
				return first.processedCoordinates == second.processedCoordinates;
			}
			return false;
		}

		public static bool operator !=(PlacementHook first, PlacementHook second)
		{
			if (!(first.hook != second.hook) && first.badResponse == second.badResponse && first.badReturn == second.badReturn)
			{
				return first.processedCoordinates != second.processedCoordinates;
			}
			return true;
		}

		public override bool Equals(object obj)
		{
			if (obj is PlacementHook)
			{
				return this == (PlacementHook)obj;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
